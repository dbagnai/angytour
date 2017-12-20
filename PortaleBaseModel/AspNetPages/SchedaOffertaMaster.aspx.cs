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

public partial class _SchedaOffertaMaster : CommonPage
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
    public bool JavaInjection = false;

    int progressivosepara = 1;
    Offerte item = new Offerte();
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

                item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
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

                DataBind();
            }
            else
            {
                output.Text = "";
            }

        }
        catch (Exception err)
        {
            output.Text = err.Message;
        }
    }

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
            if (!usacategoria) tmpcategoria = "";
            ret = CommonPage.CreaLinkRoutes(Session, false, Lingua, (testourl), "", CodiceTipologia, tmpcategoria);
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
            if (references.ResMan("Common", Lingua, "testo" + CodiceTipologia) != null)
                htmlPage = references.ResMan("Common", Lingua, "testo" + CodiceTipologia).ToString();
            if (references.ResMan("Common", Lingua, "testo" + Categoria) != null)
                htmlPage = references.ResMan("Common", Lingua, "testo" + Categoria).ToString();

          
#if false  
            string strigaperricerca = "";
            strigaperricerca = "/" + CodiceTipologia + "/" + idOfferta + "/";
            Contenuti content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            if (content == null && !string.IsNullOrEmpty(Categoria))
            {
                strigaperricerca = "/" + CodiceTipologia + "/" + Categoria + "/"; //Request.Url.AbsolutePath
                content = content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            }
            if (content == null && !string.IsNullOrEmpty(titolopagina))
            {
                strigaperricerca = "/" + CodiceTipologia + "/" + CleanUrl(titolopagina); //Request.Url.AbsolutePath
                content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            } 
#endif
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
                htmlPage = content.DescrizionebyLingua(Lingua);
                if (htmlPage.Contains("injectandloadgenericcontent")) JavaInjection = true;

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
            case "rif000002":


                column2.Attributes["class"] = "col-md-9 col-sm-9";
                column3.Attributes["class"] = "col-md-3 col-sm-3";

                divSearch.Visible = true;
                divContact.Visible = false;
                divContactBelow.Visible = false;
                ContaArticoliPerperiodo(CodiceTipologia);
                divLatestPost.Visible = false;
                //AssociaRubricheConsigliati();
                //CaricaUltimiPost(CodiceTipologia, Categoria);
                // CaricaMenuSezioniContenuto(CodiceTipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(2, 3, rptContenutiLink);
                // divCategorie.Visible = true;
                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog 
                // Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6,5);

                if (!JavaInjection)
                {

                    //controlsuggeriti = "injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12,2);";
                    //string controllatest = "injectPortfolioAndLoad(\"isotopeOfferte2.html\",\"divLatestpostContainer\", \"portlats\", 1, 12, false, \"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12);";
                    //cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";


                    sb.Clear();
                    sb.Append("(function wait() {");
                    sb.Append("  if (typeof injectScrollerAndLoad === \"function\")");
                    sb.Append("    {");
                    sb.Append("injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", true, false, 12,2);");
                    sb.Append(" }");
                    sb.Append("   else  {");
                    sb.Append("  setTimeout(wait, 50);");
                    sb.Append("  }  })();");


                    if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                    {
                        //cs.RegisterStartupScript(this.GetType(), "clistlatest", controllatest, true);
                        cs.RegisterStartupScript(this.GetType(), "controlsuggeriti", sb.ToString(), true);
                        //cs.RegisterStartupScript(this.GetType(), "cbandestra1", cbandestra1, true);
                    }


                    //BIND PER LA SCHEDA!!!!
                    //string controlresource1 = "injectandloadgenericcontent(\"schedadetails.html\",\"divItemContainter1\", \"divitem\",true,true, \"" + idOfferta + "\");";

                    sb.Clear();
                    sb.Append("(function wait() {");
                    sb.Append("  if (typeof injectandloadgenericcontent === \"function\")");
                    sb.Append("    {");
                    sb.Append("injectandloadgenericcontent(\"schedadetails.html\",\"divItemContainter1\", \"divitem\",true,true, \"" + idOfferta + "\");");
                    sb.Append(" }");
                    sb.Append("   else  {");
                    sb.Append("  setTimeout(wait, 50);");
                    sb.Append("  }  })();");
                    if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                    {
                        cs.RegisterStartupScript(this.GetType(), "controlresource1", sb.ToString(), true);
                    }
                }


                break;

            case "rif000003":
            case "rif000004":
                column1.Visible = true;
                column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-md-10 col-sm-10";
                column2.Visible = true;
                column3.Visible = false;

                divSearch.Visible = false;
                divContact.Visible = false;
                divLatestPost.Visible = false;
                divCategorie.Visible = false;

                if (!JavaInjection)
                {
                    //controlsuggeriti = "injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", false, true, 12,2);";
                    sb.Clear();
                    sb.Append("(function wait() {");
                    sb.Append("  if (typeof injectScrollerAndLoad === \"function\")");
                    sb.Append("    {");
                    sb.Append("injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", false, true, 12,2);");
                    sb.Append(" }");
                    sb.Append("   else  {");
                    sb.Append("  setTimeout(wait, 50);");
                    sb.Append("  }  })();");

                    if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                    {
                        //cs.RegisterStartupScript(this.GetType(), "clistlatest", controllatest, true);
                        cs.RegisterStartupScript(this.GetType(), "controlsuggeriti", sb.ToString(), true);
                        //cs.RegisterStartupScript(this.GetType(), "cbandestra1", cbandestra1, true);
                    }

#if true

                    //BIND PER LA SCHEDA!!!!
                    //string controlresource2 = "injectandloadgenericcontent(\"schedadetails.html\",\"divItemContainter1\", \"divitem\",false,true, \"" + idOfferta + "\");";
                    sb.Clear();
                    sb.Append("(function wait() {");
                    sb.Append("  if (typeof injectandloadgenericcontent === \"function\")");
                    sb.Append("    {");
                    sb.Append("injectandloadgenericcontent(\"schedadetails.html\",\"divItemContainter1\", \"divitem\",false,true, \"" + idOfferta + "\");");
                    sb.Append(" }");
                    sb.Append("   else  {");
                    sb.Append("  setTimeout(wait, 50);");
                    sb.Append("  }  })();");
                    if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                    {
                        cs.RegisterStartupScript(this.GetType(), "controlresource1", sb.ToString(), true);
                    }
#endif
                    //this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria

                }
                break;
            //case "rif000008":
            //case "rif000009":
            //    //lit = (Literal)Master.FindControl("litPortfolioBanners2");
            //    //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-sezioni", false, lit, Lingua, true);
            //    // ContaArticoliPerperiodo();
            //    divSearch.Visible = false;
            //    divLatestPost.Visible = false;
            //    AssociaRubricheConsigliati();
            //    column1.Visible = true;
            //    column1.Attributes["class"] = "col-md-1 col-sm-1";
            //    column2.Attributes["class"] = "col-md-10 col-sm-10";
            //    column3.Visible = false;
            //    break;
            //case "rif000010":
            //case "rif000011":
            //    // ContaArticoliPerperiodo();
            //    divSearch.Visible = false;
            //    divLatestPost.Visible = false;
            //    AssociaRubricheConsigliati();
            //    column1.Visible = true;
            //    column1.Attributes["class"] = "col-md-1 col-sm-1";
            //    column2.Attributes["class"] = "col-md-10 col-sm-10";
            //    column3.Visible = false;
            //    break;

            case "rif000012":
                break;
            //case "rif000001":
            //case "rif000004":
            //case "rif000005":
            //    divSearch.Visible = false;
            //    divSuggeriti.Visible = true;
            //    AssociaRubricheConsigliati();
            //    divLatestPost.Visible = true;
            //    CaricaUltimiPost(CodiceTipologia, Categoria);

            //    column1.Visible = false;
            //    column2.Attributes["class"] = "col-md-9 col-sm-9";
            //    column3.Attributes["class"] = "col-md-3 col-sm-3";

            //    break;
            //case "rif000002":

            //    divSearch.Visible = false;
            //    divSuggeriti.Visible = false;
            //    divLatestPost.Visible = true;
            //    CaricaUltimiPost(CodiceTipologia, Categoria);

            //    column1.Visible = false;
            //    column2.Attributes["class"] = "col-md-9 col-sm-9";
            //    column3.Attributes["class"] = "col-md-3 col-sm-3";
            //    break;
            case "rif000061":
            case "rif000062":

                column1.Visible = true;
                column1.Attributes["class"] = "col-md-2 col-sm-2";
                column2.Attributes["class"] = "col-md-8 col-sm-8";
                column2.Visible = true;
                column3.Visible = false;

                divSearch.Visible = false;
                divContact.Visible = false;
                divLatestPost.Visible = false;
                this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria

                break;
            default:
                //lit = (Literal)Master.FindControl("litPortfolioBanners1");
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-blog", false, lit, Lingua, true);

                divSearch.Visible = true;
                divLatestPost.Visible = true;
                divContact.Visible = false;
                AssociaRubricheConsigliati();
                CaricaUltimiPost(CodiceTipologia, Categoria);
                ContaArticoliPerperiodo(CodiceTipologia);
                column1.Visible = false;
                column2.Attributes["class"] = "col-md-9 col-sm-9";
                column3.Attributes["class"] = "col-md-3 col-sm-3";
                this.AssociaDati(item); //Visualizzo i dati e aggiorno eventualmente la categoria

                break;
        }

    }

    protected void CaricaUltimiPost(string tipologia, string categoria = "")
    {
        //Filtriamo alcune categorie
        string tipologiadacaricare = tipologia;
        if (string.IsNullOrEmpty(tipologiadacaricare))
            tipologiadacaricare = "rif000001,rif000002,rif000003,rif000004,rif000005,rif000006,rif000007,rif000008,rif000009";

        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
#if true
        if (tipologiadacaricare != "" && tipologiadacaricare != "-")
        {
            SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", tipologiadacaricare);
            parColl.Add(p3);
        }
        if (categoria != "")
        {
            SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", categoria);
            parColl.Add(p7);
        }
        OfferteCollection offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "10", Lingua, false);
        //offerte.RemoveAll(c => (Convert.ToInt32((c.CodiceTipologia.Substring(3))) >= 100)); //Togliamo i risultati del catalogo ( andrebbero tolti nel filtro a monte)
#endif
        //OfferteCollection offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, "6", false, Lingua, false);

        rtpLatestPost.DataSource = offerte;
        rtpLatestPost.DataBind();
    }
    private void ContaArticoliPerperiodo(string cattipo)
    {
        //offerteDM offDM = new offerteDM();
        //Dictionary<string, Dictionary<string, string>> archivioperannomese = offDM.ContaPerAnnoMese(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, Tipologia, Categoria);
        //rptArchivio.DataSource = archivioperannomese;
        //rptArchivio.DataBind();
        divArchivio.Visible = true;
        //string controllist1 = "injectArchivioAndLoad('listaarchivio.html','divArchivioList', 'archivio1','','" + cattipo + "','" + Categoria + "','');";

        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Clear();
        sb.Append("(function wait() {");
        sb.Append("  if (typeof injectArchivioAndLoad === \"function\")");
        sb.Append("    {");
        sb.Append("injectArchivioAndLoad('listaarchivio.html','divArchivioList', 'archivio1','','" + cattipo + "','" + Categoria + "','');");
        sb.Append(" }");
        sb.Append("   else  {");
        sb.Append("  setTimeout(wait, 50);");
        sb.Append("  }  })();");

        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {

            cs.RegisterStartupScript(this.GetType(), "clistarchivio", sb.ToString(), true);

        }

    }

    protected void rptArchivioMesi_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                //Cerco il link e creo
                HtmlAnchor alink = ((HtmlAnchor)(e.Item.FindControl("alink")));

                //anno
                string selanno = "";
                //selanno = ((HiddenField)((RepeaterItem)((Repeater)(sender)).Parent).FindControl("ulAnno")).Value;


                selanno = ((HtmlGenericControl)((Repeater)(sender)).Parent).Attributes["Title"];
                int year = 0;
                int.TryParse(selanno, out year);
                if (year != 0)
                {
                    string link = CreaLinkRicerca("", CodiceTipologia, Categoria, "", "", selanno, ((KeyValuePair<string, string>)e.Item.DataItem).Key, CodiceTipologia, Lingua, Session);
                    alink.HRef = link;
                    DateTime _d = new DateTime(year, Convert.ToInt16(((KeyValuePair<string, string>)e.Item.DataItem).Key), 1);
                    alink.InnerHtml = String.Format("{0:MMM yyyy}", _d) + "    (" + ((KeyValuePair<string, string>)e.Item.DataItem).Value + ")";
                }
            }
        }
    }
    protected string CrealistaFiles(object id, object lista)
    {
        string html = "";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (lista != null && CodiceTipologia != "rif000100")
        {
            //sb.Append(" <ul class=\"list-inline\">");
            //  sb.Append("<div class=\"col-md-4\">");
            foreach (Allegato a in (AllegatiCollection)lista)
            {

                if (!(a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                {
                    string link = PercorsoFiles + "/" + CodiceTipologia + "/" + id.ToString() + "/" + a.NomeFile;
                    link = CommonPage.ReplaceAbsoluteLinks(link);
                    string descrizione = a.Descrizione;
                    if (string.IsNullOrWhiteSpace(descrizione)) descrizione = a.NomeFile;

                    //sb.Append("<li>");
                    //sb.Append("<a class=\"linked\" target=\"_blank\" href=\"" + link + "\"><i class=\"fa  fa-eye  color-green\"></i>" + descrizione + "</a>");
                    //sb.Append("</li>");
                    sb.Append("<a style=\"margin-right:10px;margin-bottom:10px;min-width:190px\" class=\"buttonstyle\" target=\"_blank\" href=\"" + link + "\"><i class=\"fa fa-search\"></i>" + descrizione + "</a>");


                }

            }
            // sb.Append("</div>");
            //sb.Append("</ul>");

        }
        html = sb.ToString();
        return html;
    }
    public string SeparaRows()
    {
        string ritorno = "";
        int elementiperriga = 3;
        if (((progressivosepara) % elementiperriga) == 0)//&& i != totalefoto1)
            ritorno += "</div><div class=\"row\">";
        progressivosepara += 1;
        return ritorno;
    }
    protected void aFacebook_prerender(object sender, EventArgs e)
    {

        // ((HtmlAnchor)sender).Attributes.Add("", "");
    }
    /// <summary>
    /// Articoli consigliati in base alla rubrica
    /// </summary>
    protected void AssociaRubricheConsigliati(bool scrollerview = true)
    {
        OfferteCollection offerte = new OfferteCollection();
        //offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceTipologia, "10", true, Lingua);
        string tipologiadacaricare = CodiceTipologia;
        if (string.IsNullOrEmpty(tipologiadacaricare))
            tipologiadacaricare = "rif000001,rif000002,rif000003,rif000004,rif000005,rif000006,rif000007,rif000008,,rif000009";

        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
#if true
        if (tipologiadacaricare != "" && tipologiadacaricare != "-")
        {
            SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", tipologiadacaricare);
            parColl.Add(p3);
        }
        if (Categoria != "")
        {
            SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", Categoria);
            parColl.Add(p7);
        }
        offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "10", Lingua, false);
#endif
        progressivosepara = 1;

        if (!scrollerview)
        {
            rptArticoliSuggeriti.DataSource = offerte;
            rptArticoliSuggeriti.DataBind();
        }
        else
            Master.CaricaUltimiPostScrollerTipo1(litScrollerSuggeriti, null, "", Lingua, false, true, offerte, "10");



    }
    protected void AssociaRubricheRiferiteaid(string idcollegato)
    {
        OfferteCollection offerte = new OfferteCollection();
        offerte = offDM.CaricaOfferteCollegate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcollegato, "10", true, Lingua);
        rptArticoliSuggeriti.DataSource = offerte;
        rptArticoliSuggeriti.DataBind();
    }
    protected void Cerca_Click(object sender, EventArgs e)
    {
        //string link = CreaLinkRicerca("", CodiceTipologia, Categoria, "", "", "", "", "-", Lingua, Session, true);
        string link = CreaLinkRicerca("", CodiceTipologia, "", "", "", "", "", "-", Lingua, Session, true);
        Session.Add("testoricerca", Server.HtmlEncode(inputCerca.Value)); //carico in sessione il parametro da cercare
        Response.Redirect(link);
    }

    //protected void btnNewsletter_Click(object sender, EventArgs e)
    //{
    //    //Richiesta oroscopo con mail per inserimento in anagrafica clienti !!!!!
    //    //Rimando alla pagina di verifica iscrizione
    //    ClientiDM cliDM = new ClientiDM();
    //    Cliente tmp_Cliente = new Cliente();
    //    tmp_Cliente.Cognome = txtNome.Text;
    //    tmp_Cliente.Email = txtEmail.Text;
    //    //DateTime _d = DateTime.MinValue;
    //    //if (DateTime.TryParse(txtDataNascita.Text, out _d))
    //    //    tmp_Cliente.DataNascita = _d;
    //    Session.Add("iscrivicliente", tmp_Cliente);
    //    string linkverifica = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti.aspx?ID_cliente=&Azione=iscrivinewsletter&Lingua=" + Lingua;
    //    Response.Redirect(linkverifica);
    //}

    protected void AssociaDati(Offerte item)
    {

        //Carichiamo l'immobile a partire dal codice dello stesso e dalla lingua
        OfferteCollection offerte = new OfferteCollection();
#if true
        // item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
        offerte.Add(item);
#endif
#if false
        //if (CodiceTipologia == "rif000100")
        //{

        //    rptSocio.DataSource = offerte;
        //    rptSocio.DataBind();
        //}
        
#endif
        if (CodiceTipologia == "rif000012")
        {
            column1.Visible = false;
            column2.Visible = false;
            column3.Visible = false;
            pnlButtonsnav.Visible = false;
            divGalleryDetail.Visible = true;
            rptOfferteGalleryDetail.DataSource = offerte;
            rptOfferteGalleryDetail.DataBind();
            //CArico anche la lista
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (CodiceTipologia != "" && CodiceTipologia != "-")
            {
                SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", CodiceTipologia);
                parColl.Add(p3);
            }
            List<Offerte> offertelist = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "1000", Lingua, false);
            divContenutiGallery.Visible = true;
            Master.CaricaContenutiPortfolioRivalSubtext(CodiceTipologia, litGalleryDetails, Lingua, "", offertelist, "noborder");

        }
        else
        {
            rptOfferta.DataSource = offerte;
            rptOfferta.DataBind();
        }
        //UserPanel.Update();
    }

    protected string CreaItem(object item)
    {

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (item != null)
        {
            Offerte itemOff = (Offerte)item;

            //CARICAMENTO DATI DB
            string pathimmagine = ComponiUrlAnteprima(itemOff.FotoCollection_M.FotoAnteprima, itemOff.CodiceTipologia, itemOff.Id.ToString(), true);
            pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
            string target = "_self";

            string denominazione = itemOff.DenominazionebyLingua(Lingua);

            string link = CreaLinkRoutes(Session, false, Lingua, CleanUrl(denominazione), itemOff.Id.ToString(), itemOff.CodiceTipologia, itemOff.CodiceCategoria);


            string titolo1 = denominazione;
            string titolo2 = "<br/>";
            int i = denominazione.IndexOf("\n");
            if (i != -1)
            {
                titolo1 = denominazione.Substring(0, i);
                if (denominazione.Length >= i + 1)
                    titolo2 = denominazione.Substring(i + 1);
            }


            //////////////////////////////////////
            sb.Append("<div class=\"thumb-label-item animated seo\" ");
            sb.Append("data-animtype=\"fadeInUp\" ");
            sb.Append("data-animrepeat=\"0\" ");
            sb.Append("data-speed=\"1s\" ");
            sb.Append("data-delay=\"0.6s\" ");
            sb.Append(" > \r\n");

            sb.Append(" <div class=\"img-overlay thumb-label-item-img\"> \r\n");
            sb.Append("	                <a class=\"portfolio-zoom\" target=" + target + " href=\"" + link + "\" title=\"" + denominazione + "\" > \r\n");
            sb.Append("<img  src=\"" + pathimmagine + "\" alt=\"\" /> \r\n");


            //sb.Append("<div class=\"item-img-overlay\"> \r\n");
            ////IN alternativa aperutra galleria pretty photo    <a class="portfolio-zoom fa fa-plus" href="images/placeholders/portfolio1.jpg"  data-rel="prettyPhoto[portfolio]" title="Title goes here"></a>
            //if (denominazione.ToString() != "")
            //{

            //    sb.Append("	 <div class=\"item_img_overlay_content\">\r\n");
            //    sb.Append("  <h3 class=\"thumb-label-item-title\">\r\n");
            //    sb.Append("	    <a target=" + target + " href=\"" + link + "\" title=\"" + denominazione.ToString() + "\"  >" + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(denominazione.ToString()) + "</a>\r\n");
            //    sb.Append("  </h3>\r\n");
            //    sb.Append("	 </div>\r\n ");

            //}
            //sb.Append("</div>\r\n");
            sb.Append("</a>");
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
            sb.Append("</div>\r\n");

            sb.Append("</div>\r\n");
            ///////////////////////////////////////////

        }
        return sb.ToString();
    }
    protected string VisualizzaPosizione(object item)
    {
        string html = "";
        Offerte socio = (Offerte)item;
        //Prendiamo gli indirizzi da visualizzare

        string descrizione = "";
#if false
        Province _p = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice.ToLower() == socio.CodicePROVINCIA1_dts); });
        if (_p != null)
        {
            descrizione = _p.SiglaProvincia;

            html += "<h4 style=\"margin-bottom:3px;padding-bottom:2px\"><strong>" + socio.Nomeposizione1_dts + "</strong></h4> ";
            html += socio.Via1_dts + "<br/> ";
            html += socio.Cap1_dts + " " + socio.CodiceCOMUNE1_dts + " (" + descrizione + ")<br/> ";
            html += references.ResMan("Common",Lingua,"Telefono") + " " + socio.Telefono1_dts + "<br/> ";
        }
        descrizione = "";
        _p = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice.ToLower() == socio.CodicePROVINCIA2_dts); });
        if (_p != null)
        {
            descrizione = _p.SiglaProvincia;
            html += "<h4 style=\"margin-bottom:3px;padding-bottom:2px\"><strong>" + socio.Nomeposizione2_dts + "</strong></h4> ";
            html += socio.Via2_dts + "<br/> ";
            html += socio.Cap2_dts + " " + socio.CodiceCOMUNE2_dts + " (" + descrizione + ")<br/> ";
            html += references.ResMan("Common",Lingua,"Telefono") + " " + socio.Telefono2_dts + "<br/> ";
        }

        descrizione = "";
        _p = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice.ToLower() == socio.CodicePROVINCIA3_dts); });
        if (_p != null)
        {
            descrizione = _p.SiglaProvincia;
            html += "<h4 style=\"margin-bottom:3px;padding-bottom:2px\"><strong>" + socio.Nomeposizione3_dts + "</strong></h4> ";
            html += socio.Via3_dts + "<br/> ";
            html += socio.Cap3_dts + " " + socio.CodiceCOMUNE3_dts + " (" + descrizione + ")<br/> ";
            html += references.ResMan("Common",Lingua,"Telefono") + " " + socio.Telefono3_dts + "<br/> ";
        }
        if (socio.CodiceNAZIONE1_dts.ToLower() != "it")
        {

            html += "<strong>" + socio.Nomeposizione1_dts + "</strong><br/> ";
            html += socio.CodiceNAZIONE1_dts + "<br/> ";
            html += socio.Via1_dts + "<br/> ";
            html += socio.Cap1_dts + " " + socio.CodiceCOMUNE1_dts + " <br/> " + socio.CodicePROVINCIA1_dts + " <br/> " + socio.CodiceREGIONE1_dts + "<br/> ";
            html += references.ResMan("Common",Lingua,"Telefono") + " " + socio.Telefono1_dts + "<br/> ";

        }
        if (socio.CodiceNAZIONE2_dts.ToLower() != "it")
        {

            html += "<strong>" + socio.Nomeposizione2_dts + "</strong><br/> ";
            html += socio.CodiceNAZIONE2_dts + "<br/> ";
            html += socio.Via2_dts + "<br/> ";
            html += socio.Cap2_dts + " " + socio.CodiceCOMUNE2_dts + " <br/> " + socio.CodicePROVINCIA2_dts + " <br/> " + socio.CodiceREGIONE2_dts + "<br/> ";
            html += references.ResMan("Common",Lingua,"Telefono") + " " + socio.Telefono2_dts + "<br/> ";

        }
        if (socio.CodiceNAZIONE3_dts.ToLower() != "it")
        {

            html += "<strong>" + socio.Nomeposizione3_dts + "</strong><br/> ";
            html += socio.CodiceNAZIONE3_dts + "<br/> ";
            html += socio.Via3_dts + "<br/> ";
            html += socio.Cap3_dts + " " + socio.CodiceCOMUNE3_dts + " <br/> " + socio.CodicePROVINCIA3_dts + " <br/> " + socio.CodiceREGIONE3_dts + "<br/> ";
            html += references.ResMan("Common",Lingua,"Telefono") + " " + socio.Telefono3_dts + "<br/> ";

        } 
#endif

        Province _p = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice.ToLower() == socio.CodicePROVINCIA1_dts); });
        if (_p != null)
        {
            descrizione = _p.SiglaProvincia;
            html += socio.CodiceCOMUNE1_dts + "- " + _p.Provincia + " - " + _p.Regione;
            html += " (" + socio.CodiceNAZIONE1_dts + ") <br/> ";

        }

        return html;
    }
    protected string ImpostaTestoRichiesta()
    {
        string ret = "";
        if (CodiceTipologia == "rif000008" || CodiceTipologia == "rif000009")
            ret = references.ResMan("Common", Lingua, "TestoPrenota");
        else
            //    ret = references.ResMan("Common",Lingua,"TestoRichiedi;
            ////else if ( CodiceTipologia == "rif000001" || CodiceTipologia == "rif000004")
            ////    ret = references.ResMan("Common",Lingua,"TestoDisponibilita;
            //else
            ret = references.ResMan("Common", Lingua, "TestoDisponibilita");


        return ret;
    }
    protected bool VerificaPresenzaPrezzo(object prezzo)
    {
        bool ret = false;
        if (prezzo != null && (double)prezzo != 0)
            ret = true;
        return ret;
    }
    protected bool AttivaContatto(object flag)
    {
        bool ret = false;
        if (flag != null)
        {
            ret = ((bool)flag);
        }
        return ret;
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
        string  host = System.Web.HttpContext.Current.Request.Url.Host.ToString();
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
            ((HtmlMeta)Master.FindControl("metafbimage")).Content = ComponiUrlAnteprima(data.FotoCollection_M.FotoAnteprima, data.CodiceTipologia, data.Id.ToString(), true).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
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

        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));

        string hreflang = "";
        //METTIAMO GLI ALTERNATE
        hreflang = " hreflang=\"it\" ";
        string linkcanonicoalt = CreaLinkRoutes(null, false, "I", CleanUrl(data.DenominazioneI), data.Id.ToString(), data.CodiceTipologia);
        linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
            linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit"));

        Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
        litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" +  (linkcanonicoalt) + "\"/>";
        Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
        litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" +  (linkcanonicoalt) + "\"/>";
        if (Lingua == "I")
        {
            litcanonic.Text = "<link rel=\"canonical\"  href=\"" +  (linkcanonicoalt) + "\"/>";
            actualpagelink.Campo1 =  (linkcanonicoalt);
            actualpagelink.Campo2 = (data.DenominazioneI);
        }
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            linkcanonicoalt = CreaLinkRoutes(null, false, "GB", CleanUrl(data.DenominazioneGB), data.Id.ToString(), data.CodiceTipologia);
            linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen"));

            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" +  (linkcanonicoalt) + "\"/>";
            if (Lingua == "GB")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" +  (linkcanonicoalt) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 =  (linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(data.DenominazioneGB);
            }
        }

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
        {
            hreflang = " hreflang=\"ru\" ";
            linkcanonicoalt = CreaLinkRoutes(null, false, "RU", CleanUrl(data.DenominazioneRU), data.Id.ToString(), data.CodiceTipologia);
            linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru"));

            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + (linkcanonicoalt) + "\"/>";
            if (Lingua == "RU")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (linkcanonicoalt) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = (linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(data.DenominazioneRU);
            }
        }
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
            if (false) //Pagina copertina presente
            {
                Prodotto catcopertina = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == CodiceTipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
                if (catcopertina != null && !string.IsNullOrEmpty((catcopertina.Descrizione.ToLower().Trim())))
                {
                    Contenuti contentpercategoria = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, catcopertina.Descrizione.ToLower().Trim());
                    if (contentpercategoria != null && contentpercategoria.Id != 0)
                    {
                        Tabrif laddink = new Tabrif();
                        laddink.Campo1 = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(contentpercategoria.TitolobyLingua(Lingua)), contentpercategoria.Id.ToString(), "con001000");
                        laddink.Campo2 = contentpercategoria.TitolobyLingua(Lingua);
                        links.Add(laddink);
                    }
                }
            }

            links.Add(link);
        }
        return links;
    }

    protected void rptOfferta_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                //Offerte data = (Offerte)(e.Item.DataItem);
                //HtmlGenericControl divSocial = (HtmlGenericControl)e.Item.FindControl("divSocial");
                //divSocial.Attributes.Add("addthis:url", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/SchedaContenutoMaster.aspx?idOfferta=" + data.Id + "&CodiceTipologia=" + data.CodiceTipologia + "&Lingua=" + Lingua);
                //divSocial.Attributes.Add("addthis:title", WelcomeLibrary.UF.Utility.SostituisciTestoACapo(data.DenominazioneI));
                //divSocial.Attributes.Add("addthis:description", CommonPage.ReplaceLinks(ConteggioCaratteri(data.DescrizioneI, 90)));

                ////Titolo e descrizione pagina
                //((HtmlTitle)Master.FindControl("metaTitle")).Text = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(data.DenominazioneI);
                //((HtmlMeta)Master.FindControl("metaDesc")).Content = CommonPage.ReplaceLinks(ConteggioCaratteri(data.DescrizioneI, 90));
                ////Opengraph per facebook
                //((HtmlMeta)Master.FindControl("metafbTitle")).Content = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(data.DenominazioneI);
                //((HtmlMeta)Master.FindControl("metafbdescription")).Content = CommonPage.ReplaceLinks(ConteggioCaratteri(data.DescrizioneI, 90));

                //((HtmlMeta)Master.FindControl("metafbimage")).Content = ComponiUrlAnteprima(data.FotoCollection_M.FotoAnteprima, data.CodiceTipologia, data.Id.ToString()).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            }
        }
    }
    protected void ImgSmall_PreRender(object sender, EventArgs e)
    {
        int maxwidth = 90;
        int maxheight = 200;
        try
        {
#if true
            //Meglio testare prma se l'immagine esiste invece di fare try catch
            if (File.Exists(Server.MapPath(((Image)sender).ImageUrl)))
            {
                using (System.Drawing.Image tmp = System.Drawing.Image.FromFile(Server.MapPath(((Image)sender).ImageUrl)))
                {
                    ((Image)sender).Width = maxwidth;
                    int altezza = tmp.Height * maxwidth / tmp.Width;
                }
            }
            else
            {//File inesistente
                ((Image)sender).Width = maxwidth;
                ((Image)sender).Height = maxheight;
            }
#endif

        }
        catch
        { }
    }

    protected void imgGal_PreRender(object sender, EventArgs e)
    {
        //70x50
        try
        {
            //Meglio testare prma se l'immagine esiste invece di fare try catch
            if (File.Exists(Server.MapPath(((Image)sender).ImageUrl)))
            {
                System.Drawing.Image tmp = System.Drawing.Image.FromFile(Server.MapPath(((Image)sender).ImageUrl));

                ((Image)sender).Width = 62;
                ((Image)sender).Height = tmp.Height * 62 / tmp.Width;
                //(68/He = tw/th)
                //if (tmp.Width >= tmp.Height)
                //{
                //    ((Image)sender).Height = 60;
                //    ((Image)sender).Width = tmp.Width * 60 / tmp.Height;
                //}
                //else
                //{
                //    ((Image)sender).Height = 60;
                //    ((Image)sender).Width = tmp.Width * 60 / tmp.Height;
                //}
                tmp.Dispose();
            }
            else
            {//File inesistente
                ((Image)sender).Width = 62;
                ((Image)sender).Height = 62;
            }
        }
        catch
        { }
    }
    protected void ImgAnt_PreRender(object sender, EventArgs e)
    {
        int maxwidth = 500;
        int maxheight = 350;
        try
        {
#if true
            //Meglio testare prma se l'immagine esiste invece di fare try catch
            if (File.Exists(Server.MapPath(((Image)sender).ImageUrl)))
            {
                using (System.Drawing.Image tmp = System.Drawing.Image.FromFile(Server.MapPath(((Image)sender).ImageUrl)))
                {
                    if (tmp.Width >= tmp.Height)
                    {
                        ((Image)sender).Width = maxwidth;
                        int altezza = tmp.Height * maxwidth / tmp.Width;

                        if (altezza < maxheight)
                            ((Image)sender).Height = altezza;
                        else
                        {
                            //((HtmlGenericControl)(((Image)sender).Parent)).Attributes["style"] = "height:" + maxheight + "px;overflow: hidden; float: left; margin: 5px 5px 5px 0px";
                            ((Image)sender).Height = maxheight;
                            ((Image)sender).Width = tmp.Width * maxheight / tmp.Height;
                        }
                    }
                    else
                    {
                        ((Image)sender).Height = maxheight;
                        int larghezza = tmp.Width * maxheight / tmp.Height;
                        if (larghezza < maxwidth)
                            ((Image)sender).Width = larghezza;
                        else
                        {
                            ((Image)sender).Width = maxwidth;
                            ((Image)sender).Height = tmp.Height * maxwidth / tmp.Width;
                        }
                    }
                }
            }
            else
            {//File inesistente
                ((Image)sender).Width = maxwidth;
                ((Image)sender).Height = maxheight;
            }
#endif

        }
        catch
        { }


    }

    protected string CalcolaRapp(Object NomeAnteprima, string CodiceTipologia, string idOfferta)
    {
        string ritorno = PercorsoFiles;
        if (NomeAnteprima != null)
        {
            if (NomeAnteprima.ToString().StartsWith("Ant"))
                ritorno += "/" + CodiceTipologia.ToString() + "/" + idOfferta + "/" + NomeAnteprima.ToString().Remove(0, 3);
            else
                ritorno += "/" + CodiceTipologia.ToString() + "/" + idOfferta + "/" + NomeAnteprima.ToString();
        }
        //ritorno = ritorno.Replace("~", "http://" + HttpContext.Current.Request.Url.Host);

        //cALCOLIAMO ANCHE IL RAPPORTO D'ASPETTO DELL'IMMAGINE PER LA VISUALIZZAZIONE CORRETTA COL
        //JAVASCRIPT
        //USA LA SEGUENE
        try
        {
            System.Drawing.Image tmp = System.Drawing.Image.FromFile(Server.MapPath(ritorno));
            double rappwidthheight = ((double)tmp.Width / (double)tmp.Height);
            tmp.Dispose();
            //-> devi passare questo rapporto al javascript per il ricalcolo delle dimensioni 
            //dell'imagine ---> 
            ritorno = rappwidthheight.ToString();
        }
        catch
        {
            ritorno = "1";
        }
        return ritorno;
    }

    protected bool ControlloVisibilitaMiniature(object fotos)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 1) ret = false;
        return ret;
    }

    protected string CreaSlideNavigation(object itemobj, int maxwidth, int maxheight)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Offerte item = ((Offerte)itemobj);
        if ((item != null) && (item.FotoCollection_M.Count > 0))
        {

            foreach (Allegato a in item.FotoCollection_M)
            {
                string testotitolo = item.DenominazionebyLingua(Lingua);


                //IMMAGINE
                string pathimmagine = ComponiUrlAnteprima(a.NomeFile, item.CodiceTipologia, item.Id.ToString());
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //LINK
                string virtuallink = ComponiUrlAnteprima(a.NomeFile, item.CodiceTipologia, item.Id.ToString());
                string link = virtuallink.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1)
                {
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }

                sb.Append(" <li>\r\n");
                if (!ControlloVideo(item.FotoCollection_M.FotoAnteprima))
                {
                    sb.Append("	  <img style=\"padding:5px");
                    //if (maxwidth > 0)
                    //    sb.Append("max-width:" + maxwidth + "px;");
                    //else
                    //    sb.Append("width:auto;");
                    //if (maxheight > 0)
                    //    sb.Append("max-height:" + maxheight + "px;");
                    //else
                    //    sb.Append("height:auto;");
                    sb.Append("\"  src=\"" + pathimmagine + "\" alt=\"" + testotitolo + "\" />\r\n");
                }
                sb.Append(" </li>\r\n");

            }
        }

        return sb.ToString();
    }
    protected bool ControlloVisibilita(object fotos, object linkvideo)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 0) ret = false;
        bool onlypdf = (fotos != null && ((AllegatiCollection)fotos).Count > 0 && !((AllegatiCollection)fotos).Exists(c => (c.NomeFile.ToString().ToLower().EndsWith("jpg") || c.NomeFile.ToString().ToLower().EndsWith("gif") || c.NomeFile.ToString().ToLower().EndsWith("png"))));
        if (onlypdf) ret = false;
        if (!string.IsNullOrWhiteSpace(linkvideo.ToString())) ret = false;

        return ret;
    }

    protected string CreaSlide(object itemobj, int maxwidth, int maxheight)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Offerte item = ((Offerte)itemobj);
        if ((item != null) && (item.FotoCollection_M.Count > 0))
        {
            foreach (Allegato a in item.FotoCollection_M)
            {
                string testotitolo = "";
                if (!(a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                    continue;

                testotitolo = item.DenominazionebyLingua(Lingua);
                //IMMAGINE
                string pathimmagine = ComponiUrlAnteprima(a.NomeFile, item.CodiceTipologia, item.Id.ToString(), true);
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                //LINK
                string target = "_blank";
                string virtuallink = ComponiUrlAnteprima(a.NomeFile, item.CodiceTipologia, item.Id.ToString(), true);
                string link = virtuallink.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && !string.IsNullOrEmpty(link))
                {
                    target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }


                sb.Append("<div class=\"slide\" ");
                if (item.FotoCollection_M.Count > 1)
                    sb.Append(" data-thumb=\"" + pathimmagine + "\" ");
                sb.Append(" >\r\n");
                sb.Append("    <div class=\"slide-content\" style=\"position:relative;padding:1px\">\r\n");

                if (!ControlloVideo(item.FotoCollection_M.FotoAnteprima, item.linkVideo))
                {
                    #region FOTO
                    //if (!string.IsNullOrEmpty(link))
                    //    sb.Append("	       <a href=\"" + link + "\" target=\"" + target + "\" title=\"" + testotitolo + "\">\r\n");
                    sb.Append("	           <img itemprop=\"image\"  style=\"");
#if true
                    string imgdimstyle = "";
                    try
                    {
                        if (maxheight != 0)
                            using (System.Drawing.Image tmp = System.Drawing.Image.FromFile(Server.MapPath(virtuallink)))
                            {
                                if (tmp.Width <= tmp.Height)
                                {
                                    imgdimstyle = "width:auto;height:" + maxheight + "px;";
                                }
                            }
                    }
                    catch
                    { }
                    if (imgdimstyle == "")
                    {
                        sb.Append("max-width:100%;");
                        sb.Append("height:auto;");
                    }
                    else
                        sb.Append(imgdimstyle);
#endif
                    sb.Append("border:none\" src=\"" + pathimmagine + "\" alt=\"" + testotitolo + "\" />\r\n");

                    //if (!string.IsNullOrEmpty(link))
                    //    sb.Append("	       </a>\r\n");

                    //aggiungiamo i messaggi sopra
                    if (!string.IsNullOrEmpty(a.Descrizione))
                    {
                        sb.Append("<div   class=\"divbuttonstyle\"  style=\"position:absolute;left:30px;bottom:30px;padding:10px;text-align:left;color:#ffffff;\">");
                        sb.Append("	       <a style=\"color:#ffffff\" href=\"" + link + "\" target=\"" + target + "\" title=\"" + testotitolo + "\">\r\n");
                        sb.Append(" " + a.Descrizione);
                        sb.Append("	       </a>\r\n");
                        sb.Append("	       </div>\r\n");
                    }
                    #endregion
                }

                sb.Append("    </div>\r\n");
                sb.Append("</div>\r\n");
            }
        }


        return sb.ToString();
    }

    public void CaricaMenuSezioniContenuto(string tipologia, Repeater rptlist)
    {
        List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == tipologia)); });
        //prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));

        rptlist.DataSource = prodotti;
        rptlist.DataBind();
    }

    private void CaricaMenuContenuti(int min, int max, Repeater rptlist)
    {
        List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua); });
        sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < min || Convert.ToInt32(t.Codice.Substring(3)) > max);

        rptlist.DataSource = sezioni;
        rptlist.DataBind();
    }

    protected string estraititolo(object testotitolo)
    {
        if (testotitolo == null) return "";
        string titolo1 = testotitolo.ToString();
        string titolo2 = "";
        int i = testotitolo.ToString().IndexOf("\n");
        if (i != -1)
        {
            titolo1 = testotitolo.ToString().Substring(0, i);
            if (testotitolo.ToString().Length >= i + 1)
                titolo2 = testotitolo.ToString().Substring(i + 1);
        }
        return titolo1;
    }
    protected string estraisottotitolo(object testotitolo)
    {
        if (testotitolo == null) return "";
        string titolo1 = testotitolo.ToString();
        string titolo2 = "<br/>";
        int i = testotitolo.ToString().IndexOf("\n");
        if (i != -1)
        {
            titolo1 = testotitolo.ToString().Substring(0, i);
            if (testotitolo.ToString().Length >= i + 1)
                titolo2 = testotitolo.ToString().Substring(i + 1);
        }
        return titolo2;
    }
    protected bool ControllaVisibilitaValore(string valore)
    {
        bool ret = true;
        if (string.IsNullOrEmpty(valore.Trim())) ret = false;
        if (valore.Trim() == "0") ret = false;

        return ret;
    }
    protected bool ControllaVisibilitaPerCodice(string codicetipologia)
    {
        bool ret = false;
        if (codicetipologia == "rif000002") ret = true;
        return ret;
    }
    protected string ImpostaIntroPrezzo(string codicetipologia)
    {
        string ret = "";
        //  if (codicetipologia == "rif000002") ret = references.ResMan("Common",Lingua,"TitoloPrezzoApartire");
        //    if (codicetipologia == "rif000001") ret = references.ResMan("Common",Lingua,"TitoloPrezzo") + " ";

        return ret;
    }


    protected bool ControlloVideo(object NomeAnteprima, object linkVideo)
    {
        bool ret = false;
        //"http://www.youtube.com/embed/Z9lwY9arkj8"
        if ((linkVideo != null && ((string)linkVideo) != ""))
            ret = true;
        return ret;
    }

    protected string SorgenteVideo(object linkVideo)
    {
        string ret = "";
        //"http://www.youtube.com/embed/Z9lwY9arkj8"

        if (linkVideo != null && ((string)linkVideo) != "")
            ret = linkVideo.ToString();
        return ret;
    }

    protected string ControlloVuotoEmail(string pre, string contenuto, object flag)
    {
        string ret = "";
        if (!string.IsNullOrWhiteSpace(contenuto))
            ret = pre + contenuto;
        bool abilitacontatto = false;
        if (flag != null)
        {
            abilitacontatto = ((bool)flag);
        }
        if (abilitacontatto) ret = "";
        return ret;
    }
    protected string ControlloVuoto(string pre, string contenuto)
    {
        string ret = "";

        if (!string.IsNullOrWhiteSpace(contenuto))
            ret = pre + contenuto;

        return ret;

    }
    protected string TipoOfferta()
    {
        string ritorno = "";
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
            ritorno = "<br/><span style=\"margin-left:100px;padding:2px 10px 2px 10px;font-size:11pt;font-style:italic;color: #fff;background-color:#cc67fe\">" + item.Descrizione + "</span>";
        return ritorno;
    }
    protected string TestoSezione(string codicetipologia, bool solotitolo = false, bool nosezione = false)
    {
        string ret = "";
        WelcomeLibrary.DOM.TipologiaOfferte sezione =
              WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codicetipologia); });
        if (sezione != null)
        {
            string addtext = " " + references.ResMan("Common", Lingua, "testoSezione").ToString();
            if (nosezione) addtext = "";
            ret += addtext + CommonPage.ReplaceAbsoluteLinks(CommonPage.CrealinkElencotipologia(codicetipologia, Lingua, Session));

            if (solotitolo)
                ret = sezione.Descrizione;
        }

        return ret;
    }
    protected string ControlloVuotoPosizione(string comune, string codiceprovincia)
    {
        string ret = "";

        if (!string.IsNullOrWhiteSpace(comune))
            ret += comune;
        if (!string.IsNullOrWhiteSpace(codiceprovincia))
            ret += " (" + NomeProvincia(codiceprovincia, Lingua) + ") ";

        return ret;
    }

    protected string NomeProvincia(string codiceprovincia)
    {
        string ritorno = "";
        Province item = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codiceprovincia); });
        if (item != null)
            ritorno = item.Provincia;
        return ritorno;
    }
    protected bool VerificaPresenzaFoto(object foto)
    {
        bool ret = false;
        if (foto != null && ((List<Allegato>)foto).Count > 1)
        {
            ret = true;
        }
        return ret;

    }
    protected void btnRegistrastatistiche_Click(object sender, EventArgs e)
    {
        string id = (((Button)sender).CommandArgument);
        Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
        int _i = 0;
        int.TryParse(id, out _i);
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


    protected void btnContatti_Click(object sender, EventArgs e)
    {
        try
        {
            //Prepariamo e inviamo il mail
            string nomemittente = txtContactName.Value;
            string cognomemittente = txtContactCognome.Value;
            string mittenteMail = txtContactEmail.Value;
            string telefono = txtContactPhone.Value;
            string nomedestinatario = Nome;
            string maildestinatario = Email;
            int idperstatistiche = 0;
            string tipo = "informazioni";
            string SoggettoMail = "Richiesta " + tipo + " da " + cognomemittente + "  " + nomemittente + " tramite il sito " + Nome;
            string Descrizione = txtContactMessage.Value.Replace("\r", "<br/>") + " <br/> ";

            Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
            if (item != null && item.Id != 0)
            {
                idperstatistiche = item.Id;
                Descrizione = Descrizione.Insert(0, item.DenominazioneI.Replace("\r", "<br/>") + " <br/> ");
            }
            Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome o rag soc. Cliente: " + cognomemittente;
            Descrizione += " <br/> Telefono Cliente:  " + telefono + "   Email Cliente: " + mittenteMail + " Lingua Cliente: " + Lingua;
            Descrizione += " <br/> Confermo l'autorizzazione al trattamento dei miei dati personali (D.Lgs 196/2003)";

            if (chkContactPrivacy.Checked)
            {
                Utility.invioMailGenerico(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

                //Registro la statistica di contatto
                Statistiche stat = new Statistiche();
                stat.Data = DateTime.Now;
                stat.EmailDestinatario = maildestinatario;
                stat.EmailMittente = mittenteMail;
                stat.Idattivita = idperstatistiche;
                stat.Testomail = cognomemittente + " " + nomemittente + "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                stat.Url = "";
                statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);


                Response.Redirect(CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkContatti")) + "&idOfferta=" + idOfferta.ToString() + "&conversione=true");

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

    protected void btnContatti1_Click(object sender, EventArgs e)
    {
        try
        {
            //Prepariamo e inviamo il mail
            string nomemittente = txtContactName1.Value;
            string cognomemittente = txtContactCognome1.Value;
            string mittenteMail = txtContactEmail1.Value;
            string telefono = txtContactPhone1.Value;
            string nomedestinatario = Nome;
            string maildestinatario = Email;
            int idperstatistiche = 0;
            string tipo = "informazioni";
            string SoggettoMail = "Richiesta " + tipo + " da " + cognomemittente + "  " + nomemittente + " tramite il sito " + Nome;
            string Descrizione = txtContactMessage1.Value.Replace("\r", "<br/>") + " <br/> ";

            Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
            if (item != null && item.Id != 0)
            {
                idperstatistiche = item.Id;
                Descrizione = Descrizione.Insert(0, item.DenominazioneI.Replace("\r", "<br/>") + " <br/> ");
            }
            Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome o rag soc. Cliente: " + cognomemittente;
            Descrizione += " <br/> Telefono Cliente:  " + telefono + "   Email Cliente: " + mittenteMail + " Lingua Cliente: " + Lingua;
            Descrizione += " <br/> Confermo l'autorizzazione al trattamento dei miei dati personali (D.Lgs 196/2003)";

            if (chkContactPrivacy1.Checked)
            {
                Utility.invioMailGenerico(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

                //Registro la statistica di contatto
                Statistiche stat = new Statistiche();
                stat.Data = DateTime.Now;
                stat.EmailDestinatario = maildestinatario;
                stat.EmailMittente = mittenteMail;
                stat.Idattivita = idperstatistiche;
                stat.Testomail = cognomemittente + " " + nomemittente + "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                stat.Url = "";
                statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);


                Response.Redirect(CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkContatti")) + "&idOfferta=" + idOfferta.ToString() + "&conversione=true");

            }
            else
            {


                outputContact1.Text = references.ResMan("Common", Lingua, "txtPrivacyError");
                //Mittente.Descrizione += " <br/> Non vi Autorizzo al trattamento dei miei dati personali (D.Lgs 196/2003)";
            }

        }
        catch (Exception err)
        {
            outputContact1.Text = err.Message + " <br/> ";
            outputContact1.Text += references.ResMan("Common", Lingua, "txtMailError");
        }
    }
}