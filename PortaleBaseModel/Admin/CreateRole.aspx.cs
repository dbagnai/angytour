using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admin_CreateRole : System.Web.UI.Page
{
     
    protected void Page_Load(object sender, EventArgs e)
    {
        ////CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
        //bool generale = false;
        //foreach (string role in Roles.GetRolesForUser(User.Identity.Name))
        //{
        //    if (role.ToString() == "GeneralAdmin") generale = true;
        //}

        //if (!generale) Response.Redirect("~/index.aspx?Error=Accesso Sonsentito Solo ad Amministratori Generali");


        if (!IsPostBack)
        {
            RolesList.DataSource = this.GetRuoliFiltrati();
            RolesList.DataBind();
        }
    }

    protected ArrayList GetRuoliFiltrati()
    {
        ArrayList ruoli = new ArrayList();

        ////CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
        //bool generale = false;
        //foreach (string role in Roles.GetRolesForUser(User.Identity.Name))
        //{
        //    if (role.ToString() == "GeneralAdmin") generale = true;
        //}
        //if (generale == false)
        //{
        //    //RIMUOVIAMO IL RUOLO DI GESTORE GENERALE AGENZIE

        //    foreach (string ruolo in Roles.GetAllRoles())
        //    {
        //        if (ruolo != "GeneralAdmin") ruoli.Add(ruolo);
        //    }
        //}
        //else
        //{
            foreach (string ruolo in Roles.GetAllRoles())
            {
                ruoli.Add(ruolo);
            }

        //}

        return ruoli;
    }


    protected void Button1_Click(object sender, EventArgs e)
    {

        try
        {
         //   if (RoleName.Text.ToLower() == "generaladmin") throw (new ApplicationException("Creazione ruolo non consentita per l'utente"));
            Roles.CreateRole(RoleName.Text);
            Results.Text = "Ruolo creato!";
        }
        catch (Exception error)
        {
            Results.Text = error.Message;
        }
        finally
        {
            RolesList.DataSource = this.GetRuoliFiltrati();
            RolesList.DataBind();
        }

      
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            //if (RolesList.SelectedValue.ToLower() == "generaladmin") throw (new ApplicationException("Cancellazione ruolo non consentita "));
         
            Roles.DeleteRole(RolesList.SelectedValue);
            Results.Text = "Ruolo eliminato!";
        }
        catch (Exception error)
        {
            Results.Text = error.Message;
        }
        finally
        {
            RolesList.DataSource = this.GetRuoliFiltrati();
            RolesList.DataBind();
        }
    }
}
