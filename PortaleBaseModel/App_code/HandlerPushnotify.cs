
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using WelcomeLibrary;
using WelcomeLibrary.UF;
using WelcomeLibrary.DAL;
using WelcomeLibrary.DOM;
using System.Web.SessionState;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;


public class jreturnpush
{
    public string iddevice { set; get; }
    public DevicesCollection DevicesList { set; get; }
    public Devices Devices { set; get; }
    public Dictionary<string, string> VapidKeys { set; get; }
    public string payload { set; get; }
    //public Dictionary<string, string> objfiltro { set; get; }
}
public class HandlerPushNotify : IHttpHandler, IRequiresSessionState
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
            string szKey = item.ToString();
            if (!pars.ContainsKey(szKey))
                pars.Add(szKey, context.Request.Params[szKey].ToString());
        }

        return pars;
    }


    public void ProcessRequest(HttpContext context)
    {
        string result = "";
        context.Response.ContentType = "text/plain";

        Dictionary<string, string> pars = parseparams(context);
        string lingua = pars.ContainsKey("lng") ? pars["lng"] : "I";
        string q = pars.ContainsKey("q") ? pars["q"] : "";
        string vapidPublicKey = ConfigManagement.ReadKey("PublicKey");
        string vapidPrivateKey = "********";// ConfigManagement.ReadKey("PrivateKey");
        jreturnpush jr = new jreturnpush();
        string jrpushcontainer = pars.ContainsKey("pushcontainer") ? pars["pushcontainer"] : "";
        if (jrpushcontainer != "")
            jr = Newtonsoft.Json.JsonConvert.DeserializeObject<jreturnpush>(jrpushcontainer);

        Devices devices = null;
        string sdevices = pars.ContainsKey("devices") ? pars["devices"] : "";
        if (sdevices != "")
            devices = Newtonsoft.Json.JsonConvert.DeserializeObject<Devices>(sdevices);
        //string smail = pars.ContainsKey("mail") ? pars["mail"] : "";
        //Mail mail = Newtonsoft.Json.JsonConvert.DeserializeObject<Mail>(smail);
        try
        {

            switch (q)
            {
                case "inizializzapushserver":

                    jr.Devices = new Devices();  //Struttura da usare per il device
                    jr.VapidKeys = new Dictionary<string, string>();
                    jr.VapidKeys.Add("PublicKey", vapidPublicKey);
                    jr.VapidKeys.Add("PrivateKey", vapidPrivateKey);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(jr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "generatekeys":
                    pushDM.GenerateKeys();
                    jr.Devices = new Devices();  //Struttura da usare per il device
                    jr.VapidKeys = new Dictionary<string, string>();
                    jr.VapidKeys.Add("PublicKey", ConfigManagement.ReadKey("PublicKey"));
                    //jr.VapidKeys.Add("PrivateKey", ConfigManagement.ReadKey("PrivateKey"));
                    jr.VapidKeys.Add("PrivateKey", "*********");
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(jr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;

                case "sendnotification":

                    string selectedid = "";

                    if (jr.Devices != null)
                    {
                        selectedid = jr.Devices.Id.ToString();
                    }
                    result = pushDM.SendNotification(jr.payload, selectedid);

                    break;
                case "deviceslist":
                    pushDM.DevicesList = pushDM.CaricaDevicesFiltratiScript(WelcomeLibrary.STATIC.Global.NomeConnessioneDb); //Ricarico i devices
                    jr.Devices = new Devices();  //Struttura da usare per il device
                    jr.DevicesList = pushDM.DevicesList;  //Struttura da usare per il device
                    jr.VapidKeys = new Dictionary<string, string>();
                    jr.VapidKeys.Add("PublicKey", ConfigManagement.ReadKey("PublicKey"));
                    //jr.VapidKeys.Add("PrivateKey", ConfigManagement.ReadKey("PrivateKey"));
                    jr.VapidKeys.Add("PrivateKey", "*********");
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(jr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "deletesubscribebyid":
                    string idtodelete = "";
                    if (jr.Devices != null)
                    {
                        pushDM.CancellaDevices(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, jr.Devices.Id);

                    }
                    break;
                case "subscribedevice":
                    if (jrpushcontainer != "")
                    {
                        pushDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, jr.Devices);
                        result = jr.Devices.Id.ToString();
                    }
                    if (devices != null)
                    {
                        pushDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, devices);
                        result = devices.Id.ToString();
                    }
                    break;
                case "unsubscribedevice":
                    result = ""; //
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            string er = ex.Message;
            if (ex.InnerException != null)
                er += ex.InnerException.Message.ToString();
            result = er;
            context.Response.StatusCode = 400;
        }
        context.Response.Write(result);
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}