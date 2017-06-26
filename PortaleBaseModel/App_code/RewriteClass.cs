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
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
        string Pathdestinazione = "~/Index.aspx";
        string textmatch = requestContext.RouteData.Values["textmatch"] as string;
        string destinationselector = requestContext.RouteData.Values["destinationselector"] as string;
        string Lingua = requestContext.RouteData.Values["Lingua"] as string;
        if (Lingua == null) HttpContext.Current.Items["Lingua"] = ConfigManagement.ReadKey("deflanguage");
        switch (Lingua.ToUpper())
        {
            case "I":
            case "GB":
            case "RU":
                break;
            default:
                HttpContext.Current.Items["Lingua"] = ConfigManagement.ReadKey("deflanguage");
               Pathdestinazione = "~/Error.aspx";
                return new RedirectHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione));
                //return new ErrorHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione));
                break;
        }


        if (destinationselector == null) destinationselector = "";
        if (textmatch == null || textmatch.ToLower() == "home") return BuildManager.CreateInstanceFromVirtualPath(Pathdestinazione, typeof(Page)) as Page;

        //Carichiamo la destinazione ed i paramentri in base al testmatch ....
        string calledurl = textmatch;
        if (!string.IsNullOrEmpty(destinationselector))
            calledurl = calledurl.Insert(0, destinationselector + "/");
        calledurl = Lingua + "/" + calledurl;

        Tabrif itemurl = WelcomeLibrary.UF.SitemapManager.GetUrlRewriteaddress(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, calledurl);
        if (itemurl != null)
        {
            Pathdestinazione = itemurl.Campo1;
            Dictionary<string, string> keyvalues = WelcomeLibrary.UF.SitemapManager.SplitParameters(itemurl.Campo2);
            switch (destinationselector)
            {

                default:
                    foreach (KeyValuePair<string, string> kv in keyvalues)
                    {
                        if (kv.Key != Lingua)
                            HttpContext.Current.Items[kv.Key] = kv.Value;
                    }

                    break;
            }
        }
        //else
        //{
        //    Pathdestinazione = "~/Error.aspx";
        //    return new RedirectHandler(CommonPage.ReplaceAbsoluteLinks(Pathdestinazione));
        //}

        return BuildManager.CreateInstanceFromVirtualPath(Pathdestinazione, typeof(Page)) as Page;
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
        httpContext.Response.Status = "404 Page not found";
        httpContext.Response.StatusCode = 404;
        httpContext.Response.AppendHeader("Location", newUrl);
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
