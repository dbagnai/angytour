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
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Collections.Generic;
using System.Data.OleDb;

public partial class AspNetPages_Content_Tipo3 : CommonPage
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
    public string TipoContenuto
    {
        get { return ViewState["TipoContenuto"] != null ? (string)(ViewState["TipoContenuto"]) : ""; }
        set { ViewState["TipoContenuto"] = value; }
    }
    public string Dettaglioselezione
    {
        get { return ViewState["Dettaglioselezione"] != null ? (string)(ViewState["Dettaglioselezione"]) : ""; }
        set { ViewState["Dettaglioselezione"] = value; }
    }
    public string CodiceTipologia
    {
        get { return ViewState["CodiceTipologia"] != null ? (string)(ViewState["CodiceTipologia"]) : ""; }
        set { ViewState["CodiceTipologia"] = value; }
    }
    public string idOfferta
    {
        get { return ViewState["idOfferta"] != null ? (string)(ViewState["idOfferta"]) : ""; }
        set { ViewState["idOfferta"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;


                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);

                TipoContenuto = CaricaValoreMaster(Request, Session, "TipoContenuto");
                CodiceTipologia = CaricaValoreMaster(Request, Session, "CodiceTipologia");
                idOfferta = CaricaValoreMaster(Request, Session, "idOfferta");
                Dettaglioselezione = CaricaValoreMaster(Request, Session, "Dettaglioselezione");


                CaricaControlliJS();


                string conversione = CaricaValoreMaster(Request, Session, "conversione");
                if (conversione == "true")
                {
                    Visualizzarisposta();
                    return;
                }

                SettaTestoPagina();

                DataBind();
            }
            //else
            //{
            //    output.Text = "";
            //    lblRisposta.Text = "";
            //    lblRisposta.Visible = false;
            //    plhRisposta.Visible = false;
            //    plhForm.Visible = true;
            //}
        }
        catch (Exception err)
        {
            output.Text = err.Message;
        }
    }
    public void CaricaControlliJS()
    {
        //Carico la galleria in masterpage corretta
        //if (TipoContenuto == "")
        //    Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-home", false, Lingua);
        //else
        //    Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "header-" + TipoContenuto, false, Lingua);

        PlaceHolder plhMapMaster = (PlaceHolder)Master.FindControl("plhMapMaster");
        if (plhMapMaster != null)
            plhMapMaster.Visible = false;
        //Literal lit = (Literal)Master.FindControl("litPortfolioBanners1");
        //Master.CaricaBannersPortfolioRival("TBL_BANNERS_GENERALE", 0, 0, "banner-portfolio-sezionicatalogo", false, lit, Lingua,true);


        //Carico la galleria in masterpage corretta
        string controllistBanHead = "";
        if (TipoContenuto == "")
            controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,300);";
        else
            controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-" + TipoContenuto + "',false,2000,300);";

        ClientScriptManager cs = Page.ClientScript;
        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {
            cs.RegisterStartupScript(this.GetType(), "controllistBanHead", controllistBanHead, true);
        }

    }

#if true
    private void SettaTestoPagina()
    {
        string addtext = "";
        Offerte item = new Offerte();
        if (idOfferta != "" && idOfferta != "0")
        {
            item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
            if (item != null && item.Id != 0)
            {
                CodiceTipologia = item.CodiceTipologia;
                addtext = item.DenominazionebyLingua(Lingua) + "\r\n";
                if (item.CodiceTipologia == "rif000008" || item.CodiceTipologia == "rif000009") //Personalizzazione nel caso delle camere o appartamenti !!!
                {
                    TipoContenuto = "Prenota";
                }
            }
        }
#if true
        if (TipoContenuto == "Prenota")
        {
            ddlLocations.Items.Clear();
            ListItem li = new ListItem(Resources.Common.selLocation, "");
            ddlLocations.Items.Add(li);
            //Inseriamo la lista delle locations 
            OfferteCollection list = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceTipologia);
            if (list != null)
            {
                foreach (Offerte item1 in list)
                {
                    li = new ListItem(estraititolo(item1), item1.Id.ToString());
                    ddlLocations.Items.Add(li);
                }

                try
                {
                    ddlLocations.SelectedValue = item.Id.ToString();
                }
                catch { }
            }
        }
#endif

        txtDescrizione.Text += addtext + Resources.Common.testoMessage + "\r\n";

        EvidenziaSelezione(TipoContenuto);


        //MODIFICA VISUALIZZAZIONE SEZIONI HTML PER FORM E POSIZIONE
        switch (TipoContenuto)
        {
            case "Dovesiamo":
                plhDove.Visible = true;
                plhForm.Visible = false;
                divRoutes.Visible = false;
                break;
            case "Prenota":
                plhDove.Visible = true;
                plhForm.Visible = true;
                pnlPrenotazione.Visible = true; //Sezione per completamento form prenotazione
                divRoutes.Visible = false;

                //CaricaDdlsedi("");
                break;
            //case "Acquistousato":
            //    plhDove.Visible = false;
            //    plhForm.Visible = true;
            //    pnlPrenotazione.Visible = true; //Sezione per completamento form prenotazione
            //    CaricaDdlservizi("");
            //    CaricaDdlsedi("");
            //    break;
            case "Richiesta":
                plhForm.Visible = true;
                break;
            default:
                plhForm.Visible = true;
                plhDove.Visible = true;
                divRoutes.Visible = true;
                break;
        }

        //VISUALIZZIAMO IL TESTO CARICANDOLO OPPORTUNAMENTE
        string htmlPage = "";
        if (GetGlobalResourceObject("Common", "testo" + TipoContenuto) != null)
            htmlPage = "<h2>" + GetGlobalResourceObject("Common", "testo" + TipoContenuto).ToString() + "</h2>";
        string strigaperricerca = Request.Url.AbsolutePath + "?" + Request.QueryString;
        if (strigaperricerca.LastIndexOf("&idOfferta=") != -1)
            strigaperricerca = strigaperricerca.Substring(0, strigaperricerca.LastIndexOf("&idOfferta="));
      
        Contenuti content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
        if (content != null && content.Id != 0)
        {
            htmlPage = ReplaceAbsoluteLinks(ReplaceAbsoluteLinks(content.DescrizionebyLingua(Lingua)));

        }
        lblContenutiContatti.Text = htmlPage;

    }

#endif

    protected string estraititolo(Offerte item)
    {
        if (item == null) return "";
        string testotitolo = item.DenominazionebyLingua(Lingua);


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
    protected string NomeProvincia(string codiceprovincia)
    {
        string ritorno = "";
        Province item = Utility.ElencoProvince.Find(delegate (Province tmp) { return (tmp.Lingua == Lingua && tmp.Codice == codiceprovincia); });
        if (item != null)
            ritorno = item.Provincia;
        return ritorno;
    }


    protected void ddlRegione_OnInit(object sender, EventArgs e)
    {
        RiempiDdlRegione("", (DropDownList)sender);
    }
    private void RiempiDdlRegione(string valore, DropDownList ddlregione)
    {

        ddlregione.Items.Clear();
        ddlregione.Items.Insert(0, "Seleziona Regione");
        ddlregione.Items[0].Value = "";

        WelcomeLibrary.DOM.ProvinceCollection regioni = new WelcomeLibrary.DOM.ProvinceCollection();
        List<Province> provincelingua = Utility.ElencoProvince.FindAll(delegate (Province tmp) { return (tmp.Lingua == "I" && tmp.SiglaNazione.ToLower() == "it"); });
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
            ddlregione.Items.Add(i);
        }
        try
        {
            ddlregione.SelectedValue = valore;
        }
        catch { valore = ""; ddlregione.SelectedValue = valore; }

    }

    /// <summary>
    /// Invia al destinatario la rochiesta di interessamento
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnInvia_Click(object sender, EventArgs e)
    {
        try
        {
            //Prepariamo e inviamo il mail
            string nomemittente = txtNome.Text;
            string cognomemittente = txtSoc.Text;
            string mittenteMail = txtEmail.Text;
            string nomedestinatario = Nome;
            string maildestinatario = Email;
            Dictionary<string, string> destinatariperregione = new Dictionary<string, string>();

            int idperstatistiche = 0;
            string tipo = "informazioni";
            if (TipoContenuto == "Prenota")
                tipo = "richiesta preventivo prenotazione ";
            if (TipoContenuto == "Acquistousato")
                tipo = "vendita usato ";
            string SoggettoMail = "Richiesta " + tipo + " da " + cognomemittente + " " + nomemittente + " tramite il sito " + Nome;
            string Descrizione = txtDescrizione.Text.Replace("\r", "<br/>") + " <br/> ";

            if (idOfferta != "") //Inseriamo il dettaglio della scheda di provenienza
            {
                Offerte item = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idOfferta);
                if (item != null && item.Id != 0)
                {
                    idperstatistiche = item.Id;
                    if (!string.IsNullOrWhiteSpace(item.Email)) //Se non è vuota mando alla mail indicata nell'articolo
                    {
                        nomedestinatario = item.Email;
                        maildestinatario = item.Email;
                    }
                    Descrizione += "<br/><br/>";
                    Descrizione += "Pagina provenienza: " + item.DenominazioneI;
                    Descrizione += "<br/><br/>";
                }
            }
            if (TipoContenuto == "Richiesta")
            {
            }
            if (TipoContenuto == "Prenota")
            {

                Descrizione += " <br/> Arrivo richiesto:" + txtArrivo.Text + " Partenza Richiesta: " + txtPartenza.Text;
                Descrizione += " <br/> Numero adulti:" + txtAdulti.Text + " <br/> Numero bambini:" + txtBambini.Text + " Alloggio : " + ddlLocations.SelectedItem.Text;
            }
            //if (TipoContenuto == "Acquistousato")
            //{
            //    Descrizione += " <br/> Tipologia servizio : Vendita usato ";
            //    Descrizione += " <br/>Marca Veicolo:" + txtMarca.Text + " Modello : " + txtModello.Text;
            //    Descrizione += " <br/>Targa :" + txttarga.Text + " Km percorsi : " + txtKm.Text;
            //}
            Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome o rag soc. Cliente: " + cognomemittente;
            Descrizione += " <br/> Telefono Cliente:" + txtTel.Text + " Email Cliente: " + mittenteMail + " Lingua Cliente: " + Lingua;
            Descrizione += " <br/> Confermo l'autorizzazione al trattamento dei miei dati personali (D.Lgs 196/2003)";

            if (chkNewsletter.Checked == true)
            {
                //SoggettoMail = "Richiesta iscrizione newsletter da " + nomemittente + " tramite il sito " + Nome;
                //------------------------------------------------
                //Memorizzo i dati nel cliente per la newsletter
                //------------------------------------------------
                ClientiDM cliDM = new ClientiDM();
                Cliente cli = new Cliente();
                string lingua = Lingua;
                string tipocliente = "0"; //Cliente standard per newsletter
                                          //  cli.DataNascita = System.DateTime.Now.Date;
                cli.Lingua = lingua;
                cli.id_tipi_clienti = tipocliente;
                cli.Nome = nomemittente.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                cli.Cognome = cognomemittente.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                cli.Consenso1 = true;
                cli.ConsensoPrivacy = true;
                cli.Validato = true;
                cli.Email = mittenteMail.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                Cliente _tmp = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, cli.Email, tipocliente);
                if ((_tmp != null && _tmp.Id_cliente != 0))
                    cli.Id_cliente = _tmp.Id_cliente;
                cliDM.InserisciAggiornaCliente(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, ref cli);
            }


            if (chkPrivacy.Checked)
            {
                //Utility.invioMailGenericoSystemnet(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario, null, "", false, null, false, null, "mailing");
                //Utility.invioMailGenericoSystemnet(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
                Utility.invioMailGenerico(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

                //Registro la statistica di contatto
                Statistiche stat = new Statistiche();
                stat.Data = DateTime.Now;
                stat.EmailDestinatario = maildestinatario;
                stat.EmailMittente = mittenteMail;
                stat.Idattivita = idperstatistiche;
                stat.Testomail = cognomemittente + " " + nomemittente;
                stat.Testomail += "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                stat.Url = "";
                statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);

                ////////////////////////////////////////////////////////////////////////////////////
                //Invio mail destinatari per Regione 
                ////////////////////////////////////////////////////////////////////////////////////
                if (destinatariperregione.Count > 0)
                {
                    foreach (KeyValuePair<string, string> d in destinatariperregione)
                    {

                        maildestinatario = d.Key;
                        nomedestinatario = d.Value;
#if true

                        Utility.invioMailGenerico(nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

#endif
                        //Registro la statistica di contatto
                        stat = new Statistiche();
                        stat.Data = DateTime.Now;
                        stat.EmailDestinatario = maildestinatario;
                        stat.EmailMittente = mittenteMail;
                        stat.Idattivita = idperstatistiche;
                        stat.Testomail = cognomemittente + " " + nomemittente;
                        stat.Testomail += "<br/>" + SoggettoMail + "<br/>" + Descrizione;
                        stat.TipoContatto = enumclass.TipoContatto.invioemail.ToString();
                        stat.Url = "";
                        statisticheDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, stat);
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////

                Response.Redirect(Request.Url + "&conversione=true");

            }
            else
            {
                output.Text = Resources.Common.txtPrivacyError;
                //Mittente.Descrizione += " <br/> Non vi Autorizzo al trattamento dei miei dati personali (D.Lgs 196/2003)";
            }

        }
        catch (Exception err)
        {
            output.Text = err.Message + " <br/> ";
            lblRisposta.Text = Resources.Common.txtMailError;
        }
    }
    //private void CaricaDdlservizi(string value = "")
    //{

    //    string tipi = Resources.Common.listaServizi;
    //    Dictionary<string, string> dict = new Dictionary<string, string>();
    //    string[] tipiarray = tipi.Split(',');
    //    foreach (string s in tipiarray)
    //    {
    //        dict.Add(s, s);
    //    }

    //    ddlListaservizi.Items.Clear();
    //    ddlListaservizi.Items.Insert(0, Resources.Common.FormListaServizi.ToString());
    //    ddlListaservizi.Items[0].Value = "";
    //    ddlListaservizi.DataSource = dict;
    //    ddlListaservizi.DataTextField = "Key";
    //    ddlListaservizi.DataValueField = "Value";
    //    ddlListaservizi.DataBind();
    //    try
    //    {
    //        ddlListaservizi.SelectedItem.Text = value;
    //        if (ddlListaservizi.SelectedIndex == 0)
    //            ddlListaservizi.SelectedValue = value;
    //    }
    //    catch
    //    { }

    //}
    //private void CaricaDdlsedi(string value = "")
    //{

    //    string tipi = Resources.Common.listaSedi;
    //    Dictionary<string, string> dict = new Dictionary<string, string>();
    //    string[] tipiarray = tipi.Split(',');
    //    foreach (string s in tipiarray)
    //    {
    //        dict.Add(s, s);
    //    }

    //    ddlSedi.Items.Clear();
    //    ddlSedi.Items.Insert(0, Resources.Common.FormListaSedi.ToString());
    //    ddlSedi.Items[0].Value = "";
    //    ddlSedi.DataSource = dict;
    //    ddlSedi.DataTextField = "Key";
    //    ddlSedi.DataValueField = "Value";
    //    ddlSedi.DataBind();
    //    try
    //    {
    //        ddlSedi.SelectedItem.Text = value;
    //        if (ddlSedi.SelectedIndex == 0)
    //            ddlSedi.SelectedValue = value;
    //    }
    //    catch
    //    { }

    //}





    protected void Visualizzarisposta()
    {

        lblRisposta.Text = Resources.Common.TestoRichiestaCorretto;

        lblRisposta.Text += Resources.Common.GoogleConversione;
        plhRisposta.Visible = true;
        lblRisposta.Visible = true;

        plhForm.Visible = false;
        plhDove.Visible = false;

    }

}
