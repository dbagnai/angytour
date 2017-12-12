using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

public partial class AspNetPages_RisultatiResource : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string Regione
    {
        get { return ViewState["Regione"] != null ? (string)(ViewState["Regione"]) : ""; }
        set { ViewState["Regione"] = value; }
    }
    public string Categoria
    {
        get { return ViewState["Categoria"] != null ? (string)(ViewState["Categoria"]) : ""; }
        set { ViewState["Categoria"] = value; }
    }
    public string Tipologia
    {
        get { return ViewState["Tipologia"] != null ? (string)(ViewState["Tipologia"]) : ""; }
        set { ViewState["Tipologia"] = value; }
    }
    public string Vetrina
    {
        get { return ViewState["vetrina"] != null ? (string)(ViewState["vetrina"]) : ""; }
        set { ViewState["vetrina"] = value; }
    }
    public string testoricerca
    {
        get { return ViewState["testoricerca"] != null ? (string)(ViewState["testoricerca"]) : ""; }
        set { ViewState["testoricerca"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //Prendiamo i dati dalla querystring (Lingua) o dal Context ( caso di url rewriting )
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "vuoto", false, Lingua);

                Regione = CaricaValoreMaster(Request, Session, "Regione", true, "");
                Categoria = CaricaValoreMaster(Request, Session, "Categoria", true, "");
                Tipologia = CaricaValoreMaster(Request, Session, "Tipologia", true, "");
                Vetrina = CaricaValoreMaster(Request, Session, "vetrina", true, "");
                string tmp = CaricaValoreMaster(Request, Session, "testoricerca", false);
                if (!string.IsNullOrEmpty(tmp)) testoricerca = tmp;

                //Se cerchi nelle regioni e la trovi prendi quella come filtro
                //Se cerchi nella collezione delle regioni e non la trovi -> provi a cercarla nelle province e prendi quella comefiltro
                //Session.Add("ddlProvinciaSearch", Regione);

                SettaVisualizzazione();

            }
        }
        catch
        { }

    }

    private void SettaVisualizzazione()
    {
        string cattipo = Tipologia;
        ClientScriptManager cs = Page.ClientScript;
        Literal lit = null;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        ModificaFiltroJS();
        AssociaDatiSocial();


        if (string.IsNullOrEmpty(Tipologia)) cattipo = "%";

        sb.Clear();
        sb.Append("(function wait() {");
        sb.Append("  if (typeof injectPortfolioImmobiliAndLoad === \"function\")");
        sb.Append("    {");
        sb.Append("injectPortfolioImmobiliAndLoad(\"isotopeImmobili.html\",\"divPortfolioList1\", \"portlist1\", 1, 21, true, \"\", \"" + cattipo + "\", \"" + Categoria + "\", true, false, \"\",\"" + testoricerca + "\");");
        sb.Append(" }");
        sb.Append("   else  {");
        sb.Append("  setTimeout(wait, 50);");
        sb.Append("  }  })();");


        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {
            cs.RegisterStartupScript(this.GetType(), "clist1", sb.ToString(), true);
            //cs.RegisterStartupScript(this.GetType(), "cbandestra1", cbandestra1, true);
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
        if (Regione != "")
            objvalue["ddlRegioneSearch"] = Regione;
        //Session["ddlProvinciaSearch"] = Regione;
        //if (Session["ddlProvinciaSearch"] != null) Regione = Session["ddlProvinciaSearch"].ToString();//TEST
        if (Categoria != "")
            objvalue["ddlTipologiaSearch"] = Categoria;

        if (Vetrina != "")
        {
            bool bvetrina = false;
            bool.TryParse(Vetrina, out bvetrina);
            objvalue["vetrina"] = bvetrina.ToString();
        }
        sobjvalue = Newtonsoft.Json.JsonConvert.SerializeObject(objvalue);
        Session.Add("objfiltro", sobjvalue);
    }
    protected void AssociaDatiSocial()
    {
        Tabrif actualpagelink = new Tabrif();
        string partipologia = "";
        string parreegione = "";
        string valtipologia = "";
        string valregione = "";
        string addtext = "";
        string sobjvalue = Session["objfiltro"].ToString();
        Dictionary<string, string> objvalue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sobjvalue);
        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));
        string urlcanonico = "";
        string hreflang = "";
        string sezionedescrizione = "";
        string host = System.Web.HttpContext.Current.Request.Url.Host.ToString();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////  PER CREAZIONE LINK CANONICI E ALTERNATE ///////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///ITA//////
        TipologiaOfferte sezioneI = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == "I" && tmp.Codice == Tipologia); });
        string sezionedescrizioneI = "";
        if (sezioneI != null)
        {
            sezionedescrizioneI = sezioneI.Descrizione;
            partipologia = "";
            parreegione = "";
            valtipologia = "";
            valregione = "";
            addtext = "";
            if (Session["objfiltro"] != null)
            {
                if (objvalue != null && objvalue.Count > 0)
                {
                    if (objvalue.ContainsKey("ddlTipologiaSearch") && objvalue["ddlTipologiaSearch"] != null)
                    {
                        partipologia = objvalue["ddlTipologiaSearch"].ToString();
                        valtipologia = references.GetreftipologieValueById(partipologia, "I");
                    }
                    if (objvalue.ContainsKey("ddlRegioneSearch") && objvalue["ddlRegioneSearch"] != null)
                    {
                        parreegione = objvalue["ddlRegioneSearch"].ToString();
                        valregione = NomeRegione(parreegione, "I");
                    }

                    if (valtipologia != "") addtext += " " + valtipologia;
                    if (valregione != "") addtext += " " + valregione;
                    if (addtext != "") sezionedescrizioneI += addtext;
                }
            }
            //METTIAMO GLI ALTERNATE
            hreflang = " hreflang=\"it\" ";
            string linkcanonicoalt = CreaLinkRoutes(null, false, "I", CleanUrl(sezionedescrizioneI), "", Tipologia, partipologia, "", parreegione);
            linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit"));

            Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
            litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + linkcanonicoalt + "\"/>";
            Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
            litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + linkcanonicoalt + "\"/>";
            if (Lingua == "I")
            {
                sezionedescrizione = sezionedescrizioneI;
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + linkcanonicoalt + "\"/>";
                actualpagelink.Campo1 = linkcanonicoalt;
                actualpagelink.Campo2 = CleanUrl(sezionedescrizioneI);
            }
        }
        ///GB//////
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            TipologiaOfferte sezioneGB = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == "GB" && tmp.Codice == Tipologia); });
            string sezionedescrizioneGB = "";
            if (sezioneGB != null)
            {
                sezionedescrizioneGB = sezioneGB.Descrizione;
                partipologia = "";
                parreegione = "";
                valtipologia = "";
                valregione = "";
                addtext = "";
                if (Session["objfiltro"] != null)
                {
                    if (objvalue != null && objvalue.Count > 0)
                    {
                        if (objvalue.ContainsKey("ddlTipologiaSearch") && objvalue["ddlTipologiaSearch"] != null)
                        {
                            partipologia = objvalue["ddlTipologiaSearch"].ToString();
                            valtipologia = references.GetreftipologieValueById(partipologia, "GB");
                        }
                        if (objvalue.ContainsKey("ddlRegioneSearch") && objvalue["ddlRegioneSearch"] != null)
                        {
                            parreegione = objvalue["ddlRegioneSearch"].ToString();
                            valregione = NomeRegione(parreegione, "GB");
                        }

                        if (valtipologia != "") addtext += " " + valtipologia;
                        if (valregione != "") addtext += " " + valregione;
                        if (addtext != "") sezionedescrizioneGB += addtext;
                    }
                }
                //METTIAMO GLI ALTERNATE
                hreflang = " hreflang=\"en\" ";
                string linkcanonicoalt = CreaLinkRoutes(null, false, "GB", CleanUrl(sezionedescrizioneGB), "", Tipologia, partipologia, "", parreegione);
                linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                    linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen"));

                Literal litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
                litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" + (linkcanonicoalt) + "\"/>";
                if (Lingua == "GB")
                {
                    sezionedescrizione = sezionedescrizioneGB;
                    urlcanonico = (linkcanonicoalt);
                    litcanonic.Text = "<link rel=\"canonical\"  href=\"" + (linkcanonicoalt) + "\"/>";
                    actualpagelink.Campo1 = (linkcanonicoalt);
                    actualpagelink.Campo2 = CleanUrl(sezionedescrizioneGB);
                }
            }
        }
        ///RU//////
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
        {
            TipologiaOfferte sezioneRU = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == "RU" && tmp.Codice == Tipologia); });
            string sezionedescrizioneRU = "";
            if (sezioneRU != null)
            {
                sezionedescrizioneRU = sezioneRU.Descrizione;
                partipologia = "";
                parreegione = "";
                valtipologia = "";
                valregione = "";
                addtext = "";
                if (Session["objfiltro"] != null)
                {
                    if (objvalue != null && objvalue.Count > 0)
                    {
                        if (objvalue.ContainsKey("ddlTipologiaSearch") && objvalue["ddlTipologiaSearch"] != null)
                        {
                            partipologia = objvalue["ddlTipologiaSearch"].ToString();
                            valtipologia = references.GetreftipologieValueById(partipologia, "RU");
                        }
                        if (objvalue.ContainsKey("ddlRegioneSearch") && objvalue["ddlRegioneSearch"] != null)
                        {
                            parreegione = objvalue["ddlRegioneSearch"].ToString();
                            valregione = NomeRegione(parreegione, "RU");
                        }

                        if (valtipologia != "") addtext += " " + valtipologia;
                        if (valregione != "") addtext += " " + valregione;
                        if (addtext != "") sezionedescrizioneRU += addtext;
                    }
                }
                //METTIAMO GLI ALTERNATE
                hreflang = " hreflang=\"ru\" ";
                string linkcanonicoalt = CreaLinkRoutes(null, false, "RU", CleanUrl(sezionedescrizioneRU), "", Tipologia, partipologia, "", parreegione);
                linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                    linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru"));

                Literal litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
                litgenericalt.Text = "<link rel=\"alternate\" " + hreflang + " href=\"" +  (linkcanonicoalt) + "\"/>";
                if (Lingua == "RU")
                {
                    sezionedescrizione = sezionedescrizioneRU;
                    urlcanonico =  (linkcanonicoalt);
                    litcanonic.Text = "<link rel=\"canonical\"  href=\"" +  (linkcanonicoalt) + "\"/>";
                    actualpagelink.Campo1 =  (linkcanonicoalt);
                    actualpagelink.Campo2 = CleanUrl(sezionedescrizioneRU);
                }
            }
        }
        //////////////////////////FINE SEZIONE HREFLANG E CANONICAL /////////////////////////////////////////

        ///////////////////////////SEZIONE META TITLE E DESC E CONTENUTO HEADER PAGINA ///////////////////////
        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();
        if (Vetrina != "")
            EvidenziaSelezione(sezionedescrizione);
        litNomePagina.Text = sezionedescrizione;

        string htmlPage = "";
        urlcanonico = urlcanonico.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "");
        Contenuti content = null;
        //Prendo title e desc da contenuti statici se presenti
        content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, urlcanonico);
        string customtitle = "";
        string customdesc = "";
        if (content != null && content.Id != 0)
        {
            htmlPage = ReplaceLinks(content.DescrizionebyLingua(Lingua));
            // if (htmlPage.Contains("injectPortfolioAndLoad")) JavaInjection = true;
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
        litTextHeadPage.Text = ReplaceAbsoluteLinks(ReplaceLinks(htmlPage)); //Testo header della pagina

        /////////////////////DEFINIZIONE DEI META DI PAGINA BASE////////////////////////////////////////////////////////////
        string metametatitle = html.Convert(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione));
        string description = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(htmlPage, 150, true)).Replace("<br/>", "\r\n")).Trim();
        if (string.IsNullOrEmpty(description))
            description = references.ResMan("Common", Lingua, "descMain").Replace("<br/>", " ").Trim();
        /////////////////////////////////////////////////////////////
        //////////OPTIONAL DA RISORSE ( per mappare nelle risorse le description e title per le pagine immobiliari di ricerca
        /////////////////////////////////////////////////////////////
        var uri = new Uri(Request.Url.ToString());
        string pathAndQuery = uri.PathAndQuery;
        pathAndQuery = CleanUrl(pathAndQuery).Replace("-", "").Replace("?", "");
        string metatitlefromresourcepathAndQuery = pathAndQuery + "title"; //E' il path senza dominio e senza / e spazi con title in fondo
        string metadescfromresourcepathAndQuery = pathAndQuery + "description";//E' il path senza dominio e senza / e spazi con desc in fondo
        string testitle = "";
        string testdesc = "";
        testitle = references.ResMan("Seo", Lingua, metatitlefromresourcepathAndQuery);
        testdesc = references.ResMan("Seo", Lingua, metadescfromresourcepathAndQuery);
        if (!string.IsNullOrEmpty(testitle))
            metametatitle = testitle;
        if (!string.IsNullOrEmpty(testdesc))
            description = testitle;
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
        //TITOLO E DESCRIZIONE PAGINA
        ((HtmlTitle)Master.FindControl("metaTitle")).Text = metametatitle;
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = description;

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


}