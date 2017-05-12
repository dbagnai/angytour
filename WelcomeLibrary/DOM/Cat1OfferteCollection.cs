using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable()]
    public class CategoriaOfferteLiv1Collection : List<CategoriaOfferteLiv1>
    {
        private List<CategoriaOfferteLiv1> _CategoriaOfferteLiv1Collection;
        public CategoriaOfferteLiv1Collection()
        {
            _CategoriaOfferteLiv1Collection = new List<CategoriaOfferteLiv1>();
        }
        public List<CategoriaOfferteLiv1> GetItems()
        {
            return _CategoriaOfferteLiv1Collection;
        }

        /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto Collection
        /// </summary>
        /// <param name="list"></param>
        public CategoriaOfferteLiv1Collection(CategoriaOfferteLiv1Collection list)
        {
            CategoriaOfferteLiv1 _tmp;
            foreach (CategoriaOfferteLiv1 tmp in list)
            {
                _tmp = new CategoriaOfferteLiv1(tmp);
                this.Add(_tmp);
            }
        }
        /// <summary>
        /// Costruttore dell'oggetto a partire da una Lista generica
        /// </summary>
        /// <param name="list"></param>
        public CategoriaOfferteLiv1Collection(List<CategoriaOfferteLiv1> list)
        {
            CategoriaOfferteLiv1 _tmp;
            foreach (CategoriaOfferteLiv1 tmp in list)
            {
                _tmp = new CategoriaOfferteLiv1(tmp);
                this.Add(_tmp);
            }
        }



    }

}
