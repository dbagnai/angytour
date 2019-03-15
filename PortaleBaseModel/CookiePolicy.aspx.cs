using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CookiePolicy : CommonPage
{
   public string Lingua
   {
      get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : CommonPage.deflanguage; }
      set { ViewState["Lingua"] = value; }
   }

   protected void Page_Load(object sender, EventArgs e)
    {
      CommonPage CommonPage = new CommonPage();
      Lingua = CommonPage.CaricaValoreMaster(Request, Session, "Lingua", false, CommonPage.deflanguage);
        DataBind();
    }
}