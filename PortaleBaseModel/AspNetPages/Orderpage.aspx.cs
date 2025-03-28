﻿using System;
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
                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                string registrazione = CaricaValoreMaster(Request, Session, "reg");
                HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
                metarobots.Attributes["Content"] = "noindex,follow";

                string conversione = CaricaValoreMaster(Request, Session, "conversione");
                if (conversione == "true")
                {
                    Visualizzarisposta();
                    return;
                }

                if (registrazione != "false") // da abilita se non si vuole far far acquisti senza login ..... di registrazione utente!!!
                    VerificaStatoLoginUtente();
                RiempiDdlNazione("IT", ddlNazione);
                RiempiDdlNazione("IT", ddlNazioneS);
                CaricaCarrello(true);
            }
            else
            {
                if (Request["__EVENTTARGET"] == "refreshcarrello")
                {
                    string parameter = Request["__EVENTARGUMENT"];
                    bool referesformcliente = false;
                    if (parameter == "loginuser") referesformcliente = true;
                    CaricaCarrello(referesformcliente);
                }

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

                output.CssClass = "";
                output.Text = "";
            }
            DataBind();


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
            case "DE":
                output.Text = "<br/>Order Correctly Sent. <br/>You'll be contacted as soon as possible. " + references.ResMan("Common", Lingua, "GoogleConversione");
                break;
            case "ES":
                output.Text = "<br/>Order Correctly Sent. <br/>You'll be contacted as soon as possible. " + references.ResMan("Common", Lingua, "GoogleConversione");
                break;
        }
    }
    private void CaricaCarrello(bool forcerefreshformcliente = false)
    {
        //DATI DEL CLIENTE PRESI DAL DATABASE INZIALMENTE
        if (forcerefreshformcliente)
            CaricaDatiCliente();

        if (Session["codicesconto"] != null)
        {
            litCodiceSconto.Text = "<b>Codici sconto attivi / Active codes :</b> " + Session["codicesconto"].ToString().Replace("|", " - ");
            //txtCodiceSconto.Text = Session["codicesconto"].ToString();
        }
        else
            litCodiceSconto.Text = "<b>Nessuno sconto attivo. / No discount applied </b>";


        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP); //leggio i riferimenti per il caricamento del carrello
        eCommerceDM ecmDM = new eCommerceDM();
        CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Session.SessionID, trueIP);
        rptProdotti.DataSource = carrello;
        rptProdotti.DataBind();

        string codicenazione = SelezionaNazione(carrello, ddlNazione.SelectedValue, ddlNazioneS.SelectedValue);

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


    /// <summary>
    /// Invia la mail d'ordine finale ( usato solo per pagamento con bonifico, contanti o richiesta preventivo ) 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnConvalidaOrdine(object sender, EventArgs e)
    {
        try
        {
            //////////////////////////////////////////////////////////////////
            //Valori BASE da aggiornare PER procedere all'ordine
            //////////////////////////////////////////////////////////////////
            eCommerceDM ecom = new eCommerceDM();
            Cliente cliente = new Cliente();
            TotaliCarrello totali = new TotaliCarrello();
            CarrelloCollection prodotti = new CarrelloCollection();
            string CodiceOrdine = "";
            string modalita = "";
            Session.Add("Lingua", Lingua); //Memorizzo in session  pure la lingua per mantenerla nelle chiamate di risposta dal sistema di pagamento
            //string descrizionepagamento = "";
            if (inpContanti.Checked)
            {
                modalita = inpContanti.Value;
                // descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
            }
            if (inpBonifico.Checked)
            {
                modalita = inpBonifico.Value;
                //  descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
            }
            //if (inpPaypal.Checked)
            //{
            //    modalita = inpPaypal.Value;
            //    // descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
            //}
            if (inpPaypalnew.Checked) //nuovo sistema paypal con chiamata diretta ma passa tramite l'handler non da qui
            {
                modalita = inpPaypalnew.Value;
                // descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
            }
            if (inpstripe.Checked)  //nuovo sistema stripe con chiamata diretta ma passa tramite l'handler non da qui
            {
                modalita = inpstripe.Value;
                //  descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
            }
            if (inpPayway.Checked)
            {
                modalita = inpPayway.Value;
                // descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
            }
            if (inpRichiesta.Checked)
            {
                modalita = inpRichiesta.Value;
                // descrizionepagamento = references.ResMan("Common", Lingua, "chk" + modalita).ToString();  //_> da inserie la descrizione della forma di pagamento
            }
            if (string.IsNullOrEmpty(modalita))
            {
                output.CssClass = "alert alert-danger"; output.Text = references.ResMan("Common", Lingua, "txtPagamento").ToString();
                return;
            }

            //////////////////////////////////////////////////////////////////
            //DATI DEL CLIENTE PRESI DAL FORM
            //////////////////////////////////////////////////////////////////
            if (!CaricaDatiClienteDaForm(cliente)) return; //se la verifica cliente fallisce stoppo L'ORDINE
                                                           /////////////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////////////
            //STEP 1 ORDINE AGGIORNAMENTO DATI CLIENTE E CARRRELLO IN BASE ALLE SELEZIONI SUL FORM
            //////////////////////////////////////////////////////////////////
            Dictionary<string, object> parametriordine = new Dictionary<string, object>();
            parametriordine.Add("modalita", modalita);
            parametriordine.Add("supplementoisole", chkSupplemento.Checked);
            parametriordine.Add("supplementocontrassegno", inpContanti.Checked);
            parametriordine.Add("note", inpNote.Value);
            string reterr = AggiornaDatiPerOrdine(parametriordine, ref CodiceOrdine, ref cliente, ref totali, ref prodotti);
            output.Text += reterr;
            if (!string.IsNullOrEmpty(reterr)) return; //se errore preparazione dati non procedo
                                                       //////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////
            //SETP 2 ESECUZIONE VERA E PROPRIA DELL'ORDINE A SECONDA DELLA MODALITA
            //////////////////////////////////////////////////////////////////
            if (prodotti != null && prodotti.Count > 0 && !string.IsNullOrEmpty(CodiceOrdine))
            {

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

                if (modalita == "paypal") //PAGAMENTO CON CARTA DI CREDITO   (SOAP VECCHIO!!)
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
                if (modalita == "stripe") //NON USATO ( SI PASSA DALLA CHIAMATA ASINCRONA ALL'HANDLER)
                {
                    //qui potrei aggiornare i dati per la chiamata al paymentint .... 
                    //  ( ho trasferito le funzioni nell'handler dei pagamenti
                    // Non viene fatto il postback per il pagamento stripe
                    // QUINDI NON VIENE ESEGUITO NULLA QUI
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
                        string TestoMail = "";
                        //Inviamo le email di conferma al portale ed al cliente
                        //Invio la mail per il fornitore
                        string SoggettoMailFornitore = references.ResMan("Common", Lingua, "OrdineSoggettomailRichiesta") + Nome;
                        TestoMail = CreaMailPerFornitore(totali, prodotti);
                        //Utility.invioMailGenerico(Nome, Email, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);
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
                    int J = 0;
                    foreach (Carrello item in prodotti)
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Prepariamo le richieste di feeback per gli articoli in ordine!!
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if true
                        try
                        {
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


                    /////////////////////////////////////////////////////////////////////////////
                    //controllo se presenti CODICISCONTO con uso una tantum e li cancello dalla tabella sconti in modo da impedire riutilizzo
                    /////////////////////////////////////////////////////////////////////////////
                    try
                    {
                        string codiciscontousati = totali.Codicesconto.Trim();

                        /////////////////////////////////////////////////////////// ( vediamo se abbiamo articoli non scontabili )
                        List<long> idcarrellotoexclude = new List<long>();
                        double totalenonscontabile = 0;
                        string onlyfullpricediscountable = ConfigManagement.ReadKey("onlyfullpricediscountable");
                        bool _tmlofpd = true;
                        bool.TryParse(onlyfullpricediscountable, out _tmlofpd);
                        if (_tmlofpd)
                            foreach (Carrello itemcarrello in prodotti)
                            {
                                if (itemcarrello.Offerta != null)
                                    if (itemcarrello.Offerta.PrezzoListino != 0 && itemcarrello.Prezzo < itemcarrello.Offerta.PrezzoListino)
                                    {
                                        totalenonscontabile += (itemcarrello.Numero * itemcarrello.Prezzo);
                                        idcarrellotoexclude.Add(itemcarrello.ID);
                                    }
                            }
                        ///////////////////////////////////////////////////////////

                        eCommerceDM ecmDM = new eCommerceDM();
                        Codicesconto _params = new Codicesconto();
                        CodicescontoList listcode = new CodicescontoList(); //codici da applicare
                        string[] codiciinsessione = codiciscontousati.Split('|');
                        if (codiciinsessione != null)
                            foreach (string p in codiciinsessione)
                            {
                                if (!string.IsNullOrEmpty(p.Trim()))
                                {
                                    _params.Testocodicesconto = p;
                                    CodicescontoList _tmpcode = ecmDM.CaricaListaSconti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _params);
                                    if (_tmpcode != null && _tmpcode.Count == 1)
                                    {
                                        //eliminiamo gli sconti uso singolo (  verifico se il codice sconto usosingolo è stato applicato a prodotti a carrello o meno in base alle condizioni di applicazione degli sconti )
                                        if (_tmpcode[0].Usosingolo)
                                        {
                                            bool bruciacodiceusosingolo = false;

                                            //////////////////// NON CONSIDERO LE LISTE DI ESCLUSIONE PRODOTTI SCONTATI IN CASO DI CODICE SCONTO CUMULABILE /////
                                            double tmp_totalenonscontabile = totalenonscontabile;
                                            List<long> tmp_idcarrellotoexclude = new List<long>(idcarrellotoexclude);
                                            if (_tmpcode[0].applicaancheascontati) //codice da applicare anche a prodotti scontati
                                            {
                                                tmp_totalenonscontabile = 0;
                                                tmp_idcarrellotoexclude = new List<long>();
                                            }
                                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                            long idscaglionedascontare = (_tmpcode[0].Idscaglione != null) ? _tmpcode[0].Idscaglione.Value : 0;
                                            //Vediamo se il codicesconto ha riferimenti a prodotto o categoria/sottocategoria presenti nel carrello
                                            long idprodottodascontare = (_tmpcode[0].Idprodotto != null) ? _tmpcode[0].Idprodotto.Value : 0;

                                            string codicifiltrodascontare = (!string.IsNullOrEmpty(_tmpcode[0].Codicifiltro)) ? _tmpcode[0].Codicifiltro : "";
                                            string[] _tmplist = codicifiltrodascontare.Split(',');
                                            List<string> listcodicifiltro = (_tmplist != null) ? _tmplist.ToList() : new List<string>();
                                            listcodicifiltro.RemoveAll(i => string.IsNullOrEmpty(i));

                                            string caratteristica1filtrodascontare = (!string.IsNullOrEmpty(_tmpcode[0].caratteristica1filtro)) ? _tmpcode[0].caratteristica1filtro : "";
                                            string[] _tmpcarlist = caratteristica1filtrodascontare.Split(',');
                                            List<string> listcaratteristica1filtro = (_tmpcarlist != null) ? _tmpcarlist.ToList() : new List<string>();
                                            listcaratteristica1filtro.RemoveAll(i => string.IsNullOrEmpty(i));

#if true   //Calcolo l'importo da scontare in base alle condizioni ( come nella procedura di calcolo degli sconti ) per vedere se il codice è stato usato o meno verifico che l'importo da scontare risulti maggiore di zero ( altrimenti il codice non è statao applicato )
                                            if (idprodottodascontare == 0 && string.IsNullOrEmpty(codicifiltrodascontare) && string.IsNullOrEmpty(caratteristica1filtrodascontare) && idscaglionedascontare == 0)
                                            {
                                                double importodascontare = ((double)totali.TotaleOrdine - tmp_totalenonscontabile);
                                                if (importodascontare > 0) bruciacodiceusosingolo = true;
                                            }
                                            else
                                            {
                                                //Calcolare valore da scontare sugli elementi a carrello per applicare gli sconti riguardanti idposdotto o categorie
                                                //calcoliamo il valore su cui applicare lo sconto sulla base dell'idprodotto del codice sconto e/o sul codice categoria o sottocategoria del codice sconto escludendo quelli che sono già scontati sul listino
                                                double importodascontare = 0;

                                                //sconto su scaglione , verifico la presenza nel carrello dello scaglione specificato nello sconto ( ce ne dovrebbe essere sempre solo 1 ) per tirare fuori l'importo da scontare per gli scaglioni!
                                                if (idscaglionedascontare != 0)
                                                    prodotti.ForEach(c => importodascontare += (((String)eCommerceDM.Selezionadajson(c.jsonfield1, "idscaglione", "I")) == idscaglionedascontare.ToString()) ? (c.Numero * c.Prezzo) : 0);
                                                //Sconto su prodotto ( se non tra articoli/prodotti gia scontati )
                                                else if (idprodottodascontare != 0)
                                                    prodotti.ForEach(c => importodascontare += (c.id_prodotto == idprodottodascontare && !tmp_idcarrellotoexclude.Contains(c.ID)) ? (c.Numero * c.Prezzo) : 0);

                                                //versione combianta in and categorie e caratteristica ( devo prendere dal carrello gli elementi che soddisfano entrambe le condizioni )!!!!! 
                                                else if (listcodicifiltro.Count > 0 || listcaratteristica1filtro.Count > 0) //sconto su categorie o caratteristica1 escludendo entrambe le liste vuote
                                                {

                                                    if ((listcodicifiltro.Count > 0 && listcaratteristica1filtro.Count > 0))
                                                        prodotti.ForEach(c => importodascontare += (
                                                    (listcodicifiltro.Contains(c.Offerta.CodiceCategoria) || listcodicifiltro.Contains(c.Offerta.CodiceCategoria2Liv)) //Sconto su categoria/sottocategoria articolo 
                                                    && listcaratteristica1filtro.Contains(c.Offerta.Caratteristica1.ToString()) //sconto su caratterisrica1 ( marca o altro )
                                                    && !tmp_idcarrellotoexclude.Contains(c.ID)) //( se non in lista esclusione tra articoli/prodotti gia scontati )
                                                    ? (c.Numero * c.Prezzo) : 0);

                                                    if ((listcodicifiltro.Count == 0 && listcaratteristica1filtro.Count > 0))
                                                        prodotti.ForEach(c => importodascontare += (
                                                    listcaratteristica1filtro.Contains(c.Offerta.Caratteristica1.ToString())
                                                    && !tmp_idcarrellotoexclude.Contains(c.ID))
                                                    ? (c.Numero * c.Prezzo) : 0);

                                                    if ((listcodicifiltro.Count > 0 && listcaratteristica1filtro.Count == 0))
                                                        prodotti.ForEach(c => importodascontare += (
                                                    (listcodicifiltro.Contains(c.Offerta.CodiceCategoria) || listcodicifiltro.Contains(c.Offerta.CodiceCategoria2Liv))
                                                    && !tmp_idcarrellotoexclude.Contains(c.ID))
                                                    ? (c.Numero * c.Prezzo) : 0);

                                                }
                                                if (importodascontare > 0) bruciacodiceusosingolo = true;

                                            }

#endif

#if false //versione precedente per verifica se il codice è stato usato nel calcolo degli sconti
                                ////////////////////////////////////////////////////////
                                //(ATTENZIONE) controllare che se codicesconto di tipo NON CUMULABILE
                                //deve essere invalidato solo se a carrello presente almeno un articolo a prezzo pieno 
                                //e previsto non applicazione sconti ad articoli scontati ( onlyfullpricediscountable == true )
                                //Comanda la tipologia di codicesconto se cumulabile o no, considero solo il caso non cumulabile per vedere se mantenere il codice buono
                                // (se il codicesconto cumulabile qualisiasi prodotto scontato o meno mi invalida il codicesconto )
                                //////
                                // Il codicesconto resta valido solo se codicesconto non cumulabile -> scontabili solo prodotti a prezzo pieno (onlyfullpricediscountable) -> 
                                // non deve esistere nemmeno un prodotto a carrello con prezzo pieno ( tutti scontati ) solo allora il codicesconto è sempre buono
                                ////////////////////////////////////////////////////////
                                ///
                                ///////////////////////////////////////////////////////
                                bool prodottoprezzopienoincarrello = prodotti.Exists(c => (c.Offerta != null && c.Offerta.PrezzoListino == 0 && c.Prezzo != 0));
                                bool consentieliminazione = true;
                                if (!_tmpcode[0].applicaancheascontati && _tmlofpd && !prodottoprezzopienoincarrello)
                                    consentieliminazione = false; //blocco l'invalidazione del codice solo in questo caso
                                bool bruciacodiceusosingolo = false;
                                //codice senza riferimento a prodotto o categoria -> da bruciare SEMPRE ( a meno caso particolare sopra indicato )
                                if (!bruciacodiceusosingolo && idprodottodascontare == 0 && string.IsNullOrEmpty(codicifiltrodascontare) && string.IsNullOrEmpty(caratteristica1filtrodascontare) && idscaglionedascontare == 0 && consentieliminazione) bruciacodiceusosingolo = true;
                                ///////////////////////////////////////////////////////


                                ///CONTROLLO presenza scaglione a carrello associato al codice sconto caso idscaglione -> da bruciare
                                if (!bruciacodiceusosingolo && idscaglionedascontare != 0)
                                    bruciacodiceusosingolo = prodotti.Exists(c => (((String)eCommerceDM.Selezionadajson(c.jsonfield1, "idscaglione", "I")) == idscaglionedascontare.ToString()));


                                //controllo presenza prodotto in carrello associato al codice sconto caso id -> da bruciare
                                if (!bruciacodiceusosingolo && idprodottodascontare != 0)
                                {
                                    bruciacodiceusosingolo = prodotti.Exists(c => c.id_prodotto == idprodottodascontare);

                                    //Ulteriore verifica in base ai flag esclusione sconti su prodotti gia scontati a carrello
                                    //Se codicesconto non cumulabile e scontabili solo articoli prezzo pieno e id associato relativo a prodotto a prezzo scontato -> non brucio
                                    if (bruciacodiceusosingolo)
                                    {
                                        bool presenteprodottoprezzopieno = prodotti.Exists(c => c.id_prodotto == idprodottodascontare && (c.Offerta != null && c.Offerta.PrezzoListino == 0 && c.Prezzo != 0));
                                        if (!_tmpcode[0].applicaancheascontati && _tmlofpd && !presenteprodottoprezzopieno) bruciacodiceusosingolo = false;
                                    }
                                }


                                //oppure presenza prodotto in carrello associato al codice sconto caso categorie -> da bruciare
                                if (!bruciacodiceusosingolo && listcodicifiltro.Count > 0 && listcaratteristica1filtro.Count == 0)
                                {
                                    bruciacodiceusosingolo = prodotti.Exists(c => listcodicifiltro.Contains(c.Offerta.CodiceCategoria) || listcodicifiltro.Contains(c.Offerta.CodiceCategoria2Liv));

                                    //Ulteriore verifica in base ai flag esclusione sconti su prodotti gia scontati a carrello
                                    //Se codicesconto non cumulabile e scontabili solo articoli prezzo pieno e nessun articolo a prezzo pieno nelle categorie associate -> non brucio
                                    if (bruciacodiceusosingolo)
                                    {
                                        bool presenteprodottoprezzopieno = prodotti.Exists(c => (listcodicifiltro.Contains(c.Offerta.CodiceCategoria) || listcodicifiltro.Contains(c.Offerta.CodiceCategoria2Liv)) && (c.Offerta != null && c.Offerta.PrezzoListino == 0 && c.Prezzo != 0));

                                        if (!_tmpcode[0].applicaancheascontati && _tmlofpd && !presenteprodottoprezzopieno) bruciacodiceusosingolo = false;
                                    }
                                }
                                //oppure presenza prodotto in carrello associato al codice sconto caso caratteristica1/marca -> da bruciare
                                if (!bruciacodiceusosingolo && listcaratteristica1filtro.Count > 0 && listcodicifiltro.Count == 0)
                                {
                                    bruciacodiceusosingolo = prodotti.Exists(c => listcaratteristica1filtro.Contains(c.Offerta.Caratteristica1.ToString()));

                                    //Ulteriore verifica in base ai flag esclusione sconti su prodotti gia scontati a carrello
                                    //Se codicesconto non cumulabile e scontabili solo articoli prezzo pieno e nessun articolo a prezzo pieno con caratteristica associata -> non brucio
                                    if (bruciacodiceusosingolo)
                                    {
                                        bool presenteprodottoprezzopieno = prodotti.Exists(c => (listcaratteristica1filtro.Contains(c.Offerta.Caratteristica1.ToString())) && (c.Offerta != null && c.Offerta.PrezzoListino == 0 && c.Prezzo != 0));
                                        if (!_tmpcode[0].applicaancheascontati && _tmlofpd && !presenteprodottoprezzopieno) bruciacodiceusosingolo = false;
                                    }
                                }

#endif

                                            //METTO IL CODICE COME SCADUTO
                                            if (bruciacodiceusosingolo)
                                            {    //cancellazione codice
                                                 // ecmDM.CancellaSconto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _tmpcode[0].Id);
                                                 //inlaternativa posso settare la data di questi a un giorno passato ( per impedire che venga riusato )
                                                _tmpcode[0].Datascadenza = System.DateTime.Now.AddDays(-1);
                                                if (_tmpcode[0].Usosingolo) ecmDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _tmpcode[0]);
                                            }
                                        }
                                    }
                                }
                            }
                    }
                    catch { }
                    /////////////////////////////////////////////////////////////////////////////


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
                    Session.Remove("tmpCodiceOrdine");
                    //CreaNuovaSessione(Session, Request); //Svuota la session per un nuovo ordine!!
                    output.Text += jscodetoinject;
                    output.Text += references.ResMan("Common", Lingua, "GoogleConversione");

                    switch (Lingua)
                    {
                        case "I":
                            output.Text += references.ResMan("Common", Lingua, "risposta_5a");

                            break;
                        default:
                            output.Text += references.ResMan("Common", Lingua, "risposta_5a");
                            break;
                    }

                }
                if (modalita == "richiesta") //SOLO INVIO RICHIESTA non ordine
                {
                    try
                    {
                        string TestoMail = "";
                        //Inviamo le email di conferma al portale ed al cliente
                        //Invio la mail per il fornitore
                        string SoggettoMailFornitore = references.ResMan("Common", Lingua, "OrdineSoggettomailRichiesta") + Nome;
                        TestoMail = CreaMailPerFornitore(totali, prodotti);
                        //Utility.invioMailGenerico(Nome, Email, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);
                        Utility.invioMailGenerico(totali.Denominazionecliente, totali.Mailcliente, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);

                        //Invia la mail per il cliente
                        string SoggettoMailCliente = references.ResMan("Common", Lingua, "OrdineSoggettomailRiepilogo") + Nome;
                        TestoMail = CreaMailCliente(totali, prodotti);
                        Utility.invioMailGenerico(Nome, Email, SoggettoMailCliente, TestoMail, totali.Mailcliente, totali.Denominazionecliente, null, "", true, Server);
                    }
                    catch { }
#if true

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
                    int jj = 1;
                    foreach (Carrello item in prodotti)
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Prepariamo le richieste di feeback per gli articoli in ordine!!
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //if (jj <= 2)
                        //    try
                        //    {
                        //        Mail mailfeedback = new Mail();

                        //        mailfeedback.Sparedict["linkfeedback"] = "";//default preso dalle risorse feedbacksdefaultform
                        //        mailfeedback.Sparedict["idnewsletter"] = "";//default dalle risorse feedbackdefaultnewsletter
                        //        mailfeedback.Sparedict["deltagiorniperinvio"] = "";//default dalle risorse feedbacksdefaultdeltagg
                        //        mailfeedback.Sparedict["idclienti"] = cliente.Id_cliente.ToString();
                        //        mailfeedback.Id_card = item.id_prodotto;
                        //        HandlerNewsletter.preparamail(mailfeedback, Lingua); //Preparo le mail nello scheduler!!

                        //    }
                        //    catch { }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        item.CodiceOrdine = CodiceOrdine;
                        SalvaCodiceOrdine(item);
                        jj++;
                    }
                    InsertEventoBooking(prodotti, totali, "rif000001");
#endif

                    pnlFormOrdine.Visible = false;
                    output.Text += references.ResMan("Common", Lingua, "GoogleConversione");
                    switch (Lingua)
                    {
                        case "I":
                            output.Text += "<div><br/>Richiesta inviata correttamente. <br/>Sarete contattati a breve dal nostro personale.</div>";

                            break;
                        default:
                            output.Text += "<br/>Richiesta Correctly Sent. <br/>You'll be contacted as soon as possible.";
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
    private string AggiornaDatiPerOrdine(Dictionary<string, object> parametri, ref string CodiceOrdine, ref Cliente cliente, ref TotaliCarrello totali, ref CarrelloCollection prodotti)
    {

        #region aggiornamento carrello  e dati cliente prima dell'ordine
        string ret = "";
        //////////////////////////////////////////////////////////////////
        //PROCEDURA AGGIORNAMENTO TOTALI E ORDINE
        //////////////////////////////////////////////////////////////////
        //se Ho gia generato un codice ordine svuoto la session  totali, prodotti e cliente prima di ricalcolarli
        if (HttpContext.Current.Session["tmpCodiceOrdine"] != null)
        {
            string tmpcodiceordine = Session["tmpCodiceOrdine"].ToString();
            Session.Remove("cliente_" + tmpcodiceordine);
            Session.Remove("totali_" + tmpcodiceordine);
            Session.Remove("prodotti_" + tmpcodiceordine);
            Session.Remove("tmpCodiceOrdine");
        }


        //Per prima cosa mi riprendo i dati del carrello in base alla sessione per completare l'ordine
        //Verifico per un ultima volta che tutto sia a posto che le quantità non superino la disponibilità
        prodotti = new CarrelloCollection();
        eCommerceDM ecom = new eCommerceDM();
        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP); //leggio i riferimenti per il caricamento del carrello
        prodotti = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Session.SessionID, trueIP);
        AggiornaDatiUtenteSuCarrello(prodotti, cliente.Id_cliente);  //aggiorno il codice cliente e sconto nel carrello
        if (prodotti != null && prodotti.Count > 0)
        {


            //vERIFICA finale PER IL BOOKING PRIMA DI ORDINARE!!!
            if (!VerificaDisponibilitaEventoBooking(prodotti, "rif000001"))
            {
                ret = references.ResMan("basetext", Lingua, "testoprenotaerr1").ToString();
                return ret;
            }
            ////////////////////////////////////
            prodotti.Sort(new GenericComparer<Carrello>("Data", System.ComponentModel.ListSortDirection.Descending));

            //Genero il codice ordine dato che il cliente me lo ha confermato e me lo salvo in tabella per tutti i prodotti del carrello attuale
            //In modo da associarli ad un ordine preciso in caso di successo dell'invio del pagamento o della  mail
            CodiceOrdine = GeneraCodiceOrdine();
            bool supplementoisole = (bool)parametri["supplementoisole"];// chkSupplemento.Checked;
            bool supplementocontrassegno = (bool)parametri["supplementocontrassegno"];// inpContanti.Checked;

            //totali = CalcolaTotaliCarrello(Request, Session, cliente.CodiceNAZIONE, "", supplementoisole, supplementocontrassegno);
            //Prendo la nazione giusta dal cliente per il calcolo delle spese di spedizione
            string codicenazione = "";
            Carrello c = prodotti.Find(_c => !string.IsNullOrWhiteSpace(_c.Codicenazione)); //dal carrello
            if (c != null) codicenazione = c.Codicenazione;
            if (cliente != null && !string.IsNullOrEmpty(cliente.CodiceNAZIONE)) codicenazione = cliente.CodiceNAZIONE;
            if (cliente != null && !chkSpedizione.Checked) // se tolta spunta spedizione uguale da quella di fatturazione prendo la nazione da quella di spedizione del cliente
            {
                Cliente clispediz = Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(cliente.Serialized);
                if (clispediz != null && !string.IsNullOrEmpty(clispediz.CodiceNAZIONE))
                {
                    codicenazione = clispediz.CodiceNAZIONE;
                }
            }
            totali = CalcolaTotaliCarrello(Request, Session, codicenazione, "", supplementoisole, supplementocontrassegno);

            //PRENDIAMO I DATI DAL FORM PER LA PREPARAZIONE ORDINE //////////////////////////////////////////////////////////////////
            totali.Denominazionecliente = cliente.Cognome + " " + cliente.Nome;
            totali.Mailcliente = cliente.Email;
            totali.Dataordine = System.DateTime.Now;
            totali.CodiceOrdine = CodiceOrdine;

            totali.Indirizzofatturazione += cliente.Cognome + " " + cliente.Nome + "<br/>";
            totali.Indirizzofatturazione += cliente.Indirizzo + "<br/>";
            totali.Indirizzofatturazione += cliente.Cap + " " + cliente.CodiceCOMUNE + "  (" + ((!(string.IsNullOrWhiteSpace(NomeProvincia(cliente.CodicePROVINCIA, Lingua)))) ? NomeProvincia(cliente.CodicePROVINCIA, Lingua) : cliente.CodicePROVINCIA) + ")<br/>";
            totali.Indirizzofatturazione += "Nazione: " + cliente.CodiceNAZIONE + "<br/>";
            totali.Indirizzofatturazione += "Telefono: " + cliente.Telefono + "<br/>";
            if (!string.IsNullOrEmpty(cliente.Ragsoc))
                totali.Denominazionecliente += " " + cliente.Ragsoc + "<br/>";
            totali.Indirizzofatturazione += "P.Iva: " + cliente.Pivacf + "<br/>";
            totali.Indirizzofatturazione += "CodiceDestinatario/Pec: " + cliente.Emailpec + "<br/>";


            if (chkRichiedifattura.Checked)
                totali.Indirizzofatturazione += "Richiesta emissione fattura:  SI <br/>";
            else
                totali.Indirizzofatturazione += "Richiesta emissione fattura:  NO <br/>";

            //SE INDIRIZZO SPEDIIZONE DIVERSO -> LO MEMORIZZO NEI TOTALI ( E serializzo il dettaglio nel cliente nel campo serialized )
            string indirizzospedizione = "";
            if (!chkSpedizione.Checked && !string.IsNullOrEmpty(cliente.Serialized))
            {
                /////////////////////////////////////
                //prendiamO dati spedizione dal cliente serializzato .... in accordo col metodo usato per fatturazione anziche dal form
                Cliente clispediz = Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(cliente.Serialized);
                /////////////////////////////////////
                if (clispediz != null)
                {
                    //  inpProvinciaS.Value = (!(string.IsNullOrWhiteSpace(NomeProvincia(clispediz.CodicePROVINCIA, Lingua)))) ? NomeProvincia(clispediz.CodicePROVINCIA, Lingua) : clispediz.CodicePROVINCIA;
                    if (!string.IsNullOrWhiteSpace(clispediz.Cognome) || !string.IsNullOrWhiteSpace(clispediz.Nome))
                        indirizzospedizione += clispediz.Cognome + " " + clispediz.Nome + "<br/>";
                    else
                        indirizzospedizione += cliente.Cognome + " " + cliente.Nome + "<br/>";
                    if (!string.IsNullOrEmpty(clispediz.Indirizzo))
                        indirizzospedizione += clispediz.Indirizzo + "<br/>";
                    if (!string.IsNullOrEmpty(clispediz.Cap) && !string.IsNullOrEmpty(clispediz.CodiceCOMUNE) && !string.IsNullOrEmpty(clispediz.CodicePROVINCIA))
                    {
                        indirizzospedizione += clispediz.Cap + " " + clispediz.CodiceCOMUNE + "  (" + ((!(string.IsNullOrWhiteSpace(NomeProvincia(clispediz.CodicePROVINCIA, Lingua)))) ? NomeProvincia(clispediz.CodicePROVINCIA, Lingua) : clispediz.CodicePROVINCIA) + ")<br/>";
                        indirizzospedizione += "Nazione: " + clispediz.CodiceNAZIONE + "<br/>";
                    }
                    if (!string.IsNullOrEmpty(clispediz.Telefono))
                        indirizzospedizione += "Telefono: " + clispediz.Telefono + "<br/>";
                }
#if false
                /////////////////////////////////////
                /// INDIRIZZO SPEDIZIONE DALLE CASELLE SUL FORM ////////////////
                if (!string.IsNullOrWhiteSpace(inpCognomeS.Value) || !string.IsNullOrWhiteSpace(inpNomeS.Value))
                    indirizzospedizione += inpCognomeS.Value + " " + inpNomeS.Value + "<br/>";
                else
                    indirizzospedizione += cliente.Cognome + " " + cliente.Nome + "<br/>";

                if (!string.IsNullOrEmpty(inpIndirizzoS.Value))
                    indirizzospedizione = inpIndirizzoS.Value + "<br/>";
                if (!string.IsNullOrEmpty(inpCaps.Value) && !string.IsNullOrEmpty(inpComuneS.Value) && !string.IsNullOrEmpty(inpProvinciaS.Value))
                {
                    indirizzospedizione += inpCaps.Value + " " + inpComuneS.Value + "  (" + ((!(string.IsNullOrWhiteSpace(NomeProvincia(inpProvinciaS.Value, Lingua)))) ? NomeProvincia(inpProvinciaS.Value, Lingua) : inpProvinciaS.Value) + ")<br/>";
                    indirizzospedizione += "Nazione: " + cliente.CodiceNAZIONE + "<br/>";
                }
                if (!string.IsNullOrEmpty(inpTelS.Value))
                    indirizzospedizione += "Telefono: " + inpTelS.Value + "<br/>";
                /////////////////////////////////////  
#endif

                totali.Indirizzospedizione = indirizzospedizione;
            }
#if false   // con questa visualizza sempre la spedizione
                if (string.IsNullOrWhiteSpace(indirizzospedizione))
                {
                    totali.Indirizzospedizione = totali.Indirizzofatturazione;
                } 
#endif
            totali.Note = (string)parametri["note"];
            totali.Modalitapagamento = (string)parametri["modalita"];

            //Valorizzato solo alla ricezione del pagamento prima della spedizione o tramite la procedura con pagamento anticipato
            if (totali.Percacconto == 100)
            { totali.Pagato = false; totali.Pagatoacconto = false; } // da capire se acconto è zero se vogliamo spuntare pagato acconto!
            else
            { totali.Pagato = false; totali.Pagatoacconto = false; }
            totali.Urlpagamento = "";

            //Prepariamo i valori per la chiamata a sistema pagamento
            Session.Add("tmpCodiceOrdine", CodiceOrdine); //appoggio il codiceordine generato in sessione
            Session.Add("cliente_" + CodiceOrdine, cliente); //Mettiamo tutto in sessione per riaverlo alla conferma dell'esito positivo della transazione
            Session.Add("totali_" + CodiceOrdine, totali); //Mettiamo tutto in sessione per riaverlo alla conferma dell'esito positivo della transazione
            Session.Add("prodotti_" + CodiceOrdine, prodotti); //Mettiamo tutto in sessione per riaverlo alla conferma dell'esito positivo della transazione
        }
        else
        {
            ret = " Carrello vuoto / Empty order list. <br/> ";

        }
        return ret;
        #endregion
    }
    protected void chkSupplemento_CheckedChanged(object sender, EventArgs e)
    {
        CaricaCarrello();
    }

    protected void btnResetCodiceSconto_Click(object sender, EventArgs e)
    {
        Session.Remove("codicesconto");
        CaricaCarrello();
    }

    protected void btnCodiceSconto_Click(object sender, EventArgs e)
    {
        string insertedcode = txtCodiceSconto.Text;
        insertedcode = insertedcode.ToLower().Trim();
        bool validcode = false;
        outputCodiceSconto.Text = "";
        output.Text = "";

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////CHECK CODICE CON SCONTO CLIENTE COMMERCIALE ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //Testo Se presente una percentuale tra quelle associate ai commerciali e nel caso prendo quella (PRIORITA')
        ClientiDM cDM = new ClientiDM();
        Cliente cli = cDM.CaricaClientePerCodicesconto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, insertedcode);
        if (cli != null && cli.Id_cliente != 0 && !validcode)
        {
            Session.Add("codicesconto", insertedcode);
            txtCodiceSconto.Text = "";
            outputCodiceSconto.Text = "Sconto Applicato Correttamente / Discount applied";
            output.Text += "Sconto Applicato Correttamente / Discount applied";
            validcode = true;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////CHECK CODICE CON SCONTO SCAGLIONI ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        Dictionary<string, double> codiciscontoscaglione = CercaCodiceScontoSuCarrello(Request, Session);
        if (codiciscontoscaglione != null && codiciscontoscaglione.Count > 0 && !validcode)
        {
            //Vediamo se lo sconto è presente tra quelli dello scaglione inserito a carrello
            if (codiciscontoscaglione.ContainsKey(insertedcode))
            {
                Session.Add("codicesconto", insertedcode);
                validcode = true;
                outputCodiceSconto.Text = "Sconto Applicato Correttamente / Discount applied";
                output.Text += "Sconto Applicato Correttamente / Discount applied";
                txtCodiceSconto.Text = "";
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////CHECK CODICE CON SCONTO TBL CONFIG ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        string codefromconfig = ConfigManagement.ReadKey("codicesconto");
        if (insertedcode == codefromconfig && !validcode)
        {
            Session.Add("codicesconto", insertedcode);
            txtCodiceSconto.Text = "";
            outputCodiceSconto.Text = "Sconto Applicato Correttamente / Discount applied";
            output.Text += "Sconto Applicato Correttamente / Discount applied";
            validcode = true;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////CHECK UP TABELLA SCONTI!! ///////////////////////////////////
        ///// Possibilità di cumulare massimo 1 codice percentuale ed 1 voucher in sede  di acquisto carrello.
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        if (!validcode)
        {
            eCommerceDM ecmDM = new eCommerceDM();
            Codicesconto _params = new Codicesconto();
            CodicescontoList codicisconto = new CodicescontoList();
            //insertedcode//codice inserito da verificare
            _params.Testocodicesconto = insertedcode;
            //verifichiamo la validita del codice
            if (!string.IsNullOrEmpty(insertedcode.Trim()))
                codicisconto = ecmDM.CaricaListaSconti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _params);

            if (codicisconto != null && codicisconto.Count > 0)
            { //si suppone che ce ne sia uno solo con il codice inserito in tabella ( e' unique key in tabella tale codicesconto )
                Codicesconto codicedainserire = codicisconto[0];

                //devo vedere la lista codici in sessione se presenti ed agire di conseguenza!!
                string codicigiasettati = "";
                if (Session["codicesconto"] != null && !string.IsNullOrEmpty(Session["codicesconto"].ToString()))
                    codicigiasettati = Session["codicesconto"].ToString().ToLower().Trim();

                //////////////////////////////////////
                //check vincoli per inserimento codici multipli contemporanei
                //////////////////////////////////////
                if (string.IsNullOrEmpty(codicigiasettati)) { Session.Add("codicesconto", insertedcode); txtCodiceSconto.Text = ""; outputCodiceSconto.Text = "Sconto Applicato Correttamente"; output.Text += "Sconto Applicato Correttamente"; validcode = true; }
                else //presenti codici in sessione-> devo controllare il numero e se compatibili con quello richiesto
                {
                    /// Possibilità di cumulare massimo 1 codice percentuale ed 1 voucher in sede  di acquisto carrello.
                    CodicescontoList listcode = new CodicescontoList();
                    string[] codiciinsessione = codicigiasettati.Split('|');
                    if (codiciinsessione != null)
                    {
                        //Se presenti codici controlliamo di non averlo gia inserito o che non sia in conflitto con gli altri
                        if (codiciinsessione.Contains(codicedainserire.Testocodicesconto.ToLower()))
                        {
                            outputCodiceSconto.Text = "Codice già presente / Discount code already present"; output.Text += "Codice già presente / Discount code already present"; validcode = true; //non svuoto la lista codici in sessione
                        }
                        else if (codiciinsessione.Length < 2)  /////se codice non presente verifico che non sia in conflitto con quelli inseriti e controllo  
                        {
                            foreach (string p in codiciinsessione)
                            {
                                if (!string.IsNullOrEmpty(p.Trim()))
                                {
                                    _params.Testocodicesconto = p;
                                    CodicescontoList _tmpcode = ecmDM.CaricaListaSconti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _params);
                                    if (_tmpcode != null && _tmpcode.Count == 1)
                                    {
                                        listcode.Add(_tmpcode[0]);
                                    }
                                }
                            }

                            //check eventuale conflitto di codice ->  massimo 1 codice percentuale ed 1 voucher in sede  di acquisto carrello.
                            bool insertcodicenumerico = (codicedainserire.Scontonum != null) ? true : false;
                            bool insertcodiceperc = (codicedainserire.Scontoperc != null) ? true : false;
                            bool chkcompatibility = true;
                            foreach (Codicesconto codpresente in listcode)
                            {
                                // controlo di compatibilià dei codici ...
                                bool codnum = (codpresente.Scontonum != null) ? true : false;
                                bool codper = (codpresente.Scontoperc != null) ? true : false;

                                if (codnum == insertcodicenumerico) { chkcompatibility = false; }
                                if (codper == insertcodiceperc) { chkcompatibility = false; }
                            }
                            if (chkcompatibility)
                            {
                                Session["codicesconto"] += "|" + insertedcode;
                                txtCodiceSconto.Text = "";
                                outputCodiceSconto.Text = "Sconto Aggiunto Correttamente / Discount applied";
                                output.Text += "Sconto Aggiunto Correttamente / Discount applied";
                            }
                            else
                            { outputCodiceSconto.Text = "Impossibile inserire massimo 1 codice percentuale ed 1 voucher in sede  di acquisto carrello. / Only 1 voucher and 1 % discount allowed "; output.Text += "Impossibile inserire massimo 1 codice percentuale ed 1 voucher in sede  di acquisto carrello. / Only 1 voucher and 1 % discount allowed "; }
                            validcode = true; //comunque non cacello il codice inserito in quesot caso
                        }
                        else { outputCodiceSconto.Text = "Consentiti Massimo due codici contemporanei. / Max 2 codes at once "; output.Text += "Consentiti Massimo due codici contemporanei. / Max 2 codes at once "; validcode = true; }
                    }
                }
            }
            //////////////////////////////////////
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////// SCONTO NON valido ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        if (!validcode) //se codice non valido svuoto la sessione dai codici presenti
        {
            Session.Remove("codicesconto");
            outputCodiceSconto.Text = references.ResMan("Common", Lingua, "testoErrCodiceSconto").ToString();
            output.Text += references.ResMan("Common", Lingua, "testoErrCodiceSconto").ToString();
        }

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
    private void RiempiDdlNazione(string valore, DropDownList ddlnazionelocal)
    {
        List<Tabrif> nazioni = WelcomeLibrary.UF.Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == Lingua; });
        nazioni.Sort(new GenericComparer<Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));
        ddlnazionelocal.Items.Clear();
        foreach (Tabrif n in nazioni)
        {
            ListItem i = new ListItem(n.Campo1, n.Codice);
            ddlnazionelocal.Items.Add(i);
        }
        try
        {
            ddlnazionelocal.SelectedValue = valore.ToUpper();
        }
        catch { valore = "IT"; ddlnazionelocal.SelectedValue = valore.ToUpper(); }
    }


    protected void ddlNazione_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaCarrello();
    }
    protected void ddlNazioneS_SelectedIndexChanged(object sender, EventArgs e)
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

    private string SelezionaNazione(CarrelloCollection carrello, string selcodicenazione = "", string selcodicenaziones = "")
    {
        bool samespedizione = chkSpedizione.Checked; //se spuntata spedizione stesso indirizzo di fatturazione!
        string codicenazione = "";

        if (carrello != null)
        {
            //prendo prima dal carrello il codice nazione
            Carrello c = carrello.Find(_c => !string.IsNullOrWhiteSpace(_c.Codicenazione));
            if (c != null)
                codicenazione = c.Codicenazione;
        }
        //se presente imposto la nazione da quella di fatturazione
        if (!string.IsNullOrEmpty(selcodicenazione)) codicenazione = selcodicenazione;
        try
        {
            ddlNazione.SelectedValue = codicenazione;
        }
        catch
        { }

        //se diverso indirizzo spedizione prendo la nazione da indirizzo spedizione 
        if (!samespedizione && !string.IsNullOrEmpty(selcodicenaziones)) codicenazione = selcodicenaziones;
        if (samespedizione) // se spuntata spedizione = fatturazione uguaglio le selezioni di nazione
            try
            {
                ddlNazioneS.SelectedValue = codicenazione;
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
        {
            // liPaypal.Visible = false;
            lipaypalnew.Visible = false;
        }
        else
        {
            //  liPaypal.Visible = true;
            lipaypalnew.Visible = false;
        }

#if true //metodo alternativo di blocco pagamento e invio solo ordine

        if (totali.Bloccaacquisto)
        {
            litMessage.Text = references.ResMan("Common", Lingua, "testoBloccoacquisto");
            //liPaypal.Visible = false;
            divPayment.Visible = false;
            // inpPaypal.Checked = false;
            inpPaypalnew.Checked = false;
            inpstripe.Checked = false;
            inpRichiesta.Checked = true;
            divOrderrequest.Visible = true;
        }
        else
        {
            litMessage.Text = "";
            // liPaypal.Visible = true;
            lipaypalnew.Visible = true;
            divPayment.Visible = true;
            // inpPaypal.Checked = true;
            inpRichiesta.Checked = false;
            divOrderrequest.Visible = false;

        }
#endif


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
    /// <summary>
    /// prepara le righe per comporre il totale al fine dell'ordine su paypal in paypaldatas ( un elemento per ogni voce che compone il totale )
    /// </summary>
    /// <param name="percentualeanticipo"></param>
    /// <param name="totali"></param>
    /// <param name="prodotti"></param>
    /// <param name="paypaldatas"></param>
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

        //aggiungo un elemento per assicurazione
        if (totali.TotaleAssicurazione != 0)
        {
            dettagliitem = new List<string>();
            dettagliitem.Add("");
            dettagliitem.Add(references.ResMan("Common", Lingua, "carrellototaleassicurazione")); //Descrizione elemento")
            dettagliitem.Add("");
            dettagliitem.Add(totali.TotaleAssicurazione.ToString());
            dettagliitem.Add("1");//Quantità
            dettagliitem.Add(percentualeanticipo.ToString());//Percentuale anticipo da applicare anche alla spedizione
            if (!paypaldatas.ContainsKey("pp_insurance"))
                paypaldatas.Add("pp_insurance", dettagliitem);
        }

        //Aggiungo un elemento per spesespedizione
        if (totali.TotaleSpedizione != 0)
        {
            dettagliitem = new List<string>();
            dettagliitem.Add("");
            dettagliitem.Add(references.ResMan("Common", Lingua, "CarrelloTotaleSpedizione")); //Descrizione elemento")
            dettagliitem.Add("");
            dettagliitem.Add(totali.TotaleSpedizione.ToString());
            dettagliitem.Add("1");//Quantità
                                  //dettagliitem.Add("100");//Percentualeanticipo
            dettagliitem.Add(percentualeanticipo.ToString());//Percentuale anticipo da applicare anche alla spedizione
            if (!paypaldatas.ContainsKey("pp_expcosts"))
                paypaldatas.Add("pp_expcosts", dettagliitem);
        }

        //aggiungo elemento per sconto
        if (totali.TotaleSconto != 0)
        {
            dettagliitem = new List<string>();
            dettagliitem.Add("");
            dettagliitem.Add(references.ResMan("Common", Lingua, "testoSconto"));//Descrizione elemento
            dettagliitem.Add("");
            dettagliitem.Add("-" + totali.TotaleSconto.ToString());
            dettagliitem.Add("1");//Quantità
                                  //dettagliitem.Add("100");//Percentuale anticipo
            dettagliitem.Add(percentualeanticipo.ToString());//Percentuale anticipo ( da applicare anche allo sconto )
            if (!paypaldatas.ContainsKey("pp_discount"))
                paypaldatas.Add("pp_discount", dettagliitem);
        }

        /////////////////////////////////////////////////////////////////////
        //il totale finale deve corrispondere al segunete sempre !!!! lo inserisco nella lista voci di paypal da non sommare ai totali ma per controllo
        //((_TotaleSmaltimento + _TotaleSpedizione + _TotaleOrdine + _TotaleAssicurazione) - _TotaleSconto) * _precacconto / 100 
        /////////////////////////////////////////////////////////////////////
        double amount = 0; // importo da pagare in centesimi di euro
        if (totali.Percacconto != 100)
            amount = (totali.TotaleAcconto);
        else
            amount = ((totali.TotaleAcconto + totali.TotaleSaldo));
        dettagliitem = new List<string>();
        dettagliitem.Add("");
        //dettagliitem.Add(references.ResMan("Common", Lingua, "xxxx"));//Descrizione elemento
        dettagliitem.Add("Importo richiesto/amount to pay");//Descrizione elemento
        dettagliitem.Add("");
        dettagliitem.Add(amount.ToString());
        dettagliitem.Add("1");//Quantità
        dettagliitem.Add("100");//Percentuale anticipo
        if (!paypaldatas.ContainsKey("pp_totalamountcheck"))
            paypaldatas.Add("pp_totalamountcheck", dettagliitem);

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
        clispediz.CodiceNAZIONE = ddlNazioneS.SelectedValue.Trim();

        if (!string.IsNullOrEmpty(inpNomeS.Value.Trim()))
            clispediz.Nome = inpNomeS.Value.Trim();
        if (!string.IsNullOrEmpty(inpCognomeS.Value.Trim()))
            clispediz.Cognome = inpCognomeS.Value.Trim();
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
        references.SearchGeoCodesByText(clispediz, Lingua);


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
        references.SearchGeoCodesByText(cliente, Lingua);

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
        //CHECK CORRETTZZA DELLA MAIL INSERITA ( USARE VALIDATORE DELLE MAIL )
        ////////////////////////////////////////////////////
        //bool validemail = ActiveUp.Net.Mail.Validator.ValidateSyntax(clitmp.Email);
        bool validemail = Utility.IsValidEmail(clitmp.Email);
        if (!validemail)
        {
            output.CssClass = "alert alert-danger"; output.Text = "Email errata|Invalid Email!"; //output.Text = references.ResMan("Common", Lingua, "txtPagamento").ToString();
            return false;

        }
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

    protected bool ControlloLogin()
    {
        bool ret = true;
        if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
            ret = false;
        return ret;
    }

#if true
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
        bool esito = Membership.ValidateUser(username, password);
        if (esito == false) //provo la login con l'id davanti
        {
            ClientiDM cliDM = new ClientiDM();
            Cliente clienteinanagraficaperemail = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, username);
            if (clienteinanagraficaperemail != null && clienteinanagraficaperemail.Id_cliente != 0)
            {
                username = clienteinanagraficaperemail.Id_cliente + "-" + username;
                esito = Membership.ValidateUser(username, password);
            }
        }

        if (esito)
        {
            //FormsAuthentication.LoginUrl = references.ResMan("Common",Lingua,"Linklogin");
            //FormsAuthentication.DefaultUrl
            //FormsAuthentication.RedirectFromLoginPage(username, false);
            //FormsAuthentication.Authenticate(username, password);
            string authpersistentcookie = ConfigManagement.ReadKey("authpersistentcookie");
            bool b_authpersistentcookie = false;
            bool.TryParse(authpersistentcookie, out b_authpersistentcookie);
            FormsAuthentication.SetAuthCookie(username, b_authpersistentcookie);
            Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString());
            outputlogin.Text = "Accesso riuscito.";
            CaricaCarrello(true);
        }
        else
        {
            outputlogin.Text = "Accesso non riuscito. Se sei un nuovo utente, effettua la registrazione.";
        }
    }

#endif
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
                    //txtCodiceSconto.Text = kv.Key;
                    Session.Add("codicesconto", kv.Key); //metto in sessione lo sconto settato del cliente
                    litCodiceSconto.Text = "<b>Codici sconto attivi / Active codes :</b> " + Session["codicesconto"].ToString().Replace("|", " - ");
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
                try
                {
                    if (!string.IsNullOrWhiteSpace(clispediz.CodiceNAZIONE))
                        ddlNazioneS.SelectedValue = clispediz.CodiceNAZIONE;
                }
                catch
                { }
                if (string.IsNullOrWhiteSpace(inpNomeS.Value))
                    inpNomeS.Value = clispediz.Nome;
                if (string.IsNullOrWhiteSpace(inpCognomeS.Value))
                    inpCognomeS.Value = clispediz.Cognome;
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
            if (!string.IsNullOrEmpty(inpNomeS.Value) || !string.IsNullOrEmpty(inpCognomeS.Value) || !string.IsNullOrEmpty(inpCaps.Value) || !string.IsNullOrEmpty(inpComuneS.Value) || !string.IsNullOrEmpty(inpProvinciaS.Value)
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
            System.Threading.Thread.Sleep(1000);
            //creo un Codice Ordine Univoco
            CodiceOrdine = WelcomeLibrary.UF.RandomPassword.Generate(9, 9, new char[][]
                {
                WelcomeLibrary.UF.RandomPassword.PASSWORD_CHARS_NUMERIC.ToCharArray()
                });
            //CodiceOrdine = "ord_" + CodiceOrdine;

            eCommerceDM ecom = new eCommerceDM();
            esito = ecom.VerificaPresenzaCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceOrdine);
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
                case "DE":
                    output.Text = "Error save CodiceOrdine";
                    break;
                case "ES":
                    output.Text = "Error save CodiceOrdine";
                    break;
            }
        }
    }

    protected string CreaMailPerFornitore(TotaliCarrello totali, CarrelloCollection prodotti)
    {
        string TestoMail = "";
        //Mi preparo il testo della mail (formattiamo in html)
        TestoMail += "<table cellpadding='0' cellspacing='0' style='font-size:14px;max-width:600px;width:100%' ><tr><td  valign='top'>" + "<img width=\"200\" src=\"" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "\" />" + "</td></tr>";
        TestoMail += "<tr><td> Ordine effettuato da " + totali.Denominazionecliente + " su " + Nome + "  <br/>";
        TestoMail += "<br/><b>EMAIL CLIENTE :</b>  <br/>";
        TestoMail += totali.Mailcliente + "<br/>";
        TestoMail += references.ResMan("Common", Lingua, "testointromailfornitore");
        TestoMail += "</td></tr>";

        TestoMail += "<tr><td><br/><table  style='font-size:14px;max-width:600px;width:100%'  cellpadding='10' cellspacing='0'>";
        TestoMail += "<tr style='background-color:#e0e0e0;;border-color:#e0e0e0'><td style='font-size:14px;'>";
        TestoMail += "<br/><b>Indirizzo Fatturazione</b> :<br/> ";
        TestoMail += totali.Indirizzofatturazione + "<br/>";
        TestoMail += "</td>";
        TestoMail += "<td>";
        if (!string.IsNullOrEmpty(totali.Indirizzospedizione))
        {
            TestoMail += "<br/><b>Indirizzo Spedizione</b> :<br/>";
            TestoMail += totali.Indirizzospedizione;
        }
        TestoMail += "</td></tr></table/>";
        TestoMail += "</td></tr>";

        if (!string.IsNullOrEmpty(totali.Note))
            TestoMail += "<tr><td> <br/>Note : " + totali.Note + "<br/></td></tr>";
        TestoMail += "<tr><td><table style='border:1px solid #e0e0e0' cellpadding='10' cellspacing='0'>";
        TestoMail += "<tr><td style='font-size:14px;border-bottom:1px solid #e0e0e0'><b>CODICE ORDINE :</b> " + totali.CodiceOrdine + "<br/></td></tr>";
        int i = 1;
        foreach (Carrello item in prodotti)
        {
            TestoMail += "<tr><td style=' font-size:14px;border-bottom:1px solid #ccc'><br/><b>Articolo:</b> " + item.Offerta.DenominazioneI + "<br/>";
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

            TestoMail += "</td></tr>";
            i++;
        }
        TestoMail += "<tr><td><br/>Totale articoli " + totali.TotaleOrdine + " € </td></tr>";
        if (totali.TotaleSconto != 0)
            TestoMail += "<tr><td>Sconto applicato " + totali.TotaleSconto + " € </td></tr>";
        TestoMail += "<tr><td>";
        if (totali.TotaleSpedizione != 0)
            TestoMail += "Spese di spedizione " + totali.TotaleSpedizione + "  €<br/>";
        if (totali.TotaleSmaltimento != 0)
            TestoMail += "<br/>Spese di smaltimeto(PFU) " + totali.TotaleSmaltimento + "  €<br/>";
        if (totali.TotaleAssicurazione != 0)
            TestoMail += "Totale Assicurazione " + totali.TotaleAssicurazione + "  €<br/>";
        if (totali.Nassicurazioni != 0)
            TestoMail += "( Assicurazione per " + totali.Nassicurazioni + "  Persone )<br/>";
        TestoMail += "<br/><b>TOTALE ORDINE COMPLESSIVO: </b>" + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";
        if (totali.Percacconto != 100)
        {
            TestoMail += "<tr><td><b>RICHIESTO PAGAMENTO ACCONTO " + totali.Percacconto + "% :</b> " + totali.TotaleAcconto + " €</td></tr>";
            TestoMail += "<tr><td><b>DA SALDARE " + ":</b> " + totali.TotaleSaldo + " €</td></tr>";
        }
        else
            TestoMail += "<tr><td><b>RICHIESTO PAGAMENTO SALDO :</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

        TestoMail += "<tr><td><br/><b>Metodo di pagamento: </b>" + references.ResMan("Common", Lingua, "chk" + totali.Modalitapagamento).ToString() + " </td></tr>";
        TestoMail += "<tr><td>" + references.ResMan("Common", Lingua, "chk" + totali.Modalitapagamento + "dettaglio").ToString() + " </td></tr>";
        //chiudo tabella e riga relativa
        TestoMail += "</table></td><tr/>";
        TestoMail += "<tr><td>";

        if (!string.IsNullOrEmpty(references.ResMan("Common", Lingua, "chkcondizioni").ToString()))
            TestoMail += "<tr><td><br/>Il cliente ha selezionato :  " + references.ResMan("Common", Lingua, "chkcondizioni").ToString() + " </td></tr>";

        TestoMail += "</td></tr></table>";

        return TestoMail;
    }

    protected string CreaMailCliente(TotaliCarrello totali, CarrelloCollection prodotti)
    {
        string TestoMail = "";
        //MAIL PER IL CLIENTE DI CONFERMA ORDINE
        TestoMail += "<table cellpadding='0' cellspacing='0' style='font-size:14px;max-width:600px;width:100%' ><tr><td  valign='top'>" + "<img width=\"200\" src=\"" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "\" />" + "</td></tr>";
        //TestoMail = "<div style='width:600px;font-size:14px'><table  style='font-size:14px;max-width:600px;width:100%' cellpadding='0' cellspacing='0'><tr><td  valign='top'>" + "<img width=\"200\" src=\"http://testsite1.weapps.eu/images/main_logo.png\" />" + "</td></tr>";
        //Testo mail
        TestoMail += "<tr><td style='font-size:14px;'><br/> " + references.ResMan("Common", Lingua, "OrdineSoggettomailRiepilogo") + "<a href='" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "'>" + Nome + "</a> da " + totali.Denominazionecliente + ".<br/>";
        TestoMail += references.ResMan("Common", Lingua, "testointromailcliente");
        //TestoMail += "<br/><font color='#e12222'>Dettaglio Ordine</font> " + "<br/>";
        TestoMail += "</td></tr>";

        TestoMail += "<tr><td><br/><table  style='font-size:14px;max-width:600px;width:100%'  cellpadding='10' cellspacing='0'>";
        TestoMail += "<tr  style='background-color:#e0e0e0;border-color:#e0e0e0'><td style='font-size:14px;'>";
        TestoMail += "<br/><b>Indirizzo Fatturazione</b> :<br/> ";
        TestoMail += totali.Indirizzofatturazione + "<br/>";
        TestoMail += "</td>";
        TestoMail += "<td>";
        if (!string.IsNullOrEmpty(totali.Indirizzospedizione))
        {
            TestoMail += "<br/><b>Indirizzo Spedizione</b> :<br/>";
            TestoMail += totali.Indirizzospedizione;
        }
        TestoMail += "</td></tr></table/>";
        TestoMail += "</td></tr>";



        if (!string.IsNullOrEmpty(totali.Note))
            TestoMail += "<tr><td> <br/>Note : " + totali.Note + "<br/></td></tr>";

        TestoMail += "<tr><td><table style='border:1px solid #e0e0e0' cellpadding='10' cellspacing='0'>";
        TestoMail += "<tr><td style='font-size:14px;border-bottom:1px solid #e0e0e0'><b>CODICE ORDINE :</b> " + totali.CodiceOrdine + "<br/></td></tr>";
        int i = 1;
        foreach (Carrello item in prodotti)
        {
            TestoMail += "<tr><td style=' font-size:14px;border-bottom:1px solid #ccc'><br/><b>Articolo:</b> ";
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
                case "DE":
                    TestoMail += item.Offerta.DenominazioneDE + "<br/>";
                    break;
                case "ES":
                    TestoMail += item.Offerta.DenominazioneES + "<br/>";
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

            TestoMail += "</td></tr>";
            i++;
        }


        //RIEPILOGO ORDINE
        TestoMail += "<tr><td><br/>Totale articoli " + totali.TotaleOrdine + " € </td></tr>";
        if (totali.TotaleSconto != 0)
            TestoMail += "<tr><td>Sconto applicato " + totali.TotaleSconto + " € </td></tr>";
        TestoMail += "<tr><td>";
        if (totali.TotaleSpedizione != 0)
            TestoMail += "Spese di spedizione " + totali.TotaleSpedizione + "  €<br/>";
        if (totali.TotaleSmaltimento != 0)
            TestoMail += "<br/>Spese di smaltimeto(PFU) " + totali.TotaleSmaltimento + "  €<br/>";
        if (totali.TotaleAssicurazione != 0)
            TestoMail += "Totale Assicurazione " + totali.TotaleAssicurazione + "  €<br/>";
        if (totali.Nassicurazioni != 0)
            TestoMail += "( Assicurazione per " + totali.Nassicurazioni + "  Persone )<br/>";
        TestoMail += "<br/><b>TOTALE COMPLESSIVO ORDINE: </b>" + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

        if (totali.Percacconto != 100)
        {
            TestoMail += "<tr><td><b>RICHIESTO PAGAMENTO ACCONTO " + totali.Percacconto + "% :</b> " + totali.TotaleAcconto + " €</td></tr>";
            TestoMail += "<tr><td><b>DA SALDARE ENTRO 30 GG DATA PARTENZA " + ":</b> " + totali.TotaleSaldo + " €</td></tr>";
        }
        else
            TestoMail += "<tr><td><b>RICHIESTO PAGAMENTO SALDO  :</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

        TestoMail += "<tr><td><br/><b>Metodo di pagamento: </b>" + references.ResMan("Common", Lingua, "chk" + totali.Modalitapagamento).ToString() + " </td></tr>";
        TestoMail += "<tr><td>" + references.ResMan("Common", Lingua, "chk" + totali.Modalitapagamento + "dettaglio").ToString() + " </td></tr>";
        //chiudo tabella e riga relativa
        TestoMail += "</table></td><tr/>";


        //testo di chiusura
        TestoMail += "<tr><td style=' font-size:14px;'><br/>" + references.ResMan("Common", Lingua, "TestoConfermaOrdine" + totali.Modalitapagamento).ToString() + " </td></tr>";
        TestoMail += "<tr><td style=' font-size:14px;'><br/>" + references.ResMan("Common", Lingua, "TestoConfermaOrdine").ToString() + " </td></tr>";
        TestoMail += "<tr><td style=' font-size:14px;'><br/>" + references.ResMan("Common", Lingua, "TestoSaluti").ToString() + "<br/>" + references.ResMan("Common", Lingua, "TestoHomeIndex").ToString() + "</td></td> <br/>";
        //Inserisco il footer con i dati
        TestoMail += "<tr><td style='text-align:center; font-size:14px;'><br/><br/>" + references.ResMan("Common", Lingua, "txtFooter").ToString();
        TestoMail += "</td></tr></table></div>";

        return TestoMail;
    }


    //protected void inpContanti_ServerChange(object sender, EventArgs e)
    //{
    //    CaricaCarrello();
    //}

    //protected void inpBonifico_ServerChange(object sender, EventArgs e)
    //{
    //    CaricaCarrello();

    //}
    //protected void inpPaypal_ServerChange(object sender, EventArgs e)
    //{
    //    CaricaCarrello();
    //}

}