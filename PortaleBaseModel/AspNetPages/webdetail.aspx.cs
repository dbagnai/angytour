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

public partial class _webdetail : CommonPage
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
                CodiceTipologia = CaricaValoreMaster(Request, Session, "Tipologia", true);
                Categoria = CaricaValoreMaster(Request, Session, "Categoria", true);
                Categoria2liv = CaricaValoreMaster(Request, Session, "Categoria2liv", true);
                testoindice = CaricaValoreMaster(Request, Session, "testoindice", true);
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

                //carichiamo per id la scheda
                item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);

                //////////////////////////////////////////////
                //Redirect pagine archiviate o non trovate
                //////////////////////////////////////////////
                if (item != null && item.Id != 0 && item.Archiviato)
                {
                    if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage").ToLower() == Lingua.ToLower())
                        Response.RedirectPermanent("~"); //  caso defalut language 
                    else
                        Response.RedirectPermanent("~/" + SitemapManager.getCulturenamefromlingua(Lingua) + "/home"); //qui potresti rigirare alla sezione tipologia/categoria se vuoi o ad una particolare pagina ( essendo articolo archiviato )
                }
                else if (item == null)
                    Response.RedirectPermanent("~/" + SitemapManager.getCulturenamefromlingua(Lingua) + "/home");


                //RegistraStatistichePagina();

                DataBind();
            }
            else
            {
                output.Text = "";
            }
            if (item != null)
            {
                CodiceTipologia = item.CodiceTipologia;
                if (CodiceTipologia != "")
                    Session["Tipologia"] = CodiceTipologia;
                Categoria = item.CodiceCategoria;
                if (Categoria != "")// && CodiceTipologia != "rif000003")
                    Session["Categoria"] = Categoria;
                Categoria2liv = item.CodiceCategoria2Liv;
                if (Categoria2liv != "")// && CodiceTipologia != "rif000003")
                    Session["Categoria2liv"] = Categoria2liv;

                //replicao i valori alla master per funzione ricerca
                Master.CodiceTipologia = CodiceTipologia;
                Master.Categoria = Categoria;
                Master.Categoria2liv = Categoria2liv;


                SettaVisualizzazione(item);
            }
            //CaricaControlliJS();
        }
        catch (Exception err)
        {
            output.Text = err.Message;
        }
    }


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

            if (CodiceTipologia == "rif000001")
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



    private void SettaVisualizzazione(Offerte item)
    {
        InizializzaSeo(item);

        custombind cb = new custombind();
        string controlsuggeriti = "";
        string cbandestra1 = "";
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //MODIFICO IL LAYOUT PER LA VISUALIZZAZIONE DELLA SCHEDA DETTAGLI
        switch (CodiceTipologia)
        {

            case "rif000001":
                column1.Visible = false;
                column2.Visible = true;
                /*column3.Attributes["class"] = "col-12 col-sm-3";*/
                column2.Attributes["class"] = "col-12 col-sm-10";
                column3.Visible = false;
                divSearch.Visible = false;
                //ContaArticoliPerperiodo(CodiceTipologia);
                //  Caricalinksrubriche(CodiceTipologia);
                divContact.Visible = false;
                divContactBelow.Visible = true;

                pnlCommenti.Visible = true; //visualizzo i commenti!!

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
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);

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
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject no-plygon\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetailsprod.html,divItemContainter2, divitem,true,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);
                }

                break;
            case "rif000002":
                column1.Visible = false;
                column2.Visible = true;
                /*column3.Attributes["class"] = "col-12 col-sm-3";*/
                column2.Attributes["class"] = "col-12 col-sm-10";
                column3.Visible = false;
                divSearch.Visible = true;
                //ContaArticoliPerperiodo(CodiceTipologia);
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
                    sb.Append("injectScrollerAndLoad,owlscrollerBlog2.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);//sb.ToString();

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
                    sb.Append("injectandloadgenericcontent,schedadetailsBlog.html,divItemContainter2, divitem,true,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);//sb.ToString();
                }

                break;
            case "rif000008":
                column1.Visible = false;
                column2.Visible = true;
                /*column3.Attributes["class"] = "col-12 col-sm-3";*/
                column2.Attributes["class"] = "col-12 col-sm-10";

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
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

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
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();
                }

                break;

            case "rif000009":
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
                    sb.Append("injectScrollerAndLoad,owlscrollerTestimonialsBt3.html,divScrollerSuggeritiJs, carouselInject1,'', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 12, '3'\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetails-testimonials.html,divItemContainter2, divitem,false,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria

                }
                break;

            case "rif000003":
            case "rif000004":
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
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m-no-prezzo.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetails-nuova.html,divItemContainter2, divitem,false,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;

            case "rif000005":
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
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m-no-prezzo.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetails-nuova-gallerymasonry.html,divItemContainter2, divitem,false,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;

            case "rif000006":
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
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m-no-prezzo.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetailsGallery.html,divItemContainter2, divitem,false,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;

            case "rif000007":
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
                    sb.Append("injectScrollerAndLoad,owlscrollerOfferte-m-no-prezzo.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', false, true, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    sb.Clear();
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetails-staff.html,divItemContainter2, divitem,false,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request); //sb.ToString();

                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria
                }
                break;
            case "rif000012":
                column1.Visible = false;
                column2.Visible = true;
                /*column3.Attributes["class"] = "col-12 col-sm-3";*/
                column2.Attributes["class"] = "col-12 col-sm-8";
                column3.Visible = false;
                divSearch.Visible = true;
                //ContaArticoliPerperiodo(CodiceTipologia);
                //  Caricalinksrubriche(CodiceTipologia);
                divContact.Visible = false;
                divContactBelow.Visible = false;
                if (!JavaInjection)
                {

                    //SUGGERITI
                    //sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    //sb.Append("injectScrollerAndLoad,owlscrollerBlog2.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 12,''\"");
                    //sb.Append("\"></div>");

                    sb.Append("<div id=\"divScrollerSuggeritiJs\" class=\"inject\" params=\"");
                    sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioBlog3CardOverlay.html,divScrollerSuggeritiJs,portlist1, 1, 1,false,\'30\',\'" + CodiceTipologia + "\',\'" + Categoria + "\',false,false");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);//sb.ToString();


                    sb.Clear();

                    //BIND PER LA SCHEDA!!!!
                    sb.Append("<div id=\"divItemContainter2\" style=\"position: relative; display: none\" class=\"inject\" params=\"");
                    sb.Append("injectandloadgenericcontent,schedadetailsGallery.html,divItemContainter2, divitem,false,false, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);//sb.ToString();
                }

                break;
            default:
                column1.Visible = false;
                column2.Visible = true;
                /*column3.Attributes["class"] = "col-12 col-sm-3";*/
                column2.Attributes["class"] = "col-12 col-sm-10 offset-sm-1";
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
                    sb.Append("injectScrollerAndLoad,owlscrollerBlog2.html,divScrollerSuggeritiJs, scrollersuggeriti,'', '" + CodiceTipologia + "', '" + Categoria + "', true, false, 12,''\"");
                    sb.Append("\"></div>");
                    plhSuggeritiJs.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);//sb.ToString();

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
                    sb.Append("injectandloadgenericcontent,schedadetailsBlog.html,divItemContainter2, divitem,true,true, " + idOfferta + "\"");
                    sb.Append("\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);//sb.ToString();
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
    protected void InizializzaSeo(Offerte data)
    {
        if (data == null) return;

        string host = System.Web.HttpContext.Current.Request.Url.Host.ToString();
        string descrizione = data.DescrizionebyLingua(Lingua);


        #region CUSTOMIZZAZIONE TESTATA PAGINA
        /////////////////////////
        //CAMBIO TESTATA DI PAGINA
        /////////////////////////
        TipologiaOfferte itemtipo = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (itemtipo != null)
        {
            //string titolopagina = itemtipo.Descrizione.ToUpper();
            //litSezione.Text = titolopagina;
            //Prodotto catselected = Utility.ElencoProdotti.Find(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            //if (catselected != null)
            //{
            //    litSezione.Text += " " + catselected.Descrizione.ToUpper();
            //}
            string htmlPage = "";
            if (references.ResMan("Common", Lingua, "testo" + data.CodiceTipologia) != null)
                htmlPage = references.ResMan("Common", Lingua, "testo" + CodiceTipologia).ToString();
            if (references.ResMan("Common", Lingua, "testo" + data.CodiceCategoria) != null)
                htmlPage = references.ResMan("Common", Lingua, "testo" + data.CodiceCategoria).ToString();
            Contenuti content = null;

            string linkcanonico = CreaLinkRoutes(null, false, Lingua, CleanUrl(data.UrltextforlinkbyLingua(Lingua)), data.Id.ToString(), data.CodiceTipologia);
            linkcanonico = linkcanonico.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "");
            content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkcanonico); ;
            if (content == null || content.Id == 0) //no pagina statica associata all'url completo
            {
                linkcanonico = linkcanonico.Substring(0, linkcanonico.LastIndexOf("/") + 1);
                content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkcanonico); //Cerco pagina statica associalta
                if (content != null && !content.TitolobyLingua(Lingua).EndsWith("/")) content = null; //evito di prendere i contenuti dedicati alle pagine lista
            }
            if (content != null && content.Id != 0)
            {
                custombind cb = new custombind();
                htmlPage = cb.bind(ReplaceAbsoluteLinks(ReplaceLinks(content.DescrizionebyLingua(Lingua)).ToString()), Lingua, Page.User.Identity.Name, Session, null, null, Request);
                //if (htmlPage.Contains("injectandloadgenericcontent")) JavaInjection = true;
            }
            litTextHeadPage.Text = htmlPage;
        }
        #endregion

        /////////////////////////////////////////////
        ///META ROBOTS custom
        /////////////////////////////////////////////
        HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
        if (!string.IsNullOrEmpty(data.Robots.Trim()))
            metarobots.Attributes["Content"] = data.Robots.Trim();


        //  Categoria = data.CodiceCategoria;
        //EvidenziaSelezione(denominazione.Replace(" ", "").Replace("-", "").Replace("&", "e").Replace("'", "").Replace("?", ""));

        /////////////////////////////////////////////////////////////
        //META TITLE E DESCRIPTION  default e CUSTOM
        ////////////////////////////////////////////////////////////
        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();
        //Titolo e descrizione pagina default presi dai contenuti di pagina
        string posizione = ControlloVuotoPosizione(data.CodiceComune, data.CodiceProvincia, data.CodiceRegione, "", Lingua);
        ((HtmlTitle)Master.FindControl("metaTitle")).Text = html.Convert((data.DenominazionebyLingua(Lingua)).Replace("<br/>", " ").Trim() + " " + posizione + " " + Nome);
        string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 300, true)).Replace("<br/>", " ").Trim());
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;
        //Opengraph per facebook
        ((HtmlMeta)Master.FindControl("metafbTitle")).Content = html.Convert((data.DenominazionebyLingua(Lingua) + " " + Nome).Replace("<br/>", " ").Trim());
        simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 300, true))).Replace("<br/>", " ").Trim();
        ((HtmlMeta)Master.FindControl("metafbdescription")).Content = simpletext;
        if (data.FotoCollection_M != null && !string.IsNullOrEmpty(data.FotoCollection_M.FotoAnteprima))
            ((HtmlMeta)Master.FindControl("metafbimage")).Content = filemanage.ComponiUrlAnteprima(data.FotoCollection_M.FotoAnteprima, data.CodiceTipologia, data.Id.ToString(), true).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
        else if (data.FotoCollection_M != null && !string.IsNullOrEmpty(data.linkVideo))
            ((HtmlMeta)Master.FindControl("metafbvideourl")).Content = data.linkVideo;
        //customizzazione utente per i meta title e description
        string customdesc = "";
        string customtitle = "";
        switch (Lingua.ToLower())
        {
            case "gb":
                customdesc = data.Campo2GB;
                customtitle = data.Campo1GB;
                break;
            case "ru":
                customdesc = data.Campo2RU;
                customtitle = data.Campo1RU;
                break;
            case "FR":
                customdesc = data.Campo2FR;
                customtitle = data.Campo1FR;
                break;
            case "i":
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
        ///////CANONICAL E ALTERNATE///
        ////////////////////////////////
        Tabrif actualpagelink = new Tabrif();
        string linki = "";
        string linken = "";
        string linkru = "";
        string linkfr = "";
        string hreflang = "";
        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));

        //CULTURA it ( set canonical eactualpage )
        hreflang = " hreflang=\"it\" ";
        linki = ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, "I", CleanUrl(data.UrltextforlinkbyLingua("I")), data.Id.ToString(), data.CodiceTipologia));
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
            linki = linki.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit"));
        //FORZATURA CANONICAL utente
        string modcanonical = linki;
        if (!string.IsNullOrEmpty(data.CanonicalbyLingua("I").Trim()))
            modcanonical = (data.CanonicalbyLingua("I").Trim());

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatei").ToLower() == "true")
        {
            //alternate
            Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
            if (!string.IsNullOrEmpty(CleanUrl(data.UrltextforlinkbyLingua("I"))))
                litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "I")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>";
            }
        }

        if (Lingua.ToLower() == "i")
        {
            //canonical
            litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
            actualpagelink.Campo1 = (linki);
            actualpagelink.Campo2 = (data.DenominazionebyLingua("I"));

            //redirect al canonical se il canonical non coincide con l'url
            if (string.IsNullOrEmpty(data.CanonicalbyLingua("I").Trim())) // redirect solo se vuoto il campo di forzatura del canonical
                if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
                {
                    Response.RedirectPermanent(modcanonical, true);
                }
        }

        //cultura en ( set canonical eactualpage )
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            linken = ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, "GB", CleanUrl(data.UrltextforlinkbyLingua("GB")), data.Id.ToString(), data.CodiceTipologia));
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linken = linken.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen"));

            //FORZATURA CANONICAL utente
            modcanonical = linken;
            if (!string.IsNullOrEmpty(data.CanonicalbyLingua("GB").Trim()))
                modcanonical = (data.CanonicalbyLingua("GB").Trim());
            //alternate
            Literal litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            if (!string.IsNullOrEmpty(CleanUrl(data.UrltextforlinkbyLingua("GB"))))
                litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "GB")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>";
            }
            if (Lingua.ToLower() == "gb")
            {
                //canonical
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = (linken);
                actualpagelink.Campo2 = CleanUrl(data.DenominazionebyLingua("GB"));
                //redirect al canonical se il canonical non coincide con l'url
                if (string.IsNullOrEmpty(data.CanonicalbyLingua("GB").Trim())) // redirect solo se vuoto il campo di forzatura del canonical
                    if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
                    {
                        Response.RedirectPermanent(modcanonical, true);
                    }
            }
        }
        //CULTURA ru ( set canonical eactualpage )
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
        {
            hreflang = " hreflang=\"ru\" ";
            linkru = ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, "RU", CleanUrl(data.UrltextforlinkbyLingua("RU")), data.Id.ToString(), data.CodiceTipologia));
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkru = linkru.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru"));

            //FORZATURA CANONICAL utente
            modcanonical = linkru;
            if (!string.IsNullOrEmpty(data.CanonicalbyLingua("RU").Trim()))
                modcanonical = (data.CanonicalbyLingua("RU").Trim());
            //alternate
            Literal litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
            if (!string.IsNullOrEmpty(CleanUrl(data.UrltextforlinkbyLingua("RU"))))
                litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "RU")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>";
            }
            if (Lingua.ToLower() == "ru")
            {
                //canonical
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = (linkru);
                actualpagelink.Campo2 = CleanUrl(data.DenominazionebyLingua("RU"));
                //redirect al canonical se il canonical non coincide con l'url
                if (string.IsNullOrEmpty(data.CanonicalbyLingua("RU").Trim())) // redirect solo se vuoto il campo di forzatura del canonical
                    if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
                    {
                        Response.RedirectPermanent(modcanonical, true);
                    }
            }
        }

        //CULTURA dk ( set canonical eactualpage )
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() == "true")
        {
            hreflang = " hreflang=\"fr\" ";
            linkfr = ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, "FR", CleanUrl(item.UrltextforlinkbyLingua("FR")), data.Id.ToString(), data.CodiceTipologia));
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkfr = linkfr.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainfr"));

            //FORZATURA CANONICAL utente
            modcanonical = linkfr;
            if (!string.IsNullOrEmpty(data.CanonicalbyLingua("FR").Trim()))
                modcanonical = (data.CanonicalbyLingua("FR").Trim());
            //alternate
            Literal litgenericalt = ((Literal)Master.FindControl("litgeneric4"));
            if (!string.IsNullOrEmpty(CleanUrl(data.UrltextforlinkbyLingua("FR"))))
                litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "FR")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>";
            }
            if (Lingua.ToLower() == "fr")
            {
                //canonical
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = (linkfr);
                actualpagelink.Campo2 = CleanUrl(data.DenominazionebyLingua("FR"));
                //redirect al canonical se il canonical non coincide con l'url
                if (string.IsNullOrEmpty(data.CanonicalbyLingua("FR").Trim())) // redirect solo se vuoto il campo di forzatura del canonical
                    if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
                    {
                        Response.RedirectPermanent(modcanonical, true);
                    }
            }
        }


        //SET LINK PER CAMBIO LINGUA
        SettaLinkCambioLingua(linki, data.UrltextforlinkbyLingua("I"), linken, data.UrltextforlinkbyLingua("GB"), linkru, data.UrltextforlinkbyLingua("RU"), linkfr, data.UrltextforlinkbyLingua("FR"));

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////BREAD CRUMBS///////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        List<Tabrif> links = new List<Tabrif>();
        bool usacategorie = true;
        //if (CodiceTipologia == "rif000003") usacategorie = false;
        links = GeneraBreadcrumbPath(usacategorie);
        links.Add(actualpagelink);

        HtmlGenericControl ulbr = (HtmlGenericControl)Master.FindControl("ulBreadcrumb");
        ulbr.InnerHtml = BreadcrumbConstruction(links);


        //Comments facebook
        //divComments.Attributes.Add("data-href", actualpagelink.Campo1);

    }
    private void SettaLinkCambioLingua(string linki, string urltexti, string linken, string urltexten, string linkru, string urltextru, string linkfr, string urltextfr)
    {
        //SET LINK PER CAMBIO LINGUA
        HtmlGenericControl divCambioLingua1 = (HtmlGenericControl)Master.FindControl("divCambioLingua1");
        HtmlGenericControl divCambioLingua2 = (HtmlGenericControl)Master.FindControl("divCambioLingua2");
        HtmlGenericControl divCambioLingua3 = (HtmlGenericControl)Master.FindControl("divCambioLingua3");
        divCambioLingua1.Visible = false;
        divCambioLingua2.Visible = false;
        divCambioLingua3.Visible = false;

        //valori di default non dall'activate language
        HtmlGenericControl divCambioLinguadef1 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef1");
        HtmlGenericControl divCambioLinguadef2 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef2");
        HtmlGenericControl divCambioLinguadef3 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef3");
        divCambioLinguadef1.Visible = false;
        divCambioLinguadef2.Visible = false;
        divCambioLinguadef3.Visible = false;

        switch (Lingua.ToLower())
        {
            case "i":
                if (!string.IsNullOrEmpty(linken) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltexten)))
                {
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linken;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateen").ToLower() == "true") divCambioLinguadef1.Visible = true;

                if (!string.IsNullOrEmpty(linkru) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltextru)))
                {
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linkru;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true") divCambioLinguadef2.Visible = true;
                if (!string.IsNullOrEmpty(linkfr) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltextfr)))
                {
                    divCambioLingua3.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua3.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua3.InnerHtml += "href=\"";
                    divCambioLingua3.InnerHtml += linkfr;
                    divCambioLingua3.InnerHtml += "\" >";
                    divCambioLingua3.InnerHtml += references.ResMan("Common", Lingua, "testoCambio3").ToUpper();
                    divCambioLingua3.InnerHtml += "</a>";
                    divCambioLingua3.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() == "true") divCambioLinguadef3.Visible = true;

                break;
            case "gb":
                if (!string.IsNullOrEmpty(linki) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltexti)))
                {
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linki;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                }
                else divCambioLinguadef1.Visible = true;

                if (!string.IsNullOrEmpty(linkru) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltextru)))
                {
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linkru;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true") divCambioLinguadef2.Visible = true;
                if (!string.IsNullOrEmpty(linkfr) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltextfr)))
                {
                    divCambioLingua3.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua3.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua3.InnerHtml += "href=\"";
                    divCambioLingua3.InnerHtml += linkfr;
                    divCambioLingua3.InnerHtml += "\" >";
                    divCambioLingua3.InnerHtml += references.ResMan("Common", Lingua, "testoCambio3").ToUpper();
                    divCambioLingua3.InnerHtml += "</a>";
                    divCambioLingua3.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() == "true") divCambioLinguadef3.Visible = true;
                break;
            case "ru":
                if (!string.IsNullOrEmpty(linken) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltexten)))
                {
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linken;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateen").ToLower() == "true") divCambioLinguadef1.Visible = true;
                if (!string.IsNullOrEmpty(linki) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltexti)))
                {
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linki;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                }
                else divCambioLinguadef2.Visible = true;
                if (!string.IsNullOrEmpty(linkfr) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltextfr)))
                {
                    divCambioLingua3.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua3.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua3.InnerHtml += "href=\"";
                    divCambioLingua3.InnerHtml += linkfr;
                    divCambioLingua3.InnerHtml += "\" >";
                    divCambioLingua3.InnerHtml += references.ResMan("Common", Lingua, "testoCambio3").ToUpper();
                    divCambioLingua3.InnerHtml += "</a>";
                    divCambioLingua3.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() == "true") divCambioLinguadef3.Visible = true;
                break;
            case "fr":
                if (!string.IsNullOrEmpty(linken) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltexten)))
                {
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linken;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateen").ToLower() == "true") divCambioLinguadef1.Visible = true;
                if (!string.IsNullOrEmpty(linki) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltexti)))
                {
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linki;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                }
                else divCambioLinguadef2.Visible = true;
                if (!string.IsNullOrEmpty(linkru) && !string.IsNullOrEmpty(CommonPage.CleanUrl(urltextru)))
                {
                    divCambioLingua3.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua3.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua3.InnerHtml += "href=\"";
                    divCambioLingua3.InnerHtml += linkru;
                    divCambioLingua3.InnerHtml += "\" >";
                    divCambioLingua3.InnerHtml += references.ResMan("Common", Lingua, "testoCambio3").ToUpper();
                    divCambioLingua3.InnerHtml += "</a>";
                    divCambioLingua3.Visible = true;
                }
                else if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true") divCambioLinguadef3.Visible = true;

                break;
        }
    }

    private List<Tabrif> GeneraBreadcrumbPath(bool usacategoria)
    {
        List<Tabrif> links = new List<Tabrif>();
        Tabrif link = null;
        Tabrif link1 = null;
        Tabrif link2 = null;
        Tabrif link3 = null;
        string linkurl = "";

        link = new Tabrif();
        link.Campo1 = ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkHome"));
        link.Campo2 = references.ResMan("Common", Lingua, "testoHome");
        links.Add(link);
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
        {
            //1 livello tipologia
            linkurl = CreaLinkRoutes(null, false, Lingua, CleanUrl(item.Descrizione), "", CodiceTipologia, "", "");
            link1 = new Tabrif();
            link1.Campo1 = linkurl;
            link1.Campo2 = item.Descrizione;

            //2 livello categoria
            if (!string.IsNullOrEmpty(Categoria) && usacategoria)
            {
                Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
                if (catselected != null)
                {
                    linkurl = CreaLinkRoutes(null, false, Lingua, CleanUrl(catselected.Descrizione), "", CodiceTipologia, Categoria, "");
                    link2 = new Tabrif();
                    link2.Campo1 = linkurl;
                    link2.Campo2 = catselected.Descrizione;
                }
            }

            //3 livello categoria 2 livello
            if (!string.IsNullOrEmpty(Categoria2liv) && usacategoria)
            {
                SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
                if (categoriasprodotto != null)
                {
                    linkurl = CreaLinkRoutes(null, false, Lingua, CleanUrl(categoriasprodotto.Descrizione), "", CodiceTipologia, Categoria, Categoria2liv);
                    link3 = new Tabrif();
                    link3.Campo1 = linkurl;
                    link3.Campo2 = categoriasprodotto.Descrizione;
                }
            }

            //Customizzazione pagina copertina di navigazione sezione con pagine statiche ( HOME DI SEZIONE PERSONALIZZATE )
            //if (CodiceTipologia == "rif000003" || CodiceTipologia == "rif000002")
            if (CodiceTipologia == "rif000001" || CodiceTipologia == "rif000002")
            {
                //1 livello
                if (item != null && !string.IsNullOrEmpty(item.Descrizione.ToLower().Trim()))
                {
                    Contenuti contentpertipologia = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "home " + item.Descrizione.ToLower().Trim());
                    if (contentpertipologia != null && contentpertipologia.Id != 0)
                    {
                        link1 = new Tabrif();
                        link1.Campo1 = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(contentpertipologia.TitolobyLingua(Lingua)), contentpertipologia.Id.ToString(), "con001000"); ;
                        link1.Campo2 = contentpertipologia.TitolobyLingua(Lingua);
                    }
                }

                //2livello
                Prodotto catcopertina = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == CodiceTipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
                if (catcopertina != null && !string.IsNullOrEmpty((catcopertina.Descrizione.ToLower().Trim())))
                {
                    Contenuti contentpercategoria = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "home " + catcopertina.Descrizione.ToLower().Trim());
                    if (contentpercategoria != null && contentpercategoria.Id != 0)
                    {
                        link2 = new Tabrif();
                        link2.Campo1 = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(contentpercategoria.TitolobyLingua(Lingua)), contentpercategoria.Id.ToString(), "con001000");
                        link2.Campo2 = contentpercategoria.TitolobyLingua(Lingua);
                    }
                }


                //3livello
                SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
                if (categoriasprodotto != null && !string.IsNullOrEmpty((categoriasprodotto.Descrizione.ToLower().Trim())))
                {
                    Contenuti contentpersottocategoria = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "home " + categoriasprodotto.Descrizione.ToLower().Trim());
                    if (contentpersottocategoria != null && contentpersottocategoria.Id != 0)
                    {
                        link3 = new Tabrif();
                        link3.Campo1 = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(contentpersottocategoria.TitolobyLingua(Lingua)), contentpersottocategoria.Id.ToString(), "con001000");
                        link3.Campo2 = contentpersottocategoria.TitolobyLingua(Lingua);
                    }
                }
            }
        }

        if (link1 != null) links.Add(link1);
        if (link2 != null) links.Add(link2);
        if (link3 != null) links.Add(link3);

        return links;
    }
    private void Caricalinksrubriche(string cattipo)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        divLinksrubriche.Visible = true;
        sb.Append("<div id=\"divLinksrubrichecontainer\" style=\"overflow-y: auto\" class=\"inject\" params=\"");
        sb.Append("injcCategorieLinks,'linkslistddl2.html','divLinksrubrichecontainer', 'linksrubriche1','','" + cattipo + "','" + Categoria + "',''" + "\"");
        sb.Append("\"></div>");
        divLinksrubriche.InnerHtml = (sb.ToString());
    }
    private void ContaArticoliPerperiodo(string cattipo)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        divArchivio.Visible = true;
        divLinksrubriche.Visible = true;
        sb.Append("<div id=\"divArchivioList\" style=\"overflow-y: auto\" class=\"inject\" params=\"");
        sb.Append("injectArchivioAndLoad,'listaarchivio.html','divArchivioList', 'archivio1','','" + cattipo + "','" + Categoria + "',''" + "\"");
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
    //protected void Cerca_Click(object sender, EventArgs e)
    //{
    //    HttpContext.Current.Session.Clear();
    //    //string link = CreaLinkRicerca("", CodiceTipologia, Categoria, "", "", "", "", "-", Lingua, Session, true);
    //    string link = CreaLinkRicerca("", CodiceTipologia, "", "", "", "", "", "-", Lingua, Session, true);
    //    Session.Add("testoricerca", Server.HtmlEncode(inputCerca.Value)); //carico in sessione il parametro da cercare
    //    Response.Redirect(link);
    //}
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