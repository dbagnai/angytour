"use strict";
var jsonlanguages = "";
var versionforcache = "1";
var percorsocontenuti = "";
var percorsocomune = "";
var percorsoapp = "";
var percorsocdn = "";
var percorsoimg = "";
var percorsoexp = "";
var percorsolistaimmobili = "";
var usecdn = false;
//var lng = "I";
//var pathAbs = "";
var baseresources = '';
var JSONnazioni = "";
var JSONregioni = "";
var JSONprovince = "";
var JSONcomuni = "";
var JSONcar1 = "";
var JSONcar2 = "";
var JSONcar3 = "";
var JSONcategorie = "";
var JSONcategorie2liv = "";
var jsontipologie = "";
var percorsolistadati = "";
var percorsolistaristoranti = "";
var username = "";
var JSONrefmetrature = "";
var JSONrefprezzi = "";
var JSONrefcondizione = "";
var JSONreftipocontratto = "";
var JSONreftiporisorse = "";
var JSONreftipocontatto = "";
var bookinghandlerpath = '/lib/hnd/HandlerBooking.ashx';
var commonhandlerpath = '/lib/hnd/HandlerDataCommon.ashx';
var resourcehandlerpath = '/lib/hnd/HandlerDataImmobili.ashx';
var carrellohandlerpath = '/lib/hnd/CarrelloHandler.ashx';
var newsletterhandlerpath = '/lib/hnd/HandlerNewsletter.ashx';
var feedbackhandlerpath = '/lib/hnd/feedbackHandler.ashx';
var commenthandlerpath = '/lib/hnd/feedbackHandler.ashx';
var pushhandlerpath = '/lib/hnd/HandlerPushnotify.ashx';
var referencesloaded = false;
var promisecalling = false;
var callqueque = [];
var globalObject = {};
var enablescrolltopmem = false;

var utf8ToB64 = function (s) {
    return btoa(unescape(encodeURIComponent(s)));
};
var b64ToUtf8 = function (s) {
    s = s.replace(/\s/g, '');
    return decodeURIComponent(escape(atob(s)));
};
// Polyfills for deprecated escape/unescape() functions
if (!window.unescape) {
    window.unescape = function (s) {
        return s.replace(/%([0-9A-F]{2})/g, function (m, p) {
            return String.fromCharCode('0x' + p);
        });
    };
}
if (!window.escape) {
    window.escape = function (s) {
        var chr, hex, i = 0, l = s.length, out = '';
        for (; i < l; i++) {
            chr = s.charAt(i);
            if (chr.search(/[A-Za-z0-9\@\*\_\+\-\.\/]/) > -1) {
                out += chr; continue;
            }
            hex = s.charCodeAt(i).toString(16);
            out += '%' + (hex.length % 2 != 0 ? '0' : '') + hex;
        }
        return out;
    };
}

 