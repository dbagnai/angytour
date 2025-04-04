﻿<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="Content_Tipo3.aspx.cs" Inherits="AspNetPages_Content_Tipo3" Title=""
    MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderSubsSlider" runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function OpenWindow(Url, NomeFinestra, features) {
            window.open(Url, NomeFinestra, features);
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <%--<script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/js/googleMaps.js")%>"></script>--%>
    <script type="text/javascript">
        makeRevLower = true;
    </script>
    <div class="container" style="text-align: center">
        <div class="row">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-10 col-sm-10 col-xs-12">
                <h2 class="h1-body-title" style="color: #5c5c5c">
                    <asp:Literal Text="" runat="server" ID="litNomePagina" /></h2>
            </div>
            <div class="col-md-1 col-sm-1">
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div style="text-align: center">
                    <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- <Ajax:ToolkitScriptManager ID="ScriptManagerMaster" runat="server" AllowCustomErrorsRedirect="True"
        AsyncPostBackErrorMessage="Errore generico. Contattare HelpDesk" AsyncPostBackTimeout="400"
        EnablePartialRendering="true"
        EnablePageMethods="true" EnableScriptLocalization="true" EnableScriptGlobalization="true">
        <Scripts>
        </Scripts>
    </Ajax:ToolkitScriptManager>--%>
    <div class="row" style="text-align: center">
        <div class="col">
            <p>
                <asp:Label runat="server" ID="output" Font-Size="Medium"></asp:Label>
            </p>
            <asp:Label runat="server" ID="lblContenutiContatti"></asp:Label>

            <section class="mbr-section mbr-parallax-background p-0" style="padding-top: 80px; padding-bottom: 80px; background-image: url('/public/Files/con001000/1/contact-back.jpg')">
                <div class="mbr-overlay mbr-overlay-white"></div>
                <%--<div class="w-100 bg-light-color">--%>
                <div class="container">
                    <div class="row justify-content-center">

                        <%--    <div class="col-sm-6 p-0 pr-5 d-none d-lg-block text-center bg-figura-contatti" style="background-repeat: no-repeat !important; background-position: left bottom !important; background-size: auto 91% !important;">
                        </div>--%>

                        <asp:PlaceHolder runat="server" ID="plhForm" Visible="true">
                            <div class="col-12 col-lg-8 py-1 px-3 px-sm-5 pt-0 text-center bg-manual-secondary-color-fade">
                                <div class="ui-15">
                                    <div class="ui-content">
                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12 ui-padd">
                                                    <div class="ui-form py-5">

                                                        <div class="row pb-3" runat="server" visible="true" id="divTitlemail">
                                                            <div class="col-sm-12">
                                                                <div class="big2 text-white">
                                                                    <%= references.ResMan("Common", Lingua,"TestoContattaciFooter") %>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div id="divOrario" runat="server" visible="false">
                                                            <div class="row">
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input mt-0">
                                                                        <script>
                                                                            jQuery(function ($) {
                                                                                $("#ctl00_ContentPlaceHolder1_txtPersone").mask("99", { placeholder: " ", autoclear: false });
                                                                            });
                                                                        </script>
                                                                        <asp:TextBox ID="txtPersone" Width="99%" runat="server"
                                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPersone") %>' />
                                                                        <label class="ui-icon"><i class="fa fa-group"></i></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input">
                                                                        <asp:TextBox autocomplete="none" CssClass="form-control" ID="txtData" Width="99%" runat="server"
                                                                            placeholder='<%# references.ResMan("Common", Lingua,"FormData") %>' />
                                                                        <label class="ui-icon"><i class="fa fa-calendar"></i></label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input">
                                                                        <asp:TextBox ID="txtOrario" Width="99%" runat="server"
                                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormOrario") %>' />
                                                                        <label class="ui-icon"><i class="fa fa-clock-o"></i></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <asp:Panel runat="server" ID="pnlPrenotazione" Visible="false">

                                                            <%--<div class="row" id="divSelezione1" runat="server" visible="false">
                                                    <div class="col-sm-6">
                                                        <div class="ui-input" runat="server" id="divListaservizi">
                                                            <asp:Literal ID="Literal9" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormListaServizi %>' />
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="ddlListaservizi"
                                                                ErrorMessage='<%# references.ResMan("Common", Lingua,"FormListaServiziErr %>' Text="*" ValidationGroup="MailInfo" />
                                                            <br />
                                                            <asp:DropDownList class="form-control" runat="server" Width="99%" ID="ddlListaservizi"
                                                                AppendDataBoundItems="true" Style="margin: 0 auto; height: 30px; margin-bottom: 5px; border: 1px Solid #ccc;">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6" style="display: none">
                                                        <div class="ui-input">
                                                            <asp:Literal ID="Literal12" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormListaSedi %>' />
                                                            <br />
                                                            <asp:DropDownList class="form-control" runat="server" Width="99%" ID="ddlSedi"
                                                                AppendDataBoundItems="true" Style="margin: 0 auto; height: 30px; margin-bottom: 5px; border: 1px Solid #ccc;">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                            --%>
                                                            <%-- <div class="row" runat="server" visible="false">
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <asp:TextBox ID="txtMarca" Width="99%" runat="server"
                                                                class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormMarca %>' />
                                                            <label class="ui-icon"><i class="fa fa-car"></i></label>
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtMarca"
                                                                ErrorMessage='<%# references.ResMan("Common", Lingua,"FormMarcaErr %>' Text="*" ValidationGroup="MailInfo" />
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <asp:TextBox ID="txtModello" Width="99%" runat="server" class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormModello %>' />
                                                            <label class="ui-icon"><i class="fa fa-car"></i></label>
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtModello"
                                                                ErrorMessage='<%# references.ResMan("Common", Lingua,"FormModelloErr %>' Text="*" ValidationGroup="MailInfo" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" runat="server" visible="false">
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <asp:TextBox ID="txttarga" Width="99%" runat="server"
                                                                class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTarga %>' />
                                                            <label class="ui-icon"><i class="fa fa-car"></i></label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <asp:TextBox ID="txtKm" Width="99%" runat="server"
                                                                class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormKm %>' />
                                                            <label class="ui-icon"><i class="fa fa-tachometer"></i></label>
                                                        </div>
                                                    </div>
                                                </div>--%>

                                                            <%--COLLOCAZIONE--%>
                                                            <div class="row" runat="server" visible="true" id="divLocations">
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input mt-0">
                                                                        <%-- <asp:Literal ID="Literal1" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoLocations %>' />
                                                            <br />--%>
                                                                        <asp:DropDownList runat="server" Width="100%" ID="ddlLocations" ClientIDMode="Static" class="form-control">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                                <div class="col-sm-3 pr-1">
                                                                    <div class="ui-input mt-0">
                                                                        <%-- <Ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender1" AcceptNegative="None"
                                                                MaskType="Number" TargetControlID="txtAdulti" InputDirection="RightToLeft" Mask="99"
                                                                AutoComplete="false">
                                                            </Ajax:MaskedEditExtender>--%>
                                                                        <script>
                                                                            jQuery(function ($) {
                                                                                $("#ctl00_ContentPlaceHolder1_txtAdulti").mask("99", { placeholder: " ", autoclear: false });
                                                                            });
                                                                        </script>
                                                                        <asp:TextBox ID="txtAdulti" Width="99%" runat="server"
                                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoAdulti") %>' />
                                                                        <%--<label class="ui-icon"><i class="fa fa-group"></i></label>--%>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-3 pl-1">
                                                                    <div class="ui-input mt-0">

                                                                        <script>
                                                                            jQuery(function ($) {
                                                                                $("#ctl00_ContentPlaceHolder1_txtBambini").mask("99", { placeholder: " ", autoclear: false });
                                                                            });
                                                                        </script>
                                                                        <asp:TextBox ID="txtBambini" Width="99%" runat="server"
                                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoBambini") %>' />
                                                                        <%--<label class="ui-icon fa fa-user"></label>--%>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <%--DATA ARRIVO E PARTENZA--%>
                                                            <div class="row">
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input mt-2 mb-4">
                                                                        <asp:TextBox ID="txtArrivo" Width="99%" runat="server"
                                                                            class="form-control datepicker" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPeriododa") %>' />
                                                                        <label class="ui-icon"><i class="fa fa-calendar"></i></label>
                                                                        <%-- <Ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                                TargetControlID="txtArrivo">
                                                            </Ajax:CalendarExtender>--%>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input mt-2 mb-4">
                                                                        <asp:TextBox ID="txtPartenza" Width="99%" runat="server" class="form-control datepicker" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPeriodoa") %>' />
                                                                        <label class="ui-icon"><i class="fa fa-calendar"></i></label>
                                                                        <%--  <Ajax:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                                TargetControlID="txtPartenza">
                                                            </Ajax:CalendarExtender>--%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                        <%--NOME E COGNOME--%>
                                                        <div class="row">
                                                            <div class="col-sm-6">
                                                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtNome"
                                                                    ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' Style="text-align: left; width: 100%;" Text="* Obbligatorio" ValidationGroup="MailInfo" />
                                                                <div class="ui-input my-0">
                                                                    <asp:TextBox ID="txtNome" Width="99%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtCognome"
                                                                    ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto3Err") %>' Style="text-align: left; width: 100%;" Text="* Obbligatorio" ValidationGroup="MailInfo" />

                                                                <div class="ui-input my-0">
                                                                    <asp:TextBox ID="txtCognome" Width="99%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto3") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <br />
                                                                <%--<asp:RequiredFieldValidator runat="server" ID="reqValidatorNome" ControlToValidate="txtSoc"
                                                                        ErrorMessage='<%# references.ResMan("Common", Lingua,"formtesto16sErr") %>' Text="*" ValidationGroup="MailInfo" />--%>
                                                                <div class="ui-input my-0">
                                                                    <asp:TextBox ID="txtSoc" Width="99%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"formtesto16s") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-sm-6">
                                                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtEmail"
                                                                    ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' Text="* Obbligatorio" ValidationGroup="MailInfo" />

                                                                <div class="ui-input my-0">
                                                                    <asp:TextBox ID="txtEmail" Width="99%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtTel"
                                                                    ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto11Err") %>' Text="* Obbligatorio" ValidationGroup="MailInfo" />

                                                                <div class="ui-input mt-0">
                                                                    <asp:TextBox ID="txtTel" Width="99%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row" runat="server" id="divRegione" visible="false">
                                                            <div class="col-sm-12">
                                                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" runat="server" ID="reqValidatorRegione" ControlToValidate="ddlRegione" Enabled="false" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto6Err") %>' Text="*" ValidationGroup="MailInfo" />

                                                                <div class="ui-input my-0">
                                                                    <asp:Literal ID="Literal8" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto6") %>' />*<br />
                                                                    <asp:DropDownList class="form-control" runat="server" ID="ddlRegione" AppendDataBoundItems="true"
                                                                        OnInit="ddlRegione_OnInit" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto6") %>' />

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtDescrizione"
                                                                    ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto17Err") %>' Text="*" ValidationGroup="MailInfo" />

                                                                <div class="ui-input my-0">
                                                                    <asp:TextBox ID="txtDescrizione" Width="99%"
                                                                        TextMode="MultiLine" Font-Size="17px" Font-Names="Raleway"
                                                                        Height="150px" runat="server" class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto17") %>' />

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-sm-12" style="text-align: center">

                                                                <%--MESSAGGIO ERRORE CAMPI MANCANTI--%>
                                                                <%--<asp:ValidationSummary runat="server" ID="Summary" ValidationGroup="MailInfo" DisplayMode="BulletList"
                                                                    ShowSummary="true" HeaderText='<%# references.ResMan("Common", Lingua,"testoDatiMancanti")  %>' />--%>

                                                                <%--MESSAGGIO ERRORE RECAPTCHA--%>
                                                                <%--<div id="recaptcharesponse"></div>--%>


                                                               <%-- <asp:Button ID="btnInviaSrv" Style="display: none" runat="server" OnClick="btnInvia_Click" />--%>
                                                                <style>
                                                                    .g-recaptcha {
                                                                        margin: 15px auto !important;
                                                                        width: auto !important;
                                                                        height: auto !important;
                                                                        text-align: -webkit-center;
                                                                        text-align: -moz-center;
                                                                        text-align: -o-center;
                                                                        text-align: -ms-center;
                                                                    }

                                                                    #recaptcharesponse {
                                                                        font-size: 1.5rem;
                                                                        color: red;
                                                                    }
                                                                </style>
                                                                <script>
                                                                    function ConfirmContactValue(elembtn) {
                                                                        if ($('<%= "#" + chkPrivacy.ClientID %>')[0].checked == false) {
                                                                            $("#recaptcharesponse").html('<%= references.ResMan("Common", Lingua,"txtPrivacyError") %>');
                                                                            $('#recaptcharesponse').css({ 'display': 'block' });
                                                                            return false;
                                                                        }
                                                                        else {
                                                                            $("#recaptcharesponse").html('');
                                                                            $('#recaptcharesponse').css({ 'display': 'none' });
                                                                        }

                                                                        var contactdatas = {};
                                                                        /*ABILITARE PER CONTROLLO CAPTCHA*/
                                                                        if (typeof (grecaptcha) != 'undefined') {
                                                                            var response = grecaptcha.getResponse();
                                                                            if (response.length == 0)  //reCaptcha not verified
                                                                            {
                                                                                $("#recaptcharesponse").html('<%= references.ResMan("Common", Lingua,"testoCaptcha").Replace("'","`") %>');
                                                                                $('#recaptcharesponse').css({ 'display': 'block' });
                                                                                return false;
                                                                            } else contactdatas.recaptcharesponse = response;
                                                                        }

                                                                        if (Page_ClientValidate("MailInfo")) {
                                                                            /*do work and go for postback*/
                                                                            $(elembtn).attr("disabled", "")

                                                                            //invio nopostback con handler////////////////////////////////////////////////////
                                                                            contactdatas.chkprivacy = $('<%= "#" + chkPrivacy.ClientID %>')[0].checked;
                                                                            contactdatas.chknewsletter = $('<%= "#" + chkNewsletter.ClientID %>')[0].checked;
                                                                            getcontactdata1(contactdatas, function (contactdatas) {
                                                                                var tastotxt = $(elembtn).html();
                                                                                $(elembtn).html("Wait ..");
                                                                                inviamessaggiomail(lng, contactdatas, function (result) {
                                                                                    if (result) {
                                                                                        //in caso di errore visualizzo
                                                                                        $("#recaptcharesponse").html(result);
                                                                                        $('#recaptcharesponse').css({ 'display': 'block' });
                                                                                        $(elembtn).removeAttr("disabled")
                                                                                        $(elembtn).html(tastotxt);
                                                                                    }
                                                                                }, tastotxt);
                                                                            }, $(elembtn));
                                                                            ///////////////////////////////////////////////////////////////////////

                                                                            ////////////////////////////////////////////////////////////////////////
                                                                            ////////////////////////////////////////////////////////////////////////

                                                                        }
                                                                        else {
                                                                            $('html,body').animate({
                                                                                scrollTop: $("#" + "<%= divTitlemail.ClientID  %>").offset().top - 160
                                                                            }, 5);
                                                                            console.log('not  validated');
                                                                            return false;
                                                                        }
                                                                    }
                                                                    function getcontactdata1(contactdatas, callback) {
                                                                        var contactdatas = contactdatas || {};
                                                                        contactdatas.idofferta = '<%= idOfferta %>';
                                                                        contactdatas.tipocontenuto = '<%= TipoContenuto %>';
                                                                        contactdatas.location = $("#ddlLocations option:selected").text();
                                                                        contactdatas.regione = $("#ddlRegione option:selected").val();
                                                                        contactdatas.adulti = $("[id$='txtAdulti']").val();
                                                                        contactdatas.bambini = $("[id$='txtBambini']").val();
                                                                        contactdatas.arrivo = $("[id$='txtArrivo']").val();
                                                                        contactdatas.partenza = $("[id$='txtPartenza']").val();
                                                                        contactdatas.persone = $("[id$='txtPersone']").val();
                                                                        contactdatas.datarichiesta = $("[id$='txtData']").val();
                                                                        contactdatas.orario = $("[id$='txtOrario']").val();
                                                                        contactdatas.cognome = $("[id$='txtCognome']").val();
                                                                        contactdatas.name = $("[id$='txtNome']").val();
                                                                        contactdatas.ragsoc = $("[id$='txtSoc']").val();
                                                                        contactdatas.email = $("[id$='txtEmail']").val();
                                                                        contactdatas.telefono = $("[id$='txtTel']").val();
                                                                        contactdatas.message = $("[id$='txtDescrizione']").val();
                                                                        //contactdatas.location1 = $("#ddlSedeContact option:selected").text();
                                                                        contactdatas.tipo = "informazioni";
                                                                        callback(contactdatas);
                                                                    }
                                                                </script>

                                                                <br />

                                                                <br />
                                                                <div class="row" style="text-align: left">
                                                                    <div class="col-sm-12 text-white">
                                                                        <%= references.ResMan("basetext", Lingua,"privacytext") %>
                                                                        <div class="checkbox" style="margin-bottom: 10px">
                                                                            <label>
                                                                                <input id="chkPrivacy" runat="server" type="checkbox" />
                                                                                <span class="cr"><i class="cr-icon fa fa-check" style="color: #000"></i></span><%= references.ResMan("basetext", Lingua,"privacyconsenso") %>
                                                                            </label>
                                                                        </div>
                                                                        <div class="checkbox" style="margin-bottom: 0px">
                                                                            <label>
                                                                                <input type="checkbox" id="chkNewsletter" runat="server" />
                                                                                <span class="cr"><i class="cr-icon fa fa-check" style="color: #000"></i></span><%= references.ResMan("basetext", Lingua,"privacyconsenso1") %>
                                                                            </label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-12 p-0 pr-5 text-center">
                                                                    <%--    <div class="g-recaptcha" id="rcaptcha" data-sitekey="6LccbRMUAAAAAAN14HC8RFxwNMaqdGvJFPQEVinq"></div>--%>
                                                                    <%=  WelcomeLibrary.UF.ConfigManagement.ReadKey("recaptchask") %>
                                                                    <button id="btnInvia" type="button" class="btn btn-lg btn-block text-white" style="width: 200px" runat="server" validationgroup="MailInfo" onclick="ConfirmContactValue(this);"><%=  references.ResMan("Common", Lingua,"TestoInvio")  %> </button>

                                                                    <%--AVVISO ERRORE DATI NON INSERITI--%>
                                                                    <%-- <asp:ValidationSummary runat="server" ID="Summary" class="" ValidationGroup="MailInfo" DisplayMode="BulletList"
                                                                        ShowSummary="true" HeaderText='<%# references.ResMan("Common", Lingua,"testoDatiMancanti")  %>' />--%>

                                                                    <asp:ValidationSummary ID="Summary" ValidationGroup="MailInfo" runat="server" DisplayMode="SingleParagraph" Font-Size="Medium" ShowValidationErrors="true" HeaderText='<%# references.ResMan("Common", Lingua,"testoDatiMancanti")  +  "<br/><style> span.errorvalidateclass[style*=\"visibility: visible\"] + * {  border: 2px solid red; }  </style>" %>' />

                                                                    <%--AVVISO ERRORE TRATTAMENTO DATI NON SPUNTATO--%>
                                                                    <div id="recaptcharesponse" style="display: none;" class=""></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </div>
                <%--</div>--%>
            </section>

            <asp:PlaceHolder ID="plhRisposta" runat="server" Visible="false">
                <h2>
                    <asp:Literal ID="lblRisposta" runat="server" Text=""></asp:Literal></h2>
                <br />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plhDove" runat="server" Visible="false">
                <div id="map" style="height: 410px; width: calc(100% - 80px); margin: 40px auto">
                </div>
            </asp:PlaceHolder>
            <div class="container" runat="server" id="divRoutes" visible="false">
                <div class="row">
                    <div class="col-sm-12">
                        <%= references.ResMan("Common", Lingua,"TestoRaggiungerci") %>
                    </div>
                </div>
                <div class="row" style="text-align: center">
                    <div class="col-sm-12">
                        <asp:Label runat="server" ID="lblContenutiDove"></asp:Label>
                        <div class="divContentTitle" style="margin: 0px auto; margin-bottom: 40px; height: auto; display: block">
                            <div style="font-size: 14px; padding-top: 0px;">
                                <span id="lblPartenza">
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"GooglePartenza") %>' runat="server" />
                                </span>
                            </div>
                            <div style="font-size: 12px; padding-top: 5px; padding-bottom: 5px">
                                <input id="startvalue" type="text" style="width: 260px; height: 30px; border: 1px solid #999999; width: 300px;" />
                            </div>
                            <div style="font-size: 14px;">
                                <input id="Button1" style="box-shadow: none; border: none; color: white; cursor: pointer; width: 300px;"
                                    type="button" value="<%= references.ResMan("Common",Lingua,"GoogleCalcolapercorso").ToString() %>" onclick="return Button1_onclick()" />
                            </div>
                        </div>
                        <div id="directionpanel" style="height: auto; width: 96%; padding-bottom: 10px">
                        </div>
                    </div>
                    <div class="col-sm-2" style="text-align: left">
                        <asp:Literal ID="litPosizione" Text="" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--ELIMINO IL FORM CONTATTI DELLA MASTER--%>
    <script>
        jQuery(function ($) {
            $('#ctl00_divContattiMaster').css({ 'display': 'none' });
        });
        <%--MODULO LINK VENDITA E_COMMERCE--%>
        jQuery(function ($) {
            $('#ctl00_masterlow1').html('');
        });
    </script>
</asp:Content>
