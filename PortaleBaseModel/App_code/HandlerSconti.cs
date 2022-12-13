using System;
using System.Web;
using System.Collections.Generic;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Web.SessionState;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Data.SQLite;

public class HandlerSconti : IHttpHandler, IRequiresSessionState
{
    public Dictionary<string, string> parseparams(HttpContext context)
    {
        Dictionary<string, string> pars = new Dictionary<string, string>();
        bool isPost = false;
        isPost = context.Request.HttpMethod.ToUpper() == "POST";
        bool ismultipart = false;
        if (context.Request.ContentType.ToLower().Contains("multipart/form-data")) ismultipart = true;

        if (isPost && !ismultipart)
            pars = HandlerHelper.GetPostParams(context);
        foreach (var item in context.Request.Params.Keys)
        {
            string szKey = item.ToString();
            if (!pars.ContainsKey(szKey))
                pars.Add(szKey, context.Request.Params[szKey].ToString());
        }

        return pars;
    }


    public void ProcessRequest(HttpContext context)
    {
        string result = "";
        context.Response.ContentType = "text/plain";
        try
        {
            eCommerceDM ecDM = new eCommerceDM();
            Codicesconto empyitem = new Codicesconto();
            //empyitem.Usosingolo = true; //presetto la selezione usosingolo

            //usermanager USM = new usermanager();
            Tabrif utente = new Tabrif();
            string body = HandlerHelper.GetPostContent(context);
            Dictionary<string, string> pars = HandlerHelper.GetParamsJSON(body);
            scontivuemodel _tmpscontivm = new scontivuemodel();

            //Dictionary<string, string> pars = parseparams(context);

            string ret = "";
            string lingua = pars.ContainsKey("lng") ? pars["lng"] : "I";
            string q = pars.ContainsKey("q") ? pars["q"] : "";
            string model = pars.ContainsKey("model") ? pars["model"] : "";

            //string objfiltro = pars.ContainsKey("objfiltro") ? pars["objfiltro"] : "";
            //string sitem = pars.ContainsKey("item") ? pars["item"] : "";
            //Dictionary<string, string> filtri = new Dictionary<string, string>();
            //if (objfiltro != "" && objfiltro != null)
            //    filtri = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
            //string idpost = filtri.ContainsKey("id") ? filtri["id"] : "";
            //string smail = pars.ContainsKey("mail") ? pars["mail"] : "";
            //Mail mail = Newtonsoft.Json.JsonConvert.DeserializeObject<Mail>(smail);
            //Comment item = Newtonsoft.Json.JsonConvert.DeserializeObject<Comment>(sitem);
            //string spage = filtri.ContainsKey("page") ? filtri["page"] : "1";
            //string spagesize = filtri.ContainsKey("pagesize") ? filtri["pagesize"] : "12";
            //string enablepager = filtri.ContainsKey("enablepager") ? filtri["enablepager"] : "false";
            //long page = 0;
            //long pagesize = 0;
            //if (enablepager == "true")
            //{
            //    long.TryParse(spage, out page);
            //    long.TryParse(spagesize, out pagesize);
            //}
            //string maxrecord = filtri.ContainsKey("maxrecord") ? filtri["maxrecord"] : "";


            switch (q)
            {
                case "initpage":
                    //Modello con dati di gestione comuni per la pagina
                    initpagemodelsconti inim = Newtonsoft.Json.JsonConvert.DeserializeObject<initpagemodelsconti>(model);
                    inim.message = "";

                    //INIZIALIZZO EVENTUALI LISTE USATE NEL MODELLO DI PAGINA ....
                    List<Prodotto> listprod = WelcomeLibrary.UF.Utility.ElencoProdotti.FindAll(p => p.CodiceTipologia == "rif000001" && p.Lingua == lingua);
                    List<SProdotto> listsprod = WelcomeLibrary.UF.Utility.ElencoSottoProdotti.FindAll(p => p.Lingua == lingua && p.CodiceProdotto == inim.selectedcategoria);
                    List<Tabrif> listcar1 = WelcomeLibrary.UF.Utility.Caratteristiche[0].FindAll(p => p.Lingua == lingua);

                    inim.categoria = new OrderedDictionary();
                    inim.categoria.Add("", references.ResMan("Common", lingua, "selProdotti"));
                    if (listprod != null)
                        listprod.ForEach(p => inim.categoria.Add(p.CodiceProdotto, p.Descrizione));

                    inim.sottocategoria = new OrderedDictionary();
                    inim.sottocategoria.Add("", references.ResMan("Common", lingua, "selSProdotti"));
                    if (listsprod != null)
                    {
                        inim.selectedsottocategoria = (listsprod.Exists(s => s.CodiceSProdotto == inim.selectedsottocategoria)) ? inim.selectedsottocategoria : "";
                        listsprod.ForEach(p => inim.sottocategoria.Add(p.CodiceSProdotto, p.Descrizione));
                    }

                    inim.caratteristica1 = new OrderedDictionary();
                    inim.caratteristica1.Add("", references.ResMan("Common", lingua, "selcaratteristica1"));
                    if (listcar1 != null)
                        listcar1.ForEach(p => inim.caratteristica1.Add(p.Codice, p.Campo1));


                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(inim); // devo tornare il serializzato del model di pagina con i valori riempiti che mi servono
                    break;
                case "filterclienti": //filtro autocomplete clienti!!! ( da vedere se serve )
                    initpagemodelsconti inim1 = Newtonsoft.Json.JsonConvert.DeserializeObject<initpagemodelsconti>(model);
                    OrderedDictionary resultlist = new OrderedDictionary();
                    inim1.message = "";
                    WelcomeLibrary.DAL.ClientiDM cliDM = new ClientiDM();
                    //FILTRO   ////////////////////////////////
                    ClienteCollection resultfiltered = cliDM.GetLista("%" + inim1.filterkey + "%", WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                    if (resultfiltered != null) //inim1.maxresults
                    {
                        List<Cliente> reslutslimited = new List<Cliente>();
                        if (resultfiltered.Count > inim1.maxresults)
                            reslutslimited = resultfiltered.GetRange(0, inim1.maxresults);
                        else
                            reslutslimited.AddRange(resultfiltered);
                        reslutslimited.ForEach(c => resultlist.Add(c.Id_cliente, c.Spare3));
                    }
                    inim1.filteredautocomplete = resultlist;
                    /////////////////////////////////////////////////////////

                    //torno i risultati di filtro
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(inim1); // devo tornare il serializzato del model
                    break;
                case "filterprodotti": //filtro autocomplete clienti!!! ( da vedere se serve )
                    initpagemodelsconti inim2 = Newtonsoft.Json.JsonConvert.DeserializeObject<initpagemodelsconti>(model);
                    OrderedDictionary resultlist1 = new OrderedDictionary();
                    inim2.message = "";

                    WelcomeLibrary.DAL.offerteDM offDM = new offerteDM();
                    //FILTRO   ////////////////////////////////
                    OfferteCollection resultfiltered1 = offDM.GetLista("%" + inim2.filterkey1 + "%", inim2.maxresults.ToString(), lingua, WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "rif000001");
                    if (resultfiltered1 != null) //inim2.maxresults
                    {
                        resultfiltered1.ForEach(c => resultlist1.Add(c.Id, c.DenominazionebyLingua(lingua)));
                    }

                    inim2.filteredautocomplete1 = resultlist1;

                    //torno i risultati di filtro
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(inim2); // devo tornare il serializzato del model
                    break;
                case "loaddata":
                    scontivuemodel scontivm = Newtonsoft.Json.JsonConvert.DeserializeObject<scontivuemodel>(model);
                    // da fare caricamento nel modello dei dati sconti
                    scontivm.message = "";

                    //filtro sconti  
                    Codicesconto _localfilter = new Codicesconto(scontivm.filterparams);
                    if (!string.IsNullOrEmpty(_localfilter.Codicifiltro.Trim())) //modificatore per filtro codici categoria
                        _localfilter.Codicifiltro = "%" + _localfilter.Codicifiltro.Trim() + "%"; //filtro esteso codici


                    if (!string.IsNullOrEmpty(_localfilter.caratteristica1filtro.Trim())) //modificatore per filtro codici caratteristica1
                        _localfilter.caratteristica1filtro = "%" + _localfilter.caratteristica1filtro.Trim() + "%"; //filtro esteso caratteristica1

                    //List<SQLiteParameter> parsconti = new List<SQLiteParameter>();
                    //SQLiteParameter ps1 = new SQLiteParameter("@Id_cliente", clim.filterparams.xxxxxxx);//OleDbType.VarChar
                    //parsconti.Add(ps1);
                    //scontivm.filterparams.Codicifiltro  = ""; //vanno passati dai valori dei filtri presenti in initpagemodelsconti al momento del click di filtro inserendoli in  scontivm.filterparams e nella lista parametri di filtro parsconti
                    scontivm.list = ecDM.CaricaListaSconti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _localfilter, scontivm.Pager.CurrentPage, scontivm.Pager.PageSize);

                    long nrecordfiltrati = scontivm.list.Totrecs;
                    scontivm.Pager.TotalRecords = ((long)scontivm.list.Totrecs);
                    scontivm.Pager.GeneratePages();
                    if (nrecordfiltrati == 0) scontivm.Pager.CurrentPage = 1;
                    scontivm.itemselected = scontivm.list.Exists(i => i.Id == scontivm.idselected) ? scontivm.list.Find(i => i.Id == scontivm.idselected) : empyitem;
                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(scontivm);// devo tornare il serializzato del model
                    break;

                case "deletedata":
                    scontivuemodel scontivm1 = Newtonsoft.Json.JsonConvert.DeserializeObject<scontivuemodel>(model);

                    scontivm1.message = "";
                    if (scontivm1.idselected != 0)
                    {
                        scontivm1.message = "cancellazione eseguita";
                        string esito = ecDM.CancellaSconto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, scontivm1.idselected);
                        if (!string.IsNullOrEmpty(esito))
                            scontivm1.message = esito;
                        else scontivm1.idselected = 0; //Deselezione se cancellato

                        scontivm1.list = ecDM.CaricaListaSconti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, scontivm1.filterparams, scontivm1.Pager.CurrentPage, scontivm1.Pager.PageSize);//Passando null -> prende tutti i clienti, inoltre baypasso il filtro 
                        long nrecordfiltrati1 = scontivm1.list.Totrecs;
                        scontivm1.Pager.TotalRecords = ((long)scontivm1.list.Totrecs);
                        scontivm1.Pager.GeneratePages();
                        if (nrecordfiltrati1 == 0) scontivm1.Pager.CurrentPage = 1;

                        scontivm1.itemselected = scontivm1.list.Exists(i => i.Id == scontivm1.idselected) ? scontivm1.list.Find(i => i.Id == scontivm1.idselected) : empyitem;
                    }
                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(scontivm1);// devo tornare il serializzato del model
                    break;
                case "inserisciaggiorna":

                    scontivuemodel scontivm2 = Newtonsoft.Json.JsonConvert.DeserializeObject<scontivuemodel>(model);
                    scontivm2.message = "";
                    //scontivm2.idselected = scontivm2.itemselected.Id;
                    try
                    {
                        //FARE VERIFICHE E AGGIONAMENTO O INSERIMENTO codice sconto
                        bool validadati = true;

                        scontivm2.itemselected.Testocodicesconto = scontivm2.itemselected.Testocodicesconto.Trim();
                        if (scontivm2.itemselected.Testocodicesconto.Trim() == string.Empty)
                        {
                            scontivm2.message += "Inserire Codice Sconto ";
                            validadati = false;
                        }
                        if ((scontivm2.itemselected.Scontonum == null && scontivm2.itemselected.Scontoperc == null))
                        {
                            scontivm2.message += "Inserire Importo o percentuale di Sconto";
                            validadati = false;
                        }
                        else if ((scontivm2.itemselected.Scontonum != null && scontivm2.itemselected.Scontonum != 0))
                            if ((scontivm2.itemselected.Scontoperc != null && scontivm2.itemselected.Scontoperc != 0))
                            {
                                scontivm2.message += "Inserire solo un valore di sconto, importo € oppure importo %, impossibile assegnare entrambi";
                                validadati = false;
                            }

                        if (scontivm2.itemselected.Codicifiltro != null)
                            scontivm2.itemselected.Codicifiltro = scontivm2.itemselected.Codicifiltro.Trim();

                        if (scontivm2.itemselected.caratteristica1filtro != null)
                            scontivm2.itemselected.caratteristica1filtro = scontivm2.itemselected.caratteristica1filtro.Trim();

                        if (validadati)
                        {
                            ecDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, scontivm2.itemselected);

                            scontivm2.idselected = scontivm2.itemselected.Id;
                            scontivm2.list = ecDM.CaricaListaSconti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, scontivm2.filterparams, scontivm2.Pager.CurrentPage, scontivm2.Pager.PageSize);//Passando null -> prende tutti i clienti, inoltre baypasso il filtro 
                            long nrecordfiltrati2 = scontivm2.list.Totrecs;
                            scontivm2.Pager.TotalRecords = ((long)scontivm2.list.Totrecs);
                            scontivm2.Pager.GeneratePages();
                            if (nrecordfiltrati2 == 0) scontivm2.Pager.CurrentPage = 1;
                            scontivm2.itemselected = scontivm2.list.Exists(i => i.Id == scontivm2.idselected) ? scontivm2.list.Find(i => i.Id == scontivm2.idselected) : empyitem;

                        }
                    }
                    catch (Exception error)
                    {
                        scontivm2.message = error.Message;
                    }

                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(scontivm2);// devo tornare il serializzato del model

                    break;

            }
        }
        catch (Exception ex)
        {
            string er = ex.Message;
            if (ex.InnerException != null)
                er += ex.InnerException.Message.ToString();
            result = er;
            context.Response.StatusCode = 400;
        }
        context.Response.Write(result);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}



public class initpagemodelsconti
{
    public string message = "";
    public string messageResp = "";

    //public OrderedDictionary clientilist = new OrderedDictionary();
    //public OrderedDictionary prodottilist = new OrderedDictionary();

    public bool chkvediscaduti = false;

    public int maxresults = 50;
    //public string selectedkey = string.Empty;
    public bool isOpen = false;
    public string filterkey = string.Empty;
    public OrderedDictionary filteredautocomplete = new OrderedDictionary();

    public bool isOpen1 = false;
    public string filterkey1 = string.Empty;
    public OrderedDictionary filteredautocomplete1 = new OrderedDictionary();

    public OrderedDictionary categoria = new OrderedDictionary();
    public OrderedDictionary sottocategoria = new OrderedDictionary();
    public string selectedcategoria = "";
    public string selectedsottocategoria = "";

    public OrderedDictionary caratteristica1 = new OrderedDictionary();
    public string selectedcaratteristica1 = "";


}

public class scontivuemodel
{
    public string message = "";
    public string messageResp = "";
    public PagerModel Pager = new PagerModel(1, 20, 0);

    //public long idattivita = 0;
    public long idselected = 0;
    public CodicescontoList list = new CodicescontoList();
    public Codicesconto itemselected = new Codicesconto();
    public Codicesconto filterparams = new Codicesconto();
    //public Tabrif utente = new Tabrif();

    public scontivuemodel()
    {
        //filterparams.Datascadenza = System.DateTime.Now();

    }
}
