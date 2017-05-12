using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class TipologiaContenutiCollection : List<TipologiaContenuti>
    {
        private List<TipologiaContenuti> _TipologiaContenutiCollection;
        public TipologiaContenutiCollection()
        {
            _TipologiaContenutiCollection = new List<TipologiaContenuti>();
        }
        public List<TipologiaContenuti> GetItems()
        {
            return _TipologiaContenutiCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public TipologiaContenutiCollection(TipologiaContenutiCollection list)
        {
            TipologiaContenuti _tmpTipologiaContenuti;
            foreach (TipologiaContenuti tmp in list)
            {
                _tmpTipologiaContenuti = new TipologiaContenuti(tmp);
                this.Add(_tmpTipologiaContenuti);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public TipologiaContenutiCollection(List<TipologiaContenuti> list)
        {
            TipologiaContenuti _tmpTipologiaContenuti;
            foreach (TipologiaContenuti tmp in list)
            {
                _tmpTipologiaContenuti = new TipologiaContenuti(tmp);
                this.Add(_tmpTipologiaContenuti);
            }
        }



    }

}
