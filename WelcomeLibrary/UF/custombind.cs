using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WelcomeLibrary.UF
{
    public static class custombind
    {
        private static string Lingua = "";
        public static string bind(string text, string lingua)
        {
            Lingua = lingua;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);
            var findclasses = doc.DocumentNode.Descendants("div").Where(d =>
                   d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("inject") && d.Attributes.Contains("params")
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
                pars.ForEach(p => p = p.Trim().Trim('\''));
                string containerid = node.Id;
                //node.InnerHtml;
                functioncallmapping(pars, node);

            }
            catch { }

        }

        private static void functioncallmapping(List<string> pars, HtmlNode node)
        {
            /////////////////////////////////////////////////////////////
            //opzionalmente al termine devo anche creare una lista dei binding fatti e delle funzioni javascript di inizializzazione da chiamare idcontenitore, idcontrollo,funzione init, parametri eventuali(da capire se si puo usare unfile.js standard di inizializzazione per classe al ready)!!!!!!!
            /////////////////////////////////////////////////////////////
            if (pars.Count > 0)
                switch (pars[0])
                {
                    case "injectSliderAndLoadBanner":
                        // injectSliderAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height)
                        //Caricamento template
                        string templatetext = "";
                        if (System.IO.File.Exists(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[0]))
                            templatetext = System.IO.File.ReadAllText(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione + "\\lib\\template\\" + pars[0]);
                        HtmlDocument template = new HtmlDocument();
                        template.LoadHtml(templatetext);

                        //Lingua


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
