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
            //Prendiamo i dati dalla querystring
            Lingua = CommonPage.CaricaValoreMaster(Request, Session, "Lingua", false, "I");

            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() != "true")
            {
                if (Lingua.ToLower() == "gb") Response.RedirectPermanent("~");
            }
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() != "true")
            {
                if (Lingua.ToLower() == "ru") Response.RedirectPermanent("~");
            }

            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                ControlloLingua(); // RIABILITARE PER ONLINE per reindirizzare le lingue su domini diversi

            CodiceTipologia = CommonPage.CaricaValoreMaster(Request, Session, "Tipologia", false, "");
            Categoria = CommonPage.CaricaValoreMaster(Request, Session, "Categoria", false);
            Categoria2liv = CommonPage.CaricaValoreMaster(Request, Session, "Categoria2liv", false);
            idContenuto = CommonPage.CaricaValoreMaster(Request, Session, "idContenuto");
            idOfferta = CommonPage.CaricaValoreMaster(Request, Session, "idOfferta");
            Vetrina = CommonPage.CaricaValoreMaster(Request, Session, "vetrina", true, "");

        }
        else
        {
            if (Request["__EVENTTARGET"] == "inseriscinewsletter")
            {
                string email = Request["__EVENTARGUMENT"];
                InserisciNewsletter(email);
            }

        }

        CaricaMenu();
        //CaricaBannersAndControls();
        // SettaTestoIniziale("Pannello Ricerca Sito");
        VisualizzaTotaliCarrello();
        LoadJavascriptVariables();
        //  DataBind();
        pnlRicerca.DataBind();
        divContattiMaster.DataBind();
        req1.DataBind();
        lisearch.DataBind();
    }

    private void ControllaHttp()
    {
        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https:"))
        {
            if (!System.Web.HttpContext.Current.Request.Url.ToString().StartsWith("https:"))
            {
                Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString().Replace("http:", "https:"), true);
            }

        }
        else
        {
            if (!System.Web.HttpContext.Current.Request.Url.ToString().StartsWith("http:"))
            {
                Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString().Replace("https:", "http:"), true);
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

    private void CaricaMenu()
    {
        //carichiamo i link per le pagine dinamiche in base al tbl rif attività
        //CaricaMenuContenuti(2, 2, rptTipologieLink2High); //Inserisco il link  nel menu
        //CaricaMenuSezioniContenuto("rif000002", rptTipologieLink8High); //Inserisco il link  nel menu
        //CaricaMenuSottoSezioniContenuto("rif000001", "prod000017", rptTipologieLink6High);
        //Carica i link menu per le pagine statiche in base all'id in tabella
        //CaricaMenuLinkContenuti(1);
        //CaricaMenuLinkContenuti(2);
        //CaricaMenuLinkContenuti(4);
        //CaricaMenuLinkContenuti(5);

    }
    private void CaricaBannersAndControls()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        string sezionebannersnavigazione = "";
        switch (CodiceTipologia)
        {

            default:
                sezionebannersnavigazione = "banner-portfolio-sezioni";
                sb.Clear();

                sb.Append("<section id=\"header3-2k\" class=\"mbr-section mbr-section__container article\" style=\"background-color: #ffffff; padding-top: 40px; padding-bottom: 20px;\">");
                sb.Append("  <div class=\"container\">");
                sb.Append("      <div class=\"row\">");
                sb.Append("          <div class=\"col-xs-12\">");
                sb.Append("             <h3 class=\"mbr-section-title display-2\">" + references.ResMan("basetext", Lingua, "testoCatalogonav1") + "</h3>");
                sb.Append("             <small class=\"mbr-section-subtitle\">" + references.ResMan("basetext", Lingua, "testoCatalogonav2") + "</small>");
                sb.Append("         </div>");
                sb.Append("     </div>");
                sb.Append(" </div>");
                sb.Append("</section>");
                sb.Append("<div id=\"divnavigazioneJs0\" class=\"inject\" params=\"");
                sb.Append("injectPortfolioAndLoadBanner,'IsotopeBanner4.html','divnavigazioneJs0', 'isoBan1', 1, 1, false, '','4','','TBL_BANNERS_GENERALE','" + sezionebannersnavigazione + "',false");
                sb.Append("\"></div>");
                plhNavigazione.Text = sb.ToString();

                break;
        }

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
                _tmp.HRef = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000");
            _tmp = ((HtmlAnchor)Page.Master.FindControl("linkid" + id.ToString() + "Lateral"));
            if (_tmp != null)
                _tmp.HRef = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000");

            _tmp = ((HtmlAnchor)Page.Master.FindControl("linkid" + id.ToString()));
            if (_tmp != null)
                _tmp.HRef = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), id.ToString(), "con001000");

        }
    }

    public string CreaLinkPersonalizzato(string testo, string tipologia, string qstring = "")
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        string link = CommonPage.CreaLinkRoutes(Session, false, Lingua, testo, "", tipologia, "");
        link += "?" + qstring;
        sb.Append("<a href=\"");
        sb.Append(link);
        sb.Append("\"");
        if (CodiceTipologia == tipologia && Vetrina != "")
            sb.Append(" style=\"font-weight:500 !important\"  ");
        sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
        sb.Append("<span >" + testo.ToLower() + "</span>");
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
            string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), idps.ToString(), "con001000");
            testo = references.ResMan("Common", Lingua, "testoid" + idps);
            if (!noli) sb.Append("<li>");
            sb.Append("<a  href=\"");
            sb.Append(link);
            sb.Append("\"");
            if (idps.ToString() == idContenuto)
                sb.Append(" class=\"" + classe + "\" style=\"" + stile + "\"  ");
            sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
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


    /// <summary>
    /// Creazione lista li per tipologie da min a max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public string CreaLinkTipologie(int min, int max)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua); });
        sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < min || Convert.ToInt32(t.Codice.Substring(3)) > max);
        sezioni.Sort(new GenericComparer<TipologiaOfferte>("Codice", System.ComponentModel.ListSortDirection.Descending));
        if (sezioni != null)
            foreach (TipologiaOfferte o in sezioni)
            {
                string testo = o.Descrizione;
                string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.Codice);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (o.Codice == CodiceTipologia)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
                sb.Append(testo);
                sb.Append("</a>");
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
                string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.Codice);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                sb.Append("<li>");
                sb.Append("<a  href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (o.Codice == CodiceTipologia)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
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
    public string CreaLinkCategorie(string tipologia, string filtercode = "")
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
        prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
        if (prodotti != null)
            foreach (Prodotto o in prodotti)
            {
                string testo = o.Descrizione;
                string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.CodiceTipologia, o.CodiceProdotto);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (o.CodiceProdotto == Categoria)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
                sb.Append(testo);
                sb.Append("</a>");
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
            prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
            foreach (Prodotto o in prodotti)
            {
                string testo = o.Descrizione;
                string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", o.CodiceTipologia, o.CodiceProdotto);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                sb.Append("<li>");
                sb.Append("<a  href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (o.CodiceProdotto == Categoria)
                    sb.Append(" style=\"font-weight:600 !important;display:block;\"  ");
                sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
                sb.Append(testo);
                sb.Append("</a>");

                /*Nested level*/
                string sottomenu = CreaLinkSottoCategorie(o.CodiceTipologia, o.CodiceProdotto);
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
        }

        return sb.ToString();
    }
    /// <summary>
    /// Creazione lista li delle sottocategorie ( 1livello ) per la tipologie e categoria indicata
    /// </summary>
    /// <param name="tipologia"></param>
    /// <param name="categoria"></param>
    /// <param name="filtercode"></param>
    /// <returns></returns>
    public string CreaLinkSottoCategorie(string tipologia, string categoria, string filtercode = "")
    {
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
        if (sprodotti != null)
            foreach (SProdotto o in sprodotti)
            {
                string testo = o.Descrizione;
                string link = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testo), "", tipologia, o.CodiceProdotto, o.CodiceSProdotto);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (o.CodiceSProdotto == Categoria2liv)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
                sb.Append(testo);
                sb.Append("</a>");
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

                string link = CommonPage.CreaLinkRoutes(Session, false, Lingua, testourl, "", tipologia, t.Key);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (tipologia == CodiceTipologia && t.Key == Categoria)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
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
                sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
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

    public string CaricaLinksPerTipologia(string tipologia, string Categoria = "", string maxlinks = "6", string ordinamento = "")
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

        OfferteCollection offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, maxlinks, Lingua, null, ordinamento);  //Creiamo i link
        if (offerte != null)
            foreach (Offerte o in offerte)
            {
                string testo = o.DenominazionebyLingua(Lingua);

                string link = CommonPage.CreaLinkRoutes(Session, false, Lingua, CommonPage.CleanUrl(testo), o.Id.ToString(), o.CodiceTipologia, o.CodiceCategoria);
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                sb.Append("<li>");
                sb.Append("<a href=\"");
                sb.Append(link);
                sb.Append("\"");
                if (o.Id.ToString() == idOfferta)
                    sb.Append(" style=\"font-weight:600 !important\"  ");
                sb.Append(" onclick=\"javascript:JsSvuotaSession(this)\"  >");
                sb.Append(testo);
                sb.Append("</a>");
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
    private void LoadJavascriptVariables()
    {
        ClientScriptManager cs = Page.ClientScript;

        String scriptRegVariables = string.Format("var GooglePosizione1 = '{0}'", references.ResMan("Common", Lingua, "GooglePosizione1"));
        scriptRegVariables += "; " + string.Format("var googleurl1 = '{0}'", references.ResMan("Common", Lingua, "GoogleUrl1"));
        scriptRegVariables += "; " + string.Format("var googlepin1 = '{0}'", references.ResMan("Common", Lingua, "GooglePin1"));
        scriptRegVariables += "; " + string.Format("var GooglePosizione2 = '{0}'", references.ResMan("Common", Lingua, "GoogleUrl2"));
        scriptRegVariables += "; " + string.Format("var googleurl2 = '{0}'", references.ResMan("Common", Lingua, "GoogleUrl2"));
        scriptRegVariables += "; " + string.Format("var googlepin2 = '{0}'", references.ResMan("Common", Lingua, "GooglePin2"));
        scriptRegVariables += "; " + string.Format("var idmapcontainer = 'map'");
        scriptRegVariables += "; " + string.Format("var idmapcontainer1 = 'map1'");
        scriptRegVariables += "; " + string.Format("var iddirectionpanelcontainer = 'directionpanel'");

        scriptRegVariables += "; " + string.Format("var idofferta = '" + idOfferta + "'");
        scriptRegVariables += "; " + string.Format("var tipologia = '" + CodiceTipologia + "'");
        scriptRegVariables += "; " + string.Format("var categoria = '" + Categoria + "'");
        scriptRegVariables += "; " + string.Format("var categoria2liv = '" + Categoria2liv + "'");

        if (!cs.IsClientScriptBlockRegistered("RegVariablesScript"))
        {
            cs.RegisterClientScriptBlock(typeof(Page), "RegVariablesScript", scriptRegVariables, true);
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
        //if (DateTime.TryParse(txtDataNascita.Text, out _d))
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

    protected void btnsearch1_Click(object sender, EventArgs e)
    {
        //testoricerca
        string link = CommonPage.CreaLinkRicerca("", "-", "", "", "", "", "", "-", Lingua, Session, true);
        Session.Add("testoricerca", Server.HtmlEncode(txtSearchTop.Value)); //carico in sessione il parametro da cercare
        Response.Redirect(link);
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        //testoricerca
        string link = CommonPage.CreaLinkRicerca("", "-", "", "", "", "", "", "-", Lingua, Session, true);
        Session.Add("testoricerca", Server.HtmlEncode(searchboxinputtext.Value)); //carico in sessione il parametro da cercare
        Response.Redirect(link);
    }
    protected void btnUsatoCerca_Click(object sender, EventArgs e)
    {
        Session["Caratteristica1"] = ddlCaratteristica1.SelectedValue;
        Session["Caratteristica2"] = ddlCaratteristica2.SelectedValue;
        Session["Caratteristica3"] = ddlCaratteristica3.SelectedValue;
        Session["Caratteristica4"] = ddlCaratteristica4.SelectedValue;
        Session["FasciaPrezzo"] = ddlFascePrezzo.SelectedValue;
        Session["Vetrina"] = chkPromo.Checked;
        Session["Ordinamento"] = ddlOrdinamento.SelectedValue;
        //  Response.Redirect(references.ResMan("Common",Lingua,"linkUsato);
    }
    public void CaricaDdlOrdinamento(string value = "")
    {
        //string tipi = references.ResMan("Common",Lingua,"listaServizi;
        Dictionary<string, string> dict = new Dictionary<string, string>();
        //string[] tipiarray = tipi.Split(',');
        //foreach (string s in tipiarray)
        //{
        //    dict.Add(s, s);
        //}
        dict.Add(references.ResMan("Common", Lingua, "FormOrdinamento"), "");
        dict.Add("Data Inserimento", "DataInserimento");
        dict.Add("Prezzo", "Prezzo");
        dict.Add("Data Immatricolazione", "Data1");

        ddlOrdinamento.Items.Clear();
        //ddlOrdinamento.AppendDataBoundItems = true;
        //ddlOrdinamento.Items.Insert(0, references.ResMan("Common",Lingua,"FormOrdinamento.ToString());
        //ddlOrdinamento.Items[0].Value = "";
        ddlOrdinamento.DataSource = dict;
        ddlOrdinamento.DataTextField = "Key";
        ddlOrdinamento.DataValueField = "Value";
        ddlOrdinamento.DataBind();
        try
        {

            ddlOrdinamento.SelectedValue = value;
        }
        catch
        { }



    }
    public void CaricaDatiDdlCaratteristiche(string Lingua = "I", string p1 = "0", string p2 = "0", string p3 = "0", string p4 = "0", string fasciaprezzo = "0", bool promozioni = false)
    {

        //Riempio la ddl 
        List<Tabrif> Car1 = Utility.Caratteristiche[0].FindAll(delegate (Tabrif _t) { return _t.Lingua == Lingua; });
        ddlCaratteristica1.Items.Clear();
        ddlCaratteristica1.Items.Insert(0, references.ResMan("Common", Lingua, "selCaratteristica1"));
        ddlCaratteristica1.Items[0].Value = "0";
        ddlCaratteristica1.DataSource = Car1;
        ddlCaratteristica1.DataTextField = "Campo1";
        ddlCaratteristica1.DataValueField = "Codice";
        ddlCaratteristica1.DataBind();
        try
        {
            ddlCaratteristica1.SelectedValue = p1.ToString();
        }
        catch { }


        //Riempio la ddl  ( collegandola alla caratteristica 1 )
        List<Tabrif> Car2 = Utility.Caratteristiche[1].FindAll(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Campo2 == p1.ToString(); });
        ddlCaratteristica2.Items.Clear();
        ddlCaratteristica2.Items.Insert(0, references.ResMan("Common", Lingua, "selCaratteristica2"));
        ddlCaratteristica2.Items[0].Value = "0";
        ddlCaratteristica2.DataSource = Car2;
        ddlCaratteristica2.DataTextField = "Campo1";
        ddlCaratteristica2.DataValueField = "Codice";
        ddlCaratteristica2.DataBind();
        try
        {
            ddlCaratteristica2.SelectedValue = p2.ToString();
        }
        catch { }


        List<Tabrif> Car3 = Utility.Caratteristiche[2].FindAll(delegate (Tabrif _t) { return _t.Lingua == Lingua; });
        ddlCaratteristica3.Items.Clear();
        ddlCaratteristica3.Items.Insert(0, references.ResMan("Common", Lingua, "selCaratteristica3"));
        ddlCaratteristica3.Items[0].Value = "0";
        ddlCaratteristica3.DataSource = Car3;
        ddlCaratteristica3.DataTextField = "Campo1";
        ddlCaratteristica3.DataValueField = "Codice";
        ddlCaratteristica3.DataBind();
        try
        {
            ddlCaratteristica3.SelectedValue = p3.ToString();
        }
        catch { }


        //Riempio la ddl  
        List<Tabrif> Car4 = Utility.Caratteristiche[3].FindAll(delegate (Tabrif _t) { return _t.Lingua == Lingua; });
        ddlCaratteristica4.Items.Clear();
        ddlCaratteristica4.Items.Insert(0, references.ResMan("Common", Lingua, "selCaratteristica4"));
        ddlCaratteristica4.Items[0].Value = "0";
        ddlCaratteristica4.DataSource = Car4;
        ddlCaratteristica4.DataTextField = "Campo1";
        ddlCaratteristica4.DataValueField = "Codice";
        ddlCaratteristica4.DataBind();
        try
        {
            ddlCaratteristica4.SelectedValue = p4.ToString();
        }
        catch { }


        List<Fascediprezzo> prezzi = Utility.Fascediprezzo.FindAll(fp => fp.Lingua == Lingua && fp.CodiceTipologiaCollegata == "rif000100");
        ddlFascePrezzo.Items.Clear();
        ddlFascePrezzo.Items.Insert(0, references.ResMan("Common", Lingua, "SelezionePrezzo"));
        ddlFascePrezzo.Items[0].Value = "0";
        ddlFascePrezzo.DataSource = prezzi;
        ddlFascePrezzo.DataTextField = "Descrizione";
        ddlFascePrezzo.DataValueField = "Codice";
        ddlFascePrezzo.DataBind();
        try
        {
            ddlFascePrezzo.SelectedValue = fasciaprezzo;
        }
        catch { }

        chkPromo.Checked = promozioni;
    }
    protected void ddlCaratteristica1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlCaratteristiche(Lingua, ((DropDownList)(sender)).SelectedValue);
    }

    #region FUNZIONI GESTIONE ECOMMERCE

    public void VisualizzaTotaliCarrello()
    {

        string sessionid = "";
        string trueIP = "";
        CommonPage.CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);

        eCommerceDM ecmDM = new eCommerceDM();
        CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP);
        double totalecarrello = CommonPage.CalcolaTotaleCarrello(Request, Session, carrello);
        //if (totalecarrello == 0) divCart.Visible = false;
        litTotalHigh.Text = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { totalecarrello }) + " €";
    }
    #endregion


#if false
    public void CaricaBannersPortfolio(string Tbl_sezione, int maxwidth, int maxheight, string filtrosezione, bool mescola, Literal destinationliteral, string Lingua)
    {
        bannersDM banDM = new bannersDM(Tbl_sezione);

        dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, filtrosezione, mescola);

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //CARICAMENTO DATI DB
                string pathimmagine = dt.Rows[i]["ImageUrl"].ToString();
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (string.IsNullOrEmpty(pathimmagine))
                    pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                string target = "_blank";
                string link = dt.Rows[i]["NavigateUrl"].ToString();
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1)
                {
                    target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //////////////////////////////////////
                //sb.Append("<div class=\"thumb-label-item animated seo\" ");
                //sb.Append("data-animtype=\"fadeInUp\" ");
                //sb.Append("data-animrepeat=\"0\" ");
                //sb.Append("data-speed=\"1s\" ");
                //sb.Append("data-delay=\"0.6s\" ");
                //sb.Append(" >\r\n");
                //////////////////////////////////////

                string testotitolo = dt.Rows[i]["AlternateText"].ToString();
                string titolo1 = testotitolo;
                string titolo2 = "<br/>";
                int j = testotitolo.IndexOf("\n");
                if (j != -1)
                {
                    titolo1 = testotitolo.Substring(0, j);
                    if (testotitolo.Length >= j + 1)
                        titolo2 = testotitolo.Substring(j + 1);
                }


                sb.Append("<div class=\"thumb-label-item\" ");
                sb.Append(" >\r\n");

                sb.Append(" <div class=\"img-overlay thumb-label-item-img\">\r\n");

                sb.Append("	 <a  onclick=\"javascript:JsSvuotaSession(this)\"  class=\"portfolio-zoom\" target=" + target + " href=\"" + link + "\" title=\"" + titolo1 + "\" >");
                sb.Append("<img  src=\"" + pathimmagine + "\" alt=\"\" />\r\n");
                sb.Append("<div class=\"item-img-overlay\">\r\n");
                //IN alternativa aperutra galleria pretty photo    <a class="portfolio-zoom fa fa-plus" href="images/placeholders/portfolio1.jpg"  data-rel="prettyPhoto[portfolio]" title="Title goes here"></a>
                if (titolo1 != "")
                {

                    sb.Append("	 <div class=\"item_img_overlay_content\">\r\n");
                    sb.Append("  <h3 class=\"thumb-label-item-title\">\r\n");
                    sb.Append("	    <a  onclick=\"javascript:JsSvuotaSession(this)\"  target=" + target + " href=\"" + link + "\" title=\"" + titolo1 + "\"  >" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</a>\r\n");
                    sb.Append("  </h3>\r\n");
                    sb.Append("	 </div>\r\n ");

                }
                sb.Append("</div>\r\n");
                sb.Append("</a>\r\n");
                sb.Append("</div>\r\n");

                sb.Append("<div style=\"padding-top:0px;padding-bottom:20px;height:70px;text-align:center\" >\r\n");

                sb.Append("<h3 style=\"margin-bottom:0px\" class=\"h3-body-title-1\">\r\n");
                sb.Append("<a  onclick=\"javascript:JsSvuotaSession(this)\"  title=\"\"  target=\"" + target + "\" href=\"" + link + "\">");
                sb.Append("" + titolo1 + "");
                sb.Append("</a>\r\n");
                sb.Append("</h3>\r\n");
                sb.Append("<a  onclick=\"javascript:JsSvuotaSession(this)\"  title=\"\"  target=\"" + target + "\" href=\"" + link + "\">");
                sb.Append("<p>" + titolo2 + "</p>\r\n");
                sb.Append("</a>\r\n");
                sb.Append("</div>\r\n");


                sb.Append("</div>\r\n");
            }
            destinationliteral.Text = sb.ToString();
            destinationliteral.Parent.Visible = true;

        }
        else
            destinationliteral.Parent.Visible = false;
    }

#endif
#if false
    /// <summary>
    /// Carica dal db la tipologia indicata e la formatta per il portfolio
    /// </summary>
    /// <param name="tipologiadacaricare"></param>
    /// <param name="destinationliteral"></param>
    /// <param name="Lingua"></param>
    /// <param name="numerodacaricare"></param>
    public void CaricaContenutiPortfolio(string tipologiadacaricare, Literal destinationliteral, string Lingua, string numerodacaricare = "6", List<Offerte> passedlist = null)
    {
        offerteDM offDM = new offerteDM();

        List<Offerte> offerte = new List<Offerte>();
        if (passedlist != null)
            offerte = passedlist;
        else
            offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, numerodacaricare, false, Lingua, false);

        if ((offerte != null) && (offerte.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Offerte _o in offerte)
            {

                string testotitolo = _o.DenominazionebyLingua(Lingua);
                string descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DescrizionebyLingua(Lingua), 100, true));

                string pathimmagine = ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString());
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (string.IsNullOrEmpty(pathimmagine))
                    pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);


                string target = "_self";
                string link = CommonPage.CreaLinkRoutes(null, false, Lingua, CommonPage.CleanUrl(testotitolo), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria);

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


                //////////////////////////////////////

                //sb.Append("<div class=\"thumb-label-item animated seo\" ");
                //sb.Append("data-animtype=\"fadeInUp\" ");
                //sb.Append("data-animrepeat=\"0\" ");
                //sb.Append("data-speed=\"1s\" ");
                //sb.Append("data-delay=\"0.6s\" ");
                //sb.Append(" >\r\n");


                sb.Append("<div class=\"thumb-label-item\" style=\"background-color:white\" ");
                sb.Append(" >\r\n");

                sb.Append(" <div class=\"img-overlay thumb-label-item-img\">\r\n");

                sb.Append("	 <a   class=\"portfolio-zoom\" target=" + target + " href=\"" + link + "\" title=\"" + titolo1 + "\" >");
                sb.Append("<img  src=\"" + pathimmagine + "\" alt=\"\" />\r\n");
                sb.Append("<div class=\"item-img-overlay\">\r\n");
                //IN alternativa aperutra galleria pretty photo    <a class="portfolio-zoom fa fa-plus" href="images/placeholders/portfolio1.jpg"  data-rel="prettyPhoto[portfolio]" title="Title goes here"></a>
                if (titolo1.ToString() != "")
                {

                    sb.Append("	 <div class=\"item_img_overlay_content\">\r\n");
                    sb.Append("  <h3 class=\"thumb-label-item-title\">\r\n");
                    sb.Append("	    <a target=" + target + " href=\"" + link + "\" title=\"" + titolo1 + "\"  >" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</a>\r\n");
                    sb.Append("  </h3>\r\n");
                    sb.Append("	 </div>\r\n ");

                }
                sb.Append("</div>\r\n");
                sb.Append("</a>\r\n");
                sb.Append("</div>\r\n");

                sb.Append("<div style=\"padding-top:0px;padding-bottom:20px;height:70px;text-align:center\" >\r\n");

                sb.Append("<h3 style=\"margin-bottom:0px\" class=\"h3-body-title-1\">\r\n");
                sb.Append("<a    title=\"\"  target=\"" + target + "\" href=\"" + link + "\">");
                sb.Append("" + titolo1 + "");
                sb.Append("</a>\r\n");
                sb.Append("</h3>\r\n");
                sb.Append("<a title=\"\"  target=\"" + target + "\" href=\"" + link + "\">");
                sb.Append("<p style=\"font-size:1.1em\">" + titolo2 + "</p>\r\n");
                sb.Append("</a>\r\n");
                if (_o.Prezzo != 0)
                {
                    string prezzolistino = "";
                    if (_o.PrezzoListino != 0)
                        prezzolistino = "<span style=\"text-decoration: line-through;font-size:0.9em;color:#aaa;padding-left:10px;padding-right:10px\">" + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.PrezzoListino) + "</span>";
                    sb.Append("<div style=\"font-weight:500;font-size:1.2em\">" + references.ResMan("Common",Lingua,"TitoloPrezzo + " "
                        + prezzolistino
                        + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.Prezzo) + "</div>");
                    sb.Append("<br/>\r\n");

                }
                sb.Append("</div>\r\n");

                sb.Append("</div>\r\n");
            }
            destinationliteral.Text = sb.ToString();
            destinationliteral.Parent.Visible = true;

        }
        else
            destinationliteral.Parent.Visible = false;
    }

#endif
#if false
    public void CaricaBannerHomegallery(string Tbl_sezione, int maxwidth, int maxheight, string filtrosezione, bool mescola, string Lingua)
    {
        //http://www.orbis-ingenieria.com/code/documentation/documentation.html#!/install
        bannersDM banDM = new bannersDM(Tbl_sezione);
        WelcomeLibrary.HtmlToText cv = new WelcomeLibrary.HtmlToText();
        dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, filtrosezione, mescola);

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

#if true
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pathimmagine = dt.Rows[i]["ImageUrl"].ToString();
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                string target = "_blank";

                string link = dt.Rows[i]["NavigateUrl"].ToString();
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && !string.IsNullOrEmpty(link))
                {
                    target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }

                sb.Append("<li data-transition=\"fade\" data-slotamount=\"7\" data-masterspeed=\"300\" ");
                if (!string.IsNullOrEmpty(link))
                    sb.Append("data-link=\"" + link + "\"");
                sb.Append("> \r\n");

                string dummyimage = "~/images/dummy.png".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //  sb.Append("	                <a target=" + target + " href=\"" + link + "\" title=\"" + dt.Rows[i]["AlternateText"].ToString() + "\" >");
                sb.Append("<img data-bgfit=\"contain\" data-bgposition=\"center center\"  src=\"" + dummyimage + "\" data-lazyload=\"" + pathimmagine + "\"   alt=\"" + cv.Convert(dt.Rows[i]["AlternateText"].ToString()).Trim() + "\" ");
                sb.Append("    />");
                //alt=\"rev-full1\" data-fullwidthcentering=\"on\"
                //  sb.Append("</a>");



                if (dt.Rows[i]["AlternateText"].ToString() != "")
                {
                    string testoover = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(dt.Rows[i]["AlternateText"].ToString());

                    //sb.Append("<div class=\"tp-caption slider-text-description\" ");
                    sb.Append("<div class=\"tp-caption slider-text-description\" ");
                    sb.Append(" data-x=\"center\" ");
                    sb.Append(" data-y=\"center\"  ");
                    sb.Append(" data-hoffset=\"0\"  ");
                    sb.Append(" data-voffset=\"0\"  ");

                    //sb.Append(" data-x=\"150\" ");
                    //sb.Append(" data-y=\"50\"  ");
                    sb.Append("  data-speed=\"1600\"  ");
                    sb.Append(" data-start=\"1500\"  ");
                    sb.Append(" data-easing=\"Power4.easeOut\"  ");
                    sb.Append(" data-endspeed=\"300\"  ");
                    sb.Append(" data-endeasing=\"Power1.easeIn\"  >");
                    sb.Append(CommonPage.ReplaceLinks(testoover));
                    //sb.Append(" <a target=" + target + " href=\"" + link + "\" title=\"" + dt.Rows[i]["AlternateText"].ToString() + "\"  class=\"button btn-flat\">"
                    //         + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(dt.Rows[i]["Alternatetext"].ToString()) + "</a>");
                    sb.Append("</div> ");
                }
                sb.Append("</li>");
            }
#endif
            homeSlides.Text = sb.ToString();
            homegallery.Visible = true;
        }
        else
        {
            homegallery.Visible = false;
            VerticalSpacer.Style.Add(HtmlTextWriterStyle.Height, "135px"); //Senza testata -> spazio verticale per il menu
        }

    } 
#endif
#if false

    public void CaricaBannersFascia(string Tbl_sezione, int maxwidth, int maxheight, string filtrosezione, bool mescola, Literal destinationliteral, string Lingua)
    {

        bannersDM banDM = new bannersDM(Tbl_sezione);
        dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, filtrosezione, mescola);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<div class=\"cycle-slideshow\" data-cycle-timeout=\"3000\" data-cycle-speed=\"2000\" data-cycle-pause-on-hover=\"true\"  data-cycle-slides=\"> div\" style=\"height:auto\"  >");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //CARICAMENTO DATI DB
                string pathimmagine = dt.Rows[i]["ImageUrl"].ToString();
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                string target = "_blank";
                string link = dt.Rows[i]["NavigateUrl"].ToString();
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1)
                {
                    target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }

                //sb.Append("<a onclick=\"javascript:JsSvuotaSession(this)\" target=" + target + " href=\"" + link + "\" data-cycle-title=\"\" data-cycle-desc=\"" + dt.Rows[i]["AlternateText"].ToString() + "\" >");
                //sb.Append("<img style=\"width: 100%; height: auto\"  src=\"" + pathimmagine + "\" alt=\"\" />\r\n");
                //sb.Append("</a>\r\n");
#if false
                sb.Append("<div class=\"section-content section-px\" style=\"background-image: url('" + pathimmagine + "');\">");
                sb.Append("               <div class=\"container\">");
                sb.Append("                   <div class=\"row\" style=\"padding-top: 5%; padding-bottom: 10%; padding-left: 10%; padding-right: 10%;\">");
                sb.Append("                       <div class=\" col-md-6 col-sm-6 col-lg-8 col-xs-12\">");
                sb.Append("                           <div class=\"testimonial-big\">");
                sb.Append("                               <div class=\"testimonial-big-text animated fadeInRight animatedVisi\" data-speed=\"1s\" data-animrepeat=\"1\" data-animtype=\"fadeInRight\" data-delay=\"0s\">");
                sb.Append("                                   <br />");
                sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(CommonPage.ReplaceLinks(dt.Rows[i]["AlternateText"].ToString())));
                sb.Append("                                   <br />");
                sb.Append("                               </div>");
                sb.Append("                           </div>");
                sb.Append("                       </div>");
                sb.Append("                   </div>");
                sb.Append("               </div>");
                sb.Append("           </div>"); 
#endif

                sb.Append("          <div class=\"section-content section-px\"   style=\"background-image: url('" + pathimmagine + "');\">");
                sb.Append("               <div class=\"container\">");
                sb.Append("                   <div class=\"row\" style=\"padding-top: 10%; padding-bottom: 10%; padding-left: 2%; padding-right: 2%;\">");
                sb.Append("                       <div class=\" col-sm-offset-1 col-sm-10 col-xs-12\">");

                sb.Append("                           <div class=\"testimonial-big\">");
                sb.Append("                               <div class=\"testimonial-big-text\">");
                sb.Append("                                   <br/>");
                sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(CommonPage.ReplaceLinks(dt.Rows[i]["AlternateText"].ToString())));
                sb.Append("                                   <br/>");
                sb.Append("                               </div>");
                sb.Append("                           </div>");
                sb.Append("                       </div>");
                sb.Append("                   </div>");
                sb.Append("               </div>");
                sb.Append("           </div>");

            }
            sb.Append("           </div>");

            destinationliteral.Text = sb.ToString();
        }
    }
    public void CaricaBannersStriscia(string Tbl_sezione, int maxwidth, int maxheight, string filtrosezione, bool mescola, Literal destinationliteral, string Lingua)
    {

        //<div class="cycle-slideshow" style="width: 100%; clear: left; overflow: hidden" data-cycle-timeout="3000" data-cycle-speed="2000" data-cycle-pause-on-hover="true" data-cycle-slides="> a">
        //   <a href="yourPage1.html"  data-cycle-desc="Wood Lake Nature Preserve"><img style="width: 100%; height: auto" src="public/banners/marketaccess.jpg" ></a>
        //    <a href="yourPage2.html"  data-cycle-desc="Sky Lake Nature Preserve"><img style="width: 100%; height: auto" src="public/banners/market-access-1.jpg" ></a>
        //    <a href="yourPage3.html"  data-cycle-desc="Earth Lake Nature Preserve"><img style="width: 100%; height: auto" src="public/banners/rete-commerciale.jpg" ></a>
        //    <div class="cycle-overlay"></div>
        //</div>
        bannersDM banDM = new bannersDM(Tbl_sezione);

        dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, filtrosezione, mescola);

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(" <div class=\"cycle-slideshow\" style=\"margin-top:10px;margin-bottom:10px;");
            if (maxwidth == 0)
                sb.Append("width: 100%;");
            else
                sb.Append("width: " + maxwidth.ToString() + "px;");
            if (maxheight == 0)
                sb.Append("height:auto;");
            else
                sb.Append("height:" + maxheight.ToString() + "px;");
            sb.Append("overflow: hidden\" data-cycle-timeout=\"3000\" data-cycle-speed=\"2000\" data-cycle-pause-on-hover=\"true\" data-cycle-slides=\"> a\"> ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //CARICAMENTO DATI DB
                string pathimmagine = dt.Rows[i]["ImageUrl"].ToString();
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                string target = "_blank";
                string link = dt.Rows[i]["NavigateUrl"].ToString();
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1)
                {
                    target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }
                //////////////////////////////////////
                sb.Append("	 <a onclick=\"javascript:JsSvuotaSession(this)\" target=" + target + " href=\"" + link + "\" data-cycle-desc=\"" + dt.Rows[i]["AlternateText"].ToString() + "\" >");
                sb.Append("<img style=\"width: 100%; height: auto\"  src=\"" + pathimmagine + "\" alt=\"\" />\r\n");
                sb.Append("</a>\r\n");

                //sb.Append("<div class=\"cycle-overlay\">\r\n");
                //sb.Append("</div>\r\n");



            }
            sb.Append("</div>\r\n");

            destinationliteral.Text = sb.ToString();
            // destinationliteral.Parent.Visible = true;

        }
        //else
        //    destinationliteral.Parent.Visible = false;
    }



      public void CaricaContenutiPortfolioRival(string tipologiadacaricare, Literal destinationliteral, string Lingua, string numerodacaricare = "6", List<Offerte> passedlist = null)
    {

        offerteDM offDM = new offerteDM();

        List<Offerte> offerte = new List<Offerte>();
        if (passedlist != null)
            offerte = passedlist;
        else
            offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, numerodacaricare, true, Lingua, false);

        if ((offerte != null) && (offerte.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Offerte _o in offerte)
            {

                string testotitolo = _o.DenominazionebyLingua(Lingua);
                string descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DenominazionebyLingua(Lingua), 100, true));

                string pathimmagine = CommonPage.ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString());
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (string.IsNullOrEmpty(pathimmagine))
                    pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                string target = "_self";
                string link = CommonPage.CreaLinkRoutes(null, false, Lingua, CommonPage.CleanUrl(testotitolo), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria);

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

                sb.Append("<li class=\"work-item\">\r\n");
                sb.Append("<a    target=" + target + " href=\"" + link + "\" title=\"" + titolo1 + "\" >\r\n");
                sb.Append("  <div class=\"work-image\">\r\n");
                sb.Append("<img  src=\"" + pathimmagine + "\" alt=\"\" />\r\n");
                sb.Append("    </div>\r\n");
                sb.Append("   <div class=\"work-caption font-alt\">\r\n");
                sb.Append("       <h3 class=\"work-title\">" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</h3>\r\n");
                sb.Append("       <div class=\"work-descr\">\r\n");
                sb.Append(titolo2);
                if (_o.Prezzo != 0)
                {
                    sb.Append("<div style=\"font-weight:500;font-size:1.2em\">" + references.ResMan("Common", Lingua, "TitoloPrezzo") + ": " + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.Prezzo) + "</div>");
                }
                sb.Append("         </div>\r\n");
                sb.Append("     </div>\r\n");
                //sb.Append("   <div class=\"work-subcaption\">\r\n");
                //sb.Append("       <h3 class=\"work-title\">PROVA TITOLO SOTTO</h3>\r\n");
                //sb.Append("     </div>\r\n");
                sb.Append(" </a>\r\n");
                sb.Append("   </li>\r\n");


            }
            destinationliteral.Text = sb.ToString();
            destinationliteral.Parent.Visible = true;

        }
        else
            destinationliteral.Parent.Visible = false;

    }
    public void CaricaContenutiPortfolioRivalBordered(string tipologiadacaricare, Literal destinationliteral, string Lingua, string numerodacaricare = "6", List<Offerte> passedlist = null)
    {

        offerteDM offDM = new offerteDM();

        List<Offerte> offerte = new List<Offerte>();
        if (passedlist != null)
            offerte = passedlist;
        else
            offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, numerodacaricare, true, Lingua, false);

        if ((offerte != null) && (offerte.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Offerte _o in offerte)
            {

                string testotitolo = _o.DenominazionebyLingua(Lingua);
                string descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DescrizionebyLingua(Lingua), 100, true));


                string pathimmagine = CommonPage.ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString());
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (string.IsNullOrEmpty(pathimmagine))
                    pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                string target = "_self";
                string link = CommonPage.CreaLinkRoutes(null, false, Lingua, CommonPage.CleanUrl(testotitolo), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria);

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

                sb.Append("<li class=\"work-item\">\r\n");
                sb.Append("  <div class=\"work-div\">\r\n");

                if (_o.CodiceCategoria != "")
                {
                    sb.Append("  <div class=\"work-flag-" + _o.CodiceCategoria + "\" ></div>\r\n");
                }

                sb.Append("<a     target=" + target + " href=\"" + link + "\" title=\"" + titolo1 + "\" >\r\n");
                sb.Append("  <div class=\"work-image\">\r\n");
                sb.Append("<img  src=\"" + pathimmagine + "\" alt=\"\" />\r\n");
                sb.Append("    </div>\r\n");
                //sb.Append("   <div class=\"work-caption font-alt\">\r\n");
                //sb.Append("       <h3 class=\"work-title\">" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</h3>\r\n");
                //sb.Append("       <div class=\"work-descr\">\r\n");
                //sb.Append(titolo2);
                //if (_o.Prezzo != 0)
                //{
                //    sb.Append("<div style=\"font-weight:500;font-size:1.2em\">" + references.ResMan("Common",Lingua,"TitoloPrezzo + ": " + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:0.00}", _o.Prezzo) + "</div>");
                //}
                //sb.Append("         </div>\r\n");
                //sb.Append("     </div>\r\n");
                sb.Append("   <div class=\"work-subcaption\">\r\n");
                sb.Append("       <h3 class=\"work-title\"  style=\"font-size:1em;font-weight:600;margin-bottom:0px;color:#4a4b4c\">" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</h3>\r\n");
                sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo2) + "\r\n");
                if (_o.Prezzo != 0)
                {
                    sb.Append("<br/>\r\n");
                    sb.Append("<br/>\r\n");
                    string prezzolistino = "";

                    sb.Append("<div style=\"font-weight:500;font-size:1.4em\">" + references.ResMan("Common", Lingua, "TitoloPrezzo") + "<br/> "
                        + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.Prezzo) + "</div>");
                    if (_o.PrezzoListino != 0)
                        prezzolistino = "<span style=\"text-decoration: line-through;font-size:0.9em;color:#aaa;padding-rigth:10px\">" + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.PrezzoListino) + "</span>" + "<br/> ";
                    sb.Append(prezzolistino);

                    sb.Append("<br/>\r\n");
                }
                else //Creo uno spazio prezzo 
                {
                    sb.Append("<br/>\r\n");
                    sb.Append("<br/>\r\n");

                }
                sb.Append("     </div>\r\n");
                sb.Append(" </a>\r\n");


                sb.Append(" </div>\r\n");
                sb.Append("   </li>\r\n");


            }
            destinationliteral.Text = sb.ToString();
            destinationliteral.Parent.Visible = true;

        }
        else
            destinationliteral.Parent.Visible = false;

    }
    public void CaricaContenutiPortfolioRivalSubtext(string tipologiadacaricare, Literal destinationliteral, string Lingua, string numerodacaricare = "6", List<Offerte> passedlist = null, string versione = "", string Color = "1f809f")
    {

        offerteDM offDM = new offerteDM();

        List<Offerte> offerte = new List<Offerte>();
        if (passedlist != null)
            offerte = passedlist;
        else
            offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, numerodacaricare, true, Lingua, false);

        if ((offerte != null) && (offerte.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Offerte _o in offerte)
            {

                string testotitolo = _o.DenominazionebyLingua(Lingua);
                string descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DescrizionebyLingua(Lingua), 100, true));


                string pathimmagine = CommonPage.ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString());
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (string.IsNullOrEmpty(pathimmagine))
                    pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                string target = "_self";
                string link = CommonPage.CreaLinkRoutes(null, false, Lingua, CommonPage.CleanUrl(testotitolo), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria);

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

                sb.Append("<li class=\"work-item\">\r\n");
                if (string.IsNullOrEmpty(versione))
                    sb.Append("<div  class=\"work-div-noborder\" style=\"text-align:center\">");
                sb.Append("       <h3 class=\"work-title\" style=\"font-size:1em;font-weight:600;margin-bottom:0px;color:#4a4b4c\">" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</h3>\r\n");
                sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo2) + "\r\n");

                sb.Append("<a     target=" + target + " href=\"" + link + "\" title=\"" + titolo1 + "\" >\r\n");
                sb.Append("  <div class=\"work-image\" style=\"background-color:white\">\r\n");
                sb.Append("<img  src=\"" + pathimmagine + "\" alt=\"\" />\r\n");
                sb.Append("    </div>\r\n");
                //sb.Append("   <div class=\"work-caption font-alt\">\r\n");
                //sb.Append("       <h3 class=\"work-title\">" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</h3>\r\n");
                //sb.Append("       <div class=\"work-descr\">\r\n");
                //sb.Append(titolo2);
                //if (_o.Prezzo != 0)
                //{
                //    sb.Append("<div style=\"font-weight:500;font-size:1.2em\">" + references.ResMan("Common",Lingua,"TitoloPrezzo + ": " + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:0.00}", _o.Prezzo) + "</div>");
                //}
                //sb.Append("         </div>\r\n");
                //sb.Append("     </div>\r\n");
                sb.Append("   <div class=\"work-subcaption\" style=\"padding-top:12px;text-align:center;min-height:110px\">\r\n");


                sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(descrizione) + "\r\n");


                //else //Creo uno spazio prezzo 
                //{
                //    sb.Append("<br/>\r\n");
                //    sb.Append("<br/>\r\n");

                //}

                //if (_o.Indirizzo != "")
                //{
                //    sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(_o.Indirizzo));
                //    sb.Append("<br/>\r\n");
                //}
                //if (_o.Telefono != "")
                //{
                //    sb.Append(_o.Telefono);
                //    sb.Append("<br/>\r\n");
                //}
                //if (_o.Fax != "")
                //{
                //    sb.Append(_o.Fax);
                //    sb.Append("<br/>\r\n");
                //}
                //if (_o.Email != "")
                //{
                //    sb.Append(_o.Email);
                //    sb.Append("<br/>\r\n");
                //}
                //if (_o.Website != "")
                //{
                //    sb.Append(_o.Website);
                //    sb.Append("<br/>\r\n");
                //}
                //sb.Append("<br/>\r\n");
                //sb.Append("   <div  style=\"border-top:1px solid #ccc;margin-top:0px\"></div>\r\n");

                //sb.Append("<div class=\"pull-right\" style=\"font-weight:500\">" + references.ResMan("Common",Lingua,"TestoTooltipElenco + "</div>\r\n");

                //sb.Append("<br/>\r\n");
                sb.Append("     </div>\r\n");


                if (_o.Prezzo != 0)
                {
                    sb.Append("<div style=\"font-weight:500;float:left\">");
                    string prezzolistino = "";
                    prezzolistino = references.ResMan("Common", Lingua, "TitoloPrezzo") + " ";
                    sb.Append("<div style=\"font-weight:500;font-size:1.2em;padding-right:10px;color:#" + Color + "\">" + prezzolistino
                        + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.Prezzo));
                    //Testo intro prezzo
                    if (_o.CodiceTipologia == "rif000001")
                        sb.Append(" " + references.ResMan("Common", Lingua, "TitoloPrezzounita"));

                    if (_o.PrezzoListino != 0)
                        sb.Append("<br/>" + "<span style=\"text-decoration: line-through;font-size:0.9em;color:#aaa;padding-left:10px;padding-right:10px\">" + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.PrezzoListino) + "</span>");

                    sb.Append("</div>");


                    sb.Append("</div>\r\n");

                }
                sb.Append(" </a>\r\n");

                if (string.IsNullOrEmpty(versione))
                    sb.Append("</div>\r\n");
                sb.Append("   </li>\r\n");
            }
            destinationliteral.Text = sb.ToString();
            destinationliteral.Parent.Visible = true;

        }
        else
            destinationliteral.Parent.Visible = false;

    }
    public void CaricaBannersPortfolioRival(string Tbl_sezione, int maxwidth, int maxheight, string filtrosezione, bool mescola, Literal destinationliteral, string Lingua, bool overtext = false, int maxelements = 0, int margin = 0)
    {

        bannersDM banDM = new bannersDM(Tbl_sezione);

        dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, filtrosezione, mescola, maxelements);

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //CARICAMENTO DATI DB
                string pathimmagine = dt.Rows[i]["ImageUrl"].ToString();
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (string.IsNullOrEmpty(pathimmagine))
                    pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                string target = "_blank";
                string link = dt.Rows[i]["NavigateUrl"].ToString();
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1)
                {
                    target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //////////////////////////////////////
                //sb.Append("<div class=\"thumb-label-item animated seo\" ");
                //sb.Append("data-animtype=\"fadeInUp\" ");
                //sb.Append("data-animrepeat=\"0\" ");
                //sb.Append("data-speed=\"1s\" ");
                //sb.Append("data-delay=\"0.6s\" ");
                //sb.Append(" >\r\n");
                //////////////////////////////////////

                string testotitolo = dt.Rows[i]["AlternateText"].ToString();
                string titolo1 = testotitolo;
                string titolo2 = "<br/>";
                int j = testotitolo.IndexOf("\n");
                if (j != -1)
                {
                    titolo1 = testotitolo.Substring(0, j);
                    if (testotitolo.Length >= j + 1)
                        titolo2 = testotitolo.Substring(j + 1);
                }

                sb.Append("<li class=\"work-item\">\r\n");

                sb.Append("<a   onclick=\"javascript:JsSvuotaSession(this)\"   target=" + target + " href=\"" + link + "\" title=\"" + titolo1 + "\" >\r\n");
                sb.Append("<div class=\"work-div-noborder\" style=\"margin:" + margin + "px\">\r\n");

                sb.Append("  <div class=\"work-image\">\r\n");
                sb.Append("<img  src=\"" + pathimmagine + "\" alt=\"\" />\r\n");
                sb.Append("    </div>\r\n");

                if (overtext)
                {
                    if (!string.IsNullOrEmpty(titolo1))
                    {
                        sb.Append("     <div class=\"work-caption-over\">\r\n");
                        sb.Append("       <h3>" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</h3>\r\n");
                        if (titolo2 != "<br/>") sb.Append(titolo2);
                        sb.Append("     </div>\r\n");
                    }
                }
                else
                {
                    sb.Append("   <div class=\"work-caption font-alt\">\r\n");
                    sb.Append("       <h3 class=\"work-title\">" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(titolo1) + "</h3>\r\n");
                    sb.Append("       <div class=\"work-descr\">\r\n");
                    sb.Append(titolo2);
                    sb.Append("         </div>\r\n");
                    sb.Append("     </div>\r\n");

                }
                sb.Append("     </div>\r\n");
                sb.Append(" </a>\r\n");


                sb.Append("   </li>\r\n");

            }
            destinationliteral.Text = sb.ToString();
            destinationliteral.Parent.Visible = true;

        }
        else
            destinationliteral.Parent.Visible = false;
    }
    public void CaricaUltimiPostScrollerTipo1(Literal destinationliteral, Literal titleliteral, string tipologiadacaricare, string lingua, bool visualizzadata = true, bool visualizzaprezzo = false, OfferteCollection listtoshow = null, string maxtoshow = "6", string Color = "1f809f")
    {
        offerteDM offDM = new offerteDM();
        OfferteCollection offerte = new OfferteCollection();
        if (listtoshow == null)
            offerte = (OfferteCollection)offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, maxtoshow, false, lingua, false);
        else
            offerte = listtoshow;

        if ((offerte != null) && (offerte.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Offerte _o in offerte)
            {
                string testotitolo = _o.DenominazionebyLingua(Lingua);
                string descrizione = CommonPage.ReplaceLinks(CommonPage.ConteggioCaratteri(_o.DescrizionebyLingua(Lingua), 100, true));

                string pathimmagine = CommonPage.ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString());
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

                sb.Append("<div class=\"item\" style=\"padding:8px;margin:14px\" >\r\n");
                //ITEM CONTENT PESONALIZZATO

                //sb.Append("<div style=\"border:none;box-shadow:none\" class=\"feature animated\" data-animtype=\"fadeInUp\" data-animrepeat=\"0\" data-animspeed=\"1s\" data-animdelay=\"0.4s\">\r\n");
                sb.Append("<div style=\"border:none;box-shadow:none\" class=\"feature\" >\r\n");
                sb.Append("<a onclick=\"javascript:JsSvuotaSession(this)\"  class=\"portfolio-zoom\" title=\"\"  target=\"" + target + "\" href=\"" + link + "\">");
                sb.Append("<div class=\"feature-image img-overlay\" style=\"max-height:220px\">\r\n");
                //if (ControlloVisibilita(_o.FotoCollection_M))
                //{
                sb.Append("<img src=\"" + pathimmagine + "\" alt=\"\">\r\n");
                //}
                sb.Append("<div class=\"item-img-overlay\">\r\n");
                sb.Append("</div>\r\n");
                sb.Append("</div>\r\n");
                sb.Append("</a>\r\n");

                sb.Append("<div style=\"padding-top:10px;padding-bottom:0px;border-top:none;height:100px;text-align:center\" class=\"feature-content\">\r\n");
                sb.Append("<h3 style=\"margin-bottom:0px\" class=\"h3-body-title-1\">\r\n");
                sb.Append("<a onclick=\"javascript:JsSvuotaSession(this)\"  title=\"\"  target=\"" + target + "\" href=\"" + link + "\">");
                sb.Append("" + titolo1 + "");
                sb.Append("</a>\r\n");

                sb.Append("</h3>\r\n");
                sb.Append("<a onclick=\"javascript:JsSvuotaSession(this)\"  title=\"\"  target=\"" + target + "\" href=\"" + link + "\">");

                sb.Append("<p>" + titolo2 + "</p>\r\n");
                sb.Append("</a>\r\n");

                //sb.Append("<p>");
                //sb.Append(descrizione);
                //sb.Append("</p>\r\n");

                sb.Append("</div>\r\n");
#if true
                if (visualizzadata)
                {
                    sb.Append("<div class=\"clearfix\"></div> ");

                    sb.Append("<div class=\"feature-details\">\r\n");
                    //sb.Append("<i class=\"icon-calendar\"></i>");
                    sb.Append("<div class=\"pull-left\"> ");

                    sb.Append(string.Format("{0:dd/MM/yyyy}", _o.DataInserimento));//+ TestoSezione(_o.CodiceTipologia));
                    sb.Append("</div>\r\n");
                    sb.Append("<div class=\"feature-share\">\r\n");

                    sb.Append("<a onclick=\"javascript:JsSvuotaSession(this)\"  title=\"\"  target=\"" + target + "\" href=\"" + link + "\">");
                    sb.Append(references.ResMan("Common", Lingua, "testoContinua"));
                    sb.Append("</a>\r\n");


                    //<div class="feature-share">
                    //    <a href="#"><i class="icon-facebook"></i></a>
                    //</div>
                    sb.Append("</div>\r\n");
                    sb.Append("<div class=\"clearfix\"></div> ");

                    sb.Append("</div>\r\n");
                }

                if (visualizzaprezzo)
                {

                    if (_o.Prezzo != 0)
                    {
                        //sb.Append("<div class=\"feature-details\">\r\n");
                        sb.Append("<div style=\"font-weight:500\">");
                        string prezzolistino = "";
                        prezzolistino = references.ResMan("Common", Lingua, "TitoloPrezzo") + " ";
                        sb.Append("<div style=\"font-weight:500;font-size:1.4em;padding-right:10px;color:#" + Color + "\">" + prezzolistino
                            + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.Prezzo));
                        //Testo intro prezzo
                        if (_o.CodiceTipologia == "rif000001")
                            sb.Append(" " + references.ResMan("Common", Lingua, "TitoloPrezzounita"));

                        if (_o.PrezzoListino != 0)
                            sb.Append("<br/>" + "<span style=\"text-decoration: line-through;font-size:0.9em;color:#aaa;padding-left:10px;padding-right:10px\">" + "<i class=\"fa fa-eur\"></i> " + string.Format("{0:##,###.00}", _o.PrezzoListino) + "</span>");

                        sb.Append("</div>");
                        sb.Append("</div>\r\n");
                        //sb.Append("</div>\r\n");

                    }
                    else //Creo uno spazio prezzo 
                    {
                        sb.Append("<br/>\r\n");
                        sb.Append("<br/>\r\n");
                    }
                }

#endif
                sb.Append("</div>\r\n");

                //ITEM CONTENT PESONALIZZATO
                sb.Append(" </div>\r\n");


            }

            //titleliteral.Text = TestoSezione(tipologiadacaricare, true);


            destinationliteral.Text = sb.ToString();
            destinationliteral.Parent.Visible = true;

        }
        else
            destinationliteral.Parent.Visible = false;
    }
    public void CaricaVideoSection(string Tbl_sezione, int maxwidth, int maxheight, string filtrosezione, bool mescola, Literal destinationliteral, string Lingua, string tipo = "module")
    {

        bannersDM banDM = new bannersDM(Tbl_sezione);
        dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, filtrosezione, mescola);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //CARICAMENTO DATI DB
                string pathimmagine = dt.Rows[i]["ImageUrl"].ToString();
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                string linkVideo = dt.Rows[i]["NavigateUrl"].ToString();
                if (linkVideo.ToLower().IndexOf("http://") == -1 && linkVideo.ToLower().IndexOf("https://") == -1)
                {
                    linkVideo = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + linkVideo;
                }

                //Creo l'html per il video
                switch (tipo)
                {
                    case "module":
                        //literalVideoBanner
                        sb.Append("          <section class=\"module module-video  bg-dark-30 visibleplaystop\"  data-background=\"" + pathimmagine + "\">");
                        sb.Append("               <div class=\"container\">");
                        sb.Append("                   <div class=\"row\" style=\"padding-top: 10%; padding-bottom: 10%; padding-left: 2%; padding-right: 2%;\">");
                        sb.Append("                       <div class=\" col-sm-offset-1 col-sm-10 col-xs-12\">");
                        sb.Append("                           <div class=\"testimonial-big\">");
                        sb.Append("                               <div class=\"testimonial-big-text\">");
                        sb.Append("                                   <br/>");
                        sb.Append("                                   <br/>");
                        sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(CommonPage.ReplaceLinks(dt.Rows[i]["AlternateText"].ToString())));
                        sb.Append("                                   <br/>");
                        sb.Append("                               </div>");
                        sb.Append("                               </div>");
                        sb.Append("                               </div>");
                        sb.Append("                   </div>");
                        sb.Append("               </div>");
                        sb.Append("<div class=\"video-player\" data-property=\"{videoURL:'" + linkVideo + "', containment:'.module-video', startAt:0, mute:false, autoPlay:false, loop:true, opacity:1, quality:'default', showControls:false, showYTLogo:false, vol:25}\"></div>");

                        sb.Append("    <div class=\"video-controls-box\"  ");
                        sb.Append("        <div class=\"container\">");
                        sb.Append("            <div class=\"video-controls\">");
                        sb.Append("                <a id=\"video-volume\" class=\"fa fa-volume-up\" href=\"#\">&nbsp;</a>");
                        sb.Append("                <a id=\"video-play\" class=\"fa fa-pause\" href=\"#\">&nbsp;</a>");
                        sb.Append("            </div>");
                        sb.Append("        </div>");
                        sb.Append("    </div>");
                        sb.Append("           </section>");
                        //<!-- Video start -->
                        //<section class="module module-video  bg-dark-30" data-background='<%= CommonPage.ReplaceAbsoluteLinks("~/images/sitespecific/section-12.jpg") %>'>
                        //    <div class="container">
                        //        <div class="row">
                        //            <div class="col-sm-12">
                        //                <h2 class="module-title font-alt align-left">Our office</h2>
                        //            </div>
                        //        </div>
                        //        <div class="row">
                        //            <div class="col-sm-3">
                        //                <p class="font-serif">The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc, Europe uses the same vocabulary. The languages only differ in their grammar, their pronunciation and their most common words.</p>
                        //                <p class="font-serif">The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc, Europe uses the same vocabulary. The languages only differ in their grammar, their pronunciation and their most common words.</p>
                        //            </div>
                        //            <div class="col-sm-3">
                        //                <p class="font-serif">The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc, Europe uses the same vocabulary.</p>
                        //            </div>
                        //        </div>
                        //        <!-- .row -->
                        //    </div>
                        //    <!-- Youtube player start-->
                        //    <div class="video-player" data-property="{videoURL:'https://www.youtube.com/watch?v=iNJdPyoqt8U&rel=0', containment:'.module-video', startAt:0, mute:false, autoPlay:true, loop:true, opacity:1, quality:'hd720', showControls:false, showYTLogo:false, vol:25}"></div>
                        //    <!-- Youtube player end -->
                        //    <!-- Youtube controls start-->
                        //    <div class="video-controls-box">
                        //        <div class="container">
                        //            <div class="video-controls">
                        //                <a id="video-volume" class="fa fa-volume-up" href="#">&nbsp;</a>
                        //                <a id="video-play" class="fa fa-pause" href="#">&nbsp;</a>
                        //            </div>
                        //        </div>
                        //    </div>
                        //    <!-- Youtube controls end -->

                        //</section>
                        //<!-- Video end -->
                        break;
                    case "home":
                        // literalVideoHigh
                        sb.Append("          <section class=\"home-section home-full-height bg-dark-30 visibleplaystop\"  data-background=\"" + pathimmagine + "\">");
                        sb.Append("               <div class=\"container\">");
                        sb.Append("                   <div class=\"row\" style=\"padding-top: 10%; padding-bottom: 10%; padding-left: 2%; padding-right: 2%;\">");
                        sb.Append("                       <div class=\" col-sm-offset-1 col-sm-10 col-xs-12\">");
                        sb.Append("                                   <br/>");
                        sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(CommonPage.ReplaceLinks(dt.Rows[i]["AlternateText"].ToString())));
                        sb.Append("                                   <br/>");
                        sb.Append("                               </div>");
                        sb.Append("                   </div>");
                        sb.Append("               </div>");
                        sb.Append("<div class=\"video-player\" data-property=\"{videoURL:'" + linkVideo + "', containment:'.home-section', startAt:0, mute:false, autoPlay:false, loop:true, opacity:1, quality:'default', showControls:false, showYTLogo:false, vol:25}\"></div>");
                        sb.Append("    <div class=\"video-controls-box\"> ");
                        sb.Append("        <div class=\"container\">");
                        sb.Append("            <div class=\"video-controls\">");
                        sb.Append("                <a id=\"video-volume\" class=\"fa fa-volume-up\" href=\"#\">&nbsp;</a>");
                        sb.Append("                <a id=\"video-play\" class=\"fa fa-pause\" href=\"#\">&nbsp;</a>");
                        sb.Append("            </div>");
                        sb.Append("        </div>");
                        sb.Append("    </div>");
                        sb.Append("           </section>");
                        //<!-- Home start -->
                        //  <section id="home" class="home-section home-full-height bg-dark-30" data-background='<%= CommonPage.ReplaceAbsoluteLinks("~/images/sitespecific/section-12.jpg") %>'>
                        //      <div class="video-player" data-property="{videoURL:'https://www.youtube.com/watch?v=iNJdPyoqt8U&rel=0', containment:'.home-section', startAt:0, mute:false, autoPlay:true, loop:true, opacity:1, quality:'hd1080', showControls:false, showYTLogo:false, vol:25}"></div>
                        //      <div class="video-controls-box">
                        //          <div class="container">
                        //              <div class="video-controls">
                        //                  <a id="video-volume" class="fa fa-volume-up" href="#">&nbsp;</a>
                        //                  <a id="video-play" class="fa fa-pause" href="#">&nbsp;</a>
                        //              </div>
                        //          </div>
                        //      </div>
                        //< div class="hs-caption">
                        //    <div class="caption-content">
                        //        <div class="hs-title-size-1 font-alt mb-30">
                        //            Hello & welcome
                        //        </div>
                        //        <div class="hs-title-size-4 font-alt mb-40">
                        //            Video Sample
                        //        </div>
                        //        <a href = "#about" class="buttonstyle">Learn More</a>
                        //    </div>
                        //</div>
                        //  </section>


                        //  <!-- Home end -->
                        break;
                    default:
                        break;
                }
                destinationliteral.Text = sb.ToString();
                VerticalSpacer.Style.Add(HtmlTextWriterStyle.Height, "0px"); //Senza testata -> spazio verticale per il menu

            }
        }
    }
#endif
}
