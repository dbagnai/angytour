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
        private static Dictionary<string, string> jscommands = new Dictionary<string, string>();
        public static string bind(string text, string lingua)
        {
            Lingua = lingua;
            jscommands = new Dictionary<string, string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);
            var findclasses = doc.DocumentNode.Descendants().Where(d =>
                   d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("inject") && !d.Attributes["class"].Value.Contains("clientsideinject") && d.Attributes.Contains("params")
               );
            if ((findclasses != null) && (findclasses.Count() > 0))
            {
                foreach (var node in findclasses)
                {
                    BindElement(node);
                }

            }
            //var imgs = doc.DocumentNode.SelectNodes("//img");
            //if (imgs.Count>0)
            //{
            //	// vado a modificare solo la prima che ha una certa classe
            //}
            return doc.DocumentNode.OuterHtml;
        }

        private static void BindElement(HtmlNode node)
        {
            try
            {
                //facciamo i binding in base ai parametri
                string parametri = node.Attributes["params"].Value;
                List<String> pars = parametri.Split(',').ToList();
                List<String> parsclear = new List<string>();
                pars.ForEach(p => parsclear.Add(p.Trim().Trim('\'').Trim()));
                functioncallmapping(parsclear, node);

            }
            catch (Exception e)
            {

            }

        }

        private static void functioncallmapping(List<string> pars, HtmlNode node)
        {
            Dictionary<string, string> dictpars = new Dictionary<string, string>();
            /////////////////////////////////////////////////////////////
            //opzionalmente al termine devo anche creare una lista dei binding fatti e delle funzioni javascript di inizializzazione da chiamare idcontenitore, idcontrollo,funzione init, parametri eventuali(da capire se si puo usare unfile.js standard di inizializzazione per classe al ready)!!!!!!!
            /////////////////////////////////////////////////////////////
            if (pars.Count > 0)
                switch (pars[0])
                {
                    case "injectSliderAndLoadBanner":
                        // injectSliderAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height)

                        string containerid = node.Id; // id del contenitoure a cui appendere i risultati!
                        //node.InnerHtml; //qui devo inserire i dati col bind
                        //Caricamento template
                        if (pars.Count > 2) dictpars.Add("containerid", pars[2]);
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
                        if (pars.Count > 4) dictpars.Add("page", pars[4]);
                        if (pars.Count > 5) dictpars.Add("pagesize", pars[5]);
                        if (pars.Count > 6) dictpars.Add("enablepager", pars[6]);

                        if (!dictpars.ContainsKey("containerid")) return;
                        if (!dictpars.ContainsKey("controlid")) return;
                        if (!dictpars.ContainsKey("tblsezione")) return;
                        if (!dictpars.ContainsKey("filtrosezione")) return;

                        ///////////////////////////////////////////////////////////
                        // da togliere per abilitare il  nuovo sistema
                        ///////////////////////////////////////////////////////////

                        ///////////////////////////////////////////////////////////
                        //CARICAMENTO TEMPLATE 
                        ///////////////////////////////////////////////////////////
                        string templatetext = "";
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
                            Dictionary<string, string> dictdati = bannersDM.filterDataBanner(Lingua, dictpars, dictpars["page"], dictpars["pagesize"], dictpars["enablepager"]);
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
                                            var bindingnodes = cloneitem.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bind") && d.Attributes.Contains("mybind") && !string.IsNullOrEmpty(d.Attributes["mybind"].Value));
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
                                        node.AppendChild(elementtoappend.First().Clone()); //Appendo il blocco bindato ai dati
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Rimuoviamo il comando per evitare doppio bindig lato client da javascript ( basta rimuovere dal containe la classe inject o l'attributo params!!
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    node.Attributes["class"].Value = node.Attributes["class"].Value.Replace("inject", "");
                                    node.Attributes["params"].Remove();

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Aggiungo un comando javascript da eseguire dopo il binding per l'init del controllo
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    jscommands.Add(containerid, "initSlider('" + containerid + "'," + dictpars["width"] + "," + dictpars["height"] + ")");

                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    /////END BINDING ////////////////////////////////////////////////////////
                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                }
                            }
                        }

                        break;
                    case "injectFasciaAndLoadBanner":
                        //injectFasciaAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola) 

                        break;
                    case "injectScrollerAndLoadinner":
                        //injectScrollerAndLoadinner(type, container, controlid, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, scrollertype, categoria2Liv, vetrina, promozioni) 

                        break;
                    default:
                        break;
                }

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
                //Duplicare il sistema di binding di FillBindControls da common.js
                //( modificando nodetobind con htmlagility viene automaticamente modificato l'elemento che poi è visualizzato

                //da fare conversione c# fillbindcontrole e le funzioni di formattazione usate nell'attributo format!   ......
                string property = nodetobind.Attributes["mybind"].Value;
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
                    else if (nodetobind.Name == "input" && ((nodetobind.Attributes.Contains("type") && nodetobind.Attributes["type"].Value == "text") || !nodetobind.Attributes.Contains("type")))
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
                    else if (nodetobind.Name == "button" && nodetobind.Attributes.Contains("class") && nodetobind.Attributes["class"].Value.Contains("select"))
                    {
                        string idscheda = itemdic[property];
                        if (nodetobind.Attributes.Contains("myvalue"))
                        {
                            if (nodetobind.Attributes.Contains("onclick")) nodetobind.Attributes.Remove("onclick");
                            nodetobind.Attributes.Add("onclick", "javascript:" + nodetobind.Attributes["myvalue"].Value + "('" + idscheda + "');");
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
                        else
                            nodetobind.Attributes.Add("href", "");

                        if (nodetobind.Attributes.Contains("title"))
                            bindprophref = nodetobind.Attributes["title"].Value;
                        if (linkloaded.ContainsKey(idscheda))
                        {
                            if (linkloaded[idscheda].ContainsKey(bindprophref))
                            {
                                link = linkloaded[idscheda][bindprophref];
                                nodetobind.Attributes["href"].Value = link;

                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
                            }
                            if (linkloaded[idscheda].ContainsKey(bindproptitle))
                            {
                                testo = linkloaded[idscheda][bindproptitle];
                                nodetobind.Attributes["title"].Value = testo;

                                if (nodetobind.Attributes.Contains("style"))
                                {
                                    nodetobind.Attributes["style"].Value = nodetobind.Attributes["style"].Value.Replace("display:none", "");
                                    nodetobind.Attributes["style"].Value += ";display:block";
                                }
                                else
                                    nodetobind.Attributes.Add("style", "display:block");
                            }
                            if (link.ToLower().IndexOf(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione.ToLower()) != -1)
                            {
                                if (nodetobind.Attributes.Contains("target")) nodetobind.Attributes.Remove("target");
                                nodetobind.Attributes.Add("target", "_blank");
                            }
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
                                imgslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesratio"]);
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
                                    nodetobind.ParentNode.Attributes["style"].Value = nodetobind.ParentNode.Attributes["style"].Value.Replace("display:none", "");
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
                            //    imgslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(linkloaded[idscheda]["imagesratio"]);
                            StringBuilder sb = new StringBuilder();
                            if (imgslist.Count() > 1)
                                for (int j = 0; j < imgslist.Count(); j++)
                                {
                                    sb.Append("<li> <img style=\"padding:5px\" src=\"");

                                }
                        }

                    }
                    //da continuare .... col mapping .............. da FillBindControls

                }
                else
                {
                    string[] proprarr = property.Split('.');
                    // da fare binding per 2 livelli
                    //....

                }
            }
            catch (Exception e)
            {

            }
        }

        public static string CreaInitStringJavascript()
        {
            string jscode = "<script>\r\n";
            ///*document.addEventListener("DOMContentLoaded", function(event) { //Do work });*/
            //String jqueryready = string.Format("$(function(){0});","console.log('ready from code binder')");
            //jscommands
            jscode += "console.log('inject from custom bind');\r\n";
            if (jscommands != null)
                foreach (KeyValuePair<string, string> kv in jscommands)
                {
                    jscode += kv.Value + ";\r\n";
                }
            jscode += "</script>\r\n";
            return jscode;
        }




        //private static string SostituisciImmagineSlide(string retContent, string fileType, string slideContent, string dummyImage)
        //{
        //    if (fileType == "image")
        //    {

        //        string srcContent = "[base]/img/" + dummyImage;

        //        /***************************************************/
        //        // nel caso di survey l'immagine va sostituita nel css contenuto nella slide
        //        // cha ha la forma 
        //        //background: url('#url#') no - repeat center center fixed;
        //        // siccome è presente solo nel survey, se c'è bene sennò non importa
        //        slideContent = slideContent.Replace("#url#", srcContent);
        //        /***************************************************/

        //        HtmlDocument doc = new HtmlDocument();
        //        doc.LoadHtml(slideContent);

        //        //var findclasses = doc.DocumentNode.Descendants("div").Where(d =>
        //        //	d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("float")
        //        //);
        //        var findclasses = doc.DocumentNode.Descendants("img").Where(d =>
        //            d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("slideImage")
        //        );
        //        if ((findclasses != null) && (findclasses.Count() > 0))
        //        {
        //            // prendiamo il primo
        //            var node = findclasses.FirstOrDefault();
        //            if (node.Attributes["src"] != null)
        //                node.Attributes["src"].Value = srcContent;
        //            else
        //                node.Attributes.Add("src", srcContent);
        //        }

        //        //var imgs = doc.DocumentNode.SelectNodes("//img");
        //        //if (imgs.Count>0)
        //        //{
        //        //	// vado a modificare solo la prima che ha una certa classe

        //        //}

        //        retContent = "OKIMAGE|" + doc.DocumentNode.OuterHtml;
        //    }

        //    return retContent;
        //}


    }
}
