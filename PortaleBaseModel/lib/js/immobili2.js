"use strict";


function UpdateImmobiliJs() {
    /*FUNZIONI DI AGGIORNAMENTO ?upd=true pppore upd=sitemaps*/
    var updateresources = $.getQueryString("upd");
    if (updateresources == 'true') /*Ablitazione aggiornamento dati immobiliari, mettere una condizione temporale per farlo!*/
        CaricaDatiImmobilidaGestionale(); //Aggiornamento dati immobiliari
    if (updateresources == 'sitemaps')
        GeneraSitemapsImmobili();
}

/*IMPORTAZIONE FILE JSON IMMOBILI DAL GESTIONALE PER IL SALVATAGGIO SUL SERVER DEL SITO WEB*/
function CaricaDatiImmobilidaGestionale() {
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        cache: false,
        data: { 'q': 'getestatefromurl' },
        success: function (result) {
            try {
                console.log('successfully imported estates');
                GeneraSitemapsImmobili(); //Devo sempre rigenerere le sitempas degli immobili quando aggiorno gli immobili!!
            }
            catch (e) {
                console.log('err import estates');
                console.log(result.resultText);

            }
            return;
        },
        failure: function (result) {
            try {
                console.log('err importi estates');
                console.log(result.responseText);
            }
            catch (e) { }

        }, error: function (result) {
            try {
                console.log('err importi estates');
                console.log(result.responseText);
            }
            catch (e) { }

        }
    });
}
function GeneraSitemapsImmobili() {
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        cache: false,
        data: { 'q': 'creasitemapsresources' },
        success: function (result) {
            try {
                console.log('successly updates sitempas and regenerated url');

            }
            catch (e) { }
            return;
        },
        failure: function (result) {
            try {
                console.log('err generate sitemaps  ');
                console.log(result.responseText);
            }
            catch (e) { }

        }, error: function (result) {
            try {
                console.log('err generate sitemaps');
                console.log(result.responseText);
            }
            catch (e) { }
        }
    });
}


function CaricaResourceServer(id, lng, callback, functiontocallonend) {
    var lng = lng || "I";
    var id = id || "";
    $.ajax({
        url: pathAbs + resourcehandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        //async: false,
        data: {
            'q': 'getresource', 'id': id, 'lng': lng
        },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback('', function () {
            });
        }
    });
}



function caricaImmobilidaServer(lng, objfiltro, page, pagesize, enablepager, callback, functiontocallonend) {
    var lng = lng || "I";
    var objfiltro = objfiltro || "";
    var page = page || "";
    var pagesize = pagesize || "";
    var enablepager = enablepager || false;

    $.ajax({
        url: pathAbs + resourcehandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        //async: false,
        data: {
            'q': 'filterresources', 'objfiltro': JSON.stringify(objfiltro), 'lng': lng, 'page': page, 'pagesize': pagesize, 'enablepager': enablepager
        },
        success: function (result) {

            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, function () {
            });
        }
    });
}



///////////////////////////////////
/*Caricamento lista immobili isotope*/
///////////////////////////////////

function InjectPagerPortfolioImmobili(pagercontainer, controlid) {

    var templateHtml = pathAbs + "/lib/template/" + "pagerIsotope.html";

    $("#" + pagercontainer).load(templateHtml, function () {
        /*DA COMPLETARE MODIFICANDO NEL PAGER i VALORI IN MODO CHE CHIAMI CON I PARAMETRI GIUSTI IL CARICAMENTO DATI*/
        $('#' + pagercontainer).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });
        initHtmlPagerImmobili(controlid);
        CaricaIsotopeImmobili(controlid);
    });
}

function injectPortfolioImmobiliAndLoad(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, serializeaddparams) {
    //setTimeout(function () {
    loadref(injectPortfolioImmobiliAndLoadinner, type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, serializeaddparams, lng);
    //}, 100);
}
function injectPortfolioImmobiliAndLoadinner(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, serializeaddparams) {

    //   console.log('injectPortfolioImmobiliAndLoadinner' + container);
    var templateHtml = pathAbs + "/lib/template/" + "isotopeImmobili.html";
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
        InitIsotopeImmobiliLocal(controlid);

        //RICARICO LA PAGINA DALLA SESISONE SE PRESENTE

        getfromsession('objfiltro', function (retval) {
            var objfiltro = {};
            var params = {};
            if (retval != null && retval != '')
                objfiltro = JSON.parse(retval);
            params = objfiltro; //Metto in params tutti i valori presenti nell'objfiltro in session
            //   console.log("injectPortfolioImmobiliAndLoadinner " + JSON.stringify(objfiltro));
            //Se presente prendo la pagina in sessione per la selezione di pagina iniziale
            if (objfiltro != null && objfiltro.hasOwnProperty("page" + controlid)) {
                var propname = "page" + controlid;
                page = objfiltro[propname];
            }
            if (serializeaddparams != null && serializeaddparams != '') {
                var addparams = JSON.parse(serializeaddparams);
                for (var property in addparams) {
                    // if (params.hasOwnProperty(property)) {
                    params[property] = addparams[property];
                    //}
                }
            }

            //Usiamo memoria globale indicizzata con l'id del controllo per memorizzare i valori di filtraggio e caricamento
            var pagerdata = {};
            pagerdata["page"] = page;
            pagerdata["pagesize"] = pagesize;
            pagerdata["totalrecords"] = 0;
            pagerdata["enablepager"] = enablepager;
            pagerdata["pagerconnectedid"] = connectedid;
            globalObject[controlid + "pagerdata"] = pagerdata;

            params.container = container;/*Inserisco il nome del container nei parametri per uso successivo nel binding*/
            params.tipologia = tipologia;
            params.visualData = visualData;
            params.visualPrezzo = visualPrezzo;
            params.maxelement = maxelement;
            params.listShow = listShow;
            params.categoria = categoria;
            params.categoria2Liv = categoria2Liv;
            params.vetrina = vetrina;
            params.promozioni = promozioni;
            params.testoricerca = testoricerca;

            //params.regione = regione; //Preso da sessione
            //params.caratteristica1 = caratteristica1;//Preso da sessione
            globalObject[controlid + "params"] = params;
            //   console.log("injectPortfolioImmobiliAndLoadinner params " + JSON.stringify(params));

            if (enablepager == "true" || enablepager == true) {
                var pagercontainer = container + "Pager";
                InjectPagerPortfolioImmobili(pagercontainer, controlid);
            }
            else {
                CaricaIsotopeImmobili(controlid);
            }
        })
    });
};

function CaricaIsotopeImmobili(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];
    var page = globalObject[controlid + "pagerdata"].page;
    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;
    //var connecteid = globalObject[controlid + "pagerdata"].pagerconnectedid;

    /*MEMORIZZO LA PAGINA IN SESSIONE RICARICANDO I VALORI PRESENTI*/
    getfromsession('objfiltro', function (retval) {
        var objfiltro = {};
        if (retval != null && retval != '')
            objfiltro = JSON.parse(retval);
        var propname = "page" + controlid;
        objfiltro[propname] = page;//aggiungo al filtro in session la pagina attuale

        //Modifica per id controlli connessi per mantenere la pagina corretta nella sessione objfiltro al cambio pagina utente
        //if (connecteid != undefined && connecteid != "" && connecteid != null) {
        //    try {
        //        var pageconnected = globalObject[connecteid + "pagerdata"].page;
        //        propname = "page" + connecteid;
        //        objfiltro[propname] = pageconnected;
        //    }
        //    catch (e) { }
        //}
        //////////////////////////////////////

        putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) { });
    });
    $(".loaderrelative").show();
    var functiontocallonend = renderIsotopeImmobiliNotPaged;
    if (enablepager == "true" || enablepager == true)
        functiontocallonend = renderIsotopeImmobiliPaged;

    caricaImmobilidaServer(lng, objfiltrotmp, page, pagesize, enablepager,
        function (result, callafterfilter) {
            var localObjects = {};
            try {

                var parsedresult = JSON.parse(result);

                var temp = parsedresult["resultinfo"];
                localObjects["resultinfo"] = temp;//Dictionary parserizzato dei parametri calcolati generali
                var totalrecords = parsedresult["resultinfo"][0].totalRecords;
                globalObject[controlid + "pagerdata"].totalrecords = totalrecords;

                var strresources = "{ \"resource\":" + JSON.stringify(parsedresult["resource"]);
                strresources += "}";
                localObjects["resourcesloaded"] = strresources;

                //Prendo le liste immobili filtrate in base alla query fatta
                localObjects["imgsprimary"] = JSON.stringify(parsedresult["imgsprimary"][0]);
                localObjects["imgscomplete"] = JSON.stringify(parsedresult["imgscomplete"][0]);
                localObjects["linklistdetail"] = parsedresult["linklistdetail"][0]; //lista dei link per gli immobili passati

                $('#divSearchBar').modal('hide');

                callafterfilter(localObjects, controlid);
            }
            catch (e) {
                $(".loaderrelative").hide();
            }

        },
        functiontocallonend);
};
function renderIsotopeImmobiliNotPaged(localObjects, controlid) {
    BindIsotopeImmobili(controlid, localObjects);//I dati sono già paginati all'origine
};
function renderIsotopeImmobiliPaged(localObjects, controlid) {
    /*Da ultimare  con la paginazione e sistemare il paginatore   .......
     * Passando nella renderPager
     ................... */
    renderPagerImmobili(controlid, function (msgpager) {
        //Renderizziamo l'html dei dati nella pagina
        BindIsotopeImmobili(controlid, localObjects);//I dati sono già paginati all'origine dal chiamante
    });
}


function BindIsotopeImmobili(controlid, localObjects) {
    try {
        var data = JSON.parse(localObjects["resourcesloaded"])["resource"];
        if (!data.length) {
            $('#' + controlid).html('');
            $(".loaderrelative").hide();
            return;
        }

        var str = $('#' + controlid)[0].innerHTML;
        $('#' + controlid).parent().parent().show(); //Row COntainer

        var alternate = ($('#' + controlid).hasClass('alternatecolor'));

        //Se presente nella memoria temporanea globale modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
        //in caso di rebinding successivo dopo la prima iniezione del template (es. con controllo pager)
        if (!globalObject.hasOwnProperty(controlid + "template")) {
            globalObject[controlid + "template"] = $('#' + controlid)[0].innerHTML;
            str = globalObject[controlid + "template"];
        }
        else
            str = globalObject[controlid + "template"];

        var jquery_obj = $(str);
        var htmlout = "";

        for (var j = 0; j < data.length; j++) {
            FillBindControlsImmobili(jquery_obj, data[j], localObjects, "",
                function (ret) {

                    /*REGIONE PER EFFETTO ALTERNATE DEGLI ITEM DEL REPEATER*/
                    if (alternate) {
                        var w = $(window).width();
                        if (w > 768) {
                            if (isEven(j)) {
                                $('.odd', ret).remove();
                                ret.css("background-color", $('.even', ret).css("background-color")); //Coloro il contenitore come l'elemento attuale
                            }
                            else {
                                $('.even', ret).remove();
                                ret.css("background-color", $('.odd', ret).css("background-color"));//Coloro il contenitore come l'elemento attuale
                            }
                        }
                        else $('.odd', ret).remove(); //in xs faccio tutte uguali le righe
                    }
                    /*END REGIONE PER EFFETTO ALTERNATE DEGLI ITEM DEL REPEATER*/

                    //htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                    htmlout += (ret.outerHTML()) + "\r\n";
                });
        }


        //Inseriamo htmlout nel contenitore  $('#' + el).html e inizializziamo lo scroller
        $('#' + controlid).html('');
        // Update isotope container with new data. 
        //$('#' + el).isotope('remove',  $('#' + el).data('isotope').$allAtoms );
        $('#' + controlid).isotope('insert', $(htmlout))
            // trigger isotope again after images have been loaded
            .imagesLoaded(function () {
                $('#' + controlid).isotope('layout');
                CleanHtml($('#' + controlid));

            });
        $(".loaderrelative").hide();
    } catch (e) { $(".loaderrelative").hide(); }
};





function InitIsotopeImmobiliLocal(controlid) {
    /* ---------------------------------------------- /*
    * Portfolio 
    /* ---------------------------------------------- */
    var worksgrid = $('#' + controlid);
    var worksgrid_mode;
    if (worksgrid.hasClass('works-grid-masonry')) {
        worksgrid_mode = 'masonry';
    } else {
        worksgrid_mode = 'fitRows';
    }

    // worksgrid.imagesLoaded(function () {
    var $grid = worksgrid.isotope({
        layoutMode: worksgrid_mode,
        itemSelector: '.work-item'
        //fitRows: {
        //gutter: 10}
    });
    //  });
    $grid.on('layoutComplete',
        function (event, laidOutItems) {
            lazyLoad();
            //console.log('Isotope layout completed on ' +
            //  laidOutItems.length + ' items');
        });
}
///////////////////////////////////
/* Fine Caricamento lista immobili*/
///////////////////////////////////

/////////////////////////////////////////////////
/* Caricamento carousel scroller immobili     */
////////////////////////////////////////////////

function injectScrollerImmobiliAndLoad(type, container, controlid, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, scrollertype, categoria2Liv, vetrina, promozioni, serializeaddparams) {

    loadref(injectScrollerImmobiliAndLoadinner, type, container, controlid, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, scrollertype, categoria2Liv, vetrina, promozioni, serializeaddparams, lng);

}

function injectScrollerImmobiliAndLoadinner(type, container, controlid, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, scrollertype, categoria2Liv, vetrina, promozioni, serializeaddparams) {

    var templateHtml = pathAbs + "/lib/template/" + "owlscrollerImmobili.html";
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

        getfromsession('objfiltro', function (retval) {
            var objfiltro = {};
            var params = {};
            if (retval != null && retval != '')
                objfiltro = JSON.parse(retval);
            params = objfiltro; //Metto in params tutti i valori presenti nell'objfiltro in session

            if (serializeaddparams != null && serializeaddparams != '') {
                var addparams = JSON.parse(serializeaddparams);
                for (var property in addparams) {
                   // if (params.hasOwnProperty(property)) {
                        params[property] = addparams[property];
                    //}
                }
            }


            var pagerdata = {};
            pagerdata["page"] = 1;
            pagerdata["pagesize"] = 1;
            pagerdata["totalrecords"] = 0;
            pagerdata["enablepager"] = false;
            //pagerdata["pagerconnectedid"] = connectedid;
            globalObject[controlid + "pagerdata"] = pagerdata;

            //params.ddlTipologiaSearch = "";
            //params.ddlRegioneSearch = "";

            params.container = container;/*Inserisco il nome del container nei parametri per uso successivo nel binding*/
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
            CaricaScrollerImobili(controlid, container);
        });

    });
};

function CaricaScrollerImobili(controlid, container) {

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
    $(".loaderrelative").show();
    var functiontocallonend = renderScrollerListImmobili;
    caricaImmobilidaServer(lng, objfiltrotmp, 1, 1, false,
        function (result, callafterfilter) {
            var localObjects = {};
            try {

                var parsedresult = JSON.parse(result);

                var temp = parsedresult["resultinfo"];
                localObjects["resultinfo"] = temp;//Dictionary parserizzato dei parametri calcolati generali
                var totalrecords = parsedresult["resultinfo"][0].totalRecords;
                globalObject[controlid + "pagerdata"].totalrecords = totalrecords;

                var strresources = "{ \"resource\":" + JSON.stringify(parsedresult["resource"]);
                strresources += "}";
                localObjects["resourcesloaded"] = strresources;

                //Prendo le liste immobili filtrate in base alla query fatta
                localObjects["imgsprimary"] = JSON.stringify(parsedresult["imgsprimary"][0]);
                localObjects["imgscomplete"] = JSON.stringify(parsedresult["imgscomplete"][0]);
                localObjects["linklistdetail"] = parsedresult["linklistdetail"][0]; //lista dei link per gli immobili passati


                callafterfilter(localObjects, controlid);
            }
            catch (e) {
                $(".loaderrelative").hide();
            }

        },
        functiontocallonend);


};


function renderScrollerListImmobili(localObjects, controlid) {
    //console.log("---------RENDERSCROLLER");
    //console.log(localObjects);
    /* Prendo i primi end oggetti dalla lista completa*/
    BindScrollerImmobili(controlid, localObjects);//I dati sono già paginati all'origine
};

function BindScrollerImmobili(el, localObjects) {

    var data = JSON.parse(localObjects["resourcesloaded"])["resource"];
    if (!data.length) {
        $('#' + controlid).html('');
        $(".loaderrelative").hide();
        return;
    }



    //Se presente nella memoria temporanea globale modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
    //in caso di rebinding successivo dopo l'iniezione del template
    if (!globalObject.hasOwnProperty(el + "template")) {
        globalObject[el + "template"] = $('#' + el)[0].innerHTML;
        str = globalObject[el + "template"];
    }
    else
        str = globalObject[el + "template"];

    var objfiltrotmp = {};
    objfiltrotmp = globalObject[el + "params"];
    var container = objfiltrotmp.container;

    var str = $('#' + el)[0].innerHTML;
   // $('#' + el).parent().parent().parent().parent().show();

    var jquery_obj = $(str);
    var outerhtml = jquery_obj.outerHTML();
    var innerHtml = jquery_obj.html();
    var containeritem = outerhtml.replace(innerHtml, '');/*Prendo l'elemento contenitore*/
    var htmlout = "";
    for (var j = 0; j < data.length; j++) {
        FillBindControlsImmobili(jquery_obj, data[j], localObjects, "",
            function (ret) {
                htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
            });
    }

    //Inseriamo htmlout nel contenitore  $('#' + el).html e inizializziamo lo scroller
    $('#' + el).html('');
    $('#' + el).html(htmlout);
   
    setTimeout(function () {
        var scrollertype = globalObject[el + "params"].scrollertype;
        switch (scrollertype) {
            case "1":
                ScrollerInit1(el);
                break;
            case "2":
                ScrollerInit2(el);
                break;
            default:
                ScrollerInit(el);
                break;
        }
        CleanHtml($('#' + container));
        $('#' + container + 'Title').show();
        $('#' + container).show();
        $(".loaderrelative").hide();

    }, 100);
  

};


////////////////////////////////////////////////////////
/* FIne  Caricamento carousel scroller immobili       */
////////////////////////////////////////////////////////

//////////////////////////////////////
/* Caricamento singolo immobile     */
//////////////////////////////////////
function injectandloadimmobile(type, container, controlid, visualData, visualPrezzo, iditem) {
    loadref(injectandloadimmobileinner, type, container, controlid, visualData, visualPrezzo, iditem, lng);
}
function injectandloadimmobileinner(type, container, controlid, visualData, visualPrezzo, iditem) {
    //qui devo visualizzare il titolo
    var templateHtml = pathAbs + "/lib/template/" + "schedadetailsimmobile.html";
    if (type != null && type != '')
        templateHtml = pathAbs + "/lib/template/" + type;

    //Correggo l'id dei controlli del template per l'inzializzazione dello scroller con id univoca e corretta
    $('#' + container).html('');
    $('#' + container).load(templateHtml, function () {

        //injectOwlGalleryControls(controlid, "plhGallery");
        injectFlexsliderControlsimmobile(controlid, "plhGallery");

        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });

        var params = {};
        params.container = container;/*Inserisco il nome dle container nei parametri per uso successivo nel binding*/
        params.id = iditem;
        params.visualData = visualData;
        params.visualPrezzo = visualPrezzo;
        globalObject[controlid + "params"] = params;

        CaricaImmobile(controlid);
    });
};
function CaricaImmobile(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];
    $(".loaderrelative").show();
    var functiontocallonend = renderimmobile;
    //if (enablepager == "true" || enablepager == true)
    //    functiontocallonend = renderIsotopePaged;
    CaricaResourceServer(objfiltrotmp.id, lng,
        function (result, callafterfilter) {
            var localObjects = {};
            try {
                if (result !== null && result != '') {
                    var datiritorno = JSON.parse(result);
                    var resourceparsed = JSON.parse(datiritorno["resource"]);

                    var temp = datiritorno["resultinfo"];
                    localObjects["resultinfo"] = temp;//Dictionary parserizzato dei parametri calcolati generali

                    localObjects["resource"] = resourceparsed;
                    localObjects["linklistdetail"] = JSON.parse(datiritorno["linklistdetail"]);
                    localObjects["imgsprimary"] = datiritorno["imgsprimary"];
                    localObjects["imgscomplete"] = datiritorno["imgscomplete"];

                    callafterfilter(localObjects, controlid);
                }
            }
            catch (e) {
                //console.log(e);
                $(".loaderrelative").show();
            }
        },
        functiontocallonend);
};
function renderimmobile(localObjects, controlid) {
    bindimmobile(controlid, localObjects);//I dati sono già paginati all'origine
};
function bindimmobile(el, localObjects) {
    try {
        var data = localObjects["resource"];
        if ((data == null) || data == '') {
            $('#' + el).html('');
            $(".loaderrelative").hide();
            return;
        }

        var objfiltrotmp = {};
        objfiltrotmp = globalObject[el + "params"];
        var container = objfiltrotmp.container;

        var str = $($('#' + el)[0]).outerHTML();
        var jquery_obj = $(str);
        jquery_obj = $(jquery_obj);

        //var htmlout = "";
        $('#' + container).html('');
        // for (var j = 0; j < data.length; j++) {
        FillBindControlsImmobili(jquery_obj.wrap('<p>').parent(), data, localObjects, "",
            function (ret) {
                //htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                $('#' + container).append(ret.html()) + "\r\n";
            });
        // }
        $('#' + container).show();
        $('#' + container + 'Title').show();
        //console.log('content generic inject');
        jQuery(document).ready(function () {
            //setTimeout(function (el) { rebuildCarousel(el) }, 3000);
            //rebuildCarousel(el);
            inizializzaFlexsliderGalleryimmobile(el, container);
            //  $('.zoommgfy').magnify(); //ATTIVA LO ZOOM NELLA GALLERIA

        });
        CleanHtml($('#' + container));
        reinitaddthis();
        $(".loaderrelative").hide();
    } catch (e) { $(".loaderrelative").hide(); }
};

function reinitaddthis() {

    $('#atstbx').remove();
    //if (typeof addthis !== 'undefined') {
    //     addthis.layers.refresh();
    //    console.log('addthisreint');
    //}
    var counter = 0;
    var looper = setInterval(function () {
        if (typeof addthis !== 'undefined') {
            addthis.layers.refresh();
            console.log('addthisreint');
        }
        counter++;
        if (counter >= 5) {
            clearInterval(looper);
        };
    }, 200);
}

/*--- FLEXLIDER GALLERY -------http://www.woothemes.com/flexslider/-----------*/
function injectFlexsliderControlsimmobile(controlid, container) {
    $("#" + container).load("/lib/template/" + "flexslidergalleryimmobile.html", function () {
        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });
    });
}
function inizializzaFlexsliderGalleryimmobile(controlid, container) {
    //Plugin: flexslider con funzione di animazione dei messaggi o oggetti sopra
    // ------------------------------------------------------------------
    if ($("#" + controlid + "-main-slider") != null)
        $("#" + controlid + "-main-slider").each(function () {
            var sliderSettings = {
                animation: $(this).attr('data-transition'),
                easing: "swing",
                selector: ".slides > .slide",
                smoothHeight: false,
                // controlsContainer: ".flex-container",
                animationLoop: false,             //Boolean: Should the animation loop? If false, directionNav will received "disable" classes at either end
                slideshow: false,                //Boolean: Animate slider automatically
                //   slideshowSpeed: 4000,           //Integer: Set the speed of the slideshow cycling, in milliseconds
                //  animationSpeed: 600,            //Integer: Set the speed of animations, in milliseconds

                // Usability features
                pauseOnAction: true,            //Boolean: Pause the slideshow when interacting with control elements, highly recommended.
                pauseOnHover: true,            //Boolean: Pause the slideshow when hovering over slider, then resume when no longer hovering
                useCSS: true,                   //{NEW} Boolean: Slider will use CSS3 transitions if available
                touch: true,                    //{NEW} Boolean: Allow touch swipe navigation of the slider on touch-enabled devices
                video: false,                   //{NEW} Boolean: If using video in the slider, will prevent CSS3 3D Transforms to avoid graphical glitches
                sync: "#" + controlid + "-navigation-slider",

                // Primary Controls
                // controlNav: "thumbnails",       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                controlNav: false,       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                directionNav: true,             //Boolean: Create navigation for previous/next navigation? (true/false)
                prevText: "",           //String: Set the text for the "previous" directionNav item
                nextText: "",               //String: Set the text for the "next" directionNav item

                start: function (slider) {
                    //hide all animated elements
                    slider.find('[data-animate-in]').each(function () {
                        $(this).css('visibility', 'hidden');
                    });

                    //animate in first slide
                    slider.find('.slide').eq(1).find('[data-animate-in]').each(function () {
                        $(this).css('visibility', 'hidden');
                        if ($(this).data('animate-delay')) {
                            $(this).addClass($(this).data('animate-delay'));
                        }
                        if ($(this).data('animate-duration')) {
                            $(this).addClass($(this).data('animate-duration'));
                        }
                        $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                        $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                            function () {
                                $(this).removeClass($(this).data('animate-in'));
                            }
                        );
                    });
                },
                before: function (slider) {
                    //hide next animate element so it can animate in
                    slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                        $(this).css('visibility', 'hidden');
                    });
                },
                after: function (slider) {
                    //hide animtaed elements so they can animate in again
                    slider.find('.slide').find('[data-animate-in]').each(function () {
                        $(this).css('visibility', 'hidden');
                    });

                    //animate in next slide
                    slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                        if ($(this).data('animate-delay')) {
                            $(this).addClass($(this).data('animate-delay'));
                        }
                        if ($(this).data('animate-duration')) {
                            $(this).addClass($(this).data('animate-duration'));
                        }
                        $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                        $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                            function () {
                                $(this).removeClass($(this).data('animate-in'));
                            }
                        );
                    });
                    /* auto-restart player if paused after action */
                    if (!slider.playing) {
                        slider.play();
                    }

                }
            };

            var sliderNav = $(this).attr('data-slidernav');
            if (sliderNav !== 'auto') {
                sliderSettings = $.extend({}, sliderSettings, {
                    //  controlsContainer: '.flex-container',
                    manualControls: sliderNav + ' li a'
                });
            }
            $(this).flexslider(sliderSettings);
        });
    //    $("#" + controlid + "-main-slider").resize(); //make sure height is right load assets loaded
    if ($("#" + controlid + "-navigation-slider") != null)
        $("#" + controlid + "-navigation-slider").flexslider({
            animation: "slide",
            controlNav: false,
            animationLoop: false,
            slideshow: false,
            itemWidth: 120,
            itemHeight: 80,
            itemMargin: 5,
            asNavFor: "#" + controlid + "-main-slider"
        });

}
/*--- FLEXLIDER GALLERY ------------------*/
//////////////////////////////////////
/* Fine Caricamento singolo immobile     */
//////////////////////////////////////


/*Riceve una stringa Html parserizzata con jquery per il fill coi dati*/
function FillBindControlsImmobili(jquery_obj, dataitem, localObjects, classselector, callback) {


    var classselector = classselector || "bind";

    //Clono l'oggetto passato in una variabile locale per modificarlo
    var jquery_obj = jquery_obj.clone() || [];
    $("." + classselector, jquery_obj).each(function (index, text) {
        var proprarr = '';
        if ($(this).attr("mybind") != null)
            proprarr = $(this).attr("mybind").split('.');
        if (proprarr != null && proprarr.length != 0) {
            switch (proprarr.length) {
                case 1: //Oggetto 1 livello

                    //if (dataitem.hasOwnProperty(proprarr[0]))
                    //    $(this).val(dataitem[proprarr[0]]);
                    //else
                    //    $(this).val('');

                    if ($(this).is("input") && $(this).attr('type') == 'checkbox') {
                        if (dataitem.hasOwnProperty(proprarr[0]))
                            $(this).prop("checked", dataitem[proprarr[0]]);
                        else
                            $(this).prop("checked", false);
                    }
                    if ($(this).is("input") && ($(this).attr('type') == 'text' || $(this).attr('type') == null)) {
                        if (dataitem.hasOwnProperty(proprarr[0]))
                            $(this).attr("value", dataitem[proprarr[0]]);
                        else
                            $(this).attr("value", '');
                    }
                    else if ($(this).is("a")) {
                        var link = "";
                        var idscheda = dataitem[proprarr[0]];
                        var testo = "";
                        var bindprophref = "";
                        var bindproptitle = "";
                        if ($(this).attr("href") != null)
                            bindprophref = $(this).attr("href");
                        if ($(this).attr("title") != null)
                            bindproptitle = $(this).attr("title");

                        if (localObjects["linklistdetail"].hasOwnProperty(idscheda)) {
                            if (localObjects["linklistdetail"][idscheda].hasOwnProperty(bindprophref)) {
                                link = localObjects["linklistdetail"][idscheda][bindprophref];
                                $(this).attr("href", link);
                            }
                            if (localObjects["linklistdetail"][idscheda].hasOwnProperty(bindproptitle)) {
                                testo = localObjects["linklistdetail"][idscheda][bindproptitle];
                                $(this).attr("title", testo);
                            }
                        }
                        else
                            $(this).attr("href", '');
                    }
                    else if ($(this).is("img")) {
                        var completepath = "";
                        var idallegato = dataitem[proprarr[0]];
                        if (dataitem.hasOwnProperty(proprarr[0])) {
                            CompleteUrlImmobiliPrimaryImg(localObjects, idallegato, true, usecdn, function (imgret) {
                                completepath = imgret;
                            });
                            if (completepath != null && completepath != '')
                                $(this).attr("src", completepath);
                        }
                        //else
                        //    $(this).attr("src", '');
                    }
                    else if ($(this).is("div")
                        && ($(this).hasClass('flexmaincontainer'))
                    ) {
                        var imgslist = "";
                        var imgslistdesc = "";
                        var imgslistratio = "";
                        var contenutoslide = "";
                        var idallegato = dataitem[proprarr[0]];
                        /*Lista completa degli allegati per l'immobile*/
                        CompleteUrlImmobiliListImgs(localObjects, idallegato, false, usecdn, function (ret) {
                            imgslist = ret;
                        })
                        CompleteUrlImmobiliListImgsDesc(localObjects, idallegato, false, usecdn, function (ret) {
                            imgslistdesc = ret;
                        })
                        CompleteUrlImmobiliListImgsRatio(localObjects, idallegato, false, usecdn, function (ret) {
                            imgslistratio = ret;
                        })

                        for (var j = 0; j < imgslist.length; j++) {
                            try {
                                /*<div class="slide" data-thumb="" >
                                    <div class="slide-content" style="position:relative;padding:1px">
                                        <img itemprop="image" style="border:none" src="" alt="" />
                                        <div class="divbuttonstyle" style="position:absolute;left:30px;bottom:30px;padding:10px;text-align:left;color:#ffffff;">
                                            <a style="color:#ffffff" href="" target="" title="">&nbsp</a>
                                        </div>
                                    </div>
                                </div >*/

                                var descriptiontext = "";

                                contenutoslide += '<div class="slide"  data-thumb="';
                                contenutoslide += imgslist[j];
                                contenutoslide += '">';
                                contenutoslide += '<div class="slide-content"  style="position:relative;padding:1px">';

                                var imgstyle = "";
                                imgstyle = "max-width:100%;height:auto;";
                                var maxheight = $(this).getStyle('max-height');
                                if (maxheight !== '') {
                                    try {
                                        if (parseFloat(imgslistratio[j].replace(",", ".")) < 1) {
                                            imgstyle = "width:auto;height:" + maxheight + "px;";
                                        }
                                    }
                                    catch (e) {
                                    };
                                }

                                contenutoslide += '<img class="zoommgfy" itemprop="image"  style="border:none;' + imgstyle + '" src="';
                                contenutoslide += imgslist[j];

                                contenutoslide += '" ';

                                contenutoslide += ' data-magnify-src="';
                                contenutoslide += imgslist[j];
                                contenutoslide += '" ';

                                /*Livello di ingrandimento della lente (è fatto sempre rispetto alla dimensione dell'immagine naturale che qui gli forzo!!!)*/
                                var aspectratio = parseFloat(imgslistratio[j].replace(",", "."));
                                var imgwidth = 1100;
                                var imgheight = imgwidth / aspectratio;
                                contenutoslide += ' data-magnify-magnifiedwidth="';
                                contenutoslide += Math.floor(imgwidth);
                                contenutoslide += '" ';
                                contenutoslide += ' data-magnify-magnifiedheight="';
                                contenutoslide += Math.floor(imgheight);
                                contenutoslide += '" ';
                                /*Livello di ingrandimento della lente*/

                                contenutoslide += ' alt = "" />';

                                try {
                                    descriptiontext = imgslistdesc[j];
                                    if (descriptiontext !== '') {
                                        contenutoslide += '<div class="divbuttonstyle" style="position:absolute;left:30px;bottom:30px;padding:10px;text-align:left;color:#ffffff;">';
                                        contenutoslide += descriptiontext;
                                        contenutoslide += '</div>';
                                    }
                                } catch (e) {
                                };

                                contenutoslide += '</div>';
                                contenutoslide += '</div>';

                            }
                            catch (e) {
                            }
                        }
                        $(this).html(contenutoslide);
                        if (contenutoslide !== '')
                            $(this).parent().show();

                    }
                    else if ($(this).is("ul")
                        && ($(this).hasClass('flexnavcontainer'))
                    ) {
                        var imgslist = "";
                        var imgslistdesc = "";
                        var contenutoslide = "";
                        var idallegato = dataitem[proprarr[0]];
                        /*Lista completa degli allegati per l'immobile*/
                        CompleteUrlImmobiliListImgs(localObjects, idallegato, false, usecdn, function (imgret) {
                            imgslist = imgret;
                        })
                        CompleteUrlImmobiliListImgsDesc(localObjects, idallegato, false, usecdn, function (filesret) {
                            imgslistdesc = filesret;
                        })

                        for (var j = 0; j < imgslist.length; j++) {
                            try {
                                /*<li > <img src="" alt="" style="padding:5px" /></li > */
                                contenutoslide += '<li > <img style="padding:5px" src="';
                                var position = imgslist[j].lastIndexOf('/');
                                var pathanteprima = imgslist[j].substr(0, position + 1) + "ant" + imgslist[j].substr(position + 1);
                                contenutoslide += pathanteprima;
                                contenutoslide += '" alt="" />';
                                contenutoslide += '</li>';
                            }
                            catch (e) {
                            }
                        }
                        $(this).html(contenutoslide);
                        if (contenutoslide !== '')
                            $(this).parent().show();

                    }
                    else if ($(this).is("div")
                        && ($(this).hasClass('owl-carousel') || $(this).hasClass('img-list'))
                    ) {
                        var imgslist = "";
                        var idallegato = dataitem[proprarr[0]];
                        /*Lista completa degli allegati per l'immobile*/
                        CompleteUrlImmobiliListImgs(localObjects, idallegato, false, usecdn, function (imgret) {
                            imgslist = imgret;
                        })
                        var contenutoslide = "";
                        for (var j = 0; j < imgslist.length; j++) {
                            contenutoslide += '<div class="item">';

                            if ($(this).hasClass('img-list'))
                                contenutoslide += '<img   class="img-responsive"  src="';
                            else
                                //contenutoslide += '<img style="width:100%"  class="img-responsive"  src="';
                                contenutoslide += '<img style="margin:0px auto" class="img-responsive"  src="';

                            contenutoslide += imgslist[j];
                            contenutoslide += '"/>';
                            contenutoslide += '</div>';
                        }
                        $(this).html(contenutoslide);
                        if (contenutoslide !== '')
                            $(this).parent().show();
                    }
                    //else if ($(this).is("div")
                    //    && ($(this).hasClass('owl-slider-v2'))
                    //    ) {
                    //    var contenutoslide = "";

                    //}
                    else if ($(this).is("iframe")) {
                        var idelement = dataitem[proprarr[0]];
                        var property = "";
                        if ($(this).attr("myvalue") != null)
                            property = $(this).attr("myvalue");
                        var valore = "";
                        if (localObjects["linkloaded"].hasOwnProperty(idelement)) {
                            if (localObjects["linkloaded"][idelement].hasOwnProperty(property)) {
                                valore = localObjects["linkloaded"][idelement][property];
                                if (valore != null && valore != '') {
                                    $(this).attr("src", valore + "?rel=0");//&autoplay=1
                                    $(this).parent().show();
                                }
                            }
                        }

                    }
                    else {
                        if (dataitem.hasOwnProperty(proprarr[0])) {
                            var valore = [];
                            valore[0] = dataitem[proprarr[0]];
                            var prop = [];
                            var formatfunc = "";

                            if ($(this).attr("format") != null) {
                                formatfunc = $(this).attr("format");
                                if ($(this).attr("mybind1") != null)
                                    valore[1] = dataitem[$(this).attr("mybind1")];
                                if ($(this).attr("mybind2") != null)
                                    valore[2] = dataitem[$(this).attr("mybind2")];
                                if ($(this).attr("mybind3") != null)
                                    valore[3] = dataitem[$(this).attr("mybind3")];
                                if ($(this).attr("myvalue") != null)
                                    prop[0] = $(this).attr("myvalue");
                                if ($(this).attr("myvalue1") != null)
                                    prop[1] = $(this).attr("myvalue1");
                                if ($(this).attr("myvalue2") != null)
                                    prop[2] = $(this).attr("myvalue2");
                                window[formatfunc](localObjects, valore, prop, function (ret) {
                                    if (ret != null && Array.isArray(ret) && ret.length > 0)
                                        valore = ret[0];
                                    else
                                        valore = ret;
                                });
                            }

                            if (valore == "true" || valore == "false") {
                                if (valore == "true")
                                    $(this).hide();
                            }
                            else
                                $(this).html(valore);
                            //$(this).html(dataitem[proprarr[0]]);
                        }
                        else
                            $(this).html('');
                    }

                    break;
                case 2: //Oggetto bind di 2 livelli
                    if ($(this).is("span")) {
                        var idelement = dataitem[proprarr[0]];
                        var property = proprarr[1];
                        var valore = "";
                        if (localObjects["linklistdetail"].hasOwnProperty(idelement)) {
                            if (localObjects["linklistdetail"][idelement].hasOwnProperty(property)) {
                                valore = localObjects["linklistdetail"][idelement][property];
                                $(this).html(valore);
                            }
                        }
                        else
                            $(this).html(valore);
                    }
                    else if ($(this).is("iframe")) {
                        var idelement = dataitem[proprarr[0]];
                        var property = proprarr[1];
                        var valore = "";
                        if (localObjects["linklistdetail"].hasOwnProperty(idelement)) {
                            if (localObjects["linklistdetail"][idelement].hasOwnProperty(property)) {
                                if (valore != null && valore != '') {
                                    valore = localObjects["linklistdetail"][idelement][property];
                                    $(this).attr("src", valore + "?rel=0");//&autoplay=1
                                    $(this).parent().show();
                                }
                            }
                        }

                    }
                    break;
            }
        }
    });


    //return jquery_obj;
    callback(jquery_obj);
}

function CompleteUrlImmobiliPrimaryImg(localObjects, idallegati, anteprima, usecdn, callback) {
    var pathfile = "";
    try {
        //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/NomeFile
        //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/Descrizione
        var strfind = "{ \"imgs\" : [" + localObjects["imgsprimary"];
        strfind += " ] }";
        var selectedname = JSON.search(JSON.parse(strfind), '//*[guid_link=\"' + idallegati + '\"]/NomeFile');
        if (selectedname != null && selectedname.length > 0) {
            var prefix = "ant";
            if (!anteprima) prefix = "";
            var serverurl = percorsoapp;
            if (usecdn) serverurl = percorsocdn;
            pathfile = serverurl + percorsoimg + idallegati + "/" + prefix + selectedname[0];
        }
    } catch (e) {
    };
    callback(pathfile);
}
function CompleteUrlImmobiliListImgs(localObjects, idallegati, anteprima, usecdn, callback) {
    var arrayimgs = [];
    try {
        //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/NomeFile
        //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/Descrizione
        var strfind = "{ \"imgs\" : [" + localObjects["imgscomplete"];
        strfind += " ] }";
        var names = JSON.search(JSON.parse(strfind), '//*[guid_link=\"' + idallegati + '\"]/NomeFile');

        for (var j = 0; j < names.length; j++) {
            var prefix = "ant";
            if (!anteprima) prefix = "";
            var serverurl = percorsoapp;
            if (usecdn) serverurl = percorsocdn;
            arrayimgs.push(serverurl + percorsoimg + idallegati + "/" + prefix + names[j]);
        }
    } catch (e) {
    };
    callback(arrayimgs);
}
function CompleteUrlImmobiliListImgsDesc(localObjects, idallegati, anteprima, usecdn, callback) {
    var arrayimgs = [];
    try {
        var strfind = "{ \"imgs\" : [" + localObjects["imgscomplete"];
        strfind += " ] }";
        var descriptions = JSON.search(JSON.parse(strfind), '//*[guid_link=\"' + idallegati + '\"]/Descrizione');

        for (var j = 0; j < descriptions.length; j++) {
            arrayimgs.push(descriptions[j]);
        }

    } catch (e) {
    };
    callback(arrayimgs);
}
function CompleteUrlImmobiliListImgsRatio(localObjects, idallegati, anteprima, usecdn, callback) {
    var arrayimgs = [];
    try {
        var strfind = "{ \"imgs\" : [" + localObjects["imgscomplete"];
        strfind += " ] }";
        var ratios = JSON.search(JSON.parse(strfind), '//*[guid_link=\"' + idallegati + '\"]/imageratio');

        for (var j = 0; j < ratios.length; j++) {
            arrayimgs.push(ratios[j]);
        }
    } catch (e) {
    };
    callback(arrayimgs);
}

function frmcontratto(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONreftipocontratto);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONreftipocontratto;

    var selvalue = JSON.search(dataroot, '//data[id=' + valore[0] + ']//dettaglitipocontratto[lingua="' + lng + '"]/titolo');

    //for (var j = 0; j < dataroot["data"].length; j++) {
    //    if (dataroot["data"][j].Codice == valore[0]) {
    //        selvalue = dataroot["data"][j].Campo1;
    //        break;

    //    }
    //}

    callback(selvalue);
}
function frmtiporisorsa(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONreftiporisorse);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONreftiporisorse;

    var selvalue = JSON.search(dataroot, '//data[id=' + valore[0] + ']//dettaglitiporisorse[lingua="' + lng + '"]/titolo');
    callback(selvalue);
}
function frmcondizione(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONrefcondizione);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONrefcondizione;

    var selvalue = JSON.search(dataroot, '//data[id=' + valore[0] + ']//dettaglicondizione[lingua="' + lng + '"]/titolo');
    callback(selvalue);
}
function frmprezzo(localObjects, valore, prop, callback) {
    var retstring = "";
    var unit = baseresources[lng]["valuta"];
    retstring = unit + ' ' + valore[0].formatMoney(0, '.', ',');
    var testoriservato = baseresources[lng]["riservato"];
    var condizione = "";
    if (valore.length > 1)
        condizione = valore[1];
    if (condizione == true)
        retstring = testoriservato;
    callback(retstring);
}
function frmsuperficie(localObjects, valore, prop, callback) {
    var unit = baseresources[lng]["umsuperficie"];
    callback(valore[0] + ' ' + unit);
}
function frmterreno(localObjects, valore, prop, callback) {
    var unit = baseresources[lng]["umterreno"];
    callback(valore[0] + ' ' + unit);
}
function frmcamere(localObjects, valore, prop, callback) {
    var lbl = baseresources[lng]["camerelbl"];
    //callback("<i class=\"fa fa-bed\"></i>" + ":" + valore[0]);
    callback(lbl + ": " + valore[0]);
}
function frmbagni(localObjects, valore, prop, callback) {
    var lbl = baseresources[lng]["bagnilbl"];
    //callback("<i class=\"fa fa-bed\"></i>" + ":" + valore[0]);
    callback(lbl + ": " + valore[0]);
}
function frmcodice(localObjects, valore, prop, callback) {
    var pre = baseresources[lng]["codice"];
    callback(' ' + pre + ' ' + valore[0]);
}
function frmipe(localObjects, valore, prop, callback) {
    var unit = baseresources[lng]["umipe"];
    callback(valore[0] + ' ' + unit);
}

function frmvisibility1(localObjects, valore, prop, callback) {
    var retstring = "false";
    try {
        var object = localObjects["linklistdetail"];
        if (localObjects["linklistdetail"].hasOwnProperty(valore[0])) {
            var contenuto = object[valore[0]][prop[0]];
            if (contenuto != null && contenuto != '') {
                retstring = "true";
            }
        }
    } catch (e) { };
    callback(retstring);
}


/*Pager functions start-------------------------------------------------------------------------------*/
function initHtmlPagerImmobili(controlid) {
    //$('#' + controlid + 'btnPrevPage')
    //$('#' + controlid + 'btnNextPage')
    $('#' + controlid + 'btnNextPage').attr('onClick', "javascript:nextpageimmobili('" + controlid + "')")
    $('#' + controlid + 'btnPrevPage').attr('onClick', "javascript:prevpageimmobili('" + controlid + "')")

    //$('#' + controlid + 'btnNextPage').click(function () { nextpage("'" + controlid + "'") })
    //$('#' + controlid + 'btnPrevPage').click(function () { prevpage("'" + controlid + "'") })

    //$('#' + controlid + 'btnNextPage').on("click", { controlid: controlid }, nextpage);
    //$('#' + controlid + 'btnPrevPage').on("click", { controlid: controlid }, prevpage);
}

function nextpageimmobili(controlid) {
    //   console.log('nextpage');
    var page = globalObject[controlid + "pagerdata"].page;
    if (!isNaN(Number(page)))
        page = Number(page) + 1;
    globalObject[controlid + "pagerdata"].page = page;

    //var connecteid = globalObject[controlid + "pagerdata"].pagerconnectedid;
    //if (connecteid != undefined && connecteid != "" && connecteid != null) {
    //    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    //    var totalrecords = globalObject[controlid + "pagerdata"].totalrecords;
    //    var pagesnumber = Math.ceil(totalrecords / pagesize);
    //    if (Number(page) < totalrecords)
    //        nextpage(connecteid);
    //}


    renderPagerImmobili(controlid, function (result) {
        CaricaIsotopeImmobili(controlid);
    });
    scrolltotop.setting.scrollduration = 0;
    scrolltotop.scrollup();
}
function prevpageimmobili(controlid) {
    // console.log('prevpage');
    var page = globalObject[controlid + "pagerdata"].page;
    if (!isNaN(Number(page)))
        page = Number(page) - 1;
    globalObject[controlid + "pagerdata"].page = page;

    //var connecteid = globalObject[controlid + "pagerdata"].pagerconnectedid;
    //if (connecteid != undefined && connecteid != "" && connecteid != null) {
    //    if (Number(page) <= 1)
    //        prevpage(connecteid);
    //}


    renderPagerImmobili(controlid, function (result) {
        CaricaIsotopeImmobili(controlid);
    });
    scrolltotop.setting.scrollduration = 0;
    scrolltotop.scrollup();
}
function renderPagerImmobili(controlid, callback) {
    //console.log('renderPager');

    var page = globalObject[controlid + "pagerdata"].page;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;

    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var totalrecords = globalObject[controlid + "pagerdata"].totalrecords;
    var pagesnumber = Math.ceil(totalrecords / pagesize);

    $('#' + controlid + 'btnNextPage')[0].innerHTML = baseresources[lng]["pageravanti"];
    $('#' + controlid + 'btnPrevPage')[0].innerHTML = baseresources[lng]["pagerindietro"];

    if (page > pagesnumber) page = pagesnumber;
    if (page < 1) page = 1;
    globalObject[controlid + "pagerdata"].page = page;

    $("#" + controlid + "spantotals")[0].innerHTML = baseresources[lng]["pagertotale"] + " " + totalrecords + "<br/>";
    $("#" + controlid + "divactpage")[0].innerHTML = page + "/" + pagesnumber;
    if (pagesnumber > 1)
        $('#' + controlid + 'divPager').show();
    else
        $('#' + controlid + 'divPager').hide();
    callback('');
}

/*Pager functions end-------------------------------------------------------------------------------*/
