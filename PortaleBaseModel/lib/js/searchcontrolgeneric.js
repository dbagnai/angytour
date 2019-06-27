"use strict";



function InitSearchControls(idcontainer) {
    var idcontainer = idcontainer || "divSearchBarPlaceholder1";
    if (tipologia == "") tipologia = "rif000003";
    $("#" + idcontainer).load(pathAbs + "/lib/Template/" + "searchbar2.html", function () {
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
        objfiltro["tipologia"] = tipologia; //memorizzo in ogni caso la tipologia di ricerca 

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
        $(".searchgeo").each(function () {
            var idsel = $(this).attr("id");
            //if (objfiltro != null && objfiltro.hasOwnProperty(idsel))
            if (objfiltro != null)
                objfiltro[idsel] = $(this)[0].value;
        });



        ///////////RICERCA Con link customizzato /DA TESTARE)
        //getlinkbyfilters da creare la call per avere il link per il redirect // passando un kbjfiltro serializzato con tipologia,regione,provincia,comune,carartteristica1 e fare il redirect!!!
        //objfiltro["tipologia"] = tipologia; //serve alla creazione del link
        //var functiontocallonend = makesearch;
        //caricaDatiServerLinkCustom(lng, objfiltro,
        //    function (result, callafterfilter) {
        //        try {
        //            if (result && result != '') {

        //                callafterfilter(result);
        //            }

        //        }
        //        catch (e) { console.log(e); console.log(result); }
        //    },
        //    functiontocallonend);

        //ALETERNATIVA RICERCA CON SESSION
        //ESEGUIAMO LA RICERCA E FILTRO USANDO LA SESSIONE PER PASSARE I PARAMETRI ALLA PAGINA QUERY
        putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) {
            openLink(percorsolistaristoranti);
        });
    });

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


        } catch (e) { }

    });

    /*----------------------------------------------------------------------*/
    //FILL DELLE RICERCHE GEOGRAFICHE 
    var seln = "IT";
    var selr = "p94"; //preselezione regione  ....
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


function onchangetest(el) {
    alert($(el).id);
    //var filtervalue = "";
    //if (filterid != null && filterid != "") {
    //    var $select = $('#' + filterid);
    //    if ($select != null)
    //        filtervalue = $select.val()
    //}

}
