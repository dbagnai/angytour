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

public partial class admin_create : System.Web.UI.Page
{
    /// <summary>
    /// RESTITUISCE GLI UTENTI FILTRATI IN BASE AL RUOLO DELL OPERATORE
    /// GeneralAdmin o AgencyAdmin ....
    /// AgencyAdmin potr� vedere solo gli utenti della
    /// propria agenzia!!! E non potr� gestire il ruolo GeneralAdmin
    /// </summary>
    /// <returns></returns>
    protected MembershipUserCollection GetListaUtentiFiltrata()
    {


        ////CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
        //bool generale = false;
        //foreach (string role in Roles.GetRolesForUser(User.Identity.Name))
        //{
        //    if (role.ToString() == "GeneralAdmin") generale = true;
        //}
        ////VEDIAMO L'AGENZIA PER L'UTENTE LOGGATO
        //string Agenzia = this.getagenzia(User.Identity.Name);

        //ELIMINIAMO GLI UTENTI CHE NON DOBBIAMO VISUALIZZARE in base all'agenzia
        MembershipUserCollection MUColl = Membership.GetAllUsers();
        MembershipUserCollection MUColl_filtrata = new MembershipUserCollection();
        //if (generale == false)
        //{
        //    foreach (MembershipUser user in MUColl)
        //    {
        //        //PRENDO SOLO GLI UTENTI DELL'AGENZIA DI PERTINENZA
        //        if (this.getagenzia(user.UserName) == Agenzia) MUColl_filtrata.Add(user);
        //    }
        //}
        //else
        MUColl_filtrata = MUColl;

        return MUColl_filtrata;

    }
    protected string getagenzia(string utente)
    {
        string agenzia = "";
        //save UserLastActivityDate so it can be reassigned later
        //MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        //DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        //if (_user != null)
        //{
        //    UserLastActivityDate = _user.LastActivityDate;
        //}

        //ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
        //string agenzia = prf.Agenzia;

        //// need to reset the UserLastActivityDate that has just been updated by above two lines
        //if (_user != null)
        //{
        //    _user.LastActivityDate = UserLastActivityDate;
        //    Membership.UpdateUser(_user);
        //}

        return agenzia;

    }
    //protected ArrayList GetRuoliFiltrati()
    //{
    //    ArrayList ruoli = new ArrayList();

    //    //CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
    //    bool generale = false;
    //    foreach (string role in Roles.GetRolesForUser(User.Identity.Name))
    //    {
    //        if (role.ToString() == "GeneralAdmin") generale = true;
    //    }
    //    if (generale == false)
    //    {
    //        //RIMUOVIAMO IL RUOLO DI GESTORE GENERALE AGENZIE

    //        foreach (string ruolo in Roles.GetAllRoles())
    //        {
    //            if (ruolo != "GeneralAdmin") ruoli.Add(ruolo);
    //        }
    //    }
    //    else
    //    {
    //        foreach (string ruolo in Roles.GetAllRoles())
    //        {
    //            ruoli.Add(ruolo);
    //        }

    //    }

    //    return ruoli;
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        //CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
        //bool generale = false;
        //foreach (string role in Roles.GetRolesForUser(User.Identity.Name))
        //{
        //    if (role.ToString() == "GeneralAdmin") generale = true;
        //}

        //if (!generale) Response.Redirect("~/index.aspx?Error=Accesso Sonsentito Solo ad Amministratori Generali");

        if (!IsPostBack)
        {

            UsersList.DataSource = this.GetListaUtentiFiltrata();
            UsersList.DataBind();

            //RolesList.DataSource = Roles.GetAllRoles();
            //RolesList.DataSource = this.GetRuoliFiltrati();
            //RolesList.DataBind();

        }
        else { }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (UsersList.SelectedValue.ToLower() != "webmaster" && UsersList.SelectedValue.ToLower() != "danieleb")
        {
            Membership.DeleteUser(UsersList.SelectedValue);
            lbl_results.Text = "Utente eliminato!";

            UsersList.DataSource = this.GetListaUtentiFiltrata();
            UsersList.DataBind();
        }
    }
    protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
    {

        //if (RolesList.SelectedValue == "")
        //{
        //    Results.Text = "Errore: utente non creato selezionare Ruolo!";
        //    e.Cancel = true;
        //}

    }
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {

        //try
        //{
        //    Roles.AddUserToRole(((CreateUserWizard)sender).UserName, RolesList.SelectedValue);
        //    Results.Text = "Aggiunto a ruolo!";

        //    //Aggiungiamo AL PROFILO l'agenzia per l'operatore creato in caso di supervisore di agenzia
        //    //CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
        //    bool generale = false;
        //    foreach (string role in Roles.GetRolesForUser(User.Identity.Name))
        //    {
        //        if (role.ToString() == "GeneralAdmin") generale = true;
        //    }
        //    //VEDIAMO L'AGENZIA PER L'UTENTE LOGGATO
        //    string Agenzia = this.getagenzia(User.Identity.Name);
        //    if (generale == false)
        //    {
        //        ProfileCommon prof = (ProfileCommon)ProfileCommon.Create(((CreateUserWizard)sender).UserName);
        //        prof.Agenzia = Agenzia;
        //        prof.Save();
        //    }

        //}
        //catch (Exception error)
        //{
        //    Results.Text = error.Message;
        //}
        //finally
        //{
        //    UsersList.DataSource = this.GetListaUtentiFiltrata();
        //    UsersList.DataBind();
        //    RolesList.DataSource = this.GetRuoliFiltrati();
        //    RolesList.DataBind();

        //    // Need to set the lastactivitydate so the user is not considered online
        //    MembershipUser _user = Membership.GetUser(((CreateUserWizard)sender).UserName, false); //Prendo i dati utente senza modificare la lastactivitydate
        //    if (_user != null)
        //    {
        //        _user.LastActivityDate = DateTime.Now.AddMinutes(-Membership.UserIsOnlineTimeWindow);
        //        Membership.UpdateUser(_user);
        //    }
        //}

    }
    protected void ContinueButton_Click(object sender, EventArgs e)
    {

        CreateUserWizard1.ActiveStepIndex = 0;
        CreateUserWizard1.ContinueDestinationPageUrl = "~/Admin/CreateUser.aspx";
        UsersList.DataSource = this.GetListaUtentiFiltrata();
        UsersList.DataBind();
    }



}
