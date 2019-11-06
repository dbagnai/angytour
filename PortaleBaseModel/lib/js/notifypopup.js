"use strict";

//$(document).ready(function () {
//    PopupShow();
//    PopupShow2();
//});

function PopupShowBottomRight(idtextcontainter) {
    var idtextcontainter = idtextcontainter || "popuphtml";
    //var position = position || ".center";
    //Abilitazione PopUp se richiesto da queerstring
    //var valuefromqs = getUrlVars()["subrq"];
    //if (valuefromqs == 'request')
    //    CookiesPolicy.SetCookie('subscriptiorequest', valuefromqs, '259200', '/');
   var htmlpopup = "";
    var cookie = CookiesPolicy.GetCookie('subscriptiorequest');
    var popopen = false;
    if (cookie != 'subscribed') popopen = true;
    if ($("#" + idtextcontainter).html() != undefined) {
        htmlpopup = $("#" + idtextcontainter ).html();
    }
    if (htmlpopup != "" && popopen) { 
        $(".notifications.bottom-right").show(); //svuoto il contenuto
        $(".notifications.bottom-right").html(''); //svuoto il contenuto
        $(".notifications.bottom-right").notify({
            //message: { html: false, text: 'This is a message.' },
            message: { html: htmlpopup },
            transition:'none',
            fadeOut: { enabled: false, delay: 0 },
            type: 'custom1',
            onClose: closedpopup
        }).show(); // for the ones that aren't closable and don't fade out there is a .hide() function.
    } else $(".notifications.bottom-right").hide();
}
function closedpopup() {
    //Set cookie duration 0.002 = 2 giorni ( 0.0008 = 1 minuto )
    //CookiesPolicy.SetCookie('subscriptiorequest', 'subscribed', '0.002', '/');
    CookiesPolicy.SetCookie('subscriptiorequest', 'subscribed', '0.0008', '/');
}


function closedpopup2() {
    //Set cookie duration 0.002 = 2 giorni ( 0.0008 = 1 minuto )
    //CookiesPolicy.SetCookie('subscriptiorequest', 'subscribed', '0.002', '/');
    CookiesPolicy.SetCookie('subscriptiorequest2', 'subscribed', '0.0008', '/');
    $(".notifications.center").hide();
}
function PopupShowCenter(idtextcontainter) {
    var idtextcontainter = idtextcontainter || "popuphtml2";
    //Abilitazione PopUp se richiesto da queerstring
    //var valuefromqs = getUrlVars()["subrq"];
    //if (valuefromqs == 'request')
    //    CookiesPolicy.SetCookie('subscriptiorequest', valuefromqs, '259200', '/');
    var htmlpopup = "";
    var cookie = CookiesPolicy.GetCookie('subscriptiorequest2');
    var popopen = false;
    if (cookie != 'subscribed') popopen = true;
    if ($("#" + idtextcontainter).html() != undefined) {
        htmlpopup = $("#" + idtextcontainter).html();
    }
    if (htmlpopup != "" && popopen) {
        $(".notifications.center").show();
        $('.notifications.center').html('');
        $('.notifications.center').notify({
            //$('.bottom-right').notify({
            //message: { html: false, text: 'This is a message.' },
            message: { html: htmlpopup },
            fadeOut: { enabled: false, delay: 0 },
            transition: 'none',
            type: 'custom2',
            onClose: closedpopup2
        }).show(); // for the ones that aren't closable and don't fade out there is a .hide() function.
    } else $(".notifications.center").hide();

}


//In your script block   
//function getUrlVars() {
//    var vars = [], hash;
//    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
//    for (var i = 0; i < hashes.length; i++) {
//        hash = hashes[i].split('=');
//        vars.push(hash[0]);
//        vars[hash[0]] = hash[1];
//    }
//    return vars;
//}