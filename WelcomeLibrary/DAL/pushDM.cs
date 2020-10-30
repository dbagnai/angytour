using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPush; //https://github.com/web-push-libs/web-push-csharp/
using WelcomeLibrary;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace WelcomeLibrary.DAL
{
    //Non è stato possibile caricare il file o l'assembly 'System.Net.Http, Version=4.1.1.2, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' o una delle relative dipendenze. Impossibile trovare il file specificato.
    //Si rispolve cambianod in app.config e web.config  la riga presente con   <bindingRedirect oldVersion="0.0.0.0-4.2.0.0" newVersion="4.0.0.0" />

    public class pushDM
    {
        //Lista di device per l'invio delle notifiche push
        public static DevicesCollection DevicesList = new DevicesCollection();

        public static void GenerateKeys()
        {
            var keys = VapidHelper.GenerateVapidKeys();
            List<ConfigItem> list = new List<ConfigItem>();
            ConfigItem PublicKey = new ConfigItem();
            PublicKey.Codice = "PublicKey";
            PublicKey.Comment = "Chiave generata";
            PublicKey.Gruppo = "pushnotifications";
            PublicKey.Valore = keys.PublicKey;
            list.Add(PublicKey);
            ConfigItem PrivateKey = new ConfigItem();
            PrivateKey.Codice = "PrivateKey";
            PrivateKey.Comment = "Chiave generata";
            PrivateKey.Gruppo = "pushnotifications";
            PrivateKey.Valore = keys.PrivateKey;
            list.Add(PrivateKey);
            ConfigManagement.AggiornaConfigList(ref list);
            return;
        }



        public static string SendNotification(string payload, string id)
        {
            string errore = "";

            string vapidPublicKey = ConfigManagement.ReadKey("PublicKey");
            string vapidPrivateKey = ConfigManagement.ReadKey("PrivateKey");

            Devices devicebyid = pushDM.DevicesList.Find(d => d.Id.ToString() == id);
            if (devicebyid != null)
            {
                //invio al device specifico
                try
                {
                    var pushSubscription = new PushSubscription(devicebyid.PushEndpoint, devicebyid.PushP256DH, devicebyid.PushAuth); //endpoint di destinazione della notifica push ( arriva dalla lista delle subscription lato client  )
                    var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

                    Dictionary<string, object> options = new Dictionary<string, object>();
                    options.Add("vapidDetails", vapidDetails);
                    options.Add("gcmAPIKey", "");
                    //options.Add("headers", new Dictionary<string, object>());//elementi string, object .... che sono aggiunti alla notifica
                    //options.Add("TTL", 2419200); //in secondi tempo di default 4 settimane

                    //aggiungo l'id al payload
                    Dictionary<string, string> depayload = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,string>>(payload);
                    if(depayload !=null && depayload.ContainsKey("id")) depayload["id"] = devicebyid.Id.ToString();
                    payload = Newtonsoft.Json.JsonConvert.SerializeObject(depayload, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    var webPushClient = new WebPushClient();
                    webPushClient.SendNotification(pushSubscription, payload, options);
                    //webPushClient.SendNotification(pushSubscription, payload, vapidDetails);
                }
                catch (WebPushException e)
                {
                    //if (e.Message.ToLower().Contains("subscription no longer valid"))
                    if (e.StatusCode == System.Net.HttpStatusCode.Gone || e.StatusCode == System.Net.HttpStatusCode.Unauthorized || e.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        CancellaDevices(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, devicebyid.Id); //elimino le sottoscrizioni non più valide
                    errore += " " + e.Message + " statuscode :" + "Http STATUS code" + e.StatusCode;
                    //le sottoscrizioni che catchano per non valide le posso eleiminare da database ..... da fare
                }
            }
            else //invio a devices multipli
            {
                List<long> idtodelete = new List<long>();
                pushDM.DevicesList = pushDM.CaricaDevicesFiltratiScript(WelcomeLibrary.STATIC.Global.NomeConnessioneDb);//Ricarico tutti i device dal database application server
                DevicesCollection tmpcoll = new DevicesCollection(pushDM.DevicesList);
                foreach (Devices device in tmpcoll)
                {

                    try
                    {
                        var idkey = device.Id;
                        //invio al device specifico
                        var pushSubscription = new PushSubscription(device.PushEndpoint, device.PushP256DH, device.PushAuth); //endpoint di destinazione della notifica push ( arriva dalla lista delle subscription lato client  )
                        var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);
                        Dictionary<string, object> options = new Dictionary<string, object>();
                        options.Add("vapidDetails", vapidDetails);
                        options.Add("gcmAPIKey", "");
                        //options.Add("headers", new Dictionary<string, object>());//elementi string, object .... che sono aggiunti alla notifica
                        //options.Add("TTL", 2419200); //in secondi tempo di default 4 settimane
                        //aggiungo l'id al payload
                        Dictionary<string, string> depayload = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);
                        if (depayload != null && depayload.ContainsKey("id")) depayload["id"] = device.Id.ToString();
                        payload = Newtonsoft.Json.JsonConvert.SerializeObject(depayload, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None,
                        });

                        var webPushClient = new WebPushClient();
                        webPushClient.SendNotification(pushSubscription, payload, options);
                    }
                    catch (WebPushException e)
                    {

                        //if (e.Message.ToLower().Contains("subscription no longer valid"))
                        if (e.StatusCode == System.Net.HttpStatusCode.Gone || e.StatusCode == System.Net.HttpStatusCode.Unauthorized || e.StatusCode == System.Net.HttpStatusCode.Forbidden)
                            CancellaDevices(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, device.Id); //elimino le sottoscrizioni non più valide
                        errore += " " + e.Message + " statuscode :" + "Http STATUS code" + e.StatusCode + "\r\n";
                    }
                }


            }
            return errore;
        }



        public static Devices CaricaDevicesPerId(string connection, long Id)
        {
            if (connection == null || connection == "") return null;
            if (Id == 0) return null;

            Devices item = new Devices();
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();

            SQLiteParameter p1 = new SQLiteParameter("@id", Id); //OleDbType.VarChar
            parColl.Add(p1);
            try
            {
                string query = "SELECT  * FROM Devices  WHERE Id = @id ";
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;
                    while (reader.Read())
                    {
                        item = new Devices();

                        item.Id = reader.GetInt64(reader.GetOrdinal("Id"));
                        if (!reader["Name"].Equals(DBNull.Value))
                            item.Name = reader.GetString(reader.GetOrdinal("Name"));
                        if (!reader["PushEndpoint"].Equals(DBNull.Value))
                            item.PushEndpoint = reader.GetString(reader.GetOrdinal("PushEndpoint"));
                        if (!reader["PushP256DH"].Equals(DBNull.Value))
                            item.PushP256DH = reader.GetString(reader.GetOrdinal("PushP256DH"));
                        if (!reader["PushAuth"].Equals(DBNull.Value))
                            item.PushAuth = reader.GetString(reader.GetOrdinal("PushAuth"));

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



        public static DevicesCollection CaricaDevicesFiltratiScript(string connection, string ids = "all", string maxrecord = "", long page = 0, long pagesize = 0)
        {
            DevicesCollection list = new DevicesCollection();
            try
            {
                List<SQLiteParameter> pars = new List<SQLiteParameter>();

                if (long.TryParse(ids, out var id) && !string.IsNullOrEmpty(ids))
                {
                    SQLiteParameter pid = new SQLiteParameter("@id", id);
                    pars.Add(pid);
                    list = CaricaDevicesFiltratiScript(connection, pars, maxrecord, page, pagesize);
                }
                else if (ids == "all" || string.IsNullOrEmpty(ids))
                {
                    list = CaricaDevicesFiltratiScript(connection, pars, maxrecord, page, pagesize);

                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento :" + error.Message, error);
            }
            return list;
        }

        public static DevicesCollection CaricaDevicesFiltratiScript(string connection, List<SQLiteParameter> parColl, string maxrecord = "", long page = 0, long pagesize = 0)
        {
            DevicesCollection list = new DevicesCollection();
            if (connection == null || connection == "") return list;
            if ((page != 0 && pagesize > 1000)) return list;

            Devices item;
            try
            {
                List<SQLiteParameter> _parUsed = new List<SQLiteParameter>();
                string query = "";
                string queryfilter = "";


                query = "SELECT * FROM Devices ";
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
                query += queryfilter; //query da fare per i risultati


                query += "  order BY Id desc ";
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
                long totalrecords = dbDataAccess.ExecuteScalar<long>("SELECT count(*) FROM Devices " + queryfilter, _parUsed, connection);
                list.Recordstotali = totalrecords;

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, _parUsed, connection);
                long progressivo = 0;
                using (reader)
                {
                    if (reader == null) { return list; };
                    if (reader.HasRows == false)
                        return list;

                    while (reader.Read())
                    {
                        progressivo += 1;
                        item = new Devices();

                        item.Id = reader.GetInt64(reader.GetOrdinal("Id"));
                        if (!reader["Name"].Equals(DBNull.Value))
                            item.Name = reader.GetString(reader.GetOrdinal("Name"));
                        if (!reader["PushEndpoint"].Equals(DBNull.Value))
                            item.PushEndpoint = reader.GetString(reader.GetOrdinal("PushEndpoint"));
                        if (!reader["PushP256DH"].Equals(DBNull.Value))
                            item.PushP256DH = reader.GetString(reader.GetOrdinal("PushP256DH"));
                        if (!reader["PushAuth"].Equals(DBNull.Value))
                            item.PushAuth = reader.GetString(reader.GetOrdinal("PushAuth"));

                        list.Add(item);
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Devices :" + error.Message, error);
            }

            return list;
        }



        /// <summary>
        /// Inserisce o aggiorna una email nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public static void InserisciAggiorna(string connessione, Devices item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            if (!string.IsNullOrEmpty((item.PushEndpoint)) && item.Id == 0) //se non passo l'id in ogni caso cancello tutti gli endopoint uguali
            {
                CancellaDevices(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, 0, item.PushEndpoint);
            }


            SQLiteParameter p2b = new SQLiteParameter("@Name", item.Name);//OleDbType.VarChar
            parColl.Add(p2b);
            SQLiteParameter p2c = new SQLiteParameter("@PushAuth", item.PushAuth);//OleDbType.VarChar
            parColl.Add(p2c);
            SQLiteParameter p3 = new SQLiteParameter("@PushEndpoint", item.PushEndpoint);//OleDbType.VarChar
            parColl.Add(p3);
            SQLiteParameter p5 = new SQLiteParameter("@PushP256DH", item.PushP256DH);//OleDbType.VarChar
            parColl.Add(p5);

            string query = "";
            if (item.Id != 0)
            {
                //Update
                query = "UPDATE [Devices] SET Name=@Name,PushAuth=@PushAuth,PushEndpoint=@PushEndpoint,PushP256DH=@PushP256DH ";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO Devices (Name,PushAuth,PushEndpoint,PushP256DH )";
                query += " values ( ";
                query += "@Name,@PushAuth,@PushEndpoint,@PushP256DH )";
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


        public static void CancellaDevices(string connection, long Id, string endpoint = "")
        {
            if (connection == null || connection == "") return;
          
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@id", Id);//OleDbType.VarChar
            parColl.Add(p1);
            string query = "DELETE FROM Devices WHERE ([ID]=@id) ";
            if (!string.IsNullOrEmpty(endpoint))
            {
                SQLiteParameter p2 = new SQLiteParameter("@endpoint", endpoint);//OleDbType.VarChar
                parColl.Add(p2);
                query += " or PushEndpoint = @endpoint ";
            }
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connection);
                DevicesList.RemoveAll(dev => dev.Id == Id);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, cancellazione newsletter :" + error.Message, error);
            }
            return;
        }


    }
}
