using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;
using System.Data.SQLite;

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
        /// <param name="CodiceCaratteristica">Opzionale se specificato torna solo l'elemento del carrello col codiceprodotto e codice carrello indicato</param>
        /// <returns></returns>
        public CarrelloCollection CaricaCarrello(string connection, string SessionId, string ipClient, long id_prodotto = 0, string idcombinato = "", long idrecordcarrello = 0)
        {
            CarrelloCollection list = new CarrelloCollection();

            if (connection == null || connection == "") return list;
            if ((SessionId == null || string.IsNullOrWhiteSpace(SessionId)) && idrecordcarrello == 0) return list;
            if ((ipClient == null || string.IsNullOrWhiteSpace(ipClient)) && idrecordcarrello == 0) return list;
            if (idcombinato == null) idcombinato = "";
            //dbDataAccess.ComprimiDbAccess(connection);

            Carrello item;
            try
            {
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                string query = "";
                query = "SELECT A.ID,A.SessionId,A.Data,A.Prezzo,A.Iva,A.Numero,A.CodiceProdotto,A.jsonfield1,A.IpClient,A.Validita,A.Campo1,A.Campo2,A.Campo3,A.Campo4,A.Campo5,A.CodiceOrdine,A.ID_cliente,A.ID_prodotto,A.Codicenazione,A.Codiceprovincia,A.Codicesconto,A.Datastart,A.Dataend,B.ID as B_ID,B.CodiceTIPOLOGIA,B.DataInserimento,B.DescrizioneGB,B.DescrizioneI,B.DENOMINAZIONEGB,B.DENOMINAZIONEI,B.linkVideo,B.CodiceCOMUNE,B.CodicePROVINCIA as B_CodicePROVINCIA,B.CodiceREGIONE,B.CodiceProdotto as B_CodiceProdotto,B.CodiceCategoria,B.CodiceCategoria2Liv,B.Caratteristica1,B.Caratteristica2,B.Caratteristica3,B.Caratteristica4,B.Caratteristica5,B.Caratteristica6,B.Xmlvalue,B.DATITECNICII,B.DATITECNICIGB,B.EMAIL,B.FAX,B.INDIRIZZO,B.TELEFONO,B.WEBSITE,B.Prezzo as B_Prezzo,B.PrezzoListino,B.Vetrina,B.FotoSchema,B.FotoValori   FROM TBL_CARRELLO as A left outer join TBL_ATTIVITA as B on A.id_prodotto=B.Id where SessionId like @SessionId and IpClient like @ipClient and CodiceOrdine = '' ";

                if (idrecordcarrello != 0) { SessionId = "%"; ipClient = "%"; };

                SQLiteParameter p1 = new SQLiteParameter("@SessionId", SessionId);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@ipClient", ipClient);//OleDbType.VarChar
                parColl.Add(p2);

                if (id_prodotto != 0 || !string.IsNullOrEmpty(idcombinato))
                {
                    query += " and A.id_prodotto like @id_prodotto";
                    SQLiteParameter p3 = new SQLiteParameter("@id_prodotto", id_prodotto);//OleDbType.VarChar
                    parColl.Add(p3);
                    query += " and A.campo2 like @campo2";
                    SQLiteParameter p4 = new SQLiteParameter("@campo2", idcombinato);//OleDbType.VarChar
                    parColl.Add(p4);
                }
                //if (!string.IsNullOrEmpty(idcombinato))
                //{
                //    query += " and A.campo2 like @campo2";
                //    SQLiteParameter p4b = new SQLiteParameter("@campo2", idcombinato);//OleDbType.VarChar
                //    parColl.Add(p4b);
                //}

                if (idrecordcarrello != 0)
                {
                    query += " and A.id like @id";
                    SQLiteParameter p5 = new SQLiteParameter("@id", idrecordcarrello);//OleDbType.VarChar
                    parColl.Add(p5);
                }
                query += " order by A.id desc ";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Carrello();
                        item.ID = reader.GetInt64(reader.GetOrdinal("ID"));
                        item.IpClient = reader.GetString(reader.GetOrdinal("IpClient"));
                        item.SessionId = reader.GetString(reader.GetOrdinal("SessionId"));


                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.ID_cliente = reader.GetInt64(reader.GetOrdinal("Id_cliente"));


                        if (!reader["Codicenazione"].Equals(DBNull.Value))
                            item.Codicenazione = reader.GetString(reader.GetOrdinal("Codicenazione"));
                        if (!reader["Codiceprovincia"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("Codiceprovincia"));
                        if (!reader["Codicesconto"].Equals(DBNull.Value))
                            item.Codicesconto = reader.GetString(reader.GetOrdinal("Codicesconto"));

                        item.id_prodotto = reader.GetInt64(reader.GetOrdinal("id_prodotto"));
                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["jsonfield1"].Equals(DBNull.Value))
                            item.jsonfield1 = reader.GetString(reader.GetOrdinal("jsonfield1"));

                        item.Data = reader.GetDateTime(reader.GetOrdinal("Data"));


                        if (!reader["Datastart"].Equals(DBNull.Value))
                            item.Datastart = reader.GetDateTime(reader.GetOrdinal("Datastart"));
                        if (!reader["Dataend"].Equals(DBNull.Value))
                            item.Dataend = reader.GetDateTime(reader.GetOrdinal("Dataend"));

                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));

                        item.Iva = reader.GetInt64(reader.GetOrdinal("Iva"));
                        item.Numero = reader.GetInt64(reader.GetOrdinal("Numero"));
                        item.Validita = reader.GetInt64(reader.GetOrdinal("Validita"));

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
                        if (!reader["B_ID"].Equals(DBNull.Value))
                        {
                            offerta.Id = reader.GetInt64(reader.GetOrdinal("B_ID"));
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
                            if (!reader["B_CodicePROVINCIA"].Equals(DBNull.Value))
                                offerta.CodiceProvincia = reader.GetString(reader.GetOrdinal("B_CodicePROVINCIA"));
                            if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                                offerta.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                            if (!reader["B_CodiceProdotto"].Equals(DBNull.Value))
                                offerta.CodiceProdotto = reader.GetString(reader.GetOrdinal("B_CodiceProdotto"));
                            if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                                offerta.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                            if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                                offerta.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

                            if (!reader["Caratteristica1"].Equals(DBNull.Value))
                                offerta.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                            if (!reader["Caratteristica2"].Equals(DBNull.Value))
                                offerta.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                            if (!reader["Caratteristica3"].Equals(DBNull.Value))
                                offerta.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                            if (!reader["Caratteristica4"].Equals(DBNull.Value))
                                offerta.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                            if (!reader["Caratteristica5"].Equals(DBNull.Value))
                                offerta.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                            if (!reader["Caratteristica6"].Equals(DBNull.Value))
                                offerta.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                            if (!reader["Xmlvalue"].Equals(DBNull.Value))
                                offerta.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));

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
                            if (!reader["B_Prezzo"].Equals(DBNull.Value))
                                offerta.Prezzo = reader.GetDouble(reader.GetOrdinal("B_Prezzo"));
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
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                string query = "";
                query = "SELECT A.ID ,A.SessionId,A.Data,A.Prezzo,A.Iva,A.Numero,A.CodiceProdotto,A.jsonfield1,A.IpClient,A.Validita,A.Campo1,A.Campo2,A.Campo3,A.Campo4,A.Campo5,A.CodiceOrdine,A.ID_cliente,A.ID_prodotto,A.Codicenazione,A.Codiceprovincia,A.Codicesconto,A.Datastart,A.Dataend,B.ID as B_ID,B.CodiceTIPOLOGIA,B.DataInserimento,B.DescrizioneGB,B.DescrizioneI,B.DENOMINAZIONEGB,B.DENOMINAZIONEI,B.linkVideo,B.CodiceCOMUNE,B.CodicePROVINCIA as B_CodicePROVINCIA,B.CodiceREGIONE,B.CodiceProdotto as B_CodiceProdotto,B.CodiceCategoria,B.CodiceCategoria2Liv,B.Caratteristica1,B.Caratteristica2,B.Caratteristica3,B.Caratteristica4,B.Caratteristica5,B.Caratteristica6,B.Xmlvalue,B.DATITECNICII,B.DATITECNICIGB,B.EMAIL,B.FAX,B.INDIRIZZO,B.TELEFONO,B.WEBSITE,B.Prezzo as B_Prezzo,B.PrezzoListino,B.Vetrina,B.FotoSchema,B.FotoValori   FROM TBL_CARRELLO A left outer join TBL_ATTIVITA B on A.id_prodotto=B.Id where CodiceOrdine = @Codiceordine ";

                SQLiteParameter p1 = new SQLiteParameter("@Codiceordine", Codiceordine);//OleDbType.VarChar
                parColl.Add(p1);
                query += " order by A.id desc ";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Carrello();
                        item.ID = reader.GetInt64(reader.GetOrdinal("ID"));
                        item.IpClient = reader.GetString(reader.GetOrdinal("IpClient"));
                        item.SessionId = reader.GetString(reader.GetOrdinal("SessionId"));

                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.ID_cliente = reader.GetInt64(reader.GetOrdinal("Id_cliente"));

                        if (!reader["Codicenazione"].Equals(DBNull.Value))
                            item.Codicenazione = reader.GetString(reader.GetOrdinal("Codicenazione"));
                        if (!reader["Codiceprovincia"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("Codiceprovincia"));
                        if (!reader["Codicesconto"].Equals(DBNull.Value))
                            item.Codicesconto = reader.GetString(reader.GetOrdinal("Codicesconto"));

                        item.id_prodotto = reader.GetInt64(reader.GetOrdinal("id_prodotto"));
                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));

                        if (!reader["jsonfield1"].Equals(DBNull.Value))
                            item.jsonfield1 = reader.GetString(reader.GetOrdinal("jsonfield1"));

                        item.Data = reader.GetDateTime(reader.GetOrdinal("Data"));


                        if (!reader["Datastart"].Equals(DBNull.Value))
                            item.Datastart = reader.GetDateTime(reader.GetOrdinal("Datastart"));
                        if (!reader["Dataend"].Equals(DBNull.Value))
                            item.Dataend = reader.GetDateTime(reader.GetOrdinal("Dataend"));


                        //if (!reader["B.Prezzo"].Equals(DBNull.Value))
                        //    item.Prezzo = reader.GetDouble(reader.GetOrdinal("B.Prezzo"));
                        item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));

                        item.Iva = reader.GetInt64(reader.GetOrdinal("Iva"));
                        item.Numero = reader.GetInt64(reader.GetOrdinal("Numero"));
                        item.Validita = reader.GetInt64(reader.GetOrdinal("Validita"));

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
                        if (!reader["B_ID"].Equals(DBNull.Value))
                        {
                            offerta.Id = reader.GetInt64(reader.GetOrdinal("B_ID"));
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
                            if (!reader["B_CodicePROVINCIA"].Equals(DBNull.Value))
                                offerta.CodiceProvincia = reader.GetString(reader.GetOrdinal("B_CodicePROVINCIA"));
                            if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                                offerta.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                            if (!reader["B_CodiceProdotto"].Equals(DBNull.Value))
                                offerta.CodiceProdotto = reader.GetString(reader.GetOrdinal("B_CodiceProdotto"));
                            if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                                offerta.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                            if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                                offerta.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));
                            if (!reader["Caratteristica1"].Equals(DBNull.Value))
                                offerta.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                            if (!reader["Caratteristica2"].Equals(DBNull.Value))
                                offerta.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                            if (!reader["Caratteristica3"].Equals(DBNull.Value))
                                offerta.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                            if (!reader["Caratteristica4"].Equals(DBNull.Value))
                                offerta.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                            if (!reader["Caratteristica5"].Equals(DBNull.Value))
                                offerta.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                            if (!reader["Caratteristica6"].Equals(DBNull.Value))
                                offerta.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                            if (!reader["Xmlvalue"].Equals(DBNull.Value))
                                offerta.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));

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
                            if (!reader["B_Prezzo"].Equals(DBNull.Value))
                                offerta.Prezzo = reader.GetDouble(reader.GetOrdinal("B_Prezzo"));

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
        public long ContaProdottiCarrello(string connection, string SessionId, string ipClient)
        {
            if (connection == null || connection == "") return 0;
            if (SessionId == null || string.IsNullOrWhiteSpace(SessionId)) return 0;
            if (ipClient == null || string.IsNullOrWhiteSpace(ipClient)) return 0;
            long ret = 0;
            try
            {
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                string query = "";
                query = "SELECT count(*) as prodotti FROM TBL_CARRELLO where SessionId like @SessionId and IpClient like @ipClient and CodiceOrdine = '' ";

                SQLiteParameter p1 = new SQLiteParameter("@SessionId", SessionId);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@ipClient", ipClient);//OleDbType.VarChar
                parColl.Add(p2);

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return 0; };
                    if (reader.HasRows == false)
                        return 0;

                    while (reader.Read())
                    {
                        ret = reader.GetInt64(reader.GetOrdinal("prodotti"));
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
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@ID", ID);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Carrello();
                        item.ID = reader.GetInt64(reader.GetOrdinal("ID"));
                        item.IpClient = reader.GetString(reader.GetOrdinal("IpClient"));
                        item.SessionId = reader.GetString(reader.GetOrdinal("SessionId"));
                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.ID_cliente = reader.GetInt64(reader.GetOrdinal("Id_cliente"));
                        if (!reader["Codicenazione"].Equals(DBNull.Value))
                            item.Codicenazione = reader.GetString(reader.GetOrdinal("Codicenazione"));
                        if (!reader["Codiceprovincia"].Equals(DBNull.Value))
                            item.Codiceprovincia = reader.GetString(reader.GetOrdinal("Codiceprovincia"));
                        if (!reader["Codicesconto"].Equals(DBNull.Value))
                            item.Codicesconto = reader.GetString(reader.GetOrdinal("Codicesconto"));
                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["jsonfield1"].Equals(DBNull.Value))
                            item.jsonfield1 = reader.GetString(reader.GetOrdinal("jsonfield1"));

                        item.id_prodotto = reader.GetInt64(reader.GetOrdinal("id_prodotto"));
                        item.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                        if (!reader["Datastart"].Equals(DBNull.Value))
                            item.Datastart = reader.GetDateTime(reader.GetOrdinal("Datastart"));
                        if (!reader["Dataend"].Equals(DBNull.Value))
                            item.Dataend = reader.GetDateTime(reader.GetOrdinal("Dataend"));


                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));
                        item.Iva = reader.GetInt64(reader.GetOrdinal("Iva"));
                        item.Numero = reader.GetInt64(reader.GetOrdinal("Numero"));
                        item.Validita = reader.GetInt64(reader.GetOrdinal("Validita"));
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
                string query = "SELECT FROM TBL_CARRELLO ORDER BY CodiceOrdine COLLATE NOCASE DESC,id DESC limit 1";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

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
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@CodiceOrdine", codiceordine);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            //Per prima cosa svuotiamo il carrello dagli elementi scaduti
            SvuotaCarrello(connessione, item);

            //Controlliamo se è presente un codice prodotto caricando il carrello presente per la sessione/ip del client collegato
            CarrelloCollection ListaCarrello = CaricaCarrello(connessione, item.SessionId, item.IpClient);
            if (ListaCarrello.Count() > 0)
            {
                //Si suppone di non avere nel carrello doppioni con stesso codice prodotto
                //Carrello itemdb = ListaCarrello.Find(delegate (Carrello _c) { return _c.id_prodotto == item.id_prodotto && _c.Campo2 == item.Campo2; });
                ///Seleziono per id carrello l'elemento
                Carrello itemdb = ListaCarrello.Find(delegate (Carrello _c) { return _c.ID == item.ID; });

                //if (itemdb != null && itemdb.id_prodotto != 0)
                if (itemdb != null && itemdb.ID != 0)
                {
                    itemdb.ID_cliente = item.ID_cliente;//Permetto l'aggiornamento dell'id del cliente
                    itemdb.Codicesconto = item.Codicesconto;//Permetto l'aggiornamento del codice sconto
                    itemdb.Datastart = item.Datastart;
                    itemdb.Dataend = item.Dataend;
                    itemdb.Prezzo = item.Prezzo;
                    itemdb.jsonfield1 = item.jsonfield1;
                    itemdb.CodiceProdotto = item.CodiceProdotto;
                    itemdb.id_prodotto = item.id_prodotto;
                    itemdb.Campo2 = item.Campo2;

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

            List<SQLiteParameter> parColl = new List<SQLiteParameter>();

            SQLiteParameter p1 = new SQLiteParameter("@dataoggi", dbDataAccess.CorrectDatenow(System.DateTime.Now));
            //p1.OleDbType = OleDbType.Date;
            parColl.Add(p1);
            //SQLiteParameter p2 = new SQLiteParameter("@SessionId", item.SessionId);
            //parColl.Add(p2);
            //SQLiteParameter p3 = new SQLiteParameter("@IpClient", item.IpClient);
            //parColl.Add(p3);

            string query = "DELETE FROM TBL_CARRELLO ";
            string where = "";
            if (string.IsNullOrWhiteSpace(where))
                query += " WHERE Data is not null and (JulianDay(Data) + Validita) - JulianDay(@dataoggi) <=0 and CodiceOrdine='' ";
            //JulianDay(@dataoggi) - (JulianDay(Data) + Validita)



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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@SessionId", item.SessionId);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@Prezzo", item.Prezzo);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@Data", dbDataAccess.CorrectDatenow(System.DateTime.Now));
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@Iva", item.Iva);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Numero", item.Numero);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@IpClient", item.IpClient);
            parColl.Add(p7);
            SQLiteParameter p8 = new SQLiteParameter("@Validita", item.Validita);
            parColl.Add(p8);
            SQLiteParameter p9 = new SQLiteParameter("@CodiceOrdine", item.CodiceOrdine);
            parColl.Add(p9);
            SQLiteParameter p10 = new SQLiteParameter("@Campo1", item.Campo1);
            parColl.Add(p10);
            SQLiteParameter p11 = new SQLiteParameter("@Campo2", item.Campo2);
            parColl.Add(p11);
            SQLiteParameter pidprod = new SQLiteParameter("@id_prodotto", item.id_prodotto);
            parColl.Add(pidprod);
            SQLiteParameter pidcliente = new SQLiteParameter("@idcliente", item.ID_cliente);// 
            parColl.Add(pidcliente);
            SQLiteParameter pnazione = new SQLiteParameter("@Codicenazione", item.Codicenazione);// 
            parColl.Add(pnazione);
            SQLiteParameter pprovincia = new SQLiteParameter("@Codiceprovincia", item.Codiceprovincia);// 
            parColl.Add(pprovincia);
            SQLiteParameter pcodicesconto = new SQLiteParameter("@Codicesconto", item.Codicesconto);// 
            parColl.Add(pcodicesconto);

            SQLiteParameter pdatastart = new SQLiteParameter("@Datastart", item.Datastart);// 
            parColl.Add(pdatastart);
            SQLiteParameter pdataend = new SQLiteParameter("@Dataend", item.Dataend);// 
            parColl.Add(pdataend);

            SQLiteParameter pjsonfield1 = new SQLiteParameter("@jsonfield1", item.jsonfield1);// 
            parColl.Add(pjsonfield1);

            string query = "INSERT INTO TBL_CARRELLO (SessionId,Prezzo,Data,Iva,Numero,CodiceProdotto,IpClient,Validita,CodiceOrdine,Campo1,Campo2,id_prodotto,ID_cliente,Codicenazione,Codiceprovincia,Codicesconto,Datastart,Dataend,jsonfield1) VALUES (@SessionId,@Prezzo,@Data,@Iva,@Numero,@CodiceProdotto,@IpClient,@Validita,@CodiceOrdine,@Campo1,@Campo2,@id_prodotto,@idcliente,@Codicenazione,@Codiceprovincia,@Codicesconto,@Datastart,@Dataend,@jsonfield1)";
            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
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
                SQLiteParameter p2 = new SQLiteParameter("@Data", dbDataAccess.CorrectDatenow(System.DateTime.Now));
                parColl.Add(p2);
                SQLiteParameter p9 = new SQLiteParameter("@Numero", item.Numero);
                parColl.Add(p9);
                SQLiteParameter p3 = new SQLiteParameter("@CodiceOrdine", item.CodiceOrdine);//OleDbType.VarChar
                parColl.Add(p3);


                SQLiteParameter p4 = new SQLiteParameter("@Campo1", item.Campo1);//lo uso per l'email del cliente che fà l'ordine
                parColl.Add(p4);

                SQLiteParameter p5 = new SQLiteParameter("@Campo2", item.Campo2);//lo uso per il json delle caratteristiche
                parColl.Add(p5);

                SQLiteParameter p6 = new SQLiteParameter("@Campo3", item.Campo3);// 
                parColl.Add(p6);

                SQLiteParameter pidcliente = new SQLiteParameter("@idcliente", item.ID_cliente);// 
                parColl.Add(pidcliente);
                SQLiteParameter pnazione = new SQLiteParameter("@Codicenazione", item.Codicenazione);// 
                parColl.Add(pnazione);
                SQLiteParameter pprovincia = new SQLiteParameter("@Codiceprovincia", item.Codiceprovincia);// 
                parColl.Add(pprovincia);
                SQLiteParameter pcodicesconto = new SQLiteParameter("@Codicesconto", item.Codicesconto);// 
                parColl.Add(pcodicesconto);


                SQLiteParameter pdatastart = new SQLiteParameter("@Datastart", item.Datastart);// 
                parColl.Add(pdatastart);
                SQLiteParameter pdataend = new SQLiteParameter("@Dataend", item.Dataend);// 
                parColl.Add(pdataend);
                SQLiteParameter pprezzo = new SQLiteParameter("@Prezzo", item.Prezzo);// 
                parColl.Add(pprezzo);

                SQLiteParameter pjsonfield1 = new SQLiteParameter("@jsonfield1", item.jsonfield1);// 
                parColl.Add(pjsonfield1);

                SQLiteParameter p1 = new SQLiteParameter("@ID", item.ID);//OleDbType.VarChar
                parColl.Add(p1);


                string query = "UPDATE [TBL_CARRELLO] SET [Data]=@Data,Numero=@Numero,CodiceOrdine=@CodiceOrdine,Campo1=@Campo1,Campo2=@Campo2,Campo3=@Campo3,ID_cliente=@idcliente,Codicenazione=@Codicenazione,Codiceprovincia=@Codiceprovincia ,Codicesconto=@Codicesconto,Datastart=@Datastart,Dataend=@Dataend,Prezzo=@Prezzo,jsonfield1=@jsonfield1 WHERE ([ID]=@ID)";
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
        /// Cancellare un elemento dal carrello passandogli l'ID e il codice caratteristiche combinate
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void DeleteCarrelloPerIDCodCarr(string connessione, long ID, string CodCar)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (ID == 0) return;
            if (string.IsNullOrEmpty(CodCar)) CodCar = "";

            SQLiteParameter p1 = new SQLiteParameter("@ID", ID);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@Campo2", CodCar);
            parColl.Add(p2);

            string query = "delete FROM TBL_CARRELLO WHERE ([ID]=@ID) and ([Campo2]=@Campo2)";
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
        /// Cancellare un elemento dal carrello passandogli l'ID
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void DeleteCarrelloPerID(string connessione, long ID)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (ID == 0) return;

            SQLiteParameter p1 = new SQLiteParameter("@ID", ID);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "DELETE FROM TBL_CARRELLO WHERE ([ID]=@ID)";
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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (listaid == null || listaid == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@codicenazione", codicenazione);//OleDbType.VarChar
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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (listaid == null || listaid == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@Sessionid", sessionid);//OleDbType.VarChar
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
        public TotaliCarrelloCollection CaricaListaOrdini(string connection, List<SQLiteParameter> parColl, string maxrecord = "", bool caricacarrelloitems = false, long page = 1, long pagesize = 0)
        {
            TotaliCarrelloCollection list = new TotaliCarrelloCollection();

            if (connection == null || connection == "") return list;
            //   if (Codiceordine == null || string.IsNullOrWhiteSpace(Codiceordine)) return null;
            TotaliCarrello item = null;
            try
            {

                string query = "";
                string queryfilter = "";
                List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();

                query = "SELECT * FROM TBL_CARRELLO_ORDINI  ";


                //if (caricacarrelloitems)
                //    query += " left join TBL_CARRELLO B on Codiceordine=B.Codiceordine ";

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id_cliente"; }))
                {
                    SQLiteParameter pidcliente = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id_cliente"; });
                    _parUsed.Add(pidcliente);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Id_cliente = @Id_cliente ";
                    else
                        queryfilter += " AND Id_cliente = @Id_cliente  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id_commerciale"; }))
                {
                    SQLiteParameter pidcomm = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id_commerciale"; });
                    _parUsed.Add(pidcomm);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Id_commerciale = @Id_commerciale ";
                    else
                        queryfilter += " AND Id_commerciale = @Id_commerciale  ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Mailcliente"; }))
                {
                    SQLiteParameter pmailcliente = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Mailcliente"; });
                    _parUsed.Add(pmailcliente);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Mailcliente like @Mailcliente ";
                    else
                        queryfilter += " AND Mailcliente like @Mailcliente  ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Codiceordine"; }))
                {
                    SQLiteParameter pCodiceordine = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Codiceordine"; });
                    _parUsed.Add(pCodiceordine);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Codiceordine like @Codiceordine ";
                    else
                        queryfilter += " AND Codiceordine like @Codiceordine  ";
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@DataMin"; }))
                {
                    SQLiteParameter pDataMin = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@DataMin"; });
                    _parUsed.Add(pDataMin);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE DataOrdine >= @DataMin ";
                    else
                        queryfilter += " AND DataOrdine >= @DataMin ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@DataMax"; }))
                {
                    SQLiteParameter pDataMax = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@DataMax"; });
                    _parUsed.Add(pDataMax);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE DataOrdine <= @DataMax ";
                    else
                        queryfilter += " AND DataOrdine <= @DataMax ";
                }

                //SQL
                query += queryfilter;

                query += " order by Dataordine desc, Id desc ";

               
                if (!string.IsNullOrEmpty(maxrecord))
                    query += " limit " + maxrecord;
                else
                {
                    if (pagesize != 0)
                    {
                        query += " limit " + (page - 1) * pagesize + "," + pagesize;
                    }
                }
                /*CALCOLO IL NUMERO DI RIGHE FILTRATE TOTALI*/
                long totalrecords = dbDataAccess.ExecuteScalar<long>("SELECT count(*) FROM  TBL_CARRELLO_ORDINI  " + queryfilter, _parUsed, connection);
                list.Totrecs = totalrecords;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new TotaliCarrello();
                        item.Id = reader.GetInt64(reader.GetOrdinal("Id"));

                        if (!reader["Indirizzofatturazione"].Equals(DBNull.Value))
                            item.Indirizzofatturazione = reader.GetString(reader.GetOrdinal("Indirizzofatturazione"));

                        if (!reader["Indirizzospedizione"].Equals(DBNull.Value))
                            item.Indirizzospedizione = reader.GetString(reader.GetOrdinal("Indirizzospedizione"));

                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.Id_cliente = reader.GetInt64(reader.GetOrdinal("Id_cliente"));

                        if (!reader["Id_commerciale"].Equals(DBNull.Value))
                            item.Id_commerciale = reader.GetInt64(reader.GetOrdinal("Id_commerciale"));
                        if (!reader["Codicesconto"].Equals(DBNull.Value))
                            item.Codicesconto = reader.GetString(reader.GetOrdinal("Codicesconto"));


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

                        if (!reader["Percacconto"].Equals(DBNull.Value))
                            item.Percacconto = reader.GetDouble(reader.GetOrdinal("Percacconto"));

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
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                string query = "";
                query = "SELECT * FROM TBL_CARRELLO_ORDINI where CodiceOrdine = @Codiceordine ";

                SQLiteParameter p1 = new SQLiteParameter("@Codiceordine", Codiceordine);//OleDbType.VarChar
                parColl.Add(p1);
                query += " order by id desc ";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new TotaliCarrello();
                        item.Id = reader.GetInt64(reader.GetOrdinal("Id"));

                        if (!reader["Indirizzofatturazione"].Equals(DBNull.Value))
                            item.Indirizzofatturazione = reader.GetString(reader.GetOrdinal("Indirizzofatturazione"));

                        if (!reader["Indirizzospedizione"].Equals(DBNull.Value))
                            item.Indirizzospedizione = reader.GetString(reader.GetOrdinal("Indirizzospedizione"));

                        if (!reader["Id_cliente"].Equals(DBNull.Value))
                            item.Id_cliente = reader.GetInt64(reader.GetOrdinal("Id_cliente"));


                        if (!reader["Id_commerciale"].Equals(DBNull.Value))
                            item.Id_commerciale = reader.GetInt64(reader.GetOrdinal("Id_commerciale"));
                        if (!reader["Codicesconto"].Equals(DBNull.Value))
                            item.Codicesconto = reader.GetString(reader.GetOrdinal("Codicesconto"));


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

                        if (!reader["Percacconto"].Equals(DBNull.Value))
                            item.Percacconto = reader.GetDouble(reader.GetOrdinal("Percacconto"));

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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@Indirizzofatturazione", item.Indirizzofatturazione);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@Indirizzospedizione", item.Indirizzospedizione);
            parColl.Add(p2);

            SQLiteParameter p3;
            if (item.Dataordine != null)
                p3 = new SQLiteParameter("@Dataordine", dbDataAccess.CorrectDatenow(item.Dataordine.Value));
            else
                p3 = new SQLiteParameter("@Dataordine", System.DBNull.Value);
            parColl.Add(p3);

            SQLiteParameter pidcliente = new SQLiteParameter("@Id_cliente", item.Id_cliente);
            parColl.Add(pidcliente);
            SQLiteParameter p4 = new SQLiteParameter("@Mailcliente", item.Mailcliente);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Modalitapagamento", item.Modalitapagamento);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@Note", item.Note);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@Urlpagamento", item.Urlpagamento);
            parColl.Add(p7);
            SQLiteParameter p8 = new SQLiteParameter("@CodiceOrdine", item.CodiceOrdine);
            parColl.Add(p8);
            SQLiteParameter p9 = new SQLiteParameter("@Denominazionecliente", item.Denominazionecliente);
            parColl.Add(p9);
            SQLiteParameter ppagato = new SQLiteParameter("@Pagato", item.Pagato);
            parColl.Add(ppagato);
            SQLiteParameter ptotaleordine = new SQLiteParameter("@TotaleOrdine", item.TotaleOrdine);// 
            parColl.Add(ptotaleordine);
            SQLiteParameter ptotsconto = new SQLiteParameter("@TotaleSconto", item.TotaleSconto);// 
            parColl.Add(ptotsconto);
            SQLiteParameter pptotspedizione = new SQLiteParameter("@TotaleSpedizione", item.TotaleSpedizione);// 
            parColl.Add(pptotspedizione);


            SQLiteParameter pptotsmaltimento = new SQLiteParameter("@TotaleSmaltimento", item.TotaleSmaltimento);// 
            parColl.Add(pptotsmaltimento);
            SQLiteParameter ppsupplementospedizione = new SQLiteParameter("@Supplementospedizione", item.Supplementospedizione);// 
            parColl.Add(ppsupplementospedizione);


            SQLiteParameter pidcommerciale = new SQLiteParameter("@Id_commerciale", item.Id_commerciale);
            parColl.Add(pidcommerciale);
            SQLiteParameter pcodicesconto = new SQLiteParameter("@Codicesconto", item.Codicesconto);
            parColl.Add(pcodicesconto);

            SQLiteParameter pacconto = new SQLiteParameter("@Percacconto", item.Percacconto);
            parColl.Add(pacconto);


            string query = "INSERT INTO TBL_CARRELLO_ORDINI([Indirizzofatturazione],[Indirizzospedizione],[Dataordine],[Id_cliente],[Mailcliente],[Modalitapagamento],[Note],[Urlpagamento],[CodiceOrdine],[Denominazionecliente],[Pagato],[TotaleOrdine],[TotaleSconto],[TotaleSpedizione],TotaleSmaltimento,Supplementospedizione,Id_commerciale,Codicesconto,Percacconto) VALUES (@Indirizzofatturazione,@Indirizzospedizione,@Dataordine,@Id_cliente,@Mailcliente,@Modalitapagamento,@Note,@Urlpagamento,@CodiceOrdine,@Denominazionecliente,@Pagato,@TotaleOrdine,@TotaleSconto,@TotaleSpedizione,@TotaleSmaltimento,@Supplementospedizione,@Id_commerciale,@Codicesconto,@Percacconto)";
            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (item == null || item.CodiceOrdine == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@Indirizzofatturazione", item.Indirizzofatturazione);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@Indirizzospedizione", item.Indirizzospedizione);
            parColl.Add(p2);
            SQLiteParameter p3;
            if (item.Dataordine != null)
                p3 = new SQLiteParameter("@Dataordine", dbDataAccess.CorrectDatenow(item.Dataordine.Value));
            else
                p3 = new SQLiteParameter("@Dataordine", System.DBNull.Value);
            parColl.Add(p3);
            SQLiteParameter pidcliente = new SQLiteParameter("@Id_cliente", item.Id_cliente);
            parColl.Add(pidcliente);
            SQLiteParameter p4 = new SQLiteParameter("@Mailcliente", item.Mailcliente);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Modalitapagamento", item.Modalitapagamento);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@Note", item.Note);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@Urlpagamento", item.Urlpagamento);
            parColl.Add(p7);
            SQLiteParameter p8 = new SQLiteParameter("@CodiceOrdine", item.CodiceOrdine);
            parColl.Add(p8);
            SQLiteParameter p9 = new SQLiteParameter("@Denominazionecliente", item.Denominazionecliente);
            parColl.Add(p9);
            SQLiteParameter ppagato = new SQLiteParameter("@Pagato", item.Pagato);
            parColl.Add(ppagato);
            SQLiteParameter ptotaleordine = new SQLiteParameter("@TotaleOrdine", item.TotaleOrdine);// 
            parColl.Add(ptotaleordine);
            SQLiteParameter ptotsconto = new SQLiteParameter("@TotaleSconto", item.TotaleSconto);// 
            parColl.Add(ptotsconto);
            SQLiteParameter pptotspedizione = new SQLiteParameter("@TotaleSpedizione", item.TotaleSpedizione);// 
            parColl.Add(pptotspedizione);

            SQLiteParameter pptotsmaltimento = new SQLiteParameter("@TotaleSmaltimento", item.TotaleSmaltimento);// 
            parColl.Add(pptotsmaltimento);
            SQLiteParameter ppsupplementospedizione = new SQLiteParameter("@Supplementospedizione", item.Supplementospedizione);// 
            parColl.Add(ppsupplementospedizione);

            SQLiteParameter pidcommerciale = new SQLiteParameter("@Id_commerciale", item.Id_commerciale);
            parColl.Add(pidcommerciale);
            SQLiteParameter pcodicesconto = new SQLiteParameter("@Codicesconto", item.Codicesconto);
            parColl.Add(pcodicesconto);

            SQLiteParameter Percacconto = new SQLiteParameter("@Percacconto", item.Percacconto);
            parColl.Add(Percacconto);

            SQLiteParameter pid = new SQLiteParameter("@Id", item.Id);//OleDbType.VarChar
            parColl.Add(pid);

            string query = "UPDATE [TBL_CARRELLO_ORDINI] SET [Indirizzofatturazione]=@Indirizzofatturazione,[Indirizzospedizione]=@Indirizzospedizione,[Dataordine]=@Dataordine,[Id_cliente]=@Id_cliente,[Mailcliente]=@Mailcliente,[Modalitapagamento]=@Modalitapagamento,[Note]=@Note,[Urlpagamento]=@Urlpagamento,[CodiceOrdine]=@CodiceOrdine,[Denominazionecliente]=@Denominazionecliente,[Pagato]=@Pagato,[TotaleOrdine]=@TotaleOrdine,[TotaleSconto]=@TotaleSconto,[TotaleSpedizione]=@TotaleSpedizione,TotaleSmaltimento=@TotaleSmaltimento,Supplementospedizione=@Supplementospedizione,Id_commerciale=@Id_commerciale,Codicesconto=@Codicesconto,Percacconto=@Percacconto WHERE ([ID]=@Id)";
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
        public void DeleteOrdinePerID(string connessione, long ID)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (ID == 0) return;

            SQLiteParameter p1 = new SQLiteParameter("@ID", ID);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "delete FROM TBL_CARRELLO_ORDINI WHERE ([ID]=@ID)";
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


        public string ExportOrdersToCsv(string DestinationPath, string CsvFilename, TotaliCarrelloCollection list)
        {
            string retString = "";
            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("it-IT");
                WelcomeLibrary.UF.SharedStatic.WriteToFile(CsvFilename, DestinationPath, "", true);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (list != null)
                {
                    sb = new StringBuilder();
                    ///TRACCIATO USATO ---------------------------------------------
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Data"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Id Ordine"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Id cliente"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Mail"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Nome"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Totale"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Pagamento"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Commerciale"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Pagato"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Dettagli"));
                    sb.Append(";");
                    WelcomeLibrary.UF.SharedStatic.WriteToFile(CsvFilename, DestinationPath, sb.ToString(), false);

                    foreach (TotaliCarrello t in list)
                    {

                        sb = new StringBuilder();

                        sb.Append(WelcomeLibrary.UF.Csv.Escape(string.Format("{0:dd/MM/yyyy HH:mm:ss}", t.Dataordine)));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.CodiceOrdine));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.Id_cliente.ToString()));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.Mailcliente));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.Denominazionecliente.Replace("<br/>", "\r\n")));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                      new object[] { t.TotaleSmaltimento + t.TotaleOrdine + t.TotaleSpedizione - t.TotaleSconto }) + " €"));
                        sb.Append(";");
                        sb.Append(t.Modalitapagamento);
                        sb.Append(";");
                        sb.Append(t.Id_commerciale);
                        sb.Append(";");
                        sb.Append((t == null) ? false : t.Pagato);
                        sb.Append(";");
                        string dettaglioordine = CreaDettaglioCarrello(t.CodiceOrdine);
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(dettaglioordine));
                        sb.Append(";");

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

        private string CreaDettaglioCarrello(string codiceordine)
        {
            StringBuilder sb = new StringBuilder();
            eCommerceDM ecmDM = new eCommerceDM();
            CarrelloCollection carrellolist = ecmDM.CaricaCarrelloPerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);
            foreach (Carrello c in carrellolist)
            {
                sb.Append(" Nome : ");
                sb.Append(c.Offerta.DenominazioneI);
                #region MODIFIED CARATTERISTICHE CARRELLO
                if (!string.IsNullOrEmpty(c.Offerta.Xmlvalue))
                {
                    sb.Append("\r\n");
                    //recupero le caratteristiche del prodotto
                    List<ModelCarCombinate> listCar = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(c.Offerta.Xmlvalue);
                    ModelCarCombinate item = listCar.Find(e => e.id == c.Campo2);
                    if (item != null)
                        sb.Append(item.caratteristica1.value + "  -  " + item.caratteristica2.value);
                    sb.Append("\r\n");
                }
                #endregion

                //sb.Append(" <div class=\"product-categories muted\">");
                //sb.Append(CommonPage.TestoCategoria(c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, Lingua));
                //sb.Append(" </div>");
                //sb.Append(" <div class=\"product-categories muted\">");
                //sb.Append(CommonPage.TestoCaratteristica(0, c.Offerta.Caratteristica1.ToString(), Lingua));
                //sb.Append(" </div>");
                //sb.Append(" <div class=\"product-categories muted\">");
                //sb.Append(CommonPage.TestoCaratteristica(1, c.Offerta.Caratteristica2.ToString(), Lingua));
                //sb.Append(" </div>");
                //sb.Append(" <div class=\"product-categories muted\">");
                //sb.Append(TestoSezione(c.Offerta.CodiceTipologia));
                //sb.Append(" </div>");

                sb.Append("\r\n");
                if (c.Datastart != null && c.Dataend != null)
                {
                    sb.Append("Periodo dal " + string.Format("{0:dd/MM/yyyy}", c.Datastart) + "\r\n");
                    sb.Append(" al " + string.Format("{0:dd/MM/yyyy}", c.Dataend) + "\r\n");
                }
                string ret = "";
                Dictionary<string, string> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(c.jsonfield1.ToString());
                if (dic != null && dic.ContainsKey("adulti"))
                    ret += dic["adulti"];
                else
                    ret = "";
                sb.Append("\r\n adulti:" + ret);
                ret = "";
                if (dic != null && dic.ContainsKey("bambini"))
                    ret += dic["bambini"];
                else
                    ret = "";
                sb.Append("\r\nbambini:" + ret);
                sb.Append("\r\n");

                sb.Append(c.Numero + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { c.Prezzo }) + " €");
                sb.Append("\r\n");

                sb.Append("\r\n");
            }
            return sb.ToString();
        }
    }
}
