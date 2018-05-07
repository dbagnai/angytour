<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
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
        <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/js/googleMaps.js")%>"></script>
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
            <p><asp:Label runat="server" ID="output" Font-Size="Medium"></asp:Label></p>
            <asp:Label runat="server" ID="lblContenutiContatti"></asp:Label>
            <asp:PlaceHolder runat="server" ID="plhForm" Visible="true">
                <div style="width: 80%; margin: 20px auto; background-color: #efefef; padding: 20px; border-radius: 6px; border: 2px solid #f0f0f0">
                    <div class="ui-15">
                        <div class="ui-content">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 ui-padd">
                                        <div class="ui-form">
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
                                                <div class="row" id="divOrario" runat="server" visible="false">
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <Ajax:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                                TargetControlID="txtData">
                                                            </Ajax:CalendarExtender>
                                                            <asp:TextBox class="form-control" ID="txtData" Width="99%" runat="server"
                                                                placeholder='<%# references.ResMan("Common", Lingua,"FormData %>' />
                                                            <label class="ui-icon"><i class="fa fa-calendar"></i></label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <asp:TextBox ID="txtOrario" Width="99%" runat="server"
                                                                class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormOrario %>' />
                                                            <label class="ui-icon"><i class="fa fa-clock-o"></i></label>
                                                        </div>
                                                    </div>
                                                </div>--%>
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



                                                <div class="row" runat="server" visible="true" id="divLocations">
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <%-- <asp:Literal ID="Literal1" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTestoLocations %>' />
                                                            <br />--%>
                                                            <asp:DropDownList runat="server" Width="100%" ID="ddlLocations" class="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <asp:TextBox ID="txtArrivo" Width="99%" runat="server"
                                                                class="form-control datepicker" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPeriododa") %>' />
                                                            <label class="ui-icon"><i class="fa fa-calendar"></i></label>
                                                            <%-- <Ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                                TargetControlID="txtArrivo">
                                                            </Ajax:CalendarExtender>--%>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <asp:TextBox ID="txtPartenza" Width="99%" runat="server" class="form-control datepicker" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPeriodoa") %>' />
                                                            <label class="ui-icon"><i class="fa fa-calendar"></i></label>
                                                            <%--  <Ajax:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                                TargetControlID="txtPartenza">
                                                            </Ajax:CalendarExtender>--%>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">
                                                            <%-- <Ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender1" AcceptNegative="None"
                                                                MaskType="Number" TargetControlID="txtAdulti" InputDirection="RightToLeft" Mask="99"
                                                                AutoComplete="false">
                                                            </Ajax:MaskedEditExtender>--%>
                                                            <script>
                                                                jQuery(function ($) {
                                                                    $("#ctl00_ContentPlaceHolder1_txtAdulti").mask("99", { placeholder: " " });
                                                                });
                                                            </script>
                                                            <asp:TextBox ID="txtAdulti" Width="99%" runat="server"
                                                                class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoAdulti") %>' />
                                                            <label class="ui-icon"><i class="fa fa-group"></i></label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="ui-input">

                                                            <script>
                                                                jQuery(function ($) {
                                                                    $("#ctl00_ContentPlaceHolder1_txtBambini").mask("99", { placeholder: " " });
                                                                });
                                                            </script>
                                                            <asp:TextBox ID="txtBambini" Width="99%" runat="server"
                                                                class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoBambini") %>' />
                                                            <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                        </div>
                                                    </div>
                                                </div>


                                            </asp:Panel>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="ui-input">
                                                        <asp:TextBox ID="txtNome" Width="99%" runat="server"
                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />
                                                        <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtNome"
                                                            ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' Text="*" ValidationGroup="MailInfo" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="ui-input">
                                                        <asp:TextBox ID="txtSoc" Width="99%" runat="server"
                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto16l") %>' />
                                                        <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                        <asp:RequiredFieldValidator runat="server" ID="reqValidatorNome" ControlToValidate="txtSoc"
                                                            ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto16lErr") %>' Text="*" ValidationGroup="MailInfo" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="ui-input">
                                                        <asp:TextBox ID="txtEmail" Width="99%" runat="server"
                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' />
                                                        <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtEmail"
                                                            ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' Text="*" ValidationGroup="MailInfo" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="ui-input">
                                                        <asp:TextBox ID="txtTel" Width="99%" runat="server"
                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                                                        <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" runat="server" id="divRegione" visible="false">
                                                <div class="col-sm-12">
                                                    <div class="ui-input">
                                                        <asp:Literal ID="Literal8" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto6") %>' />*<br />
                                                        <asp:DropDownList class="form-control" runat="server" ID="ddlRegione" AppendDataBoundItems="true"
                                                            OnInit="ddlRegione_OnInit" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto6") %>' />
                                                        <asp:RequiredFieldValidator runat="server" ID="reqValidatorRegione" ControlToValidate="ddlRegione" Enabled="false"
                                                            ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto6Err") %>' Text="*" ValidationGroup="MailInfo" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <div class="ui-input">
                                                        <asp:TextBox ID="txtDescrizione" Width="99%"
                                                            TextMode="MultiLine" Font-Size="17px" Font-Names="Raleway"
                                                            Height="250px" runat="server" class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto17") %>' />
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtDescrizione"
                                                            ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto17Err") %>' Text="*" ValidationGroup="MailInfo" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-12" style="text-align: center">
                                                    <asp:ValidationSummary runat="server" ID="Summary" ValidationGroup="MailInfo" DisplayMode="BulletList"
                                                        ShowSummary="true" HeaderText='<%# references.ResMan("Common", Lingua,"testoDatiMancanti")  %>' />
                                                    <br />
                                                    <div id="recaptcharesponse"></div>
                                                    <asp:Button ID="btnInvia" runat="server" Text='<%# references.ResMan("Common", Lingua,"TestoInvio")  %>' CausesValidation="true" UseSubmitBehavior="true"
                                                        ValidationGroup="MailInfo" class="divbuttonstyle" OnClick="btnInvia_Click" OnClientClick="return ConfirmCancella()" />
                                                    <%--    <div class="g-recaptcha" id="rcaptcha" data-sitekey="6LccbRMUAAAAAAN14HC8RFxwNMaqdGvJFPQEVinq"></div>--%>
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
                                                                function ConfirmCancella() {
                                                                     if ($('<%= "#" + chkPrivacy.ClientID %>')[0].checked == false) {
                                                                         $("#recaptcharesponse").html('<%= references.ResMan("Common", Lingua,"txtPrivacyError") %>');
                                                                         return false;
                                                                     }
                                                                     else {
                                                                         $("#recaptcharesponse").html('');
                                                                     }

                                                                  <%--  var response = grecaptcha.getResponse();
                                                                    if (response.length == 0)  //reCaptcha not verified
                                                                    {
                                                                        $("#recaptcharesponse").html('<%= references.ResMan("Common", Lingua,"testoCaptcha") %>');
                                                                        return false;
                                                                    }
                                                                    else {--%>
                                                                     if (Page_ClientValidate("MailInfo")) {
                                                                         /*do work and go for postback*/
                                                                     } else {
                                                                         // alert('not validated');
                                                                     }
                                                                    //}

                                                                    //if (typeof (Page_ClientValidate) == 'function') if (Page_ClientValidate()) { return confirm('Are   you   sure   to   submit   ?'); } else return false;
                                                                }
                                                    </script>
                                                    <br /><br />
                                                    <asp:CheckBox CssClass="from-control" ID="chkPrivacy" runat="server" Checked="true" Text='<%# references.ResMan("Common", Lingua,"chkPrivacy")  %>' />
                                                    <a id="linkPrivacy" runat="server" onclick="OpenWindow('PrivacyI.htm','','scrollbars=yes,width=600,height=400')"
                                                        href="javascript:;">(D.Lgs 196/2003)</a><br />
                                                    <asp:CheckBox CssClass="from-control" ID="chkNewsletter" runat="server" Checked="true" Text='<%# references.ResMan("Common", Lingua,"titoloNewsletter1")  %>' />

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

            <asp:PlaceHolder ID="plhRisposta" runat="server" Visible="false">
                <h2><asp:Literal ID="lblRisposta" runat="server" Text=""></asp:Literal></h2>
                <br />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plhDove" runat="server" Visible="false">
                <div id="map" style="height: 410px; width: 100%; margin: 40px auto">
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
                        <div class="divContentTitle" style="margin: 0px auto; margin-bottom: 50px; height: auto; display: block">
                            <div style="font-size: 14px; padding-top: 8px;">
                                <span id="lblPartenza">
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"GooglePartenza") %>' runat="server" />
                                </span>
                            </div>
                            <div style="font-size: 12px; padding-top: 5px; padding-bottom: 5px">
                                <input id="startvalue" type="text" style="width: 260px; height: 30px;" />
                            </div>
                            <div style="margin-left: 10px; font-size: 14px;">
                                <input id="Button1" style="box-shadow: none"
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
</asp:Content>
