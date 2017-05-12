using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class SProdottoCollection : List<SProdotto>
    {
        private List<SProdotto> _SProdottoCollection;
        public SProdottoCollection()
        {
            _SProdottoCollection = new List<SProdotto>();
        }
        public List<SProdotto> GetItems()
        {
            return _SProdottoCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public SProdottoCollection(SProdottoCollection list)
        {
            SProdotto _tmp;
            foreach (SProdotto tmp in list)
            {
                _tmp = new SProdotto(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public SProdottoCollection(List<SProdotto> list)
        {
            SProdotto _tmp;
            foreach (SProdotto tmp in list)
            {
                _tmp = new SProdotto(tmp);
                this.Add(_tmp);
            }
        }
    }

}
