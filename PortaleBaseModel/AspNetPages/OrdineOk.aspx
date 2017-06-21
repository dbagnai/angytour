<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="OrdineOk.aspx.cs" Inherits="AspNetPages_OrdineOk" %>


<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <img runat="server" src="~/images/carte1.png" alt="" />
       <%-- <img src="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif" align="left" style="margin-right: 7px;" />--%>
        <br />
        <br />
        <h4>
            <asp:Label Style="font-size: 1.6em; color: black" Text='<%# references.ResMan("Common", Lingua,"defRispostaOrdine") %>' runat="server" ID="output" />
        </h4>

                 
         <button type="button" style="float:right" class="btn btn-purple btn-small" onclick="javascript:window.location.assign('<%=ReplaceAbsoluteLinks(Resources.Common.LinkOrderNoregistrazione)  %>')"><%= Resources.Common.txtOrdineRiprova %></button>
    </div>

</asp:Content>

