using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;

//NOTA IMPORTANTE PER IL FUNZIONAMENTO IN MEDIUM TRUST DEL HTMLEDITOR DI AJAX!!!!!
//Per far funzionare ajax HTMLeditor in meduim trust 
//in EditPanel.cs sui sorgenti del controltoolkit per il 4.0
// quick work around by adding a Try Catch to the following line of code: 
//ActiveMode = (ActiveModeType) Int64.Parse(post, CultureInfo.InvariantCulture); 


public partial class AreaContenuti_GestioneNewsletter : CommonPage
{
    public List<long> ListaIdClientiSelezionatiInanagrafica
    {
        get { return ViewState["ListaIdClientiSelezionatiInanagrafica"] != null ? (List<long>)(ViewState["ListaIdClientiSelezionatiInanagrafica"]) : new List<long>(); }
        set { ViewState["ListaIdClientiSelezionatiInanagrafica"] = value; }
    }
    public List<long> ListaIdClientiSelezionatiIngruppo
    {
        get { return ViewState["ListaIdClientiSelezionatiIngruppo"] != null ? (List<long>)(ViewState["ListaIdClientiSelezionatiIngruppo"]) : new List<long>(); }
        set { ViewState["ListaIdClientiSelezionatiIngruppo"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        SetCulture("it"); //forzo la cultura italia
        if (!IsPostBack)
        {
            PopolaListaNewsletter();
            PopolaTipiClienti();
            PopolaDdlNazioneFiltro();
            PopolaGruppiMailing();
            RiempiListaClientiValidatiDaAnagrafica();

        }
        if (User.IsInRole("WebMaster"))
        {
            btnComprimi.Visible = true;
        }
        //else
        //{
        //    if (Request.QueryString["preview"] == "1" && !string.IsNullOrEmpty(Request.QueryString["fileId"]))
        //    {
        //        var fileId = Request.QueryString["fileId"];
        //        var fileContents = (byte[])Session["fileContents_" + fileId];
        //        var fileContentType = (string)Session["fileContentType_" + fileId];
        //        if (fileContents != null)
        //        {
        //            Response.Clear();
        //            Response.ContentType = fileContentType;
        //            Response.BinaryWrite(fileContents);
        //            Response.End();
        //        }
        //    }

        //}
    }
    protected string ImageBasePath()
    {
        string ret = "";
        ret = ReplaceAbsoluteLinks(ConfigManagement.ReadKey("ImageRoot"));
        return ret;
    }

    protected void btnCaricafile_Click(object sender, EventArgs e)
    {
        string percorsoImmagini = ConfigManagement.ReadKey("ImageRoot");
        string Nome = upFile.FileName;
        int maxdimx = 300;
        int maxdimy = 300;
        int.TryParse(resizeDimx.Text, out maxdimx);
        int.TryParse(resizeDimy.Text, out maxdimy);
        string pathCompletoFoto = CaricaFoto(upFile, percorsoImmagini, Nome, ref maxdimx, ref maxdimy);
        //alt =”Litmus” style =”font - family: Georgia; color: #697c52; font-style: italic; font-size: 30px;”
        string alttext = "Fare Click col TASTO DESTRO DEL MOUSE e selezionare SCARICA IMMAGINI. Le Immagini normalmente sono nascoste e richiedono l'autorizzazione dell'utente per essere visualizzate.";
        string htmlfotodainserire = "<img style=\"color:#697c52;font-style:italic;font-size:30px\" alt=\"" + alttext + "\" width=\"" + maxdimx + "\" height=\"" + maxdimy + "\" src=\"" + pathCompletoFoto + "\"  />";
        if (!string.IsNullOrEmpty(pathCompletoFoto))
            //htmlEdit.Content += htmlfotodainserire;
            tinyhtmlEdit.InnerText += htmlfotodainserire;

    }
    protected string CaricaFoto(FileUpload fileupload, string percorsovirtualedestinazione, string Nome, ref int maxdimx, ref int maxdimy)
    {
        string rethtml = "";
        string error = "";
        try
        {
            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            string pathDestinazione = Server.MapPath(percorsovirtualedestinazione);
            if (!Directory.Exists(pathDestinazione))
                Directory.CreateDirectory(pathDestinazione);
            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (fileupload.HasFile)
            {
                if (fileupload.PostedFile.ContentLength > 6000000)
                {

                    error += "La foto non può essere caricata perché supera 6MB!";
                }
                else
                {
                    //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
                    string NomeCorretto = fileupload.FileName.Replace("+", "");
                    NomeCorretto = NomeCorretto.Replace("%", "");
                    NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
                    NomeCorretto = NomeCorretto.Replace(" ", "-").ToLower();
                    //Se la foto è presente la cancello
                    //if (System.IO.File.Exists(pathDestinazione  + NomeCorretto))
                    //{
                    //    File.Delete(pathDestinazione + "\\" + NomeCorretto);
                    //}
                    if (fileupload.PostedFile.ContentType == "image/jpeg" || fileupload.PostedFile.ContentType == "image/pjpeg" ||   fileupload.PostedFile.ContentType == "image/png")
                    {
                        bool ridimensiona = true;
                        //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                        if (filemanage.ResizeAndSave(fileupload.PostedFile.InputStream, ref maxdimx, ref maxdimy, pathDestinazione + NomeCorretto, ridimensiona))
                        {
                            rethtml = ReplaceAbsoluteLinks(percorsovirtualedestinazione + NomeCorretto);
                        }
                        else
                        {
                            error += ("La foto non è stata caricata! (Problema nel caricamento)");

                        }
                    }
                    else if (fileupload.PostedFile.ContentType == "application/pdf")
                    {
                        //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                        //    outupload.Text = ("La foto non è stata caricata! (Formato non previsto)"); 
                        fileupload.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                        rethtml = ReplaceAbsoluteLinks(percorsovirtualedestinazione + NomeCorretto);
                    }
                    else
                    {
                        //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                        //    outupload.Text = ("La foto non è stata caricata! (Formato non previsto)"); 
                        fileupload.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                        rethtml = ReplaceAbsoluteLinks(percorsovirtualedestinazione + NomeCorretto);
                        //error += ("La foto non è stata caricata! (Formato non previsto)");
                    }

                }
            }
            else { error += "Selezionare il file da caricare"; }
        }
        catch (Exception errorecaricamento)
        {
            error += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                error += errorecaricamento.InnerException.Message;
        }
        outupload.Text = error;
        return rethtml;
    }



    protected void ddlLinguaFiltroClientiChange(object sender, EventArgs e)
    {
        RiempiListaClientiValidatiDaAnagrafica();

    }
    protected void ddlFiltraPerNazioneChange(object sender, EventArgs e)
    {
        RiempiListaClientiValidatiDaAnagrafica();

    }

    protected Cliente SettaFiltroCliente(bool Validato, bool Consenso1, string lingua, string tipocliente, string CodiceNazione, string etamin = "", string etamax = "", string sesso = "")
    {
        Cliente _clifiltro = new Cliente();
        _clifiltro.Validato = Validato;
        _clifiltro.Consenso1 = Consenso1;
        _clifiltro.CodiceNAZIONE = CodiceNazione;
        _clifiltro.Lingua = lingua;

        if (sesso != "")
        {
            if (_clifiltro == null) _clifiltro = new Cliente();
            _clifiltro.Sesso = sesso.ToLower();
        }
        if (!string.IsNullOrEmpty(txtetamin.Text) || !string.IsNullOrEmpty(txtetamax.Text))
        {
            if (_clifiltro == null) _clifiltro = new Cliente();
            int emin = 0;
            int.TryParse(etamin, out emin);
            _clifiltro.Spare2 = emin.ToString() + "|";
            int emax = 0;
            int.TryParse(etamax, out emax);
            if (emax < emin) emax = 400; //Se ho messo eta max inferiore non metto limite eta max
            if (emax == 0 && emin > 0) emax = 400; //Se eta min diversa da zero e emax è zero non metto limite eta massima
            _clifiltro.Spare2 += emax.ToString();
        }

        if (!string.IsNullOrWhiteSpace(tipocliente))
        {
            long i = 0;
            long.TryParse(tipocliente, out i);
            _clifiltro.id_tipi_clienti = i.ToString(); //filtro per tipo cliente (0 default - clienti standard altrimenti dalla tabella riferimento tipi di clienti)
        }
        else
        {
            _clifiltro.id_tipi_clienti = "";
        }

        return _clifiltro;
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
    protected void RiempiListaClientiValidatiDaAnagrafica()
    {
        //Carichiamo i clienti destinatari usando un modello cliente per il filtraggio dei clienti ( prendo i clienti Validati, con consenso commrciale opzione 1, con la lingua indicata )
        ClientiDM cliDM = new ClientiDM();
        Cliente _clifiltro = SettaFiltroCliente(true, true, ddlLinguaFiltroClienti.SelectedValue, ddlTipiClienti.SelectedValue, ddlNazioniFiltro.SelectedValue, txtetamin.Text, txtetamax.Text, radSessoRicerca.SelectedValue);
        ClienteCollection clientifiltro = CaricaClientiFiltrati(_clifiltro);

        PopolaClientiListaAnagrafica(clientifiltro);
    }
    private void PopolaClientiListaAnagrafica(ClienteCollection clientianagrafica)
    {
        if (chkCaricamentolista.Checked == false) return;
        if (clientianagrafica != null)
            //  clientinelgruppo.Sort(new GenericComparer<Cliente>("Email", System.ComponentModel.ListSortDirection.Ascending));
            clientianagrafica.Sort(new GenericComparer2<Cliente>("Cognome", System.ComponentModel.ListSortDirection.Ascending, "Nome", System.ComponentModel.ListSortDirection.Ascending));

        ListItemCollection listcoll = new ListItemCollection();
        ListItem item = null;
        ListaIdClientiSelezionatiInanagrafica = new List<long>();

        if (clientianagrafica != null)
            foreach (Cliente c in clientianagrafica)
            {
                string testocliente = c.Cognome + " " + c.Nome + " : " + c.Email;
                item = new ListItem(testocliente, c.Id_cliente.ToString());
                listcoll.Add(item);
                ListaIdClientiSelezionatiInanagrafica.Add(c.Id_cliente);//Popolo la lista dei clienti selezionati
            }
        //string[] controls = { "LinqDataSource", "Button", "ListView", "BulletedList", "EntityDataSource" };

        listAnagraficaClienti.DataTextField = "Text";
        listAnagraficaClienti.DataValueField = "Value";
        listAnagraficaClienti.DataSource = listcoll;
        listAnagraficaClienti.DataBind();
    }
    /// <summary>
    /// Popola la lista dei clienti nel gruppo di mailing selezionato
    /// </summary>
    /// <param name="clientinelgruppo"></param>
    private void PopolaClientiNelGruppoMailing(ClienteCollection clientinelgruppo)
    {
        if (clientinelgruppo != null)
            //  clientinelgruppo.Sort(new GenericComparer<Cliente>("Email", System.ComponentModel.ListSortDirection.Ascending));
            clientinelgruppo.Sort(new GenericComparer2<Cliente>("Cognome", System.ComponentModel.ListSortDirection.Ascending, "Nome", System.ComponentModel.ListSortDirection.Ascending));

        ListItemCollection listcoll = new ListItemCollection();
        ListItem item = null;
        ListaIdClientiSelezionatiIngruppo = new List<long>();
        if (clientinelgruppo != null)
            foreach (Cliente c in clientinelgruppo)
            {
                string testocliente = c.Cognome + " " + c.Nome + " : " + c.Email;
                item = new ListItem(testocliente, c.Id_cliente.ToString());
                listcoll.Add(item);
                ListaIdClientiSelezionatiIngruppo.Add(c.Id_cliente);
            }
        //string[] controls = { "LinqDataSource", "Button", "ListView", "BulletedList", "EntityDataSource" };
        listClientiNelgruppo.DataTextField = "Text";
        listClientiNelgruppo.DataValueField = "Value";
        listClientiNelgruppo.DataSource = listcoll;
        listClientiNelgruppo.DataBind();

    }
    private void PopolaGruppiMailing(long idselected = -1)
    {
        mailingDM mDM = new mailingDM();
        //Carichiamo la lista dei gruppi clienti!!!
        TabrifCollection gruppi = new TabrifCollection();
        gruppi = mDM.CaricaGruppiClientiNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        ListItemCollection listcoll = new ListItemCollection();
        ListItem item = null;
        if (gruppi != null)
            foreach (Tabrif m in gruppi)
            {
                item = new ListItem(m.Campo1, m.Intero1.ToString());
                listcoll.Add(item);
            }
        //string[] controls = { "LinqDataSource", "Button", "ListView", "BulletedList", "EntityDataSource" };
        listGruppi.DataTextField = "Text";
        listGruppi.DataValueField = "Value";
        listGruppi.DataSource = listcoll;
        listGruppi.DataBind();
        if (idselected != -1)
        {
            try
            {
                listGruppi.SelectedValue = idselected.ToString();
            }
            catch { }
        }
    }
    protected void CreaNuovoGruppo(object sender, EventArgs e)
    {
        mailingDM mDM = new mailingDM();

        Tabrif nuovogruppo = new Tabrif();
        nuovogruppo.Id = "";
        nuovogruppo.Intero1 = 1;
        //Calcoliamo l'iltimo progessivo gruppo disponibile
        TabrifCollection gruppi = mDM.CaricaGruppiClientiNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        //Vediamo l'ultimo progressivo
        if (gruppi != null)
        {
            gruppi.Sort(new GenericComparer<Tabrif>("Intero1", System.ComponentModel.ListSortDirection.Descending));
            nuovogruppo.Intero1 = gruppi[0].Intero1 + 1; //Nuovo indice di gruppo
        }
        nuovogruppo.Campo1 = txtNomeGruppo.Text;
        nuovogruppo.Data1 = System.DateTime.Now;
        nuovogruppo.Intero2 = 0;

        //Creiamo il nuovo gruppo clienti
        long idgruppo = mDM.InserisciAggiornaNuovoGruppoClientiNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, nuovogruppo);
        PopolaGruppiMailing(idgruppo);
    }


    protected void ModificaGruppo(object sender, EventArgs e)
    {
        mailingDM mDM = new mailingDM();

        Tabrif grupposelezionato = new Tabrif();
        if (listGruppi.SelectedItem != null)
        {
            string selvalue = listGruppi.SelectedItem.Value;
            long id = 0;
            long.TryParse(selvalue, out id);
            grupposelezionato = mDM.CaricaGruppoMailing(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
            grupposelezionato.Campo1 = txtNomeGruppo.Text;
            //  grupposelezionato.Intero2 = 0;

            //Aggiorniamo il nuovo gruppo clienti
            mDM.InserisciAggiornaNuovoGruppoClientiNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, grupposelezionato);
            PopolaGruppiMailing(id);
        }
    }

    /// <summary>
    /// Selezione in listbox clienti nel gruppo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void listClientiNelgruppoitemchange(object sender, EventArgs e)
    {
        string seltext = ((ListBox)sender).SelectedItem.Text;
        string selvalue = ((ListBox)sender).SelectedItem.Value;
    }
    /// <summary>
    /// Selezione in listbox cleinti in anagrafica
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void listAnagraficaClientiitemchange(object sender, EventArgs e)
    {
        string seltext = ((ListBox)sender).SelectedItem.Text;
        string selvalue = ((ListBox)sender).SelectedItem.Value;
    }

    protected void listgruppiitemchange(object sender, EventArgs e)
    {
        string seltext = ((ListBox)sender).SelectedItem.Text;
        string selvalue = ((ListBox)sender).SelectedItem.Value;

        VisualizzaGruppoMailing(selvalue);
    }
    private void VisualizzaGruppoMailing(string selvalue)
    {
        mailingDM mDM = new mailingDM();
        long id = 0;
        long.TryParse(selvalue, out id);
        //Prima carico la lista clienti nel gruppo
        //-> selezione gruppo clienti
        // TabrifCollection clientinelgruppo = mDM.CaricaClientiNewsletterPerGruppo(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
        ClientiDM cliDM = new ClientiDM();
        ClienteCollection clientinelgruppo = cliDM.CaricaClientiPerGruppoNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
        PopolaClientiNelGruppoMailing(clientinelgruppo);

        int totaleclienti = 0;
        if (clientinelgruppo != null)
            totaleclienti = clientinelgruppo.Count;
        litClientiIngruppo.Text = "Totale clienti nel gruppo : " + totaleclienti;

        //Poi carico i dati di dettaglio per il gruppo selezionato
        Tabrif gruppomailing = mDM.CaricaGruppoMailing(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
        //Visualizziamo quanto caricato
        if (gruppomailing != null)
        {
            txtNomeGruppo.Text = gruppomailing.Campo1;
        }
    }

    // listClientiNelgruppo
    protected void AggiungiTuttiAGruppo(object sender, EventArgs e)
    {
        if (listGruppi.SelectedItem != null)
        {
            //Devo aggiungere tutti i clienti al gruppo selezionato nella lsitabox
            //dal filtro anagrafico nella listAnagraficaClienti
            string selidgruppovalue = listGruppi.SelectedItem.Value;
            long idgruppo = 0;
            long.TryParse(selidgruppovalue, out idgruppo);
            mailingDM mDM = new mailingDM();

            Cliente _clifiltro = SettaFiltroCliente(true, true, ddlLinguaFiltroClienti.SelectedValue, ddlTipiClienti.SelectedValue, ddlNazioniFiltro.SelectedValue, txtetamin.Text, txtetamax.Text, radSessoRicerca.SelectedValue);
            mDM.AggiungiClientiAGruppo(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _clifiltro, idgruppo);

            VisualizzaGruppoMailing(selidgruppovalue);

        }

    }
    protected void EliminaTuttiDaGruppo(object sender, EventArgs e)
    {

        if (listGruppi.SelectedItem != null)
        {
            //idgrupponewsletter
            string selidgruppovalue = listGruppi.SelectedItem.Value;
            long idgruppo = 0;
            long.TryParse(selidgruppovalue, out idgruppo);

            mailingDM mDM = new mailingDM();

            // Cliente _clifiltro = SettaFiltroCliente(true, true, ddlLinguaFiltroClienti.SelectedValue, ddlTipiClienti.SelectedValue);

            mDM.EliminaClientiDaGruppo(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idgruppo);

            VisualizzaGruppoMailing(selidgruppovalue);
        }


    }
    protected void AggiungiAGruppo(object sender, EventArgs e)
    {
        if (listAnagraficaClienti.SelectedItem != null && listGruppi.SelectedItem != null)
        {
            //Devo aggiungere il cliete dal gruppo selezionato nella lsitabox

            //idcliente
            string selidclientevalue = listAnagraficaClienti.SelectedItem.Value;
            //idgrupponewsletter
            string selidgruppovalue = listGruppi.SelectedItem.Value;

            long idcliente = 0;
            long idgruppo = 0;
            long.TryParse(selidclientevalue, out idcliente);
            long.TryParse(selidgruppovalue, out idgruppo);

            mailingDM mDM = new mailingDM();
            mDM.AggiungiClienteAGruppo(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcliente, idgruppo);

            VisualizzaGruppoMailing(selidgruppovalue);

        }
    }
    protected void EliminaDaGruppo(object sender, EventArgs e)
    {
        if (listClientiNelgruppo.SelectedItem != null && listGruppi.SelectedItem != null)
        {
            //Devo rimuovere da gruppo selezionato il cliente al gruppo selezionato in listbox

            //idcliente
            string selidclientevalue = listClientiNelgruppo.SelectedItem.Value;
            //idgrupponewsletter
            string selidgruppovalue = listGruppi.SelectedItem.Value;
            long idcliente = 0;
            long idgruppo = 0;
            long.TryParse(selidclientevalue, out idcliente);
            long.TryParse(selidgruppovalue, out idgruppo);

            mailingDM mDM = new mailingDM();
            mDM.EliminaClienteDaGruppo(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcliente, idgruppo);

            VisualizzaGruppoMailing(selidgruppovalue);
        }
    }

    protected void listitemchange(object sender, EventArgs e)
    {
        string seltext = ((ListBox)sender).SelectedItem.Text;
        string selvalue = ((ListBox)sender).SelectedItem.Value;
        //Carichiamo i dati della newsletter ed associamoli ai controlli
        VisualizzaNewsletter(selvalue);
        litId.Text = selvalue;
        litLink.Text = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti_adesione.aspx?ID_cliente=&ID_mail=&Lingua=" + ddlLingua.SelectedValue + "&idNewsletter=" + selvalue;
    }

    private void PopolaTipiClienti()
    {
        //ClientiDM cliDM = new ClientiDM();
        //TabrifCollection tipiclienti = cliDM.CaricaTipiClienti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        //ddlTipiClienti.Items.Clear();
        //ddlTipiClienti.DataTextField = "Campo1";
        //ddlTipiClienti.DataValueField = "Codice";
        //ddlTipiClienti.DataSource = tipiclienti;
        //ddlTipiClienti.DataBind();
        //ddlTipiClienti.SelectedValue = "0";

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
            ddlTipiClienti.SelectedValue = "0";
        }
        catch { }

    }

    protected void TipoClienteChange(object sender, EventArgs e)
    {
        RiempiListaClientiValidatiDaAnagrafica();
    }

    private void VisualizzaNewsletter(string selvalue)
    {
        mailingDM mDM = new mailingDM();
        long id = 0;
        long.TryParse(selvalue, out id);
        Mail newsletter = mDM.CaricaNewsletterPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);

        //Visualizziamo quanto caricato
        if (newsletter != null)
        {
            txtSoggetto.Text = newsletter.SoggettoMail;

            txtInvito.Text = ""; //Non è salvato nei campi mail

            txtAdesione.Text = newsletter.NoteInvio;
            ddlLingua.SelectedValue = newsletter.Lingua;

            string contenutomail = newsletter.TestoMail;
            //<div id=\"divinziocontenuti\"></div>
            try
            {
                contenutomail = contenutomail.Substring(contenutomail.IndexOf("<div id=\"divinziocontenuti\"></div>") + 34);
                //<div id=\"divfinecontenuti\"></div>
                contenutomail = contenutomail.Remove(contenutomail.IndexOf("<div id=\"divfinecontenuti\"></div>"));
            }
            catch { }
            try
            {
                //htmlEdit.Content = contenutomail;
                tinyhtmlEdit.InnerText = contenutomail;

            }
            catch (Exception err) { output.Text = err.Message; }
            //  Content.Value = contenutomail;
        }
    }
    private void SvuotaNewsletter()
    {
        PopolaListaNewsletter();
        txtSoggetto.Text = "";
        txtInvito.Text = ""; //Non è salvato nei campi mail
        txtAdesione.Text = "Compila i dati richiesti per l'adesione all'offerta.";
        ddlLingua.SelectedValue = "I";

        htmlEdit.Content = "";
        tinyhtmlEdit.InnerText = "";

        //Content.Value = "";
        txtContent.Text = "";
    }

    private void PopolaListaNewsletter(long idselected = -1)
    {
        //Carichiamo dal db la lista delle strutture nesletter ... e bindiamo gli id alla lista
        mailingDM mDM = new mailingDM();
        MailCollection newsletters = mDM.CaricaListaNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        ListItemCollection listcoll = new ListItemCollection();
        ListItem item = null;
        if (newsletters != null)
            foreach (Mail m in newsletters)
            {
                item = new ListItem(m.SoggettoMail, m.Id.ToString());
                listcoll.Add(item);
            }

        //string[] controls = { "LinqDataSource", "Button", "ListView", "BulletedList", "EntityDataSource" };
        listNewsLetter.DataTextField = "Text";
        listNewsLetter.DataValueField = "Value";
        listNewsLetter.DataSource = listcoll;
        listNewsLetter.DataBind();
        if (idselected != -1)
        {
            listNewsLetter.SelectedValue = idselected.ToString();
            litId.Text = idselected.ToString();
            string link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti_adesione.aspx?ID_cliente=&ID_mail=&Lingua=" + ddlLingua.SelectedValue + "&idNewsletter=" + idselected.ToString();
            litLink.Text = link;
        }
    }
    protected void linguachange(object sender, EventArgs e)
    {
        string idstring = "-1";
        if (listNewsLetter.SelectedItem != null)
            idstring = listNewsLetter.SelectedItem.Value;
        long id = 0;
        long.TryParse(idstring, out id);
        PopolaListaNewsletter(id);
    }
    protected void Nuova(object sender, EventArgs e)
    {
        SvuotaNewsletter();
    }


    protected void Save(object sender, EventArgs e)
    {
        try
        {
            mailingDM mDM = new mailingDM();

            //Prepariamo la newsletter
            string htmlNewsletter = "";

            htmlNewsletter += "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            //INSERIMENTO TAG PER HEADER MAIL
            htmlNewsletter += "<html  xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><title>Newsletter</title></head>";
            htmlNewsletter += "<body yahoo=fix scroll=\"auto\"  style=\"margin:0;padding:0;FONT-SIZE: 12px; FONT-FAMILY: Arial, Helvetica, sans-serif; cursor:auto;\">";
            htmlNewsletter += "<table border=\"0\" cellpadding=\"0\" align=\"center\" cellspacing=\"0\" width=\"900\" style=\"\"> <tr><td>";
            htmlNewsletter += "<div id=\"divinziocontenuti\"></div>";

            //htmlNewsletter += htmlEdit.Content; // Content.Value;//  
            htmlNewsletter += tinyhtmlEdit.InnerText; // Content.Value;//  

            //Convertiamo i riferimenti relativi in assoluti per le immagini se non già convertiti precedentemente
            // string _tmp = htmlNewsletter;
            int i = 0;
            i = htmlNewsletter.ToLower().IndexOf("src=\"", i);
            while (i != -1)
            {
                if (htmlNewsletter.ToLower().Substring(i).StartsWith("src=\"" + HttpContext.Current.Request.ApplicationPath.ToString()) && HttpContext.Current.Request.ApplicationPath.ToString() != "/")
                    htmlNewsletter = base.ReplaceFirstOccurrence(htmlNewsletter, "src=\"" + HttpContext.Current.Request.ApplicationPath.ToString(), "src=\"" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, i);
                else if (!htmlNewsletter.ToLower().Substring(i).ToLower().StartsWith("src=\"http:/") && !htmlNewsletter.ToLower().Substring(i).ToLower().StartsWith("src=\"https:/") && !htmlNewsletter.ToLower().Substring(i).ToLower().StartsWith("src=\"|"))
                {
                    if (!htmlNewsletter.Substring(i + 5).ToLower().StartsWith("/"))
                        htmlNewsletter = base.ReplaceFirstOccurrence(htmlNewsletter, "src=\"", "src=\"" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/", i);
                    else
                        htmlNewsletter = base.ReplaceFirstOccurrence(htmlNewsletter, "src=\"", "src=\"" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, i);
                }
                i = i + 5;
                i = htmlNewsletter.ToLower().IndexOf("src=\"", i);
            }

            //cerco le img ripartendo da 0 -> cerso style e poi width e al temine aggiunfo border="0" width="valore"
#if true
            i = 0;
            i = htmlNewsletter.ToLower().IndexOf("<img", i);
            while (i != -1)
            {
                int j = 0;
                j = htmlNewsletter.ToLower().IndexOf("src=", i);
                int z = 0;
                z = htmlNewsletter.ToLower().IndexOf("/>", i);
                int x = 0;
                x = htmlNewsletter.ToLower().IndexOf("border=", i);
                if (j < z)
                    if (x == -1 && x < z) //Controllo di non avere già inserito border e width per l'immagine
                    {
                        i = htmlNewsletter.IndexOf("/>", i);
                        //htmlNewsletter = htmlNewsletter.Insert(i, "border=\"0\" width=\"" + width + "\"");
                        htmlNewsletter = htmlNewsletter.Insert(i, "border=\"0\" ");

                    }
                i = i + 10;
                i = htmlNewsletter.IndexOf("<img", i);
            }

#endif
#if false
        i = 0;
        int j = 0;
        i = htmlNewsletter.IndexOf("<img", i);
        if (i != -1)
            i = htmlNewsletter.IndexOf("style=\"", i);
        while (i != -1)
        {
            string width = "";
            if (htmlNewsletter.ToLower().IndexOf("width=", i) == -1) //Controllo di non avere già inserito border e width per l'immagine
            {
                i = htmlNewsletter.ToLower().IndexOf("width:", i);
                j = htmlNewsletter.ToLower().IndexOf("px", i);
                if (i != -1 && j != -1)
                {
                    width = htmlNewsletter.Substring(i + 6, j - (i + 6)).Trim();
                    i = htmlNewsletter.IndexOf("/>", i);
                    htmlNewsletter = htmlNewsletter.Insert(i, "border=\"0\" width=\"" + width + "\"");
                }
            }
            i = i + 7;
            i = htmlNewsletter.IndexOf("<img", i);
            if (i != -1)
                i = htmlNewsletter.IndexOf("style=\"", i);
        }
        
#endif

            Mail newsletter = new Mail();
            newsletter.Tipomailing = (long)enumclass.TipoMailing.Newsletter_tipo1; //Specifico il tipo della mail per lo smistatore
            newsletter.NoteInvio = txtAdesione.Text;
            //Testi indroduttivi soggetto e mail ( da prendere dal form attuale )
            newsletter.SoggettoMail = txtSoggetto.Text; // da chidere all'utente mediante apposita box
            newsletter.Lingua = ddlLingua.SelectedValue; //Lingua della newsletter da prednere da apposita casella di selezione utente
            //DateTime _tmpdate = System.DateTime.Now;
            //DateTime.TryParseExact(txtData.Text, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _tmpdate);
            //item.DataInserimento = _tmpdate;

            //Imposto l'id della newsletter selezionata se presente
            long id = 0;
            if (listNewsLetter.SelectedItem != null)
            {
                string selvalue = listNewsLetter.SelectedItem.Value;
                long.TryParse(selvalue, out id);
            }
            newsletter.Id = id;
            long identity = mDM.InserisciAggiornaNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, newsletter);
            if (newsletter.Id == 0)
                newsletter.Id = identity;
            if (newsletter.Id != 0)
            {
                //INSERIMENTO TAG DI CHIUSURA DELLA MAIL    
                htmlNewsletter += "<div id=\"divfinecontenuti\"></div>";

                if (!string.IsNullOrWhiteSpace(txtInvito.Text.Trim())) //Se specificato un testo nel link di adesione -> inserisco il link
                {
                    //Mettiamo anche il link alla pagina specifica dell'offerta appena inserita ( qui devo rimandare al form di iscrizione con i param. di contatto )
                    string link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/Iscriviti_adesione.aspx?ID_cliente=&ID_mail=&Lingua=" + newsletter.Lingua + "&idNewsletter=" + newsletter.Id;
                    htmlNewsletter += "<br/><a href=\"" + link + "\" target=\"_blank\" style=\"font-size:18px;color:#b13c4e\">" + txtInvito.Text + "</a><br/>";
                }

                htmlNewsletter += "</td></tr></table></body></html>";
                newsletter.TestoMail = htmlNewsletter;
                mDM.InserisciAggiornaNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, newsletter);

                PopolaListaNewsletter(newsletter.Id);//Ripopolo la listbox aggiungendo la nuova newsletter
            }
            else
            {
                output.Text = "Errore durante inserimento newsletter.";
            }
            //item.CodiceContenuto = CodiceContenuto;
            //DateTime _tmpdate = System.DateTime.Now;
        
            // DateTime.TryParseExact(txtData.Text, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _tmpdate);
            //item.DataInserimento = _tmpdate;
            //int _i = 0;
            //int.TryParse(ddlStruttura.SelectedValue, out _i);
            //updrecord.Id_attivita = _i;
        }
        catch (Exception err)
        {
            output.Text = "Errore salvataggio: " + err.Message;
        }
    }

    protected void Cancella(object sender, EventArgs e)
    {
        mailingDM mDM = new mailingDM();
        //Imposto l'id della newsletter selezionata se presente
        long id = 0;
        if (listNewsLetter.SelectedItem != null)
        {
            string selvalue = listNewsLetter.SelectedItem.Value;
            long.TryParse(selvalue, out id);
        }
        mDM.CancellaNewsletterPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
        SvuotaNewsletter();
    }

    /// <summary>
    /// Prepara le mail per i clienti del gruppo selezionato
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void IncrociaGruppo(object sender, EventArgs e)
    {
        mailingDM mDM = new mailingDM();
        //Imposto l'id e carico la newsletter selezionata se presente
        long id = 0;
        if (listNewsLetter.SelectedItem != null)
        {
            string selvalue = listNewsLetter.SelectedItem.Value;
            long.TryParse(selvalue, out id);
        }
        Mail newsletter = mDM.CaricaNewsletterPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
        //Inseriamo le email nella tabella per l'invio
        if (newsletter != null)
        {
            //Carichiamo i clienti destinatari in base al gruppo selezionato
            if (listGruppi.SelectedItem != null)
            {
                ClientiDM cliDM = new ClientiDM();
                string selidgruppovalue = listGruppi.SelectedItem.Value;
                id = 0;
                long.TryParse(selidgruppovalue, out id);
                ClienteCollection clientifiltro = cliDM.CaricaClientiPerGruppoNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
                //Togliamo i clienti non validati e senza consenso per promozioni commerciali per sicurezza
                if (clientifiltro != null)
                {
                    clientifiltro.RemoveAll(delegate (Cliente c) { return c.Validato == false || c.Consenso1 == false; });

                    PreparaMailPerInvio(newsletter, clientifiltro);
                }
            }
        }
        else Message.Text = "Selezionare Newsletter per l'incrocio";
    }

    /// <summary>
    /// Crea le mail per i clienti selezionando quelli di lingua ugiale alla newsletter e dei tipologia slezionata nella lista a discesa ddltipiclienti
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Incrocia(object sender, EventArgs e)
    {
        mailingDM mDM = new mailingDM();
        //Imposto l'id della newsletter selezionata se presente
        long id = 0;
        if (listNewsLetter.SelectedItem != null)
        {
            string selvalue = listNewsLetter.SelectedItem.Value;
            long.TryParse(selvalue, out id);
        }
        Mail newsletter = mDM.CaricaNewsletterPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);

        //Inseriamo le email nella tabella per l'invio
        if (newsletter != null)
        {
            //------------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------------------------------------------
            //Qui resta da inserire un filtro clienti e ad un interfaccia utente per il raggruppamento di questi
            //USANDO la tabella di ragguppamento clienti/mailing TBL_MAILING_GRUPPI_CLIENTI 
            //che dovrà avere un suo pannello di gestione per la creazione dai raggruppamenti di clienti ..... da fare
            //tale creazione di gruppi si basa su caratteristiche del cliente quali: posizione geografica, campo rif. tipocliente (TBLRIF_Tipi_CLienti ) o altro .. da fare
            //-------------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------------------------------------------
            //Carichiamo i clienti destinatari usando un modello cliente per il filtraggio dei clienti ( prendo i clienti Validati, con consenso commrciale opzione 1, con la lingua indicata )
            ClientiDM cliDM = new ClientiDM();
            Cliente _clifiltro = SettaFiltroCliente(true, true, newsletter.Lingua, ddlTipiClienti.SelectedValue, ddlNazioniFiltro.SelectedValue, txtetamin.Text, txtetamax.Text, radSessoRicerca.SelectedValue);
            ClienteCollection clientifiltro = CaricaClientiFiltrati(_clifiltro);
            //-------------------------------------------------------------------------------------------------------------------------------------

            PreparaMailPerInvio(newsletter, clientifiltro);
        }
        else Message.Text = "Selezionare Newsletter per l'incrocio";
    }

    private ClienteCollection CaricaClientiFiltrati(Cliente _clifiltro)
    {
        //-------------------------------------------------------------------------------------------------------------------------------------
        //Carichiamo i clienti destinatari usando un modello cliente per il filtraggio dei clienti ( prendo i clienti Validati, con consenso commrciale opzione 1, con la lingua indicata )
        ClientiDM cliDM = new ClientiDM();
        ClienteCollection clientifiltro = cliDM.CaricaClientiFiltrati(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, _clifiltro);
        //-------------------------------------------------------------------------------------------------------------------------------------
        return clientifiltro;
    }

    private void PreparaMailPerInvio(Mail newsletter, ClienteCollection clientifiltro)
    {
        //--- ------------------------------------------------------------------------------------------------------
        //Eliminiamo dalla lista clienti passata quelli che hanno già ricevuto la newsletter in questione ( uso l'id del cliente )
        // usiamo il  newsletter.Id cercando tra le mail inviate ai vari cleinti le corrispondenze con il campo id_mailing_struttura
        //confrontando la newsletter attuale con quelle presenti nella TBL_MAILING relative alle mail inviate e da inviare !!!
        // ( devo matchare solo le mail che hanno datainvio != null e errore = false )
        //1. select in tbl_mailing delle mail con  errore = false , id_mailing_struttura = newsletter.id ( sono quelle inviate o da inviare non in errore corrispondenti alla neslwtter attuale ) 
        //   ( volendo per le email tra queste con data adesione vuota dovrei eliminare dalla lista quelle inviate da molto tempo in quanto senza convalida di adesione !! )
        //   -> fare in mailinDM funzione carica mail per id newsletter con parametri errore = false eventualmente con filtro datainserimento > dataminima
        //2. elimino da clientifiltro quelli che hanno un id nella lista delle mail al punto precedenti ( sono quelli che hanno già ricevuto la mailing 
        //per i quali non effettuo una nuova preparazione della stessa mail !!!)
        mailingDM mDM = new mailingDM();
        MailCollection mailvalide = mDM.CaricaMailPeridnewsletterValide(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, newsletter.Id, System.DateTime.Now.AddDays(-30));

        if (!chkForzaInvio.Checked)
        {
            //eliminiamo da clientifiltro
            foreach (Mail m in mailvalide)
                clientifiltro.RemoveAll(delegate (Cliente _c) { return _c.Id_cliente == m.Id_cliente; });
            //-------------------------------------------------------------------------------------------------------------------------------------
        }

        //Inseriamo la richiesta di mail per tutti i clienti filtrati
        if (newsletter != null)
            InserisciMailPerNewsletter(newsletter, clientifiltro);
    }

    /// <summary>
    /// Prepara la richiesta di invio mail per il sistema di smistamento delle email
    /// </summary>
    /// <param name="item"></param>
    private void InserisciMailPerNewsletter(Mail mail, ClienteCollection clientimail)
    {
        try
        {
            long id_newsletter = mail.Id;
            long totalemailpreparate = 0;
            litMails.Text = "Totale mail preparate: " + "0";
            if (clientimail != null)
            {
                mailingDM mDM = new mailingDM();


                //ALTERNATIVA CON INSERT UNICO NEL DB
                mail.DataInserimento = System.DateTime.Now;
                mail.Id_mailing_struttura = id_newsletter;//L'id della newsletter originale passata è quello di riferimento per l'inserimento della mail
                mail.Id = 0;//Inserisco sempre una nuova riga di mail ad ogni salvataggio ( attenzione potrebbe mandare mail multiple allo stesso cliente )!!!

                #region VARIAZIONE DEL MITTENTE DEL MAILING ATTUALE
                //Variazione del mittente standard della mail
                if (!string.IsNullOrWhiteSpace(txtEmailMittente.Text))
                {
                    string emailmittente = txtEmailMittente.Text;
                    string nomemittente = txtNomeMittente.Text;
                    if (emailmittente.Contains("@"))
                    {
                        emailmittente = emailmittente.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                        string testodaaggiungere = "|" + emailmittente + "|";
                        if (!string.IsNullOrWhiteSpace(nomemittente))
                            testodaaggiungere += nomemittente + "|";
                        else
                            testodaaggiungere += "|";
                        mail.NoteInvio = mail.NoteInvio.Insert(0, testodaaggiungere);//Aggiungo in testo la mail e il nome del mittente alle note di invio
                    }
                }
                #endregion

                mDM.InserisciBloccoMailPerClienti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, mail, clientimail); //Inseriamo nel db la mail pronta per l'invio al cliente specifico
                totalemailpreparate = clientimail.Count;
            }
            litMails.Text = "Totale mail preparate: " + totalemailpreparate.ToString();

        }
        catch (Exception error)
        {
            output.Text = error.Message;
            if (error.InnerException != null)
                output.Text += error.InnerException.Message.ToString();
        }
    }

    protected void Esegui(object sender, EventArgs e)
    {
        EseguiMailing();
    }
    protected void CancellaMail(object sender, EventArgs e)
    {
        mailingDM mDM = new mailingDM();
        mDM.CancellaMailInAttesa(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
        outputmailing.Text = "Eliminato mail in attesa di invio con data inferiore ad oggi";
    }
    protected void SvuotaMail(object sender, EventArgs e)
    {
        mailingDM mDM = new mailingDM();
        int giornipercancellazione = 0;
        mDM.CancellaMailPerPulizia(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, System.DateTime.Now.AddDays(-giornipercancellazione));
        outputmailing.Text = "Svuotato tabella mail";
    }

    #region PARTE RELATIVA ALLO SMISTAMENTO DELLE EMAIL ( DA SPOSTARE NELLA PAGINA DI ESECUZIONE EXECUTETASKS.ASPX

    private int maxinviiperchiamata = 1000; //Numero max di invii per chiamata del metodo di esecuzione del mailing
    private int millisecondbetweenmails = 500; //Numero max di invi per chiamata del metodo di esecuzione del mailing
    /// <summary>
    /// Smistatore delle email ne prende un tot alla volta e in base al tipo le invia
    /// </summary>
    private void EseguiMailing()
    {
        //Creo una variabile per la scrittura dei messaggi nel file di log
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");
        try
        {
            mailingDM mDM = new mailingDM();

            #region PULIZIA TABELLE SISTEMA DI MAILING
            //Pulizia tabella mail prese in carico ( dopo tre ore di attesa
            int oreattesapreseincarico = 3;
            mDM.PulisciTabellaMailPreseincarico(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, System.DateTime.Now.AddHours(-oreattesapreseincarico));
            //Per prima cosa pulisco la tabella di mailing dalle email + vecchie di una certa data
            int giornipercancellazione = 500; //Numero di giorni dopo cui le mail sono ripulite
            mDM.CancellaMailPerPulizia(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, System.DateTime.Now.AddDays(-giornipercancellazione));
            #endregion

            int countermail = 0;
            //Leggiamo le mailda inviare a blocchi ed eeguiamo gli invii
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
    /// Invio della mail
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

            //INSERISCO IL LINK PER L'UNSUBSCRIBE DALLA NEWSLETTER
            string linkUnsubscribe = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/Aspnetpages/executetasks.aspx?Azione=unsubscribe&idCliente=" + m.Id_cliente + "&Lingua=" + m.Lingua;

            //Imposto manualmente la cultura per prendere la giusta descrizione del link unsubscribe
            string culturename = "it";
            if (m.Lingua != "I")
                culturename = "en";
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culturename);
            //string value = HttpContext.GetGlobalResourceObject("CommonBase", "TestoUnsubscribe", ci).ToString();

            string value = references.ResMan("CommonBase", m.Lingua, "TestoUnsubscribe");

            //Devo prendere la risorsa per la lingua in base a m.lingua non alla lungua di visualizzazione della pagina
            if (Descrizione.IndexOf("</td></tr></table></body></html>") != -1)
                Descrizione = Descrizione.Insert(Descrizione.IndexOf("</td></tr></table></body></html>"), "<br/><a href=\"" + linkUnsubscribe + "\" target=\"_blank\" style=\"font-size:13px;color:#909090\">" + value + "</a><br/>");
            else
                Descrizione += "<br/><a href=\"" + linkUnsubscribe + "\" target=\"_blank\" style=\"font-size:13px;color:#909090\">" + value + "</a><br/>";

            Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente, null, "", true, Server);
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

    protected void btnFiltroClienti_Click(object sender, EventArgs e)
    {
        RiempiListaClientiValidatiDaAnagrafica();
    }

    protected void ComprimiDatabase(object sender, EventArgs e)
    {
        //SE VUOI ANCHE CANCELLARE I RECORD DELLA TABELLA MAIL ABILITA QUESTE
        //mailingDM mDM = new mailingDM();
        //int giornipercancellazione = 0;
        //mDM.CancellaMailPerPulizia(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, System.DateTime.Now.AddDays(-giornipercancellazione));

        output.Text = dbDataAccess.comprimiSQLiteDB(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);

    }
}