"use strict";



function InitSearchControls() {
    //console.log('initserach');
    var objfiltro = {};
    var retstring = "";
    getfromsession('objfiltro', function (retval) {
        retstring = retval;
        if (retstring != null && retstring != '')
            objfiltro = JSON.parse(retstring);
        $("#divSearchBarPlaceholder").load(pathAbs  + "/lib/Template/" + "searchbar.html", function () {
            //Qui puoi fare inizializzazione controlli su allegati
            FillSearchControls(objfiltro);
        });
    });
}

//Eseguita alla pressione del tasto ricerca
function Visualizzalistaimmobili() {
    var objfiltro = {};
    $(".searchdropdown").each(function () {
        var idsel = $(this).attr("id");
        objfiltro[idsel] = $(this)[0].value;
    });
    objfiltro["ddlNazioneSearch"] = $("#ddlNazioneSearch")[0].value;
    objfiltro["ddlRegioneSearch"] = $("#ddlRegioneSearch")[0].value;
    objfiltro["ddlProvinciaSearch"] = $("#ddlProvinciaSearch")[0].value;
    objfiltro["ddlComuneSearch"] = $("#ddlComuneSearch")[0].value;

    setTimeout(function () { 
        putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) {
            if (typeof filtraarchivioimmobili == 'function' && $("#ullist1").length != 0)
                filtraarchivioimmobili();
            else {
                openLink(percorsolistaimmobili);
            }
        })  
    }, 100);
}


function VisualizzaSearchControls() {
    var objfiltro = {};
    var retstring = "";
    getfromsession('objfiltro', function (retval) {
        retstring = retval;
        if (retstring != null && retstring != '')
            objfiltro = JSON.parse(retstring);
        //$("#divSearchBarPlaceholder").load(pathAbs + "/lib/Template/" + "searchbar.html", function () {
        //    FillSearchControls(objfiltro);
        //});
        FillSearchControls(objfiltro);//RIcarico e riassegno i valori alle caselle di ricerca
        $('#divSearchBar').modal('show');

    });
}
function HideSearchControls() {
    $('#divSearchBar').modal('hide');
}
function OnCloseSearch() {
}

function FillAndSelectRef(tableselector, lng, ddlid, selecttext, selectvalue, selectedvalue) {
    var tableselector = tableselector || "dettaglimetrature";
    var result = "";
    switch (tableselector) {
        case "dettaglimetrature":
            result = JSONrefmetrature;
            break;
        case "dettagliprezzi":
            result = JSONrefprezzi;
            break;
        case "dettaglicondizione":
            result = JSONrefcondizione;
            break;
        case "dettaglitipocontratto":
            result = JSONreftipocontratto;
            break;
        case "dettaglitiporisorse":
            result = JSONreftiporisorse;
            break;
    }
    var converteddict = convertToDictionaryandFill(result, tableselector, lng, ddlid, selecttext, selectvalue, selectedvalue);
}

function convertToDictionaryandFill(data, tableselector, lng, ddlid, selecttext, selectvalue, selectedvalue) {
    var lng = lng || "I";
    //console.log(data);
    var dictobjjs = {};
    for (var j = 0; j < data.length; j++) {
        var id = data[j].id;

        //METODO DEFIANTJS
        //var xpath = '//' + detailcollection + '[lingua="' + lng + '"]/titolo';
        //var titolo = JSON.search(data[j], xpath);
        //xpath = '//' + detailcollection + '[lingua="GB"]/titolo';
        //if (titolo == null || titolo == '')
        //    titolo = JSON.search(data[j], xpath); //Default inglese
        //xpath = '//' + detailcollection + '[lingua="I"]/titolo';
        //if (titolo == null || titolo == '')
        //    titolo = JSON.search(data[j], xpath); //a seguire italiano
        //if (titolo != null && titolo.length > 0)
        //    dictobjjs[id] = titolo[0];

        //METODO JAVASCRIPT
        var titolo = '';
        for (var i = 0; i < data[j][tableselector].length; i++) {
            if (data[j][tableselector][i].lingua == lng)
                titolo = data[j][tableselector][i].titolo
            if (data[j][tableselector][i].lingua == "I" && titolo == '')
                titolo = data[j][tableselector][i].titolo
        }
        dictobjjs[id] = titolo;
    }
    //console.log(dictobjjs);
    fillDDL(ddlid, JSON.stringify(dictobjjs), selecttext, selectvalue, selectedvalue);
    return JSON.stringify(dictobjjs);
}



function FillSearchControls(objfiltro) {
    var objfiltroint = objfiltro || {};
    //console.log(objfiltroint);
    //var message = "";
    //CaricaVariabiliRiferimento(function (result) {
    //    message = result;
        $(".searchdropdown").each(function () {
            //try {
            var proprarr = $(this).attr("mybind").split('.'); //Stabilisco il livello di annidamento sull'elemento mybind che dice quale proprietà sto bindando
            if (proprarr != null && proprarr.length != 0)
                switch (proprarr.length) {
                    case 1: //Oggetto 1 livello
                        var selectedvalueact = "";
                        var idcontrollo = $(this).attr('id');
                        /*Se passo il filtro presetto i valori*/
                        if (objfiltroint != null && objfiltroint.hasOwnProperty(idcontrollo))
                            selectedvalueact = objfiltroint[idcontrollo];
                        FillAndSelectRef(proprarr[0], lng, idcontrollo, baseresources[lng]["select" + proprarr[0]], '', selectedvalueact);
                        break;//Oggetto di secondo livello non implementato per ora
                    case 2:
                        break;
                }
            //} catch (e) { }
        });
    //});
    /*----------------------------------------------------------------------*/
    //FILL DELLE RICERCHE GEOGRAFICHE 
    var seln = "IT";
    var selp = "";
    var selr = "";
    var selc = "";
    if (objfiltro != null && objfiltro.hasOwnProperty('ddlNazioneSearch'))
        seln = objfiltro['ddlNazioneSearch'];
    if (objfiltro != null && objfiltro.hasOwnProperty('ddlRegioneSearch'))
        selr = objfiltro['ddlRegioneSearch'];
    if (objfiltro != null && objfiltro.hasOwnProperty('ddlProvinciaSearch'))
        selp = objfiltro['ddlProvinciaSearch'];
    if (objfiltro != null && objfiltro.hasOwnProperty('ddlComuneSearch'))
        selc = objfiltro['ddlComuneSearch'];
    GestioneDdlGeo('all', lng, seln, selr, selp, selc, 'ddlNazioneSearch', 'ddlRegioneSearch', 'ddlProvinciaSearch', 'ddlComuneSearch');
    /*----------------------------------------------------------------------*/
}



/*---------CARICA I DATI DI RIFERIMENTO PER LE RICERCHE( MODIFICATO CARICANDO DALLA MEMORIA STATICA SUL SERVER!! usando references )---------------------------*/
//function caricajsonmetrature() {
//    // console.log(percorsoexp + 'refmetrature.json');
//    JSONrefmetrature = (function () {
//        var JSONrefmetrature = null;
//        $.ajax({
//            url: pathAbs + percorsoexp + 'refmetrature.json',
//            //contentType: "application/json; charset=utf-8",
//            global: false,
//            async: false,
//            cache: false,
//            dataType: "text",
//            success: function (data) {
//                JSONrefmetrature = JSON.parse(data);
//                //console.log(datamod);
//            },
//            failure: function (result) {
//                sendmessage('fail init metrature')
//            }
//        });
//        return JSONrefmetrature;
//    })();
//}
//function caricajsonprezzo() {
//    JSONrefprezzi = (function () {
//        var JSONrefprezzi = null;
//        $.ajax({
//            url: pathAbs + percorsoexp + 'refprezzi.json',
//            //contentType: "application/json; charset=utf-8",
//            global: false,
//            async: false,
//            cache: false,
//            dataType: "text",
//            success: function (data) {
//                JSONrefprezzi = JSON.parse(data);
//                //console.log(datamod);
//            },
//            failure: function (result) {
//                sendmessage('fail init prezzi')
//            }
//        });
//        return JSONrefprezzi;
//    })();
//}
//function caricajsoncondizione() {
//    JSONrefcondizione = (function () {
//        var JSONrefcondizione = null;
//        $.ajax({
//            url: pathAbs + percorsoexp + 'refcondizione.json',
//            //contentType: "application/json; charset=utf-8",
//            global: false,
//            async: false,
//            cache: false,
//            dataType: "text",
//            success: function (data) {
//                JSONrefcondizione = JSON.parse(data);
//                //console.log(datamod);
//            },
//            failure: function (result) {
//                sendmessage('fail init condizione')
//            }
//        });
//        return JSONrefcondizione;
//    })();
//}
//function caricajsontipocontratto() {
//    JSONreftipocontratto = (function () {
//        var JSONreftipocontratto = null;
//        $.ajax({
//            url: pathAbs + percorsoexp + 'reftipocontratto.json',
//            //contentType: "application/json; charset=utf-8",
//            global: false,
//            async: false,
//            cache: false,
//            dataType: "text",
//            success: function (data) {
//                JSONreftipocontratto = JSON.parse(data);
//                //console.log(datamod);
//            },
//            failure: function (result) {
//                sendmessage('fail init contratto')
//            }
//        });
//        return JSONreftipocontratto;
//    })();
//}
//function caricajsontiporisorse() {
//    JSONreftiporisorse = (function () {
//        var JSONreftiporisorse = null;
//        $.ajax({
//            url: pathAbs + percorsoexp + 'reftiporisorse.json',
//            //contentType: "application/json; charset=utf-8",
//            global: false,
//            async: false,
//            cache: false,
//            dataType: "text",
//            success: function (data) {
//                JSONreftiporisorse = JSON.parse(data);
//                //console.log(datamod);
//            },
//            failure: function (result) {
//                sendmessage('fail init risorse')
//            }
//        });
//        return JSONreftiporisorse;
//    })();
//}
/*--------- FINE CARICA I DATI DI RIFERIMENTO PER LE RICERCHE( MODIFICATO CARICANDO DALLA MEMORIA STATICA SUL SERVER!! usando references )---------------------------*/

