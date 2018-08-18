using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Mail
    {
         private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private long _Id_card;
        public long Id_card
        {
            get { return _Id_card; }
            set { _Id_card = value; }
        }
         private long _Id_cliente;
        public long Id_cliente
        {
            get { return _Id_cliente; }
            set { _Id_cliente = value; }
        }
        private long _Id_mailing_struttura;
        public long Id_mailing_struttura
        {
            get { return _Id_mailing_struttura; }
            set { _Id_mailing_struttura = value; }
        }
            private long _tipomailing;
        public long Tipomailing
        {
            get { return _tipomailing; }
            set { _tipomailing = value; }
        }
           private DateTime _DataInserimento;
        public DateTime DataInserimento
        {
            get { return _DataInserimento; }
            set { _DataInserimento = value; }
        }
        private DateTime? _DataInvio;
        public DateTime? DataInvio
        {
            get { return _DataInvio; }
            set { _DataInvio = value; }
        }
        private DateTime? _DataAdesione;
        public DateTime? DataAdesione
        {
            get { return _DataAdesione; }
            set { _DataAdesione = value; }
        }
        private string _TestoMail;
        public string TestoMail
        {
            get { return _TestoMail; }
            set { _TestoMail = value; }
        }
         private string _SoggettoMail;
        public string SoggettoMail
        {
            get { return _SoggettoMail; }
            set { _SoggettoMail = value; }
        }
          private string _NoteInvio;
        public string NoteInvio
        {
            get { return _NoteInvio; }
            set { _NoteInvio = value; }
        }
           private string _Lingua;
        public string Lingua
        {
            get { return _Lingua; }
            set { _Lingua = value; }
        }
           
           private bool _Errore;
        public bool Errore
        {
            get { return _Errore; }
            set { _Errore = value; }
        }
        
           private string _testoErrore;
        public string TestoErrore
        {
            get { return _testoErrore; }
            set { _testoErrore = value; }
        }
        private Cliente _Cliente;
        public Cliente Cliente
        {
            get { return _Cliente; }
            set { _Cliente = value; }
        }

        public Dictionary<string, string> Emailaddress { get => _emailaddress; set => _emailaddress = value; }
        public Dictionary<string, string> Sparedict { get => _sparedict; set => _sparedict = value; }

        private Dictionary<string, string> _emailaddress = new Dictionary<string, string>();
        private Dictionary<string, string> _sparedict = new Dictionary<string, string>();

        public Mail()
        {
            this.Cliente = new Cliente();
            this.Id = 0;
            this.Id_card = 0;
            this.Id_cliente = 0;
            this.Id_mailing_struttura = 0;
            this.Lingua = "";
            this.NoteInvio = "";
            this.SoggettoMail = "";
            this.TestoErrore = "";
            this.TestoMail = "";
            this.Tipomailing = 0;
            this.Errore = false;
            this.DataInserimento = DateTime.MinValue;
            this.DataInvio = null;
            this.DataAdesione = null;
            this._emailaddress = new Dictionary<string, string>();
            this._sparedict = new Dictionary<string, string>();

        }
        public Mail(Mail tmp)
        {
            this.Cliente = new Cliente(tmp.Cliente);
            this.Id = tmp.Id;
            this.Id_card = tmp.Id_card;
            this.Id_cliente = tmp.Id_cliente;
            this.Id_mailing_struttura = tmp.Id_mailing_struttura;
            this.Lingua = tmp.Lingua;
            this.NoteInvio = tmp.NoteInvio;
            this.SoggettoMail = tmp.SoggettoMail;
            this.TestoErrore = tmp.TestoErrore;
            this.TestoMail = tmp.TestoMail;
            this.Tipomailing = tmp.Tipomailing;
            this.Errore = tmp.Errore;
            this.DataInserimento = tmp.DataInserimento;
            this.DataInvio = tmp.DataInvio;
            this.DataAdesione = tmp.DataAdesione;
            this._emailaddress = tmp._emailaddress;
            this._sparedict = tmp._sparedict;
        }
    }
}
