using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using WelcomeLibrary;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Web.SessionState;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Data.SQLite;
using System.Collections.Specialized;


//public class jreturncontaineritem
//{
//    public Mail mail { set; get; }
//    public Comment item { set; get; }
//    public List<Comment> list { set; get; }
//    public double totalemediastars { set; get; }
//    public long totaleapprovati { set; get; }
//    public long recordstotali { set; get; }
//    public List<simpleidname> reslist { set; get; }
//    public Dictionary<string, string> objfiltro { set; get; }
//}


public class HandlerGestioneprodotti : IHttpHandler, IRequiresSessionState
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

            string body = HandlerHelper.GetPostContent(context);
            Dictionary<string, string> pars = HandlerHelper.GetParamsJSON(body);

            //Dictionary<string, string> pars = parseparams(context);

            string ret = "";
            offerteDM offDM = new offerteDM();
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
                    initpagemodel inim = Newtonsoft.Json.JsonConvert.DeserializeObject<initpagemodel>(model);
                    inim.message = "";

                    //Status list da caricare da una chiave di risorsa per lingua come json key value serializzato
                    // iscrizioni aperte 0, quasi confermato 1, confermato 2, quasi completo 3, completo 4, in partenza 5, scaduto 6
                    string serializestatuslist = references.ResMan("Common", lingua, "statuslist");
                    OrderedDictionary statuslist = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderedDictionary>(serializestatuslist);
                    inim.statuslist = statuslist;

                    //etalist carico da risorse 
                    string serializeetalist = references.ResMan("Common", lingua, "etalist");
                    OrderedDictionary etalist = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderedDictionary>(serializeetalist);
                    inim.etalist = etalist;

                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(inim); // devo tornare il serializzato del model
                    break;
                case "filtercoordinatori":
                    initpagemodel inim1 = Newtonsoft.Json.JsonConvert.DeserializeObject<initpagemodel>(model);
                    OrderedDictionary coordlist = new OrderedDictionary();
                    inim1.message = "";
                    //FILTRO SUI COORDINATORI ////////////////////////////////
                    OfferteCollection coordinatorifiltro = offDM.CaricaOffertaPerTestourl(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, inim1.filterkey, "rif000005");//rif000005
                    //VARIANTE: oppure se vuoi predneerli dai clienti ...da faref filtro per testo sui clienti di una certa tipologia ....
                    if (coordinatorifiltro != null) //inim1.maxresults
                    {
                        List<Offerte> coordlimit = new List<Offerte>();
                        if (coordinatorifiltro.Count > inim1.maxresults)
                            coordlimit = coordinatorifiltro.GetRange(0, inim1.maxresults);
                        else
                            coordlimit.AddRange(coordinatorifiltro);
                        coordlimit.ForEach(c => coordlist.Add(c.Id, offDM.estraititolo(c, lingua)));
                    }
                    inim1.filteredautocomplete = coordlist;
                    /////////////////////////////////////////////////////////
                    ///
                    //torno i risultati di filtro
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(inim1); // devo tornare il serializzato del model
                    break;

                case "loaddata":
                    scaglionivuemodel scam = Newtonsoft.Json.JsonConvert.DeserializeObject<scaglionivuemodel>(model);

                    ////////////////////////////////////////
                    //Aggiorniamo lo stato degli scaglioni caricati per l'attività
                    ////////////////////////////////////////
                    Dictionary<string, string> parametri = new Dictionary<string, string>();
                    parametri["idprodotto"] = scam.idattivita.ToString();
                    ecDM.AggiornaStatoscaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parametri);
                    ////////////////////////////////////////

                    // da fare caricamento nel modello dei dati deli scaglioni per l'attivita selezionata
                    scam.message = "";
                    List<SQLiteParameter> parscaglioni = new List<SQLiteParameter>();
                    SQLiteParameter ps1 = new SQLiteParameter("@id_attivita", scam.idattivita);//OleDbType.VarChar
                    parscaglioni.Add(ps1);
                    if (scam.nascondiscaglionipassati) //se richiesto nascondo gli scaglioni passati
                    {
                        SQLiteParameter ps2 = new SQLiteParameter("@Data_inizio", System.DateTime.Now);//OleDbType.VarChar
                        parscaglioni.Add(ps2);
                    }

                    scam.list = offerteDM.CaricaOfferteScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parscaglioni, "");
#if FALSE

                    //PAGINAZIONE SE PRESENTE PAGINATORE IN PAGINA ( da chiamare settando la current page del paginatore nel modello )
                    scam.list = offDM.CaricaOfferteScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parscaglioni, "", scam.Pager.CurrentPage, scam.Pager.PageSize);
                    long nrecordfiltrati = scam.list.Totrecs;
                    scam.Pager.TotalRecords = ((long)scam.list.Totrecs);
                    scam.Pager.GeneratePages();
                    if (nrecordfiltrati == 0) scam.Pager.CurrentPage = 1; 
#endif

                    //////////////////////////////////////////////////////////
                    //creiamo anche una collection con i coordinatori id,nome per la visualizzazione
                    //prendendo solo quelli persenti nei risultati trovati
                    //////////////////////////////////////////////////////
                    OrderedDictionary coordlist1 = new OrderedDictionary();
                    string idlistcoord = "";
                    scam.list.ForEach(s => idlistcoord += s.idcoordinatore + ",");
                    idlistcoord = idlistcoord.TrimEnd(',');
                    OfferteCollection presentcoords = offDM.GetOffertebyidlist(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idlistcoord, lingua, "rif000005");
                    presentcoords.ForEach(c => coordlist1.Add(c.Id, offDM.estraititolo(c, lingua)));
                    scam.coordinatoriinlist = coordlist1;
                    //////////////////////////////////////////////////////////

                    Scaglioni empyitem = new Scaglioni();
                    empyitem.nmax = 15;
                    empyitem.nconferma = 10;
                    scam.itemselected = scam.list.Exists(i => i.id == scam.idscaglione) ? scam.list.Find(i => i.id == scam.idscaglione) : empyitem;

                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(scam);// devo tornare il serializzato del model
                    break;
             
                case "deletedata":
                    scaglionivuemodel scam1 = Newtonsoft.Json.JsonConvert.DeserializeObject<scaglionivuemodel>(model);
                    // da fare caricamento nel modello dei dati deli scaglioni per l'attivita selezionata
                    scam1.message = "";
                    if (scam1.idscaglione != 0)
                    {
                        scam1.message = "cancellazione eseguita";
                        string esito = offerteDM.CancellaScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, scam1.idscaglione);
                        if (!string.IsNullOrEmpty(esito))
                            scam1.message = esito;
                        else scam1.idscaglione = 0; //Deselezione se cancellato
                        //Aggiorniamo i dati
                        List<SQLiteParameter> pardel = new List<SQLiteParameter>();
                        SQLiteParameter psd1 = new SQLiteParameter("@id_attivita", scam1.idattivita);//OleDbType.VarChar
                        pardel.Add(psd1);
                        if (scam1.nascondiscaglionipassati) //se richiesto nascondo gli scaglioni passati
                        {
                            SQLiteParameter ps2 = new SQLiteParameter("@Data_inizio", System.DateTime.Now);//OleDbType.VarChar
                            pardel.Add(ps2);
                        }
                        scam1.list = offerteDM.CaricaOfferteScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, pardel);
                        Scaglioni empyitem1 = new Scaglioni();
                        empyitem1.nmax = 15;
                        empyitem1.nconferma = 10;
                        scam1.itemselected = scam1.list.Exists(i => i.id == scam1.idscaglione) ? scam1.list.Find(i => i.id == scam1.idscaglione) : empyitem1;

#if FALSE

                    //PAGINAZIONE SE PRESENTE PAGINATORE IN PAGINA ( da chiamare settando la current page del paginatore nel modello )
                    scam1.list = offerteDM.CaricaOfferteScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parscaglioni, "", scam1.Pager.CurrentPage, scam1.Pager.PageSize);
                    long nrecordfiltrati = scam1.list.Totrecs;
                    scam1.Pager.TotalRecords = ((long)scam1.list.Totrecs);
                    scam1.Pager.GeneratePages();
                    if (nrecordfiltrati == 0) scam1.Pager.CurrentPage = 1; 
#endif
                    }

                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(scam1);// devo tornare il serializzato del model
                    break;
                case "inserisciaggiorna":
                    scaglionivuemodel scam2 = Newtonsoft.Json.JsonConvert.DeserializeObject<scaglionivuemodel>(model);
                    scam2.message = "";
                    scam2.itemselected.id_attivita = scam2.idattivita;

                    //FARE VERIFICHE E AGGIONAMENTO O INSERIMENTO VOCE SCAGLIONI
                    offerteDM.InserisciAggiornaScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, scam2.itemselected);
                    scam2.idscaglione = scam2.itemselected.id;

                    ////////////////////////////////////////
                    //Aggiorniamo lo stato dello scaglione appena modificato
                    ////////////////////////////////////////
                    Dictionary<string, string> parametri1 = new Dictionary<string, string>();
                    parametri1["idscaglione"] = scam2.idscaglione.ToString();
                    ecDM.AggiornaStatoscaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parametri1);
                    ////////////////////////////////////////

                    // da fare caricamento nel modello dei dati deli scaglioni per l'attivita selezionata
                    List<SQLiteParameter> parscaglioniupdate = new List<SQLiteParameter>();
                    SQLiteParameter pu1 = new SQLiteParameter("@id_attivita", scam2.idattivita);//OleDbType.VarChar
                    parscaglioniupdate.Add(pu1);
                    if (scam2.nascondiscaglionipassati) //se richiesto nascondo gli scaglioni passati
                    {
                        SQLiteParameter ps2 = new SQLiteParameter("@Data_inizio", System.DateTime.Now);//OleDbType.VarChar
                        parscaglioniupdate.Add(ps2);
                    }

                    scam2.list = offerteDM.CaricaOfferteScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parscaglioniupdate);
                    scam2.itemselected = scam2.list.Exists(i => i.id == scam2.idscaglione) ? scam2.list.Find(i => i.id == scam2.idscaglione) : new Scaglioni();

#if FALSE

                    //PAGINAZIONE SE PRESENTE PAGINATORE IN PAGINA ( da chiamare settando la current page del paginatore nel modello )
                    scam2.list = offerteDM.CaricaOfferteScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parscaglioni, "", scam2.Pager.CurrentPage, scam2.Pager.PageSize);
                    long nrecordfiltrati = scam2.list.Totrecs;
                    scam2.Pager.TotalRecords = ((long)scam2.list.Totrecs);
                    scam2.Pager.GeneratePages();
                    if (nrecordfiltrati == 0) scam2.Pager.CurrentPage = 1; 
#endif


                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(scam2);// devo tornare il serializzato del model

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

public class initpagemodel
{
    public string message = "";
    public string messageResp = "";
    public OrderedDictionary statuslist = new OrderedDictionary();
    public OrderedDictionary etalist = new OrderedDictionary();
    public int maxresults = 50;
    public bool isOpen = false;
    public string filterkey = string.Empty;
    public string selectedkey = string.Empty;
    public OrderedDictionary filteredautocomplete = new OrderedDictionary();

}

public class scaglionivuemodel
{
    public long idattivita = 0;
    public long idscaglione = 0;
    public string message = "";
    public string messageResp = "";
    public bool nascondiscaglionipassati = true;
    public PagerModel Pager = new PagerModel(1, 20, 0);
    public ScaglioniCollection list = new ScaglioniCollection();
    public Scaglioni itemselected = new Scaglioni();
    public OrderedDictionary coordinatoriinlist = new OrderedDictionary();

    public scaglionivuemodel()
    {
        //valori default
        //this.itemselected.nconferma = 10;
        //this.itemselected.nmax = 15;
    }
}
