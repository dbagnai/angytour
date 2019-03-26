using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Data.OleDb;
using System.Collections.Generic;
using WelcomeLibrary.UF;

public partial class AspNetPages_Content_Tipo1 : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string ContenutoPagina
    {
        get { return ViewState["ContenutoPagina"] != null ? (string)(ViewState["ContenutoPagina"]) : ""; }
        set { ViewState["ContenutoPagina"] = value; }
    }
    public string idContenuto
    {
        get { return ViewState["idContenuto"] != null ? (string)(ViewState["idContenuto"]) : ""; }
        set { ViewState["idContenuto"] = value; }
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
    
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;

                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                idContenuto = CaricaValoreMaster(Request, Session, "idContenuto", true, "");
                string testoindice = CaricaValoreMaster(Request, Session, "testoindice", true, "");

                Contenuti content = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idContenuto);
                if (content == null || content.Id == 0)
                    content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Request.Url.AbsoluteUri);

                //////////////////////////////////////////////
                //Redirect pagine archiviate o non trovate
                //////////////////////////////////////////////
                if (content == null)
                    Response.RedirectPermanent("~/" + SitemapManager.getCulturenamefromlingua(Lingua) + "/home");

                InizializzaSeoandPage(content);
                RenderGallery(content);

                DataBind(); // renderizza le sezioni <%#
            }
            else
            {

            }

        }
        catch (Exception err)
        {
            //   output.Text = err.Message;
        }

    }


    private void InizializzaSeoandPage(Contenuti item)
    {
        if (item == null) return;

        string host = System.Web.HttpContext.Current.Request.Url.Host.ToString();

        //testo pagina!! con prerender lato server
        string TestoContenuto = item.DescrizionebyLingua(Lingua);
        try
        {
            //CommonPage.CustomContentInject(((HtmlGenericControl)Master.FindControl("divfooter1")), "customcontent1-" + Lingua + ".html", Lingua, Page.User.Identity.Name, Session);

            litMainContent.Text = custombind.bind(ReplaceAbsoluteLinks(ReplaceLinks(TestoContenuto).ToString()), Lingua, Page.User.Identity.Name, Session, null, null, Request);// ReplaceAbsoluteLinks(ReplaceLinks(TestoContenuto).ToString());
        }
        catch { }

        ///////////////////////////////////////////////
        //Titolo e descrizione pagina DEFAULT da contenuti
        ///////////////////////////////////////////////
        string TitoloContenuto = item.TitolobyLingua(Lingua);
        litNomeContenuti.Text = TitoloContenuto.ToString(); //Impostazione h1 di default in base al titolo
        ((HtmlTitle)Master.FindControl("metaTitle")).Text = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(TitoloContenuto.Replace("<br/>", " ").Trim() + " " + Nome + " " + references.ResMan("Common", Lingua, "testoPosizionebase"));
        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();   //;
        string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(litMainContent.Text, 300, true)).Replace("<br/>", " ").Trim());
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;
        ///////////////////////////////////////////////
        //Opengraph per facebook //////////////////////
        ///////////////////////////////////////////////
        ((HtmlMeta)Master.FindControl("metafbTitle")).Content = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(TitoloContenuto);
        ((HtmlMeta)Master.FindControl("metafbdescription")).Content = simpletext;
        /////////////////////////////////////////////////////////////
        //MODIFICA PER TITLE E DESCRIPTION CUSTOM
        ////////////////////////////////////////////////////////////
        string customtitle = "";
        string customdesc = "";
        switch (Lingua)
        {
            case "GB":
                customdesc = item.CustomdescGB;
                customtitle = item.CustomtitleGB;
                break;
            case "RU":
                customdesc = item.CustomdescRU;
                customtitle = item.CustomtitleRU;
                break;
            default:
                customdesc = item.CustomdescI;
                customtitle = item.CustomtitleI;
                break;
        }
        if (!string.IsNullOrEmpty(customtitle))
            ((HtmlTitle)Master.FindControl("metaTitle")).Text = (customtitle).Replace("<br/>", "\r\n");
        if (!string.IsNullOrEmpty(customdesc))
            ((HtmlMeta)Master.FindControl("metaDesc")).Content = customdesc.Replace("<br/>", "\r\n");

        ////////////////////////////////////////////////////////////
        //Accensione spengimento preconfigurazione titolo h1
        ////////////////////////////////////////////////////////////
        divTitle.Visible = false; //Imposto sempre spento ( lo gestisce l'utente )
#if false
        if (litNomeContenuti.Text.StartsWith(" ") && !string.IsNullOrEmpty(litNomeContenuti.Text)) divTitle.Visible = false;
        else divTitle.Visible = true; 
#endif

        /////////////////////////////////////////////
        ///META ROBOTS custom
        /////////////////////////////////////////////
        HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
        if (!string.IsNullOrEmpty(item.Robots.Trim()))
            metarobots.Attributes["Content"] = item.Robots.Trim();

        /////////////////////////////////////////////
        //METTIAMO CANONICAL E ALTERNATE default o forzati
        /////////////////////////////////////////////
        Tabrif actualpagelink = new Tabrif();
        string linki = "";
        string linken = "";
        string linkru = "";
        string hreflang = "";
        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));

        //CULTURA it ( set canonical eactualpage )
        hreflang = " hreflang=\"it\" ";
        //System.Globalization.CultureInfo ci = setCulture("I");
        //string testourlpagina = references.ResMan("Common","I","testoidUrl" + idContenuto).ToString();
        string testourlpaginaI = item.TitolobyLingua("I");
        linki = ReplaceAbsoluteLinks(CommonPage.CreaLinkRoutes(Session, true, "I", CommonPage.CleanUrl(testourlpaginaI), item.Id.ToString(), "con001000"));
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
            linki = linki.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit"));

        //FORZATURA CANONICAL utente
        string modcanonical = linki;
        if (!string.IsNullOrEmpty(item.CanonicalbyLingua("I").Trim()))
            modcanonical = (item.CanonicalbyLingua("I").Trim());
        //alternate
        Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
        litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
        //x-default
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "I")
        {
            Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
            litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>"; //il defaut settto controllando quale è la lingua default nella configurazione
        }

        if (Lingua.ToLower() == "i") //se navigo in it -> setto il canonical e l'actuallink su questo link
        {
            //canonical
            litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
            actualpagelink.Campo1 = (linki);
            actualpagelink.Campo2 = (testourlpaginaI);
            //redirect al canonical se il canonical non coincide con l'url
            if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
            {
                Response.RedirectPermanent(modcanonical, true);
            }
        }

        //CULTURA en ( set canonical eactualpage )
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            //ci = setCulture("GB");
            string testourlpaginaGB = item.TitolobyLingua("GB");
            linken = ReplaceAbsoluteLinks(CommonPage.CreaLinkRoutes(Session, true, "GB", CommonPage.CleanUrl(testourlpaginaGB), item.Id.ToString(), "con001000"));
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linken = linken.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen"));

            //FORZATURA CANONICAL utente
            modcanonical = linken;
            if (!string.IsNullOrEmpty(item.CanonicalbyLingua("GB").Trim()))
                modcanonical = (item.CanonicalbyLingua("GB").Trim());
            //alternate
            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "GB")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>"; //il defaut settto controllando quale è la lingua default nella configurazione
            }
            if (Lingua.ToLower() == "gb")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
                actualpagelink.Campo1 = (linken);
                actualpagelink.Campo2 = (testourlpaginaGB);
                //redirect al canonical se il canonical non coincide con l'url
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
            string testourlpaginaRU = item.TitolobyLingua("RU");
            linkru = ReplaceAbsoluteLinks(CommonPage.CreaLinkRoutes(Session, true, "RU", CommonPage.CleanUrl(testourlpaginaRU), item.Id.ToString(), "con001000"));
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkru = linkru.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru"));

            //FORZATURA CANONICAL utente
            modcanonical = linkru;
            if (!string.IsNullOrEmpty(item.CanonicalbyLingua("RU").Trim()))
                linkru = (item.CanonicalbyLingua("RU").Trim());
            //alternate
            litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
            litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (modcanonical) + "\"/>";
            //x-default
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("deflanguage") == "RU")
            {
                Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
                litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + (modcanonical) + "\"/>"; //il defaut settto controllando quale è la lingua default nella configurazione
            }
            if (Lingua.ToLower() == "ru")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (modcanonical) + "\"/>";
                actualpagelink.Campo1 = (linkru);
                actualpagelink.Campo2 = (testourlpaginaRU);
                //redirect al canonical se il canonical non coincide con l'url
                if (!CheckCanonicalUrl(System.Web.HttpContext.Current.Request.Url.ToString(), modcanonical, false))
                {
                    Response.RedirectPermanent(modcanonical, true);
                }
            }
        }
        //SET LINK PER CAMBIO LINGUA
        switch (Lingua.ToLower())
        {
            case "i":
                if (!string.IsNullOrEmpty(linken))
                {
                    HtmlGenericControl divCambioLingua1 = (HtmlGenericControl)Master.FindControl("divCambioLingua1");
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linken;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                    HtmlGenericControl divCambioLinguadef1 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef1");
                    divCambioLinguadef1.Visible = false;
                }
                if (!string.IsNullOrEmpty(linkru))
                {
                    HtmlGenericControl divCambioLingua2 = (HtmlGenericControl)Master.FindControl("divCambioLingua2");
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linkru;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                    HtmlGenericControl divCambioLinguadef2 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef2");
                    divCambioLinguadef2.Visible = false;
                }

                break;
            case "gb":
                if (!string.IsNullOrEmpty(linki))
                {
                    HtmlGenericControl divCambioLingua1 = (HtmlGenericControl)Master.FindControl("divCambioLingua1");
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linki;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                    HtmlGenericControl divCambioLinguadef1 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef1");
                    divCambioLinguadef1.Visible = false;
                }
                if (!string.IsNullOrEmpty(linkru))
                {
                    HtmlGenericControl divCambioLingua2 = (HtmlGenericControl)Master.FindControl("divCambioLingua2");
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linkru;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                    HtmlGenericControl divCambioLinguadef2 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef2");
                    divCambioLinguadef2.Visible = false;
                }
                break;
            case "ru":
                if (!string.IsNullOrEmpty(linken))
                {
                    HtmlGenericControl divCambioLingua1 = (HtmlGenericControl)Master.FindControl("divCambioLingua1");
                    divCambioLingua1.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua1.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua1.InnerHtml += "href=\"";
                    divCambioLingua1.InnerHtml += linken;
                    divCambioLingua1.InnerHtml += "\" >";
                    divCambioLingua1.InnerHtml += references.ResMan("Common", Lingua, "testoCambio1").ToUpper();
                    divCambioLingua1.InnerHtml += "</a>";
                    divCambioLingua1.Visible = true;
                    HtmlGenericControl divCambioLinguadef1 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef1");
                    divCambioLinguadef1.Visible = false;
                }
                if (!string.IsNullOrEmpty(linki))
                {
                    HtmlGenericControl divCambioLingua2 = (HtmlGenericControl)Master.FindControl("divCambioLingua2");
                    divCambioLingua2.InnerHtml = "<a style=\"color: White; padding: 8px\" ";
                    divCambioLingua2.InnerHtml += (" onclick=\"javascript:JsSvuotaSession(this)\"  ");
                    divCambioLingua2.InnerHtml += "href=\"";
                    divCambioLingua2.InnerHtml += linki;
                    divCambioLingua2.InnerHtml += "\" >";
                    divCambioLingua2.InnerHtml += references.ResMan("Common", Lingua, "testoCambio2").ToUpper();
                    divCambioLingua2.InnerHtml += "</a>";
                    divCambioLingua2.Visible = true;
                    HtmlGenericControl divCambioLinguadef2 = (HtmlGenericControl)Master.FindControl("divCambioLinguadef2");
                    divCambioLinguadef2.Visible = false;
                }
                break;
        }

        //BREADCRUMBS
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
        return links;
    }
    protected void EvidenziaSelezione(string testolink)
    {

        HtmlAnchor linkmenu = null;
        try
        {
            linkmenu = ((HtmlAnchor)Master.FindControl("linkid" + idContenuto + "high"));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                Control lidrop = CommonPage.FindControlRecursive(linkmenu.Parent, linkmenu.Parent.ID);

                if (lidrop != null)
                {
                    ((HtmlGenericControl)lidrop).Attributes["class"] += " active";
                }
            }
            linkmenu = ((HtmlAnchor)Master.FindControl("linkid" + idContenuto));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
            }
            linkmenu = ((HtmlAnchor)Master.FindControl("linkid" + idContenuto + "lateral"));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
            }

            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#000 !important");
            }
            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink + "high"));
            if (linkmenu != null)
            {
                Control lidrop = CommonPage.FindControlRecursive(linkmenu.Parent.Parent, linkmenu.Parent.Parent.ID);
                if (lidrop != null)
                {
                    ((HtmlGenericControl)lidrop).Attributes["class"] += " active";
                }
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
    private void RenderGallery(Contenuti content)
    {
        if (content == null) return;
        ContenutiCollection list = new ContenutiCollection();
        if (content.FotoCollection_M != null && content.FotoCollection_M.Count > 0)
        {
            list.Add(content);
            divGalleryDetail.Visible = true;

            rptOfferteGalleryDetail.DataSource = list;
            rptOfferteGalleryDetail.DataBind();
        }
    }
    protected bool ControlloVisibilita(object fotos)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 0) ret = false;
        bool onlypdf = (fotos != null && ((AllegatiCollection)fotos).Count > 0 && !((AllegatiCollection)fotos).Exists(c => (c.NomeFile.ToString().ToLower().EndsWith("jpg") || c.NomeFile.ToString().ToLower().EndsWith("gif") || c.NomeFile.ToString().ToLower().EndsWith("png"))));
        if (onlypdf) ret = false;
        return ret;
    }
    protected string CreaSlideNavigation(object itemobj, int maxwidth, int maxheight)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Contenuti item = ((Contenuti)itemobj);
        if ((item != null) && (item.FotoCollection_M.Count > 0))
        {
            foreach (Allegato a in item.FotoCollection_M)
            {
                string testotitolo = item.TitolobyLingua(Lingua);

                //IMMAGINE
                string pathimmagine = filemanage.ComponiUrlAnteprima(a.NomeFile, item.CodiceContenuto, item.Id.ToString());
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //LINK
                string virtuallink = filemanage.ComponiUrlAnteprima(a.NomeFile, item.CodiceContenuto, item.Id.ToString());
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
    protected bool ControlloVisibilitaMiniature(object fotos)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 1) ret = false;
        return ret;
    }
    protected string CreaSlide(object itemobj, int maxwidth, int maxheight)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Contenuti item = ((Contenuti)itemobj);
        if ((item != null) && (item.FotoCollection_M.Count > 0))
        {
            foreach (Allegato a in item.FotoCollection_M)
            {
                string testotitolo = "";
                if (!(a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                    continue;

                testotitolo = item.TitolobyLingua(Lingua);


                //IMMAGINE
                string pathimmagine = filemanage.ComponiUrlAnteprima(a.NomeFile, item.CodiceContenuto, item.Id.ToString(), true);
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //LINK
                string target = "_blank";
                string virtuallink = filemanage.ComponiUrlAnteprima(a.NomeFile, item.CodiceContenuto, item.Id.ToString(), true);
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


                #region FOTO
                //if (!string.IsNullOrEmpty(link))
                //    sb.Append("	       <a href=\"" + link + "\" target=\"" + target + "\" title=\"" + testotitolo + "\">\r\n");
                sb.Append("	           <img style=\"");
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
                if (!string.IsNullOrEmpty(a.DescrizionebyLingua(Lingua)))
                {
                    sb.Append("<div   class=\"divbuttonstyle\"  style=\"position:absolute;left:30px;bottom:30px;padding:10px;text-align:left;color:#ffffff;\">");
                    sb.Append("	       <a style=\"color:#ffffff\" href=\"" + link + "\" target=\"" + target + "\" title=\"" + testotitolo + "\">\r\n");
                    sb.Append(" " + a.DescrizionebyLingua(Lingua));
                    sb.Append("	       </a>\r\n");
                    sb.Append("	       </div>\r\n");
                }
                #endregion

                sb.Append("    </div>\r\n");
                sb.Append("</div>\r\n");
            }
        }


        return sb.ToString();
    }
    protected void btnNewsletter_Click(object sender, EventArgs e)
    {
        //Richiesta  per inserimento in anagrafica clienti !!!!!
        //Rimando alla pagina di verifica iscrizione
        ClientiDM cliDM = new ClientiDM();
        Cliente tmp_Cliente = new Cliente();
        tmp_Cliente.Cognome = txtNome.Value;
        tmp_Cliente.Email = txtEmail.Value;

        Session.Add("iscrivicliente", tmp_Cliente);
        string linkverifica = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti.aspx?ID_cliente=&Azione=iscrivinewsletter&Lingua=" + Lingua;
        Response.Redirect(linkverifica);
    }

#if false
    public void CaricaControlliJS()
    {
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        //Carico la galleria in masterpage corretta
        string controllistBanHead = "";
        if (idContenuto == "")
        {
            //controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,450);" ;

            sb.Clear();
            sb.Append("(function wait() {");
            sb.Append("  if (typeof injectSliderAndLoadBanner === \"function\")");
            sb.Append("    {");
            sb.Append("injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,450);");
            sb.Append(" }");
            sb.Append("   else  {");
            sb.Append("  setTimeout(wait, 50);");
            sb.Append("  }  })();");
        }
        else
        {
            //controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-" + idContenuto + "',false,2000,450);";
            sb.Clear();
            sb.Append("(function wait() {");
            sb.Append("  if (typeof injectSliderAndLoadBanner === \"function\")");
            sb.Append("    {");
            sb.Append("injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-" + idContenuto + "',false,2000,450);");
            sb.Append(" }");
            sb.Append("   else  {");
            sb.Append("  setTimeout(wait, 50);");
            sb.Append("  }  })();");
        }


        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {
            cs.RegisterStartupScript(this.GetType(), "controllistBanHead", sb.ToString(), true);

        }
        //if (idContenuto == "7")
        //{
        //    string controllistBan1 = "injectFasciaAndLoadBanner('bannerFascia2.html','divContainerBanner1', 'bannerfascia1', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia1',false);";
        //    string controllistBan2 = "injectFasciaAndLoadBanner('bannerFascia3.html','divContainerBanner2', 'bannerfascia2', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia2',false);";
        //    string controllistBan3 = "injectFasciaAndLoadBanner('bannerFascia2.html','divContainerBanner3', 'bannerfascia3', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia3',false);";
        //    string controllistBan4 = "injectFasciaAndLoadBanner('bannerFascia3.html','divContainerBanner4', 'bannerfascia4', 1, 2, false, '','2','','TBL_BANNERS_GENERALE','banners-fascia4',false);";
        //    if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        //    {
        //        cs.RegisterStartupScript(this.GetType(), "cban1", controllistBan1, true);
        //        cs.RegisterStartupScript(this.GetType(), "cban2", controllistBan2, true);
        //        cs.RegisterStartupScript(this.GetType(), "cban3", controllistBan3, true);
        //        cs.RegisterStartupScript(this.GetType(), "cban4", controllistBan4, true);
        //    }
        //}

    }
    private void CaricaControlliServerside()
    {

         //Carico la galleria in masterpage corretta
        if (idContenuto == "")
            Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);
        else
            Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-" + idContenuto, false, Lingua);
        Literal lit = null;
                if (idContenuto == "4" || idContenuto == "6" || idContenuto == "7" || idContenuto == "5")
                {
                    lit = (Literal)Master.FindControl("litPortfolioBanners1");
                    Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-menu", false, lit, Lingua, true);
                }

                if (idContenuto == "9")//Blog
                {

                    lit = (Literal)Master.FindControl("litPortfolioBanners2");
                    Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-sezioni", false, lit, Lingua);
                }
                if (idContenuto == "10")//Catalogo
                {
                    lit = (Literal)Master.FindControl("litPortfolioBanners1");
                    Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-sezionicatalogo", false, lit, Lingua, true);

                    lit = (Literal)Master.FindControl("litScroller3");
                    List<OleDbParameter> parColl = new List<OleDbParameter>();
                    OleDbParameter pcod = new OleDbParameter("@CodiceTIPOLOGIA", "rif000001");
                    parColl.Add(pcod);
                    OleDbParameter pvet = new OleDbParameter("@Vetrina", true);
                    parColl.Add(pvet);
                    OfferteCollection list = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "18", Lingua, false);
                    Master.CaricaUltimiPostScrollerTipo1(lit, null, "", Lingua, false, true, list, "");
                } 
    }
    
#endif


}
