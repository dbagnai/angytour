using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WelcomeLibrary.UF
{

    public static class MemoriaDisco
    {

        public static string physiclogdir = "";
        /// <summary>
        /// Crea un file di accesso col nome specificato
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <param name="testo"></param>
        /// <param name="nomefile"></param>
        /// <returns></returns>
        public static string CreaFileAccesso(string PhyPath, string testo, string nomefile)
        {
            string ritorno = "";
            try
            {
                //SCRIVIAMO IL FILE PER L'ACCESSO

                File.WriteAllText(PhyPath + "\\" + nomefile, testo);
            }
            catch (Exception error)
            {
                ritorno = error.Message.ToString();
            }
            return ritorno;
        }


        /// <summary>
        /// Crea il file per il controllo degli accessi via ftp
        /// Ritorna stringa vuota se non ci sono errori altrimenti ritorna la stringa di errore
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <param name="Contenuti">Deve contenere i tre elementi Operatore, Stato, Messaggio</param>
        /// <returns></returns>
        public static string CreaFileAccessoFtp(string PhyPath, Dictionary<string, string> Contenuti)
        {
            string ritorno = "";
            try
            {
                //SCRIVIAMO IL FILE PER L'ACCESSO
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<Operatore>");
                sb.Append(Contenuti["Operatore"].ToString());
                sb.Append("</Operatore>");
                sb.AppendLine();
                sb.Append("<Stato>");
                sb.Append(Contenuti["Stato"].ToString());
                sb.Append("</Stato>");
                sb.AppendLine();
                sb.Append("<Messaggio>");
                sb.Append(Contenuti["Messaggio"].ToString());
                sb.Append("</Messaggio>");
                File.WriteAllText(PhyPath + "\\ftpTransfer.txt", sb.ToString());
            }
            catch (Exception error)
            {
                ritorno = error.Message.ToString();
            }
            return ritorno;
        }
        /// <summary>
        /// Crea il file per il controllo degli accessi 
        /// Ritorna stringa vuota se non ci sono errori altrimenti ritorna la stringa di errore
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <param name="Contenuti">Deve contenere i tre elementi Operatore, Stato, Messaggio</param>
        /// <returns></returns>
        public static string CreaFileAccesso(string PhyPath, Dictionary<string, string> Contenuti, string nomefile)
        {
            string ritorno = "";
            try
            {
                //SCRIVIAMO IL FILE PER L'ACCESSO
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<Operatore>");
                sb.Append(Contenuti["Operatore"].ToString());
                sb.Append("</Operatore>");
                sb.AppendLine();
                sb.Append("<Stato>");
                sb.Append(Contenuti["Stato"].ToString());
                sb.Append("</Stato>");
                sb.AppendLine();
                sb.Append("<Messaggio>");
                sb.Append(Contenuti["Messaggio"].ToString());
                sb.Append("</Messaggio>");
                File.WriteAllText(PhyPath + "\\" + nomefile, sb.ToString());
            }
            catch (Exception error)
            {
                ritorno = error.Message.ToString();
            }
            return ritorno;
        }



        /// <summary>
        /// Legge i dati dal file di accesso ftp
        /// Ritorna null in caso di errore o se non trova il file 
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> LeggiFileAccessoFtp(string PhyPath)
        {
            Dictionary<string, string> Contenuti = null;
            try
            {
                string CONTENUTO = "";
                if (File.Exists(PhyPath + "\\ftpTransfer.txt"))
                {
                    Contenuti = new Dictionary<string, string>();
                    //Rileggiamo ciò che si è scritto
                    CONTENUTO = File.ReadAllText(PhyPath + "\\ftpTransfer.txt");
                    string Operatore = CONTENUTO.Substring(CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length, CONTENUTO.IndexOf("</Operatore>") - (CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length));
                    Contenuti.Add("Operatore", Operatore);
                    string Stato = CONTENUTO.Substring(CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length, CONTENUTO.IndexOf("</Stato>") - (CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length));
                    Contenuti.Add("Stato", Stato);
                    string Messaggio = CONTENUTO.Substring(CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length, CONTENUTO.IndexOf("</Messaggio>") - (CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length));
                    Contenuti.Add("Messaggio", Messaggio);
                }
            }
            catch
            {
                return null;
            }
            return Contenuti;
        }
        /// <summary>
        /// Legge i dati dal file di accesso
        /// Ritorna null in caso di errore o se non trova il file 
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> LeggiFileAccesso(string PhyPath, string Nomefile)
        {
            Dictionary<string, string> Contenuti = null;
            try
            {
                string CONTENUTO = "";
                if (File.Exists(PhyPath + "\\" + Nomefile))
                {
                    Contenuti = new Dictionary<string, string>();
                    //Rileggiamo ciò che si è scritto
                    CONTENUTO = File.ReadAllText(PhyPath + "\\" + Nomefile);
                    string Operatore = CONTENUTO.Substring(CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length, CONTENUTO.IndexOf("</Operatore>") - (CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length));
                    Contenuti.Add("Operatore", Operatore);
                    string Stato = CONTENUTO.Substring(CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length, CONTENUTO.IndexOf("</Stato>") - (CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length));
                    Contenuti.Add("Stato", Stato);
                    string Messaggio = CONTENUTO.Substring(CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length, CONTENUTO.IndexOf("</Messaggio>") - (CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length));
                    Contenuti.Add("Messaggio", Messaggio);
                }
            }
            catch
            {
                return null;
            }
            return Contenuti;
        }

        /// <summary>
        /// Modifica i dati dal file di accesso ftp con quelli passati
        /// Ritorna stringa vuota se tutto ok altrimenti nella stringa c'è l'errore 
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <returns></returns>
        public static string ModificaFileAccessoFtp(string PhyPath, Dictionary<string, string> Contenuti)
        {
            string ritorno = "";
            try
            {
                string CONTENUTO = "";
                if (File.Exists(PhyPath + "\\ftpTransfer.txt"))
                {
                    CONTENUTO = File.ReadAllText(PhyPath + "\\ftpTransfer.txt");
                    //Modifico l'operatore
                    CONTENUTO = CONTENUTO.Remove(CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length, CONTENUTO.IndexOf("</Operatore>") - (CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length));
                    CONTENUTO = CONTENUTO.Insert(CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length, Contenuti["Operatore"].ToString());
                    //Modifico lo stato
                    CONTENUTO = CONTENUTO.Remove(CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length, CONTENUTO.IndexOf("</Stato>") - (CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length));
                    CONTENUTO = CONTENUTO.Insert(CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length, Contenuti["Stato"].ToString());
                    //Modifico il messaggio
                    CONTENUTO = CONTENUTO.Remove(CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length, CONTENUTO.IndexOf("</Messaggio>") - (CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length));
                    CONTENUTO = CONTENUTO.Insert(CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length, Contenuti["Messaggio"].ToString());
                    File.WriteAllText(PhyPath + "\\ftpTransfer.txt", CONTENUTO);
                }
                else
                {
                    ritorno = "File non trovato";
                }

            }
            catch (Exception error)
            {
                return error.Message.ToString();
            }
            return ritorno;

        }
        /// <summary>
        /// Modifica i dati dal file di accesso con quelli passati
        /// Ritorna stringa vuota se tutto ok altrimenti nella stringa c'è l'errore 
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <returns></returns>
        public static string ModificaFileAccesso(string PhyPath, Dictionary<string, string> Contenuti, string Nomefile)
        {
            string ritorno = "";
            try
            {
                string CONTENUTO = "";
                if (File.Exists(PhyPath + "\\" + Nomefile))
                {
                    CONTENUTO = File.ReadAllText(PhyPath + "\\" + Nomefile);
                    //Modifico l'operatore
                    CONTENUTO = CONTENUTO.Remove(CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length, CONTENUTO.IndexOf("</Operatore>") - (CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length));
                    CONTENUTO = CONTENUTO.Insert(CONTENUTO.IndexOf("<Operatore>") + "<Operatore>".Length, Contenuti["Operatore"].ToString());
                    //Modifico lo stato
                    CONTENUTO = CONTENUTO.Remove(CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length, CONTENUTO.IndexOf("</Stato>") - (CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length));
                    CONTENUTO = CONTENUTO.Insert(CONTENUTO.IndexOf("<Stato>") + "<Stato>".Length, Contenuti["Stato"].ToString());
                    //Modifico il messaggio
                    CONTENUTO = CONTENUTO.Remove(CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length, CONTENUTO.IndexOf("</Messaggio>") - (CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length));
                    CONTENUTO = CONTENUTO.Insert(CONTENUTO.IndexOf("<Messaggio>") + "<Messaggio>".Length, Contenuti["Messaggio"].ToString());
                    File.WriteAllText(PhyPath + "\\" + Nomefile, CONTENUTO);
                }
                else
                {
                    ritorno = "File non trovato";
                }

            }
            catch (Exception error)
            {
                return error.Message.ToString();
            }
            return ritorno;

        }

        /// <summary>
        /// Elimina il file di accesso a ftp
        /// Ritorna stringa vuota se tutto ok altrimenti nella stringa c'è l'errore 
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <returns></returns>
        public static string EliminaFileAccessoFtp(string PhyPath)
        {
            string ritorno = "";
            try
            {
                File.Delete(PhyPath + "\\ftpTransfer.txt");
            }
            catch (Exception error)
            {
                return error.Message.ToString();
            }
            return ritorno;
        }
        /// <summary>
        /// Elimina il file di accesso 
        /// Ritorna stringa vuota se tutto ok altrimenti nella stringa c'è l'errore 
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <returns></returns>
        public static string EliminaFileAccesso(string PhyPath, string Nomefile)
        {
            string ritorno = "";
            try
            {
                File.Delete(PhyPath + "\\" + Nomefile);
            }
            catch (Exception error)
            {
                return error.Message.ToString();
            }
            return ritorno;
        }

        /// <summary>
        /// Testa se esiste il file specificato
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <returns></returns>
        public static bool EsisteFileAccessoFtp(string PhyPath)
        {
            try
            {
                if (File.Exists(PhyPath + "\\ftpTransfer.txt"))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Testa se esiste il file specificato
        /// </summary>
        /// <param name="PhyPath"></param>
        /// <returns></returns>
        public static bool EsisteFileAccesso(string PhyPath, string Nomefile)
        {
            try
            {
                if (File.Exists(PhyPath + "\\" + Nomefile))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static string scriviFileLog(Dictionary<string, string> Contenuti)
        {
            string ritorno = "";
            try
            {
                string LogDir = physiclogdir;
                if (!System.IO.Directory.Exists(LogDir))
                    System.IO.Directory.CreateDirectory(LogDir);

                //Se il file supera 1 MB lo copio 1 volta alla seconda cancello il vecchio
                if (System.IO.File.Exists(LogDir + "\\LogGlobal.txt"))
                {
                    if (getFileLen(LogDir + "\\LogGlobal.txt") > (1 * 1048576))
                    {
                        //getFileLen(LogDir + "\\LogGlobal.txt").ToString();
                        if (System.IO.File.Exists(LogDir + "\\LogGlobal_old.txt"))
                            System.IO.File.Delete(LogDir + "\\LogGlobal_old.txt");
                        System.IO.File.Copy(LogDir + "\\LogGlobal.txt", LogDir + "\\LogGlobal_old.txt", true);
                        System.Threading.Thread.Sleep(2500);
                        System.IO.File.Delete(LogDir + "\\LogGlobal.txt");
                    }
                }
                //SCRIVIAMO IL FILE PER L'ACCESSO
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("\r\n <Data>");
                sb.Append(System.DateTime.Now.ToString());
                sb.Append("</Data>");
                sb.AppendLine();
                sb.Append("<Messaggio>");
                sb.Append(Contenuti["Messaggio"].ToString());
                sb.Append("</Messaggio>");
                File.AppendAllText(LogDir + "\\LogGlobal.txt", sb.ToString());
            }
            catch (Exception error)
            {
                ritorno = error.Message.ToString();
            }
            return ritorno;
        }
        public static string scriviFileLog(Dictionary<string, string> Contenuti, string pathFisicoLog)
        {
            string ritorno = "";
            string LogDir = pathFisicoLog;

            try
            {
                if (!System.IO.Directory.Exists(LogDir))
                    System.IO.Directory.CreateDirectory(LogDir);

                //Se il file supera 1 MB lo copio 1 volta alla seconda cancello il vecchio
                if (System.IO.File.Exists(LogDir + "\\LogGlobal.txt"))
                {
                    if (getFileLen(LogDir + "\\LogGlobal.txt") > (1 * 1048576))
                    {
                        //getFileLen(LogDir + "\\LogGlobal.txt").ToString();
                        if (System.IO.File.Exists(LogDir + "\\LogGlobal_old.txt"))
                            System.IO.File.Delete(LogDir + "\\LogGlobal_old.txt");
                        System.IO.File.Copy(LogDir + "\\LogGlobal.txt", LogDir + "\\LogGlobal_old.txt", true);
                        System.Threading.Thread.Sleep(2500);
                        System.IO.File.Delete(LogDir + "\\LogGlobal.txt");
                    }
                }

                //SCRIVIAMO IL FILE PER L'ACCESSO
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("\r\n <Data>");
                sb.Append(System.DateTime.Now.ToString());
                sb.Append("</Data>");
                sb.AppendLine();
                sb.Append("<Messaggio>");
                sb.Append(Contenuti["Messaggio"].ToString());
                sb.Append("</Messaggio>");
                File.AppendAllText(LogDir + "\\LogGlobal.txt", sb.ToString());
            }
            catch (Exception error)
            {
                ritorno = error.Message.ToString();
                try
                {
                    File.AppendAllText(LogDir + "\\LogGlobal.txt", ritorno);
                }
                catch { }
            }
            return ritorno;
        }

        public static string scriviFileLog(Dictionary<string, string> Contenuti, string pathFisicoLog, string NomeFile)
        {
            string ritorno = "";
            string LogDir = pathFisicoLog;

            try
            {
                if (!System.IO.Directory.Exists(LogDir))
                    System.IO.Directory.CreateDirectory(LogDir);

                //Se il file supera 1 MB lo copio 1 volta alla seconda cancello il vecchio
                if (System.IO.File.Exists(LogDir + "\\" + NomeFile))
                {
                    if (getFileLen(LogDir + "\\" + NomeFile) > (1 * 1048576))
                    {
                        //getFileLen(LogDir + "\\LogGlobal.txt").ToString();
                        if (System.IO.File.Exists(LogDir + "\\" + NomeFile + ".Old"))
                            System.IO.File.Delete(LogDir + "\\" + NomeFile + ".Old");
                        System.IO.File.Copy(LogDir + "\\" + NomeFile, LogDir + "\\" + NomeFile + ".Old", true);
                        System.Threading.Thread.Sleep(2500);
                        System.IO.File.Delete(LogDir + "\\" + NomeFile);
                    }
                }

                //SCRIVIAMO IL FILE PER L'ACCESSO
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("\r\n <Data>");
                sb.Append(System.DateTime.Now.ToString());
                sb.Append("</Data>");
                sb.AppendLine();
                sb.Append("<Messaggio>");
                sb.Append(Contenuti["Messaggio"].ToString());
                sb.Append("</Messaggio>");
                File.AppendAllText(LogDir + "\\" + NomeFile, sb.ToString());
            }
            catch (Exception error)
            {
                ritorno = error.Message.ToString();
                try
                {
                    File.AppendAllText(LogDir + "\\" + NomeFile, ritorno);
                }
                catch { }
            }
            return ritorno;
        }


        private static long getFileLen(string filePath)
        {
            long res = -1;
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            res = fi.Length;
            return res;
        }
    }


}
