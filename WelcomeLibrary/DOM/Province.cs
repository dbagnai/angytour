using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class Province
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
        private string _Regione;
        public string Regione
        {
            get { return _Regione; }
            set { _Regione = value; }
        }
        public string _Provincia;
        public string Provincia
        {
            get { return _Provincia; }
            set { _Provincia = value; }
        }
        public string _SiglaProvincia;
        public string SiglaProvincia
        {
            get { return _SiglaProvincia; }
            set { _SiglaProvincia = value; }
        }

        private string _CodiceRegione;
        public string CodiceRegione
        {
            get { return _CodiceRegione; }
            set { _CodiceRegione = value; }
        }

        public string _CodiceProvincia;
        public string CodiceProvincia
        {
            get { return _CodiceProvincia; }
            set { _CodiceProvincia = value; }
        }

        public string _SiglaNazione;
        public string SiglaNazione
        {
            get { return _SiglaNazione; }
            set { _SiglaNazione = value; }
        }
        public Province()
        {
            this.SiglaNazione = "";
            this.Lingua = "";
            this.Codice = "";
            this.Regione = "";
            this.Provincia = "";
            this.CodiceProvincia = "";
            this.CodiceRegione = "";
            this.SiglaProvincia = "";

        }


        public Province(Province tmp)
        {

            this.Codice = tmp.Codice;
            this.Regione = tmp.Regione;
            this.Provincia = tmp.Provincia;
            this.CodiceRegione = tmp.CodiceRegione;
            this.CodiceProvincia = tmp.CodiceProvincia;
            this.SiglaProvincia = tmp.SiglaProvincia;

        }
    }
}
