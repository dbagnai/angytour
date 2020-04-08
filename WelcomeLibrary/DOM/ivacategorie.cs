using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class jsonecommerce
    {
        public List<ivacategorie> ivacategorie2liv { set; get; }

    }

    [Serializable()]
    public class ivacategorie
    {
        private string _Codice;
        public string Codice
        {
            get { return _Codice; }
            set { _Codice = value; }
        }

        //private string _Descrizione;
        //public string Descrizione
        //{
        //    get { return _Descrizione; }
        //    set { _Descrizione = value; }
        //}

        private double _ivaperc = 0;
        public double Ivaperc
        {
            get { return _ivaperc; }
            set { _ivaperc = value; }
        }
      

        public ivacategorie()
        {
            this.Codice = "";
            this.Ivaperc = 0;
        }
        public ivacategorie(ivacategorie tmp)
        {
            this.Codice = tmp.Codice;
            this.Ivaperc = tmp.Ivaperc;
        }
    }
}
