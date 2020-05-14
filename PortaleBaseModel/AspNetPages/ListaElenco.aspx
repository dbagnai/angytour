<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="ListaElenco.aspx.cs" Inherits="AspNetPages_ListaElenco" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <script type="text/javascript">
        makeRevLower = false;
    </script>
    <div class="row">
        <div>
            <div class="container">
                <div>
                    <div class="col-md-12 col-sm-12">
                        <div class="content-box content-style3">
                            <%-- <div class="content-style3-icon fa fa-quote-right"></div>--%>
                            <div class="content-style3-title">
                                <h2 class="h1-body-title" style="color: #5c5c5c">
                                    <asp:Literal Text="" runat="server" ID="litNomePagina" /></h2>
                            </div>
                            <div class="content-style3-text">
                                <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
                            </div>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <%--<div class="row">
        <!--=== Breadcrumbs ===-->
        <div class="breadcrumbs">
            <div class="container">
                <h1 class="pull-left">
                    <%= TestoSezione(Tipologia) %></h1>
                <p></p>
                <ul class="pull-right breadcrumb">
                    <li><a href='<%# references.ResMan("Common", Lingua," LinkHome %>" runat="server">
                        <asp:Literal Text='<%# references.ResMan("Common", Lingua," testoHome %>" runat="server" /></a></li>
                    <li class="active"><%= TestoSezione(Tipologia) %> </li>
                </ul>
            </div>
        </div>
        <!--/breadcrumbs-->
        <!--=== End Breadcrumbs ===-->
    </div>--%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
    <div class="col-md-1 col-sm-1">
    </div>
    <div class="col-md-10 col-sm-10" id="divColCenter" runat="server">
        <%-- REPEATER PER ELENCO FILES APRIBILI --%>
        <div class="ui-216">
            <asp:Repeater ID="rptListFiles" runat="server" ViewStateMode="Enabled">
                <ItemTemplate>
                    <%--<div class="row" runat="server" id="divTitle"
                    style="font-size: 20px; color: #06558b; padding-bottom: 5px; margin-top: 15px; border-top: 1px solid #06558b" visible="false">
                    <asp:Literal Text="" ID="litTitle" runat="server" />
                </div>--%>
                    <!-- Item Block-->
                    <div class="row work-div" style="margin-right: 5px; margin-bottom: 5px">

                        <div class="col-md-3" runat="server" visible='<%# ControlloVisibilita(Eval("FotoCollection_M")) %>'>
                            <a target="_blank" href='<%#  WelcomeLibrary.UF.filemanage.ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString()) %>' runat="server">
                                <asp:Image ID="Anteprima" runat="server"
                                    ImageUrl='<%#  WelcomeLibrary.UF.filemanage.ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString()) %>'
                                    Visible='<%# ControlloVisibilita(Eval("FotoCollection_M")) %>'
                                    class="img-responsive hover-effect" /></a>
                        </div>
                        <div class="col-md-12 col-mob">
                            <!-- Item -->
                            <div class="ui-item">
                                <!-- Details -->
                                <div class="ui-details">
                                    <!-- Paragraph -->
                                    <h3 style="margin-bottom: 5px"><%# Eval("Denominazione" + Lingua).ToString()  %></h3>
                                    <asp:Literal ID="Literal2" Text="data " runat="server" /><asp:Literal ID="Literal3"
                                        Text='<%# string.Format("{0:dd/MM/yyyy}", Eval("DataInserimento"))  %>'
                                        runat="server" /></li>
                        <br />
                                    <br />
                                    <p>
                                        <asp:Literal ID="lblBrDesc" Text='<%#  ReplaceLinks(WelcomeLibrary.UF.Utility.SostituisciTestoACapo( ConteggioCaratteri(  Eval("Descrizione" + Lingua).ToString(),1000,true )) , false) %>'
                                            runat="server"></asp:Literal>
                                    </p>
                                    <%#  CrealistaFiles(Eval("Id"),  Eval("FotoCollection_M")) %>
                                </div>
                                <!-- User -->
                                <div class="ui-user clearfix" style="display: none">
                                    <!-- User Image -->
                                    <a href="#">
                                        <img runat="server" src="~/images/logo.png" alt="" /></a>
                                    <!-- User Name -->
                                    <span></span>
                                </div>
                            </div>


                        </div>
                    </div>
                    <!-- End Clients Block-->
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <%--REPEATER ELENCO FOTO TITOLO SOTTOTITOLO PER LINK VERSO ESTERNO--%>
        <asp:Repeater ID="rptList1" runat="server" ViewStateMode="Enabled">
            <ItemTemplate>
                <!-- Item Block-->
                <div class="row clients-page" style="margin-right: 40px; margin-bottom: 20px">
                    <div class="col-md-2">
                        <a target="_blank" href='<%# "http://" + Eval("Website").ToString()  %>' runat="server">
                            <asp:Image ID="Anteprima" runat="server"
                                ImageUrl='<%# WelcomeLibrary.UF.filemanage.ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString()) %>'
                                Visible='<%# ControlloVisibilita(Eval("FotoCollection_M")) %>'
                                class="img-responsive hover-effect" /></a>
                    </div>
                    <div class="col-md-10">
                        <h3><a target="_blank" href='<%# "http://" + Eval("Website").ToString()  %>' runat="server"><%# Eval("Denominazione" + Lingua).ToString()  %></a></h3>
                        <ul class="list-inline">
                            <%--<li><i class="fa fa-map-marker color-green"></i> USA</li>--%>
                            <li><i class="fa fa-globe color-green"></i><a target="_blank" class="linked" href='<%# "http://" + Eval("Website").ToString()%>'>'<%# Eval("Website").ToString()%>'</a></li>
                            <%-- <li><i class="fa fa-briefcase color-green"></i> Web Design &amp; Development</li>--%>
                        </ul>
                        <p>
                            <asp:Literal ID="lblBrDesc" Text='<%#  ReplaceLinks(WelcomeLibrary.UF.Utility.SostituisciTestoACapo( ConteggioCaratteri ( Eval("Descrizione" + Lingua).ToString(), 300,true)) , false) %>'
                                runat="server"></asp:Literal>
                        </p>
                    </div>
                </div>
                <!-- End ITEM Block-->
            </ItemTemplate>
        </asp:Repeater>

        <%--REPEATER PER SOCI--%>
        <div class="row" style="background-color: #e6e6e6; padding: 1%; border: 1px solid white;" runat="server" id="divRicercaSoci" visible="false">
            <div class="col-sm-12">

                <div class="pull-left" style="margin-right: 1%">
                    <strong>Nome</strong><br />
                    <asp:TextBox runat="server" Width="100%" ID="txtinputCerca" />
                </div>
                <div class="pull-left" style="display: none">
                    <strong>Paese</strong>
                    <br />
                    <asp:DropDownList runat="server" Width="100%" ID="ddlNazioneRicerca" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlNazioneRicerca_OnSelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="margin-right: 1%">
                    <strong>Regione</strong>
                    <br />
                    <asp:DropDownList runat="server" Width="100%" ID="ddlRegioneRicerca" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlRegioneRicerca_OnSelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList><br />
                    <div style="display: none">
                        <input id="txtReRicerca" style="width: 100%" runat="server" value="" />
                        <br />
                    </div>
                </div>
                <div class="pull-left" style="margin-right: 1%">
                    <strong>Provincia</strong>
                    <br />
                    <asp:DropDownList runat="server" Width="100%" ID="ddlProvinciaRicerca" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProvinciaRicerca_OnSelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList><br />
                    <div style="display: none">
                        <input id="txtPrRicerca" style="width: 100%" runat="server" value="" />
                    </div>
                </div>
                <div class="pull-left" style="display: none; margin-right: 1%">
                    <br />
                    <strong>Comune</strong>
                    <br />
                    <asp:DropDownList runat="server" Width="100%" ID="ddlComuneRicerca" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlComuneRicerca_OnSelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList><br />
                    <div style="display: none">
                        <input id="txtCoRicerca" style="width: 100%" runat="server" value="" />
                        <br />
                    </div>
                </div>
                <div class="pull-right">
                    <asp:Button Text="Cerca" ID="btnCerca" runat="server" CssClass="btn btn-success" OnClick="btnCerca_Click" />
                </div>
            </div>

        </div>
     <asp:Repeater ID="rptSoci" runat="server" ViewStateMode="Enabled" OnItemDataBound="rptSoci_ItemDataBound">
            <ItemTemplate>
                <div class="row" runat="server" id="divTitle"
                    style="font-size: 20px; color: #06558b; padding-bottom: 5px; margin-top: 25px; margin-bottom: 20px; border-bottom: 1px solid #06558b" visible="false">
                    <asp:Literal Text="" ID="litRegione" runat="server" />
                </div>
                <div class="row" style="border-bottom: 1px solid #ccc; margin-bottom: 20px; padding-bottom: 5px">
                    <div class="col-lg-4">
                        <%--    <h5 style="text-transform: capitalize">
                            <a id="a1" runat="server"
                                href='<%# WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes( Lingua,Eval("Denominazione" + Lingua).ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                <asp:Literal ID="litTitolo" Text='<%# Eval("Cognome_dts").ToString() + " "  + Eval("Nome_dts").ToString()  %>'
                                    runat="server"></asp:Literal></a>
                        </h5>--%>
                        <h5 style="text-transform: capitalize; margin-bottom: 0px">
                            <a id="a4" runat="server"
                                href='<%# WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua,Eval("Denominazione" + Lingua).ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                <asp:Literal ID="Literal1" Text='<%# Eval("Denominazione" + Lingua).ToString()  %>'
                                    runat="server"></asp:Literal></a>
                        </h5>
                    </div>
                    <div class="col-lg-6">
                        <a id="a2" runat="server"
                            href='<%#  WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua,Eval("Denominazione" + Lingua).ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                            target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                            <div style="overflow: hidden; max-height: 100px; display: block; text-align: justify; font-size: 13px;">
                                <asp:Literal ID="litPosizione" runat="server"
                                    Text='<%# VisualizzaPosizione( Eval("CodiceRegione").ToString()  ,Container.DataItem ) %>'></asp:Literal>
                            </div>
                        </a>

                    </div>
                    <div class="col-lg-2">
                        <a id="a3" runat="server" class="buttonstyle" style="padding-top: 5px; padding-bottom: 5px"
                            href='<%#  WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua,Eval("Denominazione" + Lingua).ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                            target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                            <div style="color: #fff; font-size: 13px; font-weight: bold">
                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"TestoVediScheda") %>' runat="server" />
                            </div>
                        </a>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <%--REPEATER LISTA ELENCO SENZA FOTO CON DIVISIONE PER REGIONE--%>
        <asp:Repeater ID="rptList" runat="server" ViewStateMode="Enabled" OnItemDataBound="rptList_ItemDataBound">
            <ItemTemplate>
                <div class="row" runat="server" id="divTitle"
                    style="font-size: 1.3em; color: #af1016; padding-bottom: 5px; margin-top: 15px; padding-top: 10px; border-top: 1px solid #af1016" visible="false">
                    <asp:Literal Text="" ID="litRegione" runat="server" />
                </div>
                <div class="row">
                    <div class="col-lg-6">
                        <h5>
                            <a id="a1" runat="server"
                                href='<%# WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes( Lingua,Eval("Denominazione" + Lingua).ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                <asp:Literal ID="litTitolo" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(  Eval("Denominazione" + Lingua).ToString() ) %>'
                                    runat="server"></asp:Literal></a>
                        </h5>
                    </div>
                    <div class="col-lg-4">
                        <a id="a2" runat="server" href='<%# WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes( Lingua,Eval("Denominazione" + Lingua).ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                            target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                            <div style="overflow: hidden; max-height: 100px; display: block; text-align: justify; font-size: 1.1em;">
                                <asp:Literal ID="litPosizione" runat="server"
                                    Text='<%# ControlloVuotoPosizione( Eval("CodiceComune").ToString()  , Eval("CodiceProvincia").ToString(), Eval("CodiceTipologia").ToString(),Lingua ) %>'></asp:Literal>
                            </div>
                        </a>

                    </div>
                    <div class="col-lg-2">
                        <a id="a3" runat="server" href='<%# WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes( Lingua,Eval("Denominazione" + Lingua).ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                            target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                            <div style="color: #af1016; font-size: 1.2em; font-weight: 700">
                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"TestoVediScheda") %>' runat="server" />
                            </div>
                        </a>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <hr />
        <asp:Panel runat="server" ID="pnlPager" Style="padding: 0px">
            <div class="row">
                <div class="col-sm-3">
                    <asp:Button ID="btnNext" class="button btn-flat" Text='<%# references.ResMan("Common", Lingua,"txtTastoNext") %>'
                        runat="server" OnClick="btnNext_click" />
                </div>
                <div class="col-sm-6">

                    <div id="pager" class="text-center">
                        <UC:PagerEx ID="PagerRisultati" runat="server" NavigateUrl="" PageSize="15" CurrentPage="1"
                            TotalRecords="20" dimensioneGruppo="5" nGruppoPagine="1" OnPageCommand="PagerRisultati_PageCommand"
                            OnPageGroupClickNext="PagerRisultati_PageGroupClickNext" OnPageGroupClickPrev="PagerRisultati_PageGroupClickPrev" />
                    </div>

                </div>
                <div class="col-sm-3">
                    <asp:Button ID="btnPrev" class="button btn-flat" Text='<%# references.ResMan("Common", Lingua,"txtTastoPrev") %>'
                        runat="server" OnClick="btnPrev_click" />
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="col-md-3 col-sm-3" id="divColright" runat="server" visible="false">
        <!-- Sidebar Block -->
        <div class="sidebar-block" runat="server" id="divContact" visible="true">
            <div class="sidebar-content">
                <!-- UI Content -->
                <div class="ui-15">
                    <div class="ui-content">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12 col-sm-12 ui-padd">
                                    <!-- Ui Form -->
                                    <div class="ui-form">
                                        <!-- Heading -->
                                        <h3 class="h3-sidebar-title sidebar-title">
                                            <%= references.ResMan("Common", Lingua,"Testotestimonianze") %>
                                        </h3>
                                        <!-- Form -->
                                        <!-- UI Input -->
                                        <div class="ui-input">
                                            <!-- Input Box -->
                                            <input class="form-control" type="text" name="uname" validationgroup="contatti" placeholder="Nome" runat="server" id="txtContactName" />
                                            <label class="ui-icon"><i class="fa fa-user"></i></label>
                                        </div>
                                        <div class="ui-input">
                                            <input class="form-control" type="text" name="unname" validationgroup="contatti" placeholder="Cognome" runat="server" id="txtContactCognome" />
                                            <label class="ui-icon"><i class="fa fa-user"></i></label>
                                        </div>
                                        <div class="ui-input">
                                            <input type="text" class="form-control" name="unname" validationgroup="contatti" placeholder="Email" runat="server" id="txtContactEmail" />
                                            <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                        </div>
                                        <div class="ui-input">
                                            <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contatti" placeholder="Messaggio .." runat="server" id="txtContactMessage" />
                                        </div>

                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkContactPrivacy" runat="server" Checked="false" />
                                                <span class="cr"><i class="cr-icon fa fa-check"></i></span>
                                                <%= references.ResMan("Common", Lingua,"chkprivacy") %><a target="_blank" href="<%=CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua,"linkPrivacypolicy")) %>"> (<%= references.ResMan("Common", Lingua,"testoprivacyperlink") %>) </a>
                                            </label>
                                        </div>
                                      

                                        <button id="btnInvia" type="button" class="divbuttonstyle" style="width: 200px" runat="server" validationgroup="contatti" onclick="ConfirmContactValue(this);"><%=  references.ResMan("Common", Lingua,"TestoInvio")  %> </button>
                                        <asp:Button ID="btnInviaSrv" Style="display: none" runat="server" OnClick="btnContatti1_Click" />
                                        <%--    <div class="g-recaptcha" id="rcaptcha" data-sitekey="6LccbRMUAAAAAAN14HC8RFxwNMaqdGvJFPQEVinq"></div>--%>
                                        <style>
                                            .g-recaptcha {
                                                margin: 15px auto !important;
                                                width: auto !important;
                                                height: auto !important;
                                                text-align: -webkit-center;
                                                text-align: -moz-center;
                                                text-align: -o-center;
                                                text-align: -ms-center;
                                            }

                                            #recaptcharesponse {
                                                font-size: 1.5rem;
                                                color: red;
                                            }
                                        </style>
                                        <script>
                                            function ConfirmContactValue(elembtn) {
                                                if ($('<%= "#" + chkContactPrivacy.ClientID %>')[0].checked == false) {
                                                                $("#recaptcharesponse").html('<%= references.ResMan("Common", Lingua,"txtPrivacyError") %>');
                                                                return false;
                                                            }
                                                            else {
                                                                $("#recaptcharesponse").html('');
                                                            }
                                                            <%--  var response = grecaptcha.getResponse();
                                                            if (response.length == 0)  //reCaptcha not verified
                                                            {
                                                                $("#recaptcharesponse").html('<%= references.ResMan("Common", Lingua,"testoCaptcha") %>');
                                                                return false;
                                                            }
                                                            else {--%>
                                                            if (Page_ClientValidate("contatti")) {
                                                                /*do work and go for postback*/
                                                                $(elembtn).attr("disabled", "")
                                                                $(elembtn).val("Wait ..");
                                                                var buttpost = document.getElementById("<%= btnInviaSrv.ClientID  %>");
                                                    buttpost.click();
                                                } else {
                                                    // alert('not validated');
                                                    return false;
                                                }
                                                //}
                                            }
                                        </script>



                                        <div id="recaptcharesponse"></div>
                                        <div style="font-weight: 300; font-size: 10px; color: red">
                                            <asp:Literal Text="" ID="outputContact" runat="server" />
                                        </div>
                                        <div style="clear: both"></div>

                                        <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' ValidationGroup="contatti" ControlToValidate="txtContactName" runat="server" />
                                        <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto16lErr") %>' ValidationGroup="contatti" ControlToValidate="txtContactCognome" runat="server" />
                                        <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' ValidationGroup="contatti" ControlToValidate="txtContactEmail" runat="server" />

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <!-- UI Content -->


            </div>
        </div>
    </div>
</asp:Content>
