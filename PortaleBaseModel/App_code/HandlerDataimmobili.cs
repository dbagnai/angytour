
using System;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Collections.Generic;
using WelcomeLibrary;
using WelcomeLibrary.DOM;
using WelcomeLibrary.UF;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;


public class HandlerDataimmobili : IHttpHandler, IRequiresSessionState
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
            Dictionary<string, string> res = new Dictionary<string, string>();
            Dictionary<string, List<JObject>> resJObject = new Dictionary<string, List<JObject>>();
            Dictionary<string, Dictionary<string, string>> resulcomplete = new Dictionary<string, Dictionary<string, string>>();


            Dictionary<string, string> pars = parseparams(context);
            string q = pars.ContainsKey("q") ? pars["q"] : "";
            string lingua = pars.ContainsKey("lng") ? pars["lng"] : "I";
            string id = pars.ContainsKey("id") ? pars["id"] : "";

            string tipologia = pars.ContainsKey("tipologia") ? pars["tipologia"] : "rif000666";
            string categoria = pars.ContainsKey("categoria") ? pars["categoria"] : "";
            string scompletedata = pars.ContainsKey("completedata") ? pars["completedata"] : "false";
            bool completedata = false;
            bool.TryParse(scompletedata, out completedata);

            string objfiltro = pars.ContainsKey("objfiltro") ? pars["objfiltro"] : "";
            string page = pars.ContainsKey("page") ? pars["page"] : "";
            string pagesize = pars.ContainsKey("pagesize") ? pars["pagesize"] : "";
            string enablepager = pars.ContainsKey("enablepager") ? pars["enablepager"] : "";

            string link = "";
            string testotitolo = "";
            string descrizionebreve = "";
            string descrizione = "";
            string videourl = "";
            switch (q)
            {
                case "initparameters":

                    Dictionary<string, string> retdict = new Dictionary<string, string>();
                    //var JSONrefmetrature = "";
                    //var JSONrefprezzi = "";
                    //var JSONrefcondizione = "";
                    //var JSONreftipocontratto = "";
                    //var JSONreftiporisorse = "";

                    //references.refmetrature
                    //references.refprezzi
                    //references.refcondizione
                    //references.reftipocontratto
                    //references.reftiporisorse
                    retdict.Add("JSONrefmetrature", references.refmetrature);
                    retdict.Add("JSONrefprezzi", references.refprezzi);
                    retdict.Add("JSONrefcondizione", references.refcondizione);
                    retdict.Add("JSONreftipocontratto", references.reftipocontratto);
                    retdict.Add("JSONreftiporisorse", references.reftiporisorse);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(retdict, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });

                    break;
                case "getresource":
                    res = FiltraDatiById(context, id, lingua, tipologia, WelcomeLibrary.STATIC.Global.usecdn);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "filterresources":
                    //Implementare un metodo che carica il file json delle risorse 
                    //Riceve i parametri di filtro  ed effettua le ricerche ritornando
                    // una string json da parserizzare che contiene gli immobili filtrati

                    resJObject = FiltraDatiByParameters(context, objfiltro, page, pagesize, enablepager, lingua, true);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(resJObject, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                    });
                    break;
                case "creasitemapsresources":
                    //Da fare la gnerazione delle sitemap dal file estates.json
                    references.CreaSitemapImmobili(context.Server, tipologia);
                    break;
                //case "linklistdetail":
                //    string dataitem = pars.ContainsKey("dataitem") ? pars["dataitem"] : "";
                //    dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(dataitem);
                //    foreach (dynamic r in jsonResponse)
                //    {
                //        Dictionary<string, string> tmp = new Dictionary<string, string>();
                //        testotitolo = "";
                //        descrizionebreve = "";
                //        id = r.id;
                //        foreach (dynamic c in r.dettagliorisorse_1.Children())
                //        {
                //            if (c.lingua == lingua)
                //            {
                //                testotitolo = c.titolo;
                //                descrizionebreve = c.descrizione;
                //            }
                //        }
                //        if (completedata)
                //        {
                //            foreach (dynamic c in r.dettagliorisorse_2.Children())
                //            {
                //                if (c.lingua == lingua)
                //                {
                //                    videourl = c.titolo;
                //                    descrizione = c.descrizione;
                //                }
                //            }
                //            tmp.Add("video", videourl);
                //            tmp.Add("descrizione", descrizione);
                //        }
                //        link = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(testotitolo), id, tipologia, categoria, "");
                //        //res.Add(id, link);
                //        tmp.Add("titolo", testotitolo);
                //        tmp.Add("descrizionebreve", descrizionebreve);
                //        tmp.Add("link", link);
                //        resulcomplete.Add(id, tmp);
                //    }

                //    result = Newtonsoft.Json.JsonConvert.SerializeObject(resulcomplete, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                //    {
                //        NullValueHandling = NullValueHandling.Ignore,
                //        MissingMemberHandling = MissingMemberHandling.Ignore,
                //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //        PreserveReferencesHandling = PreserveReferencesHandling.None,
                //    });
                //    break;
                default:
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
        ///////////////////////////////////////////////
    }





    protected Dictionary<string, List<JObject>> FiltraDatiByParameters(HttpContext context, string objfiltro, string spage, string spagesize, string senablepager, string lingua, bool orderbyprezzo = false)
    {
        Dictionary<string, List<JObject>> ret = new Dictionary<string, List<JObject>>();
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        bool enablepager = false;
        bool.TryParse(senablepager, out enablepager);

        references references = new references(context.Server);
        if (!string.IsNullOrEmpty(objfiltro))
        {
            parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(objfiltro);
        }
        int page = 0;
        int pagesize = 0;
        int.TryParse(spage, out page);
        int.TryParse(spagesize, out pagesize);

        var smaxelement = parameters.ContainsKey("maxelement") ? parameters["maxelement"] : "";
        int maxelemnts = 0;
        int.TryParse(smaxelement, out maxelemnts);

        var codn = parameters.ContainsKey("ddlNazioneSearch") ? parameters["ddlNazioneSearch"] : "";
        var codr = parameters.ContainsKey("ddlRegioneSearch") ? parameters["ddlRegioneSearch"] : "";
        var codp = parameters.ContainsKey("ddlProvinciaSearch") ? parameters["ddlProvinciaSearch"] : "";
        var codc = parameters.ContainsKey("ddlComuneSearch") ? parameters["ddlComuneSearch"] : "";
        var idcontratto = parameters.ContainsKey("ddlContrattoSearch") ? parameters["ddlContrattoSearch"] : "";
        var idcondizione = parameters.ContainsKey("ddlCondizioneSearch") ? parameters["ddlCondizioneSearch"] : "";
        var idtipologia = parameters.ContainsKey("ddlTipologiaSearch") ? parameters["ddlTipologiaSearch"] : "";
        var idprezzi = parameters.ContainsKey("ddlPrezziSearch") ? parameters["ddlPrezziSearch"] : "";
        var idmetrature = parameters.ContainsKey("ddlMetratureSearch") ? parameters["ddlMetratureSearch"] : "";
        var vetrina = parameters.ContainsKey("vetrina") ? parameters["vetrina"] : "";

        var pmin = 0; var pmax = 0;
        var mmin = 0; var mmax = 0;
        if (!string.IsNullOrEmpty(idprezzi))
        {
            //var filterbase = "{ \"data\":" + references.refprezzi;
            //filterbase += "}";
            dynamic jsonPrezzi = Newtonsoft.Json.JsonConvert.DeserializeObject(references.refprezzi);
            foreach (dynamic r in jsonPrezzi)
            {
                if (r.id == idprezzi)
                {
                    foreach (dynamic c in r.dettagliprezzi.Children())
                    {
                        if (c.lingua == "I")
                        {
                            string selectvalue = c.descrizione;
                            if (selectvalue != null && selectvalue.Length > 0)
                            {
                                var pfascia = selectvalue.ToString().Split(';');
                                if (pfascia != null && pfascia.Length == 2)
                                {
                                    string min = pfascia[0];
                                    string max = pfascia[1];
                                    int.TryParse(min, out pmin);
                                    int.TryParse(max, out pmax);
                                    if (pmin == 0) pmin = 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(idmetrature))
        {
            //var filterbase = "{ \"data\":" + references.refprezzi;
            //filterbase += "}";
            dynamic jsonMetrature = Newtonsoft.Json.JsonConvert.DeserializeObject(references.refmetrature);
            foreach (dynamic r in jsonMetrature)
            {
                if (r.id == idmetrature)
                {
                    foreach (dynamic c in r.dettaglimetrature.Children())
                    {
                        if (c.lingua == "I")
                        {
                            string selectvalue = c.descrizione;

                            if (selectvalue != null && selectvalue.Length > 0)
                            {
                                var pfascia = selectvalue.ToString().Split(';');
                                if (pfascia != null && pfascia.Length == 2)
                                {
                                    string min = pfascia[0];
                                    string max = pfascia[1];
                                    int.TryParse(min, out mmin);
                                    int.TryParse(max, out mmax);
                                    if (mmin == 0) mmin = 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        references.CaricaDatiRisorseDaJson(lingua, "", WelcomeLibrary.STATIC.Global.usecdn);
        dynamic jsonResources = Newtonsoft.Json.JsonConvert.DeserializeObject(references.resourcesloaded);
        //jsonResources.ResultList.Sort((x, y) => x.Prezzo1.CompareTo(y.Prezzo1));
        //jsonResources.ResultList.OrderBy<dynamic, string>(r => r.Name).ToList();

        List<JObject> filtereddata = new List<JObject>();
        List<myorderclass> orderclass = new List<myorderclass>();

        if (jsonResources != null)
            foreach (dynamic r in jsonResources)
            {
                bool esito = true;
                if (r.pubblicasito != true)
                    esito = false;
                /*NAZIONE*/
                if (codn != null && codn != "" && esito)
                    if (r.codiceNAZIONE != codn) esito = false;
                /*REGIONE*/
                if (codr != null && codr != "" && esito)
                    if (r.codiceREGIONE != codr) esito = false;
                /*PROVINCIA*/
                if (codp != null && codp != "" && esito)
                    if (r.codicePROVINCIA != codp) esito = false;

                /*COMUNE*/
                if (codc != null && codc != "" && esito)
                    if (r.codiceCOMUNE != codc) esito = false;

                /*Contratto*/
                if (idcontratto != null && idcontratto != "" && esito)
                    if (r.idcontratto != idcontratto) esito = false;


                /*Tipologia*/
                if (idtipologia != null && idtipologia != "" && esito)
                    if (r.idtipologia != idtipologia) esito = false;

                /*Condizione*/
                if (idcondizione != null && idcondizione != "" && esito)
                    if (r.idcondizione != idcondizione) esito = false;

                //nascondiprezzo -> devo visualizzare riservato nel prezzo che visualizzo
                if (pmin != 0 && pmax != 0 && esito)
                    if (!(r.Prezzo1 >= pmin && r.Prezzo1 <= pmax)) esito = false;

                if (mmin != 0 && mmax != 0 && esito)
                    if (!(r.Superficie1 >= mmin && r.Superficie1 <= mmax)) esito = false;

                bool bvetrina = false;
                bool.TryParse(vetrina, out bvetrina);
                if (vetrina != "" && esito)
                    if (r.vetrina != vetrina) esito = false;

                if (esito)
                {
                    myorderclass elem = new myorderclass();
                    elem.orderval = r.Prezzo1.Value;
                    elem.obj = r;
                    orderclass.Add(elem);

                    //string singleresource = Newtonsoft.Json.JsonConvert.SerializeObject(r); //Serializzo  l'oggetto attuale
                    filtereddata.Add(r);

                    if (filtereddata.Count == maxelemnts && maxelemnts != 0) break;
                }
            }
        //    serializedresults = Newtonsoft.Json.JsonConvert.SerializeObject(filtereddata);

        /*PAGINAZIONE*/
        JObject jtot = new JObject();
        jtot.Add("totalRecords", filtereddata.Count);

        filtereddata = new List<JObject>();
        orderclass.Sort(new GenericComparer<myorderclass>("orderval", System.ComponentModel.ListSortDirection.Descending));
        orderclass.ForEach(i => filtereddata.Add(i.obj));

        if (enablepager && page != 0 && pagesize != 0)
        {
            //Facciamo il take skip
            int start = ((page - 1) * pagesize);
            //int end = start + pagesize - 1;
            if (start + pagesize > filtereddata.Count - 1)
                filtereddata = filtereddata.GetRange(start, filtereddata.Count - start);
            else
                filtereddata = filtereddata.GetRange(start, pagesize);
        }
        ret.Add("resource", filtereddata);

        List<JObject> tmplist = (new List<JObject>());
        tmplist.Add(jtot);
        ret.Add("resultinfo", tmplist);

        //Selezioniamo l'immagine primaria
        //........... imgsprimary
        Dictionary<string, Dictionary<string, string>> imgprimary = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, List<Dictionary<string, string>>> imgslistbyidallegato = new Dictionary<string, List<Dictionary<string, string>>>();
        dynamic json1 = Newtonsoft.Json.JsonConvert.DeserializeObject(references.imgsprimary);
        dynamic json2 = Newtonsoft.Json.JsonConvert.DeserializeObject(references.imgscomplete);

        //Link schede immobili filtrati
        Dictionary<string, Dictionary<string, string>> linksurl = new Dictionary<string, Dictionary<string, string>>();
        foreach (dynamic item in filtereddata)
        {
            /////////Caratterstiche principali ////////////////////
            Dictionary<string, string> tmp1 = new Dictionary<string, string>();
            string testotitolo = "";
            string descrizionebreve = "";
            string videourl = "";
            string descrizione = "";
            string idact = item.id;
            foreach (dynamic c in item.dettagliorisorse_1.Children())
            {
                if (c.lingua == lingua)
                {
                    testotitolo = c.titolo;
                    descrizionebreve = c.descrizione;
                    descrizionebreve = CommonPage.ReplaceLinks(descrizionebreve);
                    break;
                }
            }
            foreach (dynamic c in item.dettagliorisorse_2.Children())
            {
                if (c.lingua == lingua)
                {
                    videourl = c.titolo;
                    descrizione = c.descrizione;
                    descrizione = CommonPage.ReplaceLinks(descrizione);
                    break;
                }
            }
            tmp1.Add("video", videourl);
            tmp1.Add("descrizione", SostituisciACapo(descrizione));
            string link = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(testotitolo), idact, "rif000666", "", "");

            tmp1.Add("titolo", testotitolo);
            tmp1.Add("descrizionebreve", SostituisciACapo(descrizionebreve));
            tmp1.Add("link", link);
            linksurl.Add(idact, tmp1);



            ///////////////////////////////////////////

            ///////Allegati e foto///////////////////////////////
            string idallegati = item.id_allegati;
            foreach (dynamic r in json1)
            {
                //estraiamo per l'allegato con guid_link corrispondente a    item.Textfield1_dts il NomeFile e la Descrizione
                if (r.Name == idallegati)
                {
                    foreach (dynamic c in r.Children())
                    {
                        if (!imgprimary.ContainsKey(r.Name))
                        {
                            imgprimary.Add(r.Name, new Dictionary<string, string>());
                        }

                        imgprimary[idallegati].Add("NomeFile", c.NomeFile.ToString());
                        imgprimary[idallegati].Add("Descrizione", c.Descrizione.ToString());
                        imgprimary[idallegati].Add("guid_link", idallegati);
                        imgprimary[idallegati].Add("lingua", lingua);

                        break;
                    }
                    break;
                }
            }
            //Selezioniamo la lista immagini completa con indice id_allegato
            //................. imgscomplete
            foreach (dynamic r in json2)
            {
                if (r.Name == idallegati)
                {
                    if (!imgslistbyidallegato.ContainsKey(r.Name))
                    {
                        imgslistbyidallegato.Add(r.Name, new List<Dictionary<string, string>>());
                    }
                    foreach (dynamic clist in r.Children())
                    {
                        foreach (dynamic c in clist.Children())
                        {
                            //estraiamo per gli allegati con guid_link corrispondente a    item.Textfield1_dts il NomeFile e la Descrizione
                            Dictionary<string, string> tmp = new Dictionary<string, string>();
                            tmp.Add("NomeFile", c.NomeFile.ToString());
                            tmp.Add("Descrizione", c.Descrizione.ToString());
                            tmp.Add("guid_link", idallegati);
                            tmp.Add("Lingua", lingua);


                            ////-> riempiamo il dictionari imgslistbyidallegato ch epoi possiamo serializzare ed iniettare nella pagina
                            imgslistbyidallegato[idallegati].Add(tmp);
                        }
                    }
                    break;
                }
            }
            ///////////////////////////////////////////////////////

        }

        string retimgscomplete = Newtonsoft.Json.JsonConvert.SerializeObject(imgslistbyidallegato);
        string retimgsprimary = Newtonsoft.Json.JsonConvert.SerializeObject(imgprimary);
        dynamic imgsprimary = Newtonsoft.Json.JsonConvert.DeserializeObject(retimgsprimary);
        dynamic imgscomplete = Newtonsoft.Json.JsonConvert.DeserializeObject(retimgscomplete);
        tmplist = (new List<JObject>());
        tmplist.Add(imgsprimary);
        ret.Add("imgsprimary", tmplist);
        tmplist = (new List<JObject>());
        tmplist.Add(imgscomplete);
        ret.Add("imgscomplete", tmplist);


        string retlinksurl = Newtonsoft.Json.JsonConvert.SerializeObject(linksurl);
        dynamic dynretlinksurl = Newtonsoft.Json.JsonConvert.DeserializeObject(retlinksurl);
        tmplist = (new List<JObject>());
        tmplist.Add(dynretlinksurl);
        ret.Add("linklistdetail", tmplist);

#if false

        //Se voglio tornare anche i file di riferimento delle immagini completi
        tmplist = (new List<JObject>());
        dynamic imgsprimary = Newtonsoft.Json.JsonConvert.DeserializeObject(references.imgsprimary);
        tmplist.Add(imgsprimary);
        ret.Add("imgsprimary", tmplist);
        tmplist = (new List<JObject>());
        dynamic imgscomplete = Newtonsoft.Json.JsonConvert.DeserializeObject(references.imgscomplete);
        tmplist.Add(imgscomplete);
        ret.Add("imgscomplete", tmplist); 
#endif

        return ret;
    }
    protected string GeneraBackLink(string Lingua, string CodiceTipologia, bool usacategoria = true)
    {
        string ret = "";
        TipologiaOfferte item = Utility.TipologieOfferte.Find(delegate (TipologiaOfferte tmp) { return (tmp.Lingua == Lingua && tmp.Codice == CodiceTipologia); });

        if (item != null)
        {
            string testourl = item.Descrizione;

            string partipologia = "";
            string parreegione = "";
            string valtipologia = "";
            string valregione = "";
            string addtext = "";

            if (HttpContext.Current.Session["objfiltro"] != null)
            {
                string sobjvalue = HttpContext.Current.Session["objfiltro"].ToString();

                Dictionary<string, string> objvalue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sobjvalue);
                if (objvalue != null && objvalue.Count > 0)
                {

                    //QUESTA MODIFICA SAREBBE  MEGLIO RENDERLA COMUNE INSERENDOLA DENTRO SITEMAPMANAGER E NON TUTTE LE VOLTE CHE LA CHIAMO!!!
                    if (objvalue.ContainsKey("ddlTipologiaSearch") && objvalue["ddlTipologiaSearch"] != null)

                    {
                        partipologia = objvalue["ddlTipologiaSearch"].ToString();
                        valtipologia = references.GetreftipologieValueById(partipologia, Lingua);

                    }
                    if (objvalue.ContainsKey("ddlRegioneSearch") && objvalue["ddlRegioneSearch"] != null)
                    {
                        parreegione = objvalue["ddlRegioneSearch"].ToString();
                        valregione = CommonPage.NomeRegione(parreegione, Lingua);
                    }
                    if (valtipologia != "") addtext += " " + valtipologia;
                    if (valregione != "") addtext += " " + valregione;
                    if (addtext != "") testourl += addtext;

                }


            }
            ret = CommonPage.CreaLinkRoutes(HttpContext.Current.Session, false, Lingua, testourl, "", CodiceTipologia, partipologia, "", parreegione);


            //Prodotto catselected = Utility.ElencoProdotti.Find(delegate (WelcomeLibrary.DOM.Prodotto tmp) { return (tmp.Lingua == Lingua && (tmp.CodiceTipologia == CodiceTipologia && tmp.CodiceProdotto == Categoria)); });
            //if (catselected != null && usacategoria)
            //    testourl = catselected.Descrizione;
            //string tmpcategoria = Categoria;
            //if (!usacategoria) tmpcategoria = "";
            //ret = CommonPage.CreaLinkRoutes(Session, false, Lingua, CommonPage.CleanUrl(testourl), "", CodiceTipologia, tmpcategoria,"",Regione);
        }
        return ret;
    }

    protected string SostituisciACapo(string testo)
    {
        testo = testo.Replace("\r\n", "<br/>");
        testo = testo.Replace("\n", "<br/>");
        testo = testo.Replace("\r", "<br/>");
        return testo;
    }

    protected Dictionary<string, string> FiltraDatiById(HttpContext context, string id, string lingua, string tipologia, bool usecdn = false)
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> linksurl = new Dictionary<string, Dictionary<string, string>>();

        references references = new references(context.Server);
        references.CaricaDatiRisorseDaJson(lingua, "", WelcomeLibrary.STATIC.Global.usecdn);

        string singleresource = "";
        string retimgsprimary = "";
        string retimgscomplete = "";
        //Selezioniamo i dati per l'immobile 
        dynamic jsonResources = Newtonsoft.Json.JsonConvert.DeserializeObject(references.resourcesloaded);
        string idallegati = "";
        foreach (dynamic r in jsonResources)
        {
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            string testotitolo = "";
            string descrizionebreve = "";
            string videourl = "";
            string descrizione = "";
            string idact = r.id;
            //Seleziono per id
            if (idact == id)
            {
                singleresource = Newtonsoft.Json.JsonConvert.SerializeObject(r); //Serializzo  l'oggetto che cerco
                idallegati = r.id_allegati; ;
                /*Costrisco anche il link url per la scheda */
                foreach (dynamic c in r.dettagliorisorse_1.Children())
                {
                    if (c.lingua == lingua)
                    {
                        testotitolo = c.titolo;
                        descrizionebreve = (c.descrizione);
                        descrizionebreve = CommonPage.ReplaceLinks(descrizionebreve);
                        break;
                    }
                }

                foreach (dynamic c in r.dettagliorisorse_2.Children())
                {
                    if (c.lingua == lingua)
                    {
                        videourl = c.titolo;
                        descrizione = c.descrizione;
                        descrizione = CommonPage.ReplaceLinks(descrizione);
                        break;
                    }
                }
                tmp.Add("video", videourl);
                tmp.Add("descrizione", SostituisciACapo(descrizione));

                string link = CommonPage.CreaLinkRoutes(null, false, lingua, CommonPage.CleanUrl(testotitolo), id, "rif000666", "", "");
                //res.Add(id, link);
                tmp.Add("titolo", testotitolo);
                tmp.Add("descrizionebreve", SostituisciACapo(descrizionebreve));
                tmp.Add("link", link);

                //      string contactlink = "";
                //contactlink = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta&Lingua=" + lingua + "&idOfferta=" + idact;
                // tmp.Add("contactlink", contactlink);
                string printlink = WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/aspnetpages/SchedaResourceStampa.aspx?idOfferta=" + idact + "&Lingua=" + lingua;
                string bcklink = GeneraBackLink(lingua, tipologia);
                tmp.Add("printlink", printlink);
                tmp.Add("bcklink", bcklink);
                linksurl.Add(id, tmp);
                break;
            }
        }
        string retlinksurl = Newtonsoft.Json.JsonConvert.SerializeObject(linksurl);
        ret.Add("linklistdetail", retlinksurl);

        // Per ora vuota ... poi vediamo
        Dictionary<string, string> tmp1 = new Dictionary<string, string>();
        ret.Add("resultinfo", Newtonsoft.Json.JsonConvert.SerializeObject(tmp1));

        //Selezioniamo l'immagine primaria
        //........... imgsprimary
        Dictionary<string, Dictionary<string, string>> imgprimary = new Dictionary<string, Dictionary<string, string>>();
        dynamic json1 = Newtonsoft.Json.JsonConvert.DeserializeObject(references.imgsprimary);
        foreach (dynamic r in json1)
        {
            //estraiamo per l'allegato con guid_link corrispondente a    item.Textfield1_dts il NomeFile e la Descrizione
            if (r.Name == idallegati)
            {
                foreach (dynamic c in r.Children())
                {
                    if (!imgprimary.ContainsKey(r.Name))
                    {
                        imgprimary.Add(r.Name, new Dictionary<string, string>());
                    }

                    imgprimary[idallegati].Add("NomeFile", c.NomeFile.ToString());
                    imgprimary[idallegati].Add("Descrizione", c.Descrizione.ToString());
                    imgprimary[idallegati].Add("guid_link", idallegati);
                    imgprimary[idallegati].Add("lingua", lingua);

                    break;
                }
                break;
            }
        }
        retimgsprimary = Newtonsoft.Json.JsonConvert.SerializeObject(imgprimary);
        //Selezioniamo la lista immagini completa con indice id_allegato
        //................. imgscomplete
        dynamic json2 = Newtonsoft.Json.JsonConvert.DeserializeObject(references.imgscomplete);
        Dictionary<string, List<Dictionary<string, string>>> imgslistbyidallegato = new Dictionary<string, List<Dictionary<string, string>>>();
        foreach (dynamic r in json2)
        {
            if (r.Name == idallegati)
            {
                if (!imgslistbyidallegato.ContainsKey(r.Name))
                {
                    imgslistbyidallegato.Add(r.Name, new List<Dictionary<string, string>>());
                }
                foreach (dynamic clist in r.Children())
                {
                    foreach (dynamic c in clist.Children())
                    {
                        //estraiamo per gli allegati con guid_link corrispondente a    item.Textfield1_dts il NomeFile e la Descrizione
                        Dictionary<string, string> tmp = new Dictionary<string, string>();
                        tmp.Add("NomeFile", c.NomeFile.ToString());
                        tmp.Add("Descrizione", c.Descrizione.ToString());
                        tmp.Add("guid_link", idallegati);
                        tmp.Add("Lingua", lingua);

                        //Ineriamo il rapporto di aspetto dell'immagine
                        try
                        {
                            var percorsoapptmp = WelcomeLibrary.STATIC.Global.percorsoapp;
                            if (WelcomeLibrary.STATIC.Global.usecdn)
                                percorsoapptmp = WelcomeLibrary.STATIC.Global.percorsocdn;

                            string tmppathimmagine = percorsoapptmp + WelcomeLibrary.STATIC.Global.percorsoimg + idallegati + "/" + "ant" + c.NomeFile.ToString();

                            System.Net.WebRequest req = System.Net.WebRequest.Create(tmppathimmagine);
                            System.Net.WebResponse response = req.GetResponse();
                            using (System.IO.Stream stream = response.GetResponseStream())
                            {
                                //using (System.Drawing.Image tmpimg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(tmppathimmagine)))
                                using (System.Drawing.Image tmpimg = System.Drawing.Image.FromStream(stream))
                                {
                                    tmp.Add("imageratio", ((double)tmpimg.Width / (double)tmpimg.Height).ToString());
                                }
                            }
                        }
                        catch
                        { tmp.Add("imageratio", "1"); }


                        ////-> riempiamo il dictionari imgslistbyidallegato ch epoi possiamo serializzare ed iniettare nella pagina
                        imgslistbyidallegato[idallegati].Add(tmp);
                    }
                }
                break;
            }
        }
        retimgscomplete = Newtonsoft.Json.JsonConvert.SerializeObject(imgslistbyidallegato);



        ret.Add("resource", singleresource);
        ret.Add("imgsprimary", retimgsprimary);
        ret.Add("imgscomplete", retimgscomplete);

        return ret;
    }



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}



public class myorderclass
{
    public JObject obj { get; set; }
    public double orderval { get; set; }


}


//var sortedRows = rows.AsQueryable().OrderBy(r => r, new MyJObjectComparer());
//public class MyJObjectComparer : IComparer<JObject>
//{
//    public int Compare(JObject a, JObject b)
//    {
//        double obj1p = 0;
//        double obj2p = 0;
//        double.TryParse(a.Prezzo1.Value, out obj1p);

//        if ((a["Prezzo1"] == b["Prezzo1"]) && (a["Column2"] == b["Column2"]))
//            return 0;

//        if ((a["Column1"] < b["Columnq"]) || ((a["Column1"] == b["Column1"]) && (a["Column2"] < b["Column2"])))
//            return -1;

//        return 1;
//    }
//}