using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.DOM
{
    [Serializable]
    public class Card
    {
        private long _Id_card;
        public long Id_card
        {
            get { return _Id_card; }
            set { _Id_card = value; }
        }

        private string _CodiceCard;
        public string CodiceCard
        {
            get { return _CodiceCard; }
            set { _CodiceCard = value; }
        }
           private DateTime _DataGenerazione;
        public DateTime DataGenerazione
        {
            get { return _DataGenerazione; }
            set { _DataGenerazione = value; }
        }
        private DateTime? _DataAttivazione;
        public DateTime? DataAttivazione
        {
            get { return _DataAttivazione; }
            set { _DataAttivazione = value; }
        }
          private long _DurataGG;
        public long DurataGG
        {
            get { return _DurataGG; }
            set { _DurataGG = value; }
        }
        private bool _AssegnatoACard;
        public bool AssegnatoACard
        {
            get { return _AssegnatoACard; }
            set { _AssegnatoACard = value; }
        }
        public Card()
        {
            this.Id_card = 0;
            this.CodiceCard = string.Empty;
            this.DataAttivazione = null;
            this.DurataGG = 0;
            this.DataGenerazione = DateTime.MinValue;
            this.AssegnatoACard = false;

        }
        public Card(Card tmp)
        {
            this.Id_card = tmp.Id_card;
            this.CodiceCard = tmp.CodiceCard;
            this.DataAttivazione = tmp.DataAttivazione;
            this.DurataGG = tmp.DurataGG;
            this.DataGenerazione = tmp.DataGenerazione;
            this.AssegnatoACard = tmp.AssegnatoACard;
        }
    }
}
