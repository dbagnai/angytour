<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" MasterPageFile="~/Admin/MasterPageAdmin.master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div class="container">
     <div class="row">
         <div class="col-12" >
             <h1 style="text-align:center;">Lista Utenti e Ruoli:</h1><br />
             <div style="margin: 0px auto;width:60%">
    <asp:GridView ID="UsersList" runat="server" CellPadding="4" ForeColor="Black" GridLines="None" CssClass="table-responsive"
        AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="UsersList_PageIndexChanging">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#fff" ForeColor="Black" HorizontalAlign="Center"  Font-Names="Arial" />
        <HeaderStyle BackColor="#fff" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
            <asp:TemplateField HeaderText="Nome e Cognome">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getnome(Eval("Username").ToString()) %>' ID="NomeCognome" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="E-Mail">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getEmail(Eval("Username").ToString()) %>' ID="EMail" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ruoli">
                <ItemTemplate>
                    <asp:BulletedList runat="server" DataSource='<%# Roles.GetRolesForUser(Eval("Username").ToString()) %>'
                        ID="RolesList" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="OnLine">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# isonline(Eval("Username").ToString()) %>' ID="Online" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

             </div>
         </div>
                </div>
         </div>
    


</asp:Content>
