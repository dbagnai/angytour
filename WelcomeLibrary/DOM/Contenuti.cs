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

        private string _customtitleDK;
        public string CustomtitleDK
        {
            get { return _customtitleDK; }
            set { _customtitleDK = value; }
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

        private string _customdescDK;
        public string CustomdescDK
        {
            get { return _customdescDK; }
            set { _customdescDK = value; }
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
        private string _TitoloDK;
        public string TitoloDK
        {
            get { return _TitoloDK; }
            set { _TitoloDK = value; }
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
        private string _DescrizioneDK;
        public string DescrizioneDK
        {
            get { return _DescrizioneDK; }
            set { _DescrizioneDK = value; }
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

        private string _CanonicalDK;
        public string CanonicalDK
        {
            get { return _CanonicalDK; }
            set { _CanonicalDK = value; }
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
                case "DK":
                    ret = this.CanonicalDK;
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
                case "DK":
                    this.CanonicalDK = value;
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
                case "DK":
                    ret = this.TitoloDK;
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
                case "DK":
                    this.TitoloDK = value;
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
                case "DK":
                    ret = this.DescrizioneDK;
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
                case "DK":
                    this.DescrizioneDK = value;
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
            this.DescrizioneDK = "";
            this.TitoloRU = "";
            this.TitoloGB = "";
            this.TitoloDK = "";
            this.TitoloI = "";
            this.CanonicalGB = "";
            this.CanonicalRU = "";
            this.CanonicalDK = "";
            this.CanonicalI = "";
            this.Robots = "";
            this.offertaassociata = new Offerte();

            this.CustomtitleI = "";
            this.CustomdescI = "";
            this.CustomtitleGB = "";
            this.CustomdescGB = "";
            this.CustomtitleRU = "";
            this.CustomdescRU = "";

            this.CustomtitleDK = "";
            this.CustomdescDK = "";

        }
        public Contenuti(Contenuti tmp)
        {
            this.Id = tmp.Id;
            this.Id_attivita = tmp.Id_attivita;
            this.CodiceContenuto = tmp.CodiceContenuto;
            this.DataInserimento = tmp.DataInserimento;
            this.DescrizioneGB = tmp.DescrizioneGB;
            this.DescrizioneDK = tmp.DescrizioneDK;
            this.DescrizioneI = tmp.DescrizioneI;
            this.DescrizioneRU = tmp.DescrizioneRU;
            this.TitoloRU = tmp.TitoloRU;
            this.TitoloGB = tmp.TitoloGB;
            this.TitoloDK = tmp.TitoloDK;
            this.TitoloI = tmp.TitoloI;
            this.CustomtitleI = tmp.CustomtitleI;
            this.CustomdescI = tmp.CustomdescI;

            this.CustomtitleGB = tmp.CustomtitleGB;
            this.CustomdescGB = tmp.CustomdescGB;

            this.CustomtitleRU = tmp.CustomtitleRU;
            this.CustomdescRU = tmp.CustomdescRU;

            this.CustomtitleDK = tmp.CustomtitleDK;
            this.CustomdescDK = tmp.CustomdescDK;

            this.CanonicalGB = tmp.CanonicalGB;
            this.CanonicalRU = tmp.CanonicalRU;
            this.CanonicalDK = tmp.CanonicalDK;
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
