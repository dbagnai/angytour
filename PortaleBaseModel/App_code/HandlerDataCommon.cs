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

public class jpath
{
    public string percorsocontenuti { set; get; }
    public string percorsocomune { set; get; }
    public string percorsoapp { set; get; }
    public string percorsocdn { set; get; }
    public string percorsoimg { set; get; }
    public string percorsoexp { set; get; }
    public string percorsolistaimmobili { set; get; }
    public string jsonlanguages { set; get; }
    public string baseresources { set; get; }
    public string versionforcache { set; get; }
    public string dictreferences { set; get; }
    public string percorsolistadati { set; get; }
    public string jsonregioni { set; get; }
    public string jsonprovince { set; get; }
    public string jsoncategorie { set; get; }
    public string jsoncategorie2liv { set; get; }
    public string jsontipologie { set; get; }
    public bool usecdn { set; get; }
    public string username { set; get; }

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

                case "initreferencesdata":
                    var jpathcomplete = new jpath();

                    var percorsoapptmp = WelcomeLibrary.STATIC.Global.percorsoapp;// CommonPage.ReplaceAbsoluteLinks("~");
                    jpathcomplete.percorsocdn = WelcomeLibrary.STATIC.Global.percorsocdn;
                    jpathcomplete.percorsoimg = WelcomeLibrary.STATIC.Global.percorsoimg;
                    jpathcomplete.percorsoexp = WelcomeLibrary.STATIC.Global.percorsoexp;
                    jpathcomplete.usecdn = WelcomeLibrary.STATIC.Global.usecdn;
                    jpathcomplete.percorsoapp = percorsoapptmp;


                    var percorsocontenutitmp = WelcomeLibrary.STATIC.Global.PercorsoContenuti.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);// CommonPage.ReplaceAbsoluteLinks("~" + ConfigManagement.ReadKey("DataDir") + "/Files");
                    var percorsocomunetmp = WelcomeLibrary.STATIC.Global.PercorsoComune.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione); ;// CommonPage.ReplaceAbsoluteLinks("~" + ConfigManagement.ReadKey("DataDir") + "/Common");
                    jpathcomplete.percorsocomune = percorsocomunetmp;
                    jpathcomplete.percorsocontenuti = percorsocontenutitmp;
                    jpathcomplete.username = context.User.Identity.Name;

                    //LINGUE/////////////////////////////////////////////////////7
                    var filejsonlanguages = "languages.json";
                    //reflanguages = System.IO.File.ReadAllText(Server.MapPath("~/lib/cfg/" + filejsonlanguages));
                    var reflanguages = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\cfg\\" + filejsonlanguages);
                    jpathcomplete.jsonlanguages = reflanguages;

                    /////////////BASERESOURCES////////////////////////////////////////
                    jpathcomplete.baseresources = Newtonsoft.Json.JsonConvert.SerializeObject(references.GetResourcesByLingua(lingua));
                    ///////////////////////////////////////////////////////////////

                    jpathcomplete.versionforcache = WelcomeLibrary.STATIC.Global.versionforcache;

                    ////////////// REGIONI E PROIVINCE/////////////////////
                    WelcomeLibrary.DOM.TabrifCollection trifregioni = new TabrifCollection();
                    WelcomeLibrary.DOM.TabrifCollection trifprovince = new TabrifCollection();
                    WelcomeLibrary.DOM.ProvinceCollection regionitmp = new WelcomeLibrary.DOM.ProvinceCollection();
                    List<WelcomeLibrary.DOM.Province> provincelinguatmp = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua); });
                    if (provincelinguatmp != null && provincelinguatmp.Count > 0)
                    {
                        provincelinguatmp.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (WelcomeLibrary.DOM.Province item in provincelinguatmp)
                        {
                            if (item.Lingua == lingua)
                                if (!regionitmp.Exists(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Regione == item.Regione); }))
                                {
                                    regionitmp.Add(item);
                                    Tabrif t = new Tabrif();
                                    t.Codice = item.Codice;
                                    t.Campo1 = item.Regione;
                                    trifregioni.Add(t);
                                }
                            Tabrif p = new Tabrif();
                            p.Codice = item.Codice;
                            p.Campo1 = item.Provincia;
                            trifprovince.Add(p);
                        }
                    }
                    jpathcomplete.jsonregioni = Newtonsoft.Json.JsonConvert.SerializeObject(trifregioni);
                    jpathcomplete.jsonprovince = Newtonsoft.Json.JsonConvert.SerializeObject(trifprovince);
                    //////////////////////////////////////////////////////////////


                    /*Per immobili*/
                    string linktmp = "";
                    WelcomeLibrary.DOM.TipologiaOfferte tipotmp = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == lingua && tmp.Codice == "rif000666"); });
                    if (tipotmp != null)
                        linktmp = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(tipotmp.Descrizione), "", tipotmp.Codice);
                    jpathcomplete.percorsolistaimmobili = linktmp;
                    /*Per catalogo*/
                    linktmp = "";
                    tipotmp = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == lingua && tmp.Codice == "rif000001"); });
                    if (tipotmp != null)
                        linktmp = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(tipotmp.Descrizione), "", tipotmp.Codice);
                    jpathcomplete.percorsolistadati = linktmp;



                    List<Prodotto> listprod = WelcomeLibrary.UF.Utility.ElencoProdotti.FindAll(p => p.CodiceTipologia == "rif000001" && p.Lingua == lingua);
                    WelcomeLibrary.DOM.TabrifCollection trifprodotti = new TabrifCollection();
                    if (listprod != null)
                        foreach (Prodotto p in listprod)
                        {
                            Tabrif p1 = new Tabrif();
                            p1.Codice = p.CodiceProdotto;
                            p1.Campo1 = p.Descrizione;
                            trifprodotti.Add(p1);
                        }
                    jpathcomplete.jsoncategorie = Newtonsoft.Json.JsonConvert.SerializeObject(trifprodotti);


                    List<SProdotto> listsprod = WelcomeLibrary.UF.Utility.ElencoSottoProdotti.FindAll(p => p.Lingua == lingua);
                    WelcomeLibrary.DOM.TabrifCollection trifsprodotti = new TabrifCollection();
                    if (listprod != null)
                        foreach (SProdotto p in listsprod)
                        {
                            Tabrif p1 = new Tabrif();
                            p1.Codice = p.CodiceSProdotto;
                            p1.Campo2 = p.CodiceProdotto;
                            p1.Campo1 = p.Descrizione;
                            trifsprodotti.Add(p1);
                        }
                    jpathcomplete.jsoncategorie2liv = Newtonsoft.Json.JsonConvert.SerializeObject(trifsprodotti);

                    WelcomeLibrary.DOM.TabrifCollection tmptipo = new TabrifCollection();
                    if (WelcomeLibrary.UF.Utility.TipologieOfferte != null)
                        foreach (WelcomeLibrary.DOM.TipologiaOfferte p in WelcomeLibrary.UF.Utility.TipologieOfferte)
                        {
                            Tabrif p1 = new Tabrif();
                            p1.Codice = p.Codice;
                            p1.Campo1 = p.Descrizione;
                            p1.Lingua = p.Lingua;
                            tmptipo.Add(p1);
                        }
                    jpathcomplete.jsontipologie = Newtonsoft.Json.JsonConvert.SerializeObject(tmptipo);


                    //Dictionary<string, WelcomeLibrary.DOM.TabrifCollection> retdict = new Dictionary<string, WelcomeLibrary.DOM.TabrifCollection>();
                    Dictionary<string, string> retdict = new Dictionary<string, string>();

                    ////////////////ALTRE VARIABILI DI RIFERIMENTO SPECIFICHE////////////////////////////////////////
                    retdict.Add("JSONrefmetrature", references.refmetrature);
                    retdict.Add("JSONrefprezzi", references.refprezzi);
                    retdict.Add("JSONrefcondizione", references.refcondizione);
                    retdict.Add("JSONreftipocontratto", references.reftipocontratto);
                    retdict.Add("JSONreftiporisorse", references.reftiporisorse);
                    //retdict.Add("JSONgeogenerale", references.refgeogenerale);
                    ////////////////ALTRE VARIABILI DI RIFERIMENTO SPECIFICHE////////////////////////////////////////

                    retdict.Add("JSONcar1", Newtonsoft.Json.JsonConvert.SerializeObject(Utility.Caratteristiche[0]));
                    retdict.Add("JSONcar2", Newtonsoft.Json.JsonConvert.SerializeObject(Utility.Caratteristiche[1]));
                    retdict.Add("JSONcar3", Newtonsoft.Json.JsonConvert.SerializeObject(Utility.Caratteristiche[2]));
                    jpathcomplete.dictreferences = Newtonsoft.Json.JsonConvert.SerializeObject(retdict, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(jpathcomplete);
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

                    var jpath = new jpath();
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
                    value = filterData(lingua, filtri, page, pagesize, enablepager);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;

                case "caricaMenuSezioni":
                    Dictionary<string, string> filtriMenu = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);

                    Dictionary<string, string> valueRet = new Dictionary<string, string>();
                    valueRet = creaMenuSezioni(filtriMenu["min"], filtriMenu["max"], lingua);

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
                    valueBan = filterDataBanner(lingua, filtriBanner, page, pagesize, enablepager);
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

                    offerteDM offDM = new offerteDM();
                    Tabrif elem = new Tabrif();

                    Dictionary<string, Dictionary<string, string>> archivioperannomese = offDM.ContaPerAnnoMese(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, lingua, filtriArchivio["tipologia"], filtriArchivio["categoria"]);
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

    public Dictionary<string, string> creaMenuSezioni(string min, string max, string lingua)
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        int Min = Convert.ToInt32(min);
        int Max = Convert.ToInt32(max);
        List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte temp) { return (temp.Lingua == lingua); });
        sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < Min || Convert.ToInt32(t.Codice.Substring(3)) > Max);
        sezioni.Sort(new GenericComparer<TipologiaOfferte>("Codice", System.ComponentModel.ListSortDirection.Descending));

        string tempTpo = Newtonsoft.Json.JsonConvert.SerializeObject(sezioni, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
        });
        ret.Add("data", tempTpo);

        Dictionary<string, string> tmp = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> linksurl = new Dictionary<string, Dictionary<string, string>>();
        foreach (TipologiaOfferte _o in sezioni)
        {
            //ret.Add(CommonPage.CreaLinkRoutes(null, true, lingua, CommonPage.CleanUrl(item.Descrizione), "", item.Codice),item.Descrizione);
            string link = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(_o.Descrizione), "", _o.Codice);

            if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && link.ToLower().IndexOf("~") == -1)
            {
                link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
            }
            link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
            tmp = new Dictionary<string, string>();
            tmp.Add("link", link);
            tmp.Add("titolo", _o.Descrizione);

            linksurl.Add(_o.Codice, tmp);
        }
        string retlinksurl = Newtonsoft.Json.JsonConvert.SerializeObject(linksurl);
        ret.Add("linkloaded", retlinksurl);

        return ret;
    }



    public Dictionary<string, string> filterData(string lingua, Dictionary<string, string> filtri, string spage, string spagesize, string senablepager)
    {
        bool enabledpager = false;
        bool.TryParse(senablepager, out enabledpager);

        int page = 0;
        int pagesize = 0;
        int.TryParse(spage, out page);
        int.TryParse(spagesize, out pagesize);

        List<Offerte> filteredData = new List<Offerte>();
        offerteDM offDM = new offerteDM();
        Dictionary<string, string> ritorno = new Dictionary<string, string>();
        OfferteCollection offerte = new OfferteCollection();


        //CARICO FILTRANDO ////////////////////////////////////////////////////////////////
        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
        string maxrecords = "";
        if (filtri.ContainsKey("maxelement") && !string.IsNullOrEmpty(filtri["maxelement"]))
            maxrecords = filtri["maxelement"];

        if (filtri.ContainsKey("mostviewed") && !string.IsNullOrEmpty(filtri["mostviewed"]))
        {
            long maxelements = 0;
            long.TryParse(filtri["mostviewed"], out maxelements);
            if (maxelements != 0)
            {
                maxrecords = maxelements.ToString();
                //estraiamo la lista degli di più visti
                Dictionary<long, long> mostvisited = statisticheDM.ContaTutteVisite(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, filteredData, maxelements);
                long _i = 0;
                string idlistfiltro = "";
                foreach (KeyValuePair<long, long> kv in mostvisited)
                {
                    if (_i >= maxelements) break;
                    idlistfiltro += kv.Key + ",";
                    _i++;
                }
                idlistfiltro = idlistfiltro.TrimEnd(',');
                if (!string.IsNullOrEmpty(idlistfiltro))
                {
                    SQLiteParameter pidlist1 = new SQLiteParameter("@IdList", idlistfiltro);
                    parColl.Add(pidlist1);
                }
            }
        }

        if (filtri.ContainsKey("listShow") && !string.IsNullOrEmpty(filtri["listShow"]))
        {
            if (filtri["listShow"].Contains(','))
            {
                SQLiteParameter pidlist = new SQLiteParameter("@IdList", filtri["listShow"]);
                parColl.Add(pidlist);
            }
        }
        if (filtri.ContainsKey("id") && !string.IsNullOrEmpty(filtri["id"]))
        {
            SQLiteParameter pid = new SQLiteParameter("@Id", filtri["id"]);
            parColl.Add(pid);
        }
        if (filtri.ContainsKey("tipologia") && !string.IsNullOrEmpty(filtri["tipologia"]))
        {
            SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", filtri["tipologia"]);
            parColl.Add(p3);
        }
        if (filtri.ContainsKey("categoria") && !string.IsNullOrEmpty(filtri["categoria"]))
        {
            SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", filtri["categoria"]);
            parColl.Add(p7);
        }
        if (filtri.ContainsKey("categoria2Liv") && !string.IsNullOrEmpty(filtri["categoria2Liv"]))
        {
            SQLiteParameter p10 = new SQLiteParameter("@CodiceCategoria2Liv", filtri["categoria2Liv"]);
            parColl.Add(p10);
        }
        if (filtri.ContainsKey("caratteristica1") && !string.IsNullOrEmpty(filtri["caratteristica1"]))
        {
            SQLiteParameter pc1 = new SQLiteParameter("@Caratteristica1", filtri["caratteristica1"]);
            parColl.Add(pc1);
        }
        if (filtri.ContainsKey("caratteristica2") && !string.IsNullOrEmpty(filtri["caratteristica2"]))
        {
            SQLiteParameter pc2 = new SQLiteParameter("@Caratteristica2", filtri["caratteristica2"]);
            parColl.Add(pc2);
        }
        if (filtri.ContainsKey("caratteristica3") && !string.IsNullOrEmpty(filtri["caratteristica3"]))
        {
            SQLiteParameter pc3 = new SQLiteParameter("@Caratteristica3", filtri["caratteristica3"]);
            parColl.Add(pc3);
        }

        if (filtri.ContainsKey("regione") && !string.IsNullOrEmpty(filtri["regione"]))
        {
            SQLiteParameter preg = new SQLiteParameter("@CodiceREGIONE", filtri["regione"]);
            parColl.Add(preg);
        }


        if (filtri.ContainsKey("vetrina") && !string.IsNullOrEmpty(filtri["vetrina"]))
        {
            bool _tmpb = false;
            bool.TryParse(filtri["vetrina"], out _tmpb);
            SQLiteParameter pvet = new SQLiteParameter("@Vetrina", _tmpb);
            parColl.Add(pvet);
        }
        if (filtri.ContainsKey("promozioni") && !string.IsNullOrEmpty(filtri["promozioni"]))
        {
            bool _tmpb = false;
            bool.TryParse(filtri["promozioni"], out _tmpb);
            SQLiteParameter promo = new SQLiteParameter("@promozioni", _tmpb);
            parColl.Add(promo);
        }
        if (filtri.ContainsKey("testoricerca") && !string.IsNullOrEmpty(filtri["testoricerca"]))
        {
            string testoricerca = filtri["testoricerca"].Trim().Replace(" ", "%");
            SQLiteParameter p8 = new SQLiteParameter("@testoricerca", "%" + testoricerca + "%");
            parColl.Add(p8);
        }



        if (filtri.ContainsKey("mese") && !string.IsNullOrEmpty(filtri["mese"]))
            if (filtri.ContainsKey("anno") && !string.IsNullOrEmpty(filtri["anno"]))
            {
                string mese = filtri["mese"];
                string anno = filtri["anno"];
                if (mese.Trim() != "" && anno.Trim() != "")
                {
                    SQLiteParameter panno = new SQLiteParameter("@annofiltro", anno);
                    parColl.Add(panno);


                    SQLiteParameter pmese = new SQLiteParameter("@mesefiltro", mese);
                    parColl.Add(pmese);
                }

#if false
                    if (mese.Trim() != "" && anno.Trim() != "")
                    {
                        int _a = 0;
                        int.TryParse(anno, out _a);
                        int _m = 0;
                        int.TryParse(mese, out _m);
                        if (_a != 0)
                        {
                            SQLiteParameter panno = new SQLiteParameter("@annofiltro", _a);
                            parColl.Add(panno);
                        }
                        if (_m != 0)
                        {
                            SQLiteParameter pmese = new SQLiteParameter("@mesefiltro", _m);
                            parColl.Add(pmese);
                        }

                    } 
#endif
            }



        if (enabledpager && page != 0 && pagesize != 0)
        {
            offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, maxrecords, lingua, null, "", false, page, pagesize);
        }
        else if (senablepager == "skip" && page != 0 && pagesize != 0)
        {
            offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", lingua, null, "", false, page, pagesize);
            int lmaxrecords = 0;
            int.TryParse(maxrecords, out lmaxrecords);
            if (offerte != null && lmaxrecords != 0)
            {
                long nget = Math.Min(offerte.Count, lmaxrecords);
                OfferteCollection tmpoffselect = new OfferteCollection();
                for (int conta = 0; conta < nget; conta++)
                {
                    tmpoffselect.Add(offerte[conta]);
                }
                offerte = tmpoffselect;
                //if (lmaxrecords != 0)
                //    offerte = new OfferteCollection(offerte.GetRange(0, Math.Min(offerte.Count, lmaxrecords)));
            }

        }
        else
            offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, maxrecords, lingua, null, "");
        //}
        //else
        //    offerte = filtri[4];
 

#if false
        /*Old paging method*/
        if (offerte != null && offerte.Count > 0 && enabledpager && page != 0 && pagesize != 0)
        {
            //Facciamo il take skip
            int start = ((page - 1) * pagesize);
            //int end = start + pagesize - 1;
            if (start + pagesize > offerte.Count - 1)
                filteredData = offerte.GetRange(start, offerte.Count - start);
            else
                filteredData = offerte.GetRange(start, pagesize);
        }
        else filteredData = offerte;
#endif
#if false
        if (filtri.ContainsKey("maxelement") && !string.IsNullOrEmpty(filtri["maxelement"]))
        {
            int maxelem = 0;
            int.TryParse(filtri["maxelement"], out maxelem);
            if (maxelem < filteredData.Count())
                filteredData = filteredData.GetRange(0, maxelem);
        } 
#endif

        filteredData = offerte;
        string tempOff = Newtonsoft.Json.JsonConvert.SerializeObject(filteredData, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
        });
        ritorno.Add("data", tempOff);
        Dictionary<string, string> ListRet = new Dictionary<string, string>();
        ListRet.Add("visualData", filtri["visualData"]);
        ListRet.Add("visualPrezzo", filtri["visualPrezzo"]);

        string tot = "0";
        //if (offerte != null) tot = offerte.Count.ToString();
        if (offerte != null) tot = offerte.Totrecs.ToString();
        ListRet.Add("totalrecords", tot);
        string tempListret = Newtonsoft.Json.JsonConvert.SerializeObject(ListRet);
        ritorno.Add("resultinfo", tempListret);

        //Carico lista statistiche visite per inserirla nella lista di ritorno
        Dictionary<long, long> visite = new Dictionary<long, long>();
        if (filteredData != null && filteredData.Count > 0)
            visite = statisticheDM.ContaTutteVisite(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, filteredData);

        Dictionary<string, Dictionary<string, string>> linksurl = new Dictionary<string, Dictionary<string, string>>();
        foreach (Offerte _o in filteredData)
        {
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            string testotitolo = "";
            string descrizione = "";
            string datitecnici = "";
            switch (lingua)
            {
                case "GB":
                    testotitolo = _o.DenominazioneGB;
                    descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DescrizioneGB, 30000, true));
                    datitecnici = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DatitecniciGB, 30000, true));
                    break;
                default:
                    testotitolo = _o.DenominazioneI;
                    descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DescrizioneI, 30000, true));
                    datitecnici = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DatitecniciI, 30000, true));
                    break;
            }


            string linksezione = "";
            SProdotto sottocategoria = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto _tmp) { return (_tmp.Lingua == lingua && (_tmp.CodiceSProdotto == _o.CodiceCategoria2Liv)); });
            if (sottocategoria != null)
            {
                linksezione = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(sottocategoria.Descrizione), "", _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv);
                linksezione = "<a  onclick='javascript: JsSvuotaSession(this)'  href='" + linksezione + "'>" + sottocategoria.Descrizione + "</a>";
            }

            if (string.IsNullOrEmpty(linksezione))
            {
                Prodotto categoria = Utility.ElencoProdotti.Find(p => p.Lingua == lingua && (p.CodiceTipologia == _o.CodiceTipologia && p.CodiceProdotto == _o.CodiceCategoria));
                if (categoria != null)
                {
                    linksezione = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(categoria.Descrizione), "", _o.CodiceTipologia, _o.CodiceCategoria);
                    linksezione = "<a  onclick='javascript: JsSvuotaSession(this)'  href='" + linksezione + "'>" + categoria.Descrizione + "</a>";
                }
            }

            string pathimmagine = filemanage.ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString(),true,true);
            pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
            if (string.IsNullOrEmpty(pathimmagine))
                pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            string target = "";
            string link = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(testotitolo), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria);
            if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && link.ToLower().IndexOf("~") == -1)
            {
                target = "_self";
                link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
            }
            link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            //string titolo1 = testotitolo;
            //string titolo2 = "<br/>";
            //int i = testotitolo.IndexOf("\n");
            //if (i != -1)
            //{
            //    titolo1 = testotitolo.Substring(0, i);
            //    if (testotitolo.Length >= i + 1)
            //        titolo2 = testotitolo.Substring(i + 1);
            //}

            string contactlink = "";
            if (_o.Abilitacontatto) contactlink = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta&Lingua=" + lingua + "&idOfferta=" + _o.Id;
            string printlink = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/SchedaOffertaStampa.aspx?idOfferta=" + _o.Id + "&Lingua=" + lingua;
            string bcklink = GeneraBackLink(_o.CodiceTipologia, _o.CodiceCategoria, lingua);

            string pathavatar = "";
            if (string.IsNullOrEmpty(pathavatar))
                pathavatar = ("~/images/sitespecific/" + _o.Autore + ".png").Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            string numeroviews = "";
            if (visite != null && visite.ContainsKey(_o.Id))
                numeroviews = visite[_o.Id].ToString();
            tmp.Add("views", numeroviews); //Numero di visualizzazioni della scheda

            tmp.Add("contactlink", contactlink);
            tmp.Add("printlink", printlink);
            tmp.Add("bcklink", bcklink);
            tmp.Add("link", link);
            tmp.Add("linksezione", linksezione);
            tmp.Add("titolo", testotitolo);
            tmp.Add("descrizione", descrizione);
            tmp.Add("datitecnici", datitecnici);
            tmp.Add("image", pathimmagine);
            tmp.Add("avatar", pathavatar);
            tmp.Add("video", _o.linkVideo);

            //DETTAGLI PER LA LISTA COMPLETA ALLEGATI //////////////////////////////////
            if (filteredData != null && filteredData.Count == 1)  //Si riempiono solo per la scheda singola
            {

                /****CREO IL LINK ALLA SCHEDA PRECEDENTRE E PROSSIMA RISPETTO ALLA SCHEDA ATTUALE **********/
                //Carichiamo la prossima e precedente scheda di settore !!!
                if (parColl.Exists(delegate (SQLiteParameter _par) { return _par.ParameterName == "@Id"; }))
                {
                    parColl.Find(delegate (SQLiteParameter _par) { return _par.ParameterName == "@Id"; }).Value = _o.Id;
                }
                else
                {
                    SQLiteParameter pid = new SQLiteParameter("@Id", _o.Id);
                    parColl.Add(pid);
                }
                if (parColl.Exists(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceTIPOLOGIA"; }))
                {
                    parColl.Find(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceTIPOLOGIA"; }).Value = _o.CodiceTipologia; ;
                }
                else
                {
                    SQLiteParameter ptip = new SQLiteParameter("@CodiceTIPOLOGIA", _o.CodiceTipologia);
                    parColl.Add(ptip);
                }
                if (parColl.Exists(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceCategoria"; }))
                {
                    parColl.Find(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceCategoria"; }).Value = _o.CodiceCategoria;

                }
                else
                {
                    SQLiteParameter ptcat = new SQLiteParameter("@CodiceCategoria", _o.CodiceCategoria);
                    parColl.Add(ptcat);
                }
                if (parColl.Exists(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceCategoria2Liv"; }))
                {
                    parColl.Find(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceCategoria2Liv"; }).Value = _o.CodiceCategoria2Liv; ;
                }
                else
                {
                    SQLiteParameter pc2liv = new SQLiteParameter("@CodiceCategoria2Liv", _o.CodiceCategoria2Liv);
                    parColl.Add(pc2liv);
                }
                Dictionary<string, Offerte> prevnextcontent = offDM.CaricaPrevNextOfferte(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl);
                if (prevnextcontent != null)
                {
                    if (prevnextcontent.ContainsKey("prev") && prevnextcontent["prev"] != null)
                    {
                        string linkprev = CommonPage.CreaLinkRoutes(null, false, lingua, prevnextcontent["prev"].DenominazionebyLingua(lingua), prevnextcontent["prev"].Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv);
                        tmp.Add("prevlink", linkprev);
                        tmp.Add("prevlinktext", prevnextcontent["prev"].DenominazionebyLingua(lingua));

                    }
                    if (prevnextcontent.ContainsKey("next") && prevnextcontent["next"] != null)
                    {
                        string linknext = CommonPage.CreaLinkRoutes(null, false, lingua, prevnextcontent["next"].DenominazionebyLingua(lingua), prevnextcontent["next"].Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv);
                        tmp.Add("nextlink", linknext);
                        tmp.Add("nextlinktext", prevnextcontent["next"].DenominazionebyLingua(lingua));
                    }
                }
                /*****************************************************************************************************/

                List<string> imagescomplete = new List<string>();
                List<string> imagesdesc = new List<string>();
                List<string> imagesratio = new List<string>();
                List<string> filescomplete = new List<string>();
                List<string> filesdesc = new List<string>();


                if ((_o != null) && (_o.FotoCollection_M.Count > 0))
                {
                    foreach (Allegato a in _o.FotoCollection_M)
                    {
                        if ((a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                        {
                            //IMMAGINE
                            string tmppathimmagine = filemanage.ComponiUrlAnteprima(a.NomeFile, _o.CodiceTipologia, _o.Id.ToString(),true,true);
                            string abspathimmagine = tmppathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                            imagescomplete.Add(abspathimmagine);
                            //a.Descrizione -> dove la mettiamo
                            imagesdesc.Add(a.Descrizione);
                            try
                            {
                                using (System.Drawing.Image tmpimg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(tmppathimmagine)))
                                {
                                    imagesratio.Add(((double)tmpimg.Width / (double)tmpimg.Height).ToString());
                                }
                            }
                            catch
                            { imagesratio.Add("1"); }
                        }
                        else
                        {
                            //a.Descrizione -> dove la mettiamo
                            string tmppathimmagine = filemanage.ComponiUrlAnteprima(a.NomeFile, _o.CodiceTipologia, _o.Id.ToString(),true,true);
                            tmppathimmagine = tmppathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                            filescomplete.Add(tmppathimmagine);
                            filesdesc.Add(a.Descrizione);

                        }
                    }
                }

                if (!_o.Promozione)
                {
                    tmp.Add("imageslist", Newtonsoft.Json.JsonConvert.SerializeObject(imagescomplete));
                    tmp.Add("imagesdesc", Newtonsoft.Json.JsonConvert.SerializeObject(imagesdesc));
                    tmp.Add("imagesratio", Newtonsoft.Json.JsonConvert.SerializeObject(imagesratio));
                    tmp.Add("fileslist", Newtonsoft.Json.JsonConvert.SerializeObject(filescomplete));
                    tmp.Add("filesdesc", Newtonsoft.Json.JsonConvert.SerializeObject(filesdesc));
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////

            linksurl.Add(_o.Id.ToString(), tmp);
        }

        string retlinksurl = Newtonsoft.Json.JsonConvert.SerializeObject(linksurl);
        ritorno.Add("linkloaded", retlinksurl);

        return ritorno;
    }
    protected string GeneraBackLink(string tipologia, string categoria, string lingua, bool usacategoria = true)
    {
        string ret = "";
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == lingua && tmp.Codice == tipologia); });
        if (item != null)
        {
            string testourl = item.Descrizione;
            Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == lingua && (tmp.CodiceTipologia == tipologia && tmp.CodiceProdotto == categoria)); });
            if (catselected != null && usacategoria)
                testourl = catselected.Descrizione;
            string tmpcategoria = categoria;
            if (!usacategoria) tmpcategoria = "";
            ret = CommonPage.CreaLinkRoutes(null, false, lingua, (testourl), "", tipologia, tmpcategoria);
        }
        return ret;
    }

    public Dictionary<string, string> filterDataBanner(string lingua, Dictionary<string, string> filtriBanner, string spage, string spagesize, string senablepager)
    {
        bool enabledpager = false;
        bool.TryParse(senablepager, out enabledpager);

        int page = 0;
        int pagesize = 0;
        int maxelement = 0;
        int.TryParse(spage, out page);
        int.TryParse(spagesize, out pagesize);
        bool smescola = false;
        bool.TryParse(filtriBanner["mescola"], out smescola);
        Dictionary<string, string> ritorno = new Dictionary<string, string>();
        BannersCollection banners = new BannersCollection();
        string tblsezione = filtriBanner["tblsezione"];
        string filtrosezione = filtriBanner["filtrosezione"];
        int.TryParse(filtriBanner["maxelement"], out maxelement);
        bannersDM banDM = new bannersDM(tblsezione);

        banners = banDM.CaricaBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, filtrosezione.Trim(), smescola, maxelement);
        if (banners == null) banners = new BannersCollection();
        //dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, filtrosezione, mescola);

        //else
        //    offerte = filtri[4];

        List<Banners> filteredData = new List<Banners>();
        if (banners != null && banners.Count > 0 && enabledpager && page != 0 && pagesize != 0)
        {
            //Facciamo il take skip
            int start = ((page - 1) * pagesize);
            //int end = start + pagesize - 1;
            if (start + pagesize > banners.Count - 1)
                filteredData = banners.GetRange(start, banners.Count - start);
            else
                filteredData = banners.GetRange(start, pagesize).ToList();
        }
        else filteredData = banners;
        string tempOff = Newtonsoft.Json.JsonConvert.SerializeObject(filteredData, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
        });
        ritorno.Add("data", tempOff);
        Dictionary<string, string> ListRet = new Dictionary<string, string>();

        string tot = "0";
        if (banners != null) tot = banners.Count.ToString();
        ListRet.Add("totalrecords", tot);
        string tempListret = Newtonsoft.Json.JsonConvert.SerializeObject(ListRet);
        ritorno.Add("resultinfo", tempListret);


        Dictionary<string, Dictionary<string, string>> linksurl = new Dictionary<string, Dictionary<string, string>>();
        foreach (Banners _o in filteredData)
        {
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            string testotitolo = "";

            //_dr["ImageUrl"] = _ban.ImageUrlbyLingua(Lingua);
            //   _dr["NavigateUrl"] = _ban.NavigateUrlbyLingua(Lingua);
            //   _dr["AlternateText"] = _ban.AlternateTextbyLingua(Lingua);



            testotitolo = _o.AlternateTextbyLingua(lingua);
            string pathimmagine = _o.ImageUrlbyLingua(lingua);
            pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            if (string.IsNullOrEmpty(pathimmagine))
                pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            string target = "_self";
            string link = _o.NavigateUrlbyLingua(lingua);

            if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && link.ToLower().IndexOf("~") == -1)
            {
                target = "_self";
                link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
            }
            link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            tmp.Add("link", link);
            //tmp.Add("titolo", testotitolo);
            //tmp.Add("titolo", WelcomeLibrary.UF.Utility.SostituisciTestoACapo(CommonPage.ReplaceLinks(testotitolo)));
            tmp.Add("titolo", (CommonPage.ReplaceLinks(testotitolo)));
            tmp.Add("image", pathimmagine);

            linksurl.Add(_o.Id.ToString(), tmp);
        }

        string retlinksurl = Newtonsoft.Json.JsonConvert.SerializeObject(linksurl);
        ritorno.Add("linkloaded", retlinksurl);

        return ritorno;
    }


    //protected string ComponiUrlAnteprima(object NomeAnteprima, string CodiceTipologia, string idOfferta, bool noanteprima = false)
    //{
    //    string ritorno = "";
    //    string physpath = "";
    //    if (NomeAnteprima != null)
    //        if (!NomeAnteprima.ToString().ToLower().StartsWith("http://") && !NomeAnteprima.ToString().ToLower().StartsWith("https://"))
    //        {
    //            if (CodiceTipologia != "" && idOfferta != "")
    //            {
    //                //if ((NomeAnteprima.ToString().ToLower().EndsWith("jpg") || NomeAnteprima.ToString().ToLower().EndsWith("gif") || NomeAnteprima.ToString().ToLower().EndsWith("png")))
    //                //{
    //                ritorno = WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + CodiceTipologia + "/" + idOfferta.ToString();
    //                physpath = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + CodiceTipologia + "\\" + idOfferta.ToString();
    //                //Così ritorno l'immagine non di anteprima ma quella pieno formato
    //                if (NomeAnteprima.ToString().StartsWith("Ant"))
    //                    ritorno += "/" + NomeAnteprima.ToString().Remove(0, 3);
    //                else
    //                    ritorno += "/" + NomeAnteprima.ToString();
    //                //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME
    //                //string anteprimaimmagine = filemanage.ScalaImmagine(ritorno, null, physpath);
    //                //          if (anteprimaimmagine != "" && !noanteprima) ritorno = anteprimaimmagine;
    //                //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME

    //            }
    //            //    else
    //            //        ritorno = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";
    //            //}
    //        }
    //        else
    //            ritorno = NomeAnteprima.ToString();

    //    return ritorno;
    //}


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}

