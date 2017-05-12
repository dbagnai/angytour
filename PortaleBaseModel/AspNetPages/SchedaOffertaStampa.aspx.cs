using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using GELibraryRemoto.UF;
//using GELibraryRemoto.DOM;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;

public partial class _SchedaOffertaStampa : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string PercorsoComune
    {
        get { return ViewState["PercorsoComune"] != null ? (string)(ViewState["PercorsoComune"]) : ""; }
        set { ViewState["PercorsoComune"] = value; }
    }
    public string PercorsoFiles
    {
        get { return ViewState["PercorsoFiles"] != null ? (string)(ViewState["PercorsoFiles"]) : ""; }
        set { ViewState["PercorsoFiles"] = value; }
    }
    public string PercorsoAssolutoApplicazione
    {
        get { return ViewState["PercorsoAssolutoApplicazione"] != null ? (string)(ViewState["PercorsoAssolutoApplicazione"]) : ""; }
        set { ViewState["PercorsoAssolutoApplicazione"] = value; }
    }
    public string idOfferta
    {
        get { return ViewState["idOfferta"] != null ? (string)(ViewState["idOfferta"]) : ""; }
        set { ViewState["idOfferta"] = value; }
    }
    public string CodiceTipologia
    {
        get { return ViewState["Tipologia"] != null ? (string)(ViewState["Tipologia"]) : ""; }
        set { ViewState["Tipologia"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;

                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua");
                idOfferta = CaricaValoreMaster(Request, Session, "idOfferta");
                CodiceTipologia = CaricaValoreMaster(Request, Session, "Tipologia");


                #region CARICHIAMO  E ASSOCIAMO AI DATI I CONTROLLI NELLA PAGINA
                //Prendiamo la lista completa degli immobili con tutti dati relativi
                //filtrandoli in base ai parametri richiesti
                this.AssociaDati();
                #endregion

                //Inizializziamo le etichette dei controlli in base alla lingua
                //InizializzaEtichette();
                //InizializzaLink();
            }
            else
            {
                output.Text = "";

            }

        }
        catch (Exception err)
        {
            output.Text = err.Message;
        }
    }
    protected void AssociaDati()
    {
        //Carichiamo l'immobile a partire dal codice dello stesso e dalla lingua
        OfferteCollection offerte = new OfferteCollection();

#if true

        Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
        offerte.Add(item);

#endif
        //Associamo ai controlli i dati dell'immobile
        rptOfferta.DataSource = offerte;
        rptOfferta.DataBind();

    }

    protected void ImgAnt_PreRender(object sender, EventArgs e)
    {
        int maxwidth = 600;
        int maxheight = 310;
        try
        {
#if true
            //Meglio testare prma se l'immagine esiste invece di fare try catch
            if (File.Exists(Server.MapPath(((Image)sender).ImageUrl)))
            {
                using (System.Drawing.Image tmp = System.Drawing.Image.FromFile(Server.MapPath(((Image)sender).ImageUrl)))
                {
                    if (tmp.Width >= tmp.Height)
                    {
                        ((Image)sender).Width = maxwidth;
                        int altezza = tmp.Height * maxwidth / tmp.Width;

                        if (altezza < maxheight)
                            ((Image)sender).Height = altezza;
                        else
                        {
                            ((HtmlGenericControl)(((Image)sender).Parent)).Attributes["style"] = "height:" + maxheight + "px;overflow: hidden; float: left; margin: 5px 5px 5px 0px";
                            //((Image)sender).Height = maxheight;
                            //((Image)sender).Width = tmp.Width * maxheight / tmp.Height;
                        }
                    }
                    else
                    {
                        ((Image)sender).Height = maxheight;
                        int larghezza = tmp.Width * maxheight / tmp.Height;
                        if (larghezza < maxwidth)
                            ((Image)sender).Width = larghezza;
                        else
                        {
                            ((Image)sender).Width = maxwidth;
                            ((Image)sender).Height = tmp.Height * maxwidth / tmp.Width;
                        }
                    }
                }
            }
            else
            {//File inesistente
                ((Image)sender).Width = maxwidth;
                ((Image)sender).Height = maxheight;
            }
#endif

        }
        catch
        { }


    }
 
    protected string ComponiUrlFotoProgressivo(object Fotocollection_M, string CodiceTipologia, string idOfferta, int i = 1)
    {
        string ritorno = "";
        string NomeAnteprima = "";
        if (Fotocollection_M != null)
        {
            AllegatiCollection list = ((AllegatiCollection)Fotocollection_M);
            if (list.Count > i)
            {
                NomeAnteprima = list[i].NomeFile;
            }

            if (NomeAnteprima != null && NomeAnteprima != "")
            {
                         if (!NomeAnteprima.ToString().ToLower().StartsWith("http://") && !NomeAnteprima.ToString().ToLower().StartsWith("https://"))
                {
                    if (CodiceTipologia != "" && idOfferta != "")
                        if ((NomeAnteprima.ToString().ToLower().EndsWith("jpg") || NomeAnteprima.ToString().ToLower().EndsWith("gif") || NomeAnteprima.ToString().ToLower().EndsWith("png")))
                        {
                            ritorno = WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + CodiceTipologia + "/" + idOfferta.ToString();
                            //Così ritorno l'immagine non di anteprima ma quella pieno formato
                            if (NomeAnteprima.ToString().StartsWith("Ant"))
                                ritorno += "/" + NomeAnteprima.ToString().Remove(0, 3);
                            else
                                ritorno += "/" + NomeAnteprima.ToString();
                        }
                        else
                            ritorno = "";
                }
                else
                    ritorno = NomeAnteprima.ToString();
            }
            else
            {

            }

        }
        return ritorno;
    }
    protected string ShowFotoUrls(object Fotocollection_M, string CodiceTipologia, string idOfferta)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string htmlimages = "";
        string NomeAnteprima = "";
        int maximages = 7;
        int i = 1;
        if (Fotocollection_M != null)
        {
            AllegatiCollection list = ((AllegatiCollection)Fotocollection_M);
            foreach (Allegato a in list)
            {
                NomeAnteprima = a.NomeFile;
                if (NomeAnteprima != null && NomeAnteprima != "")
                {
                    string pathimage = "";
                             if (!NomeAnteprima.ToString().ToLower().StartsWith("http://") && !NomeAnteprima.ToString().ToLower().StartsWith("https://"))
                    {
                        if (CodiceTipologia != "" && idOfferta != "")
                            if ((NomeAnteprima.ToString().ToLower().EndsWith("jpg") || NomeAnteprima.ToString().ToLower().EndsWith("gif") || NomeAnteprima.ToString().ToLower().EndsWith("png")))
                            {
                                  pathimage = WelcomeLibrary.STATIC.Global.PercorsoContenuti + "/" + CodiceTipologia + "/" + idOfferta.ToString();
                                //Così ritorno l'immagine non di anteprima ma quella pieno formato
                                if (NomeAnteprima.ToString().StartsWith("Ant"))
                                    pathimage += "/" + NomeAnteprima.ToString().Remove(0, 3);
                                else
                                    pathimage += "/" + NomeAnteprima.ToString();
                            }

                    }
                    else
                    {
                          pathimage = NomeAnteprima.ToString();
                    }
                    if (!string.IsNullOrEmpty(pathimage))
                    {
                        sb.Append( "<div  style=\"overflow: hidden; margin: 5px 5px 5px 0px; height: 150px\">");
                        sb.Append( "<img src=\"" + ReplaceAbsoluteLinks(  pathimage ) + "\" Style=\"width: 180px\" />");
                        sb.Append("</div>\r\n");
                        i += 1;
                    }
                }
                if (i > maximages) break;
            }
        }
        htmlimages = sb.ToString();
        return htmlimages;
    }

    protected string ControlloVuoto(string pre, string contenuto)
    {
        string ret = "";

        if (!string.IsNullOrWhiteSpace(contenuto))
            ret = pre + contenuto;

        return ret;

    }
    protected string TipoOfferta()
    {
        string ritorno = "";
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate(TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
            ritorno = "<br/><span style=\"margin-left:100px;padding:2px 10px 2px 10px;font-size:11pt;font-style:italic;color: #fff;background-color:#cc67fe\">" + item.Descrizione + "</span>";
        return ritorno;
    }
    protected string ControlloVuotoPosizione(string comune, string codiceprovincia)
    {
        string ret = "";

        if (!string.IsNullOrWhiteSpace(comune))
            ret += comune;
        if (!string.IsNullOrWhiteSpace(codiceprovincia))
            ret += " (" + NomeProvincia(codiceprovincia) + ") ";

        return ret;
    }
    protected string NomeProvincia(string codiceprovincia)
    {
        string ritorno = "";
        Province item = Utility.ElencoProvince.Find(delegate(Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codiceprovincia); });
        if (item != null)
            ritorno = item.Provincia;
        return ritorno;
    }

    protected void ScriptManagerMaster_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {

        ((ScriptManager)sender).AsyncPostBackErrorMessage = e.Exception.Message.ToString();

        //Argomento di postback o callback non valido. 
        //La convalida degli eventi viene abilitata mediante <pages enableEventValidation="true"/> 
        //nella configurazione oppure mediante <%@ Page EnableEventValidation="true" %> in una pagina.
        //Per motivi di sicurezza, viene verificato che gli argomenti con cui eseguire il postback o 
        //il callback di eventi siano originati dal controllo server che ne aveva inizialmente eseguito 
        //il rendering. Se i dati sono validi e previsti, utilizzare il metodo 
        //ClientScriptManager.RegisterForEventValidation per registrare i dati di postback o callback 
        //per la convalida.

    }




    protected bool VerificaPresenzaPrezzo(object prezzo)
    {
        bool ret = false;
        if (prezzo != null && (double)prezzo != 0)
            ret = true;
        return ret;
    }

#if false

    protected string CreaRigheDettagli(Object itemobj)
    {

        System.Text.StringBuilder sb = new System.Text.StringBuilder();


        if (itemobj != null)
        {
            Offerte item = (Offerte)itemobj;
            if (item.CodiceTipologia != "rif000100") return "";
            Tabrif val = new Tabrif();
            sb.Append("<h3 class=\"h3-body-title-1\">DATI DEL VEICOLO<br/></h3>");

            sb.Append("<div class=\"row\" style=\"border-top:1px solid #ccc\">");
            sb.Append("<div class=\"col-sm-6\">");
            sb.Append("<br/\">");
            sb.Append("<span style=\"font-size: 1.5em; font-weight:600 ;color: #000\">");
            sb.Append("Prezzo:" + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:##,###.00}", item.Prezzo) + " €");
            sb.Append("</span>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class=\"row\">");

            sb.Append("<div class=\"col-sm-6\">");
            Tabrif cf1 = Utility.Caratteristiche[2].Find(c => c.Codice == item.Caratteristica3.ToString() && c.Lingua == Lingua);
            if (cf1 != null)
                sb.Append("<span class=\"h3-body-title-1\">" + Resources.Common.testoTipologiavettura + " </span>" + cf1.Campo1);
            sb.Append("<br/\">");
            sb.Append("<span class=\"h3-body-title-1\">" + Resources.Common.testoImmatricolazione + " </span>" + string.Format("{0:dd/MM/yyyy}", Eval("Data1")));
            sb.Append("<br/\">");
            val = ReadXmlSinglevalue(item.Xmlvalue, "mainData", "cylinderCapacity");
            sb.Append("<span class=\"h3-body-title-1\">" + Resources.Common.testoCilindrata + " </span>" + val.Campo1 + " " + val.Campo2);
            sb.Append("<br/\">");
            Tabrif cf = Utility.Caratteristiche[3].Find(c => c.Codice == item.Caratteristica4.ToString() && c.Lingua == Lingua);
            if (cf != null)
                sb.Append("<span class=\"h3-body-title-1\">" + Resources.Common.testoAlimentazione + " </span>" + cf.Campo1);

            sb.Append("<br/\">");
            val = ReadXmlSinglevalue(item.Xmlvalue, "mainData", "transmission");
            sb.Append("<span class=\"h3-body-title-1\">" + Resources.Common.testoTrasmissione + " </span>" + val.Campo1);

            sb.Append("</div>");
            sb.Append("<div class=\"col-sm-6\">");
            sb.Append("<span class=\"h3-body-title-1 \">Codice </span>" + item.CodiceProdotto);
            sb.Append("<br/\">"); val = CommonPage.ReadXmlSinglevalue(item.Xmlvalue, "mainData", "mileage");
            sb.Append("<span class=\"h3-body-title-1 \">" + Resources.Common.testoChilometraggio + " </span>" + val.Campo1 + " " + val.Campo2);
            sb.Append("<br/\">");
            val = ReadXmlSinglevalue(item.Xmlvalue, "mainData", "enginePower");
            sb.Append("<span class=\"h3-body-title-1\">" + Resources.Common.testoPotenza + " </span>" + val.Campo1 + " " + val.Campo2);
            sb.Append("<br/\">");

            List<Tabrif> colordata = ReadColorvalue(item.Xmlvalue);
            string tmp = "";
            if (colordata != null && colordata.Count > 0)
                tmp = colordata[0].Campo1;
            if (colordata != null && colordata.Count > 1)
                tmp += " " + colordata[1].Campo1;
            sb.Append("<span class=\"h3-body-title-1\">" + Resources.Common.testoColore + " </span>" + tmp);

            sb.Append("<br/\">");
            val = ReadXmlSinglevalue(item.Xmlvalue, "mainData", "bodyStyle");
            sb.Append("<span class=\"h3-body-title-1\">" + Resources.Common.testoCategoriaveicolo + " </span>" + val.Campo1);

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div class=\"row\" style=\"border-bottom:1px solid #ccc\">");
            sb.Append("<div class=\"col-sm-12\">");
            sb.Append("<br/\">");
            sb.Append("</div>");
            sb.Append("</div>");



        }


        return sb.ToString();
    }

    protected string CreaRigheOpzioni(Object itemobj)
    {
        // bool dasweltauto = false;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (itemobj != null)
        {
            Offerte item = (Offerte)itemobj;
            if (item.CodiceTipologia != "rif000100") return "";
            Tabrif val = new Tabrif();
            //Metodo per lettura delle options ( id/description -> value ( per value dovresti cercare la corrispondenza nella tabella riferimento options )
            List<Tabrif> options = ReadOptionsValues(item.Xmlvalue);
            if (options == null || options.Count == 0) return "";
            sb.Append("<h3 class=\"h3-body-title-1\">Equipaggiamenti e caratteristiche<br/></h3>");

            sb.Append("<div class=\"row\" style=\"border-top:1px solid #ccc;font-size:0.7em\">");
            foreach (Tabrif op in options)
            {

                //Dalle opzioni memorizzate nel record del db
                //op.Campo2 //id opzione
                //  op.Campo1 // Valore assunto
                //FAccio la lookup con la tabella caratteristiche5 in memoria  ( che contiene la tabella di riferimento delle opzioni )
                Tabrif oplook = Utility.Caratteristiche[4].Find(c => c.Codice == op.Campo2 && c.Lingua == Lingua);
                if (oplook != null)
                {

                    if (oplook.Campo1.Contains(","))
                    {
                        string[] rangevalues = oplook.Campo1.Split(',');
                        foreach (string s in rangevalues)
                        {
                            string[] kv = s.Split('=');
                            if (kv != null && kv.Length == 2)
                            {
                                if (kv[0].Trim() == op.Campo1)
                                {
                                    sb.Append("<div class=\"col-sm-6\">");

                                    sb.Append("<span style=\"font-weight:600\">");
                                    sb.Append(oplook.Campo3); //Descrizione dell'opzione dalla tabella lookup
                                    sb.Append(": ");
                                    sb.Append("</span>");
                                    sb.Append(kv[1].Trim().ToLower()); //Descrizione del Valore dell'opzione dalla tabella lookup
                                    sb.Append(" ");
                                    sb.Append(oplook.Campo5); //Unità di misura dell'opzione dalla tabella lookup
                                    sb.Append("</div>");

                                    //oplook.Campo4 è il raggruppanto delle caratteristiche


                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!oplook.Campo3.ToLower().Trim().Contains("descrizione testuale")) //escludo la visualizzazione della descrizione testuale html del veicolo
                        {

                            //Tipo di legno interno -> a,b,c,d sono le opzioni importanti da visualizzare da  op.Campo1.ToLower().Trim() -> accenzione dei vari loghi in alto


                            sb.Append("<div class=\"col-sm-6\">");
                            sb.Append("<span style=\"font-weight:600\">");
                            sb.Append(oplook.Campo3); //Descrizione dell'opzione dalla tabella lookup
                            sb.Append(": ");
                            sb.Append("</span>");
                            string modificavaloretesto = op.Campo1.ToLower().Trim();
                            if ((op.Campo1.ToLower().Trim() == "y")) modificavaloretesto = "sì";

                            sb.Append(modificavaloretesto);  //Descrizione del Valore dell'opzione dalla tabella lookup
                            sb.Append(" ");
                            sb.Append(oplook.Campo5); //Unità di misura dell'opzione dalla tabella lookup
                            sb.Append("</div>");

                            //Opzione 439 -programma garanzia - 3: das welt auto
                            //  if (op.Campo1.ToLower().Trim() == "3") dasweltauto = true;// valore che indica se il veicolo ha garanzia daswelt auto

                            //oplook.Campo4 è il raggruppanto delle caratteristiche
                        }
                    }

                }
            }
            sb.Append("</div>");

        }
        return sb.ToString();
    }

#endif

}