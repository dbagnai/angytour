using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class FascediprezzoCollection : List<Fascediprezzo>
    {
        private List<Fascediprezzo> _FascediprezzoCollection;
        public FascediprezzoCollection()
        {
            _FascediprezzoCollection = new List<Fascediprezzo>();
        }
        public List<Fascediprezzo> GetItems()
        {
            return _FascediprezzoCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public FascediprezzoCollection(FascediprezzoCollection list)
        {
            Fascediprezzo _tmpFascediprezzo;
            foreach (Fascediprezzo tmp in list)
            {
                _tmpFascediprezzo = new Fascediprezzo(tmp);
                this.Add(_tmpFascediprezzo);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public FascediprezzoCollection(List<Fascediprezzo> list)
        {
            Fascediprezzo _tmpFascediprezzo;
            foreach (Fascediprezzo tmp in list)
            {
                _tmpFascediprezzo = new Fascediprezzo(tmp);
                this.Add(_tmpFascediprezzo);
            }
        }
    }
}
