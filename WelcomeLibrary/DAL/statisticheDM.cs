using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;

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
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            OleDbParameter p1 = new OleDbParameter("@Idattivita", item.Idattivita);//OleDbType.VarChar
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@EmailDestinatario", item.EmailDestinatario);//OleDbType.VarChar
            parColl.Add(p2);
            OleDbParameter p2b = new OleDbParameter("@EmailMittente", item.EmailMittente);//OleDbType.VarChar
            parColl.Add(p2b);
           // string _tmp = enumclass.TipoContatto.visitaurl.ToString();

            OleDbParameter p3 = new OleDbParameter("@TipoContatto", item.TipoContatto);//OleDbType.VarChar
            parColl.Add(p3);
            OleDbParameter p5 = new OleDbParameter("@Url", item.Url);//OleDbType.VarChar
            parColl.Add(p5);
            OleDbParameter p4 = null;
            if (item.Data != null && item.Data != DateTime.MinValue)
                p4 = new OleDbParameter("@Data", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",item.Data));
            else
                p4 = new OleDbParameter("@Data", String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:dd/MM/yyyy HH:mm:ss}",System.DateTime.Now));
            //p4.OleDbType = OleDbType.Date;
            parColl.Add(p4);

            OleDbParameter p6 = new OleDbParameter("@Testomail", item.Testomail);//OleDbType.VarChar
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
