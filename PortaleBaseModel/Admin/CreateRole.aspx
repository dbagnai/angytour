<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateRole.aspx.cs" Inherits="admin_CreateRole" %>

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
        <a href="CreateUser.aspx">Creazione Utenti</a><br />
        <a href="Profile.aspx">Gestione Profilo Utente</a><br />
        <a href="Associaruoli.aspx">Associazione Ruoli a Utenti</a><br />
        </div>
        <br />
        <div>
            Lista Ruoli:<br />
            <asp:ListBox ID="RolesList" runat="server" SelectionMode="Single" Width="160px">
            </asp:ListBox>
            <asp:Button ID="Button2" runat="server" Text="Elimina Ruolo" OnClick="Button2_Click" />
            <br />
            Ruolo:
            <br />
            <asp:TextBox ID="RoleName" runat="server" Width="160px"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Crea Ruolo" OnClick="Button1_Click" />
            <br />
            <asp:Label ID="Results" runat="server"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
