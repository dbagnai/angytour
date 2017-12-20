using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Data.SQLite;
using System.Text;
using WelcomeLibrary.UF;

public partial class AreaContenuti_StoricoOrdini_New : CommonPage
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
    public string id_cliente
    {
        get { return Session["id_cliente"] != null ? (string)(Session["id_cliente"]) : ""; }
        set { Session["id_cliente"] = value; }
    }
    public string id_commerciale
    {
        get { return Session["id_commerciale"] != null ? (string)(Session["id_commerciale"]) : ""; }
        set { Session["id_commerciale"] = value; }
    }
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : CommonPage.deflanguage; }
        set { ViewState["Lingua"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            AutoCompleteExtender1.ContextKey = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            AutoCompleteExtender2.ContextKey = WelcomeLibrary.STATIC.Global.NomeConnessioneDb;
            PageGuid = System.Guid.NewGuid().ToString();
            id_cliente = CaricaValoreMaster(Request, Session, "id_cliente", true, "");
            id_commerciale = CaricaValoreMaster(Request, Session, "id_commerciale", true, "");
            CommonPage CommonPage = new CommonPage();
            Lingua = CommonPage.CaricaValoreMaster(Request, Session, "Lingua", false, "I");

            /////////////////////////////////////////////////////////////////////
            //Verifichiamo accesso socio e impostiamo la visualizzazione corretta
            //Spegnendo le cose che non devono essere visibili ai soci!!!
            /////////////////////////////////////////////////////////////////////
            usermanager USM = new usermanager();
            if (USM.ControllaRuolo(User.Identity.Name, "Commerciale"))
                ImpostaVisualizzazione();


            CaricaOrdini();
        }
    }
    private void ImpostaVisualizzazione()
    {
        string idcliente = getidcliente(User.Identity.Name);
        if (!string.IsNullOrEmpty(idcliente))
        {
            id_commerciale = idcliente;
            txtCommerciale.Text = idcliente;
            txtCommerciale.Enabled = false;
            ((HtmlGenericControl)Master.FindControl("ulMainbar")).Visible = false; //Spengo la barra navigazione

        }
        else
        {
            Response.Redirect("~/Error.aspx?Error=Utente non trovato");
        }

    }

    protected string TipopagaDisplay(object item)
    {
        if (item == null) return "";
        TotaliCarrello i = (TotaliCarrello)item;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(references.ResMan("Common", Lingua, "txt" + i.Modalitapagamento).ToString());
        return sb.ToString();
    }

    protected bool StatusCheck(object item)
    {
        if (item == null) return false;
        TotaliCarrello i = (TotaliCarrello)item;
        return i.Pagato;
    }
    protected string StatusDisplay(object item)
    {
        if (item == null) return "";
        TotaliCarrello i = (TotaliCarrello)item;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<span class=\"");
        if (!i.Pagato)
            sb.Append("class=\"text-success\"");
        else
            sb.Append("class=\"text-warning\"");
        //sb.Append("class=\"text-error\"");

        sb.Append("\">");

        if (!i.Pagato)
            sb.Append(references.ResMan("Common", Lingua, "txtOrdinenonpagato").ToString());
        else
            sb.Append(references.ResMan("Common", Lingua, "txtOrdinepagato").ToString());
        //    txtOrdineannullato

        sb.Append("</span>");

        return sb.ToString();
    }

    [System.Web.Services.WebMethod]
    public static string GetCurrentCarrello(string codice)
    {
        return VisualizzaCarrello(codice);
    }
    public static string VisualizzaCarrello(string codiceordine, bool nofoto = false)
    {
        StringBuilder sb = new StringBuilder();
        //sb.Append(codiceordine);
        eCommerceDM ecmDM = new eCommerceDM();
        CarrelloCollection carrellolist = ecmDM.CaricaCarrelloPerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);

        foreach (Carrello c in carrellolist)
        {

            //Creiamo la visualizzione degli articoli in carrello
            // da fare <li>  contenuto da prendere sotto  </li>
            sb.Append("<li style=\"padding-top:2px;padding-right:5px;margin-top:10px; border-bottom:1px solid #ddd;\">");

            if (!nofoto)
            {
                //CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString())
                sb.Append("<a target=\"_blank\"   href=\"" +
                    CommonPage.ReplaceAbsoluteLinks(CommonPage.CreaLinkRoutes(null, false, "I", CommonPage.CleanUrl(c.Offerta.DenominazioneI), c.Offerta.Id.ToString(), c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, ""))
                       + "\"  class=\"product-thumb pull-left\"  >");
                sb.Append("<img alt=\""
                    +
                    CommonPage.CleanInput(CommonPage.ConteggioCaratteri(c.Offerta.DenominazioneI, 300, true))
                    + "\" Style=\"width: auto; height: auto; max-width: 60px; max-height: 60px;\" ");
                sb.Append(" src=\"");
                sb.Append(CommonPage.ReplaceAbsoluteLinks(CommonPage.ComponiUrlAnteprima(c.Offerta.FotoCollection_M.FotoAnteprima, c.Offerta.CodiceTipologia, c.Offerta.Id.ToString())) + "\" ");
                sb.Append("\" />");
                sb.Append(" </a>");
            }

            sb.Append(" <div class=\"product-details\">");
            sb.Append(" <span class=\"product-name\">");
            sb.Append(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(c.Offerta.DenominazioneI));
            sb.Append(" </span>");


            #region MODIFIED CARATTERISTICHE CARRELLO
            if (!string.IsNullOrEmpty(c.Offerta.Xmlvalue))
            {
                sb.Append(" <div class=\"product-categories muted\">");
                //recupero le caratteristiche del prodotto
                List<ModelCarCombinate> listCar = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(c.Offerta.Xmlvalue);
                ModelCarCombinate item = listCar.Find(e => e.id == c.Campo2);
                if (item != null)
                    sb.Append(item.caratteristica1.value + "  -  " + item.caratteristica2.value);
                sb.Append(" </div>");
            }
            #endregion

            //sb.Append(" <div class=\"product-categories muted\">");
            //sb.Append(CommonPage.TestoCategoria(c.Offerta.CodiceTipologia, c.Offerta.CodiceCategoria, Lingua));
            //sb.Append(" </div>");
            //sb.Append(" <div class=\"product-categories muted\">");
            //sb.Append(CommonPage.TestoCaratteristica(0, c.Offerta.Caratteristica1.ToString(), Lingua));
            //sb.Append(" </div>");
            //sb.Append(" <div class=\"product-categories muted\">");
            //sb.Append(CommonPage.TestoCaratteristica(1, c.Offerta.Caratteristica2.ToString(), Lingua));
            //sb.Append(" </div>");
            //sb.Append(" <div class=\"product-categories muted\">");
            //sb.Append(TestoSezione(c.Offerta.CodiceTipologia));
            //sb.Append(" </div>");


            sb.Append(" <p class=\"product-calc muted\">");
            sb.Append(c.Numero + "&times;" + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { c.Prezzo }) + " €");
            sb.Append(" </p>");

            sb.Append(" </div>");
            sb.Append(" </li>");

        }
        return sb.ToString();
    }
    protected static string TestoSezione(string codicetipologia)
    {
        string ret = "";
        WelcomeLibrary.DOM.TipologiaOfferte sezione =
              WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I" && tmp.Codice == codicetipologia); });
        if (sezione != null)
        {
            ret += " nella sezione:  \"" + CommonPage.ReplaceAbsoluteLinks(CommonPage.CrealinkElencotipologia(codicetipologia, "I")) + "\"";
        }

        return ret;

    }


    protected TotaliCarrelloCollection CaricaDatiOrdini(string id_cliente = "", string codiceordine = "", string datamin = "", string datamax = "", string idcommerciale = "")
    {
        eCommerceDM eDM = new eCommerceDM();
        TotaliCarrelloCollection ordini = new TotaliCarrelloCollection();
        List<SQLiteParameter> parcoll = new List<SQLiteParameter>();
        if (!string.IsNullOrWhiteSpace(id_cliente))
        {
            SQLiteParameter parid = new SQLiteParameter("@Id_cliente", id_cliente);
            parcoll.Add(parid);
        }
        if (!string.IsNullOrWhiteSpace(idcommerciale))
        {
            SQLiteParameter parid1 = new SQLiteParameter("@Id_commerciale", idcommerciale);
            parcoll.Add(parid1);
        }
        if (!string.IsNullOrEmpty(codiceordine))
        {
            SQLiteParameter parcod = new SQLiteParameter("@Codiceordine", codiceordine);
            parcoll.Add(parcod);
        }
        if (!string.IsNullOrEmpty(datamin))
        {
            DateTime _dt;
            if (DateTime.TryParse(datamin, out _dt))
            {
                SQLiteParameter pardmin = new SQLiteParameter("@DataMin", dbDataAccess.CorrectDatenow(_dt));
                parcoll.Add(pardmin);
            }
        }
        if (!string.IsNullOrEmpty(datamax))
        {
            DateTime _dt;
            if (DateTime.TryParse(datamax, out _dt))
            {
                SQLiteParameter pardmax = new SQLiteParameter("@DataMax", dbDataAccess.CorrectDatenow(_dt));
                parcoll.Add(pardmax);
            }
        }

        ordini = eDM.CaricaListaOrdini(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parcoll, "");

        return ordini;
    }

    protected void CaricaOrdini()
    {
        string idforced = id_commerciale;
        if (string.IsNullOrEmpty(idforced))
            idforced = txtCommerciale.Text;

        TotaliCarrelloCollection ordini = CaricaDatiOrdini(txtCLIENTE.Text, txtCodiceordine.Text, txtdatamin.Text, txtdatamax.Text, idforced);
#if true
        //Selezionamo i risultati in base al numero di pagina e alla sua dimensione per la paginazione
        //Utilizzando la classe di paginazione

        WelcomeLibrary.UF.Pager<TotaliCarrello> p = new WelcomeLibrary.UF.Pager<TotaliCarrello>(ordini, true, this.Page, PageGuid + PagerRisultati.ClientID);
        PagerRisultati.TotalRecords = p.Count;
        if (PagerRisultati.TotalRecords == 0) PagerRisultati.CurrentPage = 1;
        try
        {
            PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
        }
        catch
        {
            Pagina = "1";
        }
        rtpOrdini.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
        rtpOrdini.DataBind();

#endif
        PreparaStampa();
    }
    #region PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER

    protected void PagerRisultati_PageCommand(object sender, string PageNum)
    {
        UC_PagerEx PagerRisultati = sender as UC_PagerEx;
        PagerRisultati.CurrentPage = Convert.ToInt32(PageNum);
        Pagina = PageNum;
        CaricaOrdini();
    }

    protected void PagerRisultati_PageGroupClickNext(object sender, string spare)
    {
        int pag = PagerRisultati.CurrentPage;
        pag++;
        if (pag > PagerRisultati.totalPages) pag = PagerRisultati.totalPages;
        Pagina = pag.ToString();
        CaricaOrdini();
    }

    protected void PagerRisultati_PageGroupClickPrev(object sender, string spare)
    {
        int pag = PagerRisultati.CurrentPage;
        pag--;
        if (pag < 1) pag = 1;
        Pagina = pag.ToString();
        CaricaOrdini();

    }

    #endregion


    protected void btnFiltroordine_Click(object sender, EventArgs e)
    {
        CaricaOrdini();
    }
    protected void chkPagato_CheckedChanged(object sender, EventArgs e)
    {
        string codiceordine = ((CheckBox)sender).ToolTip;
        eCommerceDM eDM = new eCommerceDM();
        TotaliCarrello c = eDM.CaricaOrdinePerCodiceOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codiceordine);
        if (c != null)
        {
            c.Pagato = ((CheckBox)sender).Checked;
            eDM.UpdateOrdine(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, c);
        }
        CaricaOrdini();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

        string idforced = id_commerciale;
        if (string.IsNullOrEmpty(idforced))
            idforced = txtCommerciale.Text;
        TotaliCarrelloCollection listaordini = CaricaDatiOrdini(txtCLIENTE.Text, txtCodiceordine.Text, txtdatamin.Text, txtdatamax.Text, idforced);
        string csvName = "export-ordini-" + string.Format("{0:dd-MM-yyyy}", System.DateTime.Now) + ".csv";
        string pathFile = WelcomeLibrary.STATIC.Global.percorsoFisicoComune + "\\_temp\\";
        if (!System.IO.Directory.Exists(pathFile))
            System.IO.Directory.CreateDirectory(pathFile);
        eCommerceDM eDM = new eCommerceDM();
        string retmessage = eDM.ExportOrdersToCsv(pathFile, csvName, listaordini);

        WelcomeLibrary.UF.SharedStatic.DownloadFile(WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp/" + csvName);
    }

    protected void PreparaStampa()
    {
        System.Text.StringBuilder sb = new StringBuilder();
        sb.Append(" <table style=\"padding:10px;border:solid 1px #000000\" class=\"table table-order table-stripped\"><tr>");
        sb.Append("<thead>");
        sb.Append("<tr style=\"background-color:#ccc;color:#fff\">");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Date</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Order ID</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Id Cliente</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Mail</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Denominazione</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Totale Ordine</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Tipo Pagamento</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Comm.</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Pagato</td>");
        sb.Append("<td style=\"background-color:#ccc;color:#fff\">Dettagli</td>");
        sb.Append("</tr>");
        sb.Append("</thead>");
        string idforced = id_commerciale;
        if (string.IsNullOrEmpty(idforced))
            idforced = txtCommerciale.Text;
        TotaliCarrelloCollection listaordini = CaricaDatiOrdini(txtCLIENTE.Text, txtCodiceordine.Text, txtdatamin.Text, txtdatamax.Text, idforced);
        if (listaordini != null)
            foreach (TotaliCarrello t in listaordini)
            {
                sb.Append("<tr>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + string.Format("{0:dd/MM/yyyy HH:mm:ss}", t.Dataordine) + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + t.CodiceOrdine + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + t.Id_cliente + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + t.Mailcliente + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + t.Denominazionecliente + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">" + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",
                       new object[] { t.TotaleSmaltimento + t.TotaleOrdine + t.TotaleSpedizione - t.TotaleSconto }) + " €" + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\" class=\"order-status\">" + TipopagaDisplay(t) + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\" class=\"order-status\">" + t.Id_commerciale + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\" class=\"order-status\">" + StatusCheck(t) + "</td>");
                sb.Append("<td style=\"background-color:#eee;color:#000;padding:3px\">");

                sb.Append("<ul style=\"list-style:none;font-size:10px\">" + VisualizzaCarrello(t.CodiceOrdine, true) + "</ul>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td colspan=\"9\" style=\"border-bottom:1px solid #000000\"></td></tr>");
            }

        sb.Append("</table>");
        Session.Add("datistampa", sb.ToString());

    }


    protected void btnStampa_Click(object sender, EventArgs e)
    {
        PreparaStampa();
    }
}