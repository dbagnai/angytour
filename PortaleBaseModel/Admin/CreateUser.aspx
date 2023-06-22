<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="admin_create" MasterPageFile="~/Admin/MasterPageAdmin.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div class="container">
        <div class="row">
            <div class="col-12">
                <div style="margin-right: auto; margin-left: auto; width: 300px;">
                    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8"
                        ForeColor="#000000" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana"
                        Font-Size="0.8em" LoginCreatedUser="False" OnCreatedUser="CreateUserWizard1_CreatedUser"
                        OnCreatingUser="CreateUserWizard1_CreatingUser">
                        <WizardSteps>
                            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                            </asp:CreateUserWizardStep>
                            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                                <ContentTemplate>
                                    <table border="0" style="font-size: 100%; font-family: Verdana">
                                        <tr>
                                            <td align="center" colspan="2" style="font-family: GoudiOldeStyleBold; color: white; background-color: #F7F6F3">
                                                <b>Completo</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Creazione dell'account completata.</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                <asp:Button ID="ContinueButton" runat="server" BackColor="#888888" BorderColor="#cccccc"
                                                    BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="Continue"
                                                    Font-Names="Verdana" ForeColor="#ffffff" OnClick="ContinueButton_Click" Text="Continua"
                                                    ValidationGroup="CreateUserWizard1" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:CompleteWizardStep>
                        </WizardSteps>
                        <SideBarStyle BackColor="#EDFCC3" BorderWidth="0px" Font-Size="0.9em" VerticalAlign="Top" />
                        <TitleTextStyle BackColor="#888888" Font-Bold="True" ForeColor="White" />
                        <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="Black" />
                        <NavigationButtonStyle BackColor="#888888" BorderColor="#CCCCCC" BorderStyle="Solid"
                            BorderWidth="1px" Font-Names="Verdana" ForeColor="#ffffff" />
                        <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
                            ForeColor="Black" HorizontalAlign="Center" />
                        <CreateUserButtonStyle BackColor="#888888" BorderColor="#CCCCCC" BorderStyle="Solid"
                            BorderWidth="1px" Font-Names="Verdana" ForeColor="#ffffff" />
                        <ContinueButtonStyle BackColor="#888888" BorderColor="#ffffff" BorderStyle="Solid"
                            BorderWidth="1px" Font-Names="Verdana" ForeColor="#ffffff" />
                        <StepStyle BorderWidth="0px" />
                    </asp:CreateUserWizard>
                </div>
            </div>
        </div>
        <br />
        <div class="row mt-1">
            <div class="col-12">
                <div style="margin-right: auto; margin-left: auto; width: 300px;">
                    <b>Ruoli disponibili:</b>
                    <br />
                    <asp:ListBox ID="RolesList" runat="server" SelectionMode="Single"></asp:ListBox>
                    <br />
                    <asp:Label ID="Results" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <br />
        <div class="row mt-1">
            <div class="col-12">
                <div style="margin-right: auto; margin-left: auto; width: 300px;">
                    <b>Lista Utenti (Selezionare per Eliminazione):</b>
                    <br />
                    <asp:DropDownList ID="UsersList" runat="server" />
                    &nbsp;<br />
                    &nbsp;<br />
                    <asp:Button ID="Button1" runat="server" ValidationGroup="Elimina" Text="Elimina Utente"
                        OnClick="Button1_Click" BackColor="#888888" ForeColor="White" />
                    <br />
                    <asp:Label ID="lbl_results" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
