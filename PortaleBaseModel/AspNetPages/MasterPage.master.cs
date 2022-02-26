using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

public partial class AspNetPages_MasterPage : System.Web.UI.MasterPage
{
    CommonPage CommonPage = new CommonPage();

    public DataTable dt
    {
        get { return ViewState["DataTable"] != null ? (DataTable)(ViewState["DataTable"]) : new DataTable(); }
        set { ViewState["DataTable"] = value; }
    }
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : CommonPage.deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string PercorsoComune
    {
        get { return ViewState["PercorsoComune"] != null ? (string)(ViewState["PercorsoComune"]) : ""; }
        set { ViewState["PercorsoComune"] = value; }
    }
    public string pathassoluto
    {
        get { return ViewState["pathassoluto"] != null ? (string)(ViewState["pathassoluto"]) : ""; }
        set { ViewState["pathassoluto"] = value; }
    }
    public string CodiceTipologia
    {
        get { return ViewState["CodiceTipologia"] != null ? (string)(ViewState["CodiceTipologia"]) : ""; }
        set { ViewState["CodiceTipologia"] = value; }
    }
    public string Categoria
    {
        get { return ViewState["Categoria"] != null ? (string)(ViewState["Categoria"]) : ""; }
        set { ViewState["Categoria"] = value; }
    }
    public string Categoria2liv
    {
        get { return ViewState["Categoria2liv"] != null ? (string)(ViewState["Categoria2liv"]) : ""; }
        set { ViewState["Categoria2liv"] = value; }
    }

    public string Caratteristica1
    {
        get { return ViewState["Caratteristica1"] != null ? (string)(ViewState["Caratteristica1"]) : ""; }
        set { ViewState["Caratteristica1"] = value; }
    }
    public string Caratteristica2
    {
        get { return ViewState["Caratteristica2"] != null ? (string)(ViewState["Caratteristica2"]) : ""; }
        set { ViewState["Caratteristica2"] = value; }
    }

    public string Caratteristica3
    {
        get { return ViewState["Caratteristica3"] != null ? (string)(ViewState["Caratteristica3"]) : ""; }
        set { ViewState["Caratteristica3"] = value; }
    }
    public string Caratteristica4
    {
        get { return ViewState["Caratteristica4"] != null ? (string)(ViewState["Caratteristica4"]) : ""; }
        set { ViewState["Caratteristica4"] = value; }
    }
    public string Caratteristica5
    {
        get { return ViewState["Caratteristica5"] != null ? (string)(ViewState["Caratteristica5"]) : ""; }
        set { ViewState["Caratteristica5"] = value; }
    }
    public string idOfferta
    {
        get { return ViewState["idOfferta"] != null ? (string)(ViewState["idOfferta"]) : ""; }
        set { ViewState["idOfferta"] = value; }
    }
    public string idContenuto
    {
        get { return ViewState["idContenuto"] != null ? (string)(ViewState["idContenuto"]) : ""; }
        set { ViewState["idContenuto"] = value; }
    }
    public string Vetrina
    {
        get { return ViewState["vetrina"] != null ? (string)(ViewState["vetrina"]) : ""; }
        set { ViewState["vetrina"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ControllaHttp();


            linkFi1.Href = CommonPage.ReplaceAbsoluteLinks("~/images/favicon.ico");
            linkFi2.Href = CommonPage.ReplaceAbsoluteLinks("~/images/favicon.png");

            htmltag.Attributes["xml:lang"] = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            //Creo l'equivalente di ~/ nel ViewState per usarlo nel javascript della pagina
            pathassoluto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
            PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
            if (string.IsNullOrWhiteSpace(metaTitle.Text))
                metaTitle.Text += references.ResMan("Common", Lingua, "titleMain").ToString().Replace("<br/>", " ").Trim();
            if (string.IsNullOrWhiteSpace(metaDesc.Content))
                metaDesc.Content += references.ResMan("Common", Lingua, "descMain").ToString().Replace("<br/>", " ").Trim();
            if (string.IsNullOrEmpty(((HtmlMeta)metafbimage).Content))
                ((HtmlMeta)metafbimage).Content = references.ResMan("Common", Lingua, "mainfbimage");


            //Prendiamo i dati dalla querystring
            Lingua = CommonPage.CaricaValoreMaster(Request, Session, "Lingua", false, CommonPage.deflanguage);

            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() != "true")
            {
                if (Lingua.ToLower() == "gb") Response.RedirectPermanent("~");
            }
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() != "true")
            {
                if (Lingua.ToLower() == "ru") Response.RedirectPermanent("~");
            }
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() != "true")
            {
                if (Lingua.ToLower() == "fr") Response.RedirectPermanent("~");
            }

            //if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
            //    ControlloLingua(); // RIABILITARE PER ONLINE per reindirizzare le lingue su domini diversi

            CodiceTipologia = CommonPage.CaricaValoreMaster(Request, Session, "Tipologia", true, CodiceTipologia);
            Categoria = CommonPage.CaricaValoreMaster(Request, Session, "Categoria", true, Categoria);
            Categoria2liv = CommonPage.CaricaValoreMaster(Request, Session, "Categoria2liv", true, Categoria2liv);
            Caratteristica1 = CommonPage.CaricaValoreMaster(Request, Session, "Caratteristica1", false);
            Caratteristica2 = CommonPage.CaricaValoreMaster(Request, Session, "Caratteristica2", false);
            Caratteristica3 = CommonPage.CaricaValoreMaster(Request, Session, "Caratteristica3", false);
            Caratteristica4 = CommonPage.CaricaValoreMaster(Request, Session, "Caratteristica4", false);
            Caratteristica5 = CommonPage.CaricaValoreMaster(Request, Session, "Caratteristica5", false); idContenuto = CommonPage.CaricaValoreMaster(Request, Session, "idContenuto", true);
            idOfferta = CommonPage.CaricaValoreMaster(Request, Session, "idOfferta", true);
            Vetrina = CommonPage.CaricaValoreMaster(Request, Session, "vetrina", true, "");
            Page.ClientScript.GetPostBackEventReference(this, string.Empty);

        }
        else
        {
            if (Request["__EVENTTARGET"] == "inseriscinewsletter")
            {
                string email = Request["__EVENTARGUMENT"];
                InserisciNewsletter(email);
            }

        }


        //pnlRicerca.DataBind();
        //lisearch.DataBind();

        divContattiMaster.DataBind();
        req1.DataBind();

        //CommonPage.CustomContentInject(((HtmlGenericControl)Page.Master.FindControl("masterlow1")), "customcontent3-" + Lingua + ".html", Lingua, Page.User.Identity.Name, Session);
        CommonPage.CustomContentInject(((HtmlGenericControl)Page.Master.FindControl("masterlow1")), "customcontent4-" + Lingua + ".html", Lingua, Page.User.Identity.Name, Session);

        CommonPage.CustomContentInject(((HtmlGenericControl)Page.Master.FindControl("masterlow3")), "customcontent-popup-" + Lingua + ".html", Lingua, Page.User.Identity.Name, Session);

    }

    /// <summary>
    /// esegue il binding tramite un template e inetta il renderizzato in pagina anche fuori da aspnet form
    /// </summary>
    /// <returns></returns>
    public string InjectDirectrenderinPage()
    {
        //return CommonPage.CustomContentRender( "customcontentvertical1-" + Lingua + ".html", Lingua, Page.User.Identity.Name, Session);
        return CommonPage.HtmlfromteplateInject("customcontentvertical2-" + Lingua + ".html", Lingua, Page.User.Identity.Name, Session);

    }

    /// <summary>
    /// Carico le chiamate di inizializzazione dalla memoria di binding del server in custombind
    /// </summary>
    /// <returns></returns>
    public string InjectedEndPageScripts()
    {
        Dictionary<string, string> addelements = new Dictionary<string, string>();
        LoadJavascriptVariablesEnd(addelements);
        string ret = custombind.CreaInitStringJavascript(Session.SessionID, addelements);
        return ret;
    }
    public string InjectedStartPageScripts()
    {
        Dictionary<string, string> addelements = new Dictionary<string, string>();
        LoadJavascriptVariablesStart(addelements);
        string ret = custombind.CreaInitStringJavascriptOnly(addelements);
        return ret;
    }
    private void LoadJavascriptVariablesStart(Dictionary<string, string> addelements = null)
    {
        //However, if you want your JavaScript code to be independently escaped for any context, you could opt for the native JavaScript encoding:
        //' becomes \x27
        //" becomes \x22

        String scriptRegVariables = "";
        scriptRegVariables += ";\r\n " + string.Format("var lng = '{0}'", Lingua);
        scriptRegVariables += ";\r\n " + string.Format("var pathAbs = '{0}'", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
        scriptRegVariables += ";\r\n" + string.Format("var idofferta = '" + idOfferta + "'");
        scriptRegVariables += ";\r\n" + string.Format("var tipologia = '" + CodiceTipologia + "'");
        scriptRegVariables += ";\r\n" + string.Format("var categoria = '" + Categoria + "'");
        scriptRegVariables += ";\r\n" + string.Format("var categoria2liv = '" + Categoria2liv + "'");
        scriptRegVariables += ";\r\n" + string.Format("var GoogleMapsKey = '" + WelcomeLibrary.UF.ConfigManagement.ReadKey("GoogleMapsKey") + "'");
        scriptRegVariables += ";\r\n" + string.Format("var stripe_publishableKey = '{0}'", ConfigManagement.ReadKey("stripe_publishableKey"));

        scriptRegVariables += ";\r\n";

        if (addelements == null) addelements = new Dictionary<string, string>();
        addelements.Add("jsvarfrommasterstart", scriptRegVariables);

    }

    private void LoadJavascriptVariablesEnd(Dictionary<string, string> addelements = null)
    {
        //However, if you want your JavaScript code to be independently escaped for any context, you could opt for the native JavaScript encoding:
        //' becomes \x27
        //" becomes \x22

        String scriptRegVariables = "";
        scriptRegVariables += ";\r\n" + string.Format("var GooglePosizione1 = '{0}'", references.ResMan("Common", Lingua, "GooglePosizione1").Replace("'", "\\'"));
        scriptRegVariables += ";\r\n" + string.Format("var googleurl1 = '{0}'", references.ResMan("Common", Lingua, "GoogleUrl1").Replace("'", "\\'"));
        scriptRegVariables += ";\r\n" + string.Format("var googlepin1 = '{0}'", references.ResMan("Common", Lingua, "GooglePin1").Replace("'", "\\'"));
        scriptRegVariables += ";\r\n" + string.Format("var GooglePosizione2 = '{0}'", references.ResMan("Common", Lingua, "GooglePosizione2").Replace("'", "\\'"));
        scriptRegVariables += ";\r\n" + string.Format("var googleurl2 = '{0}'", references.ResMan("Common", Lingua, "GoogleUrl2").Replace("'", "\\'"));
        scriptRegVariables += ";\r\n" + string.Format("var googlepin2 = '{0}'", references.ResMan("Common", Lingua, "GooglePin2").Replace("'", "\\'"));
        scriptRegVariables += ";\r\n" + string.Format("var idmapcontainer = 'map'");
        scriptRegVariables += ";\r\n" + string.Format("var idmapcontainer1 = 'map1'");
        scriptRegVariables += ";\r\n" + string.Format("var idmapcontainerlocal = 'maplocal'");
        scriptRegVariables += ";\r\n" + string.Format("var iddirectionpanelcontainer = 'directionpanel'");
        //Passo codificate base64 con encoding utf-8 le risorse necessarie al javascript della pagina iniettandole in pagina (   questo evita di attendere la promise per inizializzare le variabili javascript !!! )
        //scriptRegVariables += ";\r\n" + string.Format("loadvariables(utf8ArrayToStr(urlB64ToUint8Array('{0}')))", dataManagement.EncodeUtfToBase64(references.initreferencesdataserialized(Lingua, Page.User.Identity.Name)));
        scriptRegVariables += ";\r\n" + WelcomeLibrary.UF.Utility.waitwrappercall("loadvariables", string.Format("loadvariables(b64ToUtf8('{0}'))", dataManagement.EncodeUtfToBase64(references.initreferencesdataserialized(Lingua, Page.User.Identity.Name))));
        scriptRegVariables += ";\r\n" + WelcomeLibrary.UF.Utility.waitwrappercall("moment", string.Format("moment.locale('{0}') ", "it"));
        //scriptRegVariables += ";\r\n " + string.Format("moment.locale('{0}') ", "it");
        scriptRegVariables += ";\r\n";

        if (addelements == null) addelements = new Dictionary<string, string>();
        addelements.Add("jsvarfrommasterend", scriptRegVariables);

        //ClientScriptManager cs = Page.ClientScript;
        //if (!cs.IsClientScriptBlockRegistered("RegVariablesScript"))
        //{
        //    cs.RegisterClientScriptBlock(typeof(Page), "RegVariablesScript", scriptRegVariables, true);
        //}
    }


    private void ControllaHttp()
    {
        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https:"))
        {
            if (!System.Web.HttpContext.Current.Request.Url.ToString().StartsWith("https:"))
            {
                Response.RedirectPermanent(System.Web.HttpContext.Current.Request.Url.ToString().Replace("http:", "https:"), true);
            }

        }
        else
        {
            if (!System.Web.HttpContext.Current.Request.Url.ToString().StartsWith("http:"))
            {
                Response.RedirectPermanent(System.Web.HttpContext.Current.Request.Url.ToString().Replace("https:", "http:"), true);
            }
        }
    }
    private void ControlloLingua()
    {
        string host = System.Web.HttpContext.Current.Request.Url.Host.ToString();
        // outputContact.Text = Lingua + " " + host + " " + Request.Url.ToString();
        switch (Lingua)
        {
            case "I":
                if (!host.EndsWith(".it")) Response.Redirect(Request.Url.ToString().Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit")), true);
                break;
            case "GB":
                if (!host.EndsWith(".com")) Response.Redirect(Request.Url.ToString().Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen")), true);
                break;
            case "RU":
                if (!host.EndsWith(".ru")) Response.Redirect(Request.Url.ToString().Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru")), true);
                break;

            default:
                break;
        }
    }


    #region GESTIONE MENU E LINKS

    //private void CaricaMenu()
    //{
    //    //carichiamo i link per le pagine dinamiche in base al tbl rif attività
    //    //CaricaMenuContenuti(2, 2, rptTipologieLink2High); //Inserisco il link  nel menu
    //    //CaricaMenuSezioniContenuto("rif000002", rptTipologieLink8High); //Inserisco il link  nel menu
    //    //CaricaMenuSottoSezioniContenuto("rif000001", "prod000017", rptTipologieLink6High);
    //    //Carica i link menu per le pagine statiche in base all'id in tabella
    //    //CaricaMenuLinkContenuti(1);
    //    //CaricaMenuLinkContenuti(2);
    //    //CaricaMenuLinkContenuti(4);
    //    //CaricaMenuLinkContenuti(5);
    //}
    //private void CaricaBannersAndControls()
    //{
    //    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    //    string sezionebannersnavigazione = "";
    //    switch (CodiceTipologia)
    //    {

    //        default:
    //            sezionebannersnavigazione = "banner-portfolio-sezioni";
    //            sb.Clear();

    //            sb.Append("<section id=\"header3-2k\" class=\"mbr-section mbr-section__container article\" style=\"background-color: #ffffff; padding-top: 40px; padding-bottom: 20px;\">");
    //            sb.Append("  <div class=\"container\">");
    //            sb.Append("      <div class=\"row\">");
    //            sb.Append("          <div class=\"col-xs-12\">");
    //            sb.Append("             <h3 class=\"mbr-section-title display-2\">" + references.ResMan("basetext", Lingua, "testoCatalogonav1") + "</h3>");
    //            sb.Append("             <small class=\"mbr-section-subtitle\">" + references.ResMan("basetext", Lingua, "testoCatalogonav2") + "</small>");
    //            sb.Append("         </div>");
    //            sb.Append("     </div>");
    //            sb.Append(" </div>");
    //            sb.Append("</section>");
    //            sb.Append("<div id=\"divnavigazioneJs0\" class=\"inject\" params=\"");
    //            sb.Append("injectPortfolioAndLoadBanner,'IsotopeBanner4.html','divnavigazioneJs0', 'isoBan1', 1, 1, false, '','4','','TBL_BANNERS_GENERALE','" + sezionebannersnavigazione + "',false");
    //            sb.Append("\"></div>");
    //            plhNavigazione.Text = sb.ToString();

    //            break;
    //    }

    //}



    private List<SProdotto> RiordinaSpecialeSottoprodotti(List<SProdotto> prodotti, string orderedcodes)
    {
        List<SProdotto> orderedlist = new List<SProdotto>();
        if (orderedcodes != "")
        {
            string[] codes = orderedcodes.Split(',');
            List<string> list = new List<string>();
            list = codes.ToList<string>();
            if (list != null)
            {
                //list.ForEach(l => orderedlist.Add(prodotti.Find(i => i.CodiceProdotto == l)));
                foreach (string l in list)
                {
                    SProdotto p = prodotti.Find(i => i.CodiceSProdotto == l);
                    if (p != null)
                        orderedlist.Add(p);
                }
            }

        }
        else
            orderedlist = prodotti;
        return orderedlist;
    }

    private List<Prodotto> RiordinaSpeciale(List<Prodotto> prodotti, string orderedcodes)
    {
        List<Prodotto> orderedlist = new List<Prodotto>();
        if (orderedcodes != "")
        {
            string[] codes = orderedcodes.Split(',');
            List<string> list = new List<string>();
            list = codes.ToList<string>();
            if (list != null)
            {
                //list.ForEach(l => orderedlist.Add(prodotti.Find(i => i.CodiceProdotto == l)));
                foreach (string l in list)
                {
                    Prodotto p = prodotti.Find(i => i.CodiceProdotto == l);
                    if (p != null)
                        orderedlist.Add(p);
                }
            }
            else
                orderedlist = prodotti;
        }
        return orderedlist;
    }
    protected void CaricaMenuLinkContenuti(long id)
    {

        //Contenuti item = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ContentIDSelected);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        contenutiDM conDM = new contenutiDM();
        Contenuti item = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id.ToString());

        //Creiamo i link
        if (item != null)
        {
            string testo = item.TitolobyLingua(Lingua);

            HtmlAnchor _tmp = null;
            _tmp = ((HtmlAnchor)Page.Master.FindControl("linkid" + id.ToString() + "High"));
            if (_tmp != null)
                //_tmp.HRef = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000");
                _tmp.HRef = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000", "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);


            _tmp = ((HtmlAnchor)Page.Master.FindControl("linkid" + id.ToString() + "Lateral"));
            if (_tmp != null)
                //_tmp.HRef = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000");
                _tmp.HRef = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000", "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

            _tmp = ((HtmlAnchor)Page.Master.FindControl("linkid" + id.ToString()));
            if (_tmp != null)
                //_tmp.HRef = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000");
                _tmp.HRef = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000", "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

        }
    }

    public string CreaLinkPersonalizzato(string testo, string tipologia, string qstring = "")
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        WelcomeLibrary.DOM.TipologiaOfferte sezione =
            WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == tipologia); });
        if (sezione == null) return "";
        //string link = CommonPage.CreaLinkRoutes(Session, false, Lingua, sezione.Descrizione, "", tipologia, "");
        string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, sezione.Descrizione, "", tipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

        if (!string.IsNullOrEmpty(qstring))
            link += "?" + qstring;
        sb.Append("<a href=\"");
        sb.Append(link);
        sb.Append("\"");
        if (CodiceTipologia == tipologia && Vetrina != "")
            sb.Append(" style=\"font-weight:500 !important\"  ");
        //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
        sb.Append(" >");
        //sb.Append("<span >" + testo.ToLower() + "</span>");
        sb.Append("<span >" + testo + "</span>");
        sb.Append("</a>");

        return sb.ToString();
    }
    /*NEW CREAZIONE LINK MENU SUI 3 LIVELLI*/

    public string CreaLinkPaginastatica(int idps, bool noli = false, string classe = "", string stile = "font-weight:600 !important")
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        contenutiDM conDM = new contenutiDM();
        Contenuti item = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idps.ToString());
        //Creiamo i link
        if (item != null)
        {
            string testo = item.TitolobyLingua(Lingua);
            //string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), idps.ToString(), "con001000");
            string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), idps.ToString(), "con001000", "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

            testo = references.ResMan("Common", Lingua, "testoid" + idps);
            if (!noli) sb.Append("<li>");
            sb.Append("<a  href=\"");
            sb.Append(link);
            sb.Append("\"");
            sb.Append(" class=\"" + classe + "\"   ");
            if (idps.ToString() == idContenuto)
                sb.Append(" style=\"" + stile + "\"  ");
            sb.Append(" >");
            sb.Append(testo);
            sb.Append("</a>");
            if (!noli) sb.Append("</li>");
            /*
                 <a id="linkid10High" onclick="JsSvuotaSession(this)" runat="server" href="#">
                                        <%= references.ResMan("Common", Lingua,"testoid10") %>
                                    </a>
             */
        }
        return sb.ToString();
    }

    public string CrealinkCaratteristica(int min, int max, int progressivocaratteristica, string classoop = "", bool noli = false, string fileteredcarcodes = "")
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua); });
        sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < min || Convert.ToInt32(t.Codice.Substring(3)) > max);
        sezioni.Sort(new GenericComparer<TipologiaOfferte>("Codice", System.ComponentModel.ListSortDirection.Descending));

        List<string> list = new List<string>();
        if (fileteredcarcodes != "")
        {
            string[] codes = fileteredcarcodes.Split(',');
            list = codes.ToList<string>();
        }


        if (sezioni != null)
            foreach (TipologiaOfferte o in sezioni)
            {
                if (progressivocaratteristica == 1)
                    foreach (Tabrif elem in Utility.Caratteristiche[0])
                    {
                        Dictionary<string, string> addpars = new Dictionary<string, string>();
                        if (elem != null && !string.IsNullOrEmpty(elem.Codice) && elem.Lingua == Lingua)
                        {
                            if (list.Count > 0 && !list.Exists(c => c == elem.Codice)) continue; //salto i codici se richiesto il filtraggio

                            addpars.Add("Caratteristica1", elem.Codice);
                            //Genero il link per la tipologia
                            string testo = elem.Campo1;
                            string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", o.Codice, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars);
                            link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                            if (!noli)
                                sb.Append("<li>");
                            sb.Append("<a href=\"");
                            sb.Append(link);
                            sb.Append("\"");
                            if (!string.IsNullOrEmpty(classoop))
                                sb.Append(" class=\"" + classoop + "\"  ");
                            if (o.Codice == CodiceTipologia && Caratteristica2 == elem.Codice)
                                sb.Append(" style=\"font-weight:600 !important\"  ");
                            sb.Append(" >");
                            string testoforced = references.ResMan("Common", Lingua, "testolink" + elem.Codice);
                            if (!string.IsNullOrEmpty(testoforced)) testo = testoforced;
                            sb.Append(testo);
                            sb.Append("</a>");
                            if (!noli)
                                sb.Append("</li>");
                        }
                    }


                if (progressivocaratteristica == 2)
                    foreach (Tabrif elem in Utility.Caratteristiche[1])
                    {
                        Dictionary<string, string> addpars = new Dictionary<string, string>();
                        if (elem != null && !string.IsNullOrEmpty(elem.Codice) && elem.Lingua == Lingua)
                        {
                            if (list.Count > 0 && !list.Exists(c => c == elem.Codice)) continue; //salto i codici se richiesto il filtraggio

                            addpars.Add("Caratteristica2", elem.Codice);
                            //Genero il link per la tipologia
                            string testo = elem.Campo1;
                            string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", o.Codice, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars);
                            link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                            if (!noli)
                                sb.Append("<li>");
                            sb.Append("<a href=\"");
                            sb.Append(link);
                            sb.Append("\"");
                            if (!string.IsNullOrEmpty(classoop))
                                sb.Append(" class=\"" + classoop + "\"  ");
                            if (o.Codice == CodiceTipologia && Caratteristica2 == elem.Codice)
                                sb.Append(" style=\"font-weight:600 !important\"  ");
                            sb.Append(" >");
                            string testoforced = references.ResMan("Common", Lingua, "testolink" + elem.Codice);
                            if (!string.IsNullOrEmpty(testoforced)) testo = testoforced;
                            sb.Append(testo);
                            sb.Append("</a>");
                            if (!noli)
                                sb.Append("</li>");
                        }
                    }
            }
        return sb.ToString();
    }

    public string CrealinkCaratteristicaAutocolumn(int min, int max, int progressivocaratteristica, string classoop = "", bool noli = false, int maxrighepercolonna = 10)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua); });
        sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < min || Convert.ToInt32(t.Codice.Substring(3)) > max);
        sezioni.Sort(new GenericComparer<TipologiaOfferte>("Codice", System.ComponentModel.ListSortDirection.Descending));
        if (sezioni != null)
            foreach (TipologiaOfferte o in sezioni)
            {
                if (progressivocaratteristica == 2)
                {

                    List<Tabrif> gruppocompleto = Utility.Caratteristiche[1].FindAll(e => e.Lingua == Lingua);

                    //Incolonniamo automaticamente
                     int nlink = gruppocompleto.Count;
                    int resto = 0;
                    int colonne = Math.DivRem(nlink, maxrighepercolonna, out resto);
                    if (resto > 0) colonne += 1;

                    for (int i = 1; i <= colonne; i++)
                    {
                        int elementrange = maxrighepercolonna;
                        if (i == colonne && resto != 0) elementrange = resto;
                        else if (i == colonne && resto == 0) continue;
                        List<Tabrif> gruppoattuale = gruppocompleto.GetRange((i - 1) * maxrighepercolonna, elementrange);
                        sb.Append("<div class=\"col-auto text-left megamenu-menulist\">");
                        foreach (Tabrif elem in gruppoattuale)
                        {
                            Dictionary<string, string> addpars = new Dictionary<string, string>();
                            if (elem != null && !string.IsNullOrEmpty(elem.Codice) && elem.Lingua == Lingua)
                            {
                                addpars.Add("Caratteristica2", elem.Codice);
                                //Genero il link per la tipologia
                                string testo = elem.Campo1;
                                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", o.Codice, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars);
                                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                                sb.Append("<ul>");
                                if (!noli)
                                    sb.Append("<li>");
                                sb.Append("<a href=\"");
                                sb.Append(link);
                                sb.Append("\"");
                                if (!string.IsNullOrEmpty(classoop))
                                    sb.Append(" class=\"" + classoop + "\"  ");
                                if (o.Codice == CodiceTipologia && Caratteristica2 == elem.Codice)
                                    sb.Append(" style=\"font-weight:600 !important\"  ");
                                sb.Append(" >");
                                string testoforced = references.ResMan("Common", Lingua, "testolink" + elem.Codice);
                                if (!string.IsNullOrEmpty(testoforced)) testo = testoforced;
                                sb.Append(testo);
                                sb.Append("</a>");
                                if (!noli)
                                    sb.Append("</li>");
                                sb.Append("</ul>");

                            }
                        }
                        sb.Append("</div>");


                    }
                }
            }
        return sb.ToString();
    }


    /// <summary>
    /// Creazione lista li per tipologie da min a max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public string CreaLinkTipologie(int min, int max, string classoop = "", bool noli = false)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua); });
        sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < min || Convert.ToInt32(t.Codice.Substring(3)) > max);
        sezioni.Sort(new GenericComparer<TipologiaOfferte>("Codice", System.ComponentModel.ListSortDirection.Descending));
        if (sezioni != null)
            foreach (TipologiaOfferte o in sezioni)
            {
                string testo = o.Descrizione;
                //string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.Codice);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", o.Codice, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                if (!noli)
                    sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (!string.IsNullOrEmpty(classoop))
                    sb.Append(" class=\"" + classoop + "\"  ");
                if (o.Codice == CodiceTipologia)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");

                string testoforced = references.ResMan("Common", Lingua, "testolink" + o.Codice);
                if (!string.IsNullOrEmpty(testoforced)) testo = testoforced;
                sb.Append(testo);
                sb.Append("</a>");
                if (!noli)
                    sb.Append("</li>");
            }
        return sb.ToString();
    }



    /// <summary>
    /// Creazione lista li con sottolivelli per le tipologie indicate , depth=0 solo lista 1 primo livello categoria, depth=1 primo e secondo livello categoria e sottocategoria
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="depth"></param>
    /// <param name="ulvisibility">Rende le sottoliste sempre visibili</param>
    /// <returns></returns>
    public string CreaLinkTipologieNested(int min, int max, int depth = 0, bool ulvisibility = false)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua); });
        sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < min || Convert.ToInt32(t.Codice.Substring(3)) > max);
        sezioni.Sort(new GenericComparer<TipologiaOfferte>("Codice", System.ComponentModel.ListSortDirection.Descending));
        if (sezioni != null)
            foreach (TipologiaOfferte o in sezioni)
            {
                string testo = o.Descrizione;
                //string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.Codice);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", o.Codice, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);


                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                sb.Append("<li>");
                sb.Append("<a  href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (o.Codice == CodiceTipologia)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");
                sb.Append(testo);
                sb.Append("</a>");

                /*Nested level*/
                string sottomenu = "";
                if (depth == 0) sottomenu = CreaLinkCategorie(o.Codice);
                if (depth == 1) sottomenu = CreaLinkCategorieNested(o.Codice);
                if (!string.IsNullOrEmpty(sottomenu))
                {
                    string ulstyle = "style =\"display: none;\"";
                    if (ulvisibility) ulstyle = "";
                    sb.Append("<ul class=\"dropdown\" " + ulstyle + " >");
                    sb.Append(sottomenu);
                    sb.Append("</ul>");
                }

                sb.Append("</li>");
            }
        return sb.ToString();
    }

    /// <summary>
    /// Creazione lista li delle categorie per la tipologia indicata
    /// </summary>
    /// <param name="tipologia"></param>
    /// <param name="filtercode"></param>
    /// <returns></returns>
    public string CreaLinkCategorie(string tipologia, string filtercode = "", string classop = "", bool noli = false)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == tipologia)); });
        //prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        if (filtercode != "")
        {
            string[] codes = filtercode.Split(',');
            List<string> list = new List<string>();
            list = codes.ToList<string>();
            prodotti = prodotti.FindAll(i => list.Exists(l => l == i.CodiceProdotto));
        }
        //prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
        prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));

        //if (tipologia == "rif000001")
        //    prodotti = RiordinaSpeciale(prodotti, "prod000020,prod000013,prod000014,prod000015,prod000017,prod000016,prod000021,prod000022,prod000023,prod000027,prod000024,prod000029,prod000026,prod000001");

        if (prodotti != null)
            foreach (Prodotto o in prodotti)
            {
                string testo = o.Descrizione;
                //string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.CodiceTipologia, o.CodiceProdotto);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", o.CodiceTipologia, o.CodiceProdotto, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                if (!noli)
                    sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (!string.IsNullOrEmpty(classop))
                    sb.Append(" class=\"" + classop + "\"  ");
                if (o.CodiceProdotto == Categoria)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");

                string testoforced = references.ResMan("Common", Lingua, "testolink" + o.CodiceProdotto);
                if (!string.IsNullOrEmpty(testoforced)) testo = testoforced;

                sb.Append(testo);
                sb.Append("</a>");
                if (!noli)
                    sb.Append("</li>");
            }
        return sb.ToString();
    }



    /// <summary>
    /// Creazione lista li delle categorie e sottocategorie per la tipologia indicata
    /// </summary>
    /// <param name="tipologia"></param>
    /// <param name="filtercode"></param>
    /// <param name="ulvisibility"></param>
    /// <returns></returns>
    public string CreaLinkCategorieNested(string tipologia, string filtercode = "", bool ulvisibility = false)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == tipologia)); });
        //prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        if (filtercode != "")
        {
            string[] codes = filtercode.Split(',');
            List<string> list = new List<string>();
            list = codes.ToList<string>();
            prodotti = prodotti.FindAll(i => list.Exists(l => l == i.CodiceProdotto));
        }
        if (prodotti != null)
        {
            //prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
            prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));

            //if (tipologia == "rif000001")
            //    prodotti = RiordinaSpeciale(prodotti, "prod000020,prod000013,prod000014,prod000015,prod000017,prod000016,prod000021,prod000022,prod000023,prod000027,prod000024,prod000029,prod000026,prod000001");


            foreach (Prodotto o in prodotti)
            {
                string testo = o.Descrizione;
                //string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.CodiceTipologia, o.CodiceProdotto);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", o.CodiceTipologia, o.CodiceProdotto, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                sb.Append("<li>");
                sb.Append("<a  href=\"");
                sb.Append(link);
                sb.Append("\"");

                if (o.CodiceProdotto == Categoria)
                    sb.Append(" style=\"font-weight:600 !important;display:block;\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");
                sb.Append(testo);
                sb.Append("</a>");

                /*Nested level*/
                string sottomenu = CreaLinkSottoCategorie(o.CodiceTipologia, o.CodiceProdotto);
                if (!string.IsNullOrEmpty(sottomenu))
                {
                    string ulstyle = "style =\"display: none;\"";
                    //if (ulvisibility) ulstyle = "";
                    if (o.CodiceProdotto == Categoria) ulstyle = "";
                    sb.Append("<ul class=\"dropdown\" " + ulstyle + " >");
                    sb.Append(sottomenu);
                    sb.Append("</ul>");
                }
                sb.Append("</li>");
            }
        }

        return sb.ToString();
    }


    public string CreaLinkCategorieNestedNodropdown(string tipologia, string filtercode = "", string classop = "", bool noli = true, string localCaratteristica1 = "")
    {
        Dictionary<string, string> addpars = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(localCaratteristica1)) addpars.Add("Caratteristica1", localCaratteristica1);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == tipologia)); });
        //prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        if (filtercode != "")
        {
            string[] codes = filtercode.Split(',');
            List<string> list = new List<string>();
            list = codes.ToList<string>();
            prodotti = prodotti.FindAll(i => list.Exists(l => l == i.CodiceProdotto));
        }
        if (prodotti != null)
        {
            //prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
            prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));

            //if (tipologia == "rif000001")
            //    prodotti = RiordinaSpeciale(prodotti, "prod000020,prod000013,prod000014,prod000015,prod000017,prod000016,prod000021,prod000022,prod000023,prod000027,prod000024,prod000029,prod000026,prod000001");

            foreach (Prodotto o in prodotti)
            {
                string testo = o.Descrizione;
                //string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.CodiceTipologia, o.CodiceProdotto);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", o.CodiceTipologia, o.CodiceProdotto, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars);

                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                if (!noli)
                    sb.Append("<li>");
                sb.Append("<a  href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (!string.IsNullOrEmpty(classop))
                    sb.Append(" class=\"" + classop + "\"  ");
                if ((o.CodiceProdotto == Categoria) && (localCaratteristica1 == Caratteristica1))
                    sb.Append(" style=\"font-weight:600 !important;display:block;\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");
                sb.Append(testo);
                sb.Append("</a>");
                if (!noli)
                    sb.Append("</li>");

                /*Nested level categoria 2 liv*/
                string sottomenu = CreaLinkSottoCategorie(o.CodiceTipologia, o.CodiceProdotto, "", "", false, localCaratteristica1);
                if (!string.IsNullOrEmpty(sottomenu))
                {
                    sb.Append("<ul>");
                    sb.Append(sottomenu);
                    sb.Append("</ul>");
                }

            }
        }

        return sb.ToString();
    }


    // <summary>
    /// 
    /// </summary>
    /// <param name="tipologia"></param>
    /// <param name="categoria"></param>
    /// <param name="filtercode"></param>
    /// <param name="classop"></param>
    /// <param name="noli"></param>
    /// <param name="localCaratteristica1"></param>
    /// <returns></returns>
    public string CreaLinkSottoCategorie(string tipologia, string categoria, string filtercode = "", string classop = "", bool noli = false, string localCaratteristica1 = "")
    {
        Dictionary<string, string> addpars = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(localCaratteristica1)) addpars.Add("Caratteristica1", localCaratteristica1);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == categoria)); });
        //prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        sprodotti.Sort(new GenericComparer<SProdotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));

        if (filtercode != "")
        {
            string[] codes = filtercode.Split(',');
            List<string> list = new List<string>();
            list = codes.ToList<string>();
            sprodotti = sprodotti.FindAll(i => list.Exists(l => l == i.CodiceSProdotto));
        }
        //  sprodotti = RiordinaSpecialeSottoprodotti(sprodotti, filtercode);
        if (sprodotti != null)
            foreach (SProdotto o in sprodotti)
            {
                string testo = o.Descrizione;
                //string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", tipologia, o.CodiceProdotto, o.CodiceSProdotto);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(testo), "", tipologia, o.CodiceProdotto, o.CodiceSProdotto, "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars);

                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (!noli)
                    sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (!string.IsNullOrEmpty(classop))
                    sb.Append(" class=\"" + classop + "\"  ");
                if ((o.CodiceSProdotto == Categoria2liv) && (localCaratteristica1 == Caratteristica1))
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");
                sb.Append(testo);
                sb.Append("</a>");
                if (!noli)
                    sb.Append("</li>");
            }
        return sb.ToString();
    }

    /*NEW CREAZIONE LINK MENU SUI 3 LIVELLI*/

    public string CaricaLinksPerTipologiaImmobili(string tipologia, string ordinamento = "")
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == tipologia); });
        if (item != null)
        {
            Dictionary<string, string> tipologieimmobili = references.GetreftipologieValues(Lingua);
            foreach (KeyValuePair<string, string> t in tipologieimmobili)
            {
                string testourl = item.Descrizione + "-" + t.Value;
                string testolink = t.Value;

                //string link = CommonPage.CreaLinkRoutes(Session, false, Lingua, testourl, "", tipologia, t.Key);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, testourl, "", tipologia, t.Key, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (tipologia == CodiceTipologia && t.Key == Categoria)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");
                sb.Append(testolink);
                sb.Append("</a>");
                sb.Append("</li>");

            }
        }
        return sb.ToString();
    }
    public string CaricaLinksImmobiliFiltrati(string tipologia)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //objfiltro
        Dictionary<string, string> objvalue = new Dictionary<string, string>();
        objvalue.Add("vetrina", "true");
        string sobjvalye = Newtonsoft.Json.JsonConvert.SerializeObject(objvalue);
        Dictionary<string, List<string>> linksbyid = references.CreaFilteredResourcesLinks(HttpContext.Current, sobjvalye, Lingua, tipologia, 15);
        //Request.RequestContext.HttpContext
        if (linksbyid != null)
            foreach (KeyValuePair<string, List<string>> t in linksbyid)
            {
                string testourl = t.Value[0];
                string link = t.Value[1];
                string idact = t.Key;

                //link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                //if (idOfferta == )
                //    sb.Append(" style=\"font-weight:600 !important\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");
                sb.Append("<span >" + testourl.ToLower() + "</span>");
                sb.Append("</a>");
                sb.Append("</li>");

            }

        return sb.ToString();
    }

    public string TestoSezione(string codicetipologia, bool solotitolo = false, bool nosezione = false, string qstring = "")
    {
        string ret = "";
        WelcomeLibrary.DOM.TipologiaOfferte sezione =
              WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codicetipologia); });
        if (sezione != null)
        {
            string addtext = " " + references.ResMan("Common", Lingua, "testoSezione").ToString();
            if (nosezione) addtext = "";
            ret += addtext + CommonPage.ReplaceAbsoluteLinks(CommonPage.CrealinkElencotipologia(codicetipologia, Lingua, Session, "link1", false, qstring));

            if (solotitolo)
                ret = sezione.Descrizione;
        }

        return ret;
    }
    public string TestoSezioneCategoria(string codicetipologia, string codicecategoria, bool solotitolo = false, bool nosezione = false)
    {
        string ret = "";
        Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == codicetipologia && tmp.CodiceProdotto == codicecategoria)); });
        if (catselected != null)
        {
            string addtext = " " + references.ResMan("Common", Lingua, "testoSezione").ToString();
            if (nosezione) addtext = "";
            ret += addtext + CommonPage.ReplaceAbsoluteLinks(CommonPage.CrealinkElencotipologiaCategoria(codicetipologia, codicecategoria, Lingua, Session, "link1", false));
            if (solotitolo)
                ret = catselected.Descrizione;
        }
        return ret;
    }
    public string CaricaLinksPerTipologia(string tipologia, string Categoria = "", string maxlinks = "6", string ordinamento = "", string codicerisorsa = "", string classop = "", bool noli = false, string Sottocategoria = "", bool solotitolo = false)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //Filtriamo alcune categorie
        string tipologiadacaricare = tipologia;
        offerteDM offDM = new offerteDM();
        //OfferteCollection offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, "6", false, Lingua, false);

        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
        SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", tipologiadacaricare);
        parColl.Add(p3);
        if (Categoria.Trim() != "")
        {
            SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", Categoria);
            parColl.Add(p7);
        }
        if (Sottocategoria.Trim() != "")
        {
            SQLiteParameter p8 = new SQLiteParameter("@CodiceCategoria2Liv", Sottocategoria);
            parColl.Add(p8);
        }

        OfferteCollection offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, maxlinks, Lingua, null, ordinamento);  //Creiamo i link
        if (offerte != null)
            foreach (Offerte o in offerte)
            {
                string testo = o.DenominazionebyLingua(Lingua);

                string testofroresources = references.ResMan("Common", Lingua, codicerisorsa);
                if (!string.IsNullOrEmpty(testofroresources))
                    testo = testofroresources;

                //aggunta per solo titoli
                if (solotitolo)
                    testo = offDM.estraititolo(o, Lingua);


                //string link = CommonPage.CreaLinkRoutes(Session, false, Lingua, CommonPage.CleanUrl(o.UrltextforlinkbyLingua(Lingua)), o.Id.ToString(), o.CodiceTipologia, o.CodiceCategoria);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(o.UrltextforlinkbyLingua(Lingua)), o.Id.ToString(), o.CodiceTipologia, o.CodiceCategoria, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (!noli)
                    sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (!string.IsNullOrEmpty(classop))
                    sb.Append(" class=\"" + classop + "\"  ");
                if (o.Id.ToString() == idOfferta)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                //sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                sb.Append(" >");
                sb.Append(testo);
                sb.Append("</a>");
                if (!noli)
                    sb.Append("</li>");
            }
        return sb.ToString();
    }
    public void CaricaMenuContenuti(int min, int max, Repeater rptlist)
    {
        List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua); });
        sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < min || Convert.ToInt32(t.Codice.Substring(3)) > max);
        sezioni.Sort(new GenericComparer<TipologiaOfferte>("Codice", System.ComponentModel.ListSortDirection.Descending));


        rptlist.DataSource = sezioni;
        rptlist.DataBind();
    }
    public void CaricaMenuSezioniContenuto(string tipologia, Repeater rptlist, string filtercode = "")
    {
        List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == tipologia)); });
        //prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        if (filtercode != "")
        {
            string[] codes = filtercode.Split(',');
            List<string> list = new List<string>();
            list = codes.ToList<string>();
            prodotti = prodotti.FindAll(i => list.Exists(l => l == i.CodiceProdotto));
        }

        prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));

        rptlist.DataSource = prodotti;
        rptlist.DataBind();
    }
    public void CaricaMenuSottoSezioniContenuto(string tipologia, string categoria, Repeater rptlist, string filtercode = "")
    {
        List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == categoria)); });
        //prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        sprodotti.Sort(new GenericComparer<SProdotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));

        if (filtercode != "")
        {
            string[] codes = filtercode.Split(',');
            List<string> list = new List<string>();
            list = codes.ToList<string>();
            sprodotti = sprodotti.FindAll(i => list.Exists(l => l == i.CodiceSProdotto));
        }
        rptlist.DataSource = sprodotti;
        rptlist.DataBind();
    }
    //public string CaricaLinkSottoSezioniContenuto(string tipologia, string categoria)
    //{
    //    string ret = "";
    //    //List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == tipologia)); });
    //    //prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
    //    List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == categoria)); });
    //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //    if (sprodotti != null)
    //        foreach (SProdotto o in sprodotti)
    //        {

    //            string testo = o.Descrizione;
    //            string link = CommonPage.CreaLinkRoutes(null, false, Lingua, CommonPage.CleanUrl(testo), "", tipologia, o.CodiceProdotto, o.CodiceSProdotto);
    //            link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
    //            sb.Append("<li>");
    //            sb.Append("<div class=\"divbuttonstyle\" style=\"margin-right: 5px;  font-size: 0.8em; padding: 2px;\" >");
    //            sb.Append("<a href=\"");
    //            sb.Append(link);
    //            sb.Append("\" onclick=\"javascript:JsSvuotaSession(this)\"  >");
    //            sb.Append("<span >" + testo + "</span>");
    //            sb.Append("</a>");
    //            sb.Append("</div>");
    //            sb.Append("</li>");
    //        }
    //    ret = sb.ToString();
    //    if (ret != "")
    //        pnlCategorie.Visible = true;
    //    return ret;
    //}




    protected void rptTipologieLink_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item != null && (e.Item.ItemType == ListItemType.Header))
        {
        }
        if (e.Item.DataItem != null && e.Item != null && (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item))
        {
            if (((WelcomeLibrary.DOM.TipologiaOfferte)e.Item.DataItem).Codice == CodiceTipologia && Vetrina == "")
            {
                HtmlAnchor linkmenu = ((HtmlAnchor)e.Item.FindControl("linkRubriche"));
                if (linkmenu != null)
                {
                    //linkmenu.Style.Add(HtmlTextWriterStyle.Color, "Dark Blue");
                    //HtmlGenericControl divpulsante = ((HtmlGenericControl)e.Item.FindControl("divPulsante"));
                    //divpulsante.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e5e5e5");
                    try
                    {
                        linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                        //((HtmlGenericControl)linkmenu.Parent.Parent.Parent.Parent).Attributes.Add("class", "select");
                        //  ((HtmlGenericControl)linkmenu.Parent.Parent.Parent.Parent).Attributes["class"] += " active";
                        if (linkmenu != null)
                        {
                            Control lidrop = CommonPage.FindControlRecursive(linkmenu.Parent.Parent.Parent, linkmenu.Parent.Parent.Parent.ID);
                            if (lidrop != null)
                            {
                                ((HtmlGenericControl)lidrop).Attributes["class"] += " active";
                            }
                        }
                    }
                    catch { }
                }
            }
        }
    }



    protected void rptTipologieLinkSezioni_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item != null && (e.Item.ItemType == ListItemType.Header))
        {
        }
        if (e.Item.DataItem != null && e.Item != null && (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item))
        {
            if (((WelcomeLibrary.DOM.Prodotto)e.Item.DataItem).CodiceProdotto == Categoria)
            {
                HtmlAnchor linkmenu = ((HtmlAnchor)e.Item.FindControl("linkRubriche"));
                if (linkmenu != null)
                {   //linkmenu.Style.Add(HtmlTextWriterStyle.Color, "Dark Blue");
                    //HtmlGenericControl divpulsante = ((HtmlGenericControl)e.Item.FindControl("divPulsante"));
                    //divpulsante.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e5e5e5");
                    try
                    {
                        //((HtmlGenericControl)linkmenu.Parent).Attributes.Add("class", "active");

                        linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                        //linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#4a4b4c");

                        if (linkmenu != null)
                        {
                            Control lidrop = CommonPage.FindControlRecursive(linkmenu.Parent.Parent.Parent, linkmenu.Parent.Parent.Parent.ID);
                            if (lidrop != null)
                            {
                                ((HtmlGenericControl)lidrop).Attributes["class"] += " active";
                            }
                        }

                    }
                    catch { }
                    try
                    {
                        //((HtmlGenericControl)linkmenu.Parent.Parent.Parent.Parent).Attributes["class"] += " active";
                    }
                    catch { }
                }
            }
        }
    }

    protected void rptTipologieLinkSottoSezioni_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item != null && (e.Item.ItemType == ListItemType.Header))
        {
        }
        if (e.Item.DataItem != null && e.Item != null && (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item))
        {
            if (((WelcomeLibrary.DOM.SProdotto)e.Item.DataItem).CodiceProdotto == Categoria)
            {
                HtmlAnchor linkmenu = ((HtmlAnchor)e.Item.FindControl("linkRubriche"));
                if (linkmenu != null)
                {   //linkmenu.Style.Add(HtmlTextWriterStyle.Color, "Dark Blue");
                    //HtmlGenericControl divpulsante = ((HtmlGenericControl)e.Item.FindControl("divPulsante"));
                    //divpulsante.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e5e5e5");
                    try
                    {
                        //((HtmlGenericControl)linkmenu.Parent).Attributes.Add("class", "active");

                        linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                        //linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#4a4b4c");

                        if (linkmenu != null)
                        {
                            Control lidrop = CommonPage.FindControlRecursive(linkmenu.Parent.Parent.Parent, linkmenu.Parent.Parent.Parent.ID);
                            if (lidrop != null)
                            {
                                ((HtmlGenericControl)lidrop).Attributes["class"] += " active";
                            }
                        }

                    }
                    catch { }
                    try
                    {
                        //((HtmlGenericControl)linkmenu.Parent.Parent.Parent.Parent).Attributes["class"] += " active";
                    }
                    catch { }
                }
            }
        }
    }

    #endregion

    private void SettaTestoIniziale(string sezione)
    {
        if (litTextHeadPage.Text == "")
        {
            string htmlPage = "";
            if (references.ResMan("Common", Lingua, "Content" + sezione) != null)
                htmlPage = CommonPage.ReplaceLinks(references.ResMan("Common", Lingua, "Content" + sezione).ToString());
            litTextHeadPage.Text = htmlPage;
            string strigaperricerca = sezione;
            //strigaperricerca = Request.Url.AbsolutePath;
            //strigaperricerca = strigaperricerca.ToLower().Replace("index.aspx", "home");
            contenutiDM conDM = new contenutiDM();
            Contenuti content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            if (content != null && content.Id != 0)
            {
                htmlPage = CommonPage.ReplaceLinks(content.DescrizionebyLingua(Lingua));

                litTextHeadPage.Text = CommonPage.ReplaceAbsoluteLinks(htmlPage);
            }
            divRicerca.Visible = true;
        }
    }


    protected void btnContatti1_Click(object sender, EventArgs e)
    {
        try
        {
            //Prepariamo e inviamo il mail
            string nomemittente = txtContactName.Value;
            string mittenteMail = txtContactEmail.Value;
            string mittenteTelefono = txtContactTelefono.Value;
            string nomedestinatario = CommonPage.Nome;
            string maildestinatario = CommonPage.Email;
            long idperstatistiche = 0;

            string tipo = "informazioni";
            string SoggettoMail = "Richiesta " + tipo + " da " + nomemittente + " tramite il sito " + CommonPage.Nome;
            string Descrizione = txtContactMessage.Value.Replace("\r", "<br/>") + " <br/> ";

            Descrizione += " <br/> Telefono Cliente: " + mittenteTelefono + "  Email Cliente: " + mittenteMail + " Lingua Cliente: " + Lingua;
            Descrizione += " <br/> Il cliente ha Confermato l'autorizzazione al trattamento dei dati personali ";
            if (idOfferta != "") //Inseriamo il dettaglio della scheda di provenienza
            {
                offerteDM offDM = new offerteDM();
                Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
                if (item != null && item.Id != 0)
                {
                    long.TryParse(idOfferta, out idperstatistiche);
                    if (!string.IsNullOrWhiteSpace(item.Email)) //Se non è vuota mando alla mail indicata nell'articolo
                    {
                        nomedestinatario = item.Email;
                        maildestinatario = item.Email;
                    }
                    Descrizione += "<br/><br/>";
                    Descrizione += "Pagina provenienza: " + item.DenominazioneI + " id:" + idperstatistiche;
                    Descrizione += "<br/><br/>";
                }
            }
            if (chkContactPrivacy.Checked)
            {
                //Utility.invioMailGenerico(CommonPage.Nome, CommonPage.Email, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
                Utility.invioMailGenerico(nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

                //Registro la statistica di contatto
                Statistiche stat = new Statistiche();
                stat.Data = DateTime.Now;
                stat.EmailDestinatario = maildestinatario;
                stat.EmailMittente = mittenteMail;
                stat.Idattivita = idperstatistiche;
                stat.Testomail = nomemittente + "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                stat.Url = "";
                statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);

                Response.Redirect(CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkContatti")) + "&conversione=true");

            }
            else
            {
                outputContact.Text = references.ResMan("Common", Lingua, "txtPrivacyError");
                //Mittente.Descrizione += " <br/> Non vi Autorizzo al trattamento dei miei dati personali (D.Lgs 196/2003)";
            }

        }
        catch (Exception err)
        {
            outputContact.Text = err.Message + " <br/> ";
            outputContact.Text += references.ResMan("Common", Lingua, "txtMailError");
        }
    }

    protected void InserisciNewsletter(string email)
    {
        string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        if (System.Text.RegularExpressions.Regex.IsMatch(email, pattern))
        {
            ClientiDM cliDM = new ClientiDM();
            Cliente tmp_Cliente = new Cliente();
            tmp_Cliente.Email = email;
            Session.Add("iscrivicliente", tmp_Cliente);
            string linkverifica = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti.aspx?ID_cliente=&Azione=iscrivinewsletter&Lingua=" + Lingua;
            Response.Redirect(linkverifica);
        }

    }

    protected void btnNewsletter1_Click(object sender, EventArgs e)
    {
        //Richiesta  per inserimento in anagrafica clienti !!!!!
        //Rimando alla pagina di verifica iscrizione
        ClientiDM cliDM = new ClientiDM();
        Cliente tmp_Cliente = new Cliente();
        //tmp_Cliente.Cognome = txtNome.Value;
        tmp_Cliente.Email = txtEmail.Value;
        //DateTime _d = DateTime.MinValue;
        //if (!DateTime.TryParseExact(txtDatanascita_dts.Text, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _d))
        //    tmp_Cliente.DataNascita = _d;
        Session.Add("iscrivicliente", tmp_Cliente);
        string linkverifica = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti.aspx?ID_cliente=&Azione=iscrivinewsletter&Lingua=" + Lingua;
        Response.Redirect(linkverifica);
    }

    protected bool ControlloVisibilita(object fotos)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 0) ret = false;
        bool onlypdf = (fotos != null && ((AllegatiCollection)fotos).Count > 0 && !((AllegatiCollection)fotos).Exists(c => (c.NomeFile.ToString().ToLower().EndsWith("jpg") || c.NomeFile.ToString().ToLower().EndsWith("gif") || c.NomeFile.ToString().ToLower().EndsWith("png"))));
        if (onlypdf) ret = false;
        return ret;
    }

    //protected void btnsearch1_Click(object sender, EventArgs e)
    //{

    //    HttpContext.Current.Session.Clear();
    //    //testoricerca
    //    string link = CommonPage.CreaLinkRicerca("", "-", "", "", "", "", "", "-", Lingua, Session, true);
    //    Session.Add("testoricerca", Server.HtmlEncode(txtSearchTop.Value)); //carico in sessione il parametro da cercare
    //    Response.Redirect(link);
    //}
    //protected void btnsearch_Click(object sender, EventArgs e)
    //{


    //    HttpContext.Current.Session.Clear();
    //    //testoricerca
    //    string link = CommonPage.CreaLinkRicerca("", "-", "", "", "", "", "", "-", Lingua, Session, true);
    //    Session.Add("testoricerca", Server.HtmlEncode(searchboxinputtext.Value)); //carico in sessione il parametro da cercare
    //    Response.Redirect(link);
    //}
    //protected void btnUsatoCerca_Click(object sender, EventArgs e)
    //{
    //    Session["Caratteristica1"] = ddlCaratteristica1.SelectedValue;
    //    Session["Caratteristica2"] = ddlCaratteristica2.SelectedValue;
    //    Session["Caratteristica3"] = ddlCaratteristica3.SelectedValue;
    //    Session["Caratteristica4"] = ddlCaratteristica4.SelectedValue;
    //    Session["FasciaPrezzo"] = ddlFascePrezzo.SelectedValue;
    //    Session["Vetrina"] = chkPromo.Checked;
    //    Session["Ordinamento"] = ddlOrdinamento.SelectedValue;
    //    //  Response.Redirect(references.ResMan("Common",Lingua,"linkUsato);
    //}
    //public void CaricaDdlOrdinamento(string value = "")
    //{
    //    //string tipi = references.ResMan("Common",Lingua,"listaServizi;
    //    Dictionary<string, string> dict = new Dictionary<string, string>();
    //    //string[] tipiarray = tipi.Split(',');
    //    //foreach (string s in tipiarray)
    //    //{
    //    //    dict.Add(s, s);
    //    //}
    //    dict.Add(references.ResMan("Common", Lingua, "FormOrdinamento"), "");
    //    dict.Add("Data Inserimento", "DataInserimento");
    //    dict.Add("Prezzo", "Prezzo");
    //    dict.Add("Data Immatricolazione", "Data1");

    //    ddlOrdinamento.Items.Clear();
    //    //ddlOrdinamento.AppendDataBoundItems = true;
    //    //ddlOrdinamento.Items.Insert(0, references.ResMan("Common",Lingua,"FormOrdinamento.ToString());
    //    //ddlOrdinamento.Items[0].Value = "";
    //    ddlOrdinamento.DataSource = dict;
    //    ddlOrdinamento.DataTextField = "Key";
    //    ddlOrdinamento.DataValueField = "Value";
    //    ddlOrdinamento.DataBind();
    //    try
    //    {

    //        ddlOrdinamento.SelectedValue = value;
    //    }
    //    catch
    //    { }



    //}
    //public void CaricaDatiDdlCaratteristiche(string Lingua = "I", string p1 = "0", string p2 = "0", string p3 = "0", string p4 = "0", string fasciaprezzo = "0", bool promozioni = false)
    //{

    //    //Riempio la ddl 
    //    List<Tabrif> Car1 = Utility.Caratteristiche[0].FindAll(delegate (Tabrif _t) { return _t.Lingua == Lingua; });
    //    ddlCaratteristica1.Items.Clear();
    //    ddlCaratteristica1.Items.Insert(0, references.ResMan("Common", Lingua, "selCaratteristica1"));
    //    ddlCaratteristica1.Items[0].Value = "0";
    //    ddlCaratteristica1.DataSource = Car1;
    //    ddlCaratteristica1.DataTextField = "Campo1";
    //    ddlCaratteristica1.DataValueField = "Codice";
    //    ddlCaratteristica1.DataBind();
    //    try
    //    {
    //        ddlCaratteristica1.SelectedValue = p1.ToString();
    //    }
    //    catch { }


    //    //Riempio la ddl  ( collegandola alla caratteristica 1 )
    //    List<Tabrif> Car2 = Utility.Caratteristiche[1].FindAll(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Campo2 == p1.ToString(); });
    //    ddlCaratteristica2.Items.Clear();
    //    ddlCaratteristica2.Items.Insert(0, references.ResMan("Common", Lingua, "selCaratteristica2"));
    //    ddlCaratteristica2.Items[0].Value = "0";
    //    ddlCaratteristica2.DataSource = Car2;
    //    ddlCaratteristica2.DataTextField = "Campo1";
    //    ddlCaratteristica2.DataValueField = "Codice";
    //    ddlCaratteristica2.DataBind();
    //    try
    //    {
    //        ddlCaratteristica2.SelectedValue = p2.ToString();
    //    }
    //    catch { }


    //    List<Tabrif> Car3 = Utility.Caratteristiche[2].FindAll(delegate (Tabrif _t) { return _t.Lingua == Lingua; });
    //    ddlCaratteristica3.Items.Clear();
    //    ddlCaratteristica3.Items.Insert(0, references.ResMan("Common", Lingua, "selCaratteristica3"));
    //    ddlCaratteristica3.Items[0].Value = "0";
    //    ddlCaratteristica3.DataSource = Car3;
    //    ddlCaratteristica3.DataTextField = "Campo1";
    //    ddlCaratteristica3.DataValueField = "Codice";
    //    ddlCaratteristica3.DataBind();
    //    try
    //    {
    //        ddlCaratteristica3.SelectedValue = p3.ToString();
    //    }
    //    catch { }


    //    //Riempio la ddl  
    //    List<Tabrif> Car4 = Utility.Caratteristiche[3].FindAll(delegate (Tabrif _t) { return _t.Lingua == Lingua; });
    //    ddlCaratteristica4.Items.Clear();
    //    ddlCaratteristica4.Items.Insert(0, references.ResMan("Common", Lingua, "selCaratteristica4"));
    //    ddlCaratteristica4.Items[0].Value = "0";
    //    ddlCaratteristica4.DataSource = Car4;
    //    ddlCaratteristica4.DataTextField = "Campo1";
    //    ddlCaratteristica4.DataValueField = "Codice";
    //    ddlCaratteristica4.DataBind();
    //    try
    //    {
    //        ddlCaratteristica4.SelectedValue = p4.ToString();
    //    }
    //    catch { }


    //    List<Fascediprezzo> prezzi = Utility.Fascediprezzo.FindAll(fp => fp.Lingua == Lingua && fp.CodiceTipologiaCollegata == "rif000100");
    //    ddlFascePrezzo.Items.Clear();
    //    ddlFascePrezzo.Items.Insert(0, references.ResMan("Common", Lingua, "SelezionePrezzo"));
    //    ddlFascePrezzo.Items[0].Value = "0";
    //    ddlFascePrezzo.DataSource = prezzi;
    //    ddlFascePrezzo.DataTextField = "Descrizione";
    //    ddlFascePrezzo.DataValueField = "Codice";
    //    ddlFascePrezzo.DataBind();
    //    try
    //    {
    //        ddlFascePrezzo.SelectedValue = fasciaprezzo;
    //    }
    //    catch { }

    //    chkPromo.Checked = promozioni;
    //}
    //protected void ddlCaratteristica1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    CaricaDatiDdlCaratteristiche(Lingua, ((DropDownList)(sender)).SelectedValue);
    //}

  


}
