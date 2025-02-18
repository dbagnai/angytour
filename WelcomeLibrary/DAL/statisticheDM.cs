using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;
using System.Data.SQLite;

namespace WelcomeLibrary.DAL
{

    public class statisticheDM
    {
        public static Dictionary<long, long> ContaTutteVisite(string connection, List<Offerte> list, long limitresults = 0)
        {
            //Reverse sort dictionary
            //var retdict =  new SortedDictionary<long, long>(Comparer<long>.Create((x, y) => y.CompareTo(x)));
            Dictionary<long, long> retdict = new Dictionary<long, long>();
            StringBuilder sb = new StringBuilder();
            string query = "";
            if (connection == null || connection == "") return retdict;
            try
            {
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                query += ("SELECT COUNT(Id) as visite, Idattivita  FROM TBL_STATISTICHE ");
                if (list != null && list.Count > 0)
                {
                    query += ("WHERE Idattivita in ( ");

                    foreach (Offerte c in list)
                    {
                        query += (" " + c.Id + ",");
                    }

                    query = query.TrimEnd(',');
                    query += " ) ";
                }

                query += " Group by Idattivita order by visite DESC  ";

                if (limitresults != 0)
                    query += " limit  " + limitresults;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return retdict; };
                    if (reader.HasRows == false)
                        return retdict;

                    while (reader.Read())
                    {
                        long visite = reader.GetInt64(reader.GetOrdinal("visite"));
                        long idattivita = reader.GetInt64(reader.GetOrdinal("Idattivita"));

                        if (!retdict.ContainsKey(idattivita))
                            retdict.Add(idattivita, visite);
                    }
                }
                //Dictionary<long, long> sortedict = new Dictionary<long, long>();
                var sortedDict = (from entry in retdict orderby entry.Value descending select entry);
                retdict = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Conteggio totale stats :" + error.Message, error);
            }
            return retdict;
        }

        public static long ContaVisiteByIdcontenuto(string connection, string Idcontenuto)
        {
            long visite = 0;
            if (connection == null || connection == "") return visite;
            if (Idcontenuto == null || Idcontenuto == "") return visite;
            try
            {
                string query = "SELECT COUNT(Id) as visite FROM TBL_STATISTICHE where Idattivita=@Idattivita";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@Idattivita", Idcontenuto);//OleDbType.VarChar
                parColl.Add(p1);

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return visite; };
                    if (reader.HasRows == false)
                        return visite;

                    while (reader.Read())
                    {
                        visite = reader.GetInt64(reader.GetOrdinal("visite"));
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Conteggio stats :" + error.Message, error);
            }

            return visite;
        }
        /// <summary>
        /// vERIFICA SE PRESENTE UNA STATISTICA per un certo contenuto entro un lapse dalla data indicata per uno specifico ipaddress
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Idcontenuto"></param>
        /// <param name="testdate"></param>
        /// <returns></returns>
        public static bool CaricaLastRecordStatisticaByIdcontenuto(string connection, string Idcontenuto, DateTime basedate, TimeSpan datelapse, string ipaddress)
        {
            if (connection == null || connection == "") return false;
            if (Idcontenuto == null || Idcontenuto == "") return false;
            bool ret = false;
            StatisticheCollection list = new StatisticheCollection();
            Statistiche item;

            try
            {
                string query = "SELECT  * FROM TBL_STATISTICHE where Idattivita=@Idattivita and Testomail=@Testomail order BY Data Desc,Id ASC limit 1 ";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@Idattivita", Idcontenuto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteParameter p2 = new SQLiteParameter("@Testomail", ipaddress);//OleDbType.VarChar
                parColl.Add(p2);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return ret; };
                    if (reader.HasRows == false)
                        return ret;

                    while (reader.Read())
                    {
                        item = new Statistiche();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        //item. = reader.GetInt32(reader.GetOrdinal("Idattivita"));
                        item.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                        item.Testomail = reader.GetString(reader.GetOrdinal("Testomail"));
                        //Confermo la registrazione statistica solo se superato il timelapse indicato
                        if (((TimeSpan)(basedate - item.Data)).TotalMinutes < datelapse.TotalMinutes)
                        {
                            ret = true;
                            break;
                        }
                        //list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento stats :" + error.Message, error);
            }

            return ret;
        }

        public string CleanStatisticheAndExport(string connection, DateTime data, string DestinationPath, string CsvFilename)
        {
            string retString = "";
            try
            {
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                parColl.Add(new SQLiteParameter("@Data", dbDataAccess.CorrectDatenow(data)));
                //parColl.Add(new SQLiteParameter("@TipoContatto", enumclass.TipoContatto.visitaurl.ToString()));
                StatisticheCollection list = CaricaStatiticheFiltered(connection, parColl);
                if (list.Count > 0)
                    retString = ExportOrdersToCsv(DestinationPath, CsvFilename, list);
                if (string.IsNullOrEmpty(retString))
                    PulisciTabellaStatistiche(connection, data);
            }
            catch (Exception err)
            {
                retString = err.Message;
                if (err.InnerException != null)
                    retString += err.InnerException.Message;
            }
            return retString;
        }

        public StatisticheCollection CaricaStatiticheFiltered(string connection, List<SQLiteParameter> parColl)
        {
            StatisticheCollection list = new StatisticheCollection();
            string errorString = "";
            if (connection == null || connection == "") return list;
            Statistiche item;
            try
            {
                string query = "";
                string queryfilter = "";
                if (parColl != null && parColl.Count > 0)
                {
                    queryfilter = " WHERE ";
                    //foreach (SQLiteParameter p in parColl)
                    //{
                    //    queryfilter += p.ParameterName + " = " + p.Value + " AND ";
                    //}
                    if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Data"; }))
                    {
                        queryfilter += "  ( Data <= @Data ) AND ";
                    }
                    if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@TipoContatto"; }))
                    {
                        queryfilter += "  ( TipoContatto = @TipoContatto ) AND ";
                    }



                    queryfilter = queryfilter.TrimEnd(" AND ".ToCharArray());
                }

                query = "SELECT  * FROM TBL_STATISTICHE " + queryfilter;
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Statistiche();
                        item.TipoContatto = reader.GetString(reader.GetOrdinal("TipoContatto"));
                        item.EmailDestinatario = reader.GetString(reader.GetOrdinal("EmailDestinatario"));
                        item.EmailMittente = reader.GetString(reader.GetOrdinal("EmailMittente"));
                        item.Url = reader.GetString(reader.GetOrdinal("Url"));
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        item.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                        item.Testomail = reader.GetString(reader.GetOrdinal("Testomail"));
                        item.Idattivita = reader.GetInt64(reader.GetOrdinal("Idattivita"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception err)
            {
                errorString = err.Message;
                if (err.InnerException != null)
                    errorString += err.InnerException.Message;
                //da ritornare eventualemente!!!
            }

            return list;
        }

        public string ExportOrdersToCsv(string DestinationPath, string CsvFilename, StatisticheCollection list)
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
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Id"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Data"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Tipocontatto"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Idattivita"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("Url"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("EmailDestinatario"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("EmailMittente"));
                    sb.Append(";");
                    sb.Append(WelcomeLibrary.UF.Csv.Escape("TestoMail"));
                    sb.Append(";");
                    WelcomeLibrary.UF.SharedStatic.WriteToFile(CsvFilename, DestinationPath, sb.ToString(), false);

                    foreach (Statistiche t in list)
                    {

                        sb = new StringBuilder();

                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.Id.ToString()));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(string.Format("{0:dd/MM/yyyy HH:mm:ss}", t.Data)));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.TipoContatto));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.Idattivita.ToString()));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.Url));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.EmailDestinatario));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.EmailMittente));
                        sb.Append(";");
                        sb.Append(WelcomeLibrary.UF.Csv.Escape(t.Testomail.Replace("<br/>", "\r\n")));
                        sb.Append(";");

                        //    sb.Append(WelcomeLibrary.UF.Csv.Escape(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                        //new object[] { t.TotaleSmaltimento + t.TotaleOrdine + t.TotaleSpedizione + t.TotaleAssicurazione - t.TotaleSconto }) + " €")); 

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
        /// Cancelle le statistiche antecedenti una data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        public void PulisciTabellaStatistiche(string connection, DateTime data)
        {
            if (connection == null || connection == "") return;
            string query = "DELETE FROM TBL_STATISTICHE WHERE ( Data < @Data ) ";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter pdmin;
            pdmin = new SQLiteParameter("@Data", dbDataAccess.CorrectDatenow(data));
            parColl.Add(pdmin);

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
            }
            catch
            {
                //throw new ApplicationException("Errore, cancellazione mail prese in carico vecchie :" + error.Message, error);
            }
            return;
        }



        /// <summary>
        /// Inserisce o aggiorna i dati di un cliente nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public static void InserisciAggiorna(string connessione, Statistiche item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@Idattivita", item.Idattivita);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@EmailDestinatario", item.EmailDestinatario);//OleDbType.VarChar
            parColl.Add(p2);
            SQLiteParameter p2b = new SQLiteParameter("@EmailMittente", item.EmailMittente);//OleDbType.VarChar
            parColl.Add(p2b);
            // string _tmp = enumclass.TipoContatto.visitaurl.ToString();

            SQLiteParameter p3 = new SQLiteParameter("@TipoContatto", item.TipoContatto);//OleDbType.VarChar
            parColl.Add(p3);
            SQLiteParameter p5 = new SQLiteParameter("@Url", item.Url);//OleDbType.VarChar
            parColl.Add(p5);
            SQLiteParameter p4 = null;
            if (item.Data != null && item.Data != DateTime.MinValue)
                p4 = new SQLiteParameter("@Data", dbDataAccess.CorrectDatenow(item.Data));
            else
                p4 = new SQLiteParameter("@Data", dbDataAccess.CorrectDatenow(System.DateTime.Now));
            //p4.OleDbType = OleDbType.Date;
            parColl.Add(p4);

            SQLiteParameter p6 = new SQLiteParameter("@Testomail", item.Testomail);//OleDbType.VarChar
            parColl.Add(p6);

            string query = "";
            if (item.Id != 0)
            {
                //Update
                query = "UPDATE [TBL_STATISTICHE] SET Idattivita=@Idattivita,EmailDestinatario=@EmailDestinatario,EmailMittente=@EmailMittente,TipoContatto=@TipoContatto,Url=@Url,Data=@Data,Testomail=@Testomail";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_STATISTICHE (Idattivita,EmailDestinatario,EmailMittente,TipoContatto,Url";
                query += ",Data,Testomail )";
                query += " values ( ";
                query += "@Idattivita,@EmailDestinatario,@EmailMittente,@TipoContatto,@Url";
                query += ",@Data,@Testomail )";
            }

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento statistiche :" + error.Message, error);
            }
            return;
        }




    }
}
