 
  

$(function () {
 
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
 


