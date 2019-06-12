var lazy = [];
//registerListener('load', lazyLoad);
//registerListener('scroll',  lazyLoad);
//registerListener('resize', lazyLoad);
(function wait() {
    if (typeof registerListener === "function") {
        registerListener('load', lazyLoad);
        registerListener('scroll', lazyLoad);
        registerListener('resize', lazyLoad);
    } else {
        setTimeout(wait, 300);
    }
})();

function lazyLoad() {
    lazy = $.grep($('.lazy'), function (e) { return $(e).attr('data-src') != undefined });
    //  console.log("found lazy imgs: " + lazy.length);
    for (var i = 0; i < lazy.length; i++) {
        //if (isInViewport(lazy[i])) {
        if ($(lazy[i]).isOnScreen()) {
            //console.log($(lazy[i]));
            if (lazy[i].getAttribute('data-src')) {
                //console.log(lazy[i].getAttribute('data-src'));
                lazy[i].src = lazy[i].getAttribute('data-src');
                lazy[i].removeAttribute('data-src');
            }
        }
    }
    cleanLazy();
}

function cleanLazy() {
    lazy = Array.prototype.filter.call(lazy, function (l) { return l.getAttribute('data-src'); });
}
