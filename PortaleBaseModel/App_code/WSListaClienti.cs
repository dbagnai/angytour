using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;

/// <summary>
/// Summary description for WSListaClienti
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WSListaClienti : System.Web.Services.WebService {

    public WSListaClienti () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [System.Web.Services.WebMethod]
    public string[] GetCompletionList(
        string prefixText, int count, string contextKey)
    {
        if (count == 0)
            count = 100;
        //ArrayList items = new ArrayList(count);
        List<string> items = new List<string>();


        //riempiamo la lista clienti
        ClientiDM cliDM = new ClientiDM();
        ClienteCollection coll = cliDM.GetLista("%" + prefixText + "%", contextKey);
        int i = count;
        foreach (Cliente item in coll)
        {
            string itemfield = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(item.Spare3, item.Id_cliente.ToString());
            items.Add(itemfield);
            i--;
            if (i == 0) break;
        }
        return items.ToArray();
    }
}
