using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WelcomeLibrary.DAL;

namespace WelcomeLibrary.UF
{
   public static class ResourceManagement 
   {
      //public static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, ResourceItem>>>> Items = 
      //   new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, ResourceItem>>>>();

      public static Dictionary<string, Categoria> Items = new Dictionary<string, Categoria>();
   
      public static ResourceItem ReadKey(string Gruppo, string Lingua, string Chiave, string categoria = "")
      {
         if (!string.IsNullOrEmpty(Chiave))
            Chiave = Chiave.ToLower();         

         ResourceItem ret = new ResourceItem() { Gruppo = "", Categoria = "", Lingua = "", Chiave = "", Valore = "", Comment = "" }; // per evitare cose spiacevoli ritorno un oggetto vuoto
         if (Items.ContainsKey(Gruppo))
         {
            if (Items[Gruppo].ContainsKey(categoria))
            {

               if (Items[Gruppo][categoria].ContainsKey(Lingua))
               {
                  if (Items[Gruppo][categoria][Lingua].ContainsKey(Chiave))
                  {
                     ret = Items[Gruppo][categoria][Lingua][Chiave];

                  }
               }

            }
         }

         return ret;
      }

      public static List<ResourceItem> ReadItemsByLingua(string _gruppo, string _lingua)
      {
         List<ResourceItem> ret = new List<ResourceItem>();
         if (Items.ContainsKey(_gruppo))
         {
            foreach (var categoria in Items[_gruppo])
            {
               if (Items[_gruppo][categoria.Key].ContainsKey(_lingua))
               {
                  var resources = Items[_gruppo][categoria.Key][_lingua];
                  foreach (var m in resources)
                  {
                     ret.Add(m.Value);
                     
                  }
               }
            }
         }

         return ret;
      }

      public static void LoadResources()
      {
         Items.Clear();

         string connection = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
         string query = "SELECT ID, Gruppo,Categoria,Lingua,Chiave,Valore,Comment FROM TBL_Risorse";

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
                  ResourceItem item = new ResourceItem();

                  item.id = reader.GetInt32(reader.GetOrdinal("ID"));
                  
                  item.Gruppo = reader["Gruppo"].Equals(DBNull.Value) ? "" :reader.GetString(reader.GetOrdinal("Gruppo"));
                  item.Categoria = reader["Categoria"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Categoria"));
                  item.Lingua = reader["Lingua"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Lingua"));
                  item.Chiave = reader["Chiave"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Chiave"));
                  item.Valore = reader["Valore"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Valore"));
                  item.Comment = reader["Comment"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Comment"));

                  // case non sensitive
                  item.Chiave = item.Chiave.ToLower();

                  //if (item.Lingua == "GB")
                  //{
                  //   int a = 0;
                  //}
                  if (!Items.ContainsKey(item.Gruppo))
                  {

                     // creazione item
                     Risorse risorse = new Risorse();
                     risorse.Add(item.Chiave, item);
                     // creazione Lingua
                     //Dictionary<string, Dictionary<string, ResourceItem>> Lingua = new Dictionary<string, Dictionary<string, ResourceItem>>();
                     Lingua lingua = new Lingua();
                     lingua.Add(item.Lingua, risorse);

                     Categoria categoria = new Categoria();
                     categoria.Add(item.Categoria, lingua);

                     Items.Add(item.Gruppo, categoria);
                  }
                  else
                  {
                     // se esiste un gruppo esiste almeno una cat
                     Categoria categoria = Items[item.Gruppo];
                     if (!categoria.ContainsKey(item.Categoria))
                     {
                        // creare tutto
                        // creazione item
                        Risorse risorse = new Risorse();
                        risorse.Add(item.Chiave, item);
                        // creazione Lingua
                        //Dictionary<string, Dictionary<string, ResourceItem>> Lingua = new Dictionary<string, Dictionary<string, ResourceItem>>();
                        Lingua lingua = new Lingua();
                        lingua.Add(item.Lingua, risorse);

                        categoria.Add(item.Categoria, lingua);

                     }
                     else
                     {

                        Lingua lingua = categoria[item.Categoria];


                        if (!lingua.ContainsKey(item.Lingua))
                        {
                           Risorse risorse = new Risorse();
                           risorse.Add(item.Chiave, item);

                           lingua.Add(item.Lingua, risorse);
                        }
                        else
                        {
                           Risorse risorse = lingua[item.Lingua];
                           if (!risorse.ContainsKey(item.Chiave))
                              risorse.Add(item.Chiave,item);
                           else
                              risorse[item.Chiave] = item;
                           //lingua[item.Lingua].Add(item.Chiave, item);
                        }

                     }



                  }
               
               }
            }

         }
         catch (Exception error)
         {
            throw new ApplicationException("Errore Caricamento Tabella Risorse :" + error.Message, error);
         }

      }

   }

   public class ResourceItem
   {
      public int id { get; set; }
      public string Gruppo { get; set; }
      public string Categoria { get; set; }
      public string Lingua { get; set; }
      public string Chiave { get; set; }
      public string Valore { get; set; }
      public string Comment { get; set; }
   }

   public class Risorse : Dictionary<string, ResourceItem>
   {

   }

   public class Lingua : Dictionary<string,Risorse>
   {

   }
   public class Categoria : Dictionary<string, Lingua>
   {

   }
   public class Gruppo : Dictionary<string, Categoria>
   {

   }
}
