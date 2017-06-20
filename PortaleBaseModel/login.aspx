<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <%-- <div class="row">
        <div class="section-content top-body">
            <div class="container">
                Prova
                            <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </div>
        </div>
      
    </div>--%><%--
<!--=== Breadcrumbs ===-->
<div class="breadcrumbs">
    <div class="container">
        <h1 class="pull-left">Login</h1>
        <p></p>
        <ul class="pull-right breadcrumb">
            <li><a href="<%$ Resources:Common, LinkHome %>" runat="server">
                <asp:Literal Text="<%$ Resources:Common, testoHome %>" runat="server" /></a></li>
            <li class="active">da creare link pagina</li>
        </ul>
    </div>
</div>
<!--/breadcrumbs-->
<!--=== End Breadcrumbs ===-->
    --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        makeRevLower = false;
    </script>

    <div class="row" style="min-height: 500px">

        <div class="col-md-3"></div>
        <div class="col-md-6" style="text-align: center">
            <label class="alert-danger alert-block">
                <asp:Label Text="" ID="output" runat="server" />
            </label>
            <div style="padding: 50px;">
                <h3>
                    <asp:Literal Text="<%$ Resources : Common, testoLoginReturn %>" runat="server" /></h3>
                <hr />
                <asp:LoginView ID="LogView1" runat="server">
                    <AnonymousTemplate>

                        <div class="classic-form">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="email" class="col-sm-3 control-label">
                                        <asp:Literal Text="<%$ Resources : Common, testoLoginUtente %>" runat="server" /></label>
                                    <div class="col-sm-9">
                                        <input type="text" class="form-control" id="inputName" runat="server" name="inputName" placeholder="Username">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="password" class="col-sm-3 control-label">Password</label>
                                    <div class="col-sm-9">
                                       <%-- <a runat="server" onserverclick="btnForget_Click" class="pull-right"><%= Resources.Common.testoLoginForget %></a>--%>
                                        <input type="password" placeholder="Password" class="form-control" runat="server" id="inputPassword">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-3 col-sm-9">
                                        <%--<div class="checkbox">
                                                <label>
                                                    <input type="checkbox">Remember Me 
                                                </label>
                                            </div>--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-3 col-sm-9">
                                        <asp:Button OnClick="loginbtn_Click" class="btn btn-small btn-primary pull-right"
                                            Text="<%$ Resources : Common, testoLoginAccedi %>" runat="server" />
                                        <br />
                                        <label class="alert-danger alert-block">
                                            <asp:Label Font-Size="Medium" ID="outputlogin" runat="server" Text=""></asp:Label>
                                        </label>

                                    </div>
                                </div>

                            </div>
                        </div>

                    </AnonymousTemplate>
                    <RoleGroups>
                        <asp:RoleGroup Roles="WebMaster">
                            <ContentTemplate>

                                <a runat="server" class="btn btn-block btn-primary" alt="" href="~/Admin/default.aspx?Lingua=I" id="lnkAdminPage">AmministrazioneSito</a>
                                <br />
                                <br />
                                <a runat="server" class="btn btn-block btn-primary" alt="" href="~/AreaContenuti/default.aspx?Lingua=I" id="A1">Area Gestione Contenuti</a><br />
                                <%-- <br />
                                <a runat="server" alt="" class="btn btn-default btn-small" href="~/AreaContenuti/StoricoOrdini.aspx?Lingua=I" id="A2">Area
                                            Storico Ordini</a><br />--%>
                              <%--  <br />
                                <a  class="btn btn-block btn-primary" alt="" href="<%= ReplaceAbsoluteLinks(Resources.Common.LinkForum) %>"" id="A2">Vai al FORUM SOCI</a><br />
                             --%>   <br />
                                <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText="Disconnetti Utente "
                                    LoginText="" />
                                <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                            </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="Socio">
                            <ContentTemplate>
                                <%-- <a  class="btn btn-block btn-primary" alt="" href="<%= ReplaceAbsoluteLinks(Resources.Common.LinkForum) %>"" id="A2">Vai al FORUM SOCI</a><br />
                                <br />--%>
                              <%--  <br />
                                <a runat="server" class="btn btn-block btn-primary" alt="" href="<%$ Resources : Common, LinkSchedaSocio %>" id="lnkOrdrpage">
                                    <asp:Literal Text="<%$ Resources : Common, TestoSchedaSocio %>" runat="server" />
                                </a>
                                <br />--%>

                                <br />
                                <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText="<%$ Resources : Common, testoLoginDisconnetti %>"
                                    LoginText="" />
                                <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                            </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="Operatore">
                            <ContentTemplate>
                                <%--  <a runat="server" alt="" href="~/AreaRiservata/default.aspx" id="lnkAdminPage">Home
                            Area Riservata</a><br /><br />--%>
                                <a runat="server" class="btn btn-block btn-primary" alt="" href="<%$ Resources : Common, LinkOrder %>" id="lnkOrdrpage">
                                    <asp:Literal Text="<%$ Resources : Common, TestoProcediOrdine %>" runat="server" />
                                </a>
                                <br />
                                <br />
                                <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText="<%$ Resources : Common, testoLoginDisconnetti %>"
                                    LoginText="" />
                                <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                            </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="GestorePortale">
                            <ContentTemplate>
                                <a runat="server" alt="" class="btn btn-block btn-primary" href="~/AreaContenuti/default.aspx?Lingua=I" id="lnkAdminPage">Area
                                            Gestione Contenuti</a><br />
                                <br />
                                <br />
                                <%-- <a runat="server" alt="" class="btn btn-block btn-primary" href="~/AreaContenuti/StoricoOrdini.aspx?Lingua=I" id="A2">Area
                                            Storico Ordini</a><br />
                                <br />--%>
                                <%-- <a  class="btn btn-block btn-primary" alt="" href="<%= ReplaceAbsoluteLinks(Resources.Common.LinkForum) %>"" id="A2">Vai al FORUM SOCI</a><br />
                                <br />--%>
                                <br />
                                <br />
                                <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText="Disconnetti Utente "
                                    LoginText="" />
                                <br />
                                <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                            </ContentTemplate>
                        </asp:RoleGroup>
                    </RoleGroups>
                </asp:LoginView>

            </div>
        </div>
        <div class="col-md-3">
            <%--   <div class="classic-form" runat="server" visible="false">
                <div class="form-horizontal">
                    <div class="form-group">
                        <h3>
                            <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:Common, testoLoginNew %>" /></h3>
                        <hr>
                        <div class="form-group">
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Common, testoLoginNewExplain %>" />
                        </div>
                        <div class="form-group">

                            <a href="<%$ Resources:Common,LinkIscrivitisocio %>" class="btn btn-block btn-primary"
                                runat="server" id="btnIscriviti">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Common, testoLoginNewCreateSocio %>" /></a>
                        </div>
                    </div>
                </div>--%>
        </div>
    </div>
    <!--end:.row-->
</asp:Content>
