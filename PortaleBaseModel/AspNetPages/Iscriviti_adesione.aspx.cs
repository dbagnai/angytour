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

public partial class AspNetPages_Iscrivitiadesione : CommonPage
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
    public string Azione
    {
        get { return ViewState["Azione"] != null ? (string)(ViewState["Azione"]) : ""; }
        set { ViewState["Azione"] = value; }
    }
    public string email
    {
        get { return ViewState["email"] != null ? (string)(ViewState["email"]) : ""; }
        set { ViewState["email"] = value; }
    }
    public string ID_offerta
    {
        get { return ViewState["ID_offerta"] != null ? (string)(ViewState["ID_offerta"]) : ""; }
        set { ViewState["ID_offerta"] = value; }
    }
    public string ID_contenuto
    {
        get { return ViewState["ID_contenuto"] != null ? (string)(ViewState["ID_contenuto"]) : ""; }
        set { ViewState["ID_contenuto"] = value; }
    }
    public string ID_cliente
    {
        get { return ViewState["ID_cliente"] != null ? (string)(ViewState["ID_cliente"]) : ""; }
        set { ViewState["ID_cliente"] = value; }
    }
    public string ID_mail
    {
        get { return ViewState["ID_mail"] != null ? (string)(ViewState["ID_mail"]) : ""; }
        set { ViewState["ID_mail"] = value; }
    }
    public string idNewsletter
    {
        get { return ViewState["idNewsletter"] != null ? (string)(ViewState["idNewsletter"]) : ""; }
        set { ViewState["idNewsletter"] = value; }
    }



    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;

                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                Azione = CaricaValoreMaster(Request, Session, "Azione", true, "");
                ID_cliente = CaricaValoreMaster(Request, Session, "ID_cliente", true, "");
                ID_mail = CaricaValoreMaster(Request, Session, "ID_mail", true, "");
                idNewsletter = CaricaValoreMaster(Request, Session, "idNewsletter", true, "");
                email = CaricaValoreMaster(Request, Session, "email", true, "");

                //Carico la galleria in masterpage corretta
                //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);
                //CaricaControlliJS();

                //Titololetto di sezione e presentazione
                RptDescrizione.Text = references.ResMan("Common",Lingua, "TestoIscrivi");

                // CaricaDatiDdlRicerca("it", "", "", "", "", "");
                InzializzaEtichette();

                //Adesione proveniente da mailing newsletter
                if (!string.IsNullOrWhiteSpace(idNewsletter) && !string.IsNullOrWhiteSpace(ID_mail)) //COntrollo se provengo da una richiesta adesione a newslettter!
                {
                    mailingDM mDM = new mailingDM();
                    long id = 0;
                    long.TryParse(ID_mail, out id);
                    Mail mail = mDM.CaricaMailPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
                    long.TryParse(idNewsletter, out id);
                    Mail newsletter = mDM.CaricaNewsletterPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
                    //Titololetto di sezione e presentazione
                    litMainContent.Text = references.ResMan("Common",Lingua, "TitoloIscrivi");
                    if (mail != null)
                    {
                        //Intestazione per il form di adesione all'offerta
                        RptDescrizione.Text = mail.SoggettoMail + "<br/><br/>";
                        RptDescrizione.Text += newsletter.NoteInvio + "<br/>";
                        //Precarico la mail in base al codice cliente
                        ClientiDM cliDM = new ClientiDM();
                        Cliente cli = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ID_cliente);
                        if (cli != null)
                        {
                            //Preriempio il form ( NON LO FACCIO E' L'UTENTE CHE DEVE PROVVEDERE A RIEMPIRE, METTO SOLO NOME E COGNOME )
                            txtNome.Text = cli.Nome;
                            txtCognome.Text = cli.Cognome;
                            //txtEmail.Text = cli.Email;
                            //txtIndirizzo.Text = cli.Indirizzo;
                            //txtTelefono.Text = cli.Telefono;
                            //txtDescrizione.Text = cli.Spare1;
                            //txtPivacf.Text = cli.Pivacf;
                        }
                    }
                    return;
                }
                else
                    Response.Redirect("~/index.aspx?Lingua=" + Lingua);

#if false //Parte commmentata non usata per il form di adesione di ritorno dalla mail

                if (!string.IsNullOrWhiteSpace(ID_cliente)) //COntrollo se è una mail di validazione derivante da click su mail doubleoptin
                {
                    //Verifichiamo se l'id passato è valido -> se sì procediamo con l'atticazione del cliente!!!
                    if (Azione.ToLower() == "valida")
                    {
                        //Cerchiamo cliente e facciamo la validazione per la privacy e l'attivazione della card
                        Validaeattiva(ID_cliente);
                    }
                }
                if (Azione.ToLower() == "iscrivi") //Se provengo da una richiesta di informazioni e il cliente non era iscritto richiedo l'iscrizione
                    if (Session["iscrivicliente"] != null) //COntrollo se è una mail proveniente da una richiesta adesione offerte o struttura
                    {
                        Cliente cli = (Cliente)Session["iscrivicliente"];
                        Session.Remove("iscrivicliente");
                        //Preimposto alcuni dati del form
                        txtEmail.Text = cli.Email;
                        txtTelefono.Text = cli.Telefono;
                        chkPrivacy.Checked = true;
                        chkNewsletter.Checked = true;
                        RptDescrizione.Text = references.ResMan("CommonBase",Lingua,"TestoIscrivi1").ToString();
                    }

                // RiempiFormConDatiTest();//DA COMMENTARE A RUNTIME
                //INSCRIZIONE NEWSLETTOER OFFERTE DA HOMEPAGE
                if (Azione.ToLower() == "newsletter")
                {
                    //i dati base
                    txtEmail.Text = email;
                    chkPrivacy.Checked = true;
                    chkNewsletter.Checked = true;
                    RptDescrizione.Text = "NEWSLETTER"; //references.ResMan("CommonBase",Lingua,"TestoIscrivinewsletter").ToString();
                    //Invio la mail per la validazione dell'email
                    btnInvia_Click(null, null);
                }
                
#endif

            }
            else
            {
                output.Text = "";
                lblRisposta.Text = "";
                lblRisposta.Visible = false;
                plhRisposta.Visible = false;
                plhForm.Visible = true;
            }

         DataBind();
        }
        catch (Exception err)
        {
            output.Text = err.Message;
        }

    }
    public void CaricaControlliJS()
    {
        //Carico la galleria in masterpage corretta
        //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);

        //Carico la galleria in masterpage corretta
        string controllistBanHead = "";
        controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,300);";

        ClientScriptManager cs = Page.ClientScript;
        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {
            cs.RegisterStartupScript(this.GetType(), "controllistBanHead", controllistBanHead, true);
        }

    }
    /// <summary>
    /// Funzione chiamata per ultimare la procedra di double opt in
    /// La pagina chiamata con l'id_cliente prevvede a validare il cliente nel db!!
    /// </summary>
    /// <param name="ID_cliente"></param>
    private void Validaeattiva(string ID_cliente)
    {
        //Cerchiamo il cliente e facciamo la conferma dell'avvenuta registrazione della richiesta
        Cliente tmp_Cliente = new Cliente();
        ClientiDM cmd = new ClientiDM();

        try
        {
            tmp_Cliente = cmd.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ID_cliente);
            if (tmp_Cliente != null)
            {
                if (tmp_Cliente.Validato)
                {
                    output.Text = "Cliente già validato precedentemente. / Customer already valid.";
                    return;
                }

                //////////////////////////////////////////////////////////////////////////////
                //Prendiamo l'ip del client al momento del click nella mail di validazione
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
                tmp_Cliente.IPclient = trueIP; //Setto l'ip corrente
                //Facciamo la validazione della richiesta e aggiungiamo anche la data di richiesta
                tmp_Cliente.Validato = true;
                tmp_Cliente.DataRicezioneValidazione = DateTime.Now;
                //Adesso aggiorno
                cmd.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref tmp_Cliente);

                //Infine nascondo il form di richiesta e visualizzo il messaggio di stato!!!!!
                VisualizzaRisposta(references.ResMan("Common",Lingua, "TestoRispAttivaCliente"));
                //Inviamo la mail relativa all'avvenuta attivazione al cliente e alla società per inviare la mail
                string esitoinvio = InviaMailValidatoCliente(tmp_Cliente);
                if (string.IsNullOrWhiteSpace(esitoinvio))
                {
                    VisualizzaRisposta(references.ResMan("Common",Lingua,"TestoIscrizioneCorretta").ToString());

                    //IN caso di validazione positiva se ID_contenuto o id_offerta non vuoti -> posso reindirizzare al form di 
                    //richiesta dell'offerta!!
                    Offerte offerta = new Offerte();
                    if (!string.IsNullOrWhiteSpace(ID_offerta)) //Se presente l'id della struttura ( offerta ) imposto il corretto destinatario
                        offerta = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ID_offerta);
                    Contenuti contenuto = new Contenuti();
                    if (!string.IsNullOrWhiteSpace(ID_contenuto)) //Se presente l'idcontenuto ( promozione ) prendo il testo relativo
                        contenuto = conDM.CaricaContenutiPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ID_contenuto);
                    if (!string.IsNullOrEmpty(ID_offerta) || !string.IsNullOrEmpty(ID_contenuto))
                    {
                        if (!string.IsNullOrEmpty(ID_contenuto))
                        {
                            //string linkcontenuto = "window.open(\"Content_Tipo3_Nomaster.aspx?idOfferta=" + ID_offerta + "&idContenuto=" + ID_contenuto + "&TipoContenuto=Prenotazioni&CodiceTipologia=" + offerta.CodiceTipologia + "&Lingua=" + Lingua + "\",\"\", \"screenX=10,left=10,screenY=10,top=10,scrollbars=yes,toolbar=yes,location=no,status=no,width=600,height=430,_blank\")";
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ojs", linkcontenuto, true);

                            string linkjava1 = "window.open(\"Content_Tipo3_Nomaster.aspx?id=" + ID_offerta + "&idContenuto=" + ID_contenuto + "&TipoContenuto=Prenotazioni&CodiceTipologia=" + contenuto.CodiceContenuto + "&Lingua=" + Lingua + "\", \"_blank\")";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ojs", linkjava1, true);

                        }
                        else
                        {
                            //string linkofferta = "window.open(\"Content_Tipo3_Nomaster.aspx?idOfferta=" + ID_offerta + "&TipoContenuto=Prenotazioni&CodiceTipologia=" + offerta.CodiceTipologia + "&Lingua=" + Lingua + "\",\"\", \"screenX=10,left=10,screenY=10,top=10,scrollbars=yes,toolbar=yes,location=no,status=no,width=600,height=430,_blank\")";
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ojs", linkofferta, true);

                            string linkjava2 = "window.open(\"Content_Tipo3_Nomaster.aspx?id=" + ID_offerta + "&TipoContenuto=Prenotazioni&CodiceTipologia=" + offerta.CodiceTipologia + "&Lingua=" + Lingua + "\", \"_blank\")";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ojs", linkjava2, true);

                        }
                    }

                }
                else
                {
                    output.Text = esitoinvio;
                    return;
                }
            }
            else
            {
                output.Text = "Errore cliente non identificato. / Error customer not found.";
            }
        }
        catch (Exception err)
        {
            output.Text = err.Message + " <br/> ";

            VisualizzaRisposta(references.ResMan("Common",Lingua, "txtValidazioneError"));
        }

    }
    //private void InizializzaMeta()
    //{
    //    string[] testo = MetaContentCaller(Lingua, TipoContenuto);
    //    Page.Title = testo[0];
    //    ((HtmlMeta)Master.FindControl("metaDesc")).Content = testo[1];
    //    ((HtmlMeta)Master.FindControl("metaKeyw")).Content = testo[2];
    //    ((HtmlMeta)Master.FindControl("metaAbstract")).Content = testo[3];
    //}
    private void InzializzaEtichette()
    {
        //chkPrivacy.Text = references.ResMan("Common",Lingua, "testoPrivacy");
        Summary.HeaderText = references.ResMan("Common",Lingua, "testoDatiMancanti");
        //btnInvia.Text = references.ResMan("Common",Lingua, "TestoBtnNewsletter");
    }
    protected void RiempiFormConDatiTest()
    {
        //txtCodice.Text = "b29zt38co50im67fp41x";//card buona
        txtNome.Text = "Pinco";
        txtCognome.Text = "Pallino";
        txtEmail.Text = "dbagnai@gmail.com";
        // ddlNazione
        //ddlRegione.SelectedValue = "p82"; ddlRegione_SelectedIndexChanged(null, null);
        //ddlProvincia.SelectedValue = "p91"; ddlProvincia_SelectedIndexChanged(null, null);
        //ddlComune.SelectedValue = "Sarteano";
        //txtCap.Text = "06062";
        txtIndirizzo.Text = "Str.tale dei tali";
        txtTelefono.Text = "63636363";
        //txtProfessione.Text = "programmatore";
        //txtNascita.Text = "27/09/1984";
        txtDescrizione.Text = "Note al contatto";
        chkPrivacy.Checked = true;
        chkNewsletter.Checked = true;
    }

    protected Cliente CaricaDatiClienteDaForm()
    {
        Cliente item = new Cliente();

        item.CodiceCard = "";
        item.Nome = txtNome.Text;
        item.Cognome = txtCognome.Text;
        item.Email = txtEmail.Text;
        //item.CodiceNAZIONE = ddlNazione.SelectedValue; // ddlNazione
        //item.CodiceREGIONE = ddlRegione.SelectedValue;
        //item.CodicePROVINCIA = ddlProvincia.SelectedValue;
        //item.CodiceCOMUNE = ddlComune.SelectedValue;
        //item.Cap = txtCap.Text;
        item.Indirizzo = txtIndirizzo.Text;
        item.Telefono = txtTelefono.Text;
        //item.Professione = txtProfessione.Text;
 
        item.ConsensoPrivacy = chkPrivacy.Checked;
        item.Consenso1 = chkNewsletter.Checked;
        item.Spare1 = txtDescrizione.Text;
        //item.Pivacf = txtPivacf.Text;
        item.Lingua = Lingua;
        item.DataInvioValidazione = System.DateTime.Now;
        item.IPclient = "";

        return item;
    }

    protected void btnInviaAStrutturaSenzaValidazione_Click(object sender, EventArgs e)
    {
        try
        {
            Cliente cliente = CaricaDatiClienteDaForm(); //Leggo i dati del cliente dal form
            if (!chkPrivacy.Checked)
            {
                output.Text = references.ResMan("Common",Lingua, "txtPrivacyError");
            }
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
            cliente.IPclient = trueIP;

            mailingDM mDM = new mailingDM();
            long id = 0;
            long.TryParse(ID_mail, out id);
            //Controlliamo che la newsletter per il cliente non sia già completa di adesione
            Mail mailacliente = mDM.CaricaMailPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
            if (mailacliente == null)
            {
                output.Text = references.ResMan("Common",Lingua,"erroreAdesioneMail").ToString();
                return;
            }
            if (mailacliente != null && mailacliente.DataAdesione != null)
            {
                output.Text = references.ResMan("Common",Lingua,"erroreAdesione").ToString();
                return;
            }
            //Carico anche la newsletter
            id = 0;
            long.TryParse(idNewsletter, out id);
            Mail newsletter = mDM.CaricaNewsletterPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
            string intestazioneform = "";
            if (newsletter != null)
                intestazioneform = newsletter.NoteInvio;

            //Devo salvarmi/aggiornarmi i dati del cliente 
            ClientiDM cliDM = new ClientiDM();
            //RIEMPIAMO IL TESTO DEL FORM DALLA PAGINE E CON I CONSENSI SPUNTATI!!!
            cliente.TestoFormConsensi = references.ResMan("Common",Lingua,"testoAdesione").ToString() + "<br/>";
            if (mailacliente != null)
            {
                cliente.TestoFormConsensi += "Subject/Oggetto: " + mailacliente.SoggettoMail + "<br/>";
                cliente.TestoFormConsensi += intestazioneform + "<br/>";
            }
            cliente.TestoFormConsensi += references.ResMan("Common",Lingua,"testoPrivacy").ToString() + "<br/>";
            cliente.TestoFormConsensi += references.ResMan("Common",Lingua,"testoConsenso1").ToString();
            cliente.TestoFormConsensi += " Date: " + cliente.DataInvioValidazione.ToString() + " Authorization for IP client : " + cliente.IPclient + " <br/>";

            //Proviamo a caricare il cliente per email, per verificare se già presente in anagrafica e corrispondente all'id cliente passato nella querystring:
            Cliente clidb = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cliente.Email);
            //Prendo l'id_cliente passato nella chiamata
            long idcliente = 0;
            long.TryParse(ID_cliente, out idcliente);
            if (clidb != null && clidb.Id_cliente != 0 && clidb.Validato && clidb.Id_cliente == idcliente)//Cliente presente in anagrafica, già validato e con mail coincidente con l'id passato!!!!
            {
                clidb.Spare1 = cliente.Spare1;
                if (!string.IsNullOrWhiteSpace(cliente.Nome))
                    clidb.Nome = cliente.Nome;
                if (!string.IsNullOrWhiteSpace(cliente.Cognome))
                    clidb.Cognome = cliente.Cognome;
                if (!string.IsNullOrWhiteSpace(cliente.Indirizzo))
                    clidb.Indirizzo = cliente.Indirizzo;
                if (!string.IsNullOrWhiteSpace(cliente.Telefono))
                    clidb.Telefono = cliente.Telefono;
                clidb.TestoFormConsensi = cliente.TestoFormConsensi;

                cliente = clidb;//Prendo i valori dal cliente memorizzato nel db e non quello inserito salvo le note!!
            }
            else
            {
                //Cliente non presente in anagrafica clienti -> non faccio nulla in quanto per la newsletter i clienti devono essere precedentemente REGISTRATI
                //e validati altrimenti non posso convalidare l'adesione!!!!!
                output.Text = references.ResMan("Common",Lingua,"erroreValidazione").ToString();
                return;
            }
            //Tutto ok procedimamo con insert/update del cliente
            //cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cliente);

            //Inviamo la mail di adesione alla struttura e al cliente per il termine della procedura di adesione
            string esitoinvio = InviaMailValidatoCliente(cliente);
            if (string.IsNullOrWhiteSpace(esitoinvio))
            {
                VisualizzaRisposta(references.ResMan("Common",Lingua,"testoRispostaMailAdesione").ToString());
                //Aggiorno l'adesione alla mail specifica della newsletter per evitare risposte multiple
                mailacliente.DataAdesione = System.DateTime.Now;
                mDM.InserisciAggiornaMail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, mailacliente);
            }
            else
            {
                output.Text = esitoinvio;
                return;
            }
        }
        catch (Exception err)
        {
            output.Text = err.Message + " <br/> ";

            VisualizzaRisposta(references.ResMan("Common", Lingua, "txtIscrizioneErrore"));
        }
    }
    /// <summary>
    /// Invia la mail di avvenuta adesione alla newsletter al cliente e alla segreteria e al cliente
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    private string InviaMailValidatoCliente(Cliente cliente)
    {
        string ret = "";
        try
        {
            //Dati per la mail
            string nomecliente = cliente.Cognome + " " + cliente.Nome;
            string Mailcliente = cliente.Email;
            string Descrizione = references.ResMan("Common",Lingua,"testoMailAdesione1").ToString() + "<br/>";

            Descrizione += references.ResMan("Common",Lingua,"FormTesto2").ToString() + " " + cliente.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto3").ToString() + " " + cliente.Cognome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto4").ToString() + " " + cliente.Email + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            //Descrizione += references.ResMan("CommonBase",Lingua,"FormTesto5").ToString() + " " + cliente.CodiceNAZIONE + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto20").ToString() + " " + cliente.Pivacf + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            //Province p = WelcomeLibrary.UF.Utility.ElencoProvince.Find(delegate(Province _p) { return _p.Codice == cliente.CodiceREGIONE; });
            //if (p != null)
            //    Descrizione += references.ResMan("CommonBase",Lingua,"FormTesto6").ToString() + " " + p.Regione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            //p = WelcomeLibrary.UF.Utility.ElencoProvince.Find(delegate(Province _p) { return _p.Codice == cliente.CodicePROVINCIA; });
            //if (p != null)
            //    Descrizione += references.ResMan("CommonBase",Lingua,"FormTesto7").ToString() + " " + p.Provincia + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            //Comune c = WelcomeLibrary.UF.Utility.ElencoComuni.Find(delegate(Comune _p) { return _p.Nome.ToLower() == cliente.CodiceCOMUNE.ToLower(); });
            //if (c != null)
            //    Descrizione += references.ResMan("CommonBase",Lingua,"FormTesto8").ToString() + " " + c.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            //Descrizione += references.ResMan("CommonBase",Lingua,"FormTesto9").ToString() + " " + cliente.Cap + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto10").ToString() + " " + cliente.Indirizzo + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto11").ToString() + " " + cliente.Telefono + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            //Descrizione += references.ResMan("CommonBase",Lingua,"FormTesto12").ToString() + " " + cliente.Professione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            //Descrizione += references.ResMan("CommonBase",Lingua,"FormTesto13").ToString() + " " + cliente.DataNascita.ToShortDateString() + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto14").ToString() + " " + cliente.Spare1 + "<br/><br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            Descrizione += cliente.TestoFormConsensi + "<br/><br/>";

            //Invio della mail alla struttura emittente la newsletter -----------------------------------------------------------
            string SoggettoMailPerStruttura = "Mail di adesione a newsletter " + Nome + " dal cliente " + nomecliente;
            Utility.invioMailGenerico(Nome, Email, SoggettoMailPerStruttura, Descrizione, Email, Nome);
            //Utility.invioMailGenerico(nomecliente, Mailcliente, SoggettoMailPerStruttura, Descrizione, Email, Nome);

            //Invio della mail di conferma dell'adesione al cliente -----------------------------------------------------------
            string SoggettoMail = Nome + ", mail di conferma avvenuta adesione  / Confirmation e-mail for application";
            Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente);

        }
        catch
        {
            ret = "Errore invio mail di conferma.Contattare l'assistenza./Error sending confirmation mail contact us directly.";

        }
        return ret;
    }

    private void VisualizzaRisposta(string p)
    {
        plhRisposta.Visible = true;
        plhForm.Visible = false;
        lblRisposta.Text = p;

        lblRisposta.Visible = true;
    }

  
}
