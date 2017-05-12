using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class AnnunciCollection:List<Annunci>
    {
        private List<Annunci> _AnnunciCollection;
        public AnnunciCollection()
        {
            _AnnunciCollection = new List<Annunci>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto AnnunciCollection
        /// </summary>
        /// <param name="list"></param>
        public AnnunciCollection(AnnunciCollection list)
        {
            Annunci _tmp;
            foreach (Annunci tmp in list)
            {
                _tmp = new Annunci(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public AnnunciCollection(List<Annunci> list)
        {
            Annunci _tmp;
            foreach (Annunci tmp in list)
            {
                _tmp = new Annunci(tmp);
                this.Add(_tmp);
            }
        }

        public List<Annunci> GetItems()
        {
            return _AnnunciCollection;
        }

    }
}
