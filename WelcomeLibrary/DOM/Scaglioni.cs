using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Scaglioni
    {
        private long _id;
        public long id { get => _id; set => _id = value; }

        private long _id_attivita;
        public long id_attivita { get => _id_attivita; set => _id_attivita = value; }

        private long _idcoordinatore;
        public long idcoordinatore { get => _idcoordinatore; set => _idcoordinatore = value; }

        private DateTime? _datapartenza;
        public DateTime? datapartenza { get => _datapartenza; set => _datapartenza = value; }

        private long _durata;
        public long durata { get => _durata; set => _durata = value; }

        private Double _prezzo;
        public Double prezzo { get => _prezzo; set => _prezzo = value; }

        private long _nconferma;
        public long nconferma { get => _nconferma; set => _nconferma = value; }

        private long _nmax;
        public long nmax { get => _nmax; set => _nmax = value; }

        private long? _fasciaeta;
        public long? fasciaeta { get => _fasciaeta; set => _fasciaeta = value; }

        private long? _stato;
        public long? stato { get => _stato; set => _stato = value; }

        private string _codicesconto;
        public string codicesconto { get => _codicesconto; set => _codicesconto = value; }


        private string _jsonvalues;
        public string jsonvalues { get => _jsonvalues; set => _jsonvalues = value; }

        private Dictionary<string, string> _addedvalues;
        public Dictionary<string, string> addedvalues { get => _addedvalues; set => _addedvalues = value; }

        //public Dictionary<string, string> addedvalues
        //{
        //    get { return (!string.IsNullOrEmpty(this.jsonvalues)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(this.jsonvalues) : new Dictionary<string, string>(); }
        //    set { _TotaleAcconto = value; }
        //}

        public Scaglioni()
        {
            this.id = 0;
            this.id_attivita = 0;
            this.idcoordinatore = 0;
            this.durata = 0;
            this.datapartenza = null;
            this.prezzo = 0;
            this.nconferma = 0;
            this.nmax = 0;
            this.fasciaeta = 0;
            this.stato = 0;
            this.codicesconto = string.Empty;
            this.jsonvalues = string.Empty;
            this.addedvalues = new Dictionary<string, string>();

        }
        public Scaglioni(Scaglioni tmp)
        {
            this.id = tmp.id;
            this.id_attivita = tmp.id_attivita;
            this.idcoordinatore = tmp.idcoordinatore;
            this.durata = tmp.durata;
            this.datapartenza = tmp.datapartenza;
            this.prezzo = tmp.prezzo;
            this.nconferma = tmp.nconferma;
            this.nmax = tmp.nmax;
            this.fasciaeta = tmp.fasciaeta;
            this.stato = tmp.stato;
            this.codicesconto = tmp.codicesconto;
            this.jsonvalues = tmp.jsonvalues;
            this.addedvalues = tmp.addedvalues;

        }
    }
}
