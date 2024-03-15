using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;

/// <summary>
/// Summary description for NVPAPICaller
/// </summary>
public class NVPAPICaller
{
    //private static readonly ILog log = LogManager.GetLogger(typeof(NVPAPICaller));

    public string pendpointurl = "https://api-3t.paypal.com/nvp";
    public const string CVV2 = "CVV2";

    //Flag that determines the PayPal environment (live or sandbox)
    private bool bSandbox = true;

    private const string SIGNATURE = "SIGNATURE";
    private const string PWD = "PWD";
    private const string ACCT = "ACCT";

    //Replace <API_USERNAME> with your API Usernaem
    //Replace <API_PASSWORD> with your API Password
    //Replace <API_SIGNATURE> with your Signature
    private string APIUsername = "<API_USERNAME>";
    private string APIPassword = "<API_PASSWORD>";
    private string APISignature = "<API_SIGNATURE>";
    private string Subject = "";
    private string BNCode = "PP-ECWizard";

    //
    private string returnURL = "";
    private string cancelURL = "";


    //HttpWebRequest Timeout specified in milliseconds 
    private const int Timeout = 10000;
    private static readonly string[] SECURED_NVPS = new string[] { ACCT, CVV2, SIGNATURE, PWD };


    public NVPAPICaller(string Userid, string Pwd, string Signature, string returnurl, string cancelurl, bool sandbox = true)
    {
        SetCredentials(Userid, Pwd, Signature);
        returnURL = returnurl;
        cancelURL = cancelurl;
        bSandbox = sandbox;
    }

    /// <summary>
    /// Sets the API Credentials
    /// </summary>
    /// <param name="Userid"></param>
    /// <param name="Pwd"></param>
    /// <param name="Signature"></param>
    /// <returns></returns>
    public void SetCredentials(string Userid, string Pwd, string Signature)
    {
        APIUsername = Userid;
        APIPassword = Pwd;
        APISignature = Signature;
    }

    /// <summary>
    /// ShortcutExpressCheckout: The method that calls SetExpressCheckout API
    /// </summary>
    /// <param name="amt"></param>
    /// <param ref name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool ShortcutExpressCheckout(Dictionary<string, List<string>> paypaldatas, string Lingua, ref string token, ref string retMsg, bool authandcapture = false)
    {

        //https://developer.paypal.com/docs/classic/api/apiCredentials/
        //https://www.paypal.com/businessexp/summary

        //https://www.x.com/developers/paypal/documentation-tools/api/setexpresscheckout-api-operation-nvp
        //https://developer.paypal.com/docs/classic/express-checkout/integration-guide/ECCustomizing/

        string host = "www.paypal.com";
        if (bSandbox)
        {
            pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
            //pendpointurl = "https://api-3t.sandbox.paypal.com/2.0/";
            host = "www.sandbox.paypal.com";
        }

        //returnURL = "http://www.test.it";
        //cancelURL = "http://www.tnoest.it";

        //https://developer.paypal.com/docs/classic/express-checkout/integration-guide/ECCustomizing/
        NVPCodec encoder = new NVPCodec();
        encoder["METHOD"] = "SetExpressCheckout";
        encoder["RETURNURL"] = returnURL;
        encoder["CANCELURL"] = cancelURL;

        if (authandcapture)
            encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Authorization"; //-> FOR AUTHORIZATION AND CAPTURE METHOD
        else
            encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";//Direct payment

        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "EUR";
        //encoder["PAYMENTREQUEST_0_DESC"] = "Descrizione generale del pagamento da definire";
        //PAYMENTREQUEST_0_NOTETEX

        //encoder["EMAIL"] = "...";//Prefill email of buyer
        encoder["NOSHIPPING"] = "1"; //EVITA DI RICHIEDERE I DATI DI SPEDIZIONE IN PAYPAL
        //encoder["ADDRESSOVERRIDE"] = "0"; //AddressOverride
        encoder["ADDROVERRIDE"] = "0";
        if (Lingua == "I") Lingua = "IT";
        encoder["LOCALECODE"] = Lingua.ToUpper(); //IT oppure GB  -> nazione delle pagine nella visualizzazione paypal
        encoder["LANDINGPAGE"] = "Billing"; //login  -> mostra la pagina di login oppure il pagamento diretto
        encoder["SOLUTIONTYPE"] = "Sole";
        //ALLOWNOTE


        double totaleforcheck = 0; //totale precalcolato da richiedere ( comprende tutti i costo : articoli, spezione etc... 
        double totalamount = 0;  //totale calcolato per la richiesta ( comprende gli articoli ed anche gli atri costi aggiuntivi
                                 //che sono preparati nella memorizzazione paypaldatas prima della chiamata al pagamento)

        int i = 0;
        foreach (string item in paypaldatas.Keys)//scorro gli elementi a carrello e i costi  aggiuntivi ( preparati in precedenza ) per costruire la richiesta
        {
            //paypaldatas contiene indicizzato per ogni id i seguenti in sequenza
            List<string> dettaglio = paypaldatas[item];
            //testo1
            string testo1 = dettaglio[0]; //id prodotto o vuoto nel caso dei costi aggiuntivi (es. spedizione,assicu, sconto etc..)
            //testo2
            string testo2 = dettaglio[1]; //titolo rigo
            //testo3
            string testo3 = dettaglio[2]; //descrizione rigo
            //prezzounitario
            double d = 0;
            double.TryParse(dettaglio[3], out d); //prezzo rigo
#if false
            if (bSandbox)//In sandbox mode il prezzo è sempre 0,1 per evitar di consumare fondi
            {
                if (d > 0)
                    d = 0.1;
                else if (d < 0)
                    d = -0.1;
            } 
#endif
            if (d == 0) continue;//Non aggiungo gli ementi a costo zero -> altrimenti il sistema si irrita
            if (item.ToLower() == "pp_totalamountcheck")
            {
                double d1 = 0;
                double.TryParse(dettaglio[3], out d1); //prezzo totale per controllo
                totaleforcheck = d1;
                continue;
            }

            string prezzounitario = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), "{0:N2}", new object[] { d });

            //ncamere
            int quantita = 0;
            int.TryParse(dettaglio[4], out quantita);  //quantita rigo
            string strquantita = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), "{0:N0}", new object[] { quantita });
            //percentualeanticipo
            double percanticipo = 0;
            double.TryParse(dettaglio[5], out percanticipo); //%anticipo
            //Dettaglio articoli
#if true
            //encoder["L_PAYMENTREQUEST_0_NUMBER" + i.ToString()] = "12334.00"; //item number
            encoder["L_PAYMENTREQUEST_0_NAME" + i.ToString()] = testo2.Substring(0, (testo2.Length <= 60) ? testo2.Length : 60);
            encoder["L_PAYMENTREQUEST_0_DESC" + i.ToString()] = testo3.Substring(0, (testo3.Length <= 126) ? testo3.Length : 126);
            //encoder["L_PAYMENTREQUEST_0_AMT" + i.ToString()] = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), "{0:N2}", new object[] {d * quantita * percanticipo / 100});

            //encoder["L_PAYMENTREQUEST_0_AMT" + i.ToString()] = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), "{0:N2}", new object[] { d });
            //Inserisco la percentuale di acconto su tutti gli articoli
            encoder["L_PAYMENTREQUEST_0_AMT" + i.ToString()] = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), "{0:N2}", new object[] { d * percanticipo / 100 });
            encoder["L_PAYMENTREQUEST_0_QTY" + i.ToString()] = quantita.ToString();
            encoder["L_PAYMENTREQUEST_0_ITEMCATEGORY" + i.ToString()] = "Physical"; //L_PAYMENTREQUEST_n_ITEMCATEGORYm Digital Physical  
            //encoder["L_PAYMENTREQUEST_0_ITEMCATEGORY" + i.ToString()] = "Digital"; //L_PAYMENTREQUEST_n_ITEMCATEGORYm Digital Physical  
#endif
            totalamount += d * quantita * percanticipo / 100;
            i++;
        }
        //totaleforcheck e totalamount devono corrispondere sempre 

        //Totale costo articoli (formato numeri sempre xxxxxxxx.xx )
        encoder["PAYMENTREQUEST_0_ITEMAMT"] = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), "{0:N2}", new object[] { totalamount });
        encoder["PAYMENTREQUEST_0_AMT"] = String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-EN"), "{0:N2}", new object[] { totalamount });

        //tramite i seguenti puoi preimpostare i dati di fatturazione/spedizione
        //        &PAYMENTREQUEST_0_SHIPTONAME=John Smith
        //        &PAYMENTREQUEST_0_SHIPTOSTREET=1 Main Street
        //&PAYMENTREQUEST_0_SHIPTOCITY=San Jose
        //&PAYMENTREQUEST_0_SHIPTOSTATE=CA
        //&PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE=US
        //&PAYMENTREQUEST_0_SHIPTOZIP=95131
        //&PAYMENTREQUEST_0_EMAIL=jsmith01@example.com
        //&PAYMENTREQUEST_0_SHIPTOPHONENUM=408-559-5948
        //////////////////////////////////////////////////////////


        //SAMPLE DATAILS
        //        [requiredSecurityParameters]&METHOD=SetExpressCheckout&RETURNURL=http://coff
        //ee2go.com&CANCELURL=http://cancel.com&PAYMENTACTION=Sale&EMAIL=jsmith0
        //1@example.com&NAME=J Smith&SHIPTOSTREET=1 Main St&SHIPTOCITY=San
        //Jose&SHIPTOSTATE=CA&SHIPTOCOUNTRYCODE=US&SHIPTOZIP=95131&L_NAME0=10%
        //Decaf Kona Blend Coffee&L_NUMBER0=623083&L_DESC0=Size: 8.8-
        //oz&L_AMT0=9.95&L_QTY0=2&L_NAME1=Coffee Filter
        //bags&L_NUMBER1=6230&L_DESC1=Size: Two 24-piece
        //boxes&L_AMT1=39.70&L_QTY1=2&ITEMAMT=99.30&TAXAMT=2.59&SHIPPINGAMT=3.00
        //&HANDLINGAMT=2.99&SHIPDISCAMT=-
        //3.00&INSURANCEAMT=1.00&AMT=105.88&CURRENCYCODE=USD&ALLOWNOTE=1

        //FACCIAMO LA CHIAMATA API
        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);
        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        //SE SUCCESSO MEMORIZZO L'INDIRIZZO PAYPAL A CUI FARE IL REDIRECT PER AUTORIZZARE LA TRANSAZIONE
        //CON I PARAMETRI NECESSARI
        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            token = decoder["TOKEN"];
            //decoder["PAYERID"]; eventualmente puoi anche prendere il payerid
            string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;//+ "&useraction=commit"
            retMsg = ECURL;
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }

    /// <summary>
    /// MarkExpressCheckout: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="amt"></param>
    /// <param ref name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool MarkExpressCheckout(string amt,
                        string shipToName, string shipToStreet, string shipToStreet2,
                        string shipToCity, string shipToState, string shipToZip,
                        string shipToCountryCode, ref string token, ref string retMsg)
    {
        string host = "www.paypal.com";
        if (bSandbox)
        {
            pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
            host = "www.sandbox.paypal.com";
        }

        //returnURL = "http://www.test.it";
        //cancelURL = "http://www.tnoest.it";

        NVPCodec encoder = new NVPCodec();
        encoder["METHOD"] = "SetExpressCheckout";
        encoder["RETURNURL"] = returnURL;
        encoder["CANCELURL"] = cancelURL;
        encoder["PAYMENTREQUEST_0_AMT"] = amt;
        encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "EUR";

        //Optional Shipping Address entered on the merchant site
        encoder["PAYMENTREQUEST_0_SHIPTONAME"] = shipToName;
        encoder["PAYMENTREQUEST_0_SHIPTOSTREET"] = shipToStreet;
        encoder["PAYMENTREQUEST_0_SHIPTOSTREET2"] = shipToStreet2;
        encoder["PAYMENTREQUEST_0_SHIPTOCITY"] = shipToCity;
        encoder["PAYMENTREQUEST_0_SHIPTOSTATE"] = shipToState;
        encoder["PAYMENTREQUEST_0_SHIPTOZIP"] = shipToZip;
        encoder["PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE"] = shipToCountryCode;


        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            token = decoder["TOKEN"];

            string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;

            retMsg = ECURL;
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }



    /// <summary>
    /// GetShippingDetails: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool GetShippingDetails(string token, ref string PayerId, ref string email, ref string firstname, ref string lastname, ref string amount, ref string ShippingAddress, ref string retMsg)
    {

        if (bSandbox)
        {
            pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
        }

        NVPCodec encoder = new NVPCodec();
        encoder["METHOD"] = "GetExpressCheckoutDetails";
        encoder["TOKEN"] = token;

        //API CALL
        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);
        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);
        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            email = decoder["EMAIL"];
            firstname = decoder["FIRSTNAME"];
            lastname = decoder["LASTNAME"];
            PayerId = decoder["PAYERID"];
            amount = decoder["AMT"];
            ShippingAddress = "<table><tr>";
            ShippingAddress += "<td colspan='2'> Shipping Address</td></tr>";
            ShippingAddress += "<td> Name </td><td>" + decoder["PAYMENTREQUEST_0_SHIPTONAME"] + "</td></tr>";
            ShippingAddress += "<td> Street1 </td><td>" + decoder["PAYMENTREQUEST_0_SHIPTOSTREET"] + "</td></tr>";
            ShippingAddress += "<td> Street2 </td><td>" + decoder["PAYMENTREQUEST_0_SHIPTOSTREET2"] + "</td></tr>";
            ShippingAddress += "<td> City </td><td>" + decoder["PAYMENTREQUEST_0_SHIPTOCITY"] + "</td></tr>";
            ShippingAddress += "<td> State </td><td>" + decoder["PAYMENTREQUEST_0_SHIPTOSTATE"] + "</td></tr>";
            ShippingAddress += "<td> Zip </td><td>" + decoder["PAYMENTREQUEST_0_SHIPTOZIP"] + "</td>";
            ShippingAddress += "</tr></table>";

            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }


    /// <summary>
    /// ConfirmPayment: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool ConfirmPayment(string finalPaymentAmount, string token, string PayerId, ref string retMsg, bool authandcapture = false) //ref NVPCodec decoder, 
    {
        //https://www.x.com/developers/paypal/documentation-tools/api/doexpresscheckoutpayment-api-operation-nvp
        if (bSandbox)
        {
            pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
        }

        NVPCodec encoder = new NVPCodec();
        encoder["METHOD"] = "DoExpressCheckoutPayment";
        encoder["TOKEN"] = token;
        if (authandcapture)
            encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Authorization"; //-> FOR AUTHORIZATION AND SUCCESIVE CAPTURE METHOD
        else
            encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";//Direct payment

        encoder["PAYERID"] = PayerId;
        encoder["PAYMENTREQUEST_0_AMT"] = finalPaymentAmount;
        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "EUR";

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {

            string authorizationid = "";
            if (authandcapture)
                authorizationid = decoder["PAYMENTINFO_0_TRANSACTIONID"]; //Per utilizzo in procedura di DoCapture ( deve essere ritornato o memorizzato )
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }

    /// <summary>
    /// Funzione di capture da testare
    /// </summary>
    /// <param name="finalPaymentAmount"></param>
    /// <param name="token"></param>
    /// <param name="PayerId"></param>
    /// <param name="authid"></param>
    /// <param name="retMsg"></param>
    /// <returns></returns>
    public bool CapturePayment(string finalPaymentAmount, string token, string PayerId, string authid, ref string retMsg) //ref NVPCodec decoder, 
    {
        //https://developer.paypal.com/docs/classic/express-checkout/ht_ec-singleAuthPayment-curl-etc/


        //https://www.x.com/developers/paypal/documentation-tools/api/doexpresscheckoutpayment-api-operation-nvp
        if (bSandbox)
        {
            pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
        }

        NVPCodec encoder = new NVPCodec();
        encoder["METHOD"] = "DoCapture";
        // encoder["TOKEN"] = token;
        //  encoder["PAYERID"] = PayerId;

        //da testare
        encoder["PAYMENTREQUEST_0_AMT"] = finalPaymentAmount;
        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "EUR";
        encoder["PAYMENTINFO_0_TRANSACTIONID"] = authid;
        encoder["COMPLETETYPE"] = "Complete";


        //da testate
        //encoder["AMT"] = finalPaymentAmount;
        //encoder["CURRENCYCODE"] = "EUR";
        //encoder["AUTHORIZATIONID"] = authid;
        //encoder["COMPLETETYPE"] ="Complete";


        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {

            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }


    /// <summary>
    /// HttpCall: The main method that is used for all API calls
    /// </summary>
    /// <param name="NvpRequest"></param>
    /// <returns></returns>
    public string HttpCall(string NvpRequest) //CallNvpServer
    {
        string url = pendpointurl;

        //To Add the credentials from the profile
        string strPost = NvpRequest + "&" + buildCredentialsNVPString();
        strPost = strPost + "&BUTTONSOURCE=" + HttpUtility.UrlEncode(BNCode);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //DA abilitare per server con .net framework 4.5.1 ----IMPORTANTE!!!!

        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
        objRequest.Timeout = Timeout;
        objRequest.Method = "POST";
        //objRequest.Method = WebRequestMethods.Http.Post;
        objRequest.ContentLength = strPost.Length;

        try
        {
            using (StreamWriter myWriter = new StreamWriter(objRequest.GetRequestStream()))
            {
                myWriter.Write(strPost);
            }
        }
        catch (Exception e)
        {
            string errore = e.Message;
            /*
            if (log.IsFatalEnabled)
            {
                log.Fatal(e.Message, this);
            }*/
        }

        //Retrieve the Response returned from the NVP API call to PayPal
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        string result;
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }

        //Logging the response of the transaction
        /* if (log.IsInfoEnabled)
         {
             log.Info("Result :" +
                       " Elapsed Time : " + (DateTime.Now - startDate).Milliseconds + " ms" +
                      result);
         }
         */
        return result;
    }

    /// <summary>
    /// Credentials added to the NVP string
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    private string buildCredentialsNVPString()
    {
        NVPCodec codec = new NVPCodec();

        if (!IsEmpty(APIUsername))
            codec["USER"] = APIUsername;

        if (!IsEmpty(APIPassword))
            codec[PWD] = APIPassword;

        if (!IsEmpty(APISignature))
            codec[SIGNATURE] = APISignature;

        if (!IsEmpty(Subject))
            codec["SUBJECT"] = Subject;

        codec["VERSION"] = "98";//"2.3";
        //codec["VERSION"] = "2.3"; 
        //codec["VERSION"] = "124";

        return codec.Encode();
    }

    /// <summary>
    /// Returns if a string is empty or null
    /// </summary>
    /// <param name="s">the string</param>
    /// <returns>true if the string is not null and is not empty or just whitespace</returns>
    public static bool IsEmpty(string s)
    {
        return s == null || s.Trim() == string.Empty;
    }
}


public sealed class NVPCodec : NameValueCollection
{
    private const string AMPERSAND = "&";
    private const string EQUALS = "=";
    private static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
    private static readonly char[] EQUALS_CHAR_ARRAY = EQUALS.ToCharArray();

    /// <summary>
    /// Returns the built NVP string of all name/value pairs in the Hashtable
    /// </summary>
    /// <returns></returns>
    public string Encode()
    {
        StringBuilder sb = new StringBuilder();
        bool firstPair = true;
        foreach (string kv in AllKeys)
        {
            string name = HttpUtility.UrlEncode(kv);
            string value = HttpUtility.UrlEncode(this[kv]);
            if (!firstPair)
            {
                sb.Append(AMPERSAND);
            }
            sb.Append(name).Append(EQUALS).Append(value);
            firstPair = false;
        }
        return sb.ToString();
    }

    /// <summary>
    /// Decoding the string
    /// </summary>
    /// <param name="nvpstring"></param>
    public void Decode(string nvpstring)
    {
        Clear();
        foreach (string nvp in nvpstring.Split(AMPERSAND_CHAR_ARRAY))
        {
            string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
            if (tokens.Length >= 2)
            {
                string name = HttpUtility.UrlDecode(tokens[0]);
                string value = HttpUtility.UrlDecode(tokens[1]);
                Add(name, value);
            }
        }
    }


    #region Array methods
    public void Add(string name, string value, int index)
    {
        this.Add(GetArrayName(index, name), value);
    }

    public void Remove(string arrayName, int index)
    {
        this.Remove(GetArrayName(index, arrayName));
    }

    /// <summary>
    /// 
    /// </summary>
    public string this[string name, int index]
    {
        get
        {
            return this[GetArrayName(index, name)];
        }
        set
        {
            this[GetArrayName(index, name)] = value;
        }
    }

    private static string GetArrayName(int index, string name)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException("index", "index can not be negative : " + index);
        }
        return name + index;
    }
    #endregion
}



namespace WelcomeLibrary.UF.PaypalRestApi
{


    public class json_paypalorder
    {
        /// <summary>
        /// (*) required
        /// </summary>
        public string intent { set; get; }

        /// <summary>
        /// (*) required
        /// </summary>
        public List<json_paypalpurchaseunits> purchase_units { set; get; }

        /// <summary>
        /// object
        /// The payment source definition.
        /// </summary>
        public json_paypalpayment_source payment_source { set; get; }

        //public List<json_paypalorderitem> items { set; get; }

        //    {
        //  "intent": "CAPTURE",
        //  "purchase_units": [
        //    {
        //      "reference_id": "d9f80740-38f0-11e8-b467-0ed5f89f718b",
        //      "amount": {
        //        "currency_code": "USD",
        //        "value": "100.00"
        //      }
        //    }
        //  ],
        //  "payment_source": {
        //    "paypal": {
        //        "experience_context": {
        //            "payment_method_preference": "IMMEDIATE_PAYMENT_REQUIRED",
        //        "brand_name": "EXAMPLE INC",
        //        "locale": "en-US",
        //        "landing_page": "LOGIN",
        //        "shipping_preference": "SET_PROVIDED_ADDRESS",
        //        "user_action": "PAY_NOW",
        //        "return_url": "https://example.com/returnUrl",
        //        "cancel_url": "https://example.com/cancelUrl"
        //        }
        //    }
        //}
        //}

    }

    public class json_paypalpurchaseunits
    {
        /// <summary>
        /// string [ 1 .. 256 ] characters
        /// An array of purchase units. Each purchase unit establishes a contract between a payer and the payee. 
        /// Each purchase unit represents either a full or partial order that the payer intends to purchase from the payee.
        /// </summary>
        public string reference_id { set; get; }
        /// <summary>
        /// string[1..127] characters
        /// The purchase description.The maximum length of the character is dependent on the type of characters used.
        /// /The character length is specified assuming a US ASCII character.Depending on type of character; 
        /// (e.g.accented character, Japanese characters) the number of characters that that can be specified as input might not equal the permissible max length.
        /// </summary>
        public string description { set; get; }
        /// <summary>
        /// The API caller-provided external ID.Used to reconcile client transactions with PayPal transactions.Appears in transaction and settlement reports but is not visible to the payer.
        /// </summary>
        public string custom_id { set; get; }
        /// <summary>
        /// string [ 1 .. 127 ] characters
        /// The API caller-provided external invoice number for this order.Appears in both the payer's transaction history and the emails that the payer receives.
        /// </summary>
        public string invoice_id { set; get; }
        /// <summary>
        /// string [ 1 .. 22 ] characters
        /// The soft descriptor is the dynamic text used to construct the statement descriptor that appears on a payer's card statement.
        ///If an Order is paid using the "PayPal Wallet", the statement descriptor will appear in following format on the payer's card statement: PAYPAL_prefix+(space)+merchant_descriptor+(space)+ soft_descriptor
        /// </summary>
        public string soft_descriptor { set; get; }

        /// <summary>
        /// Array of objects
        /// An array of items that the customer purchases from the merchant.
        /// </summary>
        public List<json_paypalorderitem> items { set; get; }

        /// <summary>
        /// (*) REQUIRED
        /// </summary>
        public json_paypalamount amount { set; get; }

        /// <summary>
        /// object
        //The merchant who receives payment for this transaction.
        /// </summary>
        public json_paypalpayee payee { set; get; }
        public json_paypalshipping shipping { set; get; }

    }

    /// <summary>
    /// object
    /// The name and address of the person to whom to ship the items.
    /// </summary>
    public class json_paypalshipping
    {
        /// <summary>
        /// string [ 1 .. 255 ] characters
        /// A classification for the method of purchase fulfillment(e.g shipping, in-store pickup, etc). Either type or options may be present, but not both.
        /// Enum:	 
        /// SHIPPING The payer intends to receive the items at a specified address.
        /// PICKUP_IN_PERSON DEPRECATED.Please use "PICKUP_FROM_PERSON" instead.
        /// PICKUP_IN_STORE The payer intends to pick up the item(s) from the payee's physical store. Also termed as BOPIS, "Buy Online, Pick-up in Store". Seller protection is provided with this option.
        /// PICKUP_FROM_PERSON The payer intends to pick up the item(s) from the payee in person.Also termed as BOPIP, "Buy Online, Pick-up in Person". Seller protection is not available, since the payer is receiving the item from the payee in person, and can validate the item prior to payment.
        /// </summary>
        public string type { set; get; }

        /// <summary>
        /// 	
        //Array of objects[0..10] items
        //An array of shipping options that the payee or merchant offers to the payer to ship or pick up their items.
        /// </summary>
        public List<json_paypalshippingoption> options { set; get; }

        /// <summary>
        /// object
        /// The name of the person to whom to ship the items.Supports only the full_name property.
        /// </summary>
        public json_paypalnome name { set; get; }

        /// <summary>
        /// object
        /// The address of the person to whom to ship the items.Supports only the address_line_1, address_line_2, admin_area_1, admin_area_2, postal_code, and country_code properties.
        /// </summary>
        public json_paypaladdress address { set; get; }
    }

    public class json_paypaladdress
    {
        /// <summary>
        /// The first line of the address, such as number and street, for example, 173 Drury Lane.Needed for data entry, and Compliance and Risk checks.This field needs to pass the full address.
        /// </summary>
        public string address_line_1 { set; get; }
        /// <summary>
        /// string <= 300 characters
        /// The second line of the address, for example, a suite or apartment number.
        /// </summary>
        public string address_line_2 { set; get; }
        /// <summary>
        /// string <= 120 characters
        /// A city, town, or village.Smaller than admin_area_level_1.
        /// </summary>
        public string admin_area_2 { set; get; }
        /// <summary>
        /// string <= 300 characters
        /// The highest-level sub-division in a country, which is usually a province, state, or ISO-3166-2 subdivision.This data is formatted for postal delivery, for example, CA and not California.Value, by country, is:
        /// UK.A county.
        ///US.A state.
        ///Canada.A province.
        ///Japan.A prefecture.
        ///Switzerland.A kanton.
        /// </summary>
        public string admin_area_1 { set; get; }
        /// <summary>
        /// string <= 60 characters
        /// The postal code, which is the ZIP code or equivalent.Typically required for countries with a postal code or an equivalent.See postal code.
        /// </summary>
        public string postal_code { set; get; }
        /// <summary>
        /// string = 2 characters ^([A-Z]{2}|C2)$
        /// The 2-character ISO 3166-1 code that identifies the country or region.
        /// </summary>
        public string country_code { set; get; }

    }

    /// <summary>
    /// object
    /// The name of the person to whom to ship the items.Supports only the full_name property.
    /// </summary>
    public class json_paypalnome
    {
        /// <summary>
        /// string <= 140 characters
        /// When the party is a person, the party's given, or first, name.
        /// </summary>
        public string given_name { set; get; }
        /// <summary>
        ///   string <= 140 characters
        /// When the party is a person, the party's surname or family name. Also known as the last name. 
        /// Required when the party is a person. Use also to store multiple surnames including the matronymic, or mother's, surname.
        /// </summary>
        public string surname { set; get; }

    }

    public class json_paypalshippingoption
    {
        /// <summary>
        /// (*) REQUIRED
        /// string <= 127 characters
        /// A unique ID that identifies a payer-selected shipping option.
        /// </summary>
        public string id { set; get; }

        /// <summary>
        /// (*) REQUIRED
        ///A description that the payer sees, which helps them choose an appropriate shipping option.
        ///For example, Free Shipping, USPS Priority Shipping, Expédition prioritaire USPS, or USPS yōuxiān fā huò.Localize this description to the payer's locale.
        /// </summary>
        public string label { set; get; }

        /// <summary>
        /// (*) REQUIRED
        ///If the API request sets selected = true, it represents the shipping option that the payee or merchant expects to be pre-selected for the payer 
        ///when they first view the shipping.options in the PayPal Checkout experience. 
        ///As part of the response if a shipping.option contains selected= true, it represents the shipping option that the payer selected during the course of checkout with PayPal.
        ///Only one shipping.option can be set to selected = true.
        /// </summary>
        public bool selected { set; get; }

        /// <summary>
        /// string
        /// A classification for the method of purchase fulfillment.
        /// Enum:	Description
        /// SHIPPING The payer intends to receive the items at a specified address.
        /// PICKUP DEPRECATED.To ensure that seller protection is correctly assigned, please use 'PICKUP_IN_STORE' or 'PICKUP_FROM_PERSON' instead.Currently, this field indicates that the payer intends to pick up the items at a specified address (ie.a store address).
        /// PICKUP_IN_STORE The payer intends to pick up the item(s) from the payee's physical store. Also termed as BOPIS, "Buy Online, Pick-up in Store". Seller protection is provided with this option.
        /// PICKUP_FROM_PERSON The payer intends to pick up the item(s) from the payee in person.Also termed as BOPIP, "Buy Online, Pick-up in Person". Seller protection is not available, since the payer is receiving the item from the payee in person, and can validate the item prior to payment.
        /// </summary>
        public string type { set; get; }
    }

    /// <summary>
    /// object
    //The merchant who receives payment for this transaction.
    /// </summary>
    public class json_paypalpayee
    {
        /// <summary>
        /// string[3..254] characters(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-...Show pattern
        /// The email address of merchant.
        /// </summary>
        public string email_address { set; get; }
        /// <summary>
        /// string = 13 characters ^[2-9A-HJ-NP-Z]{13}$
        /// The encrypted PayPal account ID of the merchant.
        /// </summary>
        public string merchant_id { set; get; }


    }


    /// <summary>
    /// object
    ///The total order amount with an optional breakdown that provides details, such as the total item amount, total tax amount, shipping, handling, insurance, and discounts, if any.
    ///If you specify amount.breakdown, the amount equals item_total plus tax_total plus shipping plus handling plus insurance minus shipping_discount minus discount.
    ///The amount must be a positive number. The amount.value field supports up to 15 digits preceding the decimal.
    ///For a list of supported currencies, decimal precision, and maximum charge amount, see the PayPal REST APIs Currency Codes.
    /// </summary>
    public class json_paypalamount
    {
        /// <summary>
        /// (*) REQUIRED
        /// string = 3 characters
        ///The three-character ISO-4217 currency code that identifies the currency.
        /// </summary>
        public string currency_code { set; get; }
        /// <summary>
        /// (*) REQUIRED
        /// string <= 32 characters
        ///The value, which might be:
        /// An integer for currencies like JPY that are not typically fractional.
        ///  A decimal fraction for currencies like TND that are subdivided into thousandths.
        /// For the required number of decimal places for a currency code, see Currency Codes.
        /// </summary>
        public string value { set; get; }

        public json_paypalbreakdown breakdown { set; get; }

    }

    /// <summary>
    /// object
    //The breakdown of the amount. Breakdown provides details such as total item amount, total tax amount, shipping, handling, insurance, and discounts, if any.
    /// </summary>
    public class json_paypalbreakdown
    {
        /// <summary>
        /// object
        ///The subtotal for all items.
        ///Required if the request includes purchase_units[].items[].unit_amount
        ///Must equal the sum of (items[].unit_amount*items[].quantity) for all
        ///items.item_total
        ///value can not be a negative number.
        /// </summary>
        public json_paypalitem_total item_total { set; get; }
        /// <summary>
        /// object
        ///The shipping fee for all items within a given purchase_unit.shipping.value can not be a negative number.
        /// </summary>
        public json_paypalshippingb shipping { set; get; }
        /// <summary>
        ///   object
        ///The handling fee for all items within a given purchase_unit.handling.value can not be a negative number.
        /// </summary>
        public json_paypalhandling handling { set; get; }
        /// <summary>
        ///  object
        /// The total tax for all items.
        /// Required if the request includes purchase_units.items.tax.
        /// Must equal the sum of (items[].tax* items[].quantity) for all items.tax_total.value can not be a negative number.
        /// </summary>
        public json_paypaltax_total tax_total { set; get; }
        /// <summary>
        /// object
        ///The shipping discount for all items within a given purchase_unit.shipping_discount.value can not be a negative number.
        /// </summary>
        public json_paypalinsurance insurance { set; get; }
        /// <summary>
        ///   object
        /// The shipping discount for all items within a given purchase_unit.shipping_discount.value can not be a negative number.
        /// </summary>
        public json_paypalshipping_discount shipping_discount { set; get; }
        /// <summary>
        /// 	   object
        ///The discount for all items within a given purchase_unit.discount.value can not be a negative number.
        /// </summary>
        public json_paypaldiscount discount { set; get; }
    }

    /// <summary>
    /// 	
    //object
    //The total tax for all items. Required if the request includes purchase_units.items.tax.
    //Must equal the sum of (items[].tax * items[].quantity) for all items. tax_total.value can not be a negative number.
    /// </summary>
    public class json_paypaltax_total
    {
        public string currency_code { set; get; }
        public string value { set; get; }
    }
    /// <summary>
    /// object
    //The discount for all items within a given purchase_unit. discount.value can not be a negative number.
    /// </summary>
    public class json_paypaldiscount
    {
        public string currency_code { set; get; }
        public string value { set; get; }
    }

    /// <summary>
    /// object
    /// The insurance fee for all items within a given purchase_unit. insurance.value can not be a negative number.
    /// </summary>
    public class json_paypalinsurance
    {
        public string currency_code { set; get; }
        public string value { set; get; }
    }

    /// <summary>
    /// object
    //The shipping discount for all items within a given purchase_unit. shipping_discount.value can not be a negative number.
    /// </summary>
    public class json_paypalshipping_discount
    {
        public string currency_code { set; get; }
        public string value { set; get; }
    }
    /// <summary>
    /// object
    /// The handling fee for all items within a given purchase_unit. handling.value can not be a negative number.
    /// </summary>
    public class json_paypalhandling
    {
        public string currency_code { set; get; }
        public string value { set; get; }
    }

    /// <summary>
    /// object
    //The shipping fee for all items within a given purchase_unit. shipping.value can not be a negative number.
    /// </summary>
    public class json_paypalshippingb
    {
        public string currency_code { set; get; }
        public string value { set; get; }
    }



    /// <summary>
    /// object
    ///The subtotal for all items. Required if the request includes purchase_units[].items[].unit_amount.
    ///Must equal the sum of (items[].unit_amount * items[].quantity) for all items. item_total.value can not be a negative number.
    /// </summary>
    public class json_paypalitem_total
    {
        public string currency_code { set; get; }
        public string value { set; get; }

    }


    /// <summary>
    /// object
    /// The payment source definition.
    /// </summary>
    public class json_paypalpayment_source
    {
        public json_paypal paypal { set; get; }
    }

    /// <summary>
    /// object
    /// Indicates that PayPal Wallet is the payment source. Main use of this selection is to provide additional instructions associated with this choice like vaulting.
    /// </summary>
    public class json_paypal
    {
        public json_experience_context experience_context { set; get; }
        public string vault_id { set; get; }
        public string email_address { set; get; }


    }
    public class json_experience_context
    {
        public string payment_method_preference { set; get; }
        public string brand_name { set; get; }
        public string locale { set; get; }
        /// <summary>
        /// string [ 1 .. 13 ] characters
        ///Default: "NO_PREFERENCE"
        ///The type of landing page to show on the PayPal site for customer checkout.
        ///Enum:	Description
        ///LOGIN When the customer clicks PayPal Checkout, the customer is redirected to a page to log in to PayPal and approve the payment.
        ///GUEST_CHECKOUT When the customer clicks PayPal Checkout, the customer is redirected to a page to enter credit or debit card and other relevant billing information required to complete the purchase. This option has previously been also called as 'BILLING'
        ///NO_PREFERENCE When the customer clicks PayPal Checkout, the customer is redirected to either a page to log in to PayPal and approve the payment or to a page to enter credit or debit card and other relevant billing information required to complete the purchase, depending on their previous interaction with PayPal.
        /// </summary>
        public string landing_page { set; get; }
        public string shipping_preference { set; get; }
        public string user_action { set; get; }
        public string return_url { set; get; }
        public string cancel_url { set; get; }
    }


    public class json_paypalorderitem
    {
        /// <summary>
        /// (*) REQUIRED
        /// string [ 1 .. 127 ] characters
        /// The item name or title.
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// (*) REQUIRED
        /// string <= 10 characters
        //The item quantity.Must be a whole number.
        /// </summary>
        public string quantity { set; get; }

        /// <summary>
        /// string <= 127 characters
        //The detailed item description.
        /// </summary>
        public string description { set; get; }

        /// <summary>
        /// string <= 127 characters
        //The stock keeping unit(SKU) for the item.
        /// </summary>
        public string sku { set; get; }

        /// <summary>
        /// string [ 1 .. 2048 ] characters
        /// The URL to the item being purchased.Visible to buyer and used in buyer experiences.
        /// </summary>
        public string url { set; get; }

        /// <summary>
        /// string [ 1 .. 20 ] characters
        /// The item category type.
        /// Enum: ["DIGITAL_GOODS", "PHYSICAL_GOODS","DONATION"]
        /// </summary>
        public string category { set; get; }

        /// <summary>
        /// string [ 1 .. 2048 ] characters ^(https:)([/|.|\w|\s|-])*\.(?:jpg|gif|png|jpe...Show pattern
        /// The URL of the item's image. File type and size restrictions apply. An image that violates these restrictions will not be honored.
        /// </summary>
        public string image_url { set; get; }

        /// <summary>
        /// (*)REQUIRED
        //The item price or rate per unit. If you specify unit_amount, purchase_units[].amount.breakdown.item_total is required.
        ///Must equal unit_amount* quantity for all items.unit_amount.value can not be a negative number.
        /// </summary>
        public json_paypalunit_amout unit_amount { set; get; }
        /// <summary>
        /// tax for the item. If tax is specified, purchase_units[].amount.breakdown.tax_total is required. Must equal tax * quantity for all items. tax.value can not be a negative number.
        /// </summary>
        public json_paypaltax tax { set; get; }
        /// <summary>
        ///  Universal Product Code of the item.
        /// </summary>
        public json_paypalupc upc { set; get; }
    }

    /// <summary>
    /// object
    ///The item price or rate per unit. 
    ///If you specify unit_amount, purchase_units[].amount.breakdown.item_total is required. 
    ///Must equal unit_amount * quantity for all items. unit_amount.value can not be a negative number.
    /// </summary>
    public class json_paypalunit_amout
    {
        /// <summary>
        /// (*) REQUIRED
        /// string = 3 characters
        /// The three-character ISO-4217 currency code that identifies the currency.
        /// </summary>
        public string currency_code { set; get; }
        /// <summary>
        /// (*) REQUIRED
        ///  string <= 32 characters
        ///  The value, which might be:
        ///  An integer for currencies like JPY that are not typically fractional.
        ///  A decimal fraction for currencies like TND that are subdivided into thousandths.
        /// For the required number of decimal places for a currency code, see Currency Codes.
        /// </summary>
        public string value { set; get; }

    }

    /// <summary>
    /// object
    ///The item tax for each unit. If tax is specified, purchase_units[].amount.breakdown.tax_total is required. Must equal tax * quantity for all items. tax.value can not be a negative number.
    /// </summary>
    public class json_paypaltax
    {
        /// <summary>
        /// (*) REQUIRED
        /// string = 3 characters
        //The three-character ISO-4217 currency code that identifies the currency.
        /// </summary>
        public string currency_code { set; get; }

        /// <summary>
        /// (*) REQUIRED
        /// string <= 32 characters
        /// The value, which might be:
        /// An integer for currencies like JPY that are not typically fractional.
        /// A decimal fraction for currencies like TND that are subdivided into thousandths.
        /// For the required number of decimal places for a currency code, see Currency Codes.
        /// </summary>
        public string value { set; get; }

    }

    /// <summary>
    /// object
    /// The Universal Product Code of the item.
    /// </summary>
    public class json_paypalupc
    {
        /// <summary>
        /// (*) REQUIRED
        /// string [ 1 .. 5 ] characters
        ///The Universal Product Code type.
        /// Enum: ["UPC-A", "UPC-B","UPC-C","UPC-D","UPC-E","UPC-2","UPC-5"]
        /// </summary>
        public string type { set; get; }
        /// <summary>
        /// (*) REQUIRED
        /// string [ 6 .. 17 ] characters
        //The UPC product code of the item.
        /// </summary>
        public string code { set; get; }

    }
}