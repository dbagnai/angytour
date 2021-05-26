using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{

    public class jreturncontainercarrello
    {
        public string totale { set; get; }
        public string pezzi { set; get; }
    }

    public class jreturnstatuscarrello
    {
        public string id { set; get; }
        public string stato { set; get; }
    }

    public class jsongtagitem
    {
        public string id { set; get; }
        public string name { set; get; }
        public string list_name { set; get; }
        public string brand { set; get; }
        public string category { set; get; }
        public string variant { set; get; }
        public int list_position { set; get; }
        public long quantity { set; get; }
        public double price { set; get; }
        public string coupon { set; get; }

    }
    public class jsongtagpurchase
    {
        public string transaction_id { set; get; }
        public string affiliation { set; get; }
        public double value { set; get; }
        public string currency { set; get; }
        public double tax { set; get; }
        public double shipping { set; get; }
        public List<jsongtagitem> items { set; get; }

        //https://developers.google.com/analytics/devguides/collection/gtagjs/enhanced-ecommerce
        //        https://developers.google.com/analytics/devguides/collection/gtagjs/sending-data
        //javascript function call!!

        //gtag('event', 'purchase', {
        //            "transaction_id": "24.031608523954162",
        //  "affiliation": "Google online store",
        //  "value": 23.07,
        //  "currency": "USD",
        //  "tax": 1.24,
        //  "shipping": 0,
        //  "items": [
        //    {
        //      "id": "P12345",
        //      "name": "Android Warhol T-Shirt",
        //      "list_name": "Search Results",
        //      "brand": "Google",
        //      "category": "Apparel/T-Shirts",
        //      "variant": "Black",
        //      "list_position": 1,
        //      "quantity": 2,
        //      "price": '2.0'
        //    },
        //    {
        //      "id": "P67890",
        //      "name": "Flame challenge TShirt",
        //      "list_name": "Search Results",
        //      "brand": "MyBrand",
        //      "category": "Apparel/T-Shirts",
        //      "variant": "Red",
        //      "list_position": 2,
        //      "quantity": 1,
        //      "price": '3.0'
        //    }
        //  ]
        //});

    }

    [Serializable]
    public class TotaliCarrello
    {
        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _CodiceOrdine;
        public string CodiceOrdine
        {
            get { return _CodiceOrdine; }
            set { _CodiceOrdine = value; }
        }
        private double _TotaleOrdine;
        public double TotaleOrdine
        {
            get { return _TotaleOrdine; }
            set { _TotaleOrdine = value; }
        }
        private double _TotaleSpedizione;
        public double TotaleSpedizione
        {
            get { return _TotaleSpedizione; }
            set { _TotaleSpedizione = value; }
        }

        private double _TotaleSmaltimento;
        public double TotaleSmaltimento
        {
            get { return _TotaleSmaltimento; }
            set { _TotaleSmaltimento = value; }
        }
        private long _Nassicurazioni;
        public long Nassicurazioni
        {
            get { return _Nassicurazioni; }
            set { _Nassicurazioni = value; }
        }

        private double _TotaleAssicurazione;
        public double TotaleAssicurazione
        {
            get { return _TotaleAssicurazione; }
            set { _TotaleAssicurazione = value; }
        }
        private double _TotaleSconto;
        public double TotaleSconto
        {
            get { return _TotaleSconto; }
            set { _TotaleSconto = value; }
        }


        private bool _bloccaacquisto;
        public bool Bloccaacquisto
        {
            get { return _bloccaacquisto; }
            set { _bloccaacquisto = value; }

        }


        private double _Totalepeso;
        public double TotalePeso
        {
            get { return _Totalepeso; }
            set { _Totalepeso = value; }

        }
        private double _precacconto;
        public double Percacconto
        {
            get { return _precacconto; }
            set { _precacconto = value; }
        }

        public double TotaleAcconto
        {
            get { return (_precacconto != 100) ? ((_TotaleSmaltimento + _TotaleSpedizione + _TotaleOrdine) - _TotaleSconto) * _precacconto / 100 : 0; }
            //set { _TotaleAcconto = value; }
        }
        public double TotaleSaldo
        {
            get { return (_precacconto != 100) ? ((_TotaleSmaltimento + _TotaleSpedizione + _TotaleOrdine) - _TotaleSconto) * (100 - _precacconto) / 100 : (_TotaleSmaltimento + _TotaleSpedizione + _TotaleOrdine) - _TotaleSconto; }
            //set { _TotaleSaldo = value; }
        }


        private string _Indirizzofatturazione;
        public string Indirizzofatturazione
        {
            get { return _Indirizzofatturazione; }
            set { _Indirizzofatturazione = value; }
        }
        private string _Indirizzospedizione;
        public string Indirizzospedizione
        {
            get { return _Indirizzospedizione; }
            set { _Indirizzospedizione = value; }
        }
        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }
        private string _urlpagamento;
        public string Urlpagamento
        {
            get { return _urlpagamento; }
            set { _urlpagamento = value; }
        }

        private long _id_cliente;
        public long Id_cliente
        {
            get { return _id_cliente; }
            set { _id_cliente = value; }
        }

        private string _mailcliente;
        public string Mailcliente
        {
            get { return _mailcliente; }
            set { _mailcliente = value; }
        }
        private string _denominazionecliente;
        public string Denominazionecliente
        {
            get { return _denominazionecliente; }
            set { _denominazionecliente = value; }
        }
        private bool _pagato;
        public bool Pagato
        {
            get { return _pagato; }
            set { _pagato = value; }
        }
        private bool _pagatoacconto;
        public bool Pagatoacconto
        {
            get { return _pagatoacconto; }
            set { _pagatoacconto = value; }
        }
        private DateTime? _Dataordine;
        public DateTime? Dataordine
        {
            get { return _Dataordine; }
            set { _Dataordine = value; }
        }
        private string _modalitapagamento;
        public string Modalitapagamento
        {
            get { return _modalitapagamento; }
            set { _modalitapagamento = value; }
        }
        private CarrelloCollection _CarrelloItems;
        public CarrelloCollection CarrelloItems
        {
            get { return _CarrelloItems; }
            set { _CarrelloItems = value; }
        }

        private bool _supplementospedizione;
        public bool Supplementospedizione
        {
            get { return _supplementospedizione; }
            set { _supplementospedizione = value; }
        }

        private bool _supplementocontrassegno;
        public bool Supplementocontrassegno
        {
            get { return _supplementocontrassegno; }
            set { _supplementocontrassegno = value; }
        }

        private long _id_commerciale;
        public long Id_commerciale
        {
            get { return _id_commerciale; }
            set { _id_commerciale = value; }
        }
        private string _Codicesconto;
        public string Codicesconto
        {
            get { return _Codicesconto; }
            set { _Codicesconto = value; }
        }

        public TotaliCarrello()
        {
            Id = 0;
            Id_cliente = 0;
            Id_commerciale = 0;
            CodiceOrdine = "";
            Codicesconto = "";
            TotaleOrdine = 0;
            TotaleSconto = 0;
            TotalePeso = 0;
            TotaleAssicurazione = 0;
            Nassicurazioni = 0;
            TotaleSpedizione = 0;
            Indirizzofatturazione = "";
            Indirizzospedizione = "";
            Bloccaacquisto = false;
            Note = "";
            Pagato = false;
            Pagatoacconto = false;
            Urlpagamento = "";
            Modalitapagamento = "";
            Mailcliente = "";
            Denominazionecliente = "";
            Dataordine = null;
            CarrelloItems = new CarrelloCollection();
            Supplementospedizione = false;
            Supplementocontrassegno = false;
            TotaleSmaltimento = 0;

            Percacconto = 0;
        }
        public TotaliCarrello(TotaliCarrello tmp)
        {
            Id = tmp.Id;
            Id_cliente = tmp.Id_cliente;
            Id_commerciale = tmp.Id_commerciale;
            CodiceOrdine = tmp.CodiceOrdine;
            TotaleOrdine = tmp.TotaleOrdine;
            TotaleSconto = tmp.TotaleSconto;
            TotalePeso = tmp.TotalePeso;
            Bloccaacquisto = tmp.Bloccaacquisto;
            TotaleSpedizione = tmp.TotaleSpedizione;
            TotaleAssicurazione = tmp.TotaleAssicurazione;
            Nassicurazioni = tmp.Nassicurazioni;
            Indirizzofatturazione = tmp.Indirizzofatturazione;
            Indirizzospedizione = tmp.Indirizzospedizione;
            Note = tmp.Note;
            Pagato = tmp.Pagato;
            Pagatoacconto = tmp.Pagatoacconto;
            Urlpagamento = tmp.Urlpagamento;
            Mailcliente = tmp.Mailcliente;
            Modalitapagamento = tmp.Modalitapagamento;
            Denominazionecliente = tmp.Denominazionecliente;
            Dataordine = tmp.Dataordine;

            CarrelloItems = new CarrelloCollection(tmp.CarrelloItems);

            Supplementospedizione = tmp.Supplementospedizione;
            Supplementocontrassegno = tmp.Supplementocontrassegno;
            TotaleSmaltimento = tmp.TotaleSmaltimento;

            Percacconto = tmp.Percacconto;

        }
    }
    [Serializable]
    public class Carrello
    {
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private long _ID_cliente;
        public long ID_cliente
        {
            get { return _ID_cliente; }
            set { _ID_cliente = value; }
        }

        private long _id_prodotto;
        public long id_prodotto
        {
            get { return _id_prodotto; }
            set { _id_prodotto = value; }
        }

        private string _SessionId;
        public string SessionId
        {
            get { return _SessionId; }
            set { _SessionId = value; }
        }

        private DateTime _Data;
        public DateTime Data
        {
            get { return _Data; }
            set { _Data = value; }
        }


        private DateTime? _datastart;
        public DateTime? Datastart
        {
            get { return _datastart; }
            set { _datastart = value; }
        }

        private DateTime? _dataend;
        public DateTime? Dataend
        {
            get { return _dataend; }
            set { _dataend = value; }
        }

        private double _Prezzo;
        public double Prezzo
        {
            get { return _Prezzo; }
            set { _Prezzo = value; }
        }

        private long _Iva;
        public long Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }

        private long _Numero;
        public long Numero
        {
            get { return _Numero; }
            set { _Numero = value; }
        }

        private string _CodiceProdotto;
        public string CodiceProdotto
        {
            get { return _CodiceProdotto; }
            set { _CodiceProdotto = value; }
        }


        private string _IpClient;
        public string IpClient
        {
            get { return _IpClient; }
            set { _IpClient = value; }
        }

        private long _Validita;
        public long Validita
        {
            get { return _Validita; }
            set { _Validita = value; }
        }

        private string _CodiceOrdine;
        public string CodiceOrdine
        {
            get { return _CodiceOrdine; }
            set { _CodiceOrdine = value; }
        }

        private string _Campo1;
        public string Campo1
        {
            get { return _Campo1; }
            set { _Campo1 = value; }
        }

        private string _Campo2;
        public string Campo2
        {
            get { return _Campo2; }
            set { _Campo2 = value; }
        }

        private string _Campo3;
        public string Campo3
        {
            get { return _Campo3; }
            set { _Campo3 = value; }
        }
        private string _Codicenazione;
        public string Codicenazione
        {
            get { return _Codicenazione; }
            set { _Codicenazione = value; }
        }
        private string _Codiceprovincia;
        public string Codiceprovincia
        {
            get { return _Codiceprovincia; }
            set { _Codiceprovincia = value; }
        }
        private string _Codicesconto;
        public string Codicesconto
        {
            get { return _Codicesconto; }
            set { _Codicesconto = value; }
        }
        private Offerte _Offerta;
        public Offerte Offerta
        {
            get { return _Offerta; }
            set { _Offerta = value; }
        }
        private long _progressivo = 0;
        public long Progressivo
        {
            get { return _progressivo; }
            set { _progressivo = value; }
        }
        private string _jsonfield1;
        public string jsonfield1
        {
            get { return _jsonfield1; }
            set { _jsonfield1 = value; }
        }
        public Carrello()
        {
            this.ID = 0;
            this.ID_cliente = 0;
            this.id_prodotto = 0;
            this.IpClient = "";
            this.SessionId = "";
            this.Data = DateTime.MinValue;
            this.Prezzo = 0;
            this.Iva = 0;
            this.Numero = 0;
            this.CodiceProdotto = "";
            this.jsonfield1 = "";
            this.Validita = 0;
            this.CodiceOrdine = "";
            this.Campo1 = "";
            this.Campo2 = "";
            this.Campo3 = "";
            this.Codicenazione = "";
            this.Codiceprovincia = "";
            this.Codicesconto = "";
            this.Offerta = new Offerte();
            this.Datastart = null;
            this.Dataend = null;

        }

        public Carrello(Carrello tmp)
        {
            this.Progressivo = tmp.Progressivo;
            this.ID = tmp.ID;
            this.id_prodotto = tmp.id_prodotto;
            this.ID_cliente = tmp.ID_cliente;
            this.IpClient = tmp.IpClient;
            this.SessionId = tmp.SessionId;
            this.Data = tmp.Data;
            this.Prezzo = tmp.Prezzo;
            this.Iva = tmp.Iva;
            this.Numero = tmp.Numero;
            this.CodiceProdotto = tmp.CodiceProdotto;
            this.jsonfield1 = tmp.jsonfield1;
            this.Validita = tmp.Validita;
            this.CodiceOrdine = tmp.CodiceOrdine;
            this.Campo1 = tmp.Campo1;
            this.Campo2 = tmp.Campo2;
            this.Campo3 = tmp.Campo3;
            this.Codicenazione = tmp.Codicenazione;
            this.Codicenazione = tmp.Codiceprovincia;
            this.Codicesconto = tmp.Codicesconto;
            this.Codiceprovincia = "";
            this.Offerta = new Offerte(tmp.Offerta);

            this.Datastart = tmp.Datastart;
            this.Dataend = tmp.Dataend;

        }
    }
}
