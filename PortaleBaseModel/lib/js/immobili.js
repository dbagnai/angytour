"use strict";

var page = 1;
var pagesize = 18;
var totalrecords = 0;
var tmpLocalObjects = {}; /*Memoria locale per utilizzo in page*/
var idcurrent = "";

function InitImmobiliJS(id) {

    /*FUNZIONI DI AGGIORNAMENTO ?upd=true pppore upd=sitemaps*/
    var updateresources = $.getQueryString("upd");
    if (updateresources == 'true') /*Ablitazione aggiornamento dati immobiliari, mettere una condizione temporale per farlo!*/
        CaricaDatiImmobilidaGestionale(); //Aggiornamento dati immobiliari
    if (updateresources == 'sitemaps')
        GeneraSitemapsImmobili();
    tmpLocalObjects = {};

    //console.log('initimmobili');

   // CaricaAllegatiImmobili(function () {
        InjectPager();
        injectGalleryControls("plhGallery");
        if (id != null && id == 'caricascrollervetrina') {
            CaricaScrollerVetrina();
            //$("#titlespan").html(GetResourcesValue("Proposte"));
        }
        else if (id != null && id != '') {
            renderImmobile(id);
        }
        else 
            (injectIsotopeImmobili("divPlaceholderIsotopeList"));

   // });
}


function CaricaScrollerVetrina() {
    var params = [];
    params[0] = "";
    params[1] = "";
    params[2] = "true";
    injectScrollerImmobiliAndLoad("divJSScrollerContainer1", params);
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
                console.log('successly imported estates');
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
        url: pathAbs + resourcehandlerpath,
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

/* NON USATA */
//function CreaLinksSchedaDettaglio(data, callback) {
//    var data = data || '';
//    data = JSON.stringify(data);
//    $.ajax({
//        url: pathAbs +  resourcehandlerpath,
//        //contentType: "application/json; charset=utf-8",
//        global: false,
//        cache: false,
//        async: false,
//        dataType: "text",
//        type: "POST",
//        data: { 'q': 'linklistdetail', 'dataitem': data, 'completedata': true, 'lng': lng },
//        success: function (result) {
//            //var dataret = "{ \"data\":" + result;
//            //dataret += "}";
//            callback(result);
//        },
//        error: function (result) {
//            //sendmessage('fail creating link');
//            callback('');
//        }
//    });
//}

function InjectPager() {
    $("#divPagerPlaceholder").load(pathAbs + "/lib/template/" + "pager.html", function ()
    { });
}

//plhSimilari
function injectScrollerImmobiliAndLoad(container, params) {
    $('#' + container).load(pathAbs + "/lib/template/" + "owlscroller.html", function () {

        //Qui puoi fare inizializzazione controlli su allegati
        CaricaScroller(params);
    });
}
function CaricaScroller(params) {
    var objfiltrotmp = {};

    var idtipologia = params[0];
    var regione = params[1];
    var vetrina = params[2];

    objfiltrotmp["ddlTipologiaSearch"] = idtipologia;
    objfiltrotmp["ddlRegioneSearch"] = regione;
    objfiltrotmp["vetrina"] = vetrina;
    //FiltraListaImmobiliClientside(objfiltrotmp, renderScrollerList)

    caricaImmobilidaServer(lng, objfiltrotmp, 1, 20, true,
        function (result, callafterfilter) {

            var parsedresult = JSON.parse(result);

            totalrecords = parsedresult["resultsinfo"][0].totalRecords;
            var strresources = "{ \"resource\":" + JSON.stringify(parsedresult["resource"]);
            strresources += "}";
            tmpLocalObjects["resourcesloaded"] = strresources;
            //Prendo le liste immobili filtrate in base alla query fatta
            tmpLocalObjects["imgsprimary"] = JSON.stringify(parsedresult["imgsprimary"][0]);
            tmpLocalObjects["imgscomplete"] = JSON.stringify(parsedresult["imgscomplete"][0]);
            tmpLocalObjects["linklistdetail"] = parsedresult["linklistdetail"][0]; //lista dei link per gli immobili passati


            callafterfilter();
        },
        renderScrollerList);

}
function renderScrollerList() {
    /* Prendo i primi end oggetti dalla lista completa*/
    var objcomplete = JSON.parse(tmpLocalObjects["resourcesloaded"]);
    //var start = 0;
    //var end = 20;
    //if (objcomplete["resource"].length < end) end = objcomplete["resource"].length;
    //var objlimited = JSON.search(objcomplete, '//resource[position()>=' + start + ' and  position()<=' + end + ']');
    //BindScroller("carouselInject1", objlimited);
    BindScroller("carouselInject1", objcomplete["resource"]);//I dati sono già paginati all'origine
}
function BindScroller(el, data) {

    //Carichiamo la lista dei link e i dati di dettaglio per gli immobili
    //CreaLinksSchedaDettaglio(data,
    //      function (result) {
    //          /*Svuoto e ricarico a tutte le richieste*/
    //          if (tmpLocalObjects.hasOwnProperty("linklistdetail")) {
    //              tmpLocalObjects["linklistdetail"] = "";
    //          }
    //          try {
    //              tmpLocalObjects["linklistdetail"] = JSON.parse(result);
    //          }
    //          catch (e) { }
    //      });

    var str = $('#' + el)[0].innerHTML;
    $('#' + el).parent().parent().parent().parent().show();

    //Se presente nella memoria temporanea modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
    if (!tmpLocalObjects.hasOwnProperty(el)) {
        tmpLocalObjects[el] = $('#' + el)[0].innerHTML;
        str = tmpLocalObjects[el];
    }
    else
        str = tmpLocalObjects[el];


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
        FillBindControls(jquery_obj, data[j], "",
                    function (ret) {
                        htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                    });
    }

    //Inseriamo htmlout nel contenitore  $('#' + el).html e inizializziamo lo scroller
    $('#' + el).html(htmlout);
    CleanHtml($('#' + el));
    setTimeout(function () {
        ScrollerInit(el);
    }, 100);

}
function ScrollerInit(scrollerid) {
    jQuery(document).ready(function () {
        var owl = jQuery("#" + scrollerid);
        owl.owlCarousel({
            items: [4],
            autoPlay: 5000,
            itemsDesktop: [1199, 3], // i/tems between 1000px and 601px
            itemsTablet: [979, 2], // items between 600 and 0;
            itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
            slideSpeed: 1000
        });

        // Custom Navigation Events
        jQuery("#" + scrollerid + "next").click(function () {
            owl.trigger('owl.next');
        })
        jQuery("#" + scrollerid + "prev").click(function () {
            owl.trigger('owl.prev');
        })
    });

}


function injectIsotopeImmobili(container, params) {
    $('#' + container).html('');
    $('#' + container).load(pathAbs + "/lib/template/" + "isotope.html", function () {
        InitIsotopeNoimagesloaded();
        //Qui puoi fare inizializzazione controlli su allegati
        FiltraImmobili(params);
    });
}

function InitIsotopeNoimagesloaded() {
    /* ---------------------------------------------- /*
    * Portfolio ( da rivals )
    /* ---------------------------------------------- */
    var worksgrid = $('.works-grid');
    var worksgrid_mode;
    if (worksgrid.hasClass('works-grid-masonry')) {
        worksgrid_mode = 'masonry';
    } else {
        worksgrid_mode = 'fitRows';
    }

    // worksgrid.imagesLoaded(function () {
    worksgrid.isotope({
        layoutMode: worksgrid_mode,
        itemSelector: '.work-item'
        //fitRows: {
        //gutter: 10}
    });
    //});
}

function FiltraImmobili(params) {
    //console.log(tmpLocalObjects["resource"]);
    var objfiltro = {};

    var retstring = "";
    getfromsession('objfiltro', function (retval) {
        retstring = retval;
        if (retstring != null && retstring != '')
            objfiltro = JSON.parse(retstring);

        //MEMORIZZAZIONE IN SESSIONE DELLA PAGE
        if (objfiltro.hasOwnProperty("page"))
            page = objfiltro.page
        if (params != null && params.length > 0) {
            page = params[0];
            objfiltro.page = page;//aggiungo al filtro in session la pagina attuale
            putinsession('objfiltro', JSON.stringify(objfiltro), function (ret) { });
        }

        //ESEGUIAMO LA RICERCA E VISUALIZZAZIONE RISULTATI IMMOBILI VIA AJAX CALL
        caricaImmobilidaServer(lng, objfiltro, page, pagesize, true,
        function (result, callafterfilter) {
            var parsedresult = JSON.parse(result);

            totalrecords = parsedresult["resultsinfo"][0].totalRecords;
            var strresources = "{ \"resource\":" + JSON.stringify(parsedresult["resource"]);
            strresources += "}";
            tmpLocalObjects["resourcesloaded"] = strresources;
            //Prendo le liste immobili filtrate in base alla query fatta
            tmpLocalObjects["imgsprimary"] = JSON.stringify(parsedresult["imgsprimary"][0]);
            tmpLocalObjects["imgscomplete"] = JSON.stringify(parsedresult["imgscomplete"][0]);
            tmpLocalObjects["linklistdetail"] = parsedresult["linklistdetail"][0]; //lista dei link per gli immobili passati

            callafterfilter();
        },
            //renderListClientpaged  
        renderListServerpaged);

    });
}

function renderListServerpaged() {
    $('#divSearchBar').modal('hide');
    renderPager(function (msgpager) {
        //Renderizziamo l'html dei dati nella pagina
        if (tmpLocalObjects["resourcesloaded"] != null && tmpLocalObjects["resourcesloaded"].length != 0) {
            BindList("ullist1", JSON.parse(tmpLocalObjects["resourcesloaded"])["resource"]);
        }
    });
}


function BindList(el, data) {
    //Carichiamo la lista dei link e i dati di dettaglio per gli immobili
    //CreaLinksSchedaDettaglio(data,
    //      function (result) {
    //          /*Svuoto e ricarico a tutte le richieste*/
    //          if (tmpLocalObjects.hasOwnProperty("linklistdetail")) {
    //              tmpLocalObjects["linklistdetail"] = "";
    //          }
    //          try {
    //              tmpLocalObjects["linklistdetail"] = JSON.parse(result);
    //          }
    //          catch (e) { }
    //      });

    var str = $('#' + el)[0].innerHTML;
    $('#' + el).parent().parent().show();

    //var $jquery_obj = $($.parseHTML(str));//https://api.jquery.com/jquery.parsehtml/
    //Se presente nella memoria temporanea modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
    if (!tmpLocalObjects.hasOwnProperty(el)) {
        tmpLocalObjects[el] = $('#' + el)[0].innerHTML;
        str = tmpLocalObjects[el];
    }
    else
        str = tmpLocalObjects[el];
    var jquery_obj = $(str);

    //var outer_html = jquery_obj[0].outerHTML;
    //var outercontainer_html = jquery_obj.clone().wrap('<p>').parent().html();
    //var test1 = jquery_obj.wrapAll('<div></div>').parent().html();
    //var test1 = jquery_obj.clone().wrap('<p>').parent().html();

    var outerhtml = jquery_obj.outerHTML();
    var innerHtml = jquery_obj.html();
    var containeritem = outerhtml.replace(innerHtml, '');/*Prendo l'elemento contenitore*/
    var htmlout = "";
    var htmlitem = "";
    for (var j = 0; j < data.length; j++) {
        //htmlitem = "";
        FillBindControls(jquery_obj, data[j], "",
              function (ret) {
                  htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
              });
    }

    /*RIPULIAMO L'HTML DALLE CLASSI USATE PER IL BINDING*/
    //htmlout = $('.bind', htmlout).removeClass('bind').html();
    //htmlout = $('[mybind]', htmlout).removeAttr("mybind").html();
    //htmlout = $('[format]', htmlout).removeAttr("format").html();
    //$('<div>').append($(htmlout)) //Inserisce il contenuto in un div -> poi dovrei fare il replace....
    // $('.bind', htmlout).removeClass('bind').html();


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
}

function CaricaAllegatiImmobili(callback) {

    //Carichiamo gli allegati primary per la visualizzazione
    // if (!tmpLocalObjects.hasOwnProperty("imgsprimary") || tmpLocalObjects["imgsprimary"].length == 0)
    CaricaListaAllegatiPrimari(lng,
       function (result) {
           if (tmpLocalObjects.hasOwnProperty("imgsprimary")) {
               tmpLocalObjects["imgsprimary"] = "";
           }
           tmpLocalObjects["imgsprimary"] = result;
           CaricaListaCompletaAllegati(lng,
                    function (result) {
                        if (tmpLocalObjects.hasOwnProperty("imgscomplete")) {
                            tmpLocalObjects["imgscomplete"] = "";
                        }
                        tmpLocalObjects["imgscomplete"] = (result);
                        callback();
                    });
       });
    //Carichiamo gli listacompleta primary per la visualizzazione
    // if (!tmpLocalObjects.hasOwnProperty("imgscomplete") || tmpLocalObjects["imgscomplete"].length == 0)
    // CaricaListaCompletaAllegati(lng,
    //function (result) {
    //    if (tmpLocalObjects.hasOwnProperty("imgscomplete")) {
    //        tmpLocalObjects["imgscomplete"] = "";
    //    }
    //    tmpLocalObjects["imgscomplete"] = (result);
    //});


}

/*Riceve una stringa Html parserizzata con jquery per il fill coi dati*/
function FillBindControls(jquery_obj, dataitem, classselector, callback) {
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

                        if (tmpLocalObjects["linklistdetail"].hasOwnProperty(idscheda)) {
                            if (tmpLocalObjects["linklistdetail"][idscheda].hasOwnProperty(bindprophref)) {
                                link = tmpLocalObjects["linklistdetail"][idscheda][bindprophref];
                                $(this).attr("href", link);
                            }
                            if (tmpLocalObjects["linklistdetail"][idscheda].hasOwnProperty(bindproptitle)) {
                                testo = tmpLocalObjects["linklistdetail"][idscheda][bindproptitle];
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
                            CompleteUrlPrimaryImg(idallegato, true, usecdn, function (imgret) {
                                completepath = imgret;
                            });
                            if (completepath != null && completepath != '')
                                $(this).attr("src", completepath);
                        }
                        //else
                        //    $(this).attr("src", '');
                    }
                    else if ($(this).is("div")
                        && ($(this).hasClass('owl-carousel') || $(this).hasClass('img-list'))
                        ) {
                        var imgslist = "";
                        var idallegato = dataitem[proprarr[0]];
                        /*Lista completa degli allegati per l'immobile*/
                        CompleteUrlListImgs(idallegato, false, usecdn, function (imgret) {
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
                        $(this).parent().show();
                    }
                        //else if ($(this).is("div")
                        //    && ($(this).hasClass('owl-slider-v2'))
                        //    ) {
                        //    var contenutoslide = "";

                        //}
                    else {
                        if (dataitem.hasOwnProperty(proprarr[0])) {
                            var valore = [];
                            valore[0] = dataitem[proprarr[0]];
                            var formatfunc = "";
                            if ($(this).attr("format") != null) {
                                formatfunc = $(this).attr("format");
                                if ($(this).attr("mybind2") != null)
                                    valore[1] = dataitem[$(this).attr("mybind2")];
                                window[formatfunc](valore, function (ret) {
                                    // valore = ret;
                                    if (ret != null && Array.isArray(ret) && ret.length > 0)
                                        valore = ret[0];
                                    else
                                        valore = ret;
                                });
                            }
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
                        if (tmpLocalObjects["linklistdetail"].hasOwnProperty(idelement)) {
                            if (tmpLocalObjects["linklistdetail"][idelement].hasOwnProperty(property)) {
                                valore = tmpLocalObjects["linklistdetail"][idelement][property];
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
                        if (tmpLocalObjects["linklistdetail"].hasOwnProperty(idelement)) {
                            if (tmpLocalObjects["linklistdetail"][idelement].hasOwnProperty(property)) {
                                if (valore != null && valore != '') {
                                    valore = tmpLocalObjects["linklistdetail"][idelement][property];
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

//function formatlabelresource(valore, callback) {
//    var retstring = "";
//    try {
//        retstring = baseresources[lng][valore];
//    } catch (e) { };
//    callback(retstring);

//}
/*Funzioni di formattazione personalizzate per il binding dei dati */
//function frmprovincia(valore, callback) {
//    var dataroot = {};
//    dataroot.data = JSONprovince;
//    var selvalue = JSON.search(dataroot, '//data[Codice=\"' + valore[0] + '\"]/Provincia');
//    callback(selvalue);
//}
//function frmregione(valore, callback) {
//    var dataroot = {};
//    dataroot.data = JSONregioni;
//    var selvalue = JSON.search(dataroot, '//data[Codice=\"' + valore[0] + '\"]/Regione');
//    callback(selvalue);
//}
function frmcontratto(valore, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONreftipocontratto);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONreftipocontratto;

    var selvalue = JSON.search(dataroot, '//data[id=' + valore[0] + ']//dettaglitipocontratto[lingua="' + lng + '"]/titolo');
    callback(selvalue);
}
function frmtiporisorsa(valore, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONreftiporisorse);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONreftiporisorse;

    var selvalue = JSON.search(dataroot, '//data[id=' + valore[0] + ']//dettaglitiporisorse[lingua="' + lng + '"]/titolo');
    callback(selvalue);
}
function frmcondizione(valore, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONrefcondizione);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONrefcondizione;

    var selvalue = JSON.search(dataroot, '//data[id=' + valore[0] + ']//dettaglicondizione[lingua="' + lng + '"]/titolo');
    callback(selvalue);
}
function frmprezzo(valore, callback) {
    var retstring = "";
    var unit = baseresources[lng]["Valuta"];
    retstring = unit + ' ' + valore[0].formatMoney(0, '.', ',');
    var testoriservato = baseresources[lng]["Riservato"];
    var condizione = "";
    if (valore.length > 1)
        condizione = valore[1];
    if (condizione == true)
        retstring = testoriservato;
    callback(retstring);
}
function frmsuperficie(valore, callback) {
    var unit = baseresources[lng]["UMsuperficie"];
    callback(valore[0] + ' ' + unit);
}
function frmterreno(valore, callback) {
    var unit = baseresources[lng]["UMTerreno"];
    callback(valore[0] + ' ' + unit);
}
function frmcamere(valore, callback) {
    var lbl = baseresources[lng]["CamereLbl"];
    //callback("<i class=\"fa fa-bed\"></i>" + ":" + valore[0]);
    callback(lbl + ": " + valore[0]);
}
function frmbagni(valore, callback) {
    var lbl = baseresources[lng]["BagniLbl"];
    //callback("<i class=\"fa fa-bed\"></i>" + ":" + valore[0]);
    callback(lbl + ": " + valore[0]);
}
function frmcodice(valore, callback) {
    var pre = baseresources[lng]["Codice"];
    callback(' ' + pre + ' ' + valore[0]);
}
function frmipe(valore, callback) {
    var unit = baseresources[lng]["UMipe"];
    callback(valore[0] + ' ' + unit);
}


function renderImmobile(id) {
    var datiritorno = "";
    CaricaResource(id, lng, function (res) {
        datiritorno = JSON.parse(res);
        /*Inserisco nella memoria locale le tabelle dettaglio per l'immobile*/
        tmpLocalObjects["linklistdetail"] = JSON.parse(datiritorno["linklistdetail"]);
        tmpLocalObjects["imgsprimary"] = datiritorno["imgsprimary"];
        tmpLocalObjects["imgscomplete"] = datiritorno["imgscomplete"];
        var resourceparsed = JSON.parse(datiritorno["resource"]);
        //Prepariamo la visualizzazione immobile
        BindItem("divItem1", resourceparsed);
    });
}
function BindItem(el, dataresource) {
    var str = "";

    //Carichiamo la lista dei link e i dati di dettaglio per l'immobile (non serve viene caricata alla load dei dati delle risorse )
    //var datarray = [];
    //datarray.push(localparseddata);
    //CreaLinksSchedaDettaglio(datarray,
    //      function (result) {
    //          /*Svuoto e ricarico a tutte le richieste*/
    //          if (tmpLocalObjects.hasOwnProperty("linklistdetail")) {
    //              tmpLocalObjects["linklistdetail"] = "";
    //          }
    //          try {
    //              tmpLocalObjects["linklistdetail"] = JSON.parse(result);
    //          }
    //          catch (e) { }
    //      });

    //Prendiamo il modello dalla pagina o dalla memoria locale
    if (!tmpLocalObjects.hasOwnProperty(el)) {
        tmpLocalObjects[el] = $('#' + el)[0].innerHTML;
        str = tmpLocalObjects[el];
    }
    else
        str = tmpLocalObjects[el];
    $('#' + el).show();
    var jquery_obj = $(str);
    //var outerhtml = $('#' + el).outerHTML();
    //var innerHtml = $('#' + el).html();
    //var containeritem = outerhtml.replace(innerHtml, '');/*Prendo l'elemento contenitore*/
    var htmlout = "";
    var htmlitem = "";
    //htmlitem = FillBindControls($('<div>').append(jquery_obj), dataresource);

    FillBindControls($('<div>').append(jquery_obj), dataresource, "",
              function (ret) {
                  $('#' + el).html(ret.html());
                  setTimeout(
                      rebuildCarousel(), 100);
                  CleanHtml($('#' + el));
                  /*-------------------------------------------------------*/
                  //Carichiamo lo scroller con la  la lista dei suggeriti!!
                  /*-------------------------------------------------------*/
                  var params = [];
                  params[0] = dataresource.idtipologia;
                  params[1] = dataresource.codiceREGIONE;
                  params[2] = '';
                  injectScrollerImmobiliAndLoad("plhSimilari", params);

              });
}



function CaricaResource(id, lng, callback) {
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
            callback(result);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback('');
        }
    });
}



function CompleteUrlPrimaryImg(idallegati, anteprima, usecdn, callback) {
    var pathfile = "";
    try {
        //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/NomeFile
        //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/Descrizione
        var strfind = "{ \"imgs\" : [" + tmpLocalObjects["imgsprimary"];
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
function CompleteUrlListImgs(idallegati, anteprima, usecdn, callback) {
    var arrayimgs = [];
    try {
        //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/NomeFile
        //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/Descrizione
        var strfind = "{ \"imgs\" : [" + tmpLocalObjects["imgscomplete"];
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


function injectGalleryControls(container) {
    //http://owlgraphic.com/owlcarousel/demos/manipulations.html
    $("#" + container).load(pathAbs + "/lib/template/" + "owlslider1.html", function () {
        //Qui puoi fare inizializzazione controlli su allegati
        $("#owl-gallery").attr("mybind", 'id_allegati');
    });
}
function rebuildCarousel() {
    graphicElementsinit();
}
function graphicElementsinit() {
    /*http://owlgraphic.com/owlcarousel/demos/manipulations.html*/
    //  owl carousel ---------

    if (typeof $("#owl-gallery").owlCarousel == 'function')
        $("#owl-gallery").owlCarousel({
            navigation: false,
            pagination: true,
            slideSpeed: 300,
            paginationSpeed: 400,
            singleItem: true,
            rewindSpeed: 0,
            autoPlay: false,
            lazyLoad: true,
            autoHeight: true
            //  navigationText: [
            //"<i class='fa fa-2x fa-chevron-left'><</i>",
            //"<i class='fa fa-2x fa-chevron-right'>></i>"
            // ]
        });
    //Custom Navigation Events
    $('.owl-arrows .next').click(function () {
        $("#owl-gallery").trigger('owl.next');
    })
    $('.owl-arrows .prev').click(function () {
        $("#owl-gallery").trigger('owl.prev');
    })
    //owl carousel ------------------------------------------

}
function graphicElementsreinit() {

    if ($('#owl-gallery').data('owlCarousel') != null)
        $('#owl-gallery').data('owlCarousel').reinit({
            navigation: true,
            pagination: false,
            slideSpeed: 300,
            paginationSpeed: 400,
            singleItem: true,
            rewindSpeed: 0,
            autoPlay: false
        });
    else
        sendmessage('error with carousel');

    //$("#owl-gallery").trigger('refresh.owl.carousel', { 
    //});
    //$("#owl-gallery").owlCarousel({
    //    items: 1,
    //    nav: true,
    //    smartSpeed: 400

    //});

}


/*//  OLD CLIENT METHODS (NON USATI) ////////////////////////////////////////////////////////////////////////////////////*/
function CaricaListaAllegatiPrimari(lng, callback) {
    var lng = lng || "I";
    var dataret = "";
    var filejson = "allegatiprimary" + lng + ".json"
    $.ajax({
        url: pathAbs + percorsoexp + filejson,
        //contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        //async: false,
        dataType: "text",
        success: function (data) {
            //dataret = "{ \"data\":" + data;
            //dataret += "}";
            dataret = data;
            callback(dataret);
        },
        error: function (result) {
            sendmessage('fail load primary images real estates');
            callback('');
        }
    });
    //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/NomeFile
    //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/Descrizione
    // percorsocompletoimg = percorsoapp + percorsoimg + "/" + guid_link + "/" + nomefile

}
function CaricaListaCompletaAllegati(lng, callback) {
    var lng = lng || "I";
    var dataret = "";
    var filejson = "allegati" + lng + ".json"
    $.ajax({
        url: pathAbs + percorsoexp + filejson,
        //contentType: "application/json; charset=utf-8",
        global: false,
        //async: false,
        cache: false,
        dataType: "text",
        success: function (data) {
            //dataret = "{ \"data\":" + data;
            //dataret += "}";
            dataret = data;
            callback(dataret);
        },
        error: function (result) {
            sendmessage('fail load images real estates');
            callback('');

        }
    });
    //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/NomeFile
    //*[guid_link='adc706a7-552d-4643-9f85-0a278567429e']/Descrizione
    // percorsocompletoimg = percorsoapp + percorsoimg + "/" + guid_link + "/" + nomefile

}

function caricaImmobilidaFile(callback, functiontocallonend) {
    $.ajax({
        url: pathAbs + percorsoexp + 'estates.json',
        //contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        success: function (data) {
            ;
            var dataret = "{ \"resource\":" + data;
            dataret += "}";
            //console.log(datamod);
            callback(dataret, functiontocallonend);
        },
        error: function (result) {
            sendmessage('fail load real estates');
            callback('', function () {
            });
        }
    });
}


function FiltraListaImmobiliClientside(objfiltro, callfunctiononend) {

    caricaImmobilidaFile(
       function (result, callafterfilter) {
           tmpLocalObjects["resourcesloaded"] = result;
           var resourcesloaded = tmpLocalObjects["resourcesloaded"];

           var codn = objfiltro["ddlNazioneSearch"];
           var codr = objfiltro["ddlRegioneSearch"];
           var codp = objfiltro["ddlProvinciaSearch"];
           var codc = objfiltro["ddlComuneSearch"];
           var idcontratto = objfiltro["ddlContrattoSearch"];
           var idcondizione = objfiltro["ddlCondizioneSearch"];
           var idtipologia = objfiltro["ddlTipologiaSearch"];
           var idprezzi = objfiltro["ddlPrezziSearch"];
           var idmetrature = objfiltro["ddlMetratureSearch"];
           var vetrina = objfiltro["vetrina"];


           /*Estraiamo i parametri per metrature e pressi se prenseti*/
           var pmin = 0; var pmax = 0;
           if (idprezzi != null && idprezzi != '') {

               var strprezzi = "{ \"resource\":" + JSON.stringify(JSONrefprezzi);
               strprezzi += "}";


               var selectedprezzo = JSON.search(JSON.parse(strprezzi), '//resource[id=' + idprezzi + ']//*[lingua="I"]/descrizione');
               if (selectedprezzo != null && selectedprezzo.length > 0) {
                   var pfascia = selectedprezzo[0].split(';');
                   if (pfascia != null && pfascia.length == 2) {
                       pmin = pfascia[0];
                       pmax = pfascia[1];
                   }
               }

           }
           var mmin = 0; var mmax = 0;
           if (idmetrature != null && idmetrature != '') {
               var strmetrature = "{ \"resource\":" + JSON.stringify(JSONrefmetrature);
               strmetrature += "}";

               var selectedmetratura = JSON.search(JSON.parse(strmetrature), '//resource[id=' + idmetrature + ']//*[lingua="I"]/descrizione');
               if (selectedmetratura != null && selectedmetratura.length > 0) {
                   var pmetratura = selectedmetratura[0].split(';');
                   if (pmetratura != null && pmetratura.length == 2) {
                       mmin = pmetratura[0];
                       mmax = pmetratura[1];
                   }
               }
           }

           //XPATH FILTER WITH DEFIANCEJS SUI dti immobiliari resourcesloaded
           var objresourcesloaded = JSON.parse(resourcesloaded);
           var tmpfilter = JSON.search(objresourcesloaded, '//resource[pubblicasito="true"]');
           objresourcesloaded["resource"] = tmpfilter;
           //resourcesloaded = "{ \"resource\":" + JSON.stringify(tmpfilter);
           //resourcesloaded += "}";

           /*NAZIONE*/
           if (codn != null && codn != '') {

               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[codiceNAZIONE="' + codn + '"]');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }
           /*REGIONE*/
           if (codr != null && codr != '') {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[codiceREGIONE="' + codr + '"]');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }
           /*PROVINCIA*/
           if (codp != null && codp != '') {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[codicePROVINCIA="' + codp + '"]');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }
           /*COMUNE*/
           if (codc != null && codc != '') {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[codiceCOMUNE="' + codc + '"]');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }

           /*Contratto*/
           if (idcontratto != null && idcontratto != '') {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[idcontratto="' + idcontratto + '"]');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }

           /*Tipologia*/
           if (idtipologia != null && idtipologia != '') {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[idtipologia="' + idtipologia + '"]');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }

           /*Condizione*/
           if (idcondizione != null && idcondizione != '') {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[idcondizione="' + idcondizione + '"]');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }

           //nascondiprezzo -> devo visualizzare riservato nel prezzo che visualizzo
           if (pmin != 0 && pmax != 0) {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[Prezzo1>=' + pmin + ' and  Prezzo1<=' + pmax + ']');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }

           if (mmin != 0 && mmax != 0) {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[Superficie1>=' + mmin + ' and  Superficie1<=' + mmax + ']');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }

           if (vetrina != null && (vetrina.toLowerCase() == 'true' || vetrina == true)) {
               if (objresourcesloaded["resource"] != null && objresourcesloaded["resource"].length > 0) {
                   tmpfilter = JSON.search(objresourcesloaded, '//resource[vetrina="true"]');
                   objresourcesloaded["resource"] = tmpfilter;
               }
           }

           /*DOVRESTI ORDINARE resourcesloaded per datainserimento decrescente*/
           //sortBy(objresourcesloaded["resource"], {
           //    prop: "datainserimento",
           //    desc: false
           //});

           resourcesloaded = JSON.stringify(objresourcesloaded);
           /*Metodo con callback che attende per andare avanti*/
           //selectDataPage(resourcesloaded, 2, 3, function (result) {
           //    resourcesloaded = result;
           //});

           /*metodo asincrono con promise che prosegue il flusso e solo al termine viene chiamata la promise.then*/
           if (resourcesloaded != null && resourcesloaded.length > 0) {
               //var dataobjparsed = JSON.parse(resourcesloaded);
               totalrecords = objresourcesloaded["resource"].length;
               tmpLocalObjects["resourcesloaded"] = resourcesloaded;

               callafterfilter();
           }
       }, callfunctiononend);
}
function renderListClientpaged() {
    /*SELEZIONO E VISUALIZZO LA PAGINA DEI DATI ATTENDENDO CON UN PROMISE IL RITORNO DEI DATI PAGINATI*/
    var promise = selectDataPage(tmpLocalObjects["resourcesloaded"], page, pagesize); //Metodo che chiama la funzione con la promise-> i risutati tornano nel .then quando pronti
    promise.then(function (result) {
        //console.log('promisereturn');
        $('#divSearchBar').modal('hide');
        renderPager(function (msgpager) {
            //Renderizziamo l'html dei dati nella pagina
            if (result != null && result.length != 0) {
                BindList("ullist1", JSON.parse(result)["resource"]);
            }
        });
    });
}
function selectDataPage(data, page, pagesize) {
    var deferred = $.Deferred();
    //PER IL PAGINATORE LATO CLIENT----------------------------------------------
    var dataret = "";
    var page = page || 1;
    var pagesize = pagesize || 20;
    var start = ((page - 1) * pagesize) + 1;
    var end = start + pagesize - 1;
    var objdatamod = JSON.parse(data);
    if (objdatamod["resource"] != null && objdatamod["resource"].length > 0) {
        var tmpfilter = JSON.search(objdatamod, '//resource[position()>=' + start + ' and  position()<=' + end + ']');
        dataret = "{ \"resource\" :" + JSON.stringify(tmpfilter);
        dataret += "}";
        //setTimeout(function () {
        //    deferred.resolve(dataret);
        //}, 6000);
        deferred.resolve(dataret);//Questa tornai risultati una volta che questi sono pronti
    } else
        deferred.resolve(dataret);//Questa tornai risultati una volta che questi sono pronti
    //----------------------------------------------------------------
    return deferred.promise(); //Questa torna subito la promise dei risulati
}
/*//FINE OLD CLIENT METHODS (NON USATI) ////////////////////////////////////////////////////////////////////////////////////*/




