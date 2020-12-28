using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Data.SQLite;
using System.Text;
using WelcomeLibrary.UF;
using Newtonsoft.Json;

public partial class AreaContenuti_StoricoOrdini_New : CommonPage
{
    public string PageGuid
    {
        get { return ViewState["PageGuid"] != null ? (string)(ViewState["PageGuid"]) : ""; }
        set { ViewState["PageGuid"] = value; }
    }
    public string Pagina
    {
        get { return Session["Pagina"] != null ? (string)(Session["Pagina"]) : "1"; }
        set { Session["Pagina"] = value; }
    }
    public string id_cliente
    {
        get { return Session["id_cliente"] != null ? (string)(Session["id_cliente"]) : ""; }
        set { Session["id_cliente"] = value; }
    }
    public string id_commerciale
    {
        get { return Session["id_commerciale"] != null ? (string)(Session["id_commerciale"]) : ""; }
        set { Session["id_commerciale"] = value; }
    }
    public string id_scaglione
    {
        get { return Session["id_scaglione"] != null ? (string)(Session["id_scaglione"]) : ""; }
        set { Session["id_scaglione"] = value; }
    }
    public string id_attivita
    {
        get { return Session["id_attivita"] != null ? (string)(Session["id_attivita"]) : ""; }
        set { Session["id_attivita"] = value; }
    }


    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : CommonPage.deflanguage; }
        set { ViewState["Lingua"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetCulture("it"); //forzo la cultura italia
        if (!IsPostBack)
        {
            AutoCompleteExtender1.ContextKey = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            AutoCompleteExtender2.ContextKey = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            PageGuid = System.Guid.NewGuid().ToString();
            id_cliente = CaricaValoreMaster(Request, Session, "id_cliente", true, "");
            id_commerciale = CaricaValoreMaster(Request, Session, "id_commerciale", true, "");
            id_scaglione = CaricaValoreMaster(Request, Session, "id_scaglione", true, "");
            id_attivita = CaricaValoreMaster(Request, Session, "id_attivita", true, "");
            txtidprodotto.Value = id_attivita;
            txtidscaglione.Value = id_scaglione;
              
            hididcommerciale.Value = id_commerciale;
            hididcliente.Value = id_cliente;
            txtCLIENTE.Text = id_cliente;

            CommonPage CommonPage = new CommonPage();

            //Lingua = CommonPage.CaricaValoreMaster(Request, Session, "Lingua", false, "I");
            Lingua = "I";

            ImpostaVisualizzazione();


            CaricaOrdini();
        }
        else output.Text = "";
    }
    public string InjectedStartPageScripts()
    {
        Dictionary<string, string> addelements = new Dictionary<string, string>();
        String scriptRegVariables = "";
        //Preparazione dei modelli per vue vuoti in pagina
        scriptRegVariables += ";\r\n " + string.Format("var initpagemodelclienti = {0}",
         JsonConvert.SerializeObject(new initpagemodelclienti(), Formatting.Indented, new JsonSerializerSettings()
         {
             NullValueHandling = NullValueHandling.Ignore,
             MissingMemberHandling = MissingMemberHandling.Ignore,
             ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
             PreserveReferencesHandling = PreserveReferencesHandling.None,
         }));
        scriptRegVariables += ";\r\n " + string.Format("var clientivuemodel = {0}",
                JsonConvert.SerializeObject(new clientivuemodel(), Formatting.Indented, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                }));
        addelements.Add("jsvarfrommasterstart", scriptRegVariables);
        string ret = WelcomeLibrary.UF.custombind.CreaInitStringJavascriptOnly(addelements);
        return ret;
    }
    private void ImpostaVisualizzazione()
    {
        /////////////////////////////////////////////////////////////////////
        //Verifichiamo accesso socio e impostiamo la visualizzazione corretta
        //Spegnendo le cose che non devono essere visibili ai soci!!!
        /////////////////////////////////////////////////////////////////////
        usermanager USM = new usermanager();
        if (USM.ControllaRuolo(User.Identity.Name, "Commerciale"))
        {
            string idcliente = getidcliente(User.Identity.Name);
            if (!string.IsNullOrEmpty(idcliente))
            {
                id_commerciale = idcliente;
                txtCommerciale.Text = idcliente;
                hididcommerciale.Value = id_commerciale;
                txtCommerciale.Enabled = false;
                txtCommerciale.ReadOnly = true;
                ((HtmlGenericControl)Master.FindControl("ulMainbar")).Visible = false; //Spengo la barra navigazione
            }
            else
            {
                Response.Redirect("~/Error.aspx?Error=Utente non trovato");
            }
        }
        if (USM.ControllaRuolo(User.Identity.Name, "Distributore"))
        {
            string idcliente = getidcliente(User.Identity.Name);
            if (!string.IsNullOrEmpty(idcliente))
            {
                //txtCLIENTE.Text = "";
                id_cliente = idcliente;
                hididcliente.Value = id_cliente;
                txtCLIENTE.Text = id_cliente;
                txtCLIENTE.Enabled = false;
                txtCLIENTE.ReadOnly = true;
                txtCommerciale.Enabled = false;
                txtCommerciale.ReadOnly = true;
                ((HtmlGenericControl)Master.FindControl("ulMainbar")).Visible = false; //Spengo la barra navigazione
            }
            else
            {
                Response.Redirect("~/Error.aspx?Error=Utente non trovato");
            }
        }

        if (USM.ControllaRuolo(User.Identity.Name, "Operatore"))
        {
            string idcliente = getidcliente(User.Identity.Name);
            if (!string.IsNullOrEmpty(idcliente))
            {
                id_cliente = idcliente;
                hididcliente.Value = id_cliente;
                txtCLIENTE.Text = id_cliente;
                txtCLIENTE.Enabled = false;
                txtCLIENTE.ReadOnly = true;
                txtCommerciale.Enabled = false;
            }
            else
            {
                Response.Redirect("~/Error.aspx?Error=Utente non trovato");
            }
        }
    }

    protected bool BloccaSuRuoli(string roleslist)
    {
        bool ret = true;

        List<string> listaarray = roleslist.Trim().Split(',').ToList();
        foreach (string s in  listaarray)
        {
            usermanager USM = new usermanager();
            if (USM.ControllaRuolo(User.Identity.Name, s))
            {
                ret = false;
            }
        }
        return ret;
    }

    protected static string TipopagaDisplay(object item)
    {
        if (item == null) return "";
        TotaliCarrello i = (TotaliCarrello)item;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(references.ResMan("Common", "I", "txt" + i.Modalitapagamento).ToString());
        return sb.ToString();
    }



    protected void chkPagatoacconto_CheckedChanged(object sender, EventArgs e)
    {
        string codiceordine = ((CheckBox)sender).ToolTip;
        eCommerceDM eDM = new eCommerceDM();
        TotaliCarrello c = eDM.CaricaOrdinePerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);
        if (c != null)
        {
            c.Pagatoacconto = ((CheckBox)sender).Checked;

            ////////////////////////////////////////////////////
            //TESTO eventuale overbooking per gli scaglioni  -> SE NON OVERBOOKING ALLORA SPUNTO IL PAGAMENTO
            //Se la spunta viene tolta o ho gia pagato un saldo -> permetto di aggionare
            ///////////////////////////////////////////////////
            if (c.Pagato || !c.Pagatoacconto || !eDM.VerificaSuperamentoSuOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c.CodiceOrdine))
            {
                eDM.UpdateOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c);

                ////////////////////////////////////////
                //Aggiorniamo lo stato degli scaglioni
                ////////////////////////////////////////
                CarrelloCollection righeordine = eDM.CaricaCarrelloPerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c.CodiceOrdine);
                string listcod = "";
                righeordine.ForEach(rigo => listcod += rigo.id_prodotto + ",");
                eCommerceDM ecDM = new eCommerceDM();
                Dictionary<string, string> parametri = new Dictionary<string, string>();
                parametri["idprodotto"] = listcod;
                ecDM.AggiornaStatoscaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parametri);
                ////////////////////////////////////////
            }
            else
            {
                output.Text = "Attenzione! Superato limte massimo Iscrizioni, impossibile aggiornare, verificare nella scheda!";
            }
        }
        CaricaOrdini();
    }
    protected static bool StatusCheckAcconto(object item)
    {
        if (item == null) return false;
        TotaliCarrello i = (TotaliCarrello)item;
        return i.Pagatoacconto;
    }

    protected static string StatusCheckAccontoHtml(object item)
    {
        if (item == null) return "";
        TotaliCarrello i = (TotaliCarrello)item;
        return i.Pagatoacconto ? "checked" : "";
    }
    protected string StatusDisplayAcconto(object item)
    {
        if (item == null) return "";
        TotaliCarrello i = (TotaliCarrello)item;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<span class=\"");
        if (!i.Pagatoacconto)
            sb.Append("class=\"text-success\"");
        else
            sb.Append("class=\"text-warning\"");
        //sb.Append("class=\"text-error\"");

        sb.Append("\">");

        if (!i.Pagatoacconto)
            sb.Append(references.ResMan("Common", Lingua, "txtOrdinenonpagato").ToString());
        else
            sb.Append(references.ResMan("Common", Lingua, "txtOrdinepagato").ToString());
        //    txtOrdineannullato

        sb.Append("</span>");

        return sb.ToString();
    }




    protected void chkPagato_CheckedChanged(object sender, EventArgs e)
    {
        string codiceordine = ((CheckBox)sender).ToolTip;
        eCommerceDM eDM = new eCommerceDM();
        TotaliCarrello c = eDM.CaricaOrdinePerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);
        if (c != null)
        {
            c.Pagato = ((CheckBox)sender).Checked;

            ////////////////////////////////////////////////////
            //TESTO eventuale overbooking per gli scaglioni  -> SE NON OVERBOOKING ALLORA SPUNTO IL PAGAMENTO
            //Se la spunta viene tolta o ho gia pagato un acconto non zero -> permetto di aggionare
            ///////////////////////////////////////////////////
            if ((c.Pagatoacconto && c.TotaleAcconto != 0) || !c.Pagato || !eDM.VerificaSuperamentoSuOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c.CodiceOrdine))
            {

                eDM.UpdateOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c);
                ////////////////////////////////////////
                //Aggiorniamo lo stato degli scaglioni
                ////////////////////////////////////////
                CarrelloCollection righeordine = eDM.CaricaCarrelloPerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c.CodiceOrdine);
                string listcod = "";
                righeordine.ForEach(rigo => listcod += rigo.id_prodotto + ",");
                eCommerceDM ecDM = new eCommerceDM();
                Dictionary<string, string> parametri = new Dictionary<string, string>();
                parametri["idprodotto"] = listcod;
                ecDM.AggiornaStatoscaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parametri);
                ////////////////////////////////////////
            }
            else
            {
                output.Text = "Attenzione! Superato limte massimo Iscrizioni, impossibile aggiornare, verificare nella scheda!";
            }
        }

        CaricaOrdini();
    }
    protected static bool StatusCheck(object item)
    {
        if (item == null) return false;
        TotaliCarrello i = (TotaliCarrello)item;
        return i.Pagato;
    }
    protected string StatusDisplay(object item)
    {
        if (item == null) return "";
        TotaliCarrello i = (TotaliCarrello)item;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<span class=\"");
        if (!i.Pagato)
            sb.Append("class=\"text-success\"");
        else
            sb.Append("class=\"text-warning\"");
        //sb.Append("class=\"text-error\"");

        sb.Append("\">");

        if (!i.Pagato)
            sb.Append(references.ResMan("Common", Lingua, "txtOrdinenonpagato").ToString());
        else
            sb.Append(references.ResMan("Common", Lingua, "txtOrdinepagato").ToString());
        //    txtOrdineannullato

        sb.Append("</span>");

        return sb.ToString();
    }




    [System.Web.Services.WebMethod]
    public static string GetCurrentCarrello(string codice, string username = "")
    {
        return VisualizzaCarrello(codice, false, true, username);
    }
    public static string VisualizzaCarrello(string codiceordine, bool nofoto = false, bool perstampa = true, string username = "")
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(CommonPage.VisualizzaCarrello(null, null, codiceordine, nofoto, "I", false, perstampa, username));
        return sb.ToString();
    }
    protected static string TestoSezione(string codicetipologia)
    {
        string ret = "";
        WelcomeLibrary.DOM.TipologiaOfferte sezione =
              WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I" && tmp.Codice == codicetipologia); });
        if (sezione != null)
        {
            ret += " nella sezione:  \"" + CommonPage.ReplaceAbsoluteLinks(CommonPage.CrealinkElencotipologia(codicetipologia, "I")) + "\"";
        }
        return ret;
    }
    protected void CaricaOrdini()
    {
        string idforced = id_commerciale;
        if (string.IsNullOrEmpty(idforced))
            idforced = txtCommerciale.Text;
        //inseriamo nei  parametri di filtro  i filtri prodotto e scaglioni
        string idprodotto = txtidprodotto.Value; ;// hididprodotto.Value;// txtidprodotto.Text;
        string idscaglione = txtidscaglione.Value;

        //Stato pagamenti
        string statoacconto = radacconto.SelectedValue;
        string statosaldo = radsaldo.SelectedValue;


        //PREPARIAMO I DATI DI FILTRO
        Dictionary<string, string> parametri = new Dictionary<string, string>();
        parametri.Add("idcliente", txtCLIENTE.Text);
        parametri.Add("codiceordine", txtCodiceordine.Text);
        parametri.Add("datamin", txtdatamin.Text);
        parametri.Add("datamax", txtdatamax.Text);
        parametri.Add("idcommerciale", idforced);
        parametri.Add("idprodotto", idprodotto);
        parametri.Add("idscaglione", idscaglione);
        parametri.Add("statoacconto", statoacconto);
        parametri.Add("statosaldo", statosaldo);

        TotaliCarrelloCollection ordini = CaricaDatiOrdini(parametri, PagerRisultati.CurrentPage, PagerRisultati.PageSize);

        long nrecordfiltrati = ordini.Totrecs;
        PagerRisultati.TotalRecords = (long)ordini.Totrecs;
        if (nrecordfiltrati == 0) PagerRisultati.CurrentPage = 1;

        rtpOrdini.DataSource = ordini;
        rtpOrdini.DataBind();
#if false

        WelcomeLibrary.UF.Pager<TotaliCarrello> p = new WelcomeLibrary.UF.Pager<TotaliCarrello>(ordini, true, this.Page, PageGuid + PagerRisultati.ClientID);
        PagerRisultati.TotalRecords = p.Count;
        if (PagerRisultati.TotalRecords == 0) PagerRisultati.CurrentPage = 1;
        try
        {
            PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
        }
        catch
        {
            Pagina = "1";
        }
        rtpOrdini.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);

        rtpOrdini.DataBind();

#endif
        //if ((!string.IsNullOrEmpty(txtdatamin.Text) && !string.IsNullOrEmpty(txtdatamax.Text)) || !string.IsNullOrEmpty(txtCodiceordine.Text) || !string.IsNullOrEmpty(txtCLIENTE.Text)) PreparaStampa();
    }
    #region PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER

    protected void PagerRisultati_PageCommand(object sender, string PageNum)
    {
        UC_PagerEx PagerRisultati = sender as UC_PagerEx;
        PagerRisultati.CurrentPage = Convert.ToInt32(PageNum);
        Pagina = PageNum;
        CaricaOrdini();
    }

    protected void PagerRisultati_PageGroupClickNext(object sender, string spare)
    {
        //int pag = PagerRisultati.CurrentPage;
        //pag++;
        //if (pag > PagerRisultati.totalPages) pag = PagerRisultati.totalPages;
        //Pagina = pag.ToString();
        CaricaOrdini();
    }

    protected void PagerRisultati_PageGroupClickPrev(object sender, string spare)
    {
        //int pag = PagerRisultati.CurrentPage;
        //pag--;
        //if (pag < 1) pag = 1;
        //Pagina = pag.ToString();
        CaricaOrdini();

    }

    #endregion


    protected void btnFiltroordine_Click(object sender, EventArgs e)
    {
        CaricaOrdini();
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        string idforced = id_commerciale;
        if (string.IsNullOrEmpty(idforced))
            idforced = txtCommerciale.Text;
        //inseriamo nei  parametri di filtro  i filtri prodotto e scaglioni
        string idprodotto = txtidprodotto.Value;
        string idscaglione = txtidscaglione.Value;

        //Stato pagamenti
        string statoacconto = radacconto.SelectedValue;
        string statosaldo = radsaldo.SelectedValue;


        //PREPARIAMO I DATI DI FILTRO
        Dictionary<string, string> parametri = new Dictionary<string, string>();
        parametri.Add("idcliente", txtCLIENTE.Text);
        parametri.Add("codiceordine", txtCodiceordine.Text);
        parametri.Add("datamin", txtdatamin.Text);
        parametri.Add("datamax", txtdatamax.Text);
        parametri.Add("idcommerciale", idforced);
        parametri.Add("idprodotto", idprodotto);
        parametri.Add("idscaglione", idscaglione);
        parametri.Add("statoacconto", statoacconto);
        parametri.Add("statosaldo", statosaldo);

        TotaliCarrelloCollection listaordini = CaricaDatiOrdini(parametri);
        string csvName = "export-ordini-" + string.Format("{0:dd-MM-yyyy}", System.DateTime.Now) + ".csv";
        string pathFile = WelcomeLibrary.STATIC.Global.percorsoFisicoComune + "\\_temp\\";
        if (!System.IO.Directory.Exists(pathFile))
            System.IO.Directory.CreateDirectory(pathFile);
        eCommerceDM eDM = new eCommerceDM();
        string retmessage = eDM.ExportOrdersToCsv(pathFile, csvName, listaordini);
        WelcomeLibrary.UF.SharedStatic.DownloadFile(WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/" + csvName);
    }
    protected void btnExport1_Click(object sender, EventArgs e)
    {
        string idforced = id_commerciale;
        if (string.IsNullOrEmpty(idforced))
            idforced = txtCommerciale.Text;
        //inseriamo nei  parametri di filtro  i filtri prodotto e scaglioni
        string idprodotto = txtidprodotto.Value;
        string idscaglione = txtidscaglione.Value;

        //Stato pagamenti
        string statoacconto = radacconto.SelectedValue;
        string statosaldo = radsaldo.SelectedValue;


        //PREPARIAMO I DATI DI FILTRO
        Dictionary<string, string> parametri = new Dictionary<string, string>();
        parametri.Add("idcliente", txtCLIENTE.Text);
        parametri.Add("codiceordine", txtCodiceordine.Text);
        parametri.Add("datamin", txtdatamin.Text);
        parametri.Add("datamax", txtdatamax.Text);
        parametri.Add("idcommerciale", idforced);
        parametri.Add("idprodotto", idprodotto);
        parametri.Add("idscaglione", idscaglione);
        parametri.Add("statoacconto", statoacconto);
        parametri.Add("statosaldo", statosaldo);

        TotaliCarrelloCollection listaordini = CaricaDatiOrdini(parametri);
        string csvName = "export-ordini-" + string.Format("{0:dd-MM-yyyy}", System.DateTime.Now) + ".xlsx";
        string pathFile = WelcomeLibrary.STATIC.Global.percorsoFisicoComune + "\\_temp\\";
        if (!System.IO.Directory.Exists(pathFile))
            System.IO.Directory.CreateDirectory(pathFile);
        eCommerceDM eDM = new eCommerceDM();
        string retmessage = eDM.CreateExcelOrdini(pathFile, csvName, listaordini, references.refivacategorie);
        WelcomeLibrary.UF.SharedStatic.DownloadFile(WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/" + csvName);
    }

    //protected void btnStampa_Click(object sender, EventArgs e)
    //{
    //    PreparaStampa();
    //}

    [System.Web.Services.WebMethod]
    public static string PreparaStampa(string idcommerciale, string idcliente, string codiceordine, string datamin, string datamax, string idprodotto, string idscaglione, string statoacconto, string statosaldo)
    {

        string ret = "";
        System.Text.StringBuilder sb = new StringBuilder();
        sb.Append(" <table style=\"padding:10px;border:solid 1px #000000\" class=\"table table-order table-stripped\"><tr>");
        sb.Append("<thead>");
        sb.Append("<tr style=\"background-color:#ccc;color:#fff\">");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Date</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Order ID</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Id Cliente</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Mail</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Denominazione</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Totale Ordine</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Tipo Pagamento</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Comm.</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Pagato</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Dettagli</td>");
        sb.Append("</tr>");
        sb.Append("</thead>");

        //PREPARIAMO I DATI DI FILTRO
        Dictionary<string, string> parametri = new Dictionary<string, string>();
        parametri.Add("idcliente", idcliente);
        parametri.Add("codiceordine", codiceordine);
        parametri.Add("datamin", datamin);
        parametri.Add("datamax", datamax);
        parametri.Add("idcommerciale", idcommerciale);
        parametri.Add("idprodotto", idprodotto);
        parametri.Add("idscaglione", idscaglione);
        parametri.Add("statoacconto", statoacconto);
        parametri.Add("statosaldo", statosaldo);
        TotaliCarrelloCollection listaordini = CaricaDatiOrdini(parametri);

        if (listaordini != null)
            foreach (TotaliCarrello t in listaordini)
            {
                sb.Append("<tr>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", t.Dataordine) + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + t.CodiceOrdine + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + t.Id_cliente + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + t.Mailcliente + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + t.Denominazionecliente + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                       new object[] { t.TotaleSmaltimento + t.TotaleOrdine + t.TotaleSpedizione - t.TotaleSconto }) + " €" + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\" class=\"order-status\">" + TipopagaDisplay(t) + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\" class=\"order-status\">" + t.Id_commerciale + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\" class=\"order-status\">" + StatusCheck(t) + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">");

                sb.Append("<ul style=\"list-style:none;font-size:10px\">" + VisualizzaCarrello(t.CodiceOrdine, true) + "</ul>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td colspan=\"9\" style=\"border-bottom:1px solid #000000\"></td></tr>");
            }

        sb.Append("</table>");
        HttpContext.Current.Session.Add("datistampa", sb.ToString());
        return ret;
    }


}