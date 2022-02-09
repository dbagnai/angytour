using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace WelcomeLibrary.UF
{
    public class custombind
    {

        private string Lingua = "";
        private string Username = "";
        private System.Web.SessionState.HttpSessionState Session = null;
        private System.Web.HttpRequest Richiesta = null;

        public static Dictionary<string, Dictionary<string, string>> jscommands = new Dictionary<string, Dictionary<string, string>>();

        //public static void cleanjscommands()
        //{
        //    try
        //    {
        //        //rimozione chiavi scadute
        //        List<string> keytoremove = new List<string>();
        //        long hourslease = 1;
        //        //Svuotiamo le chiavi scadute da un certo lasso di tempo per ripulirle
        //        foreach (KeyValuePair<string, Dictionary<string, string>> kv in jscommands)
        //        {
        //            if (kv.Value.ContainsKey("timeadded"))
        //            {
        //                DateTime timestored = DateTime.FromBinary(long.Parse(kv.Value["timeadded"]));
        //                TimeSpan ts = (DateTime.Now - timestored);
        //                if (ts.Hours > hourslease) keytoremove.Add(kv.Key);
        //            }
        //        }
        //        keytoremove.ForEach(k => jscommands.Remove(k));
        //    }
        //    catch
        //    { }
        //}



        //public custombind()
        //{
        //    Lingua = "";
        //    Username = "";
        //    Session = null;
        //    Richiesta = null;
        //}

        public string bind(string text, string lingua, string username = "", System.Web.SessionState.HttpSessionState sessione = null, Dictionary<string, string> filtri = null, Dictionary<string, string> filtripager = null, System.Web.HttpRequest richiesta = null)
        {

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // questo return è da disabilitare per abilitare il  nuovo sistema di bind lato server sostitutivo di quello javascript
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if false
            return text; //TOrna il testo senza alcuna modifica
#endif
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Lingua = lingua;
            Username = username;
            Session = sessione;
            Richiesta = richiesta;

            if (Session == null) return text;


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);
            var findclasses = doc.DocumentNode.Descendants().Where(d =>
                   d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("inject") && !d.Attributes["class"].Value.Contains("clientsideinject") && d.Attributes.Contains("params")
               );
            if ((findclasses != null) && (findclasses.Count() > 0))
            {
                foreach (var node in findclasses)
                {
                    BindElement(node, doc, filtri, filtripager);
                }

            }
            //var imgs = doc.DocumentNode.SelectNodes("//img");
            //if (imgs.Count>0)
            //{
            //	// vado a modificare solo la prima che ha una certa classe
            //}
            return doc.DocumentNode.OuterHtml;
        }



        private void BindElement(HtmlNode node, HtmlDocument doc, Dictionary<string, string> filtri = null, Dictionary<string, string> filtripager = null)
        {
            try
            {
                //facciamo i binding in base ai parametri
                string parametri = node.Attributes["params"].Value;
                List<String> pars = parametri.Split(',').ToList();
                List<String> parsclear = new List<string>();
                pars.ForEach(p => parsclear.Add(p.Trim().Trim('\'').Trim()));
                functioncallmapping(parsclear, node, doc, filtri, filtripager);
            }
            catch
            {
            }

        }

        private void functioncallmapping(List<string> pars, HtmlNode node, HtmlDocument doc, Dictionary<string, string> dictpars = null, Dictionary<string, string> dictpagerpars = null)
        {
            if (dictpars == null)
                dictpars = new Dictionary<string, string>();
            else
            {
                //if (dictpars.ContainsKey("functionname"))
                //    pars.Add(dictpars["functionname"]);
                //Contrllo da fare su esistenza chiavi in dictpars .....,...
            }
            if (dictpagerpars == null)
                dictpagerpars = new Dictionary<string, string>();
            else
            {
                //Controlli da fare sulla paginazione per esistenza di chiavi base in dictpagerpars !!! page pagesize enablepager .....
            }

            string container = node.Id; // id del contenitoure a cui appendere i risultati!
            string templatetext = "";
            /////////////////////////////////////////////////////////////
            //Al termine creo una lista delle funzioni javascript di inizializzazione da chiamare idcontenitore, idcontrollo,funzione init, parametri eventuali(da capire se si puo usare unfile.js standard di inizializzazione )!!!!!!!
            /////////////////////////////////////////////////////////////
            if (pars.Count > 0)
                switch (pars[0].ToLower())
                {
                    case "injectsliderandloadbanner":
                        // injectSliderAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height)

                        //node.InnerHtml; //qui devo inserire i dati col bind
                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3) dictpars.Add("controlid", pars[3]);
                        if (pars.Count > 7) dictpars.Add("listShow", pars[7]);
                        if (pars.Count > 8) dictpars.Add("maxelement", pars[8]);
                        if (pars.Count > 9) dictpars.Add("connectedid", pars[9]);
                        if (pars.Count > 10) dictpars.Add("tblsezione", pars[10]);
                        if (pars.Count > 11) dictpars.Add("filtrosezione", pars[11]);
                        if (pars.Count > 12) dictpars.Add("mescola", pars[12]);
                        if (pars.Count > 13) dictpars.Add("width", pars[13]);
                        if (pars.Count > 14) dictpars.Add("height", pars[14]);
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 4) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6) dictpagerpars.Add("enablepager", pars[6]);

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("tblsezione")) return;
                        if (!dictpars.ContainsKey("filtrosezione")) return;

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE (sliderbanner.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind

                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"], Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Banners> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Banners>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants("ul"); //elemento del template a cui appendere i singoli elementi
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0) && data != null && data.Count > 0)
                                {
                                    var findelements = template.DocumentNode.Descendants("li"); //elemento da ripetere e bindare ai dati
                                    if ((findelements != null) && (findelements.Count() > 0))
                                    {
                                        HtmlNode cloneitemtemplate = findelements.First().Clone(); //elemento matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child anche da template e da findelements !!!
                                        foreach (Banners item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            //var bindingnodes = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }
                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                        }
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    if (elementtoappend.First() != null)
                                    {
                                        //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                        node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //if (node.ParentNode != null)
                                    //    if (node.ParentNode.Attributes.Contains("style"))
                                    //    {
                                    //        node.ParentNode.Attributes["style"].Value = node.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    //        node.ParentNode.Attributes["style"].Value += ";display:block";
                                    //    }
                                    //    else
                                    //        node.ParentNode.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi 
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();


                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container)) jscommands[Session.SessionID].Remove(container);

                                        jscommands[Session.SessionID].Add(container, WelcomeLibrary.UF.Utility.waitwrappercall("initSlider", "initSlider('" + dictpars["controlid"] + "','" + node.Id + "'," + dictpars["width"] + "," + dictpars["height"] + ")"));
                                        //jscommands[Session.SessionID].Add(container, "initSlider('" + dictpars["controlid"] + "','" + node.Id + "'," + dictpars["width"] + "," + dictpars["height"] + ")");

                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                                else
                                {
                                    //spengo il node se non ci sono elementi dentro
                                    if (node != null)
                                        if (node.Attributes.Contains("style"))
                                        {
                                            node.Attributes["class"].Value = "";
                                            node.Attributes["style"].Value = node.Attributes["style"].Value.Replace(": ", ":").Replace("display:block", "");
                                            node.Attributes["style"].Value += ";display:none !important;";
                                        }
                                        else
                                        {
                                            node.Attributes["class"].Value = "";
                                            node.Attributes.Add("style", ";display:none !important;");
                                        }
                                }


                            }
                        }

                        break;
                    case "injectfasciaandloadbanner":
                        //injectFasciaAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola) 

                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3) dictpars.Add("controlid", pars[3]);

                        if (pars.Count > 7) dictpars.Add("listShow", pars[7]);
                        if (pars.Count > 8) dictpars.Add("maxelement", pars[8]);
                        if (pars.Count > 9) dictpars.Add("connectedid", pars[9]);
                        if (pars.Count > 10) dictpars.Add("tblsezione", pars[10]);
                        if (pars.Count > 11) dictpars.Add("filtrosezione", pars[11]);
                        if (pars.Count > 12) dictpars.Add("mescola", pars[12]);
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 4) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6) dictpagerpars.Add("enablepager", pars[6]);

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("tblsezione")) return;
                        if (!dictpars.ContainsKey("filtrosezione")) return;

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE  (bannerfascia.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"], Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Banners> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Banners>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]); //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
                                {
                                    string innerelement = elementtoappend.First().InnerHtml;
                                    if ((innerelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(innerelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        foreach (Banners item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }
                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                        }
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    if (elementtoappend != null)
                                    {
                                        //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                        node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    }
                                    //if (node.ParentNode != null)
                                    //    if (node.ParentNode.Attributes.Contains("style"))
                                    //    {
                                    //        node.ParentNode.Attributes["style"].Value = node.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    //        node.ParentNode.Attributes["style"].Value += ";display:block";
                                    //    }
                                    //    else
                                    //        node.ParentNode.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container)) jscommands[Session.SessionID].Remove(container);
                                        jscommands[Session.SessionID].Add(container, WelcomeLibrary.UF.Utility.waitwrappercall("initcycleBanner", "initcycleBanner('" + dictpars["controlid"] + "','" + node.Id + "')"));
                                        //jscommands[Session.SessionID].Add(container, "initcycleBanner('" + dictpars["controlid"] + "','" + node.Id + "')");

                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectportfolioandloadbanner":
                        //injectPortfolioAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola)

                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0 && !dictpars.ContainsKey("functionname")) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1 && !dictpars.ContainsKey("templateHtml")) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2 && !dictpars.ContainsKey("container")) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3 && !dictpars.ContainsKey("controlid")) dictpars.Add("controlid", pars[3]);

                        if (pars.Count > 7 && !dictpars.ContainsKey("listShow")) dictpars.Add("listShow", pars[7]);
                        if (pars.Count > 8 && !dictpars.ContainsKey("maxelement")) dictpars.Add("maxelement", pars[8]);
                        if (pars.Count > 9 && !dictpars.ContainsKey("connectedid")) dictpars.Add("connectedid", pars[9]);
                        if (pars.Count > 10 && !dictpars.ContainsKey("tblsezione")) dictpars.Add("tblsezione", pars[10]);
                        if (pars.Count > 11 && !dictpars.ContainsKey("filtrosezione")) dictpars.Add("filtrosezione", pars[11]);
                        if (pars.Count > 12 && !dictpars.ContainsKey("mescola")) dictpars.Add("mescola", pars[12]);
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 4 && !dictpagerpars.ContainsKey("page")) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5 && !dictpagerpars.ContainsKey("pagesize")) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6 && !dictpagerpars.ContainsKey("enablepager")) dictpagerpars.Add("enablepager", pars[6]);

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("tblsezione")) return;
                        if (!dictpars.ContainsKey("filtrosezione")) return;

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE  (IsotopeBanner.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"], Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Banners> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Banners>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]);  //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
                                {

                                    bool alternate = false;
                                    if (elementtoappend.First().Attributes.Contains("class") && elementtoappend.First().Attributes["class"].Value.Contains("alternatecolor"))
                                        alternate = true;

                                    string innerelement = elementtoappend.First().InnerHtml;
                                    if ((innerelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(innerelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        int j = 0;
                                        foreach (Banners item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }

                                            ////////////////////ODD EVEN GESTIONE SFONDO
                                            if (alternate)
                                            {
                                                if (IsEven(j))
                                                {

                                                    var nodestoremove = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("odd"));
                                                    if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                    {
                                                        nodestoremove.First().Remove();
                                                    }
                                                }
                                                else
                                                {

                                                    var nodestoremove = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("even"));
                                                    if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                        if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                        {
                                                            nodestoremove.First().Remove();
                                                        }
                                                }

                                            }
                                            /////////////////////////////////////////////
                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                            j++;
                                        }
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    if (elementtoappend != null)
                                    {
                                        //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                        node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    }
                                    if (node.ParentNode != null)
                                        if (node.ParentNode.Attributes.Contains("style"))
                                        {
                                            node.ParentNode.Attributes["style"].Value = node.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.ParentNode.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.ParentNode.Attributes.Add("style", "display:block");
                                    if (node.ParentNode.ParentNode != null)
                                        if (node.ParentNode.ParentNode.Attributes.Contains("style"))
                                        {
                                            node.ParentNode.ParentNode.Attributes["style"].Value = node.ParentNode.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.ParentNode.ParentNode.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.ParentNode.ParentNode.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container)) jscommands[Session.SessionID].Remove(container);
                                        jscommands[Session.SessionID].Add(container, WelcomeLibrary.UF.Utility.waitwrappercall("InitIsotopeLocalBanner", "InitIsotopeLocalBanner('" + dictpars["controlid"] + "');"));
                                        //jscommands[Session.SessionID].Add(container, "InitIsotopeLocalBanner('" + dictpars["controlid"] + "');");

                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectbootstrapportfolioandloadbanner":
                        //injectPortfolioAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola)

                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0 && !dictpars.ContainsKey("functionname")) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1 && !dictpars.ContainsKey("templateHtml")) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2 && !dictpars.ContainsKey("container")) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3 && !dictpars.ContainsKey("controlid")) dictpars.Add("controlid", pars[3]);

                        if (pars.Count > 7 && !dictpars.ContainsKey("listShow")) dictpars.Add("listShow", pars[7]);
                        if (pars.Count > 8 && !dictpars.ContainsKey("maxelement")) dictpars.Add("maxelement", pars[8]);
                        if (pars.Count > 9 && !dictpars.ContainsKey("connectedid")) dictpars.Add("connectedid", pars[9]);
                        if (pars.Count > 10 && !dictpars.ContainsKey("tblsezione")) dictpars.Add("tblsezione", pars[10]);
                        if (pars.Count > 11 && !dictpars.ContainsKey("filtrosezione")) dictpars.Add("filtrosezione", pars[11]);
                        if (pars.Count > 12 && !dictpars.ContainsKey("mescola")) dictpars.Add("mescola", pars[12]);
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 4 && !dictpagerpars.ContainsKey("page")) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5 && !dictpagerpars.ContainsKey("pagesize")) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6 && !dictpagerpars.ContainsKey("enablepager")) dictpagerpars.Add("enablepager", pars[6]);

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("tblsezione")) return;
                        if (!dictpars.ContainsKey("filtrosezione")) return;

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE  (IsotopeBanner.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"], Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Banners> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Banners>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]);  //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0) && ((data != null) && (data.Count() > 0)))
                                {
                                    bool alternate = false;
                                    if (elementtoappend.First().Attributes.Contains("class") && elementtoappend.First().Attributes["class"].Value.Contains("alternatecolor"))
                                        alternate = true;

                                    string innerelement = elementtoappend.First().InnerHtml;
                                    if ((innerelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(innerelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        int j = 0;
                                        foreach (Banners item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }
                                            ////////////////////ODD EVEN GESTIONE SFONDO
                                            if (alternate)
                                            {
                                                if (IsEven(j))
                                                {

                                                    var nodestoremove = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("odd"));
                                                    if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                    {
                                                        nodestoremove.First().Remove();
                                                    }
                                                }
                                                else
                                                {

                                                    var nodestoremove = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("even"));
                                                    if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                        if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                        {
                                                            nodestoremove.First().Remove();
                                                        }
                                                }

                                            }
                                            /////////////////////////////////////////////
                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                            j++;
                                        }
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    if (elementtoappend != null)
                                    {
                                        //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                        node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    }
                                    if (node != null)
                                        if (node.Attributes.Contains("style"))
                                        {
                                            node.Attributes["style"].Value = node.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.Attributes.Add("style", "display:block");
                                    if (node.ParentNode != null)
                                        if (node.ParentNode.Attributes.Contains("style"))
                                        {
                                            node.ParentNode.Attributes["style"].Value = node.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.ParentNode.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.ParentNode.Attributes.Add("style", "display:block");
                                    if (node.ParentNode.ParentNode != null)
                                        if (node.ParentNode.ParentNode.Attributes.Contains("style"))
                                        {
                                            node.ParentNode.ParentNode.Attributes["style"].Value = node.ParentNode.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.ParentNode.ParentNode.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.ParentNode.ParentNode.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //    if (jscommands.ContainsKey(Session.SessionID)){
                                    //if (jscommands[Session.SessionID].ContainsKey(container)) jscommands[Session.SessionID].Remove(container);
                                    //jscommands[Session.SessionID].Add(container, "InitIsotopeLocalBanner('" + dictpars["controlid"] + "');");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectscrollerandloadbanner":
                        //injectScrollerAndLoadBanner(type, container, controlid, listShow, maxelement, scrollertype, tblsezione, filtrosezione, mescola)

                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3) dictpars.Add("controlid", pars[3]);

                        if (pars.Count > 4) dictpars.Add("listShow", pars[4]);
                        if (pars.Count > 5) dictpars.Add("maxelement", pars[5]);
                        if (pars.Count > 6) dictpars.Add("scrollertype", pars[6]);
                        if (pars.Count > 7) dictpars.Add("tblsezione", pars[7]);
                        if (pars.Count > 8) dictpars.Add("filtrosezione", pars[8]);
                        if (pars.Count > 9) dictpars.Add("mescola", pars[9]);
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 0) dictpagerpars.Add("page", "1");
                        if (pars.Count > 0) dictpagerpars.Add("pagesize", "1");
                        if (pars.Count > 0) dictpagerpars.Add("enablepager", "false");

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("tblsezione")) return;
                        if (!dictpars.ContainsKey("filtrosezione")) return;

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE  (owlscrollerBanner.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"], Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Banners> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Banners>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]); //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0) && ((data != null) && (data.Count() > 0)))
                                {
                                    string innerelement = elementtoappend.First().InnerHtml;
                                    if ((innerelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(innerelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        foreach (Banners item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }
                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                        }
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    if (elementtoappend != null)
                                    {
                                        //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                        node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    }
                                    if (node.ParentNode != null)
                                        if (node.ParentNode.Attributes.Contains("style"))
                                        {
                                            node.ParentNode.Attributes["style"].Value = node.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.ParentNode.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.ParentNode.Attributes.Add("style", "display:block");
                                    //if (node.ParentNode.ParentNode != null)
                                    //    if (node.ParentNode.ParentNode.Attributes.Contains("style"))
                                    //    {
                                    //        node.ParentNode.ParentNode.Attributes["style"].Value = node.ParentNode.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    //        node.ParentNode.ParentNode.Attributes["style"].Value += ";display:block";
                                    //    }
                                    //    else
                                    //        node.ParentNode.ParentNode.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container)) jscommands[Session.SessionID].Remove(container);
                                        jscommands[Session.SessionID].Add(container, WelcomeLibrary.UF.Utility.waitwrappercall("initScrollerBanner", "initScrollerBanner('" + dictpars["controlid"] + "', '" + dictpars["scrollertype"] + "'); "));
                                        //jscommands[Session.SessionID].Add(container, "initScrollerBanner('" + dictpars["controlid"] + "','" + dictpars["scrollertype"] + "');");

                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }

                        break;
                    case "injectscrollerandload":
                        //injectScrollerAndLoad(type, container, controlid, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, scrollertype, categoria2Liv, vetrina, promozioni) 

                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3) dictpars.Add("controlid", pars[3]);
                        if (pars.Count > 4) dictpars.Add("listShow", pars[4]);
                        if (pars.Count > 5) dictpars.Add("tipologia", pars[5]);
                        if (pars.Count > 6) dictpars.Add("categoria", pars[6]);
                        if (pars.Count > 7) dictpars.Add("visualData", pars[7]);
                        if (pars.Count > 8) dictpars.Add("visualPrezzo", pars[8]);
                        if (pars.Count > 9) dictpars.Add("maxelement", pars[9]);
                        if (pars.Count > 10) dictpars.Add("scrollertype", pars[10]);
                        else dictpars.Add("scrollertype", "");
                        if (pars.Count > 11) dictpars.Add("categoria2Liv", pars[11]);
                        if (pars.Count > 12) dictpars.Add("vetrina", pars[12]);
                        if (pars.Count > 13) dictpars.Add("promozioni", pars[13]);
                        if (pars.Count > 14 && !string.IsNullOrWhiteSpace(pars[14]) && !dictpars.ContainsKey("objfiltro")) dictpars.Add("objfiltro", pars[14]);
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 0) dictpagerpars.Add("page", "1");
                        if (pars.Count > 0) dictpagerpars.Add("pagesize", "1");
                        if (pars.Count > 0) dictpagerpars.Add("enablepager", "false");

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;

                        try
                        {
                            ////////////////////////////////////////////////
                            // Se ho passato dei parametri aggiuntivi alla funzione li sposto nella collection di filtro ( E SONO PRIORITARI RISPETTO ALLA SESSIONE )
                            ////////////////////////////////////////////////
                            bool flag_addedpars1 = false;
                            if (dictpars.ContainsKey("objfiltro"))
                            {
                                //dataManagement.EncodeToBase64
                                dictpars["objfiltro"] = dataManagement.DecodeFromBase64(dictpars["objfiltro"]);//mi aspetto i parametri con formato json serializzato e convertito base64
                                Dictionary<string, string> dictparsadded = new Dictionary<string, string>();
                                dictparsadded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictpars["objfiltro"]);
                                if (dictparsadded != null)
                                {
                                    foreach (KeyValuePair<string, string> kv in dictparsadded)
                                    {
                                        //aggiungo i parametri   se presenti
                                        if (!dictpars.ContainsKey(kv.Key)) dictpars.Add(kv.Key, kv.Value);
                                        else dictpars[kv.Key] = kv.Value;//sovrascivo il valore passato dando la priorita a quello messo in sessione
                                        flag_addedpars1 = true;
                                    }
                                }
                            }
#if false
                            //////////////////////////////////////////////////
                            //Ricarichiamo dalla session eventuali parametri aggiuntivi non passati nella chiamata di bind ma presenti in sessione
                            //////////////////////////////////////////////////
                            if (!flag_addedpars1)
                                if (Session != null && Session["objfiltro"] != null)
                                {
                                    string retval = Session["objfiltro"].ToString();//Prendo dalla sessione la chiave che contiene i parametri aggiuntivi serializzati
                                    if (retval != null && retval != "")
                                    {
                                        Dictionary<string, string> dictparsfromsession = new Dictionary<string, string>();
                                        dictparsfromsession = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(retval);
                                        if (dictparsfromsession != null)
                                        {
                                            bool skipsessionfilters = false;
                                            if (dictpars.ContainsKey("tipologia") && dictparsfromsession.ContainsKey("tipologia") && dictpars["tipologia"] != dictparsfromsession["tipologia"])
                                                skipsessionfilters = true;
                                            //if (dictpars.ContainsKey("categoria") && dictparsfromsession.ContainsKey("categoria") && dictpars["categoria"] != dictparsfromsession["categoria"])
                                            //    skipsessionfilters = true;
                                            //if (dictpars.ContainsKey("categoria2Liv") && dictparsfromsession.ContainsKey("categoria2Liv") && dictpars["categoria2Liv"] != dictparsfromsession["categoria2Liv"])
                                            //    skipsessionfilters = true;

                                            if (!skipsessionfilters)
                                                foreach (KeyValuePair<string, string> kv in dictparsfromsession)
                                                {
                                                    //if (kv.Key == ("page" + dictpars["controlid"]))
                                                    //    dictpagerpars["page"] = kv.Value;//la pagina la prendo dalla sessione se presente!
                                                    //aggiungo i parametri dalla sessione se presenti
                                                    if (!dictpars.ContainsKey(kv.Key)) dictpars.Add(kv.Key, kv.Value);
                                                    else dictpars[kv.Key] = kv.Value;//sovrascivo il valore passato dando la priorita a quello messo in sessione
                                                }
                                        }
                                    }
                                }
#endif
                        }
                        catch { }


                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE  (owlscrollerOfferte.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = offerteDM.filterData(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"], Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Offerte> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Offerte>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]); //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
                                {
                                    string innerelement = elementtoappend.First().InnerHtml;
                                    if ((innerelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(innerelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        foreach (Offerte item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }
                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                        }
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    if (elementtoappend != null)
                                    {
                                        //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                        node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    }
                                    //Rendiamo visibile il primo div contenitore del template
                                    var firstnode = template.DocumentNode.Descendants("div");
                                    if ((firstnode != null) && (firstnode.Count() > 0))
                                    {
                                        if (firstnode != null)
                                            if (firstnode.First().Attributes.Contains("style"))
                                            {
                                                firstnode.First().Attributes["style"].Value = firstnode.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                firstnode.First().Attributes["style"].Value += ";display:block";
                                            }
                                            else
                                                firstnode.First().Attributes.Add("style", "display:block");
                                    }
                                    //rendiamo visibile il contenitore superiore
                                    if (node.ParentNode != null)
                                        if (node.ParentNode.Attributes.Contains("style"))
                                        {
                                            node.ParentNode.Attributes["style"].Value = node.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.ParentNode.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.ParentNode.Attributes.Add("style", "display:block");
                                    //rendiamo visibile il titolo se presente
                                    if (node.ParentNode.ParentNode != null)
                                    {
                                        var titleelement = node.ParentNode.ParentNode.Descendants().Where(c => c.Id == (dictpars["container"] + "Title"));
                                        if (titleelement != null && (titleelement.Count() > 0))
                                        {
                                            if (titleelement.First().Attributes.Contains("style"))
                                            {
                                                titleelement.First().Attributes["style"].Value = titleelement.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                titleelement.First().Attributes["style"].Value += ";display:block";
                                            }
                                            else
                                                titleelement.First().Attributes.Add("style", "display:block");
                                        }
                                    }
                                    //if (node.ParentNode.ParentNode != null)
                                    //    if (node.ParentNode.ParentNode.Attributes.Contains("style"))
                                    //    {
                                    //        node.ParentNode.ParentNode.Attributes["style"].Value = node.ParentNode.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    //        node.ParentNode.ParentNode.Attributes["style"].Value += ";display:block";
                                    //    }
                                    //    else
                                    //        node.ParentNode.ParentNode.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        jscommands[Session.SessionID].Add(container, WelcomeLibrary.UF.Utility.waitwrappercall("initScrollertype", "initScrollertype('" + dictpars["controlid"] + "','" + dictpars["container"] + "','" + dictpars["scrollertype"] + "');"));
                                        //jscommands[Session.SessionID].Add(container, "initScrollertype('" + dictpars["controlid"] + "','" + dictpars["container"] + "','" + dictpars["scrollertype"] + "');");
                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING //////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }

                        break;
                    case "injectandloadgenericcontent":
                        // injectandloadgenericcontent(type, container, controlid, visualData, visualPrezzo, iditem)

                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3) dictpars.Add("controlid", pars[3]);
                        if (pars.Count > 4) dictpars.Add("visualData", pars[4]);
                        if (pars.Count > 5) dictpars.Add("visualPrezzo", pars[5]);
                        if (pars.Count > 6) dictpars.Add("id", pars[6]);

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("id")) return;

                        //////////////////////////////////////////////////
                        //Ricarichiamo dalla session eventuali parametri aggiuntivi non passati nella chiamata di bind ma presenti in sessione
                        //////////////////////////////////////////////////
#if false
                            if (Session != null && Session["objfiltro"] != null)
                        {
                            string retval = Session["objfiltro"].ToString();//Prendo dalla sessione la chiave che contiene i parametri aggiuntivi serializzati
                            if (retval != null && retval != "")
                            {
                                Dictionary<string, string> dictparsfromsession = new Dictionary<string, string>();
                                dictparsfromsession = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(retval);
                                if (dictparsfromsession != null)
                                {
                                    bool skipsessionfilters = false;
                                    if (dictpars.ContainsKey("tipologia") && dictparsfromsession.ContainsKey("tipologia") && dictpars["tipologia"] != dictparsfromsession["tipologia"])
                                        skipsessionfilters = true;
                                    //if (dictpars.ContainsKey("categoria") && dictparsfromsession.ContainsKey("categoria") && dictpars["categoria"] != dictparsfromsession["categoria"])
                                    //    skipsessionfilters = true;
                                    //if (dictpars.ContainsKey("categoria2Liv") && dictparsfromsession.ContainsKey("categoria2Liv") && dictpars["categoria2Liv"] != dictparsfromsession["categoria2Liv"])
                                    //    skipsessionfilters = true;

                                    if (!skipsessionfilters)
                                        foreach (KeyValuePair<string, string> kv in dictparsfromsession)
                                        {
                                            //if (kv.Key == ("page" + dictpars["controlid"]))
                                            //    dictpagerpars["page"] = kv.Value;//la pagina la prendo dalla sessione se presente!
                                            //aggiungo i parametri dalla sessione se presenti
                                            if (!dictpars.ContainsKey(kv.Key)) dictpars.Add(kv.Key, kv.Value);
                                            else dictpars[kv.Key] = kv.Value;//sovrascivo il valore passato
                                        }
                                }
                            }
                        }
#endif


                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE  (schedadetails.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind

                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICHIAMO IL IL TEMPLATE PER LA GALLERY  injectFlexsliderControls(controlid, "plhGallery" )
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            string templategallery = "";
                            if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\flexslidergallery.html"))
                                templategallery = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\flexslidergallery.html");
                            if (!string.IsNullOrEmpty(templategallery))
                            {
                                templategallery = templategallery.Replace("replaceid", dictpars["controlid"]);
                                var flexgallerycontainer = template.DocumentNode.Descendants().Where(c => c.Id.ToLower() == "plhgallery");

                                //agiunta funzione skip prima foto
                                if ((flexgallerycontainer != null) && (flexgallerycontainer.Count() > 0))
                                {
                                    if (flexgallerycontainer.First().Attributes.Contains("myvalue1"))
                                    {

                                        if (flexgallerycontainer.First().Attributes["myvalue1"].Value.ToLower() == "skip")
                                            templategallery = templategallery.Replace("skip=\"\"", "skip=\"true\"");
                                    }


                                    if (flexgallerycontainer.First().Attributes.Contains("myvalue2"))
                                    {

                                        if (flexgallerycontainer.First().Attributes["myvalue2"].Value.ToLower() != "")
                                            templategallery = templategallery.Replace("max-height=\"\"", "max-height=\"" + flexgallerycontainer.First().Attributes["myvalue2"].Value + "\"");
                                    }
                                }

                                if ((flexgallerycontainer != null) && (flexgallerycontainer.Count() > 0))
                                {
                                    HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                    tmpdoc.LoadHtml(templategallery);
                                    flexgallerycontainer.First().AppendChild(tmpdoc.DocumentNode.Clone());
                                }
                            }
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = offerteDM.filterData(Lingua, dictpars, "1", "1", "false", Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Offerte> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Offerte>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]); //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
                                {
                                    string innerelement = elementtoappend.First().InnerHtml;
                                    if ((innerelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(innerelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        foreach (Offerte item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }
                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                        }
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    if (elementtoappend != null)
                                    {
                                        //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                        node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    }
                                    //Rendiamo visibile il primo div contenitore del template
                                    var firstnode = template.DocumentNode.Descendants("div");
                                    if ((firstnode != null) && (firstnode.Count() > 0))
                                    {
                                        if (firstnode != null)
                                            if (firstnode.First().Attributes.Contains("style"))
                                            {
                                                firstnode.First().Attributes["style"].Value = firstnode.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                firstnode.First().Attributes["style"].Value += ";display:block";
                                            }
                                            else
                                                firstnode.First().Attributes.Add("style", "display:block");
                                    }
                                    //rendiamo visibile il contenitore  
                                    if (node != null)
                                        if (node.Attributes.Contains("style"))
                                        {
                                            node.Attributes["style"].Value = node.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.Attributes.Add("style", "display:block");
                                    //rendiamo visibile il titolo se presente
                                    //if (node.ParentNode.ParentNode != null)
                                    //{
                                    //    var titleelement = node.ParentNode.ParentNode.Descendants().Where(c => c.Id == (dictpars["container"] + "Title"));
                                    //    if (titleelement != null && (titleelement.Count() > 0))
                                    //    {
                                    //        if (titleelement.First().Attributes.Contains("style"))
                                    //        {
                                    //            titleelement.First().Attributes["style"].Value = titleelement.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    //            titleelement.First().Attributes["style"].Value += ";display:block";
                                    //        }
                                    //        else
                                    //            titleelement.First().Attributes.Add("style", "display:block");
                                    //    }
                                    //}
                                    //if (node.ParentNode.ParentNode != null)
                                    //    if (node.ParentNode.ParentNode.Attributes.Contains("style"))
                                    //    {
                                    //        node.ParentNode.ParentNode.Attributes["style"].Value = node.ParentNode.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    //        node.ParentNode.ParentNode.Attributes["style"].Value += ";display:block";
                                    //    }
                                    //    else
                                    //        node.ParentNode.ParentNode.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container)) jscommands[Session.SessionID].Remove(container);
                                        //jscommands[Session.SessionID].Add(container, "inizializzaFlexsliderGallery('" + dictpars["controlid"] + "','" + dictpars["container"] + "');");
                                        jscommands[Session.SessionID].Add(container, WelcomeLibrary.UF.Utility.waitwrappercall("inizializzaFlexsliderGallery", "inizializzaFlexsliderGallery('" + dictpars["controlid"] + "','" + dictpars["container"] + "');"));
                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectportfolioandload":
                        // injectPortfolioAndLoad(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, mostviewed,objfiltro) 
                        //return;

                        if (!dictpars.ContainsKey("maincontainertext")) dictpars.Add("maincontainertext", WelcomeLibrary.UF.dataManagement.EncodeToBase64(node.OuterHtml)); //memorizzo l'elemento da bindare ai dati per utilizzo del sistema di pager per riuso dopo paginazione

                        //if (!dictpars.ContainsKey("maincontainertext")) dictpars.Add("maincontainertext", WelcomeLibrary.UF.dataManagement.EncodeToBase64(node.ParentNode.OuterHtml)); //memorizzo l'elemento padrea cui appendere tutto per utilizzo del sistema di pager
                        //if (!dictpars.ContainsKey("maincontainertext")) dictpars.Add("maincontainertext", node.ParentNode.OuterHtml.Replace("\"", "\\\"").Replace("'", "|")); //memorizzo l'elemento padrea cui appendere tutto per utilizzo del sistema di pager
                        //if (!dictpars.ContainsKey("maincontainertext")) dictpars.Add("maincontainertext", Newtonsoft.Json.JsonConvert.SerializeObject(node.ParentNode.OuterHtml)); //memorizzo l'elemento padrea cui appendere tutto per utilizzo del sistema di pager
                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0 && !dictpars.ContainsKey("functionname")) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1 && !dictpars.ContainsKey("templateHtml")) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2 && !dictpars.ContainsKey("container")) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3 && !dictpars.ContainsKey("controlid")) dictpars.Add("controlid", pars[3]);
                        if (pars.Count > 7 && !dictpars.ContainsKey("listShow")) dictpars.Add("listShow", pars[7]);
                        if (pars.Count > 8 && !dictpars.ContainsKey("tipologia")) dictpars.Add("tipologia", pars[8]);
                        if (pars.Count > 9 && !dictpars.ContainsKey("categoria")) dictpars.Add("categoria", pars[9]);
                        if (pars.Count > 10 && !dictpars.ContainsKey("visualData")) dictpars.Add("visualData", pars[10]);
                        if (pars.Count > 11 && !dictpars.ContainsKey("visualPrezzo")) dictpars.Add("visualPrezzo", pars[11]);
                        if (pars.Count > 12 && !dictpars.ContainsKey("maxelement")) dictpars.Add("maxelement", pars[12]);
                        if (pars.Count > 13 && !dictpars.ContainsKey("testoricerca")) dictpars.Add("testoricerca", pars[13]);
                        if (pars.Count > 14 && !dictpars.ContainsKey("vetrina")) dictpars.Add("vetrina", pars[14]);
                        if (pars.Count > 15 && !dictpars.ContainsKey("promozioni")) dictpars.Add("promozioni", pars[15]);
                        if (pars.Count > 16 && !dictpars.ContainsKey("connectedid")) dictpars.Add("connectedid", pars[16]);
                        if (pars.Count > 17 && !dictpars.ContainsKey("categoria2Liv")) dictpars.Add("categoria2Liv", pars[17]);
                        if (pars.Count > 18 && !dictpars.ContainsKey("mostviewed")) dictpars.Add("mostviewed", pars[18]);
                        if (pars.Count > 19 && !string.IsNullOrWhiteSpace(pars[19]) && !dictpars.ContainsKey("objfiltro")) dictpars.Add("objfiltro", pars[19]);
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 4 && !dictpagerpars.ContainsKey("page")) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5 && !dictpagerpars.ContainsKey("pagesize")) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6 && !dictpagerpars.ContainsKey("enablepager")) dictpagerpars.Add("enablepager", pars[6]);

                        try
                        {
                            ////////////////////////////////////////////////
                            // Se ho passatto dei parametri aggiuntivi alla funzione li sposto nella collection di filtro ( E SONO PRIORITARI RISPETTO ALLA SESSIONE )
                            ////////////////////////////////////////////////
                            bool flag_addedpars1 = false;
                            if (dictpars.ContainsKey("objfiltro"))
                            {
                                //dataManagement.EncodeToBase64
                                dictpars["objfiltro"] = dataManagement.DecodeFromBase64(dictpars["objfiltro"]);//mi aspetto i parametri con formato json serializzato e convertito base64
                                Dictionary<string, string> dictparsadded = new Dictionary<string, string>();
                                dictparsadded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictpars["objfiltro"]);
                                if (dictparsadded != null)
                                {
                                    foreach (KeyValuePair<string, string> kv in dictparsadded)
                                    {
                                        //aggiungo i parametri   se presenti
                                        if (!dictpars.ContainsKey(kv.Key)) dictpars.Add(kv.Key, kv.Value);
                                        else dictpars[kv.Key] = kv.Value;//sovrascivo il valore passato dando la priorita a quello messo in sessione
                                        flag_addedpars1 = true;
                                    }
                                }
                            }
#if true
                            //////////////////////////////////////////////////
                            //Ricarichiamo dalla session eventuali parametri aggiuntivi non passati nella chiamata di bind ma presenti in sessione
                            //////////////////////////////////////////////////
                            if (!flag_addedpars1)
                                if (Session != null && Session["objfiltro"] != null)
                                {
                                    string retval = Session["objfiltro"].ToString();//Prendo dalla sessione la chiave che contiene i parametri aggiuntivi serializzati
                                    if (retval != null && retval != "")
                                    {
                                        Dictionary<string, string> dictparsfromsession = new Dictionary<string, string>();
                                        dictparsfromsession = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(retval);
                                        if (dictparsfromsession != null)
                                        {
                                            bool skipsessionfilters = false;
                                            if (dictpars.ContainsKey("tipologia") && dictparsfromsession.ContainsKey("tipologia") && dictpars["tipologia"] != dictparsfromsession["tipologia"])
                                                skipsessionfilters = true;
                                            //if (dictpars.ContainsKey("categoria") && dictparsfromsession.ContainsKey("categoria") && dictpars["categoria"] != dictparsfromsession["categoria"])
                                            //    skipsessionfilters = true;
                                            //if (dictpars.ContainsKey("categoria2Liv") && dictparsfromsession.ContainsKey("categoria2Liv") && dictpars["categoria2Liv"] != dictparsfromsession["categoria2Liv"])
                                            //    skipsessionfilters = true;

                                            if (!skipsessionfilters)
                                                foreach (KeyValuePair<string, string> kv in dictparsfromsession)
                                                {
                                                    //if (kv.Key == ("page" + dictpars["controlid"]))
                                                    //    dictpagerpars["page"] = kv.Value;//la pagina la prendo dalla sessione se presente!
                                                    //aggiungo i parametri dalla sessione se presenti
                                                    if (!dictpars.ContainsKey(kv.Key)) dictpars.Add(kv.Key, kv.Value);
                                                    else dictpars[kv.Key] = kv.Value;//sovrascivo il valore passato dando la priorita a quello messo in sessione
                                                }
                                        }
                                    }
                                }
#endif
                        }
                        catch { }
                        //////////////////////////////////////
                        //Se presente la quesrystring pagino con quella (PRIORITARA)
                        if (Richiesta != null)
                        {
                            if (Richiesta.QueryString.AllKeys.Contains("page"))
                            {
                                string pagina = Richiesta.QueryString.GetValues("page")[0];
                                dictpagerpars["page"] = pagina;
                            }
                        }
                        /////////////////////////////////////

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;

                        //Se non presente inserisco un nodo contenitore al quello passato
                        //var newNode = HtmlNode.CreateNode(node.OuterHtml.Replace(node.OuterHtml, "<div class=\"containernode\">" + node.OuterHtml + "</div>"));
                        //node.ParentNode.ReplaceChild(newNode, node);

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE VISUALIZZAZIONE  (isotopeOfferte.html)
                        ///////////////////////////////////////////////////////////
                        if (!dictpars["templateHtml"].Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/lib/template/"))
                            dictpars["templateHtml"] = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/lib/template/" + dictpars["templateHtml"]; //Memorizzo l'url per il caricamento del template
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = offerteDM.filterData(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"], Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Offerte> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Offerte>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                if (resultinfo != null && resultinfo.ContainsKey("totalrecords"))
                                    dictpagerpars.Add("totalrecords", resultinfo["totalrecords"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                //////////////////////////////////////////////////////////////////////////////////////////////////// 
                                //CARICAMENTO E INIZIALIZZAZIONE DELLA PAGINAZIONE SE RICHIESTO////////////////////////////
                                //////////////////////////////////////////////////////////////////////////////////////////////////// 
                                if (dictpagerpars["enablepager"] == "true")
                                {

                                    //Se non presente appendiamo il div per il pager
                                    //"<div id=\"containeridListPager\"></div> //Inserisco questo elemento se non presente
                                    var pagernode = node.ParentNode.SelectNodes("//*[contains(@id,'" + dictpars["container"] + "Pager')]");
                                    if (pagernode == null || pagernode.Count == 0)
                                    {
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporaneo
                                        tmpdoc.LoadHtml("<div id=\"" + dictpars["container"] + "Pager\"></div>");
                                        //node.ParentNode.InsertAfter(tmpdoc.DocumentNode.Clone(), node.SelectSingleNode("//*[@id='" + dictpars["container"] + "']"));
                                        //node.SelectSingleNode("//*[@id='" + dictpars["container"] + "']").ParentNode.AppendChild(tmpdoc.DocumentNode.Clone());
                                        //node.ParentNode.AppendChild(tmpdoc.DocumentNode.Clone()); //Modifica la collection padre del ciclo e va in errore
                                        node.SelectSingleNode("//*[@id='" + dictpars["container"] + "']").AppendChild(tmpdoc.DocumentNode.Clone());
                                    }


                                    string templatepager = "";
                                    if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + "pagerIsotope.html"))
                                        templatepager = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + "pagerIsotope.html");
                                    if (!string.IsNullOrEmpty(templatepager))
                                    {
                                        templatepager = templatepager.Replace("replaceid", dictpars["controlid"]);
                                        if (node.ParentNode != null)
                                        {
                                            var pagercontainer = node.ParentNode.Descendants().Where(c => c.Id == dictpars["container"] + "Pager");
                                            if ((pagercontainer != null) && (pagercontainer.Count() > 0))
                                            {
                                                HtmlDocument tmpdoc = new HtmlDocument();//Documento temporaneo
                                                tmpdoc.LoadHtml(templatepager);
                                                pagercontainer.First().AppendChild(tmpdoc.DocumentNode.Clone());
                                                ///////////////////////////////////////////////////////////////////////////////////
                                                //Inizializziamo la visualizzazione del pager ( lato server ) sostituisce renderpager lato client
                                                ///////////////////////////////////////////////////////////////////////////////////
                                                int totalrecords = 0; int.TryParse(dictpagerpars["totalrecords"], out totalrecords);
                                                int page = 1; int.TryParse(dictpagerpars["page"], out page);
                                                int pagesize = 1; int.TryParse(dictpagerpars["pagesize"], out pagesize);
                                                int pagesnumber = (int)System.Math.Ceiling((Double)totalrecords / (Double)pagesize);
                                                if (page > pagesnumber) { page = pagesnumber; }
                                                if (page < 1) { page = 1; }
                                                dictpagerpars["page"] = page.ToString();

                                                string prevpage = (page - 1 < 1) ? "1" : (page - 1).ToString();
                                                string nextpage = (page + 1 > pagesnumber) ? pagesnumber.ToString() : (page + 1).ToString();

                                                //Accendo o spengo il pager
                                                if (pagesnumber > 1)
                                                {
                                                    if ((pagercontainer != null) && (pagercontainer.Count() > 0))
                                                        if (pagercontainer.First().Attributes.Contains("style"))
                                                        {
                                                            pagercontainer.First().Attributes["style"].Value = pagercontainer.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                            pagercontainer.First().Attributes["style"].Value += ";display:block";
                                                        }
                                                        else
                                                            pagercontainer.First().Attributes.Add("style", "display:block");
                                                    var pagerinner = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "divPager");
                                                    if ((pagerinner != null) && (pagerinner.Count() > 0))
                                                        if (pagerinner.First().Attributes.Contains("style"))
                                                        {
                                                            pagerinner.First().Attributes["style"].Value = pagerinner.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                            pagerinner.First().Attributes["style"].Value += ";display:block";
                                                        }
                                                        else
                                                            pagerinner.First().Attributes.Add("style", "display:block");
                                                }
                                                ///////////////////////////////////////////////////////////////////////////////////
                                                //Inizializziamo i tasti avanti e indietro del pager e valorizziamo le etichette di testo   sostituisce renderpager lato client e anche inithtmlpager
                                                ///////////////////////////////////////////////////////////////////////////////////
                                                var btnadd = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnAddcontent");
                                                if ((btnadd != null) && (btnadd.Count() > 0))
                                                {
                                                    btnadd.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore + " add";
                                                    btnadd.First().SetAttributeValue("onClick", "javascript:addcontentbindonserver('" + dictpars["controlid"] + "')");

                                                    if (btnadd.First().Attributes.Contains("style"))
                                                    {
                                                        btnadd.First().Attributes["style"].Value = btnadd.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        btnadd.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        btnadd.First().Attributes.Add("style", "display:block");
                                                }

                                                ///////////////////////////////////////////////////////////////////////////////////
                                                ///////////PAGINAZIONE PER LINK CON QUERYSTRING
                                                ///////////////////////////////////////////////////////////////////////////////////
#if true

                                                string testoricerca = "";
                                                if (dictpars.ContainsKey("testoricerca")) testoricerca = dictpars["testoricerca"];

                                                //Contenitore numeridi pagina
                                                string linkprev = "";
                                                string linknext = "";

                                                var aNextPage = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "aNextPage");
                                                if ((aNextPage != null) && (aNextPage.Count() > 0) && Richiesta != null && page < pagesnumber)
                                                {
                                                    aNextPage.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore;
                                                    //aNextPage.First().SetAttributeValue("onClick", "javascript:nextpagebindonserver('" + dictpars["controlid"] + "')");
                                                    linknext = Richiesta.Url.LocalPath + "?" + "page=" + nextpage;//Imposto la next page per l'head
                                                    if (!string.IsNullOrEmpty(testoricerca)) linknext += "&testoricerca=" + testoricerca;
                                                    aNextPage.First().SetAttributeValue("href", linknext);


                                                    if (aNextPage.First().Attributes.Contains("style"))
                                                    {
                                                        aNextPage.First().Attributes["style"].Value = aNextPage.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        aNextPage.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        aNextPage.First().Attributes.Add("style", "display:block");
                                                }

                                                var aPrevPage = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "aPrevPage");
                                                if ((aPrevPage != null) && (aPrevPage.Count() > 0) && Richiesta != null && page > 1)
                                                {
                                                    aPrevPage.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pagerindietro").Valore;
                                                    //aPrevPage.First().SetAttributeValue("onClick", "javascript:nextpagebindonserver('" + dictpars["controlid"] + "')");

                                                    if (prevpage != "1")
                                                        linkprev = Richiesta.Url.LocalPath + "?" + "page=" + prevpage;//Imposto la next page per l'head
                                                    else
                                                        linkprev = Richiesta.Url.LocalPath;//Imposto la next page per l'head

                                                    if (!string.IsNullOrEmpty(testoricerca)) { linkprev += (linkprev.Contains("?")) ? "&testoricerca=" + testoricerca : "?testoricerca=" + testoricerca; }


                                                    aPrevPage.First().SetAttributeValue("href", linkprev);

                                                    if (aPrevPage.First().Attributes.Contains("style"))
                                                    {
                                                        aPrevPage.First().Attributes["style"].Value = aPrevPage.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        aPrevPage.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        aPrevPage.First().Attributes.Add("style", "display:block");
                                                }

                                                if (Session != null)
                                                {
                                                    Session.Remove("linkprev"); //link pagina precendente
                                                    Session.Remove("linknext"); //link pagina successiva
                                                }
                                                var pnumbers = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "pagenumbers");
                                                if ((pnumbers != null) && (pnumbers.Count() > 0) && Richiesta != null)//&& page > 1)
                                                {

                                                    //elementi da creare da 1 a  pagesnumber da accorare a pnumbers
                                                    HtmlDocument tmpdoc1 = new HtmlDocument();//Documento temporaneo
                                                    StringBuilder htmltoappend = new StringBuilder();
                                                    //Creiamo gli elementi <li class="page-item"><a class="page-link" href="#">1</a></li> 
                                                    // se attivo      <li class="page-item active"><a class="page-link" href="#">1</a></li> 

                                                    string basepagelink = Richiesta.Url.LocalPath;
                                                    if (!string.IsNullOrEmpty(testoricerca)) { basepagelink += (basepagelink.Contains("?")) ? "&testoricerca=" + testoricerca : "?testoricerca=" + testoricerca; }

                                                    if (page > 3)
                                                        htmltoappend.Append("<li class=\"page-item  pt-1\"><a class=\"page-link\" href=\"" + basepagelink + "\"><<</a></li> ");
                                                    if (!string.IsNullOrEmpty(linkprev))
                                                        htmltoappend.Append("<li class=\"page-item  pt-1\"><a class=\"page-link\" href=\"" + linkprev + "\"><</a></li> ");
                                                    for (int p = 1; p <= pagesnumber; p++)
                                                    {
                                                        if (((p >= page - 2 && p <= page + 2) && page > 2 && page < pagesnumber - 2) || (page <= 2 && (p <= 5)) || (page >= pagesnumber - 2 && (p > (pagesnumber - 5))))
                                                        {
                                                            string disabled = "";//disabled
                                                            string active = ""; //active
                                                            string linkpagina = Richiesta.Url.LocalPath + "?" + "page=" + p;
                                                            if (!string.IsNullOrEmpty(testoricerca)) { linkpagina += (linkpagina.Contains("?")) ? "&testoricerca=" + testoricerca : "?testoricerca=" + testoricerca; }


                                                            active = (p == page) ? active = "active" : active = "";
                                                            if (active != "active")
                                                                htmltoappend.Append("<li class=\"page-item " + active + " pt-1\"><a class=\"page-link\" href=\"" + linkpagina + "\">" + p + "</a></li> ");
                                                            else
                                                                htmltoappend.Append("<li class=\"page-item " + active + " pt-1\"><span class=\"page-link\" style=\"cursor:default\">" + p + "</span></li> ");



                                                        }
                                                    }
                                                    if (linknext != "")
                                                        htmltoappend.Append("<li class=\"page-item  pt-1\"><a class=\"page-link\" href=\"" + linknext + "\">></a></li> ");

                                                    if (page < pagesnumber - 2)
                                                    {
                                                        string basepagelinkactnum = Richiesta.Url.LocalPath + "?" + "page=" + pagesnumber;
                                                        if (!string.IsNullOrEmpty(testoricerca)) { basepagelinkactnum += (basepagelinkactnum.Contains("?")) ? "&testoricerca=" + testoricerca : "?testoricerca=" + testoricerca; }
                                                        htmltoappend.Append("<li class=\"page-item  pt-1\"><a class=\"page-link\" href=\"" + basepagelinkactnum + "\">>></a></li> ");
                                                    }
                                                    tmpdoc1.LoadHtml(htmltoappend.ToString());
                                                    pnumbers.First().AppendChild(tmpdoc1.DocumentNode.Clone());
                                                    if (pnumbers.First().Attributes.Contains("style"))
                                                    {
                                                        pnumbers.First().Attributes["style"].Value = pnumbers.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        pnumbers.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        pnumbers.First().Attributes.Add("style", "display:block");
                                                }



                                                //Metto in sessione i link precedente e successivo
                                                if (Session != null)
                                                {
                                                    if (!string.IsNullOrEmpty(linknext))
                                                        Session["linknext"] = linknext;
                                                    if (!string.IsNullOrEmpty(linkprev))
                                                        Session["linkprev"] = linkprev;
                                                }

#endif
                                                ///////////PAGINAZIONE PER LINK CON QUERYSTRING

                                                ///////////PAGINAZIONE CON FUNZIONI JAVASCRIPT E CARICAMENTO LATO SERVER
#if false
                                                var btnnext1 = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnNextPage1");
                                                if ((btnnext1 != null) && (btnnext1.Count() > 0))
                                                {
                                                    btnnext1.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore;
                                                    btnnext1.First().SetAttributeValue("onClick", "javascript:nextpagebindonserver('" + dictpars["controlid"] + "')");

                                                    if (btnnext1.First().Attributes.Contains("style"))
                                                    {
                                                        btnnext1.First().Attributes["style"].Value = btnnext1.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        btnnext1.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        btnnext1.First().Attributes.Add("style", "display:block");
                                                }

                                                var btnprev1 = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnPrevPage1");
                                                if ((btnprev1 != null) && (btnprev1.Count() > 0))
                                                {
                                                    btnprev1.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pagerindietro").Valore;
                                                    btnprev1.First().SetAttributeValue("onClick", "javascript:prevpagebindonserver('" + dictpars["controlid"] + "')");

                                                    if (btnprev1.First().Attributes.Contains("style"))
                                                    {
                                                        btnprev1.First().Attributes["style"].Value = btnprev1.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        btnprev1.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        btnprev1.First().Attributes.Add("style", "display:block");
                                                } 
#endif
                                                ///////////PAGINAZIONE CON FUNZIONI JAVASCRIPT E CARICAMENTO LATO SERVER

                                                //Pulsanti per chiamate lato client!
#if false
                                                var btnnext = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnNextPage");
                                                if ((btnnext != null) && (btnnext.Count() > 0))
                                                {
                                                    btnnext.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore;
                                                    btnnext.First().SetAttributeValue("onClick", "javascript:nextpage('" + dictpars["controlid"] + "')");
                                                }
                                                var btnprev = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnPrevPage");
                                                if ((btnprev != null) && (btnprev.Count() > 0))
                                                {
                                                    btnprev.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pagerindietro").Valore;
                                                    btnprev.First().SetAttributeValue("onClick", "javascript:prevpage('" + dictpars["controlid"] + "')");
                                                } 
                                                ////////////////////////////////////
#endif
                                                var lbltototali = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "spantotals");
                                                if ((lbltototali != null) && (lbltototali.Count() > 0))
                                                    lbltototali.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pagertotale").Valore + " " + totalrecords.ToString() + "<br/>";
                                                var lblactpage = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "divactpage");
                                                if ((lblactpage != null) && (lblactpage.Count() > 0))
                                                    lblactpage.First().InnerHtml = page + "/" + pagesnumber;

                                            }
                                        }
                                    }
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]); //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
                                {

                                    bool alternate = false;
                                    if (elementtoappend.First().Attributes.Contains("class") && elementtoappend.First().Attributes["class"].Value.Contains("alternatecolor"))
                                        alternate = true;

                                    string innerelement = elementtoappend.First().InnerHtml;
                                    if ((innerelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(innerelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        int j = 0;
                                        foreach (Offerte item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);

                                                }

                                            ////////////////////ODD EVEN GESTIONE SFONDO
                                            if (alternate)
                                            {
                                                if (IsEven(j))
                                                {

                                                    var nodestoremove = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("odd"));
                                                    if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                    {
                                                        nodestoremove.First().Remove();
                                                    }
                                                }
                                                else
                                                {

                                                    var nodestoremove = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("even"));
                                                    if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                        if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                        {
                                                            nodestoremove.First().Remove();
                                                        }
                                                }

                                            }
                                            /////////////////////////////////////////////

                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                            j++;
                                        }
                                    }
                                    //Rendiamo visibile il primo div contenitore del template
                                    var firstnode = template.DocumentNode.Descendants("div");
                                    if ((firstnode != null) && (firstnode.Count() > 0))
                                    {
                                        if (firstnode != null)
                                            if (firstnode.First().Attributes.Contains("style"))
                                            {
                                                firstnode.First().Attributes["style"].Value = firstnode.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                firstnode.First().Attributes["style"].Value += ";display:block";
                                            }
                                            else
                                                firstnode.First().Attributes.Add("style", "display:block");
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //var nodeconteinerisotope = node.SelectSingleNode("//*[@id='" + dictpars["container"] + "']");
                                    //nodeconteinerisotope.RemoveAllChildren();
                                    //var clonedpager = node.SelectSingleNode("//*[@id='" + dictpars["container"] + "Pager']").Clone(); //Clono il pager per non cancellarlo
                                    HtmlNode clonedpager = null; //Clono il pager per non cancellarlo
                                    var childelems = node.ChildNodes.Descendants().Where(n => n.Id == (dictpars["container"] + "Pager"));
                                    if ((childelems != null) && (childelems.Count() > 0))
                                    {
                                        //Clono il pager per non cancellarlo
                                        clonedpager = childelems.First().Clone();
                                    }
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    if (clonedpager != null && node.ParentNode.SelectSingleNode("//*[@id='" + dictpars["container"] + "Pager']") == null)
                                        node.AppendChild(clonedpager); //Appendo il pager


                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )


                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {

                                        //Aggiorno le variaibli javascript globalObject[controlid + "params"] ( da dictpars) e globalObject[controlid + "pagerdata" ] ( da dictpagerpars ) per far funzionare il pager lato client !!!
                                        //La seguente prepara una chiamata a funzione javascript che inizializza le variabili js. ( SERVE SOLO PER UTILIZZO LATO CLIENT )
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container + "-2")) jscommands[Session.SessionID].Remove(container + "-2");
                                        jscommands[Session.SessionID].Add(container + "-2", WelcomeLibrary.UF.Utility.waitwrappercall("initGlobalVarsFromServer", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');"));
                                        //jscommands[Session.SessionID].Add(container + "-2", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');");

                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        if (jscommands[Session.SessionID].ContainsKey(container + "-1")) jscommands[Session.SessionID].Remove(container + "-1");
                                        //jscommands[Session.SessionID].Add(container + "-1", "InitIsotopeLocal('" + dictpars["controlid"] + "','" + dictpars["container"] + "');");
                                        jscommands[Session.SessionID].Add(container + "-1", WelcomeLibrary.UF.Utility.waitwrappercall("InitIsotopeLocal", "InitIsotopeLocal('" + dictpars["controlid"] + "','" + dictpars["container"] + "');"));

                                        //if (dictpagerpars["enablepager"] == "true")
                                        //{
                                        //jscommands[Session.SessionID].Add(container + "-2", "initHtmlPager('" + dictpars["controlid"] + "');");
                                        // jscommands[Session.SessionID].Add(container + "-3", "renderPager('" + dictpars["controlid"] + "');"); //QESUTA LA DEVI REPLICARE LATO SERBE USA LE RISORSE!!!!!! o gli devi passare le risporse necessarie!!
                                        //}
                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING //////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectbootstrapportfolioandload":
                        // injectbootstrapportfolioandload(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, mostviewed,objfiltro) 
                        //return;

                        if (!dictpars.ContainsKey("maincontainertext")) dictpars.Add("maincontainertext", WelcomeLibrary.UF.dataManagement.EncodeToBase64(node.OuterHtml)); //memorizzo l'elemento da bindare ai dati per utilizzo del sistema di pager per riuso dopo paginazione
                        //if (!dictpars.ContainsKey("maincontainertext")) dictpars.Add("maincontainertext", WelcomeLibrary.UF.dataManagement.EncodeToBase64(node.ParentNode.OuterHtml)); //memorizzo l'elemento padrea cui appendere tutto per utilizzo del sistema di pager
                        //if (!dictpars.ContainsKey("maincontainertext")) dictpars.Add("maincontainertext", node.ParentNode.OuterHtml.Replace("\"", "\\\"").Replace("'", "|")); //memorizzo l'elemento padrea cui appendere tutto per utilizzo del sistema di pager
                        //if (!dictpars.ContainsKey("maincontainertext")) dictpars.Add("maincontainertext", Newtonsoft.Json.JsonConvert.SerializeObject(node.ParentNode.OuterHtml)); //memorizzo l'elemento padrea cui appendere tutto per utilizzo del sistema di pager
                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0 && !dictpars.ContainsKey("functionname")) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1 && !dictpars.ContainsKey("templateHtml")) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2 && !dictpars.ContainsKey("container")) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3 && !dictpars.ContainsKey("controlid")) dictpars.Add("controlid", pars[3]);
                        if (pars.Count > 7 && !dictpars.ContainsKey("listShow")) dictpars.Add("listShow", pars[7]);
                        if (pars.Count > 8 && !dictpars.ContainsKey("tipologia")) dictpars.Add("tipologia", pars[8]);
                        if (pars.Count > 9 && !dictpars.ContainsKey("categoria")) dictpars.Add("categoria", pars[9]);
                        if (pars.Count > 10 && !dictpars.ContainsKey("visualData")) dictpars.Add("visualData", pars[10]);
                        if (pars.Count > 11 && !dictpars.ContainsKey("visualPrezzo")) dictpars.Add("visualPrezzo", pars[11]);
                        if (pars.Count > 12 && !dictpars.ContainsKey("maxelement")) dictpars.Add("maxelement", pars[12]);
                        if (pars.Count > 13 && !dictpars.ContainsKey("testoricerca")) dictpars.Add("testoricerca", pars[13]);
                        if (pars.Count > 14 && !dictpars.ContainsKey("vetrina")) dictpars.Add("vetrina", pars[14]);
                        if (pars.Count > 15 && !dictpars.ContainsKey("promozioni")) dictpars.Add("promozioni", pars[15]);
                        if (pars.Count > 16 && !dictpars.ContainsKey("connectedid")) dictpars.Add("connectedid", pars[16]);
                        if (pars.Count > 17 && !dictpars.ContainsKey("categoria2Liv")) dictpars.Add("categoria2Liv", pars[17]);
                        if (pars.Count > 18 && !dictpars.ContainsKey("mostviewed")) dictpars.Add("mostviewed", pars[18]);
                        if (pars.Count > 19 && !string.IsNullOrWhiteSpace(pars[19]) && !dictpars.ContainsKey("objfiltro")) dictpars.Add("objfiltro", pars[19]);
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 4 && !dictpagerpars.ContainsKey("page")) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5 && !dictpagerpars.ContainsKey("pagesize")) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6 && !dictpagerpars.ContainsKey("enablepager")) dictpagerpars.Add("enablepager", pars[6]);

                        try
                        {
                            ////////////////////////////////////////////////
                            // Se ho passatto dei parametri aggiuntivi alla funzione inject li sposto nella collection di filtro ( E SONO PRIORITARI RISPETTO ALLA SESSIONE )
                            ////////////////////////////////////////////////
                            bool flag_addedpars1 = false;
                            if (dictpars.ContainsKey("objfiltro"))
                            {
                                //dataManagement.EncodeToBase64
                                dictpars["objfiltro"] = dataManagement.DecodeFromBase64(dictpars["objfiltro"]);//mi aspetto i parametri con formato json serializzato e convertito base64
                                Dictionary<string, string> dictparsadded = new Dictionary<string, string>();
                                dictparsadded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictpars["objfiltro"]);
                                if (dictparsadded != null)
                                {
                                    foreach (KeyValuePair<string, string> kv in dictparsadded)
                                    {
                                        //aggiungo i parametri   se presenti
                                        if (!dictpars.ContainsKey(kv.Key)) dictpars.Add(kv.Key, kv.Value);
                                        else dictpars[kv.Key] = kv.Value;//sovrascivo il valore passato dando la priorita a quello messo in sessione
                                        flag_addedpars1 = true;
                                    }
                                }
                            }
#if true
                            //////////////////////////////////////////////////
                            //Ricarichiamo dalla session eventuali parametri aggiuntivi non passati nella chiamata di bind ma presenti in sessione
                            //SECONDARIO: LA priprità e ai prametri passati nella chiamata se presenti
                            //////////////////////////////////////////////////
                            if (!flag_addedpars1)
                                if (Session != null && Session["objfiltro"] != null)
                                {
                                    string retval = Session["objfiltro"].ToString();//Prendo dalla sessione la chiave che contiene i parametri aggiuntivi serializzati
                                    if (retval != null && retval != "")
                                    {
                                        Dictionary<string, string> dictparsfromsession = new Dictionary<string, string>();
                                        dictparsfromsession = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(retval);
                                        if (dictparsfromsession != null)
                                        {
                                            bool skipsessionfilters = false;
                                            if (dictpars.ContainsKey("tipologia") && dictparsfromsession.ContainsKey("tipologia") && dictpars["tipologia"] != dictparsfromsession["tipologia"])
                                                skipsessionfilters = true;
                                            //if (dictpars.ContainsKey("categoria") && dictparsfromsession.ContainsKey("categoria") && dictpars["categoria"] != dictparsfromsession["categoria"])
                                            //    skipsessionfilters = true;
                                            //if (dictpars.ContainsKey("categoria2Liv") && dictparsfromsession.ContainsKey("categoria2Liv") && dictpars["categoria2Liv"] != dictparsfromsession["categoria2Liv"])
                                            //    skipsessionfilters = true;

                                            if (!skipsessionfilters)
                                                foreach (KeyValuePair<string, string> kv in dictparsfromsession)
                                                {
                                                    //if (kv.Key == ("page" + dictpars["controlid"]))
                                                    //    dictpagerpars["page"] = kv.Value;//la pagina la prendo dalla sessione se presente!
                                                    //aggiungo i parametri dalla sessione se presenti
                                                    if (!dictpars.ContainsKey(kv.Key)) dictpars.Add(kv.Key, kv.Value);
                                                    else dictpars[kv.Key] = kv.Value;//sovrascivo il valore passato dando la priorita a quello messo in sessione
                                                }
                                        }
                                    }
                                }
#endif
                        }
                        catch { }
                        //////////////////////////////////////
                        //Se presente la quesrystring pagino con quella (PRIORITARA)
                        if (Richiesta != null)
                        {
                            if (Richiesta.QueryString.AllKeys.Contains("page"))
                            {
                                string pagina = Richiesta.QueryString.GetValues("page")[0];
                                dictpagerpars["page"] = pagina;
                            }
                        }
                        /////////////////////////////////////

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;

                        //Se non presente inserisco un nodo contenitore al quello passato
                        //var newNode = HtmlNode.CreateNode(node.OuterHtml.Replace(node.OuterHtml, "<div class=\"containernode\">" + node.OuterHtml + "</div>"));
                        //node.ParentNode.ReplaceChild(newNode, node);

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE VISUALIZZAZIONE  (isotopeOfferte.html)
                        ///////////////////////////////////////////////////////////
                        if (!dictpars["templateHtml"].Contains(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/lib/template/"))
                            dictpars["templateHtml"] = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/lib/template/" + dictpars["templateHtml"]; //Memorizzo l'url per il caricamento del template
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            Dictionary<string, string> dictdati = offerteDM.filterData(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"], Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Offerte> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Offerte>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                if (resultinfo != null && resultinfo.ContainsKey("totalrecords"))
                                    dictpagerpars.Add("totalrecords", resultinfo["totalrecords"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                //////////////////////////////////////////////////////////////////////////////////////////////////// 
                                //CARICAMENTO E INIZIALIZZAZIONE DELLA PAGINAZIONE SE RICHIESTO////////////////////////////
                                //////////////////////////////////////////////////////////////////////////////////////////////////// 
                                if (dictpagerpars["enablepager"] == "true")
                                {

                                    //Se non presente appendiamo il div per il pager
                                    //"<div id=\"containeridListPager\"></div> //Inserisco questo elemento se non presente
                                    var pagernode = node.ParentNode.SelectNodes("//*[contains(@id,'" + dictpars["container"] + "Pager')]");
                                    if (pagernode == null || pagernode.Count == 0)
                                    {
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporaneo
                                        tmpdoc.LoadHtml("<div id=\"" + dictpars["container"] + "Pager\"></div>");
                                        //node.ParentNode.InsertAfter(tmpdoc.DocumentNode.Clone(), node.SelectSingleNode("//*[@id='" + dictpars["container"] + "']"));
                                        //node.SelectSingleNode("//*[@id='" + dictpars["container"] + "']").ParentNode.AppendChild(tmpdoc.DocumentNode.Clone());
                                        //node.ParentNode.AppendChild(tmpdoc.DocumentNode.Clone()); //Modifica la collection padre del ciclo e va in errore
                                        node.SelectSingleNode("//*[@id='" + dictpars["container"] + "']").AppendChild(tmpdoc.DocumentNode.Clone());
                                    }


                                    string templatepager = "";
                                    if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + "pagerIsotope.html"))
                                        templatepager = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + "pagerIsotope.html");
                                    if (!string.IsNullOrEmpty(templatepager))
                                    {
                                        templatepager = templatepager.Replace("replaceid", dictpars["controlid"]);
                                        if (node.ParentNode != null)
                                        {
                                            var pagercontainer = node.ParentNode.Descendants().Where(c => c.Id == dictpars["container"] + "Pager");
                                            if ((pagercontainer != null) && (pagercontainer.Count() > 0))
                                            {
                                                HtmlDocument tmpdoc = new HtmlDocument();//Documento temporaneo
                                                tmpdoc.LoadHtml(templatepager);
                                                pagercontainer.First().AppendChild(tmpdoc.DocumentNode.Clone());
                                                ///////////////////////////////////////////////////////////////////////////////////
                                                //Inizializziamo la visualizzazione del pager ( lato server ) sostituisce renderpager lato client
                                                ///////////////////////////////////////////////////////////////////////////////////
                                                int totalrecords = 0; int.TryParse(dictpagerpars["totalrecords"], out totalrecords);
                                                int page = 1; int.TryParse(dictpagerpars["page"], out page);
                                                int pagesize = 1; int.TryParse(dictpagerpars["pagesize"], out pagesize);
                                                int pagesnumber = (int)System.Math.Ceiling((Double)totalrecords / (Double)pagesize);
                                                if (page > pagesnumber) { page = pagesnumber; }
                                                if (page < 1) { page = 1; }
                                                dictpagerpars["page"] = page.ToString();

                                                string prevpage = (page - 1 < 1) ? "1" : (page - 1).ToString();
                                                string nextpage = (page + 1 > pagesnumber) ? pagesnumber.ToString() : (page + 1).ToString();

                                                //Accendo o spengo il pager
                                                if (pagesnumber > 1)
                                                {
                                                    if ((pagercontainer != null) && (pagercontainer.Count() > 0))
                                                        if (pagercontainer.First().Attributes.Contains("style"))
                                                        {
                                                            pagercontainer.First().Attributes["style"].Value = pagercontainer.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                            pagercontainer.First().Attributes["style"].Value += ";display:block";
                                                        }
                                                        else
                                                            pagercontainer.First().Attributes.Add("style", "display:block");
                                                    var pagerinner = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "divPager");
                                                    if ((pagerinner != null) && (pagerinner.Count() > 0))
                                                        if (pagerinner.First().Attributes.Contains("style"))
                                                        {
                                                            pagerinner.First().Attributes["style"].Value = pagerinner.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                            pagerinner.First().Attributes["style"].Value += ";display:block";
                                                        }
                                                        else
                                                            pagerinner.First().Attributes.Add("style", "display:block");
                                                }
                                                ///////////////////////////////////////////////////////////////////////////////////
                                                //Inizializziamo i tasti avanti e indietro del pager e valorizziamo le etichette di testo   sostituisce renderpager lato client e anche inithtmlpager
                                                ///////////////////////////////////////////////////////////////////////////////////
                                                var btnadd = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnAddcontent");
                                                if ((btnadd != null) && (btnadd.Count() > 0))
                                                {
                                                    btnadd.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore + " add";
                                                    btnadd.First().SetAttributeValue("onClick", "javascript:addcontentbindonserver('" + dictpars["controlid"] + "')");

                                                    if (btnadd.First().Attributes.Contains("style"))
                                                    {
                                                        btnadd.First().Attributes["style"].Value = btnadd.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        btnadd.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        btnadd.First().Attributes.Add("style", "display:block");
                                                }

                                                ///////////////////////////////////////////////////////////////////////////////////
                                                ///////////PAGINAZIONE PER LINK CON QUERYSTRING
                                                ///////////////////////////////////////////////////////////////////////////////////
#if true

                                                string testoricerca = "";
                                                if (dictpars.ContainsKey("testoricerca")) testoricerca = dictpars["testoricerca"];

                                                //Contenitore numeridi pagina
                                                string linkprev = "";
                                                string linknext = "";

                                                var aNextPage = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "aNextPage");
                                                if ((aNextPage != null) && (aNextPage.Count() > 0) && Richiesta != null && page < pagesnumber)
                                                {
                                                    aNextPage.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore;
                                                    //aNextPage.First().SetAttributeValue("onClick", "javascript:nextpagebindonserver('" + dictpars["controlid"] + "')");
                                                    linknext = Richiesta.Url.LocalPath + "?" + "page=" + nextpage;//Imposto la next page per l'head
                                                    if (!string.IsNullOrEmpty(testoricerca)) linknext += "&testoricerca=" + testoricerca;
                                                    aNextPage.First().SetAttributeValue("href", linknext);


                                                    if (aNextPage.First().Attributes.Contains("style"))
                                                    {
                                                        aNextPage.First().Attributes["style"].Value = aNextPage.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        aNextPage.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        aNextPage.First().Attributes.Add("style", "display:block");
                                                }

                                                var aPrevPage = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "aPrevPage");
                                                if ((aPrevPage != null) && (aPrevPage.Count() > 0) && Richiesta != null && page > 1)
                                                {
                                                    aPrevPage.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pagerindietro").Valore;
                                                    //aPrevPage.First().SetAttributeValue("onClick", "javascript:nextpagebindonserver('" + dictpars["controlid"] + "')");

                                                    if (prevpage != "1")
                                                        linkprev = Richiesta.Url.LocalPath + "?" + "page=" + prevpage;//Imposto la next page per l'head
                                                    else
                                                        linkprev = Richiesta.Url.LocalPath;//Imposto la next page per l'head

                                                    if (!string.IsNullOrEmpty(testoricerca)) { linkprev += (linkprev.Contains("?")) ? "&testoricerca=" + testoricerca : "?testoricerca=" + testoricerca; }


                                                    aPrevPage.First().SetAttributeValue("href", linkprev);

                                                    if (aPrevPage.First().Attributes.Contains("style"))
                                                    {
                                                        aPrevPage.First().Attributes["style"].Value = aPrevPage.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        aPrevPage.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        aPrevPage.First().Attributes.Add("style", "display:block");
                                                }

                                                if (Session != null)
                                                {
                                                    Session.Remove("linkprev"); //link pagina precendente
                                                    Session.Remove("linknext"); //link pagina successiva
                                                }
                                                var pnumbers = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "pagenumbers");
                                                if ((pnumbers != null) && (pnumbers.Count() > 0) && Richiesta != null)//&& page > 1)
                                                {

                                                    //elementi da creare da 1 a  pagesnumber da accorare a pnumbers
                                                    HtmlDocument tmpdoc1 = new HtmlDocument();//Documento temporaneo
                                                    StringBuilder htmltoappend = new StringBuilder();
                                                    //Creiamo gli elementi <li class="page-item"><a class="page-link" href="#">1</a></li> 
                                                    // se attivo      <li class="page-item active"><a class="page-link" href="#">1</a></li> 

                                                    string basepagelink = Richiesta.Url.LocalPath;
                                                    if (!string.IsNullOrEmpty(testoricerca)) { basepagelink += (basepagelink.Contains("?")) ? "&testoricerca=" + testoricerca : "?testoricerca=" + testoricerca; }

                                                    if (page > 3)
                                                        htmltoappend.Append("<li class=\"page-item  pt-1\"><a class=\"page-link\" href=\"" + basepagelink + "\"><<</a></li> ");
                                                    if (!string.IsNullOrEmpty(linkprev))
                                                        htmltoappend.Append("<li class=\"page-item  pt-1\"><a class=\"page-link\" href=\"" + linkprev + "\"><</a></li> ");
                                                    for (int p = 1; p <= pagesnumber; p++)
                                                    {
                                                        if (((p >= page - 2 && p <= page + 2) && page > 2 && page < pagesnumber - 2) || (page <= 2 && (p <= 5)) || (page >= pagesnumber - 2 && (p > (pagesnumber - 5))))
                                                        {
                                                            string disabled = "";//disabled
                                                            string active = ""; //active
                                                            string linkpagina = Richiesta.Url.LocalPath + "?" + "page=" + p;
                                                            if (!string.IsNullOrEmpty(testoricerca)) { linkpagina += (linkpagina.Contains("?")) ? "&testoricerca=" + testoricerca : "?testoricerca=" + testoricerca; }


                                                            active = (p == page) ? active = "active" : active = "";
                                                            if (active != "active")
                                                                htmltoappend.Append("<li class=\"page-item " + active + " pt-1\"><a class=\"page-link\" href=\"" + linkpagina + "\">" + p + "</a></li> ");
                                                            else
                                                                htmltoappend.Append("<li class=\"page-item " + active + " pt-1\"><span class=\"page-link\" style=\"cursor:default\">" + p + "</span></li> ");



                                                        }
                                                    }
                                                    if (linknext != "")
                                                        htmltoappend.Append("<li class=\"page-item  pt-1\"><a class=\"page-link\" href=\"" + linknext + "\">></a></li> ");

                                                    if (page < pagesnumber - 2)
                                                    {
                                                        string basepagelinkactnum = Richiesta.Url.LocalPath + "?" + "page=" + pagesnumber;
                                                        if (!string.IsNullOrEmpty(testoricerca)) { basepagelinkactnum += (basepagelinkactnum.Contains("?")) ? "&testoricerca=" + testoricerca : "?testoricerca=" + testoricerca; }
                                                        htmltoappend.Append("<li class=\"page-item  pt-1\"><a class=\"page-link\" href=\"" + basepagelinkactnum + "\">>></a></li> ");
                                                    }
                                                    tmpdoc1.LoadHtml(htmltoappend.ToString());
                                                    pnumbers.First().AppendChild(tmpdoc1.DocumentNode.Clone());
                                                    if (pnumbers.First().Attributes.Contains("style"))
                                                    {
                                                        pnumbers.First().Attributes["style"].Value = pnumbers.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        pnumbers.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        pnumbers.First().Attributes.Add("style", "display:block");
                                                }



                                                //Metto in sessione i link precedente e successivo
                                                if (Session != null)
                                                {
                                                    if (!string.IsNullOrEmpty(linknext))
                                                        Session["linknext"] = linknext;
                                                    if (!string.IsNullOrEmpty(linkprev))
                                                        Session["linkprev"] = linkprev;
                                                }

#endif
                                                ///////////PAGINAZIONE PER LINK CON QUERYSTRING

                                                ///////////PAGINAZIONE CON FUNZIONI JAVASCRIPT E CARICAMENTO LATO SERVER
#if false
                                                var btnnext1 = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnNextPage1");
                                                if ((btnnext1 != null) && (btnnext1.Count() > 0))
                                                {
                                                    btnnext1.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore;
                                                    btnnext1.First().SetAttributeValue("onClick", "javascript:nextpagebindonserver('" + dictpars["controlid"] + "')");

                                                    if (btnnext1.First().Attributes.Contains("style"))
                                                    {
                                                        btnnext1.First().Attributes["style"].Value = btnnext1.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        btnnext1.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        btnnext1.First().Attributes.Add("style", "display:block");
                                                }

                                                var btnprev1 = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnPrevPage1");
                                                if ((btnprev1 != null) && (btnprev1.Count() > 0))
                                                {
                                                    btnprev1.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pagerindietro").Valore;
                                                    btnprev1.First().SetAttributeValue("onClick", "javascript:prevpagebindonserver('" + dictpars["controlid"] + "')");

                                                    if (btnprev1.First().Attributes.Contains("style"))
                                                    {
                                                        btnprev1.First().Attributes["style"].Value = btnprev1.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        btnprev1.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        btnprev1.First().Attributes.Add("style", "display:block");
                                                } 
#endif
                                                ///////////PAGINAZIONE CON FUNZIONI JAVASCRIPT E CARICAMENTO LATO SERVER

                                                //Pulsanti per chiamate lato client!
#if false
                                                var btnnext = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnNextPage");
                                                if ((btnnext != null) && (btnnext.Count() > 0))
                                                {
                                                    btnnext.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore;
                                                    btnnext.First().SetAttributeValue("onClick", "javascript:nextpage('" + dictpars["controlid"] + "')");
                                                }
                                                var btnprev = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "btnPrevPage");
                                                if ((btnprev != null) && (btnprev.Count() > 0))
                                                {
                                                    btnprev.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pagerindietro").Valore;
                                                    btnprev.First().SetAttributeValue("onClick", "javascript:prevpage('" + dictpars["controlid"] + "')");
                                                } 
                                                ////////////////////////////////////
#endif
                                                var lbltototali = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "spantotals");
                                                if ((lbltototali != null) && (lbltototali.Count() > 0))
                                                    lbltototali.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pagertotale").Valore + " " + totalrecords.ToString() + "<br/>";
                                                var lblactpage = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "divactpage");
                                                if ((lblactpage != null) && (lblactpage.Count() > 0))
                                                    lblactpage.First().InnerHtml = page + "/" + pagesnumber;

                                            }
                                        }
                                    }
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]); //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
                                {

                                    bool alternate = false;
                                    if (elementtoappend.First().Attributes.Contains("class") && elementtoappend.First().Attributes["class"].Value.Contains("alternatecolor"))
                                        alternate = true;

                                    string innerelement = elementtoappend.First().InnerHtml;
                                    if ((innerelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(innerelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        elementtoappend.First().RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        int j = 0;
                                        foreach (Offerte item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection

                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);

                                                }

                                            ////////////////////ODD EVEN GESTIONE SFONDO
                                            if (alternate)
                                            {
                                                if (IsEven(j))
                                                {

                                                    //var nodes = cloneitem.SelectNodes("//*[contains(@class, 'odd')]");
                                                    //if (nodes != null)
                                                    //foreach (HtmlNode n in cloneitem.SelectNodes("*"))
                                                    //{
                                                    //    if (n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("odd"))
                                                    //        n.Remove();
                                                    //}

                                                    var nodestoremove = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("odd"));
                                                    if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                    {
                                                        nodestoremove.First().Remove();
                                                    }
                                                }
                                                else
                                                {
                                                    //var nodes = cloneitem.SelectNodes("//*[contains(@class, 'even')]");
                                                    //if (nodes != null)
                                                    //foreach (HtmlNode n in cloneitem.SelectNodes("*"))
                                                    //foreach (HtmlNode n in cloneitem.Descendants())
                                                    //{
                                                    //    if (n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("even"))
                                                    //        n.Remove();
                                                    //}

                                                    var nodestoremove = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("even"));
                                                    if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                        if ((nodestoremove != null) && (nodestoremove.Count() > 0))
                                                        {
                                                            nodestoremove.First().Remove();
                                                        }
                                                }

                                            }
                                            /////////////////////////////////////////////
                                            elementtoappend.First().AppendChild(cloneitem.Clone());
                                            j++;
                                        }
                                    }
                                    //Rendiamo visibile il primo div contenitore del template
                                    var firstnode = template.DocumentNode.Descendants("div");
                                    if ((firstnode != null) && (firstnode.Count() > 0))
                                    {
                                        if (firstnode != null)
                                            if (firstnode.First().Attributes.Contains("style"))
                                            {
                                                firstnode.First().Attributes["style"].Value = firstnode.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                firstnode.First().Attributes["style"].Value += ";display:block";
                                            }
                                            else
                                                firstnode.First().Attributes.Add("style", "display:block");
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //var nodeconteinerisotope = node.SelectSingleNode("//*[@id='" + dictpars["container"] + "']");
                                    //nodeconteinerisotope.RemoveAllChildren();
                                    //var clonedpager = node.SelectSingleNode("//*[@id='" + dictpars["container"] + "Pager']").Clone(); //Clono il pager per non cancellarlo
                                    HtmlNode clonedpager = null; //Clono il pager per non cancellarlo
                                    var childelems = node.ChildNodes.Descendants().Where(n => n.Id == (dictpars["container"] + "Pager"));
                                    if ((childelems != null) && (childelems.Count() > 0))
                                    {
                                        //Clono il pager per non cancellarlo
                                        clonedpager = childelems.First().Clone();
                                    }
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati
                                    if (clonedpager != null && node.ParentNode.SelectSingleNode("//*[@id='" + dictpars["container"] + "Pager']") == null)
                                        node.AppendChild(clonedpager); //Appendo il pager


                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        //Aggiorno le variaibli javascript globalObject[controlid + "params"] ( da dictpars) e globalObject[controlid + "pagerdata" ] ( da dictpagerpars ) per far funzionare il pager lato client !!!
                                        //La seguente prepara una chiamata a funzione javascript che inizializza le variabili js. ( SERVE SOLO PER UTILIZZO LATO CLIENT )
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container + "-2")) jscommands[Session.SessionID].Remove(container + "-2");
                                        //jscommands[Session.SessionID].Add(container + "-2", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');");

                                        jscommands[Session.SessionID].Add(container + "-2", WelcomeLibrary.UF.Utility.waitwrappercall("initGlobalVarsFromServer", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');"));


                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        //if (jscommands[Session.SessionID].ContainsKey(container + "-1")) jscommands[Session.SessionID].Remove(container + "-1");
                                        //jscommands[Session.SessionID].Add(container + "-1", "InitIsotopeLocal('" + dictpars["controlid"] + "','" + dictpars["container"] + "');");

                                        //if (dictpagerpars["enablepager"] == "true")
                                        //{
                                        //jscommands[Session.SessionID].Add(container + "-2", "initHtmlPager('" + dictpars["controlid"] + "');");
                                        // jscommands[Session.SessionID].Add(container + "-3", "renderPager('" + dictpars["controlid"] + "');"); //QESUTA LA DEVI REPLICARE LATO SERBE USA LE RISORSE!!!!!! o gli devi passare le risporse necessarie!!
                                        //}
                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING //////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectandloadgenericvideo":
                        // injectandloadgenericvideo(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height)

                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3) dictpars.Add("controlid", pars[3]);

                        if (pars.Count > 7) dictpars.Add("listShow", pars[7]);
                        if (pars.Count > 8) dictpars.Add("maxelement", pars[8]);
                        if (pars.Count > 9) dictpars.Add("connectedid", pars[9]);
                        if (pars.Count > 10) dictpars.Add("tblsezione", pars[10]);
                        if (pars.Count > 11) dictpars.Add("filtrosezione", pars[11]);
                        if (pars.Count > 12) dictpars.Add("mescola", pars[12]);
                        if (pars.Count > 13) dictpars.Add("width", pars[13]);
                        if (pars.Count > 14) dictpars.Add("height", pars[14]);
                        ////////////////////////////(PAGINAZIONE on usata ... )
                        if (pars.Count > 4) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6) dictpagerpars.Add("enablepager", pars[6]);

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("tblsezione")) return;
                        if (!dictpars.ContainsKey("filtrosezione")) return;

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE  (bannervideo.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"],Session.SessionID);
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, "1", "1", "false", Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Banners> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Banners>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]); //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
                                {
                                    string repeatelement = elementtoappend.First().OuterHtml;
                                    if ((repeatelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(repeatelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        template.DocumentNode.RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        foreach (Banners item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection nel binding
                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }

                                            template.DocumentNode.AppendChild(cloneitem.Clone());
                                        }
                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                    node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati

                                    if (node != null)
                                        if (node.Attributes.Contains("style"))
                                        {
                                            node.Attributes["style"].Value = node.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container + "-2")) jscommands[Session.SessionID].Remove(container + "-2");

                                        //jscommands[Session.SessionID].Add(container + "-2", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');");

                                        jscommands[Session.SessionID].Add(container + "-2", WelcomeLibrary.UF.Utility.waitwrappercall("initGlobalVarsFromServer", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');"));

                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        if (jscommands[Session.SessionID].ContainsKey(container)) jscommands[Session.SessionID].Remove(container);
                                        //jscommands[Session.SessionID].Add(container, "InitVideo('" + dictpars["controlid"] + "','" + node.Id + "')");
                                        jscommands[Session.SessionID].Add(container, WelcomeLibrary.UF.Utility.waitwrappercall("InitVideo", "InitVideo('" + dictpars["controlid"] + "','" + node.Id + "')"));

                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectandloadgenericbanner":
                        // injectandloadgenericbanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height)

                        //Caricamento parametri per la chiamata
                        if (pars.Count > 0) dictpars.Add("functionname", pars[0]);
                        if (pars.Count > 1) dictpars.Add("templateHtml", pars[1]);
                        if (pars.Count > 2) dictpars.Add("container", pars[2]);
                        if (pars.Count > 3) dictpars.Add("controlid", pars[3]);
                        if (pars.Count > 7) dictpars.Add("listShow", pars[7]);
                        if (pars.Count > 8) dictpars.Add("maxelement", pars[8]);
                        if (pars.Count > 9) dictpars.Add("connectedid", pars[9]);
                        if (pars.Count > 10) dictpars.Add("tblsezione", pars[10]);
                        if (pars.Count > 11) dictpars.Add("filtrosezione", pars[11]);
                        if (pars.Count > 12) dictpars.Add("mescola", pars[12]);
                        if (pars.Count > 13) dictpars.Add("width", pars[13]);
                        if (pars.Count > 14) dictpars.Add("height", pars[14]);
                        ////////////////////////////(PAGINAZIONE on usata ... )
                        if (pars.Count > 4) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6) dictpagerpars.Add("enablepager", pars[6]);

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("tblsezione")) return;
                        if (!dictpars.ContainsKey("filtrosezione")) return;

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE  (bannerimagefull.html)
                        ///////////////////////////////////////////////////////////
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[1]);
                        templatetext = templatetext.Replace("replaceid", dictpars["controlid"]);
                        if (!string.IsNullOrEmpty(templatetext))
                        {
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(templatetext); //Template per il bind
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //CARICAMENTO DATI PER BIND
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, "1", "1", "false", Session.SessionID);
                            if (dictdati != null && dictdati.Count > 0)
                            {
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //LISTE DATI DA VISUALIZZARE
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                List<Banners> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Banners>>(dictdati["data"]);
                                //Caratteristiche della lista ( totalrecords )
                                Dictionary<string, string> resultinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictdati["resultinfo"]);
                                //Collezione chiave,valore per id elemento dei valori preparati per il binding link,titolo,image
                                Dictionary<string, Dictionary<string, string>> linkloaded = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dictdati["linkloaded"]);
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                /////BINDING DATI SU TEMPLATE /////////////////////////////////////////////////////////////////////////////////////////////
                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                var elementtoappend = template.DocumentNode.Descendants().Where(c => c.Id == dictpars["controlid"]); //si presuppone che ci sia un elemento padre del template a cui appendere i singoli elementi bindtati
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
                                {
                                    string repeatelement = elementtoappend.First().OuterHtml;
                                    if ((repeatelement != ""))
                                    {
                                        //Creo una copia  del contenuto da ripetere creandoci un nuovo documento 
                                        HtmlDocument tmpdoc = new HtmlDocument();//Documento temporane per fre il binding ripetuto
                                        tmpdoc.LoadHtml(repeatelement);
                                        HtmlNode cloneitemtemplate = tmpdoc.DocumentNode.Clone(); //elemento root matrice template da ripetere per fare il binding
                                        template.DocumentNode.RemoveAllChildren(); //ATTENZIONE questa rimuove i child del ellemento primario del template
                                        foreach (Banners item in data)
                                        {
                                            //Copio i dati dall'oggetto Banners in un dictionary property,value per evitare l'utilizzo pesante di reflection nel binding!!!!
                                            Dictionary<string, string> itemdic = item.GetDictionaryElements(); //Questa prende le propieta di item e le mappa in un dictionary string,string per evitare la reflection nel binding
                                            HtmlNode cloneitem = cloneitemtemplate.Clone();
                                            //PROCEDURA DI BIND DATI
                                            var bindingnodes = cloneitem.DescendantsAndSelf().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
                                            if ((bindingnodes != null) && (bindingnodes.Count() > 0))
                                                foreach (var nodetobind in bindingnodes) //scorro gli elementi taggati per il binding e crei i blocchi da appendere
                                                {
                                                    DataBindElement(nodetobind, itemdic, linkloaded, resultinfo);
                                                }

                                            template.DocumentNode.AppendChild(cloneitem.Clone());
                                        }
                                    }
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Appendiamo l'html al contenitore corretto dopo il binding!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.RemoveAllChildren();//Per svuotare il contenitore primario in pagina
                                    //node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                    node.AppendChild(template.DocumentNode); //Appendo il blocco bindato ai dati

                                    if (node != null)
                                        if (node.Attributes.Contains("style"))
                                        {
                                            node.Attributes["style"].Value = node.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.Attributes.Add("style", "display:block");
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (node.ParentNode != null)
                                        if (node.ParentNode.Attributes.Contains("style"))
                                        {
                                            node.ParentNode.Attributes["style"].Value = node.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.ParentNode.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.ParentNode.Attributes.Add("style", "display:block");


                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();
                                    CleanHtml(node);//rimuovo gli attributi usati per il bind dagli elementi ( DA ULTIMARE )

                                    if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                    {
                                        if (!jscommands.ContainsKey(Session.SessionID))
                                            jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                        if (jscommands[Session.SessionID].ContainsKey(container + "-2")) jscommands[Session.SessionID].Remove(container + "-2");
                                        //jscommands[Session.SessionID].Add(container + "-2", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');");
                                        jscommands[Session.SessionID].Add(container + "-2", WelcomeLibrary.UF.Utility.waitwrappercall("initGlobalVarsFromServer", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');"));

                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        if (jscommands[Session.SessionID].ContainsKey(container)) jscommands[Session.SessionID].Remove(container);
                                        //jscommands[Session.SessionID].Add(container, "InitGenericBanner('" + dictpars["controlid"] + "','" + node.Id + "')");
                                        jscommands[Session.SessionID].Add(container, WelcomeLibrary.UF.Utility.waitwrappercall("InitGenericBanner", "InitGenericBanner('" + dictpars["controlid"] + "','" + node.Id + "')"));
                                    }

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    // Da implementare SUCCESSIVAMENTE le seguenti funzioni javascript 
                    // injectArchivioAndLoad - InjectCat1livLinks - injcCategorieLinks - VisualizzaSearchControls - 
                    default:
                        break;
                }

        }

        private static void CleanHtml(HtmlNode node)
        {

            foreach (HtmlNode subnode in node.DescendantsAndSelf())
            {
                if (subnode.Attributes.Contains("class"))
                    subnode.Attributes["class"].Value = subnode.Attributes["class"].Value.Replace("bind", "");
                if (subnode.Attributes.Contains("mybind"))
                    subnode.Attributes["mybind"].Remove();
                if (subnode.Attributes.Contains("mybind1"))
                    subnode.Attributes["mybind1"].Remove();
                if (subnode.Attributes.Contains("mybind2"))
                    subnode.Attributes["mybind2"].Remove();
                if (subnode.Attributes.Contains("mybind3"))
                    subnode.Attributes["mybind3"].Remove();
                if (subnode.Attributes.Contains("myvalue"))
                    subnode.Attributes["myvalue"].Remove();
                if (subnode.Attributes.Contains("myvalue1"))
                    subnode.Attributes["myvalue1"].Remove();
                if (subnode.Attributes.Contains("myvalue2"))
                    subnode.Attributes["myvalue2"].Remove();
                if (subnode.Attributes.Contains("myvalue3"))
                    subnode.Attributes["myvalue3"].Remove();
                if (subnode.Attributes.Contains("format"))
                    subnode.Attributes["format"].Remove();
            }


            //el.find('*').removeClass("inject");
            //el.find('*').removeAttr("params");
            //el.find('*').removeAttr("params");
        }
        private static bool IsEven(int value)
        {
            return value % 2 == 0;
        }
        /// <summary>
        /// Equivalente della funzione javascript FillBindControls che effettua la sostituzione dei dati nel template con i dati provenienti dal database
        /// </summary>
        /// <param name="nodetobind"></param>
        /// <param name="item"></param>
        /// <param name="linkloaded"></param>
        /// <param name="resultinfo"></param>
        private void DataBindElement(HtmlNode nodetobind, Dictionary<string, string> itemdic, Dictionary<string, Dictionary<string, string>> linkloaded, Dictionary<string, string> resultinfo)
        {
            try
            {
                WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();
                //Duplicare il sistema di binding di FillBindControls da common.js
                //( modificando nodetobind con htmlagility viene automaticamente modificato l'elemento che poi è visualizzato

                //da fare conversione c# fillbindcontrole e le funzioni di formattazione usate nell'attributo format!   ......
                string property = "";
                if (nodetobind.Attributes.Contains("mybind"))
                    property = nodetobind.Attributes["mybind"].Value;
                if (!string.IsNullOrEmpty(property) && !property.Contains("."))
                {
                    if (nodetobind.Name == "label")
                    {
                        //nodetobind.InnerHtml = (String)(item.GetType().GetProperty(property).GetValue(item, null)); //Usa Reflection.... LENTO!!!
                        if (itemdic.ContainsKey(property))
                            nodetobind.InnerHtml = itemdic[property];
                    }
                    else if (nodetobind.Name == "input" && nodetobind.Attributes.Contains("type") && nodetobind.Attributes["type"].Value == "checkbox")
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            if (nodetobind.Attributes.Contains("checked")) nodetobind.Attributes.Remove("checked");
                            if (itemdic[property] == "true")
                                nodetobind.Attributes.Add("checked", "");
                        }
                        else
                        {
                            if (nodetobind.Attributes.Contains("checked")) nodetobind.Attributes.Remove("checked");
                        }
                    }
                    else if (nodetobind.Name == "input" && ((nodetobind.Attributes.Contains("type") && nodetobind.Attributes["type"].Value == "text") || (nodetobind.Attributes.Contains("type") && nodetobind.Attributes["type"].Value == "email") || !nodetobind.Attributes.Contains("type")))
                    {
                        if (!nodetobind.Attributes.Contains("value")) nodetobind.Attributes.Add("value", "");
                        nodetobind.Attributes["value"].Value = "";
                        if (itemdic.ContainsKey(property))
                        {
                            nodetobind.Attributes["value"].Value = itemdic[property];
                        }
                        if (nodetobind.Attributes.Contains("idbind"))
                            nodetobind.Attributes["idbind"].Value = itemdic[nodetobind.Attributes["idbind"].Value];
                        if (nodetobind.Attributes.Contains("placeholder"))
                            nodetobind.Attributes["placeholder"].Value = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, nodetobind.Attributes["placeholder"].Value).Valore;
                    }
                    else if (nodetobind.Name == "textarea")
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            nodetobind.InnerHtml = itemdic[property];
                        }
                        else
                        { nodetobind.InnerHtml = ""; }
                        if (nodetobind.Attributes.Contains("idbind"))
                            nodetobind.Attributes["idbind"].Value = itemdic[nodetobind.Attributes["idbind"].Value];
                    }
                    else if ((nodetobind.Name == "span" || nodetobind.Name == "div") && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("ratinginfo"))
                    {
                        if (nodetobind.Attributes.Contains("idbind"))
                        {
                            if (!string.IsNullOrEmpty(property))
                            {
                                string introtext = "";
                                if (nodetobind.Attributes.Contains("myvalue"))
                                {
                                    introtext = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, nodetobind.Attributes["myvalue"].Value).Valore;
                                }
                                nodetobind.Attributes["idbind"].Value = itemdic[nodetobind.Attributes["idbind"].Value];
                                if (linkloaded.ContainsKey(nodetobind.Attributes["idbind"].Value) && linkloaded[nodetobind.Attributes["idbind"].Value].ContainsKey(property) && linkloaded[nodetobind.Attributes["idbind"].Value][property] != "")
                                    nodetobind.InnerHtml = introtext + "(" + linkloaded[nodetobind.Attributes["idbind"].Value][property] + ")";
                                nodetobind.Attributes["class"].Value = nodetobind.Attributes["class"].Value.Replace("ratinginfo", "");
                            }
                        }
                    }
                    else if ((nodetobind.Name == "span" || nodetobind.Name == "div") && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("rating"))
                    {
                        if (nodetobind.Attributes.Contains("idbind"))
                        {
                            if (nodetobind.Attributes.Contains("data-default-rating")) nodetobind.Attributes.Remove("data-default-rating");
                            nodetobind.Attributes["idbind"].Value = itemdic[nodetobind.Attributes["idbind"].Value];
                            if (linkloaded.ContainsKey(nodetobind.Attributes["idbind"].Value) && linkloaded[nodetobind.Attributes["idbind"].Value].ContainsKey(property) && linkloaded[nodetobind.Attributes["idbind"].Value][property] != "")
                                nodetobind.Attributes.Add("data-default-rating", linkloaded[nodetobind.Attributes["idbind"].Value][property]);
                            else
                                nodetobind.Attributes["class"].Value = nodetobind.Attributes["class"].Value.Replace("rating", "");
                        }
                    }
                    else if (nodetobind.Name == "button" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("select"))
                    {
                        string idscheda = itemdic[property];
                        if (nodetobind.Attributes.Contains("myvalue"))
                        {
                            if (nodetobind.Attributes.Contains("onclick")) nodetobind.Attributes.Remove("onclick");
                            nodetobind.Attributes.Add("onclick", "javascript:" + nodetobind.Attributes["myvalue"].Value + "('" + idscheda + "');");
                        }
                    }
                    else if (nodetobind.Name == "a" && nodetobind.Attributes["class"].Value.Contains("callbutton"))
                    {
                        string link = "";
                        string testo = "";
                        string bindprophref = "";
                        string bindproptitle = "";

                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                            idscheda = itemdic[property];

                        if (nodetobind.Attributes.Contains("href"))
                            bindprophref = nodetobind.Attributes["href"].Value;

                        if (itemdic.ContainsKey(bindprophref))
                        {
                            link = itemdic[bindprophref];
                            if (!string.IsNullOrEmpty(link))
                            {
                                nodetobind.Attributes["href"].Value = "tel:" + link;
                                nodetobind.InnerHtml += link;
                            }
                            else
                            {
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:inline-block", "");
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:block", "");
                                    nodetobind.Attributes["style"].Value += ";display:none";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:none");
                            }
                        }
                    }
                    else if (nodetobind.Name == "a")
                    {
                        string link = "";
                        string testo = "";
                        string bindprophref = "";
                        string bindproptitle = "";

                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                            idscheda = itemdic[property];

                        if (nodetobind.Attributes.Contains("href"))
                            bindprophref = nodetobind.Attributes["href"].Value;
                        if (nodetobind.Attributes.Contains("title"))
                            bindproptitle = nodetobind.Attributes["title"].Value;

                        if (linkloaded.ContainsKey(idscheda))
                        {
                            if (linkloaded[idscheda].ContainsKey(bindprophref))
                            {
                                link = linkloaded[idscheda][bindprophref];
                                nodetobind.Attributes["href"].Value = link;
                                if (!string.IsNullOrEmpty(link))
                                {
                                    if (nodetobind.Attributes.Contains("style"))
                                    {
                                        nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                        nodetobind.Attributes["style"].Value += ";display:inline-block";
                                    }
                                    else
                                        nodetobind.Attributes.Add("style", "display:inline-block");
                                    //if (nodetobind.Attributes.Contains("rel"))
                                    //    nodetobind.Attributes["rel"].Value = "follow"; //imposto follow per il link
                                }
                                else
                                {
                                    nodetobind.Attributes["href"].Value = "";
                                    if (!nodetobind.Attributes.Contains("rel"))
                                        nodetobind.Attributes.Add("rel", "nofollow");
                                    else
                                        nodetobind.Attributes["rel"].Value = "nofollow";

                                    if (nodetobind.Attributes.Contains("style"))
                                    {
                                        nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:inline-block", "");
                                        nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:block", "");
                                        nodetobind.Attributes["style"].Value += ";display:none";
                                    }
                                    else
                                        nodetobind.Attributes.Add("style", "display:none");
                                }
                            }
                            else
                            {
                                nodetobind.Attributes["href"].Value = "";
                                if (!nodetobind.Attributes.Contains("rel"))
                                    nodetobind.Attributes.Add("rel", "nofollow");
                                else
                                    nodetobind.Attributes["rel"].Value = "nofollow";

                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:block", "");
                                    nodetobind.Attributes["style"].Value += ";display:none";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:none");
                            }


                            if (linkloaded[idscheda].ContainsKey(bindproptitle))
                            {
                                testo = linkloaded[idscheda][bindproptitle];
                                nodetobind.Attributes["title"].Value = html.Convert(testo);
                                //if (nodetobind.Attributes.Contains("style"))
                                //{
                                //    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                //    nodetobind.Attributes["style"].Value += ";display:block";
                                //}
                                //else
                                //    nodetobind.Attributes.Add("style", "display:block");
                            }

                            if (link.ToLower().IndexOf(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) != -1)
                                nodetobind.SetAttributeValue("target", "_self");
                            else
                                nodetobind.SetAttributeValue("target", "_blank");

                        }
                        else
                        {
                            if (nodetobind.Attributes.Contains("style"))
                            {
                                nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:block", "");
                                nodetobind.Attributes["style"].Value += ";display:none";
                            }
                            else
                                nodetobind.Attributes.Add("style", "display:none");
                        }
                    }
                    else if (nodetobind.Name == "img" && nodetobind.Attributes["class"].Value.Contains("imgfromresource"))
                    {
                        //<img src="keyfromresource" class="bind imgfromresource" mybind="Id" attrdest="src"  />
                        string reskey = "";

                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                            idscheda = itemdic[property];
                        string attrbind = "src";
                        if (nodetobind.Attributes.Contains("attrdest"))
                            attrbind = nodetobind.Attributes["attrdest"].Value;

                        if (nodetobind.Attributes.Contains(attrbind))
                            reskey = nodetobind.Attributes[attrbind].Value;
                        string imgsrc = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, reskey).Valore;
                        if (!string.IsNullOrEmpty(imgsrc))
                        {
                            nodetobind.Attributes[attrbind].Value = imgsrc;
                        }
                        else
                        {
                            if (nodetobind.Attributes.Contains("style"))
                            {
                                nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:inline-block", "");
                                nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:block", "");
                                nodetobind.Attributes["style"].Value += ";display:none";
                            }
                            else
                                nodetobind.Attributes.Add("style", "display:none");
                        }
                    }
                    else if (nodetobind.Name == "img" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("revolution"))
                    {
                        string idscheda = "";
                        string pathImg = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("image"))
                                pathImg = linkloaded[idscheda]["image"];
                            if (nodetobind.Attributes.Contains("data-lazyload"))
                            {
                                nodetobind.Attributes["data-lazyload"].Value = pathImg;
                            }
                            else
                                nodetobind.Attributes.Add("data-lazyload", pathImg);
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("imagessequence"))
                    {
                        List<string> imgslist = new List<string>();
                        List<string> imgslistdesc = new List<string>();
                        List<string> imgslistratio = new List<string>();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imageslist") && !string.IsNullOrEmpty(linkloaded[idscheda]["imageslist"]))
                                imgslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imageslist"]);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagesdesc") && !string.IsNullOrEmpty(linkloaded[idscheda]["imagesdesc"]))
                                imgslistdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesdesc"]);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagesratio") && !string.IsNullOrEmpty(linkloaded[idscheda]["imagesratio"]))
                                imgslistratio = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesratio"]);
                            //foreach (string img in imgslist)

                            bool skipfirst = false;
                            if (nodetobind.Attributes.Contains("myvalue"))
                            {
                                string myvalue = nodetobind.Attributes["myvalue"].Value;
                                if (myvalue == "skip") skipfirst = true;
                            }

                            StringBuilder sb = new StringBuilder();
                            string maxheight = "";
                            for (int j = 0; j < imgslist.Count(); j++)
                            {
                                try
                                {
                                    if (skipfirst && j == 0)
                                    {
                                        continue; //salto la prima
                                    }

                                    /*
                                    <div class="w-100" style="width: 100%; text-align: center; margin-top: 10px; margin-bottom: 10px">
                                    <img class="img-responsive mx-auto" alt="" src="" style="margin:0px auto;background-color:#ffffff;padding: 20px">
                                    </div>
                                     */

                                    string img = imgslist[j];

                                    sb.Append("<div class=\"w-100 text-center\" >");

                                    string imgstyle = "max-width:100%;height:auto;";
                                    ////////////////////////////////////////////////////////
                                    //Eventuale impostazione max height elementi
                                    if (nodetobind.Attributes.Contains("style") && nodetobind.Attributes["style"].Value.Contains("max-height"))
                                    {
                                        string inlinestyle = nodetobind.Attributes["style"].Value;
                                        //parse style to find an element
                                        foreach (var entries in inlinestyle.Split(';'))
                                        {
                                            string[] values = entries.Split(':');
                                            if (values != null && values.Count() == 2)
                                            {
                                                if (values[0].ToLower() == "max-height") maxheight = values[1];
                                                //newStyles += values.Join(':') + ";";
                                                nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("max-height:" + values[1], "");
                                            }
                                        }


                                    }
                                    if (maxheight != "")
                                    {
                                        maxheight = maxheight.Replace("px", "");
                                        int calcheight = 0;
                                        if (int.TryParse(maxheight, out calcheight))
                                        {
                                            int actwidth = 0;
                                            if (int.TryParse(Utility.ViewportwManagerGet(Session.SessionID), out actwidth))
                                                if (calcheight > actwidth && actwidth != 0) calcheight = actwidth;
                                            try
                                            {
                                                double ar = 1;
                                                if (double.TryParse(imgslistratio[j], out ar))
                                                    if (ar < 1)
                                                    {
                                                        //imgstyle = "max-width:100%;width:auto;height:" + maxheight + "px;";
                                                        //imgstyle = "width:auto;max-width:100%;height:" + calcheight + "px;";
                                                        imgstyle = "width:auto;height:" + calcheight + "px;";
                                                    }
                                            }
                                            catch
                                            {
                                            };
                                        }
                                    }
                                    ////////////////////////////////////////////////////////

                                    //  <img class="img-responsive mx-auto" alt="" src="" style="margin:0px auto;background-color:#ffffff;padding: 20px">

                                    //  contenutoslide += "<a rel=\"prettyPhoto[pp_gal]\" href=\"" + imgslist[j] + "\">';
                                    sb.Append("<img class=\"img-fluid\"   style=\"background-color:#ffffff;padding: 10px;border:none;" + imgstyle + "\" src=\"");
                                    sb.Append(imgslist[j]);
                                    sb.Append("\" ");

                                    string altdescriptiontext = "";
                                    string descriptiontext = "";
                                    if (imgslist[j].LastIndexOf("/") != -1)
                                        altdescriptiontext = imgslist[j].Substring(imgslist[j].LastIndexOf("/") + 1);
                                    if (imgslistdesc.Count > j && imgslistdesc[j].Trim() != "")
                                    {
                                        altdescriptiontext = imgslistdesc[j];
                                        descriptiontext = imgslistdesc[j];
                                    }
                                    sb.Append(" alt=\"" + altdescriptiontext + "\" />");

                                    if (!string.IsNullOrEmpty(descriptiontext) && nodetobind.Attributes["class"].Value.Contains("showdescription"))
                                        sb.Append("<div class=\"lead img-desc\" >" + descriptiontext + "</div>");



                                    sb.Append("</div>");
                                }
                                catch
                                {
                                }
                            }



                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml = contenutoslide;
                            if (nodetobind != null && !string.IsNullOrEmpty(contenutoslide))
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");

                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("imagesgalllerysimple"))
                    {
                        List<string> imgslist = new List<string>();
                        List<string> imgslistdesc = new List<string>();
                        List<string> imgslistratio = new List<string>();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imageslist") && !string.IsNullOrEmpty(linkloaded[idscheda]["imageslist"]))
                                imgslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imageslist"]);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagesdesc") && !string.IsNullOrEmpty(linkloaded[idscheda]["imagesdesc"]))
                                imgslistdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesdesc"]);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagesratio") && !string.IsNullOrEmpty(linkloaded[idscheda]["imagesratio"]))
                                imgslistratio = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesratio"]);
                            bool skipfirst = false;
                            if (nodetobind.Attributes.Contains("myvalue"))
                            {
                                string myvalue = nodetobind.Attributes["myvalue"].Value;
                                if (myvalue == "skip") skipfirst = true;
                            }
                            string itemclass = ""; //clasee dell'elemento della gallery
                            if (nodetobind.Attributes.Contains("myvalue1"))
                            {
                                itemclass = nodetobind.Attributes["myvalue1"].Value;
                            }

                            bool prettyphoto = false; //clasee dell'elemento della gallery
                            if (nodetobind.Attributes.Contains("myvalue2"))
                            {
                                string myvalue2 = nodetobind.Attributes["myvalue2"].Value;
                                if (myvalue2 == "prettyphoto") prettyphoto = true;
                            }


                            //<div class="grid-sizer"></div>

                            StringBuilder sb = new StringBuilder();
                            string maxheight = "";
                            for (int j = 0; j < imgslist.Count(); j++)
                            {
                                try
                                {
                                    if (skipfirst && j == 0)
                                        continue; //salto la prima
                                    if (itemclass == "grid-item" && j == 0)
                                        sb.Append("<div class=\"grid-sizer\"></div>");

                                    /*
                                    <div class="w-100" style="width: 100%; text-align: center; margin-top: 10px; margin-bottom: 10px">
                                    <img class="img-responsive mx-auto" alt="" src="" style="margin:0px auto;background-color:#ffffff;padding: 20px">
                                    </div>
                                     */

                                    string img = imgslist[j];

                                    sb.Append("<div class=\"text-center " + itemclass + "\" >");
                                    string imgstyle = "max-width:100%;height:auto;";

                                    ////////////////////////////////////////////////////////
                                    //Eventuale impostazione max height elementi
                                    ////////////////////////////////////////////////////////
                                    #region Limitazione altezza massima delle foto in base al viewport
                                    if (nodetobind.Attributes.Contains("style") && nodetobind.Attributes["style"].Value.Contains("max-height"))
                                    {
                                        string inlinestyle = nodetobind.Attributes["style"].Value;
                                        //parse style to find an element
                                        foreach (var entries in inlinestyle.Split(';'))
                                        {
                                            string[] values = entries.Split(':');
                                            if (values != null && values.Count() == 2)
                                            {
                                                if (values[0].ToLower() == "max-height") maxheight = values[1];
                                                nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("max-height:" + values[1], "");
                                            }
                                        }
                                    }
                                    if (maxheight != "")
                                    {
                                        maxheight = maxheight.Replace("px", "");
                                        int calcheight = 0;
                                        if (int.TryParse(maxheight, out calcheight))
                                        {
                                            int actwidth = 0;
                                            if (int.TryParse(Utility.ViewportwManagerGet(Session.SessionID), out actwidth))
                                                if (calcheight > actwidth && actwidth != 0) calcheight = actwidth;
                                            try
                                            {
                                                double ar = 1;
                                                if (double.TryParse(imgslistratio[j], out ar))
                                                    if (ar < 1)
                                                    {
                                                        //imgstyle = "max-width:100%;width:auto;height:" + maxheight + "px;";
                                                        //imgstyle = "width:auto;max-width:100%;height:" + calcheight + "px;";
                                                        imgstyle = "width:auto;height:" + calcheight + "px;";
                                                    }
                                            }
                                            catch
                                            {
                                            };
                                        }
                                    }
                                    #endregion
                                    //////////////////////////////////////////////////////////////////////////////

                                    if (prettyphoto)
                                        sb.Append("<a rel=\"prettyPhoto[pp_gal]\" href=\"" + imgslist[j] + "\">");
                                    sb.Append("<img class=\"img-fluid\"   style=\"border:none;" + imgstyle + "\" src=\"");
                                    sb.Append(imgslist[j]);
                                    sb.Append("\" ");
                                    string altdescriptiontext = "";
                                    string descriptiontext = "";
                                    if (imgslist[j].LastIndexOf("/") != -1)
                                        altdescriptiontext = imgslist[j].Substring(imgslist[j].LastIndexOf("/") + 1);
                                    if (imgslistdesc.Count > j && imgslistdesc[j].Trim() != "")
                                    {
                                        altdescriptiontext = imgslistdesc[j];
                                        descriptiontext = imgslistdesc[j];
                                    }
                                    sb.Append(" alt=\"" + altdescriptiontext + "\" />");
                                    if (prettyphoto)
                                        sb.Append("</a>");

                                    //if (!string.IsNullOrEmpty(descriptiontext) && nodetobind.Attributes["class"].Value.Contains("showdescription"))
                                    //    sb.Append("<div class=\"lead img-desc\" >" + descriptiontext + "</div>");



                                    sb.Append("</div>");
                                }
                                catch
                                {
                                }
                            }



                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml = contenutoslide;
                            //if (nodetobind != null && !string.IsNullOrEmpty(contenutoslide))
                            //    if (nodetobind.Attributes.Contains("style"))
                            //    {
                            //        nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                            //        nodetobind.Attributes["style"].Value += ";display:block";
                            //    }
                            //    else
                            //        nodetobind.Attributes.Add("style", "display:block");

                        }
                    }


                    else if (nodetobind.Name == "li" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("revolution"))
                    {
                        string idscheda = "";
                        string pathImg = "";
                        string link = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("image"))
                                pathImg = linkloaded[idscheda]["image"];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("link"))
                                link = linkloaded[idscheda]["link"];

                            if (nodetobind.Attributes.Contains("data-link"))
                            {
                                nodetobind.Attributes["data-link"].Value = link;
                            }
                            else
                                nodetobind.Attributes.Add("data-link", link);
                            if (nodetobind.Attributes.Contains("data-thumb"))
                            {
                                nodetobind.Attributes["data-thumb"].Value = pathImg;
                            }
                            else
                                nodetobind.Attributes.Add("data-thumb", pathImg);
                        }
                    }
                    else if (nodetobind.Name == "img" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("avatar"))
                    {
                        string completepath = "";
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("avatar"))
                                completepath = linkloaded[idscheda]["avatar"];

                            nodetobind.SetAttributeValue("src", completepath);
                        }
                    }
                    else if (nodetobind.Name == "img")
                    {
                        string completepath = "";
                        string descfoto = "";
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("image"))
                                completepath = linkloaded[idscheda]["image"];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagedesc"))
                                descfoto = linkloaded[idscheda]["imagedesc"];

                            if (nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("img-ant") && completepath.ToLower().LastIndexOf("dummylogo") == -1)
                            {
                                int position = completepath.LastIndexOf('/');
                                var filename = completepath.Substring(position + 1);
                                filename = filename.Replace("-xs.", ".");
                                filename = filename.Replace("-sm.", ".");
                                filename = filename.Replace("-md.", ".");
                                filename = filename.Replace("-lg.", ".");
                                completepath = completepath.Substring(0, position + 1) + "ant" + filename;
                            }
                            if (descfoto != null && descfoto != "")
                            {
                                nodetobind.SetAttributeValue("alt", descfoto);
                            }
                            if (completepath != null && completepath != "")
                            {
                                //completepath += "?vw=" + window.outerWidth; 
                                nodetobind.SetAttributeValue("src", completepath);
                                //if (nodetobind.Attributes.Contains("src"))
                                //    nodetobind.Attributes["src"].Value = completepath;
                                //else
                                //    nodetobind.Attributes.Add("src", completepath);
                            }
                            if (nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("lazy"))
                            {
                                nodetobind.SetAttributeValue("data-src", completepath);

                                //if (nodetobind.Attributes.Contains("data-src"))
                                //    nodetobind.Attributes["data-src"].Value = completepath;
                                //else
                                //    nodetobind.Attributes.Add("data-src", completepath);

                                nodetobind.SetAttributeValue("src", "");
                            }


                        }
                    }


                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("flexmaincontainer"))
                    {
                        List<string> imgslist = new List<string>();
                        List<string> imgslistdesc = new List<string>();
                        List<string> imgslistratio = new List<string>();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imageslist") && !string.IsNullOrEmpty(linkloaded[idscheda]["imageslist"]))
                                imgslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imageslist"]);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagesdesc") && !string.IsNullOrEmpty(linkloaded[idscheda]["imagesdesc"]))
                                imgslistdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesdesc"]);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagesratio") && !string.IsNullOrEmpty(linkloaded[idscheda]["imagesratio"]))
                                imgslistratio = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesratio"]);



                            bool skipfirst = false;
                            if (nodetobind.Attributes.Contains("skip"))
                            {
                                string myvalue = nodetobind.Attributes["skip"].Value;
                                if (myvalue == "true") skipfirst = true;
                            }

                            //foreach (string img in imgslist)
                            StringBuilder sb = new StringBuilder();
                            string maxheight = "";
                            for (int j = 0; j < imgslist.Count(); j++)
                            {
                                try
                                {


                                    if (skipfirst && j == 0)
                                    {
                                        continue; //salto la prima
                                    }


                                    /*<div class="slide" data-thumb="" >
                                   <div class="slide-content" style="position:relative;padding:1px">
                                       <img itemprop="image" style="border:none" src="" alt="" />
                                       <div class="divbuttonstyle" style="position:absolute;left:30px;bottom:30px;padding:10px;text-align:left;color:#ffffff;">
                                           <a style="color:#ffffff" href="" target="" title="">&nbsp</a>
                                       </div>
                                   </div>
                               </div >*/

                                    string img = imgslist[j];

                                    sb.Append("<div class=\"slide\" data-thumb=\"");
                                    sb.Append(img);
                                    sb.Append("\">");
                                    sb.Append("<div class=\"slide-content\"  style=\"position:relative;padding: 1px\">");

                                    int maxheightfromtemplate = 0;
                                    if (nodetobind.Attributes.Contains("max-height"))
                                    {
                                        string myvalue = nodetobind.Attributes["max-height"].Value;
                                        if (int.TryParse(myvalue, out maxheightfromtemplate))
                                            maxheight = maxheightfromtemplate.ToString();
                                    }


                                    string imgstyle = "max-width:100%;height:auto;";
                                    if (nodetobind.Attributes.Contains("style") && nodetobind.Attributes["style"].Value.Contains("max-height"))
                                    {
                                        string inlinestyle = nodetobind.Attributes["style"].Value;
                                        //parse style to find an element
                                        foreach (var entries in inlinestyle.Split(';'))
                                        {
                                            string[] values = entries.Split(':');
                                            if (values != null && values.Count() == 2)
                                            {
                                                if (values[0].ToLower() == "max-height") maxheight = values[1];
                                                //newStyles += values.Join(':') + ";";
                                            }
                                        }
                                    }
                                    if (maxheight != "" && maxheight != "0")
                                    {
                                        maxheight = maxheight.Replace("px", "");
                                        int calcheight = 0;
                                        if (int.TryParse(maxheight, out calcheight))
                                        {
                                            int actwidth = 0;
                                            if (int.TryParse(Utility.ViewportwManagerGet(Session.SessionID), out actwidth))
                                                if (calcheight > actwidth) calcheight = actwidth;
                                            try
                                            {
                                                double ar = 1;
                                                if (double.TryParse(imgslistratio[j], out ar))
                                                    if (ar < 1)
                                                    {
                                                        //imgstyle = "max-width:100%;width:auto;height:" + maxheight + "px;";
                                                        //imgstyle = "width:auto;max-width:100%;height:" + calcheight + "px;";
                                                        imgstyle = "width:auto;height:" + calcheight + "px;";
                                                    }
                                            }
                                            catch
                                            {
                                            };
                                        }
                                    }
                                    //  contenutoslide += "<a rel=\"prettyPhoto[pp_gal]\" href=\"" + imgslist[j] + "\">';
                                    sb.Append("<img class=\"zoommgfy\" itemprop=\"image\"  style=\"border:none;" + imgstyle + "\" src=\"");
                                    sb.Append(imgslist[j]);
                                    sb.Append("\" ");
                                    sb.Append(" data-magnify-src=\"");
                                    sb.Append(imgslist[j]);
                                    sb.Append("\" ");

                                    /*Livello di ingrandimento della lente (è fatto sempre rispetto alla dimensione dell'immagine naturale che qui gli forzo!!!)*/
                                    double ar1 = 1;
                                    if (double.TryParse(imgslistratio[j], out ar1))
                                    {
                                        double imgwidth = 1100;
                                        double imgheight = (double)imgwidth / ar1;
                                        sb.Append(" data-magnify-magnifiedwidth=\"");
                                        sb.Append(Math.Floor(imgwidth).ToString());
                                        sb.Append("\" ");
                                        sb.Append(" data-magnify-magnifiedheight=\"");
                                        sb.Append(Math.Floor(imgheight).ToString());
                                        sb.Append("\" ");
                                    }
                                    /*Livello di ingrandimento della lente*/
                                    string descriptiontext = "";
                                    if (imgslistdesc.Count > j)
                                        descriptiontext = imgslistdesc[j];
                                    sb.Append(" alt=\"" + descriptiontext + "\" />");

                                    sb.Append("</div>");
                                    sb.Append("</div>");
                                }
                                catch
                                {
                                }
                            }
                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml = contenutoslide;
                            if (nodetobind.ParentNode != null && !string.IsNullOrEmpty(contenutoslide))
                                if (nodetobind.ParentNode.Attributes.Contains("style"))
                                {
                                    nodetobind.ParentNode.Attributes["style"].Value = nodetobind.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.ParentNode.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.ParentNode.Attributes.Add("style", "display:block");

                        }
                    }
                    else if (nodetobind.Name == "ul" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("flexnavcontainer"))
                    {
                        List<string> imgslist = new List<string>();
                        List<string> imgslistdesc = new List<string>();
                        //List<string> imgslistratio = new List<string>();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imageslist") && !string.IsNullOrEmpty(linkloaded[idscheda]["imageslist"]))
                                imgslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imageslist"]);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagesdesc") && !string.IsNullOrEmpty(linkloaded[idscheda]["imagesdesc"]))
                                imgslistdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesdesc"]);
                            //if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imagesratio") && !string.IsNullOrEmpty(linkloaded[idscheda]["imagesratio"]))
                            //    imgslistratio = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesratio"]);


                            bool skipfirst = false;
                            if (nodetobind.Attributes.Contains("skip"))
                            {
                                string myvalue = nodetobind.Attributes["skip"].Value;
                                if (myvalue == "true") skipfirst = true;
                            }

                            StringBuilder sb = new StringBuilder();
                            if (imgslist.Count() > 1)
                                for (int j = 0; j < imgslist.Count(); j++)
                                {
                                    try
                                    {
                                        if (skipfirst && j == 0)
                                        {
                                            continue; //salto la prima
                                        }


                                        sb.Append("<li> <img style=\"padding:5px\" src=\"");

                                        int position = imgslist[j].LastIndexOf('/');
                                        string filename = imgslist[j].Substring(position + 1);
                                        filename = filename.Replace("-xs.", ".");
                                        filename = filename.Replace("-sm.", ".");
                                        filename = filename.Replace("-md.", ".");
                                        filename = filename.Replace("-lg.", ".");
                                        string pathanteprima = imgslist[j].Substring(0, position + 1) + "ant" + filename;

                                        sb.Append(pathanteprima);
                                        sb.Append("\" alt=\"\" ");
                                        sb.Append("</li>");
                                    }
                                    catch { }
                                }
                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml = contenutoslide;
                            if (nodetobind.ParentNode != null && !string.IsNullOrEmpty(contenutoslide))
                                if (nodetobind.ParentNode.Attributes.Contains("style"))
                                {
                                    nodetobind.ParentNode.Attributes["style"].Value = nodetobind.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.ParentNode.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.ParentNode.Attributes.Add("style", "display:block");
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("owl-carousel") && nodetobind.Attributes["class"].Value.Contains("img-list"))
                    {
                        List<string> imgslist = new List<string>();
                        string idscheda = "";
                        StringBuilder sb = new StringBuilder();
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("imageslist") && !string.IsNullOrEmpty(linkloaded[idscheda]["imageslist"]))
                                imgslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imageslist"]);
                            if (imgslist.Count() > 1)
                                for (int j = 0; j < imgslist.Count(); j++)
                                {
                                    sb.Append("<div class=\"item\">");
                                    sb.Append("<img  class=\"img-responsive\"  src=\"");
                                    sb.Append(imgslist[j]);
                                    sb.Append("\" />");
                                    sb.Append("</div>");
                                }

                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml = contenutoslide;
                            if (nodetobind.ParentNode != null && !string.IsNullOrEmpty(contenutoslide))
                                if (nodetobind.ParentNode.Attributes.Contains("style"))
                                {
                                    nodetobind.ParentNode.Attributes["style"].Value = nodetobind.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.ParentNode.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.ParentNode.Attributes.Add("style", "display:block");
                        }

                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("files-list"))
                    {
                        List<string> fileslist = new List<string>();
                        List<string> fileslistdesc = new List<string>();
                        List<string> filelink = new List<string>();
                        string idscheda = "";
                        StringBuilder sb = new StringBuilder();
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("fileslist") && !string.IsNullOrEmpty(linkloaded[idscheda]["fileslist"]))
                                fileslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["fileslist"]);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("filesdesc") && !string.IsNullOrEmpty(linkloaded[idscheda]["filesdesc"]))
                                fileslistdesc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["filesdesc"]);
                            if (fileslist.Count() > 0)
                                for (int j = 0; j < fileslist.Count(); j++)
                                {
                                    sb.Append("<a  style=\"margin-right:10px;margin-bottom:10px;min-width:190px\" class=\"divbuttonstyle\" target=\"_blank\" href=\"");
                                    sb.Append(fileslist[j]);
                                    sb.Append("\" ><i class=\"fa fa-search\"></i>");
                                    string descrizione = "";
                                    try
                                    {
                                        descrizione = (fileslistdesc[j]);
                                    }
                                    catch
                                    {
                                    }
                                    //if (string.IsNullOrEmpty(descrizione)) descrizione = (fileslist[j]); //metto il nome in mancanza di descrizione
                                    if (string.IsNullOrEmpty(descrizione)) descrizione = WelcomeLibrary.UF.ResourceManagement.ReadKey("common", Lingua, "testoallegato").Valore;

                                    sb.Append(descrizione);
                                    sb.Append("</a>");
                                }
                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml = contenutoslide;
                            if (nodetobind != null && !string.IsNullOrEmpty(contenutoslide))
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
                        }
                    }
                    else if ((nodetobind.Name == "div" || nodetobind.Name == "section") && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("imgback"))
                    {
                        string completepath = "";
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("image"))
                                completepath = linkloaded[idscheda]["image"];

                            if (nodetobind.Attributes.Contains("class"))
                                if ((nodetobind.Attributes["class"].Value.Contains("img-ant") || (nodetobind.Attributes["class"].Value.Contains("img-noscaling"))) && completepath.ToLower().LastIndexOf("dummylogo") == -1)
                                {
                                    int position = completepath.LastIndexOf('/');
                                    var filename = completepath.Substring(position + 1);
                                    filename = filename.Replace("-xs.", ".");
                                    filename = filename.Replace("-sm.", ".");
                                    filename = filename.Replace("-md.", ".");
                                    filename = filename.Replace("-lg.", ".");
                                    if (!(nodetobind.Attributes["class"].Value.Contains("img-noscaling")))
                                        completepath = completepath.Substring(0, position + 1) + "ant" + filename;
                                }

                            if (nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("lazy"))
                            {
                                if (nodetobind != null && !string.IsNullOrEmpty(completepath))
                                    nodetobind.SetAttributeValue("data-srcbck", completepath);
                            }
                            else
                            {
                                if (nodetobind != null && !string.IsNullOrEmpty(completepath))
                                    if (nodetobind.Attributes.Contains("style"))
                                    {
                                        nodetobind.Attributes["style"].Value += ";background-image:url('" + completepath + "')";
                                    }
                                    else
                                        nodetobind.Attributes.Add("style", "background-image:url('" + completepath + "')");
                            }
                        }

                    }
                    else if ((nodetobind.Name == "div" || nodetobind.Name == "section") && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("bckvideoelement"))
                    {
                        string completepath = "";
                        string idscheda = "";
                        string link = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("image"))
                                completepath = linkloaded[idscheda]["image"];
                            if (nodetobind.Attributes.Contains("style"))
                                nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("urlplaceholder", completepath);
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("link"))
                                link = linkloaded[idscheda]["link"];
                            if (nodetobind.Attributes.Contains("data-property"))
                                nodetobind.Attributes["data-property"].Value = nodetobind.Attributes["data-property"].Value.Replace("videoplaceholder", link);
                        }
                    }

                    else if (nodetobind.Name == "iframe")
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string idelement = itemdic[property];
                            string valore = "";
                            string prop = "";
                            if (nodetobind.Attributes.Contains("myvalue"))
                                prop = (nodetobind.Attributes["myvalue"].Value);
                            if (linkloaded.ContainsKey(idelement) && linkloaded[idelement].ContainsKey(prop))
                                valore = linkloaded[idelement][prop];
                            if (valore != null && valore != "")
                            {
                                if (nodetobind.Attributes.Contains("src"))
                                    nodetobind.Attributes["src"].Value += valore + "?rel=0";//&autoplay=1

                                if (nodetobind != null && nodetobind.ParentNode != null)
                                    if (nodetobind.ParentNode.Attributes.Contains("style"))
                                    {
                                        nodetobind.ParentNode.Attributes["style"].Value = nodetobind.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                        nodetobind.ParentNode.Attributes["style"].Value += ";display:block";
                                    }
                                    else
                                        nodetobind.ParentNode.Attributes.Add("style", "display:block");
                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("bookingtool"))
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string idelement = itemdic[property];
                            if (nodetobind.Attributes.Contains("id") && !string.IsNullOrEmpty(nodetobind.Attributes["id"].Value))
                            {    //bookingtool.initbookingtool(idelement, nodetobind.Attributes["id"]);
                                if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                {

                                    if (!jscommands.ContainsKey(Session.SessionID))
                                        jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                    if (jscommands[Session.SessionID].ContainsKey(nodetobind.Attributes["id"].Value)) jscommands[Session.SessionID].Remove(nodetobind.Attributes["id"].Value);
                                    //jscommands[Session.SessionID].Add(nodetobind.Attributes["id"].Value, "bookingtool.initbookingtool(" + idelement + "," + nodetobind.Attributes["id"].Value + ");");
                                    jscommands[Session.SessionID].Add(nodetobind.Attributes["id"].Value, WelcomeLibrary.UF.Utility.waitwrappercall("bookingtool.initbookingtool", "bookingtool.initbookingtool(" + idelement + ",'" + nodetobind.Attributes["id"].Value + "');"));
                                    //Imposto la chiamata da tornare
                                }

                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("scaglionitoolcrd"))
                    {
                        ScaglioniCollection scaglioni = new ScaglioniCollection();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            StringBuilder sb = new StringBuilder();
                            string idcontrolcarrello = "";
                            if (nodetobind.Attributes.Contains("myvalue"))
                                idcontrolcarrello = nodetobind.Attributes["myvalue"].Value;
                            idscheda = itemdic[property];
                            string proprieta = "";
                            if (nodetobind.Attributes.Contains("mybind1"))
                                proprieta = nodetobind.Attributes["mybind1"].Value;

                            string idscaglioneselected = "";
                            if (nodetobind.Attributes.Contains("subidselected"))
                                idscaglioneselected = nodetobind.Attributes["subidselected"].Value;



                            Dictionary<string, string> idcoordbyidscaglione = new Dictionary<string, string>();
                            List<string> idlistcoordinatori = new List<string>();
                            string coordname = "";
                            //string scaglioniserialized = "";
                            //if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey(proprieta) && !string.IsNullOrEmpty(linkloaded[idscheda][proprieta]))
                            if (itemdic.ContainsKey(proprieta) && !string.IsNullOrEmpty(itemdic[proprieta]))
                            {
                                try
                                {
                                    //Prendo i dati dello scaglione da itemdic[proprieta] deserializzandolo ....
                                    scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(itemdic[proprieta]);
                                    //Alternativa da linkedresource
                                    //scaglioniserialized = linkloaded[idscheda][proprieta];
                                    //scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(scaglioniserialized);;
                                    //ELIMINIAMO DALLA LISTA VISUALIZZATA GLI SCAGLIONI PASSATI che non servono nella scheda con data inferiore a oggi
                                    if (scaglioni != null)
                                    {
                                        scaglioni.RemoveAll(s => s.datapartenza < System.DateTime.Now);

#if false //cevvhia versione che caricava il nome in base a id coordinatore
                                        string coordinatore = "";
                                        offerteDM offDM = new offerteDM();
                                        foreach (Scaglioni el in scaglioni)
                                        {
                                            Offerte cordinatore = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, el.idcoordinatore.ToString());
                                            if (cordinatore != null && cordinatore.Id != 0)
                                                coordinatore = offDM.estraititolo(cordinatore, Lingua);
                                            break;//prendo il primo che trovo
                                        }
                                        coordname = coordinatore; 
#endif
                                        string idcoordinatoreselected = "";
                                        foreach (Scaglioni el in scaglioni)
                                        {
                                            ////////////COORDINATORI
                                            if (!idcoordbyidscaglione.ContainsKey(el.id.ToString().ToString()))
                                                idcoordbyidscaglione.Add(el.id.ToString(), el.idcoordinatore.ToString());
                                            if (!idlistcoordinatori.Contains(el.idcoordinatore.ToString()))
                                                idlistcoordinatori.Add(el.idcoordinatore.ToString());
                                            //////////////////////////////////////////////
                                            if (el.id.ToString() == idscaglioneselected) idcoordinatoreselected = el.idcoordinatore.ToString();
                                            ///
                                        }

                                        //Creaiamo i link alle schede coordinatori ( a partire dall'id coordinatore devo caricare i dati del coordinatore )
                                        //scorro dictionary idcoordbyidscaglione dove per ogni idscglione ho idcoordinatore
                                        // e nel valore sostituisco l'id coordinatore con un dictonary key,value con icon,name,link )
                                        Dictionary<string, string> coordetails = new Dictionary<string, string>();
                                        Dictionary<string, Dictionary<string, string>> coordadatabyidscaglione = new Dictionary<string, Dictionary<string, string>>();
                                        offerteDM offDM = new offerteDM();
                                        string sessionid = "";
                                        if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                            sessionid = Session.SessionID;
                                        string stringidlistcoordinatori = "";
                                        idlistcoordinatori.ForEach(co => stringidlistcoordinatori += co + ",");
                                        stringidlistcoordinatori = stringidlistcoordinatori.TrimEnd(',');
                                        Dictionary<string, string> coordinfos = offerteDM.getlinklist(Lingua, stringidlistcoordinatori, sessionid);//passando la lista idcoordinatori me li tira su tutti senza caricarli ad ogni giro

                                        //GENERAZIONE DIRETTA HTML PER I COORDINATORI
                                        //puoi aggiungere direttamente qui con lo stringbuider i controllo da mettere in pagina che sono i link alle schede coordinatori con iconcina tonda 
                                        //<a class=\"link-coord\" href=\"\"><img class=\"img-small-rounded\" src=\"\"></a>

                                        sb.Append("<ul class=\"coordliststyle\">");
                                        foreach (string idcoordinatore in idlistcoordinatori)
                                        {
#if false
                                            //html con img contenuta
                                            sb.Append("<a class=\"link-coord\" href=\"");
                                            sb.Append(coordinfos[idcoordinatore]);
                                            sb.Append("\"><img  class=\"coordid-" + idcoordinatore + " img-small-rounded\" src=\"");
                                            sb.Append(coordinfos[idcoordinatore + "img"]);
                                            sb.Append("\"></a>");

#endif
                                            sb.Append("<li>");
                                            //sb.Append("<li style=\"float:left;\">");
                                            sb.Append("<a target=\"_blank\" data-toggle=\"tooltip\" title=\"" + coordinfos[idcoordinatore + "name"] + "\" class=\"link-coord\" href=\"");
                                            sb.Append(coordinfos[idcoordinatore]);
                                            sb.Append("\"><div  class=\"coordid-" + idcoordinatore + " container-small-rounded\" ");
                                            sb.Append(" style=\"background-image:url('" + coordinfos[idcoordinatore + "img"] + "')");
                                            sb.Append("\"></div></a>");
                                            sb.Append("<li>");
                                            //evidenzia il coordinatore selezionato e spenge gli altri
                                            if (idcoordinatoreselected == idcoordinatore) sb.Append("<style>.coordid-" + idcoordinatore + "{ border-color:#e18d0c;  } [class*='coordid-']:not(.coordid-" + idcoordinatore + ") {  display:none; } </style>");

                                        }
                                        sb.Append("</ul>");

                                    }
                                }
                                catch { }
                            }

                            //sb.Append(coordname);  //questa mette solo il nome ...
                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml += contenutoslide;
                            //Visualizzazione elemento
                            if (nodetobind != null && scaglioni != null && scaglioni.Count > 0)
                            {
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("scaglionitooleta"))
                    {
                        ScaglioniCollection scaglioni = new ScaglioniCollection();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            StringBuilder sb = new StringBuilder();
                            string idcontrolcarrello = "";
                            if (nodetobind.Attributes.Contains("myvalue"))
                                idcontrolcarrello = nodetobind.Attributes["myvalue"].Value;
                            idscheda = itemdic[property];
                            string proprieta = "";
                            if (nodetobind.Attributes.Contains("mybind1"))
                                proprieta = nodetobind.Attributes["mybind1"].Value;
                            //string scaglioniserialized = "";
                            //if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey(proprieta) && !string.IsNullOrEmpty(linkloaded[idscheda][proprieta]))
                            if (itemdic.ContainsKey(proprieta) && !string.IsNullOrEmpty(itemdic[proprieta]))
                            {
                                try
                                {
                                    //Prendo i dati dello scaglione da itemdic[proprieta] deserializzandolo ....
                                    scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(itemdic[proprieta]);


                                    //Alternativa da linkedresource
                                    //scaglioniserialized = linkloaded[idscheda][proprieta];
                                    //scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(scaglioniserialized);;
                                    //ELIMINIAMO DALLA LISTA VISUALIZZATA GLI SCAGLIONI PASSATI che non servono nella scheda con data inferiore a oggi
                                    if (scaglioni != null)
                                    {
                                        scaglioni.RemoveAll(s => s.datapartenza < System.DateTime.Now);
                                        //scaglioniserialized = Newtonsoft.Json.JsonConvert.SerializeObject(scaglioni);
                                        string etastring = "";
                                        string serializeetalist = references.ResMan("Common", Lingua, "etalist");
                                        if (!string.IsNullOrEmpty(serializeetalist))
                                        {
                                            OrderedDictionary etalist = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderedDictionary>(serializeetalist);
                                            if (etalist != null)
                                                foreach (Scaglioni el in scaglioni)
                                                {
                                                    long etaval = el.fasciaeta.Value;
                                                    etastring = etalist[etaval.ToString()].ToString();
                                                    break;//prendo il primo che trovo
                                                }
                                            sb.Append(etastring);
                                        }
                                    }
                                }
                                catch { }
                            }
                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml += contenutoslide;
                            //Visualizzazione elemento
                            if (nodetobind != null && scaglioni != null && scaglioni.Count > 0)
                            {
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:initial";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:initial");
                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("scaglionitoolminprice"))
                    {
                        ScaglioniCollection scaglioni = new ScaglioniCollection();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            StringBuilder sb = new StringBuilder();
                            string idcontrolcarrello = "";
                            if (nodetobind.Attributes.Contains("myvalue"))
                                idcontrolcarrello = nodetobind.Attributes["myvalue"].Value;
                            idscheda = itemdic[property];
                            string proprieta = "";
                            if (nodetobind.Attributes.Contains("mybind1"))
                                proprieta = nodetobind.Attributes["mybind1"].Value;
                            string scaglioniserialized = "";
                            //if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey(proprieta) && !string.IsNullOrEmpty(linkloaded[idscheda][proprieta]))
                            if (itemdic.ContainsKey(proprieta) && !string.IsNullOrEmpty(itemdic[proprieta]))
                            {
                                try
                                {
                                    //Prendo i dati dello scaglione da itemdic[proprieta] deserializzandolo ....
                                    scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(itemdic[proprieta]);

                                    //Alternativa da linkedresource
                                    //scaglioniserialized = linkloaded[idscheda][proprieta];
                                    //scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(scaglioniserialized);;
                                    //ELIMINIAMO DALLA LISTA VISUALIZZATA GLI SCAGLIONI PASSATI che non servono nella scheda con data inferiore a oggi
                                    if (scaglioni != null)
                                    {
                                        scaglioni.RemoveAll(s => s.datapartenza < System.DateTime.Now);
                                        scaglioniserialized = Newtonsoft.Json.JsonConvert.SerializeObject(scaglioni);
                                    }

                                    double pricemin = 9999999;
                                    foreach (Scaglioni el in scaglioni)
                                    {
                                        //cerco il prezzo minimo ....
                                        if (el.prezzo < pricemin) pricemin = el.prezzo;
                                    }
                                    string unit = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "valuta").Valore;
                                    sb.Append(String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:##,###.00}", new object[] { pricemin }) + ' ' + unit);

                                }
                                catch { }
                            }

                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml += contenutoslide;
                            //Visualizzazione elemento
                            if (nodetobind != null && scaglioni != null && scaglioni.Count > 0)
                            {
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("scaglionitoolmindurata"))
                    {
                        ScaglioniCollection scaglioni = new ScaglioniCollection();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            StringBuilder sb = new StringBuilder();
                            string idcontrolcarrello = "";
                            if (nodetobind.Attributes.Contains("myvalue"))
                                idcontrolcarrello = nodetobind.Attributes["myvalue"].Value;
                            idscheda = itemdic[property];
                            string proprieta = "";
                            if (nodetobind.Attributes.Contains("mybind1"))
                                proprieta = nodetobind.Attributes["mybind1"].Value;
                            string scaglioniserialized = "";
                            //if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey(proprieta) && !string.IsNullOrEmpty(linkloaded[idscheda][proprieta]))
                            if (itemdic.ContainsKey(proprieta) && !string.IsNullOrEmpty(itemdic[proprieta]))
                            {
                                try
                                {
                                    //Prendo i dati dello scaglione da itemdic[proprieta] deserializzandolo ....
                                    scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(itemdic[proprieta]);

                                    //Alternativa da linkedresource
                                    //scaglioniserialized = linkloaded[idscheda][proprieta];
                                    //scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(scaglioniserialized);;
                                    //ELIMINIAMO DALLA LISTA VISUALIZZATA GLI SCAGLIONI PASSATI che non servono nella scheda con data inferiore a oggi
                                    if (scaglioni != null)
                                    {
                                        scaglioni.RemoveAll(s => s.datapartenza < System.DateTime.Now);
                                        scaglioniserialized = Newtonsoft.Json.JsonConvert.SerializeObject(scaglioni);
                                    }

                                    long duratamin = 9999999;
                                    foreach (Scaglioni el in scaglioni)
                                    {
                                        //cerco il prezzo minimo ....
                                        if (el.durata < duratamin) duratamin = el.durata;
                                    }
                                    sb.Append(String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:##}", new object[] { duratamin }));
                                }
                                catch { }
                            }


                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml = contenutoslide + nodetobind.InnerHtml;
                            //Visualizzazione elemento
                            if (nodetobind != null && scaglioni != null && scaglioni.Count > 0)
                            {
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("scaglionitool"))
                    {
                        ScaglioniCollection scaglioni = new ScaglioniCollection();
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            StringBuilder sb = new StringBuilder();
                            string idcontrolcarrello = "";
                            if (nodetobind.Attributes.Contains("myvalue"))
                                idcontrolcarrello = nodetobind.Attributes["myvalue"].Value;

                            idscheda = itemdic[property];
                            string proprieta = "";
                            if (nodetobind.Attributes.Contains("mybind1"))
                                proprieta = nodetobind.Attributes["mybind1"].Value;


                            string idscaglioneselected = "";
                            if (nodetobind.Attributes.Contains("subidselected"))
                                idscaglioneselected = nodetobind.Attributes["subidselected"].Value;

                            string scaglioniserialized = "";
                            //if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey(proprieta) && !string.IsNullOrEmpty(linkloaded[idscheda][proprieta]))
                            if (itemdic.ContainsKey(proprieta) && !string.IsNullOrEmpty(itemdic[proprieta]))
                            {
                                try
                                {
                                    //Prendo i dati dello scaglione da itemdic[proprieta] deserializzandolo ....
                                    scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(itemdic[proprieta]);

                                    //Alternativa da linkedresource
                                    //scaglioniserialized = linkloaded[idscheda][proprieta];
                                    //scaglioni = Newtonsoft.Json.JsonConvert.DeserializeObject<ScaglioniCollection>(scaglioniserialized);
                                    //ELIMINIAMO DALLA LISTA VISUALIZZATA GLI SCAGLIONI PASSATI che non servono nella scheda con data inferiore a oggi
                                    if (scaglioni != null)
                                    {
                                        scaglioni.RemoveAll(s => s.datapartenza < System.DateTime.Now);
                                        scaglioniserialized = Newtonsoft.Json.JsonConvert.SerializeObject(scaglioni);
                                    }
                                }
                                catch { }
                            }
                            /////////////////////////////////////////////////////////////////////////////////////
                            //Metto i valori degli scaglioni su controlli hidden in pagina per la visualizzazione
                            /////////////////////////////////////////////////////////////////////////////////////
                            string scaglioniserializedencoded = dataManagement.EncodeUtfToBase64(scaglioniserialized);
                            //memorizzo i dati di tutti gli scaglioni caricati per la gestione lato client dei valori di selezione
                            sb.Append("<input id=\"" + idcontrolcarrello + "hiddenscaglioni\" type=\"hidden\" value=\"" + scaglioniserializedencoded + "\" />");
                            string serializestatuslist = references.ResMan("Common", Lingua, "statuslist");
                            string serializestatuslistencoded = dataManagement.EncodeUtfToBase64(serializestatuslist);
                            sb.Append("<input id=\"" + idcontrolcarrello + "hiddenstatus\" type=\"hidden\" value=\"" + serializestatuslistencoded + "\" />");
                            string serializeetalist = references.ResMan("Common", Lingua, "etalist");
                            string serializeetalistencoded = dataManagement.EncodeUtfToBase64(serializeetalist);
                            sb.Append("<input id=\"" + idcontrolcarrello + "hiddenetalist\" type=\"hidden\" value=\"" + serializeetalistencoded + "\" />");

                            string mustvalidate = "";
                            if (nodetobind.Attributes.Contains("needed"))
                                mustvalidate = nodetobind.Attributes["needed"].Value;
                            string controltype = "";
                            if (nodetobind.Attributes.Contains("controltype"))
                                controltype = nodetobind.Attributes["controltype"].Value;
                            string controlopt1 = "";
                            if (nodetobind.Attributes.Contains("controloption1"))
                                controlopt1 = nodetobind.Attributes["controloption1"].Value;

                            sb.Append(WelcomeLibrary.UF.ResourceManagement.ReadKey("common", Lingua, "txtintroscaglione").Valore);


                            //COORDINATORI
                            Dictionary<string, string> idcoordbyidscaglione = new Dictionary<string, string>();
                            List<string> idlistcoordinatori = new List<string>();
                            if (scaglioni != null)
                                foreach (Scaglioni el in scaglioni)
                                {
                                    ////////////COORDINATORI
                                    if (!idcoordbyidscaglione.ContainsKey(el.id.ToString().ToString()))
                                        idcoordbyidscaglione.Add(el.id.ToString(), el.idcoordinatore.ToString());
                                    if (!idlistcoordinatori.Contains(el.idcoordinatore.ToString()))
                                        idlistcoordinatori.Add(el.idcoordinatore.ToString());
                                    //////////////////////////////////////////////
                                }
#if true
                            ///////////////////////////////////////////////////////////////
                            ////////////////GESTIONE COORDINATORI /////////////////////
                            ///////////////////////////////////////////////////////////////
                            /////////////METTO IN PAGINA LE INFO PER I COORDINATORI ////////////////
                            //Creaiamo i link alle schede coordinatori ( a partire dall'id coordinatore devo caricare i dati del coordinatore )
                            //scorro dictionary idcoordbyidscaglione dove per ogni idscglione ho idcoordinatore
                            // e nel valore sostituisco l'id coordinatore con un dictonary key,value con icon,name,link )
                            Dictionary<string, string> coordetails = new Dictionary<string, string>();
                            Dictionary<string, Dictionary<string, string>> coordadatabyidscaglione = new Dictionary<string, Dictionary<string, string>>();
                            offerteDM offDM = new offerteDM();
                            string sessionid = "";
                            if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                sessionid = Session.SessionID;
                            string stringidlistcoordinatori = "";
                            idlistcoordinatori.ForEach(co => stringidlistcoordinatori += co + ",");
                            stringidlistcoordinatori = stringidlistcoordinatori.TrimEnd(',');
                            Dictionary<string, string> coordinfos = offerteDM.getlinklist(Lingua, stringidlistcoordinatori, sessionid);//passando la lista idcoordinatori me li tira su tutti senza caricarli ad ogni giro

                            foreach (KeyValuePair<string, string> kv in idcoordbyidscaglione)
                            {
                                coordetails = new Dictionary<string, string>();
                                if (coordinfos.ContainsKey(kv.Value))
                                {
                                    coordetails.Add("id", kv.Value); //id del coordinatore
                                    coordetails.Add("icon", coordinfos[kv.Value + "img"]);
                                    coordetails.Add("name", coordinfos[kv.Value + "name"]);
                                    coordetails.Add("link", coordinfos[kv.Value]);
                                    if (coordadatabyidscaglione.ContainsKey(kv.Key))
                                        coordadatabyidscaglione.Add(kv.Key, coordetails); //la key è l'id dello scaglione
                                    else
                                        coordadatabyidscaglione[kv.Key] = (coordetails);//aggiorno ma improbabile
                                }
                            }
                            string serializecoordlist = Newtonsoft.Json.JsonConvert.SerializeObject(coordadatabyidscaglione); ; //  oggetto serializzatto da caricare ( oggetto con idsscaglione, (icona, nome, link scheda per ogni coordinatore )  )
                            string serializecoordlistencoded = dataManagement.EncodeUtfToBase64(serializecoordlist);
                            sb.Append("<input id=\"" + idcontrolcarrello + "hiddencoordlist\" type=\"hidden\" value=\"" + serializecoordlistencoded + "\" />");
                            ///////////////////////////////////////////////////////////////

#endif


                            //Renderizziamo la lista degli scaglioni con l'elemento che viene specificato in controltype, dropdown, lista o altro 
                            //in base al valore dell'attributo controltype 
                            //creazione controllo select
                            if (controltype == "select")
                            {
                                sb.Append("<select id=\"" + idcontrolcarrello + "dllscaglione\" needed=\"" + mustvalidate + "\" class=\"mx-auto form-control w-100 bg-white\"  >");
                                //aggiungiamo l'elemento vuoto
                                sb.Append("<option value=\"\" >");
                                sb.Append(WelcomeLibrary.UF.ResourceManagement.ReadKey("common", Lingua, "txtselectscaglione").Valore); // da prendere dalle risorse txtselectscaglione
                                sb.Append("</option>");

                                if (scaglioni != null)
                                    foreach (Scaglioni el in scaglioni)
                                    {
                                        //STATO VIAGGIO DISABILITATO SE STATO MAGGIORE DI COMPLETO //////////////////////////////
                                        string disabled = "";
                                        string stato = "";
                                        if (el.stato != null)
                                        {
                                            if (el.stato.Value >= 4) disabled = "disabled";
                                            OrderedDictionary statuslist = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderedDictionary>(serializestatuslist);
                                            if (statuslist != null)
                                                stato = (string)statuslist[el.stato.Value.ToString()];
                                        }
                                        //////////////////////////////////////////////

                                        /////FASCIA DI ETA///////////
                                        string fasciaeta = "";
                                        if (el.fasciaeta != null)
                                        {
                                            OrderedDictionary etalist = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderedDictionary>(serializeetalist);
                                            if (etalist != null)
                                                fasciaeta = (string)etalist[el.fasciaeta.Value.ToString()];
                                        }
                                        //////////////////////////////////////////////
                                        //////////////COORDINATORI////////////////////
                                        //if (!idcoordbyidscaglione.ContainsKey(el.id.ToString().ToString()))
                                        //    idcoordbyidscaglione.Add(el.id.ToString(), el.idcoordinatore.ToString());
                                        //if (!idlistcoordinatori.Contains(el.idcoordinatore.ToString()))
                                        //    idlistcoordinatori.Add(el.idcoordinatore.ToString());
                                        ////////////////////////////////////////////////
                                        string nomecoordinatore = "";
                                        if (coordadatabyidscaglione.ContainsKey(el.id.ToString()) && coordadatabyidscaglione[el.id.ToString()] != null && coordadatabyidscaglione[el.id.ToString()].ContainsKey("name"))
                                        {
                                            nomecoordinatore = coordadatabyidscaglione[el.id.ToString()]["name"];
                                        }
                                        string datapartenza = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:dd MMM yyyy}", new object[] { el.datapartenza });
                                        string dataritorno = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:dd MMM yyyy}", new object[] { el.datapartenza.Value.AddDays(el.durata - 1) });

                                        string selected = "";
                                        if (idscaglioneselected == el.id.ToString()) selected = "selected";

                                        sb.Append("<option " + disabled + " value=\"" + el.id + "\" " + selected + " >");
                                        sb.Append(datapartenza + "/" + dataritorno + " - " + el.prezzo + "€" + " - " + stato + " (" + nomecoordinatore + ")");
                                        sb.Append("</option>");
                                    }
                                sb.Append("</select>");

                            }
                            //Controllo solo lista di testo
                            if (controltype == "")
                            {
                                if (scaglioni != null)
                                    foreach (Scaglioni el in scaglioni)
                                    {
                                        sb.Append(String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:dd MMM yyyy}", new object[] { el.datapartenza }));
                                        sb.Append("  - ");
                                        sb.Append(el.durata);
                                        sb.Append(" gg ");
                                        sb.Append(" ");
                                        sb.Append(el.prezzo);
                                        sb.Append(" € ");
                                        sb.Append("<br/>");
                                        //////////////COORDINATORI
                                        //if (!idcoordbyidscaglione.ContainsKey(el.id.ToString().ToString()))
                                        //    idcoordbyidscaglione.Add(el.id.ToString(), el.idcoordinatore.ToString());
                                        //if (!idlistcoordinatori.Contains(el.idcoordinatore.ToString()))
                                        //    idlistcoordinatori.Add(el.idcoordinatore.ToString());
                                        ////////////////////////////////////////////////

                                    }

                            }

                            //per la selezione assicurazioni
                            if (controlopt1 == "true")
                            {
                                sb.Append("<span id=\"" + idcontrolcarrello + "option1group\" style=\"display:none\">");
                                sb.Append(WelcomeLibrary.UF.ResourceManagement.ReadKey("common", Lingua, "txtintrooption1").Valore);
                                sb.Append("<span id=\"" + idcontrolcarrello + "option1infos\"></span>");
                                sb.Append("<select id=\"" + idcontrolcarrello + "ddlassicurazioni\" needed=\"" + mustvalidate + "\" class=\"mx-auto form-control w-100 bg-white\"  >");
                                //aggiungiamo l'elemento vuoto
                                sb.Append("<option value=\"0\" >");
                                sb.Append(WelcomeLibrary.UF.ResourceManagement.ReadKey("common", Lingua, "txtselectoption1").Valore); // da prendere dalle risorse txtselectscaglione
                                sb.Append("</option>");
                                int a = 1;
                                for (a = 0; a <= 10; a++)
                                {
                                    sb.Append("<option value=\"" + a + "\" >");
                                    sb.Append(a); // da prendere dalle risorse txtselectscaglione
                                    sb.Append("</option>");
                                }
                                sb.Append("</span>");
                            }


                            string contenutoslide = sb.ToString();
                            nodetobind.InnerHtml = contenutoslide;

                            //Visualizzazione elemento
                            if (nodetobind != null && scaglioni != null && scaglioni.Count > 0)
                            {
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
                            }

                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("carellotool"))
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string idelement = itemdic[property];

                            ///////////////////////////////
                            // controllo verifica prezzo diverso zero per spengimento totale carrello
                            //Il controllo vienee fatto solo se presente myvalue su carrello tool
                            ///////////////////////////////
                            string prop = "";
                            if (nodetobind.Attributes.Contains("myvalue"))
                            {
                                prop = (nodetobind.Attributes["myvalue"].Value);
                                string prezzo = itemdic[prop];
                                double p = 0;
                                double.TryParse(prezzo, out p);
                                if ((prezzo == null || prezzo == "" || p == 0))
                                {
                                    if (nodetobind.Attributes.Contains("style"))
                                    {
                                        nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:inline-block", "");
                                        nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:block", "");
                                        nodetobind.Attributes["style"].Value += ";display:none";
                                    }
                                    else
                                        nodetobind.Attributes.Add("style", "display:none");
                                    return;
                                }
                            }
                            ///////////////////////////////

                            if (nodetobind.Attributes.Contains("id") && !string.IsNullOrEmpty(nodetobind.Attributes["id"].Value))
                            {
                                string idcontenitorecarrello = nodetobind.Attributes["id"].Value;

                                if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                {   //idcontenitorecarrello += "-" + idelement; //commetare per funzionamento standard
                                    //nodetobind.Attributes["id"].Value = idcontenitorecarrello;//commetare per funzionamento standard
                                    if (!jscommands.ContainsKey(Session.SessionID))
                                        jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                    if (jscommands[Session.SessionID].ContainsKey(idcontenitorecarrello)) jscommands[Session.SessionID].Remove(idcontenitorecarrello);
                                    jscommands[Session.SessionID].Add(idcontenitorecarrello, WelcomeLibrary.UF.Utility.waitwrappercall("carrellotool.initcarrellotool", "carrellotool.initcarrellotool(" + idelement + ",'','" + Username + "','" + idcontenitorecarrello + "', 2);"));
                                    //jscommands[Session.SessionID].Add(idcontenitorecarrello, "carrellotool.initcarrellotool(" + idelement + ",'','" + Username + "','" + idcontenitorecarrello + "', 2);");  
                                    //1 carrello con data range //2 carreelo standard //3 entrambi


                                }

                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("controlscarrello"))
                    {
                        /*
                         * <div class=\"button-carrello trigcarrellosub\" style=\"padding-left: 2px !important;font-size: 2rem;\" >-</div>
                         * <input style=\"width:40px;margin-top:10px;text-align:center\" class=\"form-control\"  value=\"0\">
                         * <div class="button-carrello trigcarrelloadd" style="padding-left: 2px !important;font-size: 2rem;" >+</div>
                         */
                        if (itemdic.ContainsKey(property))
                        {
                            StringBuilder sb = new StringBuilder();
                            string idelement = itemdic[property];

                            sb.Append("<div><div class=\"button-carrello trigcarrellosub\" style=\"padding-left: 2px !important;font-size: 2rem;\"  ");
                            sb.Append(" idbind=\"" + idelement + "\" ");
                            sb.Append(" >-</div></div>");
                            sb.Append(" <div class=\"px-1\"><input style=\"width:40px;margin-top:10px;text-align:center\" class=\"form-control\"  value=\"0\" ");
                            sb.Append(" id=\"qty-" + idelement + "\" ");
                            sb.Append("< /></div>");
                            sb.Append("<div><div class=\"button-carrello trigcarrelloadd\" style=\"padding-left: 2px !important;font-size: 2rem;\"  ");
                            sb.Append(" idbind=\"" + idelement + "\" ");
                            sb.Append(" >+</div></div>");

                            //nodetobind.Attributes.Add("data-lazyload", pathImg);

                            //if (nodetobind.Attributes.Contains("id") && !string.IsNullOrEmpty(nodetobind.Attributes["id"].Value))
                            //{
                            //    string idcontenitorecarrello = nodetobind.Attributes["id"].Value;
                            //    if (jscommands.ContainsKey(idcontenitorecarrello)) jscommands.Remove(idcontenitorecarrello);
                            //    jscommands.Add(idcontenitorecarrello, "carrellotool.initcarrellotool(" + idelement + ",'','" + Username + "','" + idcontenitorecarrello + "', 2);");  //1 carrello con data range //2 carreelo standard //3 entrambi
                            //}

                            string controlliaggiunti = sb.ToString();
                            nodetobind.InnerHtml = controlliaggiunti;

                        }
                    }



                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("commenttool"))
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string idelement = itemdic[property];
                            if (nodetobind.Attributes.Contains("id") && !string.IsNullOrEmpty(nodetobind.Attributes["id"].Value))
                            {
                                string onlytotals = "false";
                                if (nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("onlytotals"))
                                    onlytotals = "true";

                                //Considero la possibilita che se passato un nome di istanza uso quello per la chiamata a funzione!! ( ATTENZIONE , va definita la variabile nel file feedbacks.js )
                                var instancename = "commenttool";//istanza di default
                                if (nodetobind.Attributes.Contains("instance") && !string.IsNullOrEmpty(nodetobind.Attributes["instance"].Value))
                                    instancename = nodetobind.Attributes["instance"].Value;

                                string viewmode = "0";
                                if (nodetobind.Attributes.Contains("viewmode") && !string.IsNullOrEmpty(nodetobind.Attributes["viewmode"].Value))
                                    viewmode = nodetobind.Attributes["viewmode"].Value;

                                string maxrecord = "";
                                if (nodetobind.Attributes.Contains("maxrecord") && !string.IsNullOrEmpty(nodetobind.Attributes["maxrecord"].Value))
                                    maxrecord = nodetobind.Attributes["maxrecord"].Value;

                                if (Session != null && !string.IsNullOrEmpty(Session.SessionID))
                                {
                                    if (!jscommands.ContainsKey(Session.SessionID))
                                        jscommands.Add(Session.SessionID, new Dictionary<string, string>());
                                    if (jscommands[Session.SessionID].ContainsKey(nodetobind.Attributes["id"].Value + "commenttool")) jscommands[Session.SessionID].Remove(nodetobind.Attributes["id"].Value + "commenttool");
                                    jscommands[Session.SessionID].Add(nodetobind.Attributes["id"].Value + "commenttool", WelcomeLibrary.UF.Utility.waitwrappercall(instancename + ".rendercommentsloadref", instancename + ".rendercommentsloadref(" + idelement + ",'" + nodetobind.Attributes["id"].Value + "','','true','1','35','" + maxrecord + "'," + onlytotals + "," + viewmode + ");"));

                                    //jscommands[Session.SessionID].Add(nodetobind.Attributes["id"].Value + "commenttool", instancename + ".rendercommentsloadref(" + idelement + ",'" + nodetobind.Attributes["id"].Value + "','','true','1','35','" + maxrecord + "'," + onlytotals + "," + viewmode + ");");  
                                    //1 carrello con data range //2 carreelo standard //3 entrambi

                                }
                            }
                        }
                    }
                    else if (nodetobind.Name == "meta")
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string ret = "";
                            List<string> valore = new List<string>();
                            valore.Add(itemdic[property]);
                            List<string> prop = new List<string>();
                            string functiontocall = "";
                            if (nodetobind.Attributes.Contains("format") && !string.IsNullOrEmpty(nodetobind.Attributes["format"].Value))
                            {
                                functiontocall = nodetobind.Attributes["format"].Value;

                                if (nodetobind.Attributes.Contains("mybind1") && itemdic.ContainsKey(nodetobind.Attributes["mybind1"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind1"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind2") && itemdic.ContainsKey(nodetobind.Attributes["mybind2"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind2"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind3") && itemdic.ContainsKey(nodetobind.Attributes["mybind3"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind3"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind4") && itemdic.ContainsKey(nodetobind.Attributes["mybind4"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind4"].Value]);
                                else valore.Add("");

                                if (nodetobind.Attributes.Contains("myvalue"))
                                    prop.Add(nodetobind.Attributes["myvalue"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue1"))
                                    prop.Add(nodetobind.Attributes["myvalue1"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue2"))
                                    prop.Add(nodetobind.Attributes["myvalue2"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue3"))
                                    prop.Add(nodetobind.Attributes["myvalue3"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue4"))
                                    prop.Add(nodetobind.Attributes["myvalue4"].Value);
                                else prop.Add("");

                                ret = CallMappedFunction(functiontocall, valore, prop, nodetobind, itemdic, linkloaded, resultinfo);
                                //if (ret != null && Array.isArray(ret) && ret.length > 0)
                                //    valore = ret[0];
                                //else
                                //    valore = ret;
                                nodetobind.SetAttributeValue("content", ret);
                            }
                        }

                    }
                    else if (nodetobind.Name == "script")
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string ret = "";
                            List<string> valore = new List<string>();
                            valore.Add(itemdic[property]); //valore iniziale su mybind
                            List<string> prop = new List<string>();
                            string functiontocall = "";
                            if (nodetobind.Attributes.Contains("format") && !string.IsNullOrEmpty(nodetobind.Attributes["format"].Value))
                            {
                                functiontocall = nodetobind.Attributes["format"].Value;

                                if (nodetobind.Attributes.Contains("mybind1") && itemdic.ContainsKey(nodetobind.Attributes["mybind1"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind1"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind2") && itemdic.ContainsKey(nodetobind.Attributes["mybind2"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind2"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind3") && itemdic.ContainsKey(nodetobind.Attributes["mybind3"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind3"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind4") && itemdic.ContainsKey(nodetobind.Attributes["mybind4"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind4"].Value]);
                                else valore.Add("");

                                if (nodetobind.Attributes.Contains("myvalue"))
                                    prop.Add(nodetobind.Attributes["myvalue"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue1"))
                                    prop.Add(nodetobind.Attributes["myvalue1"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue2"))
                                    prop.Add(nodetobind.Attributes["myvalue2"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue3"))
                                    prop.Add(nodetobind.Attributes["myvalue3"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue4"))
                                    prop.Add(nodetobind.Attributes["myvalue4"].Value);
                                else prop.Add("");

                                ret = CallMappedFunction(functiontocall, valore, prop, nodetobind, itemdic, linkloaded, resultinfo);
                                //if (ret != null && Array.isArray(ret) && ret.length > 0)
                                //    valore = ret[0];
                                //else
                                //    valore = ret;
                                nodetobind.InnerHtml = ret;
                            }
                        }

                    }
                    else
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string ret = "";
                            List<string> valore = new List<string>();
                            valore.Add(itemdic[property]);
                            List<string> prop = new List<string>();
                            string functiontocall = "";
                            if (nodetobind.Attributes.Contains("format") && !string.IsNullOrEmpty(nodetobind.Attributes["format"].Value))
                            {
                                functiontocall = nodetobind.Attributes["format"].Value;

                                if (nodetobind.Attributes.Contains("mybind1") && itemdic.ContainsKey(nodetobind.Attributes["mybind1"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind1"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind2") && itemdic.ContainsKey(nodetobind.Attributes["mybind2"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind2"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind3") && itemdic.ContainsKey(nodetobind.Attributes["mybind3"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind3"].Value]);
                                else valore.Add("");
                                if (nodetobind.Attributes.Contains("mybind4") && itemdic.ContainsKey(nodetobind.Attributes["mybind4"].Value))
                                    valore.Add(itemdic[nodetobind.Attributes["mybind4"].Value]);
                                else valore.Add("");

                                if (nodetobind.Attributes.Contains("myvalue"))
                                    prop.Add(nodetobind.Attributes["myvalue"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue1"))
                                    prop.Add(nodetobind.Attributes["myvalue1"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue2"))
                                    prop.Add(nodetobind.Attributes["myvalue2"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue3"))
                                    prop.Add(nodetobind.Attributes["myvalue3"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue4"))
                                    prop.Add(nodetobind.Attributes["myvalue4"].Value);
                                else prop.Add("");

                                ret = CallMappedFunction(functiontocall, valore, prop, nodetobind, itemdic, linkloaded, resultinfo);
                                // da finire ....
                                //if (ret != null && Array.isArray(ret) && ret.length > 0)
                                //    valore = ret[0];
                                //else
                                //    valore = ret;
                            }
                            else ret = valore[0];

                            if (ret == "true" || ret == "false")
                            {
                                if (ret == "true")
                                {
                                    if (nodetobind.Attributes.Contains("style"))
                                    {
                                        nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:block", "");
                                        nodetobind.Attributes["style"].Value += ";display:none";
                                    }
                                    else
                                        nodetobind.Attributes.Add("style", "display:none");
                                }
                            }
                            else
                                nodetobind.InnerHtml = ret;
                        }
                        else
                        {
                            nodetobind.InnerHtml = "";
                        }
                    }


                }
                else
                {
                    string[] proprarr = property.Split('.');
                    // da fare binding per 2 livelli
                    if (nodetobind.Name == "span" && itemdic.ContainsKey(proprarr[0]))
                    {
                        try
                        {
                            if (nodetobind.Attributes.Contains("mybind1"))
                            {
                                // da finire e testare ....

                                //var oggetto = itemdic[proprarr[0]];
                                //string proprieta = proprarr[1];
                                //string valore = oggetto[proprieta]; //problema da verificare per l'indice testuale
                                //nodetobind.InnerHtml = valore;
                            }
                            else
                            {
                                var idelem = itemdic[proprarr[0]];
                                string proprieta = proprarr[1];
                                string valore = "";
                                if (linkloaded.ContainsKey(idelem) && linkloaded[idelem].ContainsKey(proprieta) && !string.IsNullOrEmpty(linkloaded[idelem][proprieta]))
                                    valore = linkloaded[idelem][proprieta];
                                nodetobind.InnerHtml = valore;
                            }
                        }
                        catch { }
                    }
                    else if (nodetobind.Name == "iframe")
                    {
                        //da finire
                        if (itemdic.ContainsKey(proprarr[0]))
                        {
                            var idelement = itemdic[proprarr[0]];
                            var prop1 = proprarr[1];
                            var valore = "";
                            if (linkloaded.ContainsKey(idelement) && linkloaded[idelement].ContainsKey(prop1) && !string.IsNullOrEmpty(linkloaded[idelement][prop1]))
                            {
                                valore = linkloaded[idelement][prop1];
                                if (valore != null && valore != "")
                                {
                                    if (nodetobind.Attributes.Contains("src"))
                                        nodetobind.Attributes["src"].Value += valore + "?rel=0";//&autoplay=1

                                    if (nodetobind != null && nodetobind.ParentNode != null)
                                        if (nodetobind.ParentNode.Attributes.Contains("style"))
                                        {
                                            nodetobind.ParentNode.Attributes["style"].Value = nodetobind.ParentNode.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            nodetobind.ParentNode.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            nodetobind.ParentNode.Attributes.Add("style", "display:block");
                                }
                            }
                        }
                    }
                }

            }
            catch
            {

            }
        }

        /// <summary>
        /// Mappatura di tutte le fnzioni di formattazione ... da fare!
        /// </summary>
        /// <param name="functiontocall"></param>
        /// <param name="valore"></param>
        /// <param name="prop"></param>
        /// <param name="nodetobind"></param>
        /// <param name="itemdic"></param>
        /// <param name="linkloaded"></param>
        /// <param name="resultinfo"></param>
        /// <returns></returns>
        private string CallMappedFunction(string functiontocall, List<string> valore, List<string> prop, HtmlNode nodetobind, Dictionary<string, string> itemdic, Dictionary<string, Dictionary<string, string>> linkloaded, Dictionary<string, string> resultinfo)
        {
            string ret = "";

            switch (functiontocall)
            {
                case "formattestoreplace":
                    try
                    {
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                            ret = linkloaded[valore[0]][prop[0]];
                    }
                    catch { }
                    break;
                case "frmvisibility":
                    try
                    {
                        ret = "false";
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                            ret = linkloaded[valore[0]][prop[0]];
                        if (ret != null && ret.Trim() != "")
                            ret = "true";
                        else
                            ret = "false";
                    }
                    catch { }
                    break;
                case "formatvisibilitybyvalore":
                    try
                    {
                        ret = "false";
                        var valorenumerico = valore[1];
                        double.TryParse(valorenumerico, out double p);
                        if (p == 0)
                        {
                            ret = "true";
                        }
                        else
                            ret = "false";
                    }
                    catch { }
                    break;
                case "formatvisibilitybystring":
                    try
                    {
                        ret = "false";
                        var valorestringa = valore[1];
                        if (string.IsNullOrEmpty(valorestringa))
                        {
                            ret = "true";
                        }
                        else
                            ret = "false";
                    }
                    catch { }
                    break;
                case "formatprezzoofferta":
                    try
                    {
                        string unit = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "valuta").Valore;
                        var controllo = "";
                        if (resultinfo.ContainsKey(prop[0]))
                            controllo = resultinfo[prop[0]];
                        if (controllo == "true")
                        {
                            if (valore[0] != "" && valore[0] != "0")
                            {
                                double _tmppz = 0;
                                double.TryParse(valore[0], out _tmppz);
                                ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:##,##0.00}", new object[] { _tmppz }) + ' ' + unit;
                                //ret = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { _tmppz }) + ' ' + unit;
                            }
                        }
                    }
                    catch { }
                    break;
                case "formattitle":
                    try
                    {
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                        {
                            ret = linkloaded[valore[0]][prop[0]];
                            int i = ret.IndexOf('\n');
                            if (i > 0)
                                ret = ret.Substring(0, i);
                        }
                    }
                    catch { }
                    break;
                case "formatsubtitle":
                    try
                    {
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                        {

                            ret = linkloaded[valore[0]][prop[0]];
                            int i = ret.IndexOf('\n');
                            if (i > 0)
                            {
                                if (ret.Length >= i + 1)
                                    ret = ret.Substring(i + 1);
                            }
                            else ret = "";
                        }
                    }
                    catch { }
                    break;
                case "formatalttext":
                    try
                    {
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                        {
                            ret = linkloaded[valore[0]][prop[0]];
                        }
                    }
                    catch { }
                    break;
                case "formatbtncarrello":
                    try
                    {
                        string testoCarelloesaurito = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "testocarelloesaurito").Valore;
                        string testoInseriscicarrello = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "testoinseriscicarrello").Valore;
                        string testoVedi = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "vedi").Valore;
                        string id = valore[0];
                        string qtavendita = valore[1];
                        string xmlvalue = valore[2];
                        string prezzo = valore[3];
                        string modclass = prop[0];
                        string modtext = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, prop[1]).Valore;
                        if (!string.IsNullOrEmpty(modtext)) testoInseriscicarrello = modtext;
                        double p = 0;
                        double.TryParse(prezzo, out p);
                        double q = 0;
                        double.TryParse(qtavendita, out q);
                        if (q == 0 && qtavendita != "")
                            ret = "<div  class=\"" + modclass + " button-carrello btn-carrello-esaurito\"  >" + testoCarelloesaurito + "</div>";
                        else
                        {
                            var testocall = id + "," + Lingua + "," + Username;
                            ret = "<button type=\"button\"  class=\"" + modclass + " button-carrello\" onclick=\"javascript:InserisciCarrelloNopostback('" + testocall + "')\"  >" + testoInseriscicarrello + "</button>";
                            if ((xmlvalue != null && xmlvalue != "") || (prezzo == null || prezzo == "" || p == 0))
                            {
                                var link = "";
                                if (linkloaded.ContainsKey(id) && linkloaded[id].ContainsKey("link"))
                                    link = linkloaded[id]["link"];
                                ret = "<a href=\"" + link + "\" target=\"_self\" >";
                                ret += "<div  class=\" " + modclass + " button-carrello btn-carrello-esaurito\" >" + testoVedi + "</div>";
                                ret += "</a>";
                            }
                        }
                    }
                    catch { }
                    break;
                case "formatlinksezione":
                    try
                    {
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                            ret = linkloaded[valore[0]][prop[0]];
                        if (ret == null) ret = "";
                    }
                    catch { }
                    break;
                case "formatautore":
                    try
                    {
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                            ret = linkloaded[valore[0]][prop[0]];
                        if (ret == null) ret = "";
                    }
                    catch { }
                    break;
                case "formatviews":
                    try
                    {
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                            ret = linkloaded[valore[0]][prop[0]];
                        if (ret == null) ret = "";
                    }
                    catch { }
                    break;
                case "formatdescrizione":
                    try
                    {
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[0]))
                        {
                            string descrizione = linkloaded[valore[0]][prop[0]];
                            if (prop[1] != null && prop[1] != "")
                            {

                                int i = 0;
                                int.TryParse(prop[1], out i);
                                if (descrizione.Length >= i)
                                {
                                    int j = 1; bool stop = false;
                                    while (j < 30 && !stop && i + j + 1 < descrizione.Length)
                                    {
                                        if (descrizione.Substring(i + j, 1) == " " || descrizione.Substring(i + j, 1) == "." || descrizione.Substring(i + j, 1) == "\n") stop = true;
                                        j += 1;
                                    }
                                    descrizione = descrizione.Substring(0, i + j) + "... &#10132;";
                                }

                                if (prop[2] != null && prop[2] == "nobreak")
                                    descrizione = descrizione.Replace("\n", "&nbsp;");
                                else
                                    descrizione = descrizione.Replace("\n", "<br/>");

                                if (prop[3] != null && prop[3].ToLower().StartsWith("filtertags"))
                                {

                                    HtmlDocument doc = new HtmlDocument();
                                    doc.LoadHtml(descrizione); //Template per il bind
                                    HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//a | //img | //iframe"); //tolgo i link e img
                                    if (links != null)
                                        foreach (HtmlNode link in links)
                                        {
                                            link.Remove();
                                        }
                                    links = doc.DocumentNode.SelectNodes("//h2 | //h3 | //p | //b"); //rimpiazzo i tag h2 e h3
                                    if (links != null)
                                        foreach (HtmlNode link in links)
                                        {
                                            link.Name = "span";
                                            link.RemoveClass();
                                        }
                                    descrizione = doc.DocumentNode.OuterHtml;
                                }

                                ret = descrizione;
                            }
                        }
                    }
                    catch { }
                    break;

                case "formatlabelsconto":
                    try
                    {
                        string testosconto = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "testosconto").Valore;
                        var prezzo = valore[0];
                        var prezzolistino = valore[1];
                        double.TryParse(prezzo, out double p);
                        double.TryParse(prezzolistino, out double pl);
                        if (pl != 0 && p != 0 && (pl > p))
                        {
                            ret = "<div class=\"csstransforms prod_discount\"><span>" + testosconto + " ";
                            ret += Math.Floor((pl - p) / pl * 100) + " %";
                            ret += "</span></div>";
                        }
                    }
                    catch { }
                    break;
                case "formatlabelresource":
                    try
                    {
                        string gruppo = "basetext";
                        if (prop[2] != null && prop[2] != "")
                        { gruppo = prop[2]; }
                        string testodarisorsa = WelcomeLibrary.UF.ResourceManagement.ReadKey(gruppo, Lingua, prop[0]).Valore;
                        bool attiva = true;
                        if (prop.Count > 1)
                        {
                            var valorecontrollo = "";
                            if (itemdic.ContainsKey(prop[1]))
                            {
                                valorecontrollo = itemdic[prop[1]];
                                if (string.IsNullOrEmpty(valorecontrollo) || valorecontrollo == "0") attiva = false;
                            }
                        }
                        string controllo = null;
                        if (resultinfo.ContainsKey(prop[1]))
                            controllo = resultinfo[prop[1]];
                        if ((controllo == "true" || controllo == null) && attiva)
                            ret = testodarisorsa;
                    }
                    catch { }
                    break;
                case "formatdata1":
                    try
                    {
                        var tmpDate = valore[0];
                        string controllo = null;
                        if (resultinfo.ContainsKey(prop[0]))
                            controllo = resultinfo[prop[0]];
                        if (controllo == null || controllo == "true")
                        {
                            if (tmpDate != null && tmpDate != "")
                            {
                                //ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:dd MMM yyyy}", new object[] { tmpDate });
                                DateTime _tmpdate = new DateTime();
                                if (DateTime.TryParseExact(tmpDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _tmpdate))
                                    //if (DateTime.TryParse(tmpDate,  out _tmpdate))
                                    ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:dd MMM yyyy}", new object[] { _tmpdate });
                            }
                        }
                    }
                    catch { }
                    break;
                case "formatdata":
                    try
                    {
                        var tmpDate = valore[0];
                        string controllo = null;
                        if (resultinfo.ContainsKey(prop[0]))
                            controllo = resultinfo[prop[0]];
                        if (controllo == null || controllo == "true")
                        {
                            if (tmpDate != null && tmpDate != "")
                            {
                                // ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:dd/MM/yyyy}", new object[] { tmpDate });

                                DateTime _tmpdate = new DateTime();
                                if (DateTime.TryParseExact(tmpDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _tmpdate))
                                    //if (DateTime.TryParse(tmpDate, out _tmpdate))
                                    ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:dd/MM/yyyy}", new object[] { _tmpdate });
                            }
                        }
                    }
                    catch { }
                    break;
                case "formatvalue":
                    try
                    {
                        if (!string.IsNullOrEmpty(valore[0]))
                        {
                            //ret = valore[0];
                            string unit = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "valuta").Valore;
                            double vd = 0;
                            double.TryParse(valore[0], out vd);
                            if (vd != 0)
                                //ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:##,###.00}", new object[] { vd }) + " " + unit;
                                //ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:##,###.00}", new object[] { vd });
                                ret = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:#.00}", new object[] { vd });
                            else
                                ret = valore[0];

                        }
                    }
                    catch { }
                    break;
                case "formatstringvalue":
                    try
                    {
                        if (!string.IsNullOrEmpty(valore[0]))
                        {
                            ret = valore[0];

                            if (prop[0] != null && prop[0] == "replacebr")
                                ret = ret.Replace("\n", "<br/>");
                        }
                    }
                    catch { }
                    break;
                case "formatstringfromresource":
                    try
                    {
                        string gruppo = "basetext";
                        if (prop[0] != null && prop[0] != "")
                        { gruppo = prop[0]; }
                        if (prop[1] != null && !string.IsNullOrEmpty(prop[1]))
                        {
                            string testo = WelcomeLibrary.UF.ResourceManagement.ReadKey(gruppo, Lingua, prop[1]).Valore;
                            ret = testo;
                        }
                    }
                    catch { }
                    break;
                case "frmcategoria":
                    try
                    {
                        string tipobase = "rif000001";
                        if (prop.Count > 0 && prop[0] != "")//opzionalemnte passo la categoria nel primo myvalue
                            tipobase = prop[0];
                        List<Prodotto> plist = WelcomeLibrary.UF.Utility.ElencoProdotti.FindAll(p => p.CodiceTipologia == tipobase && p.Lingua == Lingua);
                        if (plist != null)
                        {
                            Prodotto pitem = plist.Find(p => p.CodiceProdotto == valore[0]);
                            if (pitem != null)
                                ret = pitem.Descrizione;
                        }
                    }
                    catch { }
                    break;
                case "frmcategoria2liv":
                    try
                    {
                        List<SProdotto> listsprod = WelcomeLibrary.UF.Utility.ElencoSottoProdotti.FindAll(p => p.Lingua == Lingua);
                        if (listsprod != null)
                        {
                            SProdotto pitem = listsprod.Find(p => p.CodiceSProdotto == valore[0]);
                            if (pitem != null)
                                ret = pitem.Descrizione;
                        }
                    }
                    catch { }
                    break;
                case "frmtipologia":
                    try
                    {
                        TipologiaOfferte titem = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(p => p.Codice == valore[0] && p.Lingua == Lingua);
                        if (titem != null)
                            ret = titem.Descrizione;
                    }
                    catch { }
                    break;
                case "frmcaratteristica1":
                    try
                    {
                        Tabrif c = Utility.Caratteristiche[0].Find(p => p.Codice == valore[0] && p.Lingua == Lingua);
                        if (c != null)
                            ret = c.Campo1;
                    }
                    catch { }
                    break;
                case "frmcaratteristica2":
                    try
                    {
                        Tabrif c = Utility.Caratteristiche[1].Find(p => p.Codice == valore[0] && p.Lingua == Lingua);
                        if (c != null)
                            ret = c.Campo1;
                    }
                    catch { }
                    break;
                case "frmcaratteristica3":
                    try
                    {
                        Tabrif c = Utility.Caratteristiche[2].Find(p => p.Codice == valore[0] && p.Lingua == Lingua);
                        if (c != null)
                            ret = c.Campo1;
                    }
                    catch { }
                    break;
                case "frmprovincia":
                    try
                    {
                        List<WelcomeLibrary.DOM.Province> provincelingua1 = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == Lingua); });
                        provincelingua1.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                        Province pr = provincelingua1.Find(p => p.Codice == valore[0]);
                        if (pr != null)
                            ret = pr.Provincia;
                    }
                    catch { }
                    break;
                case "frmregione":
                    try
                    {
                        List<WelcomeLibrary.DOM.Province> provincelinguatmp = Utility.ElencoProvince.FindAll(delegate (WelcomeLibrary.DOM.Province tmp) { return (tmp.Lingua == Lingua); });
                        if (provincelinguatmp != null && provincelinguatmp.Count > 0)
                        {
                            provincelinguatmp.Sort(new GenericComparer2<WelcomeLibrary.DOM.Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                            foreach (WelcomeLibrary.DOM.Province item in provincelinguatmp)
                            {
                                if (item.Lingua == Lingua)
                                    if (item.Codice == valore[0]) { ret = item.Regione; break; }
                            }
                        }
                    }
                    catch { }
                    break;
                case "frmscriptlocation": //inietta delle varibili nel dom tramite script javascript iniettatta
                    try
                    {
                        List<Dictionary<string, string>> arrayret = new List<Dictionary<string, string>>();
                        Dictionary<string, string> elems = new Dictionary<string, string>();

                        elems.Add(prop[0], "");
                        if (itemdic.ContainsKey(prop[0]))
                            elems[prop[0]] = itemdic[prop[0]];

                        elems.Add(prop[1], "");
                        if (itemdic.ContainsKey(prop[1]))
                            elems[prop[1]] = itemdic[prop[1]].Replace(",", ".");

                        elems.Add(prop[2], "");
                        if (itemdic.ContainsKey(prop[2]))
                            elems[prop[2]] = itemdic[prop[2]].Replace(",", ".");

                        string titolo = "";
                        if (linkloaded.ContainsKey(valore[0]) && linkloaded[valore[0]].ContainsKey(prop[3]))
                        {
                            titolo = linkloaded[valore[0]][prop[3]];
                        }
                        elems.Add(prop[3], titolo);
                        elems.Add("url", "https://www.google.com/maps/search/?api=1&query=" + elems[prop[1]] + "," + elems[prop[2]]); //https://www.google.com/maps/search/?api=1&query=<lat>,<lng>

                        arrayret.Add(elems);
                        ret = "var gpositems = null;\r\n " + WelcomeLibrary.UF.Utility.waitwrappercall("b64ToUtf8", ";\r\n" + string.Format(" gpositems = JSON.parse(b64ToUtf8('{0}'))", dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(arrayret))) + ";\r\n");
                    }
                    catch { }
                    break;
                default:
                    break;
            }

            return ret;
        }

        public static string CreaInitStringJavascriptOnly(Dictionary<string, string> addelements = null)
        {
            string jscode = "<script>//<![CDATA[\r\n";
            ///*document.addEventListener("DOMContentLoaded", function(event) { //Do work });*/
            //String jqueryready = string.Format("$(function(){0});","console.log('ready from code binder')");
            //jscommands
            // jscode += "console.log('inject from custom bind');\r\n";

            Dictionary<string, string> jscommandstmp = new Dictionary<string, string>();

            if (addelements != null)
                foreach (KeyValuePair<string, string> kv in addelements)
                {
                    if (jscommandstmp.ContainsKey(kv.Key)) jscommandstmp.Remove(kv.Key);
                    jscommandstmp.Add(kv.Key, kv.Value);
                }
            if (jscommandstmp != null)
                foreach (KeyValuePair<string, string> kv in jscommandstmp)
                {
                    jscode += kv.Value + ";\r\n";
                }


            jscode += "//]]></script>\r\n";

            return jscode;
        }

        public static string CreaInitStringJavascript(string sessionid, Dictionary<string, string> addelements = null)
        {
            string jscode = "<script>//<![CDATA[\r\n";
            ///*document.addEventListener("DOMContentLoaded", function(event) { //Do work });*/
            //String jqueryready = string.Format("$(function(){0});","console.log('ready from code binder')");
            //jscommands
            // jscode += "console.log('inject from custom bind');\r\n";

            Dictionary<string, string> jscommandstmp = new Dictionary<string, string>();

            AddInitjavascriptvariables(jscommandstmp);
            if (addelements != null)
                foreach (KeyValuePair<string, string> kv in addelements)
                {
                    if (jscommandstmp.ContainsKey(kv.Key)) jscommandstmp.Remove(kv.Key);
                    jscommandstmp.Add(kv.Key, kv.Value);
                }
            if (jscommandstmp != null)
                foreach (KeyValuePair<string, string> kv in jscommandstmp)
                {
                    jscode += kv.Value + ";\r\n";
                }


            if (jscommands != null && jscommands.ContainsKey(sessionid))
                foreach (KeyValuePair<string, string> kv in jscommands[sessionid])
                {
                    jscode += kv.Value + ";\r\n";
                }

            jscode += "//]]></script>\r\n";
            //jscommands[sessionid] = new Dictionary<string, string>(); 
            jscommands.Remove(sessionid);//Svuoto la memoria dopo aver iniettato le inizializzazioni in pagina
            if (jscommands.ContainsKey("")) jscommands.Remove("");

            return jscode;
        }

        public static void AddInitjavascriptvariables(Dictionary<string, string> jscommandslocal)
        {
            if (jscommandslocal.ContainsKey("NeededJSVars")) jscommandslocal.Remove("NeededJSVars");
            jscommandslocal.Add("NeededJSVars", "var cbindvapidPublicKey = '" + ConfigManagement.ReadKey("PublicKey") + "';\r\n");

        }
    }

}
