using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Web.Management;
using DocumentFormat.OpenXml.Wordprocessing;

public class CustomerController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<controller>
    //public void Post([FromBody] string value)
    //{
    //   int viao = 0;

    //   int ciao = 2;
    //}

    // PUT api/<controller>/5
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<controller>/5
    public void Delete(int id)
    {
    }


    [HttpPost]
    public void UpdateCustomer([FromBody] string value)
    {
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        System.Diagnostics.Debug.WriteLine("Inizio");
        Messaggi.Add("Messaggio", "");
        Messaggi["Messaggio"] = "Start call app " + System.DateTime.Now.ToString() + " \r\n";
        WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "logdebug.txt");

        for (int i = 0; i < 1000000000; i++)
        {
            int ciao = 2;
        }
        //System.Threading.Thread.Sleep(10000);
        System.Diagnostics.Debug.WriteLine("Terminato");
    }

    [HttpPost]
    public string UpdateCustomerValue([FromBody] string value)
    {
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        System.Diagnostics.Debug.WriteLine("Inizio");
        Messaggi.Add("Messaggio", "");
        Messaggi["Messaggio"] = "Start call app " + System.DateTime.Now.ToString() + " \r\n";
        WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "logdebug.txt");

        for (int i = 0; i < 1000000000; i++)
        {
            int ciao = 2;
        }
        //System.Threading.Thread.Sleep(10000);
        System.Diagnostics.Debug.WriteLine("Terminato");
        return "Finito funct";
    }

    [HttpPost]
    public async Task<string>  UpdateCustomerAsync([FromBody] string value)
    {
        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        System.Diagnostics.Debug.WriteLine("Inizio");
        try
        {
            Messaggi.Add("Messaggio", "");
            Messaggi["Messaggio"] = "Start call app " + System.DateTime.Now.ToString() + " \r\n";
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "logdebug.txt");


            //sz = await Request.Content.ReadAsAsync<String>();
            //var KK = await Request.Content.ReadAsFormDataAsync();
            await Task.Delay(6000);
        }
        catch (Exception ex)
        {
            //Utility.Logging.Error("ppp", ex);
        }
        for (int i = 0; i < 1000000000; i++)
        {
            int ciao = 2;
        }
        //System.Threading.Thread.Sleep(10000);
        System.Diagnostics.Debug.WriteLine("Terminato");
        return "ok finito async";
    }

    //public async  Task<string>  Altro([FromBody] string value)
    //public async Task<string> Altro(string value)
    [HttpPost]
    public async Task<string> Altro(FormDataCollection form)
    {

        System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
        string sz = "";
        try
        {
            //Messaggi.Add("Messaggio", "");
            //Messaggi["Messaggio"] = "Start call app " + System.DateTime.Now.ToString() + " \r\n";
            //WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "logdebug.txt");


            System.Diagnostics.Debug.WriteLine("Inizio Altro");

            if (this.ActionContext.RequestContext.Principal.Identity != null)
            {
                //Messaggi["Messaggio"] = "Start call app step 2" + System.DateTime.Now.ToString() + " \r\n";
                //WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "logdebug.txt");

                //string sz = Request.Content.ToString();
                //string sz = GetFromBodyString(Request);
                // sz = await Request.Content.ReadAsStringAsync();
                try
                {
                    //sz = await Request.Content.ReadAsAsync<String>();
                    //var KK = await Request.Content.ReadAsFormDataAsync();
                    await Task.Delay(1); // qui metto in attesa ( un minimo per avere la funzionalita async )
                }
                catch (Exception ex)
                {
                    //Utility.Logging.Error("ppp", ex);
                }
                sz = "prova";
                //using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Content.ToString() , Encoding.UTF8))
                //{
                //   sz = reader.ReadToEnd();
                //}
                //Messaggi["Messaggio"] = "Start call app step 3" + System.DateTime.Now.ToString() + " \r\n";
                //WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "logdebug.txt");

                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Debug.WriteLine("Terminato Altro");
                System.Diagnostics.Debug.WriteLine(sz);
            }
            int[] j = new int[5];
            j[1] = 100;
            j[2] = 200;
            j[3] = 300;
            j[4] = 400;
           
            sz = Newtonsoft.Json.JsonConvert.SerializeObject(j);
            //Messaggi["Messaggio"] = "Start call app step 4" + System.DateTime.Now.ToString() + " \r\n";
            //WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "logdebug.txt");

        }
        catch (Exception ex)
        {
            Messaggi["Messaggio"] = "Error api controller: " + System.DateTime.Now.ToString() + " \r\n";
            Messaggi["Messaggio"] += ex.Message + " \r\n";
            //WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune, "logdebug.txt");
            WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune);
        }
        return sz;
    }


    ///*Returns a string representing the content of the body 
    //of the HTTP-request.*/
    //private  Task<string> GetFromBodyString(HttpRequestMessage request)
    //{
    //   string result = string.Empty;

    //   if (request == null || request.InputStream == null)
    //      return result;

    //   request.InputStream.Position = 0;

    //   /*create a new thread in the memory to save the original 
    //   source form as may be required to read many of the 
    //   body of the current HTTP- request*/
    //   using (MemoryStream memoryStream = new MemoryStream())
    //   {
    //      //request.InputStream.CopyToMemoryStream(memoryStream);
    //      CopyToMemoryStream(request.Content. InputStream, memoryStream);
    //      using (StreamReader streamReader = new StreamReader(memoryStream))
    //      {
    //         result = streamReader.ReadToEnd();
    //      }
    //   }
    //   return result;
    //}

    /*Copies bytes from the given stream MemoryStream and writes 
    them to another stream.*/
    private void CopyToMemoryStream(Stream source, MemoryStream destination)
    {
        if (source.CanSeek)
        {
            int pos = (int)destination.Position;
            int length = (int)(source.Length - source.Position) + pos;
            destination.SetLength(length);

            while (pos < length)
                pos += source.Read(destination.GetBuffer(), pos, length - pos);
        }
        else
            source.CopyTo((Stream)destination);
    }

}
