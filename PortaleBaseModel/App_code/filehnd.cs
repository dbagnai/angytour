
using System;
using System.Web;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Web.SessionState;

public class jformdatapost
{
    public string ididofferta { set; get; }
    public string tipologia { set; get; }
    public string titolo { set; get; }
    public string sottotitolo { set; get; }
    public string descrizione { set; get; }
    public string indirizzo { set; get; }
    public string email { set; get; }
    public string telefono { set; get; }
    public string consenso { set; get; }
    public string consenso1 { set; get; }
    public string consenso2 { set; get; }
}

public class Filehnd : IHttpHandler, IRequiresSessionState
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
                    case "insertpost":
                        jformdatapost jdata = new jformdatapost();
                        string jformdatapost = pars.ContainsKey("formdata") ? pars["formdata"] : "";
                        if (jformdatapost != "")
                            jdata = Newtonsoft.Json.JsonConvert.DeserializeObject<jformdatapost>(jformdatapost);
                        //Mappiamo i dati in un oggetto offerrte da inserire
                        Offerte item = new Offerte();
                        offerteDM offDM = new offerteDM();
                        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();
                        item.Archiviato = true;
                        item.CodiceTipologia = html.Convert(jdata.tipologia).Trim();
                        item.DenominazioneI = html.Convert(jdata.titolo).Trim() + "\r\n" + html.Convert(jdata.sottotitolo).Trim();
                        item.DescrizioneI = html.Convert(jdata.descrizione).Trim();
                        item.Indirizzo = html.Convert(jdata.indirizzo).Trim();
                        item.Email = html.Convert(jdata.email).Trim();
                        item.Telefono = html.Convert(jdata.telefono).Trim();
                        item.DataInserimento = System.DateTime.Now;
                        //INSERIAMO IL POST COME ARCHIVIATO ( DA VEDERE SE FARE PRIMA CONTROLLI E PULIZIA )
                        offDM.InsertOffertaCollegata(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item);
                        offDM.InsertOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item);
                        string itemid = item.Id.ToString();
                        //Caricamento files in allegato se presenti nella reqeuest
                        foreach (string s in context.Request.Files)
                        {
                            HttpPostedFile file = context.Request.Files[s];
                            result = filemanage.CaricaFile(HttpContext.Current.Server, file, "", itemid, item.CodiceTipologia, "");
                        }
                        //Se presenti file nella cartella sessione di caricamento li collega al post e li cancella
                        string percorsofilesuploaded = WelcomeLibrary.STATIC.Global.PercorsoComune + "/_uploads/" + context.Session.SessionID;
                        percorsofilesuploaded = context.Server.MapPath(percorsofilesuploaded);
                        if (Directory.Exists(percorsofilesuploaded))
                        {
                            DirectoryInfo di = new DirectoryInfo(percorsofilesuploaded);
                            FileInfo[] fi = di.GetFiles();
                            foreach (FileInfo f in fi)
                            {
                                string retstring = filemanage.CaricaFile(HttpContext.Current.Server, percorsofilesuploaded, f.Name, item.CodiceTipologia, itemid, "");
                            }
                            di.Delete(true); //cancella la cartella ed i files
                        }
                        ///////////////////////////////////////////////////////////////////////
                        result = "";

                        //INVIAMO LA MAIL DI AVVISO INSERIMENTO
                        string testomail = "Verifica e controlla la richiesta di inserimento per il ristorante : " + itemid + "<br/>";
                        testomail += "L'utente in oggetto ha autorizzato al trattametno dei dati per la pubblicazione sul portale<br/>";
                        if (jdata.consenso1 == "true")
                            testomail += "L'utente in oggetto ha autorizzato al trattametno dei dati Per attività di marketing diretto ed indiretto e ricerche di mercato<br/>";
                        if (jdata.consenso2 == "true")
                            testomail += "L'utente in oggetto ha autorizzato al trattametno dei dati Per attività di profilazione al fine di migliorare l'offerta di prodotti e servizi<br/>";

                        string soggetto = "Richiesta inserimento ristorante " + html.Convert(jdata.titolo);
                        string destinatario = ConfigManagement.ReadKey("Nome");
                        string destinatarioemail = ConfigManagement.ReadKey("Email");
                        Utility.invioMailGenerico("Mittente", item.Email, soggetto, testomail, destinatarioemail, destinatario);
                        break;
                    case "deletefilebypath":
                        Dictionary<string, List<string>> valoriritorno1 = new Dictionary<string, List<string>>();
                        valoriritorno1.Add("messages", new List<string>());
                        valoriritorno1.Add("files", new List<string>());

                        string link = pars.ContainsKey("data") ? pars["data"] : "";
                        if (!string.IsNullOrEmpty(link) && link.LastIndexOf("/") > -1)
                        {
                            string pvirt = WelcomeLibrary.STATIC.Global.PercorsoComune + "/_uploads/" + context.Session.SessionID;
                            string pfis = pvirt;
                            pfis = context.Server.MapPath(pfis);
                            string filename = link.Substring(link.LastIndexOf("/"));
                            FileInfo fidel = new FileInfo(pfis + "\\" + filename);
                            fidel.Delete();
                            result = pfis + "\\" + filename;

                            DirectoryInfo tmpdir1 = new DirectoryInfo(pfis);
                            foreach (FileInfo fi in tmpdir1.GetFiles())
                            {
                                valoriritorno1["files"].Add(CommonPage.ReplaceAbsoluteLinks(pvirt + "/" + fi.Name));
                            }
                        }
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(valoriritorno1, Newtonsoft.Json.Formatting.Indented);
                        break;
                    case "uploadfileinfolder": //carico il file passatiin una directory temporanea in base alla sessione attuale
                        string folder = pars.ContainsKey("folder") ? pars["folder"] : "";
                        string vpercorsotemp = WelcomeLibrary.STATIC.Global.PercorsoComune + "/_uploads/" + context.Session.SessionID;
                        string percorsotemp = vpercorsotemp;
                        if (!string.IsNullOrEmpty(folder))
                            percorsotemp = folder;
                        percorsotemp = context.Server.MapPath(percorsotemp);
                        if (!Directory.Exists(percorsotemp))
                            Directory.CreateDirectory(percorsotemp);
                        //Caricamento files nella cartella temporanea
                        Dictionary<string, List<string>> valoriritorno = new Dictionary<string, List<string>>();
                        valoriritorno.Add("messages", new List<string>());
                        valoriritorno.Add("files", new List<string>());
                        foreach (string s in context.Request.Files)
                        {
                            HttpPostedFile file = context.Request.Files[s];
                            if (file.ContentLength > 20000000)
                            {
                                valoriritorno["messages"].Add(file.FileName + " :Il File non può essere caricato perché supera 20MB!</br>");
                                continue;
                            }
                            if (file.ContentType == "image/jpeg" || file.ContentType == "image/pjpeg" || file.ContentType == "image/gif" || file.ContentType == "image/png")
                            {
                                string fname = (percorsotemp + "\\" + file.FileName);
                                file.SaveAs(fname);
                            }
                            else
                                valoriritorno["messages"].Add(file.FileName + " : formato non consentito !</br>");
                        }
                        DirectoryInfo tmpdir = new DirectoryInfo(percorsotemp);
                        foreach (FileInfo fi in tmpdir.GetFiles())
                        {
                            valoriritorno["files"].Add(CommonPage.ReplaceAbsoluteLinks(vpercorsotemp + "/" + fi.Name));
                        }
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(valoriritorno, Newtonsoft.Json.Formatting.Indented);
                        break;
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
                        sbSaved.Append(fileName + "| uploaded  | ");
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