using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


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
        context.Response.BufferOutput = false; //disattivo il buffer di output

        //variabile per contenere il nome dell'immagine
        string nameImg = string.Empty;
         
        if (context.Request.QueryString["NameImg"] != null && context.Request.QueryString["NameImg"] != string.Empty)

            nameImg = context.Request.QueryString["NameImg"]; //dopo il check, assegno il nome presente in querystring

        //variabile per la dimensione (width) dell'immagine
        int targetSize = 0;

        if (context.Request.QueryString["Size"] != null && context.Request.QueryString["Size"] != string.Empty)

            targetSize = Convert.ToInt16(context.Request.QueryString["Size"]);  //dopo il controllo assegno la dimensione presente in querystring

        //inizializzo il path e lo stream dell'immagine
        string pathImg = context.Request.Path;
        Image imgStream = null;

        try
        {
            //assegno il path dell'immagine
            //pathImg =  ManagerSettings.GetVirtualPathImages + nameImg;

            //controllo se il file immagine esiste
            if (File.Exists(context.Server.MapPath(pathImg)))
            {
                //creo l'immagine dal file
                imgStream = System.Drawing.Image.FromFile(context.Server.MapPath(pathImg));
            }
            //else
            //{
            //    //imposto il path a quello di default (un'immagine di default NO-IMAGE)
            //    pathImg = "";//ManagerSettings.GetNoImages;
            //    imgStream = System.Drawing.Image.FromFile(context.Server.MapPath(pathImg));
            //}
        }

        catch
        {
            //creo l'immagine col file no-image di default
            pathImg = "";//ManagerSettings.GetNoImages;
            imgStream = System.Drawing.Image.FromFile(context.Server.MapPath(pathImg));
        }

        // Se nella querystring non era specificata la dimensione assegno quella originale
        if (targetSize == 0)
            targetSize = imgStream.Width;

        //creo una nuova bitmap ridimensionandolo con le misure passate nella query string
        //Bitmap img = new Bitmap(HelperImage.ResizeImageFile(imgStream, targetSize));
        //Bitmap img = new Bitmap("");

        //salvo l'immagine ridimensionata nell'output della risposta in formato jpeg
        //  img.Save(context.Response.OutputStream, ImageFormat.Jpeg);
    }
}
