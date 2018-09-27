using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using WelcomeLibrary.DAL;

namespace WelcomeLibrary.UF
{
    public static class ConfigManagement
    {

        public static Dictionary<string, ConfigSection> Items = new Dictionary<string, ConfigSection>();

        public static string ReadKey(string chiave)
        {
            string ret = "";
            chiave = chiave.ToLower();

            foreach (var item in Items)
            {
                foreach (var kv in item.Value)
                {
                    if (kv.Key == chiave)
                    {
                        ret = kv.Value.Valore;
                        return ret;
                    }
                }
            }

            return ret;

        }
        public static Dictionary<string, string> ReadSection(ref Exception errorret, string gruppo)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            gruppo = gruppo.ToLower();

            if (Items.ContainsKey(gruppo))
            {
                var cat = Items[gruppo];
                foreach (var kv in cat)
                {
                    if (!ret.ContainsKey(kv.Value.Codice))
                        ret.Add(kv.Value.Codice, kv.Value.Valore);
                }
            }
            else
            {
                errorret = new Exception("Sezione " + gruppo + " non trovata");
            }

            return ret;
        }

        public static string AggiornaConfigList(ref List<ConfigItem> list)
        {
            string query = "";
            string err = "";

            string connessione = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            foreach (ConfigItem item in list)
            {
                if (connessione == null || connessione == "") return err;
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();

                SQLiteParameter p2 = new SQLiteParameter("@Codice", item.Codice);
                parColl.Add(p2);
                SQLiteParameter p3 = new SQLiteParameter("@Valore", item.Valore);
                parColl.Add(p3);
                SQLiteParameter p4 = new SQLiteParameter("@Gruppo", item.Gruppo);
                parColl.Add(p4);
                SQLiteParameter p5 = new SQLiteParameter("@Comment", item.Comment);
                parColl.Add(p5);


                if (item.Id != 0)
                {
                    //Aggiorno
                    query = "UPDATE [TBL_Config] SET Codice=@Codice" + ",Valore=@Valore" + ",Gruppo=@Gruppo" + "";
                    query += " WHERE [Id] = " + item.Id + " ";
                }
                else
                {
                    Dictionary<string, ConfigSection> itemexisting = Verifyduplicate(item);
                    if (itemexisting.Count > 0)
                    {
                        //err += "Already present element config Gruppo,Codice \r\n";
                        //continue;
                        //Aggiorno solo il valore
                        query = "UPDATE [TBL_Config] SET " + "Valore=@Valore" + ",comment=@Comment";
                        query += " WHERE [Id] = " + itemexisting[item.Gruppo.ToLower()][item.Codice.ToLower()].Id + " ";
                    }
                    else
                    {
                        SQLiteParameter pcom = new SQLiteParameter("@Comment", (item.Comment != null ? item.Comment : ""));
                        parColl.Add(pcom);
                        query = "INSERT INTO [TBL_Config] (Codice,Valore,Gruppo,comment)";
                        query += " values ( ";
                        query += "@Codice,@Valore,@Gruppo,@Comment )";
                    }
                }


                try
                {
                    long retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                    LoadConfig();
                }
                catch (Exception error)
                {
                    err += "Errore, inserimento/aggiornamento config :" + error.Message;

                    //throw new ApplicationException("Errore, inserimento/aggiornamento config :" + error.Message, error);
                }
            }
            return err;
        }

        public static List<ConfigItem> ReadAsList(Dictionary<string, string> filtri)
        {
            List<ConfigItem> ret = new List<ConfigItem>();

            foreach (var item in Items)
            {
                foreach (var kv in item.Value)
                {
                    ret.Add(kv.Value);
                }
            }

            return ret;
        }


        /// <summary>
        /// Verifico la presenza di elementi con stesso Gruppo,Codice
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static Dictionary<string, ConfigSection> Verifyduplicate(ConfigItem item)
        {
            Dictionary<string, ConfigSection> ret = new Dictionary<string, ConfigSection>();
            Dictionary<string, ConfigSection> Items = LoadConfig(item.Gruppo, item.Codice);
            if (Items.Count > 0) ret = Items; //elemento gruppo/lingua/chiave esistente -> non posso inserire
            return ret;
        }

        public static void LoadConfig()
        {
            Items.Clear();

            string connection = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            string query = "SELECT ID, Gruppo,Codice,Valore FROM TBL_Config";

            try
            {

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return; };
                    if (reader.HasRows == false)
                        return;

                    while (reader.Read())
                    {
                        ConfigItem item = new ConfigItem();

                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));

                        item.Gruppo = reader["Gruppo"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Gruppo"));
                        item.Codice = reader["Codice"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Codice"));
                        item.Valore = reader["Valore"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Valore"));

                        // case non sensitive
                        item.Gruppo = item.Gruppo.ToLower();
                        item.Codice = item.Codice.ToLower();

                        if (Items.ContainsKey(item.Gruppo))
                        {
                            Items[item.Gruppo].Add(item.Codice, item);
                        }
                        else
                        {
                            ConfigSection csItem = new ConfigSection();
                            csItem.Add(item.Codice, item);
                            Items.Add(item.Gruppo, csItem);
                        }

                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Tabella Config :" + error.Message, error);
            }

        }

        public static Dictionary<string, ConfigSection> LoadConfig(string pgruppo, string pcodice)
        {
            Dictionary<string, ConfigSection> Items = new Dictionary<string, ConfigSection>(); ; // Gruppo -> Codice -> Valore

            string connection = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            string query = "SELECT ID, Gruppo,Codice,Valore FROM TBL_Config ";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (!string.IsNullOrEmpty(pgruppo))
            {
                SQLiteParameter pg = new SQLiteParameter("@Gruppo", pgruppo);//OleDbType.VarChar
                parColl.Add(pg);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE Gruppo like @Gruppo ";
                else
                    query += " AND Gruppo like @Gruppo  ";
            }
            if (!string.IsNullOrEmpty(pcodice))
            {
                SQLiteParameter pc = new SQLiteParameter("@Codice", pcodice);//OleDbType.VarChar
                parColl.Add(pc);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE Codice like @Codice ";
                else
                    query += " AND Codice like @Codice  ";
            }
            try
            {

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return Items; };
                    if (reader.HasRows == false)
                        return Items;

                    while (reader.Read())
                    {
                        ConfigItem item = new ConfigItem();

                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));

                        item.Gruppo = reader["Gruppo"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Gruppo"));
                        item.Codice = reader["Codice"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Codice"));
                        item.Valore = reader["Valore"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Valore"));

                        // case non sensitive
                        item.Gruppo = item.Gruppo.ToLower();
                        item.Codice = item.Codice.ToLower();

                        if (Items.ContainsKey(item.Gruppo))
                        {
                            Items[item.Gruppo].Add(item.Codice, item);
                        }
                        else
                        {
                            ConfigSection csItem = new ConfigSection();
                            csItem.Add(item.Codice, item);
                            Items.Add(item.Gruppo, csItem);
                        }

                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Tabella Config :" + error.Message, error);
            }
            return Items;

        }

    }

    public class ConfigItem
    {
        public long Id { get; set; }
        public string Gruppo { get; set; }
        public string Codice { get; set; }
        public string Valore { get; set; }
        public string Comment { get; set; }
    }

    public class ConfigSection : Dictionary<string, ConfigItem>
    {

    }
}


