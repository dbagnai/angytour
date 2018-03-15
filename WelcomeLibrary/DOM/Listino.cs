using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Listino
    {
        long _idlistino;
        long _idattivita;
        long _idtipolistino;
        DateTime _startdate;
        DateTime _enddate;
        DateTime _datainserimento;
        double _prezzo;
        double _prezzolistino;
        string _textfield1;
        string _textfield2;
        string _textfield3;

        public Listino(long Idlistino, long idattivita, long idtipolistino, DateTime startdate, DateTime enddate, DateTime datainserimento, double prezzo, double prezzolistino = 0, string textfield1 = "", string textfield2 = "", string textfield3 = "")
        {
            _idlistino = Idlistino;
            _idattivita = idattivita;
            _idtipolistino = idtipolistino;
            _startdate = startdate;
            _enddate = enddate;
            _datainserimento = datainserimento;
            _prezzo = prezzo;
            _prezzolistino = prezzolistino;
            _textfield1 = textfield1;
            _textfield2 = textfield2;
            _textfield3 = textfield3;
        }
        public Listino(Listino tmp)
        {
            _idlistino = tmp.Idlistino;
            _idattivita = tmp.Idattivita;
            _idtipolistino = tmp.Idtipolistino;
            _startdate = tmp.Startdate;
            _enddate = tmp.Enddate;
            _datainserimento = tmp.Datainserimento;
            _prezzo = tmp.Prezzo; ;
            _prezzolistino = tmp.Prezzolistino;
            _textfield1 = tmp.Textfield1;
            _textfield2 = tmp.Textfield2;
            _textfield3 = tmp.Textfield3;
        }
        public Listino()
        {
            _idlistino = 0;
            _idattivita = 0;
            _idtipolistino = 0;
            _startdate = DateTime.MinValue;
            _enddate = DateTime.MinValue;
            _datainserimento = DateTime.Now;
            _prezzo = 0;
            _prezzolistino = 0;
            _textfield1 = string.Empty;
            _textfield2 = string.Empty;
            _textfield3 = string.Empty;
        }

        public long Idlistino { get => _idlistino; set => _idlistino = value; }
        public long Idattivita { get => _idattivita; set => _idattivita = value; }
        public long Idtipolistino { get => _idtipolistino; set => _idtipolistino = value; }
        public DateTime Startdate { get => _startdate; set => _startdate = value; }
        public DateTime Enddate { get => _enddate; set => _enddate = value; }
        public DateTime Datainserimento { get => _datainserimento; set => _datainserimento = value; }
        public double Prezzo { get => _prezzo; set => _prezzo = value; }
        public double Prezzolistino { get => _prezzolistino; set => _prezzolistino = value; }
        public string Textfield1 { get => _textfield1; set => _textfield1 = value; }
        public string Textfield2 { get => _textfield2; set => _textfield2 = value; }
        public string Textfield3 { get => _textfield3; set => _textfield3 = value; }
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
