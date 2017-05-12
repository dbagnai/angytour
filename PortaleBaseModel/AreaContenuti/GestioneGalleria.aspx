<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GestioneGalleria.aspx.cs"
    Inherits="AreaContenuti_GestioneGalleria" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Amministrazione - Gestione Galleria</title>
</head>
<body style="font-size: 13px; background-color: #ccc; color:Black;  padding: 10px 10px 10px 10px">
    <form id="form1" runat="server">
    <a href="Default.aspx">Torna a pagina di selezione</a>
    <h2>
        Gestione Foto Gallery</h2>
    <br />
    <asp:Label ID="output" runat="server"></asp:Label>
    <h3>
        Carica foto</h3>
    <asp:FileUpload ID="UploadFoto" runat="server" />
    <asp:Button ID="btnCarica" runat="server" Text="Carica Foto" OnClick="btnCarica_Click" />
    <asp:Button ID="btnElimina" runat="server" Text="Elimina Foto" OnClick="btnElimina_Click" /><br/>
    <asp:Repeater runat="server" ID="rptFoto">
        <ItemTemplate>
            <asp:ImageButton ID="btnImmagine" OnClick="btnImmagine_Click" runat="server" Width="80px"
                ImageUrl='<%# PercorsoFiles + "/Ant" +  Eval("Value") %>' CommandArgument='<%# Eval("Value") %>'
                OnPreRender="btnImmagine_PreRender" />
        </ItemTemplate>
    </asp:Repeater>
    </form>
</body>
</html>
