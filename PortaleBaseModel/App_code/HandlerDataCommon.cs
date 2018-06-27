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


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}

