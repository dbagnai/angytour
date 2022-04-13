using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

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
                Session.Remove("objfiltro"); //Elimino Filtro modificatore che usa la sessione per selezionare i risultati visualizzati
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", true, deflanguage);
                InizializzaSeo("home");

                //CaricaControlliServerside();
                //CaricaControlliJS();
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

    private void InizializzaSeo(string sezione)
    {
        string htmlPage = "";
        if (references.ResMan("Common", Lingua, "Content" + sezione) != null)
            htmlPage = ReplaceLinks(references.ResMan("Common", Lingua, "Content" + sezione).ToString());
        litTextHeadPage.Text = htmlPage;
        string strigaperricerca = sezione;
        Contenuti content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
        if (content == null) return;

        string customdesc = "";
        string customtitle = "";
        if (content != null && content.Id != 0)
        {
            /////////////////////////////////////////////
            //Inserisco i Contenuti di Pagina con prerender !!!
            /////////////////////////////////////////////
            htmlPage = (ReplaceLinks(content.DescrizionebyLingua(Lingua)));
            custombind cb = new custombind();
            htmlPage = cb.bind(htmlPage, Lingua, User.Identity.Name, HttpContext.Current.Session, null, null, Request); //eseguiamo il bindig delle funzioni lato server per quelle identificabili
            litTextHeadPage.Text = ReplaceAbsoluteLinks(htmlPage);

            /////////////////////////////////////////////
            ///META TITLE E DESCRIPTION  ////////////////
            /////////////////////////////////////////////
            //string descrizione = content.DescrizioneI;
            //string titolopagina = content.TitoloI;
            switch (Lingua)
            {
                case "GB":
                    //descrizione = content.DescrizioneGB;
                    //titolopagina = content.TitoloGB;
                    customdesc = content.CustomdescGB;
                    customtitle = content.CustomtitleGB;
                    break;
                case "RU":
                    //descrizione = content.DescrizioneRU;
                    //titolopagina = content.TitoloRU;
                    customdesc = content.CustomdescRU;
                    customtitle = content.CustomtitleRU;
                    break;
                case "I":
                    customdesc = content.CustomdescI;
                    customtitle = content.CustomtitleI;
                    break;
                case "FR":
                    customdesc = content.CustomdescFR;
                    customtitle = content.CustomtitleFR;
                    break;
                case "DE":
                    customdesc = content.CustomdescDE;
                    customtitle = content.CustomtitleDE;
                    break;
                case "ES":
                    customdesc = content.CustomdescES;
                    customtitle = content.CustomtitleES;
                    break;
            }
            if (!string.IsNullOrEmpty(customtitle))
                ((HtmlTitle)Master.FindControl("metaTitle")).Text = (customtitle).Replace("<br/>", "\r\n");
            if (!string.IsNullOrEmpty(customdesc))
                ((HtmlMeta)Master.FindControl("metaDesc")).Content = customdesc.Replace("<br/>", "\r\n");

            /////////////////////////////////////////////////
            ////Opengraph per facebook //////////////////////
            /////////////////////////////////////////////////
            ((HtmlMeta)Master.FindControl("metafbTitle")).Content = (customtitle).Replace("<br/>", "\r\n"); ;
            ((HtmlMeta)Master.FindControl("metafbdescription")).Content = customdesc.Replace("<br/>", "\r\n");

        }

        /////////////////////////////////////////////
        ///META ROBOTS custom ///////////////////////
        /////////////////////////////////////////////
        HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
        if (!string.IsNullOrEmpty(content.Robots.Trim()))
            metarobots.Attributes["Content"] = content.Robots.Trim();

        /////////////////////////////////////////////
        ///CANONICAL e ALTERNATE
        /////////////////////////////////////////////
        Literal litgeneric = ((Literal)Master.FindControl("litgeneric"));
        string hreflang = "";
        string linki = "";
        string linken = "";
        string linkru = "";
        string linkfr = "";
        string linkde = "";
        string linkes = "";

        //CANONICAL DEFAULT /////////////////////////////////////////////
        string linkcanonico = "~";
        linkcanonico = "~/" + SitemapManager.getCulturenamefromlingua(Lingua) + "/home";
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == Lingua)
            linkcanonico = "~";
        //FORZATURA CANONICAL
        if (!string.IsNullOrEmpty(content.CanonicalbyLingua(Lingua).Trim()))
            linkcanonico = (content.CanonicalbyLingua(Lingua).Trim());
        litgeneric.Text = "<link rel=\"canonical\" href=\"" + ReplaceAbsoluteLinks(linkcanonico) + "\"/>";
        ///////////////////////////////////////////////////////////////////////

        string linkcanonicoalt = "";
        Literal litgenericalt = null;
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatei").ToLower() == "true")
        {
            //METTIAMO GLI ALTERNATE
            //Italiano
            hreflang = " hreflang=\"it\" ";
            linkcanonicoalt = "~/" + SitemapManager.getCulturenamefromlingua("I") + "/home";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "I")
                linkcanonicoalt = "~";
            //Forzatura canonical alternate
            if (!string.IsNullOrEmpty(content.CanonicalbyLingua("I").Trim()))
                linkcanonicoalt = (content.CanonicalbyLingua("I").Trim());
            linki = ReplaceAbsoluteLinks(linkcanonicoalt);
            //alternate
            litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "I")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            }
        }
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            //inglese
            hreflang = " hreflang=\"en\" ";
            linkcanonicoalt = "~/" + SitemapManager.getCulturenamefromlingua("GB") + "/home";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "GB")
                linkcanonicoalt = "~";
            //Forzatura canonical alternate
            if (!string.IsNullOrEmpty(content.CanonicalbyLingua("GB").Trim()))
                linkcanonicoalt = (content.CanonicalbyLingua("GB").Trim());
            linken = ReplaceAbsoluteLinks(linkcanonicoalt);

            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";

            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "GB")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            }
        }

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
        {
            //russo
            hreflang = " hreflang=\"ru\" ";
            linkcanonicoalt = "~/" + SitemapManager.getCulturenamefromlingua("RU") + "/home";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "RU")
                linkcanonicoalt = "~";
            //Forzatura canonical alternate
            if (!string.IsNullOrEmpty(content.CanonicalbyLingua("RU").Trim()))
                linkcanonicoalt = (content.CanonicalbyLingua("RU").Trim());
            linkru = ReplaceAbsoluteLinks(linkcanonicoalt);

            litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "RU")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            }
        }
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() == "true")
        {
            //francese
            hreflang = " hreflang=\"fr\" ";
            linkcanonicoalt = "~/" + SitemapManager.getCulturenamefromlingua("FR") + "/home";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "FR")
                linkcanonicoalt = "~";
            //Forzatura canonical alternate
            if (!string.IsNullOrEmpty(content.CanonicalbyLingua("FR").Trim()))
                linkcanonicoalt = (content.CanonicalbyLingua("FR").Trim());
            linkfr = ReplaceAbsoluteLinks(linkcanonicoalt);

            litgenericalt = ((Literal)Master.FindControl("litgeneric4"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "FR")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            }
        }

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatede").ToLower() == "true")
        {
            //tedesco
            hreflang = " hreflang=\"de\" ";
            linkcanonicoalt = "~/" + SitemapManager.getCulturenamefromlingua("DE") + "/home";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "DE")
                linkcanonicoalt = "~";
            //Forzatura canonical alternate
            if (!string.IsNullOrEmpty(content.CanonicalbyLingua("DE").Trim()))
                linkcanonicoalt = (content.CanonicalbyLingua("DE").Trim());
            linkde = ReplaceAbsoluteLinks(linkcanonicoalt);

            litgenericalt = ((Literal)Master.FindControl("litgeneric5"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "DE")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            }
        }


        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatees").ToLower() == "true")
        {
            //spagnolo
            hreflang = " hreflang=\"es\" ";
            linkcanonicoalt = "~/" + SitemapManager.getCulturenamefromlingua("ES") + "/home";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "ES")
                linkcanonicoalt = "~";
            //Forzatura canonical alternate
            if (!string.IsNullOrEmpty(content.CanonicalbyLingua("ES").Trim()))
                linkcanonicoalt = (content.CanonicalbyLingua("ES").Trim());
            linkes = ReplaceAbsoluteLinks(linkcanonicoalt);

            litgenericalt = ((Literal)Master.FindControl("litgeneric6"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "ES")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            }
        }


        SettaLinkCambioLingua(Lingua, linki, "home", linken, "home", linkru, "home", linkfr, "home", linkde, "home", linkes, "home");


        HtmlGenericControl ulbr = (HtmlGenericControl)Master.FindControl("ulBreadcrumb");
        ulbr.Visible = false;

        ////BREADCRUMBS
        //Tabrif actualpagelink = new Tabrif();
        //actualpagelink.Campo1 = (linkcanonicoalt);
        //actualpagelink.Campo2 = ("home");
        //List<Tabrif> links = new List<Tabrif>() ;
        //links.Add(actualpagelink);
        //HtmlGenericControl ulbr = (HtmlGenericControl)Master.FindControl("ulBreadcrumb");
        //ulbr.InnerHtml = BreadcrumbConstruction(links);
    }



}