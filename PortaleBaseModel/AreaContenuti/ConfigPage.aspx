<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="ConfigPage.aspx.cs" Inherits="AreaContenuti_ConfigPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <script type="text/javascript">
        var jsondetailsobj = {};
        var dataarray = [];

        //function fillsampledata() {
        //    jsondetailsobj = {};
        //    jsondetailsobj.inpCodicesconto = "aa.aa";
        //    jsondetailsobj.inpPercentualesconto = "bbb.b";
        //    dataarray.push(jsondetailsobj);
        //}

        function getdata() {
            //Chiamre server per leggere dati e Riempire ->
            // jsondetailsobj = {};
            var objfiltrotmp = {};
            objfiltrotmp["Gruppo"] = "";
            caricaParametriConfigServer(lng, objfiltrotmp,
                function (result, callafterfilter) {
                    try {
                        // qui devi riempire correttamente il datarray
                        //jsondetailsobj = JSON.parse(result);
                        //dataarray[0] = jsondetailsobj;

                        dataarray = JSON.parse(result);
                        callafterfilter("row1", "row1", dataarray)
                    }
                    catch (e) { }
                },
                databind);

        }


        /* UPDATE DEL RECORD -----------------------------------------------------------*/
        function updateData() {
            var jsondetailstxt = JSON.stringify(dataarray);
            if (jsondetailstxt != '' && jsondetailstxt != null) {
                $.ajax({
                    type: "POST",
                    url: pathAbs + commonhandlerpath,
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    data: {
                        'q': 'updateconfig', 'itemdata': jsondetailstxt
                    },
                    success: OnCompleteupdateDetail,
                    error: OnFailupdateDetail
                });
            }

        }
        function OnCompleteupdateDetail(result) {
            try {
                sendmessage('Richiesta Aggiornamento Completata.', result);
                $("#results").html('Richiesta Aggiornamento Completata.' + result);
            }
            catch (e) {
                //  sendmessage(e, '');
                $("#results").html(e);

            }
        }
        function OnFailupdateDetail(result) {
            //sendmessage('Richiesta Aggiornamento Fallita.', result.responseText);
            $("#results").html('Richiesta Aggiornamento Fallita.' + ' ' + result.responseText);

        }
        /* Procedure update  ---------------------- */

        function deletedata()
        { }



        jQuery(document).ready(function () {

            getdata();
            //   fillsampledata();
            // databind("row1", "row1", dataarray);

        });

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
                                        jsondetailsobj[proprarr[0]] = el.checked;
                                    else
                                        jsondetailsobj[proprarr[0]] = el.value.trim();
                                }
                                if ($(el).is("select"))
                                    jsondetailsobj[proprarr[0]] = el.value.trim();
                                if ($(el).is("textarea"))
                                    jsondetailsobj[proprarr[0]] = el.value.trim();
                                break;
                        }
                    }
                    else {
                        for (var j = 0; j < dataarray.length; j++)
                        //Cerchiamo l'elemento nella lista per fare l'aggiornamento
                        {
                            if (dataarray[j]["Id"] == Number(idbind)) {
                                dataarray[j][proprarr[0]] = el.value.trim();
                                break;
                            }
                        }
                    }
                //jsondetailstxt = JSON.stringify(jsondetailsobj);
                //$(hJsondetails).val(jsondetailstxt);
            }
            catch (e)
            { }
            // console.log(jsondetailsobj);
        }


        function databind(containerid, templateid, data) {
            if (!data.length) {
                // $('#' + el).html(''); 
                //$('#' + objfiltrotmp.containerid).html('');
                return;
            }
            var str = $($('#' + templateid)[0]).html();
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

      <asp:Button Text="Restart sito e aggiorna" ID="reset" OnClick="reset_Click" runat="server" />

    <span style="font-size: 1rem" id="results"></span>
    <%-- <h5>Codice Sconto</h5>
            <input class="form-control bind" mybind="prova" maxlength="20" style="width: 90px" />
            <h5>Percentuale Sconto</h5>
            <input class="form-control bind" mybind="prova2" style="width: 90px" />--%>
    <div class="row">
        <h3>Sezione Config</h3>
    </div>
    <div class="row" id="row1">
        <div class="col-sm-6">
            <div>
                <label class="bind" mybind="Id"></label>
                <label class="bind" mybind="Codice"></label>
                <input class="form-control bind" mybind="Valore" idbind="Id" maxlength="20" style="width: 80%" />
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

