using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;
using System.Drawing.Imaging;

/// <summary>
/// Descrizione di riepilogo per filemanage
/// </summary>
public class filemanage
{
    public filemanage()
    {
        //
        // TODO: aggiungere qui la logica del costruttore
        //
    }

    public string CaricaFile(HttpServerUtility Server, FileUpload UploadControl, string DescrizioneFile, string idrecord,string tipologia)
    {
        string ret = "";
        try
        {
            //Controlliamo se ho selezionato un record
            if (idrecord == null || idrecord == "")
            {
                ret += "Selezionare un elemento per associare il file";
                return ret;
            }
            int idSelected = 0;
            if (!Int32.TryParse(idrecord, out idSelected))
            {
                ret += "Selezionare un elemento per associare il file";
                return ret;
            }
            offerteDM offDM = new offerteDM();
            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
            string pathDestinazione = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + tipologia + "/" + idrecord);
            if (!Directory.Exists(pathDestinazione))
                Directory.CreateDirectory(pathDestinazione);

            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (UploadControl.HasFile)
            {
                if (UploadControl.PostedFile.ContentLength > 20000000)
                {

                   ret += "Il File non può essere caricato perché supera 20MB!";
                }
                else
                {
                    //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
                    string NomeCorretto = UploadControl.FileName.Replace("+", "");
                    NomeCorretto = NomeCorretto.Replace("%", "");
                    NomeCorretto = NomeCorretto.Replace(" ", "-");
                    NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
                    //string NomeCorretto = Server.HtmlEncode(FotoUpload1.FileName);
                    if (System.IO.File.Exists(pathDestinazione))
                    {
                       ret += ("Il File non può essere caricato perché già presente sul server!");
                    }
                    else
                    {
                        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        if (UploadControl.PostedFile.ContentType == "image/jpeg" || UploadControl.PostedFile.ContentType == "image/pjpeg" || UploadControl.PostedFile.ContentType == "image/gif")
                        {
                            int maxheight = 800;
                            int maxwidth = 1000;
                            bool ridimensiona = true;
                            //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                            if (ResizeAndSave(UploadControl.PostedFile.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                this.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto);
                                //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
                                //I DATI PER RINTRACCIARE LA FOTO-->SCHEMA E VALORI
                                try
                                {
                                    try
                                    {
                                        bool tmpret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                    }
                                    catch (Exception errins)
                                    {
                                       ret = errins.Message;
                                    }

                                    //   output.Text += "Foto Inserita Correttamente<br/>";


                                }
                                catch (Exception error)
                                {
                                    //CANCELLO LA FOTO UPLOADATA
                                    if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                    if (System.IO.File.Exists(pathDestinazione + "\\" + "Ant" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + "Ant" + NomeCorretto);
                                    //AGGIORNO IL DETAILSVIEW
                                   ret = error.Message;
                                }
                            }
                            else {ret  += ("La foto non è stata caricata! (Problema nel caricamento)"); }
                        }
                        else if (UploadControl.PostedFile.ContentType == "application/pdf")
                        {

                            //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                            UploadControl.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                            try
                            {
                                try
                                {
                                    bool tmpret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                }
                                catch (Exception errins)
                                {
                                    ret = errins.Message;
                                }

                                //  output.Text += "Documento Inserito Correttamente<br/>";

                            }
                            catch (Exception error)
                            {
                                //CANCELLO IL FILE UPLOADATO
                                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                //AGGIORNO IL DETAILSVIEW
                                ret = error.Message;
                            }
                        }
                        else
                        {

                            //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                            UploadControl.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                            try
                            {
                                try
                                {
                                    bool tmpret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                }
                                catch (Exception errins)
                                {
                                   ret = errins.Message;
                                }

                                //    output.Text += "Documento Inserito Correttamente<br/>";

                            }
                            catch (Exception error)
                            {
                                //CANCELLO IL FILE UPLOADATO
                                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                //AGGIORNO IL DETAILSVIEW
                                ret = error.Message;
                            }
                        }
                    }
                }
            }

        }
        catch (Exception errorecaricamento)
        {
           ret += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
               ret += errorecaricamento.InnerException.Message;

        }
        return ret;
    }
    private void CreaAnteprima(string filePath, int Altezza, int Larghezza, string pathAnteprime, string nomeAnteprima)
    {
        string ret = "";
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
                switch (System.IO.Path.GetExtension(filePath).ToLower())
                {
                    case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif; break;
                    case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; break;
                    case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; break;
                    default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                }

                img.Save(PathTempAnteprime + nomeAnteprima, imgF);
            }
            file.Close();
        }
        if (!System.IO.File.Exists(PathTempAnteprime + nomeAnteprima))
            ret += ("Anteprima Allegato non salvata correttamente!");

    }
    private bool ResizeAndSave(System.IO.Stream imgStr, int Width, int Height, string Filename, bool ridimensiona)
    {
        try
        {
            using (System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(imgStr))
            {
                if (ridimensiona == true)
                {
                    //CREO LE DIMENSIONI DELLA FOTO SALVATA IN BASE AL RAPORTO ORIGINALE DI ASPETTO
                    int altezzaStream = bmpStream.Height; //altezza foto originale
                    int larghezzaStream = bmpStream.Width; //larghezza foto originale
                    if (altezzaStream <= larghezzaStream)
                        Height = Convert.ToInt32(((double)Width / (double)larghezzaStream) * (double)altezzaStream);
                    else
                        Width = Convert.ToInt32(((double)Height / (double)altezzaStream) * (double)larghezzaStream);

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

}