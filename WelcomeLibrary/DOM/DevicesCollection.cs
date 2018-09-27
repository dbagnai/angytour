using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class DevicesCollection : List<Devices>
    {
        private long _recordstotali;
        public long Recordstotali { get => _recordstotali; set => _recordstotali = value; }

        private List<Devices> _DevicesCollection;
        public DevicesCollection()
        {
            _DevicesCollection = new List<Devices>();
        }
        public DevicesCollection(List<Devices> list)
        {
            Devices _tmp;
            foreach (Devices tmp in list)
            {
                _tmp = new Devices(tmp);
                this.Add(_tmp);
            }
        }
     
    }

}
