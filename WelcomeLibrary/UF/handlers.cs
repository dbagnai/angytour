using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;

namespace WelcomeLibrary.UF
{

    public class RewritingHandlerGeneric : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context,
                     string requestType,
                     string url,
                     string pathTranslated)
        {
            //context.Items["fileName"] = Path.GetFileNameWithoutExtension(url);
            ////METTO NEL CONTEXT LE COPPIE CHIAVE/VALORE PRESENTI NELLA eventuale QUERYSTRING
            //foreach (string key in context.Request.QueryString.AllKeys)
            //{
            //    context.Items[key] = context.Request.QueryString[key];
            //}

            IHttpHandler handler = null;
            //PREIMPOSTO L'HANDLER SULLA CHIAMATE ALLE PAGINE SENZA REWRITING (Serve Quando viene chiamata direttamente la pagina originaria)
            try
            {
                handler = PageParser.GetCompiledPageInstance(url,
                     pathTranslated,
                  context);
            }
            catch
            { }

            if (url.ToLower().Contains("index.aspx")) return handler;

            //----------------------------------------------------------------------------
            //IMPOSTO L'HANDLER PER EVENTUALI CHIAMATE CON REWRITING
            //QUI PUOI IMPOSTARE GLI INDIRIZZI ALTENATIVI PER LE PAGINE
            //IN CASO DI PAGINE CON QUERYSTRING DEVI REINSERIRE I VALORI
            //NEL CONTEXT E RICARICARLI DA QUESTO NELLA PAGINA
            //NBB. (Attenzione i nomi delle pagine e percorsi per il rewriting devono essere scritti qui tutti minuscoli!!!!!!!!!)
            //----------------------------------------------------------------------------
            string urlSenzaBase = "";
            string origUrlPage = "";
            string errorMapping = "";
            string urlpathreale = "";
            string param1 = "";
            string param2 = "";
            string param3 = "";
            string param4 = "";
            string param5 = "";
            string param6 = "";
            string param7 = "";
            string param8 = "";
            try
            {

                if (url.IndexOf(context.Request.ApplicationPath) != -1 && (url.IndexOf(".aspx") != -1))
                {
                    urlSenzaBase = url.Remove(0, context.Request.ApplicationPath.Length);
                    origUrlPage = urlSenzaBase;
                    //MAPPING DI CHIAMATE ALLE LISTE FILTRATE DEI PRODOTTI PER CATEGORIA (REWRITING CATALOGO LISTE PRODOTTI)
                    if (urlSenzaBase.ToLower().Contains("cottofattoamano/")) //LE CHIAMATE /articoli/tipologia-lingua-testoperindicizzazione.aspx sono mappate ai risutati ricerca prodotti
                    {
                        //http://localhost:49893/PortaleBaseModel/AspNetPages/RisultatiOfferte.aspx?Tipologia=rif000001&Categoria=prod000003&Categoria2liv=sprod000010&Pagina=&Lingua=I
                        //http://localhost:49893/PortaleBaseModel/catalogo/rif000001_I_prod000003_sprod000010_testo-per-indice.aspx
                        string consume = urlSenzaBase.ToLower().Substring(urlSenzaBase.IndexOf("cottofattoamano/") + 16);
                        param1 = consume.Substring(0, consume.IndexOf("_")); //tipologia
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        param2 = consume.Substring(0, consume.IndexOf("_")).ToUpper(); //lingua
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        param4 = consume.Substring(0, consume.IndexOf("_")); //categoria
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        param5 = consume.Substring(0, consume.IndexOf("_")); //categoria1liv
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        urlSenzaBase = "linkcatalogolista";
                        if (consume.IndexOf("_") != -1)//contiene anche l'id articolo -> invece della lista torno la scheda
                        {
                            urlSenzaBase = "linkcatalogoscheda";
                            param3 = consume.Substring(0, consume.IndexOf("_")).ToUpper(); //idarticolo
                        }
                        urlpathreale = url.ToLower().Substring(0, url.LastIndexOf("/") + 1).Replace("cottofattoamano/", "AspNetPages/");
                    }
                    //MAPPING DI CHIAMATE ALLE LISTE FILTRATE DEI PRODOTTI PER CATEGORIA (REWRITING CATALOGO LISTE PRODOTTI)
                    if (urlSenzaBase.ToLower().Contains("blog/")) //LE CHIAMATE /articoli/tipologia-lingua-testoperindicizzazione.aspx sono mappate ai risutati ricerca prodotti
                    {
                        //http://localhost:49893/PortaleBaseModel/AspNetPages/RisultatiRicerca.aspx?Tipologia=rif000001&Categoria=prod000003&Categoria2liv=sprod000010&Pagina=&Lingua=I
                        //http://localhost:49893/PortaleBaseModel/catalogo/rif000001_I_prod000003_sprod000010_testo-per-indice.aspx
                        string consume = urlSenzaBase.ToLower().Substring(urlSenzaBase.IndexOf("blog/") + 5);
                        param1 = consume.Substring(0, consume.IndexOf("_")); //tipologia
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        param2 = consume.Substring(0, consume.IndexOf("_")).ToUpper(); //lingua
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        param4 = consume.Substring(0, consume.IndexOf("_")); //categoria
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        param5 = consume.Substring(0, consume.IndexOf("_")); //categoria1liv
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        urlSenzaBase = "linkarticolilista";
                        if (consume.IndexOf("_") != -1)//contiene anche l'id articolo -> invece della lista torno la scheda
                        {
                            urlSenzaBase = "linkarticolischeda";
                            param3 = consume.Substring(0, consume.IndexOf("_")).ToUpper(); //idarticolo
                        }
                        urlpathreale = url.ToLower().Substring(0, url.LastIndexOf("/") + 1).Replace("blog/", "AspNetPages/");
                    }
                    //MAPPING DI CHIAMATE CONTENUTI TESTO STATICO (REWRITING PAGINE CONTENUTO STATICO)
                    if (urlSenzaBase.Contains("pavimentiincotto/")) //LE CHIAMATE /informazioni/contenutopagina-lingua-testoperindicizzazione.aspx sono mappate alle schede prodotto
                    {
                        string consume = urlSenzaBase.ToLower().Substring(urlSenzaBase.IndexOf("pavimentiincotto/") + 17);
                        param1 = consume.Substring(0, consume.IndexOf("_")); //contenutopagina
                        consume = consume.Substring(consume.IndexOf("_") + 1);
                        param3 = consume.Substring(0, consume.IndexOf("_")).ToUpper(); //lingua
                        urlSenzaBase = "linkinformazioni";
                        urlpathreale = url.ToLower().Substring(0, url.LastIndexOf("/") + 1).Replace("pavimentiincotto/", "AspNetPages/");
                    }
       
                    //ESEGUIAMO LA RISCRITTURA DELLE CHIAMATE
                    string desturl = "";
                    //string nomepagina = Path.GetFileNameWithoutExtension(url).ToLower();
                    switch (urlSenzaBase)
                    {
                        case "linkcatalogolista":
                            handler = PageParser.GetCompiledPageInstance(urlpathreale + "RisultatiProdotti.aspx",
                            context.Server.MapPath("~/AspNetPages/RisultatiProdotti.aspx"),
                         context);
                            context.Items["Tipologia"] = param1;
                            context.Items["Lingua"] = param2;
                            context.Items["Categoria"] = param4;
                            context.Items["Categoria2liv"] = param5;
                            break;
                        case "linkcatalogoscheda":
                            handler = PageParser.GetCompiledPageInstance(urlpathreale + "SchedaProdotto.aspx",
                            context.Server.MapPath("~/AspNetPages/SchedaProdotto.aspx"),
                         context);
                            context.Items["idOfferta"] = param3;
                            context.Items["Tipologia"] = param1;
                            context.Items["Lingua"] = param2;
                            break;
                        case "linkarticolilista":
                            handler = PageParser.GetCompiledPageInstance(urlpathreale + "RisultatiRicerca.aspx",
                            context.Server.MapPath("~/AspNetPages/RisultatiRicerca.aspx"),
                         context);
                            context.Items["Tipologia"] = param1;
                            context.Items["Lingua"] = param2;
                            context.Items["Categoria"] = param4;
                            context.Items["Categoria2liv"] = param5;
                            break;
                        case "linkarticolischeda":
                            handler = PageParser.GetCompiledPageInstance(urlpathreale + "SchedaOffertaMaster.aspx",
                            context.Server.MapPath("~/AspNetPages/SchedaOffertaMaster.aspx"),
                         context);
                            context.Items["idOfferta"] = param3;
                            context.Items["Tipologia"] = param1;
                            context.Items["Lingua"] = param2;
                            break;
                        //case "linkrubriche":
                        //    handler = PageParser.GetCompiledPageInstance(urlpathreale + "RisultatiRicerca.aspx",
                        //    context.Server.MapPath("~/AspNetPages/RisultatiRicerca.aspx"),
                        // context);
                        //    context.Items["idContenuto"] = param3;
                        //    context.Items["CodiceContenuto"] = param1;
                        //    context.Items["Lingua"] = param2;
                        //    break;
                        case "linkinformazioni":
                            handler = PageParser.GetCompiledPageInstance(urlpathreale + "content_tipo1.aspx",
                            context.Server.MapPath("~/AspNetPages/content_tipo1.aspx"),
                         context);
                            context.Items["ContenutoPagina"] = param1; //Qui devo fare un match con id contenuti statici
                            context.Items["Lingua"] = param3;
                            break;
                        default:
                            handler = PageParser.GetCompiledPageInstance(urlpathreale,
                               context.Server.MapPath("~/" + urlpathreale),
                        context);
                            break;
                    }
                }
                if (url.IndexOf(context.Request.ApplicationPath) != -1 && (url.IndexOf(".html") != -1))
                {
                    //urlpathreale =  url.Remove(0, context.Request.ApplicationPath.Length);
                    //urlpathreale = urlpathreale.Replace(".html", ".aspx"); //Rimappo l'html all'aspx corrispondente
                    // urlpathreale = url.Remove(0, context.Request.ApplicationPath.Length);

                    urlpathreale = url.Remove(0, context.Request.ApplicationPath.Length);
                    urlpathreale = urlpathreale.Replace(".html", ".aspx"); //Rimappo l'html all'aspx corrispondente
                    handler = PageParser.GetCompiledPageInstance(url.Replace(".html", ".aspx"),
                             context.Server.MapPath("~/" + urlpathreale),
                      context);
                }


            }
            catch (Exception err)
            {
                errorMapping = err.Message.ToString();
            }
            //SE L'HANDLER NO E'IMPOSTATO -> PAGINA NON ESISTENTE -> MANDO IL MESSAGGIO DI ERRORE
            if (handler == null)
            {
                context.Items["fileName"] = " <br/> Errore richiesta pagina - " + urlSenzaBase + " -   - " + errorMapping;
                handler = PageParser.GetCompiledPageInstance("~/errorPage.aspx",
             context.Server.MapPath("~/errorPage.aspx"),
             context);
            }
            return handler;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }

}
