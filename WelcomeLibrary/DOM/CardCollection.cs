using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class CardCollection:List<Card>
    {
        private List<Card> _CardCollection;
        public CardCollection()
        {
            _CardCollection = new List<Card>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto CardCollection
        /// </summary>
        /// <param name="list"></param>
        public CardCollection(CardCollection list)
        {
            Card _tmp;
            foreach (Card tmp in list)
            {
                _tmp = new Card(tmp);
                this.Add(_tmp);
            }
        }   
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public CardCollection(List<Card> list)
        {
            Card _tmp;
            foreach (Card tmp in list)
            {
                _tmp = new Card (tmp);
                this.Add(_tmp);
            }
        }

        public List<Card> GetItems()
        {
            return _CardCollection;
        }

    }
}
