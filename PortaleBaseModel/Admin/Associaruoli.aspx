<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Associaruoli.aspx.cs" Inherits="admin_roles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pagina senza titolo</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager2" runat="server" AllowCustomErrorsRedirect="True"
        AsyncPostBackErrorMessage="Errore generico. Contattare HelpDesk" AsyncPostBackTimeout="400"
        EnablePartialRendering="true" />
    <asp:UpdatePanel ID="UPDddlUser" runat="server" UpdateMode="always">
        <ContentTemplate>
            <div style="background-color:#ccc;">
                <asp:Literal ID="litMenu" runat="server" Text="SELEZIONA ATTIVITA':"></asp:Literal><br />
                <a id="A1" href="~/index.aspx" runat="server">Home Portale</a><br />
                <a href="default.aspx">Homepage Amministrazione</a><br />
                <a href="CreateUser.aspx">Creazione Utenti</a><br />
                <a href="CreateRole.aspx">Crezione Ruoli</a><br />
                <a href="Profile.aspx">Gestione Profilo Utente</a><br />
            </div>
            <br />
            <div style="width: 100%; color: #FFFFFF;">
                <div style="float: left; width: 33%;">
                    Ruolo:
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RolesList"
                        ErrorMessage="Seleziona Ruolo" />
                </div>
                <div style="float: left; width: 33%">
                    Utente:</div>
                <div style="float: left; width: 33%">
                    Comandi:</div>
            </div>
            <div style="width: 100%;">
                <div style="float: left; width: 33%">
                    <asp:ListBox ID="RolesList" runat="server" SelectionMode="Multiple" Height="60px"
                        Width="136px"></asp:ListBox>
                </div>
                <div style="float: left; width: 33%">
                    <asp:DropDownList ID="UsersList" runat="server" Width="100px" />
                </div>
                <div style="float: left; width: 33%">
                    <asp:Button ID="Button1" runat="server" Text="Associa a Ruolo" OnClick="Button1_Click" />
                    <br />
                    <asp:Button ID="Button2" runat="server" Text="Rimuovi da Ruolo" OnClick="Button2_Click" />
                    <br />
                    <asp:Label ID="Results" runat="server"></asp:Label>
                </div>
            </div>
            <div style="width: 580px; float: right">
                <div style="width: 580px; float: right">
                    Lista Utenti/Ruoli:
                </div>
                <div style="width: 580px; float: right">
                    <asp:GridView ID="Usereroles" runat="server" AutoGenerateColumns="false" CellPadding="4"
                        ForeColor="#000000" GridLines="None" Width="185px">
                        <FooterStyle BackColor="#EDFCC3" Font-Bold="True" ForeColor="#8AB95D" />
                        <RowStyle BackColor="#EDFCC3" ForeColor="Black" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#8AB95D" Font-Bold="True" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                            <asp:TemplateField HeaderText="Ruoli">
                                <ItemTemplate>
                                    <asp:BulletedList ID="RolesList" runat="server" DataSource='<%# Roles.GetRolesForUser(Eval("username").ToString()) %>'>
                                    </asp:BulletedList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="prg" AssociatedUpdatePanelID="UPDddlUser" runat="server"
        DisplayAfter="0">
        <ProgressTemplate>
            <div>
                Aggiornamento in corso ! Attendere
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </form>
</body>
</html>
