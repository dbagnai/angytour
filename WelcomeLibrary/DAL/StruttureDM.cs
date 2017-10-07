using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using System.Data.OleDb;

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
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@Email", Email); //OleDbType.VarChar
            parColl.Add(p1);

            try
            {
                string query = "";
                query = "SELECT * FROM TBL_STRUTTURE WHERE Email = @Email";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Struttura();

                        item.Id_struttura = reader.GetInt32(reader.GetOrdinal("ID_STRUTTURA"));
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
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p1 = new OleDbParameter("@ID_STRUTTURA", Id_Struttura); //OleDbType.VarChar
            parColl.Add(p1);

            try
            {
                string query = "";
                query = "SELECT * FROM TBL_STRUTTURE WHERE ID_STRUTTURA = @ID_STRUTTURA";

                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return item; };
                    if (reader.HasRows == false)
                        return item;

                    while (reader.Read())
                    {
                        item = new Struttura();

                        item.Id_struttura = reader.GetInt32(reader.GetOrdinal("ID_STRUTTURA"));
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
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;

            //OleDbParameter p1 = new OleDbParameter("@ID_struttura", item.Id_struttura);
            //parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@CodiceTIPOLOGIA", item.CodTipologia);
            parColl.Add(p2);
            OleDbParameter p3 = new OleDbParameter("@RagSoc", item.RagSoc);
            parColl.Add(p3);
            OleDbParameter p4 = new OleDbParameter("@PIva", item.PIva);
            parColl.Add(p4);
            OleDbParameter p5 = new OleDbParameter("@CodiceNAZIONE", item.CodiceNAZIONE);
            parColl.Add(p5);
            OleDbParameter p6 = new OleDbParameter("@CodiceREGIONE", item.CodiceREGIONE);
            parColl.Add(p6);
            OleDbParameter p7 = new OleDbParameter("@CodicePROVINCIA", item.CodicePROVINCIA);
            parColl.Add(p7);
            OleDbParameter p8 = new OleDbParameter("@CodiceCOMUNE", item.CodiceCOMUNE);
            parColl.Add(p8);
            OleDbParameter p9 = new OleDbParameter("@Cap", item.Cap);
            parColl.Add(p9);
            OleDbParameter p10 = new OleDbParameter("@Indirizzo", item.Indirizzo);
            parColl.Add(p10);
            OleDbParameter p11 = new OleDbParameter("@Email", item.Email);
            parColl.Add(p11);
            OleDbParameter p12 = new OleDbParameter("@Telefono", item.Telefono);
            parColl.Add(p12);
            OleDbParameter p13 = new OleDbParameter("@Cellulare", item.Cellulare);
            parColl.Add(p13);
            OleDbParameter p14 = new OleDbParameter("@Offerta1", item.Offerta1);
            parColl.Add(p14);
            OleDbParameter p15 = new OleDbParameter("@Offerta2", item.Offerta2);
            parColl.Add(p15);
            OleDbParameter p16 = new OleDbParameter("@PacchettoAdesione", item.Adesione);
            parColl.Add(p16);
            OleDbParameter p17 = new OleDbParameter("@ModalitaPagamento", item.ModPagamento);
            parColl.Add(p17);
            OleDbParameter p18 = new OleDbParameter("@IPclient", item.IPclient);
            parColl.Add(p18);

            OleDbParameter p19 = null;
            if (item.DataInvioValidazione != null)
                p19 = new OleDbParameter("@DataInvioValidazione", dbDataAccess.CorrectDatenow(item.DataInvioValidazione.Value));
            else
                p19 = new OleDbParameter("@DataInvioValidazione", System.DBNull.Value);
            //p19.OleDbType = OleDbType.Date;
            parColl.Add(p19);

            OleDbParameter p20;
            if (item.DataRicezioneValidazione != null)
                p20 = new OleDbParameter("@DataRicezioneValidazione", dbDataAccess.CorrectDatenow(item.DataRicezioneValidazione.Value));
            else
                p20 = new OleDbParameter("@DataRicezioneValidazione", System.DBNull.Value);
            //p20.OleDbType = OleDbType.Date;
            parColl.Add(p20);

            OleDbParameter p21 = new OleDbParameter("@Validato", item.Validato);
            parColl.Add(p21);
            OleDbParameter p22 = new OleDbParameter("@TestoFormConsensi", item.TestoFormConsensi);
            parColl.Add(p22);
            OleDbParameter p23 = new OleDbParameter("@ConsensoPrivacy", item.ConsensoPrivacy);//OleDbType.VarChar
            parColl.Add(p23);
            OleDbParameter p24 = new OleDbParameter("@Consenso1", item.Consenso1);//OleDbType.VarChar
            parColl.Add(p24);
            OleDbParameter p25 = new OleDbParameter("@Consenso2", item.Consenso2);//OleDbType.VarChar
            parColl.Add(p25);
            OleDbParameter p26 = new OleDbParameter("@Consenso3", item.Consenso3);//OleDbType.VarChar
            parColl.Add(p26);
            OleDbParameter p27 = new OleDbParameter("@Consenso4", item.Consenso4);//OleDbType.VarChar
            parColl.Add(p27);
            OleDbParameter p28 = new OleDbParameter("@Lingua", item.Lingua);
            parColl.Add(p28);
            OleDbParameter p29 = new OleDbParameter("@Spare1", item.Spare1);
            parColl.Add(p29);
            OleDbParameter p30 = new OleDbParameter("@Spare2", item.Spare2);
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
                int retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
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
