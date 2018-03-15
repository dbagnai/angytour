using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Eventi
    {

        long _idevento;
        long _idattivita;
        string _idvincolo;
        long _idcliente;
        DateTime _startdate;
        DateTime _enddate;
        DateTime _datainserimento;
        double _prezzo;
        string _testoevento;
        string _codicerichiesta;
        long _status;
        string _soggetto;
        string _textfield1;
        string _textfield2;
        string _textfield3;
        string _jsonfield1;

        public Eventi(long idevento, long idattivita, string idvincolo, long idcliente, DateTime startdate, DateTime enddate, DateTime datainserimento, double prezzo, string testoevento, string Codicerichiesta, long status, string soggetto, string textfield1 = "", string textfield2 = "", string textfield3 = "", string jsonfield1 = "")
        {
            _idevento = idevento;
            _idattivita = idattivita;
            _idvincolo = idvincolo;
            _idcliente = idcliente;
            _startdate = startdate;
            _enddate = enddate;
            _datainserimento = datainserimento;
            _prezzo = prezzo;
            _testoevento = testoevento;
            _codicerichiesta = Codicerichiesta;
            _status = status;
            _soggetto = soggetto;
            _textfield1 = textfield1;
            _textfield2 = textfield2;
            _textfield3 = textfield3;
            _jsonfield1 = jsonfield1;
        }
        public Eventi(Eventi tmp)
        {
            _idevento = tmp.Idevento;
            _idattivita = tmp.Idattivita;
            _idvincolo = tmp.Idvincolo;
            _idcliente = tmp.Idcliente;
            _startdate = tmp.Startdate;
            _enddate = tmp.Enddate;
            _datainserimento = tmp.Datainserimento;
            _prezzo = tmp.Prezzo; ;
            _testoevento = tmp.Testoevento;
            _codicerichiesta = tmp.Codicerichiesta;
            _status = tmp.Status;
            _soggetto = tmp.Soggetto;
            _textfield1 = tmp.Textfield1;
            _textfield2 = tmp.Textfield2;
            _textfield3 = tmp.Textfield3;
            _jsonfield1 = tmp.Jsonfield1;
        }
        public Eventi()
        {
            _idevento = 0;
            _idattivita = 0;
            _idcliente = 0;
            _idvincolo = string.Empty;
            _startdate = DateTime.MinValue;
            _enddate = DateTime.MinValue;
            _datainserimento = DateTime.Now;
            _prezzo = 0;
            _testoevento = string.Empty;
            _codicerichiesta = string.Empty;
            _status = 0;
            _soggetto = string.Empty;
            _textfield1 = string.Empty;
            _textfield2 = string.Empty;
            _textfield3 = string.Empty;
            _jsonfield1 = string.Empty;
        }

        public long Idevento { get => _idevento; set => _idevento = value; }
        public long Idattivita { get => _idattivita; set => _idattivita = value; }
        public string Idvincolo { get => _idvincolo; set => _idvincolo = value; }
        public long Idcliente { get => _idcliente; set => _idcliente = value; }
        public DateTime Startdate { get => _startdate; set => _startdate = value; }
        public DateTime Enddate { get => _enddate; set => _enddate = value; }
        public DateTime Datainserimento { get => _datainserimento; set => _datainserimento = value; }
        public double Prezzo { get => _prezzo; set => _prezzo = value; }
        public string Testoevento { get => _testoevento; set => _testoevento = value; }
        public string Codicerichiesta { get => _codicerichiesta; set => _codicerichiesta = value; }
        public long Status { get => _status; set => _status = value; }
        public string Soggetto { get => _soggetto; set => _soggetto = value; }
        public string Textfield1 { get => _textfield1; set => _textfield1 = value; }
        public string Textfield2 { get => _textfield2; set => _textfield2 = value; }
        public string Textfield3 { get => _textfield3; set => _textfield3 = value; }
        public string Jsonfield1 { get => _jsonfield1; set => _jsonfield1 = value; }
        public string Startdatestring //String.Format("{0:yyyy-MM-dd HH:mm}", _startdate)
        {
            get =>
          // "new Date(" + _startdate.Year.ToString() + ", " + _startdate.Month.ToString() + ", " + _startdate.Day.ToString() + ", " + _startdate.Hour.ToString() + ", " + _startdate.Minute.ToString() + ", " + _startdate.Second.ToString() + ")";
          String.Format("{0:yyyy-MM-dd HH:mm:ss}", _startdate);

        }
        public string Enddatestring
        {
            get =>
            //"new Date(" + _enddate.Year.ToString() + ", " + _enddate.Month.ToString() + ", " + _enddate.Day.ToString() + ", " + _enddate.Hour.ToString() + ", " + _enddate.Minute.ToString() + ", " + _enddate.Second.ToString() + ")";
          String.Format("{0:yyyy-MM-dd HH:mm:ss}", _enddate);

        }
    }
}
