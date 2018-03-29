using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class OfferteCollection:List<Offerte>
    {
        private List<Offerte> _OfferteCollection;
        public OfferteCollection()
        {
            _OfferteCollection = new List<Offerte>();
        }

        private long _totrecs = 0;
        public long Totrecs { get => _totrecs; set => _totrecs = value; }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto OfferteCollection
        /// </summary>
        /// <param name="list"></param>
        public OfferteCollection(OfferteCollection list)
        {
            Offerte _tmp;
            foreach (Offerte tmp in list)
            {
                _tmp = new Offerte(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public OfferteCollection(List<Offerte> list)
        {
            Offerte _tmp;
            foreach (Offerte tmp in list)
            {
                _tmp = new Offerte(tmp);
                this.Add(_tmp);
            }

        }

        public List<Offerte> GetItems()
        {
            return _OfferteCollection;
        }

    }
}
