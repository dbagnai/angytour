using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using WelcomeLibrary.DOM;

namespace WelcomeLibrary.DAL
{
    public class annunciDM
    {
        private string _tblarchivio = "TBL_Annunci";
        public string Tblarchivio
        {
            get { return _tblarchivio; }
            set { _tblarchivio = value; }
        }
        public annunciDM()
        { }
        public annunciDM(string nometabella)
        {
            Tblarchivio = nometabella;
        }

        /// <summary>
        /// Carica la lista completa ordinata per data di registrazione dell' Annuncio
        /// in base al codice tipologia indicato ( da tblrif_annunci )
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codicetipologia"></param>
        /// <returns></returns>
        public AnnunciCollection CaricaAnnunciPerCodice(string connection, string codicetipologia)
        {
            if (connection == null || connection == "") return null;
            if (codicetipologia == null || codicetipologia == "") return null;
            AnnunciCollection list = new AnnunciCollection();
            Annunci item;

            try
            {
                string query = "SELECT * FROM " + Tblarchivio + " where CodiceTIPOLOGIA=@CodiceTIPOLOGIA order BY DataInserimento Desc";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@CodiceTIPOLOGIA", codicetipologia);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Annunci();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSottoProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto"));

                        if (!reader["DATITECNICII"].Equals(DBNull.Value))
                            item.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                        if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                            item.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                        if (!reader["EMAIL"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        if (!reader["FAX"].Equals(DBNull.Value))
                            item.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                        if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                        if (!reader["TELEFONO"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                        if (!reader["WEBSITE"].Equals(DBNull.Value))
                            item.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));

                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));

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

                        if (!reader["Parametro1"].Equals(DBNull.Value))
                            item.Parametro1 = reader.GetString(reader.GetOrdinal("Parametro1"));
                        if (!reader["Parametro2"].Equals(DBNull.Value))
                            item.Parametro2 = reader.GetString(reader.GetOrdinal("Parametro2"));
                        if (!reader["Parametro3"].Equals(DBNull.Value))
                            item.Parametro3 = reader.GetString(reader.GetOrdinal("Parametro3"));
                        if (!reader["Parametro4"].Equals(DBNull.Value))
                            item.Parametro4 = reader.GetString(reader.GetOrdinal("Parametro4"));
                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetString(reader.GetOrdinal("Anno"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Annunci :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Carica la lista completa ordinata per data di registrazione dell' Annuncio
        /// in base al codice tipologia indicato ( da tblrif_annunci )
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codicetipologia"></param>
        /// <returns></returns>
        public AnnunciCollection CaricaAnnunciPerEmailCliente(string connection, string emailcliente)
        {
            if (connection == null || connection == "") return null;
            if (emailcliente == null || emailcliente == "") return null;
            AnnunciCollection list = new AnnunciCollection();
            Annunci item;

            try
            {
                string query = "SELECT * FROM " + Tblarchivio + " where Email=@Email ";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@Email", emailcliente);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Annunci();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSottoProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto"));

                        if (!reader["DATITECNICII"].Equals(DBNull.Value))
                            item.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                        if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                            item.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                        if (!reader["EMAIL"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        if (!reader["FAX"].Equals(DBNull.Value))
                            item.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                        if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                        if (!reader["TELEFONO"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                        if (!reader["WEBSITE"].Equals(DBNull.Value))
                            item.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));

                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));

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

                        if (!reader["Parametro1"].Equals(DBNull.Value))
                            item.Parametro1 = reader.GetString(reader.GetOrdinal("Parametro1"));
                        if (!reader["Parametro2"].Equals(DBNull.Value))
                            item.Parametro2 = reader.GetString(reader.GetOrdinal("Parametro2"));
                        if (!reader["Parametro3"].Equals(DBNull.Value))
                            item.Parametro3 = reader.GetString(reader.GetOrdinal("Parametro3"));
                        if (!reader["Parametro4"].Equals(DBNull.Value))
                            item.Parametro4 = reader.GetString(reader.GetOrdinal("Parametro4"));
                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetString(reader.GetOrdinal("Anno"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Annunci :" + error.Message, error);
            }

            return list;
        }



        /// <summary>
        /// Carica la lista delle Annunci in base ai parametri di filtro passati
        /// </summary>
        /// <param name="connection">nome della connessione al db</param>
        /// <param name="parColl">Codice Provincia,Nome Comune, Codicetipologia annuncio,Prezzo, Parametro1,Parametro2,Parametro3,Parametro4</param>
        /// <returns></returns>
        public AnnunciCollection CaricaAnnunciFiltrate(string connection, List<OleDbParameter> parColl)
        {
            AnnunciCollection list = new AnnunciCollection();
            if (connection == null || connection == "") return list;
            if (parColl == null || parColl.Count < 1) return list;
            Annunci item;
            try
            {
                string query = "";
                ////Vediamo se c'è la categoria nella parcoll -> faccio la join (usato per marchettini)
                //if (parColl.Exists(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@CodiceCATEGORIA"; }))
                //{
                //    query = "SELECT A.*,B.CodiceTIPOLOGIA as CodiceTIPOLOGIA,B.codcat1 as codcat1 FROM " + Tblarchivio + " A left join dbo_TBLRIF_Annunci_LINK_LIV1 B on A.CodiceTIPOLOGIA=B.CodiceTipologia where A.CodiceTIPOLOGIA like @CodiceTIPOLOGIA and B.codcat1 like @CodiceCATEGORIA";
                //}
                //else //come prima (new moon e welcomehome)
                query = "SELECT * FROM " + Tblarchivio + " where CodicePROVINCIA like @CodicePROVINCIA and CodiceCOMUNE like @CodiceCOMUNE and CodiceTIPOLOGIA like @CodiceTIPOLOGIA and CodiceREGIONE like  @CodiceREGIONE";
                query = " and  Parametro1 like  @Parametro1 and  Parametro2 like  @Parametro2 and  Parametro3 like  @Parametro3 and  Parametro4 like  @Parametro4 ";
                query += " and Prezzo >= @PrezzoMin and Prezzo <= @PrezzoMax order BY DataInserimento Desc";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Annunci();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));

                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSottoProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto"));

                        if (!reader["DATITECNICII"].Equals(DBNull.Value))
                            item.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                        if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                            item.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                        if (!reader["EMAIL"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        if (!reader["FAX"].Equals(DBNull.Value))
                            item.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                        if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                        if (!reader["TELEFONO"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                        if (!reader["WEBSITE"].Equals(DBNull.Value))
                            item.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));
                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));

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

                        if (!reader["Parametro1"].Equals(DBNull.Value))
                            item.Parametro1 = reader.GetString(reader.GetOrdinal("Parametro1"));
                        if (!reader["Parametro2"].Equals(DBNull.Value))
                            item.Parametro2 = reader.GetString(reader.GetOrdinal("Parametro2"));
                        if (!reader["Parametro3"].Equals(DBNull.Value))
                            item.Parametro3 = reader.GetString(reader.GetOrdinal("Parametro3"));
                        if (!reader["Parametro4"].Equals(DBNull.Value))
                            item.Parametro4 = reader.GetString(reader.GetOrdinal("Parametro4"));
                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetString(reader.GetOrdinal("Anno"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Annunci :" + error.Message, error);
            }

            return list;
        }
        public AnnunciCollection CaricaAnnunciFiltrate(string connection, List<OleDbParameter> parColl, string maxrecord)
        {
            AnnunciCollection list = new AnnunciCollection();
            if (connection == null || connection == "") return list;
            if (parColl == null || parColl.Count < 1) return list;

            Annunci item;

            try
            {

                string query = "SELECT TOP " + maxrecord + " * FROM " + Tblarchivio + " where CodicePROVINCIA like @CodicePROVINCIA and CodiceCOMUNE like @CodiceCOMUNE and CodiceTIPOLOGIA like @CodiceTIPOLOGIA and CodiceREGIONE like  @CodiceREGIONE";
                query = " and  Parametro1 like  @Parametro1 and  Parametro2 like  @Parametro2 and  Parametro3 like  @Parametro3 and  Parametro4 like  @Parametro4 ";
                query += " and Prezzo >= @PrezzoMin and Prezzo <= @PrezzoMax order BY DataInserimento Desc";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Annunci();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSottoProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto"));

                        if (!reader["DATITECNICII"].Equals(DBNull.Value))
                            item.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                        if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                            item.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                        if (!reader["EMAIL"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        if (!reader["FAX"].Equals(DBNull.Value))
                            item.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                        if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                        if (!reader["TELEFONO"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                        if (!reader["WEBSITE"].Equals(DBNull.Value))
                            item.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));
                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));

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

                        if (!reader["Parametro1"].Equals(DBNull.Value))
                            item.Parametro1 = reader.GetString(reader.GetOrdinal("Parametro1"));
                        if (!reader["Parametro2"].Equals(DBNull.Value))
                            item.Parametro2 = reader.GetString(reader.GetOrdinal("Parametro2"));
                        if (!reader["Parametro3"].Equals(DBNull.Value))
                            item.Parametro3 = reader.GetString(reader.GetOrdinal("Parametro3"));
                        if (!reader["Parametro4"].Equals(DBNull.Value))
                            item.Parametro4 = reader.GetString(reader.GetOrdinal("Parametro4"));
                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetString(reader.GetOrdinal("Anno"));
                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Annunci :" + error.Message, error);
            }

            return list;
        }
        /// <summary>
        /// Carica la lista delle Annunci in base ai parametri di filtro passati escludendo il filtro di prezzo 
        /// per i record relativi al codicetipologiabypass
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="parColl">@CodicePROVINCIA,@CodiceCOMUNE,@CodiceTIPOLOGIA,@CodiceREGIONE,@Parametro1,@Parametro2,@Parametro3,@Parametro4,@PrezzoMin,@PrezzoMax  </param>
        /// <param name="codicetipologiabypass">CodiceTipologia per cui viene escluso il fitro di prezzo</param>
        /// <returns></returns>
        public AnnunciCollection CaricaAnnunciFiltrateBypass(string connection, List<OleDbParameter> parColl, string codicetipologiabypass)
        {
            AnnunciCollection list = new AnnunciCollection();
            if (connection == null || connection == "") return list;
            if (parColl == null || parColl.Count < 2) return list;
            Annunci item;
            try
            {
                string query = "SELECT * FROM " + Tblarchivio + " where CodicePROVINCIA like @CodicePROVINCIA and CodiceCOMUNE like @CodiceCOMUNE and CodiceTIPOLOGIA like @CodiceTIPOLOGIA and CodiceREGIONE like  @CodiceREGIONE";
                query += " and  Parametro1 like  @Parametro1 and  Parametro2 like  @Parametro2 and  Parametro3 like  @Parametro3 and  Parametro4 like  @Parametro4 ";
                query += " and ( Prezzo >= @PrezzoMin or CodiceTIPOLOGIA like '%" + codicetipologiabypass + "%' )  and (Prezzo <= @PrezzoMax or CodiceTIPOLOGIA like '%" + codicetipologiabypass + "%' ) order BY DataInserimento Desc";
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;
                    while (reader.Read())
                    {
                        item = new Annunci();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));

                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSottoProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto"));

                        if (!reader["DATITECNICII"].Equals(DBNull.Value))
                            item.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                        if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                            item.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                        if (!reader["EMAIL"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        if (!reader["FAX"].Equals(DBNull.Value))
                            item.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                        if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                        if (!reader["TELEFONO"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                        if (!reader["WEBSITE"].Equals(DBNull.Value))
                            item.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));
                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));

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

                        if (!reader["Parametro1"].Equals(DBNull.Value))
                            item.Parametro1 = reader.GetString(reader.GetOrdinal("Parametro1"));
                        if (!reader["Parametro2"].Equals(DBNull.Value))
                            item.Parametro2 = reader.GetString(reader.GetOrdinal("Parametro2"));
                        if (!reader["Parametro3"].Equals(DBNull.Value))
                            item.Parametro3 = reader.GetString(reader.GetOrdinal("Parametro3"));
                        if (!reader["Parametro4"].Equals(DBNull.Value))
                            item.Parametro4 = reader.GetString(reader.GetOrdinal("Parametro4"));
                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetString(reader.GetOrdinal("Anno"));
                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Annunci :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Ricarica un'Annunci specifica in base all'id
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idAnnunci"></param>
        /// <returns></returns>
        public Annunci CaricaAnnunciPerId(string connection, string idAnnunci)
        {
            if (connection == null || connection == "") return null;
            if (idAnnunci == null || idAnnunci == "") return null;
            AnnunciCollection list = new AnnunciCollection();
            Annunci item = null;

            try
            {
                string query = "SELECT * FROM " + Tblarchivio + " where ID=@ID order BY DataInserimento Desc";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@ID", idAnnunci);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Annunci();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSottoProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto"));

                        if (!reader["DATITECNICII"].Equals(DBNull.Value))
                            item.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                        if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                            item.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                        if (!reader["EMAIL"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        if (!reader["FAX"].Equals(DBNull.Value))
                            item.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                        if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                        if (!reader["TELEFONO"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                        if (!reader["WEBSITE"].Equals(DBNull.Value))
                            item.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));
                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));

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
                        if (!reader["Parametro1"].Equals(DBNull.Value))
                            item.Parametro1 = reader.GetString(reader.GetOrdinal("Parametro1"));
                        if (!reader["Parametro2"].Equals(DBNull.Value))
                            item.Parametro2 = reader.GetString(reader.GetOrdinal("Parametro2"));
                        if (!reader["Parametro3"].Equals(DBNull.Value))
                            item.Parametro3 = reader.GetString(reader.GetOrdinal("Parametro3"));
                        if (!reader["Parametro4"].Equals(DBNull.Value))
                            item.Parametro4 = reader.GetString(reader.GetOrdinal("Parametro4"));
                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetString(reader.GetOrdinal("Anno"));
                        return (item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Annunci :" + error.Message, error);
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
                //Inserisco il percorso per la foto di anteprima
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

        /// <summary>
        /// Aggiunge la foto all'annuncio passato senza memorizzarla nel db ma solo all'oggetto
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="nomefile"></param>
        /// <param name="descrizione"></param>
        /// <returns></returns>
        public Annunci AggiungiFotoACollection(Annunci _a, string nomefile, string descrizione)
        {
            //ALCUNI CONTROLLI SULL'ESISTENZA DELLA FOTO DA INSERIRE
            AllegatiCollection FotoColl = _a.FotoCollection_M;
            Allegato F1 = (FotoColl).FindLast(delegate(Allegato agtemp) { return agtemp.NomeFile == nomefile; });
            if (F1 != null) //FOTO TROVATA GIA' ESISTENTE nel db
            {
                return _a;
            }
            //AGGIUNGIAMO LA FOTO ALLA COLLECTION
            Allegato tmp = new Allegato();
            tmp.NomeFile = nomefile;
            tmp.Descrizione = descrizione;
            FotoColl.Add(tmp);
            //RIFORMIAMO LE STRINGHE schema e valori
            //PER IL SALVATAGGIO NEL DB
            FotoColl = this.CreaStringheAllegati(FotoColl);
            return _a;
        }

        /// <summary>
        /// Elimina la foto all'annuncio passato senza memorizzare nel db ma solo sull'oggetto
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="nomefile"></param>
        /// <param name="descrizione"></param>
        /// <returns></returns>
        public Annunci EliminaFotoDaCollection(Annunci _a, string nomefile, string descrizione)
        {
            //ALCUNI CONTROLLI SULL'ESISTENZA DELLA FOTO DA INSERIRE
            AllegatiCollection FotoColl = _a.FotoCollection_M;
            Allegato F1 = (FotoColl).FindLast(delegate(Allegato agtemp) { return agtemp.NomeFile == nomefile; });
            if (F1 == null) //FOTO TROVATA GIA' ESISTENTE nel db
            {
                return _a;
            }
            FotoColl.Remove(F1);

            //RIFORMIAMO LE STRINGHE schema e valori
            //PER IL SALVATAGGIO NEL DB
            FotoColl = this.CreaStringheAllegati(FotoColl);
            return _a;
        }

        public bool insertFoto(string connection, int idAnnunci, string nomefile, string descrizione)
        {
            if (connection == "") return false;
            if (idAnnunci == 0) return false;
            //Carico le foto preesistenti nel db
            AllegatiCollection FotoColl = this.getListaFotobyId(connection, idAnnunci);
            if (FotoColl != null)
            {
                //ALCUNI CONTROLLI SULL'ESISTENZA DELLA FOTO DA INSERIRE
                Allegato F1 = (FotoColl).FindLast(delegate(Allegato agtemp) { return agtemp.NomeFile == nomefile; });
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
                OleDbParameter p3 = new OleDbParameter("@datainserimento", System.DateTime.Now.ToString());//OleDbType.VarChar
                parColl.Add(p3);
                OleDbParameter p4 = new OleDbParameter("@id", idAnnunci);//OleDbType.VarChar
                parColl.Add(p4);
                string query = "UPDATE [" + Tblarchivio + "] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori,DataInserimento=@datainserimento  WHERE ([Id]=@id)";
                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento Foto Annunci :" + error.Message, error);
                }

            }
            return true;
        }

        public bool CancellaFoto(string connection, int idAnnunci, string nomefile, string descrizione, string pathfile)
        {

            if (connection == "") return false;
            if (idAnnunci == 0) return false;
            //Carico le foto preesistenti nel db
            AllegatiCollection FotoColl = this.getListaFotobyId(connection, idAnnunci);
            if (FotoColl != null)
            {

                try
                {
                    //CONTROLLO SULL'ESISTENZA DELLA FOTO DA CANCELLARE
                    Allegato F1 = (FotoColl).FindLast(delegate(Allegato agtemp) { return agtemp.NomeFile == nomefile; });
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
                    OleDbParameter p3 = new OleDbParameter("@datainserimento", System.DateTime.Now.ToString());//OleDbType.VarChar
                    parColl.Add(p3);
                    OleDbParameter p4 = new OleDbParameter("@id", idAnnunci);//OleDbType.VarChar
                    parColl.Add(p4);
                    string query = "UPDATE [" + Tblarchivio + "] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori,DataInserimento=@datainserimento  WHERE ([Id]=@id)";
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


        /// <summary>
        /// Carica la collection delle foto a partire dall'id del record dell'Annunci passata
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idContenuto"></param>
        /// <returns></returns>
        public AllegatiCollection getListaFotobyId(string connection, int idAnnunci)
        {
            if (connection == null || connection == "") { return null; };
            if (idAnnunci == null || idAnnunci == 0) { return null; };

            string query = "SELECT [FotoSchema],[FotoValori] FROM " + Tblarchivio + " where ID=@idAnnunci";
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@idAnnunci", idAnnunci);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
            Annunci item = new Annunci();
            using (reader)
            {
                if (reader == null) { return null; };
                if (reader.HasRows == false)
                    return null;
                while (reader.Read())
                {
                    item = new Annunci();
                    item.Id = idAnnunci;
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

        /// <summary>
        /// Inserisce un record in tabella Annunci
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertAnnunci(string connessione,
        Annunci item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            OleDbParameter p1 = new OleDbParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@DENOMINAZIONEI", item.DenominazioneI);
            parColl.Add(p2);
            OleDbParameter p3 = new OleDbParameter("@DENOMINAZIONEGB", item.DenominazioneGB);
            parColl.Add(p3);
            OleDbParameter p4 = new OleDbParameter("@DescrizioneI", item.DescrizioneI);
            parColl.Add(p4);
            OleDbParameter p5 = new OleDbParameter("@DescrizioneGB", item.DescrizioneGB);
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@FotoSchema", "");
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@FotoValori", "");
            parColl.Add(p7);
            OleDbParameter p8 = new OleDbParameter("@CodiceCOMUNE", item.CodiceComune);
            parColl.Add(p8);
            OleDbParameter p9 = new OleDbParameter("@CodicePROVINCIA", item.CodiceProvincia);
            parColl.Add(p9);
            OleDbParameter p10 = new OleDbParameter("@CodiceREGIONE", item.CodiceRegione);
            parColl.Add(p10);
            OleDbParameter p11 = new OleDbParameter("@DATITECNICII", item.DatitecniciI);
            parColl.Add(p11);
            OleDbParameter p12 = new OleDbParameter("@DATITECNICIGB", item.DatitecniciGB);
            parColl.Add(p12);
            OleDbParameter p13 = new OleDbParameter("@EMAIL", item.Email);
            parColl.Add(p13);
            OleDbParameter p14 = new OleDbParameter("@FAX", item.Fax);
            parColl.Add(p14);
            OleDbParameter p15 = new OleDbParameter("@INDIRIZZO", item.Indirizzo);
            parColl.Add(p15);
            OleDbParameter p16 = new OleDbParameter("@TELEFONO", item.Telefono);
            parColl.Add(p16);
            OleDbParameter p17 = new OleDbParameter("@WEBSITE", item.Website);
            parColl.Add(p17);
            OleDbParameter p18 = new OleDbParameter("@data", System.DateTime.Now.ToString());
            parColl.Add(p18);
            OleDbParameter p19 = new OleDbParameter("@CodiceProdotto", item.CodiceProdotto);
            parColl.Add(p19);
            OleDbParameter p20 = new OleDbParameter("@CodiceSottoprodotto", item.CodiceSottoProdotto);
            parColl.Add(p20);
            OleDbParameter p21 = new OleDbParameter("@Prezzo", item.Prezzo);
            parColl.Add(p21);

            OleDbParameter p22 = new OleDbParameter("@Parametro1", item.Parametro1);
            parColl.Add(p22);
            OleDbParameter p23 = new OleDbParameter("@Parametro2", item.Parametro2);
            parColl.Add(p23);
            OleDbParameter p24 = new OleDbParameter("@Parametro3", item.Parametro3);
            parColl.Add(p24);
            OleDbParameter p25 = new OleDbParameter("@Parametro4", item.Parametro4);
            parColl.Add(p25);
            OleDbParameter p26 = new OleDbParameter("@Anno", item.Anno);
            parColl.Add(p26);

            string query = "INSERT INTO " + Tblarchivio + " ([CodiceTIPOLOGIA],[DENOMINAZIONEI],[DENOMINAZIONEGB],[DescrizioneI],[DescrizioneGB],[FotoSchema],[FotoValori],[CodiceCOMUNE],[CodicePROVINCIA],[CodiceREGIONE],[DATITECNICII],[DATITECNICIGB],[EMAIL],[FAX],[INDIRIZZO],[TELEFONO],[WEBSITE],[DataInserimento],[CodiceProdotto],[CodiceSottoprodotto],[Prezzo],[Parametro1],[Parametro2],[Parametro3],[Parametro4],[Anno] ) VALUES (@CodiceTIPOLOGIA,@DENOMINAZIONEI,@DENOMINAZIONEGB,@DescrizioneI,@DescrizioneGB,@FotoSchema,@FotoValori,@CodiceCOMUNE,@CodicePROVINCIA,@CodiceREGIONE,@DATITECNICII,@DATITECNICIGB,@EMAIL,@FAX,@INDIRIZZO,@TELEFONO,@WEBSITE,@Data,@CodiceProdotto,@CodiceSottoprodotto,@Prezzo,@Parametro1,@Parametro2,@Parametro3,@Parametro4,@Anno )";
            try
            {
                int lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                item.Id = lastidentity; //Inserisco nell'id dell'elemento inseito l'id generato dal db
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento Annunci :" + error.Message, error);
            }
            return;
        }


        /// <summary>
        /// Aggiorna un record in tabella Annunci
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateAnnunci(string connessione,
            Annunci item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            //OleDbParameter p1 = new OleDbParameter("@CodiceTIPOLOGIA", item.CodiceAnnunci);//OleDbType.VarChar
            //parColl.Add(p1);
            OleDbParameter p1 = new OleDbParameter("@DENOMINAZIONEI", item.DenominazioneI);
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@DENOMINAZIONEGB", item.DenominazioneGB);
            parColl.Add(p2);
            OleDbParameter p3 = new OleDbParameter("@DescrizioneI", item.DescrizioneI);
            parColl.Add(p3);
            OleDbParameter p4 = new OleDbParameter("@DescrizioneGB", item.DescrizioneGB);
            parColl.Add(p4);
            //OleDbParameter p5 = new OleDbParameter("@FotoSchema", "");
            //parColl.Add(p5);
            //OleDbParameter p6 = new OleDbParameter("@FotoValori", "");
            //parColl.Add(p6);
            OleDbParameter p5 = new OleDbParameter("@CodiceCOMUNE", item.CodiceComune);
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@CodicePROVINCIA", item.CodiceProvincia);
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@CodiceREGIONE", item.CodiceRegione);
            parColl.Add(p7);
            OleDbParameter p8 = new OleDbParameter("@DATITECNICII", item.DatitecniciI);
            parColl.Add(p8);
            OleDbParameter p9 = new OleDbParameter("@DATITECNICIGB", item.DatitecniciGB);
            parColl.Add(p9);
            OleDbParameter p10 = new OleDbParameter("@EMAIL", item.Email);
            parColl.Add(p10);
            OleDbParameter p11 = new OleDbParameter("@FAX", item.Fax);
            parColl.Add(p11);
            OleDbParameter p12 = new OleDbParameter("@INDIRIZZO", item.Indirizzo);
            parColl.Add(p12);
            OleDbParameter p13 = new OleDbParameter("@TELEFONO", item.Telefono);
            parColl.Add(p13);
            OleDbParameter p14 = new OleDbParameter("@WEBSITE", item.Website);
            parColl.Add(p14);
            OleDbParameter p15 = new OleDbParameter("@data", System.DateTime.Now.ToString());
            parColl.Add(p15);
            OleDbParameter p17 = new OleDbParameter("@CodiceProdotto", item.CodiceProdotto);
            parColl.Add(p17);
            OleDbParameter p18 = new OleDbParameter("@CodiceSottoprodotto", item.CodiceSottoProdotto);
            parColl.Add(p18);
            OleDbParameter p19 = new OleDbParameter("@Prezzo", item.Prezzo);
            parColl.Add(p19);

            OleDbParameter p20 = new OleDbParameter("@Parametro1", item.Parametro1);
            parColl.Add(p20);
            OleDbParameter p21 = new OleDbParameter("@Parametro2", item.Parametro2);
            parColl.Add(p21);
            OleDbParameter p22 = new OleDbParameter("@Parametro3", item.Parametro3);
            parColl.Add(p22);
            OleDbParameter p23 = new OleDbParameter("@Parametro4", item.Parametro4);
            parColl.Add(p23);
            OleDbParameter p24 = new OleDbParameter("@Anno", item.Anno);
            parColl.Add(p24);




            OleDbParameter p16 = new OleDbParameter("@Id", item.Id);
            parColl.Add(p16);
            string query = "UPDATE " + Tblarchivio + " SET [DENOMINAZIONEI]=@DENOMINAZIONEI , [DENOMINAZIONEGB]= @DENOMINAZIONEGB , [DescrizioneI]=@DescrizioneI , [DescrizioneGB]= @DescrizioneGB , [CodiceCOMUNE]=@CodiceCOMUNE ,[CodicePROVINCIA]=@CodicePROVINCIA , [CodiceREGIONE]= @CodiceREGIONE , [DATITECNICII]=@DATITECNICII , [DATITECNICIGB]= @DATITECNICIGB , [EMAIL]=@EMAIL , [FAX]=@FAX , [INDIRIZZO]= @INDIRIZZO , [TELEFONO]=@TELEFONO , [WEBSITE]=@WEBSITE , [Datainserimento]= @data , [CodiceProdotto]=@CodiceProdotto , [CodiceSottoprodotto]= @CodiceSottoprodotto , [Prezzo]= @Prezzo , [Parametro1]= @Parametro1 , [Parametro2]= @Parametro2 , [Parametro3]= @Parametro3 , [Parametro4]= @Parametro4 , [Anno]= @Anno WHERE [Id]=@Id ";


            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornamento Annunci :" + error.Message, error);
            }
            return;
        }

        public void DeleteAnnunci(string connessione,
                Annunci item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (item == null || item.Id == 0) return;

            OleDbParameter p1 = new OleDbParameter("@id", item.Id);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "DELETE FROM " + Tblarchivio + " WHERE ([ID]=@id)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione Annunci :" + error.Message, error);
            }
            return;
        }


        public void DeleteAnnunciByEmailCliente(string connessione,
              string email)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (email == null || string.IsNullOrEmpty(email)) return;

            OleDbParameter p1 = new OleDbParameter("@email", email);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "DELETE FROM " + Tblarchivio + " WHERE ([Email]=@email)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione Annunci relativi a cliente:" + error.Message, error);
            }
            return;
        }


        /// <summary>
        /// Inserisce un record in tabella PRODOTTO
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertProdotto(string connessione, Prodotto item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            if (item.Lingua == "I")
            {
                OleDbParameter p1 = new OleDbParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbParameter p3 = new OleDbParameter("@Lingua", item.Lingua);
                parColl.Add(p3);
                OleDbParameter p4 = new OleDbParameter("@Descrizione", item.Descrizione);
                parColl.Add(p4);
                OleDbParameter p2 = new OleDbParameter("@CodiceProdotto", CreareCodiceAggiornatoProdotto(connessione));
                parColl.Add(p2);

            }
            else
            {
                //se il prodotto non è in italiano, siamo al secodno inserimento quindi nell'altra lingua e gli devo assegnare lo stesso codice prodotto
                Prodotto tmp_item = CaricaUltimoProdotto(connessione);
                OleDbParameter p1 = new OleDbParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbParameter p3 = new OleDbParameter("@Lingua", item.Lingua);
                parColl.Add(p3);
                OleDbParameter p4 = new OleDbParameter("@Descrizione", item.Descrizione);
                parColl.Add(p4);
                OleDbParameter p2 = new OleDbParameter("@CodiceProdotto", tmp_item.CodiceProdotto);
                parColl.Add(p2);
            }

            string query = "INSERT INTO dbo_TBLRIF_PRODOTTO ([CodiceTipologia],[Lingua],[Descrizione],[CodiceProdotto]) VALUES (@CodiceTIPOLOGIA,@Lingua,@Descrizione,@CodiceProdotto)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento Prodotto :" + error.Message, error);
            }

            return;
        }

        /// <summary>
        /// Inserisce un record in tabella SOTTOPRODOTTO
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertSottoProdotto(string connessione, SProdotto item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            if (item.Lingua == "I")
            {
                OleDbParameter p1 = new OleDbParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbParameter p3 = new OleDbParameter("@Lingua", item.Lingua);
                parColl.Add(p3);
                OleDbParameter p4 = new OleDbParameter("@Descrizione", item.Descrizione);
                parColl.Add(p4);
                OleDbParameter p2 = new OleDbParameter("@CodiceSottoprodotto", CreareCodiceAggiornatoSottoprodotto(connessione));
                parColl.Add(p2);

            }
            else
            {
                //se il prodotto non è in italiano, siamo al secodno inserimento quindi nell'altra lingua e gli devo assegnare lo stesso codice prodotto
                SProdotto tmp_item = CaricaUltimoSottoProdotto(connessione);
                OleDbParameter p1 = new OleDbParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbParameter p3 = new OleDbParameter("@Lingua", item.Lingua);
                parColl.Add(p3);
                OleDbParameter p4 = new OleDbParameter("@Descrizione", item.Descrizione);
                parColl.Add(p4);
                OleDbParameter p2 = new OleDbParameter("@CodiceSottoprodotto", tmp_item.CodiceSProdotto);
                parColl.Add(p2);
            }

            string query = "INSERT INTO dbo_TBLRIF_SOTTOPRODOTTO ([CodiceProdotto],[Lingua],[Descrizione],[CodiceSottoprodotto]) VALUES (@CodiceProdotto,@Lingua,@Descrizione,@CodiceSottoprodotto)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento SottoProdotto :" + error.Message, error);
            }

            return;
        }

        /// <summary>
        /// Funzione che prende l'ultimo codice presente nel database e ne crea uno aggiuntivo per i prodotti
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codicetipologia"></param>
        /// <returns></returns>
        public string CreareCodiceAggiornatoProdotto(string connection)
        {
            //Carico l'ultimo prodotto inserito
            Prodotto Prodotto = CaricaUltimoProdotto(connection);
            string codice = "";
            if (Prodotto != null)
            {
                codice = Prodotto.CodiceProdotto;
            }


            //Hol'ultimo e ne creo uno nuovo
            if (!string.IsNullOrEmpty(codice))
            {

                //Funzione che calcola il codice nuovo del sotto prodotto
                string tmp_cod = codice.Substring(4);
                int int_cod = 0;
                int.TryParse(tmp_cod, out int_cod);
                int_cod = int_cod + 1;

                codice = "prod" + string.Format("{0:000000}", int_cod);
            }
            else
            {
                codice = "prod000001";
            }

            return codice;
        }

        /// <summary>
        /// Funzione che prende l'ultimo codice presente nel database e ne crea uno aggiuntivo per i sottoprodotti
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codicetipologia"></param>
        /// <returns></returns>
        public string CreareCodiceAggiornatoSottoprodotto(string connection)
        {
            //Carico l'ultimo prodotto inserito
            SProdotto SottoProdotto = CaricaUltimoSottoProdotto(connection);
            string codice = "";
            if (SottoProdotto != null) { codice = SottoProdotto.CodiceSProdotto; }


            //Hol'ultimo e ne creo uno nuovo
            if (!string.IsNullOrEmpty(codice))
            {

                //Funzione che calcola il codice nuovo del sotto prodotto
                string tmp_cod = codice.Substring(5);
                int int_cod = 0;
                int.TryParse(tmp_cod, out int_cod);
                int_cod = int_cod + 1;

                codice = "sprod" + string.Format("{0:000000}", int_cod);
            }
            else
            {
                codice = "sprod000001";
            }

            return codice;
        }


        /// <summary>
        /// Carica la lista completa ordinata per data di registrazione delle Annunci
        /// in base al codice tipologia indicato
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codiceprodotto"></param>
        /// <returns></returns>
        public Prodotto CaricaUltimoProdotto(string connection)
        {
            if (connection == null || connection == "") return null;
            Prodotto item = new Prodotto();

            try
            {
                string query = "SELECT TOP 1 * FROM dbo_TBLRIF_PRODOTTO order BY CodiceProdotto Desc";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {

                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione"));
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Prodotti :" + error.Message, error);
            }

            return item;
        }

        /// <summary>
        /// Carica la lista completa ordinata 
        /// in base al codice riporatto
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codiceprodotto"></param>
        /// <returns></returns>
        public SProdotto CaricaUltimoSottoProdotto(string connection)
        {
            if (connection == null || connection == "") return null;
            SProdotto item = new SProdotto();

            try
            {
                string query = "SELECT TOP 1 * FROM dbo_TBLRIF_SOTTOPRODOTTO order BY CodiceSottoProdotto Desc";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoProdotto"));
                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione"));
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento SottoProdotti :" + error.Message, error);
            }

            return item;
        }

        /// <summary>
        /// Funzione che carica la lista dei prodotti scelto un codice tipologia
        /// </summary>
        /// <param name="connection">Connessione</param>
        /// <param name="CodTipologia">Codice prodotto richiesto</param>
        /// <returns></returns>
        public AnnunciCollection CaricaListaProdottiPerCodiceTipologia(string connection, string CodTipologia)
        {
            AnnunciCollection list = new AnnunciCollection();
            if (connection == null || connection == "") return list;
            if (CodTipologia == null || CodTipologia == "") return list;

            Annunci item;

            try
            {
                string query = "SELECT TOP * FROM " + Tblarchivio + " where CodiceTipologia like @CodiceTipologia order BY DataInserimento Desc";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@CodiceTipologia", CodTipologia);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Annunci();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSottoProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto"));

                        if (!reader["DATITECNICII"].Equals(DBNull.Value))
                            item.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                        if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                            item.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                        if (!reader["EMAIL"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        if (!reader["FAX"].Equals(DBNull.Value))
                            item.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                        if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                        if (!reader["TELEFONO"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                        if (!reader["WEBSITE"].Equals(DBNull.Value))
                            item.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));
                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));


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
                        if (!reader["Parametro1"].Equals(DBNull.Value))
                            item.Parametro1 = reader.GetString(reader.GetOrdinal("Parametro1"));
                        if (!reader["Parametro2"].Equals(DBNull.Value))
                            item.Parametro2 = reader.GetString(reader.GetOrdinal("Parametro2"));
                        if (!reader["Parametro3"].Equals(DBNull.Value))
                            item.Parametro3 = reader.GetString(reader.GetOrdinal("Parametro3"));
                        if (!reader["Parametro4"].Equals(DBNull.Value))
                            item.Parametro4 = reader.GetString(reader.GetOrdinal("Parametro4"));
                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetString(reader.GetOrdinal("Anno"));
                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Lista Prodotti :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Funzione che carica la lista dei sotto-prodotti scelto un codice prodotto
        /// </summary>
        /// <param name="connection">Connessione</param>
        /// <param name="CodProdotto">Codice prodotto richiesto</param>
        /// <returns></returns>
        public AnnunciCollection CaricaListaSottoprodottiPerCodiceProdotto(string connection, string CodProdotto)
        {
            AnnunciCollection list = new AnnunciCollection();
            if (connection == null || connection == "") return list;
            if (CodProdotto == null || CodProdotto == "") return list;

            Annunci item;

            try
            {
                string query = "SELECT TOP * FROM " + Tblarchivio + " where CodiceProdotto like @CodiceProdotto order BY DataInserimento Desc";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@CodiceProdotto", CodProdotto);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Annunci();
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceSottoProdotto"].Equals(DBNull.Value))
                            item.CodiceSottoProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto"));

                        if (!reader["DATITECNICII"].Equals(DBNull.Value))
                            item.DatitecniciI = reader.GetString(reader.GetOrdinal("DATITECNICII"));
                        if (!reader["DATITECNICIGB"].Equals(DBNull.Value))
                            item.DatitecniciGB = reader.GetString(reader.GetOrdinal("DATITECNICIGB"));
                        if (!reader["EMAIL"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
                        if (!reader["FAX"].Equals(DBNull.Value))
                            item.Fax = reader.GetString(reader.GetOrdinal("FAX"));
                        if (!reader["INDIRIZZO"].Equals(DBNull.Value))
                            item.Indirizzo = reader.GetString(reader.GetOrdinal("INDIRIZZO"));
                        if (!reader["TELEFONO"].Equals(DBNull.Value))
                            item.Telefono = reader.GetString(reader.GetOrdinal("TELEFONO"));
                        if (!reader["WEBSITE"].Equals(DBNull.Value))
                            item.Website = reader.GetString(reader.GetOrdinal("WEBSITE"));
                        if (!reader["Prezzo"].Equals(DBNull.Value))
                            item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));


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
                        if (!reader["Parametro1"].Equals(DBNull.Value))
                            item.Parametro1 = reader.GetString(reader.GetOrdinal("Parametro1"));
                        if (!reader["Parametro2"].Equals(DBNull.Value))
                            item.Parametro2 = reader.GetString(reader.GetOrdinal("Parametro2"));
                        if (!reader["Parametro3"].Equals(DBNull.Value))
                            item.Parametro3 = reader.GetString(reader.GetOrdinal("Parametro3"));
                        if (!reader["Parametro4"].Equals(DBNull.Value))
                            item.Parametro4 = reader.GetString(reader.GetOrdinal("Parametro4"));
                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetString(reader.GetOrdinal("Anno"));
                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Lista Sottoprodotti :" + error.Message, error);
            }

            return list;
        }


    }
}
