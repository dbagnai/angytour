using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using WelcomeLibrary.DOM;
using System.Xml;
using WelcomeLibrary.UF;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace WelcomeLibrary.DAL
{
    public class offerteDM
    {
        private string _tblarchivio = "TBL_ATTIVITA";
        public string Tblarchivio
        {
            get { return _tblarchivio; }
            set { _tblarchivio = value; }
        }
        private string _tblarchiviodettaglio = "TBL_ATTIVITA_DETAIL";

        public offerteDM()
        { }
        public offerteDM(string nometabella)
        {
            Tblarchivio = nometabella;
        }

        public Dictionary<string, Offerte> CaricaPrevNextOfferte(string connection, List<SQLiteParameter> parColl, string campoordinamento = "", bool includiarchiviati = false)
        {
            Dictionary<string, Offerte> list = new Dictionary<string, Offerte>();
            if (connection == null || connection == "") return list;
            Offerte item;
            string query = "";
            string queryfilter = "";
            try
            {
                List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();
                query = "SELECT * FROM " + "TBL_ATTIVITA ";

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@IdList"; }))
                {
                    SQLiteParameter pidlist = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@IdList"; });
                    string listaid = pidlist.Value.ToString();

                    listaid = listaid.Trim().Replace("|", ",");
                    string[] listaarray = listaid.Trim().Split(',');
                    if (listaarray != null && listaarray.Length > 0)
                    {
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE Id in (    ";
                        else
                            queryfilter += " AND  Id in (      ";
                        foreach (string codice in listaarray)
                        {
                            if (!string.IsNullOrEmpty(codice.Trim()))
                                queryfilter += " " + codice + " ,";
                        }
                        queryfilter = queryfilter.TrimEnd(',') + " ) ";
                    }
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceTIPOLOGIA"; }))
                {

                    SQLiteParameter ptip = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceTIPOLOGIA"; });
                    ptip.Value = ptip.Value.ToString().Replace("|", ",");
                    if (!ptip.Value.ToString().Contains(","))
                    {
                        _parUsed.Add(ptip);
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE CodiceTIPOLOGIA like @CodiceTIPOLOGIA ";
                        else
                            queryfilter += " AND CodiceTIPOLOGIA like @CodiceTIPOLOGIA  ";
                    }
                    else
                    {

                        string[] codici = ptip.Value.ToString().Split(',');
                        if (codici != null && codici.Length > 0)
                        {
                            if (!queryfilter.ToLower().Contains("where"))
                                queryfilter += " WHERE CodiceTIPOLOGIA in (    ";
                            else
                                queryfilter += " AND  CodiceTIPOLOGIA in (      ";
                            foreach (string codice in codici)
                            {
                                if (!string.IsNullOrEmpty(codice.Trim()))
                                    queryfilter += " '" + codice + "' ,";
                            }
                            queryfilter = queryfilter.TrimEnd(',') + " ) ";
                        }
                    }
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCategoria"; }))
                {
                    SQLiteParameter pcat = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCategoria"; });
                    _parUsed.Add(pcat);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE CodiceCategoria like @CodiceCategoria ";
                    else
                        queryfilter += " AND CodiceCategoria like @CodiceCategoria  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCategoria2Liv"; }))
                {
                    SQLiteParameter pcat2liv = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCategoria2Liv"; });
                    _parUsed.Add(pcat2liv);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE CodiceCategoria2Liv like @CodiceCategoria2Liv ";
                    else
                        queryfilter += " AND CodiceCategoria2Liv like @CodiceCategoria2Liv  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Vetrina"; }))
                {
                    SQLiteParameter vetrina = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Vetrina"; });
                    _parUsed.Add(vetrina);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Vetrina = @Vetrina ";
                    else
                        queryfilter += " AND  Vetrina = @Vetrina   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@promozioni"; }))
                {
                    SQLiteParameter promozione = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@promozioni"; });

                    _parUsed.Add(promozione);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Promozione = @promozioni ";
                    else
                        queryfilter += " AND  Promozione = @promozioni   ";

                    //_parUsed.Add(promozione);
                    //if (!queryfilter.ToLower().Contains("where"))
                    //	queryfilter += " WHERE Promozione = " + ((bool)promozione.Value ? "1" : "0") + " ";
                    //else
                    //	queryfilter += " AND  Promozione = " + ((bool)promozione.Value ? "1" : "0") + "   ";

                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Autore"; }))
                {
                    SQLiteParameter Autore = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Autore"; });
                    _parUsed.Add(Autore);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Autore = @Autore ";
                    else
                        queryfilter += " AND  Autore = @Autore   ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica1"; }))
                {
                    SQLiteParameter Caratteristica1 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica1"; });
                    _parUsed.Add(Caratteristica1);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica1 = @Caratteristica1 ";
                    else
                        queryfilter += " AND  Caratteristica1 = @Caratteristica1   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica2"; }))
                {
                    SQLiteParameter Caratteristica2 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica2"; });
                    _parUsed.Add(Caratteristica2);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica2 = @Caratteristica2 ";
                    else
                        queryfilter += " AND  Caratteristica2 = @Caratteristica2   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica3"; }))
                {
                    SQLiteParameter Caratteristica3 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica3"; });
                    _parUsed.Add(Caratteristica3);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica3 = @Caratteristica3 ";
                    else
                        queryfilter += " AND  Caratteristica3 = @Caratteristica3   ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica4"; }))
                {
                    SQLiteParameter Caratteristica4 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica4"; });
                    _parUsed.Add(Caratteristica4);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica4 = @Caratteristica4 ";
                    else
                        queryfilter += " AND  Caratteristica4 = @Caratteristica4  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica5"; }))
                {
                    SQLiteParameter Caratteristica5 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica5"; });
                    _parUsed.Add(Caratteristica5);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica5 = @Caratteristica5 ";
                    else
                        queryfilter += " AND  Caratteristica5 = @Caratteristica5 ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica6"; }))
                {
                    SQLiteParameter Caratteristica6 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica6"; });
                    _parUsed.Add(Caratteristica6);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica6 = @Caratteristica6 ";
                    else
                        queryfilter += " AND  Caratteristica6 = @Caratteristica6 ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Anno"; }))
                {
                    SQLiteParameter Carannao = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Anno"; });
                    _parUsed.Add(Carannao);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Anno = @Anno ";
                    else
                        queryfilter += " AND  Anno = @Anno   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@testoricerca"; }))
                {
                    SQLiteParameter testoricerca = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@testoricerca"; });
                    _parUsed.Add(testoricerca);
                    if (!queryfilter.ToLower().Contains("where"))
                    {
                        queryfilter += " WHERE ( Id like @testoricerca or  CodiceProdotto like @testoricerca or  DenominazioneI like @testoricerca or DenominazioneGB like @testoricerca or DenominazioneRU like @testoricerca ";
                        queryfilter += " or Nome_dts like @testoricerca or Cognome_dts like @testoricerca or Emailriservata_dts like @testoricerca or Email like @testoricerca ";
                        queryfilter += " or DescrizioneI like @testoricerca or DescrizioneGB like @testoricerca or DescrizioneRU like @testoricerca or DatitecniciI like @testoricerca or DatitecniciGB like @testoricerca or DatitecniciRU like @testoricerca  ";
                        queryfilter += " or Campo1I like @testoricerca or Campo1GB like @testoricerca  or Campo1RU like @testoricerca or Campo2I like @testoricerca or Campo2GB like @testoricerca  or Campo2RU like @testoricerca  or xmlValue like @testoricerca ) ";
                    }
                    else
                    {
                        queryfilter += " AND ( Id like @testoricerca or  CodiceProdotto like @testoricerca or  DenominazioneI like @testoricerca or DenominazioneGB like @testoricerca or DenominazioneRU like @testoricerca ";
                        queryfilter += " or Nome_dts like @testoricerca or Cognome_dts like @testoricerca or Emailriservata_dts like @testoricerca or Email like @testoricerca ";
                        queryfilter += " or DescrizioneI like @testoricerca or DescrizioneGB like @testoricerca or DescrizioneRU like @testoricerca or DatitecniciI like @testoricerca or DatitecniciGB like @testoricerca or DatitecniciRU like @testoricerca  ";
                        queryfilter += " or Campo1I like @testoricerca or Campo1GB like @testoricerca  or Campo1RU like @testoricerca or Campo2I like @testoricerca or Campo2GB like @testoricerca  or Campo2RU like @testoricerca  or xmlValue like @testoricerca ) ";
                    }
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio"; })
                   && parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine"; }))
                {
                    SQLiteParameter _datainizio = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio"; });
                    SQLiteParameter datainizio = new SQLiteParameter(_datainizio.ParameterName, _datainizio.Value);
                    _parUsed.Add(datainizio);

                    SQLiteParameter _datafine = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine"; });
                    SQLiteParameter datafine = new SQLiteParameter(_datafine.ParameterName, _datafine.Value);
                    _parUsed.Add(datafine);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE  ( DataInserimento >= @Data_inizio and  DataInserimento <= @Data_fine )  ";
                    else
                        queryfilter += " AND   ( DataInserimento >= @Data_inizio and  DataInserimento <= @Data_fine )  ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio1"; })
              && parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine1"; }))
                {
                    SQLiteParameter _datainizio1 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio1"; });
                    SQLiteParameter datainizio1 = new SQLiteParameter(_datainizio1.ParameterName, _datainizio1.Value);
                    _parUsed.Add(datainizio1);

                    SQLiteParameter _datafine1 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine1"; });
                    SQLiteParameter datafine1 = new SQLiteParameter(_datafine1.ParameterName, _datafine1.Value);
                    _parUsed.Add(datafine1);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE  ( Data1 >= @Data_inizio1 and  Data1 <= @Data_fine1 )  ";
                    else
                        queryfilter += " AND   ( Data1 >= @Data_inizio1 and  Data1 <= @Data_fine1 )  ";
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@annofiltro"; }))
                {
                    SQLiteParameter _annofiltro = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@annofiltro"; });
                    SQLiteParameter annofiltro = new SQLiteParameter(_annofiltro.ParameterName, _annofiltro.Value);
                    _parUsed.Add(_annofiltro);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ((strftime('%Y',[DataInserimento])=@annofiltro))  ";
                    else
                        queryfilter += " AND  ((strftime('%Y',[DataInserimento])=@annofiltro))    ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@mesefiltro"; }))
                {
                    SQLiteParameter _mesefiltro = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@mesefiltro"; });
                    SQLiteParameter mesefiltro = new SQLiteParameter(_mesefiltro.ParameterName, _mesefiltro.Value);
                    _parUsed.Add(mesefiltro);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ((strftime('%m',[DataInserimento])=@mesefiltro))  ";
                    else
                        queryfilter += " AND  ((strftime('%m',[DataInserimento])=@mesefiltro))    ";

                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@giornofiltro"; }))
                {
                    SQLiteParameter _giornofiltro = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@giornofiltro"; });
                    SQLiteParameter giornofiltro = new SQLiteParameter(_giornofiltro.ParameterName, _giornofiltro.Value);
                    _parUsed.Add(giornofiltro);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ((strftime('%d',[DataInserimento])=@giornofiltro))  ";
                    else
                        queryfilter += " AND  ((strftime('%d',[DataInserimento])=@giornofiltro))    ";

                }

                if (!includiarchiviati)
                {
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE (Archiviato = 0)  ";
                    else
                        queryfilter += " AND  (Archiviato = 0)    ";
                }

                query += queryfilter;

                if (campoordinamento == "")
                    query += "  order BY DataInserimento Desc, Id Desc  ";
                else
                    query += "  order BY " + campoordinamento + " COLLATE NOCASE Desc, Id Desc ";



                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id"; }))
                {
                    SQLiteParameter pidvalue = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id"; });
                    _parUsed.Add(pidvalue);
                    long iditem = (long)pidvalue.Value; //valore id scheda
                    query = "drop table if exists tmp;" + "create temporary table tmp as " + query + ";";
                    query += "select rowid,* from tmp where rowid in ( (select rowid from tmp where ID=@Id)-1, (select rowid from tmp where ID=@Id) , (select rowid from tmp where ID=@Id)+1) order by rowid;";
                    string keydict = "prev";
                    SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                    using (reader)
                    {
                        if (reader == null) { return list; };
                        if (reader.HasRows == false)
                            return list;

                        while (reader.Read())
                        {
                            item = new Offerte();
                            long progressivo = reader.GetInt64(reader.GetOrdinal("rowid")); //Progressivo per ordinamento e filtro
                            item.Id = reader.GetInt64(reader.GetOrdinal("ID"));

                            if (!reader["DENOMINAZIONEGB"].Equals(DBNull.Value))
                                item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                            if (!reader["DENOMINAZIONEI"].Equals(DBNull.Value))
                                item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));
                            if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                                item.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                            item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                            item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                            if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                                item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                            if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                                item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                            if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                                item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

                            if (item.Id == iditem) // al passaggio dall'elemento centrale imopsto la chiave successiva
                                keydict = "next";
                            if (item.Id != iditem)
                            {
                                if (!list.ContainsKey(keydict))
                                    list.Add(keydict, item);
                            }
                        }
                    }
                }

            }
            catch
            {
                //throw new ApplicationException("Errore Caricamento offerte :" + error.Message, error);
            }
            return list;
        }


        /// <summary>
        /// Carica le offerte in base ai parametri passati in parColl
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="parColl"></param>
        /// <param name="maxrecord"></param>
        /// <returns></returns>
        public OfferteCollection CaricaOfferteFiltrate(string connection, List<SQLiteParameter> parColl, string maxrecord = "", string LinguaFiltro = "", bool? filtrocatalogo = null, string campoordinamento = "", bool includiarchiviati = false, long page = 1, long pagesize = 0)
        {
            OfferteCollection list = new OfferteCollection();
            if (connection == null || connection == "") return list;
            //if (parColl == null || parColl.Count < 2) return list;

            Offerte item;
            try
            {
                List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();

                //StringBuilder queryCols = new StringBuilder();
                //queryCols.AppendLine("A.ID ");
                //queryCols.AppendLine(", A.Id_dts_collegato ");
                //queryCols.AppendLine(", A.CodiceNazione ");
                //queryCols.AppendLine(", A.CodiceREGIONE ");
                //queryCols.AppendLine(", A.CodicePROVINCIA ");
                //queryCols.AppendLine(", A.CodiceCOMUNE ");
                //queryCols.AppendLine(", A.CodiceTIPOLOGIA ");
                //queryCols.AppendLine(", A.WEBSITE ");
                //queryCols.AppendLine(", A.EMAIL ");
                //queryCols.AppendLine(", A.TELEFONO ");
                //queryCols.AppendLine(", A.FAX ");
                //queryCols.AppendLine(", A.INDIRIZZO ");
                //queryCols.AppendLine(", A.DENOMINAZIONEGB ");
                //queryCols.AppendLine(", A.DENOMINAZIONEI ");
                //queryCols.AppendLine(", A.DESCRIZIONEI ");
                //queryCols.AppendLine(", A.DESCRIZIONEGB ");
                //queryCols.AppendLine(", A.DATITECNICII ");
                //queryCols.AppendLine(", A.DATITECNICIGB ");
                //queryCols.AppendLine(", A.FotoSchema ");
                //queryCols.AppendLine(", A.FotoValori ");
                //queryCols.AppendLine(", A.Datainserimento ");
                //queryCols.AppendLine(", A.CodiceProdotto ");
                //queryCols.AppendLine(", A.CodiceCategoria ");
                //queryCols.AppendLine(", A.CodiceCategoria2Liv ");
                //queryCols.AppendLine(", A.Prezzo ");
                //queryCols.AppendLine(", A.PrezzoListino ");
                //queryCols.AppendLine(", A.Vetrina ");
                //queryCols.AppendLine(", A.AbilitaContatto ");
                //queryCols.AppendLine(", A.linkVideo ");
                //queryCols.AppendLine(", A.Campo1I ");
                //queryCols.AppendLine(", A.Campo2I ");
                //queryCols.AppendLine(", A.Campo1GB ");
                //queryCols.AppendLine(", A.Campo2GB ");
                //queryCols.AppendLine(", A.Id_collegato ");
                //queryCols.AppendLine(", A.Caratteristica1 ");
                //queryCols.AppendLine(", A.Caratteristica2 ");
                //queryCols.AppendLine(", A.Anno ");
                //queryCols.AppendLine(", A.Archiviato ");
                //queryCols.AppendLine(", A.Caratteristica3 ");
                //queryCols.AppendLine(", A.Caratteristica4 ");
                //queryCols.AppendLine(", A.Caratteristica5 ");
                //queryCols.AppendLine(", A.Caratteristica6 ");
                //queryCols.AppendLine(", A.Autore ");
                //queryCols.AppendLine(", A.XmlValue ");
                //queryCols.AppendLine(", A.Data1 ");
                //queryCols.AppendLine(", A.DENOMINAZIONERU ");
                //queryCols.AppendLine(", A.DESCRIZIONERU ");
                //queryCols.AppendLine(", A.DATITECNICIRU ");
                //queryCols.AppendLine(", A.Campo1RU ");
                //queryCols.AppendLine(", A.Campo2RU ");
                //queryCols.AppendLine(", A.Promozione ");
                //queryCols.AppendLine(", A.Qta_vendita ");
                //queryCols.AppendLine(", B.ID_DTS ");
                //queryCols.AppendLine(", B.pivacf_dts ");
                //queryCols.AppendLine(", B.nome_dts ");
                //queryCols.AppendLine(", B.cognome_dts ");
                //queryCols.AppendLine(", B.datanascita_dts ");
                //queryCols.AppendLine(", B.sociopresentatore1_dts ");
                //queryCols.AppendLine(", B.sociopresentatore2_dts ");
                //queryCols.AppendLine(", B.telefonoprivato_dts ");
                //queryCols.AppendLine(", B.annolaurea_dts ");
                //queryCols.AppendLine(", B.annospecializzazione_dts ");
                //queryCols.AppendLine(", B.altrespecializzazioni_dts ");
                //queryCols.AppendLine(", B.socioSicpre_dts ");
                //queryCols.AppendLine(", B.socioIsaps_dts ");
                //queryCols.AppendLine(", B.socioaltraassociazione_dts ");
                //queryCols.AppendLine(", B.trattamenticollegati_dts ");
                //queryCols.AppendLine(", B.accettazioneStatuto_dts ");
                //queryCols.AppendLine(", B.certificazione_dts ");
                //queryCols.AppendLine(", B.emailriservata_dts ");
                //queryCols.AppendLine(", B.CodiceNAZIONE1_dts ");
                //queryCols.AppendLine(", B.CodiceREGIONE1_dts ");
                //queryCols.AppendLine(", B.CodicePROVINCIA1_dts ");
                //queryCols.AppendLine(", B.CodiceCOMUNE1_dts ");
                //queryCols.AppendLine(", B.CodiceNAZIONE2_dts ");
                //queryCols.AppendLine(", B.CodiceREGIONE2_dts ");
                //queryCols.AppendLine(", B.CodicePROVINCIA2_dts ");
                //queryCols.AppendLine(", B.CodiceCOMUNE2_dts ");
                //queryCols.AppendLine(", B.CodiceNAZIONE3_dts ");
                //queryCols.AppendLine(", B.CodiceREGIONE3_dts ");
                //queryCols.AppendLine(", B.CodicePROVINCIA3_dts ");
                //queryCols.AppendLine(", B.CodiceCOMUNE3_dts ");
                //queryCols.AppendLine(", B.latitudine1_dts ");
                //queryCols.AppendLine(", B.longitudine1_dts ");
                //queryCols.AppendLine(", B.latitudine2_dts ");
                //queryCols.AppendLine(", B.longitudine2_dts ");
                //queryCols.AppendLine(", B.latitudine3_dts ");
                //queryCols.AppendLine(", B.longitudine3_dts ");
                //queryCols.AppendLine(", B.bloccoaccesso_dts ");
                //queryCols.AppendLine(", B.via1_dts ");
                //queryCols.AppendLine(", B.cap1_dts ");
                //queryCols.AppendLine(", B.nomeposizione1_dts ");
                //queryCols.AppendLine(", B.telefono1_dts ");
                //queryCols.AppendLine(", B.via2_dts ");
                //queryCols.AppendLine(", B.cap2_dts ");
                //queryCols.AppendLine(", B.nomeposizione2_dts ");
                //queryCols.AppendLine(", B.telefono2_dts ");
                //queryCols.AppendLine(", B.via3_dts ");
                //queryCols.AppendLine(", B.cap3_dts ");
                //queryCols.AppendLine(", B.nomeposizione3_dts ");
                //queryCols.AppendLine(", B.telefono3_dts ");
                //queryCols.AppendLine(", B.pagamenti_dts ");
                //queryCols.AppendLine(", B.ricfatt_dts ");
                //queryCols.AppendLine(", B.indirizzofatt_dts ");
                //queryCols.AppendLine(", B.noteriservate_dts ");
                //queryCols.AppendLine(", B.niscrordine_dts ");
                //queryCols.AppendLine(", B.annofrequenza_dts ");
                //queryCols.AppendLine(", B.nomeuniversita_dts ");
                //queryCols.AppendLine(", B.dettagliuniversita_dts ");
                //queryCols.AppendLine(", B.Boolfields_dts ");
                //queryCols.AppendLine(", B.Textfield1_dts ");
                //queryCols.AppendLine(", B.Interventieseguiti_dts ");
                //queryCols.AppendLine(", B.locordine_dts ");



                string query = "";
                string queryfilter = "";

                //	query = "SELECT " + queryCols.ToString() + " FROM " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts  ";
                query = "SELECT A.*,B.* FROM " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts  ";


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id"; }))
                {
                    SQLiteParameter pidvalue = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id"; });
                    _parUsed.Add(pidvalue);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Id like @Id ";
                    else
                        queryfilter += " AND Id like @Id  ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@IdList"; }))
                {
                    SQLiteParameter pidlist = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@IdList"; });
                    string listaid = pidlist.Value.ToString();

                    listaid = listaid.Trim().ToString().Replace("|", ",");

                    string[] listaarray = listaid.Trim().Split(',');
                    if (listaarray != null && listaarray.Length > 0)
                    {
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE Id in (    ";
                        else
                            queryfilter += " AND  Id in (      ";
                        foreach (string codice in listaarray)
                        {
                            if (!string.IsNullOrEmpty(codice.Trim()))
                                queryfilter += " " + codice + " ,";
                        }
                        queryfilter = queryfilter.TrimEnd(',') + " ) ";
                    }
                }

                //Per ogni parametro vedo se esiste e lo inserisco nello script
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceNAZIONE"; }))
                {
                    SQLiteParameter pnaz = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceNAZIONE"; });
                    _parUsed.Add(pnaz);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ( CodiceNAZIONE1_dts like @CodiceNAZIONE or  CodiceNAZIONE2_dts like @CodiceNAZIONE  or  CodiceNAZIONE3_dts like @CodiceNAZIONE   ) ";
                    else
                        queryfilter += " AND  ( CodiceNAZIONE1_dts like @CodiceNAZIONE or  CodiceNAZIONE2_dts like @CodiceNAZIONE  or  CodiceNAZIONE3_dts like @CodiceNAZIONE )   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceREGIONE"; }))
                {
                    SQLiteParameter preg = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceREGIONE"; });
                    _parUsed.Add(preg);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ( CodiceREGIONE like @CodiceREGIONE or CodiceREGIONE1_dts like @CodiceREGIONE or CodiceREGIONE2_dts like @CodiceREGIONE or CodiceREGIONE3_dts like @CodiceREGIONE ) ";
                    else
                        queryfilter += " AND  ( CodiceREGIONE like @CodiceREGIONE or CodiceREGIONE1_dts like @CodiceREGIONE or CodiceREGIONE2_dts like @CodiceREGIONE or CodiceREGIONE3_dts like @CodiceREGIONE ) ";
                }
                //Per ogni parametro vedo se esiste e lo inserisco nello script
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodicePROVINCIA"; }))
                {
                    SQLiteParameter pprov = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodicePROVINCIA"; });
                    _parUsed.Add(pprov);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ( CodicePROVINCIA like @CodicePROVINCIA or CodicePROVINCIA1_dts like @CodicePROVINCIA or CodicePROVINCIA2_dts like @CodicePROVINCIA  or CodicePROVINCIA3_dts like @CodicePROVINCIA ) ";
                    else
                        queryfilter += " AND  ( CodicePROVINCIA like @CodicePROVINCIA or CodicePROVINCIA1_dts like @CodicePROVINCIA or CodicePROVINCIA2_dts like @CodicePROVINCIA  or CodicePROVINCIA3_dts like @CodicePROVINCIA )   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCOMUNE"; }))
                {
                    SQLiteParameter pcom = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCOMUNE"; });
                    _parUsed.Add(pcom);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ( CodiceCOMUNE like @CodiceCOMUNE or CodiceCOMUNE1_dts like @CodiceCOMUNE or CodiceCOMUNE2_dts like @CodiceCOMUNE or CodiceCOMUNE3_dts like @CodiceCOMUNE ) ";
                    else
                        queryfilter += " AND  ( CodiceCOMUNE like @CodiceCOMUNE or CodiceCOMUNE1_dts like @CodiceCOMUNE or CodiceCOMUNE2_dts like @CodiceCOMUNE or CodiceCOMUNE3_dts like @CodiceCOMUNE )   ";
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceTIPOLOGIA"; }))
                {

                    SQLiteParameter ptip = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceTIPOLOGIA"; });
                    ptip.Value = ptip.Value.ToString().Trim().Replace("|", ",");

                    if (!ptip.Value.ToString().Contains(","))
                    {
                        _parUsed.Add(ptip);
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE CodiceTIPOLOGIA like @CodiceTIPOLOGIA ";
                        else
                            queryfilter += " AND CodiceTIPOLOGIA like @CodiceTIPOLOGIA  ";
                    }
                    else
                    {
                        string[] codici = ptip.Value.ToString().Split(',');
                        if (codici != null && codici.Length > 0)
                        {
                            if (!queryfilter.ToLower().Contains("where"))
                                queryfilter += " WHERE CodiceTIPOLOGIA in (    ";
                            else
                                queryfilter += " AND  CodiceTIPOLOGIA in (      ";
                            foreach (string codice in codici)
                            {
                                if (!string.IsNullOrEmpty(codice.Trim()))
                                    queryfilter += " '" + codice + "' ,";
                            }
                            queryfilter = queryfilter.TrimEnd(',') + " ) ";
                        }
                    }
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id_collegato"; }))
                {
                    SQLiteParameter pId_collegato = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Id_collegato"; });
                    _parUsed.Add(pId_collegato);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Id_collegato like @Id_collegato ";
                    else
                        queryfilter += " AND Id_collegato like @Id_collegato  ";
                }

                if (filtrocatalogo != null)
                    if (filtrocatalogo.Value) //Filtro i codicditipologia compresi tra 100 e 199 ( catalogo ) rispetto agli atri
                    {
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE ((( CAST(SUBSTR(A.[CodiceTIPOLOGIA],4) as INTEGER) )>=100) and  ((CAST(SUBSTR(A.[CodiceTIPOLOGIA],4) as INTEGER))<200)) ";
                        else
                            queryfilter += " AND (((CAST(SUBSTR(A.[CodiceTIPOLOGIA],4) as INTEGER))>=100) and  ((CAST(SUBSTR(A.[CodiceTIPOLOGIA],4)  as INTEGER))<200)) ";
                    }
                    else
                    {
                        if (!queryfilter.ToLower().Contains("where"))
                            queryfilter += " WHERE (((CAST(SUBSTR(A.[CodiceTIPOLOGIA],4)  as INTEGER))<100) or  ((CAST(SUBSTR(A.[CodiceTIPOLOGIA],4)  as INTEGER))>=200)) ";
                        else
                            queryfilter += " AND  (((CAST(SUBSTR(A.[CodiceTIPOLOGIA],4)  as INTEGER))<100) or  ((CAST(SUBSTR(A.[CodiceTIPOLOGIA],4)  as INTEGER))>=200)) ";
                    }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Abilitacontatto"; }))
                {
                    SQLiteParameter _pabilc = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Abilitacontatto"; });
                    SQLiteParameter pabilc = new SQLiteParameter(_pabilc.ParameterName, _pabilc.Value);

                    _parUsed.Add(pabilc);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Abilitacontatto = @Abilitacontatto ";
                    else
                        queryfilter += " AND Abilitacontatto = @Abilitacontatto ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Archiviato"; }))
                {
                    SQLiteParameter _parch = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Archiviato"; });
                    SQLiteParameter parch = new SQLiteParameter(_parch.ParameterName, _parch.Value);

                    _parUsed.Add(parch);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Archiviato = @Archiviato ";
                    else
                        queryfilter += " AND Archiviato = @Archiviato ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@filtrodisponibili"; }))
                {
                    SQLiteParameter _parch = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@filtrodisponibili"; });
                    try
                    {
                        bool _par = Convert.ToBoolean(_parch.Value);

                        if (_par)
                        {
                            if (!queryfilter.ToLower().Contains("where"))
                                queryfilter += " WHERE (Qta_vendita > 0 or Qta_vendita is null)  ";
                            else
                                queryfilter += " AND (Qta_vendita > 0 or Qta_vendita is null)  ";
                        }
                        else
                        {
                            if (!queryfilter.ToLower().Contains("where"))
                                queryfilter += " WHERE (Qta_vendita <= 0 and Qta_vendita is not null)  ";
                            else
                                queryfilter += " AND (Qta_vendita <= 0 and Qta_vendita is not null)  ";
                        }

                    }
                    catch { }
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@PrezzoMin"; }))
                {
                    SQLiteParameter ppmin = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@PrezzoMin"; });
                    _parUsed.Add(ppmin);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Prezzo >= @PrezzoMin ";
                    else
                        queryfilter += " AND Prezzo >= @PrezzoMin  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@PrezzoMax"; }))
                {
                    SQLiteParameter ppmax = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@PrezzoMax"; });
                    _parUsed.Add(ppmax);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE  Prezzo <= @PrezzoMax  ";
                    else
                        queryfilter += " AND  Prezzo <= @PrezzoMax   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCategoria"; }))
                {
                    SQLiteParameter pcat = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCategoria"; });
                    _parUsed.Add(pcat);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE CodiceCategoria like @CodiceCategoria ";
                    else
                        queryfilter += " AND CodiceCategoria like @CodiceCategoria  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCategoria2Liv"; }))
                {
                    SQLiteParameter pcat2liv = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@CodiceCategoria2Liv"; });
                    _parUsed.Add(pcat2liv);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE CodiceCategoria2Liv like @CodiceCategoria2Liv ";
                    else
                        queryfilter += " AND CodiceCategoria2Liv like @CodiceCategoria2Liv  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Vetrina"; }))
                {
                    SQLiteParameter vetrina = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Vetrina"; });
                    _parUsed.Add(vetrina);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Vetrina = @Vetrina ";
                    else
                        queryfilter += " AND  Vetrina = @Vetrina   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@promozioni"; }))
                {
                    SQLiteParameter promozione = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@promozioni"; });

                    _parUsed.Add(promozione);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Promozione = @promozioni ";
                    else
                        queryfilter += " AND  Promozione = @promozioni   ";

                    //_parUsed.Add(promozione);
                    //if (!queryfilter.ToLower().Contains("where"))
                    //	queryfilter += " WHERE Promozione = " + ((bool)promozione.Value ? "1" : "0") + " ";
                    //else
                    //	queryfilter += " AND  Promozione = " + ((bool)promozione.Value ? "1" : "0") + "   ";

                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Autore"; }))
                {
                    SQLiteParameter Autore = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Autore"; });
                    _parUsed.Add(Autore);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Autore = @Autore ";
                    else
                        queryfilter += " AND  Autore = @Autore   ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica1"; }))
                {
                    SQLiteParameter Caratteristica1 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica1"; });
                    _parUsed.Add(Caratteristica1);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica1 = @Caratteristica1 ";
                    else
                        queryfilter += " AND  Caratteristica1 = @Caratteristica1   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica2"; }))
                {
                    SQLiteParameter Caratteristica2 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica2"; });
                    _parUsed.Add(Caratteristica2);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica2 = @Caratteristica2 ";
                    else
                        queryfilter += " AND  Caratteristica2 = @Caratteristica2   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica3"; }))
                {
                    SQLiteParameter Caratteristica3 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica3"; });
                    _parUsed.Add(Caratteristica3);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica3 = @Caratteristica3 ";
                    else
                        queryfilter += " AND  Caratteristica3 = @Caratteristica3   ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica4"; }))
                {
                    SQLiteParameter Caratteristica4 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica4"; });
                    _parUsed.Add(Caratteristica4);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica4 = @Caratteristica4 ";
                    else
                        queryfilter += " AND  Caratteristica4 = @Caratteristica4  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica5"; }))
                {
                    SQLiteParameter Caratteristica5 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica5"; });
                    _parUsed.Add(Caratteristica5);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica5 = @Caratteristica5 ";
                    else
                        queryfilter += " AND  Caratteristica5 = @Caratteristica5 ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica6"; }))
                {
                    SQLiteParameter Caratteristica6 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Caratteristica6"; });
                    _parUsed.Add(Caratteristica6);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Caratteristica6 = @Caratteristica6 ";
                    else
                        queryfilter += " AND  Caratteristica6 = @Caratteristica6 ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Anno"; }))
                {
                    SQLiteParameter Carannao = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Anno"; });
                    _parUsed.Add(Carannao);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Anno = @Anno ";
                    else
                        queryfilter += " AND  Anno = @Anno   ";
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@testoricerca"; }))
                {
                    SQLiteParameter testoricerca = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@testoricerca"; });
                    _parUsed.Add(testoricerca);
                    if (!queryfilter.ToLower().Contains("where"))
                    {
                        queryfilter += " WHERE ( Id like @testoricerca or  CodiceProdotto like @testoricerca or  DenominazioneI like @testoricerca or DenominazioneGB like @testoricerca or DenominazioneRU like @testoricerca ";
                        queryfilter += " or Nome_dts like @testoricerca or Cognome_dts like @testoricerca or Emailriservata_dts like @testoricerca or Email like @testoricerca ";
                        queryfilter += " or DescrizioneI like @testoricerca or DescrizioneGB like @testoricerca or DescrizioneRU like @testoricerca or DatitecniciI like @testoricerca or DatitecniciGB like @testoricerca or DatitecniciRU like @testoricerca  ";
                        queryfilter += " or Campo1I like @testoricerca or Campo1GB like @testoricerca  or Campo1RU like @testoricerca or Campo2I like @testoricerca or Campo2GB like @testoricerca  or Campo2RU like @testoricerca  or xmlValue like @testoricerca ) ";
                    }
                    else
                    {
                        queryfilter += " AND ( Id like @testoricerca or  CodiceProdotto like @testoricerca or  DenominazioneI like @testoricerca or DenominazioneGB like @testoricerca or DenominazioneRU like @testoricerca ";
                        queryfilter += " or Nome_dts like @testoricerca or Cognome_dts like @testoricerca or Emailriservata_dts like @testoricerca or Email like @testoricerca ";
                        queryfilter += " or DescrizioneI like @testoricerca or DescrizioneGB like @testoricerca or DescrizioneRU like @testoricerca or DatitecniciI like @testoricerca or DatitecniciGB like @testoricerca or DatitecniciRU like @testoricerca  ";
                        queryfilter += " or Campo1I like @testoricerca or Campo1GB like @testoricerca  or Campo1RU like @testoricerca or Campo2I like @testoricerca or Campo2GB like @testoricerca  or Campo2RU like @testoricerca  or xmlValue like @testoricerca ) ";
                    }
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@stringafiltropagamenti"; }))
                {
                    SQLiteParameter stringafiltropagamenti = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@stringafiltropagamenti"; });
                    _parUsed.Add(stringafiltropagamenti);
                    if (!queryfilter.ToLower().Contains("where"))
                    {
                        queryfilter += " WHERE ( Pagamenti_dts like @stringafiltropagamenti ) ";
                    }
                    else
                    {
                        queryfilter += " AND  ( Pagamenti_dts like @stringafiltropagamenti )   ";
                    }
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@stringafiltrotrattamenti"; }))
                {
                    SQLiteParameter stringafiltrotrattamenti = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@stringafiltrotrattamenti"; });
                    _parUsed.Add(stringafiltrotrattamenti);
                    if (!queryfilter.ToLower().Contains("where"))
                    {
                        queryfilter += " WHERE ( Trattamenticollegati_dts like @stringafiltrotrattamenti ) ";
                    }
                    else
                    {
                        queryfilter += " AND  ( Trattamenticollegati_dts like @stringafiltrotrattamenti )   ";
                    }
                }



                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio"; })
                    && parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine"; }))
                {
                    SQLiteParameter _datainizio = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio"; });
                    SQLiteParameter datainizio = new SQLiteParameter(_datainizio.ParameterName, _datainizio.Value);
                    _parUsed.Add(datainizio);

                    SQLiteParameter _datafine = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine"; });
                    SQLiteParameter datafine = new SQLiteParameter(_datafine.ParameterName, _datafine.Value);
                    _parUsed.Add(datafine);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE  ( DataInserimento >= @Data_inizio and  DataInserimento <= @Data_fine )  ";
                    else
                        queryfilter += " AND   ( DataInserimento >= @Data_inizio and  DataInserimento <= @Data_fine )  ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio1"; })
              && parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine1"; }))
                {
                    SQLiteParameter _datainizio1 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_inizio1"; });
                    SQLiteParameter datainizio1 = new SQLiteParameter(_datainizio1.ParameterName, _datainizio1.Value);
                    _parUsed.Add(datainizio1);

                    SQLiteParameter _datafine1 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data_fine1"; });
                    SQLiteParameter datafine1 = new SQLiteParameter(_datafine1.ParameterName, _datafine1.Value);
                    _parUsed.Add(datafine1);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE  ( Data1 >= @Data_inizio1 and  Data1 <= @Data_fine1 )  ";
                    else
                        queryfilter += " AND   ( Data1 >= @Data_inizio1 and  Data1 <= @Data_fine1 )  ";
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@annofiltro"; }))
                {
                    SQLiteParameter _annofiltro = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@annofiltro"; });
                    SQLiteParameter annofiltro = new SQLiteParameter(_annofiltro.ParameterName, _annofiltro.Value);
                    _parUsed.Add(_annofiltro);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ((strftime('%Y',[DataInserimento])=@annofiltro))  ";
                    else
                        queryfilter += " AND  ((strftime('%Y',[DataInserimento])=@annofiltro))    ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@mesefiltro"; }))
                {
                    SQLiteParameter _mesefiltro = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@mesefiltro"; });
                    SQLiteParameter mesefiltro = new SQLiteParameter(_mesefiltro.ParameterName, _mesefiltro.Value);
                    _parUsed.Add(mesefiltro);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ((strftime('%m',[DataInserimento])=@mesefiltro))  ";
                    else
                        queryfilter += " AND  ((strftime('%m',[DataInserimento])=@mesefiltro))    ";

                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@giornofiltro"; }))
                {
                    SQLiteParameter _giornofiltro = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@giornofiltro"; });
                    SQLiteParameter giornofiltro = new SQLiteParameter(_giornofiltro.ParameterName, _giornofiltro.Value);
                    _parUsed.Add(giornofiltro);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ((strftime('%d',[DataInserimento])=@giornofiltro))  ";
                    else
                        queryfilter += " AND  ((strftime('%d',[DataInserimento])=@giornofiltro))    ";

                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Bloccoaccesso_dts"; }))
                {
                    SQLiteParameter _Bloccoaccesso_dts = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Bloccoaccesso_dts"; });

                    SQLiteParameter Bloccoaccesso_dts = new SQLiteParameter(_Bloccoaccesso_dts.ParameterName, _Bloccoaccesso_dts.Value);
                    // Bloccoaccesso_dts.DbType = DbType.Boolean;
                    _parUsed.Add(_Bloccoaccesso_dts);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE ( Bloccoaccesso_dts=@Bloccoaccesso_dts )  ";
                    else
                        queryfilter += " AND  ( Bloccoaccesso_dts=@Bloccoaccesso_dts )    ";
                }


                if (!String.IsNullOrEmpty(LinguaFiltro))
                {
                    switch (LinguaFiltro)
                    {
                        case "GB":
                            if (!queryfilter.ToLower().Contains("where"))
                                queryfilter += " WHERE ( DenominazioneGB <> '' and DenominazioneGB is not null )  ";
                            else
                                queryfilter += " AND  ( DenominazioneGB <> ''  and DenominazioneGB is not null )    ";
                            break;
                        case "RU":
                            if (!queryfilter.ToLower().Contains("where"))
                                queryfilter += " WHERE ( DenominazioneRU <> '' and DenominazioneRU is not null )  ";
                            else
                                queryfilter += " AND  ( DenominazioneRU <> ''  and DenominazioneRU is not null )    ";
                            break;
                    }
                }
                if (!includiarchiviati)
                {
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE (Archiviato = 0)  ";
                    else
                        queryfilter += " AND  (Archiviato = 0)    ";
                }


                query += queryfilter;

                if (campoordinamento == "")
                    query += "  order BY DataInserimento Desc, Id Desc  ";
                else
                    query += "  order BY " + campoordinamento + " COLLATE NOCASE Desc, Id Desc ";

                if (!string.IsNullOrEmpty(maxrecord))
                    query += " LIMIT " + maxrecord;
                else
                {
                    if (pagesize != 0)
                    {
                        query += " limit " + (page - 1) * pagesize + "," + pagesize;
                    }
                }

                /*CALCOLO IL NUMERO DI RIGHE FILTRATE TOTALI*/

                long totalrecords = dbDataAccess.ExecuteScalar<long>("SELECT count(*) FROM  " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts  " + queryfilter, _parUsed, connection);
                list.Totrecs = totalrecords;


                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Offerte();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!reader["Id_collegato"].Equals(DBNull.Value))
                            item.Id_collegato = reader.GetInt64(reader.GetOrdinal("Id_collegato"));

                        if (!reader["Autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("Autore"));


                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));

                        if (!reader["Data1"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("Data1"));


                        if (!reader["DescrizioneGB"].Equals(DBNull.Value))
                            item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        if (!reader["DescrizioneI"].Equals(DBNull.Value))
                            item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        if (!reader["DENOMINAZIONEGB"].Equals(DBNull.Value))
                            item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        if (!reader["DENOMINAZIONEI"].Equals(DBNull.Value))
                            item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));


                        if (!reader["Campo1I"].Equals(DBNull.Value))
                            item.Campo1I = reader.GetString(reader.GetOrdinal("Campo1I"));
                        if (!reader["Campo1GB"].Equals(DBNull.Value))
                            item.Campo1GB = reader.GetString(reader.GetOrdinal("Campo1GB"));
                        if (!reader["Campo2I"].Equals(DBNull.Value))
                            item.Campo2I = reader.GetString(reader.GetOrdinal("Campo2I"));
                        if (!reader["Campo2GB"].Equals(DBNull.Value))
                            item.Campo2GB = reader.GetString(reader.GetOrdinal("Campo2GB"));

                        if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                            item.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
                        if (!reader["DescrizioneRU"].Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                            item.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                        if (!reader["Campo1RU"].Equals(DBNull.Value))
                            item.Campo1RU = reader.GetString(reader.GetOrdinal("Campo1RU"));
                        if (!reader["Campo2RU"].Equals(DBNull.Value))
                            item.Campo2RU = reader.GetString(reader.GetOrdinal("Campo2RU"));

                        if (!reader["XmlValue"].Equals(DBNull.Value))
                            item.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));


                        if (!reader["Caratteristica1"].Equals(DBNull.Value))
                            item.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                        if (!reader["Caratteristica2"].Equals(DBNull.Value))
                            item.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                        if (!reader["Caratteristica3"].Equals(DBNull.Value))
                            item.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                        if (!reader["Caratteristica4"].Equals(DBNull.Value))
                            item.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                        if (!reader["Caratteristica5"].Equals(DBNull.Value))
                            item.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                        if (!reader["Caratteristica6"].Equals(DBNull.Value))
                            item.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetInt64(reader.GetOrdinal("Anno"));


                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["linkVideo"].Equals(DBNull.Value))
                            item.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                            item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                        if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                            item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

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
                        if (!reader["PrezzoListino"].Equals(DBNull.Value))
                            item.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                        if (!reader["Vetrina"].Equals(DBNull.Value))
                            item.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));
                        if (!reader["Abilitacontatto"].Equals(DBNull.Value))
                            item.Abilitacontatto = reader.GetBoolean(reader.GetOrdinal("Abilitacontatto"));
                        if (!reader["Archiviato"].Equals(DBNull.Value))
                            item.Archiviato = reader.GetBoolean(reader.GetOrdinal("Archiviato"));

                        if (!reader["Qta_vendita"].Equals(DBNull.Value))
                            item.Qta_vendita = reader.GetDouble(reader.GetOrdinal("Qta_vendita"));
                        if (!reader["Promozione"].Equals(DBNull.Value))
                            item.Promozione = reader.GetBoolean(reader.GetOrdinal("Promozione"));

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


                        //CAMPI IN TABELLA COLLEGATA------------------------------------------------------------------
                        if (!reader["Id_dts_collegato"].Equals(DBNull.Value))
                            item.Id_dts_collegato = reader.GetInt64(reader.GetOrdinal("Id_dts_collegato"));

                        if (!reader["Pivacf_dts"].Equals(DBNull.Value))
                            item.Pivacf_dts = reader.GetString(reader.GetOrdinal("Pivacf_dts"));
                        if (!reader["Nome_dts"].Equals(DBNull.Value))
                            item.Nome_dts = reader.GetString(reader.GetOrdinal("Nome_dts"));
                        if (!reader["Cognome_dts"].Equals(DBNull.Value))
                            item.Cognome_dts = reader.GetString(reader.GetOrdinal("Cognome_dts"));
                        if (!reader["Datanascita_dts"].Equals(DBNull.Value))
                            item.Datanascita_dts = reader.GetDateTime(reader.GetOrdinal("Datanascita_dts"));
                        if (!reader["Sociopresentatore1_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore1_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore1_dts"));
                        if (!reader["Sociopresentatore2_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore2_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore2_dts"));
                        if (!reader["Telefonoprivato_dts"].Equals(DBNull.Value))
                            item.Telefonoprivato_dts = reader.GetString(reader.GetOrdinal("Telefonoprivato_dts"));
                        if (!reader["Annolaurea_dts"].Equals(DBNull.Value))
                            item.Annolaurea_dts = reader.GetString(reader.GetOrdinal("Annolaurea_dts"));
                        if (!reader["Annospecializzazione_dts"].Equals(DBNull.Value))
                            item.Annospecializzazione_dts = reader.GetString(reader.GetOrdinal("Annospecializzazione_dts"));
                        if (!reader["Altrespecializzazioni_dts"].Equals(DBNull.Value))
                            item.Altrespecializzazioni_dts = reader.GetString(reader.GetOrdinal("Altrespecializzazioni_dts"));
                        if (!reader["SocioSicpre_dts"].Equals(DBNull.Value))
                            item.SocioSicpre_dts = reader.GetBoolean(reader.GetOrdinal("SocioSicpre_dts"));
                        if (!reader["SocioIsaps_dts"].Equals(DBNull.Value))
                            item.SocioIsaps_dts = reader.GetBoolean(reader.GetOrdinal("SocioIsaps_dts"));
                        if (!reader["Socioaltraassociazione_dts"].Equals(DBNull.Value))
                            item.Socioaltraassociazione_dts = reader.GetString(reader.GetOrdinal("Socioaltraassociazione_dts"));
                        if (!reader["Trattamenticollegati_dts"].Equals(DBNull.Value))
                            item.Trattamenticollegati_dts = reader.GetString(reader.GetOrdinal("Trattamenticollegati_dts"));
                        if (!reader["AccettazioneStatuto_dts"].Equals(DBNull.Value))
                            item.AccettazioneStatuto_dts = reader.GetBoolean(reader.GetOrdinal("AccettazioneStatuto_dts"));

                        if (!reader["Certificazione_dts"].Equals(DBNull.Value))
                            item.Certificazione_dts = reader.GetBoolean(reader.GetOrdinal("Certificazione_dts"));
                        if (!reader["Emailriservata_dts"].Equals(DBNull.Value))
                            item.Emailriservata_dts = reader.GetString(reader.GetOrdinal("Emailriservata_dts"));

                        if (!reader["CodiceNAZIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE1_dts"));
                        if (!reader["CodiceREGIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE1_dts"));
                        if (!reader["CodicePROVINCIA1_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA1_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA1_dts"));
                        if (!reader["CodiceCOMUNE1_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE1_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE1_dts"));

                        if (!reader["CodiceNAZIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE2_dts"));
                        if (!reader["CodiceREGIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE2_dts"));
                        if (!reader["CodicePROVINCIA2_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA2_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA2_dts"));
                        if (!reader["CodiceCOMUNE2_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE2_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE2_dts"));

                        if (!reader["CodiceNAZIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE3_dts"));
                        if (!reader["CodiceREGIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE3_dts"));
                        if (!reader["CodicePROVINCIA3_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA3_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA3_dts"));
                        if (!reader["CodiceCOMUNE3_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE3_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE3_dts"));

                        if (!reader["Latitudine1_dts"].Equals(DBNull.Value))
                            item.Latitudine1_dts = reader.GetDouble(reader.GetOrdinal("Latitudine1_dts"));
                        if (!reader["Longitudine1_dts"].Equals(DBNull.Value))
                            item.Longitudine1_dts = reader.GetDouble(reader.GetOrdinal("Longitudine1_dts"));
                        if (!reader["Latitudine2_dts"].Equals(DBNull.Value))
                            item.Latitudine2_dts = reader.GetDouble(reader.GetOrdinal("Latitudine2_dts"));
                        if (!reader["Longitudine2_dts"].Equals(DBNull.Value))
                            item.Longitudine2_dts = reader.GetDouble(reader.GetOrdinal("Longitudine2_dts"));
                        if (!reader["Latitudine3_dts"].Equals(DBNull.Value))
                            item.Latitudine3_dts = reader.GetDouble(reader.GetOrdinal("Latitudine3_dts"));
                        if (!reader["Longitudine3_dts"].Equals(DBNull.Value))
                            item.Longitudine3_dts = reader.GetDouble(reader.GetOrdinal("Longitudine3_dts"));

                        if (!reader["Bloccoaccesso_dts"].Equals(DBNull.Value))
                            item.Bloccoaccesso_dts = reader.GetBoolean(reader.GetOrdinal("Bloccoaccesso_dts"));


                        if (!reader["Via1_dts"].Equals(DBNull.Value))
                            item.Via1_dts = reader.GetString(reader.GetOrdinal("Via1_dts"));
                        if (!reader["Cap1_dts"].Equals(DBNull.Value))
                            item.Cap1_dts = reader.GetString(reader.GetOrdinal("Cap1_dts"));
                        if (!reader["Nomeposizione1_dts"].Equals(DBNull.Value))
                            item.Nomeposizione1_dts = reader.GetString(reader.GetOrdinal("Nomeposizione1_dts"));
                        if (!reader["Telefono1_dts"].Equals(DBNull.Value))
                            item.Telefono1_dts = reader.GetString(reader.GetOrdinal("Telefono1_dts"));


                        if (!reader["Via2_dts"].Equals(DBNull.Value))
                            item.Via2_dts = reader.GetString(reader.GetOrdinal("Via2_dts"));
                        if (!reader["Cap2_dts"].Equals(DBNull.Value))
                            item.Cap2_dts = reader.GetString(reader.GetOrdinal("Cap2_dts"));
                        if (!reader["Nomeposizione2_dts"].Equals(DBNull.Value))
                            item.Nomeposizione2_dts = reader.GetString(reader.GetOrdinal("Nomeposizione2_dts"));
                        if (!reader["Telefono2_dts"].Equals(DBNull.Value))
                            item.Telefono2_dts = reader.GetString(reader.GetOrdinal("Telefono2_dts"));

                        if (!reader["Via3_dts"].Equals(DBNull.Value))
                            item.Via3_dts = reader.GetString(reader.GetOrdinal("Via3_dts"));
                        if (!reader["Cap3_dts"].Equals(DBNull.Value))
                            item.Cap3_dts = reader.GetString(reader.GetOrdinal("Cap3_dts"));
                        if (!reader["Nomeposizione3_dts"].Equals(DBNull.Value))
                            item.Nomeposizione3_dts = reader.GetString(reader.GetOrdinal("Nomeposizione3_dts"));
                        if (!reader["Telefono3_dts"].Equals(DBNull.Value))
                            item.Telefono3_dts = reader.GetString(reader.GetOrdinal("Telefono3_dts"));

                        if (!reader["Pagamenti_dts"].Equals(DBNull.Value))
                            item.Pagamenti_dts = reader.GetString(reader.GetOrdinal("Pagamenti_dts"));


                        if (!reader["ricfatt_dts"].Equals(DBNull.Value))
                            item.ricfatt_dts = reader.GetString(reader.GetOrdinal("ricfatt_dts"));

                        if (!reader["indirizzofatt_dts"].Equals(DBNull.Value))
                            item.indirizzofatt_dts = reader.GetString(reader.GetOrdinal("indirizzofatt_dts"));

                        if (!reader["noteriservate_dts"].Equals(DBNull.Value))
                            item.noteriservate_dts = reader.GetString(reader.GetOrdinal("noteriservate_dts"));

                        if (!reader["niscrordine_dts"].Equals(DBNull.Value))
                            item.niscrordine_dts = reader.GetString(reader.GetOrdinal("niscrordine_dts"));

                        if (!reader["locordine_dts"].Equals(DBNull.Value))
                            item.locordine_dts = reader.GetString(reader.GetOrdinal("locordine_dts"));

                        if (!reader["annofrequenza_dts"].Equals(DBNull.Value))
                            item.annofrequenza_dts = reader.GetString(reader.GetOrdinal("annofrequenza_dts"));
                        if (!reader["nomeuniversita_dts"].Equals(DBNull.Value))
                            item.nomeuniversita_dts = reader.GetString(reader.GetOrdinal("nomeuniversita_dts"));
                        if (!reader["dettagliuniversita_dts"].Equals(DBNull.Value))
                            item.dettagliuniversita_dts = reader.GetString(reader.GetOrdinal("dettagliuniversita_dts"));
                        if (!reader["Boolfields_dts"].Equals(DBNull.Value))
                            item.Boolfields_dts = reader.GetString(reader.GetOrdinal("Boolfields_dts"));
                        if (!reader["Textfield1_dts"].Equals(DBNull.Value))
                            item.Textfield1_dts = reader.GetString(reader.GetOrdinal("Textfield1_dts"));
                        if (!reader["Interventieseguiti_dts"].Equals(DBNull.Value))
                            item.Interventieseguiti_dts = reader.GetString(reader.GetOrdinal("Interventieseguiti_dts"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento offerte :" + error.Message, error);
            }

            return list;
        }


        /// <summary>
        /// le offerte collegate ad un certo id identificativo
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codicetipologia"></param>
        /// <returns></returns>
        public OfferteCollection CaricaOfferteCollegate(string connection, string idcollegato, string maxofferte = "", bool randomize = false, string LinguaFiltro = "", bool includiarchiviati = false, string campoordinamento = "")
        {
            if (connection == null || connection == "") return null;
            if (idcollegato == null || idcollegato == "") return null;
            OfferteCollection list = new OfferteCollection();
            Offerte item;
            try
            {
                string query = "";

                query = "SELECT A.*,B.* FROM " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts where Id_collegato=@Id_collegato ";


                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@Id_collegato", idcollegato);//OleDbType.VarChar
                parColl.Add(p1);

                if (!String.IsNullOrEmpty(LinguaFiltro))
                {
                    switch (LinguaFiltro)
                    {
                        case "GB":
                            if (!query.ToLower().Contains("where"))
                                query += " WHERE (DenominazioneGB <> '' and DenominazioneGB is not null)  ";
                            else
                                query += " AND  (DenominazioneGB <> ''  and DenominazioneGB is not null)    ";
                            break;
                        case "RU":
                            if (!query.ToLower().Contains("where"))
                                query += " WHERE (DenominazioneRU <> '' and DenominazioneRU is not null)  ";
                            else
                                query += " AND  (DenominazioneRU <> ''  and DenominazioneRU is not null)    ";
                            break;
                    }
                }
                if (!includiarchiviati)
                {
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE (Archiviato = 0)  ";
                    else
                        query += " AND  (Archiviato = 0)    ";
                }

                if (randomize)
                {
                    query += "ORDER BY random(), Id Desc";
                }
                else
                {
                    if (campoordinamento == "")
                        query += "  order BY DataInserimento Desc, Id Desc  ";
                    else
                        query += "  order BY " + campoordinamento + " COLLATE NOCASE Desc, Id Desc ";
                }

                if (!string.IsNullOrEmpty(maxofferte))
                    query = " limit " + maxofferte;


                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Offerte();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!reader["Id_collegato"].Equals(DBNull.Value))
                            item.Id_collegato = reader.GetInt64(reader.GetOrdinal("Id_collegato"));

                        if (!reader["Autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("Autore"));



                        if (!reader["Campo1I"].Equals(DBNull.Value))
                            item.Campo1I = reader.GetString(reader.GetOrdinal("Campo1I"));
                        if (!reader["Campo1GB"].Equals(DBNull.Value))
                            item.Campo1GB = reader.GetString(reader.GetOrdinal("Campo1GB"));
                        if (!reader["Campo2I"].Equals(DBNull.Value))
                            item.Campo2I = reader.GetString(reader.GetOrdinal("Campo2I"));
                        if (!reader["Campo2GB"].Equals(DBNull.Value))
                            item.Campo2GB = reader.GetString(reader.GetOrdinal("Campo2GB"));

                        if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                            item.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
                        if (!reader["DescrizioneRU"].Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                            item.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                        if (!reader["Campo1RU"].Equals(DBNull.Value))
                            item.Campo1RU = reader.GetString(reader.GetOrdinal("Campo1RU"));
                        if (!reader["Campo2RU"].Equals(DBNull.Value))
                            item.Campo2RU = reader.GetString(reader.GetOrdinal("Campo2RU"));

                        if (!reader["XmlValue"].Equals(DBNull.Value))
                            item.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));


                        if (!reader["Caratteristica1"].Equals(DBNull.Value))
                            item.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                        if (!reader["Caratteristica2"].Equals(DBNull.Value))
                            item.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                        if (!reader["Caratteristica3"].Equals(DBNull.Value))
                            item.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                        if (!reader["Caratteristica4"].Equals(DBNull.Value))
                            item.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                        if (!reader["Caratteristica5"].Equals(DBNull.Value))
                            item.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                        if (!reader["Caratteristica6"].Equals(DBNull.Value))
                            item.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetInt64(reader.GetOrdinal("Anno"));

                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Data1"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("Data1"));
                        if (!reader["DescrizioneGB"].Equals(DBNull.Value))
                            item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        if (!reader["DescrizioneI"].Equals(DBNull.Value))
                            item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        if (!reader["DENOMINAZIONEGB"].Equals(DBNull.Value))
                            item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        if (!reader["DENOMINAZIONEI"].Equals(DBNull.Value))
                            item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));


                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["linkVideo"].Equals(DBNull.Value))
                            item.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));

                        if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                            item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                        if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                            item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

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
                        if (!reader["PrezzoListino"].Equals(DBNull.Value))
                            item.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                        if (!reader["Vetrina"].Equals(DBNull.Value))
                            item.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));


                        if (!reader["Abilitacontatto"].Equals(DBNull.Value))
                            item.Abilitacontatto = reader.GetBoolean(reader.GetOrdinal("Abilitacontatto"));
                        if (!reader["Archiviato"].Equals(DBNull.Value))
                            item.Archiviato = reader.GetBoolean(reader.GetOrdinal("Archiviato"));
                        if (!reader["Qta_vendita"].Equals(DBNull.Value))
                            item.Qta_vendita = reader.GetDouble(reader.GetOrdinal("Qta_vendita"));
                        if (!reader["Promozione"].Equals(DBNull.Value))
                            item.Promozione = reader.GetBoolean(reader.GetOrdinal("Promozione"));


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

                        //CAMPI IN TABELLA COLLEGATA------------------------------------------------------------------
                        if (!reader["Id_dts_collegato"].Equals(DBNull.Value))
                            item.Id_dts_collegato = reader.GetInt64(reader.GetOrdinal("Id_dts_collegato"));

                        if (!reader["Pivacf_dts"].Equals(DBNull.Value))
                            item.Pivacf_dts = reader.GetString(reader.GetOrdinal("Pivacf_dts"));
                        if (!reader["Nome_dts"].Equals(DBNull.Value))
                            item.Nome_dts = reader.GetString(reader.GetOrdinal("Nome_dts"));
                        if (!reader["Cognome_dts"].Equals(DBNull.Value))
                            item.Cognome_dts = reader.GetString(reader.GetOrdinal("Cognome_dts"));
                        if (!reader["Datanascita_dts"].Equals(DBNull.Value))
                            item.Datanascita_dts = reader.GetDateTime(reader.GetOrdinal("Datanascita_dts"));
                        if (!reader["Sociopresentatore1_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore1_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore1_dts"));
                        if (!reader["Sociopresentatore2_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore2_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore2_dts"));
                        if (!reader["Telefonoprivato_dts"].Equals(DBNull.Value))
                            item.Telefonoprivato_dts = reader.GetString(reader.GetOrdinal("Telefonoprivato_dts"));
                        if (!reader["Annolaurea_dts"].Equals(DBNull.Value))
                            item.Annolaurea_dts = reader.GetString(reader.GetOrdinal("Annolaurea_dts"));
                        if (!reader["Annospecializzazione_dts"].Equals(DBNull.Value))
                            item.Annospecializzazione_dts = reader.GetString(reader.GetOrdinal("Annospecializzazione_dts"));
                        if (!reader["Altrespecializzazioni_dts"].Equals(DBNull.Value))
                            item.Altrespecializzazioni_dts = reader.GetString(reader.GetOrdinal("Altrespecializzazioni_dts"));
                        if (!reader["SocioSicpre_dts"].Equals(DBNull.Value))
                            item.SocioSicpre_dts = reader.GetBoolean(reader.GetOrdinal("SocioSicpre_dts"));
                        if (!reader["SocioIsaps_dts"].Equals(DBNull.Value))
                            item.SocioIsaps_dts = reader.GetBoolean(reader.GetOrdinal("SocioIsaps_dts"));
                        if (!reader["Socioaltraassociazione_dts"].Equals(DBNull.Value))
                            item.Socioaltraassociazione_dts = reader.GetString(reader.GetOrdinal("Socioaltraassociazione_dts"));
                        if (!reader["Trattamenticollegati_dts"].Equals(DBNull.Value))
                            item.Trattamenticollegati_dts = reader.GetString(reader.GetOrdinal("Trattamenticollegati_dts"));
                        if (!reader["AccettazioneStatuto_dts"].Equals(DBNull.Value))
                            item.AccettazioneStatuto_dts = reader.GetBoolean(reader.GetOrdinal("AccettazioneStatuto_dts"));

                        if (!reader["Certificazione_dts"].Equals(DBNull.Value))
                            item.Certificazione_dts = reader.GetBoolean(reader.GetOrdinal("Certificazione_dts"));
                        if (!reader["Emailriservata_dts"].Equals(DBNull.Value))
                            item.Emailriservata_dts = reader.GetString(reader.GetOrdinal("Emailriservata_dts"));

                        if (!reader["CodiceNAZIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE1_dts"));
                        if (!reader["CodiceREGIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE1_dts"));
                        if (!reader["CodicePROVINCIA1_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA1_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA1_dts"));
                        if (!reader["CodiceCOMUNE1_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE1_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE1_dts"));

                        if (!reader["CodiceNAZIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE2_dts"));
                        if (!reader["CodiceREGIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE2_dts"));
                        if (!reader["CodicePROVINCIA2_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA2_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA2_dts"));
                        if (!reader["CodiceCOMUNE2_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE2_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE2_dts"));

                        if (!reader["CodiceNAZIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE3_dts"));
                        if (!reader["CodiceREGIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE3_dts"));
                        if (!reader["CodicePROVINCIA3_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA3_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA3_dts"));
                        if (!reader["CodiceCOMUNE3_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE3_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE3_dts"));

                        if (!reader["Latitudine1_dts"].Equals(DBNull.Value))
                            item.Latitudine1_dts = reader.GetDouble(reader.GetOrdinal("Latitudine1_dts"));
                        if (!reader["Longitudine1_dts"].Equals(DBNull.Value))
                            item.Longitudine1_dts = reader.GetDouble(reader.GetOrdinal("Longitudine1_dts"));
                        if (!reader["Latitudine2_dts"].Equals(DBNull.Value))
                            item.Latitudine2_dts = reader.GetDouble(reader.GetOrdinal("Latitudine2_dts"));
                        if (!reader["Longitudine2_dts"].Equals(DBNull.Value))
                            item.Longitudine2_dts = reader.GetDouble(reader.GetOrdinal("Longitudine2_dts"));
                        if (!reader["Latitudine3_dts"].Equals(DBNull.Value))
                            item.Latitudine3_dts = reader.GetDouble(reader.GetOrdinal("Latitudine3_dts"));
                        if (!reader["Longitudine3_dts"].Equals(DBNull.Value))
                            item.Longitudine3_dts = reader.GetDouble(reader.GetOrdinal("Longitudine3_dts"));

                        if (!reader["Bloccoaccesso_dts"].Equals(DBNull.Value))
                            item.Bloccoaccesso_dts = reader.GetBoolean(reader.GetOrdinal("Bloccoaccesso_dts"));


                        if (!reader["Via1_dts"].Equals(DBNull.Value))
                            item.Via1_dts = reader.GetString(reader.GetOrdinal("Via1_dts"));
                        if (!reader["Cap1_dts"].Equals(DBNull.Value))
                            item.Cap1_dts = reader.GetString(reader.GetOrdinal("Cap1_dts"));
                        if (!reader["Nomeposizione1_dts"].Equals(DBNull.Value))
                            item.Nomeposizione1_dts = reader.GetString(reader.GetOrdinal("Nomeposizione1_dts"));
                        if (!reader["Telefono1_dts"].Equals(DBNull.Value))
                            item.Telefono1_dts = reader.GetString(reader.GetOrdinal("Telefono1_dts"));


                        if (!reader["Via2_dts"].Equals(DBNull.Value))
                            item.Via2_dts = reader.GetString(reader.GetOrdinal("Via2_dts"));
                        if (!reader["Cap2_dts"].Equals(DBNull.Value))
                            item.Cap2_dts = reader.GetString(reader.GetOrdinal("Cap2_dts"));
                        if (!reader["Nomeposizione2_dts"].Equals(DBNull.Value))
                            item.Nomeposizione2_dts = reader.GetString(reader.GetOrdinal("Nomeposizione2_dts"));
                        if (!reader["Telefono2_dts"].Equals(DBNull.Value))
                            item.Telefono2_dts = reader.GetString(reader.GetOrdinal("Telefono2_dts"));

                        if (!reader["Via3_dts"].Equals(DBNull.Value))
                            item.Via3_dts = reader.GetString(reader.GetOrdinal("Via3_dts"));
                        if (!reader["Cap3_dts"].Equals(DBNull.Value))
                            item.Cap3_dts = reader.GetString(reader.GetOrdinal("Cap3_dts"));
                        if (!reader["Nomeposizione3_dts"].Equals(DBNull.Value))
                            item.Nomeposizione3_dts = reader.GetString(reader.GetOrdinal("Nomeposizione3_dts"));
                        if (!reader["Telefono3_dts"].Equals(DBNull.Value))
                            item.Telefono3_dts = reader.GetString(reader.GetOrdinal("Telefono3_dts"));

                        if (!reader["Pagamenti_dts"].Equals(DBNull.Value))
                            item.Pagamenti_dts = reader.GetString(reader.GetOrdinal("Pagamenti_dts"));


                        if (!reader["ricfatt_dts"].Equals(DBNull.Value))
                            item.ricfatt_dts = reader.GetString(reader.GetOrdinal("ricfatt_dts"));

                        if (!reader["indirizzofatt_dts"].Equals(DBNull.Value))
                            item.indirizzofatt_dts = reader.GetString(reader.GetOrdinal("indirizzofatt_dts"));

                        if (!reader["noteriservate_dts"].Equals(DBNull.Value))
                            item.noteriservate_dts = reader.GetString(reader.GetOrdinal("noteriservate_dts"));

                        if (!reader["niscrordine_dts"].Equals(DBNull.Value))
                            item.niscrordine_dts = reader.GetString(reader.GetOrdinal("niscrordine_dts"));
                        if (!reader["locordine_dts"].Equals(DBNull.Value))
                            item.locordine_dts = reader.GetString(reader.GetOrdinal("locordine_dts"));
                        if (!reader["annofrequenza_dts"].Equals(DBNull.Value))
                            item.annofrequenza_dts = reader.GetString(reader.GetOrdinal("annofrequenza_dts"));
                        if (!reader["nomeuniversita_dts"].Equals(DBNull.Value))
                            item.nomeuniversita_dts = reader.GetString(reader.GetOrdinal("nomeuniversita_dts"));
                        if (!reader["dettagliuniversita_dts"].Equals(DBNull.Value))
                            item.dettagliuniversita_dts = reader.GetString(reader.GetOrdinal("dettagliuniversita_dts"));
                        if (!reader["Boolfields_dts"].Equals(DBNull.Value))
                            item.Boolfields_dts = reader.GetString(reader.GetOrdinal("Boolfields_dts"));
                        if (!reader["Textfield1_dts"].Equals(DBNull.Value))
                            item.Textfield1_dts = reader.GetString(reader.GetOrdinal("Textfield1_dts"));
                        if (!reader["Interventieseguiti_dts"].Equals(DBNull.Value))
                            item.Interventieseguiti_dts = reader.GetString(reader.GetOrdinal("Interventieseguiti_dts"));


                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento offerte :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Carica la lista completa ordinata per data di registrazione delle offerte
        /// in base al codice tipologia indicato
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codicetipologia"></param>
        /// <returns></returns>
        public OfferteCollection CaricaOffertePerCodice(string connection, string codicetipologia, string maxofferte = "", bool randomize = false, string LinguaFiltro = "", bool includiarchiviati = false, string campoordinamento = "", bool filtradisponibili = false)
        {
            if (connection == null || connection == "") return null;
            if (codicetipologia == null || codicetipologia == "") return null;
            OfferteCollection list = new OfferteCollection();
            Offerte item;
            try
            {
                string query = "";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                codicetipologia = codicetipologia.Trim().Replace("|", ",");

                if (!codicetipologia.Contains(","))
                {
                    query = "SELECT A.*,B.* FROM " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts where CodiceTIPOLOGIA=@CodiceTIPOLOGIA ";

                    SQLiteParameter p1 = new SQLiteParameter("@CodiceTIPOLOGIA", codicetipologia);//OleDbType.VarChar
                    parColl.Add(p1);
                }
                else
                {

                    query = "SELECT A.*,B.* FROM " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts  ";



                    string[] codici = codicetipologia.Split(',');
                    if (codici != null && codici.Length > 0)
                    {
                        if (!query.ToLower().Contains("where"))
                            query += " WHERE CodiceTIPOLOGIA in (    ";
                        else
                            query += " AND  CodiceTIPOLOGIA in (      ";
                        foreach (string codice in codici)
                        {
                            if (!string.IsNullOrEmpty(codice.Trim()))
                                query += " '" + codice + "' ,";
                        }
                        query = query.TrimEnd(',') + " ) ";
                    }
                }



                if (!String.IsNullOrEmpty(LinguaFiltro))
                {
                    switch (LinguaFiltro)
                    {
                        case "GB":
                            if (!query.ToLower().Contains("where"))
                                query += " WHERE (DenominazioneGB <> '' and DenominazioneGB is not null)  ";
                            else
                                query += " AND  (DenominazioneGB <> ''  and DenominazioneGB is not null)    ";
                            break;
                        case "RU":
                            if (!query.ToLower().Contains("where"))
                                query += " WHERE (DenominazioneRU <> '' and DenominazioneRU is not null)  ";
                            else
                                query += " AND  (DenominazioneRU <> ''  and DenominazioneRU is not null)    ";
                            break;
                    }
                }

                if (!includiarchiviati)
                {
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE (Archiviato = 0)  ";
                    else
                        query += " AND  (Archiviato = 0)    ";
                }

                if (filtradisponibili)
                {
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE (Qta_vendita > 0 or Qta_vendita is null)  ";
                    else
                        query += " AND (Qta_vendita > 0 or Qta_vendita is null)  ";
                }

                if (randomize)
                {
                    query += "ORDER BY random(), ID Desc";
                }
                else
                {

                    if (campoordinamento == "")
                        query += "  order BY DataInserimento Desc, Id Desc  ";
                    else
                        query += "  order BY " + campoordinamento + " COLLATE NOCASE Desc, Id Desc ";
                }

                if (!string.IsNullOrEmpty(maxofferte))
                    query = " limit " + maxofferte;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Offerte();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!reader["Id_collegato"].Equals(DBNull.Value))
                            item.Id_collegato = reader.GetInt64(reader.GetOrdinal("Id_collegato"));

                        if (!reader["Autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("Autore"));



                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Data1"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("Data1"));

                        if (!reader["DescrizioneGB"].Equals(DBNull.Value))
                            item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        if (!reader["DescrizioneI"].Equals(DBNull.Value))
                            item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        if (!reader["DENOMINAZIONEGB"].Equals(DBNull.Value))
                            item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        if (!reader["DENOMINAZIONEI"].Equals(DBNull.Value))
                            item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));

                        if (!reader["Campo1I"].Equals(DBNull.Value))
                            item.Campo1I = reader.GetString(reader.GetOrdinal("Campo1I"));
                        if (!reader["Campo1GB"].Equals(DBNull.Value))
                            item.Campo1GB = reader.GetString(reader.GetOrdinal("Campo1GB"));
                        if (!reader["Campo2I"].Equals(DBNull.Value))
                            item.Campo2I = reader.GetString(reader.GetOrdinal("Campo2I"));
                        if (!reader["Campo2GB"].Equals(DBNull.Value))
                            item.Campo2GB = reader.GetString(reader.GetOrdinal("Campo2GB"));

                        if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                            item.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
                        if (!reader["DescrizioneRU"].Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                            item.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                        if (!reader["Campo1RU"].Equals(DBNull.Value))
                            item.Campo1RU = reader.GetString(reader.GetOrdinal("Campo1RU"));
                        if (!reader["Campo2RU"].Equals(DBNull.Value))
                            item.Campo2RU = reader.GetString(reader.GetOrdinal("Campo2RU"));

                        if (!reader["XmlValue"].Equals(DBNull.Value))
                            item.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));


                        if (!reader["Caratteristica1"].Equals(DBNull.Value))
                            item.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                        if (!reader["Caratteristica2"].Equals(DBNull.Value))
                            item.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                        if (!reader["Caratteristica3"].Equals(DBNull.Value))
                            item.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                        if (!reader["Caratteristica4"].Equals(DBNull.Value))
                            item.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                        if (!reader["Caratteristica5"].Equals(DBNull.Value))
                            item.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                        if (!reader["Caratteristica6"].Equals(DBNull.Value))
                            item.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetInt64(reader.GetOrdinal("Anno"));


                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));

                        if (!reader["linkVideo"].Equals(DBNull.Value))
                            item.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));

                        if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                            item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                        if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                            item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

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
                        if (!reader["PrezzoListino"].Equals(DBNull.Value))
                            item.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                        if (!reader["Vetrina"].Equals(DBNull.Value))
                            item.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));


                        if (!reader["Abilitacontatto"].Equals(DBNull.Value))
                            item.Abilitacontatto = reader.GetBoolean(reader.GetOrdinal("Abilitacontatto"));
                        if (!reader["Archiviato"].Equals(DBNull.Value))
                            item.Archiviato = reader.GetBoolean(reader.GetOrdinal("Archiviato"));


                        if (!reader["Qta_vendita"].Equals(DBNull.Value))
                            item.Qta_vendita = reader.GetDouble(reader.GetOrdinal("Qta_vendita"));
                        if (!reader["Promozione"].Equals(DBNull.Value))
                            item.Promozione = reader.GetBoolean(reader.GetOrdinal("Promozione"));

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

                        //CAMPI IN TABELLA COLLEGATA------------------------------------------------------------------
                        if (!reader["Id_dts_collegato"].Equals(DBNull.Value))
                            item.Id_dts_collegato = reader.GetInt64(reader.GetOrdinal("Id_dts_collegato"));

                        if (!reader["Pivacf_dts"].Equals(DBNull.Value))
                            item.Pivacf_dts = reader.GetString(reader.GetOrdinal("Pivacf_dts"));
                        if (!reader["Nome_dts"].Equals(DBNull.Value))
                            item.Nome_dts = reader.GetString(reader.GetOrdinal("Nome_dts"));
                        if (!reader["Cognome_dts"].Equals(DBNull.Value))
                            item.Cognome_dts = reader.GetString(reader.GetOrdinal("Cognome_dts"));
                        if (!reader["Datanascita_dts"].Equals(DBNull.Value))
                            item.Datanascita_dts = reader.GetDateTime(reader.GetOrdinal("Datanascita_dts"));
                        if (!reader["Sociopresentatore1_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore1_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore1_dts"));
                        if (!reader["Sociopresentatore2_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore2_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore2_dts"));
                        if (!reader["Telefonoprivato_dts"].Equals(DBNull.Value))
                            item.Telefonoprivato_dts = reader.GetString(reader.GetOrdinal("Telefonoprivato_dts"));
                        if (!reader["Annolaurea_dts"].Equals(DBNull.Value))
                            item.Annolaurea_dts = reader.GetString(reader.GetOrdinal("Annolaurea_dts"));
                        if (!reader["Annospecializzazione_dts"].Equals(DBNull.Value))
                            item.Annospecializzazione_dts = reader.GetString(reader.GetOrdinal("Annospecializzazione_dts"));
                        if (!reader["Altrespecializzazioni_dts"].Equals(DBNull.Value))
                            item.Altrespecializzazioni_dts = reader.GetString(reader.GetOrdinal("Altrespecializzazioni_dts"));
                        if (!reader["SocioSicpre_dts"].Equals(DBNull.Value))
                            item.SocioSicpre_dts = reader.GetBoolean(reader.GetOrdinal("SocioSicpre_dts"));
                        if (!reader["SocioIsaps_dts"].Equals(DBNull.Value))
                            item.SocioIsaps_dts = reader.GetBoolean(reader.GetOrdinal("SocioIsaps_dts"));
                        if (!reader["Socioaltraassociazione_dts"].Equals(DBNull.Value))
                            item.Socioaltraassociazione_dts = reader.GetString(reader.GetOrdinal("Socioaltraassociazione_dts"));
                        if (!reader["Trattamenticollegati_dts"].Equals(DBNull.Value))
                            item.Trattamenticollegati_dts = reader.GetString(reader.GetOrdinal("Trattamenticollegati_dts"));
                        if (!reader["AccettazioneStatuto_dts"].Equals(DBNull.Value))
                            item.AccettazioneStatuto_dts = reader.GetBoolean(reader.GetOrdinal("AccettazioneStatuto_dts"));

                        if (!reader["Certificazione_dts"].Equals(DBNull.Value))
                            item.Certificazione_dts = reader.GetBoolean(reader.GetOrdinal("Certificazione_dts"));
                        if (!reader["Emailriservata_dts"].Equals(DBNull.Value))
                            item.Emailriservata_dts = reader.GetString(reader.GetOrdinal("Emailriservata_dts"));

                        if (!reader["CodiceNAZIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE1_dts"));
                        if (!reader["CodiceREGIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE1_dts"));
                        if (!reader["CodicePROVINCIA1_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA1_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA1_dts"));
                        if (!reader["CodiceCOMUNE1_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE1_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE1_dts"));

                        if (!reader["CodiceNAZIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE2_dts"));
                        if (!reader["CodiceREGIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE2_dts"));
                        if (!reader["CodicePROVINCIA2_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA2_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA2_dts"));
                        if (!reader["CodiceCOMUNE2_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE2_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE2_dts"));

                        if (!reader["CodiceNAZIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE3_dts"));
                        if (!reader["CodiceREGIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE3_dts"));
                        if (!reader["CodicePROVINCIA3_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA3_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA3_dts"));
                        if (!reader["CodiceCOMUNE3_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE3_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE3_dts"));

                        if (!reader["Latitudine1_dts"].Equals(DBNull.Value))
                            item.Latitudine1_dts = reader.GetDouble(reader.GetOrdinal("Latitudine1_dts"));
                        if (!reader["Longitudine1_dts"].Equals(DBNull.Value))
                            item.Longitudine1_dts = reader.GetDouble(reader.GetOrdinal("Longitudine1_dts"));
                        if (!reader["Latitudine2_dts"].Equals(DBNull.Value))
                            item.Latitudine2_dts = reader.GetDouble(reader.GetOrdinal("Latitudine2_dts"));
                        if (!reader["Longitudine2_dts"].Equals(DBNull.Value))
                            item.Longitudine2_dts = reader.GetDouble(reader.GetOrdinal("Longitudine2_dts"));
                        if (!reader["Latitudine3_dts"].Equals(DBNull.Value))
                            item.Latitudine3_dts = reader.GetDouble(reader.GetOrdinal("Latitudine3_dts"));
                        if (!reader["Longitudine3_dts"].Equals(DBNull.Value))
                            item.Longitudine3_dts = reader.GetDouble(reader.GetOrdinal("Longitudine3_dts"));

                        if (!reader["Bloccoaccesso_dts"].Equals(DBNull.Value))
                            item.Bloccoaccesso_dts = reader.GetBoolean(reader.GetOrdinal("Bloccoaccesso_dts"));


                        if (!reader["Via1_dts"].Equals(DBNull.Value))
                            item.Via1_dts = reader.GetString(reader.GetOrdinal("Via1_dts"));
                        if (!reader["Cap1_dts"].Equals(DBNull.Value))
                            item.Cap1_dts = reader.GetString(reader.GetOrdinal("Cap1_dts"));
                        if (!reader["Nomeposizione1_dts"].Equals(DBNull.Value))
                            item.Nomeposizione1_dts = reader.GetString(reader.GetOrdinal("Nomeposizione1_dts"));
                        if (!reader["Telefono1_dts"].Equals(DBNull.Value))
                            item.Telefono1_dts = reader.GetString(reader.GetOrdinal("Telefono1_dts"));


                        if (!reader["Via2_dts"].Equals(DBNull.Value))
                            item.Via2_dts = reader.GetString(reader.GetOrdinal("Via2_dts"));
                        if (!reader["Cap2_dts"].Equals(DBNull.Value))
                            item.Cap2_dts = reader.GetString(reader.GetOrdinal("Cap2_dts"));
                        if (!reader["Nomeposizione2_dts"].Equals(DBNull.Value))
                            item.Nomeposizione2_dts = reader.GetString(reader.GetOrdinal("Nomeposizione2_dts"));
                        if (!reader["Telefono2_dts"].Equals(DBNull.Value))
                            item.Telefono2_dts = reader.GetString(reader.GetOrdinal("Telefono2_dts"));

                        if (!reader["Via3_dts"].Equals(DBNull.Value))
                            item.Via3_dts = reader.GetString(reader.GetOrdinal("Via3_dts"));
                        if (!reader["Cap3_dts"].Equals(DBNull.Value))
                            item.Cap3_dts = reader.GetString(reader.GetOrdinal("Cap3_dts"));
                        if (!reader["Nomeposizione3_dts"].Equals(DBNull.Value))
                            item.Nomeposizione3_dts = reader.GetString(reader.GetOrdinal("Nomeposizione3_dts"));
                        if (!reader["Telefono3_dts"].Equals(DBNull.Value))
                            item.Telefono3_dts = reader.GetString(reader.GetOrdinal("Telefono3_dts"));

                        if (!reader["Pagamenti_dts"].Equals(DBNull.Value))
                            item.Pagamenti_dts = reader.GetString(reader.GetOrdinal("Pagamenti_dts"));


                        if (!reader["ricfatt_dts"].Equals(DBNull.Value))
                            item.ricfatt_dts = reader.GetString(reader.GetOrdinal("ricfatt_dts"));

                        if (!reader["indirizzofatt_dts"].Equals(DBNull.Value))
                            item.indirizzofatt_dts = reader.GetString(reader.GetOrdinal("indirizzofatt_dts"));

                        if (!reader["noteriservate_dts"].Equals(DBNull.Value))
                            item.noteriservate_dts = reader.GetString(reader.GetOrdinal("noteriservate_dts"));

                        if (!reader["niscrordine_dts"].Equals(DBNull.Value))
                            item.niscrordine_dts = reader.GetString(reader.GetOrdinal("niscrordine_dts"));
                        if (!reader["locordine_dts"].Equals(DBNull.Value))
                            item.locordine_dts = reader.GetString(reader.GetOrdinal("locordine_dts"));
                        if (!reader["annofrequenza_dts"].Equals(DBNull.Value))
                            item.annofrequenza_dts = reader.GetString(reader.GetOrdinal("annofrequenza_dts"));
                        if (!reader["nomeuniversita_dts"].Equals(DBNull.Value))
                            item.nomeuniversita_dts = reader.GetString(reader.GetOrdinal("nomeuniversita_dts"));
                        if (!reader["dettagliuniversita_dts"].Equals(DBNull.Value))
                            item.dettagliuniversita_dts = reader.GetString(reader.GetOrdinal("dettagliuniversita_dts"));
                        if (!reader["Boolfields_dts"].Equals(DBNull.Value))
                            item.Boolfields_dts = reader.GetString(reader.GetOrdinal("Boolfields_dts"));
                        if (!reader["Textfield1_dts"].Equals(DBNull.Value))
                            item.Textfield1_dts = reader.GetString(reader.GetOrdinal("Textfield1_dts"));
                        if (!reader["Interventieseguiti_dts"].Equals(DBNull.Value))
                            item.Interventieseguiti_dts = reader.GetString(reader.GetOrdinal("Interventieseguiti_dts"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento offerte :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Ricarica un'offerta specifica in base al codice prodotto univoco dell'articolo ( si suppone che per ogni codice ci sia un solo prodotto in tabella )
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idOfferta"></param>
        /// <returns></returns>
        public Offerte CaricaOffertaPerCodiceProdotto(string connection, string codiceProdotto)
        {
            if (connection == null || connection == "") return null;
            if (codiceProdotto == null || codiceProdotto == "") return null;
            Offerte item = null;

            try
            {
                string query = "SELECT  A.*,B.* FROM " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts  where CodiceProdotto=@CodiceProdotto order BY DataInserimento Desc";

                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@CodiceProdotto", codiceProdotto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Offerte();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!reader["Id_collegato"].Equals(DBNull.Value))
                            item.Id_collegato = reader.GetInt64(reader.GetOrdinal("Id_collegato"));

                        if (!reader["Autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("Autore"));



                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Data1"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("Data1"));
                        if (!reader["DescrizioneGB"].Equals(DBNull.Value))
                            item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        if (!reader["DescrizioneI"].Equals(DBNull.Value))
                            item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        if (!reader["DENOMINAZIONEGB"].Equals(DBNull.Value))
                            item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        if (!reader["DENOMINAZIONEI"].Equals(DBNull.Value))
                            item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));

                        if (!reader["Campo1I"].Equals(DBNull.Value))
                            item.Campo1I = reader.GetString(reader.GetOrdinal("Campo1I"));
                        if (!reader["Campo1GB"].Equals(DBNull.Value))
                            item.Campo1GB = reader.GetString(reader.GetOrdinal("Campo1GB"));
                        if (!reader["Campo2I"].Equals(DBNull.Value))
                            item.Campo2I = reader.GetString(reader.GetOrdinal("Campo2I"));
                        if (!reader["Campo2GB"].Equals(DBNull.Value))
                            item.Campo2GB = reader.GetString(reader.GetOrdinal("Campo2GB"));

                        if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                            item.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
                        if (!reader["DescrizioneRU"].Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                            item.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                        if (!reader["Campo1RU"].Equals(DBNull.Value))
                            item.Campo1RU = reader.GetString(reader.GetOrdinal("Campo1RU"));
                        if (!reader["Campo2RU"].Equals(DBNull.Value))
                            item.Campo2RU = reader.GetString(reader.GetOrdinal("Campo2RU"));

                        if (!reader["XmlValue"].Equals(DBNull.Value))
                            item.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));


                        if (!reader["Caratteristica1"].Equals(DBNull.Value))
                            item.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                        if (!reader["Caratteristica2"].Equals(DBNull.Value))
                            item.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                        if (!reader["Caratteristica3"].Equals(DBNull.Value))
                            item.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                        if (!reader["Caratteristica4"].Equals(DBNull.Value))
                            item.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                        if (!reader["Caratteristica5"].Equals(DBNull.Value))
                            item.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                        if (!reader["Caratteristica6"].Equals(DBNull.Value))
                            item.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetInt64(reader.GetOrdinal("Anno"));


                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["linkVideo"].Equals(DBNull.Value))
                            item.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                            item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                        if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                            item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

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
                        if (!reader["PrezzoListino"].Equals(DBNull.Value))
                            item.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                        if (!reader["Vetrina"].Equals(DBNull.Value))
                            item.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));
                        if (!reader["Abilitacontatto"].Equals(DBNull.Value))
                            item.Abilitacontatto = reader.GetBoolean(reader.GetOrdinal("Abilitacontatto"));
                        if (!reader["Archiviato"].Equals(DBNull.Value))
                            item.Archiviato = reader.GetBoolean(reader.GetOrdinal("Archiviato"));


                        if (!reader["Qta_vendita"].Equals(DBNull.Value))
                            item.Qta_vendita = reader.GetDouble(reader.GetOrdinal("Qta_vendita"));
                        if (!reader["Promozione"].Equals(DBNull.Value))
                            item.Promozione = reader.GetBoolean(reader.GetOrdinal("Promozione"));



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

                        //CAMPI IN TABELLA COLLEGATA------------------------------------------------------------------
                        if (!reader["Id_dts_collegato"].Equals(DBNull.Value))
                            item.Id_dts_collegato = reader.GetInt64(reader.GetOrdinal("Id_dts_collegato"));

                        if (!reader["Pivacf_dts"].Equals(DBNull.Value))
                            item.Pivacf_dts = reader.GetString(reader.GetOrdinal("Pivacf_dts"));
                        if (!reader["Nome_dts"].Equals(DBNull.Value))
                            item.Nome_dts = reader.GetString(reader.GetOrdinal("Nome_dts"));
                        if (!reader["Cognome_dts"].Equals(DBNull.Value))
                            item.Cognome_dts = reader.GetString(reader.GetOrdinal("Cognome_dts"));
                        if (!reader["Datanascita_dts"].Equals(DBNull.Value))
                            item.Datanascita_dts = reader.GetDateTime(reader.GetOrdinal("Datanascita_dts"));
                        if (!reader["Sociopresentatore1_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore1_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore1_dts"));
                        if (!reader["Sociopresentatore2_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore2_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore2_dts"));
                        if (!reader["Telefonoprivato_dts"].Equals(DBNull.Value))
                            item.Telefonoprivato_dts = reader.GetString(reader.GetOrdinal("Telefonoprivato_dts"));
                        if (!reader["Annolaurea_dts"].Equals(DBNull.Value))
                            item.Annolaurea_dts = reader.GetString(reader.GetOrdinal("Annolaurea_dts"));
                        if (!reader["Annospecializzazione_dts"].Equals(DBNull.Value))
                            item.Annospecializzazione_dts = reader.GetString(reader.GetOrdinal("Annospecializzazione_dts"));
                        if (!reader["Altrespecializzazioni_dts"].Equals(DBNull.Value))
                            item.Altrespecializzazioni_dts = reader.GetString(reader.GetOrdinal("Altrespecializzazioni_dts"));
                        if (!reader["SocioSicpre_dts"].Equals(DBNull.Value))
                            item.SocioSicpre_dts = reader.GetBoolean(reader.GetOrdinal("SocioSicpre_dts"));
                        if (!reader["SocioIsaps_dts"].Equals(DBNull.Value))
                            item.SocioIsaps_dts = reader.GetBoolean(reader.GetOrdinal("SocioIsaps_dts"));
                        if (!reader["Socioaltraassociazione_dts"].Equals(DBNull.Value))
                            item.Socioaltraassociazione_dts = reader.GetString(reader.GetOrdinal("Socioaltraassociazione_dts"));
                        if (!reader["Trattamenticollegati_dts"].Equals(DBNull.Value))
                            item.Trattamenticollegati_dts = reader.GetString(reader.GetOrdinal("Trattamenticollegati_dts"));
                        if (!reader["AccettazioneStatuto_dts"].Equals(DBNull.Value))
                            item.AccettazioneStatuto_dts = reader.GetBoolean(reader.GetOrdinal("AccettazioneStatuto_dts"));

                        if (!reader["Certificazione_dts"].Equals(DBNull.Value))
                            item.Certificazione_dts = reader.GetBoolean(reader.GetOrdinal("Certificazione_dts"));
                        if (!reader["Emailriservata_dts"].Equals(DBNull.Value))
                            item.Emailriservata_dts = reader.GetString(reader.GetOrdinal("Emailriservata_dts"));

                        if (!reader["CodiceNAZIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE1_dts"));
                        if (!reader["CodiceREGIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE1_dts"));
                        if (!reader["CodicePROVINCIA1_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA1_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA1_dts"));
                        if (!reader["CodiceCOMUNE1_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE1_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE1_dts"));

                        if (!reader["CodiceNAZIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE2_dts"));
                        if (!reader["CodiceREGIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE2_dts"));
                        if (!reader["CodicePROVINCIA2_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA2_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA2_dts"));
                        if (!reader["CodiceCOMUNE2_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE2_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE2_dts"));

                        if (!reader["CodiceNAZIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE3_dts"));
                        if (!reader["CodiceREGIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE3_dts"));
                        if (!reader["CodicePROVINCIA3_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA3_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA3_dts"));
                        if (!reader["CodiceCOMUNE3_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE3_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE3_dts"));

                        if (!reader["Latitudine1_dts"].Equals(DBNull.Value))
                            item.Latitudine1_dts = reader.GetDouble(reader.GetOrdinal("Latitudine1_dts"));
                        if (!reader["Longitudine1_dts"].Equals(DBNull.Value))
                            item.Longitudine1_dts = reader.GetDouble(reader.GetOrdinal("Longitudine1_dts"));
                        if (!reader["Latitudine2_dts"].Equals(DBNull.Value))
                            item.Latitudine2_dts = reader.GetDouble(reader.GetOrdinal("Latitudine2_dts"));
                        if (!reader["Longitudine2_dts"].Equals(DBNull.Value))
                            item.Longitudine2_dts = reader.GetDouble(reader.GetOrdinal("Longitudine2_dts"));
                        if (!reader["Latitudine3_dts"].Equals(DBNull.Value))
                            item.Latitudine3_dts = reader.GetDouble(reader.GetOrdinal("Latitudine3_dts"));
                        if (!reader["Longitudine3_dts"].Equals(DBNull.Value))
                            item.Longitudine3_dts = reader.GetDouble(reader.GetOrdinal("Longitudine3_dts"));

                        if (!reader["Bloccoaccesso_dts"].Equals(DBNull.Value))
                            item.Bloccoaccesso_dts = reader.GetBoolean(reader.GetOrdinal("Bloccoaccesso_dts"));


                        if (!reader["Via1_dts"].Equals(DBNull.Value))
                            item.Via1_dts = reader.GetString(reader.GetOrdinal("Via1_dts"));
                        if (!reader["Cap1_dts"].Equals(DBNull.Value))
                            item.Cap1_dts = reader.GetString(reader.GetOrdinal("Cap1_dts"));
                        if (!reader["Nomeposizione1_dts"].Equals(DBNull.Value))
                            item.Nomeposizione1_dts = reader.GetString(reader.GetOrdinal("Nomeposizione1_dts"));
                        if (!reader["Telefono1_dts"].Equals(DBNull.Value))
                            item.Telefono1_dts = reader.GetString(reader.GetOrdinal("Telefono1_dts"));


                        if (!reader["Via2_dts"].Equals(DBNull.Value))
                            item.Via2_dts = reader.GetString(reader.GetOrdinal("Via2_dts"));
                        if (!reader["Cap2_dts"].Equals(DBNull.Value))
                            item.Cap2_dts = reader.GetString(reader.GetOrdinal("Cap2_dts"));
                        if (!reader["Nomeposizione2_dts"].Equals(DBNull.Value))
                            item.Nomeposizione2_dts = reader.GetString(reader.GetOrdinal("Nomeposizione2_dts"));
                        if (!reader["Telefono2_dts"].Equals(DBNull.Value))
                            item.Telefono2_dts = reader.GetString(reader.GetOrdinal("Telefono2_dts"));

                        if (!reader["Via3_dts"].Equals(DBNull.Value))
                            item.Via3_dts = reader.GetString(reader.GetOrdinal("Via3_dts"));
                        if (!reader["Cap3_dts"].Equals(DBNull.Value))
                            item.Cap3_dts = reader.GetString(reader.GetOrdinal("Cap3_dts"));
                        if (!reader["Nomeposizione3_dts"].Equals(DBNull.Value))
                            item.Nomeposizione3_dts = reader.GetString(reader.GetOrdinal("Nomeposizione3_dts"));
                        if (!reader["Telefono3_dts"].Equals(DBNull.Value))
                            item.Telefono3_dts = reader.GetString(reader.GetOrdinal("Telefono3_dts"));

                        if (!reader["Pagamenti_dts"].Equals(DBNull.Value))
                            item.Pagamenti_dts = reader.GetString(reader.GetOrdinal("Pagamenti_dts"));


                        if (!reader["ricfatt_dts"].Equals(DBNull.Value))
                            item.ricfatt_dts = reader.GetString(reader.GetOrdinal("ricfatt_dts"));

                        if (!reader["indirizzofatt_dts"].Equals(DBNull.Value))
                            item.indirizzofatt_dts = reader.GetString(reader.GetOrdinal("indirizzofatt_dts"));

                        if (!reader["noteriservate_dts"].Equals(DBNull.Value))
                            item.noteriservate_dts = reader.GetString(reader.GetOrdinal("noteriservate_dts"));

                        if (!reader["niscrordine_dts"].Equals(DBNull.Value))
                            item.niscrordine_dts = reader.GetString(reader.GetOrdinal("niscrordine_dts"));
                        if (!reader["locordine_dts"].Equals(DBNull.Value))
                            item.locordine_dts = reader.GetString(reader.GetOrdinal("locordine_dts"));
                        if (!reader["annofrequenza_dts"].Equals(DBNull.Value))
                            item.annofrequenza_dts = reader.GetString(reader.GetOrdinal("annofrequenza_dts"));
                        if (!reader["nomeuniversita_dts"].Equals(DBNull.Value))
                            item.nomeuniversita_dts = reader.GetString(reader.GetOrdinal("nomeuniversita_dts"));
                        if (!reader["dettagliuniversita_dts"].Equals(DBNull.Value))
                            item.dettagliuniversita_dts = reader.GetString(reader.GetOrdinal("dettagliuniversita_dts"));
                        if (!reader["Boolfields_dts"].Equals(DBNull.Value))
                            item.Boolfields_dts = reader.GetString(reader.GetOrdinal("Boolfields_dts"));
                        if (!reader["Textfield1_dts"].Equals(DBNull.Value))
                            item.Textfield1_dts = reader.GetString(reader.GetOrdinal("Textfield1_dts"));
                        if (!reader["Interventieseguiti_dts"].Equals(DBNull.Value))
                            item.Interventieseguiti_dts = reader.GetString(reader.GetOrdinal("Interventieseguiti_dts"));

                        return (item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento offerta :" + error.Message, error);
            }

            return item;
        }

        /// <summary>
        /// Ricarica un'offerta specifica in base all'id
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idOfferta"></param>
        /// <returns></returns>
        public Offerte CaricaOffertaPerId(string connection, string idOfferta)
        {
            if (connection == null || connection == "") return null;
            if (idOfferta == null || idOfferta == "") return null;
            OfferteCollection list = new OfferteCollection();
            Offerte item = null;

            try
            {

                string query = "SELECT  A.*,B.* FROM " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts where ID=@ID order BY DataInserimento Desc";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@ID", idOfferta);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Offerte();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!reader["Id_collegato"].Equals(DBNull.Value))
                            item.Id_collegato = reader.GetInt64(reader.GetOrdinal("Id_collegato"));

                        if (!reader["Autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("Autore"));


                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Data1"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("Data1"));
                        if (!reader["DescrizioneGB"].Equals(DBNull.Value))
                            item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        if (!reader["DescrizioneI"].Equals(DBNull.Value))
                            item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        if (!reader["DENOMINAZIONEGB"].Equals(DBNull.Value))
                            item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        if (!reader["DENOMINAZIONEI"].Equals(DBNull.Value))
                            item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));

                        if (!reader["Campo1I"].Equals(DBNull.Value))
                            item.Campo1I = reader.GetString(reader.GetOrdinal("Campo1I"));
                        if (!reader["Campo1GB"].Equals(DBNull.Value))
                            item.Campo1GB = reader.GetString(reader.GetOrdinal("Campo1GB"));
                        if (!reader["Campo2I"].Equals(DBNull.Value))
                            item.Campo2I = reader.GetString(reader.GetOrdinal("Campo2I"));
                        if (!reader["Campo2GB"].Equals(DBNull.Value))
                            item.Campo2GB = reader.GetString(reader.GetOrdinal("Campo2GB"));

                        if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                            item.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
                        if (!reader["DescrizioneRU"].Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                            item.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                        if (!reader["Campo1RU"].Equals(DBNull.Value))
                            item.Campo1RU = reader.GetString(reader.GetOrdinal("Campo1RU"));
                        if (!reader["Campo2RU"].Equals(DBNull.Value))
                            item.Campo2RU = reader.GetString(reader.GetOrdinal("Campo2RU"));

                        if (!reader["XmlValue"].Equals(DBNull.Value))
                            item.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));


                        if (!reader["Caratteristica1"].Equals(DBNull.Value))
                            item.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                        if (!reader["Caratteristica2"].Equals(DBNull.Value))
                            item.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                        if (!reader["Caratteristica3"].Equals(DBNull.Value))
                            item.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                        if (!reader["Caratteristica4"].Equals(DBNull.Value))
                            item.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                        if (!reader["Caratteristica5"].Equals(DBNull.Value))
                            item.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                        if (!reader["Caratteristica6"].Equals(DBNull.Value))
                            item.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetInt64(reader.GetOrdinal("Anno"));


                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["linkVideo"].Equals(DBNull.Value))
                            item.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                            item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                        if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                            item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

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
                        if (!reader["PrezzoListino"].Equals(DBNull.Value))
                            item.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                        if (!reader["Vetrina"].Equals(DBNull.Value))
                            item.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));
                        if (!reader["Abilitacontatto"].Equals(DBNull.Value))
                            item.Abilitacontatto = reader.GetBoolean(reader.GetOrdinal("Abilitacontatto"));
                        if (!reader["Archiviato"].Equals(DBNull.Value))
                            item.Archiviato = reader.GetBoolean(reader.GetOrdinal("Archiviato"));
                        if (!reader["Qta_vendita"].Equals(DBNull.Value))
                            item.Qta_vendita = reader.GetDouble(reader.GetOrdinal("Qta_vendita"));
                        if (!reader["Promozione"].Equals(DBNull.Value))
                            item.Promozione = reader.GetBoolean(reader.GetOrdinal("Promozione"));


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

                        //CAMPI IN TABELLA COLLEGATA------------------------------------------------------------------
                        if (!reader["Id_dts_collegato"].Equals(DBNull.Value))
                            item.Id_dts_collegato = reader.GetInt64(reader.GetOrdinal("Id_dts_collegato"));

                        if (!reader["Pivacf_dts"].Equals(DBNull.Value))
                            item.Pivacf_dts = reader.GetString(reader.GetOrdinal("Pivacf_dts"));
                        if (!reader["Nome_dts"].Equals(DBNull.Value))
                            item.Nome_dts = reader.GetString(reader.GetOrdinal("Nome_dts"));
                        if (!reader["Cognome_dts"].Equals(DBNull.Value))
                            item.Cognome_dts = reader.GetString(reader.GetOrdinal("Cognome_dts"));
                        if (!reader["Datanascita_dts"].Equals(DBNull.Value))
                            item.Datanascita_dts = reader.GetDateTime(reader.GetOrdinal("Datanascita_dts"));
                        if (!reader["Sociopresentatore1_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore1_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore1_dts"));
                        if (!reader["Sociopresentatore2_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore2_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore2_dts"));
                        if (!reader["Telefonoprivato_dts"].Equals(DBNull.Value))
                            item.Telefonoprivato_dts = reader.GetString(reader.GetOrdinal("Telefonoprivato_dts"));
                        if (!reader["Annolaurea_dts"].Equals(DBNull.Value))
                            item.Annolaurea_dts = reader.GetString(reader.GetOrdinal("Annolaurea_dts"));
                        if (!reader["Annospecializzazione_dts"].Equals(DBNull.Value))
                            item.Annospecializzazione_dts = reader.GetString(reader.GetOrdinal("Annospecializzazione_dts"));
                        if (!reader["Altrespecializzazioni_dts"].Equals(DBNull.Value))
                            item.Altrespecializzazioni_dts = reader.GetString(reader.GetOrdinal("Altrespecializzazioni_dts"));
                        if (!reader["SocioSicpre_dts"].Equals(DBNull.Value))
                            item.SocioSicpre_dts = reader.GetBoolean(reader.GetOrdinal("SocioSicpre_dts"));
                        if (!reader["SocioIsaps_dts"].Equals(DBNull.Value))
                            item.SocioIsaps_dts = reader.GetBoolean(reader.GetOrdinal("SocioIsaps_dts"));
                        if (!reader["Socioaltraassociazione_dts"].Equals(DBNull.Value))
                            item.Socioaltraassociazione_dts = reader.GetString(reader.GetOrdinal("Socioaltraassociazione_dts"));
                        if (!reader["Trattamenticollegati_dts"].Equals(DBNull.Value))
                            item.Trattamenticollegati_dts = reader.GetString(reader.GetOrdinal("Trattamenticollegati_dts"));
                        if (!reader["AccettazioneStatuto_dts"].Equals(DBNull.Value))
                            item.AccettazioneStatuto_dts = reader.GetBoolean(reader.GetOrdinal("AccettazioneStatuto_dts"));

                        if (!reader["Certificazione_dts"].Equals(DBNull.Value))
                            item.Certificazione_dts = reader.GetBoolean(reader.GetOrdinal("Certificazione_dts"));
                        if (!reader["Emailriservata_dts"].Equals(DBNull.Value))
                            item.Emailriservata_dts = reader.GetString(reader.GetOrdinal("Emailriservata_dts"));

                        if (!reader["CodiceNAZIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE1_dts"));
                        if (!reader["CodiceREGIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE1_dts"));
                        if (!reader["CodicePROVINCIA1_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA1_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA1_dts"));
                        if (!reader["CodiceCOMUNE1_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE1_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE1_dts"));

                        if (!reader["CodiceNAZIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE2_dts"));
                        if (!reader["CodiceREGIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE2_dts"));
                        if (!reader["CodicePROVINCIA2_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA2_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA2_dts"));
                        if (!reader["CodiceCOMUNE2_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE2_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE2_dts"));

                        if (!reader["CodiceNAZIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE3_dts"));
                        if (!reader["CodiceREGIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE3_dts"));
                        if (!reader["CodicePROVINCIA3_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA3_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA3_dts"));
                        if (!reader["CodiceCOMUNE3_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE3_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE3_dts"));

                        if (!reader["Latitudine1_dts"].Equals(DBNull.Value))
                            item.Latitudine1_dts = reader.GetDouble(reader.GetOrdinal("Latitudine1_dts"));
                        if (!reader["Longitudine1_dts"].Equals(DBNull.Value))
                            item.Longitudine1_dts = reader.GetDouble(reader.GetOrdinal("Longitudine1_dts"));
                        if (!reader["Latitudine2_dts"].Equals(DBNull.Value))
                            item.Latitudine2_dts = reader.GetDouble(reader.GetOrdinal("Latitudine2_dts"));
                        if (!reader["Longitudine2_dts"].Equals(DBNull.Value))
                            item.Longitudine2_dts = reader.GetDouble(reader.GetOrdinal("Longitudine2_dts"));
                        if (!reader["Latitudine3_dts"].Equals(DBNull.Value))
                            item.Latitudine3_dts = reader.GetDouble(reader.GetOrdinal("Latitudine3_dts"));
                        if (!reader["Longitudine3_dts"].Equals(DBNull.Value))
                            item.Longitudine3_dts = reader.GetDouble(reader.GetOrdinal("Longitudine3_dts"));

                        if (!reader["Bloccoaccesso_dts"].Equals(DBNull.Value))
                            item.Bloccoaccesso_dts = reader.GetBoolean(reader.GetOrdinal("Bloccoaccesso_dts"));


                        if (!reader["Via1_dts"].Equals(DBNull.Value))
                            item.Via1_dts = reader.GetString(reader.GetOrdinal("Via1_dts"));
                        if (!reader["Cap1_dts"].Equals(DBNull.Value))
                            item.Cap1_dts = reader.GetString(reader.GetOrdinal("Cap1_dts"));
                        if (!reader["Nomeposizione1_dts"].Equals(DBNull.Value))
                            item.Nomeposizione1_dts = reader.GetString(reader.GetOrdinal("Nomeposizione1_dts"));
                        if (!reader["Telefono1_dts"].Equals(DBNull.Value))
                            item.Telefono1_dts = reader.GetString(reader.GetOrdinal("Telefono1_dts"));


                        if (!reader["Via2_dts"].Equals(DBNull.Value))
                            item.Via2_dts = reader.GetString(reader.GetOrdinal("Via2_dts"));
                        if (!reader["Cap2_dts"].Equals(DBNull.Value))
                            item.Cap2_dts = reader.GetString(reader.GetOrdinal("Cap2_dts"));
                        if (!reader["Nomeposizione2_dts"].Equals(DBNull.Value))
                            item.Nomeposizione2_dts = reader.GetString(reader.GetOrdinal("Nomeposizione2_dts"));
                        if (!reader["Telefono2_dts"].Equals(DBNull.Value))
                            item.Telefono2_dts = reader.GetString(reader.GetOrdinal("Telefono2_dts"));

                        if (!reader["Via3_dts"].Equals(DBNull.Value))
                            item.Via3_dts = reader.GetString(reader.GetOrdinal("Via3_dts"));
                        if (!reader["Cap3_dts"].Equals(DBNull.Value))
                            item.Cap3_dts = reader.GetString(reader.GetOrdinal("Cap3_dts"));
                        if (!reader["Nomeposizione3_dts"].Equals(DBNull.Value))
                            item.Nomeposizione3_dts = reader.GetString(reader.GetOrdinal("Nomeposizione3_dts"));
                        if (!reader["Telefono3_dts"].Equals(DBNull.Value))
                            item.Telefono3_dts = reader.GetString(reader.GetOrdinal("Telefono3_dts"));

                        if (!reader["Pagamenti_dts"].Equals(DBNull.Value))
                            item.Pagamenti_dts = reader.GetString(reader.GetOrdinal("Pagamenti_dts"));


                        if (!reader["ricfatt_dts"].Equals(DBNull.Value))
                            item.ricfatt_dts = reader.GetString(reader.GetOrdinal("ricfatt_dts"));

                        if (!reader["indirizzofatt_dts"].Equals(DBNull.Value))
                            item.indirizzofatt_dts = reader.GetString(reader.GetOrdinal("indirizzofatt_dts"));

                        if (!reader["noteriservate_dts"].Equals(DBNull.Value))
                            item.noteriservate_dts = reader.GetString(reader.GetOrdinal("noteriservate_dts"));

                        if (!reader["niscrordine_dts"].Equals(DBNull.Value))
                            item.niscrordine_dts = reader.GetString(reader.GetOrdinal("niscrordine_dts"));
                        if (!reader["locordine_dts"].Equals(DBNull.Value))
                            item.locordine_dts = reader.GetString(reader.GetOrdinal("locordine_dts"));
                        if (!reader["annofrequenza_dts"].Equals(DBNull.Value))
                            item.annofrequenza_dts = reader.GetString(reader.GetOrdinal("annofrequenza_dts"));
                        if (!reader["nomeuniversita_dts"].Equals(DBNull.Value))
                            item.nomeuniversita_dts = reader.GetString(reader.GetOrdinal("nomeuniversita_dts"));
                        if (!reader["dettagliuniversita_dts"].Equals(DBNull.Value))
                            item.dettagliuniversita_dts = reader.GetString(reader.GetOrdinal("dettagliuniversita_dts"));
                        if (!reader["Boolfields_dts"].Equals(DBNull.Value))
                            item.Boolfields_dts = reader.GetString(reader.GetOrdinal("Boolfields_dts"));
                        if (!reader["Textfield1_dts"].Equals(DBNull.Value))
                            item.Textfield1_dts = reader.GetString(reader.GetOrdinal("Textfield1_dts"));
                        if (!reader["Interventieseguiti_dts"].Equals(DBNull.Value))
                            item.Interventieseguiti_dts = reader.GetString(reader.GetOrdinal("Interventieseguiti_dts"));


                        return (item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento offerta :" + error.Message, error);
            }

            return item;
        }


        /// <summary>
        /// Ricarica un'offerta specifica in base all'id
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idOfferta"></param>
        /// <returns></returns>
        public Offerte CaricaOffertaPerTestourl(string connection, string testoricerca)
        {
            if (connection == null || connection == "") return null;
            if (testoricerca == null || testoricerca == "") return null;
            OfferteCollection list = new OfferteCollection();
            Offerte item = null;

            try
            {
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@Titolo", "%" + testoricerca + "%");//OleDbType.VarChar
                parColl.Add(p1);
                string query = "SELECT  A.*,B.* FROM " + Tblarchivio + " A left join " + _tblarchiviodettaglio + " B on A.id_dts_collegato=B.Id_dts where ( DENOMINAZIONEI like @Titolo or DENOMINAZIONEGB like @Titolo or DENOMINAZIONERU like @Titolo ) order BY DataInserimento Desc";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Offerte();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!reader["Id_collegato"].Equals(DBNull.Value))
                            item.Id_collegato = reader.GetInt64(reader.GetOrdinal("Id_collegato"));

                        if (!reader["Autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("Autore"));


                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Data1"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("Data1"));
                        if (!reader["DescrizioneGB"].Equals(DBNull.Value))
                            item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        if (!reader["DescrizioneI"].Equals(DBNull.Value))
                            item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        if (!reader["DENOMINAZIONEGB"].Equals(DBNull.Value))
                            item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        if (!reader["DENOMINAZIONEI"].Equals(DBNull.Value))
                            item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));

                        if (!reader["Campo1I"].Equals(DBNull.Value))
                            item.Campo1I = reader.GetString(reader.GetOrdinal("Campo1I"));
                        if (!reader["Campo1GB"].Equals(DBNull.Value))
                            item.Campo1GB = reader.GetString(reader.GetOrdinal("Campo1GB"));
                        if (!reader["Campo2I"].Equals(DBNull.Value))
                            item.Campo2I = reader.GetString(reader.GetOrdinal("Campo2I"));
                        if (!reader["Campo2GB"].Equals(DBNull.Value))
                            item.Campo2GB = reader.GetString(reader.GetOrdinal("Campo2GB"));


                        if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                            item.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
                        if (!reader["DescrizioneRU"].Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                            item.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                        if (!reader["Campo1RU"].Equals(DBNull.Value))
                            item.Campo1RU = reader.GetString(reader.GetOrdinal("Campo1RU"));
                        if (!reader["Campo2RU"].Equals(DBNull.Value))
                            item.Campo2RU = reader.GetString(reader.GetOrdinal("Campo2RU"));



                        if (!reader["XmlValue"].Equals(DBNull.Value))
                            item.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));


                        if (!reader["Caratteristica1"].Equals(DBNull.Value))
                            item.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                        if (!reader["Caratteristica2"].Equals(DBNull.Value))
                            item.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                        if (!reader["Caratteristica3"].Equals(DBNull.Value))
                            item.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                        if (!reader["Caratteristica4"].Equals(DBNull.Value))
                            item.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                        if (!reader["Caratteristica5"].Equals(DBNull.Value))
                            item.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                        if (!reader["Caratteristica6"].Equals(DBNull.Value))
                            item.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetInt64(reader.GetOrdinal("Anno"));


                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["linkVideo"].Equals(DBNull.Value))
                            item.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));
                        if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                            item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                        if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                            item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

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
                        if (!reader["PrezzoListino"].Equals(DBNull.Value))
                            item.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                        if (!reader["Vetrina"].Equals(DBNull.Value))
                            item.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));
                        if (!reader["Abilitacontatto"].Equals(DBNull.Value))
                            item.Abilitacontatto = reader.GetBoolean(reader.GetOrdinal("Abilitacontatto"));
                        if (!reader["Archiviato"].Equals(DBNull.Value))
                            item.Archiviato = reader.GetBoolean(reader.GetOrdinal("Archiviato"));
                        if (!reader["Qta_vendita"].Equals(DBNull.Value))
                            item.Qta_vendita = reader.GetDouble(reader.GetOrdinal("Qta_vendita"));
                        if (!reader["Promozione"].Equals(DBNull.Value))
                            item.Promozione = reader.GetBoolean(reader.GetOrdinal("Promozione"));


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

                        //CAMPI IN TABELLA COLLEGATA------------------------------------------------------------------
                        if (!reader["Id_dts_collegato"].Equals(DBNull.Value))
                            item.Id_dts_collegato = reader.GetInt64(reader.GetOrdinal("Id_dts_collegato"));

                        if (!reader["Pivacf_dts"].Equals(DBNull.Value))
                            item.Pivacf_dts = reader.GetString(reader.GetOrdinal("Pivacf_dts"));
                        if (!reader["Nome_dts"].Equals(DBNull.Value))
                            item.Nome_dts = reader.GetString(reader.GetOrdinal("Nome_dts"));
                        if (!reader["Cognome_dts"].Equals(DBNull.Value))
                            item.Cognome_dts = reader.GetString(reader.GetOrdinal("Cognome_dts"));
                        if (!reader["Datanascita_dts"].Equals(DBNull.Value))
                            item.Datanascita_dts = reader.GetDateTime(reader.GetOrdinal("Datanascita_dts"));
                        if (!reader["Sociopresentatore1_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore1_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore1_dts"));
                        if (!reader["Sociopresentatore2_dts"].Equals(DBNull.Value))
                            item.Sociopresentatore2_dts = reader.GetString(reader.GetOrdinal("Sociopresentatore2_dts"));
                        if (!reader["Telefonoprivato_dts"].Equals(DBNull.Value))
                            item.Telefonoprivato_dts = reader.GetString(reader.GetOrdinal("Telefonoprivato_dts"));
                        if (!reader["Annolaurea_dts"].Equals(DBNull.Value))
                            item.Annolaurea_dts = reader.GetString(reader.GetOrdinal("Annolaurea_dts"));
                        if (!reader["Annospecializzazione_dts"].Equals(DBNull.Value))
                            item.Annospecializzazione_dts = reader.GetString(reader.GetOrdinal("Annospecializzazione_dts"));
                        if (!reader["Altrespecializzazioni_dts"].Equals(DBNull.Value))
                            item.Altrespecializzazioni_dts = reader.GetString(reader.GetOrdinal("Altrespecializzazioni_dts"));
                        if (!reader["SocioSicpre_dts"].Equals(DBNull.Value))
                            item.SocioSicpre_dts = reader.GetBoolean(reader.GetOrdinal("SocioSicpre_dts"));
                        if (!reader["SocioIsaps_dts"].Equals(DBNull.Value))
                            item.SocioIsaps_dts = reader.GetBoolean(reader.GetOrdinal("SocioIsaps_dts"));
                        if (!reader["Socioaltraassociazione_dts"].Equals(DBNull.Value))
                            item.Socioaltraassociazione_dts = reader.GetString(reader.GetOrdinal("Socioaltraassociazione_dts"));
                        if (!reader["Trattamenticollegati_dts"].Equals(DBNull.Value))
                            item.Trattamenticollegati_dts = reader.GetString(reader.GetOrdinal("Trattamenticollegati_dts"));
                        if (!reader["AccettazioneStatuto_dts"].Equals(DBNull.Value))
                            item.AccettazioneStatuto_dts = reader.GetBoolean(reader.GetOrdinal("AccettazioneStatuto_dts"));

                        if (!reader["Certificazione_dts"].Equals(DBNull.Value))
                            item.Certificazione_dts = reader.GetBoolean(reader.GetOrdinal("Certificazione_dts"));
                        if (!reader["Emailriservata_dts"].Equals(DBNull.Value))
                            item.Emailriservata_dts = reader.GetString(reader.GetOrdinal("Emailriservata_dts"));

                        if (!reader["CodiceNAZIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE1_dts"));
                        if (!reader["CodiceREGIONE1_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE1_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE1_dts"));
                        if (!reader["CodicePROVINCIA1_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA1_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA1_dts"));
                        if (!reader["CodiceCOMUNE1_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE1_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE1_dts"));

                        if (!reader["CodiceNAZIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE2_dts"));
                        if (!reader["CodiceREGIONE2_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE2_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE2_dts"));
                        if (!reader["CodicePROVINCIA2_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA2_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA2_dts"));
                        if (!reader["CodiceCOMUNE2_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE2_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE2_dts"));

                        if (!reader["CodiceNAZIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceNAZIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceNAZIONE3_dts"));
                        if (!reader["CodiceREGIONE3_dts"].Equals(DBNull.Value))
                            item.CodiceREGIONE3_dts = reader.GetString(reader.GetOrdinal("CodiceREGIONE3_dts"));
                        if (!reader["CodicePROVINCIA3_dts"].Equals(DBNull.Value))
                            item.CodicePROVINCIA3_dts = reader.GetString(reader.GetOrdinal("CodicePROVINCIA3_dts"));
                        if (!reader["CodiceCOMUNE3_dts"].Equals(DBNull.Value))
                            item.CodiceCOMUNE3_dts = reader.GetString(reader.GetOrdinal("CodiceCOMUNE3_dts"));

                        if (!reader["Latitudine1_dts"].Equals(DBNull.Value))
                            item.Latitudine1_dts = reader.GetDouble(reader.GetOrdinal("Latitudine1_dts"));
                        if (!reader["Longitudine1_dts"].Equals(DBNull.Value))
                            item.Longitudine1_dts = reader.GetDouble(reader.GetOrdinal("Longitudine1_dts"));
                        if (!reader["Latitudine2_dts"].Equals(DBNull.Value))
                            item.Latitudine2_dts = reader.GetDouble(reader.GetOrdinal("Latitudine2_dts"));
                        if (!reader["Longitudine2_dts"].Equals(DBNull.Value))
                            item.Longitudine2_dts = reader.GetDouble(reader.GetOrdinal("Longitudine2_dts"));
                        if (!reader["Latitudine3_dts"].Equals(DBNull.Value))
                            item.Latitudine3_dts = reader.GetDouble(reader.GetOrdinal("Latitudine3_dts"));
                        if (!reader["Longitudine3_dts"].Equals(DBNull.Value))
                            item.Longitudine3_dts = reader.GetDouble(reader.GetOrdinal("Longitudine3_dts"));

                        if (!reader["Bloccoaccesso_dts"].Equals(DBNull.Value))
                            item.Bloccoaccesso_dts = reader.GetBoolean(reader.GetOrdinal("Bloccoaccesso_dts"));


                        if (!reader["Via1_dts"].Equals(DBNull.Value))
                            item.Via1_dts = reader.GetString(reader.GetOrdinal("Via1_dts"));
                        if (!reader["Cap1_dts"].Equals(DBNull.Value))
                            item.Cap1_dts = reader.GetString(reader.GetOrdinal("Cap1_dts"));
                        if (!reader["Nomeposizione1_dts"].Equals(DBNull.Value))
                            item.Nomeposizione1_dts = reader.GetString(reader.GetOrdinal("Nomeposizione1_dts"));
                        if (!reader["Telefono1_dts"].Equals(DBNull.Value))
                            item.Telefono1_dts = reader.GetString(reader.GetOrdinal("Telefono1_dts"));


                        if (!reader["Via2_dts"].Equals(DBNull.Value))
                            item.Via2_dts = reader.GetString(reader.GetOrdinal("Via2_dts"));
                        if (!reader["Cap2_dts"].Equals(DBNull.Value))
                            item.Cap2_dts = reader.GetString(reader.GetOrdinal("Cap2_dts"));
                        if (!reader["Nomeposizione2_dts"].Equals(DBNull.Value))
                            item.Nomeposizione2_dts = reader.GetString(reader.GetOrdinal("Nomeposizione2_dts"));
                        if (!reader["Telefono2_dts"].Equals(DBNull.Value))
                            item.Telefono2_dts = reader.GetString(reader.GetOrdinal("Telefono2_dts"));

                        if (!reader["Via3_dts"].Equals(DBNull.Value))
                            item.Via3_dts = reader.GetString(reader.GetOrdinal("Via3_dts"));
                        if (!reader["Cap3_dts"].Equals(DBNull.Value))
                            item.Cap3_dts = reader.GetString(reader.GetOrdinal("Cap3_dts"));
                        if (!reader["Nomeposizione3_dts"].Equals(DBNull.Value))
                            item.Nomeposizione3_dts = reader.GetString(reader.GetOrdinal("Nomeposizione3_dts"));
                        if (!reader["Telefono3_dts"].Equals(DBNull.Value))
                            item.Telefono3_dts = reader.GetString(reader.GetOrdinal("Telefono3_dts"));

                        if (!reader["Pagamenti_dts"].Equals(DBNull.Value))
                            item.Pagamenti_dts = reader.GetString(reader.GetOrdinal("Pagamenti_dts"));


                        if (!reader["ricfatt_dts"].Equals(DBNull.Value))
                            item.ricfatt_dts = reader.GetString(reader.GetOrdinal("ricfatt_dts"));

                        if (!reader["indirizzofatt_dts"].Equals(DBNull.Value))
                            item.indirizzofatt_dts = reader.GetString(reader.GetOrdinal("indirizzofatt_dts"));

                        if (!reader["noteriservate_dts"].Equals(DBNull.Value))
                            item.noteriservate_dts = reader.GetString(reader.GetOrdinal("noteriservate_dts"));

                        if (!reader["niscrordine_dts"].Equals(DBNull.Value))
                            item.niscrordine_dts = reader.GetString(reader.GetOrdinal("niscrordine_dts"));
                        if (!reader["locordine_dts"].Equals(DBNull.Value))
                            item.locordine_dts = reader.GetString(reader.GetOrdinal("locordine_dts"));
                        if (!reader["annofrequenza_dts"].Equals(DBNull.Value))
                            item.annofrequenza_dts = reader.GetString(reader.GetOrdinal("annofrequenza_dts"));
                        if (!reader["nomeuniversita_dts"].Equals(DBNull.Value))
                            item.nomeuniversita_dts = reader.GetString(reader.GetOrdinal("nomeuniversita_dts"));
                        if (!reader["dettagliuniversita_dts"].Equals(DBNull.Value))
                            item.dettagliuniversita_dts = reader.GetString(reader.GetOrdinal("dettagliuniversita_dts"));
                        if (!reader["Boolfields_dts"].Equals(DBNull.Value))
                            item.Boolfields_dts = reader.GetString(reader.GetOrdinal("Boolfields_dts"));
                        if (!reader["Textfield1_dts"].Equals(DBNull.Value))
                            item.Textfield1_dts = reader.GetString(reader.GetOrdinal("Textfield1_dts"));
                        if (!reader["Interventieseguiti_dts"].Equals(DBNull.Value))
                            item.Interventieseguiti_dts = reader.GetString(reader.GetOrdinal("Interventieseguiti_dts"));


                        return (item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento offerta :" + error.Message, error);
            }

            return item;
        }


        /// <summary>
        /// Carica la lista dei comuni distinti presenti in base ai record presenti nella tabella articoli
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<string> CaricaListaComuniPresenti(string connection, string codiceTipologia)
        {
            List<string> list = new List<string>();
            if (connection == null || connection == "") return list;
            List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();
            try
            {
                string query = "SELECT DISTINCT CodiceCOMUNE FROM " + Tblarchivio;

                if (!string.IsNullOrEmpty(codiceTipologia))
                {
                    SQLiteParameter ptip = new SQLiteParameter("@CodiceTIPOLOGIA", codiceTipologia);
                    _parUsed.Add(ptip);
                    if (!query.ToLower().Contains("where"))
                        query += " WHERE CodiceTIPOLOGIA like @CodiceTIPOLOGIA ";
                    else
                        query += " AND CodiceTIPOLOGIA like @CodiceTIPOLOGIA  ";
                }
                query += " order BY COLLATE NOCASE codiceCOMUNE ";

                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        // item = new Offerte();
                        string comune = reader.GetString(reader.GetOrdinal("codiceCOMUNE"));
                        if (!list.Exists(c => c.ToString().ToLower() == comune.ToLower()))
                        {
                            list.Add(comune);
                        }

                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Lettura tabella articoli :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Conta gli articoli per anno/mese e ne ritorno il numero per ogni coppia
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> ContaPerAnnoMese(string connection, string LinguaFiltro = "", string filtrotipologie = "", string filtrocategoria = "")
        {
            Dictionary<string, Dictionary<string, string>> list = new Dictionary<string, Dictionary<string, string>>();
            if (connection == null || connection == "") return list;

            try
            {
                string query = "";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                //SQLiteParameter p1 = new SQLiteParameter("@CodiceTipologia", CodTipologia);//OleDbType.VarChar
                //parColl.Add(p1);
                if (string.IsNullOrEmpty(filtrotipologie))
                {
                    query = "SELECT strftime('%m',[Datainserimento]) AS mese, strftime('%Y',[Datainserimento])  AS anno, Count(" + _tblarchivio + ".ID) AS numero FROM " + Tblarchivio;
                }
                else
                {
                    query = "SELECT  strftime('%m',[Datainserimento]) AS mese, strftime('%Y',[Datainserimento])  AS anno, Count(" + _tblarchivio + ".ID) AS numero FROM " + Tblarchivio;

                    filtrotipologie = filtrotipologie.Trim().Replace("|", ",");

                    string[] codici = filtrotipologie.Split(',');
                    if (codici != null && codici.Length > 0)
                    {
                        if (!query.ToLower().Contains("where"))
                            query += " WHERE CodiceTIPOLOGIA in (    ";
                        else
                            query += " AND  CodiceTIPOLOGIA in (      ";
                        foreach (string codice in codici)
                        {
                            if (!string.IsNullOrEmpty(codice.Trim()))
                                query += " '" + codice + "' ,";
                        }
                        query = query.TrimEnd(',') + " ) ";
                    }


                    if (!string.IsNullOrEmpty(filtrocategoria))
                    {
                        SQLiteParameter p1 = new SQLiteParameter("@CodiceCategoria", filtrocategoria);//OleDbType.VarChar
                        parColl.Add(p1);
                        if (!query.ToLower().Contains("where"))
                            query += " WHERE CodiceCategoria like @CodiceCategoria ";
                        else
                            query += " AND CodiceCategoria like @CodiceCategoria  ";
                    }


                }

                //    query += " GROUP BY Year([Datainserimento]), Month([Datainserimento]) order BY Year([Datainserimento]) DESC, Month([Datainserimento]) DESC; ";
                query += " GROUP BY  strftime('%Y',[Datainserimento]),strftime('%m',[Datainserimento]) order BY strftime('%Y',[Datainserimento]) COLLATE NOCASE DESC, strftime('%m',[Datainserimento]) COLLATE NOCASE DESC; ";


                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        // item = new Offerte();
                        string anno = reader.GetString(reader.GetOrdinal("anno"));
                        string mese = reader.GetString(reader.GetOrdinal("mese"));
                        long numero = reader.GetInt64(reader.GetOrdinal("numero"));

                        if (!list.ContainsKey(anno.ToString()))
                        {
                            list.Add(anno.ToString(), new Dictionary<string, string>());
                            list[anno.ToString()].Add(mese.ToString(), numero.ToString());
                        }
                        else
                        {
                            if (!list[anno.ToString()].ContainsKey(mese.ToString()))
                                list[anno.ToString()].Add(mese.ToString(), numero.ToString());
                            else
                            {
                                list[anno.ToString()][mese.ToString()] = numero.ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Lettura tabella articoli :" + error.Message, error);
            }

            return list;
        }


        /// <summary>
        /// Funzione che carica la lista dei sotto prodotti che hanno una certa categoria prodotto
        /// </summary>
        /// <param name="connection">Connessione</param>
        /// <param name="CodProdotto">Codice Categoria Prodotto richiesto</param>
        /// <returns></returns>
        public OfferteCollection CaricaSottoprodottiPerCodiceProdotto(string connection, string CodiceCategoria)
        {
            OfferteCollection list = new OfferteCollection();
            if (connection == null || connection == "") return list;
            if (CodiceCategoria == null || CodiceCategoria == "") return list;

            Offerte item;

            try
            {
                string query = "SELECT * FROM " + _tblarchivio + " where CodiceCategoria like @CodiceCategoria and archiviato=0 order BY DataInserimento Desc";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@CodiceCategoria", CodiceCategoria);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Offerte();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!reader["Id_collegato"].Equals(DBNull.Value))
                            item.Id_collegato = reader.GetInt64(reader.GetOrdinal("Id_collegato"));

                        if (!reader["Autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("Autore"));


                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));
                        if (!reader["Data1"].Equals(DBNull.Value))
                            item.Data1 = reader.GetDateTime(reader.GetOrdinal("Data1"));
                        if (!reader["DescrizioneGB"].Equals(DBNull.Value))
                            item.DescrizioneGB = reader.GetString(reader.GetOrdinal("DescrizioneGB"));
                        if (!reader["DescrizioneI"].Equals(DBNull.Value))
                            item.DescrizioneI = reader.GetString(reader.GetOrdinal("DescrizioneI"));
                        if (!reader["DENOMINAZIONEGB"].Equals(DBNull.Value))
                            item.DenominazioneGB = reader.GetString(reader.GetOrdinal("DENOMINAZIONEGB"));
                        if (!reader["DENOMINAZIONEI"].Equals(DBNull.Value))
                            item.DenominazioneI = reader.GetString(reader.GetOrdinal("DENOMINAZIONEI"));

                        if (!reader["Campo1I"].Equals(DBNull.Value))
                            item.Campo1I = reader.GetString(reader.GetOrdinal("Campo1I"));
                        if (!reader["Campo1GB"].Equals(DBNull.Value))
                            item.Campo1GB = reader.GetString(reader.GetOrdinal("Campo1GB"));
                        if (!reader["Campo2I"].Equals(DBNull.Value))
                            item.Campo2I = reader.GetString(reader.GetOrdinal("Campo2I"));
                        if (!reader["Campo2GB"].Equals(DBNull.Value))
                            item.Campo2GB = reader.GetString(reader.GetOrdinal("Campo2GB"));

                        if (!reader["DATITECNICIRU"].Equals(DBNull.Value))
                            item.DatitecniciRU = reader.GetString(reader.GetOrdinal("DATITECNICIRU"));
                        if (!reader["DescrizioneRU"].Equals(DBNull.Value))
                            item.DescrizioneRU = reader.GetString(reader.GetOrdinal("DescrizioneRU"));
                        if (!reader["DENOMINAZIONERU"].Equals(DBNull.Value))
                            item.DenominazioneRU = reader.GetString(reader.GetOrdinal("DENOMINAZIONERU"));
                        if (!reader["Campo1RU"].Equals(DBNull.Value))
                            item.Campo1RU = reader.GetString(reader.GetOrdinal("Campo1RU"));
                        if (!reader["Campo2RU"].Equals(DBNull.Value))
                            item.Campo2RU = reader.GetString(reader.GetOrdinal("Campo2RU"));


                        if (!reader["XmlValue"].Equals(DBNull.Value))
                            item.Xmlvalue = reader.GetString(reader.GetOrdinal("Xmlvalue"));


                        if (!reader["Caratteristica1"].Equals(DBNull.Value))
                            item.Caratteristica1 = reader.GetInt64(reader.GetOrdinal("Caratteristica1"));
                        if (!reader["Caratteristica2"].Equals(DBNull.Value))
                            item.Caratteristica2 = reader.GetInt64(reader.GetOrdinal("Caratteristica2"));
                        if (!reader["Caratteristica3"].Equals(DBNull.Value))
                            item.Caratteristica3 = reader.GetInt64(reader.GetOrdinal("Caratteristica3"));
                        if (!reader["Caratteristica4"].Equals(DBNull.Value))
                            item.Caratteristica4 = reader.GetInt64(reader.GetOrdinal("Caratteristica4"));
                        if (!reader["Caratteristica5"].Equals(DBNull.Value))
                            item.Caratteristica5 = reader.GetInt64(reader.GetOrdinal("Caratteristica5"));
                        if (!reader["Caratteristica6"].Equals(DBNull.Value))
                            item.Caratteristica6 = reader.GetInt64(reader.GetOrdinal("Caratteristica6"));

                        if (!reader["Anno"].Equals(DBNull.Value))
                            item.Anno = reader.GetInt64(reader.GetOrdinal("Anno"));


                        if (!reader["CodiceCOMUNE"].Equals(DBNull.Value))
                            item.CodiceComune = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        if (!reader["CodicePROVINCIA"].Equals(DBNull.Value))
                            item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        if (!reader["CodiceREGIONE"].Equals(DBNull.Value))
                            item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        if (!reader["linkVideo"].Equals(DBNull.Value))
                            item.linkVideo = reader.GetString(reader.GetOrdinal("linkVideo"));

                        if (!reader["CodiceProdotto"].Equals(DBNull.Value))
                            item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto"));

                        if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                            item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                        if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value))
                            item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));

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
                        if (!reader["PrezzoListino"].Equals(DBNull.Value))
                            item.PrezzoListino = reader.GetDouble(reader.GetOrdinal("PrezzoListino"));
                        if (!reader["Vetrina"].Equals(DBNull.Value))
                            item.Vetrina = reader.GetBoolean(reader.GetOrdinal("Vetrina"));
                        if (!reader["Abilitacontatto"].Equals(DBNull.Value))
                            item.Abilitacontatto = reader.GetBoolean(reader.GetOrdinal("Abilitacontatto"));
                        if (!reader["Archiviato"].Equals(DBNull.Value))
                            item.Archiviato = reader.GetBoolean(reader.GetOrdinal("Archiviato"));

                        if (!reader["Qta_vendita"].Equals(DBNull.Value))
                            item.Qta_vendita = reader.GetDouble(reader.GetOrdinal("Qta_vendita"));
                        if (!reader["Promozione"].Equals(DBNull.Value))
                            item.Promozione = reader.GetBoolean(reader.GetOrdinal("Promozione"));

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

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Sottoprodotti :" + error.Message, error);
            }

            return list;
        }

        /// <summary>
        /// Torna il conteggio delle categorie e sottocategorie raggruppate ( Categoria->Sottocategoria->numero elementi )
        /// filtrati per codice tipologia
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="CodiceCategoria"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, long>> ContaProdottiSottoprodotti(string connection, string CodiceTipologia)
        {
            Dictionary<string, Dictionary<string, long>> list = new Dictionary<string, Dictionary<string, long>>();
            if (connection == null || connection == "") return list;
            Dictionary<string, long> _sprodotti = new Dictionary<string, long>();
            try
            {
                string query = "SELECT CodiceCategoria,CodiceCategoria2Liv,count(CodiceCategoria2Liv) as totsottoprodotti FROM " + _tblarchivio + " WHERE   archiviato=0 and CodiceTIPOLOGIA = @CodiceTIPOLOGIA group by CodiceCategoria,CodiceCategoria2Liv";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@CodiceTIPOLOGIA", CodiceTipologia);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;
                    while (reader.Read())
                    {
                        if (!reader["CodiceCategoria"].Equals(DBNull.Value))
                        {
                            string codicecategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria"));
                            if (!list.ContainsKey(codicecategoria))
                            {
                                _sprodotti = new Dictionary<string, long>();
                                if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value) && !reader["totsottoprodotti"].Equals(DBNull.Value))
                                {
                                    string codicecategoria2liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));
                                    _sprodotti.Add(codicecategoria2liv, reader.GetInt64(reader.GetOrdinal("totsottoprodotti")));
                                    list.Add(codicecategoria, _sprodotti);
                                }
                            }
                            else
                            {
                                _sprodotti = list[codicecategoria];
                                if (!reader["CodiceCategoria2Liv"].Equals(DBNull.Value) && !reader["totsottoprodotti"].Equals(DBNull.Value))
                                {
                                    string codicecategoria2liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv"));
                                    _sprodotti.Add(codicecategoria2liv, reader.GetInt64(reader.GetOrdinal("totsottoprodotti")));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Conteggio prodotti/sprodotti :" + error.Message, error);
            }
            return list;
        }

        /// <summary>
        /// Ricrea la list delle foto a partire dalle stringhe schema e valori
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public AllegatiCollection CaricaAllegatiFoto(AllegatiCollection list)
        {
            //Spacchetto gli allegati che sono nella forma
            //esempio schema All1:S:0:13:Des1:S:13:7:All2:S:20:13:Des2:S:33:7:
            Allegato item = new Allegato();
            string Schema = list.Schema;
            string Value = list.Valori;
            int i = 0;
            int j = 0;
            int start = 0;
            int end = 0;
            string etype = "";
            int startseq = 0;
            while (start < Schema.Length)
            {
                etype = "all";
                //LEGGIAMO LO SCHEMA PER IL NOMEALLEGATO --------------------------------------------------------------------------
                start = Schema.IndexOf(":S:", start) + 3;
                //Controllo tipo ////////////////////////////////////////
                if (start - 3 > 0)
                {
                    startseq = Schema.LastIndexOf(":", start - 4);
                    if (startseq != -1)
                    {
                        if (Schema.Substring(startseq + 1, 3).ToLower().StartsWith("all")) etype = "all";
                        if (Schema.Substring(startseq + 1, 3).ToLower().StartsWith("des")) etype = "des";
                        if (Schema.Substring(startseq + 1, 3).ToLower().StartsWith("pro")) etype = "pro";
                    }
                }
                ///////////////////////////////////////////////////////////////////
                switch (etype)
                {
                    case "all":
                        if (!string.IsNullOrEmpty(item.NomeFile)) list.Add(item);
                        item = new Allegato();
                        end = Schema.IndexOf(":", start);
                        if (end == -1) return list;
                        i = Convert.ToInt32(Schema.Substring(start, (end - start)));//Posizione di inizio
                        start = end + 1;
                        end = Schema.IndexOf(":", start);
                        j = Convert.ToInt32(Schema.Substring(start, (end - start)));//N.Caratteri da leggere
                        start = end + 1;
                        //LEGGIAMO IL VALORE (NOMEALLEGATO)
                        item.NomeFile = Value.Substring(i, j);
                        if (!Value.Substring(i, j).ToLower().StartsWith("http://") && !Value.Substring(i, j).ToLower().StartsWith("https://"))
                            item.NomeAnteprima = "Ant" + Value.Substring(i, j);
                        else
                            item.NomeAnteprima = Value.Substring(i, j);
                        break;
                    case "des":
                        end = Schema.IndexOf(":", start);
                        i = Convert.ToInt32(Schema.Substring(start, (end - start)));//Posizione di inizio
                        start = end + 1;
                        end = Schema.IndexOf(":", start);
                        j = Convert.ToInt32(Schema.Substring(start, (end - start)));//N.Caratteri da leggere
                        start = end + 1;
                        //LEGGIAMO IL VALORE (descrizione ALLEGATO)
                        item.Descrizione = Value.Substring(i, j);
                        break;
                    case "pro":
                        end = Schema.IndexOf(":", start);
                        i = Convert.ToInt32(Schema.Substring(start, (end - start)));//Posizione di inizio
                        start = end + 1;
                        end = Schema.IndexOf(":", start);
                        j = Convert.ToInt32(Schema.Substring(start, (end - start)));//N.Caratteri da leggere
                        start = end + 1;
                        //LEGGIAMO IL VALORE (descrizione ALLEGATO)
                        int tmpro = 0;
                        int.TryParse(Value.Substring(i, j), out tmpro);
                        item.Progressivo = tmpro;
                        break;
                    default:

                        break;
                }


                //LA CARTELLA PER LE FOTO E' SEMPRE LA STESSA (potrei indicarla volendo ...)
                item.Cartella = "";
                //Inserisco il percorso per la foto di anteprima
#if false
                if (!flagprimafoto && item.NomeAnteprima != "Ant" && item.NomeAnteprima != "")
                {
                    //if (!(item.Descrizione.Trim().ToString().ToLower() == "cv" || item.Descrizione.Trim().ToString().ToLower() == "cequip"))
                    //{
                    flagprimafoto = true;
                    list.FotoAnteprima = item.NomeAnteprima;
                    //}
                }
                if (item.Progressivo == 1) list.FotoAnteprima = item.NomeAnteprima; 
#endif

                if (!(start < Schema.Length)) if (!string.IsNullOrEmpty(item.NomeFile)) list.Add(item);

            }
            // list.Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Allegato>("Progressivo", System.ComponentModel.ListSortDirection.Ascending));
            if (list != null && list.Count > 0)
            {
                list.Sort(new GenericComparer2<Allegato>("Progressivo", System.ComponentModel.ListSortDirection.Ascending, "NomeFile", System.ComponentModel.ListSortDirection.Ascending));
                list.FotoAnteprima = list[0].NomeAnteprima; //Setto la foto anteprima per tutta la collection
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
            //esempio schema All1:S:0:13:Des1:S:13:7:All2:S:20:13:Des2:S:33:7:
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

                //INSERISCO Progressivo
                len = item.Progressivo.ToString().Length;
                item.Progressivo.ToString().Replace(":S:", "SSS");//Elimina eventuali presenze
                                                                  //del carattere di separazione dalla descrizione
                list.Schema += "Pro" + n + ":S:" + pos + ":" + len + ":";
                list.Valori += item.Progressivo.ToString();
                pos += len;


            }

            return list;
        }

        public bool modificaFoto(string connection, long idOfferta, string nomefile, string descrizione, string progressivo = "")
        {
            if (connection == "") return false;
            if (idOfferta == 0) return false;
            //Carico le foto preesistenti nel db
            AllegatiCollection FotoColl = this.getListaFotobyId(connection, idOfferta);
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
                int tmp = 0;
                int.TryParse(progressivo, out tmp);
                F1.Progressivo = tmp;

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
                SQLiteParameter p4 = new SQLiteParameter("@id", idOfferta);//OleDbType.VarChar
                parColl.Add(p4);
                string query = "UPDATE [" + Tblarchivio + "] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori  WHERE ([Id]=@id)";
                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento Foto Offerte :" + error.Message, error);
                }

            }
            return true;
        }

        public bool insertFoto(string connection, long idOfferta, string nomefile, string descrizione, string progressivo = "")
        {
            if (connection == "") return false;
            if (idOfferta == 0) return false;
            //Carico le foto preesistenti nel db
            AllegatiCollection FotoColl = this.getListaFotobyId(connection, idOfferta);
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
                int tmpint = 1;
                if (!string.IsNullOrEmpty(progressivo))
                    int.TryParse(progressivo, out tmpint);
                tmp.Progressivo = tmpint;

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
                SQLiteParameter p4 = new SQLiteParameter("@id", idOfferta);//OleDbType.VarChar
                parColl.Add(p4);
                string query = "UPDATE [" + Tblarchivio + "] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori  WHERE ([Id]=@id)";
                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento Foto Offerte :" + error.Message, error);
                }

            }
            return true;
        }

        public bool CancellaFoto(string connection, long idOfferta, string nomefile, string descrizione, string pathfile)
        {

            if (connection == "") return false;
            if (idOfferta == 0) return false;
            //Carico le foto preesistenti nel db
            AllegatiCollection FotoColl = this.getListaFotobyId(connection, idOfferta);
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
                    SQLiteParameter p4 = new SQLiteParameter("@id", idOfferta);//OleDbType.VarChar
                    parColl.Add(p4);
                    string query = "UPDATE [" + Tblarchivio + "] SET [FotoSchema]=@fotoschema,[FotoValori]=@fotovalori  WHERE ([Id]=@id)";
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

        /// <summary>
        /// Carica la collection delle foto a partire dall'id del record dell'offerta passata
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idContenuto"></param>
        /// <returns></returns>
        public AllegatiCollection getListaFotobyId(string connection, long idOfferta)
        {
            if (connection == null || connection == "") { return null; };
            if (idOfferta == 0) { return null; };

            string query = "SELECT [FotoSchema],[FotoValori] FROM " + Tblarchivio + " where ID=@idOfferta";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@idOfferta", idOfferta);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
            Offerte item = new Offerte();
            using (reader)
            {
                if (reader == null) { return null; };
                if (reader.HasRows == false)
                    return null;
                while (reader.Read())
                {
                    item = new Offerte();
                    item.Id = idOfferta;
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
        /// Inserisce un record in tabella OFFERTE
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertOfferta(string connessione,
        Offerte item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@DENOMINAZIONEI", item.DenominazioneI);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@DENOMINAZIONEGB", item.DenominazioneGB);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@DescrizioneI", item.DescrizioneI);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@DescrizioneGB", item.DescrizioneGB);
            parColl.Add(p5);

            string schema = "";
            if (item.FotoCollection_M.Schema != null)
                schema = item.FotoCollection_M.Schema;

            string valori = "";
            if (item.FotoCollection_M.Valori != null)
                valori = item.FotoCollection_M.Valori;

            SQLiteParameter p6 = new SQLiteParameter("@FotoSchema", schema);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@FotoValori", valori);
            parColl.Add(p7);

            SQLiteParameter p8 = new SQLiteParameter("@CodiceCOMUNE", item.CodiceComune);
            parColl.Add(p8);
            SQLiteParameter p9 = new SQLiteParameter("@CodicePROVINCIA", item.CodiceProvincia);
            parColl.Add(p9);
            SQLiteParameter p10 = new SQLiteParameter("@CodiceREGIONE", item.CodiceRegione);
            parColl.Add(p10);
            SQLiteParameter p11 = new SQLiteParameter("@DATITECNICII", item.DatitecniciI);
            parColl.Add(p11);
            SQLiteParameter p12 = new SQLiteParameter("@DATITECNICIGB", item.DatitecniciGB);
            parColl.Add(p12);
            SQLiteParameter p13 = new SQLiteParameter("@EMAIL", item.Email);
            parColl.Add(p13);
            SQLiteParameter p14 = new SQLiteParameter("@FAX", item.Fax);
            parColl.Add(p14);
            SQLiteParameter p15 = new SQLiteParameter("@INDIRIZZO", item.Indirizzo);
            parColl.Add(p15);
            SQLiteParameter p16 = new SQLiteParameter("@TELEFONO", item.Telefono);
            parColl.Add(p16);
            SQLiteParameter p17 = new SQLiteParameter("@WEBSITE", item.Website);
            parColl.Add(p17);
            SQLiteParameter p18 = new SQLiteParameter("@data", dbDataAccess.CorrectDatenow(item.DataInserimento));
            //p18.OleDbType = OleDbType.Date;
            parColl.Add(p18);
            SQLiteParameter pdata1 = new SQLiteParameter("@data1", dbDataAccess.CorrectDatenow(item.Data1));
            parColl.Add(pdata1);

            SQLiteParameter p19 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);
            parColl.Add(p19);
            SQLiteParameter p20a = new SQLiteParameter("@CodiceCategoria", item.CodiceCategoria);
            parColl.Add(p20a);
            SQLiteParameter p20 = new SQLiteParameter("@CodiceCategoria2Liv", item.CodiceCategoria2Liv);
            parColl.Add(p20);
            SQLiteParameter p21 = new SQLiteParameter("@Prezzo", item.Prezzo);
            parColl.Add(p21);
            SQLiteParameter p22 = new SQLiteParameter("@PrezzoListino", item.PrezzoListino);
            parColl.Add(p22);
            SQLiteParameter p23 = new SQLiteParameter("@Vetrina", item.Vetrina);
            parColl.Add(p23);
            SQLiteParameter pabcon = new SQLiteParameter("@Abilitacontatto", item.Abilitacontatto);
            parColl.Add(pabcon);
            SQLiteParameter pvideo = new SQLiteParameter("@linkVideo", item.linkVideo);
            parColl.Add(pvideo);

            SQLiteParameter pcampo1i = new SQLiteParameter("@Campo1I", item.Campo1I);
            parColl.Add(pcampo1i);
            SQLiteParameter pcampoi2 = new SQLiteParameter("@Campo2I", item.Campo2I);
            parColl.Add(pcampoi2);
            SQLiteParameter pcampo1gb = new SQLiteParameter("@Campo1GB", item.Campo1GB);
            parColl.Add(pcampo1gb);
            SQLiteParameter pcampo2gb = new SQLiteParameter("@Campo2GB", item.Campo2GB);
            parColl.Add(pcampo2gb);

            SQLiteParameter pcar1i = new SQLiteParameter("@Caratteristica1", item.Caratteristica1);
            parColl.Add(pcar1i);
            SQLiteParameter pcar2i = new SQLiteParameter("@Caratteristica2", item.Caratteristica2);
            parColl.Add(pcar2i);
            SQLiteParameter pcar3i = new SQLiteParameter("@Caratteristica3", item.Caratteristica3);
            parColl.Add(pcar3i);
            SQLiteParameter pcar4i = new SQLiteParameter("@Caratteristica4", item.Caratteristica4);
            parColl.Add(pcar4i);
            SQLiteParameter pcar5i = new SQLiteParameter("@Caratteristica5", item.Caratteristica5);
            parColl.Add(pcar5i);
            SQLiteParameter pcar6i = new SQLiteParameter("@Caratteristica6", item.Caratteristica6);
            parColl.Add(pcar6i);

            SQLiteParameter pcaranno = new SQLiteParameter("@Anno", item.Anno);
            parColl.Add(pcaranno);
            SQLiteParameter parch = new SQLiteParameter("@Archiviato", item.Archiviato);
            parColl.Add(parch);

            SQLiteParameter pidcoll = new SQLiteParameter("@Id_collegato", item.Id_collegato);
            parColl.Add(pidcoll);
            SQLiteParameter pidcollsub = new SQLiteParameter("@Id_dts_collegato", item.Id_dts_collegato);
            parColl.Add(pidcollsub);

            SQLiteParameter pautore = new SQLiteParameter("@Autore", item.Autore);
            parColl.Add(pautore);

            SQLiteParameter pxmlvalue = new SQLiteParameter("@xmlvalue", item.Xmlvalue);
            parColl.Add(pxmlvalue);

            SQLiteParameter p3ru = new SQLiteParameter("@DENOMINAZIONERU", item.DenominazioneRU);
            parColl.Add(p3ru);
            SQLiteParameter p5ru = new SQLiteParameter("@DescrizioneRU", item.DescrizioneRU);
            parColl.Add(p5ru);
            SQLiteParameter p12ru = new SQLiteParameter("@DATITECNICIRU", item.DatitecniciRU);
            parColl.Add(p12ru);
            SQLiteParameter pcampo1ru = new SQLiteParameter("@Campo1RU", item.Campo1RU);
            parColl.Add(pcampo1ru);
            SQLiteParameter pcampo2ru = new SQLiteParameter("@Campo2RU", item.Campo2RU);
            parColl.Add(pcampo2ru);

            SQLiteParameter pqtavendita = new SQLiteParameter();
            if (item.Qta_vendita == null)
                pqtavendita = new SQLiteParameter("@Qta_vendita", DBNull.Value);
            else
                pqtavendita = new SQLiteParameter("@Qta_vendita", item.Qta_vendita.Value);
            parColl.Add(pqtavendita);

            SQLiteParameter pPromozione = new SQLiteParameter("@Promozione", item.Promozione);
            parColl.Add(pPromozione);



            string query = "INSERT INTO " + _tblarchivio + " ([CodiceTIPOLOGIA],[DENOMINAZIONEI],[DENOMINAZIONEGB],[DescrizioneI],[DescrizioneGB],[FotoSchema],[FotoValori],[CodiceCOMUNE],[CodicePROVINCIA],[CodiceREGIONE],[DATITECNICII],[DATITECNICIGB],[EMAIL],[FAX],[INDIRIZZO],[TELEFONO],[WEBSITE],[DataInserimento],[Data1],[CodiceProdotto],[CodiceCategoria],[CodiceCategoria2Liv],[Prezzo],[PrezzoListino],[Vetrina],[Abilitacontatto],linkVideo,campo1I,campo2I,campo1GB,campo2GB,Caratteristica1,Caratteristica2,Caratteristica3,Caratteristica4,Caratteristica5,Caratteristica6,Anno,Archiviato,Id_collegato,Id_dts_collegato,Autore,xmlValue,DENOMINAZIONERU,DescrizioneRU,DATITECNICIRU,campo1RU,campo2RU,Qta_vendita,Promozione  ) VALUES (@CodiceTIPOLOGIA,@DENOMINAZIONEI,@DENOMINAZIONEGB,@DescrizioneI,@DescrizioneGB,@FotoSchema,@FotoValori,@CodiceCOMUNE,@CodicePROVINCIA,@CodiceREGIONE,@DATITECNICII,@DATITECNICIGB,@EMAIL,@FAX,@INDIRIZZO,@TELEFONO,@WEBSITE,@Data,@data1,@CodiceProdotto,@CodiceCategoria,@CodiceCategoria2Liv,@Prezzo,@PrezzoListino,@Vetrina,@Abilitacontatto,@linkVideo,@Campo1I,@Campo2I,@Campo1GB,@Campo2GB,@Caratteristica1,@Caratteristica2,@Caratteristica3,@Caratteristica4,@Caratteristica5,@Caratteristica6,@Anno,@Archiviato,@Id_collegato,@Id_dts_collegato,@Autore,@xmlValue,@DENOMINAZIONERU,@DescrizioneRU,@DATITECNICIRU,@Campo1RU,@Campo2RU,@Qta_vendita,@Promozione )";
            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                item.Id = lastidentity; //Inserisco nell'id dell'elemento inseito l'id generato dal db
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento Offerte :" + error.Message, error);
            }
            return;
        }

        public void InsertOffertaCollegata(string connessione,
        Offerte item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter AccettazioneStatuto_dts = new SQLiteParameter("@AccettazioneStatuto_dts", item.AccettazioneStatuto_dts);//OleDbType.VarChar
            parColl.Add(AccettazioneStatuto_dts);
            SQLiteParameter Altrespecializzazioni_dts = new SQLiteParameter("@Altrespecializzazioni_dts", item.Altrespecializzazioni_dts);
            parColl.Add(Altrespecializzazioni_dts);
            SQLiteParameter Annolaurea_dts = new SQLiteParameter("@Annolaurea_dts", item.Annolaurea_dts);
            parColl.Add(Annolaurea_dts);
            SQLiteParameter Annospecializzazione_dts = new SQLiteParameter("@Annospecializzazione_dts", item.Annospecializzazione_dts);
            parColl.Add(Annospecializzazione_dts);
            SQLiteParameter Bloccoaccesso_dts = new SQLiteParameter("@Bloccoaccesso_dts", item.Bloccoaccesso_dts);
            parColl.Add(Bloccoaccesso_dts);
            SQLiteParameter Cap1_dts = new SQLiteParameter("@Cap1_dts", item.Cap1_dts);
            parColl.Add(Cap1_dts);
            SQLiteParameter Cap2_dts = new SQLiteParameter("@Cap2_dts", item.Cap2_dts);
            parColl.Add(Cap2_dts);
            SQLiteParameter Cap3_dts = new SQLiteParameter("@Cap3_dts", item.Cap3_dts);
            parColl.Add(Cap3_dts);
            SQLiteParameter Certificazione_dts = new SQLiteParameter("@Certificazione_dts", item.Certificazione_dts);
            parColl.Add(Certificazione_dts);
            SQLiteParameter CodiceCOMUNE1_dts = new SQLiteParameter("@CodiceCOMUNE1_dts", item.CodiceCOMUNE1_dts);
            parColl.Add(CodiceCOMUNE1_dts);
            SQLiteParameter CodiceCOMUNE2_dts = new SQLiteParameter("@CodiceCOMUNE2_dts", item.CodiceCOMUNE2_dts);
            parColl.Add(CodiceCOMUNE2_dts);
            SQLiteParameter CodiceCOMUNE3_dts = new SQLiteParameter("@CodiceCOMUNE3_dts", item.CodiceCOMUNE3_dts);
            parColl.Add(CodiceCOMUNE3_dts);
            SQLiteParameter CodiceNAZIONE1_dts = new SQLiteParameter("@CodiceNAZIONE1_dts", item.CodiceNAZIONE1_dts);
            parColl.Add(CodiceNAZIONE1_dts);
            SQLiteParameter CodiceNAZIONE2_dts = new SQLiteParameter("@CodiceNAZIONE2_dts", item.CodiceNAZIONE2_dts);
            parColl.Add(CodiceNAZIONE2_dts);
            SQLiteParameter CodiceNAZIONE3_dts = new SQLiteParameter("@CodiceNAZIONE3_dts", item.CodiceNAZIONE3_dts);
            parColl.Add(CodiceNAZIONE3_dts);
            SQLiteParameter CodicePROVINCIA1_dts = new SQLiteParameter("@CodicePROVINCIA1_dts", item.CodicePROVINCIA1_dts);
            parColl.Add(CodicePROVINCIA1_dts);
            SQLiteParameter CodicePROVINCIA2_dts = new SQLiteParameter("@CodicePROVINCIA2_dts", item.CodicePROVINCIA2_dts);
            parColl.Add(CodicePROVINCIA2_dts);
            SQLiteParameter CodicePROVINCIA3_dts = new SQLiteParameter("@CodicePROVINCIA3_dts", item.CodicePROVINCIA3_dts);
            parColl.Add(CodicePROVINCIA3_dts);

            SQLiteParameter Datanascita_dts = new SQLiteParameter("@Datanascita_dts", dbDataAccess.CorrectDatenow(item.Datanascita_dts));
            parColl.Add(Datanascita_dts);

            SQLiteParameter CodiceREGIONE1_dts = new SQLiteParameter("@CodiceREGIONE1_dts", item.CodiceREGIONE1_dts);
            parColl.Add(CodiceREGIONE1_dts);
            SQLiteParameter CodiceREGIONE2_dts = new SQLiteParameter("@CodiceREGIONE2_dts", item.CodiceREGIONE2_dts);
            parColl.Add(CodiceREGIONE2_dts);
            SQLiteParameter CodiceREGIONE3_dts = new SQLiteParameter("@CodiceREGIONE3_dts", item.CodiceREGIONE3_dts);
            parColl.Add(CodiceREGIONE3_dts);
            SQLiteParameter Cognome_dts = new SQLiteParameter("@Cognome_dts", item.Cognome_dts);
            parColl.Add(Cognome_dts);

            SQLiteParameter Emailriservata_dts = new SQLiteParameter("@Emailriservata_dts", item.Emailriservata_dts);
            parColl.Add(Emailriservata_dts);
            SQLiteParameter Latitudine1_dts = new SQLiteParameter("@Latitudine1_dts", item.Latitudine1_dts);
            parColl.Add(Latitudine1_dts);

            SQLiteParameter Latitudine2_dts = new SQLiteParameter("@Latitudine2_dts", item.Latitudine2_dts);
            parColl.Add(Latitudine2_dts);
            SQLiteParameter Latitudine3_dts = new SQLiteParameter("@Latitudine3_dts", item.Latitudine3_dts);
            parColl.Add(Latitudine3_dts);
            SQLiteParameter Longitudine1_dts = new SQLiteParameter("@Longitudine1_dts", item.Longitudine1_dts);
            parColl.Add(Longitudine1_dts);
            SQLiteParameter Longitudine2_dts = new SQLiteParameter("@Longitudine2_dts", item.Longitudine2_dts);
            parColl.Add(Longitudine2_dts);

            SQLiteParameter Longitudine3_dts = new SQLiteParameter("@Longitudine3_dts", item.Longitudine3_dts);
            parColl.Add(Longitudine3_dts);
            SQLiteParameter Nome_dts = new SQLiteParameter("@Nome_dts", item.Nome_dts);
            parColl.Add(Nome_dts);
            SQLiteParameter Nomeposizione1_dts = new SQLiteParameter("@Nomeposizione1_dts", item.Nomeposizione1_dts);
            parColl.Add(Nomeposizione1_dts);
            SQLiteParameter Nomeposizione2_dts = new SQLiteParameter("@Nomeposizione2_dts", item.Nomeposizione2_dts);
            parColl.Add(Nomeposizione2_dts);
            SQLiteParameter Nomeposizione3_dts = new SQLiteParameter("@Nomeposizione3_dts", item.Nomeposizione3_dts);
            parColl.Add(Nomeposizione3_dts);
            SQLiteParameter Pivacf_dts = new SQLiteParameter("@Pivacf_dts", item.Pivacf_dts);
            parColl.Add(Pivacf_dts);

            SQLiteParameter Socioaltraassociazione_dts = new SQLiteParameter("@Socioaltraassociazione_dts", item.Socioaltraassociazione_dts);
            parColl.Add(Socioaltraassociazione_dts);
            SQLiteParameter SocioIsaps_dts = new SQLiteParameter("@SocioIsaps_dts", item.SocioIsaps_dts);
            parColl.Add(SocioIsaps_dts);

            SQLiteParameter Sociopresentatore1_dts = new SQLiteParameter("@Sociopresentatore1_dts", item.Sociopresentatore1_dts);
            parColl.Add(Sociopresentatore1_dts);

            SQLiteParameter Sociopresentatore2_dts = new SQLiteParameter("@Sociopresentatore2_dts", item.Sociopresentatore2_dts);
            parColl.Add(Sociopresentatore2_dts);

            SQLiteParameter SocioSicpre_dts = new SQLiteParameter("@SocioSicpre_dts", item.SocioSicpre_dts);
            parColl.Add(SocioSicpre_dts);
            SQLiteParameter Telefono1_dts = new SQLiteParameter("@Telefono1_dts", item.Telefono1_dts);
            parColl.Add(Telefono1_dts);
            SQLiteParameter Telefono2_dts = new SQLiteParameter("@Telefono2_dts", item.Telefono2_dts);
            parColl.Add(Telefono2_dts);
            SQLiteParameter Telefono3_dts = new SQLiteParameter("@Telefono3_dts", item.Telefono3_dts);
            parColl.Add(Telefono3_dts);
            SQLiteParameter Telefonoprivato_dts = new SQLiteParameter("@Telefonoprivato_dts", item.Telefonoprivato_dts);
            parColl.Add(Telefonoprivato_dts);

            SQLiteParameter Trattamenticollegati_dts = new SQLiteParameter("@Trattamenticollegati_dts", item.Trattamenticollegati_dts);
            parColl.Add(Trattamenticollegati_dts);
            SQLiteParameter Via1_dts = new SQLiteParameter("@Via1_dts", item.Via1_dts);
            parColl.Add(Via1_dts);
            SQLiteParameter Via2_dts = new SQLiteParameter("@Via2_dts", item.Via2_dts);
            parColl.Add(Via2_dts);

            SQLiteParameter Via3_dts = new SQLiteParameter("@Via3_dts", item.Via3_dts);
            parColl.Add(Via3_dts);

            SQLiteParameter Pagamenti_dts = new SQLiteParameter("@Pagamenti_dts", item.Pagamenti_dts);
            parColl.Add(Pagamenti_dts);


            SQLiteParameter ricfatt_dts = new SQLiteParameter("@ricfatt_dts", item.ricfatt_dts);
            parColl.Add(ricfatt_dts);
            SQLiteParameter noteriservate_dts = new SQLiteParameter("@noteriservate_dts", item.noteriservate_dts);
            parColl.Add(noteriservate_dts);
            SQLiteParameter indirizzofatt_dts = new SQLiteParameter("@indirizzofatt_dts", item.indirizzofatt_dts);
            parColl.Add(indirizzofatt_dts);

            SQLiteParameter niscrordine_dts = new SQLiteParameter("@niscrordine_dts", item.niscrordine_dts);
            parColl.Add(niscrordine_dts);
            SQLiteParameter locordine_dts = new SQLiteParameter("@locordine_dts", item.locordine_dts);
            parColl.Add(locordine_dts);
            SQLiteParameter annofrequenza_dts = new SQLiteParameter("@annofrequenza_dts", item.annofrequenza_dts);
            parColl.Add(annofrequenza_dts);
            SQLiteParameter nomeuniversita_dts = new SQLiteParameter("@nomeuniversita_dts", item.nomeuniversita_dts);
            parColl.Add(nomeuniversita_dts);
            SQLiteParameter dettagliuniversita_dts = new SQLiteParameter("@dettagliuniversita_dts", item.dettagliuniversita_dts);
            parColl.Add(dettagliuniversita_dts);
            SQLiteParameter Boolfields_dts = new SQLiteParameter("@Boolfields_dts", item.Boolfields_dts);
            parColl.Add(Boolfields_dts);
            SQLiteParameter Textfield1_dts = new SQLiteParameter("@Textfield1_dts", item.Textfield1_dts);
            parColl.Add(Textfield1_dts);
            SQLiteParameter Interventieseguiti_dts = new SQLiteParameter("@Interventieseguiti_dts", item.Interventieseguiti_dts);
            parColl.Add(Interventieseguiti_dts);
            // niscrordine_dts
            //locordine_dts
            //annofrequenza_dts
            //nomeuniversita_dts
            //dettagliuniversita_dts
            //Boolfields_dts
            //Textfield1_dts
            //Interventieseguiti_dts

            string query = "INSERT INTO " + _tblarchiviodettaglio + " ([AccettazioneStatuto_dts],[Altrespecializzazioni_dts],[Annolaurea_dts],[Annospecializzazione_dts],[Bloccoaccesso_dts],[Cap1_dts],[Cap2_dts],[Cap3_dts],[Certificazione_dts],[CodiceCOMUNE1_dts],[CodiceCOMUNE2_dts],[CodiceCOMUNE3_dts],[CodiceNAZIONE1_dts],[CodiceNAZIONE2_dts],[CodiceNAZIONE3_dts],[CodicePROVINCIA1_dts],[CodicePROVINCIA2_dts],[CodicePROVINCIA3_dts],[Datanascita_dts],[CodiceREGIONE1_dts],[CodiceREGIONE2_dts],[CodiceREGIONE3_dts],[Cognome_dts],[Emailriservata_dts],[Latitudine1_dts],Latitudine2_dts,Latitudine3_dts,Longitudine1_dts,Longitudine2_dts,Longitudine3_dts,Nome_dts,Nomeposizione1_dts,Nomeposizione2_dts,Nomeposizione3_dts,Pivacf_dts,Socioaltraassociazione_dts,SocioIsaps_dts,Sociopresentatore1_dts,Sociopresentatore2_dts,SocioSicpre_dts,Telefono1_dts,Telefono2_dts,Telefono3_dts,Telefonoprivato_dts,Trattamenticollegati_dts,Via1_dts,Via2_dts,Via3_dts,Pagamenti_dts,ricfatt_dts,noteriservate_dts,indirizzofatt_dts,niscrordine_dts,locordine_dts,annofrequenza_dts,nomeuniversita_dts,dettagliuniversita_dts,Boolfields_dts,Textfield1_dts,Interventieseguiti_dts) VALUES (@AccettazioneStatuto_dts,@Altrespecializzazioni_dts,@Annolaurea_dts,@Annospecializzazione_dts,@Bloccoaccesso_dts,@Cap1_dts,@Cap2_dts,@Cap3_dts,@Certificazione_dts,@CodiceCOMUNE1_dts,@CodiceCOMUNE2_dts,@CodiceCOMUNE3_dts,@CodiceNAZIONE1_dts,@CodiceNAZIONE2_dts,@CodiceNAZIONE3_dts,@CodicePROVINCIA1_dts,@CodicePROVINCIA2_dts,@CodicePROVINCIA3_dts,@Datanascita_dts,@CodiceREGIONE1_dts,@CodiceREGIONE2_dts,@CodiceREGIONE3_dts,@Cognome_dts,@Emailriservata_dts,@Latitudine1_dts,@Latitudine2_dts,@Latitudine3_dts,@Longitudine1_dts,@Longitudine2_dts,@Longitudine3_dts,@Nome_dts,@Nomeposizione1_dts,@Nomeposizione2_dts,@Nomeposizione3_dts,@Pivacf_dts,@Socioaltraassociazione_dts,@SocioIsaps_dts,@Sociopresentatore1_dts,@Sociopresentatore2_dts,@SocioSicpre_dts,@Telefono1_dts,@Telefono2_dts,@Telefono3_dts,@Telefonoprivato_dts,@Trattamenticollegati_dts,@Via1_dts,@Via2_dts,@Via3_dts,@Pagamenti_dts,@ricfatt_dts,@noteriservate_dts,@indirizzofatt_dts,@niscrordine_dts,@locordine_dts,@annofrequenza_dts,@nomeuniversita_dts,@dettagliuniversita_dts,@Boolfields_dts,@Textfield1_dts,@Interventieseguiti_dts)";
            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                item.Id_dts_collegato = lastidentity; //Inserisco nell'id dell'elemento inseito l'id generato dal db per il record collegato
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento Offerte :" + error.Message, error);
            }
            return;

        }
        /// <summary>
        /// Aggiorna un record in tabella Offerte
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateOfferta(string connessione,
            Offerte item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            //SQLiteParameter p1 = new SQLiteParameter("@CodiceTIPOLOGIA", item.CodiceOfferta);//OleDbType.VarChar
            //parColl.Add(p1);
            SQLiteParameter p1 = new SQLiteParameter("@DENOMINAZIONEI", item.DenominazioneI);
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@DENOMINAZIONEGB", item.DenominazioneGB);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@DescrizioneI", item.DescrizioneI);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@DescrizioneGB", item.DescrizioneGB);
            parColl.Add(p4);
            string schema = "";
            if (item.FotoCollection_M.Schema != null)
                schema = item.FotoCollection_M.Schema;

            string valori = "";
            if (item.FotoCollection_M.Valori != null)
                valori = item.FotoCollection_M.Valori;

            SQLiteParameter pschema = new SQLiteParameter("@FotoSchema", schema);
            parColl.Add(pschema);
            SQLiteParameter pvalori = new SQLiteParameter("@FotoValori", valori);
            parColl.Add(pvalori);

            SQLiteParameter p5 = new SQLiteParameter("@CodiceCOMUNE", item.CodiceComune);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@CodicePROVINCIA", item.CodiceProvincia);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@CodiceREGIONE", item.CodiceRegione);
            parColl.Add(p7);
            SQLiteParameter p8 = new SQLiteParameter("@DATITECNICII", item.DatitecniciI);
            parColl.Add(p8);
            SQLiteParameter p9 = new SQLiteParameter("@DATITECNICIGB", item.DatitecniciGB);
            parColl.Add(p9);
            SQLiteParameter p10 = new SQLiteParameter("@EMAIL", item.Email);
            parColl.Add(p10);
            SQLiteParameter p11 = new SQLiteParameter("@FAX", item.Fax);
            parColl.Add(p11);
            SQLiteParameter p12 = new SQLiteParameter("@INDIRIZZO", item.Indirizzo);
            parColl.Add(p12);
            SQLiteParameter p13 = new SQLiteParameter("@TELEFONO", item.Telefono);
            parColl.Add(p13);
            SQLiteParameter p14 = new SQLiteParameter("@WEBSITE", item.Website);
            parColl.Add(p14);
            SQLiteParameter pdata = new SQLiteParameter("@data", dbDataAccess.CorrectDatenow(item.DataInserimento));
            //pdata.DbType = System.Data.DbType.DateTime;
            parColl.Add(pdata);
            SQLiteParameter pdata1 = new SQLiteParameter("@data1", dbDataAccess.CorrectDatenow(item.Data1));
            //pdata.DbType = System.Data.DbType.DateTime;
            parColl.Add(pdata1);


            SQLiteParameter p17 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);
            parColl.Add(p17);
            SQLiteParameter p18a = new SQLiteParameter("@CodiceCategoria", item.CodiceCategoria);
            parColl.Add(p18a);
            SQLiteParameter p18 = new SQLiteParameter("@CodiceCategoria2Liv", item.CodiceCategoria2Liv);
            parColl.Add(p18);
            SQLiteParameter p19 = new SQLiteParameter("@Prezzo", item.Prezzo);
            parColl.Add(p19);
            SQLiteParameter p20 = new SQLiteParameter("@PrezzoListino", item.PrezzoListino);
            parColl.Add(p20);
            SQLiteParameter p21 = new SQLiteParameter("@Vetrina", item.Vetrina);
            parColl.Add(p21);

            SQLiteParameter pabilc = new SQLiteParameter("@Abilitacontatto", item.Abilitacontatto);
            parColl.Add(pabilc);
            SQLiteParameter pvideo = new SQLiteParameter("@linkVideo", item.linkVideo);
            parColl.Add(pvideo);
            SQLiteParameter pcampo1i = new SQLiteParameter("@Campo1I", item.Campo1I);
            parColl.Add(pcampo1i);
            SQLiteParameter pcampoi2 = new SQLiteParameter("@Campo2I", item.Campo2I);
            parColl.Add(pcampoi2);
            SQLiteParameter pcampo1gb = new SQLiteParameter("@Campo1GB", item.Campo1GB);
            parColl.Add(pcampo1gb);
            SQLiteParameter pcampo2gb = new SQLiteParameter("@Campo2GB", item.Campo2GB);
            parColl.Add(pcampo2gb);
            SQLiteParameter pcar1i = new SQLiteParameter("@Caratteristica1", item.Caratteristica1);
            parColl.Add(pcar1i);
            SQLiteParameter pcar2i = new SQLiteParameter("@Caratteristica2", item.Caratteristica2);
            parColl.Add(pcar2i);
            SQLiteParameter pcar3i = new SQLiteParameter("@Caratteristica3", item.Caratteristica3);
            parColl.Add(pcar3i);
            SQLiteParameter pcar4i = new SQLiteParameter("@Caratteristica4", item.Caratteristica4);
            parColl.Add(pcar4i);
            SQLiteParameter pcar5i = new SQLiteParameter("@Caratteristica5", item.Caratteristica5);
            parColl.Add(pcar5i);
            SQLiteParameter pcar6i = new SQLiteParameter("@Caratteristica6", item.Caratteristica6);
            parColl.Add(pcar6i);

            SQLiteParameter pcaranno = new SQLiteParameter("@Anno", item.Anno);
            parColl.Add(pcaranno);
            SQLiteParameter parch = new SQLiteParameter("@Archiviato", item.Archiviato);
            parColl.Add(parch);
            SQLiteParameter pidcoll = new SQLiteParameter("@Id_collegato", item.Id_collegato);
            parColl.Add(pidcoll);
            SQLiteParameter pidcollsub = new SQLiteParameter("@Id_dts_collegato", item.Id_dts_collegato);
            parColl.Add(pidcollsub);


            SQLiteParameter pautore = new SQLiteParameter("@Autore", item.Autore);
            parColl.Add(pautore);
            SQLiteParameter pxmlValue = new SQLiteParameter("@xmlValue", item.Xmlvalue);
            parColl.Add(pxmlValue);


            SQLiteParameter p3ru = new SQLiteParameter("@DENOMINAZIONERU", item.DenominazioneRU);
            parColl.Add(p3ru);
            SQLiteParameter p5ru = new SQLiteParameter("@DescrizioneRU", item.DescrizioneRU);
            parColl.Add(p5ru);
            SQLiteParameter p12ru = new SQLiteParameter("@DATITECNICIRU", item.DatitecniciRU);
            parColl.Add(p12ru);
            SQLiteParameter pcampo1ru = new SQLiteParameter("@Campo1RU", item.Campo1RU);
            parColl.Add(pcampo1ru);
            SQLiteParameter pcampo2ru = new SQLiteParameter("@Campo2RU", item.Campo2RU);
            parColl.Add(pcampo2ru);
            SQLiteParameter pqtavendita = new SQLiteParameter();
            if (item.Qta_vendita == null)
                pqtavendita = new SQLiteParameter("@Qta_vendita", DBNull.Value);
            else
                pqtavendita = new SQLiteParameter("@Qta_vendita", item.Qta_vendita.Value);
            parColl.Add(pqtavendita);

            SQLiteParameter pPromozione = new SQLiteParameter("@Promozione", item.Promozione);
            parColl.Add(pPromozione);

            SQLiteParameter p16 = new SQLiteParameter("@Id", item.Id);
            parColl.Add(p16);
            string query = "UPDATE " + _tblarchivio + " SET [DENOMINAZIONEI]=@DENOMINAZIONEI , [DENOMINAZIONEGB]= @DENOMINAZIONEGB , [DescrizioneI]=@DescrizioneI , [DescrizioneGB]= @DescrizioneGB , [FotoSchema]=@FotoSchema, [FotoValori]=@FotoValori, [CodiceCOMUNE]=@CodiceCOMUNE ,[CodicePROVINCIA]=@CodicePROVINCIA , [CodiceREGIONE]= @CodiceREGIONE , [DATITECNICII]=@DATITECNICII , [DATITECNICIGB]= @DATITECNICIGB , [EMAIL]=@EMAIL , [FAX]=@FAX , [INDIRIZZO]= @INDIRIZZO , [TELEFONO]=@TELEFONO , [WEBSITE]=@WEBSITE , [Datainserimento]= @data , [Data1]= @data1, [CodiceProdotto]=@CodiceProdotto  , [CodiceCategoria]= @CodiceCategoria  , [CodiceCategoria2Liv]= @CodiceCategoria2Liv , [Prezzo]= @Prezzo ,  [PrezzoListino]= @PrezzoListino  , [Vetrina]= @Vetrina , [Abilitacontatto]= @Abilitacontatto  , [linkVideo]= @linkVideo , [Campo1I]= @Campo1I, [Campo2I]= @Campo2I, [Campo1GB]= @Campo1GB, [Campo2GB]= @Campo2GB ,[Caratteristica1]=@Caratteristica1,[Caratteristica2]=@Caratteristica2,[Caratteristica3]=@Caratteristica3,[Caratteristica4]=@Caratteristica4,[Caratteristica5]=@Caratteristica5,[Caratteristica6]=@Caratteristica6,[Anno]=@Anno, [Archiviato]=@Archiviato, [Id_collegato]=@Id_collegato, [Id_dts_collegato]=@Id_dts_collegato,[Autore]=@Autore,[Xmlvalue]=@Xmlvalue, DenominazioneRU=@DENOMINAZIONERU,DescrizioneRU=@DescrizioneRU,DATITECNICIRU=@DATITECNICIRU,Campo1RU=@Campo1RU,Campo2RU=@Campo2RU,  [Qta_vendita]=@Qta_vendita,[Promozione]=@Promozione   WHERE [Id]=@Id ";

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornamento :" + error.Message, error);
            }
            return;
        }


        public void UpdateOffertaCollegata(string connessione,
         Offerte item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter AccettazioneStatuto_dts = new SQLiteParameter("@AccettazioneStatuto_dts", item.AccettazioneStatuto_dts);//OleDbType.VarChar
            parColl.Add(AccettazioneStatuto_dts);
            SQLiteParameter Altrespecializzazioni_dts = new SQLiteParameter("@Altrespecializzazioni_dts", item.Altrespecializzazioni_dts);
            parColl.Add(Altrespecializzazioni_dts);
            SQLiteParameter Annolaurea_dts = new SQLiteParameter("@Annolaurea_dts", item.Annolaurea_dts);
            parColl.Add(Annolaurea_dts);
            SQLiteParameter Annospecializzazione_dts = new SQLiteParameter("@Annospecializzazione_dts", item.Annospecializzazione_dts);
            parColl.Add(Annospecializzazione_dts);
            SQLiteParameter Bloccoaccesso_dts = new SQLiteParameter("@Bloccoaccesso_dts", item.Bloccoaccesso_dts);
            parColl.Add(Bloccoaccesso_dts);
            SQLiteParameter Cap1_dts = new SQLiteParameter("@Cap1_dts", item.Cap1_dts);
            parColl.Add(Cap1_dts);
            SQLiteParameter Cap2_dts = new SQLiteParameter("@Cap2_dts", item.Cap2_dts);
            parColl.Add(Cap2_dts);
            SQLiteParameter Cap3_dts = new SQLiteParameter("@Cap3_dts", item.Cap3_dts);
            parColl.Add(Cap3_dts);
            SQLiteParameter Certificazione_dts = new SQLiteParameter("@Certificazione_dts", item.Certificazione_dts);
            parColl.Add(Certificazione_dts);
            SQLiteParameter CodiceCOMUNE1_dts = new SQLiteParameter("@CodiceCOMUNE1_dts", item.CodiceCOMUNE1_dts);
            parColl.Add(CodiceCOMUNE1_dts);
            SQLiteParameter CodiceCOMUNE2_dts = new SQLiteParameter("@CodiceCOMUNE2_dts", item.CodiceCOMUNE2_dts);
            parColl.Add(CodiceCOMUNE2_dts);
            SQLiteParameter CodiceCOMUNE3_dts = new SQLiteParameter("@CodiceCOMUNE3_dts", item.CodiceCOMUNE3_dts);
            parColl.Add(CodiceCOMUNE3_dts);
            SQLiteParameter CodiceNAZIONE1_dts = new SQLiteParameter("@CodiceNAZIONE1_dts", item.CodiceNAZIONE1_dts);
            parColl.Add(CodiceNAZIONE1_dts);
            SQLiteParameter CodiceNAZIONE2_dts = new SQLiteParameter("@CodiceNAZIONE2_dts", item.CodiceNAZIONE2_dts);
            parColl.Add(CodiceNAZIONE2_dts);
            SQLiteParameter CodiceNAZIONE3_dts = new SQLiteParameter("@CodiceNAZIONE3_dts", item.CodiceNAZIONE3_dts);
            parColl.Add(CodiceNAZIONE3_dts);
            SQLiteParameter CodicePROVINCIA1_dts = new SQLiteParameter("@CodicePROVINCIA1_dts", item.CodicePROVINCIA1_dts);
            parColl.Add(CodicePROVINCIA1_dts);
            SQLiteParameter CodicePROVINCIA2_dts = new SQLiteParameter("@CodicePROVINCIA2_dts", item.CodicePROVINCIA2_dts);
            parColl.Add(CodicePROVINCIA2_dts);
            SQLiteParameter CodicePROVINCIA3_dts = new SQLiteParameter("@CodicePROVINCIA3_dts", item.CodicePROVINCIA3_dts);
            parColl.Add(CodicePROVINCIA3_dts);

            SQLiteParameter Datanascita_dts = new SQLiteParameter("@Datanascita_dts", dbDataAccess.CorrectDatenow(item.Datanascita_dts));
            parColl.Add(Datanascita_dts);

            SQLiteParameter CodiceREGIONE1_dts = new SQLiteParameter("@CodiceREGIONE1_dts", item.CodiceREGIONE1_dts);
            parColl.Add(CodiceREGIONE1_dts);
            SQLiteParameter CodiceREGIONE2_dts = new SQLiteParameter("@CodiceREGIONE2_dts", item.CodiceREGIONE2_dts);
            parColl.Add(CodiceREGIONE2_dts);
            SQLiteParameter CodiceREGIONE3_dts = new SQLiteParameter("@CodiceREGIONE3_dts", item.CodiceREGIONE3_dts);
            parColl.Add(CodiceREGIONE3_dts);
            SQLiteParameter Cognome_dts = new SQLiteParameter("@Cognome_dts", item.Cognome_dts);
            parColl.Add(Cognome_dts);

            SQLiteParameter Emailriservata_dts = new SQLiteParameter("@Emailriservata_dts", item.Emailriservata_dts);
            parColl.Add(Emailriservata_dts);
            SQLiteParameter Latitudine1_dts = new SQLiteParameter("@Latitudine1_dts", item.Latitudine1_dts);
            parColl.Add(Latitudine1_dts);

            SQLiteParameter Latitudine2_dts = new SQLiteParameter("@Latitudine2_dts", item.Latitudine2_dts);
            parColl.Add(Latitudine2_dts);
            SQLiteParameter Latitudine3_dts = new SQLiteParameter("@Latitudine3_dts", item.Latitudine3_dts);
            parColl.Add(Latitudine3_dts);
            SQLiteParameter Longitudine1_dts = new SQLiteParameter("@Longitudine1_dts", item.Longitudine1_dts);
            parColl.Add(Longitudine1_dts);
            SQLiteParameter Longitudine2_dts = new SQLiteParameter("@Longitudine2_dts", item.Longitudine2_dts);
            parColl.Add(Longitudine2_dts);

            SQLiteParameter Longitudine3_dts = new SQLiteParameter("@Longitudine3_dts", item.Longitudine3_dts);
            parColl.Add(Longitudine3_dts);
            SQLiteParameter Nome_dts = new SQLiteParameter("@Nome_dts", item.Nome_dts);
            parColl.Add(Nome_dts);
            SQLiteParameter Nomeposizione1_dts = new SQLiteParameter("@Nomeposizione1_dts", item.Nomeposizione1_dts);
            parColl.Add(Nomeposizione1_dts);
            SQLiteParameter Nomeposizione2_dts = new SQLiteParameter("@Nomeposizione2_dts", item.Nomeposizione2_dts);
            parColl.Add(Nomeposizione2_dts);
            SQLiteParameter Nomeposizione3_dts = new SQLiteParameter("@Nomeposizione3_dts", item.Nomeposizione3_dts);
            parColl.Add(Nomeposizione3_dts);
            SQLiteParameter Pivacf_dts = new SQLiteParameter("@Pivacf_dts", item.Pivacf_dts);
            parColl.Add(Pivacf_dts);

            SQLiteParameter Socioaltraassociazione_dts = new SQLiteParameter("@Socioaltraassociazione_dts", item.Socioaltraassociazione_dts);
            parColl.Add(Socioaltraassociazione_dts);
            SQLiteParameter SocioIsaps_dts = new SQLiteParameter("@SocioIsaps_dts", item.SocioIsaps_dts);
            parColl.Add(SocioIsaps_dts);

            SQLiteParameter Sociopresentatore1_dts = new SQLiteParameter("@Sociopresentatore1_dts", item.Sociopresentatore1_dts);
            parColl.Add(Sociopresentatore1_dts);

            SQLiteParameter Sociopresentatore2_dts = new SQLiteParameter("@Sociopresentatore2_dts", item.Sociopresentatore2_dts);
            parColl.Add(Sociopresentatore2_dts);

            SQLiteParameter SocioSicpre_dts = new SQLiteParameter("@SocioSicpre_dts", item.SocioSicpre_dts);
            parColl.Add(SocioSicpre_dts);
            SQLiteParameter Telefono1_dts = new SQLiteParameter("@Telefono1_dts", item.Telefono1_dts);
            parColl.Add(Telefono1_dts);
            SQLiteParameter Telefono2_dts = new SQLiteParameter("@Telefono2_dts", item.Telefono2_dts);
            parColl.Add(Telefono2_dts);
            SQLiteParameter Telefono3_dts = new SQLiteParameter("@Telefono3_dts", item.Telefono3_dts);
            parColl.Add(Telefono3_dts);
            SQLiteParameter Telefonoprivato_dts = new SQLiteParameter("@Telefonoprivato_dts", item.Telefonoprivato_dts);
            parColl.Add(Telefonoprivato_dts);

            SQLiteParameter Trattamenticollegati_dts = new SQLiteParameter("@Trattamenticollegati_dts", item.Trattamenticollegati_dts);
            parColl.Add(Trattamenticollegati_dts);
            SQLiteParameter Via1_dts = new SQLiteParameter("@Via1_dts", item.Via1_dts);
            parColl.Add(Via1_dts);
            SQLiteParameter Via2_dts = new SQLiteParameter("@Via2_dts", item.Via2_dts);
            parColl.Add(Via2_dts);

            SQLiteParameter Via3_dts = new SQLiteParameter("@Via3_dts", item.Via3_dts);
            parColl.Add(Via3_dts);

            SQLiteParameter Pagamenti_dts = new SQLiteParameter("@Pagamenti_dts", item.Pagamenti_dts);
            parColl.Add(Pagamenti_dts);

            SQLiteParameter ricfatt_dts = new SQLiteParameter("@ricfatt_dts", item.ricfatt_dts);
            parColl.Add(ricfatt_dts);
            SQLiteParameter noteriservate_dts = new SQLiteParameter("@noteriservate_dts", item.noteriservate_dts);
            parColl.Add(noteriservate_dts);
            SQLiteParameter indirizzofatt_dts = new SQLiteParameter("@indirizzofatt_dts", item.indirizzofatt_dts);
            parColl.Add(indirizzofatt_dts);

            SQLiteParameter niscrordine_dts = new SQLiteParameter("@niscrordine_dts", item.niscrordine_dts);
            parColl.Add(niscrordine_dts);
            SQLiteParameter locordine_dts = new SQLiteParameter("@locordine_dts", item.locordine_dts);
            parColl.Add(locordine_dts);
            SQLiteParameter annofrequenza_dts = new SQLiteParameter("@annofrequenza_dts", item.annofrequenza_dts);
            parColl.Add(annofrequenza_dts);
            SQLiteParameter nomeuniversita_dts = new SQLiteParameter("@nomeuniversita_dts", item.nomeuniversita_dts);
            parColl.Add(nomeuniversita_dts);
            SQLiteParameter dettagliuniversita_dts = new SQLiteParameter("@dettagliuniversita_dts", item.dettagliuniversita_dts);
            parColl.Add(dettagliuniversita_dts);
            SQLiteParameter Boolfields_dts = new SQLiteParameter("@Boolfields_dts", item.Boolfields_dts);
            parColl.Add(Boolfields_dts);
            SQLiteParameter Textfield1_dts = new SQLiteParameter("@Textfield1_dts", item.Textfield1_dts);
            parColl.Add(Textfield1_dts);
            SQLiteParameter Interventieseguiti_dts = new SQLiteParameter("@Interventieseguiti_dts", item.Interventieseguiti_dts);
            parColl.Add(Interventieseguiti_dts);

            SQLiteParameter Id_dts = new SQLiteParameter("@Id_dts", item.Id_dts_collegato);
            parColl.Add(Id_dts);
            string query = "UPDATE " + _tblarchiviodettaglio + " SET AccettazioneStatuto_dts=@AccettazioneStatuto_dts,Altrespecializzazioni_dts=@Altrespecializzazioni_dts,Annolaurea_dts=@Annolaurea_dts,Annospecializzazione_dts=@Annospecializzazione_dts,Bloccoaccesso_dts=@Bloccoaccesso_dts,Cap1_dts=@Cap1_dts,Cap2_dts=@Cap2_dts,Cap3_dts=@Cap3_dts,Certificazione_dts=@Certificazione_dts,CodiceCOMUNE1_dts=@CodiceCOMUNE1_dts,CodiceCOMUNE2_dts=@CodiceCOMUNE2_dts,CodiceCOMUNE3_dts=@CodiceCOMUNE3_dts,CodiceNAZIONE1_dts=@CodiceNAZIONE1_dts,CodiceNAZIONE2_dts=@CodiceNAZIONE2_dts,CodiceNAZIONE3_dts=@CodiceNAZIONE3_dts,CodicePROVINCIA1_dts=@CodicePROVINCIA1_dts,CodicePROVINCIA2_dts=@CodicePROVINCIA2_dts,CodicePROVINCIA3_dts=@CodicePROVINCIA3_dts,Datanascita_dts=@Datanascita_dts,CodiceREGIONE1_dts=@CodiceREGIONE1_dts,CodiceREGIONE2_dts=@CodiceREGIONE2_dts,CodiceREGIONE3_dts=@CodiceREGIONE3_dts,Cognome_dts=@Cognome_dts,Emailriservata_dts=@Emailriservata_dts,Latitudine1_dts=@Latitudine1_dts,Latitudine2_dts=@Latitudine2_dts,Latitudine3_dts=@Latitudine3_dts,Longitudine1_dts=@Longitudine1_dts,Longitudine2_dts=@Longitudine2_dts,Longitudine3_dts=@Longitudine3_dts,Nome_dts=@Nome_dts,Nomeposizione1_dts=@Nomeposizione1_dts,Nomeposizione2_dts=@Nomeposizione2_dts,Nomeposizione3_dts=@Nomeposizione3_dts,Pivacf_dts=@Pivacf_dts,Socioaltraassociazione_dts=@Socioaltraassociazione_dts,SocioIsaps_dts=@SocioIsaps_dts,Sociopresentatore1_dts=@Sociopresentatore1_dts,Sociopresentatore2_dts=@Sociopresentatore2_dts,SocioSicpre_dts=@SocioSicpre_dts,Telefono1_dts=@Telefono1_dts,Telefono2_dts=@Telefono2_dts,Telefono3_dts=@Telefono3_dts,Telefonoprivato_dts=@Telefonoprivato_dts,Trattamenticollegati_dts=@Trattamenticollegati_dts,Via1_dts=@Via1_dts,Via2_dts=@Via2_dts,Via3_dts=@Via3_dts,Pagamenti_dts=@Pagamenti_dts,ricfatt_dts=@ricfatt_dts,noteriservate_dts=@noteriservate_dts,indirizzofatt_dts=@indirizzofatt_dts, niscrordine_dts=@niscrordine_dts, locordine_dts=@locordine_dts,annofrequenza_dts=@annofrequenza_dts, nomeuniversita_dts=@nomeuniversita_dts,dettagliuniversita_dts=@dettagliuniversita_dts, Boolfields_dts=@Boolfields_dts, Textfield1_dts=@Textfield1_dts, Interventieseguiti_dts=@Interventieseguiti_dts WHERE [Id_dts]=@Id_dts ";

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornamento :" + error.Message, error);
            }
            return;

        }
        public void DeleteOfferta(string connessione,
                 Offerte item)
        {

            try
            {
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                if (connessione == null || connessione == "") return;
                if (item == null || item.Id == 0) return;
                SQLiteParameter p1 = new SQLiteParameter("@id", item.Id);//OleDbType.VarChar
                parColl.Add(p1);
                string query = "DELETE FROM " + _tblarchivio + " WHERE ([ID]=@id)";
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);

                if (item.Id_dts_collegato != 0)
                {
                    List<SQLiteParameter> parCollsub = new List<SQLiteParameter>();
                    SQLiteParameter pidsub = new SQLiteParameter("@id_dts", item.Id_dts_collegato);//OleDbType.VarChar
                    parCollsub.Add(pidsub);
                    query = "DELETE FROM " + _tblarchiviodettaglio + " WHERE ([ID_DTS]=@id_dts)";
                    dbDataAccess.ExecuteStoredProcListOle(query, parCollsub, connessione);
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione  :" + error.Message, error);
            }
            return;
        }


        public string AggiornaCaratteristicaDaFile(string connessione, string tablename, string inputfile)
        {
            string ret = "";
            string line = "";
            System.IO.FileStream fs = new System.IO.FileStream(inputfile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            fs.Close();
            fs.Dispose();
            // Read the file and display it line by line.
            using (System.IO.StreamReader file =
                new System.IO.StreamReader(inputfile))
            {
                while ((line = file.ReadLine()) != null)
                {
                    string lineread = line;
                }
                file.Close();
            }

            return ret;
        }

        /// <summary>
        /// Inserisce o aggiorna i dati nel db per le caratteristiche delle offerte
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InserisciAggiornaCaratteristica(string connessione, Tabrif item, string tablename)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            SQLiteParameter p1 = new SQLiteParameter("@Descrizione", item.Campo1);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p21 = new SQLiteParameter("@CodiceTipo", item.Codice);
            parColl.Add(p21);
            SQLiteParameter p8 = new SQLiteParameter("@Lingua", item.Lingua);//OleDbType.VarChar
            parColl.Add(p8);

            SQLiteParameter pclink = new SQLiteParameter("@relatedCodiceTipo", item.Campo2);//OleDbType.VarChar
            parColl.Add(pclink);

            SQLiteParameter ps1 = new SQLiteParameter("@Spare1", item.Campo3);//OleDbType.VarChar
            parColl.Add(ps1);
            SQLiteParameter ps2 = new SQLiteParameter("@Spare2", item.Campo4);//OleDbType.VarChar
            parColl.Add(ps2);
            SQLiteParameter ps3 = new SQLiteParameter("@Spare3", item.Campo5);//OleDbType.VarChar
            parColl.Add(ps3);
            SQLiteParameter ps4 = new SQLiteParameter("@Spare4", item.Campo6);//OleDbType.VarChar
            parColl.Add(ps4);
            SQLiteParameter ps5 = new SQLiteParameter("@Spare5", item.Campo7);//OleDbType.VarChar
            parColl.Add(ps5);

            string query = "";
            if (item.Id != "")
            {
                //UPdate
                query = "UPDATE " + tablename + " SET Descrizione=@Descrizione,CodiceTipo=@CodiceTipo,Lingua=@Lingua,RelatedCodiceTipo = @RelatedCodiceTipo ";
                query += ",Spare1=@Spare1,Spare2=@Spare2,Spare3=@Spare3,Spare4=@Spare4,Spare5=@Spare5 ";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO  " + tablename + " (Descrizione,CodiceTipo,Lingua,RelatedCodiceTipo,Spare1,Spare2,Spare3,Spare4,Spare5)";
                query += " values ( ";
                query += "@Descrizione,@CodiceTipo,@Lingua,@RelatedCodiceTipo,@Spare1,@Spare2,@Spare3,@Spare4,@Spare5 )";
            }

            try
            {
                long retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                if (item.Id == "") item.Id = retID.ToString(); // se era insert memorizzo l'id del cliente appena inserito

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento caratteristiche offerta :" + error.Message, error);
            }
            return;
        }


        //Cancella una caratteristica controllando se utilizzata o meno
        public string DeleteCaratteristica(string connessione, Tabrif item, string tablename)
        {
            string ret = "";
            string testocaratteristica = tablename.Replace("dbo_TBLRIF_", "");
            List<string> idused = CaricaListaIdCaratteristiche(connessione, "", testocaratteristica);
            if (item == null || item.Codice == "") return "";

            if (idused.Exists(i => i == item.Codice)) return "Codice utilizzato, rimovere i riferimenti prima della cancellazione.";

            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter pcod = new SQLiteParameter("@Codice", item.Codice);//OleDbType.VarChar
            parColl.Add(pcod);
            string query = "DELETE FROM " + tablename + " WHERE Codice=@Codice";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione caratteristica  :" + error.Message, error);
            }
            return ret;
        }

        /// <summary>
        /// Carica la lista delle caratteristiche distinte presenti in base ai valori dei record presenti nella tabella articoli
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<string> CaricaListaIdCaratteristiche(string connection, string codiceTipologia = "", string Campocaratteristica = "Caratteristica1")
        {
            List<string> list = new List<string>();
            if (connection == null || connection == "") return list;
            List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();
            try
            {
                string query = "SELECT DISTINCT " + Campocaratteristica + " FROM " + Tblarchivio;

                //if (!string.IsNullOrEmpty(codiceTipologia))
                //{
                //    SQLiteParameter ptip = new SQLiteParameter("@CodiceTIPOLOGIA", codiceTipologia);
                //    _parUsed.Add(ptip);
                //    if (!query.ToLower().Contains("where"))
                //        query += " WHERE CodiceTIPOLOGIA like @CodiceTIPOLOGIA ";
                //    else
                //        query += " AND CodiceTIPOLOGIA like @CodiceTIPOLOGIA  ";
                //}
                query += " order BY " + Campocaratteristica + " COLLATE NOCASE asc ";

                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        // item = new Offerte();
                        string caratteristica = "";
                        if (!reader[Campocaratteristica].Equals(DBNull.Value))
                            caratteristica = reader.GetInt64(reader.GetOrdinal(Campocaratteristica)).ToString();
                        if (!list.Exists(c => c.ToString().ToLower() == caratteristica.ToLower()))
                        {
                            list.Add(caratteristica);
                        }

                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Lettura tabella articoli :" + error.Message, error);
            }

            return list;
        }

        public string estraititolo(Offerte item, string Lingua)
        {
            if (item == null) return "";
            string testotitolo = item.DenominazionebyLingua(Lingua);


            string titolo1 = testotitolo.ToString();
            string titolo2 = "";
            int i = testotitolo.ToString().IndexOf("\n");
            if (i != -1)
            {
                titolo1 = testotitolo.ToString().Substring(0, i);
                if (testotitolo.ToString().Length >= i + 1)
                    titolo2 = testotitolo.ToString().Substring(i + 1);
            }
            return titolo1;
        }

        #region GESTIONE CATEGORIE E SOTTOCATEGORIE


        /// <summary>
        /// Carica e restituisce dalla tbl riperimento prodotto l'ultimo creato ( è il codice categoria )
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
                string query = "SELECT * FROM dbo_TBLRIF_PRODOTTO order BY CodiceProdotto COLLATE NOCASE Desc limit 1";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
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
        /// Funzione che prende l'ultimo codice presente nel database e ne crea uno aggiuntivo per le categorie 2 liv
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
        /// torna l'ultimo sottoprodotto ( cat 2 livello ) dalla tabella di riferimento
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
                string query = "SELECT * FROM dbo_TBLRIF_SOTTOPRODOTTO order BY CodiceSottoProdotto COLLATE NOCASE Desc limit 1";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
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
        /// Aggiorna un record in tabella riferimento categorie 1 liv ( prodotto )
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateProdotto(string connessione,
            Prodotto item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            if (item.Lingua == "I")
            {
                SQLiteParameter p1 = new SQLiteParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@Descrizione", item.Descrizione);
                parColl.Add(p2);
                SQLiteParameter p3 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Lingua", item.Lingua);
                parColl.Add(p4);


                string query = "UPDATE dbo_TBLRIF_PRODOTTO SET [CodiceTipologia]=@CodiceTipologia , [Descrizione]=@Descrizione WHERE CodiceProdotto=@CodiceProdotto AND Lingua= @Lingua ";


                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento categoria prodotto in Italiano :" + error.Message, error);
                }
            }
            else
            {
                SQLiteParameter p1 = new SQLiteParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@Descrizione", item.Descrizione);
                parColl.Add(p2);
                SQLiteParameter p3 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Lingua", item.Lingua);
                parColl.Add(p4);


                string query = "UPDATE dbo_TBLRIF_PRODOTTO SET [CodiceTipologia]=@CodiceTipologia , [Descrizione]=@Descrizione WHERE CodiceProdotto=@CodiceProdotto AND Lingua= @Lingua";


                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento categoria prodotto in Inglese :" + error.Message, error);
                }
            }
            return;
        }

        /// <summary>
        /// Inserisce un record in tabella riferimento categorie 1 liv
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertProdotto(string connessione, Prodotto item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            if (item.Lingua == "I")
            {
                SQLiteParameter p1 = new SQLiteParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p3 = new SQLiteParameter("@Lingua", item.Lingua);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Descrizione", item.Descrizione);
                parColl.Add(p4);
                string nuovocodiceprodotto = CreareCodiceAggiornatoProdotto(connessione);
                SQLiteParameter p2 = new SQLiteParameter("@CodiceProdotto", nuovocodiceprodotto);
                parColl.Add(p2);
                item.CodiceProdotto = nuovocodiceprodotto;
            }
            else
            {
                //se il prodotto non è in italiano, siamo al secodno inserimento quindi nell'altra lingua e gli devo assegnare lo stesso codice prodotto
                Prodotto tmp_item = CaricaUltimoProdotto(connessione);
                SQLiteParameter p1 = new SQLiteParameter("@CodiceTIPOLOGIA", item.CodiceTipologia);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p3 = new SQLiteParameter("@Lingua", item.Lingua);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Descrizione", item.Descrizione);
                parColl.Add(p4);
                SQLiteParameter p2 = new SQLiteParameter("@CodiceProdotto", tmp_item.CodiceProdotto);
                parColl.Add(p2);
            }


            string query = "INSERT INTO dbo_TBLRIF_PRODOTTO ([CodiceTipologia],[Lingua],[Descrizione],[CodiceProdotto]) VALUES (@CodiceTIPOLOGIA,@Lingua,@Descrizione,@CodiceProdotto)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento categoria Prodotto :" + error.Message, error);
            }

            return;
        }

        /// <summary>
        /// Cancella dalla tabella di riferimento prodotti col codice indicato e anche dalla tabella sottoprodotti
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void DeleteProdotto(string connessione,
          Prodotto item, bool controllapresenza = true)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "" || string.IsNullOrEmpty(item.CodiceProdotto)) return;

            if (controllapresenza)
            {
                //Verifichiamo che nella categoria prodotti non siano presenti prodotti in quella categoria/sottocategoria
                SQLiteParameter pprov = new SQLiteParameter("@CodicePROVINCIA", "%");
                parColl.Add(pprov);
                SQLiteParameter pcom = new SQLiteParameter("@CodiceCOMUNE", "%");
                parColl.Add(pcom);
                SQLiteParameter ptip = new SQLiteParameter("@CodiceTIPOLOGIA", "%");
                parColl.Add(ptip);
                SQLiteParameter preg = new SQLiteParameter("@CodiceREGIONE", "%");
                parColl.Add(preg);
                SQLiteParameter prmin = new SQLiteParameter("@PrezzoMin", "0");
                parColl.Add(prmin);
                SQLiteParameter prmax = new SQLiteParameter("@PrezzoMax", double.MaxValue);
                parColl.Add(prmax);
                SQLiteParameter pcat = new SQLiteParameter("@CodiceCategoria", item.CodiceProdotto);
                parColl.Add(pcat);
                SQLiteParameter pcat2 = new SQLiteParameter("@CodiceCategoria2Liv", "%");
                parColl.Add(pcat2);
                OfferteCollection offerte = this.CaricaOfferteFiltrate(connessione, parColl);
                if (offerte != null && offerte.Count > 0)
                {
                    throw new ApplicationException("Errore, cancellazione Prodotto  : Presenti prodotti con la categoria selezionata, rimuovere prima della cancellazione", null);
                }
            }

            //Qui devi cancellare dalle tabelle prodotto e dalla tabella sottoprodotti ...
            parColl = new List<SQLiteParameter>();
            SQLiteParameter p1prod = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
            parColl.Add(p1prod);
            string query = "DELETE FROM dbo_TBLRIF_PRODOTTO WHERE Codiceprodotto=@Codiceprodotto";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione SottoProdotto  :" + error.Message, error);
            }

            parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
            parColl.Add(p1);
            query = "DELETE FROM dbo_TBLRIF_SOTTOPRODOTTO WHERE Codiceprodotto=@Codiceprodotto";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione SottoProdotto  :" + error.Message, error);
            }


            return;
        }


        /// <summary>
        /// Inserisce un record in tabella Categorie 2 livello ( sottoprodotti )
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InsertSottoProdotto(string connessione, SProdotto item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            if (item.Lingua == "I")
            {
                SQLiteParameter p1 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p3 = new SQLiteParameter("@Lingua", item.Lingua);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Descrizione", item.Descrizione);
                parColl.Add(p4);
                string nuovocodicesottoprodotto = CreareCodiceAggiornatoSottoprodotto(connessione);
                SQLiteParameter p2 = new SQLiteParameter("@CodiceSottoprodotto", CreareCodiceAggiornatoSottoprodotto(connessione));
                parColl.Add(p2);
                item.CodiceSProdotto = nuovocodicesottoprodotto;
            }
            else
            {
                //se il prodotto non è in italiano, siamo al secodno inserimento quindi nell'altra lingua e gli devo assegnare lo stesso codice prodotto
                SProdotto tmp_item = CaricaUltimoSottoProdotto(connessione);
                SQLiteParameter p1 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p3 = new SQLiteParameter("@Lingua", item.Lingua);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Descrizione", item.Descrizione);
                parColl.Add(p4);
                SQLiteParameter p2 = new SQLiteParameter("@CodiceSottoprodotto", tmp_item.CodiceSProdotto);
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
        /// Cancella un record in tabella di riferimento dei Sottoprodotti ( categoria 2liv )
        /// controllando che non ci siano prodotti in tale sottocategoria nella TBL_ATTIVITA
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void DeleteSottoProdotto(string connessione,
            SProdotto item, bool controllapresenza = true)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "" || string.IsNullOrEmpty(item.CodiceProdotto) || string.IsNullOrEmpty(item.CodiceSProdotto)) return;

            if (controllapresenza)
            {
                //Verifichiamo che nella categoria sottoprodotti non siano presenti prodotti in quella categoria/sottocategoria
                SQLiteParameter pprov = new SQLiteParameter("@CodicePROVINCIA", "%");
                parColl.Add(pprov);
                SQLiteParameter pcom = new SQLiteParameter("@CodiceCOMUNE", "%");
                parColl.Add(pcom);
                SQLiteParameter ptip = new SQLiteParameter("@CodiceTIPOLOGIA", "%");
                parColl.Add(ptip);
                SQLiteParameter preg = new SQLiteParameter("@CodiceREGIONE", "%");
                parColl.Add(preg);
                SQLiteParameter prmin = new SQLiteParameter("@PrezzoMin", "0");
                parColl.Add(prmin);
                SQLiteParameter prmax = new SQLiteParameter("@PrezzoMax", double.MaxValue);
                parColl.Add(prmax);
                SQLiteParameter pcat = new SQLiteParameter("@CodiceCategoria", item.CodiceProdotto);
                parColl.Add(pcat);
                SQLiteParameter pcat2 = new SQLiteParameter("@CodiceCategoria2Liv", item.CodiceSProdotto);
                parColl.Add(pcat2);
                OfferteCollection offerte = this.CaricaOfferteFiltrate(connessione, parColl);
                if (offerte != null && offerte.Count > 0)
                {
                    throw new ApplicationException("Errore, cancellazione SottoProdotto  : Presenti prodotti con la sottocategoria selezionata, rimuovere prima della cancellazione", null);
                }
            }

            parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p3 = new SQLiteParameter("@CodiceSottoprodotto", item.CodiceSProdotto);
            parColl.Add(p3);
            string query = "DELETE FROM dbo_TBLRIF_SOTTOPRODOTTO WHERE Codiceprodotto=@Codiceprodotto AND CodiceSottoprodotto=@CodiceSottoprodotto  ";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione SottoProdotto  :" + error.Message, error);
            }


            return;
        }


        /// <summary>
        /// Aggiorna un record in tabella Sottoprodotto
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void UpdateSottoProdotto(string connessione,
            SProdotto item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            if (item.Lingua == "I")
            {
                SQLiteParameter p1 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@Descrizione", item.Descrizione);
                parColl.Add(p2);
                SQLiteParameter p3 = new SQLiteParameter("@CodiceSottoprodotto", item.CodiceSProdotto);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Lingua", item.Lingua);
                parColl.Add(p4);


                string query = "UPDATE dbo_TBLRIF_SOTTOPRODOTTO SET [CodiceProdotto]=@CodiceProdotto , [Descrizione]=@Descrizione WHERE CodiceSottoprodotto=@CodiceSottoprodotto AND Lingua= @Lingua ";


                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento SottoProdotto in Italiano :" + error.Message, error);
                }
            }
            else
            {
                SQLiteParameter p1 = new SQLiteParameter("@CodiceProdotto", item.CodiceProdotto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@Descrizione", item.Descrizione);
                parColl.Add(p2);
                SQLiteParameter p3 = new SQLiteParameter("@CodiceSottoprodotto", item.CodiceSProdotto);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Lingua", item.Lingua);
                parColl.Add(p4);


                string query = "UPDATE dbo_TBLRIF_SOTTOPRODOTTO SET [CodiceProdotto]=@CodiceProdotto , [Descrizione]=@Descrizione WHERE CodiceSottoprodotto=@CodiceSottoprodotto AND Lingua= @Lingua";


                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento categoria 2liv SottoProdotto in Inglese :" + error.Message, error);
                }
            }
            return;
        }

        #endregion

        #region CREAZIONE FEED

        /// <summary>
        /// Wrapper per lingua italiana per la creazione del feed in italiano
        /// </summary>
        public void CreaRssFeed_I()
        {
            CreaRssFeed("I");
        }
        public void CreaRssFeedPerCategoria_I()
        {
            foreach (TipologiaOfferte item in Utility.TipologieOfferte)
            {
                CreaRssFeed("I", item.Codice);
                CreaRssFeed("I", item.Codice, "", "", true);
            }
        }
        /// <summary>
        /// Wrapper per lingua inglese per la creazione del feed in inglese
        /// </summary>
        public void CreaRssFeed_GB()
        {
            CreaRssFeed("GB");
        }
        public void CreaRssFeedPerCategoria_GB()
        {
            foreach (TipologiaOfferte item in Utility.TipologieOfferte)
            {
                CreaRssFeed("GB", item.Codice);
            }
        }
        /// <summary>
        /// Wrapper per lingua inglese per la creazione del feed in inglese
        /// </summary>
        public void CreaRssFeed_RU()
        {
            CreaRssFeed("RU");
        }
        public void CreaRssFeedPerCategoria_RU()
        {
            foreach (TipologiaOfferte item in Utility.TipologieOfferte)
            {
                CreaRssFeed("RU", item.Codice);
            }
        }

        /// <summary>
        /// Creo un feed rss con tutti gli immobili per ogni lingua ( inglese , italiano )
        /// </summary>
        public void CreaRssFeed(string Lng, string FiltroTipologia = "", string titolo = "", string descrizione = "", bool gmerchant = false)
        {

            titolo = (ConfigManagement.ReadKey("Nome") ?? "");
            string descrizionefeed = (ConfigManagement.ReadKey("Descrizione") ?? "");

            //string Lingua = "I";
            string Lingua = Lng;
            string titolofeed = titolo;
            TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == FiltroTipologia); });
            if (item != null)
                titolofeed += " " + item.Descrizione;

            string logfilename = "LogRss.txt";
            //string stringabase = "articoli/";
            System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();

            string PathFileXml = WelcomeLibrary.STATIC.Global.percorsoFisicoComune; //Percorsi fisico comune per l'appoggio dell'xml per il feed

            //string NomeAgenzia = ConfigManagement.ReadKey("NomeAgenzia");
            Messaggi.Add("Messaggio", "");
            Messaggi["Messaggio"] = "Creazione rss feed xml " + System.DateTime.Now.ToString() + " \r\n";

            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, logfilename);

            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            parColl = new List<SQLiteParameter>();
            offerteDM offDM = new offerteDM();
            WelcomeLibrary.DOM.OfferteCollection lista = new WelcomeLibrary.DOM.OfferteCollection();

            //Carichiamo la lista contenuto presenti 
            try
            {
                //Carichiamo in memoria tutti le ultime 1000 news ( eventualmente x tipologia )
                if (!string.IsNullOrEmpty(FiltroTipologia))
                {
                    SQLiteParameter filtrotipologia = new SQLiteParameter("@CodiceTIPOLOGIA", FiltroTipologia);
                    parColl.Add(filtrotipologia);
                }
                lista = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "1000", Lingua);
            }
            catch (Exception error)
            {
                Messaggi["Messaggio"] = " &nbsp; <br/> Errore caricamento news per feed rss: " + error.Message + " \r\n";
                WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, logfilename);
            }
            //PREPARIAMO IL FILE XML DA FORNIRE AL PORTALE
            try
            {
                //-------------------------------------------------------------------------------------------------------------
                //QUI creo L'XML PER IL PROGRAMMA DI VISUALIZZAZIONE
                //System.IO.FileStream str = new System.IO.FileStream(Server.MapPath(basevetrinadir + immobile.Codice + ".xml"), System.IO.FileMode.Create);
                string filename = PathFileXml + "\\RSSfeed" + FiltroTipologia + Lng + ".xml";
                if (gmerchant) filename = PathFileXml + "\\Merchantfeed" + FiltroTipologia + Lng + ".xml";
                System.IO.FileStream str = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                using (str)
                {
                    System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(str, System.Text.Encoding.UTF8);
                    writer.Formatting = System.Xml.Formatting.Indented;
                    // aggiungo l'intestazione XML 
                    //writer.WriteRaw("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>");
                    writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    // apro la root rss  

                    //writer.WriteStartElement("rss");
                    //writer.WriteAttributeString("xmlns:sy", "http://purl.org/rss/1.0/modules/syndication/");
                    //writer.WriteAttributeString("version", "2.0");

                    //https://support.google.com/merchants/answer/7052112?hl=en
                    writer.WriteStartElement("rss");
                    if (gmerchant)
                        writer.WriteAttributeString("xmlns:g", "http://base.google.com/ns/1.0");
                    else
                        writer.WriteAttributeString("xmlns:sy", "http://purl.org/rss/1.0/modules/syndication/");
                    writer.WriteAttributeString("version", "2.0");

                    writer.WriteStartElement("channel");
                    //Intestazione del feed
                    writer.WriteElementString("title", titolofeed);
                    writer.WriteElementString("link", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                    writer.WriteElementString("description", descrizionefeed);
                    //writer.WriteElementString("lastBuildDate", System.Xml.XmlConvert.ToString(System.DateTime.Now, "yyyy-MM-ddTHH:mm:ss+01:00"));
                    writer.WriteElementString("lastBuildDate", System.Xml.XmlConvert.ToString(System.DateTime.Now, "ddd, dd MMM yyyy HH:mm:ss 'GMT'"));
                    if (!gmerchant)
                    {
                        writer.WriteElementString("sy:updatePeriod", "daily");
                        writer.WriteElementString("sy:updateFrequency", "1");
                        writer.WriteElementString("sy:updateBase", System.Xml.XmlConvert.ToString(System.DateTime.Now, "yyy-MM-ddTHH:mmzzz"));
                    }
                    //writer.WriteElementString("language", "en-EN");
                    writer.WriteElementString("language", "it-IT");

                    //Creaiamo il feed
                    foreach (Offerte _new in lista)
                    {
                        string testotitolo = _new.DenominazionebyLingua(Lingua).ToLower();
                        string descrizioneitem = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(ReplaceLinks(WelcomeLibrary.UF.SitemapManager.ConteggioCaratteri(_new.DescrizionebyLingua(Lingua), 5000)));

                        if (_new == null || string.IsNullOrEmpty(_new.Id.ToString())) continue;
                        if (descrizioneitem == null || string.IsNullOrEmpty(descrizioneitem)) continue;

                        ////////////////////////////////////PARAMETRI BASE PER MERCHANT CENTER 
                        string gtinean = "";
                        string brand = "";
                        string gtinmpn = "";
                        string prezzo = "";
                        if (gmerchant == true) //Controllo parametri base x feed merchant!!!! id-price-brand-mpn o gtin 
                        {

                            //Cerco brand:-> codice produttore per pubblicare
                            //Cerco ean:-> codice per pubblicare
                            //Cerco mpn:-> codice per pubblicare se non presente ean
                            int start = descrizioneitem.ToLower().IndexOf("ean:");
                            if (start != -1)
                            {
                                int end1 = descrizioneitem.ToLower().IndexOf(" ", start + 5);
                                int end2 = descrizioneitem.ToLower().IndexOf("\r", start + 5);
                                int end3 = descrizioneitem.ToLower().IndexOf("\n", start + 5);
                                int end4 = descrizioneitem.ToLower().IndexOf("\r\n", start + 5);
                                //Prendiamo il minimo != -1
                                int end = -1;
                                if (end1 != -1) end = end1; if (end2 != -1) end = end2; if (end3 != -1) end = end3; if (end4 != -1) end = end4;
                                if (end1 != -1 && end1 < end) end = end1; if (end2 != -1 && end2 < end) end = end2; if (end3 != -1 && end3 < end) end = end3; if (end4 != -1 && end4 < end) end = end4;

                                if (end != -1)
                                    gtinean = descrizioneitem.Substring(start + 4, end - (start + 4)).Trim();
                            }
                            start = descrizioneitem.ToLower().IndexOf("mpn:");
                            if (start != -1)
                            {
                                int end1 = descrizioneitem.ToLower().IndexOf(" ", start + 5);
                                int end2 = descrizioneitem.ToLower().IndexOf("\r", start + 5);
                                int end3 = descrizioneitem.ToLower().IndexOf("\n", start + 5);
                                int end4 = descrizioneitem.ToLower().IndexOf("\r\n", start + 5);
                                //Prendiamo il minimo != -1
                                int end = -1;
                                if (end1 != -1) end = end1; if (end2 != -1) end = end2; if (end3 != -1) end = end3; if (end4 != -1) end = end4;
                                if (end1 != -1 && end1 < end) end = end1; if (end2 != -1 && end2 < end) end = end2; if (end3 != -1 && end3 < end) end = end3; if (end4 != -1 && end4 < end) end = end4;

                                if (end != -1)
                                    gtinmpn = descrizioneitem.Substring(start + 4, end - (start + 4)).Trim();
                            }
                            start = descrizioneitem.ToLower().IndexOf("brand:");
                            if (start != -1)
                            {
                                int end1 = descrizioneitem.ToLower().IndexOf(" ", start + 7);
                                int end2 = descrizioneitem.ToLower().IndexOf("\r", start + 7);
                                int end3 = descrizioneitem.ToLower().IndexOf("\n", start + 7);
                                int end4 = descrizioneitem.ToLower().IndexOf("\r\n", start + 7);
                                //Prendiamo il minimo != -1
                                int end = -1;
                                if (end1 != -1) end = end1; if (end2 != -1) end = end2; if (end3 != -1) end = end3; if (end4 != -1) end = end4;
                                if (end1 != -1 && end1 < end) end = end1; if (end2 != -1 && end2 < end) end = end2; if (end3 != -1 && end3 < end) end = end3; if (end4 != -1 && end4 < end) end = end4;

                                if (end != -1)
                                    brand = descrizioneitem.Substring(start + 6, end - (start + 6)).Trim();
                            }
                            start = descrizioneitem.ToLower().IndexOf("marchio:");
                            if (start != -1)
                            {
                                int end1 = descrizioneitem.ToLower().IndexOf(" ", start + 9);
                                int end2 = descrizioneitem.ToLower().IndexOf("\r", start + 9);
                                int end3 = descrizioneitem.ToLower().IndexOf("\n", start + 9);
                                int end4 = descrizioneitem.ToLower().IndexOf("\r\n", start + 9);
                                //Prendiamo il minimo != -1
                                int end = -1;
                                if (end1 != -1) end = end1; if (end2 != -1) end = end2; if (end3 != -1) end = end3; if (end4 != -1) end = end4;
                                if (end1 != -1 && end1 < end) end = end1; if (end2 != -1 && end2 < end) end = end2; if (end3 != -1 && end3 < end) end = end3; if (end4 != -1 && end4 < end) end = end4;

                                if (end != -1)
                                    brand = descrizioneitem.Substring(start + 8, end - (start + 8)).Trim();
                            }
                            start = descrizioneitem.ToLower().IndexOf("prezzo:");
                            if (start != -1)
                            {
                                int end1 = descrizioneitem.ToLower().IndexOf(" ", start + 7);
                                int end2 = descrizioneitem.ToLower().IndexOf("\r", start + 7);
                                int end3 = descrizioneitem.ToLower().IndexOf("\n", start + 7);
                                int end4 = descrizioneitem.ToLower().IndexOf("\r\n", start + 7);
                                //Prendiamo il minimo != -1
                                int end = -1;
                                if (end1 != -1) end = end1; if (end2 != -1) end = end2; if (end3 != -1) end = end3; if (end4 != -1) end = end4;
                                if (end1 != -1 && end1 < end) end = end1; if (end2 != -1 && end2 < end) end = end2; if (end3 != -1 && end3 < end) end = end3; if (end4 != -1 && end4 < end) end = end4;

                                if (end != -1)
                                    prezzo = descrizioneitem.Substring(start + 7, end - (start + 7));
                                if (_new.Prezzo == 0)
                                {
                                    double prezzodbo = 0;
                                    if (double.TryParse(prezzo, out prezzodbo))
                                    {
                                        _new.Prezzo = prezzodbo;
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(brand)) continue;
                            if (string.IsNullOrEmpty(gtinean) && string.IsNullOrEmpty(gtinmpn)) continue;
                            if (_new == null || _new.Prezzo == 0) continue;

                        }
                        ////////////////////////////////////PARAMETRI BASE PER MERCHANT CENTER 

                        writer.WriteStartElement("item");

                        //TITOLO SCHEDA
                        writer.WriteElementString("title", testotitolo.Replace("-", " "));

                        //LINK A SCHEDA
                        string UrlCompleto = "";
                        //UrlCompleto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + stringabase + _new.CodiceTipologia.Replace(" ", "_") + "_" + Lingua + "_" + _new.Id.ToString().Replace(" ", "_") + "_" + testotitolo + ".aspx";
                        UrlCompleto = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua, testotitolo, _new.Id.ToString(), _new.CodiceTipologia, _new.CodiceCategoria, "", "", "", "", true, false);
                        writer.WriteElementString("link", UrlCompleto);

                        if (!gmerchant)
                        {
                            writer.WriteStartElement("guid");
                            writer.WriteAttributeString("isPermaLink", "true");
                            writer.WriteValue(UrlCompleto);
                            writer.WriteEndElement();
                            //<pubDate>
                            writer.WriteStartElement("pubDate");
                            writer.WriteValue(System.Xml.XmlConvert.ToString(_new.DataInserimento, "ddd, dd MMM yyyy HH:mm:ss zzz"));
                            writer.WriteEndElement();
                        }

                        //Categoria
                        //<category>
                        item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == _new.CodiceTipologia); });
                        if (item != null)
                        {
                            writer.WriteStartElement("category");
                            writer.WriteValue(item.Descrizione);
                            writer.WriteEndElement();
                        }

                        //DESCRIZIONE
                        string linkimmagine = ComponiUrlAnteprima(_new.FotoCollection_M.FotoAnteprima, _new.CodiceTipologia, _new.Id.ToString()).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                        ///////////////////////////////////////////PER FEED MERCHANT GOOGLE //////////////////////////////////////

                        if (gmerchant)
                        {
                            writer.WriteStartElement("g:image_link");
                            writer.WriteValue(linkimmagine);
                            writer.WriteEndElement();
                            //if ((_new != null) && (_new.FotoCollection_M.Count > 1))
                            //{
                            //    foreach (Allegato a in _new.FotoCollection_M)
                            //    {
                            //        if ((a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                            //        {
                            //            //IMMAGINE
                            //            string tmppathimmagine = ComponiUrlAnteprima(a.NomeFile, _new.CodiceTipologia, _new.Id.ToString());
                            //            string abspathimmagine = tmppathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                            //            if (abspathimmagine != linkimmagine)
                            //            {
                            //                writer.WriteStartElement("g:additional_​​​image_​​​link");
                            //                writer.WriteValue(abspathimmagine);
                            //                writer.WriteEndElement();
                            //            }
                            //        }
                            //    }
                            //}
                            //BRAND
                            writer.WriteStartElement("g:brand");
                            writer.WriteValue(brand);
                            writer.WriteEndElement();
                            //CODIE  EAN / ISBN / UPC / JAN / ITF-14
                            if (!string.IsNullOrEmpty(gtinean))
                            {
                                writer.WriteStartElement("g:gtin");
                                writer.WriteValue(gtinean);
                                writer.WriteEndElement();
                            }
                            //MANUFACTURER PART NUMBER
                            if (!string.IsNullOrEmpty(gtinmpn))
                            {
                                writer.WriteStartElement("g:mpn");
                                writer.WriteValue(gtinmpn);
                                writer.WriteEndElement();
                            }
                            //Specifico che non fornisco codice gtin o mpn per il prodotto ( che sarebbero meglio)
                            //writer.WriteStartElement("g:identifier_​exists");
                            //writer.WriteValue("no"); //yes
                            //writer.WriteEndElement();
                            //PRODUCT CATEGORY!!!!! ( DA PERSONALIZZARE IN BASE AL SETTORE QUI METTO PER IL SITO ATTUALE )
                            //writer.WriteStartElement("g:google_​​product_​​category");
                            //writer.WriteValue("276"); //Codice o desrizione google taxonomy ( quesot è per le batterie )
                            //writer.WriteEndElement();
                            if (_new.Prezzo != 0)
                            {
                                writer.WriteStartElement("g:price");
                                writer.WriteValue(_new.Prezzo.ToString() + " EUR");
                                writer.WriteEndElement();
                            }
                            ////////////////////////////////////////////////////////////////
                            //                         < g:shipping >


                            //< g:country > US </ g:country >


                            //     < g:region > MA </ g:region >


                            //          < g:service > Ground </ g:service >


                            //               < g:price > 6.49 USD </ g:price >


                            //                  </ g:shipping >

                            writer.WriteStartElement("g:shipping");


                            writer.WriteStartElement("g:country");
                            writer.WriteValue("IT"); //prezzo spedizione
                            writer.WriteEndElement();
                            writer.WriteStartElement("g:price");
                            writer.WriteValue("0.00 EUR"); //prezzo spedizione
                            writer.WriteEndElement();

                            writer.WriteEndElement();



                            writer.WriteStartElement("g:availability");
                            writer.WriteValue("in stock"); //out of stock | preorder
                            writer.WriteEndElement();
                            writer.WriteStartElement("g:condition");
                            writer.WriteValue("nuovo"); //new refurbished used ( o nuovo ricondizionato usato )
                            writer.WriteEndElement();
                            writer.WriteStartElement("g:id");
                            writer.WriteValue(_new.Id);
                            writer.WriteEndElement();
                            ////////////////////////////////// aggiungere eventuali altri come la marca o altro...
                            //////////////////////////////////////////////////////////////////////////////////////////////////////FINE MERCHANT
                        }
                        StringBuilder sb = new StringBuilder();
                        if (_new.FotoCollection_M != null && !gmerchant)
                            sb.Append("<img style=\"margin-right: 10px; float: left\" src=\"" + linkimmagine + "\" alt=\"" + testotitolo + "\" width=\"350\" />");

                        sb.Append("<p>" + ReplaceLinks(descrizioneitem) + "</p>");

                        if (!gmerchant)
                            sb.Append("<p>Continua a leggere / Read More <a href=\"" + UrlCompleto + "\"><em>" + testotitolo + "</em></a>.</p>");
                        writer.WriteStartElement("description");
                        writer.WriteCData(sb.ToString());
                        //writer.WriteRaw("<![CDATA[Questo è un test con caratteri <>]]>");
                        writer.WriteEndElement();

                        //TAG ITEM
                        writer.WriteEndElement();
                    }

                    //Chiudo tag channel
                    writer.WriteEndElement();
                    // chiudo tag rss 
                    writer.WriteEndElement();
                    // scrivo a video e chiudo lo stream 
                    writer.Flush();
                    writer.Close();
                    str.Close();
                }
            }
            catch (Exception error)
            {
                Messaggi["Messaggio"] = " &nbsp; <br/> Errore creazione file rss xml : " + error.Message + " \r\n";
                WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, logfilename);
            }
            Messaggi["Messaggio"] = "Fine Creazione feed xml rss " + System.DateTime.Now.ToString() + " \r\n";
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, logfilename);

        }

        public static string ComponiUrlAnteprima(object NomeAnteprima, string CodiceTipologia, string idOfferta, bool noanteprima = false)
        {
            string ritorno = "";
            string physpath = "";
            if (NomeAnteprima != null)
                if (!NomeAnteprima.ToString().ToLower().StartsWith("http://") && !NomeAnteprima.ToString().ToLower().StartsWith("https://") && !NomeAnteprima.ToString().ToLower().StartsWith("https://"))
                {
                    if (CodiceTipologia != "" && idOfferta != "")
                        if ((NomeAnteprima.ToString().ToLower().EndsWith("jpg") || NomeAnteprima.ToString().ToLower().EndsWith("gif") || NomeAnteprima.ToString().ToLower().EndsWith("png")))
                        {
                            ritorno = WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + CodiceTipologia + "/" + idOfferta.ToString();
                            physpath = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + CodiceTipologia + "\\" + idOfferta.ToString();
                            //Così ritorno l'immagine non di anteprima ma quella pieno formato
                            if (NomeAnteprima.ToString().StartsWith("Ant"))
                                ritorno += "/" + NomeAnteprima.ToString().Remove(0, 3);
                            else
                                ritorno += "/" + NomeAnteprima.ToString();
                            //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME
                            //string anteprimaimmagine = filemanage.ScalaImmagine(ritorno, null, physpath);
                            //if (anteprimaimmagine != "" && !noanteprima) ritorno = anteprimaimmagine;
                            //////////////INSERITO PER LA GENERAZIONE DELLE ANTEPRIME
                        }
                        else
                            ritorno = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";
                }
                else
                    ritorno = NomeAnteprima.ToString();

            return ritorno;
        }
        /// <summary>
        /// Torna la lista dei link per id
        /// </summary>
        /// <param name="lingua"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getlinklist(string lingua, string id)
        {
            Dictionary<string, string> linklist = new Dictionary<string, string>();
            offerteDM offDM = new offerteDM();
            OfferteCollection offerte = new OfferteCollection(); List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (!id.Contains(","))
            {
                SQLiteParameter pid = new SQLiteParameter("@Id", id);
                parColl.Add(pid);
            }
            else
            {
                SQLiteParameter pid = new SQLiteParameter("@Idlist", id);
                parColl.Add(pid);
            }
            offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", lingua);
            foreach (Offerte _o in offerte)
            {
                //string target = "_blank";
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, SitemapManager.CleanUrl(_o.DenominazionebyLingua(lingua)), _o.Id.ToString(), _o.CodiceTipologia);
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && link.ToLower().IndexOf("~") == -1)
                {
                    //target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                string pathimmagine = filemanage.ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString());
                pathimmagine = filemanage.SelectImageByResolution(pathimmagine, WelcomeLibrary.STATIC.Global.Viewportw);
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                if (!linklist.ContainsKey(_o.Id.ToString()))
                    linklist.Add(_o.Id.ToString(), link);
                if (!linklist.ContainsKey(_o.Id.ToString() + "name"))
                    linklist.Add(_o.Id.ToString() + "name", _o.DenominazionebyLingua(lingua));
                if (!linklist.ContainsKey(_o.Id.ToString() + "img"))
                    linklist.Add(_o.Id.ToString() + "img", pathimmagine);

            }
            return linklist;
        }


        /// <summary>
        /// Funzione caricamento dati usata dell'handler e dalla funzione di binding
        /// </summary>
        /// <param name="lingua"></param>
        /// <param name="filtri"></param>
        /// <param name="spage"></param>
        /// <param name="spagesize"></param>
        /// <param name="senablepager"></param>
        /// <returns></returns>
        public static Dictionary<string, string> filterData(string lingua, Dictionary<string, string> filtri, string spage, string spagesize, string senablepager)
        {
            WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();

            bool gen = false;
            bool.TryParse(ConfigManagement.ReadKey("generaUrlrewrited"), out gen);

            bool enabledpager = false;
            bool.TryParse(senablepager, out enabledpager);

            int page = 0;
            int pagesize = 0;
            int.TryParse(spage, out page);
            int.TryParse(spagesize, out pagesize);

            List<Offerte> filteredData = new List<Offerte>();
            offerteDM offDM = new offerteDM();
            Dictionary<string, string> ritorno = new Dictionary<string, string>();
            OfferteCollection offerte = new OfferteCollection();


            //CARICO FILTRANDO ////////////////////////////////////////////////////////////////
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            string maxrecords = "";
            if (filtri.ContainsKey("maxelement") && !string.IsNullOrEmpty(filtri["maxelement"]))
                maxrecords = filtri["maxelement"];

            if (filtri.ContainsKey("mostviewed") && !string.IsNullOrEmpty(filtri["mostviewed"]))
            {
                long maxelements = 0;
                long.TryParse(filtri["mostviewed"], out maxelements);
                if (maxelements != 0)
                {
                    maxrecords = maxelements.ToString();
                    //estraiamo la lista degli di più visti
                    Dictionary<long, long> mostvisited = statisticheDM.ContaTutteVisite(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, filteredData, maxelements);
                    long _i = 0;
                    string idlistfiltro = "";
                    foreach (KeyValuePair<long, long> kv in mostvisited)
                    {
                        if (_i >= maxelements) break;
                        idlistfiltro += kv.Key + ",";
                        _i++;
                    }
                    idlistfiltro = idlistfiltro.TrimEnd(',');
                    if (!string.IsNullOrEmpty(idlistfiltro))
                    {
                        SQLiteParameter pidlist1 = new SQLiteParameter("@IdList", idlistfiltro);
                        parColl.Add(pidlist1);
                    }
                }
            }

            if (filtri.ContainsKey("listShow") && !string.IsNullOrEmpty(filtri["listShow"]))
            {
                if (filtri["listShow"].Contains(","))
                {
                    SQLiteParameter pidlist = new SQLiteParameter("@IdList", filtri["listShow"]);
                    parColl.Add(pidlist);
                }
            }
            if (filtri.ContainsKey("id") && !string.IsNullOrEmpty(filtri["id"]))
            {
                SQLiteParameter pid = new SQLiteParameter("@Id", filtri["id"]);
                parColl.Add(pid);
            }
            if (filtri.ContainsKey("tipologia") && !string.IsNullOrEmpty(filtri["tipologia"]))
            {
                SQLiteParameter p3 = new SQLiteParameter("@CodiceTIPOLOGIA", filtri["tipologia"]);
                parColl.Add(p3);
            }
            if (filtri.ContainsKey("categoria") && !string.IsNullOrEmpty(filtri["categoria"]))
            {
                SQLiteParameter p7 = new SQLiteParameter("@CodiceCategoria", filtri["categoria"]);
                parColl.Add(p7);
            }
            if (filtri.ContainsKey("categoria2Liv") && !string.IsNullOrEmpty(filtri["categoria2Liv"]))
            {
                SQLiteParameter p10 = new SQLiteParameter("@CodiceCategoria2Liv", filtri["categoria2Liv"]);
                parColl.Add(p10);
            }
            if (filtri.ContainsKey("caratteristica1") && !string.IsNullOrEmpty(filtri["caratteristica1"]))
            {
                SQLiteParameter pc1 = new SQLiteParameter("@Caratteristica1", filtri["caratteristica1"]);
                parColl.Add(pc1);
            }
            if (filtri.ContainsKey("caratteristica2") && !string.IsNullOrEmpty(filtri["caratteristica2"]))
            {
                SQLiteParameter pc2 = new SQLiteParameter("@Caratteristica2", filtri["caratteristica2"]);
                parColl.Add(pc2);
            }
            if (filtri.ContainsKey("caratteristica3") && !string.IsNullOrEmpty(filtri["caratteristica3"]))
            {
                SQLiteParameter pc3 = new SQLiteParameter("@Caratteristica3", filtri["caratteristica3"]);
                parColl.Add(pc3);
            }

            if (filtri.ContainsKey("regione") && !string.IsNullOrEmpty(filtri["regione"]))
            {
                SQLiteParameter preg = new SQLiteParameter("@CodiceREGIONE", filtri["regione"]);
                parColl.Add(preg);
            }


            if (filtri.ContainsKey("vetrina") && !string.IsNullOrEmpty(filtri["vetrina"]))
            {
                bool _tmpb = false;
                bool.TryParse(filtri["vetrina"], out _tmpb);
                SQLiteParameter pvet = new SQLiteParameter("@Vetrina", _tmpb);
                parColl.Add(pvet);
            }
            if (filtri.ContainsKey("promozioni") && !string.IsNullOrEmpty(filtri["promozioni"]))
            {
                bool _tmpb = false;
                bool.TryParse(filtri["promozioni"], out _tmpb);
                SQLiteParameter promo = new SQLiteParameter("@promozioni", _tmpb);
                parColl.Add(promo);
            }
            if (filtri.ContainsKey("testoricerca") && !string.IsNullOrEmpty(filtri["testoricerca"]))
            {
                string testoricerca = filtri["testoricerca"].Trim().Replace(" ", "%");
                SQLiteParameter p8 = new SQLiteParameter("@testoricerca", "%" + testoricerca + "%");
                parColl.Add(p8);
            }

            if (filtri.ContainsKey("mese") && !string.IsNullOrEmpty(filtri["mese"]))
                if (filtri.ContainsKey("anno") && !string.IsNullOrEmpty(filtri["anno"]))
                {
                    string mese = filtri["mese"];
                    string anno = filtri["anno"];
                    if (mese.Trim() != "" && anno.Trim() != "")
                    {
                        SQLiteParameter panno = new SQLiteParameter("@annofiltro", anno);
                        parColl.Add(panno);


                        SQLiteParameter pmese = new SQLiteParameter("@mesefiltro", mese);
                        parColl.Add(pmese);
                    }

#if false
                    if (mese.Trim() != "" && anno.Trim() != "")
                    {
                        int _a = 0;
                        int.TryParse(anno, out _a);
                        int _m = 0;
                        int.TryParse(mese, out _m);
                        if (_a != 0)
                        {
                            SQLiteParameter panno = new SQLiteParameter("@annofiltro", _a);
                            parColl.Add(panno);
                        }
                        if (_m != 0)
                        {
                            SQLiteParameter pmese = new SQLiteParameter("@mesefiltro", _m);
                            parColl.Add(pmese);
                        }

                    } 
#endif
                }



            if (enabledpager && page != 0 && pagesize != 0)
            {
                offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, maxrecords, lingua, null, "", false, page, pagesize);
            }
            else if (senablepager == "skip" && page != 0 && pagesize != 0)
            {
                offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", lingua, null, "", false, page, pagesize);
                int lmaxrecords = 0;
                int.TryParse(maxrecords, out lmaxrecords);
                if (offerte != null && lmaxrecords != 0)
                {
                    long nget = Math.Min(offerte.Count, lmaxrecords);
                    OfferteCollection tmpoffselect = new OfferteCollection();
                    for (int conta = 0; conta < nget; conta++)
                    {
                        tmpoffselect.Add(offerte[conta]);
                    }
                    offerte = tmpoffselect;
                    //if (lmaxrecords != 0)
                    //    offerte = new OfferteCollection(offerte.GetRange(0, Math.Min(offerte.Count, lmaxrecords)));
                }

            }
            else
                offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, maxrecords, lingua, null, "");
            //}
            //else
            //    offerte = filtri[4];


#if false
        /*Old paging method*/
        if (offerte != null && offerte.Count > 0 && enabledpager && page != 0 && pagesize != 0)
        {
            //Facciamo il take skip
            int start = ((page - 1) * pagesize);
            //int end = start + pagesize - 1;
            if (start + pagesize > offerte.Count - 1)
                filteredData = offerte.GetRange(start, offerte.Count - start);
            else
                filteredData = offerte.GetRange(start, pagesize);
        }
        else filteredData = offerte;
#endif
#if false
        if (filtri.ContainsKey("maxelement") && !string.IsNullOrEmpty(filtri["maxelement"]))
        {
            int maxelem = 0;
            int.TryParse(filtri["maxelement"], out maxelem);
            if (maxelem < filteredData.Count())
                filteredData = filteredData.GetRange(0, maxelem);
        } 
#endif

            filteredData = offerte;
            string tempOff = Newtonsoft.Json.JsonConvert.SerializeObject(filteredData, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            });
            ritorno.Add("data", tempOff); //lista dati di ritorno

            //Prepariamo anche le info utili lato client (resultinfo)
            Dictionary<string, string> ListRet = new Dictionary<string, string>();
            ListRet.Add("visualData", filtri["visualData"]);
            ListRet.Add("visualPrezzo", filtri["visualPrezzo"]);
            string tot = "0";
            if (offerte != null) tot = offerte.Totrecs.ToString();
            ListRet.Add("totalrecords", tot);
            string tempListret = Newtonsoft.Json.JsonConvert.SerializeObject(ListRet);
            ritorno.Add("resultinfo", tempListret);

            //Prepariamo un dictionary per id elemento che contiene le coppie chiave valore utili alla renderizzazione (linkloaded)
            //Carico lista statistiche visite per inserirla nella lista di ritorno
            Dictionary<long, long> visite = new Dictionary<long, long>();
            if (filteredData != null && filteredData.Count > 0)
                visite = statisticheDM.ContaTutteVisite(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, filteredData);
            Dictionary<string, Dictionary<string, string>> linksurl = new Dictionary<string, Dictionary<string, string>>();
            foreach (Offerte _o in filteredData)
            {
                Dictionary<string, string> tmp = new Dictionary<string, string>();
                string testotitolo = "";
                string descrizione = "";
                string datitecnici = "";
                switch (lingua)
                {
                    case "GB":
                        testotitolo = _o.DenominazioneGB;
                        descrizione = ReplaceLinks(WelcomeLibrary.UF.SitemapManager.ConteggioCaratteri(_o.DescrizioneGB, 30000, true));
                        datitecnici = ReplaceLinks(WelcomeLibrary.UF.SitemapManager.ConteggioCaratteri(_o.DatitecniciGB, 30000, true));
                        break;
                    default:
                        testotitolo = _o.DenominazioneI;
                        descrizione = ReplaceLinks(WelcomeLibrary.UF.SitemapManager.ConteggioCaratteri(_o.DescrizioneI, 30000, true));
                        datitecnici = ReplaceLinks(WelcomeLibrary.UF.SitemapManager.ConteggioCaratteri(_o.DatitecniciI, 30000, true));
                        break;
                }


                string linksezione = "";
                SProdotto sottocategoria = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto _tmp) { return (_tmp.Lingua == lingua && (_tmp.CodiceSProdotto == _o.CodiceCategoria2Liv)); });
                if (sottocategoria != null)
                {
                    linksezione = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, SitemapManager.CleanUrl(sottocategoria.Descrizione), "", _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv, "", "", "", gen, WelcomeLibrary.STATIC.Global.UpdateUrl);
                    //linksezione = CommonPage.CreaLinkRoutes(null, false, lingua, SitemapManager.CleanUrl(sottocategoria.Descrizione), "", _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv);
                    linksezione = "<a  onclick='javascript: JsSvuotaSession(this)'  href='" + linksezione + "'>" + sottocategoria.Descrizione + "</a>";
                }

                if (string.IsNullOrEmpty(linksezione))
                {
                    Prodotto categoria = Utility.ElencoProdotti.Find(p => p.Lingua == lingua && (p.CodiceTipologia == _o.CodiceTipologia && p.CodiceProdotto == _o.CodiceCategoria));
                    if (categoria != null)
                    {
                        linksezione = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, SitemapManager.CleanUrl(categoria.Descrizione), "", _o.CodiceTipologia, _o.CodiceCategoria, "", "", "", "", gen, WelcomeLibrary.STATIC.Global.UpdateUrl);
                        //linksezione = CommonPage.CreaLinkRoutes(null, false, lingua, SitemapManager.CleanUrl(categoria.Descrizione), "", _o.CodiceTipologia, _o.CodiceCategoria);
                        linksezione = "<a  onclick='javascript: JsSvuotaSession(this)'  href='" + linksezione + "'>" + categoria.Descrizione + "</a>";
                    }
                }

                string pathimmagine = filemanage.ComponiUrlAnteprima(_o.FotoCollection_M.FotoAnteprima, _o.CodiceTipologia, _o.Id.ToString(), true, true);
                // (potrei decidere anche di passare ls versione dell'immagine in base alla rispolizione   WelcomeLibrary.STATIC.Global.Viewportw
                pathimmagine = filemanage.SelectImageByResolution(pathimmagine, WelcomeLibrary.STATIC.Global.Viewportw);
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (string.IsNullOrEmpty(pathimmagine))
                    pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                string target = "";
                //string link = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(testotitolo), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, SitemapManager.CleanUrl(testotitolo), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, "", "", "", "", gen, WelcomeLibrary.STATIC.Global.UpdateUrl);
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && link.ToLower().IndexOf("~") == -1)
                {
                    target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //string titolo1 = testotitolo;
                //string titolo2 = "<br/>";
                //int i = testotitolo.IndexOf("\n");
                //if (i != -1)
                //{
                //    titolo1 = testotitolo.Substring(0, i);
                //    if (testotitolo.Length >= i + 1)
                //        titolo2 = testotitolo.Substring(i + 1);
                //}

                string contactlink = "";
                if (_o.Abilitacontatto) contactlink = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta&Lingua=" + lingua + "&idOfferta=" + _o.Id;
                string printlink = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/SchedaOffertaStampa.aspx?idOfferta=" + _o.Id + "&Lingua=" + lingua;
                string bcklink = SitemapManager.GeneraBackLink(_o.CodiceTipologia, _o.CodiceCategoria, lingua);

                string pathavatar = "";
                if (string.IsNullOrEmpty(pathavatar))
                    pathavatar = ("~/images/sitespecific/" + _o.Autore + ".png").Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                string numeroviews = "";
                if (visite != null && visite.ContainsKey(_o.Id))
                    numeroviews = visite[_o.Id].ToString();
                tmp.Add("views", numeroviews); //Numero di visualizzazioni della scheda

                tmp.Add("contactlink", contactlink);
                tmp.Add("printlink", printlink);
                tmp.Add("bcklink", bcklink);
                tmp.Add("link", link);
                tmp.Add("linksezione", linksezione);
                //tmp.Add("titolo", html.Convert(testotitolo));
                tmp.Add("titolo", (testotitolo));
                tmp.Add("descrizione", descrizione);
                tmp.Add("datitecnici", datitecnici);
                tmp.Add("image", pathimmagine);
                tmp.Add("avatar", pathavatar);
                tmp.Add("video", _o.linkVideo);

                //DETTAGLI PER LA LISTA COMPLETA ALLEGATI //////////////////////////////////
                if (filteredData != null && filteredData.Count == 1)  //Si riempiono solo per la scheda singola
                {

                    /****CREO IL LINK ALLA SCHEDA PRECEDENTRE E PROSSIMA RISPETTO ALLA SCHEDA ATTUALE **********/
                    //Carichiamo la prossima e precedente scheda di settore !!!
                    if (parColl.Exists(delegate (SQLiteParameter _par) { return _par.ParameterName == "@Id"; }))
                    {
                        parColl.Find(delegate (SQLiteParameter _par) { return _par.ParameterName == "@Id"; }).Value = _o.Id;
                    }
                    else
                    {
                        SQLiteParameter pid = new SQLiteParameter("@Id", _o.Id);
                        parColl.Add(pid);
                    }
                    if (parColl.Exists(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceTIPOLOGIA"; }))
                    {
                        parColl.Find(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceTIPOLOGIA"; }).Value = _o.CodiceTipologia; ;
                    }
                    else
                    {
                        SQLiteParameter ptip = new SQLiteParameter("@CodiceTIPOLOGIA", _o.CodiceTipologia);
                        parColl.Add(ptip);
                    }
                    if (parColl.Exists(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceCategoria"; }))
                    {
                        parColl.Find(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceCategoria"; }).Value = _o.CodiceCategoria;

                    }
                    else
                    {
                        SQLiteParameter ptcat = new SQLiteParameter("@CodiceCategoria", _o.CodiceCategoria);
                        parColl.Add(ptcat);
                    }
                    if (parColl.Exists(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceCategoria2Liv"; }))
                    {
                        parColl.Find(delegate (SQLiteParameter _par) { return _par.ParameterName == "@CodiceCategoria2Liv"; }).Value = _o.CodiceCategoria2Liv; ;
                    }
                    else
                    {
                        SQLiteParameter pc2liv = new SQLiteParameter("@CodiceCategoria2Liv", _o.CodiceCategoria2Liv);
                        parColl.Add(pc2liv);
                    }
                    Dictionary<string, Offerte> prevnextcontent = offDM.CaricaPrevNextOfferte(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl);
                    if (prevnextcontent != null)
                    {
                        if (prevnextcontent.ContainsKey("prev") && prevnextcontent["prev"] != null)
                        {

                            string linkprev = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, prevnextcontent["prev"].DenominazionebyLingua(lingua), prevnextcontent["prev"].Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv, "", "", "", gen, WelcomeLibrary.STATIC.Global.UpdateUrl);

                            //string linkprev = CommonPage.CreaLinkRoutes(null, false, lingua, prevnextcontent["prev"].DenominazionebyLingua(lingua), prevnextcontent["prev"].Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv);
                            tmp.Add("prevlink", linkprev);
                            tmp.Add("prevlinktext", prevnextcontent["prev"].DenominazionebyLingua(lingua));

                        }
                        if (prevnextcontent.ContainsKey("next") && prevnextcontent["next"] != null)
                        {
                            string linknext = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, prevnextcontent["next"].DenominazionebyLingua(lingua), prevnextcontent["next"].Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv, "", "", "", gen, WelcomeLibrary.STATIC.Global.UpdateUrl);

                            //string linknext = CommonPage.CreaLinkRoutes(null, false, lingua, prevnextcontent["next"].DenominazionebyLingua(lingua), prevnextcontent["next"].Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, _o.CodiceCategoria2Liv);
                            tmp.Add("nextlink", linknext);
                            tmp.Add("nextlinktext", prevnextcontent["next"].DenominazionebyLingua(lingua));
                        }
                    }
                    /*****************************************************************************************************/

                    List<string> imagescomplete = new List<string>();
                    List<string> imagesdesc = new List<string>();
                    List<string> imagesratio = new List<string>();
                    List<string> filescomplete = new List<string>();
                    List<string> filesdesc = new List<string>();


                    if ((_o != null) && (_o.FotoCollection_M.Count > 0))
                    {
                        foreach (Allegato a in _o.FotoCollection_M)
                        {
                            if ((a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                            {
                                //IMMAGINE
                                string tmppathimmagine = filemanage.ComponiUrlAnteprima(a.NomeFile, _o.CodiceTipologia, _o.Id.ToString(), true, true);
                                tmppathimmagine = filemanage.SelectImageByResolution(tmppathimmagine, WelcomeLibrary.STATIC.Global.Viewportw);

                                string abspathimmagine = tmppathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                                imagescomplete.Add(abspathimmagine);
                                //a.Descrizione -> dove la mettiamo
                                imagesdesc.Add(a.Descrizione);
                                try
                                {
                                    //using (System.Drawing.Image tmpimg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(tmppathimmagine)))
                                    using (System.Drawing.Image tmpimg = System.Drawing.Image.FromFile((tmppathimmagine).Replace("~", WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione).Replace("/", "\\").Replace("\\\\", "\\")))
                                    {
                                        imagesratio.Add(((double)tmpimg.Width / (double)tmpimg.Height).ToString());
                                    }
                                }
                                catch
                                { imagesratio.Add("1"); }
                            }
                            else
                            {
                                //a.Descrizione -> dove la mettiamo
                                string tmppathimmagine = filemanage.ComponiUrlAnteprima(a.NomeFile, _o.CodiceTipologia, _o.Id.ToString(), true, true);
                                //tmppathimmagine = filemanage.SelectImageByResolution(tmppathimmagine, WelcomeLibrary.STATIC.Global.Viewportw);

                                tmppathimmagine = tmppathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                                filescomplete.Add(tmppathimmagine);
                                filesdesc.Add(a.Descrizione);

                            }
                        }
                    }

                    if (!_o.Vetrina)
                    {
                        tmp.Add("imageslist", Newtonsoft.Json.JsonConvert.SerializeObject(imagescomplete));
                        tmp.Add("imagesdesc", Newtonsoft.Json.JsonConvert.SerializeObject(imagesdesc));
                        tmp.Add("imagesratio", Newtonsoft.Json.JsonConvert.SerializeObject(imagesratio));
                        tmp.Add("fileslist", Newtonsoft.Json.JsonConvert.SerializeObject(filescomplete));
                        tmp.Add("filesdesc", Newtonsoft.Json.JsonConvert.SerializeObject(filesdesc));
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////

                linksurl.Add(_o.Id.ToString(), tmp);
            }

            string retlinksurl = Newtonsoft.Json.JsonConvert.SerializeObject(linksurl);
            ritorno.Add("linkloaded", retlinksurl);

            return ritorno;
        }



        /// <summary>
        /// Funzione di rimpiazzo dei tag
        /// </summary>
        /// <param name="strIn"></param>
        /// <param name="nolink"></param>
        /// <returns></returns>
        public static String ReplaceLinks(string strIn, bool nolink = false)
        {
            List<string> tags = new List<string>();
            tags.Add("link:(");
            tags.Add("aimg:(");
            tags.Add("quot:(");
            tags.Add("bold:(");
            tags.Add("iden:(");
            tags.Add("butt:(");
            tags.Add("buto:(");
            tags.Add("imag:(");
            tags.Add("titl:(");
            tags.Add("vide:(");
            tags.Add("h2ti:(");

            string target = "_blank";
            string urlcorretto = "";
            string ret = strIn;
            int a = strIn.ToLower().IndexOf("link:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);
                    string url = strIn.Substring(a + 6, b - (a + 6));

                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }

                    string testourl = url;
                    //Splitto supponendo di avere lo schema ulr|testourl
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";

                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink && urlcorretto != "")
                    {
                        strIn = strIn.Replace(origtext, "<a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-weight:bold;background-color:#e0e0e0\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a>");
                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";
                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il link:( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("link:(");
            }
            ret = strIn;

            a = strIn.ToLower().IndexOf("quot:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));

                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }


                    string testourl = url;
                    //Splitto supponendo di avere lo schema ulr|testourl
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {

                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<div style=\"font-size:100%;padding:10px;margin:5px;background-color:#e0e0e0\">" + testourl + "</div>");
                        else
                            strIn = strIn.Replace(origtext, "<div  style=\"font-size:100%;padding:10px;margin:5px;background-color:#e0e0e0\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></div>");

                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";
                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("quot:(");
            }
            ret = strIn;


            a = strIn.ToLower().IndexOf("bold:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));

                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }

                    string testourl = url;
                    //Splitto supponendo di avere lo schema ulr|testourl
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<strong>" + testourl + "</strong>");
                        else
                            strIn = strIn.Replace(origtext, "<strong><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></strong>");



                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";
                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("bold:(");
            }
            ret = strIn;


            a = strIn.ToLower().IndexOf("iden:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    string testourl = url;
                    //Splitto supponendo di avere lo schema ulr|testourl
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {
                        //if (string.IsNullOrWhiteSpace(url))
                        //    strIn = strIn.Replace(origtext, "</p><span class=\"lateralbar\">" + testourl + "</span><p>");
                        //else
                        //    strIn = strIn.Replace(origtext, "</p><span class=\"lateralbar\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></span><p>");
                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<div class=\"lateralbar\">" + testourl + "</div>");
                        else
                            strIn = strIn.Replace(origtext, "<div class=\"lateralbar\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></div>");
                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";
                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("iden:(");
            }
            ret = strIn;

            a = strIn.ToLower().IndexOf("butt:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }

                    string testourl = url;
                    //Splitto supponendo di avere lo schema ulr|testourl
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }

                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);


                    if (!nolink)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<span style=\"line-height:normal;display:inline-block\" class=\"divbuttonstyle\">" + testourl + "</span>");
                        else
                            strIn = strIn.Replace(origtext, "<span style=\"line-height:normal;display:inline\"><a onclick=\"javascript:JsSvuotaSession(this)\" class=\"divbuttonstyle\" style=\"line-height:normal;display:inline-block\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></span>");



                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";
                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("butt:(");



            }
            ret = strIn;


            a = strIn.ToLower().IndexOf("buto:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }
                    string testourl = url;
                    //Splitto supponendo di avere lo schema ulr|testourl
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<span class=\"divbuttonstyleorange\">" + testourl + "</span>");
                        else
                            strIn = strIn.Replace(origtext, "<span class=\"divbuttonstyleorange\"><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-size:100%\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></span>");
                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";
                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("buto:(");
            }
            ret = strIn;


            a = strIn.ToLower().IndexOf("aimg:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }
                    string testourl = url;
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    string testourlcorretto = testourl;
                    if (!testourl.ToLower().StartsWith("http") && !testourl.ToLower().StartsWith("https") && !testourl.ToLower().StartsWith("~"))
                    {

                        testourlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + testourl;
                    }
                    if (testourl.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !testourl.ToLower().StartsWith("http") && !testourl.ToLower().StartsWith("https"))
                    {
                        
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            testourlcorretto = "https://" + testourl;
                        else
                            testourlcorretto = "http://" + testourl;
                    }
                    testourlcorretto = ReplaceAbsoluteLinks(testourlcorretto);

                    if (!nolink)
                    {
                        if (!string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<a target=\"_blank\"  href=\"" + urlcorretto + "\" ><img class=\"aimg\"  style=\"max-width:100%;border:none\"  src=\"" + testourlcorretto + "\"  /></a>");
                       

                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);

                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("aimg:(");


            }
            ret = strIn;


            a = strIn.ToLower().IndexOf("imag:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }
                    string testourl = url;
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {


                        if (!string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<img class=\"\"  style=\"max-width:100%\"  src=\"" + urlcorretto + "\" alt=\"" + testourl + "\"  />");
                        else if (!url.ToLower().StartsWith("http"))
                            strIn = strIn.Replace(origtext, "<img class=\"\"  style=\"max-width:100%\"  src=\"" + urlcorretto + "\" alt=\"" + testourl + "\"  />");

                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);

                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("imag:(");



            }
            ret = strIn;


            a = strIn.ToLower().IndexOf("vide:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }
                    string testourl = url;
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            string texthtml = "<div class=\"responsive-video\" style=\"display:block;\">";
                            texthtml += " <iframe frameborder=\"0\" allowfullscreen=\"\" class=\"\" src=\"" + urlcorretto + "\"></iframe>";
                            texthtml += "</div>";
                            strIn = strIn.Replace(origtext, texthtml);
                        }

                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);

                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il  :( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("vide:(");

            }
            ret = strIn;

            a = strIn.ToLower().IndexOf("titl:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }
                    string testourl = url;
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<span style=\"font-weight:800;font-size:1.4em\" >" + testourl + "</span>");
                        else
                            strIn = strIn.Replace(origtext, "<strong><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-weight:800;font-size:1.4em\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></strong>");
                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";

                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("titl:(");
            }
            ret = strIn;

            a = strIn.ToLower().IndexOf("h2ti:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    tags.ForEach(t => url = url.Replace(t, "")); //Non devo avre tag annidati senno si incasina !!! -> li elimino se presenti
                    int lastsplit = url.LastIndexOf('|');
                    int firstsplit = url.IndexOf('|');
                    while (lastsplit != firstsplit)
                    {
                        url = url.Remove(lastsplit, 1);
                        lastsplit = url.LastIndexOf('|');
                        firstsplit = url.IndexOf('|');
                    }
                    string testourl = url;
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        testourl = dati[1];
                    }
                    else
                        url = "";
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<h2 style=\"font-weight:800;font-size:1.4em;margin-bottom:6px\" >" + testourl + "</h2>");
                        else
                            strIn = strIn.Replace(origtext, "<h2><a  onclick=\"javascript:JsSvuotaSession(this)\"  style=\"font-weight:800;font-size:1.4em;margin-bottom:6px\" href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></h2>");
                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";

                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("h2ti:(");
            }
            ret = strIn;

            a = strIn.ToLower().IndexOf("cnfg:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    string testourl = url;
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        //testourl = dati[1];

                        testourl = ConfigManagement.ReadKey(dati[1]);

                    }
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<span>" + testourl + "</span>");
                        else
                            strIn = strIn.Replace(origtext, "<strong><a  onclick=\"javascript:JsSvuotaSession()\"   href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></strong>");

                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";

                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("cnfg:(");
            }
            ret = strIn;



            a = strIn.ToLower().IndexOf("rsrc:(");
            while (a != -1)
            {
                string origtext = "";
                int b = strIn.ToLower().IndexOf(")", a + 1);
                if (b != -1)
                {
                    origtext = strIn.Substring(a, b - a + 1);

                    string url = strIn.Substring(a + 6, b - (a + 6));
                    string testourl = url;
                    string[] dati = url.Split('|');
                    if (dati.Length == 2)
                    {
                        url = (dati[0]);
                        //testourl = dati[1];
                        string lingua = GetLinguaFromActualCulture(System.Threading.Thread.CurrentThread.CurrentCulture);
                        testourl = WelcomeLibrary.UF.ResourceManagement.ReadKey("Common", lingua, dati[1]).Valore;

                    }
                    urlcorretto = url;
                    if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https") && !url.ToLower().StartsWith("~"))
                    {
                        target = "_self";
                        urlcorretto = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + url;
                    }
                    if (url.ToLower().Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) && !url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("https"))
                    {
                        target = "_self";
                        if (WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower().StartsWith("https"))
                            urlcorretto = "https://" + url;
                        else
                            urlcorretto = "http://" + url;
                    }
                    urlcorretto = ReplaceAbsoluteLinks(urlcorretto);

                    if (!nolink)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                            strIn = strIn.Replace(origtext, "<span>" + testourl + "</span>");
                        else
                            strIn = strIn.Replace(origtext, "<strong><a  onclick=\"javascript:JsSvuotaSession()\"   href=\"" + urlcorretto + "\" target=\"" + target + "\">" + testourl + "</a></strong>");

                    }
                    else
                        strIn = strIn.Replace(origtext, testourl);
                    target = "_blank";

                }
                else
                {
                    strIn = strIn.Remove(a, 6); //SE non trovo la parentesi di chiusura -> tolgo il quot:( sennò si looppa
                }
                a = strIn.ToLower().IndexOf("rsrc:(");
            }
            ret = strIn;

            return ret;

        }

        public static string GetLinguaFromActualCulture(System.Globalization.CultureInfo currentCulture)
        {
            string lingua = ConfigManagement.ReadKey("deflanguage");

            switch (currentCulture.TwoLetterISOLanguageName.ToLower())
            {
                case "it":
                    lingua = "I";
                    break;
                case "en":
                    lingua = "GB";
                    break;
                case "ru":
                    lingua = "RU";
                    break;
                default:
                    lingua = "GB";
                    break;
            }
            return lingua;
        }

        public static string ReplaceAbsoluteLinks(string testo)
        {

            return testo.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
        }
        #endregion
    }
}
