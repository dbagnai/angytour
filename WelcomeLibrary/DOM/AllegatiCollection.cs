using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class AllegatiCollection:List<Allegato>
    {
        private List<Allegato> _AllegatiCollection;
        public AllegatiCollection()
        {
            _AllegatiCollection = new List<Allegato>();
        }

        public List<Allegato> GetItems()
        {
            return _AllegatiCollection;
        }
        private string _Schema;
        public string Schema
        {
            get { return _Schema; }
            set { _Schema = value; }
        }

        private string _Valori;
        public string Valori
        {
            get { return _Valori; }
            set { _Valori = value; }
        }

        private string _FotoAnteprima;
        public string FotoAnteprima
        {
            get { return _FotoAnteprima; }
            set { _FotoAnteprima = value; }
        }
        private string _NomeImmobile;
        public string NomeImmobile
        {
            get { return _NomeImmobile; }
            set { _NomeImmobile = value; }
        }
    }
}
