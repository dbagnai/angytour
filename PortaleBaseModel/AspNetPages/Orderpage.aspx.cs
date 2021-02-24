using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
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
                HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
                metarobots.Attributes["Content"] = "noindex,follow";
                DataBind();

                string conversione = CaricaValoreMaster(Request, Session, "conversione");
                if (conversione == "true")
                {
                    Visualizzarisposta();
                    return;
                }

                if (registrazione != "false") // da abilita se non si vuole far far acquisti senza login ..... di registrazione utente!!!
                    VerificaStatoLoginUtente();
                RiempiDdlNazione("IT", ddlNazione);
                CaricaCarrello();
            }
            else
            {



                if (Request["__EVENTTARGET"] == "recuperapass")
                {
                    recuperapass();
                }
                if (Request["__EVENTTARGET"] == "logoffuser")
                {
                    logoffbtn_Click();
                }
                if (Request["__EVENTTARGET"] == "verificalogin")
                {
                    loginbtn_Click();
                }
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
            case "RU":
                output.Text = "<br/>Order Correctly Sent. <br/>You'll be contacted as soon as possible. " + references.ResMan("Common", Lingua, "GoogleConversione");
                break;
            case "FR":
                output.Text = "<br/>Order Correctly Sent. <br/>You'll be contacted as soon as possible. " + references.ResMan("Common", Lingua, "GoogleConversione");
                break;
        }
    }
    protected void btnCodiceSconto_Click(object sender, EventArgs e)
    {
        string insertedcode = txtCodiceSconto.Text;
        string validcode = ConfigManagement.ReadKey("codicesconto"); //Codicesconto in tabella configurazione

        //Carichiamo i codici sconto dello scaglione
        Dictionary<string, double> codiciscontoscaglione = CercaCodiceScontoSuCarrello(Request, Session);

        //sconto presente su cliente anagrafica
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
        ////facciamo il check dei codici presenti negli scagioni a carrello e verifichiamo se  valido in caso lo inserisco a carrello
        else if (codiciscontoscaglione != null && codiciscontoscaglione.Count > 0)
        {
            //Vediamo se lo sconto è presente tra quelli dello scaglione inserito a carrello
            if (codiciscontoscaglione.ContainsKey(insertedcode))
            {
                Session.Add("codicesconto", insertedcode);
                lblCodiceSconto.Text = "";
            }
        }
        //vediamo se corrisponde al codice sconto presente in configurazione
        else if (insertedcode == validcode)
        {
            Session.Add("codicesconto", validcode);
            lblCodiceSconto.Text = "";
        }
        //Testo Se presente una percentuale tra quelle associate ai commerciali e nel caso prendo quella (PRIORITA')
        else
        {
            Session.Remove("codicesconto");
            txtCodiceSconto.Text = "";
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
        ///////////////////////VERIFICA PER DISTRIBUTORI OBBLIGO DI LOGIN E PRESENZA ID CLIENTE ANAGRAFICA
        usermanager USM = new usermanager();
        string idcliente = getidcliente(User.Identity.Name); //id cliente associato all'utente
        if (string.IsNullOrEmpty(idcliente) && !(USM.ControllaRuolo(Page.User.Identity.Name, "GestorePortale")) && !(USM.ControllaRuolo(Page.User.Identity.Name, "WebMaster")))
        {
            Response.Redirect("~/Error.aspx?Error=Utente non registrato, contattare il supporto per iscriversi al sistema di acquisto.");
        }
        else CaricaDatiCliente();
    }

    /// <summary>
    /// Aggiorna l'associazione del carrello ai dati dell'utente loggato ed eventuale codice sconto in sessione!!
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
        CaricaDatiCliente();
        if (Session["codicesconto"] != null)
            txtCodiceSconto.Text = Session["codicesconto"].ToString();

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

        //Spengo paypal se non riesco a calcolare le spese di spedizione
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //double costospedizionenazione = references.TrovaCostoNazione(codicenazione); //Leggo presenza Costo spedizione per nazione
        //jsonspedizioni js = references.TrovaFascespedizioneNazione(codicenazione);
        //if (costospedizionenazione == 0 && (js == null || js.fascespedizioni == null)) liPaypal.Visible = false;
        //else liPaypal.Visible = true;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //if (codicenazione.ToLower() != "it")
        //    liPaypal.Visible = false;
        //else
        //    liPaypal.Visible = true;


        //SelezionaClientePerAffitti(carrello);//Dedicato alla gestione affitti
        AggiornaDatiUtenteSuCarrello(carrello); //Aggiorno code sconto e idcliente attuale
        VisualizzaTotaliCarrello(codicenazione, "");

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
        //Dal calcolo dei totali e delle spedizioni viene indicato di bloccare l'acquisto diretto
        if (totali.Bloccaacquisto)
            liPaypal.Visible = false;
        else
            liPaypal.Visible = true;

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
            if (!CaricaDatiClienteDaForm(cliente)) return; //se la verifica cliente fallisce stoppo tutto

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
                ///


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
                totali.Indirizzofatturazione += cliente.Cap + " " + cliente.CodiceCOMUNE + "  (" + ((!(string.IsNullOrWhiteSpace(NomeProvincia(cliente.CodicePROVINCIA, Lingua)))) ? NomeProvincia(cliente.CodicePROVINCIA, Lingua) : cliente.CodicePROVINCIA) + ")<br/>";
                totali.Indirizzofatturazione += "Nazione: " + cliente.CodiceNAZIONE + "<br/>";
                totali.Indirizzofatturazione += "Telefono: " + cliente.Telefono + "<br/>";
                totali.Indirizzofatturazione += "P.Iva: " + cliente.Pivacf + "<br/>";
                totali.Indirizzofatturazione += "CodiceDestinatario/Pec: " + cliente.Emailpec + "<br/>";

                //SE INDIRIZZO SPEDIIZONE DIVERSO -> LO MEMORIZZO NEI TOTALI ( E serializzo il dettaglio nel cliente nel campo serialized )
                string indirizzospedizione = "";
                if (!chkSpedizione.Checked)
                {
                    if (!string.IsNullOrEmpty(inpIndirizzoS.Value))
                        indirizzospedizione = inpIndirizzoS.Value + "<br/>";
                    if (!string.IsNullOrEmpty(inpCaps.Value) && !string.IsNullOrEmpty(inpComuneS.Value) && !string.IsNullOrEmpty(inpProvinciaS.Value))
                    {
                        indirizzospedizione += inpCaps.Value + " " + inpComuneS.Value + "  (" + ((!(string.IsNullOrWhiteSpace(NomeProvincia(inpProvinciaS.Value, Lingua)))) ? NomeProvincia(inpProvinciaS.Value, Lingua) : inpProvinciaS.Value) + ")<br/>";
                        indirizzospedizione += "Nazione: " + cliente.CodiceNAZIONE + "<br/>";
                    }
                    if (!string.IsNullOrEmpty(inpTelS.Value))
                        indirizzospedizione += "Telefono: " + inpTelS.Value + "<br/>";

                    totali.Indirizzospedizione = indirizzospedizione;
                }
#if true   // con questa visualizza sempre la spedizione
                if (string.IsNullOrWhiteSpace(indirizzospedizione))
                {
                    totali.Indirizzospedizione = totali.Indirizzofatturazione;
                } 
#endif
                totali.Note = inpNote.Value;
                totali.Modalitapagamento = modalita;

                //Valorizzato solo alla ricezione del pagamento prima della spedizione o tramite la procedura con pagamento anticipato
                //totali.Pagato = false;
                if (totali.Percacconto == 100)
                { totali.Pagato = false; totali.Pagatoacconto = false; } // da capire se acconto è zero se vogliamo spuntare pagato acconto!
                else
                { totali.Pagato = false; totali.Pagatoacconto = false; }


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
                    string costo = string.Format(culture, "{0:0}", new object[] { Convert.ToDouble((totali.TotaleAcconto + totali.TotaleSaldo)) * 100 }); //Il totale da passare è in centesimi di euro
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
                        Utility.invioMailGenerico(Nome, Email, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);
                        //Utility.invioMailGenerico(totali.Denominazionecliente, totali.Mailcliente, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);

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
#if true
                        try
                        {
                            int J = 0;
                            J++;
                            if (J <= 2)
                            {
                                Mail mailfeedback = new Mail();

                                mailfeedback.Sparedict["linkfeedback"] = "";//default preso dalle risorse feedbacksdefaultform
                                mailfeedback.Sparedict["idnewsletter"] = "";//default dalle risorse feedbackdefaultnewsletter
                                mailfeedback.Sparedict["deltagiorniperinvio"] = "";//default dalle risorse feedbacksdefaultdeltagg
                                mailfeedback.Sparedict["idclienti"] = cliente.Id_cliente.ToString();
                                mailfeedback.Id_card = item.id_prodotto;
                                HandlerNewsletter.preparamail(mailfeedback, Lingua); //Preparo le mail nello scheduler!!
                            }
                        }
                        catch { }
#endif
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        item.CodiceOrdine = CodiceOrdine;
                        SalvaCodiceOrdine(item);
                    }
                    ////////////////////////////////////////
                    //Aggiorniamo lo stato degli scaglioni caricati per i prodotti se presenti
                    ////////////////////////////////////////
                    string listcod = "";
                    prodotti.ForEach(item => listcod += item.id_prodotto + ",");
                    Dictionary<string, string> parametri = new Dictionary<string, string>();
                    parametri["idprodotto"] = listcod;
                    ecom.AggiornaStatoscaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parametri);
                    ////////////////////////////////////////

                    InsertEventoBooking(prodotti, totali, "rif000001");

                    pnlFormOrdine.Visible = false;
                    string jscodetoinject = Creaeventopurchaseagooglegtag(totali, prodotti);

                    //Pulizia dell'ordine dalla sesoione
                    Session.Remove("cliente_" + CodiceOrdine);
                    Session.Remove("totali_" + CodiceOrdine);
                    Session.Remove("prodotti_" + CodiceOrdine);
                    //CreaNuovaSessione(Session, Request); //Svuota la session per un nuovo ordine!!
                    output.Text += jscodetoinject;
                    output.Text += references.ResMan("Common", Lingua, "GoogleConversione");

                    switch (Lingua)
                    {
                        case "I":
                            output.Text += references.ResMan("Common", Lingua, "risposta_5");

                            break;
                        default:
                            output.Text += references.ResMan("Common", Lingua, "risposta_5");
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
    //public void MemorizzaClientePerNewsletter(Cliente cli)
    //{
    //    try
    //    {
    //        //------------------------------------------------
    //        //Memorizzo i dati nel cliente per la newsletter
    //        //------------------------------------------------
    //        ClientiDM cliDM = new ClientiDM();
    //        string lingua = Lingua;
    //        string tipocliente = "0"; //Cliente standard per newsletter
    //        cli.DataNascita = System.DateTime.Now.Date;
    //        cli.Lingua = lingua;
    //        cli.id_tipi_clienti = tipocliente;
    //        cli.Consenso1 = true;
    //        cli.ConsensoPrivacy = true;
    //        cli.Validato = true;
    //        cli.Email = cli.Email.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
    //        cli.Emailpec = cli.Emailpec.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
    //        Cliente ctmp = new Cliente(cli);
    //        Cliente cliindb = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email, tipocliente); //carico per email ma dalla tipologia cliente di default (0)
    //        if ((cliindb != null && cliindb.Id_cliente != 0)) //cleiinte per la tipologia gia presente in db
    //        {
    //            //aggiorno solo i  campi indicati
    //            ctmp = new Cliente(cliindb);
    //            //ctmp.Lingua = cli.Lingua;
    //            //ctmp.id_tipi_clienti = cli.id_tipi_clienti;
    //            ctmp.Consenso1 = cli.Consenso1;
    //            ctmp.ConsensoPrivacy = cli.ConsensoPrivacy;
    //            ctmp.Validato = cli.Validato;
    //            ctmp.Email = cli.Email;
    //            ctmp.Emailpec = cli.Emailpec;
    //        }
    //        cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref ctmp);
    //        cli.Id_cliente = ctmp.Id_cliente;
    //    }
    //    catch
    //    { }
    //}

    /// <summary>
    /// carica i dati dal form cliente e aggiona il database clienti se presenti clienti corrispondenti in anagrafica con mail coincidente
    /// se non presente nel database inserisce il cliente nella tipologia 0
    /// </summary>
    /// <param name="cliente"></param>
    private bool CaricaDatiClienteDaForm(Cliente cliente)
    {
        bool ret = true;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //MEMORIZZO I DATI DI SPEDIZIONE DI DETTAGLIO IN UN CLIENTE TEMPORANEO AD HOC CHE SERIALIZZO IN UN CAMPO CLIENTE
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Cliente clispediz = new Cliente();
        clispediz.CodiceNAZIONE = ddlNazione.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(inpCaps.Value.Trim()))
            clispediz.Cap = inpCaps.Value.Trim();
        if (!string.IsNullOrEmpty(inpComuneS.Value.Trim()))
            clispediz.CodiceCOMUNE = inpComuneS.Value.Trim();
        if (!string.IsNullOrEmpty(inpProvinciaS.Value.Trim()))
            clispediz.CodicePROVINCIA = inpProvinciaS.Value.Trim();
        if (!string.IsNullOrEmpty(inpIndirizzoS.Value.Trim()))
            clispediz.Indirizzo = inpIndirizzoS.Value.Trim();
        if (!string.IsNullOrEmpty(inpTelS.Value.Trim()))
            clispediz.Telefono = inpTelS.Value.Trim();
        //Proviamo a cercare i codici geografici e settarli in base al testo
        SearchGeoCodesByText(clispediz);

        string datispedizione = Newtonsoft.Json.JsonConvert.SerializeObject(clispediz);
        cliente.Serialized = datispedizione; //Appoggio i dati di spedizione in Serialized del cliente !!!
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Dati base cliente da form
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        cliente.Cognome = inpCognome.Value;
        cliente.Nome = inpNome.Value;
        cliente.Ragsoc = inpRagsoc.Value;

        //if (inpRagsoc.Value != "")
        //{
        //    if (inpCognome.Value != inpRagsoc.Value)
        //        cliente.Nome += " " + cliente.Cognome;
        //    cliente.Cognome = inpRagsoc.Value;
        //}
        cliente.Email = inpEmail.Value.Trim();
        cliente.Emailpec = inpPec.Value.Trim();
        cliente.Pivacf = inpPiva.Value.Trim();
        cliente.Indirizzo = inpIndirizzo.Value.Trim();
        cliente.CodiceNAZIONE = ddlNazione.SelectedValue.Trim();
        cliente.CodiceCOMUNE = inpComune.Value.Trim();
        cliente.CodicePROVINCIA = inpProvincia.Value.Trim();
        cliente.Cap = inpCap.Value.Trim();
        cliente.Telefono = inpTel.Value.Trim();
        SearchGeoCodesByText(cliente);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////////
        //RICARICAMENTO E VERIFICA PRESENZA UTENTE NEL DB E/O STATO LOGGATO
        //Qui procedo ad aggiornare i dati in TBL_CLIENTI con quelli inseriti nel form
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        usermanager USM = new usermanager();
        ClienteCollection clifiltrati = new ClienteCollection();
        ClientiDM cliDM = new ClientiDM();
        Cliente clitmp = new Cliente();
        string tipologiaclientiecommece = "0"; //LA TIPOLOGIA USATA PER I CLIENTI ECOMMERCE
        clitmp.Email = cliente.Email;
        clitmp.id_tipi_clienti = tipologiaclientiecommece; //ricerco per email SOLO SULLA TIPOLOGIA CLIENTI SPECIFICATA  
        //clitmp.id_tipi_clienti = ""; //ricerco su tutte le tipologie non su una sola

        ////////////////////////////////////////////////////
        // cerco in anagrafica cliente con la mail/tipologia inserita
        ////////////////////////////////////////////////////
        Cliente clienteinanagraficaperemail = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, clitmp.Email, clitmp.id_tipi_clienti);
        bool generazioneutente = false;
        if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name)) //UTENTE MEMBERSHIP LOGGATO   -> VERIFICHE SU DATI INSERITI E AGGIONAMENTO DATI CLIENTE
        {
            //PER PRIMA PRENDO IL CLIENTE CORRISPONDENTE PER L'UTENTE LOGGATO SE PRESENTE
            //(IL CLIENTE LOGGATO E' PRIMARIO RISPETTO AL QUELLO ASSOCIATO ALLA MAIL INSERITA!!)
            string idcliente = getidcliente(User.Identity.Name); //prendo l'id anagrafica associato al cliente loggato ( se disponibile )
            Cliente cbyid = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcliente); //prende il cliente per idcliente con qualsiasi tipologia questo avesse
            if (cbyid == null || cbyid.Id_cliente == 0) //non presente cliente anagrafica associato alla login
            {
                //Utente loggato ma non ha nessun cliente associato (anomalia !) ( blocco l'ordine )
                output.CssClass = "alert alert-danger"; output.Text = "Errore. Non trovato utenza associata alla login attuale, cambiare utente o registrarsi per procedere!"; //output.Text = references.ResMan("Common", Lingua, "txtPagamento").ToString();
                return false;
            }
            else
            {
                if (clienteinanagraficaperemail != null && clienteinanagraficaperemail.Id_cliente != cbyid.Id_cliente)
                {
                    //Presente altro cliente in anagrafica con quella mail/tipologia inserita nel form non coincidente con il cliente loggato !!!!!
                    output.CssClass = "alert alert-danger";
                    //output.Text = "Errore. La mail inserita è già in anagrafica associata ad altro utente, recuperare la login o inserire altra mail per procedere!";
                    output.Text = references.ResMan("Common", Lingua, "txterrconincidenzaemail").ToString();
                    return false;
                }
                clifiltrati.Add(cbyid); //procedo aggiornando il cliente associato a quello loggato per l'aggiornamento dei dati dal form acquisto
            }

        }
        else  //UTENTE NON LOGGATO -> AGGIORNAMENTO + VERIFICA GENERAZIONE UTENTE MEMBERSHIP
        {
            if (clienteinanagraficaperemail != null && clienteinanagraficaperemail.Id_cliente != 0) //Presente in anagrafica cliente con la mail inserita nel form e ma utente attuale non loggato
            {
                clifiltrati.Add(clienteinanagraficaperemail);//aggiorno il cliente caricato per email  
                //DEVO CERCARE SE IL CLIENTE ASSOCIATO HA UN UTENTE CORRISPONDENTE NEL MEMBERSHIP IN CASO NEGATIVO INSERISCO L'UTENTE NUOVO
                string username = USM.GetUsernamebycamporofilo("idCliente", clienteinanagraficaperemail.Id_cliente.ToString());
                if (string.IsNullOrEmpty(username)) //non presente utente associato al cliente! -> lo devo generare ex novo
                    generazioneutente = true;
            }
            //Se non presente utente con la mail/tipologia indicata viene inserito e genero l'utente corrispondente
            else generazioneutente = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //INSERIMENTO O AGGIORNAMENTO CLIENTE ANAGRAFICA
        //////////////////////////////////////////////////////////////////////////////////////////////
        if (clifiltrati != null && clifiltrati.Count > 0) //trovato cliente corrispondente nel database da aggiornare ( aggiorno solo i campi specifici dal form acquisto )
        {
            //Su tutti i clienti trovati nel db aggiono i campi non modificando gli altri
            foreach (Cliente c in clifiltrati)
            {
                Cliente ctmp = new Cliente(c);
                //Aggiorno nel db solo i campi che sono nel form
                ctmp.CodiceNAZIONE = cliente.CodiceNAZIONE;
                ctmp.CodiceCOMUNE = cliente.CodiceCOMUNE;
                ctmp.CodicePROVINCIA = cliente.CodicePROVINCIA;
                ctmp.CodiceREGIONE = (!string.IsNullOrEmpty(cliente.CodiceREGIONE) ? cliente.CodiceREGIONE : ctmp.CodiceREGIONE);//Se vuoto mantengo il vecchio valore
                ctmp.Nome = cliente.Nome;
                ctmp.Cognome = cliente.Cognome;
                ctmp.Ragsoc = cliente.Ragsoc;
                ctmp.Email = cliente.Email;
                ctmp.Emailpec = cliente.Emailpec;
                ctmp.Pivacf = cliente.Pivacf;
                ctmp.Indirizzo = cliente.Indirizzo;
                ctmp.Cap = cliente.Cap;
                ctmp.Telefono = cliente.Telefono;
                ctmp.Serialized = cliente.Serialized; //Dati serializzati aggiuntivi
                cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref ctmp);
                cliente.Id_cliente = ctmp.Id_cliente;
            }
        }
        else //NON TROVATO ALCUN cliente con quella mail nella tipologia cercata  -> lo inserisco NUOVO nel database 
        {
            //MEMORIZZO CON LINGUA E CONSENSI ATTIVI PER NEWSLETTER E STORICIZZAZIONE CLIENTE
            //cliente.DataNascita = System.DateTime.Now.Date;
            cliente.DataRicezioneValidazione = System.DateTime.Now.Date;
            cliente.Lingua = Lingua;
            cliente.Consenso1 = true;
            cliente.ConsensoPrivacy = true;
            cliente.Validato = true;
            cliente.id_tipi_clienti = tipologiaclientiecommece; // tipologia impostata per ecommerce
            cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cliente);
        }

#if false //abilitare per generazione utente all'acquisto
        //////////////////////////////////////////////////////////////////////////////////////////////
        //SE richiesto GENERIAMO L'UTENTE NEL MEMBERSHIP E INVIAMO UNA MAIL DI CONFERMA AL CLIENTE CON I DATI DI REGISTRAZIONE
        //////////////////////////////////////////////////////////////////////////////////////////////
        if (generazioneutente)
        {
            string password = "";
            string username = cliente.Id_cliente.ToString() + "-" + cliente.Email;
            if (USM.VerificaPresenzaUtente(username)) //controllo se utente esistente -> devo avvisare che presente!!!
            {
                output.Text = "Utente " + username + " già registrato! Fare recupero della password o contattate il supporto per il recupero dell'acesso!";
                //devo avvisare l'utente!
            }
            else //utente da creare ex novo
            {
                USM.CreaUtente(cliente.Id_cliente.ToString(), ref username, ref password, "Operatore");
                ///////////////////////////////////////////////////////////
                //INVIA MAIL REGISTRAZIONE ALL'UTENTE
                string oggetto = references.ResMan("Common", Lingua, "txtoggettocreateuser").ToString();
                oggetto += " " + Nome;
                string testo = references.ResMan("Common", Lingua, "txttestocreateuser").ToString();
                testo = testo.Replace("|NOME|", Nome);
                testo = testo.Replace("|CREDENZIALI|", "User: " + username + " Pass: " + password);
                testo = testo.Replace("|LOGINPAGE|", "<a href=" + ReplaceAbsoluteLinks(references.ResMan("Common", "I", "linklogin")) + ">" + ReplaceAbsoluteLinks(references.ResMan("Common", "I", "linklogin")) + "</a>");
                testo += references.ResMan("Common", "I", "txtFooter").ToString();
                Utility.invioMailGenerico(Nome, Email, oggetto, testo, cliente.Email, cliente.Cognome, null, "", true, Server);
                ///////////////////////////////////////////////////////////
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////  
#endif

        return ret;
    }

    /// <summary>
    /// Ricerca i codici geografici dal testo inserito nel campi cliente in caso di italia
    /// </summary>
    /// <param name="item"></param>
    private void SearchGeoCodesByText(Cliente item)
    {
        //Se codice nazione = it Fare la ricerca per comune / provincia -> inserire codice regione 
        if (item.CodiceNAZIONE.ToLower() == "it")
        {
            // cerchiamo la provincia prima per codice
            string nomeprovincia = references.NomeProvincia(item.CodicePROVINCIA, Lingua);
            if (string.IsNullOrEmpty(nomeprovincia)) //non trovata per codice
            {
                //Cerco per nome della provincia  
                string codiceprovincia = references.TrovaCodiceProvincia(item.CodicePROVINCIA, Lingua);
                //Cerco per sigla della provincia
                if (string.IsNullOrEmpty(codiceprovincia)) codiceprovincia = references.TrovaCodiceProvinciaPerSigla(item.CodicePROVINCIA, Lingua);
                //se trovato il codice lo setto 
                if (!string.IsNullOrEmpty(codiceprovincia)) item.CodicePROVINCIA = codiceprovincia;
            }
            Province provincia = Utility.ElencoProvince.Find(p => p.Lingua == Lingua && p.Codice == item.CodicePROVINCIA);

            //Cerco il comune per nome e da li setto la provincia se non trovata sopra
            Comune comune = Utility.ElencoComuni.Find(delegate (WelcomeLibrary.DOM.Comune tmp) { return (tmp.Nome.ToLower().Trim() == item.CodiceCOMUNE.ToLower().Trim()); });
            if (comune != null && !string.IsNullOrEmpty(comune.Nome))
            {
                if (provincia == null) //se non trovata la provincia provo a settarla in base al comune
                {
                    item.CodicePROVINCIA = comune.CodiceIncrocio;
                    provincia = Utility.ElencoProvince.Find(p => p.Lingua == Lingua && p.Codice == item.CodicePROVINCIA);
                }
            }
            //Riempiamo il valore della regione se trovata la corretta provincia
            //Lista completa regioni
            ProvinceCollection regioni = references.ListaRegioni(Lingua);
            if (regioni != null && provincia != null)
            {
                Province regione = regioni.Find(r => r.Lingua == Lingua && r.CodiceRegione == provincia.CodiceRegione);
                if (regione != null) item.CodiceREGIONE = regione.Codice;
            }
        }
        return;
    }

    protected bool ControlloLogin()
    {
        bool ret = true;
        if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
            ret = false;
        return ret;
    }

    protected void recuperapass()
    {
        usermanager USM = new usermanager();
        string username = inputName.Value;
        outputlogin.Text = USM.SendAccessData(Lingua, username, Email, Nome);
    }
    protected void logoffbtn_Click()
    {
        FormsAuthentication.SignOut();

        Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString());
    }
    protected void loginbtn_Click()
    {
        string username = inputName.Value;
        string password = inputPassword.Value;
        if (Membership.ValidateUser(username, password))
        {
            //FormsAuthentication.LoginUrl = references.ResMan("Common",Lingua,"Linklogin");
            //FormsAuthentication.DefaultUrl
            //FormsAuthentication.RedirectFromLoginPage(username, false);
            //FormsAuthentication.Authenticate(username, password);
            FormsAuthentication.SetAuthCookie(username, false);
            Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString());
            outputlogin.Text = "Accesso riuscito.";
            CaricaCarrello();
        }
        else
        {
            outputlogin.Text = "Accesso non riuscito. Se sei un nuovo utente, effettua la registrazione.";
        }
    }

    /// <summary>
    /// Aggiorna i dati del cliente nel form visualizzato in base ai dati presenti nell'archivio clienti
    /// ( Per i clienti loggati che quindi hanno un utente associato per lo login )
    /// </summary>
    private void CaricaDatiCliente()
    {
        string idcliente = getidcliente(User.Identity.Name); //prendo l'id associato al cliente loggato
        ClientiDM cliDM = new ClientiDM();
        Cliente c = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcliente);
        //Riempiamo il form dei dati cliente dal database
        if (c != null && c.Id_cliente != 0)
        {
            //////////////////////////////////////////////////////////////////////////////////
            //se presente sul cliente loggato setto il codice sconto ( il primo che trovo ) e lo applico
            //////////////////////////////////////////////////////////////////////////////////
            Dictionary<string, double> dict = ClientiDM.SplitCodiciSconto(c.Codicisconto);
            if (dict != null && dict.Count > 0)
            {
                foreach (KeyValuePair<string, double> kv in dict)
                {
                    txtCodiceSconto.Text = kv.Key;
                    Session.Add("codicesconto", kv.Key);
                    txtCodiceSconto.Enabled = false;
                    break;
                }
            }
            //////////////////////////////////////////////////////////////////////////////////
            //VISUALIZZO I DATI DEL CLIENTE LETTI DAL DB
            ///////////////////////////////////////////////////////////
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
            //if (string.IsNullOrWhiteSpace(inpRagsoc.Value))
            //{
            //    inpRagsoc.Value = c.Cognome;
            //}
            if (string.IsNullOrWhiteSpace(inpRagsoc.Value))
                inpRagsoc.Value = c.Ragsoc;
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

            //Precaricare dal db anche i campi per la spedizione da .serialized
            Cliente clispediz = Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(c.Serialized);
            if (clispediz != null)
            {
                if (string.IsNullOrWhiteSpace(inpCaps.Value))
                    inpCaps.Value = clispediz.Cap;
                if (string.IsNullOrWhiteSpace(inpComuneS.Value))
                    inpComuneS.Value = clispediz.CodiceCOMUNE;
                //if (string.IsNullOrWhiteSpace(inpProvinciaS.Value))
                //    inpProvinciaS.Value = clispediz.CodicePROVINCIA;
                if (string.IsNullOrWhiteSpace(inpProvinciaS.Value))
                    inpProvinciaS.Value = (!(string.IsNullOrWhiteSpace(NomeProvincia(clispediz.CodicePROVINCIA, Lingua)))) ? NomeProvincia(clispediz.CodicePROVINCIA, Lingua) : clispediz.CodicePROVINCIA;
                if (string.IsNullOrWhiteSpace(inpIndirizzoS.Value))
                    inpIndirizzoS.Value = clispediz.Indirizzo;
                if (string.IsNullOrWhiteSpace(inpTelS.Value))
                    inpTelS.Value = clispediz.Telefono;
            }

            //Abilita i dati spedizione se presenti
            if (!string.IsNullOrEmpty(inpCaps.Value) || !string.IsNullOrEmpty(inpComuneS.Value) || !string.IsNullOrEmpty(inpProvinciaS.Value)
            || !string.IsNullOrEmpty(inpIndirizzoS.Value) || !string.IsNullOrEmpty(inpTelS.Value))
            {
                chkSpedizione.Checked = false; plhShipping.Visible = true;
            }
        }
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
                case "RU":
                    output.Text = "Error save CodiceOrdine";
                    break;
                case "FR":
                    output.Text = "Error save CodiceOrdine";
                    break;
            }
        }
    }

    protected string CreaMailPerFornitore(TotaliCarrello totali, CarrelloCollection prodotti)
    {
        string TestoMail = "";
        //Mi preparo il testo della mail (formattiamo in html)
        TestoMail += "<table cellpadding='0' cellspacing='0'  style='font-size:14px;'>";
        TestoMail += "<tr><td> Ordine effettuato da " + totali.Denominazionecliente + " tramite sito " + Nome + "  <br/>";
        TestoMail += "<br/><b>EMAIL CLIENTE :</b>  <br/>";
        TestoMail += totali.Mailcliente;
        TestoMail += "<br/><b>Indirizzo fatturazione</b> : <br/> ";
        TestoMail += totali.Indirizzofatturazione + "<br/>";
        if (!string.IsNullOrEmpty(totali.Indirizzospedizione))
        {
            TestoMail += "<br/><b>Indirizzo spedizione</b> : <br/> ";
            TestoMail += totali.Indirizzospedizione;
        }
        TestoMail += "</td></tr>";
        if (!string.IsNullOrEmpty(totali.Note))
            TestoMail += "<tr><td> <br/>Note : " + totali.Note + "<br/></td></tr>";
        TestoMail += "<tr><td><table cellpadding='0' cellspacing='0' style='font-size:14px;'><tr><td><br/> <b>DETTAGLIO ORDINE</b> </td></tr>";
        TestoMail += "<tr><td> <br/><b>CODICE ORDINE</b> : " + totali.CodiceOrdine + "<br/></td></tr>";
        int i = 1;
        foreach (Carrello item in prodotti)
        {
            TestoMail += "<tr><td><br/><b>Articolo:</b> " + item.Offerta.DenominazioneI + "<br/>";
            //DATE A CARRELLO SE PRESENTI
            if (item.Dataend != null && item.Datastart != null)
            {
                TestoMail += "<b>" + references.ResMan("Common", Lingua, "formtestoperiododa") + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Datastart.Value) + "</b> ";
                TestoMail += "<b>" + references.ResMan("Common", Lingua, "formtestoperiodoa") + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Dataend.Value) + "</b><br/> ";
            }
            //CARATTERISTICHE CARRELLO IN BASE ALLE PROPRIETA IN jsonfield1
            if (!string.IsNullOrEmpty(item.jsonfield1))
            {
                string valore3 = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "adulti", Lingua);
                string valore4 = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "bambini", Lingua);
                if (!string.IsNullOrEmpty(valore3))
                    TestoMail += "<br/>" + "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "adulti") + ": " + "</b>" + valore4 + "<br/>";
                if (!string.IsNullOrEmpty(valore4))
                    TestoMail += " " + "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "bambini") + ": " + "</b>" + valore4 + "<br/>";
            }
            //CARATTERISTICHE
            string valore1 = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "Caratteristica1", Lingua);
            string valore2 = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "Caratteristica2", Lingua);
            if (!string.IsNullOrEmpty(valore1) || !string.IsNullOrEmpty(valore2))
            {
                valore1 = references.TestoCaratteristica(0, valore1, Lingua);
                if (!string.IsNullOrEmpty(valore1))
                    TestoMail += " <b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica1") + ": " + "</b>" + valore1;
                valore2 = references.TestoCaratteristica(1, valore2, Lingua);
                if (!string.IsNullOrEmpty(valore2))
                    TestoMail += " <b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica2") + ": " + "</b>" + valore2;
            }
            //ALTRI VALORI DA jsonfield1
            //////////////////////////////////
            //INSERIMENTO DATI PER SCAGLIONI
            //////////////////////////////////
            //string prezzoscaglione = eCommerceDM.Selezionadajson(item.jsonfield1, "prezzo", Lingua);
            string datapartenza = WelcomeLibrary.UF.Utility.reformatdatetimestring((string)eCommerceDM.Selezionadajson(item.jsonfield1, "datapartenza", Lingua));
            string dataritorno = WelcomeLibrary.UF.Utility.reformatdatetimestring((string)eCommerceDM.Selezionadajson(item.jsonfield1, "dataritorno", Lingua));

            string idscaglione = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "idscaglione", Lingua);
            //scaglione completo nel carrello
            //Scaglioni scaglionedacarrello = Newtonsoft.Json.JsonConvert.DeserializeObject<Scaglioni>((String)eCommerceDM.Selezionadajson(item.jsonfield1, "scaglione", Lingua));
            if (!string.IsNullOrEmpty(idscaglione) || !string.IsNullOrEmpty(datapartenza))
            {
                if (!string.IsNullOrEmpty(datapartenza))
                    TestoMail += "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedata") + "</b>" + datapartenza + "<br/>";
                if (!string.IsNullOrEmpty(idscaglione))
                    TestoMail += "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedataritorno") + "</b>" + dataritorno + "<br/>";
                if (!string.IsNullOrEmpty(idscaglione))
                    TestoMail += "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglioneid") + "</b>" + idscaglione + "<br/>";
            }
            if (!string.IsNullOrWhiteSpace(item.CodiceProdotto))
                TestoMail += "<b>Codice Articolo : </b>" + item.CodiceProdotto + "<br/>";
            TestoMail += "<b>Id articolo :</b> " + item.Offerta.Id.ToString() + "<br/>";
            if (!string.IsNullOrEmpty(item.Campo2))
            {
                List<ModelCarCombinate> listCarr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(item.Offerta.Xmlvalue);
                ResultAutocomplete taglia = new ResultAutocomplete();
                ResultAutocomplete colore = new ResultAutocomplete();
                ModelCarCombinate elem = listCarr.Find(e => e.id == item.Campo2);
                if (elem != null)
                    TestoMail += " - " + references.ResMan("BaseText", Lingua, "selectcat1") + " : " + elem.caratteristica1.value + " - " + references.ResMan("BaseText", Lingua, "selectcat2") + " : " + elem.caratteristica2.value + "<br/>";
            }
            TestoMail += "<b> Quantità :</b> " + item.Numero + "<br/>";
            if (item.Prezzo != 0)
                TestoMail += "<b>  Prezzo Unitario :</b> " + item.Prezzo + " €<br/>";

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
        TestoMail += "<tr><td>Totale articoli " + totali.TotaleOrdine + " € </td></tr>";
        if (totali.TotaleSconto != 0)
            TestoMail += "<tr><td>Sconto applicato " + totali.TotaleSconto + " € </td></tr>";
        TestoMail += "<tr><td>";
        if (totali.TotaleSpedizione != 0)
            TestoMail += "Spese di spedizione " + totali.TotaleSpedizione + "  €<br/>";
        if (totali.TotaleSmaltimento != 0)
            TestoMail += "<br/>Spese di smaltimeto(PFU) " + totali.TotaleSmaltimento + "  €<br/>";
        TestoMail += "<br/><b>TOTALE ORDINE COMPLESSIVO: </b>" + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";
        if (totali.Percacconto != 100)
        {
            TestoMail += "<tr><td><b>RICHIESTO PAGAMENTO ACCONTO " + totali.Percacconto + "% :</b> " + totali.TotaleAcconto + " €</td></tr>";
            TestoMail += "<tr><td><b>DA SALDARE ENTRO 30 GG DATA PARTENZA " + ":</b> " + totali.TotaleSaldo + " €</td></tr>";
        }
        else
            TestoMail += "<tr><td><b>RICHIESTO PAGAMENTO SALDO  :</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

        TestoMail += "<tr><td><br/>Metodo di pagamento:  " + references.ResMan("Common", Lingua, "chk" + totali.Modalitapagamento).ToString() + " </td></tr>";
        //chiudo tabella e riga relativa
        TestoMail += "</table></td><tr/>";
        TestoMail += "<tr><td><br/>Contattare l'utente per la verifica del pagamento prescelto e le informazioni sulla spedizione e le tempistiche.";
        TestoMail += "</td></tr></table>";

        return TestoMail;
    }

    protected string CreaMailCliente(TotaliCarrello totali, CarrelloCollection prodotti)
    {
        string TestoMail = "";
        //MAIL PER IL CLIENTE DI CONFERMA ORDINE
        TestoMail = "<div style='width:600px;font-size:14px'><table  style='font-size:14px;' cellpadding='0' cellspacing='0'><tr><td  valign='top'>" + "<img width=\"200\" src=\"" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/main_logo.png\" />" + "</td></tr>";
        TestoMail += "<div style='width:600px;'><table cellpadding='0' cellspacing='0'><tr><td  valign='top'> </td></tr>";
        //Testo mail
        TestoMail += "<tr><td style='font-size:14px;'><br/> " + references.ResMan("Common", Lingua, "OrdineSoggettomailRiepilogo") + "<a href='" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "'>" + Nome + "</a> da " + totali.Denominazionecliente + " <br/>";
        TestoMail += "<br/><font color='#e12222'>Dettaglio Ordine</font> " + "<br/>";

        TestoMail += "<br/><b>Fatturazione</b> :<br/> ";
        TestoMail += totali.Indirizzofatturazione + "<br/>";
        if (!string.IsNullOrEmpty(totali.Indirizzospedizione))
        {
            TestoMail += "<br/><b>Spedizione</b> :<br/>";
            TestoMail += totali.Indirizzospedizione;
        }
        TestoMail += "</td></tr>";
        if (!string.IsNullOrEmpty(totali.Note))
            TestoMail += "<tr><td> <br/>Note : " + totali.Note + "<br/></td></tr>";

        TestoMail += "<tr><td><table cellpadding='0' cellspacing='0'>";
        TestoMail += "<tr><td style='font-size:14px;'><br/><br/><b>CODICE ORDINE :</b> " + totali.CodiceOrdine + "<br/><br/></td></tr>";
        int i = 1;
        foreach (Carrello item in prodotti)
        {
            TestoMail += "<tr><td style=' font-size:14px;'><b>Articolo:</b> ";
            switch (Lingua)
            {
                case "I":
                    TestoMail += item.Offerta.DenominazioneI + "<br/>";
                    break;
                case "GB":
                    TestoMail += item.Offerta.DenominazioneGB + "<br/>";
                    break;
                case "RU":
                    TestoMail += item.Offerta.DenominazioneRU + "<br/>";
                    break;
                case "FR":
                    TestoMail += item.Offerta.DenominazioneFR + "<br/>";
                    break;
            }

            if (item.Dataend != null && item.Datastart != null)
            {
                TestoMail += "<b>" + references.ResMan("Common", Lingua, "formtestoperiododa") + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Datastart.Value) + "</b> ";
                TestoMail += "<b>" + references.ResMan("Common", Lingua, "formtestoperiodoa") + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", item.Dataend.Value) + "</b><br/>";
            }
            string valore1 = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "Caratteristica1", Lingua);
            string valore2 = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "Caratteristica2", Lingua);
            if (!string.IsNullOrEmpty(valore1) || !string.IsNullOrEmpty(valore2))
            {
                valore1 = references.TestoCaratteristica(0, valore1, Lingua);
                if (!string.IsNullOrEmpty(valore1))
                    TestoMail += " <b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica1") + ": " + "</b>" + valore1;
                valore2 = references.TestoCaratteristica(1, valore2, Lingua);
                if (!string.IsNullOrEmpty(valore2))
                    TestoMail += " <b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica2") + ": " + "</b>" + valore2;
            }

            if (!string.IsNullOrEmpty(item.jsonfield1))
            {
                string valore3 = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "adulti", Lingua);
                string valore4 = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "bambini", Lingua);
                if (!string.IsNullOrEmpty(valore3))
                    TestoMail += "<br/>" + "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "adulti") + ": " + "</b>" + valore4 + "<br/>";
                if (!string.IsNullOrEmpty(valore4))
                    TestoMail += " " + "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "bambini") + ": " + "</b>" + valore4 + "<br/>";
            }
            //////////////////////////////////
            //INSERIMENTO DATI PER SCAGLIONI
            //////////////////////////////////
            //string prezzoscaglione = eCommerceDM.Selezionadajson(item.jsonfield1, "prezzo", Lingua);
            string datapartenza = WelcomeLibrary.UF.Utility.reformatdatetimestring((string)eCommerceDM.Selezionadajson(item.jsonfield1, "datapartenza", Lingua));
            string dataritorno = WelcomeLibrary.UF.Utility.reformatdatetimestring((string)eCommerceDM.Selezionadajson(item.jsonfield1, "dataritorno", Lingua));
            string idscaglione = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "idscaglione", Lingua);
            //caglione completo nel carrello
            //Scaglioni scaglionedacarrello = Newtonsoft.Json.JsonConvert.DeserializeObject<Scaglioni>((String)eCommerceDM.Selezionadajson(item.jsonfield1, "scaglione", Lingua));
            if (!string.IsNullOrEmpty(idscaglione) || !string.IsNullOrEmpty(datapartenza))
            {
                if (!string.IsNullOrEmpty(datapartenza))
                    TestoMail += "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedata") + "</b>" + datapartenza + "<br/>";
                if (!string.IsNullOrEmpty(idscaglione))
                    TestoMail += "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedataritorno") + "</b>" + dataritorno + "<br/>";
                if (!string.IsNullOrEmpty(idscaglione))
                    TestoMail += "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglioneid") + "</b>" + idscaglione + "<br/>";
            }
            if (!string.IsNullOrWhiteSpace(item.CodiceProdotto))
                TestoMail += "<b>Codice Articolo : </b>" + item.CodiceProdotto + "<br/>";
            TestoMail += "<b>Id articolo :</b> " + item.Offerta.Id.ToString() + "<br/>";
            if (!string.IsNullOrEmpty(item.Campo2))
            {
                List<ModelCarCombinate> listCarr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(item.Offerta.Xmlvalue);
                ResultAutocomplete taglia = new ResultAutocomplete();
                ResultAutocomplete colore = new ResultAutocomplete();
                ModelCarCombinate elem = listCarr.Find(e => e.id == item.Campo2);
                if (elem != null)
                    TestoMail += " - " + references.ResMan("BaseText", Lingua, "selectcat1") + " : " + elem.caratteristica1.value + " - " + references.ResMan("BaseText", Lingua, "selectcat2") + " : " + elem.caratteristica2.value + "<br/>";
            }
            TestoMail += "<b> Quantità :</b> " + item.Numero + "<br/>";
            if (item.Prezzo != 0)
                TestoMail += "<b>  Prezzo Unitario :</b> " + item.Prezzo + " €<br/>";

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

        TestoMail += "<tr><td>Totale articoli " + totali.TotaleOrdine + " € </td></tr>";
        if (totali.TotaleSconto != 0)
            TestoMail += "<tr><td>Sconto applicato " + totali.TotaleSconto + " € </td></tr>";
        TestoMail += "<tr><td>";
        if (totali.TotaleSpedizione != 0)
            TestoMail += "Spese di spedizione " + totali.TotaleSpedizione + "  €<br/>";
        if (totali.TotaleSmaltimento != 0)
            TestoMail += "<br/>Spese di smaltimeto(PFU) " + totali.TotaleSmaltimento + "  €<br/>";
        TestoMail += "<br/><b>TOTALE ORDINE COMPLESSIVO: </b>" + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";
        if (totali.Percacconto != 100)
        {
            TestoMail += "<tr><td><b>RICHIESTO PAGAMENTO ACCONTO " + totali.Percacconto + "% :</b> " + totali.TotaleAcconto + " €</td></tr>";
            TestoMail += "<tr><td><b>DA SALDARE ENTRO 30 GG DATA PARTENZA " + ":</b> " + totali.TotaleSaldo + " €</td></tr>";
        }
        else
            TestoMail += "<tr><td><b>RICHIESTO PAGAMENTO SALDO  :</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

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