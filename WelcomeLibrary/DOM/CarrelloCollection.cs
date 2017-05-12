using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class TotaliCarrelloCollection : List<TotaliCarrello>
    {
        private List<TotaliCarrello> _TotaliCarrelloCollection;
        public TotaliCarrelloCollection()
        {
            _TotaliCarrelloCollection = new List<TotaliCarrello>();
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto CarrelloCollection
        /// </summary>
        /// <param name="list"></param>
        public TotaliCarrelloCollection(TotaliCarrelloCollection list)
        {
            TotaliCarrello _tmp;
            foreach (TotaliCarrello tmp in list)
            {
                _tmp = new TotaliCarrello(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public TotaliCarrelloCollection(List<TotaliCarrello> list)
        {
            TotaliCarrello _tmp;
            foreach (TotaliCarrello tmp in list)
            {
                _tmp = new TotaliCarrello(tmp);
                this.Add(_tmp);
            }
        }

        public List<TotaliCarrello> GetItems()
        {
            return _TotaliCarrelloCollection;
        }
    }

    [Serializable]
    public class CarrelloCollection:List<Carrello>
    {
    private List<Carrello> _CarrelloCollection;
        public CarrelloCollection()
        {
            _CarrelloCollection = new List<Carrello>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto CarrelloCollection
        /// </summary>
        /// <param name="list"></param>
        public CarrelloCollection(CarrelloCollection list)
        {
            Carrello _tmp;
            foreach (Carrello tmp in list)
            {
                _tmp = new Carrello(tmp);
                this.Add(_tmp);
            }
        }   
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public CarrelloCollection(List<Carrello> list)
        {
            Carrello _tmp;
            foreach (Carrello tmp in list)
            {
                _tmp = new Carrello (tmp);
                this.Add(_tmp);
            }
        }

        public List<Carrello> GetItems()
        {
            return _CarrelloCollection;
        }
    }
}
