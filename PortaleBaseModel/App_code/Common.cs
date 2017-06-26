using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.Profile;
using System.Xml;
using System.Data.OleDb;
using System.Text;

/// <summary>
/// This Page class is common to all sample pages and exists as a place to
/// implement common functionality
/// </summary>
public class CommonPage : Page
{

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string SvuotaSessione(string campo1)
    {
        string ret = "";
        HttpContext.Current.Session.Clear();
        return ret;
    }


    //public TipologiaCollection ElencoTipologie = FunzioniUtilità.Tipologie;
    //public ProvinceCollection ElencoProvince = FunzioniUtilità.ElencoProvince;
    //public ImmobileDM immDMpage = new ImmobileDM();
    public contenutiDM conDM = new contenutiDM();
    public offerteDM offDM = new offerteDM();
    // public annunciDM annDM = new annunciDM();


    private static string _deflanguage;
    public static string deflanguage
    {
        get { return ConfigManagement.ReadKey("deflanguage"); }
    }
    private string _Email;
    public string Email
    {
        get { return ConfigManagement.ReadKey("Email"); }
    }
    private string _Nome;
    public string Nome
    {
        get { return ConfigManagement.ReadKey("Nome"); }
    }

    public struct PrezziStruct
    {
        private int _fascia;
        public int Fascia
        {
            get { return _fascia; }
            set { _fascia = value; }
        }

        private string _descrizione;
        public string Descrizione
        {
            get { return _descrizione; }
            set { _descrizione = value; }
        }

        private string _lingua;
        public string Lingua
        {
            get { return _lingua; }
            set { _lingua = value; }
        }
        private double _startprice;

        public double startprice
        {
            get { return _startprice; }
            set { _startprice = value; }
        }

        private double _endprice;

        public double endprice
        {
            get { return _endprice; }
            set { _endprice = value; }
        }
    }
    public static Control FindControlRecursive(Control rootControl, string controlID)
    {
        if (rootControl.ID == controlID) return rootControl;

        foreach (Control controlToSearch in rootControl.Controls)
        {
            Control controlToReturn =
                FindControlRecursive(controlToSearch, controlID);
            if (controlToReturn != null) return controlToReturn;
        }
        return null;
    }
    public static void CreaNuovaSessione(System.Web.SessionState.HttpSessionState sessione, HttpRequest richiesta)
    {
        sessione.Abandon();
        richiesta.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

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
        if (CreaAnteprima(percorsofisicofile, 450, 450, percorsofisanteprime + "\\", NomeAnteprima, false))
            percorsoanteprimagenerata = percorsoviranteprime + "/" + NomeAnteprima;
        return percorsoanteprimagenerata;
    }


    /// <summary>
    /// I path da passare sono percorsi fisici sul server!
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="Altezza"></param>
    /// <param name="Larghezza"></param>
    /// <param name="pathAnteprime"></param>
    /// <param name="nomeAnteprima"></param>
    /// <returns></returns>
    public static bool CreaAnteprima(string fileorigine, int Altezza, int Larghezza, string pathAnteprime, string nomeAnteprima, bool replacefile = false)
    {
        bool ret = false;
        try
        {
            if (System.IO.File.Exists(pathAnteprime + nomeAnteprima) && !replacefile)
                return true;
            //System.IO.File.Exists(PathTempAnteprime);
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
                if (altezzaStream <= larghezzaStream)
                    Altezza = Convert.ToInt32((double)Larghezza / (double)larghezzaStream * (double)altezzaStream);
                else
                    Larghezza = Convert.ToInt32((double)Altezza / (double)altezzaStream * (double)larghezzaStream);
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(Larghezza, Altezza));
                switch (System.IO.Path.GetExtension(fileorigine).ToLower())
                {
                    case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif; break;
                    case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; break;
                    case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; break;

                    default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                }
                img.Save(pathAnteprime + nomeAnteprima, imgF);
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
    public static string ComponiUrlAnteprima(object NomeAnteprima, string CodiceTipologia, string idOfferta, bool noanteprima = false)
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
                        string anteprimaimmagine = CommonPage.ScalaImmagine(ritorno, null, physpath);
                        if (anteprimaimmagine != "" && !noanteprima) ritorno = anteprimaimmagine;
                        //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME
                    }
                    else
                        ritorno = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";
            }
            else
                ritorno = NomeAnteprima.ToString();

        return ritorno;
    }


    public static string ControlloDotDot(object NomeAnteprima, string classe)
    {
        string ret = classe;
        if (NomeAnteprima == null || NomeAnteprima == "")
            ret = "";
        return ret;
    }
    public string ReplaceFirstOccurrence(string original, string oldValue, string newValue, int startindex)
    {
        if (String.IsNullOrEmpty(original))
            return String.Empty;
        if (String.IsNullOrEmpty(oldValue))
            return original;
        if (String.IsNullOrEmpty(newValue))
            newValue = original;
        int loc = original.IndexOf(oldValue, startindex);
        return original.Remove(loc, oldValue.Length).Insert(loc, newValue);
    }
    public static string ReplaceAbsoluteLinks(string testo)
    {

        return testo.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
    }

    public static string AppendModTime(HttpServerUtility server, string filepath)
    {
        //DateTime creation = System.IO.File.GetCreationTime(@"C:\test.txt");
        DateTime modification = System.IO.File.GetLastWriteTime(server.MapPath(filepath));
        return "?v=" + modification.ToString("ddMMyyHHmmss");
    }

    /// <summary>
    /// SPOSTA IL VIEWSTATE IN FONDO ALLA PAGINA PER 
    /// </summary>
    /// <param name="writer"></param>
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
        base.Render(htmlWriter);
        string html = stringWriter.ToString();
        int StartPoint = html.ToLower().IndexOf("<input type=\"hidden\" name=\"__viewstate\"");
        if (StartPoint >= 0)
        {
            int EndPoint = html.IndexOf("/>", StartPoint) + 2;
            string viewstateInput = html.Substring(StartPoint, EndPoint - StartPoint);
            html = html.Remove(StartPoint, EndPoint - StartPoint);
            int FormEndStart = html.IndexOf("</form>");// -1;
            if (FormEndStart >= 0)
            {
                html = html.Insert(FormEndStart, "<div style=\"display:none\">" + viewstateInput + "</div>");
            }
        }
        writer.Write(html);
    }


    //public static string ComponiUrl(object NomeAnteprima, string CodiceOfferta, string idOfferta)
    //{

    //    string ritorno = "";
    //    if (NomeAnteprima != null && CodiceOfferta != "" && idOfferta != "")
    //    {
    //        ritorno = WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + CodiceOfferta + "/" + idOfferta;
    //        if (NomeAnteprima.ToString().ToLower().StartsWith("ant"))
    //            NomeAnteprima = NomeAnteprima.ToString().Remove(0, 3);
    //        ritorno += "/" + NomeAnteprima.ToString();
    //    }
    //    return ritorno;

    //}
    public static bool ControlloVideo(object NomeAnteprima)
    {
        bool ret = false;
        //"http://www.youtube.com/embed/Z9lwY9arkj8"

        if (NomeAnteprima == null || NomeAnteprima == "")
            ret = true;
        return ret;
    }

    public static string CreaLinkRoutes(System.Web.SessionState.HttpSessionState sessione = null, bool vuotasession = false, string Lingua = "I", string denominazione = "", string id = "", string codicetipologia = "", string codicecategoria = "", string codicecat2liv = "", string regione = "")
    {

        bool gen = false;
        bool.TryParse(ConfigManagement.ReadKey("generaUrlrewrited"), out gen);
        //bool upd = false;
        //bool.TryParse(ConfigManagement.ReadKey("updateTableurlrewriting"], out upd);
        string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, denominazione, id, codicetipologia, codicecategoria, codicecat2liv, regione, "", "", gen, WelcomeLibrary.STATIC.Global.UpdateUrl);
        return link;//.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
    }


    public static string CreaLinkRicerca(string id, string codicetipologia, string codicecategoria, string codicecat2liv, string regione, string annofiltro, string mesefiltro, string denominazione, string Lingua, System.Web.SessionState.HttpSessionState sessione = null, bool vuotasession = false)
    {
        //if (vuotasession && sessione != null)
        //{
        //    sessione.Clear();
        //}
        string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, denominazione, id, codicetipologia, codicecategoria, codicecat2liv, regione, annofiltro, mesefiltro, true, true);
        return link;
    }

    public static string CrealinkElencotipologia(string CodiceTipologia, string Lingua, System.Web.SessionState.HttpSessionState sessione = null, string cssclass = "", bool notag = false, string qstring = "")
    {
        string ret = "";
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
        {
            string linkgenerato = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, item.Descrizione, "", CodiceTipologia);
            if (!string.IsNullOrEmpty(qstring)) linkgenerato += "?" + qstring;
            if (!notag)
            {
                ret = "<a href=\"" + linkgenerato + "\" ";
                if (!string.IsNullOrEmpty(cssclass))
                    ret += "class=\"" + cssclass + "\"" + "  onclick=\"javascript:JsSvuotaSession(this)\"  ";
                ret += " >" + CleanInput(item.Descrizione) + "</a>";
            }
            else
            {
                ret = linkgenerato;
            }
        }

        return ret;
    }
    public static string CrealinkElencotipologiaCategoria(string CodiceTipologia, string codicecategoria, string Lingua, System.Web.SessionState.HttpSessionState sessione = null, string cssclass = "", bool notag = false)
    {
        string ret = "";
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == codicecategoria)); });

        if (item != null && catselected != null)
        {
            string linkgenerato = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, catselected.Descrizione, "", CodiceTipologia, codicecategoria);
            if (!notag)
            {
                ret = "<a href=\"" + linkgenerato + "\" ";
                if (!string.IsNullOrEmpty(cssclass))
                    ret += "class=\"" + cssclass + "\"" + "  onclick=\"javascript:JsSvuotaSession(this)\"  ";
                ret += " >" + CleanInput(item.Descrizione + " " + catselected.Descrizione) + "</a>";
            }
            else
            {
                ret = linkgenerato;
            }
        }
        return ret;
    }


    public static string ConteggioCaratteri(string testo, int caratteri = 600, bool nolink = false, string testoAggiunto = "")
    {
        string ritorno = testo;

        if (testo.Length > caratteri)
        {
            int invio = testo.IndexOf(" ", caratteri);

            if (nolink)
            {
                if (invio != -1)
                    ritorno = testo.Substring(0, invio) + " " + testoAggiunto;
                else
                    ritorno = testo.Substring(0, caratteri) + " " + testoAggiunto;
            }
            else
            {
                if (invio != -1)
                    ritorno = testo.Substring(0, invio) + " " + testoAggiunto;
                else
                    ritorno = testo.Substring(0, caratteri) + " " + testoAggiunto;
                // references.ResMan("Common",Lingua,"testoContinua").ToString()
            }
        }
        return ritorno;
    }


    public string CaricaValoreMaster(HttpRequest richiesta, System.Web.SessionState.HttpSessionState sessione, string chiave, bool vuotasession = false, string defvalue = "")// "", true,""
    {
        string ret = defvalue;
        if (vuotasession && sessione != null)
            sessione.Remove(chiave);//Tolgo da sessione il valore

        //Page.RouteData.Values["Id"].ToString();
        if (Page.RouteData != null && Page.RouteData.Values[chiave] != null)
        { ret = Page.RouteData.Values[chiave].ToString(); }
        else if (richiesta != null && richiesta.QueryString[chiave] != null && richiesta.QueryString[chiave].ToString() != "")
        { ret = richiesta.QueryString[chiave].ToString(); }
        else if (Context != null && Context.Items[chiave] != null && Context.Items[chiave].ToString() != "")
        { ret = Context.Items[chiave].ToString(); }
        else if (sessione != null && sessione[chiave] != null && sessione[chiave].ToString() != "")
        { ret = sessione[chiave].ToString(); }

        if (ret == "-") ret = "";
        if (sessione != null)
            sessione.Add(chiave, ret);//Metto in sessione il valore
        return ret;
    }

    public static String CleanInput(string strIn)
    {
        // Replace invalid characters with empty strings.
        //return Regex.Replace(strIn, @"[^\w\.@-]", "");
        // strIn = Regex.Replace(strIn, @"[\W]", "");
        //strIn = strIn.Replace(" ", "-");
        strIn = Regex.Replace(strIn, @"[^a-zA-Zа-яА-ЯЁё0-9@\$=!:.’#%_?^<>()òàùèì &°:;-]", "");

        return strIn;
    }
    public static String CleanUrl(string strIn)
    {
        strIn = strIn.Trim();
        // Replace invalid characters with empty strings.
        //return Regex.Replace(strIn, @"[^\w\.@-]", "");
        // strIn = Regex.Replace(strIn, @"[\W]", "");
        strIn = strIn.Replace(" ", "-");
        strIn = strIn.Replace("\r", "-");
        strIn = strIn.Replace("\n", "");
        strIn = strIn.Replace("à", "a");
        strIn = strIn.Replace("è", "e");
        strIn = strIn.Replace("ì", "i");
        strIn = strIn.Replace("ò", "o");
        strIn = strIn.Replace("ù", "u");
        // strIn = Regex.Replace(strIn, @"[^a-zA-Z0-9@\_]", "");
        strIn = Regex.Replace(strIn, @"[^a-zA-Zа-яА-ЯЁё0-9@\$=_()-]", "");
        return strIn.Trim('-');

    }

    //

    /// <summary>
    /// Rimpiazza link:(www.sitodavadere.it) con un link html
    /// oppure link:(www.sitodavadere.it|testo visualizzato del link)
    /// <param name="strIn"></param>
    /// <returns></returns>
    public static String ReplaceLinks(string strIn, bool nolink = false)
    {
        string target = "_blank";
        string urlcorretto = "";
        string ret = strIn;
        int a = strIn.ToLower().IndexOf("link:(");
        while (a != -1)
        {
            string origtext = "";
            int b = strIn.ToLower().IndexOf(")", a + 1);
            if (b != -1)
            {
                origtext = strIn.Substring(a, b - a + 1);

                string url = strIn.Substring(a + 6, b - (a + 6));
                string testourl = url;
                //Splitto supponendo di avere lo schema ulr|testourl
                string[] dati = url.Split('|');
                if (dati.Length == 2)
                {
                    url = (dati[0]);
                    testourl = dati[1];
                }
                urlcorretto = url;
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                {
                    target = "_self";
                    urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                }
                if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                {
                    target = "_self";
                    if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                        urlcorretto = "https://" + url;
                    else
                        urlcorretto = "http://" + url;
                }
                urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                if (!nolink && urlcorretto != "")
                {
                    strIn = strIn.Replace(origtext, "<a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-weight:bold;background-color:#e0e0e0\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a>");
                }
                else
                    strIn = strIn.Replace(origtext, testourl);
                target = "_blank";
            }
            else
            {
                strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il link:( sennò si looppa
            }
            a = strIn.ToLower().IndexOf("link:(");
        }
        ret = strIn;

        a = strIn.ToLower().IndexOf("quot:(");
        while (a != -1)
        {
            string origtext = "";
            int b = strIn.ToLower().IndexOf(")", a + 1);
            if (b != -1)
            {
                origtext = strIn.Substring(a, b - a + 1);

                string url = strIn.Substring(a + 6, b - (a + 6));
                string testourl = url;
                //Splitto supponendo di avere lo schema ulr|testourl
                string[] dati = url.Split('|');
                if (dati.Length == 2)
                {
                    url = (dati[0]);
                    testourl = dati[1];
                }
                urlcorretto = url;
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                {
                    target = "_self";
                    urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                }
                if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                {
                    target = "_self";
                    if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                        urlcorretto = "https://" + url;
                    else
                        urlcorretto = "http://" + url;
                }
                urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                if (!nolink)
                {

                    if (string.IsNullOrWhiteSpace(url))
                        strIn = strIn.Replace(origtext, "<div style=\"font-size:100%;padding:10px;margin:5px;background-color:#e0e0e0\">" + testourl + "</div>");
                    else
                        strIn = strIn.Replace(origtext, "<div  style=\"font-size:100%;padding:10px;margin:5px;background-color:#e0e0e0\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></div>");

                }
                else
                    strIn = strIn.Replace(origtext, testourl);
                target = "_blank";
            }
            else
            {
                strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
            }
            a = strIn.ToLower().IndexOf("quot:(");
        }
        ret = strIn;


        a = strIn.ToLower().IndexOf("bold:(");
        while (a != -1)
        {
            string origtext = "";
            int b = strIn.ToLower().IndexOf(")", a + 1);
            if (b != -1)
            {
                origtext = strIn.Substring(a, b - a + 1);

                string url = strIn.Substring(a + 6, b - (a + 6));
                string testourl = url;
                //Splitto supponendo di avere lo schema ulr|testourl
                string[] dati = url.Split('|');
                if (dati.Length == 2)
                {
                    url = (dati[0]);
                    testourl = dati[1];
                }
                urlcorretto = url;
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                {
                    target = "_self";
                    urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                }
                if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                {
                    target = "_self";
                    if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                        urlcorretto = "https://" + url;
                    else
                        urlcorretto = "http://" + url;
                }
                urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                if (!nolink)
                {
                    if (string.IsNullOrWhiteSpace(url))
                        strIn = strIn.Replace(origtext, "<strong>" + testourl + "</strong>");
                    else
                        strIn = strIn.Replace(origtext, "<strong><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></strong>");



                }
                else
                    strIn = strIn.Replace(origtext, testourl);
                target = "_blank";
            }
            else
            {
                strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
            }
            a = strIn.ToLower().IndexOf("bold:(");
        }
        ret = strIn;


        a = strIn.ToLower().IndexOf("iden:(");
        while (a != -1)
        {
            string origtext = "";
            int b = strIn.ToLower().IndexOf(")", a + 1);
            if (b != -1)
            {
                origtext = strIn.Substring(a, b - a + 1);

                string url = strIn.Substring(a + 6, b - (a + 6));
                string testourl = url;
                //Splitto supponendo di avere lo schema ulr|testourl
                string[] dati = url.Split('|');
                if (dati.Length == 2)
                {
                    url = (dati[0]);
                    testourl = dati[1];
                }
                urlcorretto = url;
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                {
                    target = "_self";
                    urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                }
                if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                {
                    target = "_self";
                    if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                        urlcorretto = "https://" + url;
                    else
                        urlcorretto = "http://" + url;
                }
                urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                if (!nolink)
                {
                    //if (string.IsNullOrWhiteSpace(url))
                    //    strIn = strIn.Replace(origtext, "</p><span class=\"lateralbar\">" + testourl + "</span><p>");
                    //else
                    //    strIn = strIn.Replace(origtext, "</p><span class=\"lateralbar\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></span><p>");
                    if (string.IsNullOrWhiteSpace(url))
                        strIn = strIn.Replace(origtext, "<div class=\"lateralbar\">" + testourl + "</div>");
                    else
                        strIn = strIn.Replace(origtext, "<div class=\"lateralbar\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></div>");
                }
                else
                    strIn = strIn.Replace(origtext, testourl);
                target = "_blank";
            }
            else
            {
                strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
            }
            a = strIn.ToLower().IndexOf("iden:(");
        }
        ret = strIn;

        a = strIn.ToLower().IndexOf("butt:(");
        while (a != -1)
        {
            string origtext = "";
            int b = strIn.ToLower().IndexOf(")", a + 1);
            if (b != -1)
            {
                origtext = strIn.Substring(a, b - a + 1);

                string url = strIn.Substring(a + 6, b - (a + 6));
                string testourl = url;
                //Splitto supponendo di avere lo schema ulr|testourl
                string[] dati = url.Split('|');
                if (dati.Length == 2)
                {
                    url = (dati[0]);
                    testourl = dati[1];
                }
                urlcorretto = url;
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                {
                    target = "_self";
                    urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                }
                if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                {
                    target = "_self";
                    if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                        urlcorretto = "https://" + url;
                    else
                        urlcorretto = "http://" + url;
                }

                urlcorretto = ReplaceAbsoluteLinks(urlcorretto);


                if (!nolink)
                {
                    if (string.IsNullOrWhiteSpace(url))
                        strIn = strIn.Replace(origtext, "<span style=\"line-height:normal;display:inline\" class=\"divbuttonstyle\">" + testourl + "</span>");
                    else
                        strIn = strIn.Replace(origtext, "<span style=\"line-height:normal;display:inline\" class=\"divbuttonstyle\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"line-height:normal;display:inline\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></span>");



                }
                else
                    strIn = strIn.Replace(origtext, testourl);
                target = "_blank";
            }
            else
            {
                strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
            }
            a = strIn.ToLower().IndexOf("butt:(");



        }
        ret = strIn;


        a = strIn.ToLower().IndexOf("buto:(");
        while (a != -1)
        {
            string origtext = "";
            int b = strIn.ToLower().IndexOf(")", a + 1);
            if (b != -1)
            {
                origtext = strIn.Substring(a, b - a + 1);

                string url = strIn.Substring(a + 6, b - (a + 6));
                string testourl = url;
                //Splitto supponendo di avere lo schema ulr|testourl
                string[] dati = url.Split('|');
                if (dati.Length == 2)
                {
                    url = (dati[0]);
                    testourl = dati[1];
                }
                urlcorretto = url;
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                {
                    target = "_self";
                    urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                }
                if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                {
                    target = "_self";
                    if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                        urlcorretto = "https://" + url;
                    else
                        urlcorretto = "http://" + url;
                }
                urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                if (!nolink)
                {
                    if (string.IsNullOrWhiteSpace(url))
                        strIn = strIn.Replace(origtext, "<span class=\"divbuttonstyleorange\">" + testourl + "</span>");
                    else
                        strIn = strIn.Replace(origtext, "<span class=\"divbuttonstyleorange\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></span>");
                }
                else
                    strIn = strIn.Replace(origtext, testourl);
                target = "_blank";
            }
            else
            {
                strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
            }
            a = strIn.ToLower().IndexOf("buto:(");
        }
        ret = strIn;




        a = strIn.ToLower().IndexOf("imag:(");
        while (a != -1)
        {
            string origtext = "";
            int b = strIn.ToLower().IndexOf(")", a + 1);
            if (b != -1)
            {
                origtext = strIn.Substring(a, b - a + 1);

                string url = strIn.Substring(a + 6, b - (a + 6));
                string testourl = url;
                string[] dati = url.Split('|');
                if (dati.Length == 2)
                {
                    url = (dati[0]);
                    testourl = dati[1];
                }
                urlcorretto = url;
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                {
                    target = "_self";
                    urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                }
                if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                {
                    target = "_self";
                    if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                        urlcorretto = "https://" + url;
                    else
                        urlcorretto = "http://" + url;
                }
                urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                if (!nolink)
                {


                    if (!string.IsNullOrWhiteSpace(url))
                        strIn = strIn.Replace(origtext, "<img class=\"\"  style=\"max-width:100%\"  src=\"" + urlcorretto + "\" alt=\"" + testourl + "\"  />");
                    else if (!url.ToLower().StartsWith("http"))
                        strIn = strIn.Replace(origtext, "<img class=\"\"  style=\"max-width:100%\"  src=\"" + urlcorretto + "\" alt=\"" + testourl + "\"  />");

                }
                else
                    strIn = strIn.Replace(origtext, testourl);

            }
            else
            {
                strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
            }
            a = strIn.ToLower().IndexOf("imag:(");



        }
        ret = strIn;


        a = strIn.ToLower().IndexOf("titl:(");
        while (a != -1)
        {
            string origtext = "";
            int b = strIn.ToLower().IndexOf(")", a + 1);
            if (b != -1)
            {
                origtext = strIn.Substring(a, b - a + 1);

                string url = strIn.Substring(a + 6, b - (a + 6));
                string testourl = url;
                string[] dati = url.Split('|');
                if (dati.Length == 2)
                {
                    url = (dati[0]);
                    testourl = dati[1];
                }
                urlcorretto = url;
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                {
                    target = "_self";
                    urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                }
                if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                {
                    target = "_self";
                    if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                        urlcorretto = "https://" + url;
                    else
                        urlcorretto = "http://" + url;
                }
                urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                if (!nolink)
                {
                    if (string.IsNullOrWhiteSpace(url))
                        strIn = strIn.Replace(origtext, "<span style=\"font-weight:800;font-size:1.4em\" >" + testourl + "</span>");
                    else
                        strIn = strIn.Replace(origtext, "<strong><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-weight:800;font-size:1.4em\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></strong>");
                }
                else
                    strIn = strIn.Replace(origtext, testourl);
                target = "_blank";

            }
            else
            {
                strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
            }
            a = strIn.ToLower().IndexOf("titl:(");
        }
        ret = strIn;



        return ret;

    }

    public static string ControlloVuotoPosizione(string comune, string codiceprovincia, string codicetipologia, string Lingua)
    {
        string ret = "";

        //if (!string.IsNullOrWhiteSpace(codiceprovincia))
        //    ret += NomeRegione(codiceprovincia, Lingua).ToLower() + " ";
        if (!string.IsNullOrWhiteSpace(comune))
            ret += comune.ToLower();
        if (!string.IsNullOrWhiteSpace(codiceprovincia))
            ret += " (" + NomeProvincia(codiceprovincia, Lingua).ToLower() + ") ";

        return ret;
    }
    public static string ControlloVuotoPosizione(string comune, string codiceprovincia, string codiceregione, string codicetipologia, string Lingua)
    {
        string ret = "";

        //if (!string.IsNullOrWhiteSpace(codiceprovincia))
        //    ret += NomeRegione(codiceprovincia, Lingua).ToLower() + " ";
        if (!string.IsNullOrWhiteSpace(comune))
            ret += comune.ToLower();
        if (!string.IsNullOrWhiteSpace(codiceprovincia))
            ret += " " + NomeProvincia(codiceprovincia, Lingua).ToLower() + " ";
        if (!string.IsNullOrWhiteSpace(codiceregione))
            ret += " " + NomeRegione(codiceprovincia, Lingua).ToLower() + " ";
        return ret;
    }

    #region Wrapper Per Metodi REFERENCES 

    public static string TestoTipologia(string codicetipologia, string Lingua)
    {
        return references.TestoTipologia(codicetipologia, Lingua);
    }
    public static string TestoCategoria(string codicetipologia, string codicecategoria, string Lingua)
    {
        return references.TestoCategoria(codicetipologia, codicecategoria, Lingua);
    }
    public static string TestoCategoria2liv(string codicetipologia, string codicecategoria, string codicecategoria2liv, string Lingua)
    {
        return references.TestoCategoria2liv(codicetipologia, codicecategoria, codicecategoria2liv, Lingua);
    }
    public static string TestoCaratteristica(int progressivocaratteristica, string codice, string Lingua)
    {
        return references.TestoCaratteristica(progressivocaratteristica, codice, Lingua);
    }
    public static string NomeRegione(string codiceprovincia, string Lingua)
    {
        return references.NomeRegione(codiceprovincia, Lingua);
    }
    public static string NomeProvincia(string codiceprovincia, string Lingua)
    {
        return references.NomeProvincia(codiceprovincia, Lingua);
    }
    public static string TrovaCodiceRegione(string nomeregione, string Lingua)
    {
        return references.TrovaCodiceRegione(nomeregione, Lingua);
    }
    public static string TrovaCodiceProvincia(string nomeprovincia, string Lingua)
    {
        return references.TrovaCodiceProvincia(nomeprovincia, Lingua);

    }
    public static string TrovaCodiceNazione(string nomenazione, string Lingua)
    {
        return references.TrovaCodiceNazione(nomenazione, Lingua);
    }

    #endregion

    public CommonPage()
    {
        //All'istanza imposto il tempo di sleep per la modalità trial
        if (WelcomeLibrary.STATIC.Global.Trial && (DateTime.Now > WelcomeLibrary.STATIC.Global.Datastartrial))
            if (WelcomeLibrary.STATIC.Global.Millisecondsleeptimefortrial > 0)
                System.Threading.Thread.Sleep(WelcomeLibrary.STATIC.Global.Millisecondsleeptimefortrial);
    }


    /// <summary>
    /// Funzione per override per impostare la cultura in base alla lingua richiesta
    /// e quindi impostare la Globalizzazione/Localizzazione (Mettenola in questa classe
    /// comune permette di usare la glob/loc anche nelle masterpages)
    /// </summary>
    protected override void InitializeCulture()
    {
        string Lingua = deflanguage;
        Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);

        System.Globalization.CultureInfo c;

        switch (Lingua)
        {
            case "I":
                c = System.Globalization.CultureInfo.CreateSpecificCulture("it");
                System.Threading.Thread.CurrentThread.CurrentCulture = c;
                System.Threading.Thread.CurrentThread.CurrentUICulture = c;
                break;
            case "GB":
                c = System.Globalization.CultureInfo.CreateSpecificCulture("en");
                System.Threading.Thread.CurrentThread.CurrentCulture = c;
                System.Threading.Thread.CurrentThread.CurrentUICulture = c;
                break;
            case "RU":
                c = System.Globalization.CultureInfo.CreateSpecificCulture("ru");
                System.Threading.Thread.CurrentThread.CurrentCulture = c;
                System.Threading.Thread.CurrentThread.CurrentUICulture = c;
                break;
            default:
                c = System.Globalization.CultureInfo.CreateSpecificCulture("it");
                System.Threading.Thread.CurrentThread.CurrentCulture = c;
                System.Threading.Thread.CurrentThread.CurrentUICulture = c;
                break;
        }

        base.InitializeCulture();
    }
    #region FUNZIONI GESTIONE CARRELLO ECOMMERCE

    public string PulisciRegistrazionitemporanee()
    {
        string message = "";
        List<OleDbParameter> parColl = new List<OleDbParameter>();
        string tipologia = "rif000004";
        if (tipologia != "" && tipologia != "-")
        {
            OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", tipologia);
            parColl.Add(p3);
        }
        OleDbParameter psrch = new OleDbParameter("@Archiviato", true);
        parColl.Add(psrch);
        OleDbParameter pdini = new OleDbParameter("@Data_inizio", "01/01/1900");
        OleDbParameter pdfin = new OleDbParameter("@Data_fine", System.DateTime.Now.AddDays(-1).ToString());
        parColl.Add(pdini);
        parColl.Add(pdfin);
        List<Offerte> offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "1000", "I", false, "", true);
        if (offerte != null)
            foreach (Offerte o in offerte)
            {

                //scorro e cancello le foto presenti
                string pathDestinazione = Server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + tipologia + "/" + o.Id);
                foreach (Allegato foto in o.FotoCollection_M)
                {
                    try
                    {
                        bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o.Id, foto.NomeFile, "", pathDestinazione);
                    }
                    catch (Exception errodel)
                    {
                        message = errodel.Message;
                    }
                }

                //Cancello il record
                offDM.DeleteOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o);
                WelcomeLibrary.UF.SitemapManager.EliminaUrlrewritebyIdOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o.Id.ToString());
            }

        return message;
    }

    protected static string getidsocio(string utente)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
            UserLastActivityDate = _user.LastActivityDate;

        ProfileBase profile = ProfileBase.Create(utente);
        string idsocio = (string)profile["IdSocio"];

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }
        return idsocio;

    }

    public static string getFirstName(string utente)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
            UserLastActivityDate = _user.LastActivityDate;

        ProfileBase profile = ProfileBase.Create(utente);
        string nome = (string)profile["FirstName"];

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }
        return nome;

    }
    /// <summary>
    /// torna l'dicliente associato all'utente nel profilo
    /// </summary>
    /// <param name="utente"></param>
    /// <returns></returns>
    public static string getidcliente(string utente)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
        {
            UserLastActivityDate = _user.LastActivityDate;
        }

        //ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
        ProfileBase profile = ProfileBase.Create(utente);
        string idCliente = (string)profile["IdCliente"];

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }
        return idCliente;
    }
    public static string getmailuser(string utente)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
        {
            UserLastActivityDate = _user.LastActivityDate;
        }

        //ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
        ProfileBase profile = ProfileBase.Create(utente);
        string emailuser = (string)profile["EMail"];

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }
        return emailuser;
    }
    public static void CaricaRiferimentiCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, ref string sessionid, ref string trueIP)
    {
        if (Session != null)
            sessionid = Session.SessionID;
        //////////////////////////////////////////////////////////////////////////////
        //Prendiamo l'ip del client
        /////////////////////////////////////////////////////////////////////////////
        if (Request != null)
        {
            trueIP = "";
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ip))
            {
                string[] ipRange = ip.Split(',');
                trueIP = ipRange[0].Trim();
            }
            else
            {
                trueIP = Request.ServerVariables["REMOTE_ADDR"].Trim();
            }
        }


    }
    public static string VisualizzaCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, string codiceordine, bool nofoto = false, string Lingua = "I")

    {

        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);

        StringBuilder sb = new StringBuilder();
        //sb.Append(codiceordine);
        eCommerceDM ecmDM = new eCommerceDM();

        CarrelloCollection carrello = new CarrelloCollection();
        if (codiceordine != "")
        {
            carrello = ecmDM.CaricaCarrelloPerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);
        }
        else
        {
            carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP);
        }
        foreach (Carrello c in carrello)
        {

            //Creiamo la visualizzione degli articoli in carrello
            // da fare <li>  contenuto da prendere sotto  </li>
            sb.Append("<li style=\"padding-top:2px;padding-right:5px;margin-top:5px\">");

            string linkofferta = "";
            string testoofferta = "";
            string imgofferta = "";
            string titoloofferta = "";
            if (c.Offerta != null)
            {
                try
                {
                    if (c.Offerta.DenominazioneI != null)
                    {
                        linkofferta = CommonPage.ReplaceAbsoluteLinks(CommonPage.CreaLinkRoutes(null, false, Lingua, CommonPage.CleanUrl(c.Offerta.DenominazioneI), c.Offerta.Id.ToString(), c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, ""));
                        testoofferta = CommonPage.CleanInput(CommonPage.ConteggioCaratteri(c.Offerta.DenominazioneI, 300, true));
                        imgofferta = CommonPage.ReplaceAbsoluteLinks(CommonPage.ComponiUrlAnteprima(c.Offerta.FotoCollection_M.FotoAnteprima, c.Offerta.CodiceTipologia, c.Offerta.Id.ToString()));
                        titoloofferta = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(c.Offerta.DenominazioneI);
                    }
                }
                catch { }
            }

            if (!nofoto)
            {

                sb.Append("<a target=\"_blank\"   href=\"" +
                    linkofferta
                       + "\"  class=\"product-thumb pull-left\"  >");
                sb.Append("<img alt=\""
                    +
                  testoofferta
                    + "\" Style=\"width: auto; height: auto; max-width: 60px; max-height: 60px;\" ");
                sb.Append(" src=\"");
                sb.Append(imgofferta + "\" ");
                sb.Append("\" />");
                sb.Append(" </a>");
            }

            sb.Append(" <div class=\"product-details\">");
            sb.Append(" <h3 class=\"product-name\">");
            sb.Append(titoloofferta);
            sb.Append(" </h3>");


            //sb.Append(" <div class=\"product-categories muted\">");
            //sb.Append(CommonPage.TestoCategoria(c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, Lingua));
            //sb.Append(" </div>");
            //sb.Append(" <div class=\"product-categories muted\">");
            //sb.Append(CommonPage.TestoCaratteristica(0, c.Offerta.Caratteristica1.ToString(), Lingua));
            //sb.Append(" </div>");
            sb.Append(" <div class=\"product-categories muted\">");
            if (c.Offerta.Caratteristica6 != 0)
                sb.Append(CommonPage.TestoCaratteristica(5, c.Offerta.Caratteristica6.ToString(), Lingua));
            sb.Append(" </div>");

            //sb.Append(" <div class=\"product-categories muted\">");
            //sb.Append(TestoSezione(c.Offerta.CodiceTipologia));
            //sb.Append(" </div>");
            sb.Append(" <p class=\"product-calc muted\">");

            sb.Append(c.Numero + "&times;" + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", c.Prezzo) + " €");
            sb.Append(" </p>");

            sb.Append(" </div>");
            sb.Append(" </li>");

        }
        return sb.ToString();
    }
    public static bool AggiornaProdottoCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, int idprodotto, int quantita, string username, int idcliente = 0)
    {
        bool superamentodisponibilita = false;
        bool ret = false;
        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);

        //Carico l'elemento del carrello e lo aggiorno nel database con le modifiche di numero
        Carrello Item = null;
        CarrelloCollection ColItem = new CarrelloCollection();
        eCommerceDM ecom = new eCommerceDM();


        if (idprodotto != 0)
        {
            Item = new Carrello();
            offerteDM offDM = new offerteDM();
            Offerte off = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idprodotto.ToString());
            //Carico la riga prodotto dal carrello
            ColItem = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP, idprodotto);
            if (ColItem != null && ColItem.Count > 0)
                Item = ColItem[0];
            bool prodottoeliminato = false;
            if (off == null || off.Id == 0)
            {
                if (Item != null && Item.id_prodotto != 0)
                    ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item.ID);
                prodottoeliminato = true;
                quantita = 0;
                return ret;
            }
            //controlli sulla disponibilità articolo
            if (off.Qta_vendita != null)
            {
                if (quantita > off.Qta_vendita)
                {
                    superamentodisponibilita = true; //da usare per visualizzazione alerts
                    Session.Add("superamentoquantita", (long)off.Qta_vendita);
                    quantita = (int)off.Qta_vendita;
                }
                if (off.Qta_vendita == 0) // se il prodotto non è più disponibile lo elimino dal carrello
                {
                    if (Item != null && Item.id_prodotto != 0)
                        ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item.ID);
                    prodottoeliminato = true;
                    quantita = 0;
                    return ret;
                }
            }

            if (Item == null || Item.id_prodotto == 0) //Nuovo elemento da mettere nel carrello
            {
                Item.Data = System.DateTime.Now;
                Item.Prezzo = off.Prezzo;
                //prodotto.Iva = 0;
                //if (quantita == 0) quantita = 1;

                Item.CodiceProdotto = off.CodiceProdotto;
                Item.id_prodotto = off.Id;
                Item.Validita = 1;
                Item.SessionId = sessionid;
                Item.IpClient = trueIP;
                Item.Numero = quantita;

            }
            else
            {
                Item.Prezzo = off.Prezzo;
                Item.CodiceProdotto = off.CodiceProdotto;
                Item.id_prodotto = off.Id;
                Item.Data = DateTime.Now;
                Item.Numero = quantita;
            }

            //Aggiungo l'id anagrafica del cliente ( configurato nel profilo utente ) all'articolo nel carrello
            //In modo da avre l'associazione degli ordini con i clienti
            if (idcliente != 0) //SE passato un idcliente lo memorizzo nel rigo di carrello prodotti
            {
                Item.ID_cliente = idcliente;
            }
            if (!string.IsNullOrEmpty(username))
            {
                int i = 0;
                int.TryParse(getidcliente(username), out i);
                Item.ID_cliente = i;
            }
            if (quantita >= 1)
                ecom.InsertUpdateCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item, false);
            else
                ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item.ID);

            ret = true;
        }
        return ret;
    }
    public static bool SvuotaCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session)
    {
        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);
        eCommerceDM ecom = new eCommerceDM();
        CarrelloCollection ColItem = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP);
        List<int> idtodelete = new List<int>();
        foreach (Carrello c in ColItem)
        {
            ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c.ID);
        }
        return true;
    }
    /// <summary>
    /// Calcola i totali del carrello con vari costi accessori per la procedura di ordine
    /// </summary>
    /// <param name="Request"></param>
    /// <param name="Session"></param>
    /// <param name="codicenazione"></param>
    /// <param name="codiceprovincia"></param>
    /// <param name="supplementospedizione"></param>
    /// <returns></returns>
    public static TotaliCarrello CalcolaTotaliCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, string codicenazione, string codiceprovincia, bool supplementospedizione = false, bool supplementocontanti = false)
    {
        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);
        eCommerceDM ecom = new eCommerceDM();
        CarrelloCollection ColItem = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP);

        string idlist = "";

        TotaliCarrello totali = new TotaliCarrello();
        totali.Supplementospedizione = supplementospedizione;

        List<int> idtodelete = new List<int>();
        long idclienteincarrello = 0;
        foreach (Carrello c in ColItem)
        {
            //controlli sulla disponibilità articolo///////////////////
            if (c.Offerta.Qta_vendita != null)
            {
                if (c.Offerta.Qta_vendita == 0) // se il prodotto non è più disponibile lo elimino dal carrello
                {
                    if (c != null && c.id_prodotto != 0)
                    {
                        idtodelete.Add(c.ID);
                        continue; //Salto la sommatoria
                    }
                }
            }
            //////////////////////////////////////////////////////////
            totali.TotaleOrdine += c.Numero * (c.Prezzo + c.Iva);
            idlist += c.ID.ToString() + ",";
            idclienteincarrello = c.ID_cliente;//Ogni articolo nel carrello ha lo stesso codice id cliente
        }
        if (idlist.Length > 1)
        {
            idlist = idlist.Substring(0, idlist.Length - 1);
            idlist = idlist.Insert(0, "( ");
            idlist += " )";
        }
        foreach (int l in idtodelete) //Elimino dal carrello gli elementi non più disponibili
        {
            ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, l);
        }

        //  totali.TotaleSmaltimento = CalcolaTotaliSmaltimento(ColItem);
        totali.TotaleSconto = CalcolaSconto(Session, ColItem, totali.TotaleOrdine);
        totali.TotaleSpedizione = CalcolaSpeseSpedizione(ColItem, codicenazione, codiceprovincia, supplementospedizione, totali.TotaleOrdine, totali.TotaleSconto, supplementocontanti);

        totali.Id_cliente = (int)idclienteincarrello;
        //Aggiono i codice della nazione di spedizione nel carrello
        if (!string.IsNullOrWhiteSpace(idlist) && !string.IsNullOrWhiteSpace(codicenazione))
            ecom.UpdateCarrelloPerListaID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicenazione, idlist);
        return totali;
    }
    private static double CalcolaTotaliSmaltimento(CarrelloCollection ColItem)
    {
        double totalesmaltimento = 0;
        double _tmpauto = 0;
        double _tmpmoto = 0;

        double.TryParse(ConfigManagement.ReadKey("PFUauto"), out _tmpauto);
        double.TryParse(ConfigManagement.ReadKey("PFUmoto"), out _tmpmoto);

        foreach (Carrello c in ColItem)
        {

            if (TestoCaratteristica(5, c.Offerta.Caratteristica6.ToString(), "I").ToLower() == "auto")
            { //auto
                totalesmaltimento += c.Numero * _tmpauto;
            }

            if (TestoCaratteristica(5, c.Offerta.Caratteristica6.ToString(), "I").ToLower() == "moto")
                //Moto
                totalesmaltimento += c.Numero * _tmpmoto;
        }

        return totalesmaltimento;
    }
    public static double CalcolaTotaleCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, CarrelloCollection carrello)
    {
        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);


        eCommerceDM ecom = new eCommerceDM();

        if (carrello == null || carrello.Count == 0)
            carrello = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP);

        double totale = 0;
        foreach (Carrello c in carrello)
        {
            totale += c.Numero * (c.Prezzo + c.Iva);
        }

        return totale;
    }
    private static double CalcolaSconto(System.Web.SessionState.HttpSessionState Session, CarrelloCollection ColItem, double totalecarrello)
    {
        double valoresconto = 0;
        string percentualesconto = ConfigManagement.ReadKey("percentualesconto");
        if (Session["codicesconto"] != null && Session["codicesconto"].ToString() == ConfigManagement.ReadKey("codicesconto"))
        {
            double tmp = 0;
            double.TryParse(percentualesconto, out tmp);

            valoresconto = Math.Floor((double)totalecarrello * tmp / 100);
        }
        return valoresconto;
    }
    private static double CalcolaSpeseSpedizione(CarrelloCollection ColItem, string codicenazione, string codiceprovincia, bool supplementospedizione, double totaleordine, double totalesconto, bool contrassegno = false)
    {
        double spesespedizione = 0;
        //Da calcolare in base ai parametri passati
        //long totalearticoli = 0;
        //foreach (Carrello c in ColItem)
        //{
        //    totalearticoli += c.Numero;
        //}
        if (supplementospedizione) //Supplemento isole supplementoSpedizioni
            spesespedizione = Convert.ToDouble(ConfigManagement.ReadKey("supplementoSpedizioni"));
        if (contrassegno) //Supplemento isole supplementoSpedizioni
            spesespedizione = Convert.ToDouble(ConfigManagement.ReadKey("supplementoContrassegno"));

        //supplementoContrassegno

        switch (codicenazione)
        {
            //case "GB":
            //    spesespedizione = 20;
            //    break;
            default:
                //if (totalearticoli <= 3)
                if (totaleordine - totalesconto <= Convert.ToDouble(ConfigManagement.ReadKey("sogliaSpedizioni")))
                {
                    spesespedizione += Convert.ToDouble(ConfigManagement.ReadKey("costobaseSpedizioni")); 
                }
                break;
        }
        return spesespedizione;
    }
    public static string CaricaQuantitaNelCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, string idprodotto)
    {
        string ret = "0";
        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);

        //Carico l'elemento del carrello e lo aggiorno nel database con le modifiche di numero
        Carrello Item = null;
        CarrelloCollection ColItem = new CarrelloCollection();
        eCommerceDM ecom = new eCommerceDM();


        if (!string.IsNullOrWhiteSpace(idprodotto) && idprodotto.ToString() != "0")
        {

            int id_prodotto = 0;
            int.TryParse(idprodotto.ToString(), out id_prodotto);
            ColItem = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP, id_prodotto);
            if (ColItem != null && ColItem.Count > 0)
                Item = ColItem[0];
        }

        if (Item != null && Item.id_prodotto != 0)
            ret = Item.Numero.ToString();
        return ret;
    }


    #endregion

    #region MODIX automotive import

    public static void ParseXmlFile(HttpServerUtility server, string sourcebaseaddress, string codicedestinazione, string nrecord = "0")
    {
        int recordlimit = 0;
        int.TryParse(nrecord, out recordlimit);
        System.IO.StreamReader sr = new System.IO.StreamReader(server.MapPath(sourcebaseaddress));
        System.Xml.XmlTextReader scriptXmlReader = new System.Xml.XmlTextReader(sr);
        Dictionary<string, string> maindict = new Dictionary<string, string>();
        scriptXmlReader.WhitespaceHandling = WhitespaceHandling.None;
        string xmlNodeText = "";
        string nodeName = "";
        bool stopreading = false;
        bool skipreading = false; // da usare per saltare la lettura dei veicoli per lingue non volute
        using (scriptXmlReader)
        {

            while (!stopreading && scriptXmlReader.Read()) //Leggo una riga alla volta
            {
                switch (scriptXmlReader.NodeType)
                {
                    case XmlNodeType.Element:

                        nodeName = scriptXmlReader.Name;
                        if (nodeName == "vehiclePool")
                        {
                            string LANG = scriptXmlReader.GetAttribute("lang");
                            if (LANG != "it-IT")
                                skipreading = true;
                            else
                                skipreading = false;
                        }

                        while (scriptXmlReader.Name == "vehicle" && scriptXmlReader.NodeType == XmlNodeType.Element)
                        {
                            string ciphervalue = scriptXmlReader.GetAttribute("cipher"); //Codice veicolo da modix
                            string internalDescription = scriptXmlReader.GetAttribute("internalDescription"); //Codice veicolo interno

                            string parentXml = "";
                            if (!scriptXmlReader.IsEmptyElement)
                                parentXml = scriptXmlReader.ReadOuterXml();
                            else//Leggo senza memorizzare per avanzare la lettura del file!
                                scriptXmlReader.ReadOuterXml();

                            if (!skipreading && ciphervalue != null && !maindict.ContainsKey(ciphervalue.ToString()) && ciphervalue != string.Empty && parentXml != string.Empty)
                                maindict.Add(ciphervalue.ToString(), parentXml);
                        }

                        break;
                    case XmlNodeType.Text:
                        xmlNodeText = scriptXmlReader.Value;
                        //if (dict.ContainsKey(idnew.ToString()) && dict[idnew.ToString()].ContainsKey(nodeName))
                        //    dict[idnew.ToString()][nodeName] = xmlNodeText;
                        break;
                    case XmlNodeType.CDATA:
                        xmlNodeText = scriptXmlReader.Value;
                        //if (dict.ContainsKey(idnew.ToString()) && dict[idnew.ToString()].ContainsKey(nodeName))
                        //    dict[idnew.ToString()][nodeName] = xmlNodeText;
                        break;
                    case XmlNodeType.EndElement:
                        xmlNodeText = string.Empty;
                        if (scriptXmlReader.Name == "automotive" && scriptXmlReader.Depth == 0)
                            stopreading = true; //Fermo la lettura del file al termine per evitare una read fasulla
                        break;
                    default:
                        break;
                }
            }

        }

        /////////////////////////////////////////////////////////////
        //PARSERIZZIAMO I SINGOLI VEICOLI E INSERIAMOLI NEL DB
        /////////////////////////////////////////////////////////////
        offerteDM offDM = new offerteDM();
        Dictionary<string, Dictionary<string, List<Tabrif>>> dict = new Dictionary<string, Dictionary<string, List<Tabrif>>>();
        nodeName = "";
        OfferteCollection listacompletapercodice = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicedestinazione);

        //Scorriamo gli elementi trovati per momorizzare nel db i valori
        foreach (KeyValuePair<string, string> kv in maindict)
        {
            //Prima elimino da db eventuali record coincidenti
            Offerte o = offDM.CaricaOffertaPerCodiceProdotto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, kv.Key);
            //Se presente elimino il record e reinserisco ex novo. ( per ora )
            if (o != null && o.Id != 0 && o.CodiceTipologia == codicedestinazione)
            {
                // offDM.DeleteOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o);
                listacompletapercodice.RemoveAll(t => t.Id == o.Id);
            }
            ///////////Procedo con la creazione dell'elemento
            if (o == null) o = new Offerte();
            o.CodiceTipologia = codicedestinazione;
            o.DataInserimento = System.DateTime.Now;
            o.CodiceProdotto = kv.Key;
            o.Xmlvalue = kv.Value;
            o.Abilitacontatto = true;

            //if (!dict.ContainsKey(kv.Key)) //Chiave per il veicolo!
            //    dict.Add(kv.Key, new Dictionary<string, List<Tabrif>>());

            //ESTRAIAMO I DATI DALL'XML
            //------------

            //pRIMA LE CARATTERISTICHE PRIMARIE DI RICERCA
            Tabrif item = new Tabrif();
            int _i = 0;
            string _text = "";
            DateTime _Data = DateTime.MinValue;
            bool _tmpbool = false;
            double _tmpdbl = 0;

            item = ReadXmlSinglevalue(kv.Value, "mainData", "manufacturer"); //manufacturers -> Caratteristica1
            if (int.TryParse(item.Campo2, out _i))
                o.Caratteristica1 = _i; // viene dall'id
            _text = item.Campo1.Trim(); ;
            item = ReadXmlSinglevalue(kv.Value, "mainData", "model"); //models -> Caratteristica2
            if (int.TryParse(item.Campo2, out _i))
                o.Caratteristica2 = _i; // viene dall'id
            _text += " " + item.Campo1.Trim(); ;
            item = ReadXmlSinglevalue(kv.Value, "mainData", "usageCategory"); //usage_category ( usato,aziendale,km0 ) -> Caratteristica3
            if (int.TryParse(item.Campo2, out _i))
                o.Caratteristica3 = _i; // viene dall'id
            item = ReadXmlSinglevalue(kv.Value, "mainData", "fuel"); //fueltypes -> Caratteristica4
            if (int.TryParse(item.Campo2, out _i))
                o.Caratteristica4 = _i; // viene dall'id
            item = ReadXmlSinglevalue(kv.Value, "mainData", "submodel"); //modello testuale
            _text += "\r\n" + item.Campo1.Trim(); ;
            o.DenominazioneI = _text.Trim(); ;
            item = ReadXmlSinglevalue(kv.Value, "description", "");//Descrizione del veicolo
            o.DescrizioneI = item.Campo1; //viene dal valore

            item = ReadXmlSinglevalue(kv.Value, "dates", "date", "2");//Prendo solo la data di tipo 2 ( prima immatricolazione )
            if (DateTime.TryParse(item.Campo1, out _Data))
                o.Data1 = _Data;//   CAMPO DATA SPECIFICO PER LA PRIMA IMMATRICOLAZIONE nella stuttura offerte, serve a filtrare!!!!!!!

            //metodo lettura del prezzo ( price type=2 -> grossprice ) prezzo all'utente finale in Campo1 ho il valore in Campo2 ho l'id del tipo di prezzo
            //    <prices>
            //  <price type="2" currency="EUR">
            //    <grossPrice>16990</grossPrice>
            //  </price>
            //  <price type="1" currency="EUR">
            //    <grossPrice>16900</grossPrice>
            //  </price>
            //  <price type="4" currency="EUR">
            //    <grossPrice>16900</grossPrice>
            //  </price>
            //</prices>
            List<Tabrif> pricevalues = ReadPriceValues(kv.Value);
            Tabrif userprice = pricevalues.Find(p => p.Campo2 == "2"); //Prendo lo user price
            if (userprice != null)
            {
                double.TryParse(userprice.Campo1, out _tmpdbl);
                o.Prezzo = _tmpdbl;
            }

            //lettura dei media ( images url > lista image name ) -> esce lista url + name
            //     <media>
            //  <images url="http://picserver.devel/userdata/5/4452/fIw4MFmF/">
            //    <image name="501402378-1.jpg"/>
            //    <image name="501402378-2.jpg"/>
            //    <image name="501402378-3.jpg"/>
            //  </images>
            //  <documents/>
            //</media>
            AllegatiCollection fotolist = ReadMediaValues(kv.Value);
            o.FotoCollection_M = fotolist; //Inserisco le foto nell'elemento ( ATTENZIONE sono path assoluti su un server remoto !!! non relativi al sito base )
            if (fotolist != null && fotolist.Count > 0)
                o.FotoCollection_M.FotoAnteprima = fotolist[0].NomeFile;

            item = ReadXmlSinglevalue(kv.Value, "mainData", "isOnline"); //parametro online  per la visibilità 
            if (bool.TryParse(item.Campo1, out _tmpbool))
                o.Archiviato = !_tmpbool;

            //Lettura dei Colors ( bodycolor id/name (effettivo colore) -> paint id/value (effetto di colore )
            //Torna due elementi uno per bodycolor uno per paint
            //Per ogni elemento Campo1 è il valoro Campo2 è l'id Codice identifica il tag
            //    <colors>
            //  <bodyColors>
            //    <bodyColor id="10">
            //      <name>rot</name>
            //      <paint id="1">solid</paint>
            //    </bodyColor>
            //  </bodyColors>
            //</colors>
            List<Tabrif> colordata = ReadColorvalue(kv.Value);
            //Da integrare metodo per lettura delle options ( id/description -> value ( per value CERCHIAMO la corrispondenza nella tabella riferimento cARATTERISTICA5 )
            List<Tabrif> options = ReadOptionsValues(kv.Value);
            //Altri valori principali del veicolo ( non usati nelle ricerche =
            item = ReadXmlSinglevalue(kv.Value, "mainData", "category"); //vehicle_category //auto, moto,camper ...
            item = ReadXmlSinglevalue(kv.Value, "mainData", "mileage"); //chilometraggio km / scaleUnit
            item = ReadXmlSinglevalue(kv.Value, "mainData", "cylinderCapacity"); //cilindrata cm3   
            item = ReadXmlSinglevalue(kv.Value, "mainData", "enginePower"); //potenza kw   
            item = ReadXmlSinglevalue(kv.Value, "mainData", "transmission"); //trasmission    
            item = ReadXmlSinglevalue(kv.Value, "mainData", "bodyStyle"); //bodyStyle tipologia auto : berlina , monovolume etc...

            //QUI potresti valorizzare e formattare il campo o.datitecniciI con già le caratteristiche da visualizzare in un HTML formattato....
            //...

            //INserisco nel db il record nuovo
            if (o.Id == 0)
                offDM.InsertOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o);
            else
                offDM.UpdateOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o);

        }

        //Infine rimuovo dal db tutti i record rimasti che non sono presenti nella lista di modix
        foreach (Offerte o in listacompletapercodice)
        {
            offDM.DeleteOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o);
            System.Threading.Thread.Sleep(300);
        }
        WelcomeLibrary.UF.Utility.CaricaMemoriaStaticaCaratteristiche("rif000100");//Ricarico le tabelle di riferimento delle caratteristiche di filtraggio
    }

    /// <summary>
    /// Estrae dati da xml come attributi su campo2 e valore su campo1
    /// Va bene per strutture xml del tipo  <category id="1">Kraftfahrzeug (PKW)</category>
    /// </summary>
    /// <param name="testoxml"></param>
    /// <param name="contenitore"></param>
    /// <param name="elementi"></param>
    /// <returns></returns>
    public static Tabrif ReadXmlSinglevalue(string testoxml, string contenitore, string elementi, string filtervalue = "")
    {
        Tabrif item = new Tabrif();
        try
        {
            System.IO.StringReader _sr = new System.IO.StringReader(testoxml); //Stringa Xml per i dati del veicolo!
            System.Xml.XmlTextReader scriptXmlReader = new XmlTextReader(_sr);
            using (scriptXmlReader)
            {
                scriptXmlReader.WhitespaceHandling = WhitespaceHandling.None;
                int numAttributes;
                if (contenitore != "")
                {
                    while (scriptXmlReader.ReadToFollowing(contenitore))
                    {
                        if (scriptXmlReader.Depth == 1) //Carico solo gli elementi di primo livello
                            if (elementi != "" && scriptXmlReader.ReadToDescendant(elementi))
                            {
                                string idelement = scriptXmlReader.GetAttribute("id");
                                //PER GLI ATTRIBUTI  DEL NODO LI LEGGO TUTTI E LI CONCATENO
                                string attributes = "";
                                numAttributes = scriptXmlReader.AttributeCount;
                                for (int i = 0; i < numAttributes; i++)
                                {
                                    attributes += (scriptXmlReader.GetAttribute(i)) + "|";
                                }
                                attributes = attributes.TrimEnd('|');
                                string valore = scriptXmlReader.ReadString();

                                if (filtervalue == "")
                                {
                                    item.Campo2 = attributes;
                                    item.Campo1 = valore;
                                }
                                else
                                {
                                    if (idelement != null && idelement == filtervalue)
                                    {
                                        item.Campo2 = attributes;
                                        item.Campo1 = valore;
                                    }
                                }
                            }
                            else
                            {
                                //PER GLI ATTRIBUTI  DEL NODO LI INSERISCO NEL CAMPO2
                                numAttributes = scriptXmlReader.AttributeCount;
                                for (int i = 0; i < numAttributes; i++)
                                {
                                    item.Campo2 += (scriptXmlReader.GetAttribute(i)) + "|";
                                }
                                item.Campo2 = item.Campo2.TrimEnd('|');
                                item.Campo1 = scriptXmlReader.ReadString();
                            }
                    }
                }

                //while (!stopreading && scriptXmlReader.Read()) //Leggo una riga alla volta
                //{
                //    switch (scriptXmlReader.NodeType)
                //    {
                //        case XmlNodeType.Element:
                //            nodeName = scriptXmlReader.Name;
                //            break;
                //        case XmlNodeType.Text: //Se sono nel contenuto del nodo -> memorizzo il valore
                //            break;
                //        case XmlNodeType.CDATA: //Se sono nel contenuto del nodo -> memorizzo il valore
                //            break;
                //        case XmlNodeType.EndElement:
                //            xmlNodeText = string.Empty;
                //            if (scriptXmlReader.Name == "vehicle" && scriptXmlReader.Depth == 0)
                //                stopreading = true; //Fermo la lettura del file al termine per evitare una read fasulla

                //            break;
                //        default:
                //            break;
                //    }
                //}
            }
        }
        catch
        { }
        return item;
    }

    /// <summary>
    /// Prelievo dati da struttura color / paint
    /// <colors>
    ///      <bodyColors>
    ///        <bodyColor id="10">
    ///          <name>rot</name>
    ///          <paint id="1">solid</paint>
    ///        </bodyColor>
    ///      </bodyColors>
    ///    </colors>
    /// </summary>
    /// <param name="testoxml"></param>
    /// <param name="contenitore"></param>
    /// <param name="elementi"></param>
    /// <param name="filtervalue"></param>
    /// <returns></returns>
    public static List<Tabrif> ReadColorvalue(string testoxml)
    {
        List<Tabrif> list = new List<Tabrif>();
        try
        {
            System.IO.StringReader _sr = new System.IO.StringReader(testoxml); //Stringa Xml per i dati del veicolo!
            Tabrif color = new Tabrif();
            Tabrif paint = new Tabrif();
            System.Xml.XmlTextReader scriptXmlReader = new XmlTextReader(_sr);
            using (scriptXmlReader)
            {
                scriptXmlReader.WhitespaceHandling = WhitespaceHandling.None;
                //int numAttributes;
                while (scriptXmlReader.ReadToFollowing("colors"))
                {
                    if (scriptXmlReader.Depth == 1) //Carico solo gli elementi di primo livello
                    {
                        if (scriptXmlReader.ReadToDescendant("bodyColors"))
                        {
                            if (scriptXmlReader.ReadToDescendant("bodyColor"))
                            {
                                string idcolor = scriptXmlReader.GetAttribute("id");
                                scriptXmlReader.ReadToDescendant("name");
                                string colorname = scriptXmlReader.ReadString();
                                color.Codice = "bodyColor";
                                color.Campo1 = colorname;
                                color.Campo2 = idcolor;
                                list.Add(color);

                                scriptXmlReader.ReadToNextSibling("paint");
                                string idpaint = scriptXmlReader.GetAttribute("id");
                                string paintname = scriptXmlReader.ReadString();
                                paint.Codice = "paint";
                                paint.Campo1 = paintname;
                                paint.Campo2 = idpaint;
                                list.Add(paint);

                                //PER GLI ATTRIBUTI  DEL NODO LI INSERISCO NEL CAMPO2
                                //numAttributes = scriptXmlReader.AttributeCount;
                                //for (int i = 0; i < numAttributes; i++)
                                //{
                                //    item.Campo2 += (scriptXmlReader.GetAttribute(i)) + "|";
                                //}
                                //item.Campo2 = item.Campo2.TrimEnd('|');
                                //item.Campo1 = scriptXmlReader.ReadString();

                            }
                        }
                    }
                }

            }
        }
        catch
        { }
        return list;
    }

    /// <summary>
    /// Prelievo lista prezzi 
    /// <prices>
    ///       <price type="2" currency="EUR">
    ///      <grossPrice>10990</grossPrice>
    ///     </price>
    ///    <price type="1" currency="EUR">
    ///       <grossPrice>10900</grossPrice>
    ///      </price>
    ///     <price type="4" currency="EUR">
    ///       <grossPrice>10900</grossPrice>
    ///     </price>
    ///   </prices>
    /// </summary>
    /// <param name="testoxml"></param>
    /// <returns></returns>
    public static List<Tabrif> ReadPriceValues(string testoxml)
    {

        List<Tabrif> list = new List<Tabrif>();
        try
        {
            System.IO.StringReader _sr = new System.IO.StringReader(testoxml); //Stringa Xml per i dati del veicolo!
            Tabrif item = new Tabrif();

            System.Xml.XmlTextReader scriptXmlReader = new XmlTextReader(_sr);
            using (scriptXmlReader)
            {
                scriptXmlReader.WhitespaceHandling = WhitespaceHandling.None;
                //int numAttributes;
                while (scriptXmlReader.ReadToFollowing("prices"))
                {
                    if (scriptXmlReader.Depth == 1) //Carico solo gli elementi di primo livello
                    {
                        while (scriptXmlReader.ReadToFollowing("price"))
                        {
                            item = new Tabrif();
                            string idtype = scriptXmlReader.GetAttribute("type");
                            string currency = scriptXmlReader.GetAttribute("currency");
                            item.Campo3 = currency;
                            item.Campo2 = idtype;
                            if (scriptXmlReader.ReadToDescendant("grossPrice"))
                            {
                                string valore = scriptXmlReader.ReadString();
                                item.Campo1 = valore;
                                list.Add(item);
                            }
                        }
                    }
                }

            }
        }
        catch
        { }
        return list;
    }

    /// <summary>
    /// Torna la lista delle foto dalla stringa xml
    /// </summary>
    /// <param name="testoxml"></param>
    /// <returns></returns>
    public static AllegatiCollection ReadMediaValues(string testoxml)
    {
        AllegatiCollection fotolist = new AllegatiCollection();
        try
        {
            System.IO.StringReader _sr = new System.IO.StringReader(testoxml); //Stringa Xml per i dati del veicolo!
            List<Tabrif> list = new List<Tabrif>();
            Tabrif item = new Tabrif();
            Allegato foto = new Allegato();
            System.Xml.XmlTextReader scriptXmlReader = new XmlTextReader(_sr);
            using (scriptXmlReader)
            {
                scriptXmlReader.WhitespaceHandling = WhitespaceHandling.None;
                //int numAttributes;
                while (scriptXmlReader.ReadToFollowing("media"))
                {
                    if (scriptXmlReader.Depth == 1) //Carico solo gli elementi di primo livello
                    {
                        scriptXmlReader.ReadToDescendant("images");
                        string baseurl = scriptXmlReader.GetAttribute("url");
                        if (baseurl != "")
                            while (scriptXmlReader.ReadToFollowing("image"))
                            {
                                item = new Tabrif();
                                string nomefile = scriptXmlReader.GetAttribute("name");
                                item.Campo1 = baseurl + nomefile;
                                item.Campo2 = baseurl;

                                if (!list.Exists(t => t.Campo1 == item.Campo1))
                                    list.Add(item);

                                foto = new Allegato();
                                foto.NomeFile = baseurl + nomefile;
                                if (!fotolist.Exists(t => t.NomeFile == foto.NomeFile))
                                    fotolist.Add(foto);
                            }
                    }
                }
            }
            offerteDM offDM = new offerteDM();
            fotolist = offDM.CreaStringheAllegati(fotolist);
        }
        catch
        { }
        return fotolist;
    }

    /// <summary>
    /// Legge la lista delle opzioni per il veicolo e la torna un una lista tabrif
    /// Campo2 -> id opzione Campo1 -> Valore Opzione Codice -> Descrizione Opzione
    /// </summary>
    /// <param name="testoxml"></param>
    /// <returns></returns>
    public static List<Tabrif> ReadOptionsValues(string testoxml)
    {
        List<Tabrif> list = new List<Tabrif>();
        try
        {
            System.IO.StringReader _sr = new System.IO.StringReader(testoxml); //Stringa Xml per i dati del veicolo!
            Tabrif item = new Tabrif();
            System.Xml.XmlTextReader scriptXmlReader = new XmlTextReader(_sr);
            using (scriptXmlReader)
            {
                scriptXmlReader.WhitespaceHandling = WhitespaceHandling.None;
                //int numAttributes;
                while (scriptXmlReader.ReadToFollowing("options"))
                {
                    if (scriptXmlReader.Depth == 1) //Carico solo gli elementi di primo livello
                    {
                        while (scriptXmlReader.ReadToFollowing("option"))
                        {
                            item = new Tabrif();
                            string idopzione = scriptXmlReader.GetAttribute("id"); //id codice opzione in tabella
                            item.Campo2 = idopzione;
                            if (scriptXmlReader.ReadToDescendant("value"))
                            {
                                item.Campo1 = scriptXmlReader.ReadString(); //valore dell'opzione
                            }
                            if (scriptXmlReader.ReadToNextSibling("description"))
                            {
                                item.Codice = scriptXmlReader.ReadString(); //Descrizione dell'opzione
                            }
                            list.Add(item);
                        }
                    }
                }
            }
        }
        catch
        { }
        return list;
    }

#if false

    private static void ReadXmlMetodo1(Dictionary<string, Dictionary<string, Tabrif>> dict, string key, XmlTextReader scriptXmlReader, string tag)
    {
        bool stopreading = false;
        string xmlNodeText = "";
        string nodeName = "";
        int numAttributes;


        if (scriptXmlReader.NodeType == XmlNodeType.Element) //Se entro già su un elemento lo metto nel dictonary
        {
            string idattr = scriptXmlReader.GetAttribute("id");
            //bool startElement = scriptXmlReader.IsStartElement();
            //string nodevalue = scriptXmlReader.ReadString();
            nodeName = scriptXmlReader.Name;
            //Caso default
            if (dict.ContainsKey(key) && !dict[key].ContainsKey(nodeName))
            {
                dict[key].Add(nodeName, new Tabrif());
            }
            //PER GLI ATTRIBUTI ULTERIORI DEL NODO LI INSERISCO NEL CAMPO2
            numAttributes = scriptXmlReader.AttributeCount;
            for (int i = 0; i < numAttributes; i++)
            {
                ((Tabrif)dict[key][nodeName]).Campo2 += (scriptXmlReader.GetAttribute(i)) + "|";
            }
            ((Tabrif)dict[key][nodeName]).Campo2 = ((Tabrif)dict[key][nodeName]).Campo2.TrimEnd('|');
        }

#if true
        while (!stopreading && scriptXmlReader.Read()) //Leggo una riga alla volta
        {
            switch (scriptXmlReader.NodeType)
            {
                case XmlNodeType.Element:
                    string idattr = scriptXmlReader.GetAttribute("id");
                    //bool startElement = scriptXmlReader.IsStartElement();
                    //string nodevalue = scriptXmlReader.ReadString();
                    nodeName = scriptXmlReader.Name;
                    //Caso default
                    if (dict.ContainsKey(key) && !dict[key].ContainsKey(nodeName))
                    {
                        dict[key].Add(nodeName, new Tabrif());
                    }
                    //PER GLI ATTRIBUTI ULTERIORI DEL NODO LI INSERISCO NEL CAMPO2
                    numAttributes = scriptXmlReader.AttributeCount;
                    for (int i = 0; i < numAttributes; i++)
                    {
                        ((Tabrif)dict[key][nodeName]).Campo2 += (scriptXmlReader.GetAttribute(i)) + "|";
                    }
                    ((Tabrif)dict[key][nodeName]).Campo2 = ((Tabrif)dict[key][nodeName]).Campo2.TrimEnd('|');

                    break;

                case XmlNodeType.Text: //Se sono nel contenuto del nodo -> memorizzo il valore
                    xmlNodeText = scriptXmlReader.Value;
                    if (dict.ContainsKey(key) && dict[key].ContainsKey(nodeName))
                        ((Tabrif)dict[key][nodeName]).Campo1 = xmlNodeText;

                    break;
                case XmlNodeType.CDATA: //Se sono nel contenuto del nodo -> memorizzo il valore
                    xmlNodeText = scriptXmlReader.Value;
                    if (dict.ContainsKey(key) && dict[key].ContainsKey(nodeName))
                        ((Tabrif)dict[key][nodeName]).Campo1 = xmlNodeText;

                    break;
                case XmlNodeType.EndElement:
                    xmlNodeText = string.Empty;
                    if (scriptXmlReader.Name == "vehicle" && scriptXmlReader.Depth == 0)
                        stopreading = true; //Fermo la lettura del file al termine per evitare una read fasulla
                    if (scriptXmlReader.Name == tag) return;//Se esco dal livello nodo torno al chiamante
                    break;
                default:
                    break;
            }

        }
#endif
    }
#endif

    #endregion

    #region FUNZIONI PER INMPORT EXPORT CONTENUTI DA ALTRI SITI

    /// <summary>
    /// Carica un dictionary con i dati provenienti da altri portali che
    /// prevedono l'esportazione delle offerte in formato XML
    /// </summary>
    /// <param name="codicecontenuto"></param>
    /// <param name="nrecord"></param>
    public static void GetContentFromWeb(HttpServerUtility server, string sourcebaseaddress, string codicedestinazione, string nrecord = "0")
    {
        int recordlimit = 0;
        int.TryParse(nrecord, out recordlimit);

        // HttpRequest hreq = new HttpRequest();
        System.Xml.XmlTextReader scriptXmlReader = new System.Xml.XmlTextReader(sourcebaseaddress);
        Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();

        //XmlTextReader scriptXmlReader = new XmlTextReader(scriptXmlString);
        scriptXmlReader.WhitespaceHandling = WhitespaceHandling.None;

        string xmlNodeText = "";
        string nodeName = "";
        long idnew = 0;
        bool stopreading = false;
        using (scriptXmlReader)
        {
            while (!stopreading && scriptXmlReader.Read())
            {

                switch (scriptXmlReader.NodeType)
                {
                    case XmlNodeType.Element:

                        //bool startElement = scriptXmlReader.IsStartElement();
                        nodeName = scriptXmlReader.Name;
                        //string nodevalue = scriptXmlReader.ReadString();

                        //Per ogni item -> creo un nuovo elemento primario nel dict
                        if (nodeName == "item")
                        {
                            idnew += 1;//incremento l'indice
                            if (!dict.ContainsKey(idnew.ToString()))
                                dict.Add(idnew.ToString(), new Dictionary<string, string>());
                        }
                        else if (dict.ContainsKey(idnew.ToString()) && !dict[idnew.ToString()].ContainsKey(nodeName))
                        {
                            dict[idnew.ToString()].Add(nodeName, "");
                        }

                        //NON HO ATTRIBUTI
                        //int numAttributes = scriptXmlReader.AttributeCount;
                        //for (int i = 0; i < numAttributes; i++)
                        //{
                        //    string attributeValue = scriptXmlReader.GetAttribute(i);
                        //}
                        break;

                    case XmlNodeType.Text: //Se sono nel contenuto del nodo -> memorizzo il valore
                        xmlNodeText = scriptXmlReader.Value;
                        if (dict.ContainsKey(idnew.ToString()) && dict[idnew.ToString()].ContainsKey(nodeName))
                            dict[idnew.ToString()][nodeName] = xmlNodeText;
                        break;
                    case XmlNodeType.CDATA: //Se sono nel contenuto del nodo -> memorizzo il valore
                        xmlNodeText = scriptXmlReader.Value;
                        if (dict.ContainsKey(idnew.ToString()) && dict[idnew.ToString()].ContainsKey(nodeName))
                            dict[idnew.ToString()][nodeName] = xmlNodeText;
                        break;
                    case XmlNodeType.EndElement:
                        xmlNodeText = string.Empty;
                        if (scriptXmlReader.Name == "itemlist" && scriptXmlReader.Depth == 0)
                            stopreading = true; //Fermo la lettura del file al termine per evitare una read fasulla
                        break;
                    default:
                        break;
                }
            }
            AggiornaDatabasePostLocale(server, dict, codicedestinazione);
            //  AggiornaMemoriaStaticaPost(server, dict, codicedestinazione, scaricafilesinlocale);
        }

    }
    public static void AggiornaDatabasePostLocale(HttpServerUtility server, Dictionary<string, Dictionary<string, string>> dict, string codicedestinazione)
    {
        List<Offerte> list = new List<Offerte>();
        offerteDM offDM = new offerteDM();
        foreach (KeyValuePair<string, Dictionary<string, string>> kv in dict) //Scorro tutti i post letti e li memorizzo nel db
        {
            Offerte item = new Offerte();
            Dictionary<string, string> values = kv.Value;
            item.CodiceTipologia = codicedestinazione;

            //Prendiamo i valori di interess
            DateTime t = DateTime.MinValue;
            if (values.ContainsKey("pubDate"))
                DateTime.TryParse(values["pubDate"], out t);
            item.DataInserimento = t;
            if (values.ContainsKey("title"))
                item.DenominazioneI = values["title"].ToLower();
            //Inserisco un a capo dopo tot caratteri!!!
            if (item.DenominazioneI.Length > 40)
            {
                int posspazio = item.DenominazioneI.IndexOf(' ', 30);
                if (posspazio != -1 && posspazio < 45)
                {
                    item.DenominazioneI = item.DenominazioneI.Insert(posspazio, "\r\n");
                }
            }
            item.DenominazioneI = WelcomeLibrary.UF.Utility.ConvertHtmlToPlainText(item.DenominazioneI.Replace("<br/>", "\r\n").Replace("<br />", "\r\n"));


            if (values.ContainsKey("source"))
                item.DescrizioneI = values["source"] + "<br/>";
            if (values.ContainsKey("description"))
                item.DescrizioneI += values["description"];

            //Estraiamo il codice di provenienza dal record letto in remoto
            string id_origine_str = "0";
            int id_origine = 0;
            string indirizzocontenutocompleto = values["guid"];

            //Comunicati
            if (values.ContainsKey("guid"))
            {
                id_origine_str = values["guid"];
                id_origine_str = id_origine_str.Trim().TrimEnd('/');
                int lastpos = id_origine_str.LastIndexOf('/');
                if (lastpos != -1)
                    id_origine_str = id_origine_str.Substring(lastpos + 1);
                int.TryParse(id_origine_str, out id_origine);
                item.Id_collegato = id_origine; //Metto l'id di provenienza nel campo degli id collegati!!!
                if (item.Id_collegato != 0)
                {  //Carichiamo adesso i contenuti completi dalla pagina di dettaglio ( Chiamata WEB )
                    string fullcontent = WelcomeLibrary.UF.SharedStatic.MakeHttpHtmlGet(indirizzocontenutocompleto, 1252);
                    int poscontent = fullcontent.IndexOf(item.DescrizioneI);
                    string contenutocompleto = "";
                    if (poscontent != -1)
                    {
                        int startpos = fullcontent.IndexOf("<p>", poscontent + item.DescrizioneI.Length);
                        if (startpos != -1)
                        {
                            int endpos = fullcontent.IndexOf("</p>", startpos + item.DescrizioneI.Length);

                            if (startpos != -1 && endpos != -1 && endpos > poscontent)
                            {
                                contenutocompleto = fullcontent.Substring(startpos + 3, endpos - (startpos + 3));
                                item.DescrizioneI += "<br/><br/>" + contenutocompleto;
                            }
                        }
                    }
                }
            }

            //Rassegne stampa
            string urlfile = "";
            string filename = "";
            if (values.ContainsKey("guid"))
            {
                //Provo a prendere l'id in altra forma ( rassegne stampa )
                if (item.Id_collegato == 0)
                {
                    string testodescrizione = "";
                    id_origine_str = values["guid"];
                    id_origine_str = id_origine_str.Trim().TrimEnd('/');
                    int lastpos = id_origine_str.ToLower().LastIndexOf("id_art=");
                    if (lastpos != -1)
                        id_origine_str = id_origine_str.Substring(lastpos + 7);
                    int.TryParse(id_origine_str, out id_origine);
                    item.Id_collegato = id_origine; //Metto l'id di provenienza nel campo degli id collegati!!!
                    if (item.Id_collegato != 0)
                    {
                        //Estraiamo il file da caricare in allegato
                        //es. <a href='http://www.eoipso.it/include/rassegna/getfile.cfm?file_ID=53433'>Aicpe_BlitzQuotidiano2907.pdf</a>
                        int startpos = item.DescrizioneI.IndexOf("<a href='");

                        if (startpos != -1)
                        {
                            testodescrizione = item.DescrizioneI.Substring(0, startpos);
                            int endpoint = item.DescrizioneI.IndexOf("'", startpos + 9);
                            if (endpoint != -1)
                            {
                                urlfile = item.DescrizioneI.Substring(startpos + 9, endpoint - (startpos + 9));
                                startpos = item.DescrizioneI.IndexOf(">", endpoint);
                                if (startpos != -1)
                                {
                                    endpoint = item.DescrizioneI.IndexOf("<", startpos);
                                    if (endpoint != -1)
                                    {
                                        filename = item.DescrizioneI.Substring(startpos + 1, endpoint - (startpos + 1));


                                    }
                                }
                            }
                            item.DescrizioneI = testodescrizione;//elimino il link dal testo di provenienza
                        }
                    }
                }
            }
            // list.Add(item);


            item.DescrizioneI = WelcomeLibrary.UF.Utility.ConvertHtmlToPlainText(item.DescrizioneI.Replace("<br/>", "\r\n").Replace("<br />", "\r\n"));

            //AGGIORNIAMO IL DATABASE LOCALE
            //Facciamo una get nel db  per l'offerta con l'id_collegato specifico
            //--> se presente aggiorno quella
            //--> altrimenti inserisco
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter ptipologia = new OleDbParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);
            parColl.Add(ptipologia);
            if (id_origine != 0)
            {
                OleDbParameter pid_origine = new OleDbParameter("@Id_collegato", id_origine);
                parColl.Add(pid_origine);
            }
            List<Offerte> offertecollegate = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "1", null, null, "", true);

            if (offertecollegate != null && offertecollegate.Count == 1)
            {
                item.Id = offertecollegate[0].Id;//Aggiorno il record presente!!
                //update ( non permetto l'edit di altri campi oltre quelli che vengono dal db esterno )
                offDM.UpdateOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item);
            }
            else
            {
                //Insert
                offDM.InsertOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item);
            }

            //Fatto il caricamento se presente carico il file in allegato
            if (!string.IsNullOrEmpty(urlfile) && !string.IsNullOrEmpty(filename))
            {


                try
                {
                    //Allego il file prendendolo da web
                    CaricaFile(server, urlfile, filename, item.CodiceTipologia, item.Id.ToString()); //Inserisco i file che devo in allegato
                }
                catch (Exception err)
                {

                }
            }
        }
    }
    private static string CaricaFile(HttpServerUtility server, string urlfile, string Nomefile, string codicetipologia, string idrecord)
    {
        string responsestr = "";
        try
        {
            //Controlliamo se ho selezionato un record
            if (idrecord == null || idrecord == "")
            {
                return "No id selected!";
            }
            int idSelected = 0;
            if (!Int32.TryParse(idrecord, out idSelected))
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


    public static void AggiornaMemoriaStaticaPost(HttpServerUtility server, Dictionary<string, Dictionary<string, string>> dict, string codicedestinazione, bool scaricafilesinlocale = true)
    {
        contenutiDM conDM = new contenutiDM();
        //Memoria statica contenitore contenuti dal web
        //WelcomeLibrary.UF.Utility.ListaContenuti = new WelcomeLibrary.DOM.ContenutiCollection();

        //Riempiamo la memoria dei contatti per la visualizzazione nelle pagine dei contenuti
        //IMposto il codice contenuto di destinazione per la scrittura nel server di destinazione
        string codicecontenutoweb = codicedestinazione;
        foreach (KeyValuePair<string, Dictionary<string, string>> kv in dict)
        {
            Contenuti c = new Contenuti();
            //  conDM.InsertContenuto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c);//Inserisco nel db locale

            Dictionary<string, string> values = kv.Value;

            //Formattiamo i dati nell'oggetto contenuti
            c.CodiceContenuto = codicecontenutoweb; //Impongo il codice per i contenuti provenienti dal web

#if true
            int _id = 0;
            int.TryParse(kv.Key, out _id);
            c.Id = _id; //assegno l'identificativo progressivo del contenuto preso dal web   
#endif

            DateTime t = DateTime.MinValue;
            if (values.ContainsKey("DataInserimento"))
                DateTime.TryParse(values["DataInserimento"], out t);
            c.DataInserimento = t;

            if (values.ContainsKey("DescrizioneGB"))
                c.DescrizioneGB = values["DescrizioneGB"];

            if (values.ContainsKey("DescrizioneI"))
                c.DescrizioneI = values["DescrizioneI"];

            if (values.ContainsKey("DescrizioneRU"))
                c.DescrizioneI = values["DescrizioneRU"];

            if (values.ContainsKey("DatitecniciGB"))
                c.DescrizioneGB += values["DatitecniciGB"];
            if (values.ContainsKey("DatitecniciI"))
                c.DescrizioneI += values["DatitecniciI"];
            if (values.ContainsKey("DatitecniciRU"))
                c.DescrizioneI += values["DatitecnicRU"];

            if (values.ContainsKey("DenominazioneGB"))
                c.TitoloGB = values["DenominazioneGB"];
            if (values.ContainsKey("DenominazioneI"))
                c.TitoloI = values["DenominazioneI"];
            if (values.ContainsKey("DenominazioneRU"))
                c.TitoloI = values["DenominazioneRU"];
            //if (values.ContainsKey("linkVideo"))
            //    c.linkVideo = values["linkVideo"];

            //appoggio i dati dell'offerta presa dal web in offerta associata
            c.offertaassociata = new Offerte();
            if (values.ContainsKey("CodiceTipologia"))
                c.offertaassociata.CodiceTipologia = values["CodiceTipologia"];
            if (values.ContainsKey("CodiceComune"))
                c.offertaassociata.CodiceComune = values["CodiceComune"];
            if (values.ContainsKey("CodiceProvincia"))
                c.offertaassociata.CodiceProvincia = values["CodiceProvincia"];
            if (values.ContainsKey("CodiceRegione"))
                c.offertaassociata.CodiceRegione = values["CodiceRegione"];
            if (values.ContainsKey("CodiceProdotto"))
                c.offertaassociata.CodiceProdotto = values["CodiceProdotto"];
            if (values.ContainsKey("Email"))
                c.offertaassociata.Email = values["Email"];
            if (values.ContainsKey("Fax"))
                c.offertaassociata.Fax = values["Fax"];
            if (values.ContainsKey("Indirizzo"))
                c.offertaassociata.Indirizzo = values["Indirizzo"];
            if (values.ContainsKey("Telefono"))
                c.offertaassociata.Telefono = values["Telefono"];
            if (values.ContainsKey("Website"))
                c.offertaassociata.Website = values["Website"];
            double pr = 0;
            double.TryParse(values["Prezzo"], out pr);
            if (values.ContainsKey("Prezzo"))
                c.offertaassociata.Prezzo = pr;

            //Le foto le metto col path assoluto riferito al portale sorgente!!
            AllegatiCollection listafoto = new AllegatiCollection();
            if (values.ContainsKey("UrlPhoto"))
            {
                Allegato a = new Allegato();
                int _prog = 0;
                string[] photos = values["UrlPhoto"].Split('|'); //Prendo gli indirizzi assoluti delle foto
                if (photos != null)
                    foreach (string p in photos)
                    {
                        string nomecompletofile = p.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                        if (string.IsNullOrEmpty(nomecompletofile)) continue;
                        a = new Allegato();
                        a.NomeFile = nomecompletofile;
                        a.Progressivo = _prog;
                        if (_prog == 0) listafoto.FotoAnteprima = p;//la prima è quella di anteprima
                        listafoto.Add(a);
                        _prog += 1;
                        //Trasferisce i files sul server locale per l'utilizzo
                        //all'interno della cartella dei contenuti provenienti dal web
                        if (c.Id != 0 && nomecompletofile.LastIndexOf("/") != -1 && scaricafilesinlocale)
                        {
                            string destpath = server.MapPath(WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + c.CodiceContenuto + "/" + c.Id);
                            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(destpath);
                            if (!di.Exists) // Se presente svuoto
                                di.Create();
                            else if (_prog == 0) //ALla prima volta la svuoto!
                                di.Delete(true);

                            string filename = destpath + "\\" + nomecompletofile.Substring(nomecompletofile.LastIndexOf("/") + 1);
                            WelcomeLibrary.UF.SharedStatic.MakeHttpGet(nomecompletofile, filename);
                        }
                    }
            }
            c.FotoCollection_M = listafoto;
            if (values.ContainsKey("DescriptionPhoto"))
            {
                string[] photosdesc = values["DescriptionPhoto"].Split('|'); //Prendo le descrizioni 
                int i = 0;
                if (c.FotoCollection_M != null || photosdesc != null)
                    foreach (string d in photosdesc)
                    {
                        string descrizione = d.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                        if (string.IsNullOrEmpty(descrizione)) continue;
                        if (c.FotoCollection_M.Count > i)
                            c.FotoCollection_M[i].Descrizione = descrizione;
                        i += 1;
                    }
            }
            if (values.ContainsKey("FotoSchema"))
                c.FotoCollection_M.Schema = values["FotoSchema"];
            if (values.ContainsKey("FotoValori"))
                c.FotoCollection_M.Valori = values["FotoValori"];
            //Con le seguenti le foto sono prese dalla collection anziche linkate in remoto
#if false
            c.FotoCollection_M = new AllegatiCollection();
            c.FotoCollection_M = conDM.CaricaAllegatiFoto(c.FotoCollection_M); 
#endif

            // conDM.UpdateContenuti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c);//Inserisco nel db locale

            // WelcomeLibrary.UF.Utility.ListaContenuti.Add(c);
        }

    }


    #endregion
}

public interface IContentPlaceHolders
{
    IList GetContentPlaceHolders();
}


