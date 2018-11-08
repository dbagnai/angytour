using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AreaContenuti_MasterPage : System.Web.UI.MasterPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : "I"; }
        set { ViewState["Lingua"] = value; }
    }

    // dice samuele : ma che ci fai con questa sotto???
    CommonPage CommonPage = new CommonPage();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {


                if (Request.QueryString["Errore"] != null && Request.QueryString["Errore"] != "")
                { output.Text = Request.QueryString["Errore"].ToString(); }



                //Inizializzo i valori

                List<WelcomeLibrary.DOM.TipologiaOfferte> Tipologie = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => t.Lingua == "I" && (Convert.ToInt32(t.Codice.Substring(3)) >= 2) && (Convert.ToInt32(t.Codice.Substring(3)) <= 11));
                Tipologie.AddRange(WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => t.Lingua == "I" && Convert.ToInt32(t.Codice.Substring(3)) == 199));
                Tipologie.RemoveAll(delegate (WelcomeLibrary.DOM.TipologiaOfferte _t) { return _t.Codice == "rif000100"; });
                rptTipologia.DataSource = Tipologie;
                rptTipologia.DataBind();

                //List<WelcomeLibrary.DOM.Prodotto> listcat = WelcomeLibrary.UF.Utility.ElencoProdotti.FindAll(p => p.CodiceTipologia == "rif000002" && p.Lingua == "I");
                //rptRubricheCategorie.DataSource = listcat;
                //rptRubricheCategorie.DataBind();


                List<WelcomeLibrary.DOM.TipologiaOfferte> Catalogo = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => t.Lingua == "I" && Convert.ToInt32(t.Codice.Substring(3)) >= 1 && Convert.ToInt32(t.Codice.Substring(3)) <= 1);
                rptCatalogo.DataSource = Catalogo;
                rptCatalogo.DataBind();


                List<WelcomeLibrary.DOM.Prodotto> listcat = WelcomeLibrary.UF.Utility.ElencoProdotti.FindAll(p => p.CodiceTipologia == "rif000001" && p.Lingua == "I");
                rptCatalogoCategorie.DataSource = listcat;
                rptCatalogoCategorie.DataBind();


                ////////////////////////////////////////////////////////////////////////////////////////
                List<WelcomeLibrary.DOM.TipologiaOfferte> Tipologiepwa = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => t.Lingua == "I" && Convert.ToInt32(t.Codice.Substring(3)) >= 500 && Convert.ToInt32(t.Codice.Substring(3)) <= 600);
                rptPwa.DataSource = Tipologiepwa;
                rptPwa.DataBind();
                List<WelcomeLibrary.DOM.Prodotto> SubTipologiepwa = WelcomeLibrary.UF.Utility.ElencoProdotti.FindAll(p => p.CodiceTipologia == "rif000500" && p.Lingua == "I");
                rptPwaCategorie.DataSource = SubTipologiepwa;
                rptPwaCategorie.DataBind();
                ////////////////////////////////////////////////////////////////////////////////////////


                //Custom tipo
                //List<WelcomeLibrary.DOM.TipologiaOfferte> list = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => t.Lingua == "I" && Convert.ToInt32(t.Codice.Substring(3)) >= 61 && Convert.ToInt32(t.Codice.Substring(3)) <= 62);
                //rptCustom.DataSource = list;
                //rptCustom.DataBind();


                //Rassegna
                //List<WelcomeLibrary.DOM.TipologiaOfferte> list = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => t.Lingua == "I" && Convert.ToInt32(t.Codice.Substring(3)) >= 51 && Convert.ToInt32(t.Codice.Substring(3)) <= 51);
                //rptCustom.DataSource = list;
                //rptCustom.DataBind();


                //Commenti
                //List<WelcomeLibrary.DOM.TipologiaOfferte> list = WelcomeLibrary.UF.Utility.TipologieOfferte.FindAll(t => t.Lingua == "I" && Convert.ToInt32(t.Codice.Substring(3)) >= 101 && Convert.ToInt32(t.Codice.Substring(3)) <= 101);
                //rptCustom.DataSource = list;
                //rptCustom.DataBind();

                //Controllo contenuti pagine statiche
                WelcomeLibrary.DOM.TipologiaContenuti paginestatiche = WelcomeLibrary.UF.Utility.TipologieContenuti.Find(delegate (WelcomeLibrary.DOM.TipologiaContenuti tmp) { return (tmp.Lingua == "I" && tmp.Codice == "con001000"); });
                linkPaginestatiche.HRef = "GestioneContenuti.aspx?CodiceContenuto=" + paginestatiche.Codice;
                Titolopaginestatiche.Text = paginestatiche.Descrizione;


                WelcomeLibrary.DOM.TipologiaContenuti paginestatichepwa = WelcomeLibrary.UF.Utility.TipologieContenuti.Find(delegate (WelcomeLibrary.DOM.TipologiaContenuti tmp) { return (tmp.Lingua == "I" && tmp.Codice == "con001001"); });
                linkPaginestatichepwa.HRef = "GestioneContenuti.aspx?CodiceContenuto=" + paginestatichepwa.Codice;
                Titolopaginestatichepwa.Text = paginestatichepwa.Descrizione;


                //attivo l'hover del menu
                if (Request.FilePath.ToLower().Trim().Contains("gestioneofferte") || Request.FilePath.ToLower().Trim().Contains("gestionerodotti"))
                {
                    tagOfferteProdotti.Attributes.Add("class", "active open hover");
                    lblTitleSection.Text = "GESTIONE SEZIONE CONTENUTI";
                }
                else if (Request.FilePath.ToLower().Trim().Contains("gestionecontenuti"))
                {
                    tagPagineStatiche.Attributes.Add("class", "active open hover");
                    lblTitleSection.Text = "GESTIONE PAGINE STATICHE";
                }
                else if (Request.FilePath.ToLower().Trim().Contains("gestionebanners"))
                {
                    tagBanner.Attributes.Add("class", "active open hover");
                    lblTitleSection.Text = "GESTIONE GALLERIA E BANNER";
                }
                else if (Request.FilePath.ToLower().Trim().Contains("gestioneclienti"))
                {
                    tagContatti.Attributes.Add("class", "active open hover");
                    lblTitleSection.Text = "GESTIONE CLIENTI";
                }
                else if (Request.FilePath.ToLower().Trim().Contains("gestionenewsletter"))
                {
                    tagContatti.Attributes.Add("class", "active open hover");
                    lblTitleSection.Text = "GESTIONE NEWSLETTER";
                }
                else if (Request.FilePath.ToLower().Trim().Contains("storicoordini"))
                {
                    tagContatti.Attributes.Add("class", "active open hover");
                    lblTitleSection.Text = "STORICO ORDINI";
                }
                else if (Request.FilePath.ToLower().Trim().Contains("configpage") || Request.FilePath.ToLower().Trim().Contains("resourcepage"))
                {
                    tagConfig.Attributes.Add("class", "active open hover");
                    lblTitleSection.Text = "GESTIONE CONFIG";
                }
                else
                {
                    tagDefault.Attributes.Add("class", "active open hover");
                    lblTitleSection.Text = "DASHBOARD";
                }


            }

            litTrial.Text = WelcomeLibrary.STATIC.Global.TestTrial();

        }
        catch (Exception errore)
        {

        }
    }

    public string InjectedEndPageScripts()
    {
        Dictionary<string, string> addelements = new Dictionary<string, string>();
        LoadJavascriptVariables(addelements);
        string ret = WelcomeLibrary.UF.custombind.CreaInitStringJavascript(addelements);
        return ret;
    }
    private void LoadJavascriptVariables(Dictionary<string, string> addelements = null)
    {
        //However, if you want your JavaScript code to be independently escaped for any context, you could opt for the native JavaScript encoding:
        //' becomes \x27
        //" becomes \x22

        String scriptRegVariables = "";
        scriptRegVariables += string.Format("var lng = '{0}'", Lingua);
        scriptRegVariables += "; " + string.Format("var pathAbs = '{0}'", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
        //Passo codificate base64 con encoding utf-8 le risorse necessarie al javascript della pagina iniettandole in pagina (   questo evita di attendere la promise per inizializzare le variabili javascript !!! )
        //scriptRegVariables += ";\r\n" + string.Format("loadvariables(utf8ArrayToStr(urlB64ToUint8Array('{0}')))", dataManagement.EncodeUtfToBase64(references.initreferencesdataserialized(Lingua, Page.User.Identity.Name)));
        scriptRegVariables += ";\r\n" + string.Format("loadvariables(b64ToUtf8('{0}'))", WelcomeLibrary.UF.dataManagement.EncodeUtfToBase64(references.initreferencesdataserialized(Lingua, Page.User.Identity.Name)));


        scriptRegVariables += ";\r\n";

        if (addelements == null) addelements = new Dictionary<string, string>();
        addelements.Add("jsvarfrommaster", scriptRegVariables);

        //ClientScriptManager cs = Page.ClientScript;
        //if (!cs.IsClientScriptBlockRegistered("RegVariablesScript"))
        //{
        //    cs.RegisterClientScriptBlock(typeof(Page), "RegVariablesScript", scriptRegVariables, true);
        //}
    }

    private void ImpostaVisualizzazione()
    {
        //string idcliente = CommonPage.getidcliente(Page.User.Identity.Name);
        //if (!string.IsNullOrEmpty(idcliente))
        //{
        //    id_commerciale = idcliente;
        //}
        //else
        //{
        Response.Redirect("~/Error.aspx?Error=Utente non permesso");
        //}

    }
}
