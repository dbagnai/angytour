using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class ParametroGenerico
    {
        private string _CodiceTipologiaCollegata;
        public string CodiceTipologiaCollegata
        {
            get { return _CodiceTipologiaCollegata; }
            set { _CodiceTipologiaCollegata = value; }
        }


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


        public ParametroGenerico()
        {
            this.Lingua = "";
            this.Codice = "";
            this.CodiceTipologiaCollegata = "";
            this.Descrizione = "";

        }
        public ParametroGenerico(ParametroGenerico tmp)
        {
            this.Lingua = tmp.Lingua;
            this.Codice = tmp.Codice;
            this.CodiceTipologiaCollegata = tmp.CodiceTipologiaCollegata;
            this.Descrizione = tmp.Descrizione;

        }
    }
}
