using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;
using System.Web.UI;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Configuration;
using WelcomeLibrary.UF;

/// <summary>
/// Descrizione di riepilogo per RouteHandler
/// </summary>
public class GenericRouteHandler : IRouteHandler
{
    public GenericRouteHandler()
    {
        //
        // TODO: aggiungere qui la logica del costruttore
        //
    }
    private string Testredirect(string originalurl, bool usestatic = false)
    {
        string urltoredirect = "";
        //if (ConfigurationManager.AppSettings["Redirect"].ToString() == "true")
        //{
        if (urltoredirect != null)
        {
            // originalurl = originalurl.Replace(pathassoluto + "/", "");//Tolgo il dominio per la ricerca
            //urltoredirect = SitemapManager.TestRedirect(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, originalurl, usestatic);
            urltoredirect = SitemapManager.TestRedirect(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, originalurl);
        }
        //}
        return urltoredirect;
    }
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
        //Creo una variabile per la scrittura dei messaggi nel file di log
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        Messaggi.Add("Messaggio", "");
        string Pathdestinazione = "~/index.aspx";
        string error = "";
        try
        {

            string textmatch = requestContext.RouteData.Values["textmatch"] as string;
            string destinationselector = requestContext.RouteData.Values["destinationselector"] as string;
            string Lingua = requestContext.RouteData.Values["Lingua"] as string;

            if (Lingua == null)
            {
                HttpContext.Current.Items["Lingua"] = ConfigManagement.ReadKey("deflanguage");
                Lingua = ConfigManagement.ReadKey("deflanguage");
            }
            // Messaggi["Messaggio"] += "1";
            //HttpContext.Current.Response.Write

            /*REWRITING OLD URL **************************************************************************/
            //Query in tbl redirect
            //Se ho un match in tabella routing -> devo prendere il nuovo url da tabella e fare
            //Pathdestinazione = NUOVO URL DA TABELLA REDIRECT ; e da li fare redirect in base all'url
            string originalrequesturl = requestContext.HttpContext.Request.Path;
            string urltoredir = "";
            urltoredir = Testredirect(originalrequesturl);
            // Messaggi["Messaggio"] += "2";


            if (!string.IsNullOrEmpty(urltoredir))
            {
                return new RedirectHandler(CommonPage.ReplaceAbsoluteLinks(urltoredir));
            }
#if false
        string originalrequesturl = requestContext.HttpContext.Request.Path;
        string urltoredir = "";
        if (originalrequesturl.ToLower().Contains("Casadellabatteria/".ToLower()) || originalrequesturl.ToLower().Contains("catalogo-prodotti/".ToLower()))
            urltoredir = Testredirect(originalrequesturl);
        if (!string.IsNullOrEmpty(urltoredir))
        {
            return new RedirectHandler(CommonPage.ReplaceAbsoluteLinks(urltoredir));
        }
        if (originalrequesturl.ToLower().Contains("Casadellabatteria/Scheda-Prodotto/".ToLower()) ||
            originalrequesturl.ToLower().Contains("Casadellabatteria/Eventi/".ToLower()))
        {
            string idoldcontent = requestContext.RouteData.Values["idContenuto"] as string;
            if (idoldcontent != null)
            {
                offerteDM offDM = new offerteDM();
                Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idoldcontent);
                if (item != null)
                {
                    string linkcanonico = CommonPage.CreaLinkRoutes(null, false, Lingua, (CommonPage.CleanUrl(item.UrltextforlinkbyLingua(Lingua))), item.Id.ToString(), item.CodiceTipologia);
                    return new RedirectHandler(CommonPage.ReplaceAbsoluteLinks(linkcanonico));
                }
            }
        } 
#endif
            /*END REWRITING OLD URL **********************************************************************/
            string culturename = SitemapManager.getCulturenamefromlingua(Lingua); // abilitare per modifica codici culture lingua 19.12.18
            //  Messaggi["Messaggio"] += "3";

            // il viceveresa è SitemapManager.getLinguafromculture(ret);
            switch (culturename.ToLower())
            {
                case "i":
                case "gb":
                case "ru":
                case "fr":
                case "it":
                case "en":
                case "de":
                case "es":
                    break;
                default:
                    HttpContext.Current.Items["Lingua"] = ConfigManagement.ReadKey("deflanguage");
                    Pathdestinazione = "~/404.aspx";
                    //return new RedirectHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione));
                    return new ErrorHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione));
                    break;
            }
            //   Messaggi["Messaggio"] += "4";


            if (destinationselector == null) destinationselector = "";
            if (string.IsNullOrEmpty(textmatch) || textmatch.ToLower() == "home") return BuildManager.CreateInstanceFromVirtualPath(Pathdestinazione, typeof(Page)) as Page;

            //Costruiamo il path di destinazione in base ai segments
            string calledurl = WelcomeLibrary.UF.SitemapManager.CostruisciRewritedUrl(culturename, destinationselector, textmatch); //modifica codici culture lingua 19.12.18
            //Tabrif itemurl = WelcomeLibrary.UF.SitemapManager.GetUrlRewriteaddress(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, calledurl, true);
            Tabrif itemurl = WelcomeLibrary.UF.SitemapManager.GetUrlRewriteaddress(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, calledurl);
            if (itemurl != null)
            {
                //   Messaggi["Messaggio"] += "5a";

                Pathdestinazione = itemurl.Campo1;
                Dictionary<string, string> keyvalues = WelcomeLibrary.UF.SitemapManager.SplitParameters(itemurl.Campo2);
                switch (destinationselector)
                {

                    default:
                        foreach (KeyValuePair<string, string> kv in keyvalues)
                        {
                            //if (kv.Key.ToLower() != "lingua")
                            HttpContext.Current.Items[kv.Key] = kv.Value;
                        }

                        break;
                }
            }
            else
            {
                // Messaggi["Messaggio"] += "5b";
                Pathdestinazione = "~/404.aspx";
                //return new RedirectHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione));
                return new ErrorHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione)); //non va bene apre la pagina di default 404 di iis
            }
            //    WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);

            // Page renderedpage = new Page()
            Page renderedpage = BuildManager.CreateInstanceFromVirtualPath(Pathdestinazione, typeof(Page)) as Page;
            return renderedpage;


        }
        catch (Exception err)
        {
            error = err.Message;
            Messaggi["Messaggio"] += " Errore rewriteclass : " + err.Message + " " + System.DateTime.Now.ToString();
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
            //return new RedirectHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione));
            Pathdestinazione = "~/404.aspx";
            return new ErrorHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione));
        }
        finally
        {
            //if (!string.IsNullOrEmpty(error))
            //{
            //    Messaggi["Messaggio"] += "finally emptypath";
            //    WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);

            //}
        }
    }
}

/// <summary>
/// <para>Error page MVC handler</para>
/// </summary>
public class ErrorHandler : IHttpHandler
{
    private string newUrl;

    public ErrorHandler(string newUrl)
    {
        this.newUrl = newUrl;
    }

    public bool IsReusable
    {
        get { return true; }
    }

    public void ProcessRequest(HttpContext httpContext)
    {
        //https://stackoverflow.com/questions/10116804/how-to-return-own-404-custom-page?msclkid=74689877b25611ec89d3b0d189872bf1
        //httpContext.Server.ClearError();
        //httpContext.Response.TrySkipIisCustomErrors = true;
        httpContext.Response.Status = "404 Page not found";
        httpContext.Response.StatusCode = 404;
        //httpContext.Response.AppendHeader("Location", newUrl); //il path destinazione lo prende dal web.config httpErrors integrated pipeline ( non funziona qui )
        httpContext.Response.End();
        return;
    }
}


/// <summary>
/// <para>Redirecting MVC handler</para>
/// </summary>
public class RedirectHandler : IHttpHandler
{
    private string newUrl;

    public RedirectHandler(string newUrl)
    {
        this.newUrl = newUrl;
    }

    public bool IsReusable
    {
        get { return true; }
    }

    public void ProcessRequest(HttpContext httpContext)
    {
        httpContext.Response.Status = "301 Moved Permanently";
        httpContext.Response.StatusCode = 301;
        httpContext.Response.AppendHeader("Location", newUrl);
        return;
    }
}
