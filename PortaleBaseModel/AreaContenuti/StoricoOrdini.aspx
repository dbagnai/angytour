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

    <div class="row">
        <div class="col-lg-3">
            <div class="widget">
                <h3>Filtri ordini</h3>
                <ul class="menu" style="list-style-type: none">
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
                                Filtro cliente (ricerca per nome o email):<br />
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
                                Filtro commerciale (ricerca per nome o email):<br />
                                <asp:TextBox MaxLength="10" ID="txtCommerciale" Width="100%" runat="server" /><br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </li>

                    <li runat="server" id="liFiltroordini">Codice Ordine:<br />
                        <asp:TextBox ID="txtCodiceordine" Width="100%" runat="server" /><br />

                    </li>
                    <li>Data inizio ordine:<br />
                        <asp:TextBox runat="server" ID="txtdatamin" Width="100%" />
                        <Ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy HH:mm:ss" TargetControlID="txtdatamin">
                        </Ajax:CalendarExtender>
                    </li>
                    <li>Data fine ordine:<br />
                        <asp:TextBox runat="server" ID="txtdatamax" Width="100%" />
                        <Ajax:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy HH:mm:ss" TargetControlID="txtdatamax">
                        </Ajax:CalendarExtender>
                    </li>
                    <li>
                        <asp:Button ID="btnFiltroOrdine" runat="server" class="btn btn-info pull-right"
                            OnClick="btnFiltroordine_Click" Text="Filter"></asp:Button></li>
                </ul>
            </div>
        </div>
        <!--end:.span3-->
        <script type="text/javascript">
            function Preparadati() {

                var idcommerciale = $("#" +  '<%= hididcommerciale.ClientID %>').val();
                if (idcommerciale == ''){  idcommerciale = $("#" +  '<%= txtCommerciale.ClientID %>').val();}
                var idcliente = $("#" +  '<%= txtCLIENTE.ClientID %>').val();
                var codiceordine = $("#" +  '<%= txtCodiceordine.ClientID %>').val();
                var datamin = $("#" +  '<%= txtdatamin.ClientID %>').val();
                var datamax = $("#" + '<%= txtdatamax.ClientID %>').val();
                $.ajax({
                    type: "POST",
                    url: "StoricoOrdini.aspx/PreparaStampa",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{idcommerciale: "' + idcommerciale + '",idcliente: "' + idcliente + '",codiceordine: "' + codiceordine + '",datamin: "' + datamin + '",datamax: "' + datamax + '" }',
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
                <a class="btn btn-default btn-small pull-right" onclick="Preparadati()" href='<%= WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/AspNetPages/formStampa.aspx" %>'  target="_blank" style="text-decoration: underline;">Stampa Lista</a>
            <%-- <asp:Button Style="display: none" Text="" ID="btnStampa" OnClick="btnStampa_Click" runat="server" CommandArgument="" />--%>
              
                <asp:Button class="btn btn-default btn-small pull-right" Text="Excel" ID="Button2" OnClick="btnExport_Click" runat="server" CommandArgument="" />
                <asp:Button class="btn btn-default btn-small pull-right" Text="Excel Completo" ID="Button1" OnClick="btnExport1_Click" runat="server" CommandArgument="" />

                <h3>Lista ordini</h3>
                <table class="table table-order table-stripped">
                    <thead>
                        <tr>
                            <td>Date</td>
                            <td>Order ID</td>
                            <td>Totale Ordine</td>
                            <td>Tipo Pagamento</td>
                            <td>Stato</td>
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
                                    data: '{codice: "' + codiceordine + '" }',
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
                        </script>
                        <asp:Repeater runat="server" ID="rtpOrdini">
                            <ItemTemplate>
                                <tr>
                                    <td class="order-date"><%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("Dataordine") ) %> </td>
                                    <td class="order-id"><%# Eval("CodiceOrdine").ToString() %></td>
                                    <td class="order-amount"><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                  new object[] { (Double)Eval("TotaleSmaltimento") + (Double)Eval("TotaleOrdine") + (Double)Eval("TotaleSpedizione") - (Double)Eval("TotaleSconto") }  ) + " €" %></td>
                                    <td class="order-status"><%# TipopagaDisplay(Container.DataItem) %></td>
                                    <td class="order-status"><%# StatusDisplay(Container.DataItem) %>
                                        <asp:CheckBox Checked="<%# StatusCheck(Container.DataItem) %>" ToolTip='<%# Eval("CodiceOrdine").ToString() %>' runat="server" AutoPostBack="true" ID="chkPagato" OnCheckedChanged="chkPagato_CheckedChanged" />
                                    </td>
                                    <td class="order-amount">
                                        <div class="form-cart btn-group pull-right">
                                            <a class="btn btn-default btn-small dropdown-toggle" data-toggle="dropdown" id="details" runat="server">Dettagli<span class="caret"></span>
                                            </a>
                                            <table class="dropdown-menu product-attribute table" style="min-width: 350px; text-align: left">
                                                <tbody>
                                                    <tr>
                                                        <th>Totale Carrello</th>
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
                                                    <%--    <tr>
                                                                <th>Totale Smaltimento</th>
                                                                <td><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                       new object[] {(Double)Eval("TotaleSmaltimento")  } ) + " €" %></td>
                                                            </tr>--%>
                                                    <tr>
                                                        <th>Acconto:</th>
                                                        <td><%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                     new object[] { (Double)Eval("Percacconto") } ) + "%" %></td>
                                                    </tr>

                                                    <tr>
                                                        <th>IdCliente:</th>
                                                        <td><%# Eval("Id_cliente").ToString() %></td>
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
                                            <a class="btn btn-default b<tn-small dropdown-toggle triggerdata" data-toggle="dropdown" id="btnDetails" runat="server">Prodotti<span class="caret"></span>
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
                        <%--  <tr>
                                    <td class="order-date">Mar 1, 2013</td>
                                    <td class="order-id">PO1234-IN</td>
                                    <td class="order-amount">&pound;215.20</td>
                                    <td class="order-status"><span class="text-warning">Received</span></td>
                                    <td class="order-detail"><a href="#" class="btn btn-info">View</a>
                                </tr>
                                <tr>
                                    <td class="order-date">Feb 28, 2013</td>
                                    <td class="order-id">PO1234-IN</td>
                                    <td class="order-amount">&pound;389.00</td>
                                    <td class="order-status"><span class="text-error">Cancelled</span></td>
                                    <td class="order-detail"><a href="#" class="btn btn-info">View</a>
                                </tr>--%>
                    </tbody>
                </table>
            </div>
        </div>
        <!--end:.span9-->
    </div>



</asp:Content>

