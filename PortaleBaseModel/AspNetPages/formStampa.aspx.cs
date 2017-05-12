using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AspNetPages_formStampa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["datistampa"] != null)
            {
                litContenuti.Text = Session["datistampa"].ToString();
                Session.Remove("datistampa");
            }
        }
    }
}