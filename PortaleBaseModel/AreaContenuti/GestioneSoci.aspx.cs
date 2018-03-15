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
using System.Text;
using System.Web.Profile;

public partial class AreaContenuti_GestioneSoci : CommonPage
{
    bool debug = false;
   // offerteDM offDM = new offerteDM();

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
    public string stringafiltropagamenti
    {
        get { return ViewState["stringafiltropagamenti"] != null ? (string)(ViewState["stringafiltropagamenti"]) : ""; }
        set { ViewState["stringafiltropagamenti"] = value; }
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

    public Dictionary<string, string> trattamenti
    {
        get { return ViewState["trattamenti"] != null ? (Dictionary<string, string>)(ViewState["trattamenti"]) : new Dictionary<string, string>(); }
        set { ViewState["trattamenti"] = value; }
    }
   public string Lingua
   {
      get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
      set { ViewState["Lingua"] = value; }
   }


   protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
            PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
            PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;

            if (Request.QueryString["CodiceTipologia"] != null && Request.QueryString["CodiceTipologia"] != "")
            { TipologiaOfferte = Request.QueryString["CodiceTipologia"].ToString(); }
            if (TipologiaOfferte == null || TipologiaOfferte == "")
                Response.Redirect("default.aspx?Errore=Selezionare Tipologia");
            //Carichiamo i dati relativi al contenuto specificato
            //Da fare repeater paginato con i risultati della query sul db
            litTitolo.Text = (Utility.TipologieOfferte.Find(delegate(TipologiaOfferte tmp) { return tmp.Lingua == "I" && tmp.Codice == TipologiaOfferte; })).Descrizione;

            CaricaDllLocalizzazione("IT", "", "", "", ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);

            CaricaDllLocalizzazione("IT", "", "", "", ddlCodiceNAZIONE2_dts, ddlCodiceREGIONE2_dts, ddlCodicePROVINCIA2_dts, ddlCodiceCOMUNE2_dts, txtCodiceREGIONE2_dts, txtCodicePROVINCIA2_dts, txtCodiceCOMUNE2_dts);
            CaricaDllLocalizzazione("IT", "", "", "", ddlCodiceNAZIONE3_dts, ddlCodiceREGIONE3_dts, ddlCodicePROVINCIA3_dts, ddlCodiceCOMUNE3_dts, txtCodiceREGIONE3_dts, txtCodicePROVINCIA3_dts, txtCodiceCOMUNE3_dts);
            this.CaricaDatiDdlRicerca("", "", "", "", "");
            CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);
            this.CaricaDatiDllProdotto(TipologiaOfferte, "");
            this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");

            CaricaListaTrattamenti();//Memorizzo la lista dei trattamenti

            //Caselle filtro ricerca
            CaricaDllLocalizzazione("IT", "", "", "", ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
            ddlFiltroTrattamenti.DataTextField = "Value";
            ddlFiltroTrattamenti.DataValueField = "Key";
            ddlFiltroTrattamenti.DataSource = trattamenti;
            ddlFiltroTrattamenti.DataBind();

            //Caselle esperienze interventi


            this.CaricaDati();
            ImpostaDettaglioSolaLettura(true);

            /////////////////////////////////////////////////////////////////////
            //Verifichiamo accesso socio e impostiamo la visualizzazione corretta
            //Spegnendo le cose che non devono essere visibili ai soci!!!
            /////////////////////////////////////////////////////////////////////
            if (ControllaRuolo(User.Identity.Name, "Socio"))
                ImpostaVisualizzazioneSocio();

            DataBind();
        }
        else
        {
            output.Text = "";
            ErrorMessage.Text = "";
            ErrorMsgNuovoProdotto.Text = "";

        }
        //CodiceProdottoRicerca = ddlProdottoRicerca.SelectedValue;
        //CodiceSottoProdottoRicerca = ddlSProdottoRicerca.SelectedValue;
    }

    private void ImpostaVisualizzazioneSocio()
    {
        string idsocio = getidsocio(User.Identity.Name);
        if (!string.IsNullOrWhiteSpace(idsocio))
        {
            //imposto la visualizzazione per il socio
            OffertaIDSelected = idsocio;
            AggiornaDettaglio(idsocio);
        }
        btnCancella.Enabled = true;
        btnAggiorna.Enabled = true;
        btnAggiorna.ValidationGroup = "";

        //Blocco i campi non modificabili
        //txtSociopresentatore1_dts.Enabled = false;
        //txtSociopresentatore2_dts.Enabled = false;
        //txtAnnolaurea_dts.Enabled = false;
        //txtAnnospecializzazione_dts.Enabled = false;
        ddlCaratteristica3.Enabled = false;
        //divCurriculum.Visible = false;
        //divCEQUIP.Visible = false;

        //Spengo le parti che devono essere bloccate
        pnlRicerca.Visible = false;
        pnlGestioneCategorie.Visible = false;
        pnlGestioneTblCaratteristiche.Visible = false;

        pnlAmministrazioneSocioRiservata.Visible = false;
        pnlSoloamministratori1.Visible = false;
        pnlSoloamministratori2.Visible = false;
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


    private void CaricaDati(string statoblocco = "")
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

            if (rdBloccati.SelectedValue.Trim() != "")
            {
                if (string.IsNullOrEmpty(statoblocco))
                    statoblocco = rdBloccati.SelectedValue;
                bool _statoblocco = false;
                bool.TryParse(statoblocco, out _statoblocco);
                SQLiteParameter pstatoblocco = new SQLiteParameter("@Bloccoaccesso_dts", _statoblocco);
                parColl.Add(pstatoblocco);
            }
            if (stringafiltropagamenti.Trim() != "")
            {
                stringafiltropagamenti = stringafiltropagamenti.Replace(" ", "%");
                SQLiteParameter pstringafiltropagamenti = new SQLiteParameter("@stringafiltropagamenti", "%" + stringafiltropagamenti + "%");
                parColl.Add(pstringafiltropagamenti);
            }

            if (ddlFiltroTrattamenti.SelectedValue.Trim() != "")
            {
                SQLiteParameter pstringafiltrotrattamenti = new SQLiteParameter("@stringafiltrotrattamenti", "%" + ddlFiltroTrattamenti.SelectedValue + "%");
                parColl.Add(pstringafiltrotrattamenti);
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


            if (ddlNazioneRicerca.SelectedValue.Trim() != "")
            {
                string valorenaz = ddlNazioneRicerca.SelectedValue.Trim().Replace(" ", "%");
                SQLiteParameter pstringafiltronaz = new SQLiteParameter("@CodiceNAZIONE", "%" + valorenaz + "%");
                parColl.Add(pstringafiltronaz);
            }
            if (ddlRegioneRicerca.SelectedValue.Trim() != "")
            {
                string valorereg = ddlRegioneRicerca.SelectedValue.Trim().Replace(" ", "%");
                SQLiteParameter pstringafiltroreg = new SQLiteParameter("@CodiceREGIONE", "%" + valorereg + "%");
                parColl.Add(pstringafiltroreg);
            }
            if (ddlProvinciaRicerca.SelectedValue.Trim() != "")
            {
                string valorepro = ddlProvinciaRicerca.SelectedValue.Trim().Replace(" ", "%");
                SQLiteParameter pstringafiltropro = new SQLiteParameter("@CodicePROVINCIA", "%" + valorepro + "%");
                parColl.Add(pstringafiltropro);
            }
            if (ddlComuneRicerca.SelectedValue.Trim() != "")
            {
                string valorecom = ddlComuneRicerca.SelectedValue.Trim().Replace(" ", "%");
                SQLiteParameter pstringafiltrocom = new SQLiteParameter("@CodiceCOMUNE", "%" + valorecom + "%");
                parColl.Add(pstringafiltrocom);
            }

            //if (ddlProdottoRicerca.SelectedValue != "")
            //{
            //    SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", CodiceProdottoRicerca);
            //    parColl.Add(p7);
            //}
            //if (ddlSProdottoRicerca.SelectedValue != "")
            //{
            //    SQLiteParameter p8 = new SQLiteParameter("@CodiceCategoria2Liv", CodiceSottoProdottoRicerca);
            //    parColl.Add(p8);
            //}
            //   if (!string.IsNullOrEmpty(CodiceProdottoRicerca) || !string.IsNullOrEmpty(CodiceSottoProdottoRicerca))

            // Cognome_dts
            string campoordinamento = radOrdinamento.SelectedItem.Value;

            offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", null, null, campoordinamento, true);

#endif
            #endregion

            if (offerte == null)
            {
                rptOfferte.DataSource = offerte;
                rptOfferte.DataBind();
                output.Text = "Nessuna struttura trovata per le selezioni fatte";
                return;
            }
            // offerte.Sort(new GenericComparer<Offerte>("DataInserimento", System.ComponentModel.ListSortDirection.Descending));
        }
        catch (Exception error)
        {
            output.Text = error.Message.ToString();
            return;
        }

#if true
        //Selezionamo i risultati in base al numero di pagina e alla sua dimensione per la paginazione
        //Utilizzando la classe di paginazione
        WelcomeLibrary.UF.Pager<Offerte> _pager = new WelcomeLibrary.UF.Pager<Offerte>(offerte);
        int nrecordfiltrati = _pager.Count;
        //if (nrecordfiltrati != 0)
        PagerRisultati.TotalRecords = nrecordfiltrati;
        if (nrecordfiltrati == 0) PagerRisultati.CurrentPage = 1;
#endif
        rptOfferte.DataSource = _pager.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
        rptOfferte.DataBind();

        //Aggiorno la vista del dettaglio
        this.AggiornaDettaglio(OffertaIDSelected);
    }

    public string CategoriaSocio(string codicecategoria)
    {
        string ret = codicecategoria;
        Tabrif cat3 = Utility.Caratteristiche[2].Find(delegate(Tabrif _t) { return _t.Lingua == "I" && _t.Codice == codicecategoria; });
        if (cat3 != null)
            ret = cat3.Campo1;
        return ret;

    }

#if false

    private string CreastringTblinterventi()
    {
        string ret = "";
        string testo = litIntervento1.Text;
        ret += testo + ",";
        string valuefind = "";
        foreach (ListItem o in radIntervento1op1.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op1.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        //////////
        testo = litIntervento2.Text;
        ret += testo + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento1op2.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op2.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        ////////////
        testo = litIntervento3.Text;
        ret += testo + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento1op3.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op3.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        /////////////
        testo = litIntervento4.Text;
        ret += testo + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento1op4.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op4.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        ////////////
        testo = litIntervento5.Text;
        ret += testo + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento1op5.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op5.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        /////////
        testo = litIntervento6.Text;
        ret += testo + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento1op6.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op6.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        ////////////
        testo = litIntervento7.Text;
        ret += testo + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento1op7.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op7.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        /////////
        testo = litIntervento8.Text;
        ret += testo + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento1op8.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op8.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        ///////////
        testo = litIntervento9.Text;
        ret += testo + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento1op9.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind + ",";
        valuefind = "";
        foreach (ListItem o in radIntervento2op9.Items)
        {
            if (o.Selected == true)
            {
                valuefind = o.Value.ToString();
            }
        }
        ret += valuefind; //All'ultmi non metto la virgola finale

        return ret;
    }
    private void VisualizzaTblinterventi(string lista)
    {

        List<string> statuslist = new List<string>(lista.Split(','));
        //Abbiamo Testointervento,valoreselezionato operatore1, valoreselezionato operatore2 .....
        string intervento = litIntervento1.Text;
        int indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op1.SelectedValue = statuslist[indice + 1];
                else
                    radIntervento1op1.ClearSelection();
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op1.SelectedValue = statuslist[indice + 2];
                else
                    radIntervento2op1.ClearSelection();
            }
        }
        else
        {
            radIntervento1op1.ClearSelection();
            radIntervento2op1.ClearSelection();
        }

        intervento = litIntervento2.Text;
        indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op2.SelectedValue = statuslist[indice + 1];
                else
                    radIntervento1op2.ClearSelection();
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op2.SelectedValue = statuslist[indice + 2];
                else
                    radIntervento2op2.ClearSelection();
            }
        }
        else
        {
            radIntervento1op2.ClearSelection();
            radIntervento2op2.ClearSelection();
        }


        intervento = litIntervento3.Text;
        indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op3.SelectedValue = statuslist[indice + 1];
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op3.SelectedValue = statuslist[indice + 2];
            }
        }
        else
        {
            radIntervento1op3.ClearSelection();
            radIntervento2op3.ClearSelection();
        }

        intervento = litIntervento4.Text;
        indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op4.SelectedValue = statuslist[indice + 1];
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op4.SelectedValue = statuslist[indice + 2];
            }
        }
        else
        {
            radIntervento1op4.ClearSelection();
            radIntervento2op4.ClearSelection();
        }


        intervento = litIntervento5.Text;
        indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op5.SelectedValue = statuslist[indice + 1];
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op5.SelectedValue = statuslist[indice + 2];
            }
        }
        else
        {
            radIntervento1op5.ClearSelection();
            radIntervento2op5.ClearSelection();
        }

        intervento = litIntervento6.Text;
        indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op6.SelectedValue = statuslist[indice + 1];
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op6.SelectedValue = statuslist[indice + 2];
            }
        }
        else
        {
            radIntervento1op6.ClearSelection();
            radIntervento2op6.ClearSelection();
        }

        intervento = litIntervento7.Text;
        indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op7.SelectedValue = statuslist[indice + 1];
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op7.SelectedValue = statuslist[indice + 2];
            }
        }
        else
        {
            radIntervento1op7.ClearSelection();
            radIntervento2op7.ClearSelection();
        }

        intervento = litIntervento8.Text;
        indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op8.SelectedValue = statuslist[indice + 1];
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op8.SelectedValue = statuslist[indice + 2];
            }
        }
        else
        {
            radIntervento1op8.ClearSelection();
            radIntervento2op8.ClearSelection();
        }
        intervento = litIntervento9.Text;
        indice = statuslist.FindIndex(a => a.ToString() == intervento);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 3)
            {
                if (!string.IsNullOrEmpty(statuslist[indice + 1]))
                    radIntervento1op9.SelectedValue = statuslist[indice + 1];
                if (!string.IsNullOrEmpty(statuslist[indice + 2]))
                    radIntervento2op9.SelectedValue = statuslist[indice + 2];
            }
        }
        else
        {
            radIntervento1op9.ClearSelection();
            radIntervento2op9.ClearSelection();
        }
    }
    
    private void VisualizzaBoolfields(string lista)
    {
        List<string> statuslist = new List<string>(lista.Split(','));

        bool stato = false;
        string controlidtoset = "radCarriera1";
        radCarriera1.Items.Clear();
        int indice = statuslist.FindIndex(a => a.ToString() == controlidtoset);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 2)
                bool.TryParse(statuslist[indice + 1], out stato);
            ListItem l = new ListItem("Si", "True");
            l.Selected = stato;
            radCarriera1.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = !stato;
            radCarriera1.Items.Add(l);
        }
        else
        {
            ListItem l = new ListItem("Si", "True");
            l.Selected = false;
            radCarriera1.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = false;
            radCarriera1.Items.Add(l);
        }

        stato = false;
        controlidtoset = "radCarriera2";
        radCarriera2.Items.Clear();
        indice = statuslist.FindIndex(a => a.ToString() == controlidtoset);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 2)
                bool.TryParse(statuslist[indice + 1], out stato);
            ListItem l = new ListItem("Si", "True");
            l.Selected = stato;
            radCarriera2.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = !stato;
            radCarriera2.Items.Add(l);
        }
        else
        {
            ListItem l = new ListItem("Si", "True");
            l.Selected = false;
            radCarriera2.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = false;
            radCarriera2.Items.Add(l);
        }

        stato = false;
        controlidtoset = "radCarriera3";
        radCarriera3.Items.Clear();
        indice = statuslist.FindIndex(a => a.ToString() == controlidtoset);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 2)
                bool.TryParse(statuslist[indice + 1], out stato);
            ListItem l = new ListItem("Si", "True");
            l.Selected = stato;
            radCarriera3.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = !stato;
            radCarriera3.Items.Add(l);
        }
        else
        {
            ListItem l = new ListItem("Si", "True");
            l.Selected = false;
            radCarriera3.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = false;
            radCarriera3.Items.Add(l);
        }

        stato = false;
        controlidtoset = "radCarriera4";
        radCarriera4.Items.Clear();
        indice = statuslist.FindIndex(a => a.ToString() == controlidtoset);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 2)
                bool.TryParse(statuslist[indice + 1], out stato);
            ListItem l = new ListItem("Si", "True");
            l.Selected = stato;
            radCarriera4.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = !stato;
            radCarriera4.Items.Add(l);
        }
        else
        {
            ListItem l = new ListItem("Si", "True");
            l.Selected = false;
            radCarriera4.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = false;
            radCarriera4.Items.Add(l);
        }

        stato = false;
        controlidtoset = "radCarriera5";
        radCarriera5.Items.Clear();
        indice = statuslist.FindIndex(a => a.ToString() == controlidtoset);
        if (indice != -1)
        {
            if (statuslist.Count >= indice + 2)
                bool.TryParse(statuslist[indice + 1], out stato);
            ListItem l = new ListItem("Si", "True");
            l.Selected = stato;
            radCarriera5.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = !stato;
            radCarriera5.Items.Add(l);
        }
        else
        {
            ListItem l = new ListItem("Si", "True");
            l.Selected = false;
            radCarriera5.Items.Add(l);
            l = new ListItem("No", "False");
            l.Selected = false;
            radCarriera5.Items.Add(l);
        }
    }
    private string CreastringaBoolfields()
    {
        string ret = "";

        foreach (ListItem o in radCarriera1.Items)
        {
            if (o.Selected == true)
            {
                ret += "radCarriera1,";
                ret += o.Value.ToString() + ",";
            }
        }
        foreach (ListItem o in radCarriera2.Items)
        {
            if (o.Selected == true)
            {
                ret += "radCarriera2,";
                ret += o.Value.ToString() + ",";
            }
        }
        foreach (ListItem o in radCarriera3.Items)
        {
            if (o.Selected == true)
            {
                ret += "radCarriera3,";
                ret += o.Value.ToString() + ",";
            }
        }
        foreach (ListItem o in radCarriera4.Items)
        {
            if (o.Selected == true)
            {
                ret += "radCarriera4,";
                ret += o.Value.ToString() + ",";
            }
        }
        foreach (ListItem o in radCarriera5.Items)
        {
            if (o.Selected == true)
            {
                ret += "radCarriera5,";
                ret += o.Value.ToString() + ",";
            }
        }
        ret = ret.TrimEnd(',');


        return ret;
    }

#endif

    private void CaricaListaTrattamenti(string codicetrattamenti = "rif000101")
    {
        offerteDM offDM = new offerteDM();
        List<Offerte> lista = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetrattamenti, "", false, "", false, "DenominazioneI");
        trattamenti = new Dictionary<string, string>();
        lista.ForEach(o => trattamenti.Add(o.Id.ToString(), o.DenominazioneI));
    }


    private void VisualizzaTrattamenti(string lista)
    {
        List<string> idlist = new List<string>(lista.Split(','));
        cbtrattamentilist.Items.Clear();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (KeyValuePair<string, string> kv in trattamenti)
        {
            //<input type="checkbox" id="" value="" />
            if (idlist.Contains(kv.Key))
            {
                //   sb.Append("<input type=\"checkbox\" id=\"id_" + kv.Key + "\"    value=\"true\" >" + kv.Value +"</input>");
                ListItem l = new ListItem(kv.Value, kv.Key);
                l.Selected = true;
                cbtrattamentilist.Items.Add(l);
            }
            else
            {
                //  sb.Append("<input type=\"checkbox\" id=\"id_" + kv.Key + "\"  value=\"false\" >" + kv.Value + "</input>");
                ListItem l = new ListItem(kv.Value, kv.Key);
                l.Selected = false;
                cbtrattamentilist.Items.Add(l);
            }
        }


    }
    protected string Creastringatrattamenti()
    {
        string ret = "";
        foreach (ListItem o in cbtrattamentilist.Items)
        {
            if (o.Selected == true)
                ret += o.Value.ToString() + ",";
        }
        ret = ret.TrimEnd(',');
        return ret;
    }

    private void VisualizzaPagamenti(string lista, int annoinizio = 2011, int annotermine = 2011)
    {
        List<string> annilist = new List<string>(lista.Split(','));
        cbpagamenti.Items.Clear();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        ////////////PRENDO SEMPRE L'INTERVALLO MAGGIORE DI ANNI TRA
        //QUELLO PASSATO E QUELLO PRESENTE NEL DATABASE PER LA VISUALIZZAZIONE
        int annoinizioindb = 999999999;
        int annotermineindb = 0;
        foreach (string a in annilist)
        {
            int _t = 0;
            if (int.TryParse(a, out _t))
            {
                if (_t < annoinizioindb) annoinizioindb = _t;
                if (_t > annotermineindb) annotermineindb = _t;
            }
        }
        if (annoinizio > annoinizioindb) annoinizio = annoinizioindb;
        if (annotermine < annotermineindb) annotermine = annotermineindb;


        for (int i = annoinizio; i <= annotermine; i++)
        {
            bool stato = false;

            int indice = annilist.FindIndex(a => a.ToString() == i.ToString());
            if (indice != -1)
            {
                if (annilist.Count >= indice + 2)
                    bool.TryParse(annilist[indice + 1], out stato);
            }
            ListItem l = new ListItem(i.ToString());
            l.Selected = stato;
            cbpagamenti.Items.Add(l);
        }

    }
    protected string Creastringapagamenti()
    {
        string ret = "";
        foreach (ListItem o in cbpagamenti.Items)
        {
            if (o.Selected == true)
                ret += o.Value.ToString() + "," + "true,";
            else
                ret += o.Value.ToString() + "," + "false,";
        }
        ret = ret.TrimEnd(',');
        return ret;
    }

    private void AggiornaDettaglio(string OffertaIDSelected)
    {
        //Riempiamo i dati del dettaglio
        Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);
        if (Details != null)
        {
            // ----------SEZIONE DETTAGLIO--------------------
            txtNome_dts.Text = Details.Nome_dts;
            txtCognome_dts.Text = Details.Cognome_dts;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("it");
            txtDatanascita_dts.Text = string.Format(ci, "{0:dd/MM/yyyy}", new object[] { Details.Datanascita_dts });
            txtPivacf_dts.Text = Details.Pivacf_dts;
            //txtSociopresentatore1_dts.Text = Details.Sociopresentatore1_dts;
            //txtSociopresentatore2_dts.Text = Details.Sociopresentatore2_dts;
            txtTelefonoprivato_dts.Text = Details.Telefonoprivato_dts;
            //txtAnnolaurea_dts.Text = Details.Annolaurea_dts;
            //txtAnnospecializzazione_dts.Text = Details.Annospecializzazione_dts;
            //txtAltrespecializzazioni_dts.Text = Details.Altrespecializzazioni_dts;
            //txtSocioaltraassociazione_dts.Text = Details.Socioaltraassociazione_dts;
            txtEmailriservata_dts.Text = Details.Emailriservata_dts;

            //Boolean
            //radSocioIsaps_dts.SelectedValue = Details.SocioIsaps_dts.ToString();
            //radSocioSicpre_dts.SelectedValue = Details.SocioSicpre_dts.ToString();
            //chkSocioSicpre_dts.Checked = Details.SocioSicpre_dts;
            //chkSocioIsaps_dts.Checked = Details.SocioIsaps_dts;


            //chkAccettazioneStatuto_dts.Checked = Details.AccettazioneStatuto_dts;
            //chkCertificazione_dts.Checked = Details.Certificazione_dts;
            chkBloccoaccesso_dts.Checked = Details.Bloccoaccesso_dts;


            //Dati aggiunti
            //txtLocordine_dts.Text = Details.locordine_dts;
            //txtNiscrordine_dts.Text = Details.niscrordine_dts;
            //txtannofrequenza_dts.Text = Details.annofrequenza_dts;
            //txtnomeuniversita_dts.Text = Details.nomeuniversita_dts;
            //txtdettagliuniversita_dts.Text = Details.dettagliuniversita_dts;


            //txtTextfield1_dts.Text = Details.Textfield1_dts;

            //VisualizzaBoolfields(Details.Boolfields_dts);
            //VisualizzaTblinterventi(Details.Interventieseguiti_dts);

            VisualizzaTrattamenti(Details.Trattamenticollegati_dts);
            VisualizzaPagamenti(Details.Pagamenti_dts, 2011, System.DateTime.Now.Year);


            //Indirizzo1
            txtNomeposizione1_dts.Text = Details.Nomeposizione1_dts;
            txtVia1_dts.Value = Details.Via1_dts;
            txtCap1_dts.Value = Details.Cap1_dts;
            txtTelefono1_dts.Value = Details.Telefono1_dts;
            txtLatitudine1_dts.Text = String.Format(ci, "{0:##.#####################}", new object[] { Details.Latitudine1_dts });
            txtLongitudine1_dts.Text = String.Format(ci, "{0:##.#####################}", new object[] { Details.Longitudine1_dts });

            // Details.CodiceNAZIONE1_dts ->  ddlCodiceNAZIONE1_dts  e txtCodiceNAZIONE1_dts ;
            // Details.CodiceREGIONE1_dts ->  ddlCodiceREGIONE1_dts e txtCodiceREGIONE1_dts;
            // Details.CodicePROVINCIA1_dts ->  ddlCodicePROVINCIA1_dts e txtCodicePROVINCIA1_dts ;
            // Details.CodiceCOMUNE1_dts ->  ddlCodiceCOMUNE1_dts e CodiceCOMUNE1_dts ;
            CaricaDllLocalizzazione(Details.CodiceNAZIONE1_dts, Details.CodiceREGIONE1_dts, Details.CodicePROVINCIA1_dts, Details.CodiceCOMUNE1_dts, ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);


            //Indirizzo2
            txtNomeposizione2_dts.Text = Details.Nomeposizione2_dts;
            txtVia2_dts.Value = Details.Via2_dts;
            txtCap2_dts.Value = Details.Cap2_dts;
            txtTelefono2_dts.Value = Details.Telefono2_dts;
            txtLatitudine2_dts.Text = String.Format(ci, "{0:##.#####################}", new object[] { Details.Latitudine2_dts });
            txtLongitudine2_dts.Text = String.Format(ci, "{0:##.#####################}", new object[] { Details.Longitudine2_dts });

            // Details.CodiceNAZIONE2_dts ->  ddlCodiceNAZIONE2_dts  e txtCodiceNAZIONE2_dts ;
            // Details.CodiceREGIONE2_dts ->  ddlCodiceREGIONE2_dts e txtCodiceREGIONE2_dts;
            // Details.CodicePROVINCIA2_dts ->  ddlCodicePROVINCIA2_dts e txtCodicePROVINCIA2_dts ;
            // Details.CodiceCOMUNE2_dts ->  ddlCodiceCOMUNE2_dts e CodiceCOMUNE2_dts ;
            CaricaDllLocalizzazione(Details.CodiceNAZIONE2_dts, Details.CodiceREGIONE2_dts, Details.CodicePROVINCIA2_dts, Details.CodiceCOMUNE2_dts, ddlCodiceNAZIONE2_dts, ddlCodiceREGIONE2_dts, ddlCodicePROVINCIA2_dts, ddlCodiceCOMUNE2_dts, txtCodiceREGIONE2_dts, txtCodicePROVINCIA2_dts, txtCodiceCOMUNE2_dts);


            //Indirizzo3
            txtNomeposizione3_dts.Text = Details.Nomeposizione3_dts;
            txtVia3_dts.Value = Details.Via3_dts;
            txtCap3_dts.Value = Details.Cap3_dts;
            txtTelefono3_dts.Value = Details.Telefono3_dts;
            txtLatitudine3_dts.Text = String.Format(ci, "{0:##.#####################}", new object[] { Details.Latitudine3_dts });
            txtLongitudine3_dts.Text = String.Format(ci, "{0:##.#####################}", new object[] { Details.Longitudine3_dts });

            // Details.CodiceNAZIONE3_dts ->  ddlCodiceNAZIONE3_dts  e txtCodiceNAZIONE3_dts ;
            // Details.CodiceREGIONE3_dts ->  ddlCodiceREGIONE3_dts e txtCodiceREGIONE3_dts;
            // Details.CodicePROVINCIA3_dts ->  ddlCodicePROVINCIA3_dts e txtCodicePROVINCIA3_dts ;
            // Details.CodiceCOMUNE3_dts ->  ddlCodiceCOMUNE3_dts e CodiceCOMUNE3_dts ;
            CaricaDllLocalizzazione(Details.CodiceNAZIONE3_dts, Details.CodiceREGIONE3_dts, Details.CodicePROVINCIA3_dts, Details.CodiceCOMUNE3_dts, ddlCodiceNAZIONE3_dts, ddlCodiceREGIONE3_dts, ddlCodicePROVINCIA3_dts, ddlCodiceCOMUNE3_dts, txtCodiceREGIONE3_dts, txtCodicePROVINCIA3_dts, txtCodiceCOMUNE3_dts);

            //aggiorniamo la visualizzazione dei file nelle sezioni upload
            //pnlCV.Visible = false;
            //if (Details.FotoCollection_M != null)
            //{
            //    Allegato a = Details.FotoCollection_M.Find(o => o.Descrizione == "CV");
            //    if (a != null)
            //    {
            //        string linkallegato = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + a.NomeFile;
            //        litCV.Text = "<a href=\"" + ReplaceAbsoluteLinks(linkallegato) + "\" target=\"_blank\">" + a.NomeFile + "</a>";
            //        lnkCV.CommandArgument = a.NomeFile;
            //        lnkCV.Text = "<i class='glyphicon glyphicon-remove'></i>";
            //        pnlCV.Visible = true;
            //    }
            //    else
            //    {
            //        lnkCV.CommandArgument = "";
            //        lnkCV.Text = "";
            //    }
            //}

            //pnlCEQUIP.Visible = false;
            //if (Details.FotoCollection_M != null)
            //{
            //    Allegato a = Details.FotoCollection_M.Find(o => o.Descrizione == "CEQUIP");
            //    if (a != null)
            //    {
            //        string linkallegato = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + a.NomeFile;
            //        litCEQUIP.Text = "<a href=\"" + ReplaceAbsoluteLinks(linkallegato) + "\" target=\"_blank\">" + a.NomeFile + "</a>";
            //        lnkCEQUIP.CommandArgument = a.NomeFile;
            //        lnkCEQUIP.Text = "<i class='glyphicon glyphicon-remove'></i>";
            //        pnlCEQUIP.Visible = true;
            //    }
            //    else
            //    {
            //        lnkCEQUIP.CommandArgument = "";
            //        lnkCEQUIP.Text = "";
            //    }
            //}
            pnlRitratto.Visible = false;
            if (Details.FotoCollection_M != null)
            {
                Allegato a = Details.FotoCollection_M.Find(o => o.Descrizione == "Ritratto");
                if (a != null)
                {
                    string linkallegato = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + a.NomeFile;
                    litRitratto.Text = "<a href=\"" + ReplaceAbsoluteLinks(linkallegato) + "\" target=\"_blank\">" + a.NomeFile + "</a>";
                    lnkRitratto.CommandArgument = a.NomeFile;
                    lnkRitratto.Text = "<i class='glyphicon glyphicon-remove'></i>";
                    pnlRitratto.Visible = true;
                }
                else
                {
                    lnkRitratto.CommandArgument = "";
                    lnkRitratto.Text = "";
                }
            }

            //----------------------------------------------------------

            txtCampo1I.Text = Details.Campo1I;
            txtCampo2I.Text = Details.Campo2I;
            txtCampo1GB.Text = Details.Campo1GB;
            txtCampo2GB.Text = Details.Campo2GB;
            txtIdcollegato.Text = Details.Id_collegato.ToString();

            txtDenominazioneI.Text = Details.DenominazioneI;//(((Literal)e.Item.FindControl("lit1")).Text);
            txtDenominazioneGB.Text = Details.DenominazioneGB;//(((Literal)e.Item.FindControl("lit2")).Text);
            txtDescrizioneI.Text = Details.DescrizioneI;//(((HiddenField)e.Item.FindControl("hid3")).Value);
            txtDescrizioneGB.Text = Details.DescrizioneGB;//(((HiddenField)e.Item.FindControl("hid4")).Value);
            txtDatitecniciI.Text = Details.DatitecniciI;
            txtDatitecniciGB.Text = Details.DatitecniciGB;
            txtIndirizzo.Text = Details.Indirizzo;

            txtindirizzofatt_dts.Text = Details.indirizzofatt_dts;
            txtnoteriservate_dts.Text = Details.noteriservate_dts;

            if (radricfatt_dts.Items.FindByValue(Details.ricfatt_dts) != null)
                radricfatt_dts.SelectedValue = Details.ricfatt_dts;


            txtEmail.Text = Details.Email;
            txtWebsite.Text = Details.Website;
            txtTelefono.Text = Details.Telefono;
            txtFax.Text = Details.Fax;
            txtVideo.Text = Details.linkVideo;

            txtPrezzo.Text = Details.Prezzo.ToString();
            txtPrezzoListino.Text = Details.PrezzoListino.ToString();
            chkVetrina.Checked = Details.Vetrina;
            chkArchiviato.Checked = Details.Archiviato;

            chkContatto.Checked = Details.Abilitacontatto;

            ci = new System.Globalization.CultureInfo("it");
            txtData.Text = string.Format(ci, "{0:dd/MM/yyyy HH:mm:ss}", new object[] { Details.DataInserimento });

            txtCodiceProd.Text = Details.CodiceProdotto;

            txtAnno.Text = Details.Anno.ToString();
            CaricaDatiDdlCaratteristiche(Details.Caratteristica1, Details.Caratteristica2, Details.Caratteristica3, Details.Caratteristica4, Details.Caratteristica5, Details.Caratteristica6);

            CaricaDatiDdlRicerca(Details.CodiceRegione, Details.CodiceProvincia, Details.CodiceComune, Details.CodiceCategoria, Details.CodiceCategoria2Liv);

            //Impostiamo l'url per la foto grande(Prendo sempre la prima quando cambio elemento del repeater)
            if (Details.FotoCollection_M != null && Details.FotoCollection_M.Count > 0)
            {
                imgFoto.ImageUrl = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + Details.FotoCollection_M[0].NomeFile;
                NomeFotoSelezionata = Details.FotoCollection_M[0].NomeFile;
                txtDescrizione.Text = Details.FotoCollection_M[0].Descrizione;
            }
            else
                imgFoto.ImageUrl = "";
            //Carichiamo la galleria delle foto
            rptImmagini.DataSource = Details.FotoCollection_M;
            rptImmagini.DataBind();
        }
        else
        {
            this.SvuotaDettaglio();
        }
    }


    protected void lnkCV_Click(object sender, EventArgs e)
    {
        EliminaFile(OffertaIDSelected, ((LinkButton)sender).CommandArgument);

    }
    protected void lnkCEQUIP_Click(object sender, EventArgs e)
    {
        EliminaFile(OffertaIDSelected, ((LinkButton)sender).CommandArgument);
    }
    protected void lnkRitratto_Click(object sender, EventArgs e)
    {
        EliminaFile(OffertaIDSelected, ((LinkButton)sender).CommandArgument);
    }

    protected void btnCerca_Click(object sender, EventArgs e)
    {
        //testoricerca
        testoricerca = Server.HtmlEncode(txtinputCerca.Text);
        mese = txtinputmese.Text;
        anno = txtinputanno.Text;
        //Stato pagamenti
        string pagamenti = rdPagamenti.SelectedItem.Value;
        string annopagamenti = txtinputannopagamenti.Text;
        if (!string.IsNullOrWhiteSpace(pagamenti) && !string.IsNullOrWhiteSpace(annopagamenti))
            stringafiltropagamenti = annopagamenti + "," + pagamenti.ToString();

        CaricaDati();
    }

    protected void rptOfferte_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                //QUI CONTROLLO ED EVIDENZIO LA RIGA DEL REPEATER ATTUALMENTE SELEZIONATA
                ImageButton lnkImg = (ImageButton)e.Item.FindControl("imgSelect");
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

        if (((ImageButton)sender).CommandArgument == OffertaIDSelected)
        {
            ((ImageButton)sender).ImageUrl = "~/images/arrow_im_1.jpg";
            ((ImageButton)sender).BackColor = System.Drawing.ColorTranslator.FromHtml("#000000");

        }
        else
        {
            ((ImageButton)sender).ImageUrl = "~/images/search_icone.jpg";
            ((ImageButton)sender).BackColor = System.Drawing.ColorTranslator.FromHtml("#e9e9a4"); //System.Drawing.Color.Transparent;
        }
    }
    protected void link_click(object sender, EventArgs e)
    {
        this.SvuotaDettaglio();
        OffertaIDSelected = ((ImageButton)(sender)).CommandArgument.ToString();
        NomeFotoSelezionata = "";

        this.AggiornaDettaglio(OffertaIDSelected);
        btnCancella.Enabled = true;
        btnAggiorna.Enabled = true;
        btnAggiorna.ValidationGroup = "";
    }
    protected void linkgalleria_click(object sender, EventArgs e)
    {
        NomeFotoSelezionata = ((ImageButton)(sender)).CommandArgument.ToString();

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


        txtDescrizione.Text = ((ImageButton)(sender)).ToolTip;
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
                    string datimodificati = "";

                    updrecord.Id = tmp;
                    updrecord = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);

                    long tmpcoll = 0;
                    if (long.TryParse(txtIdcollegato.Text, out tmpcoll))
                        updrecord.Id_collegato = tmpcoll;
                    tmpcoll = 0;
                    if (long.TryParse(txtAnno.Text, out tmpcoll))
                        updrecord.Anno = tmpcoll;

                    tmpcoll = 0;

                    //if (long.TryParse(ddlCaratteristica1.SelectedValue, out tmpcoll))
                    //{
                    //    if (updrecord.Caratteristica1 != tmpcoll)
                    //    {
                    //        datimodificati += "modificata categoria professionale<br/>";

                    //    } updrecord.Caratteristica1 = tmpcoll;
                    //}
                    //tmpcoll = 0;
                    //if (long.TryParse(ddlCaratteristica2.SelectedValue, out tmpcoll))
                    //{
                    //    if (updrecord.Caratteristica2 != tmpcoll)
                    //        datimodificati += "modificata caregoria sicpre<br/>";

                    //    updrecord.Caratteristica2 = tmpcoll;
                    //}
                    tmpcoll = 0;
                    if (long.TryParse(ddlCaratteristica3.SelectedValue, out tmpcoll))
                    {
                        if (updrecord.Caratteristica3 != tmpcoll)
                            datimodificati += "modificata caregoria socio<br/>";
                        updrecord.Caratteristica3 = tmpcoll;
                    } 
                    
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

                    updrecord.DenominazioneI = txtDenominazioneI.Text;
                    updrecord.DenominazioneGB = txtDenominazioneGB.Text;
                    updrecord.DescrizioneI = txtDescrizioneI.Text;
                    updrecord.DescrizioneGB = txtDescrizioneGB.Text;
                    updrecord.DatitecniciGB = txtDatitecniciGB.Text;
                    updrecord.DatitecniciI = txtDatitecniciI.Text;

                    if (updrecord.Indirizzo != txtIndirizzo.Text)
                    {
                        datimodificati += "modificato indirizzo journal: ";
                        datimodificati += txtIndirizzo.Text + "<br/>";
                    }
                    updrecord.Indirizzo = txtIndirizzo.Text;

                    if (updrecord.indirizzofatt_dts != txtindirizzofatt_dts.Text)
                    {
                        datimodificati += "modificato indirizzo fatturazione: ";
                        datimodificati += txtindirizzofatt_dts.Text + "<br/>";
                    }
                    updrecord.indirizzofatt_dts = txtindirizzofatt_dts.Text;

                    updrecord.noteriservate_dts = txtnoteriservate_dts.Text;

                    if (updrecord.ricfatt_dts != radricfatt_dts.SelectedValue)
                    {
                        datimodificati += "modificato scelta fatturazione: ";
                        datimodificati += radricfatt_dts.SelectedValue + "<br/>";
                    }
                    updrecord.ricfatt_dts = radricfatt_dts.SelectedValue;

                    if (updrecord.Email != txtEmail.Text)
                    {
                        datimodificati += "modificato email pubblica: ";
                        datimodificati += txtEmail.Text + "<br/>";
                    }
                    updrecord.Email = txtEmail.Text;

                    if (updrecord.Website != txtWebsite.Text)
                    {
                        datimodificati += "modificato website: ";
                        datimodificati += txtWebsite.Text + "<br/>";
                    }
                    updrecord.Website = txtWebsite.Text;

                    if (updrecord.Telefono != txtTelefono.Text)
                    {
                        datimodificati += "modificato telefono pubblico: ";
                        datimodificati += txtTelefono.Text + "<br/>";
                    }
                    updrecord.Telefono = txtTelefono.Text;

                    if (updrecord.Fax != txtFax.Text)
                    {
                        datimodificati += "modificato fax pubblico: ";
                        datimodificati += txtFax.Text + "<br/>";
                    }
                    updrecord.Fax = txtFax.Text;


                    updrecord.linkVideo = txtVideo.Text;
                    updrecord.CodiceComune = ddlComune.SelectedValue;
                    updrecord.CodiceProvincia = ddlProvincia.SelectedValue;
                    updrecord.CodiceRegione = ddlRegione.SelectedValue;

                    updrecord.CodiceProdotto = txtCodiceProd.Text;

                    updrecord.CodiceCategoria = ddlProdotto.SelectedValue;
                    updrecord.CodiceCategoria2Liv = ddlSottoProdotto.SelectedValue;

                    updrecord.Vetrina = chkVetrina.Checked;


                    updrecord.Abilitacontatto = chkContatto.Checked;

                    double _tmpdbl = 0;
                    double.TryParse(txtPrezzo.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Prezzo = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtPrezzoListino.Text, out _tmpdbl);//Mettere textbox per prezzo listino
                    updrecord.PrezzoListino = _tmpdbl;

                    DateTime _tmpdate = System.DateTime.Now;
                    if (!DateTime.TryParse(txtData.Text, out _tmpdate))
                        _tmpdate = System.DateTime.Now;
                    updrecord.DataInserimento = _tmpdate;


                    // ----------SEZIONE DETTAGLIO --------------------------------------------------------------------

                    updrecord.Nome_dts = txtNome_dts.Text;
                    updrecord.Cognome_dts = txtCognome_dts.Text;

                    _tmpdate = System.DateTime.MinValue;
                    if (!DateTime.TryParse(txtDatanascita_dts.Text, out _tmpdate))
                        _tmpdate = System.DateTime.MinValue;
                    updrecord.Datanascita_dts = _tmpdate;

                    if (updrecord.Pivacf_dts != txtPivacf_dts.Text)
                    {
                        datimodificati += "modificato codice fiscale : ";
                        datimodificati += txtPivacf_dts.Text + "<br/>";
                    }
                    updrecord.Pivacf_dts = txtPivacf_dts.Text;

                    //updrecord.Sociopresentatore1_dts = txtSociopresentatore1_dts.Text;
                    //updrecord.Sociopresentatore2_dts = txtSociopresentatore2_dts.Text;

                    if (updrecord.Telefonoprivato_dts != txtTelefonoprivato_dts.Text)
                    {
                        datimodificati += "modificato telefono riservato : ";
                        datimodificati += txtTelefonoprivato_dts.Text + "<br/>";
                    }
                    updrecord.Telefonoprivato_dts = txtTelefonoprivato_dts.Text;
                    if (updrecord.Emailriservata_dts != txtEmailriservata_dts.Text)
                    {
                        datimodificati += "modificato email riservata : ";
                        datimodificati += txtTelefonoprivato_dts.Text + "<br/>";
                    }
                    updrecord.Emailriservata_dts = txtEmailriservata_dts.Text;

                    //updrecord.Annolaurea_dts = txtAnnolaurea_dts.Text;
                    //updrecord.Annospecializzazione_dts = txtAnnospecializzazione_dts.Text;
                    //updrecord.Altrespecializzazioni_dts = txtAltrespecializzazioni_dts.Text;
                    //updrecord.Socioaltraassociazione_dts = txtSocioaltraassociazione_dts.Text;

                    ////Boolean
                    //bool _b = false;
                    //bool.TryParse(radSocioSicpre_dts.SelectedValue, out _b);
                    //updrecord.SocioSicpre_dts = _b; //chkSocioSicpre_dts.Checked;
                    //_b = false;
                    //bool.TryParse(radSocioIsaps_dts.SelectedValue, out _b);
                    //updrecord.SocioIsaps_dts = _b;// chkSocioIsaps_dts.Checked;

                    //updrecord.AccettazioneStatuto_dts = chkAccettazioneStatuto_dts.Checked;
                    //updrecord.Certificazione_dts = chkCertificazione_dts.Checked;
                    updrecord.Bloccoaccesso_dts = chkBloccoaccesso_dts.Checked;



                    //Dati aggiunti

                    //updrecord.locordine_dts = txtLocordine_dts.Text;
                    //updrecord.niscrordine_dts = txtNiscrordine_dts.Text;
                    //updrecord.annofrequenza_dts = txtannofrequenza_dts.Text;
                    //updrecord.nomeuniversita_dts = txtnomeuniversita_dts.Text;
                    //updrecord.dettagliuniversita_dts = txtdettagliuniversita_dts.Text;

                    //updrecord.Textfield1_dts = txtTextfield1_dts.Text;

                    //updrecord.Boolfields_dts = CreastringaBoolfields();
                    //updrecord.Interventieseguiti_dts = CreastringTblinterventi();


                    updrecord.Trattamenticollegati_dts = Creastringatrattamenti();
                    updrecord.Pagamenti_dts = Creastringapagamenti();

                    //Indirizzo1 (SEDE1)
                    updrecord.Nomeposizione1_dts = txtNomeposizione1_dts.Text;
                    updrecord.Via1_dts = txtVia1_dts.Value;
                    updrecord.Cap1_dts = txtCap1_dts.Value;
                    updrecord.Telefono1_dts = txtTelefono1_dts.Value;

                    _tmpdbl = 0;
                    double.TryParse(txtLatitudine1_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Latitudine1_dts = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtLongitudine1_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Longitudine1_dts = _tmpdbl;
                    updrecord.CodiceNAZIONE1_dts = ddlCodiceNAZIONE1_dts.SelectedValue;
                    updrecord.CodiceREGIONE1_dts = txtCodiceREGIONE1_dts.Value;
                    updrecord.CodicePROVINCIA1_dts = txtCodicePROVINCIA1_dts.Value;
                    updrecord.CodiceCOMUNE1_dts = txtCodiceCOMUNE1_dts.Value;

                    //Indirizzo2 (SEDE2)
                    updrecord.Nomeposizione2_dts = txtNomeposizione2_dts.Text;
                    updrecord.Via2_dts = txtVia2_dts.Value;
                    updrecord.Cap2_dts = txtCap2_dts.Value;
                    updrecord.Telefono2_dts = txtTelefono2_dts.Value;

                    _tmpdbl = 0;
                    double.TryParse(txtLatitudine2_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Latitudine2_dts = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtLongitudine2_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Longitudine2_dts = _tmpdbl;
                    updrecord.CodiceNAZIONE2_dts = ddlCodiceNAZIONE2_dts.SelectedValue;
                    updrecord.CodiceREGIONE2_dts = txtCodiceREGIONE2_dts.Value;
                    updrecord.CodicePROVINCIA2_dts = txtCodicePROVINCIA2_dts.Value;
                    updrecord.CodiceCOMUNE2_dts = txtCodiceCOMUNE2_dts.Value;

                    //Indirizzo3 (SEDE3)
                    updrecord.Nomeposizione3_dts = txtNomeposizione3_dts.Text;
                    updrecord.Via3_dts = txtVia3_dts.Value;
                    updrecord.Cap3_dts = txtCap3_dts.Value;
                    updrecord.Telefono3_dts = txtTelefono3_dts.Value;

                    _tmpdbl = 0;
                    double.TryParse(txtLatitudine3_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Latitudine3_dts = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtLongitudine3_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Longitudine3_dts = _tmpdbl;
                    updrecord.CodiceNAZIONE3_dts = ddlCodiceNAZIONE3_dts.SelectedValue;
                    updrecord.CodiceREGIONE3_dts = txtCodiceREGIONE3_dts.Value;
                    updrecord.CodicePROVINCIA3_dts = txtCodicePROVINCIA3_dts.Value;
                    updrecord.CodiceCOMUNE3_dts = txtCodiceCOMUNE3_dts.Value;
                    //----------------------------------------------------------

#if false //da abilitare in fase produzione ( non debug )
                    if (updrecord.Archiviato != chkArchiviato.Checked)
                    {
                        //Modificata la visibilità online del profilo del socio
                        InviaMailAvvisoArchivioSocio(updrecord, chkArchiviato.Checked);

                    }
#endif
                    updrecord.Archiviato = chkArchiviato.Checked;


                    //Questi li devi riempire con la lista delle foto
                    //updrecord.FotoCollection_M.Schema = txtFotoSchema.Value;
                    //updrecord.FotoCollection_M.Valori = txtFotoValori.Value;
                    offDM.UpdateOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    offDM.UpdateOffertaCollegata(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

                    AggiornaAllegati();

                    btnAggiorna.Text = "Modifica";
                    btnAggiorna.ValidationGroup = "";

                    btnAnnulla.Visible = false;
                    ImpostaDettaglioSolaLettura(true);
                    btnCancella.Visible = true;
                    btnCancella.Enabled = true;
                    btnAggiorna.Enabled = true;

#if false
                    if (!string.IsNullOrEmpty(datimodificati))
                        InviaMailAvvisoModificaASegreteria(datimodificati); 
#endif

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
                    updrecord.CodiceTipologia = TipologiaOfferte;

                long tmpcoll = 0;
                    if (long.TryParse(txtIdcollegato.Text, out tmpcoll))
                        updrecord.Id_collegato = tmpcoll;

                    tmpcoll = 0;
                    if (long.TryParse(txtAnno.Text, out tmpcoll))
                        updrecord.Anno = tmpcoll;
                //tmpcoll = 0;
                //if (long.TryParse(ddlCaratteristica1.SelectedValue, out tmpcoll))
                //    updrecord.Caratteristica1 = tmpcoll;
                //tmpcoll = 0;
                //if (long.TryParse(ddlCaratteristica2.SelectedValue, out tmpcoll))
                //    updrecord.Caratteristica2 = tmpcoll;

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

                    updrecord.DenominazioneI = txtDenominazioneI.Text;
                    updrecord.DenominazioneGB = txtDenominazioneGB.Text;
                    updrecord.DescrizioneI = txtDescrizioneI.Text;
                    updrecord.DescrizioneGB = txtDescrizioneGB.Text;
                    updrecord.DatitecniciGB = txtDatitecniciGB.Text;
                    updrecord.DatitecniciI = txtDatitecniciI.Text;
                    updrecord.Indirizzo = txtIndirizzo.Text;
                    updrecord.indirizzofatt_dts = txtindirizzofatt_dts.Text;
                    updrecord.noteriservate_dts = txtnoteriservate_dts.Text;
                    updrecord.ricfatt_dts = radricfatt_dts.SelectedValue;

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

                    double _tmpdbl = 0;
                    double.TryParse(txtPrezzo.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Prezzo = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtPrezzoListino.Text, out _tmpdbl);//Mettere textbox per prezzo listino
                    updrecord.PrezzoListino = _tmpdbl;

                    DateTime _tmpdate = System.DateTime.Now;
                    if (!DateTime.TryParse(txtData.Text, out _tmpdate))
                        _tmpdate = System.DateTime.Now;
                    updrecord.DataInserimento = _tmpdate;


                    // ----------SEZIONE DETTAGLIO------------------------------
                    updrecord.Nome_dts = txtNome_dts.Text;
                    updrecord.Cognome_dts = txtCognome_dts.Text;
                    _tmpdate = System.DateTime.MinValue;
                    if (!DateTime.TryParse(txtDatanascita_dts.Text, out _tmpdate))
                        _tmpdate = System.DateTime.MinValue;
                    updrecord.Datanascita_dts = _tmpdate;


                    updrecord.Pivacf_dts = txtPivacf_dts.Text;
                    //updrecord.Sociopresentatore1_dts = txtSociopresentatore1_dts.Text;
                    //updrecord.Sociopresentatore2_dts = txtSociopresentatore2_dts.Text;
                    updrecord.Telefonoprivato_dts = txtTelefonoprivato_dts.Text;
                    //updrecord.Annolaurea_dts = txtAnnolaurea_dts.Text;
                    //updrecord.Annospecializzazione_dts = txtAnnospecializzazione_dts.Text;
                    //updrecord.Altrespecializzazioni_dts = txtAltrespecializzazioni_dts.Text;
                    //updrecord.Socioaltraassociazione_dts = txtSocioaltraassociazione_dts.Text;
                    updrecord.Emailriservata_dts = txtEmailriservata_dts.Text;

                    //Boolean

                    //bool _b = false;
                    //bool.TryParse(radSocioSicpre_dts.SelectedValue, out _b);
                    //updrecord.SocioSicpre_dts = _b; //chkSocioSicpre_dts.Checked;
                    //_b = false;
                    //bool.TryParse(radSocioIsaps_dts.SelectedValue, out _b);
                    //updrecord.SocioIsaps_dts = _b;// chkSocioIsaps_dts.Checked;

                    //updrecord.AccettazioneStatuto_dts = chkAccettazioneStatuto_dts.Checked;
                    //updrecord.Certificazione_dts = chkCertificazione_dts.Checked;
                    updrecord.Bloccoaccesso_dts = chkBloccoaccesso_dts.Checked;


                    //updrecord.locordine_dts = txtLocordine_dts.Text;
                    //updrecord.niscrordine_dts = txtNiscrordine_dts.Text;
                    //updrecord.annofrequenza_dts = txtannofrequenza_dts.Text;
                    //updrecord.nomeuniversita_dts = txtnomeuniversita_dts.Text;
                    //updrecord.dettagliuniversita_dts = txtdettagliuniversita_dts.Text;

                    //updrecord.Textfield1_dts = txtTextfield1_dts.Text;

                    //updrecord.Boolfields_dts = CreastringaBoolfields();
                    //updrecord.Interventieseguiti_dts = CreastringTblinterventi();

                    //   funzioni split e reconstruct!!!!!!
                    updrecord.Trattamenticollegati_dts = Creastringatrattamenti();
                    updrecord.Pagamenti_dts = Creastringapagamenti();

                    //Indirizzo1
                    updrecord.Nomeposizione1_dts = txtNomeposizione1_dts.Text;
                    updrecord.Via1_dts = txtVia1_dts.Value;
                    updrecord.Cap1_dts = txtCap1_dts.Value;
                    updrecord.Telefono1_dts = txtTelefono1_dts.Value;

                    _tmpdbl = 0;
                    double.TryParse(txtLatitudine1_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Latitudine1_dts = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtLongitudine1_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Longitudine1_dts = _tmpdbl;
                    updrecord.CodiceNAZIONE1_dts = ddlCodiceNAZIONE1_dts.SelectedValue;
                    updrecord.CodiceREGIONE1_dts = txtCodiceREGIONE1_dts.Value;
                    updrecord.CodicePROVINCIA1_dts = txtCodicePROVINCIA1_dts.Value;
                    updrecord.CodiceCOMUNE1_dts = txtCodiceCOMUNE1_dts.Value;

                    //Indirizzo2
                    updrecord.Nomeposizione2_dts = txtNomeposizione2_dts.Text;
                    updrecord.Via2_dts = txtVia2_dts.Value;
                    updrecord.Cap2_dts = txtCap2_dts.Value;
                    updrecord.Telefono2_dts = txtTelefono2_dts.Value;

                    _tmpdbl = 0;
                    double.TryParse(txtLatitudine2_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Latitudine2_dts = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtLongitudine2_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Longitudine2_dts = _tmpdbl;
                    updrecord.CodiceNAZIONE2_dts = ddlCodiceNAZIONE2_dts.SelectedValue;
                    updrecord.CodiceREGIONE2_dts = txtCodiceREGIONE2_dts.Value;
                    updrecord.CodicePROVINCIA2_dts = txtCodicePROVINCIA2_dts.Value;
                    updrecord.CodiceCOMUNE2_dts = txtCodiceCOMUNE2_dts.Value;

                    //Indirizzo3
                    updrecord.Nomeposizione3_dts = txtNomeposizione3_dts.Text;
                    updrecord.Via3_dts = txtVia3_dts.Value;
                    updrecord.Cap3_dts = txtCap3_dts.Value;
                    updrecord.Telefono3_dts = txtTelefono3_dts.Value;

                    _tmpdbl = 0;
                    double.TryParse(txtLatitudine3_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Latitudine3_dts = _tmpdbl;
                    _tmpdbl = 0;
                    double.TryParse(txtLongitudine3_dts.Text, out _tmpdbl);//Mettere textbox per prezzo
                    updrecord.Longitudine3_dts = _tmpdbl;
                    updrecord.CodiceNAZIONE3_dts = ddlCodiceNAZIONE3_dts.SelectedValue;
                    updrecord.CodiceREGIONE3_dts = txtCodiceREGIONE3_dts.Value;
                    updrecord.CodicePROVINCIA3_dts = txtCodicePROVINCIA3_dts.Value;
                    updrecord.CodiceCOMUNE3_dts = txtCodiceCOMUNE3_dts.Value;


                    //----------------------------------------------------------
                    //VERIFICA PER LA CREAZIONE DELL'UTENTE
                    //----------------------------------------------------------

                    if (VerificaPresenzaUtente(updrecord.Cognome_dts.Replace(" ", "").Trim().ToLower() + updrecord.Nome_dts.Replace(" ", "").Trim().ToLower()))
                    {
                        output.Text = "Attenzione già presente utente con stesso Nome e Cognome. Verificare";
                        return;
                    }
                    //DOvrei controllare anche le email per verificare la ripetizione utenti

                    //----------------------------------------------------------
                    offDM.InsertOffertaCollegata(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    offDM.InsertOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    //Seleziono l'elemento appena inserito e ci inserisco gli allegati necessari
                    OffertaIDSelected = updrecord.Id.ToString();

                    //Creo l'utente del membership
                    //string username = "";
                    //string password = "";
                    //CreaUtenteAssociato(updrecord, ref username, ref password);

                    AggiornaAllegati(); //Inserisco i file che devo in allegato


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


#if false

    private void InviaMailAvvisoModificaASegreteria(string datimodificati)
    {
        if (debug) return;

        if (ControllaRuolo(User.Identity.Name, "Socio"))
        {
            try
            {
                string SoggettoMail = Nome + " : " + "Mail modifica scheda sul sito da parte dell' utente " + User.Identity.Name;
                //string idsocio = getidsocio(User.Identity.Name);

                string Descrizione = "";
                Descrizione = references.ResMan("Specific",Lingua,"testoModificaSocio") + "<br/><br/>";
                Descrizione += datimodificati + "<br/>";



                Descrizione += references.ResMan("Common",Lingua,"testoCredits").ToString() + "<br/>";
                Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Email, Nome);
            }
            catch (Exception err)
            {
                output.Text = err.Message;
            }
        }
    }

    private void InviaMailAvvisoArchivioSocio(Offerte socio, bool statoArchivio)
    {
        if (debug) return;
        try
        {
            string SoggettoMail = Nome + " : " + "Mail modifica visibilità scheda sul sito";

            //Dati per la mail
            string nomeaderente = socio.Cognome_dts + " " + socio.Nome_dts;
            string Mailaderente = socio.Emailriservata_dts;

            string Descrizione = "";
            if (statoArchivio)
            {
                Descrizione = references.ResMan("Specific",Lingua,"testoArchivioOff") + "<br/>";
            }
            else
            {
                Descrizione = references.ResMan("Specific",Lingua,"testoArchivioOn") + "<br/>";
            }

            Descrizione += references.ResMan("Common",Lingua,"FormTesto2").ToString() + " " + socio.Nome_dts + "<br/>";
            Descrizione += references.ResMan("Common",Lingua,"FormTesto3").ToString() + " " + socio.Cognome_dts + "<br/>";
            Descrizione += references.ResMan("Common",Lingua,"FormTesto4").ToString() + " " + socio.Emailriservata_dts + "<br/>";
            Descrizione += references.ResMan("Common",Lingua,"testoCredits").ToString() + "<br/>";

#if false//DEBUG!!!!! TOGLIERE PRIMA DI ONLINE!!!!!  MANDA TUTTE LE EMAIL AL GESTORE E NON AL SOCIO
            Mailaderente = Email;
#endif
            Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailaderente, nomeaderente);



        }
        catch (Exception err)
        {
            output.Text = err.Message;
        }
    }



#endif
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

                    //Eliminiamo l'utente del membership se presente
                    EliminaUtenteAssociato(item);

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
        txtindirizzofatt_dts.Text = "";
        txtnoteriservate_dts.Text = "";

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

        OffertaIDSelected = "";

        txtCampo1GB.Text = "";
        txtCampo2I.Text = "";
        txtCampo1I.Text = "";
        txtCampo2GB.Text = "";
        txtIdcollegato.Text = "";
        txtDenominazioneI.Text = "";
        txtDenominazioneGB.Text = "";
        txtDescrizioneGB.Text = "";
        txtDescrizioneI.Text = "";
        txtPrezzo.Text = "";
        txtPrezzoListino.Text = "";
        chkVetrina.Checked = false;
        chkArchiviato.Checked = false;
        chkContatto.Checked = false;
        txtDescrizione.Text = "";
        txtCodiceProd.Text = "";
        //txtFotoSchema.Value = "";
        //txtFotoValori.Value = "";
        txtDatitecniciGB.Text = "";
        txtDatitecniciI.Text = "";
        txtIndirizzo.Text = "";
        txtindirizzofatt_dts.Text = "";
        txtnoteriservate_dts.Text = "";

        txtEmail.Text = "";
        txtWebsite.Text = "";
        txtTelefono.Text = "";
        txtFax.Text = "";
        txtVideo.Text = "";
        txtData.Text = "";
        CaricaDatiDdlRicerca("", "", "", "", "");
        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);
        txtAnno.Text = "";


        // ----------SEZIONE DETTAGLIO
        txtNome_dts.Text = "";
        txtCognome_dts.Text = "";

        txtDatanascita_dts.Text = string.Empty;
        txtPivacf_dts.Text = string.Empty;
        //txtSociopresentatore1_dts.Text = string.Empty;
        //txtSociopresentatore2_dts.Text = string.Empty;
        txtTelefonoprivato_dts.Text = string.Empty;
        //txtAnnolaurea_dts.Text = string.Empty;
        //txtAnnospecializzazione_dts.Text = string.Empty;
        //txtAltrespecializzazioni_dts.Text = string.Empty;
        //txtSocioaltraassociazione_dts.Text = string.Empty;
        txtEmailriservata_dts.Text = string.Empty;

        //Boolean
        radricfatt_dts.ClearSelection();
        //radSocioIsaps_dts.ClearSelection();
        //radSocioSicpre_dts.ClearSelection();

        //chkAccettazioneStatuto_dts.Checked = false;
        //chkCertificazione_dts.Checked = false;
        chkBloccoaccesso_dts.Checked = false;


        //txtLocordine_dts.Text = string.Empty;
        //txtNiscrordine_dts.Text = string.Empty;
        //txtannofrequenza_dts.Text = string.Empty;
        //txtnomeuniversita_dts.Text = string.Empty;
        //txtdettagliuniversita_dts.Text = string.Empty;

        //VisualizzaBoolfields("");
        //VisualizzaTblinterventi("");


        VisualizzaTrattamenti("");
        VisualizzaPagamenti("", 2011, System.DateTime.Now.Year);
        //pnlCV.Visible = false;
        //pnlCEQUIP.Visible = false;
        pnlRitratto.Visible = false;

        //Indirizzo1
        txtNomeposizione1_dts.Text = string.Empty;
        txtVia1_dts.Value = string.Empty;
        txtCap1_dts.Value = string.Empty;
        txtTelefono1_dts.Value = string.Empty;
        txtLatitudine1_dts.Text = string.Empty;
        txtLongitudine1_dts.Text = string.Empty;


        txtCodiceREGIONE1_dts.Value = string.Empty;
        txtCodicePROVINCIA1_dts.Value = string.Empty;
        txtCodiceCOMUNE1_dts.Value = string.Empty;

        //Indirizzo2
        txtNomeposizione2_dts.Text = string.Empty;
        txtVia2_dts.Value = string.Empty;
        txtCap2_dts.Value = string.Empty;
        txtTelefono2_dts.Value = string.Empty;
        txtLatitudine2_dts.Text = string.Empty;
        txtLongitudine2_dts.Text = string.Empty;

        txtCodiceREGIONE2_dts.Value = string.Empty;
        txtCodicePROVINCIA2_dts.Value = string.Empty;
        txtCodiceCOMUNE2_dts.Value = string.Empty;
        //indirizzo3
        txtNomeposizione3_dts.Text = string.Empty;
        txtVia3_dts.Value = string.Empty;
        txtCap3_dts.Value = string.Empty;
        txtTelefono3_dts.Value = string.Empty;
        txtLatitudine3_dts.Text = string.Empty;
        txtLongitudine3_dts.Text = string.Empty;

        txtCodiceREGIONE3_dts.Value = string.Empty;
        txtCodicePROVINCIA3_dts.Value = string.Empty;
        txtCodiceCOMUNE3_dts.Value = string.Empty;

        CaricaDllLocalizzazione("IT", "", "", "", ddlCodiceNAZIONE1_dts, ddlCodiceREGIONE1_dts, ddlCodicePROVINCIA1_dts, ddlCodiceCOMUNE1_dts, txtCodiceREGIONE1_dts, txtCodicePROVINCIA1_dts, txtCodiceCOMUNE1_dts);
        CaricaDllLocalizzazione("IT", "", "", "", ddlCodiceNAZIONE2_dts, ddlCodiceREGIONE2_dts, ddlCodicePROVINCIA2_dts, ddlCodiceCOMUNE2_dts, txtCodiceREGIONE2_dts, txtCodicePROVINCIA2_dts, txtCodiceCOMUNE2_dts);
        CaricaDllLocalizzazione("IT", "", "", "", ddlCodiceNAZIONE3_dts, ddlCodiceREGIONE3_dts, ddlCodicePROVINCIA3_dts, ddlCodiceCOMUNE3_dts, txtCodiceREGIONE3_dts, txtCodicePROVINCIA3_dts, txtCodiceCOMUNE3_dts);

        //----------------------------------------------------------
    }
    protected void btnAnnulla_Click(object sender, EventArgs e)
    {
        this.SvuotaDettaglio();
        this.CaricaDati();
        btnAnnulla.Visible = false;
        btnCancella.Visible = true;
        btnCancella.Enabled = false;
        this.ImpostaDettaglioSolaLettura(true);

        if (ControllaRuolo(User.Identity.Name, "Socio"))
            ImpostaVisualizzazioneSocio();
    }
    protected void ImpostaDettaglioSolaLettura(bool valore)
    {
        txtCampo1I.ReadOnly = valore;
        txtCampo2I.ReadOnly = valore;
        txtCampo1GB.ReadOnly = valore;
        txtCampo2GB.ReadOnly = valore;
        txtIdcollegato.ReadOnly = valore;
        txtDenominazioneI.ReadOnly = valore;
        txtDenominazioneGB.ReadOnly = valore;
        txtDescrizioneGB.ReadOnly = valore;
        txtDescrizioneI.ReadOnly = valore;
        txtPrezzo.ReadOnly = valore;
        txtPrezzoListino.ReadOnly = valore;
        txtCodiceProd.ReadOnly = valore;
        chkVetrina.Enabled = !valore;
        chkArchiviato.Enabled = !valore;
        //txtFotoSchema.Value = "";
        //txtFotoValori.Value = "";
        txtDatitecniciGB.ReadOnly = valore;
        txtDatitecniciI.ReadOnly = valore;
        txtIndirizzo.ReadOnly = valore;

        txtnoteriservate_dts.ReadOnly = valore;
        txtindirizzofatt_dts.ReadOnly = valore;

        txtEmail.ReadOnly = valore;
        txtWebsite.ReadOnly = valore;
        txtTelefono.ReadOnly = valore;
        txtFax.ReadOnly = valore;
        txtVideo.ReadOnly = valore;
        txtAnno.ReadOnly = valore;
        ddlRegione.Enabled = !valore;
        ddlProvincia.Enabled = !valore;
        ddlComune.Enabled = !valore;

        //ddlCaratteristica1.Enabled = !valore;
        //ddlCaratteristica2.Enabled = !valore;
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
        NomeNuovoSottIt.Enabled = false;
        NomeNuovoSottEng.Enabled = false;
        chkContatto.Enabled = !valore;

        // ----------SEZIONE DETTAGLIO
        txtNome_dts.ReadOnly = valore;
        txtCognome_dts.ReadOnly = valore;
        txtDatanascita_dts.ReadOnly = valore;
        txtPivacf_dts.ReadOnly = valore;
        //txtSociopresentatore1_dts.ReadOnly = valore;
        //txtSociopresentatore2_dts.ReadOnly = valore;
        txtTelefonoprivato_dts.ReadOnly = valore;
        //txtAnnolaurea_dts.ReadOnly = valore;
        //txtAnnospecializzazione_dts.ReadOnly = valore;
        //txtAltrespecializzazioni_dts.ReadOnly = valore;
        //txtSocioaltraassociazione_dts.ReadOnly = valore;
        txtEmailriservata_dts.ReadOnly = valore;

        //Boolean
        radricfatt_dts.Enabled = !valore;

        //radSocioSicpre_dts.Enabled = !valore;
        //radSocioIsaps_dts.Enabled = !valore;
        //chkAccettazioneStatuto_dts.Enabled = !valore;
        //chkCertificazione_dts.Enabled = !valore;
        chkBloccoaccesso_dts.Enabled = !valore;


       
        //txtLocordine_dts.ReadOnly = valore;
        //txtNiscrordine_dts.ReadOnly = valore;
        //txtannofrequenza_dts.ReadOnly = valore;
        //txtnomeuniversita_dts.ReadOnly = valore;
        //txtdettagliuniversita_dts.ReadOnly = valore;
        //txtTextfield1_dts.ReadOnly = valore;

        //radCarriera1.Enabled = !valore;
        //radCarriera2.Enabled = !valore;
        //radCarriera3.Enabled = !valore;
        //radCarriera4.Enabled = !valore;
        //radCarriera5.Enabled = !valore;

        ////Da bloccare l'editing della tabella interventi... da fare
        //radIntervento1op1.Enabled = !valore;
        //radIntervento2op1.Enabled = !valore;

        //radIntervento1op2.Enabled = !valore;
        //radIntervento2op2.Enabled = !valore;
        //radIntervento1op3.Enabled = !valore;
        //radIntervento2op3.Enabled = !valore;
        //radIntervento1op4.Enabled = !valore;
        //radIntervento2op4.Enabled = !valore;
        //radIntervento1op5.Enabled = !valore;
        //radIntervento2op5.Enabled = !valore;
        //radIntervento1op6.Enabled = !valore;
        //radIntervento2op6.Enabled = !valore;
        //radIntervento1op7.Enabled = !valore;
        //radIntervento2op7.Enabled = !valore;
        //radIntervento1op8.Enabled = !valore;
        //radIntervento2op8.Enabled = !valore;
        //radIntervento1op9.Enabled = !valore;
        //radIntervento2op9.Enabled = !valore;

        cbtrattamentilist.Enabled = !valore;
        cbpagamenti.Enabled = !valore;

        //Indirizzo1
        txtNomeposizione1_dts.ReadOnly = valore;
        txtVia1_dts.Disabled = valore;
        txtCap1_dts.Disabled = valore;
        txtTelefono1_dts.Disabled = valore;
        txtLatitudine1_dts.ReadOnly = valore;
        txtLongitudine1_dts.ReadOnly = valore;

        ddlCodiceNAZIONE1_dts.Enabled = !valore;
        ddlCodiceREGIONE1_dts.Enabled = !valore;
        ddlCodicePROVINCIA1_dts.Enabled = !valore;
        ddlCodiceCOMUNE1_dts.Enabled = !valore;

        txtCodiceREGIONE1_dts.Disabled = valore;
        txtCodicePROVINCIA1_dts.Disabled = valore;
        txtCodiceCOMUNE1_dts.Disabled = valore;

        //Indirizzo2
        txtNomeposizione2_dts.ReadOnly = valore;
        txtVia2_dts.Disabled = valore;
        txtCap2_dts.Disabled = valore;
        txtTelefono2_dts.Disabled = valore;
        txtLatitudine2_dts.ReadOnly = valore;
        txtLongitudine2_dts.ReadOnly = valore;

        ddlCodiceNAZIONE2_dts.Enabled = !valore;
        ddlCodiceREGIONE2_dts.Enabled = !valore;
        ddlCodicePROVINCIA2_dts.Enabled = !valore;
        ddlCodiceCOMUNE2_dts.Enabled = !valore;

        txtCodiceREGIONE2_dts.Disabled = valore;
        txtCodicePROVINCIA2_dts.Disabled = valore;
        txtCodiceCOMUNE2_dts.Disabled = valore;
        //indirizzo3
        txtNomeposizione3_dts.ReadOnly = valore;
        txtVia3_dts.Disabled = valore;
        txtCap3_dts.Disabled = valore;
        txtTelefono3_dts.Disabled = valore;
        txtLatitudine3_dts.ReadOnly = valore;
        txtLongitudine3_dts.ReadOnly = valore;


        ddlCodiceNAZIONE3_dts.Enabled = !valore;
        ddlCodiceREGIONE3_dts.Enabled = !valore;
        ddlCodicePROVINCIA3_dts.Enabled = !valore;
        ddlCodiceCOMUNE3_dts.Enabled = !valore;

        txtCodiceREGIONE3_dts.Disabled = valore;
        txtCodicePROVINCIA3_dts.Disabled = valore;
        txtCodiceCOMUNE3_dts.Disabled = valore;

        //----------------------------------------------------------
        //UploadCv.Enabled = !valore;
        //UploadCEQUIP.Enabled = !valore;
        UploadRitratto.Enabled = !valore;
        //----------------------------------------------------------

        if (ControllaRuolo(User.Identity.Name, "Socio"))
            ImpostaVisualizzazioneSocio();
    }

    #region Gestione Foto allegate
    //Controlliamo se necessario inserire allegati o rinnovare i precedenti
    private void AggiornaAllegati()
    {

        //CaricaFile(UploadCv, "CV", OffertaIDSelected);

        //CaricaFile(UploadCEQUIP, "CEQUIP", OffertaIDSelected);

        CaricaFile(UploadRitratto, "Ritratto", OffertaIDSelected);

    }

    protected void btnCarica_Click(object sender, EventArgs e)
    {
        CaricaFile(UploadFoto, txtDescrizione.Text, OffertaIDSelected);
    }
    protected void btnModifica_Click(object sender, EventArgs e)
    {
        ModificaFile(OffertaIDSelected, NomeFotoSelezionata, txtDescrizione.Text);
    }
    protected void btnElimina_Click(object sender, EventArgs e)
    {
        EliminaFile(OffertaIDSelected, NomeFotoSelezionata);
    }

    private void ModificaFile(string idrecored, string nomefile, string Descrizione)
    {
        try
        {
            long i = 0;
            long.TryParse(idrecored, out i);
            bool ret = offDM.modificaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, i, nomefile, Descrizione);
        }
        catch (Exception errins)
        {
            output.Text = errins.Message;
        }

        output.Text = "Descrizone file Modificata Correttamente";
        //Aggiorniamo il repeater e la foto per il record selezionato
        this.CaricaDati();
    }
    private void EliminaFile(string idrecord, string nomefile)
    {
        //Controlliamo se ho selezionato un record
        if (idrecord == null || idrecord == "")
        {
            output.Text = "Selezionare un elemento per cancellare il file";
            return;
        }

        if (nomefile == null || nomefile == "")
        {
            output.Text = "Selezionare una foto da cancellare";
            return;
        }
        long idSelected = 0;
        if (!long.TryParse(idrecord, out idSelected))
        {
            output.Text = "Selezionare un elemento per cancellare la foto";
            return;
        }
        //Ricarichiamo l'offerta selezionata dal db
        //Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected);

        //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
        string pathDestinazione = Server.MapPath(PercorsoFiles + "/" + TipologiaOfferte + "/" + idrecord);
        //-------------------------------------
        //Eliminiamo il file se presente
        //-------------------------------------
        try
        {
            try
            {
                bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, nomefile, "", pathDestinazione);
            }
            catch (Exception errodel)
            {
                output.Text = errodel.Message;
            }

            //Aggiorniamo il repeater e la foto per il record selezionato
            this.CaricaDati();

        }
        catch (Exception errore)
        {
            output.Text += " " + errore.Message;
        }
    }
    private void CaricaFile(FileUpload UploadControl, string DescrizioneFile, string idrecord)
    {
        try
        {
            //Controlliamo se ho selezionato un record
            if (idrecord == null || idrecord == "")
            {
                output.Text = "Selezionare un elemento per associare il file";
                return;
            }
            long idSelected = 0;
            if (!long.TryParse(idrecord, out idSelected))
            {
                output.Text = "Selezionare un elemento per associare il file";
                return;
            }

            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
            string pathDestinazione = Server.MapPath(PercorsoFiles + "/" + TipologiaOfferte + "/" + idrecord);
            if (!Directory.Exists(pathDestinazione))
                Directory.CreateDirectory(pathDestinazione);

            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (UploadControl.HasFile)
            {
                if (UploadControl.PostedFile.ContentLength > 10000000)
                {

                    output.Text = "Il File non può essere caricato perché supera 10MB!";
                }
                else
                {
                    //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
                    string NomeCorretto = UploadControl.FileName.Replace("+", "");
                    NomeCorretto = NomeCorretto.Replace("%", "");
                    NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
                    NomeCorretto = NomeCorretto.Replace(" ", "-").ToLower();
                    //string NomeCorretto = Server.HtmlEncode(FotoUpload1.FileName);
                    if (System.IO.File.Exists(pathDestinazione))
                    {
                        output.Text = ("Il File non può essere caricato perché già presente sul server!");
                    }
                    else
                    {
                        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        if (UploadControl.PostedFile.ContentType == "image/jpeg" || UploadControl.PostedFile.ContentType == "image/pjpeg" || UploadControl.PostedFile.ContentType == "image/gif")
                        {
                            int maxheight = 800;
                            int maxwidth = 1000;
                            bool ridimensiona = true;
                            //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                            if (ResizeAndSave(UploadControl.PostedFile.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                this.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto,450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto);
                                //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
                                //I DATI PER RINTRACCIARE LA FOTO-->SCHEMA E VALORI
                                try
                                {
                                    try
                                    {
                                        bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                    }
                                    catch (Exception errins)
                                    {
                                        output.Text = errins.Message;
                                    }

                                    output.Text += "Foto Inserita Correttamente<br/>";
                                    //Aggiorniamo il repeater e la foto per il record selezionato
                                    this.CaricaDati();
                                    //OfferteCollection list = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
                                    //rptOfferte.DataSource = list;
                                    //rptOfferte.DataBind();


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
                        else if (UploadControl.PostedFile.ContentType == "application/pdf")
                        {

                            //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                            UploadControl.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                            try
                            {
                                try
                                {
                                    bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                }
                                catch (Exception errins)
                                {
                                    output.Text = errins.Message;
                                }

                                output.Text += "Documento Inserito Correttamente<br/>";
                                //Aggiorniamo il repeater e la foto per il record selezionato
                                this.CaricaDati();
                            }
                            catch (Exception error)
                            {
                                //CANCELLO IL FILE UPLOADATO
                                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                //AGGIORNO IL DETAILSVIEW
                                output.Text = error.Message;
                            }
                        }
                        else
                        {

                            //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                            UploadControl.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                            try
                            {
                                try
                                {
                                    bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                }
                                catch (Exception errins)
                                {
                                    output.Text = errins.Message;
                                }

                                output.Text += "Documento Inserito Correttamente<br/>";
                                //Aggiorniamo il repeater e la foto per il record selezionato
                                this.CaricaDati();
                            }
                            catch (Exception error)
                            {
                                //CANCELLO IL FILE UPLOADATO
                                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                //AGGIORNO IL DETAILSVIEW
                                output.Text = error.Message;
                            }
                        }
                    }
                }
            }

        }
        catch (Exception errorecaricamento)
        {
            output.Text += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                output.Text += errorecaricamento.InnerException.Message;

        }
    }
    private bool ResizeAndSave(System.IO.Stream imgStr, int Width, int Height, string Filename, bool ridimensiona)
    {
        try
        {
            using (System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(imgStr))
            {
                if (ridimensiona == true)
                {
                    //CREO LE DIMENSIONI DELLA FOTO SALVATA IN BASE AL RAPORTO ORIGINALE DI ASPETTO
                    int altezzaStream = bmpStream.Height; //altezza foto originale
                    int larghezzaStream = bmpStream.Width; //larghezza foto originale
                    if (altezzaStream <= larghezzaStream)
                        Height = Convert.ToInt32(((double)Width / (double)larghezzaStream) * (double)altezzaStream);
                    else
                        Width = Convert.ToInt32(((double)Height / (double)altezzaStream) * (double)larghezzaStream);

                    //FINE CALCOLO ----------------------------------------------------------
                }

                using (System.Drawing.Bitmap img_orig = new System.Drawing.Bitmap(bmpStream))
                {

                    System.Drawing.Bitmap img_filtrata = img_orig;
                    //FILTRI CONTRASTO BRIGHTNESS/contrast/sturation
                    //img_filtrata = ImageProcessing.applicaSaturationCorrection(img_filtrata, 0.05);
                    //img_filtrata = ImageProcessing.applicaBrightness(img_filtrata, 0.03);
                    //img_filtrata = ImageProcessing.applicaContrast(img_filtrata, 0.75);
                    //img_filtrata = ImageProcessing.applicaAdaptiveSmoothing(img_filtrata);
                    //img_filtrata = ImageProcessing.applicaConservativeSmoothing(img_filtrata);
                    //img_filtrata = ImageProcessing.applicaHSLFilter(img_filtrata, 0.87, 0.075);
                    //img_filtrata = ImageProcessing.applicaGaussianBlur(img_filtrata, 1, 5);
                    //img_filtrata = ImageProcessing.applicaMediano(img_filtrata, 4);
                    // ImageProcessing.NoiseRemoval(img_filtrata);
                    //img_filtrata = ImageProcessing.MeanFilter(img_filtrata, 2);
                    img_filtrata = ImageProcessing.applicaResizeBilinear(img_filtrata, Width, Height); //resisze
                    //img_filtrata = ImageProcessing.applicaResizeBicubic(img_filtrata, Width, Height); //resisze

                    using (System.Drawing.Bitmap img = img_filtrata)
                    {
                        System.Drawing.Imaging.ImageFormat imgF = null;
                        switch (System.IO.Path.GetExtension(Filename).ToLower())
                        {
                            case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif; break;
                            case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; break;
                            case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; break;
                            default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                        }
                        //img.Save(Filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                        if (imgF == System.Drawing.Imaging.ImageFormat.Jpeg)
                        {

                            // Create an Encoder object based on the GUID for the Quality parameter category.
                            ImageCodecInfo jgpEncoder = GetEncoder(imgF); //ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == "image/jpeg");
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            // Create an EncoderParameters object.
                            // An EncoderParameters object has an array of EncoderParameter objects. In this case, there is only one EncoderParameter object in the array.
                            EncoderParameters myEncoderParameters = new EncoderParameters(3);
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                            myEncoderParameters.Param[2] = new EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, (int)EncoderValue.RenderProgressive);

                            img.Save(Filename, jgpEncoder, myEncoderParameters);
                        }
                        else
                            img.Save(Filename, imgF);
                    }
                }
            }
        }
        catch
        {
            return false;
        }
        return true;
    }
    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
    protected void CreaAnteprima(string filePath, int Altezza, int Larghezza, string pathAnteprime, string nomeAnteprima)
    {
        string PathTempAnteprime = pathAnteprime;
        System.Drawing.Imaging.ImageFormat imgF = null;
        //System.IO.File.Exists(PathTempAnteprime);
        if (!System.IO.Directory.Exists(PathTempAnteprime))
        {
            System.IO.Directory.CreateDirectory(PathTempAnteprime);
        }
        // throw new Exception("Cartella temporanea di destinazione per l'anteprima non trovata!");

        using (System.IO.FileStream file = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
        {
            using (System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(file))
            {
                int altezzaStream = bmpStream.Height;
                int larghezzaStream = bmpStream.Width;
                if (altezzaStream <= larghezzaStream)
                    Altezza = Convert.ToInt32((double)Larghezza / (double)larghezzaStream * (double)altezzaStream);
                else
                    Larghezza = Convert.ToInt32((double)Altezza / (double)altezzaStream * (double)larghezzaStream);
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(Larghezza, Altezza));
                switch (System.IO.Path.GetExtension(filePath).ToLower())
                {
                    case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif; break;
                    case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; break;
                    case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; break;
                    default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                }

                if (imgF == System.Drawing.Imaging.ImageFormat.Jpeg)
                {

                    // Create an Encoder object based on the GUID for the Quality parameter category.
                    ImageCodecInfo jgpEncoder = GetEncoder(imgF); //ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == "image/jpeg");
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    // Create an EncoderParameters object.
                    // An EncoderParameters object has an array of EncoderParameter objects. In this case, there is only one EncoderParameter object in the array.
                    EncoderParameters myEncoderParameters = new EncoderParameters(3);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                    myEncoderParameters.Param[2] = new EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, (int)EncoderValue.RenderProgressive);

                    img.Save(PathTempAnteprime + nomeAnteprima, jgpEncoder, myEncoderParameters);
                }
                else
                    img.Save(PathTempAnteprime + nomeAnteprima, imgF);
            }
            file.Close();
        }
        if (!System.IO.File.Exists(PathTempAnteprime + nomeAnteprima))
            output.Text = ("Anteprima Allegato non salvata correttamente!");

    }
    protected string ComponiUrlAnteprima(object FotoColl, string id)
    {
        string url = "";

        if (FotoColl != null && ((AllegatiCollection)FotoColl).Count > 0 && ((AllegatiCollection)FotoColl)[0].NomeAnteprima != null)
            if (!((AllegatiCollection)FotoColl)[0].NomeAnteprima.ToLower().StartsWith("http://") && !((AllegatiCollection)FotoColl)[0].NomeAnteprima.ToLower().StartsWith("https://"))
                url = PercorsoFiles + "/" + TipologiaOfferte + "/" + id + "/" + ((AllegatiCollection)FotoColl)[0].NomeAnteprima;
            else
                url = ((AllegatiCollection)FotoColl)[0].NomeAnteprima;

        return url;
    }
    protected string ComponiUrlGalleriaAnteprima(string NomeAnteprima)
    {
        string url = "";
        url = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + NomeAnteprima;
        //PER I FILES CHE NON SONO IMMAGINI METTO UN'IMMAGINE FISSA
        if (!(NomeAnteprima.ToLower().EndsWith("jpg") || NomeAnteprima.ToLower().EndsWith("gif") || NomeAnteprima.ToLower().EndsWith("png")))
            url = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";
        return url;
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
        prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == TipologiaOfferte || TipologiaOfferte == "")); });
        if (!string.IsNullOrEmpty(SceltaTipologia))
            prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == SceltaTipologia || SceltaTipologia == "")); });

        prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        /*Carico Anche la ddl dell'inserzione del nuovo sottoprodotto*/
        ddlProdottoNewProd1.Items.Clear();
        ddlProdottoNewProd1.Items.Insert(0, references.ResMan("Common",Lingua,"selProdotti"));
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

        List<WelcomeLibrary.DOM.TipologiaOfferte> Tipologie = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate(WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I"); });

        ddlTipologiaNewProd.Items.Clear();
        ddlTipologiaNewProd.Items.Insert(0, references.ResMan("Common",Lingua, "selSProdotti"));
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
        prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == TipologiaOfferte || TipologiaOfferte == "")); });

        prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        /*Carico Anche la ddl dell'inserzione del nuovo sottoprodotto*/
        ddlProdottoNewProd.Items.Clear();
        ddlProdottoNewProd.Items.Insert(0, references.ResMan("Common",Lingua, "selProdotti"));
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

        sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate(WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceProdotto == ddlProdottoNewProd.SelectedValue)); });

        sprodotti.Sort(new GenericComparer<SProdotto>("CodiceSProdotto", System.ComponentModel.ListSortDirection.Ascending));
        ddlProdottoNewSProd.Items.Clear();
        ddlProdottoNewSProd.Items.Insert(0, references.ResMan("Common",Lingua, "selSProdotti"));
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



        List<WelcomeLibrary.DOM.TipologiaOfferte> Tipologie = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate(WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I"); });

        ddlTipologiaNewSottProd.Items.Clear();
        ddlTipologiaNewSottProd.Items.Insert(0, references.ResMan("Common",Lingua, "selSProdotti"));
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

        prodottoIta = Utility.ElencoProdotti.Find(delegate(WelcomeLibrary.DOM.Prodotto tmp_p) { return (tmp_p.Lingua == "I" && (tmp_p.CodiceProdotto == Prodotto)); });
        prodottoEng = Utility.ElencoProdotti.Find(delegate(WelcomeLibrary.DOM.Prodotto tmp_p) { return (tmp_p.Lingua == "GB" && (tmp_p.CodiceProdotto == Prodotto)); });

        //adesso che ho il mio prodotto, posso caricare i suoi valori per la modifica o l'inserimento
        if (prodottoIta != null)
            NomeNuovoProdIt.Text = prodottoIta.Descrizione;
        else
            NomeNuovoProdIt.Text = "";

        if (prodottoEng != null)
            NomeNuovoProdEng.Text = prodottoEng.Descrizione;
        else
            NomeNuovoProdEng.Text = "";

    }

    protected void CaricaDatiFormInserimentoSott(string Tipologia, string SottoProdotto)
    {
        //carico nei form di inserimento e modifica i valori relativi al prodotto
        WelcomeLibrary.DOM.SProdotto SprodottoIta = new WelcomeLibrary.DOM.SProdotto();
        WelcomeLibrary.DOM.SProdotto SprodottoEng = new WelcomeLibrary.DOM.SProdotto();
        SprodottoIta = Utility.ElencoSottoProdotti.Find(delegate(WelcomeLibrary.DOM.SProdotto tmp_p) { return (tmp_p.Lingua == "I" && (tmp_p.CodiceSProdotto == SottoProdotto)); });
        SprodottoEng = Utility.ElencoSottoProdotti.Find(delegate(WelcomeLibrary.DOM.SProdotto tmp_p) { return (tmp_p.Lingua == "GB" && (tmp_p.CodiceSProdotto == SottoProdotto)); });

        //adesso che ho il mio prodotto, posso caricare i suoi valori per la modifica o l'inseriment
        if (SprodottoIta != null)
            NomeNuovoSottIt.Text = SprodottoIta.Descrizione;
        else
            NomeNuovoSottIt.Text = "";
        if (SprodottoEng != null)
            NomeNuovoSottEng.Text = SprodottoEng.Descrizione;
        else
            NomeNuovoSottEng.Text = "";
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
                if (string.IsNullOrEmpty(NomeNuovoProdIt.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoProdEng.Text.Trim()))
                {
                    ErrorMsgNuovoProdotto.Text = "Inserire il nome categoria prodotto in italiano e inglese!";
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
            }
            else
            {
                if (btnModificaProd.Text == "Annulla")
                {
                    this.SvuotaDettaglioProd();
                    Utility.CaricaListaStaticaProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                    this.CaricaDatiDllProdotto(TipologiaOfferte, "");
                    //   CaricaDatiDdlRicercaRepeater("", "");
                    OkButton.Text = "Nuovo";
                    btnModificaProd.Enabled = false;
                    btnModificaProd.Text = "Modifica";
                    NomeNuovoProdIt.Enabled = false;
                    NomeNuovoProdEng.Enabled = false;
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
                NomeNuovoProdIt.Text = "";
                NomeNuovoProdIt.Enabled = true;
                NomeNuovoProdEng.Enabled = true;
                //btnModificaProd.Enabled = false; ;
            }
            else
            {
                if (OkButton.Text == "Inserisci")
                {

                    if (string.IsNullOrEmpty(NomeNuovoProdIt.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoProdEng.Text.Trim()))
                    {
                        ErrorMsgNuovoProdotto.Text = "Inserire il nome prodotto in italiano e inglese!";
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

                    this.SvuotaDettaglioProd();
                    this.SvuotaDettaglioSProd();
                    Utility.CaricaListaStaticaProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                    //  CaricaDatiDdlRicercaRepeater("", "");

                    NomeNuovoProdIt.Enabled = false;
                    NomeNuovoProdEng.Enabled = false;
                    OkButton.Text = "Nuovo";
                    this.CaricaDatiDllProdotto(TipologiaOfferte, "");
                    this.CaricaDatiDllSottoprodotto(TipologiaOfferte, "", "");
                    //  CaricaDatiDdlRicercaRepeater("", "");

                    //OfferteCollection list = offDM.CaricaOffertePerCodice( WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
                    //rptOfferte.DataSource = list;
                    //rptOfferte.DataBind();
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
                NomeNuovoSottIt.Text = "";
                NomeNuovoSottIt.Enabled = true;
                NomeNuovoSottEng.Enabled = true;
                //btnModificaSottoProd.Enabled = false; ;
            }
            else
            {
                if (OkButton2.Text == "Inserisci")
                {

                    //Faccio l'inserimento in italiano
                    updrecord = new SProdotto();
                    updrecord.CodiceProdotto = CodiceProdotto;

                    if (string.IsNullOrEmpty(NomeNuovoSottIt.Text.Trim()) || string.IsNullOrEmpty(NomeNuovoSottEng.Text.Trim()))
                    {
                        ErrorMessage.Text = "inserire descrizioni sottoprodotto in italiano e inglese";
                        return;
                    }
                    //updrecord.CodiceTipologia = TipologiaOfferte;
                    updrecord.Descrizione = NomeNuovoSottIt.Text;
                    updrecord.Lingua = "I";
                    //updrecord.CodiceProdotto = CodiceProdotto;
                    offDM.InsertSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    //offDM.UpdateProdotto( WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    //Faccio l'inserimento in inglese
                    updrecord = new SProdotto();
                    updrecord.CodiceProdotto = CodiceProdotto;
                    updrecord.Descrizione = NomeNuovoSottEng.Text;
                    updrecord.Lingua = "GB";
                    //updrecord.CodiceProdotto = CodiceProdotto;
                    offDM.InsertSottoProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
                    //offDM.UpdateProdotto( WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);

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
    }

    protected void SvuotaDettaglioSProd()
    {
        NomeNuovoSottIt.Text = "";
        NomeNuovoSottEng.Text = "";
    }
    protected void ddlProdottoNewProd1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Qui devo mettere una funzione che riempie i dati con il nome (ITA / ENG) del prodotto selezionato
        //Di modo da poterlo modificare
        CodiceProdotto = ddlProdottoNewProd1.SelectedValue;
        CaricaDatiFormInserimento(ddlTipologiaNewProd.SelectedValue, ddlProdottoNewProd1.SelectedValue);
        btnModificaProd.Enabled = true;
        NomeNuovoProdIt.Enabled = true;
        NomeNuovoProdEng.Enabled = true;
        //OkButton.Enabled = false;
        OkButton.Text = "Annulla";
        btnModificaProd.Text = "Salva";
    }

    protected void ddlProdottoNewProd_SelectedIndexChange(object sender, EventArgs e)
    {
        CodiceProdotto = ddlProdottoNewProd.SelectedValue;
        CaricaDatiDllSottoprodotto(ddlTipologiaNewSottProd.SelectedValue, ddlProdottoNewProd.SelectedValue, "");
        CaricaDatiFormInserimentoSott(ddlTipologiaNewProd.SelectedValue, ddlProdottoNewSProd.SelectedValue);

    }
    protected void ddlProdottoNewSProd_SelectedIndexChange(object sender, EventArgs e)
    {
        //Qui devo mettere una funzione che riempie i dati con il nome (ITA / ENG) del prodotto selezionato per modo da poterlo modificare
        CodiceProdotto = ddlProdottoNewProd.SelectedValue;
        CaricaDatiFormInserimentoSott(ddlTipologiaNewProd.SelectedValue, ddlProdottoNewSProd.SelectedValue);
        btnModificaSottoProd.Enabled = true;
        NomeNuovoSottIt.Enabled = true;
        NomeNuovoSottEng.Enabled = true;
        OkButton2.Text = "Annulla";
        btnModificaSottoProd.Text = "Salva";
    }
    protected void TipologiaProd_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Permette il cambio della tipologia di offerte OCCHIO!!!!
        TipologiaOfferte = ddlTipologiaNewProd.SelectedValue;
        CaricaDatiDdlRicerca("", "", "", "", "");
    }
    #endregion

    #region GESTIONE CARATTERISTICHE DI RICERCA


    private void CaricaDatiDdlCaratteristiche(long p1, long p2, long p3, long p4, long p5, long p6)
    {

        //Riempio la ddl 
        //List<Tabrif> Car1 = Utility.Caratteristiche[0].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
        //ddlCaratteristica1.Items.Clear();
        //ddlCaratteristica1.Items.Insert(0, "Seleziona car1");
        //ddlCaratteristica1.Items[0].Value = "0";
        //ddlCaratteristica1.DataSource = Car1;
        //ddlCaratteristica1.DataTextField = "Campo1";
        //ddlCaratteristica1.DataValueField = "Codice";
        //ddlCaratteristica1.DataBind();
        //try
        //{
        //    ddlCaratteristica1.SelectedValue = p1.ToString();
        //}
        //catch { }

        //Riempio la ddl tipi clienti
        //ddlCaratteristica1_gest.Items.Clear();
        //ddlCaratteristica1_gest.Items.Insert(0, "Seleziona car1");
        //ddlCaratteristica1_gest.Items[0].Value = "0";
        //ddlCaratteristica1_gest.DataSource = Car1;
        //ddlCaratteristica1_gest.DataTextField = "Campo1";
        //ddlCaratteristica1_gest.DataValueField = "Codice";
        //ddlCaratteristica1_gest.DataBind();
        //try
        //{
        //    ddlCaratteristica1_gest.SelectedValue = "0";
        //}
        //catch { }


        ////Riempio la ddl  
        //List<Tabrif> Car2 = Utility.Caratteristiche[1].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
        //ddlCaratteristica2.Items.Clear();
        //ddlCaratteristica2.Items.Insert(0, "Seleziona car2");
        //ddlCaratteristica2.Items[0].Value = "0";
        //ddlCaratteristica2.DataSource = Car2;
        //ddlCaratteristica2.DataTextField = "Campo1";
        //ddlCaratteristica2.DataValueField = "Codice";
        //ddlCaratteristica2.DataBind();
        //try
        //{
        //    ddlCaratteristica2.SelectedValue = p2.ToString();
        //}
        //catch { }

        //Riempio la ddl tipi clienti
        //Car2 = Utility.Caratteristiche[1].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
        //ddlCaratteristica2_gest.Items.Clear();
        //ddlCaratteristica2_gest.Items.Insert(0, "Seleziona car2");
        //ddlCaratteristica2_gest.Items[0].Value = "0";
        //ddlCaratteristica2_gest.DataSource = Car2;
        //ddlCaratteristica2_gest.DataTextField = "Campo1";
        //ddlCaratteristica2_gest.DataValueField = "Codice";
        //ddlCaratteristica2_gest.DataBind();
        //try
        //{
        //    ddlCaratteristica2_gest.SelectedValue = "0";
        //}
        //catch { }


        //Riempio la ddl  
        List<Tabrif> Car3 = Utility.Caratteristiche[2].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
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

        //Riempio la ddl  
        Car3 = Utility.Caratteristiche[2].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
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


        ////Riempio la ddl  
        //List<Tabrif> Car4 = Utility.Caratteristiche[3].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
        //ddlCaratteristica4.Items.Clear();
        //ddlCaratteristica4.Items.Insert(0, "Seleziona car4");
        //ddlCaratteristica4.Items[0].Value = "0";
        //ddlCaratteristica4.DataSource = Car4;
        //ddlCaratteristica4.DataTextField = "Campo1";
        //ddlCaratteristica4.DataValueField = "Codice";
        //ddlCaratteristica4.DataBind();
        //try
        //{
        //    ddlCaratteristica4.SelectedValue = p4.ToString();
        //}
        //catch { }
        //Riempio la ddl tipi clienti
        //Car4 = Utility.Caratteristiche[3].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
        //ddlCaratteristica4_gest.Items.Clear();
        //ddlCaratteristica4_gest.Items.Insert(0, "Seleziona car4");
        //ddlCaratteristica4_gest.Items[0].Value = "0";
        //ddlCaratteristica4_gest.DataSource = Car4;
        //ddlCaratteristica4_gest.DataTextField = "Campo1";
        //ddlCaratteristica4_gest.DataValueField = "Codice";
        //ddlCaratteristica4_gest.DataBind();
        //try
        //{
        //    ddlCaratteristica4_gest.SelectedValue = "0";
        //}
        //catch { }


        //Riempio la ddl  
        List<Tabrif> Car5 = Utility.Caratteristiche[4].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
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
        Car5 = Utility.Caratteristiche[4].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
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
        List<Tabrif> Car6 = Utility.Caratteristiche[5].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
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
        Car6 = Utility.Caratteristiche[5].FindAll(delegate(Tabrif _t) { return _t.Lingua == "I"; });
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
        }
        else
        {
            txtCar1I.Text = "";
            txtCar1GB.Text = "";
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

        txtCar1I.Text = "";
        txtCar1GB.Text = "";

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
        }
        else
        {
            txtCar2I.Text = "";
            txtCar2GB.Text = "";
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

        txtCar2I.Text = "";
        txtCar2GB.Text = "";

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
        }
        else
        {
            txtCar3I.Text = "";
            txtCar3GB.Text = "";
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

        txtCar3I.Text = "";
        txtCar3GB.Text = "";

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
        }
        else
        {
            txtCar4I.Text = "";
            txtCar4GB.Text = "";
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

        txtCar4I.Text = "";
        txtCar4GB.Text = "";

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
        }
        else
        {
            txtCar5I.Text = "";
            txtCar5GB.Text = "";
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

        txtCar5I.Text = "";
        txtCar5GB.Text = "";

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
        }
        else
        {
            txtCar6I.Text = "";
            txtCar6GB.Text = "";
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

        txtCar6I.Text = "";
        txtCar6GB.Text = "";

        //Aggiorno la visualizzazione
        WelcomeLibrary.UF.Utility.Caratteristiche[5] = WelcomeLibrary.UF.Utility.CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica6");
        WelcomeLibrary.UF.Utility.Caratteristiche[5].Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Tabrif>("Double1", System.ComponentModel.ListSortDirection.Ascending));
        CaricaDatiDdlCaratteristiche(0, 0, 0, 0, 0, 0);

        CaricaDati();

    }


    #endregion

    #region REGIONE GESTIONE LISTE GEOGRAFICHE 1 VERSIONE


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
        List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate(Province tmp) { return (tmp.Lingua == "I"); });
        if (provincelingua != null)
        {
            provincelingua.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
            foreach (Province item in provincelingua)
            {
                if (item.Lingua == "I")
                    if (!regioni.Exists(delegate(Province tmp) { return (tmp.Regione == item.Regione); }))
                        regioni.Add(item);
            }
        }
        //regioni.Sort(new GenericComparer<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending));
        ddlRegione.Items.Clear();
        ddlRegione.Items.Insert(0, references.ResMan("Common",Lingua, "ddlTuttiregione"));
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
        ddlProvincia.Items.Insert(0, references.ResMan("Common",Lingua, "ddlTuttiprovincia"));
        ddlProvincia.Items[0].Value = "";
        if (Regione != "")
        {
            provincelingua = null;
            Province _tmp = Utility.ElencoProvince.Find(delegate(Province tmp) { return (tmp.Lingua == "I" && tmp.Codice == ddlRegione.SelectedValue); });
            if (_tmp != null)
            {
                provincelingua = Utility.ElencoProvince.FindAll(delegate(Province tmp) { return (tmp.Lingua == "I" && tmp.CodiceRegione == _tmp.CodiceRegione); });
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
        ddlComune.Items.Insert(0, references.ResMan("Common",Lingua, "ddlTuttiComune"));
        ddlComune.Items[0].Value = "";
        if (Provincia != "")
        {
            List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate(WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == Provincia); });
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
        prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == TipologiaOfferte || TipologiaOfferte == "")); });
        if (!string.IsNullOrEmpty(SceltaTipologia))
        {
            prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceTipologia == SceltaTipologia || SceltaTipologia == "")); });
        }
        prodotti.Sort(new GenericComparer<Prodotto>("CodiceProdotto", System.ComponentModel.ListSortDirection.Ascending));
        ddlProdotto.Items.Clear();
        ddlProdotto.Items.Insert(0, references.ResMan("Common",Lingua, "selProdotti"));
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
        sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate(WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == "I" && (tmp.CodiceProdotto == ddlProdotto.SelectedValue)); });
        sprodotti.Sort(new GenericComparer<SProdotto>("CodiceSProdotto", System.ComponentModel.ListSortDirection.Ascending));
        ddlSottoProdotto.Items.Clear();
        ddlSottoProdotto.Items.Insert(0, references.ResMan("Common",Lingua, "selProdotti"));
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

    #endregion

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
        List<Tabrif> nazioni = Utility.Nazioni.FindAll(delegate(Tabrif _nz) { return _nz.Lingua == "I"; });
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
            List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate(Province tmp) { return (tmp.SiglaNazione.ToLower() == dnaz.SelectedValue.ToLower()); });
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
                    if (!regioni.Exists(delegate(Province tmp) { return (tmp.Regione == p.Regione); }))
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
            Province _tmp = Utility.ElencoProvince.Find(delegate(Province tmp) { return (tmp.Lingua == "I" && tmp.Codice == dreg.SelectedValue); });
            if (_tmp != null)
            {
                List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate(Province tmp) { return (tmp.Lingua == "I" && tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == dnaz.SelectedValue.ToLower()); });
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
            List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate(WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == dpro.SelectedValue); });
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

    protected void ddlCodiceNAZIONE2_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(value, "", "", "", ddlCodiceNAZIONE2_dts, ddlCodiceREGIONE2_dts, ddlCodicePROVINCIA2_dts, ddlCodiceCOMUNE2_dts, txtCodiceREGIONE2_dts, txtCodicePROVINCIA2_dts, txtCodiceCOMUNE2_dts);
    }
    protected void ddlCodiceREGIONE2_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE2_dts.SelectedValue, value, "", "", ddlCodiceNAZIONE2_dts, ddlCodiceREGIONE2_dts, ddlCodicePROVINCIA2_dts, ddlCodiceCOMUNE2_dts, txtCodiceREGIONE2_dts, txtCodicePROVINCIA2_dts, txtCodiceCOMUNE2_dts);
    }
    protected void ddlCodicePROVINCIA2_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE2_dts.SelectedValue, ddlCodiceREGIONE2_dts.SelectedValue, value, "", ddlCodiceNAZIONE2_dts, ddlCodiceREGIONE2_dts, ddlCodicePROVINCIA2_dts, ddlCodiceCOMUNE2_dts, txtCodiceREGIONE2_dts, txtCodicePROVINCIA2_dts, txtCodiceCOMUNE2_dts);
    }
    protected void ddlCodiceCOMUNE2_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE2_dts.SelectedValue, ddlCodiceREGIONE2_dts.SelectedValue, ddlCodicePROVINCIA2_dts.SelectedValue, value, ddlCodiceNAZIONE2_dts, ddlCodiceREGIONE2_dts, ddlCodicePROVINCIA2_dts, ddlCodiceCOMUNE2_dts, txtCodiceREGIONE2_dts, txtCodicePROVINCIA2_dts, txtCodiceCOMUNE2_dts);
    }

    protected void ddlCodiceNAZIONE3_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(value, "", "", "", ddlCodiceNAZIONE3_dts, ddlCodiceREGIONE3_dts, ddlCodicePROVINCIA3_dts, ddlCodiceCOMUNE3_dts, txtCodiceREGIONE3_dts, txtCodicePROVINCIA3_dts, txtCodiceCOMUNE3_dts);
    }
    protected void ddlCodiceREGIONE3_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE3_dts.SelectedValue, value, "", "", ddlCodiceNAZIONE3_dts, ddlCodiceREGIONE3_dts, ddlCodicePROVINCIA3_dts, ddlCodiceCOMUNE3_dts, txtCodiceREGIONE3_dts, txtCodicePROVINCIA3_dts, txtCodiceCOMUNE3_dts);
    }
    protected void ddlCodicePROVINCIA3_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE3_dts.SelectedValue, ddlCodiceREGIONE3_dts.SelectedValue, value, "", ddlCodiceNAZIONE3_dts, ddlCodiceREGIONE3_dts, ddlCodicePROVINCIA3_dts, ddlCodiceCOMUNE3_dts, txtCodiceREGIONE3_dts, txtCodicePROVINCIA3_dts, txtCodiceCOMUNE3_dts);
    }
    protected void ddlCodiceCOMUNE3_dts_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlCodiceNAZIONE3_dts.SelectedValue, ddlCodiceREGIONE3_dts.SelectedValue, ddlCodicePROVINCIA3_dts.SelectedValue, value, ddlCodiceNAZIONE3_dts, ddlCodiceREGIONE3_dts, ddlCodicePROVINCIA3_dts, ddlCodiceCOMUNE3_dts, txtCodiceREGIONE3_dts, txtCodicePROVINCIA3_dts, txtCodiceCOMUNE3_dts);
    }

    //PER I FILTRI DI RICERCA
    protected void ddlNazioneRicerca_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(value, "", "", "", ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
    }
    protected void ddlRegioneRicerca_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlNazioneRicerca.SelectedValue, value, "", "", ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
    }
    protected void ddlProvinciaRicerca_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlNazioneRicerca.SelectedValue, ddlRegioneRicerca.SelectedValue, value, "", ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
    }
    protected void ddlComuneRicerca_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlNazioneRicerca.SelectedValue, ddlRegioneRicerca.SelectedValue, ddlProvinciaRicerca.SelectedValue, value, ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
    }

    #endregion

    #region GESTIONE UTENTI MEMBERSHIP
    protected bool EliminaUtenteAssociato(Offerte socio)
    {
        bool esito = true;
        try
        {
            Membership.DeleteUser(socio.Cognome_dts.Replace(" ", "").Trim().ToLower() + socio.Nome_dts.Replace(" ", "").Trim().ToLower());
        }
        catch (Exception error)
        {
            output.Text += error.Message;
            if (error.InnerException != null)
                output.Text += error.InnerException.Message.ToString();
            esito = false;
        }
        return esito;
    }
    protected void btnEliminaUtente_Click(object sender, EventArgs e)
    {

        Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);
        if (Details != null)
            if (EliminaUtenteAssociato(Details))
                lblResultsPsw.Text = "Utente Eliminato!";
    }
    protected bool VerificaPresenzaUtente(string username)
    {
        bool ret = false;
        MembershipUserCollection mucoll = Membership.FindUsersByName(username);
        if (mucoll != null && mucoll.Count > 0)
        {
            ret = true;
        }
        return ret;
    }
    protected void btnCreaUtente_Click(object sender, EventArgs e)
    {
        string NomeUtente = "";
        string password = "";
        Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);
        if (Details != null)
            if (CreaUtenteAssociato(Details, ref  NomeUtente, ref password))
                lblResultsPsw.Text = "Utente Creato! Nomeutente: " + NomeUtente + " Password: " + password;
            else
                lblResultsPsw.Text = "Utente NON Creato! ";
    }
    protected bool CreaUtenteAssociato(Offerte socio, ref string NomeUtente, ref string password)
    {
        bool esito = true;
        try
        {
            if (socio != null && !string.IsNullOrWhiteSpace(socio.Cognome_dts.Replace(" ", "").Trim().ToLower() + socio.Nome_dts.Replace(" ", "").Trim().ToLower()) && socio.Id != 0)
            {
                //Generiamo la password di accesso
                //password = Membership.GeneratePassword(6, 0);
                password = WelcomeLibrary.UF.RandomPassword.Generate(8);

                MembershipUserCollection mucoll = Membership.FindUsersByName(socio.Cognome_dts.Replace(" ", "").Trim().ToLower() + socio.Nome_dts.Replace(" ", "").Trim().ToLower());
                if (mucoll == null || mucoll.Count == 0)
                {

                    //Creiamo l'utente ( username = password riservata )
                    //NomeUtente = socio.Emailriservata_dts;
                    NomeUtente = socio.Cognome_dts.Replace(" ", "").Trim().ToLower() + socio.Nome_dts.Replace(" ", "").Trim().ToLower();
                    Membership.CreateUser(NomeUtente, password);

                    //associamo l'utente al ruolo
                    Roles.AddUserToRole(NomeUtente, "Socio");

                    //ProfileCommon prof = (ProfileCommon)ProfileCommon.Create(NomeUtente);
                    ProfileBase prof = ProfileBase.Create(NomeUtente);
                    prof["IdSocio"] = socio.Id.ToString();
                    prof.Save();

                    ////Stampo a video i dati dell'username e password Appena Creati
                    output.Text += "User: " + NomeUtente + " Psw: " + password;
                    //Username.Visible = true;
                    //Username.Text = NomeUtente;
                    //Password.Visible = true;
                    //Password.Text = password;
                }
                else
                {
                    output.Text += "Utente non creato già presente in archivio.";
                    esito = false;
                }
            }
        }
        catch (Exception error)
        {
            output.Text += error.Message;
            if (error.InnerException != null)
                output.Text += error.InnerException.Message.ToString();
            esito = false;
        }
        return esito;
    }

    protected bool ControllaRuolo(string username, string verificaruolo)
    {
        bool flag = false;
        foreach (string role in Roles.GetRolesForUser(username))
        {
            if (role.ToString() == verificaruolo) flag = true;
        }
        return flag;
    }

    protected void Cambiopass_Click(object sender, EventArgs e)
    {
        try
        {
            Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);
            if (Details != null)
            {

                MembershipUser utente = Membership.GetUser(Details.Cognome_dts.Replace(" ", "").Trim().ToLower() + Details.Nome_dts.Replace(" ", "").Trim().ToLower(), false);
                if (utente != null)
                    if (utente.ChangePassword(txtPasswordold.Text, txtPasswordnew.Text))
                        lblResultsPsw.Text = "Password Cambiata";
                    else
                        lblResultsPsw.Text = "Errore cambio password";
            }
        }
        catch (Exception errore)
        {
            lblResultsPsw.Text = errore.Message;
        }

    }
    protected void Resetpass_Click(object sender, EventArgs e)
    {
        try
        {
            Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, OffertaIDSelected);
            if (Details != null)
            {

                MembershipUser utente = Membership.GetUser(Details.Cognome_dts.Replace(" ", "").Trim().ToLower() + Details.Nome_dts.Replace(" ", "").Trim().ToLower(), false);
                string passimpostata = utente.ResetPassword();
                lblResultsPsw.Text = "La nuova password automatica è:  " + passimpostata + "      Copiare la password da qualche parte!!";
                lblquestion.Text = "Attenzione !! Premendo il pulsante Reset password viene generata una nuova password per l'utente, che invaliderà la precedente.";

                //Procedura con requires question and aswer
#if false
        if (txtanswer.Text != "")
        {
            string passimpostata = utente.ResetPassword(txtanswer.Text);
            lblResultsPsw.Text = "La nuova password automatica è:  " + passimpostata + " . Copiare la password da qualche parte!!";
        }
        else
        {
            lblquestion.Text = "Digita nella casella la riposta corretta al seguente quesito: " +  utente.PasswordQuestion + "?. Poi premi reset per resettare la password. ";
            lblResultsPsw.Text = "Password non resettata.  ";
        }
#endif
            }
        }
        catch (Exception errore)
        {
            lblResultsPsw.Text = errore.Message;
        }
    }
    #endregion

    protected void btnStampa_Click(object sender, EventArgs e)
    {
        //NOME E COGNOME 
        //INDIRIZZO SPEDIZIONE RICEVUTA
        //CODICE FISCALE
        //EMAIL
        System.Text.StringBuilder sb = new StringBuilder();
        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
        if (TipologiaOfferte != "")
        {
            SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", TipologiaOfferte);
            parColl.Add(p3);
        }
        bool _statoblocco = false;
        SQLiteParameter pstatoblocco = new SQLiteParameter("@Bloccoaccesso_dts", _statoblocco);
        parColl.Add(pstatoblocco);
        OfferteCollection offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", null, null, "Cognome_dts", false);

        sb.Append("Totale Soci filtrati ( non archiviati, non bloccati ): " + offerte.Count + " <br/>");

        sb.Append(" <table style=\"padding:10px;border:solid 1px #000000\" class=\"table table-order table-stripped\"><tr>");
        sb.Append("<thead>");
        sb.Append("<tr style=\"border-bottom:1px solid #000000;background-color:#000000\">");
        sb.Append("<td style=\"border-right:1px solid #000000;color:#ffffff\">Progr</td>");
        sb.Append("<td style=\"border-right:1px solid #000000;color:#ffffff\">Cognome</td>");
        sb.Append("<td style=\"border-right:1px solid #000000;color:#ffffff\">Nome</td>");
        sb.Append("<td style=\"border-right:1px solid #000000;color:#ffffff\">Ind. Fattura</td>");
        sb.Append("<td style=\"border-right:1px solid #000000;color:#ffffff\">Ind. Privato</td>");
        sb.Append("<td style=\"border-right:1px solid #000000;color:#ffffff\">Codice Fiscale</td>");
        sb.Append("<td style=\"border-right:1px solid #000000;color:#ffffff\">Mail R</td>");
        sb.Append("<td style=\"border-right:1px solid #000000;color:#ffffff\">Mail P</td>");
        sb.Append("</tr>");
        sb.Append("</thead>");
        int i = 0;
        if (offerte != null)
            foreach (Offerte t in offerte)
            {
                i++;
                sb.Append("<tr>");
                //sb.Append("<td style=\"border-right:1px solid #000000\">" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", t.Dataordine) + "</td>");
                sb.Append("<td style=\"border-right:1px solid #000000\">" + i + "</td>");
                sb.Append("<td style=\"border-right:1px solid #000000\">" + t.Cognome_dts + "</td>");
                sb.Append("<td style=\"border-right:1px solid #000000\">" + t.Nome_dts + "</td>");
                sb.Append("<td style=\"border-right:1px solid #000000\">" + t.indirizzofatt_dts + "</td>");
                sb.Append("<td style=\"border-right:1px solid #000000\">" + t.Indirizzo + "</td>");
                sb.Append("<td style=\"border-right:1px solid #000000\">" + t.Pivacf_dts + "</td>");
                sb.Append("<td style=\"border-right:1px solid #000000\">" + t.Emailriservata_dts + "</td>");
                sb.Append("<td style=\"border-right:1px solid #000000\">" + t.Email + "</td>");
                //sb.Append("<td style=\"border-right:1px solid #000000\">" + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                //      new object[] { t.TotaleSmaltimento + t.TotaleOrdine + t.TotaleSpedizione - t.TotaleSconto} ) + " €" + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td colspan=\"6\" style=\"border-bottom:1px solid #000000\"></td></tr>");
            }

        sb.Append("</table>");
        Session.Add("datistampa", sb.ToString());
    }
}
