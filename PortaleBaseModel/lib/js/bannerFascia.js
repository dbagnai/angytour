"use strict";
/* Chiamata tipo  injectFasciaAndLoadBanner(id contenitore destinazione, idunico dello scroller , "rif000003", true, true, 6);*/

/*Da vedere dopo la gestione di questi parametri che per adesso metto globali per farli vedere al pager*/
var idcont = null;
//$(window).resize(function () {
//    $('#' + idcont).attr("style", "\"height:auto;\"");
//});

function InjectPagerFasciaBanner(pagercontainer, controlid) {

    var templateHtml = pathAbs + "/lib/template/" + "bannerfascia.html";

    $("#" + pagercontainer).load(templateHtml, function () {
        /*DA COMPLETARE MODIFICANDO NEL PAGER i VALORI IN MODO CHE CHIAMI CON I PARAMETRI GIUSTI IL CARICAMENTO DATI*/
        $('#' + pagercontainer).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });
        initHtmlPager(controlid);
        CaricaFasciaDataBanner(controlid);
    });
}

function injectFasciaAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola) {
    loadref(injectFasciaAndLoadBannerinner, type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, lng);
}
function injectFasciaAndLoadBannerinner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola) {
    idcont = controlid;
    //qui devo visualizzare il titolo
    var templateHtml = pathAbs + "/lib/template/" + "bannerfascia.html";
    if (type != null && type != '')
        templateHtml = pathAbs + "/lib/template/" + type;

    //Correggo l'id dei controlli del template per l'inzializzazione dello scroller con id univoca e corretta
    $('#' + container).html('');
    //$('#' + container + 'Title').show();

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

        globalObject[controlid + "params"] = params;

        if (enablepager == "true" || enablepager == true) {
            var pagercontainer = container + "Pager";
            InjectPagerFasciaBanner(pagercontainer, controlid);
        }
        else {

            CaricaFasciaDataBanner(controlid);
        }
    });
};

function CaricaFasciaDataBanner(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

    var page = globalObject[controlid + "pagerdata"].page;
    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;

    var functiontocallonend = renderFasciaNotPagedBanner;
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
                    //Inserisco i valori nella memoria generale che contiene i valori per tutti i componenti
                    // globalObject[controlid] = localObjects;
                    localObjects["linkloaded"] = JSON.parse(datalink);
                    callafterfilter(localObjects, controlid);
                } else
                    console.log(result);
            }
            catch (e) {
                console.log(e);
            }
        },
        functiontocallonend);
};


function renderFasciaNotPagedBanner(localObjects, controlid) {
    BindFasciaBanner(controlid, localObjects);//I dati sono già paginati all'origine
};
//function renderIsotopePaged(localObjects, controlid) {
//    /*Da ultimare  con la paginazione e sistemare il paginatore   .......
//     * Passando nella renderPager
//     ................... */
//    renderPager(controlid, function (msgpager) {
//        //Renderizziamo l'html dei dati nella pagina
//        BindFasciaBanner(controlid, localObjects);//I dati sono già paginati all'origine dal chiamante
//    });
//}

function BindFasciaBanner(el, localObjects) {

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"];
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[el + "params"];

    if (!data.length) {
        $('#' + el).html('');
        //console.log($('#' + objfiltrotmp.containerid));
        $('#' + objfiltrotmp.containerid).html('');
        return;
    }

    var str = $($('#' + el)[0]).outerHTML();
    //$('#' + el).parent().parent().show();
    //Se presente nella memoria temporanea globale modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
    //in caso di rebinding successivo dopo l'iniezione del template in page
    //if (!globalObject.hasOwnProperty(el + "template")) {
    //    globalObject[el + "template"] = $($('#' + el)[0]).outerHTML();
    //    str = globalObject[el + "template"];
    //}
    //else
    //    str = globalObject[el + "template"];

    var jquery_obj = $(str);
    //var outerhtml = jquery_obj.outerHTML();
    // var innerHtml = jquery_obj.html();
    //var containeritem = outerhtml.replace(innerHtml, '');/*Prendo l'elemento contenitore*/
    jquery_obj = jquery_obj.html();
    jquery_obj = $(jquery_obj);
    var htmlout = "";
    var htmlitem = "";
    $('#' + el).html('');
    for (var j = 0; j < data.length; j++) {
        htmlitem = "";
        //htmlitem = FillBindControls(jquery_obj, data[j]);
        //htmlout += $(containeritem).html(htmlitem.html()).outerHTML() + "\r\n";
        FillBindControls(jquery_obj.wrap('<p>').parent(), data[j], localObjects, "",
            function (ret) {
                //htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                $('#' + el).append(ret.html()) + "\r\n";
            });
    }

    initcycleBanner(el, objfiltrotmp.containerid)

    //jQuery(document).ready(function () {
    //    $('#' + el).parent().imagesLoaded(function () {
    //        $('#' + el).cycle();
    //    });
    //});

    //$(window).load(function () {
    //    $('#' + el).parent().imagesLoaded(function () {
    //        $('#' + el).cycle({ before: onBefore });
    //        console.log('cycle start');
    //    });
    //    console.log('load win');
    //});

    //window.addEventListener("load", function (event) {
    //    console.log('load win');
    //});

    /*Old javascrip  vertical text alignment*/
    //calcAspectRatio(el);
    //$(window).on("orientationchange", function (event) {
    //    calcAspectRatio(el);
    //});

    //$(window).resize(function () {
    //    calcAspectRatio(el);
    //});

    //var counter = 0;
    //var looper = setInterval(function () {
    //    calcAspectRatio(el);
    //    counter++;
    //    if (counter >= 5) {
    //        clearInterval(looper);
    //    };
    //    console.log('recalcsize-ini' + ' ' + el);
    //}, 2000);


    CleanHtml($('#' + el));

};
function initcycleBanner(el, containerid) {

    $('#' + containerid).parent().show();
    $('#' + el).parent().show();
    $('#' + el).on('cycle-update-view', function (e, optionHash, slideOptionsHash, currentSlideEl) {
        var $slide = $(currentSlideEl);
        var h = $slide.outerHeight();
        $slide.parent().css({
            height: h
        });
    });
    setTimeout(function () {
        $('#' + el).parent().imagesLoaded(function () {
            $('#' + el).cycle();
        });
    }, 2000);
}

function calcAspectRatio(el) {
    if (el != null) {
        $('#' + el).parent().show();
        //$('#' + el).parent().attr("style", "display:block;");

        var img = $('#' + el + ' .imgBanner');
        var divtesto = $('#' + el + ' .divTesto');
        var divtesto2 = $('#' + el + ' .divTesto2');

        if ($(window).width() > 1023) {

            divtesto.each(function (index, text) {
                console.log('abefore ' + el + ' imgheight:' + img.height() + ' testoheight:' + ($(this)).parent().outerHeight())
                if (img.height() > 0 && Math.abs(img.height() - ($(this)).parent().outerHeight()) > 10) {
                    if (img.height() > ($(this)).parent().outerHeight()) {
                        console.log('a ' + el + ' imgheight:' + img.height() + ' testoheight:' + ($(this)).parent().outerHeight())
                        //console.log($('#' + el)[0].id + "(true): " + img.height() + " " + ($(this)).parent().outerHeight());
                        var paddvert = (img.height() - ($(this)).parent().outerHeight()) / 2;
                        $(this).css('padding-top', paddvert);
                        $(this).css('padding-bottom', paddvert);
                        if (($(this)).parent().outerHeight() != null && ($(this)).parent().outerHeight() != 0)
                            $(this).parent().parent().parent().parent().css('height', ($(this)).parent().outerHeight());
                        else $(this).parent().parent().parent().parent().css('height', img.height());
                    }
                    else {
                        console.log('b ' + el + 'imgheight:' + img.height() + ' testoheight:' + ($(this)).parent().outerHeight())
                        //console.log($('#' + el)[0].id + "(false): " + img.height() + " " + ($(this)).parent().outerHeight());
                        $(this).css('padding-top', 20);
                        $(this).css('padding-bottom', 20);
                        img.css('height', ($(this)).parent().outerHeight());
                        img.css('width', 'auto');
                        img.css('margin-left', (img.parent().width() - img.width()) / 2);
                        img.css('margin-right', (img.parent().width() - img.width()) / 2);
                        $(this).parent().parent().parent().parent().css('height', ($(this)).parent().outerHeight());
                    }
                }
            });

        } else //In modo mobile xs
        {

            divtesto2.each(function (index, text) {
                if (img.height() > 0) {
                    //console.log($('#' + el)[0].id + "(mobile): " + img.height() + " " + (divtesto).parent().outerHeight());
                    if ($(this).length > 0) {
                        if ((($(this)).children(":first")).length > 0 && ($(this)).children(":first")[0].innerHTML == '')
                            $(this).parent().parent().parent().parent().css('height', img.height());
                        else
                            $(this).parent().parent().parent().parent().css('height', ($(this)).parent().outerHeight() + img.height());
                    }
                    else {
                        if (((divtesto).children(":first")).length > 0 && (divtesto).children(":first")[0].innerHTML == '')
                            divtesto.parent().parent().parent().parent().css('height', img.height());
                        else
                            divtesto.parent().parent().parent().parent().css('height', (divtesto).parent().outerHeight() + img.height());
                    }
                }
            });

        }



    }
};
