using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Devices
    {
        private long _id;
        private string _name;
        private string _pushAuth;
        private string _pushEndpoint;
        private string _pushP256DH;
        public long Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string PushAuth { get => _pushAuth; set => _pushAuth = value; }
        public string PushEndpoint { get => _pushEndpoint; set => _pushEndpoint = value; }
        public string PushP256DH { get => _pushP256DH; set => _pushP256DH = value; }

        public Devices()
        {
            this.Id = 0;
            this._name = string.Empty;
            this._pushAuth = string.Empty;
            this._pushEndpoint = string.Empty;
            this._pushP256DH = string.Empty;
            
        }
        public Devices(Devices tmp)
        {
            this.Id = tmp.Id;
            this._name = tmp.Name;
            this._pushAuth = tmp.PushAuth;
            this._pushEndpoint = tmp.PushEndpoint;
            this._pushP256DH = tmp.PushP256DH;

        }

        

    }
}
