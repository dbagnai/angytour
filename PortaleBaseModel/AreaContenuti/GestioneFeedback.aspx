<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="GestioneFeedback.aspx.cs" Inherits="AreaContenuti_GestioneFeedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="/js/simplestarrating/SimpleStarRating.css" />
    <script type="text/javascript" src="/js/simplestarrating/SimpleStarRating.js"></script>
    <script type="text/javascript" src='<%= "/lib/js/feedbacks.js" + CommonPage.AppendModTime(Server,"~/lib/js/feedbacks.js") %>'></script>
    <script type="text/javascript">

        jQuery(document).ready(function () {
            commenttool.rendercommentsloadref('all', 'divCommenti', '', 'true', '1', '35');
            commenttool.initautocompleteclienti('searchcliente');
            fillddllanguages();
        });

        function fillddllanguages() {
            var langdict = [];
            var objlang = JSON.parse(jsonlanguages);
            var i = 0;
            for (var key in objlang) {
                if (!langdict.hasOwnProperty(key)) {
                    //console.log(key + ": " + obj[key]);
                    langdict[i] = {};
                    (langdict[i]).codice = key;
                    (langdict[i]).valore = objlang[key];
                    i++;
                }
            }
            console.log(langdict);
            fillDDLArraySimple($("#ddlLingue"), langdict, "lingua", "", "codice", "valore", lng);
        }

        function setlng(select)
        {
            console.log(select.value);
            lng = select.value;
            commenttool.rendercommentsloadref('all', 'divCommenti', '', 'true', '1', '35');
            commenttool.initautocompleteclienti('searchcliente');
        }
        function visualizzacommenti() {
            var idpost = $("#txtsearch").val();
            if (idpost == "") idpost = "all";
            $("#radioApprovati input").each(function (index, text) {
                if ($(this).is(":checked")) {
                    if ($(this).val() != '')
                        commenttool.objfiltro["approvati"] = $(this).val();
                    else
                        delete commenttool.objfiltro["approvati"]; //Elimino l'elemento
                }
            });
            commenttool.rendercommentsloadref(idpost, 'divCommenti', '', 'true', '1', '35');
        }

        function inseriscirichiesta() {

            var idcliente = $("#searchclienteHidden").val();
            if (idcliente == null || idcliente.length == 0) { $('#divmessage').html('Selezionare cliente per procedere'); return; }
            else $('#divmessage').html('');
            var idpost = $("#txtsearch").val();
            if (idpost == '') idpost = 0;
            var deltagg = $("#txtdeltagiorniperinvio").val();
            if (deltagg == '') deltagg = GetResourcesValue("feedbacksdefaultdeltagg");
            var idnewsletter = $("#txtidnewsletter").val();
            if (idnewsletter == '') idnewsletter = GetResourcesValue("feedbackdefaultnewsletter");
            var linkfeedback = $("#txtlinkfeedback").val();
            if (linkfeedback == '') linkfeedback = GetResourcesValue("feedbacksdefaultform");
            preparamailrichiestafeedback(linkfeedback, idnewsletter, idpost, deltagg, idcliente);

        }

        function preparamailrichiestafeedback(linkfeedback, idnewsletter, idpost, deltagg, idclienti) {
            commenttool.localcontainer.mail.Sparedict["linkfeedback"] = linkfeedback;
            commenttool.localcontainer.mail.Sparedict["idnewsletter"] = idnewsletter;
            commenttool.localcontainer.mail.Sparedict["deltagiorniperinvio"] = deltagg;
            commenttool.localcontainer.mail.Sparedict["idclienti"] = idclienti;
            commenttool.localcontainer.mail["Id_card"] = idpost;
            //Fare la richiesta di inserimento della mail tramite  handlernewsletter -> inseriscimailrichiestafeedback
            preparamail(lng, commenttool.localcontainer.mail,
                function (retpre) {
                    $('#divmessage').html(retpre);
                });
        }
        function preparamail(lng, mail, callback) {
            var lng = lng || "I";
            var mail = mail || {};

            $.ajax({
                url: pathAbs + newsletterhandlerpath,
                contentType: "application/json; charset=utf-8",
                global: false,
                cache: false,
                dataType: "text",
                type: "POST",
                //async: false,
                data: { 'q': 'inseriscimailrichiestafeedback', 'mail': JSON.stringify(mail), 'lng': lng },
                success: function (result) {
                    callback(result);

                },
                error: function (result) {

                    callback(result.responseText);
                },
                falilure: function (result) {

                    callback(result.responseText);
                }
            });
        }

    </script>
    <div class="container">
        <div class="row">
            <div class="col-sm-6" style="border-right: 1px solid #888">

                <h2>Gestione feedback</h2>
                <%--<asp:Button Text="Restart" ID="reset" OnClick="reset_Click" runat="server" />--%>
                <div>
                    <div id="radioApprovati">
                        <input type="radio" id="rdApprovati" name="approvati" value="true" />
                        Solo Approvati<br />
                        <input type="radio" id="rdNonapprovati" name="approvati" value="false" />
                        Solo Non Approvati<br />
                        <input type="radio" id="rdTutti" name="approvati" value="" checked />
                        Tutti
                    </div>
                    <input id="txtsearch" type="text" placeholder="Cerca Recenzioni per id Articolo" style="width: 250px" />
                    <button type="button" id="btnsearch" class="btn" onclick="visualizzacommenti()">Visualizza</button>
                </div>

            </div>
            <div class="col-sm-6" style="border-left: 1px solid #888">
                <h2>Gestione richieste feedback</h2>
                <input type="hidden" id="searchclienteHidden" /><br />
                <input type="text" id="searchcliente" placeholder="cerca cliente" style="min-width: 250px" /><br />
                <input type="text" id="txtidnewsletter" placeholder="id modello newsletter (opzionale)" style="min-width: 250px" value="" /><br />
                <input type="text" id="txtdeltagiorniperinvio" placeholder="intervallo giorni (opzionale)" style="min-width: 250px" value="" /><br />
                <div style="display: none">
                    <input type="text" id="txtlinkfeedback" placeholder="link pagina feedback (opzionale)" style="min-width: 250px" value="" /><br />
                </div>
                <br />
                <button type="button" id="btnrichiedifeedback" class="btn" onclick="inseriscirichiesta()">Richiedi Feedback</button>
                <div style="font-size: 1.8rem" id="divmessage"></div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <select id="ddlLingue" onchange="setlng(this)">
                    <option value=""></option>
                </select>
                <div id="divCommenti"></div>

            </div>
        </div>



    </div>
</asp:Content>

