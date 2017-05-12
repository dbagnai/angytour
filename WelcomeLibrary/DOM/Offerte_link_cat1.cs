using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    public class Offerte_cat1_link
    {
        private string _CodiceTipologia;
        public string CodiceTipologia
        {
            get { return _CodiceTipologia; }
            set { _CodiceTipologia = value; }
        }
        private string _codcat1;
        public string Codcat1
        {
            get { return _codcat1; }
            set { _codcat1 = value; }
        }
        
        public Offerte_cat1_link()
        {
            this.Codcat1="";
            this.CodiceTipologia="";
        }
        public Offerte_cat1_link(Offerte_cat1_link tmp)
        {
            this.Codcat1 = tmp.Codcat1;
            this.CodiceTipologia = tmp.CodiceTipologia;
        }
    }
}
