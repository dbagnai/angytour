using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AreaContenuti_gestionesconti : CommonPage
{
    public string idsconto
    {
        get { return ViewState["idsconto"] != null ? (string)(ViewState["idsconto"]) : ""; }
        set { ViewState["idsconto"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SetCulture("it"); //forzo la cultura italia
        if (!IsPostBack)
        {
            if (Request.QueryString["idsconto"] != null && Request.QueryString["idsconto"] != "")
            { idsconto = Request.QueryString["idsconto"].ToString(); hididselected.Value = idsconto; }


        }
        else
        {
            if (Request["__EVENTTARGET"] == "nonusato")
            {
            }
        }



    }

    public string InjectedStartPageScripts()
    {
        Dictionary<string, string> addelements = new Dictionary<string, string>();
        String scriptRegVariables = "";
        //Preparazione dei modelli per vue vuoti in pagina
        scriptRegVariables += ";\r\n " + string.Format("var initpagemodelsconti = {0}",
         JsonConvert.SerializeObject(new initpagemodelsconti(), Formatting.Indented, new JsonSerializerSettings()
         {
             NullValueHandling = NullValueHandling.Ignore,
             MissingMemberHandling = MissingMemberHandling.Ignore,
             ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
             PreserveReferencesHandling = PreserveReferencesHandling.None,
         }));
        scriptRegVariables += ";\r\n " + string.Format("var scontivuemodel = {0}",
                JsonConvert.SerializeObject(new scontivuemodel(), Formatting.Indented, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                }));
        addelements.Add("jsvarfrommasterstart", scriptRegVariables);
        string ret = WelcomeLibrary.UF.custombind.CreaInitStringJavascriptOnly(addelements);
        return ret;
    }


}