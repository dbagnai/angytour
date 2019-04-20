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

public partial class AspNetPages_weblist : CommonPage
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
    public bool Vetrina
    {
        get { return ViewState["Vetrina"] != null ? (bool)(ViewState["Vetrina"]) : false; }
        set { ViewState["Vetrina"] = value; }
    }
    public string Promozioni
    {
        get { return ViewState["Promozioni"] != null ? (string)(ViewState["Promozioni"]) : ""; }
        set { ViewState["Promozioni"] = value; }
    }
    public string Ordinamento
    {
        get { return ViewState["Ordinamento"] != null ? (string)(ViewState["Ordinamento"]) : ""; }
        set { ViewState["Ordinamento"] = value; }
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
    public string Annata
    {
        get { return ViewState["Annata"] != null ? (string)(ViewState["Annata"]) : ""; }
        set { ViewState["Annata"] = value; }
    }

    public string Categoria
    {
        get { return ViewState["Categoria"] != null ? (string)(ViewState["Categoria"]) : ""; }
        set { ViewState["Categoria"] = value; }
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
                Mesefiltro = CaricaValoreMaster(Request, Session, "Mesefiltro", false);
                Giornofiltro = CaricaValoreMaster(Request, Session, "Giornofiltro", false);
                Annata = CaricaValoreMaster(Request, Session, "Annata", false);
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
                Promozioni = CaricaValoreMaster(Request, Session, "promozioni");


                Categoria = CaricaValoreMaster(Request, Session, "Categoria", true);
                Categoria2liv = CaricaValoreMaster(Request, Session, "Categoria2liv", true);
                if (Categoria2liv == "all") Categoria2liv = "";
                //Impostiamo anche gli altri parametri di ricerca
                Tipologia = CaricaValoreMaster(Request, Session, "Tipologia", true, "");

                string tmp = "";
                tmp = CaricaValoreMaster(Request, Session, "mese", true);
                if (!string.IsNullOrEmpty(tmp)) mese = tmp;
                tmp = CaricaValoreMaster(Request, Session, "anno", true);
                if (!string.IsNullOrEmpty(tmp)) anno = tmp;
                tmp = CaricaValoreMaster(Request, Session, "testoricerca", false);
                if (!string.IsNullOrEmpty(tmp)) testoricerca = tmp;
                Session.Remove("testoricerca");

                LoadJavascriptVariables();
                SettaVisualizzazione();

                DataBind(); // renderizza le sezioni <%#
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
    private void SettaVisualizzazione()
    {
        string cattipo = Tipologia;
        ClientScriptManager cs = Page.ClientScript;
        //ModificaFiltroJS();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        InizializzaSeo();
        ModificaFiltroJS();
        custombind cb = new custombind();
        switch (Tipologia)
        {
            case "rif000001": //Simil prodotti x app
                if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                column1.Visible = true;
                column1.Attributes["class"] = "col-sm-12";
                column2.Visible = false;
                column3.Visible = false;
                divSearch.Visible = false;

                //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                string svetrina = "";
                if (Vetrina) svetrina = "true";
                //string controllist2 = "injectPortfolioAndLoad(\"isotopeProdotti1.html\",\"divPortfolioList1\", \"portlist1\", 1, 16, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\", '" + svetrina + "','" + Promozioni + "', \"\", \"" + Categoria2liv + "\");";
                sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                sb.Append("injectPortfolioAndLoad,isotopeProdotti1b.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\',\'" + svetrina + "\',\'" + Promozioni + "\',\'\', '" + Categoria2liv + "\'");
                sb.Append("\"></div>");
                sb.Append("<div id=\"divPortfolioListPager\"></div>");
                placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                break;
            case "rif000002":

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-12 col-sm-3";
                column3.Visible = false;
                ContaArticoliPerperiodo(Tipologia);
                //  Caricalinksrubriche(Tipologia); //arica la ddl con le sttocategorie
                divSearch.Visible = true;
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog2.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //placeholderrisultati.Text = sb.ToString();

                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog3Card.html,divPortfolioList,portlist1, 1, 42,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',true,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioBlog3Card.html,divPortfolioList,portlist1, 1, 42,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',true,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //placeholderrisultatinocontainer.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();


                    //sb.Clear();
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text =  cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session,null,null,Request);
                }
                break;
            case "rif000005":


                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                column3.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                //column3.Attributes["class"] = "col-md-3 col-sm-3";
                //ContaArticoliPerperiodo(Tipologia);
                divSearch.Visible = false;
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeVini.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeOfferte1.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    //placeholderrisultati
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePortfolio4Card.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000003":
            case "rif000004":

                column1.Visible = false;
                column2.Visible = false;
                column3.Visible = false;
                columnsingle.Attributes["class"] = "col-12";
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                //column3.Attributes["class"] = "col-md-3 col-sm-3";
                //ContaArticoliPerperiodo(Tipologia);
                divSearch.Visible = false;
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeVini.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeOfferte1.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    //placeholderrisultati
                    //sb.Append("<div>");
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopeSinglerowAnimated-no-button.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //sb.Append("</div>");
                    placeholderrisultatinocontainer.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000006":

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                column3.Visible = false;
                divSearch.Visible = false;

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
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000007":


                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                column3.Visible = false;

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
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000008":

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-12 col-sm-3";
                column3.Visible = false;
                ContaArticoliPerperiodo(Tipologia);
                //  Caricalinksrubriche(Tipologia); //arica la ddl con le sttocategorie
                divSearch.Visible = true;
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog2.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //placeholderrisultati.Text =  cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session,null,null,Request);

                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog-no-image.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                    //sb.Clear();
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text =  cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session,null,null,Request);
                }
                break;

            case "rif000009":


                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                column3.Visible = false;

                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeVini.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";
                    //string controllist2 = "injectPortfolioAndLoad(\"isotopeOfferte1.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\");";

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    //placeholderrisultati
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,IsotopeTestimonials.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;
            default:

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-12 col-sm-3";
                column3.Visible = false;
                ContaArticoliPerperiodo(Tipologia);
                //  Caricalinksrubriche(Tipologia); //arica la ddl con le sttocategorie
                divSearch.Visible = true;
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
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                    //sb.Clear();
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text =  cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session,null,null,Request);
                }
                break;
        }
        Setrelprevnext();

        Session.Remove("objfiltro"); //Filtro modificatore che usa la sessione per selezionare i risultati visualizzati
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


#if true //Gestione modificatori firtro categoria

        if (Promozioni != "")
        {
            objvalue["promozioni"] = Promozioni;
        }

        if (Categoria != "")
        {
            objvalue["categoria"] = Categoria;
            objvalue["ddlCategoria"] = Categoria;
        }
        else
        {
            if (objvalue.ContainsKey("ddlCategoria"))
            {
                objvalue["categoria"] = objvalue["ddlCategoria"];
            }
        }
        if (Categoria2liv != "")
        {
            objvalue["categoria2Liv"] = Categoria2liv;
            objvalue["ddlCategoria2liv"] = Categoria2liv;
        }
        else
        {

            if (objvalue.ContainsKey("ddlCategoria2liv"))
            {
                objvalue["categoria2Liv"] = objvalue["ddlCategoria2liv"];
            }
        }


        if (Regione != "")
        {
            objvalue["regione"] = Regione;
            objvalue["ddlRegione"] = Regione;
            objvalue["ddlRegioneSearch"] = Regione;
        }
        else
        {
            if (objvalue.ContainsKey("ddlRegione"))
            {
                objvalue["regione"] = objvalue["ddlRegione"];
                Regione = objvalue["ddlRegione"];
            }
            if (objvalue.ContainsKey("ddlRegioneSearch"))
            {
                objvalue["regione"] = objvalue["ddlRegioneSearch"];
                Regione = objvalue["ddlRegioneSearch"];
            }
        }
        if (Provincia != "")
        {
            objvalue["provincia"] = Provincia;
            objvalue["ddlProvincia"] = Provincia;
            objvalue["ddlProvinciaSearch"] = Provincia;
        }
        else
        {
            if (objvalue.ContainsKey("ddlProvincia"))
            {
                objvalue["provincia"] = objvalue["ddlProvincia"];
                Provincia = objvalue["ddlProvincia"];
            }
            if (objvalue.ContainsKey("ddlProvinciaSearch"))
            {
                objvalue["provincia"] = objvalue["ddlProvinciaSearch"];
                Provincia = objvalue["ddlProvinciaSearch"];
            }
        }


        if (Comune != "")
        {
            objvalue["comune"] = Comune;
            objvalue["ddlComune"] = Comune;
            objvalue["ddlComuneSearch"] = Comune;
        }
        else
        {
            if (objvalue.ContainsKey("ddlComune"))
            {
                objvalue["comune"] = objvalue["ddlComune"];
                Comune = objvalue["ddlComune"];
            }
            if (objvalue.ContainsKey("ddlComuneSearch"))
            {
                objvalue["comune"] = objvalue["ddlComuneSearch"];
                Comune = objvalue["ddlComuneSearch"];
            }
        }


        if (Caratteristica1 != "")
        {
            objvalue["caratteristica1"] = Caratteristica1;
            objvalue["hidCaratteristica1"] = Caratteristica1;
            objvalue["ddlCaratteristica1"] = Caratteristica1;
        }
        else
        {
            if (objvalue.ContainsKey("hidCaratteristica1"))
            {
                objvalue["caratteristica1"] = objvalue["hidCaratteristica1"];
                Caratteristica1 = objvalue["hidCaratteristica1"];
            }
            if (objvalue.ContainsKey("ddlCaratteristica1"))
            {
                objvalue["caratteristica1"] = objvalue["ddlCaratteristica1"];
                Caratteristica1 = objvalue["ddlCaratteristica1"];
            }

        }


        if (Caratteristica2 != "")
        {
            objvalue["caratteristica2"] = Caratteristica2;
            objvalue["hidCaratteristica2"] = Caratteristica2;
        }
        else
        {
            if (objvalue.ContainsKey("hidCaratteristica2"))
            {
                objvalue["caratteristica2"] = objvalue["hidCaratteristica2"];
                Caratteristica2 = objvalue["hidCaratteristica2"];
            }
        }

        //Id selection
        //hidricercaid ( sleezione con id )
        if (objvalue.ContainsKey("hidricercaid"))
        {
            objvalue["id"] = objvalue["hidricercaid"];
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


        if (objvalue.ContainsKey("geolocation"))
        {
            objvalue.Remove("latitudine");
            objvalue.Remove("longitudine");
            string latitudine = "";
            string longitudine = "";
            string[] latlng = objvalue["geolocation"].Split(',');
            if (latlng != null && latlng.Length == 2)//&& !string.IsNullOrWhiteSpace(address.Value))
            {
                latitudine = latlng[0];
                longitudine = latlng[1];
            }

            double lat = 0;
            double.TryParse(latitudine.Replace(".", ","), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), out lat);
            double lon = 0;
            double.TryParse(longitudine.Replace(".", ","), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), out lon);
            if (lat != 0 && lon != 0)
            {
                objvalue["latitudine"] = latitudine;
                objvalue["longitudine"] = longitudine;
                //Session.Add("Address", address.Value);
            }
            else
                objvalue.Remove("geolocation");
        }
        else
        {
            objvalue.Remove("latitudine");
            objvalue.Remove("longitudine");
        }
#endif

        sobjvalue = Newtonsoft.Json.JsonConvert.SerializeObject(objvalue);
        Session.Add("objfiltro", sobjvalue);
    }

    /// <summary>
    /// Aggiunge i link rel prev e next nell'head della pagina se inseriti in sessione
    /// </summary>
    protected void Setrelprevnext()
    {
        //"<link rel=\"prev\" href=\"\" />";
        //"<link rel=\"next\" href=\"\" />";

        HtmlHead head = (HtmlHead)Master.FindControl("masterHead");
        if (head != null)
        {
            HtmlLink linkctr = new HtmlLink();
            if (Session["linkprev"] != null && Session["linkprev"].ToString() != "")
            {
                linkctr.Attributes.Add("rel", "prev");
                linkctr.Attributes.Add("href", Session["linkprev"].ToString());
                head.Controls.Add(linkctr);
            }
            linkctr = new HtmlLink();
            if (Session["linknext"] != null && Session["linknext"].ToString() != "")
            {
                linkctr.Attributes.Add("rel", "next");
                linkctr.Attributes.Add("href", Session["linknext"].ToString());
                head.Controls.Add(linkctr);
            }
        }

        Session.Remove("linkprev");
        Session.Remove("linknext");
    }

    protected void InizializzaSeo()
    {
        string host = System.Web.HttpContext.Current.Request.Url.Host.ToString();
        Tabrif actualpagelink = new Tabrif();
        Contenuti contentcollegato = null;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////  IMPOSTAZIONE LINK CANONICI E ALTERNATE ///////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //PRENDIAMO I TESTI TIPOLOGIE PER LINGUA A SECONDA DEL LIVELLO DELLA PAGINA LISTA
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////// ALTERNATE E CANONICAL /////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        string linki = ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, "I", CleanUrl(sezionedescrizioneI), "", Tipologia, Categoria, Categoria2liv));
        string linken = ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, "GB", CleanUrl(sezionedescrizioneGB), "", Tipologia, Categoria, Categoria2liv));
        string linkru = ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, "RU", CleanUrl(sezionedescrizioneRU), "", Tipologia, Categoria, Categoria2liv));
        contentcollegato = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linki.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, ""));
        if ((contentcollegato == null || contentcollegato.Id == 0) && WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
            contentcollegato = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linken.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, ""));
        if ((contentcollegato == null || contentcollegato.Id == 0) && WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
            contentcollegato = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkru.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, ""));

        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));
        string hreflang = "";
        //METTIAMO GLI ALTERNATE
        hreflang = " hreflang=\"it\" ";
        //Leggiamo se presente un contenuto collegato nelle statiche
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
            linki = linki.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit"));
        string modcanonical = linki;

        //Se presente la querystring la includo nel canonical di pagina  ( serve per la paginazione )  
        string querystring = System.Web.HttpContext.Current.Request.Url.Query;
        if (!string.IsNullOrEmpty(querystring) && querystring.ToLower() != "?page=1")
            modcanonical += querystring;

        //FORZATURA CANONICAL utente
        if (contentcollegato != null && !string.IsNullOrEmpty(contentcollegato.CanonicalbyLingua("I").Trim()))
            modcanonical = (contentcollegato.CanonicalbyLingua("I").Trim());

        //alternate
        Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
        if (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneI)))
            litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";

        //x-default
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "I")
        {
            Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
            litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>";
        }
        if (Lingua.ToLower() == "i")
        {
            litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
            actualpagelink.Campo1 = (linki);
            actualpagelink.Campo2 = CleanUrl(sezionedescrizioneI);

            //redirect al canonical se il canonical non coincide con l'url escudendo la querystring
            if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
            {
                HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
                metarobots.Attributes["Content"] = "noindex,follow";
                if (!(!string.IsNullOrEmpty(mese) && !string.IsNullOrEmpty(anno))) //controllo filtro archivio nel qual caso non faccio redirect
                    if (!System.Web.HttpContext.Current.Request.Url.ToString().EndsWith("-")) // non faccio redirec neppure per gli indizizzi con ricerca!!! ( finicono in - )
                    {
                        Response.RedirectPermanent(modcanonical, true);
                    }
            }
        }

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            //Leggiamo se presente un contenuto collegato nelle statiche
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linken = linken.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen"));
            //FORZATURA CANONICAL utente
            modcanonical = linken;
            if (!string.IsNullOrEmpty(querystring))
                modcanonical += querystring;

            if (contentcollegato != null && !string.IsNullOrEmpty(contentcollegato.CanonicalbyLingua("GB").Trim()))
                modcanonical = (contentcollegato.CanonicalbyLingua("GB").Trim());
            //alternate
            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            if (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneGB)))
                litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "GB")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>";
            }
            if (Lingua.ToLower() == "gb")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
                actualpagelink.Campo1 = (linken);
                actualpagelink.Campo2 = CleanUrl(sezionedescrizioneGB);
                //redirect al canonical se il canonical non coincide con l'url
                if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
                {
                    HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
                    metarobots.Attributes["Content"] = "noindex,follow";
                    if (!(!string.IsNullOrEmpty(mese) && !string.IsNullOrEmpty(anno))) //controllo filtro archivio nel qual caso non faccio redirect
                        if (!System.Web.HttpContext.Current.Request.Url.ToString().EndsWith("-")) // non faccio redirec neppure per gli indizizzi con ricerca!!! ( finicono in - )
                        {
                            Response.RedirectPermanent(modcanonical, true);
                        }
                }
            }
        }
        else linken = "";
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
        {
            hreflang = " hreflang=\"ru\" ";
            //Leggiamo se presente un contenuto collegato nelle statiche
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkru = linkru.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru"));

            //FORZATURA CANONICAL utente
            modcanonical = linkru;
            if (!string.IsNullOrEmpty(querystring))
                modcanonical += querystring;
            if (contentcollegato != null && !string.IsNullOrEmpty(contentcollegato.CanonicalbyLingua("RU").Trim()))
                modcanonical = (contentcollegato.CanonicalbyLingua("RU").Trim());
            //alternate
            litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
            if (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneRU)))
                litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "RU")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>";
            }
            if (Lingua.ToLower() == "ru")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
                actualpagelink.Campo1 = (linkru);
                actualpagelink.Campo2 = CleanUrl(sezionedescrizioneRU);
                //redirect al canonical se il canonical non coincide con l'url
                if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
                {
                    HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
                    metarobots.Attributes["Content"] = "noindex,follow";
                    if (!(!string.IsNullOrEmpty(mese) && !string.IsNullOrEmpty(anno))) //controllo filtro archivio nel qual caso non faccio redirect
                        if (!System.Web.HttpContext.Current.Request.Url.ToString().EndsWith("-")) // non faccio redirec neppure per gli indizizzi con ricerca!!! ( finicono in - )
                        {
                            Response.RedirectPermanent(modcanonical, true);
                        }
                }
            }
        }
        else linkru = "";
        //GESTIONE CAMBIO LINGUA PER PAGINA
        switch (Lingua.ToLower())
        {
            case "i":
                if (!string.IsNullOrEmpty(linken) && (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneGB))))
                {
                    HtmlGenericControl divCambioLingua1 = (HtmlGenericControl)Master.FindControl("divCambioLingua1");
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linken;
                    if (System.Web.HttpContext.Current.Request.Url.Query != "")
                        divCambioLingua1.InnerHtml += System.Web.HttpContext.Current.Request.Url.Query;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                    HtmlGenericControl divCambioLinguadef1 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef1");
                    divCambioLinguadef1.Visible = false;
                }
                if (!string.IsNullOrEmpty(linkru) && (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneRU))))
                {
                    HtmlGenericControl divCambioLingua2 = (HtmlGenericControl)Master.FindControl("divCambioLingua2");
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linkru;
                    if (System.Web.HttpContext.Current.Request.Url.Query != "")
                        divCambioLingua2.InnerHtml += System.Web.HttpContext.Current.Request.Url.Query;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                    HtmlGenericControl divCambioLinguadef2 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef2");
                    divCambioLinguadef2.Visible = false;
                }

                break;
            case "gb":
                if (!string.IsNullOrEmpty(linki) && (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneI))))
                {
                    HtmlGenericControl divCambioLingua1 = (HtmlGenericControl)Master.FindControl("divCambioLingua1");
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linki;
                    if (System.Web.HttpContext.Current.Request.Url.Query != "")
                        divCambioLingua1.InnerHtml += System.Web.HttpContext.Current.Request.Url.Query;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                    HtmlGenericControl divCambioLinguadef1 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef1");
                    divCambioLinguadef1.Visible = false;
                }
                if (!string.IsNullOrEmpty(linkru) && (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneRU))))
                {
                    HtmlGenericControl divCambioLingua2 = (HtmlGenericControl)Master.FindControl("divCambioLingua2");
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linkru;
                    if (System.Web.HttpContext.Current.Request.Url.Query != "")
                        divCambioLingua2.InnerHtml += System.Web.HttpContext.Current.Request.Url.Query;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                    HtmlGenericControl divCambioLinguadef2 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef2");
                    divCambioLinguadef2.Visible = false;
                }
                break;
            case "ru":
                if (!string.IsNullOrEmpty(linken) && (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneGB))))
                {
                    HtmlGenericControl divCambioLingua1 = (HtmlGenericControl)Master.FindControl("divCambioLingua1");
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linken;
                    if (System.Web.HttpContext.Current.Request.Url.Query != "")
                        divCambioLingua1.InnerHtml += System.Web.HttpContext.Current.Request.Url.Query;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                    HtmlGenericControl divCambioLinguadef1 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef1");
                    divCambioLinguadef1.Visible = false;
                }
                if (!string.IsNullOrEmpty(linki) && (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneI))))
                {
                    HtmlGenericControl divCambioLingua2 = (HtmlGenericControl)Master.FindControl("divCambioLingua2");
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linki;
                    if (System.Web.HttpContext.Current.Request.Url.Query != "")
                        divCambioLingua2.InnerHtml += System.Web.HttpContext.Current.Request.Url.Query;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                    HtmlGenericControl divCambioLinguadef2 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef2");
                    divCambioLinguadef2.Visible = false;
                }
                break;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////SEZIONE META TITLE E DESC E CONTENUTO HEADER/h1 PAGINA ///////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        ////////EVIDENZIAZIONE MENU//////////////////////////////////////////////////////////////
        if (sezione != null)
            EvidenziaSelezione(sezione.Descrizione); // Serve Solo per la voce al top dei dropdown ....


        ////////////////////////////////////////////////////////////////
        //CARICAMENTO E CUSTOMIZZAZIONE DA PAGINE STATICHE testi e meta
        ////////////////////////////////////////////////////////////////
        string htmlPage = "";
        if (references.ResMan("Common", Lingua, "testo" + Tipologia) != null)
            htmlPage = references.ResMan("Common", Lingua, "testo" + Tipologia).ToString();
        if (references.ResMan("Common", Lingua, "testo" + Categoria) != null)
            htmlPage = references.ResMan("Common", Lingua, "testo" + Categoria).ToString();

        //prendiamo i dalti dal contenuto collegato coretto nelle statiche se presente

        string customtitle = "";
        string customdesc = "";
        if (contentcollegato != null && contentcollegato.Id != 0)
        {
            custombind cb = new custombind();
            htmlPage = cb.bind(ReplaceAbsoluteLinks(ReplaceLinks(contentcollegato.DescrizionebyLingua(Lingua)).ToString()), Lingua, Page.User.Identity.Name, Session, null, null, Request);
            //if (htmlPage.Contains("injectPortfolioAndLoad")) JavaInjection = true;
            switch (Lingua.ToLower())
            {
                case "gb":
                    customdesc = contentcollegato.CustomdescGB;
                    customtitle = contentcollegato.CustomtitleGB;
                    break;
                case "ru":
                    customdesc = contentcollegato.CustomdescRU;
                    customtitle = contentcollegato.CustomtitleRU;
                    break;
                default:
                    customdesc = contentcollegato.CustomdescI;
                    customtitle = contentcollegato.CustomtitleI;
                    break;
            }

            /////////////////////////////////////////////
            ///META ROBOTS custom
            /////////////////////////////////////////////
            HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
            if (!string.IsNullOrEmpty(contentcollegato.Robots.Trim()))
                metarobots.Attributes["Content"] = contentcollegato.Robots.Trim();
        }
        else if (sezione != null) //metto l'h1 solo se non presente una pagina statica di modifica, presupponendo che l'h1 viene dato manualmente nella pagina statica correlata
        {
            ///////////////////////////////////////////////////////////////////
            ///////////////////////NOME PAGINA H1////////////////////////////////
            ///////////////////////////////////////////////////////////////////
            string titolopagina = sezione.Descrizione;
            litNomePagina.Text = titolopagina;
            //if (Tipologia != "rif000001" && Tipologia != "rif000002" && Tipologia != "rif000010")
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
            divTitleContainer.Visible = true; //accendo il titolo
        }
        /////////////////////////////////////////////////////////////
        //META TITLE E DESCRIPTION DEFAULT E CUSTOM ( da sezione admin )
        ////////////////////////////////////////////////////////////
        /////default meta
        string metametatitle = html.Convert(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione + " " + references.ResMan("Common", Lingua, "testoPosizionebase")) + " " + Nome);
        string description = "";
        description = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(htmlPage, 300, true)).Replace("<br/>", "\r\n")).Trim();
        //custom meta
        if (!string.IsNullOrEmpty(customtitle))
            metametatitle = (customtitle).Replace("<br/>", "\r\n");
        if (!string.IsNullOrEmpty(customdesc))
            description = customdesc.Replace("<br/>", "\r\n");
        description = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(description, 300, true)).Replace("<br/>", "\r\n")).Trim();
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
        List<Tabrif> links = new List<Tabrif>();
        bool usacategorie = true;
        //if (Tipologia == "rif000001") usacategorie = false;
        links = GeneraBreadcrumbPath(usacategorie);
        //links.Add(actualpagelink); //aggiungo la pagina attuale

        HtmlGenericControl ulbr = (HtmlGenericControl)Master.FindControl("ulBreadcrumb");
        ulbr.InnerHtml = BreadcrumbConstruction(links);
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
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == Tipologia); });
        if (item != null)
        {
            //1 livello tipologia
            linkurl = CreaLinkRoutes(null, false, Lingua, CleanUrl(item.Descrizione), "", Tipologia, "", "");
            link1 = new Tabrif();
            link1.Campo1 = linkurl;
            link1.Campo2 = item.Descrizione;

            //2 livello categoria
            if (!string.IsNullOrEmpty(Categoria) && usacategoria)
            {
                Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == Tipologia && tmp.CodiceProdotto == Categoria)); });
                if (catselected != null)
                {
                    linkurl = CreaLinkRoutes(null, false, Lingua, CleanUrl(catselected.Descrizione), "", Tipologia, Categoria, "");
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
                    linkurl = CreaLinkRoutes(null, false, Lingua, CleanUrl(categoriasprodotto.Descrizione), "", Tipologia, Categoria, Categoria2liv);
                    link3 = new Tabrif();
                    link3.Campo1 = linkurl;
                    link3.Campo2 = categoriasprodotto.Descrizione;
                }
            }

            //Customizzazione pagina copertina di navigazione sezione con pagine statiche ( HOME DI SEZIONE PERSONALIZZATE )
            if (Tipologia == "rif000001" || Tipologia == "rif000002")
            //if (Tipologia == "rif000003" || Tipologia == "rif000002")
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
                Prodotto catcopertina = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
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

    private void LoadJavascriptVariables()
    {
        ClientScriptManager cs = Page.ClientScript;

        String scriptRegVariables = string.Format("var testoricerca = '" + Server.HtmlEncode(testoricerca) + "';");

        if (!cs.IsClientScriptBlockRegistered("RegVariablesScriptPage"))
        {
            cs.RegisterClientScriptBlock(typeof(Page), "RegVariablesScriptPage", scriptRegVariables, true);
        }
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
    //protected void Cerca_Click(object sender, EventArgs e)
    //{
    //    if (Server.HtmlEncode(inputCerca.Value).Trim() == "") return;
    //    //testoricerca
    //    string link = CreaLinkRicerca("", Tipologia, "", "", "", "", "", "-", Lingua, Session, true);
    //    Session.Add("testoricerca", Server.HtmlEncode(inputCerca.Value)); //carico in sessione il parametro da cercare
    //    Response.Redirect(link);
    //}


}