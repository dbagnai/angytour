<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="Iscriviti_adesione.aspx.cs" Inherits="AspNetPages_Iscrivitiadesione" Title=""
    MaintainScrollPositionOnPostback="false" %>



<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        makeRevLower = true;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <script type="text/javascript">
            function OpenWindow(Url, NomeFinestra, features) {
                window.open(Url, NomeFinestra, features);
            }
        </script>
        <div style="height: 40px; font-size: 28px">
            <asp:Literal ID="litMainContent" Text='<%# references.ResMan("Common", Lingua,"titleIscriviti") %>'
                runat="server" />
        </div>
        <br />
        <div class="divTextOfferte">
            <asp:Label runat="server" ID="RptDescrizione" CssClass="TitleText"></asp:Label><br />
        </div>
        <br />
        <asp:Label runat="server" ID="output" ForeColor="Red" Font-Size="Small"></asp:Label>
        <asp:PlaceHolder runat="server" ID="plhForm">

            <div class="row">
                <div class="col-sm-3" style="text-align: right">
                    <asp:Literal ID="Literal2" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtNome"
                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' Text="*" ValidationGroup="MailInfo" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtNome" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3" style="text-align: right">
                    <asp:Literal ID="Literal3" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto3") %>' />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtCognome"
                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto3Err") %>' Text="*" ValidationGroup="MailInfo" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtCognome" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3" style="text-align: right">
                    <asp:Literal ID="Literal5" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtEmail"
                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' Text="*" ValidationGroup="MailInfo" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtEmail" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3" style="text-align: right">
                    <asp:Literal ID="Literal6" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtIndirizzo"
                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto10Err") %>' Text="*" ValidationGroup="MailInfo" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtIndirizzo" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3" style="text-align: right">
                    <asp:Literal ID="Literal8" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtTelefono"
                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto11Err") %>' Text="*" ValidationGroup="MailInfo" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtTelefono" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3" style="text-align: right">
                    <asp:Literal ID="Literal4" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto14") %>' />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtDescrizione"
                        Height="250px" Style="margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3" style="text-align: right">&nbsp;</div>
                <div class="col-sm-9">
                      <div class="row" style="text-align: left">
                        <div class="col-sm-12 text-white">
                            <%= references.ResMan("basetext", Lingua,"privacytext") %>
                            <div class="checkbox" style="margin-bottom: 20px">
                                <label>
                                    <input id="chkPrivacy" runat="server" type="checkbox" />
                                    <span class="cr"><i class="cr-icon fa fa-check" style="color: #000"></i></span><%= references.ResMan("basetext", Lingua,"privacyconsenso") %>
                                </label>
                            </div>
                            <div class="checkbox" style="margin-bottom: 20px">
                                <label>
                                    <input type="checkbox" id="chkNewsletter" runat="server" />
                                    <span class="cr"><i class="cr-icon fa fa-check" style="color: #000"></i></span><%= references.ResMan("basetext", Lingua,"privacyconsenso1") %>
                                </label>
                            </div>
                        </div>
                    </div>

                    <br />
                    <br />
                    <button id="btnInvia" type="button" class="btn btn-lg btn-block" style="width: 200px" runat="server" validationgroup="MailInfo" onclick="ConfirmContactValue(this);"><%=  references.ResMan("Common", Lingua,"TestoBtnNewsletter")  %> </button>
                    <asp:Button ID="btnInviaSrv" Style="display: none" runat="server" OnClick="btnInviaAStrutturaSenzaValidazione_Click" />
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
                        function ConfirmContactValue(elembtn) {
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
                                $(elembtn).attr("disabled", "")
                                $(elembtn).val("Wait ..");
                                var buttpost = document.getElementById("<%= btnInviaSrv.ClientID  %>");
                                buttpost.click();
                            } else {
                                // alert('not validated');
                                return false;
                            }
                            //}
                        }
                    </script>
                    <div id="recaptcharesponse" style="margin: 5px"></div>
                    <br />
                    <asp:ValidationSummary runat="server" ID="Summary" ValidationGroup="MailInfo" DisplayMode="BulletList"
                        ShowSummary="true" HeaderText="Rilevati dati mancanti / Missing needed infos " />
                    <br />
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plhRisposta" runat="server" Visible="false">
            <div class="alert alert-block">
                <asp:Label Font-Bold="true" Font-Size="Medium" ID="lblRisposta" runat="server" Text=""></asp:Label>
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
