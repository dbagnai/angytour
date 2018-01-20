<%@ WebHandler Language="C#" Class="CarrelloHandler" %>
using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.IO;

public class CarrelloHandler : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        //La session grazie all'interfaccia impleemtntato la trovi in 
        //context.Session...

        //Opzionale si possono ricevere parametri anche via querystring
        string Lingua = "";
        if (context.Request.QueryString["Lingua"] != null)
            Lingua = context.Request.QueryString["Lingua"].ToString();
        string azione = "";
        if (context.Request.QueryString["Azione"] != null)
            azione = context.Request.QueryString["Azione"].ToString();
        try
        {
            string strJson = new StreamReader(context.Request.InputStream).ReadToEnd();
            //deserialize the parameters passed
            inputParameters objPar = Deserialize<inputParameters>(strJson);
            long idprodotto = 0;
            int quantita = 0;

            ///////////////////////////////////////////////// PRENDO I RIFERIMENTI DEL CLIENT PER IL CARRELLO
            //string ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (string.IsNullOrEmpty(ip.Trim()))
            //    ip = context.Request.ServerVariables["REMOTE_ADDR"].Trim();

            //if (!string.IsNullOrEmpty(ip))
            //{
            //    string[] ipRange = ip.Split(',');
            //    ip = ipRange[0].Trim();
            //}
            //else
            //    ip = context.Request.ServerVariables["REMOTE_ADDR"].Trim();
            //string sessionid = context.Session.SessionID;
            ////////////////////////////////////////////////////////////////////////////////////////////////

            switch (azione)
            {
                case "add":
                    if (objPar != null)
                    {
                        long.TryParse(objPar.codice, out idprodotto);
                        string codcaratt = objPar.codiceCaratt;
                        string q = CommonPage.CaricaQuantitaNelCarrello(context.Request, context.Session, idprodotto.ToString(), codcaratt);
                        int.TryParse(q, out quantita);
                        quantita += 1;//Incremento
                        CommonPage.AggiornaProdottoCarrello(context.Request, context.Session, idprodotto, quantita, objPar.Username);

                        //Calcolo il nuovo totale del carrello e lo ritorno per la visualizzazione
                        //WelcomeLibrary.DOM.TotaliCarrello totali = CommonPage.CalcolaTotaliCarrello(null, null, "", "");
                        //context.Response.Write(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",new object[] { totali.TotaleOrdine }) + " €");
                    }
                    else
                    {
                        context.Response.Write("No Data");
                    }
                    break;
                case "selectrowqty":
                    if (objPar != null)
                    {
                        string qty = CommonPage.CaricaQuantitaNelCarrello(context.Request, context.Session, objPar.idprodotto, objPar.idcombined);
                        context.Response.Write((qty));
                    }
                    else
                    {
                        context.Response.Write("0");
                    }
                    break;
                default:
                case "show":

                    if (objPar != null)
                    {

                        String output = CommonPage.VisualizzaCarrello(context.Request, context.Session, objPar.codice, false, objPar.Lingua);
                        // string jsonoutput = Serialize<inputParameters>(objPar);

                        //string fullName = objUsr.first_name + " " + objUsr.last_name;
                        //string age = objUsr.age;
                        //string qua = objUsr.qualification;
                        //context.Response.Write(string.Format("Name :{0} , Age={1}, Qualification={2}", fullName, age, qua));
                        // context.Response.Write(Serialize(output));
                        context.Response.Write((output));
                    }
                    else
                    {
                        context.Response.Write("No Data");
                    }

                    break;
                case "showtotal":
                    string retval = VisualizzaTotaliCarrello(context);
                    context.Response.Write((retval));
                    break;

            }


        }
        catch (Exception ex)
        {
            context.Response.Write("Error :" + ex.Message);
        }
    }
    public string VisualizzaTotaliCarrello(HttpContext context)
    {
        string ret = "";
        string sessionid = "";
        string trueIP = "";
        CommonPage.CaricaRiferimentiCarrello(context.Request, context.Session, ref sessionid, ref trueIP);
        WelcomeLibrary.DAL.eCommerceDM ecmDM = new WelcomeLibrary.DAL.eCommerceDM();
        WelcomeLibrary.DOM.CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP);
        ret = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}", new object[] { CommonPage.CalcolaTotaleCarrello(context.Request, context.Session, carrello) }) + " €";
        return ret;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }



    public class inputParameters
    {
        public string codice { get; set; }
        public string codiceCaratt { get; set; }
        public string Lingua { get; set; }
        public string Username { get; set; }
        public string idprodotto { get; set; }
        public string idcombined { get; set; }
    }


    // we create a userinfo class to hold the JSON value
    //public class userInfo
    //{
    //    public string first_name { get; set; }
    //    public string last_name { get; set; }
    //    public string qualification { get; set; }
    //    public string age { get; set; }
    //}


    // Converts the specified JSON string to an object of type T
    public T Deserialize<T>(string context)
    {
        string jsonData = context;

        //cast to specified objectType
        var obj = (T)new JavaScriptSerializer().Deserialize<T>(jsonData);
        return obj;
    }
    public string Serialize<T>(T context)
    {
        string obj = "";
        //cast to specified objectType
        obj = new JavaScriptSerializer().Serialize(context);
        return obj;
    }

}
