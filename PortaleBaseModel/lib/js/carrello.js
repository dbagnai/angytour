var carrellohandlerpath = '/lib/hnd/CarrelloHandler.ashx';


$(document).ready(function () {
    connectCarrelloEvents();
});

function connectCarrelloEvents() {
    //////////APERTURA DROPDOWN CARRELLO///////////////////////////////////////////
    //Funzione eseguita all'apertura del dropdown con classe triggerdata
    //$('div.btn-group button.triggerdata').click(function (e) {
    //    $(this).dropdown("toggle");
    //    $(this).parent().find("[id*='ContainerCarrelloDetails']")[0].innerText = "";
    //    var codiceordine = $(this).parent().find("[id*='ContainerCarrelloDetails']").attr("title");
    //    var contenitoredestinazione = $(this).parent().find("[id*='ContainerCarrelloDetails']");
    //    //Caricamento ajax carrello!
    //    ShowCurrentCarrello(contenitoredestinazione, codiceordine);
    //    e.preventDefault();
    //    //Reimposta la funzione che fà apire il dropdown
    //    $(this).click(function (ev) {
    //        $(this).dropdown("toggle");
    //        e.preventDefault();
    //        return false;
    //    });
    //    return false;
    //});
    //Evita che il dropdown si chiuda cliccandodi sopra
    $('.dropdown-menu').click(function (e) {
        e.stopPropagation();
    });
    /////////////INSERIMENTO NEL CARRELLO (INUTILE CON INIEZIONE JS!!!!!)/////////////////////////////////////////////
    $('button.trigcarrello').click(function (e) {
        var title = $(this).attr("title");
        InserisciCarrello(title);
        e.preventDefault();
    });
    //////////////////////////////////////////////////////////////////////
}

function GetCarrelloList(el) {
    $(el).parent().find("[id*='ContainerCarrelloDetails']")[0].innerText = "";
    //if ($(el).parent().find("[id*='ContainerCarrelloDetails']")[0].innerText == "") {
    var codiceordine = $(el).parent().find("[id*='ContainerCarrelloDetails']").attr("title");
    var contenitoredestinazione = $(el).parent().find("[id*='ContainerCarrelloDetails']");
    //Caricamento ajax carrello!
    ShowCurrentCarrello(contenitoredestinazione, codiceordine);
    //}
}
function ShowCurrentCarrello(contenitoredestinazione, codiceordine) {
    $.ajax({
        destinationControl: contenitoredestinazione,
        type: "POST",
        url: carrellohandlerpath + "?Lingua=" + lng + "&Azione=show",
    // contentType: "application/json; charset=utf-8",
    // dataType: "json",
    data: '{codice: "' + codiceordine + '" , Lingua: "' + lng + '" }',
    success: function (data) {
        OnSuccessShowcarrello(data, this.destinationControl);
    },
    failure: function (response) {
        alert(response);
    }
});
}
function OnSuccessShowcarrello(response, destination) {
    // alert(destination[0].id);//Controllo destinazione html
    //destination.append("<li>" + response.d + "</li>");
    // $(destination).Clear();
    destination.append("<li>" + response + "</li>");
}

//function InserisciCarrello(testo) {
//    var res = testo.split(",", 3);
//    var idprodotto = res[0];
//    var lingua = res[1];
//    var username = res[2];
//    var contenitoredestinazione = '';//$("#litTotalHigh");
//    AddCurrentCarrello(contenitoredestinazione, idprodotto, lingua, username);
//}
//function AddCurrentCarrello(contenitoredestinazione, idprodotto, lingua, username) {
//    $.ajax({
//        destinationControl: contenitoredestinazione,
//        type: "POST",
//        url: carrellohandlerpath + "?Lingua=" + lingua + "&Azione=add",
//    // contentType: "application/json; charset=utf-8",
//    // dataType: "json",
//        data: '{codice: "' + idprodotto + '" , Lingua: "' + lingua + '" , Username: "' + username + '" }',
//    success: function (data) {
//        OnSuccessAddcarrello(data, this.destinationControl);
//    },
//    failure: function (response) {
//        //  alert(response);
//    }
//});
//}
//function OnSuccessAddcarrello(response, destination) {
//    //$(".totalItems").empty();
//    //$(".totalItems").append(response);
//    __doPostBack();
//}
function InserisciCarrelloNopostback(testo) {
    var res = testo.split(",", 3);
    var idprodotto = res[0];
    var lingua = res[1];
    var username = res[2];
    var contenitoredestinazione = '';//$("#litTotalHigh");
    AddCurrentCarrelloNopostback(contenitoredestinazione, idprodotto, lingua, username);
}
function AddCurrentCarrelloNopostback(contenitoredestinazione, idprodotto, lingua, username) {
    $.ajax({
        destinationControl: contenitoredestinazione,
        type: "POST",
          url: carrellohandlerpath + "?Lingua=" + lingua + "&Azione=add",
    // contentType: "application/json; charset=utf-8",
    // dataType: "json",
    data: '{codice: "' + idprodotto + '" , Lingua: "' + lingua  +'" , Username: "' + username + '" }',
    success: function (data) {
        OnSuccessAddcarrelloNopostback(data, this.destinationControl);
    },
    failure: function (response) {
        //  alert(response);
    }
});
}
function OnSuccessAddcarrelloNopostback(response, destination) {
    //$(".totalItems").empty();
    //$(".totalItems").append(response);
    //__doPostBack();

    $.ajax({
        destinationControl: '',
        type: "POST",
        url: carrellohandlerpath + "?Lingua=" + lng + "&Azione=showtotal",
    // contentType: "application/json; charset=utf-8",
    // dataType: "json",
    data: '{empty: "" }',
    success: function (data) {
        $("#containerCarrello").find("[id*='litTotalHigh']")[0].innerText = data;
    },
    failure: function (response) {
        //  alert(response);
    }
});

}




