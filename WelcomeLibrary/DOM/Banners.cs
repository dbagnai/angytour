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

        private string _AlternateTextFR;
        public string AlternateTextFR
        {
            get { return _AlternateTextFR; }
            set { _AlternateTextFR = value; }
        }
        private string _ImageUrlFR;
        public string ImageUrlFR
        {
            get { return _ImageUrlFR; }
            set { _ImageUrlFR = value; }
        }
        private string _NavigateUrlFR;
        public string NavigateUrlFR
        {
            get { return _NavigateUrlFR; }
            set { _NavigateUrlFR = value; }
        }



        private string _AlternateTextES;
        public string AlternateTextES
        {
            get { return _AlternateTextES; }
            set { _AlternateTextES = value; }
        }
        private string _ImageUrlES;
        public string ImageUrlES
        {
            get { return _ImageUrlES; }
            set { _ImageUrlES = value; }
        }
        private string _NavigateUrlES;
        public string NavigateUrlES
        {
            get { return _NavigateUrlES; }
            set { _NavigateUrlES = value; }
        }


        private string _AlternateTextDE;
        public string AlternateTextDE
        {
            get { return _AlternateTextDE; }
            set { _AlternateTextDE = value; }
        }
        private string _ImageUrlDE;
        public string ImageUrlDE
        {
            get { return _ImageUrlDE; }
            set { _ImageUrlDE = value; }
        }
        private string _NavigateUrlDE;
        public string NavigateUrlDE
        {
            get { return _NavigateUrlDE; }
            set { _NavigateUrlDE = value; }
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
        private string _altimgtextFR;
        public string AltimgtextFR
        {
            get { return _altimgtextFR; }
            set { _altimgtextFR = value; }
        }

        private string _altimgtextDE;
        public string AltimgtextDE
        {
            get { return _altimgtextDE; }
            set { _altimgtextDE = value; }
        }

        private string _altimgtextES;
        public string AltimgtextES
        {
            get { return _altimgtextES; }
            set { _altimgtextES = value; }
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
                case "FR":
                    ret = (!string.IsNullOrEmpty(this._altimgtextFR)) ? this._altimgtextFR : this._altimgtextI;
                    break;
                case "ES":
                    ret = (!string.IsNullOrEmpty(this._altimgtextES)) ? this._altimgtextES : this._altimgtextI;
                    break;
                case "DE":
                    ret = (!string.IsNullOrEmpty(this._altimgtextDE)) ? this._altimgtextDE : this._altimgtextI;
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
                case "FR":
                    this._altimgtextFR = value;
                    break;
                case "ES":
                    this._altimgtextES = value;
                    break;
                case "DE":
                    this._altimgtextDE = value;
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
                case "FR":
                    ret = this.AlternateTextFR;
                    break;
                case "DE":
                    ret = this.AlternateTextDE;
                    break;
                case "ES":
                    ret = this.AlternateTextES;
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
                case "FR":
                    this.AlternateTextFR = value;
                    break;
                case "ES":
                    this.AlternateTextES = value;
                    break;
                case "DE":
                    this.AlternateTextDE = value;
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
                case "FR":
                    ret = this.ImageUrlFR;
                    break;
                case "DE":
                    ret = this.ImageUrlDE;
                    break;
                case "ES":
                    ret = this.ImageUrlES;
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
                case "FR":
                    this.ImageUrlFR = value;
                    break;
                case "ES":
                    this.ImageUrlES = value;
                    break;
                case "DE":
                    this.ImageUrlDE = value;
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
                case "FR":
                    ret = this.NavigateUrlFR;
                    break;
                case "DE":
                    ret = this.NavigateUrlDE;
                    break;
                case "ES":
                    ret = this.NavigateUrlES;
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
                case "FR":
                    this.NavigateUrlFR = value;
                    break;
                case "ES":
                    this.NavigateUrlES = value;
                    break;
                case "DE":
                    this.NavigateUrlDE = value;
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

            this.AlternateTextFR = "";
            this.ImageUrlFR = "";
            this.NavigateUrlFR = "";


            this.AlternateTextDE = "";
            this.ImageUrlDE = "";
            this.NavigateUrlDE = "";

            this.AlternateTextES = "";
            this.ImageUrlES = "";
            this.NavigateUrlES = "";

            this.AltimgtextI = string.Empty;
            this.AltimgtextGB = string.Empty;
            this.AltimgtextRU = string.Empty;
            this.AltimgtextFR = string.Empty;
            this.AltimgtextDE = string.Empty;
            this.AltimgtextES = string.Empty;


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

            this.AlternateTextFR = tmp.AlternateTextFR;
            this.ImageUrlFR = tmp.ImageUrlFR;
            this.NavigateUrlFR = tmp.NavigateUrlFR;

            this.AlternateTextES = tmp.AlternateTextES;
            this.ImageUrlES = tmp.ImageUrlES;
            this.NavigateUrlES = tmp.NavigateUrlES;

            this.AlternateTextDE = tmp.AlternateTextDE;
            this.ImageUrlDE = tmp.ImageUrlDE;
            this.NavigateUrlDE = tmp.NavigateUrlDE;

            this.AltimgtextI = tmp.AltimgtextI;
            this.AltimgtextGB = tmp.AltimgtextGB;
            this.AltimgtextRU = tmp.AltimgtextRU;
            this.AltimgtextFR = tmp.AltimgtextFR;
            this.AltimgtextDE = tmp.AltimgtextDE;
            this.AltimgtextES = tmp.AltimgtextES;



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
            _tmp["sezione"] = this.sezione;
            _tmp["NavigateUrl"] = this.NavigateUrl;
            _tmp["AlternateText"] = this.AlternateText;
            _tmp["AlternateTextGB"] = this.AlternateTextGB;
            _tmp["ImageUrlGB"] = this.ImageUrlGB;
            _tmp["NavigateUrlGB"] = this.NavigateUrlGB;
            _tmp["AlternateTextRU"] = this.AlternateTextRU;
            _tmp["ImageUrlRU"] = this.ImageUrlRU;
            _tmp["NavigateUrlRU"] = this.NavigateUrlRU;
            _tmp["AlternateTextFR"] = this.AlternateTextFR;
            _tmp["ImageUrlFR"] = this.ImageUrlFR;
            _tmp["NavigateUrlFR"] = this.NavigateUrlFR;
            _tmp["AltimgtextFR"] = this.AltimgtextFR;
            _tmp["AltimgtextI"] = this.AltimgtextI;
            _tmp["AltimgtextGB"] = this.AltimgtextGB;
            _tmp["AltimgtextRU"] = this.AltimgtextRU;


            _tmp["AlternateTextDE"] = this.AlternateTextDE;
            _tmp["ImageUrlDE"] = this.ImageUrlDE;
            _tmp["NavigateUrlDE"] = this.NavigateUrlDE;
            _tmp["AltimgtextDE"] = this.AltimgtextDE;


            _tmp["AlternateTextES"] = this.AlternateTextES;
            _tmp["ImageUrlES"] = this.ImageUrlES;
            _tmp["NavigateUrlES"] = this.NavigateUrlES;
            _tmp["AltimgtextES"] = this.AltimgtextES;

            _tmp["DataInserimento"] = string.Format("{0:dd/MM/yyyy HH:mm:ss}", new object[] { this.DataInserimento });
            return _tmp;
        }

    }
}
