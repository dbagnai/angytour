using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
     [Serializable]
    public class Prodotto
    {
        private string _CodiceTipologia;
        public string CodiceTipologia
        {
            get { return _CodiceTipologia; }
            set { _CodiceTipologia = value; }
        }
        private string _CodiceProdotto;
        public string CodiceProdotto
        {
            get { return _CodiceProdotto; }
            set { _CodiceProdotto = value; }
        }
        private string _Descrizione;
        public string Descrizione
        {
            get { return _Descrizione; }
            set { _Descrizione = value; }
        }
        private string _Lingua;
        public string Lingua
        {
            get { return _Lingua; }
            set { _Lingua = value; }
        }
        public Prodotto()
        {
            this.CodiceProdotto = "";
            this.CodiceTipologia = "";
            this.Descrizione = "";
            this.Lingua = "";
        }
        public Prodotto(Prodotto tmp)
        {
            this.CodiceProdotto = tmp.CodiceProdotto;
            this.CodiceTipologia = tmp.CodiceTipologia;
            this.Descrizione = tmp.Descrizione;
            this.Lingua = tmp.Lingua;
        }
    }
}
