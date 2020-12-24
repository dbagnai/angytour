using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;
using System.Data.OleDb;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;
using ClosedXML;

public partial class AreaContenuti_GestioneClienti : CommonPage
{
    protected void output_PreRender(object sender, EventArgs e)
    {
        //GENERO UNA TEXTBOX SUL CLIENT CHE INDICA IL RUSULTATO DELL'AZIONE SVOLTA!!!
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (output.Text != "")
        {
            string messaggio = output.Text.Replace("&nbsp <br/>", "\\r\\n");
            messaggio = messaggio.Replace("<br/>", "\\r\\n");
            messaggio = messaggio.Replace("'", "");
            sb.Append("alert('" + messaggio + "');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertCliente", sb.ToString(), true);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SetCulture("it"); //forzo la cultura italia
        if (!IsPostBack)
        {
            /////////////////////////////////////////////////////////////////////
            //Verifichiamo accesso socio e impostiamo la visualizzazione corretta
            //Spegnendo le cose che non devono essere visibili ai soci!!!
            /////////////////////////////////////////////////////////////////////
            usermanager USM = new usermanager();
            if (USM.ControllaRuolo(User.Identity.Name, "Commerciale"))
                ImpostaVisualizzazione();


            CaricaDati("");
            AutoCompleteExtender1.ContextKey = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            CaricaDatiDdlRicerca("IT", "", "", "", "0");
            PopolaDdlNazioneFiltro();
            PopolaTipiClienti();
        }
        else
        {
            output.Text = "";
            outputimporta.Text = "";
        }
    }
    private void ImpostaVisualizzazione()
    {
        string idcliente = getidcliente(User.Identity.Name);
        if (!string.IsNullOrEmpty(idcliente))
        {

            ((HtmlGenericControl)Master.FindControl("ulMainbar")).Visible = false; //Spengo la barra navigazione

        }
        else
        {
            Response.Redirect("~/Error.aspx?Error=Utente non trovato");
        }

    }
    #region PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER

    protected void PagerRisultati_PageCommand(object sender, string PageNum)
    {
        UC_PagerEx PagerRisultati = sender as UC_PagerEx;
        PagerRisultati.CurrentPage = Convert.ToInt32(PageNum);
        Cliente _paramcli = new Cliente();
        _paramcli.Lingua = "";
        _paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
        _paramcli.id_tipi_clienti = ddlTipiClientiFiltro.SelectedValue;
        CaricaDati("", _paramcli);
    }

    protected void PagerRisultati_PageGroupClickNext(object sender, string spare)
    {
        //dimensioneGruppo
        //nGruppoPagine
        UC_PagerEx PagerRisultati = sender as UC_PagerEx;
        Cliente _paramcli = new Cliente();
        _paramcli.Lingua = "";
        _paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
        _paramcli.id_tipi_clienti = ddlTipiClientiFiltro.SelectedValue;
        CaricaDati("", _paramcli);
    }

    protected void PagerRisultati_PageGroupClickPrev(object sender, string spare)
    {
        UC_PagerEx PagerRisultati = sender as UC_PagerEx;
        Cliente _paramcli = new Cliente();
        _paramcli.Lingua = "";
        _paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
        _paramcli.id_tipi_clienti = ddlTipiClientiFiltro.SelectedValue;
        CaricaDati("", _paramcli);
    }

    #endregion
    protected void CaricaDati(string codicecliente, Cliente _paramCliente = null)
    {
        ClientiDM cliDM = new ClientiDM();
        ClienteCollection cliColl = new ClienteCollection();
        Cliente cli;

        //FILTRO DATA DI NASCITA
        string _giorno = txtGiorno.Text;
        string _mese = txtMese.Text;
        string _anno = txtAnno.Text;
        if (!string.IsNullOrEmpty(_giorno) || !string.IsNullOrEmpty(_mese) || !string.IsNullOrEmpty(_anno))
        {
            _paramCliente = new Cliente();
            // _paramCliente.DataNascita 
            int anno = 0;
            int mese = 0;
            int giorno = 0;
            int.TryParse(_anno, out anno);
            int.TryParse(_mese, out mese);
            int.TryParse(_giorno, out giorno);
            _paramCliente.Spare1 = (giorno == 0 ? "0" : giorno.ToString()) + "/" + (mese == 0 ? "0" : mese.ToString()) + "/" + (anno == 0 ? "0" : anno.ToString());
        }


        if (radSessoRicerca.SelectedValue != "")
        {
            if (_paramCliente == null) _paramCliente = new Cliente();
            _paramCliente.Sesso = radSessoRicerca.SelectedValue.ToLower();
        }
        if (!string.IsNullOrEmpty(txtetamin.Text) || !string.IsNullOrEmpty(txtetamax.Text))
        {
            if (_paramCliente == null) _paramCliente = new Cliente();

            string etamin = txtetamin.Text;
            string etamax = txtetamax.Text;

            int emin = 0;
            int.TryParse(etamin, out emin);
            _paramCliente.Spare2 = emin.ToString() + "|";


            int emax = 0;
            int.TryParse(etamax, out emax);

            if (emax < emin) emax = 400; //Se ho messo eta max inferiore non metto limite eta max
            if (emax == 0 && emin > 0) emax = 400; //Se eta min diversa da zero e emax è zero non metto limite eta massima
            _paramCliente.Spare2 += emax.ToString();
        }


        if (codicecliente == "")
            cliColl = cliDM.CaricaClientiFiltrati(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _paramCliente, true, PagerRisultati.CurrentPage, PagerRisultati.PageSize);//Passando null -> prende tutti i clienti, inoltre baypasso il filtro validati
        else
        {
            cli = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicecliente);
            cliColl.Add(cli);
            cliColl.Totrecs = 1;
        }
        //cliColl.Sort(new GenericComparer2<Cliente>("Cognome", System.ComponentModel.ListSortDirection.Ascending, "Nome", System.ComponentModel.ListSortDirection.Ascending));

        //  output.Text += "row returned : " + cliColl.Count +  " total row found:" + cliColl.Totrecs;

        long nrecordfiltrati = cliColl.Totrecs;
        PagerRisultati.TotalRecords = (long)cliColl.Totrecs;
        if (nrecordfiltrati == 0) PagerRisultati.CurrentPage = 1;


#if false
        //Selezionamo i risultati in base al numero di pagina e alla sua dimensione per la paginazione
        //Utilizzando la classe di paginazione
        WelcomeLibrary.UF.Pager<Cliente> _pager = new WelcomeLibrary.UF.Pager<Cliente>(cliColl);
        int nrecordfiltrati = _pager.Count;
        //if (nrecordfiltrati != 0)
        PagerRisultati.TotalRecords = nrecordfiltrati;
        if (nrecordfiltrati == 0) PagerRisultati.CurrentPage = 1;
        rptClienti.DataSource = _pager.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);

#endif
        rptClienti.DataSource = cliColl;
        rptClienti.DataBind();
        // updPanel1.Update();
    }
    protected void rptClienti_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var selection = (Cliente)e.Item.DataItem;
            var rb = (RadioButtonList)e.Item.FindControl("radSesso");
            rb.Items.FindByValue(selection.Sesso.ToLower()).Selected = true;
        }

    }
    protected void btnSeleziona_Click(object sender, EventArgs e)
    {
        CaricaDati(txtCLIENTE.Text);
    }
    protected void FiltraPerNazione(object sender, EventArgs e)
    {
        Cliente _paramcli = new Cliente();
        _paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
        _paramcli.id_tipi_clienti = ddlTipiClientiFiltro.SelectedValue;
        _paramcli.Lingua = "";
        CaricaDati("", _paramcli);
    }
    protected void TipoClienteChange(object sender, EventArgs e)
    {
        Cliente _paramcli = new Cliente();
        _paramcli.Lingua = "";
        _paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
        _paramcli.id_tipi_clienti = ddlTipiClientiFiltro.SelectedValue;
        CaricaDati("", _paramcli);
    }
    private void PopolaDdlNazioneFiltro()
    {
        //Riempio la ddl nazione in base alla lingua
        List<Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == "I"; });
        nazioni.Sort(new GenericComparer<Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));

        ddlNazioniFiltro.Items.Clear();
        ddlNazioniFiltro.Items.Insert(0, "Tutte");
        ddlNazioniFiltro.Items[0].Value = "";
        ddlNazioniFiltro.DataSource = nazioni;
        ddlNazioniFiltro.DataTextField = "Campo1";
        ddlNazioniFiltro.DataValueField = "Codice";
        ddlNazioniFiltro.DataBind();
        try
        {
            ddlNazioniFiltro.SelectedValue = "";
        }
        catch { }
    }
    private void PopolaTipiClienti()
    {
        //Riempio la ddl tipi clienti
        List<Tabrif> tipiclienti = Utility.TipiClienti.FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlTipiClientiFiltro.Items.Clear();
        ddlTipiClientiFiltro.Items.Insert(0, "Tutti");
        ddlTipiClientiFiltro.Items[0].Value = "";
        ddlTipiClientiFiltro.DataSource = tipiclienti;
        ddlTipiClientiFiltro.DataTextField = "Campo1";
        ddlTipiClientiFiltro.DataValueField = "Codice";
        ddlTipiClientiFiltro.DataBind();
        try
        {
            ddlTipiClientiFiltro.SelectedValue = "";
        }
        catch { }

        ddlTipiClientiUpdate.Items.Clear();
        ddlTipiClientiUpdate.Items.Insert(0, "Seleziona tipo");
        ddlTipiClientiUpdate.Items[0].Value = "";
        ddlTipiClientiUpdate.DataSource = tipiclienti;
        ddlTipiClientiUpdate.DataTextField = "Campo1";
        ddlTipiClientiUpdate.DataValueField = "Codice";
        ddlTipiClientiUpdate.DataBind();
        try
        {
            ddlTipiClientiUpdate.SelectedValue = "";
        }
        catch { }



        ddlTipiClientiImporta.Items.Clear();
        ddlTipiClientiImporta.Items.Insert(0, "Seleziona tipo");
        ddlTipiClientiImporta.Items[0].Value = "";
        ddlTipiClientiImporta.DataSource = tipiclienti;
        ddlTipiClientiImporta.DataTextField = "Campo1";
        ddlTipiClientiImporta.DataValueField = "Codice";
        ddlTipiClientiImporta.DataBind();
        try
        {
            ddlTipiClientiImporta.SelectedValue = "";
        }
        catch { }

    }
    protected void TipoClienteUpdate(object sender, EventArgs e)
    {
        if (ddlTipiClientiUpdate.SelectedValue != "")
            txtTipoClienteUpdate.Text = ddlTipiClientiUpdate.SelectedItem.Text;
        else
            txtTipoClienteUpdate.Text = "";
    }
    public string GetUserData(object dataitem)
    {
        string ret = "";
        //  Literal litUserdata = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Literal)sender).NamingContainer).FindControl("litUserdata")));
        Cliente cli = new Cliente();
        cli = (Cliente)dataitem;
        usermanager USM = new usermanager();
        string username = USM.GetUsernamebycamporofilo("idCliente", cli.Id_cliente.ToString());
        if (!string.IsNullOrEmpty(username))
        {
            ret = username;
        }
        else
            ret = "non presente";
        return ret;
    }

    protected void EliminaUtente_click(object sender, EventArgs e)
    {
        Literal outrow = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("outRow")));
        Literal litMsRow = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("litMsRow")));
        ClientiDM cliDM = new ClientiDM();
        Cliente cli = new Cliente();
        //string CodClienteCliccato = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCod"))).Text;
        string CodClienteCliccato = ((Button)sender).CommandArgument;
        usermanager USM = new usermanager();
        string username = USM.GetUsernamebycamporofilo("idCliente", CodClienteCliccato);
        if (USM.EliminaUtentebyUsername(username))
        {
            outrow.Text = "Eliminato Utente";
            Literal litUserdata = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("litUserdata")));
            litUserdata.Text = "non presente";

        }
    }

    protected void Setnewpass_click(object sender, EventArgs e)
    {
        Literal outrow = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("outRow")));
        Literal litMsRow = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("litMsRow")));
        HtmlInputText txtNewpass = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtNewpass")));
        ClientiDM cliDM = new ClientiDM();
        Cliente cli = new Cliente();
        //string CodClienteCliccato = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCod"))).Text;
        string CodClienteCliccato = ((Button)sender).CommandArgument;
        usermanager USM = new usermanager();
        string username = USM.GetUsernamebycamporofilo("idCliente", CodClienteCliccato);
        string newpass = USM.Resetpassword(username);
        string msgrsp = USM.Cambiopassword(username, newpass, txtNewpass.Value);
        outrow.Text = msgrsp;
    }

    protected void Generautente_click(object sender, EventArgs e)
    {
        ClientiDM cliDM = new ClientiDM();
        Cliente cli = new Cliente();
        //string CodClienteCliccato = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCod"))).Text;
        string CodClienteCliccato = ((Button)sender).CommandArgument;
        long idcliente = 0;
        long.TryParse(CodClienteCliccato, out idcliente);
        cli.Id_cliente = idcliente;
        cli = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodClienteCliccato); //Ricarico il cliente completo dal db

        if (cli != null && cli.Id_cliente != 0)
        {
            Literal outrow = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("outRow")));
            Literal litMsRow = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("litMsRow")));

            usermanager USM = new usermanager();
            string username = USM.GetUsernamebycamporofilo("idCliente", idcliente.ToString());
            if (string.IsNullOrEmpty(username))
            {
                string password = "";
                username = idcliente + "-" + cli.Email;
                //USM.CreaUtente(idcliente.ToString(), ref username, ref password, "Commerciale");
                USM.CreaUtente(idcliente.ToString(), ref username, ref password, "Autore");
                outrow.Text = password;
                Literal litUserdata = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("litUserdata")));
                litUserdata.Text = username;
            }
            else outrow.Text = "Utente già esistente -> Username: " + username;

        }

    }
    protected void Aggiorna_click(object sender, EventArgs e)
    {
        ClientiDM cliDM = new ClientiDM();
        Cliente cli = new Cliente();
        string CodClienteCliccato = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCod"))).Text;
        long idcliente = 0;
        long.TryParse(CodClienteCliccato, out idcliente);
        cli.Id_cliente = idcliente;
        cli = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodClienteCliccato); //Ricarico il cliente completo dal db
        // e aggiorno solo i campi che mi interessano

        //cli.CodiceNAZIONE = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtNA"))).Value;
        cli.CodiceNAZIONE = ((DropDownList)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("ddlNazione"))).SelectedValue;
        cli.CodiceREGIONE = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtRE"))).Value;
        cli.CodicePROVINCIA = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtPR"))).Value;
        cli.CodiceCOMUNE = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCO"))).Value;
        cli.Cap = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCA"))).Value;
        cli.Indirizzo = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtIN"))).Value;
        cli.Telefono = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtTE"))).Value;
        cli.Emailpec = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtEMP"))).Value;
        cli.Cellulare = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCE"))).Value;
        cli.Spare2 = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtFA"))).Value;
        cli.Spare1 = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtWE"))).Value;
        cli.Pivacf = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtPI"))).Value;
        cli.Professione = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtPF"))).Value;
        cli.Codicisconto = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCS"))).Value.ToLower();

        string datanascita = ((HtmlInputText)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtDN"))).Value;
        DateTime _dt = DateTime.MinValue;
        //DateTime.TryParse(datanascita, out _dt);
        DateTime.TryParseExact(datanascita, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _dt);
        cli.DataNascita = _dt;
        cli.Consenso1 = ((HtmlInputCheckBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtC1"))).Checked;

        //DATI BASE
        cli.Cognome = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCG"))).Text.Trim();
        cli.Nome = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtNO"))).Text.Trim();
        cli.Email = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtEM"))).Text.Trim();

        //cli.Lingua = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtLN"))).Text;
        cli.Lingua = ((DropDownList)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("ddlLingua"))).SelectedValue;

        //string idtipocliente = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtTC"))).Text;
        cli.id_tipi_clienti = ((DropDownList)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("ddlTipiClienti"))).SelectedValue;
        cli.Validato = ((CheckBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("chkVD"))).Checked;

        cli.Sesso = ((RadioButtonList)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("radSesso"))).SelectedValue;

        //Controlliamo eventuale duplicazione di codici sconto
        Dictionary<string, double> dict = ClientiDM.SplitCodiciSconto(cli.Codicisconto);
        if (dict != null)
        {
            foreach (string csconto in dict.Keys)
            {
                //Vediamo se duplicata
                Cliente cliduplicato = cliDM.CaricaClientePerCodicesconto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, csconto);
                if (cliduplicato != null && cliduplicato.Id_cliente != 0 && cliduplicato.Id_cliente != cli.Id_cliente)
                {
                    output.Text = "Modificare codice sconto inserito. Presente cliente con codice sconto uguale , id cliente :" + cliduplicato.Id_cliente.ToString() + " Non consentiti clienti con codici sconto uguali";
                    return;
                }
            }
        }


        //RICORDA DI VALIDARE I CAMPI OBBLIGATORI!!!! e la tipologia di dati
        if (cli.Email.Trim() == string.Empty)
        {
            output.Text = "Inserire Email";
            return;
        }
        if (cli.Id_cliente == 0)
        {
            output.Text = "Codice Cliente non specificato";
            return;
        }
        try
        {

            //Controllo nel dbEMAIL per coincidenze!!!!
            if (cli.Email.ToLower().Trim() != "")
            {
                // ClientiCollection duplicati = cliDM.ControllaDuplicazioneDatiCliente(cli);
                Cliente duplicato = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email.ToLower().Trim(), cli.id_tipi_clienti);

                if (duplicato != null && duplicato.Id_cliente != 0 && duplicato.Id_cliente != cli.Id_cliente && duplicato.id_tipi_clienti == cli.id_tipi_clienti)
                {
                    output.Text = "Presenti clienti con email coincidente : <br/> ";
                    //foreach (Cliente tmp in duplicati)
                    //{
                    //    output.Text += tmp.Cod_cliente + " " + tmp.Rag_soc + " <br/> ";
                    //}
                    return;
                }
            }
            else
            {
                output.Text = "Inserire EMAIL <br/> ";
                return;
            }


            //Scrivo i dati nel db
            cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);

            Cliente _paramcli = new Cliente();
            _paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
            _paramcli.id_tipi_clienti = ddlTipiClientiFiltro.SelectedValue;
            _paramcli.Lingua = "";
            CaricaDati("", _paramcli);
        }
        catch (Exception error)
        {
            output.Text = error.Message;
        }


    }
    protected void Cancella_click(object sender, EventArgs e)
    {

        string CodClienteCliccato = ((TextBox)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("txtCod"))).Text;
        ClientiDM cliDM = new ClientiDM();
        long i = 0;
        long.TryParse(CodClienteCliccato, out i);
        string ritorno = cliDM.CancellaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, i);
        output.Text = ritorno;

        //Ricarico i dati per la visualizzazione aggiornata
        Cliente _paramcli = new Cliente();
        _paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
        _paramcli.id_tipi_clienti = ddlTipiClientiFiltro.SelectedValue;
        _paramcli.Lingua = "";
        CaricaDati("", _paramcli);
    }


    protected void Inserisci_click(object sender, EventArgs e)
    {
        //nello stesso modo trovo tutti gli altri valori modificati nelle textbox dall'operatore
        //// e li posso scrivere aggiornando il db o inserendo per i recod nuovi 
        //// i codici nuovi non presenti in tbl_clienti vengono automaticamente
        ////inseriti come nuovi recod in tabella dalla stored procedure USP_UpdateClienti_control
        //// basta creare una procedura per i singoli clienti MemorizzaCliente in dbDataAccess ....
        Cliente cli = new Cliente();
        ////DATI BASE
        //int id = 0;
        //int.TryParse(txtCod.Text.Trim(), out id);
        cli.Id_cliente = 0;//Nuovo Cliente
        cli.Cognome = txtCG.Text.Trim();
        cli.Nome = txtNO.Text.Trim();
        cli.Email = txtEM.Text.Trim().ToLower();



        //cli.Lingua = txtLN.Text.Trim().ToLower();
        cli.Lingua = ddlLingua.SelectedValue.ToUpper();
        //string idtipocliente = txtTC.Text;
        cli.id_tipi_clienti = ddlTipiClienti.SelectedValue;
        if (cli.id_tipi_clienti.Trim() == string.Empty)
        {
            output.Text = "Inserire Tipologia Cliente";
            return;
        }


        cli.Validato = chkVD.Checked;
        //RICORDA DI VALIDARE I CAMPI OBBLIGATORI!!!! e la tipologia di dati
        if (cli.Email.Trim() == string.Empty)
        {
            output.Text = "Inserire Email Sociale";
            return;
        }

        //cli.CodiceNAZIONE = txtNA.Value.Trim();
        cli.CodiceNAZIONE = ddlNazione.SelectedValue;
        cli.CodiceREGIONE = txtRE.Value.Trim();
        cli.CodicePROVINCIA = txtPR.Value.Trim();
        cli.CodiceCOMUNE = txtCO.Value.Trim();


        cli.Cap = txtCA.Value.Trim();
        cli.Indirizzo = txtIN.Value.Trim();
        cli.Telefono = txtTE.Value.Trim();
        cli.Emailpec = txtEMP.Value.Trim();
        cli.Cellulare = txtCE.Value.Trim();
        cli.Spare2 = txtFA.Value.Trim();
        cli.Spare1 = txtWE.Value.Trim();
        cli.Pivacf = txtPI.Value.Trim();
        cli.Professione = txtPF.Value.Trim();
        cli.Codicisconto = txtCS.Value.Trim().ToLower();

        string datanascita = txtDN.Value.Trim();
        DateTime _dt = DateTime.MinValue;
        //DateTime.TryParse(datanascita, out _dt);
        DateTime.TryParseExact(datanascita, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _dt);

        cli.DataNascita = _dt;
        cli.Consenso1 = txtC1.Checked;

        //Inseriamo i dati del cliente nel db testando la presenza di duplicati
        try
        {
            ClientiDM cliDM = new ClientiDM();

            //Controlliamo eventuale duplicazione di codici sconto
            Dictionary<string, double> dict = ClientiDM.SplitCodiciSconto(cli.Codicisconto);
            if (dict != null)
            {
                foreach (string csconto in dict.Keys)
                {
                    //Vediamo se duplicata
                    Cliente cliduplicato = cliDM.CaricaClientePerCodicesconto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, csconto);
                    if (cliduplicato != null && cliduplicato.Id_cliente != 0 && cliduplicato.Id_cliente != cli.Id_cliente)
                    {
                        output.Text = "Modificare codice sconto inserito. Presente cliente con codice sconto uguale , id cliente :" + cliduplicato.Id_cliente.ToString() + " Non consentiti clienti con codici sconto uguali <br/>";
                        return;
                    }
                }
            }

            //Controllo nel dbEMAIL per coincidenze!!!!
            if (cli.Email.ToLower().Trim() != "")
            {
                // ClientiCollection duplicati = cliDM.ControllaDuplicazioneDatiCliente(cli);
                Cliente duplicato = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email.ToLower().Trim(), cli.id_tipi_clienti);
                if (duplicato != null && duplicato.Id_cliente != 0 && duplicato.Id_cliente != cli.Id_cliente && duplicato.id_tipi_clienti == cli.id_tipi_clienti)
                {
                    output.Text = "Presenti clienti con email coincidente : <br/> ";
                    //foreach (Cliente tmp in duplicati)
                    //{
                    //    output.Text += tmp.Cod_cliente + " " + tmp.Rag_soc + " <br/> ";
                    //}
                    return;
                }
            }
            else
            {
                output.Text = "Inserire EMAIL <br/> ";
                return;
            }

            cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);


            //Ricarico i dati per la visualizzazione aggiornata
            Cliente _paramcli = new Cliente();
            _paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
            _paramcli.id_tipi_clienti = ddlTipiClientiFiltro.SelectedValue;
            _paramcli.Lingua = "";
            CaricaDati("", _paramcli);
            //se TUTTO A BUON FINE SVUOTO LE CASELLE PER L'INSERIMETNO NUOVO CLIENTE --> DA FARE
            svuotacaselleinserimento();


        }
        catch (Exception error)
        {
            output.Text = error.Message;
        }
    }
    protected void svuotacaselleinserimento()
    {
        txtCod.Text = "";
        txtCG.Text = "";
        txtNO.Text = "";
        txtEM.Text = "";
        //txtLN.Text = "";
        ddlLingua.SelectedValue = "I";
        chkVD.Checked = false;
        //txtNA.Value = "";
        ddlNazione.SelectedValue = "IT";
        txtRE.Value = "";
        txtPR.Value = "";
        txtCO.Value = "";
        ddlRegione.SelectedValue = "";
        ddlProvincia.SelectedValue = "";
        ddlComune.SelectedValue = "";
        ddlTipiClienti.SelectedValue = "";
        radSesso.SelectedIndex = 0;

        txtCA.Value = "";
        txtIN.Value = "";
        txtTE.Value = "";
        txtEMP.Value = "";
        txtCE.Value = "";
        txtFA.Value = "";
        txtWE.Value = "";
        txtPI.Value = "";
        txtPF.Value = "";
        txtCS.Value = "";
        txtDN.Value = "";
        txtC1.Value = "";

    }


    protected void cancellaClientiTipologia_Click(object sender, EventArgs e)
    {
        try
        {
            ClientiDM cliDM = new ClientiDM();
            cliDM.CancellaClientiPerTipologia(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ddlTipiClientiImporta.SelectedValue);
            CaricaDati("");
            outputimporta.Text = "Eliminato tutti i clienti per la tipologia selezionata!";

        }
        catch (Exception error)
        {
            outputimporta.Text = error.Message;
        }

    }


    #region GESTIONE SELEZIONE LOCALIZZAZIONE GEOGRAFICA E TIPO CLIENTI


    protected void btnTipiClienti_Click(object sender, EventArgs e)
    {
        ClientiDM cDM = new ClientiDM();

        //Cerichiamo il prossimo progressivo codice libero
        //Utility.TipiClienti
        int ultimoprogressivo = 0;
        int _i = 0;
        Utility.TipiClienti.ForEach(c => ultimoprogressivo = (int.TryParse(c.Codice, out _i)) ? ((_i > ultimoprogressivo) ? _i : ultimoprogressivo) : (ultimoprogressivo));
        ultimoprogressivo += 1;
        Tabrif item = new Tabrif();

        if (string.IsNullOrWhiteSpace(ddlTipiClientiUpdate.SelectedValue))
            item.Codice = ultimoprogressivo.ToString();
        else
        {
            item = Utility.TipiClienti.Find(c => c.Codice == ddlTipiClientiUpdate.SelectedValue);
        }
        if (item == null)
        {
            outputimporta.Text = "Errore selezione aggiornamento tipo cliente";
            return;
        }

        item.Campo1 = txtTipoClienteUpdate.Text;
        item.Lingua = "I";
        cDM.InserisciAggiornaTipoCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item);
        txtTipoClienteUpdate.Text = "";

        //Aggiorno la visualizzazione
        Utility.CaricaListaStaticaTipiClienti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        CaricaDatiDdlRicerca("IT", "", "", "", "0");
        CaricaDati("");
        PopolaDdlNazioneFiltro();
        PopolaTipiClienti();
    }

    private void RiempiDdlTipiClienti(string Tipologia, DropDownList ddlTipiClienti)
    {
        //Riempio la ddl tipi clienti
        List<Tabrif> tipiclienti = Utility.TipiClienti.FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlTipiClienti.Items.Clear();
        ddlTipiClienti.Items.Insert(0, "Seleziona Tipo Cliente");
        ddlTipiClienti.Items[0].Value = "";
        foreach (Tabrif t in tipiclienti)
        {
            ListItem i = new ListItem(t.Campo1, t.Codice);
            ddlTipiClienti.Items.Add(i);
        }
        try
        {
            ddlTipiClienti.SelectedValue = Tipologia.ToUpper();
        }
        catch { Tipologia = ""; ddlTipiClienti.SelectedValue = Tipologia.ToUpper(); }
    }
    protected void ddlTipiClientiImporta_OnInit(object sender, EventArgs e)
    {
        RiempiDdlTipiClienti("0", (DropDownList)sender);
    }
    protected void ddlTipiClienti_OnInit(object sender, EventArgs e)
    {
        RiempiDdlTipiClienti("", (DropDownList)sender);
    }
    private void RiempiDdlNazione(string valore, DropDownList ddlNazione)
    {
        List<Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == "I"; });
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
    protected void ddlNazione_OnInit(object sender, EventArgs e)
    {
        RiempiDdlNazione("IT", (DropDownList)sender);
    }
    protected string VerificaPresenza(object item, string codice)
    {
        DropDownList dnaz = ((DropDownList)((RepeaterItem)item).FindControl("ddlNazione"));
        DropDownList dreg = ((DropDownList)((RepeaterItem)item).FindControl("ddlRegione"));
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
        //HtmlInputControl txtRE = ((HtmlInputControl)((RepeaterItem)item).FindControl("txtRE"));
        RiempiDdlRegione("", dreg);
        return codice;
    }
    protected void ddlNazioneRepeater_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //Per trovare qualsiasi controllo nell'elemento attuale del repeater
        DropDownList dreg = ((DropDownList)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("ddlRegione"));
        DropDownList dpro = ((DropDownList)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("ddlProvincia"));
        DropDownList dcom = ((DropDownList)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("ddlComune"));

        HtmlInputControl txtRE = ((HtmlInputControl)((DropDownList)sender).FindControl("txtRE"));
        //Qui dovresti aggiornare le liste geografiche in base alle selezioni nella attuale ddl nazione
        RiempiDdlRegione("", dreg);
        RiempiDdlProvincia("", dpro);
        RiempiDdlComune("", dcom);
    }

    private void RiempiDdlRegione(string valore, DropDownList ddlregione)
    {
        DropDownList dnaz = ((DropDownList)((RepeaterItem)(ddlregione).NamingContainer).FindControl("ddlNazione"));
        ddlregione.Items.Clear();
        ddlregione.Items.Insert(0, "Seleziona Regione");
        ddlregione.Items[0].Value = "";
        HtmlInputControl txtRE = ((HtmlInputControl)((RepeaterItem)(ddlregione).NamingContainer).FindControl("txtRE"));
        txtRE.Value = valore;
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
    protected string VerificaPresenzaRegione(object item, string codice)
    {
        HtmlInputControl txtRE = ((HtmlInputControl)((RepeaterItem)item).FindControl("txtRE"));
        txtRE.Value = codice;
        DropDownList dnaz = ((DropDownList)((RepeaterItem)item).FindControl("ddlNazione"));
        DropDownList dreg = ((DropDownList)((RepeaterItem)item).FindControl("ddlRegione"));
        if (dnaz.SelectedValue != "")
            RiempiDdlRegione(codice, dreg);
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
    protected void ddlRegione_OnInit(object sender, EventArgs e)
    {
        RiempiDdlRegione("", (DropDownList)sender);
    }
    protected void ddlRegioneRepeater_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        HtmlInputControl txtRE = ((HtmlInputControl)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("txtRE"));
        txtRE.Value = ((DropDownList)sender).SelectedValue;
        ////Poi devo fare il filtro sulle ddl in sequenza in base al valore selezionato!! provincia comune
        DropDownList dpro = ((DropDownList)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("ddlProvincia"));
        DropDownList dcom = ((DropDownList)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("ddlComune"));
        RiempiDdlProvincia("", dpro);
        RiempiDdlComune("", dcom);

    }
    private void RiempiDdlProvincia(string valore, DropDownList ddlprovincia)
    {
        DropDownList dnaz = ((DropDownList)((RepeaterItem)(ddlprovincia).NamingContainer).FindControl("ddlNazione"));
        DropDownList dreg = ((DropDownList)((RepeaterItem)(ddlprovincia).NamingContainer).FindControl("ddlRegione"));
        ddlprovincia.Items.Clear();
        ddlprovincia.Items.Insert(0, "Seleziona Provincia");
        ddlprovincia.Items[0].Value = "";
        HtmlInputControl txtPR = ((HtmlInputControl)((RepeaterItem)(ddlprovincia).NamingContainer).FindControl("txtPR"));
        txtPR.Value = valore;
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
    protected string VerificaPresenzaProvincia(object item, string codice)
    {
        HtmlInputControl txtPR = ((HtmlInputControl)((RepeaterItem)item).FindControl("txtPR"));
        txtPR.Value = codice;
        DropDownList dnaz = ((DropDownList)((RepeaterItem)item).FindControl("ddlNazione"));
        DropDownList dreg = ((DropDownList)((RepeaterItem)item).FindControl("ddlRegione"));
        DropDownList dpro = ((DropDownList)((RepeaterItem)item).FindControl("ddlProvincia"));

        if (dnaz.SelectedValue != "" && dreg.SelectedValue != "")
            RiempiDdlProvincia(codice, dpro);
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
    protected void ddlProvincia_OnInit(object sender, EventArgs e)
    {
        RiempiDdlProvincia("", (DropDownList)sender);
    }
    protected void ddlProvinciaRepeater_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        HtmlInputControl txtPR = ((HtmlInputControl)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("txtPR"));
        txtPR.Value = ((DropDownList)sender).SelectedValue;
        ////Poi devo fare il filtro sulle ddl in sequenza in base al valore selezionato!! provincia comune
        DropDownList dpro = ((DropDownList)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("ddlComune"));
        RiempiDdlComune("", dpro);
    }
    protected void ddlComune_OnInit(object sender, EventArgs e)
    {
        RiempiDdlComune("", (DropDownList)sender);
    }
    private void RiempiDdlComune(string valore, DropDownList ddlcomune)
    {
        DropDownList dnaz = ((DropDownList)((RepeaterItem)(ddlcomune).NamingContainer).FindControl("ddlNazione"));
        DropDownList dreg = ((DropDownList)((RepeaterItem)(ddlcomune).NamingContainer).FindControl("ddlRegione"));
        DropDownList dpro = ((DropDownList)((RepeaterItem)(ddlcomune).NamingContainer).FindControl("ddlProvincia"));
        ddlcomune.Items.Clear();
        ddlcomune.Items.Insert(0, "Seleziona Comune");
        ddlcomune.Items[0].Value = "";
        HtmlInputControl txtCO = ((HtmlInputControl)((RepeaterItem)(ddlcomune).NamingContainer).FindControl("txtCO"));
        txtCO.Value = valore;
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
    protected string VerificaPresenzaComune(object item, string codice)
    {
        HtmlInputControl txtCO = ((HtmlInputControl)((RepeaterItem)item).FindControl("txtCO"));
        txtCO.Value = codice;
        DropDownList dnaz = ((DropDownList)((RepeaterItem)item).FindControl("ddlNazione"));
        DropDownList dreg = ((DropDownList)((RepeaterItem)item).FindControl("ddlRegione"));
        DropDownList dpro = ((DropDownList)((RepeaterItem)item).FindControl("ddlProvincia"));
        DropDownList dcom = ((DropDownList)((RepeaterItem)item).FindControl("ddlComune"));

        if (dnaz.SelectedValue != "" && dreg.SelectedValue != "" && dpro.SelectedValue != "")
            RiempiDdlComune(codice, dcom);

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
    protected void ddlComuneRepeater_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        HtmlInputControl txtCO = ((HtmlInputControl)((RepeaterItem)((DropDownList)sender).NamingContainer).FindControl("txtCO"));
        txtCO.Value = ((DropDownList)sender).SelectedValue;
    }

    private void CaricaDatiDdlRicerca(string Nazione, string Regione, string Provincia, string Comune, string Tipologia)
    {
        //Riempio la ddl tipi clienti
        List<Tabrif> tipiclienti = Utility.TipiClienti.FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
        ddlTipiClienti.Items.Clear();
        ddlTipiClienti.Items.Insert(0, "Tutti");
        ddlTipiClienti.Items[0].Value = "";

        ddlTipiClienti.DataSource = tipiclienti;
        ddlTipiClienti.DataTextField = "Campo1";
        ddlTipiClienti.DataValueField = "Codice";
        ddlTipiClienti.DataBind();
        try
        {
            ddlTipiClienti.SelectedValue = Tipologia.ToUpper();
        }
        catch { Tipologia = ""; ddlTipiClienti.SelectedValue = Tipologia.ToUpper(); }

        string siglanazione = Nazione;
        List<Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == "I"; });
        nazioni.Sort(new GenericComparer<Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));

        //Riempio la ddl nazione in base alla lingua
        ddlNazione.Items.Clear();
        ddlNazione.DataSource = nazioni;
        ddlNazione.DataTextField = "Campo1";
        ddlNazione.DataValueField = "Codice";
        ddlNazione.DataBind();
        try
        {
            ddlNazione.SelectedValue = siglanazione.ToUpper();
        }
        catch { siglanazione = "it"; ddlNazione.SelectedValue = siglanazione.ToUpper(); }

        WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
        List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.SiglaNazione.ToLower() == siglanazione.ToLower()); });
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
        ddlRegione.Items.Clear();
        ddlRegione.Items.Insert(0, "Seleziona Regione");
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
        ddlProvincia.Items.Insert(0, "Seleziona Provincia");
        ddlProvincia.Items[0].Value = "";
        if (Regione != "")
        {
            Province _tmp = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.Codice == ddlRegione.SelectedValue); });
            provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == siglanazione.ToLower()); });
            provincelingua.Sort(new GenericComparer<Province>("Provincia", System.ComponentModel.ListSortDirection.Ascending));
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
        ddlComune.Items.Insert(0, "Seleziona Comune");
        ddlComune.Items[0].Value = "";
        if (Provincia != "")
        {
            List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate (WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == Provincia); });
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
    }
    protected void ddlNazione_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca(ddlNazione.SelectedValue, "", "", "", ddlTipiClienti.SelectedValue);

    }
    protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca(ddlNazione.SelectedValue, ddlRegione.SelectedValue, ddlProvincia.SelectedValue, "", ddlTipiClienti.SelectedValue);
        txtPR.Value = ddlProvincia.SelectedValue;
    }
    protected void ddlRegione_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaDatiDdlRicerca(ddlNazione.SelectedValue, ddlRegione.SelectedValue, "", "", ddlTipiClienti.SelectedValue);
        txtRE.Value = ddlRegione.SelectedValue;
    }
    protected void ddlComune_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtCO.Value = ddlComune.SelectedValue;
    }
    #endregion


    protected void ScriptManagerMaster_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {

        ((ScriptManager)sender).AsyncPostBackErrorMessage = e.Exception.Message.ToString();

        //Argomento di postback o callback non valido. 
        //La convalida degli eventi viene abilitata mediante <pages enableEventValidation="true"/> 
        //nella configurazione oppure mediante <%@ Page EnableEventValidation="true" %> in una pagina.
        //Per motivi di sicurezza, viene verificato che gli argomenti con cui eseguire il postback o 
        //il callback di eventi siano originati dal controllo server che ne aveva inizialmente eseguito 
        //il rendering. Se i dati sono validi e previsti, utilizzare il metodo 
        //ClientScriptManager.RegisterForEventValidation per registrare i dati di postback o callback 
        //per la convalida.

    }


    protected void btnImporta_onclick(object sender, EventArgs e)
    {

        string testo = txtImporta.Text;
        string testo1 = txtImporta1.Text;
        int clientiimportati = 0;

        if (!string.IsNullOrWhiteSpace(testo))
        {
            testo = testo.Replace(";", ",").Replace("\n", ",").Trim();
            string lingua = ddlLinguaImporta.SelectedValue;
            string tipocliente = ddlTipiClientiImporta.SelectedValue;

            if (string.IsNullOrEmpty(tipocliente))
            {
                outputimporta.Text = "Errore selezionare tipologia cliente per importazione!!!!";
                return;
            }


            List<string> clienti = new List<string>();
            if (testo.Contains(',') && testo.Length != 0)
            {
                string[] splitlits = testo.Split(',');
                clienti = splitlits.ToList();
            }

            //int resto = 0;
            //Math.DivRem(clienti.Count , 2, out resto);
            //if (resto != 0)
            //{
            //    output.Text = "Numero di elementi da importare dispari, controllare la formattazione del testo!!";
            //    return;
            //}
            ClientiDM cliDM = new ClientiDM();
            Cliente cli = new Cliente();
            int ncolonne = 2;

            foreach (string _c in clienti)
            {
                try
                {
                    cli.Lingua = lingua;
                    cli.id_tipi_clienti = tipocliente;
                    switch (ncolonne)
                    {
                        case 2:
                            cli = new Cliente();
                            cli.Id_cliente = 0;//Nuovo Cliente
                            ncolonne -= 1;
                            cli.Nome = "Cliente";
                            cli.Cognome = _c.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                            cli.Consenso1 = chkCommercialeImporta.Checked;
                            cli.ConsensoPrivacy = chkPrivacyImporta.Checked;
                            if (cli.Consenso1 && cli.ConsensoPrivacy)
                                cli.Validato = true;
                            break;
                        case 1:
                            ncolonne = 2;
                            if (_c.Contains("@"))
                            {
                                cli.Email = _c.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                                Cliente _tmp = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email.ToLower().Trim(), cli.id_tipi_clienti);
                                //if (_tmp != null && _tmp.Id_cliente != 0)
                                if (_tmp != null && _tmp.Id_cliente != 0 && _tmp.id_tipi_clienti == cli.id_tipi_clienti)
                                {
                                    continue; //se cliente esistente con stessa mail e all'interno della stessa categoria di clienti salto
                                }
                                cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);
                                clientiimportati += 1;
                                System.Threading.Thread.Sleep(100);
                            }
                            break;
                    }


                }
                catch (Exception err)
                {
                    outputimporta.Text += "Errore importazione: " + err.Message + "<br/>";
                }

            }
        }
        if (!string.IsNullOrWhiteSpace(testo1))
        {
            testo1 = testo1.Replace(";", ",").Replace("\n", ",").Trim();
            string lingua = ddlLinguaImporta.SelectedValue;
            string tipocliente = ddlTipiClientiImporta.SelectedValue;

            List<string> clienti = new List<string>();
            if (testo1.Contains(',') && testo1.Length != 0)
            {
                string[] splitlits = testo1.Split(',');
                clienti = splitlits.ToList();
            }

            //int resto = 0;
            //Math.DivRem(clienti.Count , 2, out resto);
            //if (resto != 0)
            //{
            //    output.Text = "Numero di elementi da importare dispari, controllare la formattazione del testo!!";
            //    return;
            //}
            ClientiDM cliDM = new ClientiDM();
            Cliente cli = new Cliente();
            int ncolonne = 3;
            foreach (string _c in clienti)
            {
                try
                {
                    cli.Lingua = lingua;
                    cli.id_tipi_clienti = tipocliente;
                    switch (ncolonne)
                    {
                        case 3:
                            cli = new Cliente();
                            cli.Id_cliente = 0;//Nuovo Cliente
                            ncolonne -= 1;
                            cli.Nome = _c.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                            // cli.Cognome = _c.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                            cli.Consenso1 = chkCommercialeImporta.Checked;
                            cli.ConsensoPrivacy = chkPrivacyImporta.Checked;
                            if (cli.Consenso1 && cli.ConsensoPrivacy)
                                cli.Validato = true;
                            break;
                        case 2:
                            cli.Id_cliente = 0;//Nuovo Cliente
                            ncolonne -= 1;
                            //cli.Nome = "Cliente";
                            cli.Cognome = _c.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                            cli.Consenso1 = chkCommercialeImporta.Checked;
                            cli.ConsensoPrivacy = chkPrivacyImporta.Checked;
                            if (cli.Consenso1 && cli.ConsensoPrivacy)
                                cli.Validato = true;
                            break;
                        case 1:
                            ncolonne = 3;
                            if (_c.Contains("@"))
                            {
                                cli.Email = _c.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                                Cliente _tmp = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email.ToLower().Trim(), cli.id_tipi_clienti);
                                //if (_tmp != null && _tmp.Id_cliente != 0)
                                if (_tmp != null && _tmp.Id_cliente != 0 && _tmp.id_tipi_clienti == cli.id_tipi_clienti)
                                {
                                    continue; //se cliente esistente con stessa mail e all'interno della stessa categoria di clienti salto
                                }
                                cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);
                                clientiimportati += 1;
                                System.Threading.Thread.Sleep(100);
                            }
                            break;
                    }


                }
                catch (Exception err)
                {
                    outputimporta.Text += "Errore importazione: " + err.Message + "<br/>";
                }

            }
        }
        outputimporta.Text += "Terminato correttamente importazione clienti, importato " + clientiimportati + " indirizzi ";
        CaricaDati("");


    }


    protected void btnUploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (uplFile.HasFile)
            {
                if (uplFile.PostedFile.ContentLength > 10000000)
                {
                    output.Text = "Il file non può essere caricato perché supera 10MB!";
                }
                else
                {
                    //CArichiamo il file sul server
                    string pathDestinazione = WelcomeLibrary.STATIC.Global.percorsoFisicoComune + "\\_temp";
                    string virpathDestinazione = WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp";
                    DirectoryInfo di = new DirectoryInfo(pathDestinazione);
                    if (!di.Exists)
                        di.Create();
                    string NomeCorretto = "dataimport.xlsx";
                    uplFile.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                    string returl = ReplaceAbsoluteLinks(virpathDestinazione + "/" + NomeCorretto);
                    output.Text = "Importato file dati correttamente";
                }
            }
        }
        catch (Exception err)
        {
            output.Text = "Errore durante importazione file dati :" + err.Message;
        }
    }



    /// <summary>
    /// Leggiamo direttamente i dati dal file excel e inseriamo o aggiorniamo il database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnParse_Click(object sender, EventArgs e)
    {
        string lingua = ddlLinguaImporta.SelectedValue;
        string tipocliente = ddlTipiClientiImporta.SelectedValue;
        if (string.IsNullOrEmpty(tipocliente))
        {
            outputimporta.Text = "Errore selezionare tipologia cliente per importazione!!!!";
            return;
        }
        if (string.IsNullOrEmpty(lingua))
            lingua = "I";
        Dictionary<string, string> listaimportati = new Dictionary<string, string>();


        string pathDestinazione = WelcomeLibrary.STATIC.Global.percorsoFisicoComune + "\\_temp" + "\\dataimport.xlsx";
        ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook(pathDestinazione);
        var ws = wb.Worksheet("Foglio1");
        //MAPPATURA DELLE POSIZIONE DELLE COLONNE NEL FILE EXCEL IN REALAZIONE AI CAMPI DEGLI OGGETTI DA CARICARE/AGGIORNARE
        const int cog = 1;
        const int nom = 2;
        const int ema = 3;
        const int dna = 4;
        const int cap = 5;
        const int ind = 6;
        const int tel = 7;
        const int cel = 8;
        const int fax = 9;
        const int web = 10;
        const int iva = 11;
        const int prf = 12;
        const int naz = 13;
        const int reg = 14;
        const int pro = 15;
        const int com = 16;
        const int ses = 17;

        //  Cognome Nome Email DataNascita etc ...

        // Look for the first row used ( Muovo il cursore alla prima riga usata nel foglio excel
        var firstRowUsed = ws.FirstRowUsed();
        // Narrow down the row so that it only includes the used part
        var categoryRow = firstRowUsed.RowUsed();
        // Move to the next row (it now has the titles)
        categoryRow = categoryRow.RowBelow();

        ClientiDM cliDM = new ClientiDM();
        Cliente clinew = new Cliente();
        Cliente cli_byemail = new Cliente();

        int _tmp = 0;
        double _tmpdbl = 0;
        DateTime _date = DateTime.MinValue;
        string email = "";
        string errorsforrecord = "";
        int rowscounter = 0;
#if true

        while (!categoryRow.Cell(ema).IsEmpty()) //Scorro finchè non trovo caselle email vuote!!!
        {
            try
            {
                clinew = new Cliente();
                clinew.Lingua = lingua;
                clinew.id_tipi_clienti = tipocliente;
                clinew.Consenso1 = chkCommercialeImporta.Checked;
                clinew.ConsensoPrivacy = chkPrivacyImporta.Checked;
                if (clinew.Consenso1 && clinew.ConsensoPrivacy)
                    clinew.Validato = true;

                email = categoryRow.Cell(ema).GetString().Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n').ToLower();
                if (!string.IsNullOrEmpty(email) && email.Contains("@"))
                {
                    clinew.Email = email;

                    //CARICAMENTO PROPRIETA' CON VERIFICA IN TABELLA CARATTERISTICHE
                    //mezzo = categoryRow.Cell(cmezzo).GetString().Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n').ToLower();
                    //Tabrif tmezzo = Utility.Caratteristiche[5].Find(t => t.Campo1.ToLower() == mezzo);
                    //_tmp = 0;
                    //if (tmezzo != null)
                    //{
                    //    if (Int32.TryParse(tmezzo.Codice, out _tmp))
                    //        itemnew.Caratteristica6 = _tmp;
                    //}
                    //else
                    //    errorsforrecord += mezzo + " : valore non trovato mezzo ; ";

                    //string prlistino = categoryRow.Cell(cprlis).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n').ToLower();
                    //if (Double.TryParse(prlistino, out _tmpdbl))
                    //    itemnew.PrezzoListino = _tmpdbl;

                    string datanascita = categoryRow.Cell(dna).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n').ToLower();
                    //if (DateTime.TryParse(datanascita, out _date))
                    if (DateTime.TryParseExact(datanascita, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _date))
                        clinew.DataNascita = _date;

                    clinew.Cognome = categoryRow.Cell(cog).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    clinew.Nome = categoryRow.Cell(nom).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.Cap = categoryRow.Cell(cap).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.Indirizzo = categoryRow.Cell(ind).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.Telefono = categoryRow.Cell(tel).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.Cellulare = categoryRow.Cell(cel).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.Spare2 = categoryRow.Cell(fax).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.Spare1 = categoryRow.Cell(web).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.Pivacf = categoryRow.Cell(iva).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.Professione = categoryRow.Cell(prf).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    clinew.CodiceNAZIONE = categoryRow.Cell(naz).GetString().Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n').ToLower();
                    string codicenaz = TrovaCodiceNazione(clinew.CodiceNAZIONE, lingua);
                    if (!string.IsNullOrEmpty(codicenaz))
                        clinew.CodiceNAZIONE = codicenaz;
                    else
                        clinew.CodiceNAZIONE = "IT";


                    clinew.CodiceREGIONE = categoryRow.Cell(reg).GetString().Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n').ToLower();
                    string codicereg = TrovaCodiceRegione(clinew.CodiceREGIONE, lingua);
                    if (!string.IsNullOrEmpty(codicereg))
                        clinew.CodiceREGIONE = codicereg;

                    clinew.CodicePROVINCIA = categoryRow.Cell(pro).GetString().Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n').ToLower();
                    string codiceprov = TrovaCodiceProvincia(clinew.CodicePROVINCIA, lingua);
                    if (!string.IsNullOrEmpty(codiceprov))
                        clinew.CodicePROVINCIA = codiceprov;

                    clinew.CodiceCOMUNE = categoryRow.Cell(com).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    string sesso = categoryRow.Cell(ses).GetString().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    if (!string.IsNullOrEmpty(sesso))
                        if (sesso.ToLower() == "uomo" || sesso.ToLower() == "donna")
                            clinew.Sesso = sesso.ToLower();



                    //AGGIORNIAMO O INSERIAMO
                    //Vediamo se esiste nel db un'cliente con quella mail e tipologia
                    cli_byemail = new Cliente();
                    cli_byemail = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, clinew.Email.ToLower().Trim(), clinew.id_tipi_clienti);
                    if (cli_byemail != null && cli_byemail.Id_cliente != 0 && cli_byemail.id_tipi_clienti == tipocliente)
                    {
                        //E' un aggiornamento -> Aggiorniamo i campi importati
                        if (!string.IsNullOrEmpty(clinew.Cognome))
                            cli_byemail.Cognome = clinew.Cognome;
                        if (!string.IsNullOrEmpty(clinew.Nome))
                            cli_byemail.Nome = clinew.Nome;
                        if ((clinew.DataNascita) != DateTime.MinValue)
                            cli_byemail.DataNascita = clinew.DataNascita;
                        if (!string.IsNullOrEmpty(clinew.Lingua))
                            cli_byemail.Lingua = clinew.Lingua;
                        cli_byemail.Consenso1 = clinew.Consenso1;
                        cli_byemail.ConsensoPrivacy = clinew.ConsensoPrivacy;
                        cli_byemail.Validato = clinew.Validato;

                        if (!string.IsNullOrEmpty(clinew.Cap))
                            cli_byemail.Cap = clinew.Cap;
                        if (!string.IsNullOrEmpty(clinew.Indirizzo))
                            cli_byemail.Indirizzo = clinew.Indirizzo;
                        if (!string.IsNullOrEmpty(clinew.Telefono))
                            cli_byemail.Telefono = clinew.Telefono;
                        if (!string.IsNullOrEmpty(clinew.Cellulare))
                            cli_byemail.Cellulare = clinew.Cellulare;
                        if (!string.IsNullOrEmpty(clinew.Spare2))
                            cli_byemail.Spare2 = clinew.Spare2;
                        if (!string.IsNullOrEmpty(clinew.Spare1))
                            cli_byemail.Spare1 = clinew.Spare1;
                        if (!string.IsNullOrEmpty(clinew.Professione))
                            cli_byemail.Professione = clinew.Professione;
                        if (!string.IsNullOrEmpty(clinew.CodiceNAZIONE))
                            cli_byemail.CodiceNAZIONE = clinew.CodiceNAZIONE;
                        if (!string.IsNullOrEmpty(clinew.CodiceREGIONE))
                            cli_byemail.CodiceREGIONE = clinew.CodiceREGIONE;
                        if (!string.IsNullOrEmpty(clinew.CodicePROVINCIA))
                            cli_byemail.CodicePROVINCIA = clinew.CodicePROVINCIA;
                        if (!string.IsNullOrEmpty(clinew.CodiceCOMUNE))
                            cli_byemail.CodiceCOMUNE = clinew.CodiceCOMUNE;
                        if (!string.IsNullOrEmpty(clinew.Sesso))
                            cli_byemail.Sesso = clinew.Sesso;
                    }
                    else
                    {
                        //E' un inserimento di un cliente non presente per la tipologia !! -> prendo i valori caricati in importazione
                        cli_byemail = clinew;
                    }
                    cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli_byemail);

                    //Creo la lista finale per gli importati
                    if (!listaimportati.ContainsKey(clinew.Email))
                        listaimportati.Add(clinew.Email, errorsforrecord);
                    errorsforrecord = "";


                }
            }
            catch (Exception err)
            {
                output.Text += "Errore importazione: " + err.Message + "<br/>";
                outputimporta.Text += "Errore importazione: " + err.Message + "<br/>";
            }
            categoryRow = categoryRow.RowBelow();
            rowscounter++;
        }

#endif

        output.Text += "Totale Elementi trovati : " + rowscounter + " <br/>";
        output.Text += "Elementi Importati/aggiornati : " + listaimportati.Keys.Count + " clienti<br/>";

        outputimporta.Text += "Totale Elementi trovati : " + rowscounter + " <br/>";
        outputimporta.Text += "Elementi Importati/aggiornati : " + listaimportati.Keys.Count + " clienti<br/>";

        //Visualizza eventuali errori!!! di importazione
        foreach (KeyValuePair<string, string> kv in listaimportati)
        {
            string errore = kv.Value; //se ci sono errori di importazione per il codice li visualizzo
            if (!string.IsNullOrEmpty(errore))
            {
                output.Text += kv.Key + ": " + errore + "<br/>";
                outputimporta.Text += kv.Key + ": " + errore + "<br/>";
            }
        }
        CaricaDati("");
        //if (chkArchivianoninlista.Checked)
        //{
        //    //Archivio gli articoli della marca che non sono nella lista di quelli importati
        //    //e dearchivio quello presenti in archivio che sono in listaimportati !!!

        //    //Carico gli ariticoli per codicetipologia e codicecategoria ( tutti quelli di una certa marca )
        //    List<OleDbParameter> parColl = new List<OleDbParameter>();
        //    OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", "rif000100");
        //    parColl.Add(p3);
        //    OleDbParameter p7 = new OleDbParameter("@CodiceCategoria", marca);
        //    parColl.Add(p7);
        //    OfferteCollection offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", "", true, "", true);
        //    //Li scorro e elimino quelli non presenti in listaimportati ( e imposto ad attivi quelli archiviati presenti in lista )
        //    if (offerte != null)
        //        foreach (Offerte o in offerte)
        //        {

        //            if (listaimportati.ContainsKey(o.CodiceProdotto))
        //            //if (listaimportati.Exists(t => t.CodiceProdotto == o.CodiceProdotto))
        //            {
        //                o.Archiviato = false;

        //            }
        //            else
        //                o.Archiviato = true;

        //            offDM.UpdateOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o);
        //        }
        //}

    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        Cliente _paramcli = new Cliente();
        _paramcli.Lingua = "";
        //_paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
        _paramcli.id_tipi_clienti = ddlTipiClientiImporta.SelectedValue;
        CaricaDati("", _paramcli);

        ClientiDM cliDM = new ClientiDM();
        List<Cliente> cliforexport = cliDM.CaricaClientiFiltrati(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _paramcli, true);

        if (cliforexport != null)
            cliforexport.Sort(new GenericComparer2<Cliente>("Cognome", System.ComponentModel.ListSortDirection.Ascending, "Nome", System.ComponentModel.ListSortDirection.Ascending));
        string csvName = "export-clienti-" + string.Format("{0:dd-MM-yyyy}", System.DateTime.Now) + ".csv";
        string pathFile = WelcomeLibrary.STATIC.Global.percorsoFisicoComune + "\\_temp\\";
        if (!System.IO.Directory.Exists(pathFile))
            System.IO.Directory.CreateDirectory(pathFile);
        string retmessage = cliDM.ExportClientiToCsv(pathFile, csvName, cliforexport);

        WelcomeLibrary.UF.SharedStatic.DownloadFile(WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/" + csvName);

    }
}