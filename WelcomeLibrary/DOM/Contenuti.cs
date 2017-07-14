using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class Contenuti
    {
        private Int32 _Id;
        public Int32 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private Int32 _Id_attivita;
        public Int32 Id_attivita
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
            this.TitoloRU = "";
            this.TitoloGB = "";
            this.TitoloI = "";
            this.offertaassociata = new Offerte();

            this.CustomtitleI = "";
            this.CustomdescI = "";
            this.CustomtitleGB = "";
            this.CustomdescGB = "";


        }
        public Contenuti(Contenuti tmp)
        {
            this.Id = tmp.Id;
            this.Id_attivita = tmp.Id_attivita;
            this.CodiceContenuto = tmp.CodiceContenuto;
            this.DataInserimento = tmp.DataInserimento;
            this.DescrizioneGB = tmp.DescrizioneGB;
            this.DescrizioneI = tmp.DescrizioneI;
            this.DescrizioneRU = tmp.DescrizioneRU;
            this.TitoloRU = tmp.TitoloRU;
            this.TitoloGB = tmp.TitoloGB;
            this.TitoloI = tmp.TitoloI;
            this.TitoloGB = tmp.TitoloGB;
            this.TitoloI = tmp.TitoloI;
            this.CustomtitleI = tmp.CustomtitleI;
            this.CustomdescI = tmp.CustomdescI;
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
