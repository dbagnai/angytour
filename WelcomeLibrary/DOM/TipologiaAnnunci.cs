using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    public class TipologiaAnnunci
    {
        private string _Codice;
        public string Codice
        {
            get { return _Codice; }
            set { _Codice = value; }
        }
        private string _Lingua;
        public string Lingua
        {
            get { return _Lingua; }
            set { _Lingua = value; }
        }
        private string _Descrizione;
        public string Descrizione
        {
            get { return _Descrizione; }
            set { _Descrizione = value; }
        }
        public TipologiaAnnunci()
        {
            this.Lingua = "";
            this.Codice = "";
            this.Descrizione = "";
        }
        public TipologiaAnnunci(TipologiaAnnunci tmp)
        {
            this.Lingua = tmp.Lingua;
            this.Codice = tmp.Codice;
            this.Descrizione = tmp.Descrizione;
        }
    }
}
