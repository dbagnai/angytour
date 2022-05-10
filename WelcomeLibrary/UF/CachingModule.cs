using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            //context.PostRequestHandlerExecute += this.SetCacheCheck; //nuovo sistema cache pagine ( da ultimare )

            context.PostRequestHandlerExecute += this.SetAdditionalheaders;
            
            //attività che avvengono prima del rendering dei contenuti per la renspnse
            context.BeginRequest += new EventHandler(RewriteModule_BeginRequest);
            context.EndRequest += new EventHandler(PagecacheModule_EndRequest);
        }

      private void SetCacheCheck(object sender, EventArgs eventArgs)
      {
         //da modificar eper inserire condizioni di memorizzazione della cache solo se non presente per la pagina indicata
         //fare filtro per pagne da cachare e non!!!!!
         if (HttpContext.Current.Request.RawUrl == "/")
         {
            var f = new ResponseFilterStream(HttpContext.Current.Response.Filter);

            f.CaptureStream += F_CaptureStream;
            HttpContext.Current.Response.Filter = f;
         }
      }

      private void F_CaptureStream(MemoryStream ms)
      {
         string content = HttpContext.Current.Response.ContentEncoding.GetString(ms.ToArray());
         //HttpContext.Current.Items["tocompress"] = "false";

         File.WriteAllText(HttpContext.Current.Server.MapPath("/public") + "\\test.txt", content);

         int a = 0;
      }



      #endregion
      void PagecacheModule_EndRequest(object sender, EventArgs e)
        {
         //HttpContext context = HttpContext.Current;
         //String path = HttpContext.Current.Request.Url.AbsolutePath; //in base a questo path posso fare check della cache di pagina e tornare quella
         //HttpContext.Current.Response.Write(cotenutodallacache);
         //HttpContext.Current.Response.End();
         
         //using (MemoryStream ms = new MemoryStream())
         //{
            
         //   //HttpContext.Current.Response.OutputStream.CopyTo(ms);
         //   HttpContext.Current.Response.Filter.CopyTo(ms);
         //   //string sz = Encoding.ASCII.GetString(ms.ToArray());
         //   string content = HttpContext.Current.Response.ContentEncoding.GetString(ms.ToArray());
         //}
         int a = 0;
        }
        void RewriteModule_BeginRequest(object sender, EventArgs e)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////
            // gestione Cache nuova da abilitare per nuovo sistema di cache e inserire condizioni per filtro di pagine da cachare
            // e test di presenza del contenuto buono della chache
            ////////////////////////////////////////////////////////////////////////////////////////////
           //if (HttpContext.Current.Request.RawUrl == "/")
         //{
         //   if (File.Exists(HttpContext.Current.Server.MapPath("/public") + "\\test.txt"))
         //   {
         //      string sz = File.ReadAllText(HttpContext.Current.Server.MapPath("/public") + "\\test.txt");
         //      HttpContext.Current.Response.ContentType = "text/html";
         //      HttpContext.Current.Response.Write(sz);
         //      HttpContext.Current.Response.End();
         //   }
         //}

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
               //if (HttpContext.Current.Items["tocompress"] == "false")
               //   return;
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

#if true
    /// <summary>
    /// A semi-generic Stream implementation for Response.Filter with
    /// an event interface for handling Content transformations via
    /// Stream or String.    
    /// <remarks>
    /// Use with care for large output as this implementation copies
    /// the output into a memory stream and so increases memory usage.
    /// </remarks>
    /// </summary>    
    public class ResponseFilterStream : Stream
    {
        /// <summary>
        /// The original stream
        /// </summary>
        Stream _stream;

        /// <summary>
        /// Current position in the original stream
        /// </summary>
        long _position;

        /// <summary>
        /// Stream that original content is read into
        /// and then passed to TransformStream function
        /// </summary>
        MemoryStream _cacheStream = new MemoryStream(5000);

        /// <summary>
        /// Internal pointer that that keeps track of the size
        /// of the cacheStream
        /// </summary>
        int _cachePointer = 0;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseStream"></param>
        public ResponseFilterStream(Stream responseStream)
        {
            _stream = responseStream;
        }


        /// <summary>
        /// Determines whether the stream is captured
        /// </summary>
        private bool IsCaptured
        {
            get
            {

                if (CaptureStream != null || CaptureString != null ||
                    TransformStream != null || TransformString != null)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Determines whether the Write method is outputting data immediately
        /// or delaying output until Flush() is fired.
        /// </summary>
        private bool IsOutputDelayed
        {
            get
            {
                if (TransformStream != null || TransformString != null)
                    return true;

                return false;
            }
        }


        /// <summary>
        /// Event that captures Response output and makes it available
        /// as a MemoryStream instance. Output is captured but won't 
        /// affect Response output.
        /// </summary>
        public event Action<MemoryStream> CaptureStream;

        /// <summary>
        /// Event that captures Response output and makes it available
        /// as a string. Output is captured but won't affect Response output.
        /// </summary>
        public event Action<string> CaptureString;



        /// <summary>
        /// Event that allows you transform the stream as each chunk of
        /// the output is written in the Write() operation of the stream.
        /// This means that that it's possible/likely that the input 
        /// buffer will not contain the full response output but only
        /// one of potentially many chunks.
        /// 
        /// This event is called as part of the filter stream's Write() 
        /// operation.
        /// </summary>
        public event Func<byte[], byte[]> TransformWrite;


        /// <summary>
        /// Event that allows you to transform the response stream as
        /// each chunk of bytep[] output is written during the stream's write
        /// operation. This means it's possibly/likely that the string
        /// passed to the handler only contains a portion of the full
        /// output. Typical buffer chunks are around 16k a piece.
        /// 
        /// This event is called as part of the stream's Write operation.
        /// </summary>
        public event Func<string, string> TransformWriteString;

        /// <summary>
        /// This event allows capturing and transformation of the entire 
        /// output stream by caching all write operations and delaying final
        /// response output until Flush() is called on the stream.
        /// </summary>
        public event Func<MemoryStream, MemoryStream> TransformStream;

        /// <summary>
        /// Event that can be hooked up to handle Response.Filter
        /// Transformation. Passed a string that you can modify and
        /// return back as a return value. The modified content
        /// will become the final output.
        /// </summary>
        public event Func<string, string> TransformString;


        protected virtual void OnCaptureStream(MemoryStream ms)
        {
            if (CaptureStream != null)
                CaptureStream(ms);
        }


        private void OnCaptureStringInternal(MemoryStream ms)
        {
            if (CaptureString != null)
            {
                string content = HttpContext.Current.Response.ContentEncoding.GetString(ms.ToArray());
                OnCaptureString(content);
            }
        }

        protected virtual void OnCaptureString(string output)
        {
            if (CaptureString != null)
                CaptureString(output);
        }

        protected virtual byte[] OnTransformWrite(byte[] buffer)
        {
            if (TransformWrite != null)
                return TransformWrite(buffer);
            return buffer;
        }

        private byte[] OnTransformWriteStringInternal(byte[] buffer)
        {
            Encoding encoding = HttpContext.Current.Response.ContentEncoding;
            string output = OnTransformWriteString(encoding.GetString(buffer));
            return encoding.GetBytes(output);
        }

        private string OnTransformWriteString(string value)
        {
            if (TransformWriteString != null)
                return TransformWriteString(value);
            return value;
        }


        protected virtual MemoryStream OnTransformCompleteStream(MemoryStream ms)
        {
            if (TransformStream != null)
                return TransformStream(ms);

            return ms;
        }




        /// <summary>
        /// Allows transforming of strings
        /// 
        /// Note this handler is internal and not meant to be overridden
        /// as the TransformString Event has to be hooked up in order
        /// for this handler to even fire to avoid the overhead of string
        /// conversion on every pass through.
        /// </summary>
        /// <param name="responseText"></param>
        /// <returns></returns>
        private string OnTransformCompleteString(string responseText)
        {
            if (TransformString != null)
                TransformString(responseText);

            return responseText;
        }

        /// <summary>
        /// Wrapper method form OnTransformString that handles
        /// stream to string and vice versa conversions
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        internal MemoryStream OnTransformCompleteStringInternal(MemoryStream ms)
        {
            if (TransformString == null)
                return ms;

            //string content = ms.GetAsString();
            string content = HttpContext.Current.Response.ContentEncoding.GetString(ms.ToArray());

            content = TransformString(content);
            byte[] buffer = HttpContext.Current.Response.ContentEncoding.GetBytes(content);
            ms = new MemoryStream();
            ms.Write(buffer, 0, buffer.Length);
            //ms.WriteString(content);

            return ms;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override long Seek(long offset, System.IO.SeekOrigin direction)
        {
            return _stream.Seek(offset, direction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        public override void SetLength(long length)
        {
            _stream.SetLength(length);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Close()
        {
            _stream.Close();
        }

        /// <summary>
        /// Override flush by writing out the cached stream data
        /// </summary>
        public override void Flush()
        {

            if (IsCaptured && _cacheStream.Length > 0)
            {
                // Check for transform implementations
                _cacheStream = OnTransformCompleteStream(_cacheStream);
                _cacheStream = OnTransformCompleteStringInternal(_cacheStream);

                OnCaptureStream(_cacheStream);
                OnCaptureStringInternal(_cacheStream);

                // write the stream back out if output was delayed
                if (IsOutputDelayed)
                    _stream.Write(_cacheStream.ToArray(), 0, (int)_cacheStream.Length);

                // Clear the cache once we've written it out
                _cacheStream.SetLength(0);
            }

            // default flush behavior
            _stream.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }


        /// <summary>
        /// Overriden to capture output written by ASP.NET and captured
        /// into a cached stream that is written out later when Flush()
        /// is called.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (IsCaptured)
            {
                // copy to holding buffer only - we'll write out later
                _cacheStream.Write(buffer, 0, count);
                _cachePointer += count;
            }

            // just transform this buffer
            if (TransformWrite != null)
                buffer = OnTransformWrite(buffer);
            if (TransformWriteString != null)
                buffer = OnTransformWriteStringInternal(buffer);

            if (!IsOutputDelayed)
                _stream.Write(buffer, offset, buffer.Length);

        }

    }


#endif

}
