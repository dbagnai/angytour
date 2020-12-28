using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using WelcomeLibrary.UF;

public partial class login : CommonPage
{
    public string Lingua
    {
        get { return ViewState["Lingua"] != null ? (string)(ViewState["Lingua"]) : deflanguage; }
        set { ViewState["Lingua"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Prendiamo i dati dalla querystring
            Lingua = CaricaValoreMaster(Request, Session, "Lingua", false, deflanguage);
            HtmlMeta metarobots = (HtmlMeta)Master.FindControl("metaRobots");
            metarobots.Attributes["Content"] = "noindex,follow";

            #region SEZIONE MASTERPAGE GESTIONE

            HtmlGenericControl divSubheader = (HtmlGenericControl)Master.FindControl("divSubheader");
            if (divSubheader != null) divSubheader.Visible = false;
            //Master.CaricaBannerHomegallery("TBL_BANNERS_GENERALE", 0, 0, "null", false, Lingua);

            #endregion

            if (Session["Errororder"] != null)
            {
                output.Text = Session["Errororder"].ToString();
                Session.Remove("Errororder");
            }

            // se utilizzi le risorse fare dataBind
            DataBind();
        }


    }
    protected void loginbtn_Click(object sender, EventArgs e)
    {
        HtmlInputText usr = (HtmlInputText)LogView1.Controls[0].Controls[0].FindControl("inputName");
        HtmlInputText psw = (HtmlInputText)LogView1.Controls[0].Controls[0].FindControl("inputPassword");
        Label outlogin = (Label)LogView1.Controls[0].Controls[0].FindControl("outputlogin");

        string username = usr.Value;
        string password = psw.Value;
        if (controllobloccoaccesso(username))
        {
            outlogin.Text = "Accesso non consentito. Contattare l'amministratore!";
            return;
        }
        if (Membership.ValidateUser(username, password))
        {
            //FormsAuthentication.LoginUrl = references.ResMan("Common",Lingua,"Linklogin");
            //FormsAuthentication.DefaultUrl
            FormsAuthentication.RedirectFromLoginPage(username, false);
            //FormsAuthentication.Authenticate(username, password);
        }
        else
        {
            outlogin.Text = "Se sei un nuovo utente, effettua la registrazione a lato.";
        }
    }

    protected void btnIscriviti_Click(object sender, EventArgs e)
    {

    }
    protected void btnForget_Click(object sender, EventArgs e)
    {
        usermanager USM = new usermanager();
        HtmlInputText usr = (HtmlInputText)LogView1.Controls[0].Controls[0].FindControl("inputName");
        if (string.IsNullOrEmpty(usr.Value.Trim()))
        {
            output.Text = references.ResMan("Common", Lingua, "forgetRequest1");
        }
        else
            //InviaMailForgetSocio(usr.Value);
            //InviaMailForgetCliente(usr.Value);
            output.Text = USM.SendAccessData(Lingua, usr.Value, Email, Nome);
    }


    private bool controllobloccoaccesso(string username)
    {
        bool ret = false;

        MembershipUser utente = Membership.GetUser(username, false);
        if (utente != null) //utente esistente
        {
            string idsocio = getidsocio(username);
            Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idsocio);
            if (Details != null)
            {
                if (Details.Bloccoaccesso_dts) ret = true;
            }
        }
        return ret;
    }
    /// <summary>
    /// Invia la mail di modifica pass al cliente
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    private string InviaMailForgetSocio(string username)
    {
        string ret = "";
        try
        {
            MembershipUser utente = Membership.GetUser(username, false);
            if (utente != null) //utente esistente
            {
                string newpassword = WelcomeLibrary.UF.RandomPassword.Generate(8);
                string idsocio = getidsocio(username);
                Offerte Details = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idsocio);
                if (Details != null)
                {
                    string resetpass = utente.ResetPassword();
                    if (utente.ChangePassword(resetpass, newpassword))
                    {

                        string SoggettoMail = Nome + " : " + "New password for " + Nome;

                        //Dati per la mail
                        string nomecliente = Details.Cognome_dts + " " + Details.Nome_dts;
                        string Mailcliente = Details.Emailriservata_dts;
                        string Descrizione = "<br/>";

                        Descrizione += references.ResMan("Common", Lingua, "forgetResponse1").ToString() + "<br/><br/>";
                        Descrizione += references.ResMan("Common", Lingua, "FormTesto2").ToString() + " " + Details.Nome_dts + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        Descrizione += references.ResMan("Common", Lingua, "FormTesto3").ToString() + " " + Details.Cognome_dts + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        Descrizione += references.ResMan("Common", Lingua, "FormTesto4").ToString() + " " + Details.Emailriservata_dts + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        Descrizione += references.ResMan("Common", Lingua, "FormTestoPass").ToString() + " " + newpassword + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

                        ////Province p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate(Province _p) { return _p.Codice == cliente.CodiceREGIONE; });
                        ////if (p != null)
                        ////    Descrizione += references.ResMan("Common",Lingua,"FormTesto6").ToString() + " " + p.Regione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        ////p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate(Province _p) { return _p.Codice == cliente.CodicePROVINCIA; });
                        ////if (p != null)
                        ////    Descrizione += references.ResMan("Common",Lingua,"FormTesto7").ToString() + " " + p.Provincia + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        ////Comune c = WelcomeLibrary.UF.Utility.ElencoComuni.Find(delegate(Comune _p) { return _p.Nome.ToLower() == cliente.CodiceCOMUNE.ToLower(); });
                        //if (c != null)
                        //    Descrizione += references.ResMan("Common",Lingua,"FormTesto8").ToString() + " " + c.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

                        //Descrizione += references.ResMan("Common",Lingua,"FormTesto9").ToString() + " " + cliente.Cap + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        //Descrizione += references.ResMan("Common",Lingua,"FormTesto10").ToString() + " " + cliente.Indirizzo + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        //Descrizione += references.ResMan("Common",Lingua,"FormTesto11").ToString() + " " + cliente.Telefono + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        //Descrizione += references.ResMan("Common",Lingua,"FormTesto12").ToString() + " " + cliente.Professione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                        //Descrizione += references.ResMan("Common",Lingua,"FormTesto13").ToString() + " " + cliente.DataNascita.ToShortDateString() + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione

                        //Descrizione += "<a href=\"" + linkvalidazione + "\" target=\"_blank\" style=\"font-size:18px\">" + references.ResMan("Common",Lingua,"testoLinkValidazioneAttivazione").ToString() + "<br/>";
                        Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente);
                        output.Text = references.ResMan("Common", Lingua, "forgetResponse2");
                    }

                }
            }
            else
                output.Text = references.ResMan("Common", Lingua, "forgetResponse3");

        }
        catch (Exception err)
        {
            ret = "Error sending password recover. contact us directly. /  Errore invio mail recupero pass.Contattare l'assistenza.";
            output.Text = ret + " " + err.Message;
        }
        return ret;
    }

    /// <summary>
    /// Invia la mail di modifica pass al cliente
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    private string InviaMailForgetCliente(string email, string idtipocliente = "0")
    {
        string ret = "";
        try
        {
            usermanager USM = new usermanager();
            ClientiDM cliDM = new ClientiDM();
            Cliente cliente = new Cliente();
            string username = email; //Ipotizzo che mi possa passare l'username invece della mail
            string idcliente = getidcliente(username); //prendo l'id anagrafica associato al cliente loggato ( se disponibile )
            cliente = cliDM.CaricaClientePerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idcliente); //prende il cliente per idcliente con qualsiasi tipologia questo 
            if (cliente == null || cliente.Id_cliente == 0) //se non trovo l'utente con l'username allora lo cerco per email
                cliente = cliDM.CaricaClientePerEmail(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, email, idtipocliente);
            if (cliente != null && cliente.Id_cliente != 0) username = cliente.Id_cliente.ToString() + "-" + cliente.Email;

            if (USM.VerificaPresenzaUtente(username) && cliente != null && cliente.Id_cliente != 0)  //Cliente esistente ed utente esistente
            {
                string newpassword = WelcomeLibrary.UF.RandomPassword.Generate(8);
                MembershipUser utente = Membership.GetUser(username, false);
                if (utente != null)
                {
                    string resetpass = utente.ResetPassword();
                    if (utente.ChangePassword(resetpass, newpassword))
                    {
                        string SoggettoMail = Nome + " cambio password ";
                        //Dati per la mail
                        string nomecliente = cliente.Cognome + " " + cliente.Nome;
                        string Mailcliente = cliente.Email;
                        string Descrizione = "New password set for " + username + "<br/>";
                        Descrizione += references.ResMan("Common", Lingua, "forgetResponse1").ToString() + "<br/><br/>";

#if false  //riepilogo dati cliente nel form
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto2").ToString() + " " + cliente.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto3").ToString() + " " + cliente.Cognome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto4").ToString() + " " + cliente.Email + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTestoPass").ToString() + " " + newpassword + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto5").ToString() + " " + cliente.CodiceNAZIONE + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Province p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate (Province _p) { return _p.Codice == cliente.CodiceREGIONE; });
                    if (p != null)
                        Descrizione += references.ResMan("Common", Lingua, "FormTesto6").ToString() + " " + p.Regione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    p = WelcomeLibrary.UF.Utility.ElencoProvinceCompleto.Find(delegate (Province _p) { return _p.Codice == cliente.CodicePROVINCIA; });
                    if (p != null)
                        Descrizione += references.ResMan("Common", Lingua, "FormTesto7").ToString() + " " + p.Provincia + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Comune c = WelcomeLibrary.UF.Utility.ElencoComuni.Find(delegate (Comune _p) { return _p.Nome.ToLower() == cliente.CodiceCOMUNE.ToLower(); });
                    if (c != null)
                        Descrizione += references.ResMan("Common", Lingua, "FormTesto8").ToString() + " " + c.Nome + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto9").ToString() + " " + cliente.Cap + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto10").ToString() + " " + cliente.Indirizzo + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto11").ToString() + " " + cliente.Telefono + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto12").ToString() + " " + cliente.Professione + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione
                    Descrizione += references.ResMan("Common", Lingua, "FormTesto13").ToString() + " " + cliente.DataNascita.ToShortDateString() + "<br/>";//Qui devo riepilogare tutti i dati inseriti dal cliente nel form di attivazione 
#endif
                        //Descrizione += "<a href=\"" + linkvalidazione + "\" target=\"_blank\" style=\"font-size:18px\">" + references.ResMan("Common",Lingua,"testoLinkValidazioneAttivazione").ToString() + "<br/>";
                        Utility.invioMailGenerico(Nome, Email, SoggettoMail, Descrizione, Mailcliente, nomecliente);
                        output.Text = references.ResMan("Common", Lingua, "forgetResponse2");
                    }
                }

            }
            else
                output.Text = references.ResMan("Common", Lingua, "forgetResponse3");
        }
        catch (Exception err)
        {
            ret = "Error sending password recover. contact us directly. /  Errore invio mail recupero pass.Contattare l'assistenza." + " " + err.Message;
            output.Text = ret + " " + err.Message;
        }
        return ret;
    }
}