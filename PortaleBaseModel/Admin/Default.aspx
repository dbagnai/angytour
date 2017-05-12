<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" %>

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
             <a href="CreateUser.aspx">Creazione Utenti</a><br />
        <a href="CreateRole.aspx">Crezione Ruoli</a><br />
        <a href="Associaruoli.aspx">Associazione Ruoli a Utenti</a><br />
        <a href="Profile.aspx">Gestione Profilo Utente</a><br />
        </div>
        <br />
        Lista Utenti e Ruoli:<br />
        <asp:GridView ID="UsersList" runat="server" CellPadding="4" ForeColor="Black" GridLines="None"
            AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="UsersList_PageIndexChanging">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#5D7B9D" ForeColor="Black" HorizontalAlign="Center" Font-Names="Arial" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                <asp:TemplateField HeaderText="Nome e Cognome">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# getnome(Eval("Username").ToString()) %>' ID="NomeCognome" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="E-Mail">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# getEmail(Eval("Username").ToString()) %>' ID="EMail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ruoli">
                    <ItemTemplate>
                        <asp:BulletedList runat="server" DataSource='<%# Roles.GetRolesForUser(Eval("Username").ToString()) %>'
                            ID="RolesList" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OnLine">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# isonline(Eval("Username").ToString()) %>' ID="Online" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
