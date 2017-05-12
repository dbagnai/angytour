"use strict";
var sliderPresent = false;
/* Chiamata tipo  injectSliderAndLoadBanner(id contenitore destinazione, idunico dello scroller , "rif000003", true, true, 6);*/

/*Da vedere dopo la gestione di questi parametri che per adesso metto globali per farli vedere al pager*/
 
function InjectPagerSliderBanner(pagercontainer, controlid) {
    var templateHtml = pathAbs + "/lib/template/" + "sliderbanner.html";
    $("#" + pagercontainer).load(templateHtml, function () {
        /*DA COMPLETARE MODIFICANDO NEL PAGER i VALORI IN MODO CHE CHIAMI CON I PARAMETRI GIUSTI IL CARICAMENTO DATI*/
        $('#' + pagercontainer).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });
        initHtmlPager(controlid);
        CaricaSliderDataBanner(controlid);
    });
}

function injectSliderAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height) {
    loadref(injectSliderAndLoadBannerinner, type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height, lng);
}
function injectSliderAndLoadBannerinner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola,width,height) {
    //qui devo visualizzare il titolo
    var templateHtml = pathAbs + "/lib/template/" + "sliderbanner.html";
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
        //------------------------------------------------------------------------------------
        //InitIsotopeLocalBanner(controlid);
        //------------------------------------------------------------------------------------
        //Usiamo memoria globale indicizzata con l'id del controllo
        var pagerdata = {};
        pagerdata["page"] = page;
        pagerdata["pagesize"] = pagesize;
        pagerdata["totalrecords"] = 0;
        pagerdata["enablepager"] = enablepager;
        pagerdata["pagerconnectedid"] = connectedid;
        globalObject[controlid + "pagerdata"] = pagerdata;

        var params = {};
        params.containerid = container;
        params.maxelement = maxelement;
        params.listShow = listShow;
        params.tblsezione = tblsezione;
        params.filtrosezione = filtrosezione;
        params.mescola = mescola;
        params.heigth = height;
        params.width = width;

        globalObject[controlid + "params"] = params;

        if (enablepager == "true" || enablepager == true) {
            var pagercontainer = container + "Pager";
            InjectPagerSliderBanner(pagercontainer, controlid);
        }
        else {

            CaricaSliderDataBanner(controlid);
        }
    });
};

function CaricaSliderDataBanner(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

    var page = globalObject[controlid + "pagerdata"].page;
    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;

    var functiontocallonend = renderSliderNotPagedBanner;
    //if (enablepager == "true" || enablepager == true)
    //    functiontocallonend = renderIsotopePaged;
    caricaDatiServerBanner(lng, objfiltrotmp, page, pagesize, enablepager,
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
                console.log(e);
            }
        },
        functiontocallonend);
};


function renderSliderNotPagedBanner(localObjects, controlid) {
    BindSliderBanner(controlid, localObjects);//I dati sono già paginati all'origine
};
//function renderIsotopePaged(localObjects, controlid) {
//    /*Da ultimare  con la paginazione e sistemare il paginatore   .......
//     * Passando nella renderPager
//     ................... */
//    renderPager(controlid, function (msgpager) {
//        //Renderizziamo l'html dei dati nella pagina
//        BindSliderBanner(controlid, localObjects);//I dati sono già paginati all'origine dal chiamante
//    });
//}

function BindSliderBanner(el, localObjects) {

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"]
    if (!data.length)
        return;

    var objfiltrotmp = {};
    objfiltrotmp = globalObject[el + "params"];
    //objfiltrotmp.containerId

    /*-----------------------*/
    sliderPresent = true;
    //$('#menuzord').addClass('white');
    //$('#menuzord').removeClass('dark');
    //$('#divlogo').addClass('fulllogobck');
    //$('#divlogo').removeClass('fulllogobckdark');
    //$('#VerticalSpacer').attr("style", "height:70px");
    /*-----------------------*/

    var str = $($('#' + el)[0]).outerHTML();
    //$('#' + el).parent().parent().show();

    var jquery_obj = $(str).html();
    //var outerhtml = jquery_obj.outerHTML();
    // var innerHtml = jquery_obj.html();
    //var containeritem = outerhtml.replace(innerHtml, '');/*Prendo l'elemento contenitore*/
    jquery_obj = $(jquery_obj).html();
    jquery_obj = $(jquery_obj);
    var htmlout = "";
    var htmlitem = "";
    var controlDiv = $($('#' + el + ' ul')[0]).html('');
    //controlDiv.empty();
    for (var j = 0; j < data.length; j++) {
        htmlitem = "";
        //htmlitem = FillBindControls(jquery_obj, data[j]);
        //htmlout += $(containeritem).html(htmlitem.html()).outerHTML() + "\r\n";
        FillBindControls(jquery_obj.wrap('<p>').parent(), data[j], localObjects, "",
                    function (ret) {
                        //htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                        var test = $('#' + el + ' ul')[0];
                        $(test).append(ret.html()) + "\r\n";
                    });
    }

    initSlider(el, objfiltrotmp.width, objfiltrotmp.heigth);

    CleanHtml($('#' + el));

};

function initSlider(idDiv, width, height) {
    jQuery(document).ready(function ($) {
        $('#' + idDiv).parent().show();
       
        $('.rev-slider-fixed,.rev-slider-full').css('visibility', 'visible');
        $('#' + idDiv).css('visibility', 'visible');
        $('#' + idDiv).revolution({
            delay: 5000,
            startwidth: width,
            startheight: height,
            //startwidth: 1170,
            //startheight: 550,
            onHoverStop: "on",
            thumbWidth: 100,
            thumbHeight: 50,
            thumbAmount: 3,
            hideThumbs: 0,
            navigationType: "bullet",// use none, bullet or thumb
            navigationArrows: "none", // nextto, solo, none, nextto questo per le frecce laterali
            navigationStyle: "square",  // round, square, navbar, round-old, square-old, navbar-old 
            navigationHAlign: "center",
            navigationVAlign: "bottom",
            navigationHOffset: 30,
            navigationVOffset: 20,
            soloArrowLeftHalign: "left",
            soloArrowLeftValign: "center",
            soloArrowLeftHOffset: 20,
            soloArrowLeftVOffset: 0,
            soloArrowRightHalign: "right",
            soloArrowRightValign: "center",
            soloArrowRightHOffset: 20,
            soloArrowRightVOffset: 0,
            touchenabled: "on",
            stopAtSlide: -1,
            stopAfterLoops: -1,
            hideCaptionAtLimit: 0,
            hideAllCaptionAtLilmit: 0,
            hideSliderAtLimit: 0,
            fullWidth: "on",
            fullScreen: "off",
            autoHeight: "off",
            fullScreenOffsetContainer: "#topheader-to-offset",
            lazyLoad: "on",
            keyboardNavigation: "on",
            shadow: 0
        });
    });
}


