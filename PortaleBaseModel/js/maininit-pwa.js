"use strict";
jQuery(document).ready(function ($) {
    /*--------GESTIONE MODIFICA MENUZORD CON SCORRIMENTO---------------*/
    $(window).scroll(function () {
        if ($(window).scrollTop() > 0) {
            $('#mainnav').addClass('fixednav-scroll');
            $('#divlogoBrand').removeClass('fulllogobckdark-pwa-start');
        }
        else {
            $('#mainnav').removeClass('fixednav-scroll');
            $('#divlogoBrand').addClass('fulllogobckdark-pwa-start');
        }
    });
});