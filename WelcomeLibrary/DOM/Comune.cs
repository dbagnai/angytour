using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class Comune
    {
        private string _CodiceComune;
        public string CodiceComune
        {
            get { return _CodiceComune; }
            set { _CodiceComune = value; }
        }
        private string _CodiceIncrocio;
        public string CodiceIncrocio
        {
            get { return _CodiceIncrocio; }
            set { _CodiceIncrocio = value; }
        }
        private string _Nome;
        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }
        public Comune()
        {
            this.CodiceIncrocio = "";
            this.CodiceComune = "";
            this.Nome = "";
        }
        public Comune(Comune tmp)
        {
            this.CodiceIncrocio = tmp.CodiceIncrocio;
            this.CodiceComune = tmp.CodiceComune;
            this.Nome = tmp.Nome;
        }
    }
}
