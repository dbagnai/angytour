using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using System.Data.SQLite;

namespace WelcomeLibrary.DAL
{
    public class StruttureDM
    {
        /// <summary>
        /// Carica La struttura Passandogli l'id
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="Id_Struttura"></param>
        /// <returns></returns>
        public Struttura CaricaStrutturaPerEmail(string connection, string Email)
        {
            Struttura item = new Struttura();
            if (connection == null || connection == "") return item;
            //if (parColl == null || parColl.Count < 2) return list;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@Email", Email); //OleDbType.VarChar
            parColl.Add(p1);

            try
            {
                string query = "";
                query = "SELECT * FROM TBL_STRUTTURE WHERE Email = @Email";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Struttura();

                        item.Id_struttura = reader.GetInt64(reader.GetOrdinal("ID_STRUTTURA"));
                        item.Cap = reader.GetString(reader.GetOrdinal("Cap"));
                        item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));
                        item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));
                        item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        item.Email = reader.GetString(reader.GetOrdinal("Email"));
                        item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));
                        item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));
                        item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));

                        item.ModPagamento = reader.GetString(reader.GetOrdinal("ModalitaPagamento"));
                        item.Offerta1 = reader.GetString(reader.GetOrdinal("Offerta1"));
                        item.Offerta2 = reader.GetString(reader.GetOrdinal("Offerta2"));
                        item.PIva = reader.GetString(reader.GetOrdinal("PIva"));
                        item.RagSoc = reader.GetString(reader.GetOrdinal("RagSoc"));
                        item.Adesione = reader.GetString(reader.GetOrdinal("PacchettoAdesione"));
                        item.CodTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));

                        break;
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Struttura per Email :" + error.Message, error);
            }
            return item;

        }

        /// <summary>
        /// Carica La struttura Passandogli l'id
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="Id_Struttura"></param>
        /// <returns></returns>
        public Struttura CaricaStrutturaPerId(string connection, string Id_Struttura)
        {
            Struttura item = new Struttura();
            if (connection == null || connection == "") return item;
            //if (parColl == null || parColl.Count < 2) return list;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@ID_STRUTTURA", Id_Struttura); //OleDbType.VarChar
            parColl.Add(p1);

            try
            {
                string query = "";
                query = "SELECT * FROM TBL_STRUTTURE WHERE ID_STRUTTURA = @ID_STRUTTURA";

                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Struttura();

                        item.Id_struttura = reader.GetInt64(reader.GetOrdinal("ID_STRUTTURA"));
                        item.Cap = reader.GetString(reader.GetOrdinal("Cap"));
                        item.Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"));
                        item.CodiceCOMUNE = reader.GetString(reader.GetOrdinal("CodiceCOMUNE"));
                        item.CodiceNAZIONE = reader.GetString(reader.GetOrdinal("CodiceNAZIONE"));
                        item.CodicePROVINCIA = reader.GetString(reader.GetOrdinal("CodicePROVINCIA"));
                        item.CodiceREGIONE = reader.GetString(reader.GetOrdinal("CodiceREGIONE"));
                        item.Consenso1 = reader.GetBoolean(reader.GetOrdinal("Consenso1"));
                        item.Consenso2 = reader.GetBoolean(reader.GetOrdinal("Consenso2"));
                        item.Consenso3 = reader.GetBoolean(reader.GetOrdinal("Consenso3"));
                        item.Consenso4 = reader.GetBoolean(reader.GetOrdinal("Consenso4"));
                        item.ConsensoPrivacy = reader.GetBoolean(reader.GetOrdinal("ConsensoPrivacy"));
                        if (!reader["DataInvioValidazione"].Equals(DBNull.Value))
                            item.DataInvioValidazione = reader.GetDateTime(reader.GetOrdinal("DataInvioValidazione"));
                        if (!reader["DataRicezioneValidazione"].Equals(DBNull.Value))
                            item.DataRicezioneValidazione = reader.GetDateTime(reader.GetOrdinal("DataRicezioneValidazione"));
                        item.Email = reader.GetString(reader.GetOrdinal("Email"));
                        item.Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo"));
                        item.IPclient = reader.GetString(reader.GetOrdinal("IPclient"));
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        item.Spare1 = reader.GetString(reader.GetOrdinal("Spare1"));
                        item.Spare2 = reader.GetString(reader.GetOrdinal("Spare2"));
                        item.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        item.TestoFormConsensi = reader.GetString(reader.GetOrdinal("TestoFormConsensi"));
                        item.Validato = reader.GetBoolean(reader.GetOrdinal("Validato"));

                        item.ModPagamento = reader.GetString(reader.GetOrdinal("ModalitaPagamento"));
                        item.Offerta1 = reader.GetString(reader.GetOrdinal("Offerta1"));
                        item.Offerta2 = reader.GetString(reader.GetOrdinal("Offerta2"));
                        item.PIva = reader.GetString(reader.GetOrdinal("PIva"));
                        item.RagSoc = reader.GetString(reader.GetOrdinal("RagSoc"));
                        item.Adesione = reader.GetString(reader.GetOrdinal("PacchettoAdesione"));
                        item.CodTipologia = reader.GetString(reader.GetOrdinal("CodiceTIPOLOGIA"));

                        break;
                    }
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Struttura per ID :" + error.Message, error);
            }
            return item;

        }

        /// <summary>
        /// Inserisce o aggiorna i dati della struttura nel db
        /// Aggiorna se passato diverso da zero, altrimenti inserisce
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="item"></param>
        public void InserisciAggiornaStruttura(string connessione, ref Struttura item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;

            //SQLiteParameter p1 = new SQLiteParameter("@ID_struttura", item.Id_struttura);
            //parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@CodiceTIPOLOGIA", item.CodTipologia);
            parColl.Add(p2);
            SQLiteParameter p3 = new SQLiteParameter("@RagSoc", item.RagSoc);
            parColl.Add(p3);
            SQLiteParameter p4 = new SQLiteParameter("@PIva", item.PIva);
            parColl.Add(p4);
            SQLiteParameter p5 = new SQLiteParameter("@CodiceNAZIONE", item.CodiceNAZIONE);
            parColl.Add(p5);
            SQLiteParameter p6 = new SQLiteParameter("@CodiceREGIONE", item.CodiceREGIONE);
            parColl.Add(p6);
            SQLiteParameter p7 = new SQLiteParameter("@CodicePROVINCIA", item.CodicePROVINCIA);
            parColl.Add(p7);
            SQLiteParameter p8 = new SQLiteParameter("@CodiceCOMUNE", item.CodiceCOMUNE);
            parColl.Add(p8);
            SQLiteParameter p9 = new SQLiteParameter("@Cap", item.Cap);
            parColl.Add(p9);
            SQLiteParameter p10 = new SQLiteParameter("@Indirizzo", item.Indirizzo);
            parColl.Add(p10);
            SQLiteParameter p11 = new SQLiteParameter("@Email", item.Email);
            parColl.Add(p11);
            SQLiteParameter p12 = new SQLiteParameter("@Telefono", item.Telefono);
            parColl.Add(p12);
            SQLiteParameter p13 = new SQLiteParameter("@Cellulare", item.Cellulare);
            parColl.Add(p13);
            SQLiteParameter p14 = new SQLiteParameter("@Offerta1", item.Offerta1);
            parColl.Add(p14);
            SQLiteParameter p15 = new SQLiteParameter("@Offerta2", item.Offerta2);
            parColl.Add(p15);
            SQLiteParameter p16 = new SQLiteParameter("@PacchettoAdesione", item.Adesione);
            parColl.Add(p16);
            SQLiteParameter p17 = new SQLiteParameter("@ModalitaPagamento", item.ModPagamento);
            parColl.Add(p17);
            SQLiteParameter p18 = new SQLiteParameter("@IPclient", item.IPclient);
            parColl.Add(p18);

            SQLiteParameter p19 = null;
            if (item.DataInvioValidazione != null)
                p19 = new SQLiteParameter("@DataInvioValidazione", dbDataAccess.CorrectDatenow(item.DataInvioValidazione.Value));
            else
                p19 = new SQLiteParameter("@DataInvioValidazione", System.DBNull.Value);
            //p19.OleDbType = OleDbType.Date;
            parColl.Add(p19);

            SQLiteParameter p20;
            if (item.DataRicezioneValidazione != null)
                p20 = new SQLiteParameter("@DataRicezioneValidazione", dbDataAccess.CorrectDatenow(item.DataRicezioneValidazione.Value));
            else
                p20 = new SQLiteParameter("@DataRicezioneValidazione", System.DBNull.Value);
            //p20.OleDbType = OleDbType.Date;
            parColl.Add(p20);

            SQLiteParameter p21 = new SQLiteParameter("@Validato", item.Validato);
            parColl.Add(p21);
            SQLiteParameter p22 = new SQLiteParameter("@TestoFormConsensi", item.TestoFormConsensi);
            parColl.Add(p22);
            SQLiteParameter p23 = new SQLiteParameter("@ConsensoPrivacy", item.ConsensoPrivacy);//OleDbType.VarChar
            parColl.Add(p23);
            SQLiteParameter p24 = new SQLiteParameter("@Consenso1", item.Consenso1);//OleDbType.VarChar
            parColl.Add(p24);
            SQLiteParameter p25 = new SQLiteParameter("@Consenso2", item.Consenso2);//OleDbType.VarChar
            parColl.Add(p25);
            SQLiteParameter p26 = new SQLiteParameter("@Consenso3", item.Consenso3);//OleDbType.VarChar
            parColl.Add(p26);
            SQLiteParameter p27 = new SQLiteParameter("@Consenso4", item.Consenso4);//OleDbType.VarChar
            parColl.Add(p27);
            SQLiteParameter p28 = new SQLiteParameter("@Lingua", item.Lingua);
            parColl.Add(p28);
            SQLiteParameter p29 = new SQLiteParameter("@Spare1", item.Spare1);
            parColl.Add(p29);
            SQLiteParameter p30 = new SQLiteParameter("@Spare2", item.Spare2);
            parColl.Add(p30);

            string query = "";
            if (item.Id_struttura != 0)
            {
                //Aggiorno
                query = "UPDATE [TBL_STRUTTURE] SET CodiceTIPOLOGIA=@CodiceTIPOLOGIA,RagSoc=@RagSoc,PIva=@PIva,CodiceNAZIONE=@CodiceNAZIONE,CodiceREGIONE=@CodiceREGIONE,CodicePROVINCIA=@CodicePROVINCIA,CodiceCOMUNE=@CodiceCOMUNE";
                query += ",Cap=@Cap,Indirizzo=@Indirizzo,Email=@Email,Telefono=@Telefono,Cellulare=@Cellulare,Offerta1=@Offerta1,Offerta2=@Offerta2,PacchettoAdesione=@PacchettoAdesione,ModalitaPagamento=@ModalitaPagamento,IPclient=@IPclient";
                query += ",DataInvioValidazione=@DataInvioValidazione,DataRicezioneValidazione=@DataRicezioneValidazione,Validato=@Validato,TestoFormConsensi=@TestoFormConsensi,ConsensoPrivacy=@ConsensoPrivacy,Consenso1=@Consenso1,Consenso2=@Consenso2";
                query += ",Consenso3=@Consenso3,Consenso4=@Consenso4,Lingua=@Lingua,Spare1=@Spare1,Spare2=@Spare2";
                query += " WHERE [ID_STRUTTURA] = " + item.Id_struttura;
            }
            else
            {
                //Nuovo Inserimento
                query = "INSERT INTO TBL_STRUTTURE (CodiceTIPOLOGIA,RagSoc,PIva,CodiceNAZIONE,CodiceREGIONE,CodicePROVINCIA,CodiceCOMUNE,Cap,Indirizzo,Email,Telefono,Cellulare";
                query += ",Offerta1,Offerta2,PacchettoAdesione,ModalitaPagamento,IPclient,DataInvioValidazione,DataRicezioneValidazione,Validato,TestoFormConsensi";
                query += ",ConsensoPrivacy,Consenso1,Consenso2,Consenso3,Consenso4,Lingua,Spare1,Spare2)";
                query += " values ( ";
                query += "@CodiceTIPOLOGIA,@RagSoc,@PIva,@CodiceNAZIONE,@CodiceREGIONE,@CodicePROVINCIA,@CodiceCOMUNE,@Cap,@Indirizzo,@Email,@Telefono,@Cellulare,@Offerta1,@Offerta2";
                query += ",@PacchettoAdesione,@ModalitaPagamento,@IPclient,@DataInvioValidazione,@DataRicezioneValidazione,@Validato,@TestoFormConsensi,@ConsensoPrivacy";
                query += ",@Consenso1,@Consenso2,@Consenso3,@Consenso4,@Lingua,@Spare1,@Spare2 )";
            }
            try
            {
                long retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                if (item.Id_struttura == 0)
                item.Id_struttura = retID;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore, inserimento/aggiornamento Struttura :" + error.Message, error);
            }
        }
    }
}
