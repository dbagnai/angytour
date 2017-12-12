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
//using GELibraryRemoto.UF;
//using GELibraryRemoto.DOM;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;

public partial class _SchedaResourceStampa : CommonPage
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
    protected void Page_Load(object sender, EventArgs e)
    {
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
  
      

    protected void ScriptManagerMaster_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {

        ((ScriptManager)sender).AsyncPostBackErrorMessage = e.Exception.Message.ToString();

        //Argomento di postback o callback non valido. 
        //La convalida degli eventi viene abilitata mediante <pages enableEventValidation="true"/> 
        //nella configurazione oppure mediante <%@ Page EnableEventValidation="true" %> in una pagina.
        //Per motivi di sicurezza, viene verificato che gli argomenti con cui eseguire il postback o 
        //il callback di eventi siano originati dal controllo server che ne aveva inizialmente eseguito 
        //il rendering. Se i dati sono validi e previsti, utilizzare il metodo 
        //ClientScriptManager.RegisterForEventValidation per registrare i dati di postback o callback 
        //per la convalida.

    }






}