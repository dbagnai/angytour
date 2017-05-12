using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using System.Data.OleDb;

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
            query = "SELECT A.*,B.CodiceCard AS CodiceCard FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD WHERE A.Email=@Email";


            //if (parColl == null || parColl.Count < 2) return list;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@Email", email); //OleDbType.VarChar
            parColl.Add(p1);
            if (idtipocliente == "") idtipocliente = "0";//metto il cliente di tipo default per la ricerca se non specificato!!

            query += " and id_tipi_clienti=@id_tipi_clienti";
            OleDbParameter p2 = new OleDbParameter("@id_tipi_clienti", idtipocliente); //OleDbType.VarChar
            parColl.Add(p2);


            try
            {

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Cliente();

                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
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
                            if (!reader["Spare2"].Equals(DBNull.Value))
                                item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));
                        item.id_tipi_clienti = reader.GetInt32(reader.GetOrdinal("id_tipi_clienti")).ToString();

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
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@Id_cliente", ID_cliente); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "";
                query = "SELECT A.*,B.CodiceCard AS CodiceCard FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD WHERE A.Id_cliente=@Id_cliente";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Cliente();

                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
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
                            if (!reader["Spare2"].Equals(DBNull.Value))
                                item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));
                        item.id_tipi_clienti = reader.GetInt32(reader.GetOrdinal("id_tipi_clienti")).ToString();

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
        public string CancellaClientiPerTipologia(string connessione, string id_tipi_clienti)
        {
            string idret = "";
            int idtipocliente = 0;
            if (!int.TryParse(id_tipi_clienti, out idtipocliente)) return idret;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;
            if (id_tipi_clienti == null || id_tipi_clienti == "") return idret;

            string query = "DELETE FROM TBL_CLIENTI WHERE ( ID_tipi_clienti = @id_tipi_clienti ) ";
            OleDbParameter p1;
            p1 = new OleDbParameter("@id_tipi_clienti", idtipocliente);
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

        public string CancellaClientePerId(string connessione, int ID_cliente)
        {
            string idret = "";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_CLIENTI WHERE ( ID_CLIENTE = @ID_cliente ) ";
            OleDbParameter p1;
            p1 = new OleDbParameter("@ID_cliente", ID_cliente);
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


                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Cliente();

                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
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
                            if (!reader["Spare2"].Equals(DBNull.Value))
                                item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));

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

                List<OleDbParameter> parColl = new List<OleDbParameter>();

                //Prendendo dai campi del cliente posso inserire i parametri di filtraggio!!!! _paramCliente
                OleDbParameter p1 = new OleDbParameter("@Email", "%");
                if (!string.IsNullOrEmpty(_paramCliente.Email))
                {
                    p1 = new OleDbParameter("@Email", "%" + _paramCliente.Email + "%"); //OleDbType.VarChar
                }
                parColl.Add(p1);
                OleDbParameter p2 = new OleDbParameter("@Validato", _paramCliente.Validato); //OleDbType.VarChar
                parColl.Add(p2);
                OleDbParameter p3 = new OleDbParameter("@Consenso1", _paramCliente.Consenso1); //OleDbType.VarChar
                parColl.Add(p3);

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);


                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Cliente();


                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
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
                            item.card.DurataGG = reader.GetInt32(reader.GetOrdinal("DurataGG"));

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
                            if (!reader["Spare2"].Equals(DBNull.Value))
                                item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
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
        public ClienteCollection CaricaClientiPerGruppoNewsletter(string connection, int idgruppomailing)
        {
            if (connection == null || connection == "") return null;
            if (idgruppomailing == 0) return null;
            int test = 1;
            ClienteCollection List = new ClienteCollection();
            Cliente item = new Cliente();
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@GruppoMailing", idgruppomailing); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT  G.*,C.*  FROM TBL_MAILING_GRUPPI_CLIENTI G LEFT JOIN TBL_CLIENTI C ON G.ID_CLIENTE = c.ID_CLIENTE  WHERE GruppoMailing=@GruppoMailing AND  G.ID_CLIENTE <> 0 ";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Cliente();

                        if (reader["C.ID_CLIENTE"].Equals(DBNull.Value)) continue;//salto i record per cui non trovo l'id del cliente!

                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("C.ID_CLIENTE"));

                        if (!reader["ID_CARD"].Equals(DBNull.Value))
                            item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));

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
                            if (!reader["Spare2"].Equals(DBNull.Value))
                                item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));


                        if (!reader["id_tipi_clienti"].Equals(DBNull.Value))
                            item.id_tipi_clienti = reader.GetInt32(reader.GetOrdinal("id_tipi_clienti")).ToString();
                        test += 1;
                        List.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Cleinti per gruppo newsletter :" + error.Message, error);
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
            string query = "SELECT ID_CLIENTE,Cognome,Nome,Email FROM [TBL_CLIENTI] WHERE Cognome LIKE @testoricerca or Nome LIKE @testoricerca or Email like @testoricerca";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            // parametro
            OleDbParameter p1 = new OleDbParameter("@testoricerca", "%" + testoricerca + "%"); //OleDbType.VarChar
            parColl.Add(p1);
            OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, constr);
            Cliente item;
            using (reader)
            {
                if (reader == null) { return null; };
                if (reader.HasRows == false)
                    return list;
                while (reader.Read())
                {
                    item = new Cliente();
                    item.Id_cliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                    item.Spare3 = "";
                    if (!(reader["Cognome"]).Equals(DBNull.Value))
                        item.Spare3 = reader.GetString(reader.GetOrdinal("Cognome"));//
                    if (!(reader["Nome"]).Equals(DBNull.Value))
                        item.Spare3 += " " + reader.GetString(reader.GetOrdinal("Nome"));//
                    if (!(reader["Email"]).Equals(DBNull.Value))
                        item.Spare3 += " " + reader.GetString(reader.GetOrdinal("Email"));//
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
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("telefono"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("cellulare"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("professione"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("data nascita"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("cap"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("indirizzo"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("piva"));
                    sb.Append(";");

                    sb.Append(WelcomeLibrary.UF.Csv.Escape("consenso email"));



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

                        sb.Append(WelcomeLibrary.UF.Csv.Escape(c.Validato.ToString()));


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
        public ClienteCollection CaricaClientiFiltrati(string connection, Cliente _paramCliente, bool bypassvalidazione = false)
        {
            ClienteCollection list = new ClienteCollection();
            if (connection == null || connection == "") return list;
            //if (parColl == null || parColl.Count < 2) return list;
            Cliente item;
            try
            {
                string query = "";
                query = "SELECT A.*,B.CodiceCard AS CodiceCard, B.DataGenerazione as DataGenerazione, B.DataAttivazione as DataAttivazione, B.DurataGG as DurataGG,B.AssegnatoACard as AssegnatoACard ";
                query += " FROM TBL_CLIENTI A left join TBL_CODICICARD B on A.ID_CARD=B.ID_CARD WHERE Email like @Email ";

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
                    if (!string.IsNullOrWhiteSpace(_paramCliente.id_tipi_clienti))
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

                    if (!string.IsNullOrWhiteSpace(_paramCliente.Sesso))
                    {
                        OleDbParameter psesso = new OleDbParameter("@Sesso", _paramCliente.Sesso); //OleDbType.VarChar
                        parColl.Add(psesso);
                        query += " and Sesso = @Sesso ";
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

                            OleDbParameter datainizio = new OleDbParameter("@Data_inizio", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", di));
                            parColl.Add(datainizio);

                            OleDbParameter datafine = new OleDbParameter("@Data_fine", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", df));
                            parColl.Add(datafine);

                            query += " AND   ( DataNascita >= @Data_inizio and  DataNascita <= @Data_fine )  ";
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
                            OleDbParameter parannoa = new OleDbParameter("@anno", annoa); //OleDbType.VarChar
                            parColl.Add(parannoa);
                            query += " and (((Year([DataNascita]))=@anno)) ";
                        }

                        if (mese != "0")
                        {
                            int.TryParse(mese, out mesea);
                            OleDbParameter parmese = new OleDbParameter("@mese", mesea); //OleDbType.VarChar
                            parColl.Add(parmese);
                            query += " and (((Month([DataNascita]))=@mese)) ";
                        }

                        if (giorno != "0")
                        {
                            int.TryParse(giorno, out giornoa);

                            OleDbParameter pargiorno = new OleDbParameter("@giorno", giornoa); //OleDbType.VarChar
                            parColl.Add(pargiorno);
                            query += " and (((Day([DataNascita]))=@giorno)) ";
                        }


                    }

                }

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);


                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Cliente();


                        item.Id_cliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE"));
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
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
                            item.card.DurataGG = reader.GetInt32(reader.GetOrdinal("DurataGG"));
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
                        if (!reader["Email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("Email"));//
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
                            if (!reader["Spare2"].Equals(DBNull.Value))
                                item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));//
                        if (!reader["Telefono"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));//
                        if (!reader["TestoFormConsensi"].Equals(DBNull.Value))
                            item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));//
                        if (!reader["Pivacf"].Equals(DBNull.Value))
                            item.Pivacf = reader.GetString(reader.GetOrdinal("Pivacf"));//
                        if (!reader["id_tipi_clienti"].Equals(DBNull.Value))
                            item.id_tipi_clienti = reader.GetInt32(reader.GetOrdinal("id_tipi_clienti")).ToString();

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
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Tabrif();

                        item.Id = reader.GetInt32(reader.GetOrdinal("ID")).ToString();
                        if (!reader["TipoCliente"].Equals(DBNull.Value))
                            item.Campo1 = reader.GetString(reader.GetOrdinal("TipoCliente"));
                        if (!reader["CodiceTipo"].Equals(DBNull.Value))
                            item.Codice = reader.GetInt32(reader.GetOrdinal("CodiceTipo")).ToString().Trim();


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
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            OleDbParameter p1 = new OleDbParameter("@TipoCliente", item.Campo1);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p21 = new OleDbParameter("@CodiceTipo", item.Codice);
            parColl.Add(p21);
            OleDbParameter p8 = new OleDbParameter("@Lingua", item.Lingua);//OleDbType.VarChar
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
                int retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
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
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            OleDbParameter p1 = new OleDbParameter("@Id_card", item.Id_card);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p21 = new OleDbParameter("@Nome", item.Nome);
            parColl.Add(p21);
            OleDbParameter p8 = new OleDbParameter("@Cognome", item.Cognome);//OleDbType.VarChar
            parColl.Add(p8);
            OleDbParameter p5 = new OleDbParameter("@CodiceNAZIONE", item.CodiceNAZIONE);//OleDbType.VarChar
            parColl.Add(p5);
            OleDbParameter p7 = new OleDbParameter("@CodiceREGIONE", item.CodiceREGIONE);//OleDbType.VarChar
            parColl.Add(p7);
            OleDbParameter p6 = new OleDbParameter("@CodicePROVINCIA", item.CodicePROVINCIA);//OleDbType.VarChar
            parColl.Add(p6);
            OleDbParameter p4 = new OleDbParameter("@CodiceCOMUNE", item.CodiceCOMUNE);//OleDbType.VarChar
            parColl.Add(p4);
            OleDbParameter p2 = new OleDbParameter("@Cap", item.Cap);//OleDbType.VarChar
            parColl.Add(p2);
            OleDbParameter p18 = new OleDbParameter("@Indirizzo", item.Indirizzo);
            parColl.Add(p18);
            OleDbParameter p17 = new OleDbParameter("@Email", item.Email);
            parColl.Add(p17);
            OleDbParameter p25 = new OleDbParameter("@Telefono", item.Telefono);
            parColl.Add(p25);
            OleDbParameter p3 = new OleDbParameter("@Cellulare", item.Cellulare);//OleDbType.VarChar
            parColl.Add(p3);
            OleDbParameter p22 = new OleDbParameter("@Professione", item.Professione);
            parColl.Add(p22);
            OleDbParameter p19 = new OleDbParameter("@IPclient", item.IPclient);
            parColl.Add(p19);

            OleDbParameter p27 = new OleDbParameter("@Validato", item.Validato);
            parColl.Add(p27);
            OleDbParameter p26 = new OleDbParameter("@TestoFormConsensi", item.TestoFormConsensi);
            parColl.Add(p26);
            OleDbParameter p15 = new OleDbParameter("@DataNascita", item.DataNascita);
            parColl.Add(p15);

            OleDbParameter p14 = null;
            if (item.DataInvioValidazione != null)
                p14 = new OleDbParameter("@DataInvioValidazione", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", item.DataInvioValidazione.Value));
            else
                p14 = new OleDbParameter("@DataInvioValidazione", System.DBNull.Value);
            //p14.DbType = System.Data.DbType.DateTime;
            parColl.Add(p14);
            OleDbParameter p16;
            if (item.DataRicezioneValidazione != null)
                p16 = new OleDbParameter("@DataRicezioneValidazione", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", item.DataRicezioneValidazione.Value));
            else
                p16 = new OleDbParameter("@DataRicezioneValidazione", System.DBNull.Value);
            //p16.DbType = System.Data.DbType.DateTime;
            parColl.Add(p16);

            OleDbParameter p13 = new OleDbParameter("@ConsensoPrivacy", item.ConsensoPrivacy);//OleDbType.VarChar
            parColl.Add(p13);
            OleDbParameter p9 = new OleDbParameter("@Consenso1", item.Consenso1);//OleDbType.VarChar
            parColl.Add(p9);
            OleDbParameter p10 = new OleDbParameter("@Consenso2", item.Consenso2);//OleDbType.VarChar
            parColl.Add(p10);
            OleDbParameter p11 = new OleDbParameter("@Consenso3", item.Consenso3);//OleDbType.VarChar
            parColl.Add(p11);
            OleDbParameter p12 = new OleDbParameter("@Consenso4", item.Consenso4);//OleDbType.VarChar
            parColl.Add(p12);
            OleDbParameter p20 = new OleDbParameter("@Lingua", item.Lingua);
            parColl.Add(p20);
            OleDbParameter p23 = new OleDbParameter("@Spare1", item.Spare1);
            parColl.Add(p23);
            OleDbParameter p24 = new OleDbParameter("@Spare2", item.Spare2);
            parColl.Add(p24);
            OleDbParameter p28 = new OleDbParameter("@Pivacf", item.Pivacf);
            parColl.Add(p28);

            OleDbParameter pses = new OleDbParameter("@Sesso", item.Sesso);
            parColl.Add(pses);

            if (string.IsNullOrEmpty(item.id_tipi_clienti)) item.id_tipi_clienti = "0";
            OleDbParameter ptipi = new OleDbParameter("@id_tipi_clienti", item.id_tipi_clienti); //OleDbType.VarChar
            parColl.Add(ptipi);

            string query = "";
            if (item.Id_cliente != 0)
            {
                //UPdate
                query = "UPDATE [TBL_CLIENTI] SET Id_card=@Id_card,Nome=@Nome,Cognome=@Cognome,CodiceNAZIONE=@CodiceNAZIONE,CodiceREGIONE=@CodiceREGIONE,CodicePROVINCIA=@CodicePROVINCIA,CodiceCOMUNE=@CodiceCOMUNE";
                query += ",Cap=@Cap,Indirizzo=@Indirizzo,Email=@Email,Telefono=@Telefono,Cellulare=@Cellulare,Professione=@Professione,IPclient=@IPclient,Validato=@Validato,TestoFormConsensi=@TestoFormConsensi";
                query += ",DataNascita=@DataNascita,DataInvioValidazione=@DataInvioValidazione,DataRicezioneValidazione=@DataRicezioneValidazione";
                query += ",ConsensoPrivacy=@ConsensoPrivacy,Consenso1=@Consenso1,Consenso2=@Consenso2,Consenso3=@Consenso3,Consenso4=@Consenso4,Lingua=@Lingua,Spare1=@Spare1,Spare2=@Spare2,Pivacf=@Pivacf,Sesso = @Sesso,id_tipi_clienti=@id_tipi_clienti";
                query += " WHERE [Id_cliente] = " + item.Id_cliente;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_CLIENTI (Id_card,Nome,Cognome,CodiceNAZIONE,CodiceREGIONE,CodicePROVINCIA,CodiceCOMUNE";
                query += ",Cap,Indirizzo,Email,Telefono,Cellulare,Professione,IPclient,Validato,TestoFormConsensi,DataNascita,DataInvioValidazione,DataRicezioneValidazione";
                query += ",ConsensoPrivacy,Consenso1,Consenso2,Consenso3,Consenso4,Lingua,Spare1,Spare2,Pivacf,Sesso,id_tipi_clienti)";
                query += " values ( ";
                query += "@Id_card,@Nome,@Cognome,@CodiceNAZIONE,@CodiceREGIONE,@CodicePROVINCIA,@CodiceCOMUNE";
                query += ",@Cap,@Indirizzo,@Email,@Telefono,@Cellulare,@Professione, @IPClient, @Validato,@TestoFormConsensi,@DataNascita,@DataInvioValidazione,@DataRicezioneValidazione";
                query += ",@ConsensoPrivacy,@Consenso1,@Consenso2,@Consenso3,@Consenso4,@Lingua,@Spare1,@Spare2,@Pivacf,@Sesso,@id_tipi_clienti )";
            }

            try
            {
                int retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
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
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (idCliente == null || idCliente == "") return;

            OleDbParameter p27 = new OleDbParameter("@Validato", false);
            parColl.Add(p27);
            OleDbParameter p9 = new OleDbParameter("@Consenso1", false);//OleDbType.VarChar
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
