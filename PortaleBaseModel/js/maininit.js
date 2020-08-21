"use strict";


jQuery(document).ready(function ($) {
    //$(".sticker").sticky({ topSpacing: 120 });
    //setInterval(function () { if ($('#divnavigazioneJs0').isOnScreen()) { $(".sticker").unstick() } }, 1000);

    //https://github.com/tombigel/detect-zoom
    setInterval(function () {
        //var zoom = detectZoom.zoom();
        //var device = detectZoom.device();
        var device = "";
        var zoom = document.body.clientWidth / window.innerWidth;
        if (zoom > 1) {
            $("#mainnav").removeClass('fixednav');
            $("#mainnav").addClass('freenav');
            $("#mainnav1").removeClass('fixednav1');
            $("#mainnav1").addClass('freenav1');

            $("#divTop").removeClass('fixedtop');
            $("#divTop").addClass('freetop');
        } else {
            $("#mainnav").addClass('fixednav');
            $("#mainnav").removeClass('freenav');
            $("#mainnav1").addClass('fixednav1');
            $("#mainnav1").removeClass('freenav1');
            $("#divTop").removeClass('freetop');
            $("#divTop").addClass('fixedtop');
        }
        //$("#testzoom")
        //.html("Zoom:" + zoom + " device:" + device);
    }, 1000);

    jQuery("#menuzord").show();
    jQuery("#menuzord").menuzord({
        align: "left",
        scrollable: true,
        indicatorFirstLevel: "<i class='fa fa-angle-down'></i>",
        //indicatorFirstLevel: "<i class='fas fa-2x fa-sort-up'></i>",
        indicatorSecondLevel: "<i class='fa fa-angle-right'></i>",
        mobilebuttonclass: "mobilebtn",
        mobilebuttontext: "<i class='fa fa-bars'></i>"
    });
    //jQuery("#menuzord1").menuzord({
    //    align: "left",
    //    scrollable: true,
    //    indicatorFirstLevel: "<i class='fa fa-angle-down'></i>",
    //    indicatorSecondLevel: "<i class='fa fa-angle-right'></i>",
    //    mobilebuttonclass: "mobilebtn",
    //    mobilebuttontext:"CERCA"
    //});

    /* ---------------------------------------------- /*
  * SEZIONE AOS E SHOW ON SCROLL TOOL
  /* ---------------------------------------------- */
    /*-------- INIZIALIZZATORE AOS ---------------*/
    //AOS.init();
    //AOS.init({
    //    disable: 'mobile'
    //});
    AOS.init({
        disable: function () {
            var maxWidth = 800;
            return window.innerWidth < maxWidth;
        }
    });
    const sectionEls = document.querySelectorAll(".show-on-scroll");
    const options = {
        rootMargin: "0% 0% -70% 0%"
    };
    const observer = new IntersectionObserver(entries => {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add("is-visible");
            } else {
                entry.target.classList.remove("is-visible");
            }
        });
    }, options);
    sectionEls.forEach(el => observer.observe(el));
    /* ---------------------------------------------- /*
* fine SEZIONE AOS E SHOW ON SCROLL TOOL
/* ---------------------------------------------- */

    /*--------GESTIONE MODIFICA MENUZORD CON SCORRIMENTO---------------*/
    $(window).scroll(function () {
        if ($(window).scrollTop() > 0) {
            //        $('#mainnav').addClass('bckColor1');
            $('#divlogo').removeClass();
            $('.menuzord-menu').addClass('scrolled');
            $('#divlogoBrand').addClass('shrinklogo');
            //$('#mainnav').addClass('fixednav-scroll');
            //$('#divlogoBrand').removeClass('fulllogobckdark-pwa-start');

            //        $('#divlogo').addClass('shrinklogobck');

            //        if (sliderPresent) {
            //            $('#menuzord').addClass('dark');
            //            $('#menuzord').removeClass('white');
            //        }
            $('#divMessage').hide();

            ///////AFFIX MANAGER       //////////////////////////////////////////////////////////////////
            //remove affix per una colonna in base alla posizione della barra laterale
            //console.log("scrolltop :" + $(window).scrollTop() + "windows height:" + ($(document).height() - $(window).height()) + " footer height: " + $('footer').height());
            if ($(window).scrollTop() + $('footer').height() > ($(document).height() - $(window).height()))
                $(".affixfinder").removeClass('affix');
            else
                $(".affixfinder").addClass('affix');
            ///////////////////////////////////////////////////////////////////////////////////////////////
        }
        else {
            //        $('#mainnav').removeClass('bckColor1');
            $('#divlogo').removeClass();

            $('#divlogo').addClass('fulllogobckdark');
            $('#divMessage').show();

            $('.menuzord-menu').removeClass('scrolled');
            $('#divlogoBrand').removeClass('shrinklogo');
            //$('#mainnav').removeClass('fixednav-scroll');
            //$('#divlogoBrand').addClass('fulllogobckdark-pwa-start');

            //        if (sliderPresent) {
            //            $('#menuzord').addClass('white');
            //            $('#menuzord').removeClass('dark');
            //            $('#divlogo').removeClass();
            //            $('#divlogo').addClass('fulllogobck');
            //        }
        }
    });


    $('.loader').hide();

    /*--------GESTIONE MODIFICA MENUZORD CON SCORRIMENTO Home fullscrean ---------------*/
    if (true) {
        var ishome = false;
        if (window.location.pathname.toLowerCase() == "/" || window.location.pathname.toLowerCase() == "/home"
            || window.location.pathname.toLowerCase() == "/it/home" ||
            window.location.pathname.toLowerCase() == "/en/home" ||
            window.location.pathname.toLowerCase() == "/ru/home" ||
            window.location.pathname.toLowerCase() == "/fr/home" ||
            window.location.pathname.toLowerCase() == "/index.aspx") ishome = true;
        if (ishome) {
            $('.fixedtop').addClass('fixedtop-home');
            $('.fixednav').addClass('fixednav-home');
            $('.fulllogobckdark').addClass('fulllogobckdark-home');
            $('.menuzord.white .menuzord-menu > li > a, .menuzord.white .menuzord-menu > li > a span').addClass('menu-home');
            $('.menuzord.white .menuzord-menu > li > a.buttonmenu span').addClass('buttonmenu-span-home');
            $('.buttonmenu, a .buttonmenu').addClass('buttonmenu-home');
            $('.bckColor2').addClass('bckColor2-home');
            $('.catalogo-menu ul').addClass('catalogo-menu-ul-home');
            $('.catalogo-menu ul').removeClass('visible-ok');
            $('.catalogo-menu ul.dropdown li').addClass('catalogo-menu-ul-li-home');
            /*$('.menuzord-menu li.catalogo-menu .indicator').addClass('indicator-home')*/;
            $('.menuzord-menu .indicator').addClass('indicator-home');
            $('.headerspacer').addClass('headerspacer-home');
            $('.menuzord-menu ul.dropdown').addClass('dropdown-home');
            $('.megamenu').addClass('dropdown-home');
            $(window).scroll(function () {
                if ($(window).scrollTop() > 0) {
                    $('.fixedtop').removeClass('fixedtop-home');
                    $('.fixednav').removeClass('fixednav-home');
                    $('.fulllogobckdark').removeClass('fulllogobckdark-home');
                    $('.menuzord.white .menuzord-menu > li > a, .menuzord.white .menuzord-menu > li > a span').removeClass('menu-home');
                    $('.menuzord.white .menuzord-menu > li > a.buttonmenu span').removeClass('buttonmenu-span-home');
                    $('.buttonmenu, a .buttonmenu').removeClass('buttonmenu-home');
                    $('.bckColor2').removeClass('bckColor2-home');
                    $('.catalogo-menu ul').removeClass('catalogo-menu-ul-home');
                    $('.catalogo-menu ul').addClass('visible-ok');
                    $('.catalogo-menu ul ul').removeClass('visible-ok');
                    $('.headerspacer').removeClass('headerspacer-home');
                    $('.catalogo-menu ul.dropdown li').removeClass('catalogo-menu-ul-li-home');
                    //$('.menuzord-menu li.catalogo-menu .indicator').removeClass('indicator-home');
                    $('.menuzord-menu .indicator').removeClass('indicator-home');
                    $('.menuzord-menu ul.dropdown').removeClass('dropdown-home');
                    $('.megamenu').removeClass('dropdown-home');
                }
                else {
                    $('.fixedtop').addClass('fixedtop-home');
                    $('.fixednav').addClass('fixednav-home');
                    $('.fulllogobckdark').addClass('fulllogobckdark-home');
                    $('.menuzord.white .menuzord-menu > li > a, .menuzord.white .menuzord-menu > li > a span').addClass('menu-home');
                    $('.menuzord.white .menuzord-menu > li > a.buttonmenu span').addClass('buttonmenu-span-home');
                    $('.buttonmenu, a .buttonmenu').addClass('buttonmenu-home');
                    $('.bckColor2').addClass('bckColor2-home');
                    $('.catalogo-menu ul').addClass('catalogo-menu-ul-home');
                    $('.catalogo-menu ul').removeClass('visible-ok');
                    $('.catalogo-menu ul.dropdown li').addClass('catalogo-menu-ul-li-home');
                    //$('.menuzord-menu li.catalogo-menu .indicator').addClass('indicator-home');
                    $('.menuzord-menu .indicator').addClass('indicator-home');
                    $('.headerspacer').addClass('headerspacer-home');
                    $('.menuzord-menu ul.dropdown').addClass('dropdown-home');
                    $('.megamenu').addClass('dropdown-home');
                }
            });
        }
    }

    /*----------------------- inizializzazione elementi portfolio con masonry.js   --------------- */
    (function wait() {
        if (typeof $.fn.masonry !== 'undefined') {
            //if (typeof $.masonry == 'function') {
            var $grid = $('.grid').masonry({
                // set itemSelector so .grid-sizer is not used in layout
                itemSelector: '.grid-item',
                // use element for option
                columnWidth: '.grid-sizer',
                percentPosition: true,
                //gutter: 20
            });
            $grid.imagesLoaded().progress(function () {
                $grid.masonry('layout');
            });

        } else {
            setTimeout(wait, 300);
        }
    })();

    /*HAMMER INIT TO AVOID VERTICAL SWIPE ON SCROLLERS*/
    //(function wait() {
    //    if (typeof Hammer !== 'undefined') {
    //        $('.item.box').each(function () {
    //            //$('.owl-carousel-v1').each(function () {
    //            var options = {
    //                preventDefault: true
    //            };
    //            var mc = new Hammer(this, options);
    //            mc.on("panup pandown", function (e) {
    //                //mc.on("panup pandown swipeup swipedown", function (e) {
    //                //var id = e.target.id;
    //                //console.log('event vertical detecded');
    //            });
    //        });

    //    } else {
    //        setTimeout(wait, 300);
    //    }
    //})();

    /*------------------------------------------------------- */
    //InitIsotope();
    /*-----------------------*/
    //$.datepicker.setDefaults($.datepicker.regional['']);
    jQuery('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
    inizializzasimplestars(); /*inizio gli oggetti rating per la visualizzazione con le telle*/

    /*Jquery Carousel*/
    //jQuery('.carousel').carousel({
    //    interval: 3000,
    //    pause: 'hover'
    //});
    /*-----------------------*/

    /*Owl Carousel*/
    OwlCarousel.initOwlCarousel();
    /*-----------------------*/

    if (Function('/*@cc_on return document.documentMode===10@*/')()) {
        document.documentElement.className += ' ie10';
    }

    /* Show testimonial after loading */
    // $('.testimonial-item').css('visibility', 'visible');

    centeringBullets();

    /* ---------------------------------------------- /*
		 * Youtube video background from RIval
	/* ---------------------------------------------- */
    //$(function () {
    //    $(".video-player").mb_YTPlayer();
    //});
    $(".video-player").mb_YTPlayer();
    //  'onStateChange': onPlayerStateChange
    setInterval(function () {
        testifvideoinview();
        loadifisinview();
    }, 1000);
    $('#video-play').click(function (event) {
        event.preventDefault();
        if ($(this).hasClass('fa-play')) {
            $('.video-player').playYTP();
        } else {
            $('.video-player').pauseYTP();
        }
        $(this).toggleClass('fa-play fa-pause');
        return false;
    });
    $('#video-volume').click(function (event) {
        event.preventDefault();
        $('.video-player').toggleVolume();
        $(this).toggleClass('fa-volume-off fa-volume-up');
        return false;
    });
    /* ---------------------------------------------- /*
     * A jQuery plugin for fluid width video embeds
    /* ---------------------------------------------- */
    $('body').fitVids();


    if ($("html").hasClass("lt-ie9")) {
        //bread crumb last child fix for IE8
        $('.breadcrumbs li:last-child').addClass('last-child');
        $('.navigation > li:last-child').addClass('last-child-nav');
        $('.flickr_badge_wrapper .flickr_badge_image').addClass('flicker-ie');
        $('.flickr_badge_wrapper .flickr_badge_image:nth-child(3n+1)').addClass('last-child-flicker');
        $('.content-style3 ').css('width', '100%').css('width', '-=28px');
        $('.section-subscribe input[type=text]').css('width', '100%').css('width', '-=40px');
        $('.blog-search .blog-search-input').css('width', '100%').css('width', '-=40px');
        $('.tab').click(function () {
            setTimeout(function () {
                $('.content-style3 ').css('width', '100%').css('width', '-=28px');
                $('.section-subscribe input[type=text]').css('width', '100%').css('width', '-=40px');
            }, 500);
        });
    };

    //Click
    $('.searchbox .searchbox-icon,.searchbox .searchbox-inputtext').bind('click', function () {
        var $search_tbox = $('.searchbox .searchbox-inputtext');
        $search_tbox.css('width', '120px');
        $search_tbox.focus();
        $('.searchbox', this).addClass('searchbox-focus');
    });

    //Blur
    $('.top-bar .searchbox-inputtext,body').bind('blur', function () {
        var $search_tbox = $('.searchbox .searchbox-inputtext');
        $search_tbox.css('width', '0px');
        $('.searchbox', this).removeClass('searchbox-focus');
    });

    //if (document.getElementById('contact_map')) {
    //    google.maps.event.addDomListener(window, 'load', contactusMap);
    //}

    /* Portfolio PrettyPhoto */
    if (!ismobileview)
        $("a[rel^='prettyPhoto']").prettyPhoto({
            //theme: 'facebook',
            animation_speed: 'fast', /* fast/slow/normal */
            slideshow: 5000, /* false OR interval time in ms */
            autoplay_slideshow: false, /* true/false */
            opacity: 0.80,  /* Value between 0 and 1 */
            social_tools: ''
        });


    /* Info Box Listeners */
    $('.alert a.alert-remove').click(function () {
        $(this).parents('.alert').first().fadeOut();
        return false;
    });

    //place holder fallback
    //$('input, textarea').placeholder();
    //process video posts
    embed_video_processing();
    //init tooltip tipsy
    $('.social-media-icon,.tool-tip').tipsy({ gravity: 's', fade: true, offset: 5 });
    //Remove tipsy tooltip event from image overlay elements
    $('.item_img_overlay_content .social-media-icon,.top-bar .social-media-icon').unbind('mouseenter');
    //Callout Box And Message Box Mobile Button
    $('.message-box ,.callout-box').each(function () {
        var $box = $(this);
        var $button = $box.find(".btn");
        $box.append('<button href="' + $button.attr("href") + '" class="' + $button.attr("class") + ' btn-mobile">' + $button.html() + '</button>');
    });
    // stickyMenu();
    // addhovereffecttobootstrapnav();
});



var agents = ['android', 'webos', 'iphone', 'ipad', 'blackberry', 'Android', 'webos', , 'iPod', 'iPhone', 'iPad', 'Blackberry', 'BlackBerry'];
var ismobileview = false;
for (var i in agents) {

    if (navigator.userAgent.split(agents[i]).length > 1) {
        ismobileview = true;

    }
}

///////////////////////////FUNZIONI VARIE///////////////////////////////////////
var loaded = false, timeout = 20000;//loaded flag for timeout
setTimeout(function () {
    if (!loaded) {
        hideLoading();
    }
}, timeout);

$(window).load(function () {
    loaded = true;
    centeringBullets();
    hideLoading();
    var $masonryElement = $('#masonry-elements');
    $masonryElement.isotope({
        transformsEnabled: false,
        masonry: {
            columnWidth: 270,
            gutterWidth: 25
        }
    });

});

/*DropDown Menu Hover effect*/
function addhovereffecttobootstrapnav() {      
    //Add Hover effect to menus OF BOOTSTRAP
    $('ul.nav li.dropdown').hover(function () {
        $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeIn();
    }, function () {
        $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeOut();
    });
    //////////////////////////////////////
}

/* Loading functions */
function hideLoading() {
    $('.loading-container').remove();
    $('.hide-until-loading').removeClass('hide-until-loading');
}

/**
 * This function used to add some features to easytabs  out of the box.
 * @param selector
 */
function easyTabsZeina(selector, options) {
    var $ref = $(selector);

    $('.tab-container').css('visibility', 'visible');
    options = options || {};
    options['animationSpeed'] = options['animationSpeed'] || 'fast';
    $ref.easytabs(options).bind('easytabs:midTransition', function () {
        var $this = $(this), activeLink = $this.find('a.active'), offset = activeLink.offset();
        $this.find('.section-tab-arrow').css('left', ((offset.left + (activeLink.outerWidth()) / 2) - 7) + 'px');
    });

    //trigger event on init
    $ref.trigger('easytabs:midTransition');
    $(window).load(function () {
        $ref.trigger('easytabs:midTransition');
    });
}

/* Contaact Map */
var map;
function contactusMap() {
    var myLatlng, mapOptions, marker;
    var myLatlng = new google.maps.LatLng(-37.817590, 144.965188);

    mapOptions = {
        zoom: 11,
        center: myLatlng,
        scrollwheel: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById('contact_map'), mapOptions);

    marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        title: ''
    });
}

/**
 * Form Validation Helper

function form_validation(selector) {
    var errorContainerOpen = '<div class="span1 error_container" ><div class="error-box">',
            errorContainerClose = '<i class="icon-remove"></i></div></div>';

    $(selector).validate({
        errorClass: "input_error",
        errorElement: "span",
        success: function (label, element) {
        }
    });
}
 */

/**
 * Embed Video
 */
function embed_video_processing() {
    var youtube_template = '<iframe src="http://www.youtube.com/embed/{{id}}" frameborder="0" allowfullscreen=""  width="100%" height="100%" allowfullscreen></iframe>',
        vimeo_template = '<iframe src="http://player.vimeo.com/video/{{id}}?color=ffffff" frameborder="0" allowfullscreen=""  width="100%" height="360"></iframe>',
        soundcloud_template = '<iframe src="https://w.soundcloud.com/player/?url={{id}}" frameborder="0" allowfullscreen=""  width="100%" height="166"></iframe>',
        template, id;

    $('.blog-post-youtube,.blog-post-vimeo,.blog-post-soundcloud').each(function () {
        id = false;

        //youtube
        if ($(this).hasClass('blog-post-youtube')) {
            id = getYoutubeId($(this).attr('href'));
            template = youtube_template;
        }
        //vimeo
        else if ($(this).hasClass('blog-post-vimeo')) {
            id = getVimeoId($(this).attr('href'));
            template = vimeo_template;
        }
        //sound clound
        else if ($(this).hasClass('blog-post-soundcloud')) {
            id = $(this).attr('href');
            template = soundcloud_template;
        }

        if (id !== false) {
            //process the template
            $(this).replaceWith(template.replace('{{id}}', id));
        }
    });
}

/***
 * Get youtube url.
 *
 * @param url
 * @returns {*}
 */
function getYoutubeId(url) {
    var regExp = /^.*((youtu.[\w]{1,3}\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#\&\?]*).*/;
    var match = url.match(regExp);
    if (match && match[7].length == 11) {
        return match[7];
    } else {
        return false;
    }
}
/***
 * Get vimeo url.
 *
 * @param url
 * @returns {*}
 */
function getVimeoId(url) {
    var regExp = /http:\/\/(www\.)?vimeo.com\/(\d+)($|\/)/;
    var match = url.match(regExp);

    if (match) {
        return match[2];
    } else {
        return false;
    }
}

/*
 * Zeina Accordion
 * Written specially for Zeina Theme
 */
function zeinaAccordion(selector) {
    $(document).on('click', selector + ' .accordion-row .title,' + selector + ' .accordion-row .open-icon', function () {
        var me = this,
            accordion = $(this).parents('.accordion'),
            $prev,
            $accRow = $(this),
            $accTitle = $accRow.parent(), $this, icon, desc, title, activeRow,
            $accRow = $accTitle.parent(),
            toggle = accordion.data('toggle') == 'on' ? true : false;

        if (toggle === true) {
            icon = $accTitle.find('.open-icon');
            desc = $accTitle.find('.desc');
            title = $accTitle.find('.title');

            if ($accTitle.find('.close-icon').length > 0) {
                desc.slideUp('fast');
                icon.removeClass('close-icon');
                title.removeClass('active');
            }
            else {
                desc.slideDown('fast');
                icon.addClass('close-icon');
                title.addClass('active');
            }
        }
        else {
            $accRow.find('.accordion-row').each(function () {
                $this = $(this);
                icon = $this.find('.open-icon');
                desc = $this.find('.desc');
                title = $this.find('.title');

                /* if this the one which is clicked , slide up  */
                if ($accTitle[0] != this) {
                    desc.slideUp('fast');
                    icon.removeClass('close-icon');
                    title.removeClass('active');
                }
                else {
                    desc.slideDown('fast');
                    icon.addClass('close-icon');
                    title.addClass('active');
                }
            });
        }
    });

    // active div
    $(selector).each(function () {
        var $this = $(this), icon, desc, title, activeRow,
            activeIndex = parseInt($this.data('active-index')),
            activeIndex = activeIndex < 0 ? false : activeIndex;

        if (activeIndex !== false) {
            activeRow = $this.find('.accordion-row').eq(activeIndex);
            icon = activeRow.find('.open-icon');
            desc = activeRow.find('.desc');
            title = activeRow.find('.title');

            desc.slideDown('fast');
            icon.addClass('close-icon');
            title.addClass('active');
        }
    });
}
function relayoutIsotope() {

    var worksgrid = $('.works-grid');
    var worksgrid_mode;
    if (worksgrid.hasClass('works-grid-masonry')) {
        worksgrid_mode = 'masonry';
    } else {
        worksgrid_mode = 'fitRows';
    }
    worksgrid.imagesLoaded(function () {
        worksgrid.isotope('reLayout');
    });
}

function InitIsotope() {
    var worksgrid = $('.works-grid');
    var worksgrid_mode;
    if (worksgrid.hasClass('works-grid-masonry')) {
        worksgrid_mode = 'masonry';
    } else {
        worksgrid_mode = 'fitRows';
    }

    worksgrid.imagesLoaded(function () {
        worksgrid.isotope({
            layoutMode: worksgrid_mode,
            itemSelector: '.work-item'
        });
    });

    $('#filters a').click(function () {
        $('#filters .current').removeClass('current');
        $(this).addClass('current');
        var selector = $(this).attr('data-filter');

        worksgrid.isotope({
            filter: selector,
            animationOptions: {
                duration: 750,
                easing: 'linear',
                queue: false
            }
        });

        return false;
    });

    $('.post-masonry').imagesLoaded(function () {
        $('.post-masonry').masonry();
    });

    $('.portfolio-filter-container a').click(function () {
        $cont.isotope({
            filter: this.getAttribute('data-filter')
        });

        return false;
    });

    var lastClickFilter = null;
    $('.portfolio-filter a').click(function () {
        if (lastClickFilter === null) {
            $('.portfolio-filter a').removeClass('portfolio-selected');
        }
        else {
            $(lastClickFilter).removeClass('portfolio-selected');
        }

        lastClickFilter = this;
        $(this).addClass('portfolio-selected');
    });
}


/* Sticky Menu */
function stickyMenu() {
    $('#header').addClass('sticky-header');
    $('#VerticalSpacer').attr('style', 'height: 196px');
    //$(window).scroll(function () {
    //    if ($(window).scrollTop() > 0) {
    //        $('#header').addClass('sticky-header');
    //        //  $('.sticky-navigation').fadeIn();
    //        $('#VerticalSpacer').attr('style', 'height: 135px');
    //    }
    //    else {
    //        $('#header').removeClass('sticky-header');
    //        //  $('.sticky-navigation').fadeOut();
    //        $('#VerticalSpacer').attr('style', '');
    //    }
    //});
}

/* Centering Bullets */
function centeringBullets() {
    //Bullets center fixing in revolution slide
    $('.simplebullets,.slider-fixed-frame .home-bullets').each(function () {
        var $this = $(this), w = $this.width();
        $this.css('margin-left', -(w / 2) + 'px');
    });
}

/*FUNZIONE CHE TESTA SE UN OGGETTO E'DEFINITO O MENO*/
$.fn.presence = function () {
    return this.length !== 0 && this;
}

function loadifisinview() {
    var ret = "";
    $('.loadinview').each(function (i, obj) {
        if ($(this).isOnScreen()) {
            //  $(this).append('IN VIEW');
        }
        else {
            //   $(this).append('OUT VIEW');
        }
    });
    //  $("#testzoom").html(ret);
}

/*FUNZIONE CHE STOPPA O FA' RIPARTIRE YTPLAYER SE IL VIDEO E' VISBILE A SCHERMO ( USA LA CLASSE SPECIFICHA )*/
function testifvideoinview() {
    var ret = "";
    $('.visibleplaystop').each(function (i, obj) {
        if ($(this).isOnScreen()) {
            ret += i + "inview; ";
            if ($(this).find('.video-player').presence() && document.hasFocus())
                $(this).find('.video-player').playYTP();
        }
        else {
            ret += i + "outview; ";
            if ($(this).find('.video-player').presence()) {
                $(this).find('.video-player').pauseYTP();
            }
        }
    });

    //  $("#testzoom").html(ret);
}




/* ---------------------------------------------- /*
     * ANIMATION CARRELLO
    /* ---------------------------------------------- */

//$("#carrello-click").mousedown(function () {

//        //$('#carrello-start').addClass('lancia-carrello');
//        //$("#disco").animate({ right: '270px' });
//        //$("#disco").animate({ top: '0px' });
//        //$("#disco").animate({ left: '+=270', top: '-=270' }, 1000);

//    //$('#carrello-start').animate({ right: '270px', top: '0px' }, 800);

//    //$('#carrello-start')
//    //    .css({
//    //        left: "+=50px",
//    //        zindex: "99999999",
//    //        position: "fixed",
//    //    });

//});



/* ---------------------------------------------- /*
     * ANIMATION CARRELLO tipo lancia
    /* ---------------------------------------------- */


//var offsets = $('.fa-shopping-cart').offset();
//var carrellotop = offsets.top - 20;
//var carrelloleft = offsets.left - 0;

//var $follower = $("#carrello-go"),
//    mouseX = 0,
//    mouseY = 0;

//$("li:nth-child(n+1) #carrello-click").click(function (e) {
//    mouseX = e.pageX - 20;
//    mouseY = e.pageY - 20;
//    $follower.stop().css({ left: mouseX, top: mouseY });

//    $('#carrello-go').addClass('carrello-go-launch');

//    $('#carrello-go').animate({ left: carrelloleft, top: carrellotop }, 750);

//});


/* ---------------------------------------------- /*
     * ANIMATION CARRELLO tipo rotate
    /* ---------------------------------------------- */


//----- ruota carrello -----

$(function () {
    $(".button-carrello, .button-carrello1").click(function () {
        $('.button-carrello-animate').addClass('carrello-go-rotate');
        setTimeout(RemoveClass, 1000);
    });
    function RemoveClass() {
        $('.button-carrello-animate').removeClass("carrello-go-rotate");
    }
});

////----- aggiungi n° colli -----

//$(function () {
//    $('#carrello-click .button-carrello, .button-carrello1, #carrellos1plus').on("click", function () {
//        addOneToThings();
//    })
//});

//function addOneToThings() {
//    var countSpan = $('.count');
//    var currentThings = parseInt(countSpan.text());
//    currentThings++;
//    countSpan.text(currentThings);
//}

////----- sottrai n° colli -----

//$(function () {
//    $('#carrellos1minus').on("click", function () {
//        subOneToThings();
//    })
//});

//function subOneToThings() {
//    var countSpan = $('.count');
//    var currentThings = parseInt(countSpan.text());
//    currentThings--;
//    countSpan.text(currentThings);
//}

/* ---------------------------------------------- /*
     * MOVE CURSORE PWA
    /* ---------------------------------------------- */

//if ($('div.mydivclass').length) {}


/* ---------------------------------------------- /*
     * ANIMA ELEMENTO SCROLL CON ELEMENTSTOSHOW
    /* ---------------------------------------------- */

//// Detect request animation frame
//var scroll = window.requestAnimationFrame ||
//    // IE Fallback
//    function (callback) { window.setTimeout(callback, 1000 / 60) };
//var elementsToShow = document.querySelectorAll('.show-on-scroll');

//function loop() {

//    Array.prototype.forEach.call(elementsToShow, function (element) {
//        if (isElementInViewport(element)) {
//            element.classList.add('is-visible');
//        } else {
//            element.classList.remove('is-visible');
//            }
//    });

//    scroll(loop);
//}

//// Call the loop for the first time
//loop();

//// Helper function from: http://stackoverflow.com/a/7557433/274826
//function isElementInViewport(el) {
//    // special bonus for those using jQuery
//    if (typeof jQuery === "function" && el instanceof jQuery) {
//        el = el[0];
//    }
//    var rect = el.getBoundingClientRect();
//    return (
//        (rect.top <= 0
//            && rect.bottom >= 0)
//        ||
//        (rect.bottom >= (window.innerHeight || document.documentElement.clientHeight) &&
//            rect.top <= (window.innerHeight || document.documentElement.clientHeight))
//        ||
//        (rect.top >= 0 &&
//            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight))
//    );
//}

/* ---------------------------------------------- /*
     * ANIMA ELEMENTO SCROLL IMAGE CON OBSERVER
    /* ---------------------------------------------- */
//const sectionEls = document.querySelectorAll(".show-on-scroll");

//const options = {
//    //root: document.body,
//    rootMargin: "-25% 0% -25% 0%",
//    threshold: .93
//};


//const observer = new IntersectionObserver(entries => {
//    entries.forEach(function (entry) {
//        if (entry.isIntersecting) {
//            entry.target.classList.add("is-visible-text");
//        } else {
//            entry.target.classList.remove("is-visible-text");
//        }
//    });
//}, options);

//sectionEls.forEach(el => observer.observe(el));


/* ---------------------------------------------- /*
     * ANIMA ELEMENTO SCROLL TEXT CON OBSERVER ELEMENT 1 => scritte
    /* ---------------------------------------------- */
const stickyContainers = document.querySelectorAll('.show-on-scroll');
const io_options = {
    //root: document.body,
    //rootMargin: '0px 0px -390px 0px',
    rootMargin: '0px 0px -55% 0px',
    threshold: 0
};
const io_observer = new IntersectionObserver(io_callback, io_options);

stickyContainers.forEach(element => {
    io_observer.observe(element);
});

function io_callback(entries, observer) {
    entries.forEach(entry => {
        entry.target.classList.toggle('is-visible-text', entry.isIntersecting);
    });
}

/* ---------------------------------------------- /*
     * ANIMA ELEMENTO SCROLL TEXT CON OBSERVER ELEMENT 2 => ingrandimento banner
    /* ---------------------------------------------- */
const stickyContainers2 = document.querySelectorAll('.show-on-scroll2');
const io_options2 = {
    //root: document.body,
    //rootMargin: '0px 0px -390px 0px',
    rootMargin: '0px 0px -77.5% 0px',
    threshold: 0
};
const io_observer2 = new IntersectionObserver(io_callback2, io_options2);

stickyContainers2.forEach(element => {
    io_observer2.observe(element);
});

function io_callback2(entries, observer) {
    entries.forEach(entry => {
        entry.target.classList.toggle('is-visible-text', entry.isIntersecting);
    });
}

/* ---------------------------------------------- /*
     * ANIMA ELEMENTO SCROLL TEXT/IMAGE CON OBSERVER ELEMENT 3 => ingrandimento banner
    /* ---------------------------------------------- */
const stickyContainers3 = document.querySelectorAll('.show-on-scroll3');
const io_options3 = {
    rootMargin: '0px 0px -68% 0px',
    threshold: 0

};
const io_observer3 = new IntersectionObserver(io_callback3, io_options3);

stickyContainers3.forEach(element => {
    io_observer3.observe(element);
});

function io_callback3(entries, observer) {
    entries.forEach(entry => {
        entry.target.classList.toggle('is-visible-text', entry.isIntersecting);
    });
}