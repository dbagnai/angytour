
//precompilatest("btniscrivi");
//function precompilatest(btnid, callback) {
//    $("[id$='ragsoc" + btnid + "']").val('web mouse consulting srl');
//    $("[id$='nome" + btnid + "']").val('daniele bagnai');
//    $("[id$='piva" + btnid + "']").val('111111');
//    $("[id$='sdi" + btnid + "']").val('222222');
//    $("[id$='email" + btnid + "']").val('dbagnai@gmail.com');
//    $("[id$='telefono" + btnid + "']").val('333333');
//    $("[id$='cap" + btnid + "']").val('47891');
//    $("[id$='indirizzo" + btnid + "']").val('strada rovereta, 42');
//    $("[id$='caps" + btnid + "']").val('06062');
//    $("[id$='indirizzos" + btnid + "']").val('str.comunale di caioncola, 12');
//    $("[id$='descrizione" + btnid + "']").val('prova invio richiesta.');
//    $("[id$='orario" + btnid + "']").val('8/24');
//    $("[id$='chiusura" + btnid + "']").val('lunedì');
//    $("[id$='qcoffe" + btnid + "']").val('400');
//}

function ConfirmValidationFormGeneral(btnid, serveroperation) {
    var serveroperation = serveroperation || '';


    var out1 = document.getElementById("divoutput" + btnid.id);

    //($("#aspnetForm").valid()); //Attivo il controllo per la validazione
    //if (!localmessagegeneral.inputisvalid) {
    //    out1.innerHTML = localmessagegeneral.validatemsg;
    //    return;
    //}
    var chk1 = document.getElementById("chkprivacy" + btnid.id);
    if (!chk1.checked) {
        out1.innerHTML = localmessagegeneral.privacy;
        return false;
    } else {
        out1.innerHTML = '';
    }

    ////////////////////////////////////////////////////////////////
    /////ABILITARE PER CONTROLLO CAPTCHA
    ////////////////////////////////////////////////////////////////
    var response = grecaptcha.getResponse();
    if (response.length == 0)  //reCaptcha not verified
    {
        out1.innerHTML = localmessagegeneral.captcha;
        return false;
    }
    //else {
    if (true) { //fare controllo di validazione ...
        /*do work and go for postback*/
        $(btnid).attr("disabled", "");
        //invio nopostback con handler////////////////////////////////////////////////////
        var contactdatas = {};
        contactdatas.chkprivacy = chk1.checked;
        contactdatas.lingua = lng;
        var tastotxt = $(btnid).html();
        getcontactdataformgeneral(serveroperation, contactdatas, btnid, function (contactdatas) {
            if (contactdatas != null && serveroperation != '') {

                if (serveroperation == "inseriscianagraficaenotifica") {
                    /////////////////////////////////////////////////////////////
                    //Inserimento in anagrafica clienti ed invio mail al sito
                    /////////////////////////////////////////////////////////////
                    $(btnid).html("Wait ..");
                    inseriscianagraficaenotifica(lng, contactdatas, function (result) {
                        if (result == "") {
                            out1.innerHTML = (localmessagegeneral.successmsg);
                            $("#form" + btnid.id).hide();
                        }
                        else
                            out1.innerHTML = (result);
                        $(btnid).removeAttr("disabled");
                        $(btnid).html(tastotxt);
                    }, tastotxt);

                }
                else if (serveroperation == "inviamessaggiomail") {
                    /////////////////////////////////////////////////////
                    //Invio solo messaggio mail tramite procedura inviamessaggiomail ( da testare )
                    /////////////////////////////////////////////////
                    $(btnid).html("Wait ..");
                    inviamessaggiomail(lng, contactdatas, function (result) {
                        if (result == "") {
                            out1.innerHTML = (result);
                        }
                        else
                            out1.innerHTML = (result);
                        $(btnid).removeAttr("disabled");
                        $(btnid).html(tastotxt);
                    }, tastotxt);

                }
                else if (serveroperation == "preparedataandpostgeneral") {
                    /////////////////////////////////////////////////////////////
                    //(ALTERNATIVA)inserimento post attività e foto dei dati del form
                    /////////////////////////////////////////////////////////////
                    preparedataandpostgeneral(contactdatas, btnid); //inserisce il pot e notifica il webmaster
                    $(btnid).html("Wait ..");
                    inviamessaggiomail(lng, contactdatas, function (result) {
                        if (result) {
                            out1.innerHTML = (result);
                            $(btnid).removeAttr("disabled");
                            $(btnid).html(tastotxt);
                        }
                    }, tastotxt);
                }


            } else {
                $(btnid).removeAttr("disabled");
                $(btnid).html(tastotxt);
                out1.innerHTML = localmessagegeneral.validation;
            }
        }, $(btnid));
    } else {
        console.log('not  validated');
        return false;
    }
    //}
}

function getcontactdataformgeneral(serveroperation, contactdatas, btnid, callback) {
    var contactdatas = contactdatas || {};
    contactdatas.idofferta = idofferta;

    contactdatas.cognome = $("[id$='cognome" + (btnid).id + "']").val();
    contactdatas.nome = $("[id$='nome" + (btnid).id + "']").val();
    contactdatas.ragsoc = $("[id$='ragsoc" + (btnid).id + "']").val();
    contactdatas.piva = $("[id$='piva" + (btnid).id + "']").val();
    contactdatas.sdi = $("[id$='sdi" + (btnid).id + "']").val();
    contactdatas.email = $("[id$='email" + (btnid).id + "']").val();
    contactdatas.telefono = $("[id$='telefono" + (btnid).id + "']").val();
    contactdatas.cap = $("[id$='cap" + (btnid).id + "']").val();
    contactdatas.indirizzo = $("[id$='indirizzo" + (btnid).id + "']").val();
    contactdatas.caps = $("[id$='caps" + (btnid).id + "']").val();
    contactdatas.indirizzos = $("[id$='indirizzos" + (btnid).id + "']").val();
    contactdatas.descrizione = $("[id$='descrizione" + (btnid).id + "']").val();
    contactdatas.orario = $("[id$='orario" + (btnid).id + "']").val();
    contactdatas.chiusura = $("[id$='chiusura" + (btnid).id + "']").val();
    contactdatas.qcoffe = $("[id$='qcoffe" + (btnid).id + "']").val();
    if ($("[id$='chkpveloci" + (btnid).id + "']").length != 0)
        contactdatas.chkpveloci = $("[id$='chkpveloci" + (btnid).id + "']")[0].checked;
    if ($("[id$='nazione" + (btnid).id + "']").length != 0)
        contactdatas.nazione = $("[id$='nazione" + (btnid).id + "']")[0].value;
    if ($("[id$='regione" + (btnid).id + "']").length != 0)
        contactdatas.regione = $("[id$='regione" + (btnid).id + "']")[0].value;
    if ($("[id$='provincia" + (btnid).id + "']").length != 0)
        contactdatas.provincia = $("[id$='provincia" + (btnid).id + "']")[0].value;
    if ($("[id$='comune" + (btnid).id + "']").length != 0)
        contactdatas.comune = $("[id$='comune" + (btnid).id + "']")[0].value;
    if ($("[id$='naziones" + (btnid).id + "']").length != 0)
        contactdatas.naziones = $("[id$='naziones" + (btnid).id + "']")[0].value;
    if ($("[id$='regiones" + (btnid).id + "']").length != 0)
        contactdatas.regiones = $("[id$='regiones" + (btnid).id + "']")[0].value;
    if ($("[id$='provincias" + (btnid).id + "']").length != 0)
        contactdatas.provincias = $("[id$='provincias" + (btnid).id + "']")[0].value;
    if ($("[id$='comunes" + (btnid).id + "']").length != 0)
        contactdatas.comunes = $("[id$='comunes" + (btnid).id + "']")[0].value;

    if ($("[id$='chkprivacy" + (btnid).id + "']").length != 0)
        contactdatas.consenso = $("[id$='chkprivacy" + (btnid).id + "']")[0].checked;
    if ($("[id$='chkprivacy1" + (btnid).id + "']").length != 0)
        contactdatas.consenso1 = $("[id$='chkprivacy1" + (btnid).id + "']")[0].checked;
    if ($("[id$='chkprivacy2" + (btnid).id + "']").length != 0)
        contactdatas.consenso2 = $("[id$='chkprivacy2" + (btnid).id + "']")[0].checked;

    contactdatas.tipologia = localmessagegeneral.tipologia;
    contactdatas.generautente = localmessagegeneral.generautente;
    contactdatas.lingua = lng;
    contactdatas.tipocontenuto = "iscrizione"; //campionatura o altro che voglio specificare
    if (localmessagegeneral.tipocontenuto != '') contactdatas.tipocontenuto = localmessagegeneral.tipocontenuto;
    contactdatas.tipo = "post";

    var validated = true;

    if (serveroperation == "inseriscianagraficaenotifica") {

        //Validazione per campionature
        if (contactdatas.email == '') { validated = false; $("[id$='email" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='email" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.telefono == '') { validated = false; $("[id$='telefono" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='telefono" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.indirizzo == '') { validated = false; $("[id$='indirizzo" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='indirizzo" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.ragsoc == '') { validated = false;; $("[id$='ragsoc" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='ragsoc" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.nome == '') { validated = false; $("[id$='nome" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='nome" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.cognome == '') { validated = false; $("[id$='cognome" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='cognome" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.piva == '') { validated = false; $("[id$='piva" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='piva" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.sdi == '') { validated = false; $("[id$='sdi" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='sdi" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.cap == '') { validated = false; $("[id$='cap" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='cap" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.regione == '') { validated = false; $("[id$='regione" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='regione" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.provincia == '') { validated = false; $("[id$='provincia" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='provincia" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.comune == '') { validated = false; $("[id$='comune" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='comune" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.qcoffe == '') { validated = false; $("[id$='qcoffe" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='qcoffe" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.orario == '') { validated = false; $("[id$='orario" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='orario" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.chiusura == '') { validated = false; $("[id$='chiusura" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='chiusura" + (btnid).id + "']").css("borderColor", "white"); };

    }
    else if (serveroperation == "inviamessaggiomail") {
        //Specifici per messaggio richiesta via mail
        contactdatas.name = $("[id$='nome" + (btnid).id + "']").val(); 
        contactdatas.cognome = $("[id$='cognome" + (btnid).id + "']").val(); 
        contactdatas.message = $("[id$='descrizione" + (btnid).id + "']").val(); 
        contactdatas.tipo = "informazioni";
        contactdatas.tipocontenuto = "informazioni"; // prenota o altro che voglio specificare

        //validazione per iscrizione scheda
        if (contactdatas.name == '') { validated = false; $("[id$='nome" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='nome" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.email == '') { validated = false; $("[id$='email" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='email" + (btnid).id + "']").css("borderColor", "white"); };
        //if (contactdatas.telefono == '') { validated = false; $("[id$='telefono" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='telefono" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.message == '') { validated = false; $("[id$='descrizione" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='descrizione" + (btnid).id + "']").css("borderColor", "white"); };



    }
    else if (serveroperation == "preparedataandpostgeneral") {

        //specifici per inserimento di un post
        contactdatas.titolo = $("[id$='nome" + (btnid).id + "']").val();
        contactdatas.sottotitolo = $("[id$='tipologia" + (btnid).id + "']").val();
        contactdatas.prezzo = $("[id$='prezzo" + (btnid).id + "']").val();
        contactdatas.tipocontenuto = "iscrizione"; //campionatura o altro che voglio specificare


        //validazione per iscrizione scheda
        if (contactdatas.email == '') { validated = false; $("[id$='email" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='email" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.telefono == '') { validated = false; $("[id$='telefono" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='telefono" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.indirizzo == '') { validated = false; $("[id$='indirizzo" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='indirizzo" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.titolo == '') { validated = false; $("[id$='nome" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='nome" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.sottotitolo == '') { validated = false; $("[id$='tipologia" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='tipologia" + (btnid).id + "']").css("borderColor", "white"); };
        if (contactdatas.descrizione == '') { validated = false; $("[id$='descrizione" + (btnid).id + "']").css("borderColor", "red"); } else { $("[id$='descrizione" + (btnid).id + "']").css("borderColor", "white"); };
    }

    if (!validated) contactdatas = null;

    callback(contactdatas);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
//FUNZIONI CHE USANO L'HANDLER DEI FILE PER CARICARE I FILES ED INSERIRE UN POST NELLA TIPOLOGIA PASSATA
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function preparedataandpostgeneral(contactdatas, btnid) {
    var data = new FormData();
    data.append('q', 'insertpost');
    data.append('formdata', JSON.stringify(contactdatas));
    var fileUpload = document.getElementById("controluploaddoc" + btnid.id);
    //var files = fileUpload.files;
    var files = fileUpload.files;
    for (var i = 0; i < files.length; i++) {
        (files[i].name);
        data.append(files[i].name, files[i]);
    }
    var tastotxt = $(btnid).html();
    $(btnid).html("ATTENDI ..");
    insertPostWithFilesGeneral(lng, data, function (result) {
        //Meaaggio finale e svuotamento del FORM!!!
        var out1 = document.getElementById("divoutput" + btnid.id);
        if (result == "") {
            out1.innerHTML = (localmessagegeneral.successmsg);
            $("#form" + btnid.id).hide();
        }
        else
            out1.innerHTML = (result);

        $(btnid).removeAttr("disabled");
        $(btnid).html(tastotxt);
    }, tastotxt);
}
function insertPostWithFilesGeneral(lng, data, callback) {
    var lng = lng || "I";
    var data = data || {};

    $.ajax({
        url: pathAbs + "/lib/hnd/filehnd.ashx",
        global: false,
        contentType: false,
        processData: false,
        cache: false,
        type: "POST",
        //async: false,
        data: data,
        success: function (result) {
            if (callback)
                callback(result);
            //location.replace(result);//reindirizzo alla destinazione indicata dall'handler
        },
        error: function (result) {
            if (callback)
                callback(result.responseText);
        }
    });
}

//$("#lnkAttach").click(function () {
//    $("#uploaddoc").click();
//});
function UploadFilesGeneral(btnupload) {
    var out1 = document.getElementById("outputbtn" + btnupload.id);
    //////////////////////////////////////////////////////////////
    /////ABILITARE PER CONTROLLO CAPTCHA
    /////////////////////////////////////////////////////////
    //var response = grecaptcha.getResponse();
    //if (response.length == 0)  //reCaptcha not verified
    //{
    //    out1.innerHTML = localmessagegeneral.captcha;
    //    return false;
    //}
    //else
    {
        $('#' + 'control' + btnupload.id).click();
    }
}
function fileselectedgeneral(uplcontrol) {

    var startbtn = document.getElementById(uplcontrol.id.replace('control', ''));//////
    var out1 = document.getElementById("outputbtn" + uplcontrol.id.replace('control', ''));
    $(startbtn).attr("onclick", "");
    out1.innerHTML = "";
    console.log('changed selected files');
    for (var i = 0; i < uplcontrol.files.length; i++) {
        console.log(uplcontrol.files[i].name);
        out1.innerHTML += (uplcontrol.files[i].name);
    }
    //Precarico i files in una directory temporanea nel server!!!
    uploadfileintmpdirgeneral(uplcontrol, startbtn, out1);
}

function uploadfileintmpdirgeneral(uplcontrol, startbtn, out1) {
    var data = new FormData();
    var fileUpload = uplcontrol;
    var files = fileUpload.files;
    for (var i = 0; i < files.length; i++) {
        (files[i].name);
        data.append(files[i].name, files[i]);
    }
    data.append('q', 'uploadfileinfolder');
    var tastotxt = $(startbtn).html();
    $(startbtn).html("ATTENDI ..");
    $.ajax({
        url: pathAbs + "/lib/hnd/filehnd.ashx",
        global: false,
        contentType: false,
        processData: false,
        cache: false,
        type: "POST",
        //async: false,
        data: data,
        success: function (result) {
            var valoriritorno = JSON.parse(result);
            var stringmsg = "";
            if (valoriritorno.hasOwnProperty("messages")) {
                stringmsg = valoriritorno["messages"].join();
            }
            var filesimages = "";
            if (valoriritorno.hasOwnProperty("files")) {
                for (var i = 0; i < valoriritorno["files"].length; i++) {
                    filesimages += "<a style=\"cursor:cell\" link=\"" + valoriritorno["files"][i] + "\" onclick=\"imgdeletegeneral(this,'" + out1.id + "')\" ><img class=\"rounded img-thumbnail w-25\" src=\"" + valoriritorno["files"][i] + "\" ></a>";
                }
            }
            out1.innerHTML = filesimages + '<br/>' + stringmsg;
            //location.replace(result);//reindirizzo alla destinazione indicata dall'handler
            $(uplcontrol).val("");//Svuoto i files già caricati
            $(startbtn).attr("onclick", "UploadFilesGeneral(this)");
            $(startbtn).html(tastotxt);
        },
        error: function (result) {
            out1.innerHTML = result.responseText;
        }
    }, tastotxt);
}
function imgdeletegeneral(file, out1) {
    out1 = document.getElementById(out1);;
    var deleeteconfirm = confirm('Vuoi eliminare il file?');
    if (deleeteconfirm)
        $.ajax({
            url: pathAbs + "/lib/hnd/filehnd.ashx",
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'deletefilebypath', 'data': $(file).attr("link") },
            success: function (result) {
                //if (callback)
                //    callback(result);
                var valoriritorno = JSON.parse(result);
                var stringmsg = "";
                if (valoriritorno.hasOwnProperty("messages")) {
                    stringmsg = valoriritorno["messages"].join();
                }
                var filesimages = "";
                if (valoriritorno.hasOwnProperty("files")) {

                    for (var i = 0; i < valoriritorno["files"].length; i++) {
                        filesimages += "<a style=\"cursor:cell\" link=\"" + valoriritorno["files"][i] + "\" onclick=\"imgdeletegeneral(this,'" + out1.id + "')\" ><img class=\"rounded img-thumbnail w-25\" src=\"" + valoriritorno["files"][i] + "\" ></a>";
                    }
                }
                out1.innerHTML = filesimages + '<br/>' + stringmsg;
            },
            error: function (result) {
                console.log(result.responseText);
            }
        });
}

    //$(document).ready(function () {
    //    validateinitgeneral();
    //});
    //function validateinitgeneral() {
    //    $("#aspnetForm").validate({
    //        ignore: [], //Abilita il controllo dei campi Hidden!
    //        rules: {
    //            descrizionebtniscrivi: {
    //                required: true,
    //                minlength: 250,
    //                noSpacesonlytext: true
    //            }
    //        },
    //        messages: {
    //            descrizionebtniscrivi: {
    //                required: "Inserisci la descrizione del ristorante",
    //                minlength: "Nome almeno 250 caratteri"
    //            }
    //        }, showErrors: function (errorMap, errorList) {
    //            if (this.numberOfInvalids() == 0) {
    //                localmessagegeneral.inputisvalid = true;
    //                localmessagegeneral.validatemsg = "";
    //            }
    //            else {
    //                localmessagegeneral.inputisvalid = false;
    //                localmessagegeneral.validatemsg = "Impossibile inviare. Numero di errori relevati: " + this.numberOfInvalids();
    //                if (errorList.length > 0) {
    //                    for (var x = 0; x < errorList.length; x++) {
    //                        localmessagegeneral.validatemsg += "<br/>\n\u25CF " + errorList[x].message;
    //                    }
    //                }
    //            }
    //            this.defaultShowErrors();
    //        }
    //    });
    //}
