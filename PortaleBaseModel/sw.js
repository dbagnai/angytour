//This is the service worker with the Cache-first network

var CACHE = 'pwabuilder-precache';
var precacheFiles = [
    /* Add an array of files to precache for your app */
    '/bdejs/bundlejslib0',
    '/bdejs/bundlejslib1',
    '/bdejs/bundlejslib2',
    '/bdecss/bundlecss1'
];

//Install stage sets up the cache-array to configure pre-cache content
self.addEventListener('install', function (evt) {
    console.log('[PWA Builder] The service worker is being installed.');
    evt.waitUntil(precache().then(function () {
        console.log('[PWA Builder] Skip waiting on install');
        return self.skipWaiting();
    }));
});
function precache() {
    return caches.open(CACHE).then(function (cache) {
        return cache.addAll(precacheFiles);
    });
};
//allow sw to control of current page
self.addEventListener('activate', function (event) {
    console.log('[PWA Builder] Claiming clients for current page');
    return self.clients.claim();
});

//VERSIONE 1
self.addEventListener('fetch', function (evt) {
    console.log('[PWA Builder] The service worker is serving the asset.' + evt.request.url);
    evt.respondWith(fromCache(evt.request).catch(fromServer(evt.request)));
    evt.waitUntil(update(evt.request));
});
function fromCache(request) {
    //we pull files from the cache first thing so we can show them fast
    return caches.open(CACHE).then(function (cache) {
        return cache.match(request).then(function (matching) {
            return matching || Promise.reject('no-match');
        });
    });
};
function fromServer(request) {
    //this is the fallback if it is not in the cache to go to the server and get it
    return fetch(request).then(function (response) { return response });
};
function update(request) {
    //this is where we call the server to get the newest version of the 
    //file to use the next time we show view
    return caches.open(CACHE).then(function (cache) {
        return fetch(request).then(function (response) {
            return cache.put(request, response);
        });
    });
};

//VERSIONE 2
//self.addEventListener('fetch', function (event) {
//    event.respondWith(
//        caches.match(event.request)
//            .then(function (response) {
//                if (response) {
//                    return response;
//                }
//                var fetchRequest = event.request.clone();

//                return fetch(fetchRequest).then(
//                    function (response) {
//                        // Check if we received a valid response
//                        if (!response || response.status !== 200 || response.type !== 'basic') {
//                            return response;
//                        }

//                        var responseToCache = response.clone();

//                        caches.open(CACHE_NAME)
//                            .then(function (cache) {
//                                cache.put(event.request, responseToCache);
//                            });

//                        return response;
//                    }
//                );
//            })
//    );
//});