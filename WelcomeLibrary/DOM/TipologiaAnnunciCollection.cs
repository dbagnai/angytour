using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class TipologiaAnnunciCollection : List<TipologiaAnnunci>
    {
        private List<TipologiaAnnunci> _TipologiaAnnunciCollection;
        public TipologiaAnnunciCollection()
        {
            _TipologiaAnnunciCollection = new List<TipologiaAnnunci>();
        }
        public List<TipologiaAnnunci> GetItems()
        {
            return _TipologiaAnnunciCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public TipologiaAnnunciCollection(TipologiaAnnunciCollection list)
        {
            TipologiaAnnunci _tmpTipologiaAnnunci;
            foreach (TipologiaAnnunci tmp in list)
            {
                _tmpTipologiaAnnunci = new TipologiaAnnunci(tmp);
                this.Add(_tmpTipologiaAnnunci);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public TipologiaAnnunciCollection(List<TipologiaAnnunci> list)
        {
             TipologiaAnnunci _tmpTipologiaAnnunci;
            foreach (TipologiaAnnunci tmp in list)
            {
                _tmpTipologiaAnnunci = new TipologiaAnnunci(tmp);
                this.Add(_tmpTipologiaAnnunci);
            }
        }



    }

}
