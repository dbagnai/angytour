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
using Newtonsoft.Json;

public partial class AreaContenuti_GestioneClienti : CommonPage
{
    public string idcliente
    {
        get { return ViewState["idcliente"] != null ? (string)(ViewState["idcliente"]) : ""; }
        set { ViewState["idcliente"] = value; }
    }
    //protected void output_PreRender(object sender, EventArgs e)
    //{
    //    //GENERO UNA TEXTBOX SUL CLIENT CHE INDICA IL RUSULTATO DELL'AZIONE SVOLTA!!!
    //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //    if (output.Text != "")
    //    {
    //        string messaggio = output.Text.Replace("&nbsp <br/>", "\\r\\n");
    //        messaggio = messaggio.Replace("<br/>", "\\r\\n");
    //        messaggio = messaggio.Replace("'", "");
    //        sb.Append("alert('" + messaggio + "');");
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertCliente", sb.ToString(), true);
    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        SetCulture("it"); //forzo la cultura italia
        if (!IsPostBack)
        {
            if (Request.QueryString["idcliente"] != null && Request.QueryString["idcliente"] != "")
            { idcliente = Request.QueryString["idcliente"].ToString(); idclienteHidden.Value = idcliente; }

            /////////////////////////////////////////////////////////////////////
            //Verifichiamo accesso socio e impostiamo la visualizzazione corretta
            //Spegnendo le cose che non devono essere visibili ai soci!!!
            /////////////////////////////////////////////////////////////////////
            usermanager USM = new usermanager();
            if (USM.ControllaRuolo(User.Identity.Name, "Commerciale"))
                ImpostaVisualizzazione();

            PopolaTipiClienti();
        }
        else
        {
            outputimporta.Text = "";

            if (Request["__EVENTTARGET"] == "cancellaclientidatipologia")
            {
                cancellaClientiTipologia_Click();
            }


        }
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

    #region gestione import ed export per clienti
    private void PopolaTipiClienti()
    {
        //Riempio la ddl tipi clienti
        List<Tabrif> tipiclienti = Utility.TipiClienti.FindAll(delegate (Tabrif _t) { return _t.Lingua == "I"; });
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
    protected void cancellaClientiTipologia_Click()
    {
        try
        {
            ClientiDM cliDM = new ClientiDM();
            cliDM.CancellaClientiPerTipologia(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ddlTipiClientiImporta.SelectedValue);
            // CaricaDati("");
            outputimporta.Text = "Eliminato tutti i clienti per la tipologia selezionata!";
        }
        catch (Exception error)
        {
            outputimporta.Text = error.Message;
        }

    }
    protected void btnTipiClienti_Click(object sender, EventArgs e)
    {
        ClientiDM cDM = new ClientiDM();
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
                    outputimporta.Text = "Il file non può essere caricato perché supera 10MB!";
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
                    outputimporta.Text = "Importato file dati correttamente";
                }
            }
        }
        catch (Exception err)
        {
            outputimporta.Text = "Errore durante importazione file dati :" + err.Message;
        }
    }
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
                outputimporta.Text += "Errore importazione: " + err.Message + "<br/>";
            }
            categoryRow = categoryRow.RowBelow();
            rowscounter++;
        }

#endif

        outputimporta.Text += "Totale Elementi trovati : " + rowscounter + " <br/>";
        outputimporta.Text += "Elementi Importati/aggiornati : " + listaimportati.Keys.Count + " clienti<br/>";

        //Visualizza eventuali errori!!! di importazione
        foreach (KeyValuePair<string, string> kv in listaimportati)
        {
            string errore = kv.Value; //se ci sono errori di importazione per il codice li visualizzo
            if (!string.IsNullOrEmpty(errore))
            {
                outputimporta.Text += kv.Key + ": " + errore + "<br/>";
            }
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        Cliente _paramcli = new Cliente();
        _paramcli.Lingua = "";
        //_paramcli.CodiceNAZIONE = ddlNazioniFiltro.SelectedValue;
        _paramcli.id_tipi_clienti = ddlTipiClientiImporta.SelectedValue;
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

    #endregion
    #region FUNZIONI GESTIONE MEMBERSHIP PER CLIENTI

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

                outrow.Text = "User: " + username + " Pass: " + password;
                Literal litUserdata = ((Literal)(((RepeaterItem)((System.Web.UI.WebControls.Button)sender).NamingContainer).FindControl("litUserdata")));
                litUserdata.Text = username;
            }
            else outrow.Text = "Utente già esistente -> Username: " + username;

        }

    }
    #endregion

}