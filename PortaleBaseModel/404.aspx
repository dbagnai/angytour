<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="404.aspx.cs" Inherits="err404" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderIndextext" runat="Server">
    <div class="row my-5" style="text-align: center">
        <div class="col-12">
            <h2>
                <%= ReplaceAbsoluteLinks(ReplaceLinks(references.ResMan("Common",Lingua,"ContentError") ) ) %>
            </h2>
            <asp:Literal Text="" ID="output" runat="server" />
        </div>
    </div>
</asp:Content>
