using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

public partial class AspNetPages_SchedaResource : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
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
    public string idOfferta
    {
        get { return ViewState["idOfferta"] != null ? (string)(ViewState["idOfferta"]) : ""; }
        set { ViewState["idOfferta"] = value; }
    }
    public string testoindice
    {
        get { return ViewState["testoindice"] != null ? (string)(ViewState["testoindice"]) : ""; }
        set { ViewState["testoindice"] = value; }
    }
    public string CodiceTipologia
    {
        get { return ViewState["Tipologia"] != null ? (string)(ViewState["Tipologia"]) : ""; }
        set { ViewState["Tipologia"] = value; }
    }
    public string Categoria
    {
        get { return ViewState["Categoria"] != null ? (string)(ViewState["Categoria"]) : ""; }
        set { ViewState["Categoria"] = value; }
    }
    public string Regione
    {
        get { return ViewState["Regione"] != null ? (string)(ViewState["Regione"]) : ""; }
        set { ViewState["Regione"] = value; }
    }

    Offerte item = new Offerte();
    string singleresource = "";
    //Dictionary<string, List<Allegato>> imgslistbyidallegato = new Dictionary<string, List<Allkegato>>();
    Dictionary<string, List<Dictionary<string, string>>> imgslistbyidallegato = new Dictionary<string, List<Dictionary<string, string>>>();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                CodiceTipologia = CaricaValoreMaster(Request, Session, "Tipologia");
                Categoria = CaricaValoreMaster(Request, Session, "Categoria");
                Regione = CaricaValoreMaster(Request, Session, "Regione");
                testoindice = CaricaValoreMaster(Request, Session, "testoindice");
                idOfferta = CaricaValoreMaster(Request, Session, "idOfferta");

                //if (Session["ddlProvinciaSearch"] != null) Regione = Session["ddlRegioneSearch"].ToString();//TEST
                //if (Session["ddlTipologiaSearch"] != null) Categoria = Session["ddlTipologiaSearch"].ToString();//TEST

                item = CaricaDati();
                SettaTestoIniziale();
                SettaVisualizzazione(item);
                // divContact.DataBind();
                divContactBelow.DataBind();
            }
            else
            {

            }

        }
        catch (Exception err)
        {
            //MODIFICO IL LAYOUT PER LA VISUALIZZAZIONE DELLA SCHEDA DETTAGLI

        }
    }


    private void SettaVisualizzazione(Offerte item)
    {
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        switch (CodiceTipologia)
        {
            case "rif000001":
                break;
            case "rif000666":
                divContactBelow.Visible = true;

                //Filtro con i parametri caratterizzanti l'immobile
                string serializeaddparams = "";
                Dictionary<string, string> objvaluetmp = new Dictionary<string, string>();
                objvaluetmp["ddlTipologiaSearch"] = item.noteriservate_dts;
                objvaluetmp["ddlRegioneSearch"] = item.CodiceRegione;
                serializeaddparams = Newtonsoft.Json.JsonConvert.SerializeObject(objvaluetmp).Replace("\"", "\"");
                serializeaddparams = HttpUtility.JavaScriptStringEncode(serializeaddparams);

                sb.Clear();
                sb.Append("(function wait() {");
                sb.Append("  if (typeof injectScrollerAndLoad === \"function\")");
                sb.Append("    {");
                sb.Append("injectScrollerImmobiliAndLoad(\"owlscrollerImmobili.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"\", false, true, 12,2,\"\",false,false,\"" + serializeaddparams + "\");");
                sb.Append(" }");
                sb.Append("   else  {");
                sb.Append("  setTimeout(wait, 50);");
                sb.Append("  }  })();");

                if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                {
                    cs.RegisterStartupScript(this.GetType(), "controlsuggeriti", sb.ToString(), true);
                }

#if true

                //BIND PER LA SCHEDA!!!!
                sb.Clear();
                sb.Append("(function wait() {");
                sb.Append("  if (typeof injectandloadimmobile === \"function\")");
                sb.Append("    {");
                sb.Append("injectandloadimmobile(\"schedadetailsimmobile.html\",\"divItemContainter1\", \"divitem\",false,true, \"" + idOfferta + "\");");
                sb.Append(" }");
                sb.Append("   else  {");
                sb.Append("  setTimeout(wait, 50);");
                sb.Append("  }  })();");
                if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                {
                    cs.RegisterStartupScript(this.GetType(), "controlresource1", sb.ToString(), true);
                }
#endif



                break;
            default:
                //divSearch.Visible = true;
                //divLatestPost.Visible = true;
                //AssociaRubricheConsigliati();
                //CaricaUltimiPost(CodiceTipologia, Categoria);
                //ContaArticoliPerperiodo();
                // divContact.Visible = true;
                //column1.Visible = false;
                //column2.Attributes["class"] = "col-md-9 col-sm-9";
                //column3.Attributes["class"] = "col-md-3 col-sm-3";
                column1.Visible = false;
                column3.Visible = false;
                column2.Attributes["class"] = "col-md-10 col-sm-10 col-sm-offset-1";
                break;
        }
    }



    private void SetJavascriptVariables()
    {
        //ClientScriptManager cs = Page.ClientScript;
        //string csname1 = "setresourcevalue";
        //// Check to see if the startup script is already registered.
        //if (!cs.IsStartupScriptRegistered(this.GetType(), csname1))
        //{
        //    //String cstext1 = "tmpLocalObjects['resource']=" + singleresource + ";";
        //    String cstext1 = "alert('" + singleresource + "');";
        //    cs.RegisterStartupScript(this.GetType(), csname1, cstext1, true);
        //}
        ClientScriptManager cs = Page.ClientScript;
        string jScript;
        //jScript = "alert ('Javascript block of code executed')";
        jScript = "tmpLocalObjects[\"resource\"]='Prova';";
        //cs.RegisterClientScriptBlock(typeof(Page), "keyClientBlock", jScript,true);
        cs.RegisterStartupScript(typeof(Page), "keyClientBlock", jScript, true);

    }

    private void LoadJavascriptVariables()
    {
        //ClientScriptManager cs = Page.ClientScript;

        //String scriptRegVariables = string.Format("var idresource = {0}", idOfferta);
        //scriptRegVariables = string.Format("var imgsprimarytxt = {0}", references.imgsprimary);
        //scriptRegVariables += "; " + string.Format("var singleresource = {0}", singleresource);
        //var serialized = singleresource = Newtonsoft.Json.JsonConvert.SerializeObject(imgslistbyidallegato); //Serializzo 
        //scriptRegVariables += "; " + string.Format("var imgslistbyidallegato = {0}", serialized);

        //if (!cs.IsClientScriptBlockRegistered("RegVariablesScript"))
        //{
        //    cs.RegisterClientScriptBlock(typeof(Page), "RegVariablesScript", scriptRegVariables, true);
        //}
    }
    protected string GeneraBackLink(bool usacategoria = true)
    {
        string ret = "";
        TipologiaOfferte itemtipo = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (itemtipo != null)
        {
            string testourl = itemtipo.Descrizione;

            string partipologia = "";
            string parreegione = "";
            string valtipologia = "";
            string valregione = "";
            string addtext = "";

            if (Session["objfiltro"] != null)
            {
                string sobjvalue = Session["objfiltro"].ToString();

                Dictionary<string, string> objvalue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sobjvalue);
                if (objvalue != null && objvalue.Count > 0)
                {
                    //QUESTA MODIFICA SAREBBE  MEGLIO RENDERLA COMUNE INSERENDOLA DENTRO SITEMAPMANAGER E NON TUTTE LE VOLTE CHE LA CHIAMO!!!
                    if (objvalue.ContainsKey("ddlTipologiaSearch") && objvalue["ddlTipologiaSearch"] != null)
                    {
                        partipologia = objvalue["ddlTipologiaSearch"].ToString();
                        valtipologia = references.GetreftipologieValueById(partipologia, Lingua);

                    }
                    if (objvalue.ContainsKey("ddlRegioneSearch") && objvalue["ddlRegioneSearch"] != null)
                    {
                        parreegione = objvalue["ddlRegioneSearch"].ToString();
                        valregione = NomeRegione(parreegione, Lingua);
                    }
                    if (valtipologia != "") addtext += " " + valtipologia;
                    if (valregione != "") addtext += " " + valregione;
                    if (addtext != "") testourl += addtext;

                }
            }
            ret = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, testourl, "", CodiceTipologia, partipologia, "", parreegione, "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);


            //Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            //if (catselected != null && usacategoria)
            //    testourl = catselected.Descrizione;
            //string tmpcategoria = Categoria;
            //if (!usacategoria) tmpcategoria = "";
            //ret = CommonPage.CreaLinkRoutes(Session, false, Lingua, CommonPage.CleanUrl(testourl), "", CodiceTipologia, tmpcategoria,"",Regione);
        }
        return ret;
    }


    private void SettaTestoIniziale()
    {
        string htmlPage = "";

        TipologiaOfferte itemtipo = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (itemtipo != null)
        {
            string titolopagina = itemtipo.Descrizione.ToUpper();

            if (GetGlobalResourceObject("Common", "testo" + CodiceTipologia) != null)
                htmlPage = GetGlobalResourceObject("Common", "testo" + CodiceTipologia).ToString();
            if (GetGlobalResourceObject("Common", "testo" + Categoria) != null)
                htmlPage = GetGlobalResourceObject("Common", "testo" + Categoria).ToString();
        }

        Contenuti content = null;

        string denominazione = item.DenominazionebyLingua(Lingua);
        string linkcanonico = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CleanUrl(denominazione), item.Id.ToString(), item.CodiceTipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
        linkcanonico = linkcanonico.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "");
        content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkcanonico);
        if (content == null || content.Id == 0)
        {
            linkcanonico = linkcanonico.Substring(0, linkcanonico.LastIndexOf("/") + 1);
            content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkcanonico); ;
            if (content != null && !content.TitolobyLingua(Lingua).EndsWith("/")) content = null; //evito di prendere i contenuti dedicati alle pagine lista
        }

        if (content != null && content.Id != 0)
        {
            htmlPage = content.DescrizionebyLingua(Lingua);
            // if (htmlPage.Contains("injectandloadgenericcontent")) JavaInjection = true;
        }
        litTextHeadPage.Text = ReplaceAbsoluteLinks(ReplaceLinks(htmlPage));
    }

    protected string ImpostaTestoRichiesta()
    {
        string ret = "";
        if (CodiceTipologia == "rif000008" || CodiceTipologia == "rif000010")
            ret = references.ResMan("Common", Lingua, "TestoRichiedi");
        else
            ret = references.ResMan("Common", Lingua, "TestoDisponibilita");
        return ret;
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
        }
        catch { }
        try
        {
            HtmlGenericControl limenu = ((HtmlGenericControl)Master.FindControl("link" + CodiceTipologia + "high"));
            if (limenu != null)
            {
                if (limenu != null)
                {
                    ((HtmlGenericControl)limenu).Attributes["class"] += " active";
                }
            }

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
#if true

    protected void AssociaDatiSocial(Offerte item, string serveresterno)
    {

        string host = System.Web.HttpContext.Current.Request.Url.Host.ToString();

        string descrizione = item.DescrizioneI;
        string denominazione = item.DenominazioneI;
        //  Categoria = data.CodiceCategoria;
        EvidenziaSelezione(denominazione.Replace(" ", "").Replace("-", "").Replace("&", "e").Replace("'", "").Replace("?", ""));
        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();   //;

        //Titolo e descrizione pagina
        string posizione = ControlloVuotoPosizione(item.CodiceComune, item.CodiceProvincia, item.CodiceRegione, "", Lingua);

        ((HtmlTitle)Master.FindControl("metaTitle")).Text = html.Convert((denominazione).Replace("<br/>", " ").Trim() + " " + posizione);
        string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 150, true)).Replace("<br/>", " ").Trim());
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;
        //Opengraph per facebook
        ((HtmlMeta)Master.FindControl("metafbTitle")).Content = html.Convert((denominazione + " " + Nome).Replace("<br/>", " ").Trim());
        simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 300, true))).Replace("<br/>", " ").Trim();
        ((HtmlMeta)Master.FindControl("metafbdescription")).Content = simpletext;

        if (item.FotoCollection_M != null && !string.IsNullOrEmpty(item.FotoCollection_M.FotoAnteprima))
            ((HtmlMeta)Master.FindControl("metafbimage")).Content = serveresterno + WelcomeLibrary.STATIC.Global.percorsoimg + "/" + item.Textfield1_dts + "/" + item.FotoCollection_M.FotoAnteprima;
        else if (item.FotoCollection_M != null && !string.IsNullOrEmpty(item.linkVideo))
            ((HtmlMeta)Master.FindControl("metafbvideourl")).Content = item.linkVideo;

        /////////////////////////////////////////////////////////////
        //MODIFICA PER TITLE E DESCRIPTION CUSTOM
        ////////////////////////////////////////////////////////////
        string customdesc = "";
        string customtitle = "";
        switch (Lingua)
        {
            case "RU":
                customdesc = item.Campo2RU;
                customtitle = item.Campo1RU;
                break;
            case "GB":
                customdesc = item.Campo2GB;
                customtitle = item.Campo1GB;
                break;
            default:
                customdesc = item.Campo2I;
                customtitle = item.Campo1I;
                break;
        }
        if (!string.IsNullOrEmpty(customtitle))
            ((HtmlTitle)Master.FindControl("metaTitle")).Text = (customtitle).Replace("<br/>", "\r\n");
        if (!string.IsNullOrEmpty(customdesc))
            ((HtmlMeta)Master.FindControl("metaDesc")).Content = customdesc.Replace("<br/>", "\r\n");
        ////////////////////////////////////////////////////////////

        //string linkcanonico = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(  Lingua, CleanUrl(denominazione), item.Id.ToString(), item.CodiceTipologia,"","","","","",true,WelcomeLibrary.STATIC.Global.UpdateUrl);
        //Literal litgeneric = ((Literal)Master.FindControl("litgeneric"));
        //litgeneric.Text = "<link rel=\"canonical\" href=\"" + ReplaceAbsoluteLinks(linkcanonico) + "\"/>";
        Tabrif actualpagelink = new Tabrif();

        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));

        string hreflang = "";
        //METTIAMO GLI ALTERNATE
        hreflang = " hreflang=\"it\" ";
        string linkcanonicoalt = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("I", CleanUrl(item.DenominazioneI), item.Id.ToString(), item.CodiceTipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
        linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
            linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainit"));

        Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
        litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
        Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
        litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
        if (Lingua == "I")
        {
            litcanonic.Text = "<link rel=\"canonical\"  href=\"" + linkcanonicoalt + "\"/>";
            actualpagelink.Campo1 = ReplaceAbsoluteLinks(linkcanonicoalt);
            actualpagelink.Campo2 = (item.DenominazioneI);
        }
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            linkcanonicoalt = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("GB", CleanUrl(item.DenominazioneGB), item.Id.ToString(), item.CodiceTipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainen"));

            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (Lingua == "GB")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = ReplaceAbsoluteLinks(linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(item.DenominazioneGB);
            }
        }
        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() == "true")
        {
            hreflang = " hreflang=\"ru\" ";
            linkcanonicoalt = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("RU", CleanUrl(item.DenominazioneRU), item.Id.ToString(), item.CodiceTipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
            linkcanonicoalt = ReplaceAbsoluteLinks(linkcanonicoalt);
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("debug") != "true")
                linkcanonicoalt = linkcanonicoalt.Replace(host, WelcomeLibrary.UF.ConfigManagement.ReadKey("domainru"));


            litgenericalt = ((Literal)Master.FindControl("litgeneric3"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (Lingua == "RU")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = ReplaceAbsoluteLinks(linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(item.DenominazioneRU);
            }
        }
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



        TipologiaOfferte sezione = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        string sezionedescrizione = "";
        if (sezione != null)
        {
            sezionedescrizione = sezione.Descrizione;
            string partipologia = "";
            string parreegione = "";
            string valtipologia = "";
            string valregione = "";
            string addtext = "";
            if (Session["objfiltro"] != null)
            {
                string sobjvalue = Session["objfiltro"].ToString();
                Dictionary<string, string> objvalue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sobjvalue);
                if (objvalue != null && objvalue.Count > 0)
                {
                    if (objvalue.ContainsKey("ddlTipologiaSearch") && objvalue["ddlTipologiaSearch"] != null)
                    {
                        partipologia = objvalue["ddlTipologiaSearch"].ToString();
                        valtipologia = references.GetreftipologieValueById(partipologia, Lingua);
                    }
                    if (objvalue.ContainsKey("ddlRegioneSearch") && objvalue["ddlRegioneSearch"] != null)
                    {
                        parreegione = objvalue["ddlRegioneSearch"].ToString();
                        valregione = NomeRegione(parreegione, Lingua);
                    }

                    if (valtipologia != "") addtext += " " + valtipologia;
                    if (valregione != "") addtext += " " + valregione;
                    if (addtext != "") sezionedescrizione += addtext;
                }
            }
            //METTIAMO GLI ALTERNATE
            string linkcanonico = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CleanUrl(sezionedescrizione), "", CodiceTipologia, partipologia, "", parreegione, "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
            linkcanonico = ReplaceAbsoluteLinks(linkcanonico);
            link = new Tabrif();
            link.Campo1 = linkcanonico;
            link.Campo2 = CleanUrl(sezionedescrizione);
            links.Add(link);
        }
        return links;
    }


#endif
    protected Offerte CaricaDati()
    {
        references references = new references(Server);
        references.CaricaDatiRisorseDaJson(Lingua, "", WelcomeLibrary.STATIC.Global.usecdn);
        singleresource = "";
        //imgslistbyidallegato = new Dictionary<string, List<Allegato>>();
        imgslistbyidallegato = new Dictionary<string, List<Dictionary<string, string>>>();
        Offerte item = new Offerte();
        //Selezioniamo i dati per l'immobile 
        string idallegati = "";
        dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(references.resourcesloaded);
        foreach (dynamic r in jsonResponse)
        {
            string idact = r.id;

            //Seleziono per id
            if (idact == idOfferta)
            {

                singleresource = Newtonsoft.Json.JsonConvert.SerializeObject(r); //Serializzo 

                long tmpint = 0;
                long.TryParse(idOfferta, out tmpint);
                item.Id = tmpint;
                item.CodiceProdotto = r.codice;
                item.CodiceRegione = r.codiceREGIONE;
                item.CodiceProvincia = r.codicePROVINCIA;
                item.CodiceComune = r.codiceCOMUNE;
                item.CodiceTipologia = CodiceTipologia;
                item.Textfield1_dts = r.id_allegati;
                idallegati = r.id_allegati;

                item.noteriservate_dts = r.idtipologia;
                foreach (dynamic c in r.dettagliorisorse_1.Children())
                {
                    if (c.lingua == "I")
                    {
                        item.DenominazioneI = c.titolo;
                        item.DescrizioneI = c.descrizione;
                    }
                    if (c.lingua == "GB")
                    {
                        item.DenominazioneGB = c.titolo;
                        item.DescrizioneGB = c.descrizione;
                    }
                    if (c.lingua == "RU")
                    {
                        item.DenominazioneRU = c.titolo;
                        item.DescrizioneRU = c.descrizione;
                    }
                }
                foreach (dynamic c in r.dettagliorisorse_2.Children())
                {
                    if (c.lingua == "I")
                    {
                        item.linkVideo = c.titolo;
                        item.DatitecniciI = c.descrizione;
                    }
                    if (c.lingua == "GB")
                    {
                        item.linkVideo = c.titolo;
                        item.DatitecniciGB = c.descrizione;
                    }
                    if (c.lingua == "RU")
                    {
                        item.linkVideo = c.titolo;
                        item.DatitecniciRU = c.descrizione;
                    }
                }
                break;
            }
        }
        //Selezioniamo l'immagine primaria
        //........... imgsprimary
        item.FotoCollection_M = new AllegatiCollection();
        dynamic json1 = Newtonsoft.Json.JsonConvert.DeserializeObject(references.imgsprimary);
        foreach (dynamic r in json1)
        {
            //estraiamo per l'allegato con guid_link corrispondente a    item.Textfield1_dts il NomeFile e la Descrizione
            if (r.Name == item.Textfield1_dts)
            {
                foreach (dynamic c in r.Children())
                {
                    item.FotoCollection_M.FotoAnteprima = c.NomeFile;
                    item.FotoCollection_M.NomeImmobile = c.Descrizione;
                    break;
                }
                break;
            }
        }
        AssociaDatiSocial(item, references.ExternalServer);



        return item;
    }



    protected void btnContatti1_Click(object sender, EventArgs e)
    {
        try
        {

            //Prepariamo e inviamo il mail
            string nomemittente = txtContactName.Value;
            string cognomemittente = txtContactCognome.Value;
            string mittenteMail = txtContactEmail.Value;
            string telefono = txtContactPhone.Value;
            string nomedestinatario = Nome;
            string maildestinatario = Email;
            long idperstatistiche = 0;
            string tipo = "informazioni";

            string SoggettoMail = "Richiesta " + tipo + " da " + cognomemittente + "  " + nomemittente + " tramite il sito " + Nome;
            string Descrizione = txtContactMessage.Value.Replace("\r", "<br/>") + " <br/> ";


            //QUI DEVI CARICARE I DATI DAL JSON PER L?'iIMMOBILE IN BASE ALL'ID 
            Offerte item = CaricaDati();
            if (item != null && item.Id != 0)
            {
                idperstatistiche = item.Id;
                Descrizione = Descrizione.Insert(0, "Codice:" + item.CodiceProdotto + ". " + item.DenominazioneI.Replace("\r", "<br/>") + " <br/> ");
            }

            Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome o rag soc. Cliente: " + cognomemittente;
            Descrizione += " <br/> Telefono Cliente:  " + telefono + "   Email Cliente: " + mittenteMail + " Lingua Cliente: " + Lingua;
            Descrizione += " <br/> Confermo l'autorizzazione al trattamento dei miei dati personali (D.Lgs 196/2003)";

            if (chkContactPrivacy.Checked)
            {
                Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
                //Utility.invioMailGenerico(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

                //Registro la statistica di contatto
                Statistiche stat = new Statistiche();
                stat.Data = DateTime.Now;
                stat.EmailDestinatario = maildestinatario;
                stat.EmailMittente = mittenteMail;
                stat.Idattivita = idperstatistiche;
                stat.Testomail = cognomemittente + " " + nomemittente + "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                stat.Url = "";
                statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);


                Response.Redirect(CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkContatti")) + "&Tipologia=" + CodiceTipologia + "&idOfferta=" + idOfferta.ToString() + "&conversione=true");

            }
            else
            {
                outputContact.Text = references.ResMan("Common", Lingua, "txtPrivacyError");
                //Mittente.Descrizione += " <br/> Non vi Autorizzo al trattamento dei miei dati personali (D.Lgs 196/2003)";
            }

        }
        catch (Exception err)
        {
            outputContact.Text = err.Message + " <br/> ";
            outputContact.Text += references.ResMan("Common", Lingua, "txtMailError");
        }
    }


}