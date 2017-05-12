using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class TipologiaOfferteCollection : List<TipologiaOfferte>
    {
        private List<TipologiaOfferte> _TipologiaOfferteCollection;
        public TipologiaOfferteCollection()
        {
            _TipologiaOfferteCollection = new List<TipologiaOfferte>();
        }
        public List<TipologiaOfferte> GetItems()
        {
            return _TipologiaOfferteCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public TipologiaOfferteCollection(TipologiaOfferteCollection list)
        {
            TipologiaOfferte _tmpTipologiaOfferte;
            foreach (TipologiaOfferte tmp in list)
            {
                _tmpTipologiaOfferte = new TipologiaOfferte(tmp);
                this.Add(_tmpTipologiaOfferte);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public TipologiaOfferteCollection(List<TipologiaOfferte> list)
        {
             TipologiaOfferte _tmpTipologiaOfferte;
            foreach (TipologiaOfferte tmp in list)
            {
                _tmpTipologiaOfferte = new TipologiaOfferte(tmp);
                this.Add(_tmpTipologiaOfferte);
            }
        }



    }

}
