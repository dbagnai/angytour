
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

            (function wait() { //Aspettiamo che il controllo sia iniettato
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
            //AddCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, '0', Startdate, Enddate, '', quantita + 1, false,
            //    function (data) {
            //       openLink('/AspnetPages/Shoppingcart.aspx?Lingua=' + lng);//da verificare se redirect da funzione è ok
            //    });

            return;
        },
        aggiungiacarrello: function () {
            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            AddCurrentCarrelloNopostback('', idprodotto, lng, username, idcombined, '', '', null, null, '', '', false, function (data) {

                var ret = "";
                var parsedret = "";
                if (data != null && data != "")
                    parsedret = JSON.parse(data);
                if (parsedret != null && parsedret.hasOwnProperty("id"))
                    ret = parsedret.id;

                $('#' + controlid + "messages").html(parsedret.stato);

                /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/
                // idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                /***********************************************************************************************************************************************/
                carrellotool.caricaquantita();
            });
            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            //AddCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, '', null, null, '', '', true, function (data) {
            //var ret = "";
            //var parsedret = "";
            //if (data != null && data != "")
            //    parsedret = JSON.parse(data);
            //if (parsedret != null && parsedret.hasOwnProperty("id"))
            //    ret = parsedret.id;
            //    /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/

            // idcarrello = ret;   //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
            //    /***********************************************************************************************************************************************/
            //    carrellotool.caricaquantita();
            //});
            return;
        },
        sottradiacarrello: function () {
            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            SubtractCurrentCarrelloNopostback('', idprodotto, lng, username, idcombined, '', '', null, null, '', '', false, function (data) {

                var ret = "";
                var parsedret = "";
                if (data != null && data != "")
                    parsedret = JSON.parse(data);
                if (parsedret != null && parsedret.hasOwnProperty("id"))
                    ret = parsedret.id;

                $('#' + controlid + "messages").html(parsedret.stato);
                /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO e  modificare parametro ************************************************************/
                // idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                /***********************************************************************************************************************************************/
                carrellotool.caricaquantita();
            });
            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            //SubtractCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, '', null, null, '', '', true, function (data) {
            //var ret = "";
            //var parsedret = "";
            //if (data != null && data != "")
            //    parsedret = JSON.parse(data);
            //if (parsedret != null && parsedret.hasOwnProperty("id"))
            //    ret = parsedret.id;
            //    /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO e  modificare parametro ************************************************************/
            //    idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
            //    /***********************************************************************************************************************************************/
            //    carrellotool.caricaquantita();
            //});
            return;
        },
        caricaquantita: function () {

            //Testiamo se presenti le caselle di specifica delle caratteristiche - devono esserci entrambe e valorizzate per funzinonare
            if ($('#' + controlid + "Caratteristica1").length > 0 && $('#' + controlid + "Caratteristica2").length > 0) {
                if ($('#' + controlid + "Caratteristica1")[0].value != '' && $('#' + controlid + "Caratteristica2")[0].value != '')
                    idcombined = $('#' + controlid + "Caratteristica1")[0].value + "-" + $('#' + controlid + "Caratteristica2")[0].value;
                else
                    idcombined = ""; //svuoto se anche uno solo è vuoto !!!
                //debugger;
            }


            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            GetCurrentCarrelloQty('', idprodotto, idcombined, idcarrello, false, function (ret) {
                var casellaqty = "<input style =\"width:40px;margin-top:10px;text-align:center\" class=\"form-control\" id='" + controlid + "qtyi' value='" + ret + "' />";
                $('#' + controlid + "qty").html(casellaqty);
            });
            //VERSIONE CHE PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            //GetCurrentCarrelloQty('', idprodotto, idcombined, idcarrello, true, function (ret) {
            //    var casellaqty = "<input style =\"width:40px;margin-top:10px;text-align:center\" class=\"form-control\" id='" + controlid + "qtyi' value='" + ret + "' />";
            //    $('#' + controlid + "qty").html(casellaqty);
            //});
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


    function Visualizzatasti(abilita) {
        var abilita = abilita || false;

        if (configview == 1 || configview == 3) {
            $('#' + controlid + "messages").html('');
            var onclickevent = "style=\"width:160px;cursor:pointer;margin-top:10px\" onclick =\"carrellotool.inserisciacarrelloquantita()\"";
            if (!abilita) onclickevent = "style=\"width:160px;cursor:pointer;margin-top:10px\"";
            var btninserisci = "<div class=\"divbuttonstyle\"  " + onclickevent + ">" + GetResourcesValue("testoinseriscicarrellostd") + "</div>";
            $('#' + controlid + "messages").append(btninserisci);
            carrellotool.calcolatotale();
        }
        if (configview == 2 || configview == 3) {
            $('#' + controlid + "addsingle").html('');
            $('#' + controlid + "plus").html('');
            $('#' + controlid + "minus").html('');
            var onclickevent1 = "style=\"cursor:pointer;margin-top:10px\" onclick =\"carrellotool.aggiungiacarrello()\"";
            // if (!abilita) onclickevent1 = "style=\"width:60px;cursor:pointer;margin-top:10px\"";
            var btnaggiungi = "<div class=\"button-carrello\" style=\"padding-left: 2px !important;font-size: 1.4rem;\" " + onclickevent1 + ">+</div>";
            $('#' + controlid + "plus").append(btnaggiungi);

            var btnaggiungisingle = "<div class=\"divbuttonstyle\"  onclick =\"carrellotool.aggiungiacarrello()\">" + GetResourcesValue("testoinseriscicarrellostd") + "</div>";
            $('#' + controlid + "addsingle").append(btnaggiungisingle);

            var onclickevent2 = "style=\"cursor:pointer;margin-top:10px\" onclick =\"carrellotool.sottradiacarrello()\"";
            // if (!abilita) onclickevent1 = "style=\"width:60px;cursor:pointer;margin-top:10px\"";
            var btnsottrai = "<div class=\"button-carrello\" style=\"padding-left: 2px !important;font-size: 2rem;\" " + onclickevent2 + ">-</div>";
            $('#' + controlid + "minus").append(btnsottrai);

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
                        $(this).change(carrellotool.caricaquantita);
                        var proprarr = idcontrollo.replace(controlid, "");
                        //debugger;
                        var filter = "";//$(this).attr("myfilter");
                        var selectedvalueact = "";
                        /*Se passo il valore corretto -> presetto i valori idcombined*/
                        //if (objfiltroint != null && objfiltroint.hasOwnProperty(idcontrollo))
                        //    selectedvalueact = objfiltroint[idcontrollo];

                        var selstring = '';
                        if (baseresources != null && baseresources.hasOwnProperty(lng) && baseresources[lng].hasOwnProperty("select" + proprarr.toLowerCase()))
                            selstring = baseresources[lng]["select" + proprarr.toLowerCase()];

                        if (dataparsed != null && dataparsed.hasOwnProperty(proprarr) && dataparsed[proprarr] != null && dataparsed[proprarr] != '') {
                            var parseddatas = JSON.parse(dataparsed[proprarr]);
                            //Se la box contiene solo un valore lo presetto
                            if (parseddatas.length == 1)
                                selectedvalueact = parseddatas[0].Codice;
                            convertToDictionaryandFill(parseddatas, 0, lng, idcontrollo, selstring, '', selectedvalueact, filter);
                            carrellotool.caricaquantita();
                        }
                        else $(this).hide();

                    });
                }
                else $('#' + controlid + "cars").remove();
            });
            ///////////////////////////////////////////////////////////////////////////////////////////

            carrellotool.caricaquantita();
        }

    }


    function initcalendarrange() {
        Visualizzatasti(false);
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
    AddCurrentCarrelloNopostback(contenitoredestinazione, idprodotto, lingua, username);
    //AddCurrentCarrelloNopostback(contenitoredestinazione, idprodotto, lingua, username, '', '', '', null, null, '', '', false, function (data) {
    //    carrellotool.caricaquantita();
    //});

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





