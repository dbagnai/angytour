//This is the service worker with the Cache-first network https://developers.google.com/web/fundamentals/primers/service-workers/
//https://jakearchibald.com/2014/offline-cookbook/#stale-while-revalidate
//https://googlechrome.github.io/samples/service-worker/basic/index.html

// If at any point you want to force pages that use this service worker to start using a fresh
// cache, then increment the CACHE_VERSION value. It will kick off the service worker update
// flow and the old cache(s) will be purged as part of the activate event handler when the
// updated service worker is activated.

var CACHE_VERSION = 1; //Per invalidare la cache
var host = self.location.hostname;
var PRECACHE = "";
//var PRECACHE = 'Pwapreache-' + host + "-" + CACHE_VERSION;
var RUNTIMECACHE = 'Pwaruntimecache-' + host + "-" + CACHE_VERSION;

var precacheFiles = [
    /* Add an array of files to precache for your app that are needed to make site work!*/
    '/bdejs/bundlejssw',
    '/bdejs/bundlejslib0',
    '/bdecss/bundlecss1',
    '/bdejs/bundlejslib1',
    '/bdejs/bundlejslib2'
    
];

/*Show amount of cache space used*/
//navigator.storageQuota.queryInfo("temporary").then(function (info) {
//    console.log(info.quota);
//    // Result: <quota in bytes>
//    console.log(info.usage);
//    // Result: <used data in bytes>
//    //Over a certain cache size i can remove promo cache ??

//});

//Install stage sets up the cache-array to configure pre-cache content
self.addEventListener('install', function (evt) {
    evt.waitUntil(precache().then(function () {
        console.log('The service worker is installed. And skip Waiting.');
        return self.skipWaiting();
    }));
});
function precache() {
    return caches.open(RUNTIMECACHE).then(function (cache) {
        return cache.addAll(precacheFiles);
    });
};

//allow sw to control of current page
self.addEventListener('activate', function (event) { //when service worker enter in action fire activare event
    //console.log('Sw ready to handle fetches');
    //console.log('Claiming clients for current page');
    //return self.clients.claim(); //Collega al sw attuale tutte le sessioni attive aperte in precedenza

    //Elimina le vecchie versioni delle cache al cambio di versione
    const currentCaches = [PRECACHE, RUNTIMECACHE];
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return cacheNames.filter(cacheName => !currentCaches.includes(cacheName));
        }).then(cachesToDelete => {
            return Promise.all(cachesToDelete.map(cacheToDelete => {
                return caches.delete(cacheToDelete);
            }));
        }).then(() => self.clients.claim())
    );
});
//VERSIONE 2
self.addEventListener('fetch', function (event) {
    //////////////////////////////////////////////////////////////////
    // ignore anything other than GET requests and unwantend cached resources
    if (event.request.method !== 'GET') { return; } //Non gestisco le richieste diverse da GET col serviceWorker, basta ritornare senza chiamare .respondWith.
    if (event.request.url.indexOf(".ashx") !== -1) { return; } //torno tutte le chimate a handlers, basta ritornare senza chiamare .respondWith.
    if (!event.request.url.startsWith(self.location.origin)) { return; }; //skip di tutte le richieste cross origin!!
    //if (request.url.indexOf(myAPIUrl) !== -1) { return; } //Posso saltare tutte le richieste che non voglio gestire col service worker restituendole direttamente
    //////////////////////////////////////////////////////////////////

    console.log('The service worker is serving the asset.' + event.request.url);
    // handle other requests
    event.respondWith(
        caches.open(RUNTIMECACHE).then(function (cache) {
            return cache.match(event.request).then(function (response) {//rispondo con la cache se presente

                //Stale-while-revalidate
                if (event.request.cache === 'only-if-cached' && event.request.mode !== 'same-origin') {
                    return;
                }
                var fetchPromise = fetch(event.request).then(function (networkResponse) {
                    if (networkResponse) {
                        if (!networkResponse || networkResponse.status !== 200 || networkResponse.type !== 'basic') {
                            return networkResponse;
                        }
                        //console.debug("updated cached page: " + event.request.url, networkResponse);
                        cache.put(event.request, networkResponse.clone());
                    }
                    return networkResponse;
                }, function (e) {
                    // rejected promise - just ignore it, we're offline
                    //    console.warn('Couldn\'t serve response for "%s" from cache: %O', event.request.url, e);
                    return;
                });
                    //.catch(function (e) {
                    //    console.warn('Couldn\'t serve response for "%s" from cache: %O', event.request.url, e);
                    //    return;
                    //});
                return response || fetchPromise; //return event cache first!!!

                /*
                // IMPORTANT: Clone the request. A request is a stream and
                // can only be consumed once. Since we are consuming this
                // once by cache and once by the browser for fetch, we need
                // to clone the response.
                var fetchRequest = event.request.clone();
                if (response) {   // Cache hit - return response
                    event.waitUntil(update(fetchRequest)); //Update della cache continuo ( se online > aggiorna il contenuto in cache , che si vedà alla chiamata successiva )
                    return response;
                }
                else {
                    fetch(fetchRequest).then(function (response) { //se non presente la cache carico dal se server
                        // Check if we received a valid response
                        if (!response || response.status !== 200 || response.type !== 'basic') {
                            return response;
                        }
                        // IMPORTANT: Clone the response. A response is a stream
                        // and because we want the browser to consume the response at the end
                        // as well as the cache consuming the response, we need
                        // to clone it so we have two streams one for cache and one for browser.
                        var responseToCache = response.clone();
                        caches.open(CACHE)
                            .then(function (cache) {
                                cache.put(event.request, responseToCache);
                            });
                        return response;
                    });
                }
                 */

            });
        })
    );
});
//function update(request) {
//    //this is where we call the server to get the newest version of the 
//    //file to use the next time we show view
//    return caches.open(RUNTIMECACHE).then(function (cache) {
//        return fetch(request).then(function (response) {
//            if (!response || response.status !== 200 || response.type !== 'basic')
//                return;
//            else
//                return cache.put(request, response);
//        });
//    });
//};

//VERSIONE 1
//self.addEventListener('fetch', function (evt) {
//    console.log('[PWA Builder] The service worker is serving the asset.' + evt.request.url);
//    evt.respondWith(fromCache(evt.request).catch(fromServer(evt.request)));
//    evt.waitUntil(update(evt.request));
//});
//function fromCache(request) {
//    //we pull files from the cache first thing so we can show them fast
//    return caches.open(CACHE).then(function (cache) {
//        return cache.match(request).then(function (matching) {
//            return matching || Promise.reject('no-match');
//        });
//    });
//};
//function fromServer(request) {
//    //this is the fallback if it is not in the cache to go to the server and get it
//    return fetch(request).then(function (response) { return response });
//};
//function update(request) {
//    //this is where we call the server to get the newest version of the 
//    //file to use the next time we show view
//    return caches.open(CACHE).then(function (cache) {
//        return fetch(request).then(function (response) {
//            return cache.put(request, response);
//        });
//    });
//};

