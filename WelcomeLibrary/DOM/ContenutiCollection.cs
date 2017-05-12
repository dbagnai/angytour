using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class ContenutiCollection:List<Contenuti>
    {
        private List<Contenuti> _ContenutiCollection;
        public ContenutiCollection()
        {
            _ContenutiCollection = new List<Contenuti>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto ContenutiCollection
        /// </summary>
        /// <param name="list"></param>
        public ContenutiCollection(ContenutiCollection list)
        {
            Contenuti _tmp;
            foreach (Contenuti tmp in list)
            {
                _tmp = new Contenuti(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public ContenutiCollection(List<Contenuti> list)
        {
            Contenuti _tmp;
            foreach (Contenuti tmp in list)
            {
                _tmp = new Contenuti(tmp);
                this.Add(_tmp);
            }
        }

        public List<Contenuti> GetItems()
        {
            return _ContenutiCollection;
        }

    }
}
