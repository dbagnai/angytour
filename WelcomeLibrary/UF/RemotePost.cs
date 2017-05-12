using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.UF
{
    [Serializable]
    public class RemotePost
    {
        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();
        public string Url = "";
        public string Method = "POST";
        public string FormName = "form1";
        public bool test = false;

        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }
        private string CreateInput(string Name, string Value)
        {
            return "<input type=\"hidden\" name=\"" + Name + "\" value=\"" + Value + "\" />";
        }
        public string DataForm
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                //Inseriamo gli input fields hidden
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    sb.AppendLine(CreateInput(Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                if (test) sb.AppendLine(CreateInput("TEST", "true")); //caso di transazioni di test

                sb.AppendLine("<div style=\"display:none;\"><input type=\"submit\" value=\"Submit Payment Info\" /></div>");
                return sb.ToString();
            }
        }
        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write("");
            System.Web.HttpContext.Current.Response.Write(string.Format("", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("", FormName, Method, Url));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            System.Web.HttpContext.Current.Response.Write("");
            System.Web.HttpContext.Current.Response.Write("");
            System.Web.HttpContext.Current.Response.End();
        }
    }
}
