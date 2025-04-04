﻿"use strict";


function injectScrollerAndLoad(type, container, controlid, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, scrollertype, categoria2Liv, vetrina, promozioni) {

    loadref(injectScrollerAndLoadinner, type, container, controlid, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, scrollertype, categoria2Liv, vetrina, promozioni, lng);

}

function injectScrollerAndLoadinner(type, container, controlid, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, scrollertype, categoria2Liv, vetrina, promozioni) {

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
        //getfromsession('objfiltro', function (retval) {
        //    var objfiltro = {};
        //    if (retval != null && retval != '')
        //        objfiltro = JSON.parse(retval);
        //    params = objfiltro; //Metto in params tutti i valori presenti nell'objfiltro in session
        params.container = container;
        params.tipologia = tipologia;
        params.visualData = visualData;
        params.visualPrezzo = visualPrezzo;
        params.maxelement = maxelement;
        params.listShow = listShow;
        params.categoria = categoria;
        params.categoria2Liv = categoria2Liv;
        params.scrollertype = scrollertype;
        params.vetrina = vetrina;
        params.promozioni = promozioni;
        globalObject[controlid + "params"] = params;

        //Qui puoi fare inizializzazione controlli su allegati
        CaricaScroller(controlid, container);
        //});

    });
};

function CaricaScroller(controlid, container) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

    //var tipologia = tipologia || "";
    //var visualData = visualData || "false";
    //var visualPrezzo = visualPrezzo || "false";
    //var maxelement = maxelement || "16";
    //var listShow = listShow || "";
    //var categoria = categoria || ""; 
    //objfiltrotmp.tipologia = tipologia;
    //objfiltrotmp.visualData = visualData;
    //objfiltrotmp.visualPrezzo = visualPrezzo;
    //objfiltrotmp.maxelement = maxelement;
    //objfiltrotmp.listShow = listShow;
    //objfiltrotmp.categoria = categoria;

    caricaDatiServer(lng, objfiltrotmp, 1, 20, false,
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
                callafterfilter(localObjects, controlid, container);

            }
            catch (e) { }
        },
        renderScrollerList);
};



function renderScrollerList(localObjects, controlid, container) {
    //console.log("---------RENDERSCROLLER");
    //console.log(localObjects);
    /* Prendo i primi end oggetti dalla lista completa*/
    BindScroller(controlid, localObjects, container);//I dati sono già paginati all'origine
};

function BindScroller(el, localObjects, container) {

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"];
    if (!data.length) {
        $('#' + el).html('');
        return;
    }
   // var container = objfiltrotmp.container;
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
    var outerhtml = jquery_obj.outerHTML();
    var innerHtml = jquery_obj.html();
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

    //Inseriamo htmlout nel contenitore  $('#' + el).html e inizializziamo lo scroller
    $('#' + el).html('');
    $('#' + el).html(htmlout);
    CleanHtml($('#' + el));

    $('#' + el).parent().parent().parent().parent().show();
    $('#' + container + 'Title').show();
    initScrollertype(el, container, globalObject[el + "params"].scrollertype);
};

function initScrollertype(el,container, type) {

    $('#' + el).parent().parent().parent().parent().show();
    $('#' + container + 'Title').show();

    setTimeout(function () {
        var scrollertype = type;
        //console.log(scrollertype);
        switch (scrollertype) {
            case "1":
                ScrollerInit1(el);
                break;
            case "2":
                ScrollerInit2(el);
                break;
            case "5":
                ScrollerInit5(el);
                break;
            case "3":
                ScrollerInitSingolo(el);
                break;
            default:
                ScrollerInit(el);
                break;
        }
    }, 100);
}

function ScrollerInit(controlid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + controlid);
        owl.owlCarousel({
            items: [4],
            //autoPlay: 5000,
            itemsDesktop: [1199, 3], // i/tems between 1000px and 601px
            itemsTablet: [979, 2], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000,
            pagination: true,
            navigation: false,
            afterInit: lazyLoad,
            afterMove: lazyLoad,
            lazyLoad: true  //assegnare lazyOwl class a img
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

function ScrollerInit1(controlid) {
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
            afterMove: lazyLoad,
            lazyLoad: truelazyLoad,
            lazyLoad: true //assegnare lazyOwl class a img
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



function ScrollerInit2(controlid) {
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
            afterMove: lazyLoad,
            lazyLoad: true //assegnare lazyOwl class a img
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

function ScrollerInit5(controlid) {
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
            afterMove: lazyLoad,
            lazyLoad: true //assegnare lazyOwl class a img
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

function ScrollerInitSingolo(controlid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + controlid);
        owl.owlCarousel({
            items: [1],
            //autoPlay: 5000,
            itemsDesktop: [1199, 1], // i/tems between 1000px and 601px
            itemsTablet: [979, 1], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000,
            afterInit: lazyLoad,
            afterMove: lazyLoad,
            lazyLoad: true //assegnare lazyOwl class a img
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