"use strict";



function InitSearchControls(idcontainer, partipologia, template) {
    var idcontainer = idcontainer || "divSearchBarPlaceholder1";
    if (tipologia == "") tipologia = "rif000001"; //questa cerca nella tipologia in base alla pagina dove sei
    tipologia = partipologia || tipologia;
    template = template || "searchbar3.html";

    $("#" + idcontainer).load(pathAbs + "/lib/Template/" + template, function () {
        //Qui puoi fare inizializzazione controlli su allegati
        //FillSearchControls(objfiltro);

        (function wait() {
            if (typeof baseresources !== 'undefined' && baseresources != null && baseresources != '') {
                VisualizzaSearchControls();
            } else {
                setTimeout(wait, 300);
            }
        })();
        initAutocompleteRicercaCaratteristiche();
    });
}

function initAutocompleteRicercaCaratteristiche() {

    //  initializePlacesAutocomplete("geolocation");


    $(".autocompletesearch").each(function () {
        var idact = $(this).attr("id");
        $("#" + idact).autocomplete({
            source: pathAbs + commonhandlerpath + '?q=autocompletericercalist&r=20&tipologia=' + tipologia + "&lng=" + lng,
            minLength: 3,
            //appendTo: '#' + idact , //in alternativa puoi maettere la calsee ui-front al container
            open: function (event, ui) {
                $('.ui-autocomplete').css('position', 'absolute');
                $('.ui-autocomplete').css('z-index', 99999999999999);
                $('.ui-autocomplete').css('top', '0px');
                $('.ui-autocomplete').css('left', '0px');
                $('.ui-autocomplete').css('height', '300px');
                $('.ui-autocomplete').css('overflow-y', 'scroll');
                setTimeout(function () {
                    $('.ui-autocomplete').css('z-index', 99999999999999);
                }, 1);
            },
            select: function (event, ui) {
                if (ui.item != null) {
                    //$("#hidCaratteristica1").val(ui.item.codice);
                    //$("#txthidCaratteristica1").text(ui.item.value);
                    $("#" + idact).text(ui.item.label);
                    location.replace(ui.item.link); //per navigare verso eventuale link
                }
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<a>" + item.linktext + "</a>")
                .appendTo(ul);
        };
    });



    $("#txt" + "hidricercaid").autocomplete({
        source: pathAbs + commonhandlerpath + '?q=autocompletericerca&r=20&tipologia=' + tipologia + "&lng=" + lng,
        minLength: 6,
        appendTo: '#divAutocomplete2',
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
                $("#" + "hidricercaid").val(ui.item.id);
                $("#txt" + "hidricercaid").text(ui.item.label);
            }
        }
    });
    $("#" + "txt" + "hidricercaid").focus(function () {
        document.getElementById("txt" + "hidricercaid").value = '';
        document.getElementById("hidricercaid").value = '';
    });

    $("#txthidCaratteristica1").autocomplete({
        source: pathAbs + commonhandlerpath + '?q=autocompletecaratteristiche&progressivo=0',
        minLength: 3,
        appendTo: '#divAutocomplete',
        open: function (event, ui) {
            $('.ui-autocomplete').css('position', 'absolute');
            $('.ui-autocomplete').css('z-index', 99999999999999);
            $('.ui-autocomplete').css('top', '0px');
            $('.ui-autocomplete').css('left', '0px');
            setTimeout(function () {
                $('.ui-autocomplete').css('z-index', 99999999999999);
            }, 1);
        },
        select: function (event, ui) {
            if (ui.item != null) {
                $("#hidCaratteristica1").val(ui.item.codice);
                $("#txthidCaratteristica1").text(ui.item.value);
                //location.replace(ui.item.link); //per navigare verso eventuale link
            }
        }
    });

    $("#txthidCaratteristica2").autocomplete({
        source: pathAbs + commonhandlerpath + '?q=autocompletecaratteristiche&progressivo=1',
        minLength: 3,
        appendTo: '#divAutocomplete1',
        open: function (event, ui) {
            $('.ui-autocomplete').css('position', 'absolute');
            $('.ui-autocomplete').css('z-index', 99999999999999);
            $('.ui-autocomplete').css('top', '0px');
            $('.ui-autocomplete').css('left', '0px');
            setTimeout(function () {
                $('.ui-autocomplete').css('z-index', 99999999999999);
            }, 1);
        },
        select: function (event, ui) {
            if (ui.item != null) {
                $("#hidCaratteristica2").val(ui.item.codice);
                $("#txthidCaratteristica2").text(ui.item.value);
            }
        }
    });


    $("#txthidCaratteristica2").keyup(function () {
        if ($(this).val() == "") {
            // input is cleared
            //alert("2 Cleared");
            $("#hidCaratteristica2").val('');
        }
    });
    $("#txthidCaratteristica1").keyup(function () {
        if ($(this).val() == "") {
            $("#hidCaratteristica1").val('');
        }
    });

}
/**************************************************************************************************************************************************************************/
//CHIAMATA DI FILTRAGGIO Riempie objfiltro con le chiavi|valore di filtraggio e lo passa alla funzione per generare l'url di chiamata e poi fa il redirect all'URL generato!
/**************************************************************************************************************************************************************************/
function Visualizzalistadati() {
    var objfiltro = {};
    //emptysession('', function (retval) {  //non serve piu tanto i parametri non son letti per i filtri dalla sessione ma da quelli inseriti nella route

    $(".searchcheck").each(function () {
        var idsel = $(this).attr("id");
        //if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
        if (objfiltro != null && $(this)[0].checked)
            objfiltro[idsel] = $(this)[0].checked;
    });

    $(".searchcalendarrange").each(function () {
        var idsel = $(this).attr("id");
        var startDate = $('#' + idsel + 'startdate').val();
        var endDate = $('#' + idsel + 'enddate').val();
        if (objfiltro != null && startDate != "" && endDate != "")
            objfiltro[idsel] = startDate + "|" + endDate;
    });

    // console.log(retval);
    $(".searchdropdown").each(function () {
        var idsel = $(this).attr("id");
        // if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
        if (objfiltro != null && $(this)[0].value != '')
            objfiltro[idsel] = $(this)[0].value;
    });

    $(".searchpar").each(function () {
        var idsel = $(this).attr("id");
        //if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
        if (objfiltro != null && $(this)[0].value != '')
            objfiltro[idsel] = $(this)[0].value;
    });
    $(".searchval").each(function () {
        var idsel = $(this).attr("id");
        //if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
        if (objfiltro != null && $(this)[0].value != '')
            objfiltro[idsel] = $(this)[0].value;
    });
    $(".searchgeo").each(function () {
        var idsel = $(this).attr("id");
        //if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
        if (objfiltro != null && $(this)[0].value != '')
            objfiltro[idsel] = $(this)[0].value;
    });

    $(".searchjqrange").each(function () {
        var idsel = $(this).attr("id");
        var low = $(this).slider("values", 0);
        var high = $(this).slider("values", 1);

        var min = $(this).slider("option", "min");
        var max = $(this).slider("option", "max");

        var valore = '';
        if ($(this).slider().length > 0 && (low != min || high != max))
            valore = low + "|" + high;
        //if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
        if (objfiltro != null && low != '' && high != '')
            objfiltro[idsel] = valore;
    });

    objfiltro["tipologia"] = tipologia; //memorizzo in ogni caso la tipologia di ricerca che serve anche alla creazione del link url di filtraggio 


    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///VISUALIZZIAMO LA STRINGA DA USARE NELL FUNZIONE DI INJECT debugprametersforinject COME PARAMETRO DI CHIAMATA PER FILTRAGGIO
    var debugprametersforinject = utf8ToB64(JSON.stringify(objfiltro));
    console.log('stringa codificata per filtro objfiltro da usare nelle funzioni inject: ' + debugprametersforinject);
    console.log('reverse da base 64 a utf parsed: ' );
    console.log( JSON.parse(b64ToUtf8(debugprametersforinject)));
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    

    ///////////RICERCA Con link customizzato  )
    //getlinkbyfilters restituisce il link urlrewrited per il redirect // passando un objfiltro serializzato con tipologia,regione,provincia,comune,carartteristica1 ... poi faccio il redirect all'url generato!!!
    var functiontocallonend = makesearch;  //FUNZIONE CHIAMATA ALLA FINE ....
    caricaDatiServerLinkCustom(lng, objfiltro,
        function (result, callafterfilter) {
            try {
                if (result && result != '') {
                    callafterfilter(result);
                }
            }
            catch (e) { console.log(e); console.log(result); }
        },
        functiontocallonend);

    //ALETERNATIVA RICERCA CON SESSION 
    //ESEGUIAMO LA RICERCA E FILTRO USANDO LA SESSIONE PER PASSARE I PARAMETRI ALLA PAGINA QUERY
    //putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) {
    //    openLink(percorsolistaristoranti);
    //});
    //});
}

function makesearch(link) {
    openLink(link);
}

function setclickfocus(idcontrollo) {
    var e = (window.event);
    if (e.keyCode == 13) {
        // alert(e.keyCode);
        $('#' + idcontrollo).click();
        //e.preventDefault();
        //return;
    }
    //    $('#txtSearchImmobili').focus();
}

function resettuttifiltri() {
    FillSearchControls({});
}

/**************************************************************************************************************************************************************************/
//VISUALIZZAZIONE CONTROLLI DI RICERCA e impostazione valori da session objfiltro
/**************************************************************************************************************************************************************************/
function VisualizzaSearchControls() {
    var objfiltro = {};
    var retstring = "";
    getfromsession('objfiltro', function (retval) {  //ricarico dalla sessione i valori di preset delle caselle di filtro
        retstring = retval;
        if (retstring != null && retstring != '')
            objfiltro = JSON.parse(retstring);

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ///VISUALIZZIAMO LA STRINGA DA USARE NELL FUNZIONE DI INJECT debugprametersforinject COME PARAMETRO DI CHIAMATA PER FILTRAGGIO
        var debugprametersforinject = utf8ToB64(JSON.stringify(objfiltro));
        console.log('stringa codificata per filtro objfiltro da usare nelle funzioni inject: ' + debugprametersforinject);
        console.log('reverse da base 64 a utf parsed: ');
        console.log(JSON.parse(b64ToUtf8(debugprametersforinject)));
        ///////////////////////////////////////////////////////////////////////////////////////////////////


        FillSearchControls(objfiltro);//RIcarico e riassegno i valori alle caselle di ricerca
        // $('#divSearchBar').modal('show'); 
    });
}
function FillSearchControls(objfiltro) {

    var objfiltroint = objfiltro || {};
    //console.log(objfiltroint);
    //var message = "";
    //CaricaVariabiliRiferimento(function (result) {
    //    message = result;

    $(".searchreset").each(function () {
        var linkazzera = '<a href="#" onclick="javascript:resettuttifiltri()">Resetta filtri</a>';
        $('#' + $(this).attr('id')).html(linkazzera);
    });

    $(".searchcalendarrange").each(function () {
        renderCalendarControl($(this).attr('id'), objfiltroint);
    });

    //var val1 = "";
    //var val2 = "";
    //var val3 = "";
    //var val4 = "";
    //var val5 = "";
    //if (objfiltroint != null) {
    //    if (objfiltroint.hasOwnProperty('ddlAtcgmp1'))
    //        val1 = objfiltroint['ddlAtcgmp1'];
    //    if (objfiltroint.hasOwnProperty('ddlAtcgmp2'))
    //        val2 = objfiltroint['ddlAtcgmp2'];
    //    if (objfiltroint.hasOwnProperty('ddlAtcgmp3'))
    //        val3 = objfiltroint['ddlAtcgmp3'];
    //    if (objfiltroint.hasOwnProperty('ddlAtcgmp4'))
    //        val4 = objfiltroint['ddlAtcgmp4'];
    //    if (objfiltroint.hasOwnProperty('ddlAtcgmp5'))
    //        val5 = objfiltroint['ddlAtcgmp5'];
    //}
    //combinedselect('', 'all', lng, val1, val2, val3, val4, val5, 'ddlAtcgmp1', 'ddlAtcgmp2', 'ddlAtcgmp3', 'ddlAtcgmp4', 'ddlAtcgmp5');


    $(".searchdropdown").each(function () {
        //try {
        var proprarr = $(this).attr("mybind").split('.'); //Stabilisco il livello di annidamento sull'elemento mybind che dice quale proprietà sto bindando
        var filter = "";//$(this).attr("myfilter");
        if (proprarr != null && proprarr.length != 0)
            switch (proprarr.length) {
                case 1: //Oggetto 1 livello
                    var selectedvalueact = "";
                    var idcontrollo = $(this).attr('id');
                    /*Se passo il filtro presetto i valori*/
                    if (objfiltroint != null && objfiltroint.hasOwnProperty(idcontrollo))
                        selectedvalueact = objfiltroint[idcontrollo];

                    var selstring = '';
                    if (baseresources != null && baseresources.hasOwnProperty(lng) && baseresources[lng].hasOwnProperty("select" + proprarr[0].toLowerCase()))
                        selstring = baseresources[lng]["select" + proprarr[0].toLowerCase()];
                    FillAndSelectRef(proprarr[0], lng, idcontrollo, selstring, '', selectedvalueact, filter);
                    break;//Oggetto di secondo livello non implementato per ora
                case 2:
                    break;
            }
        //} catch (e) { }
    });
    //});

    $(".searchcheck").each(function () {
        try {
            // da fare gestiione checkbox si/no per id="statusconfirmfilter"
            //statusconfirmfilter ....
            var selectedvalueact = "";
            var idcontrollo = $(this).attr('id');
            if (objfiltroint != null && objfiltroint.hasOwnProperty(idcontrollo))
                selectedvalueact = objfiltroint[idcontrollo];
            if (selectedvalueact == "true")
                //$("#" + idcontrollo).attr("checked", true)
                $("#" + idcontrollo).prop("checked", true)
            else
                $("#" + idcontrollo).prop("checked", false)
            //$("#" + idcontrollo).attr("checked", false)
        } catch (e) { }
    });

    /*----------------------------------------------------------------------*/
    //CASELLE RICERCA
    /*----------------------------------------------------------------------*/
    $(".searchval").each(function () {
        try {
            var selectedvalueact = "";
            var idcontrollo = $(this).attr('id');
            if (objfiltroint != null && objfiltroint.hasOwnProperty(idcontrollo))
                selectedvalueact = objfiltroint[idcontrollo];
            var idcontrollotxt = 'txt' + $(this).attr('id');
            $("#" + idcontrollo).val(selectedvalueact);
            $("#" + idcontrollotxt).val(selectedvalueact);

            if (idcontrollo.includes('geolocation'))
                positionfromLatLng(selectedvalueact, idcontrollo);

            if (idcontrollo.includes('Caratteristica1'))
                frmcaratteristica1('', selectedvalueact, '', function (ret) {
                    $("#" + idcontrollotxt).val(ret);
                });
            if (idcontrollo.includes('Caratteristica2'))
                frmcaratteristica2('', selectedvalueact, '', function (ret) {
                    $("#" + idcontrollotxt).val(ret);
                });
            if (idcontrollo.includes('Caratteristica3'))
                frmcaratteristica3('', selectedvalueact, '', function (ret) {
                    $("#" + idcontrollotxt).val(ret);
                });

        } catch (e) { }
    });


    $(".searchjqrange").each(function () {
        try {

            var selectedvalueact = "";
            var idcontrollo = $(this).attr('id');
            if (objfiltroint != null && objfiltroint.hasOwnProperty(idcontrollo))
                selectedvalueact = objfiltroint[idcontrollo];

            InitRangeControl(idcontrollo, function (ret) {
                //Set valori ( vanno presi da selectedvalueact splittando il min e il max)
                var values = selectedvalueact.split('|');
                //sample to set ranges
                if (values.length == 2) {
                    $("#" + idcontrollo).slider("values", 0, values[0]);
                    $("#" + idcontrollo).slider("values", 1, values[1]);
                }
            });
        } catch (e) { }
    });

    /*----------------------------------------------------------------------*/
    //FILL DELLE RICERCHE GEOGRAFICHE 
    /*----------------------------------------------------------------------*/
    var seln = "";
    var selr = ""; //preselezione regione  ....
    var selp = "";
    var selc = "";
    if (objfiltro != null && objfiltro.hasOwnProperty('nazione'))
        seln = objfiltro['nazione'];
    if (objfiltro != null && objfiltro.hasOwnProperty('regione'))
        selr = objfiltro['regione'];
    if (objfiltro != null && objfiltro.hasOwnProperty('provincia'))
        selp = objfiltro['provincia'];
    if (objfiltro != null && objfiltro.hasOwnProperty('comune'))
        selc = objfiltro['comune'];
    (function wait() {
        if (typeof baseresources !== 'undefined' && baseresources != null && baseresources != '') {
            GestioneDdlGeo('all', lng, seln, selr, selp, selc, 'nazione', 'regione', 'provincia', 'comune');

        } else {
            setTimeout(wait, 300);
        }
    })();
    /*----------------------------------------------------------------------*/

    /*----------------------------------------------------------------------*/
    //FILL CATEGORIE E SOTTOCATEGORIE
    /*----------------------------------------------------------------------*/
    var cat1 = "";
    var cat2 = "";
    if (objfiltro != null && objfiltro.hasOwnProperty('categoria'))
        cat1 = objfiltro['categoria'];
    if (objfiltro != null && objfiltro.hasOwnProperty('categoria2Liv'))
        cat2 = objfiltro['categoria2Liv'];
    (function wait() {
        if (typeof baseresources !== 'undefined' && baseresources != null && baseresources != '') {
            combinedselect('', 'all', lng, cat1, cat2, '', '', '', 'categoria', 'categoria2Liv', '', '', '');
        } else {
            setTimeout(wait, 300);
        }
    })();

    /*----------------------------------------------------------------------*/
}

function InitRangeControl(idrangecontrol, callback) {
    var idrangecontrolview = idrangecontrol + "amount";
    var sliderprezzo = $("#" + idrangecontrol).slider({
        range: true,
        min: 0,
        max: 5000,
        step: 50,
        values: [0, 5000],
        slide: function (event, ui) {
            $("#" + idrangecontrolview).val("€" + ui.values[0] + " - €" + ui.values[1]);
        },
        change: function (event, ui) {
            $("#" + idrangecontrolview).val("€" + ui.values[0] + " - €" + ui.values[1]);
        }
    });
    $("#" + idrangecontrolview).val("€" + $("#" + idrangecontrol).slider("values", 0) +
        " - €" + $("#" + idrangecontrol).slider("values", 1));
    callback('');
}


function renderCalendarControl(controlid, objfiltroint) {
    var rangeselected = "";
    var selectedvalueact = '';

    if (objfiltroint != null && objfiltroint.hasOwnProperty(controlid))
        selectedvalueact = objfiltroint[controlid];
    var prvseldate = -1, curseldate = -1;
    var values = selectedvalueact.split('|');
    //sample to set ranges
    if (values.length == 2) {
        var ds = values[0].split('/');
        var de = values[1].split('/');
        if (ds != null && de != null && ds.length == 3 && de.length == 3) {
            prvseldate = (new Date(ds[2], (Number(ds[1])) - 1, ds[0])).getTime();
            curseldate = (new Date(de[2], (Number(de[1])) - 1, de[0])).getTime();
            //Risetto i valori dei campi hidden con i valori memorizzati per la chiamata successiva
            var d1 = $.datepicker.formatDate('dd/mm/yy', new Date(prvseldate), {});
            var d2 = $.datepicker.formatDate('dd/mm/yy', new Date(curseldate), {});
            $('#' + controlid + "startdate").val(d1);
            $('#' + controlid + "enddate").val(d2);
            rangeselected = (d1 + ' - ' + d2);
            $('#' + controlid + "info").html('Filtro attivo: ' + rangeselected);

            var linkazzera = '<a href="#" onclick="javascript:renderCalendarControl(\'' + controlid + '\',\'\')">Azzera filtro</a>';
            $('#' + controlid + "reset").html(linkazzera);
        }
    } else {
        $('#' + controlid + "startdate").val('');
        $('#' + controlid + "enddate").val('');
        $('#' + controlid + "info").html('');
        $("#" + controlid).datepicker('setDate', null);
        $("#" + controlid).datepicker("destroy");
    }

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
            //Evidenzio i giorni tra prv e cur per il range select
            //retvalue = [true, ((date.getTime() >= Math.min(prv, cur) && date.getTime() <= Math.max(prv, cur)) ? 'date-range-selected' : '')];
            //Evidenzio i giorni tra prv e cur per il range select
            retvalue = [elem.stato, ((date.getTime() >= Math.min(prvseldate, curseldate) && date.getTime() <= Math.max(prvseldate, curseldate)) ? elem.Class + ' date-range-selected' : elem.Class + ''), elem.Tooltip];
            return retvalue;
        },
        onSelect: function (dateText, inst) {
            var d1, d2;
            prvseldate = curseldate;
            curseldate = (new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay)).getTime();
            if (prvseldate == -1 || prvseldate == curseldate) {
                prvseldate = curseldate;
                //  rangeselected = (dateText);

                d1 = $.datepicker.formatDate('dd/mm/yy', new Date(Math.min(prvseldate, curseldate)), {});
                d2 = $.datepicker.formatDate('dd/mm/yy', new Date(Math.max(prvseldate, curseldate)), {});
                if (d1 != '' && d2 != '') {
                    //Visualizziamo le date per la selezione e controlliamo i vincoli di selezione
                    rangeselected = (d1 + ' - ' + d2);
                    $('#' + controlid + "startdate").val(d1);
                    $('#' + controlid + "enddate").val(d2);
                    $('#' + controlid + "info").html('Filtro attivo: ' + rangeselected);
                }
                else {
                    $('#' + controlid + "info").html('');
                    $('#' + controlid + "info").attr('class', '');
                    //$('#' + controlid + "messages").html('');
                    //$('#' + controlid + "messages").attr('class', '');
                }
            } else {
                rangeselected = '';
                d1 = $.datepicker.formatDate('dd/mm/yy', new Date(Math.min(prvseldate, curseldate)), {});
                d2 = $.datepicker.formatDate('dd/mm/yy', new Date(Math.max(prvseldate, curseldate)), {});
                //d1 = moment(new Date(Math.min(prv, cur))).format("DD/MM/YYYY HH:mm:ss");// $.datepicker.formatDate('dd/mm/yy', new Date(Math.min(prv, cur)), {});
                //d2 = moment(new Date(Math.max(prv, cur))).format("DD/MM/YYYY HH:mm:ss");//$.datepicker.formatDate('dd/mm/yy', new Date(Math.max(prv, cur)), {});
                if (d1 != '' && d2 != '') {
                    //Visualizziamo le date per la selezione e controlliamo i vincoli di selezione
                    rangeselected = (d1 + ' - ' + d2);
                    $('#' + controlid + "startdate").val(d1);
                    $('#' + controlid + "enddate").val(d2);
                    $('#' + controlid + "info").html('Filtro attivo: ' + rangeselected);
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
        },
        onAfterUpdate: function (inst) {
        },
        beforeShow: function () {
        }
    });
    $("#" + controlid).datepicker($.datepicker.regional["it"]);


    $(window).on('click', function (e) {
        if (document.getElementById(controlid + 'selectdate').contains(e.target)) {
            // Clicked in box
        } else {
            // Clicked outside the box
            $("#" + controlid).hide();
        }
    });
    $("#" + controlid + "selectdate").off('click').on('click', function (e) {
        e.stopPropagation();
        $("#" + controlid).show();
    });

    //$("#" + controlid).datepicker("refresh");
    //$("#" + controlid).datepicker("setDate", $("#" + controlid).datepicker("getDate").toString("yyyy/MM/dd"));
}


function HideSearchControls() {
    //$('#divSearchBar').modal('hide');
}
function OnCloseSearch() {
}
function onchangetest(el) {
    alert($(el).id);
    //var filtervalue = "";
    //if (filterid != null && filterid != "") {
    //    var $select = $('#' + filterid);
    //    if ($select != null)
    //        filtervalue = $select.val()
    //}

}
