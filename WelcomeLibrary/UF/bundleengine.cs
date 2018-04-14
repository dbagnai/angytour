
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Routing;
using NUglify;


namespace WelcomeLibrary.UF
{
    public static class BundleEngine
    {
        public enum EnumBundleMode { MD5, LastWriteTime }
        public enum EnumInjectionMode { SingleLinkOrScript, SingleCombinedScript, TagBlock }

        /// <summary>
        /// Se impostato a true, quando l'applicazione gira in localhost inietta gli script in modo convenzionale
        /// uno per uno, senza utilizzare l'handler
        /// </summary>
        public static bool DebugMode = true;

        public class BundleOptionsFactory
        {
            /// <summary>
            /// Determine if compute hash to append to query string of a script file, 
            /// </summary>
            public bool CheckFilesAlways { get; set; }

            /// <summary>
            /// Determine which algorythm use to append query string of a script file
            /// </summary>
            public EnumBundleMode BundleMode { get; set; }
            public EnumInjectionMode InjectionMode { get; set; }

            /// <summary>
            /// Tells to Render to add Type attribute on JS links : type="text/javascript"
            /// </summary>
            public bool ScriptTypeAttribute { get; set; }


            public bool minifyCss { get; set; }
            public bool minifyJs { get; set; }

            /// <summary>
            /// Determine if use file extension in handler
            /// require mapping in system.webserver/handlers, see documentation
            /// </summary>
            public bool UseFileExtension { get; set; }

            /// <summary>
            /// Determine the expiring-time of file cache for
            /// the handler output
            /// defatult : 345600 = 4 days
            /// </summary>

            public string HandlerCacheMaxAge { get; set; }

            public BundleOptionsFactory()
            {
                minifyCss = false;
                minifyJs = false;
                CheckFilesAlways = false;
                BundleMode = EnumBundleMode.MD5;
                ScriptTypeAttribute = false;
                InjectionMode = EnumInjectionMode.SingleLinkOrScript;
                UseFileExtension = false;
                HandlerCacheMaxAge = "345600";
            }
        }

        public class BundleItem
        {
            public string FileName { get; set; }
            internal string FileNameHash { get; set; }

            internal string FileContent { get; set; }
        }

        internal class BundleHashCollection : List<BundleItem>
        {
            public string HashHexString { get; set; }
            public DateTime? LastCacheBuildDateTime { get; set; }
            public string LastCacheBuildDateTimeString
            {
                get { return LastCacheBuildDateTime.HasValue ? LastCacheBuildDateTime.Value.ToString("yyyyMMddHHmmss") : ""; }
            }
            public BundleHashCollection()
            {
                HashHexString = "";
                LastCacheBuildDateTime = null;
            }

        }

        private static Dictionary<string, BundleHashCollection> BundleHashJS = new Dictionary<string, BundleHashCollection>();
        private static Dictionary<string, BundleHashCollection> BundleHashCSS = new Dictionary<string, BundleHashCollection>();
        private static Dictionary<string, List<string>> JSColl = new Dictionary<string, List<string>>();
        private static Dictionary<string, List<string>> CSSColl = new Dictionary<string, List<string>>();
        public static BundleOptionsFactory BundleOptions = new BundleOptionsFactory();

        public static void AddRoutes(RouteCollection routes)
        {

            if (BundleEngine.BundleOptions.UseFileExtension)
            {
                // per farlo funzionare come files bisogna attivare nel web config le seguenti righe
                // in system.webserver/handlers
                //<add name="CSSFileHandler" path="*.csx" verb="GET" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />
                //<add name="JSFileHandler" path="*.jsx" verb="GET" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />

                routes.Add(
                    new Route("bdejs/{bundlename}.jsx",
                    new RouteValueDictionary { { "filetype", "js" } },
                    new BoundleProviderRouteHandler())
                    );
                routes.Add(
                    new Route("bdecss/{bundlename}.csx",
                    new RouteValueDictionary { { "filetype", "css" } },
                    new BoundleProviderRouteHandler())
                    );

            }
            else
            {
                routes.Add(
                    new Route("bdejs/{bundlename}",
                    new RouteValueDictionary { { "filetype", "js" } },
                    new BoundleProviderRouteHandler())
                    );
                routes.Add(
                    new Route("bdecss/{bundlename}",
                    new RouteValueDictionary { { "filetype", "css" } },
                    new BoundleProviderRouteHandler())
                    );
            }

        }

        public static string RenderJS(string BundleName, EnumInjectionMode? injectionMode = null)
        {
            string ret = "";

            EnumInjectionMode optionInjectmode = injectionMode.HasValue ? injectionMode.Value : BundleOptions.InjectionMode;

            if (BundleHashJS.ContainsKey(BundleName))
            {
                if (BundleOptions.CheckFilesAlways)
                    ReComputeHashBundle(JSColl, BundleName);

                var bundleList = BundleHashJS[BundleName];
                StringBuilder sb = new StringBuilder();

                if (optionInjectmode == EnumInjectionMode.TagBlock)
                {
                    foreach (var m in bundleList)
                    {
                        sb.AppendLine("<script type=\"text/javascript\">");
                        sb.AppendLine(File.ReadAllText(m.FileName));
                        sb.AppendLine("</script>");
                    }
                }
                else if (optionInjectmode == EnumInjectionMode.SingleLinkOrScript)
                {
                    string scriptType = BundleOptions.ScriptTypeAttribute ? " type=\"text/javascript\"" : "";

                    foreach (var m in bundleList)
                        sb.AppendFormat("<script" + scriptType + " src=\"{0}\"></script>", m.FileNameHash);

                }
                else if (optionInjectmode == EnumInjectionMode.SingleCombinedScript)
                {
                    string scriptType = BundleOptions.ScriptTypeAttribute ? " type=\"text/javascript\"" : "";

                    if (BundleOptions.UseFileExtension)
                        sb.AppendFormat("<script" + scriptType + " src=\"/bdejs/{0}\"></script>", BundleName + ".jsx?v=" + bundleList.HashHexString);
                    else
                        sb.AppendFormat("<script" + scriptType + " src=\"/bdejs/{0}\"></script>", BundleName + "?v=" + bundleList.HashHexString);


                }

                ret = sb.ToString();
            }

            return ret;
        }

        public static string RenderFullBundleJS(string BundleName, bool fromCache = false)
        {
            string ret = "";

            if (BundleHashJS.ContainsKey(BundleName))
            {

                var bundleList = BundleHashJS[BundleName];
                StringBuilder sb = new StringBuilder();

                if (fromCache)
                {
                    foreach (var m in bundleList)
                        sb.AppendLine(m.FileContent);
                }
                else
                {
                    foreach (var m in bundleList)
                        sb.AppendLine(File.ReadAllText(m.FileName));

                }

                ret = sb.ToString();
            }

            return ret;
        }

        public static string RenderCSS(string BundleName, EnumInjectionMode? injectionMode = null)
        {
            string ret = "";

            EnumInjectionMode optionInjectmode = injectionMode.HasValue ? injectionMode.Value : BundleOptions.InjectionMode;

            if (BundleHashCSS.ContainsKey(BundleName))
            {

                var bundleList = BundleHashCSS[BundleName];
                StringBuilder sb = new StringBuilder();

                if (BundleOptions.CheckFilesAlways)
                    ReComputeHashBundle(CSSColl, BundleName);

                if (optionInjectmode == EnumInjectionMode.TagBlock)
                {
                    foreach (var m in bundleList)
                    {
                        sb.AppendLine("<style type=\"text/css\">");
                        sb.AppendLine(File.ReadAllText(m.FileName));
                        sb.AppendLine("</style>");
                    }
                }
                else if (optionInjectmode == EnumInjectionMode.SingleLinkOrScript)
                {

                    foreach (var m in bundleList)
                        sb.AppendFormat("<link rel=\"stylesheet\" href=\"{0}\" />", m.FileNameHash);
                }
                else if (optionInjectmode == EnumInjectionMode.SingleCombinedScript)
                {

                    if (BundleOptions.UseFileExtension)
                        sb.AppendFormat("<link rel=\"stylesheet\" href=\"/bdecss/{0}\" />", BundleName + ".csx?v=" + bundleList.HashHexString);
                    else
                        sb.AppendFormat("<link rel=\"stylesheet\" href=\"/bdecss/{0}\" />", BundleName + "?v=" + bundleList.HashHexString);


                }
                ret = sb.ToString();
            }

            return ret;
        }

        public static string RenderFullBundleCSS(string BundleName, bool fromCache = false)
        {
            string ret = "";

            if (BundleHashCSS.ContainsKey(BundleName))
            {

                var bundleList = BundleHashCSS[BundleName];
                StringBuilder sb = new StringBuilder();

                if (fromCache)
                {
                    foreach (var m in bundleList)
                        sb.AppendLine(m.FileContent);
                }
                else
                {
                    foreach (var m in bundleList)
                        sb.AppendLine(File.ReadAllText(m.FileName));

                }

                ret = sb.ToString();
            }

            return ret;
        }

        public static void AddBundleJS(string BundleName, params string[] paramArray)
        {
            if (paramArray != null)
                AddBundleJS(BundleName, paramArray.ToList());
        }

        public static void AddBundleJS(string BundleName, List<string> JSList)
        {
            Dictionary<string, List<string>> dc = JSColl;
            AddBundle(dc, BundleName, JSList);
        }


        public static void AddItemBundleJS(string BundleName, string js)
        {
            Dictionary<string, List<string>> dc = JSColl;
            AddItem(dc, BundleName, js);
        }
        public static void AddBundleCSS(string BundleName, params string[] paramArray)
        {
            if (paramArray != null)
                AddBundleCSS(BundleName, paramArray.ToList());
        }
        public static void AddBundleCSS(string BundleName, List<string> CSSList)
        {
            Dictionary<string, List<string>> dc = CSSColl;
            AddBundle(dc, BundleName, CSSList);
        }


        public static void AddItemBundleCSS(string BundleName, string css)
        {
            Dictionary<string, List<string>> dc = CSSColl;
            AddItem(dc, BundleName, css);
        }

        private static void AddBundle(Dictionary<string, List<string>> dc, string bundleName, List<string> fileList, bool AddToList = true)
        {
            if (dc == null) dc = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(bundleName))
                return;

            if ((fileList == null) || (fileList.Count == 0))
                return;

            bool reComputeHash = false;

            if (!dc.ContainsKey(bundleName))
            {
                dc.Add(bundleName, fileList);
                reComputeHash = true;
            }
            else
            {
                if (AddToList)
                {
                    var bundle = dc[bundleName];
                    foreach (var m in fileList)
                    {
                        if (!bundle.Contains(m))
                        {
                            bundle.Add(m);
                            reComputeHash = true;
                        }
                    }
                }
            }

            if (reComputeHash)
                ReComputeHashBundle(dc, bundleName);
        }


        private static void AddItem(Dictionary<string, List<string>> dc, string bundleName, string fileName)
        {
            if (dc == null) dc = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(bundleName))
                return;
            if (string.IsNullOrEmpty(fileName))
                return;

            bool reComputeHash = false;

            if (!dc.ContainsKey(bundleName))
            {
                List<string> tmp = new List<string>();
                tmp.Add(fileName);
                dc.Add(bundleName, tmp);
            }
            else
            {
                var bundle = dc[bundleName];
                if (!dc[bundleName].Contains(fileName))
                    dc[bundleName].Add(fileName);
            }

            if (reComputeHash)
                ReComputeHashFile(dc, bundleName, fileName);
        }

        private static void ReComputeHashBundle(Dictionary<string, List<string>> dc, string bundleName)
        {
            if ((dc != null) && (!string.IsNullOrEmpty(bundleName)))
            {
                if ((!dc.Equals(JSColl)) && (!dc.Equals(CSSColl)))
                    return;

                Dictionary<string, BundleHashCollection> bh = null;
                if (dc.Equals(JSColl))
                    bh = BundleHashJS;
                else if (dc.Equals(CSSColl))
                    bh = BundleHashCSS;

                // azzero il bundle
                if (!bh.ContainsKey(bundleName))
                    bh.Add(bundleName, new BundleHashCollection());
                else
                    bh[bundleName] = new BundleHashCollection();

                var bundleList = bh[bundleName];

                string bundleHashString = "";

                foreach (var m in dc[bundleName])
                {
                    string fname = HttpContext.Current.Server.MapPath(m);
                    if (File.Exists(fname))
                    {
                        string dest = "";
                        if (BundleOptions.BundleMode == EnumBundleMode.MD5)
                        {
                            string hash = CalcMd5FromFile(fname);
                            bundleHashString += hash;
                            dest = (m + "?v=" + hash).TrimStart('~');

                        }
                        else if (BundleOptions.BundleMode == EnumBundleMode.LastWriteTime)
                        {
                            FileInfo fi = new FileInfo(fname);
                            string fileDate = fi.LastWriteTime.ToString("yyyyMMddHHmmss");
                            bundleHashString += fileDate;
                            dest = (m + "?v=" + fileDate).TrimStart('~');
                        }

                        string content = File.ReadAllText(fname);

                        BundleItem bi = new BundleItem() { FileName = fname, FileNameHash = dest, FileContent = content };

                        bundleList.Add(bi);
                    }

                }
                bundleList.HashHexString = CalcMd5Hash(bundleHashString);
                bundleList.LastCacheBuildDateTime = DateTime.Now;
            }
        }
        private static void ReComputeHashFile(Dictionary<string, List<string>> dc, string bundleName, string fileName)
        {
            if ((dc != null) && (!string.IsNullOrEmpty(bundleName)))
            {
                if ((!dc.Equals(JSColl)) && (!dc.Equals(CSSColl)))
                    return;

                Dictionary<string, BundleHashCollection> bh = null;
                if (dc.Equals(JSColl))
                    bh = BundleHashJS;
                else if (dc.Equals(CSSColl))
                    bh = BundleHashCSS;


                if (!bh.ContainsKey(bundleName))
                    bh.Add(bundleName, new BundleHashCollection());


                var bundleList = bh[bundleName];

                string bundleHashString = "";

                foreach (var m in dc[bundleName])
                {
                    string fname = HttpContext.Current.Server.MapPath(m);
                    if (File.Exists(fname))
                    {
                        string dest = "";
                        if (BundleOptions.BundleMode == EnumBundleMode.MD5)
                        {
                            string hash = CalcMd5FromFile(fname);
                            bundleHashString += hash;
                            dest = (m + "?v=" + hash).TrimStart('~');

                        }
                        else if (BundleOptions.BundleMode == EnumBundleMode.LastWriteTime)
                        {
                            FileInfo fi = new FileInfo(fname);
                            string fileDate = fi.LastWriteTime.ToString("yyyyMMddHHmmss");
                            bundleHashString += fileDate;
                            dest = (m + "?v=" + fileDate).TrimStart('~');
                        }

                        BundleItem bi = null;

                        var f = bundleList.Where(z => z.FileName == fname).FirstOrDefault();
                        if (f != null)
                            f.FileNameHash = dest;
                        else
                        {
                            bi = new BundleItem() { FileName = fname, FileNameHash = dest };
                            bundleList.Add(bi);
                        }
                    }

                }

                bundleList.HashHexString = CalcMd5Hash(bundleHashString);
            }
        }

        private static string CalcMd5FromFile(string filename)
        {
            string sz = "";
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                if (File.Exists(filename))
                {
                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    byte[] md5Hash = md5.ComputeHash(fs);
                    fs.Close();
                    fs.Dispose();
                    sz = ByteArrayToStringHEX(md5Hash);
                }
            }
            catch { }
            return sz;
        }

        private static string CalcMd5Hash(string ToHash)
        {

            // First we need to convert the string into bytes, which means using a text encoder.
            Encoder enc = System.Text.Encoding.ASCII.GetEncoder();

            // Create a buffer large enough to hold the string
            byte[] data = new byte[ToHash.Length];
            enc.GetBytes(ToHash.ToCharArray(), 0, ToHash.Length, data, 0, true);

            // This is one implementation of the abstract class MD5.
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(data);

            //return BitConverter.ToString(result).Replace("-", "").ToLower();
            return ByteArrayToStringHEX(md5Hash);
        }

        private static string ByteArrayToStringHEX(byte[] ba, string separator = "")
        {
            string hex = BitConverter.ToString(ba);
            if (!string.IsNullOrEmpty(separator))
                hex = hex.Replace("-", separator);
            else
                hex = hex.Replace("-", "");

            return hex;
        }


    }

    public class BoundleProviderRouteHandler : System.Web.Routing.IRouteHandler
    {
        public IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
        {
            BoundleProviderHandler httpHandler = new BoundleProviderHandler();
            return httpHandler;
        }
        public class BoundleProviderHandler : IHttpHandler
        {

            public void ProcessRequest(HttpContext context)
            {
                //bool Error = false;
                string fileType = context.Request.RequestContext.RouteData.Values["filetype"] != null ? context.Request.RequestContext.RouteData.Values["filetype"].ToString() : "";
                string bundleName = context.Request.RequestContext.RouteData.Values["bundlename"] != null ? context.Request.RequestContext.RouteData.Values["bundlename"].ToString() : "";

                string CType = "text/plain";
                string ret = "";

                //routes.Add(new Route("bde/{filetype}/{bundlename}", new DownloadRouteHandler()));
                if (string.IsNullOrEmpty(fileType) || string.IsNullOrEmpty(bundleName))
                {
                    context.Response.ContentType = CType;
                    context.Response.Write("");
                    return;
                }
                //https://www.nuget.org/packages/NUglify/1.5.10
                if (fileType.ToLower() == "js")
                {
                    ret = BundleEngine.RenderFullBundleJS(bundleName, true);
                    CType = "application/javascript;charset=UTF-8";
                    if (BundleEngine.BundleOptions.minifyJs)
                        ret = minifyjs(ret);


                }
                else if (fileType.ToLower() == "css")
                {
                    ret = BundleEngine.RenderFullBundleCSS(bundleName, true);
                    CType = "text/css";
                    if (BundleEngine.BundleOptions.minifyCss)
                        ret = minifycss(ret);
                }

                // gli handler rispondono xon cache-control : private , ma così il browser non fa cache
                // con public invece farà la cache
                //context.Response.Headers.Remove("Cache-Control");
                //context.Response.Headers["Cache-Control"] = "public";

                string maxAge = !string.IsNullOrEmpty(BundleEngine.BundleOptions.HandlerCacheMaxAge) ? BundleEngine.BundleOptions.HandlerCacheMaxAge : "345600";

                context.Response.Headers.Remove("Cache-Control");
                context.Response.AppendHeader("Cache-Control", "max-age=" + maxAge);
                //context.Response.Cache.SetCacheability(HttpCacheability.Server  .NoCache);

                context.Response.ContentType = CType;

                //var minifier = new Microsoft.Ajax.Utilities.Minifier();
                //var minifiedString = minifier.MinifyJavaScript(unMinifiedString);



                context.Response.Write(ret);

            }
            public string minifyjs(string text)
            {
                string ret = text;
                NUglify.JavaScript.CodeSettings jsset = new NUglify.JavaScript.CodeSettings();
                //jsset.OutputMode = OutputMode.MultipleLines;
                jsset.PreserveImportantComments = false;
                UglifyResult urjs = NUglify.Uglify.Js(ret, jsset);
                ret = urjs.Code;
                return ret;
            }
            public string minifycss(string text)
            {
                string ret = text;
                NUglify.Css.CssSettings cssset = new NUglify.Css.CssSettings();

                //cssset.OutputMode = OutputMode.MultipleLines;
                cssset.CommentMode = NUglify.Css.CssComment.None;
                UglifyResult urcss = NUglify.Uglify.Css(ret, cssset);
                ret = urcss.Code;
                return ret;
            }

            public bool IsReusable
            {
                get
                {
                    return false;
                }
            }


        }
    }
}
