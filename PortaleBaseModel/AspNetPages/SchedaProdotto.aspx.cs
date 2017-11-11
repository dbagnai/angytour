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
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Data.OleDb;

public partial class _SchedaProdotto : CommonPage
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
    public string Categoria
    {
        get { return ViewState["Categoria"] != null ? (string)(ViewState["Categoria"]) : ""; }
        set { ViewState["Categoria"] = value; }
    }
    public string Categoria2liv
    {
        get { return ViewState["Categoria2liv"] != null ? (string)(ViewState["Categoria2liv"]) : ""; }
        set { ViewState["Categoria2liv"] = value; }
    }
    public string Caratteristica1
    {
        get { return ViewState["Caratteristica1"] != null ? (string)(ViewState["Caratteristica1"]) : ""; }
        set { ViewState["Caratteristica1"] = value; }
    }
    public string Caratteristica2
    {
        get { return ViewState["Caratteristica2"] != null ? (string)(ViewState["Caratteristica2"]) : ""; }
        set { ViewState["Caratteristica2"] = value; }
    }

    public string Caratteristica3
    {
        get { return ViewState["Caratteristica3"] != null ? (string)(ViewState["Caratteristica3"]) : ""; }
        set { ViewState["Caratteristica3"] = value; }
    }
    public string Caratteristica4
    {
        get { return ViewState["Caratteristica4"] != null ? (string)(ViewState["Caratteristica4"]) : ""; }
        set { ViewState["Caratteristica4"] = value; }
    }
    public string Caratteristica5
    {
        get { return ViewState["Caratteristica5"] != null ? (string)(ViewState["Caratteristica5"]) : ""; }
        set { ViewState["Caratteristica5"] = value; }
    }
    public string FasciaPrezzo
    {
        get { return ViewState["FasciaPrezzo"] != null ? (string)(ViewState["FasciaPrezzo"]) : ""; }
        set { ViewState["FasciaPrezzo"] = value; }
    }
    public bool Vetrina
    {
        get { return ViewState["Vetrina"] != null ? (bool)(ViewState["Vetrina"]) : false; }
        set { ViewState["Vetrina"] = value; }
    }
    public string Ordinamento
    {
        get { return ViewState["Ordinamento"] != null ? (string)(ViewState["Ordinamento"]) : ""; }
        set { ViewState["Ordinamento"] = value; }
    }
    int progressivosepara = 1;
    Offerte item = new Offerte();
    public bool JavaInjection = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //URL ORIGINALE
        //http://localhost:8888/test/ASPNETPAGES/SchedaOffertaMaster.aspx?idOfferta=106&CodiceTipologia=RIF000003&Lingua=I 
        //URL REWRITED
        //http://localhost:8888/test/articoli/rif000003-I-106-www.fraumbriaetoscana.it.aspx

        try
        {
            if (!IsPostBack)
            {
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;

                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                idOfferta = CaricaValoreMaster(Request, Session, "idOfferta");
                CodiceTipologia = CaricaValoreMaster(Request, Session, "Tipologia");
                Categoria = CaricaValoreMaster(Request, Session, "Categoria");
                Categoria2liv = CaricaValoreMaster(Request, Session, "Categoria2liv", false);
                Caratteristica1 = CaricaValoreMaster(Request, Session, "Caratteristica1", false);
                Caratteristica2 = CaricaValoreMaster(Request, Session, "Caratteristica2", false);
                Caratteristica3 = CaricaValoreMaster(Request, Session, "Caratteristica3", false);
                Caratteristica4 = CaricaValoreMaster(Request, Session, "Caratteristica4", false);
                Caratteristica5 = CaricaValoreMaster(Request, Session, "Caratteristica5", false);
                FasciaPrezzo = CaricaValoreMaster(Request, Session, "FasciaPrezzo", false);
                Ordinamento = CaricaValoreMaster(Request, Session, "Ordinamento", false, "");

                bool tmpbool = false;
                bool.TryParse(CaricaValoreMaster(Request, Session, "Vetrina"), out tmpbool);
                Vetrina = tmpbool;


                item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
                if (item != null)
                {
                    Categoria = item.CodiceCategoria;
                    if (Categoria != "")
                        Session["Categoria"] = Categoria;
                    Categoria2liv = item.CodiceCategoria2Liv;
                    if (Categoria2liv != "")
                        Session["Categoria2liv"] = Categoria2liv;
                    CodiceTipologia = item.CodiceTipologia;
                    if (CodiceTipologia != "")
                        Session["Tipologia"] = CodiceTipologia;
                    AssociaDatiSocial(item);
                }
                SettaTestoIniziale();
                SettaVisualizzazione(item);


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
    protected void SettaVisualizzazione(Offerte item)
    {
        string controlsuggeriti = "";
        ClientScriptManager cs = Page.ClientScript;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        switch (CodiceTipologia)
        {
            case "":
                column1.Visible = true;
                column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-md-10 col-sm-10";
                column3.Visible = false;
                divContact.Visible = false;
                divContact.Visible = false;
                //controlsuggeriti = "injectScrollerAndLoad(\"owlscrollerProdotti1.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", false, true, 12,2, \"" + Categoria2liv + "\");";
                sb.Clear();
                sb.Append("(function wait() {");
                sb.Append("  if (typeof injectScrollerAndLoad === \"function\")");
                sb.Append("    {");
                sb.Append("injectScrollerAndLoad(\"owlscrollerProdotti1.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", false, true, 12,2, \"" + Categoria2liv + "\");");
                sb.Append(" }");
                sb.Append("   else  {");
                sb.Append("  setTimeout(wait, 50);");
                sb.Append("  }  })();");


                if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                {
                    cs.RegisterStartupScript(this.GetType(), "controlsuggeriti", sb.ToString(), true);
                }

                //BIND PER LA SCHEDA!!!!
                //string controlresource1 = "injectandloadgenericcontent(\"schedadetailsprod.html\",\"divItemContainter1\", \"divitem\",true,true, \"" + idOfferta + "\");";

                sb.Clear();
                sb.Append("(function wait() {");
                sb.Append("  if (typeof injectandloadgenericcontent === \"function\")");
                sb.Append("    {");
                sb.Append("injectandloadgenericcontent(\"schedadetailsprod.html\",\"divItemContainter1\", \"divitem\",true,true, \"" + idOfferta + "\");");
                sb.Append(" }");
                sb.Append("   else  {");
                sb.Append("  setTimeout(wait, 50);");
                sb.Append("  }  })();");

                if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                {
                    cs.RegisterStartupScript(this.GetType(), "controlresource1", sb.ToString(), true);
                }

                break;
            default:
                column1.Visible = false;
                //column1.Attributes["class"] = "col-md-1 col-sm-1";
                column2.Attributes["class"] = "col-md-12 col-sm-12";
                column3.Visible = false;
                //column3.Attributes["class"] = "col-md-3 col-sm-3";
                divContact.Visible = false;
                //controlsuggeriti = "injectScrollerAndLoad(\"owlscrollerProdotti1.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", false, true, 12,2, \"" + Categoria2liv + "\");";

                sb.Clear();
                sb.Append("(function wait() {");
                sb.Append("  if (typeof injectandloadgenericcontent === \"function\")");
                sb.Append("    {");
                sb.Append("injectScrollerAndLoad(\"owlscrollerProdotti1.html\",\"divScrollerSuggeritiJs\", \"carSuggeriti\",\"\", \"" + CodiceTipologia + "\", \"" + Categoria + "\", false, true, 12,2, \"" + Categoria2liv + "\");");
                sb.Append(" }");
                sb.Append("   else  {");
                sb.Append("  setTimeout(wait, 50);");
                sb.Append("  }  })();");

                if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
                {
                    cs.RegisterStartupScript(this.GetType(), "controlsuggeriti", sb.ToString(), true);
                }
                this.AssociaDati();
                break;
        }
    }

    protected string GeneraBackLink(bool usacategoria = true)
    {
        string ret = "";
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
        {
            string testourl = item.Descrizione;
            Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            if (catselected != null && usacategoria)
                testourl = catselected.Descrizione;
            if (!string.IsNullOrEmpty(Categoria2liv))
            {
                SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
                if (categoriasprodotto != null && usacategoria)
                {
                    testourl = categoriasprodotto.Descrizione;
                }
            }
            string tmpcategoria = Categoria;
            string tmpcategoria2liv = Categoria2liv;
            if (!usacategoria)
            {
                tmpcategoria = ""; tmpcategoria2liv = "";
            }

            ret = CommonPage.CreaLinkRoutes(Session, false, Lingua, CommonPage.CleanUrl(testourl), "", CodiceTipologia, tmpcategoria, tmpcategoria2liv);
        }
        return ret;

    }

    private void SettaTestoIniziale()
    {

        TipologiaOfferte itemtipo = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (itemtipo != null)
        {
            string titolopagina = itemtipo.Descrizione.ToUpper();
            //litSezione.Text = titolopagina;
            //Prodotto catselected = Utility.ElencoProdotti.Find(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            //if (catselected != null)
            //{
            //    litSezione.Text += " " + catselected.Descrizione.ToUpper();
            //}

            string htmlPage = "";
            if (references.ResMan("Common", Lingua, "testo" + CodiceTipologia) != null)
                htmlPage = references.ResMan("Common", Lingua, "testo" + CodiceTipologia).ToString();
            if (references.ResMan("Common", Lingua, "testo" + Categoria) != null)
                htmlPage = references.ResMan("Common", Lingua, "testo" + Categoria).ToString();

            string strigaperricerca = "";
#if false
            strigaperricerca = "/" + CodiceTipologia + "/" + idOfferta + "/";
            Contenuti content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            if (content == null && !string.IsNullOrEmpty(Categoria))
            {
                strigaperricerca = "/" + CodiceTipologia + "/" + Categoria + "/"; //Request.Url.AbsolutePath
                content = content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            }
            if (content == null && !string.IsNullOrEmpty(titolopagina))
            {
                strigaperricerca = "/" + CodiceTipologia + "/" + CleanUrl(titolopagina); //Request.Url.AbsolutePath
                content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            } 
#endif
            Contenuti content = null;

            string denominazione = item.DenominazionebyLingua(Lingua);
            string linkcanonico = CreaLinkRoutes(null, false, Lingua, CleanUrl(denominazione), item.Id.ToString(), item.CodiceTipologia);
            linkcanonico = linkcanonico.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "");
            content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkcanonico); ;
            if (content == null || content.Id == 0)
            {
                linkcanonico = linkcanonico.Substring(0, linkcanonico.LastIndexOf("/") + 1);
                content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, linkcanonico); ;
                if (content != null && !content.TitolobyLingua(Lingua).EndsWith("/")) content = null; //evito di prendere i contenuti dedicati alle pagine lista
            }

            if (content != null && content.Id != 0)
            {
                htmlPage = content.DescrizionebyLingua(Lingua);
                if (htmlPage.Contains("injectandloadgenericcontent")) JavaInjection = true;

            }

            litTextHeadPage.Text = ReplaceAbsoluteLinks(ReplaceLinks(htmlPage));
        }


    }


    public string SeparaRows()
    {
        string ritorno = "";
        int elementiperriga = 3;
        if (((progressivosepara) % elementiperriga) == 0)//&& i != totalefoto1)
            ritorno += "</div><div class=\"row\">";
        progressivosepara += 1;
        return ritorno;
    }
    protected void aFacebook_prerender(object sender, EventArgs e)
    {

        // ((HtmlAnchor)sender).Attributes.Add("", "");
    }

    protected string ImpostaIntroPrezzo(string codicetipologia)
    {
        string ret = "";
        // if (codicetipologia == "rif000002") ret = references.ResMan("Common",Lingua,"TitoloPrezzoApartire + " ";
        if (codicetipologia == "rif000001") ret = references.ResMan("Common", Lingua, "TitoloPrezzo") + " ";

        return ret;
    }

    protected void AssociaRubricheConsigliati()
    {
        divSuggeriti.Visible = true;

        OfferteCollection offerte = new OfferteCollection();
        List<OleDbParameter> parColl = new List<OleDbParameter>();

        if (CodiceTipologia != "" && CodiceTipologia != "-")
        {
            OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", CodiceTipologia);
            parColl.Add(p3);
        }
        else return;
        if (item.Caratteristica1.ToString() != "" && item.Caratteristica1.ToString() != "0") //marca
        {
            OleDbParameter pcaratteristica1 = new OleDbParameter("@Caratteristica1", item.Caratteristica1);
            parColl.Add(pcaratteristica1);
        }
        //if (caratteristica2 != "" && caratteristica2 != "0") //modello
        //{
        //    OleDbParameter pcaratteristica2 = new OleDbParameter("@Caratteristica2", caratteristica2);
        //    parColl.Add(pcaratteristica2);
        //}
        offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "12", Lingua, null);

        //A caso sulla tipologia 12 elementi
        //offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceTipologia, "12", true, Lingua);
        progressivosepara = 1;
        rptArticoliSuggeriti.DataSource = offerte;
        rptArticoliSuggeriti.DataBind();
    }

    protected void AssociaRubricheRiferiteaid(string idcollegato)
    {
        OfferteCollection offerte = new OfferteCollection();
        offerte = offDM.CaricaOfferteCollegate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcollegato, "10", true, Lingua);
        rptArticoliSuggeriti.DataSource = offerte;
        rptArticoliSuggeriti.DataBind();
    }


    protected string CreaRigheDettaglio(Object itemobj)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (itemobj != null)
        {
            Offerte item = (Offerte)itemobj;
            string codiceprodotto = "";
            WelcomeLibrary.DOM.Prodotto p = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && tmp.CodiceProdotto == item.CodiceCategoria); });
            if (p != null)
            {
                sb.Append("  <tr class=\"alt\"><th>");
                sb.Append(references.ResMan("Common", Lingua, "ddlTuttiProdotto"));
                //sb.Append(GetGlobalResourceObject("Common", "ddlTuttiProdotto").ToString());
                sb.Append("  </th>");
                sb.Append("  <td>");
                sb.Append(p.Descrizione);
                sb.Append("  </td></tr>");
                codiceprodotto = p.CodiceProdotto;
            }
            WelcomeLibrary.DOM.SProdotto sp = WelcomeLibrary.UF.Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && tmp.CodiceSProdotto == item.CodiceCategoria2Liv); });
            if (sp != null)
            {
                sb.Append("  <tr class=\"alt\"><th>");
                sb.Append(references.ResMan("Common", Lingua, "ddlTuttiSottoProdotto"));
                sb.Append("  </th>");
                sb.Append("  <td>");
                sb.Append(sp.Descrizione);
                sb.Append("  </td></tr>");
            }


#if false

            if (item.CodiceRegione != null && !string.IsNullOrEmpty(item.CodiceRegione))
            {
                sb.Append("  <tr class=\"alt\"><th >");
                sb.Append(references.ResMan("Common", Lingua, "ddlTuttiregione"));
                sb.Append("  </th>");
                sb.Append("  <td>");
                sb.Append(NomeRegione(item.CodiceRegione, Lingua));
                sb.Append("  </td></tr>");
            } 
#endif
            //CREAZIONE DELLE DDL DI SELEZIONE PER LA SCELTA SE PRESENTE GESTIONE DISPONIBILITA' ARITICOLI PER CARATTERISTICHE
            if (!string.IsNullOrEmpty(item.Xmlvalue))
            {

                //Presento l'eventuale valore per la selezione con le caratterisiztiche per le disponibilità
                // hddTagColCod.Value = item.Caratteristica1 + "-" + item.Caratteristica2;


                List<ModelCarCombinate> listprod = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModelCarCombinate>>(item.Xmlvalue);
                List<ResultAutocomplete> listacat1 = new List<ResultAutocomplete>();
                List<ResultAutocomplete> listacar2 = new List<ResultAutocomplete>();
                foreach (ModelCarCombinate elem in listprod)
                {
                    if (!string.IsNullOrEmpty(elem.caratteristica1.id) && elem.caratteristica1.id != "0")
                    {
                        if (!listacat1.Exists(e => e.id == elem.caratteristica1.id))
                            listacat1.Add(elem.caratteristica1);
                    }

                    if (!string.IsNullOrEmpty(elem.caratteristica2.id) && elem.caratteristica2.id != "0")
                    {
                        if (!listacar2.Exists(e => e.id == elem.caratteristica2.id))
                            listacar2.Add(elem.caratteristica2);
                    }
                }
                sb.Append("  <tr class=\"alt\"><th>");
                sb.Append(references.ResMan("BaseText", Lingua, "selectcat1"));
                //sb.Append(GetGlobalResourceObject("BaseText", "selectcat1").ToString());
                sb.Append("  </th>");
                sb.Append("  <td>");
                string status0 = "disabled";
                if (listacat1 != null && listacat1.Count > 0) status0 = "";
                sb.Append("  <select id=\"ddlcat1\" class=\"form-control\" style=\"width:100%;\" onchange=\"changeValue(this,'')\" " + status0 + ">");
                sb.Append("  <option id=\"sc1\" value=\"\">" + references.ResMan("BaseText", Lingua, "genericselect") + "</option>");
                if (listacat1 != null)
                    foreach (ResultAutocomplete elem in listacat1)
                    {
                        string testoinlingua = elem.value;
                        Tabrif Car1 = Utility.Caratteristiche[0].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == elem.codice; });
                        if (Car1 != null)
                            testoinlingua = Car1.Campo1;

                        sb.Append("  <option id=\"" + elem.id + "\" value=\"" + elem.codice + "\">" + testoinlingua + "</option>");
                    }
                sb.Append("  <select>");
                sb.Append("  </td></tr>");

                //initDDLCaratteristiche(ddlcat1, listacat1, "");
                sb.Append("  <tr class=\"alt\"><th>");
                sb.Append(references.ResMan("BaseText", Lingua, "selectcat2"));
                //sb.Append(GetGlobalResourceObject("BaseText", "selectcat2").ToString());
                sb.Append("  </th>");
                sb.Append("  <td>");
                string status1 = "disabled";
                if (listacar2 != null && listacar2.Count > 0) status1 = "";
                sb.Append("  <select id=\"ddlcat2\" class=\"form-control\"  style=\"width:100%;\" onchange=\"changeValue(this,'')\" " + status1 + ">");
                sb.Append("  <option id=\"sc2\" value=\"\">" + references.ResMan("BaseText", Lingua, "genericselect") + "</option>");
                if (listacar2 != null)
                    foreach (ResultAutocomplete elem in listacar2)
                    {

                        string testoinlingua = elem.value;
                        Tabrif Car2 = Utility.Caratteristiche[1].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == elem.codice; });
                        if (Car2 != null)
                            testoinlingua = Car2.Campo1;

                        sb.Append("  <option id=\"" + elem.id + "\" value=\"" + elem.codice + "\">" + testoinlingua + "</option>");
                    }
                sb.Append("  <select>");

                sb.Append("  </td></tr>");
            }
            else // NON IMPOSTAZIONE DISPONIBILITA' PER CARATTERISTICA -> SOLO VISUALIZZAZIONE
            {
                //SOLO VISUALIZZAZIONE CARATTERISTICA
                Tabrif Car1 = Utility.Caratteristiche[0].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.Caratteristica1.ToString(); });
                if (Car1 != null)
                {
                    sb.Append("  <tr class=\"alt\"><th>");
                    sb.Append(references.ResMan("BaseText", Lingua, "selectcat1"));
                    //sb.Append(GetGlobalResourceObject("BaseText", "selectcat1").ToString());
                    sb.Append("  </th>");
                    sb.Append("  <td>");
                    sb.Append(Car1.Campo1);
                    sb.Append("  </td></tr>");
                }
                //SOLO VISUALIZZAZIONE CARATTERISTICA
                Tabrif Car2 = Utility.Caratteristiche[1].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.Caratteristica2.ToString(); });
                if (Car2 != null)
                {
                    sb.Append("  <tr class=\"alt\"><th>");
                    sb.Append(references.ResMan("BaseText", Lingua, "selectcat2"));
                    //sb.Append(GetGlobalResourceObject("BaseText", "selectcat2").ToString());
                    sb.Append("  </th>");
                    sb.Append("  <td>");
                    sb.Append(Car2.Campo1);
                    sb.Append("  </td></tr>");
                }
            }




            if (!string.IsNullOrEmpty(item.CodiceProdotto))
            {
                sb.Append("  <tr class=\"alt\"><th>");
                sb.Append(references.ResMan("BaseText", Lingua, "Codice"));
                //sb.Append(GetGlobalResourceObject("BaseText", "Codice").ToString());
                sb.Append("  </th>");
                sb.Append("  <td>");
                sb.Append(item.CodiceProdotto);
                sb.Append("  </td></tr>");
            }


            //Tabrif Car6 = Utility.Caratteristiche[5].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.Caratteristica6.ToString(); });
            //if (Car6 != null)
            //{
            //    sb.Append("  <tr class=\"alt\"><th>");
            //    sb.Append(GetGlobalResourceObject("BaseText", "selectditta").ToString());
            //    sb.Append("  </th>");
            //    sb.Append("  <td>");
            //    sb.Append(Car6.Campo1);
            //    sb.Append("  </td></tr>");
            //}




            //Tabrif Car3 = Utility.Caratteristiche[2].Find(delegate (Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.Caratteristica3.ToString(); });
            //if (Car3 != null)
            //{
            //    sb.Append("  <tr class=\"alt\"><th >");
            //    sb.Append(GetGlobalResourceObject("BaseText", "selectatcgmpliv1").ToString());
            //    sb.Append("  </th>");
            //    sb.Append("  <td>");
            //    sb.Append(references.TestoCaratteristicaSublivelli(2, 1, item.Caratteristica3, Lingua));
            //    sb.Append("  </td></tr>");
            //    sb.Append("  <tr class=\"alt\"><th >");
            //    sb.Append(GetGlobalResourceObject("BaseText", "selectatcgmpliv2").ToString());
            //    sb.Append("  </th>");
            //    sb.Append("  <td>");
            //    sb.Append(references.TestoCaratteristicaSublivelli(2, 2, item.Caratteristica3, Lingua));
            //    sb.Append("  </td></tr>");
            //    sb.Append("  <tr class=\"alt\"><th >");
            //    sb.Append(GetGlobalResourceObject("BaseText", "selectatcgmpliv3").ToString());
            //    sb.Append("  </th>");
            //    sb.Append("  <td>");
            //    sb.Append(references.TestoCaratteristicaSublivelli(2, 3, item.Caratteristica3, Lingua));
            //    sb.Append("  </td></tr>");
            //}

            //if (item.Anno != 0)
            //{
            //    sb.Append("  <tr class=\"\"><th>");
            //    sb.Append(GetGlobalResourceObject("Common", "ddlTuttiAnno").ToString());
            //    sb.Append("  </th>");
            //    sb.Append("  <td>");
            //    sb.Append(item.Anno);
            //    sb.Append("  </td></tr>");
            //}



            //Tabrif Car3 = Utility.Caratteristiche[2].Find(delegate(Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.Caratteristica3.ToString(); });
            //Tabrif Car4 = Utility.Caratteristiche[3].Find(delegate(Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.Caratteristica4.ToString(); });
            //Tabrif Car5 = Utility.Caratteristiche[4].Find(delegate(Tabrif _t) { return _t.Lingua == Lingua && _t.Codice == item.Caratteristica5.ToString(); });
            //if (Car3 != null & Car4 != null && Car5 != null)
            //{
            //    sb.Append("  <tr class=\"alt\"><th>");
            //    sb.Append(GetGlobalResourceObject("Common", "ddlTuttiCarDim").ToString());
            //    sb.Append("  </th>");
            //    sb.Append("  <td>");
            //    sb.Append(Car3.Campo1 + "/");
            //    sb.Append(Car4.Campo1 + " ");
            //    sb.Append(Car5.Campo1);
            //    sb.Append("  </td></tr>");

            //}

        }

        return sb.ToString();
    }


#if false //Sezione elaborazione dati automotive

    protected bool VarificaSuOpzioni(Object itemobj, string valore)
    {
        bool ret = false;
        //Vrificare la presenza del valore nel campo opzioni
        //Tipo di legno interno -> a,b,c,d sono le opzioni importanti da visualizzare da  op.Campo1.ToLower().Trim() -> accenzione dei vari loghi in alto
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (itemobj != null)
        {
            Offerte item = (Offerte)itemobj;
            Tabrif val = new Tabrif();
            //Metodo per lettura delle options ( id/description -> value ( per value dovresti cercare la corrispondenza nella tabella riferimento options )
            List<Tabrif> options = ReadOptionsValues(item.Xmlvalue);
            if (options == null || options.Count == 0) return false;
            foreach (Tabrif op in options)
            {
                //Dalle opzioni memorizzate nel record del db
                //op.Campo2 //id opzione
                //  op.Campo1 // Valore assunto
                //FAccio la lookup con la tabella caratteristiche5 in memoria  ( che contiene la tabella di riferimento delle opzioni )
                Tabrif oplook = Utility.Caratteristiche[4].Find(c => c.Codice == op.Campo2 && c.Lingua == Lingua);
                if (oplook != null)
                {
                    if (oplook.Campo3.ToLower().Trim().Contains("tipo di legno interno"))
                    {
                        //Tipo di legno interno -> a,b,c,d sono le opzioni importanti da visualizzare da  op.Campo1.ToLower().Trim() -> accenzione dei vari loghi in alto
                        string[] rangevalues = op.Campo1.Split(',');
                        foreach (string s in rangevalues)
                        {
                            if (s.ToLower() == valore.ToLower())
                            {
                                ret = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        return ret;

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

            sb.Append("<div class=\"row\" style=\"border-top:1px solid #ddd\">");
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

                                    sb.Append("<span style=\"font-size:0.9em\">");
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
                            sb.Append("<span style=\"font-size:0.9em\">");
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
                sb.Append("<span class=\"h3-body-title-1\">" + references.ResMan("Common",Lingua,"testoTipologiavettura") + " </span>" + cf1.Campo1);
            sb.Append("<br/\">");
            sb.Append("<span class=\"h3-body-title-1\">" + references.ResMan("Common",Lingua,"testoImmatricolazione") + " </span>" + string.Format("{0:dd/MM/yyyy}", Eval("Data1")));
            sb.Append("<br/\">");
            val = ReadXmlSinglevalue(item.Xmlvalue, "mainData", "cylinderCapacity");
            sb.Append("<span class=\"h3-body-title-1\">" + references.ResMan("Common",Lingua,"testoCilindrata") + " </span>" + val.Campo1 + " " + val.Campo2);
            sb.Append("<br/\">");
            Tabrif cf = Utility.Caratteristiche[3].Find(c => c.Codice == item.Caratteristica4.ToString() && c.Lingua == Lingua);
            if (cf != null)
                sb.Append("<span class=\"h3-body-title-1\">" + references.ResMan("Common",Lingua,"testoAlimentazione") + " </span>" + cf.Campo1);

            sb.Append("<br/\">");
            val = ReadXmlSinglevalue(item.Xmlvalue, "mainData", "transmission");
            sb.Append("<span class=\"h3-body-title-1\">" + references.ResMan("Common",Lingua,"testoTrasmissione") + " </span>" + val.Campo1);

            sb.Append("</div>");
            sb.Append("<div class=\"col-sm-6\">");
            sb.Append("<span class=\"h3-body-title-1 \">Codice </span>" + item.CodiceProdotto);
            sb.Append("<br/\">"); val = CommonPage.ReadXmlSinglevalue(item.Xmlvalue, "mainData", "mileage");
            sb.Append("<span class=\"h3-body-title-1 \">" + references.ResMan("Common",Lingua,"testoChilometraggio") + " </span>" + val.Campo1 + " " + val.Campo2);
            sb.Append("<br/\">");
            val = ReadXmlSinglevalue(item.Xmlvalue, "mainData", "enginePower");
            sb.Append("<span class=\"h3-body-title-1\">" + references.ResMan("Common",Lingua,"testoPotenza") + " </span>" + val.Campo1 + " " + val.Campo2);
            sb.Append("<br/\">");

            List<Tabrif> colordata = ReadColorvalue(item.Xmlvalue);
            string tmp = "";
            if (colordata != null && colordata.Count > 0)
                tmp = colordata[0].Campo1;
            if (colordata != null && colordata.Count > 1)
                tmp += " " + colordata[1].Campo1;
            sb.Append("<span class=\"h3-body-title-1\">" + references.ResMan("Common",Lingua,"testoColore + " </span>" + tmp);

            sb.Append("<br/\">");
            val = ReadXmlSinglevalue(item.Xmlvalue, "mainData", "bodyStyle");
            sb.Append("<span class=\"h3-body-title-1\">" + references.ResMan("Common",Lingua,"testoCategoriaveicolo") + " </span>" + val.Campo1);

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
    
#endif

    protected void AssociaDati()
    {
        //Carichiamo l'immobile a partire dal codice dello stesso e dalla lingua
        OfferteCollection offerte = new OfferteCollection();
#if true
        Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
        //if (item != null)
        //{
        //    Categoria = item.CodiceCategoria;
        //    if (Categoria != "")
        //        Session["Categoria"] = Categoria;
        //    CodiceTipologia = item.CodiceTipologia;
        //    if (CodiceTipologia != "")
        //        Session["Tipologia"] = CodiceTipologia;
        //    AssociaDatiSocial(item);
        //}
        offerte.Add(item);

#endif

        //Associamo ai controlli i dati dell'immobile
        rptOfferta.DataSource = offerte;
        rptOfferta.DataBind();
        //  UserPanel.Update();
    }


    protected bool VerificaPresenzaPrezzo(object prezzo)
    {
        bool ret = false;
        if (prezzo != null && (double)prezzo != 0)
            ret = true;
        return ret;
    }

    protected bool AttivaContatto(object flag)
    {
        bool ret = false;
        if (flag != null)
        {
            ret = ((bool)flag);
        }
        return ret;
    }
    protected void AssociaDatiSocial(Offerte data)
    {
        string descrizione = data.DescrizionebyLingua(Lingua);
        string denominazione = data.DenominazionebyLingua(Lingua);

        //  Categoria = data.CodiceCategoria;
        EvidenziaSelezione(denominazione.Replace(" ", "").Replace("-", "").Replace("&", "e").Replace("'", "").Replace("?", ""));
        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();   //;

        //Comments facebook
        //divComments.Attributes.Add("data-href", ReplaceAbsoluteLinks(CreaLinkRoutes(null, false, Lingua, CleanUrl(denominazione), data.Id.ToString(), data.CodiceTipologia)));

        //Titolo e descrizione pagina
        string posizione = ControlloVuotoPosizione(data.CodiceComune, data.CodiceProvincia, data.CodiceRegione, "", Lingua);

        ((HtmlTitle)Master.FindControl("metaTitle")).Text = html.Convert((denominazione).Replace("<br/>", " ").Trim() + " " + posizione + " " + Nome);
        string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 300, true)).Replace("<br/>", " ").Trim());
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;
        //Opengraph per facebook
        ((HtmlMeta)Master.FindControl("metafbTitle")).Content = html.Convert((denominazione + " " + Nome).Replace("<br/>", " ").Trim());
        simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 300, true))).Replace("<br/>", " ").Trim();
        ((HtmlMeta)Master.FindControl("metafbdescription")).Content = simpletext;

        if (data.FotoCollection_M != null && !string.IsNullOrEmpty(data.FotoCollection_M.FotoAnteprima))
            ((HtmlMeta)Master.FindControl("metafbimage")).Content = ComponiUrlAnteprima(data.FotoCollection_M.FotoAnteprima, data.CodiceTipologia, data.Id.ToString(), true).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
        else if (data.FotoCollection_M != null && !string.IsNullOrEmpty(data.linkVideo))
            ((HtmlMeta)Master.FindControl("metafbvideourl")).Content = data.linkVideo;

        /////////////////////////////////////////////////////////////
        //MODIFICA PER TITLE E DESCRIPTION CUSTOM
        ////////////////////////////////////////////////////////////
        string customdesc = "";
        string customtitle = "";
        switch (Lingua)
        {
            case "GB":
                customdesc = data.Campo2GB;
                customtitle = data.Campo1GB;
                break;
            default:
                customdesc = data.Campo2I;
                customtitle = data.Campo1I;
                break;
        }
        if (!string.IsNullOrEmpty(customtitle))
            ((HtmlTitle)Master.FindControl("metaTitle")).Text = (customtitle).Replace("<br/>", "\r\n");
        if (!string.IsNullOrEmpty(customdesc))
            ((HtmlMeta)Master.FindControl("metaDesc")).Content = customdesc.Replace("<br/>", "\r\n");
        ////////////////////////////////////////////////////////////

        ///////////////////////////////////
        //string linkcanonico = CreaLinkRoutes(null, false, Lingua, CleanUrl(denominazione), data.Id.ToString(), data.CodiceTipologia);
        //Literal litgeneric = ((Literal)Master.FindControl("litgeneric"));
        //litgeneric.Text = "<link rel=\"canonical\" href=\"" + ReplaceAbsoluteLinks(linkcanonico) + "\"/>";
        Tabrif actualpagelink = new Tabrif();

        Literal litcanonic = ((Literal)Master.FindControl("litgeneric"));

        string hreflang = "";
        //METTIAMO GLI ALTERNATE
        hreflang = " hreflang=\"it\" ";
        string linkcanonicoalt = CreaLinkRoutes(null, false, "I", CleanUrl(data.DenominazioneI), data.Id.ToString(), data.CodiceTipologia);
        Literal litdefault = ((Literal)Master.FindControl("litgeneric0"));
        litdefault.Text = "<link rel=\"alternate\" hreflang=\"x-default\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
        Literal litgenericalt = ((Literal)Master.FindControl("litgeneric1"));
        litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
        if (Lingua == "I")
        {
            litcanonic.Text = "<link rel=\"canonical\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            actualpagelink.Campo1 = ReplaceAbsoluteLinks(linkcanonicoalt);
            actualpagelink.Campo2 = (data.DenominazioneI);
        }

        if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() == "true")
        {
            hreflang = " hreflang=\"en\" ";
            linkcanonicoalt = CreaLinkRoutes(null, false, "GB", CleanUrl(data.DenominazioneGB), data.Id.ToString(), data.CodiceTipologia);
            litgenericalt = ((Literal)Master.FindControl("litgeneric2"));
            litgenericalt.Text = "<link  rel=\"alternate\" " + hreflang + " href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
            if (Lingua == "GB")
            {
                litcanonic.Text = "<link rel=\"canonical\"  href=\"" + ReplaceAbsoluteLinks(linkcanonicoalt) + "\"/>";
                Tabrif link = new Tabrif();
                actualpagelink.Campo1 = ReplaceAbsoluteLinks(linkcanonicoalt);
                actualpagelink.Campo2 = CleanUrl(data.DenominazioneGB);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////BREAD CRUMBS///////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        List<Tabrif> links = GeneraBreadcrumbPath(true);
        links.Add(actualpagelink);
        HtmlGenericControl ulbr = (HtmlGenericControl)Master.FindControl("ulBreadcrumb");
        ulbr.InnerHtml = BreadcrumbConstruction(links);
    }
    private List<Tabrif> GeneraBreadcrumbPath(bool usacategoria)
    {
        List<Tabrif> links = new List<Tabrif>();
        Tabrif link = new Tabrif();
        link.Campo1 = ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkHome"));
        link.Campo2 = references.ResMan("Common", Lingua, "testoHome");
        links.Add(link);


        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
        {
            string testourl = item.Descrizione;
            Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            if (catselected != null && usacategoria)
                testourl = catselected.Descrizione;
            if (!string.IsNullOrEmpty(Categoria2liv))
            {
                SProdotto categoriasprodotto = Utility.ElencoSottoProdotti.Find(delegate (WelcomeLibrary.DOM.SProdotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceProdotto == Categoria) && (tmp.CodiceSProdotto == Categoria2liv)); });
                if (categoriasprodotto != null && usacategoria)
                {
                    testourl = categoriasprodotto.Descrizione;
                }
            }
            string tmpcategoria = Categoria;
            string tmpcategoria2liv = Categoria2liv;
            if (!usacategoria)
            {
                tmpcategoria = ""; tmpcategoria2liv = "";
            }


            string linkcanonicoalt = CreaLinkRoutes(null, false, Lingua, CleanUrl(testourl), "", CodiceTipologia, tmpcategoria, tmpcategoria2liv);
            link = new Tabrif();
            link.Campo1 = linkcanonicoalt;
            link.Campo2 = testourl;

            if (true) //Pagina copertina presente
            {
                Prodotto catcopertina = WelcomeLibrary.UF.Utility.ElencoProdotti.Find(p => p.CodiceTipologia == CodiceTipologia && p.CodiceProdotto == Categoria && p.Lingua == Lingua);
                if (catcopertina != null && !string.IsNullOrEmpty((catcopertina.Descrizione.ToLower().Trim())))
                {
                    Contenuti contentpercategoria = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, catcopertina.Descrizione.ToLower().Trim());
                    if (contentpercategoria != null && contentpercategoria.Id != 0)
                    {
                        Tabrif laddink = new Tabrif();
                        laddink.Campo1 = CommonPage.CreaLinkRoutes(Session, true, Lingua, CommonPage.CleanUrl(contentpercategoria.TitolobyLingua(Lingua)), contentpercategoria.Id.ToString(), "con001000");
                        laddink.Campo2 = contentpercategoria.TitolobyLingua(Lingua);
                        links.Add(laddink);
                    }
                }
            }

            links.Add(link);
        }


        return links;
    }


    protected void Cerca_Click(object sender, EventArgs e)
    {
        string link = CreaLinkRicerca("", CodiceTipologia, "", "", "", "", "", "-", Lingua, Session, true);
        Session.Add("testoricerca", Server.HtmlEncode(inputCerca.Value)); //carico in sessione il parametro da cercare
        Response.Redirect(link);
    }

    protected void CaricaUltimiPost(string tipologia, string categoria = "")
    {
        divLatestPost.Visible = true;
        //Filtriamo alcune categorie
        string tipologiadacaricare = tipologia;
        if (string.IsNullOrEmpty(tipologiadacaricare))
            tipologiadacaricare = "rif000001,rif000002,rif000003,rif000004,rif000005,rif000006,rif000007,rif000008,,rif000009";

        List<OleDbParameter> parColl = new List<OleDbParameter>();
#if true
        if (tipologiadacaricare != "" && tipologiadacaricare != "-")
        {
            OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", tipologiadacaricare);
            parColl.Add(p3);
        }
        if (categoria != "")
        {
            OleDbParameter p7 = new OleDbParameter("@CodiceCategoria", categoria);
            parColl.Add(p7);
        }


        OfferteCollection offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "30", Lingua);
        //offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceTipologia, "12", true, Lingua);

        //offerte.RemoveAll(c => (Convert.ToInt32((c.CodiceTipologia.Substring(3))) >= 100)); //Togliamo i risultati del catalogo ( andrebbero tolti nel filtro a monte)
#endif
        //OfferteCollection offerte = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, tipologiadacaricare, "6", false, Lingua, false);

        rtpLatestPost.DataSource = offerte;
        rtpLatestPost.DataBind();
    }

    protected string estraititolo(object testotitolo)
    {
        if (testotitolo == null) return "";
        string titolo1 = testotitolo.ToString();
        string titolo2 = "";
        int i = testotitolo.ToString().IndexOf("\n");
        if (i != -1)
        {
            titolo1 = testotitolo.ToString().Substring(0, i);
            if (testotitolo.ToString().Length >= i + 1)
                titolo2 = testotitolo.ToString().Substring(i + 1);
        }
        return titolo1;
    }
    protected string estraisottotitolo(object testotitolo)
    {
        if (testotitolo == null) return "";
        string titolo1 = testotitolo.ToString();
        string titolo2 = "<br/>";
        int i = testotitolo.ToString().IndexOf("\n");
        if (i != -1)
        {
            titolo1 = testotitolo.ToString().Substring(0, i);
            if (testotitolo.ToString().Length >= i + 1)
                titolo2 = testotitolo.ToString().Substring(i + 1);
        }
        return titolo2;
    }

    protected void rptOfferta_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                //Offerte data = (Offerte)(e.Item.DataItem);
                //HtmlGenericControl divSocial = (HtmlGenericControl)e.Item.FindControl("divSocial");
                //divSocial.Attributes.Add("addthis:url", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/SchedaContenutoMaster.aspx?idOfferta=" + data.Id + "&CodiceTipologia=" + data.CodiceTipologia + "&Lingua=" + Lingua);
                //divSocial.Attributes.Add("addthis:title", WelcomeLibrary.UF.Utility.SostituisciTestoACapo(data.DenominazioneI));
                //divSocial.Attributes.Add("addthis:description", CommonPage.ReplaceLinks(ConteggioCaratteri(data.DescrizioneI, 300)));

                ////Titolo e descrizione pagina
                //((HtmlTitle)Master.FindControl("metaTitle")).Text = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(data.DenominazioneI);
                //((HtmlMeta)Master.FindControl("metaDesc")).Content = CommonPage.ReplaceLinks(ConteggioCaratteri(data.DescrizioneI, 300));
                ////Opengraph per facebook
                //((HtmlMeta)Master.FindControl("metafbTitle")).Content = WelcomeLibrary.UF.Utility.SostituisciTestoACapo(data.DenominazioneI);
                //((HtmlMeta)Master.FindControl("metafbdescription")).Content = CommonPage.ReplaceLinks(ConteggioCaratteri(data.DescrizioneI, 300));

                //((HtmlMeta)Master.FindControl("metafbimage")).Content = ComponiUrlAnteprima(data.FotoCollection_M.FotoAnteprima, data.CodiceTipologia, data.Id.ToString()).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

            }
        }
    }



    protected string CreaSlideNavigation(object itemobj, int maxwidth, int maxheight)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Offerte item = ((Offerte)itemobj);
        if ((item != null) && (item.FotoCollection_M.Count > 0))
        {

            foreach (Allegato a in item.FotoCollection_M)
            {
                string testotitolo = item.DenominazionebyLingua(Lingua);

                //IMMAGINE
                string pathimmagine = ComponiUrlAnteprima(a.NomeFile, item.CodiceTipologia, item.Id.ToString());
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //LINK
                string virtuallink = ComponiUrlAnteprima(a.NomeFile, item.CodiceTipologia, item.Id.ToString());
                string link = virtuallink.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1)
                {
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }

                sb.Append(" <li>\r\n");
                if (!ControlloVideo(item.FotoCollection_M.FotoAnteprima))
                {
                    sb.Append("	  <img style=\"padding:5px");
                    //if (maxwidth > 0)
                    //    sb.Append("max-width:" + maxwidth + "px;");
                    //else
                    //    sb.Append("width:auto;");
                    //if (maxheight > 0)
                    //    sb.Append("max-height:" + maxheight + "px;");
                    //else
                    //    sb.Append("height:auto;");
                    sb.Append("\"  src=\"" + pathimmagine + "\" alt=\"" + testotitolo + "\" />\r\n");
                }
                sb.Append(" </li>\r\n");

            }
        }

        return sb.ToString();
    }

    protected bool ControlloVisibilitaMiniature(object fotos)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 1) ret = false;
        return ret;
    }

    protected string CreaSlide(object itemobj, int maxwidth, int maxheight)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Offerte item = ((Offerte)itemobj);
        if ((item != null) && (item.FotoCollection_M.Count > 0))
        {
            foreach (Allegato a in item.FotoCollection_M)
            {
                string testotitolo = "";
                if (!(a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                    continue;
                testotitolo = item.DenominazionebyLingua(Lingua);

                //IMMAGINE
                string pathimmagine = ComponiUrlAnteprima(a.NomeFile, item.CodiceTipologia, item.Id.ToString(), true);
                pathimmagine = pathimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

                //LINK
                string target = "_blank";
                string virtuallink = ComponiUrlAnteprima(a.NomeFile, item.CodiceTipologia, item.Id.ToString(), true);
                string link = virtuallink.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                if (link.ToLower().IndexOf("https://") == -1 && link.ToLower().IndexOf("http://") == -1 && !string.IsNullOrEmpty(link))
                {
                    target = "_self";
                    link = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + link;
                }


                sb.Append("<div class=\"slide\" ");
                if (item.FotoCollection_M.Count > 1)
                    sb.Append(" data-thumb=\"" + pathimmagine + "\" ");
                sb.Append(" >\r\n");
                sb.Append("    <div class=\"slide-content\" style=\"position:relative;padding:1px\">\r\n");

                if (!ControlloVideo(item.FotoCollection_M.FotoAnteprima, item.linkVideo))
                {
                    #region FOTO
                    //if (!string.IsNullOrEmpty(link))
                    //    sb.Append("	       <a href=\"" + link + "\" target=\"" + target + "\" title=\"" + testotitolo + "\">\r\n");
                    sb.Append("	           <img class=\"zoommgfy\"  itemprop=\"image\" style=\"");
#if true
                    string imgdimstyle = "";
                    int originalwidth = 0;
                    int originalheight = 0;
                    double scalefactorzoom = 1;
                    try
                    {
                        if (maxheight != 0)
                            using (System.Drawing.Image tmp = System.Drawing.Image.FromFile(Server.MapPath(virtuallink)))
                            {
                                originalwidth = tmp.Width;
                                originalheight = tmp.Height;

                                if (tmp.Width <= tmp.Height)
                                {
                                    imgdimstyle = "width:auto;height:" + maxheight + "px;";
                                }
                            }
                    }
                    catch
                    { }
                    if (imgdimstyle == "")
                    {
                        sb.Append("max-width:100%;");
                        sb.Append("height:auto;");
                    }
                    else
                        sb.Append(imgdimstyle);
#endif
                    sb.Append("border:none\" src=\"" + pathimmagine + "\" data-magnify-magnifiedwidth=\"" + (scalefactorzoom * originalwidth).ToString() + "\" data-magnify-magnifiedheight=\"" + (scalefactorzoom * originalheight).ToString() + "\" data-magnify-src=\"" + pathimmagine + "\" alt =\"" + testotitolo + "\" />\r\n");

                    //if (!string.IsNullOrEmpty(link))
                    //    sb.Append("	       </a>\r\n");

                    //aggiungiamo i messaggi sopra
                    if (!string.IsNullOrEmpty(a.Descrizione))
                    {
                        sb.Append("<div   class=\"divbuttonstyle\"  style=\"position:absolute;left:30px;bottom:30px;padding:10px;text-align:left;color:#ffffff;\">");
                        sb.Append("	       <a style=\"color:#ffffff\" href=\"" + link + "\" target=\"" + target + "\" title=\"" + testotitolo + "\">\r\n");
                        sb.Append(" " + a.Descrizione);
                        sb.Append("	       </a>\r\n");
                        sb.Append("	       </div>\r\n");
                    }
                    #endregion
                }

                sb.Append("    </div>\r\n");
                sb.Append("</div>\r\n");
            }
        }


        return sb.ToString();
    }


    protected bool ControlloVisibilita(object fotos)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 0) ret = false;
        bool onlypdf = (fotos != null && ((AllegatiCollection)fotos).Count > 0 && !((AllegatiCollection)fotos).Exists(c => (c.NomeFile.ToString().ToLower().EndsWith("jpg") || c.NomeFile.ToString().ToLower().EndsWith("gif") || c.NomeFile.ToString().ToLower().EndsWith("png"))));
        if (onlypdf) ret = false;
        return ret;
    }
    protected bool ControlloVideo(object NomeAnteprima, object linkVideo)
    {
        bool ret = false;
        //"http://www.youtube.com/embed/Z9lwY9arkj8"
        if ((NomeAnteprima == null || NomeAnteprima == "") && (linkVideo != null && ((string)linkVideo) != ""))
            ret = true;
        return ret;
    }

    protected string SorgenteVideo(object linkVideo)
    {
        string ret = "";
        //"http://www.youtube.com/embed/Z9lwY9arkj8"

        if (linkVideo != null && ((string)linkVideo) != "")
            ret = linkVideo.ToString();
        return ret;
    }
    protected string ImpostaTestoRichiesta()
    {
        string ret = "";
        //if (CodiceTipologia == "rif000001")
        //    ret = references.ResMan("Common",Lingua, "TestoRichiedi");
        // else
        //ret = references.ResMan("Common",Lingua, "TestoDisponibilita");
        ret = references.ResMan("Common", Lingua, "titoloinformazioni");


        return ret;
    }

    protected void EvidenziaSelezione(string testolink)
    {
        HtmlAnchor linkmenu = null;


        try
        {

            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#000 !important");
            }
            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink + "high"));
            if (linkmenu != null)
            {
                Control lidrop = CommonPage.FindControlRecursive(linkmenu.Parent, linkmenu.Parent.ID);
                if (lidrop != null)
                {
                    ((HtmlGenericControl)lidrop).Attributes["class"] += " active";
                }
            }

        }
        catch { }
        try
        {
            HtmlGenericControl limenu = ((HtmlGenericControl)Master.FindControl("link" + CodiceTipologia + "high"));
            if (limenu != null)
            {
                if (limenu != null)
                {
                    ((HtmlGenericControl)limenu).Attributes["class"] += " active";
                }
            }

        }
        catch { }
        try
        {
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#000 !important");
            }

        }
        catch { }
        try
        {
            linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink + "Lateral"));
            if (linkmenu != null)
            {
                linkmenu.Style.Add(HtmlTextWriterStyle.FontWeight, "600 !important");
                linkmenu.Style.Add(HtmlTextWriterStyle.Color, "#000 !important");
            }
        }
        catch { }
    }
    protected string ControlloVuotoEmail(string pre, string contenuto, object flag)
    {
        string ret = "";
        if (!string.IsNullOrWhiteSpace(contenuto))
            ret = pre + contenuto;
        bool abilitacontatto = false;
        if (flag != null)
        {
            abilitacontatto = ((bool)flag);
        }
        if (abilitacontatto) ret = "";
        return ret;
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
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });
        if (item != null)
            ritorno = "<br/><span style=\"margin-left:100px;padding:2px 10px 2px 10px;font-size:11pt;font-style:italic;color: #fff;background-color:#cc67fe\">" + item.Descrizione + "</span>";
        return ritorno;
    }
    protected string TestoSezione(string codicetipologia, bool solotitolo = false, bool nosezione = false)
    {
        string ret = "";
        WelcomeLibrary.DOM.TipologiaOfferte sezione =
              WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codicetipologia); });
        if (sezione != null)
        {
            string addtext = " " + references.ResMan("Common", Lingua, "testoSezione").ToString();
            if (nosezione) addtext = "";
            ret += addtext + CommonPage.ReplaceAbsoluteLinks(CommonPage.CrealinkElencotipologia(codicetipologia, Lingua, Session));

            if (solotitolo)
                ret = sezione.Descrizione;
        }

        return ret;
    }
    protected bool ControllaVisibilitaPerCodice(string codicetipologia)
    {
        bool ret = true;
        //if (codicetipologia == "rif000199" || codicetipologia == "rif000100") ret = false;
        if (codicetipologia == "rif000001") ret = false;
        return ret;
    }

    protected string ControlloVuotoPosizione(string comune, string codiceprovincia)
    {
        string ret = "";

        if (!string.IsNullOrWhiteSpace(comune))
            ret += comune;
        if (!string.IsNullOrWhiteSpace(codiceprovincia))
            ret += " (" + NomeProvincia(codiceprovincia, Lingua) + ") ";

        return ret;
    }


    protected bool VerificaPresenzaFoto(object foto)
    {
        bool ret = false;
        if (foto != null && ((List<Allegato>)foto).Count > 1)
        {
            ret = true;
        }
        return ret;

    }
    protected void btnRegistrastatistiche_Click(object sender, EventArgs e)
    {
        string id = (((Button)sender).CommandArgument);
        Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id);
        int _i = 0;
        int.TryParse(id, out _i);
        //Registro la statistica di contatto
        Statistiche stat = new Statistiche();
        stat.Data = DateTime.Now;
        stat.EmailDestinatario = "";
        stat.EmailMittente = "";
        stat.Idattivita = _i;
        stat.Testomail = "";
        stat.TipoContatto = enumclass.TipoContatto.visitaurl.ToString();
        stat.Url = item.Website;
        WelcomeLibrary.DAL.statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);
    }

    protected void btnUpdateCart(object sender, EventArgs e)
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
        string codTagCombined = hddTagCombined.Value;
        AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name, codTagCombined);

        txtQuantita_temp.Value = quantita.ToString();
        //Aggiorniamo nella masterpage il numero prodotti / articoli nel carrello
        // int nprodotti = ecom.ContaProdottiCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, Session.SessionID, trueIP);

        // AssociaDati();
        //QUI DEVI FARE L'AGGIORNAMENTO DEI RIEPILOGHI DEL CARRELLO NELLA MASTER!!!!->
        AggiornaVisualizzazioneDatiCarrello();


    }

    protected void btnInsertcart(object sender, EventArgs e)
    {


        string Idtext = ((LinkButton)sender).CommandArgument.ToString();
        int idprodotto = 0;
        int.TryParse(Idtext, out idprodotto);
        string codTagCombined = hddTagCombined.Value;
        string q = CaricaQuantitaNelCarrello(Request, Session, idprodotto.ToString(), codTagCombined);
        int quantita = 0;
        int.TryParse(q, out quantita);
        quantita += 1;//Incremento
        AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name, codTagCombined);

        //QUI DEVI FARE L'AGGIORNAMENTO DEI RIEPILOGHI DEL CARRELLO NELLA MASTER!!!!->
        AggiornaVisualizzazioneDatiCarrello();
        //AssociaDati();


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
        string codTagCombined = hddTagCombined.Value;
        AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name, codTagCombined);
        if (Session["superamentoquantita"] != null)
        {
            int qtamod = 0;
            int.TryParse(Session["superamentoquantita"].ToString(), out qtamod);
            Session.Remove("superamentoquantita");
            quantita = qtamod;
            output.Text = references.ResMan("Common", Lingua, "testoCarellosuperamentoquantita"); //Resources.Common.testoCarellosuperamentoquantita;
            if (Session["nontrovata"] != null)
            {
                Session.Remove("nontrovata");
                output.Text = references.ResMan("Common", Lingua, "testoCarellononesistente");// Resources.Common.testoCarellononesistente;
            }
        }
        txtQuantita_temp.Value = quantita.ToString();



        //QUI DEVI FARE L'AGGIORNAMENTO DEI RIEPILOGHI DEL CARRELLO NELLA MASTER!!!!->
        AggiornaVisualizzazioneDatiCarrello();
        //((Literal)Master.FindControl("litProdottiCarrello")).Text = nprodotti.ToString() + references.ResMan("CommonBase",Lingua,"testoCarrello1").ToString();
        //AssociaDati();

    }

    private void AggiornaVisualizzazioneDatiCarrello()
    {
        this.Master.VisualizzaTotaliCarrello();
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
        string codTagCombined = hddTagCombined.Value;

        AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name, codTagCombined);
        if (Session["superamentoquantita"] != null)
        {
            int qtamod = 0;
            int.TryParse(Session["superamentoquantita"].ToString(), out qtamod);
            Session.Remove("superamentoquantita");
            quantita = qtamod;
            output.Text = references.ResMan("Common", Lingua, "testoCarellosuperamentoquantita"); //Resources.Common.testoCarellosuperamentoquantita;
            if (Session["nontrovata"] != null)
            {
                Session.Remove("nontrovata");
                output.Text = references.ResMan("Common", Lingua, "testoCarellononesistente");// Resources.Common.testoCarellononesistente;

            }
        }
        txtQuantita_temp.Value = quantita.ToString();

        //QUI DEVI FARE L'AGGIORNAMENTO DEI RIEPILOGHI DEL CARRELLO NELLA MASTER!!!!->
        AggiornaVisualizzazioneDatiCarrello();
        //AssociaDati();

    }

    protected void btnDelete(object sender, EventArgs e)
    {
        HtmlInputText txtQuantita_temp = (HtmlInputText)((LinkButton)sender).FindControl("txtQuantita");
        string Idtext = ((LinkButton)sender).CommandArgument.ToString();
        int idprodotto = 0;
        int.TryParse(Idtext, out idprodotto);
        int quantita = 0;
        int.TryParse(txtQuantita_temp.Value, out quantita);
        string codTagCombined = hddTagCombined.Value;

        AggiornaProdottoCarrello(Request, Session, idprodotto, quantita, User.Identity.Name, codTagCombined);

        txtQuantita_temp.Value = "0";
        AggiornaVisualizzazioneDatiCarrello();
        // AssociaDati();

    }
    protected void btnContatti_Click(object sender, EventArgs e)
    {
        try
        {
            //Prepariamo e inviamo il mail
            string nomemittente = txtContactName.Value;
            string cognomemittente = txtContactCognome.Value;
            string mittenteMail = txtContactEmail.Value;
            string telefono = txtContactPhone.Value;
            string nomedestinatario = Nome;
            string maildestinatario = Email;
            int idperstatistiche = 0;
            string tipo = "informazioni";
            string SoggettoMail = "Richiesta " + tipo + " da " + cognomemittente + "  " + nomemittente + " tramite il sito " + Nome;
            string Descrizione = txtContactMessage.Value.Replace("\r", "<br/>") + " <br/> ";

            Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
            if (item != null && item.Id != 0)
            {
                idperstatistiche = item.Id;
                Descrizione = Descrizione.Insert(0, item.DenominazioneI.Replace("\r", "<br/>") + " <br/> ");
            }
            Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome o rag soc. Cliente: " + cognomemittente;
            Descrizione += " <br/> Telefono Cliente:  " + telefono + "   Email Cliente: " + mittenteMail + " Lingua Cliente: " + Lingua;
            Descrizione += " <br/> Confermo l'autorizzazione al trattamento dei miei dati personali (D.Lgs 196/2003)";

            if (chkContactPrivacy.Checked)
            {
                Utility.invioMailGenerico(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

                //Registro la statistica di contatto
                Statistiche stat = new Statistiche();
                stat.Data = DateTime.Now;
                stat.EmailDestinatario = maildestinatario;
                stat.EmailMittente = mittenteMail;
                stat.Idattivita = idperstatistiche;
                stat.Testomail = cognomemittente + " " + nomemittente + "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                stat.Url = "";
                statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);


                Response.Redirect(CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkContatti")) + "&idOfferta=" + idOfferta.ToString() + "&conversione=true");

            }
            else
            {


                outputContact.Text = references.ResMan("Common", Lingua, "txtPrivacyError");
                //Mittente.Descrizione += " <br/> Non vi Autorizzo al trattamento dei miei dati personali (D.Lgs 196/2003)";
            }

        }
        catch (Exception err)
        {
            outputContact.Text = err.Message + " <br/> ";
            outputContact.Text += references.ResMan("Common", Lingua, "txtMailError");
        }
    }

    protected void btnContatti1_Click(object sender, EventArgs e)
    {
        try
        {
            //Prepariamo e inviamo il mail
            string nomemittente = txtContactName1.Value;
            string cognomemittente = txtContactCognome1.Value;
            string mittenteMail = txtContactEmail1.Value;
            string telefono = txtContactPhone1.Value;
            string nomedestinatario = Nome;
            string maildestinatario = Email;
            int idperstatistiche = 0;
            string tipo = "informazioni";
            string SoggettoMail = "Richiesta " + tipo + " da " + cognomemittente + "  " + nomemittente + " tramite il sito " + Nome;
            string Descrizione = txtContactMessage1.Value.Replace("\r", "<br/>") + " <br/> ";

            Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
            if (item != null && item.Id != 0)
            {
                idperstatistiche = item.Id;
                Descrizione = Descrizione.Insert(0, item.DenominazioneI.Replace("\r", "<br/>") + " <br/> ");
            }
            Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome o rag soc. Cliente: " + cognomemittente;
            Descrizione += " <br/> Telefono Cliente:  " + telefono + "   Email Cliente: " + mittenteMail + " Lingua Cliente: " + Lingua;
            Descrizione += " <br/> Confermo l'autorizzazione al trattamento dei miei dati personali (D.Lgs 196/2003)";

            if (chkContactPrivacy1.Checked)
            {
                Utility.invioMailGenerico(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

                //Registro la statistica di contatto
                Statistiche stat = new Statistiche();
                stat.Data = DateTime.Now;
                stat.EmailDestinatario = maildestinatario;
                stat.EmailMittente = mittenteMail;
                stat.Idattivita = idperstatistiche;
                stat.Testomail = cognomemittente + " " + nomemittente + "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                stat.Url = "";
                statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);


                Response.Redirect(CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua, "LinkContatti")) + "&idOfferta=" + idOfferta.ToString() + "&conversione=true");

            }
            else
            {


                outputContact1.Text = references.ResMan("Common", Lingua, "txtPrivacyError");
                //Mittente.Descrizione += " <br/> Non vi Autorizzo al trattamento dei miei dati personali (D.Lgs 196/2003)";
            }

        }
        catch (Exception err)
        {
            outputContact1.Text = err.Message + " <br/> ";
            outputContact1.Text += references.ResMan("Common", Lingua, "txtMailError");
        }
    }


}