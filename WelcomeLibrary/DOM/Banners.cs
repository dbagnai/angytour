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
        private string _AlternateTextGB;
        public string AlternateTextGB
        {
            get { return _AlternateTextGB; }
            set { _AlternateTextGB = value; }
        }
        private string _AlternateTextRU;
        public string AlternateTextRU
        {
            get { return _AlternateTextRU; }
            set { _AlternateTextRU = value; }
        }
        private string _NavigateUrl;
        public string NavigateUrl
        {
            get { return _NavigateUrl; }
            set { _NavigateUrl = value; }
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
            this.NavigateUrlGB = tmp.NavigateUrlGB; ;


            this.AlternateTextRU = tmp.AlternateTextRU;
            this.ImageUrlRU = tmp.ImageUrlRU;
            this.NavigateUrlRU = tmp.NavigateUrlRU; ;


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
            _tmp["AlternateTextRU;"] = this.AlternateTextRU;
            _tmp["ImageUrlRU"] = this.ImageUrlRU;
            _tmp["NavigateUrlRU"] = this.NavigateUrlRU;
            _tmp["sezione"] = this.sezione;
            _tmp["DataInserimento"] = string.Format("{0:dd/MM/yyyy HH:mm:ss}", this.DataInserimento);
            return _tmp;
        }

    }
}
