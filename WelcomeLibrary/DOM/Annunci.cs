using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Annunci
    {
        private Int32 _Id;
        public Int32 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _CodiceTipologia;
        public string CodiceTipologia
        {
            get { return _CodiceTipologia; }
            set { _CodiceTipologia = value; }
        }

        private string _CodiceRegione;
        public string CodiceRegione
        {
            get { return _CodiceRegione; }
            set { _CodiceRegione = value; }
        }
        private string _CodiceProvincia;
        public string CodiceProvincia
        {
            get { return _CodiceProvincia; }
            set { _CodiceProvincia = value; }
        }
        private string _CodiceComune;
        public string CodiceComune
        {
            get { return _CodiceComune; }
            set { _CodiceComune = value; }
        }

        private DateTime _DataInserimento;
        public DateTime DataInserimento
        {
            get { return _DataInserimento; }
            set { _DataInserimento = value; }
        }
        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _Website;
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }
        private string _Telefono;
        public string Telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }

        private string _Fax;
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        private string _Indirizzo;
        public string Indirizzo
        {
            get { return _Indirizzo; }
            set { _Indirizzo = value; }
        }
        private string _DenominazioneI;
        public string DenominazioneI
        {
            get { return _DenominazioneI; }
            set { _DenominazioneI = value; }
        }
        private string _DenominazioneGB;
        public string DenominazioneGB
        {
            get { return _DenominazioneGB; }
            set { _DenominazioneGB = value; }
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
        private string _DatitecniciI;
        public string DatitecniciI
        {
            get { return _DatitecniciI; }
            set { _DatitecniciI = value; }
        }
        private string _DatitecniciGB;
        public string DatitecniciGB
        {
            get { return _DatitecniciGB; }
            set { _DatitecniciGB = value; }
        }
        private AllegatiCollection _FotoCollection_M;
        public AllegatiCollection FotoCollection_M
        {
            get { return _FotoCollection_M; }
            set { _FotoCollection_M = value; }
        }
        private string _CodiceProdotto;
        public string CodiceProdotto
        {
            get { return _CodiceProdotto; }
            set { _CodiceProdotto = value; }
        }
        private string _CodiceSottoProdotto;
        public string CodiceSottoProdotto
        {
            get { return _CodiceSottoProdotto; }
            set { _CodiceSottoProdotto = value; }
        }
        private string _CodiceOfferta;
        public string CodiceOfferta
        {
            get { return _CodiceOfferta; }
            set { _CodiceOfferta = value; }
        }
        private double _prezzo;
        public double Prezzo
        {
            get { return _prezzo; }
            set { _prezzo = value; }
        }

        private string _Parametro1;
        public string Parametro1
        {
            get { return _Parametro1; }
            set { _Parametro1 = value; }
        }
        private string _Parametro2;
        public string Parametro2
        {
            get { return _Parametro2; }
            set { _Parametro2 = value; }
        }
        private string _Parametro3;
        public string Parametro3
        {
            get { return _Parametro3; }
            set { _Parametro3 = value; }
        }
        private string _Parametro4;
        public string Parametro4
        {
            get { return _Parametro4; }
            set { _Parametro4 = value; }
        }
        private string _Anno;
        public string Anno
        {
            get { return _Anno; }
            set { _Anno = value; }
        }
        public Annunci()
        {
            this.Id = 0;
            this.CodiceTipologia = "";
            this.CodiceComune = "";
            this.CodiceProvincia = "";
            this.CodiceRegione = "";
            this.CodiceOfferta = "";
            this.CodiceProdotto = "";
            this.CodiceSottoProdotto = "";
            this.DatitecniciGB = "";
            this.DatitecniciI = "";
            this.DenominazioneGB = "";
            this.DenominazioneI = "";
            this.Email = "";
            this.Fax = "";
            this.Indirizzo = "";
            this.Telefono = "";
            this.Website = "";
            this.DataInserimento = Convert.ToDateTime("01/01/1900");
            this.FotoCollection_M = new AllegatiCollection();
            this.DescrizioneGB = "";
            this.DescrizioneI = "";
            this.Prezzo = 0;
            this.Parametro1 = "";
            this.Parametro2 = "";
            this.Parametro3 = "";
            this.Parametro4 = "";
            this.Anno = "";

        }
        public Annunci(Annunci tmp)
        {
            this.Id = tmp.Id;
            this.CodiceOfferta = tmp.CodiceOfferta;
            this.Prezzo = tmp.Prezzo;
            this.CodiceTipologia = tmp.CodiceTipologia;
            this.CodiceComune = tmp.CodiceComune;
            this.CodiceProvincia = tmp.CodiceProvincia;
            this.CodiceRegione = tmp.CodiceRegione;
            this.CodiceSottoProdotto = tmp.CodiceSottoProdotto;
            this.CodiceProdotto = tmp.CodiceProdotto;
            this.DatitecniciGB = tmp.DatitecniciGB;
            this.DatitecniciI = tmp.DatitecniciI;
            this.DenominazioneGB = tmp.DenominazioneGB;
            this.DenominazioneI = tmp.DenominazioneI;
            this.Email = tmp.Email;
            this.Fax = tmp.Fax;
            this.Indirizzo = tmp.Indirizzo;
            this.Telefono = tmp.Telefono;
            this.Website = tmp.Website;
            this.DataInserimento = Convert.ToDateTime("01/01/1900");
            this.DescrizioneGB = tmp.DescrizioneGB;
            this.DescrizioneI = tmp.DescrizioneI;
            Allegato _tmp;
            foreach (Allegato tmplist in tmp.FotoCollection_M)
            {
                _tmp = new Allegato(tmplist);
                this.FotoCollection_M.Add(_tmp);
            }
            this.Parametro1 = tmp.Parametro1;
            this.Parametro2 = tmp.Parametro2;
            this.Parametro3 = tmp.Parametro3;
            this.Parametro4 = tmp.Parametro4;
            this.Anno = tmp.Anno;
        }
    }
}
