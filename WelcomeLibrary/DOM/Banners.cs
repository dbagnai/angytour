using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Banners
    {
        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _ImageUrl;
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }

        private string _AlternateText;
        public string AlternateText
        {
            get { return _AlternateText; }
            set { _AlternateText = value; }
        }
        private string _NavigateUrl;
        public string NavigateUrl
        {
            get { return _NavigateUrl; }
            set { _NavigateUrl = value; }
        }

        private string _AlternateTextGB;
        public string AlternateTextGB
        {
            get { return _AlternateTextGB; }
            set { _AlternateTextGB = value; }
        }
        private string _ImageUrlGB;
        public string ImageUrlGB
        {
            get { return _ImageUrlGB; }
            set { _ImageUrlGB = value; }
        }
        private string _NavigateUrlGB;
        public string NavigateUrlGB
        {
            get { return _NavigateUrlGB; }
            set { _NavigateUrlGB = value; }
        }


        private string _AlternateTextRU;
        public string AlternateTextRU
        {
            get { return _AlternateTextRU; }
            set { _AlternateTextRU = value; }
        }
        private string _ImageUrlRU;
        public string ImageUrlRU
        {
            get { return _ImageUrlRU; }
            set { _ImageUrlRU = value; }
        }
        private string _NavigateUrlRU;
        public string NavigateUrlRU
        {
            get { return _NavigateUrlRU; }
            set { _NavigateUrlRU = value; }
        }


        private string _AlternateTextDK;
        public string AlternateTextDK
        {
            get { return _AlternateTextDK; }
            set { _AlternateTextDK = value; }
        }
        private string _ImageUrlDK;
        public string ImageUrlDK
        {
            get { return _ImageUrlDK; }
            set { _ImageUrlDK = value; }
        }
        private string _NavigateUrlDK;
        public string NavigateUrlDK
        {
            get { return _NavigateUrlDK; }
            set { _NavigateUrlDK = value; }
        }

        private string _sezione;
        public string sezione
        {
            get { return _sezione; }
            set { _sezione = value; }
        }

        private long _progressivo;
        public long progressivo
        {
            get { return _progressivo; }
            set { _progressivo = value; }
        }

        private DateTime _DataInserimento;
        public DateTime DataInserimento
        {
            get { return _DataInserimento; }
            set { _DataInserimento = value; }
        }


        private string _altimgtextI;
        public string AltimgtextI
        {
            get { return _altimgtextI; }
            set { _altimgtextI = value; }
        }
        private string _altimgtextGB;
        public string AltimgtextGB
        {
            get { return _altimgtextGB; }
            set { _altimgtextGB = value; }
        }
        private string _altimgtextRU;
        public string AltimgtextRU
        {
            get { return _altimgtextRU; }
            set { _altimgtextRU = value; }
        }
        private string _altimgtextDK;
        public string AltimgtextDK
        {
            get { return _altimgtextDK; }
            set { _altimgtextDK = value; }
        }



        public string altimgtextbyLingua(string Lingua)
        {
            string ret = "";

            switch (Lingua)
            {
                case "GB":
                    ret = (!string.IsNullOrEmpty(this._altimgtextGB)) ? this._altimgtextGB : this._altimgtextI;
                    break;
                case "RU":
                    ret = (!string.IsNullOrEmpty(this._altimgtextRU)) ? this._altimgtextRU : this._altimgtextI;
                    break;
                case "DK":
                    ret = (!string.IsNullOrEmpty(this._altimgtextDK)) ? this._altimgtextDK : this._altimgtextI;
                    break;
                default:
                    ret = this._altimgtextI;
                    break;
            }
            return ret;
        }
        public void altimgtexttbyLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this._altimgtextGB = value;
                    break;
                case "RU":
                    this._altimgtextRU = value;
                    break;
                case "DK":
                    this._altimgtextDK = value;
                    break;
                default:
                    this._altimgtextI = value;
                    break;
            }
        }


        public string AlternateTextbyLingua(string Lingua)
        {
            string ret = "";

            switch (Lingua)
            {
                case "GB":
                    ret = this.AlternateTextGB;
                    break;
                case "RU":
                    ret = this.AlternateTextRU;
                    break;
                case "DK":
                    ret = this.AlternateTextDK;
                    break;
                default:
                    ret = this._AlternateText;
                    break;
            }
            return ret;
        }
        public void AlternateTextbyLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.AlternateTextGB = value;
                    break;
                case "RU":
                    this.AlternateTextRU = value;
                    break;
                case "DK":
                    this.AlternateTextDK = value;
                    break;
                default:
                    this._AlternateText = value;
                    break;
            }
        }
        public string ImageUrlbyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.ImageUrlGB;
                    break;
                case "RU":
                    ret = this.ImageUrlRU;
                    break;
                case "DK":
                    ret = this.ImageUrlDK;
                    break;
                default:
                    ret = this.ImageUrl;
                    break;
            }
            return ret;
        }
        public void ImageUrlbyLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.ImageUrlGB = value;
                    break;
                case "RU":
                    this.ImageUrlRU = value;
                    break;
                case "DK":
                    this.ImageUrlDK = value;
                    break;
                default:
                    this.ImageUrl = value;
                    break;
            }
        }
        public string NavigateUrlbyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.NavigateUrlGB;
                    break;
                case "RU":
                    ret = this.NavigateUrlRU;
                    break;
                case "DK":
                    ret = this.NavigateUrlDK;
                    break;
                default:
                    ret = this.NavigateUrl;
                    break;
            }
            return ret;
        }
        public void NavigateUrlbyLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.NavigateUrlGB = value;
                    break;
                case "RU":
                    this.NavigateUrlRU = value;
                    break;
                case "DK":
                    this.NavigateUrlDK = value;
                    break;
                default:
                    this.NavigateUrl = value;
                    break;
            }
        }

        public Banners()
        {
            this.progressivo = 0;
            this.Id = 0;
            this.ImageUrl = "";
            this.NavigateUrl = "";
            this.AlternateText = "";
            this.AlternateTextGB = "";
            this.ImageUrlGB = "";
            this.NavigateUrlGB = "";

            this.AlternateTextRU = "";
            this.ImageUrlRU = "";
            this.NavigateUrlRU = "";

            this.AlternateTextDK = "";
            this.ImageUrlDK = "";
            this.NavigateUrlDK = "";

            this.AltimgtextI = string.Empty;
            this.AltimgtextGB = string.Empty;
            this.AltimgtextRU = string.Empty;
            this.AltimgtextDK = string.Empty;


            this.sezione = "";
            this.DataInserimento = DateTime.MinValue;

        }
        public Banners(Banners tmp)
        {
            this.Id = tmp.Id;
            this.ImageUrl = tmp.ImageUrl;
            this.NavigateUrl = tmp.NavigateUrl;
            this.AlternateText = tmp.AlternateText;

            this.AlternateTextGB = tmp.AlternateTextGB;
            this.ImageUrlGB = tmp.ImageUrlGB;
            this.NavigateUrlGB = tmp.NavigateUrlGB; 


            this.AlternateTextRU = tmp.AlternateTextRU;
            this.ImageUrlRU = tmp.ImageUrlRU;
            this.NavigateUrlRU = tmp.NavigateUrlRU;

            this.AlternateTextDK = tmp.AlternateTextDK;
            this.ImageUrlDK = tmp.ImageUrlDK;
            this.NavigateUrlDK = tmp.NavigateUrlDK; 

            this.AltimgtextI = tmp.AltimgtextI;
            this.AltimgtextGB = tmp.AltimgtextGB;
            this.AltimgtextRU = tmp.AltimgtextRU;
            this.AltimgtextDK = tmp.AltimgtextDK;



            this.DataInserimento = tmp.DataInserimento;
            this.sezione = tmp.sezione;
            this.progressivo = tmp.progressivo;
        }

        public Dictionary<string, string> GetDictionaryElements()
        {
            Dictionary<string, string> _tmp = new Dictionary<string, string>();
            _tmp["progressivo"] = this.progressivo.ToString();
            _tmp["Id"] = this.Id.ToString();
            _tmp["ImageUrl"] = this.ImageUrl;
            _tmp["NavigateUrl"] = this.NavigateUrl;
            _tmp["AlternateText"] = this.AlternateText;
            _tmp["AlternateTextGB"] = this.AlternateTextGB;
            _tmp["ImageUrlGB"] = this.ImageUrlGB;
            _tmp["NavigateUrlGB"] = this.NavigateUrlGB;
            _tmp["AlternateTextRU"] = this.AlternateTextRU;
            _tmp["ImageUrlRU"] = this.ImageUrlRU;
            _tmp["NavigateUrlRU"] = this.NavigateUrlRU;
            _tmp["AlternateTextDK"] = this.AlternateTextDK;
            _tmp["ImageUrlDK"] = this.ImageUrlDK;
            _tmp["NavigateUrlDK"] = this.NavigateUrlDK;

            _tmp["sezione"] = this.sezione;
            _tmp["AltimgtextI"] = this.AltimgtextI;
            _tmp["AltimgtextGB"] = this.AltimgtextGB;
            _tmp["AltimgtextRU"] = this.AltimgtextRU;
            _tmp["AltimgtextDK"] = this.AltimgtextDK;
            _tmp["DataInserimento"] = string.Format("{0:dd/MM/yyyy HH:mm:ss}", new object[] { this.DataInserimento });
            return _tmp;
        }

    }
}
