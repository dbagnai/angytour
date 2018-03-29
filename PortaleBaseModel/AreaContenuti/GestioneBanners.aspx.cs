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

public partial class AreaContenuti_GestioneBannersNew : CommonPage
{
    public Banners BannerSelezionato
    {
        get { return ViewState["BannerSelezionato"] != null ? (Banners)(ViewState["BannerSelezionato"]) : new Banners(); }
        set { ViewState["BannerSelezionato"] = value; }
    }
    public string PercorsoComune
    {
        get { return ViewState["PercorsoComune"] != null ? (string)(ViewState["PercorsoComune"]) : ""; }
        set { ViewState["PercorsoComune"] = value; }
    }
    public string PercorsoFiles
    {
        get { return ViewState["PercorsoFiles"] != null ? (string)(ViewState["PercorsoFiles"]) : "~/public/Banners"; }
        set { ViewState["PercorsoFiles"] = value; }
    }
    public string NomeTblBanners
    {
        get { return ViewState["NomeTblBanners"] != null ? (string)(ViewState["NomeTblBanners"]) : "tbl_banners"; }
        set { ViewState["NomeTblBanners"] = value; }
    }
    public string Sezione
    {
        get { return ViewState["Sezione"] != null ? (string)(ViewState["Sezione"]) : ""; }
        set { ViewState["Sezione"] = value; }
    }
    public int ResizeHeight
    {
        get { return ViewState["ResizeHeight"] != null ? (int)(ViewState["ResizeHeight"]) : 190; }
        set { ViewState["ResizeHeight"] = value; }
    }
    public int ResizeWidth
    {
        get { return ViewState["ResizeWidth"] != null ? (int)(ViewState["ResizeWidth"]) : 270; }
        set { ViewState["ResizeWidth"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["NomeTblBanners"] != null && Request.QueryString["NomeTblBanners"] != "")
            { NomeTblBanners = Request.QueryString["NomeTblBanners"].ToString(); }
            else if (Context.Items["NomeTblBanners"] != null && Context.Items["NomeTblBanners"].ToString() != "")
            { NomeTblBanners = Context.Items["NomeTblBanners"].ToString(); }

            if (Request.QueryString["sezione"] != null && Request.QueryString["sezione"] != "")
            { Sezione = Request.QueryString["sezione"].ToString(); }
            else if (Context.Items["sezione"] != null && Context.Items["sezione"].ToString() != "")
            { Sezione = Context.Items["sezione"].ToString(); }

            //if (NomeTblBanners.ToLower().Contains("annunci"))
            //{
            //    List<TipologiaAnnunci> listannunci = Utility.TipologieAnnunci.FindAll(delegate (TipologiaAnnunci _t) { return _t.Lingua == "I"; });
            //    ddlAreaAnnunci.Visible = true;
            //    ddlAreaAnnunci.Items.Clear();
            //    //ddlAreaAnnunci.Items.Insert(0, "Seleziona area annunci");
            //    //ddlAreaAnnunci.Items[0].Value = "";
            //    ddlAreaAnnunci.DataSource = listannunci;
            //    ddlAreaAnnunci.DataTextField = "Descrizione";
            //    ddlAreaAnnunci.DataValueField = "Codice";
            //    ddlAreaAnnunci.DataBind();
            //    lblAnnunci.Visible = true;
            //}

            //NUOVO METODO A TABELLA UNICA
            if (Sezione.ToLower().Contains("banner-destra"))
            {
                ResizeWidth = 470;//360
                ResizeHeight = 360;//280
            }

            //NUOVO METODO A TABELLA UNICA
            if (Sezione.ToLower().Contains("banner-centro"))
            {
                ResizeWidth = 470;//360
                ResizeHeight = 360;//280
            }
            //NUOVO METODO A TABELLA UNICA
            if (Sezione.ToLower().Contains("banner-basso"))
            {
                ResizeWidth = 1200;//
                ResizeHeight = 300;//
            }


            //NUOVO METODO A TABELLA UNICA
            if (Sezione.ToLower().Contains("headhomegallery"))
            {
                ResizeWidth = 1400;// 2000;//1170
                ResizeHeight = 590;//500
            }
            if (Sezione.ToLower().Contains("header-"))
            {
                ResizeWidth = 1400;// 2000;//1170
                ResizeHeight = 590;//500
            }
            if (Sezione.ToLower().Contains("header-h"))
            {
                ResizeWidth = 1400;// 2000;//1170
                ResizeHeight = 590;//500
            }
            if (Sezione.ToLower().Contains("rif00"))
            {
                ResizeWidth = 1400;// 2000;//1170
                ResizeHeight = 590;//500
            }

            //NUOVO METODO A TABELLA UNICA
            if (Sezione.ToLower().Contains("banner-portfolio-"))
            {
                ResizeWidth = 800;
                ResizeHeight = 350;
            }
            if (Sezione.ToLower().Contains("banner-portfolio-sezioni"))
            {
                ResizeWidth = 800;
                ResizeHeight = 800;
            }
          

            if (Sezione.ToLower().Contains("banner-halfstriscia-"))
            {
                ResizeWidth = 360;//360
                ResizeHeight = 180;//280
            }
            if (Sezione.ToLower().Contains("banners-fascia"))
            {
                //ResizeWidth = 2000;//360
                //ResizeHeight = 633;//280

                ResizeWidth = 1024;//360
                ResizeHeight = 546;//280
            }

            if (Sezione.ToLower().Contains("banners-fascia-home"))
            {
                //ResizeWidth = 2000;//360
                //ResizeHeight = 633;//280

                ResizeWidth = 800;//360
                ResizeHeight = 600;//280
            }
            if (Sezione.ToLower().Contains("banners-section"))
            {

                ResizeWidth = 2000; 
                ResizeHeight = 1200; 
            }
            if (Sezione.ToLower().Contains("video-"))
            {
                ResizeWidth = 1920;//360
                ResizeHeight = 1280;//280
            }

            lbldimBanner.Text = "Dimensioni finali del banner: " + ResizeWidth + "x" + ResizeHeight;

            this.CaricaDati(Sezione);
        }
        else output.Text = "";
    }

    //protected void ddlAreaAnnunci_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    this.CaricaDati(Sezione);
    //}
    //Ricarica la lista delle foto
    protected void CaricaDati(string Sezione = "")
    {
        bannersDM banDM = new bannersDM(NomeTblBanners);
        BannersCollection banners = banDM.CaricaBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Sezione);
        //Filtriamo i banners per area se si tratta dei banner degli annunci
        //if (ddlAreaAnnunci.Visible && ddlAreaAnnunci.SelectedValue != "" && banners != null)
        //{
        //    banners.RemoveAll(delegate (Banners _b) { return !_b.NavigateUrl.Contains(ddlAreaAnnunci.SelectedValue); });
        //}

        //banners.Sort(new GenericComparer2<Banners>("progressivo", System.ComponentModel.ListSortDirection.Ascending, "DataInserimento", System.ComponentModel.ListSortDirection.Descending));

        rptFoto.DataSource = banners;
        rptFoto.DataBind();

    }

    #region Gestione banners

    protected void SvuotaDettaglio()
    {
        //btnAggiorna.Text = "Modifica";
        txtNavigateUrlGB.Text = "";
        txtDescrizioneGB.Text = "";
        imgGB.ImageUrl = "";
        txtNavigateUrlRU.Text = "";
        txtDescrizioneRU.Text = "";
        imgRU.ImageUrl = "";
        txtNavigateUrl.Text = "";
        txtDescrizioneI.Text = "";
        imgI.ImageUrl = "";
        txtProgressivo.Text = "";
    }

    protected void VisualizzaDettaglio()
    {
        //btnAggiorna.Text = "Modifica";
        if (BannerSelezionato == null || BannerSelezionato.Id == 0)
        {
            SvuotaDettaglio();
        }
        txtNavigateUrlGB.Text = BannerSelezionato.NavigateUrlGB;
        txtDescrizioneGB.Text = BannerSelezionato.AlternateTextGB;
        imgGB.ImageUrl = BannerSelezionato.ImageUrlGB;

        txtNavigateUrlRU.Text = BannerSelezionato.NavigateUrlRU;
        txtDescrizioneRU.Text = BannerSelezionato.AlternateTextRU;
        imgRU.ImageUrl = BannerSelezionato.ImageUrlRU;

        txtNavigateUrl.Text = BannerSelezionato.NavigateUrl;
        txtDescrizioneI.Text = BannerSelezionato.AlternateText;
        imgI.ImageUrl = BannerSelezionato.ImageUrl;
        txtProgressivo.Text = BannerSelezionato.progressivo.ToString();
    }


    protected void AssegnaDettaglio()
    {
        //btnAggiorna.Text = "Modifica";
        BannerSelezionato.NavigateUrlGB = txtNavigateUrlGB.Text;
        BannerSelezionato.AlternateTextGB = txtDescrizioneGB.Text;
        BannerSelezionato.ImageUrlGB = imgGB.ImageUrl;

        BannerSelezionato.NavigateUrlRU = txtNavigateUrlRU.Text;
        BannerSelezionato.AlternateTextRU = txtDescrizioneRU.Text;
        BannerSelezionato.ImageUrlRU = imgRU.ImageUrl;

        BannerSelezionato.NavigateUrl = txtNavigateUrl.Text;
        BannerSelezionato.NavigateUrlRU = txtNavigateUrlRU.Text;
        BannerSelezionato.AlternateText = txtDescrizioneI.Text;
        BannerSelezionato.ImageUrl = imgI.ImageUrl;
        BannerSelezionato.sezione = Sezione;
        int _p = 0;
        int.TryParse(txtProgressivo.Text, out _p);
        BannerSelezionato.progressivo = _p;
    }


    protected void btnINserisciRusso_Click(object sender, EventArgs e)
    {
        if (BannerSelezionato == null || BannerSelezionato.Id == 0)
        {
            output.Text = "Banner non selezionato";
            return;
        }

        if (!UploadFoto.HasFile)
        {
            output.Text = "Foto per Banner in Russo non selezionata";
            return;
        }
        //Se si tratta di cariacamento banners per homepage anniunci controllo che l'url sia correto
        //if (ddlAreaAnnunci.Visible && ddlAreaAnnunci.SelectedValue != "")
        //{
        //    if (!txtNavigateUrl.Text.ToLower().Contains("tipologia=ann"))
        //    {
        //        output.Text = "Errore indicare il parametro Tipologia nell'url per l'homepage annunci";
        //        return;
        //    }
        //    if (!txtNavigateUrlGB.Text.ToLower().Contains("tipologia=ann"))
        //    {
        //        output.Text = "Errore indicare il parametro Tipologia nell'url per l'homepage annunci sia per l'italiano che per l'inglese";
        //        return;
        //    }
        //}

        //Carico la foto in russo e aggiorno la visualizzazione (se uguali non la carico aggiorno solo il db )
        BannerSelezionato.ImageUrlRU = PercorsoFiles + "/" + LeggiNomeFotoCorrettoPerUpload(); ;
        if (BannerSelezionato.ImageUrl != BannerSelezionato.ImageUrlRU)
        {
            if (CaricaFotoSuServer(ResizeHeight, ResizeWidth, true))
            {
                //Aggiorno il db
                BannerSelezionato.ImageUrlRU = PercorsoFiles + "/" + LeggiNomeFotoCorrettoPerUpload(); ;
                bannersDM banDM = new bannersDM(NomeTblBanners);
                banDM.UpdateBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, BannerSelezionato);
                //Aggiorno la visualizzazione
                CaricaDati(Sezione);
                VisualizzaDettaglio();
            }
        }
        else
        {
            //Aggiorno il db
            BannerSelezionato.ImageUrlRU = PercorsoFiles + "/" + LeggiNomeFotoCorrettoPerUpload(); ;
            bannersDM banDM = new bannersDM(NomeTblBanners);
            banDM.UpdateBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, BannerSelezionato);
            //Aggiorno la visualizzazione
            CaricaDati(Sezione);
            VisualizzaDettaglio();
        }

    }


    protected void btnINserisciInglese_Click(object sender, EventArgs e)
    {
        if (BannerSelezionato == null || BannerSelezionato.Id == 0)
        {
            output.Text = "Banner non selezionato";
            return;
        }

        if (!UploadFoto.HasFile)
        {
            output.Text = "Foto per Banner in Inglese non selezionata";
            return;
        }
        //Se si tratta di cariacamento banners per homepage anniunci controllo che l'url sia correto
        //if (ddlAreaAnnunci.Visible && ddlAreaAnnunci.SelectedValue != "")
        //{
        //    if (!txtNavigateUrl.Text.ToLower().Contains("tipologia=ann"))
        //    {
        //        output.Text = "Errore indicare il parametro Tipologia nell'url per l'homepage annunci";
        //        return;
        //    }
        //    if (!txtNavigateUrlGB.Text.ToLower().Contains("tipologia=ann"))
        //    {
        //        output.Text = "Errore indicare il parametro Tipologia nell'url per l'homepage annunci sia per l'italiano che per l'inglese";
        //        return;
        //    }
        //}

        //Carico la foto in inglese e aggiorno la visualizzazione (se uguali non la carico aggiorno solo il db )
        BannerSelezionato.ImageUrlGB = PercorsoFiles + "/" + LeggiNomeFotoCorrettoPerUpload(); ;
        if (BannerSelezionato.ImageUrl != BannerSelezionato.ImageUrlGB)
        {
            if (CaricaFotoSuServer(ResizeHeight, ResizeWidth, true))
            {
                //Aggiorno il db
                BannerSelezionato.ImageUrlGB = PercorsoFiles + "/" + LeggiNomeFotoCorrettoPerUpload(); ;
                bannersDM banDM = new bannersDM(NomeTblBanners);
                banDM.UpdateBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, BannerSelezionato);
                //Aggiorno la visualizzazione
                CaricaDati(Sezione);
                VisualizzaDettaglio();
            }
        }
        else
        {
            //Aggiorno il db
            BannerSelezionato.ImageUrlGB = PercorsoFiles + "/" + LeggiNomeFotoCorrettoPerUpload(); ;
            bannersDM banDM = new bannersDM(NomeTblBanners);
            banDM.UpdateBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, BannerSelezionato);
            //Aggiorno la visualizzazione
            CaricaDati(Sezione);
            VisualizzaDettaglio();
        }

    }

    protected void btnAggiorna_Click(object sender, EventArgs e)
    {
        if (BannerSelezionato == null || BannerSelezionato.Id == 0)
        {
            output.Text = "Banner non selezionato";
            return;
        }
        //Se si tratta di cariacamento banners per homepage anniunci controllo che l'url sia correto
        //if (ddlAreaAnnunci.Visible && ddlAreaAnnunci.SelectedValue != "")
        //{
        //    if (!txtNavigateUrl.Text.ToLower().Contains("tipologia=ann"))
        //    {
        //        output.Text = "Errore indicare il parametro Tipologia nell'url per l'homepage annunci";
        //        return;
        //    }
        //    if (!txtNavigateUrlGB.Text.ToLower().Contains("tipologia=ann"))
        //    {
        //        output.Text = "Errore indicare il parametro Tipologia nell'url per l'homepage annunci sia per l'italiano che per l'inglese";
        //        return;
        //    }
        //}
        //Assegno i valori dalle al banner selezionato 
        AssegnaDettaglio();


        //Aggiorniamo il db
        bannersDM banDM = new bannersDM(NomeTblBanners);
        banDM.UpdateBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, BannerSelezionato);
        CaricaDati(Sezione);
        VisualizzaDettaglio();

    }

    /// <summary>
    /// Inserisce sul server l'immgine selezionata col FIleUpload
    /// </summary>
    protected bool CaricaFotoSuServer(int maxheight, int maxwidth, bool ridimensiona)
    {
        bool ret = false;
        //maxheight  = 177; //Altezza max
        //maxwidth = 266; //LArghezza max
        //ridimensiona = false; //non ridimensiono imposto una size fissa se true invece ridimensiono scalando in proporzione

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
                    //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
                    string NomeCorretto = LeggiNomeFotoCorrettoPerUpload();
                    if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto))
                    {
                        output.Text = ("La foto non può essere caricata perché già presente sul server!");

                    }
                    else
                    {
                        //Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        if (UploadFoto.PostedFile.ContentType == "image/jpeg" || UploadFoto.PostedFile.ContentType == "image/pjpeg" || UploadFoto.PostedFile.ContentType == "image/bmp" || UploadFoto.PostedFile.ContentType == "image/png")
                        {
                            //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                            if (ResizeAndSave(UploadFoto.PostedFile.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                this.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto);
                                ret = true; //Se tutto ok imposto true il caricamento
                            }
                            else { output.Text += ("La foto non è stata caricata! (Problema nel caricamento)"); }
                        }
                        else if (UploadFoto.PostedFile.ContentType == "image/gif")
                        {
                            //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                            //    output.Text = ("La foto non è stata caricata! (Formato non previsto)"); 
                            UploadFoto.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                            ret = true;
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
        return ret;
    }

    protected string LeggiNomeFotoCorrettoPerUpload()
    {
        string nome = "";
        if (UploadFoto.HasFile)
        {
            //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
            string NomeCorretto = UploadFoto.FileName.Replace("+", "");
            NomeCorretto = NomeCorretto.Replace(" ", "-");
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
    /// Carica la foto in italiano e inserisce i dati del banner nel db
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCarica_Click(object sender, EventArgs e)
    {

        string pathDestinazione = Server.MapPath(PercorsoFiles);
        if (!Directory.Exists(pathDestinazione))
            Directory.CreateDirectory(pathDestinazione);
        string NomeCorretto = LeggiNomeFotoCorrettoPerUpload();

        //Se si tratta di cariacamento banners per homepage anniunci controllo che l'url sia correto
        //if (ddlAreaAnnunci.Visible && ddlAreaAnnunci.SelectedValue != "")
        //{
        //    if (!txtNavigateUrl.Text.ToLower().Contains("tipologia=ann"))
        //    {
        //        output.Text = "Errore indicare il parametro Tipologia nell'url per l'homepage annunci";
        //        return;
        //    }
        //    if (!txtNavigateUrlGB.Text.ToLower().Contains("tipologia=ann"))
        //    {
        //        output.Text = "Errore indicare il parametro Tipologia nell'url per l'homepage annunci sia per l'italiano che per l'inglese";
        //        return;
        //    }
        //}


        if (CaricaFotoSuServer(ResizeHeight, ResizeWidth, true))
        {
            //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
            //I DATI PER RINTRACCIARE LA FOTO
            try
            {
                try
                {
                    bannersDM banDM = new bannersDM(NomeTblBanners);
                    Banners ban = new Banners();
                    ban.AlternateTextGB = txtDescrizioneGB.Text;
                    ban.NavigateUrlGB = txtNavigateUrlGB.Text;
                    ban.AlternateTextRU = txtDescrizioneRU.Text;
                    ban.NavigateUrlRU = txtNavigateUrlRU.Text;

                    ban.AlternateText = txtDescrizioneI.Text;
                    ban.NavigateUrl = txtNavigateUrl.Text;
                    ban.ImageUrl = PercorsoFiles + "/" + NomeCorretto;
                    ban.sezione = Sezione;

                    banDM.InsertBanner(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ban);
                    BannerSelezionato = ban;
                    VisualizzaDettaglio();
                }
                catch (Exception errins)
                {
                    output.Text = errins.Message;
                }

                output.Text = "Foto Inserita Correttamente";
                //Aggiorniamo il repeater e la foto per il record selezionato
                this.CaricaDati(Sezione);

            }
            catch (Exception error)//Errore aggiornamento del db -> canello le foto caricate
            {
                //CANCELLO LA FOTO UPLOADATA
                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                if (System.IO.File.Exists(pathDestinazione + "\\" + "Ant" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + "Ant" + NomeCorretto);
                //AGGIORNO IL DETAILSVIEW
                output.Text = error.Message;
            }
        }
    }




    /// <summary>
    /// Elimina la foto attualmente visualizzata dal record selezionato
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnElimina_Click(object sender, EventArgs e)
    {
        if (BannerSelezionato == null || BannerSelezionato.ImageUrl == "")
        {
            output.Text = "Selezionare un banner da cancellare";
            return;
        }

        string pathDestinazione = Server.MapPath(PercorsoFiles);
        FileInfo fi = new FileInfo(Server.MapPath(BannerSelezionato.ImageUrl));
        FileInfo fiGB = new FileInfo(Server.MapPath(BannerSelezionato.ImageUrlGB));
        FileInfo fiRU = new FileInfo(Server.MapPath(BannerSelezionato.ImageUrlRU));

        //-------------------------------------
        //Eliminiamo il file se presente
        //-------------------------------------
        try
        {
            try
            {
                // bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeFotoSelezionata, "", pathDestinazione);
                //CANCELLO LA FOTO UPLOADATA
                if (fi.Exists) fi.Delete();
                if (System.IO.File.Exists(fi.DirectoryName + "\\ant" + fi.Name)) System.IO.File.Delete(fi.DirectoryName + "\\ant" + fi.Name);

                if (fiGB.Exists) fiGB.Delete();
                if (System.IO.File.Exists(fiGB.DirectoryName + "\\ant" + fiGB.Name)) System.IO.File.Delete(fiGB.DirectoryName + "\\ant" + fiGB.Name);
                if (fiRU.Exists) fiRU.Delete();
                if (System.IO.File.Exists(fiRU.DirectoryName + "\\ant" + fiRU.Name)) System.IO.File.Delete(fiRU.DirectoryName + "\\ant" + fiRU.Name);


                //Elimino il record e svuoto la visualizzazione
                bannersDM banDM = new bannersDM(NomeTblBanners);
                banDM.DeleteBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, BannerSelezionato);
                BannerSelezionato = null;
                SvuotaDettaglio();
            }
            catch (Exception errodel)
            {
                output.Text = errodel.Message;
            }

            //Aggiorniamo il repeater e la foto per il record selezionato
            this.CaricaDati(Sezione);

        }
        catch (Exception errore)
        {
            output.Text += " " + errore.Message;
        }
    }


    /// <summary>
    /// SUB per save e resize dell'immagine
    /// </summary>
    /// <param name="imgStr"></param>
    /// <param name="Width"></param>
    /// <param name="Height"></param>
    /// <param name="Filename"></param>
    /// <param name="ridimensiona"></param>
    /// <returns></returns>
    private bool ResizeAndSave(System.IO.Stream imgStr, int Width, int Height, string Filename, bool ridimensiona)
    {
        try
        {
            System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(imgStr);
            if (ridimensiona == true)
            {
                //CREO LE DIMENSIONI DELLA FOTO SALVATA IN BASE AL RAPORTO ORIGINALE DI ASPETTO
                int altezzaStream = bmpStream.Height; //altezza foto originale
                int larghezzaStream = bmpStream.Width; //larghezza foto originale

                if (altezzaStream <= larghezzaStream)
                {
                    int Maxheight = Height;
                    if (Width > larghezzaStream) Width = larghezzaStream;
                    Height = Convert.ToInt32(((double)Width / (double)larghezzaStream) * (double)altezzaStream);
                    if (Height > Maxheight)
                    {
                        Height = Maxheight;
                        Width = Convert.ToInt32(((double)Maxheight / (double)altezzaStream) * (double)larghezzaStream);
                    }

                }
                else
                {
                    int maxwidth = Width;
                    if (Height > altezzaStream) Height = altezzaStream;
                    Width = Convert.ToInt32(((double)Height / (double)altezzaStream) * (double)larghezzaStream);
                    if (Width > maxwidth)
                    {
                        Width = maxwidth;
                        Height = Convert.ToInt32(((double)maxwidth / (double)larghezzaStream) * (double)altezzaStream);
                    }
                }
                //FINE CALCOLO ----------------------------------------------------------
            }

            using (System.Drawing.Bitmap img_orig = new System.Drawing.Bitmap(bmpStream))
            {

                System.Drawing.Bitmap img_filtrata = img_orig;
                //FILTRI CONTRASTO BRIGHTNESS/contrast/sturation
                //img_filtrata = ImageProcessing.applicaSaturationCorrection(img_filtrata, 0.05);
                //img_filtrata = ImageProcessing.applicaBrightness(img_filtrata, 0.03);
                //img_filtrata = ImageProcessing.applicaContrast(img_filtrata, 0.75);
                //img_filtrata = ImageProcessing.applicaAdaptiveSmoothing(img_filtrata);
                //img_filtrata = ImageProcessing.applicaConservativeSmoothing(img_filtrata);
                //img_filtrata = ImageProcessing.applicaHSLFilter(img_filtrata, 0.87, 0.075);
                //img_filtrata = ImageProcessing.applicaGaussianBlur(img_filtrata, 1, 5);
                //img_filtrata = ImageProcessing.applicaMediano(img_filtrata, 4);
                // ImageProcessing.NoiseRemoval(img_filtrata);
                //img_filtrata = ImageProcessing.MeanFilter(img_filtrata, 2);
                img_filtrata = ImageProcessing.applicaResizeBilinear(img_filtrata, Width, Height); //resisze
                //img_filtrata = ImageProcessing.applicaResizeBicubic(img_filtrata, Width, Height); //resisze


                //TOLGO IL FILTRO PER TEST
                //System.Drawing.Bitmap img_filtrata = img_orig;
                //RESIZE FINALE DELL'IMMAGINE
                //System.Drawing.Bitmap img = new System.Drawing.Bitmap(img_filtrata, new System.Drawing.Size(Width, Height));
                using (System.Drawing.Bitmap img = img_filtrata)
                {
                    System.Drawing.Imaging.ImageFormat imgF = null;
                    switch (System.IO.Path.GetExtension(Filename).ToLower())
                    {
                        case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif; break;
                        case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; break;
                        case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; break;
                        default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                    }
                    //img.Save(Filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    if (imgF == System.Drawing.Imaging.ImageFormat.Jpeg)
                    {
                        // Create an Encoder object based on the GUID for the Quality parameter category.
                        ImageCodecInfo jgpEncoder = GetEncoder(imgF); //ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == "image/jpeg");
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(3);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 85L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                        myEncoderParameters.Param[2] = new EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, (int)EncoderValue.RenderProgressive);
                        img.Save(Filename, jgpEncoder, myEncoderParameters);
                    }
                    else
                        img.Save(Filename, imgF);
                }

            }
        }
        catch
        {
            return false;
        }
        return true;
    }
    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }

    protected void CreaAnteprima(string filePath, int Altezza, int Larghezza, string pathAnteprime, string nomeAnteprima)
    {
        string PathTempAnteprime = pathAnteprime;
        System.Drawing.Imaging.ImageFormat imgF = null;
        //System.IO.File.Exists(PathTempAnteprime);
        if (!System.IO.Directory.Exists(PathTempAnteprime))
        {
            System.IO.Directory.CreateDirectory(PathTempAnteprime);
        }
        // throw new Exception("Cartella temporanea di destinazione per l'anteprima non trovata!");

        using (System.IO.FileStream file = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
        {
            using (System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(file))
            {
                int altezzaStream = bmpStream.Height;
                int larghezzaStream = bmpStream.Width;
                if (altezzaStream <= larghezzaStream)
                    Altezza = Convert.ToInt32((double)Larghezza / (double)larghezzaStream * (double)altezzaStream);
                else
                    Larghezza = Convert.ToInt32((double)Altezza / (double)altezzaStream * (double)larghezzaStream);
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(Larghezza, Altezza));

                //Antemprima salvata in jpg
                nomeAnteprima = nomeAnteprima.Substring(0, nomeAnteprima.LastIndexOf('.'));
                nomeAnteprima += ".jpg";

                img.Save(PathTempAnteprime + nomeAnteprima, System.Drawing.Imaging.ImageFormat.Jpeg);
                file.Close();
            }
        }
        if (!System.IO.File.Exists(PathTempAnteprime + nomeAnteprima))
            output.Text = ("Anteprima Allegato non salvata correttamente!");

    }
    //protected string ComponiUrlAnteprima(object FotoColl, string id)
    //{
    //    string url = "";

    //if (FotoColl != null && ((AllegatiCollection)FotoColl).Count > 0 && ((AllegatiCollection)FotoColl)[0].NomeAnteprima!=null)
    //        if (!((AllegatiCollection)FotoColl)[0].NomeAnteprima.ToLower().StartsWith("http://"))
    //            url = PercorsoFiles + "/" + TipologiaOfferte + "/" + id + "/" + ((AllegatiCollection)FotoColl)[0].NomeAnteprima;
    //        else
    //            url = ((AllegatiCollection)FotoColl)[0].NomeAnteprima;

    //    return url;
    //}
    //protected string ComponiUrlGalleriaAnteprima(string NomeAnteprima)
    //{
    //    string url = "";

    //    url = PercorsoFiles + "/" + TipologiaOfferte + "/" + OffertaIDSelected + "/" + NomeAnteprima;

    //    return url;
    //}
    #endregion


    protected void btnImmagine_Click(object sender, ImageClickEventArgs e)
    {
        string[] args = ((ImageButton)sender).CommandArgument.Split(',');
        if (args != null && args.Length == 2)
        {
            //NomeFotoSelezionata = args[0];
            //IdFotoSelezionata = args[1];
            bannersDM banDM = new bannersDM(NomeTblBanners);
            BannerSelezionato = banDM.CaricaBannerPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, args[1]);
            BannerSelezionato.sezione = Sezione;

            this.VisualizzaDettaglio();
        }
    }
    protected void btnImmagine_PreRender(object sender, EventArgs e)
    {
        //if (((ImageButton)sender).ClientID == ClientIDSelected)
        if (((ImageButton)sender).CommandArgument == BannerSelezionato.ImageUrl + "," + BannerSelezionato.Id)
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
