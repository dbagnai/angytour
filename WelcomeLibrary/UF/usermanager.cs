﻿using System;
using System.Collections.Generic;
using System.Web.Profile;
using System.Web.Security;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Linq;
using System.Data.SQLite;

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
                bool flagrole = false;
                foreach (string role in Roles.GetRolesForUser(user.UserName))
                {
                    if (role.ToString() == "Autore") flagrole = true;
                }
                if (flagrole)
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

                    new usersmem(); //aggiorno la lista utenti in memoria degli autori!

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

        /// <summary>
        /// fUNZIONE DI SPLIT DEI VALORI STIRNGA DEL profile provider passando 
        /// </summary>
        /// <param name="names">array split : stringe del campo propertynames</param>
        /// <param name="values">stringa property values</param>
        /// <param name="outvalcollection"></param>
        private Dictionary<string, string> ParseDataFromProfileDb(string[] names, string values)
        {
            Dictionary<string, string> namevaluescollection = new Dictionary<string, string>();
            if (names == null || values == null)
                return namevaluescollection;

            for (int iter = 0; iter < names.Length / 4; iter++)
            {
                string name = names[iter * 4].ToLowerInvariant();

                if (!namevaluescollection.ContainsKey(name)) namevaluescollection.Add(name, "");

                int startPos = Int32.Parse(names[iter * 4 + 2], System.Globalization.CultureInfo.InvariantCulture);
                int length = Int32.Parse(names[iter * 4 + 3], System.Globalization.CultureInfo.InvariantCulture);

                if (names[iter * 4 + 1] == "S" && startPos >= 0 && length > 0 && values.Length >= startPos + length)
                {
                    namevaluescollection[name] = values.Substring(startPos, length);
                }
            }
            return namevaluescollection;
        }
        private Dictionary<string, Dictionary<string, string>> GetPropertyValuesFromDatabase(string username, string propname, string propvalue)
        {
            string[] names = null;
            string values = null;
            //byte[] buffer = null;
            Dictionary<string, Dictionary<string, string>> namevaluesforuser = new Dictionary<string, Dictionary<string, string>>();

            string query = "SELECT Username,PropertyNames, PropertyValuesString FROM aspnet_Profile P left join aspnet_Users U on P.UserId=U.UserId   ";
            query += "";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();

            if (username != "")
            {
                SQLiteParameter p2 = new SQLiteParameter("@Username", username.ToLowerInvariant()); //OleDbType.VarChar
                parColl.Add(p2);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE  LoweredUsername=@Username";
                else
                    query += " AND  LoweredUsername=@Username  ";
            }


            try
            {
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, "dbmembership");
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        if (!reader["PropertyNames"].Equals(DBNull.Value))
                            names = reader.GetString(reader.GetOrdinal("PropertyNames")).Split(':');
                        if (!reader["PropertyValuesString"].Equals(DBNull.Value))
                            values = reader.GetString(reader.GetOrdinal("PropertyValuesString"));

                        string usernametmp = "";
                        if (!reader["Username"].Equals(DBNull.Value))
                            usernametmp = reader.GetString(reader.GetOrdinal("Username"));
                        if (names != null && names.Length > 0 && !string.IsNullOrEmpty(usernametmp))
                        {
                            Dictionary<string, string> namevaluescollection = ParseDataFromProfileDb(names, values);
                            //namevaluesforuser

                            if (namevaluescollection.Count > 0)
                            {
                                if (!namevaluesforuser.ContainsKey(usernametmp))
                                    namevaluesforuser.Add(usernametmp, namevaluescollection);

                                //Se presente filtro idcliente e non specificat username controllo se coincide col filtro il valore della proprieta
                                if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(propname) && !string.IsNullOrEmpty(propvalue))
                                    if (namevaluescollection.ContainsKey(propname.ToLowerInvariant()))
                                    {
                                        string valuechk = namevaluescollection[propname.ToLowerInvariant()];
                                        if (valuechk != propvalue) namevaluesforuser.Remove(usernametmp);
                                        else break; //stop trovato cliente del filtro
                                    }
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore Caricamento Cliente :" + error.Message, error);
            }
            return namevaluesforuser;
            /// <summary>
            /// ////////////////////////////////////
            /// </summary>
#if false
            SQLiteConnection cn = GetDbConnectionForProfile();
            try
            {
                using (SQLiteCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT UserId FROM " + USER_TB_NAME + " WHERE LoweredUsername = $UserName AND ApplicationId = $ApplicationId";
                    cmd.Parameters.AddWithValue("$UserName", username.ToLowerInvariant());
                    cmd.Parameters.AddWithValue("$ApplicationId", _membershipApplicationId);
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    string userId = cmd.ExecuteScalar() as string;

                    if (userId != null)
                    {
                        // User exists?
                        cmd.CommandText = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM " + PROFILE_TB_NAME + " WHERE UserId = $UserId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("$UserId", userId);


                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                names = dr.GetString(0).Split(':');
                                values = dr.GetString(1);
                                int length = (int)dr.GetBytes(2, 0L, null, 0, 0);
                                buffer = new byte[length];
                                dr.GetBytes(2, 0L, buffer, 0, length);
                            }
                        }

                        cmd.CommandText = "UPDATE " + USER_TB_NAME + " SET LastActivityDate = $LastActivityDate WHERE UserId = $UserId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("$LastActivityDate", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("$UserId", userId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                if (!IsTransactionInProgress())
                    cn.Dispose();
            }

            if (names != null && names.Length > 0)
            {
                ParseDataFromDb(names, values, buffer, svc);
            } 
#endif

        }

        public string GetUsernamebycamporofilo(string field, string value)
        {
            string retval = "";
            if (string.IsNullOrEmpty(value) || value == "0") return retval;
            //ELIMINIAMO GLI UTENTI CHE NON DOBBIAMO VISUALIZZARE in base all'agenzia
            MembershipUserCollection MUColl = Membership.GetAllUsers();

            ////////////////////////////////////////////////////////////////
            //METODO DI RICERCA DIRETTTA IN TABELLA PROFILE IN BASE
            ////a Propertynaem/èrpertyvlues con id cliente-> predno userid e dalla tabella user prendo l'username
            ////////////////////////////////////////////////////////////////
            Dictionary<string, Dictionary<string, string>> namevaluesforuser = GetPropertyValuesFromDatabase("", field, value);
            if (namevaluesforuser != null && namevaluesforuser.Count == 1) { var firstElement = namevaluesforuser.FirstOrDefault(); retval = firstElement.Key; }

#if false
            /////////////////////////VERSIONE VELOCE CHE USA IL NOME UTENTE + ID CLIENTE PER TROVARE L'UNTENTE ... NON IL MASSIMO /////
            var user = Membership.GetAllUsers().Cast<MembershipUser>().FirstOrDefault(m => m.UserName.StartsWith(value + "-"));
            ProfileBase prf = ProfileBase.Create("");
            if (user != null)
            {
                DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
                if (user != null)
                {
                    UserLastActivityDate = user.LastActivityDate;
                }
                prf = ProfileBase.Create(user.UserName);
                if (value == prf[field].ToString())
                {
                    retval = user.UserName;
                    // need to reset the UserLastActivityDate that has just been updated by above two lines
                    if (user != null)
                    {
                        user.LastActivityDate = UserLastActivityDate;
                        Membership.UpdateUser(user);
                    }
                    return retval;
                }
            } 
#endif
#if false
            //VERSIONE LENTA CHE SCORRE SEMPRE TUTTI I PROFILI MA FUNZIONA ANCHE PER NOMI CHE NON INIZIANO CON IDCLIENTE-EMAIL
            foreach (MembershipUser _user in MUColl)
            {

                DateTime UserLastActivityDate = new DateTime(1900, 1, 1);
                if (_user != null)
                {
                    UserLastActivityDate = _user.LastActivityDate;
                }
                prf = ProfileBase.Create(_user.UserName);
                if (value == prf[field].ToString())
                {
                    retval = _user.UserName;
                    // need to reset the UserLastActivityDate that has just been updated by above two lines
                    if (_user != null)
                    {
                        _user.LastActivityDate = UserLastActivityDate;
                        Membership.UpdateUser(_user);
                    }
                    break;
                }

                // need to reset the UserLastActivityDate that has just been updated by above two lines
                if (_user != null)
                {
                    _user.LastActivityDate = UserLastActivityDate;
                    Membership.UpdateUser(_user);
                }
            }
#endif

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

        /// <summary>
        /// Al momento usata solo pe ri nomi degli autori!!!
        /// </summary>
        /// <param name="utente"></param>
        /// <returns></returns>
        public static string getFullNameFromStatic(string utente)
        {
            string ret = "";
            //NON posso caricare la lista utenti a tutti i giri se un nome non è presente altrimenti avrei troppi caricamenti se il nome non è nel database!!!
            //if (usersmem._users == null || !usersmem._users.ContainsKey(utente)) new usersmem();
            if (usersmem._users == null) new usersmem(); //la carico una solo volta la lista solo se vuota!, se non è presente il nome evito senno a tutte le chiamate se il nome viene elimintato mi ricarica tutto e  se glu users sono molti si rallenta tantissimo ( è usata solo per gli autori!!! e se aggiungo un autore basta richiamare una volta il metodo costruttore  new usersmem() al momento dell'aggiunta o far il restart dell'application
            if (usersmem._users.ContainsKey(utente))
                ret = usersmem._users[utente];
            else ret = utente;
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