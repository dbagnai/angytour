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


public partial class admin_Default : CommonPage
{

    /// <summary>
    /// RESTITUISCE GLI UTENTI FILTRATI IN BASE AL RUOLO DELL OPERATORE
    /// GeneralAdmin o AgencyAdmin ....
    /// AgencyAdmin potrà vedere solo gli utenti della
    /// propria agenzia!!! E non potrà gestire il ruolo GeneralAdmin
    /// </summary>
    /// <returns></returns>
    protected MembershipUserCollection GetListaUtentiFiltrata()
    {
        //CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
        //bool generale = false;
        //foreach (string role in Roles.GetRolesForUser(User.Identity.Name))
        //{
        //    if (role.ToString() == "GeneralAdmin") generale = true;
        //}
        //VEDIAMO L'AGENZIA PER L'UTENTE LOGGATO
        //string Agenzia = this.getagenzia(User.Identity.Name);
        //ELIMINIAMO GLI UTENTI CHE NON DOBBIAMO VISUALIZZARE in base all'agenzia
        MembershipUserCollection MUColl = Membership.GetAllUsers();
        //Membership.UserIsOnlineTimeWindow;
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


    protected void Page_PreInit(object sender, EventArgs e)
    {
    
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //UsersList.DataSource = this.GetListaUtentiFiltrataCustom();
            UsersList.DataSource = this.GetListaUtentiFiltrata();

            //AL momento del bind -> viene aggiornata la lastactivitydate per gli utenti
            UsersList.DataBind();
        }

    }

    protected string isonline(string idutente)
    {
        MembershipUser _user = Membership.GetUser(idutente, false); //Prendo i dati utente senza modificare la lastactivitydate
       
        //utente.LastActivityDate = DateTime.Now.AddMinutes(-40); //Reimposto la last activity a prima della getall
        //Membership.UpdateUser(utente);
        return _user.IsOnline.ToString();
    }

    //protected string getagenzia(string utente)
    //{
    //    //  save UserLastActivityDate so it can be reassigned later
    //    MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
    //    DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
    //    if (_user != null)
    //    {
    //         UserLastActivityDate = _user.LastActivityDate;
    //    }

    //    //Accedendo alle proprietà del profilo utente viene aggiornata la lastactivitydate per l'utente
    //    ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
    //    string agenziautente = prf.Agenzia;

    //    // need to reset the UserLastActivityDate that has just been updated by above two lines
    //    if (_user != null)
    //    {
    //        _user.LastActivityDate = UserLastActivityDate;
    //        Membership.UpdateUser(_user);
    //    }

    //    return agenziautente;

    //}
    protected string getnome(string utente)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900,1,1);
        if (_user != null)
        {
            UserLastActivityDate = _user.LastActivityDate;
        }

        ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
        string nomeutente = prf.FirstName + " " + prf.LastName;

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }
        return nomeutente;

    }
    protected string getEmail(string utente)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
        {
             UserLastActivityDate = _user.LastActivityDate;
        }
        ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
        string email = prf.EMail;

        if (_user != null)
        {
            // need to reset the UserLastActivityDate that has just been updated by above two lines
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }

        return email;

    }
    protected void UsersList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        //UsersList.DataSource = this.GetListaUtentiFiltrataCustom();
        UsersList.DataSource = this.GetListaUtentiFiltrata();
        UsersList.PageIndex = e.NewPageIndex;
        UsersList.DataBind();
    }
}
