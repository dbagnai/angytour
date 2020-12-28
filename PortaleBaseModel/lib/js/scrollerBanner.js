"use strict";


function injectScrollerAndLoadBanner(type, container, controlid, listShow, maxelement, scrollertype, tblsezione, filtrosezione, mescola) {
    loadref(injectScrollerAndLoadBannerinner, type, container, controlid, listShow, maxelement, scrollertype, tblsezione, filtrosezione, mescola, lng);
}
function injectScrollerAndLoadBannerinner(type, container, controlid, listShow, maxelement, scrollertype, tblsezione, filtrosezione, mescola) {

    var templateHtml = pathAbs + "/lib/template/" + "owlscrollerOfferte.html";
    if (type != null && type != '')
        templateHtml = pathAbs + "/lib/template/" + type;

    //Correggo l'id dei controlli del template per l'inzializzazione dello scroller univoca e corretta
    $('#' + container).html('');
    //$('#' + container + 'Title').show();
    $('#' + container).load(templateHtml, function () {
        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });


        var params = {};
        params.container = container;/*Inserisco il nome dle container nei parametri per uso successivo nel binding*/
        params.maxelement = maxelement;
        params.listShow = listShow;
        params.scrollertype = scrollertype;
        params.tblsezione = tblsezione;
        params.filtrosezione = filtrosezione;
        params.mescola = mescola;
        globalObject[controlid + "params"] = params;

        //Qui puoi fare inizializzazione controlli su allegati
        CaricaScrollerBanner(controlid);

    });
};

function CaricaScrollerBanner(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

    caricaDatiServerBanner(lng, objfiltrotmp, 1, 20, false,
        function (result, callafterfilter) {
            var localObjects = {};

            try {
                var parseddata = JSON.parse(result);
                var temp = parseddata["resultinfo"];
                localObjects["resultinfo"] = JSON.parse(temp);

                //totalrecords = localObjects["resultinfo"].totalrecords;

                var data = "{ \"datalist\":" + parseddata["data"];
                data += "}";
                localObjects["dataloaded"] = data;
                var datalink = parseddata["linkloaded"];  //link creati presi da tabella
                //Inserisco i valori nella memoria generale che contiene i valori per tutti i componenti
                // globalObject[controlid] = localObjects;
                localObjects["linkloaded"] = JSON.parse(datalink);
                callafterfilter(localObjects, controlid);

            }
            catch (e) { }
        },
        renderScrollerListBanner);
};



function renderScrollerListBanner(localObjects, controlid) {
    //console.log("---------RENDERSCROLLER");
    //console.log(localObjects);
    /* Prendo i primi end oggetti dalla lista completa*/
    BindScrollerBanner(controlid, localObjects);//I dati sono già paginati all'origine
};

function BindScrollerBanner(el, localObjects) {
    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"]
    if (!data.length) {
        $('#' + el).html('');
        return;
    }
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[el + "params"];
    var container = objfiltrotmp.container;
    var str = $('#' + el)[0].innerHTML;
    //Se presente nella memoria temporanea globale modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
    //in caso di rebinding successivo dopo l'iniezione del template
    if (!globalObject.hasOwnProperty(el + "template")) {
        globalObject[el + "template"] = $('#' + el)[0].innerHTML;
        str = globalObject[el + "template"];
    }
    else
        str = globalObject[el + "template"];
    var jquery_obj = $(str);
    var innerHtml = jquery_obj.html();
    var outerhtml = jquery_obj.outerHTML();
    var containeritem = outerhtml.replace(innerHtml, '');/*Prendo l'elemento contenitore*/
    var htmlout = "";
    for (var j = 0; j < data.length; j++) {
        //htmlitem = FillBindControls(jquery_obj, data[j]);
        //htmlout += $(containeritem).html(htmlitem.html()).outerHTML() + "\r\n";
        FillBindControls(jquery_obj, data[j], localObjects, "",
            function (ret) {
                htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
            });
    }
    //$('#' + el).parent().parent().parent().parent().parent().show();
    //Inseriamo htmlout nel contenitore  $('#' + el).html e inizializziamo lo scroller
    $('#' + el).html('');
    $('#' + el).html(htmlout);
    CleanHtml($('#' + el));

    $('#' + container).parent().show();
    initScrollerBanner(el, globalObject[el + "params"].scrollertype);
};

function initScrollerBanner(el, type) {
    setTimeout(function () {
        var scrollertype = type;
        //console.log(scrollertype);
        switch (scrollertype) {
            case "1":
                ScrollerInitBanner1(el);
                break;
            case "2":
                ScrollerInitBanner2(el);
                break;
            case "3":
                ScrollerInitBanner3(el);
                break;
            case "4":
                ScrollerInitBanner4(el);
                break;
            case "5":
                ScrollerInitBanner5(el);
                break;
            default:
                ScrollerInitBanner(el);
                break;
        }
    }, 100);
}

function ScrollerInitBanner(controlid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + controlid);
        owl.owlCarousel({
            items: [4],
            //autoPlay: 5000,
            itemsDesktop: [1199, 3], // i/tems between 1000px and 601px
            itemsTablet: [979, 2], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000,
            afterInit: lazyLoad,
            afterMove: lazyLoad
        });

        // Custom Navigation Events
        jQuery("#" + controlid + "next").click(function () {
            owl.trigger('owl.next');
        })
        jQuery("#" + controlid + "prev").click(function () {
            owl.trigger('owl.prev');
        })
    });
};

function ScrollerInitBanner1(controlid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + controlid);
        owl.owlCarousel({
            items: [2],
            //autoPlay: 5000,
            itemsDesktop: [1199, 2], // i/tems between 1000px and 601px
            itemsTablet: [979, 2], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000,
            afterInit: lazyLoad,
            afterMove: lazyLoad
        });

        // Custom Navigation Events
        jQuery("#" + controlid + "next").click(function () {
            owl.trigger('owl.next');
        })
        jQuery("#" + controlid + "prev").click(function () {
            owl.trigger('owl.prev');
        })
    });
};

function ScrollerInitBanner2(controlid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + controlid);
        owl.owlCarousel({
            items: [3],
            //autoPlay: 5000,
            itemsDesktop: [1199, 2], // i/tems between 1000px and 601px
            itemsTablet: [979, 2], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000,
            afterInit: lazyLoad,
            afterMove: lazyLoad
        });

        // Custom Navigation Events
        jQuery("#" + controlid + "next").click(function () {
            owl.trigger('owl.next');
        })
        jQuery("#" + controlid + "prev").click(function () {
            owl.trigger('owl.prev');
        })
    });
};


function ScrollerInitBanner4(controlid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + controlid);
        owl.owlCarousel({
            items: [6],
            //autoPlay: 5000,
            itemsDesktop: [1199, 2], // i/tems between 1000px and 601px
            itemsTablet: [979, 2], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000,
            afterInit: lazyLoad,
            afterMove: lazyLoad
        });

        // Custom Navigation Events
        jQuery("#" + controlid + "next").click(function () {
            owl.trigger('owl.next');
        })
        jQuery("#" + controlid + "prev").click(function () {
            owl.trigger('owl.prev');
        })
    });
};


function ScrollerInitBanner5(controlid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + controlid);
        owl.owlCarousel({
            items: [5],
            //autoPlay: 5000,
            itemsDesktop: [1199, 2], // i/tems between 1000px and 601px
            itemsTablet: [979, 2], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000,
            afterInit: lazyLoad,
            afterMove: lazyLoad
        });

        // Custom Navigation Events
        jQuery("#" + controlid + "next").click(function () {
            owl.trigger('owl.next');
        })
        jQuery("#" + controlid + "prev").click(function () {
            owl.trigger('owl.prev');
        })
    });
};

function ScrollerInitBanner3(controlid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + controlid);
        owl.owlCarousel({
            items: [8],
            //autoPlay: 5000,
            itemsDesktop: [1199, 2], // i/tems between 1000px and 601px
            itemsTablet: [979, 2], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000,
            afterInit: lazyLoad,
            afterMove: lazyLoad
        });

        // Custom Navigation Events
        jQuery("#" + controlid + "next").click(function () {
            owl.trigger('owl.next');
        })
        jQuery("#" + controlid + "prev").click(function () {
            owl.trigger('owl.prev');
        })
    });
};