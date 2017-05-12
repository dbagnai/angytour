using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admin_Profile : CommonPage
{

    protected void reset_Click(object sender, EventArgs e)
    {
        System.Web.HttpRuntime.UnloadAppDomain();
    }

    /// <summary>
    /// RESTITUISCE GLI UTENTI FILTRATI IN BASE AL RUOLO DELL OPERATORE
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
    protected string getidcliente(string utente)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
        {
            UserLastActivityDate = _user.LastActivityDate;
        }

        ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
        string idCliente = prf.IdCliente;

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }
        return idCliente;
    }
    protected string getidsocio(string utente)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
        {
            UserLastActivityDate = _user.LastActivityDate;
        }

        ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
        string idsocio = prf.IdSocio;

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }
        return idsocio;
    }
    protected ArrayList GetRuoliFiltrati()
    {
        ArrayList ruoli = new ArrayList();

        //CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
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

    //protected bool UserIsGeneralAdmin(string username)
    //{
    //    //CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
    //    bool generale = false;
    //    foreach (string role in Roles.GetRolesForUser(username))
    //    {
    //        if (role.ToString() == "GeneralAdmin") generale = true;
    //    }
    //    return generale;
    //}
    //protected bool UserIsAgencyAdmin(string username)
    //{
    //    //CONTROLLIAMO SE L'UTENTE E' UN GESTORE MULTIAGENZIE
    //    bool generale = false;
    //    foreach (string role in Roles.GetRolesForUser(username))
    //    {
    //        if (role.ToString() == "AgencyAdmin") generale = true;
    //    }
    //    return generale;
    //}
    protected void Page_PreInit(object sender, EventArgs e)
    {
       
        //if (Session["Agenzie"] == null)
        //    if (CaricaSession() != true) Response.Redirect("~/index.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
      
            UsersList.DataSource = this.GetListaUtentiFiltrata();
            UsersList.DataBind();

            //  save UserLastActivityDate so it can be reassigned later
            MembershipUser _user = Membership.GetUser(UsersList.SelectedValue, false); //Prendo i dati utente senza modificare la lastactivitydate
            DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
            if (_user != null)
            {
                UserLastActivityDate = _user.LastActivityDate;
            }


            ProfileCommon prof = (ProfileCommon)Profile.GetProfile(UsersList.SelectedValue);
            if (prof != null)
            {
                FirstName.Text = prof.FirstName;
                LastName.Text = prof.LastName;
                EMail.Text = prof.EMail;
                Cellulare.Text = prof.Cellulare;
                txtIdcliente.Text = prof.IdCliente;
                txtIdsocio.Text = prof.IdSocio;
            }

            // need to reset the UserLastActivityDate that has just been updated by above two lines
            if (_user != null)
            {
                _user.LastActivityDate = UserLastActivityDate;
                Membership.UpdateUser(_user);
            }

        }
        else
        {
            Results.Text = "";

        }
    }




    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            MembershipUser utente = Membership.GetUser(UsersList.SelectedValue, false);
            if (utente.ChangePassword(txtPasswordold.Text, txtPasswordnew.Text))
                lblResultsPsw.Text = "Password Cambiata";
            else
                lblResultsPsw.Text = "Errore cambio password";
        }
        catch (Exception errore)
        {
            lblResultsPsw.Text = errore.Message;
        }

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            MembershipUser utente = Membership.GetUser(UsersList.SelectedValue, false);
            string passimpostata = utente.ResetPassword();
            lblResultsPsw.Text = "La nuova password automatica è:  " + passimpostata + "      Copiare la password da qualche parte!!";
            lblquestion.Text = "Attenzione !! Premendo il pulsante Reset password viene generata una nuova password per l'utente, che invaliderà la precedente.";

            //Procedura con requires question and aswer
#if false
        if (txtanswer.Text != "")
        {
            string passimpostata = utente.ResetPassword(txtanswer.Text);
            lblResultsPsw.Text = "La nuova password automatica è:  " + passimpostata + " . Copiare la password da qualche parte!!";
        }
        else
        {
            lblquestion.Text = "Digita nella casella la riposta corretta al seguente quesito: " +  utente.PasswordQuestion + "?. Poi premi reset per resettare la password. ";
            lblResultsPsw.Text = "Password non resettata.  ";
        }
#endif
        }
        catch (Exception errore)
        {
            lblResultsPsw.Text = errore.Message;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(UsersList.SelectedValue, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
        {
            UserLastActivityDate = _user.LastActivityDate;
        }

        try
        {
            ProfileCommon prof = (ProfileCommon)ProfileCommon.Create(UsersList.SelectedValue);

            prof.FirstName = FirstName.Text;
            prof.LastName = LastName.Text;
            prof.EMail = EMail.Text;
            prof.Cellulare = Cellulare.Text;
            prof.IdCliente = txtIdcliente.Text;
            prof.IdSocio = txtIdsocio.Text;

            
            //SALVARE IL PROFILO non dimenticarsene
            prof.Save();

        }
        catch (Exception error)
        {
            Results.Text = error.Message;
        }

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }

    }
    protected void UserList_changed(object sender, EventArgs e)
    {
        //  save UserLastActivityDate so it can be reassigned later
        MembershipUser _user = Membership.GetUser(UsersList.SelectedValue, false); //Prendo i dati utente senza modificare la lastactivitydate
        DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
        if (_user != null)
        {
            UserLastActivityDate = _user.LastActivityDate;
        }

        ProfileCommon prof = (ProfileCommon)Profile.GetProfile(UsersList.SelectedValue);
        if (prof != null)
        {

            FirstName.Text = prof.FirstName;
            LastName.Text = prof.LastName;

            EMail.Text = prof.EMail;
            Cellulare.Text = prof.Cellulare;
            txtIdcliente.Text = prof.IdCliente;
            txtIdsocio.Text = prof.IdSocio;
        }

        // need to reset the UserLastActivityDate that has just been updated by above two lines
        if (_user != null)
        {
            _user.LastActivityDate = UserLastActivityDate;
            Membership.UpdateUser(_user);
        }

    }
    
   
}
