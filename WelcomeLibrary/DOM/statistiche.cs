using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{

    [Serializable]
    public class Statistiche
    {
        private Int32 _Id;
        public Int32 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private Int32 _Idattivita;
        public Int32 Idattivita
        {
            get { return _Idattivita; }
            set { _Idattivita = value; }
        }
        private string _TipoContatto;
        public string TipoContatto
        {
            get { return _TipoContatto; }
            set { _TipoContatto = value; }
        }
        private string _Url;
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }
        private string _Testomail;
        public string Testomail
        {
            get { return _Testomail; }
            set { _Testomail = value; }
        }
        private string _EmailDestinatario;
        public string EmailDestinatario
        {
            get { return _EmailDestinatario; }
            set { _EmailDestinatario = value; }
        }
        private string _EmailMittente;
        public string EmailMittente
        {
            get { return _EmailMittente; }
            set { _EmailMittente = value; }
        }
        private DateTime _data;
        public DateTime Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public Statistiche()
        {
            this.Id = 0;
            this.Idattivita = 0;
            this.TipoContatto = "";
            this.Url = "";
            this.EmailMittente = "";
            this.EmailDestinatario = "";
            this.Testomail = "";
            this.Data = DateTime.MinValue;

        }
        public Statistiche(Statistiche tmp)
        {
            this.Id = tmp.Id;
            this.Idattivita = tmp.Idattivita;
            this.TipoContatto = tmp.TipoContatto;
            this.Url = tmp.Url;
            this.EmailMittente = tmp.EmailMittente;
            this.EmailDestinatario = tmp.EmailDestinatario;
            this.Testomail = tmp.Testomail;
            this.Data = tmp.Data;
        }
    }



}
