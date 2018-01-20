using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Data.SQLite;

public partial class AspNetPages_Mappa : CommonPage
{
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

                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua");
                ContenutoPagina = CaricaValoreMaster(Request, Session, "ContenutoPagina");

                InzializzaTestoPagina();
               //InizializzaMeta();

               //Carico la galleria in masterpage corretta
               //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);

               DataBind();
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

    protected void InzializzaTestoPagina()
    {
        string tmp = references.ResMan("Common",Lingua,"Testo" + ContenutoPagina).ToString();
        try
        {
            tmp = tmp.Substring(0, tmp.IndexOf("</font>")) + tmp.Substring(tmp.IndexOf("</font>")).ToLower();
        }
        catch { }
        litNomeContenuti.Text = tmp;

        //litMainContent.Text = GetLocalResourceObject("Testo" + ContenutoPagina).ToString();

        //string[] testi = ContentCaller(Lingua, TipoContenuto);
        //litMainContent.Text = testi[1];
        //litNomeContenuti.Text = testi[0];

        //if (testi[2] != "")
        //{
        //    imgBottom.Src = testi[2];
        //    imgBottom.Visible = true;
        //    if (testi[3] != "")
        //        linkBanner.HRef = testi[3];
        //}
        CreaLinkArticoli();
    }

    private void CreaLinkArticoli()
    {
        //litMainContent.Text;
        List<SQLiteParameter> parColl = new List<SQLiteParameter>();
        parColl = new List<SQLiteParameter>();
        offerteDM offDM = new offerteDM();
        WelcomeLibrary.DOM.OfferteCollection lista = new WelcomeLibrary.DOM.OfferteCollection();

        //Carichiamo la lista contenuto presenti 
        try
        {
            //Carichiamo in memoria tutti le ultime 1000 news
            lista = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "1000", Lingua);
        }
        catch (Exception error)
        {
            litMainContent.Text = " &nbsp; <br/> Errore caricamento news per feed rss: " + error.Message + " \r\n";
        }
        HtmlAnchor a;
        foreach (Offerte _new in lista)
        {
            if (_new == null || string.IsNullOrEmpty(_new.Id.ToString())) continue;
            string testoperindice = _new.DenominazionebyLingua(Lingua);
            
            //LINK A SCHEDA
            string UrlCompleto = "";
            UrlCompleto =
                CreaLinkRoutes(null, false, Lingua, testoperindice, _new.Id.ToString(), _new.CodiceTipologia,"","","");
           
            a = new HtmlAnchor();
            a.HRef = UrlCompleto;
            a.InnerHtml = testoperindice + "<br/>";
            a.Target = "_blank";
            divIndex.Controls.Add(a);
        }


    }


}
