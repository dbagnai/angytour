<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GestioneSoci.aspx.cs"
    Inherits="AreaContenuti_GestioneSoci" MaintainScrollPositionOnPostback="true" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestione Offerte</title>
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
</head>
<body style="font-size: 13px; background-color: #ccc; padding: 10px 10px 10px 10px">
    <form id="form1" runat="server">
        <Ajax:ToolkitScriptManager ID="ScriptManagerMaster" runat="server" AllowCustomErrorsRedirect="True"
            AsyncPostBackErrorMessage="Errore generico. Contattare HelpDesk" AsyncPostBackTimeout="400"
            EnablePartialRendering="true" EnablePageMethods="true" EnableScriptLocalization="true"
            EnableScriptGlobalization="true">
            <Scripts>
                <asp:ScriptReference Path="~/js/jquery-1.10.2.min.js" NotifyScriptLoaded="true" />
                <asp:ScriptReference Path="~/js/jquery-migrate-1.2.1.min.js" NotifyScriptLoaded="true" />
                <asp:ScriptReference Path="~/js/bootstrap.min.js" NotifyScriptLoaded="true" />
                <asp:ScriptReference Path="~/js/back-to-top.js" NotifyScriptLoaded="true" />
            </Scripts>
        </Ajax:ToolkitScriptManager>
        <!-- BOOTSTRAP -->
        <link rel="stylesheet" type="text/css" href="<%= CommonPage.ReplaceAbsoluteLinks("~/css/bootstrap.css") %>" />
        <link rel="stylesheet" type="text/css" href="<%= CommonPage.ReplaceAbsoluteLinks("~/bootstrap.css/font-awesome.min.css") %>" />

        <![endif]-->

        <script type="text/javascript">
            function ConfirmCancella() {
                //document.getElementByID("CheckBoxListExCtrl").value
                var conferma = confirm('Sei sicuro di voler cancellare ?');
                if (conferma) {
                    $get("<%=cancelHidden.ClientID%>").value = "true";
                }
                else {
                    $get("<%=cancelHidden.ClientID%>").value = "false";
                }
            }
        </script>
        <%--  <link href="../App_Themes/TemaPortale1/StylePortale1.css" rel="stylesheet" type="text/css" />--%>
        <div class="wrapper">

            <div class="container content" style="background-color: #ffffff; padding-bottom: 50px">

                <div style="width: 100%; height: 30px; background-color: #2e3192; color: white; text-align: center; margin: 0px 0px; padding: 0px 0px">
                    <h2>
                        <asp:Literal ID="litTitle" runat="server" Text="Sezione amministrazione soci"></asp:Literal></h2>
                </div>
                <br />
                <a class="btn btn-success" href="Default.aspx" style="font-size: 15px">Torna a pagina di selezione</a>
                <asp:HiddenField ID="cancelHidden" runat="server" Value="false" />
                <h2>
                    <asp:Literal ID="litTitolo" runat="server"></asp:Literal></h2>
                <h3 style="color: red">
                    <asp:Literal ID="output" runat="server"></asp:Literal></h3>
                <asp:Panel runat="server" ID="pnlRicerca">
                    <script type="text/javascript">
                        function stampalista() {
                            $get("btnStampa").click();
                        }</script>
                    <div class="row">
                        <div class="col-sm-12">
                            <a class="btn btn-primary btn-small pull-right" href='<%= WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/AspNetPages/formStampa.aspx" %>'
                                target="_blank" style="text-decoration: none;" onclick="javascript:stampalista();">Stampa Soci Per Fatturazione</a>
                            <asp:Button Style="display: none" Text="" ID="btnStampa" OnClick="btnStampa_Click" runat="server" CommandArgument="" />
                            <br />
                        </div>
                    </div>
                    <hr />
                    <div class="row" style="background-color: #eee; padding: 1%; border: 1px solid white; margin-right: 1%; margin-left: 1%">
                        <div class="col-sm-8">
                            <h2>Ricerca Socio</h2>

                            <asp:TextBox runat="server" Width="100%" ID="txtinputCerca" />
                            <Ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtinputCerca"
                                runat="server" WatermarkText='<%# references.ResMan("Common", Lingua,"wmarkCerca") %>'>
                            </Ajax:TextBoxWatermarkExtender>
                            <asp:TextBox runat="server" ID="txtinputmese" Visible="false" />
                            <Ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtinputmese"
                                runat="server" WatermarkText="mese">
                            </Ajax:TextBoxWatermarkExtender>
                            <Ajax:FilteredTextBoxExtender ID="ftbe2" runat="server" TargetControlID="txtinputmese"
                                FilterType="Custom, Numbers" ValidChars="0123456789" />
                            <asp:TextBox runat="server" ID="txtinputanno" Visible="false" />
                            <Ajax:FilteredTextBoxExtender ID="ftbe1" runat="server" TargetControlID="txtinputanno"
                                FilterType="Custom, Numbers" ValidChars="0123456789" />
                            <Ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtinputanno"
                                runat="server" WatermarkText="anno">
                            </Ajax:TextBoxWatermarkExtender>
                            <br />
                            <br />
                            Filtro stato pagamento per anno:
                            <asp:TextBox runat="server" ID="txtinputannopagamenti" Visible="true" Text="<%# System.DateTime.Now.Year.ToString() %>" />
                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtinputannopagamenti"
                                FilterType="Custom, Numbers" ValidChars="0123456789" />
                            <Ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender14" TargetControlID="txtinputannopagamenti"
                                runat="server" WatermarkText="anno pagamento">
                            </Ajax:TextBoxWatermarkExtender>
                            <asp:RadioButtonList runat="server" ID="rdPagamenti" AppendDataBoundItems="true" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Tutti" Value="" Selected="True" />
                                <asp:ListItem Text="Pagato" Value="true" />
                                <asp:ListItem Text="Non Pagato" Value="false" />
                            </asp:RadioButtonList>
                            <br />
                            <asp:RadioButtonList runat="server" ID="rdBloccati" AppendDataBoundItems="true" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Tutti" Value="" Selected="True" />
                                <asp:ListItem Text="Bloccati" Value="true" />
                                <asp:ListItem Text="Non Bloccati" Value="false" />
                            </asp:RadioButtonList>
                            <br />
                            Filtro metodiche:
                            <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlFiltroTrattamenti">
                                <asp:ListItem Text="Seleziona trattamento" Value="" />
                            </asp:DropDownList>

                            <br />
                            <br />
                            Ordina per:
                            <asp:RadioButtonList runat="server" ID="radOrdinamento" AppendDataBoundItems="true" RepeatDirection="Horizontal">
                                <asp:ListItem Text="DataInserimento" Value="" Selected="True" />
                                <asp:ListItem Text="Cognome" Value="Cognome_dts" />
                            </asp:RadioButtonList>
                            <br />
                            <br />

                            <asp:Button Text="Cerca" ID="btnCerca" runat="server" CssClass="btn btn-success" OnClick="btnCerca_Click" />

                        </div>

                        <div class="col-sm-4">
                            <strong>Paese*</strong>
                            <br />
                            <asp:DropDownList runat="server" Width="60%" ID="ddlNazioneRicerca" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlNazioneRicerca_OnSelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <br />
                            </span>

                            <strong>Regione*</strong>
                            <br />
                            <asp:DropDownList runat="server" Width="60%" ID="ddlRegioneRicerca" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlRegioneRicerca_OnSelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList><br />
                            <div style="display: none">
                                <input id="txtReRicerca" style="width: 60%" runat="server" value="" />
                                <br />
                            </div>
                            <strong>Provincia*</strong>
                            <br />
                            <asp:DropDownList runat="server" Width="60%" ID="ddlProvinciaRicerca" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProvinciaRicerca_OnSelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList><br />
                            <div style="display: none">
                                <input id="txtPrRicerca" style="width: 60%" runat="server" value="" />
                            </div>
                            <br />
                            <strong>Comune*</strong>
                            <br />
                            <asp:DropDownList runat="server" Width="60%" ID="ddlComuneRicerca" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlComuneRicerca_OnSelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList><br />
                            <div style="display: none">
                                <input id="txtCoRicerca" style="width: 60%" runat="server" value="" />
                                <br />
                            </div>
                        </div>
                    </div>

                    <div class="row" style="background-color: #2e3192; color: white; margin-right: 1%; margin-left: 1%">
                        <div class="col-xs-1"></div>
                        <div class="col-xs-1">Id</div>
                        <div class="col-xs-2">Cognome Nome</div>
                        <div class="col-xs-2">Email</div>
                        <div class="col-xs-2">Email(riserv)</div>
                        <div class="col-xs-1">Denomin.</div>
                        <div class="col-xs-1">Categoria</div>


                        <div class="col-xs-1">Archiviato</div>
                        <div class="col-xs-1">Bloccato</div>
                    </div>

                    <asp:Repeater runat="server" ID="rptOfferte" OnItemDataBound="rptOfferte_ItemDataBound">
                        <ItemTemplate>
                            <div class="row" style="margin-right: 1%; margin-left: 1%">
                                <div class="col-xs-1" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:ImageButton ID="imgSelect" Width="20" ImageUrl="~/images/search_icone.jpg" runat="server"
                                        CommandArgument='<%# Eval("Id").ToString() %>' OnPreRender="ImageButton1_PreRender"
                                        OnClick="link_click" ToolTip='<%# Eval("Id").ToString() %>' />
                                </div>
                                <div class="col-xs-1" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:Literal ID="Literal4" runat="server" Text='<%# Eval("Id").ToString() %>'></asp:Literal>
                                </div>
                                <div class="col-xs-2" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:Literal ID="Literal5" runat="server" Text='<%# Eval("Cognome_dts").ToString() + " "  +  Eval("Nome_dts").ToString()   %>'></asp:Literal>
                                </div>
                                <div class="col-xs-2" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Email").ToString()  %>'></asp:Literal>
                                </div>
                                <div class="col-xs-2" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:Literal ID="Literal6" runat="server" Text='<%# Eval("Emailriservata_dts").ToString()   %>'></asp:Literal>
                                </div>
                                <div class="col-xs-1" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:Literal ID="lit1" runat="server" Text='<%# Eval("DenominazioneI").ToString() %>'></asp:Literal>
                                </div>
                                <div class="col-xs-1" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:Literal ID="Literal2" runat="server" Text='<%# CategoriaSocio( Eval("Caratteristica3").ToString() ) %>'></asp:Literal>
                                </div>
                                <div class="col-xs-1" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:CheckBox ID="chk1" runat="server" Enabled="false" Checked='<%# Eval("Archiviato") %>' />
                                </div>
                                <div class="col-xs-1" style="border: 1px solid #ccc; padding: 3px; height: 40px; overflow-y: auto">
                                    <asp:CheckBox ID="CheckBox1" runat="server" Enabled="false" Checked='<%# Eval("Bloccoaccesso_dts") %>' />
                                </div>

                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="row" style="margin-right: 1%; margin-left: 1%">
                        <div id="pager" class="divPager">
                            <UC:PagerEx ID="PagerRisultati" runat="server" NavigateUrl="" PageSize="50" CurrentPage="1"
                                TotalRecords="0" dimensioneGruppo="20" nGruppoPagine="1" OnPageCommand="PagerRisultati_PageCommand"
                                OnPageGroupClickNext="PagerRisultati_PageGroupClickNext" OnPageGroupClickPrev="PagerRisultati_PageGroupClickPrev" />
                        </div>
                    </div>
                </asp:Panel>
                <div class="row" style="margin-left: 20px">
                    <h2>Scheda Socio</h2>
                    <hr />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Insertvalidate" />
                    <div class="pull-left" style="margin-right: 10px">
                        <asp:Panel runat="server" ID="pnlSoloamministratori1">
                            <asp:Button CssClass="btn btn-success" ID="btnNuovo" runat="server" Text="Nuovo" OnClick="btnNuovo_Click" />
                        </asp:Panel>
                    </div>
                    <div class="pull-left" style="margin-right: 10px">
                        <asp:Button CssClass="btn btn-success" ID="btnAggiorna" runat="server" Text="Modifica" OnClick="btnAggiorna_Click" ValidationGroup="Insertvalidate" />
                    </div>
                    <div class="pull-left" style="margin-right: 10px">
                        <asp:Panel runat="server" ID="pnlSoloamministratori2">
                            <asp:Button CssClass="btn btn-success" ID="btnCancella" runat="server" Text="Cancella" OnClick="btnCancella_Click"
                                OnClientClick="javascript:ConfirmCancella()" UseSubmitBehavior="true" />
                        </asp:Panel>
                    </div>
                    <div class="pull-left" style="margin-right: 10px">
                        <asp:Button CssClass="btn btn-success" ID="btnAnnulla" runat="server" Text="Annulla" OnClick="btnAnnulla_Click"
                            Visible="false" />
                    </div>
                </div>
                <div class="row" style="background-color: #eee; margin: 5px; padding: 15px">
                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label31" runat="server" Text="Nome*" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtNome_dts" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Nome Obbligatorio"
                                ControlToValidate="txtNome_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label32" runat="server" Text="Cognome*" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtCognome_dts" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Cognome Obbligatorio"
                                ControlToValidate="txtCognome_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                    </div>

                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label33" runat="server" Text="Data Nascita" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtDatanascita_dts" runat="server"></asp:TextBox>
                            <Ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDatanascita_dts">
                            </Ajax:CalendarExtender>
                        </div>
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label34" runat="server" Text="Codice Fiscale / P.Iva" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtPivacf_dts" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <%--                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label35" runat="server" Text="Socio Presentatore Effettivo 1*" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtSociopresentatore1_dts" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTestoS1Err") %>'
                                ControlToValidate="txtSociopresentatore1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                            <br />
                            <i>inserire i nominativi dei due soci presentatori</i>
                        </div>
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label36" runat="server" Text="Socio Presentatore Effettivo 2*" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtSociopresentatore2_dts" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTestoS2Err") %>'
                                ControlToValidate="txtSociopresentatore2_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                    </div>--%>
                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label37" runat="server" Text="Email Pubblica" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtEmail" runat="server"></asp:TextBox>
                            <br />
                            <i>email pubblica</i>
                        </div>
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label38" runat="server" Text="Sito Internet" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtWebsite" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-12">
                            <strong>Indirizzo email riservato *</strong>
                            <asp:TextBox runat="server" ID="txtEmailriservata_dts" /><br />
                            <i>Un indirizzo e-mail valido. Il sistema invierà tutte le e-mail a questo indirizzo.
                                L'indirizzo e-mail non sarà pubblico e verrà utilizzato soltanto se desideri ricevere una nuova password o se vuoi ricevere notizie e avvisi via e-mail.</i>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Email riservata Obbligatoria"
                                ControlToValidate="txtEmailriservata_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label4" runat="server" Text="Telefono" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtTelefono" runat="server"></asp:TextBox><br />
                            <strong><asp:Label ID="Label6" runat="server" Text="Fax" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtFax" runat="server"></asp:TextBox>
                            <br />
                            <i>telefono visibile al pubblico</i>
                        </div>
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label5" runat="server" Text="Telefono privato*" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtTelefonoprivato_dts" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage="Telefono privato obbligatorio"
                                ControlToValidate="txtTelefonoprivato_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                    </div>

                    <div class="row" style="margin-bottom: 15px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label27" runat="server" Text="Categoria Socio Gist*" /></strong><br />
                            <asp:DropDownList Width="90%" AppendDataBoundItems="true" ID="ddlCaratteristica3" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ErrorMessage="indicazione categoria socio Gist Obbligatoria"
                                ControlToValidate="ddlCaratteristica3" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                        <div class="col-sm-6"></div>
                    </div>

                    <%--  <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label6" runat="server" Text="Anno Laurea*" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtAnnolaurea_dts" runat="server"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="Anno laurea obbligatorio"
                                ControlToValidate="txtAnnolaurea_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label7" runat="server" Text="Anno Specializzazione in Chirurgia Plastica" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtAnnospecializzazione_dts" runat="server"></asp:TextBox>
                            <br />
                            <i>Nel caso di non specializzazione caricare il certificato di equipollenza ( sotto )</i>
                        </div>
                    </div>--%>


                    <%--                    <div runat="server" id="divCEQUIP">
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-6">
                                <strong>
                                    <asp:Label ID="Label39" runat="server" Text="Altre Specializzazioni" /></strong><br />
                                <asp:TextBox Width="90%" ID="txtAltrespecializzazioni_dts" runat="server"></asp:TextBox><br />
                            </div>
                            <div class="col-sm-6" style="border: 1px solid White;">
                                <strong>Certificazione di Equivalenza a Specialista in Chirurgia Plastica </strong>
                                <asp:FileUpload runat="server" ID="UploadCEQUIP" />
                                <asp:Panel runat="server" ID="pnlCEQUIP">
                                    <asp:Literal ID="litCEQUIP" Text="" runat="server" />
                                    <asp:LinkButton ToolTip="Elimina File" Text="" ID="lnkCEQUIP" runat="server" OnClick="lnkCEQUIP_Click" />
                                    <br />
                                </asp:Panel>
                                <i>Per i non specialisti in chirurgia plastica specificare quanto necessario ai fini di una valutazione per certificazione di un’equivalenza (specializzazioni, esperienze professionali, casistica operatoria comprovabile, pubblicazioni etc.)
                                    <br />
                                    I file devono pesare meno di 10 MB.<br />
                                    Tipi di file permessi: pdf doc rtf jpg jpeg tiff.</i>
                            </div>
                        </div>
                    </div>--%>
                    <%--  <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-12">
                            <strong>
                                <asp:Label ID="Label43" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoAlbo1") %>' /></strong>
                            <asp:TextBox Width="20%" ID="txtLocordine_dts" runat="server"></asp:TextBox>


                            <strong>
                                <asp:Label ID="Label44" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoAlbo2") %>' /></strong>
                            <asp:TextBox Width="20%" ID="txtNiscrordine_dts" runat="server"></asp:TextBox>

                        </div>
                    </div>--%>
                    <%-- <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-12">
                            <strong>
                                <asp:Label ID="Label46" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoSpecializzandi1") %>' />
                            </strong>
                            <br />
                            <asp:Label ID="Label47" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoSpecializzandi2") %>' />
                            <asp:TextBox Width="10%" ID="txtannofrequenza_dts" runat="server"></asp:TextBox>
                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtannofrequenza_dts"
                                FilterType="Custom, Numbers" ValidChars="0123456789" />

                            <asp:Label ID="Label48" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoSpecializzandi3") %>' />
                            <asp:TextBox Width="15%" ID="txtnomeuniversita_dts" runat="server"></asp:TextBox>
                            <asp:Label ID="Label45" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoSpecializzandi4") %>' />
                            <asp:TextBox Width="15%" ID="txtdettagliuniversita_dts" runat="server"></asp:TextBox><br />
                        </div>
                    </div>--%>

                    <%-- <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label23" runat="server" Text="Categoria Professionale*" /></strong><br />
                            <asp:DropDownList Width="90%" AppendDataBoundItems="true" ID="ddlCaratteristica1" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ErrorMessage="Categoria Professionale obbligatoria"
                                ControlToValidate="ddlCaratteristica1" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                        <div class="col-sm-6"></div>
                    </div>--%>

                    <%--                    <div class="row" style="margin-bottom: 5px" runat="server" visible="false">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label40" runat="server" Text="Socio Sicpre*" /></strong><br />
                            <asp:RadioButtonList ID="radSocioSicpre_dts" runat="server">
                                <asp:ListItem Text="Si" Value="True" />
                                <asp:ListItem Text="No" Value="False" />
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label24" runat="server" Text="Categoria Sicpre" /></strong><br />
                            <asp:DropDownList Width="90%" AppendDataBoundItems="true" ID="ddlCaratteristica2" runat="server" />
                        </div>
                    </div>--%>
                    <%-- <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label41" runat="server" Text="Socio Isaps" /></strong><br />
                            <asp:RadioButtonList ID="radSocioIsaps_dts" runat="server">
                                <asp:ListItem Text="Si" Value="True" />
                                <asp:ListItem Text="No" Value="False" />
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ErrorMessage="indicazione socio Isaps Obbligatoria"
                                ControlToValidate="radSocioIsaps_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />

                        </div>
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label42" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoSocioaltra") %>" /></strong><br />
                            <asp:TextBox Width="90%" ID="txtSocioaltraassociazione_dts" runat="server"></asp:TextBox><br />
                            <i>Inserire i nomi delle associazione separati da una virgola</i>
                        </div>
                    </div>--%>


                    <%--ESPERIENZE E INFO SU CARRIERA PROFESSIONALE Boolfields_dts--%>
                    <%--   <div class="row" style="margin-bottom: 5px; border-bottom: 1px solid #999999; border-top: 1px solid #999999">
                        <div class="col-sm-8">
                            <strong>
                                <asp:Label ID="Label49" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera1") %>" /></strong><br />
                        </div>
                        <div class="col-sm-4">
                            <asp:RadioButtonList ID="radCarriera1" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Si" Value="True" />
                                <asp:ListItem Text="No" Value="False" />
                            </asp:RadioButtonList>
                        </div>
                    </div>--%>
                    <%--    <div class="row" style="margin-bottom: 5px; border-bottom: 1px solid #999999">
                        <div class="col-sm-6">
                            <strong>
                                <asp:Label ID="Label50" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera2") %>" /></strong><br />
                        </div>
                        <div class="col-sm-6">
                            <asp:TextBox Width="90%" Height="100px" TextMode="MultiLine" ID="txtTextfield1_dts" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px; border-bottom: 1px solid #999999">
                        <div class="col-sm-8">
                            <strong>
                                <asp:Label ID="Label51" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera3") %>" /></strong><br />
                        </div>
                        <div class="col-sm-4">
                            <asp:RadioButtonList ID="radCarriera2" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Si" Value="True" />
                                <asp:ListItem Text="No" Value="False" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px; border-bottom: 1px solid #999999">
                        <div class="col-sm-8">
                            <strong>
                                <asp:Label ID="Label52" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera3b") %>" /></strong><br />
                        </div>
                        <div class="col-sm-4">
                            <asp:RadioButtonList ID="radCarriera3" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Si" Value="True" />
                                <asp:ListItem Text="No" Value="False" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px; border-bottom: 1px solid #999999">
                        <div class="col-sm-8">
                            <strong>
                                <asp:Label ID="Label53" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera4") %>" /></strong><br />
                        </div>
                        <div class="col-sm-4">
                            <asp:RadioButtonList ID="radCarriera4" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Si" Value="True" />
                                <asp:ListItem Text="No" Value="False" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px; border-bottom: 1px solid #999999">
                        <div class="col-sm-8">
                            <strong>
                                <asp:Label ID="Label54" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera5")  %>" /></strong><br />
                        </div>
                        <div class="col-sm-4">
                            <asp:RadioButtonList ID="radCarriera5" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Si" Value="True" />
                                <asp:ListItem Text="No" Value="False" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px; border-bottom: 1px solid #999999">
                        <div class="col-sm-12">
                            <strong>
                                <asp:Label ID="Label55" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera5b")  %>" /></strong><br />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px; border-bottom: 1px solid #999999">
                        <div class="col-sm-12">
                            <strong>
                                <asp:Label ID="Label56" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera6")  %>" /></strong><br />
                        </div>
                    </div>
                    

                    <div class="table-responsive">
                        <table class="table table-bordered">
                            <tr>
                                <td>
                                    <strong>
                                        <asp:Label ID="Label58" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera7")  %>" /></strong>

                                </td>
                                <td colspan="4">
                                    <strong>
                                        <asp:Label ID="Label59" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera8")  %>" /></strong>

                                </td>
                                <td colspan="4">
                                    <strong>
                                        <asp:Label ID="Label60" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera9")  %>" /></strong>

                                </td>
                            </tr>
                          
                            <tr>
                                <td>
                                    <asp:Literal ID="litIntervento1" Text="Blefaroplastica" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op1" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a")  %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c")  %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a")  %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c")  %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal ID="litIntervento2" Text="Rinoplastica" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op2" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a")  %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c")  %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op2" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal ID="litIntervento3" Text="Face-Lifting" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op3" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op3" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal ID="litIntervento4" Text="Otoplastica" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op4" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a")  %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c")  %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op4" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal ID="litIntervento5" Text="Mastoplastica Additiva" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op5" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op5" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal ID="litIntervento6" Text="Mastopessi" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op6" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op6" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal ID="litIntervento7" Text="Mastoplastica Riduttiva" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op7" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op7" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal ID="litIntervento8" Text="Addominoplastica" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op8" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op8" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal ID="litIntervento9" Text="Liposuzione" runat="server" /></td>
                                <td colspan="4">
                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento1op9" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Table"    RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera8d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                             <td colspan="4">

                                <asp:RadioButtonList CssClass="table table-bordered" ID="radIntervento2op9" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"   RepeatColumns="4">
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9a") %>' Value="1" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9b") %>' Value="2" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9c") %>' Value="3" />
                                    <asp:ListItem Text='<%# references.ResMan("Common",Lingua,"FormTestoCarriera9d") %>' Value="4" />
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                            <asp:Panel runat="server" ID="pnlTblInterventi">  </asp:Panel>
                        </table>
                    </div>

                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-12">
                            <strong>
                                <asp:Label ID="Label57" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoCarriera10") %>' /></strong><br />
                            <br />
                        </div>
                    </div>--%>
                    <%-- FINE  --- ESPERIENZE E INFO SU CARRIERA PROFESSIONALE--%>

                    <style type="text/css">
                        .cboxspaced input {
                            margin-left: 0px;
                            display: block;
                        }

                        .cboxspaced label {
                            font-size: 0.9em;
                        }

                        .cboxspaced td {
                            padding-right: 20px;
                            margin-bottom: 30px;
                            margin-right: 20px;
                            margin-left: 20px;
                        }
                    </style>
                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-12" style="background-color: #eef">
                            <br />
                            <strong>INTERVENTI CHIRURGICI*
                            </strong>
                            <i>Selezionare gli interventi che il candidato/socio effettua</i><br />
                            <asp:CheckBoxList runat="server" ID="cbtrattamentilist" CssClass="cboxspaced" AppendDataBoundItems="true" RepeatColumns="2" RepeatDirection="Vertical" RepeatLayout="Table">
                            </asp:CheckBoxList>
                            <br />
                        </div>
                    </div>

                    <%--   <div class="row" style="margin-bottom: 5px" runat="server" id="divCurriculum">
                        <div class="col-sm-12" style="border: 1px solid White; margin: 1%">
                            <strong>Per la domanda di iscrizione il candidato/a dovrà allegare un Curriculum Vitae in formato europeo firmato( <a target="_blank" href='<%= ReplaceAbsoluteLinks( PercorsoComune + "/form_cv_europeo.doc" )  %>'>scarica Modello CV europeo</a> ):*</strong>
                            <asp:FileUpload runat="server" ID="UploadCv" />


                            <asp:Panel runat="server" ID="pnlCV">
                                <asp:Literal ID="litCV" Text="" runat="server" />
                                <asp:LinkButton ToolTip="Elimina File" Text="" ID="lnkCV" runat="server" OnClick="lnkCV_Click" />
                                <br />
                            </asp:Panel>

                            <i>Il Curriculum Vitae in formato europeo deve contenere i seguenti dati ( <a target="_blank" href='<%= ReplaceAbsoluteLinks( PercorsoComune + "/form_cv_europeo.doc" )  %>'>scarica Modello CV europeo</a>  ):
                                <br />

                                •Università frequentata (nome e luogo);<br />
                                •Esperienza post-laurea in Italia ;<br />
                                •Periodi di training all’estero;<br />
                                •Appartenenza a Società ed Associazioni;<br />
                                •Lavori scientifici pubblicati e/o presentati a congressi;<br />
                                •Onorificenze professionali;
                                •Connotazioni o notizie di importanza rilevante riguardanti la professione;<br />
                                <br />
                                I file devono pesare meno di 10 MB.<br />
                                Tipi di file permessi: pdf doc rtf jpg jpeg tiff.</i>
                        </div>
                    </div>--%>

                    <%--  <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-12">
                            <strong>Accettazione statuto *</strong>
                            <asp:CheckBox Text="" runat="server" ID="chkAccettazioneStatuto_dts" Checked="false" />
                            <asp:CustomValidator ID="CustomValidator1" Text="*" ErrorMessage="Accettazione statuto Obbligatoria" runat="server"
                                ClientValidationFunction="chkAccettazioneStatuto_dts_ClientValidate" ValidationGroup="Insertvalidate"></asp:CustomValidator>
                            <script type="text/javascript">
                                function chkAccettazioneStatuto_dts_ClientValidate(sender, args) {
                                    args.IsValid = document.getElementById('<%=chkAccettazioneStatuto_dts.ClientID%>').checked;
                                }
                            </script>

                            <br />
                            <i>Dichiaro di aver letto,compreso ed accettato <a target="_blank" href="-- link a pagina da inserire">lo statuto  ed il codice etico</a> e mi impegno a rispettarli, cosciente delle sanzioni in caso di non osservanza
                            </i>
                        </div>
                    </div>--%>
                    <%-- <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-12">
                            <string>Certificazione validità dei dati * </string>
                            <asp:CheckBox Text="" runat="server" ID="chkCertificazione_dts" Checked="false" />
                            <asp:CustomValidator ID="CustomValidator2" Text="*" ErrorMessage="Accettazione Certificazione Dati Obbligatoria" runat="server"
                                ClientValidationFunction="chkCertificazione_dts_ClientValidate" ValidationGroup="Insertvalidate"></asp:CustomValidator>
                            <script type="text/javascript">
                                function chkCertificazione_dts_ClientValidate(sender, args) {
                                    args.IsValid = document.getElementById('<%=chkCertificazione_dts.ClientID%>').checked;
                                }
                            </script>
                            <br />
                            <i>Io sottoscritto certifico la validità delle informazioni presentate e sono al corrente che l’eventuale non veridicità di alcune di esse può comportare la non accettazione o l’espulsione dall’AICPE.</i>
                        </div>
                    </div>--%>

                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>Indirizzo Residenza</strong><br />
                            <asp:TextBox Width="90%" Height="100px" TextMode="MultiLine" ID="txtIndirizzo" runat="server"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" ErrorMessage="Indirizzo residenza privato Obbligatorio"
                                ControlToValidate="txtIndirizzo" runat="server" Text="*" ValidationGroup="Insertvalidate" />--%>
                        </div>
                        <div class="col-sm-6"></div>
                    </div>
                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-6">
                            <strong>Indirizzo per fatturazione*</strong><br />
                            <asp:TextBox Width="90%" Height="100px" TextMode="MultiLine" ID="txtindirizzofatt_dts" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ErrorMessage="indicazione indirizzo fatturazione Obbligatorio"
                                ControlToValidate="txtindirizzofatt_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                        </div>
                        <div class="col-sm-6" style="display: none">
                            <strong>
                                <asp:Label ID="Label11" runat="server" Text="Selezione ricevuta o fattura*" /></strong><br />
                            <asp:RadioButtonList ID="radricfatt_dts" runat="server">
                                <asp:ListItem Text="Ricevuta" Value="Ricevuta" Selected="True" />
                                <asp:ListItem Text="Fattura" Value="Fattura" />
                            </asp:RadioButtonList>
                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ErrorMessage="indicazione ricevuta o fattura Obbligatoria"
                                ControlToValidate="radricfatt_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />--%>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px">
                        <div class="col-sm-12" style="border: 1px solid White; margin: 1%">
                            <strong>Carica foto ritratto</strong>
                            <asp:FileUpload runat="server" ID="UploadRitratto" />
                            <asp:Panel runat="server" ID="pnlRitratto">
                                <asp:Literal ID="litRitratto" Text="" runat="server" />
                                <asp:LinkButton ToolTip="Elimina File" Text="" ID="lnkRitratto" runat="server" OnClick="lnkRitratto_Click" />
                                <br />
                            </asp:Panel>
                            <i>Il tuo volto virtuale o ritratto.<br />
                                Tipi di file permessi: jpg pgn bmp ( dimensione massima 10 Mbyte )</i>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlIndirizzo1">
                                <div class="row" style="border: 1px solid White; margin: 1%; padding: 20px">
                                    <h3>Indirizzo 1</h3>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Nome della posizione</strong>
                                            <i>per es. un luogo di lavoro, una sede, un punto di ritrovo</i><br />
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox Width="60%" runat="server" ID="txtNomeposizione1_dts" /><br />
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Paese*</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceNAZIONE1_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceNAZIONE1_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ErrorMessage="Nazione Indirizzo 1 Obbligatorio"
                                                ControlToValidate="ddlCodiceNAZIONE1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Regione*</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceREGIONE1_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceREGIONE1_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ErrorMessage="Regione Indirizzo 1 Obbligatoria"
                                                ControlToValidate="txtCodiceREGIONE1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                            <br />
                                            <input id="txtCodiceREGIONE1_dts" style="width: 60%" runat="server" value="" />

                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Provincia*</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodicePROVINCIA1_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodicePROVINCIA1_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ErrorMessage="Provincia Indirizzo 1 Obbligatoria"
                                                ControlToValidate="txtCodicePROVINCIA1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                            <br />
                                            <input id="txtCodicePROVINCIA1_dts" style="width: 60%" runat="server" value="" />

                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Comune*</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceCOMUNE1_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceCOMUNE1_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" ErrorMessage="Comune Indirizzo 1 Obbligatoria"
                                                ControlToValidate="txtCodiceCOMUNE1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                            <br />
                                            <input id="txtCodiceCOMUNE1_dts" style="width: 60%" runat="server" value="" />

                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Via*</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtVia1_dts" style="width: 60%" runat="server" value="" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" ErrorMessage="Via Indirizzo 1 Obbligatoria"
                                                ControlToValidate="txtVia1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Cap*</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtCap1_dts" style="width: 60%" runat="server" value="" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" ErrorMessage="Cap Indirizzo 1 Obbligatorio"
                                                ControlToValidate="txtCap1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Telefono</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtTelefono1_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Latitudine</strong><br />
                                            <asp:TextBox ID="txtLatitudine1_dts" runat="server" value="" />
                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtLatitudine1_dts"
                                                FilterType="Custom, Numbers" ValidChars="0123456789," />
                                        </div>
                                        <div class="col-sm-8">
                                            <strong>Longitudine</strong><br />
                                            <asp:TextBox ID="txtLongitudine1_dts" runat="server" value="" />
                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtLongitudine1_dts"
                                                FilterType="Custom, Numbers" ValidChars="0123456789," />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel runat="server" ID="pnlIndirizzo2">
                                <div class="row" style="border: 1px solid White; margin: 1%; padding: 20px">
                                    <h3>Indirizzo 2</h3>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Nome della posizione</strong>
                                            <i>per es. un luogo di lavoro, una sede, un punto di ritrovo</i><br />
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox Width="60%" runat="server" ID="txtNomeposizione2_dts" /><br />
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Paese</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceNAZIONE2_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceNAZIONE2_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Regione</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceREGIONE2_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceREGIONE2_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList><br />
                                            <input id="txtCodiceREGIONE2_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Provincia</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodicePROVINCIA2_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodicePROVINCIA2_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList><br />
                                            <input id="txtCodicePROVINCIA2_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Comune</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceCOMUNE2_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceCOMUNE2_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList><br />
                                            <input id="txtCodiceCOMUNE2_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Via</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtVia2_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Cap</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtCap2_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Telefono</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtTelefono2_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Latitudine</strong><br />
                                            <asp:TextBox ID="txtLatitudine2_dts" runat="server" value="" />
                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtLatitudine2_dts"
                                                FilterType="Custom, Numbers" ValidChars="0123456789," />
                                        </div>
                                        <div class="col-sm-8">

                                            <strong>Longitudine</strong><br />
                                            <asp:TextBox ID="txtLongitudine2_dts" runat="server" value="" />
                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtLongitudine2_dts"
                                                FilterType="Custom, Numbers" ValidChars="0123456789," />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="Panel2">
                                <div class="row" style="border: 1px solid White; margin: 1%; padding: 20px">
                                    <h3>Indirizzo 3</h3>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Nome della posizione</strong>
                                            <i>per es. un luogo di lavoro, una sede, un punto di ritrovo</i><br />
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox Width="60%" runat="server" ID="txtNomeposizione3_dts" /><br />
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Paese</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceNAZIONE3_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceNAZIONE3_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Regione</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceREGIONE3_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceREGIONE3_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList><br />
                                            <input id="txtCodiceREGIONE3_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Provincia</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodicePROVINCIA3_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodicePROVINCIA3_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList><br />
                                            <input id="txtCodicePROVINCIA3_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Comune</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceCOMUNE3_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceCOMUNE3_dts_OnSelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList><br />
                                            <input id="txtCodiceCOMUNE3_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Via</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtVia3_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Cap</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtCap3_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Telefono</strong>
                                        </div>
                                        <div class="col-sm-8">
                                            <input id="txtTelefono3_dts" style="width: 60%" runat="server" value="" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 5px">
                                        <div class="col-sm-4">
                                            <strong>Latitudine</strong><br />
                                            <asp:TextBox ID="txtLatitudine3_dts" runat="server" value="" />
                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtLatitudine3_dts"
                                                FilterType="Custom, Numbers" ValidChars="0123456789," />
                                        </div>
                                        <div class="col-sm-8">

                                            <strong>Longitudine</strong><br />
                                            <asp:TextBox ID="txtLongitudine3_dts" runat="server" value="" />
                                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtLongitudine3_dts"
                                                FilterType="Custom, Numbers" ValidChars="0123456789," />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <hr />
                    <asp:Panel runat="server" ID="pnlAmministrazioneSocioRiservata">
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-12">
                                <h3>Sezione amministrazione Utente</h3>
                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-6">
                                <strong>Intestazione scheda pubblica socio* (Italiano)</strong>
                                <asp:TextBox Width="90%" TextMode="MultiLine" Height="50" ID="txtDenominazioneI" runat="server"></asp:TextBox>

                            </div>
                            <div class="col-sm-6">
                                <strong>Intestazione scheda pubblica socio (Inglese)</strong>
                                <asp:TextBox Width="90%" TextMode="MultiLine" Height="50" ID="txtDenominazioneGB" runat="server"></asp:TextBox>

                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-6">
                                <strong>Descrizione socio per scheda pubblica (Italiano)</strong>
                                <asp:TextBox Width="90%" TextMode="MultiLine" Height="150" ID="txtDescrizioneI" runat="server"></asp:TextBox>

                            </div>
                            <div class="col-sm-6">
                                <strong>Descrizione socio per scheda pubblica (Inglese)</strong>
                                <asp:TextBox Width="90%" TextMode="MultiLine" Height="150" ID="txtDescrizioneGB" runat="server"></asp:TextBox>

                            </div>
                        </div>
                        <div class="row" style="border: 1px solid White; margin: 1%; padding: 20px">
                            <div class="row" style="margin-bottom: 5px">
                                <div class="col-sm-12">
                                    <h4><strong>Situazione pagamenti</strong></h4>
                                    <br />
                                    <asp:CheckBoxList runat="server" ID="cbpagamenti" CssClass="cboxspaced" AppendDataBoundItems="true" RepeatDirection="Horizontal" RepeatLayout="Table">
                                    </asp:CheckBoxList>

                                </div>
                            </div>
                            <div class="row" style="margin-bottom: 5px">
                                <div class="col-sm-12">
                                    <strong>Note riservate</strong><br />
                                    <asp:TextBox Width="90%" Height="100px" TextMode="MultiLine" ID="txtnoteriservate_dts" runat="server"></asp:TextBox>
                                </div>

                            </div>
                            <hr />
                            <div class="row" style="margin-bottom: 5px">
                                <div class="col-sm-6">
                                    <strong>
                                        <asp:Label Width="80%" ID="Label3" runat="server" Text="Blocca accesso utente a sito" /></strong>
                                    <asp:CheckBox Width="20%" ID="chkBloccoaccesso_dts" runat="server" Checked="false"></asp:CheckBox>
                                </div>
                                <div class="col-sm-6">
                                </div>
                            </div>
                            <div class="row" style="margin-bottom: 5px">
                                <div class="col-sm-6">
                                    <strong>
                                        <asp:Label Width="80%" ID="Label15" runat="server" Text="Abilita contatto" /></strong>
                                    <asp:CheckBox Width="20%" ID="chkContatto" runat="server" Checked="true"></asp:CheckBox>
                                </div>
                            </div>
                            <div class="row" style="margin-bottom: 5px">
                                <div class="col-sm-6">
                                    <strong>
                                        <asp:Label Width="80%" ID="Label26" runat="server" Text="Nascondi sul sito web" /></strong>
                                    <br />
                                    <asp:CheckBox ID="chkArchiviato" runat="server" />
                                </div>
                                <div class="col-sm-6">
                                    <strong>Data Inserimento Socio</strong>
                                    <asp:TextBox Width="60%" ID="txtData" runat="server"></asp:TextBox>
                                    <Ajax:CalendarExtender ID="cal2" runat="server" Format="dd/MM/yyyy HH.mm.ss" TargetControlID="txtData">
                                    </Ajax:CalendarExtender>
                                </div>
                            </div>
                            <div class="row">
                                <p>
                                    ----------------------------------------------------------------------<br />
                                    <strong>GESTIONE UTENTE E PASSWORD:</strong>
                                </p>

                                CREAZIONE UTENTE:<br />
                                <asp:Button Text="Crea Utente per accesso" runat="server" ID="btnCreaUtente" OnClick="btnCreaUtente_Click" /><br />
                                <br />
                                ELIMINA UTENTE:<br />
                                <asp:Button Text="Elimina Utente per accesso" runat="server" ID="btnEliminaUtente" OnClick="btnEliminaUtente_Click" /><br />
                                <br />


                                MODIFICA PASSWORD:<br />
                                Vecchia Password:<asp:TextBox ID="txtPasswordold" runat="server"></asp:TextBox>
                                Nuova Password:<asp:TextBox ID="txtPasswordnew" runat="server"></asp:TextBox>
                                <asp:Button ID="Button5" runat="server" Text="Cambio Password" OnClick="Cambiopass_Click" />
                                <br />
                                <asp:Label ID="lblquestion" runat="server" />
                                <br />
                                <asp:TextBox ID="txtanswer" runat="server"></asp:TextBox>
                                <asp:Button ID="Button6" runat="server" Text="Reset Password" Enabled="true" OnClick="Resetpass_Click" />
                                <br />
                                <h2>
                                    <asp:Label ID="lblResultsPsw" runat="server" /></h2>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="pnlGestionefoto">
                            <h2>Files allegati a scheda</h2>
                            <div class="row" style="border-top: 1px solid Black">
                                <div class="col-sm-5">
                                    <h3>
                                        <asp:Literal ID="litFoto" runat="server" Text=""></asp:Literal></h3>
                                    <div>
                                        <hr />
                                        <asp:FileUpload ID="UploadFoto" runat="server" />
                                        <br />
                                        Descrizione
                                <br />
                                        <asp:TextBox runat="server" ID="txtDescrizione" />
                                        <asp:HiddenField ID="txtFotoSchema" runat="server" />
                                        <asp:HiddenField ID="txtFotoValori" runat="server" />
                                        <br />
                                        <asp:Button ID="btnCarica" runat="server" Text="Carica Foto" OnClick="btnCarica_Click" />
                                        <asp:Button ID="btnModifica" runat="server" Text="Modifica Descrizione Foto" OnClick="btnModifica_Click" />
                                        <asp:Button ID="btnElimina" runat="server" Text="Elimina Foto" OnClick="btnElimina_Click" />
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <div class="col-sm-7"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-5">
                                    <a id="linkFoto" runat="server" target="_blank">
                                        <asp:Image ID="imgFoto" Width="370px" runat="server" CssClass="img-responsive" ImageUrl="" /></a>
                                </div>
                                <div class="col-sm-7">

                                    <ul style="list-style: none; display: inline">
                                        <asp:Repeater runat="server" ID="rptImmagini">
                                            <ItemTemplate>
                                                <li style="list-style: none; display: inline">
                                                    <asp:ImageButton Width="80px" Height="80px" ToolTip='<%# Eval("DescrizioneI").ToString() %>' ID="imgAntFoto" CommandArgument='<%# Eval("NomeFile").ToString() %>'
                                                        OnClick="linkgalleria_click" runat="server" ImageUrl='<%# ComponiUrlGalleriaAnteprima(Eval("NomeAnteprima").ToString()) %>' />
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                            <br />
                        </asp:Panel>
                    </asp:Panel>
                </div>

                <hr />


                <asp:Panel runat="server" ID="pnlCampinonutilizzati" Visible="false">
                    <table width="100%" style="margin-top: 10px; font-size: 12px; table-layout: fixed; border-collapse: collapse; background-color: #ccc"
                        cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <span style="display: inline;"><strong>
                                    <asp:Label Width="30%" ID="Label20" runat="server" Text="Videolink" /></strong>
                                    <asp:TextBox Width="60%" ID="txtVideo" runat="server"></asp:TextBox>
                                </span>

                                <span style="display: inline;"><strong>
                                    <asp:Label Width="15%" ID="Label18" runat="server" Text="Vetrina" /></strong>
                                    <asp:CheckBox ID="chkVetrina" runat="server" />
                                </span>
                                <br />
                                <span style="display: inline;"><strong>
                                    <asp:Label Width="15%" ID="Label16" runat="server" Text="Categoria 1 Livello" /></strong>
                                    <asp:DropDownList Width="60%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlProdotto"
                                        OnSelectedIndexChanged="ddlProdotto_SelectedIndexChanged" runat="server" />
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Categoria Prodotto Obbligatoria"
                            ControlToValidate="ddlProdotto" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                    --%>
                                </span>
                                <br />
                                <span style="display: inline;"><strong>
                                    <asp:Label Width="15%" ID="Label17" runat="server" Text="Categoria 2 Livello" /></strong>
                                    <asp:DropDownList Width="60%" AppendDataBoundItems="true" ID="ddlSottoProdotto" runat="server" />
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="SottoCategoria Prodotto Obbligatoria"
                                ControlToValidate="ddlSottoProdotto" runat="server" Text="*" ValidationGroup="Insertvalidate" />--%>
                                </span>
                                <br />

                                <span style="display: inline;"><strong>
                                    <asp:Label Width="15%" ID="Label12" runat="server" Text="CODICE PRODOTTO" /></strong>
                                    <asp:TextBox Width="60%" ID="txtCodiceProd" runat="server"></asp:TextBox>
                                    <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="CodiceProdotto Obbligatorio"
                                ControlToValidate="txtCodiceProd" runat="server" Text="*" ValidationGroup="Insertvalidate" />--%>
                                </span>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%; vertical-align: top;">
                                <span style="display: block;"><strong>
                                    <asp:Label Width="30%" ID="Label13" runat="server" Text="Testo promo Ita" /></strong>
                                    <asp:TextBox Width="60%" ID="txtCampo1I" runat="server"></asp:TextBox>
                                </span>
                                <span style="display: block;"><strong>
                                    <asp:Label Width="30%" ID="Labelps1" runat="server" Text="Testo promo sconto Ita" /></strong>
                                    <asp:TextBox Width="60%" ID="txtCampo2I" runat="server"></asp:TextBox>
                                </span>

                                <br />
                                <div style="display: inline">
                                    <span style="display: inline;"><strong>
                                        <asp:Label Width="30%" ID="Label1" runat="server" Text="Dettagli Ita" /></strong>
                                        <asp:TextBox Width="60%" Height="150px" TextMode="MultiLine" ID="txtDatitecniciI"
                                            runat="server"></asp:TextBox>
                                    </span>
                                </div>
                                <div>
                                    <span style="display: inline;"><strong>
                                        <asp:Label Width="30%" ID="Label9" runat="server" Text="Regione" /></strong>
                                        <asp:DropDownList Width="60%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlRegione"
                                            OnSelectedIndexChanged="ddlRegione_SelectedIndexChanged" runat="server" />
                                    </span><span style="display: inline;"><strong>
                                        <asp:Label Width="30%" ID="Label10" runat="server" Text="Provincia" /></strong>
                                        <asp:DropDownList Width="60%" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"
                                            AutoPostBack="true" ID="ddlProvincia" runat="server" />
                                    </span><span style="display: inline;"><strong>
                                        <asp:Label Width="30%" ID="Label8" runat="server" Text="Comune" /></strong>
                                        <asp:DropDownList Width="60%" AppendDataBoundItems="true" ID="ddlComune" runat="server" />
                                    </span>
                                </div>
                                <asp:Panel runat="server" Visible="true">
                                    <span style="display: inline;"><strong>
                                        <asp:Label Width="30%" ID="Label28" runat="server" Text="Caratteristica 4" /></strong>
                                        <asp:DropDownList Width="60%" AppendDataBoundItems="true" ID="ddlCaratteristica4" runat="server" />
                                    </span>
                                    <span style="display: inline;"><strong>
                                        <asp:Label Width="30%" ID="Label29" runat="server" Text="Caratteristica 5" /></strong>
                                        <asp:DropDownList Width="60%" AppendDataBoundItems="true" ID="ddlCaratteristica5" runat="server" />
                                    </span>

                                    <span style="display: inline;"><strong>
                                        <asp:Label Width="30%" ID="Label30" runat="server" Text="Caratteristica 6" /></strong>
                                        <asp:DropDownList Width="60%" AppendDataBoundItems="true" ID="ddlCaratteristica6" runat="server" />
                                    </span>
                                    <span style="display: none;"><strong>
                                        <asp:Label Width="30%" ID="Label22" runat="server" Text="Anno ( 4 cifre )" /></strong>
                                        <asp:TextBox Width="60%" ID="txtAnno" runat="server"></asp:TextBox>
                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtAnno"
                                            FilterMode="ValidChars" ValidChars="0123456789" />
                                    </span>
                                    <span style="display: block;"><strong>
                                        <asp:Label Width="30%" ID="Labelprezzo" runat="server" Text="Prezzo Offerta &euro;" /></strong>
                                        <asp:TextBox Width="60%" ID="txtPrezzo" runat="server"></asp:TextBox>
                                        <Ajax:FilteredTextBoxExtender ID="ftbe" runat="server" TargetControlID="txtPrezzo"
                                            FilterMode="ValidChars" ValidChars="0123456789," />
                                    </span><span style="display: block;"><strong>
                                        <asp:Label Width="30%" ID="Label19" runat="server" Text="Prezzo Listino &euro;" /></strong>
                                        <asp:TextBox Width="60%" ID="txtPrezzoListino" runat="server"></asp:TextBox>
                                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPrezzoListino"
                                            FilterMode="ValidChars" ValidChars="0123456789," />
                                    </span>
                                </asp:Panel>
                                <span style="display: inline;"><strong>
                                    <asp:Label Width="30%" ID="Label21" runat="server" Text="Id collegato" /></strong>
                                    <asp:TextBox Width="60%" ID="txtIdcollegato" runat="server"></asp:TextBox>
                                </span>
                            </td>
                            <td style="width: 50%; vertical-align: top;">
                                <span style="display: block;"><strong>
                                    <asp:Label Width="30%" ID="Labelpre" runat="server" Text="Testo promo Eng" /></strong>
                                    <asp:TextBox Width="60%" ID="txtCampo1GB" runat="server"></asp:TextBox>
                                </span>
                                <span style="display: block;"><strong>
                                    <asp:Label Width="30%" ID="Labelprse" runat="server" Text="Testo promo sconto Eng" /></strong>
                                    <asp:TextBox Width="60%" ID="txtCampo2GB" runat="server"></asp:TextBox>
                                </span>
                                <br />
                                <span style="display: inline; display: inline"><strong>
                                    <asp:Label Width="30%" ID="Label2" runat="server" Text="Dettaglio Eng" /></strong>
                                    <asp:TextBox Width="60%" Height="150px" TextMode="MultiLine" ID="txtDatitecniciGB"
                                        runat="server"></asp:TextBox>
                                </span>
                                <br />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlGestioneCategorie" Visible="false">
                    <h2>
                        <div style="margin-left: auto">
                            <asp:Label runat="server" ID="TitleGestione" Text="Area Modifica / Inserimento CATEGORIE "></asp:Label>
                        </div>
                    </h2>
                    <hr />
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div style="margin-left: auto; padding: 0px 10px 0px 10px; border: 1px solid #000000;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <div>
                                                    <h2>
                                                        <asp:Label runat="server" ID="TitleProdotti" Text='<%# references.ResMan("Common", Lingua,"TitleProdottiGest") %>'></asp:Label></h2>
                                                </div>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 150px;">
                                                            <asp:Label runat="server" ID="lblTipologia" Text="Tipologia 1 Livello"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList Width="90%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlTipologiaNewProd"
                                                                OnSelectedIndexChanged="TipologiaProd_SelectedIndexChanged" runat="server" Enabled="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblsceltaprod" Text="Scelta  1 Livello"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList Width="100%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlProdottoNewProd1"
                                                                runat="server" OnSelectedIndexChanged="ddlProdottoNewProd1_SelectedIndexChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="height: 35px;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="NomeIta" Text="Nome 1 Livello Ita"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="NomeNuovoProdIt" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="NomeEng" Text="Nome 1 Livello Eng"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="NomeNuovoProdEng" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Button ID="OkButton" runat="server" Text="Nuovo" OnClick="btnInsertNewProd_Click" />
                                                <asp:Button ID="btnModificaProd" runat="server" Text="Modifica" OnClick="btnModifiProd_Click" />
                                                <asp:Button ID="btnEliminaProdotto" runat="server" Text="Elimina" OnClick="btnEliminaProd_Click" /><br />
                                                <br />
                                                <asp:Label runat="server" ID="ErrorMsgNuovoProdotto" ForeColor="Red"></asp:Label>
                                                <div style="height: 50px;">
                                                </div>
                                            </td>
                                            <td>
                                                <%-- Qui aggiungiamo le dll di selezione per la modifica del SottoProdotto--%>
                                                <div style="margin-left: 50px; border-left: 1px solid #000000; padding-left: 10px;">
                                                    <div>
                                                        <h2>
                                                            <asp:Label runat="server" ID="TitleSottoprodotti" Text='<%# references.ResMan("Common", Lingua,"TitleSottProdottiGest") %>'></asp:Label></h2>
                                                    </div>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 150px;">
                                                                <asp:Label runat="server" ID="lblTipologiaSott" Text="Tipologia 1 Livello"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList Width="100%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlTipologiaNewSottProd"
                                                                    runat="server" Enabled="false" />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblProdotto" Text="Scelta 1 Livello"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList Width="100%" AppendDataBoundItems="true" ID="ddlProdottoNewProd"
                                                                    AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProdottoNewProd_SelectedIndexChange" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="Label14" Text="Scelta 2 Livello"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList Width="100%" AppendDataBoundItems="true" ID="ddlProdottoNewSProd"
                                                                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProdottoNewSProd_SelectedIndexChange" />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="NomeSottIta" Text="Nome 2 Livello Ita"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="NomeNuovoSottIt" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="NomeSottEng" Text="Nome 2 Livello Eng"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="NomeNuovoSottEng" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Button ID="OkButton2" runat="server" Text="Nuovo" OnClick="btnInsertNewSottProd_Click" />
                                                    <asp:Button ID="btnModificaSottoProd" runat="server" Text="Modifica" OnClick="btnModificaSottProd_Click" />
                                                    <asp:Button ID="btnEliminaSottoProd" runat="server" Text="Elimina" OnClick="btnEliminaSottProd_Click" /><br />
                                                    <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>
                                                </div>
                                                <div style="height: 50px;">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlGestioneTblCaratteristiche">

                    <h2 style="border-bottom: 1px solid Black">
                        <div style="margin-left: auto">
                            <asp:Label runat="server" ID="Label25" Text="Gestione tabelle riferimento"></asp:Label>
                        </div>
                    </h2>
                    <hr />

                    <div style="background-color: #fafafa">
                        <h3>Tabella Categoria Socio Gist</h3>
                        <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica3_gest" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica3update">
                        </asp:DropDownList><br />
                        Descrizione italiano:
                        <asp:TextBox runat="server" ID="txtCar3I" Text="" Width="300" /><br />
                        Descrizione inglese:
                        <asp:TextBox runat="server" ID="txtCar3GB" Text="" Width="300" /><br />
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender6" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar3I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender7" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar3GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <asp:Button Text="Aggiorna/Inserisci" ID="Button1" runat="server" OnClick="btnAggiornaCaratteristica3_Click" />
                        <br />
                    </div>

                    <asp:Panel runat="server" ID="pnlGestioneCaratteristicheExtra" Visible="false">

                        <div style="background-color: #fafafa">
                            <h3>Tabella Categoria Professionale</h3>
                            <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica1_gest" AutoPostBack="true"
                                AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica1update">
                            </asp:DropDownList><br />
                            Descrizione italiano:
                        <asp:TextBox runat="server" ID="txtCar1I" Text="" Width="300" /><br />
                            Descrizione inglese:
                        <asp:TextBox runat="server" ID="txtCar1GB" Text="" Width="300" /><br />
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="w1" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar1I">
                            </Ajax:TextBoxWatermarkExtender>
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="w2" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar1GB">
                            </Ajax:TextBoxWatermarkExtender>
                            <asp:Button Text="Aggiorna/Inserisci" ID="btnAggiornaCaratteristica1" runat="server" OnClick="btnAggiornaCaratteristica1_Click" />
                            <br />
                        </div>
                        <div style="background-color: #fafafa">
                            <h3>Tabella Categoria 2</h3>
                            <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica2_gest" AutoPostBack="true"
                                AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica2update">
                            </asp:DropDownList><br />
                            Descrizione italiano:
                        <asp:TextBox runat="server" ID="txtCar2I" Text="" Width="300" /><br />
                            Descrizione inglese:
                        <asp:TextBox runat="server" ID="txtCar2GB" Text="" Width="300" /><br />
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender4" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar2I">
                            </Ajax:TextBoxWatermarkExtender>
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender5" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar2GB">
                            </Ajax:TextBoxWatermarkExtender>
                            <asp:Button Text="Aggiorna/Inserisci" ID="btnAggiornaCaratteristica2" runat="server" OnClick="btnAggiornaCaratteristica2_Click" />
                            <br />
                        </div>


                        <div style="background-color: #fafafa">
                            <h3>Tabella Caratteristica 4</h3>
                            <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica4_gest" AutoPostBack="true"
                                AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica4update">
                            </asp:DropDownList><br />
                            Descrizione italiano:
                        <asp:TextBox runat="server" ID="txtCar4I" Text="" Width="300" /><br />
                            Descrizione inglese:
                        <asp:TextBox runat="server" ID="txtCar4GB" Text="" Width="300" /><br />
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender8" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar4I">
                            </Ajax:TextBoxWatermarkExtender>
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender9" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar4GB">
                            </Ajax:TextBoxWatermarkExtender>
                            <asp:Button Text="Aggiorna/Inserisci" ID="Button2" runat="server" OnClick="btnAggiornaCaratteristica4_Click" />
                            <br />
                        </div>

                        <div style="background-color: #fafafa">
                            <h3>Tabella Caratteristica 5</h3>
                            <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica5_gest" AutoPostBack="true"
                                AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica5update">
                            </asp:DropDownList><br />
                            Descrizione italiano:
                        <asp:TextBox runat="server" ID="txtCar5I" Text="" Width="300" /><br />
                            Descrizione inglese:
                        <asp:TextBox runat="server" ID="txtCar5GB" Text="" Width="300" /><br />
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender10" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar5I">
                            </Ajax:TextBoxWatermarkExtender>
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender11" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar5GB">
                            </Ajax:TextBoxWatermarkExtender>
                            <asp:Button Text="Aggiorna/Inserisci" ID="Button3" runat="server" OnClick="btnAggiornaCaratteristica5_Click" />
                            <br />
                        </div>

                        <div style="background-color: #fafafa">
                            <h3>Tabella Caratteristica 6</h3>
                            <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica6_gest" AutoPostBack="true"
                                AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica6update">
                            </asp:DropDownList><br />
                            Descrizione italiano:
                        <asp:TextBox runat="server" ID="txtCar6I" Text="" Width="300" /><br />
                            Descrizione inglese:
                        <asp:TextBox runat="server" ID="txtCar6GB" Text="" Width="300" /><br />
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender12" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar6I">
                            </Ajax:TextBoxWatermarkExtender>
                            <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender13" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar6GB">
                            </Ajax:TextBoxWatermarkExtender>
                            <asp:Button Text="Aggiorna/Inserisci" ID="Button4" runat="server" OnClick="btnAggiornaCaratteristica6_Click" />
                            <br />
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
