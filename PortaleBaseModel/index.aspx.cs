using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;

public partial class index : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", true, deflanguage);
              //  CaricaControlliServerside();
                CaricaControlliJS();
                SettaTestoIniziale("Home");
                InizializzaSeo();
            //PulisciRegistrazionitemporanee();

            // se utilizzi le risorse abilita il databind
            //DataBind();
            }
        }
        catch (Exception err)
        {
            //   output.Text = err.Message;
        }
    }

    private void CaricaControlliServerside()
    {
        //Literal lit = null;
        // Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);


       

#if false //VIDEO PER TESTATA E FASCE

              //CARICO VIDEO IN TESTATA
                Literal lit = (Literal)Master.FindControl("literalVideoHigh");
                Master.CaricaVideoSection("TBL_BANNERS_GENERALE", 0, 0, "video-testata", false, lit, Lingua, "home");

                //CARICO VIDEO SU BANNER 
                lit = (Literal)Master.FindControl("literalVideoBanner");
                Master.CaricaVideoSection("TBL_BANNERS_GENERALE", 0, 0, "video-banner1", false, lit, Lingua, "module");


#endif
#if false
                //CARICO LA CATEGORIA ////////////////////////////////////////////////////////////////
                //List<OleDbParameter> parColl = new List<OleDbParameter>();
                //OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", "rif000001");
                //parColl.Add(p3);
                //OleDbParameter p7 = new OleDbParameter("@CodiceCategoria", "prod000031");
                //parColl.Add(p7);
                //OfferteCollection offcat1a = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "18", Lingua, false);
                ////Literal lit = (Literal)Master.FindControl("litPortfolio1");
                ////Master.CaricaContenutiPortfolio("", lit, Lingua, "3", offcat1a);
                //Literal lit = (Literal)Master.FindControl("litScroller1");
                //Master.CaricaUltimiPostScrollerTipo1(lit, null, "", Lingua, false, true, offcat1a, "20");
                //////////////////////////////////////////////////////////////////////////////////////////////////
                //CARICO LA CATEGORIA ////////////////////////////////////////////////////////////////
                //parColl = new List<OleDbParameter>();
                //p3 = new OleDbParameter("@CodiceTIPOLOGIA", "rif000001");
                //parColl.Add(p3);
                //p7 = new OleDbParameter("@CodiceCategoria", "prod000032");
                //parColl.Add(p7);
                //OfferteCollection offcat1b = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "18", Lingua, false);
                //lit = (Literal)Master.FindControl("litScroller2");
                //Master.CaricaUltimiPostScrollerTipo1(lit, null, "", Lingua, false, true, offcat1b, "20");
                //////////////////////////////////////////////////////////////////////////////////////////////////
 
		   lit = (Literal)Master.FindControl("litScroller1");
                parColl = new List<OleDbParameter>();
                OleDbParameter pcod1 = new OleDbParameter("@CodiceTIPOLOGIA", "rif000002,rif000003,rif000004,rif000005,rif000006,rif000007,rif000008,rif000009,rif000010");
                parColl.Add(pcod1);
                OleDbParameter pvet1 = new OleDbParameter("@Vetrina", true);
                parColl.Add(pvet1);
                list = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "18", Lingua, false);
               
 Master.CaricaUltimiPostScrollerTipo1(lit, null, "", Lingua, false, false, list, "");
#endif
        //lit = (Literal)Master.FindControl("litPortfolioBannersLateral");
        //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-destra", true, lit, Lingua, false, 8,5);
        //lit = (Literal)Master.FindControl("lblPlacebannerfascia1");
        ////Master.CaricaBannersStriscia("TBL_BANNERS_GENERALE", 0, 0, "banners-fascia1", false, lit,Lingua,"1");//Metodo con scroller e immagine ridimensionabile
        //Master.CaricaBannersFascia("TBL_BANNERS_GENERALE", 0, 0, "banners-fascia1", false, lit, Lingua); ;
        //lit = (Literal)Master.FindControl("lblPlacebannerfascia2");
        ////Master.CaricaBannersStriscia("TBL_BANNERS_GENERALE", 0, 0, "banners-fascia1", false, lit,Lingua,"1");//Metodo con scroller e immagine ridimensionabile
        //Master.CaricaBannersFascia("TBL_BANNERS_GENERALE", 0, 0, "banners-fascia2", false, lit, Lingua); ;
        //lit = (Literal)Master.FindControl("litPortfolioLow");
        //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-links", false, lit, Lingua);
        //lit = (Literal)Master.FindControl("litPortfolioBanners1");
        //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-siti", false, lit, Lingua, true);
        //List<OleDbParameter> parColl = new List<OleDbParameter>();
        //OleDbParameter pcod = new OleDbParameter("@CodiceTIPOLOGIA", "rif000001");
        //parColl.Add(pcod);
        //OleDbParameter pvet = new OleDbParameter("@Vetrina", true);
        //parColl.Add(pvet);
        //OfferteCollection list = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "18", Lingua, false);
        //Master.CaricaUltimiPostScrollerTipo1(lit, null, "", Lingua, false, true, list, "");
        //lit = (Literal)Master.FindControl("litScroller1");
        //Master.CaricaUltimiPostScrollerTipo1(lit, null, "rif000009", Lingua, false, true, null, "9");
        //lit = (Literal)Master.FindControl("litScroller3");
        //Master.CaricaUltimiPostScrollerTipo1(lit, null, "rif000002", Lingua, true, false, null, "9");
    }

    private void CaricaControlliJS()
    {
        //ClientScriptManager cs = Page.ClientScript;

        //LANDING IMAGES TESTS
        // FULL BANNER JARALLAX
        //string controllistsection1 = "injectandloadgenericbanner('bannerimagefull.html','divContainerSection', 'bansect1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','banners-section1',false);";
        //SMALL BANNER JARALLAX
        //string controllistsection1 = "injectandloadgenericbanner('bannerparallax.html','divContainerSection', 'bansect1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','banners-section1',false);";

        //Banner testate / revolution
#if false
        string controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,1000);";
        string controllistBanHome = "injectFasciaAndLoadBanner('bannerFascia3.html','divContainerBannerhome', 'bannerfasciahome', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia-home',false);"; 
#endif

        //Banner testate / cycle 
        //string controllistBanHeadcycle = "injectFasciaAndLoadBanner('bannerFascialarge.html','divContainerBannerHead', 'bannerfasciahead', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','header-home',false);";

        //SEZIONE BANNERS ( banners portfolio )
        //BANNER CON ISOTOPE PORTFOLIO ( PROMO DESTRA )
        //string controllistBan2 = "injectPortfolioAndLoadBanner('IsotopeBanner1.html','divJSIsotopeContainerBanner2', 'portlistBan1', 1, 1, false, '','10','','TBL_BANNERS_GENERALE','banner-destra',false);";

        //BANNERS A FASCIA COMPLETA
        //string controllistBan1 = "injectFasciaAndLoadBanner('bannerFascia.html','divJSIsotopeContainerBanner1', 'bannerfascia1', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia1',false);";
        //string controllistBan3 = "injectFasciaAndLoadBanner('bannerFascia.html','divJSIsotopeContainerBanner3', 'bannerfascia2', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia2',false);";

        //BANNER MEZZA FASCIA
        //string controllistBan1 = "injectFasciaAndLoadBanner('bannerFascia2.html','divContainerBanner1', 'bannerfascia1', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia1',false);";
        //string controllistBan2 = "injectFasciaAndLoadBanner('bannerFascia3.html','divContainerBanner2', 'bannerfascia2', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia2',false);";
        //string controllistBan3 = "injectFasciaAndLoadBanner('bannerFascia2.html','divContainerBanner3', 'bannerfascia3', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia3',false);";
        //string controllistBan4 = "injectFasciaAndLoadBanner('bannerFascia3.html','divContainerBanner4', 'bannerfascia4', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia4',false);";

        //SEZIONE CONTENUTO
        //TESTATA PORTFOLIO TIPO BANNER
        //string control6 = "injectPortfolioAndLoad(\"isotopeOfferte.html\",\"divJSIsotopeContainer1\", \"portfolio1\", 1, 6, false, \"\", \"rif000003,rif000004,rif000005,rif000006,rif000007,rif000008,rif000009\", \"\", true, false, 6);";

        //SEZIONE SCROLLET CONTENUTI
        //string control1 = "injectScrollerAndLoad(\"owlscrollerVini1.html\",\"divScrollerjs1\", \"carouselInject1\",\"\", \"rif000003\", \"\", false, true, 12);";
        //string control2 = "injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divScrollerjs2\", \"carouselInject2\",\"\", \"rif000002\", \"\", false, false, 12);";
        //string control1 = "injectScrollerAndLoad(\"owlscrollerOfferte.html\",\"divJSScrollerContainer2\", \"carouselInject1\",\"\", \"rif000002\", \"prod000006\", true, false, 6);";

        //VERSIONE PORTFOLIO DELLA RUBRICA
        //string control3 = "injectPortfolioAndLoad(\"isotopeOfferte3.html\",\"divJSIsotopeContainer2\", \"portfolio2\", 1, 2, false, \"\", \"rif000002\", \"prod000006\", true, false, 2);";
        //string controllist2 = "injectPortfolioAndLoad(\"isotopeProdotti1.html\",\"divJSIsotopeContainer2\", \"portvetrina\", 1, 20, false, \"\", \"rif000001\", \"\", false, true, \"16\",\"\",true);";
        //if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        //    cs.RegisterStartupScript(this.GetType(), "clist1", controllist2, true);

        //VERSIONE SCROLLER tipo 1 DELLE RUBRICHE
        //string control7 = "injectScrollerAndLoad(\"owlscrollerOfferte1.html\",\"divJSScrollerContainer5\", \"carouselInject5\",\"\", \"rif000009\", \"\", true, false, 6, 1);";
        //string control8 = "injectScrollerAndLoad(\"owlscrollerOfferte1.html\",\"divJSScrollerContainer6\", \"carouselInject6\",\"\", \"rif000004\", \"\", true, false, 6, 1);";
        //string control9 = "injectScrollerAndLoad(\"owlscrollerOfferte1.html\",\"divJSScrollerContainer7\", \"carouselInject7\",\"\", \"rif000005\", \"\", true, false, 6, 1);";
        //string control10 = "injectScrollerAndLoad(\"owlscrollerOfferte1.html\",\"divJSScrollerContainer8\", \"carouselInject8\",\"\", \"rif000007\", \"\", true, false, 6, 1);";
        //string control11 = "injectScrollerAndLoad(\"owlscrollerOfferte1.html\",\"divJSScrollerContainer9\", \"carouselInject9\",\"\", \"rif000006\", \"\", true, false, 6, 1);";
        //string control12 = "injectScrollerAndLoad(\"owlscrollerOfferte1.html\",\"divJSScrollerContainer10\", \"carouselInject10\",\"\", \"rif000008\", \"\", true, false, 6, 1);";

        //SELEZIONE CONTENUTI LATERALI ULTIMI POST RUBRICHE
        //string control5 = "injectPortfolioAndLoad(\"isotopeOfferte2.html\",\"divJSIsotopeContainer3\", \"portfolio3\", 1, 12, false, \"\", \"rif000004,rif000005,rif000006,rif000007,rif000008,rif000009\", \"\", true, false, 12);";
        //string control4 = "injectPortfolioAndLoad(\"isotopeOfferte.html\",\"divJSIsotopeContainer1\", \"portfolio1\", 1, 20, false, \"\", \"rif000003\", \"\", true, false, 12);";
 
        //if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        //{
            //cs.RegisterStartupScript(this.GetType(), "controllistBanHeadcycle", controllistBanHeadcycle, true);
            //cs.RegisterStartupScript(this.GetType(), "controllistBanHead", controllistBanHead, true);
            //cs.RegisterStartupScript(this.GetType(), "cbanhome", controllistBanHome, true);
            //cs.RegisterStartupScript(this.GetType(), "c1", control1, true);
            //cs.RegisterStartupScript(this.GetType(), "c2", control2, true);
            //cs.RegisterStartupScript(this.GetType(), "cban1", controllistBan1, true);
            //cs.RegisterStartupScript(this.GetType(), "cban2", controllistBan2, true);
            //cs.RegisterStartupScript(this.GetType(), "cban3", controllistBan3, true);
            //cs.RegisterStartupScript(this.GetType(), "cban4", controllistBan4, true);
            //cs.RegisterStartupScript(this.GetType(), "csect1", controllistsection1, true);
        //}
    }

    private void InizializzaSeo()
    {
        string linkcanonico = "~";
        linkcanonico = "~/" + Lingua + "/Home";
        Literal litgeneric = ((Literal)Master.FindControl("litgeneric"));
        litgeneric.Text = "<link rel=\"canonical\" href=\"" + ReplaceAbsoluteLinks(linkcanonico) + "\"/>";
    }

    private void SettaTestoIniziale(string sezione)
    {
        string htmlPage = "";
        if (references.ResMan("Common",Lingua,"Content" + sezione) != null)
            htmlPage = ReplaceLinks(references.ResMan("Common",Lingua,"Content" + sezione).ToString());
        litTextHeadPage.Text = htmlPage;
        string strigaperricerca = sezione;
        //strigaperricerca = Request.Url.AbsolutePath;
        //strigaperricerca = strigaperricerca.ToLower().Replace("index.aspx", "home");
        Contenuti content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
        if (content != null && content.Id != 0)
        {
            htmlPage = (ReplaceLinks(content.DescrizionebyLingua(Lingua)));

            litTextHeadPage.Text = ReplaceAbsoluteLinks(htmlPage);
        }
    }
}