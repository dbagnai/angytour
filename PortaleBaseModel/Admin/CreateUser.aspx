<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="admin_create" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pagina senza titolo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div style="background-color:#ccc;">
       <asp:Literal ID="litMenu" runat="server" Text="SELEZIONA ATTIVITA':"></asp:Literal><br />
            <a id="A1" href="~/index.aspx" runat="server">Home Portale</a><br />
             <a href="default.aspx">Homepage Amministrazione</a><br />
        <a href="CreateRole.aspx">Crezione Ruoli</a><br />
        <a href="Profile.aspx">Gestione Profilo Utente</a><br />
        <a href="Associaruoli.aspx">Associazione Ruoli a Utenti</a><br />
        </div>
        <br />
        <div style="float: left; margin-right: 10px">
            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" BackColor="#EDFCC3" BorderColor="#E6E2D8"
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
                                    <td align="center" colspan="2" style="  font-family: GoudiOldeStyleBold; color: white; background-color: #8AB95D">
                                        Completo
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Creazione dell'account completata.
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Button ID="ContinueButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                                            BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="Continue"
                                            Font-Names="Verdana" ForeColor="#000000" OnClick="ContinueButton_Click" Text="Continua"
                                            ValidationGroup="CreateUserWizard1" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:CompleteWizardStep>
                </WizardSteps>
                <SideBarStyle BackColor="#EDFCC3" BorderWidth="0px" Font-Size="0.9em" VerticalAlign="Top" />
                <TitleTextStyle BackColor="#8AB95D" Font-Bold="True" ForeColor="White" />
                <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="Black" />
                <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
                    BorderWidth="1px" Font-Names="Verdana" ForeColor="#000000" />
                <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
                    ForeColor="Black" HorizontalAlign="Center" />
                <CreateUserButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
                    BorderWidth="1px" Font-Names="Verdana" ForeColor="#000000" />
                <ContinueButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
                    BorderWidth="1px" Font-Names="Verdana" ForeColor="#000000" />
                <StepStyle BorderWidth="0px" />
            </asp:CreateUserWizard>
        </div>
        Ruoli disponibili:
        <br />
        <asp:ListBox ID="RolesList" runat="server" SelectionMode="Single"></asp:ListBox>
        <br />
        <asp:Label ID="Results" runat="server"></asp:Label>
        <br />
        <br />
        Lista Utenti (Selezionare per Eliminazione):
        <br />
        <asp:DropDownList ID="UsersList" runat="server" />
        &nbsp;<br />
        <asp:Button ID="Button1" runat="server" ValidationGroup="Elimina" Text="Elimina Utente"
            OnClick="Button1_Click" />
        <br />
        <asp:Label ID="lbl_results" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
