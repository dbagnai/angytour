using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;

/// <summary>
/// Summary description for PartnersDatas
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PartnersDatas : System.Web.Services.WebService
{

    public PartnersDatas()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GetCompleteSet(string codicetipologia)
    {
        string ret = "";
        try
        {
            //Con serializzazione della classe
            WelcomeLibrary.DOM.OfferteCollection list = new WelcomeLibrary.DOM.OfferteCollection();
            offerteDM offDM = new offerteDM();
            list = offDM.CaricaOffertePerCodice(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetipologia);
            ret = dataManagement.SerializzaClasse(list);

            //Invece della serializzazione protrei creare un xml
            //con tutti i dati che poi viene parserizzato a valle 
            //e viene ricostruita la classe dai valori!!! //vedi CreazioneSitemap


        }
        catch (Exception err)
        {
            ret = err.Message;
        }
        return ret;
    }
    [WebMethod]
    public string GetCategorie(string codicetipologia)
    {
        string ret = "";
        try
        {

            List<Prodotto> prodotti = Utility.ElencoProdotti.FindAll(delegate(WelcomeLibrary.DOM.Prodotto tmp) { return ((tmp.CodiceTipologia == codicetipologia || codicetipologia == "")); });
            ProdottoCollection list = new ProdottoCollection(prodotti);
            ret = dataManagement.SerializzaClasse(list);

            //Invece della serializzazione protrei creare un xml
            //con tutti i dati che poi viene parserizzato a valle 
            //e viene ricostruita la classe dai valori!!! 


        }
        catch (Exception err)
        {
            ret = err.Message;
        }
        return ret;
    }
    [WebMethod]
    public string GetSottoCategorie(string codicetipologia)
    {
        string ret = "";
        
        try
        {
            ret = dataManagement.SerializzaClasse(WelcomeLibrary.UF.Utility.ElencoSottoProdotti);

            //Invece della serializzazione protrei creare un xml
            //con tutti i dati che poi viene parserizzato a valle 
            //e viene ricostruita la classe dai valori!!!

        }
        catch (Exception err)
        {
            ret = err.Message;
        }
        return ret;
    }
}
