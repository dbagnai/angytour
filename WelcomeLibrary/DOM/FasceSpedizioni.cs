using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class jsonspedizioni
    {
        public List<fascespedizioni> fascespedizioni { set; get; }
        public Dictionary<string, string> keyValuePairs { set; get; }
    }

    [Serializable()]
    public class fascespedizioni
    {
        private string _Codice;
        public string Codice
        {
            get { return _Codice; }
            set { _Codice = value; }
        }

        private string _Descrizione;
        public string Descrizione
        {
            get { return _Descrizione; }
            set { _Descrizione = value; }
        }

        private double _costo = 0;
        public double Costo
        {
            get { return _costo; }
            set { _costo = value; }
        }
        private double _pesomin = 0;
        public double PesoMin
        {
            get { return _pesomin; }
            set { _pesomin = value; }
        }
        private double _pesomax = 0;
        public double PesoMax
        {
            get { return _pesomax; }
            set { _pesomax = value; }
        }

        public fascespedizioni()
        {
            this.Codice = "";
            this.Descrizione = "";
            this.Costo = 0;
            this.PesoMax = 0;
            this.PesoMin = 0;
        }
        public fascespedizioni(fascespedizioni tmp)
        {
            this.Codice = tmp.Codice;
            this.Descrizione = tmp.Descrizione;
            this.Costo = tmp.Costo;
            this.PesoMax = tmp.PesoMax;
            this.PesoMin = tmp.PesoMin;
        }
    }
}
