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
using Newtonsoft.Json;

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
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        InizializzaSeo();
        ModificaFiltroJS(); //Preparo il filtor con i parametri aggiuntivi di filtro

#if FALSE

        //////////////////////////////////////////////////////////////////////////////////////
        //Preparo il filtro con i parametri aggiuntivi di filtro ( usabile nel caso di filtro per la preparazione dei parametri da passare agli snippet di inject )
        //////////////////////////////////////////////////////////////////////////////////////
        Dictionary<string, string> addpars = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(Tipologia)) addpars.Add("tipologia", Tipologia);
        if (!string.IsNullOrEmpty(Categoria)) addpars.Add("categoria", Categoria);
        if (!string.IsNullOrEmpty(Categoria2liv)) addpars.Add("categoria2Liv", Categoria2liv);
        if (!string.IsNullOrEmpty(Caratteristica1)) addpars.Add("Caratteristica1", Caratteristica1);
        if (!string.IsNullOrEmpty(Caratteristica1)) addpars.Add("Caratteristica1", Caratteristica1);
        if (!string.IsNullOrEmpty(Caratteristica1)) addpars.Add("Caratteristica1", Caratteristica1);
        if (!string.IsNullOrEmpty(Caratteristica2)) addpars.Add("Caratteristica2", Caratteristica2);
        if (!string.IsNullOrEmpty(Caratteristica3)) addpars.Add("Caratteristica3", Caratteristica3);
        string addparsserialized = Newtonsoft.Json.JsonConvert.SerializeObject(addpars, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
        });
        string addpardsencoded = dataManagement.EncodeBase64String(addparsserialized);
        //////////////////////////////////////////////////////////////////////////////////////

#endif


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

                if (!string.IsNullOrEmpty(Categoria2liv) || (string.IsNullOrEmpty(Categoria2liv) && !string.IsNullOrEmpty(Categoria)) || (!string.IsNullOrEmpty(testoricerca)) || (!string.IsNullOrEmpty(Caratteristica1)) || (!string.IsNullOrEmpty(Caratteristica2)) || (!string.IsNullOrEmpty(Caratteristica3)))
                {
                    if (!string.IsNullOrEmpty(testoricerca)) litTextHeadPage.Text = "";


                    //INSERISCO UN TEMPLATE IN TESTA NELLE PAGINE LISTA DEL CATALOGO
                    //placeholderrisultatinocontainer.Text = CommonPage.HtmlfromteplateInject("customcontentsearchmodal-" + Lingua + ".html", Lingua, Page.User.Identity.Name, Session);


                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON
                    string svetrina = "";
                    //if (Vetrina) svetrina = "true";
                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopeProdotti1b.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\',\'" + svetrina + "\',\'" + Promozioni + "\',\'\', '" + Categoria2liv + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");



                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioProdotti4Card.html,divPortfolioList,portlist1, 1, 55,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',false,true,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'" + Categoria2liv + "\',\'\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");



                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }

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
                    if (!string.IsNullOrEmpty(Categoria2liv) || (string.IsNullOrEmpty(Categoria2liv) && !string.IsNullOrEmpty(Categoria)) || (!string.IsNullOrEmpty(testoricerca)))
                    {

                        if (!string.IsNullOrEmpty(testoricerca)) litTextHeadPage.Text = "";

                        if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

                        //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                        //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog3Card.html,divPortfolioList,portlist1, 1, 42,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',true,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                        //sb.Append("\"></div>");
                        //sb.Append("<div id=\"divPortfolioListPager\"></div>");

                        sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                        //sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioBlog3Card.html,divPortfolioList,portlist1, 1, 44,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',true,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                        sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioBlog3Card-masonry.html,divPortfolioList,portlist1, 1, 44,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',false,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
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

            case "rif000005":
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

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopeSinglerowAnimated-no-button.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");


                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioProdotti3Card-nocarrello-noprezzo.html,divPortfolioList,portlist1, 1, 42,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioProdotti3Card-fullimage.html,divPortfolioList,portlist1, 1, 42,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");


                    placeholderrisultatinocontainer.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
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

                    //NUOVO METODO CON INIZIALZIZATORE NEL FILE COMMON

                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopeSinglerowAnimated-no-button.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");


                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioSingleRow-mono.html,divPortfolioList,portlist1, 1, 42,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");


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
                    sb.Append("injectbootstrapportfolioandload,isotopeGallery1.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'");
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
            case "rif000012":

                column1.Visible = true;
                column1.Attributes["class"] = "col-12";
                column2.Visible = false;
                //column2.Attributes["class"] = "col-md-1 col-sm-1";
                column3.Attributes["class"] = "col-12 col-sm-3";
                column3.Visible = false;
                //ContaArticoliPerperiodo(Tipologia);
                //  Caricalinksrubriche(Tipologia); //arica la ddl con le sttocategorie
                if (!JavaInjection)
                {
                    divSearch.Visible = false;
                    if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";


                    //sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog3.html,divPortfolioList,portlist1, 1, 9,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',true,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");

                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioBlog3CardOverlay.html,divPortfolioList,portlist1, 1, 1,false,\'30\',\'" + cattipo + "\',\'" + Categoria + "\',false,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    placeholderrisultati.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();

                    CommonPage.CustomContentInject(((HtmlGenericControl)Master.FindControl("divfooter1")), "customcontent1-" + Lingua + ".html", Lingua, Page.User.Identity.Name, Session);

                }
                break;

            case "rif000051":
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
                    sb.Append("<div id=\"divPortfolioList\" class=\"inject container\" params=\"");
                    sb.Append("injectPortfolioAndLoad,isotopeRassegnastampa.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', true, false, \'\',\'" + testoricerca + "\'");
                    sb.Append("\"></div>");
                    sb.Append("<div id=\"divPortfolioListPager\"></div>");
                    //sb.Append("</div>");
                    placeholderrisultatinocontainer.Text = cb.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session, null, null, Request);// sb.ToString();
                }
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
                    //sb.Append("injectPortfolioAndLoad,isotopePortfolioBlog3.html,divPortfolioList,portlist1, 1, 9,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',true,false,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
                    //sb.Append("\"></div>");
                    //sb.Append("<div id=\"divPortfolioListPager\"></div>");


                    sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                    sb.Append("injectbootstrapportfolioandload,bootstrapPortfolioBlog3CardOverlay.html,divPortfolioList,portlist1, 1, 42,true,\'\',\'" + cattipo + "\',\'" + Categoria + "\',false,true,\'\',\'" + testoricerca + "\',\'\',\'\',\'\',\'\'");
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

        //if (Tipologia != "rif000003")
        // Session.Remove("objfiltro"); //Filtro modificatore che usa la sessione per selezionare i risultati visualizzati
    }

    /// <summary>
    /// preparazione variabile  objvalue per i filtri aggiuntivi, potrebbe essere rimosso l0uso della sessione passando i parametri nella chiamata dell'inject del controllo, ma non valorizza le ddl di filtraggio
    /// </summary>
    private void ModificaFiltroJS()
    {

        //GESTIONE DEI FILTRI MEDIANTE LA SESSIONE
        Dictionary<string, string> objvalue = new Dictionary<string, string>();
        string sobjvalue = "";

#if false  //non ricarico i filtri dalla sessione ma solo dai parametri passati con il request context TRAMITE URL ReWRITING
        if (Session["objfiltro"] != null)
        {
            sobjvalue = Session["objfiltro"].ToString();
            objvalue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sobjvalue);
            if (objvalue == null) objvalue = new Dictionary<string, string>();
        } 
#endif

        ////CONTROLLO CAMBIO TIPOLOGIA - RESETTO IL FILTRO AZZERANDO LA SESSION
        string prevtipologia = "";
        if (objvalue.ContainsKey("tipologia"))
        {
            prevtipologia = objvalue["tipologia"];
        }
        if (prevtipologia != Tipologia)
        {
            Session.Remove("objfiltro"); //Filtro modificatore che usa la sessione per selezionare i risultati visualizzati
            objvalue = new Dictionary<string, string>();
        }
        if (Tipologia != "")
        {
            objvalue["tipologia"] = Tipologia;
        }
        if (!string.IsNullOrEmpty(mese)) //
        {
            objvalue.Remove("mese");
            objvalue["mese"] = mese;
        }
        else
            objvalue.Remove("mese");
        if (!string.IsNullOrEmpty(anno)) //
        {
            objvalue.Remove("anno");
            objvalue["anno"] = anno;
        }
        else
            objvalue.Remove("anno");


#if true //Gestione modificatori filtro categoria 
        //( c'è un problema di non suotamento dei parametri categoria,sottocategorie e gli atri con i tools iniettatti in pagine non lista se non viene svuotata la session dopo il filtraggio !! !!!)
        //DOVREI AVER RISOLTO, azzeranso la variabile objfiltro in sessione subito dopo la ricerca ed inserendo i parametri aggiuntivi nella tabella di URLREWRITING in modo che siano nei contenuti della route ad ogni chiamata
        //usando questo sistema gli else degli if sotto dovrebbero tutti essere eliminabili

        //if (Tipologia == "rif000001")
        //{
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
            objvalue["Regione"] = Regione;
            objvalue["ddlRegione"] = Regione;
            objvalue["ddlRegioneSearch"] = Regione;
        }
        else
        {
            if (objvalue.ContainsKey("ddlRegione"))
            {
                objvalue["Regione"] = objvalue["ddlRegione"];
                Regione = objvalue["ddlRegione"];
            }
            if (objvalue.ContainsKey("ddlRegioneSearch"))
            {
                objvalue["Regione"] = objvalue["ddlRegioneSearch"];
                Regione = objvalue["ddlRegioneSearch"];
            }
        }
        if (Provincia != "")
        {
            objvalue["Provincia"] = Provincia;
            objvalue["ddlProvincia"] = Provincia;
            objvalue["ddlProvinciaSearch"] = Provincia;
        }
        else
        {
            if (objvalue.ContainsKey("ddlProvincia"))
            {
                objvalue["Provincia"] = objvalue["ddlProvincia"];
                Provincia = objvalue["ddlProvincia"];
            }
            if (objvalue.ContainsKey("ddlProvinciaSearch"))
            {
                objvalue["Provincia"] = objvalue["ddlProvinciaSearch"];
                Provincia = objvalue["ddlProvinciaSearch"];
            }
        }
        if (Comune != "")
        {
            objvalue["Comune"] = Comune;
            objvalue["ddlComune"] = Comune;
            objvalue["ddlComuneSearch"] = Comune;
        }
        else
        {
            if (objvalue.ContainsKey("ddlComune"))
            {
                objvalue["Comune"] = objvalue["ddlComune"];
                Comune = objvalue["ddlComune"];
            }
            if (objvalue.ContainsKey("ddlComuneSearch"))
            {
                objvalue["Comune"] = objvalue["ddlComuneSearch"];
                Comune = objvalue["ddlComuneSearch"];
            }
        }
        if (Caratteristica1 != "")
        {
            objvalue["Caratteristica1"] = Caratteristica1;
            objvalue["hidCaratteristica1"] = Caratteristica1;
            objvalue["ddlCaratteristica1"] = Caratteristica1;
        }
        else
        {
            if (objvalue.ContainsKey("hidCaratteristica1"))
            {
                objvalue["Caratteristica1"] = objvalue["hidCaratteristica1"];
                Caratteristica1 = objvalue["hidCaratteristica1"];
            }
            if (objvalue.ContainsKey("ddlCaratteristica1"))
            {
                objvalue["Caratteristica1"] = objvalue["ddlCaratteristica1"];
                Caratteristica1 = objvalue["ddlCaratteristica1"];
            }

        }
        if (Caratteristica2 != "")
        {
            objvalue["Caratteristica2"] = Caratteristica2;
            objvalue["hidCaratteristica2"] = Caratteristica2;
        }
        else
        {
            if (objvalue.ContainsKey("hidCaratteristica2"))
            {
                objvalue["Caratteristica2"] = objvalue["hidCaratteristica2"];
                Caratteristica2 = objvalue["hidCaratteristica2"];
            }
        }
        if (Caratteristica3 != "")
        {
            objvalue["Caratteristica3"] = Caratteristica3;
            objvalue["hidCaratteristica3"] = Caratteristica3;
        }
        else
        {
            if (objvalue.ContainsKey("hidCaratteristica3"))
            {
                objvalue["Caratteristica3"] = objvalue["hidCaratteristica3"];
                Caratteristica3 = objvalue["hidCaratteristica3"];
            }
        }
        //Id selection
        //hidricercaid ( sleezione con id )
        if (objvalue.ContainsKey("hidricercaid"))
        {
            objvalue["id"] = objvalue["hidricercaid"];
        }
        //}
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
        Session.Add("objfiltro", sobjvalue); //Metto i valori di filtraggio in sessione per usarli nel custombind delle liste risultati
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
        //Eventuali parametri filtro aggiuntivo caratteristiche
        Dictionary<string, string> addpars = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(Caratteristica1)) addpars.Add("Caratteristica1", Caratteristica1);
        if (!string.IsNullOrEmpty(Caratteristica2)) addpars.Add("Caratteristica2", Caratteristica2);
        if (!string.IsNullOrEmpty(Caratteristica3)) addpars.Add("Caratteristica3", Caratteristica3);

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
        if (!string.IsNullOrEmpty(Caratteristica1))
            sezionedescrizioneI += " " + references.TestoCaratteristica(0, Caratteristica1, "I");
        if (!string.IsNullOrEmpty(Caratteristica2))
            sezionedescrizioneI += " " + references.TestoCaratteristica(1, Caratteristica2, "I");
        if (!string.IsNullOrEmpty(Caratteristica3))
            sezionedescrizioneI += " " + references.TestoCaratteristica(2, Caratteristica3, "I");


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
        if (!string.IsNullOrEmpty(Caratteristica1))
            sezionedescrizioneGB += " " + references.TestoCaratteristica(0, Caratteristica1, "GB");
        if (!string.IsNullOrEmpty(Caratteristica2))
            sezionedescrizioneGB += " " + references.TestoCaratteristica(1, Caratteristica2, "GB");
        if (!string.IsNullOrEmpty(Caratteristica3))
            sezionedescrizioneGB += " " + references.TestoCaratteristica(2, Caratteristica3, "GB");

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
        if (!string.IsNullOrEmpty(Caratteristica1))
            sezionedescrizioneRU += " " + references.TestoCaratteristica(0, Caratteristica1, "RU");
        if (!string.IsNullOrEmpty(Caratteristica2))
            sezionedescrizioneRU += " " + references.TestoCaratteristica(1, Caratteristica2, "RU");
        if (!string.IsNullOrEmpty(Caratteristica3))
            sezionedescrizioneRU += " " + references.TestoCaratteristica(2, Caratteristica3, "RU");

        ///FR/////
        WelcomeLibrary.DOM.TipologiaOfferte sezioneFR = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "FR" & tmp.Codice == Tipologia); });
        string sezionedescrizioneFR = "";
        if (sezioneFR != null)
            sezionedescrizioneFR = sezioneFR.Descrizione;
        if (!string.IsNullOrEmpty(Categoria))
        {
            Prodotto categoriaprodotto = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == "FR");
            if (categoriaprodotto != null)
            {
                sezionedescrizioneFR = categoriaprodotto.Descrizione;
            }
        }
        if (!string.IsNullOrEmpty(Categoria2liv))
        {
            SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "FR" && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
            if (categoriasprodotto != null)
            {
                sezionedescrizioneFR = categoriasprodotto.Descrizione;
            }
        }
        if (!string.IsNullOrEmpty(Caratteristica1))
            sezionedescrizioneFR += " " + references.TestoCaratteristica(0, Caratteristica1, "FR");
        if (!string.IsNullOrEmpty(Caratteristica2))
            sezionedescrizioneFR += " " + references.TestoCaratteristica(1, Caratteristica2, "FR");
        if (!string.IsNullOrEmpty(Caratteristica3))
            sezionedescrizioneFR += " " + references.TestoCaratteristica(2, Caratteristica3, "FR");



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////// ALTERNATE E CANONICAL /////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        string linki = ReplaceAbsoluteLinks(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("I", CleanUrl(sezionedescrizioneI), "", Tipologia, Categoria, Categoria2liv, "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars));
        string linken = ReplaceAbsoluteLinks(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("GB", CleanUrl(sezionedescrizioneGB), "", Tipologia, Categoria, Categoria2liv, "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars));
        string linkru = ReplaceAbsoluteLinks(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("RU", CleanUrl(sezionedescrizioneRU), "", Tipologia, Categoria, Categoria2liv, "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars));
        string linkfr = ReplaceAbsoluteLinks(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("FR", CleanUrl(sezionedescrizioneFR), "", Tipologia, Categoria, Categoria2liv, "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars));

        contentcollegato = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linki.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, ""));
        if ((contentcollegato == null || contentcollegato.Id == 0) && WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
            contentcollegato = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linken.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, ""));
        if ((contentcollegato == null || contentcollegato.Id == 0) && WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
            contentcollegato = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkru.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, ""));
        if ((contentcollegato == null || contentcollegato.Id == 0) && WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() == "true")
            contentcollegato = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkfr.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, ""));

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

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatei").ToLower() == "true")
        {
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
                //if (contentcollegato != null && string.IsNullOrEmpty(contentcollegato.CanonicalbyLingua("I").Trim())) // redirect solo se vuoto il campo di forzatura del canonical
                //    if (!(!string.IsNullOrEmpty(mese) && !string.IsNullOrEmpty(anno))) //controllo filtro archivio nel qual caso non faccio redirect
                //        if (!System.Web.HttpContext.Current.Request.Url.ToString().EndsWith("-")) // non faccio redirec neppure per gli indizizzi con ricerca!!! ( finicono in - )
                //        {
                //            Response.RedirectPermanent(modcanonical, true);
                //        }
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
            Literal litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
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
                    //if (contentcollegato != null && string.IsNullOrEmpty(contentcollegato.CanonicalbyLingua("GB").Trim())) // redirect solo se vuoto il campo di forzatura del canonical
                    //    if (!(!string.IsNullOrEmpty(mese) && !string.IsNullOrEmpty(anno))) //controllo filtro archivio nel qual caso non faccio redirect
                    //        if (!System.Web.HttpContext.Current.Request.Url.ToString().EndsWith("-")) // non faccio redirec neppure per gli indizizzi con ricerca!!! ( finicono in - )
                    //        {
                    //            Response.RedirectPermanent(modcanonical, true);
                    //        }
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
            Literal litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
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
                    //if (contentcollegato != null && string.IsNullOrEmpty(contentcollegato.CanonicalbyLingua("RU").Trim())) // redirect solo se vuoto il campo di forzatura del canonical
                    //    if (!(!string.IsNullOrEmpty(mese) && !string.IsNullOrEmpty(anno))) //controllo filtro archivio nel qual caso non faccio redirect
                    //        if (!System.Web.HttpContext.Current.Request.Url.ToString().EndsWith("-")) // non faccio redirec neppure per gli indizizzi con ricerca!!! ( finicono in - )
                    //        {
                    //            Response.RedirectPermanent(modcanonical, true);
                    //        }
                }
            }
        }
        else linkru = "";


        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() == "true")
        {
            hreflang = " hreflang=\"da\" ";
            //Leggiamo se presente un contenuto collegato nelle statiche
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkfr = linkfr.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainfr"));

            //FORZATURA CANONICAL utente
            modcanonical = linkfr;
            if (!string.IsNullOrEmpty(querystring))
                modcanonical += querystring;
            if (contentcollegato != null && !string.IsNullOrEmpty(contentcollegato.CanonicalbyLingua("FR").Trim()))
                modcanonical = (contentcollegato.CanonicalbyLingua("FR").Trim());
            //alternate
            Literal litgenericalt = ((Literal)Master.FindControl("litgeneric4"));
            if (!string.IsNullOrEmpty(CleanUrl(sezionedescrizioneFR)))
                litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "FR")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>";
            }
            if (Lingua.ToLower() == "fr")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
                actualpagelink.Campo1 = (linkfr);
                actualpagelink.Campo2 = CleanUrl(sezionedescrizioneFR);
                //redirect al canonical se il canonical non coincide con l'url
                if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
                {
                    HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
                    metarobots.Attributes["Content"] = "noindex,follow";
                    //if (string.IsNullOrEmpty(contentcollegato.CanonicalbyLingua("FR").Trim())) // redirect solo se vuoto il campo di forzatura del canonical
                    //    if (!(!string.IsNullOrEmpty(mese) && !string.IsNullOrEmpty(anno))) //controllo filtro archivio nel qual caso non faccio redirect
                    //        if (!System.Web.HttpContext.Current.Request.Url.ToString().EndsWith("-")) // non faccio redirec neppure per gli indizizzi con ricerca!!! ( finicono in - )
                    //        {
                    //            Response.RedirectPermanent(modcanonical, true);
                    //        }
                }
            }
        }
        else linkfr = "";




        //SET LINK PER CAMBIO LINGUA
        SettaLinkCambioLingua(linki, sezionedescrizioneI, linken, sezionedescrizioneGB, linkru, sezionedescrizioneRU, linkfr, sezionedescrizioneFR);



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////SEZIONE META TITLE E DESC E CONTENUTO HEADER/h1 PAGINA ///////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();
        WelcomeLibrary.DOM.TipologiaOfferte sezione = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua & tmp.Codice == Tipologia); });
        string sezionedescrizione = "";
        if (sezione != null)
            sezionedescrizione += " " + sezione.Descrizione;
        if (!string.IsNullOrEmpty(Categoria))
        {
            Prodotto categoriaprodotto = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == Tipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
            if (categoriaprodotto != null)
            {
                sezionedescrizione += " " + categoriaprodotto.Descrizione;
            }
        }
        if (!string.IsNullOrEmpty(Categoria2liv))
        {
            SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
            if (categoriasprodotto != null)
            {
                sezionedescrizione += " " + categoriasprodotto.Descrizione;
            }
        }
        if (!string.IsNullOrEmpty(Caratteristica1))
            sezionedescrizione += " " + references.TestoCaratteristica(0, Caratteristica1, Lingua);
        if (!string.IsNullOrEmpty(Caratteristica2))
            sezionedescrizione += " " + references.TestoCaratteristica(1, Caratteristica2, Lingua);
        if (!string.IsNullOrEmpty(Caratteristica3))
            sezionedescrizione += " " + references.TestoCaratteristica(2, Caratteristica3, Lingua);


        ////////EVIDENZIAZIONE MENU//////////////////////////////////////////////////////////////
        if (sezione != null)
            EvidenziaSelezione(sezione.Descrizione); // Serve Solo per la voce al top dei dropdown n non per i link finali ....


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
                case "i":
                    customdesc = contentcollegato.CustomdescI;
                    customtitle = contentcollegato.CustomtitleI;
                    break;
                case "FR":
                    customdesc = contentcollegato.CustomdescFR;
                    customtitle = contentcollegato.CustomtitleFR;
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
            if (!string.IsNullOrEmpty(Caratteristica1))
                litNomePagina.Text += " " + references.TestoCaratteristica(0, Caratteristica1, Lingua);
            if (!string.IsNullOrEmpty(Caratteristica2))
                litNomePagina.Text += " " + references.TestoCaratteristica(1, Caratteristica2, Lingua);
            if (!string.IsNullOrEmpty(Caratteristica3))
                litNomePagina.Text += " " + references.TestoCaratteristica(2, Caratteristica3, Lingua);

            ///////////////////////////////////////////////////////////////
            divTitleContainer.Visible = true; //accendo il titolo
        }
        /////////////////////////////////////////////////////////////
        //META TITLE E DESCRIPTION DEFAULT E CUSTOM ( da sezione admin )
        ////////////////////////////////////////////////////////////
        /////default meta
        //string metametatitle = html.Convert(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione + " " + references.ResMan("Common", Lingua, "testoPosizionebase")) + " " + Nome); // agingo semrpre posizione e nome ....
        string metametatitle = html.Convert(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione));
        string description = "";
        description = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(htmlPage, 300, true)).Replace("<br/>", "\r\n")).Trim();
        //custom meta
        if (!string.IsNullOrEmpty(customtitle))
            metametatitle = (customtitle).Replace("<br/>", "\r\n");
        if (!string.IsNullOrEmpty(customdesc))
            description = customdesc.Replace("<br/>", "\r\n");
        description = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(description, 300, true)).Replace("<br/>", "\r\n")).Trim();
        if (string.IsNullOrEmpty(description))
            description = metametatitle + " " + references.ResMan("Common", Lingua, "testoPosizionebase") + " " + Nome;

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
            case "dk":
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
        Dictionary<string, string> addpars = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(Caratteristica1)) addpars.Add("Caratteristica1", Caratteristica1);
        if (!string.IsNullOrEmpty(Caratteristica2)) addpars.Add("Caratteristica2", Caratteristica2);
        if (!string.IsNullOrEmpty(Caratteristica3)) addpars.Add("Caratteristica3", Caratteristica3);



        link = new Tabrif();
        link.Campo1 = ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkHome"));
        link.Campo2 = references.ResMan("Common", Lingua, "testoHome");
        links.Add(link);
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == Tipologia); });
        if (item != null)
        {
            //1 livello tipologia ( item.descrizione , andrebbe modificato se presenti addpars !!!! aggiungendo nel testo.... )
            string testosezione = item.Descrizione;
            if (!string.IsNullOrEmpty(Caratteristica1))
                testosezione += " " + references.TestoCaratteristica(0, Caratteristica1, "I");
            if (!string.IsNullOrEmpty(Caratteristica2))
                testosezione += " " + references.TestoCaratteristica(1, Caratteristica2, "I");
            if (!string.IsNullOrEmpty(Caratteristica3))
                testosezione += " " + references.TestoCaratteristica(2, Caratteristica3, "I");
            ///
            linkurl = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CleanUrl(testosezione), "", Tipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars);
            link1 = new Tabrif();
            link1.Campo1 = linkurl;
            link1.Campo2 = testosezione;


            //2 livello categoria
            if (!string.IsNullOrEmpty(Categoria) && usacategoria)
            {
                Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == Tipologia && tmp.CodiceProdotto == Categoria)); });
                if (catselected != null)
                {
                    linkurl = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CleanUrl(catselected.Descrizione), "", Tipologia, Categoria, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars);
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
                    linkurl = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CleanUrl(categoriasprodotto.Descrizione), "", Tipologia, Categoria, Categoria2liv, "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl, addpars);
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
                        link1.Campo1 = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(contentpertipologia.TitolobyLingua(Lingua)), contentpertipologia.Id.ToString(), "con001000", "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl); ;
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
                        link2.Campo1 = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(contentpercategoria.TitolobyLingua(Lingua)), contentpercategoria.Id.ToString(), "con001000", "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
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
                        link3.Campo1 = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(contentpersottocategoria.TitolobyLingua(Lingua)), contentpersottocategoria.Id.ToString(), "con001000", "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
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