"use strict";

function injectandloadgenericcontent(type, container, controlid, visualData, visualPrezzo, iditem) {
    loadref(injectandloadgenericcontentinner, type, container, controlid, visualData, visualPrezzo, iditem, lng);
}
function injectandloadgenericcontentinner(type, container, controlid, visualData, visualPrezzo, iditem) {
    //qui devo visualizzare il titolo
    var templateHtml = pathAbs + "/lib/template/" + "schedadetails.html";
    if (type != null && type != '')
        templateHtml = pathAbs + "/lib/template/" + type;

    //Correggo l'id dei controlli del template per l'inzializzazione dello scroller con id univoca e corretta
    $('#' + container).html('');
    $('#' + container).load(templateHtml, function () {

        //injectOwlGalleryControls(controlid, "plhGallery");
        injectFlexsliderControls(controlid, "plhGallery");

        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });

        var params = {};
        params.container = container;/*Inserisco il nome dle container nei parametri per uso successivo nel binding*/
        params.id = iditem;
        params.visualData = visualData;
        params.visualPrezzo = visualPrezzo;
        globalObject[controlid + "params"] = params;

        CaricaDatageneriContent(controlid);
    });
};
function CaricaDatageneriContent(controlid) {

    //per la lingua usare lng
    var objfiltrotmp = {};
    objfiltrotmp = globalObject[controlid + "params"];

    var functiontocallonend = renderGenericContent;
    //if (enablepager == "true" || enablepager == true)
    //    functiontocallonend = renderIsotopePaged;
    caricaDatiServer(lng, objfiltrotmp, '1', '1', false,
        function (result, callafterfilter) {
            var localObjects = {};

            try {

                if (result !== null && result != '') {
                    var parseddata = JSON.parse(result);
                    var temp = parseddata["resultinfo"];
                    localObjects["resultinfo"] = JSON.parse(temp);
                    var totalrecords = localObjects["resultinfo"].totalrecords;

                    //globalObject[controlid + "pagerdata"].totalrecords = totalrecords;
                    var data = "{ \"datalist\":" + parseddata["data"];
                    data += "}";
                    localObjects["dataloaded"] = data;
                    var datalink = parseddata["linkloaded"];  //link creati presi da tabella
                    //Inserisco i valori nella memoria generale che contiene i valori per tutti i componenti
                    // globalObject[controlid] = localObjects;
                    localObjects["linkloaded"] = JSON.parse(datalink);
                    callafterfilter(localObjects, controlid);
                }
            }
            catch (e) {
                //console.log(e);
            }
        },
        functiontocallonend);
};
function renderGenericContent(localObjects, controlid) {
    bindgenericcontent(controlid, localObjects);//I dati sono già paginati all'origine
};
function bindgenericcontent(el, localObjects) {

    var objcomplete = JSON.parse(localObjects["dataloaded"]);
    var data = objcomplete["datalist"]
    if (!data.length) {
        $('#' + el).html('');
        return;
    }

    var objfiltrotmp = {};
    objfiltrotmp = globalObject[el + "params"];
    var container = objfiltrotmp.container;

    var str = $($('#' + el)[0]).outerHTML();
    var jquery_obj = $(str);
    jquery_obj = $(jquery_obj);
    var htmlout = "";
    var htmlitem = "";
    $('#' + container).html('');
    for (var j = 0; j < data.length; j++) {
        FillBindControls(jquery_obj.wrap('<p>').parent(), data[j], localObjects, "",
            function (ret) {
                //htmlout += $(containeritem).html(ret.html()).outerHTML() + "\r\n";
                $('#' + container).append(ret.html()) + "\r\n";
            });
    }
    $('#' + container).show();
    $('#' + container + 'Title').show();
    //console.log('content generic inject');
    jQuery(document).ready(function () {
        //setTimeout(function (el) { rebuildCarousel(el) }, 3000);
        //rebuildCarousel(el);
        inizializzaFlexsliderGallery(el, container);
    });
    CleanHtml($('#' + container));
    reinitaddthis();

};

function reinitaddthis()
{
    //    addthis.init()
    $('#atstbx').remove();
    //addthis.toolbox('.addthis_toolbox');
    if (typeof addthis !== 'undefined') { addthis.layers.refresh(); }

    //if (window.addthis) {
    //    window.addthis = null;
    //    window._adr = null;
    //    window._atc = null;
    //    window._atd = null;
    //    window._ate = null;
    //    window._atr = null;
    //    window._atw = null;
    //}
   // return $.getScript("http://s7.addthis.com/js/300/addthis_widget.js#pubid=sdive");
}

/*--- FLEXLIDER GALLERY -------http://www.woothemes.com/flexslider/-----------*/
function injectFlexsliderControls(controlid, container) {
    $("#" + container).load("/lib/template/" + "flexslidergallery.html", function () {
        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });
    });
}
function inizializzaFlexsliderGallery(controlid, container) {
    //Plugin: flexslider con funzione di animazione dei messaggi o oggetti sopra
    // ------------------------------------------------------------------
    if ($("#" + controlid + "-main-slider") != null)
        $("#" + controlid + "-main-slider").each(function () {
            var sliderSettings = {
                animation: $(this).attr('data-transition'),
                easing: "swing",
                selector: ".slides > .slide",
                smoothHeight: false,
                // controlsContainer: ".flex-container",
                animationLoop: false,             //Boolean: Should the animation loop? If false, directionNav will received "disable" classes at either end
                slideshow: false,                //Boolean: Animate slider automatically
                //   slideshowSpeed: 4000,           //Integer: Set the speed of the slideshow cycling, in milliseconds
                //  animationSpeed: 600,            //Integer: Set the speed of animations, in milliseconds

                // Usability features
                pauseOnAction: true,            //Boolean: Pause the slideshow when interacting with control elements, highly recommended.
                pauseOnHover: true,            //Boolean: Pause the slideshow when hovering over slider, then resume when no longer hovering
                useCSS: true,                   //{NEW} Boolean: Slider will use CSS3 transitions if available
                touch: true,                    //{NEW} Boolean: Allow touch swipe navigation of the slider on touch-enabled devices
                video: false,                   //{NEW} Boolean: If using video in the slider, will prevent CSS3 3D Transforms to avoid graphical glitches
                sync: "#" + controlid + "-navigation-slider",

                // Primary Controls
                // controlNav: "thumbnails",       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                controlNav: false,       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                directionNav: true,             //Boolean: Create navigation for previous/next navigation? (true/false)
                prevText: "",           //String: Set the text for the "previous" directionNav item
                nextText: "",               //String: Set the text for the "next" directionNav item

                start: function (slider) {
                    //hide all animated elements
                    slider.find('[data-animate-in]').each(function () {
                        $(this).css('visibility', 'hidden');
                    });

                    //animate in first slide
                    slider.find('.slide').eq(1).find('[data-animate-in]').each(function () {
                        $(this).css('visibility', 'hidden');
                        if ($(this).data('animate-delay')) {
                            $(this).addClass($(this).data('animate-delay'));
                        }
                        if ($(this).data('animate-duration')) {
                            $(this).addClass($(this).data('animate-duration'));
                        }
                        $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                        $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                            function () {
                                $(this).removeClass($(this).data('animate-in'));
                            }
                        );
                    });
                },
                before: function (slider) {
                    //hide next animate element so it can animate in
                    slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                        $(this).css('visibility', 'hidden');
                    });
                },
                after: function (slider) {
                    //hide animtaed elements so they can animate in again
                    slider.find('.slide').find('[data-animate-in]').each(function () {
                        $(this).css('visibility', 'hidden');
                    });

                    //animate in next slide
                    slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                        if ($(this).data('animate-delay')) {
                            $(this).addClass($(this).data('animate-delay'));
                        }
                        if ($(this).data('animate-duration')) {
                            $(this).addClass($(this).data('animate-duration'));
                        }
                        $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                        $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                            function () {
                                $(this).removeClass($(this).data('animate-in'));
                            }
                        );
                    });
                    /* auto-restart player if paused after action */
                    if (!slider.playing) {
                        slider.play();
                    }

                }
            };

            var sliderNav = $(this).attr('data-slidernav');
            if (sliderNav !== 'auto') {
                sliderSettings = $.extend({}, sliderSettings, {
                    //  controlsContainer: '.flex-container',
                    manualControls: sliderNav + ' li a'
                });
            }
            $(this).flexslider(sliderSettings);
        });
    //    $("#" + controlid + "-main-slider").resize(); //make sure height is right load assets loaded
    if ($("#" + controlid + "-navigation-slider") != null)
        $("#" + controlid + "-navigation-slider").flexslider({
            animation: "slide",
            controlNav: false,
            animationLoop: false,
            slideshow: false,
            itemWidth: 120,
            itemHeight: 80,
            itemMargin: 5,
            asNavFor: "#" + controlid + "-main-slider"
        });

}
/*--- FLEXLIDER GALLERY ------------------*/

//owl carousel GALLERY -------http://owlgraphic.com/owlcarousel/demos/manipulations.html-----------------------------------
function injectOwlGalleryControls(controlid, container) {
    
    $("#" + container).load("/lib/template/" + "owlsliderfoto.html", function () {

        $('#' + container).find("[id^=replaceid]").each(function (index, text) {
            var currentid = $(this).prop("id");
            var replacedid = currentid.replace('replaceid', controlid);
            $(this).prop("id", replacedid);
        });

        //Qui puoi fare inizializzazione controlli su allegati
        $("#" + controlid + "owl-gallery").attr("mybind", 'Id');
    });
}
function rebuildCarousel(controlid) {
    graphicElementsinit(controlid);
}
function graphicElementsinit(controlid) {
    //console.log('reinit owl gallery');
    /*http://owlgraphic.com/owlcarousel/demos/manipulations.html */
    if (typeof $("#" + controlid + "owl-gallery").owlCarousel == 'function')
        $("#" + controlid + "owl-gallery").owlCarousel({
            navigation: false,
            pagination: true,
            slideSpeed: 300,
            paginationSpeed: 400,
            singleItem: true,
            rewindSpeed: 0,
            autoPlay: false,
            lazyLoad: true,
            autoHeight: true
            //  navigationText: [
            //"<i class='fa fa-2x fa-chevron-left'><</i>",
            //"<i class='fa fa-2x fa-chevron-right'>></i>"
            // ]
        });
    //Custom Navigation Events
    $("#" + controlid + "next").click(function () {
        $("#" + controlid + "owl-gallery").trigger('owl.next');
    })
    $("#" + controlid + "prev").click(function () {
        $("#" + controlid + "owl-gallery").trigger('owl.prev');
    })
}
function graphicElementsreinit(controlid) {

    if ($("#" + controlid + "owl-gallery").data('owlCarousel') != null)
        $("#" + controlid + "owl-gallery").data('owlCarousel').reinit({
            navigation: true,
            pagination: false,
            slideSpeed: 300,
            paginationSpeed: 400,
            singleItem: true,
            rewindSpeed: 0,
            autoPlay: false
        });
    else
        sendmessage('error with carousel');

    //$("#owl-gallery").trigger('refresh.owl.carousel', { 
    //});
    //$("#owl-gallery").owlCarousel({
    //    items: 1,
    //    nav: true,
    //    smartSpeed: 400

    //});

}
    //owl carousel GALLERY ------------------------------------------
