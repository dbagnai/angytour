using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class ComuneCollection : List<Comune>
    {
        private List<Comune> _ComuneCollection;
        public ComuneCollection()
        {
            _ComuneCollection = new List<Comune>();
        }
        public List<Comune> GetItems()
        {
            return _ComuneCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public ComuneCollection(ComuneCollection list)
        {
            Comune _tmp;
            foreach (Comune tmp in list)
            {
                _tmp = new Comune(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public ComuneCollection(List<Comune> list)
        {
            Comune _tmp;
            foreach (Comune tmp in list)
            {
                _tmp = new Comune(tmp);
                this.Add(_tmp);
            }
        }
    }

}
