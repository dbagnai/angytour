"use strict";
$(document).ready(function () {
});


var bookingtool = new function () {

    var pathfiles = "/js/jqwidgets/jqwidgets/";
    var loadedresources = 0;
    var idpassed = "";
    var controlid = "";
    var jqstartadates = new Array();
    var jqenddates = new Array();
    var jqcompletedate = new Array();
    var jqrestricteddates = new Array();
    var prv = -1, cur = -1;

    //this.initbookingtool = function () { //Funzione visibile all'esterno
    //}; 

    this.start = function (msg) { console.log('start book called ') };

    return {
        initbookingtool: function (idrisorsa, idcontrollo) {
            idpassed = idrisorsa;
            controlid = idcontrollo;

            (function wait() {
                if (typeof loadJs === "function") {
                    loadJs(pathfiles + "jqxcore.js").then(function () {
                        loadedresources += 1;
                        (function wait() {
                            if (typeof loadJs === "function") {
                                loadCss(pathfiles + "styles/jqx.base.css").then(function () { loadedresources += 1; });
                            } else {
                                setTimeout(wait, 50);
                            }
                        })();
                        (function wait() {
                            if (typeof loadJs === "function") {
                                loadJs(pathfiles + "jqxdatetimeinput.js").then(function () { loadedresources += 1; });
                            } else {
                                setTimeout(wait, 50);
                            }
                        })();
                        (function wait() {
                            if (typeof loadJs === "function") {
                                loadJs(pathfiles + "jqxdata.js").then(function () { loadedresources += 1; });
                            } else {
                                setTimeout(wait, 50);
                            }
                        })();
                        (function wait() {
                            if (typeof loadJs === "function") {
                                loadJs(pathfiles + "jqxdate.js").then(function () { loadedresources += 1; });
                            } else {
                                setTimeout(wait, 50);
                            }
                        })();
                        (function wait() {
                            if (typeof loadJs === "function") {
                                loadJs(pathfiles + "jqxcalendar.js").then(function () { loadedresources += 1; });
                            } else {
                                setTimeout(wait, 50);
                            }
                        })();
                        (function wait() {
                            if (typeof loadJs === "function") {
                                loadJs(pathfiles + "jqxtooltip.js").then(function () { loadedresources += 1; });
                            } else {
                                setTimeout(wait, 50);
                            }
                        })();
                        (function wait() {
                            if (typeof loadJs === "function") {
                                loadJs(pathfiles + "globalization/globalize.js").then(function () {
                                    loadedresources += 1;

                                    (function wait() {
                                        if (typeof loadJs === "function") {
                                            loadJs(pathfiles + "globalization/globalize.culture.it-IT.js").then(function () { loadedresources += 1; });
                                        } else {
                                            setTimeout(wait, 50);
                                        }
                                    })();

                                });
                            } else {
                                setTimeout(wait, 50);
                            }
                        })();

                    });
                } else {
                    setTimeout(wait, 50);
                }
            })();


            (function wait1() { //Aspettiamo che il controllo sia iniettato
                if ($("#" + controlid).length) {
                    insertbookingTool();
                } else {
                    setTimeout(wait1, 80);
                }
            })();


            return;
        },
        getidpassed: function (parametro) {  //altr funzione chiamabile dall'esterno che torna la somma dei due valori (test)
            return parametro + idpassed;
        }, calcolaPrezzo: function () {
            Visualizzatasti(false);

            $('#' + controlid + "info").html('');
            $('#' + controlid + "info").attr('class', '');
            //$('#' + controlid + "messages").html('');
            //$('#' + controlid + "messages").attr('class', '');

            var str = new Date(Math.min(prv, cur));
            var end = new Date(Math.max(prv, cur));

            /*+++++++++++++++++++++++++++++++++++++++++VERIFICHE VONOLI DI PRENOTAZIONE*/
            //VINCOLO SOLO SETTIMANE INTERE
            var datediffdays = moment(end).diff(moment(str), 'days');
            var remainder = datediffdays % 7;
            if (remainder != 0) {
                $('#' + controlid + "info").html('');
                $('#' + controlid + "info").attr('class', 'alert alert-danger');
                $('#' + controlid + "info").html(GetResourcesValue("testoprenotam7"));
                return;
            }
            //VINCOLO SOLO SABATO SABATO
            if (moment(end).isoWeekday() != 6 || moment(str).isoWeekday() != 6) {
                $('#' + controlid + "info").html('');
                $('#' + controlid + "info").attr('class', 'alert alert-danger');
                $('#' + controlid + "info").html(GetResourcesValue("testoprenotaday"));
                return;
            }
            /*++++++++++++++++++++++++++++++++++++++++VERIFICHE VONOLI DI PRENOTAZIONE*/

            //Ultima verifica sulla disponibilità prima di inserire in carrello e procedere con la prenotazione
            var tmpelement = globalObject[controlid + "evento"];
            tmpelement.Enddate = moment(str).set({ hour: 10, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
            tmpelement.Startdate = moment(end).set({ hour: 16, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
            tmpelement.Idevento = 0;
            tmpelement.Idattivita = idpassed;
            tmpelement.Prezzo = 0; //Prezzo da calcolare
            tmpelement.Soggetto = '';
            tmpelement.Testoevento = '';
            tmpelement.Idcliente = '0';
            tmpelement.Idvincolo = '';
            tmpelement.Codicerichiesta = '';
            tmpelement.Status = 1;

            calcolaprezzobyidattivita(moment(str).format("DD/MM/YYYY"), moment(end).format("DD/MM/YYYY"), idpassed, function (result) {
                var prezzodalistino = Number(result.trim());
                if (!isNaN(prezzodalistino) && prezzodalistino != 0) {
                    tmpelement.Prezzo = Number(result.trim());
                    var prezzomsg = "<div>" + GetResourcesValue("prezzo") + " " + prezzodalistino.formatMoney() + "€</div>";
                    $('#' + controlid + "info").html(prezzomsg);
                    $('#' + controlid + "info").attr('class', 'alert alert-success');


                    Visualizzatasti(true);

                }
                else {
                    $('#' + controlid + "info").html('');
                    $('#' + controlid + "info").attr('class', 'alert alert-danger');
                    $('#' + controlid + "info").html(GetResourcesValue("testoprenotaerr1"));
                    return;
                }
            });

            return;
        },
        InserisciPrenotazione: function () {  //altr funzione chiamabile dall'esterno che torna la somma dei due valori (test)
            $('#' + controlid + "info").html('');
            $('#' + controlid + "info").attr('class', '');
            //$('#' + controlid + "messages").html('');
            //$('#' + controlid + "messages").attr('class', '');

            var str = new Date(Math.min(prv, cur));
            var end = new Date(Math.max(prv, cur));

            /*+++++++++++++++++++++++++++++++++++++++++VERIFICHE VINCOLI DI PRENOTAZIONE*/
            //VINCOLO SOLO SETTIMANE INTERE
            var datediffdays = moment(end).diff(moment(str), 'days');
            var remainder = datediffdays % 7;
            if (remainder != 0) {
                $('#' + controlid + "info").html('');
                $('#' + controlid + "info").attr('class', 'alert alert-danger');
                $('#' + controlid + "info").html(GetResourcesValue("testoprenotam7"));
                return;
            }
            //VINCOLO SOLO SABATO SABATO
            if (moment(end).isoWeekday() != 6 || moment(str).isoWeekday() != 6) {
                $('#' + controlid + "info").html('');
                $('#' + controlid + "info").attr('class', 'alert alert-danger');
                $('#' + controlid + "info").html(GetResourcesValue("testoprenotaday"));
                return;
            }
            /*++++++++++++++++++++++++++++++++++++++++VERIFICHE VINCOLI DI PRENOTAZIONE*/

            //Ultima verifica sulla disponibilità prima di inserire in carrello e procedere con la prenotazione
            var tmpelement = globalObject[controlid + "evento"];
            tmpelement.Enddate = moment(end).set({ hour: 10, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
            tmpelement.Startdate = moment(str).set({ hour: 16, minute: 0, second: 0, millisecond: 0 }).format("DD/MM/YYYY HH:mm:ss");
            tmpelement.Idevento = 0;
            tmpelement.Idattivita = idpassed;
            tmpelement.Prezzo = 0; //Prezzo da calcolare
            tmpelement.Soggetto = '';
            tmpelement.Testoevento = '';
            tmpelement.Idcliente = '0';
            tmpelement.Idvincolo = '';
            tmpelement.Codicerichiesta = '';
            tmpelement.Status = 1;
            //Aggiungiamo gli adulti bambini !!
            var adulti = $("#" + controlid + "selectadult" + " option:selected").val();
            var bambini = $("#" + controlid + "selectchild" + " option:selected").val();
            var jsondetails = {};
            jsondetails.adulti = adulti;
            jsondetails.bambini = bambini;
            tmpelement.Jsonfield1 = JSON.stringify(jsondetails);

            calcolaprezzobyidattivita(moment(str).format("DD/MM/YYYY"), moment(end).format("DD/MM/YYYY"), idpassed, function (result) {
                var prezzodalistino = Number(result.trim());
                if (!isNaN(prezzodalistino) && prezzodalistino != 0) {
                    tmpelement.Prezzo = Number(result.trim());
                    var prezzomsg = "<div>" + GetResourcesValue("prezzo") + " " + prezzodalistino.formatMoney() + "€</div>";
                    $('#' + controlid + "info").attr('class', 'alert alert-success');
                    $('#' + controlid + "info").html(prezzomsg);

                    var functiontocallonend = function () { };
                    verificaDatiServerEvento(lng, tmpelement,
                        function (result, callafterfilter) {
                            try {
                                if (callafterfilter != null) {
                                    $('#' + controlid + "messages2").attr('class', '');
                                    $('#' + controlid + "messages2").html('');

                                    //////////////////
                                    // funzione per inserire l'elemento a carrello tipo  in carrello.js   ( con valori date , prezzo e single=true ) presi da tmpelement!!!!!
                                    //////////////////
                                    AddCurrentCarrelloNopostback('', idpassed, lng, '', '', '', tmpelement.Prezzo, tmpelement.Startdate, tmpelement.Enddate, tmpelement.Jsonfield1, 'single', false,
                                        function (result) {
                                            //Visualizzatasti(true);
                                            openLink('/AspnetPages/Shoppingcart.aspx?Lingua=' + lng);//da verificare se redirect da funzione è ok
                                            callafterfilter();
                                        });

                                }
                                else {
                                    $('#' + controlid + "info").attr('class', 'alert alert-danger');
                                    $('#' + controlid + "info").html(result);
                                }
                            }
                            catch (e) { $('#' + controlid + "info").attr('class', 'alert alert-danger'); $('#' + controlid + "info").html(e); console.log(e); console.log(result); }
                        },
                        functiontocallonend);
                }
                else {
                    $('#' + controlid + "info").html('');
                    $('#' + controlid + "info").attr('class', 'alert alert-danger');
                    $('#' + controlid + "info").html(GetResourcesValue("testoprenotaerr1"));
                    return;
                }
            });

            return;
        },
        SvuotaCarrello: function () {
            CancellaCarrello();
            return;
        }
    }


    function Visualizzatasti(abilita) {

        var abilita = abilita || false;

        $('#' + controlid + "messages2").attr('class', '');
        $('#' + controlid + "messages2").html('');

        $('#' + controlid + "messages").attr('class', '');
        $('#' + controlid + "messages").html('');

        var btncarrello = "<div class=\"divbuttonstyle\" style=\"width:160px;cursor:pointer\" onclick=\"openLink('/AspnetPages/Shoppingcart.aspx')\">" + GetResourcesValue("gotoshoppingcart") + "</div>";
        $('#' + controlid + "messages2").append(btncarrello);

        var btncancella = "<div class=\"divbuttonstylered\" style=\"width:160px;cursor:pointer;margin-top:10px\" onclick=\"bookingtool.SvuotaCarrello()\">" + GetResourcesValue("testoordineannulla") + "</div>";
        $('#' + controlid + "messages2").append(btncancella);


        var onclickevent = "style=\"width:160px;cursor:pointer;margin:0px auto;display:table\" onclick =\"bookingtool.InserisciPrenotazione()\"";
        if (!abilita) onclickevent = "style=\"width:160px;margin:0px auto;display:table\"";
        var btnprenota = "<div class=\"divbuttonstyleorange\"  " + onclickevent + " >" + GetResourcesValue("testoprenota") + "</div>";
        $('#' + controlid + "messages").append(btnprenota);


    }

    function insertbookingTool() {

        //Function to pool a variable ad continue if defined!!!
        (function waitloadedresources() {
            if (loadedresources == 9) {
                //Proceed and make the call

                geteventirisorsa('', 60, renderCalendarjquery)  //Carico gli eventi e bloco le date prenotate a calendario creando la lista --> restricteddate ( per ogni evento caricato devo sempre togliere il primo e l'ultimo giornodalle date bloccate )
                Visualizzatasti(false);
                //var restrictedDates = new Array();
                //var date1 = new Date(2018, 1, 22);
                //restrictedDates.push(date1);

                //$.getScript('/js/jqwidgets/jqwidgets/globalization/globalize.culture.it-IT.js', function () {
                //    $("#" + controlid).jqxCalendar({
                //        theme: 'light', culture: 'it-IT', width: 220, height: 220, selectionMode: 'range', restrictedDates: restrictedDates
                //    });
                //});

            } else {
                setTimeout(waitloadedresources, 80);
            }
        })();
    }

    function fillcalendarlists() {

        jqstartadates = new Array();
        jqenddates = new Array();
        jqrestricteddates = new Array();
        jqcompletedate = new Array();

        var datelimit = new Date(2001, 2, 1);
        for (var i = 0; i < globalObject[controlid + "eventi"].length; i++) {
            var evento = globalObject[controlid + "eventi"][i];
            var start = new Date(evento.Startdate);
            start = new Date(start.setHours(0)); //azzero l'ora
            start = new Date(start.setMinutes(0)); //azzero minuti
            start = new Date(start.setSeconds(0)); //azzero secondi
            if (start < datelimit) continue;
            var end = new Date(evento.Enddate);
            end = new Date(end.setHours(0)); //azzero l'ora
            end = new Date(end.setMinutes(0)); //azzero minuti
            end = new Date(end.setSeconds(0)); //azzero secondi

            //Prepariamo gli elemneti per le due silte specialdays e restricteddays
            var startsd = new Date(start);
            var endsd = new Date(end);
            var startrd = new Date(start);
            startrd.setDate(startrd.getDate() + 1);
            var endrd = new Date(end);
            for (var day = startrd; day < endrd; day.setDate(day.getDate() + 1)) {
                jqrestricteddates.push(day.getTime());
            }

            //INserisco il primo e ultimo giorno di permanenza come special dates selezionabili ( se non già presenti nei giorni riservati ! )
            var s = jqrestricteddates.indexOf(startsd.getTime());
            if (s == -1) {
                jqstartadates.push(startsd.getTime());
                jqcompletedate.push(startsd.getTime());

                //new Date(timestamp)  back
                //var elem = {};
                //elem.Date = startsd.getTime();
                //elem.Class = 'jqx-fill-halfcolorbckstart';
                //elem.Tooltip = 'Start period';
                //specialDatesjson.push(elem);
            }

            var e = jqrestricteddates.indexOf(endsd.getTime());
            if (e == -1) {
                jqenddates.push(endsd.getTime());
                jqcompletedate.push(endsd.getTime());
                //var elem = {};
                //elem.Date = endsd.getTime();
                //elem.Class = 'jqx-fill-halfcolorbckend';
                //elem.Tooltip = 'End period';
                //specialDatesjson.push(elem);
            }
        }
        /*Correggo le liste delle start ed end date in base alla lista resticted*/
        for (var i = 0; i < jqcompletedate.length; i++) {
            if (jqrestricteddates.indexOf(jqcompletedate[i]) == -1) { //non presente nelle date vietate
                //Se in entrambe le liste la metto come vietata
                if ($.inArray(jqcompletedate[i], jqenddates) != -1 && $.inArray(jqcompletedate[i], jqstartadates) != -1) {
                    //rimuoviamo dalle liste
                    jqenddates.splice($.inArray(jqcompletedate[i], jqenddates), 1);
                    jqstartadates.splice($.inArray(jqcompletedate[i], jqstartadates), 1);
                    //Aggiungiamo ai ristretti
                    jqrestricteddates.push(jqcompletedate[i]);
                }
            }
            else {
                // rimuoviamo dalle liste start ed aend date
                var index = $.inArray(jqcompletedate[i], jqenddates);
                if (index != -1)
                    jqenddates.splice(index, 1);
                index = $.inArray(jqcompletedate[i], jqstartadates);
                if (index != -1)
                    jqstartadates.splice(index, 1);
            }
        }
    }


    function renderCalendarjquery(datastartview) {

        var rangeselected = "";
        fillcalendarlists();
        //$.when(fillcalendarlists).then(function () {
        //});

        //Forzare avanti e indietro del datepicker
        //$('.ui-datepicker-' + 'prev').trigger("click");
        //$('.ui-datepicker-' + 'nexr').trigger("click");

        $("#" + controlid).datepicker({
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
                if (jqrestricteddates.indexOf(date.getTime()) != -1) {
                    elem.stato = false;
                    elem.Class = 'ui-state-reserved';
                    elem.Tooltip = 'reserved';
                    //retvalue = [false, 'ui-state-reserved', "Reserved"];
                }
                else if (jqstartadates.indexOf(date.getTime()) != -1) {
                    elem.stato = true;
                    elem.Class = 'ui-state-halfcolorbckstart';
                    elem.Tooltip = '';
                    //retvalue = [true, 'ui-state-halfcolorbckstart', ""];
                }
                else if (jqenddates.indexOf(date.getTime()) != -1) {
                    elem.stato = true;
                    elem.Class = 'ui-state-halfcolorbckend';
                    elem.Tooltip = '';
                    //retvalue = [true, 'ui-state-halfcolorbckend', ""];
                }

                //Evidenzio i giorni tra prv e cur per il range select
                //retvalue = [true, ((date.getTime() >= Math.min(prv, cur) && date.getTime() <= Math.max(prv, cur)) ? 'date-range-selected' : '')];

                //Evidenzio i giorni tra prv e cur per il range select
                retvalue = [elem.stato, ((date.getTime() >= Math.min(prv, cur) && date.getTime() <= Math.max(prv, cur)) ? elem.Class + ' date-range-selected' : elem.Class + ''), elem.Tooltip];

                return retvalue;
            },
            onSelect: function (dateText, inst) {
                var d1, d2;
                prv = cur;
                cur = (new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay)).getTime();
                if (prv == -1 || prv == cur) {
                    prv = cur;
                    rangeselected = (dateText);

                    $('#' + controlid + "info").html('');
                    $('#' + controlid + "info").attr('class', '');
                    //$('#' + controlid + "messages").html('');
                    //$('#' + controlid + "messages").attr('class', '');
                } else {
                    rangeselected = '';
                    d1 = moment(new Date(Math.min(prv, cur))).format("DD/MM/YYYY HH:mm:ss");// $.datepicker.formatDate('dd/mm/yy', new Date(Math.min(prv, cur)), {});
                    d2 = moment(new Date(Math.max(prv, cur))).format("DD/MM/YYYY HH:mm:ss");//$.datepicker.formatDate('dd/mm/yy', new Date(Math.max(prv, cur)), {});

                    //controlliamo di non esserea cavallo di giorni riservati -> ne qual caso annullo la selezione
                    //stessa cosa per periodi a cavallo di singole notti
                    var str = new Date(Math.min(prv, cur));
                    var end = new Date(Math.max(prv, cur));
                    for (var day = str; day <= end; day.setDate(day.getDate() + 1)) {
                        //Verifica sui riservati
                        if (jqrestricteddates.indexOf(day.getTime()) != -1) {
                            prv = -1, cur = -1;
                            d1 = ''; d2 = '';
                            break;
                        }
                        //verifichiamo anche che non ho start . end consecutivi su due gioni interni alla selezione
                        var dayp = new Date(day);
                        dayp.setDate(dayp.getDate() + 1);
                        if (jqstartadates.indexOf(day.getTime()) != -1 && jqenddates.indexOf(dayp.getTime()) != -1 && day < end) {
                            prv = -1, cur = -1;
                            d1 = ''; d2 = '';
                            break;
                        }
                    }
                    if (d1 != '' && d2 != '') {
                        rangeselected = (d1 + ' - ' + d2);
                        //Visualizziamo il prezzo per la selezione e controlliamo i vincoli di selezione
                        bookingtool.calcolaPrezzo();
                    }
                    else {
                        $('#' + controlid + "info").html('');
                        $('#' + controlid + "info").attr('class', '');
                        //$('#' + controlid + "messages").html('');
                        //$('#' + controlid + "messages").attr('class', '');
                    }
                }
                console.log(rangeselected);
            },
            onChangeMonthYear: function (year, month, inst) {
                geteventirisorsa(new Date(year, month, 1), 60, renderCalendarjquery);
            },
            onAfterUpdate: function (inst) {
            },
            beforeShow: function () {

            }
        });

        $("#" + controlid).datepicker($.datepicker.regional["it"]);


        //$(window).on('click', function () {
        //    $("#" + controlid).hide();
        //});
        $("#" + controlid + "container").on('click', function (e) {
            $("#" + controlid).hide();
        });
        $("#" + controlid + "selectdate").off('click').on('click', function (e) {
            e.stopPropagation();
            $("#" + controlid).show();
        });
        //$("#" + controlid).datepicker("refresh");
        //$("#" + controlid).datepicker("setDate", $("#" + controlid).datepicker("getDate").toString("yyyy/MM/dd"));
    }

    function geteventirisorsa(pdatestart, days, functocall) {


        var objfiltro = {};
        objfiltro["idreslist"] = idpassed;
        objfiltro["status"] = 1; //Prendo solo eventi confermati!!!
        objfiltro["includivincoli"] = true; //Prendo risorse tra quelle da visualizzare nei blocchi di calendario vincolanti!!!

        var datastartview = pdatestart || new Date((new Date()).setHours(0, 0, 0, 0));
        var days = days || 60;

        //IMPOSTAZIONE filtro date ( prende -60 giorni dalla data passata e + 60 dalla stessa)
        var datestart = new Date(datastartview);
        datestart.setDate(datestart.getDate() - Number(days));
        var dateend = new Date(datastartview);
        dateend.setDate(dateend.getDate() + Number(days));

        objfiltro["datestart"] = moment(datestart).format("DD/MM/YYYY HH:mm:ss");// datestart.toString('dd/MM/yyyy HH:mm:ss');
        objfiltro["dateend"] = moment(dateend).format("DD/MM/YYYY HH:mm:ss");//dateend.toString('dd/MM/yyyy HH:mm:ss');

        //CArichiamo i dati dinamicamente tramite ajax call
        var functiontocallonend = functocall;
        //objfiltro["idtipofasce"] = '2';
        caricaDatiServerEvento(lng, objfiltro,
            function (result, callafterfilter) {
                try {

                    var parseddata = '';
                    globalObject[controlid + "evento"] = '';
                    globalObject[controlid + "eventi"] = '';
                    globalObject[controlid + "listini"] = '';
                    globalObject[controlid + "reslist"] = '';
                    globalObject[controlid + "tipofasce"] = '';
                    globalObject[controlid + "statuslist"] = '';
                    globalObject[controlid + "statuslist"] = '';
                    //globalObject[controlid + "objfiltro"] = '';
                    globalObject[controlid + "vincolistrutture"] = '';
                    if (result != '') {
                        globalObject[controlid + "evento"] = JSON.parse(result).eventitem;//oggetto vuoto per update ed insert
                        globalObject[controlid + "eventi"] = JSON.parse(result).eventi;//oggetto vuoto per update ed insert
                        globalObject[controlid + "listino"] = JSON.parse(result).listinoitem;//oggetto vuoto per update ed insert
                        globalObject[controlid + "listini"] = JSON.parse(result).listini;//array oggetti json listini
                        globalObject[controlid + "reslist"] = JSON.parse(result).reslist;//array oggetti json lista risorse
                        globalObject[controlid + "tipofasce"] = JSON.parse(result).tipofasce;//array oggetti json lista tipo fasce
                        globalObject[controlid + "statuslist"] = JSON.parse(result).statuslist;//array oggetti json lista tipo fasce
                        globalObject[controlid + "objfiltro"] = JSON.parse(result).objfiltro;//array oggetti json filtri usati nel caricamento dati
                        globalObject[controlid + "vincolistrutture"] = JSON.parse(result).vincolistrutture;//array oggetti json filtri usati nel caricamento dati
                    }

                    //getitemscarrello in carrellohandler per aver gli elementi nel carrello da aggiungere ad evemti !!!!! 
                    //qui vanno evitati gli elementi a carrello delle tipologie diverse dalla 1 e aggiunti gli elementi a carrello delle risorse vincolanti !!
                    (function wait() {
                        if (typeof GetCarrelloItems === "function") {
                            GetCarrelloItems('',
                                function (resultcarrello) {
                                    if (resultcarrello != '') {
                                        try {
                                            var carrelloitems = JSON.parse(resultcarrello);
                                            for (var i = 0; i < carrelloitems.length; i++) {
                                                //carrellotiems[i];(Dataend Datastart)
                                                var item = carrelloitems[i];

                                                var idprodottocarrello = item["id_prodotto"];

                                                var vincoli = globalObject[controlid + "vincolistrutture"];
                                                var risvincolantidic = '';
                                                var vincolante = false;

                                                if (vincoli.hasOwnProperty(idpassed)) {
                                                    var risvincolanti = vincoli[idpassed];
                                                    risvincolantidic = risvincolanti.split(',');
                                                    for (var j = 0; j < risvincolantidic.length; j++) {
                                                        if (risvincolantidic[j] == idprodottocarrello) vincolante = true;
                                                    }
                                                }

                                                if (idprodottocarrello == idpassed || vincolante) //Se l'elemento nel carrello è quello che stiamo visualizzando oppure è vincolante per l'elemento attuale
                                                {
                                                    //CREO ELEMENTO TEMPORANEO
                                                    var tmpelement = JSON.parse(JSON.stringify(globalObject[controlid + "evento"]));
                                                    tmpelement.Enddate = moment(item.Dataend).format("YYYY-MM-DDTHH:mm:ss");
                                                    tmpelement.Startdate = moment(item.Datastart).format("YYYY-MM-DDTHH:mm:ss");
                                                    tmpelement.Idevento = 0;
                                                    tmpelement.Idattivita = idpassed;
                                                    //tmpelement.Prezzo = 0; //Prezzo da calcolare
                                                    tmpelement.Soggetto = 'elemento in carrello';
                                                    //AGGIUNGIAMO ELEMENTO DA CARRELLO ALLA LISTA EVENTI globalObject[controlid + "eventi"]
                                                    //Per consentire il blocco in calendario!!!
                                                    globalObject[controlid + "eventi"].push(tmpelement);
                                                }
                                            }

                                        }
                                        catch (ex) { console.log("err geteventirisorsa: " + ex); }
                                    }

                                    //Caricato tutto procedo con la visualizzazione
                                    callafterfilter(datastartview);
                                });
                        } else {
                            setTimeout(wait, 80);
                        }
                    })();


                }
                catch (e) { console.log("err geteventirisorsa: " + e); }
            },
            functiontocallonend);
    }


    function caricaDatiServerEvento(lng, objfiltro, callback, functiontocallonend) {
        var lng = lng || "I";
        var objfiltro = objfiltro || {};

        $.ajax({
            url: pathAbs + bookinghandlerpath,
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'caricaeventi', 'objfiltro': JSON.stringify(objfiltro), 'lng': lng },
            success: function (result) {
                callback(result, functiontocallonend);
            },
            error: function (result) {
                //sendmessage('fail creating link');
                callback(result.responseText, function () { });
            }
        });
    }

    function CancellaCarrello() {

        CancellaCurrentCarrello(function (result) {
            if (result != true) {
                $('#' + controlid + "info").attr('class', 'alert alert-danger');
                $('#' + controlid + "info").html(result);
            } else {
                //$('#' + controlid + "info").html('Svuotato');
                geteventirisorsa(null, 60, renderCalendarjquery);
            }
        })
    }

    function verificaDatiServerEvento(lng, item, callback, functiontocallonend) {
        var lng = lng || "I";
        var item = item || {};

        $.ajax({
            url: pathAbs + bookinghandlerpath,
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'verificadisponibilita', 'item': JSON.stringify(item), 'lng': lng },
            success: function (result) {
                callback(result, functiontocallonend);
            },
            error: function (result) {
                //sendmessage('fail creating link');
                callback(result.responseText, null);
            }
        });
    }

    function calcolaprezzobyidattivita(start, end, idattivita, callback) {
        $.ajax({
            url: pathAbs + bookinghandlerpath,
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'calcolaprezzo', 'datestart': start, 'dateend': end, 'idattivita': idattivita },
            success: function (result) {
                callback(result);
            },
            error: function (result) {
                callback(result.responseText);
                //callback(result.responseText, function () { });
            }
        });

    }

}