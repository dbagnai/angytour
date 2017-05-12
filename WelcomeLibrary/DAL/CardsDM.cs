using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using System.Data.OleDb;

namespace WelcomeLibrary.DAL
{
    public class CardsDM
    {

        /// <summary>
        /// Torna tutte le card attivate che hanno scadenza tra la data attuale e la data attuale più il numero di giorni passato nei parametri
        /// </summary>
        /// <param name="ggascadere"></param>
        /// <returns></returns>
        public CardCollection EstraiCardsProssimeScadenza(string connection, double ggascadere)
        {

            CardCollection list = new CardCollection();
            DateTime _limite = System.DateTime.Now;
            _limite = _limite.AddDays(ggascadere);

            List<OleDbParameter> parColl = new List<OleDbParameter>();

            OleDbParameter p1 = new OleDbParameter("@dataoggi", System.DateTime.Now);//OleDbType.VarChar
            p1.OleDbType = OleDbType.Date;
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@limitescadenza", _limite);//OleDbType.VarChar
            p2.OleDbType = OleDbType.Date;
            parColl.Add(p2);

            // string query = "SELECT A.*,B.CodiceCard AS CodiceCard FROM TBL_CODICICARD A left join TBL_CLIENTI B on A.ID_LINK=B.ID_CARD ";
            string query = "SELECT * FROM TBL_CODICICARD ";
            string where = "";
            if (string.IsNullOrWhiteSpace(where))
                where += " WHERE  Dataattivazione is not null and (Dataattivazione+DurataGG) > @dataoggi and (Dataattivazione+DurataGG) < @limitescadenza ";
            else
                where += " AND  Dataattivazione is not null and (Dataattivazione+DurataGG) > @dataoggi and (Dataattivazione+DurataGG) < @limitescadenza ";

            query += where + " order BY DataAttivazione Desc";

            try
            {
                Card item;
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Card();
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard"));
                        if (!reader["DataGenerazione"].Equals(DBNull.Value))
                            item.DataGenerazione = reader.GetDateTime(reader.GetOrdinal("DataGenerazione"));
                        if (!reader["DataAttivazione"].Equals(DBNull.Value))
                            item.DataAttivazione = reader.GetDateTime(reader.GetOrdinal("DataAttivazione"));
                        if (!reader["DurataGG"].Equals(DBNull.Value))
                            item.DurataGG = reader.GetInt32(reader.GetOrdinal("DurataGG"));
                        if (!reader["AssegnatoACard"].Equals(DBNull.Value))
                            item.AssegnatoACard = reader.GetBoolean(reader.GetOrdinal("AssegnatoACard"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Lettura cards prossime a scadere :" + error.Message, error);
            }
            return list;
        }


        public CardCollection EstraiCardsProssimeScadenza(string connection, double ggascadere, ref string debugtext)
        {

            CardCollection list = new CardCollection();
            DateTime _limite = System.DateTime.Now;
            _limite = _limite.AddDays(ggascadere);

            List<OleDbParameter> parColl = new List<OleDbParameter>();

            OleDbParameter p1 = new OleDbParameter("@dataoggi", System.DateTime.Now);//OleDbType.VarChar
            p1.OleDbType = OleDbType.Date;
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@limitescadenza", _limite);//OleDbType.VarChar
            p2.OleDbType = OleDbType.Date;
            parColl.Add(p2);

            // string query = "SELECT A.*,B.CodiceCard AS CodiceCard FROM TBL_CODICICARD A left join TBL_CLIENTI B on A.ID_LINK=B.ID_CARD ";
            string query = "SELECT * FROM TBL_CODICICARD ";
            string where = "";
            if (string.IsNullOrWhiteSpace(where))
                where += " WHERE  Dataattivazione is not null and (Dataattivazione+DurataGG) > @dataoggi and (Dataattivazione+DurataGG) < @limitescadenza ";
            else
                where += " AND  Dataattivazione is not null and (Dataattivazione+DurataGG) > @dataoggi and (Dataattivazione+DurataGG) < @limitescadenza ";

            query += where + " order BY DataAttivazione Desc";


            debugtext = query;
            try
            {
                Card item;
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        debugtext += "Elementi trovati.";
                        item = new Card();
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard"));
                        if (!reader["DataGenerazione"].Equals(DBNull.Value))
                            item.DataGenerazione = reader.GetDateTime(reader.GetOrdinal("DataGenerazione"));
                        if (!reader["DataAttivazione"].Equals(DBNull.Value))
                            item.DataAttivazione = reader.GetDateTime(reader.GetOrdinal("DataAttivazione"));
                        if (!reader["DurataGG"].Equals(DBNull.Value))
                            item.DurataGG = reader.GetInt32(reader.GetOrdinal("DurataGG"));
                        if (!reader["AssegnatoACard"].Equals(DBNull.Value))
                            item.AssegnatoACard = reader.GetBoolean(reader.GetOrdinal("AssegnatoACard"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Lettura cards prossime a scadere :" + error.Message, error);
            }
            return list;
        }


        /// <summary>
        /// Crea nuovi codici per le card
        /// </summary>
        /// <param name="numero"></param>
        public List<string> GeneraNuoviCodici(int numero)
        {
            List<string> codici = new List<string>();
            string erroregenerazione = "";
            for (int i = 0; i < numero; i++)
            {
                string codice = RandomPassword.Generate(20);
                TimeSpan ts = new TimeSpan();
                DateTime _start = System.DateTime.Now;
                Card card = new Card();
                int maxwait = 30;//Attendo max 30 sec per la generazione e verifica nuovo codice
                bool error = true;
                while (error)
                {
                    try
                    {
                        ts = ((TimeSpan)(DateTime.Now - _start));
                        codice = RandomPassword.Generate(20);//genero nuovo codice
                        card.CodiceCard = codice;
                        card.DurataGG = 365;//Durata standard della card prefissata
                        if (ts.Seconds > maxwait)
                        {
                            erroregenerazione = "Errore durante la generazione dei codici. Non riuscita la generazione per il numero di codici richiesti.";
                            break;//fermo la generazione dei codici ( potrei segnalare un errore )
                        }
                        //aggiungiamo il codice al db codic
                        InserisciCodice(STATIC.Global.NomeConnessioneDb, card); //Incaso di inserimento codice duplicato o errore và in catch e si riprova per 30 secondi
                        codici.Add(codice);
                        error = false;
                    }
                    catch
                    {
                        error = true;
                    }
                }
                //if (error) throw new ApplicationException(erroregenerazione);
            }
            return codici;
        }

        /// <summary>
        /// Carica una card passando il codice della stessa
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codice"></param>
        /// <returns></returns>
        public Card CaricaCardPerCodice(string connection, string codice)
        {
            Card item = new Card();

            if (connection == null || connection == "") return item;
            //if (parColl == null || parColl.Count < 2) return list;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@CodiceCard", codice);//OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "";
                query = "SELECT * FROM TBL_CODICICARD WHERE CodiceCard=@CodiceCard";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Card();
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard"));
                        if (!reader["DataGenerazione"].Equals(DBNull.Value))
                            item.DataGenerazione = reader.GetDateTime(reader.GetOrdinal("DataGenerazione"));
                        if (!reader["DataAttivazione"].Equals(DBNull.Value))
                            item.DataAttivazione = reader.GetDateTime(reader.GetOrdinal("DataAttivazione"));
                        if (!reader["DurataGG"].Equals(DBNull.Value))
                            item.DurataGG = reader.GetInt32(reader.GetOrdinal("DurataGG"));
                        if (!reader["AssegnatoACard"].Equals(DBNull.Value))
                            item.AssegnatoACard = reader.GetBoolean(reader.GetOrdinal("AssegnatoACard"));

                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento card :" + error.Message, error);
            }

            return item;
        }



        /// <summary>
        /// Ritorna la lista della card inbase ai filtri passati
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="filtroattivate">Filtra i codici in base all'attivazione o meno.Valori possibili: si,no,tutte</param>
        /// <param name="filtroassegnate">Filtra i codici in base a quelli assegnati a card o meno.Valori possibili: si,no,tutte</param>
        /// <param name="filtroscadute">Filtra i codici attivati che sono scaduti o meno.Valori possibili: si,no,tutte</param>
        /// <returns></returns>
        public CardCollection CaricaCardFiltrate(string connection, string filtroattivate = "tutte", string filtroassegnate = "tutte", string filtroscadute = "tutte")
        {
            CardCollection list = new CardCollection();
            if (connection == null || connection == "") return list;
            //if (parColl == null || parColl.Count < 2) return list;
            List<OleDbParameter> parColl = new List<OleDbParameter>();

            Card item;

            try
            {
                string query = "";
                ////Vediamo se c'è la categoria nella parcoll -> faccio la join (usato per marchettini)
                //if (parColl.Exists(delegate(OleDbParameter tmp) { return tmp.ParameterName == "@CodiceCATEGORIA"; }))
                //{
                //    query = "SELECT A.*,B.CodiceTIPOLOGIA as CodiceTIPOLOGIA,B.codcat1 as codcat1 FROM TBL_ATTIVITA A left join dbo_TBLRIF_OFFERTE_LINK_LIV1 B on A.CodiceTIPOLOGIA=B.CodiceTipologia where A.CodiceTIPOLOGIA like @CodiceTIPOLOGIA and B.codcat1 like @CodiceCATEGORIA";
                //}
                //else //come prima (new moon e welcomehome)
                //query = "SELECT * FROM TBL_CODICICARD where CodicePROVINCIA like @CodicePROVINCIA and CodiceCOMUNE like @CodiceCOMUNE and CodiceTIPOLOGIA like @CodiceTIPOLOGIA and CodiceREGIONE like  @CodiceREGIONE and Prezzo >= @PrezzoMin and Prezzo <= @PrezzoMax order BY DataInserimento Desc";
                query = "SELECT * FROM TBL_CODICICARD ";
                //INSERIAMO I FILTRI INDICATI
                string where = "";
                switch (filtroattivate)
                {
                    case "si":
                        if (string.IsNullOrWhiteSpace(where))
                            where += " WHERE DataAttivazione is NOT null ";
                        else
                            where += " AND DataAttivazione is NOT null ";
                        break;
                    case "no":
                        if (string.IsNullOrWhiteSpace(where))
                            where += " WHERE DataAttivazione is null ";
                        else
                            where += " AND DataAttivazione is null ";
                        break;
                    default: //il default è tutte!!
                        break;
                }
                switch (filtroassegnate)
                {
                    case "si":
                        if (string.IsNullOrWhiteSpace(where))
                            where += " WHERE AssegnatoACard = true ";
                        else
                            where += " AND AssegnatoACard = true ";
                        break;
                    case "no":
                        if (string.IsNullOrWhiteSpace(where))
                            where += " WHERE AssegnatoACard = false ";
                        else
                            where += " AND AssegnatoACard = false ";
                        break;
                    default: //il default è tutte!!
                        break;
                }
                OleDbParameter p1 = null;
                switch (filtroscadute)
                {
                    case "si":
                         p1 = new OleDbParameter("@dataoggi", System.DateTime.Now);//OleDbType.VarChar
                        p1.OleDbType = OleDbType.Date;
                        parColl.Add(p1);
                        if (string.IsNullOrWhiteSpace(where))
                            where += " WHERE Dataattivazione is not null and (Dataattivazione+DurataGG) < @dataoggi";
                        else
                            where += " AND  Dataattivazione is not null and (Dataattivazione+DurataGG) < @dataoggi";
                        break;
                    case "no":
                         p1 = new OleDbParameter("@dataoggi", System.DateTime.Now);//OleDbType.VarChar
                        p1.OleDbType = OleDbType.Date;
                        parColl.Add(p1);

                        if (string.IsNullOrWhiteSpace(where))
                            where += " WHERE Dataattivazione is not null and (Dataattivazione+DurataGG) > @dataoggi";
                        else
                            where += " AND  Dataattivazione is not null and (Dataattivazione+DurataGG) > @dataoggi";

                        break;
                    default: //il default è tutte!!
                        break;
                }
                query += where + " order BY DataGenerazione Desc";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        item = new Card();
                        item.Id_card = reader.GetInt32(reader.GetOrdinal("ID_CARD"));
                        if (!reader["CodiceCard"].Equals(DBNull.Value))
                            item.CodiceCard = reader.GetString(reader.GetOrdinal("CodiceCard"));
                        if (!reader["DataGenerazione"].Equals(DBNull.Value))
                            item.DataGenerazione = reader.GetDateTime(reader.GetOrdinal("DataGenerazione"));
                        if (!reader["DataAttivazione"].Equals(DBNull.Value))
                            item.DataAttivazione = reader.GetDateTime(reader.GetOrdinal("DataAttivazione"));
                        if (!reader["DurataGG"].Equals(DBNull.Value))
                            item.DurataGG = reader.GetInt32(reader.GetOrdinal("DurataGG"));
                        if (!reader["AssegnatoACard"].Equals(DBNull.Value))
                            item.AssegnatoACard = reader.GetBoolean(reader.GetOrdinal("AssegnatoACard"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento cards :" + error.Message, error);
            }

            return list;
        }



        /// <summary>
        /// Inserisce in tabella un nuovo codice card
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="card"></param>
        public void InserisciCodice(string connessione, Card item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            OleDbParameter p1 = new OleDbParameter("@CodiceCard", item.CodiceCard);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@DataGenerazione", System.DateTime.Now.ToString());
            parColl.Add(p2);
            OleDbParameter p3 = null;
            if (item.DataAttivazione != null)
                p3 = new OleDbParameter("@DataAttivazione", item.DataAttivazione.Value);
            else
                p3 = new OleDbParameter("@DataAttivazione", System.DBNull.Value);
            parColl.Add(p3);
            OleDbParameter p4 = new OleDbParameter("@DurataGG", item.DurataGG);
            parColl.Add(p4);

            //INSERT INTO TBL_CODICICARD  (CodiceCard, DataGenerazione, DataAttivazione, DurataGG)  values (CodiceCard, DataGenerazione, DataAttivazione, DurataGG);

            string query = "INSERT INTO TBL_CODICICARD  (CodiceCard, DataGenerazione, DataAttivazione, DurataGG,Assegnatoacard)  values (@CodiceCard, @DataGenerazione, @DataAttivazione, @DurataGG, false)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento Codici card :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Aggiorna lo stato di assegnazione di un codice ad una card
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="cards"></param>
        public void AggiornaAssegnazioniCard(string connessione, CardCollection cards)
        {
            if (cards == null) return;

            List<OleDbParameter> parColl = new List<OleDbParameter>();

            List<Card> _list = cards.FindAll(delegate(Card c) { return c.AssegnatoACard; });
            if (_list != null && _list.Count > 0)
            {
                string query = "UPDATE [TBL_CODICICARD] SET [Assegnatoacard]=true WHERE [Id_card] IN ( ";
                foreach (Card c in _list)
                {
                    query += c.Id_card.ToString() + ",";
                }
                query = query.TrimEnd(',');
                query += " )";

                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento assegnazioni true a card :" + error.Message, error);
                }
            }
            _list = cards.FindAll(delegate(Card c) { return !c.AssegnatoACard; });
            if (_list != null && _list.Count > 0)
            {
                string query = "UPDATE [TBL_CODICICARD] SET [Assegnatoacard]=false WHERE [Id_card] IN ( ";
                foreach (Card c in _list)
                {
                    query += c.Id_card.ToString() + ",";
                }
                query = query.TrimEnd(',');
                query += " )";

                try
                {
                    dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                }
                catch (Exception error)
                {
                    throw new ApplicationException("Errore, aggiornamento assegnazioni false a card :" + error.Message, error);
                }
            }

        }

        /// <summary>
        /// Cancella dalla tabella tutti i codici non attivati e non assegnati a card che hanno superato i mesi di scadenza
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="mesiscadenza"></param>
        public void CancellaCodiciScaduti(string connessione, int mesiscadenza)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            TimeSpan TS = new TimeSpan(mesiscadenza * 30, 0, 0, 0);
            OleDbParameter p2 = new OleDbParameter("@DataGenerazione", System.DateTime.Now.Subtract(TS).ToString());
            p2.DbType = System.Data.DbType.DateTime;
            parColl.Add(p2);
            // OleDbParameter p3 = null;
            //if (item.DataAttivazione != null)
            //    p3 = new OleDbParameter("@DataAttivazione", item.DataAttivazione.Value);
            //else
            //    p3 = new OleDbParameter("@DataAttivazione", System.DBNull.Value);

            //INSERT INTO TBL_CODICICARD  (CodiceCard, DataGenerazione, DataAttivazione, DurataGG)  values (CodiceCard, DataGenerazione, DataAttivazione, DurataGG);

            string query = "DELETE * FROM TBL_CODICICARD WHERE DataAttivazione IS NULL AND DataGenerazione<@DataGenerazione and AssegnatoACard=false";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cacellazione Codici card :" + error.Message, error);
            }
            return;
        }


        /// <summary>
        /// Controllo che la card non sia già attiva o scaduta nel caso la attivo!!!
        /// Torna: attivata -> operazione correttamente eseguita, attiva -> la card era già attiva, nonpresente-> la card non è presente, errore -> errore nel processo di attivazione
        /// 
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public WelcomeLibrary.DOM.enumclass.StatoCard AttivaCodice(string codice)
        {
            string connessione = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            //bool ret = false;
            WelcomeLibrary.DOM.enumclass.StatoCard stato;
            try
            {
                Card c = this.CaricaCardPerCodice(connessione, codice);
                //Facciamo il test di attivazione se già attivata non viene riattivata !!!!!!
                if (c != null && !string.IsNullOrWhiteSpace(c.CodiceCard))
                {
                    if (c.DataAttivazione == null)
                    {
                        //Eseguo l'attivazione
                        List<OleDbParameter> parColl = new List<OleDbParameter>();
                        OleDbParameter p1 = new OleDbParameter("@dataoggi", System.DateTime.Now);//OleDbType.VarChar
                        p1.OleDbType = OleDbType.Date;
                        parColl.Add(p1);
                        string query = "UPDATE [TBL_CODICICARD] SET [DataAttivazione]=@dataoggi WHERE [CodiceCard] = '" + codice + "'";
                        dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                        stato = WelcomeLibrary.DOM.enumclass.StatoCard.attivata;
                    }
                    else stato = WelcomeLibrary.DOM.enumclass.StatoCard.attiva;
                }
                else stato = WelcomeLibrary.DOM.enumclass.StatoCard.nonpresente;
            }
            catch (Exception error)
            {
                //  throw new ApplicationException("Errore, attivzione card :" + error.Message, error);
                stato = WelcomeLibrary.DOM.enumclass.StatoCard.errore;
            }
            return stato;
        }

        /// <summary>
        /// Verifica se una card con un certo codice è ancora valida ed attivata corretttamente
        /// Torna : attiva-> card presente e attiva correttamente, scaduta-> card scaduta , nonattiva -> card presente e non attiva, nonpresente-> card non presente nel db, errore-> errore deurante la verifica
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public WelcomeLibrary.DOM.enumclass.StatoCard VerificaValiditaCodice(string codice)
        {
            string connessione = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            //bool ret = false;
            WelcomeLibrary.DOM.enumclass.StatoCard stato;

            // string stato = "";
            try
            {
                Card c = this.CaricaCardPerCodice(connessione, codice);
                if (c != null && !string.IsNullOrWhiteSpace(c.CodiceCard))
                {
                    if (c.DataAttivazione != null)
                    {
                        if (((DateTime)c.DataAttivazione).AddDays((int)c.DurataGG) > System.DateTime.Now)
                            stato = WelcomeLibrary.DOM.enumclass.StatoCard.attiva; //Codice vuoto -> correttamente attiva e in corso
                        else stato = WelcomeLibrary.DOM.enumclass.StatoCard.scaduta;
                    }
                    else stato = WelcomeLibrary.DOM.enumclass.StatoCard.nonattiva;
                }
                else stato = WelcomeLibrary.DOM.enumclass.StatoCard.nonpresente;
            }
            catch { stato = WelcomeLibrary.DOM.enumclass.StatoCard.errore; }
            return stato;
        }

        /// <summary>
        /// Verifica se una card ( passata per intero )  è ancora valida ed attivata corretttamente
        /// Torna : attiva-> card presente e attiva correttamente, scaduta-> card scaduta , nonattiva -> card presente e non attiva, nonpresente-> card non presente nel db, errore-> errore deurante la verifica
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public WelcomeLibrary.DOM.enumclass.StatoCard VerificaValiditaCard(Card c)
        {
            WelcomeLibrary.DOM.enumclass.StatoCard stato;

            // string stato = "";
            try
            {
                if (c != null && !string.IsNullOrWhiteSpace(c.CodiceCard))
                {
                    if (c.DataAttivazione != null)
                    {
                        if (((DateTime)c.DataAttivazione).AddDays((int)c.DurataGG) > System.DateTime.Now)
                            stato = WelcomeLibrary.DOM.enumclass.StatoCard.attiva; //Codice vuoto -> correttamente attiva e in corso
                        else stato = WelcomeLibrary.DOM.enumclass.StatoCard.scaduta;
                    }
                    else stato = WelcomeLibrary.DOM.enumclass.StatoCard.nonattiva;
                }
                else stato = WelcomeLibrary.DOM.enumclass.StatoCard.nonpresente;
            }
            catch { stato = WelcomeLibrary.DOM.enumclass.StatoCard.errore; }
            return stato;
        }


        /// <summary>
        /// Verifica se un codice è già presente nella tabella codici card. Torna true se il codice è presente in qualisasi stato esso sia.
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public bool VerificaPresenzaCodice(string codice)
        {
            string connessione = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            bool ret = false;
            try
            {
                Card c = this.CaricaCardPerCodice(connessione, codice);
                if (c != null && !string.IsNullOrWhiteSpace(c.CodiceCard))
                    ret = true;
            }
            catch { ret = false; }
            return ret;
        }

    }
}
