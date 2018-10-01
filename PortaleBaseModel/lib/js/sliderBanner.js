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
function injectSliderAndLoadBannerinner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, width, height) {
    //Qui devo visualizzare il titolo
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
        params.container = container;
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

                if (result !== null && result != '' && callafterfilter != null) {
                    var parseddata = JSON.parse(result);

                    var temp = parseddata["resultinfo"];
                    localObjects["resultinfo"] = JSON.parse(temp);

                    var totalrecords = localObjects["resultinfo"].totalrecords;
                    globalObject[controlid + "pagerdata"].totalrecords = totalrecords;

                    var data = "{ \"datalist\":" + parseddata["data"];
                    data += "}";
                    localObjects["dataloaded"] = data;

                    var datalink = parseddata["linkloaded"];  //link creati presi da tabella
                    localObjects["linkloaded"] = JSON.parse(datalink);

                    callafterfilter(controlid, localObjects);
                } else
                    console.log(result);
            }
            catch (e) {
                console.log(e);
            }
        },
        functiontocallonend);
};


function renderSliderNotPagedBanner(controlid, localObjects) {
    BindSliderBanner(controlid, localObjects);//I dati sono già paginati all'origine
};
//function renderIsotopePaged(controlid, localObjects) {
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
    //objfiltrotmp.container
     //$('#' + objfiltrotmp.container).parent().show(); //Visualizzo il contenitore se era spento

    /*-----------------------*/
    sliderPresent = true;
    //$('#menuzord').addClass('white');
    //$('#menuzord').removeClass('dark');
    //$('#divlogo').addClass('fulllogobck');
    //$('#divlogo').removeClass('fulllogobckdark');
    //$('#VerticalSpacer').attr("style", "height:70px");
    /*-----------------------*/

    var str = $($('#' + el)[0]).outerHTML();
    var jquery_obj = $(str).html();
    jquery_obj = $(jquery_obj).html();
    jquery_obj = $(jquery_obj); //elemento li
    var controlDiv = $($('#' + el + ' ul')[0]).html(''); //Vuoto i valori presenti prima di aggiungerne
    for (var j = 0; j < data.length; j++) {
        FillBindControls(jquery_obj.wrap('<p>').parent(), data[j], localObjects, "",
            function (ret) {
                var test = $('#' + el + ' ul')[0];
                $(test).append(ret.html()) + "\r\n";
            });
    }

    initSlider(el, objfiltrotmp.container, objfiltrotmp.width, objfiltrotmp.heigth);
    CleanHtml($('#' + el));
    CleanHtml($('#' + el).parent());
};

//https://www.themepunch.com/faq/wrap-lines-of-text-jquery-version/
function initSlider(idDiv, idContainer, width, height) {
    // jQuery(document).ready(function ($) {

    //$('#' + idDiv).parent().attr('style', ';height:calc(100vh - 141px) !important;');
    $('#' + idContainer).parent().show();
    $('#' + idContainer).show();
    //$('#' + idDiv).parent().show();
    $('#' + idDiv).show();

    $('.rev-slider-fixed,.rev-slider-full').css('visibility', 'visible');
    $('#' + idDiv).css('visibility', 'visible');
    $('#' + idDiv).revolution({
        jsFileLocation: "/revolution464/",
        delay: 5000,
        startwidth: width,
        startheight: height,
        onHoverStop: "on",
        thumbWidth: 80,
        thumbHeight: 50,
        thumbAmount: 3,
        hideThumbs: 200,
        navigationType: "thumb",// use none, bullet or thumb
        navigationArrows: "solo", // nextto, solo, none, nextto questo per le frecce laterali
        navigationStyle: "square",  // round, square, navbar, round-old, square-old, navbar-old 
        navigationHAlign: "center",
        navigationVAlign: "bottom",
        navigationHOffset: 0,
        navigationVOffset: 5,
        soloArrowLeftHalign: "right",
        soloArrowLeftValign: "bottom",
        soloArrowLeftHOffset: 60,
        soloArrowLeftVOffset: 10,
        soloArrowRightHalign: "right",
        soloArrowRightValign: "bottom",
        soloArrowRightHOffset: 10,
        soloArrowRightVOffset: 10,
        touchenabled: "on",
        stopAtSlide: -1,
        stopAfterLoops: -1,
        hideCaptionAtLimit: 0,
        hideAllCaptionAtLimit: 0,
        hideSliderAtLimit: 0,
        fullWidth: "on",
        fullScreen: "on",// fullScreen: fullscreen,
        fullScreenAlignForce: "on",
        autoHeight: "on",
        fullScreenOffsetContainer: "#topheader-to-offset",
        lazyLoad: "on",
        keyboardNavigation: "on",
        shadow: 0
    });


    // });
}


