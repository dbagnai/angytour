using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Cliente
    {

        private long _Id_cliente;
        public long Id_cliente
        {
            get { return _Id_cliente; }
            set { _Id_cliente = value; }
        }
        private long _Id_card;
        public long Id_card
        {
            get { return _Id_card; }
            set { _Id_card = value; }
        }

        private string _CodiceCard;
        public string CodiceCard
        {
            get { return _CodiceCard; }
            set { _CodiceCard = value; }
        }

        private string _Lingua;
        public string Lingua
        {
            get { return _Lingua; }
            set { _Lingua = value; }
        }
        private string _Nome;
        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }
        private string _Cognome;
        public string Cognome
        {
            get { return _Cognome; }
            set { _Cognome = value; }
        }  
        private string _Sesso;
        public string Sesso
        {
            get { return _Sesso; }
            set { _Sesso = value; }
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
        private string _Professione;
        public string Professione
        {
            get { return _Professione; }
            set { _Professione = value; }
        }

        private DateTime _DataNascita;
        public DateTime DataNascita
        {
            get { return _DataNascita; }
            set { _DataNascita = value; }
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
        private string _Spare3;
        public string Spare3
        {
            get { return _Spare3; }
            set { _Spare3 = value; }
        }
        private Card _card;
        public Card card
        {
            get { return _card; }
            set { _card = value; }
        }
        private string _id_tipi_clienti;
        public string id_tipi_clienti
        {
            get { return _id_tipi_clienti; }
            set { _id_tipi_clienti = value; }
        }
        private string _Pivacf;
        public string Pivacf
        {
            get { return _Pivacf; }
            set { _Pivacf = value; }
        }
        private string _Codicisconto;
        public string Codicisconto
        {
            get { return _Codicisconto; }
            set { _Codicisconto = value; }
        }
        private string _Serialized;
        public string Serialized
        {
            get { return _Serialized; }
            set { _Serialized = value; }
        }
        public Cliente()
        {
            this.card = new Card();
            this.Cap = string.Empty;
            this.CodiceCard = string.Empty;
            this.Cellulare = string.Empty;
            this.CodiceNAZIONE = string.Empty;
            this.CodiceCOMUNE = string.Empty;
            this.CodicePROVINCIA = string.Empty;
            this.CodiceREGIONE = string.Empty;
            this.Cognome = string.Empty;
            this.Email = string.Empty;
            this.Consenso1 = false;
            this.Consenso2 = false;
            this.Consenso3 = false;
            this.Consenso4 = false;
            this.ConsensoPrivacy = false;
            this.DataInvioValidazione = null;
            this.DataNascita = DateTime.MinValue;
            this.DataRicezioneValidazione = null;
            this.Id_card = 0;
            this.Id_cliente = 0;
            this.Indirizzo = string.Empty;
            this.IPclient= string.Empty;
            this.Lingua=  "I";
            this.Nome=  string.Empty;
            this.Professione = string.Empty;
            this.Spare1 = string.Empty;
            this.Spare2 = string.Empty;
            this.Spare3 = string.Empty;
            this.Sesso = string.Empty;
            this.Telefono = string.Empty;
            this.TestoFormConsensi = string.Empty;
            this.Validato = false;
            this.Pivacf = string.Empty;
            this.id_tipi_clienti = "0";
            this.Codicisconto = string.Empty;
            this.Serialized = string.Empty;
           
        }
        public Cliente(Cliente tmp)
        {
            this.card = new Card(tmp.card);
            this.Cap = tmp.Cap;
            this.Email = tmp.Email;
            this.Cellulare = tmp.Cellulare;
            this.CodiceNAZIONE = tmp.CodiceNAZIONE;
            this.CodiceCOMUNE =tmp.CodiceCOMUNE;
            this.CodicePROVINCIA = tmp.CodicePROVINCIA;
            this.CodiceREGIONE = tmp.CodiceREGIONE;
            this.Sesso = tmp.Sesso;
            this.Cognome = tmp.Cognome;
            this.Consenso1 = tmp.Consenso1;
            this.Consenso2 = tmp.Consenso2;
            this.Consenso3 = tmp.Consenso3;
            this.Consenso4 = tmp.Consenso4;
            this.ConsensoPrivacy = tmp.ConsensoPrivacy;
            this.DataInvioValidazione = tmp.DataInvioValidazione;
            this.DataNascita = tmp.DataNascita;
            this.DataRicezioneValidazione = tmp.DataRicezioneValidazione;
            this.Id_card = tmp.Id_card;
            this.Id_cliente = tmp.Id_cliente;
            this.Indirizzo = tmp.Indirizzo;
            this.IPclient = tmp.IPclient;
            this.Lingua = tmp.Lingua;
            this.Nome = tmp.Nome;
            this.Professione = tmp.Professione;
            this.Spare1 = tmp.Spare1;
            this.Spare3 = tmp.Spare3;
            this.Spare2 = tmp.Spare2;
            this.Telefono = tmp.Telefono;
            this.TestoFormConsensi = tmp.TestoFormConsensi;
            this.Validato = tmp.Validato;
            this.CodiceCard = tmp.CodiceCard;
            this.Pivacf = tmp.Pivacf;
            this.Codicisconto = tmp.Codicisconto;
            this.id_tipi_clienti = tmp.id_tipi_clienti;
            this.Serialized = tmp.Serialized;

        }
    }
}
