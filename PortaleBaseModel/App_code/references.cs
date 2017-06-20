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
                if ((!string.IsNullOrEmpty(testotitolo)))
                {

                    //bool upd = false;
                    //bool.TryParse(ConfigurationManager.AppSettings["updateTableurlrewriting"], out upd);

                    link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, testotitolo, idact, tipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
                    //linklist.Add(link);
                    if (!retdict.ContainsKey(idact))
                    {
                        List<string> tmp = new List<string>();
                        tmp.Add(testotitolo);
                        tmp.Add(link);
                        retdict.Add(idact, tmp);
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
        if (System.IO.File.Exists(pathjsonfiles + filejsonlanguages))
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
        System.Globalization.CultureInfo ci = setCulture(lingua);
        Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
        //Ricerca con ResourceManager creando un metodo che trova tutti i link che contengono
        //le tipologie in questione
        System.Resources.ResourceSet rset2 = Resources.Basetext.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
        // Create an IDictionaryEnumerator to read the data in the ResourceSet.
        System.Collections.IDictionaryEnumerator id2 = rset2.GetEnumerator();
        // Iterate through the ResourceSet and display the contents to the console. 

        while (id2.MoveNext())
        {
            if (!dict.ContainsKey(lingua))
                dict.Add(lingua, new Dictionary<string, string>());
            if (!dict[lingua].ContainsKey(id2.Key.ToString()))
            {
                dict[lingua].Add(id2.Key.ToString(), HttpContext.GetGlobalResourceObject("Basetext", id2.Key.ToString(), ci).ToString());
            }
        }
        id2.Reset();
        return dict;
    }

    public static void CreaSitemapImmobili(HttpServerUtility Server, string tipologia)
    {

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
                    link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, testotitolo, idact, tipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
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

        //MEMORIZZO I VALORI dei PERCORSI FISICI DELL APPLICAZIONE
        if (ConfigurationManager.AppSettings["Posizione"].ToString() == "Remoto")
        {
            WelcomeLibrary.STATIC.Global.PercorsoContenuti = "~" + ConfigurationManager.AppSettings["DataDir"].ToString() + "/Files";
            WelcomeLibrary.STATIC.Global.PercorsoComune = "~" + ConfigurationManager.AppSettings["DataDir"].ToString() + "/Common";
            WelcomeLibrary.STATIC.Global.percorsoFisicoComune = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoComune);
            WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoContenuti);
        }
        WelcomeLibrary.UF.MemoriaDisco.physiclogdir = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoComune);
        WelcomeLibrary.STATIC.Global.percorsoapp = ConfigurationManager.AppSettings["percorsoapp"].ToString();
        WelcomeLibrary.STATIC.Global.percorsocdn = ConfigurationManager.AppSettings["percorsocdn"].ToString();
        WelcomeLibrary.STATIC.Global.percorsoimg = ConfigurationManager.AppSettings["percorsoimg"].ToString();
        WelcomeLibrary.STATIC.Global.percorsoexp = ConfigurationManager.AppSettings["percorsoexp"].ToString();
        WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione = Server.MapPath("~");
        WelcomeLibrary.STATIC.Global.versionforcache = ConfigurationManager.AppSettings["versionforcache"].ToString();

        bool upd = false;
        bool.TryParse(ConfigurationManager.AppSettings["updateTableurlrewriting"], out upd);
        WelcomeLibrary.STATIC.Global.UpdateUrl = upd;
        //------------------------------------------------------------------------------------------------
        WelcomeLibrary.STATIC.Global.NomeConnessioneDb = "dbdataaccess";

        WelcomeLibrary.STATIC.Global.usecdn = false;
        string susecdn = ConfigurationManager.AppSettings["usecdn"].ToString();
        bool tmpcdn = false;
        bool.TryParse(susecdn, out tmpcdn);
        WelcomeLibrary.STATIC.Global.usecdn = tmpcdn;
         


        references.CaricaDatiReftablesDaJson(Server); //Tabelle di riferimento per l'immobiliare

        WelcomeLibrary.UF.Utility.CaricaListaStaticaNazioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, true);
        WelcomeLibrary.UF.Utility.Nazioni.RemoveAll(n => !(n.Codice == "IT" || n.Codice == "XX"));

        WelcomeLibrary.UF.Utility.CaricaListaStaticaProvince(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        //List<WelcomeLibrary.DOM.Province> province1 = WelcomeLibrary.UF.Utility.ElencoProvince.FindAll(delegate(WelcomeLibrary.DOM.Province tmp) { return (tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == "it"); });
        //_tmp = WelcomeLibrary.UF.Utility.ElencoProvince.Find(delegate(WelcomeLibrary.DOM.Province tmp) { return (tmp.Codice == "p94"); });
        //List<WelcomeLibrary.DOM.Province> province2 = WelcomeLibrary.UF.Utility.ElencoProvince.FindAll(delegate(WelcomeLibrary.DOM.Province tmp) { return (tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == "it"); });
        WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.AddRange(WelcomeLibrary.UF.Utility.ElencoProvince);
        WelcomeLibrary.UF.Utility.CaricaListaStaticaComuni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);

        WelcomeLibrary.UF.Utility.CaricaMemoriaStaticaCaratteristiche("",false);
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
         //GELibraryRemoto.UF.FunzioniUtilità.CreaListaStaticaTipologieDaFileXML(Server.MapPath("~" + ConfigurationManager.AppSettings["DataDir"].ToString() + "/Common/" + "tipologie.xml"));
         //GELibraryRemoto.UF.FunzioniUtilità.CreaListaStaticaProvinceDaFileXML(Server.MapPath("~" + ConfigurationManager.AppSettings["DataDir"].ToString() + "/Common/" + "province.xml"));

        WelcomeLibrary.UF.ResourceManagement.LoadResources();

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

