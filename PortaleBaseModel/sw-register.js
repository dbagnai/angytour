//This is the service worker with the Cache-first network
//https://developer.mozilla.org/en-US/docs/Web/API/Service_Worker_API/Using_Service_Workers
//Add this below content to your HTML page, or add the js file to your page at the very top to register service worker
if (navigator.serviceWorker.controller) {
  console.log('[PWA Builder] active service worker found, no need to register')
} else {

//Register the ServiceWorker
  navigator.serviceWorker.register('/sw.js', {
    scope: '/I/blog/'
  }).then(function(reg) {
    console.log('Service worker has been registered for scope:'+ reg.scope);
  }).catch(function(error) {
    // registration failed
    console.log('Registration failed with ' + error);
  });
}