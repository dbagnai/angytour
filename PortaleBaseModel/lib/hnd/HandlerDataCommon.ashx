<%@ WebHandler Language="C#" Class="HandlerDataCommon" %>

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
    public string jsontipologie { set; get; }
    public bool usecdn { set; get; }

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
            string Recs = pars.ContainsKey("r") ? pars["r"].ToLower() : "50";
            string progressivo = pars.ContainsKey("progressivo") ? pars["progressivo"] : "";
            string term = pars.ContainsKey("term") ? pars["term"].ToLower() : "";
            string id = pars.ContainsKey("id") ? pars["id"] : "";
            List<ResultAutocomplete> lra = new List<ResultAutocomplete>();
            int irecs = 0;


            string lingua = pars.ContainsKey("lng") ? pars["lng"] : "I";
            string filter1 = pars.ContainsKey("filter1") ? pars["filter1"] : "";
            string filter2 = pars.ContainsKey("filter2") ? pars["filter2"] : "";
            string Key = pars.ContainsKey("key") ? pars["key"] : "";
            string Value = pars.ContainsKey("value") ? pars["value"] : "";

            string objfiltro = pars.ContainsKey("objfiltro") ? pars["objfiltro"] : "";
            string page = pars.ContainsKey("page") ? pars["page"] : "1";
            string pagesize = pars.ContainsKey("pagesize") ? pars["pagesize"] : "10";
            string enablepager = pars.ContainsKey("enablepager") ? pars["enablepager"] : "true";
            Dictionary<string, string> res = new Dictionary<string, string>();

            switch (q)
            {
                case "autocompletecaratteristiche":
                    int.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 50;

                    int progr = 0;
                    int.TryParse(progressivo, out progr);
                    //if (progr == 0) progr = 5;
                    if (term != "null")
                    {
                        List<Tabrif> Caratteristica = new List<Tabrif>();
                        Caratteristica = references.FiltraCaratteristiche(progr, term, lingua);
                        int count = 0;

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
                    var percorsocontenutitmp = WelcomeLibrary.STATIC.Global.PercorsoContenuti.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);// CommonPage.ReplaceAbsoluteLinks("~" + ConfigManagement.ReadKey("DataDir") + "/Files");
                    var percorsocomunetmp = WelcomeLibrary.STATIC.Global.PercorsoComune.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione); ;// CommonPage.ReplaceAbsoluteLinks("~" + ConfigManagement.ReadKey("DataDir") + "/Common");
                    var jpathcomplete = new jpath();
                    jpathcomplete.percorsocomune = percorsocomunetmp;
                    jpathcomplete.percorsocontenuti = percorsocontenutitmp;
                    jpathcomplete.percorsoexp = WelcomeLibrary.STATIC.Global.percorsoexp;
                    jpathcomplete.usecdn = WelcomeLibrary.STATIC.Global.usecdn;
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

                    string linktmp = "";
                    WelcomeLibrary.DOM.TipologiaOfferte tipotmp = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == lingua && tmp.Codice == "rif000001"); });
                    if (tipotmp != null)
                        linktmp = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(tipotmp.Descrizione), "", tipotmp.Codice);
                    jpathcomplete.percorsolistadati = linktmp;

                    Dictionary<string, WelcomeLibrary.DOM.TabrifCollection> retdict = new Dictionary<string, WelcomeLibrary.DOM.TabrifCollection>();

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

                    //retdict.Add("JSONrefmetrature", references.refmetrature);
                    //retdict.Add("JSONrefprezzi", references.refprezzi);
                    //retdict.Add("JSONrefcondizione", references.refcondizione);
                    //retdict.Add("JSONreftipocontratto", references.reftipocontratto);
                    //retdict.Add("JSONreftiporisorse", references.reftiporisorse);
                    //retdict.Add("JSONgeogenerale", references.refgeogenerale);
                    retdict.Add("JSONcar1", Utility.Caratteristiche[0]);
                    retdict.Add("JSONcar2", Utility.Caratteristiche[1]);
                    retdict.Add("JSONcar3", Utility.Caratteristiche[2]);
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
                    Dictionary<string, string> filtri = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
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

        offerteDM offDM = new offerteDM();
        Dictionary<string, string> ritorno = new Dictionary<string, string>();
        OfferteCollection offerte = new OfferteCollection();
        if (!filtri.ContainsKey("listShow") || string.IsNullOrEmpty(filtri["listShow"]))
        {
            //offerte = (OfferteCollection)offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, filtri["tipologia"], filtri["maxelement"], false, lingua, false);
            //CARICO FILTRANDO ////////////////////////////////////////////////////////////////
            List<System.Data.OleDb.OleDbParameter> parColl = new List<System.Data.OleDb.OleDbParameter>();

            if (filtri.ContainsKey("id") && !string.IsNullOrEmpty(filtri["id"]))
            {
                System.Data.OleDb.OleDbParameter pid = new System.Data.OleDb.OleDbParameter("@Id", filtri["id"]);
                parColl.Add(pid);
            }
            if (filtri.ContainsKey("tipologia") && !string.IsNullOrEmpty(filtri["tipologia"]))
            {
                System.Data.OleDb.OleDbParameter p3 = new System.Data.OleDb.OleDbParameter("@CodiceTIPOLOGIA", filtri["tipologia"]);
                parColl.Add(p3);
            }
            if (filtri.ContainsKey("categoria") && !string.IsNullOrEmpty(filtri["categoria"]))
            {
                System.Data.OleDb.OleDbParameter p7 = new System.Data.OleDb.OleDbParameter("@CodiceCategoria", filtri["categoria"]);
                parColl.Add(p7);
            }

            if (filtri.ContainsKey("caratteristica1") && !string.IsNullOrEmpty(filtri["caratteristica1"]))
            {
                System.Data.OleDb.OleDbParameter pc1 = new System.Data.OleDb.OleDbParameter("@Caratteristica1", filtri["caratteristica1"]);
                parColl.Add(pc1);
            }
            if (filtri.ContainsKey("caratteristica2") && !string.IsNullOrEmpty(filtri["caratteristica2"]))
            {
                System.Data.OleDb.OleDbParameter pc2 = new System.Data.OleDb.OleDbParameter("@Caratteristica2", filtri["caratteristica2"]);
                parColl.Add(pc2);
            }
            if (filtri.ContainsKey("caratteristica3") && !string.IsNullOrEmpty(filtri["caratteristica3"]))
            {
                System.Data.OleDb.OleDbParameter pc3 = new System.Data.OleDb.OleDbParameter("@Caratteristica3", filtri["caratteristica3"]);
                parColl.Add(pc3);
            }

            if (filtri.ContainsKey("regione") && !string.IsNullOrEmpty(filtri["regione"]))
            {
                System.Data.OleDb.OleDbParameter preg = new System.Data.OleDb.OleDbParameter("@CodiceREGIONE", filtri["regione"]);
                parColl.Add(preg);
            }


            if (filtri.ContainsKey("vetrina") && !string.IsNullOrEmpty(filtri["vetrina"]))
            {
                bool _tmpb = false;
                bool.TryParse(filtri["vetrina"], out _tmpb);
                System.Data.OleDb.OleDbParameter pvet = new System.Data.OleDb.OleDbParameter("@Vetrina", _tmpb);
                parColl.Add(pvet);
            }
            if (filtri.ContainsKey("promozioni") && !string.IsNullOrEmpty(filtri["promozioni"]))
            {
                System.Data.OleDb.OleDbParameter promo = new System.Data.OleDb.OleDbParameter("@promozioni", filtri["promozioni"]);
                parColl.Add(promo);
            }
            if (filtri.ContainsKey("testoricerca") && !string.IsNullOrEmpty(filtri["testoricerca"]))
            {
                string testoricerca = filtri["testoricerca"].Trim().Replace(" ", "%");
                System.Data.OleDb.OleDbParameter p8 = new System.Data.OleDb.OleDbParameter("@testoricerca", "%" + testoricerca + "%");
                parColl.Add(p8);
            }
            string maxrecords = "";

            if (filtri.ContainsKey("maxelement") && !string.IsNullOrEmpty(filtri["maxelement"]))
                maxrecords = filtri["maxelement"];

            if (filtri.ContainsKey("mese") && !string.IsNullOrEmpty(filtri["mese"]))
                if (filtri.ContainsKey("anno") && !string.IsNullOrEmpty(filtri["anno"]))
                {
                    string mese = filtri["mese"];
                    string anno = filtri["anno"];

                    if (mese.Trim() != "" && anno.Trim() != "")
                    {
                        int _a = 0;
                        int.TryParse(anno, out _a);
                        int _m = 0;
                        int.TryParse(mese, out _m);
                        if (_a != 0)
                        {
                            System.Data.OleDb.OleDbParameter panno = new System.Data.OleDb.OleDbParameter("@annofiltro", _a);
                            parColl.Add(panno);
                        }
                        if (_m != 0)
                        {
                            System.Data.OleDb.OleDbParameter pmese = new System.Data.OleDb.OleDbParameter("@mesefiltro", _m);
                            parColl.Add(pmese);
                        }

                    }
                }
            offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, maxrecords, lingua);
        }
        //else
        //    offerte = filtri[4];



        List<Offerte> filteredData = new List<Offerte>();
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


        if (filtri.ContainsKey("maxelement") && !string.IsNullOrEmpty(filtri["maxelement"]))
        {
            int maxelem = 0;
            int.TryParse(filtri["maxelement"], out maxelem);
            if (maxelem < filteredData.Count())
                filteredData = filteredData.GetRange(0, maxelem);
        }

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
        if (offerte != null) tot = offerte.Count.ToString();
        ListRet.Add("totalrecords", tot);
        string tempListret = Newtonsoft.Json.JsonConvert.SerializeObject(ListRet);
        ritorno.Add("resultinfo", tempListret);

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
                    descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DescrizioneGB, 5000, true));
                    datitecnici = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DatitecniciGB, 5000, true));
                    break;
                default:
                    testotitolo = _o.DenominazioneI;
                    descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DescrizioneI, 5000, true));
                    datitecnici = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DatitecniciI, 5000, true));
                    break;
            }
            string pathimmagine = ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString());
            pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            if (string.IsNullOrEmpty(pathimmagine))
                pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            string target = "_self";
            string link = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(testotitolo), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria);

            if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && link.ToLower().IndexOf("~") == -1)
            {
                target = "_self";
                link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
            }
            link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            string titolo1 = testotitolo;
            string titolo2 = "<br/>";
            int i = testotitolo.IndexOf("\n");
            if (i != -1)
            {
                titolo1 = testotitolo.Substring(0, i);
                if (testotitolo.Length >= i + 1)
                    titolo2 = testotitolo.Substring(i + 1);
            }

            string contactlink = "";
            if (_o.Abilitacontatto) contactlink = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta&Lingua=" + lingua + "&idOfferta=" + _o.Id;
            string printlink = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/SchedaOffertaStampa.aspx?idOfferta=" + _o.Id + "&Lingua=" + lingua;
            string bcklink = GeneraBackLink(_o.CodiceTipologia, _o.CodiceCategoria, lingua);
            tmp.Add("contactlink", contactlink);
            tmp.Add("printlink", printlink);
            tmp.Add("bcklink", bcklink);


            tmp.Add("link", link);
            tmp.Add("titolo", testotitolo);
            tmp.Add("descrizione", descrizione);
            tmp.Add("datitecnici", datitecnici);
            tmp.Add("image", pathimmagine);
            tmp.Add("video", _o.linkVideo);

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
                        string tmppathimmagine = ComponiUrlAnteprima(a.NomeFile, _o.CodiceTipologia, _o.Id.ToString());
                        string abspathimmagine = tmppathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                        imagescomplete.Add(abspathimmagine);
                        //a.Descrizione -> dove la mettiamo
                        imagesdesc.Add(a.Descrizione);
                        try
                        {

                            using (System.Drawing.Image tmpimg = System.Drawing.Image.FromFile(   HttpContext.Current.Server.MapPath(tmppathimmagine)))
                            {
                                imagesratio.Add(  ((double) tmpimg.Width/(double) tmpimg.Height).ToString()  );
                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        //a.Descrizione -> dove la mettiamo
                        string tmppathimmagine = ComponiUrlAnteprima(a.NomeFile, _o.CodiceTipologia, _o.Id.ToString());
                        tmppathimmagine = tmppathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                        filescomplete.Add(tmppathimmagine);
                        filesdesc.Add(a.Descrizione);

                    }
                }
            }
            tmp.Add("imageslist", Newtonsoft.Json.JsonConvert.SerializeObject(imagescomplete));
            tmp.Add("imagesdesc", Newtonsoft.Json.JsonConvert.SerializeObject(imagesdesc));
            tmp.Add("imagesratio", Newtonsoft.Json.JsonConvert.SerializeObject(imagesratio));
            tmp.Add("fileslist", Newtonsoft.Json.JsonConvert.SerializeObject(filescomplete));
            tmp.Add("filesdesc", Newtonsoft.Json.JsonConvert.SerializeObject(filesdesc));


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

            //string titolo1 = testotitolo;
            //string titolo2 = "<br/>";
            //int i = testotitolo.IndexOf("\n");
            //if (i != -1)
            //{
            //    titolo1 = testotitolo.Substring(0, i);
            //    if (testotitolo.Length >= i + 1)
            //        titolo2 = testotitolo.Substring(i + 1);
            //}
            //string autore = _o.Autore;

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


    protected string ComponiUrlAnteprima(object NomeAnteprima, string CodiceTipologia, string idOfferta, bool noanteprima = false)
    {
        string ritorno = "";
        string physpath = "";
        if (NomeAnteprima != null)
            if (!NomeAnteprima.ToString().ToLower().StartsWith("http://") && !NomeAnteprima.ToString().ToLower().StartsWith("https://"))
            {
                if (CodiceTipologia != "" && idOfferta != "")
                {
                    //if ((NomeAnteprima.ToString().ToLower().EndsWith("jpg") || NomeAnteprima.ToString().ToLower().EndsWith("gif") || NomeAnteprima.ToString().ToLower().EndsWith("png")))
                    //{
                    ritorno = WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + CodiceTipologia + "/" + idOfferta.ToString();
                    physpath = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + CodiceTipologia + "\\" + idOfferta.ToString();
                    //Così ritorno l'immagine non di anteprima ma quella pieno formato
                    if (NomeAnteprima.ToString().StartsWith("Ant"))
                        ritorno += "/" + NomeAnteprima.ToString().Remove(0, 3);
                    else
                        ritorno += "/" + NomeAnteprima.ToString();
                    //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME
                    //string anteprimaimmagine = CommonPage.ScalaImmagine(ritorno, null, physpath);
                    //          if (anteprimaimmagine != "" && !noanteprima) ritorno = anteprimaimmagine;
                    //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME

                }
                //    else
                //        ritorno = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";
                //}
            }
            else
                ritorno = NomeAnteprima.ToString();

        return ritorno;
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}

