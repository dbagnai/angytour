﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="OrdineOk.aspx.cs" Inherits="AspNetPages_OrdineOk" %>


<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <div class="container">
        <div class="row py-5">
            <div class="col-12">
                <img runat="server" src="~/images/carte1.png" alt="" /><br />
                <br />
                <%-- <img src="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif" align="left" style="margin-right: 7px;" />--%>
                <h4>
                    <asp:Label Style="font-size: 1.6em; color: black" Text='<%# references.ResMan("Common", Lingua,"defRispostaOrdine") %>' runat="server" ID="output" />
                </h4>
                <br />
                <asp:Panel runat="server" Visible="false" ID="pnlbtnretry">
                    <button type="button" style="float: right" class="btn btn-purple btn-small" onclick="javascript:window.location.assign('<%=ReplaceAbsoluteLinks(references.ResMan("Common",Lingua,"LinkOrderNoregistrazione"))  %>')"><%= references.ResMan("Common",Lingua,"txtOrdineRiprova") %></button>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>

