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
      public static Dictionary<string,string> ReadSection(ref Exception errorret, string categoria)
      {
         Dictionary<string, string> ret = new Dictionary<string, string();

         if (Items.ContainsKey(categoria))
         {
            var cat = Items[categoria];
            foreach (var kv in cat)
            {
               if (!ret.ContainsKey(kv.Value.Chiave))
               ret.Add(kv.Value.Chiave, kv.Value.Valore);
            }
         }
         else
         {
            errorret = new Exception("Sezione " + categoria + " non trovata");
         }

         return ret;
      }

      public static void LoadConfig()
      {
         Items.Clear();

         string connection = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
         string query = "SELECT ID, Categoria,Chiave,Valore FROM TBL_Config";

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

                  item.id = reader.GetInt32(reader.GetOrdinal("ID"));

                  item.Categoria = reader["Categoria"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Categoria"));
                  item.Chiave = reader["Chiave"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Chiave"));
                  item.Valore = reader["Valore"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Valore"));

                  // case non sensitive
                  item.Chiave = item.Chiave.ToLower();

                  if (Items.ContainsKey(item.Categoria))
                  {
                     Items[item.Categoria].Add(item.Chiave, item);
                  }
                  else
                  {
                     ConfigSection csItem = new ConfigSection();
                     csItem.Add(item.Chiave, item);
                     Items.Add(item.Categoria, csItem);
                  

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
      public int id { get; set; }
      public string Categoria { get; set; }
      public string Chiave { get; set; }
      public string Valore { get; set; }
   }

   public class ConfigSection : Dictionary<string, ConfigItem>
   {

   }
}


