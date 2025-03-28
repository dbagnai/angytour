﻿using System;
using System.Collections.Generic;
using System.Data;
using WelcomeLibrary.DOM;
using System.Data.SQLite;
using Newtonsoft.Json;
using WelcomeLibrary.UF;
using System.Linq;

namespace WelcomeLibrary.DAL
{
    public class bannersDM
    {

        string nometabella = "tbl_banners";
        public bannersDM()
        {
        }
        public bannersDM(string nometbl)
        {
            this.nometabella = nometbl;
        }
        public DataTable GetTabellaBanners(string connessione)
        {
            string query = "select * from " + nometabella;
            DataTable dt = dbDataAccess.GetDataTableOle(query, null, connessione);
            return dt;
        }
        public bool TestPresenzaTabellaBanners(string connessione)
        {
            bool ret = false;
            try
            {
                string query = "select * from " + nometabella;
                DataTable dt = dbDataAccess.GetDataTableOle(query, null, connessione);
                ret = true;
            }
            catch
            {
                ret = false;
            }

            return ret;
        }
        /// <summary>
        /// Funzione caricamento con serializzazione dei risultati usata nell'handler
        /// </summary>
        /// <param name="lingua"></param>
        /// <param name="filtriBanner"></param>
        /// <param name="spage"></param>
        /// <param name="spagesize"></param>
        /// <param name="senablepager"></param>
        /// <returns></returns>
        public static Dictionary<string, string> filterDataBanner(string lingua, Dictionary<string, string> filtriBanner, string spage, string spagesize, string senablepager, string sessionid = "")
        {
            bool enabledpager = false;
            bool.TryParse(senablepager, out enabledpager);

            int page = 0;
            int pagesize = 0;
            int maxelement = 0;
            int.TryParse(spage, out page);
            int.TryParse(spagesize, out pagesize);
            bool smescola = false;
            bool.TryParse(filtriBanner["mescola"], out smescola);
            Dictionary<string, string> ritorno = new Dictionary<string, string>();
            BannersCollection banners = new BannersCollection();
            string tblsezione = filtriBanner["tblsezione"];
            string filtrosezione = filtriBanner["filtrosezione"];
            int.TryParse(filtriBanner["maxelement"], out maxelement);
            bannersDM banDM = new bannersDM(tblsezione);

            banners = banDM.CaricaBanners(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, filtrosezione.Trim(), smescola, maxelement);
            if (banners == null) banners = new BannersCollection();
            //dt = banDM.GetTabellaBannersGuidato(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Lingua, filtrosezione, mescola);

            //else
            //    offerte = filtri[4];

            List<Banners> filteredData = new List<Banners>();
            if (banners != null && banners.Count > 0 && enabledpager && page != 0 && pagesize != 0)
            {
                //Facciamo il take skip
                int start = ((page - 1) * pagesize);
                //int end = start + pagesize - 1;
                if (start + pagesize > banners.Count - 1)
                    filteredData = banners.GetRange(start, banners.Count - start);
                else
                    filteredData = banners.GetRange(start, pagesize).ToList();
            }
            else filteredData = banners;
            string tempOff = Newtonsoft.Json.JsonConvert.SerializeObject(filteredData, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            });
            ritorno.Add("data", tempOff);
            Dictionary<string, string> ListRet = new Dictionary<string, string>();
            string tot = "0";
            if (banners != null) tot = banners.Count.ToString();
            ListRet.Add("totalrecords", tot);

            string tempListret = Newtonsoft.Json.JsonConvert.SerializeObject(ListRet);
            ritorno.Add("resultinfo", tempListret);

            Dictionary<string, Dictionary<string, string>> linksurl = new Dictionary<string, Dictionary<string, string>>();
            foreach (Banners _o in filteredData)
            {
                Dictionary<string, string> tmp = new Dictionary<string, string>();
                string testotitolo = "";

                testotitolo = _o.AlternateTextbyLingua(lingua);
                string pathimmagine = _o.ImageUrlbyLingua(lingua); //percorso virtuale della foto banner sul server!!
                string imagedesc = _o.altimgtextbyLingua(lingua); //testo per l'alt dell'imagine

                ///////////////////////////////////////
                //Ricreiamo le anteprime se non presenti 
                if (!string.IsNullOrEmpty(pathimmagine))
                {
                    string physpathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione).Replace("/", "\\").Replace("\\\\", "\\");
                    string filename = System.IO.Path.GetFileName(physpathimmagine).ToString();
                    string filepath = System.IO.Path.GetDirectoryName(physpathimmagine).ToString();
                    filemanage.CreaAnteprima(physpathimmagine, 450, 450, filepath + "\\", "Ant" + filename, false, true);
                }
                ///////////////////////////////////////
                //Qui  posso decidere di passare le versioni in base alla risoluzione   WelcomeLibrary.STATIC.Global.Viewportw
                //andatando a modificare il noem del file aggiungendo -xs -xs -md -lg  a seconda della risoluzione al nome del file
                pathimmagine = filemanage.SelectImageByResolution(pathimmagine, Utility.ViewportwManagerGet(sessionid));

                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (string.IsNullOrEmpty(pathimmagine))
                    pathimmagine = "~/images/dummylogo.jpg".Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //string target = "_self";
                string link = _o.NavigateUrlbyLingua(lingua);
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && link.ToLower().IndexOf("~") == -1)
                {
                    // target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                tmp.Add("link", link);
                tmp.Add("titolo", (offerteDM.ReplaceLinks(testotitolo)));
                tmp.Add("image", pathimmagine);
                tmp.Add("imagedesc", imagedesc);
                linksurl.Add(_o.Id.ToString(), tmp);
            }

            string retlinksurl = Newtonsoft.Json.JsonConvert.SerializeObject(linksurl);
            ritorno.Add("linkloaded", retlinksurl);

            return ritorno;
        }

        /// <summary>
        /// Crea la datatable per l'adrotator a partire dalla tabella del db
        /// in modo guidato dalla lingua
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="Lingua"></param>
        /// <returns></returns>
        public DataTable GetTabellaBannersGuidato(string connessione, string Lingua, string filtro = "", bool mescola = false, int maxbanners = 0)
        {


            BannersCollection banners = null;
            banners = this.CaricaBanners(connessione, filtro.Trim(), mescola, maxbanners);

            if (mescola && banners != null)
                banners.Shuffle();

            //Trasformiamo i banner in una datatable con la struttura adatta all'adrotator
            DataTable dt = new DataTable();
            DataColumn myDataColumn;
            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ImageUrl";
            dt.Columns.Add(myDataColumn);
            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "NavigateUrl";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AlternateText";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "altimgtext";
            dt.Columns.Add(myDataColumn);

            DataRow _dr = null;
            if (banners != null)
                foreach (Banners _ban in banners)
                {
                    _dr = dt.NewRow();//Creo la riga vuota con la struttura voluta

                    _dr["ImageUrl"] = _ban.ImageUrlbyLingua(Lingua);
                    _dr["NavigateUrl"] = _ban.NavigateUrlbyLingua(Lingua);
                    _dr["AlternateText"] = _ban.AlternateTextbyLingua(Lingua);
                    _dr["altimgtext"] = _ban.altimgtextbyLingua(Lingua);

                    if (_dr["ImageUrl"].ToString() != "")
                        dt.Rows.Add(_dr);//INserisco la riga in tabella
                }
            return dt;
        }

        public BannersCollection CaricaBanners(string connection, string filtro = "", bool randomize = false, int maxbanners = 0)
        {
            if (connection == null || connection == "") return null;
            BannersCollection list = new BannersCollection();
            Banners item;

            try
            {
                List<SQLiteParameter> parColl = null;

                string query = "SELECT * FROM " + nometabella;
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    parColl = new List<SQLiteParameter>();
                    SQLiteParameter p1 = new SQLiteParameter("@sezione", filtro);//OleDbType.VarChar
                    parColl.Add(p1);
                    query += " where sezione = @sezione  ";
                }
                if (randomize)
                {
                    query += "ORDER BY random(), Id Desc";
                }
                else
                    query += " order BY progressivo asc, DataInserimento Desc";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {

                        item = new Banners();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!(reader["Progressivo"]).Equals(DBNull.Value))
                            item.progressivo = reader.GetInt64(reader.GetOrdinal("Progressivo"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));

                        item.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        item.NavigateUrl = reader.GetString(reader.GetOrdinal("NavigateUrl"));
                        item.AlternateText = reader.GetString(reader.GetOrdinal("AlternateText"));

                        item.ImageUrlGB = reader.GetString(reader.GetOrdinal("ImageUrlGB"));
                        item.NavigateUrlGB = reader.GetString(reader.GetOrdinal("NavigateUrlGB"));
                        //Devi controllare SYstem.DBnull!!!
                        if (!(reader["AlternateTextGB"]).Equals(DBNull.Value))
                            item.AlternateTextGB = reader.GetString(reader.GetOrdinal("AlternateTextGB"));
                        else
                            item.AlternateTextGB = "";


                        item.ImageUrlRU = reader.GetString(reader.GetOrdinal("ImageUrlRU"));
                        item.NavigateUrlRU = reader.GetString(reader.GetOrdinal("NavigateUrlRU"));
                        //Devi controllare SYstem.DBnull!!!
                        if (!(reader["AlternateTextRU"]).Equals(DBNull.Value))
                            item.AlternateTextRU = reader.GetString(reader.GetOrdinal("AlternateTextRU"));
                        else
                            item.AlternateTextRU = "";

                        if (!(reader["ImageUrlFR"]).Equals(DBNull.Value)) item.ImageUrlFR = reader.GetString(reader.GetOrdinal("ImageUrlFR"));
                        if (!(reader["NavigateUrlFR"]).Equals(DBNull.Value)) item.NavigateUrlFR = reader.GetString(reader.GetOrdinal("NavigateUrlFR"));
                        if (!(reader["AlternateTextFR"]).Equals(DBNull.Value))
                            item.AlternateTextFR = reader.GetString(reader.GetOrdinal("AlternateTextFR"));
                        else
                            item.AlternateTextFR = "";


                        if (!(reader["ImageUrlES"]).Equals(DBNull.Value)) item.ImageUrlES = reader.GetString(reader.GetOrdinal("ImageUrlES"));
                        if (!(reader["NavigateUrlES"]).Equals(DBNull.Value)) item.NavigateUrlES = reader.GetString(reader.GetOrdinal("NavigateUrlES"));
                        if (!(reader["AlternateTextES"]).Equals(DBNull.Value))
                            item.AlternateTextES = reader.GetString(reader.GetOrdinal("AlternateTextES"));
                        else
                            item.AlternateTextES = "";



                        if (!(reader["ImageUrlDE"]).Equals(DBNull.Value)) item.ImageUrlDE = reader.GetString(reader.GetOrdinal("ImageUrlDE"));
                        if (!(reader["NavigateUrlDE"]).Equals(DBNull.Value)) item.NavigateUrlDE = reader.GetString(reader.GetOrdinal("NavigateUrlDE"));
                        if (!(reader["AlternateTextDE"]).Equals(DBNull.Value))
                            item.AlternateTextDE = reader.GetString(reader.GetOrdinal("AlternateTextDE"));
                        else
                            item.AlternateTextDE = "";

                        if (!(reader["altimgtextI"]).Equals(DBNull.Value))
                            item.AltimgtextI = reader.GetString(reader.GetOrdinal("altimgtextI"));
                        else
                            item.AltimgtextI = "";

                        if (!(reader["altimgtextGB"]).Equals(DBNull.Value))
                            item.AltimgtextGB = reader.GetString(reader.GetOrdinal("altimgtextGB"));
                        else
                            item.AltimgtextGB = "";

                        if (!(reader["altimgtextRU"]).Equals(DBNull.Value))
                            item.AltimgtextRU = reader.GetString(reader.GetOrdinal("altimgtextRU"));
                        else
                            item.AltimgtextRU = "";

                        if (!(reader["altimgtextFR"]).Equals(DBNull.Value))
                            item.AltimgtextFR = reader.GetString(reader.GetOrdinal("altimgtextFR"));
                        else
                            item.AltimgtextFR = "";

                        if (!(reader["altimgtextES"]).Equals(DBNull.Value))
                            item.AltimgtextES = reader.GetString(reader.GetOrdinal("altimgtextES"));
                        else
                            item.AltimgtextES = "";


                        if (!(reader["altimgtextDE"]).Equals(DBNull.Value))
                            item.AltimgtextDE = reader.GetString(reader.GetOrdinal("altimgtextDE"));
                        else
                            item.AltimgtextDE = "";

                        list.Add(item);
                        if (maxbanners != 0 && list.Count >= maxbanners) break;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Banners :" + error.Message, error);
            }

            return list;
        }

        public Banners CaricaBannerPerID(string connection, string ID)
        {
            if (connection == null || connection == "") return null;
            if (ID == null || ID == "") return null;
            BannersCollection list = new BannersCollection();
            Banners item = null;

            try
            {
                string query = "SELECT * FROM " + nometabella + " where ID=@ID  order BY progressivo asc, DataInserimento Desc";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@ID", ID);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {

                        item = new Banners();
                        item.Id = reader.GetInt64(reader.GetOrdinal("ID"));
                        if (!(reader["Progressivo"]).Equals(DBNull.Value))
                            item.progressivo = reader.GetInt64(reader.GetOrdinal("Progressivo"));

                        item.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        item.NavigateUrl = reader.GetString(reader.GetOrdinal("NavigateUrl"));
                        item.AlternateText = reader.GetString(reader.GetOrdinal("AlternateText"));
                        item.DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento"));

                        item.ImageUrlGB = reader.GetString(reader.GetOrdinal("ImageUrlGB"));
                        item.NavigateUrlGB = reader.GetString(reader.GetOrdinal("NavigateUrlGB"));
                        //Devi controllare SYstem.DBnull!!!
                        if (!(reader["AlternateTextGB"]).Equals(DBNull.Value))
                            item.AlternateTextGB = reader.GetString(reader.GetOrdinal("AlternateTextGB"));
                        else
                            item.AlternateTextGB = "";

                        item.ImageUrlRU = reader.GetString(reader.GetOrdinal("ImageUrlRU"));
                        item.NavigateUrlRU = reader.GetString(reader.GetOrdinal("NavigateUrlRU"));
                        if (!(reader["AlternateTextRU"]).Equals(DBNull.Value))
                            item.AlternateTextRU = reader.GetString(reader.GetOrdinal("AlternateTextRU"));
                        else
                            item.AlternateTextRU = "";

                        if (!(reader["ImageUrlFR"]).Equals(DBNull.Value)) item.ImageUrlFR = reader.GetString(reader.GetOrdinal("ImageUrlFR"));
                        if (!(reader["NavigateUrlFR"]).Equals(DBNull.Value)) item.NavigateUrlFR = reader.GetString(reader.GetOrdinal("NavigateUrlFR"));
                        if (!(reader["AlternateTextFR"]).Equals(DBNull.Value))
                            item.AlternateTextFR = reader.GetString(reader.GetOrdinal("AlternateTextFR"));
                        else
                            item.AlternateTextFR = "";

                        if (!(reader["ImageUrlES"]).Equals(DBNull.Value)) item.ImageUrlES = reader.GetString(reader.GetOrdinal("ImageUrlES"));
                        if (!(reader["NavigateUrlES"]).Equals(DBNull.Value)) item.NavigateUrlES = reader.GetString(reader.GetOrdinal("NavigateUrlES"));
                        if (!(reader["AlternateTextES"]).Equals(DBNull.Value))
                            item.AlternateTextES = reader.GetString(reader.GetOrdinal("AlternateTextES"));
                        else
                            item.AlternateTextES = "";


                        if (!(reader["ImageUrlDE"]).Equals(DBNull.Value)) item.ImageUrlDE = reader.GetString(reader.GetOrdinal("ImageUrlDE"));
                        if (!(reader["NavigateUrlDE"]).Equals(DBNull.Value)) item.NavigateUrlDE = reader.GetString(reader.GetOrdinal("NavigateUrlDE"));
                        if (!(reader["AlternateTextDE"]).Equals(DBNull.Value))
                            item.AlternateTextDE = reader.GetString(reader.GetOrdinal("AlternateTextDE"));
                        else
                            item.AlternateTextDE = "";


                        if (!(reader["altimgtextI"]).Equals(DBNull.Value))
                            item.AltimgtextI = reader.GetString(reader.GetOrdinal("altimgtextI"));
                        else
                            item.AltimgtextI = "";

                        if (!(reader["altimgtextGB"]).Equals(DBNull.Value))
                            item.AltimgtextGB = reader.GetString(reader.GetOrdinal("altimgtextGB"));
                        else
                            item.AltimgtextGB = "";

                        if (!(reader["altimgtextRU"]).Equals(DBNull.Value))
                            item.AltimgtextRU = reader.GetString(reader.GetOrdinal("altimgtextRU"));
                        else
                            item.AltimgtextRU = "";

                        if (!(reader["altimgtextFR"]).Equals(DBNull.Value))
                            item.AltimgtextFR = reader.GetString(reader.GetOrdinal("altimgtextFR"));
                        else
                            item.AltimgtextFR = "";

                        if (!(reader["altimgtextDE"]).Equals(DBNull.Value))
                            item.AltimgtextDE = reader.GetString(reader.GetOrdinal("altimgtextDE"));
                        else
                            item.AltimgtextDE = "";


                        if (!(reader["altimgtextES"]).Equals(DBNull.Value))
                            item.AltimgtextES = reader.GetString(reader.GetOrdinal("altimgtextES"));
                        else
                            item.AltimgtextES = "";


                        return item;
                    }
                }

            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Banner :" + error.Message, error);
            }

            return item;
        }

        public void InsertBanner(string connessione,
        Banners item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@ImageUrl", item.ImageUrl);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@NavigateUrl", item.NavigateUrl);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@AlternateText", item.AlternateText);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@AlternateTextGB", item.AlternateTextGB);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@ImageUrlGB", item.ImageUrlGB);//OleDbType.VarChar
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@NavigateUrlGB", item.NavigateUrlGB);
            parColl.Add(p6);

            SQLiteParameter p4r = new SQLiteParameter("@AlternateTextRU", item.AlternateTextRU);
            parColl.Add(p4r);
            SQLiteParameter p5r = new SQLiteParameter("@ImageUrlRU", item.ImageUrlRU);//OleDbType.VarChar
            parColl.Add(p5r);
            SQLiteParameter p6r = new SQLiteParameter("@NavigateUrlRU", item.NavigateUrlRU);
            parColl.Add(p6r);

            SQLiteParameter p4d = new SQLiteParameter("@AlternateTextFR", item.AlternateTextFR);
            parColl.Add(p4d);
            SQLiteParameter p5d = new SQLiteParameter("@ImageUrlFR", item.ImageUrlFR);//OleDbType.VarChar
            parColl.Add(p5d);
            SQLiteParameter p6d = new SQLiteParameter("@NavigateUrlFR", item.NavigateUrlFR);
            parColl.Add(p6d);


            SQLiteParameter p4e = new SQLiteParameter("@AlternateTextDE", item.AlternateTextDE);
            parColl.Add(p4e);
            SQLiteParameter p5e = new SQLiteParameter("@ImageUrlDE", item.ImageUrlDE);//OleDbType.VarChar
            parColl.Add(p5e);
            SQLiteParameter p6e = new SQLiteParameter("@NavigateUrlDE", item.NavigateUrlDE);
            parColl.Add(p6e);

            SQLiteParameter p4f = new SQLiteParameter("@AlternateTextES", item.AlternateTextES);
            parColl.Add(p4f);
            SQLiteParameter p5f = new SQLiteParameter("@ImageUrlES", item.ImageUrlES);//OleDbType.VarChar
            parColl.Add(p5f);
            SQLiteParameter p6f = new SQLiteParameter("@NavigateUrlES", item.NavigateUrlES);
            parColl.Add(p6f);

            SQLiteParameter p7 = new SQLiteParameter("@Progressivo", item.progressivo);
            parColl.Add(p7);

            SQLiteParameter p8a = new SQLiteParameter("@altimgtextI", item.AltimgtextI);
            parColl.Add(p8a);
            SQLiteParameter p8b = new SQLiteParameter("@altimgtextGB", item.AltimgtextGB);
            parColl.Add(p8b);
            SQLiteParameter p8c = new SQLiteParameter("@altimgtextRU", item.AltimgtextRU);
            parColl.Add(p8c);
            SQLiteParameter p8d = new SQLiteParameter("@altimgtextFR", item.AltimgtextFR);
            parColl.Add(p8d);
            SQLiteParameter p8e = new SQLiteParameter("@altimgtextDE", item.AltimgtextDE);
            parColl.Add(p8e);
            SQLiteParameter p8f = new SQLiteParameter("@altimgtextES", item.AltimgtextES);
            parColl.Add(p8f);


            string query = "INSERT INTO " + nometabella + "([ImageUrl],[NavigateUrl],[AlternateText],[AlternateTextGB],[ImageUrlGB],[NavigateUrlGB],[AlternateTextRU],[ImageUrlRU],[NavigateUrlRU],[AlternateTextFR],[ImageUrlFR],[NavigateUrlFR],[AlternateTextDE],[ImageUrlDE],[NavigateUrlDE],[AlternateTextES],[ImageUrlES],[NavigateUrlES],Progressivo,altimgtextI,altimgtextGB,altimgtextRU,altimgtextFR,altimgtextDE,altimgtextES) VALUES (@ImageUrl,@NavigateUrl,@AlternateText,@AlternateTextGB,@ImageUrlGB,@NavigateUrlGB,@AlternateTextRU,@ImageUrlRU,@NavigateUrlRU,@AlternateTextFR,@ImageUrlFR,@NavigateUrlFR,@AlternateTextDE,@ImageUrlDE,@NavigateUrlDE,@AlternateTextES,@ImageUrlES,@NavigateUrlES,@Progressivo,@altimgtextI,@altimgtextGB,@altimgtextRU,@altimgtextFR,@altimgtextDE,@altimgtextES)";

            if (!string.IsNullOrWhiteSpace(item.sezione))
            {
                SQLiteParameter p8 = new SQLiteParameter("@sezione", item.sezione);
                parColl.Add(p8);
                query = "INSERT INTO " + nometabella + "([ImageUrl],[NavigateUrl],[AlternateText],[AlternateTextGB],[ImageUrlGB],[NavigateUrlGB],[AlternateTextRU],[ImageUrlRU],[NavigateUrlRU],[AlternateTextFR],[ImageUrlFR],[NavigateUrlFR],[AlternateTextDE],[ImageUrlDE],[NavigateUrlDE],[AlternateTextES],[ImageUrlES],[NavigateUrlES],Progressivo,[Sezione],altimgtextI,altimgtextGB,altimgtextRU,altimgtextFR,altimgtextDE,altimgtextES) VALUES (@ImageUrl,@NavigateUrl,@AlternateText,@AlternateTextGB,@ImageUrlGB,@NavigateUrlGB,@AlternateTextRU,@ImageUrlRU,@NavigateUrlRU,@AlternateTextFR,@ImageUrlFR,@NavigateUrlFR,@AlternateTextDE,@ImageUrlDE,@NavigateUrlDE,@AlternateTextES,@ImageUrlES,@NavigateUrlES,@Progressivo,@sezione,@altimgtextI,@altimgtextGB,@altimgtextRU,@altimgtextFR,@altimgtextDE,@altimgtextES)";
            }
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento Banner :" + error.Message, error);
            }
            return;
        }

        public void UpdateBanners(string connessione,
            Banners item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p2 = new SQLiteParameter("@ImageUrl", item.ImageUrl);//OleDbType.VarChar
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@NavigateUrl", item.NavigateUrl);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@AlternateText", item.AlternateText);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@AlternateTextGB", item.AlternateTextGB);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@ImageUrlGB", item.ImageUrlGB);//OleDbType.VarChar
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@NavigateUrlGB", item.NavigateUrlGB);
            parColl.Add(p7);

            SQLiteParameter p5r = new SQLiteParameter("@AlternateTextRU", item.AlternateTextRU);
            parColl.Add(p5r);
            SQLiteParameter p6r = new SQLiteParameter("@ImageUrlRU", item.ImageUrlRU);//OleDbType.VarChar
            parColl.Add(p6r);
            SQLiteParameter p7r = new SQLiteParameter("@NavigateUrlRU", item.NavigateUrlRU);
            parColl.Add(p7r);


            SQLiteParameter p5d = new SQLiteParameter("@AlternateTextFR", item.AlternateTextFR);
            parColl.Add(p5d);
            SQLiteParameter p6d = new SQLiteParameter("@ImageUrlFR", item.ImageUrlFR);//OleDbType.VarChar
            parColl.Add(p6d);
            SQLiteParameter p7d = new SQLiteParameter("@NavigateUrlFR", item.NavigateUrlFR);
            parColl.Add(p7d);


            SQLiteParameter p4e = new SQLiteParameter("@AlternateTextDE", item.AlternateTextDE);
            parColl.Add(p4e);
            SQLiteParameter p5e = new SQLiteParameter("@ImageUrlDE", item.ImageUrlDE);//OleDbType.VarChar
            parColl.Add(p5e);
            SQLiteParameter p6e = new SQLiteParameter("@NavigateUrlDE", item.NavigateUrlDE);
            parColl.Add(p6e);

            SQLiteParameter p4f = new SQLiteParameter("@AlternateTextES", item.AlternateTextES);
            parColl.Add(p4f);
            SQLiteParameter p5f = new SQLiteParameter("@ImageUrlES", item.ImageUrlES);//OleDbType.VarChar
            parColl.Add(p5f);
            SQLiteParameter p6f = new SQLiteParameter("@NavigateUrlES", item.NavigateUrlES);
            parColl.Add(p6f);


            SQLiteParameter p8 = new SQLiteParameter("@Progressivo", item.progressivo);
            parColl.Add(p8);

            SQLiteParameter p8a = new SQLiteParameter("@altimgtextI", item.AltimgtextI);
            parColl.Add(p8a);
            SQLiteParameter p8b = new SQLiteParameter("@altimgtextGB", item.AltimgtextGB);
            parColl.Add(p8b);
            SQLiteParameter p8c = new SQLiteParameter("@altimgtextRU", item.AltimgtextRU);
            parColl.Add(p8c);
            SQLiteParameter p8d = new SQLiteParameter("@altimgtextFR", item.AltimgtextFR);
            parColl.Add(p8d);

            SQLiteParameter p8e = new SQLiteParameter("@altimgtextDE", item.AltimgtextDE);
            parColl.Add(p8e);
            SQLiteParameter p8f = new SQLiteParameter("@altimgtextES", item.AltimgtextES);
            parColl.Add(p8f);

            string query = "UPDATE [" + nometabella + "] SET [ImageUrl]=@ImageUrl,[NavigateUrl]=@NavigateUrl,[AlternateText]=@AlternateText,[AlternateTextGB]=@AlternateTextGB,[ImageUrlGB]=@ImageUrlGB,[NavigateUrlGB]=@NavigateUrlGB,[AlternateTextRU]=@AlternateTextRU,[ImageUrlRU]=@ImageUrlRU,[NavigateUrlRU]=@NavigateUrlRU,[AlternateTextFR]=@AlternateTextFR,[ImageUrlFR]=@ImageUrlFR,[NavigateUrlFR]=@NavigateUrlFR,[AlternateTextDE]=@AlternateTextDE,[ImageUrlDE]=@ImageUrlDE,[NavigateUrlDE]=@NavigateUrlDE,[AlternateTextES]=@AlternateTextES,[ImageUrlES]=@ImageUrlES,[NavigateUrlES]=@NavigateUrlES,[Progressivo]=@Progressivo,[altimgtextI]=@altimgtextI,[altimgtextGB]=@altimgtextGB,[altimgtextRU]=@altimgtextRU,[altimgtextFR]=@altimgtextFR,[altimgtextDE]=@altimgtextDE,[altimgtextES]=@altimgtextES WHERE ([Id]=@id)";

            if (!string.IsNullOrWhiteSpace(item.sezione))
            {
                SQLiteParameter p9 = new SQLiteParameter("@sezione", item.sezione);
                parColl.Add(p9);
                query = "UPDATE [" + nometabella + "] SET [ImageUrl]=@ImageUrl,[NavigateUrl]=@NavigateUrl,[AlternateText]=@AlternateText,[AlternateTextGB]=@AlternateTextGB,[ImageUrlGB]=@ImageUrlGB,[NavigateUrlGB]=@NavigateUrlGB,[AlternateTextRU]=@AlternateTextRU,[ImageUrlRU]=@ImageUrlRU,[NavigateUrlRU]=@NavigateUrlRU,[AlternateTextFR]=@AlternateTextFR,[ImageUrlFR]=@ImageUrlFR,[NavigateUrlFR]=@NavigateUrlFR,[AlternateTextDE]=@AlternateTextDE,[ImageUrlDE]=@ImageUrlDE,[NavigateUrlDE]=@NavigateUrlDE,[AlternateTextES]=@AlternateTextES,[ImageUrlES]=@ImageUrlES,[NavigateUrlES]=@NavigateUrlES,[Progressivo]=@Progressivo,[altimgtextI]=@altimgtextI,[altimgtextGB]=@altimgtextGB,[altimgtextRU]=@altimgtextRU,[altimgtextFR]=@altimgtextFR,[altimgtextDE]=@altimgtextDE,[altimgtextES]=@altimgtextES,[sezione]=@sezione  WHERE ([Id]=@id)";
            }

            SQLiteParameter p1 = new SQLiteParameter("@id", item.Id);//OleDbType.VarChar
            parColl.Add(p1);

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, aggiornamento Banners :" + error.Message, error);
            }
            return;
        }

        public void DeleteBanners(string connessione,
                Banners item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (item == null || item.Id == 0) return;

            SQLiteParameter p1 = new SQLiteParameter("@id", item.Id);//OleDbType.VarChar
            parColl.Add(p1);

            string query = "DELETE FROM " + nometabella + " WHERE ([ID]=@id)";
            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, cancellazione Banners :" + error.Message, error);
            }
            return;
        }
    }
}
