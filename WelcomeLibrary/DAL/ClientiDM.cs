using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace WelcomeLibrary.DAL
{
    public class ClientiDM
    {

        /// <summary>
        /// Carica un cliente passando l'email dello stesso filtrando per tipologia ( Attenzione:  se più di un cliente ha quella mail torna il primo )
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codice"></param>
        /// <returns></returns>
        public Cliente CaricaClientePerEmail(string connection, string email, string idtipocliente = "")
        {
            Cliente item = new Cliente();
            if (connection == null || connection == "") return item;
            string query = "";
            //query = "SELECT A.*,B.CodiceCard AS CodiceCard FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD WHERE A.Email=@Email";

            query = "SELECT A.ID_CLIENTE,A.ID_CARD,A.Cap,A.Cellulare,A.CodiceCOMUNE,A.CodiceNAZIONE,A.CodicePROVINCIA,A.CodiceREGIONE,A.Cognome,A.Consenso1,A.Consenso2,A.Consenso3,A.Consenso4,A.ConsensoPrivacy,A.DataInvioValidazione,A.Sesso,A.DataNascita,A.DataRicezioneValidazione,A.Datainserimento,A.Email,A.Emailpec,A.Indirizzo,A.IPclient,A.Lingua,A.Nome,A.Professione,A.Spare1,A.Spare2,A.Telefono,A.TestoFormConsensi,A.Validato,A.Pivacf,A.Codicisconto,A.id_tipi_clienti,A.Serialized,A.Ragsoc,B.CodiceCard AS CodiceCard FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD WHERE  A.Email like @Email";

            //if (parColl == null || parColl.Count < 2) return list;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@Email", email); //OleDbType.VarChar
            parColl.Add(p1);
            if (idtipocliente == "") idtipocliente = "0";//metto il cliente di tipo default per la ricerca se non specificato!!
            query += " and id_tipi_clienti=@id_tipi_clienti";
            SQLiteParameter p2 = new SQLiteParameter("@id_tipi_clienti", idtipocliente); //OleDbType.VarChar
            parColl.Add(p2);

            try
            {

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Cliente();

                        item.Id_cliente = reader.GetInt64(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt64(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard")); // da verificare

                        if (!reader["Cap"].Equals(DBNull.Value))
                            item.Cap = reader.GetString(reader.GetOrdinal("Cap"));
                        if (!reader["Cellulare"].Equals(DBNull.Value))
                            item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodiceNAZIONE"].Equals(DBNull.Value))
                            item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["Cognome"].Equals(DBNull.Value))
                            item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));

                        if (!reader["Sesso"].Equals(DBNull.Value))
                            item.Sesso = reader.GetString(reader.GetOrdinal("Sesso"));

                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));
                        if (!reader["DataNascita"].Equals(DBNull.Value))
                            item.DataNascita = reader.GetDateTime(reader.GetOrdinal("DataNascita"));
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        if (!reader["Email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("Email"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));

                        if (!reader["Emailpec"].Equals(DBNull.Value))
                            item.Emailpec = reader.GetString(reader.GetOrdinal("Emailpec"));
                        if (!reader["Indirizzo"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));
                        if (!reader["IPclient"].Equals(DBNull.Value))
                            item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        if (!reader["Nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        if (!reader["Professione"].Equals(DBNull.Value))
                            item.Professione = reader.GetString(reader.GetOrdinal("Professione"));
                        if (!reader["Spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));
                        if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        //if (!reader["Spare3"].Equals(DBNull.Value))
                        //    item.Spare3 = reader.GetString(reader.GetOrdinal("Spare3"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));

                        if (!reader["Codicisconto"].Equals(DBNull.Value))
                            item.Codicisconto = reader.GetString(reader.GetOrdinal("Codicisconto"));
                        item.id_tipi_clienti = reader.GetInt64(reader.GetOrdinal("id_tipi_clienti")).ToString();
                        if (!reader["Serialized"].Equals(DBNull.Value))
                            item.Serialized = reader.GetString(reader.GetOrdinal("Serialized"));
                        item.addvalues = (!string.IsNullOrEmpty(item.Serialized)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(item.Serialized) : new Cliente();

                        if (!reader["Ragsoc"].Equals(DBNull.Value))
                            item.Ragsoc = reader.GetString(reader.GetOrdinal("Ragsoc"));

                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Cliente :" + error.Message, error);
            }

            return item;
        }

        public Cliente CaricaClientePerCodicesconto(string connection, string codicesconto, string idtipocliente = "")
        {
            Cliente item = new Cliente();
            if (connection == null || connection == "") return item;
            if (codicesconto == null || codicesconto == "") return item;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();

            string query = "";
            query = "SELECT *  FROM TBL_CLIENTI ";


            if (!string.IsNullOrWhiteSpace(codicesconto))
            {
                //Il tipo cliente di default è lo 0 (consumatore ) quindi se non definito il tipo prende quel tipo lì
                SQLiteParameter pcodsconto = new SQLiteParameter("@Codicisconto", "%" + codicesconto + "%"); //OleDbType.VarChar
                parColl.Add(pcodsconto);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE Codicisconto like @Codicisconto ";
                else
                    query += " AND Codicisconto like @Codicisconto  ";
                SQLiteParameter p1 = new SQLiteParameter("@Codicesconto", codicesconto); //OleDbType.VarChar
                parColl.Add(p1);
            }


            if (idtipocliente != "")
            {
                SQLiteParameter p2 = new SQLiteParameter("@id_tipi_clienti", idtipocliente); //OleDbType.VarChar
                parColl.Add(p2);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE  id_tipi_clienti=@id_tipi_clienti";
                else
                    query += " AND  id_tipi_clienti=@id_tipi_clienti  ";
            }


            try
            {

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Cliente();

                        item.Id_cliente = reader.GetInt64(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt64(reader.GetOrdinal("ID_CARD"));

                        if (!reader["Cap"].Equals(DBNull.Value))
                            item.Cap = reader.GetString(reader.GetOrdinal("Cap"));
                        if (!reader["Cellulare"].Equals(DBNull.Value))
                            item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodiceNAZIONE"].Equals(DBNull.Value))
                            item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["Cognome"].Equals(DBNull.Value))
                            item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));

                        if (!reader["Sesso"].Equals(DBNull.Value))
                            item.Sesso = reader.GetString(reader.GetOrdinal("Sesso"));

                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));
                        if (!reader["DataNascita"].Equals(DBNull.Value))
                            item.DataNascita = reader.GetDateTime(reader.GetOrdinal("DataNascita"));
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("Email"));

                        if (!reader["Emailpec"].Equals(DBNull.Value))
                            item.Emailpec = reader.GetString(reader.GetOrdinal("Emailpec"));
                        if (!reader["Indirizzo"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));
                        if (!reader["IPclient"].Equals(DBNull.Value))
                            item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        if (!reader["Nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        if (!reader["Professione"].Equals(DBNull.Value))
                            item.Professione = reader.GetString(reader.GetOrdinal("Professione"));
                        if (!reader["Spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));
                        if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        //if (!reader["Spare3"].Equals(DBNull.Value))
                        //    item.Spare3 = reader.GetString(reader.GetOrdinal("Spare3"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));

                        if (!reader["Codicisconto"].Equals(DBNull.Value))
                            item.Codicisconto = reader.GetString(reader.GetOrdinal("Codicisconto"));
                        item.id_tipi_clienti = reader.GetInt64(reader.GetOrdinal("id_tipi_clienti")).ToString();
                        if (!reader["Serialized"].Equals(DBNull.Value))
                            item.Serialized = reader.GetString(reader.GetOrdinal("Serialized"));
                        item.addvalues = (!string.IsNullOrEmpty(item.Serialized)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(item.Serialized) : new Cliente();

                        if (!reader["Ragsoc"].Equals(DBNull.Value))
                            item.Ragsoc = reader.GetString(reader.GetOrdinal("Ragsoc"));
                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Cliente :" + error.Message, error);
            }

            return item;
        }
        /// <summary>
        /// Splitta in un dictionari le coppie codicesconto;percentualesconto;......... etc
        /// </summary>
        /// <param name="codici"></param>
        /// <returns></returns>
        public static Dictionary<string, double> SplitCodiciSconto(string codici)
        {
            Dictionary<string, double> dict = new Dictionary<string, double>();
            string[] listaarray = codici.Split(';');
            string key = "";
            bool even = false;
            foreach (string codice in listaarray)
            {
                if (!even)
                {
                    key = codice.ToLower().Trim();
                    if (!dict.ContainsKey(key) && !string.IsNullOrEmpty(key)) dict.Add(key, 0);
                }
                else
                {
                    double convperc = 0;
                    double.TryParse(codice, out convperc);
                    if (dict.ContainsKey(key)) dict[key] = convperc;
                    key = "";
                }
                even = !even;
            }
            return dict;
        }

        /// <summary>
        /// Carica un cliente in base all'identificativo dello stesso nel db
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ID_cliente"></param>
        /// <returns></returns>
        public Cliente CaricaClientePerId(string connection, string ID_cliente)
        {
            Cliente item = new Cliente();
            if (connection == null || connection == "") return item;
            //if (parColl == null || parColl.Count < 2) return list;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@Id_cliente", ID_cliente); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "";
                query = "SELECT A.ID_CLIENTE,A.ID_CARD,A.Cap,A.Cellulare,A.CodiceCOMUNE,A.CodiceNAZIONE,A.CodicePROVINCIA,A.CodiceREGIONE,A.Cognome,A.Consenso1,A.Consenso2,A.Consenso3,A.Consenso4,A.ConsensoPrivacy,A.DataInvioValidazione,A.Sesso,A.DataNascita,A.DataRicezioneValidazione,A.Datainserimento,A.Email,A.Emailpec,A.Indirizzo,A.IPclient,A.Lingua,A.Nome,A.Professione,A.Spare1,A.Spare2,A.Telefono,A.TestoFormConsensi,A.Validato,A.Pivacf,A.Codicisconto,A.id_tipi_clienti,A.Serialized,A.Ragsoc,B.CodiceCard AS CodiceCard FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD WHERE A.Id_cliente=@Id_cliente";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Cliente();

                        item.Id_cliente = reader.GetInt64(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt64(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard")); // da verificare
                        if (!reader["Cap"].Equals(DBNull.Value))
                            item.Cap = reader.GetString(reader.GetOrdinal("Cap"));
                        if (!reader["Cellulare"].Equals(DBNull.Value))
                            item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodiceNAZIONE"].Equals(DBNull.Value))
                            item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["Cognome"].Equals(DBNull.Value))
                            item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));
                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));


                        if (!reader["Sesso"].Equals(DBNull.Value))
                            item.Sesso = reader.GetString(reader.GetOrdinal("Sesso"));

                        if (!reader["DataNascita"].Equals(DBNull.Value))
                            item.DataNascita = reader.GetDateTime(reader.GetOrdinal("DataNascita"));
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        if (!reader["Email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("Email"));
                        if (!reader["Emailpec"].Equals(DBNull.Value))
                            item.Emailpec = reader.GetString(reader.GetOrdinal("Emailpec"));
                        if (!reader["Indirizzo"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));
                        if (!reader["IPclient"].Equals(DBNull.Value))
                            item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        if (!reader["Nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        if (!reader["Professione"].Equals(DBNull.Value))
                            item.Professione = reader.GetString(reader.GetOrdinal("Professione"));
                        if (!reader["Spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));
                        if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        //if (!reader["Spare3"].Equals(DBNull.Value))
                        //    item.Spare3 = reader.GetString(reader.GetOrdinal("Spare3"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));
                        if (!reader["Codicisconto"].Equals(DBNull.Value))
                            item.Codicisconto = reader.GetString(reader.GetOrdinal("Codicisconto"));
                        item.id_tipi_clienti = reader.GetInt64(reader.GetOrdinal("id_tipi_clienti")).ToString();
                        if (!reader["Serialized"].Equals(DBNull.Value))
                            item.Serialized = reader.GetString(reader.GetOrdinal("Serialized"));
                        item.addvalues = (!string.IsNullOrEmpty(item.Serialized)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(item.Serialized) : new Cliente();

                        if (!reader["Ragsoc"].Equals(DBNull.Value))
                            item.Ragsoc = reader.GetString(reader.GetOrdinal("Ragsoc"));
                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Cliente :" + error.Message, error);
            }

            return item;
        }



        public static Cliente GetNomeClientePerId(string connection, string ID_cliente)
        {
            Cliente item = new Cliente();
            if (connection == null || connection == "") return item;
            //if (parColl == null || parColl.Count < 2) return list;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@Id_cliente", ID_cliente); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "";
                query = "SELECT  Cognome,Nome FROM TBL_CLIENTI  WHERE Id_cliente=@Id_cliente";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Cliente();
                        if (!reader["Cognome"].Equals(DBNull.Value))
                            item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));
                        item.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        break;
                    }
                }

            }
            catch
            {
                //throw new ApplicationException("Errore Caricamento Cliente :" + error.Message, error);
            }

            return item;
        }


        public string CancellaClientiPerTipologia(string connessione, string id_tipi_clienti)
        {
            string idret = "";
            long idtipocliente = 0;
            if (!long.TryParse(id_tipi_clienti, out idtipocliente)) return idret;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;
            if (id_tipi_clienti == null || id_tipi_clienti == "") return idret;

            string query = "DELETE FROM TBL_CLIENTI WHERE ( ID_tipi_clienti = @id_tipi_clienti ) ";
            SQLiteParameter p1;
            p1 = new SQLiteParameter("@id_tipi_clienti", idtipocliente);
            parColl.Add(p1);

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                idret = "Errore, eliminazione clienti per tipo :" + error.Message;
            }
            return idret;
        }

        public string CancellaClientePerId(string connessione, long ID_cliente)
        {
            string idret = "";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_CLIENTI WHERE ( ID_CLIENTE = @ID_cliente ) ";
            SQLiteParameter p1;
            p1 = new SQLiteParameter("@ID_cliente", ID_cliente);
            parColl.Add(p1);
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                idret = "Errore, eliminazione cliente :" + error.Message;
            }
            return idret;
        }

#if false
        /// <summary>
        /// Carica i clienti validati che sono associati alla lista id delle cards passate
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        public ClienteCollection CaricaClientiInbaseacards(string connection, CardCollection cards)
        {
            Cliente item = new Cliente();
            ClienteCollection list = new ClienteCollection();
            if (connection == null || connection == "") return list;
            try
            {
                string query = "";
                query = "SELECT A.*,B.CodiceCard AS CodiceCard FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD ";
                query += " WHERE Validato = true ";
                if (cards != null && cards.Count > 0)
                {
                    query += "  AND A.Id_card in (  ";
                    foreach (Card card in cards)
                    {
                        query += " " + card.Id_card + " ,";
                    }
                    query = query.TrimEnd(',') + " ) ";
                }
                else return list;


                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item.Id_cliente = reader.GetInt64(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt64(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard")); // da verificare
                        if (!reader["Cap"].Equals(DBNull.Value))
                            item.Cap = reader.GetString(reader.GetOrdinal("Cap"));
                        if (!reader["Cellulare"].Equals(DBNull.Value))
                            item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodiceNAZIONE"].Equals(DBNull.Value))
                            item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["Cognome"].Equals(DBNull.Value))
                            item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));
                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));

                        if (!reader["Sesso"].Equals(DBNull.Value))
                            item.Sesso = reader.GetString(reader.GetOrdinal("Sesso"));

                        if (!reader["DataNascita"].Equals(DBNull.Value))
                            item.DataNascita = reader.GetDateTime(reader.GetOrdinal("DataNascita"));
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        if (!reader["Email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("Email"));
                        if (!reader["Indirizzo"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));
                        if (!reader["IPclient"].Equals(DBNull.Value))
                            item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        if (!reader["Nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        if (!reader["Professione"].Equals(DBNull.Value))
                            item.Professione = reader.GetString(reader.GetOrdinal("Professione"));
                        if (!reader["Spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));
                         if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        //if (!reader["Spare3"].Equals(DBNull.Value))
                        //    item.Spare3 = reader.GetString(reader.GetOrdinal("Spare3"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));
                        if (!reader["Codicisconto"].Equals(DBNull.Value))
                            item.Codicisconto = reader.GetString(reader.GetOrdinal("Codicisconto"));
                        item.id_tipi_clienti = reader.GetInt64(reader.GetOrdinal("id_tipi_clienti")).ToString();
                    if (!reader["Serialized"].Equals(DBNull.Value))
                            item.Serialized = reader.GetString(reader.GetOrdinal("Serialized"));
                               if (!reader["Ragsoc"].Equals(DBNull.Value))
                            item.Ragsoc = reader.GetString(reader.GetOrdinal("Ragsoc"));
                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("CaricaClientiInbaseacards() - Errore Caricamento Clienti :" + error.Message, error);
            }

            return list;
        }

#endif

#if false
        /// <summary>
        /// Ritorna la lista clienti inbase ai filtri passati 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="_paramCliente">Parametri di filtro email,validato,consenso1</param>
        /// <returns></returns>
        public ClienteCollection CaricaClientiFiltrati(string connection, Cliente _paramCliente)
        {
            ClienteCollection list = new ClienteCollection();
            if (connection == null || connection == "") return list;
            //if (parColl == null || parColl.Count < 2) return list;

            Cliente item;

            try
            {
                string query = "";
                query = "SELECT A.*,B.CodiceCard AS CodiceCard, B.DataGenerazione as DataGenerazione, B.DataAttivazione as DataAttivazione, B.DurataGG as DurataGG,B.AssegnatoACard as AssegnatoACard ";
                query += " FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD WHERE Email like @Email and Validato = @Validato and Consenso1 = @Consenso1";

                List<SQLiteParameter> parColl = new List<SQLiteParameter>();

                //Prendendo dai campi del cliente posso inserire i parametri di filtraggio!!!! _paramCliente
                SQLiteParameter p1 = new SQLiteParameter("@Email", "%");
                if (!string.IsNullOrEmpty(_paramCliente.Email))
                {
                    p1 = new SQLiteParameter("@Email", "%" + _paramCliente.Email + "%"); //OleDbType.VarChar
                }
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@Validato", _paramCliente.Validato); //OleDbType.VarChar
                parColl.Add(p2);
                SQLiteParameter p3 = new SQLiteParameter("@Consenso1", _paramCliente.Consenso1); //OleDbType.VarChar
                parColl.Add(p3);

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);


                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Cliente();


                        item.Id_cliente = reader.GetInt64(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt64(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard"));

                        //Carico tutti i dati della eventuale card associata al cliente
                        item.card.Id_card = item.Id_card;
                        item.card.CodiceCard = item.CodiceCard;
                        if (!reader["AssegnatoACard"].Equals(DBNull.Value))
                            item.card.AssegnatoACard = reader.GetBoolean(reader.GetOrdinal("AssegnatoACard"));
                        if (!reader["DataAttivazione"].Equals(DBNull.Value))
                            item.card.DataAttivazione = reader.GetDateTime(reader.GetOrdinal("DataAttivazione"));
                        if (!reader["DataGenerazione"].Equals(DBNull.Value))
                            item.card.DataGenerazione = reader.GetDateTime(reader.GetOrdinal("DataGenerazione"));
                        if (!reader["DurataGG"].Equals(DBNull.Value))
                            item.card.DurataGG = reader.GetInt64(reader.GetOrdinal("DurataGG"));

                        if (!reader["Cap"].Equals(DBNull.Value))
                            item.Cap = reader.GetString(reader.GetOrdinal("Cap"));
                        if (!reader["Cellulare"].Equals(DBNull.Value))
                            item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodiceNAZIONE"].Equals(DBNull.Value))
                            item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["Cognome"].Equals(DBNull.Value))
                            item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));
                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));

                        if (!reader["DataNascita"].Equals(DBNull.Value))
                            item.DataNascita = reader.GetDateTime(reader.GetOrdinal("DataNascita"));
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        if (!reader["Email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("Email"));
                        if (!reader["Indirizzo"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));
                        if (!reader["IPclient"].Equals(DBNull.Value))
                            item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        if (!reader["Nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        if (!reader["Professione"].Equals(DBNull.Value))
                            item.Professione = reader.GetString(reader.GetOrdinal("Professione"));
                        if (!reader["Spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));
                        if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        //if (!reader["Spare3"].Equals(DBNull.Value))
                        //    item.Spare3 = reader.GetString(reader.GetOrdinal("Spare3"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Clienti :" + error.Message, error);
            }

            return list;
        }
        
#endif
        /// <summary>
        /// Carica tutti i clienti presenti in  un certo gruppo newsletter
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idgruppomailing"></param>
        /// <returns></returns>
        public ClienteCollection CaricaClientiPerGruppoNewsletter(string connection, long idgruppomailing)
        {
            if (connection == null || connection == "") return null;
            if (idgruppomailing == 0) return null;
            int test = 1;
            ClienteCollection List = new ClienteCollection();
            Cliente item = new Cliente();
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@GruppoMailing", idgruppomailing); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT  G.GruppoMailing as G_GruppoMailing,G.ID_CLIENTE as G_ID_CLIENTE,G.DescrizioneGruppoMailing,G.DataInserimento,G.Attivo,C.ID_CLIENTE as C_ID_CLIENTE,C.ID_CARD,C.Cap,C.Cellulare,C.CodiceCOMUNE,C.CodiceNAZIONE,C.CodicePROVINCIA,C.CodiceREGIONE,C.Cognome,C.Consenso1,C.Consenso2,C.Consenso3,C.Consenso4,C.ConsensoPrivacy,C.DataInvioValidazione,C.Datainserimento,C.Sesso,C.DataNascita,C.DataRicezioneValidazione,C.Email,C.Emailpec,C.Indirizzo,C.IPclient,C.Lingua,C.Nome,C.Professione,C.Spare1,C.Spare2,C.Telefono,C.TestoFormConsensi,C.Validato,C.Pivacf,C.Codicisconto,C.id_tipi_clienti,C.Serialized,C.Ragsoc FROM TBL_MAILING_GRUPPI_CLIENTI G LEFT JOIN TBL_CLIENTI C ON G.ID_CLIENTE = c.ID_CLIENTE  WHERE GruppoMailing=@GruppoMailing AND  G.ID_CLIENTE <> 0 ";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Cliente();

                        if (reader["C_ID_CLIENTE"].Equals(DBNull.Value)) continue;//salto i record per cui non trovo l'id del cliente!

                        item.Id_cliente = reader.GetInt64(reader.GetOrdinal("C_ID_CLIENTE"));

                        if (!reader["ID_CARD"].Equals(DBNull.Value))
                            item.Id_card = reader.GetInt64(reader.GetOrdinal("ID_CARD"));

                        if (!reader["Cap"].Equals(DBNull.Value))
                            item.Cap = reader.GetString(reader.GetOrdinal("Cap"));
                        if (!reader["Cellulare"].Equals(DBNull.Value))
                            item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodiceNAZIONE"].Equals(DBNull.Value))
                            item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["Cognome"].Equals(DBNull.Value))
                            item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));
                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));

                        if (!reader["Sesso"].Equals(DBNull.Value))
                            item.Sesso = reader.GetString(reader.GetOrdinal("Sesso"));

                        if (!reader["DataNascita"].Equals(DBNull.Value))
                            item.DataNascita = reader.GetDateTime(reader.GetOrdinal("DataNascita"));
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));

                        if (!reader["Email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("Email"));
                        if (!reader["Emailpec"].Equals(DBNull.Value))
                            item.Emailpec = reader.GetString(reader.GetOrdinal("Emailpec"));
                        if (!reader["Indirizzo"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));
                        if (!reader["IPclient"].Equals(DBNull.Value))
                            item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        if (!reader["Nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                        if (!reader["Professione"].Equals(DBNull.Value))
                            item.Professione = reader.GetString(reader.GetOrdinal("Professione"));
                        if (!reader["Spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));
                        if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        //if (!reader["Spare3"].Equals(DBNull.Value))
                        //    item.Spare3 = reader.GetString(reader.GetOrdinal("Spare3"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));
                        if (!reader["Codicisconto"].Equals(DBNull.Value))
                            item.Codicisconto = reader.GetString(reader.GetOrdinal("Codicisconto"));

                        if (!reader["id_tipi_clienti"].Equals(DBNull.Value))
                            item.id_tipi_clienti = reader.GetInt64(reader.GetOrdinal("id_tipi_clienti")).ToString();
                        if (!reader["Serialized"].Equals(DBNull.Value))
                            item.Serialized = reader.GetString(reader.GetOrdinal("Serialized"));
                        item.addvalues = (!string.IsNullOrEmpty(item.Serialized)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(item.Serialized) : new Cliente();
                        if (!reader["Ragsoc"].Equals(DBNull.Value))
                            item.Ragsoc = reader.GetString(reader.GetOrdinal("Ragsoc"));

                        test += 1;
                        List.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Clienti per gruppo newsletter :" + error.Message, error);
            }
            return List;
        }
        /// <summary>
        /// Carica la lista codice/descrizione clienti dal db, filtrando in base alla descrizione dei clienti ( per il webservice )
        /// </summary>
        /// <returns></returns>
        public ClienteCollection GetLista(string testoricerca, string constr)
        {
            ClienteCollection list = new ClienteCollection();
            if (constr == "") return list;
            string query = "SELECT ID_CLIENTE,Cognome,Nome,Email FROM [TBL_CLIENTI] WHERE  cast(ID_CLIENTE as text)   LIKE @testoricerca or Cognome LIKE @testoricerca or Nome LIKE @testoricerca or Email like @testoricerca";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            // parametro
            SQLiteParameter p1 = new SQLiteParameter("@testoricerca", "%" + testoricerca + "%"); //OleDbType.VarChar
            parColl.Add(p1);

            long tmp = 0;
            if (long.TryParse(testoricerca, out tmp))
            {
                SQLiteParameter p2 = new SQLiteParameter("@idcliente", tmp); //OleDbType.VarChar
                parColl.Add(p2);
                query += " or ID_CLIENTE = @idcliente ";
            }
            SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, constr);
            Cliente item;
            using (reader)
            {
                if (reader == null) { return null; };
                if (reader.HasRows == false)
                    return list;
                while (reader.Read())
                {
                    item = new Cliente();
                    item.Id_cliente = reader.GetInt64(reader.GetOrdinal("ID_CLIENTE"));
                    item.Spare3 = "";
                    if (!(reader["Cognome"]).Equals(DBNull.Value))
                    {
                        item.Spare3 = reader.GetString(reader.GetOrdinal("Cognome"));//
                        item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));//
                    }
                    if (!(reader["Nome"]).Equals(DBNull.Value))
                    {
                        item.Spare3 += " " + reader.GetString(reader.GetOrdinal("Nome"));//
                        item.Nome += reader.GetString(reader.GetOrdinal("Nome"));//
                    }
                    if (!(reader["Email"]).Equals(DBNull.Value))
                    {
                        item.Spare3 += " " + reader.GetString(reader.GetOrdinal("Email"));//
                        item.Email += reader.GetString(reader.GetOrdinal("Email"));//
                    }
                    // aggiungo alla collection
                    list.Add(item);
                }
            }
            return list;
        }

        public string ExportClientiToCsv(string DestinationPath, string CsvFilename, List<Cliente> list)
        {
            string retString = "";
            string lingua = "I";
            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("it-IT");
                WelcomeLibrary.UF.SharedStatic.WriteToFile(CsvFilename, DestinationPath, "", true);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (list != null)

                {
                    sb = new StringBuilder();
                    ///TRACCIATO USATO ---------------------------------------------
                    //Id_cliente,
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("id"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("tipo cliente"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("lingua"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Nome"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Cognome"));
                    sb.Append(";");
                    //sb.Append(WelcomeLibrary.UF.Csv.Escape("Ragione sociale"));
                    //sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("nazione"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("regione"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("provincia"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("comune"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("email"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("emailpec"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("telefono"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("cellulare"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Documento"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("data nascita"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("cap"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("indirizzo"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("piva"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("codici sconto"));
                    sb.Append(";");

                    sb.Append(WelcomeLibrary.UF.Csv.Escape("consenso email"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("data invio validazione"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("data ricezione validazione"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("data inserimento"));




                    WelcomeLibrary.UF.SharedStatic.WriteToFile(CsvFilename, DestinationPath, sb.ToString(), false);

                    foreach (Cliente c in list)
                    {

                        sb = new StringBuilder();
                        string tmp = "";
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Id_cliente.ToString()));
                        sb.Append(";");
                        string tipocliente = c.id_tipi_clienti;
                        Tabrif tipoclienteobj = Utility.TipiClienti.Find(delegate (Tabrif _t) { return _t.Lingua == "I" && _t.Codice == c.id_tipi_clienti; });
                        if (tipoclienteobj != null)
                            tipocliente = tipoclienteobj.Campo1;
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(tipocliente));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Lingua));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Nome));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Cognome));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.CodiceNAZIONE));
                        sb.Append(";");
                        WelcomeLibrary.DOM.Province psearch = WelcomeLibrary.UF.Utility.ElencoProvince.Find(t => t.Lingua == lingua && t.Codice == c.CodiceREGIONE);
                        if (psearch != null)
                            tmp = psearch.Regione;
                        sb.Append(tmp);
                        sb.Append(";");
                        psearch = WelcomeLibrary.UF.Utility.ElencoProvince.Find(t => t.Lingua == lingua && t.Codice == c.CodicePROVINCIA);
                        if (psearch != null)
                            tmp = psearch.Provincia;
                        sb.Append(tmp);
                        sb.Append(";");
                        sb.Append(c.CodiceCOMUNE);
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Email));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Emailpec));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Telefono));
                        sb.Append(";");

                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Cellulare));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Professione));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(string.Format("{0:dd/MM/yyyy HH:mm:ss}", c.DataNascita)));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Cap));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Indirizzo));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Pivacf));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Codicisconto));
                        sb.Append(";");

                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Validato.ToString()));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(string.Format("{0:dd/MM/yyyy HH:mm:ss}", c.DataInvioValidazione)));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(string.Format("{0:dd/MM/yyyy HH:mm:ss}", c.DataRicezioneValidazione)));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(string.Format("{0:dd/MM/yyyy HH:mm:ss}", c.DataInserimento)));


                        WelcomeLibrary.UF.SharedStatic.WriteToFile(CsvFilename, DestinationPath, sb.ToString(), false);



                    }
                }
            }
            catch (Exception err)
            {
                retString = err.Message;
                if (err.InnerException != null)
                    retString += err.InnerException.Message;
            }
            return retString;
        }


        /// <summary>
        /// Ritorna la lista clienti inbase ai filtri passati 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="_paramCliente">Parametri di filtro email,validato,consenso1</param>
        /// <returns></returns>
        public ClienteCollection CaricaClientiFiltrati(string connection, Cliente _paramCliente, bool bypassvalidazione = false, long page = 1, long pagesize = 0)
        {
            ClienteCollection list = new ClienteCollection();
            if (connection == null || connection == "") return list;
            //if (parColl == null || parColl.Count < 2) return list;
            Cliente item;
            try
            {
                string query = "";
                //query = "SELECT A.*,B.CodiceCard AS CodiceCard, B.DataGenerazione as DataGenerazione, B.DataAttivazione as DataAttivazione, B.DurataGG as DurataGG,B.AssegnatoACard as AssegnatoACard ";
                //query += " FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD WHERE Email like @Email ";

                query = "SELECT A.ID_CLIENTE,A.ID_CARD,A.Cap,A.Cellulare,A.CodiceCOMUNE,A.CodiceNAZIONE,A.CodicePROVINCIA,A.CodiceREGIONE,A.Cognome,A.Consenso1,A.Consenso2,A.Consenso3,A.Consenso4,A.ConsensoPrivacy,A.DataInvioValidazione,A.Sesso,A.DataNascita,A.DataRicezioneValidazione,A.Datainserimento,A.Email,A.Emailpec,A.Indirizzo,A.IPclient,A.Lingua,A.Nome,A.Professione,A.Spare1,A.Spare2,A.Telefono,A.TestoFormConsensi,A.Validato,A.Pivacf,A.Codicisconto,A.id_tipi_clienti,A.Serialized,A.Ragsoc,B.CodiceCard AS CodiceCard, B.DataGenerazione as DataGenerazione, B.DataAttivazione as DataAttivazione, B.DurataGG as DurataGG,B.AssegnatoACard as AssegnatoACard  FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD ";

                string queryfilter = " WHERE Email like @Email ";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                //Prendendo dai campi del cliente posso inserire i parametri di filtraggio!!!! _paramCliente
                SQLiteParameter p1 = new SQLiteParameter("@Email", "%");
                if (_paramCliente != null && !string.IsNullOrEmpty(_paramCliente.Email))
                {
                    //p1 = new SQLiteParameter("@Email", "%" + _paramCliente.Email + "%"); //puo restituire piu clienti se la mail è contenuta nella parte inizio o fine di un altra
                    p1 = new SQLiteParameter("@Email", _paramCliente.Email); //puo restituire piu clienti se la mail è contenuta nella parte inizio o fine di un altra
                }

                parColl.Add(p1);
                if (_paramCliente != null)
                {



                    if (_paramCliente.Id_cliente != 0)
                    {
                        SQLiteParameter p2 = new SQLiteParameter("@Id_cliente", _paramCliente.Id_cliente); //OleDbType.VarChar
                        parColl.Add(p2);
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE  Id_cliente=@Id_cliente";
                        else
                            queryfilter += " AND  Id_cliente=@Id_cliente  ";
                    }

                    if (!bypassvalidazione)
                    {
                        queryfilter += " and Validato = @Validato and Consenso1 = @Consenso1 ";
                        SQLiteParameter p2 = new SQLiteParameter("@Validato", _paramCliente.Validato); //OleDbType.VarChar
                        parColl.Add(p2);
                        SQLiteParameter p3 = new SQLiteParameter("@Consenso1", _paramCliente.Consenso1); //OleDbType.VarChar
                        parColl.Add(p3);
                    }
                    if (!string.IsNullOrWhiteSpace(_paramCliente.Codicisconto))
                    {
                        //Il tipo cliente di default è lo 0 (consumatore ) quindi se non definito il tipo prende quel tipo lì
                        //SQLiteParameter pcodsconto = new SQLiteParameter("@Codicisconto", "%" + _paramCliente.Codicisconto + "%"); //OleDbType.VarChar
                        SQLiteParameter pcodsconto = new SQLiteParameter("@Codicisconto", _paramCliente.Codicisconto); //OleDbType.VarChar
                        parColl.Add(pcodsconto);
                        queryfilter += " and Codicisconto like @Codicisconto ";
                    }

                    if (!string.IsNullOrWhiteSpace(_paramCliente.id_tipi_clienti))
                    {
                        //Il tipo cliente di default è lo 0 (consumatore ) quindi se non definito il tipo prende quel tipo lì
                        SQLiteParameter ptipi = new SQLiteParameter("@id_tipi_clienti", _paramCliente.id_tipi_clienti); //OleDbType.VarChar
                        parColl.Add(ptipi);
                        queryfilter += " and ID_tipi_clienti = @id_tipi_clienti ";
                    }
                    if (!string.IsNullOrWhiteSpace(_paramCliente.CodiceNAZIONE))
                    {
                        SQLiteParameter pnazione = new SQLiteParameter("@codnaz", _paramCliente.CodiceNAZIONE); //OleDbType.VarChar
                        parColl.Add(pnazione);
                        queryfilter += " and CodiceNAZIONE = @codnaz ";
                    }
                    if (!string.IsNullOrWhiteSpace(_paramCliente.Lingua))
                    {
                        SQLiteParameter plingua = new SQLiteParameter("@lingua", _paramCliente.Lingua); //OleDbType.VarChar
                        parColl.Add(plingua);
                        queryfilter += " and Lingua = @lingua ";
                    }

                    if (!string.IsNullOrWhiteSpace(_paramCliente.Sesso))
                    {
                        SQLiteParameter psesso = new SQLiteParameter("@Sesso", _paramCliente.Sesso); //OleDbType.VarChar
                        parColl.Add(psesso);
                        queryfilter += " and Sesso = @Sesso ";
                    }

                    if (!string.IsNullOrWhiteSpace(_paramCliente.Spare2)) //FIltro per età
                    {

                        string consume = _paramCliente.Spare2;
                        string etamin = consume.Substring(0, consume.IndexOf("|"));
                        consume = consume.Substring(consume.IndexOf("|") + 1);
                        string etamax = consume;
                        int emin = 0; int emax = 0;
                        if (etamin != "" && etamax != "")
                        {
                            int.TryParse(etamin, out emin);
                            int.TryParse(etamax, out emax);
                            DateTime di = System.DateTime.Now.AddYears(-emax);
                            DateTime df = System.DateTime.Now.AddYears(-emin);

                            SQLiteParameter datainizio = new SQLiteParameter("@Data_inizio", dbDataAccess.CorrectDatenow(di));
                            parColl.Add(datainizio);

                            SQLiteParameter datafine = new SQLiteParameter("@Data_fine", dbDataAccess.CorrectDatenow(df));
                            parColl.Add(datafine);

                            queryfilter += " AND   ( DataNascita >= @Data_inizio and  DataNascita <= @Data_fine )  ";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(_paramCliente.Spare1)) //Filtro giorno di nascita
                    {
                        string consume = _paramCliente.Spare1;
                        string giorno = consume.Substring(0, consume.IndexOf("/"));
                        consume = consume.Substring(consume.IndexOf("/") + 1);
                        string mese = consume.Substring(0, consume.IndexOf("/"));
                        consume = consume.Substring(consume.IndexOf("/") + 1);
                        string anno = consume;
                        int annoa = 0; int annob = 0; int mesea = 0; int meseb = 0; int giornoa = 0; int giornob = 0;

                        if (anno != "0")
                        {
                            int.TryParse(anno, out annoa);
                            SQLiteParameter parannoa = new SQLiteParameter("@anno", annoa); //OleDbType.VarChar
                            parColl.Add(parannoa);
                            queryfilter += " and (strftime('%Y',[DataNascita])=@anno)) ";
                        }

                        if (mese != "0")
                        {
                            int.TryParse(mese, out mesea);
                            SQLiteParameter parmese = new SQLiteParameter("@mese", mesea); //OleDbType.VarChar
                            parColl.Add(parmese);
                            queryfilter += " and (strftime('%m',[DataNascita])=@mese)) ";
                        }

                        if (giorno != "0")
                        {
                            int.TryParse(giorno, out giornoa);

                            SQLiteParameter pargiorno = new SQLiteParameter("@giorno", giornoa); //OleDbType.VarChar
                            parColl.Add(pargiorno);
                            queryfilter += " and (strftime('%d',[DataNascita])=@giorno)) ";
                        }


                    }

                }
                query += queryfilter;

                query += " order by Cognome COLLATE NOCASE asc,Nome COLLATE NOCASE asc ";
                if (pagesize != 0)
                {
                    query += " limit " + (page - 1) * pagesize + "," + pagesize;
                }

                /*CALCOLO IL NUMERO DI RIGHE FILTRATE TOTALI*/
                long totalrecords = dbDataAccess.ExecuteScalar<long>("SELECT count(*) FROM TBL_CLIENTI " + queryfilter, parColl, connection);
                list.Totrecs = totalrecords;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Cliente();
                        item.Id_cliente = reader.GetInt64(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt64(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard"));

                        //Carico tutti i dati della eventuale card associata al cliente
                        item.card.Id_card = item.Id_card;
                        item.card.CodiceCard = item.CodiceCard;
                        if (!reader["AssegnatoACard"].Equals(DBNull.Value))
                            item.card.AssegnatoACard = reader.GetBoolean(reader.GetOrdinal("AssegnatoACard"));
                        if (!reader["DataAttivazione"].Equals(DBNull.Value))
                            item.card.DataAttivazione = reader.GetDateTime(reader.GetOrdinal("DataAttivazione"));
                        if (!reader["DataGenerazione"].Equals(DBNull.Value))
                            item.card.DataGenerazione = reader.GetDateTime(reader.GetOrdinal("DataGenerazione"));
                        if (!reader["DurataGG"].Equals(DBNull.Value))
                            item.card.DurataGG = reader.GetInt64(reader.GetOrdinal("DurataGG"));
                        //--------------------------------------------------------------------
                        if (!reader["Cap"].Equals(DBNull.Value))
                            item.Cap = reader.GetString(reader.GetOrdinal("Cap"));//
                        if (!reader["Cellulare"].Equals(DBNull.Value))
                            item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));//
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));//
                        if (!reader["CodiceNAZIONE"].Equals(DBNull.Value))
                            item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));//
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));//
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));//
                        if (!reader["Cognome"].Equals(DBNull.Value))
                            item.Cognome = reader.GetString(reader.GetOrdinal("Cognome"));//
                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));//
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        if (!reader["Sesso"].Equals(DBNull.Value))
                            item.Sesso = reader.GetString(reader.GetOrdinal("Sesso"));

                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));
                        if (!reader["DataNascita"].Equals(DBNull.Value))
                            item.DataNascita = reader.GetDateTime(reader.GetOrdinal("DataNascita"));//
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        if (!reader["DataInserimento"].Equals(DBNull.Value))
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("Email"));//
                        if (!reader["Emailpec"].Equals(DBNull.Value))
                            item.Emailpec = reader.GetString(reader.GetOrdinal("Emailpec"));
                        if (!reader["Indirizzo"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));//
                        if (!reader["IPclient"].Equals(DBNull.Value))
                            item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        if (!reader["Lingua"].Equals(DBNull.Value))
                            item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));//
                        if (!reader["Nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("Nome"));//
                        if (!reader["Professione"].Equals(DBNull.Value))
                            item.Professione = reader.GetString(reader.GetOrdinal("Professione"));//
                        if (!reader["Spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));//
                        if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        //if (!reader["Spare3"].Equals(DBNull.Value))
                        //    item.Spare3 = reader.GetString(reader.GetOrdinal("Spare3"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));//
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));//
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));//
                        if (!reader["Codicisconto"].Equals(DBNull.Value))
                            item.Codicisconto = reader.GetString(reader.GetOrdinal("Codicisconto"));

                        if (!reader["id_tipi_clienti"].Equals(DBNull.Value))
                            item.id_tipi_clienti = reader.GetInt64(reader.GetOrdinal("id_tipi_clienti")).ToString();
                        if (!reader["Serialized"].Equals(DBNull.Value))
                            item.Serialized = reader.GetString(reader.GetOrdinal("Serialized"));
                        item.addvalues = (!string.IsNullOrEmpty(item.Serialized)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(item.Serialized) : new Cliente();
                        if (!reader["Ragsoc"].Equals(DBNull.Value))
                            item.Ragsoc = reader.GetString(reader.GetOrdinal("Ragsoc"));
                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Clienti :" + error.Message, error);
            }

            return list;
        }


        /// <summary>
        /// CArica le tipologi e di riferimento dei clienti dall'apposita tabella
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public TabrifCollection CaricaTipiClienti(string connection)
        {
            TabrifCollection list = new TabrifCollection();
            if (connection == null || connection == "") return list;

            Tabrif item;
            try
            {
                string query = "";
                query = "SELECT * FROM dbo_TBLRIF_TIPI_CLIENTI  ";
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Tabrif();

                        item.Id = reader.GetInt64(reader.GetOrdinal("ID")).ToString();
                        if (!reader["TipoCliente"].Equals(DBNull.Value))
                            item.Campo1 = reader.GetString(reader.GetOrdinal("TipoCliente"));
                        if (!reader["CodiceTipo"].Equals(DBNull.Value))
                            item.Codice = reader.GetInt64(reader.GetOrdinal("CodiceTipo")).ToString().Trim();


                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Tipi Clienti :" + error.Message, error);
            }

            return list;
        }
        /// <summary>
        /// Inserisce o aggiorna i dati di un cliente nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InserisciAggiornaTipoCliente(string connessione, Tabrif item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            SQLiteParameter p1 = new SQLiteParameter("@TipoCliente", item.Campo1);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p21 = new SQLiteParameter("@CodiceTipo", item.Codice);
            parColl.Add(p21);
            SQLiteParameter p8 = new SQLiteParameter("@Lingua", item.Lingua);//OleDbType.VarChar
            parColl.Add(p8);

            string query = "";
            if (item.Id != "")
            {
                //UPdate
                query = "UPDATE [dbo_TBLRIF_TIPI_CLIENTI] SET TipoCliente=@TipoCliente,CodiceTipo=@CodiceTipo,Lingua=@Lingua ";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO dbo_TBLRIF_TIPI_CLIENTI (TipoCliente,CodiceTipo,Lingua )";
                query += " values ( ";
                query += "@TipoCliente,@CodiceTipo,@Lingua )";
            }

            try
            {
                long retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                if (item.Id == "") item.Id = retID.ToString(); // se era insert memorizzo l'id del cliente appena inserito

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento Cliente :" + error.Message, error);
            }
            return;
        }


        /// <summary>
        /// Inserisce o aggiorna i dati di un cliente nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InserisciAggiornaCliente(string connessione, ref Cliente item)
        {

#if false

            if (!string.IsNullOrEmpty(item.Email))
            {
                //bool validemail = ActiveUp.Net.Mail.Validator.ValidateSyntax(item.Email);
                bool validemail = IsValidEmail(item.Email);
                if (!validemail)
                {
                    //  output.CssClass = "alert alert-danger"; output.Text = "Email errata|Invalid Email!";
                    return;
                }
            } 
#endif

            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            SQLiteParameter p1 = new SQLiteParameter("@Id_card", item.Id_card);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p21 = new SQLiteParameter("@Nome", item.Nome);
            parColl.Add(p21);
            SQLiteParameter p8 = new SQLiteParameter("@Cognome", item.Cognome);//OleDbType.VarChar
            parColl.Add(p8);
            SQLiteParameter p5 = new SQLiteParameter("@CodiceNAZIONE", item.CodiceNAZIONE);//OleDbType.VarChar
            parColl.Add(p5);
            SQLiteParameter p7 = new SQLiteParameter("@CodiceREGIONE", item.CodiceREGIONE);//OleDbType.VarChar
            parColl.Add(p7);
            SQLiteParameter p6 = new SQLiteParameter("@CodicePROVINCIA", item.CodicePROVINCIA);//OleDbType.VarChar
            parColl.Add(p6);
            SQLiteParameter p4 = new SQLiteParameter("@CodiceCOMUNE", item.CodiceCOMUNE);//OleDbType.VarChar
            parColl.Add(p4);
            SQLiteParameter p2 = new SQLiteParameter("@Cap", item.Cap);//OleDbType.VarChar
            parColl.Add(p2);
            SQLiteParameter p18 = new SQLiteParameter("@Indirizzo", item.Indirizzo);
            parColl.Add(p18);
            SQLiteParameter p17 = new SQLiteParameter("@Email", item.Email);
            parColl.Add(p17);
            SQLiteParameter p17b = new SQLiteParameter("@Emailpec", item.Emailpec);
            parColl.Add(p17b);
            SQLiteParameter p25 = new SQLiteParameter("@Telefono", item.Telefono);
            parColl.Add(p25);
            SQLiteParameter p3 = new SQLiteParameter("@Cellulare", item.Cellulare);//OleDbType.VarChar
            parColl.Add(p3);
            SQLiteParameter p22 = new SQLiteParameter("@Professione", item.Professione);
            parColl.Add(p22);
            SQLiteParameter p19 = new SQLiteParameter("@IPclient", item.IPclient);
            parColl.Add(p19);

            SQLiteParameter p27 = new SQLiteParameter("@Validato", item.Validato);
            parColl.Add(p27);
            SQLiteParameter p26 = new SQLiteParameter("@TestoFormConsensi", item.TestoFormConsensi);
            parColl.Add(p26);
            SQLiteParameter p15 = new SQLiteParameter("@DataNascita", dbDataAccess.CorrectDatenow(item.DataNascita));
            parColl.Add(p15);

            SQLiteParameter p14 = null;
            if (item.DataInvioValidazione != null)
                p14 = new SQLiteParameter("@DataInvioValidazione", dbDataAccess.CorrectDatenow(item.DataInvioValidazione.Value));
            else
                p14 = new SQLiteParameter("@DataInvioValidazione", System.DBNull.Value);
            //p14.DbType = System.Data.DbType.DateTime;
            parColl.Add(p14);
            SQLiteParameter p16;
            if (item.DataRicezioneValidazione != null)
                p16 = new SQLiteParameter("@DataRicezioneValidazione", dbDataAccess.CorrectDatenow(item.DataRicezioneValidazione.Value));
            else
                p16 = new SQLiteParameter("@DataRicezioneValidazione", System.DBNull.Value);
            //p16.DbType = System.Data.DbType.DateTime;
            parColl.Add(p16);


            SQLiteParameter p16b;
            if (item.DataInserimento != null)
                p16b = new SQLiteParameter("@DataInserimento", dbDataAccess.CorrectDatenow(item.DataInserimento.Value));
            else
                p16b = new SQLiteParameter("@DataInserimento", System.DBNull.Value);
            if (item.Id_cliente == 0 && item.DataInserimento == null) //Metto la data attuale per nuovo clientie
                p16b.Value = dbDataAccess.CorrectDatenow(System.DateTime.Now);
            parColl.Add(p16b);


            SQLiteParameter p13 = new SQLiteParameter("@ConsensoPrivacy", item.ConsensoPrivacy);//OleDbType.VarChar
            parColl.Add(p13);
            SQLiteParameter p9 = new SQLiteParameter("@Consenso1", item.Consenso1);//OleDbType.VarChar
            parColl.Add(p9);
            SQLiteParameter p10 = new SQLiteParameter("@Consenso2", item.Consenso2);//OleDbType.VarChar
            parColl.Add(p10);
            SQLiteParameter p11 = new SQLiteParameter("@Consenso3", item.Consenso3);//OleDbType.VarChar
            parColl.Add(p11);
            SQLiteParameter p12 = new SQLiteParameter("@Consenso4", item.Consenso4);//OleDbType.VarChar
            parColl.Add(p12);
            SQLiteParameter p20 = new SQLiteParameter("@Lingua", item.Lingua);
            parColl.Add(p20);
            SQLiteParameter p23 = new SQLiteParameter("@Spare1", item.Spare1);
            parColl.Add(p23);
            SQLiteParameter p24 = new SQLiteParameter("@Spare2", item.Spare2);
            parColl.Add(p24);
            SQLiteParameter p28 = new SQLiteParameter("@Pivacf", item.Pivacf);
            parColl.Add(p28);
            SQLiteParameter pses = new SQLiteParameter("@Sesso", item.Sesso);
            parColl.Add(pses);
            if (string.IsNullOrEmpty(item.id_tipi_clienti)) item.id_tipi_clienti = "0";
            SQLiteParameter ptipi = new SQLiteParameter("@id_tipi_clienti", item.id_tipi_clienti); //OleDbType.VarChar
            parColl.Add(ptipi);

            SQLiteParameter pcsco = new SQLiteParameter("@Codicisconto", item.Codicisconto);
            parColl.Add(pcsco);

         
            SQLiteParameter pserial = new SQLiteParameter("@Serialized", item.Serialized);
            parColl.Add(pserial);

            SQLiteParameter pragsoc = new SQLiteParameter("@Ragsoc", item.Ragsoc);
            parColl.Add(pragsoc);

            string query = "";
            if (item.Id_cliente != 0)
            {
                //UPdate
                query = "UPDATE [TBL_CLIENTI] SET Id_card=@Id_card,Nome=@Nome,Cognome=@Cognome,CodiceNAZIONE=@CodiceNAZIONE,CodiceREGIONE=@CodiceREGIONE,CodicePROVINCIA=@CodicePROVINCIA,CodiceCOMUNE=@CodiceCOMUNE";
                query += ",Cap=@Cap,Indirizzo=@Indirizzo,Email=@Email,Emailpec=@Emailpec,Telefono=@Telefono,Cellulare=@Cellulare,Professione=@Professione,IPclient=@IPclient,Validato=@Validato,TestoFormConsensi=@TestoFormConsensi";
                query += ",DataNascita=@DataNascita,DataInvioValidazione=@DataInvioValidazione,DataRicezioneValidazione=@DataRicezioneValidazione,DataInserimento=@DataInserimento";
                query += ",ConsensoPrivacy=@ConsensoPrivacy,Consenso1=@Consenso1,Consenso2=@Consenso2,Consenso3=@Consenso3,Consenso4=@Consenso4,Lingua=@Lingua,Spare1=@Spare1,Spare2=@Spare2,Pivacf=@Pivacf,Sesso = @Sesso,id_tipi_clienti=@id_tipi_clienti,Codicisconto=@Codicisconto,Serialized=@Serialized,Ragsoc=@Ragsoc";
                query += " WHERE [Id_cliente] = " + item.Id_cliente;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_CLIENTI (Id_card,Nome,Cognome,CodiceNAZIONE,CodiceREGIONE,CodicePROVINCIA,CodiceCOMUNE";
                query += ",Cap,Indirizzo,Email,Emailpec,Telefono,Cellulare,Professione,IPclient,Validato,TestoFormConsensi,DataNascita,DataInvioValidazione,DataRicezioneValidazione,DataInserimento";
                query += ",ConsensoPrivacy,Consenso1,Consenso2,Consenso3,Consenso4,Lingua,Spare1,Spare2,Pivacf,Sesso,id_tipi_clienti,Codicisconto,Serialized,Ragsoc)";
                query += " values ( ";
                query += "@Id_card,@Nome,@Cognome,@CodiceNAZIONE,@CodiceREGIONE,@CodicePROVINCIA,@CodiceCOMUNE";
                query += ",@Cap,@Indirizzo,@Email,@Emailpec,@Telefono,@Cellulare,@Professione, @IPClient, @Validato,@TestoFormConsensi,@DataNascita,@DataInvioValidazione,@DataRicezioneValidazione,@DataInserimento";
                query += ",@ConsensoPrivacy,@Consenso1,@Consenso2,@Consenso3,@Consenso4,@Lingua,@Spare1,@Spare2,@Pivacf,@Sesso,@id_tipi_clienti,@Codicisconto,@Serialized,@Ragsoc )";
            }

            try
            {
                long retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                if (item.Id_cliente == 0) item.Id_cliente = retID; // se era insert memorizzo l'id del cliente appena inserito

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento Cliente :" + error.Message, error);
            }
            return;
        }



        /// <summary>
        /// Elimina la sottoscrizione alle mailing del cliente con l'id passato
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="idCliente"></param>
        public void unsubscribeCliente(string connessione, string idCliente)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (idCliente == null || idCliente == "") return;

            SQLiteParameter p27 = new SQLiteParameter("@Validato", false);
            parColl.Add(p27);
            SQLiteParameter p9 = new SQLiteParameter("@Consenso1", false);//OleDbType.VarChar
            parColl.Add(p9);

            //UPdate
            string query = "UPDATE [TBL_CLIENTI] SET  Validato=@Validato,Consenso1=@Consenso1";
            query += " WHERE [Id_cliente] = " + idCliente;

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, unsubscribe Cliente :" + error.Message, error);
            }
            return;
        }



    }
}
