using System;
using System.Collections.Generic;
using System.Text;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;
using System.Data.SQLite;

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
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@CodiceContenuto", codicecontenuto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Contenuti();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));

                        long _i = 0;
                        if (!(reader["ID_ATTIVITA"]).Equals(DBNull.Value))
                            long.TryParse(reader["ID_ATTIVITA"].ToString(), out _i);
                        item.Id_attivita = _i;
                        item.CodiceContenuto = reader.GetString(reader.GetOrdinal("CodiceContenuto"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!(reader["DescrizioneFR"]).Equals(DBNull.Value)) item.DescrizioneFR = reader.GetString(reader.GetOrdinal("DescrizioneFR"));
                        if (!(reader["DescrizioneES"]).Equals(DBNull.Value)) item.DescrizioneES = reader.GetString(reader.GetOrdinal("DescrizioneES"));
                        if (!(reader["DescrizioneDE"]).Equals(DBNull.Value)) item.DescrizioneDE = reader.GetString(reader.GetOrdinal("DescrizioneDE"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.TitoloGB = reader.GetString(reader.GetOrdinal("TitoloGB"));
                        item.TitoloRU = reader.GetString(reader.GetOrdinal("TitoloRU"));
                        if (!(reader["TitoloFR"]).Equals(DBNull.Value)) item.TitoloFR = reader.GetString(reader.GetOrdinal("TitoloFR"));
                        if (!(reader["TitoloDE"]).Equals(DBNull.Value)) item.TitoloDE = reader.GetString(reader.GetOrdinal("TitoloDE"));
                        if (!(reader["TitoloES"]).Equals(DBNull.Value)) item.TitoloES = reader.GetString(reader.GetOrdinal("TitoloES"));
                        item.TitoloI = reader.GetString(reader.GetOrdinal("TitoloI"));


                        if (!(reader["customtitleI"]).Equals(DBNull.Value))
                            item.CustomtitleI = reader.GetString(reader.GetOrdinal("customtitleI"));
                        if (!(reader["customdescI"]).Equals(DBNull.Value))
                            item.CustomdescI = reader.GetString(reader.GetOrdinal("customdescI"));
                        if (!(reader["customtitleGB"]).Equals(DBNull.Value))
                            item.CustomtitleGB = reader.GetString(reader.GetOrdinal("customtitleGB"));
                        if (!(reader["customdescGB"]).Equals(DBNull.Value))
                            item.CustomdescGB = reader.GetString(reader.GetOrdinal("customdescGB"));
                        if (!(reader["customtitleRU"]).Equals(DBNull.Value))
                            item.CustomtitleRU = reader.GetString(reader.GetOrdinal("customtitleRU"));
                        if (!(reader["customdescRU"]).Equals(DBNull.Value))
                            item.CustomdescRU = reader.GetString(reader.GetOrdinal("customdescRU"));
                        if (!(reader["customtitleFR"]).Equals(DBNull.Value))
                            item.CustomtitleFR = reader.GetString(reader.GetOrdinal("customtitleFR"));
                        if (!(reader["customdescFR"]).Equals(DBNull.Value))
                            item.CustomdescFR = reader.GetString(reader.GetOrdinal("customdescFR"));

                        if (!(reader["customtitleDE"]).Equals(DBNull.Value))
                            item.CustomtitleDE = reader.GetString(reader.GetOrdinal("customtitleDE"));
                        if (!(reader["customdescDE"]).Equals(DBNull.Value))
                            item.CustomdescDE = reader.GetString(reader.GetOrdinal("customdescDE"));

                        if (!(reader["customtitleES"]).Equals(DBNull.Value))
                            item.CustomtitleES = reader.GetString(reader.GetOrdinal("customtitleES"));
                        if (!(reader["customdescES"]).Equals(DBNull.Value))
                            item.CustomdescES = reader.GetString(reader.GetOrdinal("customdescES"));


                        if (!(reader["canonicalGB"]).Equals(DBNull.Value))
                            item.CanonicalGB = reader.GetString(reader.GetOrdinal("canonicalGB"));
                        if (!(reader["canonicalI"]).Equals(DBNull.Value))
                            item.CanonicalI = reader.GetString(reader.GetOrdinal("canonicalI"));
                        if (!(reader["canonicalRU"]).Equals(DBNull.Value))
                            item.CanonicalRU = reader.GetString(reader.GetOrdinal("canonicalRU"));
                        if (!(reader["canonicalFR"]).Equals(DBNull.Value))
                            item.CanonicalFR = reader.GetString(reader.GetOrdinal("canonicalFR"));
                        if (!(reader["canonicalDE"]).Equals(DBNull.Value))
                            item.CanonicalDE = reader.GetString(reader.GetOrdinal("canonicalDE"));
                        if (!(reader["canonicalES"]).Equals(DBNull.Value))
                            item.CanonicalES = reader.GetString(reader.GetOrdinal("canonicalES"));
                        if (!(reader["robots"]).Equals(DBNull.Value))
                            item.Robots = reader.GetString(reader.GetOrdinal("robots"));

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
                string query = "SELECT  A.ID as A_ID,A.ID_ATTIVITA,A.CodiceContenuto,A.DataInserimento as A_DataInserimento,A.DescrizioneI as A_DescrizioneI,A.TitoloI,A.DescrizioneGB as A_DescrizioneGB,A.TitoloGB,A.customtitleI,A.customdescI,A.customtitleGB,A.customdescGB,A.customtitleRU,A.customtitleFR,A.customtitleDE,A.customtitleES,A.customdescRU,A.DescrizioneRU as A_DescrizioneRU,A.TitoloRU,A.customdescFR,A.DescrizioneFR as A_DescrizioneFR,A.TitoloFR,A.customdescDE,A.DescrizioneDE as A_DescrizioneDE,A.TitoloDE,A.customdescES,A.DescrizioneES as A_DescrizioneES,A.TitoloES,A.FotoSchema as  A_FotoSchema,A.FotoValori as A_FotoValori,A.canonicalI as A_canonicalI,A.canonicalGB as A_canonicalGB,A.canonicalRU as A_canonicalRU,A.canonicalFR as A_canonicalFR,A.canonicalDE as A_canonicalDE,A.canonicalES as A_canonicalES,A.robots as A_robots" +
                    ",B.ID as B_ID,B.CodiceTIPOLOGIA,B.DataInserimento as B_DataInserimento,B.DescrizioneI as B_DescrizioneI,B.DENOMINAZIONEI,B.CodiceCOMUNE,B.CodicePROVINCIA,B.CodiceREGIONE,B.CodiceProdotto,B.CodiceCategoria,B.CodiceCategoria2Liv,B.DATITECNICII,B.DENOMINAZIONEGB,B.DescrizioneGB as B_DescrizioneGB,B.DATITECNICIGB,B.DescrizioneRU as B_DescrizioneRU,B.DENOMINAZIONERU,B.DATITECNICIRU,B.DescrizioneFR as B_DescrizioneFR,B.DENOMINAZIONEFR,B.DATITECNICIFR,B.DescrizioneDE as B_DescrizioneDE,B.DENOMINAZIONEDE,B.DATITECNICIDE,B.DescrizioneES as B_DescrizioneES,B.DENOMINAZIONEES,B.DATITECNICIES,B.EMAIL,B.FAX,B.INDIRIZZO,B.TELEFONO,B.WEBSITE,B.Prezzo,B.FotoSchema as B_FotoSchema,B.FotoValori as B_FotoValori,B.canonicalI as B_canonicalI,B.canonicalGB as B_canonicalGB,B.canonicalRU as B_canonicalRU,B.canonicalFR as B_canonicalFR,B.canonicalDE as B_canonicalDE,B.canonicalES as B_canonicalES,B.urlcustomI,B.urlcustomGB,B.urlcustomRU,B.urlcustomFR,B.urlcustomDE,B.urlcustomES,B.robots as B_robots " +
                    "FROM TBL_CONTENUTI A left join TBL_ATTIVITA B on A.ID_ATTIVITA=B.ID where CodiceContenuto=@CodiceContenuto order BY A.DataInserimento  Desc, A.ID Desc limit " + maxrecord;

                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@CodiceContenuto", codicecontenuto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Contenuti();
                        item.Id = reader.GetInt64(reader.GetOrdinal("A_ID"));

                        long _i = 0;
                        if (!(reader["ID_ATTIVITA"]).Equals(DBNull.Value))
                            long.TryParse(reader["ID_ATTIVITA"].ToString(), out _i);
                        item.Id_attivita = _i;

                        if (caricaofferteassociate && _i != 0)
                        {
                            Offerte offerta = new Offerte();
                            offerta.Id = reader.GetInt64(reader.GetOrdinal("B_ID"));
                            offerta.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                            offerta.DataInserimento = reader.GetDateTime(reader.GetOrdinal("B_DataInserimento"));

                            offerta.DescrizioneI = reader.GetString(reader.GetOrdinal("B_DescrizioneI"));
                            offerta.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                            if (!reader["DATITECNICII"].Equals(DBNull.Value))
                                offerta.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));

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


                            offerta.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                            offerta.DescrizioneGB = reader.GetString(reader.GetOrdinal("B_DescrizioneGB"));
                            if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                                offerta.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));


                            if (!reader["B_DescrizioneRU"].Equals(DBNull.Value))
                                offerta.DescrizioneRU = reader.GetString(reader.GetOrdinal("B_DescrizioneRU"));
                            if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                                offerta.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                            if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                                offerta.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));

                            if (!reader["B_DescrizioneFR"].Equals(DBNull.Value))
                                offerta.DescrizioneFR = reader.GetString(reader.GetOrdinal("B_DescrizioneFR"));
                            if (!reader["DENOMINAZIONEFR"].Equals(DBNull.Value))
                                offerta.DenominazioneFR = reader.GetString(reader.GetOrdinal("DENOMINAZIONEFR"));
                            if (!reader["DATITECNICIFR"].Equals(DBNull.Value))
                                offerta.DatitecniciFR = reader.GetString(reader.GetOrdinal("DATITECNICIFR"));

                            if (!reader["B_DescrizioneDE"].Equals(DBNull.Value))
                                offerta.DescrizioneDE = reader.GetString(reader.GetOrdinal("B_DescrizioneDE"));
                            if (!reader["DENOMINAZIONEDE"].Equals(DBNull.Value))
                                offerta.DenominazioneDE = reader.GetString(reader.GetOrdinal("DENOMINAZIONEDE"));
                            if (!reader["DATITECNICIDE"].Equals(DBNull.Value))
                                offerta.DatitecniciDE = reader.GetString(reader.GetOrdinal("DATITECNICIDE"));

                            if (!reader["B_DescrizioneES"].Equals(DBNull.Value))
                                offerta.DescrizioneES = reader.GetString(reader.GetOrdinal("B_DescrizioneES"));
                            if (!reader["DENOMINAZIONEES"].Equals(DBNull.Value))
                                offerta.DenominazioneES = reader.GetString(reader.GetOrdinal("DENOMINAZIONEES"));
                            if (!reader["DATITECNICIES"].Equals(DBNull.Value))
                                offerta.DatitecniciES = reader.GetString(reader.GetOrdinal("DATITECNICIES"));


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
                            if (!(reader["B_FotoSchema"]).Equals(DBNull.Value))
                                offerta.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("B_FotoSchema"));
                            else
                                offerta.FotoCollection_M.Schema = "";
                            if (!(reader["B_FotoValori"]).Equals(DBNull.Value))
                                offerta.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("B_FotoValori"));
                            else
                                offerta.FotoCollection_M.Valori = "";


                            if (!reader["urlcustomI"].Equals(DBNull.Value))
                                offerta.UrlcustomI = reader.GetString(reader.GetOrdinal("urlcustomI"));
                            if (!reader["urlcustomGB"].Equals(DBNull.Value))
                                offerta.UrlcustomGB = reader.GetString(reader.GetOrdinal("urlcustomGB"));
                            if (!reader["urlcustomRU"].Equals(DBNull.Value))
                                offerta.UrlcustomRU = reader.GetString(reader.GetOrdinal("urlcustomRU"));
                            if (!reader["urlcustomFR"].Equals(DBNull.Value))
                                offerta.UrlcustomFR = reader.GetString(reader.GetOrdinal("urlcustomFR"));

                            if (!reader["urlcustomES"].Equals(DBNull.Value))
                                offerta.UrlcustomES = reader.GetString(reader.GetOrdinal("urlcustomES"));
                            if (!reader["urlcustomDE"].Equals(DBNull.Value))
                                offerta.UrlcustomDE = reader.GetString(reader.GetOrdinal("urlcustomDE"));

                            if (!reader["B_canonicalGB"].Equals(DBNull.Value))
                                offerta.CanonicalGB = reader.GetString(reader.GetOrdinal("B_canonicalGB"));
                            if (!reader["B_canonicalI"].Equals(DBNull.Value))
                                offerta.CanonicalI = reader.GetString(reader.GetOrdinal("B_canonicalI"));
                            if (!reader["B_canonicalRU"].Equals(DBNull.Value))
                                offerta.CanonicalRU = reader.GetString(reader.GetOrdinal("B_canonicalRU"));
                            if (!reader["B_canonicalFR"].Equals(DBNull.Value))
                                offerta.CanonicalFR = reader.GetString(reader.GetOrdinal("B_canonicalFR"));

                            if (!reader["B_canonicalDE"].Equals(DBNull.Value))
                                offerta.CanonicalDE = reader.GetString(reader.GetOrdinal("B_canonicalDE"));
                            if (!reader["B_canonicalES"].Equals(DBNull.Value))
                                offerta.CanonicalES = reader.GetString(reader.GetOrdinal("B_canonicalES"));

                            if (!reader["B_robots"].Equals(DBNull.Value))
                                offerta.Robots = reader.GetString(reader.GetOrdinal("B_robots"));



                            //Creo la lista delle foto
                            offerta.FotoCollection_M = this.CaricaAllegatiFoto(offerta.FotoCollection_M);
                            item.offertaassociata = offerta;
                        }

                        item.CodiceContenuto = reader.GetString(reader.GetOrdinal("CodiceContenuto"));

                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("A_DataInserimento"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("A_DescrizioneI"));
                        item.TitoloI = reader.GetString(reader.GetOrdinal("TitoloI"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("A_DescrizioneGB"));
                        item.TitoloGB = reader.GetString(reader.GetOrdinal("TitoloGB"));

                        if (!(reader["A_DescrizioneRU"]).Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("A_DescrizioneRU"));
                        if (!(reader["TitoloRU"]).Equals(DBNull.Value))
                            item.TitoloRU = reader.GetString(reader.GetOrdinal("TitoloRU"));

                        if (!(reader["A_DescrizioneFR"]).Equals(DBNull.Value))
                            item.DescrizioneFR = reader.GetString(reader.GetOrdinal("A_DescrizioneFR"));
                        if (!(reader["TitoloFR"]).Equals(DBNull.Value))
                            item.TitoloFR = reader.GetString(reader.GetOrdinal("TitoloFR"));

                        if (!(reader["A_DescrizioneES"]).Equals(DBNull.Value))
                            item.DescrizioneES = reader.GetString(reader.GetOrdinal("A_DescrizioneES"));
                        if (!(reader["TitoloES"]).Equals(DBNull.Value))
                            item.TitoloES = reader.GetString(reader.GetOrdinal("TitoloES"));

                        if (!(reader["A_DescrizioneDE"]).Equals(DBNull.Value))
                            item.DescrizioneDE = reader.GetString(reader.GetOrdinal("A_DescrizioneDE"));
                        if (!(reader["TitoloDE"]).Equals(DBNull.Value))
                            item.TitoloDE = reader.GetString(reader.GetOrdinal("TitoloDE"));

                        if (!(reader["customtitleI"]).Equals(DBNull.Value))
                            item.CustomtitleI = reader.GetString(reader.GetOrdinal("customtitleI"));
                        if (!(reader["customdescI"]).Equals(DBNull.Value))
                            item.CustomdescI = reader.GetString(reader.GetOrdinal("customdescI"));
                        if (!(reader["customtitleGB"]).Equals(DBNull.Value))
                            item.CustomtitleGB = reader.GetString(reader.GetOrdinal("customtitleGB"));
                        if (!(reader["customdescGB"]).Equals(DBNull.Value))
                            item.CustomdescGB = reader.GetString(reader.GetOrdinal("customdescGB"));
                        if (!(reader["customtitleRU"]).Equals(DBNull.Value))
                            item.CustomtitleRU = reader.GetString(reader.GetOrdinal("customtitleRU"));
                        if (!(reader["customdescRU"]).Equals(DBNull.Value))
                            item.CustomdescRU = reader.GetString(reader.GetOrdinal("customdescRU"));
                        if (!(reader["customtitleFR"]).Equals(DBNull.Value))
                            item.CustomtitleFR = reader.GetString(reader.GetOrdinal("customtitleFR"));
                        if (!(reader["customdescFR"]).Equals(DBNull.Value))
                            item.CustomdescFR = reader.GetString(reader.GetOrdinal("customdescFR"));

                        if (!(reader["customtitleDE"]).Equals(DBNull.Value))
                            item.CustomtitleDE = reader.GetString(reader.GetOrdinal("customtitleDE"));
                        if (!(reader["customdescDE"]).Equals(DBNull.Value))
                            item.CustomdescDE = reader.GetString(reader.GetOrdinal("customdescDE"));

                        if (!(reader["customtitleES"]).Equals(DBNull.Value))
                            item.CustomtitleES = reader.GetString(reader.GetOrdinal("customtitleES"));
                        if (!(reader["customdescES"]).Equals(DBNull.Value))
                            item.CustomdescES = reader.GetString(reader.GetOrdinal("customdescES"));



                        if (!reader["A_canonicalGB"].Equals(DBNull.Value))
                            item.CanonicalGB = reader.GetString(reader.GetOrdinal("A_canonicalGB"));
                        if (!reader["A_canonicalI"].Equals(DBNull.Value))
                            item.CanonicalI = reader.GetString(reader.GetOrdinal("A_canonicalI"));
                        if (!reader["A_canonicalRU"].Equals(DBNull.Value))
                            item.CanonicalRU = reader.GetString(reader.GetOrdinal("A_canonicalRU"));
                        if (!reader["A_canonicalFR"].Equals(DBNull.Value))
                            item.CanonicalFR = reader.GetString(reader.GetOrdinal("A_canonicalFR"));

                        if (!reader["A_canonicalES"].Equals(DBNull.Value))
                            item.CanonicalES = reader.GetString(reader.GetOrdinal("A_canonicalES"));

                        if (!reader["A_canonicalDE"].Equals(DBNull.Value))
                            item.CanonicalDE = reader.GetString(reader.GetOrdinal("A_canonicalDE"));


                        if (!(reader["A_FotoSchema"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Schema = reader.GetString(reader.GetOrdinal("A_FotoSchema"));
                        else
                            item.FotoCollection_M.Schema = "";
                        if (!(reader["A_FotoValori"]).Equals(DBNull.Value))
                            item.FotoCollection_M.Valori = reader.GetString(reader.GetOrdinal("A_FotoValori"));
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
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@ID", idContenuto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Contenuti();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));

                        long _i = 0;
                        if (!(reader["ID_ATTIVITA"]).Equals(DBNull.Value))
                            long.TryParse(reader["ID_ATTIVITA"].ToString(), out _i);
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
                        if (!(reader["customtitleRU"]).Equals(DBNull.Value))
                            item.CustomtitleRU = reader.GetString(reader.GetOrdinal("customtitleRU"));
                        if (!(reader["customdescRU"]).Equals(DBNull.Value))
                            item.CustomdescRU = reader.GetString(reader.GetOrdinal("customdescRU"));
                        if (!(reader["customtitleFR"]).Equals(DBNull.Value))
                            item.CustomtitleFR = reader.GetString(reader.GetOrdinal("customtitleFR"));
                        if (!(reader["customdescFR"]).Equals(DBNull.Value))
                            item.CustomdescFR = reader.GetString(reader.GetOrdinal("customdescFR"));

                        if (!(reader["customtitleDE"]).Equals(DBNull.Value))
                            item.CustomtitleDE = reader.GetString(reader.GetOrdinal("customtitleDE"));
                        if (!(reader["customdescDE"]).Equals(DBNull.Value))
                            item.CustomdescDE = reader.GetString(reader.GetOrdinal("customdescDE"));

                        if (!(reader["customtitleES"]).Equals(DBNull.Value))
                            item.CustomtitleES = reader.GetString(reader.GetOrdinal("customtitleES"));
                        if (!(reader["customdescES"]).Equals(DBNull.Value))
                            item.CustomdescES = reader.GetString(reader.GetOrdinal("customdescES"));


                        if (!(reader["canonicalGB"]).Equals(DBNull.Value))
                            item.CanonicalGB = reader.GetString(reader.GetOrdinal("canonicalGB"));
                        if (!(reader["canonicalI"]).Equals(DBNull.Value))
                            item.CanonicalI = reader.GetString(reader.GetOrdinal("canonicalI"));
                        if (!(reader["canonicalRU"]).Equals(DBNull.Value))
                            item.CanonicalRU = reader.GetString(reader.GetOrdinal("canonicalRU"));
                        if (!(reader["canonicalFR"]).Equals(DBNull.Value))
                            item.CanonicalFR = reader.GetString(reader.GetOrdinal("canonicalFR"));

                        if (!(reader["canonicalES"]).Equals(DBNull.Value))
                            item.CanonicalES = reader.GetString(reader.GetOrdinal("canonicalES"));

                        if (!(reader["canonicalDE"]).Equals(DBNull.Value))
                            item.CanonicalDE = reader.GetString(reader.GetOrdinal("canonicalDE"));

                        if (!(reader["robots"]).Equals(DBNull.Value))
                            item.Robots = reader.GetString(reader.GetOrdinal("robots"));

                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.TitoloGB = reader.GetString(reader.GetOrdinal("TitoloGB"));

                        if (!(reader["DescrizioneRU"]).Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!(reader["TitoloRU"]).Equals(DBNull.Value))
                            item.TitoloRU = reader.GetString(reader.GetOrdinal("TitoloRU"));


                        if (!(reader["DescrizioneFR"]).Equals(DBNull.Value))
                            item.DescrizioneFR = reader.GetString(reader.GetOrdinal("DescrizioneFR"));
                        if (!(reader["TitoloFR"]).Equals(DBNull.Value))
                            item.TitoloFR = reader.GetString(reader.GetOrdinal("TitoloFR"));

                        if (!(reader["DescrizioneDE"]).Equals(DBNull.Value))
                            item.DescrizioneDE = reader.GetString(reader.GetOrdinal("DescrizioneDE"));
                        if (!(reader["TitoloDE"]).Equals(DBNull.Value))
                            item.TitoloDE = reader.GetString(reader.GetOrdinal("TitoloDE"));

                        if (!(reader["DescrizioneES"]).Equals(DBNull.Value))
                            item.DescrizioneES = reader.GetString(reader.GetOrdinal("DescrizioneES"));
                        if (!(reader["TitoloES"]).Equals(DBNull.Value))
                            item.TitoloES = reader.GetString(reader.GetOrdinal("TitoloES"));

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
                string query = "SELECT * FROM TBL_CONTENUTI where ( TitoloI like @Titolo or TitoloGB like @Titolo or TitoloRU like @Titolo or TitoloFR like @Titolo  or TitoloDE like @Titolo  or TitoloES like @Titolo )";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                //SQLiteParameter p1 = new SQLiteParameter("@Titolo", "%" + URI + "%");//OleDbType.VarChar
                SQLiteParameter p1 = new SQLiteParameter("@Titolo", "%" + URI);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Contenuti();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));

                        long _i = 0;
                        if (!(reader["ID_ATTIVITA"]).Equals(DBNull.Value))
                            long.TryParse(reader["ID_ATTIVITA"].ToString(), out _i);
                        item.Id_attivita = _i;

                        item.CodiceContenuto = reader.GetString(reader.GetOrdinal("CodiceContenuto"));

                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.TitoloGB = reader.GetString(reader.GetOrdinal("TitoloGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.TitoloI = reader.GetString(reader.GetOrdinal("TitoloI"));

                        if (!(reader["DescrizioneRU"]).Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!(reader["TitoloRU"]).Equals(DBNull.Value))
                            item.TitoloRU = reader.GetString(reader.GetOrdinal("TitoloRU"));
                        if (!(reader["DescrizioneFR"]).Equals(DBNull.Value))
                            item.DescrizioneFR = reader.GetString(reader.GetOrdinal("DescrizioneFR"));
                        if (!(reader["TitoloFR"]).Equals(DBNull.Value))
                            item.TitoloFR = reader.GetString(reader.GetOrdinal("TitoloFR"));

                        if (!(reader["DescrizioneES"]).Equals(DBNull.Value))
                            item.DescrizioneES = reader.GetString(reader.GetOrdinal("DescrizioneES"));
                        if (!(reader["TitoloES"]).Equals(DBNull.Value))
                            item.TitoloES = reader.GetString(reader.GetOrdinal("TitoloES"));

                        if (!(reader["DescrizioneDE"]).Equals(DBNull.Value))
                            item.DescrizioneDE = reader.GetString(reader.GetOrdinal("DescrizioneDE"));
                        if (!(reader["TitoloDE"]).Equals(DBNull.Value))
                            item.TitoloDE = reader.GetString(reader.GetOrdinal("TitoloDE"));

                        if (!(reader["customtitleI"]).Equals(DBNull.Value))
                            item.CustomtitleI = reader.GetString(reader.GetOrdinal("customtitleI"));
                        if (!(reader["customdescI"]).Equals(DBNull.Value))
                            item.CustomdescI = reader.GetString(reader.GetOrdinal("customdescI"));
                        if (!(reader["customtitleGB"]).Equals(DBNull.Value))
                            item.CustomtitleGB = reader.GetString(reader.GetOrdinal("customtitleGB"));
                        if (!(reader["customdescGB"]).Equals(DBNull.Value))
                            item.CustomdescGB = reader.GetString(reader.GetOrdinal("customdescGB"));
                        if (!(reader["customtitleRU"]).Equals(DBNull.Value))
                            item.CustomtitleRU = reader.GetString(reader.GetOrdinal("customtitleRU"));
                        if (!(reader["customdescRU"]).Equals(DBNull.Value))
                            item.CustomdescRU = reader.GetString(reader.GetOrdinal("customdescRU"));
                        if (!(reader["customtitleFR"]).Equals(DBNull.Value))
                            item.CustomtitleFR = reader.GetString(reader.GetOrdinal("customtitleFR"));
                        if (!(reader["customdescFR"]).Equals(DBNull.Value))
                            item.CustomdescFR = reader.GetString(reader.GetOrdinal("customdescFR"));

                        if (!(reader["customtitleDE"]).Equals(DBNull.Value))
                            item.CustomtitleDE = reader.GetString(reader.GetOrdinal("customtitleDE"));
                        if (!(reader["customdescDE"]).Equals(DBNull.Value))
                            item.CustomdescDE = reader.GetString(reader.GetOrdinal("customdescDE"));

                        if (!(reader["customtitleES"]).Equals(DBNull.Value))
                            item.CustomtitleES = reader.GetString(reader.GetOrdinal("customtitleES"));
                        if (!(reader["customdescES"]).Equals(DBNull.Value))
                            item.CustomdescES = reader.GetString(reader.GetOrdinal("customdescES"));


                        if (!(reader["canonicalGB"]).Equals(DBNull.Value))
                            item.CanonicalGB = reader.GetString(reader.GetOrdinal("canonicalGB"));
                        if (!(reader["canonicalI"]).Equals(DBNull.Value))
                            item.CanonicalI = reader.GetString(reader.GetOrdinal("canonicalI"));
                        if (!(reader["canonicalRU"]).Equals(DBNull.Value))
                            item.CanonicalRU = reader.GetString(reader.GetOrdinal("canonicalRU"));
                        if (!(reader["canonicalFR"]).Equals(DBNull.Value))
                            item.CanonicalFR = reader.GetString(reader.GetOrdinal("canonicalFR"));
                        if (!(reader["canonicalES"]).Equals(DBNull.Value))
                            item.CanonicalES = reader.GetString(reader.GetOrdinal("canonicalES"));
                        if (!(reader["canonicalDE"]).Equals(DBNull.Value))
                            item.CanonicalDE = reader.GetString(reader.GetOrdinal("canonicalDE"));
                        if (!(reader["robots"]).Equals(DBNull.Value))
                            item.Robots = reader.GetString(reader.GetOrdinal("robots"));

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
                item.DescrizioneI = Value.Substring(i, j);
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
                len = item.DescrizioneI.Length;
                item.DescrizioneI.Replace(":S:", "SSS");//Elimina eventuali presenze
                //del carattere di separazione dalla descrizione
                list.Schema += "Des" + n + ":S:" + pos + ":" + len + ":";
                list.Valori += item.DescrizioneI;
                pos += len;
            }

            return list;
        }

        public bool insertFoto(string connection, long idContenuto, string nomefile, string descrizione)
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
                tmp.DescrizioneI = descrizione;
                FotoColl.Add(tmp);
                //RIFORMIAMO LE STRINGHE schema e valori
                //PER IL SALVATAGGIO NEL DB
                FotoColl = this.CreaStringheAllegati(FotoColl);
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@fotoschema", FotoColl.Schema);
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@fotovalori", FotoColl.Valori);
                parColl.Add(p2);
                //SQLiteParameter p3 = new SQLiteParameter("@datainserimento", System.DateTime.Now.ToString());//OleDbType.VarChar
                //parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@id", idContenuto);//OleDbType.VarChar
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


        public bool CancellaFoto(string connection, long idContenuto, string nomefile, string descrizione, string pathfile)
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
                    List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                    SQLiteParameter p1 = new SQLiteParameter("@fotoschema", FotoColl.Schema);
                    parColl.Add(p1);
                    SQLiteParameter p2 = new SQLiteParameter("@fotovalori", FotoColl.Valori);
                    parColl.Add(p2);
                    //SQLiteParameter p3 = new SQLiteParameter("@datainserimento", System.DateTime.Now.ToString());//OleDbType.VarChar
                    //parColl.Add(p3);
                    SQLiteParameter p4 = new SQLiteParameter("@id", idContenuto);//OleDbType.VarChar
                    parColl.Add(p4);
                    string query = "UPDATE [TBL_CONTENUTI] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori  WHERE ([Id]=@id)";
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);

                    //ESEGUIAMO LA CANCELLAZIONE FISICA
                    //DEI FILE IMMAGINE E ANTEPRIMA DAL SERVER
                    if (System.IO.File.Exists(pathfile + "\\" + nomefile))
                    {
                        string filenamenoext = System.IO.Path.GetFileNameWithoutExtension(pathfile + "\\" + nomefile).ToString();
                        string fileext = System.IO.Path.GetExtension(pathfile + "\\" + nomefile).ToLower();
                        string filename_xs = pathfile + "\\" + filenamenoext + "-xs" + fileext;
                        string filename_sm = pathfile + "\\" + filenamenoext + "-sm" + fileext;
                        string filename_md = pathfile + "\\" + filenamenoext + "-md" + fileext;
                        string filename_lg = pathfile + "\\" + filenamenoext + "-lg" + fileext;
                        if (System.IO.File.Exists(filename_xs)) System.IO.File.Delete(filename_xs);
                        if (System.IO.File.Exists(filename_sm)) System.IO.File.Delete(filename_sm);
                        if (System.IO.File.Exists(filename_md)) System.IO.File.Delete(filename_md);
                        if (System.IO.File.Exists(filename_lg)) System.IO.File.Delete(filename_lg);

                    }
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
        public bool modificaFoto(string connection, long idContenuto, string nomefile, string descrizione)
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
                F1.DescrizioneI = descrizione;

                //RIFORMIAMO LE STRINGHE schema e valori
                //PER IL SALVATAGGIO NEL DB
                FotoColl = this.CreaStringheAllegati(FotoColl);

                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@fotoschema", FotoColl.Schema);
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@fotovalori", FotoColl.Valori);
                parColl.Add(p2);
                //SQLiteParameter p3 = new SQLiteParameter("@datainserimento", System.DateTime.Now.ToString());//OleDbType.VarChar
                //parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@id", idContenuto);//OleDbType.VarChar
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
        public AllegatiCollection getListaFotobyId(string connection, long idContenuto)
        {
            if (connection == null || connection == "") { return null; };
            if (idContenuto == 0) { return null; };

            string query = "SELECT [FotoSchema],[FotoValori] FROM TBL_CONTENUTI where ID=@idContenuto";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@idContenuto", idContenuto);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@codicecontenuto", item.CodiceContenuto);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@titoloi", item.TitoloI);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@titologb", item.TitoloGB);
            parColl.Add(p3);
            SQLiteParameter p3r = new SQLiteParameter("@titoloru", item.TitoloRU);
            parColl.Add(p3r);
            SQLiteParameter p3d = new SQLiteParameter("@titoloFR", item.TitoloFR);
            parColl.Add(p3d);
            SQLiteParameter p3e = new SQLiteParameter("@titoloDE", item.TitoloDE);
            parColl.Add(p3e);
            SQLiteParameter p3f = new SQLiteParameter("@titoloES", item.TitoloES);
            parColl.Add(p3f);
            SQLiteParameter p4 = new SQLiteParameter("@descrizionei", item.DescrizioneI);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@descrizionegb", item.DescrizioneGB);
            parColl.Add(p5);
            SQLiteParameter p5r = new SQLiteParameter("@descrizioneru", item.DescrizioneRU);
            parColl.Add(p5r);
            SQLiteParameter p5d = new SQLiteParameter("@descrizioneFR", item.DescrizioneFR);
            parColl.Add(p5d);

            SQLiteParameter p5e = new SQLiteParameter("@descrizioneDE", item.DescrizioneDE);
            parColl.Add(p5e);

            SQLiteParameter p5f = new SQLiteParameter("@descrizioneES", item.DescrizioneES);
            parColl.Add(p5f);


            SQLiteParameter p6 = new SQLiteParameter("@fotoschema", "");//item.FotoCollection_M.Schema
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@fotovalori", "");//item.FotoCollection_M.Valori
            parColl.Add(p7);

            SQLiteParameter p8 = new SQLiteParameter("@data", dbDataAccess.CorrectDatenow(item.DataInserimento));
            //p8.OleDbType = OleDbType.Date;
            parColl.Add(p8);

            SQLiteParameter p9 = new SQLiteParameter("@id_attivita", item.Id_attivita);
            parColl.Add(p9);

            SQLiteParameter pct1 = new SQLiteParameter("@customtitleI", item.CustomtitleI);
            parColl.Add(pct1);
            SQLiteParameter pct2 = new SQLiteParameter("@customtitleGB", item.CustomtitleGB);
            parColl.Add(pct2);
            SQLiteParameter pct3 = new SQLiteParameter("@customtitleRU", item.CustomtitleRU);
            parColl.Add(pct3);
            SQLiteParameter pct4 = new SQLiteParameter("@customtitleFR", item.CustomtitleFR);
            parColl.Add(pct4);
            SQLiteParameter pct5 = new SQLiteParameter("@customtitleDE", item.CustomtitleDE);
            parColl.Add(pct5);
            SQLiteParameter pct6 = new SQLiteParameter("@customtitleES", item.CustomtitleES);
            parColl.Add(pct6);


            SQLiteParameter pcd1 = new SQLiteParameter("@customdescI", item.CustomdescI);
            parColl.Add(pcd1);
            SQLiteParameter pcd2 = new SQLiteParameter("@customdescGB", item.CustomdescGB);
            parColl.Add(pcd2);
            SQLiteParameter pcd3 = new SQLiteParameter("@customdescRU", item.CustomdescRU);
            parColl.Add(pcd3);
            SQLiteParameter pcd4 = new SQLiteParameter("@customdescFR", item.CustomdescFR);
            parColl.Add(pcd4);
            SQLiteParameter pcd5 = new SQLiteParameter("@customdescDE", item.CustomdescDE);
            parColl.Add(pcd5);
            SQLiteParameter pcd6 = new SQLiteParameter("@customdescES", item.CustomdescES);
            parColl.Add(pcd6);

            SQLiteParameter pca1 = new SQLiteParameter("@canonicalI", item.CanonicalI);
            parColl.Add(pca1);
            SQLiteParameter pca2 = new SQLiteParameter("@canonicalGB", item.CanonicalGB);
            parColl.Add(pca2);
            SQLiteParameter pca3 = new SQLiteParameter("@canonicalRU", item.CanonicalRU);
            parColl.Add(pca3);
            SQLiteParameter pca4 = new SQLiteParameter("@canonicalFR", item.CanonicalFR);
            parColl.Add(pca4);
            SQLiteParameter pca5 = new SQLiteParameter("@canonicalDE", item.CanonicalDE);
            parColl.Add(pca5);
            SQLiteParameter pca6 = new SQLiteParameter("@canonicalES", item.CanonicalES);
            parColl.Add(pca6);
            SQLiteParameter pr1 = new SQLiteParameter("@robots", item.Robots);
            parColl.Add(pr1);

            string query = "INSERT INTO TBL_CONTENUTI([CodiceContenuto],[TitoloI],[TitoloGB],[TitoloRU],[TitoloFR],[TitoloDE],[TitoloES],[DescrizioneI],[DescrizioneGB],[DescrizioneRU],[DescrizioneFR],[DescrizioneDE],[DescrizioneES],[FotoSchema],[FotoValori],[DataInserimento],[Id_attivita],customtitleI,customtitleGB,customtitleRU,customtitleFR,customtitleDE,customtitleES,customdescI,customdescGB,customdescRU,customdescFR,customdescDE,customdescES,canonicalI,canonicalGB,canonicalRU,canonicalFR,canonicalDE,canonicalES,robots) VALUES (@codicecontenuto,@titoloi,@titologb,@titoloru,@titoloFR,@titoloDE,@titoloES,@descrizionei,@descrizionegb,@descrizioneru,@descrizioneFR,@descrizioneDE,@descrizioneES,@fotoschema,@fotovalori,@data,@Id_attivita,@customtitleI,@customtitleGB,@customtitleRU,@customtitleFR,@customtitleDE,@customtitleES,@customdescI,@customdescGB,@customdescRU,@customdescFR,@customdescDE,@customdescES,@canonicalI,@canonicalGB,@canonicalRU,@canonicalFR,@canonicalDE,@canonicalES,@robots)";
            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p2 = new SQLiteParameter("@titoloi", item.TitoloI);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@titologb", item.TitoloGB);
            parColl.Add(p3);
            SQLiteParameter p3r = new SQLiteParameter("@titoloru", item.TitoloRU);
            parColl.Add(p3r);
            SQLiteParameter p3d = new SQLiteParameter("@titoloFR", item.TitoloFR);
            parColl.Add(p3d);
            SQLiteParameter p3e = new SQLiteParameter("@titoloDE", item.TitoloDE);
            parColl.Add(p3e);
            SQLiteParameter p3f = new SQLiteParameter("@titoloES", item.TitoloES);
            parColl.Add(p3f);
            SQLiteParameter p4 = new SQLiteParameter("@descrizionei", item.DescrizioneI);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@descrizionegb", item.DescrizioneGB);
            parColl.Add(p5);
            SQLiteParameter p5r = new SQLiteParameter("@descrizioneru", item.DescrizioneRU);
            parColl.Add(p5r);
            SQLiteParameter p5d = new SQLiteParameter("@descrizioneFR", item.DescrizioneFR);
            parColl.Add(p5d);
            SQLiteParameter p5e = new SQLiteParameter("@descrizioneDE", item.DescrizioneDE);
            parColl.Add(p5e);
            SQLiteParameter p5f = new SQLiteParameter("@descrizioneES", item.DescrizioneES);
            parColl.Add(p5f);
            //SQLiteParameter p6 = new SQLiteParameter("@fotoschema", item.FotoCollection_M.Schema);
            //parColl.Add(p6);
            //SQLiteParameter p7 = new SQLiteParameter("@fotovalori", item.FotoCollection_M.Valori);
            //parColl.Add(p7);

            SQLiteParameter p8 = new SQLiteParameter("@data", dbDataAccess.CorrectDatenow(item.DataInserimento));
            //p8.OleDbType = OleDbType.Date;
            parColl.Add(p8);

            SQLiteParameter p9 = new SQLiteParameter("@Id_attivita", item.Id_attivita);
            parColl.Add(p9);

            SQLiteParameter pct1 = new SQLiteParameter("@customtitleI", item.CustomtitleI);
            parColl.Add(pct1);
            SQLiteParameter pct2 = new SQLiteParameter("@customtitleGB", item.CustomtitleGB);
            parColl.Add(pct2);
            SQLiteParameter pct3 = new SQLiteParameter("@customtitleRU", item.CustomtitleRU);
            parColl.Add(pct3);
            SQLiteParameter pct4 = new SQLiteParameter("@customtitleFR", item.CustomtitleFR);
            parColl.Add(pct4);
            SQLiteParameter pct5 = new SQLiteParameter("@customtitleDE", item.CustomtitleDE);
            parColl.Add(pct5);
            SQLiteParameter pct6 = new SQLiteParameter("@customtitleES", item.CustomtitleES);
            parColl.Add(pct6);

            SQLiteParameter pcd1 = new SQLiteParameter("@customdescI", item.CustomdescI);
            parColl.Add(pcd1);
            SQLiteParameter pcd2 = new SQLiteParameter("@customdescGB", item.CustomdescGB);
            parColl.Add(pcd2);
            SQLiteParameter pcd3 = new SQLiteParameter("@customdescRU", item.CustomdescRU);
            parColl.Add(pcd3);
            SQLiteParameter pcd4 = new SQLiteParameter("@customdescFR", item.CustomdescFR);
            parColl.Add(pcd4);
            SQLiteParameter pcd5 = new SQLiteParameter("@customdescDE", item.CustomdescDE);
            parColl.Add(pcd5);
            SQLiteParameter pcd6 = new SQLiteParameter("@customdescES", item.CustomdescES);
            parColl.Add(pcd6);

            SQLiteParameter pca1 = new SQLiteParameter("@canonicalI", item.CanonicalI);
            parColl.Add(pca1);
            SQLiteParameter pca2 = new SQLiteParameter("@canonicalGB", item.CanonicalGB);
            parColl.Add(pca2);
            SQLiteParameter pca3 = new SQLiteParameter("@canonicalRU", item.CanonicalRU);
            parColl.Add(pca3);
            SQLiteParameter pca4 = new SQLiteParameter("@canonicalFR", item.CanonicalFR);
            parColl.Add(pca4);
            SQLiteParameter pca5 = new SQLiteParameter("@canonicalDE", item.CanonicalDE);
            parColl.Add(pca5);
            SQLiteParameter pca6 = new SQLiteParameter("@canonicalES", item.CanonicalES);
            parColl.Add(pca6);
            SQLiteParameter pr1 = new SQLiteParameter("@robots", item.Robots);
            parColl.Add(pr1);



            SQLiteParameter p1 = new SQLiteParameter("@id", item.Id);//OleDbType.VarChar
            parColl.Add(p1);


            string query = "UPDATE [TBL_CONTENUTI] SET [TitoloI]=@titoloi,[TitoloGB]=@titologb,[TitoloRU]=@titoloru,[TitoloFR]=@titoloFR,[TitoloDE]=@titoloDE,[TitoloES]=@titoloES,[DescrizioneI]=@descrizionei,[DescrizioneGB]=@descrizionegb,[DescrizioneRU]=@descrizioneru,[DescrizioneFR]=@descrizioneFR,[DescrizioneDE]=@descrizioneDE,[DescrizioneES]=@descrizioneES,DataInserimento=@data,Id_attivita=@Id_attivita,customtitleI=@customtitleI,customtitleGB=@customtitleGB,customtitleRU=@customtitleRU,customtitleFR=@customtitleFR,customtitleDE=@customtitleDE,customtitleES=@customtitleES,customdescI=@customdescI,customdescGB=@customdescGB,customdescRU=@customdescRU,customdescFR=@customdescFR,customdescDE=@customdescDE,customdescES=@customdescES,canonicalI=@canonicalI,canonicalGB=@canonicalGB,canonicalRU=@canonicalRU,canonicalFR=@canonicalFR,canonicalDE=@canonicalDE,canonicalES=@canonicalES,robots=@robots   WHERE ([Id]=@id)";
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
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (item == null || item.Id == 0) return;

            SQLiteParameter p1 = new SQLiteParameter("@id", item.Id);//OleDbType.VarChar
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
