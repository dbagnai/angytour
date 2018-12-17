using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

/// <summary>
/// Descrizione di riepilogo per references
/// </summary>
public class references
{

    private string _externalServer = "";
    public string ExternalServer
    {
        get { return _externalServer; }
        set { _externalServer = value; }
    }
    public string resourcesloaded = "";
    public string imgsprimary = "";
    public string imgscomplete = "";
    HttpServerUtility Server = null;


    public static string reftiporisorse = "";
    public static string reftipocontratto = "";
    public static string refprezzi = "";
    public static string refmetrature = "";
    public static string refcondizione = "";
    public static string reflanguages = "";


    public class jpath
    {
        public string percorsocontenuti { set; get; }
        public string percorsocomune { set; get; }
        public string percorsoapp { set; get; }
        public string percorsocdn { set; get; }
        public string percorsoimg { set; get; }
        public string percorsoexp { set; get; }
        public string percorsolistaimmobili { set; get; }
        public string percorsolistaristoranti { set; get; }
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

    public references(HttpServerUtility ServerPar)
    {
        Server = ServerPar;
    }

    public void CaricaDatiRisorseDaJson(string Lingua, string specificfile = "", bool usecdn = false)
    {
        //caricamento dati immobili da Json files
        var pathexp = "~" + WelcomeLibrary.STATIC.Global.percorsoexp;
        ExternalServer = WelcomeLibrary.STATIC.Global.percorsoapp;
        if (usecdn) ExternalServer = WelcomeLibrary.STATIC.Global.percorsocdn;

        string pathjsonfiles = WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione;
        if (Server == null)
        {
            pathjsonfiles += WelcomeLibrary.STATIC.Global.percorsoexp.Replace("/", "\\");
        }
        else
        {
            pathjsonfiles = Server.MapPath(pathexp);
        }



        //URL immagini ExternalServer + WelcomeLibrary.STATIC.Global.percorsoimg  + "/" + idallegati + "/" + nomefile


        if (string.IsNullOrEmpty(specificfile) || specificfile == "estates")
        {
            var filejsonimmobili = "estates.json";
            resourcesloaded = System.IO.File.ReadAllText(pathjsonfiles + filejsonimmobili);
        }

        if (string.IsNullOrEmpty(specificfile) || specificfile == "allegatiprimary")
        {
            var filejsonallprimary = "allegatiprimary" + Lingua + ".json";
            imgsprimary = System.IO.File.ReadAllText(pathjsonfiles + filejsonallprimary);

        }

        if (string.IsNullOrEmpty(specificfile) || specificfile == "allegati")
        {
            var filejsonallcomplete = "allegati" + Lingua + ".json";
            imgscomplete = System.IO.File.ReadAllText(pathjsonfiles + filejsonallcomplete);

        }

    }


    public static Dictionary<string, string> GetreftipologieValues(string Lingua)
    {
        Dictionary<string, string> value = new Dictionary<string, string>();

        //reftipologie
        dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(reftiporisorse);
        foreach (dynamic r in jsonResponse)
        {
            string idact = r.id;
            //Seleziono per id

            foreach (dynamic c in r.dettaglitiporisorse.Children())
            {
                if (c.lingua == Lingua)
                {
                    string titolo = c.titolo;
                    string id = idact;
                    if (!value.ContainsKey(idact))
                        value.Add(idact, titolo);
                    break;
                }
            }

        }
        return value;
    }

    public static string GetreftipologieValueById(string id, string Lingua)
    {
        string value = "";

        //reftipologie
        dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(reftiporisorse);
        foreach (dynamic r in jsonResponse)
        {
            string idact = r.id;

            //Seleziono per id
            if (idact == id)
            {

                foreach (dynamic c in r.dettaglitiporisorse.Children())
                {
                    if (c.lingua == Lingua)
                    {
                        value = c.titolo;
                        break;
                    }
                }
                break;
            }
        }
        return value;
    }
    public static Dictionary<string, List<string>> CreaFilteredResourcesLinks(HttpContext context, string objfiltro, string Lingua, string tipologia, int maxresults = 20)
    {

        string percorsoBase = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
        string PathSitemap = WelcomeLibrary.STATIC.Global.percorsoFisicoComune;
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        references references = new references(context.Server);
        if (!string.IsNullOrEmpty(objfiltro))
        {
            parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
        }
        var codn = parameters.ContainsKey("ddlNazioneSearch") ? parameters["ddlNazioneSearch"] : "";
        var codr = parameters.ContainsKey("ddlRegioneSearch") ? parameters["ddlRegioneSearch"] : "";
        var codp = parameters.ContainsKey("ddlProvinciaSearch") ? parameters["ddlProvinciaSearch"] : "";
        var codc = parameters.ContainsKey("ddlComuneSearch") ? parameters["ddlComuneSearch"] : "";
        var idcontratto = parameters.ContainsKey("ddlContrattoSearch") ? parameters["ddlContrattoSearch"] : "";
        var idcondizione = parameters.ContainsKey("ddlCondizioneSearch") ? parameters["ddlCondizioneSearch"] : "";
        var idtipologia = parameters.ContainsKey("ddlTipologiaSearch") ? parameters["ddlTipologiaSearch"] : "";
        var idprezzi = parameters.ContainsKey("ddlPrezziSearch") ? parameters["ddlPrezziSearch"] : "";
        var idmetrature = parameters.ContainsKey("ddlMetratureSearch") ? parameters["ddlMetratureSearch"] : "";
        var vetrina = parameters.ContainsKey("vetrina") ? parameters["vetrina"] : "";
        var pmin = 0; var pmax = 0;
        var mmin = 0; var mmax = 0;
        if (!string.IsNullOrEmpty(idprezzi))
        {
            //var filterbase = "{ \"data\":" + references.refprezzi;
            //filterbase += "}";
            dynamic jsonPrezzi = Newtonsoft.Json.JsonConvert.DeserializeObject(references.refprezzi);
            foreach (dynamic r in jsonPrezzi)
            {
                if (r.id == idprezzi)
                {
                    foreach (dynamic c in r.dettagliprezzi.Children())
                    {
                        if (c.lingua == "I")
                        {
                            string selectvalue = c.descrizione;
                            if (selectvalue != null && selectvalue.Length > 0)
                            {
                                var pfascia = selectvalue.ToString().Split(';');
                                if (pfascia != null && pfascia.Length == 2)
                                {
                                    string min = pfascia[0];
                                    string max = pfascia[1];
                                    int.TryParse(min, out pmin);
                                    int.TryParse(max, out pmax);
                                }
                            }
                        }
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(idmetrature))
        {
            //var filterbase = "{ \"data\":" + references.refprezzi;
            //filterbase += "}";
            dynamic jsonMetrature = Newtonsoft.Json.JsonConvert.DeserializeObject(references.refmetrature);
            foreach (dynamic r in jsonMetrature)
            {
                if (r.id == idmetrature)
                {
                    foreach (dynamic c in r.dettaglimetrature.Children())
                    {
                        if (c.lingua == "I")
                        {
                            string selectvalue = c.descrizione;

                            if (selectvalue != null && selectvalue.Length > 0)
                            {
                                var pfascia = selectvalue.ToString().Split(';');
                                if (pfascia != null && pfascia.Length == 2)
                                {
                                    string min = pfascia[0];
                                    string max = pfascia[1];
                                    int.TryParse(min, out mmin);
                                    int.TryParse(max, out mmax);
                                }
                            }
                        }
                    }
                }
            }
        }


        dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(references.reflanguages);

        //List<string> linklist = new List<string>();
        Dictionary<string, List<string>> retdict = new Dictionary<string, List<string>>();
        //Fare ciclo sugli immobili
        references.CaricaDatiRisorseDaJson(Lingua, "", WelcomeLibrary.STATIC.Global.usecdn);
        dynamic jsonResources = Newtonsoft.Json.JsonConvert.DeserializeObject(references.resourcesloaded);
        string idallegati = "";
        foreach (dynamic r in jsonResources)
        {
            string idact = r.id;
            idallegati = r.id_allegati;
            bool esito = true;
            if (r.pubblicasito != true)
                esito = false;
            /*NAZIONE*/
            if (codn != null && codn != "" && esito)
                if (r.codiceNAZIONE != codn) esito = false;
            /*REGIONE*/
            if (codr != null && codr != "" && esito)
                if (r.codiceREGIONE != codr) esito = false;
            /*PROVINCIA*/
            if (codp != null && codp != "" && esito)
                if (r.codicePROVINCIA != codp) esito = false;

            /*COMUNE*/
            if (codc != null && codc != "" && esito)
                if (r.codiceCOMUNE != codc) esito = false;

            /*Contratto*/
            if (idcontratto != null && idcontratto != "" && esito)
                if (r.idcontratto != idcontratto) esito = false;


            /*Tipologia*/
            if (idtipologia != null && idtipologia != "" && esito)
                if (r.idtipologia != idtipologia) esito = false;

            /*Condizione*/
            if (idcondizione != null && idcondizione != "" && esito)
                if (r.idcondizione != idcondizione) esito = false;

            //nascondiprezzo -> devo visualizzare riservato nel prezzo che visualizzo
            if (pmin != 0 && pmax != 0 && esito)
                if (!(r.Prezzo1 >= pmin && r.Prezzo1 <= pmax)) esito = false;

            if (mmin != 0 && mmax != 0 && esito)
                if (!(r.Superficie1 >= mmin && r.Superficie1 <= mmax)) esito = false;

            bool bvetrina = false;
            bool.TryParse(vetrina, out bvetrina);
            if (vetrina != "" && esito)
                if (r.vetrina != vetrina) esito = false;

            if (esito)
            {
                string testotitolo = "";
                string link = "";
                foreach (dynamic c in r.dettagliorisorse_1.Children())
                {
                    if (c.lingua == Lingua)
                    {
                        testotitolo = c.titolo;
                        //= c.descrizione;
                        break;
                    }
                }
                //Creaimo la lista dei link
                int count = 0;
                if ((!string.IsNullOrEmpty(testotitolo)))
                {

                    //bool upd = false;
                    //bool.TryParse(ConfigManagement.ReadKey("updateTableurlrewriting"), out upd);

                    link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, testotitolo, idact, tipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
                    //linklist.Add(link);
                    if (!retdict.ContainsKey(idact))
                    {
                        List<string> tmp = new List<string>();
                        tmp.Add(testotitolo);
                        tmp.Add(link);
                        retdict.Add(idact, tmp);
                        count++;

                        if (count > maxresults) break;

                    }
                }
            }
        }
        return retdict;

    }
    public static void CaricaDatiReftablesDaJson(HttpServerUtility Server)
    {
        var pathexp = "~" + WelcomeLibrary.STATIC.Global.percorsoexp;
        string pathjsonfiles = WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione;

        if (Server == null)
        {
            pathjsonfiles += WelcomeLibrary.STATIC.Global.percorsoexp.Replace("/", "\\");
        }
        else
        {
            pathjsonfiles = Server.MapPath(pathexp);
        }

        var filejsontipologie = "reftiporisorse.json";
        if (System.IO.File.Exists(pathjsonfiles + filejsontipologie))
            reftiporisorse = System.IO.File.ReadAllText(pathjsonfiles + filejsontipologie);
        ///INTEGREARE LE ALTRE TABELLE REF A LINGUA COMUNE...................
        ///
        var filejsoncontratti = "reftipocontratto.json";
        if (System.IO.File.Exists(pathjsonfiles + filejsoncontratti))
            reftipocontratto = System.IO.File.ReadAllText(pathjsonfiles + filejsoncontratti);

        var filejsonprezzi = "refprezzi.json";
        if (System.IO.File.Exists(pathjsonfiles + filejsonprezzi))
            refprezzi = System.IO.File.ReadAllText(pathjsonfiles + filejsonprezzi);

        var filejsonrefmetrature = "refmetrature.json";
        if (System.IO.File.Exists(pathjsonfiles + filejsonrefmetrature))
            refmetrature = System.IO.File.ReadAllText(pathjsonfiles + filejsonrefmetrature);

        var filejsonrefcondizione = "refcondizione.json";
        if (System.IO.File.Exists(pathjsonfiles + filejsonrefcondizione))
            refcondizione = System.IO.File.ReadAllText(pathjsonfiles + filejsonrefcondizione);

        var filejsonlanguages = "languages.json";
        //reflanguages = System.IO.File.ReadAllText(Server.MapPath("~/lib/cfg/" + filejsonlanguages));
        //if (System.IO.File.Exists(pathjsonfiles + filejsonlanguages))
        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\cfg\\" + filejsonlanguages))
            reflanguages = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\cfg\\" + filejsonlanguages);

    }

    public static System.Globalization.CultureInfo setCulture(string lng)
    {
        string culturename = "";
        switch (lng)
        {
            case "I":
                culturename = "it";
                break;
            case "GB":
                culturename = "en";
                break;
            case "RU":
                culturename = "ru";
                break;
            default:
                culturename = "it";
                break;
        }
        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culturename);
        return ci;
    }
    public static Dictionary<string, Dictionary<string, string>> GetResourcesByLingua(string lingua = "I")
    {
        Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();

        List<ResourceItem> lri = ResourceManagement.ReadItemsByLingua("BaseText", lingua);
        foreach (var r in lri)
        {
            if (!dict.ContainsKey(lingua))
                dict.Add(lingua, new Dictionary<string, string>());
            if (!dict[lingua].ContainsKey(r.Chiave))
            {
                dict[lingua].Add(r.Chiave, CommonPage.ReplaceLinks(r.Valore));
            }

        }

        return dict;
    }
    public static void RegenerateUrlSearchLinks(HttpServerUtility Server, string tipologia)
    {
        List<string> linktipologielist = new List<string>();
        List<string> linktipologieregionilist = new List<string>();
        StringBuilder sb1 = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();

        dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(references.reflanguages);
        foreach (dynamic l in jsonResponse)
        {
            //Link per letipologie immobiliari
            string Lingua = l.Value.Path;
            WelcomeLibrary.DOM.ProvinceCollection regioni = ListaRegioni(Lingua);
            TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == tipologia); });
            if (item != null)
            {
                Dictionary<string, string> tipologieimmobili = references.GetreftipologieValues(Lingua);
                foreach (KeyValuePair<string, string> t in tipologieimmobili)
                {
                    sb1 = new StringBuilder();
                    sb1.Append(item.Descrizione);
                    sb1.Append("-");
                    sb1.Append(t.Value);
                    //string testourl = item.Descrizione + "-" + t.Value;
                    //string testolink = t.Value;
                    string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, sb1.ToString(), "", tipologia, t.Key, "", "", "", "", true, true);
                    //linktipologielist.Add(link);
                    //Link per tipologie immobiliari e regioni
                    if (regioni != null)
                        foreach (WelcomeLibrary.DOM.Province r in regioni)
                        {
                            sb2 = new StringBuilder();
                            sb2.Append(sb1.ToString());
                            sb2.Append("-");
                            sb2.Append(r.Regione);
                            //string testourl2 = testourl + "-" + r.Regione;
                            link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, sb2.ToString(), "", tipologia, t.Key, "", r.Codice, "", "", true, true);
                            //link per regione immobile
                            link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, item.Descrizione + "-" + r.Regione, "", tipologia, "", "", r.Codice, "", "", true, true);
                            //linktipologieregionilist.Add(link);
                        }
                }
            }
        }
    }

    public static void CreaSitemapImmobili(HttpServerUtility Server, string tipologia)
    {
        //Per prima cosa rigenero i link per le ricerche tipologia e tipologia / regione
        RegenerateUrlSearchLinks(Server, tipologia);

        string percorsoBase = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
        string PathSitemap = WelcomeLibrary.STATIC.Global.percorsoFisicoComune;
        string host = percorsoBase.Replace(".", "");
        host = host.Replace(":", "");
        host = host.Replace("/", "");
        references references = new references(Server);
        dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(references.reflanguages);
        foreach (dynamic l in jsonResponse)
        {

            string Lingua = l.Value.Path;
#if true

            List<string> linklist = new List<string>();
            //Fare ciclo sugli immobili
            references.CaricaDatiRisorseDaJson(Lingua, "", WelcomeLibrary.STATIC.Global.usecdn);
            dynamic jsonResources = Newtonsoft.Json.JsonConvert.DeserializeObject(references.resourcesloaded);
            string idallegati = "";
            foreach (dynamic r in jsonResources)
            {
                string idact = r.id;
                idallegati = r.id_allegati;
                string testotitolo = "";
                foreach (dynamic c in r.dettagliorisorse_1.Children())
                {
                    if (c.lingua == Lingua)
                    {
                        testotitolo = c.titolo;
                        //= c.descrizione;
                        break;
                    }
                }
                //Creaimo la lista dei link
                string link = "";
                if ((!string.IsNullOrEmpty(testotitolo)))
                {
                    link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, testotitolo, idact, tipologia, "", "", "", "", "", true, true);
                    linklist.Add(link);
                }
            }
            references.CreazioneSitemap("sitemapLinkResources" + Lingua + host, PathSitemap, linklist, System.DateTime.Today.ToString("yyyy-MM-dd"), "monthly", "1");
#endif

        }
    }
    public static void CreazioneSitemap(string NomeSitemap, string PathCartellaDestinazione, List<string> ListaLink, string Lastmod, string Changefreq, string Priority)
    {
        //Funzione che mi crea il file sitemap.xml in formato base
        System.IO.FileStream SitMap = new System.IO.FileStream(PathCartellaDestinazione + "\\" + NomeSitemap + ".xml", System.IO.FileMode.Create);
        using (SitMap)
        {

            //Inizio a scrivere il file
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(SitMap, Encoding.Default);
            writer.Formatting = System.Xml.Formatting.Indented;

            // aggiungo l'intestazione XML 
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>");
            //writer.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

            //Qui ci metto l'apertura dell'elemento
            writer.WriteStartElement("urlset");
            writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

            //Adesso si ripete per ogni elemento della lista
            if (ListaLink != null)
                foreach (string file in ListaLink)
                {
                    if (string.IsNullOrEmpty(file)) continue;
                    //Apro l'elemento interno URL
                    writer.WriteStartElement("url");
                    //Apro l'elemento loc
                    writer.WriteStartElement("loc");
                    writer.WriteRaw(file);
                    writer.WriteEndElement();
                    //Apro l'elemento lastmod
                    writer.WriteStartElement("lastmod");
                    writer.WriteRaw(Lastmod);
                    writer.WriteEndElement();
                    //apro l'elemento changefreq
                    writer.WriteStartElement("changefreq");
                    writer.WriteRaw(Changefreq);
                    writer.WriteEndElement();
                    //Apro lelemento priority
                    writer.WriteStartElement("priority");
                    writer.WriteRaw(Priority);
                    writer.WriteEndElement();
                    //Chiudo l'url
                    writer.WriteEndElement();
                }

            //Chiudo l'elemento 
            writer.WriteEndElement();

            //// scrivo a video e chiudo lo stream 
            writer.Flush();
            writer.Close();
            SitMap.Close();
        }
    }


    public static void CaricaMemoriaStatica(HttpServerUtility Server)
    {
        WelcomeLibrary.STATIC.Global.NomeConnessioneDb = "dbdataaccess";

        WelcomeLibrary.UF.ConfigManagement.LoadConfig();
        WelcomeLibrary.UF.ResourceManagement.LoadResources();

        //MEMORIZZO I VALORI dei PERCORSI FISICI DELL APPLICAZIONE
        if (ConfigManagement.ReadKey("Posizione") == "Remoto")
        {
            WelcomeLibrary.STATIC.Global.PercorsoContenuti = "~" + ConfigManagement.ReadKey("DataDir") + "/Files";
            WelcomeLibrary.STATIC.Global.PercorsoComune = "~" + ConfigManagement.ReadKey("DataDir") + "/Common";
            WelcomeLibrary.STATIC.Global.percorsoFisicoComune = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoComune);
            WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoContenuti);
        }
        WelcomeLibrary.UF.MemoriaDisco.physiclogdir = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoComune);
        WelcomeLibrary.STATIC.Global.percorsoapp = ConfigManagement.ReadKey("percorsoapp");
        WelcomeLibrary.STATIC.Global.percorsocdn = ConfigManagement.ReadKey("percorsocdn");
        WelcomeLibrary.STATIC.Global.percorsoimg = ConfigManagement.ReadKey("percorsoimg");
        WelcomeLibrary.STATIC.Global.percorsoexp = ConfigManagement.ReadKey("percorsoexp");
        WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione = Server.MapPath("~");
        WelcomeLibrary.STATIC.Global.versionforcache = ConfigManagement.ReadKey("versionforcache");

        bool upd = false;
        bool.TryParse(ConfigManagement.ReadKey("updateTableurlrewriting"), out upd);
        WelcomeLibrary.STATIC.Global.UpdateUrl = upd;
        //------------------------------------------------------------------------------------------------

        WelcomeLibrary.STATIC.Global.usecdn = false;
        string susecdn = ConfigManagement.ReadKey("usecdn");
        bool tmpcdn = false;
        bool.TryParse(susecdn, out tmpcdn);
        WelcomeLibrary.STATIC.Global.usecdn = tmpcdn;


        references.CaricaDatiReftablesDaJson(Server); //Tabelle di riferimento per l'immobiliare

        WelcomeLibrary.UF.Utility.CaricaListaStaticaNazioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, true);
        //WelcomeLibrary.UF.Utility.Nazioni.RemoveAll(n => !(n.Codice == "IT" || n.Codice == "XX"));

        WelcomeLibrary.UF.Utility.CaricaListaStaticaProvince(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        //List<WelcomeLibrary.DOM.Province> province1 = WelcomeLibrary.UF.Utility.ElencoProvince.FindAll(delegate(WelcomeLibrary.DOM.Province tmp) { return (tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == "it"); });
        //_tmp = WelcomeLibrary.UF.Utility.ElencoProvince.Find(delegate(WelcomeLibrary.DOM.Province tmp) { return (tmp.Codice == "p94"); });
        //List<WelcomeLibrary.DOM.Province> province2 = WelcomeLibrary.UF.Utility.ElencoProvince.FindAll(delegate(WelcomeLibrary.DOM.Province tmp) { return (tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == "it"); });
        WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.AddRange(WelcomeLibrary.UF.Utility.ElencoProvince);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaComuni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);

        WelcomeLibrary.UF.Utility.CaricaMemoriaStaticaCaratteristiche("");
        //CRICHIAMO LE LISTE STATICHE TIPOLOGIEANNUNCI, TIPOLOGIECONTENUTI,TIPOLOGIEOFFERTE,ELENCOPROVINCE,ELENCOCOMUNI
        //  WelcomeLibrary.UF.Utility.CaricaListaStaticaTipologieAnnunci(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaTipologieOfferte(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaTipologieContenuti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaTipiClienti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);


        //Parametri di ricerca ( Fasce di prezzo e parametri generici tipo1 e tipo2 )
        WelcomeLibrary.UF.Utility.CaricaListaStaticaFascediprezzo(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaParametro1(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaParametro2(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);

        //WelcomeLibrary.UF.Utility.CaricaListaStaticaTipologieAttivita(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);

        //CARICA LA LISTA DELLE TIPOLOGIE E DELLE PROVINCE IN UNA MEMORIA STATICA PER RIUTILIZZO
        //GELibraryRemoto.UF.FunzioniUtilità.CreaListaStaticaTipologieDaFileXML(Server.MapPath("~" + ConfigManagement.ReadKey("DataDir"].ToString() + "/Common/" + "tipologie.xml"));
        //GELibraryRemoto.UF.FunzioniUtilità.CreaListaStaticaProvinceDaFileXML(Server.MapPath("~" + ConfigManagement.ReadKey("DataDir"].ToString() + "/Common/" + "province.xml"));

        //WelcomeLibrary.DAL.bookingDM.InitVincoli(); //Vincoli per il book , usa quelli preimpostati!!!

    }

    /// <summary>
    /// Serializza un oggetto con tutte le variabili base ad uso del javascript
    /// </summary>
    /// <param name="lingua"></param>
    /// <param name="loggedusername"></param>
    /// <returns></returns>
    public static string initreferencesdataserialized(string lingua,string loggedusername)
    {
        var jpathcomplete = new references.jpath();
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
        jpathcomplete.username = loggedusername;
        //LINGUE/////////////////////////////////////////////////////7
        var filejsonlanguages = "languages.json";
        //reflanguages = System.IO.File.ReadAllText(Server.MapPath("~/lib/cfg/" + filejsonlanguages));
        var reflanguages = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\cfg\\" + filejsonlanguages).Replace("\r\n","").Replace("\n", "").Replace("\r", "");
        jpathcomplete.jsonlanguages = reflanguages;// Newtonsoft.Json.JsonConvert.SerializeObject(reflanguages); 
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


        /*Per ristoranti*/
        linktmp = "";
        WelcomeLibrary.DOM.TipologiaOfferte tipotmp1 = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == lingua && tmp.Codice == "rif000003"); });
        if (tipotmp1 != null)
            linktmp = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(tipotmp1.Descrizione), "", tipotmp1.Codice);

        jpathcomplete.percorsolistaristoranti = linktmp;
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
        jpathcomplete.dictreferences = Newtonsoft.Json.JsonConvert.SerializeObject(retdict, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
        });

        return  Newtonsoft.Json.JsonConvert.SerializeObject(jpathcomplete);

    }


    public static string ResMan(string Gruppo, string Lingua, string Chiave, string Categoria = "")
    {
        return WelcomeLibrary.UF.ResourceManagement.ReadKey(Gruppo, Lingua, Chiave, Categoria).Valore;
    }

    public static string NomeRegione(string codiceprovincia, string Lingua)
    {
        string ritorno = "";
        Province item = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codiceprovincia); });
        if (item != null)
            ritorno = item.Regione;
        return ritorno;
    }
    public static string NomeProvincia(string codiceprovincia, string Lingua)
    {
        string ritorno = "";
        Province item = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codiceprovincia); });
        if (item != null)
            ritorno = item.Provincia;
        return ritorno;
    }
    public static WelcomeLibrary.DOM.ProvinceCollection ListaRegioni(string Lingua)
    {
        WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
        List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == Lingua); });
        if (provincelingua != null)
        {
            provincelingua.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
            foreach (Province item in provincelingua)
            {
                if (item.Lingua == Lingua)
                    if (!regioni.Exists(delegate (Province tmp) { return (tmp.Regione == item.Regione); }))
                        regioni.Add(item);
            }
        }
        return regioni;
    }
    public static string TrovaCodiceRegione(string nomeregione, string Lingua)
    {
        string ritorno = "";
        if (string.IsNullOrEmpty(nomeregione)) return ritorno;
        WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
        List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == Lingua); });
        if (provincelingua != null)
        {
            provincelingua.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
            foreach (Province item in provincelingua)
            {
                if (item.Lingua == Lingua)
                    if (!regioni.Exists(delegate (Province tmp) { return (tmp.Regione == item.Regione); }))
                        regioni.Add(item);
            }
        }
        Province reg = regioni.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Regione.ToLower().Contains(nomeregione.ToLower().Trim())); });
        if (reg != null)
            ritorno = reg.Codice;
        return ritorno;
    }
    public static string TrovaCodiceProvincia(string nomeprovincia, string Lingua)
    {
        string ritorno = "";
        if (string.IsNullOrEmpty(nomeprovincia)) return ritorno;
        Province item = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Provincia.ToLower().Contains(nomeprovincia.ToLower().Trim())); });
        if (item != null)
            ritorno = item.Codice;
        return ritorno;
    }
    public static string TrovaCodiceNazione(string nomenazione, string Lingua)
    {
        string ritorno = "";
        if (string.IsNullOrEmpty(nomenazione)) return ritorno;
        Tabrif item = Utility.Nazioni.Find(delegate (Tabrif tmp) { return (tmp.Lingua == Lingua && tmp.Campo1.ToLower().Contains(nomenazione.ToLower().Trim())); });
        if (item != null)
            ritorno = item.Codice;
        return ritorno;
    }
    public static double TrovaCostoNazione(string codice, string Lingua = "I")
    {
        double ritorno = 0;
        if (string.IsNullOrEmpty(codice)) return ritorno;
        Tabrif item = WelcomeLibrary.UF.Utility.Nazioni.Find(n => n.Codice == codice && n.Lingua == Lingua);
        if (item != null)
            ritorno = item.Double1;
        return ritorno;
    }
    public static string TestoTipologia(string codicetipologia, string Lingua)
    {
        string retstr = "";
        TipologiaOfferte tipo = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte _p) { return _p.Codice == codicetipologia && _p.Lingua == Lingua; });
        if (tipo != null)
        {
            retstr = tipo.Descrizione;
        }
        return retstr;
    }
    public static string TestoCategoria(string codicetipologia, string codicecategoria, string Lingua)
    {
        string retstr = "";
        Prodotto categoria = Utility.ElencoProdotti.Find(delegate (Prodotto _p) { return _p.CodiceProdotto == codicecategoria && _p.CodiceTipologia == codicetipologia && _p.Lingua == Lingua; });
        if (categoria != null)
        {
            retstr = categoria.Descrizione;
        }
        return retstr;
    }
    public static string TestoCategoria2liv(string codicetipologia, string codicecategoria, string codicecategoria2liv, string Lingua)
    {
        string retstr = "";
        //Prodotto categoria = Utility.ElencoProdotti.Find(delegate(Prodotto _p) { return _p.CodiceProdotto == codicecategoria && _p.CodiceTipologia == codicetipologia && _p.Lingua == Lingua; });
        //if (categoria != null)
        //{
        //    retstr = categoria.Descrizione;
        //}
        SProdotto categoria2liv = Utility.ElencoSottoProdotti.Find(delegate (SProdotto _p) { return _p.CodiceProdotto == codicecategoria && _p.CodiceSProdotto == codicecategoria2liv && _p.Lingua == Lingua; });
        if (categoria2liv != null)
        {
            retstr += " " + categoria2liv.Descrizione;
        }

        return retstr;
    }
    public static string TestoCaratteristica(int progressivocaratteristica, string codice, string Lingua)
    {
        string retstr = "";
        Tabrif Caratteristica = Utility.Caratteristiche[progressivocaratteristica].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == codice; });
        if (Caratteristica != null)
        {
            retstr = Caratteristica.Campo1;
        }
        return retstr;
    }
    public static string TestoCaratteristicaJson(string idJson, string codeJson, string Lingua)
    {
        string retstr = "";
        if (!string.IsNullOrEmpty(codeJson))
        {
            List<ModelCarCombinate> listCar = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(codeJson);
            if (listCar != null && listCar.Count > 0)
            {
                ResultAutocomplete car1 = new ResultAutocomplete();
                ResultAutocomplete car2 = new ResultAutocomplete();
                foreach (ModelCarCombinate item in listCar)
                {
                    if (item.id == idJson)
                    {
                        car1 = item.caratteristica1;
                        car2 = item.caratteristica2;
                    }
                }

                string testoinlinguacar1 = car1.value;
                Tabrif Car1mem = Utility.Caratteristiche[0].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == car1.codice; });
                if (Car1mem != null)
                    testoinlinguacar1 = Car1mem.Campo1;

                string testoinlinguacar2 = car2.value;
                Tabrif Car2mem = Utility.Caratteristiche[1].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == car2.codice; });
                if (Car2mem != null)
                    testoinlinguacar2 = Car2mem.Campo1;

                retstr = "  -  " + testoinlinguacar1 + "  -  " + testoinlinguacar2;
            }
        }
        return retstr;
    }
    public static List<Tabrif> FiltraCaratteristiche(int progr, string term, string lingua = "I")
    {
        List<Tabrif> Caratteristica = new List<Tabrif>();
        if (Utility.Caratteristiche[progr] != null && Utility.Caratteristiche[progr].Count > 0) //Query in memoria
        {
            Caratteristica = Utility.Caratteristiche[progr].FindAll(c => c.Campo1.ToLower().Contains(term.ToLower()) && c.Lingua == lingua);
        }
        //else //Query sul db se in memoria non presenti valori
        //{
        //    switch (progr.ToString())
        //    {
        //        case "0":
        //            Caratteristica = Utility.CaricaListaCaratteristicaFiltrata("", "dbo_TBLRIF_Caratteristica1", term.ToLower(), lingua);
        //            break;
        //        case "1":
        //            Caratteristica = Utility.CaricaListaCaratteristicaFiltrata("", "dbo_TBLRIF_Caratteristica2", term.ToLower(), lingua);
        //            break;
        //        case "2":
        //            Caratteristica = Utility.CaricaListaCaratteristicaFiltrata("", "dbo_TBLRIF_Caratteristica3", term.ToLower(), lingua);
        //            break;
        //        case "3":
        //            Caratteristica = Utility.CaricaListaCaratteristicaFiltrata("", "dbo_TBLRIF_Caratteristica4", term.ToLower(), lingua);
        //            break;
        //        case "4":
        //            Caratteristica = Utility.CaricaListaCaratteristicaFiltrata("", "dbo_TBLRIF_Caratteristica5", term.ToLower(), lingua);
        //            break;
        //        case "5":
        //            Caratteristica = Utility.CaricaListaCaratteristicaFiltrata("", "dbo_TBLRIF_Caratteristica6", term.ToLower(), lingua);
        //            break;
        //    }
        //}
        return Caratteristica;
    }
}

