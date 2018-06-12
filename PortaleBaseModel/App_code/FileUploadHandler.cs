
using System;
using System.Web;
using System.Drawing.Imaging;
using WelcomeLibrary.UF;

public class FileUploadHandler : IHttpHandler
{

    public static string output = "";

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
                    output = filemanage.CaricaFile(HttpContext.Current.Server, file, Descrizione, idrecord, Tipologia, "");
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
            output = filemanage.EliminaFile(HttpContext.Current.Server, idrecord, Tipologia, nomefile);
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

     

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}