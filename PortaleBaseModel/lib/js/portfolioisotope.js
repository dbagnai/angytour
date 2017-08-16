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
function injectPortfolioAndLoad(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv) {
    loadref(injectPortfolioAndLoadinner, type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv, lng);
}
function injectPortfolioAndLoadinner(type, container, controlid, page, pagesize, enablepager, listShow, tipologia, categoria, visualData, visualPrezzo, maxelement, testoricerca, vetrina, promozioni, connectedid, categoria2Liv) {

 //   console.log('injectPortfolioAndLoadinner' + container);
    var templateHtml = pathAbs + "/lib/template/" + "isotopeOfferte.html";
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
        InitIsotopeLocal(controlid);

        //RICARICO LA PAGINA DALLA SESISONE SE PRESENTE

        getfromsession('objfiltro', function (retval) {
            var objfiltro = {};
            var params = {};
            if (retval != null && retval != '')
                objfiltro = JSON.parse(retval);
            params = objfiltro; //Metto in params tutti i valori presenti nell'objfiltro in session
         //   console.log("injectPortfolioAndLoadinner " + JSON.stringify(objfiltro));

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
            //params.regione = regione; //Preso da sessione
            //params.caratteristica1 = caratteristica1;//Preso da sessione
            globalObject[controlid + "params"] = params;
         //   console.log("injectPortfolioAndLoadinner params " + JSON.stringify(params));

            if (enablepager == "true" || enablepager == true) {
                var pagercontainer = container + "Pager";
                InjectPagerPortfolio(pagercontainer, controlid);
            }
            else {
                CaricaIsotopeData(controlid);
            }
        })
    });
};

function CaricaIsotopeData(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

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
        objfiltro[propname] = page;//aggiungo al filtro in session la pagina attuale

        //Modifica per id controlli connessi per mantenere la pagina corretta nella sessione objfiltro al cambio pagina utente
        if (connecteid != undefined && connecteid != "" && connecteid != null) {
            try {
                var pageconnected = globalObject[connecteid + "pagerdata"].page;
                propname = "page" + connecteid;
                objfiltro[propname] = pageconnected;
            }
            catch (e) { }
        }
        //////////////////////////////////////

        putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) { });
    });

    var functiontocallonend = renderIsotopeNotPaged;
    if (enablepager == "true" || enablepager == true)
        functiontocallonend = renderIsotopePaged;
    caricaDatiServer(lng, objfiltrotmp, page, pagesize, enablepager,
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

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"]
    if (!data.length) {
        $('#' + el).html('');
        return;
    }

    var str = $('#' + el)[0].innerHTML;
    $('#' + el).parent().parent().show();


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
    var htmlitem = "";


    for (var j = 0; j < data.length; j++) {
        htmlitem = "";
        //htmlitem = FillBindControls(jquery_obj, data[j]);
        //htmlout += $(containeritem).html(htmlitem.html()).outerHTML() + "\r\n";
        FillBindControls(jquery_obj, data[j], localObjects, "",
                    function (ret) {
                        htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                    });
    }

    //Inseriamo htmlout nel contenitore  $('#' + el).html e inizializziamo lo scroller
    $('#' + el).html('');
    // Update isotope container with new data. 
    //$('#' + el).isotope('remove',  $('#' + el).data('isotope').$allAtoms );
    $('#' + el).isotope('insert', $(htmlout))
      // trigger isotope again after images have been loaded
      .imagesLoaded(function () {
          $('#' + el).isotope('layout');
          CleanHtml($('#' + el));

      });

};




function InitIsotopeLocal(controlid) {
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


/*Pager functions start-------------------------------------------------------------------------------*/
function initHtmlPager(controlid) {
    //$('#' + controlid + 'btnPrevPage')
    //$('#' + controlid + 'btnNextPage')
    $('#' + controlid + 'btnNextPage').attr('onClick', "javascript:nextpage('" + controlid + "')")
    $('#' + controlid + 'btnPrevPage').attr('onClick', "javascript:prevpage('" + controlid + "')")

    //$('#' + controlid + 'btnNextPage').click(function () { nextpage("'" + controlid + "'") })
    //$('#' + controlid + 'btnPrevPage').click(function () { prevpage("'" + controlid + "'") })

    //$('#' + controlid + 'btnNextPage').on("click", { controlid: controlid }, nextpage);
    //$('#' + controlid + 'btnPrevPage').on("click", { controlid: controlid }, prevpage);
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
function renderPager(controlid, callback) {
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