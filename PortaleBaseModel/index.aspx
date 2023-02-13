<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" EnableViewState="false"
    CodeFile="index.aspx.cs" Inherits="index" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderIndextext" runat="Server">
    <asp:Literal Text="" ID="litNomePagina" runat="server" />
    <asp:Literal Text="" ID="litTextHeadPage" runat="server" /> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
 

