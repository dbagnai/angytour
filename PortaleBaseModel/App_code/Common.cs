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
using System.Data.SQLite;
using System.Text;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Globalization;


public class simpleidname
{
    public string id { set; get; }
    public string name { set; get; }
}


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
    public static string BreadcrumbConstruction(List<Tabrif> links, bool resetsession = true)
    {
        //Creo la struttura annidata dei link
        StringBuilder sb = new StringBuilder();
        int i = 1;
        foreach (Tabrif l in links)
        {
            sb.Append("<li itemprop=\"itemListElement\" itemscope itemtype=\"https://schema.org/ListItem\"> ");

            //if (links.Count != i)
            //{
            sb.Append("<a itemtype=\"https://schema.org/Thing\" itemprop=\"item\"");
            sb.Append(" href=\"" + l.Campo1 + "\">");
            sb.Append("<span itemprop=\"name\">");
            sb.Append(l.Campo2);
            sb.Append("</span>");
            sb.Append("</a> ");
            //}
            //else
            //{
            //    sb.Append(l.Campo2);
            //}

            sb.Append("<meta itemprop=\"position\" content=\"" + i + "\" />");
            sb.Append("</li>");

            i++;

        }
        return sb.ToString();
    }


    /// <summary>
    ///  Ritorna un contenuto renderizzato da template come stringa html
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="Lingua"></param>
    /// <param name="username"></param>
    /// <param name="sessione"></param>
    /// <returns></returns>
    public static string HtmlfromteplateInject(string filename, string Lingua, string username, System.Web.SessionState.HttpSessionState sessione = null)
    {
        string htmlret = "";
        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + filename))
        {
            string bindedcustomcontent = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + filename);
            custombind cb = new custombind();
            htmlret = cb.bind(bindedcustomcontent, Lingua, username, sessione, null, null, System.Web.HttpContext.Current.Request);// sb.ToString();
        }
        return htmlret;
    }

    /// <summary>
    /// Aggiunge un contenuto renderizzato da template nel controllo specificato
    /// </summary>
    /// <param name="divcontainer"></param>
    /// <param name="filename"></param>
    /// <param name="Lingua"></param>
    /// <param name="username"></param>
    /// <param name="sessione"></param>
    public static void CustomContentInject(System.Web.UI.HtmlControls.HtmlGenericControl divcontainer, string filename, string Lingua, string username, System.Web.SessionState.HttpSessionState sessione = null)
    {

        if (divcontainer != null && System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + filename))
        {

            string bindedcustomcontent = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + filename);
            custombind cb = new custombind();
            divcontainer.InnerHtml = cb.bind(bindedcustomcontent, Lingua, username, sessione, null, null, System.Web.HttpContext.Current.Request);// sb.ToString();
            divcontainer.Visible = true;
        }
    }


    public static string ControlloDotDot(object NomeAnteprima, string classe)
    {
        string ret = classe;
        if (NomeAnteprima == null || NomeAnteprima.ToString() == "")
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
    public static string ReplaceCdnLinks(string testo)
    {

        string path = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
        string cdnuse = WelcomeLibrary.UF.ConfigManagement.ReadKey("usecdn").ToLower();
        string pathcdn = WelcomeLibrary.UF.ConfigManagement.ReadKey("percorsocdn").ToLower();
        if (cdnuse == "true")
        {
            path = pathcdn + "/" + System.Web.HttpContext.Current.Request.Url.Host.ToString();

        }

        return testo.Replace("~", path);
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

        //if (cachedurl)
        //    writer( htmldachache)
        //        else standard renderd


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

#if true
        /////////////////////////
        //////MINIFICAZIONE HTML
        /////////////////////////
        //righe per minificare html in pagina ( problema funzionamento updatepanels nei postback !!! )
        if (!(IsPostBack || IsCallback)) //se vuoi evitare manipolazione nei post o callback ( per updatepanel)
            html = NUglify.Uglify.Html(html).Code; 
#endif
        // da salvare l'html per la cache in corrispondenza dell' Request.RawUrl

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

        if (NomeAnteprima == null || NomeAnteprima.ToString() == "")
            ret = true;
        return ret;
    }

    public static bool CheckCanonicalUrl(string calledurl, string canonicalurl, bool removeqstring = true)
    {
        //redirect al canonical se il canonical non coincide con l'url
        if (calledurl.IndexOf("?") != -1 && removeqstring)
            calledurl = calledurl.Substring(0, calledurl.IndexOf("?"));
        if (calledurl != canonicalurl) return false;
        else return true;
        //response.StatusCode = 301; // se il canonical è diverso dall'url ritorno moved permanently
    }

    public static string CreaLinkRoutes(System.Web.SessionState.HttpSessionState sessione = null, bool vuotasession = false, string Lingua = "I", string denominazione = "", string id = "", string codicetipologia = "", string codicecategoria = "", string codicecat2liv = "", string regione = "")
    {

        //bool gen = false;
        //bool.TryParse(ConfigManagement.ReadKey("generaUrlrewrited"), out gen);
        string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, denominazione, id, codicetipologia, codicecategoria, codicecat2liv, regione, "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
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
            string linkgenerato = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, item.Descrizione, "", CodiceTipologia, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
            if (!string.IsNullOrEmpty(qstring)) linkgenerato += "?" + qstring;
            if (!notag)
            {
                ret = "<a href=\"" + linkgenerato + "\" ";
                if (!string.IsNullOrEmpty(cssclass))
                    ret += "class=\"" + cssclass + "\"";
                //ret += "   onclick =\"javascript:JsSvuotaSession(this)\" " ;
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
                    ret += "class=\"" + cssclass + "\"";
                //ret += "   onclick =\"javascript:JsSvuotaSession(this)\" " ;
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
        return SitemapManager.ConteggioCaratteri(testo, caratteri, nolink, testoAggiunto);
#if false
        tring ritorno = testo;

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
#endif
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

        if (chiave.ToLower() == "lingua") ret = SitemapManager.getLinguafromculture(ret);//  per modifica codici culture lingua 19.12.18

        if (ret == "-") ret = "";
        //if (sessione != null)
        //    sessione.Add(chiave, ret);//Metto in sessione il valore
        return ret;
    }

    public static String CleanInput(string strIn)
    {
        return SitemapManager.CleanInput(strIn);
#if false
        // Replace invalid characters with empty strings.
        //return Regex.Replace(strIn, @"[^\w\.@-]", "");
        // strIn = Regex.Replace(strIn, @"[\W]", "");
        //strIn = strIn.Replace(" ", "-");
        strIn = Regex.Replace(strIn, @"[^a-zA-Zа-яА-ЯЁё0-9@\$=!:.’#%_?^<>()òàùèì &°:;-]", "");

        return strIn; 
#endif
    }
    public static String CleanUrl(string strIn)
    {
        return SitemapManager.CleanUrl(strIn);
#if false
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
        strIn = strIn.Replace("&", "e");
        // strIn = Regex.Replace(strIn, @"[^a-zA-Z0-9@\_]", "");
        strIn = Regex.Replace(strIn, @"[^a-zA-Zа-яА-ЯЁё0-9@\$=_()-]", "");
        return strIn.Trim('-'); 
#endif

    }

    /// <summary>
    /// Rimpiazza link:(www.sitodavadere.it) con un link html
    /// oppure link:(www.sitodavadere.it|testo visualizzato del link)
    /// <param name="strIn"></param>
    /// <returns></returns>
    public static String ReplaceLinks(string strIn, bool nolink = false)
    {
        return offerteDM.ReplaceLinks(strIn, nolink);


    }

    private static string GetLinguaFromActualCulture(CultureInfo currentCulture)
    {
        return offerteDM.GetLinguaFromActualCulture(currentCulture);

    }
    public static System.Globalization.CultureInfo setCulture(string lng)
    {
        string culturename = "";
        switch (lng)
        {
            case "I":
                culturename = "it";
                break;
            case "GB":
                culturename = "en";
                break;
            case "RU":
                culturename = "ru";
                break;
            case "FR":
                culturename = "fr";
                break;
            default:
                culturename = "it";
                break;
        }
        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culturename);
        return ci;
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


    protected static string getidsocio(string utente)
    {
        return usermanager.getidsocio(utente);
    }

    public static string getFirstName(string utente)
    {
        return usermanager.getFirstName(utente);
    }

    public static string getidcliente(string utente)
    {
        return usermanager.getidcliente(utente);
    }
    public static string getmailuser(string utente)
    {
        return usermanager.getmailuser(utente);
    }

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
    public static string TestoCaratteristicaJson(string IdJson, string codeJson, string Lingua)
    {
        return references.TestoCaratteristicaJson(IdJson, codeJson, Lingua);
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
    protected void SetCulture(string culturename = "it")
    {
        System.Globalization.CultureInfo c;
        c = System.Globalization.CultureInfo.CreateSpecificCulture(culturename);
        System.Threading.Thread.CurrentThread.CurrentCulture = c;
        System.Threading.Thread.CurrentThread.CurrentUICulture = c;
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
            case "FR":
                c = System.Globalization.CultureInfo.CreateSpecificCulture("fr");
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
        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
        string tipologia = "rif000004";
        if (tipologia != "" && tipologia != "-")
        {
            SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", tipologia);
            parColl.Add(p3);
        }
        SQLiteParameter psrch = new SQLiteParameter("@Archiviato", true);
        parColl.Add(psrch);
        SQLiteParameter pdini = new SQLiteParameter("@Data_inizio", "01/01/1900");
        SQLiteParameter pdfin = new SQLiteParameter("@Data_fine", System.DateTime.Now.AddDays(-1).ToString());
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

    public static string Selezionadajson(object item, string key, string Lingua)
    {
        string ret = "";
        if (item != null && item.ToString() != "")
        {
            try
            {
                ret = "<b>" + references.ResMan("basetext", Lingua, "formtesto" + key) + ": " + "</b>";
                Dictionary<string, string> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(item.ToString());
                if (dic != null && dic.ContainsKey(key))
                    ret += dic[key];
                else
                    ret = "";
            }
            catch { }
        }
        return ret;
    }

    public static string VisualizzaCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, string codiceordine, bool nofoto = false, string Lingua = "I", bool serializeddatas = false, bool perstampa = false)
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
        if (!serializeddatas)
        {
            foreach (Carrello c in carrello)
            {
                //Creiamo la visualizzione degli articoli in carrello
                // da fare <li>  contenuto da prendere sotto  </li>
                sb.Append("<li style=\"position:relative;border-bottom:1px solid #ddd\">");
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
                            //linkofferta = CommonPage.ReplaceAbsoluteLinks(CommonPage.CreaLinkRoutes(null, false, Lingua, CommonPage.CleanUrl(c.Offerta.UrltextforlinkbyLingua(Lingua)), c.Offerta.Id.ToString(), c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, ""));
                            linkofferta = CommonPage.ReplaceAbsoluteLinks(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, CommonPage.CleanUrl(c.Offerta.UrltextforlinkbyLingua(Lingua)), c.Offerta.Id.ToString(), c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl));

                            testoofferta = CommonPage.CleanInput(CommonPage.ConteggioCaratteri(c.Offerta.DenominazioneI, 300, true));
                            imgofferta = CommonPage.ReplaceAbsoluteLinks(filemanage.ComponiUrlAnteprima(c.Offerta.FotoCollection_M.FotoAnteprima, c.Offerta.CodiceTipologia, c.Offerta.Id.ToString()));
                            titoloofferta = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(c.Offerta.DenominazioneI);
                        }
                    }
                    catch { }
                }

                if (!perstampa)
                {
                    if (!nofoto)
                    {
                        sb.Append("<a target=\"_blank\"   href=\"" +
                            linkofferta
                               + "\"  class=\"product-thumb pull-left\" style=\"margin:0; border:none;\"  >");
                        sb.Append(" <div class=\"cart-image\">");
                        sb.Append("<img alt=\"" + testoofferta + "\" Style=\"width: auto; height: auto; max-width: 100%; max-height: 100%;\" ");
                        sb.Append(" src=\"");
                        sb.Append(imgofferta + "\" ");
                        sb.Append("\" />");

                        sb.Append(" </div>");

                        sb.Append(" </a>");
                    }
                    sb.Append(" <div class=\"clearfix\"></div>");
                    sb.Append(" <div class=\"product-details\" style=\"\">");
                    sb.Append(" <p class=\"product-name\" style=\"display:none\"><b>");
                    sb.Append(titoloofferta);
                    sb.Append(" </b></p>");
                }
                else
                {
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
                    sb.Append(" <p class=\"product-name\" style=\"\">");
                    sb.Append(titoloofferta);
                    sb.Append(" </p>");

                }

                //sb.Append(" <p class=\"product-calc muted\">");
                //sb.Append(c.Numero + "&times;" + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { c.Prezzo }) + " €");
                //sb.Append(" </p>");
                //sb.Append(" <div class=\"product-categories muted\">");
                //sb.Append(CommonPage.TestoCategoria(c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, Lingua));
                //sb.Append(" </div>");
                //sb.Append(" <div class=\"product-categories muted\">");
                //sb.Append(CommonPage.TestoCaratteristica(0, c.Offerta.Caratteristica1.ToString(), Lingua));
                //sb.Append(" </div>");
                //sb.Append(" <div class=\"product-categories muted\">");
                //if (c.Offerta.Caratteristica6 != 0)
                //    sb.Append(CommonPage.TestoCaratteristica(5, c.Offerta.Caratteristica6.ToString(), Lingua));
                //sb.Append(" </div>");

                #region MODIFIED CARATTERISTICHE CARRELLO
                if (!string.IsNullOrEmpty(c.Offerta.Xmlvalue))
                {
                    sb.Append(" <p class=\"product-categories muted\">");
                    //recupero le caratteristiche del prodotto
                    List<ModelCarCombinate> listCar = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(c.Offerta.Xmlvalue);
                    ModelCarCombinate item = listCar.Find(e => e.id == c.Campo2);
                    if (item != null)
                    {
                        string testoinlinguacar1 = item.caratteristica1.value;
                        Tabrif Car1 = Utility.Caratteristiche[0].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.caratteristica1.codice; });
                        if (Car1 != null)
                            testoinlinguacar1 = Car1.Campo1;

                        string testoinlinguacar2 = item.caratteristica2.value;
                        Tabrif Car2 = Utility.Caratteristiche[1].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.caratteristica2.codice; });
                        if (Car2 != null)
                            testoinlinguacar2 = Car2.Campo1;

                        sb.Append(testoinlinguacar1 + "  -  " + testoinlinguacar2);
                    }

                    sb.Append(" </p>");
                }
                #endregion

                //CARATTERISTICHE CARRELLO IN BASE ALLE PROPRIETA IN jsonfield1
                string valore1 = eCommerceDM.Selezionadajson(c.jsonfield1, "Caratteristica1", Lingua);
                string valore2 = eCommerceDM.Selezionadajson(c.jsonfield1, "Caratteristica2", Lingua);
                if (!string.IsNullOrEmpty(valore1) || !string.IsNullOrEmpty(valore2))
                {
                    sb.Append(" <p class=\"product-categories muted\">");
                    valore1 = references.TestoCaratteristica(0, valore1, Lingua);
                    if (!string.IsNullOrEmpty(valore1))
                        sb.Append("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica1") + ": " + "</b>" + valore1 + "<br/>");
                    valore2 = references.TestoCaratteristica(1, valore2, Lingua);
                    if (!string.IsNullOrEmpty(valore2))
                        sb.Append("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica2") + ": " + "</b>" + valore2);
                    sb.Append(" </p>");
                }

                if (c.Datastart != null && c.Dataend != null)
                {
                    sb.Append(" <p class=\"product-categories muted\">");
                    sb.Append(references.ResMan("Common", Lingua, "formtestoperiododa") + ": " + "</b>" + string.Format("{0:dd/MM/yyyy}", c.Datastart) + "<br/>");
                    sb.Append(references.ResMan("Common", Lingua, "formtestoperiodoa") + ": " + "</b>" + string.Format("{0:dd/MM/yyyy}", c.Dataend));
                    sb.Append(" </p>");
                }
                string valore3 = eCommerceDM.Selezionadajson(c.jsonfield1, "adulti", Lingua);
                string valore4 = eCommerceDM.Selezionadajson(c.jsonfield1, "bambini", Lingua);
                if (!string.IsNullOrEmpty(valore3) || !string.IsNullOrEmpty(valore4))
                {
                    sb.Append(" <p class=\"product-categories muted\">");
                    if (!string.IsNullOrEmpty(valore3))
                        sb.Append("<br/>" + "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "adulti") + ": " + "</b>" + valore3);
                    if (!string.IsNullOrEmpty(valore3))
                        sb.Append(" " + "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "bambini") + ": " + "</b>" + valore4 + "<br/>");
                    sb.Append(" </p>");
                }

                //sb.Append(" <div class=\"product-categories muted\">");
                //sb.Append(TestoSezione(c.Offerta.CodiceTipologia));
                //sb.Append(" </div>");

                sb.Append(" <p class=\"product-categories muted product-price\" ");
                if (!perstampa)
                    sb.Append(" style =\"font-size:1rem; color:#fff; padding:8px; text-align:center;\"");
                sb.Append(">");
                sb.Append(c.Numero + "&times;" + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { c.Prezzo }) + " €");
                sb.Append(" </p>");



                sb.Append(" </div>");
                sb.Append(" </li>");

            }
        }
        else
        {
            //Serializzo e ritorno tutto il carrello
            sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(carrello, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            }));

        }



        return sb.ToString();
    }

    public static string AggiornaProdottoCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, long idprodotto, long quantita, string username, string idcombinato = "", long idcarrello = 0, long idcliente = 0, double prezzo = 0, DateTime? datastart = null, DateTime? dataend = null, string jsonfield1 = "", bool forceidcarrello = false)
    {
        string ret = "";
        string sessionid = "";
        string trueIP = "";
        if (quantita < 1) quantita = 0;

        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);

        //Carico l'elemento del carrello e lo aggiorno nel database con le modifiche di numero
        Carrello Item = null;
        CarrelloCollection ColItem = new CarrelloCollection();
        eCommerceDM ecom = new eCommerceDM();

        bool abilitacarimento = false;
        if (idprodotto != 0 || (idcarrello != 0)) abilitacarimento = true;
        //else if (idcarrello != 0) abilitacarimento = true;
        if (abilitacarimento)
        {
            Item = new Carrello();
            offerteDM offDM = new offerteDM();

            //Carico la riga prodotto dal carrello in base all'idcarrello oppure per la combinazione idprodotto/idcombinato  ( oppure in base all'idcarrello se passato!)
            if (!forceidcarrello)
                ColItem = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP, idprodotto, idcombinato, idcarrello);//Carico per prodotto/combinato o per idcarrello
            else
                ColItem = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, null, null, 0, "", idcarrello); //forzo di caricare il carrello solo per idcarrello e non per prodotto in modo da consentire righe carrello multiple per l stesso prodotto!!!!

            if (ColItem != null && ColItem.Count > 0)
            {
                Item = ColItem[0]; //SUPPONENDO CHE PER UN CERTO idprodotto/sdcombinato ci sia solo un elemento a carrello

                //se la richiesta di caricamento del rigo era per idcarrello   -> aggiorno l'idprodotto
                idprodotto = Item.id_prodotto;
                //se la richiesta di caricamento del rigo era per idcarrello   -> aggiorno il selettore per le caratteristiche
                idcombinato = Item.Campo2;
            }

            //Carichiamo il prodotto dal database per verificare l'esistenza e l'eventuale disponibilità
            Offerte off = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idprodotto.ToString());
            //bool prodottoeliminato = false;
            if (off == null || off.Id == 0)
            {
                if (Item != null && Item.id_prodotto != 0)
                    ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item.ID);
                //prodottoeliminato = true;
                quantita = 0;
                return ret;
            }

            //controlli sulla disponibilità articolo ( disponibilitò generale )
            if (off.Qta_vendita != null)
            {
                if (quantita > off.Qta_vendita)
                {
                    Session.Add("superamentoquantita", (long)off.Qta_vendita);
                    quantita = (long)off.Qta_vendita;
                }
                else Session.Remove("superamentoquantita");
                if (off.Qta_vendita == 0) // se il prodotto non è più disponibile lo elimino dal carrello
                {
                    if (Item != null && Item.id_prodotto != 0)
                        ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item.ID);
                    //prodottoeliminato = true;
                    quantita = 0;
                    return ret;
                }
            }
            else if (!string.IsNullOrEmpty(off.Xmlvalue))  //andiamo a controllare la disponibilità se presenti limiti per  caratteristiche 
            {
                List<ModelCarCombinate> listprod = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(off.Xmlvalue);
                bool exist = false;
                if (!string.IsNullOrEmpty(idcombinato))
                {
                    Session.Remove("selezionacaratteristiche"); //testocarrelloselcar
                    Session.Add("nontrovata", 0);  //testocarellononesistente
                    foreach (ModelCarCombinate item in listprod)
                    {
                        if (item.id == idcombinato)
                        {
                            Session.Remove("nontrovata");
                            //Qui ho trovato la combinazione che mi serve
                            exist = true;
                            long qta = 0;
                            long.TryParse(item.qta, out qta);
                            //devo verificare se c'è quella disponibilita in base alle caratteristiche selezionate
                            if (quantita > qta)
                            {
                                Session.Add("superamentoquantita", (long)qta);
                                quantita = qta;
                            }
                            else Session.Remove("superamentoquantita");
                            if (qta == 0) // se il prodotto non è più disponibile lo elimino dal carrello
                            {
                                if (Item != null && Item.id_prodotto != 0)
                                    ecom.DeleteCarrelloPerIDCodCarr(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item.ID, Item.Campo2);
                                //prodottoeliminato = true;
                                quantita = 0;
                                return ret;
                            }
                            break;
                        }
                    }
                }
                else Session.Add("selezionacaratteristiche", 0);  //testocarrelloselcar

                //controllo se non ho trovato elementi col filtro caratteristiche indicato
                if (!exist)
                {
                    //non ho l'elemento nella lista diponibili, stampo a video che quella combinazione non è disponibile
                    // o non esiste
                    //e lo elimino dal carrello
                    if (Item != null && Item.id_prodotto != 0)
                        ecom.DeleteCarrelloPerIDCodCarr(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item.ID, Item.Campo2);
                    //  prodottoeliminato = true;
                    Session.Add("superamentoquantita", 0);
                    quantita = 0;
                    return ret;
                }
            }

            double prezzocarrello = 0;
            if (prezzo != 0)
                prezzocarrello = prezzo;
            else
                prezzocarrello = off.Prezzo;

            if (Item == null || Item.ID == 0) //Nuovo elemento da mettere nel carrello
            {
                Item.Data = System.DateTime.Now;
                Item.Prezzo = prezzocarrello;
                //prodotto.Iva = 0;
                //if (quantita == 0) quantita = 1;
                Item.CodiceProdotto = off.CodiceProdotto;
                Item.id_prodotto = off.Id;
                Item.Validita = 1;
                Item.SessionId = sessionid;
                Item.IpClient = trueIP;
                Item.Numero = quantita;
                Item.Campo2 = idcombinato;
                Item.Datastart = datastart;
                Item.Dataend = dataend;
                Item.jsonfield1 = jsonfield1;
                Item.Iva = (long)eCommerceDM.Getivabycodice2liv(off.CodiceCategoria2Liv, references.refivacategorie); //prendo l'iva dalla tabella categorie
            }
            else
            {
                Item.Data = DateTime.Now;
                Item.Prezzo = prezzocarrello;
                Item.CodiceProdotto = off.CodiceProdotto;
                Item.id_prodotto = off.Id;
                Item.Numero = quantita;
                Item.Campo2 = idcombinato;
                if (datastart != null)
                    Item.Datastart = datastart;
                if (dataend != null)
                    Item.Dataend = dataend;
                Item.jsonfield1 = jsonfield1;
                Item.Iva = (long)eCommerceDM.Getivabycodice2liv(off.CodiceCategoria2Liv, references.refivacategorie);//prendo l'iva dalla tabella categorie
            }

            //Memorizzo nel carrello il codice sconto se applicato
            if (Session["codicesconto"] != null)
            {
                Item.Codicesconto = Session["codicesconto"].ToString();
            }

            //Aggiungo l'id anagrafica del cliente ( configurato nel profilo utente ) all'articolo nel carrello
            //In modo da avre l'associazione degli ordini con i clienti

            if (idcliente != 0) //SE passato un idcliente lo memorizzo nel rigo di carrello prodotti
            {
                Item.ID_cliente = idcliente;
            }
            if (!string.IsNullOrEmpty(username))
            {
                long i = 0;
                long.TryParse(getidcliente(username), out i);
                Item.ID_cliente = i;
            }
            if (quantita >= 1)
                ecom.InsertUpdateCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item, false);
            else
                ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Item.ID);

            ret = Item.ID.ToString();
        }
        return ret;
    }

    public static bool SvuotaCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, long id = 0)
    {
        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);
        eCommerceDM ecom = new eCommerceDM();
        CarrelloCollection ColItem = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP);
        List<long> idtodelete = new List<long>();
        foreach (Carrello c in ColItem)
        {
            if (id == 0)
                ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c.ID);
            else if (id == c.ID)
            {
                ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c.ID);
            }
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
        string codicesconto = "";
        TotaliCarrello totali = new TotaliCarrello();
        totali.Supplementospedizione = supplementospedizione;
        totali.Supplementocontrassegno = supplementocontanti;

        List<long> idtodelete = new List<long>();
        long idclienteincarrello = 0;
        foreach (Carrello c in ColItem)
        {
            //////////////////////////////////////////////////////////
            //controlli sulla disponibilità articolo/////////////////// ( QUI VA' MODIFICATO PER LA GESTIONE CON LEGAME A CARATTERISTICHE !!! DA FARE)
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

            //////////////////////////////////////////////////////////
            bool foundzeropeso = false;
            if (c.Offerta.Peso != null && c.Offerta.Peso.Value != 0) //Calcolo il peso totale della merce.. da capire che succede quando hai dei pesi nullo o a zero ....
            {
                totali.TotalePeso += c.Offerta.Peso.Value * (double)c.Numero;
            }
            else foundzeropeso = true;
            //////////////////////////////////////////////////////////


            //totali.TotaleOrdine += c.Numero * (c.Prezzo * (1 + c.Iva/100)); //nel caso di prezzi imponibili
            totali.TotaleOrdine += c.Numero * (c.Prezzo);
            idlist += c.ID.ToString() + ",";
            idclienteincarrello = c.ID_cliente;//Ogni articolo nel carrello ha lo stesso codice id cliente
            codicesconto = c.Codicesconto;
        }
        if (idlist.Length > 1)
        {
            idlist = idlist.Substring(0, idlist.Length - 1);
            idlist = idlist.Insert(0, "( ");
            idlist += " )";
        }
        foreach (long l in idtodelete) //Elimino dal carrello gli elementi non più disponibili
        {
            ecom.DeleteCarrelloPerID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, l);
        }

        //  totali.TotaleSmaltimento = CalcolaTotaliSmaltimento(ColItem);
        totali.Codicesconto = codicesconto;

        //Metto il riferimento all' id cliente commerciale se presente associato al codice sconto
        ClientiDM cDM = new ClientiDM();
        Cliente cli = cDM.CaricaClientePerCodicesconto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicesconto);
        if (cli != null && cli.Id_cliente != 0) totali.Id_commerciale = cli.Id_cliente;

        totali.TotaleSconto = CalcolaSconto(Session, ColItem, totali.TotaleOrdine, cli);
        totali.TotaleSpedizione = CalcolaSpeseSpedizione(ColItem, codicenazione, codiceprovincia, totali);

        totali.Id_cliente = (long)idclienteincarrello;
        //Aggiono i codice della nazione di spedizione nel carrello
        if (!string.IsNullOrWhiteSpace(idlist) && !string.IsNullOrWhiteSpace(codicenazione))
            ecom.UpdateCarrelloPerListaID(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicenazione, idlist);

        //Momorizziamo nei totali se presente un acconto!!!
        double percentualeanticipo = Convert.ToDouble(ConfigManagement.ReadKey("percAnticipoPagamento"));
        //La percentuale di anticipo è 100% se la data di inizio periodo ripetto oggi è inferiore a 60 gg
        if (ColItem.Exists(p => p.Datastart != null && ((TimeSpan)(p.Datastart.Value - DateTime.Now)).Days < 60))
            percentualeanticipo = 100;
        totali.Percacconto = percentualeanticipo;

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
    public static string VisualizzaTotaliCarrello(HttpContext context, string idprodotto = "", string idcombined = "", string idcarrello = "")
    {
        string ret = "";
        string sessionid = "";
        string trueIP = "";

        long lprod = 0;
        long.TryParse(idprodotto, out lprod);
        long lcarr = 0;
        long.TryParse(idcarrello, out lcarr);

        CommonPage.CaricaRiferimentiCarrello(context.Request, context.Session, ref sessionid, ref trueIP);
        WelcomeLibrary.DAL.eCommerceDM ecmDM = new WelcomeLibrary.DAL.eCommerceDM();
        WelcomeLibrary.DOM.CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP, lprod, idcombined, lcarr);
        ret = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { CommonPage.CalcolaTotaleCarrello(context.Request, context.Session, carrello) }) + " €";
        return ret;
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
            //totale += c.Numero * (c.Prezzo * (1 + c.Iva/100)); //nel caso di prezi unitari imponibili
            totale += c.Numero * (c.Prezzo);
        }

        return totale;
    }
    public static string VisualizzaPezziCarrello(HttpContext context, string idprodotto = "", string idcombined = "", string idcarrello = "")
    {
        string ret = "";
        string sessionid = "";
        string trueIP = "";

        long lprod = 0;
        long.TryParse(idprodotto, out lprod);
        long lcarr = 0;
        long.TryParse(idcarrello, out lcarr);

        CommonPage.CaricaRiferimentiCarrello(context.Request, context.Session, ref sessionid, ref trueIP);
        WelcomeLibrary.DAL.eCommerceDM ecmDM = new WelcomeLibrary.DAL.eCommerceDM();
        WelcomeLibrary.DOM.CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP, lprod, idcombined, lcarr);
        ret = CommonPage.CalcolaPezziCarrello(context.Request, context.Session, carrello).ToString();
        return ret;
    }
    public static long CalcolaPezziCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, CarrelloCollection carrello)
    {
        string sessionid = "";
        string trueIP = "";
        CaricaRiferimentiCarrello(Request, Session, ref sessionid, ref trueIP);
        eCommerceDM ecom = new eCommerceDM();
        if (carrello == null || carrello.Count == 0)
            carrello = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP);

        long pezzi = 0;
        foreach (Carrello c in carrello)
        {
            pezzi += c.Numero;
        }
        return pezzi;
    }

    private static double CalcolaSconto(System.Web.SessionState.HttpSessionState Session, CarrelloCollection ColItem, double totalecarrello, Cliente cli = null) // da modificare per gestione sconti commerciali!!!!
    {
        double valoresconto = 0;
        string percentualesconto = ConfigManagement.ReadKey("percentualesconto");
        //Prendo la percentuale da quella in configurazione
        if (Session["codicesconto"] != null && Session["codicesconto"].ToString().ToLower() == ConfigManagement.ReadKey("codicesconto").ToLower())
        {
            double tmp = 0;
            double.TryParse(percentualesconto, out tmp);
            valoresconto = Math.Round(((double)totalecarrello * tmp / 100), 2, MidpointRounding.ToEven);
        }

        //Testo Se presente una percentuale tra quelle associate ai commerciali e nel caso prendo quella (PRIORITA')
        if (Session["codicesconto"] != null && !string.IsNullOrEmpty(Session["codicesconto"].ToString()))
        {
            double percscontocommerciale = 0;
            //Se presente un codice sconto
            string codicesconto = Session["codicesconto"].ToString().ToLower();
            //Promviamo a vedere se il codice sconto ha associato un0anagrafica commerciale
            //Metto il riferimento all' id cliente commerciale se presente associato al codice sconto
            if (cli != null && cli.Id_cliente != 0)
            {
                Dictionary<string, double> dict = ClientiDM.SplitCodiciSconto(cli.Codicisconto);
                if (dict != null && dict.ContainsKey(codicesconto))
                {
                    percscontocommerciale = dict[codicesconto];
                    valoresconto = Math.Round(((double)totalecarrello * percscontocommerciale / 100), 2, MidpointRounding.ToEven);
                }
            }
        }
        return valoresconto;
    }


    public static string Creaeventopurchaseagooglegtag(TotaliCarrello totali, CarrelloCollection prodotti)
    {
        string ret = "";
        try
        {
            if (totali == null || prodotti == null) return string.Empty;
            /////////////////////////////////////////////////////////////////////
            //EVENTO PER GOOGLE GTAG PER L'ACQUISTO 
            /////////////////////////////////////////////////////////////////////
            String scriptRegVariables = "";
            string jsoncarrelloordine = "";
            //https://developers.google.com/analytics/devguides/collection/gtagjs/enhanced-ecommerce
            // qui devo inserire i dati del carrello e dei prodotti e serializzari per gtag
            // da fare con totali e prodotti DOM.jsongtagpurchase DOM.jsongtagitem //.....
            WelcomeLibrary.DOM.jsongtagpurchase jtagpurchaseevent = new jsongtagpurchase();
            WelcomeLibrary.DOM.jsongtagitem purchaseitem = new jsongtagitem();
            jtagpurchaseevent.transaction_id = totali.CodiceOrdine;
            jtagpurchaseevent.affiliation = ConfigManagement.ReadKey("Nome");
            jtagpurchaseevent.value = totali.TotaleOrdine - totali.TotaleSconto;
            jtagpurchaseevent.tax = 0;// qui dovresti scorporare l'iva
            jtagpurchaseevent.shipping = totali.TotaleSpedizione;
            jtagpurchaseevent.items = new List<jsongtagitem>();
            foreach (Carrello c in prodotti)
            {
                purchaseitem = new jsongtagitem();
                purchaseitem.id = c.Offerta.Id.ToString(); //Id scheda prodott ( sarebbe meglio lo sku o ptn // da ricavare dalla descrzione se presente
                //string skuprod = offerteDM.Getvaluebytag("ean:", c.Offerta.DescrizioneI);
                //skuprod = offerteDM.Getvaluebytag("mpn:", c.Offerta.DescrizioneI);
                purchaseitem.name = c.Offerta.DenominazioneI;
                purchaseitem.list_name = "";//nome della lista filtro di ricerca risultati
                string text = offerteDM.Getvaluebytag("brand:", c.Offerta.DescrizioneI);
                if (string.IsNullOrEmpty(text)) text = offerteDM.Getvaluebytag("marchio:", c.Offerta.DescrizioneI);
                purchaseitem.brand = text;
                purchaseitem.category = references.TestoCategoria(c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, "I"); ; //Categoria di catalogo del prodotto
                purchaseitem.variant = ""; //eventuale caratteristica del prodotto
                purchaseitem.price = c.Prezzo;
                purchaseitem.quantity = c.Numero;
                purchaseitem.coupon = c.Codicesconto;
                purchaseitem.list_position = 0;
                jtagpurchaseevent.items.Add(purchaseitem);
            }
            jsoncarrelloordine = Newtonsoft.Json.JsonConvert.SerializeObject(jtagpurchaseevent);

            scriptRegVariables += ";\r\n " + string.Format("gtag('event', 'purchase', {0});console.log('gtag called;');", jsoncarrelloordine);
            scriptRegVariables = WelcomeLibrary.UF.Utility.waitwrappercall("gtag", scriptRegVariables); //wrapper fo waiting
            Dictionary<string, string> addelements = new Dictionary<string, string>();
            addelements.Add("jsvarfrommasterstart", scriptRegVariables);
            ret = custombind.CreaInitStringJavascriptOnly(addelements);
            /////////////////////////////////////////////////////////////////////
        }
        catch { }
        return ret;
    }

    private static double CalcolaSpeseSpedizione(CarrelloCollection ColItem, string codicenazione, string codiceprovincia, TotaliCarrello totali)
    {
        double spesespedizione = 0;
        totali.Bloccaacquisto = false;
        ////////////////////////////////////////////
        //CALCOLO COSTI SPEDIZIONE A PESO 
        //(  impostare il json per i pesi nella colonna apposita in base al smaple json tbl_nazioni per le nazioni di intersse, bloccare paypal dove indicato nel json nell'apposito campo. )
        ////////////////////////////////////////////
        double? costoperpeso = 0;
        costoperpeso = CalcolaSpedizioneSuPeso(totali, codicenazione);
        if (costoperpeso != null)
        {

            if (costoperpeso != 999999)
                spesespedizione = costoperpeso.Value;
            else
            {
                //devo bloccare paypal e non far fare l'acquisto!!!! con carta su indicazione della verifica sistema pesi
                totali.Bloccaacquisto = true; //blocoo l'acquisto con carta !!!
            }
        }
        else
        {
            double costofinalespedizione = 0;
            double totaleordine = totali.TotaleOrdine;
            double totalesconto = totali.TotaleSconto;
            ////////////////////////////////////////////////////////////
            // CALCOLO COSTI SPEDIZIONE metodo classico BASE PER NAZIONE
            ///////////////////////////////////////////////////////////
            double costodefaultitalia = 0;
            double.TryParse(ConfigManagement.ReadKey("costobasespedizioni"), out costodefaultitalia);
            double costodefaultestero = 0;
            double.TryParse(ConfigManagement.ReadKey("defaultesterospedizione"), out costodefaultestero);
            //Modificare per caricare il campo per gli scaglioni di peso dalla tabella nazioni per il conteggio 
            double costospedizionenazione = references.TrovaCostoNazione(codicenazione); //Costo spedizione per nazione
            if (costospedizionenazione == 0) totali.Bloccaacquisto = true; //blocoo l'acquisto con carta !!! ( caso costo non specificato in tabella nazioni )

            //SE NON PRESENTE COSTO SPEDIZIONE IN TABELLA NAZIONI PRENDO IL DEFAULT
            if (codicenazione.ToLower() == "it" && costospedizionenazione == 0) costospedizionenazione = costodefaultitalia; //Costostandard per l'estero
            if (codicenazione.ToLower() != "it" && costospedizionenazione == 0) costospedizionenazione = costodefaultestero; //Costostandard per l'estero
            costofinalespedizione = costospedizionenazione;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            ///// impostazione spese spedizione con controllo SOGLIE Azzeramento METODO CLASSICO ( NO PESO )
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            double sogliaitalia = 0;
            double.TryParse(ConfigManagement.ReadKey("sogliaSpedizioni"), out sogliaitalia);
            double sogliaestero = 0;
            double.TryParse(ConfigManagement.ReadKey("sogliaspedizioniestero"), out sogliaestero);
            switch (codicenazione)
            {
                case "IT":
                    if (totaleordine - totalesconto < sogliaitalia)
                    {
                        spesespedizione += costofinalespedizione;// Convert.ToDouble(ConfigManagement.ReadKey("costobaseSpedizioni"));
                    }
                    break;
                default:
                    if (totaleordine - totalesconto < sogliaestero)
                    {
                        spesespedizione += costofinalespedizione;
                    }
                    break;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        //Da calcolare in base ai parametri passati EVENTUALI altri aspetti delle spedizioni
        //long totalearticoli = 0;
        //foreach (Carrello c in ColItem)
        //{
        //    totalearticoli += c.Numero;
        //}

        //SUPPLEMENTI OBBLIGATORI FISSI ( CON SPUNTE )
        double tmpconv = 0;
        double.TryParse(ConfigManagement.ReadKey("supplementoSpedizioni"), out tmpconv);
        if (totali.Supplementospedizione) //Supplemento isole supplementoSpedizioni
            spesespedizione += tmpconv;
        tmpconv = 0;
        double.TryParse(ConfigManagement.ReadKey("supplementoContrassegno"), out tmpconv);
        if (totali.Supplementocontrassegno) //Supplemento contrassegno  
            spesespedizione += tmpconv;

        return spesespedizione;
    }

    /// <summary>
    /// Se torna null -> devo usare il metodo classico di calcolo delle spedizioni, altrimenti devi prendere il valore del costo ritornato
    /// (considera già il peso massimo spedibile, eventuale azzeramento dei costi per nazione, eventuali supplementi )
    /// </summary>
    /// <param name="peso"></param>
    /// <param name="codicenazione"></param>
    /// <returns></returns>
    private static double? CalcolaSpedizioneSuPeso(TotaliCarrello totali, string codicenazione)
    {
        double? ret = null;
        jsonspedizioni js = references.TrovaFascespedizioneNazione(codicenazione);
        if (js != null && js.fascespedizioni != null)
        {
            fascespedizioni fascia = js.fascespedizioni.Find(e => e.PesoMin <= totali.TotalePeso && e.PesoMax > totali.TotalePeso);
            if (fascia != null) ret = fascia.Costo;

            // logiche aggiuntive di calcolo
            //verifica soglie azzeramenti e superamento di peso
            // se diversa da zero e l'imponibile supera questo importo devo azzerare il costo di spedizione totali.  totali.TotaleOrdine -  totali.TotaleSconto
            double sogliazz = 0;
            if (js.keyValuePairs.ContainsKey("sogliaazzeramento"))
                double.TryParse(js.keyValuePairs["sogliaazzeramento"].ToString(), out sogliazz);
            if (totali.TotaleOrdine - totali.TotaleSconto >= sogliazz && sogliazz != 0) ret = 0;
            // da aggiungere alle spese di spedizione ( ret se presente )
            double daziaggiunta = 0;
            if (js.keyValuePairs.ContainsKey("supplementosp"))
                double.TryParse(js.keyValuePairs["supplementosp"].ToString(), out daziaggiunta);
            ret += daziaggiunta;
            // controllo con valore totali.TotalePeso
            double pesomassimo = 0;
            if (js.keyValuePairs.ContainsKey("sogliapeso"))
                double.TryParse(js.keyValuePairs["sogliapeso"].ToString(), out pesomassimo);
            if (pesomassimo != 0 && totali.TotalePeso >= pesomassimo)
                ret = 999999; // comunicare di bloccare paypal e ordine

        }
        return ret;
    }

    public static string CaricaQuantitaNelCarrello(HttpRequest Request, System.Web.SessionState.HttpSessionState Session, string idprodotto, string codcar, string idcarrello = "")
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

            long id_prodotto = 0;
            long.TryParse(idprodotto.ToString(), out id_prodotto);

            long lidcarrello = 0;
            long.TryParse(idcarrello.ToString(), out lidcarrello);

            ColItem = ecom.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP, id_prodotto, codcar, lidcarrello);
            if (ColItem != null && ColItem.Count > 0)
                Item = ColItem[0];
        }

        if (Item != null && Item.id_prodotto != 0)
            ret = Item.Numero.ToString();
        return ret;
    }
    public static TotaliCarrelloCollection CaricaDatiOrdini(string id_cliente = "", string codiceordine = "", string datamin = "", string datamax = "", string idcommerciale = "", long page = 1, long pagesize = 0)
    {
        eCommerceDM eDM = new eCommerceDM();
        TotaliCarrelloCollection ordini = new TotaliCarrelloCollection();
        List<SQLiteParameter> parcoll = new List<SQLiteParameter>();
        if (!string.IsNullOrWhiteSpace(id_cliente))
        {
            SQLiteParameter parid = new SQLiteParameter("@Id_cliente", id_cliente);
            parcoll.Add(parid);
        }
        if (!string.IsNullOrWhiteSpace(idcommerciale))
        {
            SQLiteParameter parid1 = new SQLiteParameter("@Id_commerciale", idcommerciale);
            parcoll.Add(parid1);
        }
        if (!string.IsNullOrEmpty(codiceordine))
        {
            SQLiteParameter parcod = new SQLiteParameter("@Codiceordine", codiceordine);
            parcoll.Add(parcod);
        }
        if (!string.IsNullOrEmpty(datamin))
        {
            DateTime _dt;
            //if (DateTime.TryParse(datamin, out _dt))
            if (DateTime.TryParseExact(datamin, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _dt))
            {
                SQLiteParameter pardmin = new SQLiteParameter("@DataMin", dbDataAccess.CorrectDatenow(_dt));
                parcoll.Add(pardmin);
            }
        }
        if (!string.IsNullOrEmpty(datamax))
        {
            DateTime _dt;
            //if (DateTime.TryParse(datamax, out _dt))
            if (DateTime.TryParseExact(datamax, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _dt))
            {
                SQLiteParameter pardmax = new SQLiteParameter("@DataMax", dbDataAccess.CorrectDatenow(_dt));
                parcoll.Add(pardmax);
            }
        }

        ordini = eDM.CaricaListaOrdini(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parcoll, "", false, page, pagesize);

        return ordini;
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
                                                                      //if (DateTime.TryParse(item.Campo1, out _Data))
            if (DateTime.TryParseExact(item.Campo1, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _Data))
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
                //DateTime.TryParse(values["pubDate"], out t);
                DateTime.TryParseExact(values["pubDate"], "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out t);
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
            long id_origine = 0;
            string indirizzocontenutocompleto = values["guid"];

            //Comunicati
            if (values.ContainsKey("guid"))
            {
                id_origine_str = values["guid"];
                id_origine_str = id_origine_str.Trim().TrimEnd('/');
                int lastpos = id_origine_str.LastIndexOf('/');
                if (lastpos != -1)
                    id_origine_str = id_origine_str.Substring(lastpos + 1);
                long.TryParse(id_origine_str, out id_origine);
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
                    long.TryParse(id_origine_str, out id_origine);
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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter ptipologia = new SQLiteParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);
            parColl.Add(ptipologia);
            if (id_origine != 0)
            {
                SQLiteParameter pid_origine = new SQLiteParameter("@Id_collegato", id_origine);
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
                    filemanage.CaricaFile(server, urlfile, filename, item.CodiceTipologia, item.Id.ToString());
                    //Inserisco i file che devo in allegato
                }
                catch (Exception err)
                {

                }
            }
        }
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
            long _id = 0;
            long.TryParse(kv.Key, out _id);
            c.Id = _id; //assegno l'identificativo progressivo del contenuto preso dal web   
#endif

            DateTime t = DateTime.MinValue;
            if (values.ContainsKey("DataInserimento"))
                //DateTime.TryParse(values["DataInserimento"], out t);
                DateTime.TryParseExact(values["DataInserimento"], "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out t);
            c.DataInserimento = t;

            if (values.ContainsKey("DescrizioneGB"))
                c.DescrizioneGB = values["DescrizioneGB"];
            if (values.ContainsKey("DescrizioneI"))
                c.DescrizioneI = values["DescrizioneI"];
            if (values.ContainsKey("DescrizioneRU"))
                c.DescrizioneI = values["DescrizioneRU"];
            if (values.ContainsKey("DescrizioneFR"))
                c.DescrizioneI = values["DescrizioneFR"];

            if (values.ContainsKey("DatitecniciGB"))
                c.DescrizioneGB += values["DatitecniciGB"];
            if (values.ContainsKey("DatitecniciI"))
                c.DescrizioneI += values["DatitecniciI"];
            if (values.ContainsKey("DatitecniciRU"))
                c.DescrizioneI += values["DatitecnicRU"];
            if (values.ContainsKey("DatitecniciFR"))
                c.DescrizioneI += values["DatitecnicFR"];

            if (values.ContainsKey("DenominazioneGB"))
                c.TitoloGB = values["DenominazioneGB"];
            if (values.ContainsKey("DenominazioneI"))
                c.TitoloI = values["DenominazioneI"];
            if (values.ContainsKey("DenominazioneRU"))
                c.TitoloRU = values["DenominazioneRU"];
            if (values.ContainsKey("DenominazioneFR"))
                c.TitoloFR = values["DenominazioneFR"];
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
                            c.FotoCollection_M[i].DescrizioneI = descrizione;
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


