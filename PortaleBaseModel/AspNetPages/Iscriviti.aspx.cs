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
using System.Web.Profile;

public partial class _Iscriviti : CommonPage
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
    public string SubAct
    {
        get { return ViewState["SubAct"] != null ? (string)(ViewState["SubAct"]) : ""; }
        set { ViewState["SubAct"] = value; }
    }
    public string ID_cliente
    {
        get { return ViewState["ID_cliente"] != null ? (string)(ViewState["ID_cliente"]) : ""; }
        set { ViewState["ID_cliente"] = value; }
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
    public string originalsid
    {
        get { return ViewState["originalsid"] != null ? (string)(ViewState["originalsid"]) : ""; }
        set { ViewState["originalsid"] = value; }
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
                Azione = CaricaValoreMaster(Request, Session, "Azione");
                SubAct = CaricaValoreMaster(Request, Session, "SubAct");

                //Carico il session id originale della richiesta di iscrizione per ricaricare il carrello
                originalsid = CaricaValoreMaster(Request, Session, "sid");

                ID_cliente = CaricaValoreMaster(Request, Session, "ID_cliente");
                //Lo uso per mandare la mail relativa all'offerta richiesta al termine della validazione
                ID_offerta = CaricaValoreMaster(Request, Session, "ID_offerta");
                //Lo uso per mandare la mail relativa alla struttura richiesta al termine della validazione
                ID_contenuto = CaricaValoreMaster(Request, Session, "ID_contenuto");

              //  CaricaControlliJS();

                CaricaDatiDdlRicerca("it", "", "", "", "", "");
                InzializzaEtichette();
                if (!string.IsNullOrWhiteSpace(ID_cliente)) //COntrollo se è una mail di validazione derivante da click su mail doubleoptin
                {
                    //Verifichiamo se l'id passato è valido -> se sì procediamo con l'atticazione del cliente!!!
                    if (Azione.ToLower() == "valida")
                    {
                        //((Literal)Master.FindControl("sectionName")).Text = references.ResMan("Common",Lingua,"TitoloIscrivi").ToString();
                        //Cerchiamo cliente e facciamo la validazione per la privacy e l'attivazione della card
                        Validaeattiva(ID_cliente);
                    }
                }
                if (Azione.ToLower() == "iscrivi") //Se provengo da una richiesta di informazioni e il cliente non era iscritto richiedo l'iscrizione
                    if (Session["iscrivicliente"] != null) //COntrollo se è una mail proveniente da una richiesta adesione offerte o struttura
                    {
                        ((Literal)Master.FindControl("sectionName")).Text = references.ResMan("Common",Lingua,"TitoloIscrivi").ToString();
                        Cliente cli = (Cliente)Session["iscrivicliente"];
                        Session.Remove("iscrivicliente");
                        //Preimposto alcuni dati del form
                        txtEmail.Text = cli.Email;
                        txtTelefono.Text = cli.Telefono;
                        chkPrivacy.Checked = true;
                        chkConsensoMail.Checked = true;
                        litDescrizioneIscrivi.Text = references.ResMan("Common",Lingua,"TestoIscrivi1").ToString();
                    }
                if (Azione.ToLower() == "iscrivinewsletter")
                    if (Session["iscrivicliente"] != null)
                    {
                        // ((Literal)Master.FindControl("sectionName")).Text = references.ResMan("Common",Lingua,"TitoloIscrivi").ToString();

                        Cliente cli = (Cliente)Session["iscrivicliente"];
                        Session.Remove("iscrivicliente");
                        //Preimposto alcuni dati del form
                        txtEmail.Text = cli.Email;
                        //  txtTelefono.Text = cli.Telefono;
                        chkPrivacy.Checked = true;
                        chkConsensoMail.Checked = true;
                        cli.Lingua = Lingua;
                        RichiestaIscrizioneNewsletter(cli);
                    }
                //RiempiFormConDatiTest();//DA COMMENTARE A RUNTIME
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

    protected void RichiestaIscrizioneNewsletter(Cliente cli)
    {
        //Testo se l'email del cliente è presente in anagrafica e validata per l'iscriziokne
        //Ne qual caso invio la mail di richiesta direttamente al portale
        ClientiDM cliDM = new ClientiDM();
        Cliente dbCliente = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email);
        string linkvalidazione = "";
        string ret = "";
        if (dbCliente != null && dbCliente.Id_cliente != 0) //Cliente esistente
        {
            //Cliente presente 
            if (dbCliente.Validato)
            {
                //-> verifico se cliente validato e mando la mail di avviso richiesta iscrizione al portale
                //dbCliente.DataNascita = cli.DataNascita; //Prendo la data ultima passata
                ret = InviaMailrichiestaIscrizione(dbCliente);
                VisualizzaRisposta(ret);
            }
            else
            {
                string testo = references.ResMan("Common",Lingua,"testoIscrizioneNewsletter").ToString();
                testo += references.ResMan("Common",Lingua,"TestoIscrivi1").ToString();
                litDescrizioneIscrivi.Text = testo;

                //se non validato ? richiedo validazione ???
                linkvalidazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti.aspx?ID_cliente=" + dbCliente.Id_cliente + "&Azione=Valida&SubAct=1&Lingua=" + Lingua;
                // VisualizzaRisposta("Cliente presente non validato --- finire integrazione ");
                //Response.Redirect(linkvalidazione);
                string esitoinvio = InviaMailPerValidazioneaCliente(dbCliente, linkvalidazione);
                if (string.IsNullOrWhiteSpace(esitoinvio))
                {
                    ret = references.ResMan("Common",Lingua,"testoRispostaMailIscrizione").ToString();
                }
                else
                {
                    ret = esitoinvio;
                }
                VisualizzaRisposta(ret);

            }
        }
        else //Cliente non esistente
        {
            string testo = references.ResMan("Common",Lingua,"testoIscrizioneNewsletter").ToString();
            testo += references.ResMan("Common",Lingua,"TestoIscrivi1").ToString();
            litDescrizioneIscrivi.Text = testo;

            ////Cliente non presente -> inserirlo nel db e poi attivare il mailing per la validazione col cliente
            cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);
            //se non validato ? richiedo validazione ???
            linkvalidazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti.aspx?ID_cliente=" + cli.Id_cliente + "&Azione=Valida&SubAct=1&Lingua=" + Lingua;
            // VisualizzaRisposta("Cliente presente non validato --- finire integrazione ");
            //Response.Redirect(linkvalidazione);
            string esitoinvio = InviaMailPerValidazioneaCliente(cli, linkvalidazione);
            if (string.IsNullOrWhiteSpace(esitoinvio))
                ret = references.ResMan("Common",Lingua,"testoRispostaMailIscrizione").ToString();
            else
            {
                ret = esitoinvio;
            }
            VisualizzaRisposta(ret);

        }
    }
    private string InviaMailrichiestaIscrizione(Cliente cliente)
    {
        //Mandiamo la mail al portale per la richiesta iscrizione newsletter
        string ret = "";
        try
        {
            //Dati per la mail
            string nomecliente = cliente.Cognome + " " + cliente.Nome;
            string Mailcliente = cliente.Email;
            //testoMailOroscopo
            string Descrizione = references.ResMan("Common",Lingua,"testoMailIscrizione").ToString() + "<br/>";
            Descrizione += references.ResMan("Common",Lingua,"FormTesto2").ToString() + " " + cliente.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto3").ToString() + " " + cliente.Cognome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto4").ToString() + " " + cliente.Email + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto5").ToString() + " " + cliente.CodiceNAZIONE + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Province p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate (Province _p) { return _p.Codice == cliente.CodiceREGIONE; });
            if (p != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto6").ToString() + " " + p.Regione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate (Province _p) { return _p.Codice == cliente.CodicePROVINCIA; });
            if (p != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto7").ToString() + " " + p.Provincia + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Comune c = WelcomeLibrary.UF.Utility.ElencoComuni.Find(delegate (Comune _p) { return _p.Nome.ToLower() == cliente.CodiceCOMUNE.ToLower(); });
            if (c != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto8").ToString() + " " + c.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto9").ToString() + " " + cliente.Cap + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto10").ToString() + " " + cliente.Indirizzo + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto11").ToString() + " " + cliente.Telefono + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto12").ToString() + " " + cliente.Professione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto13").ToString() + " " + cliente.DataNascita.ToShortDateString() + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto14").ToString() + " " + cliente.Spare1 + "<br/><br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += cliente.TestoFormConsensi + "<br/>";

            if (chkConsensoMail.Checked == true)
                Descrizione += " <br/> Il cliente ha richiesto l'adesione all'invio newsletter : " + references.ResMan("Common", Lingua, "titolonewsletter1").ToString() + "<br/>";


            string SoggettoMail = references.ResMan("Common",Lingua,"testoMailIscrizione").ToString();
            Utility.invioMailGenerico(nomecliente, Mailcliente, SoggettoMail, Descrizione, Email, Nome);
            ret = references.ResMan("Common",Lingua,"testoMailIscrizioneOK").ToString();

        }
        catch (Exception err)
        {
            ret += references.ResMan("Common",Lingua,"testoMailIscrizionenoOK").ToString();
        }
        return ret;

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
            string ret = "";
            ret = references.ResMan("Common",Lingua,"TitoloIscrivi").ToString();

            tmp_Cliente = cmd.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ID_cliente);
            if (tmp_Cliente != null)
            {
                if (tmp_Cliente.Validato)
                {
                    output.Text = "Cliente già validato precedentemente. / Customer already valid.";
                    VisualizzaRisposta(output.Text);
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
                tmp_Cliente.Consenso1 = true;//Setto anche il consenso commerciale
                tmp_Cliente.DataRicezioneValidazione = DateTime.Now;
                tmp_Cliente.Lingua = Lingua;
                //Adesso aggiorno
                cmd.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref tmp_Cliente);

                //Se tutto ok ->
                //Creo l'utente nel database di autenticazione utenti
                // Username = emailcliente -> Password generata
                string username = "";
                string password = "";
                //CreaUtenteAssociato(tmp_Cliente, ref username, ref password);

                //Ricarichiamo il carrello originari se presente il sid
                RicaricaCarrello();

                //Infine nascondo il form di richiesta e visualizzo il messaggio di stato!!!!!
                ret = references.ResMan("Common",Lingua, "TestoRispAttivaCliente");
                //Inviamo la mail relativa all'avvenuta attivazione al cliente e alla società per inviare la mail
                string esitoinvio = InviaMailValidatoCliente(tmp_Cliente, password);
                if (string.IsNullOrWhiteSpace(esitoinvio))
                {
                    ret += "<br/>" + references.ResMan("Common",Lingua,"TestoIscrizioneCorretta").ToString();
                    switch (SubAct) //Eseguo l'attività indicata dal subact
                    {
                        case "1": //SubAct per invio richiesta  al portale
                            // ret += "<br/>" + InviaMailrichiestaIscrizione(tmp_Cliente);
                            break;
                        default:
                            break;
                    }

                    #region GESTIONE REDIRECT PER RICHIESTE OFFERTE E CONTENUTI
#if false
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
                            //string linkcontenuto = "window.open(\"Content_Tipo3_Nomaster.aspx?idOfferta=" + ID_offerta + "&idContenuto=" + ID_contenuto + "&TipoContenuto=Prenotazioni&CodiceTipologia=" + offerta.CodiceTipologia + "&Lingua=" + Lingua + "\",\"\", \"screenX=10,left=10,screenY=10,top=10,scrollbars=auto,toolbar=yes,location=no,status=no,width=600,height=430,_blank\")";
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ojs", linkcontenuto, true);

                            string linkjava1 = "window.open(\"Content_Tipo3_Nomaster.aspx?idOfferta=" + ID_offerta + "&idContenuto=" + ID_contenuto + "&TipoContenuto=Prenotazioni&CodiceTipologia=" + offerta.CodiceTipologia + "&Lingua=" + Lingua + "\", \"_blank\")";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ojs", linkjava1, true);

                        }
                        else
                        {
                            //string linkofferta = "window.open(\"Content_Tipo3_Nomaster.aspx?idOfferta=" + ID_offerta + "&TipoContenuto=Prenotazioni&CodiceTipologia=" + offerta.CodiceTipologia + "&Lingua=" + Lingua + "\",\"\", \"screenX=10,left=10,screenY=10,top=10,scrollbars=auto,toolbar=yes,location=no,status=no,width=600,height=430,_blank\")";
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ojs", linkofferta, true);

                            string linkjava2 = "window.open(\"Content_Tipo3_Nomaster.aspx?idOfferta=" + ID_offerta + "&TipoContenuto=Prenotazioni&CodiceTipologia=" + offerta.CodiceTipologia + "&Lingua=" + Lingua + "\", \"_blank\")";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ojs", linkjava2, true);

                        }
                    }

#endif
                    #endregion
                }
                else
                {
                    ret += "<br/>" + esitoinvio;
                    VisualizzaRisposta(ret);
                    return;
                }
            }
            else
            {
                ret += "<br/>" + "Errore cliente non identificato. / Error customer not found.";
            }

            VisualizzaRisposta(ret);
        }
        catch (Exception err)
        {
            output.Text = err.Message + " <br/> ";
            VisualizzaRisposta(references.ResMan("Common",Lingua,"txtValidazioneError"));

        }

    }

    private void RicaricaCarrello()
    {
        if (originalsid != "")
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
            CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, originalsid, trueIP);
            string idlisttomodify = "";
            foreach (Carrello c in carrello)
            {
                idlisttomodify += c.ID.ToString() + ",";
            }
            if (idlisttomodify.Length > 1)
            {
                idlisttomodify = idlisttomodify.Substring(0, idlisttomodify.Length - 1);
                idlisttomodify = idlisttomodify.Insert(0, "( ");
                idlisttomodify += " )";
            }
            if (!string.IsNullOrEmpty(idlisttomodify))
                ecmDM.UpdateCarrelloSessionidPerListaID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Session.SessionID, idlisttomodify);
        }
    }

    /// <summary>
    /// Funzione che crea l'utente (usename e password) per gestire il condominio
    /// </summary>
    /// <param name="ID"></param>
    protected bool CreaUtenteAssociato(Cliente cliente, ref string NomeUtente, ref string password)
    {
        bool esito = true;
        try
        {
            if (cliente != null && !string.IsNullOrWhiteSpace(cliente.Email) && cliente.Id_cliente != 0)
            {
                //Generiamo la password di accesso
                //password = Membership.GeneratePassword(6, 0);
                password = WelcomeLibrary.UF.RandomPassword.Generate(8);

                //Creiamo l'utente
                NomeUtente = cliente.Email;
                Membership.CreateUser(NomeUtente, password);


                //associamo l'utente al ruolo
                Roles.AddUserToRole(NomeUtente, "Operatore");

                //ProfileCommon prof = (ProfileCommon)ProfileCommon.Create(NomeUtente);
                    ProfileBase prof = ProfileBase.Create(NomeUtente);
                prof["IdCliente"] = cliente.Id_cliente.ToString();
                prof.Save();

                ////Stampo a video i dati dell'username e password Appena Creati
                //Username.Visible = true;
                //Username.Text = NomeUtente;
                //Password.Visible = true;
                //Password.Text = password;
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
        chkPrivacy.Text = references.ResMan("Common",Lingua, "testoPrivacy");
        Summary.HeaderText = references.ResMan("Common",Lingua, "testoDatiMancanti");
        btnInvia.Text = references.ResMan("Common",Lingua, "TestoBtnNewsletter");
    }
    protected void RiempiFormConDatiTest()
    {
        //txtCodice.Text = "b29zt38co50im67fp41x";//card buona
        txtNome.Text = "Pinco";
        txtCognome.Text = "Pallino";
        txtEmail.Text = "dbagnai@gmail.com";
        // ddlNazione
        ddlRegione.SelectedValue = "p82"; ddlRegione_SelectedIndexChanged(null, null);
        ddlProvincia.SelectedValue = "p91"; ddlProvincia_SelectedIndexChanged(null, null);
        ddlComune.SelectedValue = "Sarteano";
        txtCap.Text = "06062";
        txtIndirizzo.Text = "Str.tale dei tali";
        txtTelefono.Text = "63636363";
        txtProfessione.Text = "programmatore";
        txtNascita.Text = "27/09/1984";
        txtDescrizione.Text = "Note al contatto di attivazione";
        chkPrivacy.Checked = true;
        chkConsensoMail.Checked = true;
    }
    protected Cliente CaricaDatiClienteDaForm()
    {
        Cliente item = new Cliente();

        item.CodiceCard = "";
        item.Nome = txtNome.Text;
        item.Cognome = txtCognome.Text;
        item.Email = txtEmail.Text;
        item.CodiceNAZIONE = ddlNazione.SelectedValue; // ddlNazione
        item.CodiceREGIONE = ddlRegione.SelectedValue;
        item.CodicePROVINCIA = ddlProvincia.SelectedValue;
        item.CodiceCOMUNE = ddlComune.SelectedValue;
        item.Cap = txtCap.Text;
        item.Indirizzo = txtIndirizzo.Text;
        item.Telefono = txtTelefono.Text;
        item.Professione = txtProfessione.Text;
        DateTime tmp = DateTime.MinValue;
        DateTime.TryParse(txtNascita.Text, out tmp);
        item.DataNascita = tmp;
        item.ConsensoPrivacy = chkPrivacy.Checked;
        item.Consenso1 = chkConsensoMail.Checked;
        item.Spare1 = txtDescrizione.Text;
        item.Pivacf = txtPiva.Text;
        item.Lingua = Lingua;
        item.DataInvioValidazione = System.DateTime.Now;
        item.IPclient = "";

        return item;

    }
    /// <summary>
    /// Invia al cliente la mail contente il link di validazione dei dati inseriti
    /// al momento della validazione del cliente questo viene impostato nel db
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnInvia_Click(object sender, EventArgs e)
    {
        try
        {
            Cliente cliente = CaricaDatiClienteDaForm(); //Leggo i dati del cliente dal form
#if true
            //Testo se l'email del cliente è presente in anagrafica e validata per l'iscriziokne
            //Ne qual caso invio la mail di richiesta direttamente al portale
            ClientiDM cliDM = new ClientiDM();
            Cliente dbCliente = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cliente.Email);
            if (dbCliente != null && dbCliente.Id_cliente != 0) //Cliente esistente
            {
                output.Text = references.ResMan("Common",Lingua, "txtValidateVerifica");
                
            }
#endif

            if (!chkPrivacy.Checked)
            {
               output.Text = references.ResMan("Common", Lingua, "txtPrivacyError");
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

            //Devo salvarmi i dati del cliente come primo accesso, lasciando vuoti i campi di Data ricezione Validazione e Codice Card


            //RIEMPIAMO IL TESTO DEL FORM DALLA PAGINE E CON I CONSENSI SPUNTATI!!!
            cliente.TestoFormConsensi = "Autorizzazione da client ip address : " + cliente.IPclient + " in data " + cliente.DataInvioValidazione.ToString() + "<br/>";
            cliente.TestoFormConsensi += references.ResMan("Common",Lingua,"TestoIscrivi").ToString() + "\r\n";
            cliente.TestoFormConsensi += references.ResMan("Common",Lingua,"testoPrivacy").ToString() + "\r\n";
            cliente.TestoFormConsensi += references.ResMan("Common",Lingua,"testoConsenso1").ToString();
            //Proviamo a caricare il cleinte per email, per verificare se già presente in anagrafica:
            Cliente clidb = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cliente.Email);
            if (clidb != null && clidb.Id_cliente != 0)//Cliente presente in anagrafica
            {
                //Sovrascrivo i dati letti dal db con quelli specificati nel form( considerandoli maggiormente aggiornati )
                cliente.Id_cliente = clidb.Id_cliente;
            }
            //Tutto ok procedimamo con insert/update del cliente
            cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cliente);
            //Tiro su l'id_cliente in caso di inserimento
            //if (cliente.Id_cliente == 0)
            //    cliente = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cliente.Email);

            //Prepariamo e inviamo il mail al cliente e per la validazione dei dati e il completamento della procedura di attivazione
            //mettendo il link di validazione alla pagina con ID_CLIENTE  e Azione=valida -> ( inoltre persisto l'eventuale id_offerta o ID_contenuto per la richiesta finale dopo la validazione
            string linkvalidazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti.aspx?ID_cliente=" + cliente.Id_cliente + "&Azione=Valida&ID_offerta=" + ID_offerta + "&ID_contenuto=" + ID_contenuto + "&sid=" + Session.SessionID + "&Lingua=" + Lingua;

            string esitoinvio = InviaMailPerValidazioneaCliente(cliente, linkvalidazione);
            if (string.IsNullOrWhiteSpace(esitoinvio))
                VisualizzaRisposta(references.ResMan("Common",Lingua,"testoRispostaMailIscrizione").ToString());
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
    /// Invia la mail al cliente per la validazione dell'iscrizione al servizio 
    /// </summary>
    /// <param name="cliente"></param>
    /// <param name="linkvalidazione"></param>
    /// <returns></returns>
    private string InviaMailPerValidazioneaCliente(Cliente cliente, string linkvalidazione)
    {
        string ret = "";
        try
        {
            string SoggettoMail = Nome + " : " + references.ResMan("Common",Lingua,"testoMailIscrizione0").ToString();

            //Dati per la mail
            string nomecliente = cliente.Cognome + " " + cliente.Nome;
            string Mailcliente = cliente.Email;

            string Descrizione = Nome + " : " + references.ResMan("Common",Lingua,"testoMailIscrizione1").ToString() + "<br/>";
            Descrizione += references.ResMan("Common",Lingua,"TestoIscrivi").ToString() + "<br/><br/>";

            //Descrizione += references.ResMan("Common",Lingua,"FormTesto1").ToString() + " " + cliente.CodiceCard + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto2").ToString() + " " + cliente.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto3").ToString() + " " + cliente.Cognome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto4").ToString() + " " + cliente.Email + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto5").ToString() + " " + cliente.CodiceNAZIONE + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            Province p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate (Province _p) { return _p.Codice == cliente.CodiceREGIONE; });
            if (p != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto6").ToString() + " " + p.Regione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate (Province _p) { return _p.Codice == cliente.CodicePROVINCIA; });
            if (p != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto7").ToString() + " " + p.Provincia + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Comune c = WelcomeLibrary.UF.Utility.ElencoComuni.Find(delegate (Comune _p) { return _p.Nome.ToLower() == cliente.CodiceCOMUNE.ToLower(); });
            if (c != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto8").ToString() + " " + c.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto9").ToString() + " " + cliente.Cap + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto10").ToString() + " " + cliente.Indirizzo + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto11").ToString() + " " + cliente.Telefono + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto12").ToString() + " " + cliente.Professione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto13").ToString() + " " + cliente.DataNascita.ToShortDateString() + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto14").ToString() + " " + cliente.Spare1 + "<br/><br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            Descrizione += cliente.TestoFormConsensi + "<br/>";
            Descrizione += "<a href=\"" + linkvalidazione + "\" target=\"_blank\" style=\"font-size:22px;color:#b13c4e\">" + references.ResMan("Common",Lingua,"testoLinkValidazioneIscrizione").ToString() + "<br/>";
            Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente);
        }
        catch (Exception err)
        {
            ret = "Errore invio mail di richiesta.Contattare l'assistenza./Erros sending request mail contact us directly.";

        }
        return ret;
    }
    /// <summary>
    /// Invia la mail di avvenuta registrazione al cliente e alla segreteria  
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    private string InviaMailValidatoCliente(Cliente cliente, string password)
    {
        string ret = "";
        try
        {
            string SoggettoMail = Nome + " : " + "Mail di conferma avvenuta iscrizione portale  / Confirmation e-mail for application to  portal.";

            //Dati per la mail
            string nomecliente = cliente.Cognome + " " + cliente.Nome;
            string Mailcliente = cliente.Email;
            string Descrizione = references.ResMan("Common",Lingua,"testoMailIscrizione1").ToString() + "<br/>";
            Descrizione += references.ResMan("Common",Lingua,"TestoIscrivi").ToString() + "<br/>";

            //Descrizione += references.ResMan("Common",Lingua,"FormTesto1").ToString() + " " + cliente.CodiceCard + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto2").ToString() + " " + cliente.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto3").ToString() + " " + cliente.Cognome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto4").ToString() + " " + cliente.Email + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTestoPass").ToString() + " " + password + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto5").ToString() + " " + cliente.CodiceNAZIONE + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            Province p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate (Province _p) { return _p.Codice == cliente.CodiceREGIONE; });
            if (p != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto6").ToString() + " " + p.Regione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate (Province _p) { return _p.Codice == cliente.CodicePROVINCIA; });
            if (p != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto7").ToString() + " " + p.Provincia + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Comune c = WelcomeLibrary.UF.Utility.ElencoComuni.Find(delegate (Comune _p) { return _p.Nome.ToLower() == cliente.CodiceCOMUNE.ToLower(); });
            if (c != null)
                Descrizione += references.ResMan("Common",Lingua,"FormTesto8").ToString() + " " + c.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            Descrizione += references.ResMan("Common",Lingua,"FormTesto9").ToString() + " " + cliente.Cap + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto10").ToString() + " " + cliente.Indirizzo + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto11").ToString() + " " + cliente.Telefono + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto12").ToString() + " " + cliente.Professione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
            Descrizione += references.ResMan("Common",Lingua,"FormTesto13").ToString() + " " + cliente.DataNascita.ToShortDateString() + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            Descrizione += references.ResMan("Common",Lingua,"FormTesto14").ToString() + " " + cliente.Spare1 + "<br/><br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

            Descrizione += cliente.TestoFormConsensi + "<br/>";
            Descrizione += " <br/> Il cliente ha richiesto l'invio newsletter : " + references.ResMan("Common", Lingua, "titolonewsletter1").ToString() + "<br/>";

            //Descrizione += "<a href=\"" + linkvalidazione + "\" target=\"_blank\" style=\"font-size:18px\">" + references.ResMan("Common",Lingua,"testoLinkValidazioneAttivazione").ToString() + "<br/>";
            Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente);

            string SoggettoMailPerGestore = "Mail di iscrizione nuovo cliente al sito web.";
            Utility.invioMailGenerico(nomecliente, Mailcliente, SoggettoMailPerGestore, Descrizione, Email, Nome);
        }
        catch (Exception err)
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
    private void RiempiDdlNazione(string valore, DropDownList ddlNazione)
    {
        List<Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == Lingua; });
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
    private void RiempiDdlRegione(string valore, DropDownList ddlregione)
    {
        DropDownList dnaz = ((DropDownList)(ddlNazione));
        ddlregione.Items.Clear();
        ddlregione.Items.Insert(0, "Seleziona Regione");
        ddlregione.Items[0].Value = "";
        //HtmlInputControl txtRE = ((HtmlInputControl)((RepeaterItem)(ddlregione).NamingContainer).FindControl("txtRE"));
        //txtRE.Value = valore;
        //HtmlInputControl txtPR = ((HtmlInputControl)((RepeaterItem)(ddlregione).NamingContainer).FindControl("txtPR"));
        //txtPR.Value = "";
        //HtmlInputControl txtCO = ((HtmlInputControl)((RepeaterItem)(ddlregione).NamingContainer).FindControl("txtCO"));
        //txtCO.Value = "";
        if (dnaz.SelectedValue == "IT")
        {
            WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
            List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.SiglaNazione.ToLower() == dnaz.SelectedValue.ToLower()); });
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
            foreach (Province r in regioni)
            {
                ListItem i = new ListItem(r.Regione, r.Codice);
                ddlregione.Items.Add(i);
            }
            try
            {
                ddlregione.SelectedValue = valore;
            }
            catch { valore = ""; ddlregione.SelectedValue = valore; }
        }
    }
    private void RiempiDdlProvincia(string valore, DropDownList ddlprovincia)
    {
        DropDownList dnaz = (ddlNazione);
        DropDownList dreg = (ddlRegione);
        ddlprovincia.Items.Clear();
        ddlprovincia.Items.Insert(0, "Seleziona Provincia");
        ddlprovincia.Items[0].Value = "";
        //HtmlInputControl txtPR = ((HtmlInputControl)((RepeaterItem)(ddlprovincia).NamingContainer).FindControl("txtPR"));
        //txtPR.Value = valore;
        //HtmlInputControl txtCO = ((HtmlInputControl)((RepeaterItem)(ddlprovincia).NamingContainer).FindControl("txtCO"));
        //txtCO.Value = "";
        if (dreg.SelectedValue != "" && dnaz.SelectedValue == "IT")
        {
            Province _tmp = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.Codice == dreg.SelectedValue); });
            List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == dnaz.SelectedValue.ToLower()); });
            provincelingua.Sort(new GenericComparer<Province>("Provincia", System.ComponentModel.ListSortDirection.Ascending));
            foreach (Province r in provincelingua)
            {
                ListItem i = new ListItem(r.Provincia, r.Codice);
                ddlprovincia.Items.Add(i);
            }
            try
            {
                ddlprovincia.SelectedValue = valore;
            }
            catch { valore = ""; ddlprovincia.SelectedValue = valore; }
        }
    }
    private void RiempiDdlComune(string valore, DropDownList ddlcomune)
    {
        DropDownList dnaz = (ddlNazione);
        DropDownList dreg = (ddlRegione);
        DropDownList dpro = (ddlProvincia);
        ddlcomune.Items.Clear();
        ddlcomune.Items.Insert(0, "Seleziona Comune");
        ddlcomune.Items[0].Value = "";
        //HtmlInputControl txtCO = ((HtmlInputControl)((RepeaterItem)(ddlcomune).NamingContainer).FindControl("txtCO"));
        //txtCO.Value = valore;
        if (dpro.SelectedValue != "" && dnaz.SelectedValue == "IT")
        {
            List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate (WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == dpro.SelectedValue); });
            comunilingua.Sort(new GenericComparer<Comune>("Nome", System.ComponentModel.ListSortDirection.Ascending));
            foreach (Comune r in comunilingua)
            {
                ListItem i = new ListItem(r.Nome, r.Nome);
                ddlcomune.Items.Add(i);
            }
            try
            {
                ddlcomune.SelectedValue = valore;
            }
            catch { valore = ""; ddlcomune.SelectedValue = valore; }
        }
    }

    private void CaricaDatiDdlRicerca(string Nazione, string Regione, string Provincia, string Comune, string Tipologia, string Fasciaprezzo)
    {
        string siglanazione = Nazione;
        RiempiDdlNazione(siglanazione.ToUpper(), ddlNazione);
        RiempiDdlRegione(Regione, ddlRegione);
        RiempiDdlProvincia(Provincia, ddlProvincia);
        RiempiDdlComune(Comune, ddlComune);
    }
    protected void ddlNazione_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca(ddlNazione.SelectedValue, "", "", "", "", "");

    }
    protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca(ddlNazione.SelectedValue, ddlRegione.SelectedValue, ddlProvincia.SelectedValue, "", "", "");

    }
    protected void ddlRegione_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca(ddlNazione.SelectedValue, ddlRegione.SelectedValue, "", "", "", "");
    }

}
