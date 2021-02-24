
"use strict";



function RecuperoPassword(username, callback) {
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        data: { 'q': 'recuperapass', 'username': username, 'lng': lng },
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            //callback(result.responseText);
            callback('');
        }
    });
}

function Logoff(callback) {
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        data: { 'q': 'logoffuser', 'lng': lng },
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            //callback(result.responseText);
            callback('');
        }
    });
}


function Loginuser(username, password, callback) {
    $.ajax({
        url: pathAbs + commonhandlerpath,
        contentType: "application/json; charset=utf-8",
        global: false,
        cache: false,
        dataType: "text",
        type: "POST",
        data: { 'q': 'verificalogin', 'username': username, 'password': password, 'lng': lng },
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            //callback(result.responseText);
            callback('');
        }
    });
}
