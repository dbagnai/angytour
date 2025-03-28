﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;

namespace WelcomeLibrary.DAL
{
    public class commentsDM
    {

        public Comment CaricaPerId(string connection, long Id)
        {
            if (connection == null || connection == "") return null;
            if (Id == 0) return null;

            Comment item = new Comment();
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();

            SQLiteParameter p1 = new SQLiteParameter("@id", Id); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT  * FROM TBL_comments  WHERE Id = @id ";
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Comment();
                        // SELECT TBL_comments.ID, TBL_comments.idpost, TBL_comments.data, TBL_comments.autore, TBL_comments.email, 
                        //TBL_comments.nome, TBL_comments.codiceimmobile, TBL_comments.testo, TBL_comments.titolo
                        item.Id = reader.GetInt64(reader.GetOrdinal("id"));
                        item.Idpost = reader.GetInt64(reader.GetOrdinal("idpost"));
                        item.Idcollegato = reader.GetInt64(reader.GetOrdinal("idcollegato"));
                        item.stelle = reader.GetDouble(reader.GetOrdinal("stelle"));
                        if (!reader["data"].Equals(DBNull.Value))
                            item.Data = reader.GetDateTime(reader.GetOrdinal("data"));
                        if (!reader["autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("autore"));
                        if (!reader["email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("email"));
                        if (!reader["nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("nome"));
                        if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));

                        if (!reader["testoI"].Equals(DBNull.Value))
                            item.TestoI = reader.GetString(reader.GetOrdinal("testoI"));
                        if (!reader["titoloI"].Equals(DBNull.Value))
                            item.TitoloI = reader.GetString(reader.GetOrdinal("titoloI"));

                        if (!reader["testoGB"].Equals(DBNull.Value))
                            item.TestoGB = reader.GetString(reader.GetOrdinal("testoGB"));
                        if (!reader["titoloGB"].Equals(DBNull.Value))
                            item.TitoloGB = reader.GetString(reader.GetOrdinal("titoloGB"));

                        if (!reader["testoRU"].Equals(DBNull.Value))
                            item.TestoRU = reader.GetString(reader.GetOrdinal("testoRU"));
                        if (!reader["titoloRU"].Equals(DBNull.Value))
                            item.TitoloRU = reader.GetString(reader.GetOrdinal("titoloRU"));

                        if (!reader["testoFR"].Equals(DBNull.Value))
                            item.TestoFR = reader.GetString(reader.GetOrdinal("testoFR"));
                        if (!reader["titoloFR"].Equals(DBNull.Value))
                            item.TitoloFR = reader.GetString(reader.GetOrdinal("titoloFR"));

                        if (!reader["testoES"].Equals(DBNull.Value))
                            item.TestoES = reader.GetString(reader.GetOrdinal("testoES"));
                        if (!reader["titoloES"].Equals(DBNull.Value))
                            item.TitoloES = reader.GetString(reader.GetOrdinal("titoloES"));

                        if (!reader["testoDE"].Equals(DBNull.Value))
                            item.TestoDE = reader.GetString(reader.GetOrdinal("testoDE"));
                        if (!reader["titoloDE"].Equals(DBNull.Value))
                            item.TitoloDE = reader.GetString(reader.GetOrdinal("titoloDE"));

                        if (!reader["spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("spare1"));
                        if (!reader["Approvato"].Equals(DBNull.Value))
                            item.Approvato = reader.GetBoolean(reader.GetOrdinal("Approvato"));

                        break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento  :" + error.Message, error);
            }
            return item;
        }
        public static Dictionary<long, List<double>> Calcolamediastarsbyid(string connection, List<Offerte> list, long limitresults = 0)
        {
            //Reverse sort dictionary
            //var retdict =  new SortedDictionary<long, long>(Comparer<long>.Create((x, y) => y.CompareTo(x)));
            Dictionary<long, List<double>> retdict = new Dictionary<long, List<double>>();
            StringBuilder sb = new StringBuilder();
            string query = "";
            if (connection == null || connection == "") return retdict;
            try
            {
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                query += ("SELECT sum(stelle) as totalestars, count(idpost) as nvotiperid, idpost  FROM TBL_comments ");
                if (list != null && list.Count > 0)
                {
                    query += ("WHERE idpost in ( ");

                    foreach (Offerte c in list)
                    {
                        query += (" " + c.Id + ",");
                    }

                    query = query.TrimEnd(',');
                    query += " )   AND  (Approvato = 1) ";
                }

                query += " Group by idpost order by totalestars DESC  ";

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
                        double totalestars = reader.GetDouble(reader.GetOrdinal("totalestars"));
                        long nvotiperid = reader.GetInt64(reader.GetOrdinal("nvotiperid"));
                        long idpost = reader.GetInt64(reader.GetOrdinal("idpost"));

                        if (!retdict.ContainsKey(idpost))
                        {
                            //retdict.Add(idpost, Math.Round(((double)totalestars / nvotiperid), 1, MidpointRounding.ToEven));
                            retdict.Add(idpost, new List<double>());
                            retdict[idpost].Add(Math.Round(((double)totalestars / nvotiperid), 1, MidpointRounding.ToEven));
                            retdict[idpost].Add(nvotiperid);
                        }
                    }
                }
                //Dictionary<long, long> sortedict = new Dictionary<long, long>();
                var sortedDict = (from entry in retdict orderby entry.Value descending select entry);
                retdict = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch (Exception error)
            {
                // throw new ApplicationException("Errore Conteggio totale stelle :" + error.Message, error);
            }
            return retdict;
        }


        public CommentsCollection CaricaCommentiFiltratiScript(string connection, string idpost, bool? approvato = null, string maxrecord = "", long page = 0, long pagesize = 0)
        {
            CommentsCollection list = new CommentsCollection();
            try
            {
                List<SQLiteParameter> pars = new List<SQLiteParameter>();
                if (idpost != "")
                {
                    if (long.TryParse(idpost, out var id))
                    {
                        SQLiteParameter pidpost = new SQLiteParameter("@idpost", id);
                        pars.Add(pidpost);
                        if (approvato != null)
                        {
                            SQLiteParameter pappro = new SQLiteParameter("@approvato", approvato.Value);
                            pars.Add(pappro);
                        }
                        list = CaricaCommentiFiltratiScript(connection, pars, maxrecord, page, pagesize);
                    }
                    else if (idpost == "all")
                    {
                        if (approvato != null)
                        {
                            SQLiteParameter pappro = new SQLiteParameter("@approvato", approvato.Value);
                            pars.Add(pappro);
                        }
                        list = CaricaCommentiFiltratiScript(connection, pars, maxrecord, page, pagesize);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento :" + error.Message, error);
            }
            return list;
        }

        public CommentsCollection CaricaCommentiFiltratiScript(string connection, List<SQLiteParameter> parColl, string maxrecord = "", long page = 0, long pagesize = 0)
        {
            CommentsCollection list = new CommentsCollection();
            if (connection == null || connection == "") return list;
            if ((parColl == null || parColl.Count < 1) && (page != 0 && pagesize > 1000)) return list;

            Comment item;
            try
            {
                List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();
                string query = "";
                string queryfilter = "";


                query = "SELECT * FROM TBL_comments ";


                //Per ogni parametro vedo se esiste e lo inserisco nello script
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@id"; }))
                {
                    SQLiteParameter pid = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@id"; });
                    _parUsed.Add(pid);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE id like @id ";
                    else
                        queryfilter += " AND id like @id  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idpost"; }))
                {
                    SQLiteParameter pidpost = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@idpost"; });
                    _parUsed.Add(pidpost);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE idpost like @idpost ";
                    else
                        queryfilter += " AND idpost like @idpost  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@data"; }))
                {
                    SQLiteParameter pdata = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@data"; });
                    _parUsed.Add(pdata);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE data like @data ";
                    else
                        queryfilter += " AND data like @data  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@autore"; }))
                {
                    SQLiteParameter pautore = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@autore"; });
                    _parUsed.Add(pautore);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE autore like @autore ";
                    else
                        queryfilter += " AND autore like @autore  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@email"; }))
                {
                    SQLiteParameter pemail = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@email"; });
                    _parUsed.Add(pemail);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE email like @email ";
                    else
                        queryfilter += " AND email like @email  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@nome"; }))
                {
                    SQLiteParameter pnome = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@nome"; });
                    _parUsed.Add(pnome);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE  nome like @nome  ";
                    else
                        queryfilter += " AND  nome like @nome   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Spare2"; }))
                {
                    SQLiteParameter pSpare2 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@Spare2"; });
                    _parUsed.Add(pSpare2);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE Spare2 like @Spare2 ";
                    else
                        queryfilter += " AND Spare2 like @Spare2  ";
                }

                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@testo"; }))
                {
                    SQLiteParameter ptesto = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@testo"; });
                    _parUsed.Add(ptesto);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE testoI like @testo or testoGB like @testo or testoRU like @testo  or testoFR like @testo  or testoDE like @testo  or testoES like @testo ";
                    else
                        queryfilter += " AND testoI like @testo  or testoGB like @testo or testoRU like @testo  or testoFR like @testo  or testoDE like @testo  or testoES like @testo  ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@titolo"; }))
                {
                    SQLiteParameter ptitolo = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@titolo"; });
                    _parUsed.Add(ptitolo);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE titoloI like @titolo  or titoloGB like @titolo  or titoloRU like @titolo  or titoloFR like @titolo or titoloDE like @titolo or titoloES like @titolo ";
                    else
                        queryfilter += " AND  titoloI like @titolo  or titoloGB like @titolo  or titoloRU like @titolo  or titoloFR like @titolo or titoloDE like @titolo or titoloES like @titolo ";
                }


                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@spare1"; }))
                {
                    SQLiteParameter pspare1 = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@spare1"; });
                    _parUsed.Add(pspare1);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE spare1 = @spare1 ";
                    else
                        queryfilter += " AND  spare1 = @spare1   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@approvato"; }))
                {
                    SQLiteParameter papprovato = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@approvato"; });
                    _parUsed.Add(papprovato);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE approvato = @approvato ";
                    else
                        queryfilter += " AND  approvato = @approvato   ";
                }
                if (parColl.Exists(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@testoricerca"; }))
                {
                    SQLiteParameter testoricerca = parColl.Find(delegate (SQLiteParameter tmp) { return tmp.ParameterName == "@testoricerca"; });
                    _parUsed.Add(testoricerca);
                    if (!queryfilter.ToLower().Contains("where"))
                        queryfilter += " WHERE (  titoloI like @titolo or titoloGB like @titolo  or titoloRU like @titolo or titoloFR like @titolo or titoloDE like @titolo or titoloES like @titolo  or  testoI like @testo  or testoGB like @testo or testoRU like @testo or testoFR like @testo or testoDE like @testo or testoES like @testo  or spare1 like @spare1) ";
                    else
                        queryfilter += " AND (  titoloI like @titolo or titoloGB like @titolo  or titoloRU like @titolo or titoloFR like @titolo or titoloDE like @titolo or titoloES like @titolo  or  testoI like @testo  or testoGB like @testo or testoRU like @testo or testoFR like @testo   or testoDE like @testo  or testoES like @testo  or spare1 like @spare1) ";
                }

                query += queryfilter; //query da fare per i risultati


                query += "  order BY data Desc , ID desc ";
                if (!string.IsNullOrEmpty(maxrecord))
                    query += " LIMIT " + maxrecord;
                else
                {
                    if (pagesize != 0)
                    {
                        query += " limit " + (page - 1) * pagesize + "," + pagesize;
                    }
                }


                //Calcolo records totali e calcolo la media .stelle  e totale .approvato
                long totalrecords = dbDataAccess.ExecuteScalar<long>("SELECT count(*) FROM TBL_comments " + queryfilter, _parUsed, connection);
                list.Recordstotali = totalrecords;
                if (!queryfilter.ToLower().Contains("where"))
                    queryfilter += " WHERE (Approvato = 1)  ";
                else
                    queryfilter += " AND  (Approvato = 1)  ";
                long approvati = dbDataAccess.ExecuteScalar<long>("SELECT count(*) FROM TBL_comments " + queryfilter, _parUsed, connection);
                list.Napprovati = approvati;
                double sommastelle = dbDataAccess.ExecuteScalar<double>("SELECT sum(stelle) FROM TBL_comments " + queryfilter, _parUsed, connection);
                if (approvati != 0)
                    list.Mediatotalestars = Math.Round(((double)sommastelle / approvati), 1, MidpointRounding.ToEven);

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                long progressivo = 0;
                //double mediatotalestars = 0;
                //long napprovati = 0;
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        progressivo += 1;
                        item = new Comment();
                        item.Id = reader.GetInt64(reader.GetOrdinal("id"));
                        item.Idpost = reader.GetInt64(reader.GetOrdinal("idpost"));
                        item.Idcollegato = reader.GetInt64(reader.GetOrdinal("idcollegato"));
                        item.stelle = reader.GetDouble(reader.GetOrdinal("stelle"));
                        if (!reader["data"].Equals(DBNull.Value))
                            item.Data = reader.GetDateTime(reader.GetOrdinal("data"));
                        if (!reader["autore"].Equals(DBNull.Value))
                            item.Autore = reader.GetString(reader.GetOrdinal("autore"));
                        if (!reader["email"].Equals(DBNull.Value))
                            item.Email = reader.GetString(reader.GetOrdinal("email"));
                        if (!reader["nome"].Equals(DBNull.Value))
                            item.Nome = reader.GetString(reader.GetOrdinal("nome"));

                        if (!reader["testoI"].Equals(DBNull.Value))
                            item.TestoI = reader.GetString(reader.GetOrdinal("testoI"));
                        if (!reader["titoloI"].Equals(DBNull.Value))
                            item.TitoloI = reader.GetString(reader.GetOrdinal("titoloI"));
                        if (!reader["testoGB"].Equals(DBNull.Value))
                            item.TestoGB = reader.GetString(reader.GetOrdinal("testoGB"));
                        if (!reader["titoloGB"].Equals(DBNull.Value))
                            item.TitoloGB = reader.GetString(reader.GetOrdinal("titoloGB"));
                        if (!reader["testoRU"].Equals(DBNull.Value))
                            item.TestoRU = reader.GetString(reader.GetOrdinal("testoRU"));
                        if (!reader["titoloRU"].Equals(DBNull.Value))
                            item.TitoloRU = reader.GetString(reader.GetOrdinal("titoloRU"));
                        if (!reader["testoFR"].Equals(DBNull.Value))
                            item.TestoFR = reader.GetString(reader.GetOrdinal("testoFR"));
                        if (!reader["titoloFR"].Equals(DBNull.Value))
                            item.TitoloFR = reader.GetString(reader.GetOrdinal("titoloFR"));

                        if (!reader["testoES"].Equals(DBNull.Value))
                            item.TestoES = reader.GetString(reader.GetOrdinal("testoES"));
                        if (!reader["titoloES"].Equals(DBNull.Value))
                            item.TitoloES = reader.GetString(reader.GetOrdinal("titoloES"));

                        if (!reader["testoDE"].Equals(DBNull.Value))
                            item.TestoDE = reader.GetString(reader.GetOrdinal("testoDE"));
                        if (!reader["titoloDE"].Equals(DBNull.Value))
                            item.TitoloDE = reader.GetString(reader.GetOrdinal("titoloDE"));

                        if (!reader["Spare2"].Equals(DBNull.Value))
                            item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        if (!reader["spare1"].Equals(DBNull.Value))
                            item.Spare1 = reader.GetString(reader.GetOrdinal("spare1"));
                        if (!reader["Approvato"].Equals(DBNull.Value))
                            item.Approvato = reader.GetBoolean(reader.GetOrdinal("Approvato"));

                        //if (item.Approvato)
                        //{
                        //    napprovati += 1;
                        //    mediatotalestars += item.stelle;
                        //}
                        //list.Recordstotali = progressivo;
                        //list.Mediatotalestars = Math.Round(((double)mediatotalestars / napprovati), 1, MidpointRounding.ToEven);
                        //list.Napprovati = napprovati;

                        //if (page != 0 && pagesize != 0)
                        //{
                        //    if (progressivo <= (page - 1) * pagesize) continue;
                        //    if (progressivo > (page) * pagesize) continue;
                        //}
                        list.Add(item);

                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento :" + error.Message, error);
            }

            return list;
        }


        public void Cancella(string connection, long Id)
        {
            if (connection == null || connection == "") return;
            if (Id == 0) return;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@id", Id);//OleDbType.VarChar
            parColl.Add(p1);
            string query = "DELETE FROM TBL_comments WHERE ([ID]=@id)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, cancellazione newsletter :" + error.Message, error);
            }
            return;
        }

        public void AbilitaCommento(string connessione, long id, bool status)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            SQLiteParameter papp = new SQLiteParameter("@approvato", status);//OleDbType.VarChar
            parColl.Add(papp);
            string query = "";
            //Update
            query = "UPDATE [TBL_comments] SET approvato=@approvato ";
            query += " WHERE [Id] = " + id;
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornamento stato commento  :" + error.Message, error);
            }
            return;
        }


        /// <summary>
        /// Inserisce o aggiorna una email nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InserisciAggiorna(string connessione, Comment item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p2 = new SQLiteParameter("@idpost", item.Idpost);//OleDbType.VarChar
            parColl.Add(p2);
            SQLiteParameter p2b = new SQLiteParameter("@idcollegato", item.Idcollegato);//OleDbType.VarChar
            parColl.Add(p2b);
            SQLiteParameter p2c = new SQLiteParameter("@stelle", item.stelle);//OleDbType.VarChar
            parColl.Add(p2c);

            SQLiteParameter p3 = new SQLiteParameter("@nome", item.Nome);//OleDbType.VarChar
            parColl.Add(p3);
            SQLiteParameter p5 = new SQLiteParameter("@testoI", item.TestoI);//OleDbType.VarChar
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@titoloI", item.TitoloI);//OleDbType.VarChar
            parColl.Add(p6);

            SQLiteParameter p5b = new SQLiteParameter("@testoGB", item.TestoGB);//OleDbType.VarChar
            parColl.Add(p5b);
            SQLiteParameter p6b = new SQLiteParameter("@titoloGB", item.TitoloGB);//OleDbType.VarChar
            parColl.Add(p6b);

            SQLiteParameter p5c = new SQLiteParameter("@testoRU", item.TestoRU);//OleDbType.VarChar
            parColl.Add(p5c);
            SQLiteParameter p6c = new SQLiteParameter("@titoloRU", item.TitoloRU);//OleDbType.VarChar
            parColl.Add(p6c);

            SQLiteParameter p5d = new SQLiteParameter("@testoFR", item.TestoFR);//OleDbType.VarChar
            parColl.Add(p5d);
            SQLiteParameter p6d = new SQLiteParameter("@titoloFR", item.TitoloFR);//OleDbType.VarChar
            parColl.Add(p6d);

            SQLiteParameter p5e = new SQLiteParameter("@testoDE", item.TestoDE);//OleDbType.VarChar
            parColl.Add(p5e);
            SQLiteParameter p6e = new SQLiteParameter("@titoloDE", item.TitoloDE);//OleDbType.VarChar
            parColl.Add(p6e);


            SQLiteParameter p5f = new SQLiteParameter("@testoES", item.TestoES);//OleDbType.VarChar
            parColl.Add(p5f);
            SQLiteParameter p6f = new SQLiteParameter("@titoloES", item.TitoloES);//OleDbType.VarChar
            parColl.Add(p6f);

            SQLiteParameter p7 = new SQLiteParameter("@autore", item.Autore);//OleDbType.VarChar
            parColl.Add(p7);
            SQLiteParameter p9 = new SQLiteParameter("@Spare2", item.Spare2);//OleDbType.VarChar
            parColl.Add(p9);
            SQLiteParameter p10 = new SQLiteParameter("@email", item.Email);//OleDbType.VarChar
            parColl.Add(p10);

            SQLiteParameter p4 = null;
            if (item.Data != null && item.Data != DateTime.MinValue)
                p4 = new SQLiteParameter("@data", dbDataAccess.CorrectDatenow(item.Data));
            else
                p4 = new SQLiteParameter("@data", dbDataAccess.CorrectDatenow(System.DateTime.Now));
            p4.DbType = System.Data.DbType.DateTime;
            parColl.Add(p4);
            SQLiteParameter p11 = new SQLiteParameter("@spare1", item.Spare1);//OleDbType.VarChar
            parColl.Add(p11);
            SQLiteParameter papp = new SQLiteParameter("@approvato", item.Approvato);//OleDbType.VarChar
            parColl.Add(papp);

            string query = "";
            if (item.Id != 0)
            {
                //Update
                query = "UPDATE [TBL_comments] SET idpost=@idpost,idcollegato=@idcollegato,stelle=@stelle,nome=@nome,testoI=@testoI,titoloI=@titoloI,testoGB=@testoGB,titoloGB=@titoloGB,testoRU=@testoRU,titoloRU=@titoloRU,testoFR=@testoFR,titoloFR=@titoloFR,testoDE=@testoDE,titoloDE=@titoloDE,testoES=@testoES,titoloES=@titoloES";
                query += ",autore=@autore,Spare2=@Spare2,email=@email,data=@data,spare1=@spare1,approvato=@approvato ";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_comments (idpost,idcollegato,stelle,nome,testoI,titoloI,testoGB,titoloGB,testoRU,titoloRU,testoFR,titoloFR,testoDE,titoloDE,testoES,titoloES";
                query += ",autore,Spare2,email,data,spare1,approvato )";
                query += " values ( ";
                query += "@idpost,@idcollegato,@stelle,@nome,@testoI,@titoloI,@testoGB,@titoloGB,@testoRU,@titoloRU,@testoFR,@titoloFR,@testoDE,@titoloDE,@testoES,@titoloES,@autore";
                query += ",@Spare2,@email,@data,@spare1,@approvato )";
            }

            try
            {
                long lastidentity = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                if (item.Id == 0) item.Id = lastidentity; //Inserisco nell'id dell'elemento inseito l'id generato dal db   
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento  :" + error.Message, error);
            }
            return;
        }

    }
}
