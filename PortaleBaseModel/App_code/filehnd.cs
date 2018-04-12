
using System;
using System.Web;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;

public class Filehnd : IHttpHandler
{

    public Dictionary<string, string> parseparams(HttpContext context)
    {
        Dictionary<string, string> pars = new Dictionary<string, string>();
        bool isPost = false;
        isPost = context.Request.HttpMethod.ToUpper() == "POST";
        bool ismultipart = false;
        if (context.Request.ContentType.ToLower().Contains("multipart/form-data")) ismultipart = true;

        if (isPost && !ismultipart)
            pars = HandlerHelper.GetPostParams(context);
        foreach (var item in context.Request.Params.Keys)
        {
            string szKey = item.ToString();
            if (!pars.ContainsKey(szKey))
                pars.Add(szKey, context.Request.Params[szKey].ToString());
        }

        return pars;
    }


    public void ProcessRequest(HttpContext context)
    {
        string result = "";
        context.Response.ContentType = "text/plain";
        try
        {
            /* prendiamo i parametri */
            Dictionary<string, string> pars = parseparams(context);

            System.Text.StringBuilder sbError = new System.Text.StringBuilder();
            System.Text.StringBuilder sbSaved = new System.Text.StringBuilder();

            // vediamo se c'è da fare il refresh
            if (pars.ContainsKey("q"))
            {
                string q = pars["q"].ToLower();
                switch (q)
                {
                    case "delete":
                        result = ("OK|Azione " + q + " eseguita.");
                        break;
                    case "store":
                        storeFile(context, pars, sbSaved, sbError);
                        manageResponse(context, sbSaved, sbError);
                        // result = ("OK|Azione " + q + " eseguita.");

                        break;
                    case "":
                        break;
                    default:
                        result = ("ALERT|Parametro richiesto " + q + " non gestito.");
                        return;
                        break;

                }
            }
        }
        catch (Exception ex)
        {
            string er = ex.Message;
            result = er;
            context.Response.StatusCode = 400;
        }
        context.Response.Write(result);
    }


    private void manageResponse(HttpContext context, System.Text.StringBuilder sbSaved, System.Text.StringBuilder sbError)
    {

        string szError = sbError.ToString();
        if (!string.IsNullOrEmpty(szError))
        {
            context.Response.Write("ALERT|" + szError);
            return;
        }

        string szSaved = sbSaved.ToString().TrimEnd('|');
        context.Response.Write(szSaved);
        return;
    }



    private void storeFile(HttpContext context, Dictionary<string, string> pars,
            System.Text.StringBuilder sbSaved, System.Text.StringBuilder sbError)
    {

        foreach (string s in context.Request.Files)
        {

            HttpPostedFile file = context.Request.Files[s];
            //  int fileSizeInBytes = file.ContentLength;
            string fileName = file.FileName;
            string fileExtension = Path.GetExtension(fileName);// file.ContentType;
            System.Diagnostics.Debug.Print("fname : " + fileName);
            // salviamo il file
            if (!string.IsNullOrEmpty(fileName))
            {
                try
                {

                    string file_to_save = fileName; //"MyPHOTO_" + numFiles.ToString() + fileExtension;
                    string uploadPath = pars.ContainsKey("filepath") ? pars["filepath"].Replace("/", "\\") : "";
                    string idrecord = pars.ContainsKey("idselected") ? pars["idselected"].Trim() : "";
                    string tipologia = pars.ContainsKey("tipologia") ? pars["tipologia"].Trim() : "";


                    string ret = filemanage.CaricaFile(HttpContext.Current.Server, file, "", idrecord, tipologia, "");
                    if (string.IsNullOrEmpty(ret))
                        sbSaved.Append(fileName + " uploaded  | ");
                    else
                        sbError.AppendLine("Errore salvataggio file  : " + fileName + " Details: " + ret);
                }
                catch (Exception ex)
                {
                    //  Utility.Logging.Error("ufx/file_saveas/" + fileName, ex);
                    sbError.AppendLine("Errore salvataggio file in FS : " + fileName + " Details: " + ex.Message);
                }
            }
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