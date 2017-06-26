using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;

public partial class AreaRiservata_Forumdetail : CommonPage
{
    public string PageGuid
    {
        get { return ViewState["PageGuid"] != null ? (string)(ViewState["PageGuid"]) : ""; }
        set { ViewState["PageGuid"] = value; }
    }
    public string Pagina
    {
        get { return Session["Pagina"] != null ? (string)(Session["Pagina"]) : "1"; }
        set { Session["Pagina"] = value; }
    }
    public DataTable dt
    {
        get { return ViewState["DataTable"] != null ? (DataTable)(ViewState["DataTable"]) : new DataTable(); }
        set { ViewState["DataTable"] = value; }
    }
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string ContenutoPagina
    {
        get { return ViewState["ContenutoPagina"] != null ? (string)(ViewState["ContenutoPagina"]) : ""; }
        set { ViewState["ContenutoPagina"] = value; }
    }
    public string idOfferta
    {
        get { return ViewState["idOfferta"] != null ? (string)(ViewState["idOfferta"]) : ""; }
        set { ViewState["idOfferta"] = value; }
    }
    public string testoricerca
    {
        get { return ViewState["testoricerca"] != null ? (string)(ViewState["testoricerca"]) : ""; }
        set { ViewState["testoricerca"] = value; }
    }
    public string Tipologia
    {
        get { return ViewState["Tipologia"] != null ? (string)(ViewState["Tipologia"]) : ""; }
        set { ViewState["Tipologia"] = value; }
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
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;
                ((HtmlTitle)Master.FindControl("metaTitle")).Text = WelcomeLibrary.UF.Utility.SostituisciTestoACapo("Forum " + " " + Nome);
                ((HtmlMeta)Master.FindControl("metaDesc")).Content = "";
       
                //Prendiamo i dati dalla querystring
                Tipologia = CaricaValoreMaster(Request, Session, "Tipologia", false);
                string tmp = "";
                tmp = CaricaValoreMaster(Request, Session, "testoricerca", false);
                if (!string.IsNullOrEmpty(tmp)) testoricerca = tmp;
                Lingua = CaricaValoreMaster(Request, Session, "Lingua",false,"I");
                ContenutoPagina = CaricaValoreMaster(Request, Session, "ContenutoPagina");
                idOfferta = CaricaValoreMaster(Request, Session, "idOfferta", true, "");
                string testoindice = CaricaValoreMaster(Request, Session, "testoindice", true, "");
 
               InizializzaTestiPagina();
                AssociaDati();

            }
            else
            {

            }

        }
        catch (Exception err)
        {
            //   output.Text = err.Message;
        }

    }
    private void InizializzaTestiPagina()
    {
        WelcomeLibrary.DOM.TipologiaOfferte sezione = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate(WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I" & tmp.Codice == Tipologia); });
        WelcomeLibrary.DOM.TipologiaOfferte sezione_inlingua = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate(WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua & tmp.Codice == Tipologia); });
        //if (offerte != null && offerte.Count > 0)
        //    AssociaDatiSocial(offerte[0]);
        if (sezione != null)
        {
            EvidenziaSelezione(sezione.Descrizione);

            string titolopagina = sezione_inlingua.Descrizione.ToUpper();
            //Prodotto catselected = Utility.ElencoProdotti.Find(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == Tipologia && tmp.CodiceProdotto == categoria)); });
            //if (catselected != null)
            //    titolopagina = catselected.Descrizione;
            litNomePagina.Text = titolopagina;

            string htmlPage = "";
            if (references.ResMan("Common",Lingua,"testo" + Tipologia) != null)
                htmlPage = references.ResMan("Common",Lingua,"testo" + Tipologia).ToString();
            litTextHeadPage.Text = ReplaceAbsoluteLinks(htmlPage);
        }
    }
    protected void EvidenziaSelezione(string testolink)
    {
        HtmlAnchor linkmenu = null;
        try
        {
            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#999999");
            }
        }
        catch { }
        try
        {
            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink + "high"));
            if (linkmenu != null)
            {
                ((HtmlGenericControl)linkmenu.Parent).Attributes.Add("class", "active");
                ((HtmlGenericControl)linkmenu.Parent.Parent).Attributes["class"] += " active";
            }
        }
        catch { }
    }
    private void AssociaDati()
    {
        //Associamo la lista degli argomenti di discussione
        List<Offerte> list = new List<Offerte>();


        AssociaDatiRepeater(list);
    }
    protected void AssociaDatiRepeater(List<Offerte> list)
    {



    }
     
}
