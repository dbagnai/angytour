using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using System.Data.SQLite;

namespace WelcomeLibrary.DAL
{
    public class bookingDM
    {
        //idtipolistino = 1 -> giornaliera --  2 -> settimanale

        #region GESTIONE LISTINI

        /// <summary>
        ///  Carica tutte le fasce di costo interessate in base al periodo indicato, all'attività ed al tipo di tariffa da usare
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="idattivita"></param>
        /// <param name="Idtipolistino"></param>
        /// <returns></returns>
        public List<Listino> CaricaFasceCosto(string connection, DateTime? startdate, DateTime? enddate, string idattivita, string Idtipolistino)
        {
            List<Listino> fasce = new List<Listino>();
            Listino item = null;
            try
            {
                string query = "SELECT * FROM TBL_LISTINO ";
                string queryfilter = "";

                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                long _i = 0;


                if (!string.IsNullOrEmpty(idattivita))
                {
                    long.TryParse(idattivita, out _i);
                    SQLiteParameter p1 = new SQLiteParameter("@idattivita", _i);
                    parColl.Add(p1);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idattivita = @idattivita ";
                    else
                        queryfilter += " AND idattivita = @idattivita  ";
                }
                if (!string.IsNullOrEmpty(Idtipolistino))
                {
                    long.TryParse(Idtipolistino, out _i);
                    SQLiteParameter p2 = new SQLiteParameter("@Idtipolistino", _i);
                    parColl.Add(p2);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Idtipolistino = @Idtipolistino ";
                    else
                        queryfilter += " AND Idtipolistino = @Idtipolistino  ";
                }
                if (startdate != null && enddate != null)
                {
                    SQLiteParameter p3 = new SQLiteParameter("@startdate", startdate);
                    parColl.Add(p3);
                    SQLiteParameter p4 = new SQLiteParameter("@enddate", enddate);
                    parColl.Add(p4);

                    if (!queryfilter.ToLower().Contains("where")) queryfilter += " WHERE not ( Enddate < @startdate or Startdate > @enddate  )  ";
                    else
                        queryfilter += " AND not ( Enddate < @startdate or Startdate > @enddate  )   ";
                }

                //long.TryParse(idattivita, out _i);
                //SQLiteParameter p1 = new SQLiteParameter("@idattivita", _i);
                //parColl.Add(p1);
                //_i = 0;
                //long.TryParse(Idtipolistino, out _i);
                //SQLiteParameter p2 = new SQLiteParameter("@Idtipolistino", _i);
                //parColl.Add(p2);

                //SQLiteParameter p3 = new SQLiteParameter("@startdate", startdate);
                //parColl.Add(p3);
                //SQLiteParameter p4 = new SQLiteParameter("@enddate", enddate);
                //parColl.Add(p4);

                query += queryfilter;
                query += " order by startdate asc";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Listino();
                        item.Idlistino = reader.GetInt64(reader.GetOrdinal("Idlistino"));
                        item.Idattivita = reader.GetInt64(reader.GetOrdinal("Idattivita"));
                        item.Idtipolistino = reader.GetInt64(reader.GetOrdinal("Idtipolistino"));
                        item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));
                        item.Prezzolistino = reader.GetDouble(reader.GetOrdinal("Prezzolistino"));
                        item.Startdate = reader.GetDateTime(reader.GetOrdinal("Startdate"));
                        item.Enddate = reader.GetDateTime(reader.GetOrdinal("Enddate"));
                        item.Textfield3 = "Perifstino";// + item.Idlistino;
                        fasce.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore CaricaFasceCosto :" + error.Message, error);
            }

            return fasce;
        }


        /// <summary>
        ///  Conta tutte le fasce di costo interessate in base al periodo indicato, all'attività ed al tipo di tariffa da usare
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="idattivita"></param>
        /// <param name="Idtipolistino"></param>
        /// <returns></returns>
        public long ContaFascelistinoNelPeriodo(string connection, DateTime startdate, DateTime enddate, string idattivita, string Idtipolistino, string idescluso = "")
        {
            long ret = 0;
            try
            {
                string query = "SELECT count(*) as nfasce FROM TBL_LISTINO ";
                string queryfilter = "";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                long _i = 0;

                if (!string.IsNullOrEmpty(idescluso))
                {
                    long.TryParse(idescluso, out _i);
                    SQLiteParameter p0 = new SQLiteParameter("@idescluso", _i);
                    parColl.Add(p0);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idlistino <> @idescluso ";
                    else
                        queryfilter += " AND idlistino <> @idescluso ";
                }
                if (!string.IsNullOrEmpty(idattivita))
                {
                    long.TryParse(idattivita, out _i);
                    SQLiteParameter p1 = new SQLiteParameter("@idattivita", _i);
                    parColl.Add(p1);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idattivita = @idattivita ";
                    else
                        queryfilter += " AND idattivita = @idattivita  ";
                }
                if (!string.IsNullOrEmpty(Idtipolistino))
                {
                    long.TryParse(Idtipolistino, out _i);
                    SQLiteParameter p2 = new SQLiteParameter("@Idtipolistino", _i);
                    parColl.Add(p2);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Idtipolistino = @Idtipolistino ";
                    else
                        queryfilter += " AND Idtipolistino = @Idtipolistino  ";
                }
                if (startdate != null && enddate != null)
                {
                    SQLiteParameter p3 = new SQLiteParameter("@startdate", startdate);
                    parColl.Add(p3);
                    SQLiteParameter p4 = new SQLiteParameter("@enddate", enddate);
                    parColl.Add(p4);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE not ( Enddate < @startdate or Startdate > @enddate  )  ";
                    else
                        queryfilter += " AND not ( Enddate < @startdate or Startdate > @enddate  )   ";
                }

                query += queryfilter;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return 0; };
                    if (reader.HasRows == false)
                        return 0;
                    while (reader.Read())
                    {
                        ret = reader.GetInt64(reader.GetOrdinal("nfasce"));
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore ContaFascelistinoNelPeriodo :" + error.Message, error);
            }
            return ret;
        }



        /// <summary>
        /// Carica tutte le fasce di costo per una certa attività e tipo di tariffa
        /// </summary>
        /// <param name="idrisorsa"></param>
        /// <param name="tipotariffa"></param>
        /// <returns></returns>
        public List<Listino> CaricaFasceCostoByAttivitaTipotariffa(string connection, string idattivita, string tipotariffa)
        {
            List<Listino> fasce = new List<Listino>();

            Listino item = null;
            try
            {
                string query = "SELECT * FROM TBL_LISTINO ";
                string queryfilter = "";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                long _i = 0;

                if (!string.IsNullOrEmpty(idattivita))
                {
                    long.TryParse(idattivita, out _i);
                    SQLiteParameter p1 = new SQLiteParameter("@idattivita", _i);
                    parColl.Add(p1);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idattivita = @idattivita ";
                    else
                        queryfilter += " AND idattivita = @idattivita  ";
                }
                if (!string.IsNullOrEmpty(tipotariffa))
                {
                    long.TryParse(tipotariffa, out _i);
                    SQLiteParameter p2 = new SQLiteParameter("@Idtipolistino", _i);
                    parColl.Add(p2);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Idtipolistino = @Idtipolistino ";
                    else
                        queryfilter += " AND Idtipolistino = @Idtipolistino  ";
                }
                query += queryfilter;

                //SQLiteParameter p3 = new SQLiteParameter("@startdate", startdate);
                //parColl.Add(p3);
                //SQLiteParameter p4 = new SQLiteParameter("@enddate", enddate);
                //parColl.Add(p4);

                query += " order by idattivita asc";
                //query += " order by startdate asc";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Listino();
                        item.Idlistino = reader.GetInt64(reader.GetOrdinal("Idlistino"));
                        item.Idattivita = reader.GetInt64(reader.GetOrdinal("Idattivita"));
                        item.Idtipolistino = reader.GetInt64(reader.GetOrdinal("Idtipolistino"));
                        item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));
                        item.Prezzolistino = reader.GetDouble(reader.GetOrdinal("Prezzolistino"));
                        item.Startdate = reader.GetDateTime(reader.GetOrdinal("Startdate"));
                        item.Enddate = reader.GetDateTime(reader.GetOrdinal("Enddate"));
                        item.Textfield3 = "Periodo Listino" + " - Tariffa " + item.Idtipolistino;
                        fasce.Add(item);
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore CaricaFasceCosto :" + error.Message, error);
            }
            return fasce;
        }





        /// <summary>
        /// Carica la voce di listino per idlistino
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Listino CaricaListinoPerId(string connection, string idlistino)
        {
            if (connection == null || connection == "") return null;
            if (idlistino == null || idlistino == "") return null;
            Listino item = null;

            try
            {
                string query = "SELECT * FROM TBL_LISTINO where idlistino=@ID";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                long _i = 0;
                long.TryParse(idlistino, out _i);
                SQLiteParameter p1 = new SQLiteParameter("@ID", _i);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Listino();
                        item.Idlistino = reader.GetInt64(reader.GetOrdinal("Idlistino"));
                        item.Idattivita = reader.GetInt64(reader.GetOrdinal("Idattivita"));
                        item.Idtipolistino = reader.GetInt64(reader.GetOrdinal("Idtipolistino"));
                        item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));
                        item.Prezzolistino = reader.GetDouble(reader.GetOrdinal("Prezzolistino"));
                        item.Startdate = reader.GetDateTime(reader.GetOrdinal("Startdate"));
                        item.Enddate = reader.GetDateTime(reader.GetOrdinal("Enddate"));
                        item.Textfield3 = "Periodo Listino" + " - Tariffa " + item.Idtipolistino;
                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore CaricaListinoPerId :" + error.Message, error);
            }
            return item;
        }

        /// <summary>
        /// aggiornamento della posizione temporale di una voce listino
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="idattivita"></param>
        public static void dbUpdateListinoRange(string connessione, string id, DateTime start, DateTime end, string idattivita, string idtariffa)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            long _i = 0;
            long.TryParse(id, out _i);
            if (_i == 0) return;
            long _ia = 0;
            long.TryParse(idattivita, out _ia);
            long _it = 0;
            long.TryParse(idtariffa, out _it);
            SQLiteParameter p2 = new SQLiteParameter("@Idattivita", _ia);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@Idtipolistino", _it);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@Startdate", start);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Enddate", end);
            parColl.Add(p5);

            SQLiteParameter p1 = new SQLiteParameter("@ID", _i);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "UPDATE [TBL_LISTINO] SET [Idattivita]=@Idattivita,Idtipolistino=@Idtipolistino,Startdate=@Startdate,Enddate=@Enddate  WHERE ([Idlistino]=@ID)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbUpdateListinoRange :" + error.Message, error);
            }
        }

        /// <summary>
        /// Aggiornamento completo voci di listino
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="idattivita"></param>
        /// <param name="idtariffa"></param>
        public static void dbUpdateListino(string connessione, string idlistino, DateTime start, DateTime end, string idattivita, string idtariffa, double prezzo, double prezzolistino)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            long _i = 0;
            long.TryParse(idlistino, out _i);
            if (_i == 0) return;
            long _ia = 0;
            long.TryParse(idattivita, out _ia);
            long _it = 0;
            long.TryParse(idtariffa, out _it);
            SQLiteParameter p2 = new SQLiteParameter("@Idattivita", _ia);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@Idtipolistino", _it);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@Startdate", start);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Enddate", end);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@prezzo", prezzo);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@prezzolistino", prezzolistino);
            parColl.Add(p7);

            SQLiteParameter p1 = new SQLiteParameter("@ID", _i);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "UPDATE [TBL_LISTINO] SET [Idattivita]=@Idattivita,Idtipolistino=@Idtipolistino,Startdate=@Startdate,Enddate=@Enddate,prezzo=@prezzo,prezzolistino=@prezzolistino  WHERE ([Idlistino]=@ID)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbUpdateListino :" + error.Message, error);
            }
        }



        public static void dbInsertListino(string connessione, Listino _d)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            SQLiteParameter p2 = new SQLiteParameter("@Idattivita", _d.Idattivita);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@Idtipolistino", _d.Idtipolistino);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@Startdate", _d.Startdate);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Enddate", _d.Enddate);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@prezzo", _d.Prezzo);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@prezzolistino", _d.Prezzolistino);
            parColl.Add(p7);
            string query = "INSERT INTO TBL_LISTINO ([Idattivita],[Idtipolistino],[Startdate],[Enddate],[Prezzo],[Prezzolistino]) VALUES (@Idattivita,@Idtipolistino,@Startdate,@Enddate,@Prezzo,@Prezzolistino)";
            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                _d.Idlistino = lastidentity; //Inserisco nell'id dell'elemento inseito l'id generato dal db
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbInsertListino :" + error.Message, error);
            }
        }

        public static void dbDeleteListino(string connessione, string idlistino)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            long _i = 0;
            long.TryParse(idlistino, out _i);
            if (_i == 0) return;

            SQLiteParameter p1 = new SQLiteParameter("@ID", _i);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "delete FROM TBL_LISTINO WHERE ([idlistino]=@ID)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbDeleteListino :" + error.Message, error);
            }
            return;
        }

#if false //vecchio metodo che calcolava anche se le fasce del periodo non erano presenti!!! prendendo le ultime presenti
        /// <summary>
        /// Calcola il costo complessivo per una certa attivita e periodo, con l'utilizzo della tabella dei listini
        /// considerando la differenziazione tra prezzo settimanale e giornaliero
        /// Se il periodo richiesto supera la settimana i giorni aggiuntivi sono calcolati col prezzo giornaliero del periodo
        /// (ATTENZIONE I LISTINI DEVONO ESSERE INSERITI SEMPRE IN UN UNICO ANNO SOLARE!!! VIENE PRESO SEMPRE L'ULTIMO ANNO INSERITO)
        /// </summary>
        public double CalcolaPrezzoBase(string connection, DateTime eventstart, DateTime eventend, string idattivita, ref string error)
        {
            bool coincidenzatipoannotabellalistino = false;
            double prezzocalcolato = 0;
            int ngiorni = 0;
            int nsettimane = Math.DivRem(((TimeSpan)(eventend.Date - eventstart.Date)).Days, 7, out ngiorni);
            List<Listino> fascesettimanali = new List<Listino>();
            List<Listino> fascegiornaliere = new List<Listino>();

            //Ora devo calcolare l'importo base suddividendo in periodi settimanali e giornalieri e calcolando il costo in base alle tariffe di listino
            //tipologia tariff a giornaliera 1 - settimanale 2

            //CARICO LE FASCE DI COSTO INTERESSATE DALLA TABELLA LISTINO (SETTIMANALI)
            fascesettimanali = CaricaFasceCostoByAttivitaTipotariffa(connection, idattivita, "2");
            if (fascesettimanali == null || fascesettimanali.Count() == 0) //Se non presenti le fasce settimanali uso solo il giornaliero
                ngiorni = ((TimeSpan)(eventend.Date - eventstart.Date)).Days;
            //CARICO LE FASCE DI COSTO INTERESSATE DALLA TABELLA LISTINO (GIORNALIERE)
            fascegiornaliere = CaricaFasceCostoByAttivitaTipotariffa(connection, idattivita, "1");

            //UNICO ANNO RIFERIMENTO PER I LISTINI LE FASCE DEVONO ESSERE SEMPRE IN UN UNICO ANNO
            fascesettimanali = fascesettimanali.OrderByDescending(o => o.Enddate).ToList();
            //elimino le fasce fuori dall'anno ( commentato faccio un filtro dopo )
            int annoref = fascesettimanali.First().Enddate.Year;
            fascesettimanali.RemoveAll(f => f.Enddate.Year != annoref || f.Startdate.Year != annoref);

            fascegiornaliere = fascegiornaliere.OrderByDescending(o => o.Enddate).ToList();
            //elimino le fasce fuori dall'anno ( commentato faccio un filtro dopo )
            int annoref1 = fascegiornaliere.First().Enddate.Year;
            fascegiornaliere.RemoveAll(f => f.Enddate.Year != annoref1 || f.Startdate.Year != annoref1);
            ////////////////////////////////

            //////////////VERIFICHE SU ANNO BISESTILE (da spostare dentro al ciclo sotto !!!!  ) //////////////////////
            bool annolistinobisestile = true;
            if (fascesettimanali != null && fascesettimanali.Count() > 0) { annolistinobisestile = DateTime.IsLeapYear(fascesettimanali.First().Enddate.Year); }
            else if (fascegiornaliere != null && fascegiornaliere.Count() > 0) { annolistinobisestile = DateTime.IsLeapYear(fascegiornaliere.First().Enddate.Year); }
            bool annorichiestabisestile = DateTime.IsLeapYear(eventstart.Year);
            if (annolistinobisestile == annorichiestabisestile) coincidenzatipoannotabellalistino = true;
            ////////////////////////////

            //CALCOLIAMO IL COSTO IN RELAZIONE ALLE SETTIMANE PRESENTI NEL PERIODO INDICATO
            DateTime _startcalc = eventstart;
            DateTime _endcalc = eventstart.AddDays(7);
            if (nsettimane != 0)
            {
                if (fascesettimanali != null && fascesettimanali.Count() > 0)
                {
                    for (int _i = 1; _i <= nsettimane; _i++) //Ciclo sulle settimane del periodo richiesto
                    {
                        Listino fasciadicostostart = null;
                        Listino fasciadicostoend = null;

                        //PER PRIMA COSA FILTRIAMO O SELEZIONIAMO LE FASCE DI COSTO DI INTERESSE IN BASE ALL'ANNO DELLA DATE DELLA RICHIESTA
                        // da fare verifica sulla presenza delle fasce corrette di costo


                        //Vediamo i costi una settimana alla volta ( per un'idattivita e un tipo di tariffa dovrebbe uscire solo un elemento listino per ogni data di ricerca)
                        //CERCO LE FASCE DI COSTO IN CUI RIENTRANO LA DATA INIZIO E FINE SETTIMANA PER LA SETTIMANA ATTUALE DEL CICLO

                        //CONTROLLO SULLA DATA INZIO SETTIMANA------------------------------------------------------------------------
                        List<Listino> settimanaselezionata = null;
                        if (coincidenzatipoannotabellalistino) //entrambi bisestile o entrambi no
                            settimanaselezionata = fascesettimanali.Where(c => _startcalc.DayOfYear >= c.Startdate.DayOfYear && _startcalc.DayOfYear <= c.Enddate.DayOfYear).ToList();
                        else
                        {
                            if (annolistinobisestile)
                                settimanaselezionata = fascesettimanali.Where(c => _startcalc.DayOfYear >= c.Startdate.AddYears(1).DayOfYear && _startcalc.DayOfYear <= c.Enddate.AddYears(1).DayOfYear).ToList();
                            if (annorichiestabisestile)
                                settimanaselezionata = fascesettimanali.Where(c => _startcalc.AddYears(1).DayOfYear >= c.Startdate.DayOfYear && _startcalc.AddYears(1).DayOfYear <= c.Enddate.DayOfYear).ToList();
                        }
                        //Controllo che sia uscita una sola fascia di listino
                        if (settimanaselezionata != null && settimanaselezionata.Count == 1)
                            fasciadicostostart = settimanaselezionata[settimanaselezionata.Count - 1];
                        else if (settimanaselezionata != null && settimanaselezionata.Count > 1)
                        {
                            //fasciadicostostart = settimanaselezionata[settimanaselezionata.Count - 1];
                            error += "Errore trovate fasce di costo multiple per la data: " + _startcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 2 .Verificare";
                        }
                        else if (settimanaselezionata == null)
                        {
                            error += "Non trovata fascia di costo per la data: " + _startcalc.ToShortDateString() + " alloggio: " + idattivita + " tipotariffa: 2 .Verificare";
                        }
                        //---------------------------------------------------------------------------------------------------------------

                        //CONTROLLI SULLA DATA DI FINE SETTIMANA-------------------------------------------------------------------------
                        if (coincidenzatipoannotabellalistino)//entrambi bisestile o entrambi no
                            settimanaselezionata = fascesettimanali.Where(c => _endcalc.DayOfYear >= c.Startdate.DayOfYear && _endcalc.DayOfYear <= c.Enddate.DayOfYear).ToList();
                        else
                        {
                            if (annolistinobisestile)
                                settimanaselezionata = fascesettimanali.Where(c => _endcalc.DayOfYear >= c.Startdate.AddYears(1).DayOfYear && _endcalc.DayOfYear <= c.Enddate.AddYears(1).DayOfYear).ToList();
                            if (annorichiestabisestile)
                                settimanaselezionata = fascesettimanali.Where(c => _endcalc.AddYears(1).DayOfYear >= c.Startdate.DayOfYear && _endcalc.AddYears(1).DayOfYear <= c.Enddate.DayOfYear).ToList();
                        }
                        //Controllo che sia uscita una sola fascia di listino
                        if (settimanaselezionata != null && settimanaselezionata.Count == 1)
                            fasciadicostoend = settimanaselezionata[settimanaselezionata.Count - 1];
                        else if (settimanaselezionata != null && settimanaselezionata.Count > 1)
                        {
                            //fasciadicostoend = settimanaselezionata[settimanaselezionata.Count - 1];
                            error += "Errore trovate fasce di costo multiple per la data: " + _endcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 2 .Verificare";
                        }
                        else if (settimanaselezionata == null)
                        {
                            error += "Non trovata fascia di costo per la data: " + _endcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 2 .Verificare";
                        }

                        //---------------------------------------------------------------------------------------------------------------
                        //Passiamo al calcolo del costo in base al periodo richiesto
                        //Il criterio è tale che per il costo della settimana scelgo la fascia a costo maggiore tra quelle trovate anche se 
                        //la settimana cade a cavallo di due periodi di costo
                        if (fasciadicostostart != null && fasciadicostoend != null)
                        {
                            //Da fare diverso!!-> vedere se coincidono la fasciaster ed end >> prendi il prezzo , se non coincidono -> siamo a cavallo di due fasce quindi devo fare un calcolo in base alla sovrapposizione delle date!!!
                            //da fare ..
                            if (fasciadicostostart.Prezzo > fasciadicostoend.Prezzo)
                                prezzocalcolato += fasciadicostostart.Prezzo;
                            else
                                prezzocalcolato += fasciadicostoend.Prezzo;
                        }
                        else
                        {
                            error += "Non trovato (oppure valori multipli) fascia per il calcolo dei costi nel periodo : " + _startcalc.ToShortDateString() + "  -  " + _endcalc.ToShortDateString();
                        }

                        //Passiamo ala settimana successiva per il cumulo dei costi totali
                        _startcalc = _startcalc.AddDays(7);
                        _endcalc = _startcalc.AddDays(7);

                    }
                }

            }

            //CALCOLIAMO IL COSTO IN BASE AI GIORNI AGGIUNTIVI PRESENTI NEL PERIODO RICHIESTO (_startcalc)
            if (ngiorni != 0)
            {
                if (fascegiornaliere != null && fascegiornaliere.Count() > 0)
                {
                    for (int _i = 1; _i <= ngiorni; _i++) //Ciclo sui giorni del periodo richiesto
                    {
                        Listino fasciaselezionata = null;

                        //List<Listino> periodolistino = fascegiornaliere.Where(c => _startcalc >= c.startdate && _startcalc <= c.enddate).ToList();
                        List<Listino> periodolistino = null;
                        if (coincidenzatipoannotabellalistino)//entrambi bisestile o entrambi no
                            periodolistino = fascegiornaliere.Where(c => _startcalc.DayOfYear >= c.Startdate.DayOfYear && _startcalc.DayOfYear <= c.Enddate.DayOfYear).ToList();
                        else
                        {
                            if (annolistinobisestile)
                                periodolistino = fascegiornaliere.Where(c => _startcalc.DayOfYear >= c.Startdate.AddYears(1).DayOfYear && _startcalc.DayOfYear <= c.Enddate.AddYears(1).DayOfYear).ToList();
                            if (annorichiestabisestile)
                                periodolistino = fascegiornaliere.Where(c => _startcalc.AddYears(1).DayOfYear >= c.Startdate.DayOfYear && _startcalc.AddYears(1).DayOfYear <= c.Enddate.DayOfYear).ToList();
                        }
                        //Controllo che sia uscita una sola fascia di listino
                        if (periodolistino != null && periodolistino.Count == 1)
                            fasciaselezionata = periodolistino[0];
                        else if (periodolistino != null && periodolistino.Count > 1)
                        {
                            fasciaselezionata = periodolistino[0]; //Se trovo + fasce prendo la prima
                            error += "Errore trovate fasce di costo multiple per la data: " + _startcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 1 .Verificare";
                        }
                        else if (periodolistino == null)
                        {
                            error += "Non trovata fascia di costo per la data: " + _startcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 1 .Verificare";
                        }

                        if (fasciaselezionata != null)
                        {
                            prezzocalcolato += fasciaselezionata.Prezzo;
                        }
                        else
                        {
                            error += "Non trovato fascia per il calcolo dei costi nel periodo: " + _startcalc.ToShortDateString() + "  -  " + _endcalc.ToShortDateString();
                        }
                        _startcalc = _startcalc.AddDays(1);//avanzo al giorno successivo
                    }
                }
            }
            return prezzocalcolato;
        }

#endif

        /// <summary>
        /// Calcola il prezzo in base alle date ed ai listini !! presuppone la presenza delle fasce di costo per i periodi di interesse senno non calcola
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="eventstart"></param>
        /// <param name="eventend"></param>
        /// <param name="idattivita"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public double CalcolaPrezzoBase(string connection, DateTime eventstart, DateTime eventend, string idattivita, ref string error)
        {
            bool coincidenzatipoannotabellalistino = false;
            double prezzocalcolato = 0;
            int ngiorni = 0;
            int nsettimane = Math.DivRem(((TimeSpan)(eventend.Date - eventstart.Date)).Days, 7, out ngiorni);
            List<Listino> fascesettimanali = new List<Listino>();
            List<Listino> fascegiornaliere = new List<Listino>();

            //Ora devo calcolare l'importo base suddividendo in periodi settimanali e giornalieri e calcolando il costo in base alle tariffe di listino
            //tipologia tariff a giornaliera 1 - settimanale 2

            //CARICO LE FASCE DI COSTO INTERESSATE DALLA TABELLA LISTINO (SETTIMANALI)
            fascesettimanali = CaricaFasceCostoByAttivitaTipotariffa(connection, idattivita, "2");
            if (fascesettimanali == null || fascesettimanali.Count() == 0) //Se non presenti le fasce settimanali uso solo il giornaliero
                ngiorni = ((TimeSpan)(eventend.Date - eventstart.Date)).Days;
            //CARICO LE FASCE DI COSTO INTERESSATE DALLA TABELLA LISTINO (GIORNALIERE)
            fascegiornaliere = CaricaFasceCostoByAttivitaTipotariffa(connection, idattivita, "1");

            //UNICO ANNO RIFERIMENTO PER I LISTINI LE FASCE DEVONO ESSERE SEMPRE IN UN UNICO ANNO
            fascesettimanali = fascesettimanali.OrderByDescending(o => o.Enddate).ToList();
            //elimino le fasce fuori dall'anno ( commentato faccio un filtro dopo )
            //int annoref = fascesettimanali.First().Enddate.Year;
            //fascesettimanali.RemoveAll(f => f.Enddate.Year != annoref || f.Startdate.Year != annoref);

            fascegiornaliere = fascegiornaliere.OrderByDescending(o => o.Enddate).ToList();
            //elimino le fasce fuori dall'anno ( commentato faccio un filtro dopo )
            //int annoref1 = fascegiornaliere.First().Enddate.Year;
            //fascegiornaliere.RemoveAll(f => f.Enddate.Year != annoref1 || f.Startdate.Year != annoref1);
            ////////////////////////////////

            //////////////VERIFICHE SU ANNO BISESTILE (da spostare dentro al ciclo sotto !!!!  ) //////////////////////
            //bool annolistinobisestile = true;
            //if (fascesettimanali != null && fascesettimanali.Count() > 0) { annolistinobisestile = DateTime.IsLeapYear(fascesettimanali.First().Enddate.Year); }
            //else if (fascegiornaliere != null && fascegiornaliere.Count() > 0) { annolistinobisestile = DateTime.IsLeapYear(fascegiornaliere.First().Enddate.Year); }
            //bool annorichiestabisestile = DateTime.IsLeapYear(eventstart.Year);
            //if (annolistinobisestile == annorichiestabisestile) coincidenzatipoannotabellalistino = true;
            ////////////////////////////

            //CALCOLIAMO IL COSTO IN RELAZIONE ALLE SETTIMANE PRESENTI NEL PERIODO INDICATO
            DateTime _startcalc = eventstart;
            DateTime _endcalc = eventstart.AddDays(7);
            if (nsettimane != 0)
            {
                if (fascesettimanali != null && fascesettimanali.Count() > 0)
                {
                    for (int _i = 1; _i <= nsettimane; _i++) //Ciclo sulle settimane del periodo richiesto
                    {
                        Listino fasciadicostostart = null;
                        Listino fasciadicostoend = null;

                        //CONTROLLO SULLA DATA INZIO SETTIMANA------------------------------------------------------------------------
                        List<Listino> settimanaselezionata = null;
                        settimanaselezionata = fascesettimanali.Where(c => _startcalc >= c.Startdate && _startcalc <= c.Enddate).ToList();

                        //Controllo che sia uscita una sola fascia di listino
                        if (settimanaselezionata != null && settimanaselezionata.Count == 1)
                            fasciadicostostart = settimanaselezionata[settimanaselezionata.Count - 1];
                        else if (settimanaselezionata != null && settimanaselezionata.Count > 1)
                        {
                            //fasciadicostostart = settimanaselezionata[settimanaselezionata.Count - 1];
                            error += "Errore trovate fasce di costo multiple per la data: " + _startcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 2 .Verificare";
                        }
                        else if (settimanaselezionata == null)
                        {
                            error += "Non trovata fascia di costo per la data: " + _startcalc.ToShortDateString() + " alloggio: " + idattivita + " tipotariffa: 2 .Verificare";
                        }
                        //---------------------------------------------------------------------------------------------------------------

                        //CONTROLLI SULLA DATA DI FINE SETTIMANA-------------------------------------------------------------------------
                        settimanaselezionata = fascesettimanali.Where(c => _endcalc >= c.Startdate && _endcalc <= c.Enddate).ToList();

                        //Controllo che sia uscita una sola fascia di listino
                        if (settimanaselezionata != null && settimanaselezionata.Count == 1)
                            fasciadicostoend = settimanaselezionata[settimanaselezionata.Count - 1];
                        else if (settimanaselezionata != null && settimanaselezionata.Count > 1)
                        {
                            //fasciadicostoend = settimanaselezionata[settimanaselezionata.Count - 1];
                            error += "Errore trovate fasce di costo multiple per la data: " + _endcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 2 .Verificare";
                        }
                        else if (settimanaselezionata == null)
                        {
                            error += "Non trovata fascia di costo per la data: " + _endcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 2 .Verificare";
                        }

                        //---------------------------------------------------------------------------------------------------------------
                        //Passiamo al calcolo del costo in base al periodo richiesto
                        //Il criterio è tale che per il costo della settimana scelgo la fascia a costo maggiore tra quelle trovate anche se 
                        //la settimana cade a cavallo di due periodi di costo
                        if (fasciadicostostart != null && fasciadicostoend != null)
                        {
                            //Da fare diverso!!-> vedere se coincidono la fasciaster ed end >> prendi il prezzo , se non coincidono -> siamo a cavallo di due fasce quindi devo fare un calcolo in base alla sovrapposizione delle date!!!
                            //da fare ..
                            if (fasciadicostostart.Startdate == fasciadicostoend.Startdate && fasciadicostostart.Enddate == fasciadicostoend.Enddate)
                            {
                                prezzocalcolato += fasciadicostostart.Prezzo;
                            }
                            else // fasce start e fine diverse ( devo calcolare il prezzo in base alla percentuale di sovarpposizione nelle fasce in settimi delle date di inizio e fine periodo
                            {
                                ///////////////////////////////VERIFICARE !!!! DEBUG
                                double dayinprimafascia = 0;
                                dayinprimafascia = ((TimeSpan)(fasciadicostostart.Enddate.Date - _startcalc.Date)).Days + 1; //Ultima notte sempre nel periodo iniziale
                                double dayinsecondafascia = 0;
                                dayinsecondafascia = ((TimeSpan)(_endcalc.Date - fasciadicostoend.Startdate.Date)).Days;
                                double perc1fascia = dayinprimafascia / (dayinprimafascia + dayinsecondafascia);
                                double perc2fascia = dayinsecondafascia / (dayinprimafascia + dayinsecondafascia);
                                prezzocalcolato += perc1fascia * fasciadicostostart.Prezzo + perc2fascia * fasciadicostoend.Prezzo;
                                ///////////////////////////////VERIFICARE !!!! DEBUG
                            }

                            //if (fasciadicostostart.Prezzo > fasciadicostoend.Prezzo)
                            //    prezzocalcolato += fasciadicostostart.Prezzo;
                            //else
                            //    prezzocalcolato += fasciadicostoend.Prezzo;
                        }
                        else
                        {
                            error += "Non trovato (oppure valori multipli) fascia per il calcolo dei costi nel periodo : " + _startcalc.ToShortDateString() + "  -  " + _endcalc.ToShortDateString();
                        }

                        //Passiamo ala settimana successiva per il cumulo dei costi totali
                        _startcalc = _startcalc.AddDays(7);
                        _endcalc = _startcalc.AddDays(7);

                    }
                }

            }

            //CALCOLIAMO IL COSTO IN BASE AI GIORNI AGGIUNTIVI PRESENTI NEL PERIODO RICHIESTO (_startcalc)
            if (ngiorni != 0)
            {
                if (fascegiornaliere != null && fascegiornaliere.Count() > 0)
                {
                    for (int _i = 1; _i <= ngiorni; _i++) //Ciclo sui giorni del periodo richiesto
                    {
                        Listino fasciaselezionata = null;

                        //List<Listino> periodolistino = fascegiornaliere.Where(c => _startcalc >= c.startdate && _startcalc <= c.enddate).ToList();
                        List<Listino> periodolistino = null;
                        periodolistino = fascegiornaliere.Where(c => _startcalc >= c.Startdate && _startcalc <= c.Enddate).ToList();

                        //Controllo che sia uscita una sola fascia di listino
                        if (periodolistino != null && periodolistino.Count == 1)
                            fasciaselezionata = periodolistino[0];
                        else if (periodolistino != null && periodolistino.Count > 1)
                        {
                            // fasciaselezionata = periodolistino[0]; //Se trovo + fasce prendo la prima
                            error += "Errore trovate fasce di costo multiple per la data: " + _startcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 1 .Verificare";
                        }
                        else if (periodolistino == null)
                        {
                            error += "Non trovata fascia di costo per la data: " + _startcalc.ToShortDateString() + " id: " + idattivita + " tipotariffa: 1 .Verificare";
                        }

                        if (fasciaselezionata != null)
                        {
                            prezzocalcolato += fasciaselezionata.Prezzo;
                        }
                        else
                        {
                            error += "Non trovato fascia per il calcolo dei costi nel periodo: " + _startcalc.ToShortDateString() + "  -  " + _endcalc.ToShortDateString();
                        }
                        _startcalc = _startcalc.AddDays(1);//avanzo al giorno successivo
                    }
                }
            }
            return prezzocalcolato;
        }



        #endregion


        #region REGIONE GESTIONE CALENDARIO PRENOTAZIONI
        /// <summary>
        /// Memoria per i vincoli la key è idattivita vincolata e nel valore ci sono gli idattivita vincolanti separati da virgole ( da riempire a startup )
        /// </summary>
        public static Dictionary<string, string> vincolistrutture = new Dictionary<string, string>();

        public static void InitVincoli(Dictionary<string, string> vincoli = null)
        {
            if (vincoli == null)
            {
                ///VINCOLI HARD CODED DA METTERE IN FUNZIONE INIT
                ////////////////////////////////////////////////////////////////
                vincolistrutture = new Dictionary<string, string>();
                vincolistrutture.Add("8", "9,10");
                vincolistrutture.Add("9", "8");
                vincolistrutture.Add("10", "8");
                ////////////////////////////////////////////////////////////////
            }
            else
                vincolistrutture = vincoli;
        }

        public static string GeneraVincolo(string idattivita)
        {
            string ret = "";
            foreach (KeyValuePair<string, string> kv in vincolistrutture)
            {
                if (idattivita == kv.Key)
                    ret += kv.Value + ",";
            }
            ret = ret.TrimEnd(',');

            return ret;
        }

        public static bool VerificaPresenzaCodicepreventivo(string connection, string codice)
        {
            bool flag = true;
            if (connection == null || connection == "") return true;
            if (codice == null || codice == "") return true;
            try
            {
                string query = "SELECT count(*) as occorrenze FROM TBL_Eventi where codicerichiesta=@codicerichiesta";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@codicerichiesta", codice);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return flag; };
                    if (reader.HasRows == false)
                        return flag;

                    while (reader.Read())
                    {
                        long n = reader.GetInt64(reader.GetOrdinal("occorrenze"));
                        if (n > 0) flag = false;
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore VerificaPresenzaCodicepreventivo :" + error.Message, error);
            }
            return flag;
        }
        /// <summary>
        /// Genero un nuovo codice preventivo a partire dall'ultimo presente in tabella
        /// </summary>
        /// <returns></returns>
        public static string GeneraCodicePreventivo(string connection)
        {
            string CodicePreventivo = "";
            try
            {
                string query = "SELECT codicerichiesta FROM TBL_Eventi where (trim(codicerichiesta) <> '' and codicerichiesta is not null) order by codicerichiesta COLLATE NOCASE desc limit 1";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader != null)
                        while (reader.Read())
                        {
                            CodicePreventivo = reader.GetString(reader.GetOrdinal("codicerichiesta"));
                            break;
                        }
                }
            }
            catch
            {
                //throw new ApplicationException("Errore VerificaPresenzaCodicepreventivo :" + error.Message, error);
            }
            //controllo che non sia vuoto
            if (!string.IsNullOrEmpty(CodicePreventivo))
            {
                //non è vuoto, quindi lo prendo e faccio piu uno
                if (CodicePreventivo.Contains("p"))
                {
                    string tmp_cod = CodicePreventivo.Substring(1);
                    long int_cod = 0;
                    long.TryParse(tmp_cod, out int_cod);
                    int_cod = int_cod + 1;
                    CodicePreventivo = "p" + string.Format("{0:000000000}", int_cod);
                }
            }
            else
            {
                //se è vuoto gli assegno il primo
                CodicePreventivo = "p" + string.Format("{0:000000000}", 1);
            }
            return CodicePreventivo;
        }


        public static Eventi dbGetEvent(string connection, string idevento)
        {
            if (connection == null || connection == "") return null;
            if (idevento == null || idevento == "") return null;
            Eventi item = null;
            try
            {
                string query = "SELECT * FROM TBL_Eventi where idevento=@ID";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                long _i = 0;
                long.TryParse(idevento, out _i);
                SQLiteParameter p1 = new SQLiteParameter("@ID", _i);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        item = new Eventi();
                        item.Idevento = reader.GetInt64(reader.GetOrdinal("Idevento"));
                        item.Idattivita = reader.GetInt64(reader.GetOrdinal("Idattivita"));
                        item.Idvincolo = reader.GetString(reader.GetOrdinal("Idvincolo"));
                        item.Idcliente = reader.GetInt64(reader.GetOrdinal("Idcliente"));
                        item.Status = reader.GetInt64(reader.GetOrdinal("Status"));
                        item.Codicerichiesta = reader.GetString(reader.GetOrdinal("Codicerichiesta"));
                        item.Testoevento = reader.GetString(reader.GetOrdinal("Testoevento"));
                        item.Soggetto = reader.GetString(reader.GetOrdinal("Soggetto"));
                        item.Jsonfield1 = reader.GetString(reader.GetOrdinal("Jsonfield1"));
                        item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));
                        item.Datainserimento = reader.GetDateTime(reader.GetOrdinal("Datainserimento"));
                        item.Startdate = reader.GetDateTime(reader.GetOrdinal("Startdate"));
                        item.Enddate = reader.GetDateTime(reader.GetOrdinal("Enddate"));

                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore dbGetEvent :" + error.Message, error);
            }
            return item;
        }

        /// <summary>
        /// Seleziona in calendario tutti gli eventi che ricadono nel periodo start -> start+days
        /// </summary>
        /// <param name="start"></param>
        /// <param name="days"></param>
        /// <param name="idattivita"></param>
        /// <param name="testofiltro"></param>
        /// <param name="idcliente">se vuoto prende tutti i clienti</param>
        /// <returns></returns>
        public static List<Eventi> dbGetEvents(string connection, DateTime start, long days, string idattivita = "", long? status = null, string idcliente = "", string codicerichiesta = "", string testofiltro = "", bool includivincoli = false)
        {
            List<Eventi> eventi = new List<Eventi>();
            Eventi item = new Eventi();
            try
            {
                DateTime dateend = start.AddDays(days);

                string query = "SELECT * FROM TBL_Eventi ";
                string queryfilter = " where not ( Enddate < @startdate or Startdate > @enddate  ) ";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();

                long _i = 0;
                long.TryParse(idattivita, out _i);
                long _idcli = 0;
                long.TryParse(idcliente, out _idcli);

                SQLiteParameter p3 = new SQLiteParameter("@startdate", start);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@enddate", dateend);
                parColl.Add(p4);

                if (!string.IsNullOrEmpty(idcliente))
                {
                    SQLiteParameter pidcliente = new SQLiteParameter("@idcliente", _idcli);
                    parColl.Add(pidcliente);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idcliente = @idcliente ";
                    else
                        queryfilter += " AND idcliente = @idcliente  ";
                }

                string idvincolo = "";
                if (includivincoli) idvincolo = bookingDM.GeneraVincolo(idattivita);
                string[] codiciarray = idvincolo.Split(',');
                List<string> codici = codiciarray.ToList();
                if (!string.IsNullOrEmpty(idattivita))
                    codici.Add(_i.ToString());
                codici.RemoveAll(c => c == "");
                if (codici != null && codici.Count > 0)
                {
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idattivita in (    ";
                    else
                        queryfilter += " AND  idattivita in (      ";

                    foreach (string codice in codici)
                    {
                        if (!string.IsNullOrEmpty(codice.Trim()))
                        {
                            long codiceconv = 0;
                            if (long.TryParse(codice.Trim(), out codiceconv))
                                queryfilter += " " + codiceconv + " ,";
                        }
                    }
                    queryfilter = queryfilter.TrimEnd(',') + " ) ";
                }


                //if (!string.IsNullOrEmpty(idattivita))
                //{
                //    SQLiteParameter pidattivita = new SQLiteParameter("@idattivita", _i);
                //    parColl.Add(pidattivita);

                //    if (!queryfilter.ToLower().Contains("where"))
                //        queryfilter += " WHERE idattivita = @idattivita ";
                //    else
                //        queryfilter += " AND idattivita = @idattivita  ";
                //}
                if (!string.IsNullOrEmpty(codicerichiesta))
                {
                    SQLiteParameter pcodicerichiesta = new SQLiteParameter("@codicerichiesta", codicerichiesta);
                    parColl.Add(pcodicerichiesta);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE codicerichiesta like @codicerichiesta ";
                    else
                        queryfilter += " AND codicerichiesta like @codicerichiesta  ";
                }
                if (!string.IsNullOrEmpty(testofiltro))
                {
                    SQLiteParameter ptestoevento = new SQLiteParameter("@testoevento", "%" + testofiltro + "%");
                    parColl.Add(ptestoevento);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE testoevento like @testoevento or soggetto like @testoevento ";
                    else
                        queryfilter += " AND testoevento like @testoevento or soggetto like @testoevento  ";
                }
                if (status != null)
                {
                    SQLiteParameter pstatus = new SQLiteParameter("@status", status.Value);
                    parColl.Add(pstatus);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE status = @status ";
                    else
                        queryfilter += " AND status = @status  ";
                }

                query += queryfilter;
                query += " order by datainserimento asc";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Eventi();
                        item.Idevento = reader.GetInt64(reader.GetOrdinal("Idevento"));
                        item.Idattivita = reader.GetInt64(reader.GetOrdinal("Idattivita"));
                        item.Idvincolo = reader.GetString(reader.GetOrdinal("Idvincolo"));
                        item.Idcliente = reader.GetInt64(reader.GetOrdinal("Idcliente"));
                        item.Status = reader.GetInt64(reader.GetOrdinal("Status"));
                        item.Codicerichiesta = reader.GetString(reader.GetOrdinal("Codicerichiesta"));
                        item.Testoevento = reader.GetString(reader.GetOrdinal("Testoevento"));
                        item.Soggetto = reader.GetString(reader.GetOrdinal("Soggetto"));
                        item.Jsonfield1 = reader.GetString(reader.GetOrdinal("Jsonfield1"));
                        item.Prezzo = reader.GetDouble(reader.GetOrdinal("Prezzo"));
                        item.Datainserimento = reader.GetDateTime(reader.GetOrdinal("Datainserimento"));
                        item.Startdate = reader.GetDateTime(reader.GetOrdinal("Startdate"));
                        item.Enddate = reader.GetDateTime(reader.GetOrdinal("Enddate"));

                        eventi.Add(item);
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore dbGetEvents :" + error.Message, error);
            }

            return eventi;
        }

        public static string dbInsertEvent(string connessione, DateTime start, DateTime end, string idattivita, long status)
        {
            string idret = "";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;
            long _i = 0;
            long.TryParse(idattivita, out _i);
            SQLiteParameter p2 = new SQLiteParameter("@Idattivita", _i);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@status", status);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@Startdate", start);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Enddate", end);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@codicerichiesta", GeneraCodicePreventivo(connessione));
            parColl.Add(p6);

            string Idvincolo = "";// GeneraVincolo(idattivita); //Creo il vincolo dalla memoria dei vincoli
            SQLiteParameter p7 = new SQLiteParameter("@Idvincolo", Idvincolo);
            parColl.Add(p7);

            string query = "INSERT INTO TBL_Eventi ([Idattivita],[Startdate],[Enddate],[status],[codicerichiesta],[Idvincolo]) VALUES (@Idattivita,@Startdate,@Enddate,@status,@codicerichiesta,@Idvincolo)";
            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                idret = lastidentity.ToString(); //Inserisco nell'id dell'elemento inseito l'id generato dal db
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbInsertEvent :" + error.Message, error);
            }

            return idret;
        }

        public static string dbInsertEvent(string connessione, Eventi e)
        {
            string idret = "";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;

            SQLiteParameter p2 = new SQLiteParameter("@Idattivita", e.Idattivita);
            parColl.Add(p2);
            e.Idvincolo = "";// GeneraVincolo(e.Idattivita.ToString()); //Creo il vincolo dalla memoria dei vincoli
            SQLiteParameter p3 = new SQLiteParameter("@Idvincolo", e.Idvincolo);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@Startdate", e.Startdate);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Enddate", e.Enddate);
            parColl.Add(p5);
            string codicepreventivo = e.Codicerichiesta;
            if (string.IsNullOrEmpty(codicepreventivo)) codicepreventivo = GeneraCodicePreventivo(connessione);
            SQLiteParameter p6 = new SQLiteParameter("@codicerichiesta", codicepreventivo);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@Idcliente", e.Idcliente);
            parColl.Add(p7);
            SQLiteParameter p8 = new SQLiteParameter("@Prezzo", e.Prezzo);
            parColl.Add(p8);
            SQLiteParameter p9 = new SQLiteParameter("@Testoevento", e.Testoevento);
            parColl.Add(p9);
            SQLiteParameter p10 = new SQLiteParameter("@Status", e.Status);
            parColl.Add(p10);
            SQLiteParameter p11 = new SQLiteParameter("@soggetto", e.Soggetto);
            parColl.Add(p11);

            SQLiteParameter pJsonfield1 = new SQLiteParameter("@Jsonfield1", e.Jsonfield1);
            parColl.Add(pJsonfield1);

            string query = "INSERT INTO TBL_Eventi ([Idattivita],Idvincolo,[Startdate],[Enddate],[codicerichiesta],Idcliente,Prezzo,Testoevento,Status,Soggetto,Jsonfield1) VALUES (@Idattivita,@Idvincolo,@Startdate,@Enddate,@codicerichiesta,@Idcliente,@Prezzo,@Testoevento,@status,@soggetto,@Jsonfield1)";
            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                idret = lastidentity.ToString(); //Inserisco nell'id dell'elemento inseito l'id generato dal db
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbInsertEvent :" + error.Message, error);
            }

            return idret;
        }

        /// <summary>
        /// Aggiorna un evento solo come collocazione temporale, durata e risorsa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="idattivita"></param>
        /// <param name="status">opzionale non modificato se non passato</param>
        public static void dbUpdateEvent(string connessione, string id, DateTime start, DateTime end, string idattivita, long? status = null)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            string query = "UPDATE [TBL_Eventi] SET [Idattivita]=@Idattivita,Startdate=@Startdate,Enddate=@Enddate,Idvincolo=@Idvincolo ";

            long _i = 0;
            long.TryParse(id, out _i);
            if (_i == 0) return;
            long _ia = 0;
            long.TryParse(idattivita, out _ia);

            SQLiteParameter p2 = new SQLiteParameter("@Idattivita", _ia);
            parColl.Add(p2);
            SQLiteParameter p4 = new SQLiteParameter("@Startdate", start);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Enddate", end);
            parColl.Add(p5);
            if (status != null)
            {
                SQLiteParameter p6 = new SQLiteParameter("@status", status.Value);
                parColl.Add(p6);
                query += ",status=@status ";
            }
            string Idvincolo = "";// GeneraVincolo(idattivita); //Creo il vincolo dalla memoria dei vincoli
            SQLiteParameter p7 = new SQLiteParameter("@Idvincolo", Idvincolo);
            parColl.Add(p7);
            SQLiteParameter p1 = new SQLiteParameter("@idevento", _i);//OleDbType.VarChar
            parColl.Add(p1);

            query += " WHERE ([idevento] = @idevento)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbUpdateEvent :" + error.Message, error);
            }
        }

        /// <summary>
        /// Aggiornamento completo dati evento
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="e"></param>
        public static void dbUpdateEvent(string connessione, Eventi e)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            SQLiteParameter p2 = new SQLiteParameter("@Idattivita", e.Idattivita);
            parColl.Add(p2);

            string Idvincolo = "";// GeneraVincolo(e.Idattivita.ToString()); //Creo il vincolo dalla memoria dei vincoli
            SQLiteParameter p3 = new SQLiteParameter("@Idvincolo", Idvincolo);
            parColl.Add(p3);

            SQLiteParameter p4 = new SQLiteParameter("@Startdate", e.Startdate);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@Enddate", e.Enddate);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@codicerichiesta", e.Codicerichiesta);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@Idcliente", e.Idcliente);
            parColl.Add(p7);
            SQLiteParameter p8 = new SQLiteParameter("@Prezzo", e.Prezzo);
            parColl.Add(p8);
            SQLiteParameter p9 = new SQLiteParameter("@Testoevento", e.Testoevento);
            parColl.Add(p9);
            SQLiteParameter p10 = new SQLiteParameter("@Status", e.Status);
            parColl.Add(p10);
            SQLiteParameter p11 = new SQLiteParameter("@soggetto", e.Soggetto);
            parColl.Add(p11);
            SQLiteParameter pJsonfield1 = new SQLiteParameter("@Jsonfield1", e.Jsonfield1);
            parColl.Add(pJsonfield1);
            SQLiteParameter p1 = new SQLiteParameter("@idevento", e.Idevento);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "UPDATE [TBL_Eventi] SET [Idattivita]=@Idattivita,idvincolo=@idvincolo,idcliente=@idcliente,Startdate=@Startdate,Enddate=@Enddate,codicerichiesta=@codicerichiesta,Prezzo=@Prezzo,Testoevento=@Testoevento,Status=@Status,Soggetto=@Soggetto,Jsonfield1=@Jsonfield1 ";
            query += " WHERE ([idevento] = @idevento)";

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbUpdateEvent :" + error.Message, error);
            }
        }
        public static void dbDeleteEvent(string connessione, string idevento)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            long _i = 0;
            long.TryParse(idevento, out _i);
            if (_i == 0) return;

            SQLiteParameter p1 = new SQLiteParameter("@ID", _i);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "delete FROM TBL_Eventi WHERE ([idevento]=@ID)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbDeleteEvent :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Controlla se in un intervallo start - end è presente un evento per la risorsa indicata ( e per le eventuali risorse vincolanti ) che non abbia l'idevento passato
        /// torna true se non ci sono eventi in conflitto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static bool dbIsFree(string connessione, string idevento, DateTime start, DateTime end, string idattivita, bool soloconfermati = true, string idvincolo = "")
        {
            long count = 0;
            if (connessione == null || connessione == "") return count == 0;

            List<Eventi> eventi = new List<Eventi>();
            Eventi item = new Eventi();
            try
            {

                string query = "SELECT count(*) as neventi FROM TBL_Eventi  ";
                string queryfilter = " where not ( Enddate <= @startdate or Startdate >= @enddate  ) and idevento <> @idevento ";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();

                long _i = 0;

                long.TryParse(idattivita, out _i);

                long _idevento = 0;
                long.TryParse(idevento, out _idevento);

                SQLiteParameter pidevento = new SQLiteParameter("@idevento", _idevento);
                parColl.Add(pidevento);
                SQLiteParameter p3 = new SQLiteParameter("@startdate", start);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@enddate", end);
                parColl.Add(p4);
                if (soloconfermati)
                {
                    SQLiteParameter pstatus = new SQLiteParameter("@status", 1); //1 : solo stato confermata
                    parColl.Add(pstatus);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE status = @status ";
                    else
                        queryfilter += " AND status = @status  ";
                }
                //if (!string.IsNullOrEmpty(idattivita))
                //{
                //    SQLiteParameter pidattivita = new SQLiteParameter("@idattivita", _i);
                //    parColl.Add(pidattivita);

                //    if (!queryfilter.ToLower().Contains("where"))
                //        queryfilter += " WHERE idattivita = @idattivita ";
                //    else
                //        queryfilter += " AND idattivita = @idattivita  ";
                //}
                if (string.IsNullOrEmpty(idvincolo)) idvincolo = bookingDM.GeneraVincolo(idattivita);
                string[] codiciarray = idvincolo.Split(',');
                List<string> codici = codiciarray.ToList();
                codici.Add(_i.ToString());
                codici.RemoveAll(c => c == "");
                if (codici != null && codici.Count > 0)
                {
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idattivita in (    ";
                    else
                        queryfilter += " AND  idattivita in (      ";

                    foreach (string codice in codici)
                    {
                        if (!string.IsNullOrEmpty(codice.Trim()))
                        {
                            long codiceconv = 0;
                            if (long.TryParse(codice.Trim(), out codiceconv))
                                queryfilter += " " + codiceconv + " ,";
                        }
                    }
                    queryfilter = queryfilter.TrimEnd(',') + " ) ";
                }

                query += queryfilter;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connessione);
                using (reader)
                {
                    if (reader == null) { return count == 0; };
                    if (reader.HasRows == false)
                        return count == 0;
                    while (reader.Read())
                    {
                        count = reader.GetInt64(reader.GetOrdinal("neventi"));
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore dbIsFree :" + error.Message, error);
            }
            return count == 0;
        }

        /// <summary>
        /// Mi indica quante prenotazioni sono presenti per una attivita in un dato periodo
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idevento"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="idattivita"></param>
        /// <param name="escludidaconfermare"></param>
        /// <returns></returns>
        public static long ContaPrenotazioniNelPeriodo(string connessione, string idevento, DateTime start, DateTime end, string idattivita, bool escludidaconfermare = true)
        {
            long count = 0;
            if (connessione == null || connessione == "") return count;

            List<Eventi> eventi = new List<Eventi>();
            Eventi item = new Eventi();
            try
            {

                string query = "SELECT count(*) as neventi FROM TBL_Eventi  ";
                string queryfilter = " where not ( Enddate <= @startdate or Startdate >= @enddate  ) and idevento <> @idevento ";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();

                long _i = 0;
                long.TryParse(idattivita, out _i);

                long _idevento = 0;
                long.TryParse(idevento, out _idevento);

                SQLiteParameter pidevento = new SQLiteParameter("@idevento", _idevento);
                parColl.Add(pidevento);
                SQLiteParameter p3 = new SQLiteParameter("@startdate", start);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@enddate", end);
                parColl.Add(p4);

                if (!string.IsNullOrEmpty(idattivita))
                {
                    SQLiteParameter pidattivita = new SQLiteParameter("@idattivita", _i);
                    parColl.Add(pidattivita);

                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idattivita = @idattivita ";
                    else
                        queryfilter += " AND idattivita = @idattivita  ";
                }
                if (escludidaconfermare)
                {
                    SQLiteParameter pstatus = new SQLiteParameter("@status", 1); //1 : solo stato confermata
                    parColl.Add(pstatus);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE status = @status ";
                    else
                        queryfilter += " AND status = @status  ";
                }
                //string[] codiciarray = idvincolo.Split(',');
                //List<string> codici = codiciarray.ToList();
                //codici.Add(_i.ToString());
                //if (codici != null && codici.Count > 0)
                //{
                //    if (!queryfilter.ToLower().Contains("where"))
                //        queryfilter += " WHERE idattivita in (    ";
                //    else
                //        queryfilter += " AND  idattivita in (      ";

                //    foreach (string codice in codici)
                //    {
                //        if (!string.IsNullOrEmpty(codice.Trim()))
                //        {
                //            long codiceconv = 0;
                //            if (long.TryParse(codice.Trim(), out codiceconv))
                //                queryfilter += " " + codiceconv + " ,";
                //        }
                //    }
                //    queryfilter = queryfilter.TrimEnd(',') + " ) ";
                //}

                query += queryfilter;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connessione);
                using (reader)
                {
                    if (reader == null) { return count; };
                    if (reader.HasRows == false)
                        return count;
                    while (reader.Read())
                    {
                        count = reader.GetInt64(reader.GetOrdinal("neventi"));
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore ContaPrenotazioniNelPeriodo :" + error.Message, error);
            }
            return count;
        }

        /// <summary>
        /// Imposta confermate tutte le prenotazioni che hanno gli id nella lista passata
        /// </summary>
        /// <param name="guidprenotazioni"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ConfermaPrenotazioni(string connessione, List<string> ideventi, ref string message)
        {
            bool ret = false;
            if (connessione == null || connessione == "") return ret;
            if (ideventi == null) return true;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            try
            {
                foreach (string id in ideventi)
                {
                    parColl = new List<SQLiteParameter>();
                    long _i = 0;
                    long.TryParse(id, out _i);
                    SQLiteParameter p1 = new SQLiteParameter("@idevento", _i);//OleDbType.VarChar
                    parColl.Add(p1);
                    SQLiteParameter p6 = new SQLiteParameter("@status", 1);
                    parColl.Add(p6);
                    string query = "UPDATE [TBL_Eventi] SET [status]=@status WHERE ([idevento] = @idevento)";
                    try
                    {
                        dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                    }
                    catch (Exception err) { message += err.Message; }

                }
            }
            catch (Exception err)
            {
                ret = false;
                message = err.Message;
            }

            return ret;
        }


        /// <summary>
        /// Cancella tutte le prenotazioni non confermate con data minore di quella indicata oppure quelle col codicerichiesta indicato ma non confermate
        /// </summary>
        /// <param name="datalimite"></param>
        public static void CancellaPrenotazioniNonConfermate(string connessione, DateTime datalimite, string codicerichiesta = "")
        {

            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;


            SQLiteParameter p1 = new SQLiteParameter("@datalimite", datalimite);//OleDbType.VarChar
            parColl.Add(p1);

            SQLiteParameter p2 = new SQLiteParameter("@codicerichiesta", codicerichiesta);//OleDbType.VarChar
            parColl.Add(p2);

            string query = "delete FROM TBL_Eventi WHERE (datainserimento<@datalimite or length(@codicerichiesta) > 0 ) and status=0 and ( codicerichiesta like ( '%' || @codicerichiesta  || '%' ) or length(@codicerichiesta) = 0  ) ";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, dbDeleteEvent :" + error.Message, error);
            }
            return;

        }


        #endregion
    }
}
