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

               DataBind();

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



}