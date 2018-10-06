//This is the service worker with the Cache-first network
//https://developer.mozilla.org/en-US/docs/Web/API/Service_Worker_API/Using_Service_Workers
//Add this below content to your HTML page, or add the js file to your page at the very top to register service worker

//navigator.serviceWorker.getRegistration().then(function(r){r.unregister();});

let swRegistration;
(function () {
    'use strict';
    //if (('serviceWorker' in navigator) && (  'PushManager' in window)) {
    if (('serviceWorker' in navigator)) {

        window.addEventListener('load', function () {
            navigator.serviceWorker.register('/sw.js', { /*scope: '/I/blog/'*/ }).then(function (swReg) {
                // Registration was successful
                swRegistration = swReg;

                console.log('ServiceWorker registration successful with scope: ', swRegistration.scope);
                console.log('Service Worker is registered', swRegistration);

                pushM.applicationServerPublicKey = cbindvapidPublicKey;
                pushM.initializeUI(); //Gestione delle notifiche push

                //registration.update(); //questa forza l'aggiornamento deidati della cache del sw


                if ('storage' in navigator && 'estimate' in navigator.storage) {
                    navigator.storage.estimate().then(estimate => {
                        console.log(`Using ${estimate.usage} out of ${estimate.quota}`);
                        var $used = document.querySelector(".storage-use"),
                            $available = document.querySelector(".storage-available");
                        //$used.innerText = Math.floor(estimate.usage / 1000000);
                        //$available.innerText = Math.floor(estimate.quota / 1000000);
                    });
                }

                ////ESEMPI COMUNICAZIONE WEBPAGE -> serviceworker e viceversa 
                //https://googlechrome.github.io/samples/service-worker/post-message/
                //https://github.com/w3c-webmob/ServiceWorkersDemos

                //Invio messaggio da pagina a serviceworker
                //Utilizzo della funzione service worker postMessage - Comando per fare il trim della cache
                //if (navigator.serviceWorker.controller != null)
                // navigator.serviceWorker.controller.postMessage({'command': 'trimCaches'}); //invio messaggio al servicewirker dalla pagina

                //Gestore evento su web page a seguto messaggio proveniente da service worker
                navigator.serviceWorker.addEventListener('message', function (event) {
                    //if (event.data.timestamp - currentTimestamp > 4 * 60 * 60 * 1000) { //timestamp in millisecondi
                    //    document.location.reload();
                    //}
                    //if (event.data.type == 'updatecachequeque') { //timestamp in millisecondi
                    //    var messaggio = event.data.message;
                    //    localforage.setItem('callqueque', messaggio, function (err, value) { console.log('updated callqueque ' + value + '  ' + err); });

                    //}
                    //if (event.data.type == 'getcachequeque') { //timestamp in millisecondi
                    //    var requestQueque;
                    //    //invio la coda al serviceworker
                    //    localforage.getItem('callqueque', function (err, value) {
                    //        if (!err) {
                    //            requestQueque = value || [];
                    //            if (navigator.serviceWorker.controller != null)
                    //                navigator.serviceWorker.controller.postMessage({ 'command': 'getcachequeque', 'message': requestQueque }); //invio messaggio al servicewirker dalla pagina
                    //        }
                    //    });

                    //}
                    //if (event.data.type == 'clearcachequeque') { //timestamp in millisecondi
                    //    localforage.removeItem('callqueque', function (err, value) { console.log('removed callqueque ' + value + '  ' + err); });
                    //}
                });


            }, function (err) {
                // registration failed :(
                console.log('ServiceWorker registration failed: ', err);
            });
        });



    };


})();

/*fUNZIONE GESTIONE E ATTIVAZIONE DELLE NOTIFICHE PUSH PER LA CREAZIONE DELLE SOTTOSCRIZIONI NECESSARIE ALL'INVIO*/
var pushM = new function () {

    var mainscope = this; //Variabile ad uso interno!
    /*Gestione push events 
     * https://developers.google.com/web/fundamentals/codelabs/push-notifications/
     https://web-push-codelab.glitch.me/ 
     */
    this.applicationServerPublicKey = "";
    //Public Key
    //BLNc1fwQwvTEBohCVlr1VL0qE-rCBIBpKhqwWdI029ikIEh67VAMQppJnxloUOyvLfC7ClRtXN7xsaMgMOU6IYY
    //Private Key
    //pU5v_A93OiiqGmF8_JVYa5MlyNpMA8jHov3bsBhDEOY
    this.pushButton = document.querySelector('.js-push-btn');
    this.isSubscribed = false;
    if (mainscope.pushButton)
        mainscope.pushButton.addEventListener('click', function () {
            mainscope.pushButton.disabled = true;
            if (mainscope.isSubscribed) {
                unsubscribeUser();
            } else {
                subscribeUser();
            }
        });

    this.initializeUI = function () {
        if ('serviceWorker' in navigator && 'PushManager' in window) {
            // Set the initial subscription value
            swRegistration.pushManager.getSubscription()
                .then(function (subscription) {

                    if (subscription) {
                        if (mainscope.base64Encode(subscription.options.applicationServerKey) != mainscope.applicationServerPublicKey) //sottoscrizione non più valida per cambio keys->unsubscribe
                        {
                            subscription.unsubscribe().then(function (successful) {
                                mainscope.initializeUI(); //recall initialize
                                return;
                            });
                            return;
                        }
                    }

                    mainscope.isSubscribed = !(subscription === null);
                    updateSubscriptionOnServer(subscription);
                    if (mainscope.isSubscribed) {
                        console.log('User IS subscribed.');
                    } else {
                        console.log('User is NOT subscribed.');
                    }
                    updateBtn();
                });
        } else {
            console.warn('Push messaging is not supported');
            if (mainscope.pushButton)
                mainscope.pushButton.innerHTML = '<i class="fa fa-2x fa-ban"></i>';
        }
    };


    function verifyvalidsubscriptionandask() {
        if ('serviceWorker' in navigator && 'PushManager' in window) {
            // Set the initial subscription value
            swRegistration.pushManager.getSubscription()
                .then(function (subscription) {
                    //Controllo se sottoscritto e la puclikkey coincide
                    if (subscription) {
                        if (mainscope.base64Encode(subscription.options.applicationServerKey) != mainscope.applicationServerPublicKey) //sottoscrizione non più valida per cambio keys->unsubscribe
                        {
                            subscription.unsubscribe().then(function (successful) {
                                //do something to ask user for subscription!!! or ;
                                // subscribeUser();
                                return;
                            });
                            return;
                        }
                    } else {
                        //do something to ask user for subscription!!!;
                        // subscribeUser();
                    }
                });
        };
    }


    function subscribeUser() {
        console.log('asked subscription');
        //we take the application server's public key, which is base 64 URL safe encoded, and we convert it to a UInt8Array as this is the expected input of the subscribe call.
        const applicationServerKey = mainscope.urlB64ToUint8Array(mainscope.applicationServerPublicKey);
        swRegistration.pushManager.subscribe({
            userVisibleOnly: true, //required an admission that you will show a notification every time a push is sent
            applicationServerKey: applicationServerKey
        })
            .then(function (subscription) {
                console.log('User is subscribed.');
                updateSubscriptionOnServer(subscription);
                mainscope.isSubscribed = true;
                updateBtn();
            })
            .catch(function (err) {
                console.log('Failed to subscribe the user: ', err);
                updateBtn();
            });
    }

    function updateSubscriptionOnServer(subscription) {
        // TODO: Send subscription to application server
        const subscriptionJson = document.querySelector('.js-subscription-json');
        const subscriptionDetails = document.querySelector('.js-subscription-details');

        if (subscription) {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Questa è la subscription che chi richiede il push deve inviare per eesere riconosciuto
            //CAMBIA PER OGNI BROWSER E SESSIONE QUINDI AL MOMENTO DELLA SOTTOSCRIZIONE 
            //QUESTA VA' PASSATA AL SERVER DI PUSH CHE DEVE AVERE LA LISTA DI TUTTE LE SOTTOSCRIZIONI A CUI INVIARE LE PUSH!!!
            if (subscriptionJson)
                subscriptionJson.textContent = JSON.stringify(subscription); //testo della sottoscrizione!
            //Valori della subscription per fare il push ( da passare al server di push )
            var Name = "";
            var p256dh = mainscope.base64Encode(subscription.getKey('p256dh'));
            var auth = mainscope.base64Encode(subscription.getKey('auth'));
            var pushEndpoint = subscription.endpoint;
            var Devices = {};
            Devices.Id = 0;
            Devices.Name = "";
            Devices.PushP256DH = p256dh;
            Devices.PushAuth = auth;
            Devices.PushEndpoint = pushEndpoint;
            //chiamata a handler per salvare/aggiornare il device!  del device jobj["Devices"]
            updateDevicesubscriptions(Devices);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //subscriptionDetails.classList.remove('d-none');
        } else {
            //subscriptionDetails.classList.add('d-none');
        }
    }
    function unsubscribeUser() {
        //da inserire gestione database per eliminazione del device jobj["Devices"]
        swRegistration.pushManager.getSubscription()
            .then(function (subscription) {
                if (subscription) {
                    return subscription.unsubscribe();
                }
            })
            .catch(function (error) {
                console.log('Error unsubscribing', error);
            })
            .then(function () {
                updateSubscriptionOnServer(null);

                console.log('User is unsubscribed.');
                mainscope.isSubscribed = false;
                updateBtn();
            });
    }
    function updateDevicesubscriptions(Devices) {
        var Devices = Devices || {};
        try {
            var devicesserialized = JSON.stringify(Devices);
            if (devicesserialized != '' && devicesserialized != null) {
                $.ajax({
                    type: "POST",
                    url: pathAbs + pushhandlerpath,
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    data: {
                        'q': 'subscribedevice', 'devices': devicesserialized
                    },
                    success: function (result) { console.log('Updated Devices on application server'); },
                    error: function (result) { console.log('Error updating devices'); },
                    failure: function (result) { console.log('Error updating devices'); }
                });
            }
        } catch (e) { };
    }
    function updateBtn() {
        if (mainscope.pushButton) {
            if (Notification.permission === 'denied') {
                mainscope.pushButton.innerHTML = '<i class="fa fa-2x fa-ban"></i>';
                mainscope.pushButton.disabled = true;
                updateSubscriptionOnServer(null);
                return;
            }
            if (mainscope.isSubscribed) {
                mainscope.pushButton.innerHTML = '<i class="fa fa-2x fa-bell"></i>';
            } else {
                mainscope.pushButton.innerHTML = '<i class="fa fa-2x fa-bell-slash" ></i>';
            }
            mainscope.pushButton.disabled = false;
        }
    }


    this.base64Encode = function (uint8Array) {
        const base64 = btoa(String.fromCharCode.apply(null, new Uint8Array(uint8Array)));
        return base64
            .replace(/\=/g, '') // eslint-disable-line no-useless-escape
            .replace(/\+/g, '-')
            .replace(/\//g, '_');
    };
    this.urlB64ToUint8Array = function (base64String) {
        const padding = '='.repeat((4 - base64String.length % 4) % 4);
        const base64 = (base64String + padding)
            .replace(/\-/g, '+')
            .replace(/_/g, '/');

        const rawData = window.atob(base64);
        const outputArray = new Uint8Array(rawData.length);

        for (let i = 0; i < rawData.length; ++i) {
            outputArray[i] = rawData.charCodeAt(i);
        }
        return outputArray;
    };


}();