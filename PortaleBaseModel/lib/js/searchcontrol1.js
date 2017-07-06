"use strict";



function InitSearchControls() {
    $("#divSearchBarPlaceholder1").load(pathAbs + "/lib/Template/" + "searchbar2.html", function () {
        //Qui puoi fare inizializzazione controlli su allegati
        //FillSearchControls(objfiltro);
        VisualizzaSearchControls();
        initAutocompleteRicercaCaratteristiche();

    });
}
function initAutocompleteRicercaCaratteristiche() {
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
function Visualizzalistadati() {
    var objfiltro = {};
    //  JsSvuotaSession(this);
    emptysession('', function (retval) {
       // console.log(retval);
        $(".searchdropdown").each(function () {
            var idsel = $(this).attr("id");
            // if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
            if (objfiltro != null)
                objfiltro[idsel] = $(this)[0].value;
        });
        $(".searchpar").each(function () {
            var idsel = $(this).attr("id");
            //if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
            if (objfiltro != null)
                objfiltro[idsel] = $(this)[0].value;
        });
        $(".searchval").each(function () {
            var idsel = $(this).attr("id");
            //if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
            if (objfiltro != null)
                objfiltro[idsel] = $(this)[0].value;
        });

        //ESEGUIAMO LA RICERCA E FILTRO
        putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) {

            //console.log("Visualizzalistadati " + JSON.stringify(objfiltro));
            openLink(percorsolistadati);
        });
    });

}

function VisualizzaSearchControls() {
    var objfiltro = {};
    var retstring = "";
    getfromsession('objfiltro', function (retval) {

        retstring = retval;
        if (retstring != null && retstring != '')
            objfiltro = JSON.parse(retstring);
        // console.log("VisualizzaSearchControls " + JSON.stringify(objfiltro));

        FillSearchControls(objfiltro);//RIcarico e riassegno i valori alle caselle di ricerca
        // $('#divSearchBar').modal('show'); 
    });
}
function HideSearchControls() {
    //$('#divSearchBar').modal('hide');
}
function OnCloseSearch() {
}



function FillSearchControls(objfiltro) {
    var objfiltroint = objfiltro || {};
    //console.log(objfiltroint);
    //var message = "";
    //CaricaVariabiliRiferimento(function (result) {
    //    message = result;

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
                    FillAndSelectRef(proprarr[0], lng, idcontrollo, baseresources[lng]["select" + proprarr[0].toLocaleLowerCase()], '', selectedvalueact, filter);
                    break;//Oggetto di secondo livello non implementato per ora
                case 2:
                    break;
            }
        //} catch (e) { }
    });

    //});
    $(".searchval").each(function () {
        try {
            var selectedvalueact = "";
            var idcontrollo = $(this).attr('id');
            if (objfiltroint != null && objfiltroint.hasOwnProperty(idcontrollo))
                selectedvalueact = objfiltroint[idcontrollo];
            var idcontrollotxt = 'txt' + $(this).attr('id');
            $("#" + idcontrollo).val(selectedvalueact);

            if (idcontrollo.includes('Caratteristica1'))
                frmcaratteristica1('', selectedvalueact, '', function (ret) {
                    $("#" + idcontrollotxt).val(ret);
                });
            if (idcontrollo.includes('Caratteristica2'))
                frmcaratteristica2('', selectedvalueact, '', function (ret) {
                    $("#" + idcontrollotxt).val(ret);
                });


        } catch (e) { }

    });
  
    /*----------------------------------------------------------------------*/
    //FILL DELLE RICERCHE GEOGRAFICHE 
    //var seln = "IT";
    //var selp = "";
    //var selr = "";
    //var selc = "";
    //if (objfiltro != null && objfiltro.hasOwnProperty('ddlNazioneSearch'))
    //    seln = objfiltro['ddlNazioneSearch'];
    //if (objfiltro != null && objfiltro.hasOwnProperty('ddlRegioneSearch'))
    //    selr = objfiltro['ddlRegioneSearch'];
    //if (objfiltro != null && objfiltro.hasOwnProperty('ddlProvinciaSearch'))
    //    selp = objfiltro['ddlProvinciaSearch'];
    //if (objfiltro != null && objfiltro.hasOwnProperty('ddlComuneSearch'))
    //    selc = objfiltro['ddlComuneSearch'];
    //GestioneDdlGeo2('all', lng, seln, selr, selp, selc, 'ddlNazioneSearch', 'ddlRegioneSearch', 'ddlProvinciaSearch', 'ddlComuneSearch');
    /*----------------------------------------------------------------------*/
}
function FillAndSelectRef(tableselector, lng, ddlid, selecttext, selectvalue, selectedvalue, filter) {
    var tableselector = tableselector || "";
    var result = "";
    var levelfilter = 0;
    switch (tableselector) {
        case "Caratteristica1":
            result = JSONcar1;
            break;
        case "Caratteristica2":
            result = JSONcar2;
            break;
        case "Caratteristica3":
            result = JSONcar3;
            break;
        case "Categoria":
            result = JSONcategorie;
            break;
        case "Regione":
            result = JSONregioni;
            break;
            //case "dettaglimetrature":
            //    result = JSONrefmetrature;
            //    break;
            //case "dettagliprezzi":
            //    result = JSONrefprezzi;
            //    break;
            //case "dettaglicondizione":
            //    result = JSONrefcondizione;
            //    break;
            //case "dettaglitipocontratto":
            //    result = JSONreftipocontratto;
            //    break;
            //case "dettaglitiporisorse":
            //    result = JSONreftiporisorse;
            //    break;
        case "":
            break;
    }
    var converteddict = convertToDictionaryandFill(result, levelfilter, lng, ddlid, selecttext, selectvalue, selectedvalue, filter);
}

function convertToDictionaryandFill(data, levelfilter, lng, ddlid, selecttext, selectvalue, selectedvalue, filter) {
    var lng = lng || "I";
    var levelfilter = levelfilter || 0;
    //console.log(data);
    var dictobjjs = {};

    for (var j = 0; j < data.length; j++) {
        var codice = data[j].Codice;
        var descrizione = data[j].Campo1;
        dictobjjs[codice] = descrizione;
    }
    //console.log(dictobjjs);
    fillDDL(ddlid, JSON.stringify(dictobjjs), selecttext, selectvalue, selectedvalue);
    return JSON.stringify(dictobjjs);
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


function combinedselect(callerid, command, lng, val1, val2, val3, val4, val5, id1, id2, id3, id4, id5) {
    var id1 = id1 || "ddlAtcgmp1";
    var id2 = id2 || "ddlAtcgmp2";
    var id3 = id3 || "ddlAtcgmp3";
    var id4 = id4 || "ddlAtcgmp4";
    var id5 = id5 || "ddlAtcgmp5";

    if (command == 'all') {
        //Per startup set
        if ($('#' + id1) != null)
            FillAndSelectRef($('#' + id1).attr("mybind"), lng, id1, baseresources[lng]["select" + $('#' + id1).attr("mybind").toLowerCase()], '', val1, "");
        if ($('#' + id2) != null)
            FillAndSelectRef($('#' + id2).attr("mybind"), lng, id2, baseresources[lng]["select" + $('#' + id2).attr("mybind").toLowerCase()], '', val2, val1);
        if ($('#' + id3) != null)
            FillAndSelectRef($('#' + id3).attr("mybind"), lng, id3, baseresources[lng]["select" + $('#' + id3).attr("mybind").toLowerCase()], '', val3, val2);
        if ($('#' + id4) != null)
            FillAndSelectRef($('#' + id4).attr("mybind"), lng, id4, baseresources[lng]["select" + $('#' + id4).attr("mybind").toLowerCase()], '', val4, val3);
        if ($('#' + id5) != null)
            FillAndSelectRef($('#' + id5).attr("mybind"), lng, id5, baseresources[lng]["select" + $('#' + id5).attr("mybind").toLowerCase()], '', val5, val4);
    }
    else {
        var flagcascade = false;
        if ($('#' + id1) != null && $('#' + id1).length > 0)
            if (callerid == id1) {
                var actval = $('#' + id1)[0].value;
                if (flagcascade) actval = '';
                FillAndSelectRef($('#' + id1).attr("mybind"), lng, id1, baseresources[lng]["select" + $('#' + id1).attr("mybind").toLowerCase()], '', actval, "");
                flagcascade = true;
            }
        if ($('#' + id2) != null && $('#' + id2).length > 0)
            if (callerid == id2 || flagcascade) {
                var actval = $('#' + id2)[0].value;
                if (flagcascade) actval = '';
                FillAndSelectRef($('#' + id2).attr("mybind"), lng, id2, baseresources[lng]["select" + $('#' + id2).attr("mybind").toLowerCase()], '', actval, $('#' + id1)[0].value);
                flagcascade = true;
            }
        if ($('#' + id3) != null && $('#' + id3).length > 0)
            if (callerid == id3 || flagcascade) {
                var actval = $('#' + id3)[0].value;
                if (flagcascade) actval = '';
                FillAndSelectRef($('#' + id3).attr("mybind"), lng, id3, baseresources[lng]["select" + $('#' + id3).attr("mybind").toLowerCase()], '', actval, $('#' + id2)[0].value);
                flagcascade = true;
            }
        if ($('#' + id4) != null && $('#' + id4).length > 0)
            if (callerid == id4 || flagcascade) {
                var actval = $('#' + id4)[0].value;
                if (flagcascade) actval = '';
                FillAndSelectRef($('#' + id4).attr("mybind"), lng, id4, baseresources[lng]["select" + $('#' + id4).attr("mybind").toLowerCase()], '', actval, $('#' + id3)[0].value);
                flagcascade = true;
            }
        if ($('#' + id5) != null && $('#' + id5).length > 0)
            if (callerid == id5 || flagcascade) {
                var actval = $('#' + id5)[0].value;
                if (flagcascade) actval = '';
                FillAndSelectRef($('#' + id5).attr("mybind"), lng, id5, baseresources[lng]["select" + $('#' + id5).attr("mybind").toLowerCase()], '', actval, $('#' + id4)[0].value);
                flagcascade = true;
            }

    }
}
