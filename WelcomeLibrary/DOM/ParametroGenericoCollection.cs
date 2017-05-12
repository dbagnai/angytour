using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class ParametroGenericoCollection : List<ParametroGenerico>
    {
        private List<ParametroGenerico> _ParametroGenericoCollection;
        public ParametroGenericoCollection()
        {
            _ParametroGenericoCollection = new List<ParametroGenerico>();
        }
        public List<ParametroGenerico> GetItems()
        {
            return _ParametroGenericoCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public ParametroGenericoCollection(ParametroGenericoCollection list)
        {
            ParametroGenerico _tmpParametroGenerico;
            foreach (ParametroGenerico tmp in list)
            {
                _tmpParametroGenerico = new ParametroGenerico(tmp);
                this.Add(_tmpParametroGenerico);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public ParametroGenericoCollection(List<ParametroGenerico> list)
        {
            ParametroGenerico _tmpParametroGenerico;
            foreach (ParametroGenerico tmp in list)
            {
                _tmpParametroGenerico = new ParametroGenerico(tmp);
                this.Add(_tmpParametroGenerico);
            }
        }
    }
}
