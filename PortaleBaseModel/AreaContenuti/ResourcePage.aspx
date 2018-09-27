<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="ResourcePage.aspx.cs" Inherits="AreaContenuti_ResourcePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <script type="text/javascript">

        var templatehtml = null;
        var dataarray = [];
        var dataarrayUpdated = [];
        var dataarrayInsert = null;
        var newitem = null;
        jQuery(document).ready(function () {
            loadref(getdata, 'I');
        });

        function getdata() {
            $(".loader").show();
            var $select = $('#ddlLingue');
            if ($('#ddlLingue option').length == 0) {
                fillDDL("ddlLingue", jsonlanguages, "Seleziona Lingua", "", "I");
            }

            //Chiamre server per leggere dati e Riempire ->
            // jsondetailsobj = {};
            var objfiltrotmp = {};
            objfiltrotmp["Gruppo"] = "";
            caricaParametriRisorseServer($select.val(), objfiltrotmp,
                function (result, callafterfilter) {
                    try {
                        $(".loader").fadeOut("slow");

                        // qui devi riempire correttamente il datarray
                        //jsondetailsobj = JSON.parse(result);
                        //dataarray[0] = jsondetailsobj;
                        dataarrayInsert = null;
                        dataarrayUpdated = [];
                        dataarray = JSON.parse(result);
                        if (dataarray.length > 0) {
                            newitem = getCopy(dataarray[0]);
                            SetInsertValues();
                        }
                        console.log(dataarray);
                        console.log(dataarrayInsert);
                        console.log(dataarrayUpdated);

                        callafterfilter("row1", "row1", dataarray)

                    }
                    catch (e) { }
                },
                databind);

        }

        function SetInsertValues() {
            var select1 = $('#newchiave');
            var select2 = $('#newgruppo');
            var select3 = $('#newvalore');

            select1.attr("value", newitem["Chiave"] != null ? newitem["Chiave"] : '');
            select2.attr("value", newitem["Gruppo"] != null ? newitem["Gruppo"] : '');
            select3.attr("value", newitem["Valore"] != null ? newitem["Valore"] : '');

        }

        /* UPDATE DEL RECORD -----------------------------------------------------------*/
        function updateData() {
            $(".loader").fadeIn("slow");

            var jsondetailstxt = JSON.stringify(dataarrayUpdated);

            if (dataarrayInsert != null && dataarrayInsert != []) // per insert
                jsondetailstxt = JSON.stringify(dataarrayInsert);


            if (jsondetailstxt != '' && jsondetailstxt != null) {
                $.ajax({
                    type: "POST",
                    url: pathAbs + commonhandlerpath,
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    data: {
                        'q': 'updateresources', 'itemdata': jsondetailstxt
                    },
                    success: OnCompleteupdateDetail,
                    error: OnFailupdateDetail
                });
            }

        }
        function OnCompleteupdateDetail(result) {
            try {
                getdata(); //Ricarico il tutto


                sendmessage('Richiesta Aggiornamento Completata.', result);
                $("#results").html('Richiesta Aggiornamento Completata.' + result);
            }
            catch (e) {
                $(".loader").fadeOut("slow");

                //  sendmessage(e, '');
                $("#results").html(e);

            }
        }
        function OnFailupdateDetail(result) {
            $(".loader").fadeOut("slow");

            //sendmessage('Richiesta Aggiornamento Fallita.', result.responseText);
            $("#results").html('Richiesta Aggiornamento Fallita.' + ' ' + result.responseText);

        }
        /* Procedure update  ---------------------- */

        function deletedata() { }

        function insertdata() {
            var $select = $('#ddlLingue');
            if (newitem == null) {
                sendmessage('Errore valore inserimanto non impostato.', '');
                return;
            }

            newitem["Lingua"] = $select.val();
            /*Controllo se tutti valori ok*/
            if (newitem["Lingua"] == null || newitem["Lingua"] == '') {
                sendmessage('Errore Lingua non impostata.', '');
                return;
            }
            if (newitem["Gruppo"] == null || newitem["Gruppo"] == '') {
                sendmessage('Errore Gruppo non impostato.', '');
                return;
            }
            if (newitem["Chiave"] == null || newitem["Chiave"] == '') {
                sendmessage('Errore Chiave non impostata.', '');
                return;
            }
            dataarrayInsert = [];
            dataarrayInsert.push(newitem);
            updateData();

            console.log(newitem);
            console.log(dataarrayInsert);
            sendmessage('Richiesta inserimento in corso.', '');

        }


        function bindchangeevent() {
            $('textarea').on("input propertychange", function () {
                updatejson((this));
            });
            $('input').on("input propertychange", function () {
                updatejson((this));
            });
            $('select').on("input propertychange", function () {
                updatejson((this));
            });
        }

        //Prendo il valore modificato dall'utente nel controllo ed aggiorno il JSON del dettaglio -----------------------------*/
        function updatejson(el) {
            try {
                /*Qui ho dei sottoelementi nella struttura del json*/
                //Stabiliamo il livello di annidamento del controllo nella struttura in base all'attributo mybind 
                var proprarr = $(el).attr("mybind").split('.');
                var idbind = $(el).attr("idbind");

                if (proprarr != null && proprarr.length != 0)
                    if (idbind == null || idbind.length == 0) {
                        switch (proprarr.length) {
                            case 1: //Oggetto 1 livello
                                //if (!jsondetailsobj.hasOwnProperty(proprarr[0])) {
                                //    //devo creare l'elemento mancante nel JSON
                                //    var lackelem = "{" + proprarr[0] + " : ''  }";
                                //    jQuery.extend(jsondetailsobj, lackelem);
                                //}
                                if ($(el).is("input")) {
                                    if ($(el).attr('type') == 'checkbox')
                                        newitem[proprarr[0]] = el.checked;
                                    else
                                        newitem[proprarr[0]] = el.value.trim();
                                }
                                if ($(el).is("select"))
                                    newitem[proprarr[0]] = el.value.trim();
                                if ($(el).is("textarea"))
                                    newitem[proprarr[0]] = el.value.trim();


                                break;
                        }
                    }
                    else {
                        for (var j = 0; j < dataarray.length; j++)
                        //Cerchiamo l'elemento nella lista per fare l'aggiornamento
                        {
                            if (dataarray[j]["Id"] == Number(idbind)) {
                                dataarray[j][proprarr[0]] = el.value.trim();
                                AggiornaDataArrayUpdated(dataarray[j], idbind);
                                break;
                            }
                        }


                        var h = 0;
                        //for (var j = 0; j < dataarray.length; j++)
                        ////Cerchiamo l'elemento nella lista per fare l'aggiornamento
                        //{
                        //    if (dataarray[j]["Id"] == Number(idbind)) {
                        //        dataarray[j][proprarr[0]] = el.value.trim();
                        //        break;
                        //    }
                        //}
                    }
                //jsondetailstxt = JSON.stringify(jsondetailsobj);
                //$(hJsondetails).val(jsondetailstxt);
            }
            catch (e) { }
            // console.log(jsondetailsobj);
        }

        function AggiornaDataArrayUpdated(dataItem, idbind) {
            var trovato = false;
            for (var j = 0; j < dataarrayUpdated.length; j++)
            //Cerchiamo l'elemento nella lista per fare l'aggiornamento
            {
                if (dataarrayUpdated[j]["Id"] == Number(idbind)) {
                    dataarrayUpdated[j] = dataItem;
                    trovato = true;
                    break;
                }
            }
            if (!trovato)
                dataarrayUpdated.push(dataItem);
        }

        function databind(containerid, templateid, data) {
            if (!data.length) {
                // $('#' + el).html(''); 
                //$('#' + objfiltrotmp.containerid).html('');
                $('#' + containerid).html('');
                return;
            }
            //Memorizzo il template per i successivi binding
            var str = $($('#' + templateid)[0]).html();
            if (templatehtml == null) {
                str = $($('#' + templateid)[0]).html();
                templatehtml = str;
            } else str = templatehtml;

            var jquery_obj = $(str);


            var container = $('#' + containerid);
            $(container).html('');
            for (var j = 0; j < data.length; j++) {
                FillBindControls(jquery_obj.wrap('<p>').parent(), data[j], "", "",
                    function (ret) {
                        $(container).append(ret.html()) + "<br/>\r\n";
                    });
            }
            bindchangeevent();

        }


    </script>
    <h1>Parametri configurazione</h1>
    <asp:Button Text="Restart" ID="reset" OnClick="reset_Click" runat="server" />


    <select id="ddlLingue" onchange="getdata()"></select><br />
    <span style="font-size: 2rem" id="results"></span>


    <div class="row">
        <h3>Sezione Risorse</h3>
    </div>
    <div style="background-color: #ccc; padding: 20px">
        <h2>Inserisci nuova voce:</h2>
        <div class="row">
            <div class="col-sm-6">
                <b>Chiave</b>
                <input class="form-control" id="newchiave" mybind="Chiave" maxlength="50" style="width: 80%" />
                <b>Gruppo</b>
                <input class="form-control" id="newgruppo" mybind="Gruppo" maxlength="50" style="width: 80%" />
            </div>
            <div class="col-sm-6">
                <b>Valore</b>
                <textarea class="form-control" id="newvalore" mybind="Valore" rows="4" style="width: 80%"></textarea>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <button class="btn btn-primary" type="button" onclick="javascript:insertdata()">Aggiungi</button>
            </div>
        </div>
    </div>
    <p>&nbsp;</p>
    <div style="background-color: #ddd; padding: 20px">
        <h2>Aggiorna valori presenti</h2>
        <div class="row" id="row1">
            <div class="col-sm-12">
                <div>
                    |<label class="bind" mybind="Id"></label>
                    |<label class="bind" mybind="Chiave"></label>
                    |<label class="bind" mybind="Gruppo"></label>|
                |<label class="bind" mybind="Lingua"></label>|
                |<label class="bind" mybind="Categoria"></label>|
             <%--   |<label class="bind" mybind="Comment"></label>|--%>
                    <textarea class="form-control bind" mybind="Valore" idbind="Id" rows="4" style="width: 80%"></textarea>
                </div>
            </div>
        </div>
    </div>
    <p>&nbsp;</p>


    <div class="row">
        <div class="col-sm-6">
            <button class="btn btn-primary" type="button" onclick="javascript:updateData()">Memorizza</button>
        </div>
    </div>

</asp:Content>

