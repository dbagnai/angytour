<%@ WebHandler Language="C#" Class="CarrelloHandler" %>
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WelcomeLibrary.UF;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;

public class CarrelloHandler : IHttpHandler, IRequiresSessionState
{
    public Dictionary<string, string> parseparams(HttpContext context)
    {
        Dictionary<string, string> pars = new Dictionary<string, string>();
        bool isPost = false;
        isPost = context.Request.HttpMethod.ToUpper() == "POST";
        bool ismultipart = false;
        if (context.Request.ContentType.ToLower().Contains("multipart/form-data")) ismultipart = true;

        if (isPost && !ismultipart)
            pars = HandlerHelper.GetPostParams(context);
        foreach (var item in context.Request.Params.Keys)
        {
            if (item != null)
            {
                string szKey = item.ToString();
                if (!pars.ContainsKey(szKey))
                    pars.Add(szKey, context.Request.Params[szKey].ToString());
            }
        }

        return pars;
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        Dictionary<string, string> pars = parseparams(context);

        //Opzionale si possono ricevere parametri anche via querystring
        //string Lingua = "";
        //if (context.Request.QueryString["Lingua"] != null)
        //   Lingua = context.Request.QueryString["Lingua"].ToString();
        //string azione = "";
        //if (context.Request.QueryString["Azione"] != null)
        //    azione = context.Request.QueryString["Azione"].ToString();
        //string single = "";
        //if (context.Request.QueryString["single"] != null)
        //    single = context.Request.QueryString["single"].ToString();

        string sazione = pars.ContainsKey("Azione") ? pars["Azione"] : "";
        string mode = pars.ContainsKey("mode") ? pars["mode"] : "";
        string sprezzo = pars.ContainsKey("prezzo") ? pars["prezzo"] : "";
        string sdatastart = pars.ContainsKey("datastart") ? pars["datastart"] : "";
        string sdataend = pars.ContainsKey("dataend") ? pars["dataend"] : "";
        string scodice = pars.ContainsKey("codice") ? pars["codice"] : "";
        //string scodiceCaratt = pars.ContainsKey("codiceCaratt") ? pars["codiceCaratt"] : "";
        string sidcarrello = pars.ContainsKey("idcarrello") ? pars["idcarrello"] : "";
        string sidprodotto = pars.ContainsKey("idprodotto") ? pars["idprodotto"] : "";
        string sidcombined = pars.ContainsKey("idcombined") ? pars["idcombined"] : "";
        string sUsername = pars.ContainsKey("Username") ? pars["Username"] : "";
        string jsonfield1 = pars.ContainsKey("jsonfield1") ? pars["jsonfield1"] : "";
        string forceidcarrello = pars.ContainsKey("forceidcarrello") ? pars["forceidcarrello"] : "";

        string Lingua = pars.ContainsKey("Lingua") ? pars["Lingua"] : "";
        string output = "";

        try
        {
            //string strJson = new StreamReader(context.Request.InputStream).ReadToEnd();
            //deserialize the parameters passed
            //inputParameters objPar = Deserialize<inputParameters>(strJson);
            // inputParameters objPar = Newtonsoft.Json.JsonConvert.DeserializeObject<inputParameters>(strJson); //Se un campo dei parametri è un json v+ in crisi
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

            long idprodotto = 0;
            long idcarrello = 0;
            long quantita = 0;
            double prezzo = 0;
            string returnedidcarrello = "";
            DateTime? datastart = null;
            DateTime? dataend = null;
            DateTime tmp = DateTime.MinValue;
            //string codcaratt = scodiceCaratt;

            switch (sazione)
            {
                case "add":
                    double.TryParse(sprezzo, out prezzo);
                    if (DateTime.TryParseExact(sdatastart, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tmp)) datastart = tmp;
                    if (DateTime.TryParseExact(sdataend, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tmp)) dataend = tmp;
                    long.TryParse(sidprodotto, out idprodotto);
                    long.TryParse(sidcarrello, out idcarrello);
                    //Variabile per forzare il caricamento del carrello solo per id tbl_carrello in modo da consentire inserimenti multipli per uno stesso prodotto
                    bool bforceidcarrello = false;
                    bool.TryParse(forceidcarrello, out bforceidcarrello);

                    if (mode == "single") quantita = 1;
                    else
                    {
                        if (string.IsNullOrEmpty(mode)) //se mode è string vuota incremento la quantita presente a carrello
                        {
                            string q = CommonPage.CaricaQuantitaNelCarrello(context.Request, context.Session, idprodotto.ToString(), sidcombined, sidcarrello);
                            long.TryParse(q, out quantita);
                            if (bforceidcarrello && (string.IsNullOrEmpty(sidcarrello) || sidcarrello == "0")) quantita = 0; //Azzero la quantità se idcarrello =0 0 e modalità forced per inserimenti multipli
                            quantita += 1;//Incremento sempre id 1 unità
                        }
                        else //se mode contiene un numero memorizzo quella quantità nel carrello imponendo il valore passato nella richiesta come totale
                        {
                            long.TryParse(mode, out quantita);
                        }
                    }

                    returnedidcarrello = CommonPage.AggiornaProdottoCarrello(context.Request, context.Session, idprodotto, quantita, sUsername, sidcombined, idcarrello, 0, prezzo, datastart, dataend, jsonfield1, bforceidcarrello);
                    context.Response.Write((returnedidcarrello));

                    //Calcolo il nuovo totale del carrello e lo ritorno per la visualizzazione
                    //WelcomeLibrary.DOM.TotaliCarrello totali = CommonPage.CalcolaTotaliCarrello(null, null, "", "");
                    //context.Response.Write(String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:N2}",new object[] { totali.TotaleOrdine }) + " €");

                    break;
                case "subtract":
                    double.TryParse(sprezzo, out prezzo);
                    if (DateTime.TryParseExact(sdatastart, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tmp)) datastart = tmp;
                    if (DateTime.TryParseExact(sdataend, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tmp)) dataend = tmp;
                    long.TryParse(sidprodotto, out idprodotto);
                    long.TryParse(sidcarrello, out idcarrello);
                    //Variabile per forzare il caricamento del carrello solo per id tbl_carrello in modo da consentire inserimenti multipli per uno stesso prodotto
                    bool b1forceidcarrello;
                    bool.TryParse(forceidcarrello, out b1forceidcarrello);
                    if (mode == "single") quantita = 1;
                    else
                    {
                        if (string.IsNullOrEmpty(mode)) //se mode è string vuota incremento la quantita presente a carrello
                        {
                            string q = CommonPage.CaricaQuantitaNelCarrello(context.Request, context.Session, idprodotto.ToString(), sidcombined, sidcarrello);
                            long.TryParse(q, out quantita);
                            if (b1forceidcarrello && (string.IsNullOrEmpty(sidcarrello) || sidcarrello == "0")) quantita = 0; //Azzero la quantità se idcarrello =0 0 e modalità forced per inserimenti multipli

                            quantita -= 1;//Incremento sempre id 1 unità
                        }
                        else //se mode contiene un numero memorizzo quella quantità nel carrello imponendo il valore passato nella richiesta come totale
                        {
                            long.TryParse(mode, out quantita);
                        }
                    }

                    returnedidcarrello = CommonPage.AggiornaProdottoCarrello(context.Request, context.Session, idprodotto, quantita, sUsername, sidcombined, idcarrello, 0, prezzo, datastart, dataend, jsonfield1, b1forceidcarrello);
                    context.Response.Write((returnedidcarrello));
                    break;
                case "svuotacarrello":
                    CommonPage.SvuotaCarrello(context.Request, context.Session);
                    context.Response.Write("");
                    break;
                case "cancellabyid":
                    long idc = 0;
                    if (long.TryParse(sidcarrello, out idc))
                    {
                        CommonPage.SvuotaCarrello(context.Request, context.Session, idc);
                    }
                    break;
                case "selectrowqty":
                    string qty = "0";
                    qty = CommonPage.CaricaQuantitaNelCarrello(context.Request, context.Session, sidprodotto, sidcombined, sidcarrello);

                    //Se modalità forced elementi multipili e idcarrello 0 -> annullo le quantità
                    bool b2forceidcarrello;
                    bool.TryParse(forceidcarrello, out b2forceidcarrello);
                    if (b2forceidcarrello && (string.IsNullOrEmpty(sidcarrello) || sidcarrello == "0")) qty = "0";

                    context.Response.Write((qty));

                    break;
                default:
                case "show":
                    output = CommonPage.VisualizzaCarrello(context.Request, context.Session, scodice, false, Lingua);

                    context.Response.Write((output));
                    break;
                case "getitemscarrello":
                    output = CommonPage.VisualizzaCarrello(context.Request, context.Session, scodice, true, Lingua, true);
                    context.Response.Write((output));
                    break;
                case "showtotal":
                    output = VisualizzaTotaliCarrello(context);
                    context.Response.Write((output));
                    break;
                case "showtotalforproduct":
                    output = VisualizzaTotaliCarrello(context, sidprodotto, sidcombined, sidcarrello);
                    context.Response.Write((output));
                    break;
                case "getpriceforproduct":
                    offerteDM offDM = new offerteDM();
                    Offerte off = offDM.CaricaOffertaPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sidprodotto);
                    if (off != null)
                    {
                        output = off.Prezzo.ToString();
                    }
                    context.Response.Write((output));
                    break;

            }

        }
        catch (Exception ex)
        {
            string er = ex.Message;
            context.Response.StatusCode = 400;
            context.Response.Write((er));

        }
    }
    public string VisualizzaTotaliCarrello(HttpContext context, string idprodotto = "", string idcombined = "", string idcarrello = "")
    {
        string ret = "";
        string sessionid = "";
        string trueIP = "";

        long lprod = 0;
        long.TryParse(idprodotto, out lprod);
        long lcarr = 0;
        long.TryParse(idcarrello, out lcarr);

        CommonPage.CaricaRiferimentiCarrello(context.Request, context.Session, ref sessionid, ref trueIP);
        WelcomeLibrary.DAL.eCommerceDM ecmDM = new WelcomeLibrary.DAL.eCommerceDM();
        WelcomeLibrary.DOM.CarrelloCollection carrello = ecmDM.CaricaCarrello(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, sessionid, trueIP, lprod, idcombined, lcarr);
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


#if false

    public class inputParameters
    {
        public string idcarrello { get; set; }
        public string codice { get; set; }
        public string codiceCaratt { get; set; }
        public string Lingua { get; set; }
        public string Username { get; set; }
        public string idprodotto { get; set; }
        public string idcombined { get; set; }
        public string prezzo { get; set; }
        public string datastart { get; set; }
        public string dataend { get; set; }
        //public Dictionary<string, string> jsonfield1 { get; set; }
        public string jsonfield1 { get; set; }
    }


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

#endif
}
