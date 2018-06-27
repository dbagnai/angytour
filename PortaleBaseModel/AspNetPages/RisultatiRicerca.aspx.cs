using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Data.SQLite;

public partial class AspNetPages_RisultatiRicerca : CommonPage
{
    public string PageGuid
    {
        get { return ViewState["PageGuid"] != null ? (string)(ViewState["PageGuid"]) : ""; }
        set { ViewState["PageGuid"] = value; }
    }

    public string Pagina
    {
        get { return Session["Pagina"] != null ? (string)(Session["Pagina"]) : "1"; }
        set { Session["Pagina"] = value; }
    }

    public DataTable dt
    {
        get { return ViewState["DataTable"] != null ? (DataTable)(ViewState["DataTable"]) : new DataTable(); }
        set { ViewState["DataTable"] = value; }
    }

    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }

    public string Tipologia
    {
        get { return ViewState["Tipologia"] != null ? (string)(ViewState["Tipologia"]) : ""; }
        set { ViewState["Tipologia"] = value; }
    }

    public string Provincia
    {
        get { return ViewState["Provincia"] != null ? (string)(ViewState["Provincia"]) : ""; }
        set { ViewState["Provincia"] = value; }
    }

    public string Regione
    {
        get { return ViewState["Regione"] != null ? (string)(ViewState["Regione"]) : ""; }
        set { ViewState["Regione"] = value; }
    }

    public string FasciaPrezzo
    {
        get { return ViewState["FasciaPrezzo"] != null ? (string)(ViewState["FasciaPrezzo"]) : ""; }
        set { ViewState["FasciaPrezzo"] = value; }
    }

    public string Mesefiltro
    {
        get { return ViewState["Mesefiltro"] != null ? (string)(ViewState["Mesefiltro"]) : ""; }
        set { ViewState["Mesefiltro"] = value; }
    }

    public string Giornofiltro
    {
        get { return ViewState["Giornofiltro"] != null ? (string)(ViewState["Giornofiltro"]) : ""; }
        set { ViewState["Giornofiltro"] = value; }
    }

    public string Comune
    {
        get { return ViewState["Comune"] != null ? (string)(ViewState["Comune"]) : ""; }
        set { ViewState["Comune"] = value; }
    }

    public string Categoria2liv
    {
        get { return ViewState["Categoria2liv"] != null ? (string)(ViewState["Categoria2liv"]) : ""; }
        set { ViewState["Categoria2liv"] = value; }
    }

    public string testoricerca
    {
        get { return ViewState["testoricerca"] != null ? (string)(ViewState["testoricerca"]) : ""; }
        set { ViewState["testoricerca"] = value; }
    }

    public string mese
    {
        get { return ViewState["mese"] != null ? (string)(ViewState["mese"]) : ""; }
        set { ViewState["mese"] = value; }
    }

    public string anno
    {
        get { return ViewState["anno"] != null ? (string)(ViewState["anno"]) : ""; }
        set { ViewState["anno"] = value; }
    }

    public string Categoria
    {
        get { return ViewState["Categoria"] != null ? (string)(ViewState["Categoria"]) : ""; }
        set { ViewState["Categoria"] = value; }
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

    //Vediamo l'agenzia del''immobile
    //AgenziaCollection agenziegestite_pagina = new AgenziaCollection();
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //bannersDM banDM = new bannersDM();
        //dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb,Lingua);
        //AdRotator1.DataSource = dt;
        //AdRotator1.DataBind();
    }

    public bool JavaInjection = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //http://localhost:8888/test/articoli/rif000002-I-testoperindicizzazione.aspx

                PageGuid = System.Guid.NewGuid().ToString();

                //Creo l'equivalente di ~/ nel ViewState per usarlo nel javascript della pagina
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;

                //Prendiamo i dati dalla querystring (Lingua) o dal Context ( caso di url rewriting )
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                Regione = CaricaValoreMaster(Request, Session, "Regione", false);
                Provincia = CaricaValoreMaster(Request, Session, "Provincia", false, "");
                Comune = CaricaValoreMaster(Request, Session, "Comune", false);
                Categoria = CaricaValoreMaster(Request, Session, "Categoria", false);

                FasciaPrezzo = CaricaValoreMaster(Request, Session, "FasciaPrezzo", false);
                Giornofiltro = CaricaValoreMaster(Request, Session, "Giornofiltro", false);
                Categoria2liv = CaricaValoreMaster(Request, Session, "Categoria2liv", false);
                if (Categoria2liv == "all") Categoria2liv = "";

                Mesefiltro = CaricaValoreMaster(Request, Session, "Mesefiltro", false);

                //Aggiungo la lingua al pager se presente nella querystring
                //PagerRisultati.NavigateUrl += "?Lingua=" + Lingua;
                //Impostiamo anche gli altri parametri di ricerca
                Tipologia = CaricaValoreMaster(Request, Session, "Tipologia", false, "");
                string tmp = "";
                tmp = CaricaValoreMaster(Request, Session, "mese", false);
                if (!string.IsNullOrEmpty(tmp)) mese = tmp;
                tmp = CaricaValoreMaster(Request, Session, "anno", false);
                if (!string.IsNullOrEmpty(tmp)) anno = tmp;
                tmp = CaricaValoreMaster(Request, Session, "testoricerca", false);
                if (!string.IsNullOrEmpty(tmp)) testoricerca = tmp;

                #region SEZIONE MASTERPAGE GESTIONE

                //string sectionforbanner = Tipologia;
                //if (!string.IsNullOrEmpty(Categoria))
                //    sectionforbanner += "-" + Categoria;
                //if (!string.IsNullOrEmpty(Tipologia))
                //    Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, sectionforbanner, false, Lingua);
                //else
                //    Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);

                //HtmlGenericControl divshow = (HtmlGenericControl)Master.FindControl("divSfondo1");
                //divshow.Visible = true;
                //Literal lit = (Literal)Master.FindControl("litPortfolioBanners2");
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-sezioni", false, lit, Lingua);

                #endregion SEZIONE MASTERPAGE GESTIONE

                //In caso di richiesta specifica di una pagina dei risultati la prendo dalla querystring
                //Pagina = CaricaValoreMaster(Request, Session, "Pagina", true, "1");
                //Pagina = "1";
                //int _p = 0;
                //if (int.TryParse(Pagina, out _p))
                //{ PagerRisultati.CurrentPage = _p; }
                // CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista link del blog
                LoadJavascriptVariables();
                SettaVisualizzazione();
                // CaricaControlliJS();

                DataBind();
            }
            else
            {
            }
        }
        catch (Exception err)
        {
            // output.Text = err.Message;
        }
    }

    private void LoadJavascriptVariables()
    {
        ClientScriptManager cs = Page.ClientScript;

        String scriptRegVariables = string.Format("var testoricerca = '" + Server.HtmlEncode(testoricerca) + "';");

        if (!cs.IsClientScriptBlockRegistered("RegVariablesScriptPage"))
        {
            cs.RegisterClientScriptBlock(typeof(Page), "RegVariablesScriptPage", scriptRegVariables, true);
        }
    }

    public void CaricaControlliJS()
    {
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //Carico la galleria in masterpage corretta
        string controllistBanHead = "";
        string sectionforbanner = Tipologia;
        if (!string.IsNullOrEmpty(Categoria))
            sectionforbanner += "-" + Categoria;
        if (string.IsNullOrEmpty(Tipologia))
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

    private void SettaVisualizzazione()
    {
        string cattipo = Tipologia;
        ClientScriptManager cs = Page.ClientScript;
        Literal lit = null;
        ModificaFiltroJS();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        switch (Tipologia)
        {
            case "rif000001":
                if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                AssociaDatiSocial();

                column1.Visible = true;
                column1.Attributes["class"] = "col-sm-12";
                column2.Visible = false;
                column3.Visible = false;
                //ContaArticoliPerperiodo(Tipologia);
                divSearch.Visible = false;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;

                //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                //placeholderrisultati
                sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                sb.Append("injectPortfolioAndLoad,isotopeProdotti1.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                sb.Append("\"></div>");
                sb.Append("<div id=\"divPortfolioListPager\"></div>");
                placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();

                break;

            case "rif000002":
                AssociaDatiSocial();

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-12 col-sm-3";
                column3.Visible = false;
                ContaArticoliPerperiodo(Tipologia);
                //  Caricalinksrubriche(Tipologia); //arica la ddl con le sttocategorie
                divSearch.Visible = true;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;
                //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
                //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog2.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //placeholderrisultati.Text = sb.ToString();

                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog3.html,divPortfolioList,portlist1, 1, 9,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',true,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                    sb.Append("\"></div>");
                   sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();

                    //sb.Clear();
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);
                }

                break;

            //case "rif000005":
            //    AssociaDatiSocial();

            //    column1.Visible = true;
            //    column1.Attributes["class"] = "col-12";
            //    column2.Visible = false;
            //    //column2.Attributes["class"] = "col-md-1 col-sm-1";
            //    column3.Attributes["class"] = "col-12 col-sm-3";
            //    column3.Visible = false;
            //    ContaArticoliPerperiodo(Tipologia);
            //    //  Caricalinksrubriche(Tipologia); //arica la ddl con le sttocategorie
            //    divSearch.Visible = true;
            //    divLatestPost.Visible = false;
            //    //CaricaUltimiPost(Tipologia, Categoria);
            //    //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
            //    //CaricaMenuContenuti(4, 9, rptContenutiLink);
            //    divCategorie.Visible = false;
            //    //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
            //    //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
            //    //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);
            //    if (!JavaInjection)
            //    {
            //        if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

            //        //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
            //        //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog2.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
            //        //sb.Append("\"></div>");
            //        //sb.Append("<div id=\"divPortfolioListPager\"></div>");
            //        //placeholderrisultati.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);

            //        sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
            //        sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog-no-image.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'\'");
            //        sb.Append("\"></div>");
            //        sb.Append("<div id=\"divPortfolioListPager\"></div>");
            //        placeholderrisultati.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);

            //        //sb.Clear();
            //        //sb.Append("<div class=\"sfondo-contenitore\">");
            //        //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
            //        //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
            //        //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
            //        //sb.Append("\"></div>");
            //        //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
            //        //sb.Append("</div>");
            //        //placeholderlateral.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);

            //    }

            //    break;

            //case "rif000003":
            //case "rif000004":

            //    AssociaDatiSocial();

            //    column1.Visible = true;
            //    column1.Attributes["class"] = "col-12";
            //    column2.Visible = false;
            //    column3.Visible = false;

            //    //column2.Attributes["class"] = "col-md-1 col-sm-1";
            //    //column3.Attributes["class"] = "col-md-3 col-sm-3";
            //    //ContaArticoliPerperiodo(Tipologia);
            //    divSearch.Visible = false;
            //    divLatestPost.Visible = false;
            //    //CaricaUltimiPost(Tipologia, Categoria);
            //    //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
            //    //CaricaMenuContenuti(4, 9, rptContenutiLink);
            //    divCategorie.Visible = false;
            //    //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
            //    //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
            //    //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);

            //    if (!JavaInjection)
            //    {
            //        if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
            //        //string controllist2 = "injectPortfolioAndLoad(\"isotopeVini.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";
            //        //string controllist2 = "injectPortfolioAndLoad(\"isotopeOfferte1.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";

            //        //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
            //        //placeholderrisultati
            //        sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
            //        sb.Append("injectPortfolioAndLoad,isotopeSinglerowAnimated.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
            //        sb.Append("\"></div>");
            //        sb.Append("<div id=\"divPortfolioListPager\"></div>");
            //        placeholderrisultati.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);

            //    }

            //    //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
            //    break;

            case "rif000005":

                AssociaDatiSocial();

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                column3.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                //column3.Attributes["class"] = "col-md-3 col-sm-3";
                //ContaArticoliPerperiodo(Tipologia);
                divSearch.Visible = false;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;
                //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
                //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);

                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeVini.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeOfferte1.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    //placeholderrisultati
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopeOffertePortfolio.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000003":
            case "rif000004":
                AssociaDatiSocial();

                column1.Visible = false;
                column2.Visible = false;
                column3.Visible = false;

                columnsingle.Attributes["class"] = "col-12";

                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                //column3.Attributes["class"] = "col-md-3 col-sm-3";
                //ContaArticoliPerperiodo(Tipologia);
                divSearch.Visible = false;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;
                //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
                //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);

                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeVini.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeOfferte1.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    //placeholderrisultati
                    //sb.Append("<div>");
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopeSinglerowAnimated.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //sb.Append("</div>");
                    placeholderrisultatinocontainer.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000006":
                AssociaDatiSocial();

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                column3.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                //column3.Attributes["class"] = "col-md-3 col-sm-3";
                //ContaArticoliPerperiodo(Tipologia);
                divSearch.Visible = false;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;
                //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
                //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);

                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeVini.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeOfferte1.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    //placeholderrisultati
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopeGallery1.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000007":

                AssociaDatiSocial();

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                column3.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                //column3.Attributes["class"] = "col-md-3 col-sm-3";
                //ContaArticoliPerperiodo(Tipologia);
                divSearch.Visible = false;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;
                //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
                //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);

                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeVini.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeOfferte1.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    //placeholderrisultati
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopeStaff.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000008":
                AssociaDatiSocial();

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-12 col-sm-3";
                column3.Visible = false;
                ContaArticoliPerperiodo(Tipologia);
                //  Caricalinksrubriche(Tipologia); //arica la ddl con le sttocategorie
                divSearch.Visible = true;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;
                //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
                //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog2.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //placeholderrisultati.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);

                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog-no-image.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();

                    //sb.Clear();
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);
                }
                break;

            case "rif000009":

                AssociaDatiSocial();
                column1.Visible = true;
                column1.Attributes["class"] = "col-md-9 col-sm-9";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-md-3 col-sm-3";

                // ContaArticoliPerperiodo(Tipologia);
                divSearch.Visible = false;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;

                //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
                //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);
                if (!JavaInjection)
                {
                    //string controllist3 = "injectPortfolioAndLoad(\"isotopeOfferte5.html\",\"divContainerBannerslat1\", \"portlist2\", 1, 10, false, \"\", \"" + "rif000003" + "\", \"" + "" + "\", false, true, \"6\",\"" + testoricerca + "\");";

                    sb.Clear();

                    sb.Append("<div id=\"divPortfolioListLat\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopeOfferte5.html,divPortfolioListLat, portlistlat1, 1, 21, false, \'\', \'" + "rif000003" + "\', \'\', false, true, \'6\',\'\'");
                    sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    plhContainerLat.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();

                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    //placeholderrisultati
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopeOfferte4.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();
                }

                break;

            default:
                AssociaDatiSocial();

                column1.Visible = true;
                column1.Attributes["class"] = "col-md-9 col-sm-9";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-md-3 col-sm-3";
                ContaArticoliPerperiodo(Tipologia);
                // Caricalinksrubriche(Tipologia);
                divSearch.Visible = true;
                divLatestPost.Visible = false;
                //CaricaUltimiPost(Tipologia, Categoria);
                //CaricaMenuSezioniContenuto(Tipologia, rptProdottiContenutiLink);
                //CaricaMenuContenuti(4, 9, rptContenutiLink);
                divCategorie.Visible = false;
                //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
                //string cbandestra1 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divContainerBannerslat1', 'isotBannDestra1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, litBannersLaterali, Lingua, false, 6, 5);
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                    //placeholderrisultati
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog2.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();

                    sb.Clear();

                    sb.Clear();
                    sb.Append("<div class=\"sfondo-contenitore\">");
                    sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    sb.Append("</div>");
                    placeholderlateral.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();
                    //
                }

                break;
        }
    }

    private void ModificaFiltroJS()
    {
        //GESTIONE DEI FILTRI MEDIANTE LA SESSIONE
        Dictionary<string, string> objvalue = new Dictionary<string, string>();
        string sobjvalue = "";
        if (Session["objfiltro"] != null)
        {
            sobjvalue = Session["objfiltro"].ToString();
            objvalue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sobjvalue);
            if (objvalue == null) objvalue = new Dictionary<string, string>();
        }
        if (!string.IsNullOrEmpty(mese)) //
        {
            objvalue.Remove("mese");
            objvalue["mese"] = mese;
        }
        if (!string.IsNullOrEmpty(anno)) //
        {
            objvalue.Remove("anno");
            objvalue["anno"] = anno;
        }
        //if (Caratteristica6 != "") //ditta
        //{
        //    objvalue.Remove("hidCaratteristica6");
        //    if (Caratteristica6.Length >= 1)
        //        objvalue["hidCaratteristica6"] = Caratteristica6;

        //}
        //if (Caratteristica3 != "")
        //{
        //    objvalue.Remove("ddlAtcgmp1");
        //    objvalue.Remove("ddlAtcgmp2");
        //    objvalue.Remove("ddlAtcgmp3");
        //    objvalue.Remove("ddlAtcgmp4");
        //    objvalue.Remove("ddlAtcgmp5");

        //    if (Caratteristica3.Length >= 1)
        //        objvalue["ddlAtcgmp1"] = Caratteristica3.Substring(0, 1);
        //    if (Caratteristica3.Length >= 3)
        //        objvalue["ddlAtcgmp2"] = Caratteristica3.Substring(0, 3);
        //    if (Caratteristica3.Length >= 4)
        //        objvalue["ddlAtcgmp3"] = Caratteristica3.Substring(0, 4);
        //}
        sobjvalue = Newtonsoft.Json.JsonConvert.SerializeObject(objvalue);
        Session.Add("objfiltro", sobjvalue);
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
                    string link = CreaLinkRicerca("", Tipologia, Categoria, "", "", selanno, ((KeyValuePair<string, string>)e.Item.DataItem).Key, Tipologia, Lingua, Session);
                    alink.HRef = link;
                    DateTime _d = new DateTime(year, Convert.ToInt16(((KeyValuePair<string, string>)e.Item.DataItem).Key), 1);
                    alink.InnerHtml = String.Format("{0:MMM yyyy}", _d) + "    (" + ((KeyValuePair<string, string>)e.Item.DataItem).Value + ")";
                }
            }
        }
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

    protected void Cerca_Click(object sender, EventArgs e)
    {
        if (Server.HtmlEncode(inputCerca.Value).Trim() == "") return;
        //testoricerca
        string link = CreaLinkRicerca("", Tipologia, "", "", "", "", "", "-", Lingua, Session, true);
        Session.Add("testoricerca", Server.HtmlEncode(inputCerca.Value)); //carico in sessione il parametro da cercare
        Response.Redirect(link);
    }

    protected void CaricaUltimiPost(string tipologia, string categoria = "")
    {
        //Filtriamo alcune categorie
        string tipologiadacaricare = tipologia;
        if (string.IsNullOrEmpty(tipologiadacaricare))
            tipologiadacaricare = "rif000001,rif000002,rif000003,rif000004,rif000005,rif000006,rif000007,rif000008,,rif000009";

        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
#if true
        if (tipologia != "" && tipologia != "-")
        {
            SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", tipologia);
            parColl.Add(p3);
        }
        if (categoria != "")
        {
            SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", categoria);
            parColl.Add(p7);
        }
        OfferteCollection offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "1000", Lingua, false);
        //offerte.RemoveAll(c => (Convert.ToInt32((c.CodiceTipologia.Substring(3))) >= 100)); //Togliamo i risultati del catalogo ( andrebbero tolti nel filtro a monte)
#endif
        //OfferteCollection offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, "6", false, Lingua, false);

        rtpLatestPost.DataSource = offerte;
        rtpLatestPost.DataBind();
    }

    protected string CreaItem(object item)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (item != null)
        {
            Offerte itemOff = (Offerte)item;

            //CARICAMENTO DATI DB
            string pathimmagine = filemanage.ComponiUrlAnteprima(itemOff.FotoCollection_M.FotoAnteprima, itemOff.CodiceTipologia, itemOff.Id.ToString());
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

            sb.Append("	  <a class=\"portfolio-zoom\" target=" + target + " href=\"" + link + "\" title=\"" + denominazione + "\" > \r\n");
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

    private void AssociaDati()
    {
        //Eseguiamo la ricerca richiesta
        //Prendiamo la lista completa delle offerte con tutti dati relativi
        //filtrandoli in base ai parametri richiesti
        OfferteCollection offerte = new OfferteCollection();
        string regione = Regione;
        string provincia = Provincia;
        string comune = Comune;
        string tipologia = Tipologia;
        string fasciadiprezzo = FasciaPrezzo;
        string categoria = Categoria;
        string categoria2liv = Categoria2liv;

        //btnNexth.Text = references.ResMan("Common",Lingua,"txtTastoNext").ToString();
        //btnPrevh.Text = references.ResMan("Common",Lingua,"txtTastoPrev").ToString();

        //btnNext.Text = references.ResMan("Common",Lingua,"txtTastoNext").ToString();
        //btnPrev.Text = references.ResMan("Common",Lingua,"txtTastoPrev").ToString();

        //InizializzaEtichette();

        #region Versione con db ACCESS

        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
#if true

        if (provincia != "")
        {
            SQLiteParameter p1 = new SQLiteParameter("@CodicePROVINCIA", provincia);
            parColl.Add(p1);
        }
        if (comune != "")
        {
            SQLiteParameter p2 = new SQLiteParameter("@CodiceCOMUNE", comune);
            parColl.Add(p2);
        }
        if (tipologia != "" && tipologia != "-")
        {
            SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", tipologia);
            parColl.Add(p3);
        }
        if (regione != "" && regione != "-")
        {
            SQLiteParameter p4 = new SQLiteParameter("@CodiceREGIONE", regione);
            parColl.Add(p4);
        }
        double PrezzoMin = double.MinValue;
        double PrezzoMax = double.MaxValue;
        if (!string.IsNullOrWhiteSpace(fasciadiprezzo))
        {
            Fascediprezzo _selfascia = Utility.Fascediprezzo.Find(delegate (Fascediprezzo tmp) { return (tmp.Lingua == Lingua && tmp.Codice == fasciadiprezzo); });
            if (_selfascia != null)
            {
                PrezzoMin = _selfascia.PrezzoMin;
                PrezzoMax = _selfascia.PrezzoMax;
            }
            SQLiteParameter p5 = new SQLiteParameter("@PrezzoMin", PrezzoMin);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@PrezzoMax", PrezzoMax);
            parColl.Add(p6);
        }
        if (categoria != "")
        {
            SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", categoria);
            parColl.Add(p7);
        }
        if (categoria2liv != "")
        {
            SQLiteParameter p8 = new SQLiteParameter("@CodiceCategoria2Liv", categoria2liv);
            parColl.Add(p8);
        }
        if (testoricerca.Trim() != "")
        {
            testoricerca = testoricerca.Replace(" ", "%");
            SQLiteParameter p8 = new SQLiteParameter("@testoricerca", "%" + testoricerca + "%");
            parColl.Add(p8);
        }
        if (mese.Trim() != "" && anno.Trim() != "")
        {
            int _a = 0;
            int.TryParse(anno, out _a);
            int _m = 0;
            int.TryParse(mese, out _m);
            if (_a != 0)
            {
                SQLiteParameter p8 = new SQLiteParameter("@annofiltro", _a);
                parColl.Add(p8);
            }
            if (_m != 0)
            {
                SQLiteParameter p9 = new SQLiteParameter("@mesefiltro", _m);
                parColl.Add(p9);
            }
        }
        if (Giornofiltro.Trim() != "")
        {
            int _g = 0;
            int.TryParse(Giornofiltro, out _g);
            if (_g != 0)
            {
                SQLiteParameter pgiorno = new SQLiteParameter("@giornofiltro", _g);
                parColl.Add(pgiorno);
            }
        }
        offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "1000", Lingua, false);
        // offerte.RemoveAll(c => (Convert.ToInt32((c.CodiceTipologia.Substring(3))) >= 100)); //Togliamo i risultati del catalogo ( andrebbero tolti nel filtro a monte)

#endif

        #endregion Versione con db ACCESS

        AssociaDatiSocial();
        //if (offerte != null && offerte.Count > 0)
        //    AssociaDatiSocial(offerte[0]);

        Pager<Offerte> p = new Pager<Offerte>();
        if (offerte != null && offerte.Count > PagerRisultati.PageSize)
        {
            pnlPager.Visible = true;
        }
        else
            pnlPager.Visible = false;

        p = new Pager<Offerte>(offerte, true, this.Page, PageGuid + PagerRisultati.ClientID);
        PagerRisultati.TotalRecords = p.Count;
        try
        {
            PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
            //PagerRisultatiLow.CurrentPage = Convert.ToInt32(Pagina);
        }
        catch
        {
            Pagina = "1";
        }
        AssociaDatiRepeater(p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize));
    }

    protected void AssociaDatiSocial()
    {
        string host = System.Web.HttpContext.Current.Request.Url.Host.ToString();
        Tabrif actualpagelink = new Tabrif();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////  PER CREAZIONE LINK CANONICI E ALTERNATE ///////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///ITA///
        WelcomeLibrary.DOM.TipologiaOfferte sezioneI = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I" & tmp.Codice == Tipologia); });
        string sezionedescrizioneI = "";
        if (sezioneI != null)
            sezionedescrizioneI = sezioneI.Descrizione;
        if (!string.IsNullOrEmpty(Categoria))
        {
            Prodotto categoriaprodotto = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == "I");
            if (categoriaprodotto != null)
            {
                sezionedescrizioneI = categoriaprodotto.Descrizione;
            }
        }
        if (!string.IsNullOrEmpty(Categoria2liv))
        {
            SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
            if (categoriasprodotto != null)
            {
                sezionedescrizioneI = categoriasprodotto.Descrizione;
            }
        }

        ///GB/////
        WelcomeLibrary.DOM.TipologiaOfferte sezioneGB = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "GB" & tmp.Codice == Tipologia); });
        string sezionedescrizioneGB = "";
        if (sezioneGB != null)
            sezionedescrizioneGB = sezioneGB.Descrizione;
        if (!string.IsNullOrEmpty(Categoria))
        {
            Prodotto categoriaprodotto = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == "GB");
            if (categoriaprodotto != null)
            {
                sezionedescrizioneGB = categoriaprodotto.Descrizione;
            }
        }
        if (!string.IsNullOrEmpty(Categoria2liv))
        {
            SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "GB" && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
            if (categoriasprodotto != null)
            {
                sezionedescrizioneGB = categoriasprodotto.Descrizione;
            }
        }

        ///RU/////
        WelcomeLibrary.DOM.TipologiaOfferte sezioneRU = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "RU" & tmp.Codice == Tipologia); });
        string sezionedescrizioneRU = "";
        if (sezioneRU != null)
            sezionedescrizioneRU = sezioneRU.Descrizione;
        if (!string.IsNullOrEmpty(Categoria))
        {
            Prodotto categoriaprodotto = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == "RU");
            if (categoriaprodotto != null)
            {
                sezionedescrizioneRU = categoriaprodotto.Descrizione;
            }
        }
        if (!string.IsNullOrEmpty(Categoria2liv))
        {
            SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "RU" && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
            if (categoriasprodotto != null)
            {
                sezionedescrizioneRU = categoriasprodotto.Descrizione;
            }
        }

        ////////////
        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));
        string urlcanonico = "";
        string hreflang = "";
        //METTIAMO GLI ALTERNATE
        hreflang = " hreflang=\"it\" ";
        string linkcanonicoalt = CreaLinkRoutes(null, false, "I", CleanUrl(sezionedescrizioneI), "", Tipologia, Categoria, Categoria2liv);
        linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
            linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit"));

        Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
        litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (linkcanonicoalt) + "\"/>";
        Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
        litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (linkcanonicoalt) + "\"/>";

        if (Lingua == "I")
        {
            urlcanonico = (linkcanonicoalt);
            litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (linkcanonicoalt) + "\"/>";
            actualpagelink.Campo1 = (linkcanonicoalt);
            actualpagelink.Campo2 = CleanUrl(sezionedescrizioneI);
        }
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            linkcanonicoalt = CreaLinkRoutes(null, false, "GB", CleanUrl(sezionedescrizioneGB), "", Tipologia, Categoria, Categoria2liv);
            linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen"));

            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (linkcanonicoalt) + "\"/>";
            if (Lingua == "GB")
            {
                urlcanonico = (linkcanonicoalt);
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (linkcanonicoalt) + "\"/>";
                actualpagelink.Campo1 = (linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(sezionedescrizioneGB);
            }
        }
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
        {
            hreflang = " hreflang=\"ru\" ";
            linkcanonicoalt = CreaLinkRoutes(null, false, "RU", CleanUrl(sezionedescrizioneRU), "", Tipologia, Categoria, Categoria2liv);
            linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru"));
            litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
            litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (linkcanonicoalt) + "\"/>";
            if (Lingua == "RU")
            {
                urlcanonico = (linkcanonicoalt);
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (linkcanonicoalt) + "\"/>";
                actualpagelink.Campo1 = (linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(sezionedescrizioneRU);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////SEZIONE META TITLE E DESC E CONTENUTO HEADER PAGINA ///////////////////////

        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();

        WelcomeLibrary.DOM.TipologiaOfferte sezione = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua & tmp.Codice == Tipologia); });
        string sezionedescrizione = "";
        if (!string.IsNullOrEmpty(Categoria2liv))
        {
            SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
            if (categoriasprodotto != null)
            {
                sezionedescrizione += " " + categoriasprodotto.Descrizione;
            }
        }
        if (!string.IsNullOrEmpty(Categoria))
        {
            Prodotto categoriaprodotto = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
            if (categoriaprodotto != null)
            {
                sezionedescrizione += " " + categoriaprodotto.Descrizione;
            }
        }
        if (sezione != null)
            sezionedescrizione += " " + sezione.Descrizione;
        if (sezione != null)
        {
            ////////EVIDENZIAZIONE MENU
            EvidenziaSelezione(sezione.Descrizione); // Server Solo per la voce al top dei dropdown ....

            ///////////////////////NOME PAGINA////////////////////////////////
            string titolopagina = sezione.Descrizione;
            //litNomePagina.Text = titolopagina;
            litNomePagina.Text = "";

            Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == Tipologia && tmp.CodiceProdotto == Categoria)); });
            if (catselected != null && (litNomePagina.Text.ToLower().Trim() != catselected.Descrizione.ToLower().Trim()))
            {
                litNomePagina.Text += " " + catselected.Descrizione;
            }
            SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
            if (categoriasprodotto != null)
            {
                litNomePagina.Text += " " + categoriasprodotto.Descrizione;
            }
            ///////////////////////////////////////////////////////////////
        }

        string htmlPage = "";
        //CARICAMENTO TESTI DA RISORSE
        if (references.ResMan("Common", Lingua, "testo" + Tipologia) != null)
            htmlPage = references.ResMan("Common", Lingua, "testo" + Tipologia).ToString();
        if (references.ResMan("Common", Lingua, "testo" + Categoria) != null)
            htmlPage = references.ResMan("Common", Lingua, "testo" + Categoria).ToString();
        urlcanonico = urlcanonico.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "");
        Contenuti content = null;
        content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, urlcanonico);
        string customtitle = "";
        string customdesc = "";
        if (content != null && content.Id != 0)
        {
            htmlPage = ReplaceLinks(content.DescrizionebyLingua(Lingua));
            if (htmlPage.Contains("injectPortfolioAndLoad")) JavaInjection = true;
            switch (Lingua)
            {
                case "GB":
                    customdesc = content.CustomdescGB;
                    customtitle = content.CustomtitleGB;
                    break;

                case "RU":
                    customdesc = content.CustomdescRU;
                    customtitle = content.CustomtitleRU;
                    break;

                default:
                    customdesc = content.CustomdescI;
                    customtitle = content.CustomtitleI;
                    break;
            }
        }

        string metametatitle = html.Convert(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione + " " + references.ResMan("Common", Lingua, "testoPosizionebase")) + " " + Nome);
        string description = "";
        description = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(htmlPage, 150, true)).Replace("<br/>", "\r\n")).Trim();

        /////////////////////////////////////////////////////////////
        //MODIFICA PER TITLE E DESCRIPTION CUSTOM ( da sezione admin )
        ////////////////////////////////////////////////////////////
        if (!string.IsNullOrEmpty(customtitle))
            metametatitle = (customtitle).Replace("<br/>", "\r\n");
        if (!string.IsNullOrEmpty(customdesc))
            description = customdesc.Replace("<br/>", "\r\n");
        description = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(description, 150, true)).Replace("<br/>", "\r\n")).Trim();
        ////////////////////////////////////////////////////////////
        //Opengraph per facebook
        ((HtmlMeta)Master.FindControl("metafbTitle")).Content = html.Convert(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione)).Trim();
        ((HtmlMeta)Master.FindControl("metafbdescription")).Content = description;
        //////////////////////////////////////////
        ((HtmlTitle)Master.FindControl("metaTitle")).Text = metametatitle;
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = description;
        //////////////////////////////////////////////////////////////////////////
        litTextHeadPage.Text = ReplaceAbsoluteLinks(ReplaceLinks(htmlPage));

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////BREAD CRUMBS///////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        List<Tabrif> links = GeneraBreadcrumbPath(true);
        if (false) //Pagina copertina presente
        {
            Prodotto catcopertina = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
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

        return links;
    }

    protected void EvidenziaSelezione(string testolink)
    {
        HtmlAnchor linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink.Replace(" ", "")));
        Console.WriteLine(testolink);
        try
        {
            linkmenu = ((HtmlAnchor)Master.FindControl("linkid" + Tipologia + "high"));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                Control lidrop = CommonPage.FindControlRecursive(linkmenu.Parent, linkmenu.Parent.ID);

                if (lidrop != null)
                {
                    ((HtmlGenericControl)lidrop).Attributes["class"] += " active";
                }
            }
            linkmenu = ((HtmlAnchor)Master.FindControl("linkid" + Tipologia));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
            }
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                //linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#000 !important");
            }

            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink + "high"));
            if (linkmenu != null)
            {
                Control lidrop = CommonPage.FindControlRecursive(linkmenu.Parent, linkmenu.Parent.ID);
                if (lidrop != null)
                {
                    ((HtmlGenericControl)lidrop).Attributes["class"] += " active";
                }
            }
        }
        catch { }
    }

    protected void AssociaDatiRepeater(List<Offerte> list)
    {
        switch (Tipologia)
        {
            case "rif000002":
                break;
            //case "rif000008":
            //case "rif000009":
            //case "rif000010":
            //case "rif000011":
            //    Master.CaricaContenutiPortfolioRivalBordered(Tipologia, litPortfolioRivals3b, Lingua, "", list);
            //    break;
            //case "rif000002":
            //    Master.CaricaContenutiPortfolioRivalSubtext(Tipologia, litPortfolioRivals2, Lingua, "", list);
            //    break;
            //case "rif000012":
            //    Master.CaricaContenutiPortfolioRivalSubtext(Tipologia, litGalleryDetails, Lingua, "", list, "noborder");
            //    break;
            default:
                rptOfferte.DataSource = list;
                rptOfferte.DataBind();
                break;
        }
    }

    protected bool VerificaPresenzaPrezzo(object prezzo)
    {
        bool ret = false;
        if (prezzo != null && (double)prezzo != 0)
            ret = true;
        return ret;
    }

    protected string SetStyle(string tipologia)
    {
        string ret = "";
        if (tipologia == Tipologia)
        {
            ret = "color:#43983d";
        }
        return ret;
    }

    protected bool ControllaVisibilitaPerCodice(string codicetipologia)
    {
        bool ret = true;
        //if (codicetipologia == "rif000001" || codicetipologia == "rif000002" || codicetipologia == "rif000004") ret = false;
        return ret;
    }

    protected bool ControlloVisibilita(object fotos)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 0) ret = false;
        bool onlypdf = (fotos != null && ((AllegatiCollection)fotos).Count > 0 && !((AllegatiCollection)fotos).Exists(c => (c.NomeFile.ToString().ToLower().EndsWith("jpg") || c.NomeFile.ToString().ToLower().EndsWith("gif") || c.NomeFile.ToString().ToLower().EndsWith("png"))));
        if (onlypdf) ret = false;
        return ret;
    }

    protected bool ControlloVideo(object NomeAnteprima, object linkVideo)
    {
        bool ret = false;
        //"http://www.youtube.com/embed/Z9lwY9arkj8"
        if ((NomeAnteprima == null || NomeAnteprima.ToString() == "") && (linkVideo != null && ((string)linkVideo) != ""))
            ret = true;
        return ret;
    }

    protected string SorgenteVideo(object linkVideo)
    {
        string ret = "";
        //"http://www.youtube.com/embed/Z9lwY9arkj8"

        if (linkVideo != null && linkVideo.ToString() != "")
            ret = linkVideo.ToString();
        return ret;
    }

    protected void ImgAnt_PreRender(object sender, EventArgs e)
    {
        int maxwidth = 465;
        int maxheight = 310;
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
                            //((HtmlGenericControl)(((Image)sender).Parent)).Attributes["style"] = "height:" + maxheight + "px;overflow: hidden; float: left; margin:  5px";
                            //((Image)sender).Height = maxheight;
                            //((Image)sender).Width = tmp.Width * maxheight / tmp.Height;
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

    //protected string ControlloVuotoPosizione(string comune, string codiceprovincia, string codicetipologia)
    //{
    //    string ret = "";
    //    //WelcomeLibrary.DOM.TipologiaOfferte sezione =
    //    // WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate(WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == "rif000100"); });
    //    //if (codicetipologia == "rif000100")
    //    //{
    //    if (!string.IsNullOrWhiteSpace(codiceprovincia))
    //        ret += NomeRegione(codiceprovincia).ToLower() + " ";
    //    if (!string.IsNullOrWhiteSpace(codiceprovincia))
    //        ret += " (" + NomeProvincia(codiceprovincia).ToLower() + ") ";
    //    if (!string.IsNullOrWhiteSpace(comune))
    //        ret += comune.ToLower();
    //    //}
    //    return ret;
    //}
    //protected string NomeRegione(string codiceprovincia)
    //{
    //    string ritorno = "";
    //    Province item = Utility.ElencoProvince.Find(delegate(Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codiceprovincia); });
    //    if (item != null)
    //        ritorno = item.Regione;
    //    return ritorno;
    //}
    //protected string NomeProvincia(string codiceprovincia)
    //{
    //    string ritorno = "";
    //    Province item = Utility.ElencoProvince.Find(delegate(Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codiceprovincia); });
    //    if (item != null)
    //        ritorno = item.Provincia;
    //    return ritorno;
    //}

    #region PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER

    protected void btnPrev_click(object sender, EventArgs e)
    {
        int pag = PagerRisultati.CurrentPage;
        pag--;
        if (pag < 1) pag = 1;
        Pagina = pag.ToString();
        //Session["Pagina"] = Pagina;

        AssociaDati();
    }

    protected void btnNext_click(object sender, EventArgs e)
    {
        int pag = PagerRisultati.CurrentPage;
        pag++;
        if (pag > PagerRisultati.totalPages) pag = PagerRisultati.totalPages;
        Pagina = pag.ToString();
        //Session["Pagina"] = Pagina;

        AssociaDati();
    }

    protected void PagerRisultati_PageCommand(object sender, string PageNum)
    {
        PagerRisultati.CurrentPage = Convert.ToInt32(PageNum);
        Pagina = PageNum;
        //Session["Pagina"] = Pagina;

        Pager<Offerte> p = new Pager<Offerte>();
        if (p.LoadFromCache(this, PageGuid + PagerRisultati.ClientID))
        {
            AssociaDatiRepeater(p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize));
        }
        else
        {
            AssociaDati();
        }
    }

    protected void PagerRisultati_PageGroupClickNext(object sender, string spare)
    {
        //PagerRisultatiLow.nGruppoPagine += 1;
    }

    protected void PagerRisultati_PageGroupClickPrev(object sender, string spare)
    {
        //PagerRisultatiLow.nGruppoPagine -= 1;
    }

    protected void PagerRisultatiLow_PageGroupClickNext(object sender, string spare)
    {
        PagerRisultati.nGruppoPagine += 1;
    }

    protected void PagerRisultatiLow_PageGroupClickPrev(object sender, string spare)
    {
        PagerRisultati.nGruppoPagine -= 1;
    }

    #endregion PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER
}