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


public class jreturncontainerfdb
{
    public Mail mail { set; get; }
    public Comment item { set; get; }
    public List<Comment> list { set; get; }
    public double totalemediastars { set; get; }
    public long totaleapprovati { set; get; }
    public long recordstotali { set; get; }
    public List<simpleidname> reslist { set; get; }
    public Dictionary<string, string> objfiltro { set; get; }
}




public class feedbackHandler : IHttpHandler, IRequiresSessionState
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
        try
        {
            commentsDM comDM = new commentsDM();
            Dictionary<string, string> pars = parseparams(context);
            string lingua = pars.ContainsKey("lng") ? pars["lng"] : "I";
            string q = pars.ContainsKey("q") ? pars["q"] : "";
            string objfiltro = pars.ContainsKey("objfiltro") ? pars["objfiltro"] : "";
            string sitem = pars.ContainsKey("item") ? pars["item"] : "";
            Dictionary<string, string> filtri = new Dictionary<string, string>();
            if (objfiltro != "" && objfiltro != null)
                filtri = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
            string idpost = filtri.ContainsKey("id") ? filtri["id"] : "";
            string smail = pars.ContainsKey("mail") ? pars["mail"] : "";
            Mail mail = Newtonsoft.Json.JsonConvert.DeserializeObject<Mail>(smail);

            Comment item = Newtonsoft.Json.JsonConvert.DeserializeObject<Comment>(sitem);
            string spage = filtri.ContainsKey("page") ? filtri["page"] : "1";
            string spagesize = filtri.ContainsKey("pagesize") ? filtri["pagesize"] : "12";
            string enablepager = filtri.ContainsKey("enablepager") ? filtri["enablepager"] : "false";
            long page = 0;
            long pagesize = 0;
            if (enablepager == "true")
            {
                long.TryParse(spage, out page);
                long.TryParse(spagesize, out pagesize);
            }
            string maxrecord = filtri.ContainsKey("maxrecord") ? filtri["maxrecord"] : "";


            switch (q)
            {
                case "caricacommenti":
                    CommentsCollection comments = new CommentsCollection();
                    bool? soloapprovati = true;
                    string logged = filtri.ContainsKey("logged") ? filtri["logged"] : "";
                    if (logged == "true") soloapprovati = null;

                    if (filtri.ContainsKey("approvati"))
                    {
                        bool bapprovati = false;
                        bool.TryParse(filtri["approvati"], out bapprovati);
                        soloapprovati = bapprovati;
                    }
                    comments = comDM.CaricaCommentiFiltratiScript(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idpost, soloapprovati, maxrecord, page, pagesize);

                    comments.ForEach(c => c.Lingua = lingua);//permette di settare la lingua nella lista per usare la variabile testo e titolo già settate per lingua

                    //HandlerDataCommon.getlinklist(lingua,)

                    jreturncontainerfdb jr = new jreturncontainerfdb();
                    //Setto gli indirizzi base per l'invio delle mail
                    jr.mail = new Mail();
                    jr.mail.Emailaddress.Add("defaultsendername", ConfigManagement.ReadKey("Nome"));
                    jr.mail.Emailaddress.Add("defaultsenderemail", ConfigManagement.ReadKey("Email"));
                    jr.mail.Emailaddress.Add("defaultdestname", ConfigManagement.ReadKey("Nome"));
                    jr.mail.Emailaddress.Add("defaultdestemail", ConfigManagement.ReadKey("Email"));

                    jr.item = new Comment();
                    jr.totaleapprovati = comments.Napprovati;
                    jr.totalemediastars = comments.Mediatotalestars;
                    jr.recordstotali = comments.Recordstotali;
                    jr.list = comments;
                    jr.objfiltro = filtri;
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(jr, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "insertcommenti":
                    if (item != null && item.Id == 0)
                    {
                        comDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item);
                        result = item.Id.ToString();
                    }
                    break;
                case "updatecommenti":
                    if (item != null && item.Id != 0)
                    {
                        comDM.InserisciAggiorna(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item);
                    }
                    break;
                case "deletecommenti":
                    if (item != null && item.Id != 0)
                    {
                        comDM.Cancella(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, item.Id);

                    }

                    break;
                case "inviamailfeedback":

                    //Contiene i dati della mail da inviare !!!
                    if (mail != null)
                    {
                        if (mail.Emailaddress["defaultdestemail"] != string.Empty && mail.Emailaddress["defaultsenderemail"] != string.Empty)
                        {
                            //Considera che l'id post potrebbe anche essere 0 !!!
                            mail.SoggettoMail = "Avviso inserimento recensione da approvare sul sito " + ConfigManagement.ReadKey("Nome");
                            mail.TestoMail += "Nuova recensione inserita il " + string.Format("{0:dd-MM-yyyy}", System.DateTime.Now) + " da ";
                            mail.TestoMail += mail.Emailaddress["defaultsendername"] + " - email : " + mail.Emailaddress["defaultsenderemail"] + "<br/>";
                            if (mail.Sparedict.ContainsKey("Idpost"))
                            {
                                string idposttmp = mail.Sparedict["Idpost"];
                                Dictionary<string, string> links = offerteDM.getlinklist(lingua, idposttmp);
                                if (links != null && links.ContainsKey(idposttmp))
                                    mail.TestoMail += "Link scheda recensione: " + links[idposttmp] + "<br/>";
                                else
                                    mail.TestoMail += "Recensione generica non collegata a prodotto <br/>";
                            }


                            if (mail.Sparedict.ContainsKey("Id"))
                            {
                                //Questo è l'id del commento inserito ( per riferimento )
                                mail.TestoMail += "Id messaggio recensione: " + mail.Sparedict["Id"] + "<br/>";
                            }

                            Utility.invioMailGenerico(mail.Emailaddress["defaultsendername"], mail.Emailaddress["defaultsenderemail"], mail.SoggettoMail, mail.TestoMail, mail.Emailaddress["defaultdestemail"], mail.Emailaddress["defaultdestname"]);
                        }
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            string er = ex.Message;
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
