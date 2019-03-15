using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AreaContenuti_ResourcePage : CommonPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetCulture("it"); //forzo la cultura italia

    }
    protected void reset_Click(object sender, EventArgs e)
	 {
		 System.Web.HttpRuntime.UnloadAppDomain();
	 }

}