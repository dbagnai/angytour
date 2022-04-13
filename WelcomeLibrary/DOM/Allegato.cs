using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Allegato : IDisposable
    {
        private int _Progressivo;
        public int Progressivo
        {
            get { return _Progressivo; }
            set { _Progressivo = value; }
        }

        private string _NomeFile;
        public string NomeFile
        {
            get { return _NomeFile; }
            set { _NomeFile = value; }
        }

        //private string _Descrizione;
        //public string Descrizione
        //{
        //    get { return _Descrizione; }
        //    set { _Descrizione = value; }
        //}
        private string _DescrizioneI;
        public string DescrizioneI
        {
            get { return _DescrizioneI; }
            set { _DescrizioneI = value; }

        }

        private string _DescrizioneGB;
        public string DescrizioneGB
        {
            get { return _DescrizioneGB; }
            set { _DescrizioneGB = value; }

        }
        private string _DescrizioneRU;
        public string DescrizioneRU
        {
            get { return _DescrizioneRU; }
            set { _DescrizioneRU = value; }

        }
        private string _DescrizioneFR;
        public string DescrizioneFR
        {
            get { return _DescrizioneFR; }
            set { _DescrizioneFR = value; }

        }

        private string _DescrizioneDE;
        public string DescrizioneDE
        {
            get { return _DescrizioneDE; }
            set { _DescrizioneDE = value; }

        }

        private string _DescrizioneES;
        public string DescrizioneES
        {
            get { return _DescrizioneES; }
            set { _DescrizioneES = value; }

        }
        private string _Cartella;
        public string Cartella
        {
            get { return _Cartella; }
            set { _Cartella = value; }
        }

        private string _NomeAnteprima;
        public string NomeAnteprima
        {
            get { return _NomeAnteprima; }
            set { _NomeAnteprima = value; }
        }

        private bool _Acquisito;
        public bool Acquisito
        {
            get { return _Acquisito; }
            set { _Acquisito = value; }
        }

        private System.IO.Stream _File;
        public System.IO.Stream File
        {
            get { return _File; }
            set { _File = value; }
        }

        public Allegato(Allegato tmp)
        {
            this.Acquisito = tmp.Acquisito;
            this.Cartella = tmp.Cartella;
            //this.Descrizione = tmp.Descrizione;
            this.DescrizioneI = tmp.DescrizioneI;
            this.DescrizioneGB = tmp.DescrizioneGB;
            this.DescrizioneRU = tmp.DescrizioneRU;
            this.DescrizioneFR = tmp.DescrizioneFR;
            this.DescrizioneDE = tmp.DescrizioneDE;
            this.DescrizioneES = tmp.DescrizioneES;
            this.File = tmp.File;
            this.NomeAnteprima = tmp.NomeAnteprima;
            this.NomeFile = tmp.NomeFile;
            this.Progressivo = tmp.Progressivo;
        }
        public Allegato()
        {
            this.Acquisito = false;
            this.Cartella = "";
            //this.Descrizione = "";
            this.DescrizioneI = string.Empty;
            this.DescrizioneGB = string.Empty;
            this.DescrizioneRU = string.Empty;
            this.DescrizioneFR = string.Empty;
            this.DescrizioneDE = string.Empty;
            this.DescrizioneES = string.Empty;
            this.File = null;
            this.NomeAnteprima = "";
            this.NomeFile = "";
            this.Progressivo = 1;
        }

        public string DescrizionebyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneGB)) ? this.DescrizioneGB : this.DescrizioneI;
                    break;
                case "RU":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneRU)) ? this.DescrizioneRU : this.DescrizioneI;
                    break;
                case "FR":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneFR)) ? this.DescrizioneFR : this.DescrizioneI;
                    break;
                case "DE":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneDE)) ? this.DescrizioneDE : this.DescrizioneI;
                    break;
                case "ES":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneES)) ? this.DescrizioneES : this.DescrizioneI;
                    break;
                default:
                    ret = this.DescrizioneI;
                    break;
            }
            return ret;
        }
        public void DescrizionebyLingua(string Lingua, string value)
        {

            switch (Lingua)
            {
                case "GB":
                    this.DescrizioneGB = value;
                    break;
                case "RU":
                    this.DescrizioneRU = value;
                    break;
                case "FR":
                    this.DescrizioneFR = value;
                    break;
                case "DE":
                    this.DescrizioneDE = value;
                    break;
                case "ES":
                    this.DescrizioneES = value;
                    break;
                default:
                    this.DescrizioneI = value;
                    break;
            }
        }

        #region IDisposable Membri di

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).

            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
        }

        // Use C# destructor syntax for finalization code.
        ~Allegato()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion

    }
}
