using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WelcomeLibrary.DOM;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using WelcomeLibrary.DAL;
using Newtonsoft.Json;

namespace WelcomeLibrary.UF
{
    public static class SitemapManager
    {
        /// <summary>
        /// Check nella tablella reindirizzamenti per presenza redirect
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="urltoredirect"></param>
        /// <returns></returns>
        public static string TestRedirect(string connection, string urltoredirect)
        {
            string ret = "";

            if (connection == null || connection == "") { return null; };
            //  if (urltoredirect == null || urltoredirect == "") { return null; };

            string query = "SELECT * FROM TBL_redirect where originalUrl like @urltoredirect";
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            SQLiteParameter p1 = new SQLiteParameter("@urltoredirect", urltoredirect);//OleDbType.VarChar
            parColl.Add(p1);
            SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
            Tabrif item = new Tabrif();
            using (reader)
            {
                if (reader == null) { return null; };
                if (reader.HasRows == false)
                    return null;
                while (reader.Read())
                {
                    item = new Tabrif();
                    item.Id = reader.GetInt64(reader.GetOrdinal("ID")).ToString();

                    if (!reader["originalUrl"].Equals(DBNull.Value))
                        item.Campo1 = reader.GetString(reader.GetOrdinal("originalUrl"));
                    if (!reader["redirectedUrl"].Equals(DBNull.Value))
                        item.Campo2 = reader.GetString(reader.GetOrdinal("redirectedUrl"));
                    ret = item.Campo2;
                }
            }

            return ret;
        }
        public static string GeneraBackLink(string tipologia, string categoria, string lingua, bool usacategoria = true)
        {
            string ret = "";
            TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == lingua && tmp.Codice == tipologia); });
            if (item != null)
            {
                string testourl = item.Descrizione;
                Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == lingua && (tmp.CodiceTipologia == tipologia && tmp.CodiceProdotto == categoria)); });
                if (catselected != null && usacategoria)
                    testourl = catselected.Descrizione;
                string tmpcategoria = categoria;
                if (!usacategoria) tmpcategoria = "";

                //bool.TryParse(ConfigManagement.ReadKey("generaUrlrewrited"), out bool gen);
                ret = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, (testourl), "", tipologia, tmpcategoria, "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

            }
            return ret;
        }
        public static Dictionary<string, string> creaMenuSezioni(string min, string max, string lingua)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            int Min = Convert.ToInt32(min);
            int Max = Convert.ToInt32(max);
            List<WelcomeLibrary.DOM.TipologiaOfferte> sezioni = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte temp) { return (temp.Lingua == lingua); });
            sezioni.RemoveAll(t => Convert.ToInt32(t.Codice.Substring(3)) < Min || Convert.ToInt32(t.Codice.Substring(3)) > Max);
            sezioni.Sort(new GenericComparer<TipologiaOfferte>("Codice", System.ComponentModel.ListSortDirection.Descending));

            string tempTpo = Newtonsoft.Json.JsonConvert.SerializeObject(sezioni, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            });
            ret.Add("data", tempTpo);

            Dictionary<string, string> tmp = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, string>> linksurl = new Dictionary<string, Dictionary<string, string>>();
            foreach (TipologiaOfferte _o in sezioni)
            {
                //bool.TryParse(ConfigManagement.ReadKey("generaUrlrewrited"), out bool gen);
                string link = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, CleanUrl(_o.Descrizione), "", _o.Codice, "", "", "", "", "", true, WelcomeLibrary.STATIC.Global.UpdateUrl);

                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && link.ToLower().IndexOf("~") == -1)
                {
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }
                link = link.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                tmp = new Dictionary<string, string>();
                tmp.Add("link", link);
                tmp.Add("titolo", _o.Descrizione);

                linksurl.Add(_o.Codice, tmp);
            }
            string retlinksurl = Newtonsoft.Json.JsonConvert.SerializeObject(linksurl);
            ret.Add("linkloaded", retlinksurl);

            return ret;
        }


        /// <summary>
        /// Rigenera tutti i link nella tabella di urlrewriting per le tipologie e per le categorie 1 e 2 livello
        /// </summary>
        public static List<string> RigeneraLinkSezioniUrlrewrited(string Lingua = "", string removetipologie = "")
        {
            List<string> listalinks = new List<string>();
            List<TipologiaOfferte> listfiltered = WelcomeLibrary.UF.Utility.TipologieOfferte;
            if (!string.IsNullOrEmpty(Lingua))
                listfiltered = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => t.Lingua == Lingua);
            if (!string.IsNullOrEmpty(removetipologie))
            {
                string[] arrtipologie = removetipologie.ToLower().Split(',');
                List<string> ltipidarimuovere = arrtipologie.ToList<string>();
                if (ltipidarimuovere != null)
                {
                    foreach (string _t in ltipidarimuovere)
                    {
                        listfiltered.RemoveAll(t => t.Codice == _t);
                    }
                }
            }

            foreach (TipologiaOfferte tipologia in listfiltered)
            {
                //WelcomeLibrary.DOM.TipologiaOfferte sezione =
                //    WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == tipologia.Lingua && tmp.Codice == tipologia.Codice); });
                //Genero il link per la tipologia
                listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, tipologia.Descrizione, "", tipologia.Codice, "", "", "", "", "", true, true));
                List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == tipologia.Lingua && (tmp.CodiceTipologia == tipologia.Codice)); });
                foreach (Prodotto p in prodotti)
                {
                    //Genero il link per la categoria
                    listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, p.Descrizione, "", tipologia.Codice, p.CodiceProdotto, "", "", "", "", true, true));
                    List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == tipologia.Lingua && (tmp.CodiceProdotto == p.CodiceProdotto)); });
                    foreach (SProdotto s in sprodotti)
                    {
                        //Genero il link per la sotto categoria
                        listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, s.Descrizione, "", tipologia.Codice, p.CodiceProdotto, s.CodiceSProdotto, "", "", "", true, true));
                    }
                }
            }
#if true
            TipologiaOfferte tipologiapercatalogo = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(t => t.Codice == "rif000001" && t.Lingua == Lingua);
            /*GENERAZIONE LINK PER CARATTERISTICHE   ( da estendere la generazione per caratteristiche e combinazioni usate nei menu si fa solo per il catalogo )*/
            if (Utility.Caratteristiche.Count >= 1)
                foreach (Tabrif elem in Utility.Caratteristiche[0])
                {
                    Dictionary<string, string> addpars = new Dictionary<string, string>();
                    if (elem != null && !string.IsNullOrEmpty(elem.Codice) && elem.Lingua == Lingua)
                    {
                        addpars.Add("Caratteristica1", elem.Codice);
                        //foreach (TipologiaOfferte tipologia in listfiltered)
                        if (tipologiapercatalogo != null)
                        {
                            //Genero il link per la tipologia
                            listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologiapercatalogo.Lingua, tipologiapercatalogo.Descrizione, "", tipologiapercatalogo.Codice, "", "", "", "", "", true, true, addpars));
                            List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == tipologiapercatalogo.Lingua && (tmp.CodiceTipologia == tipologiapercatalogo.Codice)); });
                            foreach (Prodotto p in prodotti)
                            {
                                //Genero il link per la categoria
                                listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologiapercatalogo.Lingua, p.Descrizione, "", tipologiapercatalogo.Codice, p.CodiceProdotto, "", "", "", "", true, true, addpars));
                                List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == tipologiapercatalogo.Lingua && (tmp.CodiceProdotto == p.CodiceProdotto)); });
                                foreach (SProdotto s in sprodotti)
                                {
                                    //Genero il link per la sotto categoria
                                    listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologiapercatalogo.Lingua, s.Descrizione, "", tipologiapercatalogo.Codice, p.CodiceProdotto, s.CodiceSProdotto, "", "", "", true, true, addpars));
                                }
                            }
                        }
                    }
                }
            if (Utility.Caratteristiche.Count >= 2)
                foreach (Tabrif elem in Utility.Caratteristiche[1])
                {
                    Dictionary<string, string> addpars = new Dictionary<string, string>();
                    if (elem != null && !string.IsNullOrEmpty(elem.Codice) && elem.Lingua == Lingua)
                    {
                        addpars.Add("Caratteristica2", elem.Codice);
                        //foreach (TipologiaOfferte tipologia in listfiltered)
                        if (tipologiapercatalogo != null)
                        {
                            //Genero il link per la tipologia
                            listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologiapercatalogo.Lingua, tipologiapercatalogo.Descrizione, "", tipologiapercatalogo.Codice, "", "", "", "", "", true, true, addpars));
                            List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == tipologiapercatalogo.Lingua && (tmp.CodiceTipologia == tipologiapercatalogo.Codice)); });
                            foreach (Prodotto p in prodotti)
                            {
                                //Genero il link per la categoria
                                listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologiapercatalogo.Lingua, p.Descrizione, "", tipologiapercatalogo.Codice, p.CodiceProdotto, "", "", "", "", true, true, addpars));
                                List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == tipologiapercatalogo.Lingua && (tmp.CodiceProdotto == p.CodiceProdotto)); });
                                foreach (SProdotto s in sprodotti)
                                {
                                    //Genero il link per la sotto categoria
                                    listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologiapercatalogo.Lingua, s.Descrizione, "", tipologiapercatalogo.Codice, p.CodiceProdotto, s.CodiceSProdotto, "", "", "", true, true, addpars));
                                }
                            }
                        }
                    }
                }
#endif
#if FALSE
            if (Utility.Caratteristiche.Count >= 3)
                foreach (Tabrif elem in Utility.Caratteristiche[2])
                {
                    Dictionary<string, string> addpars = new Dictionary<string, string>();
                   if (elem != null && !string.IsNullOrEmpty(elem.Codice) && elem.Lingua == Lingua)
                    {
                        addpars.Add("Caratteristica3", elem.Codice);
                        foreach (TipologiaOfferte tipologia in listfiltered)
                        {
                            //Genero il link per la tipologia
                            listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, tipologia.Descrizione, "", tipologia.Codice, "", "", "", "", "", true, true, addpars));
                            List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == tipologia.Lingua && (tmp.CodiceTipologia == tipologia.Codice)); });
                            foreach (Prodotto p in prodotti)
                            {
                                //Genero il link per la categoria
                                listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, p.Descrizione, "", tipologia.Codice, p.CodiceProdotto, "", "", "", "", true, true, addpars));
                                List<SProdotto> sprodotti = Utility.ElencoSottoProdotti.FindAll(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == tipologia.Lingua && (tmp.CodiceProdotto == p.CodiceProdotto)); });
                                foreach (SProdotto s in sprodotti)
                                {
                                    //Genero il link per la sotto categoria
                                    listalinks.Add(WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(tipologia.Lingua, s.Descrizione, "", tipologia.Codice, p.CodiceProdotto, s.CodiceSProdotto, "", "", "", true, true, addpars));
                                }
                            }
                        }
                    }
                } 
#endif

            return listalinks;
        }


        /// <summary>
        /// Crea i link urlrewrited usando il dictionarty dei parametri aggiuntivi a partre da tipologia, categoria e sottocategoria
        /// aggiungendo i parametri aggiuntivi di filtro
        /// </summary>
        /// <param name="filtriadded"></param>
        /// <param name="lingua"></param>
        /// <param name="generalink">Se true aggiorna la tabella dei link urlrewriting</param>
        /// <returns></returns>
        public static string getlinkbyfiltri(Dictionary<string, string> filtriadded, string lingua, bool generalink = true)
        {
            string linkcostruito = "";
            Dictionary<string, string> addpars = new Dictionary<string, string>();
            string testourl = "";
            string tipologiatmp = "-";
            string categoria2Liv = "";
            string categoria = "";
            foreach (KeyValuePair<string, string> kv in filtriadded)
            {
                if (kv.Key.ToLower() == ("tipologia"))
                {
                    tipologiatmp = kv.Value;
                    TipologiaOfferte tipologiafound = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(t => t.Codice == tipologiatmp && t.Lingua == lingua);
                    testourl += tipologiafound.Descrizione;
                }
                else if (kv.Key.ToLower() == ("categoria"))
                {
                    categoria = kv.Value;
                }
                else if (kv.Key.ToLower() == ("categoria2liv"))
                {
                    categoria2Liv = kv.Value;
                }
                else
                {
                    addpars.Add(kv.Key, kv.Value);
                }
            }
            testourl = testourl.Trim().Replace(" ", "-");
            linkcostruito = SitemapManager.CreaLinkRoutes(lingua, testourl, "", tipologiatmp, categoria, categoria2Liv, "", "", "", true, generalink, addpars);
            return linkcostruito;
        }


        public static string getlinktipologia(string codicetipologia, string lingua)
        {
            string ret = "";
            //Visualizzo i link di per tipologia
            TipologiaOfferte tipo = Utility.TipologieOfferte.Find(s => s.Lingua == lingua && s.Codice == codicetipologia);
            if (tipo != null)
            {
                ret = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, tipo.Descrizione, "", tipo.Codice, "", "", "", "", "", true, false);
            }
            return ret;
        }
        public static string getlinksezione(string codicetipologia, string codiceprodotto, string lingua)
        {
            string ret = "";
            //Visualizzo i link di sezione prodotto
            Prodotto prod = Utility.ElencoProdotti.Find(s => s.Lingua == lingua && s.CodiceTipologia == codicetipologia && s.CodiceProdotto == codiceprodotto);
            if (prod != null)
            {
                ret = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, prod.Descrizione, "", prod.CodiceTipologia, prod.CodiceProdotto, "", "", "", "", true, false);
            }
            return ret;
        }

        public static string getlinksottosezione(string codicetipologia, string codiceprodotto, string codicesottoprodotto, string lingua)
        {
            string ret = "";
            //Visualizzo i link di sezione sottoprodotti
            Prodotto prod = Utility.ElencoProdotti.Find(s => s.Lingua == lingua && s.CodiceTipologia == codicetipologia && s.CodiceProdotto == codiceprodotto);
            SProdotto sprod = Utility.ElencoSottoProdotti.Find(s => s.Lingua == lingua && s.CodiceProdotto == codiceprodotto && s.CodiceSProdotto == codicesottoprodotto);
            if (prod != null && sprod != null)
            {
                ret = WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(lingua, sprod.Descrizione, "", prod.CodiceTipologia, sprod.CodiceProdotto, sprod.CodiceSProdotto, "", "", "", true, false);
            }
            return ret;
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
                    UrlCompleto = CreaLinkRoutes(Lingua, _o.UrltextforlinkbyLingua(Lingua), _o.Id.ToString(), _o.CodiceTipologia, _o.CodiceCategoria, "", "", "", "", true, rigeneraUrlrewritetable);
                    ListaLink.Add(UrlCompleto);
                }
            return ListaLink;
        }

        public static string CreaLinkRoutes(string Lingua, string denominazione, string id, string codicetipologia, string codicecategoria = "", string codicecat2liv = "",
            string regione = "", string annofiltro = "", string mesefiltro = "", bool generaUrlrewrited = true, bool updateTableurlrewriting = false, Dictionary<string, string> addparms = null)
        {
            if (denominazione == null) denominazione = "";
            string destinationselector = "";
            Lingua = Lingua.ToUpper();
            string link = "";
            Tabrif urlRewrited = new Tabrif();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string cleandenominazione = ConteggioCaratteri(CleanUrl(denominazione.Trim().Replace(" ", "-")), 100).ToLower().Trim();
            // if (string.IsNullOrEmpty(cleandenominazione)) cleandenominazione = "-";

            if (!string.IsNullOrEmpty(codicetipologia) && codicetipologia == "con001000" || !string.IsNullOrEmpty(codicetipologia) && codicetipologia == "con001002")
            {
                /////////////////////////////////////
                //Creo l'url per il rewriting
                /////////////////////////////////////
                destinationselector = ""; //Può anche essere vuoto questo ed il tuttofunziona!!
                urlRewrited = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, cleandenominazione, id, "pagina", "", "", "", "", "", addparms);
            }
            else if (!string.IsNullOrEmpty(codicetipologia) && codicetipologia == "con001001")
            {
                /////////////////////////////////////
                //Creo l'url per il rewriting
                /////////////////////////////////////
                destinationselector = "pwa";
                urlRewrited = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, cleandenominazione, id, "pagina", "", "", "", "", "", addparms);
            }
            else if (!string.IsNullOrWhiteSpace(id)) //Link a Scheda dettaglio!!
            {
                //string destinationselector = "";
                TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codicetipologia); });
                if (item != null)
                    destinationselector = ConteggioCaratteri(CleanUrl(item.Descrizione.Trim().Replace(" ", "-")), 100).ToLower().Trim();

                /////////////////////////////////////
                //Creo l'url per il rewriting
                /////////////////////////////////////
                //if (codicetipologia == "rif000666") cleandenominazione += "i";
                //Rendo unici i path immobili aggiungendo in coda  un - ( altrimenti potrei avere dei casi ambigui tra TBL_ATTIVITA e immobili in quanto non essendo dalla stessa tabella potrei avere id unguali per due elementi diversi )
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
                    destinationselector = ConteggioCaratteri(CleanUrl(item.Descrizione.Trim().Replace(" ", "-")), 100).ToLower().Trim();


                //Creiamo il link riscritto base
                if (string.IsNullOrEmpty(cleandenominazione)) cleandenominazione = "-";// "-";
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

        /// <summary>
        /// Prepara l'elemento di rewriting da scrivere in tabella
        /// (non modifica il testo dell'url fà solo la preparazione per la scrittura nel db)
        /// </summary>
        /// <param name="Lingua"></param>
        /// <param name="Tipologia"></param>
        /// <param name="destinationselector"></param>
        /// <param name="textmatch"></param>
        /// <param name="id"></param>
        /// <param name="tipopagina"></param>
        /// <param name="Categoria"></param>
        /// <param name="Categoria2liv"></param>
        /// <param name="anno"></param>
        /// <param name="mese"></param>
        /// <param name="regione"></param>
        /// <param name="addparms"></param>
        /// <returns></returns>
        public static Tabrif GeneraRewritingElement(string Lingua, string Tipologia, string destinationselector, string textmatch, string id = "", string tipopagina = "lista", string Categoria = "", string Categoria2liv = "", string anno = "", string mese = "", string regione = "", Dictionary<string, string> addparms = null)
        {
            Tabrif urlRewrited = new Tabrif();
            var parameters = new Dictionary<string, string>();
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
            if (!string.IsNullOrEmpty(Categoria))
                parameters.Add("Categoria", Categoria);
            if (!string.IsNullOrEmpty(Categoria2liv))
                parameters.Add("Categoria2liv", Categoria2liv);
            if (!string.IsNullOrEmpty(anno))
                parameters.Add("anno", anno);
            if (!string.IsNullOrEmpty(mese))
                parameters.Add("mese", mese);
            if (!string.IsNullOrEmpty(regione))
                parameters.Add("regione", regione);

            if (addparms != null)
                foreach (KeyValuePair<string, string> kv in addparms)
                {
                    if (kv.Key.ToLower() == "filtrodisponibili") continue; //non creo una customizzazione del link per i disponibili in qunato è implicito in tutti i filtri!!!!
                    if (!parameters.ContainsKey(kv.Key))
                        parameters.Add(kv.Key, kv.Value);
                    else
                        parameters[kv.Key] = kv.Value;
                }

            string cleantextmatch = CleanUrl(textmatch.Trim().Replace(" ", "-").Replace("--", "-")).ToLower().Trim();
            if (string.IsNullOrEmpty(cleantextmatch)) cleantextmatch = "-";

            //Crea l'oggetto per la Memorizzazione in tabella il path per i rwwriting
            urlRewrited = CreaElementoRewriting(
                CostruisciRewritedUrl(Lingua, destinationselector, cleantextmatch, id),
                OriginalPathdestinazioneByTipologia(Tipologia, tipopagina),
                Creaparametersstring(parameters));

            return urlRewrited;
        }

        /// <summary>
        /// Customizza il testo dell'url in base ai parametri aggiuntivi addparms e alle categorie 1 e 2 liv, regione, anno o mese
        /// </summary>
        /// <param name="destinationselector"></param>
        /// <param name="cleandenominazione"></param>
        /// <param name="codicetipologia"></param>
        /// <param name="codicecategoria"></param>
        /// <param name="Lingua"></param>
        /// <param name="codicecat2liv"></param>
        /// <param name="regione"></param>
        /// <param name="annofiltro"></param>
        /// <param name="mesefiltro"></param>
        /// <param name="addparms"></param>
        /// <returns></returns>
        public static Tabrif creaUrlListaModified(string destinationselector, string cleandenominazione, string codicetipologia, string codicecategoria, string Lingua, string codicecat2liv, string regione, string annofiltro, string mesefiltro, Dictionary<string, string> addparms = null)
        {
            Tabrif ret = null;
            string testounicolink = cleandenominazione;

            //////////////////////////////////////////////////////////
            /////testo principale dell'url costruito in base alle categorie se passate
            //////////////////////////////////////////////////////////
            string testomodificatore1 = "";
            Prodotto categoriaprodotto = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == codicetipologia && p.CodiceProdotto == codicecategoria && p.Lingua == Lingua);
            if (categoriaprodotto != null)
            {
                testounicolink = "";
                string tmps = CleanUrl(categoriaprodotto.Descrizione.Trim());
                if (!testounicolink.ToLower().Contains(tmps.ToLower())) //se non gia presente nel testo passato
                    testomodificatore1 += tmps + "-";
            }

            SProdotto categoria2liv = Utility.ElencoSottoProdotti.Find(delegate (SProdotto p) { return p.CodiceProdotto == codicecategoria && p.CodiceSProdotto == codicecat2liv && p.Lingua == Lingua; });
            if (categoria2liv != null)
            {
                testounicolink = "";
                string tmps = CleanUrl(categoria2liv.Descrizione.Trim());
                if (!testounicolink.ToLower().Contains(tmps.ToLower())) //se non gia presente nel testo passato
                    testomodificatore1 += tmps + "-";
            }
            //Modificatore testo in presenza del parametro di regione, anno o mese
            Province item = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == regione); });
            if (item != null)
                testomodificatore1 += CleanUrl(item.Regione.Trim()) + "-";
            if (!string.IsNullOrEmpty(CleanUrl(annofiltro.Trim())))
                testomodificatore1 += CleanUrl(annofiltro.Trim()) + "-";
            if (!string.IsNullOrEmpty(CleanUrl(mesefiltro.Trim())))
                testomodificatore1 += CleanUrl(mesefiltro.Trim()) + "-";

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //aggiungo in coda ALL'URL la sequenza dei codici per rendere unica la stringa ( qui farebbe comodo uno shortner!!)
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            string testomodificatore2 = "";
            testomodificatore2 += codicecategoria.Replace("prod", "").TrimStart('0');
            testomodificatore2 += codicecat2liv.Replace("sprod", "").TrimStart('0');
            testomodificatore2 += regione.Replace("p", "").TrimStart('0');
            testomodificatore2 += annofiltro;
            testomodificatore2 += mesefiltro;
            if (addparms != null)
                foreach (KeyValuePair<string, string> kv in addparms)
                {
                    string tmptxt = modificatestourlbyparameter(kv, Lingua);
                    testomodificatore1 += (!string.IsNullOrEmpty(tmptxt)) ? tmptxt + "-" : ""; //estendo l'url col testo in base al filtro dei parametri aggiuntivi passati
                    if (kv.Value != "0") //per valore chiave a zero non faccio il replace senno la chiave 0 la annullo!!!
                        testomodificatore2 += CleanUrl(kv.Value).Replace("p", "").TrimStart('0'); //modifico il testo dell'url usando i codici concatenati per differenziare l'url a prescindere dal testo!
                    else
                        testomodificatore2 += CleanUrl(kv.Value);//modifico il testo dell'url usando i codici concatenati per differenziare l'url a prescindere dal testo!

                }

            if (!string.IsNullOrEmpty(testomodificatore2))
            {
                if (!string.IsNullOrEmpty(testomodificatore1))
                    //testounicolink = (testomodificatore1 + testounicolink).Trim('-');
                    testounicolink = (testomodificatore1).Trim('-');
                if (!string.IsNullOrEmpty(testomodificatore2))
                    testounicolink += "-p" + testomodificatore2;


                testounicolink = testounicolink.Trim('-');
                /////////////////////////////////////
                //Creo l'url per il rewriting ( nei vari casi per gli elenchi modifico il testo della chiamata a seconda dei parametri )
                ///////////////////////////////////// 
                ret = GeneraRewritingElement(Lingua, codicetipologia, destinationselector, testounicolink, "", "lista", codicecategoria, codicecat2liv, annofiltro, mesefiltro, regione, addparms);
            }
            return ret;
        }

        /// <summary>
        /// Modifica il testo dell'url in base al testo relativo del panametro di filtraggio passato
        /// </summary>
        /// <param name="kv"></param>
        /// <returns></returns>
        public static string modificatestourlbyparameter(KeyValuePair<string, string> kv, string lingua)
        {
            string retxt = "";
            if (kv.Key.ToLower() == ("caratteristica1"))
            {
                Tabrif c = Utility.Caratteristiche[0].Find(p => p.Codice == kv.Value && p.Lingua == lingua);
                if (c != null)
                {
                    retxt = c.Campo1.Trim();
                }
            }
            if (kv.Key.ToLower() == ("caratteristica2"))
            {
                Tabrif c = Utility.Caratteristiche[1].Find(p => p.Codice == kv.Value && p.Lingua == lingua);
                if (c != null)
                {
                    retxt = c.Campo1.Trim();
                }
            }
            if (kv.Key.ToLower() == ("caratteristica3"))
            {
                Tabrif c = Utility.Caratteristiche[2].Find(p => p.Codice == kv.Value && p.Lingua == lingua);
                if (c != null)
                {
                    retxt = c.Campo1.Trim();
                }
            }
            if (kv.Key.ToLower() == ("caratteristica4"))
            {
                Tabrif c = Utility.Caratteristiche[3].Find(p => p.Codice == kv.Value && p.Lingua == lingua);
                if (c != null)
                {
                    retxt = c.Campo1.Trim();
                }
            }
            if (kv.Key.ToLower() == ("caratteristica5"))
            {
                Tabrif c = Utility.Caratteristiche[4].Find(p => p.Codice == kv.Value && p.Lingua == lingua);
                if (c != null)
                {
                    retxt = c.Campo1.Trim();
                }
            }

            if (kv.Key.ToLower() == ("regione"))
            {
                string nomeregione = references.NomeRegione(kv.Value, lingua);
                if (!string.IsNullOrEmpty(nomeregione))
                {
                    retxt = nomeregione.Trim();
                }
            }
            if (kv.Key.ToLower() == ("promozioni"))
            {
                retxt = "promo"; //per ora metto fisso questo testo
            }
            
            if (kv.Key.ToLower() == ("prezzofilter"))
            {
                retxt = "prezzofilter"; //per ora metto fisso questo testo
            }
            if (kv.Key.ToLower() == ("datapartenzafilter"))
            {
                retxt = "datafilter"; //per ora metto fisso questo testo
            }
            if (kv.Key.ToLower() == ("statuslistfilter"))
            {
                retxt = "statuslistfilter"; //per ora metto fisso questo testo
            }
            if (kv.Key.ToLower() == ("statusconfirmfilter"))
            {
                retxt = "statusconfirmed"; //per ora metto fisso questo testo
            }
            if (kv.Key.ToLower() == ("etalistfilter"))
            {
                retxt = "etalistfilter"; //per ora metto fisso questo testo
            }
            if (kv.Key.ToLower() == ("duratalistfilter"))
            {
                retxt = "duratalistfilter"; //per ora metto fisso questo testo
            }

            //if (kv.Key.ToLower() == ("promozioni"))
            //{
            //        retxt = "promo";
            //}


            //if (filtriadded.ContainsKey("provincia"))
            //{
            //    string nomeprovincia = references.NomeProvincia(filtriadded["provincia"], lingua);
            //    if (!string.IsNullOrEmpty(nomeprovincia))
            //    {
            //        testourl += nomeprovincia + " ";
            //        addpars.Add("Provincia", filtriadded["provincia"]);
            //    }
            //}
            //if (filtriadded.ContainsKey("comune"))
            //{
            //    string nomecomune = filtriadded["comune"];
            //    if (!string.IsNullOrEmpty(nomecomune))
            //    {
            //        testourl += nomecomune + " ";
            //        addpars.Add("Comune", filtriadded["comune"]);
            //    }
            //}
            ////eventuale aggiunta di geolocation e hidricercaid // per la crezione di url
            ////if (filtriadded.ContainsKey("geolocation"))
            ////{
            ////    string geolocation = filtriadded["geolocation"];
            ////    if (!string.IsNullOrEmpty(geolocation))
            ////    {
            ////        testourl += "testodacalcolare in base alla poszione con un criterio" + " ";
            ////        addpars.Add("Geolocation", filtriadded["geolocation"]);
            ////    }
            ////}

            return CleanUrl(retxt);
        }


        public static string getCulturenamefromlingua(string lng)
        {
            string culturename = "";
            switch (lng)
            {
                case "I":
                case "it":
                    culturename = "it";
                    break;
                case "GB":
                case "en":
                    culturename = "en";
                    break;
                case "RU":
                case "ru":
                    culturename = "ru";
                    break;
                case "FR":
                case "fr":
                    culturename = "fr";
                    break;
                default:
                    culturename = "";
                    break;
            }
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culturename);
            return culturename;
        }
        public static string getLinguafromculture(string lng)
        {
            string culturename = "";
            switch (lng)
            {
                case "it":
                case "I":
                    culturename = "I";
                    break;
                case "en":
                case "GB":
                    culturename = "GB";
                    break;
                case "ru":
                case "RU":
                    culturename = "RU";
                    break;
                case "fr":
                case "FR":
                    culturename = "FR";
                    break;
                default:
                    culturename = "";
                    break;
            }
            //System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culturename);
            return culturename;
        }

        public static string ConteggioCaratteri(string testo, int caratteri = 600, bool nolink = false, string testoAggiunto = "")
        {
            string ritorno = testo;

            if (testo.Length > caratteri)
            {
                int invio = testo.IndexOf(" ", caratteri);

                if (nolink)
                {
                    if (invio != -1)
                        ritorno = testo.Substring(0, invio) + " " + testoAggiunto;
                    else
                        ritorno = testo.Substring(0, caratteri) + " " + testoAggiunto;
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
            strIn = strIn.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "-");

            strIn = strIn.Replace("à", "a");
            strIn = strIn.Replace("è", "e");
            strIn = strIn.Replace("ì", "i");
            strIn = strIn.Replace("ò", "o");
            strIn = strIn.Replace("ù", "u");
            strIn = strIn.Replace("&", "e");
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
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1000 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1000)
                        Pathdestinazione = "~/AspNetPages/Content_tipo1.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1001 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1001)
                        Pathdestinazione = "~/AspNetPages/pwaContent_tipo1.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1002 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1002)
                        Pathdestinazione = "~/AspNetPages/Content_tipo1.aspx";
                    break;
                case "scheda": //Route schede aritcoli
                               //Pathdestinazione = "~/AspNetPages/SchedaOffertaMaster.aspx";
                    Pathdestinazione = "~/AspNetPages/webdetail.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) > 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 50)
                        Pathdestinazione = "~/AspNetPages/webdetail.aspx";
                    //Pathdestinazione = "~/AspNetPages/SchedaOffertaMaster.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1) //
                        Pathdestinazione = "~/AspNetPages/webdetail.aspx";
                    //Pathdestinazione = "~/AspNetPages/SchedaProdotto.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 51 && Convert.ToInt32(codicetipologia.Substring(3)) <= 60) //No Scheda apribile
                        Pathdestinazione = "";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 61 && Convert.ToInt32(codicetipologia.Substring(3)) <= 62)
                        Pathdestinazione = "~/AspNetPages/webdetail.aspx";
                    //Pathdestinazione = "~/AspNetPages/SchedaOffertaMaster.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 101 && Convert.ToInt32(codicetipologia.Substring(3)) <= 101) //
                        Pathdestinazione = "~/AspNetPages/webdetail.aspx";
                    //Pathdestinazione = "~/AspNetPages/SchedaProdotto.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 500 && Convert.ToInt32(codicetipologia.Substring(3)) <= 600) //
                        Pathdestinazione = "~/AspNetPages/pwadetail.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 199 && Convert.ToInt32(codicetipologia.Substring(3)) <= 199) //No Scheda apribile
                        Pathdestinazione = "";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1000 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1000) //No Scheda apribile
                        Pathdestinazione = "";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 666 && Convert.ToInt32(codicetipologia.Substring(3)) <= 666)
                        Pathdestinazione = "~/AspNetPages/SchedaResource.aspx";
                    break;
                case "lista": //Route liste ricerche
                              //Pathdestinazione = "~/AspNetPages/RisultatiRicerca.aspx";
                    Pathdestinazione = "~/AspNetPages/weblist.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1) //
                        Pathdestinazione = "~/AspNetPages/weblist.aspx";
                    //Pathdestinazione = "~/AspNetPages/RisultatiProdotti.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) > 1 && Convert.ToInt32(codicetipologia.Substring(3)) <= 50)
                        Pathdestinazione = "~/AspNetPages/weblist.aspx";
                    //Pathdestinazione = "~/AspNetPages/RisultatiRicerca.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 51 && Convert.ToInt32(codicetipologia.Substring(3)) <= 62) //
                        Pathdestinazione = "~/AspNetPages/weblist.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 101 && Convert.ToInt32(codicetipologia.Substring(3)) <= 101) //
                        Pathdestinazione = "~/AspNetPages/weblist.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 500 && Convert.ToInt32(codicetipologia.Substring(3)) <= 600) //
                        Pathdestinazione = "~/AspNetPages/pwalist.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 199 && Convert.ToInt32(codicetipologia.Substring(3)) <= 199) //
                        Pathdestinazione = "~/AspNetPages/weblist.aspx";
                    //Pathdestinazione = "~/AspNetPages/RisultatiRicerca.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 1000 && Convert.ToInt32(codicetipologia.Substring(3)) <= 1000) //
                        Pathdestinazione = "~/AreaRiservata/Forum.aspx";
                    if (codicetipologia.Length > 3 && Convert.ToInt32(codicetipologia.Substring(3)) >= 666 && Convert.ToInt32(codicetipologia.Substring(3)) <= 666)
                        Pathdestinazione = "~/AspNetPages/RisultatiResource.aspx";
                    break;
                default:
                    //Pathdestinazione = "~/AspNetPages/RisultatiRicerca.aspx";
                    Pathdestinazione = "~/AspNetPages/weblist.aspx";
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
            //string rewritedurl = Lingua;
            //Modifica per gestione link con lingua corretta // abilitare per modifica codici culture lingua 19.12.18
#if true
            string correctlingua = getCulturenamefromlingua(Lingua);
            string rewritedurl = correctlingua;
#endif
            //if (!string.IsNullOrEmpty(destinationselector))
            if (!string.IsNullOrEmpty(destinationselector) && textmatch.ToLower() != destinationselector.ToLower())
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
                string query = "SELECT * FROM TBL_URLREWRITING WHERE Calledurl like @Calledurl";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@Calledurl", calledurl);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        Tabrif _item = new Tabrif();

                        _item = new Tabrif();
                        _item.Id = reader.GetInt64(reader.GetOrdinal("ID")).ToString();
                        if (!reader["Relatedid"].Equals(DBNull.Value))
                            _item.Intero1 = reader.GetInt64(reader.GetOrdinal("Relatedid"));
                        _item.Codice = reader.GetString(reader.GetOrdinal("Calledurl")).ToString().Trim();
                        _item.Campo1 = reader.GetString(reader.GetOrdinal("Pathdestinazione")).Trim();
                        _item.Campo2 = reader.GetString(reader.GetOrdinal("Parametri")).Trim();


                        //  Dictionary<string, string> keyvalues = WelcomeLibrary.UF.SitemapManager.SplitParameters(_item.Campo2);

                        item = _item;
                    }
                }


            }
            catch
            {
                // throw new ApplicationException("Errore Caricamento tabella urlrewriting :" + error.Message, error);
            }
            return item;
        }
        public static void InserisciAggiornaUrlrewrite(string connessione, Tabrif item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (string.IsNullOrEmpty(item.Codice.Trim()) || string.IsNullOrEmpty(item.Campo1.Trim())) return;

            //Se presente carico l'elemento nel db per l'aggiornamento
            Tabrif itemindb = GetUrlRewriteaddress(connessione, item.Codice);
            if (itemindb != null)
            {
                item.Id = itemindb.Id;
            }
            SQLiteParameter p1 = new SQLiteParameter("@Calledurl", item.Codice);
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@Pathdestinazione", item.Campo1);
            parColl.Add(p2);
            SQLiteParameter p2b = new SQLiteParameter("@Parametri", item.Campo2);
            parColl.Add(p2b);

            long i = 0;
            if (item.Intero1 != null)
                i = item.Intero1.Value;
            SQLiteParameter p2c = new SQLiteParameter("@Relatedid", i);
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
            catch
            {
                //  throw new ApplicationException("Errore, inserimento/aggiornamento urlrewrite :" + error.Message, error);
            }
            return;
        }
        public static long EliminaUrlrewrite(string connessione, Tabrif item)
        {
            long idret = -1;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_URLREWRITING WHERE ( Calledurl = @Calledurl ) ";
            SQLiteParameter p1;
            p1 = new SQLiteParameter("@Calledurl", item.Codice);
            parColl.Add(p1);
            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch
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
        public static long EliminaUrlrewritebyIdOfferta(string connessione, string id)
        {
            long idret = -1;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_URLREWRITING WHERE ( ( Calledurl like @Calledurl or Parametri like @Parametroid ) and ( Parametri like '%idOfferta%' ) and ( Parametri not like @Parametroexcludi )) ";
            SQLiteParameter p1;
            p1 = new SQLiteParameter("@Calledurl", "%-" + id);
            parColl.Add(p1);
            SQLiteParameter p2;
            p2 = new SQLiteParameter("@Parametroid", "%;idOfferta," + id + ";%");
            parColl.Add(p2);
            SQLiteParameter pexc;
            pexc = new SQLiteParameter("@Parametroexcludi", "%Tipologia,rif000666%");
            parColl.Add(pexc);

            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch
            {
                //throw new ApplicationException("Errore, eliminazione Mail da presa in carico:" + error.Message, error);
            }
            return idret;
        }


        public static long EliminaUrlrewritebyIdContenuto(string connessione, string id)
        {
            long idret = -1;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_URLREWRITING WHERE ( Calledurl like @Calledurl or Parametri like @Parametroid ) and ( Parametri like '%idContenuto%' )  ";
            SQLiteParameter p1;
            p1 = new SQLiteParameter("@Calledurl", "%-" + id);
            parColl.Add(p1);
            SQLiteParameter p2;
            p2 = new SQLiteParameter("@Parametroid", "%;idContenuto," + id + ";%");
            parColl.Add(p2);

            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch
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




        public static Tabrif GetRedirecturl(string connection, string originalurl)
        {
            if (connection == null || connection == "") return null;
            if (originalurl == null || originalurl == "") return null;
            Tabrif item = new Tabrif();

            try
            {
                string query = "SELECT * FROM TBL_Redirect WHERE originalurl = @originalurl";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@originalurl", originalurl);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return null; };
                    if (reader.HasRows == false)
                        return null;

                    while (reader.Read())
                    {
                        Tabrif _item = new Tabrif();

                        _item = new Tabrif();
                        _item.Id = reader.GetInt64(reader.GetOrdinal("ID")).ToString();
                        _item.Campo1 = reader.GetString(reader.GetOrdinal("originalurl")).ToString().Trim();
                        _item.Campo2 = reader.GetString(reader.GetOrdinal("redirectedurl")).Trim();

                        item = _item;
                        break;//prendo il primo trovato
                    }
                }


            }
            catch
            {
                // throw new ApplicationException("Errore Caricamento tabella urlrewriting :" + error.Message, error);
            }
            return item;
        }
        public static void InserisciAggiornaRedirecturl(string connessione, Tabrif item)
        {
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return;
            if (string.IsNullOrEmpty(item.Campo1.Trim())) return;

            //Se presente carico l'elemento nel db per l'aggiornamento
            Tabrif itemindb = GetRedirecturl(connessione, item.Campo1);
            if (itemindb != null)
            {
                item.Id = itemindb.Id;
            }
            SQLiteParameter p1 = new SQLiteParameter("@originalurl", item.Campo1);
            parColl.Add(p1);
            SQLiteParameter p2 = new SQLiteParameter("@redirectedurl", item.Campo2);
            parColl.Add(p2);

            string query = "";
            if (item.Id != "")
            {
                //Update
                query = "UPDATE [TBL_Redirect] SET originalurl=@originalurl,redirectedurl=@redirectedurl";
                query += " WHERE [Id] = " + item.Id;
            }
            else
            {
                //Insert
                query = "INSERT INTO TBL_Redirect (originalurl,redirectedurl";
                query += " )";
                query += " values ( ";
                query += "@originalurl,@redirectedurl";
                query += " )";
            }

            try
            {
                long retID = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
                if (item.Id == "") item.Id = retID.ToString(); // se era insert memorizzo l'id del cliente appena inserito
            }
            catch
            {
                //  throw new ApplicationException("Errore, inserimento/aggiornamento urlrewrite :" + error.Message, error);
            }
            return;
        }
        public static long EliminRedirecturl(string connessione, Tabrif item)
        {
            long idret = -1;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_Redirect WHERE ( originalurl = @originalurl ) ";
            SQLiteParameter p1;
            p1 = new SQLiteParameter("@originalurl", item.Campo1);
            parColl.Add(p1);
            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch
            {
                //throw new ApplicationException("Errore, eliminazione Mail da presa in carico:" + error.Message, error);
            }
            return idret;
        }

        public static long EliminRedirecturlNotInidlist(string connessione, List<string> idlist)
        {
            long idret = -1;
            List<SQLiteParameter> parColl = new List<SQLiteParameter>();
            if (connessione == null || connessione == "") return idret;

            string query = "DELETE FROM TBL_Redirect  ";

            string queryfilter = "";

            if (idlist != null && idlist.Count > 0)
            {
                if (!queryfilter.ToLower().Contains("where"))
                    queryfilter += " WHERE Id not in (    ";
                else
                    queryfilter += " AND  Id not in (      ";
                foreach (string id in idlist)
                {
                    if (!string.IsNullOrEmpty(id.Trim()))
                        queryfilter += " " + id + " ,";
                }
                queryfilter = queryfilter.TrimEnd(',') + " ) ";
            }
            query += queryfilter;

            try
            {
                idret = dbDataAccess.ExecuteStoredProcListOle(query, parColl, connessione);
            }
            catch
            {
                //throw new ApplicationException("Errore, eliminazione Mail da presa in carico:" + error.Message, error);
            }
            return idret;
        }
    }
}