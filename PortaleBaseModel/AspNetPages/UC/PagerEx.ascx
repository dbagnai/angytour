<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PagerEx.ascx.cs" Inherits="UC_PagerEx"
    EnableTheming="true" %>
<ul class="pagination" id="ulPager" runat="server">
    <li id="liPrev" runat="server">
        <asp:LinkButton ID="prevBtn" runat="server" Text="&laquo;"
            OnCommand="prevBtn_Command" CommandArgument="" />
    </li>
    <asp:Repeater ID="RepeaterPages" runat="server">
        <ItemTemplate>
            <li runat="server" class='<%# Eval("CssClass") %>'>
                <asp:LinkButton ID="lbPage"  runat="server" Text='<%# Eval("PageNumber") %>'
                    OnCommand="lbPage_Command" CommandArgument='<%# Eval("PageNumber") %>'>
                </asp:LinkButton>
            </li>
        </ItemTemplate>
    </asp:Repeater>
    <li id="liNext" runat="server">
        <asp:LinkButton ID="nextBtn" runat="server" Text="&raquo;"
            OnCommand="nextBtn_Command" CommandArgument="" />
    </li>
</ul>
