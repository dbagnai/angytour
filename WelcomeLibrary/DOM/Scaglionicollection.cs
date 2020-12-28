using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class ScaglioniCollection : List<Scaglioni>
    {
        private List<Scaglioni> _scaglionicollection;
        public ScaglioniCollection()
        {
            _scaglionicollection = new List<Scaglioni>();
        }

        private long _totrecs = 0;
        public long Totrecs { get => _totrecs; set => _totrecs = value; }


        private string _serialized = string.Empty;
        public string Serialized { get => _serialized; set => _serialized = value; }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto ScaglioniCollection
        /// </summary>
        /// <param name="list"></param>
        public ScaglioniCollection(ScaglioniCollection list)
        {
            Scaglioni _tmp;
            foreach (Scaglioni tmp in list)
            {
                _tmp = new Scaglioni(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public ScaglioniCollection(List<Scaglioni> list)
        {
            Scaglioni _tmp;
            foreach (Scaglioni tmp in list)
            {
                _tmp = new Scaglioni(tmp);
                this.Add(_tmp);
            }
        }

        public List<Scaglioni> GetItems()
        {
            return _scaglionicollection;
        }

    }
}
