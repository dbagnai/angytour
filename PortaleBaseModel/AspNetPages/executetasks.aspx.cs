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
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Collections.Generic;

public partial class _executetasks : CommonPage
{
    private int maxinviiperchiamata = 900; //Numero max di invi per chiamata del metodo di esecuzione del mailing
    private int millisecondbetweenmails = 300; //Numero max di invi per chiamata del metodo di esecuzione del mailing

    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string Azione
    {
        get { return ViewState["Azione"] != null ? (string)(ViewState["Azione"]) : ""; }
        set { ViewState["Azione"] = value; }
    }
    public string idCliente
    {
        get { return ViewState["idCliente"] != null ? (string)(ViewState["idCliente"]) : ""; }
        set { ViewState["idCliente"] = value; }
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
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");
        Messaggi["Messaggio"] += "Page_Load(): Esecuzione pagina task: " + System.DateTime.Now.ToString();

        if (WelcomeLibrary.STATIC.Global.statomailing) // imposto lo stato di lavori in corso
        {
            litMainContent.Text = "Richiesta in corso, impossibile fare richieste duplicate.";
            return;
        }
        WelcomeLibrary.STATIC.Global.statomailing = true; //Imposto lo stato di mailing in corso per evitare incroci con altre chiamate alla pagina
        try
        {
            if (!IsPostBack)
            {
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;

                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                Azione = CaricaValoreMaster(Request, Session, "Azione", true, "");
                idCliente = CaricaValoreMaster(Request, Session, "idCliente", true, "");

                switch (Azione)
                {
                    case "unsubscribe":
                        ClientiDM cliDM = new ClientiDM();
                        //idCliente
                        cliDM.unsubscribeCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idCliente);
                        litMainContent.Text = references.ResMan("Common", Lingua, "rispostaUnsubscribe").ToString();
                        break;
                    case "aggiornacontenutiweb":
                        AggiornaContenutiDaWeb();
                        break;
                    case "modiximport":



                        ModixImport("rif000100");
                        break;
                    default:
                        //VerificaScadenzeCard();
                        EseguiMailing();
                        break;
                }

                // litMainContent.Text = "";
            }
            else
            {
                litMainContent.Text = "";
            }

        }
        catch (Exception err)
        {
            //DEVI LOGGARE EVENTUALI ERRORI IN OPPORTUNO FILE DI LOG
            Messaggi["Messaggio"] += "Page_Load(): Errore pagina task: " + err.Message + " " + System.DateTime.Now.ToString();
        }
        finally
        {
            WelcomeLibrary.STATIC.Global.statomailing = false; //Imposto lo stato di esecuzione terminata!!!
        }
        WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
    }
#if false

    /// <summary>
    /// Verifico su tutte le card attive quelle che sono prossime alla scadenza
    /// e preparo le email da inviare ai clienti per la proposta di rinnovo
    /// </summary>
    private void VerificaScadenzeCard()
    {
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");

        try
        {
            CardsDM cDM = new CardsDM();
            string debugtext = "";
            CardCollection cards = cDM.EstraiCardsProssimeScadenza(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, 30, ref debugtext);

            // litMainContent.Text = debugtext +  " N.Cards a scadere : " + cards.Count.ToString();

            //Adesso creaimo i record per l'invio delle email controllando che per le card in questione
            //non siano già state inviate email! ( si usa l'id_card e Tipomailing )
            mailingDM mDM = new mailingDM();
            MailCollection mailsinviate = mDM.CaricaMailFiltratePerIdcardTipomailing(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cards, enumclass.TipoMailing.AvvisoScadenzaCard);
            //enumclass.TipoMailing.AvvisoInserimentoStruttura -> 1
            //enumclass.TipoMailing.AvvisoScadenzaCard -> 0
            //enumclass.TipoMailing result = new enumclass.TipoMailing();
            //Enum.TryParse<enumclass.TipoMailing>("1", out result);
            //litMainContent.Text = result.ToString();
            //litMainContent.Text += " " + ((int)result);

            //Togliamo dalla lista mail da mandare quelle relative a cards già inviate o pronte per l'invio
            if (mailsinviate != null)
                cards.RemoveAll(delegate(Card c) { return mailsinviate.Exists(delegate(Mail m) { return m.Id_card == c.Id_card; }); });

            //Prepariamo i record tabella mailing per l'invio degli avvisi di scadenza ai clienti
            ClientiDM cliDm = new ClientiDM();
            ClienteCollection clienti = cliDm.CaricaClientiInbaseacards(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cards);

            // litMainContent.Text += "N. clienti relativi alle card a scadere : " + clienti.Count.ToString();


            Mail item = new Mail();
            MailCollection maildainviare = new MailCollection();
            if (clienti != null)
            {
                foreach (Cliente c in clienti)
                {
                    item = new Mail();
                    item.DataInserimento = System.DateTime.Now;
                    item.Id_card = c.Id_card;
                    item.Id_cliente = c.Id_cliente;
                    item.Lingua = c.Lingua;
                    item.Tipomailing = (Int32)enumclass.TipoMailing.AvvisoScadenzaCard;
                    item.NoteInvio = "Invio automatico mail per scadenza card cliente.";
                    item.SoggettoMail = GetLocalResourceObject("oggettoMailScadenzaCard").ToString();
                    item.TestoMail = GetLocalResourceObject("testoMailScadenzaCard").ToString();
                    maildainviare.Add(item);
                    mDM.InserisciAggiornaMail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item); //Inseriamo nel db per l'invio
                }
                Messaggi["Messaggio"] += "VerificaScadenzeCard() Preparate " + clienti.Count + " mail.";
                WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
            }

        }
        catch (Exception err)
        {
            //DEVI LOGGARE EVENTUALI ERRORI IN OPPORTUNO FILE DI LOG
            Messaggi["Messaggio"] += "VerificaScadenzeCard(): Errore invio mail: " + err.Message + " " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
        }
    }
    
#endif

    void ModixImport(string codicedestinazione)
    {
        //Procedura per zippare una cartella
        //string sourcefilespath = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/_modix/");
        //string destinationarchive = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/compressedfile.zip");
        //WelcomeLibrary.UF.Utility.ZipCompletefolder(destinationarchive, "", sourcefilespath);

        //Unzippo il file originale
        string sourcefilezippath = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/_modix/stock_rubeca.zip");
        string destinationfilepath = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/_modix/");
        WelcomeLibrary.UF.Utility.UnZip(sourcefilezippath, destinationfilepath, "automotive.it-IT.xml");
        string UrlSorgente = WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/_modix/automotive.it-IT.xml";

        ParseXmlFile(Server, UrlSorgente, codicedestinazione
                          , ConfigManagement.ReadKey("Web_Numerocontenuti"));

    }

    void AggiornaContenutiDaWeb(string UrlSorgente, string codicedestinazione)
    {
        CommonPage.GetContentFromWeb(Server, UrlSorgente, codicedestinazione
                           , ConfigManagement.ReadKey("Web_Numerocontenuti"));
    }
    private void AggiornaContenutiDaWeb()
    {
        //http://www.eoipso.it/rss/comunicati/rss_122.xml //--> rif000061
        //http://www.eoipso.it/rss/rassegnestampa/rss_122.xml //--> rif000060

        AggiornaContenutiDaWeb("http://www.eoipso.it/rss/comunicati/rss_122.xml", ConfigManagement.ReadKey("Web_CodiceDestinazioneComunicati")); //Comunicati
        AggiornaContenutiDaWeb("http://www.eoipso.it/rss/rassegnestampa/rss_122.xml", ConfigManagement.ReadKey("Web_CodiceDestinazioneRassegna")); //Comunicati
    }

    /// <summary>
    /// Leggo dalla tabella mail, se ci sono messaggi da inviare ed inoltro quelli in attesa
    /// E' possibile in base al tipo mailing diversificare l'invio delle email e la modalità
    /// </summary>
    private void EseguiMailing()
    {
        ////Creo una variabile per la scrittura dei messaggi nel file di log
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");
        try
        {
            mailingDM mDM = new mailingDM();

            #region PULIZIA TABELLE SISTEMA DI MAILING
            //Pulizia tabella mail prese in carico
            int oreattesapreseincarico = 3;
            mDM.PulisciTabellaMailPreseincarico(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, System.DateTime.Now.AddHours(-oreattesapreseincarico));
            //Per prima cosa pulisco la tabella di mailing dalle email + vecchie di una certa data
            int giornipercancellazione = 500; //Numero di giorni dopo cui le mail sono ripulite
            mDM.CancellaMailPerPulizia(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, System.DateTime.Now.AddDays(-giornipercancellazione));
            #endregion

            int countermail = 0;
            //Leggiamo ed esguiamo gli invii
            MailCollection mails = mDM.CaricaMailDaInviare(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, maxinviiperchiamata);
            mDM.MarcaMailpreseincarico(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, maxinviiperchiamata);

            string ret = "";
            foreach (Mail m in mails)
            {
                enumclass.TipoMailing tipomail = new enumclass.TipoMailing();
                Enum.TryParse<enumclass.TipoMailing>(m.Tipomailing.ToString(), out tipomail);
                switch (tipomail)
                {
                    case enumclass.TipoMailing.Newsletter_tipo1: //Mailing per newsletter
                        //invio dei mail per newsletter
                        ret = InviaMailing(m);
                        if (!string.IsNullOrEmpty(ret)) // In caso di problemi di invio memorizzo l'errore
                        {
                            Messaggi["Messaggio"] = "EseguiMailing(): Errore invio mail newsletter : " + ret;
                            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
                        }
                        else countermail++;
                        break;
                    //case enumclass.TipoMailing.AvvisoScadenzaCard: //MAIL PER AVVISI DI SCADENZA DELLE CARD ECONOMY ROOM
                    //    //Facciamo l'invio della mail
                    //    ret = InviaMailsScadenzaCardACliente(m);
                    //    if (!string.IsNullOrEmpty(ret)) // In caso di problemi di invio memorizzo l'errore
                    //    {
                    //        Messaggi["Messaggio"] = "EseguiMailing(): Errore invio mail avviso scadenza : " + ret;
                    //        WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
                    //    }
                    //    else countermail++;
                    //    break;
                    case enumclass.TipoMailing.AvvisoInserimentoStruttura:
                        //invio dei mail per inserimento nuova struttura ( che sono inseriti dalla gestione offerte a catalogo  
                        ret = InviaMailNuovaStrutturaACliente(m);
                        if (!string.IsNullOrEmpty(ret)) // In caso di problemi di invio memorizzo l'errore
                        {
                            Messaggi["Messaggio"] = "EseguiMailing(): Errore invio mail avviso inserimento nuova struttura : " + ret;
                            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
                        }
                        else countermail++;
                        break;
                    case enumclass.TipoMailing.AvvisoNuovaofferta:
                        //invio dei mail per inserimento nuova struttura ( che sono inseriti dalla gestione offerte a catalogo  
                        ret = InviaMailNuovaOffertaACliente(m);
                        if (!string.IsNullOrEmpty(ret)) // In caso di problemi di invio memorizzo l'errore
                        {
                            Messaggi["Messaggio"] = "EseguiMailing(): Errore invio mail avviso inserimento news : " + ret;
                            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
                        }
                        else countermail++;
                        break;
                    default:
                        break;
                }
                //Elimino la mail da quelle prese in carico dal mailer
                mDM.EliminaMaildapresaincarico(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, m.Id);
            }
            //LOGGO IL TOTALE DELLE MAIL INVIATE
            Messaggi["Messaggio"] += "EseguiMailing(): inviate " + countermail.ToString() + " mail in data " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
        }
        catch (Exception err)
        {
            //DEVI LOGGARE EVENTUALI ERRORI IN OPPORTUNO FILE DI LOG
            Messaggi["Messaggio"] += "EseguiMailing(): Errore invio mail: " + err.Message + " " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
        }
        finally
        {

        }
    }
    /// <summary>
    /// Invio della mail per la newsletter preparata
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private string InviaMailing(Mail m)
    {
        mailingDM mDM = new mailingDM();
        string ret = "";
        try
        {
            string SoggettoMail = m.SoggettoMail;
            //Dati per la mail
            string nomecliente = m.Cliente.Cognome + " " + m.Cliente.Nome;
            if (string.IsNullOrEmpty(nomecliente.Trim())) nomecliente = m.Cliente.Email;
            string Mailcliente = m.Cliente.Email;
            string Descrizione = m.TestoMail;
            Descrizione = Descrizione.Replace("ID_cliente=&", "ID_cliente=" + m.Id_cliente + "&");//Inserisco nel link di ritorno l'id del cliente
            Descrizione = Descrizione.Replace("ID_mail=&", "ID_mail=" + m.Id + "&");//Inserisco nel link di ritorno l'id della mail che invio

            //Personalizzazione invio a cliente col il nome
            SoggettoMail = SoggettoMail.Replace("|cliente|", nomecliente);
            Descrizione = Descrizione.Replace("|cliente|", nomecliente);//Inserisco nel link di ritorno l'id della mail che invio

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Replace nel testo descrizione della mail per inserire il link alla scheda feedcak ed i dati del rpodotto collegato eventuale
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<string, string> sparedict = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(m.NoteInvio) && m.NoteInvio.LastIndexOf("||{") != -1 && m.NoteInvio.LastIndexOf("}||") != -1)
            {
                if (m.NoteInvio.LastIndexOf("}||") - m.NoteInvio.LastIndexOf("||{") - 3 > 0)
                {
                    string serializesparedict = m.NoteInvio.Substring(m.NoteInvio.LastIndexOf("||{") + 3, m.NoteInvio.LastIndexOf("}||") - m.NoteInvio.LastIndexOf("||{") - 3);
                    sparedict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(serializesparedict);
                }

            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////
            //--> TAG POSSIBILI 
            //|feedbacklnk| -> link alla scheda in tbl_attivita cui fà riferimento il deedback  
            //|feedbackfrm| -> link alla form per inserimento del feedback  
            //|feedbacknme| -> nome del prodotto alla form per inserimento del feedback  
            //|feedbackimg| -> immagnie del prodotto alla form per inserimento del feedback  
            /////////////////////////////////////////////////////////////////////////////////////////////////////
            long idpost = m.Id_card; //CONTIENE L'ID del post cui fa riferimento il feedback dalla procedura handlernewsletter "inseriscimailrichiestafeedback":
            Dictionary<string, string> links = offerteDM.getlinklist(m.Lingua, idpost.ToString(), Session.SessionID);
            if (links != null && links.ContainsKey(idpost.ToString()))
            {
                Descrizione = Descrizione.Replace("|feedbacklnk|", links[idpost.ToString()]);
                if (links.ContainsKey(idpost.ToString() + "name"))
                    Descrizione = Descrizione.Replace("|feedbacknme|", links[idpost.ToString() + "name"]);
                if (links.ContainsKey(idpost.ToString() + "img"))
                    Descrizione = Descrizione.Replace("|feedbackimg|", links[idpost.ToString() + "img"]);
            }
            else
            {
                Descrizione = Descrizione.Replace("|feedbacklnk|", "");
                Descrizione = Descrizione.Replace("|feedbacknme|", "");
                Descrizione = Descrizione.Replace("|feedbackimg|", "");
            }

            if (sparedict.ContainsKey("linkfeedback"))
            {
                Descrizione = Descrizione.Replace("|feedbackfrm|", sparedict["linkfeedback"] + "?idpost=" + idpost.ToString() + "&idcliente=" + m.Id_cliente);
            }
            else
                Descrizione = Descrizione.Replace("|feedbackfrm|", "");

            /////////////////////////////////////////////////////////////////////////////////////////////////////


            //Personalizzazione del mittente
            string nomemittente = Nome;
            string mailmittente = Email;
            //-> da finire decidendo dove pescare eventuali diversi mittenti!!!!
            if (!string.IsNullOrWhiteSpace(m.NoteInvio) && m.NoteInvio.StartsWith("|"))
            {
                string _tmpemail = "";
                string _tmpnome = "";
                //Facciamo la richiesta variazione del mittente per la mail
                string testonote = m.NoteInvio;
                testonote = testonote.Substring(1);
                int primoseparatore = testonote.IndexOf("|");
                if (primoseparatore != -1)
                {
                    _tmpemail = testonote.Substring(0, primoseparatore);

                    if (_tmpemail.Contains("@"))
                    {
                        int secondoseparatore = testonote.IndexOf("|", primoseparatore + 1);
                        if (secondoseparatore != -1)
                        {
                            _tmpnome = testonote.Substring(primoseparatore + 1, secondoseparatore - primoseparatore - 1);
                            nomemittente = _tmpnome;
                            mailmittente = _tmpemail;
                        }
                    }
                }
            }
            //---------------------------------------------------------

            //INSERISCO IL LINK PER L'UNSUBSCRIBE DALLA NEWSLETTER
            string linkUnsubscribe = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/executetasks.aspx?Azione=unsubscribe&idCliente=" + m.Id_cliente + "&Lingua=" + m.Lingua;

            //Imposto manualmente la cultura per prendere la giusta descrizione del link unsubscribe in base alla lingua della mail da mandare!
            string culturename = "it";
            if (m.Lingua != "I")
                culturename = "en";
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culturename);
            //string value = HttpContext.references.ResMan("Common", m.Lingua,"TestoUnsubscribe", ci).ToString();
            string value = references.ResMan("Common", m.Lingua, "TestoUnsubscribe");

            //Variazione denominazione del mittente per l'unsubscribe ( se richiesto )
            value = value.ToLower().Replace(Nome.ToLower(), nomemittente.ToLower());

            //Devo prendere la risorsa per la lingua in base a m.lingua non alla lungua di visualizzazione della pagina
            if (Descrizione.IndexOf("</td></tr></table></body></html>") != -1)
                Descrizione = Descrizione.Insert(Descrizione.IndexOf("</td></tr></table></body></html>"), "<br/><a href=\"" + linkUnsubscribe + "\" target=\"_blank\" style=\"font-size:13px;color:#909090\">" + value + "</a><br/>");
            else
                Descrizione += "<br/><a href=\"" + linkUnsubscribe + "\" target=\"_blank\" style=\"font-size:13px;color:#909090\">" + value + "</a><br/>";

            Utility.invioMailGenerico(nomemittente, mailmittente, SoggettoMail, Descrizione, Mailcliente, nomecliente, null, "", false, Server, false, null, "mailing");
            m.NoteInvio += " | Invio eseguito correttamente.";
            m.DataInvio = System.DateTime.Now;
        }
        catch (Exception err)
        {
            ret = "InviaMailingACliente() : Errore invio mail a cliente, Id Mail: " + m.Id + " " + err.Message + "\r\n";
            //Settiamo la mail come in errore per evitare un nuovo invio della stessa e scriviamo l'errore nella stessa mail nel db
            m.Errore = true;
            m.TestoErrore = ret;
        }
        try
        {
            mDM.InserisciAggiornaMail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, m);
            System.Threading.Thread.Sleep(millisecondbetweenmails);//Attesa tra l'invio delle mail
        }
        catch (Exception error)
        {
            ret += "Errore aggionamento tabella mail dopo invio: " + error.Message;
        }
        return ret;
    }

    /// <summary>
    /// Manda la mail di avviso scadenza card al cliente
    /// </summary>
    /// <param name="m"></param>
    /// <param name="link"></param>
    /// <returns></returns>
    private string InviaMailsScadenzaCardACliente(Mail m)
    {

        mailingDM mDM = new mailingDM();
        string ret = "";
        try
        {
            string SoggettoMail = m.SoggettoMail;
            //Dati per la mail
            string nomecliente = m.Cliente.Cognome + " " + m.Cliente.Nome;
            string Mailcliente = m.Cliente.Email;
            string Descrizione = m.TestoMail + "<br/><br/>";
            string link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/RichiediCard.aspx?Lingua=" + m.Lingua.ToUpper();
            Descrizione += "<a href=\"" + link + "\" target=\"_blank\" style=\"font-size:22px;color:#b13c4e\">" + references.ResMan("Common", Lingua, "TitoloRichiedi").ToString() + "<br/><br/><br/>";

            string linkUnsubscribe = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/executetasks.aspx?Azione=unsubscribe&idCliente=" + m.Id_cliente + "&Lingua=" + Lingua;
            Descrizione += "<a href=\"" + linkUnsubscribe + "\" target=\"_blank\" style=\"font-size:13px;color:#909090\">" + references.ResMan("Common", Lingua, "TestoUnsubscribe").ToString() + "<br/><br/><br/>";


            Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente);
            m.NoteInvio = "Invio eseguito correttamente.";
            m.DataInvio = System.DateTime.Now;
        }
        catch (Exception err)
        {
            ret = "InviaMailsACliente() : Errore invio mail avviso scadenza card, Id Mail: " + m.Id + " " + err.Message + "\r\n";
            //Settiamo la mail come in errore per evitare un nuovo invio della stessa e scriviamo l'errore nella stessa mail nel db
            m.Errore = true;
            m.TestoErrore = ret;
        }
        try
        {
            mDM.InserisciAggiornaMail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, m);
            System.Threading.Thread.Sleep(millisecondbetweenmails);//Attesa tra l'invio delle mail
        }
        catch (Exception error)
        {
            ret += "Errore aggionamento tabella mail dopo invio: " + error.Message;
        }
        return ret;
    }

    /// <summary>
    /// Mando la mail di informazione per inserimento nuova struttura
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private string InviaMailNuovaStrutturaACliente(Mail m)
    {
        mailingDM mDM = new mailingDM();
        string ret = "";
        try
        {
            string SoggettoMail = m.SoggettoMail;
            //Dati per la mail
            string nomecliente = m.Cliente.Cognome + " " + m.Cliente.Nome;
            if (string.IsNullOrEmpty(nomecliente.Trim())) nomecliente = m.Cliente.Email;
            string Mailcliente = m.Cliente.Email;
            string Descrizione = m.TestoMail + "<br/><br/><br/>";
            string linkUnsubscribe = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/executetasks.aspx?Azione=unsubscribe&idCliente=" + m.Id_cliente + "&Lingua=" + Lingua;
            Descrizione += "<a href=\"" + linkUnsubscribe + "\" target=\"_blank\" style=\"font-size:13px;color:#909090\">" + references.ResMan("Common", Lingua, "TestoUnsubscribe").ToString() + "<br/><br/><br/>";

            Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente);
            m.NoteInvio = "Invio eseguito correttamente.";
            m.DataInvio = System.DateTime.Now;
        }
        catch (Exception err)
        {
            ret = "InviaMailNuovaStrutturaACliente() : Errore invio mail avviso eventi/offerte a cliente, Id Mail: " + m.Id + " " + err.Message + "\r\n";
            //Settiamo la mail come in errore per evitare un nuovo invio della stessa e scriviamo l'errore nella stessa mail nel db
            m.Errore = true;
            m.TestoErrore = ret;
        }
        try
        {
            mDM.InserisciAggiornaMail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, m);
            System.Threading.Thread.Sleep(millisecondbetweenmails);//Attesa tra l'invio delle mail
        }
        catch (Exception error)
        {
            ret += "Errore aggionamento tabella mail dopo invio: " + error.Message;
        }
        return ret;
    }

    /// <summary>
    /// Invia la mail di informazione per inserimento nuova offerta ricettiva
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private string InviaMailNuovaOffertaACliente(Mail m)
    {
        mailingDM mDM = new mailingDM();
        string ret = "";
        try
        {
            string SoggettoMail = m.SoggettoMail;
            //Dati per la mail
            string nomecliente = m.Cliente.Cognome + " " + m.Cliente.Nome;
            string Mailcliente = m.Cliente.Email;
            string Descrizione = m.TestoMail + "<br/><br/>";

            string linkUnsubscribe = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/executetasks.aspx?Azione=unsubscribe&idCliente=" + m.Id_cliente + "&Lingua=" + Lingua;
            Descrizione += "<a href=\"" + linkUnsubscribe + "\" target=\"_blank\" style=\"font-size:13px;color:#909090\">" + references.ResMan("Common", Lingua, "TestoUnsubscribe").ToString() + "<br/><br/><br/>";

            Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente);
            m.NoteInvio = "Invio eseguito correttamente.";
            m.DataInvio = System.DateTime.Now;
        }
        catch (Exception err)
        {
            ret = "InviaMailNuovaOffertaACliente() : Errore invio mail avviso news a cliente, Id Mail: " + m.Id + " " + err.Message + "\r\n";
            //Settiamo la mail come in errore per evitare un nuovo invio della stessa e scriviamo l'errore nella stessa mail nel db
            m.Errore = true;
            m.TestoErrore = ret;
        }
        try
        {
            mDM.InserisciAggiornaMail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, m);
            System.Threading.Thread.Sleep(millisecondbetweenmails);//Attesa tra l'invio delle mail
        }
        catch (Exception error)
        {
            ret += "Errore aggionamento tabella mail dopo invio: " + error.Message;
        }
        return ret;
    }

}
