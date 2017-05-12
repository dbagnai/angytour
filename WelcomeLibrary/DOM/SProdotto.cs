using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
     [Serializable]
    public class SProdotto
    {
        private string _CodiceSProdotto;
        public string CodiceSProdotto
        {
            get { return _CodiceSProdotto; }
            set { _CodiceSProdotto = value; }
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
        public SProdotto()
        {
            this.CodiceProdotto = "";
            this.CodiceSProdotto = "";
            this.Descrizione = "";
            this.Lingua = "";
        }
        public SProdotto(SProdotto tmp)
        {
            this.CodiceProdotto = tmp.CodiceProdotto;
            this.CodiceSProdotto = tmp.CodiceSProdotto;
            this.Descrizione = tmp.Descrizione;
            this.Lingua = tmp.Lingua;
        }
    }
}
