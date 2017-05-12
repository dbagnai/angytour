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

        private string _Descrizione;
        public string Descrizione
        {
            get { return _Descrizione; }
            set { _Descrizione = value; }
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
            this.Descrizione = tmp.Descrizione;
            this.File = tmp.File;
            this.NomeAnteprima = tmp.NomeAnteprima;
            this.NomeFile = tmp.NomeFile;
            this.Progressivo = tmp.Progressivo;
        }
        public Allegato()
        {
            this.Acquisito = false;
            this.Cartella = "";
            this.Descrizione = "";
            this.File = null;
            this.NomeAnteprima = "";
            this.NomeFile = "";
            this.Progressivo = 0;
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
