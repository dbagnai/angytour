"use strict";

function injectandloadgenericvideo(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height) {
    loadref(injectandloadgenericvideoinner, type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height, lng);
}
function injectandloadgenericvideoinner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height) {
    //qui devo visualizzare il titolo
    var templateHtml = pathAbs + "/lib/template/" + "bannervideo.html";
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
        //replace all occurence of replaceid
        $('#' + container).html($('#' + container).html().replace(/replaceid/g, controlid));

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
function bindgenericbanner(controlid, localObjects) {

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"]
    if (!data.length) {
        $('#' + controlid).html('');
        return;
    }

    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];
    var container = objfiltrotmp.container;
    //var width = objfiltrotmp.width;
    //var height = objfiltrotmp.height;


    var str = $($('#' + controlid)[0]).outerHTML();
    var jquery_obj = $(str);
    jquery_obj = $(jquery_obj);
    $('#' + container).html('');
    for (var j = 0; j < data.length; j++) {
        if (j > 0) break; //Per ora permetto un solo video
        FillBindControls(jquery_obj.wrap('<p>').parent(), data[j], localObjects, "",
            function (ret) {
                //htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                $('#' + container).append(ret.html()) + "\r\n";
            });
    }
    CleanHtml($('#' + container));
    console.log('video inject');

    InitVideo(controlid, container); //inzializzo il video
};

function InitVideo(controlid, container) {

    $('#' + container).show();
    $('#' + container + 'Title').show();
    $(function () {
        //var muteval = false;
        //jQuery("#" + controlid + "togglevol").addClass('btn-vol-on');
        var muteval = true;
        jQuery("#" + controlid + "togglevol").addClass('btn-vol-on');
        jQuery("#" + controlid + "togglevol").toggleClass('btn-vol-on');
        var autoplay = false;
        jQuery("#" + controlid + "toggleplay").addClass('btn-play-on');

        var volev = 50;
        var w = $(window).width();
        if (w <= 768) {
            muteval = true;
            autoplay = false;
            volev = 0;
            jQuery("#" + controlid + "togglevol").toggleClass('btn-vol-on');
        }


        var options = {
            // mobileFallbackImage: "http://www.hdwallpapers.in/walls/pink_cosmos_flowers-wide.jpg",
            playOnlyIfVisible: false,
            mute: muteval,
            autoPlay: autoplay,
            vol: volev
        };

        //var options = {
        //    // mobileFallbackImage: "http://www.hdwallpapers.in/walls/pink_cosmos_flowers-wide.jpg",
        //    playOnlyIfVisible: false 
        //};

        //myPlayer = jQuery(".player").YTPlayer(options);
        var myPlayer = jQuery("#" + controlid).YTPlayer(options);

        //var myPlayer = jQuery(".player").YTPlayer({
        //    onReady: function (player) {
        //    }
        //});
    });

};