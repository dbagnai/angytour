<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="GestioneClienti.aspx.cs" Inherits="AreaContenuti_GestioneClienti" %>


<%@ MasterType VirtualPath="~/AreaContenuti/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <%-- <meta http-equiv="x-ua-compatible" content="IE=9" />--%>

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
    <style type="text/css">
        .pagination {
            display: inline-block;
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination > li {
                display: inline;
            }

                .pagination > li > a,
                .pagination > li > span {
                    float: left;
                    padding: 4px 12px;
                    line-height: 1.428571429;
                    text-decoration: none;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    border-left-width: 0;
                }

                .pagination > li:first-child > a,
                .pagination > li:first-child > span {
                    border-left-width: 1px;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination > li:last-child > a,
                .pagination > li:last-child > span {
                    border-top-right-radius: 4px;
                    border-bottom-right-radius: 4px;
                }

                .pagination > li > a:hover,
                .pagination > li > a:focus,
                .pagination > .active > a,
                .pagination > .active > span {
                    background-color: #f5f5f5;
                }

            .pagination > .active > a,
            .pagination > .active > span {
                color: #999999;
                cursor: default;
            }

            .pagination > .disabled > span,
            .pagination > .disabled > a,
            .pagination > .disabled > a:hover,
            .pagination > .disabled > a:focus {
                color: #999999;
                cursor: not-allowed;
                background-color: #ffffff;
            }

        .pagination-large > li > a,
        .pagination-large > li > span {
            padding: 14px 16px;
            font-size: 18px;
        }

        .pagination-large > li:first-child > a,
        .pagination-large > li:first-child > span {
            border-bottom-left-radius: 6px;
            border-top-left-radius: 6px;
        }

        .pagination-large > li:last-child > a,
        .pagination-large > li:last-child > span {
            border-top-right-radius: 6px;
            border-bottom-right-radius: 6px;
        }

        .pagination-small > li > a,
        .pagination-small > li > span {
            padding: 5px 10px;
            font-size: 12px;
        }

        .pagination-small > li:first-child > a,
        .pagination-small > li:first-child > span {
            border-bottom-left-radius: 3px;
            border-top-left-radius: 3px;
        }

        .pagination-small > li:last-child > a,
        .pagination-small > li:last-child > span {
            border-top-right-radius: 3px;
            border-bottom-right-radius: 3px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="background-color: White; padding: 20px; min-height: 1800px; position: relative;">

        <asp:UpdatePanel ID="updPanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Label ID="output" runat="server" OnPreRender="output_PreRender"></asp:Label>
                Ricerca Rapida cliente:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               
                    <asp:TextBox Width="300px" MaxLength="7" ID="txtCLIENTE" runat="server" />
                <asp:Button ID="btnSeleziona" runat="server" OnClick="btnSeleziona_Click" Text="Seleziona Cliente" />
                <br />
                <br />
                Filtro clienti Giorno/mese/anno nascita<br />
                <asp:TextBox runat="server" ID="txtGiorno" Width="50" />
                <asp:TextBox runat="server" ID="txtMese" Width="50" />
                <asp:TextBox runat="server" ID="txtAnno" Width="50" />
                <br />
                <br />
                Filtro clienti età min:
           
                    <asp:TextBox runat="server" ID="txtetamin" Width="50" />

                Filtro clienti età max:
           
                    <asp:TextBox runat="server" ID="txtetamax" Width="50" />

                <br />
                <br />
                Filtro Sesso clienti:
           
                    <asp:RadioButtonList runat="server" ID="radSessoRicerca">
                        <asp:ListItem Text="Qualsiasi" Value="" Selected="True" />
                        <asp:ListItem Text="Uomo" Value="uomo" />
                        <asp:ListItem Text="Donna" Value="donna" />
                    </asp:RadioButtonList>
                <br />
                <div style="float: left; width: 200px">Filtra Nazione:</div>
                <asp:DropDownList Width="310px" runat="server" ID="ddlNazioniFiltro" AppendDataBoundItems="true"
                    AutoPostBack="true" OnSelectedIndexChanged="FiltraPerNazione" />
                <br />
                <div style="float: left; width: 200px">
                    <asp:Literal ID="Literal2" Text="Seleziona Tipologia Clienti: " runat="server" />
                </div>
                <asp:DropDownList runat="server" Width="310px" ID="ddlTipiClientiFiltro" AutoPostBack="true"
                    AppendDataBoundItems="true" OnSelectedIndexChanged="TipoClienteChange">
                </asp:DropDownList>
                <div style="clear: both; height: 1px"></div>
                <br />
                <script type="text/javascript">
                    function ConfermaCancella(btn) {
                        if (confirm('Vuoi cancellare il cliente corrente?')) {
                            var clid = btn.id;
                            //clid = clid.replace(/_/g, '$');
                            stopPost = false;
                            //$get(clid).click();
                        }
                        else {
                            stopPost = true;
                            //prm.abortPostBack();
                        }
                    }
                    function ConfermaCancellaClienti() {
                        if (confirm('Vuoi cancellare tutti i clienti per la tipologia selezionata?')) {

                            //clid = clid.replace(/_/g, '$');
                            stopPost = false;
                            //$get(clid).click();
                        }
                        else {
                            stopPost = true;
                            //prm.abortPostBack();
                        }
                    }
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    var stopPost = false;
                    function pageLoad() {
                        prm.add_initializeRequest(initializeRequest);
                    }
                    function initializeRequest(sender, args) {
                        if (stopPost == true) {
                            args.set_cancel(true);
                            stopPost = false;
                        }
                        if (prm.get_isInAsyncPostBack()) //Se già in postback asincrono non ne permetto due insieme
                        {
                            alert('Richiesta già in corso attendere il termine.');
                            args.set_cancel(true);
                        }
                    }
                </script>
                <Ajax:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtCLIENTE"
                    OnClientItemSelected="GetCodeR" ServiceMethod="GetCompletionList" ServicePath="WS/WSListaClienti.asmx"
                    MinimumPrefixLength="3" EnableCaching="true" UseContextKey="true" CompletionSetCount="100"
                    CompletionInterval="1000" CompletionListCssClass="autocomplete_completionListElement"
                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" />
                <script type="text/javascript" language="javascript">
                    function GetCodeR(source, eventArgs) {
                        $get("<%= txtCLIENTE.ClientID %>").value = eventArgs.get_value();
                    }
                </script>
                <br />
                <br />
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0" DynamicLayout="false"
                    AssociatedUpdatePanelID="updPanel1">
                    <ProgressTemplate>
                        <div style="position: absolute; background-color: White; color: Black; padding: 2px">
                            <img id="Img1" runat="server" alt="" src="~/Images/indicator.gif" />Attendi...
                   
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table style="width: 100%; background-color: black; color: white">
                    <tr>
                        <td style="width: 50px">Id</td>
                        <td style="">Cognome o Rag.Soc.</td>
                        <td style="width: 150px">Nome</td>
                        <td style="width: 160px">Email</td>
                        <td style="width: 100px">Lingua</td>
                        <td style="width: 150px">Tipo cliente</td>
                        <td style="width: 40px">Valido</td>
                    </tr>
                </table>
                <asp:Repeater ID="rptClienti" runat="server" EnableViewState="true" OnItemDataBound="rptClienti_OnItemDataBound">
                    <ItemTemplate>
                        <asp:Panel ID="PopupMenu" CssClass="popupMenu" runat="server"
                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                            <div style="max-width: 350px; width: 100%; float: left; padding: 5px">
                                <div style="text-align: center; width: 97%; background-color: Black; color: White;">
                                    DATI ANAGRAFICI CLIENTE
                                </div>
                                <table style="width: 100%; table-layout: fixed">
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Nazione
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <%--   <input class="txtInputPopup" maxlength="2" id="txtNA" runat="server" value='<%# Eval("CodiceNAZIONE") %>' />--%>
                                                <asp:DropDownList runat="server" Width="185" ID="ddlNazione" AppendDataBoundItems="true"
                                                    AutoPostBack="true" OnInit="ddlNazione_OnInit" OnSelectedIndexChanged="ddlNazioneRepeater_OnSelectedIndexChanged"
                                                    SelectedValue='<%# VerificaPresenza(Container,Eval("CodiceNAZIONE").ToString()) %>'
                                                    SkinID="Insert_ddl" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Regione
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtRE" maxlength="100" class="txtInputPopup" runat="server" value='<%# Eval("CodiceREGIONE") %>' />
                                                <asp:DropDownList runat="server" Width="185" ID="ddlRegione" AppendDataBoundItems="true"
                                                    AutoPostBack="true" OnInit="ddlRegione_OnInit" OnSelectedIndexChanged="ddlRegioneRepeater_OnSelectedIndexChanged"
                                                    SelectedValue='<%# VerificaPresenzaRegione(Container,Eval("CodiceREGIONE").ToString()) %>'
                                                    SkinID="Insert_ddl" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Provincia
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <%-- <input id="txtCOa" alt="'aaaaaaaaaa' , autoTab : 'false'" class="txtInputPopup" runat="server" value='<%# Eval("Comune") %>' />--%>
                                                <input id="txtPR" maxlength="100" class="txtInputPopup" runat="server" value='<%# Eval("CodicePROVINCIA") %>' />
                                                <asp:DropDownList runat="server" Width="185" ID="ddlProvincia" AppendDataBoundItems="true"
                                                    AutoPostBack="true" OnInit="ddlProvincia_OnInit" OnSelectedIndexChanged="ddlProvinciaRepeater_OnSelectedIndexChanged"
                                                    SelectedValue='<%# VerificaPresenzaProvincia(Container,Eval("CodicePROVINCIA").ToString()) %>'
                                                    SkinID="Insert_ddl" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Comune
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtCO" class="txtInputPopupUpper" runat="server" value='<%# Eval("CodiceCOMUNE") %>' />
                                                <asp:DropDownList runat="server" Width="185" ID="ddlComune" AppendDataBoundItems="true"
                                                    AutoPostBack="true" OnInit="ddlComune_OnInit" OnSelectedIndexChanged="ddlComuneRepeater_OnSelectedIndexChanged"
                                                    SelectedValue='<%# VerificaPresenzaComune(Container,Eval("CodiceCOMUNE").ToString()) %>'
                                                    SkinID="Insert_ddl" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Cap
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtCA" maxlength="10" class="txtInputPopup" runat="server" value='<%# Eval("Cap") %>' />
                                                <%--<asp:TextBox SkinID="Insert_txt" ID="txtCAP" runat="server" Text='<%# Eval("Cap") %>' />--%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Indirizzo
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtIN" maxlength="300" class="txtInputPopup" runat="server" value='<%# Eval("Indirizzo") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                C.D./Pec
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtEMP" maxlength="500" class="txtInputPopup" runat="server" value='<%# Eval("Emailpec") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Telefono
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtTE" maxlength="50" class="txtInputPopup" runat="server" value='<%# Eval("Telefono") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Cell
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtCE" maxlength="50" class="txtInputPopup" runat="server" value='<%# Eval("Cellulare") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Fax
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtFA" maxlength="200" class="txtInputPopup" runat="server" value='<%# Eval("Spare2") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Website
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtWE" maxlength="200" class="txtInputPopup" runat="server" value='<%# Eval("Spare1") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                PivaCF
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtPI" maxlength="20" class="txtInputPopup" runat="server" value='<%# Eval("Pivacf") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Professione
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtPF" maxlength="50" class="txtInputPopup" runat="server" value='<%# Eval("Professione") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Data Nascita
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtDN" maxlength="10" class="txtInputPopup" runat="server" value='<%# Eval("DataNascita") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Codici Sconto<br />
                                                (codice;perc%;..)
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtCS" maxlength="1000" class="txtInputPopup" runat="server" value='<%# Eval("Codicisconto") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Sesso
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">

                                                <asp:RadioButtonList runat="server" ID="radSesso" DataValueField='<%# Eval("Sesso") %>'>
                                                    <asp:ListItem Text="Valore Non Inserito" Value="" />
                                                    <asp:ListItem Text="Uomo" Value="uomo" />
                                                    <asp:ListItem Text="Donna" Value="donna" />
                                                </asp:RadioButtonList>

                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px; background-color: Black; color: White;">
                                            <div style="overflow: hidden; width: 80px">
                                                Consenso Commerciale
                                               
                                            </div>
                                        </td>
                                        <td>
                                            <div style="overflow: hidden; width: auto">
                                                <input id="txtC1" type="checkbox" runat="server" checked='<%# Eval("Consenso1") %>' />
                                            </div>
                                        </td>
                                    </tr>
                                </table>

                                <asp:Button ID="btnAggiorna" runat="server" UseSubmitBehavior="false" OnClick="Aggiorna_click"
                                    CommandArgument='<%# Eval("ID_CLIENTE") %>' Text="Aggiorna" />
                                <asp:Button ID="btCancella" runat="server" UseSubmitBehavior="false" Text="Cancella"
                                    OnClientClick="javascript:ConfermaCancella(this)" OnClick="Cancella_click" />
                                <hr />
                                <b>Utente  per accesso</b>
                                <asp:Literal Text="<%# GetUserData( Container.DataItem ) %>" runat="server" ID="litUserdata" />
                                <br />
                                <asp:Button ID="btnGenera" Style="float: left" runat="server" UseSubmitBehavior="false" OnClick="Generautente_click"
                                    CommandArgument='<%#  Eval("ID_CLIENTE") %>' Text="GeneraUtente" />
                                <asp:Button ID="Button2" Style="float: left" runat="server" UseSubmitBehavior="false" OnClick="EliminaUtente_click"
                                    CommandArgument='<%#  Eval("ID_CLIENTE") %>' Text="Elimina Utente" /><br />
                                <asp:Button ID="btnNewpass" Style="float: left" runat="server" UseSubmitBehavior="false" OnClick="Setnewpass_click"
                                    CommandArgument='<%#  Eval("ID_CLIENTE") %>' Text="Imposta Password" />
                                <input id="txtNewpass" style="float: left" maxlength="10" class="txtNewpass" runat="server" value='' /><br />
                                <div style="clear: both"></div>
                                <span style="font-size: 16px">
                                    <asp:Literal Text="" ID="litMsRow" runat="server" />
                                    <asp:Literal Text="" ID="outRow" runat="server" />
                                </span>
                            </div>
                        </asp:Panel>
                        <Ajax:PopupControlExtender ID="hoe2" TargetControlID="pnlRiga" PopupControlID="PopupMenu"
                            runat="server" Position="Bottom" OffsetX="50" OffsetY="0">
                        </Ajax:PopupControlExtender>
                        <asp:Panel runat="server" ID="pnlRiga">

                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50px">
                                        <div style="overflow: hidden; width: 50px">
                                            <asp:TextBox Width="100%" SkinID="Insert_txt" MaxLength="8" ReadOnly="true" ID="txtCod"
                                                runat="server" Text='<%# Eval("ID_CLIENTE") %>' />
                                        </div>
                                    </td>
                                    <td>
                                        <div style="overflow: hidden;">
                                            <asp:TextBox Width="100%" MaxLength="200" SkinID="Insert_txt" ID="txtCG" runat="server"
                                                Text='<%# Eval("Cognome") %>' />
                                        </div>
                                    </td>
                                    <td style="width: 150px">
                                        <div style="overflow: hidden; width: 150px">
                                            <asp:TextBox Width="100%" MaxLength="200" SkinID="Insert_txt" ID="txtNO" runat="server"
                                                Text='<%# Eval("Nome") %>' />
                                        </div>
                                    </td>
                                    <td style="width: 160px">
                                        <div style="overflow: hidden; width: 160px">
                                            <asp:TextBox Width="100%" MaxLength="100" SkinID="Insert_txt" ID="txtEM" runat="server"
                                                Text='<%# Eval("Email") %>' />
                                        </div>
                                    </td>
                                    <td style="width: 100px">
                                        <div style="overflow: hidden; width: 100px">
                                            <asp:DropDownList Width="100%" runat="server" ID="ddlLingua" SelectedValue='<%# Eval("Lingua") %>'
                                                SkinID="Insert_ddl">
                                                <asp:ListItem Text="Italiano" Value="I" Selected="True" />
                                                <asp:ListItem Text="Inglese" Value="GB" />
                                                <asp:ListItem Text="Russo" Value="RU" />
                                                <asp:ListItem Text="Danese" Value="DK" />
                                            </asp:DropDownList>
                                            <%-- <asp:TextBox Width="100%" MaxLength="15" SkinID="Insert_txt" ID="txtLN" runat="server"
                                                Text='<%# Eval("Lingua") %>' />--%>
                                        </div>
                                    </td>
                                    <td style="width: 150px">
                                        <div style="overflow: hidden; width: 150px">
                                            <asp:DropDownList runat="server" ID="ddlTipiClienti" AppendDataBoundItems="true" Width="100%" SelectedValue='<%# Eval("id_tipi_clienti") %>'
                                                OnInit="ddlTipiClienti_OnInit" SkinID="Insert_ddl" />
                                        </div>
                                    </td>
                                    <td style="width: 40px">
                                        <div style="overflow: hidden; width: 40px; text-align: center">
                                            <asp:CheckBox ID="chkVD" runat="server" Checked='<%# Eval("Validato") %>' />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:Repeater>
                <div style="float: right">

                    <UC:PagerEx ID="PagerRisultati" runat="server" NavigateUrl="" PageSize="20" CurrentPage="1"
                        TotalRecords="0" dimensioneGruppo="20" nGruppoPagine="1" OnPageCommand="PagerRisultati_PageCommand"
                        OnPageGroupClickNext="PagerRisultati_PageGroupClickNext" OnPageGroupClickPrev="PagerRisultati_PageGroupClickPrev" />

                </div>
                <div style="clear: both">
                </div>
                <asp:Button ID="btnNuovo" runat="server" UseSubmitBehavior="false" Text="Nuovo Cliente" />
                <%-- <Ajax:HoverMenuExtender ID="hme2" runat="Server" TargetControlID="btnNuovo" PopupControlID="PopupMenu"
                HoverCssClass="popupHover" PopupPosition="Bottom" OffsetX="50" OffsetY="0" PopDelay="100" />--%>
                <Ajax:PopupControlExtender ID="hoe3" TargetControlID="btnNuovo" PopupControlID="PopupMenu1"
                    runat="server" Position="Bottom" OffsetX="50" OffsetY="0" />
                <asp:Panel ID="PopupMenu1" CssClass="popupMenu" runat="server"
                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1">
                    <div style="max-width: 850px; width: 100%">
                        <table style="width: 100%; background-color: Black; color: White; font-size: 12pt">
                            <tr>
                                <td style="width: 40px">
                                    <div style="width: 40px">Codice </div>
                                </td>
                                <td>
                                    <div>
                                        Cognome/RagSoc
                                        <asp:RequiredFieldValidator ID="reqRagsoc" ErrorMessage="*" ValidationGroup="inserimento"
                                            runat="server" ControlToValidate="txtCG"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td style="width: 150px">
                                    <div style="width: 100px">
                                        Nome
                                       
                                    </div>
                                </td>
                                <td style="width: 160px">
                                    <div style="width: 160px">
                                        EMAIL
                                       
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ValidationGroup="inserimento"
                                                runat="server" ControlToValidate="txtEM"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td style="width: 70px">
                                    <div style="width: 70px">
                                        Lingua
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ValidationGroup="inserimento"
                                            runat="server" ControlToValidate="ddlLingua"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td style="width: 80px">
                                    <div style="width: 80px">
                                        Tipo Cliente 
                                    </div>
                                </td>
                                <td style="width: 40px">
                                    <div style="width: 40px">
                                        Validato
                                       
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 40px">
                                    <div style="overflow: hidden; width: 40px">
                                        <asp:TextBox Width="100%" SkinID="Insert_txt" CssClass="txtInputPopupUpper" MaxLength="8"
                                            ReadOnly="true" ID="txtCod" runat="server" Text="" />
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden;">
                                        <asp:TextBox Width="100%" MaxLength="200" CssClass="txtInputPopup" SkinID="Insert_txt"
                                            ID="txtCG" runat="server" Text="" />
                                    </div>
                                </td>
                                <td style="width: 150px">
                                    <div style="overflow: hidden; width: 150px">
                                        <asp:TextBox Width="100%" MaxLength="200" SkinID="Insert_txt" ID="txtNO" runat="server"
                                            Text="" CssClass="txtInputPopup" />
                                    </div>
                                </td>
                                <td style="width: 160px">
                                    <div style="overflow: hidden; width: 160px">
                                        <asp:TextBox Width="100%" MaxLength="100" SkinID="Insert_txt" ID="txtEM" runat="server"
                                            Text="" CssClass="txtInputPopup" />
                                    </div>
                                </td>
                                <td style="width: 70px">
                                    <div style="overflow: hidden; width: 70px">
                                        <asp:DropDownList Width="100%" runat="server" ID="ddlLingua" SkinID="Insert_ddl">
                                            <asp:ListItem Text="Italiano" Value="I" Selected="True" />
                                            <asp:ListItem Text="Inglese" Value="GB" />
                                            <asp:ListItem Text="Russo" Value="RU" />
                                            <asp:ListItem Text="Danese" Value="DK" />
                                        </asp:DropDownList>
                                        <%--<asp:TextBox Width="100%" MaxLength="15" SkinID="Insert_txt" ID="txtLN" runat="server"
                                            Text="" CssClass="txtInputPopupUpper" />--%>
                                    </div>
                                </td>
                                <td style="width: 80px">
                                    <div style="overflow: hidden; width: 80px">
                                        <asp:DropDownList runat="server" ID="ddlTipiClienti" Width="100%" SkinID="Insert_ddl" AppendDataBoundItems="true" />
                                        <%-- <asp:TextBox Width="100%" MaxLength="10" SkinID="Insert_txt" ID="txtTC" runat="server"
                                            Text='<%# Eval("id_tipi_clienti") %>' CssClass="txtInputPopup" />--%>
                                    </div>
                                </td>
                                <td style="width: 40px">
                                    <div style="overflow: hidden; width: 40px; text-align: center">
                                        <asp:CheckBox ID="chkVD" runat="server" Checked='<%# Eval("Validato") %>' />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 33%; float: left">
                        <div style="text-align: center; width: 97%; background-color: Black; color: White;">
                            DETTAGLIO DATI ANAGRAFICI CLIENTE
                           
                        </div>
                        <table style="width: 840px">
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Nazione
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <asp:DropDownList runat="server" ID="ddlNazione" AppendDataBoundItems="true" AutoPostBack="true"
                                            SkinID="Insert_ddl" OnSelectedIndexChanged="ddlNazione_SelectedIndexChanged" />
                                        <%-- <input class="txtInputPopup" maxlength="2" id="txtNA" runat="server" value="" />--%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Regione
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <asp:DropDownList runat="server" ID="ddlRegione" AppendDataBoundItems="true" AutoPostBack="true"
                                            SkinID="Insert_ddl" OnSelectedIndexChanged="ddlRegione_SelectedIndexChanged" />
                                        <input id="txtRE" maxlength="100" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Provincia
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <asp:DropDownList runat="server" ID="ddlProvincia" AppendDataBoundItems="true" AutoPostBack="true"
                                            SkinID="Insert_ddl" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" />
                                        <input id="txtPR" maxlength="100" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Comune
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <asp:DropDownList runat="server" ID="ddlComune" AppendDataBoundItems="true" SkinID="Insert_ddl"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlComune_SelectedIndexChanged" />
                                        <input id="txtCO" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Cap
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtCA" maxlength="10" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Indirizzo
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtIN" maxlength="300" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        C.D./Pec
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtEMP" maxlength="500" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Telefono
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtTE" maxlength="50" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Cell
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtCE" maxlength="50" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Fax
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtFA" maxlength="200" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Website
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtWE" maxlength="200" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        PivaCF
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtPI" maxlength="20" class="txtInputPopupUpper" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Professione
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtPF" maxlength="50" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Data Nascita
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtDN" maxlength="10" class="txtInputPopup" runat="server" value="" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 80px">
                                        Codici Sconto ( codice;percentuale;... )
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtCS" maxlength="1000" class="txtInputPopup" runat="server" value='' />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Sesso
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">

                                        <asp:RadioButtonList runat="server" ID="radSesso">
                                            <asp:ListItem Text="Selezionare Valore" Value="" />
                                            <asp:ListItem Text="Uomo" Value="Uomo" />
                                            <asp:ListItem Text="Donna" Value="Donna" />
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                            </tr>



                            <tr>
                                <td style="width: 200px; background-color: Black; color: White;">
                                    <div style="overflow: hidden; width: 100%">
                                        Consenso Commerciale
                                       
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: hidden; width: 100%">
                                        <input id="txtC1" type="checkbox" runat="server" checked="false" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnInserisci" runat="server" ValidationGroup="inserimento" UseSubmitBehavior="false"
                            Text="Inserisci Cliente" OnClick="Inserisci_click" />
                    </div>
                </asp:Panel>
                <div style="clear: both;"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div style="height: auto; border: 1px solid black; padding: 10px; position: relative">

            <div class="pull-left">
                <asp:Label Text="<h1>Importazione liste clienti TIPO 1</h1><br/>(FORMATO ELENCO : nome1,email1,nome2,email2 .... usando come separatore la , o il ;)"
                    runat="server" />
                <br />
                <asp:TextBox runat="server" TextMode="MultiLine" Height="200" Width="600" ID="txtImporta" /><br />
                <br />
                <br />
            </div>
            <div class="pull-left">
                <asp:Label Text="<h1>Importazione liste clienti TIPO 2</h1><br/>(FORMATO ELENCO : nome1,cognome1,email1,nome2,cognome2,email2 .... usando come separatore la , o il ;)"
                    runat="server" />
                <br />
                <asp:TextBox runat="server" TextMode="MultiLine" Height="200" Width="600" ID="txtImporta1" /><br />
            </div>
            <div class="clearfix"></div>
            <br />
            <div>
                Carica file Excel Per importazioni clienti:
                           
                    <asp:FileUpload runat="server" ID="uplFile" />
                <asp:Button Text="Carica file importazione" runat="server" ID="Button1" OnClick="btnUploadFile_Click" CausesValidation="false" />
                <br />
                <asp:Button Text="Importa Clienti da file" runat="server" ID="btnParse" OnClick="btnParse_Click" CausesValidation="false" />
                <br />
                <br />
            </div>

            <div style="width: 200px">
                <asp:Label ID="Label1" Text="Lingua Clienti per importazione" runat="server" />
            </div>
            <asp:DropDownList runat="server" ID="ddlLinguaImporta">
                <asp:ListItem Text="Italiano" Value="I" Selected="True" />
                <asp:ListItem Text="Inglese" Value="GB" />
                <asp:ListItem Text="Russo" Value="RU" />
                <asp:ListItem Text="Danese" Value="DK" />
            </asp:DropDownList>
            <br />



            <div style="width: 200px">
                <asp:Label ID="Label2" Text="Tipo Clienti per importazione" runat="server" />
            </div>
            <asp:DropDownList runat="server" ID="ddlTipiClientiImporta" AppendDataBoundItems="true" OnInit="ddlTipiClientiImporta_OnInit" />
            <br />
            <br />
            <asp:Button Text="Cancella Tutti Clienti della Tipologia" UseSubmitBehavior="false" OnClientClick="javascript:ConfermaCancellaClienti(this)" runat="server" ID="cancellaClientiTipologia" OnClick="cancellaClientiTipologia_Click" />
            <br />
            <br />
            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Esporta Archivio Clienti" />
            <br />
            <br />


            <div>
                <asp:CheckBox ID="chkCommercialeImporta" Text="Consenso Commerciale" runat="server"
                    Checked="false" /><br />
                <asp:CheckBox ID="chkPrivacyImporta" Text="Consenso Privacy" runat="server" Checked="false" />
            </div>
            <br />
            <asp:Button Text="Importa" runat="server" OnClick="btnImporta_onclick" ID="btnImporta" />
            <%--  <asp:UpdateProgress ID="updateprogress1" runat="server" DisplayAfter="0" DynamicLayout="false"
                            AssociatedUpdatePanelID="updpanel1">
                            <ProgressTemplate>
                                <div style="position: absolute; background-color: white; color: black; padding: 2px">
                                    <img id="Img2" runat="server" alt="" src="~/Images/indicator.gif" />Attendi...
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>


            <br />
            <asp:Literal Text="" runat="server" ID="outputimporta" />
            <br />
            <h3>Gestione tipi di cliente</h3>
            <asp:DropDownList runat="server" Width="310px" ID="ddlTipiClientiUpdate" AutoPostBack="true"
                AppendDataBoundItems="true" OnSelectedIndexChanged="TipoClienteUpdate">
            </asp:DropDownList>
            <asp:TextBox runat="server" ID="txtTipoClienteUpdate" Text="" Width="200" />
            <Ajax:TextBoxWatermarkExtender runat="server" WatermarkText="Inserire nuovo tipo cliente" TargetControlID="txtTipoClienteUpdate">
            </Ajax:TextBoxWatermarkExtender>
            <asp:Button Text="Aggiorna" ID="btnTipiClienti" runat="server" OnClick="btnTipiClienti_Click" />
            <br />

        </div>

    </div>

</asp:Content>
