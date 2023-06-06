
$.extend($.easing, {
    easeInOutCubic: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    }
});

$.fn.outerFind = function (selector) {
    return this.find(selector).addBack(selector);
};

(function ($, sr) {
    // debouncing function from John Hann
    // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
    var debounce = function (func, threshold, execAsap) {
        var timeout;

        return function debounced() {
            var obj = this, args = arguments;
            function delayed() {
                if (!execAsap) func.apply(obj, args);
                timeout = null;
            };

            if (timeout) clearTimeout(timeout);
            else if (execAsap) func.apply(obj, args);

            timeout = setTimeout(delayed, threshold || 100);
        };
    }
    // smartresize
    jQuery.fn[sr] = function (fn) { return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

})(jQuery, 'smartresize');

$.isMobile = function (type) {
    var reg = [];
    var any = {
        blackberry: 'BlackBerry',
        android: 'Android',
        windows: 'IEMobile',
        opera: 'Opera Mini',
        ios: 'iPhone|iPad|iPod'
    };
    type = 'undefined' == $.type(type) ? '*' : type.toLowerCase();
    if ('*' == type) reg = $.map(any, function (v) { return v; });
    else if (type in any) reg.push(any[type]);
    return !!(reg.length && navigator.userAgent.match(new RegExp(reg.join('|'), 'i')));
};


$(function () {


    // init
    $(window).resize().scroll();

    // smooth scroll
    $(document).click(function (e) {
        try {
            var target = e.target;
            //if ($(target).parents().hasClass('extTestimonials1')) {
            //    return;
            //}
            do {
                if (target.hash) {
                    var useBody = /#bottom|#top/g.test(target.hash);
                    $(useBody ? 'body' : target.hash).each(function () {
                        e.preventDefault();
                        // in css sticky navbar has height 64px
                        var stickyMenuHeight = $('.mbr-navbar--sticky').length ? 64 : 0;
                        var goTo = target.hash == '#bottom'
                            ? ($(this).height() - $(window).height())
                            : ($(this).offset().top - stickyMenuHeight);
                        //Disable Accordion's and Tab's scroll
                        if ($(this).hasClass('panel-collapse') || $(this).hasClass('tab-pane')) { return };
                        //$('html, body').stop().animate({
                        //    scrollTop: goTo
                        //}, 500, 'easeInOutCubic');
                        // Animate the scroll to the destination...
                        $('html, body').animate(
                            {
                                scrollTop: $(target.hash).offset().top // Scroll to this location.
                            }, {
                            // Set the duration long enough to allow time
                            // to lazy load the elements.
                            duration: 1000,

                            // At each animation step, check whether the target has moved.
                            step: function (now, fx) {

                                // Where is the target now located on the page?
                                // i.e. its location will change as images etc. are lazy loaded
                                var newOffset = $(target.hash).offset().top;

                                // If where we were originally planning to scroll to is not
                                // the same as the new offset (newOffset) then change where
                                // the animation is scrolling to (fx.end).
                                if (fx.end !== newOffset)
                                    fx.end = newOffset;
                            }
                        }
                        );

                    });
                    break;
                }
            } while (target = target.parentNode);
        } catch (e) {
            // throw e;
        }
    });



});



