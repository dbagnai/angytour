using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Data.SQLite;

public partial class _pwadetail : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string PercorsoComune
    {
        get { return ViewState["PercorsoComune"] != null ? (string)(ViewState["PercorsoComune"]) : ""; }
        set { ViewState["PercorsoComune"] = value; }
    }
    public string PercorsoFiles
    {
        get { return ViewState["PercorsoFiles"] != null ? (string)(ViewState["PercorsoFiles"]) : ""; }
        set { ViewState["PercorsoFiles"] = value; }
    }
    public string PercorsoAssolutoApplicazione
    {
        get { return ViewState["PercorsoAssolutoApplicazione"] != null ? (string)(ViewState["PercorsoAssolutoApplicazione"]) : ""; }
        set { ViewState["PercorsoAssolutoApplicazione"] = value; }
    }

    public string idOfferta
    {
        get { return ViewState["idOfferta"] != null ? (string)(ViewState["idOfferta"]) : ""; }
        set { ViewState["idOfferta"] = value; }
    }

    public string testoindice
    {
        get { return ViewState["testoindice"] != null ? (string)(ViewState["testoindice"]) : ""; }
        set { ViewState["testoindice"] = value; }
    }

    public string CodiceTipologia
    {
        get { return ViewState["Tipologia"] != null ? (string)(ViewState["Tipologia"]) : ""; }
        set { ViewState["Tipologia"] = value; }
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
    public string FasciaPrezzo
    {
        get { return ViewState["FasciaPrezzo"] != null ? (string)(ViewState["FasciaPrezzo"]) : ""; }
        set { ViewState["FasciaPrezzo"] = value; }
    }
    public bool Vetrina
    {
        get { return ViewState["Vetrina"] != null ? (bool)(ViewState["Vetrina"]) : false; }
        set { ViewState["Vetrina"] = value; }
    }
    public string Ordinamento
    {
        get { return ViewState["Ordinamento"] != null ? (string)(ViewState["Ordinamento"]) : ""; }
        set { ViewState["Ordinamento"] = value; }
    }
    public bool JavaInjection = false;

    public Offerte item
    {
        get { return ViewState["item"] != null ? (Offerte)(ViewState["item"]) : new Offerte(); }
        set { ViewState["item"] = value; }
    }

    private int progressivosepara = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;

                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                idOfferta = CaricaValoreMaster(Request, Session, "idOfferta");

                CodiceTipologia = CaricaValoreMaster(Request, Session, "Tipologia");
                Categoria = CaricaValoreMaster(Request, Session, "Categoria");
                Categoria2liv = CaricaValoreMaster(Request, Session, "Categoria2liv", false);
                testoindice = CaricaValoreMaster(Request, Session, "testoindice");

                Caratteristica1 = CaricaValoreMaster(Request, Session, "Caratteristica1", false);
                Caratteristica2 = CaricaValoreMaster(Request, Session, "Caratteristica2", false);
                Caratteristica3 = CaricaValoreMaster(Request, Session, "Caratteristica3", false);
                Caratteristica4 = CaricaValoreMaster(Request, Session, "Caratteristica4", false);
                Caratteristica5 = CaricaValoreMaster(Request, Session, "Caratteristica5", false);
                FasciaPrezzo = CaricaValoreMaster(Request, Session, "FasciaPrezzo", false);
                Ordinamento = CaricaValoreMaster(Request, Session, "Ordinamento", false, "");
                bool tmpbool = false;
                bool.TryParse(CaricaValoreMaster(Request, Session, "Vetrina"), out tmpbool);
                Vetrina = tmpbool;


                item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
                RegistraStatistichePagina();

                DataBind();
            }
            else
            {
                output.Text = "";
            }
            if (item != null)
            {
                Categoria = item.CodiceCategoria;
                if (Categoria != "")
                    Session["Categoria"] = Categoria;
                Categoria2liv = item.CodiceCategoria2Liv;
                if (Categoria2liv != "")
                    Session["Categoria2liv"] = Categoria2liv;
                CodiceTipologia = item.CodiceTipologia;
                if (CodiceTipologia != "")
                    Session["Tipologia"] = CodiceTipologia;
                AssociaDatiSocial(item);
            }
            //CaricaControlliJS();
            SettaTestoIniziale();
            SettaVisualizzazione(item);
        }
        catch (Exception err)
        {
            output.Text = err.Message;
        }
    }

#if false
    public void CaricaControlliJS()
    {
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        //Carico la galleria in masterpage corretta
        string controllistBanHead = "";
        string sectionforbanner = CodiceTipologia;
        if (!string.IsNullOrEmpty(Categoria))
            sectionforbanner += "-" + Categoria;

        if (string.IsNullOrEmpty(CodiceTipologia))
        {
            //controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,1000);";

            sb.Clear();
            sb.Append("(function wait() {");
            sb.Append("  if (typeof injectSliderAndLoadBanner === \"function\")");
            sb.Append("    {");
            sb.Append("injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,1000);");
            sb.Append(" }");
            sb.Append("   else  {");
            sb.Append("  setTimeout(wait, 50);");
            sb.Append("  }  })();");
        }
        else
        {
            //controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','" + sectionforbanner + "',false,2000,1000);";

            sb.Clear();
            sb.Append("(function wait() {");
            sb.Append("  if (typeof injectSliderAndLoadBanner === \"function\")");
            sb.Append("    {");
            sb.Append("injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','" + sectionforbanner + "',false,2000,1000);");
            sb.Append(" }");
            sb.Append("   else  {");
            sb.Append("  setTimeout(wait, 50);");
            sb.Append("  }  })();");
        }

        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {
            cs.RegisterStartupScript(this.GetType(), "controllistBanHead", sb.ToString(), true);
        }
    }

#endif
    private void RegistraStatistichePagina()
    {
        // throw new NotImplementedException();
        string currenturl = Request.Url.ToString();
        //////////////////////////////////////////////////////////////////////////////
        //Prendiamo l'ip del client
        /////////////////////////////////////////////////////////////////////////////
        string trueIP = "";
        string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (!string.IsNullOrEmpty(ip))
        {
            string[] ipRange = ip.Split(',');
            trueIP = ipRange[0].Trim();
        }
        else
        {
            trueIP = Request.ServerVariables["REMOTE_ADDR"].Trim();
        }
        string id = idOfferta;
        int _i = 0;
        int.TryParse(id, out _i);
        //  string codicecontenuto = CodiceContenuto;

        bool visitaintiemlapse = WelcomeLibrary.DAL.statisticheDM.CaricaLastRecordStatisticaByIdcontenuto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta, System.DateTime.Now, new TimeSpan(0, 30, 0), trueIP);
        if (!visitaintiemlapse)
        {
            //Verifichiamo nel timelapse se presente una richiesta di pagina per qual contenuto dallo stesso IP
            //2 min lapse , ip address

            //Registro la statistica di contatto
            Statistiche stat = new Statistiche();
            stat.Url = currenturl;
            stat.Data = DateTime.Now;
            stat.Idattivita = _i;

            stat.EmailDestinatario = "";
            stat.EmailMittente = "";
            stat.Testomail = trueIP;
            stat.TipoContatto = enumclass.TipoContatto.visitapagina.ToString();
            WelcomeLibrary.DAL.statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);
        }
    }
    protected string GeneraBackLink(bool usacategoria = true)
    {
        string ret = "";
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
        {
            string testourl = item.Descrizione;
            Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            if (catselected != null && usacategoria)
                testourl = catselected.Descrizione;
            string tmpcategoria = Categoria;
            string tmpcategoria2liv = "";

            if (CodiceTipologia == "rif000501")
            {
                if (!string.IsNullOrEmpty(Categoria2liv))
                {
                    SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
                    if (categoriasprodotto != null && usacategoria)
                    {
                        testourl = categoriasprodotto.Descrizione;
                    }
                }
                tmpcategoria2liv = Categoria2liv;
                if (!usacategoria)
                {
                    tmpcategoria = ""; tmpcategoria2liv = "";
                }
            }

            if (!usacategoria) tmpcategoria = "";
            ret = CommonPage.CreaLinkRoutes(Session, false, Lingua, (testourl), "", CodiceTipologia, tmpcategoria, tmpcategoria2liv);
        }
        return ret;
    }
    private void SettaTestoIniziale()
    {
        TipologiaOfferte itemtipo = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (itemtipo != null)
        {
            string titolopagina = itemtipo.Descrizione.ToUpper();
            //litSezione.Text = titolopagina;
            //Prodotto catselected = Utility.ElencoProdotti.Find(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            //if (catselected != null)
            //{
            //    litSezione.Text += " " + catselected.Descrizione.ToUpper();
            //}

            string htmlPage = "";
            if (references.ResMan("Common", Lingua, "testo" + item.CodiceTipologia) != null)
                htmlPage = references.ResMan("Common", Lingua, "testo" + CodiceTipologia).ToString();
            if (references.ResMan("Common", Lingua, "testo" + item.CodiceCategoria) != null)
                htmlPage = references.ResMan("Common", Lingua, "testo" + item.CodiceCategoria).ToString();

            Contenuti content = null;

            string denominazione = item.DenominazionebyLingua(Lingua);
            string linkcanonico = CreaLinkRoutes(null, false, Lingua, CleanUrl(denominazione), item.Id.ToString(), item.CodiceTipologia);
            linkcanonico = linkcanonico.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "");
            content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkcanonico); ;
            if (content == null || content.Id == 0)
            {
                linkcanonico = linkcanonico.Substring(0, linkcanonico.LastIndexOf("/") + 1);
                content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkcanonico); ;
                if (content != null && !content.TitolobyLingua(Lingua).EndsWith("/")) content = null; //evito di prendere i contenuti dedicati alle pagine lista
            }

            if (content != null && content.Id != 0)
            {
                htmlPage = custombind.bind(ReplaceAbsoluteLinks(ReplaceLinks(content.DescrizionebyLingua(Lingua)).ToString()), Lingua, Page.User.Identity.Name, Session, null, null, Request);
                //if (htmlPage.Contains("injectandloadgenericcontent")) JavaInjection = true;
            }

            litTextHeadPage.Text = ReplaceAbsoluteLinks(ReplaceLinks(htmlPage));
        }
    }

    private void SettaVisualizzazione(Offerte item)
    {
        string controlsuggeriti = "";
        string cbandestra1 = "";
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //MODIFICO IL LAYOUT PER LA VISUALIZZAZIONE DELLA SCHEDA DETTAGLI
        switch (CodiceTipologia)
        {

            case "rif000501":
                column1.Visible = false;
                column2.Visible = true;
                /*column3.Attributes["class"] = "col-12 col-sm-3";*/
                column2.Attributes["class"] = "col-12 col-sm-9";
                column3.Visible = false;
                divSearch.Visible = false;
                //ContaArticoliPerperiodo(CodiceTipologia);
                //  Caricalinksrubriche(CodiceTipologia);
                divContact.Visible = false;
                divContactBelow.Visible = true;

                HtmlGenericControl divc = ((HtmlGenericControl)Master.FindControl("divContattiMaster"));
                divc.Visible = false;

                if (!JavaInjection)
                {
                    //controlsuggeriti = "injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12,2);";
                    //string controllatest = "injectPortfolioAndLoad(\"isotopeOfferte2.html\",\"divLatestpostContainer\", \"portlats\", 1, 12, false, \"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12);";
                    //cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";

                    //SUGGERITI
                    sb.Clear();
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectScrollerAndLoad,owlscrollerProdotti1.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,'', '" + Categoria2liv + "'");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);

                    sb.Clear();

                    //ULTIMI ARTICOLI
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divLatest1Title\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divLatest1\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divLatest1, latestposts1, 1, 6, false, '', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 6,'','','',''," + Categoria2liv + "");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divLatest1Pager\">&nbsp;</div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text = sb.ToString();

                    sb.Clear();

                    //BIND PER LA SCHEDA!!!!
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetailsprod.html,divItemContainter2, divitem,true,true, " + idOfferta + "\");");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);
                }

                break;

            case "rif000508":
                column1.Visible = false;
                column2.Visible = true;
                /*column3.Attributes["class"] = "col-12 col-sm-3";*/
                column2.Attributes["class"] = "col-12 col-sm-9";

                column3.Visible = false;

                divSearch.Visible = true;
                ContaArticoliPerperiodo(CodiceTipologia);
                //  Caricalinksrubriche(CodiceTipologia);
                divContact.Visible = false;
                divContactBelow.Visible = false;
                if (!JavaInjection)
                {
                    //controlsuggeriti = "injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12,2);";
                    //string controllatest = "injectPortfolioAndLoad(\"isotopeOfferte2.html\",\"divLatestpostContainer\", \"portlats\", 1, 12, false, \"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12);";
                    //cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";

                    //SUGGERITI
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectScrollerAndLoad,owlscrollerBlog-no-image.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();

                    //ULTIMI ARTICOLI
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divLatest1Title\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divLatest1\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divLatest1, latestposts1, 1, 6, false, '', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 6,'','','',''," + Categoria2liv + "");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divLatest1Pager\">&nbsp;</div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text = sb.ToString();

                    sb.Clear();

                    //BIND PER LA SCHEDA!!!!
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetailsBlog-no-image.html,divItemContainter2, divitem,true,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();
                }

                break;

            case "rif000509":
                column1.Visible = false;
                column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-12 col-sm-10";
                column2.Visible = true;
                column3.Visible = false;
                divSearch.Visible = false;
                divContact.Visible = false;

                if (!JavaInjection)
                {


                    sb.Clear();
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectScrollerAndLoad,owlscrollerTestimonialsBt3.html,divScrollerSuggeritiJs, carouselInject1,'', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 12, '3'\");");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetails-testimonials.html,divItemContainter2, divitem,false,true, " + idOfferta + "\");");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria

                }
                break;

            case "rif000503":
                column1.Visible = false;
                column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-12";
                column2.Visible = true;
                column3.Visible = false;
                divSearch.Visible = false;
                divContact.Visible = false;

                if (!JavaInjection)
                {
                    sb.Clear();
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m-no-prezzo.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedaPWAdetails-info.html,divItemContainter2, divitem,false,true, " + idOfferta + "\");");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;

            case "rif000504":
                column1.Visible = false;
                column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-12";
                column2.Visible = true;
                column3.Visible = false;
                divSearch.Visible = false;
                divContact.Visible = false;

                if (!JavaInjection)
                {
                    sb.Clear();
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m-no-prezzo.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedaPWAdetails-offerte.html,divItemContainter2, divitem,false,true, " + idOfferta + "\");");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;

            case "rif000505":
                column1.Visible = false;
                column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-12 col-sm-10";
                column2.Visible = true;
                column3.Visible = false;
                divSearch.Visible = false;
                divContact.Visible = false;

                if (!JavaInjection)
                {
                    sb.Clear();
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetails-nuova.html,divItemContainter2, divitem,false,true, " + idOfferta + "\");");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;

            case "rif000506":
                column1.Visible = false;
                column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-10";
                column2.Visible = true;
                column3.Visible = false;
                divSearch.Visible = false;
                divContact.Visible = false;

                if (!JavaInjection)
                {
                    sb.Clear();
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m-no-prezzo.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetailsGallery.html,divItemContainter2, divitem,false,true, " + idOfferta + "\");");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;

            case "rif000507":
                column1.Visible = false;
                column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-10";
                column2.Visible = true;
                column3.Visible = false;
                divSearch.Visible = false;
                divContact.Visible = false;

                if (!JavaInjection)
                {
                    sb.Clear();
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m-no-prezzo.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetails-staff.html,divItemContainter2, divitem,false,true, " + idOfferta + "\");");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;

            default:
                column1.Visible = false;
                column2.Visible = true;
                /*column3.Attributes["class"] = "col-12 col-sm-3";*/
                column2.Attributes["class"] = "col-12 col-md-9";
                column3.Visible = false;
                divSearch.Visible = true;
                ContaArticoliPerperiodo(CodiceTipologia);
                //  Caricalinksrubriche(CodiceTipologia);
                divContact.Visible = false;
                divContactBelow.Visible = true;
                if (!JavaInjection)
                {
                    //controlsuggeriti = "injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12,2);";
                    //string controllatest = "injectPortfolioAndLoad(\"isotopeOfferte2.html\",\"divLatestpostContainer\", \"portlats\", 1, 12, false, \"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12);";
                    //cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";

                    //SUGGERITI
                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    //sb.Append("injectScrollerAndLoad,owlPWAscroller.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 12,''\"");
                    sb.Append("injectScrollerAndLoad,swiperPWAscroller.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);//sb.ToString();

                    sb.Clear();

                    //ULTIMI ARTICOLI
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divLatest1Title\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divLatest1\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divLatest1, latestposts1, 1, 6, false, '', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 6,'','','',''," + Categoria2liv + "");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divLatest1Pager\">&nbsp;</div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text = sb.ToString();

                    sb.Clear();

                    //BIND PER LA SCHEDA!!!!
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedaPWAdetails.html,divItemContainter2, divitem,true,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);//sb.ToString();
                }

                break;

        }
    }

    protected void EvidenziaSelezione(string testolink)
    {
        HtmlAnchor linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink));
        try
        {
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#000 !important");
            }
        }
        catch { }
        try
        {
            HtmlGenericControl limenu = ((HtmlGenericControl)Master.FindControl("link" + CodiceTipologia + "high"));
            if (limenu != null)
            {
                if (limenu != null)
                {
                    ((HtmlGenericControl)limenu).Attributes["class"] += " active";
                }
            }
        }
        catch { }
        try
        {
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#000 !important");
            }
        }
        catch { }
        try
        {
            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink + "Lateral"));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#000 !important");
            }
        }
        catch { }
    }
    protected void AssociaDatiSocial(Offerte data)
    {
        string host = System.Web.HttpContext.Current.Request.Url.Host.ToString();
        string descrizione = data.DescrizionebyLingua(Lingua);
        string denominazione = data.DenominazionebyLingua(Lingua);

        //  Categoria = data.CodiceCategoria;
        EvidenziaSelezione(denominazione.Replace(" ", "").Replace("-", "").Replace("&", "e").Replace("'", "").Replace("?", ""));
        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();   //;

        //Comments facebook
        //divComments.Attributes.Add("data-href", ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, Lingua, CleanUrl(denominazione), data.Id.ToString(), data.CodiceTipologia)));

        //Titolo e descrizione pagina
        string posizione = ControlloVuotoPosizione(data.CodiceComune, data.CodiceProvincia, data.CodiceRegione, "", Lingua);

        ((HtmlTitle)Master.FindControl("metaTitle")).Text = html.Convert((denominazione).Replace("<br/>", " ").Trim() + " " + posizione + " " + Nome);
        string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 300, true)).Replace("<br/>", " ").Trim());
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;
        //Opengraph per facebook
        ((HtmlMeta)Master.FindControl("metafbTitle")).Content = html.Convert((denominazione + " " + Nome).Replace("<br/>", " ").Trim());
        simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 300, true))).Replace("<br/>", " ").Trim();
        ((HtmlMeta)Master.FindControl("metafbdescription")).Content = simpletext;
        if (data.FotoCollection_M != null && !string.IsNullOrEmpty(data.FotoCollection_M.FotoAnteprima))
            ((HtmlMeta)Master.FindControl("metafbimage")).Content = filemanage.ComponiUrlAnteprima(data.FotoCollection_M.FotoAnteprima, data.CodiceTipologia, data.Id.ToString(), true).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
        else if (data.FotoCollection_M != null && !string.IsNullOrEmpty(data.linkVideo))
            ((HtmlMeta)Master.FindControl("metafbvideourl")).Content = data.linkVideo;

        /////////////////////////////////////////////////////////////
        //MODIFICA PER TITLE E DESCRIPTION CUSTOM
        ////////////////////////////////////////////////////////////
        string customdesc = "";
        string customtitle = "";
        switch (Lingua)
        {
            case "GB":
                customdesc = data.Campo2GB;
                customtitle = data.Campo1GB;
                break;
            case "RU":
                customdesc = data.Campo2RU;
                customtitle = data.Campo1RU;
                break;
            default:
                customdesc = data.Campo2I;
                customtitle = data.Campo1I;
                break;
        }
        if (!string.IsNullOrEmpty(customtitle))
            ((HtmlTitle)Master.FindControl("metaTitle")).Text = (customtitle).Replace("<br/>", "\r\n");
        if (!string.IsNullOrEmpty(customdesc))
            ((HtmlMeta)Master.FindControl("metaDesc")).Content = customdesc.Replace("<br/>", "\r\n");
        ////////////////////////////////////////////////////////////

        ///////////////////////////////////
        //string linkcanonico = CreaLinkRoutes(null, false, Lingua, CleanUrl(denominazione), data.Id.ToString(), data.CodiceTipologia);
        //Literal litgeneric = ((Literal)Master.FindControl("litgeneric"));
        //litgeneric.Text = "<link rel=\"canonical\" href=\"" + ReplaceAbsoluteLinks(linkcanonico) + "\"/>";
        Tabrif actualpagelink = new Tabrif();
        string urlcambiolinguaenit = "";

        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));

        string hreflang = "";
        //METTIAMO GLI ALTERNATE
        hreflang = " hreflang=\"it\" ";
        string linkcanonicoalt = CreaLinkRoutes(null, false, "I", CleanUrl(data.DenominazioneI), data.Id.ToString(), data.CodiceTipologia);
        linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
            linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit"));

        Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
        litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (linkcanonicoalt) + "\"/>";
        Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
        litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + (linkcanonicoalt) + "\"/>";
        if (Lingua == "I")
        {
            litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (linkcanonicoalt) + "\"/>";
            actualpagelink.Campo1 = (linkcanonicoalt);
            actualpagelink.Campo2 = (data.DenominazioneI);
        }
        else urlcambiolinguaenit = linkcanonicoalt;
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            linkcanonicoalt = CreaLinkRoutes(null, false, "GB", CleanUrl(data.DenominazioneGB), data.Id.ToString(), data.CodiceTipologia);
            linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen"));

            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + (linkcanonicoalt) + "\"/>";
            if (Lingua == "GB")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (linkcanonicoalt) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = (linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(data.DenominazioneGB);
            }
            else urlcambiolinguaenit = linkcanonicoalt;
        }

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
        {
            hreflang = " hreflang=\"ru\" ";
            linkcanonicoalt = CreaLinkRoutes(null, false, "RU", CleanUrl(data.DenominazioneRU), data.Id.ToString(), data.CodiceTipologia);
            linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru"));

            litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + (linkcanonicoalt) + "\"/>";
            if (Lingua == "RU")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (linkcanonicoalt) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = (linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(data.DenominazioneRU);
            }
        }
        HtmlGenericControl divCambioLinguaen = (HtmlGenericControl)Master.FindControl("divCambioLinguaen");
        divCambioLinguaen.InnerHtml = "<a style=\"color: White; padding: 0px\" ";
        divCambioLinguaen.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
        divCambioLinguaen.InnerHtml += "href=\"";
        divCambioLinguaen.InnerHtml += urlcambiolinguaenit;
        divCambioLinguaen.InnerHtml += "\" >";
        divCambioLinguaen.InnerHtml += references.ResMan("Common", Lingua, "testoCambio").ToUpper();
        divCambioLinguaen.InnerHtml += "</a>";
        divCambioLinguaen.Visible = true;
        HtmlGenericControl divCambioLinguadef = (HtmlGenericControl)Master.FindControl("divCambioLinguadef");
        divCambioLinguadef.Visible = false;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////BREAD CRUMBS///////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        List<Tabrif> links = GeneraBreadcrumbPath(true);
        links.Add(actualpagelink);

        HtmlGenericControl ulbr = (HtmlGenericControl)Master.FindControl("ulBreadcrumb");
        ulbr.InnerHtml = BreadcrumbConstruction(links);

    }
    private List<Tabrif> GeneraBreadcrumbPath(bool usacategoria)
    {
        List<Tabrif> links = new List<Tabrif>();
        Tabrif link = new Tabrif();
        link.Campo1 = ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkHome"));
        link.Campo2 = references.ResMan("Common", Lingua, "testoHome");
        links.Add(link);

        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
        {
            string testourl = item.Descrizione;
            Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            if (catselected != null && usacategoria)
                testourl = catselected.Descrizione;
            if (!string.IsNullOrEmpty(Categoria2liv))
            {
                SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
                if (categoriasprodotto != null && usacategoria)
                {
                    testourl = categoriasprodotto.Descrizione;
                }
            }
            string tmpcategoria = Categoria;
            string tmpcategoria2liv = Categoria2liv;
            if (!usacategoria)
            {
                tmpcategoria = ""; tmpcategoria2liv = "";
            }

            string linkcanonicoalt = CreaLinkRoutes(null, false, Lingua, CleanUrl(testourl), "", CodiceTipologia, tmpcategoria, tmpcategoria2liv);
            link = new Tabrif();
            link.Campo1 = linkcanonicoalt;
            link.Campo2 = testourl;

            //if (CodiceTipologia == "rif000003") //Pagina copertina presente
            if (CodiceTipologia == "rif000501") //Pagina copertina presente
            {
                if (item != null && !string.IsNullOrEmpty(item.Descrizione.ToLower().Trim()))
                {
                    Contenuti contentpertipologia = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "categorie " + item.Descrizione.ToLower().Trim());
                    if (contentpertipologia != null && contentpertipologia.Id != 0)
                    {
                        Tabrif laddink = new Tabrif();
                        laddink.Campo1 = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(contentpertipologia.TitolobyLingua(Lingua)), contentpertipologia.Id.ToString(), "con001000");
                        laddink.Campo2 = contentpertipologia.TitolobyLingua(Lingua);
                        links.Add(laddink);
                    }
                }
                //Prodotto catcopertina = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == CodiceTipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
                //if (catcopertina != null && !string.IsNullOrEmpty((catcopertina.Descrizione.ToLower().Trim())))
                //{
                //    Contenuti contentpercategoria = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, catcopertina.Descrizione.ToLower().Trim());
                //    if (contentpercategoria != null && contentpercategoria.Id != 0)
                //    {
                //        Tabrif laddink = new Tabrif();
                //        laddink.Campo1 = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(contentpercategoria.TitolobyLingua(Lingua)), contentpercategoria.Id.ToString(), "con001000");
                //        laddink.Campo2 = contentpercategoria.TitolobyLingua(Lingua);
                //        links.Add(laddink);
                //    }
                //}
            }

            links.Add(link);
        }
        return links;
    }

    private void Caricalinksrubriche(string cattipo)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        divLinksrubriche.Visible = true;
        sb.Append("<div id=\"divLinksrubrichecontainer\" style=\"overflow-y: auto\" class=\"inject\" params=\"");
        sb.Append("injcCategorieLinks,'linkslistddl2.html','divLinksrubrichecontainer', 'linksrubriche1','','" + cattipo + "','" + Categoria + "',''" + "\");");
        sb.Append("\"></div>");
        divLinksrubriche.InnerHtml = (sb.ToString());
    }
    private void ContaArticoliPerperiodo(string cattipo)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        divArchivio.Visible = true;
        divLinksrubriche.Visible = true;
        sb.Append("<div id=\"divArchivioList\" style=\"overflow-y: auto\" class=\"inject\" params=\"");
        sb.Append("injectArchivioAndLoad,'listaarchivio.html','divArchivioList', 'archivio1','','" + cattipo + "','" + Categoria + "',''" + "\");");
        sb.Append("\"></div>");
        divArchivio.InnerHtml = (sb.ToString());

        //ClientScriptManager cs = Page.ClientScript;
        //System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //sb.Clear();
        //sb.Append("(function wait() {");
        //sb.Append("  if (typeof injectArchivioAndLoad === \"function\")");
        //sb.Append("    {");
        //sb.Append("injectArchivioAndLoad('listaarchivio.html','divArchivioList', 'archivio1','','" + cattipo + "','" + Categoria + "','');");
        //sb.Append(" }");
        //sb.Append("   else  {");
        //sb.Append("  setTimeout(wait, 50);");
        //sb.Append("  }  })();");
        //if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        //{
        //    cs.RegisterStartupScript(this.GetType(), "clistarchivio", sb.ToString(), true);
        //}
    }
    protected void Cerca_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Clear();
        //string link = CreaLinkRicerca("", CodiceTipologia, Categoria, "", "", "", "", "-", Lingua, Session, true);
        string link = CreaLinkRicerca("", CodiceTipologia, "", "", "", "", "", "-", Lingua, Session, true);
        Session.Add("testoricerca", Server.HtmlEncode(inputCerca.Value)); //carico in sessione il parametro da cercare
        Response.Redirect(link);
    }
    protected void btnRegistrastatistiche_Click(object sender, EventArgs e)
    {
        string id = (((Button)sender).CommandArgument);
        Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
        long _i = 0;
        long.TryParse(id, out _i);
        //Registro la statistica di contatto
        Statistiche stat = new Statistiche();
        stat.Data = DateTime.Now;
        stat.EmailDestinatario = "";
        stat.EmailMittente = "";
        stat.Idattivita = _i;
        stat.Testomail = "";
        stat.TipoContatto = enumclass.TipoContatto.visitaurl.ToString();
        stat.Url = item.Website;
        WelcomeLibrary.DAL.statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);
    }


 
}