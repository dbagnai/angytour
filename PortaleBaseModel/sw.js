//This is the service worker with the Cache-first network 
//https://developers.google.com/web/fundamentals/primers/service-workers/
//https://jakearchibald.com/2014/offline-cookbook/#stale-while-revalidate
//https://googlechrome.github.io/samples/service-worker/basic/index.html
//https://www.afasterweb.com/2017/01/31/upgrading-your-service-worker-cache/


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//URL CHE VENGONO PRECARICATI COMPRESE LE RISORSE PRESENTI IN PAGINA
//////////////////////////////////////////////////////////////
var pagesTofetch = [
    /* array of  pages that i WANT to completely pre - cache!*/
    /*Vengono aggiunte a quelle che sono indicate in fase di registrazione*/
];
var pagesToservewithsw = [
    /* array of  pages that i WANT to serve with serviceworker !! !*/
    //DA IMPLEMENTARE FUNZIONE DI SELEZIONE DELLE PAGINE DA SERVIRE
];

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// If at any point you want to force pages that use this service worker to start using a fresh
// cache, then increment the CACHE_VERSION value. It will kick off the service worker update
// flow and the old cache(s) will be purged as part of the activate event handler when the
// updated service worker is activated.
var CACHE_VERSION = 2; //Per invalidare la cache

//CARICHIAMO LE RISORSE CHE VERRANNO USATE DAL SERVICEWORKER
self.importScripts("/js/localforage.min.js",
    "/lib/sw/invalidation-mgr.js",
    "/lib/sw/date-mgr.js"
);
//if ('function' === typeof importScripts) {
//    importScripts('/js/localforage.min.js',
//    '/lib/sw/invalidation-mgr.js',
//    '/lib/sw/date-mgr.js');//carica una risorsa js da usare nel serviceworker!
//}

var host = self.location.hostname;
const
    pagesCacheName = CACHE_VERSION + host + 'pages',
    assetsCacheName = CACHE_VERSION + host + 'assets',
    coreCacheName = CACHE_VERSION + host + 'core',
    invalidationManager = new InvalidationManager([{
        "cacheName": coreCacheName,
        "invalidationStrategy": "ttl",
        "strategyOptions": {
            "ttl": 86400 //1 day
            //604800 //1 week
        }
    },
    {
        "cacheName": assetsCacheName,
        "invalidationStrategy": "maxItems",
        "strategyOptions": {
            "max": 10000
        }
    }]);


var allowedDomains = [
    /*Sn array of external domain that i WANT to cache!*/
    'fonts.googleapis.com',
    'fonts.gstatic.com'
];

var corenotcriticalCacheUrls = [
    /* Add an array of files to precache for your app that are needed to make site work!*/
    '/bdejs/bundlejslib0',
    '/bdecss/bundlecss1',
    '/bdejs/bundlejslib1',
    '/bdejs/bundlejslib2',
    '/error.aspx'
];
var corecriticalCacheUrls = [
    /* Add an array of files to precache for your app that are needed to make site work!*/

];
function updateCoreCache() {
    return caches.open(coreCacheName)
        .then(cache => {
            //cache.addAll(corenotcriticalCacheUrls); // important, but not critical resources
            corenotcriticalCacheUrls.forEach(element => { cache.add(element).catch(() => { /*err adding*/ }); });//QUESTA PERMETTE DI PRECARICARE ANCHE SE UN URL VA IN ERRORE!!!!
            // Make installation contingent on storing core cache items (ATTENZIONE CON ADDALL SE FALLISCE UN URL FALLISCONO TUTTI E IL SERVICEWORKER NON PARTE!!!)
            return cache.addAll(corecriticalCacheUrls);
        });
}

/* GESTIONE CODA DELLE RICHIESTE OFFLINE  ////////////////////////////////////////////////////////////////////////////////////////////    */
const isOnLine = () => isOnlinevar;
var isOnlinevar = false;
function checkonline() {
    var onlinepromisecheck = fetch(new Request('/images/favicon.ico'), { //Uso questo file per vedere se siamo online OCCHIO!
        headers: {
            'Cache-Control': 'no-cache'
        }
    }).then(networkResponse => {
        isOnlinevar = true; return true;
    }).catch((err) => {
        //we are offline!!!
        //Memorizziamo le chiamate che mi servono in una coda per riuso successivo
        isOnlinevar = false; return false;
    });
    return onlinepromisecheck;
}
function indexDbcheck() {
    if (self.indexedDB) {
        console.log('IndexedDB is supported');
    }
}
const requestBuffer = {
    _requestQueue: [],
    _requestQueueStorage: [],
    intervalId: null,
    updateRequestQueque(message) {
        //this._requestQueue = message;
        this._requestQueueStorage = message;
    },
    pushRequestForRetry(request, event) {
        //var cachedrequest = request.clone();
        this._requestQueue.push(request); //Memorizzo la richiesta nella coda ( imprementare eventualmente sistema persistente alla chiusura del browser!!! )
        if (localforage) {
            var localobj = this;
            //Dalla request devo creare un oggetto json che contiene request.url,payload, method e headers che mi servono per replicare la richiesta al server!!!
            //ed andare ad agigungerlo all'array di oggetti fetch serializzati  this._requestQueueStorage facciamo un [] di oggetti {} che ognuno contiene i dati per fare le chiamate
            var requestobj = {};
            var newrequest = request.clone();
            newrequest.text().then(function (data) {
                requestobj.payload = data;
                requestobj.method = newrequest.method;
                requestobj.url = newrequest.url;
                requestobj.mode = newrequest.mode;
                requestobj.accept = newrequest.headers.get("Accept");
                requestobj.contenttype = newrequest.headers.get("Content-Type");
                //newrequest.headers.forEach(function (element) {
                //    console.log(element);
                //});
                localobj._requestQueueStorage.push(requestobj);
                console.log('Writing indexedDB' + localobj._requestQueueStorage);
                localforage.setItem('callqueque', localobj._requestQueueStorage, function (err, value) {
                    if (!localobj.intervalId) {
                        localobj.start(event);
                    } console.log('updated callqueque ' + value + '  ' + err);
                });
            }, localobj);
        }
        else if (!this.intervalId) {
            this.start(event);
        }
    },
    start(event) {
        const retry = async () => {
            if (isOnLine()) {
                console.log('[service worker] connection re-established, retrying with buffered requests');

                //Se presenti richieste nella coda di memoria volatile faccio le chiamate
                if (this._requestQueue && Array.isArray(this._requestQueue) && this._requestQueue.length > 0)
                    while (this._requestQueue.length) {
                        await fetch(this._requestQueue.shift()); //esegue le fetch delle richieste nella coda!!!!
                    }
                //in alternativa se presenti richeste nello storage persistente faccio le fetch 
                else if (this._requestQueueStorage && Array.isArray(this._requestQueueStorage) && this._requestQueueStorage.length > 0)
                    while (this._requestQueueStorage.length) {
                        var currequest = this._requestQueueStorage.shift();
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //RICREO LA RICHIESTA ED ESEGUO LA FETCH
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////
                        var requestUrl = currequest.url;
                        var payload = currequest.payload;// JSON.stringify(currequest.payload);
                        if (!(typeof currequest.payload === 'string' || currequest.payload instanceof String))
                            payload = JSON.stringify(currequest.payload);
                        var method = currequest.method;
                        var mode = currequest.mode;
                        var headers = {
                            'Accept': currequest.accept,//'application/json',
                            'Content-Type': currequest.contenttype,//'application/json; charset=utf-8',
                            'Cache-Control': 'no-cache'
                        }; // if you have any other headers put them here, aggiungi se vuoi anche  'Content-Length': payload.length,

                        await fetch(requestUrl, {
                            headers: headers,
                            mode: mode,
                            method: method,
                            body: payload
                        }).then(function (response) {
                            console.log('server response', response);
                            if (response.status < 400) {
                                //do nothing
                            }
                        }).catch(function (error) {
                            console.error('Send to Server failed:', error);
                        });
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }

                this._requestQueue = [];
                this._requestQueueStorage = [];
                if (localforage)
                    localforage.removeItem('callqueque', function (err, value) { console.log('removed callqueque ' + value + '  ' + err); });
                const client = await clients.get(event.clientId);
                if (client) {
                    client.postMessage({
                        msg: 'Connection re-established; pending requests flushed.'
                    });
                }
                clearTimeout(this.intervalId);
                this.intervalId = null;

            } else {
                this.intervalId = setTimeout(retry, 5000);
            }
            checkonline(); //Aggiorno la varialbile isOnLine testando se siamo tornati online!
        };
        this.intervalId = setTimeout(retry, 5000);
    }
}
function recoverRequestsCache(event) {
    if (localforage)
        localforage.getItem('callqueque', function (err, value) {
            if (value && !err)
                if (Array.isArray(value)) {
                    requestBuffer.updateRequestQueque(value);
                    if (!requestBuffer.intervalId) {
                        requestBuffer.start(event);
                    }
                } else localforage.removeItem('callqueque', function (err, value) { console.log('removed callqueque ' + value + '  ' + err); });
        });
}
/* FINE GESTIONE RICHIESTE OFFLINE ////////////////////////////////////////////////////////////////////////////// */

//Install stage sets up the cache-array to configure pre-cache content
self.addEventListener('install', function (event) {
    if (event && event.target.toString() == '[object ServiceWorkerGlobalScope]') {
        console.log('Service worker installing');
        event.waitUntil(
            updateCoreCache().then(function () {
                console.log('The service worker is installed. And skip Waiting.');
                return self.skipWaiting();
            }));
    }
});


//allow sw to control of current page
self.addEventListener('activate', function (event) { //when service worker enter in action fire activare event
    //Elimina le vecchie versioni delle cache al cambio di versione
    if (event && event.target.toString() == '[object ServiceWorkerGlobalScope]') {
        console.log('Service worker activated ready to handle fetches');
        event.waitUntil(
            clearCaches().then(() => {
                return self.clients.claim();
            })
        );
    }
});
//Elimina le vecchie versioni della cache
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

    ////////////////////////////////////////////////////////////////////////////
    // controlliamo se la chiamata deve essere servita dal service worker o meno
    if (!shouldFetch(event)) {

        ////////////////////////////////////////////////////////////////////////////
        //test handler call per contatti se online -> se corrisponde inserisco nella push cache per rifare la chiamata dopo
        ////////////////////////////////////////////////////////////////////////////
        if (request.url.toLowerCase().indexOf(".ashx") !== -1) {
            ///var getPayload = JSON.parse(context.getVariable('request.content'));
            var newrequest = request.clone();
            newrequest.text().then(function (data) {
                //  decodeURIComponent(data)
                if (data.indexOf("inviamessaggiomail") !== -1) {
                    checkonline().then(function (onlinestatus) {
                        //console.log("Online statis from check :" + onlinestatus + " requesturl:" + request.url);
                        if (!onlinestatus) {
                            var newrequestcached = request.clone();
                            //Siamo offline
                            console.log('[service worker] app is offline - storing a request to retry later');
                            requestBuffer.pushRequestForRetry(newrequestcached.clone(), event);
                            //event.respondWith(Promise.resolve(new Response({}, { status: 202 }))); // 202 - Accepted
                        }
                    });
                }
            }).catch((err) => { console.log("Erron retrieving data payload for request: " + err); });
            //return new Response({}, { status: 202 });
            return;
        } else {
            ////////////////////////////////////////////////////////////////////////////
            //direct call from servicwworker directly to network
            ////////////////////////////////////////////////////////////////////////////
            event.respondWith(
                fetch(request)//Cerico e ritorno direttamente dal network
                    .catch((err) => {
                        //we are offline!!!
                        return;
                    })
            );
            return;
        }

    }
    ////////////////////////////////////////////////////////////////////////////

    //console.log('The service worker is serving url: ' + request.url);
    if (isIncoreCache(event.request.url)) { //Elementi caricati in core cache ( li gestisco in manera dedicata!!! )
        event.respondWith(
            // First Try cache, 
            caches.match(request).then(response => {
                // if (request.url.indexOf('bundlejssw') !== -1) return;
                //////////////////////AGGIORNAMENTO CACHE IN BACKGROUND SE LINEA DISPONIBILE//////////
                //Creo una richiesta CORS ( anche nei casi in cui i domini non coincidano x usare con domini esterni)
                //QUesta aggiorna la cache del service worker se il sistema � online altrimenti ritorna
                var myHeaders = new Headers();
                var myInit = {
                    method: 'GET',
                    headers: myHeaders,
                    mode: 'cors'
                };
                var myRequest = new Request(request.url, myInit);
                var fetchPromise = fetch(myRequest).then(networkResponse => {
                    if (networkResponse.ok && networkResponse.status === 200) //status in the range 200 to 299 -> to cache only if data is present in response
                    {
                        addToCache(coreCacheName, myRequest, networkResponse.clone());
                        //console.log("Updated corecache: " + myRequest.url);
                    }
                    return networkResponse;
                }).catch(() => {
                    console.log("err fetching coreresources");
                    return new Response('');
                });
                return response || fetchPromise; //return event cache first!!!

            })
        );
    }
    //For text/html requests tecnica network first then fallback to cache
    else if (acceptHeader.indexOf('text/html') !== -1) { //in pagecache
        // Try network first and add to cache or try cache with offline fallback
        event.respondWith(
            fetch(request)
                .then(response => {
                    if (response.ok && response.status === 200)
                        addToCache(pagesCacheName, request, response.clone());
                    return response;
                })
                // Try cache second with offline fallback
                .catch(() => {
                    return caches.match(request).then(response => {
                        return response || new Response('<svg role="img" aria-labelledby="offline-title" viewBox="0 0 400 300" xmlns="http://www.w3.org/2000/svg"><title id="offline-title">Offline</title><g fill="none" fill-rule="evenodd"><path fill="#D8D8D8" d="M0 0h400v300H0z"/><text fill="#9B9B9B" font-family="Helvetica Neue,Arial,Helvetica,sans-serif" font-size="72" font-weight="bold"><tspan x="93" y="172">offline</tspan></text></g></svg>', { headers: { 'Content-Type': 'image/svg+xml' } });// caches.match('/error.aspx');
                    });
                })
        );
    } else if (acceptHeader.indexOf('text/html') == -1) { //elementi non html in assetcache ( TUTTE LE RISORSE USATE )
        // handle other requests con cache first -> then retrieve from network and cache
        event.respondWith(
            caches.match(request).then(function (response) {//rispondo con la cache se presente
                //////////////////////AGGIORNAMENTO CACHE IN BACKGROUND SE LINEA DISPONIBILE//////////
                //Creo una richiesta CORS ( anche nei casi in cui i domini non coincidano x usare con google fonts)
                //QUesta aggiorna l'application cache del service worker se il sistema � online altrimenti ritorna
                var myHeaders = new Headers();
                var myInit = {
                    method: 'GET',
                    headers: myHeaders,
                    mode: 'cors'
                };
                var myRequest = new Request(request.url, myInit);
                var fetchPromise = fetch(myRequest).then(networkResponse => {
                    if (networkResponse.ok && networkResponse.status === 200) //status in the range 200 to 299 -> to cache only whet data is present in response
                    {
                        addToCache(assetsCacheName, myRequest, networkResponse.clone());
                        //console.log("Updated assetcache: " + myRequest.url);
                    }
                    return networkResponse;
                }).catch(() => {
                    console.log("err update fetch assetres ");// + request.url);
                    return new Response('<svg role="img" aria-labelledby="offline-title" viewBox="0 0 400 300" xmlns="http://www.w3.org/2000/svg"><title id="offline-title">Offline</title><g fill="none" fill-rule="evenodd"><path fill="#D8D8D8" d="M0 0h400v300H0z"/><text fill="#9B9B9B" font-family="Helvetica Neue,Arial,Helvetica,sans-serif" font-size="72" font-weight="bold"><tspan x="93" y="172">offline</tspan></text></g></svg>', { headers: { 'Content-Type': 'image/svg+xml' } });
                });
                //////////////////////////////////////

                return response || fetchPromise; //return event cache first!!!
            })
        );
    }
});
function isIncoreCache(url) {
    //  let request = event.request,
    pathPattern = /^\/(?:(20[0-9]{2}|bdejs|bdecss)\/(.+)?)?$/;
    urlcomplete = new URL(url, self.registration.scope);
    let pathname = urlcomplete.pathname;
    return (
        !!(pathPattern.exec(pathname) && pathname != '/')
    )
}

function urlstoFetch(event) { //prende solo gli url col path indicato!! del tipo /qualcosa/blog/qualcosa ...
    let request = event.request,
        pathPattern = /^\/(.+)\/(?:(blog|altropathdaincludere)\/(.+)?)?$/,
        url = new URL(request.url, self.registration.scope);
    return (
        (pathPattern.exec(url.pathname) && url.pathname != '/')
    );
}
// Check if request is something SW should handle
function shouldFetch(event) {
    let request = event.request,   //Non gestisco le richieste diverse da GET col serviceWorker, basta ritornare senza chiamare .respondWith.
        // pathPattern = /^\/(?:(blog|altro)\/(.+)?)?$/, // pattern di paths da includere nelle fetch del sw ( ablitandolo prendero solo i path indicati col service worker)
        ishandler = (request.url.indexOf(".ashx") !== -1), //torno tutte le chimate a handlers, basta ritornare senza chiamare la .respondWith.
        url = new URL(request.url, self.registration.scope);
    var sameorigincheck = (url.origin === self.location.origin);//normalmente skip di tutte le richieste cross origin!!
    if (checkAllowedomains(request.url)) sameorigincheck = true; //Bypass dello skip per gli allowed domains

    return (request.method === 'GET' &&
        !ishandler &&
        // !!(pathPattern.exec(url.pathname)) &&
        sameorigincheck
    );
}
function checkAllowedomains(url) {//Esamino la lista dei domini esterni permessi e ritorno un boolean
    //let request = event.request,   //Non gestisco le richieste diverse da GET col serviceWorker, basta ritornare senza chiamare .respondWith.

    url = new URL(url, self.registration.scope); //skip di tutte le richieste cross origin!
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


//////////////////////////////////////////////////////////////////////////////////////////////////
function addToCache(cacheName, request, response) {
    return caches.open(cacheName)
        .then(cache => cache.put(request, response))
}
function preloadContents(preloadurls) {
    checkonline().then(function (onlinestatus) {
        if (onlinestatus)
            preloadurls.forEach(element => {
                //carichiamo l'url  ( devo passare una lista di pagine html  ) e con lo scrape dei link iterni mettiamo in cache tutte le risorse necesarie
                fetchAndCache(element);
            });
    });
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ESEGUE IL FETCH DELL'URL PASSATO E DEI CONTENUTI NECESSARI A QUESTO - INOLTRE CARICA LE PAGINE LINKATE CON A HREF E ATTRIBUTO PRELOAD
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function fetchAndCache(url) {

    let fetchPromise = fetch(url).then(function (response) {
        if (response.status < 400) {
            console.log('got ' + url);
            var _tmpresponse = response.clone();
            if (_tmpresponse.headers.get("Content-type").indexOf("text/html") != -1)
                caches.open(pagesCacheName).then(cache => {
                    cache.put(url, _tmpresponse.clone());
                });
        }
        //var params = { 'url': url, 'text': response.text() }; //passo la pagina al parser per i contenuti interni
        return response.text();
    }).then(function (text) {

        //let text = textitem.text;
        //var url = response.url;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //PRECARICAMENTO DI TUTTI I TAG IN PAGINA CHE FAANO RICHIESTE , LINK, SCRIPT , AXD..... URL
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //console.log(text);
        var pattern = /(?<=<img[^<]+?src=(\"|'))[^"']+/gi; //cerco contenuto attributo src degli oggetti img
        var pattern1 = /(?<=style=[^<]+?url\s*\(\s*(\"|'))[^"']+/gi; //cerco contenuto attributo url(' degli oggetti dentro attributo style
        var pattern2 = /(?<=<script[^<]+?src=\s*(\"|'))[^"']+/gi;  //elementi script src=
        //elementi link href= o src= che contengono rel="stylesheet"
        var pattern3 = /(?<=<link.*\s*rel=.stylesheet\s*.*href=(\"|'))[^"']+/gi;
        var pattern4 = /(?<=<link.*\s*href=(\"|'))(?=[^>]+?rel=.stylesheet\s*.*)[^"']+/gi;
        var pattern5 = /(?<=<link.*\s*rel=.stylesheet\s*.*src=(\"|'))[^"']+/gi;
        var pattern6 = /(?<=<link.*\s*src=(\"|'))(?=[^>]+?rel=.stylesheet\s*.*)[^"']+/gi;
        let assets = text.match(pattern) || [];
        assets = assets.concat(text.match(pattern1) || []);
        assets = assets.concat(text.match(pattern2) || []);
        assets = assets.concat(text.match(pattern3) || []);
        assets = assets.concat(text.match(pattern4) || []);
        assets = assets.concat(text.match(pattern5) || []);
        assets = assets.concat(text.match(pattern6) || []);
        //console.log(assets);
        assets = Array.from(new Set(assets)); //elimino le ripetizioni
        if (assets) {
            assets.forEach(element => {
                console.log('preloading assets ...');
                let destinationcache = assetsCacheName;
                if (isIncoreCache(element))
                    destinationcache = coreCacheName;

                //Sistema con cache diretta
                //caches.open(destinationcache).then(cache => {
                //    console.log(destinationcache, element);
                //    cache.add(element)
                //        .then(() => {
                //            //cached
                //        }).catch((err) => {
                //            //err
                //        });
                //});

                let url = new URL(element, self.registration.scope);
                var sameorigincheck = (url.origin === self.location.origin);
                if (checkAllowedomains(element)) sameorigincheck = true;
                if (sameorigincheck) {
                    let myHeaders = new Headers();
                    let myInit = {
                        method: 'GET',
                        headers: myHeaders,
                        mode: 'cors'
                    };
                    let myRequest = new Request(element, myInit);
                    fetch(myRequest).then(networkResponse => {
                        if (networkResponse.ok && networkResponse.status === 200) //status in the range 200 to 299 -> to cache only if data is present in response
                        {
                            addToCache(destinationcache, myRequest, networkResponse.clone()); //inseriamo in cache il file richiesto

                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //Qui per i file di tipo css  v� parserizzata la networkResponse.clone() cercando i link dentro url() url("") url('') e aggiungerli alla coda di assets da richiedere
                            //DA FILTRARE I FILE CCS DA PARSERIZZARE ... ALTRIMENTI CARICA UNA BALLA DI FILES!!!! ( QUESTA PARTE E' UN PO PESANTE!! )
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            if (networkResponse.headers.get("Content-type").toLowerCase().indexOf("text/css") != -1) {
                                var newrequest = networkResponse.clone();
                                //  console.log(newrequest.url);
                                newrequest.text().then(function (data) {
                                    var patterncss2 = /(?<={[^<]+?url\s*\(\s*(\"|'|))[^"')]+/gi; //cerco contenuto attributo url(' degli oggetti dentro attributo style
                                    var assetsfromcss = (data.match(patterncss2) || []); //questi sono da agigungere alle risorse ....
                                    //prima l'array � da ridurre eliminando gli elemnti uguali in quanto ci sono molte ripetizioni
                                    let assetsfromcssunique = Array.from(new Set(assetsfromcss));
                                    if (assetsfromcssunique) {
                                        assetsfromcssunique.forEach(element1 => {
                                            let destinationcache1 = assetsCacheName;
                                            if (isIncoreCache(element1))
                                                destinationcache1 = coreCacheName;
                                            let urlcorretto = new URL(element1, self.registration.scope); //gli url da estrarre sono fa fare cosi
                                            //da fare le fetch per questi e metterli in cacHE destinationcache1
                                            // INSERIAMO IN CACHE
                                            var sameorigincheck1 = (urlcorretto.origin === self.location.origin);
                                            if (checkAllowedomains(element1)) sameorigincheck1 = true;
                                            if (sameorigincheck1) {
                                                try {
                                                    let myHeaders1 = new Headers();
                                                    let myInit1 = {
                                                        method: 'GET',
                                                        headers: myHeaders1,
                                                        mode: 'cors'
                                                    };
                                                    let myRequest1 = new Request(urlcorretto, myInit1);
                                                    fetch(myRequest1).then(networkResponse1 => {
                                                        if (networkResponse1.ok && networkResponse1.status === 200) //status in the range 200 to 299 -> to cache only if data is present in response
                                                        {
                                                            addToCache(destinationcache1, myRequest1, networkResponse1.clone()); //inseriamo in cache il file richiesto
                                                            //   console.log('added from css: ' + myRequest1.url ); 
                                                            console.log('preloading assets in css ...');
                                                        }
                                                        return;
                                                    }).catch((err) => { console.log(err); });
                                                } catch (e) { console.log(e); };
                                            }
                                        });
                                    }
                                });
                            }
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            // console.log(destinationcache, element);
                        }
                        return;
                    });
                }
            });

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //RICERCA DEI LINK PRESENTI IN PAGINA!!!! ( DA SELEZIONARE con ATTRIBUTO PRELOAD DEI LINK IN PAGINA !!! ) -> PRENDO SOLO QUELLI CON ATTRIBUTO PRELOAD E FACCIO LA FETCH
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //const regex = /<(.|\n)*?>/g; extracts al tags
        //Ritorna tutti i valori dentro gli href /(?<=<.*\s*href=(\"|'))[^\"']+/gi
        //Ritorna tutti i valori dentro gli href che nel tag hanno preload dopo 
        var patternurl1 = /(?<=<.*\s*href=(\"|'))(?=[^>]+?preload\s*.*)[^\"']+/gi;
        //Ritorna tutti i valori dentro gli href che nel tag hanno preload prima    
        var patternurl2 = /(?<=<.*\s*preload\s*.*href=(\"|'))[^\"']+/gi;
        let pageslinked = text.match(patternurl1) || [];
        pageslinked = pageslinked.concat(text.match(patternurl2) || []);
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        return pageslinked; //Pagine da caricare indicate come link tipo preload nella pagina originaria passata

    });

    //Promise per passare il valore orignale del caller
    let dummypromise = new Promise((resolve, reject) => {
        resolve({ url });
    });

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Promise congiunta ! eseguita AL  termine delle due indicate negli argomenti , serve d avere un contesto CONTEMPORANEO con entrambe le risposte
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    return Promise.all([fetchPromise, dummypromise]).then(function (values) {
        var pagestofetch = values[0];
        var originalurl = values[1];
        //Rimuovere l'url originario della fetchandcache dalla lsita generata se presente !!!! da fare
        //pageslinked - cerco url e lo tolgo se presente !!
        //console.log(pageslinked);
        //(ATTENZIONE AI LOOP DI CARICAMENTO PAGINA!!!! se chiami il fetchAndCache di una pagina che contine il link a se stessa come preload!!!!!! )
        pagestofetch = Array.from(new Set(pagestofetch)); //elimino ripetizioni negli url
        if (pagestofetch)
            pagestofetch.forEach(element => {
                let urlcompleto = new URL(originalurl.url, self.registration.scope); //gli url da estrarre sono fa fare cosi
                if (urlcompleto.href.toLowerCase().indexOf(element.toLowerCase() == -1)) {
                    fetchAndCache(element);
                    console.log('fetching pages ... ' + element);
                }
            });
    });

    // return fetchPromise;
}

//helper chre trova tutti i match in base ad una regular expression in una string
//function getAllMatches(regex, text) {
//    if (regex.constructor !== RegExp) {
//        throw new Error('not RegExp');
//    }
//    var res = [];
//    var match = null;
//    if (regex.global) {
//        while (match = regex.exec(text)) {
//            res.push(match);
//        }
//    }
//    else {
//        if (match = regex.exec(text)) {
//            res.push(match);
//        }
//    }
//}
//////////////////////////////////////////////////////////////////////////////////////////////////
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
//////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////
//MESSAGE PARSER !!! //////////////////////////////////////////////////////////////////////////////
//Mi metto in ascolto dai messaggi inviati dalle pagine web  -> verso il serviceworker!!
//////////////////////////////////////////////////////////////////////////////////////////////////
self.addEventListener('message', event => {
    let data = event.data;

    if (data.command == "pagestoserve") {
        let _pagesToservewithsw = JSON.parse(data.parameters) || [];
        let pagesToservewithswlocal = pagesToservewithsw.concat(_pagesToservewithsw || []);
        //da vedere come usare questa lista pagesToservewithswlocal ( per fare un eventuale filtro di pagine servite )

    }
    if (data.command == "preloadurls") {
        let preloadurls = JSON.parse(data.parameters) || [];
        preloadurls = pagesTofetch.concat(preloadurls || []);
        preloadContents(preloadurls);
    }
    if (data.command == "invalidatecache") {
        //Cache invalidation manager
        invalidationManager.invalidateCache(coreCacheName);
        invalidationManager.invalidateCache(assetsCacheName);
        invalidationManager.invalidateCache(pagesCacheName);
    }

    if (data.command == "recoverstoredcalls") {
        //Vedo se nello storage persistente ci sono chiamate da fare ed in caso le eseguo
        recoverRequestsCache(event);
    }

    //Creo Listener per comando postMessage trimCaches del service Worker
    if (data.command == "trimcache") {
        trimCache(pagesCacheName, 25);
        trimCache(assetsCacheName, 30);
    };


    //}
    //if (data.command == "getcachequeque") {
    //    requestBuffer.updateRequestQueque(data.message);
    //}
});

/*Invio un messagigo dal serviceworker a tutti i client per comandare azioni sula pagina! */
function sendNotificationTopages(response, type, message) {
    clients.matchAll().then(function (clients) {
        clients.forEach(function (client) {
            client.postMessage({
                type: type,
                message: message,
                timestamp: Date.now()
            });
        });
    });
}


//////////////////////////////////////////////////////////////////////////////////////////////////
//////*PUSH NOTIFICATION EVENT SW*/ //////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
self.addEventListener('push', function (event) {
    console.log('[Service Worker] Push Received.');
    if (!(self.Notification && self.Notification.permission === 'granted')) {
        console.log('[Service Worker] Push Not Granted.');
        return;
    }

    // console.log(`[Service Worker] Push Received had this data : "${event.data.text()}"`);
    //https://developer.mozilla.org/en-US/docs/Web/API/ServiceWorkerRegistration/showNotification
    var data = {};
    if (event.data) {
        data = event.data.json();
    }
    const title = data.title;
    const tagvalue = data.tag;
    const options = {
        body: data.message,
        icon: 'images/icon.png',
        badge: 'images/badge.png',
        tag: tagvalue,
        data: { //dati recuperabili nell'evento click sulla notifica
            time: new Date(Date.now()).toString(),
            message: data.message,
            link: data.link
        },
        actions: [
            { action: 'ok', title: 'Yes' },
            { action: 'decline', title: 'No' }
        ]
    };

    event.waitUntil(
        notificationshow(event, tagvalue, title, options)
    );
});

function notificationshow(event, tag, title, options) {

    return new Promise(resolve => {
        self.registration
            .getNotifications({ tag })
            .then(existingNotifications => {
                for (var i = 0; i < existingNotifications.length; i++) {
                    var existingNotification = existingNotifications[i];
                    // existingNotification.close(); //chiudo le notifice presenti
                }
            })
            .then(() => {
                return self.registration
                    .showNotification(title, options);
            })
            .then(resolve);
    });
    //return self.registration.showNotification(title, options);
}

//https://web-push-book.gauntface.com/chapter-05/04-common-notification-patterns/#open-a-window
self.addEventListener('notificationclick', function (event) {

    if (event.action == 'ok') {
        // do something
        console.log('Yes clicked');
    }
    if (event.action == 'decline') {
        // do something
        console.log('No clicked');
    }
    const notificationData = event.notification.data;
    console.log('[Service Worker] Notification click Received. The data notification had the following parameters:');
    Object.keys(notificationData).forEach((key) => {
        console.log(` ${key}: ${notificationData[key]}`);
    });
    event.notification.close();
    event.waitUntil(
        clients.openWindow(notificationData['link'])
    );
});


/*Evento di tracciatura della chiusura della notifica*/
//self.addEventListener('notificationclose', function (event) {
//    const dismissedNotification = event.notification;

//    const promiseChain = notificationCloseAnalytics();
//    event.waitUntil(promiseChain);
//});
//self.addEventListener('pushsubscriptionchange', function (event) {
//    console.log('[Service Worker]: \'pushsubscriptionchange\' event fired.');
//    const applicationServerKey = pushM.urlB64ToUint8Array(pushM.applicationServerPublicKey);
//    event.waitUntil(
//        self.registration.pushManager.subscribe({
//            userVisibleOnly: true,
//            applicationServerKey: applicationServerKey
//        })
//            .then(function (newSubscription) {
//                // TODO: Send to application server
//                console.log('[Service Worker] New subscription: ', newSubscription);
//            })
//    );
//});
/*PUSH NOTIFICATIN EVENT SW*/

