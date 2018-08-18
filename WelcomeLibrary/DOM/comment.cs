using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Comment
    {
        private long _id;
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private Boolean _approvato;
        public Boolean Approvato
        {
            get { return _approvato; }
            set { _approvato = value; }
        }
        private DateTime _data;

        public DateTime Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private string _autore;

        public string Autore
        {
            get { return _autore; }
            set { _autore = value; }
        }
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _nome;

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }
        private string _spare2;
        public string Spare2
        {
            get { return _spare2; }
            set { _spare2 = value; }
        }
        private long _idpost;

        public long Idpost
        {
            get { return _idpost; }
            set { _idpost = value; }
        }

        private long _idcollegato;

        public long Idcollegato
        {
            get { return _idcollegato; }
            set { _idcollegato = value; }
        }

        private double _stelle;

        public double stelle
        {
            get { return _stelle; }
            set { _stelle = value; }
        }

        private string _lingua;
        public string Lingua
        {
            get { return _lingua; }
            set { _lingua = value; }
        }



        private string _testoI;
        public string TestoI
        {
            get { return _testoI; }
            set { _testoI = value; }
        }
        private string _titoloI;
        public string TitoloI
        {
            get { return _titoloI; }
            set { _titoloI = value; }
        }
        private string _testoGB;
        public string TestoGB
        {
            get { return _testoGB; }
            set { _testoGB = value; }
        }
        private string _titoloGB;
        public string TitoloGB
        {
            get { return _titoloGB; }
            set { _titoloGB = value; }
        }
        private string _testoRU;
        public string TestoRU
        {
            get { return _testoRU; }
            set { _testoRU = value; }
        }
        private string _titoloRU;
        public string TitoloRU
        {
            get { return _titoloRU; }
            set { _titoloRU = value; }
        }
        private string _spare1;

        public string Spare1
        {
            get { return _spare1; }
            set { _spare1 = value; }
        }
        public Comment()
        {
            this._autore = string.Empty;
            this._spare2 = string.Empty;
            this._data = DateTime.MinValue;
            this._email = string.Empty;
            this._id = 0;
            this._idpost = 0;
            this._idcollegato = 0;
            this.stelle = 0;
            this._nome = string.Empty;
            this._testoI = string.Empty;
            this._titoloI = string.Empty;
            this._testoGB = string.Empty;
            this._titoloGB = string.Empty;
            this._testoRU = string.Empty;
            this._titoloRU = string.Empty;
            this.Spare1 = string.Empty;
            this.Approvato = false;
            this.Lingua = "I";
        }
        public Comment(Comment tmp)
        {
            this._autore = tmp.Autore;
            this._spare2 = tmp.Spare2;
            this._data = tmp.Data;
            this._email = tmp.Email;
            this._id = tmp.Id;
            this.Idcollegato = tmp.Idcollegato;
            this.stelle = tmp.stelle;
            this._idpost = tmp.Idpost;
            this._nome = tmp.Nome;
            this._testoI = tmp.TestoI;
            this._titoloI = tmp.TitoloI;
            this._testoGB = tmp.TestoGB;
            this._titoloGB = tmp.TitoloGB;
            this._testoRU = tmp.TestoRU;
            this._titoloRU = tmp.TitoloRU;
            this.Spare1 = tmp.Spare1;
            this.Approvato = tmp.Approvato;
            this.Lingua = tmp.Lingua;
        }

        private string _testo;
        public string Testo
        {
            get { return TestobyLingua(this._lingua); }
        }
        private string _titolo;
        public string Titolo
        {
            get { return TitolobyLingua(this._lingua); }
        }

        public string TitolobyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.TitoloGB;
                    break;
                case "RU":
                    ret = this.TitoloRU;
                    break;
                default:
                    ret = this.TitoloI;
                    break;
            }
            return ret;
        }


        public void TitolobyLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.TitoloGB = value;
                    break;
                case "RU":
                    this.TitoloRU = value;
                    break;
                default:
                    this.TitoloI = value;
                    break;
            }
        }
        public string TestobyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.TestoGB;
                    break;
                case "RU":
                    ret = this.TestoRU;
                    break;
                default:
                    ret = this.TestoI;
                    break;
            }
            return ret;
        }
        public void TestobyLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.TestoGB = value;
                    break;
                case "RU":
                    this.TestoRU = value;
                    break;
                default:
                    this.TestoI = value;
                    break;
            }
        }

    }
}
