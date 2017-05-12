using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class MailCollection:List<Mail>
    {
        private List<Mail> _MailCollection;
        public MailCollection()
        {
            _MailCollection = new List<Mail>();
        }

          /// <summary>
        /// Costruttore dell'oggetto a partire da un oggetto MailCollection
        /// </summary>
        /// <param name="list"></param>
        public MailCollection(MailCollection list)
        {
            Mail _tmp;
            foreach (Mail tmp in list)
            {
                _tmp = new Mail(tmp);
                this.Add(_tmp);
            }
        }   
        /// <summary>
        /// Costruttore dell'oggetto a partire da una List Collection
        /// </summary>
        /// <param name="list"></param>
        public MailCollection(List<Mail> list)
        {
            Mail _tmp;
            foreach (Mail tmp in list)
            {
                _tmp = new Mail (tmp);
                this.Add(_tmp);
            }
        }

        public List<Mail> GetItems()
        {
            return _MailCollection;
        }

    }
}
