using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

public partial class AspNetPages_ordine_form : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string idordine = "";
            if (Request.QueryString["tran"] != null && Request.QueryString["tran"] != "")
            { idordine = Request.QueryString["tran"].ToString(); }
            // idordine = Page.Request["tran"]; //Questo in caso di server.transer
            idordine = dataManagement.DecodeFromBase64(idordine);

            //Cliente cliente = (Cliente)Session["cliente_" + idordine];
            //Session.Remove("cliente_" + idordine);
            //CarrelloCollection prodotti = (CarrelloCollection)Session["ordineid_" + idordine];
            //Session.Remove("ordineid_" + idordine);
            RemotePost remotepost = (RemotePost)Session["form_" + idordine];
            Session.Remove("form_" + idordine);

            this.payway.Action = remotepost.Url; //URL DELLA CHIAMATA
            this.payway.InnerHtml = remotepost.DataForm; //CAMPI HIDDEN CON TUTTI I DATI
        }
    }
}