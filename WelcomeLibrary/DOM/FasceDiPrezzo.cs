using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

      [Serializable()]
    public class Fascediprezzo
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
        private double _PrezzoMin = double.MinValue;
        public double PrezzoMin
        {
            get { return _PrezzoMin; }
            set { _PrezzoMin = value; }
        }
        private double _PrezzoMax = double.MaxValue;
        public double PrezzoMax
        {
            get { return _PrezzoMax; }
            set { _PrezzoMax = value; }
        }

        public Fascediprezzo()
        {
            this.Lingua = "";
            this.Codice = "";
            this.CodiceTipologiaCollegata = "";
            this.Descrizione = "";
            this.PrezzoMin = double.MinValue;
            this.PrezzoMax = double.MaxValue;
        }
        public Fascediprezzo(Fascediprezzo tmp)
        {
            this.Lingua = tmp.Lingua;
            this.Codice = tmp.Codice;
            this.CodiceTipologiaCollegata = tmp.CodiceTipologiaCollegata;
            this.Descrizione = tmp.Descrizione;
            this.PrezzoMin = tmp.PrezzoMin;
            this.PrezzoMax = tmp.PrezzoMax;
        }
    }
}
