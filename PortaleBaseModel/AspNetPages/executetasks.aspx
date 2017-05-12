<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="executetasks.aspx.cs" Inherits="_executetasks" Title=""
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <section class="section">
            <div class="row">
                <asp:Literal ID="litMainContent" runat="server"></asp:Literal>
            </div>
        </section>
    </div>
</asp:Content>
