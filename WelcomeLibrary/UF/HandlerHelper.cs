using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WelcomeLibrary.UF
{
    public static class HandlerHelper
    {
        //private Dictionary<string, string> parameterList = new Dictionary<string, string>();					
        public static string initialDTSfromExtern { get; set; }
        public static string initialDTEfromExtern { get; set; }

        public static string GetPostContent(HttpContext context)
        {
            string ret = "";
            try
            {
                Stream body = context.Request.InputStream;
                System.Text.Encoding encoding = context.Request.ContentEncoding;
                using (StreamReader reader = new StreamReader(body, encoding))
                {
                    ret = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch { }

            return ret;
        }

        public static Dictionary<string, string> GetParams(string Body)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(Body))
            {
                if (Body.IndexOf("Content-Disposition") == -1)
                {
                    string[] m = Body.Split('\r');
                    for (int i = 0; i < m.Length; i++)
                    {
                        string[] c = m[i].Split('=');
                        string key = (c.Length > 0) ? c[0].ToLower() : "";
                        string val = (c.Length > 1) ? c[1] : "";
                        if (!string.IsNullOrEmpty(key))
                            ret.Add(key, val);
                    }
                }
                else
                {
                    // mo so cazzi
                    //--Bf-Z9YBEDdy1rtt6anQ98_AM3_CxPIzTEF0pF3Bc
                    //Content-Disposition: form-data; name="keystream"; filename="keystream"
                    //Content-Type: application/octet-stream


                    //bnVsbDw/eG1sIHZlcnNpb249IjEuMCIgZW5jb2Rpbmc9InV0Zi04Ij8+DQo8
                    //Uml0b3JubyB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1M
                    //U2NoZW1hLWluc3RhbmNlIiB4bWxuczp4c2Q9Imh0dHA6Ly93d3cudzMub3Jn

                    MemoryStream ms = new MemoryStream();
                    TextWriter tw = new StreamWriter(ms);
                    tw.Write(Body);
                    tw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    try
                    {
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            bool readBoundary = true;
                            string boundaryCode = "";
                            // tiriamo su il primo parametro
                            while (!sr.EndOfStream)
                            {
                                if (readBoundary) { boundaryCode = sr.ReadLine(); readBoundary = false; }

                                string paramKey = "";
                                string sz = "";
                                int countheader = 0;
                                while (countheader < 3)
                                {
                                    sz = sr.ReadLine();
                                    if (sz.IndexOf("Content-Disposition") > -1)
                                    {
                                        string[] h = sz.Split(';');
                                        for (int i = 0; i < h.Length; i++)
                                        {
                                            if (h[i].IndexOf("name=") > -1)
                                            {
                                                string[] hh = h[i].Split('=');
                                                if (hh.Length > 1) paramKey = hh[1].Trim('"');

                                                break;
                                            }
                                        }
                                    }
                                    countheader++;
                                }

                                StringBuilder MultiPartBody = new StringBuilder();
                                //if (!sr.EndOfStream) sz = sr.ReadLine();
                                while (!sr.EndOfStream)
                                {
                                    sz = sr.ReadLine();
                                    if (sz == boundaryCode) break;
                                    MultiPartBody.Append(sz);

                                }
                                if ((!string.IsNullOrEmpty(paramKey)) && (MultiPartBody.Length > 0))
                                {
                                    string mp = MultiPartBody.ToString();
                                    string newBoundary = boundaryCode + "--";
                                    if (mp.EndsWith(newBoundary))
                                        mp = mp.Remove(mp.Length - newBoundary.Length, newBoundary.Length);
                                    ret.Add(paramKey, mp);
                                }
                            }
                            sr.Close();
                        }
                    }
                    catch  { 
                        //Utility.Logging.Error("GetParams", ex); 
                    }
                    tw.Close();
                    ms.Close();
                }

            }
            //parameterList = ret;
            return ret;
        }

        public static Dictionary<string, string> GetPostParams(HttpContext context)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string Body = GetPostContent(context);
            if (!string.IsNullOrEmpty(Body))
            {
                string[] m = Body.Split('&');
                for (int i = 0; i < m.Length; i++)
                {
                    string[] c = m[i].Split('=');
                    //string key = (c.Length > 0) ? System.Web.HttpUtility.UrlDecode(c[0].ToLower()) : "";
                    string key = (c.Length > 0) ? System.Web.HttpUtility.UrlDecode(c[0]) : "";
                    string val = (c.Length > 1) ? System.Web.HttpUtility.UrlDecode(c[1]) : "";
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (!ret.ContainsKey(key))
                            ret.Add(key, val);
                        else
                            ret[key] = val;
                    }
                }
            }
            //parameterList = ret;
            return ret;
        }

        public static string ParamFormatter(string param)
        {
            return param.Replace(' ', '+').Replace("%2f", "/").Replace("%3d", "=");
        }


    }
}
