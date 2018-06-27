using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
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

public partial class AspNetPages_RisultatiProdotti : CommonPage
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
    public string Annata
    {
        get { return ViewState["Annata"] != null ? (string)(ViewState["Annata"]) : ""; }
        set { ViewState["Annata"] = value; }
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

    int progressivosepara = 0;
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
                Mesefiltro = CaricaValoreMaster(Request, Session, "Mesefiltro", false);
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


                //Aggiungo la lingua al pager se presente nella querystring
                //PagerRisultati.NavigateUrl += "?Lingua=" + Lingua;
                //Impostiamo anche gli altri parametri di ricerca
                Tipologia = CaricaValoreMaster(Request, Session, "Tipologia", false);
                string tmp = "";
                tmp = CaricaValoreMaster(Request, Session, "mese", false);
                if (!string.IsNullOrEmpty(tmp)) mese = tmp;
                tmp = CaricaValoreMaster(Request, Session, "anno", false);
                if (!string.IsNullOrEmpty(tmp)) anno = tmp;

                tmp = CaricaValoreMaster(Request, Session, "testoricerca", false);
                if (!string.IsNullOrEmpty(tmp)) testoricerca = tmp;

                CaricaControlliJS();

                #region SEZIONE MASTERPAGE GESTIONE

                //if (!string.IsNullOrEmpty(Tipologia))
                //    Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, Tipologia, false, Lingua);
                //else
                //    Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);


                //Literal lit = (Literal)Master.FindControl("litPortfolioBanners1");
                //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-sezionicatalogo", false, lit, Lingua, true);


#if false
                Panel p = (Panel)Master.FindControl("pnlRicerca");
                if (p != null)
                {
                    p.Visible = true;
                    Master.CaricaDatiDdlCaratteristiche(Lingua, Caratteristica1, Caratteristica2, Caratteristica3, Caratteristica4, FasciaPrezzo, Vetrina);
                    Master.CaricaDdlOrdinamento(Ordinamento);
                }
                
#endif 
                //CAmbio colore alla barra in alto
                //HtmlGenericControl divBackMenu = (HtmlGenericControl)Master.FindControl("divBckMenu");
                //divBackMenu.Attributes["style"] = "background-color:#f56f28";
                //HtmlGenericControl divMobileUl = (HtmlGenericControl)Master.FindControl("mobilenav1");
                //divMobileUl.Attributes["style"] = "background-color:#f56f28";

                #endregion

                //In caso di richiesta specifica di una pagina dei risultati la prendo dalla querystring
                Pagina = CaricaValoreMaster(Request, Session, "Pagina", true, "1");
                //Pagina = "1";
                //int _p = 0;
                //if (int.TryParse(Pagina, out _p))
                //{ PagerRisultati.CurrentPage = _p; }

                //AssociaDati();

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
    private void SettaVisualizzazione()
    {
        string cattipo = Tipologia;
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        switch (Tipologia)
        {
            case "rif000001":

                if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";
                AssociaDatiSocial();
                ModificaFiltroJS(); //Inserisce nell'objfiltro della session i valori di filtraggio
                string svetrina = "";
                if (Vetrina) svetrina = "true";
                //string controllist2 = "injectPortfolioAndLoad(\"isotopeProdotti1.html\",\"divPortfolioList1\", \"portlist1\", 1, 16, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", false, true, \"\",\"" + testoricerca + "\", '" + svetrina + "','" + Promozioni + "', \"\", \"" + Categoria2liv + "\");";




                sb.Append("<div id=\"divPortfolioList\" class=\"inject\" params=\"");
                sb.Append("injectPortfolioAndLoad,isotopeProdotti1.html,divPortfolioList, portlist1, 1, 42, true, \'\', \'" + cattipo + "\', \'" + Categoria + "\', false, true, \'\',\'" + testoricerca + "\'," + svetrina + "','" + Promozioni + "', \"\", \"" + Categoria2liv + "\");");
                sb.Append("\"></div>");
                sb.Append("<div id=\"divPortfolioListPager\"></div>");
                placeholderrisultati.Text = custombind.bind(sb.ToString(), Lingua, Page.User.Identity.Name, Session);// sb.ToString();


                //SettaTestoIniziale();

                //CaricaMenuContenuti(1, 20, rptContenutiLink); //Carico la lista laterale link del blog 
                break;

            default:
                AssociaDati();
                break;
        }
    }
    private void ModificaFiltroJS()
    {
        //GESTIONE DEI FILTRI MEDIANTE LA SESSIONE ( RIPRENDO DALLA SESSIONE I VALORI PER IL FILTRO )
        Dictionary<string, string> objvalue = new Dictionary<string, string>();
        string sobjvalue = "";
        if (Session["objfiltro"] != null)
        {
            sobjvalue = Session["objfiltro"].ToString();
            objvalue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sobjvalue);
            if (objvalue == null) objvalue = new Dictionary<string, string>();
        }
        //AGIORNIAMO IL CONTENITORE OBJFILTRO USATO DALL'HANDLE PER IL FILTRO
        if (Promozioni != "")
        {
            objvalue["promozioni"] = Promozioni;
        }


        //ddlCaratteristica1
        //ddlRegione
        //ddlCategoria
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

        sobjvalue = Newtonsoft.Json.JsonConvert.SerializeObject(objvalue);
        Session.Add("objfiltro", sobjvalue);

    }
    public void CaricaControlliJS()
    {
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        //Carico la galleria in masterpage corretta
        //string controllistBanHead = "";
        //if (Tipologia == "")
        //    controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,300);";
        //else
        //    controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','" + Tipologia + "',false,2000,300);";

        if (string.IsNullOrEmpty(Tipologia))
        {
            //controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,1000);";

            sb.Clear();
            sb.Append("(function wait() {");
            sb.Append("  if (typeof injectSliderAndLoadBanner === \"function\")");
            sb.Append("    {");
            sb.Append("injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,300);");
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
            sb.Append("injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','" + Tipologia + "',false,2000,300);");
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
    private void AssociaDati()
    {
        //btnNext.Text = references.ResMan("Common",Lingua,"txtTastoNext").ToString();
        //btnPrev.Text = references.ResMan("Common",Lingua,"txtTastoPrev").ToString();

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
        string annata = Annata;
        string caratteristica1 = Caratteristica1;
        string caratteristica2 = Caratteristica2;
        string caratteristica3 = Caratteristica3;
        string caratteristica4 = Caratteristica4;
        string caratteristica5 = Caratteristica5;
        string ordinamento = Ordinamento;

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
            Fascediprezzo _selfascia = Utility.Fascediprezzo.Find(delegate (Fascediprezzo tmp) { return (tmp.Lingua == Lingua && tmp.Codice == fasciadiprezzo && tmp.CodiceTipologiaCollegata == tipologia); });
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

        if (annata != "" && annata != "0")
        {
            SQLiteParameter pannata = new SQLiteParameter("@Anno", annata);
            parColl.Add(pannata);
        }
        if (caratteristica1 != "" && caratteristica1 != "0")
        {
            SQLiteParameter pcaratteristica1 = new SQLiteParameter("@Caratteristica1", caratteristica1);
            parColl.Add(pcaratteristica1);
        }
        if (caratteristica2 != "" && caratteristica2 != "0")
        {
            SQLiteParameter pcaratteristica2 = new SQLiteParameter("@Caratteristica2", caratteristica2);
            parColl.Add(pcaratteristica2);
        }
        if (caratteristica3 != "" && caratteristica3 != "0")
        {
            SQLiteParameter pcaratteristica3 = new SQLiteParameter("@Caratteristica3", caratteristica3);
            parColl.Add(pcaratteristica3);
        }
        if (caratteristica4 != "" && caratteristica4 != "0")
        {
            SQLiteParameter pcaratteristica4 = new SQLiteParameter("@Caratteristica4", caratteristica4);
            parColl.Add(pcaratteristica4);
        }
        if (caratteristica5 != "" && caratteristica5 != "0")
        {
            SQLiteParameter pcaratteristica5 = new SQLiteParameter("@Caratteristica5", caratteristica5);
            parColl.Add(pcaratteristica5);
        }

        if (Vetrina != false)
        {
            SQLiteParameter pvetrina = new SQLiteParameter("@Vetrina", Vetrina);
            parColl.Add(pvetrina);
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
        if (Mesefiltro.Trim() != "")//|| anno.Trim() != "")
        {
            int _a = 0;
            int.TryParse(anno, out _a);
            int _m = 0;
            int.TryParse(Mesefiltro, out _m);
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
        //Prezzo //Data1

        offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "1000", Lingua, null, ordinamento);// "CodiceCategoria"
        //offerte.RemoveAll(c => (Convert.ToInt32((c.CodiceTipologia.Substring(3))) < 100)); //Togliamo i risultati del blog ( andrebbero tolti nel filtro a monte)

        //if (offerte != null && offerte.Count > 0)
        //    AssociaDatiSocial(offerte[0]);
        //SettaTestoIniziale();
        AssociaDatiSocial();

#endif
        #endregion

        Pager<Offerte> p = new Pager<Offerte>();
        if (offerte != null && offerte.Count > PagerRisultati.PageSize)
        {
            pnlPager.Visible = true;

        }
        else
            pnlPager.Visible = false;
        p = new Pager<Offerte>(offerte, true, this.Page, PageGuid + PagerRisultati.ClientID);
        PagerRisultati.TotalRecords = p.Count;
        //Se sono nel catalogo eventi cerco di impostare tra i risultati di ricerca la data odierna come data di partenza
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

    protected void AssociaDatiRepeater(List<Offerte> list)
    {
        progressivosepara = 1;
        switch (Tipologia)
        {
            //case "rif000001":
            //    Master.CaricaContenutiPortfolioRivalSubtext(Tipologia, litPortfolioRivals3b, Lingua, "", list, "", "1f809f");
            //    //  Master.CaricaContenutiPortfolioRivalBordered(Tipologia, litPortfolioRivals3b, Lingua, "", list);

            //    break;
            default:
                rptProdotti.DataSource = list;
                rptProdotti.DataBind();
                break;
        }
    }


    protected void AssociaDatiSocial()
    {
        Tabrif actualpagelink = new Tabrif();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////  PER CREAZIONE LINK CANONICI E ALTERNATE ///////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));
        string urlcanonico = "";
        string hreflang = "";
        //METTIAMO GLI ALTERNATE
        hreflang = " hreflang=\"it\" ";
        string linkcanonicoalt = CreaLinkRoutes(null, false, "I", CleanUrl(sezionedescrizioneI), "", Tipologia, Categoria, Categoria2liv);
        Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
        litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
        Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
        litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
        if (Lingua == "I")
        {
            urlcanonico = ReplaceAbsoluteLinks(linkcanonicoalt); litcanonic.Text = "<link rel=\"canonical\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>"; actualpagelink.Campo1 = ReplaceAbsoluteLinks(linkcanonicoalt);
            actualpagelink.Campo2 = CleanUrl(sezionedescrizioneI);
        }
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            linkcanonicoalt = CreaLinkRoutes(null, false, "GB", CleanUrl(sezionedescrizioneGB), "", Tipologia, Categoria, Categoria2liv);
            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (Lingua == "GB")
            {
                urlcanonico = ReplaceAbsoluteLinks(linkcanonicoalt); litcanonic.Text = "<link rel=\"canonical\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
                actualpagelink.Campo1 = ReplaceAbsoluteLinks(linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(sezionedescrizioneGB);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////DEFINIZIONE DEI TITOLI E CONTENUTI DI PAGINA ////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

        string htmlPage = "";
        //TEST CARICAMENTO TESTI DA RISORSE
        if (references.ResMan("Common", Lingua, "testo" + Tipologia) != null)
            htmlPage = references.ResMan("Common", Lingua, "testo" + Tipologia).ToString();
        if (references.ResMan("Common", Lingua, "testo" + Categoria) != null)
            htmlPage = references.ResMan("Common", Lingua, "testo" + Categoria).ToString();
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
        urlcanonico = urlcanonico.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "");
        Contenuti content = null;
        content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, urlcanonico);
        string customtitle = "";
        string customdesc = "";
        if (content != null && content.Id != 0)
        {
            htmlPage = custombind.bind(ReplaceAbsoluteLinks(ReplaceLinks(content.DescrizionebyLingua(Lingua))), Lingua, Page.User.Identity.Name, Session); //ReplaceAbsoluteLinks(ReplaceLinks(content.DescrizionebyLingua(Lingua)));
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
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////DEFINIZIONE DEI META DI PAGINA ////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        string metametatitle = html.Convert(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione + " " + references.ResMan("Common", Lingua, "testoPosizionebase")) + " " + Nome);
        string description = "";
        description = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(htmlPage, 300, true)).Replace("<br/>", "\r\n")).Trim();

        /////////////////////////////////////////////////////////////
        //MODIFICA PER TITLE E DESCRIPTION CUSTOM
        ////////////////////////////////////////////////////////////
        if (!string.IsNullOrEmpty(customtitle))
            metametatitle = (customtitle).Replace("<br/>", "\r\n");
        if (!string.IsNullOrEmpty(customdesc))
            description = customdesc.Replace("<br/>", "\r\n");
        ////////////////////////////////////////////////////////////
        //Opengraph per facebook
        ((HtmlMeta)Master.FindControl("metafbTitle")).Content = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione).Trim();
        ((HtmlMeta)Master.FindControl("metafbdescription")).Content = description;
        //////////////////////////////////////////
        ((HtmlTitle)Master.FindControl("metaTitle")).Text = metametatitle;
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = description;
        //////////////////////////////////////////////////////////////////////////
        litTextHeadPage.Text = ((htmlPage));


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////BREAD CRUMBS///////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        List<Tabrif> links = GeneraBreadcrumbPath(true);
        if (true) //Pagina copertina presente
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


        //TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == Tipologia); });
        //if (item != null)
        //{
        //    string testourl = item.Descrizione;
        //    Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == Tipologia && tmp.CodiceProdotto == Categoria)); });
        //    if (catselected != null && usacategoria)
        //        testourl = catselected.Descrizione;
        //    if (!string.IsNullOrEmpty(Categoria2liv))
        //    {
        //        SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
        //        if (categoriasprodotto != null && usacategoria)
        //        {
        //            testourl = categoriasprodotto.Descrizione;
        //        }
        //    }
        //    string tmpcategoria = Categoria;
        //    string tmpcategoria2liv = Categoria2liv;
        //    if (!usacategoria)
        //    {
        //        tmpcategoria = ""; tmpcategoria2liv = "";
        //    }


        //    string linkcanonicoalt = CreaLinkRoutes(null, false, Lingua, CleanUrl(testourl), "", Tipologia, tmpcategoria, tmpcategoria2liv);
        //    link = new Tabrif();
        //    link.Campo1 = linkcanonicoalt;
        //    link.Campo2 = testourl;
        //    links.Add(link);
        //}
        return links;
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
            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink + "high"));

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
    //    protected void ImgAnt_PreRender(object sender, EventArgs e)
    //    {
    //        int maxwidth = 465;
    //        int maxheight = 310;
    //        try
    //        {
    //#if true
    //            //Meglio testare prma se l'immagine esiste invece di fare try catch
    //            if (File.Exists(Server.MapPath(((Image)sender).ImageUrl)))
    //            {
    //                using (System.Drawing.Image tmp = System.Drawing.Image.FromFile(Server.MapPath(((Image)sender).ImageUrl)))
    //                {
    //                    if (tmp.Width >= tmp.Height)
    //                    {
    //                        ((Image)sender).Width = maxwidth;
    //                        int altezza = tmp.Height * maxwidth / tmp.Width;

    //                        if (altezza < maxheight)
    //                            ((Image)sender).Height = altezza;
    //                        else
    //                        {
    //                            //((HtmlGenericControl)(((Image)sender).Parent)).Attributes["style"] = "height:" + maxheight + "px;overflow: hidden; float: left; margin:  5px";
    //                            //((Image)sender).Height = maxheight;
    //                            //((Image)sender).Width = tmp.Width * maxheight / tmp.Height;


    //                        }

    //                    }
    //                    else
    //                    {
    //                        ((Image)sender).Height = maxheight;
    //                        int larghezza = tmp.Width * maxheight / tmp.Height;
    //                        if (larghezza < maxwidth)
    //                            ((Image)sender).Width = larghezza;
    //                        else
    //                        {
    //                            ((Image)sender).Width = maxwidth;
    //                            ((Image)sender).Height = tmp.Height * maxwidth / tmp.Width;
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {//File inesistente
    //                ((Image)sender).Width = maxwidth;
    //                ((Image)sender).Height = maxheight;
    //            }
    //#endif

    //        }
    //        catch
    //        { }


    //    }
    protected bool VerificaPresenzaPrezzo(object prezzo)
    {
        bool ret = false;
        if (prezzo != null && (double)prezzo != 0)
            ret = true;
        return ret;
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


    protected string ComponiUrl(object NomeAnteprima, string CodiceOfferta, string idOfferta)
    {

        string ritorno = "";
        if (NomeAnteprima != null && CodiceOfferta != "" && idOfferta != "")
        {
            ritorno = PercorsoFiles + "/" + CodiceOfferta + "/" + idOfferta;
            if (NomeAnteprima.ToString().ToLower().StartsWith("ant"))
                NomeAnteprima = NomeAnteprima.ToString().Remove(0, 3);
            ritorno += "/" + NomeAnteprima.ToString();
            //string anteprima = filemanage.ScalaImmagineImmobile(ritorno, NomeAnteprima.ToString(), Server);
            //if (!string.IsNullOrEmpty(anteprima))
            //    ritorno = anteprima;
        }
        return ritorno;

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


    //protected void btnInsertcart(object sender, EventArgs e)
    //{


    //    string Idtext = ((LinkButton)sender).CommandArgument.ToString();
    //    int idprodotto = 0;
    //    int.TryParse(Idtext, out idprodotto);

    //    string codTagCombined = hddTagCombined.Value;
    //    string q = CaricaQuantitaNelCarrello(Request, Session, idprodotto.ToString(), codTagCombined);
    //    int quantita = 0;
    //    int.TryParse(q, out quantita);
    //    quantita += 1;//Incremento
    //    AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name, codTagCombined);

    //    //QUI DEVI FARE L'AGGIORNAMENTO DEI RIEPILOGHI DEL CARRELLO NELLA MASTER!!!!->
    //    AggiornaVisualizzazioneDatiCarrello();
    //    AssociaDati();


    //}

    //private void AggiornaVisualizzazioneDatiCarrello()
    //{
    //    //  this.Master.VisualizzaCarrello();
    //}

    #region PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER
    protected void btnPrev_click(object sender, EventArgs e)
    {
        int pag = PagerRisultati.CurrentPage;
        pag++;
        if (pag > PagerRisultati.totalPages) pag = PagerRisultati.totalPages;
        Pagina = pag.ToString();
        //Session["Pagina"] = Pagina;
        AssociaDati();
    }

    protected void btnNext_click(object sender, EventArgs e)
    {

        int pag = PagerRisultati.CurrentPage;
        pag--;
        if (pag < 1) pag = 1;
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



    #endregion


}
