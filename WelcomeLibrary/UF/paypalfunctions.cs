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


        double totaleforcheck = 0; //totale precalcolato da richiedere
        double totalamount = 0;
        int i = 0;
        foreach (string item in paypaldatas.Keys)//scorro gli elementi a carrello
        {
            //paypaldatas contiene indicizzato per ogni id i seguenti in sequenza
            List<string> dettaglio = paypaldatas[item];
            //testo1
            string testo1 = dettaglio[0]; //id prodotto o vuoto
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