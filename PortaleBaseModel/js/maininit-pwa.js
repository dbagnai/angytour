"use strict";
jQuery(document).ready(function ($) {
    /*--------GESTIONE MODIFICA MENUZORD CON SCORRIMENTO---------------*/
    $(window).scroll(function () {
        if ($(window).scrollTop() > 0) {
            //$('#mainnav').addClass('bckColor1');        
        }
        else {
            //$('#mainnav').removeClass('bckColor1');            
        }
    });

});


/*-------- CURSORE MENU TOP PWA ---------------*/
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

/*-------- CURSORE SOTTMENU MENU TOP ACTIVE PWA ---------------*/
$(function () {
    if ($('.submenutop')[0]) {
        $('ul#ulSubmenu > li:first-of-type a').css('border', 'none');
    }
    else {
        $('ul#ulSubmenu > li:first-of-type a').css('border', '1px solid');
        $('ul#ulSubmenu > li:first-of-type a').css('border-color', '#098f8f #098f8f #fff');
        $('ul#ulSubmenu > li:first-of-type a').css('background-color', '#fff');
        $('ul#ulSubmenu > li:first-of-type a').css('font-weight', '600');
        $('ul#ulSubmenu > li:first-of-type a').css('color', '#032f2f');
    }
});
$(function () {
    if ($('ul#ulSubmenu > li:first-of-type a.submenutop')[0]) {
        $('ul#ulSubmenu > li:first-of-type a.submenutop').css('border', '1px solid');
        $('ul#ulSubmenu > li:first-of-type a.submenutop').css('border-color', '#098f8f #098f8f #fff');
        $('ul#ulSubmenu > li:first-of-type a.submenutop').css('background-color', '#fff');
    }
});