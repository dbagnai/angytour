using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace WelcomeLibrary.UF
{
    public class CachingModule : IHttpModule
    {
        bool enablecompression = false;
        /// <summary>
        /// Il modulo dovrà essere configurato nel file Web.config del
        /// Web e registrato con IIS prima di poter essere utilizzato. Per ulteriori informazioni
        /// visitare il sito all'indirizzo: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //Inserire qui il codice di pulizia.
        }

        public void Init(HttpApplication context)
        {
            // Segue un esempio di come gestire l'evento LogRequest e fornire la relativa 
            // implementazione della registrazione personalizzata
            context.PreSendRequestHeaders += this.SetDefaultCacheHeader;
           
            //ABILITAZIONE COMPRESSIONE CONTENUTI CON VARIABILE DI CONFIG
            //ATTENZIONE!!! disabilitare la static e dinamic conpression su web.config sennoo con la doppia compressione non funziona 
            //METTERE SU WEB.CONFIG ( SE IMPOSTI enablecontentcompression=true)
            //	<urlCompression doStaticCompression="false" doDynamicCompression="false" />
            if (WelcomeLibrary.UF.ConfigManagement.ReadKey("enablecontentcompression").ToLower() == "true")
                enablecompression = true;
            if (enablecompression)
                context.PostRequestHandlerExecute += this.SetCompressionHnd;
            context.PostRequestHandlerExecute += this.SetAdditionalheaders;
            
            //attività che avvengono prima del rendering dei contenuti per la renspnse
            context.BeginRequest += new EventHandler(RewriteModule_BeginRequest);
        }

        #endregion

        void RewriteModule_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            //String path = HttpContext.Current.Request.Url.AbsolutePath; //in base a questo path posso fare check della cache di pagina e tornare quella
            //con
            //HttpContext.Current.Response.Write(cotenutodallacache);
            //HttpContext.Current.Response.End();
            //da completare salvando le pagine in una tabella al primo rendering e aggiungendo la politica di gestione cache

        }
        private void SetAdditionalheaders(object sender, EventArgs eventArgs)
        {
            HttpContext context = HttpContext.Current;
            String path = HttpContext.Current.Request.Url.AbsolutePath;
            //Accept-CH: DPR, Width, Viewport-Width
            if (!HttpContext.Current.Response.HeadersWritten)
            {
                HttpContext.Current.Response.AppendHeader("Accept-CH", "DPR, Width, Viewport-Width");
                HttpContext.Current.Response.AppendHeader("Vary", "Accept-Encoding");
            }

            String Viewportwidth = context.Request.Headers.Get("Viewport-Width"); //Questa è la viewport del CLIENT presa dalla richiesta !!!
            if (!string.IsNullOrEmpty(Viewportwidth) && (HttpContext.Current.Session != null))
            {
                //WelcomeLibrary.STATIC.Global.Viewportw = Viewportwidth;
                Utility.ViewportwManagerSet(HttpContext.Current.Session.SessionID, Viewportwidth);
            }
        }


        private void SetCompressionHnd(object sender, EventArgs eventArgs)
        {
            HttpContext context = HttpContext.Current;
            String encoding = context.Request.Headers.Get("Accept-Encoding");
            String path = HttpContext.Current.Request.Url.AbsolutePath;

            string compresstypes = WelcomeLibrary.UF.ConfigManagement.ReadKey("compresstypes").ToLower();
            List<string> listmimetypetocomress = compresstypes.Split(',').ToList();

            //text/plain,text/html,text/css,application/javascript,application/json,application/atom+xml,application/xaml+xml,message/*
            if (listmimetypetocomress != null)
            {
                if (!listmimetypetocomress.Exists(item => HttpContext.Current.Response.ContentType.ToLower().Contains(item.ToLower())))
                    return;
            }
            else return;


            if (encoding == null)
                return;
            encoding = encoding.ToLower();
            if (path != null)
            {
                path = path.ToLower();
                if (path.Contains(".axd")) return;
            }

            if (encoding.Contains("gzip"))
            {
                //context.Response.Filter = new System.IO.Compression.GZipStream(context.Response.Filter, System.IO.Compression.CompressionMode.Compress);
                //context.Response.Filter = new System.IO.Compression.GZipStream(context.Response.Filter, System.IO.Compression.CompressionLevel.Fastest);
                context.Response.Filter = new System.IO.Compression.GZipStream(context.Response.Filter, System.IO.Compression.CompressionLevel.Optimal);


                if (!HttpContext.Current.Response.HeadersWritten)
                    HttpContext.Current.Response.AppendHeader("Content-Encoding", "gzip");
            }
            else
            {
                //context.Response.Filter = new System.IO.Compression.DeflateStream(context.Response.Filter, System.IO.Compression.CompressionMode.Compress);
                context.Response.Filter = new System.IO.Compression.DeflateStream(context.Response.Filter, System.IO.Compression.CompressionLevel.Optimal);
                if (!HttpContext.Current.Response.HeadersWritten)
                    HttpContext.Current.Response.AppendHeader("Content-Encoding", "deflate");
            }

        }

        private void SetDefaultCacheHeader(object sender, EventArgs eventArgs)
        {
            double secondsduration = 690000;
            bool nocache = false;
            string finalpath = HttpContext.Current.Request.Url.AbsolutePath;
            if (finalpath.ToLower().EndsWith(".ashx"))
                nocache = true;
            if (finalpath.ToLower().EndsWith(".axd"))
                nocache = true;
            if (HttpContext.Current.Response.ContentType == "text/plain")
                nocache = true;
            if (HttpContext.Current.Response.ContentType == "text/html")
                nocache = true;

            if (!nocache)
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
                HttpContext.Current.Response.Cache.SetMaxAge(TimeSpan.FromSeconds(secondsduration));
                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(secondsduration));
            }
            else
            {
                DisableClientCaching();
            }
        }
        public void OnLogRequest(Object source, EventArgs e)
        {
            //La logica della registrazione personalizzata può essere inserita qui
        }


        private void DisableClientCaching()
        {
            // Do any of these result in META tags e.g. <META HTTP-EQUIV="Expire" CONTENT="-1">
            // HTTP Headers or both?

            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);

            // Does this only work for IE?
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Is this required for FireFox? Would be good to do this without magic strings.
            // Won't it overwrite the previous setting
            HttpContext.Current.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Current.Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            HttpContext.Current.Response.AppendHeader("Expires", "0"); // Proxies.
            // Why is it necessary to explicitly call SetExpires. Presume it is still better than calling
            // Response.Headers.Add( directly
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
        }
    }
}
