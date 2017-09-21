using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using WelcomeLibrary.DAL;

namespace WelcomeLibrary.UF
{
    public static class SitemapManager
    {

        /// <summary>
        /// Rigenera tutti i link nella tabella di urlrewriting per le tipologie e per le categorie 1 e 2 livello
        /// </summary>
        public static void RigeneraLinkSezioniUrlrewrited()
        {
            foreach (TipologiaOfferte tipologia in WelcomeLibrary.UF.Utility.TipologieOfferte)
            {
                //WelcomeLibrary.DOM.TipologiaOfferte sezione =
                //    WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == tipologia.Lingua && tmp.Codice == tipologia.Codice); });
                //Genero il link per la tipologia
                WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, tipologia.Descrizione, "", tipologia.Codice, "", "", "", "", "", true, true);

                List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == tipologia.Lingua && (tmp.CodiceTipologia == tipologia.Codice)); });
                foreach (Prodotto p in prodotti)
                {
                    //Genero il link per la categoria
                    WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, p.Descrizione, "", tipologia.Codice, p.CodiceProdotto, "", "", "", "", true, true);

                    List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == tipologia.Lingua && (tmp.CodiceProdotto == p.CodiceProdotto)); });
                    foreach (SProdotto s in sprodotti)
                    {
                        //Genero il link per la sotto categoria
                        WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, s.Descrizione, "", tipologia.Codice, p.CodiceProdotto, s.CodiceSProdotto, "", "", "", true, true);
                    }
                }
            }
            /*GENERAZIONE LINK PER CARATTERISTICHE*/
            if (Utility.Caratteristiche.Count >= 3)
            {
                foreach (Tabrif elem in Utility.Caratteristiche[2])
                {
                    //switch (elem.Codice.Length)
                    //{
                    //    case 1:
                    //    case 3:
                    //    case 4:
                    //        Dictionary<string, string> addparms = new Dictionary<string, string>();
                    //        addparms.Add("Caratteristica3", elem.Codice);
                    //        string testolink = "";
                    //        testolink += " " + Utility.TestoCaratteristicaSublivelli(2, 1, elem.Codice, elem.Lingua);
                    //        testolink += " " + Utility.TestoCaratteristicaSublivelli(2, 2, elem.Codice, elem.Lingua);
                    //        testolink += " " + Utility.TestoCaratteristicaSublivelli(2, 3, elem.Codice, elem.Lingua);
                    //        WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(elem.Lingua, testolink, "", "rif000100", "", "", "", "", "", true, true, addparms);
                    //        break;
                    //}
                }
            }
        }




        /// <summary>
        /// Genera la lista dei link alle schede prodotto per la collection passata
        /// </summary>
        /// <param name="Collection"></param>
        /// <param name="Lingua"></param>
        /// <param name="percorsoBase"></param>
        /// <param name="stringabase"></param>
        /// <returns></returns>
        public static List<string> CreaLinksSchedeProdottoDaOfferte(OfferteCollection Collection, string Lingua, string percorsoBase, string stringabase, bool rigeneraUrlrewritetable = false)
        {
            List<string> ListaLink = new List<string>();
            //Mi Prendo la collection di immobili e mi ricavo una lista di stringhe
            if (Collection != null)
                foreach (Offerte _o in Collection)
                {
                    string UrlCompleto = "";
                    string testoperindice = _o.DenominazionebyLingua(Lingua);

                    UrlCompleto = CreaLinkRoutes(Lingua, testoperindice, _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, "", "", "", "", true, rigeneraUrlrewritetable);

                    ListaLink.Add(UrlCompleto);
                }
            return ListaLink;
        }

        public static string CreaLinkRoutes(string Lingua, string denominazione, string id, string codicetipologia, string codicecategoria = "", string codicecat2liv = "", string regione = "", string annofiltro = "", string mesefiltro = "", bool generaUrlrewrited = false, bool updateTableurlrewriting = false, Dictionary<string, string> addparms = null)
        {
            string destinationselector = "";
            Lingua = Lingua.ToUpper();
            string link = "";
            Tabrif urlRewrited = new Tabrif();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string cleandenominazione = ConteggioCaratteri(CleanUrl(denominazione.Trim().Replace(" ", "-")), 100).ToLower().Trim();
            // if (string.IsNullOrEmpty(cleandenominazione)) cleandenominazione = "-";

            if (!string.IsNullOrEmpty(codicetipologia) && codicetipologia == "con001000")
            {
                /////////////////////////////////////
                //Creo l'url per il rewriting
                /////////////////////////////////////
                destinationselector = "web";
                urlRewrited = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, cleandenominazione, id, "pagina", "", "", "", "", "", addparms);
            }
            else if (!string.IsNullOrWhiteSpace(id)) //Link a Scheda dettaglio!!
            {
                //string destinationselector = "";
                TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codicetipologia); });
                if (item != null)
                    destinationselector = ConteggioCaratteri(CleanUrl(item.Descrizione.Trim().Replace(" ", "-")), 20).ToLower().Trim();

                /////////////////////////////////////
                //Creo l'url per il rewriting
                /////////////////////////////////////
                if (codicetipologia == "rif000666") cleandenominazione += "-"; //Rendo unici i path immobili aggiungendo in coda     
                urlRewrited = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, cleandenominazione, id, "scheda");
            }
            else if (string.IsNullOrEmpty(codicetipologia) || codicetipologia.Length != 9)
            {
                //USATA IN caso di ricerca senza specifica di codice tipologia
                codicetipologia = "-";
                //link = "~/info/" + Lingua + "/" + codicetipologia + "/" + cleandenominazione;
                /////////////////////////////////////
                //Creo l'url per il rewriting
                ///////////////////////////////////// 
                destinationselector = "info";
                urlRewrited = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, cleandenominazione + "-s1", "", "lista", "", "", "", "", "", addparms);

                if (!string.IsNullOrEmpty(regione) || !string.IsNullOrEmpty(mesefiltro) || !string.IsNullOrEmpty(annofiltro))
                {
                    if (string.IsNullOrEmpty(regione)) regione = "-";
                    if (string.IsNullOrEmpty(mesefiltro)) mesefiltro = "-";
                    if (string.IsNullOrEmpty(annofiltro)) annofiltro = "-";
                    //link = "~/info/" + Lingua + "/" + codicetipologia + "/" + regione + "/" + annofiltro + "/" + mesefiltro + "/" + cleandenominazione;
                    /////////////////////////////////////
                    //Creo l'url per il rewriting
                    /////////////////////////////////////
                    urlRewrited = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, cleandenominazione + "-" + regione + "-" + annofiltro + "-" + mesefiltro, "", "lista", "", "", annofiltro, mesefiltro, regione, addparms);
                }
                if (!string.IsNullOrEmpty(codicecategoria) || !string.IsNullOrEmpty(codicecat2liv))
                {
                    if (string.IsNullOrEmpty(codicecategoria)) codicecategoria = "-";

                    //link = "~/info/" + Lingua + "/" + codicetipologia + "/" + codicecategoria + "/" + codicecat2liv + "/" + cleandenominazione;
                    /////////////////////////////////////
                    //Creo l'url per il rewriting
                    ///////////////////////////////////// 
                    urlRewrited = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, cleandenominazione + "-" + codicecategoria + "-" + codicecat2liv, "", "lista", codicecategoria, codicecat2liv, "", "", "", addparms);
                    ///////////////////////////////////// 
                }

            }
            else //PAGINE ELENCO
            {

#if false //TESTI URL DEFINITI IN MANIERA FISSA
                if (Convert.ToInt32(codicetipologia.Substring(3)) >= 666 && Convert.ToInt32(codicetipologia.Substring(3)) <= 666) //Lista resources
                {
                    destinationselector = "estate";
                }
                if (Convert.ToInt32(codicetipologia.Substring(3)) >= 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1) //Lista tipo catalogo
                {
                    destinationselector = "catalogo";
                    //link = "~/catalogo/" + Lingua + "/" + codicetipologia + "/" + cleandenominazione;
                }
                if (Convert.ToInt32(codicetipologia.Substring(3)) > 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 50) //Lista tipo blog
                {
                    destinationselector = "info";
                }
                if (Convert.ToInt32(codicetipologia.Substring(3)) >= 51 && Convert.ToInt32(codicetipologia.Substring(3)) <= 60) //Lista tipo elenco
                {
                    destinationselector = "list";
                }
                if (Convert.ToInt32(codicetipologia.Substring(3)) >= 101 && Convert.ToInt32(codicetipologia.Substring(3)) <= 101) //Lista tipo elenco
                {
                    destinationselector = "list";
                }
                if (Convert.ToInt32(codicetipologia.Substring(3)) >= 199 && Convert.ToInt32(codicetipologia.Substring(3)) <= 199) //Lista tipo elenco
                {
                    destinationselector = "list";
                }
                if (Convert.ToInt32(codicetipologia.Substring(3)) >= 1000 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1000) //forum
                {
                    destinationselector = "forum";
                } 
#endif

                //TESTI URL IN BASE ALLE TIPOLOGIE
                //string destinationselector = "";
                TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codicetipologia); });
                if (item != null)
                    destinationselector = ConteggioCaratteri(CleanUrl(item.Descrizione.Trim().Replace(" ", "-")), 20).ToLower().Trim();


                //Creiamo il link riscritto base
                if (string.IsNullOrEmpty(cleandenominazione)) cleandenominazione = "-";
                urlRewrited = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, cleandenominazione, "", "lista", "", "", "", "", "", addparms);
                //Modifico il link nel caso di link con categorie/sottocategorie o parametri per fare una personalizzazione del testo
                Tabrif urlRewritedModified = creaUrlListaModified(destinationselector, cleandenominazione, codicetipologia, codicecategoria, Lingua, codicecat2liv, regione, annofiltro, mesefiltro, addparms);

                if (urlRewritedModified != null)
                    urlRewrited = urlRewritedModified;
            }

            //link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
            //if (generaUrlrewrited) //SE devo tornare l'url riscritto -> modifico il valore di ritorno della chiamata
            //{
            link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + urlRewrited.Codice;
            //}
            if (updateTableurlrewriting) //Se devo fare l'update della tabella di rewriting -> cancello i valori precedenti e riscrivo i nuovi
            {
                //Aggiorno o inserisco nella tabella urlrwwriting
                InserisciAggiornaUrlrewrite(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, urlRewrited);
            }
            return link;
        }

        public static Tabrif creaUrlListaModified(string destinationselector, string cleandenominazione, string codicetipologia, string codicecategoria, string Lingua, string codicecat2liv, string regione, string annofiltro, string mesefiltro, Dictionary<string, string> addparms = null)
        {
            Tabrif ret = null;
            string testounicolink = cleandenominazione;
            string testomodificatore1 = "";
            Prodotto categoriaprodotto = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == codicetipologia && p.CodiceProdotto == codicecategoria && p.Lingua == Lingua);
            if (categoriaprodotto != null)
            {
                string tmps = CleanUrl(categoriaprodotto.Descrizione.Trim());
                if (!testounicolink.ToLower().Contains(tmps.ToLower()))
                    testomodificatore1 += tmps;
            }

            SProdotto categoria2liv = Utility.ElencoSottoProdotti.Find(delegate (SProdotto p) { return p.CodiceProdotto == codicecategoria && p.CodiceSProdotto == codicecat2liv && p.Lingua == Lingua; });
            if (categoria2liv != null)
            {
                string tmps = CleanUrl(categoria2liv.Descrizione.Trim());
                if (!testounicolink.ToLower().Contains(tmps.ToLower()))
                    testomodificatore1 += tmps;
            }

            Province item = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == regione); });
            if (item != null)
                testomodificatore1 += CleanUrl(item.Regione.Trim());
            testomodificatore1 += CleanUrl(annofiltro.Trim());
            testomodificatore1 += CleanUrl(mesefiltro.Trim());

            string testomodificatore2 = "";
            testomodificatore2 += codicecategoria.Replace("prod", "").TrimStart('0');
            testomodificatore2 += codicecat2liv.Replace("sprod", "").TrimStart('0');
            testomodificatore2 += regione;
            testomodificatore2 += annofiltro;
            testomodificatore2 += mesefiltro;

            if (addparms != null)
                foreach (KeyValuePair<string, string> kv in addparms)
                {
                    //testomodificatore1 += CleanUrl(kv.Key);
                    testomodificatore2 += CleanUrl(kv.Value);
                }

            if (!string.IsNullOrEmpty(testomodificatore2))
            {
                if (!string.IsNullOrEmpty(testomodificatore1))
                    //testounicolink += "-" + testomodificatore1;
                    testounicolink = (testomodificatore1 + "-" + testounicolink).Trim('-');
                if (!string.IsNullOrEmpty(testomodificatore2))
                    testounicolink += "-" + testomodificatore2;


                /////////////////////////////////////
                //Creo l'url per il rewriting ( nei vari casi per gli elenchi modifico il testo della chiamata a seconda dei parametri )
                ///////////////////////////////////// 
                ret = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, testounicolink, "", "lista", codicecategoria, codicecat2liv, annofiltro, mesefiltro, regione, addparms);
            }
            return ret;
        }


        public static Tabrif GeneraRewritingElement(string Lingua, string Tipologia, string destinationselector, string textmatch, string id = "", string tipopagina = "lista", string Categoria = "", string Categoria2liv = "", string anno = "", string mese = "", string regione = "", Dictionary<string, string> addparms = null)
        {
            Tabrif urlRewrited = new Tabrif();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("Lingua", Lingua.ToUpper());
            if (Tipologia.ToLower().StartsWith("rif"))
            {
                parameters.Add("Tipologia", Tipologia);
                parameters.Add("idOfferta", id);
            }
            else if (Tipologia.ToLower().StartsWith("con"))
            {
                parameters.Add("idContenuto", id);
                parameters.Add("CodiceContenuto", Tipologia);
            }
            parameters.Add("Categoria", Categoria);
            parameters.Add("Categoria2liv", Categoria2liv);
            parameters.Add("anno", anno);
            parameters.Add("mese", mese);
            parameters.Add("Regione", regione);
            if (addparms != null)
                foreach (KeyValuePair<string, string> kv in addparms)
                {
                    parameters.Add(kv.Key, kv.Value);
                }

            string cleantextmatch = ConteggioCaratteri(CleanUrl(textmatch.Trim().Replace(" ", "-")), 100).ToLower().Trim();
            if (string.IsNullOrEmpty(cleantextmatch)) cleantextmatch = "-";
            urlRewrited = CreaElementoRewriting(
                CostruisciRewritedUrl(Lingua, destinationselector, cleantextmatch, id)
                , OriginalPathdestinazioneByTipologia(Tipologia, tipopagina), Creaparametersstring(parameters));

            return urlRewrited;
        }


        //public static Tabrif GeneraRewritingElement(string Lingua, string Tipologia, string destinationselector, string textmatch, string id = "", string tipopagina = "lista", string Categoria = "", string Categoria2liv = "", string anno = "", string mese = "", string regione = "")
        //{
        //    Tabrif urlRewrited = new Tabrif();
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();

        //    parameters.Add("Lingua", Lingua.ToUpper());
        //    if (Tipologia.ToLower().StartsWith("rif"))
        //    {
        //        parameters.Add("Tipologia", Tipologia);
        //        parameters.Add("idOfferta", id);
        //    }
        //    else if (Tipologia.ToLower().StartsWith("con"))
        //    {
        //        parameters.Add("idContenuto", id);
        //        parameters.Add("CodiceContenuto", Tipologia);
        //    }
        //    else
        //    {
        //        parameters.Add("Tipologia", Tipologia);
        //        parameters.Add("idOfferta", id);
        //    }


        //    parameters.Add("Categoria", Categoria);
        //    parameters.Add("Categoria2Liv", Categoria2liv);
        //    parameters.Add("anno", anno);
        //    parameters.Add("mese", mese);
        //    parameters.Add("Regione", regione);
        //    urlRewrited = CreaElementoRewriting(
        //        CostruisciRewritedUrl(Lingua, destinationselector, textmatch, id)
        //        , OriginalPathdestinazioneByTipologia(Tipologia, tipopagina), Creaparametersstring(parameters));

        //    return urlRewrited;
        //}





        public static string ConteggioCaratteri(string testo, int caratteri = 600, bool nolink = false, string testoAggiunto = "")
        {
            string ritorno = testo;

            if (testo.Length > caratteri)
            {
                int invio = testo.IndexOf(" ", caratteri);

                if (nolink)
                {
                    if (invio != -1)
                        ritorno = testo.Substring(0, invio) + "";
                    else
                        ritorno = testo.Substring(0, caratteri) + "";
                }
                else
                {
                    if (invio != -1)
                        ritorno = testo.Substring(0, invio) + " " + testoAggiunto;
                    else
                        ritorno = testo.Substring(0, caratteri) + " " + testoAggiunto;
                    // references.ResMan("Common",Lingua,"testoContinua").ToString()
                }
            }
            return ritorno;
        }
        public static String CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            //return Regex.Replace(strIn, @"[^\w\.@-]", "");
            // strIn = Regex.Replace(strIn, @"[\W]", "");
            //strIn = strIn.Replace(" ", "-");
            strIn = Regex.Replace(strIn, @"[^a-zA-Zа-яА-ЯЁё0-9@\$=!:.’#%_?^<>()òàùèì &°:;-]", "");

            return strIn;
        }
        public static String CleanUrl(string strIn)
        {
            strIn = strIn.Trim();
            // Replace invalid characters with empty strings.
            //return Regex.Replace(strIn, @"[^\w\.@-]", "");
            // strIn = Regex.Replace(strIn, @"[\W]", "");
            strIn = strIn.Replace(" ", "-");
            strIn = strIn.Replace("\r", "-");
            strIn = strIn.Replace("\n", "");
            strIn = strIn.Replace("à", "a");
            strIn = strIn.Replace("è", "e");
            strIn = strIn.Replace("ì", "i");
            strIn = strIn.Replace("ò", "o");
            strIn = strIn.Replace("ù", "u");
            // strIn = Regex.Replace(strIn, @"[^a-zA-Z0-9@\_]", "");
            strIn = Regex.Replace(strIn, @"[^a-zA-Zа-яА-ЯЁё0-9@\$=_()-]", "");
            return strIn.Trim('-');
        }

        /// <summary>
        /// Mappa il giusto percorso di destinazione in base al codice di tipologia per inserire il pathdestinazione corretto nella TBL_URLREWRITING
        /// per le varie tipologie di pagine da chiamara ( scheda, lista etc... )
        /// </summary>
        /// <param name="codicetipologia"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static string OriginalPathdestinazioneByTipologia(string codicetipologia, string tipopagina = "scheda")
        {
            string Pathdestinazione = "";
            switch (tipopagina)
            {
                case "pagina": //Route pagine statiche
                    Pathdestinazione = "~/AspNetPages/Content_tipo1.aspx";
                    break;
                case "scheda": //Route schede aritcoli
                    Pathdestinazione = "~/AspNetPages/SchedaOffertaMaster.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) > 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 50)
                        Pathdestinazione = "~/AspNetPages/SchedaOffertaMaster.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1) //
                        Pathdestinazione = "~/AspNetPages/SchedaProdotto.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 51 && Convert.ToInt32(codicetipologia.Substring(3)) <= 60) //No Scheda apribile
                        Pathdestinazione = "";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 61 && Convert.ToInt32(codicetipologia.Substring(3)) <= 62)
                        Pathdestinazione = "~/AspNetPages/SchedaOffertaMaster.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 101 && Convert.ToInt32(codicetipologia.Substring(3)) <= 101) //
                        Pathdestinazione = "~/AspNetPages/SchedaProdotto.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 199 && Convert.ToInt32(codicetipologia.Substring(3)) <= 199) //No Scheda apribile
                        Pathdestinazione = "";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1000 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1000) //No Scheda apribile
                        Pathdestinazione = "";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 666 && Convert.ToInt32(codicetipologia.Substring(3)) <= 666)
                        Pathdestinazione = "~/AspNetPages/SchedaResource.aspx";
                    break;
                case "lista": //Route liste ricerche
                    Pathdestinazione = "~/AspNetPages/RisultatiRicerca.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1) //
                        Pathdestinazione = "~/AspNetPages/RisultatiProdotti.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) > 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 50)
                        Pathdestinazione = "~/AspNetPages/RisultatiRicerca.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 51 && Convert.ToInt32(codicetipologia.Substring(3)) <= 60) //
                        Pathdestinazione = "~/AspNetPages/ListaElenco.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 61 && Convert.ToInt32(codicetipologia.Substring(3)) <= 62) //
                        Pathdestinazione = "~/AspNetPages/ListaElenco.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 101 && Convert.ToInt32(codicetipologia.Substring(3)) <= 101) //
                        Pathdestinazione = "~/AspNetPages/ListaElenco.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 199 && Convert.ToInt32(codicetipologia.Substring(3)) <= 199) //
                        Pathdestinazione = "~/AspNetPages/RisultatiRicerca.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1000 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1000) //
                        Pathdestinazione = "~/AreaRiservata/Forum.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 666 && Convert.ToInt32(codicetipologia.Substring(3)) <= 666)
                        Pathdestinazione = "~/AspNetPages/RisultatiResource.aspx";
                    break;
                default:
                    Pathdestinazione = "~/AspNetPages/RisultatiRicerca.aspx";
                    break;
            }
            return Pathdestinazione;
        }


        public static Tabrif CreaElementoRewriting(string calledurl, string pathdestinazione, string parametri)
        {
            Tabrif urlRewrited = new Tabrif();
            urlRewrited.Codice = calledurl;
            urlRewrited.Campo1 = pathdestinazione;
            urlRewrited.Campo2 = parametri;
            return urlRewrited;

        }

        /// <summary>
        /// Costruisce l'url riscritto per impostare il calledurl da matchare nella TBL_URLREWRITING
        /// </summary>
        /// <param name="Lingua"></param>
        /// <param name="destinationselector"></param>
        /// <param name="textmatch"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static string CostruisciRewritedUrl(string Lingua = "I", string destinationselector = "", string textmatch = "-", string id = "")
        {
            string rewritedurl = Lingua;
            if (!string.IsNullOrEmpty(destinationselector))
                rewritedurl += "/" + destinationselector;
            rewritedurl += "/" + textmatch;
            if (!string.IsNullOrEmpty(id))
                rewritedurl += "-" + id;

            return rewritedurl;
        }

        /// <summary>
        /// Torna Id=Id,Codice=Calledurl,campo1=Pathdestinazione,campo2=Parametri
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="calledurl"></param>
        /// <returns></returns>
        public static Tabrif GetUrlRewriteaddress(string connection, string calledurl)
        {
            if (connection == null || connection == "") return null;
            if (calledurl == null || calledurl == "") return null;
            Tabrif item = new Tabrif();

            try
            {
                string query = "SELECT * FROM TBL_URLREWRITING WHERE Calledurl=@Calledurl";
                List<OleDbParameter> parColl = new List<OleDbParameter>();
                OleDbParameter p1 = new OleDbParameter("@Calledurl", calledurl);//OleDbType.VarChar
                parColl.Add(p1);
                OleDbDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        Tabrif _item = new Tabrif();

                        _item = new Tabrif();
                        _item.Id = reader.GetInt32(reader.GetOrdinal("ID")).ToString();
                        if (!reader["Relatedid"].Equals(DBNull.Value))
                            _item.Intero1 = reader.GetInt32(reader.GetOrdinal("Relatedid"));
                        _item.Codice = reader.GetString(reader.GetOrdinal("Calledurl")).ToString().Trim();
                        _item.Campo1 = reader.GetString(reader.GetOrdinal("Pathdestinazione")).Trim();
                        _item.Campo2 = reader.GetString(reader.GetOrdinal("Parametri")).Trim();


                        //  Dictionary<string, string> keyvalues = WelcomeLibrary.UF.SitemapManager.SplitParameters(_item.Campo2);

                        item = _item;
                    }
                }


            }
            catch (Exception error)
            {
                // throw new ApplicationException("Errore Caricamento tabella urlrewriting :" + error.Message, error);
            }
            return item;
        }
        public static void InserisciAggiornaUrlrewrite(string connessione, Tabrif item)
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return;
            if (string.IsNullOrEmpty(item.Codice.Trim()) || string.IsNullOrEmpty(item.Campo1.Trim())) return;

            //Se presente carico l'elemento nel db per l'aggiornamento
            Tabrif itemindb = GetUrlRewriteaddress(connessione, item.Codice);
            if (itemindb != null)
            {
                item.Id = itemindb.Id;
            }
            OleDbParameter p1 = new OleDbParameter("@Calledurl", item.Codice);
            parColl.Add(p1);
            OleDbParameter p2 = new OleDbParameter("@Pathdestinazione", item.Campo1);
            parColl.Add(p2);
            OleDbParameter p2b = new OleDbParameter("@Parametri", item.Campo2);
            parColl.Add(p2b);

            int i = 0;
            if (item.Intero1 != null)
                i = item.Intero1.Value;
            OleDbParameter p2c = new OleDbParameter("@Relatedid", i);
            parColl.Add(p2c);

            string query = "";
            if (item.Id != "")
            {
                //Update
                query = "UPDATE [TBL_URLREWRITING] SET Calledurl=@Calledurl,Pathdestinazione=@Pathdestinazione,Parametri=@Parametri,Relatedid=@Relatedid";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_URLREWRITING (Calledurl,Pathdestinazione,Parametri,Relatedid";
                query += " )";
                query += " values ( ";
                query += "@Calledurl,@Pathdestinazione,@Parametri,@Relatedid";
                query += " )";
            }

            try
            {
                dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                //  throw new ApplicationException("Errore, inserimento/aggiornamento urlrewrite :" + error.Message, error);
            }
            return;
        }
        public static int EliminaUrlrewrite(string connessione, Tabrif item)
        {
            int idret = -1;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_URLREWRITING WHERE ( Calledurl = @Calledurl ) ";
            OleDbParameter p1;
            p1 = new OleDbParameter("@Calledurl", item.Codice);
            parColl.Add(p1);
            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, eliminazione Mail da presa in carico:" + error.Message, error);
            }
            return idret;
        }
        /// <summary>
        /// Elimina gli urlrewrided in tabella in base all'id
        /// </summary>
        /// <param name="connessione"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int EliminaUrlrewritebyIdOfferta(string connessione, string id)
        {
            int idret = -1;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_URLREWRITING WHERE ( ( Calledurl like @Calledurl or Parametri like @Parametroid ) and ( Parametri not like @Parametroexcludi )) ";
            OleDbParameter p1;
            p1 = new OleDbParameter("@Calledurl", "%" + id);
            parColl.Add(p1);
            OleDbParameter p2;
            p2 = new OleDbParameter("@Parametroid", "%;idOfferta," + id + ";%");
            parColl.Add(p2);
            OleDbParameter pexc;
            pexc = new OleDbParameter("@Parametroexcludi", "%Tipologia,rif000666%");
            parColl.Add(pexc);

            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, eliminazione Mail da presa in carico:" + error.Message, error);
            }
            return idret;
        }


        public static int EliminaUrlrewritebyIdContenuto(string connessione, string id)
        {
            int idret = -1;
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_URLREWRITING WHERE ( Calledurl like @Calledurl or Parametri like @Parametroid ) ";
            OleDbParameter p1;
            p1 = new OleDbParameter("@Calledurl", "%" + id);
            parColl.Add(p1);
            OleDbParameter p2;
            p2 = new OleDbParameter("@Parametroid", "%;idContenuto," + id + ";%");
            parColl.Add(p2);
            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch (Exception error)
            {
                //throw new ApplicationException("Errore, eliminazione Mail da presa in carico:" + error.Message, error);
            }
            return idret;
        }


        public static Dictionary<string, string> SplitParameters(string item)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] coppie = item.Split(';');
            foreach (string s in coppie)
            {
                string[] keyval = s.Split(',');
                if (keyval != null && keyval.Length == 2)
                {
                    if (!dict.ContainsKey(keyval[0]) && !string.IsNullOrEmpty(keyval[0].Trim()) && !string.IsNullOrEmpty(keyval[1].Trim()))
                        dict.Add(keyval[0], keyval[1]);
                }
            }
            return dict;
        }
        /// <summary>
        /// Dal dictionary dei parametri ricostruisce la stringa per la tabella di rewriting
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string Creaparametersstring(Dictionary<string, string> dict)
        {
            string parametri = "";

            foreach (KeyValuePair<string, string> kv in dict)
            {
                parametri += kv.Key.Replace(",", "").Replace(";", "") + "," + kv.Value.Replace(",", "").Replace(";", "") + ";";
            }
            parametri = parametri.TrimEnd(';');
            return parametri;
        }
    }
}