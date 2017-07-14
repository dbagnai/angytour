using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

namespace WelcomeLibrary.DAL
{
    public class contenutiDM
    {

        /// <summary>
        /// Carica la lista completa dei contenuti in base al codice passato
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codicecontenuto"></param>
        /// <returns></returns>
        public ContenutiCollection CaricaContenutiPerCodice(string connection, string codicecontenuto)
        {
            if (connection == null || connection == "") return null;
            if (codicecontenuto == null || codicecontenuto == "") return null;
            ContenutiCollection list = new ContenutiCollection();
            Contenuti item;

            try
            {
                string query = "SELECT * FROM TBL_CONTENUTI where CodiceContenuto=@CodiceContenuto order BY DataInserimento Desc";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@CodiceContenuto", codicecontenuto);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Contenuti();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));

                        int _i = 0;
                        if (!(reader["ID_ATTIVITA"]).Equals(DBNull.Value))
                            int.TryParse(reader["ID_ATTIVITA"].ToString(), out _i);
                        item.Id_attivita = _i;
                        item.CodiceContenuto = reader.GetString(reader.GetOrdinal("CodiceContenuto"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.TitoloGB = reader.GetString(reader.GetOrdinal("TitoloGB"));
                        item.TitoloRU = reader.GetString(reader.GetOrdinal("TitoloRU"));
                        item.TitoloI = reader.GetString(reader.GetOrdinal("TitoloI"));


                        if (!(reader["customtitleI"]).Equals(DBNull.Value))
                            item.CustomtitleI = reader.GetString(reader.GetOrdinal("customtitleI"));
                        if (!(reader["customdescI"]).Equals(DBNull.Value))
                            item.CustomdescI = reader.GetString(reader.GetOrdinal("customdescI"));
                        if (!(reader["customtitleGB"]).Equals(DBNull.Value))
                            item.CustomtitleGB = reader.GetString(reader.GetOrdinal("customtitleGB"));
                        if (!(reader["customdescGB"]).Equals(DBNull.Value))
                            item.CustomdescGB = reader.GetString(reader.GetOrdinal("customdescGB"));


                        if (!(reader["FotoSchema"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("FotoSchema"));
                        else
                            item.FotoCollection_M.Schema = "";
                        if (!(reader["FotoValori"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("FotoValori"));
                        else
                            item.FotoCollection_M.Valori = "";
                        //Da completare creando la lista delle foto col percorso corretto per
                        //la visualizzazione
                        item.FotoCollection_M = this.CaricaAllegatiFoto(item.FotoCollection_M);


                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento contenuti :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Carica la lista dei contenuti per codice limitando i risultati agli ultimi maxrecord elementi
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codicecontenuto"></param>
        /// <param name="maxrecord"></param>
        /// <returns></returns>
        public ContenutiCollection CaricaContenutiPerCodice(string connection, string codicecontenuto, string maxrecord, bool caricaofferteassociate = false)
        {
            if (connection == null || connection == "") return null;
            if (codicecontenuto == null || codicecontenuto == "") return null;
            offerteDM offDM = new offerteDM();
            ContenutiCollection list = new ContenutiCollection();
            Contenuti item;

            try
            {
                string query = "SELECT TOP " + maxrecord + " A.*,B.* FROM TBL_CONTENUTI A left join TBL_ATTIVITA B on A.ID_ATTIVITA=B.ID where CodiceContenuto=@CodiceContenuto order BY A.DataInserimento  Desc, A.ID Desc";

                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@CodiceContenuto", codicecontenuto);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Contenuti();
                        item.Id = reader.GetInt32(reader.GetOrdinal("A.ID"));

                        int _i = 0;
                        if (!(reader["ID_ATTIVITA"]).Equals(DBNull.Value))
                            int.TryParse(reader["ID_ATTIVITA"].ToString(), out _i);
                        item.Id_attivita = _i;

                        if (caricaofferteassociate && _i != 0)
                        {
                            Offerte offerta = new Offerte();
                            offerta.Id = reader.GetInt32(reader.GetOrdinal("B.ID"));
                            offerta.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                            offerta.DataInserimento = reader.GetDateTime(reader.GetOrdinal("B.DataInserimento"));
                            offerta.DescrizioneI = reader.GetString(reader.GetOrdinal("B.DescrizioneI"));
                            offerta.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                            if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                                offerta.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                            if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                                offerta.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                            if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                                offerta.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                            if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                                offerta.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));

                            if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                                offerta.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                            if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                                offerta.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

                            if (!reader["DATITECNICII"].Equals(DBNull.Value))
                                offerta.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));


                            offerta.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                            offerta.DescrizioneGB = reader.GetString(reader.GetOrdinal("B.DescrizioneGB"));
                            if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                                offerta.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));


                            if (!reader["B.DescrizioneRU"].Equals(DBNull.Value))
                                offerta.DescrizioneRU = reader.GetString(reader.GetOrdinal("B.DescrizioneRU"));
                            if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                                offerta.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
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
                            if (!reader["Prezzo"].Equals(DBNull.Value))
                                offerta.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));
                            if (!(reader["B.FotoSchema"]).Equals(DBNull.Value))
                                offerta.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("B.FotoSchema"));
                            else
                                offerta.FotoCollection_M.Schema = "";
                            if (!(reader["B.FotoValori"]).Equals(DBNull.Value))
                                offerta.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("B.FotoValori"));
                            else
                                offerta.FotoCollection_M.Valori = "";
                            //Creo la lista delle foto
                            offerta.FotoCollection_M = this.CaricaAllegatiFoto(offerta.FotoCollection_M);
                            item.offertaassociata = offerta;
                        }

                        item.CodiceContenuto = reader.GetString(reader.GetOrdinal("CodiceContenuto"));

                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("A.DataInserimento"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("A.DescrizioneI"));
                        item.TitoloI = reader.GetString(reader.GetOrdinal("TitoloI"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("A.DescrizioneGB"));
                        item.TitoloGB = reader.GetString(reader.GetOrdinal("TitoloGB"));

                        if (!(reader["customtitleI"]).Equals(DBNull.Value))
                            item.CustomtitleI = reader.GetString(reader.GetOrdinal("customtitleI"));
                        if (!(reader["customdescI"]).Equals(DBNull.Value))
                            item.CustomdescI = reader.GetString(reader.GetOrdinal("customdescI"));
                        if (!(reader["customtitleGB"]).Equals(DBNull.Value))
                            item.CustomtitleGB = reader.GetString(reader.GetOrdinal("customtitleGB"));
                        if (!(reader["customdescGB"]).Equals(DBNull.Value))
                            item.CustomdescGB = reader.GetString(reader.GetOrdinal("customdescGB"));

                        if (!(reader["A.DescrizioneRU"]).Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("A.DescrizioneRU"));
                        if (!(reader["TitoloRU"]).Equals(DBNull.Value))
                            item.TitoloRU = reader.GetString(reader.GetOrdinal("TitoloRU"));

                        if (!(reader["A.FotoSchema"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("A.FotoSchema"));
                        else
                            item.FotoCollection_M.Schema = "";
                        if (!(reader["A.FotoValori"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("A.FotoValori"));
                        else
                            item.FotoCollection_M.Valori = "";
                        //Da completare creando la lista delle foto col percorso corretto per
                        //la visualizzazione
                        item.FotoCollection_M = this.CaricaAllegatiFoto(item.FotoCollection_M);


                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento contenuti :" + error.Message, error);
            }

            return list;
        }

        public Contenuti CaricaContenutiPerId(string connection, string idContenuto)
        {
            if (connection == null || connection == "") return null;
            if (idContenuto == null || idContenuto == "") return null;
            Contenuti item = null;

            try
            {
                string query = "SELECT * FROM TBL_CONTENUTI where ID=@ID order BY DataInserimento Desc";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@ID", idContenuto);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Contenuti();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));

                        int _i = 0;
                        if (!(reader["ID_ATTIVITA"]).Equals(DBNull.Value))
                            int.TryParse(reader["ID_ATTIVITA"].ToString(), out _i);
                        item.Id_attivita = _i;

                        item.CodiceContenuto = reader.GetString(reader.GetOrdinal("CodiceContenuto"));

                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.TitoloI = reader.GetString(reader.GetOrdinal("TitoloI"));

                        if (!(reader["customtitleI"]).Equals(DBNull.Value))
                            item.CustomtitleI = reader.GetString(reader.GetOrdinal("customtitleI"));
                        if (!(reader["customdescI"]).Equals(DBNull.Value))
                            item.CustomdescI = reader.GetString(reader.GetOrdinal("customdescI"));
                        if (!(reader["customtitleGB"]).Equals(DBNull.Value))
                            item.CustomtitleGB = reader.GetString(reader.GetOrdinal("customtitleGB"));
                        if (!(reader["customdescGB"]).Equals(DBNull.Value))
                            item.CustomdescGB = reader.GetString(reader.GetOrdinal("customdescGB"));

                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.TitoloGB = reader.GetString(reader.GetOrdinal("TitoloGB"));

                        if (!(reader["DescrizioneRU"]).Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!(reader["TitoloRU"]).Equals(DBNull.Value))
                            item.TitoloRU = reader.GetString(reader.GetOrdinal("TitoloRU"));


                        if (!(reader["FotoSchema"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("FotoSchema"));
                        else
                            item.FotoCollection_M.Schema = "";
                        if (!(reader["FotoValori"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("FotoValori"));
                        else
                            item.FotoCollection_M.Valori = "";
                        //Creo la lista delle foto
                        item.FotoCollection_M = this.CaricaAllegatiFoto(item.FotoCollection_M);

                        return item;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento contenuto :" + error.Message, error);
            }

            return item;
        }

        /// <summary>
        /// IN base alla string URI Passata ritorna il primo contenuto che trova che matcha la stringa passata
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="URI"></param>
        /// <returns></returns>
        public Contenuti CaricaContenutiPerURI(string connection, string URI)
        {
            if (connection == null || connection == "") return null;
            if (URI == null || URI == "") return null;
            Contenuti item = null;

            try
            {
                string query = "SELECT * FROM TBL_CONTENUTI where ( TitoloI like @Titolo or TitoloGB like @Titolo )";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                //OleDbParameter p1 = new OleDbParameter("@Titolo", "%" + URI + "%");//OleDbType.VarChar
                OleDbParameter p1 = new OleDbParameter("@Titolo", "%" + URI );//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Contenuti();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));

                        int _i = 0;
                        if (!(reader["ID_ATTIVITA"]).Equals(DBNull.Value))
                            int.TryParse(reader["ID_ATTIVITA"].ToString(), out _i);
                        item.Id_attivita = _i;

                        item.CodiceContenuto = reader.GetString(reader.GetOrdinal("CodiceContenuto"));

                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.TitoloGB = reader.GetString(reader.GetOrdinal("TitoloGB"));
                        item.TitoloI = reader.GetString(reader.GetOrdinal("TitoloI"));

                        if (!(reader["DescrizioneRU"]).Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!(reader["TitoloRU"]).Equals(DBNull.Value))
                            item.TitoloRU = reader.GetString(reader.GetOrdinal("TitoloRU"));

                        if (!(reader["customtitleI"]).Equals(DBNull.Value))
                            item.CustomtitleI = reader.GetString(reader.GetOrdinal("customtitleI"));
                        if (!(reader["customdescI"]).Equals(DBNull.Value))
                            item.CustomdescI = reader.GetString(reader.GetOrdinal("customdescI"));
                        if (!(reader["customtitleGB"]).Equals(DBNull.Value))
                            item.CustomtitleGB = reader.GetString(reader.GetOrdinal("customtitleGB"));
                        if (!(reader["customdescGB"]).Equals(DBNull.Value))
                            item.CustomdescGB = reader.GetString(reader.GetOrdinal("customdescGB"));

                        if (!(reader["FotoSchema"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("FotoSchema"));
                        else
                            item.FotoCollection_M.Schema = "";
                        if (!(reader["FotoValori"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("FotoValori"));
                        else
                            item.FotoCollection_M.Valori = "";
                        //Creo la lista delle foto
                        item.FotoCollection_M = this.CaricaAllegatiFoto(item.FotoCollection_M);

                        return item;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento contenuto :" + error.Message, error);
            }

            return item;
        }

        /// <summary>
        /// Ricrea la list delle foto a partire dalle stringhe schema e valori
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public AllegatiCollection CaricaAllegatiFoto(AllegatiCollection list)
        {
            Allegato item;
            string Schema = list.Schema;
            string Value = list.Valori;
            int i = 0;
            int j = 0;
            int start = 0;
            int end = 0;

            while (start < Schema.Length)
            {
                item = new Allegato();
                //LEGGIAMO LO SCHEMA PER IL NOMEALLEGATO
                start = Schema.IndexOf(":S:", start) + 3;
                end = Schema.IndexOf(":", start);
                if (end == -1) return list;
                i = Convert.ToInt32(Schema.Substring(start, (end - start)));//Posizione di inizio
                start = end + 1;
                end = Schema.IndexOf(":", start);
                j = Convert.ToInt32(Schema.Substring(start, (end - start)));//N.Caratteri da leggere
                start = end + 1;
                //LEGGIAMO IL VALORE (NOMEALLEGATO)
                item.NomeFile = Value.Substring(i, j);
                item.NomeAnteprima = "Ant" + Value.Substring(i, j); ;
                //LEGGIAMO LO SCHEMA PER LA DESCRIZIONE ALLEGATO
                start = Schema.IndexOf(":S:", start) + 3;
                end = Schema.IndexOf(":", start);
                i = Convert.ToInt32(Schema.Substring(start, (end - start)));//Posizione di inizio
                start = end + 1;
                end = Schema.IndexOf(":", start);
                j = Convert.ToInt32(Schema.Substring(start, (end - start)));//N.Caratteri da leggere
                start = end + 1;
                //LEGGIAMO IL VALORE (descrizione ALLEGATO)
                item.Descrizione = Value.Substring(i, j);
                //LA CARTELLA PER LE FOTO E' SEMPRE LA STESSA
                item.Cartella = "";
                //Inserisco il percorso per la foto di anteprima (Prendo sempre la prima)
                if (list.Count == 0 && item.NomeAnteprima != "Ant" && item.NomeAnteprima != "")
                {
                    list.FotoAnteprima = item.NomeAnteprima;
                }
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Dalla lista delle foto riproduce le stringhe Schema e Valori
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public AllegatiCollection CreaStringheAllegati(AllegatiCollection list)
        {
            //Impacchettiamo gli allegati creando le strinche schema / valori
            //esempio schema Foto1:S:0:13:Descr1:S:13:7:Foto2:S:20:13:Descr2:S:33:7:
            int pos = 0; //Posizione iniziale per lo schema
            int n = 0;
            list.Schema = "";
            list.Valori = "";
            int len = 0;
            foreach (Allegato item in list)
            {
                n += 1;

                //INSERISCO NOME FILE 
                len = item.NomeFile.Length;
                item.NomeFile.Replace(":S:", "SSS");//Elimina eventuali presenze
                //del carattere di separazione dal nomefile
                list.Schema += "All" + n + ":S:" + pos + ":" + len + ":";
                list.Valori += item.NomeFile;
                pos += len;

                //INSERISCO DESCRIZIONE
                len = item.Descrizione.Length;
                item.Descrizione.Replace(":S:", "SSS");//Elimina eventuali presenze
                //del carattere di separazione dalla descrizione
                list.Schema += "Des" + n + ":S:" + pos + ":" + len + ":";
                list.Valori += item.Descrizione;
                pos += len;
            }

            return list;
        }

        public bool insertFoto(string connection, int idContenuto, string nomefile, string descrizione)
        {
            if (connection == "") return false;
            if (idContenuto == 0) return false;
            //Carico le foto preesistenti nel db
            AllegatiCollection FotoColl = this.getListaFotobyId(connection, idContenuto);
            if (FotoColl != null)
            {
                //ALCUNI CONTROLLI SULL'ESISTENZA DELLA FOTO DA INSERIRE
                Allegato F1 = (FotoColl).FindLast(delegate (Allegato agtemp) { return agtemp.NomeFile == nomefile; });
                if (F1 != null) //FOTO TROVATA GIA' ESISTENTE nel db
                {
                    return false;
                }
                //AGGIUNGIAMO LA FOTO ALLA COLLECTION
                Allegato tmp = new Allegato();
                tmp.NomeFile = nomefile;
                tmp.Descrizione = descrizione;
                FotoColl.Add(tmp);
                //RIFORMIAMO LE STRINGHE schema e valori
                //PER IL SALVATAGGIO NEL DB
                FotoColl = this.CreaStringheAllegati(FotoColl);
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@fotoschema", FotoColl.Schema);
                parColl.Add(p1);
                OleDbParameter p2 = new OleDbParameter("@fotovalori", FotoColl.Valori);
                parColl.Add(p2);
                //OleDbParameter p3 = new OleDbParameter("@datainserimento", System.DateTime.Now.ToString());//OleDbType.VarChar
                //parColl.Add(p3);
                OleDbParameter p4 = new OleDbParameter("@id", idContenuto);//OleDbType.VarChar
                parColl.Add(p4);
                string query = "UPDATE [TBL_CONTENUTI] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori  WHERE ([Id]=@id)";
                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento Contenuti Foto :" + error.Message, error);
                }

            }
            return true;
        }


        public bool CancellaFoto(string connection, int idContenuto, string nomefile, string descrizione, string pathfile)
        {

            if (connection == "") return false;
            if (idContenuto == 0) return false;
            //Carico le foto preesistenti nel db
            AllegatiCollection FotoColl = this.getListaFotobyId(connection, idContenuto);
            if (FotoColl != null)
            {

                try
                {
                    //CONTROLLO SULL'ESISTENZA DELLA FOTO DA CANCELLARE
                    Allegato F1 = (FotoColl).FindLast(delegate (Allegato agtemp) { return agtemp.NomeFile == nomefile; });
                    if (F1 == null) //FOTO non TROVATA nel db
                    {
                        return false;
                    }
                    //RIMUOVIAMO LA FOTO DALLA COLLECTION
                    FotoColl.Remove(F1);
                    //RIFORMIAMO LE STRINGHE schema e valori
                    //PER IL SALVATAGGIO NEL DB
                    FotoColl = this.CreaStringheAllegati(FotoColl);
                    List<OleDbParameter> parColl = new List<OleDbParameter>();
                    OleDbParameter p1 = new OleDbParameter("@fotoschema", FotoColl.Schema);
                    parColl.Add(p1);
                    OleDbParameter p2 = new OleDbParameter("@fotovalori", FotoColl.Valori);
                    parColl.Add(p2);
                    //OleDbParameter p3 = new OleDbParameter("@datainserimento", System.DateTime.Now.ToString());//OleDbType.VarChar
                    //parColl.Add(p3);
                    OleDbParameter p4 = new OleDbParameter("@id", idContenuto);//OleDbType.VarChar
                    parColl.Add(p4);
                    string query = "UPDATE [TBL_CONTENUTI] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori  WHERE ([Id]=@id)";
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);

                    //ESEGUIAMO LA CANCELLAZIONE FISICA
                    //DEI FILE IMMAGINE E ANTEPRIMA DAL SERVER
                    if (System.IO.File.Exists(pathfile + "\\" + nomefile)) System.IO.File.Delete(pathfile + "\\" + nomefile);
                    if (System.IO.File.Exists(pathfile + "\\" + "Ant" + nomefile)) System.IO.File.Delete(pathfile + "\\" + "Ant" + nomefile);


                }
                catch (Exception error)
                {
                    throw new ApplicationException("Cancella Foto:" + error.Message, error);
                }

            }
            return true;
        }
        public bool modificaFoto(string connection, int idContenuto, string nomefile, string descrizione)
        {
            if (connection == "") return false;
            if (idContenuto == 0) return false;
            //Carico le foto preesistenti nel db
            AllegatiCollection FotoColl = this.getListaFotobyId(connection, idContenuto);
            if (FotoColl != null)
            {
                //ALCUNI CONTROLLI SULL'ESISTENZA DELLA FOTO DA INSERIRE
                Allegato F1 = (FotoColl).FindLast(delegate (Allegato agtemp) { return agtemp.NomeFile == nomefile; });
                if (F1 == null) //FOTO TROVATA GIA' ESISTENTE nel db
                {
                    return false;
                }
                //MODIFICHIAMO LA FOTO NELLA COLLECTION

                F1.NomeFile = nomefile;
                F1.Descrizione = descrizione;

                //RIFORMIAMO LE STRINGHE schema e valori
                //PER IL SALVATAGGIO NEL DB
                FotoColl = this.CreaStringheAllegati(FotoColl);

                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@fotoschema", FotoColl.Schema);
                parColl.Add(p1);
                OleDbParameter p2 = new OleDbParameter("@fotovalori", FotoColl.Valori);
                parColl.Add(p2);
                //OleDbParameter p3 = new OleDbParameter("@datainserimento", System.DateTime.Now.ToString());//OleDbType.VarChar
                //parColl.Add(p3);
                OleDbParameter p4 = new OleDbParameter("@id", idContenuto);//OleDbType.VarChar
                parColl.Add(p4);
                string query = "UPDATE [TBL_CONTENUTI] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori  WHERE ([Id]=@id)";
                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento Foto Descrizione :" + error.Message, error);
                }

            }
            return true;
        }


        /// <summary>
        /// Carica la collection delle foto a partire dall'id del record del contenuto passato
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idContenuto"></param>
        /// <returns></returns>
        public AllegatiCollection getListaFotobyId(string connection, int idContenuto)
        {
            if (connection == null || connection == "") { return null; };
            if (idContenuto == null || idContenuto == 0) { return null; };

            string query = "SELECT [FotoSchema],[FotoValori] FROM TBL_CONTENUTI where ID=@idContenuto";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@idContenuto", idContenuto);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
            Contenuti item = new Contenuti();
            using (reader)
            {
                if (reader == null) { return null; };
                if (reader.HasRows == false)
                    return null;
                while (reader.Read())
                {
                    item = new Contenuti();
                    item.Id = idContenuto;
                    //  CARICHIAMO 
                    //FOTO ALLEGATE
                    if (!(reader["FotoSchema"]).Equals(DBNull.Value))
                        item.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("FotoSchema"));
                    else
                        item.FotoCollection_M.Schema = "";
                    if (!(reader["FotoValori"]).Equals(DBNull.Value))
                        item.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("FotoValori"));
                    else
                        item.FotoCollection_M.Valori = "";
                    item.FotoCollection_M = this.CaricaAllegatiFoto(item.FotoCollection_M);
                    return item.FotoCollection_M; ; //Ritorna solo 1 record
                }
            }
            return null;
        }

        public void InsertContenuto(string connessione,
        Contenuti item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            OleDbParameter p1 = new OleDbParameter("@codicecontenuto", item.CodiceContenuto);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@titoloi", item.TitoloI);
            parColl.Add(p2);
            OleDbParameter p3 = new OleDbParameter("@titologb", item.TitoloGB);
            parColl.Add(p3);
            OleDbParameter p3r = new OleDbParameter("@titoloru", item.TitoloRU);
            parColl.Add(p3r);
            OleDbParameter p4 = new OleDbParameter("@descrizionei", item.DescrizioneI);
            parColl.Add(p4);
            OleDbParameter p5 = new OleDbParameter("@descrizionegb", item.DescrizioneGB);
            parColl.Add(p5);
            OleDbParameter p5r = new OleDbParameter("@descrizioneru", item.DescrizioneRU);
            parColl.Add(p5r);



            OleDbParameter p6 = new OleDbParameter("@fotoschema", "");//item.FotoCollection_M.Schema
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@fotovalori", "");//item.FotoCollection_M.Valori
            parColl.Add(p7);

            OleDbParameter p8 = new OleDbParameter("@data", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", item.DataInserimento));
            //p8.OleDbType = OleDbType.Date;
            parColl.Add(p8);

            OleDbParameter p9 = new OleDbParameter("@id_attivita", item.Id_attivita);
            parColl.Add(p9);

            OleDbParameter pct1 = new OleDbParameter("@customtitleI", item.CustomtitleI);
            parColl.Add(pct1);
            OleDbParameter pct2 = new OleDbParameter("@customtitleGB", item.CustomtitleGB);
            parColl.Add(pct2);
            OleDbParameter pcd1 = new OleDbParameter("@customdescI", item.CustomdescI);
            parColl.Add(pcd1);
            OleDbParameter pcd2 = new OleDbParameter("@customdescGB", item.CustomdescGB);
            parColl.Add(pcd2);

            string query = "INSERT INTO TBL_CONTENUTI([CodiceContenuto],[TitoloI],[TitoloGB],[TitoloRU],[DescrizioneI],[DescrizioneGB],[DescrizioneRU],[FotoSchema],[FotoValori],[DataInserimento],[Id_attivita],customtitleI,customtitleGB,customdescI,customdescGB) VALUES (@codicecontenuto,@titoloi,@titologb,@titoloru,@descrizionei,@descrizionegb,@descrizioneru,@fotoschema,@fotovalori,@Data,@Id_attivita,@customtitleI,@customtitleGB,@customdescI,@customdescGB)";
            try
            {
                int lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                item.Id = lastidentity; //Inserisco nell'id dell'elemento inseito l'id generato dal db
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento contenuto :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Aggiorna un contenuto nel db
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateContenuti(string connessione,
            Contenuti item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            OleDbParameter p2 = new OleDbParameter("@titoloi", item.TitoloI);
            parColl.Add(p2);
            OleDbParameter p3 = new OleDbParameter("@titologb", item.TitoloGB);
            parColl.Add(p3);
            OleDbParameter p3r = new OleDbParameter("@titoloru", item.TitoloRU);
            parColl.Add(p3r);
            OleDbParameter p4 = new OleDbParameter("@descrizionei", item.DescrizioneI);
            parColl.Add(p4);
            OleDbParameter p5 = new OleDbParameter("@descrizionegb", item.DescrizioneGB);
            parColl.Add(p5);
            OleDbParameter p5r = new OleDbParameter("@descrizioneru", item.DescrizioneRU);
            parColl.Add(p5r);
            //OleDbParameter p6 = new OleDbParameter("@fotoschema", item.FotoCollection_M.Schema);
            //parColl.Add(p6);
            //OleDbParameter p7 = new OleDbParameter("@fotovalori", item.FotoCollection_M.Valori);
            //parColl.Add(p7);

            OleDbParameter p8 = new OleDbParameter("@data", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}", item.DataInserimento));
            //p8.OleDbType = OleDbType.Date;
            parColl.Add(p8);

            OleDbParameter p9 = new OleDbParameter("@Id_attivita", item.Id_attivita);
            parColl.Add(p9);

            OleDbParameter pct1 = new OleDbParameter("@customtitleI", item.CustomtitleI);
            parColl.Add(pct1);
            OleDbParameter pct2 = new OleDbParameter("@customtitleGB", item.CustomtitleGB);
            parColl.Add(pct2);
            OleDbParameter pcd1 = new OleDbParameter("@customdescI", item.CustomdescI);
            parColl.Add(pcd1);
            OleDbParameter pcd2 = new OleDbParameter("@customdescGB", item.CustomdescGB);
            parColl.Add(pcd2);


            OleDbParameter p1 = new OleDbParameter("@id", item.Id);//OleDbType.VarChar
            parColl.Add(p1);


            string query = "UPDATE [TBL_CONTENUTI] SET [TitoloI]=@titoloi,[TitoloGB]=@titologb,[TitoloRU]=@titoloru,[DescrizioneI]=@descrizionei,[DescrizioneGB]=@descrizionegb,[DescrizioneRU]=@descrizioneru,DataInserimento=@data,Id_attivita=@Id_attivita,customtitleI=@customtitleI,customtitleGB=@customtitleGB,customdescI=@customdescI,customdescGB=@customdescGB   WHERE ([Id]=@id)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornamento Contenuti :" + error.Message, error);
            }
            return;
        }

        public void DeleteContenuti(string connessione,
                Contenuti item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (item == null || item.Id == 0) return;

            OleDbParameter p1 = new OleDbParameter("@id", item.Id);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "DELETE FROM TBL_CONTENUTI WHERE ([ID]=@id)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione Contenuti :" + error.Message, error);
            }
            return;
        }


    }
}
