using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Xml;

namespace WelcomeLibrary.DAL
{
    public static class dbDataAccess
    {
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
        public static bool WriteTableToFileXml(string query, List<DbParameter> parms, string nomefile, string nometabella, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SqlConnection conn = new SqlConnection(connessione);

            SqlDataAdapter DScmdXML;
            DataSet DSXML = new DataSet();
            //SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.Text;
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
        /// Carica un Dataset in base alla query e argomenti passati da una tabella
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="nomefile"></param>
        /// <returns></returns>
        public static DataSet ReadDataset(string query, List<DbParameter> parms, string nometabella, String Conn)
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

            conn.Close();
            return DSXML;
        }

        /// <summary>
        /// Scrive un Dataset in un file XML
        /// </summary>
        /// <param name="DSXML"></param>
        /// <param name="nomefile"></param>
        /// <returns></returns>
        public static bool WriteDatasetToXml(string nomefile, DataSet DSXML)
        {
            //SCRIVO IL DATASET COME FILE XML
            DSXML.WriteXml(nomefile, XmlWriteMode.WriteSchema);
            System.IO.StreamWriter xmlSW2 = new System.IO.StreamWriter(nomefile);
            DSXML.WriteXml(xmlSW2, XmlWriteMode.WriteSchema);
            xmlSW2.Flush();
            xmlSW2.Close();
            return true;
        }

        /// <summary>
        /// Scrive in un File XML le righe dalla posizione iniziale (StartRow) 
        /// alla Posizione finale (StartRow + Lenght - 1)
        /// prese dalla tabella in posizione (NumTabella) contenuta nel dataset DSXML passato nei parametri
        /// </summary>
        /// <param name="DSXML"> Dataset Origine dei dati</param>
        /// <param name="nomefile">Nome del file completo di percorso fisico che verrà scritto</param>
        /// <param name="NumTabella">Posizione della tabella nel dataset di origine</param>
        /// <param name="StartRow">Riga iniziale </param>
        /// <param name="Lenght">Riga finale</param>
        /// <returns></returns>
        public static bool WriteDatasetToXml(string nomefile, DataSet DSXML, string NomeTabella, int StartRow, int Lenght)
        {
            //Copio la riga che mi interessa da un dataset a uno nuovo
            DataSet tmp = new DataSet();
            DataTable TblCopia = DSXML.Tables[NomeTabella].Clone();
            TblCopia.TableName = NomeTabella;
            tmp.Tables.Add(TblCopia);
            //Aggiungiamo le righe di interesse
            for (int i = 0; i <= Lenght - 1; i++)
            {
                tmp.Tables[NomeTabella].ImportRow(DSXML.Tables[NomeTabella].Rows[StartRow + i]);
            }

            //SCRIVO IL DATASET COME FILE XML
            tmp.WriteXml(nomefile, XmlWriteMode.WriteSchema);
            System.IO.StreamWriter xmlSW2 = new System.IO.StreamWriter(nomefile);
            tmp.WriteXml(xmlSW2, XmlWriteMode.WriteSchema);
            xmlSW2.Flush();
            xmlSW2.Close();
            return true;
        }

        /// <summary>
        /// Ricrea un datset a partire dal file Xml indicato
        /// </summary>
        /// <param name="nomefile"></param>
        /// <returns></returns>
        public static DataSet ReadXmlFileToDataset(string nomefile)
        {
            DataSet DSXML = new DataSet();
            DSXML.ReadXml(nomefile, XmlReadMode.ReadSchema);
            return DSXML;
        }

        /// <summary>
        /// Test lettura tabella con xmlreader _> problemi risultati incompleti
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(string query, List<DbParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SqlConnection conn = new SqlConnection(connessione);

            //SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            //SqlXml
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            // controllo che siano passati parametri
            if (parms != null && parms.Count > 0)
                for (int i = 0; i < parms.Count; i++)
                    cmd.Parameters.Add(parms[i]);


            //cmd.RootTag = "ListaImmobili";
            //cmd1.BasePath = Server.MapPath("/BankSite/");
            //cmd1.CommandText = xmlQuery;
            //cmd1.XslPath = "xsl/MySavings.xsl";


            XmlReader ritorno;

            ritorno = cmd.ExecuteXmlReader();
            conn.Close();
            return ritorno;
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



        public static T ExecuteScalar<T>(string Query, List<DbParameter> parameters, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;


            //UTILIZZO GENERICO DEL METODO
            //int totalRecords = ExecuteScalar<int>("SELECT COUNT(*) FROM authors", null);

            //using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlConnection conn = new SqlConnection(connessione))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(Query, conn))
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
        /// Apre un reader tramite una connessione oledb (per access)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static OleDbDataReader GetReaderOle(string query, OleDbParameter[] parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connessione);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);

            // controllo che siano passati parametri
            if (parms != null && parms.Length > 0)
                for (int i = 0; i < parms.Length; i++)
                    cmd.Parameters.Add(parms[i]);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }


        /// <summary>
        /// Apre un reader traminte una connessione a sqlserver
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static DbDataReader GetReaderList(string query, List<DbParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            SqlConnection conn = new SqlConnection(connessione);

            //SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Clear();

            cmd.CommandType = CommandType.StoredProcedure;

            // controllo che siano passati parametri
            if (parms != null && parms.Count > 0)
                for (int i = 0; i < parms.Count; i++)
                    cmd.Parameters.Add(parms[i]);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Apre un reader usando una connessione oledb(access)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static OleDbDataReader GetReaderListOle(string query, List<OleDbParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connessione);
            conn.Open();

            OleDbCommand cmd = new OleDbCommand(query, conn);

            // controllo che siano passati parametri
            if (parms != null && parms.Count > 0)
                for (int i = 0; i < parms.Count; i++)
                    cmd.Parameters.Add(parms[i]);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }


        public static DataTable GetDataTableOle(string query, List<OleDbParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connessione);
            conn.Open();
            DataTable dt = new DataTable();

            using (conn)
            {
                //// controllo che siano passati parametri
                //if (parms != null && parms.Count > 0)
                //    for (int i = 0; i < parms.Count; i++)
                //        cmd.Parameters.Add(parms[i]);

                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;

        }

        public static void ComprimiDbAccess(string Conn)
        {
            try
            {
                string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
                connessione += ";Jet OLEDB:Engine Type=5";
                connessione = connessione.Replace("|DataDirectory|", AppDomain.CurrentDomain.BaseDirectory + "\\App_Data");
                JRO.JetEngine jro = new JRO.JetEngine();
               
                //Comprime il database ma lo copia in uno nuovo!!
                jro.CompactDatabase(connessione, connessione + ";Jet OLEDB:Engine Type=5");

                //Aggiungi riferimento  fare clic sulla scheda COM e selezionare Microsoft Jet and Replication Objects 2.1 Library. 
                //Dim jro As JRO.JetEngine
                //jro = New JRO.JetEngine()
                //jro.CompactDatabase("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\nwind.mdb", _
                //"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\NewNwind.mdb;Jet OLEDB:Engine Type=5")
            }
            catch { }

        }

        public static int ExecuteStoredProcListOle(string query, List<OleDbParameter> parms, string Conn)
        {
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            int ritorno = 0;
            //SqlConnection conn = new SqlConnection(ConnectionString);
            using (OleDbConnection conn = new OleDbConnection(connessione))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
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
                        cmd.CommandText = "Select @@Identity";
                        ritorno = (int)cmd.ExecuteScalar();
                    }

                    conn.Close();
                }
            }
            return ritorno;
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
        /// Creiamo anche un metodo per eseguire query di aggiornamento tramite connessione oledb(access)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static int ExecuteStoredProcOle(string query, OleDbParameter[] parms, string Conn)
        {
            int ritorno = 0;
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connessione);
            conn.Open();

            OleDbCommand cmd = new OleDbCommand(query, conn);
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
        /// <summary>
        /// Creiamo anche un metodo per eseguire query di selezione come stored procedure
        /// usando una connessione oledb(access)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <param name="dt"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static int ExecuteSelectStoredProcOle(string query, OleDbParameter[] parms, DataTable dt, string Conn)
        {
            int ritorno = 0;
            string connessione = System.Configuration.ConfigurationManager.ConnectionStrings[Conn].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connessione);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
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
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = cmd;
                ritorno = da.Fill(dt);
                conn.Close();
            }

            return ritorno;
        }

    }
}
