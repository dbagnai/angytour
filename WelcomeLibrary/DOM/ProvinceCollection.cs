using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class ProvinceCollection : List<Province>
    {
        private List<Province> _ProvinceCollection;
        public ProvinceCollection()
        {
            _ProvinceCollection = new List<Province>();
        }
        public List<Province> GetItems()
        {
            return _ProvinceCollection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public ProvinceCollection(ProvinceCollection list)
        {
            Province _tmpprovincia;
            foreach (Province tmp in list)
            {
                _tmpprovincia = new Province(tmp);
                this.Add(_tmpprovincia);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public ProvinceCollection(List<Province> list)
        {
            Province _tmpprovincia;
            foreach (Province tmp in list)
            {
                _tmpprovincia = new Province(tmp);
                this.Add(_tmpprovincia);
            }
        }


    }

}
