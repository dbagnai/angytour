using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using WelcomeLibrary;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SQLite;



public class simpleidname
{
    public string id { set; get; }
    public string name { set; get; }
}

public class jreturncontainer
{
    public Eventi eventitem { set; get; }
    public List<Eventi> eventi { set; get; }
    public Listino listinoitem { set; get; }
    public List<Listino> listini { set; get; }
    public List<simpleidname> reslist { set; get; }
    public List<simpleidname> tipofasce { set; get; }
    public List<simpleidname> statuslist { set; get; }
    public Dictionary<string, string> objfiltro { set; get; }
    public Dictionary<string, string> vincolistrutture { set; get; }

}
public class HandlerBooking : IHttpHandler, IRequiresSessionState
{
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    public Dictionary<string, string> parseparams(HttpContext context)
    {
        Dictionary<string, string> pars = new Dictionary<string, string>();
        bool isPost = false;
        isPost = context.Request.HttpMethod.ToUpper() == "POST";
        bool ismultipart = false;
        if (context.Request.ContentType.ToLower().Contains("multipart/form-data")) ismultipart = true;

        if (isPost && !ismultipart)
            pars = HandlerHelper.GetPostParams(context);
        foreach (var item in context.Request.Params.Keys)
        {
            string szKey = item.ToString();
            if (!pars.ContainsKey(szKey))
                pars.Add(szKey, context.Request.Params[szKey].ToString());
        }

        return pars;
    }

    public void ProcessRequest(HttpContext context)
    {
        string result = "";
        context.Response.ContentType = "text/plain";
        Dictionary<string, string> pars = parseparams(context);

        ////////////////PARAMERTRI AUTOMPLETE
        string term = pars.ContainsKey("term") ? pars["term"].ToLower() : "";
        string id = pars.ContainsKey("id") ? pars["id"] : "";
        List<ResultAutocomplete> lra = new List<ResultAutocomplete>();
        string Recs = pars.ContainsKey("r") ? pars["r"].ToLower() : "50";
        long irecs = 0;
        /////////////////////////////////

        bookingDM bDM = new bookingDM();
        Listino itemtoupdate = new Listino();
        Eventi eventotoupdate = new Eventi();
        long noccorrenze = 0;
        offerteDM offDM = new offerteDM();
        OfferteCollection listlocations = new OfferteCollection();
        List<simpleidname> listresources = new List<simpleidname>();

        List<simpleidname> listtipofasce = new List<simpleidname>();
        listtipofasce.Add(new simpleidname() { id = "1", name = "Giornaliero" });//Hard coded da prendere da db
        listtipofasce.Add(new simpleidname() { id = "2", name = "Settimanale" });//Hard coded da prendere da db

        List<simpleidname> liststatus = new List<simpleidname>();
        liststatus.Add(new simpleidname() { id = "0", name = "Da Confermare" });//Hard coded da prendere da db
        liststatus.Add(new simpleidname() { id = "1", name = "Confermato" });//Hard coded da prendere da db
        //liststatus.Add(new simpleidname() { id = "2", name = "Opzionato" });//Hard coded da prendere da db

        string q = pars.ContainsKey("q") ? pars["q"] : "";
        string lingua = pars.ContainsKey("lng") ? pars["lng"] : "I";
        string item = pars.ContainsKey("item") ? pars["item"] : "";

        string idtipofasce = pars.ContainsKey("idtipofasce") ? pars["idtipofasce"] : "2";
        string idreslist = pars.ContainsKey("idreslist") ? pars["idreslist"] : "";
        string tipologia = pars.ContainsKey("tipologia") ? pars["tipologia"] : "rif000001";
        DateTime datestart = DateTime.MinValue;
        DateTime dateend = DateTime.MaxValue;
        long? status = null;
        bool includivincoli = false;

        string objfiltro = pars.ContainsKey("objfiltro") ? pars["objfiltro"] : "";
        Dictionary<string, string> filtri = new Dictionary<string, string>();
        if (objfiltro != "" && objfiltro != null)
            filtri = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
        if (filtri.ContainsKey("idtipofasce"))
            idtipofasce = filtri["idtipofasce"];
        else
            filtri["idtipofasce"] = idtipofasce;

        if (filtri.ContainsKey("idreslist"))
            idreslist = filtri["idreslist"];
        else
            filtri["idreslist"] = idreslist;

        if (filtri.ContainsKey("idtipologia"))
            tipologia = filtri["idtipologia"];
        else
            filtri["idtipologia"] = tipologia;

        if (filtri.ContainsKey("status"))
        {
            long l = 0;
            if (long.TryParse(filtri["status"], out l))
                status = l;
        }

        if (filtri.ContainsKey("includivincoli"))
        {
            bool.TryParse(filtri["includivincoli"], out includivincoli);
        }


        if (filtri.ContainsKey("datestart"))
        {
            DateTime tmpdate = DateTime.MinValue;
            bool isvalid = DateTime.TryParseExact(filtri["datestart"], "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tmpdate);
            if (isvalid) datestart = tmpdate;
        }
        if (filtri.ContainsKey("dateend"))
        {
            DateTime tmpdate = DateTime.MinValue;
            bool isvalid = DateTime.TryParseExact(filtri["dateend"], "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tmpdate);
            if (isvalid) dateend = tmpdate;
        }

        try
        {
            switch (q)
            {
                case "autocompleteclienti":
                    long.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 20;

                    if (term != "null")
                    {
                        ClientiDM cliDM = new ClientiDM();
                        ClienteCollection coll = cliDM.GetLista("%" + term + "%", WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                        long count = 0;

                        ResultAutocomplete ra = new ResultAutocomplete() { id = "", label = "", value = "Tutti", codice = "" };
                        lra.Add(ra);
                        if (coll != null)
                            foreach (Cliente r in coll)
                            {
                                ra = new ResultAutocomplete() { id = r.Id_cliente.ToString(), label = r.Spare3, value = r.Spare3, codice = r.Spare3.ToString() };
                                if (id == null || id == "") lra.Add(ra);
                                else if (id != "" && r.Id_cliente.ToString() == id) lra.Add(ra);
                                count++;
                                if (count > irecs) break;
                            }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(lra, Newtonsoft.Json.Formatting.Indented);
                    break;
                case "clientebyid":
                    if (id != "")
                    {
                        ClientiDM cliDM = new ClientiDM();
                        Cliente cli = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
                        if (cli != null)
                        {

                            result = Newtonsoft.Json.JsonConvert.SerializeObject(cli, Newtonsoft.Json.Formatting.Indented);// ; cli.Cognome + " " + cli.Nome + " " + cli.Email;
                        }
                    }

                    break;
                case "calcolaprezzo":
                    if (pars.ContainsKey("datestart"))
                    {
                        DateTime tmpdate = DateTime.MinValue;
                        bool isvalid = DateTime.TryParseExact(pars["datestart"], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tmpdate);
                        if (isvalid) datestart = tmpdate;
                    }
                    if (pars.ContainsKey("dateend"))
                    {
                        DateTime tmpdate = DateTime.MinValue;
                        bool isvalid = DateTime.TryParseExact(pars["dateend"], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tmpdate);
                        if (isvalid) dateend = tmpdate;
                    }
                    string idattivita = pars.ContainsKey("idattivita") ? pars["idattivita"] : "";
                    string erroreret = "";
                    double prezzocalcolato = bDM.CalcolaPrezzoBase(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, datestart, dateend, idattivita, ref erroreret);
                    if (!string.IsNullOrEmpty(erroreret))
                    {
                        throw new Exception("Errore calcolo prezzo! " + erroreret);
                    }
                    else
                        result = prezzocalcolato.ToString();

                    break;

                case "inseriscilistino":

                    itemtoupdate = new Listino();
                    if (item != "" && item != null)
                        itemtoupdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Listino>(item, new JsonSerializerSettings()
                        {
                            DateFormatString = "dd/MM/yyyy HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore,
                            //DateFormatString = "dd/MM/yyyy",
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None
                        });

                    //Verifica duplicati/sovrapposizione
                    noccorrenze = bDM.ContaFascelistinoNelPeriodo(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, itemtoupdate.Startdate, itemtoupdate.Enddate, itemtoupdate.Idattivita.ToString(), itemtoupdate.Idtipolistino.ToString());
                    if (noccorrenze == 0)
                    {
                        //Aggiorniamo i dati
                        bookingDM.dbInsertListino(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, itemtoupdate);
                        //ritorno l'elemento inserito ( puoi prederne l'idlistino per aggiornare nel )
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(itemtoupdate, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None,
                        });
                    }
                    break;

                case "aggiornalistino":

                    itemtoupdate = new Listino();
                    if (item != "" && item != null)
                        itemtoupdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Listino>(item, new JsonSerializerSettings()
                        {
                            DateFormatString = "dd/MM/yyyy HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore,
                            //DateFormatString = "dd/MM/yyyy",
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None
                        });
                    //Verifica duplicati/sovrapposizione escludendo l'elemento in aggiornamento
                    noccorrenze = bDM.ContaFascelistinoNelPeriodo(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, itemtoupdate.Startdate, itemtoupdate.Enddate, itemtoupdate.Idattivita.ToString(), itemtoupdate.Idtipolistino.ToString(), itemtoupdate.Idlistino.ToString());
                    if (noccorrenze == 0)
                    {
                        //Aggiorniamo i dati
                        bookingDM.dbUpdateListino(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, itemtoupdate.Idlistino.ToString(), itemtoupdate.Startdate, itemtoupdate.Enddate, itemtoupdate.Idattivita.ToString(), itemtoupdate.Idtipolistino.ToString(), itemtoupdate.Prezzo, itemtoupdate.Prezzolistino);
                    }
                    else
                    {
                        throw new Exception("Sovrapposizione periodi, non aggiornato!");
                    }


                    break;

                case "deletelistino":

                    itemtoupdate = new Listino();
                    if (item != "" && item != null)
                        itemtoupdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Listino>(item, new JsonSerializerSettings()
                        {
                            DateFormatString = "dd/MM/yyyy HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore,
                            //DateFormatString = "dd/MM/yyyy",
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None
                        });
                    //Aggiorniamo i dati
                    if (itemtoupdate.Idlistino != 0)
                        bookingDM.dbDeleteListino(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, itemtoupdate.Idlistino.ToString());


                    break;
                case "caricafascelistino":
                    List<Listino> list = new List<Listino>();

                    list = bDM.CaricaFasceCostoByAttivitaTipotariffa(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idreslist, idtipofasce);
                    if (list == null) list = new List<Listino>();
                    listlocations = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologia);
                    if (listlocations != null)
                    {
                        foreach (Offerte item1 in listlocations)
                        {
                            //listresources.Add(new simpleidname() { id = item1.Id.ToString(), name = offDM.estraititolo(item1, lingua) });
                            listresources.Add(new simpleidname() { id = item1.Id.ToString(), name = item1.DenominazionebyLingua(lingua).Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ") });
                        }
                    }
                    //Inseriamo i nomi delle location nei listini caricati direttamente nel campo listino usato in visualizzazione
                    list.ForEach(l => l.Textfield1 = (listresources.Exists(lr => lr.id == l.Idattivita.ToString())) ? listresources.Find(lr => lr.id == l.Idattivita.ToString()).name : "noname-" + l.Idattivita);

                    //DATI DI TEST
                    //list.Add(new Listino(100, 8, 2, new DateTime(2018, 2, 11, 12, 00, 00), new DateTime(2018, 2, 19, 12, 00, 00), DateTime.Now, 1000, 0, "La tenuta Casa Baccano villa completa"));
                    //list.Add(new Listino(200, 10, 2, new DateTime(2018, 2, 10, 12, 00, 00), new DateTime(2018, 2, 19, 12, 00, 00), DateTime.Now, 1000, 0, "Casa Baccano Bassa Agriturismo"));
                    //list.Add(new Listino(300, 9, 2, new DateTime(2018, 2, 12, 12, 00, 00), new DateTime(2018, 2, 18, 12, 00, 00), DateTime.Now, 1100, 0, "Casa Baccano Alto villa di lusso"));

                    //AGGIUNGO UN VALORE FITTIZIO PER OGNI RISORSA IN MODO DA FORZARE IL JQXSCHEDULER A FAR VEDERE SEMPRE TUTTE LE RISORSE
                    int fakeid = int.MaxValue;
                    foreach (simpleidname i in listresources)
                    {
                        fakeid -= 1;
                        long idl = 0;
                        long.TryParse(i.id, out idl);
                        long idt = 0;
                        long.TryParse(idtipofasce, out idt);
                        list.Add(new Listino(fakeid, idl, idt, new DateTime(2000, 1, 1, 00, 00, 00), new DateTime(2000, 1, 2, 00, 00, 00), DateTime.Now, 0, 0, i.name, "", "dummy-" + idl));
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (string.IsNullOrEmpty(idreslist))
                        //Riordino gli eventi per averli sempre in ordine di idattivita nella lista serializzata
                        list.Sort(new GenericComparer<WelcomeLibrary.DOM.Listino>("Idattivita", System.ComponentModel.ListSortDirection.Ascending));
                    else //metto in in cima l'evento ricercato per visualizzarlo in testa !!!
                    {
                        List<Listino> ordered = list.FindAll(e => e.Idattivita.ToString() == idreslist);
                        list.RemoveAll(e => e.Idattivita.ToString() == idreslist);
                        ordered.AddRange(list);
                        list = ordered;
                    }


                    //Torno sia le fasce dei listini che la lista id-name delle risorse/attivita
                    jreturncontainer jr = new jreturncontainer();
                    jr.listini = list;
                    jr.reslist = listresources;
                    jr.tipofasce = listtipofasce;
                    jr.statuslist = liststatus;
                    jr.objfiltro = filtri;
                    jr.listinoitem = new Listino();//Elemento di riferimento per update

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(jr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "caricalistaattivitaetipofasce":

                    listlocations = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologia);
                    if (listlocations != null)
                    {
                        foreach (Offerte item1 in listlocations)
                        {
                            //listresources.Add(new simpleidname() { id = item1.Id.ToString(), name = offDM.estraititolo(item1, lingua) });
                            listresources.Add(new simpleidname() { id = item1.Id.ToString(), name = item1.DenominazionebyLingua(lingua).Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ") });
                        }
                    }
                    jreturncontainer jr1 = new jreturncontainer();
                    jr1.listini = new List<Listino>();
                    jr1.reslist = listresources;
                    jr1.tipofasce = listtipofasce;
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(jr1, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "caricaeventi":
                    List<Eventi> eventi = new List<Eventi>();
                    TimeSpan ts = (dateend - datestart);
                    eventi = bookingDM.dbGetEvents(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, datestart, ts.Days, idreslist, status,"","","", includivincoli);
                    if (eventi == null) eventi = new List<Eventi>();

                    //LOCATIONS
                    listlocations = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologia);
                    if (listlocations != null)
                    {
                        foreach (Offerte item1 in listlocations)
                        {
                            //listresources.Add(new simpleidname() { id = item1.Id.ToString(), name = offDM.estraititolo(item1, lingua) });
                            listresources.Add(new simpleidname() { id = item1.Id.ToString(), name = item1.DenominazionebyLingua(lingua).Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ") });
                        }
                    }
                    //Inseriamo i nomi delle location nei listini caricati direttamente nel campo listino usato in visualizzazione
                    eventi.ForEach(l => l.Textfield1 = (listresources.Exists(lr => lr.id == l.Idattivita.ToString())) ? listresources.Find(lr => lr.id == l.Idattivita.ToString()).name : "noname-" + l.Idattivita);

                    //DATI DI TEST
                    //list.Add(new Listino(100, 8, 2, new DateTime(2018, 2, 11, 12, 00, 00), new DateTime(2018, 2, 19, 12, 00, 00), DateTime.Now, 1000, 0, "La tenuta Casa Baccano villa completa"));
                    //list.Add(new Listino(200, 10, 2, new DateTime(2018, 2, 10, 12, 00, 00), new DateTime(2018, 2, 19, 12, 00, 00), DateTime.Now, 1000, 0, "Casa Baccano Bassa Agriturismo"));
                    //list.Add(new Listino(300, 9, 2, new DateTime(2018, 2, 12, 12, 00, 00), new DateTime(2018, 2, 18, 12, 00, 00), DateTime.Now, 1100, 0, "Casa Baccano Alto villa di lusso"));

                    //AGGIUNGO UN VALORE FITTIZIO PER OGNI RISORSA IN MODO DA FORZARE IL JQXSCHEDULER A FAR VEDERE SEMPRE TUTTE LE RISORSE
                    int fakeid1 = int.MaxValue;
                    foreach (simpleidname i in listresources)
                    {
                        fakeid1 -= 1;
                        long idl = 0;
                        long.TryParse(i.id, out idl);
                        long idt = 0;
                        long.TryParse(idtipofasce, out idt);
                        eventi.Add(new Eventi(fakeid1, idl, "", 0, new DateTime(2000, 1, 1, 00, 00, 00), new DateTime(2000, 1, 2, 00, 00, 00), DateTime.Now, 0, "dummy event", "", 0, "dummy-" + idl, i.name));
                    }
                    if (string.IsNullOrEmpty(idreslist))
                        //Riordino gli eventi per averli sempre in ordine di idattivita nella lista serializzata
                        eventi.Sort(new GenericComparer<WelcomeLibrary.DOM.Eventi>("Idattivita", System.ComponentModel.ListSortDirection.Ascending));
                    else //metto in in cima l'evento ricercato per visualizzarlo in testa !!!
                    {
                        List<Eventi> ordered = eventi.FindAll(e => e.Idattivita.ToString() == idreslist);
                        eventi.RemoveAll(e => e.Idattivita.ToString() == idreslist);
                        ordered.AddRange(eventi);
                        eventi = ordered;
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////

                    //Torno sia le fasce dei listini che la lista id-name delle risorse/attivita

                    jreturncontainer jr2 = new jreturncontainer();
                    jr2.eventitem = new Eventi();
                    jr2.eventi = eventi;
                    jr2.listini = new List<Listino>();
                    jr2.reslist = listresources;
                    jr2.tipofasce = listtipofasce;
                    jr2.statuslist = liststatus;
                    jr2.objfiltro = filtri;
                    jr2.vincolistrutture = bookingDM.vincolistrutture;
                    jr2.listinoitem = new Listino();//Elemento di riferimento per update

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(jr2, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "verificadisponibilita":

                    eventotoupdate = new Eventi();
                    if (item != "" && item != null)
                        eventotoupdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Eventi>(item, new JsonSerializerSettings()
                        {
                            DateFormatString = "dd/MM/yyyy HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore,
                            //DateFormatString = "dd/MM/yyyy",
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None
                        });
                    if (eventotoupdate.Startdate >= eventotoupdate.Enddate)
                    {
                        throw new Exception("Non inserito. Data inzio maggiore di data fine, correggere!");
                    }
                    //Verifica duplicati/sovrapposizione
                    bool nonelem = bookingDM.dbIsFree(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "0", eventotoupdate.Startdate, eventotoupdate.Enddate, eventotoupdate.Idattivita.ToString());
                    if (nonelem)
                    {
                        result = "Ok";
                        //Ok si può mettere a carrello !!!
                        //a vedere come fare
                    }
                    else
                    {
                        throw new Exception("Nessuna disponibilità per il periodo!");
                    }


                    break;
                case "inseriscievento":

                    eventotoupdate = new Eventi();
                    if (item != "" && item != null)
                        eventotoupdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Eventi>(item, new JsonSerializerSettings()
                        {
                            DateFormatString = "dd/MM/yyyy HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore,
                            //DateFormatString = "dd/MM/yyyy",
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None
                        });
                    if (eventotoupdate.Startdate >= eventotoupdate.Enddate)
                    {
                        throw new Exception("Non inserito. Data inzio maggiore di data fine, correggere!");
                    }
                    //Verifica duplicati/sovrapposizione
                    bool nessunelemento = bookingDM.dbIsFree(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "0", eventotoupdate.Startdate, eventotoupdate.Enddate, eventotoupdate.Idattivita.ToString());
                    if (nessunelemento)
                    {
                        //Aggiorniamo i dati
                        bookingDM.dbInsertEvent(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, eventotoupdate);
                        //ritorno l'elemento inserito ( puoi prederne l'idlistino per aggiornare nel )
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(eventotoupdate, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None,
                        });
                    }
                    else
                    {
                        throw new Exception("Sovrapposizione periodi, non inserito!");
                    }

                    break;
                case "aggiornaevento":


                    eventotoupdate = new Eventi();
                    if (item != "" && item != null)
                        eventotoupdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Eventi>(item, new JsonSerializerSettings()
                        {
                            DateFormatString = "dd/MM/yyyy HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore,
                            //DateFormatString = "dd/MM/yyyy",
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None
                        });

                    if (eventotoupdate.Startdate >= eventotoupdate.Enddate)
                    {
                        throw new Exception("Non aggiornato. Data inzio maggiore di data fine, correggere!");
                    }
                    //Verifica duplicati/sovrapposizione escludendo l'elemento in aggiornamento

                    bool nooverlap = bookingDM.dbIsFree(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, eventotoupdate.Idevento.ToString(), eventotoupdate.Startdate, eventotoupdate.Enddate, eventotoupdate.Idattivita.ToString());
                    if (nooverlap)
                    {
                        //Aggiorniamo i dati
                        bookingDM.dbUpdateEvent(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, eventotoupdate);
                    }
                    else
                    {
                        throw new Exception("Sovrapposizione periodi, non aggiornato!");
                    }

                    break;
                case "deleteevento":

                    eventotoupdate = new Eventi();
                    if (item != "" && item != null)
                        eventotoupdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Eventi>(item, new JsonSerializerSettings()
                        {
                            DateFormatString = "dd/MM/yyyy HH:mm:ss",
                            NullValueHandling = NullValueHandling.Ignore,
                            //DateFormatString = "dd/MM/yyyy",
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None
                        });
                    //Aggiorniamo i dati
                    if (eventotoupdate.Idevento != 0)
                        bookingDM.dbDeleteEvent(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, eventotoupdate.Idevento.ToString());



                    break;
            }
        }
        catch (Exception ex)
        {
            string er = ex.Message;
            result = er;
            context.Response.StatusCode = 400;
        }
        context.Response.Write(result);
        ///////////////////////////////////////////////
    }



}
