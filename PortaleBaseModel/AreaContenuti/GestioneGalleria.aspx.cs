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
                            if (ResizeAndSave(UploadFoto.PostedFile.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                this.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto);
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
                        ImageCodecInfo jgpEncoder = GetEncoder(imgF);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        // Create an EncoderParameters object.
                        // An EncoderParameters object has an array of EncoderParameter objects. In this case, there is only one EncoderParameter object in the array.
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 85L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
                        myEncoderParameters.Param[0] = myEncoderParameter;
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
            System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(file);
            int altezzaStream = bmpStream.Height;
            int larghezzaStream = bmpStream.Width;
            if (altezzaStream <= larghezzaStream)
                Altezza = Convert.ToInt32((double)Larghezza / (double)larghezzaStream * (double)altezzaStream);
            else
                Larghezza = Convert.ToInt32((double)Altezza / (double)altezzaStream * (double)larghezzaStream);
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(Larghezza, Altezza));
            switch (System.IO.Path.GetExtension(filePath).ToLower())
            {
                case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif; break;
                case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; break;
                case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; break;

                default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
            }
            if (imgF == System.Drawing.Imaging.ImageFormat.Jpeg)
            {

                // Create an Encoder object based on the GUID for the Quality parameter category.
                ImageCodecInfo jgpEncoder = GetEncoder(imgF); //ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == "image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter objects. In this case, there is only one EncoderParameter object in the array.
                EncoderParameters myEncoderParameters = new EncoderParameters(3);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
                myEncoderParameters.Param[0] = myEncoderParameter;
                myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                myEncoderParameters.Param[2] = new EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, (int)EncoderValue.RenderProgressive);

                img.Save(PathTempAnteprime + nomeAnteprima, jgpEncoder, myEncoderParameters);
            }
            else
                img.Save(PathTempAnteprime + nomeAnteprima, imgF);
            file.Close();
        }
        if (!System.IO.File.Exists(PathTempAnteprime + nomeAnteprima))
            output.Text = ("Anteprima Allegato non salvata correttamente!");

    }
    //protected string ComponiUrlAnteprima(object FotoColl, string id)
    //{
    //    string url = "";

    //    if (FotoColl != null && ((AllegatiCollection)FotoColl).Count > 0)
    //        url = PercorsoFiles + "/" + TipologiaOfferte + "/" + id + "/" + ((AllegatiCollection)FotoColl)[0].NomeAnteprima;

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
