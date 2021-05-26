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
            //danese
            hreflang = " hreflang=\"da\" ";
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

        SettaLinkCambioLingua(linki, linken, linkru, linkfr);


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

    private void SettaLinkCambioLingua(string linki, string linken, string linkru, string linkfr)
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
                if (!string.IsNullOrEmpty(linken))
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
                if (!string.IsNullOrEmpty(linkru))
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
                if (!string.IsNullOrEmpty(linkfr))
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

                break;
            case "gb":
                if (!string.IsNullOrEmpty(linki))
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
                if (!string.IsNullOrEmpty(linkru))
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
                if (!string.IsNullOrEmpty(linkfr))
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
                break;
            case "ru":
                if (!string.IsNullOrEmpty(linken))
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
                if (!string.IsNullOrEmpty(linki))
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
                if (!string.IsNullOrEmpty(linkfr))
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
                break;
            case "fr":
                if (!string.IsNullOrEmpty(linken))
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
                if (!string.IsNullOrEmpty(linki))
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
                if (!string.IsNullOrEmpty(linkru))
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
                break;
        }

    }

     
}