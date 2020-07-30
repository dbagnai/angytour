"use strict";

function InjectCat1livLinks(type, container, controlid, listShow, tipologia, categoria, categoria2Liv) {
    loadref(InjectCat1livLinksinner, type, container, controlid, listShow, tipologia, categoria, categoria2Liv, lng);
}
function InjectCat1livLinksinner(type, container, controlid, listShow, tipologia, categoria, categoria2Liv) {
    //qui devo visualizzare il titolo
    var templateHtml = pathAbs + "/lib/template/" + "linkslistddl1.html";
    if (type != null && type != '')
        templateHtml = pathAbs + "/lib/template/" + type;

    //Correggo l'id dei controlli del template per l'inzializzazione dello scroller con id univoca e corretta
    $('#' + container).html('');
    $('#' + container + 'Title').show();
    $('#' + container).load(templateHtml, function () {

        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });

        var params = {};

        //RICARICO LA PAGINA DALLA SESISONE SE PRESENTE
        //getfromsession('objfiltro', function (retval) {
        //    var objfiltro = {};
        //    if (retval != null && retval != '')
        //        objfiltro = JSON.parse(retval);
        //    params = objfiltro;

        params.tipologia = tipologia;
        params.listShow = listShow;
        params.categoria = categoria;
        params.categoria2Liv = categoria2Liv;
        params.container = container;

        globalObject[controlid + "params"] = params;

        (function wait() {
            if (typeof baseresources !== 'undefined' && baseresources != null && baseresources != '') {

                CaricaTipologieLinksData(controlid);
            } else {
                setTimeout(wait, 300);
            }
        })();


   
        //});
    });
}

function CaricaTipologieLinksData(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

    /*MEMORIZZO LA PAGINA IN SESSIONE*/
    //getfromsession('objfiltro', function (retval) {
    //    var objfiltro = {};
    //    if (retval != null && retval != '')
    //        objfiltro = JSON.parse(retval);
    //    putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) { });
    //});

    var functiontocallonend = BindTipologieLinks;

    caricaDatiServerLinksTipologie(lng, objfiltrotmp,
        function (result, callafterfilter) {
            var localObjects = {};

            try {
                var parseddata = JSON.parse(result);
                //console.log(parseddata);

                //var temp = parseddata["resultinfo"];
                //localObjects["resultinfo"] = JSON.parse(temp);

                //var totalrecords = localObjects["resultinfo"].totalrecords;
                //globalObject[controlid + "pagerdata"].totalrecords = totalrecords;

                var data = "{ \"datalist\":" + parseddata["data"];
                data += "}";
                localObjects["dataloaded"] = data;
                //var datalink = parseddata["linkloaded"];  //link creati presi da tabella
                //Inserisco i valori nella memoria generale che contiene i valori per tutti i componenti
                // globalObject[controlid] = localObjects;
                //localObjects["linkloaded"] = JSON.parse(datalink);
                callafterfilter(localObjects, controlid);

            }
            catch (e) { console.log(e); console.log(result); }
        },
        functiontocallonend);
};


//function renderIsotopeNotPaged(localObjects, controlid) {
//    BindIsotope(controlid, localObjects);//I dati sono già paginati all'origine
//};

function BindTipologieLinks(localObjects, el) {

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"]
    if (data == undefined || data == null) return;

    //console.log(data);
    //data = sortobj(data);
    //console.log(data);

    var objFiltroTemp = {};
    objFiltroTemp = globalObject[el + "params"];
    var container = objFiltroTemp.container;

    var str = $('#' + el)[0].innerHTML;  //elemento interno da bindare
    //Se presente nella memoria temporanea globale modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
    //in caso di rebinding successivo dopo l'iniezione del template
    //if (!globalObject.hasOwnProperty(el + "template")) {
    //    globalObject[el + "template"] = $('#' + el)[0].innerHTML;
    //    str = globalObject[el + "template"];
    //}
    //else
    //    str = globalObject[el + "template"];

    try {
        var jquery_obj = $(str);
        var outerhtml = $($('#' + el)[0]).outerHTML();  /*select*/


        var containeritem = jquery_obj.empty();/*optgroup*/
        var containerSelect = $(outerhtml).html(''); /*select vuota*/
        var sel = baseresources[lng]["selectgenericrubrica"];
        //appendo optgroup alla select
        var tempOptionGroup = containeritem.clone();
        tempOptionGroup.attr('label', sel);
        tempOptionGroup.append('<option style="cursor:pointer;" value="">' + sel + '</option>');
        containerSelect.append(tempOptionGroup); //(non metto l'elemento aggiuntivo)
        //////////////////////////////////////

        /////////appendo gli optiongroup degli anni
        //containeritem.empty();
        Object.keys(data).sort().reverse().forEach(function (key) {
            containeritem.attr('label', key);
            var ret = fillDDLArray(containeritem, data[key], "", "", "Campo1", "Campo2");
            containerSelect.append(ret);
        });
        //for (var item in data) {
        //    containeritem.attr('label', item);
        //    var ret = fillDDLArray(containeritem, data[item], "", "", "Campo1", "Campo2");
        //    containerSelect.append(ret);
        //}

        //CleanHtml($('#' + el));
        $('#' + container).html('').show();
        $('#' + container).append(containerSelect);
    }
    catch (e) {
        console.log(e);
    }



};

