

//Queste variabili le inietto dal codebehind
//var GooglePosizione1 = '<%= references.ResMan("Common",Lingua, "GooglePosizione1" ).ToString()  %>';
//var googleurl1 = '<%= references.ResMan("Common",Lingua, "GoogleUrl1" ).ToString()  %>';
//var googlepin1 = '<%= references.ResMan("Common",Lingua, "GooglePin1" ).ToString()  %>';
//var GooglePosizione2 = '<%= references.ResMan("Common",Lingua, "GooglePosizione2" ).ToString()  %>';
//var googleurl2 = '<%= references.ResMan("Common",Lingua, "GoogleUrl2" ).ToString()  %>';
//var googlepin2 = '<%= references.ResMan("Common",Lingua, "GooglePin2" ).ToString()  %>';


var googleposlat1 = "";
var googleposlng1 = "";
var googleposlat2 = "";
var googleposlng2 = "";

function inizializzaVars() {
    if (typeof GooglePosizione1 != 'undefined' && typeof GooglePosizione2 != 'undefined') {
        var res = GooglePosizione1.split(",");
        if (res.length >= 2) {
            googleposlat1 = res[0];
            googleposlng1 = res[1];
        }
        var res2 = GooglePosizione2.split(",");
        if (res2.length >= 2) {
            googleposlat2 = res2[0];
            googleposlng2 = res2[1];
        }
    }



}

$(document).ready(function () {
    inizializzaVars();
    loadScript();
});

function loadScript() {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = 'https://maps.googleapis.com/maps/api/js?key=' + GoogleMapsKey + '&v=3.exp&libraries=places&' +
        'callback=InitializeMap';
    document.body.appendChild(script);
}

var directionsDisplay;
var directionsService;
var map; var map1;
var localmap;
var ns = {}; // a name space

var markersArray = [];
var geocoder;


function initializePlacesAutocomplete(idcontrollo) {

    (function wait() {

        if (typeof google === "object") {

            var input = document.getElementById("txt" + idcontrollo);
            if (input != null) {
                var autocomplete = new google.maps.places.Autocomplete(input);
                ns.oldValue = input.value;

                $("#" + "txt" + idcontrollo).focus(function () {
                    document.getElementById("txt" + idcontrollo).value = '';
                    document.getElementById(idcontrollo).value = '';
                });

                google.maps.event.addListener(autocomplete, 'place_changed', function () {
                    // input.className = '';
                    var place = autocomplete.getPlace();
                    if (!place.geometry) {
                        // Inform the user that the place was not found and return.
                        //input.className = 'notfound';
                        // input.value = "notfoud";
                        return;
                    }
                    var address = '';
                    //if (place.address_components) {
                    //    address = [
                    //        (place.address_components[0] && place.address_components[0].short_name || ''),
                    //       // (place.address_components[1] && place.address_components[1].short_name || ''),
                    //        (place.address_components[2] && place.address_components[2].short_name || '')
                    //    ].join(' ');
                    //}
                    var customformattedaddress = "";
                    for (i in place.address_components) {
                        if (place.address_components[i].types[0] == "locality")
                            customformattedaddress = place.address_components[i].short_name;
                        if (place.address_components[i].types[0] == "administrative_area_level_2")
                            customformattedaddress += ", " + place.address_components[i].short_name;
                        //if (place.address_components[i].types[0] == "administrative_area_level_1")
                        //    customformattedaddress += ", " +place.address_components[i].short_name;
                        if (place.address_components[i].types[0] == "country")
                            customformattedaddress += ", " + place.address_components[i].short_name;
                    }


                    document.getElementById('txt' + idcontrollo).value = customformattedaddress;// place.address_components[0].short_name + ", " + place.address_components[2].short_name + ", " + place.address_components[4].short_name;
                    console.log(address);
                    //formatted_address
                    var coords = place.geometry.location;
                    document.getElementById(idcontrollo).value = coords;
                    document.getElementById(idcontrollo).value = document.getElementById(idcontrollo).value.replace("(", "");
                    document.getElementById(idcontrollo).value = document.getElementById(idcontrollo).value.replace(")", "");



                    //document.getElementById("litTest").innerText = document.getElementById(idcontrollo).value;
                    //if (coords != '')
                    //    document.getElementById('btnCercaLow').click(); //Scateno il click del tasto di ricerca
                });
                // google.maps.event.addDomListener(window, 'load', initialize);
            }
        } else {
            setTimeout(function () { initializePlacesAutocomplete(idcontrollo); }, 50);
        }
    })();

}

function codeAddress(indirizzo, idcontrollo, map) {
    var infowindow;
    var txtcontrol = 'txt' + idcontrollo;
    var address = document.getElementById(txtcontrol).value;
    if (indirizzo != "")
        address = indirizzo;

    geocoder = new google.maps.Geocoder();
    //var latlng = new google.maps.LatLng("41.913008", "12.457123");
    //if (!isNaN(lat) && !isNaN(lng)) {
    //    latlng = new google.maps.LatLng(lat, lng);
    //}

    geocoder.geocode({ 'address': address }, function (results, status) {
        //deleteOverlays();//Toglie tutti i marker dalla mappa
        if (status == google.maps.GeocoderStatus.OK) {

            //alert(results.length);
            map.setCenter(results[0].geometry.location);
            map.setZoom(14);
            infowindow = new google.maps.InfoWindow({});
            infowindow.setContent(results[0].formatted_address);
            addMarker(results[0].geometry.location, '');

            document.getElementById(txtcontrol).value = results[0].formatted_address;

            document.getElementById(idcontrollo).value = results[0].geometry.location;
            document.getElementById(idcontrollo).value = document.getElementById(idcontrollo).value.replace("(", "");
            document.getElementById(idcontrollo).value = document.getElementById(idcontrollo).value.replace(")", "");
            // alert(document.getElementById(idcontrollo).value);
            //__doPostBack(); //aggiorno la pagina passando il valore

        } else {
            alert('Errore localizzazione , motivo: ' + status);
        }
    });
}

function codeAddressOnlydata(indirizzo, idcontrollo) {

    var txtcontrol = 'txt' + idcontrollo;

    var address = document.getElementById(txtcontrol).value;
    if (indirizzo != "")
        address = indirizzo;

    geocoder = new google.maps.Geocoder();
    //var latlng = new google.maps.LatLng("41.913008", "12.457123");
    //if (!isNaN(lat) && !isNaN(lng)) {
    //    latlng = new google.maps.LatLng(lat, lng);
    //}

    geocoder.geocode({ 'address': address }, function (results, status) {
        //deleteOverlays();//Toglie tutti i marker dalla mappa
        if (status == google.maps.GeocoderStatus.OK) {

            document.getElementById(txtcontrol).value = results[0].formatted_address;

            var coords = results[0].geometry.location;
            document.getElementById(idcontrollo).value = coords;
            document.getElementById(idcontrollo).value = document.getElementById(idcontrollo).value.replace("(", "");
            document.getElementById(idcontrollo).value = document.getElementById(idcontrollo).value.replace(")", "");
            // alert(document.getElementById(idcontrollo).value);
            //__doPostBack(); //aggiorno la pagina passando il valore
            // document.getElementById('btnCercaLow').click(); //Scateno il click del tasto di ricerca
        } else {
            alert('Errore localizzazione , motivo: ' + status);
        }
    });
}


//Funzione che inizializza la mappa di google
function InitializeMap() {

    //if (typeof idmapcontainer == 'undefined') return;

    /*https://snazzymaps.com*/


    var styles4 = [{ "featureType": "administrative", "elementType": "all", "stylers": [{ "visibility": "on" }, { "lightness": 33 }] }, { "featureType": "administrative", "elementType": "labels", "stylers": [{ "saturation": "-100" }] }, { "featureType": "administrative", "elementType": "labels.text", "stylers": [{ "gamma": "0.75" }] }, { "featureType": "administrative.neighborhood", "elementType": "labels.text.fill", "stylers": [{ "lightness": "-37" }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#f9f9f9" }] }, { "featureType": "landscape.man_made", "elementType": "geometry", "stylers": [{ "saturation": "-100" }, { "lightness": "40" }, { "visibility": "off" }] }, { "featureType": "landscape.natural", "elementType": "labels.text.fill", "stylers": [{ "saturation": "-100" }, { "lightness": "-37" }] }, { "featureType": "landscape.natural", "elementType": "labels.text.stroke", "stylers": [{ "saturation": "-100" }, { "lightness": "100" }, { "weight": "2" }] }, { "featureType": "landscape.natural", "elementType": "labels.icon", "stylers": [{ "saturation": "-100" }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "saturation": "-100" }, { "lightness": "80" }] }, { "featureType": "poi", "elementType": "labels", "stylers": [{ "saturation": "-100" }, { "lightness": "0" }] }, { "featureType": "poi.attraction", "elementType": "geometry", "stylers": [{ "lightness": "-4" }, { "saturation": "-100" }] }, { "featureType": "poi.park", "elementType": "geometry", "stylers": [{ "color": "#c5dac6" }, { "visibility": "on" }, { "saturation": "-95" }, { "lightness": "62" }] }, { "featureType": "poi.park", "elementType": "labels", "stylers": [{ "visibility": "on" }, { "lightness": 20 }] }, { "featureType": "road", "elementType": "all", "stylers": [{ "lightness": 20 }] }, { "featureType": "road", "elementType": "labels", "stylers": [{ "saturation": "-100" }, { "gamma": "1.00" }] }, { "featureType": "road", "elementType": "labels.text", "stylers": [{ "gamma": "0.50" }] }, { "featureType": "road", "elementType": "labels.icon", "stylers": [{ "saturation": "-100" }, { "gamma": "0.50" }] }, { "featureType": "road.highway", "elementType": "geometry", "stylers": [{ "color": "#c5c6c6" }, { "saturation": "-100" }] }, { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "lightness": "-13" }] }, { "featureType": "road.highway", "elementType": "labels.icon", "stylers": [{ "lightness": "0" }, { "gamma": "1.09" }] }, { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "color": "#e4d7c6" }, { "saturation": "-100" }, { "lightness": "47" }] }, { "featureType": "road.arterial", "elementType": "geometry.stroke", "stylers": [{ "lightness": "-12" }] }, { "featureType": "road.arterial", "elementType": "labels.icon", "stylers": [{ "saturation": "-100" }] }, { "featureType": "road.local", "elementType": "geometry", "stylers": [{ "color": "#fbfaf7" }, { "lightness": "77" }] }, { "featureType": "road.local", "elementType": "geometry.fill", "stylers": [{ "lightness": "-5" }, { "saturation": "-100" }] }, { "featureType": "road.local", "elementType": "geometry.stroke", "stylers": [{ "saturation": "-100" }, { "lightness": "-15" }] }, { "featureType": "transit.station.airport", "elementType": "geometry", "stylers": [{ "lightness": "47" }, { "saturation": "-100" }] }, { "featureType": "water", "elementType": "all", "stylers": [{ "visibility": "on" }, { "color": "#acbcc9" }] }, { "featureType": "water", "elementType": "geometry", "stylers": [{ "saturation": "53" }] }, { "featureType": "water", "elementType": "labels.text.fill", "stylers": [{ "lightness": "-42" }, { "saturation": "17" }] }, { "featureType": "water", "elementType": "labels.text.stroke", "stylers": [{ "lightness": "61" }] }];



    var styles2 = [{ "featureType": "administrative", "elementType": "labels.text.fill", "stylers": [{ "color": "#444444" }] }, { "featureType": "landscape", "elementType": "all", "stylers": [{ "color": "#f2f2f2" }] }, { "featureType": "landscape", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }] }, { "featureType": "landscape.man_made", "elementType": "geometry.fill", "stylers": [{ "hue": "#ffd100" }, { "saturation": "44" }] }, { "featureType": "landscape.man_made", "elementType": "geometry.stroke", "stylers": [{ "saturation": "-1" }, { "hue": "#ff0000" }] }, { "featureType": "landscape.natural", "elementType": "geometry", "stylers": [{ "saturation": "-16" }] }, { "featureType": "landscape.natural", "elementType": "geometry.fill", "stylers": [{ "hue": "#ffd100" }, { "saturation": "44" }] }, { "featureType": "poi", "elementType": "all", "stylers": [{ "visibility": "off" }] }, { "featureType": "road", "elementType": "all", "stylers": [{ "saturation": "-30" }, { "lightness": "12" }, { "hue": "#ff8e00" }] }, { "featureType": "road.highway", "elementType": "all", "stylers": [{ "visibility": "simplified" }, { "saturation": "-26" }] }, { "featureType": "road.arterial", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit", "elementType": "all", "stylers": [{ "visibility": "off" }] }, { "featureType": "water", "elementType": "all", "stylers": [{ "color": "#c0b78d" }, { "visibility": "on" }, { "saturation": "4" }, { "lightness": "40" }] }, { "featureType": "water", "elementType": "geometry", "stylers": [{ "hue": "#ffe300" }] }, { "featureType": "water", "elementType": "geometry.fill", "stylers": [{ "hue": "#ffe300" }, { "saturation": "-3" }, { "lightness": "-10" }] }, { "featureType": "water", "elementType": "labels", "stylers": [{ "hue": "#ff0000" }, { "saturation": "-100" }, { "lightness": "-5" }] }, { "featureType": "water", "elementType": "labels.text.fill", "stylers": [{ "visibility": "off" }] }, { "featureType": "water", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "off" }] }]
    /*Stile 1*/
    var styles1 = [
        {
            "featureType": "landscape",
            "stylers": [
                {
                    "saturation": -100
                },
                {
                    "lightness": 65
                },
                {
                    "visibility": "on"
                }
            ]
        },
        {
            "featureType": "poi",
            "stylers": [
                {
                    "saturation": -100
                },
                {
                    "lightness": 51
                },
                {
                    "visibility": "simplified"
                }
            ]
        },
        {
            "featureType": "road.highway",
            "stylers": [
                {
                    "saturation": -100
                },
                {
                    "visibility": "simplified"
                }
            ]
        },
        {
            "featureType": "road.arterial",
            "stylers": [
                {
                    "saturation": -100
                },
                {
                    "lightness": 30
                },
                {
                    "visibility": "on"
                }
            ]
        },
        {
            "featureType": "road.local",
            "stylers": [
                {
                    "saturation": -100
                },
                {
                    "lightness": 40
                },
                {
                    "visibility": "on"
                }
            ]
        },
        {
            "featureType": "transit",
            "stylers": [
                {
                    "saturation": -100
                },
                {
                    "visibility": "simplified"
                }
            ]
        },
        {
            "featureType": "administrative.province",
            "stylers": [
                {
                    "visibility": "off"
                }
            ]
        },
        {
            "featureType": "water",
            "elementType": "labels",
            "stylers": [
                {
                    "visibility": "on"
                },
                {
                    "lightness": -25
                },
                {
                    "saturation": -100
                }
            ]
        },
        {
            "featureType": "water",
            "elementType": "geometry",
            "stylers": [
                {
                    "hue": "#ffff00"
                },
                {
                    "lightness": -25
                },
                {
                    "saturation": -97
                }
            ]
        }
    ]

    /* Style of the map 1*/
    var styles3 = [
        {
            stylers: [
                { hue: "#00ffe6" },
                { saturation: -20 }
            ]
        }, {
            featureType: "road",
            elementType: "geometry",
            stylers: [
                { lightness: 100 },
                { visibility: "simplified" }
            ]
        }, {
            featureType: "road",
            elementType: "labels",
            stylers: [
                { visibility: "on" }
            ]
        }, {
            featureType: "poi",
            elementType: "labels",
            stylers: [
                { visibility: "off" }
            ]
        }

    ];
    // Create a new StyledMapType object, passing it the array of styles,
    // as well as the name to be displayed on the map type control.
    var styledMap = new google.maps.StyledMapType(styles4, { name: "Styled Map" });
    var styledMap1 = new google.maps.StyledMapType(styles4, { name: "Styled Map" });


    if (document.getElementById(idmapcontainer) != null)// && document.getElementById('directionpanel') != null) 
    {
        directionsDisplay = new google.maps.DirectionsRenderer();
        var latlng = new google.maps.LatLng(googleposlat1, googleposlng1);
        var myOptions =
        {
            zoom: 9,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            scrollwheel: false,
            mapTypeControlOptions: {
                mapTypeIds: [google.maps.MapTypeId.ROADMAP, 'map_style']
            }
        };
        map = new google.maps.Map(document.getElementById(idmapcontainer), myOptions);

        //Associate the styled map with the MapTypeId and set it to display.
        map.mapTypes.set('map_style', styledMap);
        map.setMapTypeId('map_style');

        //var panoramaOptions = {
        //    position: latlng,
        //    pov: {
        //        heading:260,
        //        pitch: 3
        //    }
        //};
        //var panorama = new google.maps.StreetViewPanorama(document.getElementById('map'), panoramaOptions);
        //map.setStreetView(panorama);

        if (document.getElementById(iddirectionpanelcontainer) != null) {
            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById(iddirectionpanelcontainer));
        }
        markicons(map);
    }
    else if (document.getElementById(idmapcontainer1) != null)// && document.getElementById('directionpanel') != null) 
    {
        var latlng = new google.maps.LatLng(googleposlat1, googleposlng1);
        var myOptions =
        {
            zoom: 9,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            scrollwheel: false,
            mapTypeControlOptions: {
                mapTypeIds: [google.maps.MapTypeId.ROADMAP, 'map_style']
            }
        };
        map1 = new google.maps.Map(document.getElementById(idmapcontainer1), myOptions);

        //Associate the styled map with the MapTypeId and set it to display.
        map1.mapTypes.set('map_style', styledMap);
        map1.setMapTypeId('map_style');

        //var panoramaOptions = {
        //    position: latlng,
        //    pov: {
        //        heading:260,
        //        pitch: 3
        //    }
        //};
        //var panorama = new google.maps.StreetViewPanorama(document.getElementById('map'), panoramaOptions);
        //map.setStreetView(panorama);
        markicons(map1);

    }


    if (typeof gpositems !== 'undefined' && gpositems != null) {
        var mapcnt = document.getElementById(idmapcontainerlocal);
        if (mapcnt != null) {
            var latlng = new google.maps.LatLng(googleposlat1, googleposlng1);
            var myOptions =
            {
                zoom: 9,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                scrollwheel: false,
                mapTypeControlOptions: {
                    mapTypeIds: [google.maps.MapTypeId.ROADMAP, 'map_style']
                }
            };
            localmap = new google.maps.Map(mapcnt, myOptions);

            //Associate the styled map with the MapTypeId and set it to display.
            localmap.mapTypes.set('map_style', styledMap1);
            localmap.setMapTypeId('map_style');

            //var panoramaOptions = {
            //    position: latlng,
            //    pov: {
            //        heading:260,
            //        pitch: 3
            //    }
            //};
            //var panorama = new google.maps.StreetViewPanorama(document.getElementById('map'), panoramaOptions);
            //map.setStreetView(panorama);

            markiconslocal(localmap);
        }

    }



}
function markiconslocal(maplocal) {

    var ltlng = [];
    var infowindow;
    var marker;
    var urlmarker = [];
    var testomarker = [];
    if (typeof gpositems != undefined && gpositems != null)
        for (var i = 0; i < gpositems.length; i++) {


            urlmarker.push(gpositems[i]["url"]);
            testomarker.push(gpositems[i]["titolo"] + '<br/><a target="_blank" href="' + urlmarker[i] + '">' +
                'Vedi Mappa Completa/See complete map</a>');
            ltlng.push(new google.maps.LatLng(gpositems[i]["Latitudine1_dts"], gpositems[i]["Longitudine1_dts"]));
            var center = new google.maps.LatLng(gpositems[0]["Latitudine1_dts"], gpositems[0]["Longitudine1_dts"]);
            maplocal.setCenter(center);
            ///////////////////
            //infowindow.setContent(testomarker);
            infowindow = new google.maps.InfoWindow({});
            marker = new google.maps.Marker({
                map: maplocal,
                position: ltlng[i]
            });
            (function (i, marker) {
                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.setContent("<div>" + testomarker[i] + "</div>");
                    infowindow.open(maplocal, marker);
                });
            })(i, marker);
            ///////////////////

            google.maps.event.addListenerOnce(maplocal, 'idle', function () {
                google.maps.event.trigger(marker, 'click');
            });
        }

}
//funzione che mette il punto dell'attività sulla mappa
function markicons(maplocal) {
    var ltlng = [];
    ltlng.push(new google.maps.LatLng(googleposlat1, googleposlng1));
    if (googleposlat2 != '')
        ltlng.push(new google.maps.LatLng(googleposlat2, googleposlng2));
    var center = new google.maps.LatLng(googleposlat1, googleposlng1);
    maplocal.setCenter(center);
    var infowindow;
    var marker;
    var urlmarker = [];
    var testomarker = [];
    for (var i = 0; i < ltlng.length; i++) {

        if (i == 0) {
            urlmarker.push(googleurl1);
            testomarker.push(googlepin1 + '<br/><a target="_blank" href="' + urlmarker[i] + '">' +
                'Vedi Mappa Completa/See complete map</a> ');
        }
        if (i == 1) {
            urlmarker.push(googleurl2);
            testomarker.push(googlepin2 + '<br/><a target="_blank" href="' + urlmarker[i] + '">' +
                'Vedi Mappa Completa/See complete map</a> ');
        }
        ///////////////////
        //infowindow.setContent(testomarker);
        infowindow = new google.maps.InfoWindow({});
        marker = new google.maps.Marker({
            map: maplocal,
            position: ltlng[i]
        });
        (function (i, marker) {
            google.maps.event.addListener(marker, 'click', function () {
                infowindow.setContent("<div>" + testomarker[i] + "</div>");
                infowindow.open(map, marker);
            });
        })(i, marker);
        ///////////////////

        google.maps.event.addListenerOnce(maplocal, 'idle', function () {
            google.maps.event.trigger(marker, 'click');
        });
    }

}


function codeLatLng(map) {
    var input = document.getElementById('latlng').value;
    var latlngStr = input.split(',', 2);
    var lat = parseFloat(latlngStr[0]);
    var lng = parseFloat(latlngStr[1]);
    var latlng = new google.maps.LatLng(lat, lng);

    geocoder = new google.maps.Geocoder();


    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                map.setZoom(8);
                addMarker(results[0].geometry.location, '');
                infowindow.setContent(results[0].formatted_address);
                //infowindow.open(map, marker);
                map.setCenter(results[0].geometry.location);

            } else {
                alert('Posizione non trovata');
            }
        } else {
            alert('Errore localizzazione per: ' + status);
        }
    });
}

function codeLatLng(lat, lng, map) {
    var latlng = null;
    if (!isNaN(lat) && !isNaN(lng)) {
        latlng = new google.maps.LatLng(lat, lng);
    }
    geocoder = new google.maps.Geocoder();


    if (latlng != null) {
        geocoder.geocode({ 'latLng': latlng }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                if (results[0]) {
                    map.setZoom(8);
                    addMarker(results[0].geometry.location, '');
                    infowindow.setContent(results[0].formatted_address);
                    //infowindow.open(map, marker);
                    map.setCenter(results[0].geometry.location);

                } else {
                    alert('Posizione non trovata');
                }
            } else {
                alert('Errore localizzazione per: ' + status);
            }
        });
    }
}

function positionfromLatLng(coordstext, idcontrollo) {

    (function wait() {

        if (typeof google === "object") {
            var txtcontrol = 'txt' + idcontrollo;
            var latlngStr = coordstext.split(',', 2);
            var lat = parseFloat(latlngStr[0]);
            var lng = parseFloat(latlngStr[1]);
            var latlng = new google.maps.LatLng(lat, lng);
            geocoder = new google.maps.Geocoder();

            if (latlng != null) {
                geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            var customformattedaddress = "";
                            for (i in results[0].address_components) {
                                if (results[0].address_components[i].types[0] == "locality")
                                    customformattedaddress = results[0].address_components[i].short_name;
                                if (results[0].address_components[i].types[0] == "administrative_area_level_2")
                                    customformattedaddress += ", " + results[0].address_components[i].short_name;
                                //if (results[0].address_components[i].types[0] == "administrative_area_level_1")
                                //    customformattedaddress += ", " +results[0].address_components[i].short_name;
                                if (results[0].address_components[i].types[0] == "country")
                                    customformattedaddress += ", " + results[0].address_components[i].short_name;
                            }

                            document.getElementById(txtcontrol).value = customformattedaddress;// results[0].formatted_address;

                        } else {
                            // alert('Posizione non trovata');
                        }
                    } else {
                        console.log('Errore localizzazione per: ' + status);
                    }
                });
            }

        } else {
            setTimeout(function () { positionfromLatLng(coordstext, idcontrollo); }, 50);
        }
    })();
}


function addMarker(location, testo, map) {
    marker = new google.maps.Marker({
        draggable: true,
        animation: google.maps.Animation.DROP,
        title: testo,
        zIndex: 1,
        //icon: {
        //    path: google.maps.SymbolPath.BACKWARD_OPEN_ARROW,
        //    scale: 3
        //},
        position: location,
        map: map
    });
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });
    markersArray.push(marker);
}
// Removes the overlays from the map, but keeps them in the array
function clearOverlays() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
    }
}
// Deletes all markers in the array by removing references to them
function deleteOverlays() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }
}
// Shows any overlays currently in the array
function showOverlays() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(map);
        }
    }
}


function Button1_onclick() {
    calcRoute();
}

//funzione che calcola il percorso sulla mappa
function calcRoute() {
    directionsService = new google.maps.DirectionsService();
    var start = document.getElementById('startvalue').value;
    var end = GooglePosizione1;
    var request = {
        origin: start,
        destination: end,
        travelMode: google.maps.DirectionsTravelMode.DRIVING
    };
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(response);
        }
    });
}

