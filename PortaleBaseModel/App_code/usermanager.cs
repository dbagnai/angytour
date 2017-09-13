using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

/// <summary>
/// Descrizione di riepilogo per usermanager
/// </summary>
public class usermanager
{
    public usermanager()
    {
        //
        // TODO: aggiungere qui la logica del costruttore
        //
    }


    public bool EliminaUtentebyUsername(string username)
    {
        bool esito = true;
        string text = "";
        try
        {
            Membership.DeleteUser(username);
        }
        catch (Exception error)
        {
            text += error.Message;
            if (error.InnerException != null)
                text += error.InnerException.Message.ToString();
            esito = false;
        }
        return esito;
    }
    public bool VerificaPresenzaUtente(string username)
    {
        bool ret = false;
        MembershipUserCollection mucoll = Membership.FindUsersByName(username);
        if (mucoll != null && mucoll.Count > 0)
        {
            ret = true;
        }
        return ret;
    }
    public bool ControllaRuolo(string username, string verificaruolo)
    {
        bool flag = false;
        foreach (string role in Roles.GetRolesForUser(username))
        {
            if (role.ToString() == verificaruolo) flag = true;
        }
        return flag;
    }

    public string Cambiopassword(string username, string oldpass, string newpass)
    {
        string text = "";
        try
        {
            MembershipUser utente = Membership.GetUser(username, false);
            if (utente != null)
                if (utente.ChangePassword(oldpass, newpass))
                    text = "Password Cambiata";
                else
                    text = "Errore cambio password";
        }
        catch (Exception errore)
        {
            text = errore.Message;
        }
        return text;
    }
    public string Resetpassword(string username)
    {
        string text = "";
        try
        {
            MembershipUser utente = Membership.GetUser(username, false);
            string passimpostata = utente.ResetPassword();
            text = "La nuova password automatica è:  " + passimpostata;

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
            text = errore.Message;
        }
        return text;
    }

    public bool CreaUtente(string idassociato, ref string username, ref string password, string role = "Operatore")
    {
        bool esito = false;
        try
        {
            //Generiamo la password di accesso
            //password = Membership.GeneratePassword(6, 0);
            if (string.IsNullOrEmpty(password))
                password = WelcomeLibrary.UF.RandomPassword.Generate(8);
            if (!string.IsNullOrEmpty(username))
                Membership.CreateUser(username, password);
            //associamo l'utente al ruolo
             Roles.AddUserToRole(username, role);
            ProfileCommon prof = (ProfileCommon)ProfileCommon.Create(username);
            prof.IdCliente = idassociato;
            prof.Save();
            password = "User: "+ username +" Psw: " + password;
            esito = true;
        }
        catch (Exception error)
        {
            esito = false;
            password += error.Message;
            if (error.InnerException != null)
                password += error.InnerException.Message.ToString();
        }
        return esito;
    }
    

}