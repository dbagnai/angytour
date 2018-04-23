using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using WelcomeLibrary.UF;
using System.Collections.Generic;

public class HandlerImage : IHttpHandler, IRequiresSessionState
{
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void ProcessRequest(HttpContext context)
    {

        // Setup the response settings
        context.Response.ContentType = "image/jpeg"; //imposto il Contenuto della risposta su jpeg
        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.Cache.SetMaxAge(TimeSpan.FromSeconds(690000));
        context.Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(690000));
        context.Response.BufferOutput = false; //disattivo il buffer di output
        string pathImg = context.Request.Path;

        //variabile per contenere il nome dell'immagine ( devo estrarlo )
        string nameImg = string.Empty;

        nameImg = context.Request.Url.Segments[context.Request.Url.Segments.Length - 1];
        // nameImg = context.Request.Path.Substring(context.Request.Path.LastIndexOf('/') + 1);

        //variabile per la dimensione (width) dell'immagine
        //dopo il controllo assegno la dimensione presente in querystring
        int targetSize = 0;
        if (context.Request.QueryString["vw"] != null && context.Request.QueryString["vw"] != string.Empty)
            targetSize = Convert.ToInt16(context.Request.QueryString["vw"]);
        int width = targetSize;
        int height = targetSize;

        //if (targetSize >= 1 && targetSize <= 399)
        //{
        //    width = 300;
        //    height = 250;

        //}
        //else if (targetSize >= 400 && targetSize <= 769)
        //{
        //    width = 800;
        //    height = 450;
        //}
        //else if (targetSize >= 800 && targetSize <= 1180)
        //{
        //    width = 1100;
        //    height = 650;
        //}
        //else if (targetSize >= 1181 && targetSize <= 1300)
        //{
        //    width = 1200;
        //    height = 800;
        //}
        //else if (targetSize >= 1300 && targetSize <= 1600)
        //{
        //    width = 1600;
        //    height = 1400;
        //}
        //else if (targetSize >= 1600)
        //{
        //    width = 1800;
        //    height = 1600;
        //}



        //inizializzo il path e lo stream dell'immagine
        Image imgStream = null;
        try
        {

            //Se l'immagine è un riferimento dall'esterno
            //if (pathImg.ToString().ToLower().StartsWith("http://") || pathImg.ToString().ToLower().StartsWith("https://"))
            //{
            //    try
            //    {
            //        System.Net.WebRequest req = System.Net.WebRequest.Create(pathImg);
            //        System.Net.WebResponse response = req.GetResponse();
            //        using (System.IO.Stream stream = response.GetResponseStream())
            //        {
            //            imgStream = System.Drawing.Image.FromStream(stream));
            //        }
            //    }
            //    catch
            //    { }
            //}

            //controllo se il file immagine esiste       
            if (File.Exists(context.Server.MapPath(pathImg)))
            {
                //creo l'immagine dal file
                imgStream = System.Drawing.Image.FromFile(context.Server.MapPath(pathImg));
            }
            else
            {
                //imposto il path a quello di default (un'immagine di default NO-IMAGE)
                pathImg = "~/images/dummyimage.png";//ManagerSettings.GetNoImages;
                imgStream = System.Drawing.Image.FromFile(context.Server.MapPath(pathImg));
            }
            // Se nella querystring non era specificata la dimensione assegno quella originale
            if (targetSize == 0)
            {
                width = imgStream.Width;
                height = imgStream.Height;
            }
        }

        catch
        {
            //creo l'immagine col file no-image di default
            pathImg = "~/images/dummyimage.png";//ManagerSettings.GetNoImages;
            imgStream = System.Drawing.Image.FromFile(context.Server.MapPath(pathImg));
        }

        //creo una nuova bitmap ridimensionandolo con le misure passate nella query string
        Bitmap img = new Bitmap(HelperImage.Resize(imgStream, width, height, true));

        //salvo l'immagine ridimensionata nell'output della risposta 
        System.Drawing.Imaging.ImageFormat imgF = null;
        switch (System.IO.Path.GetExtension(nameImg).ToLower())
        {
            case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif;  context.Response.ContentType = "image/GIF"; break;
            case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; context.Response.ContentType = "image/png"; break;
            case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; context.Response.ContentType = "image/bmp"; break;
            default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
        }
        if (imgF == System.Drawing.Imaging.ImageFormat.Jpeg)
        {
            // Create an Encoder object based on the GUID for the Quality parameter category.
            ImageCodecInfo jgpEncoder = GetEncoder(imgF);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter objects. In this case, there is only one EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 95L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
            myEncoderParameters.Param[0] = myEncoderParameter;
            img.Save(context.Response.OutputStream, jgpEncoder, myEncoderParameters);
        }
        else
            img.Save(context.Response.OutputStream, imgF);
    }
    private static ImageCodecInfo GetEncoder(ImageFormat format)
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
