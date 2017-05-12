using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class TabrifCollection : List<Tabrif>
    {
        private List<Tabrif> _TabrifCollection;

        public TabrifCollection()
        {
            _TabrifCollection = new List<Tabrif>();
        }

        public List<Tabrif> GetItems()
        {
            return _TabrifCollection;
        }   
    }
}
