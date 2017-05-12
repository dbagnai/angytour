using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class BannersCollection:List<Banners>
    {
        private List<Banners> _BannersCollection;
        public BannersCollection()
        {
            _BannersCollection = new List<Banners>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto BannersCollection
        /// </summary>
        /// <param name="list"></param>
        public BannersCollection(BannersCollection list)
        {
            Banners _tmp;
            foreach (Banners tmp in list)
            {
                _tmp = new Banners(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public BannersCollection(List<Banners> list)
        {
            Banners _tmp;
            foreach (Banners tmp in list)
            {
                _tmp = new Banners(tmp);
                this.Add(_tmp);
            }
        }

        public List<Banners> GetItems()
        {
            return _BannersCollection;
        }

    }
}
