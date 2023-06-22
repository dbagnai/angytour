<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Associaruoli.aspx.cs" Inherits="admin_roles"  MasterPageFile="~/Admin/MasterPageAdmin.master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server" AllowCustomErrorsRedirect="True"
        AsyncPostBackErrorMessage="Errore generico. Contattare HelpDesk" AsyncPostBackTimeout="400"
        EnablePartialRendering="true" />
    <asp:UpdatePanel ID="UPDddlUser" runat="server" UpdateMode="always">
        <ContentTemplate>
            <div class="container">
            <div class="row">
             <div class="col-12">
                <div style="text-align:center">
                   <b>Lista Utenti/Ruoli:</b> 
                </div>
                <div style="margin-left:auto; margin-right:auto; width:350px;">
                    <asp:GridView ID="Usereroles" runat="server" CssClass="tblutenti-custom" AutoGenerateColumns="false" CellPadding="4"
                        ForeColor="#000000" GridLines="None" Width="185px">
                        <FooterStyle BackColor="#F7F6F3" Font-Bold="True" ForeColor="#8AB95D" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="Black" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle Font-Bold="True" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#888888" Font-Bold="True" ForeColor="#ffffff" />
                        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                            <asp:TemplateField HeaderText="Ruoli">
                                <ItemTemplate>
                                    <asp:BulletedList ID="RolesList" runat="server" DataSource='<%# Roles.GetRolesForUser(Eval("username").ToString()) %>'>
                                    </asp:BulletedList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                </div>
            </div>
           <div class="row">
            <div class="col-12"> 
                <div style="margin-left:auto;margin-right:auto;width:300px">
                    <div style="float:left; width:50%">
                   <b>Utente:</b> </div>
                <div style="float:left; width:50%">
                    <b>Ruolo</b> </div><br />
                <div style="float:left; width:50%">
                    <asp:DropDownList ID="UsersList" runat="server" Width="100px"/>   
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RolesList"
                        ErrorMessage="Seleziona Ruolo" /></div>
                <div style="float:left; width:50%">
                    <asp:ListBox ID="RolesList" runat="server" SelectionMode="Multiple" Height="60px"
                        Width="136px"></asp:ListBox> 
                    </div>
                    </div>
           </div>
                </div><br />
   <div class="row">
            <div class="col-12">
                <div style="text-align:center">
                   <b>Comandi:</b> </div>
                    <div style="margin-left:auto; margin-right:auto; width:350px;">
                       <div style="float:left; width:50%">
                    <asp:Button ID="Button1"  CssClass="btn" runat="server" Text="Associa a Ruolo"  BackColor="#888888" ForeColor="White" OnClick="Button1_Click" />
                           </div>
                          <div style="float:left; width:50%">
                    <asp:Button ID="Button2" CssClass="btn ml-1" runat="server" Text="Rimuovi da Ruolo" BackColor="#888888" ForeColor="White" OnClick="Button2_Click" />
                              </div>
                    <br />
                    <asp:Label ID="Results" runat="server"></asp:Label>
            </div>
                </div>
                </div> </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
    <asp:UpdateProgress ID="prg" AssociatedUpdatePanelID="UPDddlUser" runat="server"
        DisplayAfter="0">
        <ProgressTemplate>
            <div>
                Aggiornamento in corso ! Attendere
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>
