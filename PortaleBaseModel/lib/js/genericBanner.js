"use strict";

function injectandloadgenericbanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height) {
    loadref(injectandloadgenericbannerinner, type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height, lng);
}
function injectandloadgenericbannerinner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height) {
    //qui devo visualizzare il titolo
    var templateHtml = pathAbs + "/lib/template/" + "bannerimagefull.html";
    if (type != null && type != '')
        templateHtml = pathAbs + "/lib/template/" + type;

    //Correggo l'id dei controlli del template per l'inzializzazione dello scroller con id univoca e corretta
    $('#' + container).html('');
    $('#' + container).load(templateHtml, function () {
        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });
        var pagerdata = {};
        pagerdata["page"] = page;
        pagerdata["pagesize"] = pagesize;
        pagerdata["totalrecords"] = 0;
        pagerdata["enablepager"] = enablepager;
        pagerdata["pagerconnectedid"] = connectedid;
        globalObject[controlid + "pagerdata"] = pagerdata;

        var params = {};
        params.container = container;/*Inserisco il nome dle container nei parametri per uso successivo nel binding*/
        params.maxelement = maxelement;
        params.listShow = listShow;
        params.tblsezione = tblsezione;
        params.filtrosezione = filtrosezione;
        params.mescola = mescola;
        params.heigth = height;
        params.width = width;

        globalObject[controlid + "params"] = params;

        CaricaDatagenericBanner(controlid);
    });
};
function CaricaDatagenericBanner(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

    var page = globalObject[controlid + "pagerdata"].page;
    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;

    var functiontocallonend = renderGenericBanner;
    //if (enablepager == "true" || enablepager == true)
    //    functiontocallonend = renderIsotopePaged;
    caricaDatiServerBanner(lng, objfiltrotmp, '1', '1', false,
        function (result, callafterfilter) {
            var localObjects = {};

            try {

                if (result !== null && result != '') {
                    var parseddata = JSON.parse(result);
                    var temp = parseddata["resultinfo"];
                    localObjects["resultinfo"] = JSON.parse(temp);
                    var totalrecords = localObjects["resultinfo"].totalrecords;
                    globalObject[controlid + "pagerdata"].totalrecords = totalrecords;
                    var data = "{ \"datalist\":" + parseddata["data"];
                    data += "}";
                    localObjects["dataloaded"] = data;
                    var datalink = parseddata["linkloaded"];  //link creati presi da tabella
                    //Inserisco i valori nella memoria generale che contiene i valori per tutti i componenti
                    // globalObject[controlid] = localObjects;
                    localObjects["linkloaded"] = JSON.parse(datalink);
                    callafterfilter(localObjects, controlid);
                }
            }
            catch (e) {
                //console.log(e);
            }
        },
        functiontocallonend);
};
function renderGenericBanner(localObjects, controlid) {
    bindgenericbanner(controlid, localObjects);//I dati sono già paginati all'origine
};
function bindgenericbanner(el, localObjects) {

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"]
    if (!data.length) {
        $('#' + el).html('');
        return;
    }

    var objfiltrotmp = {};
    objfiltrotmp = globalObject[el + "params"];
    var container = objfiltrotmp.container;

    var str = $($('#' + el)[0]).outerHTML();
    var jquery_obj = $(str);
    jquery_obj = $(jquery_obj);
    var htmlout = "";
    var htmlitem = "";
    $('#' + container).html('');
    for (var j = 0; j < data.length; j++) {
        FillBindControls(jquery_obj.wrap('<p>').parent(), data[j], localObjects, "",
            function (ret) {
                //htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                $('#' + container).append(ret.html()) + "\r\n";
            });
    }
     console.log('banner jaralax inject');
    CleanHtml($('#' + container));
    InitGenericBanner(controlid, container);
}; 

function InitGenericBanner(controlid, container) {
    $('#' + container).show();
    $('#' + container + 'Title').show();
    $(function () {
        $('.mbr-parallax-background').jarallax({
            speed: 0.2
        });
    });
};