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
using System.IO;
using ClosedXML.Excel;

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

            Carrello item;
            try
            {
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                string query = "";
                query = "SELECT A.ID,A.SessionId,A.Data,A.Prezzo,A.Iva,A.Numero,A.CodiceProdotto,A.jsonfield1,A.IpClient,A.Validita,A.Campo1,A.Campo2,A.Campo3,A.Campo4,A.Campo5,A.CodiceOrdine,A.ID_cliente,A.ID_prodotto,A.Codicenazione,A.Codiceprovincia,A.Codicesconto,A.Datastart,A.Dataend,B.ID as B_ID,B.CodiceTIPOLOGIA,B.DataInserimento,B.DescrizioneGB,B.DescrizioneRU,B.DescrizioneFR,B.DescrizioneI,B.DENOMINAZIONEGB,B.DENOMINAZIONERU,B.DENOMINAZIONEFR,B.DENOMINAZIONEI,B.linkVideo,B.CodiceCOMUNE,B.CodicePROVINCIA as B_CodicePROVINCIA,B.CodiceREGIONE,B.CodiceProdotto as B_CodiceProdotto,B.CodiceCategoria,B.CodiceCategoria2Liv,B.Caratteristica1,B.Caratteristica2,B.Caratteristica3,B.Caratteristica4,B.Caratteristica5,B.Caratteristica6,B.Xmlvalue,B.DATITECNICII,B.DATITECNICIGB,B.DATITECNICIRU,B.DATITECNICIFR,B.EMAIL,B.FAX,B.INDIRIZZO,B.TELEFONO,B.WEBSITE,B.Prezzo as B_Prezzo,B.PrezzoListino,B.Vetrina,B.FotoSchema,B.FotoValori,B.urlcustomI,B.urlcustomGB,B.urlcustomRU,B.urlcustomFR,B.Peso FROM TBL_CARRELLO as A left outer join TBL_ATTIVITA as B on A.id_prodotto=B.Id where SessionId like @SessionId and IpClient like @ipClient and CodiceOrdine = '' ";

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
                            offerta.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                            if (!(reader["DescrizioneFR"]).Equals(DBNull.Value)) offerta.DescrizioneFR = reader.GetString(reader.GetOrdinal("DescrizioneFR"));
                            if (!(reader["DenominazioneFR"]).Equals(DBNull.Value)) offerta.DenominazioneFR = reader.GetString(reader.GetOrdinal("DENOMINAZIONEFR"));
                            if (!(reader["DescrizioneRU"]).Equals(DBNull.Value)) offerta.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                            if (!(reader["DenominazioneRU"]).Equals(DBNull.Value)) offerta.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                            offerta.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                            offerta.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                            if (!reader["linkVideo"].Equals(DBNull.Value))
                                offerta.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));


                            if (!(reader["urlcustomGB"]).Equals(DBNull.Value))
                                offerta.UrlcustomGB = reader.GetString(reader.GetOrdinal("urlcustomGB"));
                            if (!(reader["urlcustomI"]).Equals(DBNull.Value))
                                offerta.UrlcustomI = reader.GetString(reader.GetOrdinal("urlcustomI"));
                            if (!(reader["urlcustomRU"]).Equals(DBNull.Value))
                                offerta.UrlcustomRU = reader.GetString(reader.GetOrdinal("urlcustomRU"));
                            if (!(reader["urlcustomFR"]).Equals(DBNull.Value))
                                offerta.UrlcustomFR = reader.GetString(reader.GetOrdinal("urlcustomFR"));


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
                            if (!reader["DATITECNICIFR"].Equals(DBNull.Value))
                                offerta.DatitecniciFR = reader.GetString(reader.GetOrdinal("DATITECNICIFR"));
                            if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                                offerta.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
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
                            if (!reader["Peso"].Equals(DBNull.Value))
                                offerta.Peso = reader.GetDouble(reader.GetOrdinal("Peso"));

                            if (!reader["Peso"].Equals(DBNull.Value))
                                offerta.Peso = reader.GetDouble(reader.GetOrdinal("Peso"));
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
                query = "SELECT A.ID ,A.SessionId,A.Data,A.Prezzo,A.Iva,A.Numero,A.CodiceProdotto,A.jsonfield1,A.IpClient,A.Validita,A.Campo1,A.Campo2,A.Campo3,A.Campo4,A.Campo5,A.CodiceOrdine,A.ID_cliente,A.ID_prodotto,A.Codicenazione,A.Codiceprovincia,A.Codicesconto,A.Datastart,A.Dataend, B.ID as B_ID,B.CodiceTIPOLOGIA,B.DataInserimento,B.DescrizioneGB,B.DescrizioneRU,B.DescrizioneFR,B.DescrizioneI,B.DENOMINAZIONEGB,B.DENOMINAZIONERU,B.DENOMINAZIONEFR,B.DENOMINAZIONEI,B.linkVideo,B.CodiceCOMUNE,B.CodicePROVINCIA as B_CodicePROVINCIA,B.CodiceREGIONE,B.CodiceProdotto as B_CodiceProdotto,B.CodiceCategoria,B.CodiceCategoria2Liv,B.Caratteristica1,B.Caratteristica2,B.Caratteristica3,B.Caratteristica4,B.Caratteristica5,B.Caratteristica6,B.Xmlvalue,B.DATITECNICII,B.DATITECNICIGB,B.DATITECNICIRU,B.DATITECNICIFR,B.EMAIL,B.FAX,B.INDIRIZZO,B.TELEFONO,B.WEBSITE,B.Prezzo as B_Prezzo,B.PrezzoListino,B.Peso,B.Vetrina,B.FotoSchema,B.FotoValori,B.urlcustomI,B.urlcustomGB,B.urlcustomRU,B.urlcustomFR FROM TBL_CARRELLO A left outer join TBL_ATTIVITA B on A.id_prodotto=B.Id where CodiceOrdine = @Codiceordine ";


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
                            offerta.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                            if (!(reader["DescrizioneFR"]).Equals(DBNull.Value)) offerta.DescrizioneFR = reader.GetString(reader.GetOrdinal("DescrizioneFR"));
                            if (!(reader["DenominazioneFR"]).Equals(DBNull.Value)) offerta.DenominazioneFR = reader.GetString(reader.GetOrdinal("DENOMINAZIONEFR"));
                            if (!(reader["DescrizioneRU"]).Equals(DBNull.Value)) offerta.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                            if (!(reader["DenominazioneRU"]).Equals(DBNull.Value)) offerta.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));


                            offerta.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
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

                            if (!(reader["urlcustomGB"]).Equals(DBNull.Value))
                                offerta.UrlcustomGB = reader.GetString(reader.GetOrdinal("urlcustomGB"));
                            if (!(reader["urlcustomI"]).Equals(DBNull.Value))
                                offerta.UrlcustomI = reader.GetString(reader.GetOrdinal("urlcustomI"));
                            if (!(reader["urlcustomRU"]).Equals(DBNull.Value))
                                offerta.UrlcustomRU = reader.GetString(reader.GetOrdinal("urlcustomRU"));
                            if (!(reader["urlcustomFR"]).Equals(DBNull.Value))
                                offerta.UrlcustomFR = reader.GetString(reader.GetOrdinal("urlcustomFR"));


                            if (!reader["Xmlvalue"].Equals(DBNull.Value))
                                offerta.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));

                            if (!reader["DATITECNICII"].Equals(DBNull.Value))
                                offerta.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                            if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                                offerta.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                            if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                                offerta.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
                            if (!reader["DATITECNICIFR"].Equals(DBNull.Value))
                                offerta.DatitecniciFR = reader.GetString(reader.GetOrdinal("DATITECNICIFR"));

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
        /// Funzione che Fa inserimento e aggiornamento in tabella carrello prodotti di un elemento
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
                    itemdb.Iva = item.Iva;

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
                SQLiteParameter piva = new SQLiteParameter("@iva", item.Iva);// 
                parColl.Add(piva);

                SQLiteParameter p1 = new SQLiteParameter("@ID", item.ID);//OleDbType.VarChar
                parColl.Add(p1);


                string query = "UPDATE [TBL_CARRELLO] SET [Data]=@Data,Numero=@Numero,CodiceOrdine=@CodiceOrdine,Campo1=@Campo1,Campo2=@Campo2,Campo3=@Campo3,ID_cliente=@idcliente,Codicenazione=@Codicenazione,Codiceprovincia=@Codiceprovincia ,Codicesconto=@Codicesconto,Datastart=@Datastart,Dataend=@Dataend,Prezzo=@Prezzo,jsonfield1=@jsonfield1,iva=@iva WHERE ([ID]=@ID)";

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

        public void DeleteCarrelloPerCodiceOrdine(string connessione, string codiceordine)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (string.IsNullOrEmpty(codiceordine)) return;

            SQLiteParameter p1 = new SQLiteParameter("@CodiceOrdine", codiceordine);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "DELETE FROM TBL_CARRELLO WHERE ([CodiceOrdine]=@CodiceOrdine)";
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
        /// Esegue il check su un codiceordine se la quantita di acquisto nel carrello provoca il superamento dello scaglione associato 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codiceordine"></param>
        /// <returns></returns>
        public bool VerificaSuperamentoSuOrdine(string connection, string codiceordine)
        {
            bool ret = false;
            //TotaliCarrello c = this.CaricaOrdinePerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);
            //if (c != null)
            //{ }
            offerteDM offDM = new offerteDM();
            CarrelloCollection righeordine = this.CaricaCarrelloPerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);
            //string listcod = "";
            //righeordine.ForEach(rigo => listcod += rigo.id_prodotto + ",");
            if (righeordine != null)
                foreach (Carrello item in righeordine) //nella casistica base dovrei avere un solo rigo con lo scaglione ( non potendo avere + scaglioni per codiceordine )
                {
                    if (!string.IsNullOrEmpty(item.jsonfield1))
                    {
                        string idscaglionedajson = (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(item.jsonfield1, "idscaglione", "I");
                        long quantita = item.Numero;//quantita da aggiungere presente nell'ordine sul codice passato
                        long idscaglione = 0;
                        if (long.TryParse(idscaglionedajson, out idscaglione))
                        {
                            List<SQLiteParameter> parscaglioni = new List<SQLiteParameter>();
                            SQLiteParameter ps1 = new SQLiteParameter("@id", idscaglione);//OleDbType.VarChar
                            parscaglioni.Add(ps1);

                            ScaglioniCollection listascaglioni = offerteDM.CaricaOfferteScaglioni(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parscaglioni);
                            if (listascaglioni != null)
                            {
                                string s_niscritti = ""; long niscritti = 0;
                                Scaglioni scitem = listascaglioni.Find(s => s.id == idscaglione);
                                if (scitem != null && scitem.addedvalues.ContainsKey("niscritti"))
                                {
                                    s_niscritti = scitem.addedvalues["niscritti"];
                                    long.TryParse(s_niscritti, out niscritti);
                                    if ((quantita + niscritti) > scitem.nmax)
                                    {
                                        ret = true;
                                    }
                                }
                            }

                        }
                    }
                }
            return ret;
        }

        /// <summary>
        /// Aggiorna lo stato degli scaglione per niscritti (ordini pagati o acconto) e nattesa ( ordini non pagati ) e per gli idscaglione o idprodotto passati nei parametri,
        /// inoltre aggiorna lo stato dello Scaglione tra quelli previsti.
        /// Vengono aggionati i numeri ordini solo per gli scaglioni fino a 20 gg successivi alla partenza impostata poi si congelano.
        /// Per gli scaglioni in stato SOSPESO (7) i numeri di attesa e iscritti si aggionano ma non lo stato che rimane bloccato
        /// Tutti gli stati maggiori o uguali a 4 ( confermato ) hanno bloccata la possibilità di inserimento a carrello sia in cusotmbind che in javascript 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="parametri">Passare idprodotto o idscaglione singoli o separati da virgole</param>
        public void AggiornaStatoscaglioni(string connection, Dictionary<string, string> parametri)
        {
            offerteDM offDM = new offerteDM();
            //parametri.Add("idprodotto", idprodotto);
            //parametri.Add("idscaglione", idscaglione);
            //parametri.Add("statoacconto", statoacconto);
            //parametri.Add("statosaldo", statosaldo);

            //per un idscaglioni o per gli scaglioni di un idattivita devo verificare lo stato dei pagamenti sulla tabella del carrello
            // eCommerceDM riportare I SEGUENTI VALORI in numero di occorrenze, attenzione da conteggiare in base alla quantità a carrello per gli ordini:
            //- in attesa pagamento sono GLI ORDINI PER un pacchetto ma che ancora non hanno pagato l'acconto o il saldo
            //- Iscritti sono quelli che hanno pagato acconto o saldo

            ////////////////////////////////////////////////////////////////
            //Carichiamo tutti gli scaglioni per idprodotto o idscaglione ( che sono quelli che verranno aggiornati per le disponibilita )
            ////////////////////////////////////////////////////////////////
            string listaidprodotto = "";
            List<SQLiteParameter> parscaglioni = new List<SQLiteParameter>();
            if (parametri.ContainsKey("idprodotto") && !string.IsNullOrWhiteSpace(parametri["idprodotto"]))
            {
                listaidprodotto = parametri["idprodotto"];
                SQLiteParameter ps1 = new SQLiteParameter("@id_attivita", listaidprodotto);//OleDbType.VarChar
                parscaglioni.Add(ps1);
            }
            string listaidscaglioni = "";
            if (parametri.ContainsKey("idscaglione") && !string.IsNullOrWhiteSpace(parametri["idscaglione"]))
            {
                listaidscaglioni = parametri["idscaglione"];
                SQLiteParameter ps2 = new SQLiteParameter("@id", listaidscaglioni);//OleDbType.VarChar
                parscaglioni.Add(ps2);
            }
            // inserisco il parametro di filtro scaglioni : prendo gli scaglioni che hanno datapartenza > data attuale - 20 GIORNI
            //in modo da non aggiornare sempre tutti anche nel passato oltre quelli SCADUTI 20 GG!!!! )
#if true
            SQLiteParameter ps3 = new SQLiteParameter("@Data_inizio", System.DateTime.Now.AddDays(-20));
            parscaglioni.Add(ps3);
#endif
            ScaglioniCollection listascaglioni = offerteDM.CaricaOfferteScaglioni(connection, parscaglioni);

            //////////////////////////////////////////////////////////////////////////////////////////////
            //CARICHIAMO I DATI DI CARRELLO E DELLA TABELLA ORDINI per gli scaglioni che ci interessano
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            eCommerceDM eDM = new eCommerceDM();
            List<SQLiteParameter> parcoll = new List<SQLiteParameter>();

            if (parametri.ContainsKey("idprodotto") && !string.IsNullOrWhiteSpace(parametri["idprodotto"]))
            {
                SQLiteParameter paridprodotto = new SQLiteParameter("@idprodotto", parametri["idprodotto"]);
                parcoll.Add(paridprodotto);
            }
            if (parametri.ContainsKey("idscaglione") && !string.IsNullOrWhiteSpace(parametri["idscaglione"]))
            {
                SQLiteParameter paridscaglione = new SQLiteParameter("@idscaglione", parametri["idscaglione"]);
                parcoll.Add(paridscaglione);
            }
            //Faccio il filtro sulle righe a carrello ma questo non fornisce i dati sui pagamenti, però prende la lista dei codici ordini ed i dati relativi allo scaglione o tutti gli scaglioni per un prodotto
            CarrelloCollection righeordiniacarrello = FiltraCodiciPerOfferteScaglioni(connection, parcoll);
            if (righeordiniacarrello != null)
            {
                List<SQLiteParameter> parcoll1 = new List<SQLiteParameter>();
                //Partiamo dalla lista codiciordine filtrata che ho caricato
                SQLiteParameter parcod = new SQLiteParameter("@Codiceordine", "");
                string listcod = "";
                righeordiniacarrello.ForEach(c => listcod += c.CodiceOrdine + ",");
                parcod.Value = listcod;
                parcoll1.Add(parcod);
                // CARICHIAMO TUTTI GLI ORDINI ASSOCIATI AI CODICI ORDINE DEL CARRELLO
                // ESTRATTI in base a idattivita o idscaglione 
                //dove troviamo i dati sui pagamenti caricando gli ordini corrispondenti
                TotaliCarrelloCollection ordinicomplessivi = eDM.CaricaListaOrdini(connection, parcoll1);
                foreach (Scaglioni scaglione in listascaglioni) //Scorriamo ed aggiorniamo i dati degli scaglioni per ciascuno
                {
                    //azzeriamo il conteggio per lo scaglione attuale
                    scaglione.addedvalues.Remove("nattesa");
                    scaglione.addedvalues.Remove("niscritti");
                    scaglione.addedvalues["nattesa"] = "0";
                    scaglione.addedvalues["niscritti"] = "0";
                    long nattesa = 0; long niscritti = 0;
                    foreach (Carrello carrello in righeordiniacarrello)   //Scorriamo tutte le righe di carrello che contengono lo scaglione di interesse e sommiamo
                    {
                        //    string prezzoscaglione = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "prezzo", "I");
                        //string datapartenza = Utility.reformatdatetimestring((string)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(itemcarrello.jsonfield1, "datapartenza", "I"));
                        //string dataritorno = Utility.reformatdatetimestring((string)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(itemcarrello.jsonfield1, "dataritorno", "I"));
                        //    string idscaglione = (String)eCommerceDM.Selezionadajson(item.jsonfield1, "idscaglione", "I");
                        //    try
                        //    {
                        //        Scaglioni scaglionedacarrello = Newtonsoft.Json.JsonConvert.DeserializeObject<Scaglioni>((String)eCommerceDM.Selezionadajson(item.jsonfield1, "scaglione", "I"));
                        //    }
                        //    catch { }
                        if ((string)eCommerceDM.Selezionadajson(carrello.jsonfield1, "idscaglione", "I") == scaglione.id.ToString())
                        {
                            //Filtriamo l'ordine corrispondente al rigo attuale ( ce ne puo essere uno solo nella tablella degli ordini !!! )
                            TotaliCarrello ordinepercodice = (TotaliCarrello)ordinicomplessivi.Find(o => o.CodiceOrdine == carrello.CodiceOrdine);
                            if (ordinepercodice != null)
                            {
                                long quantita = carrello.Numero;
                                //Adesso conteggiamo i valori per iscritti e in attesa e memorizziamoli per ogni scaglione !
                                //-  nattesa  : in attesa pagamento sono quelli che hanno ordinato il pacchetto ma ancora non hanno pagato l'acconto o il saldo
                                //- niscritti : Iscritti sono quelli che hanno pagato acconto o saldo
                                if (!ordinepercodice.Pagatoacconto && !ordinepercodice.Pagato) nattesa += quantita;
                                else if (ordinepercodice.Pagatoacconto && !ordinepercodice.Pagato) { if (ordinepercodice.TotaleAcconto != 0) niscritti += quantita; else nattesa += quantita; }
                                else if (!ordinepercodice.Pagatoacconto && ordinepercodice.Pagato) { niscritti += quantita; } // se pagato un saldo anche se acconto non pagato si considerano iscritti ( caso raro di una che paga il saldo senza pagare l'acconto )
                                else if (ordinepercodice.Pagatoacconto && ordinepercodice.Pagato) niscritti += quantita;
                            }
                        }
                    }
                    scaglione.addedvalues["nattesa"] = nattesa.ToString(); ;
                    scaglione.addedvalues["niscritti"] = niscritti.ToString();
                    scaglione.jsonvalues = Newtonsoft.Json.JsonConvert.SerializeObject(scaglione.addedvalues);
                    //    //Facciamo  update dei dati per lo scaglione nel db scaglioni ( da decidere se farlo a fine ciclo completo su tutti ... )
                    //Settiamo lo stato degli scaglioni in base ai numeri di iscritti
                    SetScaglioneStatus(scaglione);
                    offerteDM.InserisciAggiornaScaglioni(connection, scaglione); // aggiornamento per singolo valore nel ciclo ... sarebbe meglio farlo a fine ciclo come update complessivo di  listascaglioni
                }

            }
        }

        /// <summary>
        /// STATO CONFERMATO: iscritti maggiore o uguale a NCONFERMA dello scaglione
        /// STATO QUASI CONFERMATO: iscritti maggiore o uguale a 70 % del valore NCONFERMA dello scaglione
        /// STATO QUASI COMPLETO: iscritti maggiore o uguale a 70 % del valore NMAX dello scaglione
        /// STATO COMPLETO : iscritti uguale a valore NMAX dello scaglione
        /// {"0":"iscrizioni aperte","1":"quasi confermato","2":"confermato","3":"quasi completo","4":"completo","5":"in partenza","6":"scaduto","7":"sospeso"}
        /// </summary>
        /// <param name="scaglione"></param>
        private void SetScaglioneStatus(Scaglioni scaglione, double percscarto = 0.7)
        {
            if (scaglione.stato != 7) // se SOSPESO 7 lo scaglione non ricalcola variazioni di stato in base agli iscritti MA AGGIORNA SOLO I NUMERI ISCRITTI
            {
                double niscritti = 0;
                //   scaglione.addedvalues["nattesa"]
                if (double.TryParse(scaglione.addedvalues["niscritti"], out niscritti))
                {
                    if (niscritti >= 0 && niscritti < (scaglione.nconferma * percscarto)) scaglione.stato = 0;
                    if (niscritti >= (scaglione.nconferma * percscarto) && niscritti < (scaglione.nconferma)) scaglione.stato = 1;
                    if (niscritti >= (scaglione.nconferma) && niscritti < (scaglione.nmax * percscarto)) scaglione.stato = 2;
                    if (niscritti >= (scaglione.nmax * percscarto) && niscritti < (scaglione.nmax)) scaglione.stato = 3;
                    if (niscritti >= (scaglione.nmax)) scaglione.stato = 4;
                }

                //aggiornamenti stato post partenza, ATTENZIONE funziona solo se la chiamata di aggiornamento dati degli istritti scaglioni gira entro 20 gg dalla data partenza
                //cioe se viene chiamato l'evento di aggiornamento dei dati scaglioni entro quel termine
                if (System.DateTime.Now.AddDays(0) >= scaglione.datapartenza && scaglione.stato >= 2 && scaglione.stato <= 4) scaglione.stato = 5; //confermato ed in partenza
                if (System.DateTime.Now.AddDays(0) >= scaglione.datapartenza && scaglione.stato < 2) scaglione.stato = 6; //scaduto il termine di iscrizione
            }

        }


        /// <summary>
        /// CArica la lista degli ordini per data discendente in base ai parametri di filtro passati
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

                //if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Codiceordine"; }))
                //{
                //    SQLiteParameter pCodiceordine = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Codiceordine"; });
                //    _parUsed.Add(pCodiceordine);
                //    if (!queryfilter.ToLower().Contains("where"))
                //        queryfilter += " WHERE Codiceordine like @Codiceordine ";
                //    else
                //        queryfilter += " AND Codiceordine like @Codiceordine  ";


                ////////////////////////////////////////////////////////////////////////////////////////////////
                //FIltro la lista codiciordine presenti nella tbl_carrello in base a attivita e scaglione passati
                /////////////////////////////////////////////////////////////////////////////////////////// 
                CarrelloCollection righeordiniacarrello = FiltraCodiciPerOfferteScaglioni(connection, parColl);
                if (righeordiniacarrello != null) //FILTRO ORDINI ATTIVATO SUI PARAMETRI DEL CARRELLO
                    if (righeordiniacarrello.Count > 0)
                    {
                        string listcod = "";
                        righeordiniacarrello.ForEach(c => listcod += "," + c.CodiceOrdine);
                        if (!parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Codiceordine"; }))
                        {
                            SQLiteParameter parcod = new SQLiteParameter("@Codiceordine", listcod);
                            parColl.Add(parcod);
                        }
                        else // facciamo un operatore and delle due liste passata e filtrata dal carrello
                        {
                            SQLiteParameter pcodlist = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Codiceordine"; });
                            string parameterscodici = pcodlist.Value.ToString();
                            parameterscodici = parameterscodici.Trim().ToString().Replace("|", ",");
                            //FACCIAMO UN'OPERAZIONE AND DELLE DUE LISTE CODICI
                            List<string> passedlist = parameterscodici.Trim().Split(',').ToList();
                            List<string> filteredlist = listcod.Trim().Split(',').ToList();
                            string joinedandlist = string.Empty;
                            foreach (string el in filteredlist)
                            {
                                if (!string.IsNullOrEmpty(el.Trim()))
                                    if (passedlist.Exists(el1 => el1 == el))
                                        joinedandlist += "," + el.Trim();
                            }
                            if (!string.IsNullOrEmpty(joinedandlist))
                                pcodlist.Value = joinedandlist; //sovrascrivo lo lista codiciordini passata con quella filtrata dando la priprita a questa
                            else return list; //parColl.Remove(pcodlist); NON CI SONO CODICI COMUNI NELLE LISTE.. DEVO TORNARE LISTA VUOTA
                        }
                    }
                    else return list; //se il filtro ordini si è attivato per presenza dei parametri dedicati e non ci sono risultati devo tornare lista vuota
                ////////////////////////////////////////////////////////////////
                ///

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Codiceordine"; }))
                {
                    SQLiteParameter pcodlist = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Codiceordine"; });
                    string listcod = pcodlist.Value.ToString();
                    listcod = listcod.Trim().ToString().Replace("|", ",");
                    if (!listcod.Contains(","))
                    {
                        _parUsed.Add(pcodlist);
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE Codiceordine like @Codiceordine ";
                        else
                            queryfilter += " AND Codiceordine like @Codiceordine  ";
                    }
                    else
                    {
                        string[] listaarray = listcod.Trim().Split(',');
                        if (listaarray != null && listaarray.Length > 0)
                        {
                            if (!queryfilter.ToLower().Contains("where"))
                                queryfilter += " WHERE Codiceordine in (    ";
                            else
                                queryfilter += " AND  Codiceordine in (      ";
                            foreach (string codice in listaarray)
                            {
                                if (!string.IsNullOrEmpty(codice.Trim()))
                                    queryfilter += " '" + codice + "' ,";
                            }
                            queryfilter = queryfilter.TrimEnd(',') + " ) ";
                        }
                    }
                }



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

                //Filtro per id ordine o lista id ordini separate da | o da ,
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@id"; }))
                {
                    SQLiteParameter pidlist = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@id"; });
                    string listaid = pidlist.Value.ToString();
                    listaid = listaid.Trim().ToString().Replace("|", ",");

                    if (!listaid.Contains(","))
                    {
                        _parUsed.Add(pidlist);
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE id like @id ";
                        else
                            queryfilter += " AND id like @id  ";
                    }
                    else
                    {
                        string[] listaarray = listaid.Trim().Split(',');
                        if (listaarray != null && listaarray.Length > 0)
                        {
                            if (!queryfilter.ToLower().Contains("where"))
                                queryfilter += " WHERE id in (    ";
                            else
                                queryfilter += " AND  id in (      ";
                            foreach (string codice in listaarray)
                            {
                                if (!string.IsNullOrEmpty(codice.Trim()))
                                    queryfilter += " " + codice + " ,";
                            }
                            queryfilter = queryfilter.TrimEnd(',') + " ) ";
                        }
                    }
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


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@statoacconto"; }))
                {
                    SQLiteParameter pstatoacconto = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@statoacconto"; });
                    _parUsed.Add(pstatoacconto);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Pagatoacconto = @statoacconto ";
                    else
                        queryfilter += " AND Pagatoacconto = @statoacconto ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@statosaldo"; }))
                {
                    SQLiteParameter pstatosaldo = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@statosaldo"; });
                    _parUsed.Add(pstatosaldo);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Pagato = @statosaldo ";
                    else
                        queryfilter += " AND Pagato = @statosaldo ";
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

                        if (!reader["Pagatoacconto"].Equals(DBNull.Value))
                            item.Pagatoacconto = reader.GetBoolean(reader.GetOrdinal("Pagatoacconto"));

                        if (!reader["TotaleOrdine"].Equals(DBNull.Value))
                            item.TotaleOrdine = reader.GetDouble(reader.GetOrdinal("TotaleOrdine"));

                        if (!reader["TotaleSconto"].Equals(DBNull.Value))
                            item.TotaleSconto = reader.GetDouble(reader.GetOrdinal("TotaleSconto"));
                        if (!reader["TotaleSpedizione"].Equals(DBNull.Value))
                            item.TotaleSpedizione = reader.GetDouble(reader.GetOrdinal("TotaleSpedizione"));

                        if (!reader["TotaleAssicurazione"].Equals(DBNull.Value))
                            item.TotaleAssicurazione = reader.GetDouble(reader.GetOrdinal("TotaleAssicurazione"));
                        if (!reader["Nassicurazioni"].Equals(DBNull.Value))
                            item.Nassicurazioni = reader.GetInt64(reader.GetOrdinal("Nassicurazioni"));

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
        /// Filtra i codiciordine a carrello in base ai parametri indicati facendo le query con JSON1 in sqllite direttamente nei campi json.
        /// Se torna null il filtro non si è attivato quindi lo devo bypassare, ma se trona una lista con count >=0 devo considerare il risultato...
        /// </summary>
        /// <param name="parColl"></param>
        /// <returns></returns>
        private CarrelloCollection FiltraCodiciPerOfferteScaglioni(string connection, List<SQLiteParameter> parColl)
        {
            CarrelloCollection dettagliordini = null;

            //List<string> listacodici = null; //risultato di default per non attivazione
            List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();
            string queryfilter = "";
            //string query = "SELECT DISTINCT CodiceOrdine FROM TBL_CARRELLO  ";
            string query = "SELECT * FROM TBL_CARRELLO  ";
            queryfilter += " WHERE CodiceOrdine != ''    ";
            bool activatefiltering = false;

            //INTRODUCIAMO IL FILTRO PRODOTTI E SCAGLIONI sugli ordini
            //if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idprodotto"; }))
            //{
            //    SQLiteParameter pidattivita = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idprodotto"; });
            //    if (pidattivita.Value != null && !string.IsNullOrEmpty(pidattivita.Value.ToString()))
            //    {
            //        _parUsed.Add(pidattivita);
            //        activatefiltering = true;
            //        if (!queryfilter.ToLower().Contains("where"))
            //            queryfilter += " WHERE ID_prodotto = @idprodotto ";
            //        else
            //            queryfilter += " AND ID_prodotto = @idprodotto ";
            //    }
            //}
            if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idprodotto"; }))
            {
                SQLiteParameter pidlist = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idprodotto"; });
                string listaid = pidlist.Value.ToString();
                listaid = listaid.Trim().ToString().Replace("|", ",");
                if (!listaid.Contains(","))
                {
                    _parUsed.Add(pidlist);
                    activatefiltering = true;
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ID_prodotto like @idprodotto ";
                    else
                        queryfilter += " AND ID_prodotto like @idprodotto  ";
                }
                else
                {
                    string[] listaarray = listaid.Trim().Split(',');
                    if (listaarray != null && listaarray.Length > 0)
                    {
                        activatefiltering = true;
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE ID_prodotto in (    ";
                        else
                            queryfilter += " AND  ID_prodotto in (      ";
                        foreach (string codice in listaarray)
                        {
                            if (!string.IsNullOrEmpty(codice.Trim()))
                                queryfilter += " " + codice + " ,";
                        }
                        queryfilter = queryfilter.TrimEnd(',') + " ) ";
                    }
                }
            }

            //if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idscaglione"; }))
            //{
            //    SQLiteParameter pidscaglione = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idscaglione"; });
            //    if (pidscaglione.Value != null && !string.IsNullOrEmpty(pidscaglione.Value.ToString()))
            //    {
            //        pidscaglione.DbType = System.Data.DbType.String;
            //        _parUsed.Add(pidscaglione);
            //        activatefiltering = true;

            //        if (!queryfilter.ToLower().Contains("where"))
            //            queryfilter += " WHERE json_extract(jsonfield1, '$.idscaglione') = @idscaglione ";
            //        else
            //            queryfilter += " AND  json_extract(jsonfield1, '$.idscaglione') = @idscaglione ";
            //    }
            //}
            if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idscaglione"; }))
            {
                SQLiteParameter pidlistscaglioni = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idscaglione"; });
                string listaid = pidlistscaglioni.Value.ToString();
                listaid = listaid.Trim().ToString().Replace("|", ",");
                if (!listaid.Contains(","))
                {
                    activatefiltering = true;
                    _parUsed.Add(pidlistscaglioni);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.idscaglione') = @idscaglione ";
                    else
                        queryfilter += " AND JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.idscaglione') = @idscaglione ";
                }
                else
                {
                    activatefiltering = true;
                    string[] listaarray = listaid.Trim().Split(',');
                    if (listaarray != null && listaarray.Length > 0)
                    {
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.idscaglione')  in (    ";
                        else
                            queryfilter += " WHERE JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.idscaglione')  in (      ";
                        foreach (string codice in listaarray)
                        {
                            if (!string.IsNullOrEmpty(codice.Trim()))
                                queryfilter += " " + codice + " ,";
                        }
                        queryfilter = queryfilter.TrimEnd(',') + " ) ";
                    }
                }
            }
            if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine"; }))
            {
                activatefiltering = true;
                SQLiteParameter pdatapartenza = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine"; });
                //.Value.ToString("yyyy-MM-dd 00:00:00")  //formato corretto per filtro data
                pdatapartenza.DbType = System.Data.DbType.DateTime;
                _parUsed.Add(pdatapartenza);

                if (!queryfilter.ToLower().Contains("where"))
                    queryfilter += " WHERE JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.datapartenza') < @Data_fine ";
                else
                    queryfilter += " AND  JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.datapartenza') < @Data_fine ";

                //  where((json_extract(jsonfield1, '$.datapartenza')) > '2020-12-17')
            }
            if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio"; }))
            {
                activatefiltering = true;
                SQLiteParameter pdatapartenza = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio"; });
                //.Value.ToString("yyyy-MM-dd 00:00:00")  //formato corretto per filtro data
                pdatapartenza.DbType = System.Data.DbType.DateTime;
                _parUsed.Add(pdatapartenza);

                if (!queryfilter.ToLower().Contains("where"))
                    queryfilter += " WHERE JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.datapartenza') > @Data_inizio ";
                else
                    queryfilter += " AND JSON_VALID(jsonfield1) and  json_extract(jsonfield1, '$.datapartenza') > @Data_inizio ";

                //  where((json_extract(jsonfield1, '$.datapartenza')) > '2020-12-17')
            }

            if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@PrezzoMin"; }))
            {
                activatefiltering = true;
                SQLiteParameter pdatapartenza = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@PrezzoMin"; });
                //.Value.ToString("yyyy-MM-dd 00:00:00")  //formato corretto per filtro data
                pdatapartenza.DbType = System.Data.DbType.DateTime;
                _parUsed.Add(pdatapartenza);

                if (!queryfilter.ToLower().Contains("where"))
                    queryfilter += " WHERE JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.prezzo') >= @PrezzoMin ";
                else
                    queryfilter += " AND JSON_VALID(jsonfield1) and  json_extract(jsonfield1, '$.prezzo') >= @PrezzoMin ";
            }

            if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@PrezzoMax"; }))
            {
                activatefiltering = true;
                SQLiteParameter pdatapartenza = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@PrezzoMax"; });
                //.Value.ToString("yyyy-MM-dd 00:00:00")  //formato corretto per filtro data
                pdatapartenza.DbType = System.Data.DbType.DateTime;
                _parUsed.Add(pdatapartenza);

                if (!queryfilter.ToLower().Contains("where"))
                    queryfilter += " WHERE JSON_VALID(jsonfield1) and json_extract(jsonfield1, '$.prezzo') <= @PrezzoMax ";
                else
                    queryfilter += " AND JSON_VALID(jsonfield1) and  json_extract(jsonfield1, '$.prezzo') <= @PrezzoMax ";
            }

            query += queryfilter;
            if (activatefiltering)
            {
                dettagliordini = new CarrelloCollection();
                //listacodici = new List<string>();
                Carrello item = new Carrello();
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection, true);
                using (reader)
                {
                    if (reader == null) { return dettagliordini; };
                    if (reader.HasRows == false)
                        return dettagliordini;
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

                        //  Scaglioni scaglionedacarrello = Newtonsoft.Json.JsonConvert.DeserializeObject<Scaglioni>((String)eCommerceDM.Selezionadajson(item.jsonfield1, "scaglione", "I"));
                        dettagliordini.Add(item);
                        // listacodici.Add(item.CodiceOrdine);
                    }
                }
            }


            return dettagliordini;
        }



        /// <summary>
        /// Carica la lista completa degli elementi del carrello in base al codice ordine passato 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Codiceordine"></param>
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

                        if (!reader["Pagatoacconto"].Equals(DBNull.Value))
                            item.Pagatoacconto = reader.GetBoolean(reader.GetOrdinal("Pagatoacconto"));

                        if (!reader["TotaleOrdine"].Equals(DBNull.Value))
                            item.TotaleOrdine = reader.GetDouble(reader.GetOrdinal("TotaleOrdine"));

                        if (!reader["TotaleSconto"].Equals(DBNull.Value))
                            item.TotaleSconto = reader.GetDouble(reader.GetOrdinal("TotaleSconto"));
                        if (!reader["TotaleSpedizione"].Equals(DBNull.Value))
                            item.TotaleSpedizione = reader.GetDouble(reader.GetOrdinal("TotaleSpedizione"));


                        if (!reader["TotaleAssicurazione"].Equals(DBNull.Value))
                            item.TotaleAssicurazione = reader.GetDouble(reader.GetOrdinal("TotaleAssicurazione"));
                        if (!reader["Nassicurazioni"].Equals(DBNull.Value))
                            item.Nassicurazioni = reader.GetInt64(reader.GetOrdinal("Nassicurazioni"));

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

            SQLiteParameter ppagatoacconto = new SQLiteParameter("@Pagatoacconto", item.Pagatoacconto);
            parColl.Add(ppagatoacconto);

            SQLiteParameter ptotaleordine = new SQLiteParameter("@TotaleOrdine", item.TotaleOrdine);// 
            parColl.Add(ptotaleordine);
            SQLiteParameter ptotsconto = new SQLiteParameter("@TotaleSconto", item.TotaleSconto);// 
            parColl.Add(ptotsconto);
            SQLiteParameter pptotspedizione = new SQLiteParameter("@TotaleSpedizione", item.TotaleSpedizione);// 
            parColl.Add(pptotspedizione);

            // TotaleAssicurazione Nassicurazioni
            SQLiteParameter pptotassicurazione = new SQLiteParameter("@TotaleAssicurazione", item.TotaleAssicurazione);// 
            parColl.Add(pptotassicurazione);
            SQLiteParameter pnassicurazioni = new SQLiteParameter("@Nassicurazioni", item.Nassicurazioni);// 
            parColl.Add(pnassicurazioni);


            //valutare se aggiungere il peso

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


            string query = "INSERT INTO TBL_CARRELLO_ORDINI([Indirizzofatturazione],[Indirizzospedizione],[Dataordine],[Id_cliente],[Mailcliente],[Modalitapagamento],[Note],[Urlpagamento],[CodiceOrdine],[Denominazionecliente],[Pagato],[Pagatoacconto],[TotaleOrdine],[TotaleSconto],[TotaleSpedizione],TotaleAssicurazione,Nassicurazioni,TotaleSmaltimento,Supplementospedizione,Id_commerciale,Codicesconto,Percacconto) VALUES (@Indirizzofatturazione,@Indirizzospedizione,@Dataordine,@Id_cliente,@Mailcliente,@Modalitapagamento,@Note,@Urlpagamento,@CodiceOrdine,@Denominazionecliente,@Pagato,@Pagatoacconto,@TotaleOrdine,@TotaleSconto,@TotaleSpedizione,@TotaleAssicurazione,@Nassicurazioni,@TotaleSmaltimento,@Supplementospedizione,@Id_commerciale,@Codicesconto,@Percacconto)";
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
            SQLiteParameter ppagatoacconto = new SQLiteParameter("@Pagatoacconto", item.Pagatoacconto);
            parColl.Add(ppagatoacconto);
            SQLiteParameter ptotaleordine = new SQLiteParameter("@TotaleOrdine", item.TotaleOrdine);// 
            parColl.Add(ptotaleordine);
            SQLiteParameter ptotsconto = new SQLiteParameter("@TotaleSconto", item.TotaleSconto);// 
            parColl.Add(ptotsconto);
            SQLiteParameter pptotspedizione = new SQLiteParameter("@TotaleSpedizione", item.TotaleSpedizione);// 
            parColl.Add(pptotspedizione);
            // TotaleAssicurazione Nassicurazioni
            SQLiteParameter pptotassicurazione = new SQLiteParameter("@TotaleAssicurazione", item.TotaleAssicurazione);// 
            parColl.Add(pptotassicurazione);
            SQLiteParameter pnassicurazioni = new SQLiteParameter("@Nassicurazioni", item.Nassicurazioni);// 
            parColl.Add(pnassicurazioni);

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

            string query = "UPDATE [TBL_CARRELLO_ORDINI] SET [Indirizzofatturazione]=@Indirizzofatturazione,[Indirizzospedizione]=@Indirizzospedizione,[Dataordine]=@Dataordine,[Id_cliente]=@Id_cliente,[Mailcliente]=@Mailcliente,[Modalitapagamento]=@Modalitapagamento,[Note]=@Note,[Urlpagamento]=@Urlpagamento,[CodiceOrdine]=@CodiceOrdine,[Denominazionecliente]=@Denominazionecliente,[Pagato]=@Pagato,[Pagatoacconto]=@Pagatoacconto,[TotaleOrdine]=@TotaleOrdine,[TotaleSconto]=@TotaleSconto,[TotaleSpedizione]=@TotaleSpedizione,[TotaleAssicurazione]=@TotaleAssicurazione,[Nassicurazioni]=@Nassicurazioni,TotaleSmaltimento=@TotaleSmaltimento,Supplementospedizione=@Supplementospedizione,Id_commerciale=@Id_commerciale,Codicesconto=@Codicesconto,Percacconto=@Percacconto WHERE ([ID]=@Id)";

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

        public void DeleteOrdinePerCodice(string connessione, string CodiceOrdine)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (string.IsNullOrEmpty(CodiceOrdine)) return;

            SQLiteParameter p1 = new SQLiteParameter("@CodiceOrdine", CodiceOrdine);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "delete FROM TBL_CARRELLO_ORDINI WHERE ([CodiceOrdine]=@CodiceOrdine)";
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
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Pagatoacconto"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Percacconto"));
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
                    new object[] { t.TotaleSmaltimento + t.TotaleOrdine + t.TotaleSpedizione + t.TotaleAssicurazione - t.TotaleSconto }) + " €"));
                        sb.Append(";");
                        sb.Append(t.Modalitapagamento);
                        sb.Append(";");
                        sb.Append(t.Id_commerciale);
                        sb.Append(";");
                        sb.Append((t == null) ? false : t.Pagato);
                        sb.Append(";");
                        sb.Append((t == null) ? false : t.Pagatoacconto);
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                      new object[] { t.Percacconto })));
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


        public static double Getivabycodice2liv(string codice2liv, string refivacategorie)
        {
            double ret = 0;
            try
            {
                jsonecommerce jsondatiecommerce = Newtonsoft.Json.JsonConvert.DeserializeObject<jsonecommerce>(refivacategorie);
                if (jsondatiecommerce != null && jsondatiecommerce.ivacategorie2liv != null)
                {
                    ivacategorie itemiva = jsondatiecommerce.ivacategorie2liv.FirstOrDefault(i => i.Codice == codice2liv);
                    if (itemiva != null)
                        ret = itemiva.Ivaperc;
                }
            }
            catch { }
            return ret;
        }


        /// <summary>
        /// export ordini in excel completo con scorporo iva da 
        /// </summary>
        /// <param name="DestinationPath"></param>
        /// <param name="CsvFilename"></param>
        /// <param name="list"></param>
        /// <param name="refivacategorie"></param>
        /// <returns></returns>
        public string CreateExcelOrdini(string DestinationPath, string CsvFilename, TotaliCarrelloCollection list, string refivacategorie)
        {
            string retString = "";
            try
            {
                ClientiDM cliDM = new ClientiDM();

                MemoryStream ms = new MemoryStream();
                if (list == null || list.Count == 0)
                    return "";
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Orders");

                ////////////////////////
                ///header 
                ////////////////////////
                int colTarget = 1;
                ws.Cell(1, colTarget).Value = "Data"; colTarget++;
                ws.Cell(1, colTarget).Value = "Codice Ordine"; colTarget++;
                ws.Cell(1, colTarget).Value = "Id Cliente"; colTarget++;
                ws.Cell(1, colTarget).Value = "Email"; colTarget++;
                ws.Cell(1, colTarget).Value = "Telefono"; colTarget++;
                ws.Cell(1, colTarget).Value = "Nome"; colTarget++;
                ws.Cell(1, colTarget).Value = "Imp.Totale"; colTarget++;
                ws.Cell(1, colTarget).Value = "Iva Totale"; colTarget++;
                ws.Cell(1, colTarget).Value = "Sconto"; colTarget++;
                ws.Cell(1, colTarget).Value = "Spedizione"; colTarget++;
                ws.Cell(1, colTarget).Value = "Totale ordine"; colTarget++;
                ws.Cell(1, colTarget).Value = "% acconto"; colTarget++;
                ws.Cell(1, colTarget).Value = "Acconto"; colTarget++;
                ws.Cell(1, colTarget).Value = "Saldo"; colTarget++;
                ws.Cell(1, colTarget).Value = "Pagamento"; colTarget++;
                ws.Cell(1, colTarget).Value = "Pagatoacconto"; colTarget++;
                ws.Cell(1, colTarget).Value = "Pagato"; colTarget++;
                ws.Cell(1, colTarget).Value = "Idcommerciale"; colTarget++;
                ws.Cell(1, colTarget).Value = "Codice sconto"; colTarget++;
                //dati specifici di prodotto ( collegati al catalogo sono dettagli,confezione che variano se si varia le descrizioni prodotto in catalogo )
                ws.Cell(1, colTarget).Value = "Id Prodotto"; colTarget++;
                ws.Cell(1, colTarget).Value = "Codice Pr."; colTarget++;
                ws.Cell(1, colTarget).Value = "Articolo"; colTarget++;
                ws.Cell(1, colTarget).Value = "Dettaglio"; colTarget++;
                ws.Cell(1, colTarget).Value = "Qtà"; colTarget++;
                ws.Cell(1, colTarget).Value = "Prezzo U."; colTarget++;
                ws.Cell(1, colTarget).Value = "Imp. Rigo"; colTarget++;
                ws.Cell(1, colTarget).Value = "Iva Rigo"; colTarget++;
                ws.Cell(1, colTarget).Value = "Iva %"; colTarget++;
                ws.Cell(1, colTarget).Value = "Scaglione"; colTarget++;
                ws.Cell(1, colTarget).Value = "Partenza"; colTarget++;
                ws.Cell(1, colTarget).Value = "Ritorno"; colTarget++;


                /////////////////////////////
                // format
                var rngHeaders = ws.Range(1, 1, 1, colTarget); // The address is relative to rngTable (NOT the worksheet)
                rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngHeaders.Style.Font.Bold = true;
                rngHeaders.Style.Font.FontColor = XLColor.DarkBlue;
                rngHeaders.Style.Fill.BackgroundColor = XLColor.FromHtml("#C6EFCE");// .from();// XLColor.Aqua;
                var rngHTotale = ws.Range(1, 10, 1, 10); // The address is relative to rngTable (NOT the worksheet)
                rngHTotale.Style.Font.FontColor = XLColor.Red;
                ////////////////////////
                ///
                int j = 2;
                foreach (TotaliCarrello t in list)
                {

                    //Totali carrello memorizzati nella tabella ordini
                    string dataordine = string.Format("{0:dd/MM/yyyy HH:mm:ss}", t.Dataordine);
                    string codiceordine = string.Format(t.CodiceOrdine);
                    string idcliente = string.Format(t.Id_cliente.ToString()); //Caricando i dati del cliente per id puoi inserire anche quelli
                    Cliente cli = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcliente);//Dati completi del cliente
                    string telefonocliente = cli != null ? cli.Telefono : "";
                    string mailcliente = string.Format(t.Mailcliente);
                    string nomecliente = string.Format(t.Denominazionecliente.Replace("<br/>", " "));
                    //string imponibileordine = string.Format("");  //imponibile totale ordine ( da calcolare in base ai righi di ordine e iva )
                    //string ivaordine = string.Format("");  //iva totale odine ( da calcolare in base ai righi di ordine e iva )
                    string scontoordine = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                      new object[] { t.TotaleSconto }));
                    string costospedizione = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
             new object[] { t.TotaleSpedizione }));
                    string totaleordine = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
          new object[] { t.TotaleAcconto + t.TotaleSaldo }));
                    string totaleacconto = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
       new object[] { t.TotaleAcconto }));
                    string totalesaldo = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
    new object[] { t.TotaleSaldo }));
                    string modalitapagamento = string.Format(t.Modalitapagamento);
                    string pagato = string.Format(((t == null) ? false : t.Pagato).ToString());
                    string pagatoacconto = string.Format(((t == null) ? false : t.Pagatoacconto).ToString());
                    string percacconto = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                   new object[] { t.Percacconto }));
                    string idcommerciale = string.Format(t.Id_commerciale.ToString());
                    string codicesconto = string.Format(t.Codicesconto);

                    //Carico dal carrello gli articoli
                    eCommerceDM ecmDM = new eCommerceDM();
                    offerteDM offDM = new offerteDM();
                    CarrelloCollection carrellolist = ecmDM.CaricaCarrelloPerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, t.CodiceOrdine);
                    double _imponibileordine = 0;
                    double _ivaordine = 0;

                    List<int> listarighi = new List<int>();
                    if (carrellolist != null)
                        foreach (Carrello itemcarrello in carrellolist)
                        {
                            listarighi.Add(j);

                            double percentualeiva = 0;
                            //prendiamo l'aliquota iva per il prodotto in base alla categoria di 2 livello
                            if (refivacategorie != "" && itemcarrello.Offerta != null)
                            {
                                percentualeiva = Getivabycodice2liv(itemcarrello.Offerta.CodiceCategoria2Liv, refivacategorie);
                            }
                            //prendo preferenzialmente l'aliquota iva memorizzata nel carrello componenti registrati al momento dell'ordine invece che quella nel file json ( che potrebbe cambiare )
                            if (itemcarrello.Iva != 0)
                                percentualeiva = itemcarrello.Iva; //aliquota iva da usare se memorizzata nel carrello al momento della registrazione del rigo di ordine ( DA FARE per evitare modifiche al variare della tabella caratterstiche / aliquote iva )

                            ws.Cell(j, 20).Value = itemcarrello.id_prodotto;
                            ws.Cell(j, 21).Value = itemcarrello.CodiceProdotto;
                            /////////////////////////////////////////////////////////
                            string nomearticolo = itemcarrello.Offerta.DenominazioneI.Replace(",", "").Replace("\"", "").Replace("'", "").Replace(";", "");
                            int i = nomearticolo.IndexOf('\n');
                            string titolo = nomearticolo;
                            string sottotitolo = "";
                            if (i > 0)
                                titolo = nomearticolo.Substring(0, i);
                            if (i > 0)
                                if (nomearticolo.Length >= i + 1)
                                    sottotitolo = nomearticolo.Substring(i + 1);
                            /////////////////////////////////////////////////////////
                            ws.Cell(j, 22).Value = titolo; //dettagli
                            ws.Cell(j, 23).Value = sottotitolo; //confezione
                            ws.Cell(j, 24).Value = string.Format(itemcarrello.Numero.ToString());  //"Qtà"; 
                            ws.Cell(j, 25).Value = string.Format(itemcarrello.Prezzo.ToString());  //"Prezzo Unitario ivato; 
                            double valoreivarigo = itemcarrello.Numero * itemcarrello.Prezzo * (percentualeiva / 100);
                            double imponibilerigo = (itemcarrello.Numero * itemcarrello.Prezzo) - valoreivarigo;
                            _imponibileordine += imponibilerigo;
                            _ivaordine += valoreivarigo;
                            ws.Cell(j, 26).Value = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                              new object[] { imponibilerigo }));  //Imponibile rigo; 
                            ws.Cell(j, 27).Value = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
           new object[] { valoreivarigo }));  //Iva riga ordine; 
                            ws.Cell(j, 28).Value = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
    new object[] { percentualeiva }));  //% Iva; 

                            #region EVENTUALI INTEGRAZIONI PER EXPORT RELATIVO ALLE CARATTERISTICHE COMBINATE E NON ( da modificare per incolonnare su excel invece che nello strigbuider)

                            //string valore1 = (String)eCommerceDM.Selezionadajson(itemcarrello.jsonfield1, "Caratteristica1", "I");
                            //valore1 = references.TestoCaratteristica(0, valore1, "I");
                            //string valore2 = (String)eCommerceDM.Selezionadajson(itemcarrello.jsonfield1, "Caratteristica2", "I");
                            //valore2 = references.TestoCaratteristica(1, valore2, "I");
                            //ws.Cell(j, 29).Value = valore1;
                            //ws.Cell(j, 30).Value = valore2;

                            string datapartenza = Utility.reformatdatetimestring((string)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(itemcarrello.jsonfield1, "datapartenza", "I"));
                            string dataritorno = Utility.reformatdatetimestring((string)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(itemcarrello.jsonfield1, "dataritorno", "I"));

                            string idscaglione = (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(itemcarrello.jsonfield1, "idscaglione", "I");
                            ws.Cell(j, 29).Value = idscaglione;
                            ws.Cell(j, 30).Value = datapartenza;
                            ws.Cell(j, 31).Value = dataritorno;


                            //if (!string.IsNullOrEmpty(itemcarrello.Offerta.Xmlvalue))
                            //{
                            //     
                            //    //recupero le caratteristiche del prodotto
                            //    List<ModelCarCombinate> listCar = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(itemcarrello.Offerta.Xmlvalue);
                            //    ModelCarCombinate item = listCar.Find(e => e.id == itemcarrello.Campo2);
                            //    if (item != null)
                            //        ws.Cell(j, 15).Value = (item.caratteristica1.value + "  -  " + item.caratteristica2.value);
                            //    
                            //}

                            ////sb1.Append(" <div class=\"product-categories muted\">");
                            ////sb1.Append(CommonPage.TestoCategoria(itemcarrello.Offerta.CodiceTipologia, itemcarrello.Offerta.CodiceCategoria, Lingua));
                            ////sb1.Append(" </div>");
                            ////sb1.Append(" <div class=\"product-categories muted\">");
                            ////sb1.Append(CommonPage.TestoCaratteristica(0, itemcarrello.Offerta.Caratteristica1.ToString(), Lingua));
                            ////sb1.Append(" </div>");
                            ////sb1.Append(" <div class=\"product-categories muted\">");
                            ////sb1.Append(CommonPage.TestoCaratteristica(1, itemcarrello.Offerta.Caratteristica2.ToString(), Lingua));
                            ////sb1.Append(" </div>");
                            ////sb1.Append(" <div class=\"product-categories muted\">");
                            ////sb1.Append(TestoSezione(itemcarrello.Offerta.CodiceTipologia));
                            ////sb1.Append(" </div>");

                            //////////////////////////////////////////////////////

                            //if (itemcarrello.Datastart != null && itemcarrello.Dataend != null)
                            //{
                            //    sb1.Append("Periodo dal " + string.Format("{0:dd/MM/yyyy}", itemcarrello.Datastart) + "\r\n");
                            //    sb1.Append(" al " + string.Format("{0:dd/MM/yyyy}", itemcarrello.Dataend) + "\r\n");
                            //}
                            //string ret = "";
                            //Dictionary<string, string> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(itemcarrello.jsonfield1.ToString());
                            //if (dic != null && diitemcarrello.ContainsKey("adulti"))
                            //{
                            //    ret += dic["adulti"];
                            //    sb1.Append("\r\n adulti:" + ret);

                            //}
                            //else
                            //    ret = "";
                            //ret = "";
                            //if (dic != null && diitemcarrello.ContainsKey("bambini"))
                            //{
                            //    ret += dic["bambini"];
                            //    sb1.Append("\r\nbambini:" + ret);


                            //}
                            //else
                            //    ret = "";
                            ////////////////////////////////////////////////////
                            #endregion

                            //Per avere i totali su ogni riga di ordine spostare qui la region sotto
                            #region Valori totali relativi all'ordine dalla tabella carrello ordini
                            //dati ripetuti per ogni rigo di ordine
                            //ws.Cell(i, 1).Value = dataordine;
                            //ws.Cell(i, 2).Value = codiceordine;
                            //ws.Cell(i, 3).Value = idcliente;
                            //ws.Cell(i, 4).Value = mailcliente;
                            //ws.Cell(i, 5).Value = telefonocliente;
                            //ws.Cell(i, 6).Value = nomecliente;
                            //ws.Cell(i, 9).Value = scontoordine;
                            //ws.Cell(i, 10).Value = costospedizione;
                            //ws.Cell(i, 11).Value = totaleordine;
                            //ws.Cell(i, 12).Value = percacconto;
                            //ws.Cell(i, 13).Value = totaleacconto;
                            //ws.Cell(i, 14).Value = totalesaldo;
                            //ws.Cell(i, 15).Value = modalitapagamento;
                            //ws.Cell(i, 16).Value = pagatoacconto;
                            //ws.Cell(i, 17).Value = pagato;
                            //ws.Cell(i, 18).Value = idcommerciale;
                            //ws.Cell(i, 19).Value = codicesconto;
                            #endregion

                            j++;
                        }

                    //aggiorniamo i valori calcolati tramite gli elementi del carrello
                    foreach (int i in listarighi)
                    {

                        #region Valori totali relativi all'ordine dalla tabella carrello ordini
                        //dati solo sulla prima riga di ordine
                        ws.Cell(i, 1).Value = dataordine;
                        ws.Cell(i, 2).Value = codiceordine;
                        ws.Cell(i, 3).Value = idcliente;
                        ws.Cell(i, 4).Value = mailcliente;
                        ws.Cell(i, 5).Value = telefonocliente;
                        ws.Cell(i, 6).Value = nomecliente;
                        ws.Cell(i, 9).Value = scontoordine;
                        ws.Cell(i, 10).Value = costospedizione;
                        ws.Cell(i, 11).Value = totaleordine;
                        ws.Cell(i, 12).Value = percacconto;
                        ws.Cell(i, 13).Value = totaleacconto;
                        ws.Cell(i, 14).Value = totalesaldo;
                        ws.Cell(i, 15).Value = modalitapagamento;
                        ws.Cell(i, 16).Value = pagatoacconto;
                        ws.Cell(i, 17).Value = pagato;
                        ws.Cell(i, 18).Value = idcommerciale;
                        ws.Cell(i, 19).Value = codicesconto;
                        #endregion

                        //Imponibile ed iva totale dell'ordine
                        ws.Cell(i, 7).Value = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                             new object[] { _imponibileordine }));  //imponibile totale ordine ( calcolato in base ai righi di ordine e iva )
                        ws.Cell(i, 8).Value = string.Format(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                          new object[] { _ivaordine }));   //iva totale odine ( calcolato in base ai righi di ordine e iva )
                        break; //Abilitare per avre i totali solo sulla 1 riga di ordine
                    }
                }

                ////////////////////////

                wb.SaveAs(DestinationPath + CsvFilename);
                retString = "";
            }
            catch (Exception err)
            {
                retString = err.Message;
                if (err.InnerException != null)
                    retString += err.InnerException.Message;
            }
            return retString;

        }

        public static object Selezionadajson(object item, string key, string Lingua)
        {
            string ret = "";
            if (item != null && item.ToString() != "")
            {
                try
                {
                    // ret = "<b>" + references.ResMan("basetext", Lingua, "formtesto" + key) + ": " + "</b>";
                    Dictionary<string, object> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(item.ToString());
                    if (dic != null && dic.ContainsKey(key))
                        ret += dic[key];
                    else
                        ret = "";
                }
                catch { }
            }
            return ret;
        }
        private string CreaDettaglioCarrello(string codiceordine)
        {
            StringBuilder sb = new StringBuilder();
            eCommerceDM ecmDM = new eCommerceDM();
            CarrelloCollection carrellolist = ecmDM.CaricaCarrelloPerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);
            foreach (Carrello c in carrellolist)
            {

                sb.Append(" Articolo : ");
                sb.Append(c.Offerta.DenominazioneI.Replace(",", "").Replace("\"", "").Replace("'", "").Replace(";", ""));
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
                {
                    ret += dic["adulti"];
                    sb.Append("\r\n adulti:" + ret);
                    sb.Append("\r\n");
                }
                else
                    ret = "";

                ret = "";
                if (dic != null && dic.ContainsKey("bambini"))
                {
                    ret += dic["bambini"];
                    sb.Append("\r\nbambini:" + ret);
                    sb.Append("\r\n");

                }
                else
                    ret = "";


                sb.Append(c.Numero + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { c.Prezzo }) + " €");
                sb.Append("\r\n");

            }
            return sb.ToString();
        }


        /// <summary>
        /// CArica l'intera lista codici sconto 
        /// opp
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public CodicescontoList CaricaListaSconti(string connection, Codicesconto _params, long page = 1, long pagesize = 0)
        {
            CodicescontoList list = new CodicescontoList();

            if (connection == null || connection == "") return list;
            Codicesconto item = null;
            try
            {
                string query = "";
                string queryfilter = "";
                List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();

                query = "SELECT * FROM TBL_SCONTI  ";

                //if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@id"; }))
                if (_params.Id != 0)
                {
                    //SQLiteParameter pidvalue = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@id"; });
                    SQLiteParameter pidvalue = new SQLiteParameter("@id", _params.Id);
                    _parUsed.Add(pidvalue);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE id like @id ";
                    else
                        queryfilter += " AND id like @id  ";
                }

                //if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idprodotto"; }))
                if (_params.Idprodotto != null)
                {
                    //SQLiteParameter pidcliente = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idprodotto"; });
                    SQLiteParameter pidprodotto = new SQLiteParameter("@idprodotto", _params.Idprodotto);
                    _parUsed.Add(pidprodotto);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idprodotto = @idprodotto ";
                    else
                        queryfilter += " AND idprodotto = @idprodotto  ";
                }

                if (_params.Idcliente != null)
                {
                    //SQLiteParameter pidcliente = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idcliente"; });
                    SQLiteParameter pidcliente = new SQLiteParameter("@idcliente", _params.Idcliente);
                    _parUsed.Add(pidcliente);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idcliente = @idcliente ";
                    else
                        queryfilter += " AND idcliente = @idcliente  ";
                }

                if (!string.IsNullOrEmpty(_params.Testocodicesconto))
                {
                    //SQLiteParameter pCodiceordine = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@codicifiltro"; });
                    SQLiteParameter pCodicesconto = new SQLiteParameter("@testocodicesconto", _params.Testocodicesconto);
                    _parUsed.Add(pCodicesconto);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE testocodicesconto like @testocodicesconto ";
                    else
                        queryfilter += " AND testocodicesconto like @testocodicesconto  ";
                }

                if (!string.IsNullOrEmpty(_params.Codicifiltro))
                {
                    //SQLiteParameter pCodiceordine = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@codicifiltro"; });
                    SQLiteParameter pCodicefiltro = new SQLiteParameter("@codicifiltro", _params.Codicifiltro);
                    _parUsed.Add(pCodicefiltro);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE codicifiltro like @codicifiltro ";
                    else
                        queryfilter += " AND codicifiltro like @codicifiltro  ";
                }

                //if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@datascadenza"; }))
                if (_params.Datascadenza != null)
                {
                    //SQLiteParameter pDataMin = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@datascadenza"; });
                    SQLiteParameter pDataMin = new SQLiteParameter("@datascadenza", _params.Datascadenza);
                    _parUsed.Add(pDataMin);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ( datascadenza <= @datascadenza and datascadenza is not null )";
                    else
                        queryfilter += " AND ( datascadenza <= @datascadenza  and datascadenza is not null ) ";
                }
                else
                {
                    SQLiteParameter dataattuale = new SQLiteParameter("@dataattuale", dbDataAccess.CorrectDatenow(System.DateTime.Now.Date));
                    _parUsed.Add(dataattuale);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE (datascadenza >= @dataattuale or datascadenza is null)  ";
                    else
                        queryfilter += " AND (datascadenza >= @dataattuale or datascadenza is null)  ";
                }
                //SQL
                query += queryfilter;
                query += " order by Id desc ";
                if (pagesize != 0)
                {
                    query += " limit " + (page - 1) * pagesize + "," + pagesize;
                }

                /*CALCOLO IL NUMERO DI RIGHE FILTRATE TOTALI*/
                long totalrecords = dbDataAccess.ExecuteScalar<long>("SELECT count(*) FROM  TBL_SCONTI  " + queryfilter, _parUsed, connection);
                list.Totrecs = totalrecords;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Codicesconto();
                        item.Id = reader.GetInt64(reader.GetOrdinal("id"));
                        if (!reader["usosingolo"].Equals(DBNull.Value))
                            item.Usosingolo = reader.GetBoolean(reader.GetOrdinal("usosingolo"));
                        if (!reader["datascadenza"].Equals(DBNull.Value))
                            item.Datascadenza = reader.GetDateTime(reader.GetOrdinal("datascadenza"));
                        if (!reader["idcliente"].Equals(DBNull.Value))
                            item.Idcliente = reader.GetInt64(reader.GetOrdinal("idcliente"));
                        if (!reader["idprodotto"].Equals(DBNull.Value))
                            item.Idprodotto = reader.GetInt64(reader.GetOrdinal("idprodotto"));
                        if (!reader["codicifiltro"].Equals(DBNull.Value))
                            item.Codicifiltro = reader.GetString(reader.GetOrdinal("codicifiltro"));
                        if (!reader["testocodicesconto"].Equals(DBNull.Value))
                            item.Testocodicesconto = reader.GetString(reader.GetOrdinal("testocodicesconto"));
                        if (!reader["scontonum"].Equals(DBNull.Value))
                            item.Scontonum = reader.GetDouble(reader.GetOrdinal("scontonum"));
                        if (!reader["scontoperc"].Equals(DBNull.Value))
                            item.Scontoperc = reader.GetDouble(reader.GetOrdinal("scontoperc"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Lista Sconti :" + error.Message, error);
            }

            return list;
        }

        public Codicesconto CaricaPerId(string connection, long Id)
        {
            if (connection == null || connection == "") return null;
            if (Id == 0) return null;
            Codicesconto item = new Codicesconto();
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();

            SQLiteParameter p1 = new SQLiteParameter("@id", Id); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT  * FROM TBL_SCONTI  WHERE id = @id ";
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Codicesconto();
                        item.Id = reader.GetInt64(reader.GetOrdinal("id"));
                        if (!reader["usosingolo"].Equals(DBNull.Value))
                            item.Usosingolo = reader.GetBoolean(reader.GetOrdinal("usosingolo"));
                        if (!reader["datascadenza"].Equals(DBNull.Value))
                            item.Datascadenza = reader.GetDateTime(reader.GetOrdinal("datascadenza"));
                        if (!reader["idcliente"].Equals(DBNull.Value))
                            item.Idcliente = reader.GetInt64(reader.GetOrdinal("idcliente"));
                        if (!reader["idprodotto"].Equals(DBNull.Value))
                            item.Idprodotto = reader.GetInt64(reader.GetOrdinal("idprodotto"));
                        if (!reader["codicifiltro"].Equals(DBNull.Value))
                            item.Codicifiltro = reader.GetString(reader.GetOrdinal("codicifiltro"));
                        if (!reader["testocodicesconto"].Equals(DBNull.Value))
                            item.Testocodicesconto = reader.GetString(reader.GetOrdinal("testocodicesconto"));
                        if (!reader["scontonum"].Equals(DBNull.Value))
                            item.Scontonum = reader.GetDouble(reader.GetOrdinal("scontonum"));
                        if (!reader["scontoperc"].Equals(DBNull.Value))
                            item.Scontoperc = reader.GetDouble(reader.GetOrdinal("scontoperc"));

                        break;
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento sconto per id :" + error.Message, error);
            }
            return item;
        }
        public string CancellaSconto(string connection, long Id)
        {
            string ret = "";
            if (connection == null || connection == "") return "connection not specified";
            if (Id == 0) return "id non specificato";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@id", Id);//OleDbType.VarChar
            parColl.Add(p1);
            string query = "DELETE FROM TBL_SCONTI WHERE ([id]=@id)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
            }
            catch (Exception error)
            {
                ret = "Errore, cancellazione  :" + error.Message;
                //throw new ApplicationException("Errore, cancellazione  :" + error.Message, error);
            }
            return ret;
        }
        /// <summary>
        /// Inserisce o aggiorna una email nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InserisciAggiorna(string connessione, Codicesconto item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@usosingolo", item.Usosingolo);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = null;
            //p4 = new SQLiteParameter("@datascadenza", ((item.Datascadenza) == null) ? item.Datascadenza : dbDataAccess.CorrectDatenow(item.Datascadenza.Value));
            p2 = new SQLiteParameter("@datascadenza", item.Datascadenza);
            parColl.Add(p2);

            SQLiteParameter p3 = new SQLiteParameter("@idcliente", item.Idcliente);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@idprodotto", item.Idprodotto);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@codicifiltro", item.Codicifiltro);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@scontonum", item.Scontonum);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@scontoperc", item.Scontoperc);
            parColl.Add(p7);
            SQLiteParameter p8 = new SQLiteParameter("@testocodicesconto", item.Testocodicesconto);
            parColl.Add(p8);

            string query = "";
            if (item.Id != 0)
            {
                //Update
                query = "UPDATE [TBL_SCONTI] SET usosingolo=@usosingolo,datascadenza=@datascadenza,idcliente=@idcliente,idprodotto=@idprodotto,codicifiltro=@codicifiltro,scontonum=@scontonum,scontoperc=@scontoperc,testocodicesconto=@testocodicesconto";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_SCONTI (usosingolo,datascadenza,idcliente,idprodotto,codicifiltro,scontonum,scontoperc,testocodicesconto)";
                query += " values ( ";
                query += " @usosingolo,@datascadenza,@idcliente,@idprodotto,@codicifiltro,@scontonum,@scontoperc,@testocodicesconto )";
            }

            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                if (item.Id == 0) item.Id = lastidentity;  //Inserisco nell'id dell'elemento inseito l'id generato dal db   
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento  :" + error.Message, error);
            }
            return;
        }



    }
}
