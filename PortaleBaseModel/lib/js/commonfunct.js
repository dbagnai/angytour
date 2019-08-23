
(function ($) {
    $.extend({
        getQueryString: function (name) {
            function parseParams() {
                var params = {},
                    e,
                    a = /\+/g,  // Regex for replacing addition symbol with a space
                    r = /([^&=]+)=?([^&]*)/g,
                    d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
                    q = window.location.search.substring(1);

                while (e = r.exec(q))
                    params[d(e[1])] = d(e[2]);

                return params;
            }

            if (!this.queryStringParams)
                this.queryStringParams = parseParams();

            return this.queryStringParams[name];
        }
    });
})(jQuery);


$.fn.isOnScreen = function () {
    var win = $(window);
    var viewport = {
        top: win.scrollTop(),
        left: win.scrollLeft()
    };
    viewport.right = viewport.left + win.width();
    viewport.bottom = viewport.top + win.height();
    //console.log(viewport);
    var percdelta = 0.2;
    viewport.top -= viewport.top * percdelta;
    viewport.bottom += viewport.bottom * percdelta;
    viewport.left -= viewport.left * percdelta;
    viewport.right += viewport.right * percdelta;

    var bounds = this.offset();
    bounds.right = bounds.left + this.outerWidth();
    bounds.bottom = bounds.top + this.outerHeight();
    try {
        if (bounds.left == bounds.right && bounds.top == bounds.bottom) {
            bounds = this.parent().offset();
            bounds.right = bounds.left + this.parent().outerWidth();
            bounds.bottom = bounds.top + this.parent().outerHeight();
        }
    } catch (e) { }
    //console.log(bounds);
    return (!(viewport.right < bounds.left || viewport.left > bounds.right || viewport.bottom < bounds.top || viewport.top > bounds.bottom));
};
