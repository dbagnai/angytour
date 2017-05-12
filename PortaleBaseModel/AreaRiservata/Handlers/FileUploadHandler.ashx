<%@ WebHandler Language="C#" Class="FileUploadHandler" %>

using System;
using System.Web;

public class FileUploadHandler : IHttpHandler
{

    public string output = "";

    public void ProcessRequest(HttpContext context)
    {
        string idrecord = "";
        string nomefile = "";
        string Tipologia = "";
        string Descrizione = "";
        string Azione = "";

        if (context.Request.QueryString["id"] != null)
            idrecord = context.Request.QueryString["id"].ToString();
        if (context.Request.QueryString["nomefile"] != null)
            nomefile = context.Request.QueryString["nomefile"].ToString();
        if (context.Request.QueryString["Tipologia"] != null)
            Tipologia = context.Request.QueryString["Tipologia"].ToString();
        if (context.Request.QueryString["Descrizione"] != null)
            Descrizione = context.Request.QueryString["Descrizione"].ToString();
        if (context.Request.QueryString["Azione"] != null)
            Azione = context.Request.QueryString["Azione"].ToString();

        if (idrecord.Trim() == "" || Tipologia.Trim() == "")
        {
            output = "Post non selezionato correttamente!";
            context.Response.ContentType = "text/plain";
            context.Response.Write(output);
            return;
        }
        if (Azione == "Inserisci")
        {
            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection files = context.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    // string fname = context.Server.MapPath("~/uploads/" + file.FileName);    
                    // file.SaveAs(fname);    
                    CaricaFile(file, Descrizione, idrecord, Tipologia);
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(output);
            }
            else
            {
                output = "Seleziona file da caricare";
                context.Response.ContentType = "text/plain";
                context.Response.Write(output);
            }
        }
        if (Azione == "Cancella")
        {
            EliminaFile(idrecord, nomefile, Tipologia);
            context.Response.ContentType = "text/plain";
            context.Response.Write(output);
        }
        if (Azione == "Modifica")
        {
            // da finire ....
            context.Response.ContentType = "text/plain";
            context.Response.Write(output);
        }
    }


    #region CARICAMENTO ALLEGATI ALLEGATI


    //[System.Web.Services.WebMethod]
    //public static string UploadFileFunction(string id,string data)
    //{
    //    return AggiornaPostExecute();
    //}
    //public static string AggiornaPostExecute()
    //{
    //    StringBuilder sb = new StringBuilder();
    //try
    //{
    //    foreach (string file in Request.Files)
    //    {
    //        var fileContent = Request.Files[file];
    //        if (fileContent != null && fileContent.ContentLength > 0)
    //        {
    //            // get a stream
    //            var stream = fileContent.InputStream;
    //            // and optionally write the file to disk
    //            var fileName = Path.GetFileName(file);
    //            var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
    //            using (var fileStream = File.Create(path))
    //            {
    //                stream.CopyTo(fileStream);
    //            }
    //        }
    //    }
    //}
    //catch (Exception)
    //{
    //    //Response.StatusCode = (int)HttpStatusCode.BadRequest;
    //    //return Json("Upload failed");
    //}
    //    return sb.ToString();

    //}

    private void ModificaFile(string idrecord, string nomefile, string Descrizione)
    {
        // file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);
        // file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);
        try
        {
            WelcomeLibrary.DAL.offerteDM offDM = new WelcomeLibrary.DAL.offerteDM();

            int i = 0;
            int.TryParse(idrecord, out i);
            bool ret = offDM.modificaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, i, nomefile, Descrizione);
        }
        catch (Exception errins)
        {
            output = errins.Message;
        }

        output = "Descrizone file Modificata Correttamente";

    }
    private void EliminaFile(string idrecord, string nomefile, string Tipologia)
    {
        // file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);
        //Controlliamo se ho selezionato un record
        if (idrecord == null || idrecord == "")
        {
            output = "Selezionare un elemento per cancellare il file";
            return;
        }

        if (nomefile == null || nomefile == "")
        {
            output = "Selezionare una foto da cancellare";
            return;
        }
        int idSelected = 0;
        if (!Int32.TryParse(idrecord, out idSelected))
        {
            output = "Selezionare un elemento per cancellare la foto";
            return;
        }
        //Ricarichiamo l'offerta selezionata dal db
        //Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected);
        WelcomeLibrary.DAL.offerteDM offDM = new WelcomeLibrary.DAL.offerteDM();

        //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
        string pathDestinazione = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + Tipologia + "\\" + idrecord;
        //-------------------------------------
        //Eliminiamo il file se presente
        //-------------------------------------
        try
        {
            try
            {
                bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, nomefile, "", pathDestinazione);
                output = "File Eliminato!";
            }
            catch (Exception errodel)
            {
                output = errodel.Message;
            }

        }
        catch (Exception errore)
        {
            output += " " + errore.Message;
        }
    }
    private void CaricaFile(HttpPostedFile file, string DescrizioneFile, string idrecord, string Tipologia)
    {
        try
        {
            //Controlliamo se ho selezionato un record
            if (idrecord == null || idrecord == "")
            {
                output = "Selezionare un elemento per associare il file";
                return;
            }
            int idSelected = 0;
            if (!Int32.TryParse(idrecord, out idSelected))
            {
                output = "Selezionare un elemento per associare il file";
                return;
            }

            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
            string pathDestinazione = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + Tipologia + "\\" + idrecord;
            if (!System.IO.Directory.Exists(pathDestinazione))
                System.IO.Directory.CreateDirectory(pathDestinazione);
            WelcomeLibrary.DAL.offerteDM offDM = new WelcomeLibrary.DAL.offerteDM();

            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (file.ContentLength > 10000000)
            {

                output = "Il File non può essere caricato perché supera 10MB!";
            }
            else
            {
                //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER

                string NomeCorretto = file.FileName.Replace("+", "").Substring(file.FileName.LastIndexOf("\\") + 1);
                NomeCorretto = NomeCorretto.Replace("%", "");
                NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
                NomeCorretto = NomeCorretto.Replace(" ", "-").ToLower();
                //string NomeCorretto = Server.HtmlEncode(FotoUpload1.FileName);
                if (System.IO.File.Exists(pathDestinazione))
                {
                    output = ("Il File non può essere caricato perché già presente sul server!");
                }
                else
                {
                    Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    if (file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                    {
                        int maxheight = 800;
                        int maxwidth = 1000;
                        bool ridimensiona = true;
                        //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                        if (ResizeAndSave(file.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                        {
                            //Creiamo l'anteprima Piccola per usi in liste
                            this.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 350, 350, pathDestinazione + "\\", "Ant" + NomeCorretto);
                            //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
                            //I DATI PER RINTRACCIARE LA FOTO-->SCHEMA E VALORI
                            try
                            {
                                try
                                {
                                    bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                }
                                catch (Exception errins)
                                {
                                    output = errins.Message;
                                }

                                output += "Foto Inserita Correttamente";
                            }
                            catch (Exception error)
                            {
                                //CANCELLO LA FOTO UPLOADATA
                                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                if (System.IO.File.Exists(pathDestinazione + "\\" + "Ant" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + "Ant" + NomeCorretto);
                                //AGGIORNO IL DETAILSVIEW
                                output = error.Message;
                            }
                        }
                        else { output += ("La foto non è stata caricata! (Problema nel caricamento)"); }
                    }
                    else if (file.ContentType == "application/pdf")
                    {

                        //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                        file.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                        try
                        {
                            try
                            {
                                bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                            }
                            catch (Exception errins)
                            {
                                output = errins.Message;
                            }

                            output += "Pdf Inserito Correttamente";

                        }
                        catch (Exception error)
                        {
                            //CANCELLO IL FILE UPLOADATO
                            if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                            //AGGIORNO IL DETAILSVIEW
                            output = error.Message;
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
                                bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                            }
                            catch (Exception errins)
                            {
                                output = errins.Message;
                            }

                            output += "Documento Inserito Correttamente";

                        }
                        catch (Exception error)
                        {
                            //CANCELLO IL FILE UPLOADATO
                            if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                            //AGGIORNO IL DETAILSVIEW
                            output = error.Message;
                        }
                    }
                }
            }

        }
        catch (Exception errorecaricamento)
        {
            output += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                output += errorecaricamento.InnerException.Message;

        }
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
                    //img_filtrata = WelcomeLibrary.UF.ImageProcessing.applicaBrightness(img_filtrata, 0.03);
                    //img_filtrata = ImageProcessing.applicaContrast(img_filtrata, 0.75);
                    //img_filtrata = ImageProcessing.applicaAdaptiveSmoothing(img_filtrata);
                    //img_filtrata = ImageProcessing.applicaConservativeSmoothing(img_filtrata);
                    //img_filtrata = ImageProcessing.applicaHSLFilter(img_filtrata, 0.87, 0.075);
                    //img_filtrata = ImageProcessing.applicaGaussianBlur(img_filtrata, 1, 5);
                    //img_filtrata = ImageProcessing.applicaMediano(img_filtrata, 4);
                    // ImageProcessing.NoiseRemoval(img_filtrata);
                    //img_filtrata = ImageProcessing.MeanFilter(img_filtrata, 2);
                    img_filtrata = WelcomeLibrary.UF.ImageProcessing.applicaResizeBilinear(img_filtrata, Width, Height); //resisze
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
                            System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(imgF);
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            // Create an EncoderParameters object.
                            // An EncoderParameters object has an array of EncoderParameter objects. In this case, there is only one EncoderParameter object in the array.
                            System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                            System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 85L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
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
    private System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
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
            output = ("Anteprima Allegato non salvata correttamente!");

    }


    #endregion



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}  