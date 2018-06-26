
"use strict";
/* Chiamata tipo  injectPortfolioAndLoad(id contenitore destinazione, idunico dello scroller , "rif000003", true, true, 6);*/
/*Da vedere dopo la gestione di questi parametri che per adesso metto globali per farli vedere al pager*/

function InjectPagerPortfolio(pagercontainer, controlid) {
    var templateHtml = pathAbs + "/lib/template/" + "pagerIsotope.html";
    $("#" + pagercontainer).load(templateHtml, function () {
        /*DA COMPLETARE MODIFICANDO NEL PAGER i VALORI IN MODO CHE CHIAMI CON I PARAMETRI GIUSTI IL CARICAMENTO DATI*/
        $('#' + pagercontainer).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });
        initHtmlPager(controlid);
        CaricaIsotopeData(controlid);
    });
}

function injectPortfolioAndLoad(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, mostviewed) {
    //setTimeout(function () {
    loadref(injectPortfolioAndLoadinner, type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, mostviewed, lng);
    //}, 100);
}
function injectPortfolioAndLoadinner(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, mostviewed) {

    //   console.log('injectPortfolioAndLoadinner' + container);
    var templateHtml = pathAbs + "/lib/template/" + "isotopeOfferte.html";
    if (type != null && type != '')
        templateHtml = pathAbs + "/lib/template/" + type;

    //Correggo l'id dei controlli del template per l'inzializzazione dello scroller con id univoca e corretta
    $('#' + container).html('');

    //$('#' + container).load(templateHtml, function () {
    //    $('#' + container).find("[id^=replaceid]").each(function (index, text) {
    //        var currentid = $(this).prop("id");
    //        var replacedid = currentid.replace('replaceid', controlid);
    //        $(this).prop("id", replacedid);
    //    });


    //RICARICO LA PAGINA INIZIALE DALLA SESISONE SE PRESENTE INVECE DI PRENDERE QUELLA PASSATA NEI PARAMETRI
    getfromsession('objfiltro', function (retval) {
        var objfiltro = {};
        var params = {};
        if (retval != null && retval != '')
            objfiltro = JSON.parse(retval);
        params = objfiltro; //Metto in params tutti i valori presenti nell'objfiltro in session
        //Se presente prendo la pagina in sessione per la selezione di pagina iniziale
        if (objfiltro != null && objfiltro.hasOwnProperty("page" + controlid)) {
            var propname = "page" + controlid;
            page = objfiltro[propname];
        }

        //Usiamo memoria globale indicizzata con l'id del controllo
        var pagerdata = {};
        pagerdata["page"] = page;
        pagerdata["pagesize"] = pagesize;
        pagerdata["totalrecords"] = 0;
        pagerdata["enablepager"] = enablepager;
        pagerdata["pagerconnectedid"] = connectedid;
        globalObject[controlid + "pagerdata"] = pagerdata;

        params.templateHtml = templateHtml;
        params.container = container;/*Inserisco il nome del container nei parametri per uso successivo nel binding*/
        params.tipologia = tipologia;
        params.visualData = visualData;
        params.visualPrezzo = visualPrezzo;
        params.maxelement = maxelement;
        params.listShow = listShow;
        params.categoria = categoria;
        params.categoria2Liv = categoria2Liv;
        params.testoricerca = testoricerca;
        params.vetrina = vetrina;
        params.promozioni = promozioni;
        params.mostviewed = mostviewed;
        //params.regione = regione; //Preso da sessione
        //params.caratteristica1 = caratteristica1;//Preso da sessione
        globalObject[controlid + "params"] = params;
        //console.log("injectPortfolioAndLoadinner params " + JSON.stringify(params));

        if (enablepager == "true" || enablepager == true) {
            var pagercontainer = container + "Pager";
            InjectPagerPortfolio(pagercontainer, controlid);
        }
        else {
            CaricaIsotopeData(controlid);
        }
    })

    //});
};

function CaricaIsotopeData(controlid) {

    var page = globalObject[controlid + "pagerdata"].page;
    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;
    var connecteid = globalObject[controlid + "pagerdata"].pagerconnectedid;

    /*MEMORIZZO LA PAGINA IN SESSIONE*/
    getfromsession('objfiltro', function (retval) {
        var objfiltro = {};
        if (retval != null && retval != '')
            objfiltro = JSON.parse(retval);
        var propname = "page" + controlid;
        objfiltro[propname] = page;//aggiungo al filtro in session la pagina attuale dalla memoria globale javascript
        //Modifica per id controlli connessi per mantenere la pagina corretta nella sessione objfiltro al cambio pagina utente
        if (connecteid != undefined && connecteid != "" && connecteid != null) {
            try {
                var pageconnected = globalObject[connecteid + "pagerdata"].page;
                propname = "page" + connecteid;
                objfiltro[propname] = pageconnected;
            }
            catch (e) { }
        }
        putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) { });
    });
    //////////////////////////////////////

    $(".loaderrelative").show();
    var functiontocallonend = renderIsotopeNotPaged;
    if (enablepager == "true" || enablepager == true)
        functiontocallonend = renderIsotopePaged;

    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];
    caricaDatiServer(lng, objfiltrotmp, page, pagesize, enablepager,
        function (result, callafterfilter) {
            var localObjects = {};

            try {

                // caso particolare in cui potrei sommare i risultati precedenti in memoria q quelli ritornati per avere la modalità VISUALIZZA ALTRI in aggiunta ai presenti
                if (result != '') {
                    //Devo ricaricare il template se non presente
                    var parseddata = JSON.parse(result);
                    var temp = parseddata["resultinfo"];
                    localObjects["resultinfo"] = JSON.parse(temp);//Dictionary parserizzato dei parametri calcolati generali
                    var totalrecords = localObjects["resultinfo"].totalrecords;
                    globalObject[controlid + "pagerdata"].totalrecords = totalrecords;
                    var data = "{ \"datalist\":" + parseddata["data"];
                    data += "}";
                    localObjects["dataloaded"] = data;
                    var datalink = parseddata["linkloaded"];  //link creati presi da tabella
                    //Inserisco i valori nella memoria generale che contiene i valori per tutti i componenti
                    localObjects["linkloaded"] = JSON.parse(datalink);

                    //Ricarico il template in pagina per il binding(serve se il primo binding è stato fatto lato server altrimrn il template non lo trovo in pagina)
                    var container = objfiltrotmp.container;
                    var templateHtml = objfiltrotmp.templateHtml;
                    $('#' + container).html('');
                    $('#' + container).load(templateHtml, function () {
                        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
                            var currentid = $(this).prop("id");
                            var replacedid = currentid.replace('replaceid', controlid);
                            $(this).prop("id", replacedid);
                        });
                        callafterfilter(localObjects, controlid);
                    });
                }
            }
            catch (e) {
                $(".loaderrelative").hide();
            }
        },
        functiontocallonend);
};


function renderIsotopeNotPaged(localObjects, controlid) {
    BindIsotope(controlid, localObjects);//I dati sono già paginati all'origine
};
function renderIsotopePaged(localObjects, controlid) {
    /*Da ultimare  con la paginazione e sistemare il paginatore   .......
     * Passando nella renderPager
     ................... */
    renderPager(controlid, function (msgpager) {
        //Renderizziamo l'html dei dati nella pagina
        BindIsotope(controlid, localObjects);//I dati sono già paginati all'origine dal chiamante
    });
}

function BindIsotope(el, localObjects) {

    try {
        var objcomplete = JSON.parse(localObjects["dataloaded"]);
        var data = objcomplete["datalist"]
        if (!data.length) {
            $('#' + el).html('');
            $(".loaderrelative").hide();
            return;
        }

        var objfiltrotmp = {};
        objfiltrotmp = globalObject[el + "params"];
        var container = objfiltrotmp.container;

        var str = $('#' + el)[0].innerHTML; //elemento da ripetere ( preso dalla pagina )
        ////////////////////////////////
        //Memorizzo il template per eventuali rebind lato client
        //if (!globalObject.hasOwnProperty(el + "template")) {
        //    globalObject[el + "template"] = $('#' + el)[0].innerHTML;
        //    str = globalObject[el + "template"];
        //} else
        //    str = globalObject[el + "template"];
        ///////////////////////////////////

        var alternate = ($('#' + el).hasClass('alternatecolor'));
        var jquery_obj = $(str);
        //var outerhtml = jquery_obj.outerHTML();
        //var innerHtml = jquery_obj.html();
        //  var containeritem = outerhtml.replace(innerHtml, '');/*Prendo l'elemento contenitore vuoto da ripetere*/
        var htmlout = "";
        // var htmlitem = "";

        for (var j = 0; j < data.length; j++) {
            // htmlitem = "";
            //htmlitem = FillBindControls(jquery_obj, data[j]);
            //htmlout += $(containeritem).html(htmlitem.html()).outerHTML() + "\r\n";
            FillBindControls(jquery_obj, data[j], localObjects, "",
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

                    htmlout += (ret.outerHTML()) + "\r\n";
                });
        }

        //Inseriamo htmlout nel contenitore  $('#' + el).html e inizializziamo lo scroller
        $('#' + el).html((htmlout));
        CleanHtml($('#' + el));
        InitIsotopeLocal(el, container);

        $(".loaderrelative").hide();
    } catch (e) { $(".loaderrelative").hide(); }
};


function InitIsotopeLocal(controlid, container) {


    $('#' + container + 'Title').show();
    $('#' + controlid).parent().parent().show(); //Row COntainer
    $('#' + controlid).parent().parent().parent().parent().show();

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
    $grid.on('layoutComplete',
        function (event, laidOutItems) {
            lazyLoad();
            //Abilito traking vertical scroll position e reimposto la posizioneiniziale se presente in memoria
            enablescrolltopmem = true;
            reinitscrollpos(); //Return to previosus scrolltop pos of page
        });


    $grid.on('arrangeComplete', function (event, filteredItems) {
        console.log('arrange is complete');
    });

}



/*Pager functions per client side binding start-------------------------------------------------------------------------------*/

function initHtmlPager(controlid) {
    $('#' + controlid + 'btnNextPage').attr('onClick', "javascript:nextpage('" + controlid + "')")
    $('#' + controlid + 'btnPrevPage').attr('onClick', "javascript:prevpage('" + controlid + "')")
}
function renderPager(controlid, callback) {
    //console.log('renderPager');
    var page = globalObject[controlid + "pagerdata"].page;
    var pagesize = globalObject[controlid + "pagerdata"].pagesize;
    var enablepager = globalObject[controlid + "pagerdata"].enablepager;
    var totalrecords = globalObject[controlid + "pagerdata"].totalrecords;

    var pagesnumber = Math.ceil(totalrecords / pagesize);
    if (page > pagesnumber) page = pagesnumber;
    if (page < 1) page = 1;
    globalObject[controlid + "pagerdata"].page = page;

    $('#' + controlid + 'btnNextPage')[0].innerHTML = baseresources[lng]["pageravanti"];
    $('#' + controlid + 'btnNextPage').show();
    $('#' + controlid + 'btnPrevPage')[0].innerHTML = baseresources[lng]["pagerindietro"];
    $('#' + controlid + 'btnPrevPage').show();
    $("#" + controlid + "spantotals")[0].innerHTML = baseresources[lng]["pagertotale"] + " " + totalrecords + "<br/>";
    //$('#' + controlid + 'btnNextPage')[0].innerHTML = 'Avanti';
    //$('#' + controlid + 'btnPrevPage')[0].innerHTML = 'Indietro';
    //$("#" + controlid + "spantotals")[0].innerHTML = 'Totale ' + " " + totalrecords + "<br/>";
    $("#" + controlid + "divactpage")[0].innerHTML = page + "/" + pagesnumber;

    if (pagesnumber > 1)
        $('#' + controlid + 'divPager').show();
    else
        $('#' + controlid + 'divPager').hide();
    if (callback != null)
        callback('');
}

function nextpage(controlid) {
    //   console.log('nextpage');
    var page = globalObject[controlid + "pagerdata"].page;
    if (!isNaN(Number(page)))
        page = Number(page) + 1;
    globalObject[controlid + "pagerdata"].page = page;

    var connecteid = globalObject[controlid + "pagerdata"].pagerconnectedid;
    if (connecteid != undefined && connecteid != "" && connecteid != null) {
        var pagesize = globalObject[controlid + "pagerdata"].pagesize;
        var totalrecords = globalObject[controlid + "pagerdata"].totalrecords;
        var pagesnumber = Math.ceil(totalrecords / pagesize);
        if (Number(page) < totalrecords)
            nextpage(connecteid);
    }
    renderPager(controlid, function (result) {
        CaricaIsotopeData(controlid);
    });
    scrolltotop.setting.scrollduration = 0;
    scrolltotop.scrollup();
}
function prevpage(controlid) {
    // console.log('prevpage');
    var page = globalObject[controlid + "pagerdata"].page;
    if (!isNaN(Number(page)))
        page = Number(page) - 1;
    globalObject[controlid + "pagerdata"].page = page;

    var connecteid = globalObject[controlid + "pagerdata"].pagerconnectedid;
    if (connecteid != undefined && connecteid != "" && connecteid != null) {
        if (Number(page) <= 1)
            prevpage(connecteid);
    }
    renderPager(controlid, function (result) {
        CaricaIsotopeData(controlid);
    });
    scrolltotop.setting.scrollduration = 0;
    scrolltotop.scrollup();
}
/*Pager functions per client side binding end-------------------------------------------------------------------------------*/

/**
PAGINAZIONE CON BINDING SERVER SIDE -----------------------------------
 */
function initGlobalVarsFromServer(controlid, dictpars, dictpagerpars) {
    //console.log('initGlobalVarsFromServer');
    if (dictpars != null && dictpars != '') {
        globalObject[controlid + "params"] = JSON.parse(dictpars);
    }
    if (dictpagerpars != null && dictpagerpars != '') {
        globalObject[controlid + "pagerdata"] = JSON.parse(dictpagerpars);
    }
}
function nextpagebindonserver(controlid) {

    var page = globalObject[controlid + "pagerdata"].page;
    if (!isNaN(Number(page)))
        page = Number(page) + 1;
    var pagesnumber = Math.ceil(globalObject[controlid + "pagerdata"].totalrecords / globalObject[controlid + "pagerdata"].pagesize);
    var pageover = false;
    if (page > pagesnumber) { page = pagesnumber; pageover = true; };
    if (page < 1) { page = 1; pageover = true };
    globalObject[controlid + "pagerdata"].page = page;
    scrolltotop.setting.scrollduration = 0;
    scrolltotop.scrollup();
    /*MEMORIZZO LA PAGINA IN SESSIONE (per mantenere la posizione di pagina al cambio di url)*/
    getfromsession('objfiltro', function (retval) {
        var objfiltro = {};
        if (retval != null && retval != '')
            objfiltro = JSON.parse(retval);
        var propname = "page" + controlid;
        objfiltro[propname] = page;//aggiungo al filtro in session la pagina attuale dalla memoria globale javascript
        putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) {

            if (!pageover)
                CaricaDatiServerBinded(lng, globalObject[controlid + "params"], globalObject[controlid + "pagerdata"].page, globalObject[controlid + "pagerdata"].pagesize, globalObject[controlid + "pagerdata"].enablepager, function (result) {
                    var containerid = globalObject[controlid + "params"].container;
                    if (result != '') {
                        try {
                            //Inserisco un elemento di ancor per appendere i dati in pagina prima del contenitore corrente
                            $("#" + containerid).before("<span id='" + containerid + "-pr'></span>");
                            //Elimino i vecchi elementi dal contenitore con la pagina precedente
                            $("#" + containerid).remove();
                            $("#" + containerid + "Pager").remove();
                            var resparsed = JSON.parse(result);
                            //inseriscso l'html renderizzato nel contenitore
                            $("#" + containerid + "-pr").after(resparsed["html"]);
                            // devo scorrere resparsed["jscommands"] e chiamare le funzioni li presenti con  window[itemtocall.name].apply(this, itemtocall.args) o eval ( ...)  .. da finire
                            if (resparsed != null && resparsed.hasOwnProperty("jscommands"))
                                for (var key in resparsed["jscommands"]) {
                                    if (resparsed["jscommands"].hasOwnProperty(key)) {
                                        try {
                                            //console.log(key, resparsed["jscommands"][key]);
                                            eval(resparsed["jscommands"][key]);//Eseguiamo i comandi indicati dal server
                                        } catch{ }
                                    }
                                }
                        } catch { }
                    }
                });

        });
    });

}

function prevpagebindonserver(controlid) {

    var page = globalObject[controlid + "pagerdata"].page;
    if (!isNaN(Number(page)))
        page = Number(page) - 1;
    var pagesnumber = Math.ceil(globalObject[controlid + "pagerdata"].totalrecords / globalObject[controlid + "pagerdata"].pagesize);
    var pageover = false;
    if (page > pagesnumber) { page = pagesnumber; pageover = true; };
    if (page < 1) { page = 1; pageover = true };
    globalObject[controlid + "pagerdata"].page = page;
    scrolltotop.setting.scrollduration = 0;
    scrolltotop.scrollup();
    /*MEMORIZZO LA PAGINA IN SESSIONE (per mantenere la posizione di pagina al cambio di url)*/
    getfromsession('objfiltro', function (retval) {
        var objfiltro = {};
        if (retval != null && retval != '')
            objfiltro = JSON.parse(retval);
        var propname = "page" + controlid;
        objfiltro[propname] = page;//aggiungo al filtro in session la pagina attuale dalla memoria globale javascript
        putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) {

            if (!pageover)
                CaricaDatiServerBinded(lng, globalObject[controlid + "params"], globalObject[controlid + "pagerdata"].page, globalObject[controlid + "pagerdata"].pagesize, globalObject[controlid + "pagerdata"].enablepager, function (result) {
                    var containerid = globalObject[controlid + "params"].container;
                    if (result != '') {
                        try {
                            //Inserisco un elemento di ancor per appendere i dati in pagina
                            $("#" + containerid).before("<span id='" + containerid + "-pr'></span>");
                            //Elimino i vecchi elementi
                            $("#" + containerid).remove();
                            $("#" + containerid + "Pager").remove();
                            var resparsed = JSON.parse(result);
                            //inseriscso l'html renderizzato nel contenitore
                            $("#" + containerid + "-pr").after(resparsed["html"]);
                            // devo scorrere resparsed["jscommands"] e chiamare le funzioni li presenti con  window[itemtocall.name].apply(this, itemtocall.args) o eval ( ...)  .. da finire
                            if (resparsed != null && resparsed.hasOwnProperty("jscommands"))
                                for (var key in resparsed["jscommands"]) {
                                    if (resparsed["jscommands"].hasOwnProperty(key)) {
                                        try {
                                            //f(key, resparsed["jscommands"][key]);
                                            eval(resparsed["jscommands"][key]);
                                        } catch{ }
                                    }
                                }
                        } catch { }
                    }
                });


        });
    });

}
/**
PAGINAZIONE CON BINDING SERVER SIDE END -----------------------------------
 */