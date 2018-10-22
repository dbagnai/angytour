<%@ Application Language="C#" %>
<%@ Assembly Name="System.Configuration" %>
<%@ Assembly Name="System.Web" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Data.SQLite" %>
<%@ Import Namespace="WelcomeLibrary.UF" %>

<script RunAt="server">

    //////////////////////////////////////////////////////////////////////////////
    /////KEEP ALIVE TIMER///////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    public static DateTime When1;
    public static double Every1;
    private System.Timers.Timer OpTimer1;

    /// <summary>
    /// Funzione di Inizializzazione del Timer per la schedulazione delle opearazioni
    /// </summary>
    private void StartTimer1()
    {
        while (When1 <= DateTime.Now)
        {
            When1 = When1.AddHours(Every1);
        }
        OpTimer1 = new System.Timers.Timer(GetInterval1());
        OpTimer1.AutoReset = false; //Il timer non riparte automaticamente dopo l'evento ontimer
        OpTimer1.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent1); //Imposto il delegato per l'evento ontimer
        OpTimer1.Enabled = true; //Faccio partire il timer
    }
    /// <summary>
    /// Ritorna il numero di millisecondi da adesso all'orario prestabilito per l'esecuzione dell'evento
    /// del timer OnTimer
    /// </summary>
    /// <returns></returns>
    private double GetInterval1()
    {
        TimeSpan diff = When1.Subtract(DateTime.Now);

        //ricalcolo l'intervallo del timer per multipli della cadenza
        //successivamente all'istante attuale
        if (diff.TotalMinutes < 0)
        {
            while (When1 <= DateTime.Now)
            {
                When1 = When1.AddHours(Every1);
            }
            diff = When1.Subtract(DateTime.Now);
            // diff = DateTime.Parse(StartHour).AddDays(1).Subtract(DateTime.Now);
        }
        //risultato in millisecondi
        return diff.Ticks / 10000;
    }
    /// <summary>
    /// Evento relativo a OpTimer per l'esecuzione delle operazioni agli intervalli stabiliti
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public void OnTimedEvent1(object source, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            //this.Ping();
            string tmp = WelcomeLibrary.UF.SharedStatic.MakeHttpHtmlGet(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/keepalive.aspx", 1252);
        }
        catch { };
        //Reimposto l'esecuzione tra Every ore rispetto alla data di ultima esecuzione
        When1 = When1.AddHours(Every1);
        //Riprendo l'intervallo così prendo in considerazione il tempo di elaborazione passato
        //con il tempo - potrebbe sballare
        OpTimer1.Interval = (Double)GetInterval1();
        OpTimer1.Start();

    }
    //////////////////////////////////////////////////////////////////////////////
    ////KEEP ALIVE APP////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    //Variabili per Timer per esecuzione compiti schedulati
    public static DateTime When;
    public static double Every;
    private System.Timers.Timer OpTimer;
    /// <summary>
    /// Funzione di Inizializzazione del Timer per la schedulazione delle opearazioni
    /// </summary>
    private void StartTimer()
    {
        while (When <= DateTime.Now)
        {
            When = When.AddHours(Every);
        }
        OpTimer = new System.Timers.Timer(GetInterval());
        OpTimer.AutoReset = false; //Il timer non riparte automaticamente dopo l'evento ontimer
        OpTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent); //Imposto il delegato per l'evento ontimer
        OpTimer.Enabled = true; //Faccio partire il timer
    }
    /// <summary>
    /// Ritorna il numero di millisecondi da adesso all'orario prestabilito per l'esecuzione dell'evento
    /// del timer OnTimer
    /// </summary>
    /// <returns></returns>
    private double GetInterval()
    {
        TimeSpan diff = When.Subtract(DateTime.Now);

        //ricalcolo l'intervallo del timer per multipli della cadenza
        //successivamente all'istante attuale
        if (diff.TotalMinutes < 0)
        {
            while (When <= DateTime.Now)
            {
                When = When.AddHours(Every);
            }
            diff = When.Subtract(DateTime.Now);
            // diff = DateTime.Parse(StartHour).AddDays(1).Subtract(DateTime.Now);
        }

        //risultato in millisecondi
        return diff.Ticks / 10000;
    }
    /// <summary>
    /// Evento relativo a OpTimer per l'esecuzione delle operazioni agli intervalli stabiliti
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
    {
        //Creo una variabile per la scrittura dei messaggi nel file di log
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");

        //---------------------------------------------
        //Operazioni varie da eseguire
        //iterazione con database ecc.
        //---------------------------------------------
        try
        {
            //Dalla cartella degli immobili aggiorno il db sql su cui vengono fatte le ricerche
            Messaggi["Messaggio"] += "Inizio aggiornamenti " + System.DateTime.Now.ToString() + "\r\n";
            //qui devo fare l'aggiornamento del db In remoto
            WelcomeLibrary.DAL.offerteDM offDM = new WelcomeLibrary.DAL.offerteDM();

            if (!WelcomeLibrary.UF.MemoriaDisco.EsisteFileAccesso(WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "NowTrasferingIndirectMode.txt"))
            {
                //Creo il file di controllo per avitare avvi multipli del processo
                string ret = WelcomeLibrary.UF.MemoriaDisco.CreaFileAccesso(WelcomeLibrary.STATIC.Global.percorsoFisicoComune, System.DateTime.Now.ToString(), "NowTrasferingIndirectMode.txt");

                //CREAZIONE DEI FEED RSS-----------------------------------------------------------------
                System.Threading.Thread trUpdaterssFeed_I = new System.Threading.Thread(offDM.CreaRssFeedPerCategoria_I);
                trUpdaterssFeed_I.Start();

                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
                {
                    System.Threading.Thread trUpdaterssFeed_GB = new System.Threading.Thread(offDM.CreaRssFeedPerCategoria_GB);
                    trUpdaterssFeed_GB.Start();
                }
                //System.Threading.Thread trUpdaterssFeed_RU = new System.Threading.Thread(offDM.CreaRssFeedPerCategoria_RU);
                //trUpdaterssFeed_RU.Start();

                //---------------------------------------------------------------------------------------
                //---------------------------------------------------------------------------------------
                //AGGIORNIAMO ANCHE LA SITEMAP
                System.Threading.Thread trUpdateSitemap = new System.Threading.Thread(CreaSitemps);
                trUpdateSitemap.Start();
                //---------------------------------------------------------------------------------------
            }
            else
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(WelcomeLibrary.STATIC.Global.percorsoFisicoComune + "\\" + "NowTrasferingIndirectMode.txt");
                if (fi != null)
                {
                    if (((TimeSpan)(System.DateTime.Today - fi.CreationTime.Date)).Days >= 2)
                    {
                        //Elimino il file in quanto probabilmente si è interrotto il processo in modo imprevisto
                        fi.Delete();
                    }
                }
            }

        }
        catch (Exception errore)
        {
            //Devi scrivere l'errore in un file di log (per gli errori) sennò nessuno lo vede!!!!
            Messaggi["Messaggio"] += " Errore avvio aggiornamento feed: " + errore.Message + " " + System.DateTime.Now.ToString();
            if (errore.InnerException != null)
                Messaggi["Messaggio"] += " Errore interno aggiornamento feed : " + errore.InnerException.Message.ToString() + " " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune);
        }
        //---------------------------------------------

        //Reimposto l'esecuzione tra Every ore rispetto alla data di ultima esecuzione
        When = When.AddHours(Every);
        //Riprendo l'intervallo così prendo in considerazione il tempo di elaborazione passato
        //con il tempo - potrebbe sballare
        OpTimer.Interval = (Double)GetInterval();
        OpTimer.Start();
    }
    public void CreaSitemps()
    {
        //Creo una variabile per la scrittura dei messaggi nel file di log
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");

        try
        {

            string percorsoBase = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
            string PathSitemap = WelcomeLibrary.UF.MemoriaDisco.physiclogdir;//.Replace("\\Common", "");
            string host = percorsoBase.Replace(".", "");
            host = host.Replace(":", "");
            host = host.Replace("/", "");
            Messaggi["Messaggio"] += host.ToLower();

            //Carichiamo la lista delle offerte totale
            WelcomeLibrary.DAL.offerteDM offDM = new WelcomeLibrary.DAL.offerteDM();
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            parColl = new List<SQLiteParameter>();
            //SQLiteParameter ptipo = new SQLiteParameter("@CodiceTIPOLOGIA", "rif000001"); //Catalogo prodotti unico
            //parColl.Add(ptipo);

            //VA' CREATO UN FILTRO PER LA GENERAZIONE
            //rif000001-rif000050 ( rubriche da 0 a 50 ) ( elenco tipo blog + scheda ) -> CreaLinkAScheda href='<%# CreaLinkAScheda(Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString(),"","" ,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Lingua,Session,true) %>'

            //rif000051 -rif000060 -> non ci sono le schede ma solo le liste con i pdf (lista elenco!!) NON CI SONO LE SCHEDE SINGOLE!! MA SOLO LE LISTE
            //rif000061 -> per  i comunicati stmapa come blog -> CreaLinkAScheda href='<%# CreaLinkAScheda(Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString(),"","" ,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Lingua,Session,true) %>'

            //rif000100 soci -> schede CreaLinkGenerico //CreaLinkGenerico(Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString(),"","","","" ,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Lingua,Session,true) %>'
            //rif000101 -> trattaemtni   schede indirizzabili a  href='<%# CreaLinkRicercaprodotti(Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString(),"","","","" ,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Lingua,Session,false) %>'

            //rif000199 -> partners SOLO ELENCO NON CI SONO LE SCHEDE singole i link mandano fuori
            List<string> Tmp_linksite = new List<string>();

            string Lingua = "I";
            Tmp_linksite.AddRange(WelcomeLibrary.UF.SitemapManager.RigeneraLinkSezioniUrlrewrited(Lingua, "rif000012,rif000051,rif000061,rif000062,rif000101,rif000666"));
            WelcomeLibrary.DOM.OfferteCollection offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "10000", Lingua);
            //Questa è da rivedere in base al codice tipologia !!!!!
            Tmp_linksite.AddRange(WelcomeLibrary.UF.SitemapManager.CreaLinksSchedeProdottoDaOfferte(offerte, Lingua, percorsoBase, "", true));
            references.CreazioneSitemap("sitemapLink" + Lingua + host, PathSitemap, Tmp_linksite, System.DateTime.Today.ToString("yyyy-MM-dd"), "monthly", "1");

            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
            {
                Lingua = "GB";
                Tmp_linksite = new List<string>();
                Tmp_linksite.AddRange(WelcomeLibrary.UF.SitemapManager.RigeneraLinkSezioniUrlrewrited(Lingua, "rif000012,rif000051,rif000061,rif000062,rif000101,rif000666"));
                offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "10000", Lingua);
                Tmp_linksite = WelcomeLibrary.UF.SitemapManager.CreaLinksSchedeProdottoDaOfferte(offerte, Lingua, percorsoBase, "", true);
                references.CreazioneSitemap("sitemapLink" + Lingua + host, PathSitemap, Tmp_linksite, System.DateTime.Today.ToString("yyyy-MM-dd"), "monthly", "1");

            }
#if false
            Lingua = "RU";
            Tmp_linksite = new List<string>();
            Tmp_linksite.AddRange(WelcomeLibrary.UF.SitemapManager.RigeneraLinkSezioniUrlrewrited(Lingua, "rif000012,rif000051,rif000061,rif000062,rif000101,rif000666"));
            offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "10000", Lingua);
            Tmp_linksite = WelcomeLibrary.UF.SitemapManager.CreaLinksSchedeProdottoDaOfferte(offerte, Lingua, percorsoBase, "", true);
            references.CreazioneSitemap("sitemapLink" + Lingua + host, PathSitemap, Tmp_linksite, System.DateTime.Today.ToString("yyyy-MM-dd"), "monthly", "1");
#endif

            //references.CreaSitemapImmobili(null, "rif000666");//Sitemap per la parte immobiliare

            Messaggi["Messaggio"] += "Generato sitemap  " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
        }
        catch (Exception errore)
        {
            //Devi scrivere l'errore in un file di log (per gli errori) sennò nessuno lo vede!!!!
            Messaggi["Messaggio"] += " Errore aggiornamento in app start: " + errore.Message + " " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
        }
        finally
        {
            //Rimuovo il file di accesso per la concorrenza
            string retfa = WelcomeLibrary.UF.MemoriaDisco.EliminaFileAccesso(WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "NowTrasferingIndirectMode.txt");
            //Creare un file di log con il successo/fallimento di tutte le operazioni
            Messaggi["Messaggio"] += "Terminato Aggiornamento  " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune);
        }
    }

    void Application_Start(object sender, EventArgs e)
    {
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");
        Messaggi["Messaggio"] += " Application start : " + System.DateTime.Now.ToString();

        try
        {
            //CaricaMemoriaStatica();
            references.CaricaMemoriaStatica(Server);
            WelcomeLibrary.UF.SitemapManager.RigeneraLinkSezioniUrlrewrited();//rigenera i link per le tipologie/categorie e sottocategorie per url rewriting
            //Carico la memoria statica per la modalità trial del portale
            //se suparato la data di scadenza di prova allora ativo lamodalità trial del prodotto
            //Calcolo l'hash
            //string hash = WelcomeLibrary.STATIC.Global.MD5GenerateHashWithSecret(ConfigManagement.ReadKey("trialkey"));
            string nome = CommonPage.getFirstName("webmaster");
            string code = WelcomeLibrary.UF.ConfigManagement.ReadKey("trialkey");
            if (nome == "17169") code = "";
            if (!WelcomeLibrary.STATIC.Global.MD5CheckHashWithSecret(code, WelcomeLibrary.STATIC.Global.Codicesblocco))
            {
                WelcomeLibrary.STATIC.Global.Trial = true;
            }

            RegisterRoutes(System.Web.Routing.RouteTable.Routes);
            //-----------------------------------------------------------------------
            //Inizializzazione Timer per gestione attività schedulate da applicazione
            //-----------------------------------------------------------------------
            //StartHour = System.Configuration.ConfigManagement.ReadKey("OraStartRiepiloghi"].ToString();
            //StartDay = Convert.ToInt32(System.Configuration.ConfigManagement.ReadKey("GiornoStartRiepiloghi"]);
            //-----------------------------------------------------------------------
            //Inizializzazione Timer per gestione attvità schedulate da applicazione
            //-----------------------------------------------------------------------
            When = DateTime.Parse(DateTime.Now.AddMinutes(1).ToString());
            //Every = 0.01; //Ciclo di esecuzione in ore
            Every = 4; //Ciclo di esecuzione in ore
            StartTimer();

            //TIMER2 -> PER KEEP ALIVE ( blocca idle timeout brevi di IIS )
            When1 = DateTime.Parse(DateTime.Now.AddMinutes(2).ToString());
            //Every = 0.01; //Ciclo di esecuzione in ore
            Every1 = 0.03; //Ciclo di esecuzione in ore
            StartTimer1();

            //Creo una variabile per la scrittura dei messaggi nel file di log
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi); //Per fare questa scritturo ho problemi di file permissions sul getdirectory

        }
        catch (Exception errore)
        {
            Messaggi["Messaggio"] += " Errore aggiornamento portale: " + errore.Message + " " + System.DateTime.Now.ToString();
            if (errore.InnerException != null)
                Messaggi["Messaggio"] += " Errore aggiornamento portale: " + errore.InnerException.Message + " " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi); //Per fare questa scritturo ho problemi di file permissions sul getdirectory

        }
    }

    void Application_BeginRequest(Object source, EventArgs e)
    {
        //if (System.Configuration.ConfigManagement.ReadKey("enableHttps"] == "true")
        //    if (HttpContext.Current.Request.IsSecureConnection.Equals(false))
        //    {
        //        Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
        //    }

        HttpApplication app = (HttpApplication)source;
        HttpContext context = app.Context;
        FirstRequestInitialization.Initialize(context);
    }
    class FirstRequestInitialization
    {
        private static bool s_InitializedAlready = false;
        private static object s_lock = new Object();
        // Initialize only on the first request
        public static void Initialize(HttpContext context)
        {
            if (s_InitializedAlready)
            {
                return;
            }
            lock (s_lock)
            {
                if (s_InitializedAlready)
                {
                    return;
                }

                string appaddress = "";
                if (System.Web.HttpContext.Current.Request.Url.AbsolutePath != "/")
                {
                    int pathpos = System.Web.HttpContext.Current.Request.Url.ToString().IndexOf(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
                    if (pathpos != -1)
                    {
                        appaddress = System.Web.HttpContext.Current.Request.Url.ToString().Substring(0, pathpos);
                        appaddress += HttpContext.Current.Request.ApplicationPath.ToString().ToLower().TrimEnd('/');
                    }
                    else
                    {
                        pathpos = System.Web.HttpContext.Current.Request.Url.ToString().IndexOf(HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Url.AbsolutePath));
                        if (pathpos != -1)
                        {
                            appaddress = System.Web.HttpContext.Current.Request.Url.ToString().Substring(0, pathpos);
                            appaddress += HttpContext.Current.Request.ApplicationPath.ToString().ToLower().TrimEnd('/');
                        }
                    }
                }
                else
                {
                    appaddress = System.Web.HttpContext.Current.Request.Url.ToString();
                    int startquery = appaddress.IndexOf("?");
                    if (startquery != -1)
                        appaddress = appaddress.Substring(0, startquery);
                    string query = System.Web.HttpContext.Current.Request.Url.Query;
                    if (!string.IsNullOrEmpty(query))
                    {
                        appaddress.Replace(HttpContext.Current.Server.UrlDecode(query), "");
                        appaddress = appaddress.Replace(HttpContext.Current.Server.UrlDecode(query).Replace("%20", "%2520"), "");
                        appaddress.Replace((query), "");
                    }
                    appaddress = appaddress.ToLower().TrimEnd('/');
                }

                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("enableHttps").ToLower() == "true")
                    appaddress = appaddress.ToLower().Replace("http:", "https:");

                System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
                Messaggi.Add("Messaggio", "");
                if (appaddress == "")//Caso di chiamata strana fallback
                {
                    Messaggi["Messaggio"] += " Errore global init original path: " + System.Web.HttpContext.Current.Request.Url.ToString();
                    Messaggi["Messaggio"] += " Errore global init AbsolutePath : " +
                    System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                    Messaggi["Messaggio"] += " Errore global init ApplicationPath : " + HttpContext.Current.Request.ApplicationPath.ToString();
                    Messaggi["Messaggio"] += " Errore global init server host : " + System.Web.HttpContext.Current.Request.Url.Host.ToString();
                    Messaggi["Messaggio"] += " Errore global init server appaddress : " + appaddress;
                    WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
                }
                else
                {
                    Messaggi["Messaggio"] += " Global init original path: " + System.Web.HttpContext.Current.Request.Url.ToString();
                    Messaggi["Messaggio"] += " Global init AbsolutePath : " +
                 System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                    Messaggi["Messaggio"] += " Global init ApplicationPath : " + HttpContext.Current.Request.ApplicationPath.ToString();
                    Messaggi["Messaggio"] += " Global init Query : " + System.Web.HttpContext.Current.Request.Url.Query; ;
                    Messaggi["Messaggio"] += " Global init server host : " + System.Web.HttpContext.Current.Request.Url.Host.ToString();
                    Messaggi["Messaggio"] += " Global init server appaddress : " + appaddress;
                    WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
                }

                WelcomeLibrary.STATIC.Global.percorsobaseapplicazione = appaddress;
                WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione = HttpContext.Current.Request.PhysicalApplicationPath;
                s_InitializedAlready = true;
            }
        }
    }

    /// <summary>
    /// Funzione di replace con regexp
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="complete"></param>
    /// <param name="replacement"></param>
    /// <returns></returns>
    public static string RegexReplace(string pattern, string complete, string replacement)
    {

        //string pattern = "\\s+";
        string tmpValue = pattern.Replace("/", "\\/");
        tmpValue = tmpValue.Replace("?", "\\?");
        string patternComplete = tmpValue + "$";
        Regex rgx = new Regex(patternComplete);
        string result = rgx.Replace(complete, replacement);
        return result;
    }

    void RegisterBundles(System.Web.Routing.RouteCollection routes)
    {

        // DICHIARAZIONE BUNDLES E MODALITA' RITORNO
        BundleEngine.AddRoutes(routes);//Route x bundler
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("BundlesCombine").ToLower() != "true")
            BundleEngine.BundleOptions.InjectionMode = BundleEngine.EnumInjectionMode.SingleLinkOrScript; //Opzioni di ritorno dell'handler
        else
            BundleEngine.BundleOptions.InjectionMode = BundleEngine.EnumInjectionMode.SingleCombinedScript; //Opzioni di ritorno dell'handler

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("BundlesCheckFilesAlways").ToLower() == "true")
            BundleEngine.BundleOptions.CheckFilesAlways = true; //Rilegge md5 tutti files ad ogni chiamata e non solo a riavvio app
                                                                //BundleEngine.BundleOptions.BundleMode = BundleEngine.EnumBundleMode.LastWriteTime;
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("Bundlesnifycss").ToLower() == "true")
            BundleEngine.BundleOptions.minifyCss = true;
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("Bundlesminifyjs").ToLower() == "true")
            BundleEngine.BundleOptions.minifyJs = true;

        BundleEngine.AddBundleJS("bundlejssw",
          // "~/sw.js", //Service Worker
          "~/sw-register.js"  //Service Worker registration
        );

        BundleEngine.AddBundleJS("bundlejslib0",
            "~/js/jquery321/jquery-3.2.1.min.js",
            "~/js/jquery321/jquery-migrate-3.0.1.min.js",
             "~/js/bootstrap400/popper.min.js",
            "~/js/bootstrap400/js/bootstrap.min.js",
            "~/js/localforage.min.js",
            //"~/js/idkeyval/idb-keyval-iife.js",
            "~/js/menuzord/files/js/menuzord.js",
            "~/js/pako/pako.min.js",
            "~/js/notifications/notify.min.js",
            "~/js/notifications/notify-metro.js",
            "~/js/notifications/notifications.js",
            "~/js/moment-with-locales.min.js", //big trova alternativa!!!
            "~/js/imagesloaded.pkgd.js",
            "~/js/isotope304/isotope.pkgd.min.js",
            "~/js/jquery.cycle.js",
            "~/js/jquery.cycle2.carousel.js",
             "~/js/owl-carousel.js",
            "~/js/owl-carousel/owl-carousel/owl.carousel.min.js",
            "~/js/jquery-ui-1.12.1.custom/jquery-ui.js",
            "~/js/jqueryui/jquery.maskedinput.js",
            "~/js/revolution464/jquery.themepunch.plugins.min.js",
            "~/js/revolution464/jquery.themepunch.tools.min.js",
            "~/js/revolution464/jquery.themepunch.revolution.min.js",
             "~/lib/js/common.js"
        );

        BundleEngine.AddBundleJS("bundlejslib1",
            //"~/js/magnify/js/jquery.magnify.js",
            //"~/js/magnify/js/jquery.magnify-mobile.js"
            //"~/lib/js/immobili2.js",
            "~/lib/js/genericContent.js",
            "~/lib/js/genericBanner.js",
            "~/lib/js/sliderBanner.js",
            "~/lib/js/scroller.js",
            "~/lib/js/scrollerBanner.js",
            "~/lib/js/portfolioisotope.js",
            "~/lib/js/bannerFascia.js",
            "~/lib/js/portfolioisotopeBanner.js",
            "~/lib/js/archivio.js",
            "~/lib/js/feedbacks.js",
            "~/lib/js/linkslistddl1.js",
            "~/lib/js/linkslistddl2.js",
            "~/js/lazyloadimg.js",
            "~/lib/js/searchcontrol.js",
            "~/lib/js/carrello.js",
            "~/lib/js/bookingtool.js",
            "~/lib/js/videoBanner.js"
        );

        BundleEngine.AddBundleJS("bundlejslib2",
            //~/js/jquery.validate.js,
            //~/js/defiant/defiant.min.js,
            //"https://www.google.com/recaptcha/api.js",
            "~/js/animationEnigne.js",
            "~/js/jq.appear.js",
            "~/js/jquery.base64.js",
            "~/js/jquery.easing.1.3.js",
            "~/js/jquery.prettyPhoto.js",
            "~/js/jquery.tipsy.js",
            "~/js/jQuery.XDomainRequest.js",
            "~/js/back-to-top.js",
            "~/js/flexslider/jquery.flexslider-min.js",
            "~/js/it-cookies-policy.js",
            "~/js/YTPlayer/jquery.mb.YTPlayer.js",
            "~/js/simplestarrating/SimpleStarRating.js",
            //"~/css/fontawesome541/js/all.js",
            //"~/css/fontawesome541/js/v4-shims.js",
            "~/js/jquery.fitvids.js",
            "~/js/detect-zoom.min.js",
            "~/js/maininit.js",
            "~/js/landing/jarallax.js",
            "~/js/landing/script.js"
        );

        BundleEngine.AddBundleCSS("bundlecss1",
            "~/js/bootstrap400/css/bootstrap.min.css",
            "~/css/revolution_settings.css",
            "~/js/menuzord/files/css/menuzord.css",
            "~/js/menuzord/files/css/skins/menuzord-colored.css",
             "~/js/menuzord/files/css/menuzordcustomize.css",
             "~/css/fontawesome541/css/all.min.css",
              "~/css/fontawesome541/css/v4-shims.min.css",
            "~/css/tipsy.css",
            "~/css/prettyPhoto.css",
            "~/css/isotop_animation.css",
            "~/css/animate.css",
            "~/js/simplestarrating/SimpleStarRating.css",
              "~/css/style1.css",
            "~/js/jquery-ui-1.12.1.custom/jquery-ui.min.css",
            "~/js/jquery-ui-1.11.4.custom/customizeautocomplete.css",
            "~/js/notifications/notification.css",
            "~/js/owl-carousel/owl-carousel/owl.carousel.css",
            "~/js/flexslider/flexslider.css",
            "~/js/magnify/css/magnify.css",
            "~/css/ashobiz/ashobiz.base.css",
            "~/css/ashobiz/ashobiz-176.css",
            "~/css/ui-personalization.css",
             "~/css/opj/style-100.css", /*SET-COLORI*/
             "~/css/mbr-faq.css",
             "~/css/mbr-additional.css",
             "~/css/landing.css",
             "~/css/custom1.css",
             "~/css/custom.css"
          );

        BundleEngine.AddBundleCSS("bundlecsspwa",
         "~/js/bootstrap400/css/bootstrap.min.css",
         "~/css/revolution_settings.css",
         "~/js/menuzord/files/css/menuzord.css",
         "~/js/menuzord/files/css/skins/menuzord-colored.css",
          "~/js/menuzord/files/css/menuzordcustomize.css", 
                    "~/css/fontawesome541/css/all.min.css",
              "~/css/fontawesome541/css/v4-shims.min.css",
              "~/css/tipsy.css",
         "~/css/prettyPhoto.css",
         "~/css/isotop_animation.css",
         "~/css/animate.css",
         "~/js/simplestarrating/SimpleStarRating.css",
         "~/js/jquery-ui-1.12.1.custom/jquery-ui.min.css",
         "~/js/jquery-ui-1.11.4.custom/customizeautocomplete.css",
         "~/js/notifications/notification.css",
         "~/js/owl-carousel/owl-carousel/owl.carousel.css",
         "~/js/flexslider/flexslider.css",
         "~/js/magnify/css/magnify.css",
         "~/css/ashobiz/ashobiz.base.css",
         "~/css/ashobiz/ashobiz-176.css",
           "~/css/style1.css",
         "~/css/ui-personalization.css",
          "~/css/opj/style-100.css", /*SET-COLORI*/
          "~/css/mbr-faq.css",
          "~/css/mbr-additional.css",
          "~/css/landing.css",
          "~/css/custom1.css",
          "~/css/custom.css"
       );
    }

    void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        RegisterBundles(routes); // Bundles da iniettare css/js

        //routes.Add("Generic", new System.Web.Routing.Route("{textmatch}", new GenericRouteHandler()));
        routes.Add("Generic1", new System.Web.Routing.Route("{Lingua}/{textmatch}", new GenericRouteHandler()));
        routes.Add("Generic2", new System.Web.Routing.Route("{Lingua}/{destinationselector}/{textmatch}", new GenericRouteHandler()));
        routes.MapPageRoute(
            "HomeRoute",
            "",
            "~/Index.aspx"
        );

        //Fall back per tutti i link in route che non hanno il . all'interno
        routes.Add(new Route("{textmatch}",
        new RouteValueDictionary { { "textmatch", "default" } },
        new RouteValueDictionary { { "textmatch", @"^([^.]+)$" } },
        new GenericRouteHandler()));

        RoutingForOldUrlRewrite(routes); //Da configurae con i vecchi valori di route da renindirizzare
    }
    void RoutingForOldUrlRewrite(System.Web.Routing.RouteCollection routes) //Vecchi parametri routing per redirect
    {
        //Dovremmo mappare anche per le vecchie liste di raggruppamento ( queste sono da mappare in tabella redirect !!!! per intero  )
        //routes.Add("Genericoldpagstatica", new System.Web.Routing.Route("Informazioni/{Lingua}/{idContenuto}/{testoindice}", new GenericRouteHandler()));
        //routes.Add("Genericoldschedaablog", new System.Web.Routing.Route("Articolo/{Lingua}/{Tipologia}/{idOfferta}/{testoindice}", new GenericRouteHandler()));
        //routes.Add("Genericoldlistablog", new System.Web.Routing.Route("info/{Lingua}/{Tipologia}/{testoindice}", new GenericRouteHandler()));
        //routes.Add("Genericoldlistacatblog", new System.Web.Routing.Route("info/{Lingua}/{Tipologia}/{Categoria}/{testoindice}", new GenericRouteHandler()));
        //routes.Add("Genericoldlistacatsottocatblog", new System.Web.Routing.Route("info/{Lingua}/{Tipologia}/{Categoria}/{Categoria2liv}/{testoindice}", new GenericRouteHandler()));
        //routes.Add("Genericoldschedaprod", new System.Web.Routing.Route("prodotto/{Lingua}/{Tipologia}/{idOfferta}/{testoindice}", new GenericRouteHandler()));
        //routes.Add("Genericoldlistaprod", new System.Web.Routing.Route("catalogo/{Lingua}/{Tipologia}/{testoindice}", new GenericRouteHandler()));
        //routes.Add("Genericoldlistaprodcat", new System.Web.Routing.Route("catalogo/{Lingua}/{Tipologia}/{Categoria}/{testoindice}", new GenericRouteHandler()));
        //routes.Add("Genericoldlistaprodcatsottocat", new System.Web.Routing.Route("catalogo/{Lingua}/{Tipologia}/{Categoria}/{Categoria2liv}/{testoindice}", new GenericRouteHandler()));
    }
    void Application_End(object sender, EventArgs e)
    {
        //  Codice eseguito all'arresto dell'applicazione
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Codice eseguito in caso di errore non gestito
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Codice eseguito all'avvio di una nuova sessione
    }

    void Session_End(object sender, EventArgs e)
    {
        // Codice eseguito al termine di una sessione.
        // Nota: l'evento Session_End viene generato solo quando la modalità sessionstate
        // è impostata su InProc nel file Web.config. Se la modalità è impostata su StateServer
        // o SQLServer, l'evento non viene generato.

    }
</script>
