using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
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

			Gruppo = Gruppo.ToLower();
			Lingua = Lingua.ToLower();
			categoria = categoria.ToLower();

			DateTime st = DateTime.Now;

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

			//System.Diagnostics.Debug.Print("ReadKey : gruppo:" + Gruppo + " categoria:" + categoria + " Lingua:" + Lingua + " chiave:" + Chiave + " valore:" + ret.Valore + " " + (DateTime.Now - st).TotalMilliseconds.ToString());

			return ret;
		}

		public static List<ResourceItem> ReadItemsByLingua(string _lingua)
		{

			_lingua = _lingua.ToLower();
			List<ResourceItem> ret = new List<ResourceItem>();
			
			DateTime st = DateTime.Now;
			foreach (var _gruppoItem in Items)
			{
				var _gruppo = _gruppoItem.Key.ToLower();
				foreach (var categoria in Items[_gruppo])
				{
					if (Items[_gruppo][categoria.Key.ToLower()].ContainsKey(_lingua))
					{
						var resources = Items[_gruppo][categoria.Key.ToLower()][_lingua];
						foreach (var m in resources)
						{
							ret.Add(m.Value);

						}
					}
				}
			}

			//System.Diagnostics.Debug.Print("Lingua:" + _lingua + " valore:" + ret.Count + " " + (DateTime.Now - st).TotalMilliseconds.ToString());
			return ret;
		}
		public static List<ResourceItem> ReadItemsByLingua(string _gruppo, string _lingua)
		{
			_gruppo = _gruppo.ToLower();
			_lingua = _lingua.ToLower();
			List<ResourceItem> ret = new List<ResourceItem>();
			DateTime st = DateTime.Now;
			if (Items.ContainsKey(_gruppo))
			{
				foreach (var categoria in Items[_gruppo])
				{
					if (Items[_gruppo][categoria.Key.ToLower()].ContainsKey(_lingua))
					{
						var resources = Items[_gruppo][categoria.Key.ToLower()][_lingua];
						foreach (var m in resources)
						{
							ret.Add(m.Value);

						}
					}
				}
			}

			//System.Diagnostics.Debug.Print("gruppo:" + _gruppo + " Lingua:" + _lingua + " valore:" + ret.Count + " " + (DateTime.Now - st).TotalMilliseconds.ToString());
			return ret;
		}

		public static void LoadResources()
		{
			Items.Clear();
			DateTime st = DateTime.Now;

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

						item.Id = reader.GetInt32(reader.GetOrdinal("ID"));

						item.Gruppo = reader["Gruppo"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Gruppo"));
						item.Categoria = reader["Categoria"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Categoria"));
						item.Lingua = reader["Lingua"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Lingua"));
						item.Chiave = reader["Chiave"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Chiave"));
						item.Valore = reader["Valore"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Valore"));
						item.Comment = reader["Comment"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Comment"));

						// case non sensitive
						item.Gruppo = item.Gruppo.ToLower();
						item.Categoria = item.Categoria.ToLower();
						item.Lingua = item.Lingua.ToLower();
						item.Chiave = item.Chiave.ToLower();

						item.Valore = (item.Valore);
						//item.Valore = HtmlEncode(item.Valore);

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
										risorse.Add(item.Chiave, item);
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

			//System.Diagnostics.Debug.Print("Lettura risorse:" + Items.Count.ToString() + " " + (DateTime.Now - st).TotalMilliseconds.ToString());
		}

        public static Dictionary<string, Categoria> LoadResources(string pgruppo = "", string pcategoria = "", string plingua = "", string pchiave = "")
        {
            Dictionary<string, Categoria> Items = new Dictionary<string, Categoria>(); ; // Gruppo -> Categoria -> Lingua -> Chiave -> Valore
            DateTime st = DateTime.Now;

            string connection = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            string query = "SELECT ID, Gruppo,Categoria,Lingua,Chiave,Valore,Comment FROM TBL_Risorse";
            List<OleDbParameter> parColl = new List<OleDbParameter>();


            if (!string.IsNullOrEmpty(pgruppo))
            {
                OleDbParameter pg = new OleDbParameter("@Gruppo", pgruppo);//OleDbType.VarChar
                parColl.Add(pg);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE Gruppo like @Gruppo ";
                else
                    query += " AND Gruppo like @Gruppo  ";
            }
            if (!string.IsNullOrEmpty(pcategoria))
            {
                OleDbParameter pc = new OleDbParameter("@Categoria", pcategoria);//OleDbType.VarChar
                parColl.Add(pc);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE Categoria like @Categoria ";
                else
                    query += " AND Categoria like @Categoria  ";
            }
            if (!string.IsNullOrEmpty(plingua))
            {
                OleDbParameter pl = new OleDbParameter("@Lingua", plingua);//OleDbType.VarChar
                parColl.Add(pl);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE Lingua like @Lingua ";
                else
                    query += " AND Lingua like @Lingua  ";
            }
            if (!string.IsNullOrEmpty(pchiave))
            {
                OleDbParameter pch = new OleDbParameter("@Chiave", pchiave);//OleDbType.VarChar
                parColl.Add(pch);
                if (!query.ToLower().Contains("where"))
                    query += " WHERE Chiave like @Chiave ";
                else
                    query += " AND Chiave like @Chiave  ";
            }

            try
            {

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return Items; };
                    if (reader.HasRows == false)
                        return Items;

                    while (reader.Read())
                    {
                        ResourceItem item = new ResourceItem();

                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));

                        item.Gruppo = reader["Gruppo"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Gruppo"));
                        item.Categoria = reader["Categoria"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Categoria"));
                        item.Lingua = reader["Lingua"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Lingua"));
                        item.Chiave = reader["Chiave"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Chiave"));
                        item.Valore = reader["Valore"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Valore"));
                        item.Comment = reader["Comment"].Equals(DBNull.Value) ? "" : reader.GetString(reader.GetOrdinal("Comment"));

                        // case non sensitive
                        item.Gruppo = item.Gruppo.ToLower();
                        item.Categoria = item.Categoria.ToLower();
                        item.Lingua = item.Lingua.ToLower();
                        item.Chiave = item.Chiave.ToLower();

                        item.Valore = (item.Valore);
                        //item.Valore = HtmlEncode(item.Valore);

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
                            Lingua lingua = new Lingua();
                            lingua.Add(item.Lingua, risorse);
                            Categoria categoria = new Categoria();
                            categoria.Add(item.Categoria, lingua);
                            Items.Add(item.Gruppo, categoria);
                        }
                        else //Gruppo presente
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
                            else //categoria presente
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
                                        risorse.Add(item.Chiave, item);
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
                // throw new ApplicationException("Errore Caricamento Tabella Risorse :" + error.Message, error);
            }
            return Items;
            //System.Diagnostics.Debug.Print("Lettura risorse:" + Items.Count.ToString() + " " + (DateTime.Now - st).TotalMilliseconds.ToString());
        }


        public static string AggiornaResourceList(ref List<ResourceItem> list)
        {
            string query = "";
            string err = "";

            string connessione = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            foreach (ResourceItem item in list)
            {
                if (connessione == null || connessione == "") return err;
                List<OleDbParameter> parColl = new List<OleDbParameter>();

                OleDbParameter p2 = new OleDbParameter("@Chiave", item.Chiave);
                parColl.Add(p2);
                OleDbParameter p3 = new OleDbParameter("@Valore", item.Valore);
                parColl.Add(p3);
                OleDbParameter p4 = new OleDbParameter("@Gruppo", item.Gruppo);
                parColl.Add(p4);


                if (item.Id != 0)
                {
                    //Aggiorno
                    query = "UPDATE [TBL_Risorse] SET Chiave=@Chiave" + ",Valore=@Valore" + ",Gruppo=@Gruppo" + "";
                    query += " WHERE [Id] = " + item.Id + " ";
                }
                else
                {
                    //Verifichiamo se già esiste un elemento Chiave/Gruppo/Lingua con gli stessi valori per insert
                    if (Verifyduplicate(item))
                    {
                        err += "Already present element Gruppo,Lingua,Chiave \r\n";
                        continue;
                    }

                    OleDbParameter pcat = new OleDbParameter("@Categoria", (item.Categoria != null ? item.Categoria : ""));
                    parColl.Add(pcat);
                    OleDbParameter pcom = new OleDbParameter("@Comment", (item.Comment != null ? item.Comment : ""));
                    parColl.Add(pcom);
                    OleDbParameter plin = new OleDbParameter("@Lingua", item.Lingua);
                    parColl.Add(plin);

                    query = "INSERT INTO TBL_Risorse (Chiave,Valore,Gruppo,Categoria";
                    query += ",Comment,Lingua )";
                    query += " values ( ";
                    query += "@Chiave,@Valore,@Gruppo,@Categoria";
                    query += ",@Comment,@Lingua )";
                }
                try
                {
                    int retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                    LoadResources();
                }
                catch (Exception error)
                {
                    err += "Errore, inserimento/aggiornamento risorse :" + error.Message;

                    //throw new ApplicationException("Errore, inserimento/aggiornamento config :" + error.Message, error);
                }
            }
            return err;
        }

        /// <summary>
        /// Verifico la presenza di elementi con stesso Gruppo,Lingua,Chiave
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static bool Verifyduplicate(ResourceItem item)
        {
            bool ret = false;
            Dictionary<string, Categoria> Items = LoadResources(item.Gruppo, "", item.Lingua, item.Chiave);
            if (Items.Count > 0) ret = true; //elemento gruppo/lingua/chiave esistente -> non posso inserire
            return ret;
        }

    }



	public class ResourceItem
	{
		public int Id { get; set; }
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

	public class Lingua : Dictionary<string, Risorse>
	{

	}
	public class Categoria : Dictionary<string, Lingua>
	{

	}
	public class Gruppo : Dictionary<string, Categoria>
	{

	}
}
