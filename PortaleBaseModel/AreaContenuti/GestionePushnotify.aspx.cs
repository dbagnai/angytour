using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;

public partial class AreaContenuti_GestionePushnotify : CommonPage
{

    public string PercorsoComune
    {
        get { return ViewState["PercorsoComune"] != null ? (string)(ViewState["PercorsoComune"]) : ""; }
        set { ViewState["PercorsoComune"] = value; }
    }
    public string PercorsoFiles
    {
        get { return ViewState["PercorsoFiles"] != null ? (string)(ViewState["PercorsoFiles"]) : ""; }
        set { ViewState["PercorsoFiles"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
        PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;
 

    }
}