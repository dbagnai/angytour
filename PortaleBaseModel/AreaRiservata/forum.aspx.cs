using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;
using System.IO;
using System.Drawing.Imaging;

public partial class AreaRiservata_Default : CommonPage
{
    public bool debug = false; //Abilita le funzioni in produzione per l'invio delle email all'inserimento dei post

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
    public Dictionary<int, List<Offerte>> dictRepost = new Dictionary<int, List<Offerte>>();
    public Dictionary<string, Offerte> dictSocibyUsername = new Dictionary<string, Offerte>();
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
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, "I");
                ContenutoPagina = CaricaValoreMaster(Request, Session, "ContenutoPagina");
                idOfferta = CaricaValoreMaster(Request, Session, "idOfferta", true, "");
                string testoindice = CaricaValoreMaster(Request, Session, "testoindice", true, "");

                hidCurrentPostActive.Value = idOfferta;
                 
                 Pagina = "1";
                int _p = 0;
                if (int.TryParse(Pagina, out _p))
                { PagerRisultati.CurrentPage = _p; }

                InizializzaTestiPagina();
                AssociaDati();
            }
            else
            {
                string parameter = Request["__EVENTARGUMENT"];
                if (parameter != null && !parameter.Equals(""))
                {
                    if (parameter == "aggiornavisualizzazione")
                    {
                        AssociaDati();
                    }
                    if (parameter == "aggiornavisualizzazioneconmail")
                    {
                        AssociaDati();

                        InviaMailAvvisoInserimentoArgomento(hidCurrentPostActive.Value);
                    }
                }
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
            if (GetGlobalResourceObject("Common", "testo" + Tipologia) != null)
                htmlPage = GetGlobalResourceObject("Common", "testo" + Tipologia).ToString();
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
    protected void Cerca_Click(object sender, EventArgs e)
    {
        //testoricerca
        string link = CreaLinkRicerca("", Tipologia, "", "", "", "", "", "-", Lingua, Session, true);
        Session.Add("testoricerca", Server.HtmlEncode(inputCerca.Value)); //carico in sessione il parametro da cercare
        Response.Redirect(link);
    }
    protected void AssociaDati()
    {
        //Associamo la lista degli argomenti di discussione
        List<Offerte> list = new List<Offerte>();
        //string idsocio = getidsocio(User.Identity.Name);
        CaricaListaSoci();

        //Carichiamo i dati
        try
        {
            string mese = "";
            string anno = "";
            List<Offerte> listPost = new List<Offerte>();
            List<Offerte> listRepost = new List<Offerte>();
            List<OleDbParameter> parColl = new List<OleDbParameter>();

            //if (tipologia == "") tipologia = "%";
            if (Tipologia != "")
            {
                OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", Tipologia);
                parColl.Add(p3);
            }


            if (!string.IsNullOrEmpty(testoricerca.Trim())) //Caricamento con filtro di testo
            {
                testoricerca = testoricerca.Replace(" ", "%");
                OleDbParameter p7 = new OleDbParameter("@testoricerca", "%" + testoricerca + "%");
                parColl.Add(p7);
                list = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", null, null, "", true);
                Session.Remove("testoricerca");
                if (list != null && list.Count > 0)
                {
                    string idlist = "";
                    //Estraimo i codici post che sono presenti nei risulati di ricerca
                    foreach (Offerte o in list)
                    {
                        if (o.Id_collegato == 0)
                            idlist += o.Id.ToString() + ",";
                        else
                            idlist += o.Id_collegato.ToString() + ",";
                    }
                    idlist = idlist.TrimEnd(',');
                    //CArico tutti i post principali dalla lista id trovata
                    parColl = new List<OleDbParameter>();
                    OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", Tipologia);
                    parColl.Add(p3);
                    OleDbParameter pilist = new OleDbParameter("@IdList", idlist);
                    parColl.Add(pilist);
                    OleDbParameter pcollegato = new OleDbParameter("@Id_collegato", "0");
                    parColl.Add(pcollegato);
                    listPost = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", null, null, "", true);
                    if (listPost != null && listPost.Count > 0)
                    {
                        dictRepost = new Dictionary<int, List<Offerte>>();
                        foreach (Offerte o in listPost)
                        {
                            List<Offerte> orderredrepost = offDM.CaricaOfferteCollegate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o.Id.ToString(), "", false, Lingua, true);
                            if (orderredrepost != null && orderredrepost.Count > 0)
                            {
                                if (!dictRepost.ContainsKey(o.Id))
                                {
                                    orderredrepost.Sort(new GenericComparer<Offerte>("DataInserimento", System.ComponentModel.ListSortDirection.Descending));
                                    dictRepost.Add(o.Id, orderredrepost);
                                }
                            }
                            else dictRepost.Add(o.Id, new List<Offerte>());
                        }
                    }


                }
            }
            else if (idOfferta != "0" && idOfferta != "")   //Se presente filtro per id del post -> carico solo quello
            {
                OleDbParameter pid = new OleDbParameter("@Id", idOfferta);
                parColl.Add(pid);
                OleDbParameter pcollegato = new OleDbParameter("@Id_collegato", "0");
                parColl.Add(pcollegato);
                listPost = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", null, null, "", true);
                dictRepost = new Dictionary<int, List<Offerte>>();
                foreach (Offerte o in listPost)
                {
                    List<Offerte> orderredrepost = offDM.CaricaOfferteCollegate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o.Id.ToString(), "", false, Lingua, true);
                    if (orderredrepost != null && orderredrepost.Count > 0)
                    {
                        if (!dictRepost.ContainsKey(o.Id))
                        {
                            orderredrepost.Sort(new GenericComparer<Offerte>("DataInserimento", System.ComponentModel.ListSortDirection.Descending));
                            dictRepost.Add(o.Id, orderredrepost);
                        }
                    }
                    else dictRepost.Add(o.Id, new List<Offerte>());

                }
            }
            else //Caricamento complessivo di tutti i post
            {
                list = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", null, null, "", true);
                if (list != null)
                {
                    listPost = list.FindAll(c => c.Id_collegato == 0);
                    listRepost = list.FindAll(c => c.Id_collegato != 0);
                }
                dictRepost = new Dictionary<int, List<Offerte>>();
                foreach (Offerte o in listPost)
                {
                    if (!dictRepost.ContainsKey(o.Id))
                    {
                        List<Offerte> orderredrepost = listRepost.FindAll(c => c.Id_collegato == o.Id);
                        orderredrepost.Sort(new GenericComparer<Offerte>("DataInserimento", System.ComponentModel.ListSortDirection.Descending));
                        if (orderredrepost != null)
                            dictRepost.Add(o.Id, orderredrepost);
                        else
                            dictRepost.Add(o.Id, new List<Offerte>());

                    }
                }
            }

#if true
            //Selezionamo i risultati in base al numero di pagina e alla sua dimensione per la paginazione
            //Utilizzando la classe di paginazione

            WelcomeLibrary.UF.Pager<Offerte> _pager = new Pager<Offerte>(listPost, false, this.Page, PageGuid + PagerRisultati.ClientID);

            try
            {
                PagerRisultati.CurrentPage = Convert.ToInt32(Pagina);
            }
            catch
            {
                Pagina = "1";
            }

            int nrecordfiltrati = _pager.Count;
            PagerRisultati.TotalRecords = nrecordfiltrati;
            if (nrecordfiltrati == 0) PagerRisultati.CurrentPage = 1;
#endif
            AssociaDatiRepeater(_pager.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize));


        }
        catch (Exception error)
        {
            output.Text = error.Message.ToString();
            return;
        }


    }
    protected void AssociaDatiRepeater(List<Offerte> list)
    {

        rptPosts.DataSource = list;
        rptPosts.DataBind();
    }
    protected bool ControllaVisibilitaAutore(string Autore)
    {
        bool ret = false;
        if (Autore == User.Identity.Name || ControllaRuolo(User.Identity.Name, "GestorePortale") || ControllaRuolo(User.Identity.Name, "WebMaster"))
            ret = true;
        return ret;
    }

    protected bool ControllaRuolo(string username, string verificaruolo)
    {
        bool flag = false;
        foreach (string role in Roles.GetRolesForUser(username))
        {
            if (role.ToString() == verificaruolo) flag = true;
        }
        return flag;
    }
    protected void Inseriscibtn_Click(Object sender, EventArgs e)
    {
        try
        {
            Offerte updrecord = new Offerte();
            updrecord.Autore = User.Identity.Name;
            updrecord.CodiceTipologia = Tipologia;

            int tmpcoll = 0;
            //if (Int32.TryParse(txtIdcollegato.Text, out tmpcoll))
            //    updrecord.Id_collegato = tmpcoll;

            updrecord.DenominazioneI = txtDenominazioneI.Text;
            updrecord.DescrizioneI = txtDescrizioneI.Text;

            if (updrecord.DenominazioneI.Trim() == string.Empty && updrecord.DescrizioneI.Trim() == string.Empty) return;


            DateTime _tmpdate = System.DateTime.Now;
            //if (!DateTime.TryParse(txtData.Text, out _tmpdate))
            //    _tmpdate = System.DateTime.Now;
            updrecord.DataInserimento = _tmpdate;
            offDM.InsertOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, updrecord);
            //Inviamo una mailing a tutti i soci ...
            // da fare ...

            this.SvuotaDettaglio();
            AssociaDati();
            InviaMailAvvisoInserimentoArgomento(updrecord.Id.ToString());

        }
        catch (Exception error)
        {
            output.Text += error.Message;
            if (error.InnerException != null)
                output.Text += error.InnerException.Message.ToString();
        }

    }
    protected void SvuotaDettaglio()
    {
        txtDenominazioneI.Text = string.Empty;
        txtDescrizioneI.Text = string.Empty;
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
            AssociaDatiRepeater(p.GetPagerList(PagerRisultati.CurrentPage, PagerRisultati.PageSize));
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

    [System.Web.Services.WebMethod]
    public static string CancellaPost(string id)
    {
        return CancellaPostExecute(id);
    }
    public static string CancellaPostExecute(string id)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            int tmp = 0;
            if (Int32.TryParse(id, out tmp))
            {
                offerteDM offDM = new offerteDM();
                Offerte o = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id.ToString());
                if (o != null)
                {
                    //scorro e cancello le foto presenti in relazione al post
                    string pathDestinazione = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + o.CodiceTipologia + "\\" + o.Id.ToString();
                    foreach (Allegato foto in o.FotoCollection_M)
                    {
                        try
                        {
                            bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o.Id, foto.NomeFile, "", pathDestinazione);
                        }
                        catch (Exception errodel)
                        {
                            sb.Append(errodel.Message);
                        }
                    }
                    //Cancello il record
                    offDM.DeleteOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o);

                    //Cancellimo anche i post collegati e relative foto e documenti se presenti
                    List<Offerte> listcollegate = offDM.CaricaOfferteCollegate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id, "", false, "", true);
                    if (listcollegate != null)
                        foreach (Offerte oc in listcollegate)
                        {
                            pathDestinazione = WelcomeLibrary.STATIC.Global.PercorsoFiscoContenuti + "\\" + oc.CodiceTipologia + "\\" + oc.Id.ToString();
                            foreach (Allegato foto in oc.FotoCollection_M)
                            {
                                try
                                {
                                    bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, oc.Id, foto.NomeFile, "", pathDestinazione);
                                }
                                catch (Exception errodel)
                                {
                                    sb.Append(errodel.Message);
                                }
                            }
                            //Cancello il record
                            offDM.DeleteOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, oc);
                        }

                    sb.Append("Attendi Cancellazione del post in corso!");
                }

            }
        }
        catch (Exception err)
        {
            sb.Append("Errore cancellazione Post");
        }
        return sb.ToString();
    }


    [System.Web.Services.WebMethod]
    public static string AggiornaPost(string id, string denominazione, string descrizione)
    {
        return AggiornaPostExecute(id, denominazione, descrizione);
    }
    public static string AggiornaPostExecute(string id, string denominazione, string descrizione)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            Offerte updrecord = new Offerte();
            int tmp = 0;
            if (Int32.TryParse(id, out tmp))
            {

                offerteDM offDM = new offerteDM();
                Offerte o = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id.ToString());
                if (o != null)
                {
                    //CommonPage.CleanInput()

                    o.DenominazioneI = denominazione;
                    o.DescrizioneI = descrizione;
                    offDM.UpdateOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, o);
                    sb.Append(" Aggiornato post!");
                }

            }
        }
        catch (Exception err)
        {
            sb.Append("Errore aggiornamento Post");
        }
        return sb.ToString();
    }


    [System.Web.Services.WebMethod]
    public static string InserisciRePost(string id, string denominazione, string descrizione, string userattuale)
    {
        return InserisciRepostPostExecute(id, denominazione, descrizione, userattuale);
    }
    public static string InserisciRepostPostExecute(string id, string denominazione, string descrizione, string userattuale)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            Offerte newrecord = new Offerte();
            int tmp = 0;
            if (Int32.TryParse(id, out tmp))
            {

                offerteDM offDM = new offerteDM();
                Offerte o = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, id.ToString()); //POST PRINCIPALE
                if (tmp != 0 && o != null)
                {
                    //CommonPage.CleanInput()
                    newrecord.DenominazioneI = denominazione;
                    newrecord.DescrizioneI = (descrizione);
                    newrecord.CodiceTipologia = o.CodiceTipologia;
                    DateTime _tmpdate = System.DateTime.Now;
                    newrecord.DataInserimento = _tmpdate;
                    newrecord.Autore = userattuale;
                    newrecord.Id_collegato = tmp; //ollego il repost al post principale
                    offDM.InsertOfferta(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, newrecord);
                    sb.Append("Attendi Per Inserimento commento a Post principale!");
                }

            }
        }
        catch (Exception err)
        {
            sb.Append("Errore Inserimento commento a Post Principale");
        }
        return sb.ToString();
    }


    protected string CrealistaFiles(object id, object lista, object Autore)
    {
        string html = "";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (lista != null)
        {
            //sb.Append(" <ul class=\"list-inline\">");
            //  sb.Append("<div class=\"col-md-4\">");
            foreach (Allegato a in (AllegatiCollection)lista)
            {
                string link = PercorsoFiles + "/" + Tipologia + "/" + id.ToString() + "/" + a.NomeFile;
                link = CommonPage.ReplaceAbsoluteLinks(link);
                string descrizione = a.Descrizione;
                if (string.IsNullOrWhiteSpace(descrizione)) descrizione = a.NomeFile;
                sb.Append("<li>");
                if ((a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                {
                    //sb.Append("<a class=\"linked\" target=\"_blank\" href=\"" + link + "\"><i class=\"fa  fa-eye  color-green\"></i>" + descrizione + "</a>");
                    sb.Append("<a style=\"margin-right:10px;margin-bottom:10px;min-width:190px\"  target=\"_blank\" href=\"" + link + "\"><div class=\"fa fa-file-image-o\">" + descrizione + "</div></a>");

                }
                else
                {
                    //sb.Append("<a class=\"linked\" target=\"_blank\" href=\"" + link + "\"><i class=\"fa  fa-eye  color-green\"></i>" + descrizione + "</a>");
                    sb.Append("<a style=\"margin-right:10px;margin-bottom:10px;min-width:190px\"  target=\"_blank\" href=\"" + link + "\"><div class=\"fa fa-file\">" + descrizione + "</div></a>");
                }
                if (ControllaVisibilitaAutore(Autore.ToString()))
                    sb.Append(" <a onmouseover=\"this.style.cursor='pointer'\" onclick=\"javascript:fileDeletePost('" + id.ToString() + "','" + a.NomeFile + "')\" title=\"Cancella\" >  <div class=\"fa fa fa-times\"></div></a>");

                sb.Append("</li>");
            }
            // sb.Append("</div>");
            //sb.Append("</ul>");

        }
        html = sb.ToString();
        return html;
    }


    protected string CrealistaImages(object id, object lista)
    {

        string html = "";

        string active = " active";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (lista != null)
        {
            foreach (Allegato a in (AllegatiCollection)lista)
            {
                string link = PercorsoFiles + "/" + Tipologia + "/" + id.ToString() + "/" + a.NomeFile;
                link = CommonPage.ReplaceAbsoluteLinks(link);
                string descrizione = a.Descrizione;
                if (string.IsNullOrWhiteSpace(descrizione)) descrizione = a.NomeFile;
                if ((a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                {
                    sb.Append("<div class=\"slider-img" + active + "\" style=\"left: 0px; top: 0px; display: none; position: absolute; z-index: 100; opacity: 0;\">");
                    if ((a.NomeFile.ToString().ToLower().EndsWith("jpg") || a.NomeFile.ToString().ToLower().EndsWith("gif") || a.NomeFile.ToString().ToLower().EndsWith("png")))
                    {

                        sb.Append("<img alt=\"\" src=\"" + link + "\" />");
                    }
                    sb.Append("</div>");
                    active = "";
                }
            }

        }
        html = sb.ToString();
        return html;
    }
    public string CalcolaNumeroCommenti(int Id)
    {
        string ret ="";
        if(dictRepost.ContainsKey(Id))
        {
            int totalerepost = ((List<Offerte>)dictRepost[(Id)]).Count;
            ret = "Totale commenti : " + totalerepost.ToString();
        }
        return ret;
    }

    public string VisualizzaImmagineSocio(string username)
    {
        string urlimmagine = "~/AreaRiservata/media/gist-symbol.png";
        if (dictSocibyUsername.ContainsKey(username))
            if (dictSocibyUsername[username] != null)
            {
                Offerte Details = dictSocibyUsername[username];
                //Caricare il ritratto dall'archivio soci --> da fare!!
                if (Details.FotoCollection_M != null)
                {
                    Allegato a = Details.FotoCollection_M.Find(o => o.Descrizione == "Ritratto");
                    if (a != null)
                    {
                        urlimmagine = PercorsoFiles + "/" + Details.CodiceTipologia + "/" + Details.Id + "/" + a.NomeFile;
                    }
                }
            }
        return urlimmagine;
    }

    protected string CercaNomeSociobyUsername(string username)
    {
        string ret = username;
        if (dictSocibyUsername.ContainsKey(username))
            if (dictSocibyUsername[username] != null)
            {
                ret = ((Offerte)dictSocibyUsername[username]).Cognome_dts + " " + ((Offerte)dictSocibyUsername[username]).Nome_dts;
            }

        return ret;
    }
    protected void CaricaListaSoci()
    {
        dictSocibyUsername = new Dictionary<string, Offerte>();
        //Prememorizziamo i dati che mi servono dei soci in una sola volta
        try
        {
            List<OleDbParameter> parColl = new List<OleDbParameter>();
            OleDbParameter p3 = new OleDbParameter("@CodiceTIPOLOGIA", "rif000100");
            parColl.Add(p3);
            //bool _statoblocco = false;
            //OleDbParameter pstatoblocco = new OleDbParameter("@Bloccoaccesso_dts", _statoblocco);
            //parColl.Add(pstatoblocco);
            List<Offerte> soci = offDM.CaricaOfferteFiltrate(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, parColl, "", null, null, "", false);
            if (soci != null)
            {
                MembershipUserCollection MUColl = Membership.GetAllUsers();
                Dictionary<string, string> sociusers = new Dictionary<string, string>();
                foreach (MembershipUser user in MUColl)
                {
                    string idsocio = CommonPage.getidsocio(user.UserName);
                    if (!sociusers.ContainsKey(idsocio))
                        sociusers.Add(idsocio, user.UserName);
                }
                foreach (Offerte socio in soci)
                {

                    if (sociusers[socio.Id.ToString()] != null)
                    {
                        if (!dictSocibyUsername.ContainsKey(sociusers[socio.Id.ToString()].ToString()))
                            dictSocibyUsername.Add(sociusers[socio.Id.ToString()].ToString(), socio);
                    }
                    else //Non ho un username per il socio non essendo presente nel membership!!!!
                    {
                        //Decidere se gestirlo....
                    }
                }
            }
        }
        catch (Exception error)
        {
            output.Text = error.Message.ToString();
            return;
        }

    }

    private void InviaMailAvvisoInserimentoArgomento(string idmodificato)
    {
        if (debug) return;
        try
        {
            //Tito su il post appena inserito
            Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idmodificato);

            string nomesocio = CercaNomeSociobyUsername(User.Identity.Name);
            string SoggettoMail = " Inserito nuovo argomento nel forum da parte del socio " + nomesocio;

            string linkmodificato = CommonPage.ReplaceAbsoluteLinks(CommonPage.CreaLinkRoutes(null, false, Lingua, CommonPage.CleanUrl(item.DenominazioneI), idmodificato, Tipologia));
            string Descrizione = item.DenominazioneI + "<br/>";
              Descrizione += item.DescrizioneI + "<br/>";


            Descrizione += "Per vedere il contenuto completo online clicca sul link qui sotto:" + "<br/><br/>";
            //Descrizione += "<a target=\"_blank\" href=" + linkmodificato + ">Clicca qui per andare al contenuto nel forum.</a><br/>";
            Descrizione += linkmodificato;
            Descrizione = Descrizione.Replace("<br/>", "\r\n");

#if false
            //Invio singo
            foreach (KeyValuePair<string, Offerte> kv in dictSocibyUsername)
            {
                string nomedestinatario = ((Offerte)kv.Value).Cognome_dts + " " + ((Offerte)kv.Value).Nome_dts;
                string maildestinatario = ((Offerte)kv.Value).Emailriservata_dts;
                Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
            }
#endif

            if (dictSocibyUsername.Count > 0)
            {

                List<string> bccemail = new List<string>();
                foreach (KeyValuePair<string, Offerte> kv in dictSocibyUsername)
                {
                   // string nomedestinatario = ((Offerte)kv.Value).Cognome_dts + " " + ((Offerte)kv.Value).Nome_dts;
                    string maildestinatario = ((Offerte)kv.Value).Emailriservata_dts;
                    bccemail.Add(maildestinatario);
                }

                //if (User.Identity.Name.ToLower()=="segreteria" ||  User.Identity.Name.ToLower()=="webmaster" )
                string emailsegreteria = getmailuser("Segreteria");
                if (!string.IsNullOrEmpty(emailsegreteria.Trim())) //Mando tutte le mail sempre anche alla segreteria
                {
                    //string nomedestinatario = "Segreteria";
                    string maildestinatario = emailsegreteria;
                    bccemail.Add(maildestinatario);
                }

                //Invio in bcc a gruppo unico
               Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, "", "", null, "", false, null, true, bccemail);
            }

        }
        catch (Exception err)
        {
            output.Text = err.Message;
        }
    }


#if false
    #region CARICAMENTO ALLEGATI ALLEGATI

     

    private void ModificaFile(string idrecord, string nomefile, string Descrizione)
    {
        try
        {
            int i = 0;
            int.TryParse(idrecord, out i);
            bool ret = offDM.modificaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, i, nomefile, Descrizione);
        }
        catch (Exception errins)
        {
            output.Text = errins.Message;
        }

        output.Text = "Descrizone file Modificata Correttamente";
        //Aggiorniamo il repeater e la foto per il record selezionato
        AssociaDati();
    }
    private void EliminaFile(string idrecord, string nomefile)
    {
        //Controlliamo se ho selezionato un record
        if (idrecord == null || idrecord == "")
        {
            output.Text = "Selezionare un elemento per cancellare il file";
            return;
        }

        if (nomefile == null || nomefile == "")
        {
            output.Text = "Selezionare una foto da cancellare";
            return;
        }
        int idSelected = 0;
        if (!Int32.TryParse(idrecord, out idSelected))
        {
            output.Text = "Selezionare un elemento per cancellare la foto";
            return;
        }
        //Ricarichiamo l'offerta selezionata dal db
        //Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected);

        //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
        string pathDestinazione = Server.MapPath(PercorsoFiles + "/" + Tipologia + "/" + idrecord);
        //-------------------------------------
        //Eliminiamo il file se presente
        //-------------------------------------
        try
        {
            try
            {
                bool ret = offDM.CancellaFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, nomefile, "", pathDestinazione);
            }
            catch (Exception errodel)
            {
                output.Text = errodel.Message;
            }

            //Aggiorniamo il repeater e la foto per il record selezionato
            AssociaDati();

        }
        catch (Exception errore)
        {
            output.Text += " " + errore.Message;
        }
    }
    private void CaricaFile(FileUpload UploadControl, string DescrizioneFile, string idrecord)
    {
        try
        {
            //Controlliamo se ho selezionato un record
            if (idrecord == null || idrecord == "")
            {
                output.Text = "Selezionare un elemento per associare il file";
                return;
            }
            int idSelected = 0;
            if (!Int32.TryParse(idrecord, out idSelected))
            {
                output.Text = "Selezionare un elemento per associare il file";
                return;
            }

            //Verifichiamo la presenza del percorso di destinazione altrimenti lo creiamo
            //Percorso files Offerte del tipo percorsobasecartellafiles/con000001/4
            string pathDestinazione = Server.MapPath(PercorsoFiles + "/" + Tipologia + "/" + idrecord);
            if (!Directory.Exists(pathDestinazione))
                Directory.CreateDirectory(pathDestinazione);

            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (UploadControl.HasFile)
            {
                if (UploadControl.PostedFile.ContentLength > 10000000)
                {

                    output.Text = "Il File non può essere caricato perché supera 10MB!";
                }
                else
                {
                    //ELIMINO I CARATTERI CHE CREANO PROBLEMI IN APERTURA AL BROWSER
                    string NomeCorretto = UploadControl.FileName.Replace("+", "");
                    NomeCorretto = NomeCorretto.Replace("%", "");
                    NomeCorretto = NomeCorretto.Replace("'", "").ToLower();
                    NomeCorretto = NomeCorretto.Replace(" ", "-").ToLower();
                    //string NomeCorretto = Server.HtmlEncode(FotoUpload1.FileName);
                    if (System.IO.File.Exists(pathDestinazione))
                    {
                        output.Text = ("Il File non può essere caricato perché già presente sul server!");
                    }
                    else
                    {
                        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        if (UploadControl.PostedFile.ContentType == "image/jpeg" || UploadControl.PostedFile.ContentType == "image/png" || UploadControl.PostedFile.ContentType == "image/gif")
                        {
                            int maxheight = 800;
                            int maxwidth = 1000;
                            bool ridimensiona = true;
                            //RIDIMENSIONO E FACCIO L'UPLOAD DELLA FOTO!!!
                            if (ResizeAndSave(UploadControl.PostedFile.InputStream, maxwidth, maxheight, pathDestinazione + "\\" + NomeCorretto, ridimensiona))
                            {
                                //Creiamo l'anteprima Piccola per usi in liste
                                this.CreaAnteprima(pathDestinazione + "\\" + NomeCorretto, 150, 150, pathDestinazione + "\\", "Ant" + NomeCorretto);
                                //ESITO POSITIVO DELL'UPLOAD --> SCRIVO NEL DB
                                //I DATI PER RINTRACCIARE LA FOTO-->SCHEMA E VALORI
                                try
                                {
                                    try
                                    {
                                        bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                    }
                                    catch (Exception errins)
                                    {
                                        output.Text = errins.Message;
                                    }

                                    output.Text += "Foto Inserita Correttamente<br/>";
                                    //Aggiorniamo il repeater e la foto per il record selezionato
                                    AssociaDati();
                                    //OfferteCollection list = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, TipologiaOfferte);
                                    //rptOfferte.DataSource = list;
                                    //rptOfferte.DataBind();


                                }
                                catch (Exception error)
                                {
                                    //CANCELLO LA FOTO UPLOADATA
                                    if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                    if (System.IO.File.Exists(pathDestinazione + "\\" + "Ant" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + "Ant" + NomeCorretto);
                                    //AGGIORNO IL DETAILSVIEW
                                    output.Text = error.Message;
                                }
                            }
                            else { output.Text += ("La foto non è stata caricata! (Problema nel caricamento)"); }
                        }
                        else if (UploadControl.PostedFile.ContentType == "application/pdf")
                        {

                            //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                            UploadControl.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                            try
                            {
                                try
                                {
                                    bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                }
                                catch (Exception errins)
                                {
                                    output.Text = errins.Message;
                                }

                                output.Text += "Documento Inserito Correttamente<br/>";
                                //Aggiorniamo il repeater e la foto per il record selezionato
                                AssociaDati();
                            }
                            catch (Exception error)
                            {
                                //CANCELLO IL FILE UPLOADATO
                                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                //AGGIORNO IL DETAILSVIEW
                                output.Text = error.Message;
                            }
                        }
                        else
                        {

                            //ANZICHE COME FOTO LO CARICO COME DOCUMENTO PERCHE' NON RICONOSCO IL FORMATO  
                            UploadControl.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                            try
                            {
                                try
                                {
                                    bool ret = offDM.insertFoto(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idSelected, NomeCorretto, DescrizioneFile);
                                }
                                catch (Exception errins)
                                {
                                    output.Text = errins.Message;
                                }

                                output.Text += "Documento Inserito Correttamente<br/>";
                                //Aggiorniamo il repeater e la foto per il record selezionato
                                AssociaDati();
                            }
                            catch (Exception error)
                            {
                                //CANCELLO IL FILE UPLOADATO
                                if (System.IO.File.Exists(pathDestinazione + "\\" + NomeCorretto)) System.IO.File.Delete(pathDestinazione + "\\" + NomeCorretto);
                                //AGGIORNO IL DETAILSVIEW
                                output.Text = error.Message;
                            }
                        }
                    }
                }
            }

        }
        catch (Exception errorecaricamento)
        {
            output.Text += errorecaricamento.Message;
            if (errorecaricamento.InnerException != null)
                output.Text += errorecaricamento.InnerException.Message;

        }
    }
    private bool ResizeAndSave(System.IO.Stream imgStr, int Width, int Height, string Filename, bool ridimensiona)
    {
        try
        {
            using (System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(imgStr))
            {
                if (ridimensiona == true)
                {
                    //CREO LE DIMENSIONI DELLA FOTO SALVATA IN BASE AL RAPORTO ORIGINALE DI ASPETTO
                    int altezzaStream = bmpStream.Height; //altezza foto originale
                    int larghezzaStream = bmpStream.Width; //larghezza foto originale
                    if (altezzaStream <= larghezzaStream)
                        Height = Convert.ToInt32(((double)Width / (double)larghezzaStream) * (double)altezzaStream);
                    else
                        Width = Convert.ToInt32(((double)Height / (double)altezzaStream) * (double)larghezzaStream);

                    //FINE CALCOLO ----------------------------------------------------------
                }

                using (System.Drawing.Bitmap img_orig = new System.Drawing.Bitmap(bmpStream))
                {

                    System.Drawing.Bitmap img_filtrata = img_orig;
                    //FILTRI CONTRASTO BRIGHTNESS/contrast/sturation
                    //img_filtrata = ImageProcessing.applicaSaturationCorrection(img_filtrata, 0.05);
                    img_filtrata = ImageProcessing.applicaBrightness(img_filtrata, 0.03);
                    //img_filtrata = ImageProcessing.applicaContrast(img_filtrata, 0.75);
                    //img_filtrata = ImageProcessing.applicaAdaptiveSmoothing(img_filtrata);
                    //img_filtrata = ImageProcessing.applicaConservativeSmoothing(img_filtrata);
                    //img_filtrata = ImageProcessing.applicaHSLFilter(img_filtrata, 0.87, 0.075);
                    //img_filtrata = ImageProcessing.applicaGaussianBlur(img_filtrata, 1, 5);
                    //img_filtrata = ImageProcessing.applicaMediano(img_filtrata, 4);
                    // ImageProcessing.NoiseRemoval(img_filtrata);
                    //img_filtrata = ImageProcessing.MeanFilter(img_filtrata, 2);
                    img_filtrata = ImageProcessing.applicaResizeBilinear(img_filtrata, Width, Height); //resisze
                    //img_filtrata = ImageProcessing.applicaResizeBicubic(img_filtrata, Width, Height); //resisze

                    using (System.Drawing.Bitmap img = img_filtrata)
                    {
                        System.Drawing.Imaging.ImageFormat imgF = null;
                        switch (System.IO.Path.GetExtension(Filename).ToLower())
                        {
                            case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif; break;
                            case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; break;
                            case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; break;
                            default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                        }
                        //img.Save(Filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                        if (imgF == System.Drawing.Imaging.ImageFormat.Jpeg)
                        {
                            // Create an Encoder object based on the GUID for the Quality parameter category.
                            ImageCodecInfo jgpEncoder = GetEncoder(imgF);
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            // Create an EncoderParameters object.
                            // An EncoderParameters object has an array of EncoderParameter objects. In this case, there is only one EncoderParameter object in the array.
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 85L); //Livelli di compressione da 0L a 100L ( peggio -> meglio)
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            img.Save(Filename, jgpEncoder, myEncoderParameters);
                        }
                        else
                            img.Save(Filename, imgF);
                    }
                }
            }
        }
        catch
        {
            return false;
        }
        return true;
    }
    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
    protected void CreaAnteprima(string filePath, int Altezza, int Larghezza, string pathAnteprime, string nomeAnteprima)
    {
        string PathTempAnteprime = pathAnteprime;
        System.Drawing.Imaging.ImageFormat imgF = null;
        //System.IO.File.Exists(PathTempAnteprime);
        if (!System.IO.Directory.Exists(PathTempAnteprime))
        {
            System.IO.Directory.CreateDirectory(PathTempAnteprime);
        }
        // throw new Exception("Cartella temporanea di destinazione per l'anteprima non trovata!");

        using (System.IO.FileStream file = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
        {
            using (System.Drawing.Image bmpStream = System.Drawing.Image.FromStream(file))
            {
                int altezzaStream = bmpStream.Height;
                int larghezzaStream = bmpStream.Width;
                if (altezzaStream <= larghezzaStream)
                    Altezza = Convert.ToInt32((double)Larghezza / (double)larghezzaStream * (double)altezzaStream);
                else
                    Larghezza = Convert.ToInt32((double)Altezza / (double)altezzaStream * (double)larghezzaStream);
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(bmpStream, new System.Drawing.Size(Larghezza, Altezza));
                switch (System.IO.Path.GetExtension(filePath).ToLower())
                {
                    case ".gif": imgF = System.Drawing.Imaging.ImageFormat.Gif; break;
                    case ".png": imgF = System.Drawing.Imaging.ImageFormat.Png; break;
                    case ".bmp": imgF = System.Drawing.Imaging.ImageFormat.Bmp; break;
                    default: imgF = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                }

                img.Save(PathTempAnteprime + nomeAnteprima, imgF);
            }
            file.Close();
        }
        if (!System.IO.File.Exists(PathTempAnteprime + nomeAnteprima))
            output.Text = ("Anteprima Allegato non salvata correttamente!");

    }
    protected string ComponiUrlAnteprima(object FotoColl, string id)
    {
        string url = "";

         if (FotoColl != null && ((AllegatiCollection)FotoColl).Count > 0 && ((AllegatiCollection)FotoColl)[0].NomeAnteprima!=null)
            if (!((AllegatiCollection)FotoColl)[0].NomeAnteprima.ToLower().StartsWith("http://") && !((AllegatiCollection)FotoColl)[0].NomeAnteprima.ToLower().StartsWith("https://"))
                url = PercorsoFiles + "/" + TipologiaOfferte + "/" + id + "/" + ((AllegatiCollection)FotoColl)[0].NomeAnteprima;
            else
                url = ((AllegatiCollection)FotoColl)[0].NomeAnteprima;

        return url;
    }
    protected string ComponiUrlGalleriaAnteprima(string NomeAnteprima, string id)
    {
        string url = "";
        url = PercorsoFiles + "/" + Tipologia + "/" + id + "/" + NomeAnteprima;
        //PER I FILES CHE NON SONO IMMAGINI METTO UN'IMMAGINE FISSA
        if (!(NomeAnteprima.ToLower().EndsWith("jpg") || NomeAnteprima.ToLower().EndsWith("gif") || NomeAnteprima.ToLower().EndsWith("png")))
            url = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/images/pdf.png";
        return url;
    }

    #endregion

#endif
}
