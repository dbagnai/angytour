using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Drawing.Imaging;
using System.Data.SQLite;


public partial class AreaContenuti_Default3 : CommonPage
{
   // offerteDM offDM = new offerteDM();
    public string ClientIDSelected
    {
        get { return ViewState["ClientIDSelected"] != null ? (string)(ViewState["ClientIDSelected"]) : ""; }
        set { ViewState["ClientIDSelected"] = value; }
    }
    public string OffertaIDSelected
    {
        get { return ViewState["OffertaIDSelected"] != null ? (string)(ViewState["OffertaIDSelected"]) : ""; }
        set { ViewState["OffertaIDSelected"] = value; }
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
    public string TipologiaOfferte
    {
        get { return ViewState["TipologiaOfferte"] != null ? (string)(ViewState["TipologiaOfferte"]) : ""; }
        set { ViewState["TipologiaOfferte"] = value; }
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
    public string testoricerca
    {
        get { return ViewState["testoricerca"] != null ? (string)(ViewState["testoricerca"]) : ""; }
        set { ViewState["testoricerca"] = value; }
    }
    public string CodiceProdotto
    {
        get { return ViewState["CodiceProdotto"] != null ? (string)(ViewState["CodiceProdotto"]) : ""; }
        set { ViewState["CodiceProdotto"] = value; }
    }
    public string CodiceSottoProdotto
    {
        get { return ViewState["CodiceSottoProdotto"] != null ? (string)(ViewState["CodiceSottoProdotto"]) : ""; }
        set { ViewState["CodiceSottoProdotto"] = value; }
    }

    public string CodiceProdottoRicerca
    {
        get { return ViewState["CodiceProdottoRicerca"] != null ? (string)(ViewState["CodiceProdottoRicerca"]) : ""; }
        set { ViewState["CodiceProdottoRicerca"] = value; }
    }
    public string CodiceSottoProdottoRicerca
    {
        get { return ViewState["CodiceSottoProdottoRicerca"] != null ? (string)(ViewState["CodiceSottoProdottoRicerca"]) : ""; }
        set { ViewState["CodiceSottoProdottoRicerca"] = value; }
    }
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string Autore
    {
        get { return ViewState["Autore"] != null ? (string)(ViewState["Autore"]) : ""; }
        set { ViewState["Autore"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            /////////////////////////////////////////////////////////////////////
            //Verifichiamo accesso  e impostiamo la visualizzazione corretta
            //Spegnendo le cose che non devono essere visibili ai soci!!!
            /////////////////////////////////////////////////////////////////////
            usermanager USM = new usermanager();
            if (USM.ControllaRuolo(User.Identity.Name, "Autore"))
            {
                Autore = User.Identity.Name;
                ImpostaVisualizzazione();
            }


            Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);

            PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
            PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;

            if (Request.QueryString["CodiceTipologia"] != null && Request.QueryString["CodiceTipologia"] != "")
            { TipologiaOfferte = Request.QueryString["CodiceTipologia"].ToString(); }
            if (TipologiaOfferte == null || TipologiaOfferte == "")
                Response.Redirect("default.aspx?Errore=Selezionare Tipo Offerta");
            hidTipologia.Value = TipologiaOfferte;

            if (Request.QueryString["CodiceProdotto"] != null && Request.QueryString["CodiceProdotto"] != "")
            { CodiceProdotto = Request.QueryString["CodiceProdotto"].ToString(); }

            if (TipologiaOfferte == "rif000061" || TipologiaOfferte == "rif000062")
            {
                pnlIndirizzo0.Visible = false;
                pnlIndirizzo1.Visible = true;
            }

            //Carichiamo i dati relativi al contenuto specificato
            //Da fare repeater paginato con i risultati della query sul db
            litTitolo.Text = (Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return tmp.Lingua == "I" && tmp.Codice == TipologiaOfferte; })).Descrizione;

            CaricaDllLocalizzazione("IT", "", "", "", ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);


            //Carichiamo le ddl per la collocazione geografica senza selezioni
            // CaricaDatiDdlRicercaRepeater("", "");
            this.CaricaDatiDdlRicerca("", "", "", "", "");
            CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);

            this.CaricaDatiDllProdotto(TipologiaOfferte, "");
            this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");

            this.CaricaDati();
            ImpostaDettaglioSolaLettura(true);

        }
        else
        {
            if (Request["__EVENTTARGET"] == "aggiornatettaglio")
            {
                //  string email = Request["__EVENTARGUMENT"];
                CaricaDati();
            }
            output.Text = "";
            ErrorMessage.Text = "";
            ErrorMsgNuovoProdotto.Text = "";

        }
        //CodiceProdottoRicerca = ddlProdottoRicerca.SelectedValue;
        //CodiceSottoProdottoRicerca = ddlSProdottoRicerca.SelectedValue;
    }

    private void ImpostaVisualizzazione()
    {
        string idcliente = getidcliente(User.Identity.Name);
        if (!string.IsNullOrEmpty(idcliente))
        {

            //((HtmlGenericControl)Master.FindControl("ulMainbar")).Visible = false; //Spengo la barra navigazione
            ((HtmlGenericControl)Master.FindControl("tagPagineStatiche")).Visible = false; //Spengo la barra navigazione
            ((HtmlGenericControl)Master.FindControl("tagBanner")).Visible = false; //Spengo la barra navigazione
            ((HtmlGenericControl)Master.FindControl("tagContatti")).Visible = false; //Spengo la barra navigazione
            ((HtmlGenericControl)Master.FindControl("tagConfig")).Visible = false; //Spengo la barra navigazione


        }
        else
        {
            Response.Redirect("~/Error.aspx?Error=Utente non trovato");
        }

    }

    #region PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER

    protected void PagerRisultati_PageCommand(object sender, string PageNum)
    {
        UC_PagerEx PagerRisultati = sender as UC_PagerEx;
        PagerRisultati.CurrentPage = Convert.ToInt32(PageNum);
        CaricaDati();
    }

    protected void PagerRisultati_PageGroupClickNext(object sender, string spare)
    {
        //dimensioneGruppo
        //nGruppoPagine
        CaricaDati();
    }

    protected void PagerRisultati_PageGroupClickPrev(object sender, string spare)
    {
        CaricaDati();
    }

    #endregion


    private void CaricaDati()
    {
        OfferteCollection offerte = null;
        try
        {

            #region Versione con db ACCESS
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
#if true
            //if (tipologia == "") tipologia = "%";
            if (TipologiaOfferte != "")
            {
                SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", TipologiaOfferte);
                parColl.Add(p3);
            }
            if (testoricerca.Trim() != "")
            {
                testoricerca = testoricerca.Replace(" ", "%");
                SQLiteParameter p7 = new SQLiteParameter("@testoricerca", "%" + testoricerca + "%");
                parColl.Add(p7);
            }
            if (Autore.Trim() != "")
            {
                SQLiteParameter pautore = new SQLiteParameter("@Autore", Autore);
                parColl.Add(pautore);
            }

            if (mese.Trim() != "" || anno.Trim() != "")
            {
                int _a = 0;
                int.TryParse(anno, out _a);
                int _m = 0;
                int.TryParse(mese, out _m);
                if (_a != 0)
                {
                    SQLiteParameter p8 = new SQLiteParameter("@annofiltro", _a); //Data registr. ordine
                    parColl.Add(p8);
                }
                if (_m != 0)
                {
                    SQLiteParameter p9 = new SQLiteParameter("@mesefiltro", _m); //Data registr. ordine
                    parColl.Add(p9);
                }
            }
            if (CodiceProdotto != "")
            {
                SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", CodiceProdotto);
                parColl.Add(p7);
            }
            if (ddlSottoProdSearch.SelectedValue != "")
            {
                SQLiteParameter p8 = new SQLiteParameter("@CodiceCategoria2Liv", CodiceSottoProdottoRicerca);
                parColl.Add(p8);
            }

            offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", null, null, "", true, PagerRisultati.CurrentPage, PagerRisultati.PageSize);

#endif
            #endregion

            //offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
            if (offerte == null || offerte.Count == 0)
            {
                rptOfferte.DataSource = offerte;
                rptOfferte.DataBind();
                output.Text = "Nessun valore trovato per le selezioni fatte";
                return;
            }
            //offerte.Sort(new GenericComparer<Offerte>("DataInserimento", System.ComponentModel.ListSortDirection.Descending));

        }
        catch (Exception error)
        {
            output.Text = error.Message.ToString();
            return;
        }

        long nrecordfiltrati = offerte.Totrecs;
        PagerRisultati.TotalRecords = (long)offerte.Totrecs;
        if (nrecordfiltrati == 0) PagerRisultati.CurrentPage = 1;

#if false
        //Selezionamo i risultati in base al numero di pagina e alla sua dimensione per la paginazione
        //Utilizzando la classe di paginazione
        WelcomeLibrary.UF.Pager<Offerte> _pager = new WelcomeLibrary.UF.Pager<Offerte>(offerte);
        int nrecordfiltrati = _pager.Count;
        PagerRisultati.TotalRecords = nrecordfiltrati;
        if (nrecordfiltrati == 0) PagerRisultati.CurrentPage = 1;
        rptOfferte.DataSource = _pager.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);

#endif
        rptOfferte.DataSource = offerte;
        rptOfferte.DataBind();

        //Aggiorno la vista del dettaglio
        this.AggiornaDettaglio(OffertaIDSelected);
    }

    private void AggiornaDettaglio(string OffertaIDSelected)
    {
        //Riempiamo i dati del dettaglio
        Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);
        if (Details != null)
        {
            txtCampo1I.Text = Details.Campo1I;
            txtCampo2I.Text = Details.Campo2I;
            txtCampo1GB.Text = Details.Campo1GB;
            txtCampo2GB.Text = Details.Campo2GB;

            txtCampo1RU.Text = Details.Campo1RU;
            txtCampo2RU.Text = Details.Campo2RU;

            txtIdcollegato.Text = Details.Id_collegato.ToString();

            txtDenominazioneI.Text = Details.DenominazioneI;//(((Literal)e.Item.FindControl("lit1")).Text);
            txtDenominazioneGB.Text = Details.DenominazioneGB;//(((Literal)e.Item.FindControl("lit2")).Text);
            txtDenominazioneRU.Text = Details.DenominazioneRU;//(((Literal)e.Item.FindControl("lit2")).Text);
            txtDescrizioneI.Text = Details.DescrizioneI;//(((HiddenField)e.Item.FindControl("hid3")).Value);
            txtDescrizioneGB.Text = Details.DescrizioneGB;//(((HiddenField)e.Item.FindControl("hid4")).Value);
            txtDescrizioneRU.Text = Details.DescrizioneRU;//(((HiddenField)e.Item.FindControl("hid4")).Value);
            txtDatitecniciI.Text = Details.DatitecniciI;
            txtDatitecniciGB.Text = Details.DatitecniciGB;
            txtDatitecniciRU.Text = Details.DatitecniciRU;
            txtIndirizzo.Text = Details.Indirizzo;
            txtEmail.Text = Details.Email;
            txtWebsite.Text = Details.Website;
            txtTelefono.Text = Details.Telefono;
            txtFax.Text = Details.Fax;
            txtVideo.Text = Details.linkVideo;
            txtAutore.Text = Details.Autore;

            txtPrezzo.Text = Details.Prezzo.ToString();
            txtPrezzoListino.Text = Details.PrezzoListino.ToString();
            txtQta_vendita.Text = (Details.Qta_vendita != null) ? Details.Qta_vendita.ToString() : "";


            chkVetrina.Checked = Details.Vetrina;
            chkArchiviato.Checked = Details.Archiviato;
            chkPromozione.Checked = Details.Promozione;

            chkContatto.Checked = Details.Abilitacontatto;

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("it");
            txtData.Text = string.Format(ci, "{0:dd/MM/yyyy HH:mm:ss}", new object[] { Details.DataInserimento });


            txtLatitudine1_dts.Text = String.Format(ci, "{0:##.#####################}", new object[] { Details.Latitudine1_dts });
            txtLongitudine1_dts.Text = String.Format(ci, "{0:##.#####################}", new object[] { Details.Longitudine1_dts });

            txtCodiceProd.Text = Details.CodiceProdotto;

            txtAnno.Text = Details.Anno.ToString();
            CaricaDatiDdlCaratteristiche(Details.Caratteristica1, Details.Caratteristica2, Details.Caratteristica3, Details.Caratteristica4, Details.Caratteristica5, Details.Caratteristica6);

            CaricaDatiDdlRicerca(Details.CodiceRegione, Details.CodiceProvincia, Details.CodiceComune, Details.CodiceCategoria, Details.CodiceCategoria2Liv);

            CaricaDllLocalizzazione(Details.CodiceNAZIONE1_dts, Details.CodiceREGIONE1_dts, Details.CodicePROVINCIA1_dts, Details.CodiceCOMUNE1_dts, ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);


            //Impostiamo l'url per la foto grande(Prendo sempre la prima quando cambio elemento del repeater)
            if (Details.FotoCollection_M != null && Details.FotoCollection_M.Count > 0)
            {
                imgFoto.ImageUrl = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + Details.FotoCollection_M[0].NomeFile;
                NomeFotoSelezionata = Details.FotoCollection_M[0].NomeFile;
                txtDescrizione.Text = Details.FotoCollection_M[0].Descrizione;
                txtProgressivo.Text = Details.FotoCollection_M[0].Progressivo.ToString();
            }
            else
            {
                imgFoto.ImageUrl = "";
                NomeFotoSelezionata = "";
                txtDescrizione.Text = "";
                txtProgressivo.Text = "";

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



    protected void ImgBtnCerca_Click(object sender, EventArgs e)
    {
        //testoricerca
        testoricerca = Server.HtmlEncode(txtinputCerca.Text);
        mese = txtinputmese.Text;
        anno = txtinputanno.Text;
        CaricaDati();
    }



#if false
    private void CaricaDatiDdlRicercaRepeater(string Categoria, string SottoCategoria)
    {

        string SceltaTipologia = TipologiaOfferte;
        List<WelcomeLibrary.DOM.Prodotto> prodotti = new List<WelcomeLibrary.DOM.Prodotto>();
        prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == TipologiaOfferte || TipologiaOfferte == "")); });
        if (!string.IsNullOrEmpty(SceltaTipologia))
        {
            prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == SceltaTipologia || SceltaTipologia == "")); });
        }
        prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));

        ddlProdottoRicerca.Items.Clear();
        ddlProdottoRicerca.Items.Insert(0, TestoTuttiProdotti("I"));
        ddlProdottoRicerca.Items[0].Value = "";
        ddlProdottoRicerca.DataSource = prodotti;
        ddlProdottoRicerca.DataTextField = "Descrizione";
        ddlProdottoRicerca.DataValueField = "CodiceProdotto";
        ddlProdottoRicerca.DataBind();
        try
        {
            ddlProdottoRicerca.SelectedValue = Categoria;
        }
        catch { }

        List<WelcomeLibrary.DOM.SProdotto> sprodotti = new List<WelcomeLibrary.DOM.SProdotto>();
        sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate(WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceProdotto == Categoria)); });
        sprodotti.Sort(new GenericComparer<SProdotto>("CodiceSProdotto", System.ComponentModel.ListSortDirection.Ascending));
        ddlSProdottoRicerca.Items.Clear();
        ddlSProdottoRicerca.Items.Insert(0, TestoTuttiSProdotti("I"));
        ddlSProdottoRicerca.Items[0].Value = "";
        ddlSProdottoRicerca.DataSource = sprodotti;
        ddlSProdottoRicerca.DataTextField = "Descrizione";
        ddlSProdottoRicerca.DataValueField = "CodiceSProdotto";
        ddlSProdottoRicerca.DataBind();
        try
        {
            ddlSProdottoRicerca.SelectedValue = SottoCategoria;
        }
        catch { }

    }
    

    protected void ddlProdottoRicerca_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicercaRepeater(CodiceProdottoRicerca, "");
        //this.CaricaDati();
    }
    protected void ddlSProdottoRicerca_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CaricaDati();
    }
#endif
    protected void ddlSottoProdSearch_SelectedIndexChange(object sender, EventArgs e)
    {
        ////Qui devo mettere una funzione che riempie i dati con il nome (ITA / ENG / RU) del prodotto selezionato
        ////Di modo da poterlo modificare
        //CodiceProdotto = ddlProdottoNewProd1.SelectedValue;
        //CaricaDatiFormInserimento(ddlTipologiaNewProd.SelectedValue, ddlProdottoNewProd1.SelectedValue);
        //btnModificaProd.Enabled = true;
        //NomeNuovoProdIt.Enabled = true;
        //NomeNuovoProdEng.Enabled = true;
        //NomeNuovoProdRu.Enabled = true;
        ////OkButton.Enabled = false;
        //OkButton.Text = "Annulla";
        //btnModificaProd.Text = "Salva";
        CodiceSottoProdottoRicerca = ddlSottoProdSearch.SelectedValue;
    }


    /// <summary>
    /// Carica i dati nelle ddl regione/prov/comune 
    /// selezionando i valori passati se coerenti
    /// CodiceRegione,CodiceProvincia,Nome Comune
    /// </summary>
    /// <param name="Regione">Codice della Regione</param>
    /// <param name="Provincia">Codice della Provincia</param>
    /// <param name="Comune">nome del Comune</param>
    private void CaricaDatiDdlRicerca(string Regione, string Provincia, string Comune, string Categoria, string SottoCategoria)
    {
        WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
        List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I"); });
        if (provincelingua != null)
        {
            provincelingua.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
            foreach (Province item in provincelingua)
            {
                if (item.Lingua == "I")
                    if (!regioni.Exists(delegate (Province tmp) { return (tmp.Regione == item.Regione); }))
                        regioni.Add(item);
            }
        }
        //regioni.Sort(new GenericComparer<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending));
        ddlRegione.Items.Clear();
        ddlRegione.Items.Insert(0, references.ResMan("Common", Lingua, "ddlTuttiregione"));
        ddlRegione.Items[0].Value = "";
        ddlRegione.DataSource = regioni;
        ddlRegione.DataTextField = "Regione";
        ddlRegione.DataValueField = "Codice";
        ddlRegione.DataBind();
        try
        {
            ddlRegione.SelectedValue = Regione;
        }
        catch { }
        //Province
        ddlProvincia.Items.Clear();
        ddlProvincia.Items.Insert(0, references.ResMan("Common", Lingua, "ddlTuttiprovincia"));
        ddlProvincia.Items[0].Value = "";
        if (Regione != "")
        {
            provincelingua = null;
            Province _tmp = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.Codice == ddlRegione.SelectedValue); });
            if (_tmp != null)
            {
                provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.CodiceRegione == _tmp.CodiceRegione); });
                provincelingua.Sort(new GenericComparer<Province>("Provincia", System.ComponentModel.ListSortDirection.Ascending));
            }
            ddlProvincia.DataSource = provincelingua;
            ddlProvincia.DataTextField = "Provincia";
            ddlProvincia.DataValueField = "Codice";
            ddlProvincia.DataBind();
            try
            {
                ddlProvincia.SelectedValue = Provincia;
            }
            catch { }
        }
        //Comuni
        ddlComune.Items.Clear();
        ddlComune.Items.Insert(0, references.ResMan("Common", Lingua, "ddlTuttiComune"));
        ddlComune.Items[0].Value = "";
        if (Provincia != "")
        {
            List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate (WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == Provincia); });
            if (comunilingua != null)
                comunilingua.Sort(new GenericComparer<Comune>("Nome", System.ComponentModel.ListSortDirection.Ascending));
            ddlComune.DataSource = comunilingua;
            ddlComune.DataTextField = "Nome";
            ddlComune.DataValueField = "Nome";
            ddlComune.DataBind();
            try
            {
                ddlComune.SelectedValue = Comune;
            }
            catch { }
        }


        string SceltaTipologia = TipologiaOfferte;
        List<WelcomeLibrary.DOM.Prodotto> prodotti = new List<WelcomeLibrary.DOM.Prodotto>();
        prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == TipologiaOfferte || TipologiaOfferte == "")); });
        if (!string.IsNullOrEmpty(SceltaTipologia))
        {
            prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == SceltaTipologia || SceltaTipologia == "")); });
        }
        prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        ddlProdotto.Items.Clear();
        ddlProdotto.Items.Insert(0, references.ResMan("Common", Lingua, "selProdotti"));
        ddlProdotto.Items[0].Value = "";
        ddlProdotto.DataSource = prodotti;
        ddlProdotto.DataTextField = "Descrizione";
        ddlProdotto.DataValueField = "CodiceProdotto";
        ddlProdotto.DataBind();
        try
        {
            ddlProdotto.SelectedValue = Categoria;
        }
        catch { }

        List<WelcomeLibrary.DOM.SProdotto> sprodotti = new List<WelcomeLibrary.DOM.SProdotto>();
        sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceProdotto == ddlProdotto.SelectedValue)); });
        sprodotti.Sort(new GenericComparer<SProdotto>("CodiceSProdotto", System.ComponentModel.ListSortDirection.Ascending));
        ddlSottoProdotto.Items.Clear();
        ddlSottoProdotto.Items.Insert(0, references.ResMan("Common", Lingua, "selSProdotti"));
        ddlSottoProdotto.Items[0].Value = "";
        ddlSottoProdotto.DataSource = sprodotti;
        ddlSottoProdotto.DataTextField = "Descrizione";
        ddlSottoProdotto.DataValueField = "CodiceSProdotto";
        ddlSottoProdotto.DataBind();
        try
        {
            ddlSottoProdotto.SelectedValue = SottoCategoria;
        }
        catch { }


        sprodotti = new List<WelcomeLibrary.DOM.SProdotto>();
        sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceProdotto == Categoria)); });
        sprodotti.Sort(new GenericComparer<SProdotto>("CodiceSProdotto", System.ComponentModel.ListSortDirection.Ascending));
        ddlSottoProdSearch.Items.Clear();
        ddlSottoProdSearch.Items.Insert(0, references.ResMan("Common", Lingua, "selSProdotti"));
        ddlSottoProdSearch.Items[0].Value = "";
        ddlSottoProdSearch.DataSource = sprodotti;
        ddlSottoProdSearch.DataTextField = "Descrizione";
        ddlSottoProdSearch.DataValueField = "CodiceSProdotto";
        ddlSottoProdSearch.DataBind();
        try
        {
            ddlSottoProdSearch.SelectedValue = "";
            CodiceSottoProdottoRicerca = "";
        }
        catch { }

    }
    protected void ddlProdotto_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca("", "", "", ddlProdotto.SelectedValue, "");
    }
    protected void ddlRegione_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca(ddlRegione.SelectedValue, "", "", ddlProdotto.SelectedValue, ddlSottoProdotto.SelectedValue);
    }

    protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca(ddlRegione.SelectedValue, ddlProvincia.SelectedValue, "", ddlProdotto.SelectedValue, ddlSottoProdotto.SelectedValue);
    }


    protected void rptOfferte_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                //QUI CONTROLLO ED EVIDENZIO LA RIGA DEL REPEATER ATTUALMENTE SELEZIONATA
                LinkButton lnkImg = (LinkButton)e.Item.FindControl("imgSelect");
                if (lnkImg != null) if (lnkImg.CommandArgument == OffertaIDSelected && OffertaIDSelected != "")
                    {
                        //    //Riempiamo i dati del dettaglio
                        //    Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);
                        //    txtDenominazioneI.Text = Details.DenominazioneI;//(((Literal)e.Item.FindControl("lit1")).Text);
                        //    txtDenominazioneGB.Text = Details.DenominazioneGB;//(((Literal)e.Item.FindControl("lit2")).Text);
                        //    txtDescrizioneI.Text = Details.DescrizioneI;//(((HiddenField)e.Item.FindControl("hid3")).Value);
                        //    txtDescrizioneGB.Text = Details.DescrizioneGB;//(((HiddenField)e.Item.FindControl("hid4")).Value);
                        //    txtDatitecniciI.Text = Details.DatitecniciI;
                        //    txtIndirizzo.Text = Details.Indirizzo;
                        //    txtEmail.Text = Details.Email;
                        //    txtWebsite.Text = Details.Website;
                        //    txtTelefono.Text = Details.Telefono;
                        //    txtFax.Text = Details.Fax;
                        //    CaricaDatiDdlRicerca(Details.CodiceRegione, Details.CodiceProvincia, Details.CodiceComune);

                        //    //txtFotoSchema.Value = Details.FotoCollection_M.Schema;//(((HiddenField)e.Item.FindControl("hid5")).Value);
                        //    //txtFotoValori.Value = Details.FotoCollection_M.Valori;//(((HiddenField)e.Item.FindControl("hid6")).Value);
                        //    //Impostiamo l'url per la foto grande(Prendo sempre la prima quando cambio elemento del repeater)
                        //    if (Details.FotoCollection_M != null && Details.FotoCollection_M.Count > 0)
                        //    {
                        //        imgFoto.ImageUrl = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + Details.FotoCollection_M[0].NomeFile;
                        //        NomeFotoSelezionata = Details.FotoCollection_M[0].NomeFile;
                        //    }
                        //    else
                        //        imgFoto.ImageUrl = "";
                        //    //Carichiamo la galleria delle foto
                        //    rptImmagini.DataSource = Details.FotoCollection_M;
                        //    rptImmagini.DataBind();

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
        //if (((LinkButton)sender).CommandArgument == OffertaIDSelected)
        //{
        //    ((LinkButton)sender).BackColor = System.Drawing.ColorTranslator.FromHtml("#000000");

        //}
        //else
        //{
        //    ((LinkButton)sender).BackColor = System.Drawing.ColorTranslator.FromHtml("#fff"); //System.Drawing.Color.Transparent;
        //};

        if (((LinkButton)sender).CommandArgument == OffertaIDSelected)
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
        OffertaIDSelected = ((LinkButton)(sender)).CommandArgument.ToString();
        ClientIDSelected = ((LinkButton)(sender)).ClientID;
        hidIdselected.Value = OffertaIDSelected;
        NomeFotoSelezionata = "";

        this.AggiornaDettaglio(OffertaIDSelected);
        btnCancella.Enabled = true;
        btnAggiorna.Enabled = true;
        btnAggiorna.ValidationGroup = "";
    }
    protected void linkgalleria_click(object sender, EventArgs e)
    {
        string serializedallegato = ((ImageButton)(sender)).CommandArgument.ToString();
        Allegato allegato = Newtonsoft.Json.JsonConvert.DeserializeObject<Allegato>(serializedallegato
                  , new Newtonsoft.Json.JsonSerializerSettings()
                  {
                      DateFormatString = "dd/MM/yyyy HH:mm:ss",
                      //DateFormatString = "dd/MM/yyyy",
                      MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                      ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                      PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None
                  });

        NomeFotoSelezionata = allegato.NomeFile;
        //NomeFotoSelezionata = ((ImageButton)(sender)).CommandArgument.ToString();


        string percorsofile = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + NomeFotoSelezionata;
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

        //txtDescrizione.Text = ((ImageButton)(sender)).ToolTip;
        txtDescrizione.Text = allegato.Descrizione;
        txtProgressivo.Text = allegato.Progressivo.ToString();
    }
    protected void btnNuovo_Click(object sender, EventArgs e)
    {
        this.SvuotaDettaglio();
        ImpostaDettaglioSolaLettura(false);
        prefillDettaglio();
        btnAggiorna.Text = "Inserisci";
        btnAggiorna.ValidationGroup = "Insertvalidate";
        btnCancella.Visible = false;
        btnCancella.Enabled = false;
        btnAnnulla.Visible = true;
        btnAnnulla.Enabled = true;
    }


    protected void btnAggiorna_Click(object sender, EventArgs e)
    {
        btnCancella.Visible = false;
        btnCancella.Enabled = false;
        btnAnnulla.Visible = true;
        btnAnnulla.Enabled = true;

        //valutazioni updrecord = new valutazioni();
        Offerte updrecord = new Offerte();
        try
        {
            // ArrayList tipologie = valDM.CaricaTipologie("Access.dbEdil2000");
            if (btnAggiorna.Text == "Modifica")
            {
                btnAggiorna.Text = "Aggiorna";
                btnAggiorna.ValidationGroup = "Insertvalidate";
                ImpostaDettaglioSolaLettura(false);
                return;
            }
            if (btnAggiorna.Text == "Aggiorna")
            {
                updrecord = new Offerte();
                long tmp = 0;
                if (long.TryParse(OffertaIDSelected, out tmp))
                {
                    updrecord.Id = tmp;
                    updrecord = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);


                    long tmpcoll = 0;
                    if (long.TryParse(txtIdcollegato.Text, out tmpcoll))
                        updrecord.Id_collegato = tmpcoll;

                    tmpcoll = 0;
                    if (long.TryParse(txtAnno.Text, out tmpcoll))
                        updrecord.Anno = tmpcoll;
                    tmpcoll = 0;
                    if (long.TryParse(ddlCaratteristica1.SelectedValue, out tmpcoll))
                        updrecord.Caratteristica1 = tmpcoll;
                    tmpcoll = 0;
                    if (long.TryParse(ddlCaratteristica2.SelectedValue, out tmpcoll))
                        updrecord.Caratteristica2 = tmpcoll;

                    tmpcoll = 0;
                    if (long.TryParse(ddlCaratteristica3.SelectedValue, out tmpcoll))
                        updrecord.Caratteristica3 = tmpcoll;
                    tmpcoll = 0;
                    if (long.TryParse(ddlCaratteristica4.SelectedValue, out tmpcoll))
                        updrecord.Caratteristica4 = tmpcoll;
                    tmpcoll = 0;
                    if (long.TryParse(ddlCaratteristica5.SelectedValue, out tmpcoll))
                        updrecord.Caratteristica5 = tmpcoll;
                    if (long.TryParse(ddlCaratteristica6.SelectedValue, out tmpcoll))
                        updrecord.Caratteristica6 = tmpcoll;

                    updrecord.Campo1I = txtCampo1I.Text;
                    updrecord.Campo2I = txtCampo2I.Text;
                    updrecord.Campo1GB = txtCampo1GB.Text;
                    updrecord.Campo2GB = txtCampo2GB.Text;

                    updrecord.Campo1RU = txtCampo1RU.Text;
                    updrecord.Campo2RU = txtCampo2RU.Text;

                    updrecord.DenominazioneI = txtDenominazioneI.Text;
                    updrecord.DenominazioneGB = txtDenominazioneGB.Text;
                    updrecord.DenominazioneRU = txtDenominazioneRU.Text;
                    updrecord.DescrizioneI = txtDescrizioneI.Text;
                    updrecord.DescrizioneGB = txtDescrizioneGB.Text;
                    updrecord.DatitecniciGB = txtDatitecniciGB.Text;
                    updrecord.DescrizioneRU = txtDescrizioneRU.Text;
                    updrecord.DatitecniciRU = txtDatitecniciRU.Text;

                    updrecord.DatitecniciI = txtDatitecniciI.Text;
                    updrecord.Indirizzo = txtIndirizzo.Text;
                    updrecord.Email = txtEmail.Text;
                    updrecord.Website = txtWebsite.Text;
                    updrecord.Telefono = txtTelefono.Text;
                    updrecord.Fax = txtFax.Text;
                    updrecord.linkVideo = txtVideo.Text;
                    updrecord.CodiceComune = ddlComune.SelectedValue;
                    updrecord.CodiceProvincia = ddlProvincia.SelectedValue;
                    updrecord.CodiceRegione = ddlRegione.SelectedValue;

                    updrecord.CodiceProdotto = txtCodiceProd.Text;

                    updrecord.CodiceCategoria = ddlProdotto.SelectedValue;
                    updrecord.CodiceCategoria2Liv = ddlSottoProdotto.SelectedValue;

                    updrecord.Vetrina = chkVetrina.Checked;
                    updrecord.Archiviato = chkArchiviato.Checked;
                    updrecord.Abilitacontatto = chkContatto.Checked;
                    updrecord.Promozione = chkPromozione.Checked;

                    double _tmpdbl = 0;
                    double.TryParse(txtPrezzo.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Prezzo = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtPrezzoListino.Text, out _tmpdbl);//Mettere textbox per prezzo listino
                    updrecord.PrezzoListino = _tmpdbl;


                    _tmpdbl = 0;
                    if (double.TryParse(txtQta_vendita.Text, out _tmpdbl))
                    { updrecord.Qta_vendita = _tmpdbl; }
                    else
                        updrecord.Qta_vendita = null;

                    DateTime _tmpdate = System.DateTime.Now;
                    if (!DateTime.TryParse(txtData.Text, out _tmpdate))
                        _tmpdate = System.DateTime.Now;
                    updrecord.DataInserimento = _tmpdate;

                    updrecord.CodiceNAZIONE1_dts = ddlCodiceNAZIONE1_dts.SelectedValue;
                    updrecord.CodiceREGIONE1_dts = txtCodiceREGIONE1_dts.Value;
                    updrecord.CodicePROVINCIA1_dts = txtCodicePROVINCIA1_dts.Value;
                    updrecord.CodiceCOMUNE1_dts = txtCodiceCOMUNE1_dts.Value;

                    _tmpdbl = 0;
                    double.TryParse(txtLatitudine1_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Latitudine1_dts = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtLongitudine1_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Longitudine1_dts = _tmpdbl;

                    if (updrecord.Id_dts_collegato == 0)
                        offDM.InsertOffertaCollegata(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    else
                        offDM.UpdateOffertaCollegata(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    offDM.UpdateOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);


                    btnAggiorna.Text = "Modifica";
                    btnAggiorna.ValidationGroup = "";

                    btnAnnulla.Visible = false;
                    ImpostaDettaglioSolaLettura(true);
                    btnCancella.Visible = true;
                    btnCancella.Enabled = true;
                    btnAggiorna.Enabled = true;
                }
            }
            else
                if (btnAggiorna.Text == "Inserisci")
            {
                //if (string.IsNullOrEmpty(ddlProdotto.SelectedValue) || string.IsNullOrEmpty(ddlSottoProdotto.SelectedValue))
                //{
                //    output.Text = "Selezionare categoria di appartenenza prodotto/sottoprodotto";
                //    return;
                //}
                updrecord = new Offerte();
                updrecord.Autore = User.Identity.Name;
                updrecord.CodiceTipologia = TipologiaOfferte;

                long tmpcoll = 0;
                if (long.TryParse(txtIdcollegato.Text, out tmpcoll))
                    updrecord.Id_collegato = tmpcoll;

                tmpcoll = 0;
                if (long.TryParse(txtAnno.Text, out tmpcoll))
                    updrecord.Anno = tmpcoll;
                tmpcoll = 0;
                if (long.TryParse(ddlCaratteristica1.SelectedValue, out tmpcoll))
                    updrecord.Caratteristica1 = tmpcoll;
                tmpcoll = 0;
                if (long.TryParse(ddlCaratteristica2.SelectedValue, out tmpcoll))
                    updrecord.Caratteristica2 = tmpcoll;

                tmpcoll = 0;
                if (long.TryParse(ddlCaratteristica3.SelectedValue, out tmpcoll))
                    updrecord.Caratteristica3 = tmpcoll;
                tmpcoll = 0;
                if (long.TryParse(ddlCaratteristica4.SelectedValue, out tmpcoll))
                    updrecord.Caratteristica4 = tmpcoll;
                tmpcoll = 0;
                if (long.TryParse(ddlCaratteristica5.SelectedValue, out tmpcoll))
                    updrecord.Caratteristica5 = tmpcoll;
                if (long.TryParse(ddlCaratteristica6.SelectedValue, out tmpcoll))
                    updrecord.Caratteristica6 = tmpcoll;

                updrecord.Campo1I = txtCampo1I.Text;
                updrecord.Campo2I = txtCampo2I.Text;
                updrecord.Campo1GB = txtCampo1GB.Text;
                updrecord.Campo2GB = txtCampo2GB.Text;
                updrecord.Campo1RU = txtCampo1RU.Text;
                updrecord.Campo2RU = txtCampo2RU.Text;

                updrecord.DenominazioneI = txtDenominazioneI.Text;
                updrecord.DenominazioneGB = txtDenominazioneGB.Text;
                updrecord.DenominazioneRU = txtDenominazioneRU.Text;
                updrecord.DescrizioneI = txtDescrizioneI.Text;
                updrecord.DescrizioneGB = txtDescrizioneGB.Text;
                updrecord.DatitecniciGB = txtDatitecniciGB.Text;
                updrecord.DescrizioneRU = txtDescrizioneRU.Text;
                updrecord.DatitecniciRU = txtDatitecniciRU.Text;
                updrecord.DatitecniciI = txtDatitecniciI.Text;
                updrecord.Indirizzo = txtIndirizzo.Text;
                updrecord.Email = txtEmail.Text;
                updrecord.Website = txtWebsite.Text;
                updrecord.Telefono = txtTelefono.Text;
                updrecord.Fax = txtFax.Text;
                updrecord.linkVideo = txtVideo.Text;
                updrecord.CodiceComune = ddlComune.SelectedValue;
                updrecord.CodiceProvincia = ddlProvincia.SelectedValue;
                updrecord.CodiceRegione = ddlRegione.SelectedValue;
                updrecord.CodiceProdotto = txtCodiceProd.Text;

                updrecord.CodiceNAZIONE1_dts = ddlCodiceNAZIONE1_dts.SelectedValue;
                updrecord.CodiceREGIONE1_dts = txtCodiceREGIONE1_dts.Value;
                updrecord.CodicePROVINCIA1_dts = txtCodicePROVINCIA1_dts.Value;
                updrecord.CodiceCOMUNE1_dts = txtCodiceCOMUNE1_dts.Value;

                updrecord.CodiceCategoria = ddlProdotto.SelectedValue;
                updrecord.CodiceCategoria2Liv = ddlSottoProdotto.SelectedValue;

                updrecord.Vetrina = chkVetrina.Checked;
                updrecord.Archiviato = chkArchiviato.Checked;
                updrecord.Abilitacontatto = chkContatto.Checked;
                updrecord.Promozione = chkPromozione.Checked;

                double _tmpdbl = 0;
                double.TryParse(txtPrezzo.Text, out _tmpdbl);//Mettere textbox per prezzo
                updrecord.Prezzo = _tmpdbl;
                _tmpdbl = 0;
                double.TryParse(txtPrezzoListino.Text, out _tmpdbl);//Mettere textbox per prezzo listino
                updrecord.PrezzoListino = _tmpdbl;
                _tmpdbl = 0;
                if (double.TryParse(txtQta_vendita.Text, out _tmpdbl))
                { updrecord.Qta_vendita = _tmpdbl; }
                else
                    updrecord.Qta_vendita = null;

                DateTime _tmpdate = System.DateTime.Now;
                if (!DateTime.TryParse(txtData.Text, out _tmpdate))
                    _tmpdate = System.DateTime.Now;
                updrecord.DataInserimento = _tmpdate;


                _tmpdbl = 0;
                double.TryParse(txtLatitudine1_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                updrecord.Latitudine1_dts = _tmpdbl;
                _tmpdbl = 0;
                double.TryParse(txtLongitudine1_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                updrecord.Longitudine1_dts = _tmpdbl;


                //Questi li devi riempire con la lista delle foto
                //updrecord.FotoCollection_M.Schema = txtFotoSchema.Value;
                //updrecord.FotoCollection_M.Valori = txtFotoValori.Value;

                //Potrei controllare in base ad alcuni parametri se l'offerta è già presente o meno ( per ora non lo faccio )

                offDM.InsertOffertaCollegata(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                offDM.InsertOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                //Seleziono l'elemento appena inserito e ci inserisco gli allegati necessari
                OffertaIDSelected = updrecord.Id.ToString();
                hidIdselected.Value = OffertaIDSelected;


                //Inviamo una mailing a tutti i clienti validati ( e con card attiva ) per l'offerta in questione appena inserita
                //if (updrecord.CodiceCategoria == "prod000005")
                //    this.InviaMailistInserimentoStruttura(updrecord); //in updrecord ho l'id dell'offerta appena inserita!!

                this.SvuotaDettaglio();

                btnAggiorna.Text = "Modifica";
                btnAggiorna.ValidationGroup = "";
                btnAnnulla.Visible = false;
                ImpostaDettaglioSolaLettura(true);
                btnCancella.Visible = true;
                btnCancella.Enabled = true;
                btnAggiorna.Enabled = true;

            }

            ////////////////////////////////////////////////////////////////////
            //Creo o aggiorno l'url per il rewriting in tutte le lingue ...
            ////////////////////////////////////////////////////////////////////
            Tabrif urlRewrited = new Tabrif();
            WelcomeLibrary.UF.SitemapManager.EliminaUrlrewritebyIdOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord.Id.ToString());

            WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("I", updrecord.DenominazionebyLingua("I"), updrecord.Id.ToString(), TipologiaOfferte, "", "", "", "", "", true, true);
            WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("GB", updrecord.DenominazionebyLingua("GB"), updrecord.Id.ToString(), TipologiaOfferte, "", "", "", "", "", true, true);
            WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes("RU", updrecord.DenominazionebyLingua("RU"), updrecord.Id.ToString(), TipologiaOfferte, "", "", "", "", "", true, true);


            /////////////////////////////////////


            this.CaricaDati();
            //OfferteCollection list = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
            //rptOfferte.DataSource = list;
            //rptOfferte.DataBind();

        }
        catch (Exception error)
        {
            output.Text += error.Message;
            if (error.InnerException != null)
                output.Text += error.InnerException.Message.ToString();
        }

    }

    /// <summary>
    /// Prepara la richiesta di una mail di avviso ai clienti per avviso inserimento di una nuova struttura nell'A.C.D. Tuscar
    /// </summary>
    /// <param name="item"></param>
    private void InviaMailistInserimentoStruttura(Offerte item)
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
                //Eliminiamo i clieenti con card scadute (Opzionale - commentare se non voluto )
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
                    mail.Tipomailing = (long)enumclass.TipoMailing.AvvisoInserimentoStruttura;
                    mail.NoteInvio = "";

                    mail.SoggettoMail = references.ResMan("Common", Lingua, "oggettoMailInserimentoStruttura").ToString();
                    mail.TestoMail = references.ResMan("Common", Lingua, "testoMailInserimentoStruttura").ToString() + "<br/>";

                    //Mettiamo anche il link alla pagina specifica della struttura appena inserita
                    string link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/SchedaOfferta.aspx?Lingua=" + c.Lingua.ToUpper() + "&idOfferta=" + item.Id + "&CodiceTipologia=" + item.CodiceTipologia; //idOfferta=38&CodiceTipologia=rif000002&Lingua=I
                    mail.TestoMail += "<a href=\"" + link + "\" target=\"_blank\" style=\"font-size:22px;color:#b13c4e\">" + references.ResMan("Common", Lingua, "TestoLinkAStruttura").ToString() + "<br/>";

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
    /// <summary>
    /// Cancella il record selezionato nell'elenco
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancella_Click(object sender, EventArgs e)
    {
        btnAggiorna.Text = "Modifica";
        btnAggiorna.ValidationGroup = "";

        ImpostaDettaglioSolaLettura(true);
        if (cancelHidden.Value == "true")
        {
            cancelHidden.Value = "false";
            try
            {
                Offerte updrecord = new Offerte();
                long tmp = 0;
                if (long.TryParse(OffertaIDSelected, out tmp))
                {
                    updrecord.Id = tmp;
                    //Devi cancellare anche le foto allegate altrimenti restano
                    //Tiro su il record aggiornato
                    //Ricarichiamo l'offerta selezionata dal db
                    Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);
                    //scorro e cancello le foto presenti
                    string pathDestinazione = Server.MapPath(PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected);
                    foreach (Allegato foto in item.FotoCollection_M)
                    {
                        try
                        {
                            bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tmp, foto.NomeFile, "", pathDestinazione);
                        }
                        catch (Exception errodel)
                        {
                            output.Text = errodel.Message;
                        }
                    }

                    //Cancello il record
                    offDM.DeleteOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    WelcomeLibrary.UF.SitemapManager.EliminaUrlrewritebyIdOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord.Id.ToString());

                    this.SvuotaDettaglio();
                    this.CaricaDati();
                    //OfferteCollection list = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
                    //rptOfferte.DataSource = list;
                    //rptOfferte.DataBind();

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
    protected void prefillDettaglio()
    {
        txtIndirizzo.Text = "";
        txtEmail.Text = "";
        txtWebsite.Text = "";
        txtTelefono.Text = "";
        txtFax.Text = "";
        txtVideo.Text = "";
        //  CaricaDatiDdlRicerca("p94", "p94", "Città della Pieve", "", "");

    }
    protected void SvuotaDettaglio()
    {
        btnAggiorna.Text = "Modifica";
        imgFoto.ImageUrl = "";
        NomeFotoSelezionata = "";
        rptImmagini.DataSource = null;
        rptImmagini.DataBind();
        ClientIDSelected = "";
        OffertaIDSelected = "";
        hidIdselected.Value = "";

        txtCampo1GB.Text = "";
        txtCampo1RU.Text = "";
        txtCampo2I.Text = "";
        txtCampo1I.Text = "";
        txtCampo2GB.Text = "";
        txtCampo2RU.Text = "";
        txtIdcollegato.Text = "";
        txtDenominazioneI.Text = "";
        txtDenominazioneGB.Text = "";
        txtDescrizioneGB.Text = "";
        txtDenominazioneRU.Text = "";
        txtDescrizioneRU.Text = "";
        txtDescrizioneI.Text = "";
        txtPrezzo.Text = "";
        txtPrezzoListino.Text = "";
        txtQta_vendita.Text = "";
        chkVetrina.Checked = false;
        chkArchiviato.Checked = false;
        chkContatto.Checked = false;
        chkPromozione.Checked = false;

        txtDescrizione.Text = "";
        txtProgressivo.Text = "";
        txtCodiceProd.Text = "";
        //txtFotoSchema.Value = "";
        //txtFotoValori.Value = "";
        txtDatitecniciGB.Text = "";
        txtDatitecniciRU.Text = "";
        txtDatitecniciI.Text = "";
        txtIndirizzo.Text = "";
        txtEmail.Text = "";
        txtWebsite.Text = "";
        txtTelefono.Text = "";
        txtFax.Text = "";
        txtVideo.Text = "";
        txtData.Text = "";

        txtLatitudine1_dts.Text = string.Empty;
        txtLongitudine1_dts.Text = string.Empty;
        CaricaDatiDdlRicerca("", "", "", CodiceProdotto, "");
        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);
        txtAnno.Text = "";

        CaricaDllLocalizzazione("IT", "", "", "", ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);

    }
    protected void btnAnnulla_Click(object sender, EventArgs e)
    {

        this.SvuotaDettaglio();
        this.CaricaDati();
        btnAnnulla.Visible = false;
        btnCancella.Visible = true;
        btnCancella.Enabled = false;
        this.ImpostaDettaglioSolaLettura(true);

    }
    protected void ImpostaDettaglioSolaLettura(bool valore)
    {
        txtCampo1I.ReadOnly = valore;
        txtCampo2I.ReadOnly = valore;
        txtCampo1GB.ReadOnly = valore;
        txtCampo2GB.ReadOnly = valore;
        txtCampo1RU.ReadOnly = valore;
        txtCampo2RU.ReadOnly = valore;
        txtIdcollegato.ReadOnly = valore;
        txtDenominazioneI.ReadOnly = valore;
        txtDenominazioneGB.ReadOnly = valore;
        txtDescrizioneGB.ReadOnly = valore;
        txtDenominazioneRU.ReadOnly = valore;
        txtDescrizioneRU.ReadOnly = valore;
        txtDescrizioneI.ReadOnly = valore;
        txtPrezzo.ReadOnly = valore;
        txtPrezzoListino.ReadOnly = valore;
        txtQta_vendita.ReadOnly = valore;
        txtCodiceProd.ReadOnly = valore;
        chkVetrina.Enabled = !valore;
        chkArchiviato.Enabled = !valore;
        chkPromozione.Enabled = !valore;
        //txtFotoSchema.Value = "";
        //txtFotoValori.Value = "";
        txtDatitecniciGB.ReadOnly = valore;
        txtDatitecniciRU.ReadOnly = valore;
        txtDatitecniciI.ReadOnly = valore;
        txtIndirizzo.ReadOnly = valore;
        txtEmail.ReadOnly = valore;
        txtWebsite.ReadOnly = valore;
        txtTelefono.ReadOnly = valore;
        txtFax.ReadOnly = valore;
        txtVideo.ReadOnly = valore;
        txtAnno.ReadOnly = valore;
        ddlRegione.Enabled = !valore;
        ddlProvincia.Enabled = !valore;
        ddlComune.Enabled = !valore;

        txtLatitudine1_dts.ReadOnly = valore;
        txtLongitudine1_dts.ReadOnly = valore;

        ddlCaratteristica1.Enabled = !valore;
        ddlCaratteristica2.Enabled = !valore;
        ddlCaratteristica3.Enabled = !valore;
        ddlCaratteristica4.Enabled = !valore;
        ddlCaratteristica5.Enabled = !valore;
        ddlCaratteristica6.Enabled = !valore;


        txtData.ReadOnly = valore;

        ddlProdotto.Enabled = !valore;
        ddlSottoProdotto.Enabled = !valore;

        //hiddenTargetControlForModalPopup.Enabled = !valore;
        //hiddenTargetControlForModalPopup1.Enabled = !valore;

        //btnModificaSottoProd.Enabled = !valore;
        //btnModificaProd.Enabled = !valore;

        btnAggiorna.Enabled = !valore;
        btnCancella.Enabled = !valore;

        OkButton.Enabled = true;
        //OkButton2.Enabled = true;
        NomeNuovoProdIt.Enabled = false;
        NomeNuovoProdEng.Enabled = false;
        NomeNuovoProdRu.Enabled = false;
        NomeNuovoSottIt.Enabled = false;
        NomeNuovoSottRu.Enabled = false;
        chkContatto.Enabled = !valore;


    }


    #region GESTIONE CASELLE LOCALIZZAZIONE VERSIONE 2

    public void CaricaDllLocalizzazione(string valorenaz, string valorereg, string valorepr, string valoreco, DropDownList dnaz, DropDownList dreg, DropDownList dpro, DropDownList dcom, HtmlInputControl txtRE, HtmlInputControl txtPR, HtmlInputControl txtCO)
    {
        //Riempimento a cascata
        RiempiDdlNazione(valorenaz, dnaz);
        RiempiDdlRegione(valorereg, dnaz, dreg, txtRE);
        RiempiDdlProvincia(valorepr, dnaz, dreg, dpro, txtPR);
        RiempiDdlComune(valoreco, dnaz, dreg, dpro, dcom, txtCO);
    }

    private void RiempiDdlNazione(string valore, DropDownList dnaz)
    {
        List<Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == "I"; });
        nazioni.Sort(new GenericComparer<Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));
        dnaz.Items.Clear();
        foreach (Tabrif n in nazioni)
        {
            ListItem i = new ListItem(n.Campo1, n.Codice);
            dnaz.Items.Add(i);
        }
        try
        {
            dnaz.SelectedValue = valore.ToUpper();
        }
        catch { AggiungiNazione(valore, dnaz); }
    }
    private void RiempiDdlRegione(string valore, DropDownList dnaz, DropDownList dreg, HtmlInputControl txtRE)
    {
        bool found = false;
        dreg.Items.Clear();
        dreg.Items.Insert(0, "Seleziona Regione");
        dreg.Items[0].Value = "";
        txtRE.Value = valore;
        if (dnaz.SelectedValue == "IT")
        {
            WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
            List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.SiglaNazione.ToLower() == dnaz.SelectedValue.ToLower()); });
            if (provincelingua != null)
            {
                provincelingua.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                List<string> codiciregione = new List<string>(); //Creo la lista dei codici regione in base al corretto ordinamento!
                foreach (Province item in provincelingua)
                {
                    if (item.Lingua == "I")
                        if (!codiciregione.Contains(item.Codice))
                            codiciregione.Add(item.Codice);
                }
                foreach (string c in codiciregione)
                {
                    Province p = provincelingua.Find(_p => _p.Codice == c);
                    if (!regioni.Exists(delegate (Province tmp) { return (tmp.Regione == p.Regione); }))
                        regioni.Add(p);
                }
            }
            foreach (Province r in regioni)
            {
                ListItem i = new ListItem(r.Regione, r.Codice);
                dreg.Items.Add(i);
            }

            if (dreg.Items.FindByValue(valore) != null)
            {
                dreg.SelectedValue = valore;
                found = true;
            }
            //else
            //{
            //    AggiungiRegione(valore, dreg);
            //}

        }
        if (found && !string.IsNullOrEmpty(valore.Trim()))
        {
            dreg.Visible = true;
            txtRE.Visible = false;
        }
        else
        {
            dreg.Visible = true;
            txtRE.Visible = true;
        }

    }
    private void RiempiDdlProvincia(string valore, DropDownList dnaz, DropDownList dreg, DropDownList dpro, HtmlInputControl txtPR)
    {
        bool found = false;
        dpro.Items.Clear();
        dpro.Items.Insert(0, "Seleziona Provincia");
        dpro.Items[0].Value = "";
        txtPR.Value = valore;
        if (dreg.SelectedValue != "" && dnaz.SelectedValue == "IT")
        {
            Province _tmp = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.Codice == dreg.SelectedValue); });
            if (_tmp != null)
            {
                List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == dnaz.SelectedValue.ToLower()); });
                provincelingua.Sort(new GenericComparer<Province>("Provincia", System.ComponentModel.ListSortDirection.Ascending));
                foreach (Province r in provincelingua)
                {
                    ListItem i = new ListItem(r.Provincia, r.Codice);
                    dpro.Items.Add(i);
                }
            }
            if (dpro.Items.FindByValue(valore) != null)
            {
                dpro.SelectedValue = valore;
                found = true;
            }
            //else
            //{
            //    AggiungiProvincia(valore, dpro);
            //}
        }
        if (found && !string.IsNullOrEmpty(valore.Trim()))
        {
            dpro.Visible = true;
            txtPR.Visible = false;
        }
        else
        {
            dpro.Visible = true;
            txtPR.Visible = true;
        }
    }
    private void RiempiDdlComune(string valore, DropDownList dnaz, DropDownList dreg, DropDownList dpro, DropDownList dcom, HtmlInputControl txtCO)
    {
        bool found = false;
        dcom.Items.Clear();
        dcom.Items.Insert(0, "Seleziona Comune");
        dcom.Items[0].Value = "";
        txtCO.Value = valore;
        if (dpro.SelectedValue != "" && dnaz.SelectedValue == "IT")
        {
            List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate (WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == dpro.SelectedValue); });
            if (comunilingua != null)
            {
                comunilingua.Sort(new GenericComparer<Comune>("Nome", System.ComponentModel.ListSortDirection.Ascending));
                foreach (Comune r in comunilingua)
                {
                    ListItem i = new ListItem(r.Nome, r.Nome);
                    dcom.Items.Add(i);
                }
            }

            if (dcom.Items.FindByValue(valore) != null)
            {
                dcom.SelectedValue = valore;
                found = true;
            }
            //else AggiungiComune(valore, dcom);

        }
        if (found && !string.IsNullOrEmpty(valore.Trim()))
        {
            dcom.Visible = true;
            txtCO.Visible = false;
        }
        else
        {
            dcom.Visible = true;
            txtCO.Visible = true;
        }
    }
    protected string AggiungiNazione(string codice, DropDownList dnaz)
    {
        try
        {
            //Se il valore non esiste lo aggiungo 
            ListItem selli = new ListItem(codice, codice);
            if (dnaz.Items.FindByValue(codice) == null)
            {
                dnaz.Items.Add(selli);
                dnaz.SelectedValue = codice;
            }
            else
                dnaz.SelectedValue = codice;
        }
        catch { }
        return codice;
    }
    protected string AggiungiRegione(string codice, DropDownList dreg)
    {
        try
        {
            //Se il valore non esiste lo aggiungo 
            ListItem selli = new ListItem(codice, codice);
            if (dreg.Items.FindByValue(codice) == null)
            {
                dreg.Items.Add(selli);
                dreg.SelectedValue = codice;
            }
            else
                dreg.SelectedValue = codice;
        }
        catch { }

        return codice;
    }
    protected string AggiungiProvincia(string codice, DropDownList dpro)
    {
        try
        {
            //Se il valore non esiste lo aggiungo 
            ListItem selli = new ListItem(codice, codice);
            if (dpro.Items.FindByValue(codice) == null)
            {
                dpro.Items.Add(selli);
                dpro.SelectedValue = codice;
            }
            else
                dpro.SelectedValue = codice;
        }
        catch { }

        return codice;
    }
    protected string AggiungiComune(string codice, DropDownList dcom)
    {

        try
        {
            //Se il valore non esiste lo aggiungo 
            ListItem selli = new ListItem(codice, codice);
            if (dcom.Items.FindByValue(codice) == null)
            {
                dcom.Items.Add(selli);
                dcom.SelectedValue = codice;
            }
            else
                dcom.SelectedValue = codice;
        }
        catch { }

        return codice;
    }

    protected void ddlCodiceNAZIONE1_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(value, "", "", "", ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);
    }
    protected void ddlCodiceREGIONE1_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE1_dts.SelectedValue, value, "", "", ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);
    }
    protected void ddlCodicePROVINCIA1_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE1_dts.SelectedValue, ddlCodiceREGIONE1_dts.SelectedValue, value, "", ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);
    }
    protected void ddlCodiceCOMUNE1_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE1_dts.SelectedValue, ddlCodiceREGIONE1_dts.SelectedValue, ddlCodicePROVINCIA1_dts.SelectedValue, value, ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);
    }


    #endregion


    #region GESTIONE FILES ALLEGATI

    protected void btnModifica_Click(object sender, EventArgs e)
    {
        string retmsg = filemanage.ModificaFile(OffertaIDSelected, NomeFotoSelezionata, txtDescrizione.Text, txtProgressivo.Text);
        if (string.IsNullOrEmpty(retmsg))
        {
            this.CaricaDati();
            retmsg = "Modifica File correttamente avvenuta.";
        }
        output.Text += retmsg;
    }
    protected void btnCarica_Click(object sender, EventArgs e)
    {
        string retmsg = filemanage.CaricaFile(Server, UploadFoto, txtDescrizione.Text, OffertaIDSelected, TipologiaOfferte, txtProgressivo.Text);
        if (string.IsNullOrEmpty(retmsg))
        {
            this.CaricaDati();
            retmsg = "Caricamento File correttamente avvenuto.";
        }
        output.Text += retmsg;
    }

    /// <summary>
    /// Elimina la foto attualmente visualizzata dal record selezionato
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnElimina_Click(object sender, EventArgs e)
    {
        string retmsg = filemanage.EliminaFile(Server, OffertaIDSelected, TipologiaOfferte, NomeFotoSelezionata);
        if (string.IsNullOrEmpty(retmsg))
        {
            this.CaricaDati();
            retmsg = "Cancellazione File correttamente avvenuta.";
        }
        output.Text += retmsg;
    }


    #endregion

    #region GESTIONE CATEGORIE 1 E SECONDO LIVELLO


    /// <summary>
    /// Funzione dedicata al caricamento delle dll relative all'inserimento ed alla modifica del sottoprodotto
    /// </summary>
    /// <param name="Tipologia"></param>
    /// <param name="Prodotto"></param>
    /// <param name="Sottoprodotto"></param>
    private void CaricaDatiDllProdotto(string Tipologia, string Prodotto)
    {
        string SceltaTipologia = Tipologia;
        List<WelcomeLibrary.DOM.Prodotto> prodotti = new List<WelcomeLibrary.DOM.Prodotto>();
        prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == TipologiaOfferte || TipologiaOfferte == "")); });
        if (!string.IsNullOrEmpty(SceltaTipologia))
            prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == SceltaTipologia || SceltaTipologia == "")); });

        prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        /*Carico Anche la ddl dell'inserzione del nuovo sottoprodotto*/
        ddlProdottoNewProd1.Items.Clear();
        ddlProdottoNewProd1.Items.Insert(0, references.ResMan("Common", Lingua, "selProdotti"));
        ddlProdottoNewProd1.Items[0].Value = "";
        ddlProdottoNewProd1.DataSource = prodotti;
        ddlProdottoNewProd1.DataTextField = "Descrizione";
        ddlProdottoNewProd1.DataValueField = "CodiceProdotto";
        ddlProdottoNewProd1.DataBind();
        try
        {
            ddlProdottoNewProd1.SelectedValue = Prodotto;
        }
        catch { }

        List<WelcomeLibrary.DOM.TipologiaOfferte> Tipologie = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I"); });

        ddlTipologiaNewProd.Items.Clear();
        ddlTipologiaNewProd.Items.Insert(0, references.ResMan("Common", Lingua, "selSProdotti"));
        ddlTipologiaNewProd.Items[0].Value = "";
        ddlTipologiaNewProd.DataSource = Tipologie;
        ddlTipologiaNewProd.DataTextField = "Descrizione";
        ddlTipologiaNewProd.DataValueField = "Codice";
        ddlTipologiaNewProd.DataBind();
        try
        {
            ddlTipologiaNewProd.SelectedValue = SceltaTipologia;
        }
        catch { }
    }

    /// <summary>
    /// Funzione dedicata al caricamento delle dll relative all'inserimento ed alla modifica del sottoprodotto
    /// </summary>
    /// <param name="Tipologia"></param>
    /// <param name="Prodotto"></param>
    /// <param name="Sottoprodotto"></param>
    private void CaricaDatiDllSottoprodotto(string Tipologia, string Prodotto, string Sottoprodotto)
    {
        string SceltaTipologia = Tipologia;

        List<WelcomeLibrary.DOM.Prodotto> prodotti = new List<WelcomeLibrary.DOM.Prodotto>();
        prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == TipologiaOfferte || TipologiaOfferte == "")); });

        prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        /*Carico Anche la ddl dell'inserzione del nuovo sottoprodotto*/
        ddlProdottoNewProd.Items.Clear();
        ddlProdottoNewProd.Items.Insert(0, references.ResMan("Common", Lingua, "selProdotti"));
        ddlProdottoNewProd.Items[0].Value = "";
        ddlProdottoNewProd.DataSource = prodotti;
        ddlProdottoNewProd.DataTextField = "Descrizione";
        ddlProdottoNewProd.DataValueField = "CodiceProdotto";
        ddlProdottoNewProd.DataBind();
        try
        {
            ddlProdottoNewProd.SelectedValue = Prodotto;
        }
        catch { }


        List<WelcomeLibrary.DOM.SProdotto> sprodotti = new List<WelcomeLibrary.DOM.SProdotto>();

        sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceProdotto == ddlProdottoNewProd.SelectedValue)); });

        sprodotti.Sort(new GenericComparer<SProdotto>("CodiceSProdotto", System.ComponentModel.ListSortDirection.Ascending));
        ddlProdottoNewSProd.Items.Clear();
        ddlProdottoNewSProd.Items.Insert(0, references.ResMan("Common", Lingua, "selSProdotti"));
        ddlProdottoNewSProd.Items[0].Value = "";
        ddlProdottoNewSProd.DataSource = sprodotti;
        ddlProdottoNewSProd.DataTextField = "Descrizione";
        ddlProdottoNewSProd.DataValueField = "CodiceSProdotto";
        ddlProdottoNewSProd.DataBind();
        try
        {
            ddlProdottoNewSProd.SelectedValue = Sottoprodotto;
        }
        catch { }



        List<WelcomeLibrary.DOM.TipologiaOfferte> Tipologie = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I"); });

        ddlTipologiaNewSottProd.Items.Clear();
        ddlTipologiaNewSottProd.Items.Insert(0, references.ResMan("Common", Lingua, "selSProdotti"));
        ddlTipologiaNewSottProd.Items[0].Value = "";
        ddlTipologiaNewSottProd.DataSource = Tipologie;
        ddlTipologiaNewSottProd.DataTextField = "Descrizione";
        ddlTipologiaNewSottProd.DataValueField = "Codice";
        ddlTipologiaNewSottProd.DataBind();
        try
        {
            ddlTipologiaNewSottProd.SelectedValue = SceltaTipologia;
        }
        catch { }
    }

    protected void CaricaDatiFormInserimento(string Tipologia, string Prodotto)
    {

        //carico nei form di inserimento e modifica i valori relativi al prodotto
        WelcomeLibrary.DOM.Prodotto prodottoIta = new WelcomeLibrary.DOM.Prodotto();
        WelcomeLibrary.DOM.Prodotto prodottoEng = new WelcomeLibrary.DOM.Prodotto();
        WelcomeLibrary.DOM.Prodotto prodottoRu = new WelcomeLibrary.DOM.Prodotto();

        prodottoIta = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp_p) { return (tmp_p.Lingua == "I" && (tmp_p.CodiceProdotto == Prodotto)); });
        prodottoEng = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp_p) { return (tmp_p.Lingua == "GB" && (tmp_p.CodiceProdotto == Prodotto)); });
        prodottoRu = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp_p) { return (tmp_p.Lingua == "RU" && (tmp_p.CodiceProdotto == Prodotto)); });

        //adesso che ho il mio prodotto, posso caricare i suoi valori per la modifica o l'inserimento
        if (prodottoIta != null)
            NomeNuovoProdIt.Text = prodottoIta.Descrizione;
        else
            NomeNuovoProdIt.Text = "";

        if (prodottoEng != null)
            NomeNuovoProdEng.Text = prodottoEng.Descrizione;
        else
            NomeNuovoProdEng.Text = "";

        if (prodottoEng != null)
            NomeNuovoProdRu.Text = prodottoRu.Descrizione;
        else
            NomeNuovoProdRu.Text = "";

    }

    protected void CaricaDatiFormInserimentoSott(string Tipologia, string SottoProdotto)
    {
        //carico nei form di inserimento e modifica i valori relativi al prodotto
        WelcomeLibrary.DOM.SProdotto SprodottoIta = new WelcomeLibrary.DOM.SProdotto();
        WelcomeLibrary.DOM.SProdotto SprodottoEng = new WelcomeLibrary.DOM.SProdotto();
        WelcomeLibrary.DOM.SProdotto SprodottoRu = new WelcomeLibrary.DOM.SProdotto();
        SprodottoIta = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp_p) { return (tmp_p.Lingua == "I" && (tmp_p.CodiceSProdotto == SottoProdotto)); });
        SprodottoEng = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp_p) { return (tmp_p.Lingua == "GB" && (tmp_p.CodiceSProdotto == SottoProdotto)); });
        SprodottoRu = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp_p) { return (tmp_p.Lingua == "RU" && (tmp_p.CodiceSProdotto == SottoProdotto)); });

        //adesso che ho il mio prodotto, posso caricare i suoi valori per la modifica o l'inseriment
        if (SprodottoIta != null)
            NomeNuovoSottIt.Text = SprodottoIta.Descrizione;
        else
            NomeNuovoSottIt.Text = "";
        if (SprodottoEng != null)
            NomeNuovoSottEng.Text = SprodottoEng.Descrizione;
        else
            NomeNuovoSottEng.Text = "";


        if (SprodottoRu != null)
            NomeNuovoSottRu.Text = SprodottoRu.Descrizione;
        else
            NomeNuovoSottRu.Text = "";
    }
    protected void btnEliminaProd_Click(object sender, EventArgs e)
    {
        Prodotto updrecord = new Prodotto();
        try
        {
            if (string.IsNullOrEmpty(ddlProdottoNewProd1.SelectedValue))
            {
                ErrorMessage.Text = "Selezionare prodotto  per la rimozione!";
                return;
            }
            //Faccio rimozione veerificando che non ci siamo prodotti con quel codice prodotto/sottoprodotto
            updrecord = new Prodotto();
            updrecord.CodiceProdotto = ddlProdottoNewProd1.SelectedValue;

            //Eliminazione prodotto
            offDM.DeleteProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
            this.SvuotaDettaglioProd();

            Utility.CaricaListaStaticaProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
            Utility.CaricaListaStaticaSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
            //  CaricaDatiDdlRicercaRepeater("", "");
            this.CaricaDatiDllProdotto(TipologiaOfferte, "");
            this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");
            OkButton.Text = "Nuovo";
            btnModificaProd.Enabled = false;
            btnModificaProd.Text = "Modifica";
            NomeNuovoProdIt.Text = "";
            NomeNuovoProdEng.Text = "";
            NomeNuovoProdRu.Text = "";

        }
        catch (Exception error)
        {
            ErrorMsgNuovoProdotto.Text = error.Message;
            if (error.InnerException != null)
                ErrorMsgNuovoProdotto.Text += error.InnerException.Message.ToString();
        }
    }

    protected void btnModifiProd_Click(object sender, EventArgs e)
    {
        //valutazioni updrecord = new valutazioni();
        Prodotto updrecord = new Prodotto();
        try
        {
            if (btnModificaProd.Text == "Salva")
            {
                if (string.IsNullOrEmpty(NomeNuovoProdIt.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoProdEng.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoProdRu.Text.Trim()))
                {
                    ErrorMsgNuovoProdotto.Text = "Inserire il nome categoria prodotto in italiano inglese e russo!";
                    return;
                }
                //Faccio l'inserimento in italiano
                updrecord = new Prodotto();
                updrecord.CodiceTipologia = TipologiaOfferte;
                updrecord.Descrizione = NomeNuovoProdIt.Text;
                updrecord.Lingua = "I";
                updrecord.CodiceProdotto = CodiceProdotto;

                offDM.UpdateProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

                //Faccio l'inserimento in inglese
                updrecord = new Prodotto();
                updrecord.CodiceTipologia = TipologiaOfferte;
                updrecord.Descrizione = NomeNuovoProdEng.Text;
                updrecord.Lingua = "GB";
                updrecord.CodiceProdotto = CodiceProdotto;
                offDM.UpdateProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);


                //Faccio l'inserimento in russo
                updrecord = new Prodotto();
                updrecord.CodiceTipologia = TipologiaOfferte;
                updrecord.Descrizione = NomeNuovoProdRu.Text;
                updrecord.Lingua = "RU";
                updrecord.CodiceProdotto = CodiceProdotto;
                offDM.UpdateProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

                this.SvuotaDettaglioProd();
                Utility.CaricaListaStaticaProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                //   CaricaDatiDdlRicercaRepeater("", "");
                this.CaricaDatiDllProdotto(TipologiaOfferte, "");
                this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");

                //OfferteCollection list = offDM.CaricaOffertePerCodice( WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
                //rptOfferte.DataSource = list;
                //rptOfferte.DataBind();
                OkButton.Text = "Nuovo";
                btnModificaProd.Enabled = false;
                btnModificaProd.Text = "Modifica";
                NomeNuovoProdIt.Enabled = false;
                NomeNuovoProdEng.Enabled = false;
                NomeNuovoProdRu.Enabled = false;

                WelcomeLibrary.UF.SitemapManager.RigeneraLinkSezioniUrlrewrited();//rigenera i link per le tipologie/categorie e sottocategorie per url rewriting
                ;
            }
            else
            {
                if (btnModificaProd.Text == "Annulla")
                {
                    this.SvuotaDettaglioProd();
                    Utility.CaricaListaStaticaProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                    this.CaricaDatiDdlRicerca("", "", "", "", "");
                    this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");

                    OkButton.Text = "Nuovo";
                    btnModificaProd.Enabled = false;
                    btnModificaProd.Text = "Modifica";
                    NomeNuovoProdIt.Enabled = false;
                    NomeNuovoProdEng.Enabled = false;
                    NomeNuovoProdRu.Enabled = false;
                }
            }

        }
        catch (Exception error)
        {
            ErrorMsgNuovoProdotto.Text = error.Message;
            if (error.InnerException != null)
                ErrorMsgNuovoProdotto.Text += error.InnerException.Message.ToString();
        }

    }

    protected void btnInsertNewProd_Click(object sender, EventArgs e)
    {
        //valutazioni updrecord = new valutazioni();
        Prodotto updrecord = new Prodotto();

        try
        {
            if (OkButton.Text == "Nuovo")
            {

                OkButton.Text = "Inserisci";
                btnModificaProd.Text = "Annulla";
                btnModificaProd.Enabled = true;
                NomeNuovoProdEng.Text = "";
                NomeNuovoProdRu.Text = "";
                NomeNuovoProdIt.Text = "";
                NomeNuovoProdIt.Enabled = true;
                NomeNuovoProdEng.Enabled = true;
                NomeNuovoProdRu.Enabled = true;
                //btnModificaProd.Enabled = false; ;
            }
            else
            {
                if (OkButton.Text == "Inserisci")
                {

                    if (string.IsNullOrEmpty(NomeNuovoProdIt.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoProdEng.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoProdRu.Text.Trim()))
                    {
                        ErrorMsgNuovoProdotto.Text = "Inserire il nome prodotto in italiano inglese e russo!";
                        return;
                    }
                    //Faccio l'inserimento in italiano
                    updrecord = new Prodotto();
                    updrecord.CodiceTipologia = TipologiaOfferte;
                    updrecord.Descrizione = NomeNuovoProdIt.Text;
                    updrecord.Lingua = "I";
                    //updrecord.CodiceProdotto = CodiceProdotto;
                    offDM.InsertProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

                    //Faccio l'inserimento in inglese 
                    updrecord = new Prodotto();
                    updrecord.CodiceTipologia = TipologiaOfferte;
                    updrecord.Descrizione = NomeNuovoProdEng.Text;
                    updrecord.Lingua = "GB";
                    offDM.InsertProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

                    //Faccio l'inserimento in russo 
                    updrecord = new Prodotto();
                    updrecord.CodiceTipologia = TipologiaOfferte;
                    updrecord.Descrizione = NomeNuovoProdRu.Text;
                    updrecord.Lingua = "RU";
                    offDM.InsertProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);


                    this.SvuotaDettaglioProd();
                    this.SvuotaDettaglioSProd();
                    Utility.CaricaListaStaticaProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                    //  CaricaDatiDdlRicercaRepeater("", "");

                    NomeNuovoProdIt.Enabled = false;
                    NomeNuovoProdEng.Enabled = false;
                    NomeNuovoProdRu.Enabled = false;
                    OkButton.Text = "Nuovo";
                    this.CaricaDatiDllProdotto(TipologiaOfferte, "");
                    this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");
                    //  CaricaDatiDdlRicercaRepeater("", "");

                    //OfferteCollection list = offDM.CaricaOffertePerCodice( WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
                    //rptOfferte.DataSource = list;
                    //rptOfferte.DataBind();

                    WelcomeLibrary.UF.SitemapManager.RigeneraLinkSezioniUrlrewrited();//rigenera i link per le tipologie/categorie e sottocategorie per url rewriting
                    ;
                }
                else
                {
                    if (OkButton.Text == "Annulla")
                    {

                        this.SvuotaDettaglioProd();
                        Utility.CaricaListaStaticaSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                        this.CaricaDatiDllProdotto(TipologiaOfferte, "");
                        //     CaricaDatiDdlRicercaRepeater("", "");

                        OkButton.Text = "Nuovo";
                        btnModificaProd.Enabled = false;
                        btnModificaProd.Text = "Modifica";
                    }
                }
            }

        }
        catch (Exception error)
        {
            ErrorMsgNuovoProdotto.Text = error.Message;
            if (error.InnerException != null)
                ErrorMsgNuovoProdotto.Text += error.InnerException.Message.ToString();
        }

    }

    protected void btnEliminaSottProd_Click(object sender, EventArgs e)
    {

        //valutazioni updrecord = new valutazioni();
        SProdotto updrecord = new SProdotto();
        try
        {
            if (string.IsNullOrEmpty(ddlProdottoNewProd.SelectedValue) || string.IsNullOrEmpty(ddlProdottoNewSProd.SelectedValue))
            {
                ErrorMessage.Text = "Selezionare sottoprodotto  per la rimozione!";
                return;
            }
            //Faccio rimozione veerificando che non ci siamo prodotti con quel codice prodotto/sottoprodotto
            updrecord = new SProdotto();
            updrecord.CodiceProdotto = ddlProdottoNewProd.SelectedValue;
            updrecord.CodiceSProdotto = ddlProdottoNewSProd.SelectedValue;
            offDM.DeleteSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

            this.SvuotaDettaglioSProd();
            Utility.CaricaListaStaticaSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
            this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");
            //  CaricaDatiDdlRicercaRepeater("", "");
            OkButton2.Text = "Nuovo";
            btnModificaSottoProd.Enabled = false;
            btnModificaSottoProd.Text = "Modifica";
            NomeNuovoSottIt.Text = "";
            NomeNuovoSottEng.Text = "";
            NomeNuovoSottRu.Text = "";

        }
        catch (Exception error)
        {
            ErrorMessage.Text = error.Message;
            if (error.InnerException != null)
                ErrorMessage.Text += error.InnerException.Message.ToString();
        }
    }
    protected void btnModificaSottProd_Click(object sender, EventArgs e)
    {
        //valutazioni updrecord = new valutazioni();
        SProdotto updrecord = new SProdotto();
        try
        {
            if (btnModificaSottoProd.Text == "Salva")
            {

                if (string.IsNullOrEmpty(ddlProdottoNewProd.SelectedValue) || string.IsNullOrEmpty(NomeNuovoSottIt.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoSottEng.Text.Trim()))
                {
                    ErrorMessage.Text = "Inserire il nome sottoprodotto in italiano e inglese!";
                    return;
                }
                //Faccio l'inserimento in italiano
                updrecord = new SProdotto();
                updrecord.CodiceProdotto = ddlProdottoNewProd.SelectedValue;
                updrecord.Descrizione = NomeNuovoSottIt.Text;
                updrecord.CodiceSProdotto = ddlProdottoNewSProd.SelectedValue;
                updrecord.Lingua = "I";
                offDM.UpdateSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);


                //Faccio l'inserimento in inglese
                updrecord = new SProdotto();
                updrecord.CodiceProdotto = ddlProdottoNewProd.SelectedValue;
                updrecord.Descrizione = NomeNuovoSottEng.Text;
                updrecord.CodiceSProdotto = ddlProdottoNewSProd.SelectedValue;
                updrecord.Lingua = "GB";
                offDM.UpdateSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);


                //Faccio l'inserimento in russo
                updrecord = new SProdotto();
                updrecord.CodiceProdotto = ddlProdottoNewProd.SelectedValue;
                updrecord.Descrizione = NomeNuovoSottRu.Text;
                updrecord.CodiceSProdotto = ddlProdottoNewSProd.SelectedValue;
                updrecord.Lingua = "RU";
                offDM.UpdateSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);


                this.SvuotaDettaglioSProd();
                Utility.CaricaListaStaticaSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");
                //  CaricaDatiDdlRicercaRepeater("", "");

                //OfferteCollection list = offDM.CaricaOffertePerCodice( WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
                //rptOfferte.DataSource = list;
                //rptOfferte.DataBind();
                //ApriSottoprodotto.Value = "fasle";
                OkButton2.Text = "Nuovo";
                btnModificaSottoProd.Text = "Modifica";
                btnModificaSottoProd.Enabled = false;
                WelcomeLibrary.UF.SitemapManager.RigeneraLinkSezioniUrlrewrited();//rigenera i link per le tipologie/categorie e sottocategorie per url rewriting
                ;
            }
            else
            {
                if (btnModificaSottoProd.Text == "Annulla")
                {
                    this.SvuotaDettaglioSProd();
                    Utility.CaricaListaStaticaProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                    this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");
                    //   CaricaDatiDdlRicercaRepeater("", "");

                    OkButton2.Text = "Nuovo";
                    btnModificaSottoProd.Enabled = false;
                    NomeNuovoSottIt.Enabled = false;
                    NomeNuovoSottEng.Enabled = false;
                    NomeNuovoSottRu.Enabled = false;
                }
            }


        }
        catch (Exception error)
        {
            ErrorMessage.Text = error.Message;
            if (error.InnerException != null)
                ErrorMessage.Text += error.InnerException.Message.ToString();
        }

    }

    protected void btnInsertNewSottProd_Click(object sender, EventArgs e)
    {
        //valutazioni updrecord = new valutazioni();
        SProdotto updrecord = new SProdotto();

        try
        {
            if (OkButton2.Text == "Nuovo")
            {
                if (string.IsNullOrEmpty(ddlProdottoNewProd.SelectedValue))
                {
                    ErrorMessage.Text = "Selezionare categoria prodotto per inserimento sottoprodotto.";
                    return;
                }

                OkButton2.Text = "Inserisci";
                btnModificaSottoProd.Text = "Annulla";
                btnModificaSottoProd.Enabled = true;
                NomeNuovoSottEng.Text = "";
                NomeNuovoSottRu.Text = "";
                NomeNuovoSottIt.Text = "";
                NomeNuovoSottIt.Enabled = true;
                NomeNuovoSottEng.Enabled = true;
                NomeNuovoSottRu.Enabled = true;
                //btnModificaSottoProd.Enabled = false; ;
            }
            else
            {
                if (OkButton2.Text == "Inserisci")
                {

                    //Faccio l'inserimento in italiano
                    updrecord = new SProdotto();
                    updrecord.CodiceProdotto = CodiceProdotto;

                    if (string.IsNullOrEmpty(NomeNuovoSottIt.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoSottEng.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoSottRu.Text.Trim()))
                    {
                        ErrorMessage.Text = "inserire descrizioni sottoprodotto in italiano, inglese e russo";
                        return;
                    }
                    //updrecord.CodiceTipologia = TipologiaOfferte;
                    updrecord.Descrizione = NomeNuovoSottIt.Text;
                    updrecord.Lingua = "I";
                    offDM.InsertSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    //Faccio l'inserimento in inglese
                    updrecord = new SProdotto();
                    updrecord.CodiceProdotto = CodiceProdotto;
                    updrecord.Descrizione = NomeNuovoSottEng.Text;
                    updrecord.Lingua = "GB";
                    offDM.InsertSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    //Russo
                    updrecord = new SProdotto();
                    updrecord.CodiceProdotto = CodiceProdotto;
                    updrecord.Descrizione = NomeNuovoSottRu.Text;
                    updrecord.Lingua = "RU";
                    offDM.InsertSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

                    this.SvuotaDettaglioProd();
                    this.SvuotaDettaglioSProd();
                    Utility.CaricaListaStaticaSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                    this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");
                    //  CaricaDatiDdlRicercaRepeater("", "");

                    OkButton2.Text = "Nuovo";
                    btnModificaSottoProd.Enabled = false;
                    btnModificaSottoProd.Text = "Modifica";
                    NomeNuovoSottIt.Text = "";
                    NomeNuovoSottEng.Text = "";
                    NomeNuovoSottRu.Text = "";
                    WelcomeLibrary.UF.SitemapManager.RigeneraLinkSezioniUrlrewrited();//rigenera i link per le tipologie/categorie e sottocategorie per url rewriting
                    ;
                }
                else
                {
                    if (OkButton2.Text == "Annulla")
                    {

                        this.SvuotaDettaglioProd();
                        Utility.CaricaListaStaticaSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                        this.CaricaDatiDdlRicerca("", "", "", "", "");
                        this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");
                        //     CaricaDatiDdlRicercaRepeater("", "");

                        OkButton2.Text = "Nuovo";
                        btnModificaSottoProd.Enabled = false;
                        btnModificaSottoProd.Text = "Modifica";
                        NomeNuovoSottIt.Text = "";
                        NomeNuovoSottEng.Text = "";
                        NomeNuovoSottRu.Text = "";
                    }
                }
            }

        }
        catch (Exception error)
        {
            ErrorMessage.Text = error.Message;
            if (error.InnerException != null)
                output.Text += error.InnerException.Message.ToString();
            ErrorMessage.Text = "Selezionare un prodotto a cui affidare il SottoProdotto";
        }

    }
    protected void SvuotaDettaglioProd()
    {
        NomeNuovoProdIt.Text = "";
        NomeNuovoProdEng.Text = "";
        NomeNuovoProdRu.Text = "";

    }

    protected void SvuotaDettaglioSProd()
    {
        NomeNuovoSottIt.Text = "";
        NomeNuovoSottEng.Text = "";
        NomeNuovoSottRu.Text = "";
    }
    protected void ddlProdottoNewProd1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Qui devo mettere una funzione che riempie i dati con il nome (ITA / ENG / RU) del prodotto selezionato
        //Di modo da poterlo modificare
        CodiceProdotto = ddlProdottoNewProd1.SelectedValue;

        CaricaDatiFormInserimento(ddlTipologiaNewProd.SelectedValue, ddlProdottoNewProd1.SelectedValue);
        btnModificaProd.Enabled = true;
        NomeNuovoProdIt.Enabled = true;
        NomeNuovoProdEng.Enabled = true;
        NomeNuovoProdRu.Enabled = true;
        //OkButton.Enabled = false;
        OkButton.Text = "Annulla";
        btnModificaProd.Text = "Salva";

        linksezioneI.Text = WelcomeLibrary.UF.SitemapManager.getlinksezione(TipologiaOfferte, ddlProdottoNewProd1.SelectedValue, "I");
        linksezioneGB.Text = WelcomeLibrary.UF.SitemapManager.getlinksezione(TipologiaOfferte, ddlProdottoNewProd1.SelectedValue, "GB");
        linksezioneRU.Text = WelcomeLibrary.UF.SitemapManager.getlinksezione(TipologiaOfferte, ddlProdottoNewProd1.SelectedValue, "RU");
    }

    protected void ddlProdottoNewProd_SelectedIndexChange(object sender, EventArgs e)
    {
        CodiceProdotto = ddlProdottoNewProd.SelectedValue;
        CaricaDatiDllSottoprodotto(ddlTipologiaNewSottProd.SelectedValue, ddlProdottoNewProd.SelectedValue, "");
        CaricaDatiFormInserimentoSott(ddlTipologiaNewProd.SelectedValue, ddlProdottoNewSProd.SelectedValue);
        linksottosezioneI.Text = "";
        linksottosezioneGB.Text = "";
        linksottosezioneRU.Text = "";
    }
    protected void ddlProdottoNewSProd_SelectedIndexChange(object sender, EventArgs e)
    {
        //Qui devo mettere una funzione che riempie i dati con il nome (ITA / ENG) del prodotto selezionato per modo da poterlo modificare
        CodiceProdotto = ddlProdottoNewProd.SelectedValue;
        CaricaDatiFormInserimentoSott(ddlTipologiaNewProd.SelectedValue, ddlProdottoNewSProd.SelectedValue);
        btnModificaSottoProd.Enabled = true;
        NomeNuovoSottIt.Enabled = true;
        NomeNuovoSottEng.Enabled = true;
        NomeNuovoSottRu.Enabled = true;
        OkButton2.Text = "Annulla";
        btnModificaSottoProd.Text = "Salva";

        linksottosezioneI.Text = WelcomeLibrary.UF.SitemapManager.getlinksottosezione(TipologiaOfferte, ddlProdottoNewProd.SelectedValue, ddlProdottoNewSProd.SelectedValue, "I");
        linksottosezioneGB.Text = WelcomeLibrary.UF.SitemapManager.getlinksottosezione(TipologiaOfferte, ddlProdottoNewProd.SelectedValue, ddlProdottoNewSProd.SelectedValue, "GB");
        linksottosezioneRU.Text = WelcomeLibrary.UF.SitemapManager.getlinksottosezione(TipologiaOfferte, ddlProdottoNewProd.SelectedValue, ddlProdottoNewSProd.SelectedValue, "RU");

    }
    protected void TipologiaProd_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Permette il cambio della tipologia di offerte OCCHIO!!!!
        TipologiaOfferte = ddlTipologiaNewProd.SelectedValue;
        CaricaDatiDdlRicerca("", "", "", "", "");
        linksezioneI.Text = "";
        linksezioneGB.Text = "";
        linksezioneRU.Text = "";
    }
    #endregion

    #region GESTIONE CARATTERISTICHE DI RICERCA


    private void CaricaDatiDdlCaratteristiche(long p1, long p2, long p3, long p4, long p5, long p6)
    {

        //Riempio la ddl 
        List<Tabrif> Car1 = Utility.Caratteristiche[0].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica1.Items.Clear();
        ddlCaratteristica1.Items.Insert(0, "Seleziona car1");
        ddlCaratteristica1.Items[0].Value = "0";
        ddlCaratteristica1.DataSource = Car1;
        ddlCaratteristica1.DataTextField = "Campo1";
        ddlCaratteristica1.DataValueField = "Codice";
        ddlCaratteristica1.DataBind();
        try
        {
            ddlCaratteristica1.SelectedValue = p1.ToString();
        }
        catch { }

        //Riempio la ddl tipi clienti
        ddlCaratteristica1_gest.Items.Clear();
        ddlCaratteristica1_gest.Items.Insert(0, "Seleziona car1");
        ddlCaratteristica1_gest.Items[0].Value = "0";
        ddlCaratteristica1_gest.DataSource = Car1;
        ddlCaratteristica1_gest.DataTextField = "Campo1";
        ddlCaratteristica1_gest.DataValueField = "Codice";
        ddlCaratteristica1_gest.DataBind();
        try
        {
            ddlCaratteristica1_gest.SelectedValue = "0";
        }
        catch { }


        //Riempio la ddl  
        List<Tabrif> Car2 = Utility.Caratteristiche[1].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica2.Items.Clear();
        ddlCaratteristica2.Items.Insert(0, "Seleziona car2");
        ddlCaratteristica2.Items[0].Value = "0";
        ddlCaratteristica2.DataSource = Car2;
        ddlCaratteristica2.DataTextField = "Campo1";
        ddlCaratteristica2.DataValueField = "Codice";
        ddlCaratteristica2.DataBind();
        try
        {
            ddlCaratteristica2.SelectedValue = p2.ToString();
        }
        catch { }

        //Riempio la ddl tipi clienti
        Car2 = Utility.Caratteristiche[1].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica2_gest.Items.Clear();
        ddlCaratteristica2_gest.Items.Insert(0, "Seleziona car2");
        ddlCaratteristica2_gest.Items[0].Value = "0";
        ddlCaratteristica2_gest.DataSource = Car2;
        ddlCaratteristica2_gest.DataTextField = "Campo1";
        ddlCaratteristica2_gest.DataValueField = "Codice";
        ddlCaratteristica2_gest.DataBind();
        try
        {
            ddlCaratteristica2_gest.SelectedValue = "0";
        }
        catch { }


        //Riempio la ddl  
        List<Tabrif> Car3 = Utility.Caratteristiche[2].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica3.Items.Clear();
        ddlCaratteristica3.Items.Insert(0, "Seleziona car3");
        ddlCaratteristica3.Items[0].Value = "0";
        ddlCaratteristica3.DataSource = Car3;
        ddlCaratteristica3.DataTextField = "Campo1";
        ddlCaratteristica3.DataValueField = "Codice";
        ddlCaratteristica3.DataBind();
        try
        {
            ddlCaratteristica3.SelectedValue = p3.ToString();
        }
        catch { }

        //Riempio la ddl tipi clienti
        Car3 = Utility.Caratteristiche[2].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica3_gest.Items.Clear();
        ddlCaratteristica3_gest.Items.Insert(0, "Seleziona car3");
        ddlCaratteristica3_gest.Items[0].Value = "0";
        ddlCaratteristica3_gest.DataSource = Car3;
        ddlCaratteristica3_gest.DataTextField = "Campo1";
        ddlCaratteristica3_gest.DataValueField = "Codice";
        ddlCaratteristica3_gest.DataBind();
        try
        {
            ddlCaratteristica3_gest.SelectedValue = "0";
        }
        catch { }


        //Riempio la ddl  
        List<Tabrif> Car4 = Utility.Caratteristiche[3].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica4.Items.Clear();
        ddlCaratteristica4.Items.Insert(0, "Seleziona car4");
        ddlCaratteristica4.Items[0].Value = "0";
        ddlCaratteristica4.DataSource = Car4;
        ddlCaratteristica4.DataTextField = "Campo1";
        ddlCaratteristica4.DataValueField = "Codice";
        ddlCaratteristica4.DataBind();
        try
        {
            ddlCaratteristica4.SelectedValue = p4.ToString();
        }
        catch { }
        //Riempio la ddl tipi clienti
        Car4 = Utility.Caratteristiche[3].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica4_gest.Items.Clear();
        ddlCaratteristica4_gest.Items.Insert(0, "Seleziona car4");
        ddlCaratteristica4_gest.Items[0].Value = "0";
        ddlCaratteristica4_gest.DataSource = Car4;
        ddlCaratteristica4_gest.DataTextField = "Campo1";
        ddlCaratteristica4_gest.DataValueField = "Codice";
        ddlCaratteristica4_gest.DataBind();
        try
        {
            ddlCaratteristica4_gest.SelectedValue = "0";
        }
        catch { }


        //Riempio la ddl  
        List<Tabrif> Car5 = Utility.Caratteristiche[4].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica5.Items.Clear();
        ddlCaratteristica5.Items.Insert(0, "Seleziona car5");
        ddlCaratteristica5.Items[0].Value = "0";
        ddlCaratteristica5.DataSource = Car5;
        ddlCaratteristica5.DataTextField = "Campo1";
        ddlCaratteristica5.DataValueField = "Codice";
        ddlCaratteristica5.DataBind();
        try
        {
            ddlCaratteristica5.SelectedValue = p5.ToString();
        }
        catch { }

        //Riempio la ddl tipi clienti
        Car5 = Utility.Caratteristiche[4].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica5_gest.Items.Clear();
        ddlCaratteristica5_gest.Items.Insert(0, "Seleziona car5");
        ddlCaratteristica5_gest.Items[0].Value = "0";
        ddlCaratteristica5_gest.DataSource = Car5;
        ddlCaratteristica5_gest.DataTextField = "Campo1";
        ddlCaratteristica5_gest.DataValueField = "Codice";
        ddlCaratteristica5_gest.DataBind();
        try
        {
            ddlCaratteristica5_gest.SelectedValue = "0";
        }
        catch { }



        //Riempio la ddl  
        List<Tabrif> Car6 = Utility.Caratteristiche[5].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica6.Items.Clear();
        ddlCaratteristica6.Items.Insert(0, "Seleziona car6");
        ddlCaratteristica6.Items[0].Value = "0";
        ddlCaratteristica6.DataSource = Car6;
        ddlCaratteristica6.DataTextField = "Campo1";
        ddlCaratteristica6.DataValueField = "Codice";
        ddlCaratteristica6.DataBind();
        try
        {
            ddlCaratteristica6.SelectedValue = p6.ToString();
        }
        catch { }

        //Riempio la ddl tipi clienti
        Car6 = Utility.Caratteristiche[5].FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlCaratteristica6_gest.Items.Clear();
        ddlCaratteristica6_gest.Items.Insert(0, "Seleziona car6");
        ddlCaratteristica6_gest.Items[0].Value = "0";
        ddlCaratteristica6_gest.DataSource = Car6;
        ddlCaratteristica6_gest.DataTextField = "Campo1";
        ddlCaratteristica6_gest.DataValueField = "Codice";
        ddlCaratteristica6_gest.DataBind();
        try
        {
            ddlCaratteristica6_gest.SelectedValue = "0";
        }
        catch { }


    }

    protected void caratteristica1update(object sender, EventArgs e)
    {
        if (ddlCaratteristica1_gest.SelectedValue != "")
        {
            Tabrif t = Utility.Caratteristiche[0].Find(c => c.Codice == ddlCaratteristica1_gest.SelectedValue && c.Lingua == "I");
            if (t != null)
                txtCar1I.Text = t.Campo1;
            t = Utility.Caratteristiche[0].Find(c => c.Codice == ddlCaratteristica1_gest.SelectedValue && c.Lingua == "GB");
            if (t != null)
                txtCar1GB.Text = t.Campo1;
            t = Utility.Caratteristiche[0].Find(c => c.Codice == ddlCaratteristica1_gest.SelectedValue && c.Lingua == "RU");
            if (t != null)
                txtCar1RU.Text = t.Campo1;
        }
        else
        {
            txtCar1I.Text = "";
            txtCar1GB.Text = "";
            txtCar1RU.Text = "";

        }
    }
    protected void btnAggiornaCaratteristica1_Click(object sender, EventArgs e)
    {
        offerteDM DM = new offerteDM();

        //Cerichiamo il prossimo progressivo codice libero
        //Utility.TipiClienti
        int ultimoprogressivo = 0;
        int _i = 0;
        Utility.Caratteristiche[0].ForEach(c => ultimoprogressivo = (int.TryParse(c.Codice, out _i)) ? ((_i > ultimoprogressivo) ? _i : ultimoprogressivo) : (ultimoprogressivo));
        ultimoprogressivo += 1;

        Tabrif item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica1_gest.SelectedValue) || ddlCaratteristica1_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[0].Find(c => c.Codice == ddlCaratteristica1_gest.SelectedValue && c.Lingua == "I");
        }
        item.Campo1 = txtCar1I.Text;
        item.Lingua = "I";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica1");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica1_gest.SelectedValue) || ddlCaratteristica1_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[0].Find(c => c.Codice == ddlCaratteristica1_gest.SelectedValue && c.Lingua == "GB");
        }
        item.Campo1 = txtCar1GB.Text;
        item.Lingua = "GB";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica1");


        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica1_gest.SelectedValue) || ddlCaratteristica1_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[0].Find(c => c.Codice == ddlCaratteristica1_gest.SelectedValue && c.Lingua == "RU");
        }
        item.Campo1 = txtCar1RU.Text;
        item.Lingua = "RU";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica1");


        txtCar1I.Text = "";
        txtCar1GB.Text = "";
        txtCar1RU.Text = "";

        //Aggiorno la visualizzazione
        WelcomeLibrary.UF.Utility.Caratteristiche[0] = WelcomeLibrary.UF.Utility.CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica1");
        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);

        CaricaDati();

    }
    protected void caratteristica2update(object sender, EventArgs e)
    {
        if (ddlCaratteristica2_gest.SelectedValue != "")
        {
            Tabrif t = Utility.Caratteristiche[1].Find(c => c.Codice == ddlCaratteristica2_gest.SelectedValue && c.Lingua == "I");
            if (t != null)
                txtCar2I.Text = t.Campo1;
            t = Utility.Caratteristiche[1].Find(c => c.Codice == ddlCaratteristica2_gest.SelectedValue && c.Lingua == "GB");
            if (t != null)
                txtCar2GB.Text = t.Campo1;

            t = Utility.Caratteristiche[1].Find(c => c.Codice == ddlCaratteristica2_gest.SelectedValue && c.Lingua == "RU");
            if (t != null)
                txtCar2RU.Text = t.Campo1;

        }
        else
        {
            txtCar2I.Text = "";
            txtCar2GB.Text = "";
            txtCar2RU.Text = "";
        }
    }
    protected void btnAggiornaCaratteristica2_Click(object sender, EventArgs e)
    {
        offerteDM DM = new offerteDM();

        //Cerichiamo il prossimo progressivo codice libero
        int ultimoprogressivo = 0;
        int _i = 0;
        Utility.Caratteristiche[1].ForEach(c => ultimoprogressivo = (int.TryParse(c.Codice, out _i)) ? ((_i > ultimoprogressivo) ? _i : ultimoprogressivo) : (ultimoprogressivo));
        ultimoprogressivo += 1;

        Tabrif item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica2_gest.SelectedValue) || ddlCaratteristica2_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[1].Find(c => c.Codice == ddlCaratteristica2_gest.SelectedValue && c.Lingua == "I");
        }
        item.Campo1 = txtCar2I.Text;
        item.Lingua = "I";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica2");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica2_gest.SelectedValue) || ddlCaratteristica2_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[1].Find(c => c.Codice == ddlCaratteristica2_gest.SelectedValue && c.Lingua == "GB");
        }
        item.Campo1 = txtCar2GB.Text;
        item.Lingua = "GB";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica2");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica2_gest.SelectedValue) || ddlCaratteristica2_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[1].Find(c => c.Codice == ddlCaratteristica2_gest.SelectedValue && c.Lingua == "RU");
        }
        item.Campo1 = txtCar2RU.Text;
        item.Lingua = "RU";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica2");


        txtCar2I.Text = "";
        txtCar2GB.Text = "";
        txtCar2RU.Text = "";

        //Aggiorno la visualizzazione
        WelcomeLibrary.UF.Utility.Caratteristiche[1] = WelcomeLibrary.UF.Utility.CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica2");
        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);

        CaricaDati();

    }

    //Caratteristica 3
    protected void caratteristica3update(object sender, EventArgs e)
    {
        if (ddlCaratteristica3_gest.SelectedValue != "")
        {
            Tabrif t = Utility.Caratteristiche[2].Find(c => c.Codice == ddlCaratteristica3_gest.SelectedValue && c.Lingua == "I");
            if (t != null)
                txtCar3I.Text = t.Campo1;
            t = Utility.Caratteristiche[2].Find(c => c.Codice == ddlCaratteristica3_gest.SelectedValue && c.Lingua == "GB");
            if (t != null)
                txtCar3GB.Text = t.Campo1;

            t = Utility.Caratteristiche[2].Find(c => c.Codice == ddlCaratteristica3_gest.SelectedValue && c.Lingua == "RU");
            if (t != null)
                txtCar3RU.Text = t.Campo1;
        }
        else
        {
            txtCar3I.Text = "";
            txtCar3GB.Text = "";
            txtCar3RU.Text = "";
        }
    }
    protected void btnAggiornaCaratteristica3_Click(object sender, EventArgs e)
    {
        offerteDM DM = new offerteDM();

        //Cerichiamo il prossimo progressivo codice libero
        int ultimoprogressivo = 0;
        int _i = 0;
        Utility.Caratteristiche[2].ForEach(c => ultimoprogressivo = (int.TryParse(c.Codice, out _i)) ? ((_i > ultimoprogressivo) ? _i : ultimoprogressivo) : (ultimoprogressivo));
        ultimoprogressivo += 1;

        Tabrif item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica3_gest.SelectedValue) || ddlCaratteristica3_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[2].Find(c => c.Codice == ddlCaratteristica3_gest.SelectedValue && c.Lingua == "I");
        }
        item.Campo1 = txtCar3I.Text;
        item.Lingua = "I";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica3");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica3_gest.SelectedValue) || ddlCaratteristica3_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[2].Find(c => c.Codice == ddlCaratteristica3_gest.SelectedValue && c.Lingua == "GB");
        }
        item.Campo1 = txtCar3GB.Text;
        item.Lingua = "GB";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica3");


        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica3_gest.SelectedValue) || ddlCaratteristica3_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[2].Find(c => c.Codice == ddlCaratteristica3_gest.SelectedValue && c.Lingua == "RU");
        }
        item.Campo1 = txtCar3RU.Text;
        item.Lingua = "RU";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica3");


        txtCar3I.Text = "";
        txtCar3GB.Text = "";
        txtCar3RU.Text = "";

        //Aggiorno la visualizzazione
        WelcomeLibrary.UF.Utility.Caratteristiche[2] = WelcomeLibrary.UF.Utility.CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica3");


        WelcomeLibrary.UF.Utility.Caratteristiche[2].Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Tabrif>("Double1", System.ComponentModel.ListSortDirection.Ascending));



        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);
        CaricaDati();

    }

    //Caratteristica 4
    protected void caratteristica4update(object sender, EventArgs e)
    {
        if (ddlCaratteristica4_gest.SelectedValue != "")
        {
            Tabrif t = Utility.Caratteristiche[3].Find(c => c.Codice == ddlCaratteristica4_gest.SelectedValue && c.Lingua == "I");
            if (t != null)
                txtCar4I.Text = t.Campo1;
            t = Utility.Caratteristiche[3].Find(c => c.Codice == ddlCaratteristica4_gest.SelectedValue && c.Lingua == "GB");
            if (t != null)
                txtCar4GB.Text = t.Campo1;
            t = Utility.Caratteristiche[3].Find(c => c.Codice == ddlCaratteristica4_gest.SelectedValue && c.Lingua == "RU");
            if (t != null)
                txtCar4RU.Text = t.Campo1;
        }
        else
        {
            txtCar4I.Text = "";
            txtCar4GB.Text = "";
            txtCar4RU.Text = "";
        }
    }
    protected void btnAggiornaCaratteristica4_Click(object sender, EventArgs e)
    {
        offerteDM DM = new offerteDM();

        //Cerichiamo il prossimo progressivo codice libero
        int ultimoprogressivo = 0;
        int _i = 0;
        Utility.Caratteristiche[3].ForEach(c => ultimoprogressivo = (int.TryParse(c.Codice, out _i)) ? ((_i > ultimoprogressivo) ? _i : ultimoprogressivo) : (ultimoprogressivo));
        ultimoprogressivo += 1;

        Tabrif item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica4_gest.SelectedValue) || ddlCaratteristica4_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[3].Find(c => c.Codice == ddlCaratteristica4_gest.SelectedValue && c.Lingua == "I");
        }
        item.Campo1 = txtCar4I.Text;
        item.Lingua = "I";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica4");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica4_gest.SelectedValue) || ddlCaratteristica4_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[3].Find(c => c.Codice == ddlCaratteristica4_gest.SelectedValue && c.Lingua == "GB");
        }
        item.Campo1 = txtCar4GB.Text;
        item.Lingua = "GB";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica4");


        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica4_gest.SelectedValue) || ddlCaratteristica4_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[3].Find(c => c.Codice == ddlCaratteristica4_gest.SelectedValue && c.Lingua == "RU");
        }
        item.Campo1 = txtCar4RU.Text;
        item.Lingua = "RU";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica4");


        txtCar4I.Text = "";
        txtCar4GB.Text = "";
        txtCar4RU.Text = "";

        //Aggiorno la visualizzazione
        WelcomeLibrary.UF.Utility.Caratteristiche[3] = WelcomeLibrary.UF.Utility.CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica4");
        WelcomeLibrary.UF.Utility.Caratteristiche[3].Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Tabrif>("Double1", System.ComponentModel.ListSortDirection.Ascending));
        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);

        CaricaDati();

    }

    //Caratteristica 5
    protected void caratteristica5update(object sender, EventArgs e)
    {
        if (ddlCaratteristica5_gest.SelectedValue != "")
        {
            Tabrif t = Utility.Caratteristiche[4].Find(c => c.Codice == ddlCaratteristica5_gest.SelectedValue && c.Lingua == "I");
            if (t != null)
                txtCar5I.Text = t.Campo1;
            t = Utility.Caratteristiche[4].Find(c => c.Codice == ddlCaratteristica5_gest.SelectedValue && c.Lingua == "GB");
            if (t != null)
                txtCar5GB.Text = t.Campo1;
            t = Utility.Caratteristiche[4].Find(c => c.Codice == ddlCaratteristica5_gest.SelectedValue && c.Lingua == "RU");
            if (t != null)
                txtCar5RU.Text = t.Campo1;
        }
        else
        {
            txtCar5I.Text = "";
            txtCar5GB.Text = "";
            txtCar5RU.Text = "";
        }
    }
    protected void btnAggiornaCaratteristica5_Click(object sender, EventArgs e)
    {
        offerteDM DM = new offerteDM();

        //Cerichiamo il prossimo progressivo codice libero
        int ultimoprogressivo = 0;
        int _i = 0;
        Utility.Caratteristiche[4].ForEach(c => ultimoprogressivo = (int.TryParse(c.Codice, out _i)) ? ((_i > ultimoprogressivo) ? _i : ultimoprogressivo) : (ultimoprogressivo));
        ultimoprogressivo += 1;

        Tabrif item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica5_gest.SelectedValue) || ddlCaratteristica5_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[4].Find(c => c.Codice == ddlCaratteristica5_gest.SelectedValue && c.Lingua == "I");
        }
        item.Campo1 = txtCar5I.Text;
        item.Lingua = "I";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica5");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica5_gest.SelectedValue) || ddlCaratteristica5_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[4].Find(c => c.Codice == ddlCaratteristica5_gest.SelectedValue && c.Lingua == "GB");
        }
        item.Campo1 = txtCar5GB.Text;
        item.Lingua = "GB";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica5");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica5_gest.SelectedValue) || ddlCaratteristica5_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[4].Find(c => c.Codice == ddlCaratteristica5_gest.SelectedValue && c.Lingua == "RU");
        }
        item.Campo1 = txtCar5RU.Text;
        item.Lingua = "RU";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica5");


        txtCar5I.Text = "";
        txtCar5GB.Text = "";
        txtCar5RU.Text = "";

        //Aggiorno la visualizzazione
        WelcomeLibrary.UF.Utility.Caratteristiche[4] = WelcomeLibrary.UF.Utility.CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica5");
        WelcomeLibrary.UF.Utility.Caratteristiche[4].Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Tabrif>("Double1", System.ComponentModel.ListSortDirection.Ascending));
        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);

        CaricaDati();

    }


    //Caratteristica 6
    protected void caratteristica6update(object sender, EventArgs e)
    {
        if (ddlCaratteristica6_gest.SelectedValue != "")
        {
            Tabrif t = Utility.Caratteristiche[5].Find(c => c.Codice == ddlCaratteristica6_gest.SelectedValue && c.Lingua == "I");
            if (t != null)
                txtCar6I.Text = t.Campo1;
            t = Utility.Caratteristiche[5].Find(c => c.Codice == ddlCaratteristica6_gest.SelectedValue && c.Lingua == "GB");
            if (t != null)
                txtCar6GB.Text = t.Campo1;
            t = Utility.Caratteristiche[5].Find(c => c.Codice == ddlCaratteristica6_gest.SelectedValue && c.Lingua == "RU");
            if (t != null)
                txtCar6RU.Text = t.Campo1;
        }
        else
        {
            txtCar6I.Text = "";
            txtCar6GB.Text = "";
            txtCar6RU.Text = "";
        }
    }
    protected void btnAggiornaCaratteristica6_Click(object sender, EventArgs e)
    {
        offerteDM DM = new offerteDM();

        //Cerichiamo il prossimo progressivo codice libero
        int ultimoprogressivo = 0;
        int _i = 0;
        Utility.Caratteristiche[5].ForEach(c => ultimoprogressivo = (int.TryParse(c.Codice, out _i)) ? ((_i > ultimoprogressivo) ? _i : ultimoprogressivo) : (ultimoprogressivo));
        ultimoprogressivo += 1;

        Tabrif item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica6_gest.SelectedValue) || ddlCaratteristica6_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[5].Find(c => c.Codice == ddlCaratteristica6_gest.SelectedValue && c.Lingua == "I");
        }
        item.Campo1 = txtCar6I.Text;
        item.Lingua = "I";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica6");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica6_gest.SelectedValue) || ddlCaratteristica6_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[5].Find(c => c.Codice == ddlCaratteristica6_gest.SelectedValue && c.Lingua == "GB");
        }
        item.Campo1 = txtCar6GB.Text;
        item.Lingua = "GB";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica6");

        item = new Tabrif();
        if (string.IsNullOrWhiteSpace(ddlCaratteristica6_gest.SelectedValue) || ddlCaratteristica6_gest.SelectedValue == "0")
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.Caratteristiche[5].Find(c => c.Codice == ddlCaratteristica6_gest.SelectedValue && c.Lingua == "RU");
        }
        item.Campo1 = txtCar6RU.Text;
        item.Lingua = "RU";
        DM.InserisciAggiornaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item, "dbo_TBLRIF_Caratteristica6");


        txtCar6I.Text = "";
        txtCar6GB.Text = "";
        txtCar6RU.Text = "";

        //Aggiorno la visualizzazione
        WelcomeLibrary.UF.Utility.Caratteristiche[5] = WelcomeLibrary.UF.Utility.CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica6");
        WelcomeLibrary.UF.Utility.Caratteristiche[5].Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Tabrif>("Double1", System.ComponentModel.ListSortDirection.Ascending));
        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);

        CaricaDati();

    }


    #endregion
}