using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

public partial class AspNetPages_Orderpage : CommonPage
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
                string registrazione = CaricaValoreMaster(Request, Session, "reg");
                //Carico la galleria in masterpage corretta
                //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "vuoto", false, Lingua);
                //HtmlGenericControl divmaster = (HtmlGenericControl)Master.FindControl("SezioneContenutiHome");
                //divmaster.Visible = false;
                //Literal lit = (Literal)Master.FindControl("litPortfolioLow");
                //Master.CaricaBannersPortfolio("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-low", false, lit, Lingua);
                DataBind();

                string conversione = CaricaValoreMaster(Request, Session, "conversione");
                if (conversione == "true")
                {
                    Visualizzarisposta();
                    return;
                }

                if (registrazione != "false")
                    VerificaStatoLoginUtente();

                RiempiDdlNazione("IT", ddlNazione);
                CaricaCarrello();
            }
            else
            {
                if (Request["__EVENTTARGET"] == "refreshcarrello")
                {
                    CaricaCarrello();
                }
                output.CssClass = "";
                output.Text = "";
                DataBind();
            }


        }
        catch (Exception err)
        {
            output.CssClass = "alert alert-danger";
            output.Text = err.Message;
        }
    }
    protected void Visualizzarisposta()
    {
        pnlFormOrdine.Visible = false;
        output.CssClass = "alert alert-danger";
        switch (Lingua)
        {
            case "I":
                output.Text = "<div><br/>Ordine inviato correttamente. <br/>Sarete contattati a breve dal nostro personale.</div> " + references.ResMan("Common", Lingua, "GoogleConversione");
                break;
            case "GB":
                output.Text = "<br/>Order Correctly Sent. <br/>You'll be contacted as soon as possible. " + references.ResMan("Common", Lingua, "GoogleConversione");
                break;
        }
    }
    protected void btnCodiceSconto_Click(object sender, EventArgs e)
    {
        string insertedcode = txtCodiceSconto.Text.ToLower();
        string validcode = ConfigManagement.ReadKey("codicesconto");

        ClientiDM cDM = new ClientiDM();
        Cliente cli = cDM.CaricaClientePerCodicesconto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, insertedcode);
        if (cli != null && cli.Id_cliente != 0)
        {
            Session.Add("codicesconto", insertedcode);
            lblCodiceSconto.Text = "";
            // int idcommercialeassociato = cli.Id_cliente;
            //double percscontocommerciale = 0;
            //Dictionary<string, double> dict = ClientiDM.SplitCodiciSconto(cli.Codicisconto);
            //if (dict != null && dict.ContainsKey(insertedcode))
            //{
            //    percscontocommerciale = dict[insertedcode];
            //}
        }
        else if (insertedcode == validcode)
        {
            Session.Add("codicesconto", validcode);
            lblCodiceSconto.Text = "";
        }
        //Testo Se presente una percentuale tra quelle associate ai commerciali e nel caso prendo quella (PRIORITA')

        else
        {
            Session.Remove("codicesconto");
            lblCodiceSconto.Text = references.ResMan("Common", Lingua, "testoErrCodiceSconto").ToString();
        }
        CaricaCarrello();
    }

    protected void chkSupplemento_CheckedChanged(object sender, EventArgs e)
    {
        CaricaCarrello();
    }
    private void VerificaStatoLoginUtente()
    {
        if (!(User.Identity != null && !string.IsNullOrWhiteSpace(User.Identity.Name)))
        {
            Session.Add("Errororder", references.ResMan("Common", Lingua, "testoRichiestalogin").ToString());
            Response.Redirect(references.ResMan("Common", Lingua, "linkLogin").ToString());
        }
    }

    /// <summary>
    /// Aggiorna l'associazione del carrello ai dati dell'utente loggato!!
    /// </summary>
    /// <param name="carrello"></param>
    private void AggiornaDatiUtenteSuCarrello(CarrelloCollection carrello, long idcliente = 0)
    {
        string codicesconto = "";
        if (Session["codicesconto"] != null)
        {
            codicesconto = Session["codicesconto"].ToString();
        }
        foreach (Carrello c in carrello)
        {
            // if (User.Identity != null && User.Identity.Name != "")
            c.ID_cliente = idcliente;
            c.Codicesconto = codicesconto; //metto il codice sconto nella lista prodotti nel carrello
            AggiornaProdottoCarrello(Request, Session, c.id_prodotto, c.Numero, User.Identity.Name, c.Campo2, c.ID, idcliente, c.Prezzo, c.Datastart, c.Dataend, c.jsonfield1);
        }
    }
    private void RiempiDdlNazione(string valore, DropDownList ddlNazione)
    {
        List<Tabrif> nazioni = WelcomeLibrary.UF.Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == Lingua; });
        nazioni.Sort(new GenericComparer<Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));
        ddlNazione.Items.Clear();
        foreach (Tabrif n in nazioni)
        {
            ListItem i = new ListItem(n.Campo1, n.Codice);
            ddlNazione.Items.Add(i);
        }
        try
        {
            ddlNazione.SelectedValue = valore.ToUpper();
        }
        catch { valore = "IT"; ddlNazione.SelectedValue = valore.ToUpper(); }
    }


    protected void ddlNazione_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaCarrello();
    }
    protected string TestoSezione(string codicetipologia)
    {
        string ret = "";
        WelcomeLibrary.DOM.TipologiaOfferte sezione =
              WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codicetipologia); });
        if (sezione != null)
        {
            ret += " " + references.ResMan("Common", Lingua, "testoSezione").ToString() + " \"" + CommonPage.ReplaceAbsoluteLinks(CrealinkElencotipologia(codicetipologia, Lingua, Session)) + "\"";
        }
        return ret;
    }
    protected string TotaleArticolo(object Numero, object Prezzo)
    {
        string ret = "";
        double n = 0;
        double p = 0;
        double.TryParse(Numero.ToString(), out n);
        double.TryParse(Prezzo.ToString(), out p);
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("it-IT");
        ret = string.Format("{0:N2}", p * n, ci);
        return ret;

    }
    private void CaricaCarrello()
    {
        eCommerceDM ecmDM = new eCommerceDM();
        //////////////////////////////////////////////////////////////////////////////
        //Prendiamo l'ip del client
        /////////////////////////////////////////////////////////////////////////////
        string trueIP = "";
        string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (!string.IsNullOrEmpty(ip))
        {
            string[] ipRange = ip.Split(',');
            trueIP = ipRange[0].Trim();
        }
        else
        {
            trueIP = Request.ServerVariables["REMOTE_ADDR"].Trim();
        }
        CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Session.SessionID, trueIP);
        
        rptProdotti.DataSource = carrello;
        rptProdotti.DataBind();

        string codicenazione = SelezionaNazione(carrello, ddlNazione.SelectedValue);
        //SelezionaClientePerAffitti(carrello);//Dedicato alla gestione affitti
        AggiornaDatiUtenteSuCarrello(carrello); //Aggiorno code sconto e idcliente
        VisualizzaTotaliCarrello(codicenazione, "");
        CaricaDatiCliente();

    }

    private void CaricaDatiCliente()
    {
        string idcliente = getidcliente(User.Identity.Name);
        ClientiDM cliDM = new ClientiDM();
        Cliente c = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcliente);
        //Riempiamo il form di fatturazione ....

        if (c != null && c.Id_cliente != 0)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(c.CodiceNAZIONE))
                    ddlNazione.SelectedValue = c.CodiceNAZIONE;
            }
            catch
            { }
            if (string.IsNullOrWhiteSpace(inpNome.Value))
                inpNome.Value = c.Nome;
            if (string.IsNullOrWhiteSpace(inpCognome.Value))
                inpCognome.Value = c.Cognome;
            if (string.IsNullOrWhiteSpace(inpRagsoc.Value))
                inpRagsoc.Value = c.Cognome;
            if (string.IsNullOrWhiteSpace(inpPiva.Value))
                inpPiva.Value = c.Pivacf;
            if (string.IsNullOrWhiteSpace(inpIndirizzo.Value))
                inpIndirizzo.Value = c.Indirizzo;
            if (string.IsNullOrWhiteSpace(inpComune.Value))
                inpComune.Value = c.CodiceCOMUNE;
            if (string.IsNullOrWhiteSpace(inpProvincia.Value))
                inpProvincia.Value = (!(string.IsNullOrWhiteSpace(NomeProvincia(c.CodicePROVINCIA, Lingua)))) ? NomeProvincia(c.CodicePROVINCIA, Lingua) : c.CodicePROVINCIA;
            if (string.IsNullOrWhiteSpace(inpCap.Value))
                inpCap.Value = c.Cap;
            if (string.IsNullOrWhiteSpace(inpEmail.Value))
                inpEmail.Value = c.Email;
            if (string.IsNullOrWhiteSpace(inpTel.Value))
                inpTel.Value = c.Telefono;
            if (string.IsNullOrWhiteSpace(inpPec.Value))
                inpPec.Value = c.Emailpec;
        }


        //Qui potrei aggiornare i dati del cliente in TBL_CLIENTI!!!!-> 
        //da valutare se farlo o meno!!!

    }

    private void SelezionaClientePerAffitti(CarrelloCollection carrello)
    {
        if (carrello != null)
        {
            Carrello c = carrello.Find(_c => _c.Offerta != null && !string.IsNullOrWhiteSpace(_c.Offerta.Email));
            if (c != null)
            {
                Offerte o = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c.Offerta.Id.ToString());
                if (o != null)
                {
                    if (string.IsNullOrWhiteSpace(inpNome.Value))
                        inpNome.Value = o.Nome_dts;
                    if (string.IsNullOrWhiteSpace(inpCognome.Value))
                        inpCognome.Value = o.Cognome_dts;
                    if (string.IsNullOrWhiteSpace(inpEmail.Value))
                        inpEmail.Value = o.Email;
                    if (string.IsNullOrWhiteSpace(inpTel.Value))
                        inpTel.Value = o.Telefono;
                }
            }
        }

    }

    private string SelezionaNazione(CarrelloCollection carrello, string selcodicenazione = "")
    {
        string codicenazione = "";
        if (carrello != null)
        {
            Carrello c = carrello.Find(_c => !string.IsNullOrWhiteSpace(_c.Codicenazione));
            if (c != null)
                codicenazione = c.Codicenazione;
            if (!string.IsNullOrEmpty(selcodicenazione)) codicenazione = selcodicenazione;
        }
        try
        {
            ddlNazione.SelectedValue = codicenazione;
        }
        catch
        { }
        return codicenazione;
    }
    private void VisualizzaTotaliCarrello(string codicenazione, string codiceprovincia)
    {

        bool supplementoisole = chkSupplemento.Checked;
        bool supplementocontrassegno = inpContanti.Checked;
        TotaliCarrello totali = CalcolaTotaliCarrello(Request, Session, codicenazione, codiceprovincia, supplementoisole, supplementocontrassegno);
        List<TotaliCarrello> list = new List<TotaliCarrello>();
        list.Add(totali);
        rptTotali.DataSource = list;
        rptTotali.DataBind();
    }


    protected void checkbox_click(object sender, EventArgs e)
    {
        plhShipping.Visible = !((CheckBox)sender).Checked;
        CaricaCarrello();

    }

    /// <summary>
    /// Invia la mail d'ordine finale
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnConvalidaOrdine(object sender, EventArgs e)
    {
        try
        {
            string TestoMail = "";
            string CodiceOrdine = "";
            //////////////////////////////////////////////////////////////////////////////
            //Prendiamo l'ip del client
            /////////////////////////////////////////////////////////////////////////////
            string trueIP = "";
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ip))
            {
                string[] ipRange = ip.Split(',');
                trueIP = ipRange[0].Trim();
            }
            else
            {
                trueIP = Request.ServerVariables["REMOTE_ADDR"].Trim();
            }
            eCommerceDM ecom = new eCommerceDM();
            //DATI DEL CLIENTE PRESI DAL FORM
            Cliente cliente = new Cliente();
            //MEMORIZZO I DATI DI SPEDIZIONE DI DETTAGLIO IN UN CLIENTE TEMPORANEO AD HOC CHE SERIALIZZO IN UN CAMPO CLIENTEcmd
            Cliente clispediz = new Cliente(cliente);
            if (!string.IsNullOrEmpty(inpCaps.Value.Trim()))
                clispediz.Cap = inpCaps.Value;
            if (!string.IsNullOrEmpty(inpComuneS.Value.Trim()))
                clispediz.CodiceCOMUNE = inpComuneS.Value;
            if (!string.IsNullOrEmpty(inpProvinciaS.Value.Trim()))
                clispediz.CodicePROVINCIA = inpProvinciaS.Value;
            if (!string.IsNullOrEmpty(inpIndirizzoS.Value.Trim()))
                clispediz.Indirizzo = inpIndirizzoS.Value;
            if (!string.IsNullOrEmpty(inpTelS.Value.Trim()))
                clispediz.Telefono = inpTelS.Value;
            string cliserialized = Newtonsoft.Json.JsonConvert.SerializeObject(clispediz);
            cliente.Serialized = cliserialized; //Appoggio i dati di spedizione in Serialized del cliente !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            CaricaDatiClienteDaForm(cliente);

            if (!(User.Identity != null && !string.IsNullOrWhiteSpace(User.Identity.Name))) // Se non loggato metto il cliente tra quelli newsletter
                MemorizzaClientePerNewsletter(cliente);

            //per prima cosa mi riprendo i dati del carrello in base alla sessione per completare l'ordine
            CarrelloCollection prodotti = new CarrelloCollection();
            prodotti = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Session.SessionID, trueIP);
            AggiornaDatiUtenteSuCarrello(prodotti, cliente.Id_cliente); ; //Verifico per un ultima volta che tutto sia a posto che le quantità non superino la disponibilità

            if (prodotti != null && prodotti.Count > 0)
            {


                //vERIFICA finale PER IL BOOKING PRIMA DI ORDINARE!!!
                if (!VerificaDisponibilitaEventoBooking(prodotti, "rif000001"))
                {
                    output.CssClass = "alert alert-danger";
                    output.Text = references.ResMan("basetext", Lingua, "testoprenotaerr1").ToString();
                    return;
                }
                ////////////////////////////////////

                prodotti.Sort(new GenericComparer<Carrello>("Data", System.ComponentModel.ListSortDirection.Descending));
                //Genero il codice ordine dato che il cliente me lo ha confermato e me lo salvo in tabella per tutti i prodotti del carrello attuale
                //In modo da associarli ad un ordine preciso in caso di successo dell'invio del pagamento o della  mail
                CodiceOrdine = GeneraCodiceOrdine();
                bool supplementoisole = chkSupplemento.Checked;
                bool supplementocontrassegno = inpContanti.Checked;
                TotaliCarrello totali = CalcolaTotaliCarrello(Request, Session, cliente.CodiceNAZIONE, "", supplementoisole, supplementocontrassegno);

                string modalita = "";
                string descrizionepagamento = "";

                if (inpContanti.Checked)
                {
                    modalita = inpContanti.Value;
                    descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
                }
                if (inpBonifico.Checked)
                {
                    modalita = inpBonifico.Value;
                    descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
                }
                if (inpPaypal.Checked)
                {
                    modalita = inpPaypal.Value;
                    descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
                }

                if (inpPayway.Checked)
                {
                    modalita = inpPayway.Value;
                    descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
                }
                if (string.IsNullOrEmpty(modalita))
                {
                    output.CssClass = "alert alert-danger"; output.Text = references.ResMan("Common", Lingua, "txtPagamento").ToString();
                    return;
                }
                Session.Add("Lingua", Lingua); //Memorizzo in session  pure la lingua per mantenerla nelle chiamate di risposta dal sistema di pagamento

                //PRENDIAMO I DATI DAL FORM PER LA PREPARAZIONE ORDINE //////////////////////////////////////////////////////////////////
                totali.Denominazionecliente = cliente.Cognome + " " + cliente.Nome;
                totali.Mailcliente = cliente.Email;
                totali.Dataordine = System.DateTime.Now;
                totali.CodiceOrdine = CodiceOrdine;
                totali.Indirizzofatturazione = cliente.Indirizzo + "<br/>";
                totali.Indirizzofatturazione += cliente.Cap + " " + cliente.CodiceCOMUNE + "  (" + cliente.CodicePROVINCIA + ")<br/>";
                totali.Indirizzofatturazione += "Country: " + cliente.CodiceNAZIONE + "<br/>";
                totali.Indirizzofatturazione += "Ph: " + cliente.Telefono + "<br/>";
                totali.Indirizzofatturazione += "Vat: " + cliente.Pivacf + "<br/>";
                totali.Indirizzofatturazione += "CodiceDestinatario/Pec: " + cliente.Emailpec + "<br/>";

                //SE INDIRIZZO SPEDIIZONE DIVERSO -> LO MEMORIZZO NEI TOTALI ( E serializzo il dettaglio nel cliente nel campo serialized )
                string indirizzospedizione = "";
                if (!chkSpedizione.Checked)
                {
                    indirizzospedizione = inpIndirizzoS.Value + "<br/>";
                    indirizzospedizione += inpCaps.Value + " " + inpComuneS.Value + "  (" + inpProvinciaS.Value + ")<br/>";
                    indirizzospedizione += "Country: " + cliente.CodiceNAZIONE + "<br/>";
                    indirizzospedizione += "Ph: " + inpTelS.Value + "<br/>";
                    totali.Indirizzospedizione = indirizzospedizione;
                }
                if (string.IsNullOrWhiteSpace(indirizzospedizione))
                {
                    totali.Indirizzospedizione = totali.Indirizzofatturazione;
                }
                totali.Note = inpNote.Value;
                totali.Modalitapagamento = modalita;
                totali.Pagato = false; //Valorizzato solo alla ricezione del pagamento prima della spedizione o tramite la procedura con pagamento anticipato
                totali.Urlpagamento = "";

                //Prepariamo i valori per la chiamata a Paypal
                Session.Add("cliente_" + CodiceOrdine, cliente); //Mettiamo tutto in sessione per riaverlo alla conferma dell'esito positivo della transazione
                Session.Add("totali_" + CodiceOrdine, totali); //Mettiamo tutto in sessione per riaverlo alla conferma dell'esito positivo della transazione
                Session.Add("prodotti_" + CodiceOrdine, prodotti); //Mettiamo tutto in sessione per riaverlo alla conferma dell'esito positivo della transazione

                // / END // PRENDIAMO I DATI DAL FORM PER LA PREPARAZIONE ORDINE //////////////////////////////////////////////////////////////////

                /////EFFETTUIAMO LA PROCEDURA CORRETTA IN BASE ALLA MODALITA' DI PAGAMENTO
                if (modalita == "payway") //PAGAMENTO CON CARTA DI CREDITO  
                {
                    ///////////////////////SEZIONE IMPOSTAZIONE PAYWAY//////////////////////////////
                    //CREIAMO IL FORM PER IL POST PER PAYWAY
                    ////////////////////////////////////////////////////////////////////////////////

                    string merchantid = ConfigManagement.ReadKey("merchantid");
                    string urlpagamentosoar = ConfigManagement.ReadKey("urlsoar");
                    RemotePost remotepost = new RemotePost();
                    remotepost.test = Convert.ToBoolean(ConfigManagement.ReadKey("testsoar"));//IMPOSTO LA MODALITA' DI TEST DEL SISTEMA DI PAGAMENTO DA WEB CONFIG 
                    remotepost.Url = urlpagamentosoar;// 
                    remotepost.Add("MID", merchantid);//id mercante fornito da soar
                    remotepost.Add("OID", CodiceOrdine);//codice dell'ordine attuale identificativo
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("it-IT");
                    string costo = string.Format(culture, "{0:0}", new object[] { Convert.ToDouble(totali.TotaleOrdine - totali.TotaleSconto + totali.TotaleSpedizione) * 100 }); //Il totale da passare è in centesimi di euro
                    remotepost.Add("IMP", costo);//Il campo importo è numerico, specificato in centesimi. Esempio : 001920 = 19 Euro e 20 centesimi
                    remotepost.Add("LAN", "it");//it o en
                    remotepost.Add("_Nome", cliente.Nome);
                    remotepost.Add("_EMail", cliente.Email);
                    try
                    {
                        string Encodedidordine = dataManagement.EncodeToBase64(CodiceOrdine);
                        this.Session.Add("form_" + CodiceOrdine, remotepost);
                        //con popup
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "apripagpagamento", "window.open('ordine_form.aspx?tran=" + Encodedidordine + "',target='_blank')", true);
                        //senza popup
                        Response.Redirect("ordine_form.aspx?tran=" + Encodedidordine);
                        //Server.Transfer("ordine_form.aspx?tran=" + Encodedidordine, true);//Chiamo la pagina di post passando il guid con prefisso codificato nella querystring
                        //Response.Redirect("PalTranEx.aspx?tran=" + szEncodedGuid);
                    }
                    catch (Exception ex)
                    {
                        output.CssClass = "alert alert-danger"; output.Text = ex.Message;
                    }
                    //CREO LA CHIAMATA IN POST ALLA PAGINA PAGAMENTI (ALTRO SISTEMA SENZA PAGINA DI APPOGGIO)
                    //remotepost.Url = "http://www.jigar.net/demo/HttpRequestDemoServer.aspx";//;//https://wsso.bccsoar.it:443/vtrans/ezcode.do
                    //remotepost.Add("field1", "Huckleberry");
                    //remotepost.Add("field2", "Finn");
                    //remotepost.Post()
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                }

                if (modalita == "paypal") //PAGAMENTO CON CARTA DI CREDITO  
                {
                    //Procediamo con l'odine su paypal
                    //Valori impostazione paypal
                    string Encodedidordine = dataManagement.EncodeToBase64(CodiceOrdine);
                    string returl = (ConfigManagement.ReadKey("returlPaypal")) + Encodedidordine + "&Lingua=" + Lingua;
                    string cancelurl = (ConfigManagement.ReadKey("cancelurlPaypal")) + Encodedidordine + "&error=true" + "&Lingua=" + Lingua;
                    //if (!returl.ToLower().StartsWith(PercorsoAssolutoApplicazione.ToLower()))
                    //    returl = PercorsoAssolutoApplicazione + "/" + returl;
                    //if (!cancelurl.ToLower().StartsWith(PercorsoAssolutoApplicazione.ToLower()))
                    //    cancelurl = PercorsoAssolutoApplicazione + "/" + cancelurl;
                    Dictionary<string, List<string>> paypaldatas = new Dictionary<string, List<string>>();
                    //Formattiamo i dati per paypal per le descrizioni e il totale ordine
                    MemorizzaDatiPerPaypal(totali.Percacconto, totali, prodotti, ref paypaldatas);
                    //eseguiamo la procedura di pagamento su paypal
                    bool authandcapturemode = Convert.ToBoolean(ConfigManagement.ReadKey("authandcapturePaypal"));
                    EseguiCheckOutPaypal(returl, cancelurl, paypaldatas, authandcapturemode); // da traspormare in auth and capture true succesivamente
                }
                if (modalita == "bacs" || modalita == "contanti") //PAGAMENTO CON BONIFICO
                {
                    //try
                    //{
                    //    CreaFileExportOrdini(totali, prodotti, cliente);
                    //}
                    //catch (Exception err)
                    //{
                    //    output.Text = err.Message + " <br/> ";
                    //    switch (Lingua)
                    //    {
                    //        case "I":
                    //            output.Text += "Errore creazione export ordini. Contattatare l'assistenza. ";
                    //            break;
                    //        case "GB":
                    //            output.Text += "Error creating export order. Contact support.";
                    //            break;
                    //    }
                    //}


                    try
                    {
                        //Inviamo le email di conferma al portale ed al cliente
                        //Invio la mail per il fornitore
                        string SoggettoMailFornitore = references.ResMan("Common", Lingua, "OrdineSoggettomailRichiesta") + Nome;
                        TestoMail = CreaMailPerFornitore(totali, prodotti);
                        Utility.invioMailGenerico(totali.Denominazionecliente, totali.Mailcliente, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);
                        //Invia la mail per il cliente
                        string SoggettoMailCliente = references.ResMan("Common", Lingua, "OrdineSoggettomailRiepilogo") + Nome;
                        TestoMail = CreaMailCliente(totali, prodotti);
                        Utility.invioMailGenerico(Nome, Email, SoggettoMailCliente, TestoMail, totali.Mailcliente, totali.Denominazionecliente, null, "", true, Server);
                    }
                    catch { }


                    try
                    {
                        //Qui devo scrivere nella tabella ordini i dati qui memorizzati ( TBL_CARRELLO_ORDINI )
                        ecom.InsertOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, totali);
                    }
                    catch (Exception eins)
                    {
                        switch (Lingua)
                        {
                            case "I":
                                output.Text += "Errore inserimento db ordine " + eins.Message;
                                break;
                            default:
                                output.Text += "Error inserting db order " + eins.Message;
                                break;
                        }
                    }
                    //AGGIORNO  I PRODOTTI NEL CARRELLO INSERENDO IL CODICE DI ORDINE
                    //E GLI ALTRI DATI ACCESSORI ( TBL_CARRELLO )
                    foreach (Carrello item in prodotti)
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Prepariamo le richieste di feeback per gli articoli in ordine!!
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        try
                        {
                            Mail mailfeedback = new Mail();

                            mailfeedback.Sparedict["linkfeedback"] = "";//default preso dalle risorse feedbacksdefaultform
                            mailfeedback.Sparedict["idnewsletter"] = "";//default dalle risorse feedbackdefaultnewsletter
                            mailfeedback.Sparedict["deltagiorniperinvio"] = "";//default dalle risorse feedbacksdefaultdeltagg
                            mailfeedback.Sparedict["idclienti"] = cliente.Id_cliente.ToString();
                            mailfeedback.Id_card = item.id_prodotto;
                            HandlerNewsletter.preparamail(mailfeedback, Lingua); //Preparo le mail nello scheduler!!

                        }
                        catch { }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        item.CodiceOrdine = CodiceOrdine;
                        SalvaCodiceOrdine(item);
                    }
                    InsertEventoBooking(prodotti, totali, "rif000001");


                    //CreaNuovaSessione(Session, Request); //Svuota la session per un nuovo ordine!!
                    pnlFormOrdine.Visible = false;
                    output.Text += references.ResMan("Common", Lingua, "GoogleConversione");
                    switch (Lingua)
                    {
                        case "I":
                            output.Text += "<div><br/>Ordine inviato correttamente. <br/>Sarete contattati a breve dal nostro personale.</div>";
                           
                            break;
                        default:
                            output.Text += "<br/>Order Correctly Sent. <br/>You'll be contacted as soon as possible.";
                            break;
                    }

                }
            }
            else
            {
                output.Text += " Carrello vuoto / Empty order list. <br/> ";
            }
        }
        catch (Exception err)
        {
            output.Text += err.Message + " <br/> ";
            switch (Lingua)
            {
                case "I":
                    output.Text += "Errore invio ordine ";
                    break;
                default:
                    output.Text += "Error sending order ";
                    break;
            }
        }
    }

    private void InsertEventoBooking(CarrelloCollection prodotti, TotaliCarrello totali, string filtrotipologia)
    {
        foreach (Carrello c in prodotti)
        {
            if (c.Dataend != null && c.Datastart != null)
            {
                if (!string.IsNullOrEmpty(filtrotipologia))
                {
                    if (c.Offerta.CodiceTipologia != filtrotipologia) continue;
                }
                try
                {
                    Eventi tmpelement = new Eventi();
                    tmpelement.Enddate = c.Dataend.Value;
                    tmpelement.Startdate = c.Datastart.Value;
                    tmpelement.Prezzo = c.Prezzo;
                    tmpelement.Idattivita = c.id_prodotto;
                    tmpelement.Soggetto = "Order.n. " + c.CodiceOrdine;
                    if (totali.Percacconto != 100) tmpelement.Soggetto += " (Richiesto Acconto : " + totali.Percacconto + "%)";
                    tmpelement.Testoevento = "";
                    tmpelement.Idcliente = c.ID_cliente;
                    tmpelement.Idvincolo = "";
                    tmpelement.Codicerichiesta = "";//identificativo della richiesta a calendario generato all'inserimento
                    tmpelement.Status = 1; //Stato   confermato dell'evento/prenotazione per le richieste con pagamento Bonifico ( ovviamente se poi non pagato ... l'operatore dovrà eliminarlo dal calendario )
                    tmpelement.Jsonfield1 = c.jsonfield1;

                    bookingDM.dbInsertEvent(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tmpelement);
                }
                catch { }
            }

        }
    }
    /// <summary>
    /// torna true se non disponibile il periodo per i prodotti scelti
    /// </summary>
    /// <param name="prodotti"></param>
    /// <returns></returns>
    private bool VerificaDisponibilitaEventoBooking(CarrelloCollection prodotti, string filtrotipologia)
    {
        bool free = true;
        foreach (Carrello c in prodotti)
        {
            if (!string.IsNullOrEmpty(filtrotipologia))
            {
                if (c.Offerta.CodiceTipologia != filtrotipologia) continue;
            }
            if (c.Dataend != null && c.Datastart != null)
            {
                try
                {


                    bool nonelem = bookingDM.dbIsFree(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "0", c.Datastart.Value, c.Dataend.Value, c.id_prodotto.ToString());
                    if (!nonelem)
                    {
                        free = false;
                    }
                }
                catch { }
            }

        }
        return free;
    }
    private static string FormatXML(string unformattedXml)
    {
        // first read the xml ignoring whitespace
        XmlReaderSettings readeroptions = new XmlReaderSettings { IgnoreWhitespace = true, ConformanceLevel = ConformanceLevel.Fragment };
        XmlReader reader = XmlReader.Create(new System.IO.StringReader(unformattedXml), readeroptions);

        // then write it out with indentation
        StringBuilder sb = new StringBuilder();
        XmlWriterSettings xmlSettingsWithIndentation = new XmlWriterSettings { Encoding = Encoding.Unicode, OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment, NewLineOnAttributes = false, Indent = true };

        using (XmlWriter writer = XmlWriter.Create(sb, xmlSettingsWithIndentation))
        {
            writer.WriteNode(reader, true);
        }

        return sb.ToString();
    }
#if false
    private void CreaFileExportOrdini(TotaliCarrello totali, CarrelloCollection prodotti, Cliente cliente)
    {
        Dictionary<string, CompleteOrderData> ordini = new Dictionary<string, CompleteOrderData>();

        string fileordini = Server.MapPath("~/_transf/wf_ordini_web.xml");
        //Per prima cosa devo leggere il file se presente -> e caricarlo in memoria per l'elaborazione
        //(POTREI ANCHE PRENDERE IL TESTO NEL FILE ALL'INTERNO DEL TAG ORDINI ed ACCODARLO A QUELLO CREATO DEL NUOVO ORDINE SENZA RIPARSERIZZARE L'XML CARICATO)

        string entireXmlContent = "";
        if (System.IO.File.Exists(fileordini))
        {
            //LETTURA FILE ORDINI SE PRESENTE
            //Se presente Prima carico eventuale file presente per aggiornarne il contenuto 
            System.IO.StreamReader sr = new System.IO.StreamReader(fileordini,
                                     Encoding.GetEncoding("iso-8859-1"));
            System.Xml.XmlTextReader scriptXmlReader = new System.Xml.XmlTextReader(sr);
            scriptXmlReader.WhitespaceHandling = WhitespaceHandling.None;
            //TotaliCarrello totalitmp = new TotaliCarrello();
            //CarrelloCollection prodottitmp = new CarrelloCollection();
            //Cliente clientetmp = new Cliente();
            using (scriptXmlReader)
            {
                if (scriptXmlReader.ReadToFollowing("ORDINI"))
                {
                    entireXmlContent = scriptXmlReader.ReadInnerXml(); //LEGGO TUTTO IL CONTENUTO DELL'XML!!!

                }
            }

            //Formatto il contenuto dell'xml per reinserirlo nel file finale
            entireXmlContent = "\r\n" + FormatXML(entireXmlContent);
            entireXmlContent = entireXmlContent.Replace("\r\n", "\r\n\t") + "\r\n";
        }
        //DATIORDINE NUOVO DA INSERIRE ( O MODIFICARE )
        string codiceordine = totali.CodiceOrdine;
        //Cerco l'ordine corrente per codice ordine e se presente lo elimino dalla lista trovata nel file per sostituirlo con quello in corso!
        if (ordini.ContainsKey(codiceordine))
            ordini.Remove(codiceordine);

        ///////////////////////////////////////////////////////////////////////////
        //SCRIVIAMO IL FILE XML con tutti gli ordini
        //Funzione che mi crea il file sitemap.xml in formato base
        System.IO.FileStream orderfile = new System.IO.FileStream(fileordini, System.IO.FileMode.Create);
        using (orderfile)
        {

            //Inizio a scrivere il file
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(orderfile, Encoding.Default);
            writer.Formatting = System.Xml.Formatting.Indented;

            // aggiungo l'intestazione XML 
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>");
            //writer.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

            //Qui ci metto l'apertura dell'elemento
            // writer.WriteStartElement("urlset");
            //   writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

            writer.WriteStartElement("ORDINI"); //ORDINI
            writer.WriteStartElement("ORDINE"); //ORDINE

            writer.WriteStartElement("DATIORDINE");//DATIORDINE
            /////////////////////////////
            writer.WriteStartElement("NUMORDINE");
            writer.WriteRaw(codiceordine);
            writer.WriteEndElement();
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("it");
            string dataordine = string.Format(ci, "{0:ddMMyyyyHHmmss}", new object[] {totali.Dataordine );
            writer.WriteStartElement("DATAORDINE");
            writer.WriteRaw(dataordine);
            writer.WriteEndElement();
            string statoordine = "processing";
            writer.WriteStartElement("STATOORDINE");
            writer.WriteRaw(statoordine);
            writer.WriteEndElement();
            string pesoordine = "0";
            writer.WriteStartElement("PESOORDINE");
            writer.WriteRaw(pesoordine);
            writer.WriteEndElement();
            string modalitapagamento = totali.Modalitapagamento;
            writer.WriteStartElement("TIPOPAGAMENTO");
            writer.WriteRaw(modalitapagamento);
            writer.WriteEndElement();
            string spesetrasporto = totali.TotaleSpedizione.ToString();
            writer.WriteStartElement("SPESETRASPORTO");
            writer.WriteRaw(spesetrasporto);
            writer.WriteEndElement();
            string noteordine = totali.Note;
            writer.WriteStartElement("NOTEORDINE");
            writer.WriteRaw(noteordine);
            writer.WriteEndElement();
            /////////////////////////////
            writer.WriteEndElement();//DATIORDINE


            writer.WriteStartElement("CLIENTE");//CLIENTE
            /////////////////////////////
            writer.WriteStartElement("DATIFATTURAZIONE");//DATIFATTURAZIONE
            /////////////////////////////
            if (cliente != null)
            {
                writer.WriteStartElement("EMAIL");
                writer.WriteRaw(cliente.Email);
                writer.WriteEndElement();
                writer.WriteStartElement("RAGIONESOCIALE");
                writer.WriteRaw(cliente.Cognome + " " + cliente.Nome);
                writer.WriteEndElement();
                writer.WriteStartElement("INDIRIZZO");
                writer.WriteRaw(cliente.Indirizzo);
                writer.WriteEndElement();
                writer.WriteStartElement("CAP");
                writer.WriteRaw(cliente.Cap);
                writer.WriteEndElement();
                writer.WriteStartElement("CITTA");
                writer.WriteRaw(cliente.CodiceCOMUNE);
                writer.WriteEndElement();
                writer.WriteStartElement("PROVINCIA");
                writer.WriteRaw(cliente.CodicePROVINCIA);
                writer.WriteEndElement();
                writer.WriteStartElement("STATO");
                writer.WriteRaw(cliente.CodiceNAZIONE);
                writer.WriteEndElement();
                writer.WriteStartElement("PARTITAIVA");
                writer.WriteRaw(cliente.Pivacf);
                writer.WriteEndElement();
                writer.WriteStartElement("TELEFONO");
                writer.WriteRaw(cliente.Telefono);
                writer.WriteEndElement();
                writer.WriteStartElement("FAX");
                //writer.WriteRaw(cliente.);
                writer.WriteEndElement();
            }
            /////////////////////////////
            writer.WriteEndElement();//DATIFATTURAZIONE

            writer.WriteStartElement("DATISPEDIZIONE");//DATISPEDIZIONE
            /////////////////////////////
            Cliente clispediz = null;
            try { clispediz = Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(cliente.Spare1); } catch { }
            if (clispediz != null)
            {
                writer.WriteStartElement("RAGIONESOCIALE");
                writer.WriteRaw(clispediz.Cognome + " " + clispediz.Nome);
                writer.WriteEndElement();
                writer.WriteStartElement("INDIRIZZO");
                writer.WriteRaw(clispediz.Indirizzo);
                writer.WriteEndElement();
                writer.WriteStartElement("CAP");
                writer.WriteRaw(clispediz.Cap);
                writer.WriteEndElement();
                writer.WriteStartElement("CITTA");
                writer.WriteRaw(clispediz.CodiceCOMUNE);
                writer.WriteEndElement();
                writer.WriteStartElement("PROVINCIA");
                writer.WriteRaw(clispediz.CodicePROVINCIA);
                writer.WriteEndElement();
                writer.WriteStartElement("STATO");
                writer.WriteRaw(clispediz.CodiceNAZIONE);
                writer.WriteEndElement();
                writer.WriteStartElement("PARTITAIVA");
                writer.WriteRaw(clispediz.Pivacf);
                writer.WriteEndElement();
                writer.WriteStartElement("TELEFONO");
                writer.WriteRaw(clispediz.Telefono);
                writer.WriteEndElement();
                writer.WriteStartElement("FAX");
                //writer.WriteRaw(cliente.);
                writer.WriteEndElement();
            }
            /////////////////////////////
            writer.WriteEndElement();//DATISPEDIZIONE
            /////////////////////////////
            writer.WriteEndElement();//CLIENTE


            writer.WriteStartElement("DETTAGLIOARTICOLI");//DETTAGLIOARTICOLI
            /////////////////////////////
            double prezzofinale = totali.TotaleOrdine - totali.TotaleSconto;
            //ARTICOLI
            foreach (Carrello prodotto in prodotti)
            {
                double percripartizionesconto = totali.TotaleSconto / totali.TotaleOrdine; //% Sconto generale sul totale del prezzo da ripartire sugli articoli

                writer.WriteStartElement("ARTICOLO");
                switch (prodotto.Offerta.CodiceTipologia)
                {
                    case "rif000100":
                        //prodotto.CodiceProdotto //Codice prodotto ( è quello ministeriale per i prodotti standard
                        //prodotto.Prezzo  //Prezzo ivato unitario del prodotto ( da moltiplicare per la quantità ... )
                        //prodotto.Numero; //numero prodotti inseriti in carrello
                        //prodotto.Offerta.Pivacf_dts //valore percentuale iva
                        //prodotto.Offerta.DenominazioneI //Descrizione
                        writer.WriteStartElement("CODICE");
                        writer.WriteRaw(prodotto.CodiceProdotto);
                        writer.WriteEndElement();
                        writer.WriteStartElement("DESCRIZIONE");
                        writer.WriteRaw(prodotto.Offerta.DenominazioneI);
                        writer.WriteEndElement();
                        writer.WriteStartElement("QUANTITA");
                        writer.WriteRaw(prodotto.Numero.ToString()); //numero di prodotti in ordine
                        writer.WriteEndElement();
                        writer.WriteStartElement("IVA");
                        writer.WriteRaw(prodotto.Offerta.Pivacf_dts);
                        writer.WriteEndElement();
                        writer.WriteStartElement("PREZZO");
                        double tmpps = Math.Round(prodotto.Prezzo * percripartizionesconto, 2); //Calcolo lo sconto 
                        tmpps = prodotto.Prezzo - tmpps; //prezzo scontato del prodotto 
                        //////////////////////////////////////////////////////////////////////////////////////////
                        // ... in base a tag del prezzo del file.xml ( se unitario o totale )
                        //tmpps = prodotto.Numero * tmpps; //Moltiplico il prezzo unitario scontato del prodotto per il numero prodotti richiesti
                        writer.WriteRaw(tmpps.ToString());// (   writer.WriteRaw(tmpps) se nel campo PREZZO ci và il prezzo unitario del prodotto )
                        writer.WriteEndElement();
                        break;
                    case "rif000101":
                        //prodotto.Prezzo  //Prezzo TOTALE ivato del KIT comprensivo di sconto
                        //prodotto.CodiceProdotto //Codice prodotto -> potrebbe essere null nel kit -> comunque devi prendere i codici nei sottoprodtti
                        //ATTENZIONE Per i prezzi dei prodotti componenti le offerte devi ripartire il totale del kit in proporzione sui prodotti componenti
                        // creare gli articoli VARIANDO IL PREZZO DEI SINGOLI IN MODO CHE LA SOMMA DI TUTTI DIA IL PREZZO DEL KIT!!!!!!!...
                        //(value, price, iva, codice, qta)  I prodotti collegati alle offerte sono contenuti nella lista ...>
                        if (prodotto != null && prodotto.Offerta != null)
                        {
                            //Applichiamo lo sconto generale sul totale ad ogni articolo di carrello ( in modo da applicare lo sconto complessivo )
                            double tmppsk = Math.Round(prodotto.Prezzo * percripartizionesconto, 2);
                            tmppsk = prodotto.Prezzo - tmppsk;  //prodotto.Prezzo -> Prezzo ivato unitario del prodotto ( da moltiplicare per la quantità ... )
                            double qtakit = prodotto.Numero; //numero di kit selezionati
                            List<ResultAutocomplete> listprod = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultAutocomplete>>(prodotto.Offerta.XmlValue);

                            //ANDIAMO A INSERIRE I PRODOTTI DEL KIT NEL FILE EXPORT APPLICANDO LA RIPARTIZIONE DEL COSTO COMPLESSIVO DEL KIT
                            //CALCOLIAMO il totale dei prodotti del kit in base ai prezzi degli articoli componenti senza sconto del kit
                            double totalenonscontato = 0;
                            foreach (ResultAutocomplete p in listprod)
                            {
                                double tmpp = 0;
                                double.TryParse(p.price, out tmpp);
                                double tmpq = 0;
                                double.TryParse(p.qta, out tmpq);
                                double tmpsum = tmpq * tmpp * qtakit;
                                totalenonscontato += tmpsum; //TOTALE NON SCONTATO PER I PRODOTTI CHE COMPONGONO IL KIT
                            }
                            //CAlcolo la percentuale di sconto da applicare ai componenti del kit
                            double percsconto = (tmppsk * qtakit) / (totalenonscontato); //% sconto da applicare sul prezzo originario degli articoli
                            foreach (ResultAutocomplete p in listprod)
                            {
                                writer.WriteStartElement("CODICE");
                                writer.WriteRaw(p.codice);
                                writer.WriteEndElement();
                                writer.WriteStartElement("DESCRIZIONE");
                                writer.WriteRaw(p.value);
                                writer.WriteEndElement();
                                writer.WriteStartElement("QUANTITA");
                                double qta = 0;
                                double.TryParse(p.qta, out qta); //quantità prodotti compresi nel kit
                                double totaleordinati = qtakit * qta; //Moltiplico per il numero di kit acquistati -> totale prodotti da spedire
                                writer.WriteRaw(totaleordinati.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("IVA");
                                writer.WriteRaw(p.iva);
                                writer.WriteEndElement();
                                double tmpp = 0;
                                double.TryParse(p.price, out tmpp);
                                tmpp = Math.Round(tmpp * percsconto, 2); //Prezzo unitario scontato del prodotto singolo nel kit
                                writer.WriteStartElement("PREZZO");
                                ///////////////////////////////////////////////////
                                // ... in base a tag del prezzo del file.xml ( se unitario o totale ) ->
                                //Moltiplico il prezzo unitario scontato del kit offerta per i componenti del kit per il numero di kit acquistati
                                //tmpp = qtakit * qta * tmpp;
                                writer.WriteRaw(tmpp.ToString());  //Per mettere il prezzo totale e non unitario del prodotto và moltiplicato * qtakit
                                writer.WriteEndElement();
                            }
                        }
                        break;
                    default:
                        break;
                }
                writer.WriteEndElement();//CLOSE ARTICOLO
            }
            /////////////////////////////
            writer.WriteEndElement();// CLOSE DETTAGLIOARTICOLI


            writer.WriteEndElement(); //CLOSE ORDINE

            //Inserisco l'xml precedentemente presente nel file
            writer.WriteRaw(entireXmlContent);

            //var xd = System.Xml.Linq.XDocument.Parse(entireXmlContent);
            //xd.WriteTo(writer);

            //Chiudo l'elemento 
            writer.WriteEndElement();//CLOSE ORDINI

            //// scrivo a video e chiudo lo stream 
            writer.Flush();
            writer.Close();
            orderfile.Close();
        }

    }

#endif
    private void MemorizzaDatiPerPaypal(double percentualeanticipo, TotaliCarrello totali, CarrelloCollection prodotti, ref Dictionary<string, List<string>> paypaldatas)
    {
        paypaldatas = new Dictionary<string, List<string>>();
        List<string> dettagliitem;
        foreach (Carrello p in prodotti)
        {
            dettagliitem = new List<string>();
            dettagliitem.Add(p.Offerta.Id.ToString()); // Meglio p.Offerta.Id per identificare l'articolo non p.CodiceProdotto che in questo caso non è detto ci sia
            string titoloarticolo = "";
            if (percentualeanticipo != 100) titoloarticolo += "Acconto " + percentualeanticipo + "% ";
            if (p.Offerta != null)
            {
                titoloarticolo += p.Offerta.DenominazioneI;
            }
            else titoloarticolo = ("no title");

            if (p.Dataend != null && p.Datastart != null)
            {
                titoloarticolo += p.Datastart != null ? string.Format("{0:dd/MM/yyyy}", p.Datastart.Value) : "";
                titoloarticolo += p.Dataend != null ? "-" + string.Format("{0:dd/MM/yyyy}", p.Dataend.Value) : "";
            }
            dettagliitem.Add(titoloarticolo);//Prendo il valore I per ora senza modificare x lingua

            string testodescrizionevocecarrello = "";
            //Qui dovresti valorizzare la descrizione di dettaglio della voce di carrello!!
            dettagliitem.Add(testodescrizionevocecarrello);
            dettagliitem.Add(p.Prezzo.ToString());
            dettagliitem.Add(p.Numero.ToString());
            dettagliitem.Add(percentualeanticipo.ToString());
            if (!paypaldatas.ContainsKey(p.Offerta.Id.ToString()))//p.CodiceProdotto
                paypaldatas.Add(p.Offerta.Id.ToString(), dettagliitem);//p.CodiceProdotto
        }

        //Aggiungo un elemento per smaltimnto
        //dettagliitem = new List<string>();
        //dettagliitem.Add("");
        //dettagliitem.Add(references.ResMan("Common",Lingua,"CarrelloTotaleSmaltimento); //Descrizione elemento
        //dettagliitem.Add("");
        //dettagliitem.Add(totali.TotaleSmaltimento.ToString());
        //dettagliitem.Add("1");//Quantità
        //dettagliitem.Add("100");//Percentualeanticipo
        //if (!paypaldatas.ContainsKey("pp_pfucosts"))
        //    paypaldatas.Add("pp_pfucosts", dettagliitem);

        //Aggiungo un elemento per spesespedizione
        dettagliitem = new List<string>();
        dettagliitem.Add("");
        dettagliitem.Add(references.ResMan("Common", Lingua, "CarrelloTotaleSpedizione")); //Descrizione elemento")
        dettagliitem.Add("");
        dettagliitem.Add(totali.TotaleSpedizione.ToString());
        dettagliitem.Add("1");//Quantità
        dettagliitem.Add("100");//Percentualeanticipo
        if (!paypaldatas.ContainsKey("pp_expcosts"))
            paypaldatas.Add("pp_expcosts", dettagliitem);

        //aggiungo elemento per scontp
        dettagliitem = new List<string>();
        dettagliitem.Add("");
        dettagliitem.Add(references.ResMan("Common", Lingua, "testoSconto"));//Descrizione elemento
        dettagliitem.Add("");
        dettagliitem.Add("-" + totali.TotaleSconto.ToString());
        dettagliitem.Add("1");//Quantità
        dettagliitem.Add("100");//Percentuale anticipo
        if (!paypaldatas.ContainsKey("pp_discount"))
            paypaldatas.Add("pp_discount", dettagliitem);
    }

    /// <summary>
    /// da modificare per prevedere l'inserimento di descrizioni dettagliate per le prenotazioni. .....
    /// </summary>
    /// <param name="returnurl"></param>
    /// <param name="cancelurl"></param>
    /// <param name="amount"></param>
    protected void EseguiCheckOutPaypal(string returnurl, string cancelurl, Dictionary<string, List<string>> paypaldatas, bool authandcapture)
    {
        //PEr authorization and capture method -> Da implementare come metodo di conferma pagamento
        // https://developer.paypal.com/docs/classic/express-checkout/ht_ec-singleAuthPayment-curl-etc/
        //https://ppmts.custhelp.com/app/answers/detail/a_id/939/kw/PayPal%20Express%20Checkout%20Payment%20Actions

        string user = ConfigManagement.ReadKey("userPaypal");
        string pwd = ConfigManagement.ReadKey("pwdPaypal");
        string signature = ConfigManagement.ReadKey("signaturePaypal");
        bool sandbox = Convert.ToBoolean(ConfigManagement.ReadKey("sandboxPaypal"));
        NVPAPICaller test = new NVPAPICaller(user, pwd, signature, returnurl, cancelurl, sandbox);

        string retMsg = "";
        string token = "";
        if (paypaldatas != null && paypaldatas.Count > 0)
        {
            bool ret = test.ShortcutExpressCheckout(paypaldatas, Lingua, ref token, ref retMsg, authandcapture);
            if (ret)
            {
                HttpContext.Current.Session["token"] = token;

                //Eventualmente da metter anche in session l'info se ho usato l'auth and capture o direct payment!!! per ora non lo passo
                Response.Redirect(retMsg); //vado alla pagina di pagamento per eseguire l'operazione
            }
            else
            {
                //Response.Redirect("Paypal/APIError.aspx?" + retMsg);
                string[] retCol = retMsg.Split('&');
                string errcode = retCol.FirstOrDefault(c => c.Contains("ErrorCode")).Replace("ErrorCode=", ""); ;
                string desc = retCol.FirstOrDefault(c => c.Contains("Desc")).Replace("Desc=", ""); ;
                string desc2 = retCol.FirstOrDefault(c => c.Contains("Desc2")).Replace("Desc2=", ""); ;
                output.CssClass = "alert alert-danger"; output.Text = references.ResMan("Common", Lingua, "risposta_4").ToString() + " Error" + errcode + "<br/>";
                output.Text += desc + "<br/>";
                output.Text += desc2 + "<br/>";
            }
        }
        else
        {
            //Response.Redirect("Paypal/APIError.aspx?ErrorCode=AmtMissing");
            output.CssClass = "alert alert-danger"; output.Text = references.ResMan("Common", Lingua, "risposta_4").ToString() + "  Assente importo operazione.<br/>";
        }
    }
    public void MemorizzaClientePerNewsletter(Cliente cli)
    {
        try
        {
            //------------------------------------------------
            //Memorizzo i dati nel cliente per la newsletter
            //------------------------------------------------
            ClientiDM cliDM = new ClientiDM();
            string lingua = Lingua;
            string tipocliente = "0"; //Cliente standard per newsletter

            cli.DataNascita = System.DateTime.Now.Date;
            cli.Lingua = lingua;
            cli.id_tipi_clienti = tipocliente;
            cli.Consenso1 = true;
            cli.ConsensoPrivacy = true;
            cli.Validato = true;
            cli.Email = cli.Email.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
            cli.Emailpec = cli.Emailpec.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
            Cliente _tmp = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email);
            if ((_tmp != null && _tmp.Id_cliente != 0))
                cli.Id_cliente = _tmp.Id_cliente;
            cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);
        }
        catch
        { }
    }

    private void CaricaDatiClienteDaForm(Cliente cliente)
    {

        cliente.CodiceNAZIONE = ddlNazione.SelectedValue;
        cliente.Cognome = inpCognome.Value;
        cliente.Nome = inpNome.Value;
        if (inpRagsoc.Value != "")
        {
            cliente.Cognome = inpRagsoc.Value;
            cliente.Nome = "";

        }
        cliente.Email = inpEmail.Value;
        cliente.Emailpec = inpPec.Value;

        cliente.Pivacf = inpPiva.Value;
        cliente.Indirizzo = inpIndirizzo.Value;
        cliente.CodiceCOMUNE = inpComune.Value;
        cliente.CodicePROVINCIA = inpProvincia.Value;
        cliente.Cap = inpCap.Value;
        cliente.Telefono = inpTel.Value;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Qui procedo ad aggiornare i dati in TBL_CLIENTI con quelli inseriti nel form ( facendo un match sulla mail del cliente in tabella!! )
        ClientiDM cliDM = new ClientiDM();
        Cliente clitmp = new Cliente();
        clitmp.Email = cliente.Email;
        ClienteCollection clifiltrati = cliDM.CaricaClientiFiltrati(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, clitmp, true);
        //clifiltrati = cliDM.GetLista(clitmp.Email, ""); ;
        foreach (Cliente c in clifiltrati)
        {
            Cliente ctmp = new Cliente(c);
            ctmp.CodiceNAZIONE = cliente.CodiceNAZIONE;
            ctmp.Nome = cliente.Nome;
            ctmp.Cognome = cliente.Cognome;
            ctmp.Email = cliente.Email;
            ctmp.Emailpec = cliente.Emailpec;
            ctmp.Pivacf = cliente.Pivacf;
            ctmp.Indirizzo = cliente.Indirizzo;

            ctmp.CodiceCOMUNE = cliente.CodiceCOMUNE;
            ctmp.CodicePROVINCIA = cliente.CodicePROVINCIA;

            ctmp.Cap = cliente.Cap;
            ctmp.Telefono = cliente.Telefono;
            ctmp.Serialized = cliente.Serialized; //Dati serializzati aggiuntivi

            cliDM.InserisciAggiornaCliente("", ref ctmp);
            cliente.Id_cliente = clitmp.Id_cliente;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }

    protected string GeneraCodiceOrdine()
    {
        string CodiceOrdine = "";

#if true
        bool esito = true;
        while (esito)
        {
            //creo un Codice Ordine Univoco
            CodiceOrdine = WelcomeLibrary.UF.RandomPassword.Generate(9, 9, new char[][]
        {
         WelcomeLibrary.UF.RandomPassword.PASSWORD_CHARS_NUMERIC.ToCharArray()
        });
            //CodiceOrdine = "ord_" + CodiceOrdine;

            eCommerceDM ecom = new eCommerceDM();
            esito = ecom.VerificaPresenzaCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceOrdine);
            System.Threading.Thread.Sleep(2000);
        }
#endif

        return CodiceOrdine;
    }

    protected void SalvaCodiceOrdine(Carrello item)
    {
        try
        {
            //salvo  il codice ordine nel db come ordine avvenuto
            if (item != null)
            {
                eCommerceDM ecom = new eCommerceDM();
                ecom.UpdateCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item);
            }
        }
        catch (Exception err)
        {
            output.CssClass = "alert alert-danger"; output.Text = err.Message + " <br/> ";
            switch (Lingua)
            {
                case "I":
                    output.Text = "Errore salvataggio codiceOrdine ";
                    break;
                case "GB":
                    output.Text = "Error save CodiceOrdine";
                    break;
            }
        }
    }

    protected string CreaMailPerFornitore(TotaliCarrello totali, CarrelloCollection prodotti)
    {
        string TestoMail = "";

        //Mi preparo il testo della mail (formattiamo in html)
        TestoMail += "<table cellpadding='0' cellspacing='0'  style='font-size:14px;'><tr><td> Ordine effettuato da " + totali.Denominazionecliente + " tramite sito " + Nome + "  <br/>";
        TestoMail += "<br/>I dati dell' utente sono indirizzo fatturazione : <br/> ";
        TestoMail += totali.Indirizzofatturazione + "<br/>";

        //if (string.IsNullOrEmpty(totali.Indirizzospedizione))
        //{
        TestoMail += "<br/>I dati dell' utente sono indirizzo spedizione : <br/> ";
        TestoMail += totali.Indirizzospedizione;
        //}
        TestoMail += "</td></tr>";
        if (!string.IsNullOrEmpty(totali.Note))
            TestoMail += "<tr><td> <br/>Note : " + totali.Note + "<br/></td></tr>";


        TestoMail += "<tr><td><table cellpadding='0' cellspacing='0' style='font-size:14px;'><tr><td><br/> DETTAGLIO ORDINE <br/></td></tr>";
        TestoMail += "<tr><td> <br/>CODICE ORDINE : " + totali.CodiceOrdine + "<br/></td></tr>";
        int i = 1;
        foreach (Carrello item in prodotti)
        {
            TestoMail += "<tr><td><br/>" + i.ToString() + " - " + item.Offerta.DenominazioneI + "<br/>";

            if (item.Dataend != null && item.Datastart != null)
            {
                TestoMail += "<b>" + references.ResMan("Common", Lingua, "formtestoperiododa") + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Datastart.Value) + "</b> ";
                TestoMail += "<b>" + references.ResMan("Common", Lingua, "formtestoperiodoa") + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Dataend.Value) + "</b><br/> ";
            }
            if (!string.IsNullOrEmpty(item.jsonfield1))
            {
                TestoMail += Selezionadajson(item.jsonfield1, "adulti", Lingua) + "<br/>";
                TestoMail += Selezionadajson(item.jsonfield1, "bambini", Lingua) + "<br/>";
            }
            if (!string.IsNullOrWhiteSpace(item.CodiceProdotto))
                TestoMail += "CODICE PRODOTTO : " + item.CodiceProdotto + "<br/>";
            TestoMail += " ID PRODOTTO : " + item.Offerta.Id.ToString();
            if (!string.IsNullOrEmpty(item.Campo2))
            {
                List<ModelCarCombinate> listCarr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(item.Offerta.Xmlvalue);
                ResultAutocomplete taglia = new ResultAutocomplete();
                ResultAutocomplete colore = new ResultAutocomplete();
                ModelCarCombinate elem = listCarr.Find(e => e.id == item.Campo2);
                if (elem != null)
                    TestoMail += " - " + references.ResMan("BaseText", Lingua, "selectcat1") + " : " + elem.caratteristica1.value + " - " + references.ResMan("BaseText", Lingua, "selectcat2") + " : " + elem.caratteristica2.value;
            }
            TestoMail += " QUANTITA' : " + item.Numero;
            if (item.Prezzo != 0)
                TestoMail += "  Prezzo Unitario : " + item.Prezzo + " €<br/>";

            //QUI POSSIAMO INSERIRE I DETTAGLI SE E' UN PACCHETTO KIT OFFERTA
            //if (item != null && item.Offerta != null && item.Offerta.CodiceTipologia == "rif000101")
            //{
            //    TestoMail += "      Prodotti Contenuti nel pacchetto:<br/>";
            //    List<ResultAutocomplete> listprod = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultAutocomplete>>(item.Offerta.XmlValue);
            //    foreach (ResultAutocomplete p in listprod) //Calcolo il totale non scontato del kit ->
            //    {
            //        TestoMail += "      Codice:" + p.codice + " Descrizione:" + p.value + " Qta:" + p.qta + "<br/>";
            //    }
            //}

            TestoMail += " <br/></td></tr>";
            i++;
        }


        if (totali.TotaleSconto != 0)
            TestoMail += "<tr><td><br/>SCONTO APPLICATO " + totali.TotaleSconto + " € </td></tr>";
        TestoMail += "<tr><td>";
        if (totali.TotaleSpedizione != 0)
            TestoMail += "SPESE DI SPEDIZIONE " + totali.TotaleSpedizione + "  €<br/>";
        if (totali.TotaleSmaltimento != 0)
            TestoMail += "<br/>SPESE DI SMALTIMENTO(PFU) " + totali.TotaleSmaltimento + "  €<br/>";
        TestoMail += "<br/>TOTALE COMPLESSIVO: " + (totali.TotaleSmaltimento + totali.TotaleOrdine + totali.TotaleSpedizione - totali.TotaleSconto) + " €</td></tr>";

        if (totali.Percacconto != 100)
            TestoMail += "<tr><td><br/>RICHIESTO PAGAMENTO ACCONTO " + totali.Percacconto + "%: " + (totali.TotaleSmaltimento + totali.TotaleOrdine + totali.TotaleSpedizione - totali.TotaleSconto) * totali.Percacconto / 100 + " €</td></tr>";


        TestoMail += "<tr><td><br/>MODALITA' DI PAGAMENTO: " + references.ResMan("Common", Lingua, "chk" + totali.Modalitapagamento).ToString() + " </td></tr>";
        //chiudo tabella e riga relativa
        TestoMail += "</table></td><tr/>";
        TestoMail += "<tr><td><br/>L'utente è in attesa di essere ricontattato per confermare la disponibilità e per comunicargli i dettagli del pagamento.";
        TestoMail += "</td></tr></table>";

        return TestoMail;
    }

    protected string CreaMailCliente(TotaliCarrello totali, CarrelloCollection prodotti)
    {
        string TestoMail = "";

        //CAMPI NECESSARI DAL FORM 
        //txtNome.Text- txtEmail.Text  - txtTelefono.Text -  txtIndirizzo.Text - lblPrezzoSpedizione.Text - lblTotaleSpese.Text

        //MAIL PER IL CLIENTE DI CONFERMA ORDINE
        TestoMail = "<div style='width:600px;font-size:14px'><table  style='font-size:14px;' cellpadding='0' cellspacing='0'><tr><td  valign='top'>" + "<img width=\"600\" src=\"" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/main_logo.png\" />" + "</td></tr>";
        TestoMail += "<div style='width:600px;'><table cellpadding='0' cellspacing='0'><tr><td  valign='top'> </td></tr>";
        //Testo mail
        TestoMail += "<tr><td style='font-size:14px;'><br/> " + references.ResMan("Common", Lingua, "OrdineSoggettomailRiepilogo") + "<a href='" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "'>" + Nome + "</a> da " + totali.Denominazionecliente + " <br/>";
        TestoMail += "<br/><font color='#e12222'>Dettaglio Ordine</font> " + "<br/>";

        TestoMail += "<br/>Fatturazione : <br/> ";
        TestoMail += totali.Indirizzofatturazione + "<br/>";

        //if (string.IsNullOrEmpty(totali.Indirizzospedizione))
        //{
        TestoMail += "<br/>Spedizione : <br/> ";
        TestoMail += totali.Indirizzospedizione;
        //}
        TestoMail += "</td></tr>";
        if (!string.IsNullOrEmpty(totali.Note))
            TestoMail += "<tr><td> <br/>Note : " + totali.Note + "<br/></td></tr>";

        TestoMail += "<tr><td><table cellpadding='0' cellspacing='0'>";
        TestoMail += "<tr><td style='font-size:14px;'>CODICE ORDINE : " + totali.CodiceOrdine + "<br/></td></tr>";
        int i = 1;
        foreach (Carrello item in prodotti)
        {
            TestoMail += "<tr><td style=' font-size:14px;'>" + i.ToString() + " - ";
            switch (Lingua)
            {
                case "GB":
                    TestoMail += item.Offerta.DenominazioneGB + "<br/>";
                    break;
                default:
                    TestoMail += item.Offerta.DenominazioneI + "<br/>";
                    break;
            }

            if (item.Dataend != null && item.Datastart != null)
            {
                TestoMail += "<b>" + references.ResMan("Common", Lingua, "formtestoperiododa") + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Datastart.Value) + "</b> ";
                TestoMail += "<b>" + references.ResMan("Common", Lingua, "formtestoperiodoa") + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Dataend.Value) + "</b><br/>";
            }
            if (!string.IsNullOrEmpty(item.jsonfield1))
            {
                TestoMail += Selezionadajson(item.jsonfield1, "adulti", Lingua) + "<br/>";
                TestoMail += Selezionadajson(item.jsonfield1, "bambini", Lingua) + "<br/>";
            }

            if (!string.IsNullOrWhiteSpace(item.CodiceProdotto))
                TestoMail += "Codice Prodotto : " + item.CodiceProdotto + "<br/>";
            TestoMail += "  Id Prodotto : " + item.Offerta.Id.ToString();
            if (!string.IsNullOrEmpty(item.Campo2))
            {
                List<ModelCarCombinate> listCarr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(item.Offerta.Xmlvalue);
                ResultAutocomplete taglia = new ResultAutocomplete();
                ResultAutocomplete colore = new ResultAutocomplete();
                ModelCarCombinate elem = listCarr.Find(e => e.id == item.Campo2);
                if (elem != null)
                    TestoMail += " - " + references.ResMan("BaseText", Lingua, "selectcat1") + " : " + elem.caratteristica1.value + " - " + references.ResMan("BaseText", Lingua, "selectcat2") + " : " + elem.caratteristica2.value;
            }
            TestoMail += " Quantità : " + item.Numero;
            if (item.Prezzo != 0)
                TestoMail += "  Prezzo unitario : " + item.Prezzo + " €<br/>";

            //QUI POSSIAMO INSERIRE I DETTAGLI SE E' UN PACCHETTO KIT OFFERTA
            //if (item != null && item.Offerta != null && item.Offerta.CodiceTipologia == "rif000101")
            //{
            //    TestoMail += "      Prodotti Contenuti nel pacchetto:<br/>";
            //    List<ResultAutocomplete> listprod = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultAutocomplete>>(item.Offerta.XmlValue);
            //    foreach (ResultAutocomplete p in listprod) //Calcolo il totale non scontato del kit ->
            //    {
            //        TestoMail += "      Codice:" + p.codice + " Descrizione:" + p.value + " Qta:" + p.qta + "<br/>";
            //    }
            //}

            TestoMail += "<br/></td></tr>";
            i++;
        }

        if (totali.TotaleSconto != 0)
            TestoMail += "<tr><td><br/>Sconto applicato " + totali.TotaleSconto + " € </td></tr>";
        TestoMail += "<tr><td>";
        if (totali.TotaleSpedizione != 0)
            TestoMail += "<br/>Spese di spedizione " + totali.TotaleSpedizione + " €<br/>";
        if (totali.TotaleSmaltimento != 0)
            TestoMail += "<br/>Spese smaltimento(PFU) " + totali.TotaleSmaltimento + " €<br/>";
        TestoMail += "Totale ordine: " + (totali.TotaleSmaltimento + totali.TotaleOrdine + totali.TotaleSpedizione - totali.TotaleSconto) + " €</td></tr>";

        //La percentuale di anticipo è 100% se la data di inizio periodo ripetto oggi è inferiore a 60 gg
        if (totali.Percacconto != 100)
            TestoMail += "<tr><td><br/>RICHIESTO PAGAMENTO ACCONTO " + totali.Percacconto + "%: " + (totali.TotaleSmaltimento + totali.TotaleOrdine + totali.TotaleSpedizione - totali.TotaleSconto) * totali.Percacconto / 100 + " €</td></tr>";


        TestoMail += "<tr><td><br/>Metodo di pagamento: " + references.ResMan("Common", Lingua, "chk" + totali.Modalitapagamento).ToString() + " </td></tr>";
        //chiudo tabella e riga relativa
        TestoMail += "</table></td><tr/>";
        //testo di chiusura
        TestoMail += "<tr><td style=' font-size:14px;'><br/>" + references.ResMan("Common", Lingua, "TestoConfermaOrdine").ToString() + " </td></tr>";
        TestoMail += "<tr><td style=' font-size:14px;'><br/>" + references.ResMan("Common", Lingua, "TestoSaluti").ToString() + "<br/>" + references.ResMan("Common", Lingua, "TestoHomeIndex").ToString() + "</td></td> <br/>";

        //Inserisco il footer con i dati
        TestoMail += "<tr><td style='text-align:center; font-size:14px;'><br/><br/>" + references.ResMan("Common", Lingua, "txtFooter").ToString();

        TestoMail += "</td></tr></table></div>";

        return TestoMail;
    }



    protected void inpContanti_ServerChange(object sender, EventArgs e)
    {
        CaricaCarrello();
    }

    protected void inpBonifico_ServerChange(object sender, EventArgs e)
    {
        CaricaCarrello();

    }

    protected void inpPaypal_ServerChange(object sender, EventArgs e)
    {
        CaricaCarrello();

    }
}