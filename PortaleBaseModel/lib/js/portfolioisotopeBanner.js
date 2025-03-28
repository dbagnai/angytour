﻿"use strict";
/* Chiamata tipo  injectPortfolioAndLoadBanner(id contenitore destinazione, idunico dello scroller , "rif000003", true, true, 6);*/

/*Da vedere dopo la gestione di questi parametri che per adesso metto globali per farli vedere al pager*/

function InjectPagerPortfolioBanner(pagercontainer, controlid) {

    var templateHtml = pathAbs + "/lib/template/" + "IsotopeBanner.html";

    $("#" + pagercontainer).load(templateHtml, function () {
        /*DA COMPLETARE MODIFICANDO NEL PAGER i VALORI IN MODO CHE CHIAMI CON I PARAMETRI GIUSTI IL CARICAMENTO DATI*/
        $('#' + pagercontainer).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });

        initHtmlPagerBanner(controlid);
        CaricaIsotopeDataBanner(controlid);
    });
}


function injectPortfolioAndLoadBanner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola) {
    setTimeout(function () {
        loadref(injectPortfolioAndLoadBannerinner, type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola, lng);
    }, 100);

}
function injectPortfolioAndLoadBannerinner(type, container, controlid, page, pagesize, enablepager, listShow, maxelement, connectedid, tblsezione, filtrosezione, mescola) {

    //qui devo visualizzare il titolo
    var templateHtml = pathAbs + "/lib/template/" + "IsotopeBanner.html";
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

        //InitIsotopeLocalBanner(controlid);
        //Usiamo memoria globale indicizzata con l'id del controllo
        var pagerdata = {};
        pagerdata["page"] = page;
        pagerdata["pagesize"] = pagesize;
        pagerdata["totalrecords"] = 0;
        pagerdata["enablepager"] = enablepager;
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
            InjectPagerPortfolioBanner(pagercontainer, controlid);
        }
        else {

            CaricaIsotopeDataBanner(controlid);
        }
    });
};

function CaricaIsotopeDataBanner(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

    var page = globalObject[controlid + "pagerdata"].page;
    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;

    var functiontocallonend = renderIsotopeNotPagedBanner;
    if (enablepager == "true" || enablepager == true)
        functiontocallonend = renderIsotopePagedBanner;
    caricaDatiServerBanner(lng, objfiltrotmp, page, pagesize, enablepager,
        function (result, callafterfilter) {
            var localObjects = {};

            try {
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
            catch (e) { }
        },
        functiontocallonend);
};


function renderIsotopeNotPagedBanner(localObjects, controlid) {
    BindIsotopeBanner(controlid, localObjects);//I dati sono già paginati all'origine
};
function renderIsotopePagedBanner(localObjects, controlid) {
    /*Da ultimare  con la paginazione e sistemare il paginatore   .......
     * Passando nella renderPager
     ................... */
    renderPagerBanner(controlid, function (msgpager) {
        //Renderizziamo l'html dei dati nella pagina
        BindIsotopeBanner(controlid, localObjects);//I dati sono già paginati all'origine dal chiamante
    });
}

function BindIsotopeBanner(el, localObjects) {

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"]
    if (!data.length) {
        $('#' + el).html('');
        return;
    }


    var str = $('#' + el)[0].innerHTML; //Elemento da ripetere
    //Se presente nella memoria temporanea globale modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
    //in caso di rebinding successivo dopo l'iniezione del template
    if (!globalObject.hasOwnProperty(el + "template")) {
        globalObject[el + "template"] = $('#' + el)[0].innerHTML;
        str = globalObject[el + "template"];
    }
    else
        str = globalObject[el + "template"];
    var jquery_obj = $(str);

    //var outerhtml = jquery_obj.outerHTML();
    //var innerHtml = jquery_obj.html();
    /*Prendo l'elemento contenitore*/
    //var containeritem = outerhtml.replace(innerHtml, '');
    var htmlout = "";
    for (var j = 0; j < data.length; j++) {
        FillBindControls(jquery_obj, data[j], localObjects, "",
            function (ret) {
                htmlout += (ret.outerHTML()) + "\r\n";
                //htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
            });
    }
    $('#' + el).parent().parent().show(); //titolo
    $('#' + el).parent().parent().parent().parent().show(); //contenitore blocco
     
    //Inseriamo htmlout nel contenitore  $('#' + el).html e inizializziamo lo scroller
    $('#' + el).html((htmlout));
    CleanHtml($('#' + el));
    InitIsotopeLocalBanner(el);
   // setTimeout(function () { InitIsotopeLocalBanner(el); }, 100)

};


function InitIsotopeLocalBanner(controlid) {

    /* ---------------------------------------------- /*
   * Portfolio  https://isotope.metafizzy.co/methods.html
   /* ---------------------------------------------- */
    var worksgrid = $('#' + controlid);
    var worksgrid_mode;
    if (worksgrid.hasClass('works-grid-masonry')) {
        worksgrid_mode = 'masonry';
    } else {
        worksgrid_mode = 'fitRows';
    }

    //Vediamo se già inizializzato in precedenza l'isotope e nel caso eliminiamolo
    var gridcheck = worksgrid.data('isotope');
    if (gridcheck != null) {
        var elems = gridcheck.getFilteredItemElements();
        worksgrid.isotope('destroy');
    }
    // worksgrid.imagesLoaded(function () {
    var $grid = worksgrid.isotope({
        layoutMode: worksgrid_mode,
        itemSelector: '.work-item',
        isInitLayout: false //disable layout on initialization, so you can use methods or add events before the initial layout (altrimenti layoutcomplete no parte e neppure layout).
        //fitRows: {
        //gutter: 10}
    });
    // });

    //$grid.isotope('reloadItems');
    //$grid.isotope('getItemElements')

    worksgrid.imagesLoaded(function () {
        $grid.isotope('layout');
    });

    //On event layoutComplete i make what needed
    //$grid.on('layoutComplete',
    //    function (event, laidOutItems) {
    //        lazyLoad();
    //        //Abilito traking vertical scroll position e reimposto la posizioneiniziale se presente in memoria
    //        enablescrolltopmem = true;
    //        reinitscrollpos(); //Return to previosus scrolltop pos of page
    //    });


    $grid.on('arrangeComplete', function (event, filteredItems) {
        console.log('arrange is complete');
    });
}

/*Pager functions start-------------------------------------------------------------------------------*/
function initHtmlPagerBanner(controlid) {
    //$('#' + controlid + 'btnPrevPage')
    //$('#' + controlid + 'btnNextPage')
    $('#' + controlid + 'btnNextPageBanner').attr('onClick', "javascript:nextpageBanner('" + controlid + "')")
    $('#' + controlid + 'btnPrevPageBanner').attr('onClick', "javascript:prevpageBanner('" + controlid + "')")


    //$('#' + controlid + 'btnNextPage').click(function () { nextpage("'" + controlid + "'") })
    //$('#' + controlid + 'btnPrevPage').click(function () { prevpage("'" + controlid + "'") })

    //$('#' + controlid + 'btnNextPage').on("click", { controlid: controlid }, nextpage);
    //$('#' + controlid + 'btnPrevPage').on("click", { controlid: controlid }, prevpage);
}

function nextpageBanner(controlid) {
    console.log('nextpage');
    var page = globalObject[controlid + "pagerdata"].page;

    if (!isNaN(Number(page)))
        page = Number(page) + 1;
    globalObject[controlid + "pagerdata"].page = page;
    renderPagerBanner(controlid, function (result) {
        CaricaIsotopeDataBanner(controlid);
    });
    scrolltotop.scrollup();
}
function prevpageBanner(controlid) {
    console.log('nextpage');
    var page = globalObject[controlid + "pagerdata"].page;

    if (!isNaN(Number(page)))
        page = Number(page) - 1;
    globalObject[controlid + "pagerdata"].page = page;
    renderPagerBanner(controlid, function (result) {
        CaricaIsotopeDataBanner(controlid);
    });
    scrolltotop.scrollup();
}
function renderPagerBanner(controlid, callback) {
    console.log('renderPagerBanner');

    var page = globalObject[controlid + "pagerdata"].page;
    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;
    var totalrecords = globalObject[controlid + "pagerdata"].totalrecords;

    $('#' + controlid + 'btnNextPageBanner')[0].innerHTML = baseresources[lng]["pageravanti"];
    $('#' + controlid + 'btnPrevPageBanner')[0].innerHTML = baseresources[lng]["pagerindietro"];
    var pagesnumber = Math.ceil(totalrecords / pagesize);
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