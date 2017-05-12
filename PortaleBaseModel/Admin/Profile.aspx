<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="admin_Profile" %>

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
            <a href="Associaruoli.aspx">Associazione Ruoli a Utenti</a><br />
            </div>
            <br />
            Utente da modificare:
            <br />
            <asp:DropDownList ID="UsersList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UserList_changed" />
            &nbsp;<br />
            <br />
            <div>
                Nome:<br />
                <asp:TextBox ID="FirstName" runat="server"></asp:TextBox><br />
                Cognome:<br />
                <asp:TextBox ID="LastName" runat="server"></asp:TextBox><br />
                <br />
                E-Mail:<br />
                <asp:TextBox ID="EMail" runat="server"></asp:TextBox><br />
                Cellulare:<br />
                <asp:TextBox ID="Cellulare" runat="server"></asp:TextBox><br />
                Id Cliente anagrafica:<br />
                <asp:TextBox ID="txtIdcliente" runat="server"></asp:TextBox><br />
                Id Socio :<br />
                <asp:TextBox ID="txtIdsocio" runat="server"></asp:TextBox><br />
                <asp:Button ID="Button1" runat="server" Text="Registra" OnClick="Button1_Click" />
                <p>
                    Ricordarsi che per memorizzare qualsiasi modifica è necessario salvare premendo<br />
                    il tasto "Registra" dopo le variazioni!!!!
                </p>
                   <asp:Button Text="Restart" ID="reset" OnClick="reset_Click" runat="server" />
            </div>
            <asp:Label ID="Results" runat="server"></asp:Label>
            <p>
                <br />
                ----------------------------------------------------------------------<br />
                CAMBIO PASSWORD PER L'UTENTE:
            </p>
            Vecchia Password:<asp:TextBox ID="txtPasswordold" runat="server"></asp:TextBox>
            Nuova Password:<asp:TextBox ID="txtPasswordnew" runat="server"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="Cambio Password" OnClick="Button2_Click" />
            <br />
            <asp:Label ID="lblquestion" runat="server" />
            <br />
            <asp:TextBox ID="txtanswer" runat="server"></asp:TextBox>
            <asp:Button ID="Button3" runat="server" Text="Reset Password" Enabled="true" OnClick="Button3_Click" />
            <br />
            <asp:Label ID="lblResultsPsw" runat="server" />
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
