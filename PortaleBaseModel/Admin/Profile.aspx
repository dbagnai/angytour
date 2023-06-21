<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="admin_Profile"  MasterPageFile="~/Admin/MasterPageAdmin.master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server" AllowCustomErrorsRedirect="True"
        AsyncPostBackErrorMessage="Errore generico. Contattare HelpDesk" AsyncPostBackTimeout="400"
        EnablePartialRendering="true" />
    <asp:UpdatePanel ID="UPDddlUser" runat="server" UpdateMode="always">
        <ContentTemplate>
       
            <br />
            <div class="container">
                <div class="row">
                    <div class="col-4" style="margin-left:auto; margin-right:auto; width:500px;">
            <br />
           <b>Utente da modificare:</b>  
            <br />
            <asp:DropDownList CssClass="form-control UserList" ID="UsersList" runat="server" BackColor="#ffffff" AutoPostBack="true" OnSelectedIndexChanged="UserList_changed" />
            &nbsp;<br />
            <br />
            <div>
                <b>Nome:</b><br />
                <asp:TextBox ID="FirstName"  CssClass="form-control"  BackColor="#ffffff" runat="server"></asp:TextBox>
                <b>Cognome:</b><br />
                <asp:TextBox ID="LastName"  CssClass="form-control" BackColor="#ffffff" runat="server"></asp:TextBox>
               <b>E-Mail:</b> <br />
                <asp:TextBox ID="EMail"  CssClass="form-control" BackColor="#ffffff" runat="server"></asp:TextBox>
               <b>Cellulare:</b> <br />
                <asp:TextBox ID="Cellulare"  CssClass="form-control" BackColor="#ffffff" runat="server"></asp:TextBox>
               <b>Id Cliente anagrafica:</b> <br />
                <asp:TextBox ID="txtIdcliente"  CssClass="form-control" BackColor="#ffffff" runat="server"></asp:TextBox>
              <b>Id Socio :</b> <br />
                <asp:TextBox ID="txtIdsocio"  CssClass="form-control" BackColor="#ffffff" runat="server"></asp:TextBox>

                  <b> prova :</b><br />
                <asp:TextBox ID="txtprova" BackColor="#ffffff" CssClass="form-control" runat="server"></asp:TextBox>


                <asp:Button ID="Button1" runat="server"  BackColor="#fff" Text="Registra" OnClick="Button1_Click"  CssClass="mt-1"/>
                <p>
                    <b>Ricordarsi che per memorizzare qualsiasi modifica è necessario salvare premendo
                    il tasto "Registra" dopo le variazioni!!!!</b>
                </p>
                   <asp:Button Text="Restart" ID="reset" OnClick="reset_Click" runat="server" BackColor="#fff"/>
            </div>
            <asp:Label ID="Results" runat="server"></asp:Label>
            <p>
                <br />
                ----------------------------------------------------------------------<br />
                <b>CAMBIO PASSWORD PER L'UTENTE:</b>
            </p>
            <b>Vecchia Password:</b><asp:TextBox ID="txtPasswordold" BackColor="#ffffff"  CssClass="form-control" runat="server"></asp:TextBox>
            <b>Nuova Password:</b><asp:TextBox ID="txtPasswordnew" BackColor="#ffffff" CssClass="form-control" runat="server"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="Cambio Password" OnClick="Button2_Click" BackColor="#fff" />
            <br />
            <asp:Label ID="lblquestion" runat="server" />
            <br />
            <asp:TextBox ID="txtanswer" BackColor="#ffffff"  CssClass="form-control" runat="server"></asp:TextBox>
            <asp:Button ID="Button3" runat="server" Text="Reset Password" Enabled="true" OnClick="Button3_Click" BackColor="#fff"/>
            <br />
            <asp:Label ID="lblResultsPsw" runat="server" />
                        </div>
                </div>
                </div>
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