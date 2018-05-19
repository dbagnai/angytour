using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;
using System.Drawing.Imaging;

public partial class AreaContenuti_GestioneGalleria : CommonPage
{

    public string NomeFotoSelezionata
    {
        get { return ViewState["NomeFotoSelezionata"] != null ? (string)(ViewState["NomeFotoSelezionata"]) : ""; }
        set { ViewState["NomeFotoSelezionata"] = value; }
    }
    public string PercorsoComune
    {
        get { return ViewState["PercorsoComune"] != null ? (string)(ViewState["PercorsoComune"]) : ""; }
        set { ViewState["PercorsoComune"] = value; }
    }
    public string PercorsoFiles
    {
        get { return ViewState["PercorsoFiles"] != null ? (string)(ViewState["PercorsoFiles"]) : "~/public/Gallery"; }
        set { ViewState["PercorsoFiles"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            this.CaricaDati();
        }
    }

    //Ricarica la lista delle foto
    private void CaricaDati()
    {
        string pathFoto = Server.MapPath(PercorsoFiles);
        if (!Directory.Exists(pathFoto))
            Directory.CreateDirectory(pathFoto);

        string[] files = Directory.GetFiles(pathFoto);
        Dictionary<int, string> bindlist = new Dictionary<int, string>();
        int i = 0;
        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            if (!fi.Name.ToLower().StartsWith("ant"))
                bindlist.Add(i, fi.Name);
            i++;
        }

        rptFoto.DataSource = bindlist;
        rptFoto.DataBind();
    }

    #region Gestione Foto allegate
    protected void btnCarica_Click(object sender, EventArgs e)
    {
        try
        {

            string pathDestinazione = Server.MapPath(PercorsoFiles);
            if (!Directory.Exists(pathDestinazione))
                Directory.CreateDirectory(pathDestinazione);

            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (UploadFoto.HasFile)
            {
                if (UploadFoto.PostedFile.ContentLength > 6000000)
                {
                    output.Text = "La foto non può essere caricata perché supera 6MB!";
                }
                else
                {
                    string NomeCorretto = LeggiNomeFotoCorrettoPerUpload();
                    //string NomeCorretto = Server.HtmlEncode(FotoUpload1.FileName);
                    if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto))
                    {
                        output.Text = ("La foto non può essere caricata perché già presente sul server!");
                    }
                    else
                    {
                        //Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        if (UploadFoto.PostedFile.ContentType == "image/jpeg" || UploadFoto.PostedFile.ContentType == "image/pjpeg" || UploadFoto.PostedFile.ContentType == "image/gif" || UploadFoto.PostedFile.ContentType == "image/bmp")
                        {
                            int maxheight = 700;
                            int maxwidth = 900;
                            bool ridimensiona = true;
                            //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                            if (filemanage.ResizeAndSave(UploadFoto.PostedFile.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                if (!filemanage.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto))
                                    output.Text = ("Anteprima Allegato non salvata correttamente!");
                                //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
                                //I DATI PER RINTRACCIARE LA FOTO-->SCHEMA E VALORI
                                try
                                {
                                    try
                                    {
                                        // bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, "");
                                    }
                                    catch (Exception errins)
                                    {
                                        output.Text = errins.Message;
                                    }

                                    output.Text = "Foto Inserita Correttamente";
                                    //Aggiorniamo il repeater e la foto per il record selezionato
                                    this.CaricaDati();

                                }
                                catch (Exception error)
                                {
                                    //CANCELLO LA FOTO UPLOADATA
                                    if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                    if (System.IO.File.Exists(pathDestinazione + "\\" + "Ant" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + "Ant" + NomeCorretto);
                                    //AGGIORNO IL DETAILSVIEW
                                    output.Text = error.Message;
                                }
                            }
                            else { output.Text += ("La foto non è stata caricata! (Problema nel caricamento)"); }
                        }
                        else { output.Text = ("La foto non è stata caricata! (Formato non previsto)"); }
                    }
                }
            }
            else { output.Text = "Selezionare il file da caricare"; }
        }
        catch (Exception errorecaricamento)
        {
            output.Text += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                output.Text += errorecaricamento.InnerException.Message;

        }
    }

    protected string LeggiNomeFotoCorrettoPerUpload()
    {
        string nome = "";
        if (UploadFoto.HasFile)
        {
            //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
            string NomeCorretto = UploadFoto.FileName.Replace("+", "");
            NomeCorretto = NomeCorretto.Replace("%", "");
            NomeCorretto = NomeCorretto.Replace("_", "");
            NomeCorretto = NomeCorretto.Replace("à", "a");
            NomeCorretto = NomeCorretto.Replace("è", "e");
            NomeCorretto = NomeCorretto.Replace("ì", "i");
            NomeCorretto = NomeCorretto.Replace("ò", "o");
            NomeCorretto = NomeCorretto.Replace("ù", "u");
            NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
            nome = NomeCorretto;
        }
        return nome;
    }


    /// <summary>
    /// Elimina la foto attualmente visualizzata dal record selezionato
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnElimina_Click(object sender, EventArgs e)
    {


        if (NomeFotoSelezionata == null || NomeFotoSelezionata == "")
        {
            output.Text = "Selezionare una foto da cancellare";
            return;
        }

        //Ricarichiamo l'offerta selezionata dal db
        //Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected);

        //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
        string pathDestinazione = Server.MapPath(PercorsoFiles);
        //-------------------------------------
        //Eliminiamo il file se presente
        //-------------------------------------
        try
        {
            try
            {
                // bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeFotoSelezionata, "", pathDestinazione);
                //CANCELLO LA FOTO UPLOADATA
                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeFotoSelezionata)) System.IO.File.Delete(pathDestinazione + "\\" + NomeFotoSelezionata);
                if (System.IO.File.Exists(pathDestinazione + "\\" + "Ant" + NomeFotoSelezionata)) System.IO.File.Delete(pathDestinazione + "\\" + "Ant" + NomeFotoSelezionata);
            }
            catch (Exception errodel)
            {
                output.Text = errodel.Message;
            }

            //imgFoto.ImageUrl = "";
            //txtFotoSchema.Value = "";
            //txtFotoValori.Value = "";
            //Aggiorniamo il repeater e la foto per il record selezionato
            this.CaricaDati();
            //OfferteCollection list = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
            //rptOfferte.DataSource = list;
            //rptOfferte.DataBind();

        }
        catch (Exception errore)
        {
            output.Text += " " + errore.Message;
        }
    }
     
    #endregion


    protected void btnImmagine_Click(object sender, ImageClickEventArgs e)
    {
        NomeFotoSelezionata = ((ImageButton)sender).CommandArgument;

    }
    protected void btnImmagine_PreRender(object sender, EventArgs e)
    {
        //if (((ImageButton)sender).ClientID == ClientIDSelected)
        if (((ImageButton)sender).CommandArgument == NomeFotoSelezionata)
        {

            ((ImageButton)sender).BorderColor = System.Drawing.ColorTranslator.FromHtml("#e3c4b5");
            ((ImageButton)sender).BorderWidth = new Unit(4);
        }
        else
        {
            ((ImageButton)sender).BorderColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            ((ImageButton)sender).BorderWidth = new Unit(0);

        }
    }
}
