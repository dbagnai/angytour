"use strict";

//Dishiaro più istanze della closure per utilizzo contemporaneo
var commenttool = new commentclosure('commenttool');
var commenttool1 = new commentclosure('commenttool1'); //Per utilizzo multiplo in pagina
//var commenttool2 = new commentclosure('commenttool2'); //Per utilizzo multiplo in pagina

//DEFINZIONE DELLA CLOSURE PER I COMMENTI!
function commentclosure(varname) {

    this.varname = varname; //Nome della variabile che istanzia lo scope, ad uso interno !!

    this.commentsvisible = true; //Blocca la visualizzazione dei riepiloghi e della lista commenti
    this.onlytotals = false; //Visualizzo solo i totali 
    this.viewmode = 0; //Imposta la modalità di visualizzazione 0 - default completo di form inserimento 1 - scroller commenti ultimi 2- lista commenti senza form inserimento
    this.mailadvice = true; //abilitazioneinvio mail di avviso inserimento di feedback per il gestore
    this.templatehtml = 'feedbacklist.html'; //default template per visualizzazione dei commenti
    this.templatehtmlinsert = 'feedbackinsert1.html'; //default template per visualizzazione dei commenti
    this.noinsertform = false;
    this.insertformup = false;

    var mainscope = this;
    this.message = '';
    this.localcontainer = {};
    this.objfiltro = {};

    this.idpost = '';
    this.idcontainer = '';
    this.enablepager = false;
    this.nomecliente = "";
    this.cognomecliente = "";
    this.emailcliente = "";
    //this.idmailschedule = 0; //id newsletter per invio invito al feedback
    //this.mailschedule = false; //abilitazione mail per inserimento in scheduler invio
    //this.deltadays = 30;//intervallo dopo il quale inviare la mail al cliente per richiesta feedback


    //this.rendercommentsloadref = function (idpost, idcontainer) { return rendercommentsloadref; }; //Mappare fuori una funzione interna
    //function rendercommentsloadref(idpost, idcontainer) {
    //    loadref(rendercomments, idpost, idcontainer, lng);
    //}
    this.rendercommentsloadref = function (idpost, idcontainer, templatecustom, enablepager, page, pagesize, maxrecord, onlytotals, viewmode, noinsertform, insertformup, templatehtmlinsert) {

        if (templatehtmlinsert != '' && templatehtmlinsert != null)
            mainscope.templatehtmlinsert = templatehtmlinsert;

        if (templatecustom != '' && templatecustom != null)
            mainscope.templatehtml = templatecustom;
        if (noinsertform != '' && noinsertform != null)
            mainscope.noinsertform = noinsertform;
        if (insertformup != '' && insertformup != null)
            mainscope.insertformup = insertformup;

        if (onlytotals != '' && onlytotals != null)
            mainscope.onlytotals = onlytotals;

        if (viewmode != '' && viewmode != null)
            mainscope.viewmode = viewmode;
        //else
        //    mainscope.viewmode = 0;

        if (enablepager != '' && enablepager != null && enablepager == 'true') {
            {
                mainscope.enablepager = true;
                mainscope.objfiltro['enablepager'] = enablepager;
            }
            if (!isNaN(Number(page)))
                mainscope.objfiltro['page'] = Number(page);
            else mainscope.objfiltro['page'] = 1;
            if (!isNaN(Number(pagesize)))
                mainscope.objfiltro['pagesize'] = Number(pagesize);
            else mainscope.objfiltro['pagesize'] = 12;
        }
        if (maxrecord != '' && maxrecord != null) {
            {
                mainscope.enablepager = false;
                mainscope.objfiltro['enablepager'] = false;
                mainscope.objfiltro['maxrecord'] = maxrecord;
            }
        }
        //loadref(mainscope.varname + '.rendercomments', idpost, idcontainer, lng); //Faccio la chiamata inserendola nella coda di controllo per caricamento dati di riferimento!
        (function wait() {
            if (typeof baseresources !== 'undefined' && baseresources != null && baseresources != '') {
                loadref(mainscope.varname + '.rendercomments', idpost, idcontainer, lng); //Faccio la chiamata inserendola nella coda di controllo per caricamento dati di riferimento!
            } else {
                setTimeout(wait, 300);
            }
        })();

    };
    this.rendercomments = function (idpost, idcontainer) {

        if (idpost == null || idpost == undefined || idpost.length == 0) {
            // mainscope.commentsvisible = false; //Posso Bloccare la visualizzazzione dei commenti 
            var idposttmp = $.getQueryString("idpost");
            if (idposttmp != null && idposttmp != undefined && idposttmp.length != 0) {
                idpost = idposttmp;
            }
            else
                idpost = 0;
        }
        var idcliente = $.getQueryString("idcliente"); //Se in querystring c'è un cliente lo carico per la preselezione nei campi di inserimento
        if (idcliente != null && idcliente != undefined && idcliente.length != 0) {
            //Caricare nome cognome email cliente e inseririli nel form preinserimento
            getcliente(idcliente, function (ret) {
                if (ret != null && ret.length > 0) { mainscope.emailcliente = ret[0]["email"]; mainscope.nomecliente = ret[0]["nome"]; mainscope.cognomecliente = ret[0]["cognome"]; }
            });
        }

        mainscope.objfiltro['id'] = idpost;
        mainscope.idpost = idpost;
        mainscope.idcontainer = idcontainer;

        var tmpfilter = "";
        if (username != null && username != '' && mainscope.viewmode == 0) {
            mainscope.templatehtml = 'feedbacklist-admin.html';
            mainscope.objfiltro['logged'] = true;
            tmpfilter = mainscope.objfiltro;
        }
        if (mainscope.commentsvisible) tmpfilter = mainscope.objfiltro;

        caricacommentsbyidpost(lng, tmpfilter, function (ret, mainscope) {
            if (mainscope.onlytotals) {
                if (mainscope.localcontainer.totaleapprovati > 0) {

                    $("#" + mainscope.idcontainer).prepend("<span class='feedTotals'  id='" + mainscope.idcontainer + "-totals'></span>");
                    $("#" + mainscope.idcontainer + "-totals").prepend(" (" + mainscope.localcontainer.totaleapprovati + ") <span class=\"rating\" disabled data-default-rating=" + mainscope.localcontainer.totalemediastars + "></span><br/><br/>");
                    $("#" + mainscope.idcontainer + "-totals").prepend(GetResourcesValue('feedbacks1'));
                    $("#" + mainscope.idcontainer + "-totals").prepend(GetResourcesValue('feedbackstitle1'));
                    inizializzastars();
                }
            }
            else
                ShowList(mainscope.templatehtml, mainscope.idcontainer, mainscope.idcontainer + '-ctrl', mainscope.localcontainer.list,
                    function () {
                        switch (mainscope.viewmode) {
                            case 1:     //Visualizzazione tipo scroller
                                //inizializzaiomo lo scroller
                                inizializzastars();
                                //Inizializzo lo scroller
                                jQuery(document).ready(function () {
                                    var owl = jQuery("#" + mainscope.idcontainer + '-ctrl');
                                    owl.owlCarousel({
                                        items: [1],
                                        autoPlay: 5000,
                                        itemsDesktop: [1199, 1], // i/tems between 1000px and 601px
                                        itemsTablet: [979, 1], // items between 600 and 0;
                                        itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
                                        slideSpeed: 1000,
                                        afterInit: lazyLoad,
                                        afterMove: lazyLoad
                                    });
                                    // Custom Navigation Events
                                    jQuery("#" + mainscope.idcontainer + '-ctrl' + "next").click(function () {
                                        owl.trigger('owl.next');
                                    });
                                    jQuery("#" + mainscope.idcontainer + '-ctrl' + "prev").click(function () {
                                        owl.trigger('owl.prev');
                                    });
                                });
                                break;
                            //case 2: //Visualizzazione tipo lista senza form inserimento
                            //    var aperto = $('#' + mainscope.idcontainer + '-ctrl' + 'collapseOne').hasClass('show');

                            //    //////////////////////////////////////////////////////////////////////
                            //    //TOTALI Visualizziamo il riepilogo e la media totale delle recenzioni
                            //    //////////////////////////////////////////////////////////////////////
                            //    if ((mainscope.commentsvisible) || (username != null && username != '')) {
                            //        $("#" + mainscope.idcontainer + '-ctrl').prepend(" (" + mainscope.localcontainer.totaleapprovati + ") <span class=\"rating\" disabled data-default-rating=" + mainscope.localcontainer.totalemediastars + "></span><br/><br/>");
                            //        $("#" + mainscope.idcontainer + '-ctrl').prepend(GetResourcesValue('feedbacks'));
                            //        //  $("#" + mainscope.idcontainer+ '-ctrl').prepend(GetResourcesValue('feedbackstitle'));
                            //        inizializzastars();
                            //    }
                            //    /////////////////////////////////////////////////////////////////

                            //    ///////////////////////////
                            //    //GESTIONE PAGINAZIONE/////
                            //    ///////////////////////////
                            //    if (mainscope.enablepager && $("#" + mainscope.idcontainer + '-ctrl').length > 0) {
                            //        $("#" + mainscope.idcontainer + '-ctrl').append("<li><div  id='" + mainscope.idcontainer + "-pager'></div></li>");
                            //        inizializzapager(mainscope.idcontainer + '-pager');
                            //    }
                            //    ////////////////////////////////////
                            //    inizializzastars();
                            //    $('#' + mainscope.idcontainer + '-ctrl' + 'collapseOne').addClass('show');//Apro l'accordion per regolare le altezze delle textarea 
                            //    $('textarea').autoHeight();
                            //    if (!aperto)
                            //        $('#' + mainscope.idcontainer + '-ctrl' + 'collapseOne').removeClass('show');//Chiudo l'accordion dopo aver ricalcolato le altezze se necessario
                            //    ///////////////////////////////////

                            //    break;

                            default:
                                /////////////////////////////////////////////////////////////////
                                //TOTALI Visualizziamo il riepilogo e la media totale delle recenzioni
                                /////////////////////////////////////////////////////////////////
                                if ((mainscope.commentsvisible) || (username != null && username != '')) {
                                    $("#" + mainscope.idcontainer).prepend("<div class='feedTotals container pt-1'  id='" + mainscope.idcontainer + "-totals'></div>");
                                    $("#" + mainscope.idcontainer + "-totals").prepend(" (" + mainscope.localcontainer.totaleapprovati + ") <span class=\"rating\" disabled data-default-rating=" + mainscope.localcontainer.totalemediastars + "></span><br/><br/>");
                                    $("#" + mainscope.idcontainer + "-totals").prepend(GetResourcesValue('feedbacks'));
                                    $("#" + mainscope.idcontainer + "-totals").prepend(GetResourcesValue('feedbackstitle'));
                                    inizializzastars();
                                }
                                /////////////////////////////////////////////////////////////////

                                ///////////////////////////
                                //GESTIONE PAGINAZIONE//
                                ///////////////////////////
                                if (mainscope.enablepager && $("#" + mainscope.idcontainer + '-ctrl').length > 0) {
                                    $("#" + mainscope.idcontainer + '-ctrl').append("<li><div  id='" + mainscope.idcontainer + "-pager'></div></li>");
                                    inizializzapager(mainscope.idcontainer + '-pager');
                                }
                                ///////////////////////////

                                /////////////////////////////////////////////////////////////////
                                //Inseriamo il tool per permettere l'inserimento di un comment
                                /////////////////////////////////////////////////////////////////
                                if (!mainscope.noinsertform) {
                                    if (mainscope.insertformup)
                                        $("#" + mainscope.idcontainer).prepend("<div  id='" + mainscope.idcontainer + "-head'></div>");
                                    else
                                        $("#" + mainscope.idcontainer).append("<div  id='" + mainscope.idcontainer + "-head'></div>");

                                    var dummyarray = [];
                                    //Precompliamo i campi necessari per il form di inserimento
                                    var testonome = mainscope.nomecliente + ' ' + mainscope.cognomecliente;
                                    if (mainscope.localcontainer.item.hasOwnProperty('Nome'))
                                        mainscope.localcontainer.item['Nome'] = testonome.trim();
                                    if (mainscope.localcontainer.item.hasOwnProperty('Email'))
                                        mainscope.localcontainer.item['Email'] = mainscope.emailcliente;
                                    dummyarray.push(mainscope.localcontainer.item);

                                    var aperto1 = $('#' + mainscope.idcontainer + '-ctrl' + 'collapseOne').hasClass('show');

                                    if (mainscope.localcontainer.item != '')
                                        ShowList(mainscope.templatehtmlinsert, mainscope.idcontainer + '-head', mainscope.idcontainer + '-head-ctrl', dummyarray,
                                            function () {
                                                $('#' + mainscope.idcontainer + '-head-ctrl' + 'tresponse').html(mainscope.message);
                                                //window.scroll(0, document.querySelector("#' + mainscope.idcontainer + 'tresponse").offsetTop - 0);
                                                if (mainscope.message.length > 0)
                                                    $('html, body').animate({ scrollTop: $("#" + mainscope.idcontainer + '-head-ctrl' + "tresponse").offset().top - 150 }, 100);
                                                mainscope.message = "";

                                                inizializzastars();
                                                $('#' + mainscope.idcontainer + '-ctrl' + 'collapseOne').addClass('show');//Apro l'accordion per regolare le altezze delle textarea 
                                                $('textarea').autoHeight();
                                                if (!aperto1)
                                                    $('#' + mainscope.idcontainer + '-ctrl' + 'collapseOne').removeClass('show');//Chiudo l'accordion dopo aver ricalcolato le altezze se necessario
                                            });
                                } else {
                                    inizializzastars();
                                    $('#' + mainscope.idcontainer + '-ctrl' + 'collapseOne').addClass('show');//Apro l'accordion per regolare le altezze delle textarea 
                                    $('textarea').autoHeight();
                                    if (!aperto1)
                                        $('#' + mainscope.idcontainer + '-ctrl' + 'collapseOne').removeClass('show');//Chiudo l'accordion dopo aver ricalcolato le altezze se necessario
                                }
                                /////////////////////////////////////////////////////////////////
                                break;
                        }
                    });
        });
        return;
    };
    function inizializzapager(idpager) {
        var recordstotali = mainscope.localcontainer.recordstotali;
        var page = mainscope.objfiltro['page'];
        var pagesize = mainscope.objfiltro['pagesize'];
        page = Number(page);
        pagesize = Number(pagesize);

        if (page > 1)
            $("#" + idpager).append("<button  type='button' class='btn' id='" + idpager + "-prev'>" + GetResourcesValue("pagerindietro") + "</button>");
        if (!isNaN(page) && !isNaN(pagesize))
            if (!(((page + 1) * pagesize) - recordstotali >= pagesize))
                $("#" + idpager).append("<button type='button' class='btn' id='" + idpager + "-next'>" + GetResourcesValue("pageravanti") + "</button>");

        if ($("#" + idpager + '-next').length)
            $("#" + idpager + '-next')[0].addEventListener('click', function (elem) {
                console.log('avanti');
                var recordstotali = mainscope.localcontainer.recordstotali;
                var page = mainscope.objfiltro['page'];
                var pagesize = mainscope.objfiltro['pagesize'];
                page = Number(page);
                pagesize = Number(pagesize);
                if (!isNaN(page) && !isNaN(pagesize)) {
                    page += 1;
                    if ((page * pagesize) - recordstotali >= pagesize) { page -= 1; }
                    else {
                        mainscope.objfiltro['page'] = page;
                        mainscope.rendercomments(mainscope.idpost, mainscope.idcontainer);
                    }
                }
            });
        if ($("#" + idpager + '-prev').length)
            $("#" + idpager + '-prev')[0].addEventListener('click', function (elem) {
                console.log('indietro');
                var recordstotali = mainscope.localcontainer.recordstotali;
                var page = mainscope.objfiltro['page'];
                var pagesize = mainscope.objfiltro['pagesize'];
                page = Number(page);
                pagesize = Number(pagesize);
                if (!isNaN(page) && !isNaN(pagesize)) {
                    page -= 1;
                    if (page < 1) { page = 1; }
                    else {
                        mainscope.objfiltro['page'] = page;
                        mainscope.rendercomments(mainscope.idpost, mainscope.idcontainer);
                    }
                }
            });
    }

    function inizializzastars() {
        var ratings = document.getElementsByClassName('rating');
        for (var i = 0; i < ratings.length; i++) {
            var r = new SimpleStarRating(ratings[i]); //Inizializza la visualizzazione delle stelline

            //evento per cattura i click di rating dagli utenti sugli elementi star!!!! 
            ratings[i].addEventListener('rate', function (elem) {
                console.log('Rating: ' + elem.detail);
                var found = false;
                var tmpId = $(elem.target).attr('idbind');
                var valore = elem.detail;
                var property = $(elem.target).attr('mybind');
                if (tmpId != null && tmpId != '') //Aggiornamento vecchio
                    for (var j = 0; j < mainscope.localcontainer.list.length; j++) {
                        if (mainscope.localcontainer.list[j].hasOwnProperty(property)) {
                            if (mainscope.localcontainer.list[j].Id == tmpId) {
                                found = true;
                                mainscope.localcontainer.list[j][property] = valore;
                                console.log(mainscope.localcontainer.list[j][property]);
                            }
                        }
                    }
                else { //Nuovo valore
                    if (mainscope.localcontainer.item.hasOwnProperty(property)) {
                        found = true;
                        if (!isNaN(Number(mainscope.idpost)))
                            mainscope.localcontainer.item['Idpost'] = Number(mainscope.idpost);
                        else
                            mainscope.localcontainer.item['Idpost'] = 0;
                        mainscope.localcontainer.item[property] = valore;
                        console.log(mainscope.localcontainer.item[property]);
                    }
                }
            });
        }
    }

    this.initautocompleteclienti = function (idcontrollo) {
        $("#" + idcontrollo).autocomplete({
            source: pathAbs + commonhandlerpath + '?q=autocompleteclienti&r=20',
            minLength: 0,
            //appendTo: '#' + idcontrollo,
            open: function () {
                //console.log("OPEN");
                setTimeout(function () {
                    $('.ui-autocomplete').css('z-index', 99999999999999);
                }, 0);
            },
            //open: function (event, ui) {
            //    $('.ui-autocomplete').css('position', 'absolute');
            //    $('.ui-autocomplete').css('z-index', 99999999999999);
            //    $('.ui-autocomplete').css('top', '0px');
            //    $('.ui-autocomplete').css('left', '0px');
            //    setTimeout(function () {
            //        $('.ui-autocomplete').css('z-index', 99999999999999);
            //    }, 1);
            //},
            select: function (event, ui) {
                if (ui.item != null) {
                    $("#" + idcontrollo + "Hidden").val(ui.item.id);
                    $("#" + idcontrollo).text(ui.item.label);
                }
            }
        }).on("focus", function () {
            $(this).autocomplete("search", "");
        });

    }

    function getcliente(id, callback) {
        var lng = lng || "I";
        $.ajax({
            url: pathAbs + commonhandlerpath,
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'autocompleteclienti', 'term': id },
            success: function (result) {
                var parseditem = '';
                if (result.length > 0)
                    parseditem = JSON.parse(result);
                callback(parseditem);
            },
            error: function (result) {
                callback('');
            },
            falilure: function (result) {
                callback('');
            }
        });
    }


    /* Al change del tag vado ad aggiornare la memoria generale per il successivo salvataggio
     */
    this.onchangeValue = function (elem) {
        //console.log(elem);
        var found = false;
        var tmpId = $(elem).attr('idbind');
        var property = $(elem).attr('mybind');

        if ($(elem).attr('type') == 'checkbox') {
            for (var j = 0; j < mainscope.localcontainer.list.length; j++) {
                if (mainscope.localcontainer.list[j].hasOwnProperty(property)) {
                    if (mainscope.localcontainer.list[j].Id == tmpId) {
                        found = true;
                        mainscope.localcontainer.list[j][property] = $(elem).is(":checked");
                        console.log(mainscope.localcontainer.list[j][property]);
                    }
                }
            }
        }
        else {
            var newval = $(elem).val();
            for (var j = 0; j < mainscope.localcontainer.list.length; j++) {
                if (mainscope.localcontainer.list[j].hasOwnProperty(property)) {
                    if (mainscope.localcontainer.list[j].Id == tmpId) {
                        found = true;
                        mainscope.localcontainer.list[j][property] = newval;
                        mainscope.localcontainer.list[j][property + lng] = newval;
                        console.log(mainscope.localcontainer.list[j][property]);
                    }
                }
            }
        }

    };
    this.onchangenewValue = function (elem) {
        //console.log(elem);
        var found = false;
        var tmpId = 0;
        var newval = $(elem).val();
        var property = $(elem).attr('mybind');
        if (mainscope.localcontainer.item.hasOwnProperty(property)) {
            found = true;
            if (!isNaN(Number(mainscope.idpost)))
                mainscope.localcontainer.item['Idpost'] = Number(mainscope.idpost);
            else
                mainscope.localcontainer.item['Idpost'] = 0;

            mainscope.localcontainer.item[property] = newval;
            mainscope.localcontainer.item[property + lng] = newval;
            console.log(mainscope.localcontainer.item[property]);
        }
    };


    this.selectitem = function (elem) {
        // console.log(elem);
        //var found = false;
        //var property = $(elem).attr('mybind');
        //for (var j = 0; j < mainscope.localcontainer.list.length; j++) {
        //    if (mainscope.localcontainer.list[j].hasOwnProperty(property)) {
        //        if (mainscope.localcontainer.list[j].Id == tmpId) {
        //            found = true;
        //            mainscope.localcontainer.list[j][property] = newval;
        //            console.log(mainscope.localcontainer.list[j][property]);
        //        }
        //    }
        //}
    };
    this.insertitem = function (elem) {
        //console.log(elem);
        $(elem).attr("disabled", true);
        var found = false;
        var tmpId = 0;
        if (mainscope.localcontainer.item != null) {
            insertcomments(lng, mainscope.localcontainer.item, function (ret) {
                console.log(ret);
                if (ret == '' || ret == null) {
                    mainscope.message = 'Recensione inserita in attesa di approvazione';
                    if (mainscope.mailadvice) {
                        mainscope.localcontainer.mail.Emailaddress.defaultsenderemail = mainscope.localcontainer.item['Email'];
                        mainscope.localcontainer.mail.Emailaddress.defaultsendername = mainscope.localcontainer.item['Nome'];
                        //Inserire i dati necessari per l'invio nella mail 
                        mainscope.localcontainer.mail.Sparedict["Idpost"] = mainscope.localcontainer.item['Idpost']; //Id prodotto se presente
                        mainscope.localcontainer.mail.Sparedict["Id"] = mainscope.localcontainer.item["Id"]; //id recensione inserita

                        inviamail(lng, mainscope.localcontainer.mail, function (retinvio) {

                        });
                    }
                }
                else {
                    mainscope.message = 'Errore: ' + ret;
                }

                // if (username != null && username != '')
                mainscope.rendercomments(mainscope.idpost, mainscope.idcontainer);
            });
        }
    };

    this.updateitem = function (id) {
        //console.log(elem);
        var found = false;
        var tmpId = id;
        //var property = $(elem).attr('mybind');
        for (var j = 0; j < mainscope.localcontainer.list.length; j++) {
            if (mainscope.localcontainer.list[j].hasOwnProperty('Id')) {
                if (mainscope.localcontainer.list[j].Id == tmpId) {
                    found = true;
                    console.log('update');
                    console.log(mainscope.localcontainer.list[j]);
                    updatecomments(lng, mainscope.localcontainer.list[j], function (ret) {
                        console.log(ret);
                        if (ret == '' || ret == null)
                            //$('#' + mainscope.idcontainer + 'tresponse').html('Recenzione inserita in attesa di approvazione');
                            mainscope.message = 'Recensione aggiornata';
                        else
                            //  $('#' + mainscope.idcontainer + 'tresponse').html('Errore: ' + ret);
                            mainscope.message = 'Errore: ' + ret;
                        mainscope.rendercomments(mainscope.idpost, mainscope.idcontainer);
                    });
                    // mainscope.localcontainer.list[j] //elemento da passare per l'aggiornamento
                }
            }
        }
    };

    this.deleteitem = function (id) {
        //console.log(elem);
        var found = false;
        var tmpId = id;
        //var property = $(elem).attr('mybind');
        for (var j = 0; j < mainscope.localcontainer.list.length; j++) {
            if (mainscope.localcontainer.list[j].hasOwnProperty('Id')) {
                if (mainscope.localcontainer.list[j].Id == tmpId) {
                    found = true;
                    console.log('delete');
                    console.log(mainscope.localcontainer.list[j])
                    // mainscope.localcontainer.list[j] //elemento da passare per l'aggiornamento
                    deletecomments(lng, mainscope.localcontainer.list[j], function (ret) {
                        console.log(ret);
                        if (ret == '' || ret == null)
                            //$('#tresponse').html('Recenzione inserita in attesa di approvazione');
                            mainscope.message = 'Recenzione cancellata';
                        else
                            //  $('#tresponse').html('Errore: ' + ret);
                            mainscope.message = 'Errore: ' + ret;
                        mainscope.rendercomments(mainscope.idpost, mainscope.idcontainer);
                    })
                }
            }
        }
    };

    function caricacommentsbyidpost(lng, objfiltro, callback) {
        var lng = lng || "I";
        var objfiltro = objfiltro || {};

        //Svuoto il contenitore dei commenti
        mainscope.localcontainer = {};
        mainscope.localcontainer["item"] = '';
        mainscope.localcontainer["list"] = '';
        mainscope.localcontainer["objfiltro"] = '';
        mainscope.localcontainer["totaleapprovati"] = '';
        mainscope.localcontainer["totalemediastars"] = '';
        mainscope.localcontainer["recordstotali"] = '';
        mainscope.localcontainer["mail"] = '';

        $.ajax({
            url: pathAbs + commenthandlerpath,
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'caricacommenti', 'lng': lng, 'objfiltro': JSON.stringify(objfiltro) },
            success: function (result) {
                var parseddata = '';

                if (result != '') {
                    var parsedres = JSON.parse(result);
                    mainscope.localcontainer["item"] = parsedres.item;//oggetto vuoto per update ed insert
                    mainscope.localcontainer["mail"] = parsedres.mail;//oggetto vuoto per update ed insert
                    mainscope.localcontainer["list"] = parsedres.list;//lista di ritorno
                    mainscope.localcontainer["totaleapprovati"] = parsedres.totaleapprovati;// 
                    mainscope.localcontainer["totalemediastars"] = parsedres.totalemediastars;// 
                    mainscope.localcontainer["recordstotali"] = parsedres.recordstotali;// 
                    mainscope.localcontainer["objfiltro"] = JSON.parse(result).objfiltro;//array oggetti json filtri usati nel caricamento dati
                }
                callback('', mainscope);
            },
            error: function (result) {
                //sendmessage('fail creating link');
                callback(result.responseText, mainscope);
            }
        });
    };

    function Validazionedati(item, callback) {

        if (item.stelle == 0) {
            callback('Dare una valutazione da 1 a 5'); return;
        }
        if (item["Nome"] == 0 || item["Nome"].length == 0) {
            callback('Inserire nome o nickname'); return;
        }
        if (item["Email"] == 0 || item["Email"].length == 0 || !validateEmail(item["Email"])) {
            callback('Inserire email'); return;
        }
        if (item["Titolo" + lng] == 0 || item["Titolo" + lng].length == 0) {
            callback('Inserire un titolo per la recenzione'); return;
        }
        if (item["Testo" + lng] == 0 || item["Testo" + lng].length == 0) {
            callback('Inserire un messaggio di testo per la recenzione'); return;
        }

        callback('');
    }

    function inviamail(lng, mail, callback) {
        var lng = lng || "I";
        var mail = mail || {};
        //Campi di default passati nel costruttore
        //mainscope.localcontainer.mail.Emailaddress.defaultdestemail
        //mainscope.localcontainer.mail.Emailaddress.defaultdestname
        //mainscope.localcontainer.mail.Emailaddress.defaultsenderemail
        //mainscope.localcontainer.mail.Emailaddress.defaultsendername
        $.ajax({
            url: pathAbs + commenthandlerpath,
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'inviamailfeedback', 'mail': JSON.stringify(mail), 'lng': lng },
            success: function (result) {
                callback('');

            },
            error: function (result) {

                callback(result.responseText);
            },
            falilure: function (result) {

                callback(result.responseText);
            }
        });
    }

    function updatecomments(lng, item, callback) {
        var lng = lng || "I";
        var item = item || {};

        Validazionedati(item,
            function (ret) {
                $('#' + mainscope.idcontainer + '-head-ctrl' + 'tresponse').html(ret);
                if (ret.length > 0) {
                    $('html, body').animate({ scrollTop: $("#" + mainscope.idcontainer + '-head-ctrl' + "tresponse").offset().top - 150 }, 100);
                    return;
                }


                $.ajax({
                    url: pathAbs + commenthandlerpath,
                    contentType: "application/json; charset=utf-8",
                    global: false,
                    cache: false,
                    dataType: "text",
                    type: "POST",
                    //async: false,
                    data: { 'q': 'updatecommenti', 'item': JSON.stringify(item), 'lng': lng },
                    success: function (result) {
                        callback('');
                    },
                    error: function (result) {
                        //sendmessage('fail creating link');
                        callback(result.responseText);
                    },
                    falilure: function (result) {
                        //sendmessage('fail creating link');
                        callback(result.responseText);
                    }
                });
            });
    }

    function insertcomments(lng, item, callback) {
        var lng = lng || "I";
        var item = item || {};

        Validazionedati(item,
            function (ret) {
                $('#' + mainscope.idcontainer + '-head-ctrl' + 'tresponse').html(ret);
                if (ret.length > 0) {
                    $('html, body').animate({ scrollTop: $("#" + mainscope.idcontainer + '-head-ctrl' + "tresponse").offset().top - 150 }, 100);
                    $('#' + mainscope.idcontainer + '-head-ctrlinsertbtn').removeAttr("disabled");
                    return;
                }


                $.ajax({
                    url: pathAbs + commenthandlerpath,
                    contentType: "application/json; charset=utf-8",
                    global: false,
                    cache: false,
                    dataType: "text",
                    type: "POST",
                    //async: false,
                    data: { 'q': 'insertcommenti', 'item': JSON.stringify(item), 'lng': lng },
                    success: function (result) {
                        //result contiene l'id del messaggio inserito
                        mainscope.localcontainer.item["Id"] = result;
                        callback('');
                    },
                    error: function (result) {
                        //sendmessage('fail creating link');
                        callback(result.responseText);
                    },
                    falilure: function (result) {
                        //sendmessage('fail creating link');
                        callback(result.responseText);
                    }
                });
            });

    }


    function deletecomments(lng, item, callback) {
        var lng = lng || "I";
        var item = item || {};

        $.ajax({
            url: pathAbs + commenthandlerpath,
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'deletecommenti', 'item': JSON.stringify(item), 'lng': lng },
            success: function (result) {
                callback('');
            },
            error: function (result) {
                //sendmessage('fail creating link');
                callback(result.responseText);
            },
            falilure: function (result) {
                //sendmessage('fail creating link');
                callback(result.responseText);
            }
        });
    }


};