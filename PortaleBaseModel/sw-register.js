//This is the service worker with the Cache-first network
//https://developer.mozilla.org/en-US/docs/Web/API/Service_Worker_API/Using_Service_Workers
//Add this below content to your HTML page, or add the js file to your page at the very top to register service worker

//navigator.serviceWorker.getRegistration().then(function(r){r.unregister();});

//if (navigator.serviceWorker.controller) {
//        console.log('[PWA Builder] active service worker found, no need to register');
//} else {
//    //Register the ServiceWorker
//      navigator.serviceWorker.register('/sw.js', {
//        //scope: '/I/blog/'
//      }).then(function(reg) {
//        console.log('Service worker has been registered for scope:'+ reg.scope);
//      }).catch(function(error) {
//        // registration failed
//        console.log('Registration failed with ' + error);
//      });
//};
(function () {
    'use strict';
    if ('serviceWorker' in navigator) {
        window.addEventListener('load', function () {
            navigator.serviceWorker.register('/sw.js').then(function (registration) {
                // Registration was successful
                console.log('ServiceWorker registration successful with scope: ', registration.scope);
                //registration.update(); //questa forza l'aggiornamento deidati della cache del sw

                //Utilizzo della funzione service worker postMessage - Comando per fare il trim della cache
                //if (navigator.serviceWorker.controller != null) 
                // navigator.serviceWorker.controller.postMessage({'command': 'trimCaches'});


            }, function (err) {
                // registration failed :(
                console.log('ServiceWorker registration failed: ', err);
            });
        });

      

    };

})();
