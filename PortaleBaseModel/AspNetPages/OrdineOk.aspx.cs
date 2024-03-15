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

            HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
            metarobots.Attributes["Content"] = "noindex,follow";

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


            /////////////////////////////////////////////////////////////////
            //CODICI PER TRANSAZIONE PAYPAL REST!!!
            /////////////////////////////////////////////////////////////////
            string idordine_PP = CaricaValoreMaster(Request, Session, "tranrestpp");
            string Encodedidordine_PP = idordine_PP;
            error = string.Empty;
            idordine_PP = dataManagement.DecodeFromBase64(idordine_PP);
            //Fare chiamata per stripe per visualizzare l'accaduto stripe
            if (!string.IsNullOrEmpty(idordine_PP)) //PAGAMENTO stripe
            {
                error = CaricaValoreMaster(Request, Session, "error");
                if (error == "true")
                {
                    //errore stripe operazione...da visualizzare
                    VisualizzaErrorpaypalrest(idordine_PP, Encodedidordine_PP); //da completare
                    return;
                }
                else
                {
                    /// successo operazione stripe .. da visualizzare
                    Visualizzasuccepaypalrest(idordine_PP, Encodedidordine_PP); //da completare
                    return;
                }
            }

            /////////////////////////////////////////////////////////////////
            //CODICI PER TRANSAZIONE STRIPE!!!
            /////////////////////////////////////////////////////////////////
            string idordine_stripe = CaricaValoreMaster(Request, Session, "stripetran");
            string Encodedidordine_stripe = idordine_stripe;
            error = string.Empty;
            idordine_stripe = dataManagement.DecodeFromBase64(idordine_stripe);
            //Fare chiamata per stripe per visualizzare l'accaduto stripe
            if (!string.IsNullOrEmpty(idordine_stripe)) //PAGAMENTO stripe
            {
                error = CaricaValoreMaster(Request, Session, "error");
                if (error == "true")
                {
                    //errore stripe operazione...da visualizzare
                    VisualizzaErrorstripe(idordine_stripe, Encodedidordine_stripe); //da completare
                    return;
                }
                else
                {
                    /// successo operazione stripe .. da visualizzare
                    Visualizzasuccestripe(idordine_stripe, Encodedidordine_stripe); //da completare
                    return;
                }
            }
            DataBind();

        }
    }

    private void Visualizzasuccepaypalrest(string idordine, string encodedidordine)
    {
        string CodiceOrdine = idordine;
        //output.Text += references.ResMan("Common", Lingua, "risposta_5");
        if (Session["thankyoumessages"] != null)
            output.Text += Session["thankyoumessages"].ToString();// references.ResMan("Common", Lingua, "thankyoumessages");
        Session.Remove("thankyoumessages");
    }
    private void VisualizzaErrorpaypalrest(string idordine, string encodedidordine)
    {
        // throw new NotImplementedException();
        output.Text = references.ResMan("Common", Lingua, "risposta_4").ToString() + " Order: " + idordine + "<br/>";

        if (Session["thankyoumessages"] != null)
            output.Text += Session["thankyoumessages"].ToString();// references.ResMan("Common", Lingua, "thankyoumessages");
        Session.Remove("thankyoumessages");
        pnlbtnretry.Visible = true;
    }

    private void Visualizzasuccestripe(string idordine_stripe, string encodedidordine_stripe)
    {
        string CodiceOrdine = idordine_stripe;
        //output.Text += references.ResMan("Common", Lingua, "risposta_5");
        if (Session["thankyoumessages"] != null)
            output.Text += Session["thankyoumessages"].ToString();// references.ResMan("Common", Lingua, "thankyoumessages");
        Session.Remove("thankyoumessages");

        //Queste attività le ho fatte prima nell handler prima di chiamare la thankyoupage
#if false
        Cliente cliente = new Cliente();
        CarrelloCollection prodotti = new CarrelloCollection();
        TotaliCarrello totali = new TotaliCarrello();
        if (Session["totali_" + CodiceOrdine] != null && Session["cliente_" + CodiceOrdine] != null && Session["prodotti_" + CodiceOrdine] != null)
        {
            cliente = (Cliente)Session["cliente_" + CodiceOrdine];
            Session.Remove("cliente_" + CodiceOrdine);
            totali = (TotaliCarrello)Session["totali_" + CodiceOrdine];
            Session.Remove("totali_" + CodiceOrdine);
            prodotti = (CarrelloCollection)Session["prodotti_" + CodiceOrdine];
            Session.Remove("prodotti_" + CodiceOrdine);

            string jscodetoinject = CommonPage.Creaeventopurchaseagooglegtag(totali, prodotti);
            output.Text = jscodetoinject;
            output.Text += references.ResMan("Common", Lingua, "risposta_5");
            output.Text += " Order: " + idordine_stripe;
            output.Text += references.ResMan("Common", Lingua, "GoogleConversione");
        }
#endif

    }

    private void VisualizzaErrorstripe(string idordine_stripe, string encodedidordine_stripe)
    {
        // throw new NotImplementedException();
        output.Text = references.ResMan("Common", Lingua, "risposta_4").ToString() + " Order: " + idordine_stripe + "<br/>";

        if (Session["thankyoumessages"] != null)
            output.Text += Session["thankyoumessages"].ToString();// references.ResMan("Common", Lingua, "thankyoumessages");
        Session.Remove("thankyoumessages");

        pnlbtnretry.Visible = true;
    }

    private void GetShippingDetails(string idordine, string Encodedidordine)
    {
        //Creo una variabile per la scrittura dei messaggi nel file di log
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");

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
            Messaggi["Messaggio"] += "Before calling GetShippingDetails, ordine: " + idordine + " " + System.DateTime.Now.ToString() + "\r\n";

            //Chiamo paypal GetExpressCheckoutDetails per avere il payerid e procedere
            bool ret = test.GetShippingDetails(HttpContext.Current.Session["token"].ToString(), ref payerid, ref email, ref firstname, ref lastname, ref amount, ref shipaddress, ref retMsg);
            if (ret)
            {
                Messaggi["Messaggio"] += "GetShippingDetails ok Paypal, ordine: " + idordine + " " + System.DateTime.Now.ToString() + "\r\n";
                // output.Text += "Payerid:" + payerid + " " + shipaddress;
#if true
                //Per eseguire la transazione finale qui
                //chiamo il metodo ConfirmPayment col payerid, il token provenienti e l'importo da addebitare delle due richieste precedenti
                //setExpressCheckout e GetShippingDetails -> 
                if (test.ConfirmPayment(amount, HttpContext.Current.Session["token"].ToString(), payerid, ref retMsg, authandcapturemode))
                {
                    Messaggi["Messaggio"] += "ConfirmPayment ok Paypal, ordine:  " + idordine + " " + System.DateTime.Now.ToString() + "\r\n";
                    //Se ok, registro la transazione e mando le email di conferma
                    RegistrazioneOrdinePaypal(idordine, Encodedidordine);
                }
                else
                {
                    Messaggi["Messaggio"] += "ConfirmPayment error Paypal, ordine:  " + idordine + " " + System.DateTime.Now.ToString() + "\r\n";
                    //Response.Redirect("Paypal/APIError.aspx?" + retMsg);
                    string[] retCol = retMsg.Split('&');
                    string errcode = retCol.FirstOrDefault(c => c.Contains("ErrorCode")).Replace("ErrorCode=", ""); ;
                    string desc = retCol.FirstOrDefault(c => c.Contains("Desc")).Replace("Desc=", ""); ;
                    string desc2 = retCol.FirstOrDefault(c => c.Contains("Desc2")).Replace("Desc2=", ""); ;
                    output.Text = references.ResMan("Common", Lingua, "risposta_7").ToString() + " Errorcode " + errcode + "<br/>";
                    output.Text += desc + "<br/>";
                    output.Text += desc2 + "<br/>";
                    pnlbtnretry.Visible = true;
                    Messaggi["Messaggio"] += "ConfirmPayment error Paypal:  " + idordine + " " + output.Text + "\r\n";

                }
#endif
            }
            else
            {
                Messaggi["Messaggio"] += "GetShippingDetails error Paypal, ordine: " + idordine + " " + System.DateTime.Now.ToString() + "\r\n";
                //Response.Redirect("Paypal/APIError.aspx?" + retMsg);
                string[] retCol = retMsg.Split('&');
                string errcode = retCol.FirstOrDefault(c => c.Contains("ErrorCode")).Replace("ErrorCode=", ""); ;
                string desc = retCol.FirstOrDefault(c => c.Contains("Desc")).Replace("Desc=", ""); ;
                string desc2 = retCol.FirstOrDefault(c => c.Contains("Desc2")).Replace("Desc2=", ""); ;
                output.Text = references.ResMan("Common", Lingua, "risposta_6").ToString() + " Errorcode " + errcode + "<br/>";
                output.Text += desc + "<br/>";
                output.Text += desc2 + "<br/>";
                pnlbtnretry.Visible = true;
                Messaggi["Messaggio"] += "GetShippingDetails error Paypal, ordine: " + idordine + " " + output.Text + "\r\n";

            }
        }
#endif

        WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune);
    }

    private void RegistrazioneOrdinePaypal(string CodiceOrdine, string Encodedidordine)
    {
        //Creo una variabile per la scrittura dei messaggi nel file di log
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");
        Messaggi["Messaggio"] += "RegistrazioneOrdinePaypal ok, da registrare ordine : " + CodiceOrdine + " " + System.DateTime.Now.ToString() + "\r\n";

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
        int J = 0;
        foreach (Carrello item in prodotti)
        {
            item.CodiceOrdine = CodiceOrdine;
            SalvaCodiceOrdine(item);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Prepariamo le richieste di feeback per gli articoli in ordine!!
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            try
            {
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



#if true
            // da abilitare per decrementare le quantià vendute
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
        Messaggi["Messaggio"] += "RegistrazioneOrdinePaypal ok, registrato ordine : " + CodiceOrdine + " " + System.DateTime.Now.ToString() + "\r\n";

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
                case "DE":
                    output.Text += "Error creating export order. Contact support.";
                    break;
                case "ES":
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
            //Utility.invioMailGenerico(Nome, Email, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);
            Utility.invioMailGenerico(totali.Denominazionecliente, totali.Mailcliente, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);
            //Invia la mail per il cliente
            string SoggettoMailCliente = references.ResMan("Common", Lingua, "OrdineSoggettomailRiepilogo") + Nome;
            TestoMail = CreaMailCliente(totali, prodotti);
            Utility.invioMailGenerico(Nome, Email, SoggettoMailCliente, TestoMail, totali.Mailcliente, totali.Denominazionecliente, null, "", true, Server);
        }
        catch { }


#endif
        WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune);

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
            //Utility.invioMailGenerico(Nome, Email, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);
            Utility.invioMailGenerico(totali.Denominazionecliente, totali.Mailcliente, SoggettoMailFornitore, TestoMail, Email, Nome, null, "", true, Server);
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
            TestoMail += "<tr><td><b>PAGATO ACCONTO </b> " + totali.Percacconto + "% :</b> " + (totali.TotaleAcconto) + " €</td></tr>";
            TestoMail += "<tr><td><b>DA SALDARE " + ":</b> " + totali.TotaleSaldo + " €</td></tr>";
        }
        else
            TestoMail += "<tr><td><b>PAGATO SALDO :</b> " + (totali.TotaleAcconto + totali.TotaleSaldo) + " €</td></tr>";

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
            TestoMail += "<tr><td><b>DA SALDARE " + ":</b> " + totali.TotaleSaldo + " €</td></tr>";
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




}