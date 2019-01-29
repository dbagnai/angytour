using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using WelcomeLibrary;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SQLite;

public class jreturncontainerdata
{
    public string html { set; get; }
    public Dictionary<string, string> jscommands { set; get; }
}

public class HandlerDataCommon : IHttpHandler, IRequiresSessionState
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
            Dictionary<string, string> pars = parseparams(context);
            string q = pars.ContainsKey("q") ? pars["q"] : "";

            string term = pars.ContainsKey("term") ? pars["term"].ToLower() : "";
            string id = pars.ContainsKey("id") ? pars["id"] : "";
            List<ResultAutocomplete> lra = new List<ResultAutocomplete>();
            string Recs = pars.ContainsKey("r") ? pars["r"].ToLower() : "50";
            long irecs = 0;
            string lingua = pars.ContainsKey("lng") ? pars["lng"] : "I";
            string progressivo = pars.ContainsKey("progressivo") ? pars["progressivo"] : "";
            string tipologia = pars.ContainsKey("tipologia") ? pars["tipologia"] : "";

            string filter1 = pars.ContainsKey("filter1") ? pars["filter1"] : "";
            string filter2 = pars.ContainsKey("filter2") ? pars["filter2"] : "";
            string Key = pars.ContainsKey("key") ? pars["key"] : "";
            string Value = pars.ContainsKey("value") ? pars["value"] : "";

            string objfiltro = pars.ContainsKey("objfiltro") ? pars["objfiltro"] : "";
            string page = pars.ContainsKey("page") ? pars["page"] : "1";
            string pagesize = pars.ContainsKey("pagesize") ? pars["pagesize"] : "10";
            string enablepager = pars.ContainsKey("enablepager") ? pars["enablepager"] : "true";
            Dictionary<string, string> res = new Dictionary<string, string>();
            Dictionary<string, string> filtri = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);

            switch (q)
            {
                case "autocompleteclienti":
                    long.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 20;
                    if (term != "null")
                    {
                        ClientiDM cliDM = new ClientiDM();
                        ClienteCollection coll = cliDM.GetLista(term, WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                        long count = 0;
                        ResultAutocomplete ra = null;
                        // ResultAutocomplete ra = new ResultAutocomplete() { id = "", label = "", value = "Deseleziona", codice = "" };
                        //lra.Add(ra);
                        if (coll != null)
                            foreach (Cliente r in coll)
                            {
                                ra = new ResultAutocomplete() { id = r.Id_cliente.ToString(), label = r.Spare3, email = r.Email, nome = r.Nome.ToString(), cognome = r.Cognome.ToString() };
                                if (id == null || id == "") lra.Add(ra);
                                else if (id != "" && r.Id_cliente.ToString() == id) lra.Add(ra);
                                count++;
                                if (count > irecs) break;
                            }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(lra, Newtonsoft.Json.Formatting.Indented);
                    break;
                case "autocompletecaratteristiche":
                    long.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 50;

                    int progr = 0;
                    int.TryParse(progressivo, out progr);
                    //if (progr == 0) progr = 5;
                    if (term != "null")
                    {
                        List<Tabrif> Caratteristica = new List<Tabrif>();
                        Caratteristica = references.FiltraCaratteristiche(progr, term, lingua);
                        long count = 0;

                        ResultAutocomplete ra = new ResultAutocomplete() { id = "", label = "", value = "Tutti", codice = "" };
                        lra.Add(ra);
                        if (Caratteristica != null)
                            foreach (Tabrif r in Caratteristica)
                            {
                                ra = new ResultAutocomplete() { id = r.Id.ToString(), label = r.Campo1, value = r.Campo1, codice = r.Codice.ToString() };
                                if (id == null || id == "") lra.Add(ra);
                                else if (id != "" && r.Id.ToString() == id) lra.Add(ra);
                                count++;
                                if (count > irecs) break;
                            }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(lra, Newtonsoft.Json.Formatting.Indented);
                    break;
                case "autocompletericerca":
                    long.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 20;
                    if (term != "null")
                    {
                        offerteDM offDM = new offerteDM();
                        OfferteCollection coll = offDM.GetLista(term, irecs.ToString(), lingua, WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                        long count = 0;
                        ResultAutocomplete ra1 = new ResultAutocomplete() { id = "", label = "Deseleziona" };
                        lra.Add(ra1);
                        if (coll != null)
                            foreach (Offerte r in coll)
                            {
                                ra1 = new ResultAutocomplete() { id = r.Id.ToString(), label = r.DenominazionebyLingua(lingua) };
                                if (id == null || id == "") lra.Add(ra1);
                                else if (id != "" && r.Id.ToString() == id) lra.Add(ra1);
                                count++;
                                if (count > irecs) break;
                            }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(lra, Newtonsoft.Json.Formatting.Indented);
                    break;
                case "initreferencesdata":

                    result = references.initreferencesdataserialized(lingua, context.User.Identity.Name);


                    break;
                case "inviamessaggiomail":
                    string smaildata = pars.ContainsKey("data") ? pars["data"] : "";
                    Dictionary<string, string> maildata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(smaildata);

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Prepariamo e inviamo il mail
                    Dictionary<string, string> destinatariperregione = new Dictionary<string, string>();
                    string nomemittente = (maildata.GetValueOrDefault("name") ?? "");
                    string cognomemittente = (maildata.GetValueOrDefault("cognome") ?? "");
                    string mittenteMail = (maildata.GetValueOrDefault("email") ?? "");
                    string mittenteTelefono = (maildata.GetValueOrDefault("telefono") ?? "");
                    string message = (maildata.GetValueOrDefault("message") ?? "");
                    string location = (maildata.GetValueOrDefault("location") ?? "");
                    string regione = (maildata.GetValueOrDefault("regione") ?? "");
                    string adulti = (maildata.GetValueOrDefault("adulti") ?? "");
                    string bambini = (maildata.GetValueOrDefault("bambini") ?? "");
                    string arrivo = (maildata.GetValueOrDefault("arrivo") ?? "");
                    string partenza = (maildata.GetValueOrDefault("partenza") ?? "");
                    string chkprivacy = (maildata.GetValueOrDefault("chkprivacy") ?? "");
                    string chknewsletter = (maildata.GetValueOrDefault("chknewsletter") ?? "");
                    bool spuntaprivacy = false;
                    bool spuntanewsletter = false;
                    bool.TryParse(chkprivacy, out spuntaprivacy);
                    bool.TryParse(chknewsletter, out spuntanewsletter);

                    string idofferta = (maildata.GetValueOrDefault("idofferta") ?? "");
                    string nomedestinatario = ConfigManagement.ReadKey("Nome");
                    string maildestinatario = ConfigManagement.ReadKey("Email");
                    string tipocontenuto = (maildata.GetValueOrDefault("tipocontenuto") ?? "");
                    long idperstatistiche = 0;
                    string tipo = (maildata.GetValueOrDefault("tipo") ?? "");
                    if (tipocontenuto == "Prenota")
                        tipo = "richiesta preventivo prenotazione ";
                    if (tipocontenuto == "Acquistousato")
                        tipo = "vendita usato ";

                    string SoggettoMail = "Richiesta " + tipo + " da " + cognomemittente + " " + nomemittente + " tramite il sito " + ConfigManagement.ReadKey("Nome");
                    string Descrizione = message.Replace("\r", "<br/>") + " <br/> ";
                    if (idofferta != "") //Inseriamo il dettaglio della scheda di provenienza
                    {
                        offerteDM offDM = new offerteDM();
                        Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idofferta);
                        if (item != null && item.Id != 0)
                        {
                            long.TryParse(idofferta, out idperstatistiche);
                            if (!string.IsNullOrWhiteSpace(item.Email)) //Se non è vuota mando alla mail indicata nell'articolo
                            {
                                nomedestinatario = item.Email;
                                maildestinatario = item.Email;
                            }
                            Descrizione += "<br/><br/>";
                            Descrizione += "Pagina provenienza: " + item.DenominazioneI.Replace("\r", "<br/>") + " id:" + idperstatistiche;
                            Descrizione += "<br/><br/>";
                        }
                    }
                    if (tipocontenuto == "Richiesta")
                    {
                    }
                    if (tipocontenuto == "Prenota")
                    {
                        Descrizione += " <br/> Arrivo richiesto:" + arrivo + " Partenza Richiesta: " + partenza;
                        Descrizione += " <br/> Numero adulti:" + adulti + " <br/> Numero bambini:" + bambini + " Alloggio : " + location;
                    }
                    Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome o rag soc. Cliente: " + cognomemittente;
                    Descrizione += " <br/> Telefono Cliente: " + mittenteTelefono + "  Email Cliente: " + mittenteMail + " Lingua Cliente: " + lingua;
                    Descrizione += " <br/> Il cliente ha Confermato l'autorizzazione al trattamento dei dati personali. ";

                    if (spuntanewsletter == true)
                    {
                        Descrizione += " <br/> Il cliente ha richiesto l'invio newsletter. " + references.ResMan("Common", lingua, "titolonewsletter1").ToString() + "<br/>";

                        //SoggettoMail = "Richiesta iscrizione newsletter da " + nomemittente + " tramite il sito " + Nome;
                        //------------------------------------------------
                        //Memorizzo i dati nel cliente per la newsletter
                        //------------------------------------------------
                        ClientiDM cliDM = new ClientiDM();
                        Cliente cli = new Cliente();
                        string tipocliente = "0"; //Cliente standard per newsletter
                        //  cli.DataNascita = System.DateTime.Now.Date;
                        cli.Lingua = lingua;
                        cli.id_tipi_clienti = tipocliente;
                        cli.Nome = nomemittente.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                        cli.Cognome = cognomemittente.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                        cli.Consenso1 = true;
                        cli.ConsensoPrivacy = true;
                        cli.Validato = true;
                        cli.Email = mittenteMail.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                        Cliente _clitmp = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email, tipocliente);
                        if ((_clitmp != null && _clitmp.Id_cliente != 0))
                            cli.Id_cliente = _clitmp.Id_cliente;
                        cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);
                    }

                    if (spuntaprivacy)
                    {
                        Utility.invioMailGenerico(nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
                        // Registro la statistica di contatto
                        Statistiche stat = new Statistiche();
                        stat.Data = DateTime.Now;
                        stat.EmailDestinatario = maildestinatario;
                        stat.EmailMittente = mittenteMail;
                        stat.Idattivita = idperstatistiche;
                        stat.Testomail = nomemittente + "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                        stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                        stat.Url = "";
                        statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);
                        result = CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", lingua, "LinkContatti"));
                        if (idofferta != "") result += "&idOfferta=" + idofferta.ToString();
                        result += "&conversione=true";
                    }
                    else
                    {
                        result = references.ResMan("Common", lingua, "txtPrivacyError") + " <br/> Mancata Autorizzazione privacy"; ;
                        throw new ApplicationException(result);
                    }

                    break;
                case "putinsession":
                    context.Session.Add(Key, Value);
                    break;
                case "getfromsession":

                    if (context.Session[Key] != null)
                        result = context.Session[Key].ToString();
                    break;
                case "emptysession":
                    HttpContext.Current.Session.Clear();
                    result = "svuotata sessione";
                    break;

                case "caricaConfig":

                    List<ConfigItem> retconfig = new List<ConfigItem>();

                    retconfig = ConfigManagement.ReadAsList(filtri);
                    if (retconfig == null || retconfig.Count == 0)
                        retconfig.Add(new ConfigItem());

                    //Dictionary<string, string> dictconfig  =  filterDictionaryConfig(filtri);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(retconfig, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "updateconfig":
                    string itemdata = pars.ContainsKey("itemdata") ? pars["itemdata"] : "";
                    List<ConfigItem> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConfigItem>>(itemdata
                    , new JsonSerializerSettings()
                    {
                        DateFormatString = "dd/MM/yyyy HH:mm:ss",
                        //DateFormatString = "dd/MM/yyyy",
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    //Chiamiamo l'aggiornamento di tutti i dati
                    //ConfigDM cDM = new ConfigDM();
                    //result = cDM.AggiornaConfigList(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref list);
                    result = ConfigManagement.AggiornaConfigList(ref list);

                    break;

                case "caricaresources":

                    List<ResourceItem> retresource = new List<ResourceItem>();

                    retresource = ResourceManagement.ReadItemsByLingua(lingua);
                    if (retresource == null || retresource.Count == 0)
                        retresource.Add(new ResourceItem());

                    //Dictionary<string, string> dictconfig  =  filterDictionaryConfig(filtri);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(retresource, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "updateresources":
                    string itemdataresource = pars.ContainsKey("itemdata") ? pars["itemdata"] : "";
                    List<ResourceItem> listresource = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResourceItem>>(itemdataresource
                    , new JsonSerializerSettings()
                    {
                        DateFormatString = "dd/MM/yyyy HH:mm:ss",
                        NullValueHandling = NullValueHandling.Ignore,
                        //DateFormatString = "dd/MM/yyyy",
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    //Chiamiamo l'aggiornamento di tutti i dati
                    result = ResourceManagement.AggiornaResourceList(ref listresource);

                    break;


                case "initresources":/*Caricamento delle risorse di testo per lingua*/
                                     //System.Globalization.CultureInfo ci = references.setCulture(lingua);
                    Dictionary<string, Dictionary<string, string>> dict = references.GetResourcesByLingua(lingua);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
                    break;
                //Inizializzazione varibili base per javascipt
                case "initmainvars":
                    var percorsocontenuti = WelcomeLibrary.STATIC.Global.PercorsoContenuti.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);// CommonPage.ReplaceAbsoluteLinks("~" + ConfigManagement.ReadKey("DataDir") + "/Files");
                    var percorsocomune = WelcomeLibrary.STATIC.Global.PercorsoComune.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione); ;// CommonPage.ReplaceAbsoluteLinks("~" + ConfigManagement.ReadKey("DataDir") + "/Common");

                    var jpath = new references.jpath();
                    jpath.percorsocomune = percorsocomune;
                    jpath.percorsocontenuti = percorsocontenuti;
                    jpath.usecdn = WelcomeLibrary.STATIC.Global.usecdn;

                    System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                             new System.Web.Script.Serialization.JavaScriptSerializer();
                    result = oSerializer.Serialize(jpath);
                    //Oppure
                    //result = Newtonsoft.Json.JsonConvert.SerializeObject(jpath);
                    break;
                //Caricamento JSON struttura generale Nazioni
                case "caricaJSONnazioni":
                    List<WelcomeLibrary.DOM.Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (WelcomeLibrary.DOM.Tabrif _nz) { return _nz.Lingua == lingua; });
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(nazioni);
                    break;
                //Caricamento JSON struttura generale province ----------------
                case "caricaJSONprovince":
                    List<WelcomeLibrary.DOM.Province> provincelingua1 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua); });
                    provincelingua1.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(provincelingua1);
                    break;
                //Caricamento JSON struttura generale province ----------------
                case "caricaJSONregioni":
                    WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
                    List<WelcomeLibrary.DOM.Province> provincelingua2 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua); });
                    if (provincelingua2 != null && provincelingua2.Count > 0)
                    {
                        provincelingua2.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (WelcomeLibrary.DOM.Province item in provincelingua2)
                        {
                            if (item.Lingua == lingua)
                                if (!regioni.Exists(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Regione == item.Regione); }))
                                    regioni.Add(item);
                        }
                    }


                    result = Newtonsoft.Json.JsonConvert.SerializeObject(regioni);
                    break;
                //Caricamento JSON struttura generale COmuni
                case "caricaJSONcomuni":
                    List<WelcomeLibrary.DOM.Comune> comuniordered = new List<WelcomeLibrary.DOM.Comune>();
                    comuniordered.AddRange(Utility.ElencoComuni);
                    if (comuniordered != null)
                        comuniordered.Sort(new GenericComparer<WelcomeLibrary.DOM.Comune>("Nome", System.ComponentModel.ListSortDirection.Ascending));
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(comuniordered);
                    break;


                //Selezione lista delle nazioni--------------------------
                case "fillddlnazioni":
                    //Parametri di filtro e selezione
                    List<WelcomeLibrary.DOM.Tabrif> nazionifill = Utility.Nazioni.FindAll(delegate (WelcomeLibrary.DOM.Tabrif _nz) { return _nz.Lingua == lingua; });
                    nazionifill.Sort(new GenericComparer<WelcomeLibrary.DOM.Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));
                    //Campo1 -> value
                    //    Codice -> key
                    foreach (WelcomeLibrary.DOM.Tabrif r in nazionifill)
                        res.Add(r.Codice, r.Campo1);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    break;
                //Selezione lista delle regioni---------------------------
                case "fillddlregioni":
                    //Parametri di filtro e selezione

                    WelcomeLibrary.DOM.ProvinceCollection regionifill = new WelcomeLibrary.DOM.ProvinceCollection();
                    List<WelcomeLibrary.DOM.Province> provincelingua3 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua && tmp.SiglaNazione.ToLower() == filter1.ToLower()); });
                    if (provincelingua3 != null && provincelingua3.Count > 0)
                    {
                        provincelingua3.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (WelcomeLibrary.DOM.Province item in provincelingua3)
                        {
                            if (item.Lingua == lingua)
                                if (!regionifill.Exists(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Regione == item.Regione); }))
                                    regionifill.Add(item);
                        }
                    }
                    //Campo1 -> value
                    //Codice -> key
                    foreach (WelcomeLibrary.DOM.Province r in regionifill)
                        res.Add(r.Codice, r.Regione);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    break;
                //Selezione lista provinceie ----------------------------------------------------------
                case "fillddlprovince":
                    //Parametri di filtro e selezione

                    WelcomeLibrary.DOM.Province _tmp = Utility.ElencoProvince.Find(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua && tmp.Codice == filter2); });
                    if (_tmp != null)
                    {
                        List<WelcomeLibrary.DOM.Province> provincelingua4 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua && tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == filter1.ToLower()); });
                        provincelingua4.Sort(new GenericComparer<WelcomeLibrary.DOM.Province>("Provincia", System.ComponentModel.ListSortDirection.Ascending));
                        //Campo1 -> value
                        //    Codice -> key
                        foreach (WelcomeLibrary.DOM.Province r in provincelingua4)
                            res.Add(r.Codice, r.Provincia);
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    }

                    break;
                //Selezione lista comuni ----------------------------------------------
                case "fillddlcomuni":

                    List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate (WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == filter2); });
                    if (comunilingua != null)
                        comunilingua.Sort(new GenericComparer<WelcomeLibrary.DOM.Comune>("Nome", System.ComponentModel.ListSortDirection.Ascending));
                    //Campo1 -> value
                    //    Codice -> key

                    foreach (WelcomeLibrary.DOM.Comune r in comunilingua)
                        res.Add(r.Nome, r.Nome);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    break;

                default:
                    break;
                case "caricaDati":

                    //////////////////////////////////////////////////////////////////////
                    //recupero i parametri che mi servono da objfiltro
                    //////////////////////////////////////////////////////////////////////                    
                    Dictionary<string, string> value = new Dictionary<string, string>();
                    value = offerteDM.filterData(lingua, filtri, page, pagesize, enablepager);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;
                case "caricahmtlbinded":
                    string htmlout = "";
                    Dictionary<string, string> filtripager = new Dictionary<string, string>();
                    filtripager.Add("page", page);
                    filtripager.Add("pagesize", pagesize);
                    filtripager.Add("enablepager", enablepager);
                    //htmlout = custombind.getbindedhtmlstring(filtri, filtripager, lingua, context.User.Identity.Name, context.Session);

                    String maincontainertext = "";
                    if (filtri.ContainsKey("maincontainertext")) maincontainertext = WelcomeLibrary.UF.dataManagement.DecodeFromBase64(filtri["maincontainertext"]);
                    //if (filtri.ContainsKey("maincontainertext")) maincontainertext = filtri["maincontainertext"].Replace("\\\"", "\"").Replace("|", "'");
                    //if (filtri.ContainsKey("maincontainertext")) maincontainertext = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(filtri["maincontainertext"]);
                    if (!string.IsNullOrEmpty(maincontainertext))
                    {
                        htmlout = custombind.bind(maincontainertext, lingua, context.User.Identity.Name, context.Session, filtri, filtripager);
                        jreturncontainerdata jr = new jreturncontainerdata();
                        jr.html = htmlout;
                        jr.jscommands = custombind.jscommands;
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(jr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None,
                        });
                        //Svuoto jscommands in memoria
                        custombind.jscommands = new Dictionary<string, string>();
                    }
                    break;
                case "getlinkbyid":
                    Dictionary<string, string> filtriid = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> valueRet1 = new Dictionary<string, string>();
                    if (filtriid.ContainsKey("id"))
                        valueRet1 = offerteDM.getlinklist(lingua, filtriid["id"]);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(valueRet1, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "getlinkbyfilters": //Mi crea un link con i custom filters ( da usare per la search personalizzata a catalogo )
                    Dictionary<string, string> filtriadded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> addpars = new Dictionary<string, string>();
                    string tipologiatmp = "-";
                    string testourl = "";
                    if (filtriadded.ContainsKey("tipologia"))
                    {
                        tipologiatmp = filtriadded["tipologia"];
                    }
                    if (filtriadded.ContainsKey("caratteristica1"))
                    {
                        //testourl = references.TestoCaratteristica(0, filtriadded["caratteristica1"], lingua);
                        Tabrif c = Utility.Caratteristiche[0].Find(p => p.Codice == filtriadded["caratteristica1"] && p.Lingua == lingua);
                        if (c != null)
                        {
                            testourl = c.Campo1 + " ";
                            addpars.Add("Caratteristica1", filtriadded["caratteristica1"]);
                        }
                    }
                    if (filtriadded.ContainsKey("regione"))
                    {
                        string nomeregione = references.NomeRegione(filtriadded["regione"], lingua);
                        if (!string.IsNullOrEmpty(nomeregione))
                        {
                            testourl += nomeregione + " ";
                            addpars.Add("Regione", filtriadded["regione"]);
                        }
                    }
                    if (filtriadded.ContainsKey("provincia"))
                    {
                        string nomeprovincia = references.NomeProvincia(filtriadded["provincia"], lingua);
                        if (!string.IsNullOrEmpty(nomeprovincia))
                        {
                            testourl += nomeprovincia + " ";
                            addpars.Add("Provincia", filtriadded["provincia"]);
                        }
                    }
                    if (filtriadded.ContainsKey("comune"))
                    {
                        string nomecomune = filtriadded["comune"];
                        if (!string.IsNullOrEmpty(nomecomune))
                        {
                            testourl += nomecomune + " ";
                            addpars.Add("Comune", filtriadded["comune"]);
                        }
                    }
                    //eventuale aggiunta di geolocation e hidricercaid // per la crezione di url
                    //if (filtriadded.ContainsKey("geolocation"))
                    //{
                    //    string geolocation = filtriadded["geolocation"];
                    //    if (!string.IsNullOrEmpty(geolocation))
                    //    {
                    //        testourl += "testodacalcolare in base alla poszione con un criterio" + " ";
                    //        addpars.Add("Geolocation", filtriadded["geolocation"]);
                    //    }
                    //}


                    testourl = testourl.Trim().Replace(" ", "-");
                    string linkcustom = SitemapManager.CreaLinkRoutes(lingua, testourl, "", tipologiatmp, "", "", "", "", "", true, true, addpars);

                    result = linkcustom;

                    break;
                case "caricaMenuSezioni":
                    Dictionary<string, string> filtriMenu = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);

                    Dictionary<string, string> valueRet = new Dictionary<string, string>();
                    valueRet = SitemapManager.creaMenuSezioni(filtriMenu["min"], filtriMenu["max"], lingua);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(valueRet, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "caricaDatiBanner":
                    Dictionary<string, string> filtriBanner = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);

                    Dictionary<string, string> valueBan = new Dictionary<string, string>();
                    valueBan = bannersDM.filterDataBanner(lingua, filtriBanner, page, pagesize, enablepager);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(valueBan, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "caricaLinks2liv":

                    //////////////////////////////////////////////////////////////////////
                    //recupero i parametri che mi servono da objfiltro
                    //////////////////////////////////////////////////////////////////////
                    Dictionary<string, string> filtriCategorie = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> linksDictionary = new Dictionary<string, string>();
                    Dictionary<string, List<Tabrif>> mainDictionary = new Dictionary<string, List<Tabrif>>();
                    Tabrif elemlink = new Tabrif();

                    List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == lingua && (tmp.CodiceTipologia == filtriCategorie["tipologia"])); });
                    prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                    if (prodotti != null)
                    {
                        prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (Prodotto o in prodotti)
                        {
                            string testo = o.Descrizione;
                            //string linkcategoria = CommonPage.CreaLinkRoutes(null, false, lingua, (testo), "", o.CodiceTipologia, o.CodiceProdotto);
                            //linkcategoria = linkcategoria.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                            List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == lingua && (tmp.CodiceProdotto == o.CodiceProdotto)); });
                            sprodotti.Sort(new GenericComparer<SProdotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                            if (sprodotti != null)
                            {
                                foreach (SProdotto s in sprodotti)
                                {
                                    string testosprod = s.Descrizione;
                                    string linksprod = CommonPage.CreaLinkRoutes(null, false, lingua, (testosprod), "", filtriCategorie["tipologia"], s.CodiceProdotto, s.CodiceSProdotto);
                                    linksprod = linksprod.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                                    elemlink = new Tabrif();
                                    elemlink.Codice = s.CodiceSProdotto;
                                    elemlink.Campo1 = linksprod;
                                    elemlink.Campo2 = testosprod;
                                    if (mainDictionary.ContainsKey(testo))
                                    {
                                        mainDictionary[testo].Add(elemlink);
                                    }
                                    else
                                    {
                                        List<Tabrif> tmpList = new List<Tabrif>();
                                        mainDictionary.Add(testo, tmpList);
                                        mainDictionary[testo].Add(elemlink);
                                    }
                                }
                            }


                        }
                    }

                    string serializedmaindictionary = Newtonsoft.Json.JsonConvert.SerializeObject(mainDictionary, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    linksDictionary.Add("data", serializedmaindictionary);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(linksDictionary, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;
                case "caricaLinks1liv":

                    //////////////////////////////////////////////////////////////////////
                    //recupero i parametri che mi servono da objfiltro
                    //////////////////////////////////////////////////////////////////////
                    Dictionary<string, string> filtriCategorie1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> linksDictionary1 = new Dictionary<string, string>();
                    Dictionary<string, List<Tabrif>> mainDictionary1 = new Dictionary<string, List<Tabrif>>();
                    Tabrif elemlink1 = new Tabrif();
                    //filtriCategorie1["tipologia"]
                    List<string> tipologie = new List<string>();
                    if (filtriCategorie1.ContainsKey("tipologia"))
                        tipologie = filtriCategorie1["tipologia"].Split('|').ToList<string>();
                    List<TipologiaOfferte> tlist = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => tipologie.Contains(t.Codice) && t.Lingua == lingua);
                    if (tlist != null)
                    {
                        tlist.Sort(new GenericComparer<TipologiaOfferte>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (TipologiaOfferte t in tlist)
                        {
                            string testotipologia = t.Descrizione;
                            List<Prodotto> prodotti1 = Utility.ElencoProdotti.FindAll(p => p.Lingua == lingua && p.CodiceTipologia == t.Codice);
                            prodotti1.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                            if (prodotti1 != null)
                            {
                                prodotti1.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                                foreach (Prodotto o in prodotti1)
                                {
                                    string testoprodotto = o.Descrizione;
                                    string linkcategoria = CommonPage.CreaLinkRoutes(null, false, lingua, (testoprodotto), "", o.CodiceTipologia, o.CodiceProdotto);
                                    linkcategoria = linkcategoria.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                                    elemlink1 = new Tabrif();
                                    elemlink1.Codice = o.CodiceProdotto;
                                    elemlink1.Campo1 = linkcategoria;
                                    elemlink1.Campo2 = testoprodotto;
                                    if (mainDictionary1.ContainsKey(testotipologia))
                                    {
                                        mainDictionary1[testotipologia].Add(elemlink1);
                                    }
                                    else
                                    {
                                        List<Tabrif> tmpList = new List<Tabrif>();
                                        mainDictionary1.Add(testotipologia, tmpList);
                                        mainDictionary1[testotipologia].Add(elemlink1);
                                    }
                                }
                            }
                        }
                    }
                    string serializedmainDictionary1 = Newtonsoft.Json.JsonConvert.SerializeObject(mainDictionary1, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    linksDictionary1.Add("data", serializedmainDictionary1);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(linksDictionary1, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;
                case "caricaDatiArchivio":

                    //////////////////////////////////////////////////////////////////////
                    //recupero i parametri che mi servono da objfiltro
                    //////////////////////////////////////////////////////////////////////
                    Dictionary<string, string> filtriArchivio = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> valueArchivio = new Dictionary<string, string>();
                    Dictionary<string, List<Tabrif>> tmpArchivio = new Dictionary<string, List<Tabrif>>();
                    offerteDM offDM1 = new offerteDM();
                    Tabrif elem = new Tabrif();

                    Dictionary<string, Dictionary<string, string>> archivioperannomese = offDM1.ContaPerAnnoMese(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, lingua, filtriArchivio["tipologia"], filtriArchivio["categoria"]);
                    string link = "";
                    foreach (string anno in archivioperannomese.Keys)
                    {
                        foreach (string mese in archivioperannomese[anno].Keys)
                        {
                            elem = new Tabrif();
                            link = "";
                            link = CommonPage.CreaLinkRicerca("", filtriArchivio["tipologia"], filtriArchivio["categoria"], "", "", anno, mese, filtriArchivio["tipologia"], lingua, HttpContext.Current.Session);
                            elem.Codice = mese;
                            elem.Campo1 = link;
                            DateTime _d = new DateTime(Convert.ToInt16(anno), Convert.ToInt16(mese), 1);
                            elem.Campo2 = String.Format("{0:MMM yyyy}", _d) + "    (" + archivioperannomese[anno][mese] + ")";

                            if (tmpArchivio.ContainsKey(anno))
                            {
                                tmpArchivio[anno].Add(elem);
                            }
                            else
                            {
                                List<Tabrif> tmpList = new List<Tabrif>();
                                tmpArchivio.Add(anno, tmpList);
                                tmpArchivio[anno].Add(elem);
                            }
                        }
                    }
                    string tempOffArchivio = Newtonsoft.Json.JsonConvert.SerializeObject(tmpArchivio, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    valueArchivio.Add("data", tempOffArchivio);
                    //value = filterDataArchivio(lingua, filtriArchivio);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(valueArchivio, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;
                case "getestatefromurl": /*Importazione datatbase immobiliare da gestionale*/

                    string Urlexport = WelcomeLibrary.STATIC.Global.percorsoapp + WelcomeLibrary.STATIC.Global.percorsoexp.TrimEnd('/') + ".zip";
                    string zippedfile = WelcomeLibrary.STATIC.Global.PercorsoComune + "/_export.zip";
                    zippedfile = context.Server.MapPath(zippedfile);
                    WelcomeLibrary.UF.SharedStatic.MakeHttpGet(Urlexport, zippedfile);

                    string destinationfilepath = context.Server.MapPath("~" + WelcomeLibrary.STATIC.Global.percorsoexp);
                    string originaldir = WelcomeLibrary.STATIC.Global.percorsoexp;
                    string temporarydir = WelcomeLibrary.STATIC.Global.percorsoexp.TrimEnd('/') + "tmp/";
                    string tmpdestinationfilepath = context.Server.MapPath("~" + temporarydir);

                    //Utilizzo una dir di appoggio di lavoro temporanea per evitare conflitti sulla lettura dei filese /public/common/_exporttmp/
                    if (!System.IO.Directory.Exists(tmpdestinationfilepath))
                        System.IO.Directory.CreateDirectory(tmpdestinationfilepath);
                    WelcomeLibrary.UF.Utility.UnZip(zippedfile, tmpdestinationfilepath, ""); //Unzip to temporary folder
                                                                                             //CAMBIO TEMPORANEAMENTE DIRECTORY DI LAVORO PER IL SITO PER EVITARE CONFILITTI DI LOCK PER I FILES
                    WelcomeLibrary.STATIC.Global.percorsoexp = temporarydir;///////////////
                    System.Threading.Thread.Sleep(2000); //Attendo eventuali richieste in corso da ultimare

                    //Scompattiamo i nuovi files nella directory finale
                    WelcomeLibrary.UF.Utility.UnZip(zippedfile, destinationfilepath, "");
                    //Reimposto la directory originale!!
                    WelcomeLibrary.STATIC.Global.percorsoexp = originaldir;///////////////
                    references.CaricaMemoriaStatica(context.Server); //Aggiorno la memoria statica se variata

                    System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
                    Messaggi.Add("Messaggio", "");
                    Messaggi["Messaggio"] += " Aggiornato correttamente immobili da gestionale " + System.DateTime.Now.ToString();
                    WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
                    break;
                case "creasitemapsresources":
                    //Da fare la gnerazione delle sitemap dal file estates.json
                    references.CreaSitemapImmobili(context.Server, "rif000666");
                    break;

            }
        }
        catch (Exception ex)
        {
            string er = ex.Message;
            result = er;
            context.Response.StatusCode = 400;
        }
        context.Response.Write(result);
        ///////////////////////////////////////////////
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}

