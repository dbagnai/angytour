
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


public class jreturnnlcontainer
{
    public Mail mail { set; get; }
    public Dictionary<string, string> objfiltro { set; get; }
}
public class HandlerNewsletter : IHttpHandler, IRequiresSessionState
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

        string smail = pars.ContainsKey("mail") ? pars["mail"] : "";
        Mail mail = Newtonsoft.Json.JsonConvert.DeserializeObject<Mail>(smail);
        try
        {

            switch (q)
            {
                case "inseriscimailrichiestafeedback":
                    result = preparamail(mail, lingua);
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

    /// <summary>
    /// mail.Sparedict["linkfeedback"]  //WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/" + lingua + "/casadellabatteria/inserisci-feedback-12"; //Passare nei parametri di chiamata
    /// mail.Sparedict["idnewsletter"]   //Settare l'id schema della newsletter per richiesta feedback
    /// mail.Id_card  id del post a cui fà riferimento la richiesta di feed back 
    /// mail.Sparedict["deltagiorniperinvio"] //parametro per  determinare dopo quanti giorni inviare le email di richiesta feedback  
    /// mail.Sparedict["idclienti"]  //clienti per i quali preparare l'invio  come lista di id seprati da ,
    /// </summary>
    /// <param name="mail"></param>
    /// <param name="lingua"></param>
    /// <returns></returns>
    public static string preparamail(Mail mail, string lingua)
    {
        mailingDM mDM = new mailingDM();
        ClienteCollection clientimail = new ClienteCollection();
        string message = "";
        int totalemailpreparate = 0;
        message = "Totale mail preparate: " + "0";

        //id del post/articolo viene passato dal chiamante della mail da inviare  -> lo metto in id_card per riusarlo quando viene inviata la mail!!
        //   mail.Id_card <= idpost; //

        string sidnewsletter = references.ResMan("basetext", lingua, "feedbackdefaultnewsletter");
        int idnewsletter = 0;
        int.TryParse(sidnewsletter, out idnewsletter);
        if (mail.Sparedict.ContainsKey("idnewsletter") && !string.IsNullOrEmpty(mail.Sparedict["idnewsletter"])) { int.TryParse(mail.Sparedict["idnewsletter"], out idnewsletter); }
        mail.Sparedict["idnewsletter"] = idnewsletter.ToString();
        mail.Id_mailing_struttura = idnewsletter;//L'id della newsletter originale passata è quello di riferimento per l'inserimento della mail

        string linkfeedbackform = references.ResMan("basetext", lingua, "feedbacksdefaultform");
        if (mail.Sparedict.ContainsKey("linkfeedback") && !string.IsNullOrEmpty(mail.Sparedict["linkfeedback"])) { linkfeedbackform = mail.Sparedict["linkfeedback"]; }
        if (!linkfeedbackform.ToLower().StartsWith("http")) linkfeedbackform = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + linkfeedbackform;
        mail.Sparedict["linkfeedback"] = linkfeedbackform; //viene passato alla funzione di inserimento ma non salvato in struttura

        string sdeltagiorniperinvio = references.ResMan("basetext", lingua, "feedbacksdefaultdeltagg");
        double deltagiorniperinvio = 20;
        double.TryParse(sdeltagiorniperinvio, out deltagiorniperinvio);
        if (mail.Sparedict.ContainsKey("deltagiorniperinvio") && !string.IsNullOrEmpty(mail.Sparedict["deltagiorniperinvio"])) { double.TryParse(mail.Sparedict["deltagiorniperinvio"], out deltagiorniperinvio); }
        mail.Sparedict["deltagiorniperinvio"] = deltagiorniperinvio.ToString();

        string serializedsparedict = Newtonsoft.Json.JsonConvert.SerializeObject(mail.Sparedict, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
        });

        Mail newsletterschema = mDM.CaricaNewsletterPerId(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, idnewsletter);
        if (newsletterschema != null)
        {
            //PREPRARIAMO LE INDICAZIONE PER INSERIRE LE EMAIL NELLO SCHEDULER DI INVIO
            mail.Id = 0;//Inserisco sempre una nuova riga di mail ad ogni salvataggio ( attenzione potrebbe mandare mail multiple allo stesso cliente )!!!
            mail.TestoMail = newsletterschema.TestoMail;
            mail.SoggettoMail = newsletterschema.SoggettoMail;
            mail.Lingua = lingua;
            mail.Tipomailing = newsletterschema.Tipomailing;
            mail.NoteInvio = newsletterschema.NoteInvio;
            mail.NoteInvio += "||{" + serializedsparedict + "}||"; //Appoggio nelle note di invio eventuali parametri utili nella prodedura di invio delle mail

            mail.DataInserimento = System.DateTime.Now.AddDays(deltagiorniperinvio);

            if (mail.Sparedict.ContainsKey("idclienti") && !string.IsNullOrEmpty(mail.Sparedict["idclienti"]))
            {
                string[] listaarray = mail.Sparedict["idclienti"].Split(',');
                if (listaarray != null && listaarray.Length > 0)
                {
                    Cliente c = new Cliente();
                    foreach (string idcliente in listaarray)
                    {
                        if (!string.IsNullOrEmpty(idcliente.Trim()))
                        {
                            c = new Cliente();
                            int sid = 0;
                            if (int.TryParse(idcliente.Trim(), out sid))
                            {
                                c.Id_cliente = sid;
                                clientimail.Add(c);
                            }
                        }
                    }
                }

                #region VARIAZIONE DEL MITTENTE DEL MAILING ATTUALE //NON USATA IN QUESTO CASO
                //Variazione del mittente standard della mail
                //if (!string.IsNullOrWhiteSpace(txtEmailMittente.Text))
                //{
                //    string emailmittente = txtEmailMittente.Text;
                //    string nomemittente = txtNomeMittente.Text;
                //    if (emailmittente.Contains("@"))
                //    {
                //        emailmittente = emailmittente.Trim().Trim('\t').Trim('\\').Trim('\r').Trim('\n');
                //        string testodaaggiungere = "|" + emailmittente + "|";
                //        if (!string.IsNullOrWhiteSpace(nomemittente))
                //            testodaaggiungere += nomemittente + "|";
                //        else
                //            testodaaggiungere += "|";
                //        mail.NoteInvio = mail.NoteInvio.Insert(0, testodaaggiungere);//Aggiungo in testo la mail e il nome del mittente alle note di invio
                //    }
                //}
                #endregion

                mDM.InserisciBloccoMailPerClienti(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, mail, clientimail); //Inseriamo nel db la mail pronta per l'invio al cliente specifico
                totalemailpreparate = clientimail.Count;
            }
        }
        message = "Totale mail preparate: " + totalemailpreparate.ToString();

        return message;
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}