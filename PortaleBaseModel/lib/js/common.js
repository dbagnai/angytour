
"use strict";

var jsonlanguages = "";
var versionforcache = "1";
var percorsocontenuti = "";
var percorsocomune = "";
var percorsoapp = "";
var percorsocdn = "";
var percorsoimg = "";
var percorsoexp = "";
var percorsolistaimmobili = "";
var usecdn = false;
var lng = "I";
var baseresources = '';
var JSONnazioni = "";
var JSONregioni = "";
var JSONprovince = "";
var JSONcomuni = "";
var JSONcar1 = "";
var JSONcar2 = "";
var JSONcar3 = "";
var JSONcategorie = "";
var JSONcategorie2liv = "";
var jsontipologie = "";
var percorsolistadati = "";
var pathAbs = "";
var username = "";
var JSONrefmetrature = "";
var JSONrefprezzi = "";
var JSONrefcondizione = "";
var JSONreftipocontratto = "";
var JSONreftiporisorse = "";
var JSONreftipocontatto = "";
var bookinghandlerpath = '/lib/hnd/HandlerBooking.ashx';
var commonhandlerpath = '/lib/hnd/HandlerDataCommon.ashx';
var resourcehandlerpath = '/lib/hnd/HandlerDataImmobili.ashx';
var carrellohandlerpath = '/lib/hnd/CarrelloHandler.ashx';

var referencesloaded = false;
var promisecalling = false;
var callqueque = [];
var globalObject = {};


var enablescrolltopmem = false;
/*Memorizzo posizione scrolltop in session ----------------------------------------*/
//history.scrollRestoration = 'manual';
//$(window).on('unload', function () {
//    $(window).scrollTop(0);
//}); 
$(window).scroll(function () {
    var pathName = document.location.pathname;
    var scrollPosition = $(window).scrollTop();
    if (enablescrolltopmem) {
        sessionStorage.setItem("scrollPosition_" + pathName, scrollPosition.toString());
        // console.log(scrollPosition);
    }
});
/*Recover scroll top pos from memory*/
var reinitscrollpos = function () {
    var pathName = document.location.pathname;
    if (sessionStorage["scrollPosition_" + pathName]) {
        $(window).scrollTop(sessionStorage.getItem("scrollPosition_" + pathName) || 0);
        console.log("restore pos to : " + sessionStorage.getItem("scrollPosition_" + pathName));
    }
};
/*Memorizzo posizione scrolltop in session ----------------------------------------*/


$(document).ready(function () {
    //   searchtaginjectandcall();
});
searchtaginjectandcall();

/*Seleziona i tag con classe inject e Chiama la funzione specificata nell'attributo params passando i parametri a seguire*/
function searchtaginjectandcall() {
    $(".inject").each(function () {
        var container = $(this);
        var callerpars = container.attr('params').split(',');
        var callerid = container.prop("id");
        var itemtocall = {};
        for (var i = 0; i < callerpars.length; i++) {
            //  console.log(callerpars[i]);
            if (i == 0) {
                itemtocall = {};
                itemtocall.args = [];
                itemtocall.name = callerpars[i].trim().replace(/\'/g, "");
            } else {
                if (callerpars[i].trim().toLowerCase() == ("false") || callerpars[i].trim().toLowerCase() == ("true"))
                    itemtocall.args.push((callerpars[i].trim() == 'true'));
                else if (!isNaN(Number(callerpars[i].trim())))
                    itemtocall.args.push(Number(callerpars[i].trim()));
                else
                    itemtocall.args.push(callerpars[i].trim().replace(/\'/g, ""));
            }
        }
        //window[itemtocall.name].apply(this, itemtocall.args)//make the call
        /*Chiamo la funzione controllando se definita, altrimenti attendo che lo sia e la richiamo*/
        (function wait() {
            if (typeof window[itemtocall.name] === "function") {
                window[itemtocall.name].apply(this, itemtocall.args)//make the call
            } else {
                setTimeout(wait, 50);
            }
        })();
    });
}


function initLingua(lingua) {
    lng = lingua || "I";
    moment.locale("it");
    var clearlocalmem = $.getQueryString("clear");
    if (clearlocalmem == 'true')
        manageclientstorage("clear");
}
function testCacheversion(serverversion) {
    if (versionforcache != serverversion) manageclientstorage("clear");
}
//QUESTA PERMETTE DI CHIAMARE LA FUNZIONE PASSATA NEI PARAMENTRI AL TERMINE DELLA FUNZIONE DI PROMISE QUI DEFINITA
//AGGIUNGENDO I PARAMETRI DELLA FUNZIONE PRESENT NELLA LISTA DELLA CHIAMATA
//Si chiama con loadref(nomefunzione,parametro1,parametro2, .... , lingua)
function loadref(functocall) {
    var lingua = 'I'; //QUesta la prendo sempre come ultimo argomento di chiamata

    //MEMORIZZO LA CHIAMATA
    var item = {};
    item.name = functocall.name;
    item.args = [];

    //MEMORIZZO GLI ARGOMENTI DI CHIAMATA IN ARGS
    var args = new Array();
    for (var i = 1; i < arguments.length; i++) {
        args.push(arguments[i]);
        lingua = arguments[i];
        item.args.push(arguments[i]);

    }

    if (promisecalling)
        callqueque.push(item); //metto nella coda delle chiamate durante l'esecuzione della promise iniziale per inizializzare le referenze base
    else {
        promisecalling = true;
        var promise = initreferencesdata(lingua);
        promise.then(function (result) {
            window[functocall.name].apply(this, args);
            //AL RITORNO DALLA PROMISE CHIAMO ANCHE TUTTE LE FUNZIONI NELLA CODA DIRETTAMENTE
            var tmpqueue = jQuery.extend(true, [], callqueque);
            callqueque = []; //SVUOTO LA CODA
            promisecalling = false;
            for (var i = 0; i < tmpqueue.length; i++) {
                var callitem = tmpqueue[i];
                //callqueque.splice(i, 1);
                //callqueque.remByName(callitem.name); //tolgo dall'array la chiamata
                //window[callitem.name].apply(this, callitem.args);//Questa chiama senza promise la funzione finale
                (function wait() {
                    if (typeof window[callitem.name] === "function") {
                        window[callitem.name].apply(this, callitem.args)//make the call
                    } else {
                        setTimeout(wait, 50);
                    }
                })();
            }
        });
    }
}

function loadvariables(result) {
    //PERCORSI APPLICAZIONE//////////////////////////////////////////////
    var jobj = JSON.parse(result);
    percorsoapp = jobj["percorsoapp"];
    percorsocdn = jobj["percorsocdn"];
    percorsoimg = jobj["percorsoimg"];
    percorsoexp = jobj["percorsoexp"];
    percorsolistaimmobili = jobj["percorsolistaimmobili"];

    versionforcache = jobj["versionforcache"];
    percorsocomune = jobj["percorsocomune"];
    percorsocontenuti = jobj["percorsocontenuti"];
    usecdn = jobj["usecdn"];
    //////////////////////// //Stringa json con le lingue
    jsonlanguages = jobj["jsonlanguages"];
    percorsolistadati = jobj["percorsolistadati"];
    ///////////////OGGETTO Json con le risorse
    baseresources = JSON.parse(jobj["baseresources"]);
    ///////////////OGGETTO Json con le regioni
    JSONregioni = JSON.parse(jobj["jsonregioni"]);
    ///////////OGGETTO Json con le province
    JSONprovince = JSON.parse(jobj["jsonprovince"]);
    JSONcategorie = JSON.parse(jobj["jsoncategorie"]);
    JSONcategorie2liv = JSON.parse(jobj["jsoncategorie2liv"]);
    jsontipologie = JSON.parse(jobj["jsontipologie"]);
    username = jobj["username"];

    ////////////////ALTRE VARIABILI DI RIFERIMENTO SPECIFICHE////////////////////////////////////////
    var dictresources = JSON.parse(jobj["dictreferences"]);
    ////console.log(baseresources);
    if (dictresources["JSONrefmetrature"] != null && dictresources["JSONrefmetrature"] != '')
        JSONrefmetrature = JSON.parse(dictresources["JSONrefmetrature"]);
    if (dictresources["JSONrefprezzi"] != null && dictresources["JSONrefprezzi"] != '')
        JSONrefprezzi = JSON.parse(dictresources["JSONrefprezzi"]);
    if (dictresources["JSONrefcondizione"] != null && dictresources["JSONrefcondizione"] != '')
        JSONrefcondizione = JSON.parse(dictresources["JSONrefcondizione"]);
    if (dictresources["JSONreftipocontratto"] != null && dictresources["JSONreftipocontratto"] != '')
        JSONreftipocontratto = JSON.parse(dictresources["JSONreftipocontratto"]);
    if (dictresources["JSONreftiporisorse"] != null && dictresources["JSONreftiporisorse"] != '')
        JSONreftiporisorse = JSON.parse(dictresources["JSONreftiporisorse"]);
    //JSONgeogenerale = JSON.parse(dictresources["JSONgeogenerale"]);
    //JSONcar1 = JSON.parse(dictresources["JSONclasse"]);
    if (dictresources["JSONcar1"] != null && dictresources["JSONcar1"] != '')
        JSONcar1 = JSON.parse((dictresources["JSONcar1"]));
    if (dictresources["JSONcar2"] != null && dictresources["JSONcar2"] != '')
        JSONcar2 = JSON.parse((dictresources["JSONcar2"]));
    if (dictresources["JSONcar3"] != null && dictresources["JSONcar3"] != '')
        JSONcar3 = JSON.parse((dictresources["JSONcar3"]));
    ////////////////ALTRE VARIABILI DI RIFERIMENTO SPECIFICHE////////////////////////////////////////

    referencesloaded = true;
}

/*FUNZIONE PER CARICARE I DATI REFERENZA UNICA (VERSIONE localFORAGE con client memory)*/
function initreferencesdata(lingua) {
    lng = lingua || "I";
    var deferred = $.Deferred();
    //QUI PUOI TESTARE CHE SE LE VARIABILI SONO NGIA CARICHE NON RICARICHI
    if (!referencesloaded) {
        var promiseclientstorage = getfromclientstorage("referencesvars" + lng);
        promiseclientstorage.then(function (result) {
            var datainmemory = result;
            if (datainmemory == '') {
                $.ajax({
                    url: pathAbs + commonhandlerpath,
                    contentType: "application/json; charset=utf-8",
                    //async: false,
                    cache: false,
                    data: { 'q': 'initreferencesdata', 'lng': lng },
                    success: function (result) {
                        try {

                            manageclientstorage("put", "referencesvars" + lng, result, 1); //Metto i valori di ritorno in localstorage con 1 ora scadenza
                            console.log('loadvars from server');
                            loadvariables(result);
                        }
                        catch (e) {
                            sendmessage('fail init initreferencesdata', e);
                        }
                        deferred.resolve('');
                    },
                    failure: function (result) {
                        sendmessage('fail init initreferencesdata', '');
                        deferred.resolve('');
                    }
                });
            }
            else {
                console.log('loadvars from memory cache');
                loadvariables(datainmemory); //Prendo i valori di init dal localstorage
                deferred.resolve('');
            }

        });
    } else { deferred.resolve(''); }
    return deferred.promise();
}


function getfromclientstorage(key) {
    var deferredclientstorage = $.Deferred();
    var expired = false;
    localforage.getItem('datestored').then(function (value) {

        var expiretime = value;
        if (moment(new Date()) > moment(expiretime, "DD/MM/YYYY HH:mm:ss")) {
            expired = true;
            localforage.removeItem('datestored').then(function () {
                localforage.removeItem(key).then(function () {
                    // Run this code once the key has been removed.
                    console.log('expired localforage values!');
                    deferredclientstorage.resolve('');

                }).catch(function (err) {
                    // This code runs if there were any errors
                    console.log('localforage error clearing values');
                    console.log(err);
                    deferredclientstorage.resolve('');

                });
            }).catch(function (err) {
                // This code runs if there were any errors
                console.log('localforage error clearing data');
                console.log(err);
                deferredclientstorage.resolve('');

            });
        } else {
            localforage.getItem(key).then(function (value) {
                if (value != null && value != '')
                    value = pako.inflate(value, { to: 'string' }); //Descompress -> and return!!!!
                else value = '';
                deferredclientstorage.resolve(value);

            }).catch(function (err) {
                // This code runs if there were any errors
                console.log('Error retrievin localforage values key');
                console.log(err);
                deferredclientstorage.resolve('');

            });
        }

    }).catch(function (err) {
        // This code runs if there were any errors
        console.log('Error retrievin localforage datestored key');
        console.log(err);
        deferredclientstorage.resolve('');
    });

    return deferredclientstorage.promise();
}
function manageclientstorage(action, key, value, durationhours) {

    /*Lib per Zlib*/
    var pako = window.pako; //Compression Lib
    if (action == 'put') {

        localforage.removeItem(key).then(function () {
            var binaryString = pako.deflate(value, { to: 'string' }); //Compress
            localforage.setItem(key, binaryString).then(function (value) {
                // Do other things once the value has been saved.
                var datenow = new Date();
                datenow = new moment(datenow).add(durationhours, 'h');
                localforage.setItem('datestored', datenow.format("DD/MM/YYYY HH:mm:ss")).then(function (value) {
                    console.log(value);
                }).catch(function (err) {
                    console.log('localforage error inserting datestored');
                    console.log(err);
                });
            }).catch(function (err) {
                console.log('localforage error inserting values');
                console.log(err);
            });
        }).catch(function (err) {
            console.log('localforage error removing key');
            console.log(err);
        });

    }
    if (action == 'clear') {
        localforage.length().then(function (numberOfKeys) {
            console.log("Key in localforage db:" + numberOfKeys);
            if (numberOfKeys != 0)
                localforage.clear().then(function () {
                    console.log('cleared local memory and reload');
                    localforage.removeItem('datestored'); //Forza il rinnovamento della cache ( alo prossimo giro viene rinnovata la memoria del browser)
                    //navigate to url withou querystring
                    //window.location.replace(window.location.href.replace(/^([^\?]+)(\??.*)$/gi, "$1")).reload(true);
                    window.location.href = window.location.href.replace(/^([^\?]+)(\??.*)$/gi, "$1");
                    window.location.reload(true);

                }).catch(function (err) {
                    // This code runs if there were any errors
                    console.log('Error clearing localforage');
                    console.log(err);
                });

        }).catch(function (err) {
            // This code runs if there were any errors
            console.log(err);
        });


    }
    if (action == 'lenght') {
        localforage.length().then(function (numberOfKeys) {
            // Outputs the length of the database.
            console.log(numberOfKeys);
        }).catch(function (err) {
            // This code runs if there were any errors
            console.log(err);
        });
    }
}

/* //INIT REFERECES OLD VERSIONE ( con localstorage )
function initreferencesdata(lingua) {
    lng = lingua || "I";
    var deferred = $.Deferred();

    //QUI PUOI TESTARE CHE SE LE VARIABILI SONO NGIA CARICHE NON RICARICHI
    if (!referencesloaded) {
        var localstoragedata = managelocalstorage("get", "referencesvars" + lng, "", "");
        if (localstoragedata == '') {
            $.ajax({
                url: pathAbs + commonhandlerpath,
                contentType: "application/json; charset=utf-8",
                //async: false,
                cache: false,
                data: { 'q': 'initreferencesdata', 'lng': lng },
                success: function (result) {
                    try {

                        managelocalstorage("put", "referencesvars" + lng, result, 1); //Metto i valori di ritorno in localstorage con 1 ora scadenza
                        loadvariables(result);
                    }
                    catch (e) {
                        sendmessage('fail init initreferencesdata', e);
                    }
                    deferred.resolve('');
                },
                failure: function (result) {
                    sendmessage('fail init initreferencesdata', '');
                    deferred.resolve('');
                }
            });
        }
        else {

            loadvariables(localstoragedata); //Prendo i valori di init dal localstorage
            deferred.resolve('');
        }
    } else { deferred.resolve(''); }
    return deferred.promise();
}
function managelocalstorage(action, key, value, durationhours) {
    try {
        var pako = window.pako; //Compression Lib
        if (action == 'put') {
            localStorage.removeItem(key);
            var unused = 999999999;
            //var unused = getUnusedSpaceOfLocalStorageInBytes();
            //var used = getUsedSpaceOfLocalStorageInBytes();
            //var quota = unused + used;

            var binaryString = pako.deflate(value, { to: 'string' }); //Compress
            try {
                if (binaryString.length < unused) {

                    localStorage.setItem(key, binaryString)
                    //Controllare se quello che messo dentro è lungo come quello inserito
                    var datenow = new Date();
                    datenow = new moment(datenow).add(durationhours, 'h');
                    localStorage.setItem("datestored", datenow.format("DD/MM/YYYY HH:mm:ss"))
                }
            }
            catch (err) {
                localStorage.removeItem(key);
                console.log('localstorage size excedeed');
            }

        }
        if (action == 'get') {
            var expired = false;
            if (localStorage.getItem("datestored")) {
                var expiretime = localStorage.getItem("datestored");
                if (moment(new Date()) > moment(expiretime, "DD/MM/YYYY HH:mm:ss")) {
                    expired = true;
                    localStorage.removeItem("datestored");
                    localStorage.removeItem(key);
                }
            }
            if (expired) {
                return "";
            }
            else
                if (localStorage.getItem(key)) {
                    //  console.log("Used space" + getUsedSpaceOfLocalStorageInBytes());
                    value = localStorage.getItem(key);
                    value = pako.inflate(value, { to: 'string' }); //Descompress
                }
        }
        if (action == 'clear') {
            console.log('clear and reload');
            //CLEAR ALL LOCAL STORAGE
            window.localStorage.clear();
            location.reload(true);
            //localStorage.removeItem(key);
        }
    } catch (e) { }
    return value;
}
function getUsedSpaceOfLocalStorageInBytes() {
    // Returns the total number of used space (in Bytes) of the Local Storage
    var b = 0;
    for (var key in window.localStorage) {
        if (window.localStorage.hasOwnProperty(key)) {
            b += key.length + localStorage.getItem(key).length;
        }
    }
    return b;
}

function getUnusedSpaceOfLocalStorageInBytes() { //NON USARE QUESTA ALLOCA TROPPA MEMORIA NEL BROWSER
    var maxByteSize = 10485760; // 10MB
    var minByteSize = 0;
    var tryByteSize = 0;
    var testQuotaKey = 'testQuota';
    var timeout = 20000;
    var startTime = new Date().getTime();
    var unusedSpace = 0;
    var runtime;
    do {
        runtime = new Date().getTime() - startTime;
        try {
            tryByteSize = Math.floor((maxByteSize + minByteSize) / 2);
            localStorage.setItem(testQuotaKey, new Array(tryByteSize).join('1'));
            minByteSize = tryByteSize;
        } catch (e) {
            maxByteSize = tryByteSize - 1;
        }
    } while ((maxByteSize - minByteSize > 1) && runtime < timeout);

    localStorage.removeItem(testQuotaKey);

    if (runtime >= timeout) {
        console.log("Unused space calculation may be off due to timeout.");
    }

    // Compensate for the byte size of the key that was used, then subtract 1 byte because the last value of the tryByteSize threw the exception
    unusedSpace = tryByteSize + testQuotaKey.length - 1;
    return unusedSpace;
}

 */

function CaricaListaLingue() {
    if (jsonlanguages === '')
        jsonlanguages = (function () {
            var jsonlanguages = null;
            $.ajax({
                //async: false,
                global: false,
                //contentType: "application/json; charset=utf-8",
                cache: false,
                url: pathAbs + '/lib/cfg/languages.json',
                dataType: "text",
                success: function (data) {
                    jsonlanguages = data;
                    //console.log(jsonlanguages);

                },
                failure: function (result) {
                    sendmessage('fail init languages')
                }
            });
            return jsonlanguages;
        })();
}

function InizializzaPercorsiApplicazione() {
    /*Inizializzo i percorsi nelle variabili javascript*/
    if (percorsocomune === '' || percorsocontenuti === '')
        $.ajax({
            url: pathAbs + commonhandlerpath,
            contentType: "application/json; charset=utf-8",
            //async: false,
            cache: false,
            data: { 'q': 'initmainvars', 'lng': lng },
            success: function (result) {
                try {
                    var jobj = JSON.parse(result);
                    percorsocomune = jobj["percorsocomune"];
                    percorsocontenuti = jobj["percorsocontenuti"];
                    usecdn = jobj["usecdn"];

                }
                catch (e) {
                    sendmessage('fail init vars', e);
                }
            },
            failure: function (result) {
                sendmessage('fail init vars', '');
            }
        });
}
function CaricaGlobalResources(lng, searchtext) {

    var lng = lng || "I";
    var searchtext = searchtext || "";

    if (baseresources === '')
        $.ajax({
            url: pathAbs + commonhandlerpath,
            contentType: "application/json; charset=utf-8",
            cache: false,
            //async: false,
            data: { 'q': 'initresources', 'lng': lng },
            success: function (result) {
                try {
                    baseresources = JSON.parse(result);
                    //console.log(baseresources);

                }
                catch (e) { }
                return false;
            }
        });

}



function GetResourcesValue(key, data) {
    var ret = "";
    var data = data || baseresources;
    if (data != null)
        ret = data[lng][key];
    return ret;

}
/*------------FUNZIONI RIEMPIMENTO DDL ------------------------------------------------*/
function fillDDLArraySimple(container, jsonDictionary, selectionText, selectionValue, nameKey, nameValue, selectedvalue) {
    try {
        var sel = '';
        try { sel = baseresources[lng]["selectgeneric"]; } catch (e) { }
        if (selectionText != null) sel = selectionText;
        var $select = $(container);
        if ($select) {
            $select.find('option').remove();
            var listitems = "";
            listitems += '<option value="' + selectionValue + '"> ' + sel + ' </option>';
            try {
                for (var i = 0; i < jsonDictionary.length; i++) {
                    listitems += '<option style="cursor:pointer;" value="' + jsonDictionary[i][nameKey] + '">' + jsonDictionary[i][nameValue] + '</option>';
                }
            }
            catch (e1) { }
            $select.append(listitems);
            $select.val(selectedvalue);
            if ($select.val() != selectedvalue && selectedvalue != null) {
                //$select.find('option').remove();
                //listitems += '<option value="' + selectedvalue + '">' + selectedvalue + '</option>';
                //$select.append(listitems);
                //$select.val(selectedvalue);
            }
            //$select.change(); //Chiama il change per aggiornare le variabili javascript ( se presente l'evento nel controllo )
        }
    }
    catch (e) {
    }
}

function fillDDLArray(container, jsonDictionary, selectionText, selectionValue, nameKey, nameValue) {
    try {
        var sel = '';
        try { sel = baseresources[lng]["selectgeneric"]; } catch (e) { }
        if (selectionText != null) sel = selectionText;
        var $select = $(container).clone();
        if ($select.length) {

            //$select.find('option').remove();
            var listitems = "";
            // listitems += '<option value="' + selectionValue + '"> ' + sel + ' </option>';

            try {
                for (var i = 0; i < jsonDictionary.length; i++) {
                    listitems += '<option style="cursor:pointer;" value="' + jsonDictionary[i][nameKey] + '">' + jsonDictionary[i][nameValue] + '</option>';
                }
            }
            catch (e1) { }
            $select.append(listitems);

        }
        return $select;
    }
    catch (e) {
    }
}

function initDdl(q, ddlid, selecttext, selectvalue, selectedvalue, lng, filter1, filter2) {
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        cache: false,
        data: { 'q': q, 'lng': lng, 'filter1': filter1, 'filter2': filter2 },
        success: function (result) {
            try {
                fillDDL(ddlid, result, selecttext, selectvalue, selectedvalue);
            }
            catch (e) { }
            return false;
        }
    });
}
function fillDDL(ddlID, jsonDictionary, selectionText, selectionVaue, selectedvalue) {
    try {
        // console.log("fiiDDL: " + ddlID + ",deftext:" + selectionText + ",defval:" + selectionVaue + ",selval:" + selectedvalue)
        var sel = baseresources[lng]["selectgeneric"];
        if (selectionText != null) sel = selectionText;
        var $select = $('#' + ddlID);
        if ($select.length) {
            $select.find('option').remove();
            var listitems = "";
            listitems += '<option value="' + selectionVaue + '"> ' + sel + ' </option>';
            try {
                var opers = JSON.parse(jsonDictionary);
                $.each(opers, function (key, value) {
                    listitems += '<option value="' + key + '">' + value + '</option>';
                });
            }
            catch (e1) { }
            $select.append(listitems);

            $select.val(selectedvalue);
            if ($select.val() != selectedvalue && selectedvalue != null) {
                $select.find('option').remove();
                listitems += '<option value="' + selectedvalue + '">' + selectedvalue + '</option>';
                $select.append(listitems);
                $select.val(selectedvalue);
            }
            $select.click(); //Chiama il click per aggiornare le variabili javascript ( se presente l'evento nel controllo )

        }
    }
    catch (e) {

    }
}

/*----------------------GESIONE VALORI GEO -------------------------------*/

/*Caricamento Struttura JSON per regioni provincie comuni in una variabile javascript per la visualizzazione*/
/*Funzionin caricamento loste JSON geografiche*/
function caricajsonnazioni(lng) {
    var lng = lng || "I";
    //JSONnazioni
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        cache: false,
        async: false,
        data: { 'q': 'caricaJSONnazioni', 'lng': lng },
        success: function (result) {
            try {
                JSONnazioni = JSON.parse(result);;
            }
            catch (e) { }
            return false;
        }
    });

}

function caricajsonregioni(lng) {
    var lng = lng || "I";
    //JSONprovince
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        cache: false,
        async: false,
        data: { 'q': 'caricaJSONregioni', 'lng': lng },
        success: function (result) {
            try {
                JSONregioni = JSON.parse(result);
            }
            catch (e) { }
            return false;
        }
    });
}
function caricajsonprovince(lng) {
    var lng = lng || "I";
    //JSONprovince
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        cache: false,
        async: false,
        data: { 'q': 'caricaJSONprovince', 'lng': lng },
        success: function (result) {
            try {
                JSONprovince = JSON.parse(result);
            }
            catch (e) { }
            return false;
        }
    });
}
function caricajsoncomuni(lng) {
    var lng = lng || "I";
    //JSONprovince
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        cache: false,
        async: false,
        data: { 'q': 'caricaJSONcomuni', 'lng': lng },
        success: function (result) {
            try {
                JSONcomuni = JSON.parse(result);
            }
            catch (e) { }
            return false;
        }
    });
}


function GestioneDdlGeo(idcaller, lng, seln, selr, selp, selc, idddln, idddlr, iddlp, idddlc) {
    idddln = idddln || "ddlNazione";
    idddlr = idddlr || "ddlRegione";
    iddlp = iddlp || "ddlProvincia";
    idddlc = idddlc || "ddlComune";

    //console.log(idcaller + "," + lng + "," + seln + "," + selr + "," + selp + "," + selc + "," + idddln +  "," +  idddlr + "," + iddlp + "," + idddlc);

    var callerpars = idcaller.split(',');
    var callerid = "";
    var cascade = "";
    if (callerpars != null && callerpars.length != 0) {
        switch (callerpars.length) {
            case 1:
                callerid = callerpars[0];
                break;
            case 2:
                callerid = callerpars[0];
                cascade = callerpars[1];
                break;
        }
    }

    if (callerid == 'all') {
        //console.log(seln + "," + selr + "," + selp + "," + selc);
        initDdl('fillddlnazioni', idddln, baseresources[lng]["selectnazione"], '', seln, lng);
        initDdl('fillddlregioni', idddlr, baseresources[lng]["selectregione"], '', selr, lng, seln);
        initDdl('fillddlprovince', iddlp, baseresources[lng]["selectprovincia"], '', selp, lng, seln, selr);
        initDdl('fillddlcomuni', idddlc, baseresources[lng]["selectcomune"], '', selc, lng, seln, selp);
    }
    else {
        var flagcascade = false;
        if (callerid == idddln) {
            initDdl('fillddlnazioni', idddln, baseresources[lng]["selectnazione"], '', seln, lng);
            if (cascade == 'cascade') flagcascade = true;
        }
        if (callerid == idddlr || flagcascade) {
            initDdl('fillddlregioni', idddlr, baseresources[lng]["selectregione"], '', selr, lng, seln);
            if (cascade == 'cascade') flagcascade = true;
        }
        if (callerid == iddlp || flagcascade) {
            initDdl('fillddlprovince', iddlp, baseresources[lng]["selectprovincia"], '', selp, lng, seln, selr);
            if (cascade == 'cascade') flagcascade = true;
        }
        if (callerid == idddlc || flagcascade) {
            initDdl('fillddlcomuni', idddlc, baseresources[lng]["selectcomune"], '', selc, lng, seln, selp);
        }
    }

}
/*-------------------fine gestione valori GEO --------------------------------------------------------*/

/* ---------- FUNZIONI GESTIONE DATI E BINDING -------------------------------------------------------*/

function CleanHtml(el) {
    el.find('*').removeAttr("mybind");
    el.find('*').removeAttr("mybind1");
    el.find('*').removeAttr("mybind2");
    el.find('*').removeAttr("mybind3");
    el.find('*').removeAttr("myvalue");
    el.find('*').removeAttr("myvalue1");
    el.find('*').removeAttr("myvalue2");
    el.find('*').removeAttr("myvalue3");
    el.find('*').removeAttr("format");
    el.find('*').removeClass("bind");
    el.find('*').removeClass("inject");
    el.find('*').removeAttr("params");

    el.removeAttr("mybind");
    el.removeAttr("mybind1");
    el.removeAttr("mybind2");
    el.removeAttr("mybind3");
    el.removeAttr("myvalue");
    el.removeAttr("myvalue1");
    el.removeAttr("myvalue2");
    el.removeAttr("myvalue3");
    el.removeAttr("format");
    el.removeClass("bind");
    el.removeClass("inject");
    el.removeAttr("params");


}


/*Visualizza un lista i dati passati col template indicato*/
function ShowList(templatename, container, controlid, data) {
    var localObjects = {};

    var templateHtml = pathAbs + "/lib/template/" + "genericlista.html";
    if (templatename != null && templatename != '')
        templateHtml = pathAbs + "/lib/template/" + templatename;

    //Correggo l'id dei controlli del template per l'inzializzazione dello scroller univoca e corretta
    $('#' + container).html('');

    if (data !== null && data.length > 0) {
        $('#' + container).load(templateHtml, function () {
            $('#' + container).find("[id^=replaceid]").each(function (index, text) {
                var currentid = $(this).prop("id");
                var replacedid = currentid.replace('replaceid', controlid);
                $(this).prop("id", replacedid);
            });
            setTimeout(function () {
                if (!data.length) return;
                var str = $('#' + controlid)[0].outerHTML;
                //$('#' + el).parent().parent().parent().parent().show();
                //Se presente nella memoria temporanea globale modelli devo riprendere la struttura HTML template da li e non dalla pagina modficata
                //in caso di rebinding successivo dopo l'iniezione del template
                if (!globalObject.hasOwnProperty(controlid + "template")) {
                    globalObject[controlid + "template"] = $('#' + controlid)[0].outerHTML;
                    str = globalObject[controlid + "template"];
                }
                else
                    str = globalObject[controlid + "template"];

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
                //Inseriamo htmlout nel contenitore  $('#' + el).html 
                $('#' + controlid).html('');
                $('#' + controlid).html(htmlout);
                CleanHtml($('#' + controlid));
            }, 500);
        });
    }
}




/*Riceve una stringa Html parserizzata con jquery per il fill coi dati*/
function FillBindControls(jquery_obj, dataitem, localObjects, classselector, callback) {

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

                    if ($(this).is("label")) {
                        $(this).attr("value", dataitem[proprarr[0]]);
                        $(this).html(dataitem[proprarr[0]]);
                    }
                    else if ($(this).is("input") && $(this).attr('type') == 'checkbox') {
                        if (dataitem.hasOwnProperty(proprarr[0]))
                            $(this).prop("checked", dataitem[proprarr[0]]);
                        else
                            $(this).prop("checked", false);
                    }
                    else if ($(this).is("input") && ($(this).attr('type') == 'text' || $(this).attr('type') == null)) {
                        if (dataitem.hasOwnProperty(proprarr[0]))
                            $(this).attr("value", dataitem[proprarr[0]]);
                        else
                            $(this).attr("value", '');

                        if ($(this).attr("idbind") != null)
                            $(this).attr("idbind", dataitem[$(this).attr("idbind")]);


                        if ($(this).attr("placeholder") != null)
                            $(this).attr("placeholder", baseresources[lng][$(this).attr("placeholder")]);

                    }

                    else if ($(this).is("textarea")) {
                        if (dataitem.hasOwnProperty(proprarr[0]))
                            $(this).text(dataitem[proprarr[0]]);
                        else
                            $(this).text('');

                        if ($(this).attr("idbind") != null)
                            $(this).attr("idbind", dataitem[$(this).attr("idbind")]);

                    }
                    else if ($(this).is("button") && $(this).hasClass('select')) {
                        var idscheda = dataitem[proprarr[0]];
                        var prop = [];
                        if ($(this).attr("myvalue") != null)
                            prop[0] = $(this).attr("myvalue");
                        $(this).attr("onclick", "javascript:" + prop[0] + "('" + idscheda + "')");
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


                        if (localObjects["linkloaded"].hasOwnProperty(idscheda)) {
                            if (localObjects["linkloaded"][idscheda].hasOwnProperty(bindprophref)) {
                                link = localObjects["linkloaded"][idscheda][bindprophref];
                                $(this).attr("href", link);
                                //$(this).show();
                                $(this).css("display", "block");
                            }
                            if (localObjects["linkloaded"][idscheda].hasOwnProperty(bindproptitle)) {
                                testo = localObjects["linkloaded"][idscheda][bindproptitle];
                                $(this).attr("title", testo);
                            }
                            if (link.toLowerCase().indexOf(pathAbs) != 0) $(this).attr("target", "_blank");
                            //if (!link.toLowerCase().startsWith(pathAbs)) $(this).attr("target", "_blank");

                        }
                        else {
                            $(this).attr("href", '');
                            $(this).css("display", "none");
                        }
                    }
                    else if ($(this).is("img") && $(this).hasClass('revolution')) {
                        var idallegato = dataitem[proprarr[0]];
                        if (dataitem.hasOwnProperty(proprarr[0])) {
                            //var testoalt = localObjects["linkloaded"][idallegato]['testoalt'];
                            var pathImg = localObjects["linkloaded"][idallegato]['image'];
                            $(this).attr("data-lazyload", pathImg);
                            //$(this).attr("alt", testoalt);
                        }
                    }
                    else if ($(this).is("img") && $(this).hasClass('avatar')) {
                        var idallegato = dataitem[proprarr[0]];
                        if (dataitem.hasOwnProperty(proprarr[0])) {
                            //var testoalt = localObjects["linkloaded"][idallegato]['testoalt'];
                            var pathImg = localObjects["linkloaded"][idallegato]['avatar'];
                            $(this).attr("src", pathImg);
                            //$(this).attr("alt", testoalt);
                        }
                    }
                    else if ($(this).is("li") && $(this).hasClass('revolution')) {
                        var idallegato = dataitem[proprarr[0]];
                        if (dataitem.hasOwnProperty(proprarr[0])) {
                            var pathImg = localObjects["linkloaded"][idallegato]['image'];
                            var link = localObjects["linkloaded"][idallegato]['link'];
                            $(this).attr("data-link", link);
                            $(this).attr("data-thumb", pathImg);
                        }
                    }
                    else if ($(this).is("img")) {
                        var completepath = "";
                        var idallegato = dataitem[proprarr[0]];
                        if (dataitem.hasOwnProperty(proprarr[0])) {
                            CompleteUrlPrimaryImg(localObjects, idallegato, true, usecdn, function (imgret) {
                                completepath = imgret;
                            });
                            if ($(this).hasClass('img-ant') && completepath.indexOf('dummylogo') == -1) {
                                var position = completepath.lastIndexOf('/');
                                completepath = completepath.substr(0, position + 1) + "ant" + completepath.substr(position + 1);
                            }
                            if (completepath != null && completepath != '')
                                $(this).attr("src", completepath);
                            if ($(this).hasClass('lazy')) {
                                $(this).attr("data-src", completepath);
                                $(this).attr("src", "");
                            }
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
                        CompleteUrlListImgs(localObjects, idallegato, false, usecdn, function (ret) {
                            imgslist = ret;
                        })
                        CompleteUrlListImgsDesc(localObjects, idallegato, false, usecdn, function (ret) {
                            imgslistdesc = ret;
                        })

                        CompleteUrlListImgsRatio(localObjects, idallegato, false, usecdn, function (ret) {
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
                                    maxheight = maxheight.replace("px", "");
                                    var docwidth = $(document).width();
                                    if (maxheight > docwidth) maxheight = docwidth;
                                    try {
                                        if (parseFloat(imgslistratio[j].replace(",", ".")) < 1) {
                                            //imgstyle = "max-width:100%;width:auto;height:" + maxheight + "px;";
                                            imgstyle = "max-width:100%;width:auto;height:" + maxheight + "px;";
                                        }
                                    }
                                    catch (e) {
                                    };
                                }

                              //  contenutoslide += '<a rel="prettyPhoto[pp_gal]" href="' + imgslist[j] + '">';

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

                              //  contenutoslide += '</a>';


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
                        CompleteUrlListImgs(localObjects, idallegato, false, usecdn, function (imgret) {
                            imgslist = imgret;
                        })
                        CompleteUrlListImgsDesc(localObjects, idallegato, false, usecdn, function (filesret) {
                            imgslistdesc = filesret;
                        })
                        if (imgslist.length > 1)
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
                        && ($(this).hasClass('bookingtool'))
                    ) {
                        var idrisorsa = dataitem[proprarr[0]];
                        bookingtool.initbookingtool(idrisorsa, $(this).attr("id"));
                    }
                    else if ($(this).is("div")
                        && ($(this).hasClass('carellotool'))
                    ) {
                        var idrisorsa = dataitem[proprarr[0]];
                        //var prezzounitario = dataitem[proprarr[1]]; // da passaere
                        var idcontrollo = $(this).attr("id");
                        carrellotool.initcarrellotool(idrisorsa, '', username, idcontrollo, 2); //1 carrello con data range //2 carreelo standard //3 entrambi
                    }
                    else if ($(this).is("div")
                        && ($(this).hasClass('owl-carousel') || $(this).hasClass('img-list'))
                    ) {
                        var imgslist = "";
                        var contenutoslide = "";
                        var idallegato = dataitem[proprarr[0]];
                        /*Lista completa degli allegati per l'immobile*/
                        CompleteUrlListImgs(localObjects, idallegato, false, usecdn, function (imgret) {
                            imgslist = imgret;
                        })
                        for (var j = 0; j < imgslist.length; j++) {
                            contenutoslide += '<div class="item">';
                            contenutoslide += '<img  class="img-responsive"  src="';
                            contenutoslide += imgslist[j];
                            contenutoslide += '"/>';
                            contenutoslide += '</div>';
                        }
                        $(this).html(contenutoslide);
                        if (contenutoslide !== '')
                            $(this).parent().show();

                    }
                    else if ($(this).is("div")
                        && ($(this).hasClass('files-list'))
                    ) {
                        var fileslist = "";
                        var fileslistdesc = "";
                        var filelink = "";
                        var idallegato = dataitem[proprarr[0]];
                        /*Lista completa degli allegati per l'immobile*/
                        CompleteUrlListFiles(localObjects, idallegato, false, usecdn, function (filesret) {
                            fileslist = filesret;
                        })
                        CompleteUrlListFilesDesc(localObjects, idallegato, false, usecdn, function (filesret) {
                            fileslistdesc = filesret;
                        })
                        for (var j = 0; j < fileslist.length; j++) {
                            filelink += '<a  style="margin-right:10px;margin-bottom:10px;min-width:190px" class="buttonstyle" target="_blank" href="';
                            filelink += fileslist[j];
                            filelink += '" ><i class="fa fa-search"></i>';
                            try {
                                filelink += fileslistdesc[j];
                            } catch (e) {
                                filelink += fileslist[j];
                            };
                            filelink += '</a>';
                        }
                        $(this).html(filelink);
                        if (filelink !== '') $(this).show();

                    }
                    else if (($(this).is("div") || $(this).is("section"))
                        && ($(this).hasClass('imgback'))
                    ) {
                        var imgslist = "";
                        var idallegato = dataitem[proprarr[0]];
                        var completepath = "";
                        /*Lista completa degli allegati per l'immobile*/
                        CompleteUrlPrimaryImg(localObjects, idallegato, true, usecdn, function (imgret) {
                            completepath = imgret;
                        });
                        if (completepath != null && completepath != '')
                            $(this).css("background-image", "url('" + completepath + "')");
                        //$(this).attr("style", "background-image: url('" + completepath + "')");

                    }
                    else if (($(this).is("div") || $(this).is("section"))
                        && ($(this).hasClass('bckvideoelement'))
                    ) {
                        var imgslist = "";
                        var id = dataitem[proprarr[0]];
                        var completepath = "";
                        /*Allegato primario*/
                        CompleteUrlPrimaryImg(localObjects, id, true, usecdn, function (imgret) {
                            completepath = imgret;
                        });
                        if (completepath != null && completepath != '') {
                            var styletext = $(this).attr("style");
                            if (styletext != undefined && styletext != null && styletext != '') {
                                styletext = styletext.replace(/urlplaceholder/g, completepath);
                                $(this).attr("style", styletext);
                            }
                        }
                        var link = localObjects["linkloaded"][id]['link'];
                        if (link != null && link != '') {
                            var datatext = $(this).attr("data-property");
                            if (datatext != undefined && datatext != null && datatext != '') {
                                datatext = datatext.replace(/videoplaceholder/g, link);
                                $(this).attr("data-property", datatext);
                            }
                        }
                        //var testo = localObjects["linkloaded"][id]['titolo'];

                    }
                    else if ($(this).is("meta")) {
                        if (dataitem.hasOwnProperty(proprarr[0])) {
                            var valore = [];
                            valore[0] = dataitem[proprarr[0]]; //id attuale record
                            var prop = [];
                            var formatfunc = "";
                            if ($(this).attr("format") != null) {
                                formatfunc = $(this).attr("format");
                                if ($(this).attr("mybind1") != null)
                                    valore[1] = dataitem[$(this).attr("mybind1")];
                                if ($(this).attr("mybind2") != null)
                                    valore[2] = dataitem[$(this).attr("mybind2")];
                                if ($(this).attr("myvalue") != null)
                                    prop[0] = $(this).attr("myvalue");
                                if ($(this).attr("myvalue1") != null)
                                    prop[1] = $(this).attr("myvalue1");
                                if ($(this).attr("myvalue2") != null)
                                    prop[2] = $(this).attr("myvalue2");

                                window[formatfunc](localObjects, valore, prop, function (ret) {
                                    // valore = ret;
                                    if (ret != null && Array.isArray(ret) && ret.length > 0)
                                        valore = ret[0];
                                    else
                                        valore = ret;
                                });
                                $(this).attr("content", valore);
                            }
                        }
                    }
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
                    //else if ($(this).is("div")
                    //    && ($(this).hasClass('owl-slider-v2'))
                    //    ) {
                    //    var contenutoslide = "";
                    //}
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

                            //if (valore != '' && valore != null) $(this).show();
                            //$(this).html(valore);

                            if (valore == "true" || valore == "false") {
                                if (valore == "true")
                                    $(this).hide();
                            }
                            else
                                $(this).html(valore);

                        }
                        else
                            $(this).html('');
                    }


                    break;
                case 2: //Oggetto bind di 2 livelli
                    if ($(this).is("span")) {
                        if ($(this).attr('mybind1') != undefined) {
                            var object = dataitem[proprarr[0]];
                            var property = proprarr[1];
                            var valore = object[property];
                            $(this).html(valore);
                        }
                        else {
                            var idelement = dataitem[proprarr[0]];
                            var property = proprarr[1];
                            var valore = "";
                            if (localObjects["linkloaded"].hasOwnProperty(idelement)) {
                                if (localObjects["linkloaded"][idelement].hasOwnProperty(property)) {
                                    valore = localObjects["linkloaded"][idelement][property];
                                    $(this).html(valore);
                                }
                            }
                            else
                                $(this).html(valore);
                        }
                    }
                    else if ($(this).is("iframe")) {
                        var idelement = dataitem[proprarr[0]];
                        var property = proprarr[1];
                        var valore = "";
                        if (localObjects["linkloaded"].hasOwnProperty(idelement)) {
                            if (localObjects["linkloaded"][idelement].hasOwnProperty(property)) {
                                if (valore != null && valore != '') {
                                    valore = localObjects["linkloaded"][idelement][property];
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
};

function CompleteUrlPrimaryImg(localObjects, idallegati, anteprima, usecdn, callback) {
    var pathfile = "";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(idallegati)) {
            pathfile = object[idallegati].image;
        }
    } catch (e) { };
    callback(pathfile);
};
function CompleteUrlListImgs(localObjects, idallegati, anteprima, usecdn, callback) {
    var arrayimgs = [];
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(idallegati)) {
            var completeimgs = JSON.parse(object[idallegati].imageslist);
            for (var j = 0; j < completeimgs.length; j++) {
                arrayimgs.push(completeimgs[j]);
            }
        }
    } catch (e) {
    };
    callback(arrayimgs);
}
function CompleteUrlListImgsDesc(localObjects, idallegati, anteprima, usecdn, callback) {
    var arrayimgs = [];
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(idallegati)) {
            var completeimgs = JSON.parse(object[idallegati].imagesdesc);
            for (var j = 0; j < completeimgs.length; j++) {
                arrayimgs.push(completeimgs[j]);
            }
        }
    } catch (e) {
    };
    callback(arrayimgs);
}
function CompleteUrlListImgsRatio(localObjects, idallegati, anteprima, usecdn, callback) {
    var arrayimgs = [];
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(idallegati)) {
            var completeimgs = JSON.parse(object[idallegati].imagesratio);
            for (var j = 0; j < completeimgs.length; j++) {
                arrayimgs.push(completeimgs[j]);
            }
        }
    } catch (e) {
    };
    callback(arrayimgs);
}
function CompleteUrlListFiles(localObjects, idallegati, anteprima, usecdn, callback) {
    var arrayfiles = [];
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(idallegati)) {
            var completefiles = JSON.parse(object[idallegati].fileslist);
            for (var j = 0; j < completefiles.length; j++) {
                arrayfiles.push(completefiles[j]);
            }
        }
    } catch (e) {
    };
    callback(arrayfiles);
}
function CompleteUrlListFilesDesc(localObjects, idallegati, anteprima, usecdn, callback) {
    var arrayfiles = [];
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(idallegati)) {
            var completefiles = JSON.parse(object[idallegati].filesdesc);
            for (var j = 0; j < completefiles.length; j++) {
                arrayfiles.push(completefiles[j]);
            }
        }
    } catch (e) {
    };
    callback(arrayfiles);
}

function frmvisibility(localObjects, valore, prop, callback) {
    var retstring = "false";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(valore[0])) {
            var contenuto = object[valore[0]][prop[0]];
            if (contenuto != null && contenuto != '') {
                retstring = "true";
            }
        }
    } catch (e) { };
    callback(retstring);
}
function formatprezzoofferta(localObjects, valore, prop, callback) {
    var retstring = "";
    var unit = baseresources[lng]["valuta"];
    var controllo = localObjects["resultinfo"][prop[0]];
    if (controllo == "true") {
        if (valore[0] != '0')
            retstring = valore[0].formatMoney(0, '.', ',') + ' ' + unit;
    }
    callback(retstring);
}

function formattitle(localObjects, valore, prop, callback) {
    var retstring = "";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(valore[0])) {

            var titolo = object[valore[0]][prop[0]];
            var i = titolo.indexOf("\n");
            if (i > 0) {
                titolo = titolo.substring(0, i);
            }
            retstring = titolo;
        }
    } catch (e) { };
    callback(retstring);
}

function formatsubtitle(localObjects, valore, prop, callback) {
    var retstring = "";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(valore[0])) {

            var titolo = object[valore[0]][prop[0]];
            var i = titolo.indexOf("\n");
            if (i > 0) {
                if (titolo.length >= i + 1)
                    titolo = titolo.substring(i + 1, titolo.length);
            }
            retstring = titolo;
        }
    } catch (e) { };
    callback(retstring);
}


function formatbtncarrello(localObjects, valore, prop, callback) {
    var retstring = "";
    var testoCarelloesaurito = baseresources[lng]["testocarelloesaurito"];
    var testoInseriscicarrello = baseresources[lng]["testoinseriscicarrello"];
    var testoVedi = baseresources[lng]["vedi"];

    var id = valore[0];
    var qtavendita = valore[1];
    var xmlvalue = valore[2];
    var prezzo = valore[3];
    //JSON.parse(localObjects.dataloaded)["datalist"]

    if (qtavendita == 0) {
        retstring = "<div style=\"width:90px;line-height:15px; padding-top:10px;\"  class=\"divbuttonstyle\"  >" + testoCarelloesaurito + "</div>";
    } else {
        //retstring = "<button type=\"button\" style=\"float:right\" class=\"btn btn-purple btn-small trigcarrello\" title=\"" + id + "," + lng + "," + username + "\"  >" + testoInseriscicarrello + "</button>";
        var testocall = id + "," + lng + "," + username;

        retstring = "<button type=\"button\" style=\" background-color:#121212;\" class=\"btn btn-purple\" onclick=\"javascript:InserisciCarrelloNopostback('" + testocall + "')\"  >" + testoInseriscicarrello + "</button>";

        if ((xmlvalue != null && xmlvalue != "") || (prezzo == null || prezzo == "" || prezzo == 0)) {
            var link = localObjects["linkloaded"][id]["link"];
            retstring = "<a href=\"" + link + "\" target=\"_self\" >";
            retstring += "<div  style=\"background-color:#121212\" class=\"btn btn-purple\"  >" + testoVedi + "</div>";
            retstring += "</a>";
        }
    }
    callback(retstring);
}

function formatlinksezione(localObjects, valore, prop, callback) {
    var retstring = "";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(valore[0])) {
            var linksezione = object[valore[0]][prop[0]];
            if (linksezione != null && linksezione != "")
                retstring = linksezione;
        }
    } catch (e) { };
    callback(retstring);
}

function formatautore(localObjects, valore, prop, callback) {
    var retstring = "";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(valore[0])) {
            var autore = object[valore[0]][prop[0]];
            if (autore != null && autore != "")
                retstring = autore;
        }
    } catch (e) { };
    callback(retstring);
}
function formatviews(localObjects, valore, prop, callback) {
    var retstring = "";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(valore[0])) {
            var valore = object[valore[0]][prop[0]];
            if (valore != null && valore != "")
                retstring = valore;
        }
    } catch (e) { };
    callback(retstring);
}
function formattestoreplace(localObjects, valore, prop, callback) {
    var retstring = "";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(valore[0])) {
            retstring = object[valore[0]][prop[0]];
        }
    } catch (e) { };
    callback(retstring);

}
function formatdescrizione(localObjects, valore, prop, callback) {
    var retstring = "";
    try {
        var object = localObjects["linkloaded"];
        if (localObjects["linkloaded"].hasOwnProperty(valore[0])) {

            var descrizione = object[valore[0]][prop[0]];
            if (prop[1] != undefined && prop[1] != null && prop[1] != "") {
                if (descrizione.length >= parseInt(prop[1])) {
                    var i = parseInt(prop[1]); var j = 1; var stop = false;
                    while (j < 30 && !stop && i + j + 1 < descrizione.lenght) {
                        if (descrizione.substring(i + j, i + j + 1) == ' ' || descrizione.substring(i + j, i + j + 1) == '.' || descrizione.substring(i + j, i + j + 1) == '\n') stop = true;
                        j += 1;
                    }
                    descrizione = descrizione.substring(0, parseInt(prop[1]) + j) + " >>";
                }
                if (prop[2] != undefined && prop[2] != null && prop[2] == "nobreak")
                    retstring = descrizione.replace(/\n/g, "&nbsp;");
                else
                    retstring = descrizione.replace(/\n/g, "<br/>");


            }
        }
    } catch (e) { };
    callback(retstring);

}




function formatlabelsconto(localObjects, valore, prop, callback) {
    var retstring = "";
    try {
        var testosconto = baseresources[lng]["testosconto"];
        // <div class="csstransforms prod_discount">Sconto 20%</div>
        var prezzo = valore[0];
        var prezzolistino = valore[1];
        if (Number(prezzolistino) != 0 && Number(prezzo) != 0 && (Number(prezzolistino) > Number(prezzo))) {
            retstring = "<div class=\"csstransforms prod_discount\">" + testosconto + " ";
            retstring += Math.floor((Number(prezzolistino) - Number(prezzo)) / Number(prezzolistino) * 100) + " %";
            retstring += "</div>";
        }

    } catch (e) { };
    callback(retstring);

}

function formatlabelresource(localObjects, valore, prop, callback) {
    var retstring = "";
    try {

        var controllo = localObjects["resultinfo"][prop[1]];
        if (controllo == "true" || controllo == null) {
            retstring = baseresources[lng][prop[0]];
        }
    } catch (e) { };
    callback(retstring);

}
function frmcategoria(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONtipologia);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONcategorie;
    var selvalue = "";
    //var selvalue = JSON.search(dataroot, '//data[Codice="' + valore[0] + '"]/Campo1');
    if (JSONcategorie != null)
        for (var j = 0; j < dataroot["data"].length; j++) {
            if (dataroot["data"][j].Codice == valore[0]) {
                selvalue = dataroot["data"][j].Campo1;
                break;

            }
        }

    //if (selvalue != null && selvalue != undefined && selvalue.length > 0)
    //    selvalue = selvalue[0].toLowerCase();
    callback(selvalue);
}

function frmcategoria2liv(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONtipologia);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONcategorie2liv;
    var selvalue = "";
    //var selvalue = JSON.search(dataroot, '//data[Codice="' + valore[0] + '"]/Campo1');
    if (JSONcategorie2liv != null)
        for (var j = 0; j < dataroot["data"].length; j++) {
            if (dataroot["data"][j].Codice == valore[0]) {
                selvalue = dataroot["data"][j].Campo1;
                break;

            }
        }

    //if (selvalue != null && selvalue != undefined && selvalue.length > 0)
    //    selvalue = selvalue[0].toLowerCase();
    callback(selvalue);
}

function frmtipologia(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONtipologia);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = jsontipologie;
    var selvalue = "";
    //var selvalue = JSON.search(dataroot, '//data[Codice="' + valore[0] + '"]/Campo1');
    for (var j = 0; j < dataroot["data"].length; j++) {
        if (dataroot["data"][j].Codice == valore[0] && dataroot["data"][j].Lingua == lng) {
            selvalue = dataroot["data"][j].Campo1;
            break;

        }
    }

    //if (selvalue != null && selvalue != undefined && selvalue.length > 0)
    //    selvalue = selvalue[0].toLowerCase();
    callback(selvalue);
}
function frmprovincia(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONgeogenerale);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONprovince;
    var selvalue = '';
    //var selvalue = JSON.search(dataroot, '(//data[cod_provincia=\"' + valore[0] + '\" and lingua=\"' + lng + '\" ]/provincia)[last()]');

    for (var j = 0; j < dataroot["data"].length; j++) {
        if (dataroot["data"][j].Codice == valore[0]) {
            selvalue = dataroot["data"][j].Campo1;
            break;
        }
    }
    if (selvalue != null && selvalue != undefined && selvalue.length > 0)
        selvalue = selvalue.toLowerCase();
    callback(selvalue);
}
function frmregione(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONgeogenerale);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONregioni;

    //console.log(dataroot);
    //var selvalue = JSON.search(dataroot, '(//data[cod_regione=\"' + valore[0] + '\" and lingua=\"' + lng + '\"]/regione)[last()]');
    var selvalue = '';
    for (var j = 0; j < dataroot["data"].length; j++) {
        if (dataroot["data"][j].Codice == valore[0]) {
            selvalue = dataroot["data"][j].Campo1;
            break;
        }
    }

    if (selvalue != null && selvalue != undefined && selvalue.length > 0)
        selvalue = selvalue.toLowerCase();
    callback(selvalue);
}




function frmcaratteristica1(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONtipologia);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONcar1;
    var selvalue = "";
    //var selvalue = JSON.search(dataroot, '//data[Codice="' + valore[0] + '"]/Campo1');
    for (var j = 0; j < dataroot["data"].length; j++) {
        if (dataroot["data"][j].Codice == valore[0]) {
            selvalue = dataroot["data"][j].Campo1;
            break;

        }
    }

    //if (selvalue != null && selvalue != undefined && selvalue.length > 0)
    //    selvalue = selvalue[0].toLowerCase();
    callback(selvalue);
}
function frmcaratteristica2(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONtipologia);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONcar2;
    var selvalue = "";
    //var selvalue = JSON.search(dataroot, '//data[Codice="' + valore[0] + '"]/Campo1');
    for (var j = 0; j < dataroot["data"].length; j++) {
        if (dataroot["data"][j].Codice == valore[0]) {
            selvalue = dataroot["data"][j].Campo1;
            break;

        }
    }

    //if (selvalue != null && selvalue != undefined && selvalue.length > 0)
    //    selvalue = selvalue[0].toLowerCase();
    callback(selvalue);
}
function frmcaratteristica3(localObjects, valore, prop, callback) {
    //var dataroot = "{ \"data\":" + JSON.stringify(JSONtipologia);
    //dataroot += "}";
    //dataroot = JSON.parse(dataroot);

    var dataroot = {};
    dataroot.data = JSONcar3;
    var selvalue = "";
    //var selvalue = JSON.search(dataroot, '//data[Codice="' + valore[0] + '"]/Campo1');
    for (var j = 0; j < dataroot["data"].length; j++) {
        if (dataroot["data"][j].Codice == valore[0]) {
            selvalue = dataroot["data"][j].Campo1;
            break;

        }
    }

    //if (selvalue != null && selvalue != undefined && selvalue.length > 0)
    //    selvalue = selvalue[0].toLowerCase();
    callback(selvalue);
}
function formatdata1(localObjects, valore, prop, callback) {
    var retstring = "";
    var tmpDate = valore[0];
    var controllo = localObjects["resultinfo"][prop[0]];
    if (controllo == "true") {
        if (tmpDate && tmpDate != "") {
            var objData = new Date(tmpDate);

            //var dateFormattedwithtime = getDate(objData) + " " + getTime(objData);

            //var dateFormattedwithtime = moment(objData).format('DD/MM/YYYY HH:mm:ss')
            var dateFormattedwithtime = moment(objData).format('DD MMM YYYY')

            //var d = formattedDate.getDate();
            //var m = formattedDate.getMonth();
            //m += 1;  // JavaScript months are 0-11
            //var y = formattedDate.getFullYear();
            //return d + "/" + m + "/" + y;
            retstring = dateFormattedwithtime;
        }
    }

    callback(retstring);
}
function formatdata(localObjects, valore, prop, callback) {
    var retstring = "";
    var tmpDate = valore[0];
    var controllo = localObjects["resultinfo"][prop[0]];
    if (controllo == "true") {
        if (tmpDate && tmpDate != "") {
            var objData = new Date(tmpDate);

            //var dateFormattedwithtime = getDate(objData) + " " + getTime(objData);

            //var dateFormattedwithtime = moment(objData).format('DD/MM/YYYY HH:mm:ss')
            var dateFormattedwithtime = moment(objData).format('DD/MM/YYYY')

            //var d = formattedDate.getDate();
            //var m = formattedDate.getMonth();
            //m += 1;  // JavaScript months are 0-11
            //var y = formattedDate.getFullYear();
            //return d + "/" + m + "/" + y;
            retstring = dateFormattedwithtime;
        }
    }

    callback(retstring);
}

function formatvalue(localObjects, valore, prop, callback) {

    var retstring = valore[0];

    callback(retstring);
}

//CARICAMENTO DATI --------------------------------------------------------------------------------------
function caricaParametriConfigServer(lng, objfiltro, callback, functiontocallonend) {
    var lng = lng || "I";
    var objfiltro = objfiltro || "";
    var page = page || "";
    var pagesize = pagesize || "";
    var enablepager = enablepager || false;

    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        //async: false,
        data: { 'q': 'caricaConfig', 'objfiltro': JSON.stringify(objfiltro) },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, function () { });
        }
    });
}

function caricaParametriRisorseServer(lng, objfiltro, callback, functiontocallonend) {
    var lng = lng || "I";
    var objfiltro = objfiltro || "";
    var page = page || "";
    var pagesize = pagesize || "";
    var enablepager = enablepager || false;

    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        //async: false,
        data: { 'q': 'caricaresources', 'objfiltro': JSON.stringify(objfiltro), 'lng': lng },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, function () { });
        }
    });
}

function caricaDatiServerArchivio(lng, objfiltro, callback, functiontocallonend) {
    var lng = lng || "I";
    var objfiltro = objfiltro || "";

    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        //async: false,
        data: { 'q': 'caricaDatiArchivio', 'objfiltro': JSON.stringify(objfiltro), 'lng': lng },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, function () { });
        }
    });
}


function caricaDatiServerLinkscategorie(lng, objfiltro, callback, functiontocallonend) {
    var lng = lng || "I";
    var objfiltro = objfiltro || "";

    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        //async: false,
        data: { 'q': 'caricaLinks2liv', 'objfiltro': JSON.stringify(objfiltro), 'lng': lng },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, function () { });
        }
    });
}

function caricaDatiServer(lng, objfiltro, page, pagesize, enablepager, callback, functiontocallonend) {
    var lng = lng || "I";
    var objfiltro = objfiltro || "";
    var page = page || "";
    var pagesize = pagesize || "";
    var enablepager = enablepager || false;

    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        //async: false,
        data: { 'q': 'caricaDati', 'objfiltro': JSON.stringify(objfiltro), 'lng': lng, 'page': page, 'pagesize': pagesize, 'enablepager': enablepager },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, function () { });
        }
    });
}


function caricaDatiServerBanner(lng, objfiltro, page, pagesize, enablepager, callback, functiontocallonend) {
    var lng = lng || "I";
    var objfiltro = objfiltro || "";
    var page = page || "";
    var pagesize = pagesize || "";
    var enablepager = enablepager || false;

    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        //async: false,
        data: { 'q': 'caricaDatiBanner', 'objfiltro': JSON.stringify(objfiltro), 'lng': lng, 'page': page, 'pagesize': pagesize, 'enablepager': enablepager },
        success: function (result) {
            callback(result, functiontocallonend);
        },
        error: function (result) {
            //sendmessage('fail creating link');
            callback(result.responseText, function () { });
        }
    });
}


function putinsession(key, value, callback) {
    //putinsession
    var key = key || "";
    var value = value || "";
    if (key != null && key != '')
        $.ajax({
            url: pathAbs + commonhandlerpath,
            contentType: "application/json; charset=utf-8",
            cache: false,
            data: { 'q': 'putinsession', 'key': key, 'value': value },
            success: function (result) {
                callback(result);
            },
            error: function (result) {
                //sendmessage('fail creating link');
                callback('');
            }
        });
}
function getfromsession(key, callback) {
    //getfromsession
    var key = key || "";
    if (key != null && key != '')
        $.ajax({
            url: pathAbs + commonhandlerpath,
            contentType: "application/json; charset=utf-8",
            cache: false,
            //async: false,
            data: { 'q': 'getfromsession', 'key': key },
            success: function (result) {
                callback(result);
            },
            error: function (result) {
                //sendmessage('fail creating link');
                callback('');
            }
        });
}
function JsSvuotaSession(el) {
    var link = "";
    if (el !== null && el !== undefined) {
        //link = $(el).attr("href");
        //  console.log($(el).attr("href"));
    }
    emptysession(link);
    return false;
}
function emptysession(link, callback) {
    //getfromsession
    var link = link || "";
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        cache: false,
        //async: false,
        data: { 'q': 'emptysession', 'link': link },
        success: function (result) {
            if (callback !== null && callback !== undefined)
                callback(result);
            console.log('CLEAREDSESSION');
            //if (link != '') openLink(link);
            return false;
        },
        error: function (result) {
            //sendmessage('fail creating link');
            if (callback !== null && callback !== undefined)
                callback('');
        }
    });
}

/* ---------- FUNZIONI GESTIONE DATI E BINDING -------------------------------------------------------*/


/*---FUNZIONI UTILITA'----------------------------*/

//METODO 1
var require = function (src, callback) {
    callback = callback || function () { };
    var newScriptTag = document.createElement('script'),
        firstScriptTag = document.getElementsByTagName('script')[0];
    newScriptTag.src = src;
    newScriptTag.async = true;
    newScriptTag.onload = newScriptTag.onreadystatechange = function () {
        (!this.readyState || this.readyState === 'loaded' || this.readyState === 'complete') && (callback());
    };
    firstScriptTag.parentNode.insertBefore(newScriptTag, firstScriptTag);
}


//METODO 2 Caricamento dinamico di file script .js con 
function loadJs(url) {
    return new Promise(resolve => {
        const script = document.createElement("script");
        script.src = url;
        script.onload = resolve;
        document.head.appendChild(script);
    });
}
function loadCss(url) {
    return new Promise(resolve => {
        const script = document.createElement("link");
        script.href = url;
        script.rel = "stylesheet";
        script.type = "text/css";
        script.onload = resolve;
        document.head.appendChild(script);
    });
}

function openLink(link) {
    //console.log('openlink:' + link);
    window.location.href = link;
    return false;
}

function sendmessage(title, text) {
    $.Notification.autoHideNotify('success', 'top right', title, text)
}

function sendmessagefixed(title, text) {
    $.Notification.notify('warning', 'top center', title, text);

}

function isEven(n) {
    return n == parseFloat(n) ? !(n % 2) : void 0;
}

function endsWith(str, word) {
    return str.indexOf(word, str.length - word.length) !== -1;
}
function jsAddSlashes(str) {
    if (str != null) {
        str = JSON.stringify(String(str));
        str = str.substring(1, str.length - 1);
    }
    return str;
}

function getTime(passdata) {

    var tmpData = new Date(passdata) || new Date();
    var date_obj = tmpData;
    var date_obj_time = date_obj;
    try {

        var date_obj_hours = date_obj.format("HH");
        var date_obj_mins = date_obj.format("mm");
        var date_obj_second = date_obj.format("ss");
        date_obj = date_obj_time = date_obj_hours + ":" + date_obj_mins + ":" + date_obj_second;
    }
    catch (e) {

    }
    return date_obj_time;
}
function getDate(passdata) {

    var tmpData = new Date(passdata) || new Date();
    var date_obj = tmpData;
    var date_obj_date = date_obj;
    try {

        var date_obj_day = date_obj.format("dd");
        var date_obj_month = date_obj.format("MM");
        var date_obj_year = date_obj.format("yyyy");
        date_obj_date = date_obj_day + "/" + date_obj_month + "/" + date_obj_year;
    }
    catch (e) {

    } return date_obj_date;
}
function DownloadFile(filetodownload) {
    $.get(
        filetodownload,
        function (data, textStatus, jqXHR) {
            window.location = (filetodownload);
        });
}
$.fn.isOnScreen = function () {
    var win = $(window);
    var viewport = {
        top: win.scrollTop(),
        left: win.scrollLeft()
    };
    viewport.right = viewport.left + win.width();
    viewport.bottom = viewport.top + win.height();
    //console.log(viewport);
    var percdelta = 0.2;
    viewport.top -= viewport.top * percdelta;
    viewport.bottom += viewport.bottom * percdelta;
    viewport.left -= viewport.left * percdelta;
    viewport.right += viewport.right * percdelta;

    var bounds = this.offset();
    bounds.right = bounds.left + this.outerWidth();
    bounds.bottom = bounds.top + this.outerHeight();
    try {
        if (bounds.left == bounds.right && bounds.top == bounds.bottom) {
            bounds = this.parent().offset();
            bounds.right = bounds.left + this.parent().outerWidth();
            bounds.bottom = bounds.top + this.parent().outerHeight();
        }
    } catch (e) { }
    //console.log(bounds);
    return (!(viewport.right < bounds.left || viewport.left > bounds.right || viewport.bottom < bounds.top || viewport.top > bounds.bottom));

};
function registerListener(event, func) {
    if (window.addEventListener) {
        window.addEventListener(event, func)
    } else {
        window.attachEvent('on' + event, func)
    }
}


(function ($) {
    $.extend({
        getQueryString: function (name) {
            function parseParams() {
                var params = {},
                    e,
                    a = /\+/g,  // Regex for replacing addition symbol with a space
                    r = /([^&=]+)=?([^&]*)/g,
                    d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
                    q = window.location.search.substring(1);

                while (e = r.exec(q))
                    params[d(e[1])] = d(e[2]);

                return params;
            }

            if (!this.queryStringParams)
                this.queryStringParams = parseParams();

            return this.queryStringParams[name];
        }
    });
})(jQuery);


function getMeta(url) {
    $("<img/>").attr("src", url).load(function () {
        s = { w: this.width, h: this.height };
        alert(s.w + ' ' + s.h);
    });
}

$.fn.extend({
    // get style attributes that were set on the first element of the jQuery object
    getStyle: function (prop) {
        var elem = this[0];
        var actuallySetStyles = {};
        for (var i = 0; i < elem.style.length; i++) {
            var key = elem.style[i];
            if (prop == key)
                return elem.style[key];
            actuallySetStyles[key] = elem.style[key];
        }
        if (!prop)
            return actuallySetStyles;
    }
})

$.fn.outerHTML = function () {
    var $t = $(this);
    if ("outerHTML" in $t[0]) return $t[0].outerHTML;
    else return $t.clone().wrap('<p>').parent().html();
}

function getCopy(objectToCopy) {
    var copy = {};
    for (var prop in objectToCopy) {
        if (typeof (objectToCopy[prop]) === "object") {
            copy[prop] = getCopy(objectToCopy[prop]);
        }
        else {
            copy[prop] = null;
        }
    }
    return copy;
}


//if (typeof Array.prototype.contains !== 'function') {
//    Array.prototype.contains = function (needle) {
//        var i = null;
//        for (i in this) {
//            if (this[i] == needle) {
//                return true;
//            }
//        }
//        return false;
//    }
//}
//Array.prototype.remByName = function (val) {
//    for (var i = 0; i < this.length; i++) {
//        if (this[i].name === val) {
//            this.splice(i, 1);
//            i--;
//        }
//    }
//    return this;
//}

String.prototype.capitalizeFirstLetter = function () {
    return this.charAt(0).toUpperCase() + this.slice(1).toLowerCase();
}
Number.prototype.pad = function (len) {
    return (new Array(len + 1).join("0") + this).slice(-len);
}
if (typeof String.prototype.trim != 'function') { // detect native implementation
    String.prototype.trim = function () {
        return this.replace(/^\s+/, '').replace(/\s+$/, '');
    };
}
Date.prototype.today = function (separator) {
    var separator = separator || "/";
    return ((this.getDate() < 10) ? "0" : "") + this.getDate() + separator + (((this.getMonth() + 1) < 10) ? "0" : "") + (this.getMonth() + 1) + separator + this.getFullYear();
}
Date.prototype.timeNow = function (separator) {
    var separator = separator || ":";
    return ((this.getHours() < 10) ? "0" : "") + this.getHours() + separator + ((this.getMinutes() < 10) ? "0" : "") + this.getMinutes() + separator + ((this.getSeconds() < 10) ? "0" : "") + this.getSeconds();
}
Number.prototype.formatMoney = function (decPlaces, thouSeparator, decSeparator) {
    var n = this,
        decPlaces = isNaN(decPlaces = Math.abs(decPlaces)) ? 2 : decPlaces,
        decSeparator = decSeparator == undefined ? "." : decSeparator,
        thouSeparator = thouSeparator == undefined ? "," : thouSeparator,
        sign = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(decPlaces)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return sign + (j ? i.substr(0, j) + thouSeparator : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thouSeparator) + (decPlaces ? decSeparator + Math.abs(n - i).toFixed(decPlaces).slice(2) : "");
};
var sortBy = (function () {
    //cached privated objects
    var _toString = Object.prototype.toString,
        //the default parser function
        _parser = function (x) { return x; },
        //gets the item to be sorted
        _getItem = function (x) {
            return this.parser((x !== null && typeof x === "object" && x[this.prop]) || x);
        };
    // Creates a method for sorting the Array
    // @array: the Array of elements
    // @o.prop: property name (if it is an Array of objects)
    // @o.desc: determines whether the sort is descending
    // @o.parser: function to parse the items to expected type
    return function (array, o) {
        if (!(array instanceof Array) || !array.length)
            return [];
        if (_toString.call(o) !== "[object Object]")
            o = {};
        if (typeof o.parser !== "function")
            o.parser = _parser;
        o.desc = !!o.desc ? -1 : 1;
        return array.sort(function (a, b) {
            a = _getItem.call(o, a);
            b = _getItem.call(o, b);
            return o.desc * (a < b ? -1 : +(a > b));
        });
    };

}());
//Fix Function#name on browsers that do not support it (IE):
if (!(function f() { }).name) {
    Object.defineProperty(Function.prototype, 'name', {
        get: function () {
            var name = this.toString().match(/^function\s*([^\s(]+)/)[1];
            // For better performance only parse once, and then cache the
            // result through a new accessor for repeated access.
            Object.defineProperty(this, 'name', { value: name });
            return name;
        }
    });
}


