using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class Tabrif
    {



        private TabrifCollection _Lingue;
        public TabrifCollection Lingue
        {
            get { return _Lingue; }
            set { _Lingue = value; }
        }


        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
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


        private string _Campo1;
        public string Campo1
        {
            get { return _Campo1; }
            set { _Campo1 = value; }
        }
        private string _Campo2;
        public string Campo2
        {
            get { return _Campo2; }
            set { _Campo2 = value; }
        }
        private string _Campo3;
        public string Campo3
        {
            get { return _Campo3; }
            set { _Campo3 = value; }
        }
        private string _Campo4;
        public string Campo4
        {
            get { return _Campo4; }
            set { _Campo4 = value; }
        }
        private string _Campo5;
        public string Campo5
        {
            get { return _Campo5; }
            set { _Campo5 = value; }
        }
        private string _Campo6;
        public string Campo6
        {
            get { return _Campo6; }
            set { _Campo6 = value; }
        }
        private string _Campo7;
        public string Campo7
        {
            get { return _Campo7; }
            set { _Campo7 = value; }
        }
        private string _Campo8;
        public string Campo8
        {
            get { return _Campo8; }
            set { _Campo8 = value; }
        }
        private string _Campo9;
        public string Campo9
        {
            get { return _Campo9; }
            set { _Campo9 = value; }
        }
        private string _Campo10;
        public string Campo10
        {
            get { return _Campo10; }
            set { _Campo10 = value; }
        }
        private bool _Bool1;
        public bool Bool1
        {
            get { return _Bool1; }
            set { _Bool1 = value; }
        }
        private bool _Bool2;
        public bool Bool2
        {
            get { return _Bool2; }
            set { _Bool2 = value; }
        }
        private DateTime _Data1;
        public DateTime Data1
        {
            get { return _Data1; }
            set { _Data1 = value; }
        }


        private long? _Intero1;
        public long? Intero1
        {
            get { return _Intero1; }
            set { _Intero1 = value; }
        }
        private long? _Intero2;
        public long? Intero2
        {
            get { return _Intero2; }
            set { _Intero2 = value; }
        }
        private long? _Intero3;
        public long? Intero3
        {
            get { return _Intero3; }
            set { _Intero3 = value; }
        }
        private double _Double1;
        public double Double1
        {
            get { return _Double1; }
            set { _Double1 = value; }
        }

        private Byte[] _Rowstamp;
        public Byte[] Rowstamp
        {
            get { return _Rowstamp; }
            set { _Rowstamp = value; }
        }

        public Tabrif(string id, string codice, string lingua, string campo1, string campo2 = "", string campo3 = "", string campo4 = "", string campo5 = "", string campo6 = "", string campo7 = "", string campo8 = "", string campo9 = "", string campo10 = "", bool bool1 = false, bool bool2 = false, string data1 = "01/01/0001", long? intero1 = null, long? intero2 = null, long? intero3 = null, double double1 = 0, TabrifCollection lingue = null)
        {
            Lingue = lingue;
            Id = id;
            Codice = codice;
            Lingua = lingua;
            Campo1 = campo1;
            Campo2 = campo2;
            Campo3 = campo3;
            Campo4 = campo4;
            Campo5 = campo5;
            Campo6 = campo6;
            Campo7 = campo7;
            Campo8 = campo8;
            Campo9 = campo9;
            Campo10 = campo10;
            Bool1 = bool1;
            Bool2 = bool2;
            Data1 = DateTime.Parse(data1);
            Intero1 = intero1;
            Intero2 = intero2;
            Intero3 = intero3;
            Double1 = double1;
            Rowstamp = (byte[])Convert.FromBase64String("");
        }

        public Tabrif()
        {
            this.Id = "";
            this.Double1 = 0;
            this.Lingua = "";
            this.Codice = "";
            this.Campo1 = "";
            this.Campo2 = "";
            this.Campo3 = "";
            this.Campo4 = "";
            this.Campo5 = "";
            this.Campo6 = "";
            this.Campo7 = "";
            this.Campo8 = "";
            this.Campo9 = "";
            this.Campo10 = "";
            this.Bool1 = false;
            this.Bool2 = false;
            this.Rowstamp = (byte[])Convert.FromBase64String("");
        }
    }
}
