<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="Mappa.aspx.cs" Inherits="AspNetPages_Mappa" Title=""
    MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

          <script type="text/javascript">
              makeRevLower = true;
    </script>

    <div class="row">
        <h1>
            <asp:Literal runat="server" ID="litNomeContenuti">
            </asp:Literal>
        </h1>
        <div style="margin-left: 10px; padding-right: 10px">
            <asp:Literal ID="litMainContent" runat="server"></asp:Literal>
            <br />
            <a id="linkBanner" runat="server" href="#" target="_blank">
                <img style="border: none" id="imgBottom" alt="" src="" runat="server" visible="false" /></a>
        </div>
        <div class="divlinks" id="divIndex" runat="server" style="width: 100%; font-size: 16px; margin-top: 5px">
            <asp:TreeView ID="TreeView1" runat="server" DataSourceID="SiteMapDataSource1" Font-Size="Small"
                Target="_blank">
            </asp:TreeView>
            <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
            <div style="font-size: 14pt; color: #2e3192; margin-left: 10px; margin-top: 10px; margin-bottom: 10px;">
                <asp:Literal ID="Literal1" Text='<%$ Resources:Common,TitoloListaArticoli %>' runat="server" />
            </div>
        </div>
    </div>

</asp:Content>
