using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;
using System.Data.SQLite;

namespace WelcomeLibrary.DAL
{
    
    public class statisticheDM
    {

        /// <summary>
        /// Inserisce o aggiorna i dati di un cliente nel db.
        /// Aggiorna se passato id diverso da zero altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public static void InserisciAggiorna(string connessione, Statistiche item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            SQLiteParameter p1 = new SQLiteParameter("@Idattivita", item.Idattivita);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@EmailDestinatario", item.EmailDestinatario);//OleDbType.VarChar
            parColl.Add(p2);
            SQLiteParameter p2b = new SQLiteParameter("@EmailMittente", item.EmailMittente);//OleDbType.VarChar
            parColl.Add(p2b);
           // string _tmp = enumclass.TipoContatto.visitaurl.ToString();

            SQLiteParameter p3 = new SQLiteParameter("@TipoContatto", item.TipoContatto);//OleDbType.VarChar
            parColl.Add(p3);
            SQLiteParameter p5 = new SQLiteParameter("@Url", item.Url);//OleDbType.VarChar
            parColl.Add(p5);
            SQLiteParameter p4 = null;
            if (item.Data != null && item.Data != DateTime.MinValue)
                p4 = new SQLiteParameter("@Data", dbDataAccess.CorrectDatenow(item.Data));
            else
                p4 = new SQLiteParameter("@Data", dbDataAccess.CorrectDatenow(System.DateTime.Now));
            //p4.OleDbType = OleDbType.Date;
            parColl.Add(p4);

            SQLiteParameter p6 = new SQLiteParameter("@Testomail", item.Testomail);//OleDbType.VarChar
            parColl.Add(p6);

            string query = "";
            if (item.Id != 0)
            {
                //Update
                query = "UPDATE [TBL_STATISTICHE] SET Idattivita=@Idattivita,EmailDestinatario=@EmailDestinatario,EmailMitttente=@EmailMitttente,TipoContatto=@TipoContatto,Url=@Url,Data=@Data,Testomail=@Testomail";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_STATISTICHE (Idattivita,EmailDestinatario,EmailMittente,TipoContatto,Url";
                query += ",Data,Testomail )";
                query += " values ( ";
                query += "@Idattivita,@EmailDestinatario,@EmailMittente,@TipoContatto,@Url";
                query += ",@Data,@TestoMail )";
            }

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento statistiche :" + error.Message, error);
            }
            return;
        }




    }
}
