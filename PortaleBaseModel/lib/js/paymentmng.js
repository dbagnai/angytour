"use strict";

////////////////////// START STRIPE PAYMENT /////////////////////////////
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

var fetchpaymentintent = function (paymentIntentId, callback, errorbck) {

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
            var messagefromhnd = "";
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
                errorbck(data);
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
            }, function (data) {

                $(btnorder).removeAttr("disabled")
                $(btnorder).html(tastotxt);
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
    contactdatas.richiedifattura = $("[id$='chkRichiedifattura']")[0].checked;

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

///////////////////////END STRIPE PAYMENT /////////////////////////////

///////////////////////START paypal payment /////////////////////////////

//Opzione 1 inserimento sdk paypal con chiamata script diretta sincrone
function loadScriptPaypal() {
    //https://developer.paypal.com/sdk/js/configuration/
    //Le Varibili sotto sono iniettate in pagina da masterpage codebehind
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = 'https://www.paypal.com/sdk/js?client-id=' + paypal_clientid +
        '&components=' + paypal_components +
        '&enable-funding=' + paypal_enablefunding +
        '&disable-funding=' + paypal_disablefunding +
        '&intent=' + paypal_intent +
        '&currency=EUR';
    document.body.appendChild(script);
}
//Carico script di paypal con chiamata sincrona diretta ( verificando la presenza del contenitore )
if (document.getElementById("paypal-button-container") != null)
    loadScriptPaypal();

//in alternativa da vedere chiamata asincrona ...   https://www.npmjs.com/package/@paypal/paypal-js
//... da vedere evempio se serve
//carico script paypa con chiamata asincrona ( ma  devi installare il pacchetto npm install @paypal/paypal-js ))
//  if (typeof loadScript === "function") {
//}

let cardField = null;
let paypal_buttons = null;
var paypalfinalizedatas = {};
var paypalregisterorder = {
    q: 'paypal-register-complete-order', 'lng': lng, paypalfinalizedatas: ''
};
var paypalcollecteddatas = {};
var paypalcreateorder = {
    q: 'paypal-create-order', 'lng': lng, contactdatas: ''
};
var paypalcancelorder = {
    q: 'paypal-cancel-order', 'lng': lng, contactdatas: ''
};
(function wait() {
    if (typeof window.paypal === "object") {
        initpaypalelements();
    } else {
        setTimeout(wait, 50);
    }
})();


function initpaypalelements() {

    paypal_buttons = window.paypal
        .Buttons({
            style: {
                layout: 'vertical',
                color: 'black',
                shape: 'rect',
                label: 'paypal',
                tagline: false
            },
            createOrder: createOrderCallback,
            onApprove: onApproveCallback,
            onCancel: onCancelCallback,
            //onError: onErrorCallback
        });
    paypal_buttons.render("#paypal-button-container");


    cardField = window.paypal.CardFields({
        createOrder: createOrderCallback,
        onApprove: onApproveCallback,
        onCancel: onCancelCallback,
        //onError: onErrorCallback
    });
    // Render each field after checking for eligibility
    if (cardField.isEligible()) {
        const nameField = cardField.NameField();
        nameField.render("#card-name-field-container");

        const numberField = cardField.NumberField();
        numberField.render("#card-number-field-container");

        const cvvField = cardField.CVVField();
        cvvField.render("#card-cvv-field-container");

        const expiryField = cardField.ExpiryField();
        expiryField.render("#card-expiry-field-container");

        // Add click listener to submit button and call the submit function on the CardField component
        document
            .getElementById("multi-card-field-button")
            .addEventListener("click", () => {
                cardField.submit().catch((error) => {
                    resultMessage(
                        `Sorry, your transaction could not be processed...<br><br>${error}`,
                    );
                });
            });
    } else {
        // Hides card fields if the merchant isn't eligible
        document.querySelector("#card-form").style = "display: none";
    }


}

//Procedura (NON USATA) da completare  !!!
//nel caso di acquisto con campi card custom per paypal
function executeorderpaypal(btnorder) {

    $(btnorder).attr("disabled", "")
    var tastotxt = $(btnorder).html();
    $(btnorder).html("Wait ..");
    paypalcollecteddatas = {};
    getcontactdataformorder(paypalcollecteddatas, function (paypalcollecteddatas) {

        if (paypalcollecteddatas.validated == true) { //prima controllo la validazione dei dati ...
            paypalcreateorder.contactdatas = JSON.stringify(paypalcollecteddatas); //per create and update

            showModalPaypal(); //MODAL CON I CAMPI CARTA PER PAYPAL

            $(btnorder).removeAttr("disabled")
            $(btnorder).html(tastotxt);
        }
        else {
            $(btnorder).removeAttr("disabled")
            $(btnorder).html(tastotxt);
            console.log('not  validated');
            var messaggioerrore = paypalcollecteddatas.messages + ' ';
            resultMessage(messaggioerrore);
            setTimeout(function () {
                resultMessage("");
            }, 10000);
        }

    });

}

async function raccoltaDatiFormAsync() {
    paypalcollecteddatas = {};
    getcontactdataformorder(paypalcollecteddatas, function (paypalcollecteddatas) {
        paypalcreateorder.contactdatas = JSON.stringify(paypalcollecteddatas);
    });
}

async function createOrderCallback() {
    let errorStringCompleteCreate = "";
    try {

        //////////////TEST CON CHIAMAAT WEBAPI ( da completare, problemi nel routing )////////////////////)
        //var ser = $('#aspnetForm').serialize();
        //const response = await $.post('/api/Webapi/CreateOrderAsync', JSON.stringify(ser));
        //const response = await $.post('/api/Webapi/CreateOrderAsync', JSON.stringify(paypalcreateorder));
        //.success(function (data) {
        //    // Do something to tell the user that all went well.
        //    //console.log(data);
        //    //var sv = Object.prototype.toString.call(data);
        //    //console.trace(sv);
        //})
        //.error(function (data, msg, detail) {
        //    alert(data + '\n' + msg + '\n' + detail)
        //});
        ////////////////////////////////////////////////////////////////////////////

        await raccoltaDatiFormAsync(); //raccolgo i dati del form e li metto in paypalcreateorder.contactdatas ( con await aspetto che la procedura sia completata)
        if (paypalcreateorder.contactdatas == undefined || JSON.parse(paypalcreateorder.contactdatas).validated != true) { //prima controllo la validazione dei dati ...
            //stop e mado errore
            console.log('not  validated');
            var messaggioerrore = JSON.parse(paypalcreateorder.contactdatas).messages + ' ';
            resultMessage(messaggioerrore);
            throw new Error(messaggioerrore);
        }

        /////////////////////////
        //chiamata server con handler
        /////////////////////////
        const response = await fetch("/lib/hnd/HandlerPayments.ashx", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            // use the "body" param to optionally pass additional order information
            // like product ids and quantities
            body: JSON.stringify(paypalcreateorder)
        });

        let responsefromserver = await response.json();
        let orderData = "{}";
        if (testJSON(responsefromserver.paypal_response))
            orderData = JSON.parse(responsefromserver.paypal_response);

        //imposto i valori provenienti dalla createorder sul server nella variabile che sono usati nella procedura di approvazione del pagamento
        paypalfinalizedatas["codiceordine"] = responsefromserver.codiceordine;
        paypalfinalizedatas["nome"] = responsefromserver.nome;
        if (responsefromserver.stoperror == "true") { errorStringCompleteCreate = 'Errore creazione ordine. ' + responsefromserver.messages; resultMessage(errorStringCompleteCreate); }

        //Controllo i dati da paypal e verifico se ci sono errori
        if (orderData.id) {
            //console.log('id paypal da procedura create:' + orderData.id);
            return orderData.id;
        } else {
            const errorDetail = orderData?.details?.[0];
            const errorMessage = errorDetail
                ? `${errorDetail.issue} ${errorDetail.description} (${orderData.debug_id})`
                : JSON.stringify(orderData);

            throw new Error(errorMessage);
        }
        /////////////////////////


    } catch (error) {
        console.error(error);
        resultMessage(`Errore procedura pagamento <br/>` + errorStringCompleteCreate + ` <br/>${error}`);
    }
}

async function onApproveCallback(data, actions) {
    //${data.orderID}
    let errorStringCompleteApprove = "";
    try {
        //   console.log('orderID inviato da paypal, inizio procedura approve:' + data.orderID);

        paypalfinalizedatas["orderID"] = data.orderID; //id ordine passato da paypal relativo allordine approvato
        paypalregisterorder.paypalfinalizedatas = JSON.stringify(paypalfinalizedatas);
        const response = await fetch((pathAbs + "/lib/hnd/HandlerPayments.ashx"), {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(paypalregisterorder)
        });

        let responsefromserver = await response.json();
        let orderData = "{}";
        if (testJSON(responsefromserver.paypal_response))
            orderData = JSON.parse(responsefromserver.paypal_response);
        //verifica finale per paga successo ordine!!!
        if (responsefromserver.stoperror == "true") { errorStringCompleteApprove = 'Errore completamento ordine. ' + responsefromserver.messages; resultMessage(errorStringCompleteApprove); }


        // Three cases to handle ( returning from approve on server ):
        //   (1) Recoverable INSTRUMENT_DECLINED -> call actions.restart()
        //   (2) Other non-recoverable errors -> Show a failure message
        //   (3) Successful transaction -> Show confirmation or thank you message
        const transaction =
            orderData?.purchase_units?.[0]?.payments?.captures?.[0] ||
            orderData?.purchase_units?.[0]?.payments?.authorizations?.[0];
        const errorDetail = orderData?.details?.[0];

        // this actions.restart() behavior only applies to the Buttons component
        if (errorDetail?.issue === "INSTRUMENT_DECLINED" && !data.card && actions) {
            // (1) Recoverable INSTRUMENT_DECLINED -> call actions.restart()
            // recoverable state, per https://developer.paypal.com/docs/checkout/standard/customize/handle-funding-failures/
            return actions.restart();
        } else if (
            errorDetail ||
            !transaction ||
            transaction.status === "DECLINED"
        ) {
            // (2) Other non-recoverable errors -> Show a failure message
            let errorMessage;
            if (transaction) {
                errorMessage = `Transaction ${transaction.status}: ${transaction.id}`;
            } else if (errorDetail) {
                errorMessage = `${errorDetail.description} (${orderData.debug_id})`;
            } else {
                errorMessage = JSON.stringify(orderData);
            }

            throw new Error(errorMessage);
        } else {
            // (3) Successful transaction -> Show confirmation or thank you message
            // Or go to another URL:  actions.redirect('thank_you.html');

            //1. VERSIONE con messaggio in pagina
            resultMessage(
                `Transaction ${transaction.status}: ${transaction.id}<br><br>Vedi in console log per i dettagli!`,
            );
            console.log(
                "Capture result",
                orderData,
                JSON.stringify(orderData, null, 2),
            );
            paypal_buttons.close();//nasconde i bottoni di pagamento

            //2. (VERSIONE ALTERNATIVA)  redirect finale alla pagina THANKYOU!!!
            if (responsefromserver.stoperror != "true")
                location.assign(responsefromserver["SuccessUrl"]);

        }
    } catch (error) {
        console.error(error);
        resultMessage(
            `Impossibile completare la transazione ...<br>` + errorStringCompleteApprove + ` <br>${error}`,
        );
    }
}

async function onCancelCallback(data) {
    resultMessage("Pagamento annullato dall'utente!");

    //paypalfinalizedatas["orderID"] = data.orderID; //id ordine passato da paypal relativo allordine approvato
    //paypalcancelorder.paypalfinalizedatas = JSON.stringify(paypalfinalizedatas);
    //const response = await fetch((pathAbs + "/lib/hnd/HandlerPayments.ashx"), {
    //    method: "POST",
    //    headers: {
    //        "Content-Type": "application/json",
    //    },
    //    body: JSON.stringify(paypalcancelorder)
    //});

    //const orderData = await response.json();
    // da ultimare  ....
    // le attivita in caoso di cancellazione...
}
//async function onErrorCallback(data) {
//    resultMessage("The transaction returned error!");
//}

// Example function to show a result to the user. Your site's UI library can be used instead.
function resultMessage(message) {
    const container = document.querySelector("#result-message");
    container.innerHTML = message;
}
///////////////////////END paypal payment /////////////////////////////



