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
            <li><a href='<%# references.ResMan("Common",Lingua,"LinkHome") %>' runat="server">
                <asp:Literal Text='<%# references.ResMan("Common",Lingua,"testoHome") %>' runat="server" /></a></li>
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
    <div class="container" style="min-height: 50vh">
        <div class="row align-items-center">
            <div class="col"></div>
            <div class="col align-self-center text-center">
                <label class="alert-danger alert-block">
                    <asp:Label Text="" ID="output" runat="server" />
                </label>
                <div style="padding: 50px;">
                    <h3>
                        <asp:Literal Text='<%# references.ResMan("Common",Lingua,"testoLoginReturn") %>' runat="server" /></h3>
                    <hr />
                </div>
            </div>
            <div class="col"></div>
        </div>
        <div class="row  align-items-start">
            <div class="col align-self-center">
                <asp:LoginView ID="LogView1" runat="server">
                    <AnonymousTemplate>
                        <div class="row  justify-content-center mb-3">
                            <div class="col-sm-3">
                                <div class="input-group flex-nowrap">
                                    <div class="input-group-prepend">
                                        <asp:Label CssClass="input-group-text" Text='<%# references.ResMan("Common",Lingua,"testoLoginUtente") %>' runat="server" />
                                    </div>
                                    <input type="text" class="form-control" id="inputName" runat="server" name="inputName" placeholder="Username">
                                </div>
                            </div>
                        </div>
                        <div class="row  justify-content-center mb-3">
                            <div class="col-sm-3">
                                <div class="input-group flex-nowrap">
                                    <div class="input-group-prepend">
                                        <asp:Label CssClass="input-group-text" runat="server">Password</asp:Label>
                                    </div>
                                    <%-- <a runat="server" onserverclick="btnForget_Click" class="pull-right"><%= references.ResMan("Common",Lingua,"testoLoginForget") %></a>--%>
                                    <input type="password" placeholder="Password" class="form-control" runat="server" id="inputPassword">
                                </div>
                            </div>
                        </div>
                        <div class="row  justify-content-center mb-3">
                            <div class="col-sm-3">
                                <div class="input-group flex-nowrap">
                                    <%--<div class="checkbox">
                                                <label>
                                                    <input type="checkbox">Remember Me 
                                                </label>
                                            </div>--%>
                                </div>
                            </div>
                        </div>
                        <div class="row  justify-content-center mb-3">
                            <div class="col-sm-3 text-align-right">
                                <div class="input-group flex-nowrap">
                                    <asp:Button OnClick="loginbtn_Click" class="btn btn-small btn-primary pull-right"
                                        Text='<%# references.ResMan("Common",Lingua,"testoLoginAccedi") %>' runat="server" /><br />
                                    <label class="alert-danger alert-block">
                                        <asp:Label Font-Size="Medium" ID="outputlogin" runat="server" Text=""></asp:Label>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </AnonymousTemplate>
                    <RoleGroups>
                        <asp:RoleGroup Roles="WebMaster">
                            <ContentTemplate>
                                <script>
                                    console.log("callin clear cache");
                                    $(document).ready(function () {
                                        manageclientstorage("clear");
                                    });
                                </script>
                                <div class="row  justify-content-center">
                                    <div class="col-sm-3">
                                        <a runat="server" class="btn btn-block btn-primary" alt="" href="~/Admin/default.aspx?Lingua=I" id="lnkAdminPage">AmministrazioneSito</a>
                                        <br />
                                        <br />
                                        <a runat="server" class="btn btn-block btn-primary" alt="" href="~/AreaContenuti/default.aspx?Lingua=I" id="A1">Gestione Contenuti</a>
                                        <%-- <br />
                                <a runat="server" alt="" class="btn btn-default btn-small" href="~/AreaContenuti/StoricoOrdini.aspx?Lingua=I" id="A2">Area
                                            Storico Ordini</a><br />--%>
                                        <%--  <br />
                                <a  class="btn btn-block btn-primary" alt="" href='<%= ReplaceAbsoluteLinks(references.ResMan("Common",Lingua,"LinkForum")) %>' id="A2">Vai al FORUM SOCI</a><br />
                                        --%>
                                        <br />
                                        <br />
                                        <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText="Disconnetti Utente "
                                            LoginText="" LogoutPageUrl="~/login.aspx?clear=true" LogoutAction="Redirect" />
                                        <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />

                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="Socio">
                            <ContentTemplate>
                                <script>
                                    console.log("callin clear cache");
                                    $(document).ready(function () {
                                        manageclientstorage("clear");
                                    });
                                </script>
                                <div class="row  justify-content-center">
                                    <div class="col-sm-3">
                                        <%-- <a  class="btn btn-block btn-primary" alt="" href="<%= ReplaceAbsoluteLinks(references.ResMan("Common",Lingua,"LinkForum) %>"" id="A2">Vai al FORUM SOCI</a><br />
                                <br />--%>
                                        <%--  <br />
                                <a runat="server" class="btn btn-block btn-primary" alt="" href='<%# references.ResMan("Common",Lingua,"LinkSchedaSocio") %>' id="lnkOrdrpage">
                                    <asp:Literal Text='<%# references.ResMan("Common",Lingua,"TestoSchedaSocio %>" runat="server" />
                                </a>
                                <br />--%>

                                        <br />
                                        <br />
                                        <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText='<%# references.ResMan("Common",Lingua,"testoLoginDisconnetti") %>'
                                            LoginText="" LogoutPageUrl="~/login.aspx?clear=true" LogoutAction="Redirect" />
                                        <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="Operatore">
                            <ContentTemplate>
                                <script>
                                    console.log("callin clear cache");
                                   $(document).ready(function () {
                                        manageclientstorage("clear");
                                    });
                                </script>
                                <div class="row  justify-content-center">
                                    <div class="col-sm-3">
                                        <%--  <a runat="server" alt="" href="~/AreaRiservata/default.aspx" id="lnkAdminPage">Home
                            Area Riservata</a><br /><br />--%>
                                        <a runat="server" class="btn btn-block btn-primary" alt="" href='<%# references.ResMan("Common",Lingua,"LinkOrder") %>' id="lnkOrdrpage">
                                            <asp:Literal Text='<%# references.ResMan("Common",Lingua,"TestoProcediOrdine") %>' runat="server" />
                                        </a>
                                        <br />
                                        <br />
                                        <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText='<%# references.ResMan("Common",Lingua,"testoLoginDisconnetti") %>'
                                            LoginText="" LogoutPageUrl="~/login.aspx?clear=true" LogoutAction="Redirect" />
                                        <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="Autore">
                            <ContentTemplate>
                                <script>
                                    console.log("callin clear cache");
                                   $(document).ready(function () {
                                        manageclientstorage("clear");
                                    });
                                </script>
                                <div class="row  justify-content-center">
                                    <div class="col-sm-3">
                                        <a runat="server" alt="" class="btn btn-block btn-primary" href="~/AreaContenuti/GestioneOfferte.aspx?CodiceTipologia=rif000002&Lingua=I" id="lnkAdminPage">Area Personale</a><br />
                                        <%--  <a runat="server" alt="" class="btn btn-block btn-primary" href="~/AreaContenuti/StoricoOrdini.aspx?Lingua=I" id="A2">Area Storico Ordini</a><br /> --%>
                                        <%-- <a  class="btn btn-block btn-primary" alt="" href='<%= ReplaceAbsoluteLinks(references.ResMan("Common",Lingua,"LinkForum")) %>' id="A2">Vai al FORUM SOCI</a><br />
                                <br />--%>
                                        <br />
                                        <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText="Disconnetti Utente "
                                            LoginText="" LogoutPageUrl="~/login.aspx?clear=true" LogoutAction="Redirect" />
                                        <br />
                                        <br />
                                        <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="GestorePortale">
                            <ContentTemplate>
                                <script>
                                    console.log("callin clear cache");
                                   $(document).ready(function () {
                                        manageclientstorage("clear");
                                    });
                                </script>
                                <div class="row  justify-content-center">
                                    <div class="col-sm-3">
                                        <a runat="server" alt="" class="btn btn-block btn-primary" href="~/AreaContenuti/default.aspx?Lingua=I" id="lnkAdminPage">Area Riservata Personale</a>
                                        <%--  <a runat="server" alt="" class="btn btn-block btn-primary" href="~/AreaContenuti/StoricoOrdini.aspx?Lingua=I" id="A2">Area
                                            Storico Ordini</a><br />--%>
                                        <%-- <a  class="btn btn-block btn-primary" alt="" href='<%= ReplaceAbsoluteLinks(references.ResMan("Common",Lingua,"LinkForum")) %>' id="A2">Vai al FORUM SOCI</a><br />
                                <br />--%>
                                        <br />
                                        <br />
                                        <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText="Disconnetti Utente "
                                            LoginText="" LogoutPageUrl="~/login.aspx?clear=true" LogoutAction="Redirect" />
                                        <br />
                                        <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:RoleGroup>
                        <asp:RoleGroup Roles="Commerciale">
                            <ContentTemplate>
                                <script>
                                    console.log("callin clear cache");
                                   $(document).ready(function () {
                                        manageclientstorage("clear");
                                    });
                                </script>
                                <div class="row  justify-content-center">
                                    <div class="col-sm-3">
                                        <%-- <a runat="server" alt="" class="btn btn-block btn-primary" href="~/AreaContenuti/default.aspx?Lingua=I" id="lnkAdminPage">Area Riservata Personale</a><br />--%>
                                        <br />
                                        <a runat="server" alt="" class="btn btn-block btn-primary" href="~/AreaContenuti/StoricoOrdini.aspx?Lingua=I" id="A2">Area
                                            Storico Ordini</a><br />
                                        <br />
                                        <%-- <a  class="btn btn-block btn-primary" alt="" href='<%= ReplaceAbsoluteLinks(references.ResMan("Common",Lingua,"LinkForum")) %>' id="A2">Vai al FORUM SOCI</a><br />
                                <br />--%>
                                        <br />
                                        <br />
                                        <asp:LoginStatus class="btn btn-block btn-primary" ID="LoginStatus1" runat="server" LogoutText="Disconnetti Utente "
                                            LoginText="" LogoutPageUrl="~/login.aspx?clear=true" LogoutAction="Redirect" />
                                        <br />
                                        <asp:LoginName ID="logName" runat="server" ForeColor="GrayText" Height="10px" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:RoleGroup>
                    </RoleGroups>
                </asp:LoginView>
                <div runat="server" visible="false">
                    <div class="row  justify-content-center">
                        <div class="col-sm-3">
                            <div class="input-group">
                                <h3>
                                    <asp:Literal ID="Literal11" runat="server" Text='<%# references.ResMan("Common",Lingua,"testoLoginNew") %>' /></h3>
                                <hr>
                                <asp:Label CssClass="input-group-text" ID="Literal1" runat="server" Text='<%# references.ResMan("Common",Lingua,"testoLoginNewExplain") %>' />
                                <a href='<%# references.ResMan("Common",Lingua,"LinkIscriviti") %>' class="btn btn-block btn-primary"
                                    runat="server" id="btnIscriviti">
                                    <asp:Literal ID="Literal2" runat="server" Text='<%# references.ResMan("Common",Lingua,"testoIscrizione") %>' /></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
