using System;
using System.Collections.Generic;
using System.Web.Profile;
using System.Web.Security;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;

namespace WelcomeLibrary.UF
{
    public class usersmem
    {
        public static Dictionary<string, string> _users = null;
        public usersmem()
        {
            if (_users == null) _users = new Dictionary<string, string>();
            MembershipUserCollection MUColl = Membership.GetAllUsers();
            foreach (MembershipUser user in MUColl)
            {
                string idclente = usermanager.getidcliente(user.UserName);
                string nomecliente = user.UserName;
                if (!string.IsNullOrEmpty(idclente))
                {
                    WelcomeLibrary.DOM.Cliente c = WelcomeLibrary.DAL.ClientiDM.GetNomeClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idclente);
                    nomecliente = (c.Cognome + " " + c.Nome).Trim();
                }
                if (!_users.ContainsKey(user.UserName))
                    _users.Add(user.UserName, nomecliente);
                else
                    _users[user.UserName] = nomecliente;
            }
        }
    }

    /// <summary>
    /// Descrizione di riepilogo per usermanager
    /// </summary>
    public class usermanager
    {
        public usermanager()
        {
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
            if (!string.IsNullOrEmpty(username))
            {
                MembershipUserCollection mucoll = Membership.FindUsersByName(username);
                if (mucoll != null && mucoll.Count > 0)
                {
                    ret = true;
                }
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
                text = passimpostata;
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

        /// <summary>
        /// Cambia la pass per l'utente indicando o la mail o lo username e la invia per email
        /// e manda un mail con i dati per accedere al sito
        /// </summary>
        /// <param name="lingua"></param>
        /// <param name="email"></param>
        /// <param name="idtipocliente"></param>
        /// <param name="mittentenome"></param>
        /// <param name="mittentemail"></param>
        /// <returns></returns>
        public string SendAccessData(string lingua, string emailoruser, string mittentemail, string mittentenome = "", string idtipocliente = "0")
        {
            string ret = "";
            try
            {
                usermanager USM = new usermanager();
                ClientiDM cliDM = new ClientiDM();
                Cliente cliente = new Cliente();
                string username = emailoruser; //Ipotizzo che mi possa passare l'username invece della mail
                string idcliente = getidcliente(username); //prendo l'id anagrafica associato al cliente loggato ( se disponibile )
                cliente = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcliente); //prende il cliente per idcliente con qualsiasi tipologia questo 
                if (cliente == null || cliente.Id_cliente == 0) //se non trovo l'utente con l'username allora lo cerco per email
                    cliente = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, emailoruser, idtipocliente);
                if (cliente != null && cliente.Id_cliente != 0) username = cliente.Id_cliente.ToString() + "-" + cliente.Email;

                if (USM.VerificaPresenzaUtente(username) && cliente != null && cliente.Id_cliente != 0)  //Cliente esistente ed utente esistente
                {
                    string newpassword = WelcomeLibrary.UF.RandomPassword.Generate(8);
                    MembershipUser utente = Membership.GetUser(username, false);
                    if (utente != null)
                    {
                        string resetpass = utente.ResetPassword();
                        if (utente.ChangePassword(resetpass, newpassword))
                        {
                            string SoggettoMail = " Mail invio dati da " + mittentenome;
                            //Dati per la mail
                            string nomecliente = cliente.Cognome + " " + cliente.Nome;
                            string Mailcliente = cliente.Email;
                            string Descrizione = "Password set for " + username + "<br/>";
                            Descrizione += newpassword + "<br/><br/>";

                            Descrizione += references.ResMan("Common", lingua, "forgetResponse1").ToString() + "<br/><br/>";
                            Utility.invioMailGenerico(mittentenome, mittentemail, SoggettoMail, Descrizione, Mailcliente, nomecliente);
                            ret = references.ResMan("Common", lingua, "forgetResponse2");
                        }
                    }

                }
                else
                    ret = references.ResMan("Common", lingua, "forgetResponse3");
            }
            catch (Exception err)
            {
                ret = references.ResMan("Common", lingua, "forgetResponse3") + "<br/>" + err.Message;
            }
            return ret;
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
                {
                    Membership.CreateUser(username, password);
                    //associamo l'utente al ruolo
                    Roles.AddUserToRole(username, role);
                    //ProfileCommon prof = (ProfileCommon)ProfileCommon.Create(username);
                    ProfileBase prof = ProfileBase.Create(username);
                    prof["IdCliente"] = idassociato;
                    prof.Save();
                    //password = "User: " + username + " Psw: " + password;
                    esito = true;
                }
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

        public string GetUsernamebycamporofilo(string field, string value)
        {
            string retval = "";
            //ELIMINIAMO GLI UTENTI CHE NON DOBBIAMO VISUALIZZARE in base all'agenzia
            MembershipUserCollection MUColl = Membership.GetAllUsers();
            // MembershipUserCollection MUColl_filtrata = new MembershipUserCollection();

            foreach (MembershipUser _user in MUColl)
            {
                DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
                if (_user != null)
                {
                    UserLastActivityDate = _user.LastActivityDate;
                }
                //ProfileCommon prf = (ProfileCommon)ProfileCommon.Create(_user.UserName);
                ProfileBase prf = ProfileBase.Create(_user.UserName);
                string valueselected = prf[field].ToString();
                if (value == valueselected)
                { retval = _user.UserName; }
                // need to reset the UserLastActivityDate that has just been updated by above two lines
                if (_user != null)
                {
                    _user.LastActivityDate = UserLastActivityDate;
                    Membership.UpdateUser(_user);
                }
            }

            //MUColl_filtrata = MUColl;

            return retval;

        }


        public static string getidsocio(string utente)
        {
            //  save UserLastActivityDate so it can be reassigned later
            MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
            DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
            if (_user != null)
                UserLastActivityDate = _user.LastActivityDate;

            ProfileBase profile = ProfileBase.Create(utente);
            string idsocio = (string)profile["IdSocio"];

            // need to reset the UserLastActivityDate that has just been updated by above two lines
            if (_user != null)
            {
                _user.LastActivityDate = UserLastActivityDate;
                Membership.UpdateUser(_user);
            }
            return idsocio;

        }

        public static string getFullNameFromStatic(string utente)
        {
            string ret = "";
            if (usersmem._users == null || !usersmem._users.ContainsKey(utente)) new usersmem();
            else
            {
                ret = usersmem._users[utente];
            }
            return ret;
        }
        public static string getFullName(string utente)
        {
            //  save UserLastActivityDate so it can be reassigned later
            MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
            DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
            if (_user != null)
                UserLastActivityDate = _user.LastActivityDate;

            ProfileBase profile = ProfileBase.Create(utente);
            string fullname = (string)profile["LastName"];
            fullname += " " + (string)profile["FirstName"];

            // need to reset the UserLastActivityDate that has just been updated by above two lines
            if (_user != null)
            {
                _user.LastActivityDate = UserLastActivityDate;
                Membership.UpdateUser(_user);
            }
            return fullname;

        }
        public static string getFirstName(string utente)
        {
            //  save UserLastActivityDate so it can be reassigned later
            MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
            DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
            if (_user != null)
                UserLastActivityDate = _user.LastActivityDate;

            ProfileBase profile = ProfileBase.Create(utente);
            string nome = (string)profile["FirstName"];

            // need to reset the UserLastActivityDate that has just been updated by above two lines
            if (_user != null)
            {
                _user.LastActivityDate = UserLastActivityDate;
                Membership.UpdateUser(_user);
            }
            return nome;

        }
        public static string setFirstName(string utente, string nome)
        {
            //  save UserLastActivityDate so it can be reassigned later
            MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
            DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
            if (_user != null)
                UserLastActivityDate = _user.LastActivityDate;

            ProfileBase profile = ProfileBase.Create(utente);
            profile["FirstName"] = nome;
            profile.Save();

            // need to reset the UserLastActivityDate that has just been updated by above two lines
            if (_user != null)
            {
                _user.LastActivityDate = UserLastActivityDate;
                Membership.UpdateUser(_user);
            }
            return nome;

        }
        /// <summary>
        /// torna l'dicliente associato all'utente nel profilo
        /// </summary>
        /// <param name="utente"></param>
        /// <returns></returns>
        public static string getidcliente(string utente)
        {
            //  save UserLastActivityDate so it can be reassigned later
            MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
            DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
            if (_user != null)
            {
                UserLastActivityDate = _user.LastActivityDate;
            }

            //ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
            ProfileBase profile = ProfileBase.Create(utente);
            string idCliente = "";
            if (profile != null)
                idCliente = (string)profile["IdCliente"];

            // need to reset the UserLastActivityDate that has just been updated by above two lines
            if (_user != null)
            {
                _user.LastActivityDate = UserLastActivityDate;
                Membership.UpdateUser(_user);
            }
            return idCliente;
        }
        public static string getmailuser(string utente)
        {
            //  save UserLastActivityDate so it can be reassigned later
            MembershipUser _user = Membership.GetUser(utente, false); //Prendo i dati utente senza modificare la lastactivitydate
            DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
            if (_user != null)
            {
                UserLastActivityDate = _user.LastActivityDate;
            }

            //ProfileCommon prf = (ProfileCommon)Profile.GetProfile(utente);
            ProfileBase profile = ProfileBase.Create(utente);
            string emailuser = (string)profile["EMail"];

            // need to reset the UserLastActivityDate that has just been updated by above two lines
            if (_user != null)
            {
                _user.LastActivityDate = UserLastActivityDate;
                Membership.UpdateUser(_user);
            }
            return emailuser;
        }
    }

}