<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="forum-detail.aspx.cs"
    Inherits="AreaRiservata_Forumdetail" Title="Pagina senza titolo" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">

    <div class="row">
        <div class="section-content top-body">
            <div class="container">
                <div class="col-md-3 col-sm-3">
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="content-box content-style3">
                        <%--<div class="content-style3-icon fa fa-quote-right"></div>--%>
                        <div class="content-style3-title">
                            <h2 class="h1-body-title" style="color: #5c5c5c">
                                <asp:Literal Text="" runat="server" ID="litNomePagina" />
                            </h2>
                        </div>
                        <div class="content-style3-text">
                            <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                </div>
            </div>
        </div>
    </div>
    <div style="background-color: rgba(0, 0, 0, 0.1);">
        <div class="container">
            Dettaglio
        <br />
            <br />
            <br />

        </div>
    </div>
</asp:Content>



