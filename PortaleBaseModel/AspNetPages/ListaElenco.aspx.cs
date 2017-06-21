using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;
using System.Data.OleDb;

public partial class AspNetPages_ListaElenco : CommonPage
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
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    public string Tipologia
    {
        get { return ViewState["Tipologia"] != null ? (string)(ViewState["Tipologia"]) : "rif000199"; }
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
    string regioneperrepeater = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //http://localhost:8888/test/articoli/rif000002-I-testoperindicizzazione.aspx

                PageGuid = System.Guid.NewGuid().ToString();

                //Creo l'equivalente di ~/ nel ViewState per usarlo nel javascript della pagina
                PercorsoAssolutoApplicazione = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione;
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;

                //Prendiamo i dati dalla querystring (Lingua) o dal Context ( caso di url rewriting )
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
                Tipologia = CaricaValoreMaster(Request, Session, "Tipologia", false);
                Categoria = CaricaValoreMaster(Request, Session, "Categoria", false);
                Categoria2liv = CaricaValoreMaster(Request, Session, "Categoria2liv", false);

                //In caso di richiesta specifica di una pagina dei risultati la prendo dalla querystring
                //Pagina = CaricaValoreMaster(Request, Session, "Pagina", true, "1");
                Pagina = "1";
                int _p = 0;
                if (int.TryParse(Pagina, out _p))
                { PagerRisultati.CurrentPage = _p; }

                CaricaControlliJS();

                //#region SEZIONE MASTERPAGE GESTIONE
                //if (!string.IsNullOrEmpty(Tipologia))
                //    Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, Tipologia, false, Lingua);
                //else
                //    Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);

                //#endregion

                CaricaDllLocalizzazione("IT", "", "", "", ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);


                AssociaDati();

               //Inizializziamo le etichette dei controlli in base alla lingua
               //InizializzaEtichette();
               //InizializzaMeta();

               DataBind();
            }
            else
            {
            }
        }
        catch (Exception err)
        {
            // output.Text = err.Message;
        }
    }
    public void CaricaControlliJS()
    {
        ClientScriptManager cs = Page.ClientScript;

        //Carico la galleria in masterpage corretta
        string controllistBanHead = "";
        if (Tipologia == "")
            controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,300);";
        else
            controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','" + Tipologia + "',false,2000,300);";

        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {
            cs.RegisterStartupScript(this.GetType(), "controllistBanHead", controllistBanHead, true);
        }
    }
    protected string CrealistaFiles(object id, object lista)
    {
        string html = "";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (lista != null)
        {
            //sb.Append(" <ul class=\"list-inline\">");
            // sb.Append("<div class=\"col-md-4\">");

            foreach (Allegato a in (AllegatiCollection)lista)
            {

                if (!(a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                {
                    string link = PercorsoFiles + "/" + Tipologia + "/" + id.ToString() + "/" + a.NomeFile;
                    link = CommonPage.ReplaceAbsoluteLinks(link);
                    string descrizione = a.Descrizione;
                    if (string.IsNullOrWhiteSpace(descrizione)) descrizione = a.NomeFile;

                    //sb.Append("<li>");
                    //sb.Append("<a class=\"linked\" target=\"_blank\" href=\"" + link + "\"><i class=\"fa  fa-eye  color-green\"></i>" + descrizione + "</a>");
                    //sb.Append("</li>");
                    sb.Append("<a style=\"margin-right:10px;margin-bottom:10px;min-width:190px\" class=\"btn-sm btn-primary\" target=\"_blank\" href=\"" + link + "\"><i class=\"fa fa-search\"></i>" + descrizione + "</a>");


                }

            }
            //sb.Append("</ul>");
            // sb.Append("</div>");


        }
        html = sb.ToString();
        return html;
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


    protected bool ControlloVisibilita(object fotos)
    {
        bool ret = true;
        if (fotos == null || ((AllegatiCollection)fotos).Count <= 0) ret = false;
        bool onlypdf = (fotos != null && ((AllegatiCollection)fotos).Count > 0 && !((AllegatiCollection)fotos).Exists(c => (c.NomeFile.ToString().ToLower().EndsWith("jpg") || c.NomeFile.ToString().ToLower().EndsWith("gif") || c.NomeFile.ToString().ToLower().EndsWith("png"))));
        if (onlypdf) ret = false;
        return ret;
    }

    protected void rptListFiles_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                Offerte item = (Offerte)(e.Item.DataItem);
                if (regioneperrepeater != item.DataInserimento.Date.Year.ToString())
                {
                    regioneperrepeater = item.CodiceRegione;
                    //QUI CONTROLLO ED EVIDENZIO LA RIGA DEL REPEATER ATTUALMENTE SELEZIONATA
                    Literal litTitle = (Literal)e.Item.FindControl("litTitle");
                    HtmlGenericControl divTitle = (HtmlGenericControl)e.Item.FindControl("divTitle");
                    litTitle.Text = item.DataInserimento.Date.Year.ToString().ToUpper();
                    divTitle.Visible = true;
                }
            }
        }
    }

    protected void rptSoci_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                Offerte item = (Offerte)(e.Item.DataItem);
                if (regioneperrepeater != item.CodiceRegione && item.CodiceRegione.Trim() != "")
                {
                    regioneperrepeater = item.CodiceRegione;
                    //QUI CONTROLLO ED EVIDENZIO LA RIGA DEL REPEATER ATTUALMENTE SELEZIONATA
                    Literal litRegione = (Literal)e.Item.FindControl("litRegione");
                    HtmlGenericControl divTitle = (HtmlGenericControl)e.Item.FindControl("divTitle");
                    litRegione.Text = NomeRegione(item.CodiceRegione, Lingua).ToUpper();
                    divTitle.Visible = true;
                }
                if (item.CodiceRegione.Trim() == "")
                {
                    //QUI CONTROLLO ED EVIDENZIO LA RIGA DEL REPEATER ATTUALMENTE SELEZIONATA
                    Literal litRegione = (Literal)e.Item.FindControl("litRegione");
                    HtmlGenericControl divTitle = (HtmlGenericControl)e.Item.FindControl("divTitle");
                    litRegione.Text = "ALTRE NAZIONI";
                    divTitle.Visible = true;
                }
            }
        }
    }
    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (e.Item.DataItem != null)
            {
                Offerte item = (Offerte)(e.Item.DataItem);
                if (regioneperrepeater != item.CodiceRegione)
                {
                    regioneperrepeater = item.CodiceRegione;
                    //QUI CONTROLLO ED EVIDENZIO LA RIGA DEL REPEATER ATTUALMENTE SELEZIONATA
                    Literal litRegione = (Literal)e.Item.FindControl("litRegione");
                    HtmlGenericControl divTitle = (HtmlGenericControl)e.Item.FindControl("divTitle");
                    litRegione.Text = NomeRegione(item.CodiceRegione, Lingua).ToUpper();
                    divTitle.Visible = true;
                }
            }
        }
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
    private void AssociaDati(string testoricerca = "", string naz = "", string reg = "", string pro = "", string com = "")
    {
        //Eseguiamo la ricerca richiesta
        //Prendiamo la lista completa delle offerte con tutti dati relativi
        //filtrandoli in base ai parametri richiesti
        OfferteCollection offerte = new OfferteCollection();
        string tipologia = Tipologia;
        btnNext.Text = GetGlobalResourceObject("Common", "txtTastoNextPartners").ToString();
        btnPrev.Text = GetGlobalResourceObject("Common", "txtTastoPrevPartners").ToString();

        //InizializzaEtichette();
        #region Versione con db ACCESS
        List<OleDbParameter> parColl = new List<OleDbParameter>();
#if true

        if (tipologia != "" && tipologia != "-")
        {
            OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", tipologia);
            parColl.Add(p3);
        }

        if (testoricerca.Trim() != "")
        {
            testoricerca = testoricerca.Replace(" ", "%");
            OleDbParameter p7 = new OleDbParameter("@testoricerca", "%" + testoricerca + "%");
            parColl.Add(p7);
        }
        //Per ora non filtro le nazioni
        //if (ddlNazioneRicerca.SelectedValue.Trim() != "")
        //{
        //    string valorenaz = ddlNazioneRicerca.SelectedValue.Trim().Replace(" ", "%");
        //    OleDbParameter pstringafiltronaz = new OleDbParameter("@CodiceNAZIONE", "%" + valorenaz + "%");
        //    parColl.Add(pstringafiltronaz);
        //}
        bool flagfiltroregione = false;
        if (ddlRegioneRicerca.SelectedValue.Trim() != "")
        {
            flagfiltroregione = true;
            string valorereg = ddlRegioneRicerca.SelectedValue.Trim().Replace(" ", "%");
            OleDbParameter pstringafiltroreg = new OleDbParameter("@CodiceREGIONE", "%" + valorereg + "%");
            parColl.Add(pstringafiltroreg);
        }
        if (ddlProvinciaRicerca.SelectedValue.Trim() != "")
        {
            string valorepro = ddlProvinciaRicerca.SelectedValue.Trim().Replace(" ", "%");
            OleDbParameter pstringafiltropro = new OleDbParameter("@CodicePROVINCIA", "%" + valorepro + "%");
            parColl.Add(pstringafiltropro);
        }
        //Per ora non filtro i comuni
        //if (ddlComuneRicerca.SelectedValue.Trim() != "")
        //{
        //    string valorecom = ddlComuneRicerca.SelectedValue.Trim().Replace(" ", "%");
        //    OleDbParameter pstringafiltrocom = new OleDbParameter("@CodiceCOMUNE", "%" + valorecom + "%");
        //    parColl.Add(pstringafiltrocom);
        //}

        if (Categoria.Trim() != "")
        {
            OleDbParameter pcat = new OleDbParameter("@CodiceCategoria", "%" + Categoria + "%");
            parColl.Add(pcat);

        }

        if (Categoria2liv.Trim() != "")
        {
            OleDbParameter pcat2 = new OleDbParameter("@CodiceCategoria2Liv", "%" + Categoria2liv + "%");
            parColl.Add(pcat2);

        }
        offerte = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "5000", Lingua);

        if (offerte != null) //ORDINAMENTO PER REGIONE
            offerte.Sort(new GenericComparer<Offerte>("DataInserimento", System.ComponentModel.ListSortDirection.Descending));

        regioneperrepeater = "";
        if (offerte != null && offerte.Count > 0)
            AssociaDatiSocial(offerte[0]);
#endif
        #endregion
#if true
        SettaTestoIniziale();

        pnlPager.Visible = true;
        if (Tipologia == "rif000199") //Partners ( uso il repeater adatto a seconda dei casi
        {
            Pager<Offerte> p = new Pager<Offerte>(offerte, true, this.Page, PageGuid + PagerRisultati.ClientID);
            PagerRisultati.TotalRecords = p.Count;
            if (p.Count <= PagerRisultati.PageSize) pnlPager.Visible = false; //Spengo il pager se c'è una sola pagina

            try
            {
                PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
                //PagerRisultatiLow.CurrentPage = Convert.ToInt32(Pagina);
            }
            catch
            {
                Pagina = "1";
            }

            rptList1.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
            rptList1.DataBind();
        }
        else if (Tipologia == "rif000061" || Tipologia == "rif000062")  //Repeater con raggruppamento per regione
        {
            if (offerte != null) //ORDINAMENTO PER REGIONE
                offerte.Sort(new GenericComparer<Offerte>("CodiceRegione", System.ComponentModel.ListSortDirection.Ascending));
            List<Offerte> sociperregioneconripetizioni = new List<Offerte>();
            //Prendiamo la lista delle regioni!
            WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
            List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.SiglaNazione.ToLower() == "it"); });
            if (provincelingua != null)
            {
                provincelingua.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                foreach (Province item in provincelingua)
                {
                    if (!regioni.Exists(delegate (Province tmp) { return (tmp.Regione == item.Regione); }))
                        regioni.Add(item);
                }
                regioni.Sort(new GenericComparer<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending));
            }

            //SE FILTRO PER REGIONE -> VISUALIZZO SOLO QUELLA
            if (flagfiltroregione == true)
                regioni.RemoveAll(r => r.Codice != ddlRegioneRicerca.SelectedValue.Trim());

            foreach (Province r in regioni)
            {
                ListItem i = new ListItem(r.Regione, r.Codice);
                //Creaimo una lista per il repeater ordinata per regione con la ripetizione dei soci corretta 
                List<Offerte> sociregione = offerte.FindAll(o => (o.CodiceNAZIONE1_dts.ToLower() == "it" && o.CodiceREGIONE1_dts == r.Codice) || (o.CodiceNAZIONE2_dts.ToLower() == "it" && o.CodiceREGIONE2_dts == r.Codice) || (o.CodiceNAZIONE3_dts.ToLower() == "it" && o.CodiceREGIONE3_dts == r.Codice));
                sociregione.Sort(new GenericComparer<Offerte>("Denominazione" + Lingua, System.ComponentModel.ListSortDirection.Ascending));
                //sociregione.Sort(new GenericComparer2<Offerte>("Cognome_dts", System.ComponentModel.ListSortDirection.Ascending, "Nome_dts", System.ComponentModel.ListSortDirection.Descending));
                //Imposto la regione per la visualizzazione nel repeater
                foreach (Offerte o in sociregione)
                {
                    Offerte oclone = new Offerte(o);
                    oclone.CodiceRegione = r.Codice;
                    sociperregioneconripetizioni.Add(oclone);
                }
            }
            if (flagfiltroregione == false)
            {  //Qui dovresti aggiungere i soci con nazione non italia che sono esclusi dalla visualizzazione
               //List<Offerte> socialtrenazioni = offerte.FindAll(o => (o.CodiceNAZIONE1_dts.ToLower() != "it") || (o.CodiceNAZIONE2_dts.ToLower() != "it") || (o.CodiceNAZIONE3_dts.ToLower() != "it"));
               //socialtrenazioni.Sort(new GenericComparer2<Offerte>("Cognome_dts", System.ComponentModel.ListSortDirection.Ascending, "Nome_dts", System.ComponentModel.ListSortDirection.Descending));

                List<Offerte> socialtrenazioni = offerte.FindAll(o => (o.CodiceNAZIONE1_dts.ToLower() != "it") );
                //socialtrenazioni.Sort(new GenericComparer2<Offerte>("Cognome_dts", System.ComponentModel.ListSortDirection.Ascending, "Nome_dts", System.ComponentModel.ListSortDirection.Descending));
                socialtrenazioni.Sort(new GenericComparer<Offerte>("Denominazione" + Lingua, System.ComponentModel.ListSortDirection.Ascending));
                foreach (Offerte o in socialtrenazioni)
                {
                    Offerte oclone = new Offerte(o);
                    oclone.CodiceRegione = ""; //qui andrebbe messa un qualcosa per identificare la nazione!!!
                    sociperregioneconripetizioni.Add(oclone);
                }
            }


            //sociperregioneconripetizioni.RemoveAll(s => s.CodiceRegione=="" && )

            pnlPager.Visible = true;
            Pager<Offerte> p = new Pager<Offerte>(sociperregioneconripetizioni, true, this.Page, PageGuid + PagerRisultati.ClientID);
            PagerRisultati.TotalRecords = p.Count;
            if (p.Count <= PagerRisultati.PageSize) pnlPager.Visible = false; //Spengo il pager se c'è una sola pagina

            try
            {
                PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
                //PagerRisultatiLow.CurrentPage = Convert.ToInt32(Pagina);
            }
            catch
            {
                Pagina = "1";
            }
            divRicercaSoci.Visible = false;
            rptSoci.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
            rptSoci.DataBind();
        }
        else if (Tipologia == "rif000051" || Tipologia == "rif000101")
        {
            if (Tipologia == "rif000101")
            {
                divColCenter.Attributes["class"] = "col-md-8 col-sm-8";
                divColright.Attributes["class"] = "col-md-3 col-sm-3";
                divColright.Visible = true;
            }
            Pager<Offerte> p = new Pager<Offerte>(offerte, true, this.Page, PageGuid + PagerRisultati.ClientID);
            PagerRisultati.TotalRecords = p.Count;
            if (p.Count <= PagerRisultati.PageSize) pnlPager.Visible = false; //Spengo il pager se c'è una sola pagina

            try
            {
                PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
                //PagerRisultatiLow.CurrentPage = Convert.ToInt32(Pagina);
            }
            catch
            {
                Pagina = "1";
            }

            rptListFiles.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
            rptListFiles.DataBind();
        }

        //else  //Repeater con raggruppamento per regione
        //{
        //    if (offerte != null) //ORDINAMENTO PER REGIONE
        //        offerte.Sort(new GenericComparer<Offerte>("CodiceRegione", System.ComponentModel.ListSortDirection.Ascending));
        //    Pager<Offerte> p = new Pager<Offerte>(offerte, true, this.Page, PageGuid + PagerRisultati.ClientID);
        //    PagerRisultati.TotalRecords = p.Count;
        //    if (p.Count <= PagerRisultati.PageSize) pnlPager.Visible = false; //Spengo il pager se c'è una sola pagina
        //    try
        //    {
        //        PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
        //        //PagerRisultatiLow.CurrentPage = Convert.ToInt32(Pagina);
        //    }
        //    catch
        //    {
        //        Pagina = "1";
        //    }
        //    rptList.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
        //    rptList.DataBind();
        //}
        //else if (Tipologia == "rif000100" || Tipologia == "rif000101")
        //{
        //    rptListFiles.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
        //    rptListFiles.DataBind();
        //}


        //else if (Tipologia == "rif000100")  //Repeater con raggruppamento per regione
        //{
        //    List<Offerte> sociperregioneconripetizioni = new List<Offerte>();
        //    //Prendiamo la lista delle regioni!
        //    WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
        //    List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate(Province tmp) { return (tmp.Lingua == Lingua && tmp.SiglaNazione.ToLower() == "it"); });
        //    if (provincelingua != null)
        //    {
        //        provincelingua.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
        //        foreach (Province item in provincelingua)
        //        {
        //            if (!regioni.Exists(delegate(Province tmp) { return (tmp.Regione == item.Regione); }))
        //                regioni.Add(item);
        //        }
        //        regioni.Sort(new GenericComparer<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending));
        //    }

        //    //SE FILTRO PER REGIONE -> VISUALIZZO SOLO QUELLA
        //    if (flagfiltroregione == true)
        //        regioni.RemoveAll(r => r.Codice != ddlRegioneRicerca.SelectedValue.Trim());

        //    foreach (Province r in regioni)
        //    {
        //        ListItem i = new ListItem(r.Regione, r.Codice);
        //        //Creaimo una lista per il repeater ordinata per regione con la ripetizione dei soci corretta 
        //        List<Offerte> sociregione = offerte.FindAll(o => (o.CodiceNAZIONE1_dts.ToLower() == "it" && o.CodiceREGIONE1_dts == r.Codice) || (o.CodiceNAZIONE2_dts.ToLower() == "it" && o.CodiceREGIONE2_dts == r.Codice) || (o.CodiceNAZIONE3_dts.ToLower() == "it" && o.CodiceREGIONE3_dts == r.Codice));
        //        //Imposto la regione per la visualizzazione nel repeater
        //        foreach (Offerte o in sociregione)
        //        {
        //            Offerte oclone = new Offerte(o);
        //            oclone.CodiceRegione = r.Codice;
        //            sociperregioneconripetizioni.Add(oclone);
        //        }
        //    }
        //    if (flagfiltroregione == false)
        //    {  //Qui dovresti aggiungere i soci con nazione non italia che sono esclusi dalla visualizzazione
        //        List<Offerte> socialtrenazioni = offerte.FindAll(o => (o.CodiceNAZIONE1_dts.ToLower() != "it") || (o.CodiceNAZIONE2_dts.ToLower() != "it") || (o.CodiceNAZIONE3_dts.ToLower() != "it"));
        //        foreach (Offerte o in socialtrenazioni)
        //        {
        //            Offerte oclone = new Offerte(o);
        //            oclone.CodiceRegione = ""; //qui andrebbe messa un qualcosa per identificare la nazione!!!
        //            sociperregioneconripetizioni.Add(oclone);
        //        }
        //    }
        //    pnlPager.Visible = true;
        //    p = new Pager<Offerte>(sociperregioneconripetizioni, true, this.Page, PageGuid + PagerRisultati.ClientID);
        //    PagerRisultati.TotalRecords = p.Count;
        //    if (p.Count <= PagerRisultati.PageSize) pnlPager.Visible = false; //Spengo il pager se c'è una sola pagina

        //    try
        //    {
        //        PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
        //        //PagerRisultatiLow.CurrentPage = Convert.ToInt32(Pagina);
        //    }
        //    catch
        //    {
        //        Pagina = "1";
        //    }
        //    divRicercaSoci.Visible = true;
        //    rptSoci.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
        //    rptSoci.DataBind();
        //}
        //if (Tipologia == "rif000100")   //Repeater con raggruppamento per regione
        //{

        //    rptList.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
        //    rptList.DataBind();
        //}
#endif

    }


    private void SettaTestoIniziale()
    {
        WelcomeLibrary.DOM.TipologiaOfferte sezione = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == "I" & tmp.Codice == Tipologia); });
        WelcomeLibrary.DOM.TipologiaOfferte sezione_inlingua = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua & tmp.Codice == Tipologia); });
        if (sezione != null)
        {
            EvidenziaSelezione(sezione.Descrizione);
            string titolopagina = sezione_inlingua.Descrizione.ToUpper();
            litNomePagina.Text = titolopagina;
            //Prodotto catselected = Utility.ElencoProdotti.Find(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == Tipologia && tmp.CodiceProdotto == Categoria)); });
            //if (catselected != null)
            //{
            //    titolopagina = catselected.Descrizione;
            //    litNomePagina.Text += " " + titolopagina;
            //}


            string htmlPage = "";
            if (GetGlobalResourceObject("Common", "testo" + Tipologia) != null)
                htmlPage = GetGlobalResourceObject("Common", "testo" + Tipologia).ToString();

            if (GetGlobalResourceObject("Common", "testo" + Categoria) != null)
                htmlPage = GetGlobalResourceObject("Common", "testo" + Categoria).ToString();
            string strigaperricerca = ""; //Request.Url.AbsolutePath
            if (!string.IsNullOrEmpty(Categoria))
                strigaperricerca = "/" + Tipologia + "/" + Categoria + "/"; //Request.Url.AbsolutePath

            Contenuti content = content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            if (content == null && !string.IsNullOrEmpty(titolopagina))
                strigaperricerca = "/" + Tipologia + "/" + CleanUrl(titolopagina); //Request.Url.AbsolutePath
            content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
            if (content != null && content.Id != 0)
            {
                htmlPage = ReplaceLinks(content.DescrizionebyLingua(Lingua));
            }
            WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();   //;
            string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(htmlPage, 90, true)).Replace("<br/>", " ").Trim());
            ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;

            litTextHeadPage.Text = ReplaceAbsoluteLinks(ReplaceLinks(htmlPage));
        }
    }


    protected string VisualizzaPosizione(string codiceregione, object item)
    {
        string html = "";
        Offerte socio = (Offerte)item;
        //Prendiamo gli indirizzi da visualizzare

        if (codiceregione != "" && (socio.CodiceNAZIONE1_dts.ToLower() == "it" || socio.CodiceNAZIONE2_dts.ToLower() == "it" || socio.CodiceNAZIONE3_dts.ToLower() == "it"))
        {
            if (socio.CodiceREGIONE1_dts == codiceregione)
            {
                Province _p = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice.ToLower() == socio.CodicePROVINCIA1_dts); });
                if (_p != null)
                {
                    html += socio.Via1_dts + " " + socio.Cap1_dts + " " + socio.CodiceCOMUNE1_dts + " (" + _p.SiglaProvincia + ")";
                }
            }
            if (socio.CodiceREGIONE2_dts == codiceregione)
            {
                Province _p = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice.ToLower() == socio.CodicePROVINCIA2_dts); });
                if (_p != null)
                {
                    html += socio.Via2_dts + " " + socio.Cap2_dts + " " + socio.CodiceCOMUNE2_dts + " (" + _p.SiglaProvincia + ")";
                }
            }
            if (socio.CodiceREGIONE3_dts == codiceregione)
            {
                Province _p = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice.ToLower() == socio.CodicePROVINCIA3_dts); });
                if (_p != null)
                {
                    html += socio.Via3_dts + " " + socio.Cap3_dts + " " + socio.CodiceCOMUNE3_dts + " (" + _p.SiglaProvincia + ")";
                }
            }
        }
        else //Altre nazioni
        {

            if (socio.CodiceNAZIONE1_dts.ToLower() != "it")
            {

                html += socio.Via1_dts + " " + socio.Cap1_dts + " " + socio.CodiceCOMUNE1_dts + " " + socio.CodicePROVINCIA1_dts + " " + socio.CodiceREGIONE1_dts + " " + socio.CodiceNAZIONE1_dts + "<br/> ";

            }
            if (socio.CodiceNAZIONE2_dts.ToLower() != "it")
            {

                html += socio.Via2_dts + " " + socio.Cap2_dts + " " + socio.CodiceCOMUNE2_dts + " " + socio.CodicePROVINCIA2_dts + " " + socio.CodiceREGIONE2_dts + " " + socio.CodiceNAZIONE2_dts + "<br/> ";

            }
            if (socio.CodiceNAZIONE3_dts.ToLower() != "it")
            {

                html += socio.Via3_dts + "" + socio.Cap3_dts + " " + socio.CodiceCOMUNE3_dts + " " + socio.CodicePROVINCIA3_dts + " " + socio.CodiceREGIONE3_dts + " " + socio.CodiceNAZIONE3_dts + "<br/> ";

            }

        }

        return html;
    }

    protected void AssociaDatiSocial(Offerte data)
    {
        //HtmlGenericControl divSocial = (HtmlGenericControl)e.Item.FindControl("divSocial");
        //divSocial.Attributes.Add("addthis:url", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/SchedaContenutoMaster.aspx?idOfferta=" + data.Id + "&CodiceTipologia=" + data.CodiceTipologia + "&Lingua=" + Lingua);
        //divSocial.Attributes.Add("addthis:title", WelcomeLibrary.UF.Utility.SostituisciTestoACapo(data.DenominazioneI) + " " + Nome);
        //divSocial.Attributes.Add("addthis:description", CommonPage.ReplaceLinks(ConteggioCaratteri(data.DescrizioneI, 90)));

        WelcomeLibrary.DOM.TipologiaOfferte sezione = WelcomeLibrary.UF.Utility.TipologieOfferte.Find(delegate (WelcomeLibrary.DOM.TipologiaOfferte tmp) { return (tmp.Lingua == Lingua & tmp.Codice == Tipologia); });
        string sezionedescrizione = "";
        if (sezione != null)
            sezionedescrizione = sezione.Descrizione;

        string descrizione = data.DescrizionebyLingua(Lingua);
        string denominazione = data.DenominazionebyLingua(Lingua);

        WelcomeLibrary.HtmlToText html = new WelcomeLibrary.HtmlToText();   //;

        //Titolo e descrizione pagina
        ((HtmlTitle)Master.FindControl("metaTitle")).Text = html.Convert(Nome + " " + WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione).Trim());

        string simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 90, true)).Replace("<br/>", " ").Trim());
        ((HtmlMeta)Master.FindControl("metaDesc")).Content = simpletext;

        //Opengraph per facebook
        ((HtmlMeta)Master.FindControl("metafbTitle")).Content = html.Convert(WelcomeLibrary.UF.Utility.SostituisciTestoACapo(sezionedescrizione + " " + Nome));
        simpletext = html.Convert(CommonPage.ReplaceLinks(ConteggioCaratteri(descrizione, 90, true))).Replace("<br/>", " ").Trim();
        ((HtmlMeta)Master.FindControl("metafbdescription")).Content = simpletext;


        //((HtmlMeta)Master.FindControl("metafbimage")).Content = ComponiUrlAnteprima(data.FotoCollection_M.FotoAnteprima, data.CodiceTipologia, data.Id.ToString()).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);

        //if (data.FotoCollection_M != null && !string.IsNullOrEmpty(data.FotoCollection_M.FotoAnteprima))
        //    ((HtmlMeta)Master.FindControl("metafbimage")).Content = ComponiUrlAnteprima(data.FotoCollection_M.FotoAnteprima, data.CodiceTipologia, data.Id.ToString()).Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
        //else if (data.FotoCollection_M != null && !string.IsNullOrEmpty(data.linkVideo))
        //    ((HtmlMeta)Master.FindControl("metafbvideourl")).Content = data.linkVideo;

        string linkcanonico = CreaLinkRoutes(null, false, Lingua, CleanUrl(denominazione), "", data.CodiceTipologia);

        Literal litgeneric = ((Literal)Master.FindControl("litgeneric"));
        litgeneric.Text = "<link rel=\"canonical\" href=\"" + ReplaceAbsoluteLinks(linkcanonico) + "\"/>";
    }


    protected void EvidenziaSelezione(string testolink)
    {
        HtmlAnchor linkmenu = ((HtmlAnchor)Master.FindControl("link" + testolink));


        try
        {
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
    protected void btnContatti1_Click(object sender, EventArgs e)
    {
        try
        {
            //Prepariamo e inviamo il mail
            string nomemittente = txtContactName.Value;
            string cognomemittente = txtContactCognome.Value;
            string mittenteMail = txtContactEmail.Value;
            string telefono = "";// txtContactPhone.Value;
            string nomedestinatario = Nome;
            string maildestinatario = Email;
            int idperstatistiche = 0;
            string tipo = "testimonianze";
            string SoggettoMail = "Inserimento " + tipo + " da " + cognomemittente + "  " + nomemittente + " tramite il sito " + Nome;
            string Descrizione = txtContactMessage.Value.Replace("\r", "<br/>") + " <br/> ";

            //Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
            //if (item != null && item.Id != 0)
            //{
            //    idperstatistiche = item.Id;
            //    Descrizione.Insert(0, item.DenominazioneI.Replace("\r", "<br/>") + " <br/> ");
            //}
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
                WelcomeLibrary.DAL.statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);


                Response.Redirect(CommonPage.ReplaceAbsoluteLinks(Resources.Common.LinkContatti) + "&conversione=true");

            }
            else
            {
                outputContact.Text = Resources.Common.txtPrivacyError.ToString();
                //Mittente.Descrizione += " <br/> Non vi Autorizzo al trattamento dei miei dati personali (D.Lgs 196/2003)";
            }

        }
        catch (Exception err)
        {
            outputContact.Text = err.Message + " <br/> ";
            outputContact.Text += Resources.Common.txtMailError.ToString();
        }
    }

    #region PARTE RELATIVA ALLA PAGINAZIONE DEL REPEATER
    protected void btnPrev_click(object sender, EventArgs e)
    {
        int pag = PagerRisultati.CurrentPage;
        pag++;
        if (pag > PagerRisultati.totalPages) pag = PagerRisultati.totalPages;
        Pagina = pag.ToString();
        //Session["Pagina"] = Pagina;
        AssociaDati();
    }

    protected void btnNext_click(object sender, EventArgs e)
    {

        int pag = PagerRisultati.CurrentPage;
        pag--;
        if (pag < 1) pag = 1;
        Pagina = pag.ToString();
        //Session["Pagina"] = Pagina;

        AssociaDati();
    }
    protected void PagerRisultati_PageCommand(object sender, string PageNum)
    {
        PagerRisultati.CurrentPage = Convert.ToInt32(PageNum);
        Pagina = PageNum;
        //Session["Pagina"] = Pagina;

        Pager<Offerte> p = new Pager<Offerte>();
        if (p.LoadFromCache(this, PageGuid + PagerRisultati.ClientID))
        {

            if (Tipologia == "rif000199") //Partners ( uso il repeater adatto a seconda dei casi
            {
                rptList1.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
                rptList1.DataBind();
            }
            //else if (Tipologia == "rif000100" || Tipologia == "rif000101")
            //{
            //    rptListFiles.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
            //    rptListFiles.DataBind();
            //}
            else if (Tipologia == "rif000051" || Tipologia == "rif000101")
            {
                regioneperrepeater = "";
                rptListFiles.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
                rptListFiles.DataBind();
            }
            else if (Tipologia == "rif000061")
            {
                regioneperrepeater = "";
                rptSoci.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
                rptSoci.DataBind();
            }
            else //Repeater con raggruppamento per regione
            {
                regioneperrepeater = "";
                rptList.DataSource = p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize);
                rptList.DataBind();
            }
        }
        else
        {
            AssociaDati();
        }
    }
    protected void PagerRisultati_PageGroupClickNext(object sender, string spare)
    {

        //PagerRisultatiLow.nGruppoPagine += 1;

    }
    protected void PagerRisultati_PageGroupClickPrev(object sender, string spare)
    {
        //PagerRisultatiLow.nGruppoPagine -= 1;


    }
    protected void PagerRisultatiLow_PageGroupClickNext(object sender, string spare)
    {
        PagerRisultati.nGruppoPagine += 1;

    }
    protected void PagerRisultatiLow_PageGroupClickPrev(object sender, string spare)
    {
        PagerRisultati.nGruppoPagine -= 1;

    }



    #endregion

    protected void btnCerca_Click(object sender, EventArgs e)
    {
        string testoricerca = Server.HtmlEncode(txtinputCerca.Text);
        AssociaDati(testoricerca, ddlNazioneRicerca.SelectedValue, ddlRegioneRicerca.SelectedValue, ddlRegioneRicerca.SelectedValue, ddlComuneRicerca.SelectedValue);
    }
    #region GESTIONE CASELLE LOCALIZZAZIONE VERSIONE 2

    public void CaricaDllLocalizzazione(string valorenaz, string valorereg, string valorepr, string valoreco, DropDownList dnaz, DropDownList dreg, DropDownList dpro, DropDownList dcom, HtmlInputControl txtRE, HtmlInputControl txtPR, HtmlInputControl txtCO)
    {
        //Riempimento a cascata
        RiempiDdlNazione(valorenaz, dnaz);
        RiempiDdlRegione(valorereg, dnaz, dreg, txtRE);
        RiempiDdlProvincia(valorepr, dnaz, dreg, dpro, txtPR);
        RiempiDdlComune(valoreco, dnaz, dreg, dpro, dcom, txtCO);
    }

    private void RiempiDdlNazione(string valore, DropDownList dnaz)
    {
        List<Tabrif> nazioni = Utility.Nazioni.FindAll(delegate (Tabrif _nz) { return _nz.Lingua == "I"; });
        nazioni.Sort(new GenericComparer<Tabrif>("Campo1", System.ComponentModel.ListSortDirection.Ascending));
        dnaz.Items.Clear();
        foreach (Tabrif n in nazioni)
        {
            ListItem i = new ListItem(n.Campo1, n.Codice);
            dnaz.Items.Add(i);
        }
        try
        {
            dnaz.SelectedValue = valore.ToUpper();
        }
        catch { AggiungiNazione(valore, dnaz); }
    }
    private void RiempiDdlRegione(string valore, DropDownList dnaz, DropDownList dreg, HtmlInputControl txtRE)
    {
        bool found = false;
        dreg.Items.Clear();
        dreg.Items.Insert(0, "Seleziona Regione");
        dreg.Items[0].Value = "";
        txtRE.Value = valore;
        if (dnaz.SelectedValue == "IT")
        {
            WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
            List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.SiglaNazione.ToLower() == dnaz.SelectedValue.ToLower()); });
            if (provincelingua != null)
            {
                provincelingua.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));
                foreach (Province item in provincelingua)
                {
                    if (item.Lingua == "I")
                        if (!regioni.Exists(delegate (Province tmp) { return (tmp.Regione == item.Regione); }))
                            regioni.Add(item);
                }
            }
            foreach (Province r in regioni)
            {
                ListItem i = new ListItem(r.Regione, r.Codice);
                dreg.Items.Add(i);
            }

            if (dreg.Items.FindByValue(valore) != null)
            {
                dreg.SelectedValue = valore;
                found = true;
            }
            //else
            //{
            //    AggiungiRegione(valore, dreg);
            //}

        }
        if (found && !string.IsNullOrEmpty(valore.Trim()))
        {
            dreg.Visible = true;
            txtRE.Visible = false;
        }
        else
        {
            dreg.Visible = true;
            txtRE.Visible = true;
        }

    }
    private void RiempiDdlProvincia(string valore, DropDownList dnaz, DropDownList dreg, DropDownList dpro, HtmlInputControl txtPR)
    {
        bool found = false;
        dpro.Items.Clear();
        dpro.Items.Insert(0, "Seleziona Provincia");
        dpro.Items[0].Value = "";
        txtPR.Value = valore;
        if (dreg.SelectedValue != "" && dnaz.SelectedValue == "IT")
        {
            Province _tmp = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.Codice == dreg.SelectedValue); });
            if (_tmp != null)
            {
                List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.CodiceRegione == _tmp.CodiceRegione && _tmp.SiglaNazione.ToLower() == dnaz.SelectedValue.ToLower()); });
                provincelingua.Sort(new GenericComparer<Province>("Provincia", System.ComponentModel.ListSortDirection.Ascending));
                foreach (Province r in provincelingua)
                {
                    ListItem i = new ListItem(r.Provincia, r.Codice);
                    dpro.Items.Add(i);
                }
            }
            if (dpro.Items.FindByValue(valore) != null)
            {
                dpro.SelectedValue = valore;
                found = true;
            }
            //else
            //{
            //    AggiungiProvincia(valore, dpro);
            //}
        }
        if (found && !string.IsNullOrEmpty(valore.Trim()))
        {
            dpro.Visible = true;
            txtPR.Visible = false;
        }
        else
        {
            dpro.Visible = true;
            txtPR.Visible = true;
        }
    }
    private void RiempiDdlComune(string valore, DropDownList dnaz, DropDownList dreg, DropDownList dpro, DropDownList dcom, HtmlInputControl txtCO)
    {
        bool found = false;
        dcom.Items.Clear();
        dcom.Items.Insert(0, "Seleziona Comune");
        dcom.Items[0].Value = "";
        txtCO.Value = valore;
        if (dpro.SelectedValue != "" && dnaz.SelectedValue == "IT")
        {
            List<WelcomeLibrary.DOM.Comune> comunilingua = Utility.ElencoComuni.FindAll(delegate (WelcomeLibrary.DOM.Comune tmp) { return (tmp.CodiceIncrocio == dpro.SelectedValue); });
            if (comunilingua != null)
            {
                comunilingua.Sort(new GenericComparer<Comune>("Nome", System.ComponentModel.ListSortDirection.Ascending));
                foreach (Comune r in comunilingua)
                {
                    ListItem i = new ListItem(r.Nome, r.Nome);
                    dcom.Items.Add(i);
                }
            }

            if (dcom.Items.FindByValue(valore) != null)
            {
                dcom.SelectedValue = valore;
                found = true;
            }
            //else AggiungiComune(valore, dcom);

        }
        if (found && !string.IsNullOrEmpty(valore.Trim()))
        {
            dcom.Visible = true;
            txtCO.Visible = false;
        }
        else
        {
            dcom.Visible = true;
            txtCO.Visible = true;
        }
    }
    protected string AggiungiNazione(string codice, DropDownList dnaz)
    {
        try
        {
            //Se il valore non esiste lo aggiungo 
            ListItem selli = new ListItem(codice, codice);
            if (dnaz.Items.FindByValue(codice) == null)
            {
                dnaz.Items.Add(selli);
                dnaz.SelectedValue = codice;
            }
            else
                dnaz.SelectedValue = codice;
        }
        catch { }
        return codice;
    }
    protected string AggiungiRegione(string codice, DropDownList dreg)
    {
        try
        {
            //Se il valore non esiste lo aggiungo 
            ListItem selli = new ListItem(codice, codice);
            if (dreg.Items.FindByValue(codice) == null)
            {
                dreg.Items.Add(selli);
                dreg.SelectedValue = codice;
            }
            else
                dreg.SelectedValue = codice;
        }
        catch { }

        return codice;
    }
    protected string AggiungiProvincia(string codice, DropDownList dpro)
    {
        try
        {
            //Se il valore non esiste lo aggiungo 
            ListItem selli = new ListItem(codice, codice);
            if (dpro.Items.FindByValue(codice) == null)
            {
                dpro.Items.Add(selli);
                dpro.SelectedValue = codice;
            }
            else
                dpro.SelectedValue = codice;
        }
        catch { }

        return codice;
    }
    protected string AggiungiComune(string codice, DropDownList dcom)
    {

        try
        {
            //Se il valore non esiste lo aggiungo 
            ListItem selli = new ListItem(codice, codice);
            if (dcom.Items.FindByValue(codice) == null)
            {
                dcom.Items.Add(selli);
                dcom.SelectedValue = codice;
            }
            else
                dcom.SelectedValue = codice;
        }
        catch { }

        return codice;
    }


    //PER I FILTRI DI RICERCA
    protected void ddlNazioneRicerca_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(value, "", "", "", ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
    }
    protected void ddlRegioneRicerca_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlNazioneRicerca.SelectedValue, value, "", "", ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
    }
    protected void ddlProvinciaRicerca_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlNazioneRicerca.SelectedValue, ddlRegioneRicerca.SelectedValue, value, "", ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
    }
    protected void ddlComuneRicerca_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string value = ((DropDownList)sender).SelectedValue;
        CaricaDllLocalizzazione(ddlNazioneRicerca.SelectedValue, ddlRegioneRicerca.SelectedValue, ddlProvinciaRicerca.SelectedValue, value, ddlNazioneRicerca, ddlRegioneRicerca, ddlProvinciaRicerca, ddlComuneRicerca, txtReRicerca, txtPrRicerca, txtCoRicerca);
    }

    #endregion

}