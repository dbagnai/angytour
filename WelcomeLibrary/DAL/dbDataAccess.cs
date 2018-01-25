using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.IO;
using System.Data.SQLite;

namespace WelcomeLibrary.DAL
{
    public static class dbDataAccess
    {

        public static DateTime CorrectDatenow(DateTime dataorig)
        {
            DateTime _tmpdate = System.DateTime.Now;
            if (!DateTime.TryParse(dataorig.ToString(), out _tmpdate))
                DateTime.TryParse(System.DateTime.Now.ToString(), out _tmpdate);
            return _tmpdate;
        }


        /// <summary>
        /// Apre un reader tramite una connessione oledb (per sqllite)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static SQLiteDataReader GetReaderOle(string query, SQLiteParameter[] parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(connessione);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);

            // controllo che siano passati parametri
            if (parms != null && parms.Length > 0)
                for (int i = 0; i < parms.Length; i++)
                    cmd.Parameters.Add(parms[i]);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>OleDbConnect
        /// Apre un reader usando una connessione oledb(access)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static SQLiteDataReader GetReaderListOle(string query, List<SQLiteParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(connessione);
            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(query, conn);

            // controllo che siano passati parametri
            if (parms != null && parms.Count > 0)
                for (int i = 0; i < parms.Count; i++)
                    cmd.Parameters.Add(parms[i]);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }


        public static DataTable GetDataTableOle(string query, List<SQLiteParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(connessione);
            conn.Open();
            DataTable dt = new DataTable();

            using (conn)
            {
                //// controllo che siano passati parametri
                //if (parms != null && parms.Count > 0)
                //    for (int i = 0; i < parms.Count; i++)
                //        cmd.Parameters.Add(parms[i]);

                SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;

        }

        public static string comprimiSQLiteDB(string Conn)
        {
            string ret = "";

            try
            {
                ExecuteScalar<long>("VACUUM;", null, Conn);
                ret = "Compresso db correttamente";
            }
            catch { ret = "Errore compressione archivio DB"; }

            return ret;
        }

        public static long ExecuteStoredProcListOle(string query, List<SQLiteParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            long ritorno = 0;
            //SqlConnection conn = new SqlConnection(ConnectionString);
            using (SQLiteConnection conn = new SQLiteConnection(connessione))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                if ((((string)query.ToUpper()).StartsWith("SELECT ") == true))
                {
                    //Per fare questo devi usare il reader
                    return 0;
                }

                if ((((string)query.ToUpper()).StartsWith("INSERT ") != true)
                && (((string)query.ToUpper()).StartsWith("UPDATE ") != true)
                && (((string)query.ToUpper()).StartsWith("DELETE ") != true))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();

                    // controllo che siano passati parametri
                    if (parms != null && parms.Count > 0)
                        for (int i = 0; i < parms.Count; i++)
                            cmd.Parameters.Add(parms[i]);


                    //CommandBehavior.CloseConnection
                    ritorno = cmd.ExecuteNonQuery();


                    conn.Close();

                }
                else
                {
                    cmd.Parameters.Clear();

                    // controllo che siano passati parametri
                    if (parms != null && parms.Count > 0)
                        for (int i = 0; i < parms.Count; i++)
                            cmd.Parameters.Add(parms[i]);

                    //CommandBehavior.CloseConnection
                    ritorno = cmd.ExecuteNonQuery();

                    if ((((string)query.ToUpper()).StartsWith("INSERT ") == true)) //nella query di insert ritorno l'identity appena generata
                    {
                        //cmd.CommandText = "Select @@Identity";
                        cmd.CommandText = "SELECT last_insert_rowid()";
                        //object o = cmd.ExecuteScalar();
                        ritorno = (long)cmd.ExecuteScalar();
                    }

                    conn.Close();
                }
            }
            return ritorno;
        }


        /// <summary>
        /// Creiamo anche un metodo per eseguire query di aggiornamento tramite connessione sqllite
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static int ExecuteStoredProcOle(string query, SQLiteParameter[] parms, string Conn)
        {
            int ritorno = 0;
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(connessione);
            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            if ((((string)query.ToUpper()).StartsWith("SELECT ") == true))
            {
                //Per fare questo devi usare il reader
                return 0;
            }

            if ((((string)query.ToUpper()).StartsWith("INSERT ") != true)
            && (((string)query.ToUpper()).StartsWith("UPDATE ") != true)
            && (((string)query.ToUpper()).StartsWith("DELETE ") != true))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // controllo che siano passati parametri
                if (parms != null && parms.Length > 0)
                    for (int i = 0; i < parms.Length; i++)
                        cmd.Parameters.Add(parms[i]);

                //CommandBehavior.CloseConnection
                ritorno = cmd.ExecuteNonQuery();
                conn.Close();

            }
            else
            {
                // controllo che siano passati parametri
                if (parms != null && parms.Length > 0)
                    for (int i = 0; i < parms.Length; i++)
                        cmd.Parameters.Add(parms[i]);

                //CommandBehavior.CloseConnection
                ritorno = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return ritorno;
        }




        public static T ExecuteScalar<T>(string Query, List<SQLiteParameter> parameters, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            //UTILIZZO GENERICO DEL METODO
            //int totalRecords = ExecuteScalar<int>("SELECT COUNT(*) FROM authors", null);
            //using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SQLiteConnection conn = new SQLiteConnection(connessione))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand(Query, conn))
                {
                    command.Parameters.Clear();
                    // controllo che siano passati parametri
                    if (parameters != null && parameters.Count > 0)
                        for (int i = 0; i < parameters.Count; i++)
                            command.Parameters.Add(parameters[i]);
                    T ritorno = (T)command.ExecuteScalar();
                    conn.Close();
                    return ritorno;
                }
            }
        }


        /// <summary>
        /// Creiamo anche un metodo per eseguire query di selezione come stored procedure
        /// usando una connessione oledb(access)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="dt"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static int ExecuteSelectStoredProcOle(string query, SQLiteParameter[] parms, DataTable dt, string Conn)
        {
            int ritorno = 0;
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(connessione);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            if ((((string)query.ToUpper()).StartsWith("SELECT ") == true))
            {
                //Per fare questo devi usare il reader
                return 0;
            }

            if ((((string)query.ToUpper()).StartsWith("INSERT ") != true)
            && (((string)query.ToUpper()).StartsWith("UPDATE ") != true)
            && (((string)query.ToUpper()).StartsWith("DELETE ") != true))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // controllo che siano passati parametri
                if (parms != null && parms.Length > 0)
                    for (int i = 0; i < parms.Length; i++)
                        cmd.Parameters.Add(parms[i]);
                SQLiteDataAdapter da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;
                ritorno = da.Fill(dt);
                conn.Close();
            }

            return ritorno;
        }


        //QUESTO DATAMANAGER SELEZIONA IL DB IN BASE
        //ALL'AZIENDA IMPOSTATA NELL'APPOSITA PROPRIETA' DBAZIENDA
        //USA LA SINTASSI sqlserver + dbAzienda per la stringa di connessione
        //private static string _dbAzienda;
        //public static string DbAzienda
        //{
        //    get { return dbDataAccess._dbAzienda; }
        //    set { dbDataAccess._dbAzienda = value; }
        //}

        //internal static string ConnectionString
        //{
        //    get { return System.Configuration.ConfigurationManager.ConnectionStrings[_dbAzienda].ConnectionString; }
        //}


        /// <summary>
        /// Esporta i risultati della query in un file XML
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="nomefile"></param>
        /// <returns></returns>
        public static bool ReadXmlToFile(string query, List<DbParameter> parms, string nomefile, string nometabella, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SqlConnection conn = new SqlConnection(connessione);

            SqlDataAdapter DScmdXML;
            DataSet DSXML = new DataSet();
            //SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            // controllo che siano passati parametri
            if (parms != null && parms.Count > 0)
                for (int i = 0; i < parms.Count; i++)
                    cmd.Parameters.Add(parms[i]);

            DScmdXML = new SqlDataAdapter(cmd);
            DScmdXML.Fill(DSXML, nometabella);

            //SCRIVO IL DATASET COME FILE XML
            DSXML.WriteXml(nomefile, XmlWriteMode.WriteSchema);
            System.IO.StreamWriter xmlSW2 = new System.IO.StreamWriter(nomefile);
            DSXML.WriteXml(xmlSW2, XmlWriteMode.WriteSchema);
            xmlSW2.Flush();
            xmlSW2.Close();

            conn.Close();
            return true;
        }

        /// <summary>
        ///  Test lettura tabella con sqlreader _> problemi risultati incompleti
        /// usando la query che ritorna formato xml
        /// con la clausola select * FROM [TABLE] FOR XML  PATH('Immobile'),ROOT('ListaImmobili') ,ELEMENTS
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteSqlReader(string query, List<DbParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SqlConnection conn = new SqlConnection(connessione);

            //SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            // controllo che siano passati parametri
            if (parms != null && parms.Count > 0)
                for (int i = 0; i < parms.Count; i++)
                    cmd.Parameters.Add(parms[i]);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }



        /// <summary>
        /// Apre un reader tramite una connessione a sqlserver
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static DbDataReader GetReader(string query, DbParameter[] parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SqlConnection conn = new SqlConnection(connessione);

            //SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Clear();
            // controllo che siano passati parametri
            if (parms != null && parms.Length > 0)
                for (int i = 0; i < parms.Length; i++)
                    cmd.Parameters.Add(parms[i]);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

     
        /// <summary>
        /// Creiamo anche un metodo per eseguire query di aggiornamento usando una connessione sql
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static int ExecuteStoredProc(string query, DbParameter[] parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SqlConnection conn = new SqlConnection(connessione);
            int ritorno = 0;
            //SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            if ((((string)query.ToUpper()).StartsWith("SELECT ") == true))
            {
                //Per fare questo devi usare il reader
                return 0;
            }
            if ((((string)query.ToUpper()).StartsWith("INSERT ") != true)
            && (((string)query.ToUpper()).StartsWith("UPDATE ") != true)
            && (((string)query.ToUpper()).StartsWith("DELETE ") != true))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                // controllo che siano passati parametri
                if (parms != null && parms.Length > 0)
                    for (int i = 0; i < parms.Length; i++)
                        cmd.Parameters.Add(parms[i]);

                //CommandBehavior.CloseConnection
                ritorno = cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                cmd.Parameters.Clear();

                // controllo che siano passati parametri
                if (parms != null && parms.Length > 0)
                    for (int i = 0; i < parms.Length; i++)
                        cmd.Parameters.Add(parms[i]);

                //CommandBehavior.CloseConnection
                ritorno = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return ritorno;
        }




        /// <summary>
        /// Creiamo anche un metodo per eseguire query di aggiornamento con collection di parametri
        /// tramite una connessione sqlserver
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static int ExecuteStoredProcList(string query, List<DbParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SqlConnection conn = new SqlConnection(connessione);

            int ritorno = 0;
            //SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            if ((((string)query.ToUpper()).StartsWith("SELECT ") == true))
            {
                //Per fare questo devi usare il reader
                return 0;
            }

            if ((((string)query.ToUpper()).StartsWith("INSERT ") != true)
            && (((string)query.ToUpper()).StartsWith("UPDATE ") != true)
            && (((string)query.ToUpper()).StartsWith("DELETE ") != true))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                // controllo che siano passati parametri
                if (parms != null && parms.Count > 0)
                    for (int i = 0; i < parms.Count; i++)
                        cmd.Parameters.Add(parms[i]);

                //CommandBehavior.CloseConnection
                ritorno = cmd.ExecuteNonQuery();
                conn.Close();

            }
            else
            {
                cmd.Parameters.Clear();

                // controllo che siano passati parametri
                if (parms != null && parms.Count > 0)
                    for (int i = 0; i < parms.Count; i++)
                        cmd.Parameters.Add(parms[i]);

                //CommandBehavior.CloseConnection
                ritorno = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return ritorno;
        }
      

    }
}
