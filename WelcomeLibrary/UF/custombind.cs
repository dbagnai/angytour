using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Text.RegularExpressions;

namespace WelcomeLibrary.UF
{
    public static class custombind
    {
        private static string Lingua = "";
        private static string Username = "";
        private static System.Web.SessionState.HttpSessionState Session = null;
        private static System.Web.HttpRequest Richiesta = null;
        public static Dictionary<string, string> jscommands = new Dictionary<string, string>();
        public static string bind(string text, string lingua, string username = "", System.Web.SessionState.HttpSessionState sessione = null, Dictionary<string, string> filtri = null, Dictionary<string, string> filtripager = null, System.Web.HttpRequest richiesta = null)
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



        private static void BindElement(HtmlNode node, HtmlDocument doc, Dictionary<string, string> filtri = null, Dictionary<string, string> filtripager = null)
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

        private static void functioncallmapping(List<string> pars, HtmlNode node, HtmlDocument doc, Dictionary<string, string> dictpars = null, Dictionary<string, string> dictpagerpars = null)
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
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
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
                                if ((elementtoappend != null) && (elementtoappend.Count() > 0))
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
                                    if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    jscommands.Add(container, "initSlider('" + dictpars["controlid"] + "','" + node.Id + "'," + dictpars["width"] + "," + dictpars["height"] + ")");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
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
                                    if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    jscommands.Add(container, "initcycleBanner('" + dictpars["controlid"] + "','" + node.Id + "')");

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
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
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
                                    if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    jscommands.Add(container, "InitIsotopeLocalBanner('" + dictpars["controlid"] + "');");

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
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
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
                                    if (node != null)
                                        if (node.Attributes.Contains("style"))
                                        {
                                            node.Attributes["style"].Value = node.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                            node.Attributes["style"].Value += ";display:block";
                                        }
                                        else
                                            node.ParentNode.Attributes.Add("style", "display:block");
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
                                    //if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    //jscommands.Add(container, "InitIsotopeLocalBanner('" + dictpars["controlid"] + "');");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectscrollerandLoadbanner":
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
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
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
                                    if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    jscommands.Add(container, "initScrollerBanner('" + dictpars["controlid"] + "','" + dictpars["scrollertype"] + "');");

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
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 0) dictpagerpars.Add("page", "1");
                        if (pars.Count > 0) dictpagerpars.Add("pagesize", "1");
                        if (pars.Count > 0) dictpagerpars.Add("enablepager", "false");

                        if (!dictpars.ContainsKey("container")) return;
                        if (!dictpars.ContainsKey("controlid")) return;


                        //////////////////////////////////////////////////
                        //Ricarichiamo dalla session eventuali parametri aggiuntivi non passati nella chiamata di bind ma presenti in sessione
                        //////////////////////////////////////////////////
#if true
                        if (Session != null && Session["objfiltro"] != null)
                        {
                            string retval = Session["objfiltro"].ToString();//Prendo dalla sessione la chiave che contiene i parametri aggiuntivi serializzati
                            if (retval != null && retval != "")
                            {
                                Dictionary<string, string> dictparsfromsession = new Dictionary<string, string>();
                                dictparsfromsession = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(retval);
                                if (dictparsfromsession != null)
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
#endif


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
                            Dictionary<string, string> dictdati = offerteDM.filterData(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
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
                                    if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    jscommands.Add(container, "initScrollertype('" + dictpars["controlid"] + "','" + dictpars["container"] + "','" + dictpars["scrollertype"] + "');");

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
#if true
                        if (Session != null && Session["objfiltro"] != null)
                        {
                            string retval = Session["objfiltro"].ToString();//Prendo dalla sessione la chiave che contiene i parametri aggiuntivi serializzati
                            if (retval != null && retval != "")
                            {
                                Dictionary<string, string> dictparsfromsession = new Dictionary<string, string>();
                                dictparsfromsession = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(retval);
                                if (dictparsfromsession != null)
                                    foreach (KeyValuePair<string, string> kv in dictparsfromsession)
                                    {
                                        //if (kv.Key == ("page" + dictpars["controlid"]))
                                        //    dictpagerpars["page"] = kv.Value;//la pagina la prendo dalla sessione se presente!
                                        //aggiungo i parametri dalla sessione se presenti
                                        if (!dictpars.ContainsKey(kv.Key)) dictpars.Add(kv.Key, kv.Value);
                                        //else dictpars[kv.Key] = kv.Value;//sovrascivo il valore passato
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
                                var flexgallerycontainer = template.DocumentNode.Descendants().Where(c => c.Id == "plhGallery");
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
                            Dictionary<string, string> dictdati = offerteDM.filterData(Lingua, dictpars, "1", "1", "false");
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
                                    if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    jscommands.Add(container, "inizializzaFlexsliderGallery('" + dictpars["controlid"] + "','" + dictpars["container"] + "');");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectportfolioandload":
                        // injectPortfolioAndLoad(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, mostviewed) 
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
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 4 && !dictpagerpars.ContainsKey("page")) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5 && !dictpagerpars.ContainsKey("pagesize")) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6 && !dictpagerpars.ContainsKey("enablepager")) dictpagerpars.Add("enablepager", pars[6]);

                        //////////////////////////////////////////////////
                        //Ricarichiamo dalla session eventuali parametri aggiuntivi non passati nella chiamata di bind ma presenti in sessione
                        //////////////////////////////////////////////////
#if true
                        if (Session != null && Session["objfiltro"] != null)
                        {
                            string retval = Session["objfiltro"].ToString();//Prendo dalla sessione la chiave che contiene i parametri aggiuntivi serializzati
                            if (retval != null && retval != "")
                            {
                                Dictionary<string, string> dictparsfromsession = new Dictionary<string, string>();
                                dictparsfromsession = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(retval);
                                if (dictparsfromsession != null)
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
#endif
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
                            Dictionary<string, string> dictdati = offerteDM.filterData(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
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

                                                ///////////PAGINAZIONE PER LINK CON QUERYSTRING
#if true
                                                var aNextPage = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "aNextPage");
                                                if ((aNextPage != null) && (aNextPage.Count() > 0) && Richiesta != null && page < pagesnumber)
                                                {
                                                    aNextPage.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore;
                                                    //aNextPage.First().SetAttributeValue("onClick", "javascript:nextpagebindonserver('" + dictpars["controlid"] + "')");
                                                    aNextPage.First().SetAttributeValue("href", Richiesta.Url.LocalPath + "?" + "page=" + nextpage);
                                                    //aNextPage.First().SetAttributeValue("onClick", "scrolltotop.scrollup();");

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
                                                    aPrevPage.First().SetAttributeValue("href", Richiesta.Url.LocalPath + "?" + "page=" + prevpage);
                                                    //aPrevPage.First().SetAttributeValue("onClick", "scrolltotop.scrollup();");

                                                    if (aPrevPage.First().Attributes.Contains("style"))
                                                    {
                                                        aPrevPage.First().Attributes["style"].Value = aPrevPage.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        aPrevPage.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        aPrevPage.First().Attributes.Add("style", "display:block");
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


                                    //Aggiorno le variaibli javascript globalObject[controlid + "params"] ( da dictpars) e globalObject[controlid + "pagerdata" ] ( da dictpagerpars ) per far funzionare il pager lato client !!!
                                    //La seguente prepara una chiamata a funzione javascript che inizializza le variabili js. ( SERVE SOLO PER UTILIZZO LATO CLIENT )
                                    if (jscommands.ContainsKey(container + "-2")) jscommands.Remove(container + "-2");
                                    jscommands.Add(container + "-2", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (jscommands.ContainsKey(container + "-1")) jscommands.Remove(container + "-1");
                                    jscommands.Add(container + "-1", "InitIsotopeLocal('" + dictpars["controlid"] + "','" + dictpars["container"] + "');");

                                    //if (dictpagerpars["enablepager"] == "true")
                                    //{
                                    //jscommands.Add(container + "-2", "initHtmlPager('" + dictpars["controlid"] + "');");
                                    // jscommands.Add(container + "-3", "renderPager('" + dictpars["controlid"] + "');"); //QESUTA LA DEVI REPLICARE LATO SERBE USA LE RISORSE!!!!!! o gli devi passare le risporse necessarie!!
                                    //}

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING //////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }
                        break;
                    case "injectbootstrapportfolioandload":
                        // injectPortfolioAndLoad(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, mostviewed) 
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
                        ////////////////////////////(PAGINAZIONE ... )
                        if (pars.Count > 4 && !dictpagerpars.ContainsKey("page")) dictpagerpars.Add("page", pars[4]);
                        if (pars.Count > 5 && !dictpagerpars.ContainsKey("pagesize")) dictpagerpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6 && !dictpagerpars.ContainsKey("enablepager")) dictpagerpars.Add("enablepager", pars[6]);

                        //////////////////////////////////////////////////
                        //Ricarichiamo dalla session eventuali parametri aggiuntivi non passati nella chiamata di bind ma presenti in sessione
                        //////////////////////////////////////////////////
#if true
                        if (Session != null && Session["objfiltro"] != null)
                        {
                            string retval = Session["objfiltro"].ToString();//Prendo dalla sessione la chiave che contiene i parametri aggiuntivi serializzati
                            if (retval != null && retval != "")
                            {
                                Dictionary<string, string> dictparsfromsession = new Dictionary<string, string>();
                                dictparsfromsession = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(retval);
                                if (dictparsfromsession != null)
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
#endif
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
                            Dictionary<string, string> dictdati = offerteDM.filterData(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
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

                                                ///////////PAGINAZIONE PER LINK CON QUERYSTRING
#if true
                                                var aNextPage = pagercontainer.First().Descendants().Where(t => t.Id == dictpars["controlid"] + "aNextPage");
                                                if ((aNextPage != null) && (aNextPage.Count() > 0) && Richiesta != null && page < pagesnumber)
                                                {
                                                    aNextPage.First().InnerHtml = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, "pageravanti").Valore;
                                                    //aNextPage.First().SetAttributeValue("onClick", "javascript:nextpagebindonserver('" + dictpars["controlid"] + "')");
                                                    aNextPage.First().SetAttributeValue("href", Richiesta.Url.LocalPath + "?" + "page=" + nextpage);

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
                                                    aPrevPage.First().SetAttributeValue("href", Richiesta.Url.LocalPath + "?" + "page=" + prevpage);

                                                    if (aPrevPage.First().Attributes.Contains("style"))
                                                    {
                                                        aPrevPage.First().Attributes["style"].Value = aPrevPage.First().Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                                        aPrevPage.First().Attributes["style"].Value += ";display:block";
                                                    }
                                                    else
                                                        aPrevPage.First().Attributes.Add("style", "display:block");
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


                                    //Aggiorno le variaibli javascript globalObject[controlid + "params"] ( da dictpars) e globalObject[controlid + "pagerdata" ] ( da dictpagerpars ) per far funzionare il pager lato client !!!
                                    //La seguente prepara una chiamata a funzione javascript che inizializza le variabili js. ( SERVE SOLO PER UTILIZZO LATO CLIENT )
                                    if (jscommands.ContainsKey(container + "-2")) jscommands.Remove(container + "-2");
                                    jscommands.Add(container + "-2", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //if (jscommands.ContainsKey(container + "-1")) jscommands.Remove(container + "-1");
                                    //jscommands.Add(container + "-1", "InitIsotopeLocal('" + dictpars["controlid"] + "','" + dictpars["container"] + "');");

                                    //if (dictpagerpars["enablepager"] == "true")
                                    //{
                                    //jscommands.Add(container + "-2", "initHtmlPager('" + dictpars["controlid"] + "');");
                                    // jscommands.Add(container + "-3", "renderPager('" + dictpars["controlid"] + "');"); //QESUTA LA DEVI REPLICARE LATO SERBE USA LE RISORSE!!!!!! o gli devi passare le risporse necessarie!!
                                    //}

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
                            //Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpagerpars["page"], dictpagerpars["pagesize"], dictpagerpars["enablepager"]);
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, "1", "1", "false");
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

                                    if (jscommands.ContainsKey(container + "-2")) jscommands.Remove(container + "-2");
                                    jscommands.Add(container + "-2", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    jscommands.Add(container, "InitVideo('" + dictpars["controlid"] + "','" + node.Id + "')");

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
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, "1", "1", "false");
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

                                    if (jscommands.ContainsKey(container + "-2")) jscommands.Remove(container + "-2");
                                    jscommands.Add(container + "-2", "initGlobalVarsFromServer('" + dictpars["controlid"] + "','" + dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(dictpars)) + "','" + Newtonsoft.Json.JsonConvert.SerializeObject(dictpagerpars) + "');");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (jscommands.ContainsKey(container)) jscommands.Remove(container);
                                    jscommands.Add(container, "InitGenericBanner('" + dictpars["controlid"] + "','" + node.Id + "')");

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
        public static bool IsEven(int value)
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
        private static void DataBindElement(HtmlNode nodetobind, Dictionary<string, string> itemdic, Dictionary<string, Dictionary<string, string>> linkloaded, Dictionary<string, string> resultinfo)
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
                            nodetobind.Attributes["href"].Value = "tel:" + link;
                            nodetobind.InnerHtml += link;
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
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
                            }
                            if (linkloaded[idscheda].ContainsKey(bindproptitle))
                            {
                                testo = linkloaded[idscheda][bindproptitle];
                                nodetobind.Attributes["title"].Value = html.Convert(testo);
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace(": ", ":").Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
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
                    else if (nodetobind.Name == "img")
                    {
                        string completepath = "";
                        string idscheda = "";
                        if (itemdic.ContainsKey(property))
                        {
                            idscheda = itemdic[property];
                            if (linkloaded.ContainsKey(idscheda) && linkloaded[idscheda].ContainsKey("image"))
                                completepath = linkloaded[idscheda]["image"];
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
                            //foreach (string img in imgslist)
                            StringBuilder sb = new StringBuilder();
                            for (int j = 0; j < imgslist.Count(); j++)
                            {
                                try
                                {
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

                                    string imgstyle = "max-width:100%;height:auto;";
                                    string maxheight = "";
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
                                    if (maxheight != "")
                                    {
                                        maxheight = maxheight.Replace("px", "");
                                        int calcheight = 0;
                                        if (int.TryParse(maxheight, out calcheight))
                                        {
                                            int actwidth = 0;
                                            if (int.TryParse(WelcomeLibrary.STATIC.Global.Viewportw, out actwidth))
                                                if (calcheight > actwidth) calcheight = actwidth;
                                            try
                                            {
                                                double ar = 1;
                                                if (double.TryParse(imgslistratio[j], out ar))
                                                    if (ar < 1)
                                                    {
                                                        //imgstyle = "max-width:100%;width:auto;height:" + maxheight + "px;";
                                                        imgstyle = "max-width:100%;width:auto;height:" + maxheight + "px;";
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
                            StringBuilder sb = new StringBuilder();
                            if (imgslist.Count() > 1)
                                for (int j = 0; j < imgslist.Count(); j++)
                                {
                                    try
                                    {
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
                                    if (string.IsNullOrEmpty(descrizione)) descrizione = (fileslist[j]);
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


                            if (nodetobind != null && !string.IsNullOrEmpty(completepath))
                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value += ";background-image:url('" + completepath + "')";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "background-image:url('" + completepath + "')");
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
                                if (jscommands.ContainsKey(nodetobind.Attributes["id"].Value)) jscommands.Remove(nodetobind.Attributes["id"].Value);
                                jscommands.Add(nodetobind.Attributes["id"].Value, "bookingtool.initbookingtool(" + idelement + "," + nodetobind.Attributes["id"].Value + ");"); //Imposto la chiamata da tornare
                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("carellotool"))
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string idelement = itemdic[property];
                            if (nodetobind.Attributes.Contains("id") && !string.IsNullOrEmpty(nodetobind.Attributes["id"].Value))
                            {    //carrellotool.initcarrellotool(idelement,  ..... );
                                if (jscommands.ContainsKey(nodetobind.Attributes["id"].Value)) jscommands.Remove(nodetobind.Attributes["id"].Value);
                                jscommands.Add(nodetobind.Attributes["id"].Value, "carrellotool.initcarrellotool(" + idelement + ",'','" + Username + "','" + nodetobind.Attributes["id"].Value + "', 2);");  //1 carrello con data range //2 carreelo standard //3 entrambi
                            }
                        }
                    }
                    else if (nodetobind.Name == "div" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("commenttool"))
                    {
                        if (itemdic.ContainsKey(property))
                        {
                            string idelement = itemdic[property];
                            if (nodetobind.Attributes.Contains("id") && !string.IsNullOrEmpty(nodetobind.Attributes["id"].Value))
                            {    //carrellotool.initcarrellotool(idelement,  ..... );
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

                                if (jscommands.ContainsKey(nodetobind.Attributes["id"].Value + "commenttool")) jscommands.Remove(nodetobind.Attributes["id"].Value + "commenttool");
                                jscommands.Add(nodetobind.Attributes["id"].Value + "commenttool", instancename + ".rendercommentsloadref(" + idelement + ",'" + nodetobind.Attributes["id"].Value + "','','true','1','35','" + maxrecord + "'," + onlytotals + "," + viewmode + ");");  //1 carrello con data range //2 carreelo standard //3 entrambi
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

                                if (nodetobind.Attributes.Contains("myvalue"))
                                    prop.Add(nodetobind.Attributes["myvalue"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue1"))
                                    prop.Add(nodetobind.Attributes["myvalue1"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue2"))
                                    prop.Add(nodetobind.Attributes["myvalue2"].Value);
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

                                if (nodetobind.Attributes.Contains("myvalue"))
                                    prop.Add(nodetobind.Attributes["myvalue"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue1"))
                                    prop.Add(nodetobind.Attributes["myvalue1"].Value);
                                else prop.Add("");
                                if (nodetobind.Attributes.Contains("myvalue2"))
                                    prop.Add(nodetobind.Attributes["myvalue2"].Value);
                                else prop.Add("");

                                ret = CallMappedFunction(functiontocall, valore, prop, nodetobind, itemdic, linkloaded, resultinfo);
                                // da finire ....
                                //if (ret != null && Array.isArray(ret) && ret.length > 0)
                                //    valore = ret[0];
                                //else
                                //    valore = ret;
                            }

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
        private static string CallMappedFunction(string functiontocall, List<string> valore, List<string> prop, HtmlNode nodetobind, Dictionary<string, string> itemdic, Dictionary<string, Dictionary<string, string>> linkloaded, Dictionary<string, string> resultinfo)
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

                                ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:##,###.00}", new object[] { valore[0] }) + ' ' + unit;
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
                        double p = 0;
                        double.TryParse(prezzo, out p);
                        double q = 0;
                        double.TryParse(qtavendita, out q);
                        if (q == 0 && qtavendita != "")
                            ret = "<div  class=\"button-carrello btn-carrello-esaurito\"  >" + testoCarelloesaurito + "</div>";
                        else
                        {
                            var testocall = id + "," + Lingua + "," + Username;
                            ret = "<button type=\"button\"  class=\"button-carrello\" onclick=\"javascript:InserisciCarrelloNopostback('" + testocall + "')\"  >" + testoInseriscicarrello + "</button>";
                            if ((xmlvalue != null && xmlvalue != "") || (prezzo == null || prezzo == "" || p == 0))
                            {
                                var link = "";
                                if (linkloaded.ContainsKey(id) && linkloaded[id].ContainsKey("link"))
                                    link = linkloaded[id]["link"];
                                ret = "<a href=\"" + link + "\" target=\"_self\" >";
                                ret += "<div  class=\"button-carrello btn-carrello-esaurito\" >" + testoVedi + "</div>";
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
                                    descrizione = descrizione.Substring(0, i + j) + " >>";
                                }
                                if (prop[2] != null && prop[2] == "nobreak")
                                    ret = descrizione.Replace("\n", "&nbsp;");
                                else
                                    ret = descrizione.Replace("\n", "<br/>");
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
                            ret = "<div class=\"csstransforms prod_discount\">" + testosconto + " ";
                            ret += Math.Floor((pl - p) / pl * 100) + " %";
                            ret += "</div>";
                        }
                    }
                    catch { }
                    break;
                case "formatlabelresource":
                    try
                    {
                        string testodarisorsa = WelcomeLibrary.UF.ResourceManagement.ReadKey("basetext", Lingua, prop[0]).Valore;
                        string controllo = null;
                        if (resultinfo.ContainsKey(prop[1]))
                            controllo = resultinfo[prop[1]];
                        if (controllo == "true" || controllo == null)
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
                        if (controllo == "true")
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
                        if (controllo == "true")
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
                                ret = String.Format(WelcomeLibrary.UF.Utility.setCulture(Lingua), "{0:##,###.00}", new object[] { vd }) + " " + unit;
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
                        ret = ";\r\n" + string.Format("var gpositems = JSON.parse(b64ToUtf8('{0}'))", dataManagement.EncodeUtfToBase64(Newtonsoft.Json.JsonConvert.SerializeObject(arrayret))) + ";\r\n";
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
            string jscode = "<script>//![CDATA[\r\n";
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

        public static string CreaInitStringJavascript(Dictionary<string, string> addelements = null)
        {
            string jscode = "<script>//![CDATA[\r\n";
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


            if (jscommands != null)
                foreach (KeyValuePair<string, string> kv in jscommands)
                {
                    jscode += kv.Value + ";\r\n";
                }

            jscode += "//]]></script>\r\n";
            jscommands = new Dictionary<string, string>(); //Svuoto la memoria dopo aver iniettato le inizializzazioni in pagina

            return jscode;
        }

        public static void AddInitjavascriptvariables(Dictionary<string, string> jscommands)
        {
            if (jscommands.ContainsKey("NeededJSVars")) jscommands.Remove("NeededJSVars");
            jscommands.Add("NeededJSVars", "var cbindvapidPublicKey = '" + ConfigManagement.ReadKey("PublicKey") + "';\r\n");

        }
    }

}
