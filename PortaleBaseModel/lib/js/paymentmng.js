"use strict";

var tmppaymentIntentId = "";
var confirmpaymentdata = {};
var registerdict = {
    q: 'register-complete-order', 'lng': lng, confirmpaymentdata: ''
};
var inputdict = {
    q: 'create-payment-intent', 'lng': lng, contactdatas: ''
};
var inputdictupdate = {
    q: 'update-payment-intent', 'lng': lng, contactdatas: '', 'paymentintentid': ''
};
var inputdictupdateorcreate = {
    q: 'create-and-update-payment-intent', 'lng': lng, contactdatas: '', 'paymentintentid': ''
};
var stripelocal = null;

$(document).ready(function () {
    // A reference to Stripe.js (library must be loaded in page) initialized with a fake API key.
    // Sign in to see examples pre-filled with your key. 
    if (typeof Stripe === "function") {
        console.log('stripe loaded!');
        stripelocal = Stripe(stripe_publishableKey);
        // Disable the button until we have Stripe set up on the page
        document.querySelector("#submit-payment-button").disabled = true;
        loadingstripe(true);
    };
});

// Show a spinner on payment submission
var loadingstripe = function (isloadingstripe) {
    if (isloadingstripe) {
        // Disable the button and show a spinner
        document.querySelector("#submit-payment-button").disabled = true;
        document.querySelector("#spinnerstripe").classList.remove("hidden");
        document.querySelector("#button-text").classList.add("hidden");
    } else {
        document.querySelector("#submit-payment-button").disabled = false;
        document.querySelector("#spinnerstripe").classList.add("hidden");
        document.querySelector("#button-text").classList.remove("hidden");
    }
};

var fetchpaymentintent = function (paymentIntentId, callback) {

    if (stripelocal == null) return;
    //PROCEDURA UNICA CREATE OR UPDATE
    //create or update payment intent
    inputdictupdateorcreate.paymentintentid = paymentIntentId;
    inputdictupdateorcreate.username = username;
    fetch((pathAbs + "/lib/hnd/HandlerPayments.ashx"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(inputdictupdateorcreate)
    })
        .then(function (result) {
            return result.json();
        })
        .then(function (data) {
            var errfromhnd = "";
            if (data.messages != undefined) messagefromhnd = data.messages;
            if (data.clientSecret == undefined) { loadingstripe(true); document.querySelector("#card-error").innerHTML = 'stripe not initialized' + messagefromhnd; document.querySelector("#card-error-pre").innerHTML = 'stripe not initialized' + messagefromhnd; return; }

            if (paymentIntentId == '') { //da fare solo se create payment intent
                var elements = stripelocal.elements();
                var style = {
                    base: {
                        color: "#32325d",
                        fontFamily: 'Arial, sans-serif',
                        fontSmoothing: "antialiased",
                        fontSize: "16px",
                        "::placeholder": {
                            color: "#32325d"
                        }
                    },
                    invalid: {
                        fontFamily: 'Arial, sans-serif',
                        color: "#fa755a",
                        iconColor: "#fa755a"
                    }
                };
                var card = elements.create("card", { style: style });
                // Stripe injects an iframe into the DOM
                card.mount("#card-element");
                card.on("change", function (event) {
                    // Disable the Pay button if there are no card details in the Element
                    // se non ci sono errori per la carta abilito o viceversa disabilito il bottone
                    document.querySelector("#submit-payment-button").disabled = event.empty;
                    document.querySelector("#card-error").innerHTML = event.error ? event.error.message : "";
                });
                //console.log(data.clientSecret);
                var tmpstring = data.clientSecret;
                var position = tmpstring.lastIndexOf('_secret');
                tmppaymentIntentId = tmpstring.substr(0, position); //memorizzo il paymentintetid per aggiornarlo dopo
                var divpayment = document.getElementById("submit-payment-button");
                divpayment.addEventListener("click", function (event) {
                    event.preventDefault();
                    // Complete payment when the submit button is clicked !!!
                    payWithCard(stripelocal, card, data.clientSecret);
                });
            }
            //controllo errori processo ordine .....
            if (data.stoperror != '') {
                var errorMsg = document.querySelector("#card-error");
                var errorMsgpre = document.querySelector("#card-error-pre");
                errorMsgpre.innerHTML = data.messages;
                errorMsg.innerHTML = data.messages;
                setTimeout(function () {
                    errorMsgpre.innerHTML = "";
                    errorMsg.innerHTML = "";
                }, 10000);
                return;
            }

            loadingstripe(false);
            callback(data); //se tutto ok faccio la call back per procedere
        });

    //PROCEDURE SEPARATE PER UPDATE E CREATE
    ////update intent
    //if (paymentIntentId != undefined && paymentIntentId != '') {
    //    inputdictupdate.paymentintentid = paymentIntentId;
    //    inputdictupdate.username = username;
    //    fetch((pathAbs + "/lib/hnd/HandlerPayments.ashx"), {
    //        method: "POST",
    //        headers: { "Content-Type": "application/json" },
    //        body: JSON.stringify(inputdictupdate)
    //    })
    //        .then(function (result) {
    //            return result.json();
    //        })
    //        .then(function (data) {
    //            var errfromhnd = "";
    //            if (data.messages != undefined) messagefromhnd = data.messages;
    //            if (data.clientSecret == undefined) { loadingstripe(true); document.querySelector("#card-error").innerHTML = 'stripe not initialized' + messagefromhnd; document.querySelector("#card-error-pre").innerHTML = 'stripe not initialized' + messagefromhnd; return; }
    //            //controllo errori processo ordine .....
    //            if (data.stoperror != '') {
    //                var errorMsg = document.querySelector("#card-error");
    //                var errorMsgpre = document.querySelector("#card-error-pre");
    //                errorMsgpre.innerHTML = data.messages;
    //                errorMsg.innerHTML = data.messages;
    //                setTimeout(function () {
    //                    errorMsgpre.innerHTML = "";
    //                    errorMsg.innerHTML = "";
    //                }, 10000);
    //                return;
    //            }
    //            loadingstripe(false);
    //            callback(data); //se tutto ok faccio la call back per procedere
    //        });
    //}
    //else { //create intent
    //    fetch((pathAbs + "/lib/hnd/HandlerPayments.ashx"), {
    //        method: "POST",
    //        headers: { "Content-Type": "application/json" },
    //        body: JSON.stringify(inputdict)
    //    })
    //        .then(function (result) {
    //            return result.json();
    //        })
    //        .then(function (data) {
    //            if (data.messages != undefined) messagefromhnd = data.messages;
    //            if (data.clientSecret == undefined) { loadingstripe(true); document.querySelector("#card-error").innerHTML = 'stripe not initialized' + messagefromhnd; document.querySelector("#card-error-pre").innerHTML = 'stripe not initialized' + messagefromhnd; return; }
    //            var elements = stripelocal.elements();
    //            var style = {
    //                base: {
    //                    color: "#32325d",
    //                    fontFamily: 'Arial, sans-serif',
    //                    fontSmoothing: "antialiased",
    //                    fontSize: "16px",
    //                    "::placeholder": {
    //                        color: "#32325d"
    //                    }
    //                },
    //                invalid: {
    //                    fontFamily: 'Arial, sans-serif',
    //                    color: "#fa755a",
    //                    iconColor: "#fa755a"
    //                }
    //            };
    //            var card = elements.create("card", { style: style });
    //            // Stripe injects an iframe into the DOM
    //            card.mount("#card-element");
    //            card.on("change", function (event) {
    //                // Disable the Pay button if there are no card details in the Element
    //                // se non ci sono errori per la carta abilito o viceversa disabilito il bottone
    //                document.querySelector("#submit-payment-button").disabled = event.empty;
    //                document.querySelector("#card-error").innerHTML = event.error ? event.error.message : "";
    //            });
    //            //console.log(data.clientSecret);
    //            var tmpstring = data.clientSecret;
    //            var position = tmpstring.lastIndexOf('_secret');
    //            tmppaymentIntentId = tmpstring.substr(0, position); //memorizzo il paymentintetid per aggiornarlo dopo

    //            var divpayment = document.getElementById("submit-payment-button");
    //            divpayment.addEventListener("click", function (event) {
    //                event.preventDefault();
    //                // Complete payment when the submit button is clicked !!!
    //                payWithCard(stripelocal, card, data.clientSecret);
    //            });
    //            loadingstripe(false);
    //        });
    //}
}
//chiamo al caricamento subito il fetchintent si crea subito un payment intent al page load!!!
//fetchpaymentintent();

/////////////////////////////////////////////////////////////////////
//Procedeura esecuzione ordine con aggiornamenot contestuale di paymetintnt 
/////////////////////////////////////////////////////////////////////
function executeorder(btnorder) {

    //dovrei aggiornare i dati   totali, prodotti e clienti del carrello e metterli in sessione
    //se faccio un pastback il payement intent id è rigenerato exnovo !!! non lo posso fare
    //__doPostBack('convalidaordinestripe', args); // da modificare devo inserire quello che viene fatto qui in un apposito handler che aggiorna la sessione !!!
    $(btnorder).attr("disabled", "")
    var tastotxt = $(btnorder).html();
    $(btnorder).html("Wait ..");

    var contactdatas = {};
    getcontactdataformorder(contactdatas, function (contactdatas) {
        if (contactdatas.validated == true) { //prima controllo la validazione dei dati ...
            //  console.log("Validation output : " + contactdatas.messages);  
            //passo i dati del form inseriti dall'utente per la chiamata al server
            //inputdictupdate.contactdatas = JSON.stringify(contactdatas); // solo update
            inputdictupdateorcreate.contactdatas = JSON.stringify(contactdatas); //per create and update
            // variabile  tmppaymentIntentId contien l'id del paymentintet da aggionare ( viene generato all'apertura della pagina ordine )
            fetchpaymentintent(tmppaymentIntentId, function (data) {
                //setto il codice ordine ed il nome utente che viene usato dalla procedura di pagamento finale
                confirmpaymentdata["codiceordine"] = data.codiceordine;
                confirmpaymentdata["nome"] = data.nome;

                $(btnorder).removeAttr("disabled")
                $(btnorder).html(tastotxt);

                //richiediamo di eseguire il pagamento con la carta tramite una modal box che apro adesso
                showModalStripe();  // se tutto ok per il pagamento viene chiamata la funzione orderComplete dove l'utente esegue la transazione di pagamento tramite modal box
            });
        }
        else {
            $(btnorder).removeAttr("disabled")
            $(btnorder).html(tastotxt);

            console.log('not  validated');
            var messaggioerrore = contactdatas.messages + ' ';
            //visualizziamo il messagio del validatorie
            var errorMsg = document.querySelector("#card-error");
            var errorMsgpre = document.querySelector("#card-error-pre");
            errorMsgpre.innerHTML = messaggioerrore;
            errorMsg.innerHTML = messaggioerrore;
            setTimeout(function () {
                errorMsgpre.innerHTML = "";
                errorMsg.innerHTML = "";
            }, 10000);
            return false;
        }
    });

}
/////////////////////////////////////////////////////////////////////
//Raccolta dati da form e validazione !!!
/////////////////////////////////////////////////////////////////////
function getcontactdataformorder(contactdatas, callback) {

    var contactdatas = contactdatas || {};
    contactdatas.username = username; //memorizzo l'username loggato
    var validated = true;
    contactdatas.validated = validated;
    contactdatas.messages = ''; //variabile per output errori da riempire per la validazione

    //SPEDIZIONE
    if ($("[id$='chkSpedizione']").length != 0)
        contactdatas.chkSpedizione = $("[id$='chkSpedizione']")[0].checked; //chkSpedizione
    if ($("[id$='ddlNazione']").length != 0)
        contactdatas.naziones = $("[id$='ddlNazione']")[0].value;
    if ($("[id$='ddlNazioneS']").length != 0)
        contactdatas.naziones = $("[id$='ddlNazioneS']")[0].value;
    contactdatas.nomes = $("[id$='inpNomeS']").val();
    contactdatas.cognomes = $("[id$='inpCognomeS']").val();
    contactdatas.caps = $("[id$='inpCaps']").val();
    contactdatas.comunes = $("[id$='inpComuneS']").val();
    contactdatas.provincias = $("[id$='inpProvinciaS']").val();
    contactdatas.indirizzos = $("[id$='inpIndirizzoS']").val();
    contactdatas.telefonos = $("[id$='inpTelS']").val();

    //DATI PRIMARI
    contactdatas.Cognome = $("[id$='inpCognome']").val();
    contactdatas.Nome = $("[id$='inpNome']").val();
    contactdatas.Ragsoc = $("[id$='inpRagsoc']").val();
    contactdatas.Email = $("[id$='inpEmail']").val();
    contactdatas.Emailpec = $("[id$='inpPec']").val();
    contactdatas.Pivacf = $("[id$='inpPiva']").val();
    contactdatas.Indirizzo = $("[id$='inpIndirizzo']").val();
    if ($("[id$='ddlNazione']").length != 0)
        contactdatas.CodiceNAZIONE = $("[id$='ddlNazione']")[0].value;
    contactdatas.CodiceCOMUNE = $("[id$='inpComune']").val();
    contactdatas.CodicePROVINCIA = $("[id$='inpProvincia']").val();
    contactdatas.Cap = $("[id$='inpCap']").val();
    contactdatas.Telefono = $("[id$='inpTel']").val();
    if ($("[id$='chkcondizioni']").length != 0)
        contactdatas.chkcondizioni = $("[id$='chkcondizioni']")[0].checked; //chkcondizioni

    //altre selezioni 
    contactdatas.Note = $("[id$='inpNote']").val();
    contactdatas.supplementoisole = $("[id$='chkSupplemento']")[0].checked;

    //Modalità di pagamenti
    contactdatas.supplementocontrassegno = false;
    contactdatas.Modalita = "";
    $('input:radio').each(function () {
        if ($(this).is(':checked')) {
            // You have a checked radio button here...
            contactdatas.Modalita = $(this).val();
            if ($(this).val() == 'contanti') contactdatas.supplementocontrassegno = true;

        }
    });

    //validazione dati !!!!
    if (contactdatas.Nome == '') { validated = false; contactdatas.messages += contactdatasgeneral.msgnome; $("[id$='inpNome']").css("borderColor", "red"); } else { $("[id$='inpNome']").css("borderColor", "white"); };
    if (contactdatas.Cognome == '') { validated = false; contactdatas.messages += contactdatasgeneral.msgcognome; $("[id$='inpCognome']").css("borderColor", "red"); } else { $("[id$='inpCognome']").css("borderColor", "white"); };
    if (contactdatas.Email == '') { validated = false; contactdatas.messages += contactdatasgeneral.msgemail; $("[id$='inpEmail']").css("borderColor", "red"); } else { $("[id$='inpEmail']").css("borderColor", "white"); };
    if (contactdatas.Telefono == '') { validated = false; contactdatas.messages += contactdatasgeneral.msgtel; $("[id$='inpTel']").css("borderColor", "red"); } else { $("[id$='inpTel']").css("borderColor", "white"); };
    if (contactdatas.Indirizzo == '') { validated = false; contactdatas.messages += contactdatasgeneral.msgindirizzo; $("[id$='inpIndirizzo']").css("borderColor", "red"); } else { $("[id$='inpIndirizzo']").css("borderColor", "white"); };
    if (contactdatas.Cap == '') { validated = false; contactdatas.messages += contactdatasgeneral.masgcap; $("[id$='inpCap']").css("borderColor", "red"); } else { $("[id$='inpCap']").css("borderColor", "white"); };
    if (contactdatas.CodicePROVINCIA == '') { validated = false; contactdatas.messages += contactdatasgeneral.msgprovincia; $("[id$='inpProvincia']").css("borderColor", "red"); } else { $("[id$='inpProvincia']").css("borderColor", "white"); };
    if (contactdatas.CodiceCOMUNE == '') { validated = false; contactdatas.messages += contactdatasgeneral.msgcomune; $("[id$='inpComune']").css("borderColor", "red"); } else { $("[id$='inpComune']").css("borderColor", "white"); };
    if (contactdatas.CodiceNAZIONE == '') { validated = false; $("[id$='ddlNazione']").css("outline", "2px solid red"); } else { $("[id$='ddlNazione']").css("outline", "1px solid white"); };
  /*  if (contactdatas.chkcondizioni == false) { validated = false; contactdatas.messages += contactdatasgeneral.msgcondizioni + '<br />'; $("[id$='chkcondizioni']").css("outline", "2px solid red"); } else { $("[id$='chkcondizioni']").css("outline", "1px solid white"); };*/

    // ( validazione dati sul form abilitare per versione online )
    if (!validated) { contactdatas.validated = false; contactdatas.messages = 'Compilare correttamente i dati nel modulo di ordine, per procedere!<br/><br/>' + contactdatas.messages } //abilitare per la validazione lato client !!

    callback(contactdatas);
}

// Calls stripelocal.confirmCardPayment
// If the card requires authentication Stripe shows a pop-up modal to
// prompt the user to enter authentication details without leaving your page.
var payWithCard = function (stripelocal, card, clientSecret) {
    if (stripelocal == null) return;

    loadingstripe(true);
    stripelocal
        .confirmCardPayment(clientSecret, {
            //receipt_email: document.getElementById('email').value,
            payment_method: {
                card: card,
                billing_details: {
                    name: confirmpaymentdata["nome"] + ' ' + confirmpaymentdata["codiceordine"]  // questo è visualizzato nello storico transazione stripe
                }
            }
        })
        .then(function (result) {
            if (result.error) {
                // Show error to your customer
                showError(result.error.message);
            } else {
                // The payment succeeded!
                orderComplete(result.paymentIntent.id);
            }
        });
};

/* ------- UI helpers ------- */
function registerOrderComplete(callback) {
    registerdict.confirmpaymentdata = JSON.stringify(confirmpaymentdata);
    fetch((pathAbs + "/lib/hnd/HandlerPayments.ashx"), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(registerdict)
    })
        //.then(function (result) {
        //    callback(result);
        //})
        .then(function (result) {
            return result.json();
        })
        .then(function (data) {
            callback(data);
        })
};
// Shows a success message when the payment is complete
var orderComplete = function (paymentIntentId) {
    loadingstripe(true);
    /////////////////////////////////////////////////////////////////////////
    // fatto il pagamento con carta!! chiamiamo la procedura meorizza ordine nel db e redirect a thankyuopage - register-complete-order ( handler )
    /////////////////////////////////////////////////////////////////////////
    // aggiornare il database (analogo attivirtà pagina ordineok per paypal) memorizzare ordine, codice ordine su carrello e satististiche, svuotare la session e visualizzare la pagina di successo (thankyou)
    registerOrderComplete(function (data) {
        loadingstripe(false);
        if (data.stoperror == "true") { loadingstripe(false); document.querySelector("#card-error").innerHTML = 'errore registrazione ordine. ' + data.messages; document.querySelector("#card-error-pre").innerHTML = 'errore registrazione ordine.' + data.messages; return; }

        //inserisco in pagina il testo e codice di risposta
        //var cardresponse = document.querySelector("#card-completecode");
        //cardresponse.innerHTML = data.messages; //qui viene iniettato in pagina il codice di successo e i codici per google !!!!

        //redirect per  a thankyou page !
        location.assign(data["SuccessUrl"]);

        //redirect dopo un tempo
        //setTimeout(function () { 
        //    location.assign(data["SuccessUrl"]);
        //}, 10000);

        //spengere non serve quello sotto 
        //document
        //    .querySelector(".result-message a")
        //    .setAttribute(
        //        "href",
        //        "https://dashboard.stripe.com/test/payments/" + paymentIntentId
        //    );
        //document.querySelector(".result-message").classList.remove("hidden");
        document.querySelector("#submit-payment-button").disabled = true;
    });

};

// Show the customer the error from Stripe if their card fails to charge
var showError = function (errorMsgText) {
    loadingstripe(false);
    var errorMsg = document.querySelector("#card-error");
    var errorMsgpre = document.querySelector("#card-error-pre");
    errorMsg.innerHTML = errorMsgText;
    errorMsgpre.innerHTML = errorMsgText;
    setTimeout(function () {
        errorMsg.innerHTML = "";
        errorMsgpre.innerHTML = "";
    }, 10000);
};



