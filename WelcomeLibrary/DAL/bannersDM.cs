using System;
using System.Collections.Generic;
using System.Data;
using WelcomeLibrary.DOM;
using System.Data.SQLite;

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
            catch (Exception err)
            {
                ret = false;
            }

            return ret;
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

            if (mescola && banners!=null)
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

            DataRow _dr = null;
            if (banners != null)
                foreach (Banners _ban in banners)
                {
                    _dr = dt.NewRow();//Creo la riga vuota con la struttura voluta

                    _dr["ImageUrl"] = _ban.ImageUrlbyLingua(Lingua);
                    _dr["NavigateUrl"] = _ban.NavigateUrlbyLingua(Lingua);
                    _dr["AlternateText"] = _ban.AlternateTextbyLingua(Lingua);

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
                    query += "ORDER BY rnd(INT(NOW*id)-NOW*id), Id Desc";
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
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        if (!(reader["Progressivo"]).Equals(DBNull.Value))
                            item.progressivo = reader.GetInt32(reader.GetOrdinal("Progressivo"));

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
                        item.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        if (!(reader["Progressivo"]).Equals(DBNull.Value))
                            item.progressivo = reader.GetInt32(reader.GetOrdinal("Progressivo"));

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
                        //Devi controllare SYstem.DBnull!!!
                        if (!(reader["AlternateTextRU"]).Equals(DBNull.Value))
                            item.AlternateTextRU = reader.GetString(reader.GetOrdinal("AlternateTextRU"));

                        else
                            item.AlternateTextRU = "";

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

            SQLiteParameter p7 = new SQLiteParameter("@Progressivo", item.progressivo);
            parColl.Add(p7);



            string query = "INSERT INTO " + nometabella + "([ImageUrl],[NavigateUrl],[AlternateText],[AlternateTextGB],[ImageUrlGB],[NavigateUrlGB],[AlternateTextRU],[ImageUrlRU],[NavigateUrlRU],Progressivo) VALUES (@ImageUrl,@NavigateUrl,@AlternateText,@AlternateTextGB,@ImageUrlGB,@NavigateUrlGB,@AlternateTextRU,@ImageUrlRU,@NavigateUrlRU,@Progressivo)";

            if (!string.IsNullOrWhiteSpace(item.sezione))
            {
                SQLiteParameter p8 = new SQLiteParameter("@sezione", item.sezione);
                parColl.Add(p8);
                query = "INSERT INTO " + nometabella + "([ImageUrl],[NavigateUrl],[AlternateText],[AlternateTextGB],[ImageUrlGB],[NavigateUrlGB],[AlternateTextRU],[ImageUrlRU],[NavigateUrlRU],Progressivo,[Sezione]) VALUES (@ImageUrl,@NavigateUrl,@AlternateText,@AlternateTextGB,@ImageUrlGB,@NavigateUrlGB,@AlternateTextRU,@ImageUrlRU,@NavigateUrlRU,@Progressivo,@sezione)";
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
            SQLiteParameter p7 = new SQLiteParameter("@NavigateUrGB", item.NavigateUrlGB);
            parColl.Add(p7);

            SQLiteParameter p5r = new SQLiteParameter("@AlternateTextRU", item.AlternateTextRU);
            parColl.Add(p5r);
            SQLiteParameter p6r = new SQLiteParameter("@ImageUrlRU", item.ImageUrlRU);//OleDbType.VarChar
            parColl.Add(p6r);
            SQLiteParameter p7r = new SQLiteParameter("@NavigateUrRU", item.NavigateUrlRU);
            parColl.Add(p7r);

            SQLiteParameter p8 = new SQLiteParameter("@Progressivo", item.progressivo);
            parColl.Add(p8);
            string query = "UPDATE [" + nometabella + "] SET [ImageUrl]=@ImageUrl,[NavigateUrl]=@NavigateUrl,[AlternateText]=@AlternateText,[AlternateTextGB]=@AlternateTextGB,[ImageUrlGB]=@ImageUrlGB,[NavigateUrlGB]=@NavigateUrlGB,[AlternateTextRU]=@AlternateTextRU,[ImageUrlRU]=@ImageUrlRU,[NavigateUrlRU]=@NavigateUrlRU,[Progressivo]=@Progressivo  WHERE ([Id]=@id)";

            if (!string.IsNullOrWhiteSpace(item.sezione))
            {
                SQLiteParameter p9 = new SQLiteParameter("@sezione", item.sezione);
                parColl.Add(p9);
                query = "UPDATE [" + nometabella + "] SET [ImageUrl]=@ImageUrl,[NavigateUrl]=@NavigateUrl,[AlternateText]=@AlternateText,[AlternateTextGB]=@AlternateTextGB,[ImageUrlGB]=@ImageUrlGB,[NavigateUrlGB]=@NavigateUrlGB,[AlternateTextRU]=@AlternateTextRU,[ImageUrlRU]=@ImageUrlRU,[NavigateUrlRU]=@NavigateUrlRU,[Progressivo]=@Progressivo,[sezione]=@sezione  WHERE ([Id]=@id)";
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
