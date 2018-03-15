
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
        modifyscopevalues(pidrisorsa, pidcombined, pidcarrello, pusername, idcontrollo, cfgvista) {
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
        inserisciacarrelloquantita() { //inserisce nel carrello una quantità esatta per l'articolo calcolata con la differnza dei giorni tramite le date
            var start = new Date(Math.min(prv, cur));
            var end = new Date(Math.max(prv, cur));
            // console.log("inserisci carrello " + idprodotto + " " + start + " " + end);
            var Enddate = moment(end).set({ hour: 0, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
            var Startdate = moment(start).set({ hour: 0, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
            var quantita = moment(end).diff(moment(start), 'days');

            //VERSIONE CHE PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            AddCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, '', Startdate, Enddate, '', quantita + 1, true,
                function (ret) {
                    /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/
                    idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                    /***********************************************************************************************************************************************/
                    openLink('/AspnetPages/Shoppingcart.aspx?Lingua=' + lng);//da verificare se redirect da funzione è ok
                });

            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            //AddCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, '0', Startdate, Enddate, '', quantita + 1, false,
            //    function (ret) {
            //       openLink('/AspnetPages/Shoppingcart.aspx?Lingua=' + lng);//da verificare se redirect da funzione è ok
            //    });

            return;
        },
        aggiungiacarrello() {
            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            AddCurrentCarrelloNopostback('', idprodotto, lng, username, '', '', '', null, null, '', '', false, function (ret) {
                /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/
                // idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                /***********************************************************************************************************************************************/
                carrellotool.caricaquantita();
            });
            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            //AddCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, '', null, null, '', '', true, function (ret) {
            //    /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO **********************************************************************/
            //    idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
            //    /***********************************************************************************************************************************************/
            //    carrellotool.caricaquantita();
            //});
            return;
        },
        sottradiacarrello() {
            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            SubtractCurrentCarrelloNopostback('', idprodotto, lng, username, '', '', '', null, null, '', '', false, function (ret) {
                /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO e  modificare parametro ************************************************************/
                // idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
                /***********************************************************************************************************************************************/
                carrellotool.caricaquantita();
            });
             //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            //SubtractCurrentCarrelloNopostback('', idprodotto, lng, username, '', idcarrello, '', null, null, '', '', true, function (ret) {
            //    /************COMMENTARE LA RIGA PER OPERATIVITA' NORMALE SINGOLO RIGO CARRELLO PER PRODOTTO e  modificare parametro ************************************************************/
            //    idcarrello = ret; //SERVE NEL CASO IMPOSTAZIONE CON FORCEIDCARRELLO IN MODO CHE L'AGGIORNAMENTO/INSERIMENTO SIA SOLO PER IDCARRELLO e non PRODOTTO ( in modo da consentire inserimenti multipli )
            //    /***********************************************************************************************************************************************/
            //    carrellotool.caricaquantita();
            //});
            return;
        },
        caricaquantita() {
            //VERSIONE CHE NON PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            GetCurrentCarrelloQty('', idprodotto, '', idcarrello,false, function (ret) {
                var casellaqty = "<input style =\"width:40px;margin-top:10px;text-align:center\" class=\"form-control\" id='" + controlid + "qtyi' value='" + ret + "' />";
                $('#' + controlid + "qty").html(casellaqty);
            });
             //VERSIONE CHE PERMETTE DI INSERIRE PIù RIGHICARRELLO CON STESSO PRODOTTO
            //GetCurrentCarrelloQty('', idprodotto, '', idcarrello, true, function (ret) {
            //    var casellaqty = "<input style =\"width:40px;margin-top:10px;text-align:center\" class=\"form-control\" id='" + controlid + "qtyi' value='" + ret + "' />";
            //    $('#' + controlid + "qty").html(casellaqty);
            //});
        },
        caricatotale() { //Carica il totale a carrello attuale per l'elemento passato  
            GetCarrelloTotalForItem(idprodotto, idcombined, idcarrello, function (ret) {
                $('#' + controlid + "price").val(ret);
            });
            return;
        },
        calcolatotale() {
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
    }


    function Visualizzatasti(abilita) {
        var abilita = abilita || false;

        if (configview == 1 || configview == 3) {
            $('#' + controlid + "messages").html('');
            var onclickevent = "style=\"width:160px;cursor:pointer;margin-top:10px\" onclick =\"carrellotool.inserisciacarrelloquantita()\"";
            if (!abilita) onclickevent = "style=\"width:160px;cursor:pointer;margin-top:10px\"";
            var btninserisci = "<div class=\"divbuttonstyle\"  " + onclickevent + ">" + GetResourcesValue("testoinseriscicarrello") + "</div>";
            $('#' + controlid + "messages").append(btninserisci);
            carrellotool.calcolatotale();
        }
        if (configview == 2 || configview == 3) {
            $('#' + controlid + "plus").html('');
            $('#' + controlid + "minus").html('');
            var onclickevent1 = "style=\"width:60px;cursor:pointer;margin-top:10px\" onclick =\"carrellotool.aggiungiacarrello()\"";
            // if (!abilita) onclickevent1 = "style=\"width:60px;cursor:pointer;margin-top:10px\"";
            var btnaggiungi = "<div class=\"divbuttonstyle\"  " + onclickevent1 + ">+</div>";
            $('#' + controlid + "plus").append(btnaggiungi);

            var onclickevent2 = "style=\"width:60px;cursor:pointer;margin-top:10px\" onclick =\"carrellotool.sottradiacarrello()\"";
            // if (!abilita) onclickevent1 = "style=\"width:60px;cursor:pointer;margin-top:10px\"";
            var btnsottrai = "<div class=\"divbuttonstyle\"  " + onclickevent2 + ">-</div>";
            $('#' + controlid + "minus").append(btnsottrai);

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
}

function GetCarrelloList(el) {
    $(el).parent().find("[id*='ContainerCarrelloDetails']")[0].innerText = "";
    //if ($(el).parent().find("[id*='ContainerCarrelloDetails']")[0].innerText == "") {
    var codiceordine = $(el).parent().find("[id*='ContainerCarrelloDetails']").attr("title");
    var contenitoredestinazione = $(el).parent().find("[id*='ContainerCarrelloDetails']");
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
            $("#containerCarrello").find("[id*='litTotalHigh']")[0].innerText = data;
            //$("#containerCarrelloMobile").find("[id*='litTotalHigh']")[0].innerText = data; //????
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
            $("#containerCarrello").find("[id*='litTotalHigh']")[0].innerText = data;
            //$("#containerCarrelloMobile").find("[id*='litTotalHigh']")[0].innerText = data; //????
            if (callback != null)
                callback(true);
        },
        failure: function (response) {
            //  alert(response);
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




