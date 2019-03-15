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
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;
using System.IO;
using System.Security;
using System.Drawing.Imaging;
using System.Collections.Generic;
public partial class AreaContenuti_GestioneContenutiNew : CommonPage
{
    contenutiDM conDM = new contenutiDM();
    public string PageGuid
    {
        get { return ViewState["PageGuid"] != null ? (string)(ViewState["PageGuid"]) : ""; }
        set { ViewState["PageGuid"] = value; }
    }
    public string Pagina
    {
        get { return ViewState["Pagina"] != null ? (string)(ViewState["Pagina"]) : ""; }
        set { ViewState["Pagina"] = value; }
    }
    public string ClientIDSelected
    {
        get { return ViewState["ClientIDSelected"] != null ? (string)(ViewState["ClientIDSelected"]) : ""; }
        set { ViewState["ClientIDSelected"] = value; }
    }
    public string ContentIDSelected
    {
        get { return ViewState["ContentIDSelected"] != null ? (string)(ViewState["ContentIDSelected"]) : ""; }
        set { ViewState["ContentIDSelected"] = value; }
    }
    public string NomeFotoSelezionata
    {
        get { return ViewState["NomeFotoSelezionata"] != null ? (string)(ViewState["NomeFotoSelezionata"]) : ""; }
        set { ViewState["NomeFotoSelezionata"] = value; }
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
    public string CodiceContenuto
    {
        get { return ViewState["CodiceContenuto"] != null ? (string)(ViewState["CodiceContenuto"]) : ""; }
        set { ViewState["CodiceContenuto"] = value; }
    }

    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : ""; }
        set { ViewState["Lingua"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SetCulture("it"); //forzo la cultura italia
        if (!IsPostBack)
        {
            PageGuid = System.Guid.NewGuid().ToString();
            PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
            PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;
            Lingua = "I";
            if (Request.QueryString["CodiceContenuto"] != null && Request.QueryString["CodiceContenuto"] != "")
            { CodiceContenuto = Request.QueryString["CodiceContenuto"].ToString(); }
            if (CodiceContenuto == null || CodiceContenuto == "")
                Response.Redirect("default.aspx?Errore=Selezionare Tipo Contenuto");

            if (CodiceContenuto == "con001000") //Area pagine statiche
            {
                plhHtmlEditorRU.Visible = true;
                plhHtmlEditorGB.Visible = true;
                plhHtmlEditorI.Visible = true;

                plhSimpleEditorRU.Visible = false;
                plhSimpleEditorGB.Visible = false;
                plhSimpleEditorI.Visible = false;
            }
            else
            {
                //plhHtmlEditorRU.Visible = false;
                //plhHtmlEditorGB.Visible = false;
                //plhHtmlEditorI.Visible = false;

                //plhSimpleEditorRU.Visible = true;
                //plhSimpleEditorGB.Visible = true;
                //plhSimpleEditorI.Visible = true;

                plhHtmlEditorRU.Visible = true;
                plhHtmlEditorGB.Visible = true;
                plhHtmlEditorI.Visible = true;

                plhSimpleEditorRU.Visible = false;
                plhSimpleEditorGB.Visible = false;
                plhSimpleEditorI.Visible = false;
            }

            //Carichiamo i dati relativi al contenuto specificato
            //Da fare repeater paginato con i risultati della query sul db
            litTitolo.Text = (Utility.TipologieContenuti.Find(delegate (TipologiaContenuti tmp) { return tmp.Lingua == "I" && tmp.Codice == CodiceContenuto; })).Descrizione;

            CaricaTipologieStrutture();
            CaricaListaStrutture(ddlTipologie.SelectedValue, "0");
            this.CaricaDati();

            ImpostaDettaglioSolaLettura(true);
        }
        else
        {

            output.Text = "";

        }
    }

    public string CreaLinkPaginastatica(long idps, bool noli = false, string classe = "", string stile = "font-weight:600 !important", string lng = "")
    {
        if (string.IsNullOrEmpty(lng)) lng = Lingua;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        contenutiDM conDM = new contenutiDM();
        Contenuti item = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idps.ToString());
        //Creiamo i link
        if (item != null)
        {

            string testo = item.TitolobyLingua(lng);
            string link = CommonPage.CreaLinkRoutes(Session, true, lng, CommonPage.CleanUrl(testo), idps.ToString(), CodiceContenuto);
            testo = "Vedi"; //Forzo testo fisso
            if (!noli) sb.Append("<li>");
            sb.Append("<a  href=\"");
            sb.Append(link);
            sb.Append("\"");
            if (idps.ToString() == ContentIDSelected)
                sb.Append(" class=\"" + classe + "\" style=\"" + stile + "\"  ");
            sb.Append(" target=\"_blank\" onclick=\"javascript:JsSvuotaSession(this)\"  >");
            sb.Append(testo);
            sb.Append("</a>");
            if (!noli) sb.Append("</li>");
            /*
                 <a id="linkid10High" onclick="JsSvuotaSession(this)" runat="server" href="#">
                                        <%= references.ResMan("Common", Lingua,"testoid10") %>
                                    </a>
             */
        }
        return sb.ToString();
    }


    #region PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER

    protected void PagerRisultati_PageCommand(object sender, string PageNum)
    {
        PagerRisultati.CurrentPage = Convert.ToInt32(PageNum);
        Pagina = PageNum;
        Pager<Offerte> p = new Pager<Offerte>();
        if (p.LoadFromCache(this, PageGuid + PagerRisultati.ClientID))
        {
            rptContenuti.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
            rptContenuti.DataBind();
        }
        else
        {
            CaricaDati();
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

    //void lnkGroupPrev_Command(object sender, CommandEventArgs e)
    //{

    //    int ntotalepagine = PagerRisultati.totalPages;
    //    //dimensioneGruppo
    //    //nGruppoPagine
    //    int totalegruppi = 0;
    //    if ((int)(ntotalepagine / PagerRisultati.dimensioneGruppo) > 0)
    //        totalegruppi = (int)System.Math.Ceiling(((Double)ntotalepagine / (Double)PagerRisultati.dimensioneGruppo));
    //    else
    //        totalegruppi = 1;
    //    if (PagerRisultati.nGruppoPagine > 1)
    //        PagerRisultati.nGruppoPagine -= 1;

    //    this.btnRicerca_Click(null, null);

    //}
    //void lnkGroupNext_Command(object sender, CommandEventArgs e)
    //{

    //    int ntotalepagine = PagerRisultati.totalPages;
    //    //dimensioneGruppo
    //    //nGruppoPagine
    //    int totalegruppi = 0;
    //    if ((int)(ntotalepagine / PagerRisultati.dimensioneGruppo) > 0)
    //        totalegruppi = (int)System.Math.Ceiling(((Double)ntotalepagine / (Double)PagerRisultati.dimensioneGruppo));
    //    else
    //        totalegruppi = 1;
    //    if (PagerRisultati.nGruppoPagine < totalegruppi)
    //        PagerRisultati.nGruppoPagine += 1;

    //    this.btnRicerca_Click(null, null);
    //}

    #endregion


    private void CaricaTipologieStrutture()
    {
        List<WelcomeLibrary.DOM.TipologiaOfferte> Tipologie = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I"); });

        ddlTipologie.Items.Clear();
        ddlTipologie.Items.Insert(0, "Seleziona tipologia");
        ddlTipologie.Items[0].Value = "";
        ddlTipologie.DataSource = Tipologie;
        ddlTipologie.DataTextField = "Descrizione";
        ddlTipologie.DataValueField = "Codice";
        ddlTipologie.DataBind();
    }
    protected void ddlTipologie_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaListaStrutture(ddlTipologie.SelectedValue, "0");

    }
    private void CaricaListaStrutture(string codicetipologia, string idstruttura = "0")
    {

        OfferteCollection offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetipologia);

        ddlStruttura.Items.Clear();
        ddlStruttura.Items.Insert(0, "Seleziona struttura");
        ddlStruttura.Items[0].Value = "0";
        ddlStruttura.DataSource = offerte;
        ddlStruttura.DataTextField = "DenominazioneI";
        ddlStruttura.DataValueField = "Id";
        ddlStruttura.DataBind();
        try
        {
            ddlStruttura.SelectedValue = idstruttura;
        }
        catch { }
    }

    private void CaricaDati()
    {
        ContenutiCollection contenuti = conDM.CaricaContenutiPerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceContenuto, "5000");

        Pager<Contenuti> p = new Pager<Contenuti>(contenuti, false, this.Page, PageGuid + PagerRisultati.ClientID);
        PagerRisultati.TotalRecords = p.Count;
        //PagerRisultatiLow.TotalRecords = p.Count;
        try
        {
            PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
            //PagerRisultatiLow.CurrentPage = Convert.ToInt32(Pagina);

        }
        catch { }
        rptContenuti.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize); ;
        //rptContenuti.DataSource = contenuti;
        rptContenuti.DataBind();
        this.AggiornaDettaglio(ContentIDSelected);
    }
    private void AggiornaDettaglio(string id)
    {
        Contenuti Details = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ContentIDSelected);
        if (Details != null)
        {
            //Carichiamo eventuale struttura associata nel dettaglio

            if (Details.Id_attivita != 0)
            {
                Offerte offerta = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Details.Id_attivita.ToString());
                if (offerta != null)
                {
                    ddlTipologie.SelectedValue = offerta.CodiceTipologia;
                    CaricaListaStrutture(ddlTipologie.SelectedValue, Details.Id_attivita.ToString());
                }
            }

            litlinkI.Text = CreaLinkPaginastatica(Details.Id, true, "", "", "I");
            litlinkGB.Text = CreaLinkPaginastatica(Details.Id, true, "", "", "GB");
            litlinkRU.Text = CreaLinkPaginastatica(Details.Id, true, "", "", "RU");

            //Riempiamo i dati del dettaglio
            txtTitoloI.Text = Details.TitoloI;//(((Literal)e.Item.FindControl("lit1")).Text);
            txtTitoloGB.Text = Details.TitoloGB;//(((Literal)e.Item.FindControl("lit2")).Text);
            txtTitoloRU.Text = Details.TitoloRU;//(((Literal)e.Item.FindControl("lit2")).Text);

            txtCustomtitleI.Text = Details.CustomtitleI;
            txtCustomtitleGB.Text = Details.CustomtitleGB;
            txtCustomtitleRU.Text = Details.CustomtitleRU;
            txtCustomdescI.Text = Details.CustomdescI;
            txtCustomdescGB.Text = Details.CustomdescGB;
            txtCustomdescRU.Text = Details.CustomdescRU;

            txtCanonicalI.Text = Details.CanonicalI;
            txtCanonicalGB.Text = Details.CanonicalGB;
            txtCanonicalRU.Text = Details.CanonicalRU;
            txtRobots.Text = Details.Robots;

            if (CodiceContenuto == "con001000")
            {
                tinyhtmlEditRU.InnerText = Details.DescrizioneRU;
                tinyhtmlEditGB.InnerText = Details.DescrizioneGB;
                tinyhtmlEditI.InnerText = Details.DescrizioneI;


            }
            else
            {
                //txtDescrizioneI.Text = Details.DescrizioneI;//(((Literal)e.Item.FindControl("lit3")).Text);
                //txtDescrizioneGB.Text = Details.DescrizioneGB;//(((Literal)e.Item.FindControl("lit4")).Text);
                //txtDescrizioneRU.Text = Details.DescrizioneRU;//(((Literal)e.Item.FindControl("lit4")).Text);
                tinyhtmlEditRU.InnerText = Details.DescrizioneRU;
                tinyhtmlEditGB.InnerText = Details.DescrizioneGB;
                tinyhtmlEditI.InnerText = Details.DescrizioneI;

            }

            txtData.Text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", Details.DataInserimento);

            //Impostiamo l'url per la foto (Prendo sempre la prima)
            if (Details.FotoCollection_M != null && Details.FotoCollection_M.Count > 0)
            {
                imgFoto.ImageUrl = PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected + "/" + Details.FotoCollection_M[0].NomeFile;
                NomeFotoSelezionata = Details.FotoCollection_M[0].NomeFile;
                txtDescrizione.Text = Details.FotoCollection_M[0].Descrizione;
            }
            else
            {
                imgFoto.ImageUrl = "";
                txtDescrizione.Text = "";
            }
            //Carichiamo la galleria delle foto
            rptImmagini.DataSource = Details.FotoCollection_M;
            rptImmagini.DataBind();
        }
        else
        {
            this.SvuotaDettaglio();
        }
    }

    protected void rptContenuti_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                //QUI CONTROLLO ED EVIDENZIO LA RIGA DEL REPEATER ATTUALMENTE SELEZIONATA
                LinkButton lnkImg = (LinkButton)e.Item.FindControl("imgSelect");
                if (lnkImg != null) if (lnkImg.CommandArgument == ContentIDSelected && ContentIDSelected != "")
                    {
                        //txtFotoSchema.Value = (((Literal)e.Item.FindControl("lit5")).Text);
                        //txtFotoValori.Value = (((Literal)e.Item.FindControl("lit6")).Text);
                        //AllegatiCollection FotoColl = new AllegatiCollection();
                        //FotoColl.Schema = txtFotoSchema.Value;
                        //FotoColl.Valori = txtFotoValori.Value;
                        //FotoColl = conDM.CaricaAllegatiFoto(FotoColl);

                    }
                    else
                    {
                    }
            }
        }
    }

    protected void ImageButton1_PreRender(object sender, EventArgs e)
    {
        //if (((ImageButton)sender).ClientID == ClientIDSelected)
        //if (((ImageButton)sender).CommandArgument == ContentIDSelected)
        //{
        //    ((ImageButton)sender).ImageUrl = "~/images/arrow_im_1.jpg";
        //    ((ImageButton)sender).BackColor = System.Drawing.ColorTranslator.FromHtml("#000000");

        //}
        //else
        //{
        //    ((ImageButton)sender).ImageUrl = "~/images/search_icone.jpg";
        //    ((ImageButton)sender).BackColor = System.Drawing.ColorTranslator.FromHtml("#e9e9a4"); //System.Drawing.Color.Transparent;
        //}


        if (((LinkButton)sender).CommandArgument == ContentIDSelected)
        {
            HtmlControl control = (HtmlControl)((LinkButton)sender).FindControl("imgButton");
            control.Attributes.Add("class", "fa fa-search fa-2x blue");
        }
        else
        {
            HtmlControl control = (HtmlControl)((LinkButton)sender).FindControl("imgButton");
            control.Attributes.Add("class", "fa fa-search fa-2x");
        }

    }
    protected void link_click(object sender, EventArgs e)
    {
        this.SvuotaDettaglio();
        ContentIDSelected = ((LinkButton)(sender)).CommandArgument.ToString();
        ClientIDSelected = ((LinkButton)(sender)).ClientID;
        NomeFotoSelezionata = "";

        this.AggiornaDettaglio(ContentIDSelected);


        //Devo memorizzare il clientid per la checkbox della linea cliccata scorrendo 
        //il repeater
        //SCORRO IL REPEATER PER TROVARE LA RIGA CLICCATA e i sui sotto elementi
        //if (rptContenuti.Items != null && rptContenuti.Items.Count != 0)
        //{
        //    foreach (System.Web.UI.WebControls.RepeaterItem item in rptContenuti.Items)
        //    {
        //        if (((ImageButton)(item.FindControl("imgSelect"))).ClientID == ClientIDSelected)
        //        {
        //            txtTitoloI.Text = (((Literal)item.FindControl("lit1")).Text);
        //            txtTitoloGB.Text = (((Literal)item.FindControl("lit2")).Text);
        //            txtDescrizioneI.Text = (((Literal)item.FindControl("lit3")).Text);
        //            txtDescrizioneGB.Text = (((Literal)item.FindControl("lit4")).Text);
        //            txtFotoSchema.Value = (((Literal)item.FindControl("lit5")).Text);
        //            txtFotoValori.Value = (((Literal)item.FindControl("lit6")).Text);
        //            AllegatiCollection FotoColl = new AllegatiCollection();
        //            FotoColl.Schema = txtFotoSchema.Value;
        //            FotoColl.Valori = txtFotoValori.Value;
        //            FotoColl = conDM.CaricaAllegatiFoto(FotoColl);
        //            //Impostiamo l'url per la foto (Prendo sempre la prima)
        //            if (FotoColl != null && FotoColl.Count > 0)
        //                imgFoto.ImageUrl = PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected + "/" + FotoColl[0].NomeFile;
        //            else
        //                imgFoto.ImageUrl = "";
        //        }
        //    }
        //}
    }
    protected void linkgalleria_click(object sender, EventArgs e)
    {
        NomeFotoSelezionata = ((ImageButton)(sender)).CommandArgument.ToString();

        string percorsofile = PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected + "/" + NomeFotoSelezionata;
        imgFoto.ImageUrl = percorsofile;


        //PER I FILES CHE NON SONO IMMAGINI METTO UN'IMMAGINE FISSA
        if (!(NomeFotoSelezionata.ToLower().EndsWith("jpg") || NomeFotoSelezionata.ToLower().EndsWith("gif") || NomeFotoSelezionata.ToLower().EndsWith("png")))
        {
            imgFoto.ImageUrl = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";
            /// WelcomeLibrary.UF.SharedStatic.DownloadFile(percorsofile); con questa faccio partire il download del file
            linkFoto.HRef = percorsofile;
        }
        else
            linkFoto.HRef = "";


        txtDescrizione.Text = ((ImageButton)(sender)).ToolTip;
    }
    protected void btnNuovo_Click(object sender, EventArgs e)
    {
        this.SvuotaDettaglio();
        ImpostaDettaglioSolaLettura(false);
        btnAggiorna.Text = "Inserisci";
    }
    protected string SostituisciACapo(string testo)
    {
        testo = testo.Replace("\r\n", "<br/>");
        return testo;
    }

    protected void btnAggiorna_Click(object sender, EventArgs e)
    {
        //valutazioni updrecord = new valutazioni();
        Contenuti updrecord = new Contenuti();
        try
        {
            // ArrayList tipologie = valDM.CaricaTipologie("Access.dbEdil2000");
            if (btnAggiorna.Text == "Modifica")
            {
                btnAggiorna.Text = "Aggiorna";
                ImpostaDettaglioSolaLettura(false);
                return;
            }
            if (btnAggiorna.Text == "Aggiorna")
            {
                updrecord = new Contenuti();
                long tmp = 0;
                if (long.TryParse(ContentIDSelected, out tmp))
                {
                    updrecord.Id = tmp;
                    updrecord.CodiceContenuto = CodiceContenuto;
                    updrecord.TitoloI = txtTitoloI.Text;// WelcomeLibrary.UF.SitemapManager.CleanUrl(txtTitoloI.Text);
                    updrecord.TitoloGB = txtTitoloGB.Text; // WelcomeLibrary.UF.SitemapManager.CleanUrl(txtTitoloGB.Text);
                    updrecord.TitoloRU = txtTitoloRU.Text;// WelcomeLibrary.UF.SitemapManager.CleanUrl(txtTitoloRU.Text);

                    updrecord.CustomtitleI = txtCustomtitleI.Text;
                    updrecord.CustomtitleGB = txtCustomtitleGB.Text;
                    updrecord.CustomtitleRU = txtCustomtitleRU.Text;
                    updrecord.CustomdescI = txtCustomdescI.Text;
                    updrecord.CustomdescGB = txtCustomdescGB.Text;
                    updrecord.CustomdescRU = txtCustomdescRU.Text;


                    updrecord.CanonicalI = txtCanonicalI.Text;
                    updrecord.CanonicalGB = txtCanonicalGB.Text;
                    updrecord.CanonicalRU = txtCanonicalRU.Text;
                    updrecord.Robots = txtRobots.Text;

                    if (CodiceContenuto == "con001000")
                    {
                        updrecord.DescrizioneI = tinyhtmlEditI.InnerText;
                        updrecord.DescrizioneGB = tinyhtmlEditGB.InnerText;
                        updrecord.DescrizioneRU = tinyhtmlEditRU.InnerText;


                    }
                    else
                    {
                        updrecord.DescrizioneI = tinyhtmlEditI.InnerText;
                        updrecord.DescrizioneGB = tinyhtmlEditGB.InnerText;
                        updrecord.DescrizioneRU = tinyhtmlEditRU.InnerText;

                        //updrecord.DescrizioneI = txtDescrizioneI.Text;
                        //updrecord.DescrizioneGB = txtDescrizioneGB.Text;
                        //updrecord.DescrizioneRU = txtDescrizioneRU.Text;
                    }

                    DateTime _tmpdate = System.DateTime.Now;
                    //DateTime.TryParse(txtData.Text, out _tmpdate);
                    DateTime.TryParseExact(txtData.Text, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _tmpdate);
                    updrecord.DataInserimento = _tmpdate;
                    long _i = 0;
                    long.TryParse(ddlStruttura.SelectedValue, out _i);
                    updrecord.Id_attivita = _i;
                    //Questi li devi riempire con la lista delle foto
                    //updrecord.FotoCollection_M.Schema = txtFotoSchema.Value;
                    //updrecord.FotoCollection_M.Valori = txtFotoValori.Value;
                    conDM.UpdateContenuti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    btnAggiorna.Text = "Modifica";
                    ImpostaDettaglioSolaLettura(true);
                }
            }
            else
                if (btnAggiorna.Text == "Inserisci")
            {
                updrecord = new Contenuti();
                updrecord.CodiceContenuto = CodiceContenuto;
                updrecord.TitoloI = txtTitoloI.Text;// WelcomeLibrary.UF.SitemapManager.CleanUrl(txtTitoloI.Text);
                updrecord.TitoloGB = txtTitoloGB.Text; // WelcomeLibrary.UF.SitemapManager.CleanUrl(txtTitoloGB.Text);
                updrecord.TitoloRU = txtTitoloRU.Text;// WelcomeLibrary.UF.SitemapManager.CleanUrl(txtTitoloRU.Text);

                updrecord.CustomtitleI = txtCustomtitleI.Text;
                updrecord.CustomtitleGB = txtCustomtitleGB.Text;
                updrecord.CustomtitleRU = txtCustomtitleRU.Text;
                updrecord.CustomdescI = txtCustomdescI.Text;
                updrecord.CustomdescGB = txtCustomdescGB.Text;
                updrecord.CustomdescRU = txtCustomdescRU.Text;


                updrecord.CanonicalI = txtCanonicalI.Text;
                updrecord.CanonicalGB = txtCanonicalGB.Text;
                updrecord.CanonicalRU = txtCanonicalRU.Text;
                updrecord.Robots = txtRobots.Text;

                if (CodiceContenuto == "con001000")
                {
                    //updrecord.DescrizioneI = htmlEditI.Content;
                    //updrecord.DescrizioneGB = htmlEditGB.Content;
                    //updrecord.DescrizioneRU = htmlEditRU.Content;

                    updrecord.DescrizioneI = tinyhtmlEditI.InnerText;
                    updrecord.DescrizioneGB = tinyhtmlEditGB.InnerText;
                    updrecord.DescrizioneRU = tinyhtmlEditRU.InnerText;

                }
                else
                {
                    updrecord.DescrizioneI = tinyhtmlEditI.InnerText;
                    updrecord.DescrizioneGB = tinyhtmlEditGB.InnerText;
                    updrecord.DescrizioneRU = tinyhtmlEditRU.InnerText;

                    //updrecord.DescrizioneI = txtDescrizioneI.Text;
                    //updrecord.DescrizioneGB = txtDescrizioneGB.Text;
                    //updrecord.DescrizioneRU = txtDescrizioneRU.Text;
                }

                DateTime _tmpdate = System.DateTime.Now;
                //DateTime.TryParse(txtData.Text, out _tmpdate);
                DateTime.TryParseExact(txtData.Text, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _tmpdate);
                updrecord.DataInserimento = _tmpdate;
                //Questi li devi riempire con la lista delle foto
                //updrecord.FotoCollection_M.Schema = txtFotoSchema.Value;
                //updrecord.FotoCollection_M.Valori = txtFotoValori.Value;
                long _i = 0;
                long.TryParse(ddlStruttura.SelectedValue, out _i);
                updrecord.Id_attivita = _i;

                conDM.InsertContenuto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

                //Inviamo una mailing a tutti i clienti validati ( e con card attiva ) per l'offerta in questione appena inserita
                //if (updrecord.CodiceContenuto == "con000008") //mailing solo per le offerte
                //    this.InviaMailistInserimentoContenuto(updrecord); //in updrecord ho l'id del contenuto appena inseritO!!

                this.SvuotaDettaglio();
                ImpostaDettaglioSolaLettura(true);

            }

            ////////////////////////////////////////////////////////////////////
            //Creo o aggiorno l'url per il rewriting in tutte le lingue ...
            ////////////////////////////////////////////////////////////////////
            //Elimino gli url precendenti !!!!!  ( ma questo fa andare in home i vecchi url !!! non è il massimo)!!!!
            ///WelcomeLibrary.UF.SitemapManager.EliminaUrlrewritebyIdContenuto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord.Id.ToString());

            WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("I", updrecord.TitolobyLingua("I"), updrecord.Id.ToString(), CodiceContenuto, "", "", "", "", "", true, true);
            WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("GB", updrecord.TitolobyLingua("GB"), updrecord.Id.ToString(), CodiceContenuto, "", "", "", "", "", true, true);
            WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("RU", updrecord.TitolobyLingua("RU"), updrecord.Id.ToString(), CodiceContenuto, "", "", "", "", "", true, true);


            /////////////////////////////////////

            this.CaricaDati();
            //ContenutiCollection list = conDM.CaricaContenutiPerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceContenuto);
            //rptContenuti.DataSource = list;
            //rptContenuti.DataBind();

        }
        catch (Exception error)
        {
            output.Text = error.Message;
            if (error.InnerException != null)
                output.Text += error.InnerException.Message.ToString();
        }

    }
    /// <summary>
    /// Prepara la richiesta di una mail di avviso ai clienti per avviso inserimento di una nuova offerta nell'WebMouse
    /// </summary>
    /// <param name="item"></param>
    private void InviaMailistInserimentoContenuto(Contenuti item)
    {
        try
        {
            //Carichiamo tutti i clienti destinatari ( prendo i clienti Validati, con consenso commrciale opzione 1, e con card attivata e non scaduta )
            // CardsDM cDM = new CardsDM();
            ClientiDM cliDM = new ClientiDM();
            Cliente _clifiltro = new Cliente();
            _clifiltro.Validato = true;
            _clifiltro.Consenso1 = true;
            ClienteCollection clientimail = cliDM.CaricaClientiFiltrati(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _clifiltro);

            if (clientimail != null)
            {
                //Eliminiamo i clienti con card scadute (Opzionale - commentare se non voluto )
                //clientimail.RemoveAll(delegate(Cliente _c) { return cDM.VerificaValiditaCard(_c.card) != WelcomeLibrary.DOM.enumclass.StatoCard.attiva; });

                mailingDM mDM = new mailingDM();
                Mail mail = new Mail();

                foreach (Cliente c in clientimail)
                {
                    //prepariamo le mail da inviare
                    mail = new Mail();

                    mail.DataInserimento = System.DateTime.Now;
                    mail.Id_card = c.Id_card;
                    mail.Id_cliente = c.Id_cliente;
                    mail.Lingua = c.Lingua;
                    mail.Tipomailing = (long)enumclass.TipoMailing.AvvisoNuovaofferta;
                    mail.NoteInvio = "";

                    mail.SoggettoMail = references.ResMan("Common", mail.Lingua, "oggettoMailInserimentoOfferta");
                    mail.TestoMail = references.ResMan("Common", mail.Lingua, "testoMailInserimentoOfferta") + "<br/>";

                    //Mettiamo anche il link alla pagina specifica dell'offerta appena inserita
                    string link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/SchedaContenuto.aspx?Lingua=" + c.Lingua.ToUpper() + "&idContenuto=" + item.Id + "&CodiceContenuto=" + item.CodiceContenuto;
                    mail.TestoMail += "<a href=\"" + link + "\" target=\"_blank\" style=\"font-size:22px;color:#b13c4e\">" + references.ResMan("Common", c.Lingua.ToUpper(), "TestoLinkAOfferta").ToString() + "<br/>";

                    mDM.InserisciAggiornaMail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, mail); //Inseriamo nel db per l'invio

                }
            }
        }
        catch (Exception error)
        {
            output.Text = error.Message;
            if (error.InnerException != null)
                output.Text += error.InnerException.Message.ToString();
        }
    }

    protected void btnCancella_Click(object sender, EventArgs e)
    {
        btnAggiorna.Text = "Modifica";
        ImpostaDettaglioSolaLettura(true);
        if (cancelHidden.Value == "true")
        {
            cancelHidden.Value = "false";
            try
            {
                Contenuti updrecord = new Contenuti();
                long tmp = 0;
                if (long.TryParse(ContentIDSelected, out tmp))
                {
                    updrecord.Id = tmp;

                    //Devi cancellare anche le foto allegate altrimenti restano
                    //.....da fare!!!
                    //Scorro la lista delle foto e cancello quelle presenti
                    Contenuti item = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ContentIDSelected);
                    string pathDestinazione = Server.MapPath(PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected);
                    foreach (Allegato foto in item.FotoCollection_M)
                    {
                        //Eseguo la cancellazione
                        try
                        {
                            bool ret = conDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tmp, foto.NomeFile, "", pathDestinazione);
                        }
                        catch (Exception errodel)
                        {
                            output.Text = errodel.Message;
                        }
                    }


                    //Prima Cancello
                    conDM.DeleteContenuti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

                    WelcomeLibrary.UF.SitemapManager.EliminaUrlrewritebyIdContenuto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord.Id.ToString());


                    this.SvuotaDettaglio();
                    this.CaricaDati();

                    //ContenutiCollection list = conDM.CaricaContenutiPerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceContenuto);
                    //rptContenuti.DataSource = list;
                    //rptContenuti.DataBind();

                }
            }
            catch (Exception error)
            {
                output.Text = error.Message;
                if (error.InnerException != null)
                    output.Text += error.InnerException.Message.ToString();
            }
        }
    }

    protected void SvuotaDettaglio()
    {
        ddlTipologie.SelectedValue = "";
        ddlStruttura.SelectedValue = "0";
        btnAggiorna.Text = "Modifica";
        imgFoto.ImageUrl = "";
        NomeFotoSelezionata = "";
        rptImmagini.DataSource = null;
        rptImmagini.DataBind();
        ClientIDSelected = "";
        ContentIDSelected = "";
        txtTitoloI.Text = "";
        txtTitoloGB.Text = "";
        txtDescrizioneGB.Text = "";
        txtTitoloRU.Text = "";
        txtDescrizioneRU.Text = "";
        txtDescrizioneI.Text = "";
        tinyhtmlEditI.InnerText = "";
        tinyhtmlEditGB.InnerText = "";
        tinyhtmlEditRU.InnerText = "";

        txtCustomtitleI.Text = "";
        txtCustomtitleGB.Text = "";
        txtCustomtitleRU.Text = "";
        txtCustomdescI.Text = "";
        txtCustomdescGB.Text = "";
        txtCustomdescRU.Text = "";

        txtCanonicalI.Text = "";
        txtCanonicalGB.Text = "";
        txtCanonicalRU.Text = "";
        txtRobots.Text = "";

        txtData.Text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", System.DateTime.Now);
        //txtFotoSchema.Value = "";
        //txtFotoValori.Value = "";
    }
    protected void ImpostaDettaglioSolaLettura(bool valore)
    {
        txtCustomtitleI.ReadOnly = valore;
        txtCustomtitleGB.ReadOnly = valore;
        txtCustomtitleRU.ReadOnly = valore;
        txtCustomdescI.ReadOnly = valore;
        txtCustomdescGB.ReadOnly = valore;
        txtCustomdescRU.ReadOnly = valore;


        txtCanonicalI.ReadOnly = valore;
        txtCanonicalGB.ReadOnly = valore;
        txtCanonicalRU.ReadOnly = valore;
        txtRobots.ReadOnly = valore;

        txtTitoloI.ReadOnly = valore;
        txtTitoloGB.ReadOnly = valore;
        txtDescrizioneGB.ReadOnly = valore;
        txtTitoloRU.ReadOnly = valore;
        txtDescrizioneRU.ReadOnly = valore;
        txtDescrizioneI.ReadOnly = valore;
        ddlTipologie.Enabled = !valore;
        ddlStruttura.Enabled = !valore;
        //txtFotoSchema.Value = "";
        //txtFotoValori.Value = "";
    }


    protected void btnCaricafileRU_Click(object sender, EventArgs e)
    {
        //Controlliamo se ho selezionato un record
        if (ContentIDSelected == null || ContentIDSelected == "")
        {
            output.Text = "Selezionare un elemento per associare la foto";
            return;
        }
        long idSelected = 0;
        if (!long.TryParse(ContentIDSelected, out idSelected))
        {
            output.Text = "Selezionare un elemento per associare la foto";
            return;
        }

        string Nome = upFileRU.FileName;
        int maxdimxRU = 300;
        int maxdimyRU = 300;
        int.TryParse(resizeDimxRU.Text, out maxdimxRU);
        int.TryParse(resizeDimyRU.Text, out maxdimyRU);

        string pathCompletoFoto = CaricaFoto(upFileRU, PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected + "/", Nome, maxdimxRU, maxdimyRU);
        string htmlfotodainserire = "<img src=\"" + pathCompletoFoto + "\"  />";

        if (pathCompletoFoto.ToLower().Contains(".jpg") || pathCompletoFoto.ToLower().Contains("png") || pathCompletoFoto.ToLower().Contains(".gif"))
            htmlfotodainserire = "<img src=\"" + pathCompletoFoto + "\"  />";
        else
            htmlfotodainserire = "<a href=\"" + pathCompletoFoto + "\" >" + Nome + "</a>";

        if (!string.IsNullOrEmpty(pathCompletoFoto))
            // htmlEditRU.Content += htmlfotodainserire;
            tinyhtmlEditRU.InnerText += htmlfotodainserire;
    }


    protected void btnCaricafileGB_Click(object sender, EventArgs e)
    {
        //Controlliamo se ho selezionato un record
        if (ContentIDSelected == null || ContentIDSelected == "")
        {
            output.Text = "Selezionare un elemento per associare la foto";
            return;
        }
        long idSelected = 0;
        if (!long.TryParse(ContentIDSelected, out idSelected))
        {
            output.Text = "Selezionare un elemento per associare la foto";
            return;
        }

        string Nome = upFileGB.FileName;
        int maxdimxGB = 300;
        int maxdimyGB = 300;
        int.TryParse(resizeDimxGB.Text, out maxdimxGB);
        int.TryParse(resizeDimyGB.Text, out maxdimyGB);

        string pathCompletoFoto = CaricaFoto(upFileGB, PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected + "/", Nome, maxdimxGB, maxdimyGB);
        string htmlfotodainserire = "<img src=\"" + pathCompletoFoto + "\"  />";

        if (pathCompletoFoto.ToLower().Contains(".jpg") || pathCompletoFoto.ToLower().Contains("png") || pathCompletoFoto.ToLower().Contains(".gif"))
            htmlfotodainserire = "<img src=\"" + pathCompletoFoto + "\"  />";
        else
            htmlfotodainserire = "<a href=\"" + pathCompletoFoto + "\" >" + Nome + "</a>";

        if (!string.IsNullOrEmpty(pathCompletoFoto))
            // htmlEditGB.Content += htmlfotodainserire;
            tinyhtmlEditGB.InnerText += htmlfotodainserire;



    }
    protected void btnCaricafile_Click(object sender, EventArgs e)
    {
        //Controlliamo se ho selezionato un record
        if (ContentIDSelected == null || ContentIDSelected == "")
        {
            output.Text = "Selezionare un elemento per associare la foto";
            return;
        }
        long idSelected = 0;
        if (!long.TryParse(ContentIDSelected, out idSelected))
        {
            output.Text = "Selezionare un elemento per associare la foto";
            return;
        }

        string Nome = upFile.FileName;
        int maxdimx = 300;
        int maxdimy = 300;
        int.TryParse(resizeDimx.Text, out maxdimx);
        int.TryParse(resizeDimy.Text, out maxdimy);

        string pathCompletoFoto = CaricaFoto(upFile, PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected + "/", Nome, maxdimx, maxdimy);
        string htmlfotodainserire = "";

        if (pathCompletoFoto.ToLower().Contains(".jpg") || pathCompletoFoto.ToLower().Contains("png") || pathCompletoFoto.ToLower().Contains(".gif"))
            htmlfotodainserire = "<img src=\"" + pathCompletoFoto + "\"  />";
        else
            htmlfotodainserire = "<a href=\"" + pathCompletoFoto + "\" >" + Nome + "</a>";

        if (!string.IsNullOrEmpty(pathCompletoFoto))
            tinyhtmlEditI.InnerText += htmlfotodainserire;
        //htmlEditI.Content += htmlfotodainserire;

    }
    protected string CaricaFoto(FileUpload fileupload, string percorsovirtualedestinazione, string Nome, int maxdimx, int maxdimy)
    {
        string rethtml = "";
        string error = "";
        try
        {
            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            string pathDestinazione = Server.MapPath(percorsovirtualedestinazione);
            if (!Directory.Exists(pathDestinazione))
                Directory.CreateDirectory(pathDestinazione);
            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (fileupload.HasFile)
            {
                if (fileupload.PostedFile.ContentLength > 25000000)
                {

                    error += "La foto non può essere caricata perché supera 25MB!";
                }
                else
                {
                    //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
                    string NomeCorretto = fileupload.FileName.Replace("+", "");
                    NomeCorretto = NomeCorretto.Replace(" ", "-");
                    NomeCorretto = NomeCorretto.Replace("%", "");
                    NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
                    //Se la foto è presente la cancello
                    if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto))
                    {
                        File.Delete(pathDestinazione + "\\" + NomeCorretto);
                    }
                    if (fileupload.PostedFile.ContentType == "image/jpeg" || fileupload.PostedFile.ContentType == "image/pjpeg" || fileupload.PostedFile.ContentType == "image/gif" || fileupload.PostedFile.ContentType == "image/png")
                    {
                        bool ridimensiona = true;
                        //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                        if (filemanage.ResizeAndSave(fileupload.PostedFile.InputStream, maxdimx, maxdimy, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                        {
                            //Creo anche le anteprime e le varie versioni del file per dimensioni diverse
                            if (!filemanage.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto, true, true))
                                output.Text = ("Anteprima Allegato non salvata correttamente!");


                            rethtml = ReplaceAbsoluteLinks(percorsovirtualedestinazione + NomeCorretto);
                        }
                        else
                        {
                            error += ("La foto non è stata caricata! (Problema nel caricamento)");

                        }
                    }
                    else if (fileupload.PostedFile.ContentType == "application/pdf")
                    {
                        //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                        //    outupload.Text = ("La foto non è stata caricata! (Formato non previsto)"); 
                        fileupload.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                        rethtml = ReplaceAbsoluteLinks(percorsovirtualedestinazione + NomeCorretto);
                    }
                    else
                    {
                        //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                        //    outupload.Text = ("La foto non è stata caricata! (Formato non previsto)"); 
                        fileupload.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                        rethtml = ReplaceAbsoluteLinks(percorsovirtualedestinazione + NomeCorretto);
                        //error += ("La foto non è stata caricata! (Formato non previsto)");
                    }
                }
            }
            else { error += "Selezionare il file da caricare"; }
        }
        catch (Exception errorecaricamento)
        {
            error += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                error += errorecaricamento.InnerException.Message;
        }
        outupload.Text = error;
        return rethtml;
    }




    #region Gestione Foto allegate


    protected string ComponiUrlGalleriaAnteprima(string NomeAnteprima)
    {
        string url = "";

        url = PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected + "/" + NomeAnteprima;
        //PER I FILES CHE NON SONO IMMAGINI METTO UN'IMMAGINE FISSA
        if (!(NomeAnteprima.ToLower().EndsWith("jpg") || NomeAnteprima.ToLower().EndsWith("gif") || NomeFotoSelezionata.ToLower().EndsWith("png")))
            url = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";



        return url;
    }
    protected void btnModifica_Click(object sender, EventArgs e)
    {
        try
        {
            long i = 0;
            long.TryParse(ContentIDSelected, out i);
            bool ret = conDM.modificaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, i, NomeFotoSelezionata, txtDescrizione.Text);
        }
        catch (Exception errins)
        {
            output.Text = errins.Message;
        }

        output.Text = "Foto Modificata Correttamente";
        //Aggiorniamo il repeater e la foto per il record selezionato
        this.CaricaDati();
    }
    protected void btnCarica_Click(object sender, EventArgs e)
    {
        try
        {
            //Controlliamo se ho selezionato un record
            if (ContentIDSelected == null || ContentIDSelected == "")
            {
                output.Text = "Selezionare un elemento per associare la foto";
                return;
            }
            long idSelected = 0;
            if (!long.TryParse(ContentIDSelected, out idSelected))
            {
                output.Text = "Selezionare un elemento per associare la foto";
                return;
            }
            Contenuti item = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ContentIDSelected);
            //if (item != null && item.FotoCollection_M.Count > 0)
            //{
            //    output.Text = "Foto già presente, eliminare foto prima di reinserire!";
            //    return;
            //}

            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            //Percorso files contenuti del tipo percorsobasecartellafiles/con000001/4
            string pathDestinazione = Server.MapPath(PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected);
            if (!Directory.Exists(pathDestinazione))
                Directory.CreateDirectory(pathDestinazione);

            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (UploadFoto.HasFile)
            {
                if (UploadFoto.PostedFile.ContentLength > 25000000)
                {

                    output.Text += "La foto non può essere caricata perché supera 25MB!";
                }
                else
                {
                    //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
                    string NomeCorretto = UploadFoto.FileName.Replace("+", "");
                    NomeCorretto = NomeCorretto.Replace("%", "");
                    NomeCorretto = NomeCorretto.Replace(" ", "-");
                    NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
                    //string NomeCorretto = Server.HtmlEncode(FotoUpload1.FileName);
                    if (System.IO.File.Exists(pathDestinazione))
                    {
                        output.Text += ("La foto non può essere caricata perché già presente sul server!");
                    }
                    else
                    {
                        //Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        if (UploadFoto.PostedFile.ContentType == "image/jpeg" || UploadFoto.PostedFile.ContentType == "image/pjpeg" || UploadFoto.PostedFile.ContentType == "image/gif" || UploadFoto.PostedFile.ContentType == "image/png")
                        {
                            int maxheight = 800;
                            int maxwidth = 1000;
                            bool ridimensiona = true;
                            //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                            if (filemanage.ResizeAndSave(UploadFoto.PostedFile.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                if (!filemanage.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto, true, true))
                                    output.Text = ("Anteprima Allegato non salvata correttamente!");

                                //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
                                //I DATI PER RINTRACCIARE LA FOTO-->SCHEMA E VALORI
                                try
                                {
                                    try
                                    {
                                        bool ret = conDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, txtDescrizione.Text);
                                    }
                                    catch (Exception errins)
                                    {
                                        output.Text = errins.Message;
                                    }

                                    output.Text += "Foto Inserita Correttamente";
                                    //Aggiorniamo il repeater e la foto per il record selezionato
                                    this.CaricaDati();
                                    //ContenutiCollection list = conDM.CaricaContenutiPerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceContenuto);
                                    //rptContenuti.DataSource = list;
                                    //rptContenuti.DataBind();


                                }
                                catch (Exception error)
                                {
                                    //CANCELLO LA FOTO UPLOADATA
                                    if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                    if (System.IO.File.Exists(pathDestinazione + "\\" + "Ant" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + "Ant" + NomeCorretto);
                                    //AGGIORNO IL DETAILSVIEW
                                    output.Text = error.Message;
                                }
                            }
                            else { output.Text += ("La foto non è stata caricata! (Problema nel caricamento)"); }
                        }
                        else { output.Text += ("La foto non è stata caricata! (Formato non previsto)"); }
                    }
                }
            }
            else { output.Text += "Selezionare il file da caricare"; }
        }
        catch (Exception errorecaricamento)
        {
            output.Text += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                output.Text += errorecaricamento.InnerException.Message;
        }
    }


    protected void btnElimina_Click(object sender, EventArgs e)
    {
        //Controlliamo se ho selezionato un record
        if (ContentIDSelected == null || ContentIDSelected == "")
        {
            output.Text = "Selezionare un elemento per cancellare la foto";
            return;
        }

        if (NomeFotoSelezionata == null || NomeFotoSelezionata == "")
        {
            output.Text = "Selezionare una foto da cancellare";
            return;
        }
        long idSelected = 0;
        if (!long.TryParse(ContentIDSelected, out idSelected))
        {
            output.Text = "Selezionare un elemento per cancellare la foto";
            return;
        }
        //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
        //Percorso files contenuti del tipo percorsobasecartellafiles/con000001/4
        string pathDestinazione = Server.MapPath(PercorsoFiles + "/" + CodiceContenuto + "/" + ContentIDSelected);
        //if (!Directory.Exists(pathDestinazione))
        //    Directory.CreateDirectory(pathDestinazione);
        //-------------------------------------
        //Eliminiamo il file se presente
        //-------------------------------------
        try
        {

            try
            {
                bool ret = conDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeFotoSelezionata, "", pathDestinazione);
            }
            catch (Exception errodel)
            {
                output.Text = errodel.Message;
            }


            //Aggiorniamo il repeater e la foto per il record selezionato
            this.CaricaDati();
            //ContenutiCollection list = conDM.CaricaContenutiPerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceContenuto);
            //rptContenuti.DataSource = list;
            //rptContenuti.DataBind();
            ////imgFoto.ImageUrl = "";
            //txtFotoSchema.Value = "";
            //txtFotoValori.Value = "";
        }
        catch (Exception errore)
        {
            output.Text += " " + errore.Message;
        }
    }




    #endregion


}