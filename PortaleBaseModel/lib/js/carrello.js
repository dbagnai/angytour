"use strict";

$(document).ready(function () {
    connectCarrelloEvents();
});


var carrellotool = new function () {
    var idprodotto = "";
    var idcarrello = "";
    var idcombined = "";
    var username = "";
    var controlid = "";
    var prv = -1, cur = -1;
    var configview = 1; //1 solo calendario , 2 solo tasti +/-, 3 entrambi
    return {
        modifyscopevalues: function (pidrisorsa, pidcombined, pidcarrello, pusername, idcontrollo, cfgvista) {
            idprodotto = pidrisorsa;
            idcombined = pidcombined;
            idcarrello = pidcarrello;
            var pusername = pusername || username;
            username = pusername;
            var idcontrollo = idcontrollo || controlid;
            controlid = idcontrollo;
            var cfgvista = cfgvista || configview;
            configview = cfgvista;
        },
        initcarrellotool: function (pidrisorsa, pidcombined, pusername, idcontrollo, cfgvista) {
            idprodotto = pidrisorsa;
            idcombined = pidcombined;
            username = pusername;
            controlid = idcontrollo;
            var cfgvista = cfgvista || configview;
            configview = cfgvista;
            //initcalendarrange();

            (function wait() { //Aspettiamo che il controllo sia iniettato e inzializziamo i tati
                if ($("#" + controlid).length) {
                    initcalendarrange();
                } else {
                    setTimeout(wait, 80);
                }
            })();

            return;
        },
        inserisciacarrelloquantita: function () { //inserisce nel carrello una quantità esatta per l'articolo calcolata con la differnza dei giorni tramite le date
            var start = new Date(Math.min(prv, cur));
            var end = new Date(Math.max(prv, cur));
            // console.log("inserisci carrello " + idprodotto + " " + start + " " + end);
            var Enddate = moment(end).set({ hour: 0, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
            var Startdate = moment(start).set({ hour: 0, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
            var quantita = moment(end).diff(moment(start), 'days');

            //VERSIONE CHE PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            AddCurrentCarrelloNopostback('', idprodotto, lng, username, idcombined, idcarrello, '', Startdate, Enddate, '', quantita + 1, true,
                function (data) {
                    /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/
                    var ret = "";
                    var parsedret = "";
                    if (data != null && data != "")
                        parsedret = JSON.parse(data);
                    if (parsedret != null && parsedret.hasOwnProperty("id"))
                        ret = parsedret.id;

                    idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                    /***********************************************************************************************************************************************/
                    openLink('/AspnetPages/Shoppingcart.aspx?Lingua=' + lng);//da verificare se redirect da funzione è ok
                });

            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            //AddCurrentCarrelloNopostback('', idprodotto, lng, username, idcombined, idcarrello, '0', Startdate, Enddate, '', quantita + 1, false,
            //    function (data) {
            //        /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/
            //        var ret = "";
            //        var parsedret = "";
            //        if (data != null && data != "")
            //            parsedret = JSON.parse(data);
            //        if (parsedret != null && parsedret.hasOwnProperty("id"))
            //            ret = parsedret.id;

            //        //idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
            //        /***********************************************************************************************************************************************/
            //        openLink('/AspnetPages/Shoppingcart.aspx?Lingua=' + lng);//da verificare se redirect da funzione è ok
            //    });

            return;
        },
        aggiornacarrello: function (operatingtype) {
            var operatingtype = operatingtype || '';
            getjsonfield(function (jsondetails) { //Leggiamo eventuali proprieta dell'articolo e aggiungiamole all'elemento del carrello attuale

                if (jsondetails != null && jsondetails.hasOwnProperty("notvalidmsg")) //controllo della validazione dei dati dettaglio dal form
                {
                    $('#' + controlid + "messages").html(jsondetails["notvalidmsg"]);
                    return;
                }
                //////////MODIFICA DEL PREZZO ARTICOLO SULLO SCAGLIONE//////////
                var prezzo = "";
                // forziamo il prezzo articolo per il caso degli scaglioni prendendolo dallo scaglione
                if (jsondetails != null && jsondetails.hasOwnProperty("prezzo") && jsondetails.hasOwnProperty("idscaglione"))
                    prezzo = jsondetails['prezzo'];
                ////////////////////////////
                var Jsonfield1 = JSON.stringify(jsondetails);

                var quantitarichiesta = $('#' + controlid + "qtyi").val(); //setto la quantita presente per fare solo l'aggiornamento

                if (quantitarichiesta != '' && quantitarichiesta != '0')
                    if (operatingtype == '') {
                        //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
                        AddCurrentCarrelloNopostback('', idprodotto, lng, username, idcombined, '', prezzo, null, null, Jsonfield1, quantitarichiesta, false, function (data) {
                            var ret = "";
                            var parsedret = "";
                            if (data != null && data != "")
                                parsedret = JSON.parse(data);
                            if (parsedret != null && parsedret.hasOwnProperty("id"))
                                ret = parsedret.id;

                            var codetoexecute = "";
                            if (parsedret != null && parsedret.hasOwnProperty("jscodetoexecute")) {
                                codetoexecute = parsedret.jscodetoexecute;
                                $('#' + "endspaceforjs").html(codetoexecute);
                            }

                            $('#' + controlid + "messages").html(parsedret.stato);
                            idcarrello = ret;  //(aggiunta)comunque memorizzo l'id del record carrello inserito o modificato
                            carrellotool.caricaquantita();
                        });
                    }
                    else if (operatingtype == 'multiplo') {
                        //VERSIONE CHE PERMETTE DI INSERIRE PIù RIGHI CARRELLO CON STESSO PRODOTTO
                        AddCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, prezzo, null, null, Jsonfield1, quantitarichiesta, true, function (data) {
                            var ret = "";
                            var parsedret = "";
                            if (data != null && data != "")
                                parsedret = JSON.parse(data);
                            if (parsedret != null && parsedret.hasOwnProperty("id"))
                                ret = parsedret.id;

                            var codetoexecute = "";
                            if (parsedret != null && parsedret.hasOwnProperty("jscodetoexecute")) {
                                codetoexecute = parsedret.jscodetoexecute;
                                $('#' + "endspaceforjs").html(codetoexecute);
                            }

                            $('#' + controlid + "messages").html(parsedret.stato);
                            /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/
                            idcarrello = ret;   //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                            /***********************************************************************************************************************************************/
                            carrellotool.caricaquantita(operatingtype);
                        });
                    }
            });
            return;
        },
        aggiungiacarrello: function (operatingtype) {
            var operatingtype = operatingtype || '';

            getjsonfield(function (jsondetails) { //Leggiamo eventuali proprieta dell'articolo e aggiungiamole all'elemento del carrello attuale

                if (jsondetails != null && jsondetails.hasOwnProperty("notvalidmsg")) //controllo della validazione dei dati dettaglio dal form
                {
                    $('#' + controlid + "messages").html(jsondetails["notvalidmsg"]);
                    return;
                }
                //////////MODIFICA DEL PREZZO ARTICOLO SULLO SCAGLIONE//////////
                var prezzo = "";
                // forziamo il prezzo articolo per il caso degli scaglioni prendendolo dallo scaglione
                if (jsondetails != null && jsondetails.hasOwnProperty("prezzo") && jsondetails.hasOwnProperty("idscaglione"))
                    prezzo = jsondetails['prezzo'];
                ////////////////////////////
                var Jsonfield1 = JSON.stringify(jsondetails);

                if (operatingtype == '') {
                    //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
                    AddCurrentCarrelloNopostback('', idprodotto, lng, username, idcombined, '', prezzo, null, null, Jsonfield1, '', false, function (data) {
                        var ret = "";
                        var parsedret = "";
                        if (data != null && data != "")
                            parsedret = JSON.parse(data);
                        if (parsedret != null && parsedret.hasOwnProperty("id"))
                            ret = parsedret.id;
                        $('#' + controlid + "messages").html(parsedret.stato);
                        idcarrello = ret;  //(aggiunta)comunque memorizzo l'id del record carrello inserito o modificato
                        carrellotool.caricaquantita();
                    });
                }
                else if (operatingtype == 'multiplo') {
                    //VERSIONE CHE PERMETTE DI INSERIRE PIù RIGHI CARRELLO CON STESSO PRODOTTO
                    AddCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, prezzo, null, null, Jsonfield1, '', true, function (data) {
                        var ret = "";
                        var parsedret = "";
                        if (data != null && data != "")
                            parsedret = JSON.parse(data);
                        if (parsedret != null && parsedret.hasOwnProperty("id"))
                            ret = parsedret.id;
                        $('#' + controlid + "messages").html(parsedret.stato);
                        /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/
                        idcarrello = ret;   //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                        /***********************************************************************************************************************************************/
                        carrellotool.caricaquantita(operatingtype);
                    });
                }
            });
            return;
        },
        sottradiacarrello: function (operatingtype) {
            var operatingtype = operatingtype || '';
            getjsonfield(function (jsondetails) { //Leggiamo eventuali proprieta dell'articolo e aggiungiamole all'elemento del carrello attuale
                if (jsondetails != null && jsondetails.hasOwnProperty("notvalidmsg")) //controllo della validazione dei dati dettaglio dal form
                {
                    $('#' + controlid + "messages").html(jsondetails["notvalidmsg"]);
                    return;
                }
                //////////MODIFICA DEL PREZZO ARTICOLO SULLO SCAGLIONE//////////
                var prezzo = "";
                // forziamo il prezzo articolo per il caso degli scaglioni prendendolo dallo scaglione
                if (jsondetails != null && jsondetails.hasOwnProperty("prezzo") && jsondetails.hasOwnProperty("idscaglione"))
                    prezzo = jsondetails['prezzo'];
                ////////////////////////////


                var Jsonfield1 = JSON.stringify(jsondetails);
                if (operatingtype == '') {
                    //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
                    SubtractCurrentCarrelloNopostback('', idprodotto, lng, username, idcombined, '', prezzo, null, null, Jsonfield1, '', false, function (data) {
                        var ret = "";
                        var parsedret = "";
                        if (data != null && data != "")
                            parsedret = JSON.parse(data);
                        if (parsedret != null && parsedret.hasOwnProperty("id"))
                            ret = parsedret.id;
                        $('#' + controlid + "messages").html(parsedret.stato);
                        idcarrello = ret;  //(aggiunta)comunque memorizzo l'id del record carrello inserito o modificato
                        carrellotool.caricaquantita();
                    });
                }
                else if (operatingtype == 'multiplo') {
                    //VERSIONE CHE  PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
                    SubtractCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, prezzo, null, null, Jsonfield1, '', true, function (data) {
                        var ret = "";
                        var parsedret = "";
                        if (data != null && data != "")
                            parsedret = JSON.parse(data);
                        if (parsedret != null && parsedret.hasOwnProperty("id"))
                            ret = parsedret.id;
                        $('#' + controlid + "messages").html(parsedret.stato);
                        /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO e  modificare parametro ************************************************************/
                        idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                        /***********************************************************************************************************************************************/
                        carrellotool.caricaquantita(operatingtype);
                    });
                }

            });
            return;
        },
        caricaquantita: function (operatingtype) {
            var operatingtype = operatingtype || '';

            //Testiamo se presenti le caselle di specifica delle caratteristiche relative al controllo quantità - devono esserci entrambe e valorizzate per funzinonare
            if ($('#' + controlid + "Caratteristica1").length > 0 && $('#' + controlid + "Caratteristica2").length > 0) {
                if ($('#' + controlid + "Caratteristica1")[0].value != '' && $('#' + controlid + "Caratteristica2")[0].value != '')
                    idcombined = $('#' + controlid + "Caratteristica1")[0].value + "-" + $('#' + controlid + "Caratteristica2")[0].value;
                else
                    idcombined = ""; //svuoto se anche uno solo è vuoto !!!
                //debugger;
            }

            if (operatingtype == '') {
                //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
                GetCurrentCarrelloQty('', idprodotto, idcombined, idcarrello, false, function (ret) {
                    var casellaqty = "<input style =\"width:40px;margin-top:10px;text-align:center\" class=\"form-control\" id='" + controlid + "qtyi' value='" + ret + "' />";
                    $('#' + controlid + "qty").html(casellaqty);
                });
            }
            else if (operatingtype == 'multiplo') {
                //VERSIONE CHE PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
                GetCurrentCarrelloQty('', idprodotto, idcombined, idcarrello, true, function (ret) {
                    var casellaqty = "<input style =\"width:40px;margin-top:10px;text-align:center\" class=\"form-control\" id='" + controlid + "qtyi' value='" + ret + "' />";
                    $('#' + controlid + "qty").html(casellaqty);
                });
            }

        },
        caricatotale: function () { //Carica il totale a carrello attuale per l'elemento passato  
            GetCarrelloTotalForItem(idprodotto, idcombined, idcarrello, function (ret) {
                $('#' + controlid + "price").val(ret);
            });
            return;
        },
        calcolatotale: function () {
            GetPriceForItem(idprodotto, function (ret) {
                var prezzototaleitem = '';
                if (prv != -1 && cur != -1) {
                    var start = new Date(Math.min(prv, cur));
                    var end = new Date(Math.max(prv, cur));
                    // console.log("inserisci carrello " + idprodotto + " " + start + " " + end);
                    var Enddate = moment(end).set({ hour: 0, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
                    var Startdate = moment(start).set({ hour: 0, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
                    var quantita = moment(end).diff(moment(start), 'days');
                    if (!isNaN(Number(ret))) {
                        var totaleprodotto = Number(ret) * (quantita + 1);
                        prezzototaleitem = totaleprodotto.formatMoney(".", ",");
                    }
                }
                $('#' + controlid + "price").val(prezzototaleitem);
            });

            return;
        }
    };

    //////////////////////////////////////////////////////////////
    ///PREPARA IL JSON A PARTIRE DEI CAMPI DI INSERIMENTO SUL FORM
    //////////////////////////////////////////////////////////////
    function getjsonfield(callback) {
        var jsondetails = {};
        var mustvalidate = false;

        /////////////////////////////////////////////////////////////////////
        //Memorizzazione caratteristiche secondarie selezionabili a carrello
        /////////////////////////////////////////////////////////////////////
        $(".ddlproperties").each(function () {
            var idelem = $(this).attr('id');  //id delle select box
            var proprarr = $(this).attr('bindingprop');
            //////
            var needed = $(this).attr('needed');
            if (needed != null && needed != "" && needed == "true")
                mustvalidate = true;
            else
                mustvalidate = false;
            //////
            if ((idelem != null && idelem != "") && (proprarr != null && proprarr != "")) {
                var valore = $("#" + idelem + " option:selected").val();
                jsondetails[proprarr] = valore;
                //check per valore necessario
                if (mustvalidate)
                    if (valore == null || valore == "") {
                        if (jsondetails["notvalidmsg"] == null) jsondetails["notvalidmsg"] = (GetResourcesValue("msgcarrellovalido") + "<br/>");
                        jsondetails["notvalidmsg"] += (GetResourcesValue("select" + proprarr.toLowerCase()) + "<br/>");
                    }
            }
        });

        ///////////////////////////////////////////////////////////////////////////////////////////
        //////////GESTIONE memorizzazione PER SCADENZE PARTENZE nel carrello
        //////////////////////////////////////////////////////////////////////////////////////////
        //controllo select con nome "#" + controlid + "dllscaglione" contine la lista scaglione/idscaglione selezionato
        // controllo input di tipo hidden "#" + controlid + "hiddenscaglioni" contiene il serializzato encodedbase64 competo della lsita scaglioni
        // controllo input di tipo hidden "#" + controlid + "hiddenstatus" contiene il serializzato encodedbase64 competo della lsita statuslist
        // controllo input di tipo hidden "#" + controlid + "hiddenetalist" contiene il serializzato encodedbase64 competo della lsita etalist
        // controllo input di tipo hidden "#" + controlid + "hiddencoordlist" contiene il serializzato encodedbase64 competo della lsita dei coordinatori indicizzata per idscaglione 
        if ($("#" + controlid + "dllscaglione").length) {
            var actcontrol = $("#" + controlid + "dllscaglione");
            var idscaglione = actcontrol.val();
            var statoscaglione = 0;
            var needed = actcontrol.attr('needed');
            if (needed != null && needed != "" && needed == "true")
                mustvalidate = true;
            else
                mustvalidate = false;
            var scaglioniserialized = $("#" + controlid + "hiddenscaglioni").val();
            scaglioniserialized = b64ToUtf8(scaglioniserialized);
            if (scaglioniserialized != null && scaglioniserialized != '') {
                var scaglioni = JSON.parse(scaglioniserialized);
                //se non ci sono scaglioni inseriti
                if (scaglioni.length != 0) {
                    //devo cercare nella lsita scaglioni quello con id == idscaglione selezionato per inserilo nel record del carrello
                    for (var j = 0; j < scaglioni.length; j++) {
                        if (scaglioni[j] != null && scaglioni[j].hasOwnProperty("id") && scaglioni[j]["id"] == idscaglione) {

                            //Contorllo lo stato dello scaglione per bloccare l'acquisto quando non possibile in base allo stato
                            if (scaglioni[j].hasOwnProperty("stato"))
                                statoscaglione = scaglioni[j]['stato'];
                            if (!isNaN(Number(statoscaglione)))
                                statoscaglione = Number(statoscaglione);

                            //Preparazione di jsondetails mettiamo idscaglione , prezzo , datapartenza, dataritorno diretti nel record carrello jsonfield1
                            jsondetails["idscaglione"] = idscaglione;
                            if (scaglioni[j].hasOwnProperty("prezzo"))
                                jsondetails["prezzo"] = scaglioni[j]['prezzo'];
                            if (scaglioni[j] != null && scaglioni[j].hasOwnProperty("datapartenza"))
                                jsondetails["datapartenza"] = moment(new Date(scaglioni[j]['datapartenza'])).format("YYYY-MM-DD HH:mm:ss");
                            if (scaglioni[j] != null && scaglioni[j].hasOwnProperty("durata")) {
                                var durata = scaglioni[j]["durata"];
                                jsondetails["dataritorno"] = moment(new Date(scaglioni[j]['datapartenza'])).add(durata - 1, 'days').format("YYYY-MM-DD HH:mm:ss");
                            }

                            /////////////////////
                            if ($("#" + controlid + "ddlassicurazioni").length) {
                                var assdropdown = $("#" + controlid + "ddlassicurazioni");
                                var nassicurazioni = assdropdown.val();
                                if (scaglioni[j].addedvalues["costoassicurazione"] != undefined && scaglioni[j].addedvalues["costoassicurazione"] != '') {
                                    $("#" + controlid + "option1group").show()
                                    jsondetails["costoassicurazione"] = scaglioni[j].addedvalues["costoassicurazione"];// il valore addedvalues è caricato nella gestione dello scaglione
                                }
                                else { delete jsondetails['costoassicurazione']; $("#" + controlid + "option1group").hide() }
                                if (nassicurazioni != '0') {
                                    jsondetails["nassicurazioni"] = nassicurazioni;
                                } else delete jsondetails['nassicurazioni'];

                            }
                            /////////////////////

                            //Con la seguente metto nel record del carrello tutti i dati completi dello scaglione selezionato serializzato in jsonfield1
                            //jsondetails["scaglione"] = JSON.stringify(scaglioni[j]);
                            jsondetails["scaglione"] = scaglioni[j];

                        }
                    }
                    //fare la validazione se mustvalidate e controllo stato scaglioni
                    if (mustvalidate)
                        if (idscaglione == null || idscaglione == "" || statoscaglione >= 5) {
                            if (jsondetails["notvalidmsg"] == null) jsondetails["notvalidmsg"] = (GetResourcesValue("msgcarrellovalido") + "<br/>");
                            if (idscaglione == null || idscaglione == "") jsondetails["notvalidmsg"] += (GetResourcesValue("msgcarrellodata") + "<br/>");
                            if (statoscaglione >= 5) jsondetails["notvalidmsg"] += (GetResourcesValue("msgcarrellostato") + "<br/>");
                        }
                }
            }

        }
        callback(jsondetails);
    }

    function Visualizzatasti(abilita) {
        var abilita = abilita || false;
        var optiontype = $('#' + controlid + "qty").attr('optiontype');

        //versione con calendari di selezione
        if (configview == 1 || configview == 3) {
            $('#' + controlid + "messages").html('');
            var onclickevent = "style=\"width:160px;cursor:pointer;margin-top:10px\" onclick =\"carrellotool.inserisciacarrelloquantita()\"";
            if (!abilita) onclickevent = "style=\"width:160px;cursor:pointer;margin-top:10px\"";
            var btninserisci = "<div class=\"divbuttonstyle\"  " + onclickevent + ">" + GetResourcesValue("testoinseriscicarrello") + "</div>";
            $('#' + controlid + "messages").append(btninserisci);
            carrellotool.calcolatotale();
        }

        //versione standard carrello
        if (configview == 2 || configview == 3) {
            $('#' + controlid + "addsingle").html('');
            $('#' + controlid + "plus").html('');
            $('#' + controlid + "minus").html('');

            var onclickevent1 = "style=\"cursor:pointer;margin-top:10px\" onclick =\"carrellotool.aggiungiacarrello('" + optiontype + "')\"";
            // if (!abilita) onclickevent1 = "style=\"width:60px;cursor:pointer;margin-top:10px\"";
            var btnaggiungi = "<div class=\"button-carrello\" style=\"padding-left: 2px !important;font-size: 1.4rem;\" " + onclickevent1 + ">+</div>";
            $('#' + controlid + "plus").append(btnaggiungi);

            var btnaggiungisingle = "<div class=\"divbuttonstyle\"  onclick =\"carrellotool.aggiungiacarrello('" + optiontype + "')\">" + GetResourcesValue("testoinseriscicarrellostd") + "</div>";
            $('#' + controlid + "addsingle").append(btnaggiungisingle);

            var onclickevent2 = "style=\"cursor:pointer;margin-top:10px\" onclick =\"carrellotool.sottradiacarrello('" + optiontype + "')\"";
            // if (!abilita) onclickevent1 = "style=\"width:60px;cursor:pointer;margin-top:10px\"";
            var btnsottrai = "<div class=\"button-carrello\" style=\"padding-left: 2px !important;font-size: 2rem;\" " + onclickevent2 + ">-</div>";
            $('#' + controlid + "minus").append(btnsottrai);

            /////////////////////////////////////////////////////////////////////////////////////////////////
            ////GESTIONE PROPRIETA' CARATTERISTICHE DEL PRODOTTO NON COMBINATE COME PROPRIETA DEL CARRELLO DA INSERIRE IN jsonfield1 alla selezione
            /////////////////////////////////////////////////////////////////////////////////////////////////
            $(".ddlproperties").each(function () {
                $('#' + controlid + "addproperties").show();
                var proprarr = $(this).attr('bindingprop');
                var selectedlist = "";
                switch (proprarr) {
                    case "Caratteristica1":
                        selectedlist = JSONcar1;
                        break;
                    case "Caratteristica2":
                        selectedlist = JSONcar2;
                        break;
                    case "Caratteristica3":
                        selectedlist = JSONcar3;
                        break;
                    case "":
                        break;
                }
                $(this).change(setviewfield);//visualizziamo immagine correlata alla selezione

                // per riempire la ddl
                var idcontrollo = $(this).attr('id');
                var selstring = '';
                if (baseresources != null && baseresources.hasOwnProperty(lng) && baseresources[lng].hasOwnProperty("select" + proprarr.toLowerCase()))
                    selstring = baseresources[lng]["select" + proprarr.toLowerCase()];
                //Se la box contiene solo un valore lo presetto
                var selectedvalueact = "";
                if (selectedlist != null && selectedlist.length == 1)
                    selectedvalueact = parseddatas[0].Codice;
                convertToDictionaryandFill(selectedlist, 0, lng, idcontrollo, selstring, '', selectedvalueact, "");
            });



            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Riempio le selectbox con i valori delle caratteristiche ( i valori da bindare li devo predendere in base a idprodotto)
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            GetCarcombinedForItem(idprodotto, function (data) {
                if (data != null && data != '') {
                    var dataparsed = JSON.parse(data);
                    $('#' + controlid + "cars").show();
                    $(".ddlcaratteristiche").each(function () {
                        var localidcontrol = controlid;
                        var localidp = idprodotto;
                        var localidcombine = idcombined;
                        //console.log(localidp + localidcontrol + localidcombine);

                        var idcontrollo = $(this).attr('id');
                        //var proprarr = idcontrollo.replace(controlid, "");

                        $(this).change(carrellotool.caricaquantita);
                        var proprarr = $(this).attr('bindingprop');
                        var filter = "";//$(this).attr("myfilter");
                        var selectedvalueact = "";
                        /*Se passo il valore corretto -> presetto i valori idcombined*/
                        //if (objfiltroint != null && objfiltroint.hasOwnProperty(proprarr))
                        //    selectedvalueact = objfiltroint[proprarr];

                        var selstring = '';
                        if (baseresources != null && baseresources.hasOwnProperty(lng) && baseresources[lng].hasOwnProperty("select" + proprarr.toLowerCase()))
                            selstring = baseresources[lng]["select" + proprarr.toLowerCase()];
                        if (dataparsed != null && dataparsed.hasOwnProperty(proprarr) && dataparsed[proprarr] != null && dataparsed[proprarr] != '') {
                            var parseddatas = JSON.parse(dataparsed[proprarr]);
                            //Se la box contiene solo un valore lo presetto
                            if (parseddatas.length == 1)
                                selectedvalueact = parseddatas[0].Codice;
                            convertToDictionaryandFill(parseddatas, 0, lng, idcontrollo, selstring, '', selectedvalueact, filter);
                            carrellotool.caricaquantita(optiontype);
                        }
                        else $(this).hide();
                    });
                }
                else $('#' + controlid + "cars").remove();
            });
            ///////////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////////////////////////
            //////////GESTIONE VISUALIZZAZIONE PER SCADENZE PARTENZE
            ///////////////////////////////////////////////////////////////////////////////////////////
            if ($("#" + controlid + "dllscaglione").length) {
                //impostiamo i valori di dettaglio nella visualizzazione se necessario al change della ddl
                //imposto la funzione di change
                $("#" + controlid + "dllscaglione").change(setscaglionedetail);
                $("#" + controlid + "ddlassicurazioni").change(aggiornaassicurazione); //aggiorna il carrello al cambio assicurazione

                /*RICARICAMENTO CARRELLO E SELEZIONE IN DDL*/
                //Caricando i prodotti a carrello cerco se esiste un elemento con idprodotto uguale a quello della scheda aperta,
                //se questo ha un idscaglione setto il valore nella ddlscaglione  
                //stessa cosa setto la  ddlassicurazioni col n.assicurazioni
                ////carica il serializzato di tutti gli elementi presenti nel carrello attuale
                if (idprodotto != '')
                    GetCarrelloItems('', function (data) { //carico tutto il carrello come elemento json serializzato per impostare la visualizzazione nella scheda
                        var carrellocompleto = {};
                        //facendo il parse hai l'oggetto carrello completo!!!
                        if (Object.prototype.toString.call(data) === "[object String]" && testJSON(data)) {
                            carrellocompleto = JSON.parse(data);
                            //Cerco l'elemento con l'idprodotto voluto
                            for (var j = 0; j < carrellocompleto.length; j++) {
                                var idcarrelloact = carrellocompleto[j].ID;
                                var idprodottoincarello = carrellocompleto[j].id_prodotto;
                                if (idprodotto == idprodottoincarello && idprodotto != '') {
                                    if (Object.prototype.toString.call(carrellocompleto[j].jsonfield1) === "[object String]" && testJSON(carrellocompleto[j].jsonfield1)) {
                                        var parsedjson = JSON.parse(carrellocompleto[j].jsonfield1);
                                        var idscaglione = parsedjson.idscaglione;
                                        var costoassicurazione = parsedjson.costoassicurazione;
                                        var nassicurazioni = parsedjson.nassicurazioni;
                                        if ($("#" + controlid + "dllscaglione").length && idscaglione != undefined)
                                            $("#" + controlid + "dllscaglione").val(idscaglione);
                                        if ($("#" + controlid + "ddlassicurazioni").length && nassicurazioni != undefined) {
                                            $("#" + controlid + "ddlassicurazioni").val(nassicurazioni);
                                        }

                                        idcarrello = idcarrelloact;//Imposto la selezione dell'elemento a carello nel caso presenza scaglioni
                                        $("#" + controlid + "dllscaglione").trigger('change', ['notemptycart']); //Chiama il click per aggiornare le variabili javascript senza vuotare il carrello
                                        break;

                                    }
                                }
                            }
                        }
                    });
                /* FINE RICARICAMENTO CARRELLO*/
            }

            carrellotool.caricaquantita(optiontype);
        }

    }
    function aggiornaassicurazione(evt, param) {
        carrellotool.aggiornacarrello();
    }
    function setscaglionedetail(evt, param) {
        var param = param || '';
        console.log('inserire qui le modifiche al form da visualizzare al cambio scaglione!!!' + ' idcarrello: ' + idcarrello);

        //Eliminiamo l'elemento dal carrello al cambio scaglione 
        //(funziona solo se aggiungo o tolgo qualcosa a carrello! in quanto viene valorizzato idcarrello, 
        //altrimenti dovrei alla selezione scaglione valorizare l'idcarrelo corrispondente .. facendo un caricamento del carrello dal server
        if (idcarrello != '' && param != 'notemptycart') {
            CancellaCurrentCarrellobyid(idcarrello, function (ret) {

                //svuoto casella assicurazione
                if ($("#" + controlid + "ddlassicurazioni").length)
                    $("#" + controlid + "ddlassicurazioni").val('0');

                //da vedere se mandare un messaggio
                console.log('eliminato voce' + ret);
                carrellotool.caricaquantita();
            });
        }

        //id ddl per la selizione scaglione
        //var idcontrollo = $(event.target).attr('id');
        //var valore = $("#" + idcontrollo + " option:selected").val(); //qyest è l'id database dello scaglione
        //var testo = $("#" + idcontrollo + " option:selected").text();

        //Prendiamo lo scaglione selezionato e visualizziamo i valori
        getjsonfield(function (jsondetails) { //Leggiamo eventuali proprieta dell'articolo

            //////////////////////////////////////////////
            //Visualizzo alcuni dati dello scaglioone selezionato
            if (jsondetails != null && jsondetails.hasOwnProperty("costoassicurazione") && jsondetails["costoassicurazione"] != undefined && jsondetails.hasOwnProperty("idscaglione")) {
                $("#" + controlid + "option1infos").html(GetResourcesValue("lblcostoass") + ' ' + jsondetails["costoassicurazione"] + "€" + "/" + GetResourcesValue("lblpersona"));
            } else $("#" + controlid + "option1infos").html("");
            //////////////////////////////////////////////

            //////////////////////////////////////////////
            //Evidenziamo il coordinatore se presente
            //////////////////////////////////////////////
            $(".coordstylecontainer").html('');//cancello lo stile inserito per la visualizzazione coordinatori
            if (jsondetails != null && jsondetails.hasOwnProperty("idscaglione")) {
                //cerchiamo nell'hiddenfield id coordinatore per lo scaglione e inseriamo la classe per evidenziare in pagina
                var coordserialized = $("#" + controlid + "hiddencoordlist").val();
                coordserialized = b64ToUtf8(coordserialized);
                if (coordserialized != null && coordserialized != '') {
                    var coordbyscaglione = JSON.parse(coordserialized);
                    if (coordbyscaglione != null && coordbyscaglione.hasOwnProperty(jsondetails["idscaglione"]) && coordbyscaglione[jsondetails["idscaglione"]] != undefined) {
                        var idcoordscaglione = coordbyscaglione[jsondetails["idscaglione"]]["id"];
                        if (idcoordscaglione != undefined) {
                            //potresti inserire in pagina lo stile per evidenziare del tipo
                            //<style>.coordid-idcoord{ border-color:red;  } </style>
                            var strigstyle = "<style>.coordid-" + idcoordscaglione + "{ border-color:#e18d0c;  } [class*='coordid-']:not(.coordid-" + idcoordscaglione + ") {  display:none; } </style>";
                            //var strigstyle = "<style>.coordid-" + idcoordscaglione + "{ border-color:#e18d0c;  }  </style>";
                            $(".coordstylecontainer").html(strigstyle);
                        }
                    }
                }
            }
            //////////////////////////////////////////////


            //if (jsondetails != null && jsondetails.hasOwnProperty("prezzo") && jsondetails.hasOwnProperty("idscaglione"))
            //prezzo = jsondetails['prezzo'];

            //controlli da valorizzare ( da vedere dove serve all'interno di jsondetails ho i valori per la visualizzazione)
            // mettendoli nel template posso trovarli e modificarli di seguito
            //$("#" + controlid + "testodadecidere")

            //QUI POTREI CARICARE O LEGGERE DA CAMPO HIDDEN VALORI DEL COORDINATORE E VISUALIZZARLI IN BASE ALL'IDSCAGLIONE SELEZIONATO
            //.....
        });



    }

    //Visualizza in base alla selezione della selectbox l'immagine giusta
    function setviewfield() {
        var idcontrollo = $(event.target).attr('id');
        var valore = $("#" + idcontrollo + " option:selected").val();
        var testo = $("#" + idcontrollo + " option:selected").text();
        var destinationid = idcontrollo + "view";
        if ($("#" + destinationid).length > 0) {
            $("#" + destinationid).html(testo);
            var imgprefix = $("#" + destinationid).attr("imgprefix");
            var pathimgselected = percorsocontenuti + "/con001000/1/" + imgprefix + "-" + valore + ".jpg";
            var ccsvalue = 'background-image:url(' + pathimgselected + ');  background-position: center center  !important;  background-repeat:no-repeat;background-size: cover !important; width: 100%;height:60px';
            $("#" + destinationid).attr("style", ccsvalue);
        }
    }

    /* Funzione di startup per il carrello */
    function initcalendarrange() {

        /* Funzione di visualizzazione tasti gestione carrello */
        Visualizzatasti(false);

        //Casistica particolare con selezione di periodo calendario per booking
        if (configview == 1 || configview == 3) {
            //console.log('called init initcalendarrange id: ' + controlid);
            //console.log($("#" + controlid + "calendar"));
            $("#" + controlid + "selectdate").show();
            var rangeselected = "";
            $("#" + controlid + "calendar").datepicker({
                //numberOfMonths: 3,
                changeMonth: true,
                changeYear: true,
                minDate: "0", showOtherMonths: true, selectOtherMonths: true, altFormat: "dd", dateFormat: "dd/m/yy", regional: "it",
                beforeShowDay: function (date) {
                    var elem = {};
                    elem.stato = true;
                    elem.Class = '';
                    elem.Tooltip = '';
                    var retvalue = "";
                    //Evidenzio i giorni tra prv e cur per il range select
                    retvalue = [elem.stato, ((date.getTime() >= Math.min(prv, cur) && date.getTime() <= Math.max(prv, cur)) ? elem.Class + ' date-range-selected' : elem.Class + ''), elem.Tooltip];
                    // console.log($.datepicker.formatDate('dd-mm-yy', date));
                    return retvalue;
                },
                onSelect: function (dateText, inst) {
                    Visualizzatasti(false);
                    var d1, d2;
                    prv = cur;
                    cur = (new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay)).getTime();
                    if (prv == -1 || prv == cur) {
                        prv = cur;
                        rangeselected = (dateText);
                        Visualizzatasti(true);

                    } else {
                        rangeselected = '';
                        d1 = moment(new Date(Math.min(prv, cur))).format("DD/MM/YYYY HH:mm:ss");// $.datepicker.formatDate('dd/mm/yy', new Date(Math.min(prv, cur)), {});
                        d2 = moment(new Date(Math.max(prv, cur))).format("DD/MM/YYYY HH:mm:ss");//$.datepicker.formatDate('dd/mm/yy', new Date(Math.max(prv, cur)), {});
                        if (d1 != '' && d2 != '') {
                            rangeselected = (d1 + ' - ' + d2);
                            //Visualizziamo il prezzo per la selezione e controlliamo i vincoli di selezione
                            Visualizzatasti(true);
                        }
                        else {
                            //$('#' + controlid + "info").html('');
                            //$('#' + controlid + "info").attr('class', '');
                            //$('#' + controlid + "messages").html('');
                            //$('#' + controlid + "messages").attr('class', '');
                        }
                    }
                },
                onChangeMonthYear: function (year, month, inst) {
                    // geteventirisorsa(new Date(year, month, 1), 60, renderCalendarjquery);
                    //console.log('cmy:' + inst);
                    //console.log('cmy:' + $.datepicker.formatDate('dd/mm/yy', new Date(year, month, 1), {}));
                },
                onAfterUpdate: function (inst) {
                },
                beforeShow: function () {
                    // console.log('beforeShow');
                }
            });

            $("#" + controlid + "calendar").datepicker($.datepicker.regional["it"]);

            $("#" + controlid).parent().parent().on('click', function (e) {
                $("#" + controlid + "calendar").hide();
            });
            $("#" + controlid + "selectdate").off('click').on('click', function (e) {
                e.stopPropagation();
                $("#" + controlid + "calendar").show();
            });
        }

    }


}


function connectCarrelloEvents() {
    //////////APERTURA DROPDOWN CARRELLO///////////////////////////////////////////
    //Funzione eseguita all'apertura del dropdown con classe triggerdata
    //$('div.btn-group button.triggerdata').click(function (e) {
    //    $(this).dropdown("toggle");to
    //    $(this).parent().find("[id*='ContainerCarrelloDetails']")[0].innerText = "";
    //    var codiceordine = $(this).parent().find("[id*='ContainerCarrelloDetails']").attr("title");
    //    var contenitoredestinazione = $(this).parent().find("[id*='ContainerCarrelloDetails']");
    //    //Caricamento ajax carrello!
    //    ShowCurrentCarrello(contenitoredestinazione, codiceordine);
    //    e.preventDefault();
    //    //Reimposta la funzione che fà apire il dropdown
    //    $(this).click(function (ev) {
    //        $(this).dropdown("toggle");
    //        e.preventDefault();
    //        return false;
    //    });
    //    return false;
    //});

    //Evita che il dropdown si chiuda cliccandodi sopra
    $('.dropdown-menu').click(function (e) {
        e.stopPropagation();
    });
    /////////////INSERIMENTO NEL CARRELLO (INUTILE CON INIEZIONE JS!!!!!)/////////////////////////////////////////////
    $('button.trigcarrello').click(function (e) {
        var title = $(this).attr("title");
        InserisciCarrelloNopostback(title);
        e.preventDefault();
    });
    //////////////////////////////////////////////////////////////////////

    GetCarrelloTotal(); //Visualizza il totale del carrello
}

function GetCarrelloList(el) {
    //$(el).parent().find("[id*='ContainerCarrelloDetails']")[0].innerText = "";
    //var codiceordine = $(el).parent().find("[id*='ContainerCarrelloDetails']").attr("title");
    //var contenitoredestinazione = $(el).parent().find("[id*='ContainerCarrelloDetails']");


    $(el).parent().find("[class*='carrelloelemslist']").each(function (index) {
        // console.log(index + ": " + $(this).text());
        $(this).html("");
    });
    var codiceordine = $(el).parent().find("[class*='carrelloelemslist']").attr("title");
    var contenitoredestinazione = $(el).parent().find("[class*='carrelloelemslist']");

    //Caricamento ajax carrello!
    ShowCurrentCarrello(contenitoredestinazione, codiceordine);
    //}
}


function ShowCurrentCarrello(contenitoredestinazione, codiceordine) {
    $.ajax({
        destinationControl: contenitoredestinazione,
        type: "POST",
        dataType: "text",
        url: pathAbs + carrellohandlerpath + "?Lingua=" + lng + "&Azione=show",
        data: { 'codice': codiceordine, 'Lingua': lng },
        success: function (data) {
            OnSuccessShowcarrello(data, this.destinationControl);
        },
        failure: function (response) {
            alert(response);
        }
    });
}
function OnSuccessShowcarrello(response, destination) {
    // alert(destination[0].id);//Controllo destinazione html
    //destination.append("<li>" + response.d + "</li>");
    // $(destination).Clear();
    destination.append("<li>" + response + "</li>");
}

function GetCarrelloItems(codiceordine, callback) {
    $.ajax({
        url: pathAbs + carrellohandlerpath + "?Lingua=" + lng + "&Azione=getitemscarrello",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        global: false,
        cache: false,
        data: { 'codice': codiceordine, 'Lingua': lng },
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            // callback(result.responseText);
            callback('');
        }
    });
}

function GetCurrentCarrelloQty(contenitoredestinazione, idprodotto, idcombined, idcarrello, forceidcarrello, callback) {

    var forceidcarrello = forceidcarrello || false;
    $.ajax({
        destinationControl: contenitoredestinazione,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        url: pathAbs + carrellohandlerpath + "?Azione=selectrowqty",
        data: { 'idprodotto': idprodotto, 'idcombined': idcombined, 'idcarrello': idcarrello, 'forceidcarrello': forceidcarrello },
        success: function (data) {
            if (callback == null)
                $.find("[id*='" + this.destinationControl + "']")[0].value = data;
            else {
                callback(data);
            }
            //console.log(data);
        },
        failure: function (response) {
            //  alert(response);
        }
    });
}

function SubtractCurrentCarrelloNopostback(contenitoredestinazione, idprodotto, lingua, username, idcombined, idcarrello, prezzo, datastart, dataend, Jsonfield1, mode, forceidcarrello, callback) {
    var mode = mode || '';
    var dataend = dataend || '';
    var datastart = datastart || '';
    var prezzo = prezzo || '';
    var forceidcarrello = forceidcarrello || false;
    $.ajax({
        destinationControl: contenitoredestinazione,
        url: pathAbs + carrellohandlerpath + "?Lingua=" + lingua + "&Azione=subtract" + "&mode=" + mode,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        data: { 'idprodotto': idprodotto, 'idcombined': idcombined, 'idcarrello': idcarrello, 'Lingua': lingua, 'Username': username, 'prezzo': prezzo, 'datastart': datastart, 'dataend': dataend, 'jsonfield1': Jsonfield1, 'forceidcarrello': forceidcarrello },
        success: function (data) {
            OnSuccessAddsub(data, this.destinationControl, callback);
        },
        failure: function (response) {
            //  alert(response);
        }
    });
}

function InserisciCarrelloNopostback(testo) {
    var res = testo.split(",", 3);
    var idprodotto = res[0];
    var lingua = res[1];
    var username = res[2];
    var contenitoredestinazione = '';
    AddCurrentCarrelloNopostback(contenitoredestinazione, idprodotto, lingua, username, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined,
        function (data) {  /*qui in data json ho i valori tornati dalla chiamata che posso iniettare es. tracking .....*/

            var codetoexecute = "";
            if (parsedret != null && parsedret.hasOwnProperty("jscodetoexecute")) {
                codetoexecute = parsedret.jscodetoexecute;
                $('#' + "endspaceforjs").html(codetoexecute);
            }

        });

}

function AddCurrentCarrelloNopostback(contenitoredestinazione, idprodotto, lingua, username, idcombined, idcarrello, prezzo, datastart, dataend, Jsonfield1, mode, forceidcarrello, callback) {
    var mode = mode || '';
    var dataend = dataend || '';
    var datastart = datastart || '';
    var prezzo = prezzo || '';
    var forceidcarrello = forceidcarrello || false;
    $.ajax({
        destinationControl: contenitoredestinazione,
        url: pathAbs + carrellohandlerpath + "?Lingua=" + lingua + "&Azione=add" + "&mode=" + mode,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        data: { 'idprodotto': idprodotto, 'idcombined': idcombined, 'idcarrello': idcarrello, 'Lingua': lingua, 'Username': username, 'prezzo': prezzo, 'datastart': datastart, 'dataend': dataend, 'jsonfield1': Jsonfield1, 'forceidcarrello': forceidcarrello },
        success: function (data) {
            OnSuccessAddsub(data, this.destinationControl, callback);
        },
        failure: function (response) {
            //  alert(response);
        }
    });
}

function OnSuccessAddsub(datain, destination, callback) {
    $.ajax({
        destinationControl: '',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        url: pathAbs + carrellohandlerpath + "?Lingua=" + lng + "&Azione=showtotal",
        data: {},
        success: function (data) {
            if (data != null && data != '') {
                var dataparsed = JSON.parse(data);
                $("[class*='carrellomaincontainer']").find("[class*='carrellototalvalue']").html(dataparsed.totale);
                $("[class*='carrellomaincontainer']").find("[class*='count']").html(dataparsed.pezzi);
                //$("#containerCarrello").find("[id*='litTotalHigh']")[0].innerText = data;
            }
            if (callback != null)
                callback(datain);
        },
        failure: function (response) {
            //  alert(response);
            if (callback != null)
                callback('');
        }
    });
}


function CancellaCurrentCarrello(callback) {
    $.ajax({
        //destinationControl: contenitoredestinazione,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        url: pathAbs + carrellohandlerpath + "?Azione=svuotacarrello",
        data: {},
        success: function (data) {
            OnSuccesscarrelloNopostback('', '', callback);
        },
        failure: function (response) {
            //  alert(response);
            callback(result.responseText);
        }
    });
}

function CancellaCurrentCarrellobyid(idcarrello, callback) {
    var contenitoredestinazione = '';//$(el).parent().find("[class*='carrelloelemslist']");
    $.ajax({
        destinationControl: contenitoredestinazione,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        url: pathAbs + carrellohandlerpath + "?Azione=cancellabyid",
        data: { 'idcarrello': idcarrello },
        success: function (data) {
            OnSuccesscarrelloNopostback('', '', callback);
        },
        failure: function (response) {
            //  alert(response);
            callback(result.responseText);
        }
    });
}


function CancellaOrdineByCodice(codiceordine, callback) {
    var contenitoredestinazione = '';//$(el).parent().find("[class*='carrelloelemslist']");
    $.ajax({
        destinationControl: contenitoredestinazione,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        url: pathAbs + carrellohandlerpath + "?Azione=eliminaordine",
        data: { 'codiceordine': codiceordine },
        success: function (data) {
            OnSuccesscarrelloNopostback('', '', callback(data));
        },
        failure: function (response) {
            //  alert(response);
            callback(result.responseText);
        }
    });
}
function OnSuccesscarrelloNopostback(response, destination, callback) {
    //$(".totalItems").empty();
    //$(".totalItems").append(response);
    //__doPostBack();
    $.ajax({
        destinationControl: '',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        url: pathAbs + carrellohandlerpath + "?Lingua=" + lng + "&Azione=showtotal",
        // contentType: "application/json; charset=utf-8",
        // dataType: "json",
        data: {},
        success: function (data) {
            if (data != null && data != '') {
                var dataparsed = JSON.parse(data);
                $("[class*='carrellomaincontainer']").find("[class*='carrellototalvalue']").html(dataparsed.totale);
                $("[class*='carrellomaincontainer']").find("[class*='count']").html(dataparsed.pezzi);
                //$("#containerCarrello").find("[id*='litTotalHigh']")[0].innerText = data;
            }
            if (callback != null)
                callback(true);
        },
        failure: function (response) {
            //  alert(response);
        }
    });
}


function GetCarrelloTotal(callback) {
    $.ajax({
        destinationControl: '',
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        url: pathAbs + carrellohandlerpath + "?Lingua=" + lng + "&Azione=showtotal",
        data: {},
        success: function (data) {
            if (data != null && data != '') {
                var dataparsed = JSON.parse(data);
                $("[class*='carrellomaincontainer']").find("[class*='carrellototalvalue']").html(dataparsed.totale);
                $("[class*='carrellomaincontainer']").find("[class*='count']").html(dataparsed.pezzi);
                //$("#containerCarrello").find("[id*='litTotalHigh']")[0].innerText = data;
            }
            if (callback != null)
                callback(data);
        },
        failure: function (response) {
            //  alert(response);
            if (callback != null)
                callback('');
        }
    });
}

function GetCarrelloTotalForItem(idprodotto, idcombined, idcarrello, callback) {
    $.ajax({
        url: pathAbs + carrellohandlerpath,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        global: false,
        cache: false,
        data: { 'Azione': 'showtotalforproduct', 'idprodotto': idprodotto, 'idcombined': idcombined, 'idcarrello': idcarrello, 'Lingua': lng },
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            //callback(result.responseText);
            callback('');
        }
    });
}


function GetPriceForItem(idprodotto, callback) {
    $.ajax({
        url: pathAbs + carrellohandlerpath,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        global: false,
        cache: false,
        data: { 'Azione': 'getpriceforproduct', 'idprodotto': idprodotto },
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            //callback(result.responseText);
            callback('');
        }
    });
}

function GetCarcombinedForItem(idprodotto, callback) {
    $.ajax({
        url: pathAbs + carrellohandlerpath,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "text",
        global: false,
        cache: false,
        data: { 'Azione': 'getxmlvalueforproduct', 'idprodotto': idprodotto, 'Lingua': lng },
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            //callback(result.responseText);
            callback('');
        }
    });
}





