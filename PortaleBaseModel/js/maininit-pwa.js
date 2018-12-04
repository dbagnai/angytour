"use strict";
jQuery(document).ready(function ($) {
    /*-------------------------------------------------------------*/
    /*--------GESTIONE MODIFICA MENUZORD CON SCORRIMENTO-----------*/
    /*-------------------------------------------------------------*/
    //$(window).scroll(function () {
    //    if ($(window).scrollTop() > 61) {
    //        $('.slidemenu').addClass('slidemenu-fixed');        
    //    }
    //    else {
    //        $('.slidemenu').removeClass('slidemenu-fixed');            
    //    }
    //});

    //var lastScrollTop = 0;
    //$(window).on('scroll', function () {
    //    st = $(this).scrollTop();
    //    if (st < lastScrollTop) {
    //        $('.slidemenu').addClass('slidemenu-fixed');
    //    }
    //    else {
    //        $('.slidemenu').removeClass('slidemenu-fixed');
    //    }
    //    lastScrollTop = st;
    //});

    $('body').on('mousewheel', function (e) {
        if ($(window).scrollTop() > 61)
        if (e.originalEvent.wheelDelta > 0) {
            $('.slidemenu').addClass('slidemenu-fixed');
        }
        else {
            $('.slidemenu').removeClass('slidemenu-fixed');
        }
    });


});

/*---------------------------------------------*/
/*-------- CURSORE MENU TOP PWA ---------------*/
/*---------------------------------------------*/
$(function () {
    if ($('#slide-label-1 li.activeelemntli')[0]) {
        $('.slider .bar').css('margin-left', '0%');
    }
    else if ($('#slide-label-2 li.activeelemntli')[0]) {
        $('.slider .bar').css('margin-left', '25%');
    }
    else if ($('#slide-label-3 li.activeelemntli')[0]) {
        $('.slider .bar').css('margin-left', '50%');
    }
    else if ($('#slide-label-4 li.activeelemntli')[0]) {
        $('.slider .bar').css('margin-left', '75%');
    }
});

/*-------------------------------------------------------------*/
/*------------ SOTTMENU MENU TOP ACTIVE PWA -------------------*/
/*-------------------------------------------------------------*/

/*--- lo accendo se il path della pagina è quello delle NEWS --*/
$(function () {
    if (window.location.pathname == "/I/comunicazioni/comunicazioni") {
        $('ul#ulSubmenu.nav').css('display', 'flex');
    }
    else if ($('.submenutop')[0]) {
        $('ul#ulSubmenu.nav').css('display', 'flex');
    }
    else {
        $('ul#ulSubmenu.nav').css('display', 'none');
    }
});

/*-------- incornicio il 1° <li> quando APRO le NEWS ------------*/
$(function () {
    if ($('.submenutop').length == 0) {
        $('ul#ulSubmenu > li:first-of-type a').addClass("nav-link active submenutop");
        //$('ul#ulSubmenu > li:first-of-type a').css('border', '1px solid');
        //$('ul#ulSubmenu > li:first-of-type a').css('border-color', '#098f8f #098f8f #fff');
        //$('ul#ulSubmenu > li:first-of-type a').css('background-color', '#fff');
        //$('ul#ulSubmenu > li:first-of-type a').css('font-weight', '600');
        //$('ul#ulSubmenu > li:first-of-type a').css('color', '#032f2f');
    }
});

/*-------- incornicio il 1° <li> quando ENTRO nella pagina corrispondente ------------*/
//$(function () {
//    if ($('ul#ulSubmenu > li:first-of-type a.submenutop')[0]) {
//        $('ul#ulSubmenu > li:first-of-type a.submenutop').css('border', '1px solid');
//        $('ul#ulSubmenu > li:first-of-type a.submenutop').css('border-color', '#098f8f #098f8f #fff');
//        $('ul#ulSubmenu > li:first-of-type a.submenutop').css('background-color', '#fff');
//    }
//});


/*-------------------------------------------------------------*/
/*-------------------- SEARCH ON / OFF ------------------------*/
/*-------------------------------------------------------------*/
$(function () {
document.getElementById("iconSearchSwg").onclick = function () {
    $('#searchpwa').animate({ marginBottom: '60px' }, 500);
    };
});

$(function () {
    document.querySelector(".closesearchpwa").onclick = function () {
        $('#searchpwa').animate({ marginBottom: '-10px' }, 500);
    };
});

