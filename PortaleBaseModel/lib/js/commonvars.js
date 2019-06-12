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

 