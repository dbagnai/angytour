using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Offerte_cat1_linkCollection:List<Offerte_cat1_link>
    {
        private List<Offerte_cat1_link> _Offerte_cat1_linkCollection;
        public Offerte_cat1_linkCollection()
        {
            _Offerte_cat1_linkCollection = new List<Offerte_cat1_link>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto OfferteCollection
        /// </summary>
        /// <param name="list"></param>
        public Offerte_cat1_linkCollection(Offerte_cat1_linkCollection list)
        {
            Offerte_cat1_link _tmp;
            foreach (Offerte_cat1_link tmp in list)
            {
                _tmp = new Offerte_cat1_link(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public Offerte_cat1_linkCollection(List<Offerte_cat1_link> list)
        {
            Offerte_cat1_link _tmp;
            foreach (Offerte_cat1_link tmp in list)
            {
                _tmp = new Offerte_cat1_link(tmp);
                this.Add(_tmp);
            }
        }

        public List<Offerte_cat1_link> GetItems()
        {
            return _Offerte_cat1_linkCollection;
        }

    }
}
