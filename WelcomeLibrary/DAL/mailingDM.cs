using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;
using System.Data.OleDb;

namespace WelcomeLibrary.DAL
{
    public class mailingDM
    {

        /// <summary>
        /// Carica le email filtrando per tipomailing e prendendo quelle relative agli ID_CARD delle card passati in collection e non in errore
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cards">Se la collection è vuota non ritorna nulla</param>
        /// <param name="tipomailing"></param>
        /// <returns></returns>
        public MailCollection CaricaMailFiltratePerIdcardTipomailing(string connection, CardCollection cards, enumclass.TipoMailing? tipomailing)
        {
            MailCollection list = new MailCollection();
            if (connection == null || connection == "") return list;

            Mail item = new Mail();
            //List<OleDbParameter> parColl = new List<OleDbParameter>();
            //OleDbParameter p1 = new OleDbParameter("@Email", email); //OleDbType.VarChar
            //parColl.Add(p1);
            try
            {
                string query = "";
                query = "SELECT * FROM TBL_MAILING WHERE Errore = false ";
                if (tipomailing != null)
                    query += " and TipoMailing = " + (Int32)tipomailing + " ";
                if (cards != null && cards.Count > 0)
                {
                    query += " and id_card in (  ";
                    foreach (Card card in cards)
                    {
                        query += " " + card.Id_card + " ,";
                        //where id_card in ( 1231 , 444 )
                    }
                    query = query.TrimEnd(',') + " ) ";
                }
                else return list;

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Mail();


                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
                        if (!reader["Lingua"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        if (!reader["NoteInvio"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.NoteInvio = reader.GetString(reader.GetOrdinal("NoteInvio"));
                        if (!reader["SoggettoMail"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.SoggettoMail = reader.GetString(reader.GetOrdinal("SoggettoMail"));
                        if (!reader["TestoErrore"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.TestoErrore = reader.GetString(reader.GetOrdinal("TestoErrore"));
                        if (!reader["TestoMail"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.TestoMail = reader.GetString(reader.GetOrdinal("TestoMail"));
                        item.Tipomailing = reader.GetInt32(reader.GetOrdinal("Tipomailing"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["DataInvio"].Equals(DBNull.Value))
                            item.DataInvio = reader.GetDateTime(reader.GetOrdinal("DataInvio"));
                        if (!reader["DataAdesione"].Equals(DBNull.Value))
                            item.DataAdesione = reader.GetDateTime(reader.GetOrdinal("DataAdesione"));
                        if (!reader["Errore"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.Errore = reader.GetBoolean(reader.GetOrdinal("Errore"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("CaricaMailFiltratePerIdcardTipomailing() - Errore Caricamento mails :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Carica la lista delle email ancora da inviare e non in errore.
        /// Inoltre per sicurezza dovresti marcare le mail caricate in modo che una seconda chiamata alla
        /// funzione prima del tempo di invio non provochi un secondo invio agli stessi indirizzi presi in carico
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public MailCollection CaricaMailDaInviare(string connection, int? MaxEmail)
        {
            MailCollection list = new MailCollection();
            if (connection == null || connection == "") return list;

            Mail item = new Mail();
            //List<OleDbParameter> parColl = new List<OleDbParameter>();
            //OleDbParameter p1 = new OleDbParameter("@Email", email); //OleDbType.VarChar
            //parColl.Add(p1);
            try
            {
                string query = "SELECT";
                if (MaxEmail != null)
                    query += " TOP " + MaxEmail.Value;
                query += " *,C.Email as EmailCliente,C.Nome as Nome,C.Cognome as Cognome,  MC.Id_mail AS mailincharge FROM ( TBL_MAILING M left join TBL_CLIENTI C on M.ID_CLIENTE=C.ID_CLIENTE ) left join TBL_MAILING_ONCHARGE MC on M.ID = MC.Id_mail WHERE (((M.DataInvio) Is Null) AND ((M.Errore)=False) AND ((MC.Id_mail) Is Null)) order by M.ID";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Mail();

                        item.Id = reader.GetInt32(reader.GetOrdinal("M.ID"));
                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("M.ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("M.ID_CARD"));
                        if (!reader["M.Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("M.Lingua"));

                        if (!reader["NoteInvio"].Equals(DBNull.Value))
                            item.NoteInvio = reader.GetString(reader.GetOrdinal("NoteInvio"));
                        if (!reader["SoggettoMail"].Equals(DBNull.Value))
                            item.SoggettoMail = reader.GetString(reader.GetOrdinal("SoggettoMail"));
                        if (!reader["TestoErrore"].Equals(DBNull.Value))
                            item.TestoErrore = reader.GetString(reader.GetOrdinal("TestoErrore"));
                        if (!reader["TestoMail"].Equals(DBNull.Value))
                            item.TestoMail = reader.GetString(reader.GetOrdinal("TestoMail"));
                        item.Tipomailing = reader.GetInt32(reader.GetOrdinal("Tipomailing"));
                        if (!reader["M.DataInserimento"].Equals(DBNull.Value))
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("M.DataInserimento"));
                        if (!reader["DataInvio"].Equals(DBNull.Value))
                            item.DataInvio = reader.GetDateTime(reader.GetOrdinal("DataInvio"));
                        if (!reader["DataAdesione"].Equals(DBNull.Value))
                            item.DataAdesione = reader.GetDateTime(reader.GetOrdinal("DataAdesione"));
                        if (!reader["Errore"].Equals(DBNull.Value))
                            item.Errore = reader.GetBoolean(reader.GetOrdinal("Errore"));
                        if (!reader["ID_mailing_struttura"].Equals(DBNull.Value))
                            item.Id_mailing_struttura = reader.GetInt32(reader.GetOrdinal("ID_mailing_struttura"));

                        if (!reader["EmailCliente"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.Cliente.Email = reader.GetString(reader.GetOrdinal("EmailCliente"));
                        if (!reader["Nome"].Equals(DBNull.Value)) //nome presa dalla tabella clienti
                            item.Cliente.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        if (!reader["Cognome"].Equals(DBNull.Value)) //Cognome presa dalla tabella clienti
                            item.Cliente.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento mails :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Imposta la tabella per le mail prese in carico dal mailer
        /// ( usa l'ordinamento per id e lo stesso max email della routine di caricamento dati per il mailing )
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="idmailcoll"></param>
        /// <param name="MaxEmail"></param>
        /// <returns></returns>
        public int MarcaMailpreseincarico(string connessione, int? MaxEmail)
        {
            int idret = -1;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;
            string query = "";
            //Insert
            //query = "INSERT INTO TBL_MAILING_ONCHARGE ( Id_mail , DataInserimento )";
            //query += " values ( ";
            //query += "@id,@DataInserimento)";
            query = "Insert into TBL_MAILING_ONCHARGE (Id_mail) Select ";
            if (MaxEmail != null)
                query += " top " + MaxEmail.Value.ToString();
            query += " M.id FROM  TBL_MAILING M left join TBL_MAILING_ONCHARGE MC on M.ID = MC.Id_mail WHERE (((M.DataInvio) Is Null) AND ((M.Errore)=False) AND ((MC.Id_mail) Is Null)) order by M.ID";

            /*
           INSERT INTO MyTable (FirstCol, SecondCol)
SELECT 'First' ,1
UNION ALL
SELECT 'Second' ,2
UNION ALL
SELECT 'Third' ,3
UNION ALL
SELECT 'Fourth' ,4
UNION ALL
SELECT 'Fifth' ,5
GO
           * oppure 
           * 
           * INSERT INTO YourTable (FirstCol, SecondCol)
VALUES (‘First’ , 1) , (‘Second’ , 2) , (‘Third’ , ’3′), (‘Fourth’ , ’4′) (‘and so on’)
           * 
           * oppure 
           * Insert into yourtable (table1col, table2col)
Select table1col, table2col
From table1 inner join table2 on table1.table1col = table2.table2col
           */
            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, setta mail prese in carico dal mailer :" + error.Message, error);
            }
            return idret;
        }

        /// <summary>
        /// Elimina dalla tabella delle mail prese in carico la mail con l'id specificato
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="idmail"></param>
        /// <returns></returns>
        public int EliminaMaildapresaincarico(string connessione, int idmail)
        {
            int idret = -1;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_MAILING_ONCHARGE WHERE ( Id_mail = @idmail ) ";
            OleDbParameter p1;
            p1 = new OleDbParameter("@Id_mail", idmail);
            parColl.Add(p1);
            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, eliminazione Mail da presa in carico:" + error.Message, error);
            }
            return idret;
        }

        /// <summary>
        /// Ripulisce dalla tabella dagli id delle email prese in carico che sono più vecchie di una certa data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        public void PulisciTabellaMailPreseincarico(string connection, DateTime data)
        {
            if (connection == null || connection == "") return;
            string query = "DELETE FROM TBL_MAILING_ONCHARGE WHERE ( DataInserimento < @DataInserimento ) ";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter pdmin;
            pdmin = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",data));
            //pdmin.DbType = System.Data.DbType.DateTime;
            parColl.Add(pdmin);

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, cancellazione mail prese in carico vecchie :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Carico dalla tabella mail quelle inviate o da inviare non in errore
        /// con data maggiore di quella indicata riferite all' idnewsletter indicato.
        /// Per vedere i clienti già interessati all'invio di quella newsletter
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idnewsletter"></param>
        /// <param name="mindate"></param>
        /// <returns></returns>
        public MailCollection CaricaMailPeridnewsletterValide(string connection, int idnewsletter, DateTime? mindate)
        {
            MailCollection list = new MailCollection();
            if (connection == null || connection == "") return list;
            if (idnewsletter == 0) return list;

            Mail item = new Mail();

            try
            {

                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@ID_mailing_struttura", idnewsletter); //OleDbType.VarChar
                parColl.Add(p1);
                string query = "SELECT *  FROM TBL_MAILING WHERE Errore = false and ID_mailing_struttura = @ID_mailing_struttura";

                if (mindate != null)
                {
                    OleDbParameter pdmin;
                    pdmin = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",mindate.Value));
                    //pdmin.DbType = System.Data.DbType.DateTime;
                    parColl.Add(pdmin);
                    query += " and DataInserimento > @DataInserimento";
                }

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Mail();

                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));

                        if (!reader["NoteInvio"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.NoteInvio = reader.GetString(reader.GetOrdinal("NoteInvio"));
                        if (!reader["SoggettoMail"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.SoggettoMail = reader.GetString(reader.GetOrdinal("SoggettoMail"));
                        if (!reader["TestoErrore"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.TestoErrore = reader.GetString(reader.GetOrdinal("TestoErrore"));
                        if (!reader["TestoMail"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.TestoMail = reader.GetString(reader.GetOrdinal("TestoMail"));
                        item.Tipomailing = reader.GetInt32(reader.GetOrdinal("Tipomailing"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["DataInvio"].Equals(DBNull.Value))
                            item.DataInvio = reader.GetDateTime(reader.GetOrdinal("DataInvio"));
                        if (!reader["DataAdesione"].Equals(DBNull.Value))
                            item.DataAdesione = reader.GetDateTime(reader.GetOrdinal("DataAdesione"));
                        if (!reader["Errore"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.Errore = reader.GetBoolean(reader.GetOrdinal("Errore"));
                        if (!reader["ID_mailing_struttura"].Equals(DBNull.Value))
                            item.Id_mailing_struttura = reader.GetInt32(reader.GetOrdinal("ID_mailing_struttura"));


                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento mails per idnewsletter :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Carica una singola mail dalla tabella in base all'id
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Mail CaricaMailPerId(string connection, int Id)
        {
            if (connection == null || connection == "") return null;
            if (Id == 0) return null;

            Mail item = new Mail();
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@Id", Id); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT *,C.Email as EmailCliente,C.Nome as Nome,C.Cognome as Cognome FROM TBL_MAILING M left join TBL_CLIENTI C on M.ID_CLIENTE=C.ID_CLIENTE  WHERE Id = @Id ";
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Mail();

                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("M.ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("M.ID_CARD"));
                        if (!reader["M.Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("M.Lingua"));

                        if (!reader["NoteInvio"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.NoteInvio = reader.GetString(reader.GetOrdinal("NoteInvio"));
                        if (!reader["SoggettoMail"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.SoggettoMail = reader.GetString(reader.GetOrdinal("SoggettoMail"));
                        if (!reader["TestoErrore"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.TestoErrore = reader.GetString(reader.GetOrdinal("TestoErrore"));
                        if (!reader["TestoMail"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.TestoMail = reader.GetString(reader.GetOrdinal("TestoMail"));
                        item.Tipomailing = reader.GetInt32(reader.GetOrdinal("Tipomailing"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["DataInvio"].Equals(DBNull.Value))
                            item.DataInvio = reader.GetDateTime(reader.GetOrdinal("DataInvio"));
                        if (!reader["DataAdesione"].Equals(DBNull.Value))
                            item.DataAdesione = reader.GetDateTime(reader.GetOrdinal("DataAdesione"));
                        if (!reader["Errore"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.Errore = reader.GetBoolean(reader.GetOrdinal("Errore"));

                        if (!reader["ID_mailing_struttura"].Equals(DBNull.Value))
                            item.Id_mailing_struttura = reader.GetInt32(reader.GetOrdinal("ID_mailing_struttura"));

                        if (!reader["EmailCliente"].Equals(DBNull.Value)) //Email presa dalla tabella clienti
                            item.Cliente.Email = reader.GetString(reader.GetOrdinal("EmailCliente"));
                        if (!reader["Nome"].Equals(DBNull.Value)) //nome presa dalla tabella clienti
                            item.Cliente.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        if (!reader["Cognome"].Equals(DBNull.Value)) //Cognome presa dalla tabella clienti
                            item.Cliente.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));

                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento mail per id :" + error.Message, error);
            }
            return item;
        }

        /// <summary>
        /// Cancella tutte le mail + vecchie di una certa data!
        /// (Sarebbe da rifare ogni tanto anche l'identty seed per la tabella mail!!!)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        public void CancellaMailPerPulizia(string connection, DateTime data)
        {
            if (connection == null || connection == "") return;
            string query = "DELETE FROM TBL_MAILING WHERE ( DataInserimento < @DataInserimento ) ";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter pdmin;
            pdmin = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",data));
            //pdmin.DbType = System.Data.DbType.DateTime;
            parColl.Add(pdmin);

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione mail vecchie :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Cancella tutte le mail + vecchie di una certa data!
        /// (Sarebbe da rifare ogni tanto anche l'identty seed per la tabella mail!!!)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        public void CancellaMailInAttesa(string connection)
        {
            if (connection == null || connection == "") return;
            string query = "DELETE FROM TBL_MAILING WHERE (((DataInvio) Is Null) AND ((Errore)=False)) ";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            //OleDbParameter pdmin;
            //pdmin = new OleDbParameter("@DataInserimento", data.ToString());
            //pdmin.DbType = System.Data.DbType.DateTime;
            //parColl.Add(pdmin);

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione mail vecchie :" + error.Message, error);
            }
            return;
        }
        /// <summary>
        /// Inserisce o aggiorna una email nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InserisciAggiornaMail(string connessione, Mail item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            OleDbParameter p1 = new OleDbParameter("@Id_card", item.Id_card);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@Id_cliente", item.Id_cliente);//OleDbType.VarChar
            parColl.Add(p2);
            OleDbParameter p3 = new OleDbParameter("@TipoMailing", item.Tipomailing);//OleDbType.VarChar
            parColl.Add(p3);

            OleDbParameter p5 = new OleDbParameter("@TestoMail", item.TestoMail);//OleDbType.VarChar
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@SoggettoMail", item.SoggettoMail);//OleDbType.VarChar
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@NoteInvio", item.NoteInvio);//OleDbType.VarChar
            parColl.Add(p7);

            OleDbParameter p9 = new OleDbParameter("@Lingua", item.Lingua);//OleDbType.VarChar
            parColl.Add(p9);
            OleDbParameter p10 = new OleDbParameter("@Errore", item.Errore);//OleDbType.VarChar
            parColl.Add(p10);
            OleDbParameter p11 = new OleDbParameter("@TestoErrore", item.TestoErrore);//OleDbType.VarChar
            parColl.Add(p11);

            OleDbParameter p4 = null;
            if (item.DataInserimento != null)
                p4 = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.DataInserimento));
            else
                p4 = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",System.DateTime.Now));
            //p4.DbType = System.Data.DbType.DateTime;
            parColl.Add(p4);
            OleDbParameter p8;
            if (item.DataInvio != null)
                p8 = new OleDbParameter("@DataInvio", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.DataInvio.Value));
            else
                p8 = new OleDbParameter("@DataInvio", System.DBNull.Value);
            //p8.DbType = System.Data.DbType.DateTime;
            parColl.Add(p8);

            OleDbParameter pdadesione;
            if (item.DataAdesione != null)
                pdadesione = new OleDbParameter("@DataAdesione", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.DataAdesione.Value));
            else
                pdadesione = new OleDbParameter("@DataAdesione", System.DBNull.Value);
            //pdadesione.DbType = System.Data.DbType.DateTime;
            parColl.Add(pdadesione);

            OleDbParameter pidnewsletter = new OleDbParameter("@ID_mailing_struttura", item.Id_mailing_struttura);//OleDbType.VarChar
            parColl.Add(pidnewsletter);

            string query = "";
            if (item.Id != 0)
            {
                //Update
                query = "UPDATE [TBL_MAILING] SET Id_card=@Id_card,Id_cliente=@Id_cliente,TipoMailing=@TipoMailing,TestoMail=@TestoMail";
                query += ",SoggettoMail=@SoggettoMail,NoteInvio=@NoteInvio,Lingua=@Lingua,Errore=@Errore,TestoErrore=@TestoErrore,DataInserimento=@DataInserimento,DataInvio=@DataInvio,DataAdesione=@DataAdesione,ID_mailing_struttura=@ID_mailing_struttura  ";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_MAILING (Id_card,Id_cliente,TipoMailing,TestoMail";
                query += ",SoggettoMail,NoteInvio,Lingua,Errore,TestoErrore,DataInserimento,DataInvio,DataAdesione,ID_mailing_struttura )";
                query += " values ( ";
                query += "@Id_card,@Id_cliente,@TipoMailing,@TestoMail";
                query += ",@SoggettoMail,@NoteInvio,@Lingua,@Errore,@TestoErrore,@DataInserimento,@DataInvio,@DataAdesione,@ID_mailing_struttura )";
            }

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento Mail :" + error.Message, error);
            }
            return;
        }
        /// <summary>
        /// Inserisce la mail  per il blocco dei clienti indicati nella lista
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        /// <param name="idclienti"></param>
        public void InserisciBloccoMailPerClienti(string connessione, Mail item, ClienteCollection idclienti)
        {
            if (idclienti == null || idclienti.Count == 0) return;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            OleDbParameter p1 = new OleDbParameter("@Id_card", item.Id_card);//OleDbType.VarChar
            parColl.Add(p1);

            OleDbParameter p3 = new OleDbParameter("@TipoMailing", item.Tipomailing);//OleDbType.VarChar
            parColl.Add(p3);

            OleDbParameter p5 = new OleDbParameter("@TestoMail", item.TestoMail);//OleDbType.VarChar
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@SoggettoMail", item.SoggettoMail);//OleDbType.VarChar
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@NoteInvio", item.NoteInvio);//OleDbType.VarChar
            parColl.Add(p7);

            OleDbParameter p9 = new OleDbParameter("@Lingua", item.Lingua);//OleDbType.VarChar
            parColl.Add(p9);
            OleDbParameter p10 = new OleDbParameter("@Errore", item.Errore);//OleDbType.VarChar
            parColl.Add(p10);
            OleDbParameter p11 = new OleDbParameter("@TestoErrore", item.TestoErrore);//OleDbType.VarChar
            parColl.Add(p11);

            OleDbParameter p4 = null;
            if (item.DataInserimento != null)
                p4 = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.DataInserimento));
            else
                p4 = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",System.DateTime.Now));
           // p4.DbType = System.Data.DbType.DateTime;
            parColl.Add(p4);
            OleDbParameter p8;
            if (item.DataInvio != null)
                p8 = new OleDbParameter("@DataInvio", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.DataInvio.Value));
            else
                p8 = new OleDbParameter("@DataInvio", System.DBNull.Value);
            //p8.DbType = System.Data.DbType.DateTime;
            parColl.Add(p8);
            OleDbParameter pdadesione;
            if (item.DataAdesione != null)
                pdadesione = new OleDbParameter("@DataAdesione", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.DataAdesione.Value));
            else
                pdadesione = new OleDbParameter("@DataAdesione", System.DBNull.Value);
            //pdadesione.DbType = System.Data.DbType.DateTime;
            parColl.Add(pdadesione);
            OleDbParameter pidnewsletter = new OleDbParameter("@ID_mailing_struttura", item.Id_mailing_struttura);//OleDbType.VarChar
            parColl.Add(pidnewsletter);

            string query = "";

            //Insert
            query = "INSERT INTO TBL_MAILING (Id_card,Id_cliente,TipoMailing,TestoMail";
            query += ",SoggettoMail,NoteInvio,Lingua,Errore,TestoErrore,DataInserimento,DataInvio,DataAdesione,ID_mailing_struttura )";
            query += "  select @Id_card, tbl_clienti.ID_Cliente, @TipoMailing,@TestoMail";
            query += ",@SoggettoMail,@NoteInvio,@Lingua,@Errore,@TestoErrore,@DataInserimento,@DataInvio,@DataAdesione,@ID_mailing_struttura from tbl_clienti ";
            //query += "  select 0, tbl_clienti.ID_Cliente, '" + gruppo.Campo1 + "'  AS Espr1, '" + gruppo.Data1.ToShortDateString() + "'  AS Espr2, " + gruppo.Intero1 + "  AS Espr4 from tbl_clienti ";

            query += "  where id_cliente in (  ";
            foreach (Cliente c in idclienti)
            {
                query += c.Id_cliente + ",";
            }
            query = query.TrimEnd(',');
            query += " ) ";

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento Mail :" + error.Message, error);
            }
            return;
        }


        /// <summary>
        /// Inserisce o aggiorna i dati di una newsletter nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce.
        /// Ritorna il valore dell'identity della newsletter inserita.
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public int InserisciAggiornaNewsletter(string connessione, Mail item)
        {
            int idret = -1;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;

            OleDbParameter p3 = new OleDbParameter("@TipoMailing", item.Tipomailing);//OleDbType.VarChar
            parColl.Add(p3);
            OleDbParameter p5 = new OleDbParameter("@TestoMail", item.TestoMail);//OleDbType.VarChar
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@SoggettoMail", item.SoggettoMail);//OleDbType.VarChar
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@NoteInvio", item.NoteInvio);//OleDbType.VarChar
            parColl.Add(p7);
            OleDbParameter p9 = new OleDbParameter("@Lingua", item.Lingua);//OleDbType.VarChar
            parColl.Add(p9);

            OleDbParameter p4 = null;
            if (item.DataInserimento != null && item.DataInserimento != DateTime.MinValue)
                p4 = new OleDbParameter("@DataInserimento",String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", item.DataInserimento));
            else
                p4 = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",System.DateTime.Now));
            //p4.DbType = System.Data.DbType.DateTime;
            parColl.Add(p4);

            string query = "";
            if (item.Id != 0)
            {
                //Update
                query = "UPDATE [TBL_MAILING_STRUTTURA] SET TipoMailing=@TipoMailing,TestoMail=@TestoMail";
                query += ",SoggettoMail=@SoggettoMail,NoteInvio=@NoteInvio,Lingua=@Lingua,DataInserimento=@DataInserimento ";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_MAILING_STRUTTURA (TipoMailing,TestoMail";
                query += ",SoggettoMail,NoteInvio,Lingua,DataInserimento )";
                query += " values ( ";
                query += "@TipoMailing,@TestoMail";
                query += ",@SoggettoMail,@NoteInvio,@Lingua,@DataInserimento)";
            }

            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento struttura Mail :" + error.Message, error);
            }
            return idret;
        }


        public void CancellaNewsletterPerId(string connection, int Id)
        {
            if (connection == null || connection == "") return;
            if (Id == 0) return;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@id", Id);//OleDbType.VarChar
            parColl.Add(p1);
            string query = "DELETE FROM TBL_MAILING_STRUTTURA WHERE ([ID]=@id)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, cancellazione newsletter :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Carica una newlsetter dalla tabella della struttura
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Mail CaricaNewsletterPerId(string connection, int Id)
        {
            if (connection == null || connection == "") return null;
            if (Id == 0) return null;

            Mail item = new Mail();
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@id", Id); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT  * FROM TBL_MAILING_STRUTTURA  WHERE Id = @id ";
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Mail();
                        //TBL_MAILING_STRUTTURA.TipoMailing, TBL_MAILING_STRUTTURA.TestoMail, TBL_MAILING_STRUTTURA.SoggettoMail, TBL_MAILING_STRUTTURA.NoteInvio, TBL_MAILING_STRUTTURA.DataInserimento, TBL_MAILING_STRUTTURA.Lingua
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.Tipomailing = reader.GetInt32(reader.GetOrdinal("TipoMailing"));
                        if (!reader["TestoMail"].Equals(DBNull.Value))
                            item.TestoMail = reader.GetString(reader.GetOrdinal("TestoMail"));
                        if (!reader["SoggettoMail"].Equals(DBNull.Value))
                            item.SoggettoMail = reader.GetString(reader.GetOrdinal("SoggettoMail"));
                        if (!reader["NoteInvio"].Equals(DBNull.Value))
                            item.NoteInvio = reader.GetString(reader.GetOrdinal("NoteInvio"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));

                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento newsletter :" + error.Message, error);
            }
            return item;
        }

        /// <summary>
        /// Carica la lista dei gruppi di clienti per le newsletter
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mindate"></param>
        /// <returns></returns>
        public MailCollection CaricaListaNewsletter(string connection, DateTime? mindate = null)
        {
            MailCollection list = new MailCollection();
            if (connection == null || connection == "") return null;

            Mail item = new Mail();
            string query = "SELECT  * FROM TBL_MAILING_STRUTTURA order by Id";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (mindate != null)
            {
                OleDbParameter p1 = new OleDbParameter("@DataInserimento", mindate.Value.ToShortDateString()); //OleDbType.VarChar
                parColl.Add(p1);
                query += " WHERE DataInserimento > @DataInserimento ";
            }

            try
            {
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Mail();
                        //TBL_MAILING_STRUTTURA.TipoMailing, TBL_MAILING_STRUTTURA.TestoMail, TBL_MAILING_STRUTTURA.SoggettoMail, TBL_MAILING_STRUTTURA.NoteInvio, TBL_MAILING_STRUTTURA.DataInserimento, TBL_MAILING_STRUTTURA.Lingua
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.Tipomailing = reader.GetInt32(reader.GetOrdinal("TipoMailing"));
                        if (!reader["TestoMail"].Equals(DBNull.Value))
                            item.TestoMail = reader.GetString(reader.GetOrdinal("TestoMail"));
                        if (!reader["SoggettoMail"].Equals(DBNull.Value))
                            item.SoggettoMail = reader.GetString(reader.GetOrdinal("SoggettoMail"));
                        if (!reader["NoteInvio"].Equals(DBNull.Value))
                            item.NoteInvio = reader.GetString(reader.GetOrdinal("NoteInvio"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento lista newsletter :" + error.Message, error);
            }
            return list;
        }

        /// <summary>
        /// Carica la lista dei gruppi clienti per le mailing
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public TabrifCollection CaricaGruppiClientiNewsletter(string connection)
        {
            if (connection == null || connection == "") return null;

            TabrifCollection List = new TabrifCollection();
            Tabrif item = new Tabrif();
            //List<OleDbParameter> parColl = new List<OleDbParameter>();
            //OleDbParameter p1 = new OleDbParameter("@id", Id); //OleDbType.VarChar
            //parColl.Add(p1);
            try
            {
                string query = "SELECT DISTINCT GruppoMailing, DescrizioneGruppoMailing FROM TBL_MAILING_GRUPPI_CLIENTI order by DescrizioneGruppoMailing asc";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Tabrif();
                        item.Intero1 = reader.GetInt32(reader.GetOrdinal("GruppoMailing"));
                        if (!reader["DescrizioneGruppoMailing"].Equals(DBNull.Value))
                            item.Campo1 = reader.GetString(reader.GetOrdinal("DescrizioneGruppoMailing"));
                        List.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento gruppi per newsletter :" + error.Message, error);
            }
            return List;
        }

        /// <summary>
        /// Inserisce un nuovo gruppo clienti
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public int InserisciAggiornaNuovoGruppoClientiNewsletter(string connessione, Tabrif item)
        {
            int idret = -1;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;

            int idrecord = 0;
            int.TryParse(item.Id, out idrecord);


            string query = "";
            if (idrecord != 0) // Se il gruppo già esiste modifico solo la descrizione per il gruppo intero
            {
                OleDbParameter p5 = new OleDbParameter("@DescrizioneGruppoMailing", item.Campo1);//OleDbType.VarChar
                parColl.Add(p5);
                OleDbParameter p3 = new OleDbParameter("@GruppoMailing", item.Intero1);//OleDbType.VarChar
                parColl.Add(p3);
                //Update
                query = "UPDATE [TBL_MAILING_GRUPPI_CLIENTI] SET DescrizioneGruppoMailing=@DescrizioneGruppoMailing";
                query += " WHERE GruppoMailing = @GruppoMailing";
            }
            else
            {
                OleDbParameter p2 = new OleDbParameter("@ID_CLIENTE", item.Intero2);//OleDbType.VarChar
                parColl.Add(p2);
                OleDbParameter p5 = new OleDbParameter("@DescrizioneGruppoMailing", item.Campo1);//OleDbType.VarChar
                parColl.Add(p5);
                OleDbParameter p4 = null;
                p4 = new OleDbParameter("@DataInserimento", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.Data1));
                //p4.DbType = System.Data.DbType.DateTime;
                parColl.Add(p4);
                OleDbParameter p7 = new OleDbParameter("@Attivo", item.Bool1);//OleDbType.VarChar
                parColl.Add(p7);
                OleDbParameter p3 = new OleDbParameter("@GruppoMailing", item.Intero1);//OleDbType.VarChar
                parColl.Add(p3);
                //Insert
                query = "INSERT INTO TBL_MAILING_GRUPPI_CLIENTI (Id_cliente,DescrizioneGruppoMailing";
                query += ",DataInserimento,Attivo,GruppoMailing )";
                query += " values ( ";
                query += "@Id_cliente,@DescrizioneGruppoMailing";
                query += ",@DataInserimento,@Attivo,@GruppoMailing )";
            }
            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento gruppo Mailing :" + error.Message, error);
            }
            return idret;
        }

        /// <summary>
        /// elimina un cliente preciso dal gruppo clienti identificato
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="id_cliente"></param>
        /// <param name="idgruppo"></param>
        /// <returns></returns>
        public int EliminaClienteDaGruppoClientiNewsletter(string connessione, int id_cliente, int idgruppo)
        {
            int idret = -1;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_MAILING_GRUPPI_CLIENTI WHERE ( GruppoMailing = @GruppoMailing AND ID_CLIENTE = @ID_CLIENTE  ) ";
            OleDbParameter p1;
            p1 = new OleDbParameter("@GruppoMailing", idgruppo);
            parColl.Add(p1);
            OleDbParameter p2;
            p2 = new OleDbParameter("@ID_CLIENTE", id_cliente);
            parColl.Add(p2);
            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, eliminazione Mail da presa in carico:" + error.Message, error);
            }
            return idret;
        }
        /// <summary>
        /// Carica i dati base del gruppo di mailing clienti ( C'è sempre un record per gruppo con id_cliente = 0 che ne descrive le caratteristiche )
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="idGruppoMailing"></param>
        /// <returns></returns>
        public Tabrif CaricaGruppoMailing(string connection, int idgruppomailing)
        {
            if (connection == null || connection == "") return null;
            if (idgruppomailing == 0) return null;

            Tabrif item = new Tabrif();
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@GruppoMailing", idgruppomailing); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT  *  FROM TBL_MAILING_GRUPPI_CLIENTI WHERE GruppoMailing=@GruppoMailing AND ID_CLIENTE = 0 ";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Tabrif();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID")).ToString();
                        item.Intero1 = reader.GetInt32(reader.GetOrdinal("GruppoMailing"));
                        item.Intero2 = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        if (!reader["DescrizioneGruppoMailing"].Equals(DBNull.Value))
                            item.Campo1 = reader.GetString(reader.GetOrdinal("DescrizioneGruppoMailing"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Attivo"].Equals(DBNull.Value))
                            item.Bool1 = reader.GetBoolean(reader.GetOrdinal("Attivo"));
                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento gruppo per newsletter :" + error.Message, error);
            }
            return item;
        }
        /// <summary>
        /// Torna la lista dei clienti att che fanno parte di un certo gruppo
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idgruppomailing"></param>
        /// <returns></returns>
        public TabrifCollection CaricaClientiNewsletterPerGruppo(string connection, int idgruppomailing)
        {
            if (connection == null || connection == "") return null;
            if (idgruppomailing == 0) return null;

            TabrifCollection List = new TabrifCollection();
            Tabrif item = new Tabrif();
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@GruppoMailing", idgruppomailing); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT  *  FROM TBL_MAILING_GRUPPI_CLIENTI WHERE GruppoMailing=@GruppoMailing AND Attivo = true AND  ID_CLIENTE <> 0 ";
                //string query = "SELECT  *  FROM TBL_MAILING_GRUPPI_CLIENTI G LEFT JOIN TBL_CLIENTI C ON G.ID_CLIENTE = c.ID_CLIENTE  WHERE GruppoMailing=@GruppoMailing AND Attivo = true AND  ID_CLIENTE <> 0 ";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Tabrif();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID")).ToString();
                        item.Intero1 = reader.GetInt32(reader.GetOrdinal("GruppoMailing"));
                        item.Intero2 = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        if (!reader["DescrizioneGruppoMailing"].Equals(DBNull.Value))
                            item.Campo1 = reader.GetString(reader.GetOrdinal("DescrizioneGruppoMailing"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Attivo"].Equals(DBNull.Value))
                            item.Bool1 = reader.GetBoolean(reader.GetOrdinal("Attivo"));
                        List.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento gruppi per newsletter :" + error.Message, error);
            }
            return List;
        }

        /// <summary>
        /// Inserisce un nuovo cliente in un gruppo controllando se già presente
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id_cliente"></param>
        /// <param name="idGruppo"></param>
        public void AggiungiClienteAGruppo(string connection, int id_cliente, int idGruppo)
        {
            //Inseriamo nel gruppo il cliente indicato
            Tabrif gruppo = CaricaGruppoMailing(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idGruppo);
            gruppo.Id = "";//In modo da inserire un nuov record
            gruppo.Data1 = System.DateTime.Now;
            gruppo.Intero2 = id_cliente;
            //inseriamo il nuovo cliente nel gruppo ( dovresti verificare cho non ci sia già! )
            int idgruppo = InserisciAggiornaNuovoGruppoClientiNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, gruppo);
            return;
        }

        /// <summary>
        /// Aggiunge un blocco di clienti corrispondenti ai parametri di filtro passati 
        /// alla tabella relativa ai gruppi
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="_paramCliente"></param>
        /// <param name="idGruppo"></param>
        /// <param name="bypassvalidazione"></param>
        public void AggiungiClientiAGruppo(string connection, Cliente _paramCliente, int idGruppo, bool bypassvalidazione = false)
        {
            if (connection == null || connection == "") return;

            //Inseriamo nel gruppo il cliente indicato , carico per prima cosa il gruppo
            Tabrif gruppo = CaricaGruppoMailing(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idGruppo);
            if (gruppo == null || gruppo.Id == "") return;
            gruppo.Id = "";//In modo da inserire un nuov record
            gruppo.Data1 = System.DateTime.Now;
            //gruppo.Intero2 = id_cliente;

            string query = "";
            ////Insert
            query = "INSERT INTO TBL_MAILING_GRUPPI_CLIENTI ( ID_cliente, DescrizioneGruppoMailing";
            query += ", DataInserimento, GruppoMailing )";
            query += "  select tbl_clienti.ID_Cliente, '" + gruppo.Campo1 + "'  AS Espr1, '" + gruppo.Data1.ToShortDateString() + "'  AS Espr2, " + gruppo.Intero1 + "  AS Espr4 from tbl_clienti ";
            query += " WHERE Email like @Email ";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            //Prendendo dai campi del cliente posso inserire i parametri di filtraggio!!!! _paramCliente
            OleDbParameter p1 = new OleDbParameter("@Email", "%");
            if (_paramCliente != null && !string.IsNullOrEmpty(_paramCliente.Email))
            {
                p1 = new OleDbParameter("@Email", "%" + _paramCliente.Email + "%"); //OleDbType.VarChar
            }
            parColl.Add(p1);
            if (_paramCliente != null)
            {
                if (!bypassvalidazione)
                {
                    query += " and Validato = @Validato and Consenso1 = @Consenso1 ";
                    OleDbParameter p2 = new OleDbParameter("@Validato", _paramCliente.Validato); //OleDbType.VarChar
                    parColl.Add(p2);
                    OleDbParameter p3 = new OleDbParameter("@Consenso1", _paramCliente.Consenso1); //OleDbType.VarChar
                    parColl.Add(p3);
                }
                if (!string.IsNullOrEmpty(_paramCliente.id_tipi_clienti))
                {
                    //Il tipo cliente di default è lo 0 (consumatore ) quindi se non definito il tipo prende quel tipo lì
                    OleDbParameter ptipi = new OleDbParameter("@id_tipi_clienti", _paramCliente.id_tipi_clienti); //OleDbType.VarChar
                    parColl.Add(ptipi);
                    query += " and ID_tipi_clienti = @id_tipi_clienti ";
                }
                if (!string.IsNullOrWhiteSpace(_paramCliente.CodiceNAZIONE))
                {
                    OleDbParameter pnazione = new OleDbParameter("@codnaz", _paramCliente.CodiceNAZIONE); //OleDbType.VarChar
                    parColl.Add(pnazione);
                    query += " and CodiceNAZIONE = @codnaz ";
                }
                if (!string.IsNullOrWhiteSpace(_paramCliente.Lingua))
                {
                    OleDbParameter plingua = new OleDbParameter("@lingua", _paramCliente.Lingua); //OleDbType.VarChar
                    parColl.Add(plingua);
                    query += " and Lingua = @lingua ";
                }
            }

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento clienti in gruppo Mailing :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Rimuove un cliente da un gruppo
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id_cliente"></param>
        /// <param name="idGruppo"></param>
        public void EliminaClienteDaGruppo(string connection, int id_cliente, int idGruppo)
        {
            //Devo eliminare quella coppia gruppomailing idcliente dalla tabelle__mailing_gruppi
            EliminaClienteDaGruppoClientiNewsletter(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id_cliente, idGruppo);
            return;
        }

        /// <summary>
        /// Elimino dal gruppo mailing selezionato tutti i clienti pressenti
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id_cliente"></param>
        /// <param name="idGruppo"></param>
        public void EliminaClientiDaGruppo(string connection, int idGruppo)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connection == null || connection == "") return;

            //string query = "DELETE FROM TBL_MAILING_GRUPPI_CLIENTI WHERE ( GruppoMailing = @GruppoMailing AND ID_CLIENTE = @ID_CLIENTE  ) ";
            string query = "DELETE FROM TBL_MAILING_GRUPPI_CLIENTI WHERE ( GruppoMailing = @GruppoMailing AND ID_CLIENTE <> 0 ) ";

            OleDbParameter pgruppomailing;
            pgruppomailing = new OleDbParameter("@GruppoMailing", idGruppo);
            parColl.Add(pgruppomailing);

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, eliminazione clienti da gruppo mailing :" + error.Message, error);
            }
            return;

        }


    }
}
