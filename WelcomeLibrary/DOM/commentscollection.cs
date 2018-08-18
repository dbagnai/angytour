using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class CommentsCollection : List<Comment>
    {
        private double _mediatotalestars;
        private long _napprovati;
        private long _recordstotali;

        private List<Comment> _CommentsCollection;
        public CommentsCollection()
        {
            _CommentsCollection = new List<Comment>();
        }
        public CommentsCollection(List<Comment> list)
        {
            Comment _tmp;
            foreach (Comment tmp in list)
            {
                _tmp = new Comment(tmp);
                this.Add(_tmp);
            }
        }

        public double Mediatotalestars { get => _mediatotalestars; set => _mediatotalestars = value; }
        public long Napprovati { get => _napprovati; set => _napprovati = value; }
        public long Recordstotali { get => _recordstotali; set => _recordstotali = value; }
    }

}
