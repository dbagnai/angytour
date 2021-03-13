<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="StoricoOrdini.aspx.cs" Inherits="AreaContenuti_StoricoOrdini_New" %>


<%@ MasterType VirtualPath="~/AreaContenuti/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="x-ua-compatible" content="IE=9" />

    <style type="text/css">
        /*STILI PER LE FINESTRE DI POPUP------------------------------------*/
        /*Popup Control*/
        .MaskedEditFocus {
            background-color: #ffffcc;
            color: #000000;
        }

        .popupControlCalendar {
            background-color: White;
            position: absolute;
            visibility: hidden;
        }
        /*Popup Control*/
        .popupControl {
            position: absolute;
            visibility: hidden;
            border: none;
            padding: 2px 2px 2px 2px;
            border: Solid 1Px Black;
        }

        .modalPopup {
            background-color: #ffffdd;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .closeIcon {
            background-position: right top;
            background-image: url(../../images/close_icon.gif);
            background-repeat: no-repeat;
            border: none;
            position: absolute;
            cursor: pointer;
            width: 19px;
            top: 0px;
            height: 19px;
            right: 0px;
            background-color: transparent;
        }

        a.handclick, a.handclick:link, a.handclick:hover, a.handclick:visited {
            cursor: pointer;
            color: White;
        }
        /*Hover Menu*/
        .popupMenu {
            position: absolute;
            visibility: hidden;
            background-color: #F5F7F8;
            opacity: .99;
            filter: alpha(opacity=99);
            font-family: Garamond,Tahoma;
            font-size: 10pt;
        }

        .popupHover {
            background-image: url(../immagini/header-opened.png);
            background-repeat: repeat-x;
            background-position: left top;
            background-color: #F5F7F8;
        }

        .txtInputPopup {
            font-family: Garamond,Tahoma;
            font-size: 8pt;
            background-color: #F5F7F8;
            width: 95%;
        }

        .txtInputPopupUpper {
            font-family: Garamond,Tahoma;
            text-transform: uppercase;
            font-size: 8pt;
            background-color: #F5F7F8;
            width: 95%;
        }
        /*AutoComplete flyout */
        .autocomplete_completionListElement {
            visibility: hidden;
            margin: 0px;
            background-color: white;
            color: black;
            font-size: 9pt; /*border : buttonshadow;*/
            border-width: 1px;
            border-style: solid;
            cursor: default;
            overflow: auto;
            height: 200px;
            text-align: left;
            list-style-type: none;
            z-index: 10000;
        }

        /* AutoComplete highlighted item */
        .autocomplete_highlightedListItem {
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }

        /* AutoComplete item */
        .autocomplete_listItem {
            background-color: window;
            color: black;
            padding: 1px;
        }

        .normal {
            background-color: #FFFFFF;
        }

        .highlight {
            background-color: #8888FF;
        }

        .selected {
            background-color: #5555FF;
        }

        /* Liste riordinabili */
        .sortable1 li {
            margin: 0px;
            padding: 0px;
            list-style-type: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField runat="server" ID="hididcommerciale" />
    <asp:HiddenField runat="server" ID="hididcliente" />
    <asp:Label Text="" ID="output" runat="server" Style="font-size: 1.8rem; color: red" />
    <script>
        $(document).ready(function () {
            initautocompletefilter();
        });
        function initautocompletefilter() {

            //autocompletericerca su handlerdatacommon in base all' id della tbl_attivita
            $("#txt" + "idprodotto").autocomplete({
                source: pathAbs + commonhandlerpath + '?q=autocompletericerca&r=20&tipologia=rif000001&lng=' + lng,
                minLength: 5,
                appendTo: '#divAutocomplete1',
                open: function () {
                    //console.log("OPEN");
                    setTimeout(function () {
                        $('.ui-autocomplete').css('z-index', 99999999999999);
                    }, 0);
                },
                select: function (event, ui) {
                    if (ui.item != null) {
                        event.preventDefault();
                        // $("#hid" + "idprodotto").val(ui.item.id);
                        //  $("#txt" + "idprodotto").text(ui.item.label);
                        $("#txt" + "idprodotto").val(ui.item.id);
                    }
                }
            });
            $("#" + "txt" + "idprodotto").focus(function () {
                document.getElementById("txtidprodotto").value = '';
                document.getElementById("hididprodotto").value = '';
            });

            //autocompletericerca su handlerdatacommon
            //$("#txt" + "idscaglione").autocomplete({
            //    source: pathAbs + commonhandlerpath + '?q=autocompletericerca&r=20&tipologia=rif000001&lng=' + lng,
            //    minLength: 5,
            //    appendTo: '#divAutocomplete2',
            //    open: function () {
            //        setTimeout(function () {
            //            $('.ui-autocomplete').css('z-index', 99999999999999);
            //        }, 0);
            //    },
            //    select: function (event, ui) {
            //        if (ui.item != null) {
            //              event.preventDefault();
            //            // $("#hid" + "idscaglione").val(ui.item.id);
            //            //  $("#txt" + "idscaglione").text(ui.item.label);
            //            $("#txt" + "idscaglione").text(ui.item.id);
            //        }
            //    }
            //});
            //$("#" + "txt" + "idscaglione").focus(function () {
            //    document.getElementById("txtidscaglione").value = '';
            //    document.getElementById("hididscaglione").value = '';
            //});


        }
    </script>
    <div class="row">
        <div class="col-lg-3">
            <div class="widget">
                <h3 class="">Filtri ordini</h3>
                <asp:Button ID="btnFiltroOrdine" ClientIDMode="Static" runat="server" class="btn btn-info"
                    OnClick="btnFiltroordine_Click" Text="Filter"></asp:Button><br />
                <br />
                <div class="clearfix"></div>
                <ul class="menu" style="list-style-type: none">

                    <li  runat="server" id="liacconto"><b>Stato Acconto :</b><br />
                        <asp:RadioButtonList ID="radacconto" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Qualsiasi" Value="" Selected="True" />
                            <asp:ListItem Text="Acconto da pagare" Value="0" />
                            <asp:ListItem Text="Acconto Pagato" Value="1" />
                        </asp:RadioButtonList><br />
                    </li>
                    <li runat="server" id="lisaldo"><b>Stato Saldo :</b><br />
                        <asp:RadioButtonList ID="radsaldo" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Qualsiasi" Value="" Selected="True" />
                            <asp:ListItem Text="Saldo da pagare" Value="0" />
                            <asp:ListItem Text="Saldo Pagato" Value="1" />
                        </asp:RadioButtonList><br />
                    </li>
                  
                    <li visible="false" runat="server" id="liidprodotto"><b>Codice Pacchetto:</b><br />
                        <div id="divAutocomplete1" style="position: relative; margin: 0px auto">
                            <input type="text" id="txtidprodotto" style="width: 100%" clientidmode="Static" runat="server" /><br />
                            <input type="hidden" id="hididprodotto" clientidmode="Static" runat="server" />
                        </div>

                    </li>
                    <li visible="false" runat="server" id="liidscaglione"><b>Codice Partenza/Scaglione:</b><br />
                        <div id="divAutocomplete2" style="position: relative; margin: 0px auto">
                            <input type="text" id="txtidscaglione" style="width: 100%" clientidmode="Static" runat="server" /><br />
                        </div>
                    </li>
                    

                    <li runat="server" id="liFiltroordini"><b>Codice Ordine:</b><br />
                        <asp:TextBox ID="txtCodiceordine" Width="100%" runat="server" /><br />
                    </li>
                    <li><b>Data inizio ordine:</b><br />
                        <asp:TextBox runat="server" ID="txtdatamin" Width="100%" />
                        <Ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy HH:mm:ss" TargetControlID="txtdatamin">
                        </Ajax:CalendarExtender>
                    </li>
                    <li><b>Data fine ordine:</b><br />
                        <asp:TextBox runat="server" ID="txtdatamax" Width="100%" />
                        <Ajax:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy HH:mm:ss" TargetControlID="txtdatamax">
                        </Ajax:CalendarExtender>
                    </li>
                      <li runat="server" id="liFiltroclienti">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <Ajax:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtCLIENTE"
                                    OnClientItemSelected="GetCodeR" ServiceMethod="GetCompletionList" ServicePath="WS/WSListaClienti.asmx"
                                    MinimumPrefixLength="3" EnableCaching="true" UseContextKey="true" CompletionSetCount="100"
                                    CompletionInterval="1000" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" />
                                <script type="text/javascript">
                                    function GetCodeR(source, eventArgs) {
                                        $get("<%= txtCLIENTE.ClientID %>").value = eventArgs.get_value();
                                    }
                                </script>
                                <b>Filtro cliente (ricerca per nome o email):</b><br />
                                <asp:TextBox MaxLength="10" ID="txtCLIENTE" Width="100%" runat="server" /><br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </li>
                    <li runat="server" id="liFiltroCommerciali">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <Ajax:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtCommerciale"
                                    OnClientItemSelected="GetCodeC" ServiceMethod="GetCompletionList" ServicePath="WS/WSListaClienti.asmx"
                                    MinimumPrefixLength="3" EnableCaching="true" UseContextKey="true" CompletionSetCount="100"
                                    CompletionInterval="1000" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" />
                                <script type="text/javascript">
                                    function GetCodeC(source, eventArgs) {
                                        $get("<%= txtCommerciale.ClientID %>").value = eventArgs.get_value();
                                    }
                                </script>
                                <b>Filtro commerciale (ricerca per nome o email):</b><br />
                                <asp:TextBox MaxLength="10" ID="txtCommerciale" Width="100%" runat="server" /><br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </div>
        </div>
        <script type="text/javascript">
            function Preparadati() {
                var idcommerciale = $("#" +  '<%= hididcommerciale.ClientID %>').val();
                if (idcommerciale == '') { idcommerciale = $("#" +  '<%= txtCommerciale.ClientID %>').val(); }
                var idcliente = $("#" +  '<%= txtCLIENTE.ClientID %>').val();
                var codiceordine = $("#" +  '<%= txtCodiceordine.ClientID %>').val();
                var datamin = $("#" +  '<%= txtdatamin.ClientID %>').val();
                var datamax = $("#" + '<%= txtdatamax.ClientID %>').val();
                var idprodotto = $("#" + '<%= txtidprodotto.ClientID %>').val();
                var idscaglione = $("#" + '<%= txtidscaglione.ClientID %>').val();
                var statoacconto = $("input[name='<%= radacconto.ClientID.Replace("_","$") %>']:checked").val();
                var statosaldo = $("input[name='<%= radsaldo.ClientID.Replace("_","$") %>']:checked").val();
                $.ajax({
                    type: "POST",
                    url: "StoricoOrdini.aspx/PreparaStampa",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{idcommerciale: "' + idcommerciale + '",idcliente: "' + idcliente + '",codiceordine: "' + codiceordine + '",datamin: "' + datamin + '",datamax: "' + datamax + '",idprodotto: "' + idprodotto + '",idscaglione: "' + idscaglione + '",statoacconto: "' + statoacconto + '",statosaldo: "' + statosaldo + '" }',
                    success: function (data) {
                        // OnSuccess(data, this.destinationControl);
                        //window.location.href = '/AspNetPages/formStampa.aspx';
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
            //function stampalista() {
            //    $get("ctl00_ContentPlaceHolder1_btnStampa").click();
            //}
        </script>
        <div class="col-lg-9">
            <div class="widget">
                <div style="padding: 5px">
                    <a class="btn btn-default btn-small pull-right" onclick="Preparadati()" href='<%= WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/AspNetPages/formStampa.aspx" %>' target="_blank" style="text-decoration: underline;">Stampa Lista</a>
                    <%-- <asp:Button Style="display: none" Text="" ID="btnStampa" OnClick="btnStampa_Click" runat="server" CommandArgument="" />--%>
                    <%--<asp:Button class="btn btn-default btn-small pull-right" Text="Excel" ID="Button2" OnClick="btnExport_Click" runat="server" CommandArgument="" />--%>
                    <asp:Button class="btn btn-default btn-small pull-right" Style="margin-right: 10px" Text="Excel Export" ID="Button1" OnClick="btnExport1_Click" runat="server" CommandArgument="" />
                </div>
                <h3>Lista ordini</h3>
                <table class="table table-order table-stripped">
                    <thead>
                        <tr>
                            <td></td>
                            <td>Data Ordine</td>
                            <td>Codice Ordine</td>
                            <td>Totale Ordine</td>
                            <td>Acconto</td>
                            <td>Acconto</td>
                            <td>Saldo</td>
                            <td>Saldo</td>
                            <td>Tipo Pagamento</td>
                            <td></td>
                            <td></td>
                        </tr>
                    </thead>
                    <tbody>
                        <script type="text/javascript">
                            $(document).ready(function () {
                                //Funzione eseguita all'apertura del dropdown con classe triggerdata
                                $('div.btn-group a.triggerdata').click(function (e) {
                                    var $div = $(this).parent();
                                    $(this).dropdown("toggle");
                                    //alert($(this).parent().find("[id*='ContainerCarrelloDetails']")[0].id);
                                    if ($(this).parent().find("[id*='ContainerCarrelloDetails']")[0].innerText == "") {
                                        var codiceordine = $(this).parent().find("[id*='ContainerCarrelloDetails']").attr("title");
                                        var contenitoredestinazione = $(this).parent().find("[id*='ContainerCarrelloDetails']");
                                        //Caricamento ajax carrello!
                                        ShowCurrentCarrello(contenitoredestinazione, codiceordine);
                                    }
                                    e.preventDefault();
                                    //Reimposta la funzione che fà apire il dropdown
                                    $(this).click(function (ev) {
                                        $(this).dropdown("toggle");
                                        return false;
                                    });
                                    return false;
                                });

                                //Evita che il dropdown si chiuda cliccandodi sopra
                                $('.dropdown-menu').click(function (e) {
                                    e.stopPropagation();
                                });

                            });
                            function ShowCurrentCarrello(contenitoredestinazione, codiceordine) {
                                $.ajax({
                                    destinationControl: contenitoredestinazione,
                                    type: "POST",
                                    url: "StoricoOrdini.aspx/GetCurrentCarrello",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    data: '{codice: "' + codiceordine + '", username: "' + username + '" }',
                                    success: function (data) {
                                        OnSuccess(data, this.destinationControl);
                                    },
                                    failure: function (response) {
                                        alert(response.d);
                                    }
                                });
                            }
                            function OnSuccess(response, destination) {
                                // alert(response.d);
                                // alert(destination[0].id);//Controllo destinazione html
                                destination.append("<li>" + response.d + "</li>");
                            }
                            function CancellaOrdine(codiceordine) {
                                var conferma = confirm('Sei sicuro di voler cancellare questo ordine?');
                                if (conferma) {
                                    console.log(codiceordine);
                                    if (codiceordine != '') {
                                        CancellaOrdineByCodice(codiceordine, function (ret) {
                                            console.log('eliminato ordine: ' + ret);
                                            if (ret == codiceordine) {
                                                var buttpost = document.getElementById("<%= btnFiltroOrdine.ClientID  %>");
                                                buttpost.click();
                                            } else
                                                console.log('errore eliminazione ordine: ' + ret);
                                        });
                                    }
                                }
                            }
                            //function callupdatepacc(idcontrollo) {
                            //    console.log(idcontrollo);
                            //}
                        </script>
                        <asp:Repeater runat="server" ID="rtpOrdini">
                            <ItemTemplate>
                                <tr>
                                    <td  ><div runat="server" visible='<%# BloccaSuRuoli("Operatore") %>'><a  class="btn btn-default btn-small" href="<%# "javascript:CancellaOrdine('" + Eval("CodiceOrdine").ToString() + "')" %>">Del</a></div></td>
                                    <td class="order-date"><%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("Dataordine") ) %> </td>
                                    <td class="order-id"><%# Eval("CodiceOrdine").ToString() %></td>
                                    <%--    <td class="order-amount"><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                  new object[] { (Double)Eval("TotaleSmaltimento") + (Double)Eval("TotaleOrdine") + (Double)Eval("TotaleSpedizione") - (Double)Eval("TotaleSconto") }  ) + " €" %></td>--%>
                                    <td class="order-amount"><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                  new object[] { (Double)Eval("TotaleAcconto") + (Double)Eval("TotaleSaldo") }  ) + " €" %></td>

                                    <td class="order-amount"><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                  new object[] { (Double)Eval("TotaleAcconto")}  ) + " €" %></td>
                                    <td class="order-status"><%# StatusDisplayAcconto(Container.DataItem) %>
                                     
                                        <asp:CheckBox  Enabled='<%# BloccaSuRuoli("Operatore") %>' Checked="<%# StatusCheckAcconto(Container.DataItem) %>" ToolTip='<%# Eval("CodiceOrdine").ToString() %>' runat="server" ID="chkPagatoacconto" OnCheckedChanged="chkPagatoacconto_CheckedChanged" AutoPostBack="true" /> 
                                        <%--  <input type="checkbox" name="name" <%# StatusCheckAccontoHtml(Container.DataItem) %> onclick="<%# "javascript:callupdatepacc('" + Eval("CodiceOrdine").ToString() + "')"  %>"  />--%>
                                    </td>
                                    <td class="order-amount"><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                  new object[] { (Double)Eval("TotaleSaldo") }  ) + " €" %></td>
                                    <td class="order-status"><%# StatusDisplay(Container.DataItem) %>
                                        <asp:CheckBox  Enabled='<%# BloccaSuRuoli("Operatore") %>'  Checked="<%# StatusCheck(Container.DataItem) %>" ToolTip='<%# Eval("CodiceOrdine").ToString() %>' runat="server" AutoPostBack="true" ID="chkPagato" OnCheckedChanged="chkPagato_CheckedChanged" />
                                    </td>
                                    <td class="order-status"><%# TipopagaDisplay(Container.DataItem) %></td>
                                    <td class="order-amount">
                                        <div class="form-cart btn-group pull-right">
                                            <a class="btn btn-default btn-small dropdown-toggle" data-toggle="dropdown" id="details" runat="server">Dettagli<span class="caret"></span>
                                            </a>
                                            <table class="dropdown-menu product-attribute table" style="min-width: 350px; text-align: left">
                                                <tbody>
                                                    <tr>
                                                        <th>Totale articoli Carrello</th>
                                                        <td><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                   new object[] { (Double)Eval("TotaleOrdine") } ) + " €" %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Totale Spedizione</th>
                                                        <td><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                      new object[] { (Double)Eval("TotaleSpedizione") }  ) + " €" %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Totale Sconto</th>
                                                        <td><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                     new object[] { (Double)Eval("TotaleSconto") } ) + " €" %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Acconto:</th>
                                                        <td><%# ( (Double)Eval("Percacconto")!=100? String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}", new object[] { (Double)Eval("Percacconto") } ) + "% / " : ""  ) + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                     new object[] { (Double)Eval("TotaleAcconto") } ) + " €" %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Totale a saldo:</th>
                                                        <td><%#   String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                     new object[] { (Double)Eval("TotaleSaldo") } ) + " €" %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>IdCliente:</th>
                                                        <td><%# "<a href=\"/AreaContenuti/gestioneclienti.aspx?idcliente=" + Eval("Id_cliente").ToString() + "\" target=\"_blank\">" + Eval("Id_cliente").ToString() + "</a>" %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Mail Cliente:</th>
                                                        <td><%# Eval("Mailcliente").ToString() %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Nome Cliente:</th>
                                                        <td>
                                                            <%# Eval("Denominazionecliente").ToString() %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Fatturazione:</th>
                                                        <td><%# Eval("Indirizzofatturazione").ToString() %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Spedizione:</th>
                                                        <td>
                                                            <%# Eval("Indirizzospedizione").ToString() %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>IdCommerciale:</th>
                                                        <td><%# Eval("Id_commerciale").ToString() %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>CodSconto:</th>
                                                        <td><%# Eval("Codicesconto").ToString() %></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Note:</th>
                                                        <td>
                                                            <%# Eval("Note").ToString() %></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </td>
                                    <td class="order-detail">
                                        <div class="form-cart btn-group pull-right">
                                            <a class="btn btn-default btn-small dropdown-toggle triggerdata" data-toggle="dropdown" id="btnDetails" runat="server">Articoli<span class="caret"></span>
                                            </a>
                                            <ul class="dropdown-menu dropdown-cart" style="width: 300px; list-style: none">
                                                <li>
                                                    <ul class="products-list-mini" id="ContainerCarrelloDetails" style="list-style: none" runat="server" title='<%# Eval("CodiceOrdine").ToString() %>'>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td>
                                <div class="pull-right">
                                    <UC:PagerEx ID="PagerRisultati" runat="server" NavigateUrl="" PageSize="30" CurrentPage="1"
                                        TotalRecords="0" dimensioneGruppo="20" nGruppoPagine="1" OnPageCommand="PagerRisultati_PageCommand"
                                        OnPageGroupClickNext="PagerRisultati_PageGroupClickNext" OnPageGroupClickPrev="PagerRisultati_PageGroupClickPrev" />
                                </div>
                                <div class="clearfix">
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
      <%=  InjectedStartPageScripts() %>
    <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/lib/js/gestioneclienticontroller.js")+ CommonPage.AppendModTime(Server,"~/lib/js/gestioneclienticontroller.js")%>"></script>
       <script>
           $(document).ready(function () {
               ClientiManager.initVUE($("[id$='txtCLIENTE']").val());
           });
       </script>
       <asp:Panel runat="server" ID="pnlClienti" Visible="true">
        <div id="vueContainer">
            <div class="row">
                <div class="col-xs-12 col-sm-12">
                    <span style="font-size: 1.4rem; color: crimson">
                        <span>{{vm.message}}</span>
                    </span>
                </div> 
                <div class="col-xs-12 col-sm-6">
                       <h2>Dati anagrafici</h2>

                    <div style="background: #ccc; padding: 10px 30px 30px 30px;">
                        <div class="row" style="padding-bottom: 10px">
                             <input type="button" class="btn btn-primary btn-sm"  v-on:click="inserisciAggiornaDettaglio()" value="Aggiorna/Inserisci" />
                        </div>
                        <div class="row" style="padding-bottom: 10px">
                            <div class="col-sm-3 item-text text-left">
                                <strong>Id Cliente</strong><br />
                                <span v-html="vm.itemselected.Id_cliente"></span>
                            </div>
                              <div class="col-sm-2 item-text  text-left">
                                <strong>Privacy</strong><br /> 
                                <input type="checkbox" v-model:checked="vm.itemselected.ConsensoPrivacy"  disabled   />
                            </div> 
                            <div class="col-sm-2 item-text  text-left">
                                 <strong>Mailing</strong><br />
                                <input type="checkbox" v-model:checked="vm.itemselected.Consenso1" disabled   />
                            </div>
                            <div class="col-sm-2 item-text  text-left">
                                <strong>Validato</strong><br /> 
                                <input type="checkbox" v-model:checked="vm.itemselected.Validato"  disabled  />
                            </div>
                             <div class="col-sm-3 item-text  text-left">
                                 <strong>Tipologia</strong><br /> 
                                <select v-model="vm.itemselected.id_tipi_clienti" disabled>
                                     <option v-for="(value,key) in im.tipiclientilist" :value="key">{{ value }}</option>
                                </select>
                            </div>
                        </div>
                         <div class="row" style="padding-bottom: 10px">
                            <div class="col-sm-3 item-text text-left">
                                <strong>Telefono</strong><br />
                                <input type="text" class="form-control" v-model="vm.itemselected.Telefono"   />
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                  <strong>Cellulare</strong><br />
                                <input type="text" class="form-control" v-model="vm.itemselected.Cellulare"   />
                            </div>
                             <div class="col-sm-3 item-text  text-left">
                               <strong>Email</strong><br /> 
                                <input type="text" class="form-control" v-model="vm.itemselected.Email"   />
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                <strong>Lingua</strong>
                                <br />
                                <select  class="form-control"  v-model="vm.itemselected.Lingua">
                                         <option v-for="(value,key) in im.languageslist" :value="key">{{ value }}</option>
                                </select>
                            </div>
                        </div>
                         <div class="row" style="background: #ddd;border: 1px solid black;padding:10px">
                                <div class="col-sm-12 item-text text-left">
                                   <h4><strong>Indirizzo principale ( fatturazione )</strong></h4>
                                </div>
                                 <div class="row" style="padding-bottom: 10px">
                            <div class="col-sm-3 item-text text-left">
                               <strong>Cognome</strong><br />
                                <input type="text" class="form-control" v-model="vm.itemselected.Cognome"   />
                            </div>
                             <div class="col-sm-3 item-text  text-left">
                                   <strong>Nome</strong><br /> 
                                <input type="text" class="form-control" v-model="vm.itemselected.Nome"   />
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                   <strong>Azienda</strong><br /> 
                                <input type="text" class="form-control" v-model="vm.itemselected.Ragsoc"   />
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                
                            </div>
                        </div>
                                 <div class="row" style="padding-bottom: 10px">
                                    <div class="col-sm-3 item-text text-left">
                                        <strong>Cap</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.Cap"    />
                                    </div>
                                     <div class="col-sm-3 item-text  text-left">
                                           <strong>Indirizzo/Via</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.Indirizzo"    />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                          <strong>P.Iva/CF</strong><br /> 
                                        <input type="text" class="form-control" v-model="vm.itemselected.Pivacf"   />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                        <strong>SDI/PEC</strong><br /> 
                                        <input type="text" class="form-control" v-model="vm.itemselected.Emailpec"   />
                                    </div>
                                </div>
                                 <div class="row" style="padding-bottom: 10px">
                                      <div class="col-sm-3 item-text  text-left">
                                              <strong>Nazione: </strong>
                                              <br />
                                            <select class="form-control" v-model="vm.itemselected.CodiceNAZIONE"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListRegione', 'caricaddlregione','vm.itemselected.CodiceREGIONE');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListProvincia', 'caricaddlprovincia','vm.itemselected.CodicePROVINCIA');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListComune', 'caricaddlcomune','vm.itemselected.CodiceCOMUNE')">
                                                <option v-for="(value,key) in vm.geolist1.ListNazione" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.CodiceNAZIONE"  style="display:none"  />
                                    </div>
                                      <div class="col-sm-3 item-text  text-left">
                                            <strong>Regione: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.CodiceREGIONE"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListProvincia', 'caricaddlprovincia','vm.itemselected.CodicePROVINCIA');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListComune', 'caricaddlcomune','vm.itemselected.CodiceCOMUNE')" >
                                                <option v-for="(value,key) in vm.geolist1.ListRegione" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.CodiceREGIONE"    />

                                    </div>
                                       <div class="col-sm-3 item-text  text-left">
                                            <strong>Provincia: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.CodicePROVINCIA"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListComune', 'caricaddlcomune','vm.itemselected.CodiceCOMUNE')">
                                                <option v-for="(value,key) in vm.geolist1.ListProvincia" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.CodicePROVINCIA"    />
                                    </div>
                                       <div class="col-sm-3 item-text  text-left">
                                            <strong>Comune: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.CodiceCOMUNE"  >
                                                <option v-for="(value,key) in vm.geolist1.ListComune" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.CodiceCOMUNE"    />
                                    </div>
                                </div>
                        </div>
                         <div class="row" style="background: #e0e0e0;border: 1px solid black;padding:10px">
                                <div class="col-sm-12 item-text text-left">
                                   <h4><strong>Indirizzo spedizione ( opzionale )</strong></h4>
                                </div>
                              <div class="col-sm-3 item-text text-left">
                                        <strong>Nome</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.addvalues.Nome"    />
                                    </div>
                                     <div class="col-sm-3 item-text  text-left">
                                           <strong>Cognome</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.addvalues.Cognome"    />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                    </div>
                                </div>
                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-sm-3 item-text text-left">
                                        <strong>Cap</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.addvalues.Cap"    />
                                    </div>
                                     <div class="col-sm-3 item-text  text-left">
                                           <strong>Indirizzo/Via</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.addvalues.Indirizzo"    />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                          <strong>Telefono</strong><br /> 
                                        <input type="text" class="form-control" v-model="vm.itemselected.addvalues.Telefono"   />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                    </div>
                                </div>
                               <div class="row" style="padding-bottom: 10px">
                                      <div class="col-sm-3 item-text  text-left">
                                              <strong>Nazione: </strong>
                                              <br />
                                            <select class="form-control" v-model="vm.itemselected.addvalues.CodiceNAZIONE"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListRegione', 'caricaddlregione','vm.itemselected.addvalues.CodiceREGIONE');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListProvincia', 'caricaddlprovincia','vm.itemselected.addvalues.CodicePROVINCIA');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListComune', 'caricaddlcomune','vm.itemselected.addvalues.CodiceCOMUNE')">
                                                <option v-for="(value,key) in vm.geolist2.ListNazione" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.addvalues.CodiceNAZIONE"  style="display:none"  />
                                    </div>
                                      <div class="col-sm-3 item-text  text-left">
                                            <strong>Regione: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.addvalues.CodiceREGIONE"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListProvincia', 'caricaddlprovincia','vm.itemselected.addvalues.CodicePROVINCIA');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListComune', 'caricaddlcomune','vm.itemselected.addvalues.CodiceCOMUNE')" >
                                                <option v-for="(value,key) in vm.geolist2.ListRegione" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.addvalues.CodiceREGIONE"    />

                                    </div>
                                       <div class="col-sm-3 item-text  text-left">
                                            <strong>Provincia: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.addvalues.CodicePROVINCIA"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListComune', 'caricaddlcomune','vm.itemselected.addvalues.CodiceCOMUNE')">
                                                <option v-for="(value,key) in vm.geolist2.ListProvincia" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.addvalues.CodicePROVINCIA"    />
                                    </div>
                                       <div class="col-sm-3 item-text  text-left">
                                            <strong>Comune: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.addvalues.CodiceCOMUNE"  >
                                                <option v-for="(value,key) in vm.geolist2.ListComune" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.addvalues.CodiceCOMUNE"    />
                                    </div>
                                </div>
                        </div>
                        <div class="row" style="padding-bottom: 10px">
                             <div class="col-sm-3 item-text  text-left">
                                  <strong>Documento</strong><br /> 
                                <input type="text" class="form-control" v-model="vm.itemselected.Professione"   />
                            </div>
                             <div class="col-sm-3 item-text  text-left">
                                   <strong>Sesso </strong><br />
                                 <select class="form-control" v-model="vm.itemselected.Sesso">
                                <option v-for="(value,key) in im.generelist" :value="key">{{ value }}</option>
                                   </select>
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                  <strong>Data Nascita</strong><br /> 
                                  <input type="text" class="form-control"
                                    v-bind:value="vm.itemselected.DataNascita | formatshortDate"
                                    v-on:blur="vm.itemselected.DataNascita = formatDateforvue($event.target.value)"
                                    />
                            </div>
                            <div class="col-sm-3 item-text text-left">
                                <strong>Website</strong><br />
                                <input class="form-control" type="text" v-model="vm.itemselected.Spare1"    />
                            </div>
                        </div>
                     
                            <div class="row" style="background: #ccc;border: 1px solid black;padding:10px">
                            <div class="col-sm-12 item-text text-left">
                                <h4><strong>Accesso utente</strong></h4>
                            </div>
                          <div class="row" style="padding-bottom: 10px">
                            <div class="col-sm-12 item-text  text-left">
                               <b>User:</b>   <span v-html="vm.utente.Campo1"></span><br />
                               <b>Pass:</b>   <input class="form-control" type="text" v-model="vm.utente.Campo2"    /> <br />
                                 <input type="button" class="btn btn-primary btn-sm" style="margin:5px;width:150px"   value="Cambio Password" onclick="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('cambiopassword', JSON.stringify(vuecontroller.vm.utente), 'vm.utente',function(data){   },function(data){  })" />
                                 <span style="font-size: 1.4rem; color: crimson">
                                   <br /> <span>{{vm.utente.Campo3}}</span>
                                </span>
                            </div>
                        </div>
                        </div>
                        </div>
                    </div>
          
               </div>
            </div>
    </asp:Panel>


</asp:Content>

