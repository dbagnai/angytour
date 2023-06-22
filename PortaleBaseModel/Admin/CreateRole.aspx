<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateRole.aspx.cs" Inherits="admin_CreateRole" MasterPageFile="~/Admin/MasterPageAdmin.master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div class="container">
        <div class="row">
            <div class="col-12">
                <div style="margin-right: auto; margin-left: auto; width: 400px;">
                    <b>Lista Ruoli:</b><br />
                    <asp:ListBox ID="RolesList" runat="server" SelectionMode="Single" Width="300px" Height="100px"></asp:ListBox><br />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12 mt-2">
                <div style="margin-right: auto; margin-left: auto; width: 400px;">
                    <asp:Button ID="Button1" runat="server" Text="Crea Ruolo" BackColor="#888888" ForeColor="White" OnClick="Button1_Click" />
                    <b>Ruolo:</b>
                    <asp:TextBox ID="RoleName" runat="server" Width="160px"></asp:TextBox>
                    <br />
                    <asp:Label ID="Results" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12 mt-2">
                <div style="margin-right: auto; margin-left: auto; width: 400px;">
                    <asp:Button ID="Button2" runat="server" Text="Elimina Ruolo" BackColor="#888888" ForeColor="White" OnClick="Button2_Click" />
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
