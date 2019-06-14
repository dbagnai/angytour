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
            foreach (string key in context.Request.QueryString.AllKeys)
            {
                context.Items[key] = context.Request.QueryString[key];
            }

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

          return handler;
             
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }

}
