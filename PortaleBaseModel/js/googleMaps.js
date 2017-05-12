

//Queste variabili le inietto dal codebehind
//var GooglePosizione1 = '<%= GetGlobalResourceObject("Common",  "GooglePosizione1" ).ToString()  %>';
//var googleurl1 = '<%= GetGlobalResourceObject("Common",  "GoogleUrl1" ).ToString()  %>';
//var googlepin1 = '<%= GetGlobalResourceObject("Common",  "GooglePin1" ).ToString()  %>';
//var GooglePosizione2 = '<%= GetGlobalResourceObject("Common",  "GooglePosizione2" ).ToString()  %>';
//var googleurl2 = '<%= GetGlobalResourceObject("Common",  "GoogleUrl2" ).ToString()  %>';
//var googlepin2 = '<%= GetGlobalResourceObject("Common",  "GooglePin2" ).ToString()  %>';

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
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAI9pleffZQ2n3y81P7fxemgGkVfZwbD4g&v=3.7&' +
        'callback=InitializeMap';
    document.body.appendChild(script);
}

var directionsDisplay;
var directionsService;
var map; var map1;



//Funzione che inizializza la mappa di google
function InitializeMap() {

    if (typeof idmapcontainer == 'undefined') return;

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
    var styledMap = new google.maps.StyledMapType(styles2, { name: "Styled Map" });


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

}

//funzione che mette il punto dell'attività sulla mappa
function markicons(maplocal) {
    var ltlng = [];
    ltlng.push(new google.maps.LatLng(googleposlat1, googleposlng1));
    if (googleposlat2 != '')
        ltlng.push(new google.maps.LatLng(googleposlat2, googleposlng2));



    var center = new google.maps.LatLng(googleposlat1, googleposlng1);
    maplocal.setCenter(center);
    for (var i = 0; i < ltlng.length; i++) {

        var urlmarker = "";
        if (i == 0)
            urlmarker = googleurl1;
        if (i == 1)
            urlmarker = googleurl2;

        var marker = new google.maps.Marker({
            map: maplocal,
            position: ltlng[i],
            url: urlmarker
        });
        //if (!infowindow) {
        var infowindow = new google.maps.InfoWindow();
        //}
        var testomarker = "";
        if (i == 0)
            testomarker = googlepin1;
        if (i == 1)
            testomarker = googlepin2;
        testomarker += '<br/><a target="_blank" href="' + marker.url + '">' +
     'Vedi Mappa Completa/See complete map</a> ';

        //   var contentString =
        //'<div class="popup">' +
        //'<h2 id="berlin">Berlin</h2>' +
        //'<p>Center of Berlin</b><br/>' +
        //'<small><b>Lat.</b> 52.520196, <b>Lon.</b> 13.406067</small></p>' +
        //'<a target="_blank" href="' + marker.url + '">' +
        //'Vedi Mappa Completa/See complete map</a> ' +
        //'</div>';
        // infowindow.setContent(contentString);
        infowindow.setContent(testomarker);

        //  infoWindow.setPosition(ltlng[i]);
        (function (i, marker) {
            google.maps.event.addListener(marker, 'click', function () {
                //window.open(marker.url, '_blank');
                infowindow.open(maplocal, marker);
            });
        })(i, marker);

        //google.maps.event.addListenerOnce(maplocal, 'idle', function () {
        //    infowindow.open(maplocal, marker);
        //});
        google.maps.event.addListenerOnce(maplocal, 'idle', function () {
            google.maps.event.trigger(marker, 'click');
        });
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

