using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using WelcomeLibrary;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SQLite;

public class jreturncontainerdata
{
    public string html { set; get; }
    public Dictionary<string, string> jscommands { set; get; }
}

public class HandlerDataCommon : IHttpHandler, IRequiresSessionState
{
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
        try
        {
            usermanager USM = new usermanager();
            Dictionary<string, string> pars = parseparams(context);
            string q = pars.ContainsKey("q") ? pars["q"] : "";

            string term = pars.ContainsKey("term") ? pars["term"].ToLower() : "";
            string id = pars.ContainsKey("id") ? pars["id"] : "";
            List<ResultAutocomplete> lra = new List<ResultAutocomplete>();
            string Recs = pars.ContainsKey("r") ? pars["r"].ToLower() : "50";
            long irecs = 0;
            string lingua = pars.ContainsKey("lng") ? pars["lng"] : "I";
            string progressivo = pars.ContainsKey("progressivo") ? pars["progressivo"] : "";
            string tipologia = pars.ContainsKey("tipologia") ? pars["tipologia"] : "";

            string filter1 = pars.ContainsKey("filter1") ? pars["filter1"] : "";
            string filter2 = pars.ContainsKey("filter2") ? pars["filter2"] : "";
            string Key = pars.ContainsKey("key") ? pars["key"] : "";
            string Value = pars.ContainsKey("value") ? pars["value"] : "";

            string objfiltro = pars.ContainsKey("objfiltro") ? pars["objfiltro"] : "";
            string page = pars.ContainsKey("page") ? pars["page"] : "1";
            string pagesize = pars.ContainsKey("pagesize") ? pars["pagesize"] : "10";
            string enablepager = pars.ContainsKey("enablepager") ? pars["enablepager"] : "true";
            Dictionary<string, string> res = new Dictionary<string, string>();
            Dictionary<string, string> filtri = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);

            switch (q)
            {
                case "autocompleteclienti":
                    long.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 20;
                    if (term != "null")
                    {
                        ClientiDM cliDM = new ClientiDM();
                        ClienteCollection coll = cliDM.GetLista(term, WelcomeLibrary.STATIC.Global.NomeConnessioneDb);
                        long count = 0;
                        ResultAutocomplete ra = null;
                        // ResultAutocomplete ra = new ResultAutocomplete() { id = "", label = "", value = "Deseleziona", codice = "" };
                        //lra.Add(ra);
                        if (coll != null)
                            foreach (Cliente r in coll)
                            {
                                ra = new ResultAutocomplete() { id = r.Id_cliente.ToString(), label = r.Spare3, email = r.Email, nome = r.Nome.ToString(), cognome = r.Cognome.ToString() };
                                if (id == null || id == "") lra.Add(ra);
                                else if (id != "" && r.Id_cliente.ToString() == id) lra.Add(ra);
                                count++;
                                if (count > irecs) break;
                            }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(lra, Newtonsoft.Json.Formatting.Indented);
                    break;
                case "autocompletecaratteristiche":
                    long.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 50;

                    int progr = 0;
                    int.TryParse(progressivo, out progr);
                    //if (progr == 0) progr = 5;
                    if (term != "null")
                    {
                        List<Tabrif> Caratteristica = new List<Tabrif>();
                        Caratteristica = references.FiltraCaratteristiche(progr, term, lingua);
                        long count = 0;

                        ResultAutocomplete ra = new ResultAutocomplete() { id = "", label = "", value = "Tutti", codice = "" };
                        lra.Add(ra);
                        if (Caratteristica != null)
                            foreach (Tabrif r in Caratteristica)
                            {
                                ra = new ResultAutocomplete() { id = r.Id.ToString(), label = r.Campo1, value = r.Campo1, codice = r.Codice.ToString() };
                                if (id == null || id == "") lra.Add(ra);
                                else if (id != "" && r.Id.ToString() == id) lra.Add(ra);
                                count++;
                                if (count > irecs) break;
                            }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(lra, Newtonsoft.Json.Formatting.Indented);
                    break;
                case "autocompletericercalist":
                    long.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 20;
                    if (term != "null")
                    {
                        offerteDM offDM = new offerteDM();
                        OfferteCollection coll = offDM.GetLista(term, irecs.ToString(), lingua, WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologia);
                        long count = 0;
                        ResultAutocomplete ra1 = new ResultAutocomplete() { id = "", label = "Scegli destinazione", linktext = "<span style=\"font-weight:600\">" + "Scegli destinazione" + "</span>" };
                        lra.Add(ra1);
                        if (coll != null)
                            foreach (Offerte r in coll)
                            {
                                string shorttext = r.DenominazionebyLingua(lingua);
                                string htmltext = "";
                                htmltext += "<span style=\"font-weight:600\">" + r.DenominazionebyLingua(lingua) + "</span>";
                                Scaglioni scaglione = r.Scaglioni.Find(s => s.datapartenza > System.DateTime.Now);  //prendo solo quelli che hanno scaglioni in partenza dopo oggi
                                if (scaglione != null)
                                {
                                    htmltext += " <br/> ";
                                    htmltext += scaglione.durata + " " + references.ResMan("Common", lingua, "testogiorni").ToString();
                                    htmltext += " • ";
                                    htmltext += references.ResMan("Common", lingua, "titoloprezzoapartire").ToString() + " " + scaglione.prezzo + "€";

                                    string linkurlidhtml = "<a target=\"_self\" href =\"" + WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, r.UrltextforlinkbyLingua(lingua), r.Id.ToString(), r.CodiceTipologia.ToString()) + "\" >" + htmltext + "</a>";

                                    string linkurlsimple = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, r.UrltextforlinkbyLingua(lingua), r.Id.ToString(), r.CodiceTipologia.ToString());

                                    ra1 = new ResultAutocomplete() { id = r.Id.ToString(), label = shorttext, link = linkurlsimple, linktext = htmltext };
                                    if (id == null || id == "") lra.Add(ra1);
                                    else if (id != "" && r.Id.ToString() == id) lra.Add(ra1);
                                    count++;
                                    if (count > irecs) break;
                                }
                            }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(lra, Newtonsoft.Json.Formatting.Indented);
                    break;
                case "autocompletericerca":
                    long.TryParse(Recs, out irecs);
                    if (irecs == 0) irecs = 20;
                    if (term != "null")
                    {
                        offerteDM offDM = new offerteDM();
                        OfferteCollection coll = offDM.GetLista(term, irecs.ToString(), lingua, WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologia);
                        long count = 0;
                        ResultAutocomplete ra1 = new ResultAutocomplete() { id = "", label = "Deseleziona" };
                        lra.Add(ra1);
                        if (coll != null)
                            foreach (Offerte r in coll)
                            {
                                ra1 = new ResultAutocomplete() { id = r.Id.ToString(), label = r.DenominazionebyLingua(lingua) };
                                if (id == null || id == "") lra.Add(ra1);
                                else if (id != "" && r.Id.ToString() == id) lra.Add(ra1);
                                count++;
                                if (count > irecs) break;
                            }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(lra, Newtonsoft.Json.Formatting.Indented);
                    break;
                case "initreferencesdata":
                    result = references.initreferencesdataserialized(lingua, context.User.Identity.Name);
                    break;
                case "cercacontenuti":
                    string ssearchdata = pars.ContainsKey("data") ? pars["data"] : "";
                    if (!string.IsNullOrEmpty(ssearchdata))
                    {
                        string par1 = "";
                        string par2 = "";
                        string par3 = "";
                        string par4 = "";
                        Dictionary<string, string> searchdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(ssearchdata);
                        if (searchdata.ContainsKey("testoricerca"))
                            context.Session.Add("testoricerca", searchdata["testoricerca"].ToString());
                        if (searchdata.ContainsKey("categoria"))
                            par1 = searchdata["categoria"].ToString();
                        if (searchdata.ContainsKey("categoria2liv"))
                            par2 = searchdata["categoria2liv"].ToString();
                        string linkret = CommonPage.CreaLinkRicerca("", searchdata["tipologia"].ToString(), par1, par2, "", "", "", "-", lingua);
                        result = CommonPage.ReplaceAbsoluteLinks(linkret);
                    }
                    break;
                case "inviamessaggiomail":
                    string smaildata = pars.ContainsKey("data") ? pars["data"] : "";
                    Dictionary<string, string> maildata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(smaildata);

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Prepariamo e inviamo il mail
                    Dictionary<string, string> destinatariperregione = new Dictionary<string, string>();
                    string nomemittente = (maildata.GetValueOrDefault("name") ?? "");
                    string cognomemittente = (maildata.GetValueOrDefault("cognome") ?? "");
                    string ragsoc = (maildata.GetValueOrDefault("ragsoc") ?? "");
                    string mittenteMail = (maildata.GetValueOrDefault("email") ?? "");
                    string mittenteTelefono = (maildata.GetValueOrDefault("telefono") ?? "");
                    string message = (maildata.GetValueOrDefault("message") ?? "");
                    string location = (maildata.GetValueOrDefault("location") ?? "");
                    string regione = (maildata.GetValueOrDefault("regione") ?? "");
                    string adulti = (maildata.GetValueOrDefault("adulti") ?? "");
                    string persone = (maildata.GetValueOrDefault("persone") ?? "");
                    string bambini = (maildata.GetValueOrDefault("bambini") ?? "");
                    string arrivo = (maildata.GetValueOrDefault("arrivo") ?? "");
                    string partenza = (maildata.GetValueOrDefault("partenza") ?? "");
                    string orario = (maildata.GetValueOrDefault("orario") ?? "");
                    string location1 = (maildata.GetValueOrDefault("location1") ?? "");
                    string datarichiesta = (maildata.GetValueOrDefault("datarichiesta") ?? "");

                    string chkprivacy = (maildata.GetValueOrDefault("chkprivacy") ?? "");  // da form masterpage
                    if ((maildata.GetValueOrDefault("consenso") != null))
                        chkprivacy = (maildata.GetValueOrDefault("consenso") ?? ""); //da modulo iscrizione
                    string chknewsletter = (maildata.GetValueOrDefault("chknewsletter") ?? "");  // da form masterpage
                    if ((maildata.GetValueOrDefault("consenso1") != null))
                        chknewsletter = (maildata.GetValueOrDefault("consenso1") ?? ""); //da modulo iscrizione
                    bool spuntaprivacy = false;
                    bool spuntanewsletter = false;
                    bool.TryParse(chkprivacy, out spuntaprivacy);
                    bool.TryParse(chknewsletter, out spuntanewsletter);

                    string idofferta = (maildata.GetValueOrDefault("idofferta") ?? "");
                    string nomedestinatario = ConfigManagement.ReadKey("Nome");
                    string maildestinatario = ConfigManagement.ReadKey("Email");
                    string tipocontenuto = (maildata.GetValueOrDefault("tipocontenuto") ?? "");
                    long idperstatistiche = 0;
                    string tipo = (maildata.GetValueOrDefault("tipo") ?? "");
                    if (tipocontenuto == "Prenota")
                        tipo = "richiesta preventivo prenotazione ";
                    if (tipocontenuto == "Acquistousato")
                        tipo = "vendita usato ";

                    string SoggettoMail = "Richiesta " + tipo + " da " + ragsoc + " " + cognomemittente + " " + nomemittente + " tramite il sito " + ConfigManagement.ReadKey("Nome");
                    HtmlToText htmltotext = new HtmlToText();
                    string Descrizione = htmltotext.Convert(message).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br/>") + " <br/> ";
                    //string Descrizione = (message).Replace("\r", "<br/>") + " <br/> ";
                    if (idofferta != "") //Inseriamo il dettaglio della scheda di provenienza
                    {
                        offerteDM offDM = new offerteDM();
                        Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idofferta);
                        if (item != null && item.Id != 0)
                        {
                            long.TryParse(idofferta, out idperstatistiche);
                            if (!string.IsNullOrWhiteSpace(item.Email)) //Se non è vuota mando alla mail indicata nell'articolo
                            {
                                nomedestinatario = item.Email;
                                maildestinatario = item.Email;
                            }
                            Descrizione += "<br/><br/>";
                            Descrizione += "Pagina provenienza: " + item.DenominazioneI.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br/>") + " id:" + idperstatistiche;
                            Descrizione += "<br/><br/>";
                        }
                    }
                    if (tipocontenuto == "Richiesta")
                    {
                    }
                    if (tipocontenuto == "Prenota")
                    {
                        Descrizione += " <br/> Arrivo richiesto:" + arrivo + " Partenza Richiesta: " + partenza;
                        Descrizione += " <br/> Numero adulti:" + adulti + " <br/> Numero bambini:" + bambini + " Alloggio : " + location;
                    }

                    if (tipocontenuto == "Prenotawed")
                    {
                        Descrizione += " <br/> Data richiesta:" + datarichiesta + " Orario Richiesto: " + orario;
                    }
                    if (tipocontenuto == "Prenotaapt")
                    {
                        Descrizione += " <br/> Ristorante:" + location1;
                        Descrizione += " <br/> Persone:" + persone;
                        Descrizione += " <br/> Data richiesta:" + datarichiesta + " Orario Richiesto: " + orario;
                    }
                    Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome Cliente: " + cognomemittente;
                    Descrizione += " <br/> Azienda: " + ragsoc;
                    Descrizione += " <br/> Telefono Cliente: " + mittenteTelefono + "  Email Cliente: " + mittenteMail + " Lingua Cliente: " + lingua;
                    Descrizione += " <br/> Il cliente ha Confermato l'autorizzazione al trattamento dei dati personali. ";

                    if (spuntanewsletter == true)
                    {
                        Descrizione += " <br/> Il cliente ha richiesto l'invio newsletter. " + references.ResMan("Common", lingua, "titolonewsletter1").ToString() + "<br/>";

                        //SoggettoMail = "Richiesta iscrizione newsletter da " + nomemittente + " tramite il sito " + Nome;
                        //------------------------------------------------
                        //Memorizzo i dati nel cliente per la newsletter
                        //------------------------------------------------
                        ClientiDM cliDM = new ClientiDM();
                        Cliente cli = new Cliente();
                        string tipocliente = "0"; //Cliente standard per newsletter
                        //  cli.DataNascita = System.DateTime.Now.Date;
                        cli.Lingua = lingua;
                        cli.id_tipi_clienti = tipocliente;
                        cli.Nome = nomemittente.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                        cli.Cognome = cognomemittente.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                        cli.Ragsoc = ragsoc.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                        cli.Consenso1 = true;
                        cli.ConsensoPrivacy = true;
                        cli.Validato = true;
                        cli.Email = mittenteMail.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                        Cliente _clitmp = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email, tipocliente);
                        if ((_clitmp != null && _clitmp.Id_cliente != 0))
                            cli.Id_cliente = _clitmp.Id_cliente;
                        cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);
                    }

                    if (spuntaprivacy)
                    {
                        Utility.invioMailGenerico(ConfigManagement.ReadKey("Nome"), ConfigManagement.ReadKey("Email"), SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
                        //Utility.invioMailGenerico(nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
                        // Registro la statistica di contatto
                        Statistiche stat = new Statistiche();
                        stat.Data = DateTime.Now;
                        stat.EmailDestinatario = maildestinatario;
                        stat.EmailMittente = mittenteMail;
                        stat.Idattivita = idperstatistiche;
                        stat.Testomail = ragsoc + " " + nomemittente + "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                        stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                        stat.Url = "";
                        statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);
                        result = CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", lingua, "LinkContatti"));
                        if (idofferta != "") result += "&idOfferta=" + idofferta.ToString();
                        result += "&conversione=true";
                    }
                    else
                    {
                        result = references.ResMan("Common", lingua, "txtPrivacyError") + " <br/> Mancata Autorizzazione privacy"; ;
                        throw new ApplicationException(result);
                    }

                    break;
                case "insertanagraficaeinviamail":
                    string spasseddata = pars.ContainsKey("data") ? pars["data"] : "";
                    Dictionary<string, string> data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(spasseddata);
                    //string actlingua = (data.GetValueOrDefault("lingua") ?? ""); //viene gia passata

                    Cliente cliente = new Cliente();
                    //Dati Spedizione opzionali
                    Cliente clispediz = new Cliente(cliente);
                    clispediz.Cap = (data.GetValueOrDefault("caps") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    clispediz.Indirizzo = (data.GetValueOrDefault("indirizzos") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    clispediz.CodiceNAZIONE = (data.GetValueOrDefault("naziones") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    clispediz.CodiceREGIONE = (data.GetValueOrDefault("regiones") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    clispediz.CodicePROVINCIA = (data.GetValueOrDefault("provincias") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    clispediz.CodiceCOMUNE = (data.GetValueOrDefault("comunes") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    string cliserialized = Newtonsoft.Json.JsonConvert.SerializeObject(clispediz);
                    cliente.Serialized = cliserialized; //Appoggio i dati di spedizione in Serialized del cliente !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    cliente.Nome = (data.GetValueOrDefault("nome") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    cliente.Cognome = (data.GetValueOrDefault("cognome") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    cliente.Ragsoc = (data.GetValueOrDefault("ragsoc") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    cliente.Email = (data.GetValueOrDefault("email") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    cliente.Telefono = (data.GetValueOrDefault("telefono") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    cliente.Pivacf = (data.GetValueOrDefault("piva") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    cliente.Emailpec = (data.GetValueOrDefault("sdi") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    //Fatturazione
                    cliente.Cap = (data.GetValueOrDefault("cap") ?? "");
                    cliente.Indirizzo = (data.GetValueOrDefault("indirizzo") ?? "");
                    cliente.CodiceNAZIONE = (data.GetValueOrDefault("nazione") ?? "");
                    cliente.CodiceREGIONE = (data.GetValueOrDefault("regione") ?? "");
                    cliente.CodicePROVINCIA = (data.GetValueOrDefault("provincia") ?? "");
                    cliente.CodiceCOMUNE = (data.GetValueOrDefault("comune") ?? "");



                    string datiaggiuntivi = "";
                    //datiaggiuntivi += "Orario aperuta: " + (data.GetValueOrDefault("orario") ?? "");
                    //datiaggiuntivi += " Giorno chiusura: " + (data.GetValueOrDefault("chiusura") ?? "");
                    //datiaggiuntivi += " Quantità giornaliera caffè: " + (data.GetValueOrDefault("chiusura") ?? "");
                    //datiaggiuntivi += " Pranzi veloci: " + (data.GetValueOrDefault("chkpveloci") ?? "");
                    string generautente = (data.GetValueOrDefault("generautente") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                    string descrizione1 = (data.GetValueOrDefault("descrizione") ?? "");
                    string tipocontenuto1 = (data.GetValueOrDefault("tipocontenuto") ?? "");


                    string chkprivacy1 = (data.GetValueOrDefault("chkprivacy") ?? "");  // da form masterpage
                    if ((data.GetValueOrDefault("consenso") != null))
                        chkprivacy1 = (data.GetValueOrDefault("consenso") ?? ""); //da modulo iscrizione
                    string chknewsletter1 = (data.GetValueOrDefault("chknewsletter") ?? "");  // da form masterpage
                    if ((data.GetValueOrDefault("consenso1") != null))
                        chknewsletter1 = (data.GetValueOrDefault("consenso1") ?? ""); //da modulo iscrizione
                    bool spuntaprivacy1 = false;
                    bool spuntanewsletter1 = false;
                    bool.TryParse(chkprivacy1, out spuntaprivacy1);
                    bool.TryParse(chknewsletter1, out spuntanewsletter1);


                    string nomedestinatario1 = ConfigManagement.ReadKey("Nome");
                    string maildestinatario1 = ConfigManagement.ReadKey("Email");

                    //------------------------------------------------
                    //Memorizzo i dati nel cliente in anagrafica
                    //------------------------------------------------
                    //string tipocliente1 = "0"; //Cliente standard per newsletter ( cambiare il tipo cliente in base al valore desiderato o passarlo alla funzione di inserimento nei dati )
                    string tipocliente1 = (data.GetValueOrDefault("tipologia") ?? "").Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');

                    //cliente.DataNascita = System.DateTime.Now.Date;
                    cliente.Lingua = lingua;
                    cliente.id_tipi_clienti = tipocliente1;
                    cliente.Consenso1 = true;
                    cliente.ConsensoPrivacy = true;
                    cliente.Validato = true;
                    ClientiDM clidm = new ClientiDM();
                    Cliente _clitmp1 = clidm.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cliente.Email, tipocliente1);
                    if ((_clitmp1 != null && _clitmp1.Id_cliente != 0))
                        cliente.Id_cliente = _clitmp1.Id_cliente;
                    clidm.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cliente);
#if false
                    //Impostiamo  il codice sconto con valore zero per nuovo cliente inserito
                    cliente.Codicisconto = cliente.Id_cliente + "-sconto;0";
                    clidm.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cliente); 
#endif
#if false  //Aggiorn il cliente inserndo il codice sconto se richiesto
                    if (tipocontenuto1.ToLower() == "iscrizione wineclub") //
                    {
                        //Impostiamo  il codice sconto con valore zero per nuovo cliente inserito
                        cliente.Codicisconto = "wineclub-" + cliente.Id_cliente + ";0";
                        clidm.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cliente);
                    }
#endif
                    //------------------------------------------------
                    //Invio mail avviso al gestore  ( inseriamo tutti i dati dei form )
                    //------------------------------------------------
                    string SoggettoMail1 = "Richiesta " + tipocontenuto1 + " da " + cliente.Cognome + " tramite il sito " + ConfigManagement.ReadKey("Nome");
                    string Descrizione1 = descrizione1.Replace("\r", "<br/>") + " <br/> ";
                    Descrizione1 += " <br/> Il cliente ha richiesto : " + tipocontenuto1;
                    Descrizione1 += " <br/> Azienda:" + cliente.Ragsoc;
                    Descrizione1 += " <br/> Cognome cliente:" + cliente.Cognome + "<br/>Nome: " + cliente.Nome;
                    Descrizione1 += " <br/> Id anagrafica:" + cliente.Id_cliente;
                    Descrizione1 += " <br/> Telefono : " + cliente.Telefono + "  <br/>Email : " + cliente.Email;
                    Descrizione1 += " <br/> Piva : " + cliente.Pivacf + "  <br/>SDI/Pec : " + cliente.Emailpec + " <br/>Lingua : " + lingua;

                    string indirizzofatt = cliente.Indirizzo + "<br/>";
                    indirizzofatt += cliente.Cap + " " + cliente.CodiceCOMUNE + "  (" + references.NomeProvincia(cliente.CodicePROVINCIA, lingua) + ")<br/>";
                    indirizzofatt += "Nazione: " + cliente.CodiceNAZIONE + "<br/>";
                    Descrizione1 += " <br/><br/>Dati Fatturazione: <br/>" + indirizzofatt;

                    string indirizzosped = indirizzofatt;
                    if (!string.IsNullOrEmpty(clispediz.Indirizzo))
                    {
                        indirizzosped = clispediz.Indirizzo + "<br/>";
                        indirizzosped += clispediz.Cap + " " + clispediz.CodiceCOMUNE + "  (" + references.NomeProvincia(clispediz.CodicePROVINCIA, lingua) + ")<br/>";
                        indirizzosped += "Nazione: " + clispediz.CodiceNAZIONE + "<br/>";
                    }
                    Descrizione1 += " <br/>Dati Spedizione: <br/>" + indirizzosped;
                    Descrizione1 += " <br/> Info Aggiuntive:" + datiaggiuntivi;
                    Descrizione1 += " <br/> Il cliente ha Confermato l'autorizzazione al trattamento dei dati personali. ";
                    if (spuntanewsletter1 == true)
                    {
                        Descrizione1 += " <br/> Il cliente ha richiesto l'invio newsletter. " + references.ResMan("Common", lingua, "titolonewsletter1").ToString() + "<br/>";
                    }
                    Utility.invioMailGenerico(nomedestinatario1, maildestinatario1, SoggettoMail1, Descrizione1, maildestinatario1, nomedestinatario1);



#if false //abilitare per generazione utente ecommerce dopo invio del form
                    //////////////////////////////////////////////////////////////////////////////////////////////
                    //SE richiesto GENERIAMO L'UTENTE NEL MEMBERSHIP E INVIAMO UNA MAIL DI CONFERMA AL CLIENTE CON I DATI DI REGISTRAZIONE
                    //////////////////////////////////////////////////////////////////////////////////////////////
                    if (generautente == "1")
                    {
                        string password = "";
                        //Imposto lo username come da regola
                        string username = cliente.Id_cliente.ToString() + "-" + cliente.Email;
                        //Verifichiamo che con la mail usata non ci sia gia un utente del membership
                        if (USM.VerificaPresenzaUtente(username)) //utente esistente -> devo avvisare che presente!!!
                        {
                            result = "Utente " + username + " già registrato! Fare recupero della password o contattate il supporto per il recupero dell'accesso!"; //devo avvisare l'utente!
                        }
                        else //utente da creare ex novo
                        {
                            USM.CreaUtente(cliente.Id_cliente.ToString(), ref username, ref password, "Operatore");
                            ///////////////////////////////////////////////////////////
                            //INVIA MAIL REGISTRAZIONE ALL'UTENTE
                            string oggetto = references.ResMan("Common", lingua, "txtoggettocreateuser").ToString();
                            oggetto += " " + ConfigManagement.ReadKey("Nome");
                            string testo = references.ResMan("Common", lingua, "txttestocreateuser").ToString();
                            testo = testo.Replace("|NOME|", ConfigManagement.ReadKey("Nome"));
                            testo = testo.Replace("|CREDENZIALI|", "User: " + username + " Pass: " + password);
                            testo = testo.Replace("|LOGINPAGE|", "<a href=" + CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", lingua, "linklogin")) + ">" + CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", lingua, "linklogin")) + "</a>");
                            testo += references.ResMan("Common", lingua, "txtFooter").ToString();
                            Utility.invioMailGenerico(ConfigManagement.ReadKey("Nome"), ConfigManagement.ReadKey("Email"), oggetto, testo, cliente.Email, cliente.Cognome, null, "", true);
                            ///////////////////////////////////////////////////////////
                        }
                    } 
#endif
                    // Registro la statistica di contatto
                    Statistiche stat1 = new Statistiche();
                    stat1.Data = DateTime.Now;
                    stat1.EmailDestinatario = maildestinatario1;
                    stat1.EmailMittente = cliente.Email;
                    // stat.Idattivita = idperstatistiche;
                    stat1.Testomail = SoggettoMail1 + " <br/>- " + Descrizione1;
                    stat1.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                    stat1.Url = "";
                    statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat1);
                    if (string.IsNullOrEmpty(result))
                    {
                        result = CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", lingua, "LinkContatti"));
                        // if (idofferta != "") result += "&idOfferta=" + idofferta.ToString();
                        result += "&conversione=true";
                    }

                    break;
                case "putinsession":
                    context.Session.Add(Key, Value);
                    break;
                case "getfromsession":
                    if (context.Session[Key] != null)
                        result = context.Session[Key].ToString();
                    break;
                case "emptysession":
                    HttpContext.Current.Session.Clear();
                    result = "svuotata sessione";
                    break;
                case "caricaConfig":
                    List<ConfigItem> retconfig = new List<ConfigItem>();
                    retconfig = ConfigManagement.ReadAsList(filtri);
                    if (retconfig == null || retconfig.Count == 0)
                        retconfig.Add(new ConfigItem());
                    //Dictionary<string, string> dictconfig  =  filterDictionaryConfig(filtri);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(retconfig, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "updateconfig":
                    string itemdata = pars.ContainsKey("itemdata") ? pars["itemdata"] : "";
                    List<ConfigItem> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConfigItem>>(itemdata
                    , new JsonSerializerSettings()
                    {
                        DateFormatString = "dd/MM/yyyy HH:mm:ss",
                        //DateFormatString = "dd/MM/yyyy",
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    //Chiamiamo l'aggiornamento di tutti i dati
                    //ConfigDM cDM = new ConfigDM();
                    //result = cDM.AggiornaConfigList(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref list);
                    result = ConfigManagement.AggiornaConfigList(ref list);
                    break;
                case "caricaresources":
                    List<ResourceItem> retresource = new List<ResourceItem>();
                    retresource = ResourceManagement.ReadItemsByLingua(lingua);
                    if (retresource == null || retresource.Count == 0)
                        retresource.Add(new ResourceItem());

                    //Dictionary<string, string> dictconfig  =  filterDictionaryConfig(filtri);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(retresource, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "updateresources":
                    string itemdataresource = pars.ContainsKey("itemdata") ? pars["itemdata"] : "";
                    List<ResourceItem> listresource = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResourceItem>>(itemdataresource
                    , new JsonSerializerSettings()
                    {
                        DateFormatString = "dd/MM/yyyy HH:mm:ss",
                        NullValueHandling = NullValueHandling.Ignore,
                        //DateFormatString = "dd/MM/yyyy",
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    //Chiamiamo l'aggiornamento di tutti i dati
                    result = ResourceManagement.AggiornaResourceList(ref listresource);
                    break;
                case "initresources":/*Caricamento delle risorse di testo per lingua*/
                    //System.Globalization.CultureInfo ci = references.setCulture(lingua);
                    Dictionary<string, Dictionary<string, string>> dict = references.GetResourcesByLingua(lingua);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
                    break;
                //Inizializzazione varibili base per javascipt
                case "initmainvars":
                    var percorsocontenuti = WelcomeLibrary.STATIC.Global.PercorsoContenuti.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);// CommonPage.ReplaceAbsoluteLinks("~" + ConfigManagement.ReadKey("DataDir") + "/Files");
                    var percorsocomune = WelcomeLibrary.STATIC.Global.PercorsoComune.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione); ;// CommonPage.ReplaceAbsoluteLinks("~" + ConfigManagement.ReadKey("DataDir") + "/Common");

                    var jpath = new references.jpath();
                    jpath.percorsocomune = percorsocomune;
                    jpath.percorsocontenuti = percorsocontenuti;
                    jpath.usecdn = WelcomeLibrary.STATIC.Global.usecdn;

                    System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                             new System.Web.Script.Serialization.JavaScriptSerializer();
                    result = oSerializer.Serialize(jpath);
                    //Oppure
                    //result = Newtonsoft.Json.JsonConvert.SerializeObject(jpath);
                    break;
                //Caricamento JSON struttura generale Nazioni
                case "caricaJSONnazioni":
                    List<WelcomeLibrary.DOM.Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (WelcomeLibrary.DOM.Tabrif _nz) { return _nz.Lingua == lingua; });
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(nazioni);
                    break;
                //Caricamento JSON struttura generale province ----------------
                case "caricaJSONprovince":
                    List<WelcomeLibrary.DOM.Province> provincelingua1 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua); });
                    provincelingua1.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(provincelingua1);
                    break;
                //Caricamento JSON struttura generale province ----------------
                case "caricaJSONregioni":
                    WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
                    List<WelcomeLibrary.DOM.Province> provincelingua2 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua); });
                    if (provincelingua2 != null && provincelingua2.Count > 0)
                    {
                        provincelingua2.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (WelcomeLibrary.DOM.Province item in provincelingua2)
                        {
                            if (item.Lingua == lingua)
                                if (!regioni.Exists(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Regione == item.Regione); }))
                                    regioni.Add(item);
                        }
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(regioni);
                    break;
                //Caricamento JSON struttura generale COmuni
                case "caricaJSONcomuni":
                    List<WelcomeLibrary.DOM.Comune> comuniordered = new List<WelcomeLibrary.DOM.Comune>();
                    comuniordered.AddRange(Utility.ElencoComuni);
                    if (comuniordered != null)
                        comuniordered.Sort(new GenericComparer<WelcomeLibrary.DOM.Comune>("Nome", System.ComponentModel.ListSortDirection.Ascending));
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(comuniordered);
                    break;
                //Selezione lista delle nazioni--------------------------
                case "fillddlnazioni":
                    //Parametri di filtro e selezione
                    List<WelcomeLibrary.DOM.Tabrif> nazionifill = Utility.Nazioni.FindAll(delegate (WelcomeLibrary.DOM.Tabrif _nz) { return _nz.Lingua == lingua; });
                    nazionifill.Sort(new GenericComparer<WelcomeLibrary.DOM.Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));
                    //Campo1 -> value
                    //    Codice -> key
                    foreach (WelcomeLibrary.DOM.Tabrif r in nazionifill)
                        res.Add(r.Codice, r.Campo1);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    break;
                //Selezione lista delle regioni---------------------------
                case "fillddlregioni":
                    //Parametri di filtro e selezione

                    WelcomeLibrary.DOM.ProvinceCollection regionifill = new WelcomeLibrary.DOM.ProvinceCollection();
                    List<WelcomeLibrary.DOM.Province> provincelingua3 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua && tmp.SiglaNazione.ToLower() == filter1.ToLower()); });
                    if (provincelingua3 != null && provincelingua3.Count > 0)
                    {
                        provincelingua3.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (WelcomeLibrary.DOM.Province item in provincelingua3)
                        {
                            if (item.Lingua == lingua)
                                if (!regionifill.Exists(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Regione == item.Regione); }))
                                    regionifill.Add(item);
                        }
                    }
                    //Campo1 -> value
                    //Codice -> key
                    foreach (WelcomeLibrary.DOM.Province r in regionifill)
                        res.Add(r.Codice, r.Regione);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    break;
                //Selezione lista provinceie ----------------------------------------------------------
                case "fillddlprovince":
                    //Parametri di filtro e selezione

                    WelcomeLibrary.DOM.Province _tmp = Utility.ElencoProvince.Find(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua && tmp.Codice == filter2); });
                    if (_tmp != null)
                    {
                        List<WelcomeLibrary.DOM.Province> provincelingua4 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == lingua && tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == filter1.ToLower()); });
                        provincelingua4.Sort(new GenericComparer<WelcomeLibrary.DOM.Province>("Provincia", System.ComponentModel.ListSortDirection.Ascending));
                        //Campo1 -> value
                        //    Codice -> key
                        foreach (WelcomeLibrary.DOM.Province r in provincelingua4)
                            res.Add(r.Codice, r.Provincia);
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    }

                    break;
                //Selezione lista comuni ----------------------------------------------
                case "fillddlcomuni":
                    List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate (WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == filter2); });
                    if (comunilingua != null)
                        comunilingua.Sort(new GenericComparer<WelcomeLibrary.DOM.Comune>("Nome", System.ComponentModel.ListSortDirection.Ascending));
                    //Campo1 -> value
                    //    Codice -> key

                    foreach (WelcomeLibrary.DOM.Comune r in comunilingua)
                        res.Add(r.Nome, r.Nome);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
                    break;

                default:
                    break;
                case "caricaDati":
                    //////////////////////////////////////////////////////////////////////
                    //recupero i parametri che mi servono da objfiltro
                    //////////////////////////////////////////////////////////////////////                    
                    Dictionary<string, string> value = new Dictionary<string, string>();
                    value = offerteDM.filterData(lingua, filtri, page, pagesize, enablepager);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;
                case "caricahmtlbinded":
                    custombind cb = new custombind();
                    string htmlout = "";
                    Dictionary<string, string> filtripager = new Dictionary<string, string>();
                    filtripager.Add("page", page);
                    filtripager.Add("pagesize", pagesize);
                    filtripager.Add("enablepager", enablepager);
                    //htmlout = custombind.getbindedhtmlstring(filtri, filtripager, lingua, context.User.Identity.Name, context.Session);

                    String maincontainertext = "";
                    if (filtri.ContainsKey("maincontainertext")) maincontainertext = WelcomeLibrary.UF.dataManagement.DecodeFromBase64(filtri["maincontainertext"]);
                    //if (filtri.ContainsKey("maincontainertext")) maincontainertext = filtri["maincontainertext"].Replace("\\\"", "\"").Replace("|", "'");
                    //if (filtri.ContainsKey("maincontainertext")) maincontainertext = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(filtri["maincontainertext"]);
                    if (!string.IsNullOrEmpty(maincontainertext))
                    {
                        htmlout = cb.bind(maincontainertext, lingua, context.User.Identity.Name, context.Session, filtri, filtripager, context.Request);
                        jreturncontainerdata jr = new jreturncontainerdata();
                        jr.html = htmlout;
                        jr.jscommands = custombind.jscommands[context.Session.SessionID];
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(jr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None,
                        });
                        //Svuoto jscommands in memoria
                        custombind.jscommands.Remove(context.Session.SessionID);
                    }
                    break;
                case "getlinkbyid": //torna una dictionary per id, idname, idimg di valori che linkano delle schede in base all'id o idlist passato
                    Dictionary<string, string> filtriid = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> valueRet1 = new Dictionary<string, string>();
                    if (filtriid.ContainsKey("id"))
                        valueRet1 = offerteDM.getlinklist(lingua, filtriid["id"], context.Session.SessionID);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(valueRet1, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "getlinkbyfilters": //Mi crea un link con i custom filters ( da usare per la search personalizzata a catalogo )
                    Dictionary<string, string> filtriadded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    string linkcustom = WelcomeLibrary.UF.SitemapManager.getlinkbyfiltri(filtriadded, lingua);
                    result = linkcustom;
                    break;
                case "caricaMenuSezioni":
                    Dictionary<string, string> filtriMenu = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);

                    Dictionary<string, string> valueRet = new Dictionary<string, string>();
                    valueRet = SitemapManager.creaMenuSezioni(filtriMenu["min"], filtriMenu["max"], lingua);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(valueRet, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "caricaDatiBanner":
                    Dictionary<string, string> filtriBanner = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);

                    Dictionary<string, string> valueBan = new Dictionary<string, string>();
                    valueBan = bannersDM.filterDataBanner(lingua, filtriBanner, page, pagesize, enablepager, context.Session.SessionID);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(valueBan, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "caricaLinks2liv":

                    //////////////////////////////////////////////////////////////////////
                    //recupero i parametri che mi servono da objfiltro
                    //////////////////////////////////////////////////////////////////////
                    Dictionary<string, string> filtriCategorie = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> linksDictionary = new Dictionary<string, string>();
                    Dictionary<string, List<Tabrif>> mainDictionary = new Dictionary<string, List<Tabrif>>();
                    Tabrif elemlink = new Tabrif();

                    List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == lingua && (tmp.CodiceTipologia == filtriCategorie["tipologia"])); });
                    prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                    if (prodotti != null)
                    {
                        prodotti.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (Prodotto o in prodotti)
                        {
                            string testo = o.Descrizione;
                            //string linkcategoria = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, testo, "", o.CodiceTipologia, o.CodiceProdotto, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
                            //linkcategoria = linkcategoria.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                            List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == lingua && (tmp.CodiceProdotto == o.CodiceProdotto)); });
                            sprodotti.Sort(new GenericComparer<SProdotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                            if (sprodotti != null)
                            {
                                foreach (SProdotto s in sprodotti)
                                {
                                    string testosprod = s.Descrizione;
                                    //string linksprod = CommonPage.CreaLinkRoutes(null, false, lingua, (testosprod), "", filtriCategorie["tipologia"], s.CodiceProdotto, s.CodiceSProdotto);
                                    string linksprod = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, testosprod, "", filtriCategorie["tipologia"], s.CodiceProdotto, s.CodiceSProdotto, "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);
                                    linksprod = linksprod.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                                    elemlink = new Tabrif();
                                    elemlink.Codice = s.CodiceSProdotto;
                                    elemlink.Campo1 = linksprod;
                                    elemlink.Campo2 = testosprod;
                                    if (mainDictionary.ContainsKey(testo))
                                    {
                                        mainDictionary[testo].Add(elemlink);
                                    }
                                    else
                                    {
                                        List<Tabrif> tmpList = new List<Tabrif>();
                                        mainDictionary.Add(testo, tmpList);
                                        mainDictionary[testo].Add(elemlink);
                                    }
                                }
                            }


                        }
                    }

                    string serializedmaindictionary = Newtonsoft.Json.JsonConvert.SerializeObject(mainDictionary, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    linksDictionary.Add("data", serializedmaindictionary);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(linksDictionary, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;
                case "caricaLinks1liv":

                    //////////////////////////////////////////////////////////////////////
                    //recupero i parametri che mi servono da objfiltro
                    //////////////////////////////////////////////////////////////////////
                    Dictionary<string, string> filtriCategorie1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> linksDictionary1 = new Dictionary<string, string>();
                    Dictionary<string, List<Tabrif>> mainDictionary1 = new Dictionary<string, List<Tabrif>>();
                    Tabrif elemlink1 = new Tabrif();
                    //filtriCategorie1["tipologia"]
                    List<string> tipologie = new List<string>();
                    if (filtriCategorie1.ContainsKey("tipologia"))
                        tipologie = filtriCategorie1["tipologia"].Split('|').ToList<string>();
                    List<TipologiaOfferte> tlist = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => tipologie.Contains(t.Codice) && t.Lingua == lingua);
                    if (tlist != null)
                    {
                        tlist.Sort(new GenericComparer<TipologiaOfferte>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                        foreach (TipologiaOfferte t in tlist)
                        {
                            string testotipologia = t.Descrizione;
                            List<Prodotto> prodotti1 = Utility.ElencoProdotti.FindAll(p => p.Lingua == lingua && p.CodiceTipologia == t.Codice);
                            prodotti1.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                            if (prodotti1 != null)
                            {
                                prodotti1.Sort(new GenericComparer<Prodotto>("Descrizione", System.ComponentModel.ListSortDirection.Ascending));
                                foreach (Prodotto o in prodotti1)
                                {
                                    string testoprodotto = o.Descrizione;
                                    //string linkcategoria = CommonPage.CreaLinkRoutes(null, false, lingua, (testoprodotto), "", o.CodiceTipologia, o.CodiceProdotto);
                                    string linkcategoria = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, testoprodotto, "", o.CodiceTipologia, o.CodiceProdotto, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

                                    linkcategoria = linkcategoria.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                                    elemlink1 = new Tabrif();
                                    elemlink1.Codice = o.CodiceProdotto;
                                    elemlink1.Campo1 = linkcategoria;
                                    elemlink1.Campo2 = testoprodotto;
                                    if (mainDictionary1.ContainsKey(testotipologia))
                                    {
                                        mainDictionary1[testotipologia].Add(elemlink1);
                                    }
                                    else
                                    {
                                        List<Tabrif> tmpList = new List<Tabrif>();
                                        mainDictionary1.Add(testotipologia, tmpList);
                                        mainDictionary1[testotipologia].Add(elemlink1);
                                    }
                                }
                            }
                        }
                    }
                    string serializedmainDictionary1 = Newtonsoft.Json.JsonConvert.SerializeObject(mainDictionary1, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    linksDictionary1.Add("data", serializedmainDictionary1);

                    result = Newtonsoft.Json.JsonConvert.SerializeObject(linksDictionary1, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;
                case "caricaDatiArchivio":

                    //////////////////////////////////////////////////////////////////////
                    //recupero i parametri che mi servono da objfiltro
                    //////////////////////////////////////////////////////////////////////
                    Dictionary<string, string> filtriArchivio = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
                    Dictionary<string, string> valueArchivio = new Dictionary<string, string>();
                    Dictionary<string, List<Tabrif>> tmpArchivio = new Dictionary<string, List<Tabrif>>();
                    offerteDM offDM1 = new offerteDM();
                    Tabrif elem = new Tabrif();

                    Dictionary<string, Dictionary<string, string>> archivioperannomese = offDM1.ContaPerAnnoMese(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, lingua, filtriArchivio["tipologia"], filtriArchivio["categoria"]);
                    string link = "";
                    foreach (string anno in archivioperannomese.Keys)
                    {
                        foreach (string mese in archivioperannomese[anno].Keys)
                        {
                            elem = new Tabrif();
                            link = "";
                            link = CommonPage.CreaLinkRicerca("", filtriArchivio["tipologia"], filtriArchivio["categoria"], "", "", anno, mese, filtriArchivio["tipologia"], lingua, HttpContext.Current.Session);
                            elem.Codice = mese;
                            elem.Campo1 = link;
                            DateTime _d = new DateTime(Convert.ToInt16(anno), Convert.ToInt16(mese), 1);
                            elem.Campo2 = String.Format("{0:MMM yyyy}", _d) + "    (" + archivioperannomese[anno][mese] + ")";

                            if (tmpArchivio.ContainsKey(anno))
                            {
                                tmpArchivio[anno].Add(elem);
                            }
                            else
                            {
                                List<Tabrif> tmpList = new List<Tabrif>();
                                tmpArchivio.Add(anno, tmpList);
                                tmpArchivio[anno].Add(elem);
                            }
                        }
                    }
                    string tempOffArchivio = Newtonsoft.Json.JsonConvert.SerializeObject(tmpArchivio, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    });
                    valueArchivio.Add("data", tempOffArchivio);
                    //value = filterDataArchivio(lingua, filtriArchivio);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(valueArchivio, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    ////////////////////////////////////////////////////////////////////////////
                    break;
                case "getestatefromurl": /*Importazione datatbase immobiliare da gestionale*/

                    string Urlexport = WelcomeLibrary.STATIC.Global.percorsoapp + WelcomeLibrary.STATIC.Global.percorsoexp.TrimEnd('/') + ".zip";
                    string zippedfile = WelcomeLibrary.STATIC.Global.PercorsoComune + "/_export.zip";
                    zippedfile = context.Server.MapPath(zippedfile);
                    WelcomeLibrary.UF.SharedStatic.MakeHttpGet(Urlexport, zippedfile);

                    string destinationfilepath = context.Server.MapPath("~" + WelcomeLibrary.STATIC.Global.percorsoexp);
                    string originaldir = WelcomeLibrary.STATIC.Global.percorsoexp;
                    string temporarydir = WelcomeLibrary.STATIC.Global.percorsoexp.TrimEnd('/') + "tmp/";
                    string tmpdestinationfilepath = context.Server.MapPath("~" + temporarydir);

                    //Utilizzo una dir di appoggio di lavoro temporanea per evitare conflitti sulla lettura dei filese /public/common/_exporttmp/
                    if (!System.IO.Directory.Exists(tmpdestinationfilepath))
                        System.IO.Directory.CreateDirectory(tmpdestinationfilepath);
                    WelcomeLibrary.UF.Utility.UnZip(zippedfile, tmpdestinationfilepath, ""); //Unzip to temporary folder
                                                                                             //CAMBIO TEMPORANEAMENTE DIRECTORY DI LAVORO PER IL SITO PER EVITARE CONFILITTI DI LOCK PER I FILES
                    WelcomeLibrary.STATIC.Global.percorsoexp = temporarydir;///////////////
                    System.Threading.Thread.Sleep(2000); //Attendo eventuali richieste in corso da ultimare

                    //Scompattiamo i nuovi files nella directory finale
                    WelcomeLibrary.UF.Utility.UnZip(zippedfile, destinationfilepath, "");
                    //Reimposto la directory originale!!
                    WelcomeLibrary.STATIC.Global.percorsoexp = originaldir;///////////////
                    references.CaricaMemoriaStatica(context.Server); //Aggiorno la memoria statica se variata

                    System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
                    Messaggi.Add("Messaggio", "");
                    Messaggi["Messaggio"] += " Aggiornato correttamente immobili da gestionale " + System.DateTime.Now.ToString();
                    WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi);
                    break;
                case "creasitemapsresources":
                    //Da fare la gnerazione delle sitemap dal file estates.json
                    references.CreaSitemapImmobili(context.Server, "rif000666");
                    break;
                case "recuperapass":
                    result = USM.SendAccessData(lingua, pars.ContainsKey("username") ? pars["username"] : "", ConfigManagement.ReadKey("Email"), ConfigManagement.ReadKey("Nome"));
                    break;
                case "changename":
                    result = usermanager.setFirstName(pars.ContainsKey("username") ? pars["username"] : "", pars.ContainsKey("nome") ? pars["nome"] : "");
                    break;
                case "logoffuser":
                    System.Web.Security.FormsAuthentication.SignOut();
                    break;
                case "verificalogin":
                    string usernamelog = pars.ContainsKey("username") ? pars["username"] : "";
                    string passwordlog = pars.ContainsKey("password") ? pars["password"] : "";
                    if (System.Web.Security.Membership.ValidateUser(usernamelog, passwordlog))
                    {
                        System.Web.Security.FormsAuthentication.SetAuthCookie(usernamelog, false);
                        result = "";
                    }
                    else
                    {
                        result = "Accesso non riuscito. Se sei un nuovo utente, effettua la registrazione.";
                    }
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


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}

