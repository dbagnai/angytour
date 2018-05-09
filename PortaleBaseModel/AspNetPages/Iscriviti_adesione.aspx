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
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox ID="chkPrivacy" runat="server" Checked="false" />
                            <span class="cr"><i class="cr-icon fa fa-check"></i></span>
                            <%= references.ResMan("Common", Lingua,"chkprivacy") %><a target="_blank" href="<%=CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua,"linkinformativa")) %>"> (<%= references.ResMan("Common", Lingua,"testoprivacyperlink") %>) </a>
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

                    <br />
                    <br />
                    <asp:Button ID="btnInvia" runat="server" Text="Invia" CausesValidation="true" UseSubmitBehavior="false"
                        ValidationGroup="MailInfo" OnClick="btnInviaAStrutturaSenzaValidazione_Click" />
                    <%-- <asp:UpdateProgress ID="UpdateProgress4" runat="server" DisplayAfter="0" DynamicLayout="false"
                                        AssociatedUpdatePanelID="updIscrivi">
                                        <ProgressTemplate>
                                            <div style="float: left; background-color: Transparent; color: Black; padding: 0px">
                                                <img alt="" src="../images/Varie/indicator.gif" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>--%>
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
