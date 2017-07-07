using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
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
				List<OleDbParameter> parColl = new List<OleDbParameter>();

				OleDbParameter p2 = new OleDbParameter("@Codice", item.Codice);
				parColl.Add(p2);
				OleDbParameter p3 = new OleDbParameter("@Valore", item.Valore);
				parColl.Add(p3);
				OleDbParameter p4 = new OleDbParameter("@Gruppo", item.Gruppo);
				parColl.Add(p4);


				if (item.Id != 0)
				{
					//Aggiorno
					query = "UPDATE [TBL_CONFIG] SET Codice=@Codice" + ",Valore=@Valore" + ",Gruppo=@Gruppo" + "";
					query += " WHERE [Id] = " + item.Id + " ";
				}
				try
				{
					int retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
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
		public static void LoadConfig()
		{
			Items.Clear();

			string connection = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
			string query = "SELECT ID, Gruppo,Codice,Valore FROM TBL_Config";

			try
			{

				OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, null, connection);
				using (reader)
				{
					if (reader == null) { return; };
					if (reader.HasRows == false)
						return;

					while (reader.Read())
					{
						ConfigItem item = new ConfigItem();

						item.Id = reader.GetInt32(reader.GetOrdinal("ID"));

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

	}

	public class ConfigItem
	{
		public int Id { get; set; }
		public string Gruppo { get; set; }
		public string Codice { get; set; }
		public string Valore { get; set; }
	}

	public class ConfigSection : Dictionary<string, ConfigItem>
	{

	}
}


