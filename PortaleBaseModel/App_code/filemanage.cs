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
    public static int reslevels = 2; //Imposta il numero di livelli di immagini da usare in base alle rispuluzioni 1- solo xs 2-solo xs sm etc ...

    public filemanage()
    {
        //
        // TODO: aggiungere qui la logica del costruttore
        //
    }
    public static string ModificaFile(string idrecord, string nomefile, string DescrizioneFile, string progressivofile)
    {
        string ret = "";
        try
        {
            offerteDM offDM = new offerteDM();
            long i = 0;
            long.TryParse(idrecord, out i);
            if (!offDM.modificaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, i, nomefile, DescrizioneFile, progressivofile))
                ret = "Errore Modifica file";
        }
        catch (Exception errins)
        {
            ret = errins.Message;
        }
        return ret;
    }

    public static string CaricaFile(HttpServerUtility Server, HttpPostedFile file, string DescrizioneFile, string idrecord, string tipologia, string progressivofile)
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
            long idSelected = 0;
            if (!long.TryParse(idrecord, out idSelected))
            {
                ret += "Selezionare un elemento per associare il file";
                return ret;
            }
            offerteDM offDM = new offerteDM();
            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
            //  string pathDestinazione = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + Tipologia + "\\" + idrecord; //senza server.mapath
            string pathDestinazione = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + tipologia + "/" + idrecord);
            if (!Directory.Exists(pathDestinazione))
                Directory.CreateDirectory(pathDestinazione);

            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (file != null)
            {
                if (file.ContentLength > 20000000)
                {

                    ret += "Il File non può essere caricato perché supera 20MB!";
                }
                else
                {
                    //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
                    string NomeCorretto = file.FileName.Replace("+", "");
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
                        if (file.ContentType == "image/jpeg" || file.ContentType == "image/pjpeg" || file.ContentType == "image/gif" || file.ContentType == "image/png")
                        {
                            int maxheight = 1000;
                            int maxwidth = 1000;
                            bool ridimensiona = true;
                            //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                            if (ResizeAndSave(file.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto, true, true);
                                //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
                                //I DATI PER RINTRACCIARE LA FOTO-->SCHEMA E VALORI
                                try
                                {
                                    try
                                    {
                                        bool tmpret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile, progressivofile);
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
                            else { ret += ("La foto non è stata caricata! (Problema nel caricamento)"); }
                        }
                        else if (file.ContentType == "application/pdf")
                        {

                            //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                            file.SaveAs(pathDestinazione + "\\" + NomeCorretto);
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
                            file.SaveAs(pathDestinazione + "\\" + NomeCorretto);
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
            else { ret = "Selezionare il file da caricare"; }
        }
        catch (Exception errorecaricamento)
        {
            ret += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                ret += errorecaricamento.InnerException.Message;

        }
        return ret;
    }


    public static string CaricaFile(HttpServerUtility Server, FileUpload UploadControl, string DescrizioneFile, string idrecord, string tipologia, string progressivofile)
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
            long idSelected = 0;
            if (!long.TryParse(idrecord, out idSelected))
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
                            int maxheight = 1000;
                            int maxwidth = 1000;
                            bool ridimensiona = true;
                            //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                            if (ResizeAndSave(UploadControl.PostedFile.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 450, 450, pathDestinazione + "\\", "Ant" + NomeCorretto, true, true);
                                //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
                                //I DATI PER RINTRACCIARE LA FOTO-->SCHEMA E VALORI
                                try
                                {
                                    try
                                    {
                                        bool tmpret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile, progressivofile);
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
                            else { ret += ("La foto non è stata caricata! (Problema nel caricamento)"); }
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
            else { ret = "Selezionare il file da caricare"; }
        }
        catch (Exception errorecaricamento)
        {
            ret += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                ret += errorecaricamento.InnerException.Message;

        }
        return ret;
    }
    public static string CaricaFile(HttpServerUtility server, string urlfile, string Nomefile, string codicetipologia, string idrecord)
    {
        string responsestr = "";
        try
        {
            //Controlliamo se ho selezionato un record
            if (idrecord == null || idrecord == "")
            {
                return "No id selected!";
            }
            long idSelected = 0;
            if (!long.TryParse(idrecord, out idSelected))
            {
                return "No id selected!";
            }

            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
            string pathDestinazione = server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + codicetipologia + "/" + idrecord);
            if (!System.IO.Directory.Exists(pathDestinazione))
                System.IO.Directory.CreateDirectory(pathDestinazione);

            //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
            string NomeCorretto = Nomefile.Replace("+", "");
            NomeCorretto = NomeCorretto.Replace("%", "");
            NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
            //string NomeCorretto = Server.HtmlEncode(FotoUpload1.FileName);
            if (System.IO.File.Exists(pathDestinazione))
            {
                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
            }
            //Faccio la get da web e salvo nel percorso di destinazione
            WelcomeLibrary.UF.SharedStatic.MakeHttpGet(urlfile, pathDestinazione + "\\" + NomeCorretto);
            try
            {
                try
                {
                    offerteDM offDM = new offerteDM();
                    bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, "");
                }
                catch (Exception errins)
                {

                }
                responsestr += "";//tutto ok file caricato
            }
            catch (Exception error)
            {
                //CANCELLO IL FILE UPLOADATO
                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                responsestr += error.Message;
                if (error.InnerException != null)
                    responsestr += error.InnerException.Message;
            }
        }
        catch (Exception errorecaricamento)
        {
            responsestr += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                responsestr += errorecaricamento.InnerException.Message;

        }
        return responsestr;
    }
    public static string EliminaFile(HttpServerUtility Server, string idrecord, string tipologia, string nomefile)
    {
        string ret = "";
        //Controlliamo se ho selezionato un record
        if (idrecord == null || idrecord == "")
        {
            ret = "Selezionare un elemento per cancellare la foto";
            return ret;
        }

        if (nomefile == null || nomefile == "")
        {
            ret = "Selezionare una foto da cancellare";
            return ret;
        }
        long idSelected = 0;
        if (!long.TryParse(idrecord, out idSelected))
        {
            ret = "Selezionare un elemento per cancellare la foto";
            return ret;
        }
        //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
        // string pathDestinazione = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + Tipologia + "\\" + idrecord;
        string pathDestinazione = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + tipologia + "/" + idrecord);
        //-------------------------------------
        //Eliminiamo il file se presente
        //-------------------------------------
        try
        {
            try
            {
                offerteDM offDM = new offerteDM();
                if (!offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, nomefile, "", pathDestinazione))
                    ret = "Errore cancellazione file!";
            }
            catch (Exception errodel)
            {
                ret = errodel.Message;
            }
        }
        catch (Exception errore)
        {
            ret += " " + errore.Message;
        }
        return ret;
    }



    public static bool ResizeAndSave(System.IO.Stream imgStr, int Width, int Height, string Filename, bool ridimensiona)
    {
        try
        {
            using (System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(imgStr))
            {
                //if (ridimensiona == true)
                //{
                //    int altezzaStream = bmpStream.Height; //altezza foto originale
                //    int larghezzaStream = bmpStream.Width; //larghezza foto originale
                //    if (altezzaStream <= larghezzaStream)
                //        Height = Convert.ToInt32(((double)Width / (double)larghezzaStream) * (double)altezzaStream);
                //    else
                //        Width = Convert.ToInt32(((double)Height / (double)altezzaStream) * (double)larghezzaStream);
                //}
                if (ridimensiona == true)
                {
                    //CREO LE DIMENSIONI DELLA FOTO SALVATA IN BASE AL RAPORTO ORIGINALE DI ASPETTO
                    int altezzaStream = bmpStream.Height; //altezza foto originale
                    int larghezzaStream = bmpStream.Width; //larghezza foto originale
                    RicalcolaDimensioni(altezzaStream, larghezzaStream, ref Width, ref Height);
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
                    //img_filtrata = ImageProcessing.applicaResizeBilinear(img_filtrata, Width, Height); //resisze
                    //img_filtrata = ImageProcessing.applicaResizeBicubic(img_filtrata, Width, Height); //resisze

                    img_filtrata = new System.Drawing.Bitmap(img_filtrata, new System.Drawing.Size(Width, Height));
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
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
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

    /// <summary>
    /// SUB per save e resize dell'immagine
    /// </summary>
    /// <param name="imgStr"></param>
    /// <param name="Width"></param>
    /// <param name="Height"></param>
    /// <param name="Filename"></param>
    /// <param name="ridimensiona"></param>
    /// <returns></returns>
    public static bool ResizeAndSave(System.IO.Stream imgStr, ref int Width, ref int Height, string Filename, bool ridimensiona)
    {
        try
        {
            System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(imgStr);
            if (ridimensiona == true)
            {
                //CREO LE DIMENSIONI DELLA FOTO SALVATA IN BASE AL RAPORTO ORIGINALE DI ASPETTO
                int altezzaStream = bmpStream.Height; //altezza foto originale
                int larghezzaStream = bmpStream.Width; //larghezza foto originale
                RicalcolaDimensioni(altezzaStream, larghezzaStream, ref Width, ref Height);
                //FINE CALCOLO ----------------------------------------------------------
            }

            using (System.Drawing.Bitmap img_orig = new System.Drawing.Bitmap(bmpStream))
            {
                System.Drawing.Bitmap img_filtrata = img_orig;
                img_filtrata = new System.Drawing.Bitmap(img_filtrata, new System.Drawing.Size(Width, Height));
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
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        img.Save(Filename, jgpEncoder, myEncoderParameters);
                    }
                    else
                        img.Save(Filename, imgF);
                }
            }
        }
        catch (Exception errore)
        {
            throw new ApplicationException("Resize and upload:", errore);
        }
        return true;
    }

    public static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
    {
        System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
        foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
    public static bool CreaAnteprima(string fileorigine, int Width, int Height, string pathAnteprime, string nomeAnteprima, bool replacefile = false, bool generateversions = false)
    {
       // generateversions = false; //COMMENTARE SE VUOI ELIMINARE IL FUNZIONAMENTO CON LE RISOLUZIONI MULTIPLE PER LE IMMAGINI
        bool ret = false;
        try
        {
            if (!System.IO.File.Exists(fileorigine))
                return false;

            bool existsanteprima = false;
            if (System.IO.File.Exists(pathAnteprime + nomeAnteprima))
                existsanteprima = true;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Scalatura immagini per bootstrap (Creo le versioni dell'imagine per le varie risoluzioni , se non presenti) 4 versioni
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // -xs 0> <576px  | -sm  576> <768  | -md 769< <992  |  -lg >992 <1200  | -xl(no estensione) >1200
            string filepath = System.IO.Path.GetDirectoryName(fileorigine).ToString();
            string filenamenoext = System.IO.Path.GetFileNameWithoutExtension(fileorigine).ToString();
            string fileext = System.IO.Path.GetExtension(fileorigine).ToLower();
            int larghezza_xs = 576; int altezza_xs = 576; System.Drawing.Bitmap img_xs = null; string filename_xs = filenamenoext + "-xs" + fileext;
            int larghezza_sm = 768; int altezza_sm = 768; System.Drawing.Bitmap img_sm = null; string filename_sm = filenamenoext + "-sm" + fileext;
            int larghezza_md = 992; int altezza_md = 992; System.Drawing.Bitmap img_md = null; string filename_md = filenamenoext + "-md" + fileext;
            int larghezza_lg = 1200; int altezza_lg = 1200; System.Drawing.Bitmap img_lg = null; string filename_lg = filenamenoext + "-lg" + fileext;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool existreducedfiles = true;
            if (generateversions)
                if ((!System.IO.File.Exists(filepath + "\\" + filename_xs) && filemanage.reslevels >= 1) ||
                    (!System.IO.File.Exists(filepath + "\\" + filename_sm) && filemanage.reslevels >= 2) ||
                    (!System.IO.File.Exists(filepath + "\\" + filename_md) && filemanage.reslevels >= 3) ||
                    (!System.IO.File.Exists(filepath + "\\" + filename_lg) && filemanage.reslevels >= 4))
                    existreducedfiles = false;

            if (existsanteprima && existreducedfiles && !replacefile)
                return true;

            if (!System.IO.Directory.Exists(pathAnteprime))
            {
                System.IO.Directory.CreateDirectory(pathAnteprime);
            }
            // throw new Exception("Cartella temporanea di destinazione per l'anteprima non trovata!");
            using (System.IO.FileStream file = new System.IO.FileStream(fileorigine, System.IO.FileMode.Open))
            {
                System.Drawing.Imaging.ImageFormat imgF = null;
                System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(file);
                int altezzaStream = bmpStream.Height;
                int larghezzaStream = bmpStream.Width;
                RicalcolaDimensioni(altezzaStream, larghezzaStream, ref Width, ref Height);
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(Width, Height));

                if (generateversions)
                {
                    if (filemanage.reslevels >= 1)
                        if (larghezzaStream > larghezza_xs)
                        {
                            RicalcolaDimensioni(altezzaStream, larghezzaStream, ref larghezza_xs, ref altezza_xs);
                            img_xs = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(larghezza_xs, altezza_xs));
                        }
                        else img_xs = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(larghezzaStream, altezzaStream));
                    if (filemanage.reslevels >= 2)
                        if (larghezzaStream > larghezza_sm)
                        {
                            RicalcolaDimensioni(altezzaStream, larghezzaStream, ref larghezza_sm, ref altezza_sm);
                            img_sm = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(larghezza_sm, altezza_sm));
                        }
                        else img_sm = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(larghezzaStream, altezzaStream));
                    if (filemanage.reslevels >= 3)
                        if (larghezzaStream > larghezza_md)
                        {
                            RicalcolaDimensioni(altezzaStream, larghezzaStream, ref larghezza_md, ref altezza_md);
                            img_md = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(larghezza_md, altezza_md));
                        }
                        else img_md = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(larghezzaStream, altezzaStream));
                    if (filemanage.reslevels >= 4)
                        if (larghezzaStream > larghezza_lg)
                        {
                            RicalcolaDimensioni(altezzaStream, larghezzaStream, ref larghezza_lg, ref altezza_lg);
                            img_lg = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(larghezza_lg, altezza_lg));
                        }
                        else img_lg = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(larghezzaStream, altezzaStream));
                    /////////////////////////////////////////  
                }

                switch (System.IO.Path.GetExtension(fileorigine).ToLower())
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
                    EncoderParameters myEncoderParameters = new EncoderParameters(3);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                    myEncoderParameters.Param[2] = new EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, (int)EncoderValue.RenderProgressive);
                    img.Save(pathAnteprime + nomeAnteprima, jgpEncoder, myEncoderParameters);

                    if (generateversions)
                    {
                        if ((!System.IO.File.Exists(pathAnteprime + filename_xs) || replacefile) && img_xs != null)
                            img_xs.Save(pathAnteprime + filename_xs, jgpEncoder, myEncoderParameters);
                        if ((!System.IO.File.Exists(pathAnteprime + filename_sm) || replacefile) && img_sm != null)
                            img_sm.Save(pathAnteprime + filename_sm, jgpEncoder, myEncoderParameters);
                        if ((!System.IO.File.Exists(pathAnteprime + filename_md) || replacefile) && img_md != null)
                            img_md.Save(pathAnteprime + filename_md, jgpEncoder, myEncoderParameters);
                        if ((!System.IO.File.Exists(pathAnteprime + filename_lg) || replacefile) && img_lg != null)
                            img_lg.Save(pathAnteprime + filename_lg, jgpEncoder, myEncoderParameters);
                    }
                }
                else
                {
                    img.Save(pathAnteprime + nomeAnteprima, imgF);
                    if (generateversions)
                    {

                        if ((!System.IO.File.Exists(pathAnteprime + filename_xs) || replacefile) && img_xs != null)
                            img_xs.Save(pathAnteprime + filename_xs, imgF);
                        if ((!System.IO.File.Exists(pathAnteprime + filename_sm) || replacefile) && img_sm != null)
                            img_sm.Save(pathAnteprime + filename_sm, imgF);
                        if ((!System.IO.File.Exists(pathAnteprime + filename_md) || replacefile) && img_md != null)
                            img_md.Save(pathAnteprime + filename_md, imgF);
                        if ((!System.IO.File.Exists(pathAnteprime + filename_lg) || replacefile) && img_lg != null)
                            img_lg.Save(pathAnteprime + filename_lg, imgF);
                    }

                }

                file.Close();
                ret = true;
                if (!System.IO.File.Exists(pathAnteprime + nomeAnteprima))
                    ret = false;
            }
        }
        catch
        { ret = false; }
        return ret;

    }
    public static void RicalcolaDimensioni(int altezzaStream, int larghezzaStream, ref int Width, ref int Height)
    {
        ///////////////////VERSIONE BASE IMMAGINE
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
    }

    public static string ComponiUrlAnteprima(object NomeAnteprima, string CodiceTipologia, string idOfferta, bool noanteprima = false, bool nonpdf = false)
    {
        string ritorno = "";
        string physpath = "";
        if (NomeAnteprima != null)
            if (!NomeAnteprima.ToString().ToLower().StartsWith("http://") && !NomeAnteprima.ToString().ToLower().StartsWith("https://") && !NomeAnteprima.ToString().ToLower().StartsWith("https://"))
            {
                if (CodiceTipologia != "" && idOfferta != "")
                    if ((NomeAnteprima.ToString().ToLower().EndsWith("jpg") || NomeAnteprima.ToString().ToLower().EndsWith("gif") || NomeAnteprima.ToString().ToLower().EndsWith("png")))
                    {
                        ritorno = WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + CodiceTipologia + "/" + idOfferta.ToString();
                        physpath = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + CodiceTipologia + "\\" + idOfferta.ToString();
                        //Così ritorno l'immagine non di anteprima ma quella pieno formato
                        if (NomeAnteprima.ToString().StartsWith("Ant"))
                            ritorno += "/" + NomeAnteprima.ToString().Remove(0, 3);
                        else
                            ritorno += "/" + NomeAnteprima.ToString();
                        //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME
                        string anteprimaimmagine = ScalaImmagine(ritorno, null, physpath);
                        if (anteprimaimmagine != "" && !noanteprima) ritorno = anteprimaimmagine;
                        //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME
                    }
                    else if (!nonpdf)
                        ritorno = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";
            }
            else
                ritorno = NomeAnteprima.ToString();
        return ritorno;
    }
    public static string ScalaImmagine(string pathFileorigine, HttpServerUtility Server, string percorsoFisicoorigine = "")
    {
        //string percorsoviranteprime = WelcomeLibrary.STATIC.Global.PercorsoComune;
        //string percorsofisanteprime = WelcomeLibrary.STATIC.Global.percorsoFisicoComune;
        int posname = pathFileorigine.LastIndexOf('/');
        if (posname < 0) return "";

        string percorsoviranteprime = pathFileorigine.Substring(0, posname);
        string percorsofisanteprime = percorsoFisicoorigine;
        if (Server != null) percorsofisanteprime = Server.MapPath(percorsoviranteprime);
        string NomeAnteprima = pathFileorigine.Substring(posname + 1);
        string percorsofisicofile = percorsofisanteprime + "\\" + NomeAnteprima;
        if (Server != null) Server.MapPath(pathFileorigine).ToString();

        if (NomeAnteprima.ToString().StartsWith("Ant"))
            NomeAnteprima = NomeAnteprima.ToString().Remove(0, 3);
        NomeAnteprima = "Ant" + NomeAnteprima;

        string percorsoanteprimagenerata = "";
        if (filemanage.CreaAnteprima(percorsofisicofile, 450, 450, percorsofisanteprime + "\\", NomeAnteprima, false, true)) //qui con true puoi forzare la rigenerazione di tutte le anteprime anche se già generate
            percorsoanteprimagenerata = percorsoviranteprime + "/" + NomeAnteprima;
        return percorsoanteprimagenerata;
    }
    public static string SelectImageByResolution(string pathimmagine, string viewportw)
    {
        // -xs 0> <576px  | -sm  576> <768  | -md 769< <992  |  -lg >992 <1200  | -xl(no estensione) >1200
        string retname = pathimmagine;
        int actwidth = 0;
        if(!string.IsNullOrEmpty(pathimmagine) && pathimmagine.LastIndexOf('.')!=-1)
        if (int.TryParse(viewportw, out actwidth))
        {
            string modifier = "";
            if (actwidth <= 576 && filemanage.reslevels >= 1)
                modifier = "-xs";
            else if (actwidth > 576 && actwidth <= 768 && filemanage.reslevels >= 2)
                modifier = "-sm";
            else if (actwidth > 768 && actwidth <= 992 && filemanage.reslevels >= 3)
                modifier = "-md";
            else if (actwidth > 992 && actwidth <= 1200 && filemanage.reslevels >= 4)
                modifier = "-lg";
            int extpos = pathimmagine.LastIndexOf('.');
            retname = pathimmagine.Insert(extpos, modifier);
        }
        return retname;

    }

}