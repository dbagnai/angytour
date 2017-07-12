using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

namespace WelcomeLibrary.DAL
{
    public class eCommerceDM
    {
        /// <summary>
        /// Carica la lista completa degli elementi del carrello in base al SessionID o su Ipclient , restituisce tutta la lista relativa ad un certo ip e sessionid
        /// Escludendo gli elementi relativi a ordini già effettuati(cioè quelli che hanno il codice ordine diverso da vuoto).
        /// Opzionalmente si può scegliere di selezionare un solo codice prodotto
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="SessionId"></param>
        /// <param name="IpClient"></param>
        /// <param name="CodiceProdotto">Opzionale se specificato torna solo l'elemento del carrello col codiceprodotto indicato</param>
        /// <returns></returns>
        public CarrelloCollection CaricaCarrello(string connection, string SessionId, string ipClient, int id_prodotto = 0)
        {
            CarrelloCollection list = new CarrelloCollection();

            if (connection == null || connection == "") return list;
            if (SessionId == null || string.IsNullOrWhiteSpace(SessionId)) return list;
            if (ipClient == null || string.IsNullOrWhiteSpace(ipClient)) return list;

            //dbDataAccess.ComprimiDbAccess(connection);

            Carrello item;
            try
            {
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                string query = "";
                query = "SELECT A.*,B.* FROM TBL_CARRELLO A left outer join TBL_ATTIVITA B on A.id_prodotto=B.Id where SessionId like @SessionId and IpClient like @ipClient and CodiceOrdine = '' ";

                OleDbParameter p1 = new OleDbParameter("@SessionId", SessionId);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbParameter p2 = new OleDbParameter("@ipClient", ipClient);//OleDbType.VarChar
                parColl.Add(p2);
                if (id_prodotto != 0)
                {
                    query += " and A.id_prodotto like @id_prodotto";
                    OleDbParameter p3 = new OleDbParameter("@id_prodotto", id_prodotto);//OleDbType.VarChar
                    parColl.Add(p3);
                }
                query += " order by A.id desc ";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Carrello();
                        item.ID = reader.GetInt32(reader.GetOrdinal("A.ID"));
                        item.IpClient = reader.GetString(reader.GetOrdinal("IpClient"));
                        item.SessionId = reader.GetString(reader.GetOrdinal("SessionId"));


                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.ID_cliente = reader.GetInt32(reader.GetOrdinal("Id_cliente"));


                        if (!reader["A.Codicenazione"].Equals(DBNull.Value))
                            item.Codicenazione = reader.GetString(reader.GetOrdinal("A.Codicenazione"));
                        if (!reader["A.Codiceprovincia"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("A.Codiceprovincia"));
                        if (!reader["Codicesconto"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("Codicesconto"));

                        item.id_prodotto = reader.GetInt32(reader.GetOrdinal("id_prodotto"));
                        if (!reader["A.CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("A.CodiceProdotto"));

                        item.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                        //if (!reader["B.Prezzo"].Equals(DBNull.Value))
                        //    item.Prezzo = reader.GetDouble(reader.GetOrdinal("B.Prezzo"));
                        item.Prezzo = reader.GetDouble(reader.GetOrdinal("A.Prezzo"));

                        item.Iva = reader.GetInt32(reader.GetOrdinal("Iva"));
                        item.Numero = reader.GetInt32(reader.GetOrdinal("Numero"));
                        item.Validita = reader.GetInt32(reader.GetOrdinal("Validita"));

                        if (!reader["Campo1"].Equals(DBNull.Value))
                            item.Campo1 = reader.GetString(reader.GetOrdinal("Campo1"));
                        if (!reader["Campo2"].Equals(DBNull.Value))
                            item.Campo2 = reader.GetString(reader.GetOrdinal("Campo2"));
                        if (!reader["Campo3"].Equals(DBNull.Value))
                            item.Campo3 = reader.GetString(reader.GetOrdinal("Campo3"));

                        if (!reader["CodiceOrdine"].Equals(DBNull.Value))
                            item.CodiceOrdine = reader.GetString(reader.GetOrdinal("CodiceOrdine"));

                        //carichiamo i dati relativi all'offerta scelta
                        //item.Offerta = CaricaDatiOffertaRelativa(connection, item.CodiceProdotto);
                        Offerte offerta = new Offerte();
                        if (!reader["B.ID"].Equals(DBNull.Value))
                        {
                            offerta.Id = reader.GetInt32(reader.GetOrdinal("B.ID"));
                            offerta.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                            offerta.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                            offerta.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                            offerta.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                            offerta.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                            offerta.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                            if (!reader["linkVideo"].Equals(DBNull.Value))
                                offerta.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                            if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                                offerta.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                            if (!reader["B.CodicePROVINCIA"].Equals(DBNull.Value))
                                offerta.CodiceProvincia = reader.GetString(reader.GetOrdinal("A.CodicePROVINCIA"));
                            if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                                offerta.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                            if (!reader["B.CodiceProdotto"].Equals(DBNull.Value))
                                offerta.CodiceProdotto = reader.GetString(reader.GetOrdinal("B.CodiceProdotto"));
                            if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                                offerta.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                            if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                                offerta.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

                            if (!reader["Caratteristica1"].Equals(DBNull.Value))
                                offerta.Caratteristica1 = reader.GetInt32(reader.GetOrdinal("Caratteristica1"));
                            if (!reader["Caratteristica2"].Equals(DBNull.Value))
                                offerta.Caratteristica2 = reader.GetInt32(reader.GetOrdinal("Caratteristica2"));
                            if (!reader["Caratteristica3"].Equals(DBNull.Value))
                                offerta.Caratteristica3 = reader.GetInt32(reader.GetOrdinal("Caratteristica3"));
                            if (!reader["Caratteristica4"].Equals(DBNull.Value))
                                offerta.Caratteristica4 = reader.GetInt32(reader.GetOrdinal("Caratteristica4"));
                            if (!reader["Caratteristica5"].Equals(DBNull.Value))
                                offerta.Caratteristica5 = reader.GetInt32(reader.GetOrdinal("Caratteristica5"));
                            if (!reader["Caratteristica6"].Equals(DBNull.Value))
                                offerta.Caratteristica6 = reader.GetInt32(reader.GetOrdinal("Caratteristica6"));

                            if (!reader["DATITECNICII"].Equals(DBNull.Value))
                                offerta.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                            if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                                offerta.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                            if (!reader["EMAIL"].Equals(DBNull.Value))
                                offerta.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                            if (!reader["FAX"].Equals(DBNull.Value))
                                offerta.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                            if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                                offerta.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                            if (!reader["TELEFONO"].Equals(DBNull.Value))
                                offerta.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                            if (!reader["WEBSITE"].Equals(DBNull.Value))
                                offerta.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));
                            if (!reader["B.Prezzo"].Equals(DBNull.Value))
                                offerta.Prezzo = reader.GetDouble(reader.GetOrdinal("B.Prezzo"));
                            if (!reader["PrezzoListino"].Equals(DBNull.Value))
                                offerta.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                            if (!reader["Vetrina"].Equals(DBNull.Value))
                                offerta.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));

                            if (!(reader["FotoSchema"]).Equals(DBNull.Value))
                                offerta.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("FotoSchema"));
                            else
                                offerta.FotoCollection_M.Schema = "";
                            if (!(reader["FotoValori"]).Equals(DBNull.Value))
                                offerta.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("FotoValori"));
                            else
                                offerta.FotoCollection_M.Valori = "";
                            //Creo la lista delle foto
                            offerteDM offDm = new offerteDM();
                            offerta.FotoCollection_M = offDm.CaricaAllegatiFoto(offerta.FotoCollection_M);
                        }
                        //quindi affidiamo l'offerta al prodotto
                        item.Offerta = offerta;

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Contenuti Carrello per SessionID :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Carica la lista completa degli elementi del carrello su codiceordine, 
        /// restituisce tutta la lista.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Codiceordine"></param>
        /// <returns></returns>
        public CarrelloCollection CaricaCarrelloPerCodiceOrdine(string connection, string Codiceordine)
        {
            if (connection == null || connection == "") return null;
            if (Codiceordine == null || string.IsNullOrWhiteSpace(Codiceordine)) return null;

            //dbDataAccess.ComprimiDbAccess(connection);

            CarrelloCollection list = new CarrelloCollection();
            Carrello item;
            try
            {
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                string query = "";
                query = "SELECT A.*,B.* FROM TBL_CARRELLO A left outer join TBL_ATTIVITA B on A.id_prodotto=B.Id where CodiceOrdine = @Codiceordine ";

                OleDbParameter p1 = new OleDbParameter("@Codiceordine", Codiceordine);//OleDbType.VarChar
                parColl.Add(p1);
                query += " order by A.id desc ";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Carrello();
                        item.ID = reader.GetInt32(reader.GetOrdinal("A.ID"));
                        item.IpClient = reader.GetString(reader.GetOrdinal("IpClient"));
                        item.SessionId = reader.GetString(reader.GetOrdinal("SessionId"));

                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.ID_cliente = reader.GetInt32(reader.GetOrdinal("Id_cliente"));

                        if (!reader["A.Codicenazione"].Equals(DBNull.Value))
                            item.Codicenazione = reader.GetString(reader.GetOrdinal("A.Codicenazione"));
                        if (!reader["A.Codiceprovincia"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("A.Codiceprovincia"));
                        if (!reader["Codicesconto"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("Codicesconto"));

                        item.id_prodotto = reader.GetInt32(reader.GetOrdinal("id_prodotto"));
                        if (!reader["A.CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("A.CodiceProdotto"));

                        item.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                        //if (!reader["B.Prezzo"].Equals(DBNull.Value))
                        //    item.Prezzo = reader.GetDouble(reader.GetOrdinal("B.Prezzo"));
                        item.Prezzo = reader.GetDouble(reader.GetOrdinal("A.Prezzo"));

                        item.Iva = reader.GetInt32(reader.GetOrdinal("Iva"));
                        item.Numero = reader.GetInt32(reader.GetOrdinal("Numero"));
                        item.Validita = reader.GetInt32(reader.GetOrdinal("Validita"));

                        if (!reader["Campo1"].Equals(DBNull.Value))
                            item.Campo1 = reader.GetString(reader.GetOrdinal("Campo1"));
                        if (!reader["Campo2"].Equals(DBNull.Value))
                            item.Campo2 = reader.GetString(reader.GetOrdinal("Campo2"));
                        if (!reader["Campo3"].Equals(DBNull.Value))
                            item.Campo3 = reader.GetString(reader.GetOrdinal("Campo3"));

                        if (!reader["CodiceOrdine"].Equals(DBNull.Value))
                            item.CodiceOrdine = reader.GetString(reader.GetOrdinal("CodiceOrdine"));


                        //carichiamo i dati relativi all'offerta scelta
                        //item.Offerta = CaricaDatiOffertaRelativa(connection, item.CodiceProdotto);
                        Offerte offerta = new Offerte();
                        if (!reader["B.ID"].Equals(DBNull.Value))
                        {
                            offerta.Id = reader.GetInt32(reader.GetOrdinal("B.ID"));
                              offerta.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                            offerta.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                            offerta.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                            offerta.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                            offerta.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                            offerta.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                            if (!reader["linkVideo"].Equals(DBNull.Value))
                                offerta.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                            if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                                offerta.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                            if (!reader["B.CodicePROVINCIA"].Equals(DBNull.Value))
                                offerta.CodiceProvincia = reader.GetString(reader.GetOrdinal("A.CodicePROVINCIA"));
                            if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                                offerta.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                            if (!reader["B.CodiceProdotto"].Equals(DBNull.Value))
                                offerta.CodiceProdotto = reader.GetString(reader.GetOrdinal("B.CodiceProdotto"));
                            if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                                offerta.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                            if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                                offerta.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));
                            if (!reader["Caratteristica1"].Equals(DBNull.Value))
                                offerta.Caratteristica1 = reader.GetInt32(reader.GetOrdinal("Caratteristica1"));
                            if (!reader["Caratteristica2"].Equals(DBNull.Value))
                                offerta.Caratteristica2 = reader.GetInt32(reader.GetOrdinal("Caratteristica2"));
                            if (!reader["Caratteristica3"].Equals(DBNull.Value))
                                offerta.Caratteristica3 = reader.GetInt32(reader.GetOrdinal("Caratteristica3"));
                            if (!reader["Caratteristica4"].Equals(DBNull.Value))
                                offerta.Caratteristica4 = reader.GetInt32(reader.GetOrdinal("Caratteristica4"));
                            if (!reader["Caratteristica5"].Equals(DBNull.Value))
                                offerta.Caratteristica5 = reader.GetInt32(reader.GetOrdinal("Caratteristica5"));
                            if (!reader["Caratteristica6"].Equals(DBNull.Value))
                                offerta.Caratteristica6 = reader.GetInt32(reader.GetOrdinal("Caratteristica6"));

                            if (!reader["DATITECNICII"].Equals(DBNull.Value))
                                offerta.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                            if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                                offerta.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                            if (!reader["EMAIL"].Equals(DBNull.Value))
                                offerta.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                            if (!reader["FAX"].Equals(DBNull.Value))
                                offerta.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                            if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                                offerta.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                            if (!reader["TELEFONO"].Equals(DBNull.Value))
                                offerta.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                            if (!reader["WEBSITE"].Equals(DBNull.Value))
                                offerta.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));
                            if (!reader["B.Prezzo"].Equals(DBNull.Value))
                                offerta.Prezzo = reader.GetDouble(reader.GetOrdinal("B.Prezzo"));

                            if (!reader["PrezzoListino"].Equals(DBNull.Value))
                                offerta.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                            if (!reader["Vetrina"].Equals(DBNull.Value))
                                offerta.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));

                            if (!(reader["FotoSchema"]).Equals(DBNull.Value))
                                offerta.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("FotoSchema"));
                            else
                                offerta.FotoCollection_M.Schema = "";
                            if (!(reader["FotoValori"]).Equals(DBNull.Value))
                                offerta.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("FotoValori"));
                            else
                                offerta.FotoCollection_M.Valori = "";
                            //Creo la lista delle foto
                            offerteDM offDm = new offerteDM();
                            offerta.FotoCollection_M = offDm.CaricaAllegatiFoto(offerta.FotoCollection_M);
                        }
                        //quindi affidiamo l'offerta al prodotto
                        item.Offerta = offerta;

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Contenuti Carrello per Codice Ordine :" + error.Message, error);
            }

            return list;
        }


        /// <summary>
        /// Conta il nuomero di prodotti nel carrello per la sessione dall'ip corrente
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="SessionId"></param>
        /// <param name="ipClient"></param>
        /// <returns></returns>
        public int ContaProdottiCarrello(string connection, string SessionId, string ipClient)
        {
            if (connection == null || connection == "") return 0;
            if (SessionId == null || string.IsNullOrWhiteSpace(SessionId)) return 0;
            if (ipClient == null || string.IsNullOrWhiteSpace(ipClient)) return 0;
            int ret = 0;
            try
            {
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                string query = "";
                query = "SELECT count(*) as prodotti FROM TBL_CARRELLO where SessionId like @SessionId and IpClient like @ipClient and CodiceOrdine = '' ";

                OleDbParameter p1 = new OleDbParameter("@SessionId", SessionId);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbParameter p2 = new OleDbParameter("@ipClient", ipClient);//OleDbType.VarChar
                parColl.Add(p2);

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return 0; };
                    if (reader.HasRows == false)
                        return 0;

                    while (reader.Read())
                    {
                        ret = reader.GetInt32(reader.GetOrdinal("prodotti"));
                    }
                }

            }
            catch
            {
                //hrow new ApplicationException("Errore Caricamento Contenuti Carrello per SessionID :" + error.Message, error);
            }

            return ret;
        }

        /// <summary>
        /// Carica un elemento specifico (prodotto) del carrello in base al ID passato
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Carrello CaricaCarrelloPerId(string connection, string ID)
        {
            if (connection == null || connection == "") return null;
            if (ID == null || ID == "") return null;
            Carrello item = null;

            try
            {
                string query = "SELECT * FROM TBL_CARRELLO where ID=@ID";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@ID", ID);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Carrello();
                        item.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.IpClient = reader.GetString(reader.GetOrdinal("IpClient"));
                        item.SessionId = reader.GetString(reader.GetOrdinal("SessionId"));
                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.ID_cliente = reader.GetInt32(reader.GetOrdinal("Id_cliente"));
                        if (!reader["Codicenazione"].Equals(DBNull.Value))
                            item.Codicenazione = reader.GetString(reader.GetOrdinal("Codicenazione"));
                        if (!reader["Codiceprovincia"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("Codiceprovincia"));
                        if (!reader["Codicesconto"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("Codicesconto"));
                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        item.id_prodotto = reader.GetInt32(reader.GetOrdinal("id_prodotto"));
                        item.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));
                        item.Iva = reader.GetInt32(reader.GetOrdinal("Iva"));
                        item.Numero = reader.GetInt32(reader.GetOrdinal("Numero"));
                        item.Validita = reader.GetInt32(reader.GetOrdinal("Validita"));
                        if (!reader["Campo1"].Equals(DBNull.Value))
                            item.Campo1 = reader.GetString(reader.GetOrdinal("Campo1"));
                        if (!reader["Campo2"].Equals(DBNull.Value))
                            item.Campo2 = reader.GetString(reader.GetOrdinal("Campo2"));
                        if (!reader["Campo3"].Equals(DBNull.Value))
                            item.Campo3 = reader.GetString(reader.GetOrdinal("Campo3"));
                        if (!reader["CodiceOrdine"].Equals(DBNull.Value))
                            item.CodiceOrdine = reader.GetString(reader.GetOrdinal("CodiceOrdine"));

                        offerteDM offDm = new offerteDM();
                        item.Offerta = offDm.CaricaOffertaPerId(connection, item.id_prodotto.ToString());

                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Carrello per Codice ID :" + error.Message, error);
            }

            return item;
        }

        /// <summary>
        /// Carica l'ultimo codice ordine generato dalla tabella carrello
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CaricaCodiceUltimoOrdine(string connection)
        {
            if (connection == null || connection == "") return null;
            CarrelloCollection list = new CarrelloCollection();
            Carrello item = new Carrello();
            string CodiceOrdine = "";

            try
            {
                //string query = "SELECT * FROM TBL_CARRELLO order BY CodiceOrdine Desc";
                string query = "SELECT TOP 1 * FROM TBL_CARRELLO ORDER BY CodiceOrdine DESC,id desc";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Carrello();
                        item.CodiceOrdine = reader.GetString(reader.GetOrdinal("CodiceOrdine"));


                    }
                }

                //controllo che non sia vuoto
                if (!string.IsNullOrEmpty(item.CodiceOrdine.ToString()))
                {
                    CodiceOrdine = item.CodiceOrdine;
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Ultimo Codice Ordine Inserito :" + error.Message, error);
            }

            return CodiceOrdine;
        }

        /// <summary>
        /// Torna true se esiste il codice ordine indicato altrimenti false
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codiceordine"></param>
        /// <returns></returns>
        public bool VerificaPresenzaCodiceOrdine(string connection, string codiceordine)
        {
            bool ret = true;

            if (connection == null || connection == "") return ret;
            CarrelloCollection list = new CarrelloCollection();
            Carrello item = new Carrello();
            //string CodiceOrdine = "";

            try
            {

                string query = "SELECT * FROM TBL_CARRELLO where CodiceOrdine=@CodiceOrdine";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@CodiceOrdine", codiceordine);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new Carrello();
                        item.CodiceOrdine = reader.GetString(reader.GetOrdinal("CodiceOrdine"));
                        ret = true;

                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Ultimo Codice Ordine Inserito :" + error.Message, error);
            }

            return ret;
        }

        /// <summary>
        /// Funzione che Fa inserimento e aggiornamento in tabella carrello prodotti
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertUpdateCarrello(string connessione, Carrello item, bool aggiungiavalori = true)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            //Per prima cosa svuotiamo il carrello dagli elementi scaduti
            SvuotaCarrello(connessione, item);

            //Controlliamo se è presente un codice prodotto caricando il carrello presente per la sessione/ip del client collegato
            CarrelloCollection ListaCarrello = CaricaCarrello(connessione, item.SessionId, item.IpClient);
            if (ListaCarrello.Count() > 0)
            {
                //Si suppone di non avere nel carrello doppioni con stesso codice prodotto
                Carrello itemdb = ListaCarrello.Find(delegate(Carrello _c) { return _c.id_prodotto == item.id_prodotto; });

                if (itemdb != null && itemdb.id_prodotto != 0)
                {
                    itemdb.ID_cliente = item.ID_cliente;//Permetto l'aggiornamento dell'id del cliente
                    if (aggiungiavalori == true)
                        //aumento la quantità del valore passato a quella presente nel db 
                        itemdb.Numero += item.Numero;
                    else//Sovrascrivo la quantità nel db con quella passata
                        itemdb.Numero = item.Numero;
                    UpdateCarrello(connessione, itemdb);
                }
                else
                {
                    //se non trovo il ocdice prodotto all'interno del carrello, faccio un insert
                    InsertCarrello(connessione, item);
                }
            }
            else
            {
                //se non trovo il ocdice prodotto all'interno del carrello, faccio un insert
                InsertCarrello(connessione, item);
            }

        }

        /// <summary>
        /// Funzione che cancella tutti gli elementi che sono scaduti dal carrello
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void SvuotaCarrello(string connessione, Carrello item)
        {
            CardCollection list = new CardCollection();
            if (connessione == null || connessione == "") return;
            if (item.SessionId == null || item.SessionId == "") return;
            if (item.IpClient == null || item.IpClient == "") return;

            List<OleDbParameter> parColl = new List<OleDbParameter>();

            OleDbParameter p1 = new OleDbParameter("@dataoggi", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", System.DateTime.Now));
            //p1.OleDbType = OleDbType.Date;
            parColl.Add(p1);
            //OleDbParameter p2 = new OleDbParameter("@SessionId", item.SessionId);
            //parColl.Add(p2);
            //OleDbParameter p3 = new OleDbParameter("@IpClient", item.IpClient);
            //parColl.Add(p3);

            string query = "DELETE * FROM TBL_CARRELLO ";
            string where = "";
            if (string.IsNullOrWhiteSpace(where))
                //query += " WHERE Data is not null and (Data+Validita) < @dataoggi and CodiceOrdine='' ";
                query += " WHERE Data is not null and DateDiff('d', @dataoggi, DateAdd('d', Validita, Data)) <= 0 and CodiceOrdine='' ";


            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione Elemento Scaduto dal carrello :" + error.Message, error);
            }


            return;
        }

        /// <summary>
        /// Inserisco un elemento nel carrello 
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertCarrello(string connessione, Carrello item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            OleDbParameter p1 = new OleDbParameter("@SessionId", item.SessionId);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@Prezzo", item.Prezzo);
            parColl.Add(p2);
            OleDbParameter p3 = new OleDbParameter("@Data", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", System.DateTime.Now));
            parColl.Add(p3);
            OleDbParameter p4 = new OleDbParameter("@Iva", item.Iva);
            parColl.Add(p4);
            OleDbParameter p5 = new OleDbParameter("@Numero", item.Numero);
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@CodiceProdotto", item.CodiceProdotto);
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@IpClient", item.IpClient);
            parColl.Add(p7);
            OleDbParameter p8 = new OleDbParameter("@Validita", item.Validita);
            parColl.Add(p8);
            OleDbParameter p9 = new OleDbParameter("@CodiceOrdine", item.CodiceOrdine);
            parColl.Add(p9);
            OleDbParameter pidprod = new OleDbParameter("@id_prodotto", item.id_prodotto);
            parColl.Add(pidprod);
            OleDbParameter pidcliente = new OleDbParameter("@idcliente", item.ID_cliente);// 
            parColl.Add(pidcliente);
            OleDbParameter pnazione = new OleDbParameter("@Codicenazione", item.Codicenazione);// 
            parColl.Add(pnazione);
            OleDbParameter pprovincia = new OleDbParameter("@Codiceprovincia", item.Codiceprovincia);// 
            parColl.Add(pprovincia);
            OleDbParameter pcodicesconto = new OleDbParameter("@Codicesconto", item.Codicesconto);// 
            parColl.Add(pcodicesconto);

            string query = "INSERT INTO TBL_CARRELLO([SessionId],[Prezzo],[Data],[Iva],[Numero],[CodiceProdotto],[IpClient],[Validita],[CodiceOrdine],[id_prodotto],[ID_cliente],[Codicenazione],[Codiceprovincia],[Codicesconto]) VALUES (@SessionId,@Prezzo,@Data,@Iva,@Numero,@CodiceProdotto,@IpClient,@Validita,@CodiceOrdine,@id_prodotto,@idcliente,@Codicenazione,@Codiceprovincia,@Codicesconto)";
            try
            {
                int lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                item.ID = lastidentity; //Inserisco nell'id dell'elemento inseito l'id generato dal db
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento elemento nel carrello :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Update elemento Carello
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateCarrello(string connessione, Carrello item)
        {
            //string CodiceOrdineTemporaneo = "";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (item == null || item.id_prodotto == 0 || item.ID == 0) return;
            //if (string.IsNullOrEmpty(item.CodiceOrdine))
            //{
            //    CodiceOrdineTemporaneo = "ravtemporaneo";
            //}
            //else
            //{
            //    CodiceOrdineTemporaneo = item.CodiceOrdine;
            //}

            if (item.Numero > 0)
            {
                OleDbParameter p2 = new OleDbParameter("@Data", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", System.DateTime.Now));
                parColl.Add(p2);
                OleDbParameter p9 = new OleDbParameter("@Numero", item.Numero);
                parColl.Add(p9);
                OleDbParameter p3 = new OleDbParameter("@CodiceOrdine", item.CodiceOrdine);//OleDbType.VarChar
                parColl.Add(p3);


                OleDbParameter p4 = new OleDbParameter("@Campo1", item.Campo1);//lo uso per l'email del cliente che fà l'ordine
                parColl.Add(p4);

                OleDbParameter p5 = new OleDbParameter("@Campo2", item.Campo2);// 
                parColl.Add(p5);

                OleDbParameter p6 = new OleDbParameter("@Campo3", item.Campo3);// 
                parColl.Add(p6);

                OleDbParameter pidcliente = new OleDbParameter("@idcliente", item.ID_cliente);// 
                parColl.Add(pidcliente);
                OleDbParameter pnazione = new OleDbParameter("@Codicenazione", item.Codicenazione);// 
                parColl.Add(pnazione);
                OleDbParameter pprovincia = new OleDbParameter("@Codiceprovincia", item.Codiceprovincia);// 
                parColl.Add(pprovincia);
                OleDbParameter pcodicesconto = new OleDbParameter("@Codicesconto", item.Codicesconto);// 
                parColl.Add(pcodicesconto);

                OleDbParameter p1 = new OleDbParameter("@ID", item.ID);//OleDbType.VarChar
                parColl.Add(p1);


                string query = "UPDATE [TBL_CARRELLO] SET [Data]=@Data,Numero=@Numero,CodiceOrdine=@CodiceOrdine,Campo1=@Campo1,Campo2=@Campo2,Campo3=@Campo3,ID_cliente=@ID_cliente,Codicenazione=@Codicenazione,Codiceprovincia=@Codiceprovincia ,Codicesconto=@Codicesconto WHERE ([ID]=@ID)";
                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento Contenuti :" + error.Message, error);
                }

            }
            else
                DeleteCarrelloPerID(connessione, item.ID);
            return;
        }

        /// <summary>
        /// Cancellare un elemento dal carrello passandogli l'ID
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void DeleteCarrelloPerID(string connessione, int ID)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (ID == null || ID == 0) return;

            OleDbParameter p1 = new OleDbParameter("@ID", ID);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "DELETE * FROM TBL_CARRELLO WHERE ([ID]=@ID)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione Elemento dal carrello :" + error.Message, error);
            }
            return;
        }


        /// <summary>
        /// Modifica il campo nazione nel carrello
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateCarrelloPerListaID(string connessione, string codicenazione, string listaid)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (listaid == null || listaid == "") return;

            OleDbParameter p1 = new OleDbParameter("@codicenazione", codicenazione);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "UPDATE TBL_CARRELLO SET Codicenazione = @codicenazione WHERE [ID] in " + listaid;
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornament Elemento dal carrello :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Modifica il campo nazione nel carrello
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateCarrelloSessionidPerListaID(string connessione, string sessionid, string listaid)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (listaid == null || listaid == "") return;

            OleDbParameter p1 = new OleDbParameter("@Sessionid", sessionid);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "UPDATE TBL_CARRELLO SET Sessionid = @Sessionid WHERE [ID] in " + listaid;
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornamento Elemento dal carrello :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// CArica l'intera lista degli ordini per data discendente
        /// opp
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public TotaliCarrelloCollection CaricaListaOrdini(string connection, List<OleDbParameter> parColl, string maxrecord = "", bool caricacarrelloitems = false)
        {
            TotaliCarrelloCollection list = new TotaliCarrelloCollection();

            if (connection == null || connection == "") return list;
            //   if (Codiceordine == null || string.IsNullOrWhiteSpace(Codiceordine)) return null;
            TotaliCarrello item = null;
            try
            {

                string query = "";
                List<OleDbParameter> _parUsed = new List<OleDbParameter>();

                if (string.IsNullOrEmpty(maxrecord))
                    query = "SELECT * FROM TBL_CARRELLO_ORDINI  ";
                else
                    query = "SELECT  TOP " + maxrecord + " * FROM TBL_CARRELLO_ORDINI ";

                //if (caricacarrelloitems)
                //    query += " left join TBL_CARRELLO B on Codiceordine=B.Codiceordine ";

                if (parColl.Exists(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@Id_cliente"; }))
                {
                    OleDbParameter pidcliente = parColl.Find(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@Id_cliente"; });
                    _parUsed.Add(pidcliente);
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE Id_cliente = @Id_cliente ";
                    else
                        query += " AND Id_cliente = @Id_cliente  ";
                }


                if (parColl.Exists(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@Mailcliente"; }))
                {
                    OleDbParameter pmailcliente = parColl.Find(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@Mailcliente"; });
                    _parUsed.Add(pmailcliente);
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE Mailcliente like @Mailcliente ";
                    else
                        query += " AND Mailcliente like @Mailcliente  ";
                }

                if (parColl.Exists(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@Codiceordine"; }))
                {
                    OleDbParameter pCodiceordine = parColl.Find(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@Codiceordine"; });
                    _parUsed.Add(pCodiceordine);
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE Codiceordine like @Codiceordine ";
                    else
                        query += " AND Codiceordine like @Codiceordine  ";
                }


                if (parColl.Exists(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@DataMin"; }))
                {
                    OleDbParameter pDataMin = parColl.Find(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@DataMin"; });
                    _parUsed.Add(pDataMin);
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE DataOrdine >= @DataMin ";
                    else
                        query += " AND DataOrdine >= @DataMin ";
                }
                if (parColl.Exists(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@DataMax"; }))
                {
                    OleDbParameter pDataMax = parColl.Find(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@DataMax"; });
                    _parUsed.Add(pDataMax);
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE DataOrdine <= @DataMax ";
                    else
                        query += " AND DataOrdine <= @DataMax ";
                }

                //OleDbParameter p1 = new OleDbParameter("@Codiceordine", Codiceordine);//OleDbType.VarChar
                //parColl.Add(p1);
                query += " order by Dataordine desc, Id desc ";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new TotaliCarrello();
                        item.Id = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!reader["Indirizzofatturazione"].Equals(DBNull.Value))
                            item.Indirizzofatturazione = reader.GetString(reader.GetOrdinal("Indirizzofatturazione"));

                        if (!reader["Indirizzospedizione"].Equals(DBNull.Value))
                            item.Indirizzospedizione = reader.GetString(reader.GetOrdinal("Indirizzospedizione"));

                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.Id_cliente = reader.GetInt32(reader.GetOrdinal("Id_cliente"));


                        if (!reader["Mailcliente"].Equals(DBNull.Value))
                            item.Mailcliente = reader.GetString(reader.GetOrdinal("Mailcliente"));

                        if (!reader["Modalitapagamento"].Equals(DBNull.Value))
                            item.Modalitapagamento = reader.GetString(reader.GetOrdinal("Modalitapagamento"));

                        if (!reader["Note"].Equals(DBNull.Value))
                            item.Note = reader.GetString(reader.GetOrdinal("Note"));

                        if (!reader["Urlpagamento"].Equals(DBNull.Value))
                            item.Urlpagamento = reader.GetString(reader.GetOrdinal("Urlpagamento"));

                        if (!reader["CodiceOrdine"].Equals(DBNull.Value))
                            item.CodiceOrdine = reader.GetString(reader.GetOrdinal("CodiceOrdine"));

                        if (!reader["Denominazionecliente"].Equals(DBNull.Value))
                            item.Denominazionecliente = reader.GetString(reader.GetOrdinal("Denominazionecliente"));

                        if (!reader["Pagato"].Equals(DBNull.Value))
                            item.Pagato = reader.GetBoolean(reader.GetOrdinal("Pagato"));

                        if (!reader["TotaleOrdine"].Equals(DBNull.Value))
                            item.TotaleOrdine = reader.GetDouble(reader.GetOrdinal("TotaleOrdine"));

                        if (!reader["TotaleSconto"].Equals(DBNull.Value))
                            item.TotaleSconto = reader.GetDouble(reader.GetOrdinal("TotaleSconto"));
                        if (!reader["TotaleSpedizione"].Equals(DBNull.Value))
                            item.TotaleSpedizione = reader.GetDouble(reader.GetOrdinal("TotaleSpedizione"));


                        if (!reader["TotaleSmaltimento"].Equals(DBNull.Value))
                            item.TotaleSmaltimento = reader.GetDouble(reader.GetOrdinal("TotaleSmaltimento"));
                        if (!reader["Supplementospedizione"].Equals(DBNull.Value))
                            item.Supplementospedizione = reader.GetBoolean(reader.GetOrdinal("Supplementospedizione"));



                        if (!reader["Dataordine"].Equals(DBNull.Value))
                            item.Dataordine = reader.GetDateTime(reader.GetOrdinal("Dataordine"));

                        
                        list.Add(item); //Elemento unico in base al codiceordine
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Lista Ordini :" + error.Message, error);
            }

            return list;
        }



        /// <summary>
        /// Carica la lista completa degli elementi del carrello in base al SessionID o su Ipclient , restituisce tutta la lista relativa ad un certo ip e sessionid
        /// Escludendo gli elementi relativi a ordini già effettuati(cioè quelli che hanno il codice ordine diverso da vuoto).
        /// Opzionalmente si può scegliere di selezionare un solo codice prodotto
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="SessionId"></param>
        /// <param name="IpClient"></param>
        /// <param name="CodiceProdotto">Opzionale se specificato torna solo l'elemento del carrello col codiceprodotto indicato</param>
        /// <returns></returns>
        public TotaliCarrello CaricaOrdinePerCodiceOrdine(string connection, string Codiceordine)
        {
            if (connection == null || connection == "") return null;
            if (Codiceordine == null || string.IsNullOrWhiteSpace(Codiceordine)) return null;

            TotaliCarrello item = null;
            try
            {
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                string query = "";
                query = "SELECT * FROM TBL_CARRELLO_ORDINI where CodiceOrdine = @Codiceordine ";

                OleDbParameter p1 = new OleDbParameter("@Codiceordine", Codiceordine);//OleDbType.VarChar
                parColl.Add(p1);
                query += " order by id desc ";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new TotaliCarrello();
                        item.Id = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!reader["Indirizzofatturazione"].Equals(DBNull.Value))
                            item.Indirizzofatturazione = reader.GetString(reader.GetOrdinal("Indirizzofatturazione"));

                        if (!reader["Indirizzospedizione"].Equals(DBNull.Value))
                            item.Indirizzospedizione = reader.GetString(reader.GetOrdinal("Indirizzospedizione"));

                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.Id_cliente = reader.GetInt32(reader.GetOrdinal("Id_cliente"));


                        if (!reader["Mailcliente"].Equals(DBNull.Value))
                            item.Mailcliente = reader.GetString(reader.GetOrdinal("Mailcliente"));

                        if (!reader["Modalitapagamento"].Equals(DBNull.Value))
                            item.Modalitapagamento = reader.GetString(reader.GetOrdinal("Modalitapagamento"));

                        if (!reader["Note"].Equals(DBNull.Value))
                            item.Note = reader.GetString(reader.GetOrdinal("Note"));

                        if (!reader["Urlpagamento"].Equals(DBNull.Value))
                            item.Urlpagamento = reader.GetString(reader.GetOrdinal("Urlpagamento"));

                        if (!reader["CodiceOrdine"].Equals(DBNull.Value))
                            item.CodiceOrdine = reader.GetString(reader.GetOrdinal("CodiceOrdine"));

                        if (!reader["Denominazionecliente"].Equals(DBNull.Value))
                            item.Denominazionecliente = reader.GetString(reader.GetOrdinal("Denominazionecliente"));

                        if (!reader["Pagato"].Equals(DBNull.Value))
                            item.Pagato = reader.GetBoolean(reader.GetOrdinal("Pagato"));

                        if (!reader["TotaleOrdine"].Equals(DBNull.Value))
                            item.TotaleOrdine = reader.GetDouble(reader.GetOrdinal("TotaleOrdine"));

                        if (!reader["TotaleSconto"].Equals(DBNull.Value))
                            item.TotaleSconto = reader.GetDouble(reader.GetOrdinal("TotaleSconto"));
                        if (!reader["TotaleSpedizione"].Equals(DBNull.Value))
                            item.TotaleSpedizione = reader.GetDouble(reader.GetOrdinal("TotaleSpedizione"));


                        if (!reader["TotaleSmaltimento"].Equals(DBNull.Value))
                            item.TotaleSmaltimento = reader.GetDouble(reader.GetOrdinal("TotaleSmaltimento"));
                        if (!reader["Supplementospedizione"].Equals(DBNull.Value))
                            item.Supplementospedizione = reader.GetBoolean(reader.GetOrdinal("Supplementospedizione"));

                        if (!reader["Dataordine"].Equals(DBNull.Value))
                            item.Dataordine = reader.GetDateTime(reader.GetOrdinal("Dataordine"));

                        break; //Elemento unico in base al codiceordine
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Ordine per Codice Ordine :" + error.Message, error);
            }

            return item;
        }
       
        /// <summary>
        /// Inserisco un elemento nella tabella degli ordini 
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertOrdine(string connessione, TotaliCarrello item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            OleDbParameter p1 = new OleDbParameter("@Indirizzofatturazione", item.Indirizzofatturazione);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@Indirizzospedizione", item.Indirizzospedizione);
            parColl.Add(p2);

            OleDbParameter p3;
            if (item.Dataordine != null)
                p3 = new OleDbParameter("@Dataordine", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", item.Dataordine.Value));
            else
                p3 = new OleDbParameter("@Dataordine", System.DBNull.Value);
            parColl.Add(p3);

            OleDbParameter pidcliente = new OleDbParameter("@Id_cliente", item.Id_cliente);
            parColl.Add(pidcliente);
            OleDbParameter p4 = new OleDbParameter("@Mailcliente", item.Mailcliente);
            parColl.Add(p4);
            OleDbParameter p5 = new OleDbParameter("@Modalitapagamento", item.Modalitapagamento);
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@Note", item.Note);
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@Urlpagamento", item.Urlpagamento);
            parColl.Add(p7);
            OleDbParameter p8 = new OleDbParameter("@CodiceOrdine", item.CodiceOrdine);
            parColl.Add(p8);
            OleDbParameter p9 = new OleDbParameter("@Denominazionecliente", item.Denominazionecliente);
            parColl.Add(p9);
            OleDbParameter ppagato = new OleDbParameter("@Pagato", item.Pagato);
            parColl.Add(ppagato);
            OleDbParameter ptotaleordine = new OleDbParameter("@TotaleOrdine", item.TotaleOrdine);// 
            parColl.Add(ptotaleordine);
            OleDbParameter ptotsconto = new OleDbParameter("@TotaleSconto", item.TotaleSconto);// 
            parColl.Add(ptotsconto);
            OleDbParameter pptotspedizione = new OleDbParameter("@TotaleSpedizione", item.TotaleSpedizione);// 
            parColl.Add(pptotspedizione);


            OleDbParameter pptotsmaltimento = new OleDbParameter("@TotaleSmaltimento", item.TotaleSmaltimento);// 
            parColl.Add(pptotsmaltimento);
            OleDbParameter ppsupplementospedizione = new OleDbParameter("@Supplementospedizione", item.Supplementospedizione);// 
            parColl.Add(ppsupplementospedizione);

            string query = "INSERT INTO TBL_CARRELLO_ORDINI([Indirizzofatturazione],[Indirizzospedizione],[Dataordine],[Id_cliente],[Mailcliente],[Modalitapagamento],[Note],[Urlpagamento],[CodiceOrdine],[Denominazionecliente],[Pagato],[TotaleOrdine],[TotaleSconto],[TotaleSpedizione],TotaleSmaltimento,Supplementospedizione) VALUES (@Indirizzofatturazione,@Indirizzospedizione,@Dataordine,@Id_cliente,@Mailcliente,@Modalitapagamento,@Note,@Urlpagamento,@CodiceOrdine,@Denominazionecliente,@Pagato,@TotaleOrdine,@TotaleSconto,@TotaleSpedizione,@TotaleSmaltimento,@Supplementospedizione)";
            try
            {
                int lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                item.Id = lastidentity; //Inserisco nell'id dell'elemento inseito l'id generato dal db
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento elemento tabella ordini :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Update elemento Carello
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateOrdine(string connessione, TotaliCarrello item)
        {
            //string CodiceOrdineTemporaneo = "";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (item == null || item.CodiceOrdine == "") return;

            OleDbParameter p1 = new OleDbParameter("@Indirizzofatturazione", item.Indirizzofatturazione);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@Indirizzospedizione", item.Indirizzospedizione);
            parColl.Add(p2);
            OleDbParameter p3;
            if (item.Dataordine != null)
                p3 = new OleDbParameter("@Dataordine", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.Dataordine.Value));
            else
                p3 = new OleDbParameter("@Dataordine", System.DBNull.Value);
            parColl.Add(p3);
            OleDbParameter pidcliente = new OleDbParameter("@Id_cliente", item.Id_cliente);
            parColl.Add(pidcliente);
            OleDbParameter p4 = new OleDbParameter("@Mailcliente", item.Mailcliente);
            parColl.Add(p4);
            OleDbParameter p5 = new OleDbParameter("@Modalitapagamento", item.Modalitapagamento);
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@Note", item.Note);
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@Urlpagamento", item.Urlpagamento);
            parColl.Add(p7);
            OleDbParameter p8 = new OleDbParameter("@CodiceOrdine", item.CodiceOrdine);
            parColl.Add(p8);
            OleDbParameter p9 = new OleDbParameter("@Denominazionecliente", item.Denominazionecliente);
            parColl.Add(p9);
            OleDbParameter ppagato = new OleDbParameter("@Pagato", item.Pagato);
            parColl.Add(ppagato);
            OleDbParameter ptotaleordine = new OleDbParameter("@TotaleOrdine", item.TotaleOrdine);// 
            parColl.Add(ptotaleordine);
            OleDbParameter ptotsconto = new OleDbParameter("@TotaleSconto", item.TotaleSconto);// 
            parColl.Add(ptotsconto);
            OleDbParameter pptotspedizione = new OleDbParameter("@TotaleSpedizione", item.TotaleSpedizione);// 
            parColl.Add(pptotspedizione);



            OleDbParameter pptotsmaltimento = new OleDbParameter("@TotaleSmaltimento", item.TotaleSmaltimento);// 
            parColl.Add(pptotsmaltimento);
            OleDbParameter ppsupplementospedizione = new OleDbParameter("@Supplementospedizione", item.Supplementospedizione);// 
            parColl.Add(ppsupplementospedizione);

            OleDbParameter pid = new OleDbParameter("@Id", item.Id);//OleDbType.VarChar
            parColl.Add(pid);

            string query = "UPDATE [TBL_CARRELLO_ORDINI] SET [Indirizzofatturazione]=@Indirizzofatturazione,[Indirizzospedizione]=@Indirizzospedizione,[Dataordine]=@Dataordine,[Id_cliente]=@Id_cliente,[Mailcliente]=@Mailcliente,[Modalitapagamento]=@Modalitapagamento,[Note]=@Note,[Urlpagamento]=@Urlpagamento,[CodiceOrdine]=@CodiceOrdine,[Denominazionecliente]=@Denominazionecliente,[Pagato]=@Pagato,[TotaleOrdine]=@TotaleOrdine,[TotaleSconto]=@TotaleSconto,[TotaleSpedizione]=@TotaleSpedizione,TotaleSmaltimento=@TotaleSmaltimento,Supplementospedizione=@Supplementospedizione WHERE ([ID]=@Id)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornamento Tabella ordini :" + error.Message, error);
            }


            return;
        }

        /// <summary>
        /// Cancellare un elemento dalla tabella ordini passandogli l'ID
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void DeleteOrdinePerID(string connessione, int ID)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (ID == null || ID == 0) return;

            OleDbParameter p1 = new OleDbParameter("@ID", ID);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "DELETE * FROM TBL_CARRELLO_ORDINI WHERE ([ID]=@ID)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione Elemento ordini :" + error.Message, error);
            }
            return;
        }

    }
}
