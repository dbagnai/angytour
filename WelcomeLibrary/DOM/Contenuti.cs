using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class Contenuti
    {
        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private long _Id_attivita;
        public long Id_attivita
        {
            get { return _Id_attivita; }
            set { _Id_attivita = value; }
        }

        private string _CodiceContenuto;
        public string CodiceContenuto
        {
            get { return _CodiceContenuto; }
            set { _CodiceContenuto = value; }
        }

        private string _customtitleI;
        public string CustomtitleI
        {
            get { return _customtitleI; }
            set { _customtitleI = value; }
        }
        private string _customtitleGB;
        public string CustomtitleGB
        {
            get { return _customtitleGB; }
            set { _customtitleGB = value; }
        }
        private string _customtitleRU;
        public string CustomtitleRU
        {
            get { return _customtitleRU; }
            set { _customtitleRU = value; }
        }

        private string _customtitleFR;
        public string CustomtitleFR
        {
            get { return _customtitleFR; }
            set { _customtitleFR = value; }
        }
        private string _customdescI;
        public string CustomdescI
        {
            get { return _customdescI; }
            set { _customdescI = value; }
        }

        private string _customdescGB;
        public string CustomdescGB
        {
            get { return _customdescGB; }
            set { _customdescGB = value; }
        }
        private string _customdescRU;
        public string CustomdescRU
        {
            get { return _customdescRU; }
            set { _customdescRU = value; }
        }

        private string _customdescFR;
        public string CustomdescFR
        {
            get { return _customdescFR; }
            set { _customdescFR = value; }
        }


        private string _TitoloI;
        public string TitoloI
        {
            get { return _TitoloI; }
            set { _TitoloI = value; }
        }
        private string _TitoloGB;
        public string TitoloGB
        {
            get { return _TitoloGB; }
            set { _TitoloGB = value; }
        }
        private string _TitoloRU;
        public string TitoloRU
        {
            get { return _TitoloRU; }
            set { _TitoloRU = value; }
        }
        private string _TitoloFR;
        public string TitoloFR
        {
            get { return _TitoloFR; }
            set { _TitoloFR = value; }
        }
        private string _DescrizioneI;
        public string DescrizioneI
        {
            get { return _DescrizioneI; }
            set { _DescrizioneI = value; }
        }
        private string _DescrizioneGB;
        public string DescrizioneGB
        {
            get { return _DescrizioneGB; }
            set { _DescrizioneGB = value; }
        }
        private string _DescrizioneRU;
        public string DescrizioneRU
        {
            get { return _DescrizioneRU; }
            set { _DescrizioneRU = value; }
        }
        private string _DescrizioneFR;
        public string DescrizioneFR
        {
            get { return _DescrizioneFR; }
            set { _DescrizioneFR = value; }
        }


        private string _CanonicalI;
        public string CanonicalI
        {
            get { return _CanonicalI; }
            set { _CanonicalI = value; }
        }
        private string _CanonicalGB;
        public string CanonicalGB
        {
            get { return _CanonicalGB; }
            set { _CanonicalGB = value; }
        }
        private string _CanonicalRU;
        public string CanonicalRU
        {
            get { return _CanonicalRU; }
            set { _CanonicalRU = value; }
        }

        private string _CanonicalFR;
        public string CanonicalFR
        {
            get { return _CanonicalFR; }
            set { _CanonicalFR = value; }
        }
        private string _robots;
        public string Robots
        {
            get { return _robots; }
            set { _robots = value; }
        }

        private DateTime _DataInserimento;
        public DateTime DataInserimento
        {
            get { return _DataInserimento; }
            set { _DataInserimento = value; }
        }

        private AllegatiCollection _FotoCollection_M;
        public AllegatiCollection FotoCollection_M
        {
            get { return _FotoCollection_M; }
            set { _FotoCollection_M = value; }
        }

        private Offerte _offertaassociata;
        public Offerte offertaassociata
        {
            get { return _offertaassociata; }
            set { _offertaassociata = value; }
        }


        public string CanonicalbyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.CanonicalGB;
                    break;
                case "RU":
                    ret = this.CanonicalRU;
                    break;
                case "FR":
                    ret = this.CanonicalFR;
                    break;
                default:
                    ret = this.CanonicalI;
                    break;
            }
            return ret;
        }
        public void CanonicalbyLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.CanonicalGB = value;
                    break;
                case "RU":
                    this.CanonicalRU = value;
                    break;
                case "FR":
                    this.CanonicalFR = value;
                    break;
                default:
                    this.CanonicalI = value;
                    break;
            }
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
                case "FR":
                    ret = this.TitoloFR;
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
                case "FR":
                    this.TitoloFR = value;
                    break;
                default:
                    this.TitoloI = value;
                    break;
            }
        }
        public string DescrizionebyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.DescrizioneGB;
                    break;
                case "RU":
                    ret = this.DescrizioneRU;
                    break;
                case "FR":
                    ret = this.DescrizioneFR;
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
                default:
                    this.DescrizioneI = value;
                    break;
            }
        }

        public Contenuti()
        {
            this.Id = 0;
            this.Id_attivita = 0;
            this.CodiceContenuto = "";
            this.DataInserimento = Convert.ToDateTime("01/01/1900");
            this.FotoCollection_M = new AllegatiCollection();
            this.DescrizioneGB = "";
            this.DescrizioneI = "";
            this.DescrizioneRU = "";
            this.DescrizioneFR = "";
            this.TitoloRU = "";
            this.TitoloGB = "";
            this.TitoloFR = "";
            this.TitoloI = "";
            this.CanonicalGB = "";
            this.CanonicalRU = "";
            this.CanonicalFR = "";
            this.CanonicalI = "";
            this.Robots = "";
            this.offertaassociata = new Offerte();

            this.CustomtitleI = "";
            this.CustomdescI = "";
            this.CustomtitleGB = "";
            this.CustomdescGB = "";
            this.CustomtitleRU = "";
            this.CustomdescRU = "";

            this.CustomtitleFR = "";
            this.CustomdescFR = "";

        }
        public Contenuti(Contenuti tmp)
        {
            this.Id = tmp.Id;
            this.Id_attivita = tmp.Id_attivita;
            this.CodiceContenuto = tmp.CodiceContenuto;
            this.DataInserimento = tmp.DataInserimento;
            this.DescrizioneGB = tmp.DescrizioneGB;
            this.DescrizioneFR = tmp.DescrizioneFR;
            this.DescrizioneI = tmp.DescrizioneI;
            this.DescrizioneRU = tmp.DescrizioneRU;
            this.TitoloRU = tmp.TitoloRU;
            this.TitoloGB = tmp.TitoloGB;
            this.TitoloFR = tmp.TitoloFR;
            this.TitoloI = tmp.TitoloI;
            this.CustomtitleI = tmp.CustomtitleI;
            this.CustomdescI = tmp.CustomdescI;

            this.CustomtitleGB = tmp.CustomtitleGB;
            this.CustomdescGB = tmp.CustomdescGB;

            this.CustomtitleRU = tmp.CustomtitleRU;
            this.CustomdescRU = tmp.CustomdescRU;

            this.CustomtitleFR = tmp.CustomtitleFR;
            this.CustomdescFR = tmp.CustomdescFR;

            this.CanonicalGB = tmp.CanonicalGB;
            this.CanonicalRU = tmp.CanonicalRU;
            this.CanonicalFR = tmp.CanonicalFR;
            this.CanonicalI = tmp.CanonicalI;
            this.Robots = tmp.Robots;

            this.offertaassociata = new Offerte(tmp.offertaassociata);

            Allegato _tmp;
            foreach (Allegato tmplist in tmp.FotoCollection_M)
            {
                _tmp = new Allegato(tmplist);
                this.FotoCollection_M.Add(_tmp);
            }
        }





    }
}
