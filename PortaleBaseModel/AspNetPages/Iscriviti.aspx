<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="Iscriviti.aspx.cs" Inherits="_Iscriviti" Title="" %>


<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <script type="text/javascript">
        makeRevLower = true;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <%--   <asp:UpdatePanel runat="server" ID="updIscrivi">
        <ContentTemplate>--%>
        <div class="row">
            <div class="col-12">
                <div class="alert alert-block" style="color: #444">
                    <asp:Label Font-Size="Medium" ID="output" runat="server" Text=""></asp:Label>
                    <asp:Label runat="server" ID="litDescrizioneIscrivi" Text="" Font-Size="Medium" />
                </div>
            </div>
        </div>

        <script type="text/javascript">
            function OpenWindow(Url, NomeFinestra, features) {
                window.open(Url, NomeFinestra, features);
            }
        </script>

        <asp:PlaceHolder runat="server" ID="plhForm">
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal2" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtNome" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal3" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto16l") %>' />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtCognome"
                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto16lErr") %>' Text="*" ValidationGroup="MailInfo" />
                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtCognome" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal5" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' />*
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtEmail"
                                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' Text="*" ValidationGroup="MailInfo" />
                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtEmail" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <!-- Inserire qui ddlnaz/reg/prov/com -->
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal11" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto5") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:DropDownList ID="ddlNazione"
                        CssClass="form-control" Width="50%" AutoPostBack="true" runat="server" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddlNazione_SelectedIndexChanged" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal12" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto6") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:DropDownList ID="ddlRegione"
                        CssClass="form-control" Width="50%" AutoPostBack="true" runat="server" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddlRegione_SelectedIndexChanged" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal13" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:DropDownList ID="ddlProvincia"
                        AutoPostBack="true" CssClass="form-control" Width="50%" runat="server" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal14" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto8") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:DropDownList ID="ddlComune"
                        CssClass="form-control" Width="50%" runat="server" AppendDataBoundItems="true" />
                </div>
            </div>
            <!-- Fine selez. geografica-->
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal7" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtCap" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal6" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtIndirizzo" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal1" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto20") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtPiva" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal8" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtTelefono" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal9" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto12") %>' />

                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtProfessione" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal10" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto13") %>' />

                </div>
                <div class="col-12 col-sm-9">

                    <asp:TextBox ID="txtNascita" Width="100%" CssClass="datepicker"
                        Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />

                    <%-- <Ajax:CalendarExtender ID="calext" runat="server" Format="dd/MM/yyyy"
                                        TargetControlID="txtNascita">
                                    </Ajax:CalendarExtender>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">
                    <asp:Literal ID="Literal4" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto14") %>' />
                </div>
                <div class="col-12 col-sm-9">
                    <asp:TextBox ID="txtDescrizione" Width="100%" Font-Names="Raleway" TextMode="MultiLine"
                        Height="250px" Style="margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3" style="text-align: left">&nbsp;</div>
                <div class="col-12 col-sm-9">
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox ID="chkPrivacy" runat="server" Checked="false" />
                            <span class="cr"><i class="cr-icon fa fa-check"></i></span>
                            <%= references.ResMan("Common", Lingua,"chkprivacy") %><a target="_blank" href="<%=CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua,"linkPrivacypolicy")) %>"> (<%= references.ResMan("Common", Lingua,"testoprivacyperlink") %>) </a>
                        </label>
                    </div>
                    <br />
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox ID="chkConsensoMail" runat="server" Checked="false" />
                            <span class="cr"><i class="cr-icon fa fa-check"></i></span>
                            <%= references.ResMan("Common", Lingua,"testoConsenso1") %>
                        </label>
                    </div>
                    <button id="btnInvia" type="button" class="btn btn-lg btn-block" style="width: 200px" runat="server" validationgroup="MailInfo" onclick="ConfirmContactValue(this);"><%=  references.ResMan("Common", Lingua,"TestoBtnNewsletter")  %> </button>
                    <asp:Button ID="btnInviaSrv" Style="display: none" runat="server" OnClick="btnInvia_Click" />
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
                    <asp:ValidationSummary runat="server" ID="Summary" ValidationGroup="MailInfo" DisplayMode="BulletList"
                        ShowSummary="true" HeaderText="Rilevati dati mancanti per l'invio della proposta:" />
                </div>
            </div>

        </asp:PlaceHolder>

        <div class="row">
            <div class="col-12">
                <asp:PlaceHolder ID="plhRisposta" runat="server" Visible="false">
                    <div class="alert alert-block" style="color: #444">
                        <asp:Label Font-Size="Medium" ID="lblRisposta" runat="server" Text=""></asp:Label>
                        <br />
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
