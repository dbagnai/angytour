using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class StrutturaCollection : List<Struttura>
    {
        private List<Struttura> _StrutturaCollection;
        public StrutturaCollection()
        {
            _StrutturaCollection = new List<Struttura>();
        }

        
          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto StrutturaCollection
        /// </summary>
        /// <param name="list"></param>
        public StrutturaCollection(StrutturaCollection list)
        {
            Struttura _tmp;
            foreach (Struttura tmp in list)
            {
                _tmp = new Struttura(tmp);
                this.Add(_tmp);
            }
        } 

                /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public StrutturaCollection(List<Struttura> list)
        {
            Struttura _tmp;
            foreach (Struttura tmp in list)
            {
                _tmp = new Struttura(tmp);
                this.Add(_tmp);
            }
        }

        public List<Struttura> GetItems()
        {
            return _StrutturaCollection;
        }
    }
}
