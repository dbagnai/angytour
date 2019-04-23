<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" EnableViewState="false"
    CodeFile="weblist.aspx.cs" Inherits="AspNetPages_weblist" Title="" Culture="it-IT"
    MaintainScrollPositionOnPostback="false" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <script type="text/javascript">
        makeRevLower = true;
        //history.scrollRestoration = 'manual'; //Evito che il browser mi riporti automaticamente alla posizione verticale
    </script>
    <div class="container" id="divTitleContainer" runat="server" clientidmode="static" visible="false">
        <div class="row" runat="server" id="divTitle" style="text-align: center; margin-top: 10px">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-12 col-sm-12 col-12">
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

<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="searchblock" class="position-relative my-0 mx-auto" style="max-width: 1400px; padding-right: 15px; padding-left: 15px;">

        <div class="d-flex flex-row-reverse">
            <div class="col-12 col-sm-3 pl-0 pl-sm-2 pr-0" runat="server" id="divSearch" visible="false">
                <div class="sidebar-content tags blog-search ">
                    <div class="input-group flex-nowrap">
                        <%-- <input enableviewstate="true" class="form-control blog-search-input" name="q" type="text" placeholder='<%# references.ResMan("Common", Lingua,"TestoCercaBlog") %>' runat="server" id="inputCerca" /> --%>
                        <input class="form-control blog-search-input" onkeydown="if (event.keyCode == 13)  document.getElementById('btncerca1').click()" name="searchtext" id="searchtext" type="text" placeholder='<%# references.ResMan("Common", Lingua,"TestoCercaBlog") %>' />
                        <span class="input-group-addon">
                            <button type="button" onclick="cerca('searchtext')" id="btncerca1" class="bg-primary-color rounded-right blog-search-button fa fa-search" />
                            <%-- <button onserverclick="Cerca_Click" id="BtnCerca" class="blog-search-button fa fa-search" runat="server" clientidmode="Static" /> --%>
                        </span>
                    </div>
                </div>
            </div>
            <div class="pr-2 my-3" style="max-width: 350px;" runat="server" id="divArchivio" visible="false">
            </div>
        </div>
    </div>
    <div class="row">
        <div class="" runat="server" id="columnsingle">

            <asp:Literal Text="" ID="placeholderrisultatinocontainer" runat="server" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
    <div class="col-md-1 col-sm-1" runat="server" id="column1" visible="false">

        <asp:Literal Text="" ID="placeholderrisultati" runat="server" />
    </div>
    <div class="col-md-9 col-sm-9" runat="server" id="column2">
    </div>
    <div class="col-md-3 col-sm-3" runat="server" id="column3">
        <div class="sidebar" runat="server" id="RightSidebar">
            <!-- Sidebar Block -->
            <div style="max-width: 350px; margin: 10px auto" runat="server" id="divLinksrubriche" visible="false">
            </div>
        </div>
        <!-- Sidebar Block -->
        <asp:Literal Text="" ID="placeholderlateral" runat="server" />
        <asp:Literal Text="" ID="placeholderlateral1" runat="server" />
        <asp:Literal Text="" ID="placeholderlateral2" runat="server" />
        <asp:Literal Text="" ID="placeholderlateral3" runat="server" />
    </div>
</asp:Content>
