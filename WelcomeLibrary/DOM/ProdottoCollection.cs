using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class ProdottoCollection : List<Prodotto>
    {
        private List<Prodotto> _prodottoCollection;
        public ProdottoCollection()
        {
            _prodottoCollection = new List<Prodotto>();
        }
        public List<Prodotto> GetItems()
        {
            return _prodottoCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public ProdottoCollection(ProdottoCollection list)
        {
            Prodotto _tmp;
            foreach (Prodotto tmp in list)
            {
                _tmp = new Prodotto(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public ProdottoCollection(List<Prodotto> list)
        {
            Prodotto _tmp;
            foreach (Prodotto tmp in list)
            {
                _tmp = new Prodotto(tmp);
                this.Add(_tmp);
            }
        }
    }

}
