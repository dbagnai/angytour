using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;

public partial class error : CommonPage
{
    public  string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string Errore
    {
        get { return ViewState["Error"] != null ? (string)(ViewState["Error"]) : ""; }
        set { ViewState["Error"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                Errore = CaricaValoreMaster(Request, Session, "Error", false, "");
                //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "vuoto", false, Lingua);
                output.Text = Errore;
            }
            ((HtmlMeta)Master.FindControl("metaRobots")).Content = "noindex,nofollow";
        }
        catch (Exception err)
        {
            //   output.Text = err.Message;
        }
    }

 
}