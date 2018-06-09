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

        public static string bind(string text)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);



            return doc.DocumentNode.OuterHtml;
        }

        private static string SostituisciImmagineSlide(string retContent, string fileType, string slideContent, string dummyImage)
        {
            if (fileType == "image")
            {

                string srcContent = "[base]/img/" + dummyImage;

                /***************************************************/
                // nel caso di survey l'immagine va sostituita nel css contenuto nella slide
                // cha ha la forma 
                //background: url('#url#') no - repeat center center fixed;
                // siccome è presente solo nel survey, se c'è bene sennò non importa
                slideContent = slideContent.Replace("#url#", srcContent);
                /***************************************************/

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(slideContent);

                //var findclasses = doc.DocumentNode.Descendants("div").Where(d =>
                //	d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("float")
                //);
                var findclasses = doc.DocumentNode.Descendants("img").Where(d =>
                    d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("slideImage")
                );
                if ((findclasses != null) && (findclasses.Count() > 0))
                {
                    // prendiamo il primo
                    var node = findclasses.FirstOrDefault();
                    if (node.Attributes["src"] != null)
                        node.Attributes["src"].Value = srcContent;
                    else
                        node.Attributes.Add("src", srcContent);
                }

                //var imgs = doc.DocumentNode.SelectNodes("//img");
                //if (imgs.Count>0)
                //{
                //	// vado a modificare solo la prima che ha una certa classe

                //}

                retContent = "OKIMAGE|" + doc.DocumentNode.OuterHtml;
            }

            return retContent;
        }


    }
}
