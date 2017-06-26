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
    public string TipologiaOfferte
    {
        get { return ViewState["TipologiaOfferte"] != null ? (string)(ViewState["TipologiaOfferte"]) : ""; }
        set { ViewState["TipologiaOfferte"] = value; }
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
                //TipologiaOfferte = "rif000004"; //Codifica fissa per richieste

                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                ContenutoPagina = CaricaValoreMaster(Request, Session, "ContenutoPagina");
                idContenuto = CaricaValoreMaster(Request, Session, "idContenuto", true, "");
                string testoindice = CaricaValoreMaster(Request, Session, "testoindice", true, "");

                ContenutoPagina = ""; //Azzero il contenuto pagina per il rewriting egli url classici

                CaricaControlliJS();
                CaricaControlliServerside();
                //  CaricaForms();

                Contenuti content = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idContenuto);
                if (content == null || content.Id == 0)
                    content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Request.Url.AbsoluteUri);
                RenderGallery(content);

                InzializzaTestoPagina(content);
                InizializzaSeo();
               DataBind();
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
    private void CaricaControlliServerside()
    {



#if false
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
#endif
    }
    public void CaricaControlliJS()
    {
        ClientScriptManager cs = Page.ClientScript;

        //Carico la galleria in masterpage corretta
        string controllistBanHead = "";
        if (idContenuto == "")
            controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,300);";
        else
            controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-" + idContenuto + "',false,2000,300);";

        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {
            cs.RegisterStartupScript(this.GetType(), "controllistBanHead", controllistBanHead, true);

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

    private void InizializzaSeo()
    {
        string testourlpagina = GetGlobalResourceObject("Common", "testoidUrl" + idContenuto).ToString();

        string linkcanonico = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(testourlpagina), idContenuto, "con001000");
        // string linkcanonico = "~/Informazioni/" + Lingua + "/" + ContenutoPagina + "/" + CaricaValoreMaster(Request, Session, "testoindice"); ;
        Literal litgeneric = ((Literal)Master.FindControl("litgeneric"));
        litgeneric.Text = "<link rel=\"canonical\" href=\"" + ReplaceAbsoluteLinks(linkcanonico) + "\"/>";
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
                string pathimmagine = ComponiUrlAnteprima(a.NomeFile, item.CodiceContenuto, item.Id.ToString());
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //LINK
                string virtuallink = ComponiUrlAnteprima(a.NomeFile, item.CodiceContenuto, item.Id.ToString());
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
                string pathimmagine = ComponiUrlAnteprima(a.NomeFile, item.CodiceContenuto, item.Id.ToString(),true);
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //LINK
                string target = "_blank";
                string virtuallink = ComponiUrlAnteprima(a.NomeFile, item.CodiceContenuto, item.Id.ToString(),true);
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
                if (!string.IsNullOrEmpty(a.Descrizione))
                {
                    sb.Append("<div   class=\"divbuttonstyle\"  style=\"position:absolute;left:30px;bottom:30px;padding:10px;text-align:left;color:#ffffff;\">");
                    sb.Append("	       <a style=\"color:#ffffff\" href=\"" + link + "\" target=\"" + target + "\" title=\"" + testotitolo + "\">\r\n");
                    sb.Append(" " + a.Descrizione);
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
        //DateTime _d = DateTime.MinValue;
        //if (DateTime.TryParse(txtDataNascita.Text, out _d))
        //    tmp_Cliente.DataNascita = _d;
        Session.Add("iscrivicliente", tmp_Cliente);
        string linkverifica = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti.aspx?ID_cliente=&Azione=iscrivinewsletter&Lingua=" + Lingua;
        Response.Redirect(linkverifica);
    }



    protected void InzializzaTestoPagina(Contenuti content)
    {

        /////////////////////////////////////////////////////
        if (!string.IsNullOrEmpty(ContenutoPagina))
        {
            string tmp = GetGlobalResourceObject("Common", "Testo" + ContenutoPagina).ToString();
            litNomeContenuti.Text = tmp;
            litMainContent.Text = ReplaceAbsoluteLinks(ReplaceLinks(GetLocalResourceObject("Testo" + ContenutoPagina).ToString()));
            //Titolo e descrizione pagina
            ((HtmlTitle)Master.FindControl("metaTitle")).Text = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(tmp + " " + references.ResMan("Common",Lingua, "testoPosizionebase") + " " + Nome);

            WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();   //;

            string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(litMainContent.Text, 300, true)).Replace("<br/>", "\r\n"));
            ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;
            //Opengraph per facebook
            ((HtmlMeta)Master.FindControl("metafbTitle")).Content = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(tmp + " " + Nome);
            simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(litMainContent.Text, 300, true))).Replace("<br/>", "\r\n");
            ((HtmlMeta)Master.FindControl("metafbdescription")).Content = simpletext;


        }
        if (content != null)
        {
            if (content.Id == 12) //Visualizzo il form per le newsletter
                pnlNewsletter.Visible = true;
            //if (content.Id == 9)
            //    divTitleFreccia.Visible = false;

            string DescrizioneContenuto = content.TitolobyLingua(Lingua);
            string TestoContenuto = content.DescrizionebyLingua(Lingua);

            //if (content.Id != 9 && content.Id != 10) //Non metto il titolo pagina in questo caso
            litNomeContenuti.Text = DescrizioneContenuto.ToString();

            EvidenziaSelezione(content.TitoloI.Replace(" ", "").Replace("-", "").Replace("&", "e").Replace("'", "").Replace("?", ""));

            try
            {
                litMainContent.Text =
                    ReplaceAbsoluteLinks(ReplaceLinks(TestoContenuto).ToString());
            }
            catch { }
            //Titolo e descrizione pagina
            ((HtmlTitle)Master.FindControl("metaTitle")).Text = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(DescrizioneContenuto.Replace("<br/>", " ").Trim() + " " + references.ResMan("Common",Lingua, "testoPosizionebase") + " " + Nome);

            WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();   //;
            string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(litMainContent.Text, 300, true)).Replace("<br/>", " ").Trim());
            ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;

            //Opengraph per facebook
            ((HtmlMeta)Master.FindControl("metafbTitle")).Content = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(DescrizioneContenuto + " " + Nome);
            simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(litMainContent.Text, 300, true))).Replace("<br/>", " ").Trim();
            ((HtmlMeta)Master.FindControl("metafbdescription")).Content = simpletext;

        }
        if (litNomeContenuti.Text.StartsWith(" ")) divTitle.Visible = false;
    }


    protected string ConteggioCaratteri(string testo, int caratteri = 600, bool nolink = false)
    {
        string ritorno = testo;

        if (testo.Length > caratteri)
        {
            int invio = testo.IndexOf(" ", caratteri);

            if (nolink)
            {
                if (invio != -1)
                    ritorno = testo.Substring(0, invio) + "";
                else
                    ritorno = testo.Substring(0, caratteri) + "";
            }
            else
            {
                if (invio != -1)
                    ritorno = testo.Substring(0, invio) + "..." + GetGlobalResourceObject("Common", "testoContinua").ToString();
                else
                    ritorno = testo.Substring(0, caratteri) + "..." + GetGlobalResourceObject("Common", "testoContinua").ToString();
            }
        }
        return ritorno;
    }



}
