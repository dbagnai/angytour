<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="RisultatiResource.aspx.cs" Inherits="AspNetPages_RisultatiResource" EnableViewState="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderSubsSlider" runat="Server">
    <script>
        history.scrollRestoration = 'manual'; //Evito che il browser mi riporti automaticamente alla posizione verticale
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <div class="container" style="text-align: center; margin-top: 10px">
        <div class="row" runat="server" id="divTitle">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12">
                <h1 class="title-block" style="line-height: normal;">
                    <asp:Literal Text="" runat="server" ID="litNomePagina" /></h1>
            </div>
        </div>
    </div>
    <div class="loaderrelative" style="display: none">
        <div class="spinner"></div>
    </div>
    <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%--<div id="divResources" >
        <div class="pippo">
            <span class="bind" placeholder="test 1" mybind="id"></span>
            <span class="bind" placeholder="test 2" mybind="Prezzo1"></span>
            <input class="bind" placeholder="test 3" mybind="id" />
            <img class="bind"  src="" mybind="id_allegati" />
        </div>
    </div>--%>
    
    <div id="divPortfolioList1"></div>
    <div id="divPortfolioList1Pager"></div>
        <asp:Literal Text="" ID="placeholderrisultati" runat="server" />


    <div id="divPlaceholderIsotopeList"></div>
    <div class="pull-left" id="divPagerPlaceholder">
    </div>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderIndextext" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentPlaceHolderJs" runat="Server">

    <%--  <script type="text/javascript">
        $(document).ready(function () {
            loadref(InitImmobiliJS,'','<%= Lingua %>');
        });</script>--%>
</asp:Content>



