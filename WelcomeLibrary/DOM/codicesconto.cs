using System;
using System.Collections.Generic;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Codicesconto
    {
        long _id;
        bool _usosingolo;
        DateTime? _datascadenza;
        long? _idcliente;
        long? _idprodotto;
        string _codicifiltro;
        double? _scontonum;
        double? _scontoperc;
        string _testocodicesconto;

        public long Id { get => _id; set => _id = value; }
        public bool Usosingolo { get => _usosingolo; set => _usosingolo = value; }
        public DateTime? Datascadenza { get => _datascadenza; set => _datascadenza = value; }
        public long? Idcliente { get => _idcliente; set => _idcliente = value; }
        public long? Idprodotto { get => _idprodotto; set => _idprodotto = value; }
        public string Codicifiltro { get => _codicifiltro; set => _codicifiltro = value; }
        public double? Scontonum { get => _scontonum; set => _scontonum = value; }
        public double? Scontoperc { get => _scontoperc; set => _scontoperc = value; }
        public string Testocodicesconto { get => _testocodicesconto; set => _testocodicesconto = value; }

        public Codicesconto()
        {
            _id = 0;
            _usosingolo = false;
            _datascadenza = null;
            _idcliente = null;
            _idprodotto = null;
            _scontonum = null;
            _scontoperc = null;
            _testocodicesconto = "";
            _codicifiltro = "";
        }

        public Codicesconto(Codicesconto tmp)
        {
            _id = tmp.Id;
            _usosingolo = tmp.Usosingolo;
            _datascadenza = tmp.Datascadenza;
            _idcliente = tmp.Idcliente;
            _idprodotto = tmp.Idprodotto;
            _codicifiltro = tmp.Codicifiltro;
            _scontonum = tmp.Scontonum;
            _scontoperc = tmp.Scontoperc;
            _testocodicesconto = tmp.Testocodicesconto;
        }

    }


    [Serializable]
    public class CodicescontoList : List<Codicesconto>
    {
        private long _totrecs;

        private List<Codicesconto> _CodicescontoList;
        public CodicescontoList()
        {
            _CodicescontoList = new List<Codicesconto>();
        }
        public CodicescontoList(List<Codicesconto> list)
        {
            Codicesconto _tmp;
            foreach (Codicesconto tmp in list)
            {
                _tmp = new Codicesconto(tmp);
                this.Add(_tmp);
            }
        }
        public long Totrecs { get => _totrecs; set => _totrecs = value; }
    }

}
