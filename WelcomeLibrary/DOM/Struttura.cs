using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Struttura
    {
        private Int32 _Id_struttura;
        public Int32 Id_struttura
        {
            get { return _Id_struttura; }
            set { _Id_struttura = value; }
        }

        private string _CodTipologia;
        public string CodTipologia
        {
            get { return _CodTipologia; }
            set { _CodTipologia = value; }
        }

        private string _Lingua;
        public string Lingua
        {
            get { return _Lingua; }
            set { _Lingua = value; }
        }

        private string _RagSoc;
        public string RagSoc
        {
            get { return _RagSoc; }
            set { _RagSoc = value; }
        }

        private string _PIva;
        public string PIva
        {
            get { return _PIva; }
            set { _PIva = value; }
        }

        private string _CodiceNAZIONE;
        public string CodiceNAZIONE
        {
            get { return _CodiceNAZIONE; }
            set { _CodiceNAZIONE = value; }
        }

        private string _CodiceREGIONE;
        public string CodiceREGIONE
        {
            get { return _CodiceREGIONE; }
            set { _CodiceREGIONE = value; }
        }

        private string _CodicePROVINCIA;
        public string CodicePROVINCIA
        {
            get { return _CodicePROVINCIA; }
            set { _CodicePROVINCIA = value; }
        }

        private string _CodiceCOMUNE;
        public string CodiceCOMUNE
        {
            get { return _CodiceCOMUNE; }
            set { _CodiceCOMUNE = value; }
        }

        private string _cap;
        public string Cap
        {
            get { return _cap; }
            set { _cap = value; }
        }

        private string _Indirizzo;
        public string Indirizzo
        {
            get { return _Indirizzo; }
            set { _Indirizzo = value; }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Telefono;
        public string Telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }

        private string _Cellulare;
        public string Cellulare
        {
            get { return _Cellulare; }
            set { _Cellulare = value; }
        }

        private string _Offerta1;
        public string Offerta1
        {
            get { return _Offerta1; }
            set { _Offerta1 = value; }
        }

        private string _Offerta2;
        public string Offerta2
        {
            get { return _Offerta2; }
            set { _Offerta2 = value; }
        }

        private string _Adesione;
        public string Adesione
        {
            get { return _Adesione; }
            set { _Adesione = value; }
        }

        private string _ModPagamento;
        public string ModPagamento
        {
            get { return _ModPagamento; }
            set { _ModPagamento = value; }
        }

        private string _IPclient;
        public string IPclient
        {
            get { return _IPclient; }
            set { _IPclient = value; }
        }

        private DateTime? _DataInvioValidazione;
        public DateTime? DataInvioValidazione
        {
            get { return _DataInvioValidazione; }
            set { _DataInvioValidazione = value; }
        }

        private DateTime? _DataRicezioneValidazione;
        public DateTime? DataRicezioneValidazione
        {
            get { return _DataRicezioneValidazione; }
            set { _DataRicezioneValidazione = value; }
        }

        private bool _Validato;
        public bool Validato
        {
            get { return _Validato; }
            set { _Validato = value; }
        }

        private string _TestoFormConsensi;
        public string TestoFormConsensi
        {
            get { return _TestoFormConsensi; }
            set { _TestoFormConsensi = value; }
        }

        private bool _ConsensoPrivacy;
        public bool ConsensoPrivacy
        {
            get { return _ConsensoPrivacy; }
            set { _ConsensoPrivacy = value; }
        }

        private bool _Consenso1;
        public bool Consenso1
        {
            get { return _Consenso1; }
            set { _Consenso1 = value; }
        }

        private bool _Consenso2;
        public bool Consenso2
        {
            get { return _Consenso2; }
            set { _Consenso2 = value; }
        }

        private bool _Consenso3;
        public bool Consenso3
        {
            get { return _Consenso3; }
            set { _Consenso3 = value; }
        }

        private bool _Consenso4;
        public bool Consenso4
        {
            get { return _Consenso4; }
            set { _Consenso4 = value; }
        }

        private string _Spare1;
        public string Spare1
        {
            get { return _Spare1; }
            set { _Spare1 = value; }
        }

        private string _Spare2;
        public string Spare2
        {
            get { return _Spare2; }
            set { _Spare2 = value; }
        }

        public Struttura()
        {
            this.Cap = string.Empty;
            this.CodTipologia = string.Empty;
            this.Cellulare = string.Empty;
            this.CodiceNAZIONE = string.Empty;
            this.CodiceCOMUNE = string.Empty;
            this.CodicePROVINCIA = string.Empty;
            this.CodiceREGIONE = string.Empty;
            this.PIva = string.Empty;
            this.Consenso1 = false;
            this.Consenso2 = false;
            this.Consenso3 = false;
            this.Consenso4 = false;
            this.ConsensoPrivacy = false;
            this.DataInvioValidazione = null;
            
            this.ModPagamento = string.Empty;
            this.Adesione = string.Empty;
            this.Email = string.Empty;
            this.Offerta1 = string.Empty;
            this.Offerta2 = string.Empty;

            this.DataRicezioneValidazione = null;
            this.Id_struttura = 0;

            this.Indirizzo = string.Empty;
            this.IPclient = string.Empty;
            this.Lingua = string.Empty; ;
            this.RagSoc = string.Empty;

            this.Spare1 = string.Empty;
            this.Spare2 = string.Empty;
            this.Telefono = string.Empty;
            this.TestoFormConsensi = string.Empty;
            this.Validato = false;

        }
        public Struttura(Struttura tmp)
        {
            this.Cap = tmp.Cap;
            this.CodTipologia = tmp.CodTipologia;
            this.Cellulare = tmp.Cellulare;
            this.CodiceNAZIONE = tmp.CodiceNAZIONE;
            this.CodiceCOMUNE = tmp.CodiceCOMUNE;
            this.CodicePROVINCIA = tmp.CodicePROVINCIA;
            this.CodiceREGIONE = tmp.CodiceREGIONE;
            this.PIva = tmp.PIva;
            this.Consenso1 = tmp.Consenso1;
            this.Consenso2 = tmp.Consenso2;
            this.Consenso3 = tmp.Consenso3;
            this.Consenso4 = tmp.Consenso4;
            this.ConsensoPrivacy = tmp.ConsensoPrivacy;
            this.DataInvioValidazione = tmp.DataInvioValidazione;
            this.ModPagamento = tmp.ModPagamento;
            this.Adesione = tmp.Adesione;
            this.Email = tmp.Email;
            this.Offerta1 = tmp.Offerta1;
            this.Offerta2 = tmp.Offerta2;
            this.DataRicezioneValidazione = tmp.DataRicezioneValidazione;
            this.Id_struttura = tmp.Id_struttura;
            this.Indirizzo = tmp.Indirizzo;
            this.IPclient = tmp.IPclient;
            this.Lingua = tmp.Lingua;
            this.RagSoc = tmp.RagSoc;
            this.Spare1 = tmp.Spare1;
            this.Spare2 = tmp.Spare2;
            this.Telefono = tmp.Telefono;
            this.TestoFormConsensi = tmp.TestoFormConsensi;
            this.Validato = tmp.Validato;
        }

    }
}
