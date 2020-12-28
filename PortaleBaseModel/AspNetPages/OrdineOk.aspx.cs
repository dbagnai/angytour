using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;

public partial class AspNetPages_OrdineOk : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : "I"; }
        set { ViewState["Lingua"] = value; }
    }
    public string shoplogin
    {
        get { return ViewState["shoplogin"] != null ? (string)(ViewState["shoplogin"]) : ""; }
        set { ViewState["shoplogin"] = value; }
    }

    public string stringacrittata
    {
        get { return ViewState["stringacrittata"] != null ? (string)(ViewState["stringacrittata"]) : ""; }
        set { ViewState["stringacrittata"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Prendiamo i dati dalla querystring
            Lingua = CaricaValoreMaster(Request, Session, "Lingua");
            //Carico la galleria in masterpage corretta
            //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "vuoto", false, Lingua);


            /////////////////////////////////////////////////////////////////
            //CODICI PER TRANSAZIONE PAYWAY SOAR!!!
            /////////////////////////////////////////////////////////////////
            //STA [SNA] Si, No, Annullata
            //OID Order ID
            //CODAUTO Codice Autorizzazione = "xxxxxxxx"
            string sta = string.Empty;
            string oid = string.Empty;
            string codauto = string.Empty;
            if (Request.QueryString["STA"] != null && Request.QueryString["STA"] != "")
            { sta = Request.QueryString["STA"].ToString(); }
            if (Request.QueryString["OID"] != null && Request.QueryString["OID"] != "")
            { oid = Request.QueryString["OID"].ToString(); }
            if (Request.QueryString["CODAUTO"] != null && Request.QueryString["CODAUTO"] != "")
            { codauto = Request.QueryString["CODAUTO"].ToString(); }
            if (!string.IsNullOrEmpty(codauto) || !string.IsNullOrEmpty(oid) || !string.IsNullOrEmpty(sta))//PAGAMENTO TRAMITE PAYWAY SOAR
            {
                if (sta == "S")
                {
                    RegistrazioneOrdinePayway(oid, sta, codauto);
                }
                else
                {
                    VisualizzaErrorePayway(oid, sta, codauto);
                }
                return;
            }
            /////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////
            //CODICI PER TRANSAZIONE PAYPAL!!!
            /////////////////////////////////////////////////////////////////
            string idordine = string.Empty;
            string error = string.Empty;
            idordine = CaricaValoreMaster(Request, Session, "tran");
            string Encodedidordine = idordine;
            idordine = dataManagement.DecodeFromBase64(idordine);
            if (!string.IsNullOrEmpty(idordine)) //PAGAMENTO VIA PAYPAL
            {
                error = CaricaValoreMaster(Request, Session, "error");
                if (error == "true")
                {
                    VisualizzaErrorePaypal(idordine);
                    return;
                }
                //Carico i dettagli del venditore e della spedizione ed eseguo la transazione finale
                GetShippingDetails(idordine, Encodedidordine);
                return;
            }
            /////////////////////////////////////////////////////////////////

            DataBind();

        }
    }


    private void GetShippingDetails(string idordine, string Encodedidordine)
    {
#if true
        //CON UNA CHIAMATA GetExpressCheckoutDetails col token originale HttpContext.Current.Session["token"]  
        //prendo i dettagli del pagante da memorizzare o utilizzare per le email
        string user = ConfigManagement.ReadKey("userPaypal");
        string pwd = ConfigManagement.ReadKey("pwdPaypal");
        string signature = ConfigManagement.ReadKey("signaturePaypal");
        bool sandbox = Convert.ToBoolean(ConfigManagement.ReadKey("sandboxPaypal"));
        //string returl = (ConfigManagement.ReadKey("returlPaypal") + Encodedidordine + "&details=true";  
        //string cancelurl = (ConfigManagement.ReadKey("cancelurlPaypal"))) + Encodedidordine + "&error=true";  + "&error=true";
        NVPAPICaller test = new NVPAPICaller(user, pwd, signature, "", "", sandbox);
        string payerid = "";
        string shipaddress = "";
        string retMsg = "";
        string email = "";
        string amount = "";
        string firstname = "";
        string lastname = "";

        bool authandcapturemode = Convert.ToBoolean(ConfigManagement.ReadKey("authandcapturePaypal"));
        if (HttpContext.Current.Session["token"] != null)
        {
            //Chiamo paypal GetExpressCheckoutDetails per avere il payerid e procedere
            bool ret = test.GetShippingDetails(HttpContext.Current.Session["token"].ToString(), ref payerid, ref email, ref firstname, ref lastname, ref amount, ref shipaddress, ref retMsg);
            if (ret)
            {
                // output.Text += "Payerid:" + payerid + " " + shipaddress;
#if true
                //Per eseguire la transazione finale qui
                //chiamo il metodo ConfirmPayment col payerid, il token provenienti e l'importo da addebitare delle due richieste precedenti
                //setExpressCheckout e GetShippingDetails -> 
                if (test.ConfirmPayment(amount, HttpContext.Current.Session["token"].ToString(), payerid, ref retMsg, authandcapturemode))
                {
                    //Se ok, registro la transazione e mando le email di conferma
                    RegistrazioneOrdinePaypal(idordine, Encodedidordine);
                }
                else
                {
                    //Response.Redirect("Paypal/APIError.aspx?" + retMsg);
                    string[] retCol = retMsg.Split('&');
                    string errcode = retCol.FirstOrDefault(c => c.Contains("ErrorCode")).Replace("ErrorCode=", ""); ;
                    string desc = retCol.FirstOrDefault(c => c.Contains("Desc")).Replace("Desc=", ""); ;
                    string desc2 = retCol.FirstOrDefault(c => c.Contains("Desc2")).Replace("Desc2=", ""); ;
                    output.Text = references.ResMan("Common", Lingua, "risposta_7").ToString() + " Error" + errcode + "<br/>";
                    output.Text += desc + "<br/>";
                    output.Text += desc2 + "<br/>";
                }
#endif
            }
            else
            {
                //Response.Redirect("Paypal/APIError.aspx?" + retMsg);
                string[] retCol = retMsg.Split('&');
                string errcode = retCol.FirstOrDefault(c => c.Contains("ErrorCode")).Replace("ErrorCode=", ""); ;
                string desc = retCol.FirstOrDefault(c => c.Contains("Desc")).Replace("Desc=", ""); ;
                string desc2 = retCol.FirstOrDefault(c => c.Contains("Desc2")).Replace("Desc2=", ""); ;
                output.Text = references.ResMan("Common", Lingua, "risposta_6").ToString() + " Error" + errcode + "<br/>";
                output.Text += desc + "<br/>";
                output.Text += desc2 + "<br/>";
            }
        }
#endif

    }

    private void RegistrazioneOrdinePaypal(string CodiceOrdine, string Encodedidordine)
    {
        eCommerceDM ecom = new eCommerceDM();
        //Completiamo l'ordine registrando nel carrello e inviamo le email!! 
        Cliente cliente = new Cliente();
        CarrelloCollection prodotti = new CarrelloCollection();
        TotaliCarrello totali = new TotaliCarrello();

        if (Session["totali_" + CodiceOrdine] != null || Session["cliente_" + CodiceOrdine] != null || Session["prodotti_" + CodiceOrdine] != null)
        {
            cliente = (Cliente)Session["cliente_" + CodiceOrdine];
            Session.Remove("cliente_" + CodiceOrdine);
            totali = (TotaliCarrello)Session["totali_" + CodiceOrdine];
            Session.Remove("totali_" + CodiceOrdine);
            prodotti = (CarrelloCollection)Session["prodotti_" + CodiceOrdine];
            Session.Remove("prodotti_" + CodiceOrdine);

            string jscodetoinject = Creaeventopurchaseagooglegtag(totali, prodotti);
            output.Text = jscodetoinject;
            output.Text += references.ResMan("Common", Lingua, "risposta_5");
            output.Text += references.ResMan("Common", Lingua, "GoogleConversione");
        }


#if true

        //Qui devo scrivere nella tabella ordini
        //i dati qui memorizzati ( TBL_CARRELLO_ORDINI )
        //bool authandcapturemode = Convert.ToBoolean(ConfigurationManager.AppSettings["authandcapturePaypal"]);
        //if (!authandcapturemode)
        //    totali.Pagato = true; //Nel caso di transazione paypal con carta in modalità diretta!! la setto pagata!!!
        if (totali.Percacconto == 100)
        { totali.Pagatoacconto = true; totali.Pagato = true; }
        else
            totali.Pagatoacconto = true;


        ecom.InsertOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, totali);
        //AGGIORNO  I PRODOTTI NEL CARRELLO INSERENDO IL CODICE DI ORDINE
        //E GLI ALTRI DATI ACCESSORI ( TBL_CARRELLO )
        foreach (Carrello item in prodotti)
        {
            item.CodiceOrdine = CodiceOrdine;
            SalvaCodiceOrdine(item);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Prepariamo le richieste di feeback per gli articoli in ordine!!
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            try
            {
                int J = 0;
                Mail mailfeedback = new Mail();
                J++;
                if (J <= 2)
                {
                    mailfeedback.Sparedict["linkfeedback"] = "";//default preso dalle risorse feedbacksdefaultform
                    mailfeedback.Sparedict["idnewsletter"] = "";//default dalle risorse feedbackdefaultnewsletter
                    mailfeedback.Sparedict["deltagiorniperinvio"] = "";//default dalle risorse feedbacksdefaultdeltagg
                    mailfeedback.Sparedict["idclienti"] = cliente.Id_cliente.ToString();
                    mailfeedback.Id_card = item.id_prodotto;
                    HandlerNewsletter.preparamail(mailfeedback, Lingua); //Preparo le mail nello scheduler!!
                }
            }
            catch { }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#if true // da abilitare per decrementare le quantià vendute
            //Decrementiamo anche le quantità per i prodotti che sono a disponibilità limitata
            // togliendo dal catalogo la quantità venduta dell'articolo presente a carrello
            offerteDM offDM = new offerteDM();
            Offerte off = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item.id_prodotto.ToString());
            if (off != null)
            {
                if (off.Qta_vendita != null)
                {
                    off.Qta_vendita = off.Qta_vendita.Value - item.Numero;
                    if (off.Qta_vendita < 0) off.Qta_vendita = 0;
                    offDM.UpdateOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, off);
                    offDM.UpdateOffertaCollegata(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, off);
                }
                else if (!string.IsNullOrEmpty(off.Xmlvalue))
                {
                    //qui devo controllare le disponibilità per tagli/colore
                    List<ModelCarCombinate> listCarr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(off.Xmlvalue);
                    foreach (ModelCarCombinate elem in listCarr)
                    {
                        if (elem.id == item.Campo2)
                        {
                            long qtaCalc = 0;
                            qtaCalc = (Convert.ToInt64(elem.qta)) - item.Numero;
                            if (qtaCalc < 0) elem.qta = "0";
                            else elem.qta = qtaCalc.ToString();
                        }
                    }
                    //Adesso serializzo, sostituisco e risalvo
                    string ret = Newtonsoft.Json.JsonConvert.SerializeObject(listCarr);
                    off.Xmlvalue = ret;

                    offDM.UpdateOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, off);
                    offDM.UpdateOffertaCollegata(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, off);
                }
            }
#endif
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

        //Creiamo il file per export degli ordini...
        try
        {
            //CreaFileExportOrdini(totali, prodotti, cliente);
        }
        catch (Exception err)
        {
            output.Text = err.Message + " <br/> ";
            switch (Lingua)
            {
                case "I":
                    output.Text += "Errore creazione export ordini. Contattatare l'assistenza. ";
                    break;
                case "GB":
                    output.Text += "Error creating export order. Contact support.";
                    break;
                case "RU":
                    output.Text += "Error creating export order. Contact support.";
                    break;
                case "FR":
                    output.Text += "Error creating export order. Contact support.";
                    break;
            }
        }

        try
        {
            //Inviamo le email di conferma al portale ed al cliente
            string TestoMail = "";
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


#endif

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
                    tmpelement.Status = 1; //Stato confermato dell'evento/prenotazione pagate con paypal
                    tmpelement.Jsonfield1 = c.jsonfield1;

                    bookingDM.dbInsertEvent(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tmpelement);
                }
                catch { }
            }

        }
    }
    private void VisualizzaErrorePaypal(string codiceordine)
    {
        output.Text = "Errore transazione non avvenuta, cod " + codiceordine;
        TotaliCarrello totali = new TotaliCarrello();
        Cliente cliente = new Cliente();
        CarrelloCollection prodotti = new CarrelloCollection();
        if (Session["totali_" + codiceordine] != null || Session["cliente_" + codiceordine] != null || Session["prodotti_" + codiceordine] != null)
        {
            totali = (TotaliCarrello)Session["totali_" + codiceordine];
            Session.Remove("totali_" + codiceordine);
            cliente = (Cliente)Session["cliente_" + codiceordine];
            Session.Remove("cliente_" + codiceordine);
            prodotti = (CarrelloCollection)Session["prodotti_" + codiceordine];
            Session.Remove("prodotti_" + codiceordine);
            //  output.Text += "Url per ripetere il pagamento: " + cliente.Spare2; //LA RIPETIZ PAGAMENTO HA PROBLEMI LA SESSIONE MUORE!
        }
    }



    private void VisualizzaErrorePayway(string oid, string sta, string codauto)
    {
        output.Text = "Errore transazione non avvenuta, cod " + codauto + " errore " + sta;
        string codiceordine = oid;
        TotaliCarrello totali = new TotaliCarrello();
        Cliente cliente = new Cliente();
        CarrelloCollection prodotti = new CarrelloCollection();

        if (Session["totali_" + codiceordine] != null || Session["cliente_" + codiceordine] != null || Session["prodotti_" + codiceordine] != null)
        {

            totali = (TotaliCarrello)Session["totali_" + codiceordine];
            Session.Remove("totali_" + codiceordine);
            cliente = (Cliente)Session["cliente_" + codiceordine];
            Session.Remove("cliente_" + codiceordine);
            prodotti = (CarrelloCollection)Session["prodotti_" + codiceordine];
            Session.Remove("prodotti_" + codiceordine);
            //  output.Text += "Url per ripetere il pagamento: " + cliente.Spare2; //LA RIPETIZ PAGAMENTO HA PROBLEMI LA SESSIONE MUORE!
        }
    }

    private void RegistrazioneOrdinePayway(string oid, string sta, string codauto)
    {

        string CodiceOrdine = oid;
        eCommerceDM ecom = new eCommerceDM();
        //Completiamo l'ordine registrando nel carrello e inviamo le email!! 
        Cliente cliente = new Cliente();
        CarrelloCollection prodotti = new CarrelloCollection();
        TotaliCarrello totali = new TotaliCarrello();
        if (Session["totali_" + CodiceOrdine] != null || Session["cliente_" + CodiceOrdine] != null || Session["prodotti_" + CodiceOrdine] != null)
        {
            cliente = (Cliente)Session["cliente_" + CodiceOrdine];
            Session.Remove("cliente_" + CodiceOrdine);
            totali = (TotaliCarrello)Session["totali_" + CodiceOrdine];
            Session.Remove("totali_" + CodiceOrdine);
            prodotti = (CarrelloCollection)Session["prodotti_" + CodiceOrdine];
            Session.Remove("prodotti_" + CodiceOrdine);
            output.Text = references.ResMan("Common", Lingua, "risposta_5");
        }

        //Siccome tutto a buon fine -> setto pagato ( in realtà nella modalità con accettazione
        // su payway del venditaore il pagato lo dovrebbe settare lui nella gestione ordini ) !!!
        if (totali.Percacconto == 100)
        { totali.Pagatoacconto = true; totali.Pagato = true; }
        else
            totali.Pagatoacconto = true;

        //Qui devo scrivere nella tabella ordini
        //i dati qui memorizzati ( TBL_CARRELLO_ORDINI )
        //bool authandcapturemode = Convert.ToBoolean(ConfigManagement.ReadKey("authandcapturePaypal"));
        //if (!authandcapturemode)
        //    totali.Pagato = true; //Nel caso di transazione paypal con carta in modalità diretta!! la setto pagata!!!
        ecom.InsertOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, totali);
        //AGGIORNO  I PRODOTTI NEL CARRELLO INSERENDO IL CODICE DI ORDINE
        //E GLI ALTRI DATI ACCESSORI ( TBL_CARRELLO )
        foreach (Carrello item in prodotti)
        {
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

        try
        {
            //Inviamo le email di conferma al portale ed al cliente
            string TestoMail = "";
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
        catch
        {

            output.Text += "Errore invio email di conferma / Error sending confirmation email ";
        }
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
            output.Text = err.Message + " <br/> ";
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
        TestoMail += "<tr><td><table cellpadding='0' cellspacing='0' style='font-size:14px;'><tr><td><br/> DETTAGLIO ORDINE </td></tr>";
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


        TestoMail += "<tr><td>Totale Articoli " + totali.TotaleOrdine + " € </td></tr>";
        if (totali.TotaleSconto != 0)
            TestoMail += "<tr><td><br/>Sconto applicato " + totali.TotaleSconto + " € </td></tr>";
        TestoMail += "<tr><td>";
        if (totali.TotaleSpedizione != 0)
            TestoMail += "<br/>Spese di spedizione " + totali.TotaleSpedizione + " €<br/>";
        if (totali.TotaleSmaltimento != 0)
            TestoMail += "<br/>Spese smaltimento(PFU) " + totali.TotaleSmaltimento + " €<br/>";
        TestoMail += "<b>Totale ordine complessivo:</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

        if (totali.Percacconto != 100)
        {
            TestoMail += "<tr><td><b>PAGATO ACCONTO </b> " + totali.Percacconto + "% :</b> " + (totali.TotaleAcconto) + " €</td></tr>";
            TestoMail += "<tr><td><b>DA SALDARE ENTRO 30 GG DATA PARTENZA" + ":</b> " + totali.TotaleSaldo + " €</td></tr>";
        }
        else
            TestoMail += "<tr><td><b>PAGATO SALDO  :</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

        TestoMail += "<tr><td><br/>Metodo di pagamento:  " + references.ResMan("Common", Lingua, "chk" + totali.Modalitapagamento).ToString() + " </td></tr>";
        //chiudo tabella e riga relativa
        TestoMail += "</table></td><tr/>";
        TestoMail += "<tr><td><br/>L'utente ha correttamente pagato tramite la modalità qui indicata, è in attesa di vorstro contatto per la  spedizione della merce.";

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
            //scaglione completo nel carrello
            //Scaglioni scaglionedacarrello =  Newtonsoft.Json.JsonConvert.DeserializeObject<Scaglioni>((String)eCommerceDM.Selezionadajson(item.jsonfield1, "scaglione", Lingua));
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

        TestoMail += "<tr><td>Totale Articoli " + totali.TotaleOrdine + " € </td></tr>";
        if (totali.TotaleSconto != 0)
            TestoMail += "<tr><td><br/>Sconto applicato " + totali.TotaleSconto + " € </td></tr>";
        TestoMail += "<tr><td>";
        if (totali.TotaleSpedizione != 0)
            TestoMail += "<br/>Spese di spedizione " + totali.TotaleSpedizione + " €<br/>";
        if (totali.TotaleSmaltimento != 0)
            TestoMail += "<br/>Spese smaltimento(PFU) " + totali.TotaleSmaltimento + " €<br/>";
        TestoMail += "<b>Totale ordine complessivo:</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

        if (totali.Percacconto != 100)
        {
            TestoMail += "<tr><td><b>PAGATO ACCONTO </b> " + totali.Percacconto + "% :</b> " + (totali.TotaleAcconto) + " €</td></tr>";
            TestoMail += "<tr><td><b>DA SALDARE ENTRO 30 GG DATA PARTENZA " + ":</b> " + totali.TotaleSaldo + " €</td></tr>";
        }
        else
            TestoMail += "<tr><td><b>PAGATO SALDO  :</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

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



}