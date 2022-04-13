using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class AllegatiCollection : List<Allegato>
    {
        private List<Allegato> _AllegatiCollection;
        public AllegatiCollection()
        {
            _AllegatiCollection = new List<Allegato>();
        }

        public List<Allegato> GetItems()
        {
            return _AllegatiCollection;
        }
        private string _Schema;
        public string Schema
        {
            get { return _Schema; }
            set { _Schema = value; }
        }

        private string _Valori;
        public string Valori
        {
            get { return _Valori; }
            set { _Valori = value; }
        }

        private string _FotoAnteprima;
        public string FotoAnteprima
        {
            get { return _FotoAnteprima; }
            set { _FotoAnteprima = value; }
        }
        private string _NomeImmobile;
        public string NomeImmobile
        {
            get { return _NomeImmobile; }
            set { _NomeImmobile = value; }
        }

        private string _DecrizioneI;
        public string DescrizioneI
        {
            get { return _DecrizioneI; }
            set { _DecrizioneI = value; }
        }

        private string _DecrizioneGB;
        public string DescrizioneGB
        {
            get { return _DecrizioneGB; }
            set { _DecrizioneGB = value; }
        }

        private string _DecrizioneRU;
        public string DescrizioneRU
        {
            get { return _DecrizioneRU; }
            set { _DecrizioneRU = value; }
        }

        private string _DecrizioneFR;
        public string DescrizioneFR
        {
            get { return _DecrizioneFR; }
            set { _DecrizioneFR = value; }
        }

        private string _DecrizioneDE;
        public string DescrizioneDE
        {
            get { return _DecrizioneDE; }
            set { _DecrizioneDE = value; }
        }
        private string _DecrizioneES;
        public string DescrizioneES
        {
            get { return _DecrizioneES; }
            set { _DecrizioneES = value; }

        }
        public string DescrizionebyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneGB)) ? this.DescrizioneGB : this.DescrizioneI;
                    break;
                case "RU":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneRU)) ? this.DescrizioneRU : this.DescrizioneI;
                    break;
                case "FR":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneFR)) ? this.DescrizioneFR : this.DescrizioneI;
                    break;
                case "DE":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneDE)) ? this.DescrizioneDE : this.DescrizioneI;
                    break;
                case "ES":
                    ret = (!string.IsNullOrEmpty(this.DescrizioneES)) ? this.DescrizioneES : this.DescrizioneI;
                    break;
                default:
                    ret = this.DescrizioneI;
                    break;
            }
            return ret;
        }
        public void DescrizionebyLingua(string Lingua, string value)
        {

            switch (Lingua)
            {
                case "GB":
                    this.DescrizioneGB = value;
                    break;
                case "RU":
                    this.DescrizioneRU = value;
                    break;
                case "FR":
                    this.DescrizioneFR = value;
                    break;
                case "DE":
                    this.DescrizioneDE = value;
                    break;
                case "ES":
                    this.DescrizioneES = value;
                    break;
                default:
                    this.DescrizioneI = value;
                    break;
            }
        }


    }
}
