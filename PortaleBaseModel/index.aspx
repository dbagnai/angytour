<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" EnableViewState="false"
    CodeFile="index.aspx.cs" Inherits="index" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderIndextext" runat="Server">

    <asp:Literal Text="" ID="litNomePagina" runat="server" />
    <asp:Literal Text="" ID="litTextHeadPage" runat="server" />

    <%-- <!--=== Breadcrumbs ===-->
        <div class="breadcrumbs">
            <div class="container">
                <h1 class="pull-left">Login</h1>
                <p></p>
                <ul class="pull-right breadcrumb">
                    <li><a href='<%# references.ResMan("Common",Lingua,"LinkHome") %>' runat="server">
                        <asp:Literal Text='<%# references.ResMan("Common",Lingua,"testoHome") %>' runat="server" /></a></li>
                    <li class="active">da creare link pagina</li>
                </ul>
            </div>
        </div>
        <!--/breadcrumbs-->
        <!--=== End Breadcrumbs ===-->--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
 

