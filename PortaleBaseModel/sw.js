//This is the service worker with the Cache-first network https://developers.google.com/web/fundamentals/primers/service-workers/
//https://jakearchibald.com/2014/offline-cookbook/#stale-while-revalidate
//https://googlechrome.github.io/samples/service-worker/basic/index.html
//https://www.afasterweb.com/2017/01/31/upgrading-your-service-worker-cache/

// If at any point you want to force pages that use this service worker to start using a fresh
// cache, then increment the CACHE_VERSION value. It will kick off the service worker update
// flow and the old cache(s) will be purged as part of the activate event handler when the
// updated service worker is activated.

var CACHE_VERSION = 2; //Per invalidare la cache
var host = self.location.hostname;
const coreCacheName = CACHE_VERSION + host + 'core';
const pagesCacheName = CACHE_VERSION + host + 'pages';
const assetsCacheName = CACHE_VERSION + host + 'assets';

var myPwaUrlPath = ""; /* /I/blog/ */

var allowedDomains = [
    /*Sn array of external domain that i WANT to cache!*/
    'fonts.googleapis.com',
    'fonts.gstatic.com'
];

var coreCacheUrls = [
    /* Add an array of files to precache for your app that are needed to make site work!*/
    '/error.aspx',
    '/bdejs/bundlejssw',
    '/bdejs/bundlejslib0',
    '/bdecss/bundlecss1',
    '/bdejs/bundlejslib1',
    '/bdejs/bundlejslib2'
];
function updateCoreCache() {
    return caches.open(coreCacheName)
        .then(cache => {
            // Make installation contingent on storing core cache items
            return cache.addAll(coreCacheUrls);
        });
}


//Install stage sets up the cache-array to configure pre-cache content
self.addEventListener('install', function (evt) {
    evt.waitUntil(
        updateCoreCache().then(function () {
            console.log('The service worker is installed. And skip Waiting.');
            return self.skipWaiting();
        }));
});


//allow sw to control of current page
self.addEventListener('activate', function (event) { //when service worker enter in action fire activare event
    console.log('Service worker activated ready to handle fetches');
    //Elimina le vecchie versioni delle cache al cambio di versione
    event.waitUntil(
        clearCaches().then(() => {
            //console.log('Claiming clients for current page');
            return self.clients.claim();
        })
    );
});
//Elimina le vecchie venrsioni della cache
function clearCaches() {
    const currentCaches = [coreCacheName, pagesCacheName, assetsCacheName];
    return caches.keys().then(cacheNames => {
        return cacheNames.filter(cacheName => !currentCaches.includes(cacheName));
    }).then(cachesToDelete => {
        return Promise.all(cachesToDelete.map(cacheToDelete => {
            return caches.delete(cacheToDelete);
        }));
    })
}


self.addEventListener('fetch', function (event) {

    let request = event.request, acceptHeader = event.request.headers.get('Accept');

    //////////////////////////////////////////////////////////////////
    // ignore anything other than GET requests and unwantend cached resources

    if (!shouldFetch(event)) {
        event.respondWith(
            fetch(request)
                .catch(() => {
                    return;
                })
        );
        return;
    }


    //if (!shouldFetch(event)) return;  
    //if (request.url.indexOf(myPwaUrlPath) !== -1) { return; } //Posso saltare tutte le richieste che non voglio gestire col service worker restituendole direttamente ( oppure viceversa ) 
    //////////////////////////////////////////////////////////////////
    console.log('The service worker is serving url: ' + request.url);

    if (isIncoreCache(event)) { //Elementi caricati in core cache ( li gestisco in manera dedicata!!! )
        // First Try cache, then network (and cache update), then offline fallback
        event.respondWith(
            caches.match(request)
                .then(response => {
                    return response || fetch(request)
                        .then(response => {
                            if (response.ok && response.status === 200)
                                addToCache(coreCacheName, request, response.clone());
                            return response;
                        })
                        .catch((err) => {
                            return;
                            //return new Response('<svg role="img" aria-labelledby="offline-title" viewBox="0 0 400 300" xmlns="http://www.w3.org/2000/svg"><title id="offline-title">Offline</title><g fill="none" fill-rule="evenodd"><path fill="#D8D8D8" d="M0 0h400v300H0z"/><text fill="#9B9B9B" font-family="Helvetica Neue,Arial,Helvetica,sans-serif" font-size="72" font-weight="bold"><tspan x="93" y="172">offline</tspan></text></g></svg>', { headers: { 'Content-Type': 'image/svg+xml' } });
                        });
                })
        );
    }
    //For text/html requests tecnica network first then fallback to cache
    else if (acceptHeader.indexOf('text/html') !== -1) { //in pagecache
        // Try network first
        event.respondWith(
            fetch(request)
                .then(response => {
                    if (response.ok && response.status === 200)
                        addToCache(pagesCacheName, request, response.clone());
                    return response;
                })
                // Try cache second with offline fallback
                .catch(() => {
                    return caches.match(event.request).then(response => {
                        return response || caches.match('/error.aspx');
                    })
                })
        );
    } else if (acceptHeader.indexOf('text/html') == -1) { //elementi non html in assetcache ( TUTTE LE RISORSE USATE )

        // handle other requests con cache first -> then retrieve from network and cache
        event.respondWith(
            caches.match(request).then(function (response) {//rispondo con la cache se presente

                //////////////////////AGGIORNAMENTO CACHE IN BACKGROUND SE LINEA DISPONIBILE//////////
                //Creo una richiesta CORS ( anche nei casi in cui i domini non coincidano x usare con google fonts)
                //QUesta aggiorna l'application cache del service worker se il sistema è online altrimenti ritorna
                var myHeaders = new Headers();
                var myInit = {
                    method: 'GET',
                    headers: myHeaders,
                    mode: 'cors'
                };
                var myRequest = new Request(request.url, myInit);
                var fetchPromise = fetch(myRequest).then(networkResponse => {
                    if (networkResponse.ok && networkResponse.status === 200) //status in the range 200 to 299 -> to cache only whet data is present in response
                        addToCache(assetsCacheName, request, networkResponse.clone());
                    return networkResponse;
                }).catch((err) => {
                    // console.log(err);
                    return new Response('<svg role="img" aria-labelledby="offline-title" viewBox="0 0 400 300" xmlns="http://www.w3.org/2000/svg"><title id="offline-title">Offline</title><g fill="none" fill-rule="evenodd"><path fill="#D8D8D8" d="M0 0h400v300H0z"/><text fill="#9B9B9B" font-family="Helvetica Neue,Arial,Helvetica,sans-serif" font-size="72" font-weight="bold"><tspan x="93" y="172">offline</tspan></text></g></svg>', { headers: { 'Content-Type': 'image/svg+xml' } });
                });
                //////////////////////////////////////

                return response || fetchPromise; //return event cache first!!!
            })
        );


    }
});

function isIncoreCache(event) {
    let request = event.request,
        pathPattern = /^\/(?:(20[0-9]{2}|bdejs|bdecss)\/(.+)?)?$/,
        url = new URL(request.url);
    return (
        !!(pathPattern.exec(url.pathname) && url.pathname != '/')
    )
}

function urlstoFetch(event) { //prende solo gli url col path indicato!! del tipo /qualcosa/blog/qualcosa ...
    let request = event.request,
        pathPattern = /^\/(.+)\/(?:(blog|altropathdaincludere)\/(.+)?)?$/,
        url = new URL(request.url);
    return (
        (pathPattern.exec(url.pathname) && url.pathname != '/')
    )
}
// Check if request is something SW should handle
function shouldFetch(event) {
    let request = event.request,   //Non gestisco le richieste diverse da GET col serviceWorker, basta ritornare senza chiamare .respondWith.
        // pathPattern = /^\/(?:(blog|altro)\/(.+)?)?$/, // pattern di paths da includere nelle fetch del sw ( ablitandolo prendero solo i path indicati col service worker)
        ishandler = (event.request.url.indexOf(".ashx") !== -1), //torno tutte le chimate a handlers, basta ritornare senza chiamare .respondWith.
        url = new URL(request.url); //skip di tutte le richieste cross origin!!

    var sameorigincheck = (url.origin === self.location.origin);
    if (checkAllowedomains(event)) sameorigincheck = true; //Bypass per glia llowed domains

    return (request.method === 'GET' &&
        !ishandler &&
        // !!(pathPattern.exec(url.pathname)) &&
        sameorigincheck
    )
}
function checkAllowedomains(event) {
    let request = event.request,   //Non gestisco le richieste diverse da GET col serviceWorker, basta ritornare senza chiamare .respondWith.
        url = new URL(request.url); //skip di tutte le richieste cross origin!

    for (var i = allowedDomains.length - 1; i >= 0; --i) {
        if (url.origin.indexOf(allowedDomains[i]) != -1) {
            // str contains arr[i]
            return true;
        }
    }
    return false;
    //allowedDomains
    //return allowedDomains.some(substring => url.origin.includes(substring));
}

function addToCache(cacheName, request, response) {
    caches.open(cacheName)
        .then(cache => cache.put(request, response));
}
// Trim specified cache to max size
function trimCache(cacheName, maxItems) {
    caches.open(cacheName).then(function (cache) {
        cache.keys().then(function (keys) {
            if (keys.length > maxItems) {
                cache.delete(keys[0]).then(trimCache(cacheName, maxItems));
            }
        });
    });
}
//Creo Listener per comando postMessage trimCaches del service Worker
self.addEventListener('message', event => {
    var data = event.data;
    if (data.command == "trimCache") {
        trimCache(pagesCacheName, 25);
        trimCache(assetsCacheName, 30);
    };
});


/*Show amount of cache space used*/
//navigator.storageQuota.queryInfo("temporary").then(function (info) {
//    console.log(info.quota);
//    // Result: <quota in bytes>
//    console.log(info.usage);
//    // Result: <used data in bytes>
//    //Over a certain cache size i can remove promo cache ??

//});

/*PUSH SAMPLES*/
self.addEventListener('push', function (event) {
    event.waitUntil(
        self.registration.showNotification('ServiceWorker Cookbook', {
            body: 'Alea iacta est'
        })
    );
});