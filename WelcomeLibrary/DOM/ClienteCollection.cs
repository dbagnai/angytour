using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class ClienteCollection:List<Cliente>
    {
        private List<Cliente> _ClienteCollection;
        public ClienteCollection()
        {
            _ClienteCollection = new List<Cliente>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto ClienteCollection
        /// </summary>
        /// <param name="list"></param>
        public ClienteCollection(ClienteCollection list)
        {
            Cliente _tmp;
            foreach (Cliente tmp in list)
            {
                _tmp = new Cliente(tmp);
                this.Add(_tmp);
            }
        }   
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public ClienteCollection(List<Cliente> list)
        {
            Cliente _tmp;
            foreach (Cliente tmp in list)
            {
                _tmp = new Cliente (tmp);
                this.Add(_tmp);
            }
        }

        public List<Cliente> GetItems()
        {
            return _ClienteCollection;
        }

    }
}
