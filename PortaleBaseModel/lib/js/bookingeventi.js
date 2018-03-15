"use strict";

var basedateviewscheduler = new $.jqx.date('todayDate');
var visibleresource = "";
var divmessages = "divMessages";
var oldAppointmentvalues = "";
$(document).ready(function () {
    getdataandrenderscheduler("schedulerprenotazioni", '', basedateviewscheduler);

});


function reloadbyresourceid(caller) {
    //var test = $(caller);
    //var resourceId = txt;//test[0].value 
    // $('#schedulerprenotazioni').jqxScheduler('showAppointmentsByResource', resourceId);

    var txt = $("#" + caller.id + " option:selected").text();
    var val = $("#" + caller.id + " option:selected").val();

    var objfiltro = {};
    objfiltro["idreslist"] = val;
    getdataandrenderscheduler("schedulerprenotazioni", objfiltro, basedateviewscheduler);
}

function initautocomplete(id) {
    $("#" + id).autocomplete({
        source: pathAbs + bookinghandlerpath + '?q=autocompleteclienti&r=20',
        minLength: 0,
        //appendTo: '#' + id,
        open: function () {
            //console.log("OPEN");
            setTimeout(function () {
                $('.ui-autocomplete').css('z-index', 99999999999999);
            }, 0);
        },
        //open: function (event, ui) {
        //    $('.ui-autocomplete').css('position', 'absolute');
        //    $('.ui-autocomplete').css('z-index', 99999999999999);
        //    $('.ui-autocomplete').css('top', '0px');
        //    $('.ui-autocomplete').css('left', '0px');
        //    setTimeout(function () {
        //        $('.ui-autocomplete').css('z-index', 99999999999999);
        //    }, 1);
        //},
        select: function (event, ui) {
            if (ui.item != null) {
                $("#" + id + "Hidden").val(ui.item.id);
                $("#" + id).text(ui.item.label);
            }
        }
    }).on("focus", function () {
        $(this).autocomplete("search", "");
    });

}
function caricaclientebyid(idcliente, idcontrollo, callback) {
    //clientebyid
    $.ajax({
        url: pathAbs + bookinghandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        //async: false,
        data: { 'q': 'clientebyid', 'id': idcliente },
        success: function (result) {
            // callback(result, functiontocallonend);
            if (result != '') {
                var parsedcli = JSON.parse(result);
                if (callback == null)
                    $('#' + idcontrollo).val(parsedcli.Cognome + " " + parsedcli.Nome + " " + parsedcli.Email);
                else {
                    callback(parsedcli);
                    //var textformatted = '';
                    //textformatted = parsedcli.Cognome + " " + parsedcli.Nome + " " + parsedcli.Email;
                    //$('#' + idcontrollo).val(textformatted);
                }
            }
        },
        error: function (result) {
            //sendmessage('fail creating link');
            //callback(result.responseText, function () { });
            $('#' + divmessages).attr('class', 'alert alert-danger'); $('#' + divmessages).html(result.responseText);
        }
    });

}
function calcolaprezzobyidattivita(start, end, idattivita, idcontrollo) {
    //clientebyid
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
            // callback(result, functiontocallonend);
            $('#' + idcontrollo).val(result);
        },
        error: function (result) {
            //callback(result.responseText, function () { });
            $('#' + divmessages).attr('class', 'alert alert-danger'); $('#' + divmessages).html(result.responseText);
        }
    });

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
function updateDatiServerEvento(lng, item, callback, functiontocallonend) {
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
        data: { 'q': 'aggiornaevento', 'item': JSON.stringify(item), 'lng': lng },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, null);
        },
        falilure: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, null);
        }
    });
}
function insertDatiServerEvento(lng, item, callback, functiontocallonend) {
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
        data: { 'q': 'inseriscievento', 'item': JSON.stringify(item), 'lng': lng },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, null);
        },
        falilure: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, null);
        }
    });
}


function deleteDatiServerEvento(lng, item, callback, functiontocallonend) {
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
        data: { 'q': 'deleteevento', 'item': JSON.stringify(item), 'lng': lng },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, null);
        },
        falilure: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, null);
        }
    });
}

function getdataandrenderscheduler(controlid, objfiltro, datastartview) {

    if (objfiltro == '') objfiltro = globalObject[controlid + "objfiltro"];
    var objfiltro = objfiltro || {};
    var datastartview = datastartview || new $.jqx.date(2018, 1, 1);

    //TEST DATA Lista dati vò caricata on query ajax
    //var appointments = new Array();
    //var appointment1 = {
    //    id: "id1",
    //    description: "George brings projector for presentations.",
    //    location: "",
    //    subject: "Quarterly Project Review Meeting",
    //    calendar: "Casa Baccano Bassa Agriturismo",
    //    calendarkv: "10",
    //    start: new Date(2017, 10, 23, 9, 0, 0),
    //    end: new Date(2017, 10, 23, 16, 0, 0),
    //    prezzof: "customfiledval1"
    //}
    //appointments.push(appointment1);
    //globalObject[controlid + "appointments"] = appointments;

    //IMPOSTAZIONE filtro date ( prende -10 giorni dalla data passata e + 60 dalla stessa)
    var datestart = datastartview.addDays(-60);
    var dateend = datastartview.addDays(60);
    objfiltro["datestart"] = datestart.toString('dd/MM/yyyy HH:mm:ss');
    objfiltro["dateend"] = dateend.toString('dd/MM/yyyy HH:mm:ss');


    //CArichiamo i dati dinamicamente tramite ajax call
    var functiontocallonend = displayScheduler;
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
                //globalObject[controlid + "objfiltro"] = '';
                if (result != '') {
                    globalObject[controlid + "evento"] = JSON.parse(result).eventitem;//oggetto vuoto per update ed insert
                    globalObject[controlid + "eventi"] = JSON.parse(result).eventi;//oggetto vuoto per update ed insert
                    globalObject[controlid + "listino"] = JSON.parse(result).listinoitem;//oggetto vuoto per update ed insert
                    globalObject[controlid + "listini"] = JSON.parse(result).listini;//array oggetti json listini
                    globalObject[controlid + "reslist"] = JSON.parse(result).reslist;//array oggetti json lista risorse
                    globalObject[controlid + "tipofasce"] = JSON.parse(result).tipofasce;//array oggetti json lista tipo fasce
                    globalObject[controlid + "statuslist"] = JSON.parse(result).statuslist;//array oggetti json lista tipo fasce

                    globalObject[controlid + "objfiltro"] = JSON.parse(result).objfiltro;//array oggetti json filtri usati nel caricamento dati
                    fillselect(controlid);
                }


                callafterfilter(controlid, datastartview);

            }
            catch (e) { console.log(e); console.log(result); }
        },
        functiontocallonend);


    //  displayScheduler(controlid);

}
function fillselect(controlid) {
    $(".searchlist").each(function () {
        try {
            var idcontrollo = $(this).attr('id');
            var prefix = idcontrollo.substring(0, 4);
            idcontrollo = idcontrollo.substring(4);;//Rimuovo il prefisso 4 caratteri per trovare la lista
            var selectedvalue = '';

            if (globalObject[controlid + "objfiltro"].hasOwnProperty('id' + idcontrollo))
                selectedvalue = globalObject[controlid + "objfiltro"]['id' + idcontrollo];
            fillDDLArraySimple("#" + prefix + idcontrollo, globalObject[controlid + idcontrollo], 'Seleziona ', '', 'id', 'name', selectedvalue);

        } catch (e) { }
    });
}
function displayScheduler(controlid, datastartview) {

    var datastartview = datastartview || new $.jqx.date(2018, 1, 1);

    var source =
        {
            datatype: "json",
            dataFields: [
                { name: 'id', type: 'string', map: 'Idevento' },
                { name: 'calendarkv', type: 'string', map: 'Idattivita' },
                { name: 'prezzof', type: 'string', map: 'Prezzo' },
                { name: 'start', type: 'date', type: 'date', format: "yyyy-MM-dd HH:mm:ss", map: 'Startdatestring' },
                { name: 'end', type: 'date', type: 'date', format: "yyyy-MM-dd HH:mm:ss", map: 'Enddatestring' },
                { name: 'description', type: 'string', map: 'Testoevento' },
                { name: 'place', type: 'string', map: 'Textfield2' },
                { name: 'subject', type: 'string', map: 'Soggetto' },
                { name: 'calendar', type: 'string', map: 'Textfield1' },
                { name: 'idcliente', type: 'string', map: 'Idcliente' },
                { name: 'idvincolo', type: 'string', map: 'Idvincolo' },
                { name: 'jsonfield1', type: 'string', map: 'Jsonfield1' },
                { name: 'codicerichiesta', type: 'string', map: 'Codicerichiesta' },
                { name: 'status', type: 'string', map: 'Status' },
                { name: 'datainserimento', type: 'date', type: 'date', format: "yyyy-MM-ddTHH:mm:ss", map: 'Datainserimento' }
            ],
            id: 'Idevento',
            localData: globalObject[controlid + "eventi"]
        };
    //var dataAdapter = new $.jqx.dataAdapter(source, {
    //    downloadComplete: function (data, status, xhr) { },
    //    loadComplete: function (data) { },
    //    loadError: function (xhr, status, error) { }
    //});
    var dataAdapterSource = new $.jqx.dataAdapter(source, {
        loadComplete: function (data) {
        }
    });

    var rsdataAdapter = new $.jqx.dataAdapter(globalObject[controlid + "reslist"]);
    var rsdataAdapterstatus = new $.jqx.dataAdapter(globalObject[controlid + "statuslist"]);

    var datemin = new $.jqx.date(2010, 1, 1); // da decidere che data mettere;
    var datemax = new $.jqx.date(9999, 1, 1); // da decidere che data mettere;
    //$('#' + controlid).jqxScheduler('destroy');

    $("#" + controlid).jqxScheduler({
        date: datastartview,
        min: datemin, //Setto la data minima navaigabile
        max: datemax,  //Setto la data massima navigabile
        width: '100%',
        height: '100%',
        dayNameFormat: "abbr",
        source: dataAdapterSource,
        showLegend: true,
        localization: {
            // separator of parts of a date (e.g. '/' in 11/05/1955)
            '/': "/",
            // separator of parts of a time (e.g. ':' in 05:44 PM)
            ':': ":",
            // the first day of the week (0 = Sunday, 1 = Monday, etc)
            firstDay: 0,
            days: {
                // full day names
                names: ["Domenica", "Lunedì", "martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato"],
                // abbreviated day names
                namesAbbr: ["Dom", "Lun", "mar", "Mer", "Gio", "ven", "Sab"],
                // shortest day names
                namesShort: ["Do", "Lu", "Ma", "Me", "Gi", "Ve", "Sa"]
            },
            months: {
                // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
                names: ["Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre", ""],
                // abbreviated month names
                namesAbbr: ["Gen", "Feb", "Mar", "Apr", "Mag", "Giu", "Lug", "Ago", "Set", "Ott", "Nov", "Dic", ""]
            },
            // AM and PM designators in one of these forms:
            // The usual view, and the upper and lower case versions
            //      [standard,lowercase,uppercase]
            // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
            //      null
            AM: ["AM", "am", "AM"],
            PM: ["PM", "pm", "PM"],
            eras: [
                // eras in reverse chronological order.
                // name: the name of the era in this culture (e.g. A.D., C.E.)
                // start: when the era starts in ticks (gregorian, gmt), null if it is the earliest supported era.
                // offset: offset in years from gregorian calendar
                { "name": "A.D.", "start": null, "offset": 0 }
            ],
            twoDigitYearMax: 2029,
            patterns: {
                // short date pattern
                d: "d/M/yyyy",
                // long date pattern
                D: "dddd, dd MMMM, yyyy",
                // short time pattern
                t: "h:mm tt",
                // long time pattern
                T: "h:mm:ss tt",
                // long date, short time pattern
                f: "dddd, dd MMMM, yyyy h:mm tt",
                // long date, long time pattern
                F: "dddd, dd MMMM, yyyy h:mm:ss tt",
                // month/day pattern
                M: "dd MMMM",
                // month/year pattern
                Y: "MMMM yyyy",
                // S is a sortable format that does not vary by culture
                S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss",
                // formatting of dates in MySQL DataBases
                ISO: "yyyy-MM-dd hh:mm:ss",
                ISO2: "yyyy-MM-dd HH:mm:ss",
                d1: "dd.MM.yyyy",
                d2: "dd-MM-yyyy",
                d3: "dd-MMMM-yyyy",
                d4: "dd-MM-yy",
                d5: "H:mm",
                d6: "HH:mm",
                d7: "HH:mm tt",
                d8: "dd/MMMM/yyyy",
                d9: "MMMM-dd",
                d10: "MM-dd",
                d11: "MM-dd-yyyy"
            },
            agendaViewString: "Agenda",
            agendaAllDayString: "giorno intero",
            agendaDateColumn: "Data",
            agendaTimeColumn: "Ora",
            agendaAppointmentColumn: "Appuntamento",
            backString: "Indietro",
            forwardString: "Avanti",
            toolBarPreviousButtonString: "precedente",
            toolBarNextButtonString: "prossimo",
            emptyDataString: "Dati non presenti",
            loadString: "Caricamento...",
            clearString: "Cancella",
            todayString: "Oggi",
            dayViewString: "Giorno",
            weekViewString: "Settimana",
            monthViewString: "Mese",
            timelineDayViewString: "Timeline Giorno",
            timelineWeekViewString: "Timeline Settimana",
            timelineMonthViewString: "Timeline Mese",
            loadingErrorMessage: "The data is still loading and you cannot set a property or call a method. You can do that once the data binding is completed. jqxScheduler raises the 'bindingComplete' event when the binding is completed.",
            editRecurringAppointmentDialogTitleString: "Edit Recurring Appointment",
            editRecurringAppointmentDialogContentString: "Do you want to edit only this occurrence or the series?",
            editRecurringAppointmentDialogOccurrenceString: "Edit Occurrence",
            editRecurringAppointmentDialogSeriesString: "Edit The Series",
            editDialogTitleString: "Modifica Appuntamenti",
            editDialogCreateTitleString: "Crea Nuovo Appuntamento",
            contextMenuEditAppointmentString: "Modifica Appuntamento",
            contextMenuCreateAppointmentString: "Crea Nuovo Appuntamento",
            editDialogSubjectString: "Soggetto",
            editDialogLocationString: "Posizione",
            editDialogFromString: "Da",
            editDialogToString: "A",
            editDialogAllDayString: "Tutto il giorno",
            editDialogExceptionsString: "Eccezioni",
            editDialogResetExceptionsString: "Resetta dopo salvataggio",
            editDialogDescriptionString: "Descrizione",
            editDialogResourceIdString: "Proprietario",
            editDialogStatusString: "Stato",
            editDialogColorString: "Colore",
            editDialogColorPlaceHolderString: "Seleziona Colore",
            editDialogTimeZoneString: "Time Zone",
            editDialogSelectTimeZoneString: "Select Time Zone",
            editDialogSaveString: "Salva",
            editDialogDeleteString: "Cancella",
            editDialogCancelString: "Annulla",
            editDialogRepeatString: "Ripeti",
            editDialogRepeatEveryString: "Ripeti ogni",
            editDialogRepeatEveryWeekString: "settimana(e)",
            editDialogRepeatEveryYearString: "anno(i)",
            editDialogRepeatEveryDayString: "giorno(i)",
            editDialogRepeatNeverString: "Mai",
            editDialogRepeatDailyString: "Giornalmente",
            editDialogRepeatWeeklyString: "Settimanalmente",
            editDialogRepeatMonthlyString: "Mensilmente",
            editDialogRepeatYearlyString: "Annualmente",
            editDialogRepeatEveryMonthString: "mese(i)",
            editDialogRepeatEveryMonthDayString: "Giorno",
            editDialogRepeatFirstString: "primo",
            editDialogRepeatSecondString: "secondo",
            editDialogRepeatThirdString: "terzo",
            editDialogRepeatFourthString: "quarto",
            editDialogRepeatLastString: "ultimo",
            editDialogRepeatEndString: "Fine",
            editDialogRepeatAfterString: "Dopo",
            editDialogRepeatOnString: "Su",
            editDialogRepeatOfString: "di",
            editDialogRepeatOccurrencesString: "elemento(i)",
            editDialogRepeatSaveString: "Salva elemento",
            editDialogRepeatSaveSeriesString: "Salva serie",
            editDialogRepeatDeleteString: "Cancella elemento",
            editDialogRepeatDeleteSeriesString: "Cancella Serie",
            editDialogStatuses:
                {
                    free: "Libero",
                    tentative: "Tentativo",
                    busy: "Occupato",
                    outOfOffice: "Assente"
                },
            loadingErrorMessage: "The data is still loading and you cannot set a property or call a method. You can do that once the data binding is completed. jqxScheduler raises the 'bindingComplete' event when the binding is completed.",
        },


        // called when the dialog is craeted.
        editDialogCreate: function (dialog, fields, editAppointment) {
            // hide repeat option
            fields.repeatContainer.hide();
            // hide status option
            //fields.statusContainer.hide();
            // hide timeZone option
            fields.timeZoneContainer.hide();
            // hide color option
            fields.colorContainer.hide();
            fields.locationContainer.hide();
            //fields.descriptionContainer.hide();
            fields.statusContainer.hide();
            fields.allDayContainer.hide();
            fields.repeat.hide();
            fields.repeatContainer.hide();
            fields.repeatLabel.hide();
            fields.repeatPanel.hide();
            // fields.subjectContainer.attr("style", "visibility:hidden");
            fields.subjectLabel.html("Titolo");
            //fields.locationLabel.html("Where");
            fields.fromLabel.html("Start");
            fields.toLabel.html("End");
            fields.resourceLabel.html("Location Def");
            // fields.resourceContainer.hide();//Se fai hide non funziona più bene l'interfaccia

            fields.resourceContainer.attr("style", "visibility:hidden");

            //Add custom ddl for resources
            //fields.resource.jqxDropDownList({ source: rsdataAdapter, displayMember: "name", valueMember: "id" });

            //Add custom fields
            var jsonfield1Container = ''
            jsonfield1Container += "<div id='jsonfield1Container'>"
            jsonfield1Container += "<div class='jqx-scheduler-edit-dialog-label'>jsonfield1</div>"
            jsonfield1Container += "<div class='jqx-scheduler-edit-dialog-field'><input type='text' id='divjsonfield1' /></div>"
            jsonfield1Container += "</div>"
            $(fields.toContainer).after(jsonfield1Container);
            $('#divjsonfield1').jqxInput({ width: '99%', disabled: true, height: 25, placeHolder: 'jsonfield1' });
            $('#jsonfield1Container').attr("style", "display:none");

            var jsonviewContainer = ''
            jsonviewContainer += "<div id='jsonfield1Container'>"
            jsonviewContainer += "<div class='jqx-scheduler-edit-dialog-label'>Info</div>"
            jsonviewContainer += "<div class='jqx-scheduler-edit-dialog-field' id='divshowjson'></div>"
            jsonviewContainer += "</div>"
            $(fields.toContainer).after(jsonviewContainer);

            var clienteContainer = ''
            clienteContainer += "<div>"
            clienteContainer += "<div class='jqx-scheduler-edit-dialog-label' style=\"line-height:16px\" id=\"divClienteLabel\" onclick='javascript:viewcliente(this)'>Cliente<div style=\"font-size:8px;color#ccc\">(Click per dettagli)</div></div>"
            clienteContainer += "<div class='jqx-scheduler-edit-dialog-field' style=\"position:relative;overflow:visible\"><input  type='text' id='divCliente' /><input   type='hidden' id='divClienteHidden' /></div>"
            clienteContainer += "</div>"
            $(fields.toContainer).after(clienteContainer);
            $('#divCliente').jqxInput({ width: '99%', height: 25, placeHolder: 'Cliente' });


            var vincoloContainer = ''
            vincoloContainer += "<div id='vincoloContainer'>"
            vincoloContainer += "<div class='jqx-scheduler-edit-dialog-label'>Vincolo</div>"
            vincoloContainer += "<div class='jqx-scheduler-edit-dialog-field'><input type='text' id='divVincolo' /></div>"
            vincoloContainer += "</div>"
            $(fields.toContainer).after(vincoloContainer);
            $('#divVincolo').jqxInput({ width: '99%', disabled: true, height: 25, placeHolder: 'Vincolo' });
            $('#vincoloContainer').attr("style", "display:none");



            var CodicerichiestaContainer = ''
            CodicerichiestaContainer += "<div>"
            CodicerichiestaContainer += "<div class='jqx-scheduler-edit-dialog-label'>Codicerichiesta</div>"
            CodicerichiestaContainer += "<div class='jqx-scheduler-edit-dialog-field'><input type='text' id='divCodicerichiesta' /></div>"
            CodicerichiestaContainer += "</div>"
            $(fields.toContainer).after(CodicerichiestaContainer);
            $('#divCodicerichiesta').jqxInput({ width: '99%', disabled: true, height: 25, placeHolder: 'Codicerichiesta' });

            var DatainserimentoContainer = ''
            DatainserimentoContainer += "<div>"
            DatainserimentoContainer += "<div class='jqx-scheduler-edit-dialog-label'>Datainserimento</div>"
            DatainserimentoContainer += "<div class='jqx-scheduler-edit-dialog-field'><input type='text' id='divDatainserimento' /></div>"
            DatainserimentoContainer += "</div>"
            $(fields.toContainer).after(DatainserimentoContainer);
            $('#divDatainserimento').jqxInput({ width: '99%', disabled: true, height: 25, placeHolder: 'Datainserimento' });

            var StatusContainer = ''
            StatusContainer += "<div>"
            StatusContainer += "<div class='jqx-scheduler-edit-dialog-label'>Status</div>"
            StatusContainer += "<div class='jqx-scheduler-edit-dialog-field'><div id='ddlStatus'></div></div>"
            StatusContainer += "</div>"
            $(fields.toContainer).after(StatusContainer);
            //$('#divStatus').jqxInput({ width: '99%', height: 25, placeHolder: 'Status' });
            $('#ddlStatus').jqxDropDownList({ width: '99%', height: 25, placeHolder: 'Stato', disabled: false, source: rsdataAdapterstatus, displayMember: "name", valueMember: "id" });




            var prezzoContainer = ''
            prezzoContainer += "<div>"
            prezzoContainer += "<div class='jqx-scheduler-edit-dialog-label' style=\"line-height:16px\"  onclick='javascript:ricalcolaprezzo()'>Prezzo<div style=\"font-size:8px;color#ccc\">(Click ricalcola)</div></div>"
            prezzoContainer += "<div class='jqx-scheduler-edit-dialog-field'><input type='text' id='divPrezzo' /></div>"
            prezzoContainer += "</div>"
            $(fields.toContainer).after(prezzoContainer);
            $('#divPrezzo').jqxInput({ width: '99%', height: 25, placeHolder: 'Prezzo' });



            var resourcecustomContainer = '';
            resourcecustomContainer += "<div>"
            resourcecustomContainer += "<div class='jqx-scheduler-edit-dialog-label'>Location</div>"
            resourcecustomContainer += "<div class='jqx-scheduler-edit-dialog-field'><div id='ddlResource'>"
            resourcecustomContainer += "</div></div>"
            resourcecustomContainer += "</div>"
            $(fields.toContainer).after(resourcecustomContainer);
            $('#ddlResource').jqxDropDownList({ width: '99%', height: 25, placeHolder: 'Location', disabled: true, source: rsdataAdapter, displayMember: "name", valueMember: "id" });





            //$('#ddlResource').on('select', function (event) {
            //    $("#ddlResource_hide").val($(this).val());
            //});


        },
        editDialogOpen: function (dialog, fields, editAppointment) {
            initautocomplete('divCliente');
        },
        editDialogClose: function (dialog, fields, editAppointment) {
        },
        /**
     * called when a key is pressed while the dialog is on focus. Returning true or false as a result disables the built-in keyDown handler.
     * @param {Object} dialog - jqxWindow's jQuery object.
     * @param {Object} fields - Object with all widgets inside the dialog.
     * @param {Object} the selected appointment instance or NULL when the dialog is opened from cells selection.
     * @param {jQuery.Event Object} the keyDown event.
     */
        editDialogKeyDown: function (dialog, fields, editAppointment, event) {
            //console.log(fields.from.val());
            //console.log(fields.to.val());
            //console.log(editAppointment.to.toString('yyyy-MM-ddTHH:mm:ss')); 
        },
        ready: function () {
            var width = $("#" + controlid).jqxScheduler('scrollWidth');
            var height = $("#" + controlid).jqxScheduler('scrollHeight');
            //$("#" + controlid).jqxScheduler('scrollTop', 50);
            //$("#" + controlid).jqxScheduler('scrollLeft', 0);
            //console.log($("#" + controlid).jqxScheduler());
            //var toppos = $("#" + controlid).jqxScheduler().scrollTop();
            //var leftpos = $("#" + controlid).jqxScheduler().scrollLeft();
            //console.log("Scroll Width:" + width + ", Scroll Height:" + height + ", top pos " + toppos + ", left pos " + leftpos);

            //console.log("ready: " + visibleresource);
            //if (visibleresource != '')
            //    $("#" + controlid).jqxScheduler('ensureAppointmentVisible', visibleresource);

        },
        resources:
            {
                colorScheme: "scheme05",
                dataField: "calendar",
                orientation: "vertical",
                source: new $.jqx.dataAdapter(source) //rsdataAdapter
            },
        appointmentDataFields:
            {
                from: "start",
                to: "end",
                id: "id",
                description: "description",
                location: "place",
                subject: "subject",
                resourceId: "calendar",
                resourcecustom: "calendarkv",
                prezzo: "prezzof",
                datainserimento: "datainserimento",
                idcliente: "idcliente",
                idvincolo: "idvincolo",
                jsonfield1: "jsonfield1",
                codicerichiesta: "codicerichiesta",
                status: "status"
            },
        view: 'monthView',
        views:
            [
                //{ type: "dayView", showWeekends: true, timeRuler: { scaleStartHour: 7, scaleEndHour: 21 } },
                //{ type: "weekView", appointmentHeight: 30, showWeekends: true, timeRuler: { scaleStartHour: 0, scaleEndHour: 23 } },
                { type: "monthView", appointmentHeight: 30, showWeekends: true, timeRuler: { formatString: "dd MMM" } }
                //{ type: 'timelineDayView', appointmentHeight: 60, timeSlotWidth: 50, timeRuler: { formatString: "dd/MM HH:mm" } },
                //{ type: 'timelineWeekView', appointmentHeight: 60, timeSlotWidth: 50, timeRuler: { formatString: "dd/MM HH:mm" } },
                //{ type: 'timelineMonthView', appointmentHeight: 60, timeSlotWidth: 50, timeRuler: { formatString: "dd/MM" } }
            ]

    });

    $("#" + controlid).off('viewChange').on('viewChange', function (event) {
        var args = event.args;
        var from = args.from;
        var to = args.to;
        var date = args.date;
    });

    $("#" + controlid).off('dateChange').on('dateChange', function (event) {
        var args = event.args;
        var from = args.from;
        var to = args.to;
        var date = args.date;
        basedateviewscheduler = date;
        getdataandrenderscheduler("schedulerprenotazioni", '', basedateviewscheduler);
        $("#" + controlid).off('dateChange');
        //console.log("dateChange is raised" + date.toString('dd/MM/yyyy HH:mm:ss'));
    });

    /*Aggiunta campo custom nel dialog*/
    $("#" + controlid).off('editDialogCreate').on('editDialogCreate', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        var dialog = args.dialog;
        var fields = args.fields;

        //var resourcekvddl = '';
        //resourcekvddl += "<div>"
        //resourcekvddl += "<div class='jqx-scheduler-edit-dialog-label'>Location</div>"
        //resourcekvddl += "<div class='jqx-scheduler-edit-dialog-field'><div id='ddlResource'>"
        //resourcekvddl += "</div></div>"
        //resourcekvddl += "</div>"

        //var PrezzoField = ''
        //PrezzoField += "<div>"
        //PrezzoField += "<div class='jqx-scheduler-edit-dialog-label'>Prezzo</div>"
        //PrezzoField += "<div class='jqx-scheduler-edit-dialog-field'><input type='text' id='divPrezzo' /></div>"
        //PrezzoField += "</div>"
        //var i = 0;
        //$('#dialogscheduler').children('div').each(function () { // loop trough the div's (only first level childs) elements in dialogscheduler
        //    i += 1;
        //    if (i == 2) {
        //        $(this).after(PrezzoField);// places the field in the third position.
        //        $(this).after(resourcekvddl); //After place the ddl
        //    };
        //});

        //$('#ddlResource').jqxDropDownList({ width: '99%', height: 25, placeHolder: 'Location', source: rsdataAdapter, displayMember: "name", valueMember: "id" });

        //$('#divPrezzo').jqxInput({ width: '99%', height: 25, placeHolder: 'Prezzo' });

    });
    $("#" + controlid).off('editDialogKeyDown').on('editDialogKeyDown', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        var dialog = args.dialog;
        var fields = args.fields;
    });
    $("#" + controlid).off('editDialogOpen').on('editDialogOpen', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        var dialog = args.dialog;
        var fields = args.fields;

        if (appointment) {
            $('#divPrezzo').val(appointment.prezzo);
            $('#ddlResource').val(appointment.resourcecustom);
            $('#divClienteHidden').val(appointment.idcliente);
            caricaclientebyid(appointment.idcliente, "divCliente");
            //  $('#divCliente').val('da caricare');  //////////////////////////facciamo una chiata ajax per caricare il cleinte  
            $('#divVincolo').val(appointment.idvincolo);
            $('#divjsonfield1').val(appointment.jsonfield1);
            $('#divCodicerichiesta').val(appointment.codicerichiesta);
            $('#divDatainserimento').val(moment(appointment.datainserimento).format("DD/MM/YYYY HH:mm:ss"));
            $('#ddlStatus').val(appointment.status);

            if (appointment.jsonfield1 != '' && appointment.jsonfield1 != null) {
                try {
                    var details = JSON.parse(appointment.jsonfield1);
                    var testoinfo = "";
                    if (details.hasOwnProperty("bambini"))
                        testoinfo += GetResourcesValue("formtestobambini") + " " + details["bambini"] + " ";
                    if (details.hasOwnProperty("adulti"))
                        testoinfo += GetResourcesValue("formtestoadulti") + " " + details["adulti"];
                    $('#divshowjson').html(testoinfo);
                } catch (er) { }
            }

        } else {


            $('#divCliente').val('');
            $('#divClienteHidden').val('');
            $('#divVincolo').val('');
            $('#divjsonfield1').val('');
            $('#divCodicerichiesta').val('');
            $('#divDatainserimento').val('');
            $('#ddlStatus').val('0');
            //get form ddl from value
            //var item = $("#ddlResource").jqxDropDownList('getItemByValue', fields.resource.val());
            //Get in dll by finding by text in custom ddl
            var items = $("#ddlResource").jqxDropDownList('getItems');
            // find the index by searching for an item with specific value (il valore è dal campo resource).
            var indexToSelect = -1;
            $.each(items, function (index) {
                if (this.label == fields.resource.val()) {
                    indexToSelect = index;
                    return false;
                }
            });
            $("#ddlResource").jqxDropDownList({ selectedIndex: indexToSelect });

            //Calcoliamo il prezzo per il periodo e l'attivita selezionata
            $('#divPrezzo').val('0');
            var enddate = fields.to.val();
            var startdate = fields.from.val();
            var idresource = $("#ddlResource").jqxDropDownList().val();
            calcolaprezzobyidattivita(startdate, enddate, idresource, 'divPrezzo');

        };
    });

    $("#" + controlid).off('appointmentDelete').on('appointmentDelete', function (event) {
        var args = event.args;
        var appointment = args.appointment;

        var tmpelement = globalObject[controlid + "evento"];
        tmpelement.idevento = appointment.id;
        var functiontocallonend = enddelete;
        deleteDatiServerEvento(lng, tmpelement,
            function (result, callafterfilter) {
                try {
                    if (callafterfilter != null)
                        callafterfilter(controlid);
                    else {
                        $('#' + divmessages).attr('class', 'alert alert-danger'); $('#' + divmessages).html(result);  //  appointment = oldAppointmentvalues;
                        getdataandrenderscheduler(controlid, '', basedateviewscheduler);
                    }
                }
                catch (e) { $('#' + divmessages).attr('class', 'alert alert-danger'); $('#' + divmessages).html(e); console.log(e); console.log(result); }
            },
            functiontocallonend);
        //console.log("appointmentDelete is raised");
    });


    $("#" + controlid).off('appointmentChange').on('appointmentChange', function (event) {
        //console.log('raised appointmentChange');
        var args = event.args;
        var appointment = args.appointment; //Nuovi valori appuntamento!!!
        //console.log(appointment)
        //console.log(oldAppointmentvalues)

        //riprendo i valori custom dal vecchio elemento INFATTI nel nuovo sono sempre UNDEFINED
        if (appointment.prezzo == null) appointment.prezzo = oldAppointmentvalues.prezzo;
        if (appointment.resourcecustom == null) appointment.resourcecustom = oldAppointmentvalues.resourcecustom;
        if (appointment.idcliente == null) appointment.idcliente = oldAppointmentvalues.idcliente;
        if (appointment.idvincolo == null) appointment.idvincolo = oldAppointmentvalues.idvincolo;
        if (appointment.jsonfield1 == null) appointment.jsonfield1 = oldAppointmentvalues.jsonfield1;
        if (appointment.codicerichiesta == null) appointment.codicerichiesta = oldAppointmentvalues.codicerichiesta;
        //if (appointment.datainserimento == null) appointment.datainserimento = oldAppointmentvalues.datainserimento;
        if (appointment.status == null) appointment.status = oldAppointmentvalues.status;

        //if ($("#ddlResource").length > 0) { //Forzo il set del resourceid inlinea con quello della ddlresource custom
        //    var testoselezione = $("#ddlResource").jqxDropDownList('getSelectedItem').label
        //    appointment.resourceId = testoselezione; // se non lo salvi lo perdi
        //}

        if ($("#ddlResource").val() != null && $("#ddlResource").val() == appointment.resourcecustom) {
            appointment.resourcecustom = $("#ddlResource").val();
            if ($("#divPrezzo").val() != null)
                appointment.prezzo = $("#divPrezzo").val();
            if (appointment.prezzo == '') appointment.prezzo = 0;
            if ($("#divClienteHidden").val() != null)
                appointment.idcliente = $("#divClienteHidden").val();
            if (appointment.idcliente == '') appointment.idcliente = 0;
            if ($("#divVincolo").val() != null)
                appointment.idvincolo = $("#divVincolo").val();
            if ($("#divjsonfield1").val() != null)
                appointment.idjsonfield1 = $("#divjsonfield1").val();

            if ($("#divCodicerichiesta").val() != null)
                appointment.codicerichiesta = $("#divCodicerichiesta").val();

            //if ($("#divDatainserimento").val() != null)
            //    appointment.datainserimento = $("#divDatainserimento").val();

            if ($("#ddlStatus").val() != null && $("#ddlStatus").val() != '')
                appointment.status = $("#ddlStatus").val();
            if (appointment.status == '') appointment.status = 0;
        }
        //Riempiamo l'elemento della memoria per l'aggiornamento
        var tmpelement = globalObject[controlid + "evento"];
        //IMPOSTO ORARI CUSTOM PER IL SALVATAGGIO DATI
        appointment.to = new $.jqx.date(appointment.to.year(), appointment.to.month(), appointment.to.day(), 10, 0, 0);
        appointment.from = new $.jqx.date(appointment.from.year(), appointment.from.month(), appointment.from.day(), 16, 0, 0);
        tmpelement.Enddate = appointment.to.toString('yyyy-MM-ddTHH:mm:ss')
        tmpelement.Startdate = appointment.from.toString('yyyy-MM-ddTHH:mm:ss')
        tmpelement.idevento = appointment.id;
        tmpelement.Idattivita = appointment.resourcecustom;
        tmpelement.Prezzo = appointment.prezzo;
        tmpelement.Soggetto = appointment.subject;
        tmpelement.Testoevento = appointment.description;
        tmpelement.idcliente = appointment.idcliente;
        tmpelement.idvincolo = appointment.idvincolo;
        tmpelement.jsonfield1 = appointment.jsonfield1;
        tmpelement.codicerichiesta = appointment.codicerichiesta;
        tmpelement.status = appointment.status;


        // tmplistino.Idtipolistino = appointment.description;

        var functiontocallonend = endupdate;
        updateDatiServerEvento(lng, tmpelement,
            function (result, callafterfilter) {
                try {
                    if (callafterfilter != null)
                        callafterfilter(controlid);
                    else {
                        $('#' + divmessages).attr('class', 'alert alert-danger'); $('#' + divmessages).html(result);
                        //  appointment = oldAppointmentvalues;
                        getdataandrenderscheduler(controlid, '', basedateviewscheduler);
                    }
                }
                catch (e) { $('#' + divmessages).attr('class', 'alert alert-danger'); $('#' + divmessages).html(e); console.log(e); console.log(result); }
            },
            functiontocallonend);

    });



    $("#" + controlid).off('appointmentAdd').on('appointmentAdd', function (event) {
        var args = event.args;
        var appointment = args.appointment;

        if ($("#divPrezzo").val() != null)
            appointment.prezzo = $("#divPrezzo").val();
        if (appointment.prezzo == '') appointment.prezzo = 0;
        if ($("#divClienteHidden").val() != null)
            appointment.idcliente = $("#divClienteHidden").val();
        if (appointment.idcliente == '') appointment.idcliente = 0;
        if ($("#divVincolo").val() != null)
            appointment.idvincolo = $("#divVincolo").val();
        if ($("#divjsonfield1").val() != null)
            appointment.jsonfield1 = $("#divjsonfield1").val();
        if ($("#divCodicerichiesta").val() != null)
            appointment.codicerichiesta = $("#divCodicerichiesta").val();
        //if ($("#divDatainserimento").val() != null)
        //    appointment.datainserimento = $("#divDatainserimento").val();
        if ($("#ddlStatus").val() != null)
            appointment.status = $("#ddlStatus").val();
        if (appointment.status == '') appointment.status = 0;
        if ($("#ddlResource").val() != null)
            appointment.resourcecustom = $("#ddlResource").val();

        //if ($("#ddlResource").length > 0) {
        //    var testoselezione = $("#ddlResource").jqxDropDownList('getSelectedItem').label
        //    appointment.resourceId = testoselezione;
        //}

        //Riempiamo l'elemento nuovo  per l'aggiornamento
        var tmpelement = globalObject[controlid + "evento"];
        //IMPOSTO ORARI CUSTOM PER IL SALVATAGGIO DATI
        appointment.to = new $.jqx.date(appointment.to.year(), appointment.to.month(), appointment.to.day(), 10, 0, 0);
        appointment.from = new $.jqx.date(appointment.from.year(), appointment.from.month(), appointment.from.day(), 16, 0, 0);
        tmpelement.Enddate = appointment.to.toString('yyyy-MM-ddTHH:mm:ss');
        tmpelement.Startdate = appointment.from.toString('yyyy-MM-ddTHH:mm:ss');
        tmpelement.idevento = 0;
        tmpelement.Idattivita = appointment.resourcecustom;
        tmpelement.Prezzo = appointment.prezzo;
        tmpelement.Soggetto = appointment.subject;
        tmpelement.Testoevento = appointment.description;
        tmpelement.idcliente = appointment.idcliente;
        tmpelement.idvincolo = appointment.idvincolo;
        tmpelement.jsonfield1 = appointment.jsonfield1;
        tmpelement.codicerichiesta = appointment.codicerichiesta;
        tmpelement.status = appointment.status;

        //if ($("#ddl1tipofasce").val() != null)
        //    tmplistino.Idtipolistino = $("#ddl1tipofasce").val();

        var functiontocallonend = endinsert;
        insertDatiServerEvento(lng, tmpelement,
            function (result, callafterfilter) {
                try {
                    if (result != '') {
                        try {
                            //tmpelement = JSON.parse(result);
                            //var new_key = tmpelement.idevento;
                            //var old_key = appointment.id;
                            //TEST
                            //if (old_key !== new_key) {
                            //    Object.defineProperty(appointment.jqxAppointment.scheduler.appointmentsByKey, new_key,
                            //        Object.getOwnPropertyDescriptor(appointment.jqxAppointment.scheduler.appointmentsByKey, old_key));
                            //    delete appointment.jqxAppointment.scheduler.appointmentsByKey[old_key];
                            //}
                            //appointment.id = appointment.jqxAppointment.id = appointment.originalData.id = new_key;
                            //METODO 1
                            //$("#" + controlid).jqxScheduler('beginAppointmentsUpdate');
                            //appointment.id = new_key;
                            //$("#" + controlid).jqxScheduler('endAppointmentsUpdate');

                            //METODO 2
                            //$('#schedulerprenotazioni').jqxScheduler('deleteAppointment', old_key);
                            //appointment.id = new_key;
                            //$('#schedulerprenotazioni').jqxScheduler('addAppointment', appointment);
                        }
                        catch (er) { }

                    }
                    //Aggioranre id elemento scheduler!!
                    if (callafterfilter != null) {

                        callafterfilter(controlid);
                    }
                    else {
                        //  appointment = oldAppointmentvalues;

                        $('#' + divmessages).attr('class', 'alert alert-danger');
                        $('#' + divmessages).html(result);
                        getdataandrenderscheduler(controlid, '', basedateviewscheduler);
                    }
                }
                catch (e) { $('#' + divmessages).attr('class', 'alert alert-danger'); $('#' + divmessages).html(e); console.log(e); console.log(result); }
            },
            functiontocallonend);

        //console.log("appointmentAdd is raised");
    });
    $("#" + controlid).off('appointmentClick').on('appointmentClick', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        oldAppointmentvalues = args.appointment; //Memorizzo i vecchi valori dell'appuntamento per usarli nell evento change!!!!
        //console.log("appointmentClick is raised");
        // console.log(oldAppointmentvalues);
        //visibleresource = appointment.resourceId;
        //var width = $("#" + controlid).jqxScheduler('scrollWidth');
        //var height = $("#" + controlid).jqxScheduler('scrollHeight');
        //$("#" + controlid).jqxScheduler('scrollTop', height);
        //$("#" + controlid).jqxScheduler('scrollLeft', 0);
        //console.log($("#" + controlid).jqxScheduler());
        //var toppos = $("#" + controlid).jqxScheduler().scrollTop();
        //var leftpos = $("#" + controlid).jqxScheduler().scrollLeft();
        //console.log("Scroll Width:" + width + ", Scroll Height:" + height + ", top pos " + toppos + ", left pos " + leftpos);

    });
    $("#" + controlid).off('appointmentDoubleClick').on('appointmentDoubleClick', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        // appointment fields
        // originalData - the bound data.
        // from - jqxDate object which returns when appointment starts.
        // to - jqxDate objet which returns when appointment ends.
        // status - String which returns the appointment's status("busy", "tentative", "outOfOffice", "free", "").
        // resourceId - String which returns the appointment's resouzeId
        // hidden - Boolean which returns whether the appointment is visible.
        // allDay - Boolean which returns whether the appointment is allDay Appointment.
        // resiable - Boolean which returns whether the appointment is resiable Appointment.
        // draggable - Boolean which returns whether the appointment is resiable Appointment.
        // id - String or Number which returns the appointment's ID.
        // subject - String which returns the appointment's subject.
        // location - String which returns the appointment's location.
        // description - String which returns the appointment's description.
        // tooltip - String which returns the appointment's tooltip.

        console.log("appointmentDoubleClick is raised");
    });

    $("#" + controlid).off('cellClick').on('cellClick', function (event) {
        var args = event.args;
        var cell = args.cell;

        console.log("cellClick is raised");
    });

}

function ricalcolaprezzo() {
    // $("#" + controlid).jqxScheduler('getAppointmentProperty', appointmentId, propertyName);

    //Calcoliamo il prezzo per il periodo e l'attivita selezionata
    $('#divPrezzo').val('0');
    var enddate = oldAppointmentvalues.to.toString('dd/MM/yyyy');
    var startdate = oldAppointmentvalues.from.toString('dd/MM/yyyy');
    var idresource = $("#ddlResource").jqxDropDownList().val();
    calcolaprezzobyidattivita(startdate, enddate, idresource, 'divPrezzo');
}
function viewcliente(elem) {

    var testocliente = '';

    caricaclientebyid($('#divClienteHidden').val(), "",
        function (parsedcli) {
            testocliente = "Id:" + parsedcli.Id_cliente + "<br/>Nome: " + parsedcli.Cognome
                + " " + parsedcli.Nome + "<br/>Email: " + parsedcli.Email
                + "<br/>Tel: " + parsedcli.Telefono + " " + parsedcli.Cellulare;

            if ($(elem).data("tooltipset") != null && $(elem).data("tooltipset"))
                $(elem).tooltip('destroy');
            $(elem).tooltip({
                title: testocliente,
                trigger: 'manual',
                placement: 'right',
                offset: '0 0',
                html: true
            }).data("tooltipset", true).tooltip('toggle');
        });


    //$(elem).click(function () {
    //    $(elem).tooltip('show');
    //}) ; 
    console.log('clicked ' + $('#divClienteHidden').val())
}


function endupdate(controlid) {
    console.log('done update'); $('#' + divmessages).attr('class', 'alert alert-success');
    $('#' + divmessages).html('Aggiornato');
}
function enddelete(controlid) {
    console.log('done delete'); $('#' + divmessages).attr('class', 'alert alert-success');
    $('#' + divmessages).html('Cancellato');
}
function endinsert(controlid) {
    getdataandrenderscheduler(controlid, '', basedateviewscheduler); //Ricarico dal db , ma sarebbe meglio refresahre lo scheduler
    console.log('done insert');
    $('#' + divmessages).attr('class', 'alert alert-success');
    $('#' + divmessages).html('Inserito');
}
