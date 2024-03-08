using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class StatisticheCollection:List<Statistiche>
    {

         private List<Statistiche> _StatisticheCollection;
        public StatisticheCollection()
        {
            _StatisticheCollection = new List<Statistiche>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto 
        /// </summary>
        /// <param name="list"></param>
        public StatisticheCollection(StatisticheCollection list)
        {
            Statistiche _tmp;
            foreach (Statistiche tmp in list)
            {
                _tmp = new Statistiche(tmp);
                this.Add(_tmp);
            }
        }   
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public StatisticheCollection(List<Statistiche> list)
        {
            Statistiche _tmp;
            foreach (Statistiche tmp in list)
            {
                _tmp = new Statistiche (tmp);
                this.Add(_tmp);
            }
        }

        public List<Statistiche> GetItems()
        {
            return _StatisticheCollection;
        }

    }
}
