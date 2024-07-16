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
                Session.Remove("objfiltro"); //Elimino Filtro modificatore che usa la sessione per selezionare i risultati visualizzati
                PercorsoComune = WelcomeLibrary.STATIC.Global.PercorsoComune;
                PercorsoFiles = WelcomeLibrary.STATIC.Global.PercorsoContenuti;


                //Prendiamo i dati dalla querystring
                Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);

                TipoContenuto = CaricaValoreMaster(Request, Session, "TipoContenuto");
                CodiceTipologia = CaricaValoreMaster(Request, Session, "CodiceTipologia");
                idOfferta = CaricaValoreMaster(Request, Session, "idOfferta");
                Dettaglioselezione = CaricaValoreMaster(Request, Session, "Dettaglioselezione");

                //if (!string.IsNullOrEmpty(idOfferta))
                //{
                HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
                metarobots.Attributes["Content"] = "noindex,follow";
                //}

                //  CaricaControlliJS();


                string conversione = CaricaValoreMaster(Request, Session, "conversione");
                if (conversione == "true")
                {
                    Visualizzarisposta();
                    return;
                }

                SettaTestoPagina();

                string linki = "";
                string linken = "";
                string linkru = "";
                string linkfr = "";
                string linkde = "";
                string linkes = "";
                string urlcorrente = HttpContext.Current.Request.Url.ToString();
                //string absoluteurlcorrente = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                //string absoluteurlandquerycorrente = HttpContext.Current.Request.Url.PathAndQuery.ToString();
                string lngfromquerys = HttpContext.Current.Request.QueryString.Get("Lingua");
                if (!string.IsNullOrEmpty(lngfromquerys))
                {
                    linki = urlcorrente.Replace("Lingua=" + lngfromquerys, "Lingua=I");
                    linken = urlcorrente.Replace("Lingua=" + lngfromquerys, "Lingua=GB");
                    linkru = urlcorrente.Replace("Lingua=" + lngfromquerys, "Lingua=RU");
                    linkes = urlcorrente.Replace("Lingua=" + lngfromquerys, "Lingua=ES");
                    linkfr = urlcorrente.Replace("Lingua=" + lngfromquerys, "Lingua=FR");
                    linkde = urlcorrente.Replace("Lingua=" + lngfromquerys, "Lingua=DE");

                }
                else if (HttpContext.Current.Request.QueryString.AllKeys.Length > 0)
                {
                    linki = (urlcorrente + "&" + "Lingua=I");
                    linken = (urlcorrente + "&" + "Lingua=GB");
                    linkru = (urlcorrente + "&" + "Lingua=RU");
                    linkfr = (urlcorrente + "&" + "Lingua=FR");
                    linkde = (urlcorrente + "&" + "Lingua=DE");
                    linkes = (urlcorrente + "&" + "Lingua=ES");
                }
                else if (HttpContext.Current.Request.QueryString.AllKeys.Length == 0)
                {
                    linki = (urlcorrente + "?" + "Lingua=I");
                    linken = (urlcorrente + "?" + "Lingua=GB");
                    linkru = (urlcorrente + "?" + "Lingua=RU");
                    linkfr = (urlcorrente + "?" + "Lingua=FR");
                    linkde = (urlcorrente + "?" + "Lingua=DE");
                    linkes = (urlcorrente + "?" + "Lingua=ES");
                }
                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatei").ToLower() != "true") linki = "";
                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activategb").ToLower() != "true") linken = "";
                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activateru").ToLower() != "true") linkru = "";
                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatees").ToLower() != "true") linkes = "";
                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatefr").ToLower() != "true") linkfr = "";
                if (WelcomeLibrary.UF.ConfigManagement.ReadKey("activatede").ToLower() != "true") linkde = "";
                SettaLinkCambioLingua(Lingua, linki, "Content_Tipo3", linken, "Content_Tipo3", linkru, "Content_Tipo3", linkfr, "Content_Tipo3", linkde, "Content_Tipo3", linkes, "Content_Tipo3");

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

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        //Carico la galleria in masterpage corretta
        string controllistBanHead = "";

        if (TipoContenuto == "")
        {
            //controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,300);";
            sb.Clear();
            sb.Append("(function wait() {");
            sb.Append("  if (typeof injectSliderAndLoadBanner === \"function\")");
            sb.Append("    {");
            sb.Append("injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-home',false,2000,300);");
            sb.Append(" }");
            sb.Append("   else  {");
            sb.Append("  setTimeout(wait, 50);");
            sb.Append("  }  })();");
        }
        else
        {
            //controllistBanHead = "injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-" + TipoContenuto + "',false,2000,300);";

            sb.Clear();
            sb.Append("(function wait() {");
            sb.Append("  if (typeof injectSliderAndLoadBanner === \"function\")");
            sb.Append("    {");
            sb.Append("injectSliderAndLoadBanner('sliderBanner.html','divSliderBanner', 'bannerslider1', 1, 2, false, '','','','TBL_BANNERS_GENERALE','header-" + TipoContenuto + "',false,2000,300);");
            sb.Append(" }");
            sb.Append("   else  {");
            sb.Append("  setTimeout(wait, 50);");
            sb.Append("  }  })();");
        }

        ClientScriptManager cs = Page.ClientScript;
        if (!cs.IsStartupScriptRegistered(this.GetType(), ""))
        {
            cs.RegisterStartupScript(this.GetType(), "controllistBanHead", sb.ToString(), true);
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
            ListItem li = new ListItem(references.ResMan("Common", Lingua, "selLocation"), "");
            ddlLocations.Items.Add(li);
            //Inseriamo la lista delle locations 
            OfferteCollection list = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, CodiceTipologia);
            if (list != null)
            {
                foreach (Offerte item1 in list)
                {
                    li = new ListItem(offDM.estraititolo(item1, Lingua), item1.Id.ToString());
                    item.DenominazionebyLingua(Lingua);
                    ddlLocations.Items.Add(li);
                }
                try
                {
                    if (item != null && item.Id != 0)
                        ddlLocations.SelectedValue = item.Id.ToString();
                }
                catch { }
            }
        }
#endif

        txtDescrizione.Text += addtext + references.ResMan("Common", Lingua, "testoMessage") + "\r\n";

        EvidenziaSelezione(TipoContenuto);


        //MODIFICA VISUALIZZAZIONE SEZIONI HTML PER FORM E POSIZIONE
        switch (TipoContenuto)
        {
            case "Dovesiamo":
                plhDove.Visible = false;
                plhForm.Visible = false;
                divRoutes.Visible = false;
                break;
            case "Prenota":
                plhDove.Visible = false;
                plhForm.Visible = true;
                pnlPrenotazione.Visible = true; //Sezione per completamento form prenotazione
                divRoutes.Visible = false;
                // divTitlemail.Visible = false;

                //CaricaDdlsedi("");
                break;
            case "Prenotaapt":
                plhDove.Visible = false;
                plhForm.Visible = true;
                divOrario.Visible = true;
                pnlPrenotazione.Visible = false; //Sezione per completamento form prenotazione
                divRoutes.Visible = false;
                // divTitlemail.Visible = false;
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
        if (references.ResMan("Common", Lingua, "testo" + TipoContenuto) != null)
            htmlPage = "<h2>" + references.ResMan("Common", Lingua, "testo" + TipoContenuto).ToString() + "</h2>";
        string strigaperricerca = Request.Url.AbsolutePath + "?" + Request.QueryString;
        if (strigaperricerca.LastIndexOf("&idOfferta=") != -1)
            strigaperricerca = strigaperricerca.Substring(0, strigaperricerca.LastIndexOf("&idOfferta="));

        Contenuti content = conDM.CaricaContenutiPerURI(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, strigaperricerca);
        if (content != null && content.Id != 0)
        {
            htmlPage = ReplaceLinks(ReplaceAbsoluteLinks(content.DescrizionebyLingua(Lingua)));

        }
        custombind cb = new custombind();
        lblContenutiContatti.Text = cb.bind(htmlPage, Lingua, Page.User.Identity.Name, Session, null, null, Request);// htmlPage;

    }

#endif



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
#if false

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
            string ragionesociale = txtSoc.Text;
            string cognomemittente = txtCognome.Text;
            string mittenteMail = txtEmail.Text;
            string nomedestinatario = Nome;
            string maildestinatario = Email;
            Dictionary<string, string> destinatariperregione = new Dictionary<string, string>();

            long idperstatistiche = 0;
            string tipo = "informazioni";
            if (TipoContenuto == "Prenota")
                tipo = "richiesta preventivo prenotazione ";
            if (TipoContenuto == "Acquistousato")
                tipo = "vendita usato ";
            string SoggettoMail = "Richiesta " + tipo + " da " + ragionesociale + " " + cognomemittente + " " + nomemittente + " tramite il sito " + Nome;
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
            Descrizione += " <br/> Nome Cliente:" + nomemittente + " Cognome   Cliente: " + cognomemittente;
            Descrizione += " <br/> Azienda:" + ragionesociale;
            Descrizione += " <br/> Telefono Cliente:" + txtTel.Text + " Email Cliente: " + mittenteMail + " Lingua Cliente: " + Lingua;
            Descrizione += " <br/> Il cliente ha Confermato l'autorizzazione al trattamento dei  dati personali ";

            if (chkNewsletter.Checked == true)
            {
                Descrizione += " <br/> Il cliente ha richiesto l'invio newsletter. " + references.ResMan("Common", Lingua, "titolonewsletter1").ToString() + "<br/>";

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
                cli.Ragsoc = ragionesociale.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
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
                //Utility.invioMailGenerico(ConfigManagement.ReadKey("Nome"), ConfigManagement.ReadKey("Email"), SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
                Utility.invioMailGenerico(cognomemittente + " " + nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

                //Registro la statistica di contatto
                Statistiche stat = new Statistiche();
                stat.Data = DateTime.Now;
                stat.EmailDestinatario = maildestinatario;
                stat.EmailMittente = mittenteMail;
                stat.Idattivita = idperstatistiche;
                stat.Testomail = ragionesociale;
                stat.Testomail += "<br/>" + cognomemittente + " " + nomemittente;
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

                        //Utility.invioMailGenerico(ConfigManagement.ReadKey("Nome"), ConfigManagement.ReadKey("Email"), SoggettoMail, Descrizione, maildestinatario, nomedestinatario);
                        Utility.invioMailGenerico(nomemittente, mittenteMail, SoggettoMail, Descrizione, maildestinatario, nomedestinatario);

#endif
                        //Registro la statistica di contatto
                        stat = new Statistiche();
                        stat.Data = DateTime.Now;
                        stat.EmailDestinatario = maildestinatario;
                        stat.EmailMittente = mittenteMail;
                        stat.Idattivita = idperstatistiche;
                        stat.Testomail = ragionesociale;
                        stat.Testomail += "<br/>" + cognomemittente + " " + nomemittente;
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
                output.Text = references.ResMan("Common", Lingua, "txtPrivacyError");
                //Mittente.Descrizione += " <br/> Non vi Autorizzo al trattamento dei miei dati personali (D.Lgs 196/2003)";
            }

        }
        catch (Exception err)
        {
            output.Text = err.Message + " <br/> ";
            lblRisposta.Text = references.ResMan("Common", Lingua, "txtMailError");
        }
    }

#endif
    //private void CaricaDdlservizi(string value = "")
    //{

    //    string tipi = references.ResMan("Common",Lingua,"listaServizi;
    //    Dictionary<string, string> dict = new Dictionary<string, string>();
    //    string[] tipiarray = tipi.Split(',');
    //    foreach (string s in tipiarray)
    //    {
    //        dict.Add(s, s);
    //    }

    //    ddlListaservizi.Items.Clear();
    //    ddlListaservizi.Items.Insert(0, references.ResMan("Common",Lingua,"FormListaServizi.ToString());
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

    //    string tipi = references.ResMan("Common",Lingua,"listaSedi;
    //    Dictionary<string, string> dict = new Dictionary<string, string>();
    //    string[] tipiarray = tipi.Split(',');
    //    foreach (string s in tipiarray)
    //    {
    //        dict.Add(s, s);
    //    }

    //    ddlSedi.Items.Clear();
    //    ddlSedi.Items.Insert(0, references.ResMan("Common",Lingua,"FormListaSedi.ToString());
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

        lblRisposta.Text = references.ResMan("Common", Lingua, "TestoRichiestaCorretto");

        lblRisposta.Text += references.ResMan("Common", Lingua, "GoogleConversione");
        plhRisposta.Visible = true;
        lblRisposta.Visible = true;

        plhForm.Visible = false;
        plhDove.Visible = false;

    }

}
