using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;

public partial class AspNetPages_Shoppingcart : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : "I"; }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;

                //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "vuoto", false, Lingua);
             
               
                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua");

                RiempiDdlNazione("IT", ddlNazione);
                CaricaCarrello();

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

    private void CaricaCarrello()
    {
        eCommerceDM ecmDM = new eCommerceDM();
        //////////////////////////////////////////////////////////////////////////////
        //Prendiamo l'ip del client
        /////////////////////////////////////////////////////////////////////////////
        string trueIP = "";
        string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (!string.IsNullOrEmpty(ip))
        {
            string[] ipRange = ip.Split(',');
            trueIP = ipRange[0].Trim();
        }
        else
        {
            trueIP = Request.ServerVariables["REMOTE_ADDR"].Trim();
        }
        CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Session.SessionID, trueIP);
        string codicenazione = SelezionaNazione(carrello);
        VisualizzaTotaliCarrello(codicenazione, "");

        //aggiorno il carrello in quanto se erano presenti articoli esauriti vengono rimossi
        carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Session.SessionID, trueIP);

        rptProdotti.DataSource = carrello;
        rptProdotti.DataBind();
    }
    private string SelezionaNazione(CarrelloCollection carrello)
    {
        string codicenazione = "";
        if (carrello != null)
        {
            Carrello c = carrello.Find(_c => !string.IsNullOrWhiteSpace(_c.Codicenazione));
            if (c != null)
                codicenazione = c.Codicenazione;
        }
        try
        {
            ddlNazione.SelectedValue = codicenazione;
        }
        catch
        { }
        return codicenazione;
    }
    private void VisualizzaTotaliCarrello(string codicenazione, string codiceprovincia)
    {
        TotaliCarrello totali = CalcolaTotaliCarrello(Request, Session, codicenazione, codiceprovincia);
        List<TotaliCarrello> list = new List<TotaliCarrello>();
        list.Add(totali);
        rptTotali.DataSource = list;
        rptTotali.DataBind();
        this.Master.VisualizzaTotaliCarrello();

    }

    protected void lnkUpdateCart_Click(object sender, EventArgs e)
    {
        VisualizzaTotaliCarrello(ddlNazione.SelectedValue, "");
    }



    protected bool VerificaPresenzaPrezzo(object prezzo)
    {
        bool ret = false;
        if (prezzo != null && (double)prezzo != 0)
            ret = true;
        return ret;
    }

    protected string TestoSezione(string codicetipologia)
    {
        string ret = "";
        WelcomeLibrary.DOM.TipologiaOfferte sezione =
              WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codicetipologia); });
        if (sezione != null)
        {
            ret += " " + GetGlobalResourceObject("Common", "testoSezione").ToString() + " \"" + CommonPage.ReplaceAbsoluteLinks(CrealinkElencotipologia(codicetipologia, Lingua, Session)) + "\"";
        }
        return ret;
    }
    protected string TotaleArticolo(object Numero, object Prezzo)
    {
        string ret = "";
        double n = 0;
        double p = 0;
        double.TryParse(Numero.ToString(), out n);
        double.TryParse(Prezzo.ToString(), out p);
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("it-IT");
        ret = string.Format("{0:N2}", p * n, ci);
        return ret;

    }
    protected void btnDelete(object sender, EventArgs e)
    {
        HtmlInputText txtQuantita_temp = (HtmlInputText)((LinkButton)sender).FindControl("txtQuantita");
        string Idtext = ((LinkButton)sender).CommandArgument.ToString();
        int idprodotto = 0;
        int.TryParse(Idtext, out idprodotto);
        int quantita = 0;
        //int.TryParse(txtQuantita_temp.Value, out quantita);

        AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name);
        if (Session["superamentoquantita"] != null)
        {
            int qtamod = 0;
            int.TryParse(Session["superamentoquantita"].ToString(), out qtamod);
            Session.Remove("superamentoquantita");
            quantita = qtamod;
            output.Text = references.ResMan("Common",Lingua,"testoCarellosuperamentoquantita");
        }
        txtQuantita_temp.Value = "0";
        //AggiornaVisualizzazioneDatiCarrello();
        CaricaCarrello();

    }
    protected void btnDecrement(object sender, EventArgs e)
    {
        HtmlInputText txtQuantita_temp = (HtmlInputText)((LinkButton)sender).FindControl("txtQuantita");
        string Idtext = ((LinkButton)sender).CommandArgument.ToString();
        int idprodotto = 0;
        int.TryParse(Idtext, out idprodotto);
        int quantita = 0;
        int.TryParse(txtQuantita_temp.Value, out quantita);
        quantita -= 1;//Decremento
        if (quantita < 1) quantita = 0;
        AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name);
        if (Session["superamentoquantita"] != null)
        {
            int qtamod = 0;
            int.TryParse(Session["superamentoquantita"].ToString(), out qtamod);
            Session.Remove("superamentoquantita");
            quantita = qtamod;
            output.Text = references.ResMan("Common",Lingua, "testoCarellosuperamentoquantita");
        }
        txtQuantita_temp.Value = quantita.ToString();

        //QUI DEVI FARE L'AGGIORNAMENTO DEI RIEPILOGHI DEL CARRELLO NELLA MASTER!!!!->
        //  AggiornaVisualizzazioneDatiCarrello();
        CaricaCarrello();

    }
   
    protected void btnIncrement(object sender, EventArgs e)
    {

        //if (rptOfferta.Items != null && rptOfferta.Items.Count != 0)
        //{
        //    foreach (System.Web.UI.WebControls.RepeaterItem item in rptOfferta.Items)
        //    {
        //        string quant = ((HtmlInputText)(item.FindControl("txtQuantita"))).Value;
        //    }
        //}
        HtmlInputText txtQuantita_temp = (HtmlInputText)((LinkButton)sender).FindControl("txtQuantita");
        string Idtext = ((LinkButton)sender).CommandArgument.ToString();
        int idprodotto = 0;
        int.TryParse(Idtext, out idprodotto);
        int quantita = 0;
        int.TryParse(txtQuantita_temp.Value, out quantita);

        quantita += 1;//Incremento
        AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name);
        if (Session["superamentoquantita"] != null)
        {
            int qtamod = 0;
            int.TryParse(Session["superamentoquantita"].ToString(), out qtamod);
            Session.Remove("superamentoquantita");
            quantita = qtamod;
            output.Text = references.ResMan("Common",Lingua, "testoCarellosuperamentoquantita");
        }
        txtQuantita_temp.Value = quantita.ToString();

        //QUI DEVI FARE L'AGGIORNAMENTO DEI RIEPILOGHI DEL CARRELLO NELLA MASTER!!!!->
        // AggiornaVisualizzazioneDatiCarrello();
        CaricaCarrello();

    }

    private void RiempiDdlNazione(string valore, DropDownList ddlNazione)
    {
        List<Tabrif> nazioni = WelcomeLibrary.UF.Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == Lingua; });
        nazioni.Sort(new GenericComparer<Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));
        ddlNazione.Items.Clear();
        foreach (Tabrif n in nazioni)
        {
            ListItem i = new ListItem(n.Campo1, n.Codice);
            ddlNazione.Items.Add(i);
        }
        try
        {
            ddlNazione.SelectedValue = valore.ToUpper();
        }
        catch { valore = "IT"; ddlNazione.SelectedValue = valore.ToUpper(); }
    }

}