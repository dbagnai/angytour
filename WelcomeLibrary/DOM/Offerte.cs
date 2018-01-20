using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Offerte
    {





        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private long _Id_collegato;
        public long Id_collegato
        {
            get { return _Id_collegato; }
            set { _Id_collegato = value; }
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
        private DateTime _Data1;
        public DateTime Data1
        {
            get { return _Data1; }
            set { _Data1 = value; }
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


        private string _Campo1I;
        public string Campo1I
        {
            get { return _Campo1I; }
            set { _Campo1I = value; }
        }
        private string _Campo1GB;
        public string Campo1GB
        {
            get { return _Campo1GB; }
            set { _Campo1GB = value; }
        }
        private string _Campo2I;
        public string Campo2I
        {
            get { return _Campo2I; }
            set { _Campo2I = value; }
        }
        private string _Campo2GB;
        public string Campo2GB
        {
            get { return _Campo2GB; }
            set { _Campo2GB = value; }
        }



        private string _Campo1RU;
        public string Campo1RU
        {
            get { return _Campo1RU; }
            set { _Campo1RU = value; }
        }
        private string _Campo2RU;
        public string Campo2RU
        {
            get { return _Campo2RU; }
            set { _Campo2RU = value; }

        }

        private string _xmlvalue;
        public string Xmlvalue
        {
            get { return _xmlvalue; }
            set { _xmlvalue = value; }
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
        private string _DenominazioneRU;
        public string DenominazioneRU
        {
            get { return _DenominazioneRU; }
            set { _DenominazioneRU = value; }
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
        private string _DatitecniciRU;
        public string DatitecniciRU
        {
            get { return _DatitecniciRU; }
            set { _DatitecniciRU = value; }
        }
        private AllegatiCollection _FotoCollection_M;
        public AllegatiCollection FotoCollection_M
        {
            get { return _FotoCollection_M; }
            set { _FotoCollection_M = value; }
        }


        private string _Autore;
        public string Autore
        {
            get { return _Autore; }
            set { _Autore = value; }
        }



        private string _CodiceProdotto;
        public string CodiceProdotto
        {
            get { return _CodiceProdotto; }
            set { _CodiceProdotto = value; }
        }


        private string _CodiceCategoria;
        public string CodiceCategoria
        {
            get { return _CodiceCategoria; }
            set { _CodiceCategoria = value; }
        }

        private string _CodiceCategoria2Liv;
        public string CodiceCategoria2Liv
        {
            get { return _CodiceCategoria2Liv; }
            set { _CodiceCategoria2Liv = value; }
        }

        private string _CodiceOfferta;
        public string CodiceOfferta
        {
            get { return _CodiceOfferta; }
            set { _CodiceOfferta = value; }
        }
        private string _linkVideo;
        public string linkVideo
        {
            get { return _linkVideo; }
            set { _linkVideo = value; }
        }
        private double _prezzo;
        public double Prezzo
        {
            get { return _prezzo; }
            set { _prezzo = value; }
        }
        private double _prezzolistino;
        public double PrezzoListino
        {
            get { return _prezzolistino; }
            set { _prezzolistino = value; }
        }
        private bool _vetrina;
        public bool Vetrina
        {
            get { return _vetrina; }
            set { _vetrina = value; }
        }


        private double? _qta_vendita;
        public double? Qta_vendita
        {
            get { return _qta_vendita; }
            set { _qta_vendita = value; }
        }
        private bool _promozione;
        public bool Promozione
        {
            get { return _promozione; }
            set { _promozione = value; }

        }

        private bool _abilitacontatto;
        public bool Abilitacontatto
        {
            get { return _abilitacontatto; }
            set { _abilitacontatto = value; }
        }
        private bool _Archiviato;
        public bool Archiviato
        {
            get { return _Archiviato; }
            set { _Archiviato = value; }
        }

        private long _Caratteristica1;
        public long Caratteristica1
        {
            get { return _Caratteristica1; }
            set { _Caratteristica1 = value; }
        }

        private long _Caratteristica2;
        public long Caratteristica2
        {
            get { return _Caratteristica2; }
            set { _Caratteristica2 = value; }
        }


        private long _Caratteristica3;
        public long Caratteristica3
        {
            get { return _Caratteristica3; }
            set { _Caratteristica3 = value; }
        }


        private long _Caratteristica4;
        public long Caratteristica4
        {
            get { return _Caratteristica4; }
            set { _Caratteristica4 = value; }
        }


        private long _Caratteristica5;
        public long Caratteristica5
        {
            get { return _Caratteristica5; }
            set { _Caratteristica5 = value; }
        }
        private long _Caratteristica6;
        public long Caratteristica6
        {
            get { return _Caratteristica6; }
            set { _Caratteristica6 = value; }
        }

        private long _Anno;
        public long Anno
        {
            get { return _Anno; }
            set { _Anno = value; }
        }


        private long _Id_dts_collegato;
        public long Id_dts_collegato
        {
            get { return _Id_dts_collegato; }
            set { _Id_dts_collegato = value; }
        }

        private string _pivacf_dts;
        public string Pivacf_dts
        {
            get { return _pivacf_dts; }
            set { _pivacf_dts = value; }
        }

        private string _nome_dts;
        public string Nome_dts
        {
            get { return _nome_dts; }
            set { _nome_dts = value; }
        }
        private string _cognome_dts;

        public string Cognome_dts
        {
            get { return _cognome_dts; }
            set { _cognome_dts = value; }
        }
        private DateTime _datanascita_dts;

        public DateTime Datanascita_dts
        {
            get { return _datanascita_dts; }
            set { _datanascita_dts = value; }
        }
        private string _sociopresentatore1_dts;

        public string Sociopresentatore1_dts
        {
            get { return _sociopresentatore1_dts; }
            set { _sociopresentatore1_dts = value; }
        }
        private string _sociopresentatore2_dts;

        public string Sociopresentatore2_dts
        {
            get { return _sociopresentatore2_dts; }
            set { _sociopresentatore2_dts = value; }
        }
        private string _telefonoprivato_dts;

        public string Telefonoprivato_dts
        {
            get { return _telefonoprivato_dts; }
            set { _telefonoprivato_dts = value; }
        }
        private string _annolaurea_dts;

        public string Annolaurea_dts
        {
            get { return _annolaurea_dts; }
            set { _annolaurea_dts = value; }
        }
        private string _annospecializzazione_dts;

        public string Annospecializzazione_dts
        {
            get { return _annospecializzazione_dts; }
            set { _annospecializzazione_dts = value; }
        }
        private string _altrespecializzazioni_dts;

        public string Altrespecializzazioni_dts
        {
            get { return _altrespecializzazioni_dts; }
            set { _altrespecializzazioni_dts = value; }
        }
        private bool _socioSicpre_dts;

        public bool SocioSicpre_dts
        {
            get { return _socioSicpre_dts; }
            set { _socioSicpre_dts = value; }
        }
        private bool _socioIsaps_dts;

        public bool SocioIsaps_dts
        {
            get { return _socioIsaps_dts; }
            set { _socioIsaps_dts = value; }
        }
        private string _socioaltraassociazione_dts;

        public string Socioaltraassociazione_dts
        {
            get { return _socioaltraassociazione_dts; }
            set { _socioaltraassociazione_dts = value; }
        }
        private string _trattamenticollegati_dts;

        public string Trattamenticollegati_dts
        {
            get { return _trattamenticollegati_dts; }
            set { _trattamenticollegati_dts = value; }
        }
        private bool _accettazioneStatuto_dts;

        public bool AccettazioneStatuto_dts
        {
            get { return _accettazioneStatuto_dts; }
            set { _accettazioneStatuto_dts = value; }
        }
        private bool _certificazione_dts;

        public bool Certificazione_dts
        {
            get { return _certificazione_dts; }
            set { _certificazione_dts = value; }
        }
        private string _emailriservata_dts;

        public string Emailriservata_dts
        {
            get { return _emailriservata_dts; }
            set { _emailriservata_dts = value; }
        }
        private string _codiceNAZIONE1_dts;

        public string CodiceNAZIONE1_dts
        {
            get { return _codiceNAZIONE1_dts; }
            set { _codiceNAZIONE1_dts = value; }
        }
        private string _codiceREGIONE1_dts;

        public string CodiceREGIONE1_dts
        {
            get { return _codiceREGIONE1_dts; }
            set { _codiceREGIONE1_dts = value; }
        }
        private string _codicePROVINCIA1_dts;

        public string CodicePROVINCIA1_dts
        {
            get { return _codicePROVINCIA1_dts; }
            set { _codicePROVINCIA1_dts = value; }
        }
        private string _codiceCOMUNE1_dts;

        public string CodiceCOMUNE1_dts
        {
            get { return _codiceCOMUNE1_dts; }
            set { _codiceCOMUNE1_dts = value; }
        }
        private string _codiceNAZIONE2_dts;

        public string CodiceNAZIONE2_dts
        {
            get { return _codiceNAZIONE2_dts; }
            set { _codiceNAZIONE2_dts = value; }
        }

        private string _codiceREGIONE2_dts;
        public string CodiceREGIONE2_dts
        {
            get { return _codiceREGIONE2_dts; }
            set { _codiceREGIONE2_dts = value; }
        }
        private string _codicePROVINCIA2_dts;
        public string CodicePROVINCIA2_dts
        {
            get { return _codicePROVINCIA2_dts; }
            set { _codicePROVINCIA2_dts = value; }
        }
        private string _codiceCOMUNE2_dts;

        public string CodiceCOMUNE2_dts
        {
            get { return _codiceCOMUNE2_dts; }
            set { _codiceCOMUNE2_dts = value; }
        }
        private string _codiceNAZIONE3_dts;

        public string CodiceNAZIONE3_dts
        {
            get { return _codiceNAZIONE3_dts; }
            set { _codiceNAZIONE3_dts = value; }
        }
        private string _codiceREGIONE3_dts;

        public string CodiceREGIONE3_dts
        {
            get { return _codiceREGIONE3_dts; }
            set { _codiceREGIONE3_dts = value; }
        }
        private string _codicePROVINCIA3_dts;
        public string CodicePROVINCIA3_dts
        {
            get { return _codicePROVINCIA3_dts; }
            set { _codicePROVINCIA3_dts = value; }
        }
        private string _codiceCOMUNE3_dts;
        public string CodiceCOMUNE3_dts
        {
            get { return _codiceCOMUNE3_dts; }
            set { _codiceCOMUNE3_dts = value; }
        }
        private double _latitudine1_dts;

        public double Latitudine1_dts
        {
            get { return _latitudine1_dts; }
            set { _latitudine1_dts = value; }
        }
        private double _longitudine1_dts;

        public double Longitudine1_dts
        {
            get { return _longitudine1_dts; }
            set { _longitudine1_dts = value; }
        }
        private double _latitudine2_dts;

        public double Latitudine2_dts
        {
            get { return _latitudine2_dts; }
            set { _latitudine2_dts = value; }
        }
        private double _longitudine2_dts;

        public double Longitudine2_dts
        {
            get { return _longitudine2_dts; }
            set { _longitudine2_dts = value; }
        }
        private double _latitudine3_dts;

        public double Latitudine3_dts
        {
            get { return _latitudine3_dts; }
            set { _latitudine3_dts = value; }
        }
        private double _longitudine3_dts;

        public double Longitudine3_dts
        {
            get { return _longitudine3_dts; }
            set { _longitudine3_dts = value; }
        }
        private bool _bloccoaccesso_dts;

        public bool Bloccoaccesso_dts
        {
            get { return _bloccoaccesso_dts; }
            set { _bloccoaccesso_dts = value; }
        }
        private string _via1_dts;

        public string Via1_dts
        {
            get { return _via1_dts; }
            set { _via1_dts = value; }
        }
        private string _cap1_dts;

        public string Cap1_dts
        {
            get { return _cap1_dts; }
            set { _cap1_dts = value; }
        }
        private string _nomeposizione1_dts;

        public string Nomeposizione1_dts
        {
            get { return _nomeposizione1_dts; }
            set { _nomeposizione1_dts = value; }
        }
        private string _telefono1_dts;

        public string Telefono1_dts
        {
            get { return _telefono1_dts; }
            set { _telefono1_dts = value; }
        }
        private string _via2_dts;

        public string Via2_dts
        {
            get { return _via2_dts; }
            set { _via2_dts = value; }
        }
        private string _cap2_dts;

        public string Cap2_dts
        {
            get { return _cap2_dts; }
            set { _cap2_dts = value; }
        }
        private string _nomeposizione2_dts;

        public string Nomeposizione2_dts
        {
            get { return _nomeposizione2_dts; }
            set { _nomeposizione2_dts = value; }
        }
        private string _telefono2_dts;

        public string Telefono2_dts
        {
            get { return _telefono2_dts; }
            set { _telefono2_dts = value; }
        }
        private string _via3_dts;

        public string Via3_dts
        {
            get { return _via3_dts; }
            set { _via3_dts = value; }
        }
        private string _cap3_dts;

        public string Cap3_dts
        {
            get { return _cap3_dts; }
            set { _cap3_dts = value; }
        }
        private string _nomeposizione3_dts;

        public string Nomeposizione3_dts
        {
            get { return _nomeposizione3_dts; }
            set { _nomeposizione3_dts = value; }
        }
        private string _telefono3_dts;
        public string Telefono3_dts
        {
            get { return _telefono3_dts; }
            set { _telefono3_dts = value; }
        }

        private string _pagamenti_dts;
        public string Pagamenti_dts
        {
            get { return _pagamenti_dts; }
            set { _pagamenti_dts = value; }
        }

        private string _ricfatt_dts;
        public string ricfatt_dts
        {
            get { return _ricfatt_dts; }
            set { _ricfatt_dts = value; }
        }


        private string _indirizzofatt_dts;
        public string indirizzofatt_dts
        {
            get { return _indirizzofatt_dts; }
            set { _indirizzofatt_dts = value; }
        }

        private string _noteriservate_dts;
        public string noteriservate_dts
        {
            get { return _noteriservate_dts; }
            set { _noteriservate_dts = value; }
        }


        private string _niscrordine_dts;
        public string niscrordine_dts
        {
            get { return _niscrordine_dts; }
            set { _niscrordine_dts = value; }
        }
        private string _locordine_dts;
        public string locordine_dts
        {
            get { return _locordine_dts; }
            set { _locordine_dts = value; }
        }


        private string _annofrequenza_dts;
        public string annofrequenza_dts
        {
            get { return _annofrequenza_dts; }
            set { _annofrequenza_dts = value; }
        }
        private string _nomeuniversita_dts;
        public string nomeuniversita_dts
        {
            get { return _nomeuniversita_dts; }
            set { _nomeuniversita_dts = value; }
        }
        private string _dettagliuniversita_dts;
        public string dettagliuniversita_dts
        {
            get { return _dettagliuniversita_dts; }
            set { _dettagliuniversita_dts = value; }
        }
        private string _Boolfields_dts;
        public string Boolfields_dts
        {
            get { return _Boolfields_dts; }
            set { _Boolfields_dts = value; }
        }
        private string _Textfield1_dts;
        public string Textfield1_dts
        {
            get { return _Textfield1_dts; }
            set { _Textfield1_dts = value; }
        }
        private string _Interventieseguiti_dts;
        public string Interventieseguiti_dts
        {
            get { return _Interventieseguiti_dts; }
            set { _Interventieseguiti_dts = value; }
        }


        public string DenominazionebyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.DenominazioneGB;
                    break;
                case "RU":
                    ret = this.DenominazioneRU;
                    break;
                default:
                    ret = this.DenominazioneI;
                    break;
            }
            return ret;
        }

        public void DenominazionebyLingua(string Lingua, string value)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    this.DenominazioneGB = value;
                    break;
                case "RU":
                    this.DenominazioneRU = value;
                    break;
                default:
                    this.DenominazioneI = value;
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
        public string DatitecnicibyLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.DatitecniciGB;
                    break;
                case "RU":
                    ret = this.DatitecniciRU;
                    break;
                default:
                    ret = this.DatitecniciI;
                    break;
            }
            return ret;
        }
        public void DatitecnicibyLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.DatitecniciGB = value;
                    break;
                case "RU":
                    this.DatitecniciRU = value;
                    break;
                default:
                    this.DatitecniciI = value;
                    break;
            }
        }
        public string Campo1byLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.Campo1GB;
                    break;
                case "RU":
                    ret = this.Campo1RU;
                    break;
                default:
                    ret = this.Campo1I;
                    break;
            }
            return ret;
        }
        public void Campo1byLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.Campo1GB = value;
                    break;
                case "RU":
                    this.Campo1RU = value;
                    break;
                default:
                    this.Campo1I = value;
                    break;
            }
        }
        public string Campo2byLingua(string Lingua)
        {
            string ret = "";
            switch (Lingua)
            {
                case "GB":
                    ret = this.Campo2GB;
                    break;
                case "RU":
                    ret = this.Campo2RU;
                    break;
                default:
                    ret = this.Campo2I;
                    break;
            }
            return ret;
        }
        public void Campo2byLingua(string Lingua, string value)
        {
            switch (Lingua)
            {
                case "GB":
                    this.Campo2GB = value;
                    break;
                case "RU":
                    this.Campo2RU = value;
                    break;
                default:
                    this.Campo2I = value;
                    break;
            }
        }
        public Offerte()
        {
            this.Id = 0;
            this.Id_collegato = 0;
            this.Id_dts_collegato = 0;
            this._Campo1I = string.Empty;
            this._Campo1GB = string.Empty;
            this._Campo2I = string.Empty;
            this._Campo2GB = string.Empty;

            this._Campo1RU = string.Empty;
            this._Campo2RU = string.Empty;


            this.Autore = string.Empty;
            this.CodiceTipologia = "";
            this.CodiceComune = "";
            this.CodiceProvincia = "";
            this.CodiceRegione = "";
            this.CodiceOfferta = "";
            this.CodiceProdotto = "";
            this.CodiceCategoria = "";
            this.CodiceCategoria2Liv = "";
            this.DatitecniciGB = "";
            this.DatitecniciRU = "";
            this.DatitecniciI = "";
            this.DenominazioneGB = "";
            this.DenominazioneRU = "";

            this.DatitecniciRU = "";
            this.DenominazioneRU = "";


            this.DenominazioneI = "";
            this.Email = "";
            this.Fax = "";
            this.Indirizzo = "";
            this.Telefono = "";
            this.Website = "";
            this.DataInserimento = Convert.ToDateTime("01/01/1900");
            this.Data1 = Convert.ToDateTime("01/01/1900");
            this.FotoCollection_M = new AllegatiCollection();
            this.DescrizioneGB = "";
            this.DescrizioneRU = "";
            this.DescrizioneI = "";
            this.Prezzo = 0;
            this.PrezzoListino = 0;
            this.Vetrina = false;
            this.Promozione = false;
            this.Qta_vendita = null;
            this.Abilitacontatto = false;
            this.Archiviato = false;
            this.linkVideo = "";
            this.Xmlvalue = string.Empty;
            this.Caratteristica1 = 0;
            this.Caratteristica2 = 0;
            this.Caratteristica3 = 0;
            this.Caratteristica4 = 0;
            this.Caratteristica5 = 0;
            this.Caratteristica6 = 0;
            this.Anno = 0;


            this.Pivacf_dts = string.Empty;
            this.Nome_dts = string.Empty;
            this.Cognome_dts = string.Empty;
            this.Datanascita_dts = DateTime.MinValue;
            this.Sociopresentatore1_dts = string.Empty;
            this.Sociopresentatore2_dts = string.Empty;
            this.Telefonoprivato_dts = string.Empty;
            this.Annolaurea_dts = string.Empty;
            this.Annospecializzazione_dts = string.Empty;
            this.Altrespecializzazioni_dts = string.Empty;
            this.SocioSicpre_dts = false;
            this.SocioIsaps_dts = false;
            this.Socioaltraassociazione_dts = string.Empty;
            this.Trattamenticollegati_dts = string.Empty;
            this.AccettazioneStatuto_dts = false;
            this.Certificazione_dts = false;
            this.Emailriservata_dts = string.Empty;
            this.CodiceNAZIONE1_dts = string.Empty;
            this.CodiceREGIONE1_dts = string.Empty;
            this.CodicePROVINCIA1_dts = string.Empty;
            this.CodiceCOMUNE1_dts = string.Empty;
            this.CodiceNAZIONE2_dts = string.Empty;
            this.CodiceREGIONE2_dts = string.Empty;
            this.CodicePROVINCIA2_dts = string.Empty;
            this.CodiceCOMUNE2_dts = string.Empty;
            this.CodiceNAZIONE3_dts = string.Empty;
            this.CodiceREGIONE3_dts = string.Empty;
            this.CodicePROVINCIA3_dts = string.Empty;
            this.CodiceCOMUNE3_dts = string.Empty;
            this.Latitudine1_dts = 0;
            this.Longitudine1_dts = 0;
            this.Latitudine2_dts = 0;
            this.Longitudine2_dts = 0;
            this.Latitudine3_dts = 0;
            this.Longitudine3_dts = 0;
            this.Bloccoaccesso_dts = false;
            this.Via1_dts = string.Empty;
            this.Cap1_dts = string.Empty;
            this.Nomeposizione1_dts = string.Empty;
            this.Telefono1_dts = string.Empty;
            this.Via2_dts = string.Empty;
            this.Cap2_dts = string.Empty;
            this.Nomeposizione2_dts = string.Empty;
            this.Telefono2_dts = string.Empty;
            this.Via3_dts = string.Empty;
            this.Cap3_dts = string.Empty;
            this.Nomeposizione3_dts = string.Empty;
            this.Telefono3_dts = string.Empty;
            this.Pagamenti_dts = string.Empty;

            this.ricfatt_dts = string.Empty;
            this.noteriservate_dts = string.Empty;
            this.indirizzofatt_dts = string.Empty;

            this.niscrordine_dts = string.Empty;
            this.locordine_dts = string.Empty;


            this.annofrequenza_dts = string.Empty;
            this.nomeuniversita_dts = string.Empty;
            this.dettagliuniversita_dts = string.Empty;
            this.Boolfields_dts = string.Empty;
            this.Textfield1_dts = string.Empty;
            this.Interventieseguiti_dts = string.Empty;


        }

        public Offerte(Offerte tmp)
        {
            this.Id = tmp.Id;
            this.Id_collegato = tmp.Id_collegato;
            this.Id_dts_collegato = tmp.Id_dts_collegato;
            this.Campo1I = tmp.Campo1I;
            this.Campo1GB = tmp.Campo1GB;
            this.Campo1RU = tmp.Campo1RU;
            this.Campo2I = tmp.Campo2I;
            this.Campo2GB = tmp.Campo2GB;
            this.Campo2RU = tmp.Campo2RU;
            this.CodiceOfferta = tmp.CodiceOfferta;
            this.Prezzo = tmp.Prezzo;
            this.CodiceTipologia = tmp.CodiceTipologia;
            this.CodiceComune = tmp.CodiceComune;
            this.CodiceProvincia = tmp.CodiceProvincia;
            this.CodiceRegione = tmp.CodiceRegione;
            this._CodiceCategoria = tmp._CodiceCategoria;
            this._CodiceCategoria2Liv = tmp._CodiceCategoria2Liv;
            this.CodiceProdotto = tmp.CodiceProdotto;
            this.DatitecniciGB = tmp.DatitecniciGB;
            this.DatitecniciRU = tmp.DatitecniciRU;
            this.DatitecniciI = tmp.DatitecniciI;
            this.DenominazioneGB = tmp.DenominazioneGB;
            this.DenominazioneRU = tmp.DenominazioneRU;
            this.DenominazioneI = tmp.DenominazioneI;
            this.Email = tmp.Email;
            this.Fax = tmp.Fax;
            this.Indirizzo = tmp.Indirizzo;
            this.Telefono = tmp.Telefono;
            this.Website = tmp.Website;
            this.DataInserimento = tmp.DataInserimento;
            this.DataInserimento = tmp.Data1;
            this.DescrizioneGB = tmp.DescrizioneGB;
            this.DescrizioneRU = tmp.DescrizioneRU;
            this.DescrizioneI = tmp.DescrizioneI;
            this.PrezzoListino = tmp.PrezzoListino;
            this.Vetrina = tmp.Vetrina;
            this.Promozione = tmp.Promozione;
            this.Qta_vendita = tmp.Qta_vendita;
            this.Abilitacontatto = tmp.Abilitacontatto;
            this.Archiviato = tmp.Archiviato;
            this.linkVideo = tmp.linkVideo;
            this.Autore = tmp.Autore;
            this.Xmlvalue = tmp.Xmlvalue;

            this.Caratteristica1 = tmp.Caratteristica1;
            this.Caratteristica2 = tmp.Caratteristica2;
            this.Caratteristica3 = tmp.Caratteristica3;
            this.Caratteristica4 = tmp.Caratteristica4;
            this.Caratteristica5 = tmp.Caratteristica5;
            this.Caratteristica6 = tmp.Caratteristica6;
            this.Anno = tmp.Anno;

            Allegato _tmp;
            this.FotoCollection_M = new AllegatiCollection();
            foreach (Allegato tmplist in tmp.FotoCollection_M)
            {
                _tmp = new Allegato(tmplist);
                this.FotoCollection_M.Add(_tmp);
            }


            this.Pivacf_dts = tmp.Pivacf_dts;

            this.Nome_dts = tmp.Nome_dts;
            this.Cognome_dts = tmp.Cognome_dts;
            this.Datanascita_dts = tmp.Datanascita_dts;
            this.Sociopresentatore1_dts = tmp.Sociopresentatore1_dts;
            this.Sociopresentatore2_dts = tmp.Sociopresentatore2_dts;
            this.Telefonoprivato_dts = tmp.Telefonoprivato_dts;
            this.Annolaurea_dts = tmp.Annolaurea_dts;
            this.Annospecializzazione_dts = tmp.Annospecializzazione_dts;
            this.Altrespecializzazioni_dts = tmp.Altrespecializzazioni_dts;
            this.SocioSicpre_dts = tmp.SocioSicpre_dts;
            this.SocioIsaps_dts = tmp.SocioIsaps_dts;
            this.Socioaltraassociazione_dts = tmp.Socioaltraassociazione_dts;
            this.Trattamenticollegati_dts = tmp.Trattamenticollegati_dts;
            this.AccettazioneStatuto_dts = tmp.AccettazioneStatuto_dts;
            this.Certificazione_dts = tmp.Certificazione_dts;
            this.Emailriservata_dts = tmp.Emailriservata_dts;
            this.CodiceNAZIONE1_dts = tmp.CodiceNAZIONE1_dts;
            this.CodiceREGIONE1_dts = tmp.CodiceREGIONE1_dts;
            this.CodicePROVINCIA1_dts = tmp.CodicePROVINCIA1_dts;
            this.CodiceCOMUNE1_dts = tmp.CodiceCOMUNE1_dts;
            this.CodiceNAZIONE2_dts = tmp.CodiceNAZIONE2_dts;
            this.CodiceREGIONE2_dts = tmp.CodiceREGIONE2_dts;
            this.CodicePROVINCIA2_dts = tmp.CodicePROVINCIA2_dts;
            this.CodiceCOMUNE2_dts = tmp.CodiceCOMUNE2_dts;
            this.CodiceNAZIONE3_dts = tmp.CodiceNAZIONE3_dts;
            this.CodiceREGIONE3_dts = tmp.CodiceREGIONE3_dts;
            this.CodicePROVINCIA3_dts = tmp.CodicePROVINCIA3_dts;
            this.CodiceCOMUNE3_dts = tmp.CodiceCOMUNE3_dts;
            this.Latitudine1_dts = tmp.Latitudine1_dts;
            this.Longitudine1_dts = tmp.Longitudine1_dts;
            this.Latitudine2_dts = tmp.Latitudine2_dts;
            this.Longitudine2_dts = tmp.Longitudine2_dts;
            this.Latitudine3_dts = tmp.Latitudine3_dts;
            this.Longitudine3_dts = tmp.Longitudine3_dts;
            this.Bloccoaccesso_dts = tmp.Bloccoaccesso_dts;
            this.Via1_dts = tmp.Via1_dts;
            this.Cap1_dts = tmp.Cap1_dts;
            this.Nomeposizione1_dts = tmp.Nomeposizione1_dts;
            this.Telefono1_dts = tmp.Telefono1_dts;
            this.Via2_dts = tmp.Via2_dts;
            this.Cap2_dts = tmp.Cap2_dts;
            this.Nomeposizione2_dts = tmp.Nomeposizione2_dts;
            this.Telefono2_dts = tmp.Telefono2_dts;
            this.Via3_dts = tmp.Via3_dts;
            this.Cap3_dts = tmp.Cap3_dts;
            this.Nomeposizione3_dts = tmp.Nomeposizione3_dts;
            this.Telefono3_dts = tmp.Telefono3_dts;
            this.Pagamenti_dts = tmp.Pagamenti_dts;

            this.ricfatt_dts = tmp.ricfatt_dts;
            this.noteriservate_dts = tmp.noteriservate_dts;
            this.indirizzofatt_dts = tmp.indirizzofatt_dts;



            this.niscrordine_dts = tmp.niscrordine_dts;
            this.locordine_dts = tmp.locordine_dts;
            this.annofrequenza_dts = tmp.annofrequenza_dts;
            this.nomeuniversita_dts = tmp.nomeuniversita_dts;
            this.dettagliuniversita_dts = tmp.dettagliuniversita_dts;
            this.Boolfields_dts = tmp.Boolfields_dts;
            this.Textfield1_dts = tmp.Textfield1_dts;
            this.Interventieseguiti_dts = tmp.Interventieseguiti_dts;


        }
    }
}
