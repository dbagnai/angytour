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

public partial class AspNetPages_pwalist : CommonPage
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
                Categoria = CaricaValoreMaster(Request, Session, "Categoria", false);
                Giornofiltro = CaricaValoreMaster(Request, Session, "Giornofiltro", false);
                Categoria2liv = CaricaValoreMaster(Request, Session, "Categoria2liv", false);
                if (Categoria2liv == "all") Categoria2liv = "";

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

                //Impostiamo anche gli altri parametri di ricerca
                Tipologia = CaricaValoreMaster(Request, Session, "Tipologia", false, "");
                string tmp = "";
                tmp = CaricaValoreMaster(Request, Session, "mese", false);
                if (!string.IsNullOrEmpty(tmp)) mese = tmp;
                tmp = CaricaValoreMaster(Request, Session, "anno", false);
                if (!string.IsNullOrEmpty(tmp)) anno = tmp;
                tmp = CaricaValoreMaster(Request, Session, "testoricerca", false);
                if (!string.IsNullOrEmpty(tmp)) testoricerca = tmp;


                LoadJavascriptVariables();
                SettaVisualizzazione();

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

    private void SettaVisualizzazione()
    {
        string cattipo = Tipologia;
        ClientScriptManager cs = Page.ClientScript;
        Literal lit = null;
        ModificaFiltroJS();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        switch (Tipologia)
        {
            case "rif000501": //Simil prodotti x app
                if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                AssociaDatiSocial();

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
                sb.Append("injectPortfolioAndLoad,isotopeProdotti1b.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'," + svetrina + "','" + Promozioni + "', \"\", \"" + Categoria2liv + "\");");
                sb.Append("\"></div>");
                sb.Append("<div id=\"divPortfolioListPager\"></div>");
                placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                break;
            case "rif000505":

                AssociaDatiSocial();

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
                    sb.Append("injectPortfolioAndLoad,isotopeOffertePortfolio.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000503":
                AssociaDatiSocial();

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
                    sb.Append("injectPortfolioAndLoad,isotopePWASinglerowAnimated.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //sb.Append("</div>");
                    placeholderrisultatinocontainer.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000504":
                AssociaDatiSocial();

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
                    sb.Append("injectPortfolioAndLoad,isotopePWASinglerow-offerte.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //sb.Append("</div>");
                    placeholderrisultatinocontainer.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000506":
                AssociaDatiSocial();

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
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000507":

                AssociaDatiSocial();

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
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;

            case "rif000508":
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
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog2.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //placeholderrisultati.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session,null,null,Request);

                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog-no-image.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                    //sb.Clear();
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session,null,null,Request);
                }
                break;

            case "rif000509":

                AssociaDatiSocial();

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
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog
                break;
            case "rif000502":
                AssociaDatiSocial();
                PreselezionaCategoria();

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-12 col-sm-3";
                column3.Visible = false;
                //ContaArticoliPerperiodo(Tipologia);
                //  Caricalinksrubriche(Tipologia); //arica la ddl con le sttocategorie
                divSearch.Visible = false;
                if (!JavaInjection)
                {
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog2.html,divPortfolioList, portlist1, 1, 24, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //placeholderrisultati.Text = sb.ToString();

                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopePWAPortfolioBlog3.html,divPortfolioList,portlist1, 1, 9,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',true,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                    //sb.Clear();
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session,null,null,Request);
                }
                break;
            default:
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
                    placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                    //sb.Clear();
                    //sb.Append("<div class=\"sfondo-contenitore\">");
                    //sb.Append("<div id=\"divPortfolioLateralTitle\" class=\"title-style1\">" + references.ResMan("basetext", Lingua, "testopanel1") + "</div>");
                    //sb.Append("<div id=\"divPortfolioLateral\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioSingleRowSmall.html,divPortfolioLateral, portlist2, 2, 24, 'skip', \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, 8,\'" + testoricerca + "\', \'\', \'\', \'\', \'" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioLateralPager\"></div>");
                    //sb.Append("</div>");
                    //placeholderlateral.Text =  custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session,null,null,Request);
                }
                break;
        }
    }
    protected void PreselezionaCategoria()
    {
        if (Categoria == "")
        {
            List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == Tipologia)); });

            if (prodotti != null && prodotti.Count > 0)
            {
                prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
                Categoria = prodotti[0].CodiceProdotto;
            }
        }
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
        string urlcambiolinguaenit = "";
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
        else urlcambiolinguaenit = linkcanonicoalt;
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
            else urlcambiolinguaenit = linkcanonicoalt;
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
            //if (htmlPage.Contains("injectPortfolioAndLoad")) JavaInjection = true;
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
        if (Tipologia == "rif000501") //Pagina copertina presente
        {
            if (sezione != null && !string.IsNullOrEmpty(sezione.Descrizione.ToLower().Trim()))
            {
                Contenuti contentpertipologia = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "categorie " + sezione.Descrizione.ToLower().Trim());
                if (contentpertipologia != null && contentpertipologia.Id != 0)
                {
                    Tabrif laddink = new Tabrif();
                    laddink.Campo1 = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(contentpertipologia.TitolobyLingua(Lingua)), contentpertipologia.Id.ToString(), "con001000");
                    laddink.Campo2 = contentpertipologia.TitolobyLingua(Lingua);
                    links.Add(laddink);
                }
            }

            //Prodotto catcopertina = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
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
                Categoria = objvalue["ddlCategoria"];
            }
        }

        if (Regione != "")
        {
            objvalue["regione"] = Regione;
            objvalue["ddlRegione"] = Regione;
        }
        else
        {
            if (objvalue.ContainsKey("ddlRegione"))
            {
                objvalue["regione"] = objvalue["ddlRegione"];
                Regione = objvalue["ddlRegione"];
            }
        }
        if (Caratteristica1 != "")
        {
            objvalue["caratteristica1"] = Caratteristica1;
            objvalue["hidCaratteristica1"] = Caratteristica1;
        }
        else
        {
            if (objvalue.ContainsKey("hidCaratteristica1"))
            {
                objvalue["caratteristica1"] = objvalue["hidCaratteristica1"];
                Caratteristica1 = objvalue["hidCaratteristica1"];
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
    protected void Cerca_Click(object sender, EventArgs e)
    {
        if (Server.HtmlEncode(inputCerca.Value).Trim() == "") return;
        //testoricerca
        string link = CreaLinkRicerca("", Tipologia, "", "", "", "", "", "-", Lingua, Session, true);
        Session.Add("testoricerca", Server.HtmlEncode(inputCerca.Value)); //carico in sessione il parametro da cercare
        Response.Redirect(link);
    }


}