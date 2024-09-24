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


public class HandlerGestioneclienti : IHttpHandler, IRequiresSessionState
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
            ClientiDM cliDM = new ClientiDM();
            Cliente empyitem = new Cliente();
            empyitem.addvalues = new Cliente();//IMPORTANTE INIZIALIZZ PER DATI SPEDIZ COLLEGATI
            usermanager USM = new usermanager();
            Tabrif utente = new Tabrif();
            string body = HandlerHelper.GetPostContent(context);
            Dictionary<string, string> pars = HandlerHelper.GetParamsJSON(body);
            clientivuemodel _tmpclim = new clientivuemodel();

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
                    initpagemodelclienti inim = Newtonsoft.Json.JsonConvert.DeserializeObject<initpagemodelclienti>(model);
                    inim.message = "";

                    List<Tabrif> tipiclienti = Utility.TipiClienti.FindAll(delegate (Tabrif _t) { return _t.Lingua == lingua; });
                    OrderedDictionary tipiclientilist = new OrderedDictionary();
                    tipiclientilist.Add("", "Seleziona Tipo Cliente");
                    foreach (Tabrif t in tipiclienti)
                        tipiclientilist.Add(t.Codice, t.Campo1);
                    inim.tipiclientilist = tipiclientilist;

                    List<Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == lingua; });
                    nazioni.Sort(new GenericComparer<Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));

                    OrderedDictionary nazionilist = new OrderedDictionary();
                    nazionilist.Add("", "Seleziona nazione");
                    foreach (Tabrif t in nazioni)
                        nazionilist.Add(t.Codice, t.Campo1);
                    inim.nazionilist = nazionilist;

                    string sgenerelist = references.ResMan("Common", lingua, "generelist");
                    OrderedDictionary serializegenerelist = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderedDictionary>(sgenerelist);
                    inim.generelist = serializegenerelist;

                    var filejsonlanguages = "languages.json";
                    //reflanguages = System.IO.File.ReadAllText(Server.MapPath("~/lib/cfg/" + filejsonlanguages));
                    var reflanguages = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\cfg\\" + filejsonlanguages).Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                    OrderedDictionary linguelist = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderedDictionary>(reflanguages);
                    inim.languageslist.Add("", "Seleziona lingua");
                    foreach (System.Collections.DictionaryEntry item in linguelist)
                        inim.languageslist.Add(item.Key, item.Value);


                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(inim); // devo tornare il serializzato del model
                    break;
                case "filterclienti":
                    initpagemodelclienti inim1 = Newtonsoft.Json.JsonConvert.DeserializeObject<initpagemodelclienti>(model);
                    OrderedDictionary resultlist = new OrderedDictionary();
                    inim1.message = "";

                    //FILTRO SUI CLIENTI ////////////////////////////////
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

                case "loaddata":
                    clientivuemodel clim = Newtonsoft.Json.JsonConvert.DeserializeObject<clientivuemodel>(model);
                    // da fare caricamento nel modello dei dati deli clienti per l'attivita selezionata
                    clim.message = "";
                    //List<SQLiteParameter> parclienti = new List<SQLiteParameter>();
                    //SQLiteParameter ps1 = new SQLiteParameter("@Id_cliente", clim.filterparams.Id_cliente);//OleDbType.VarChar
                    //parclienti.Add(ps1);
                    //SQLiteParameter ps2 = new SQLiteParameter("@id_tipi_clienti", clim.filterparams.id_tipi_clienti);//OleDbType.VarChar
                    //parclienti.Add(ps2);
                    //SQLiteParameter ps3 = new SQLiteParameter("@codnaz", clim.filterparams.CodiceNAZIONE);//OleDbType.VarChar
                    //parclienti.Add(ps3);


                    //clim.filterparams.CodiceNAZIONE = ""; //vanno passati dai valori dei filtri in initpagemodelclienti al momento del click di filtro inserendoli in  clim.filterparams.
                    //clim.filterparams.id_tipi_clienti = "";//vanno passati dai valori dei filtri in initpagemodelclienti al momento del click di filtro inserendoli in  clim.filterparams.
                    //clim.filterparams.Lingua = "";//visualizza sempre i clienti in tutte le lingue
                    clim.list = cliDM.CaricaClientiFiltrati(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, clim.filterparams, true, clim.Pager.CurrentPage, clim.Pager.PageSize);//Passando null -> prende tutti i clienti, inoltre baypasso il filtro 

                    long nrecordfiltrati = clim.list.Totrecs;
                    clim.Pager.TotalRecords = ((long)clim.list.Totrecs);
                    clim.Pager.GeneratePages();
                    if (nrecordfiltrati == 0) clim.Pager.CurrentPage = 1;

                    clim.itemselected = clim.list.Exists(i => i.Id_cliente == clim.idselected) ? clim.list.Find(i => i.Id_cliente == clim.idselected) : empyitem;
                    //DATI UTENTE ASSOCIATO SE PRESENTE
                    clim.utente.Id = clim.itemselected.Id_cliente.ToString();
                    clim.utente.Campo1 = USM.GetUsernamebycamporofilo("idCliente", clim.itemselected.Id_cliente.ToString());

                    //precarico le liste geo per la visualizzazione di dettaglio in gelist1 in base al cliente selezionato
                    references.caricadatiddlgeo(clim.geolist1, clim.itemselected.CodiceREGIONE, clim.itemselected.CodicePROVINCIA, clim.itemselected.CodiceCOMUNE, clim.itemselected.CodiceNAZIONE, lingua);
                    references.caricadatiddlgeo(clim.geolist2, clim.itemselected.addvalues.CodiceREGIONE, clim.itemselected.addvalues.CodicePROVINCIA, clim.itemselected.addvalues.CodiceCOMUNE, clim.itemselected.addvalues.CodiceNAZIONE, lingua);

                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(clim);// devo tornare il serializzato del model
                    break;
                case "cambiopassword":
                    if (!string.IsNullOrEmpty(model))
                        utente = Newtonsoft.Json.JsonConvert.DeserializeObject<Tabrif>(model);

                    if (utente != null && !string.IsNullOrEmpty(utente.Campo1) && !string.IsNullOrEmpty(utente.Campo2))
                    {
                        //string username = USM.GetUsernamebycamporofilo("idCliente", idcliente);
                        string resetpass = USM.Resetpassword(utente.Campo1);
                        string msgrsp = USM.Cambiopassword(utente.Campo1, resetpass, utente.Campo2);
                        utente.Campo3 = msgrsp.ToLower();
                    }
                    else
                        utente.Campo3 = "Inserire la nuova pass!";

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(utente);
                    break;
                case "generautente":
                    utente = new Tabrif();
                    if (!string.IsNullOrEmpty(model))
                        utente = Newtonsoft.Json.JsonConvert.DeserializeObject<Tabrif>(model);
                    if (utente != null && !string.IsNullOrEmpty(utente.Id))
                    {
                        empyitem = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, utente.Id); //Ricarico il cliente completo dal db
                        if (empyitem != null && empyitem.Id_cliente != 0)
                        {
                            string username = USM.GetUsernamebycamporofilo("idCliente", empyitem.Id_cliente.ToString());
                            if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(empyitem.Email))
                            {
                                string password = "";
                                username = empyitem.Id_cliente.ToString() + "-" + empyitem.Email;
                                USM.CreaUtente(empyitem.Id_cliente.ToString(), ref username, ref password, "Operatore");
                                utente.Campo2 = password;
                                utente.Campo1 = username;
                                utente.Campo3 = "Utente creato";

                            }
                            else
                            utente.Campo3 = "Utente già esistente -> Username: " + username;
                        }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(utente);
                    break;
                case "eliminautente":
                    utente = new Tabrif();
                    if (!string.IsNullOrEmpty(model))
                        utente = Newtonsoft.Json.JsonConvert.DeserializeObject<Tabrif>(model);
                    string username1 = USM.GetUsernamebycamporofilo("idCliente", utente.Id);
                    if (USM.EliminaUtentebyUsername(username1))
                    {
                        utente.Campo3 = "Eliminato Utente";
                        utente.Campo1 = "";
                        utente.Campo2 = "";
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(utente);
                    break;
                case "caricaddlregione":
                    //model valore passato
                    references.caricadatiddlgeo(_tmpclim.geolist1, "", "", "", model, lingua);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(_tmpclim.geolist1.ListRegione);
                    break;
                case "caricaddlprovincia":
                    //model
                    references.caricadatiddlgeo(_tmpclim.geolist1, model, "", "", "IT", lingua);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(_tmpclim.geolist1.ListProvincia);
                    break;
                case "caricaddlcomune":
                    //model
                    references.caricadatiddlgeo(_tmpclim.geolist1, "", model, "", "IT", lingua);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(_tmpclim.geolist1.ListComune);
                    break;
                case "deletedata":
                    clientivuemodel clim1 = Newtonsoft.Json.JsonConvert.DeserializeObject<clientivuemodel>(model);
                    // da fare caricamento nel modello dei dati deli scaglioni per l'attivita selezionata
                    clim1.message = "";
                    if (clim1.idselected != 0)
                    {
                        clim1.message = "cancellazione eseguita";
                        string esito = cliDM.CancellaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, clim1.idselected);
                        if (!string.IsNullOrEmpty(esito))
                            clim1.message = esito;
                        else clim1.idselected = 0; //Deselezione se cancellato


                        //clim1.filterparams.Lingua = "";//visualizza sempre i clienti in tutte le lingue
                        clim1.list = cliDM.CaricaClientiFiltrati(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, clim1.filterparams, true, clim1.Pager.CurrentPage, clim1.Pager.PageSize);//Passando null -> prende tutti i clienti, inoltre baypasso il filtro 
                        long nrecordfiltrati1 = clim1.list.Totrecs;
                        clim1.Pager.TotalRecords = ((long)clim1.list.Totrecs);
                        clim1.Pager.GeneratePages();
                        if (nrecordfiltrati1 == 0) clim1.Pager.CurrentPage = 1;

                        clim1.itemselected = clim1.list.Exists(i => i.Id_cliente == clim1.idselected) ? clim1.list.Find(i => i.Id_cliente == clim1.idselected) : empyitem;
                        //precarico le liste geo per la visualizzazione di dettaglio in gelist1 in base al cliente selezionato
                        references.caricadatiddlgeo(clim1.geolist1, clim1.itemselected.CodiceREGIONE, clim1.itemselected.CodicePROVINCIA, clim1.itemselected.CodiceCOMUNE, clim1.itemselected.CodiceNAZIONE, lingua);
                        references.caricadatiddlgeo(clim1.geolist2, clim1.itemselected.addvalues.CodiceREGIONE, clim1.itemselected.addvalues.CodicePROVINCIA, clim1.itemselected.addvalues.CodiceCOMUNE, clim1.itemselected.addvalues.CodiceNAZIONE, lingua);
                    }
                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(clim1);// devo tornare il serializzato del model
                    break;
                case "inserisciaggiorna":
                    clientivuemodel clim2 = Newtonsoft.Json.JsonConvert.DeserializeObject<clientivuemodel>(model);
                    clim2.message = "";
                    //clim2.idselected = clim2.itemselected.Id_cliente;

                    //FARE VERIFICHE E AGGIONAMENTO O INSERIMENTO CLIENTE
                    bool validadati = true;
                    clim2.itemselected.id_tipi_clienti = clim2.itemselected.id_tipi_clienti.Trim();
                    if (clim2.itemselected.id_tipi_clienti.Trim() == string.Empty)
                    {
                        clim2.message += "Inserire Tipologia Cliente";
                        validadati = false;
                    }
                    clim2.itemselected.Email = clim2.itemselected.Email.Trim();
                    if (clim2.itemselected.Email.Trim() == string.Empty)
                    {
                        clim2.message += "Inserire Email";
                        validadati = false;
                    }
                    clim2.itemselected.Nome = clim2.itemselected.Nome.Trim();
                    clim2.itemselected.Cognome = clim2.itemselected.Cognome.Trim();
                    clim2.itemselected.CodiceNAZIONE = clim2.itemselected.CodiceNAZIONE.Trim();
                    clim2.itemselected.CodiceREGIONE = clim2.itemselected.CodiceREGIONE.Trim();
                    clim2.itemselected.CodicePROVINCIA = clim2.itemselected.CodicePROVINCIA.Trim();
                    clim2.itemselected.CodiceCOMUNE = clim2.itemselected.CodiceCOMUNE.Trim();
                    clim2.itemselected.Cap = clim2.itemselected.Cap.Trim();
                    clim2.itemselected.Indirizzo = clim2.itemselected.Indirizzo.Trim();
                    clim2.itemselected.Telefono = clim2.itemselected.Telefono.Trim();
                    clim2.itemselected.Emailpec = clim2.itemselected.Emailpec.Trim();
                    clim2.itemselected.Pivacf = clim2.itemselected.Pivacf.Trim();
                    clim2.itemselected.Professione = clim2.itemselected.Professione.Trim();
                    clim2.itemselected.Codicisconto = clim2.itemselected.Codicisconto.Trim();
                    clim2.itemselected.Spare2 = clim2.itemselected.Spare2.Trim();
                    clim2.itemselected.Spare1 = clim2.itemselected.Spare1.Trim();
                    //Riserializzo i dati aggiuntivi se modificati
                    clim2.itemselected.Serialized = Newtonsoft.Json.JsonConvert.SerializeObject(clim2.itemselected.addvalues, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    try
                    {
                        //Controlliamo eventuale duplicazione di codici sconto
                        Dictionary<string, double> dict = ClientiDM.SplitCodiciSconto(clim2.itemselected.Codicisconto);
                        if (dict != null)
                        {
                            foreach (string csconto in dict.Keys)
                            {
                                //Vediamo se duplicata
                                Cliente cliduplicato = cliDM.CaricaClientePerCodicesconto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, csconto);
                                if (cliduplicato != null && cliduplicato.Id_cliente != 0 && cliduplicato.Id_cliente != clim2.itemselected.Id_cliente)
                                {
                                    clim2.message += "Modificare codice sconto inserito. Presente cliente con codice sconto uguale , id cliente :" + cliduplicato.Id_cliente.ToString() + " Non consentiti clienti con codici sconto uguali <br/>";
                                    validadati = false;
                                }
                            }
                        }
                        //Controllo nel dbEMAIL per coincidenze!!!!
                        if (clim2.itemselected.Email.ToLower().Trim() != "")
                        {
                            // ClientiCollection duplicati = cliDM.ControllaDuplicazioneDatiCliente(cli);
                            Cliente duplicato = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, clim2.itemselected.Email.ToLower().Trim(), clim2.itemselected.id_tipi_clienti);
                            if (duplicato != null && duplicato.Id_cliente != 0 && duplicato.Id_cliente != clim2.itemselected.Id_cliente && duplicato.id_tipi_clienti == clim2.itemselected.id_tipi_clienti)
                            {
                                clim2.message += "Presenti clienti con email coincidente : <br/> ";
                                validadati = false;
                            }
                        }
                        else
                        {
                            clim2.message += "Inserire EMAIL cliente <br/> ";
                            validadati = false;
                        }
                    }
                    catch (Exception error)
                    {
                        clim2.message = error.Message;
                        validadati = false;
                    }

                    if (validadati)
                    {
                        cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref clim2.itemselected);
                        clim2.idselected = clim2.itemselected.Id_cliente;
                        clim2.filterparams.Lingua = "";//visualizza sempre i clienti in tutte le lingue
                        clim2.list = cliDM.CaricaClientiFiltrati(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, clim2.filterparams, true, clim2.Pager.CurrentPage, clim2.Pager.PageSize);//Passando null -> prende tutti i clienti, inoltre baypasso il filtro 
                        long nrecordfiltrati2 = clim2.list.Totrecs;
                        clim2.Pager.TotalRecords = ((long)clim2.list.Totrecs);
                        clim2.Pager.GeneratePages();
                        if (nrecordfiltrati2 == 0) clim2.Pager.CurrentPage = 1;
                        clim2.itemselected = clim2.list.Exists(i => i.Id_cliente == clim2.idselected) ? clim2.list.Find(i => i.Id_cliente == clim2.idselected) : empyitem;
                        //precarico le liste geo per la visualizzazione di dettaglio in gelist1 in base al cliente selezionato
                        references.caricadatiddlgeo(clim2.geolist1, clim2.itemselected.CodiceREGIONE, clim2.itemselected.CodicePROVINCIA, clim2.itemselected.CodiceCOMUNE, clim2.itemselected.CodiceNAZIONE, lingua);
                        references.caricadatiddlgeo(clim2.geolist2, clim2.itemselected.addvalues.CodiceREGIONE, clim2.itemselected.addvalues.CodicePROVINCIA, clim2.itemselected.addvalues.CodiceCOMUNE, clim2.itemselected.addvalues.CodiceNAZIONE, lingua);
                    }

                    //context.Response.ContentType = "application/json";
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(clim2);// devo tornare il serializzato del model

                    break;
                case "getuserbyidcliente":

                    //Cliente cli = new Cliente();
                    //cli = (Cliente)dataitem;
                    //usermanager USM = new usermanager();
                    //string username = USM.GetUsernamebycamporofilo("idCliente", cli.Id_cliente.ToString());
                    //if (!string.IsNullOrEmpty(username))
                    //{
                    //    result = username;
                    //}
                    //else
                    //    result = "non presente";
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



public class initpagemodelclienti
{
    public string message = "";
    public string messageResp = "";

    //public OrderedDictionary statuslist = new OrderedDictionary();
    //public OrderedDictionary etalist = new OrderedDictionary();
    //lista tipi clienti 
    //lista nazioni per filtro!
    public OrderedDictionary nazionilist = new OrderedDictionary();
    public OrderedDictionary tipiclientilist = new OrderedDictionary();
    public OrderedDictionary languageslist = new OrderedDictionary();
    public OrderedDictionary generelist = new OrderedDictionary();


    public int maxresults = 50;
    public bool isOpen = false;
    public string filterkey = string.Empty;
    public string selectedkey = string.Empty;
    public OrderedDictionary filteredautocomplete = new OrderedDictionary();

}

public class clientivuemodel
{
    //public long idattivita = 0;
    public long idselected = 0;
    public ClienteCollection list = new ClienteCollection();
    public Cliente itemselected = new Cliente();
    public Cliente filterparams = new Cliente();
    public Tabrif utente = new Tabrif();

    public listegeografiche geolist1 = new listegeografiche();
    public listegeografiche geolist2 = new listegeografiche();

    public string message = "";
    public string messageResp = "";
    public PagerModel Pager = new PagerModel(1, 40, 0);
    public OrderedDictionary coordinatoriinlist = new OrderedDictionary();
    public clientivuemodel()
    {
        filterparams.id_tipi_clienti = "";
        filterparams.Lingua = "";
        itemselected.addvalues = new Cliente();
    }
}
