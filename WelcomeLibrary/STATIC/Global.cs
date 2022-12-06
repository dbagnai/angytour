using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.STATIC
{
    public static class Global
    {
        private static string _NomeConnessioneDb;
        public static string NomeConnessioneDb
        {
            get { return _NomeConnessioneDb; }
            set { _NomeConnessioneDb = value; }
        }



        private static Dictionary<string, Dictionary<string, string>> _viewportw = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> Viewportw
        {
            get { return _viewportw; }
            set { _viewportw = value; }
        }

        private static bool _updateUrl = false;
        public static bool UpdateUrl
        {
            get { return _updateUrl; }
            set { _updateUrl = value; }
        }
        private static string _PercorsoContenuti;
        public static string PercorsoContenuti
        {
            get { return _PercorsoContenuti; }
            set { _PercorsoContenuti = value; }
        }
        private static string _PercorsoFiscoContenuti;
        public static string PercorsoFiscoContenuti
        {
            get { return _PercorsoFiscoContenuti; }
            set { _PercorsoFiscoContenuti = value; }
        }

        private static string _PercorsoComune;
        public static string PercorsoComune
        {
            get { return _PercorsoComune; }
            set { _PercorsoComune = value; }
        }
        private static string _PercorsoFisicoComune;
        public static string percorsoFisicoComune
        {
            get { return _PercorsoFisicoComune; }
            set { _PercorsoFisicoComune = value; }
        }
        //private static string _percorsobaseapplicazione = string.Empty; //sarebbe da attivare previa verifica 
        private static string _percorsobaseapplicazione;
        public static string percorsobaseapplicazione
        {
            get { return _percorsobaseapplicazione; }
            set { _percorsobaseapplicazione = value; }
        }
        private static string _percorsofisicoapplicazione;
        public static string percorsofisicoapplicazione
        {
            get { return _percorsofisicoapplicazione; }
            set { _percorsofisicoapplicazione = value; }
        }

 
        private static string _percorsoapp;
        public static string percorsoapp
        {
            get { return _percorsoapp; }
            set { _percorsoapp = value; }
        }
        private static string _percorsocdn;
        public static string percorsocdn
        {
            get { return _percorsocdn; }
            set { _percorsocdn = value; }
        }
        private static string _percorsoimg;
        public static string percorsoimg
        {
            get { return _percorsoimg; }
            set { _percorsoimg = value; }
        }

        private static bool _usecdn;
        public static bool usecdn
        {
            get { return _usecdn; }
            set { _usecdn = value; }
        }

        private static string _versionforcache;
        public static string versionforcache
        {
            get { return _versionforcache; }
            set { _versionforcache = value; }
        }


        private static string _percorsoexp;
        public static string percorsoexp
        {
            get { return _percorsoexp; }
            set { _percorsoexp = value; }
        }

        private static bool _statomailing;
        public static bool statomailing
        {
            get { return _statomailing; }
            set { _statomailing = value; }
        }

        //Codice sblocco da mettere nel web config
        //    <!--VERSIONE COMPLETA-->
        //<!--<add key="trialkey" value="sd5hge4s!ye63!jnssjsjs99aqws553hhhd6ssnnd6wwkc.dee"/>-->

        //Chiave di sblocco hashed (la chiave in chiaro è quella sopra - questa è quella hashed usata nel confronto)
        private static string _codicesblocco = "Gwu28geyn3yg44kVMgqEvw==";
        public static string Codicesblocco
        {
            get { return _codicesblocco; }
            set { _codicesblocco = value; }
        }
        /// <summary>
        /// Data di partenza della modalità trial
        /// </summary>
        private static DateTime _datastartrial = DateTime.Parse("10/10/2009");
        public static DateTime Datastartrial
        {
            get { return _datastartrial; }
            set { _datastartrial = value; }
        }

        private static bool _trial = false;
        public static bool Trial
        {
            get { return _trial; }
            set { _trial = value; }
        }
        //Attesa per il caricamento di ogni pagina in modo trial
        //private static int _millisecondsleeptimefortrial = Convert.ToInt32(((TimeSpan)(DateTime.Now - Datastartrial)).TotalDays * 2000);
        private static int _millisecondsleeptimefortrial = 5000;
        public static int Millisecondsleeptimefortrial
        {
            get { return Global._millisecondsleeptimefortrial; }
            set { Global._millisecondsleeptimefortrial = value; }
        }

        /// <summary>
        /// Testa se scaduto il periodo di prova o se mancano meno di 20 gg alla scandenza
        /// nei due casi manda il messaggio di avvio opportuno
        /// </summary>
        /// <returns></returns>
        public static string TestTrial()
        {
            string ritorno = "";
            if (WelcomeLibrary.STATIC.Global.Trial)
            {
                if ((WelcomeLibrary.STATIC.Global.Datastartrial - DateTime.Now).TotalDays > 0)
                {
                    if (((TimeSpan)(WelcomeLibrary.STATIC.Global.Datastartrial - DateTime.Now)).TotalDays < 20)
                    {
                        int giornirimasti = Convert.ToInt32(((TimeSpan)(WelcomeLibrary.STATIC.Global.Datastartrial - DateTime.Now)).TotalDays);
                        ritorno = "Attenzione rimangono " + giornirimasti.ToString() + " giorni alla scadenza del periodo di prova del portale. Superato tale termine il portale continuerà ad operare ma con funzionalità e velocità ridotte.";
                    }
                }
                else
                    ritorno = "Attenzione è scaduto il tempo di prova per il sito web, il sistema opera correntemente con funzionalità e velocità ridotte.<br/>Contattare l'assistenza per la regolarizzazione dell'acquisto del prodotto.";
                //Elimino il messaggio (COmmentare per avere il messaggio )
                ritorno = "-";
            }



            return ritorno;
        }


        public static string MD5GenerateHashWithSecret(string inputString)
        {
            string secretString = "";
            return MD5GenerateHash(inputString + secretString);
        }

        /// <summary>
        /// Controlla la corrispondenza della chiave con la sua versione hashed
        /// impostata nella classe Global
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="base64Hash"></param>
        /// <returns></returns>
        public static bool MD5CheckHashWithSecret(string inputString, string base64Hash)
        {
            return MD5GenerateHashWithSecret(inputString) == base64Hash;
        }


        /// <summary>
        /// Compute MD5 hash of string 
        /// and return a string encoded base64
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string MD5GenerateHash(string inputString)
        {
            System.Security.Cryptography.MD5 hash = System.Security.Cryptography.MD5.Create();
            Byte[] inputBytes = ASCIIEncoding.Default.GetBytes(inputString);
            Byte[] outputBytes = hash.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }



    }
}
