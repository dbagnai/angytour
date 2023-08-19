<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" EnableViewState="false"
    CodeFile="webdetail.aspx.cs"
    EnableTheming="true" Culture="it-IT" Inherits="_webdetail" Title="" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <div id="divTitleContainer" style="display: none">
        <div class="row">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-10 col-sm-10 col-12">
                <asp:Literal Text="" runat="server" ID="litSezione" />
            </div>
            <div class="col-md-1 col-sm-1">
            </div>
        </div>
    </div>
    <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
    <script type="text/javascript">
        function goBack() {
            window.history.back()
        }
        $(document).ready(function () {
        });
    </script>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Literal Text="" ID="placeholderrisultatinocontainer" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">

    <div class="row justify-content-center" id="divSezioneSchedaContenuto">
        <div class="col-12" runat="server" id="column1" visible="false">
        </div>
        <div class="col-md-9 col-sm-9" runat="server" id="column2">
            <div class="clearfix" style="margin: 0px">
                <div class="float-right" style="max-width: 350px; margin: 10px auto" runat="server" id="divSearch" visible="false">
                    <div class="sidebar-content tags blog-search ">
                        <div class="input-group flex-nowrap">
                            <%-- <input enableviewstate="true" class="form-control blog-search-input" name="q" type="text" placeholder='<%# references.ResMan("Common", Lingua,"TestoCercaBlog") %>' runat="server" id="inputCerca" /> --%>
                            <input class="form-control blog-search-input" onkeydown="if (event.keyCode == 13)  document.getElementById('btncerca1').click()" name="searchtext" id="searchtext" type="text" placeholder='<%# references.ResMan("Common", Lingua,"TestoCercaBlog") %>' />
                            <span class="input-group-addon">
                                <button type="button" onclick="cerca('searchtext')" id="btncerca1" class="blog-search-button fa fa-search" />
                                <%-- <button onserverclick="Cerca_Click" id="BtnCerca" class="blog-search-button fa fa-search" runat="server" clientidmode="Static" /> --%>
                                
                            </span>
                        </div>

                    </div>
                </div>
                <div class="float-right pr-2" style="max-width: 350px; margin: 10px auto" runat="server" id="divArchivio" visible="false">
                </div>
            </div>
            <div class="row">
                <asp:Label ID="output" runat="server"></asp:Label>
            </div>
            <div id="divItemContainter1" style="position: relative; display: none"></div>
            <asp:Literal Text="" ID="placeholderrisultati" runat="server" />
            <!-- Go to www.addthis.com/dashboard to customize your tools -->
            <%--<script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-5ab9f0ea30a9272e"></script>--%>
        </div>
        <div class="col-md-3 col-sm-3" runat="server" id="column3">
            <%--<div class="sticky-top" style="top: 120px" id="divColumnsticky">--%>
            <div class="position-sticky mb-5" style="top: 120px" id="divColumnsticky">

                <!-- Sidebar Block -->
                <div style="max-width: 350px; margin: 10px auto" runat="server" id="divLinksrubriche" visible="false">
                </div>

                <asp:Literal Text="" ID="placeholderlateral" runat="server" />
                <asp:Literal Text="" ID="placeholderlateral1" runat="server" />
                <asp:Literal Text="" ID="placeholderlateral2" runat="server" />
                <asp:Literal Text="" ID="placeholderlateral3" runat="server" />
                <!-- Sidebar Block -->
                <div runat="server" id="divContact" visible="true">
                    <div>
                        <div class="ui-15">
                            <div class="ui-content">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 ui-padd">
                                            <!-- Ui Form -->
                                            <div class="ui-form">
                                                <!-- Heading -->
                                                <h3 class="h3-sidebar-title sidebar-title">
                                                    <%= references.ResMan("Common", Lingua,"TestoDisponibilita") %>
                                                </h3>
                                                <!-- Form -->
                                                <!-- UI Input -->
                                                <div class="ui-input">
                                                    <!-- Input Box -->
                                                    <input class="form-control" type="text" name="uname" validationgroup="contattilateral" placeholder="Nome" runat="server" id="txtContactName1" />
                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Cognome" runat="server" id="txtContactCognome1" />
                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Telefono" runat="server" id="txtContactPhone1" />
                                                    <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input type="text" class="form-control" name="unname" validationgroup="contattilateral" placeholder="Email" runat="server" id="txtContactEmail1" />
                                                    <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contattilateral" placeholder="Messaggio .." runat="server" id="txtContactMessage1" />
                                                </div>
                                                <div class="row" style="text-align: left">
                                                    <div class="col-sm-12">
                                                        <%= references.ResMan("basetext", Lingua,"privacytext") %>
                                                        <div class="checkbox" style="margin-bottom: 20px">
                                                            <label>
                                                                <input id="chkContactPrivacy1" runat="server" type="checkbox" />
                                                                <span class="cr"><i class="cr-icon fa fa-check" style="color: #000"></i></span><%= references.ResMan("basetext", Lingua,"privacyconsenso") %>
                                                            </label>
                                                        </div>
                                                        <div class="checkbox" style="margin-bottom: 20px">
                                                            <label>
                                                                <input type="checkbox" id="chkNewsletter1" runat="server" />
                                                                <span class="cr"><i class="cr-icon fa fa-check" style="color: #000"></i></span><%= references.ResMan("basetext", Lingua,"privacyconsenso1") %>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <script>
                                                    function ConfirmValidationForm1(elembtn) {
                                                        var c = document.getElementById("<%= chkContactPrivacy1.ClientID  %>");
                                                        var out1 = document.getElementById("outputContact1div");
                                                        if (!chk1.checked) {
                                                            out1.innerHTML = '<%= references.ResMan("Common", Lingua,"txtprivacyerror")%>';
                                                            return false;
                                                        } else { out1.innerHTML = ''; }
                                                        if (Page_ClientValidate("contattilateral")) {
                                                            /*do work and go for postback*/
                                                            console.log('ok validated');
                                                          <%--  var buttpost = document.getElementById("<%= btnFormContattoSrv.ClientID  %>");--%>
                                                            $(elembtn).attr("disabled", "");

                                                            //invio nopostback con handler////////////////////////////////////////////////////
                                                            var contactdatas = {};
                                                            contactdatas.chkprivacy = $('<%= "#" + chkContactPrivacy1.ClientID %>')[0].checked;
                                                            contactdatas.chknewsletter = $('<%= "#" + chkNewsletter1.ClientID %>')[0].checked;
                                                            getcontactdata1(contactdatas, function (contactdatas) {
                                                                var tastotxt = $(elembtn).html();
                                                                $(elembtn).html("Wait ..");
                                                                inviamessaggiomail(lng, contactdatas, function (result) {
                                                                    if (result) {
                                                                        //in caso di errore visualizzo
                                                                        document.getElementById("outputContact1div").innerHTML = (result);
                                                                        $(elembtn).removeAttr("disabled");
                                                                        $(elembtn).html(tastotxt);
                                                                    }
                                                                }, tastotxt);
                                                            }, $(elembtn));
                                                            ///////////////////////////////////////////////////////////////////////

                                                                            ////////////////////////////////////////////////////////////////////////
                                                            //Invio con postback
                                                            <%--  var buttpost = document.getElementById("<%= btnInviaSrv.ClientID  %>");
                                                            $(elembtn).html("Wait ..");
                                                            buttpost.click();--%>
                                                            ////////////////////////////////////////////////////////////////////////
                                                        } else {
                                                            console.log('not  validated');
                                                            return false;
                                                        }
                                                    }

                                                    function getcontactdata1(contactdatas, callback) {
                                                        var contactdatas = contactdatas || {};
                                                        contactdatas.idofferta = '<%= idOfferta %>';
                                                        contactdatas.name = $("[id$='txtContactName1']").val();
                                                        contactdatas.cognome = $("[id$='txtContactCognome1']").val();
                                                        contactdatas.email = $("[id$='txtContactEmail1']").val();
                                                        contactdatas.telefono = $("[id$='txtContactPhone1']").val();
                                                        contactdatas.message = $("[id$='txtContactMessage1']").val();
                                                        contactdatas.tipo = "informazioni";
                                                        callback(contactdatas);
                                                    }

                                                </script>
                                                <button id="btnFormContatto" type="button" class="btn btn-lg btn-block" style="width: 200px" runat="server" validationgroup="contattilateral" onclick="ConfirmValidationForm1(this);"><%= references.ResMan("Common", Lingua,"TestoInvio") %> </button>
                                                <%-- <asp:Button ID="btnFormContattoSrv" Style="display: none" runat="server" OnClick="btnContatti1_Click" />--%>

                                                <div style="clear: both"></div>
                                                <div style="font-weight: 300; font-size: 1rem; color: red" id="outputContact1div">
                                                    <asp:Literal Text="" ID="outputContact1" runat="server" />
                                                </div>

                                                <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactName1" runat="server" />
                                                <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto16lErr") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactCognome1" runat="server" />
                                                <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactEmail1" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Sidebar Block -->
                <div id="divContainerBannerslat1"></div>
                <div class="sidebar-block" runat="server" id="div2" visible="false">
                    <div class="row" style="margin-bottom: 10px; margin-top: 0px">
                        <ul class="works-grid works-grid-gut works-grid-1 works-hover-lw">
                            <asp:Literal ID="litBannersLaterali" runat="server"></asp:Literal>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- <div class="row">
            <div id="fb-root"></div>
            <script type="text/javascript">
                (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id)) return;
                    js = d.createElement(s); js.id = id;
                    js.src = "//connect.facebook.net/it_IT/all.js#xfbml=1&appId=435846069925856";
                    fjs.parentNode.insertBefore(js, fjs);
                }(document, 'script', 'facebook-jssdk'));
            </script>
            <div class="fb-comments" data-width="650" data-num-posts="5" runat="server" id="divComments"></div>
        </div>--%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderIndextext" runat="Server">
   
        <%--RECENSIONI CLIENTI--%>
    <div runat="server" visible="true">
        <asp:Panel ID="pnlCommenti" runat="server" Visible="false">
            <div id="divCommenti" class="inject py-5 commenti-details-page bg-white" params="commenttool.rendercommentsloadref,'<%= idOfferta %>','divCommenti','feedbacklist2.html','true','1','35','',false,'',false,false"></div>
        </asp:Panel>
    </div>

     <%--ALTRI ARTICOLI--%>
    <%--<div class="bd-light-color" style="position: relative; border-top: 10px dotted; background-color:#fff;" id="divSuggeritiContainer">--%>
    <div class="bg-light-color" id="divSuggeritiContainer">
        <div style="max-width: 1600px; margin: 0px auto; position: relative; padding: 30px 25px;">
            <div id="divScrollerSuggeritiJsTitle" class="row justify-content-center mb-4" style="display: none; margin-left: 30px; margin-right: 30px">
                <div class="row">
                    <div class="col-sm-12 col-12">
                        <div class="subtitle-block clearfix">

                            <div class="row" style="text-align: left; padding-bottom: 0px; padding-top: 0px; margin-bottom: 0px; line-height: 40px; color: #33332e;">
                                <div class="pull-left lead">
                                    <h2 class="mbr-section-title" style="margin-bottom: 3px">
                                        <%--<%= (CodiceTipologia=="rif000100" || CodiceTipologia=="rif000101" || CodiceTipologia=="rif000003") ?  references.ResMan("Common",Lingua,"titoloCollegati").ToString(): references.ResMan("Common",Lingua,"titoloCatalogoConsigliati").ToString() %>--%>
                                        <%= (!string.IsNullOrEmpty(references.ResMan("Common", Lingua, "titoloCollegati" + CodiceTipologia).ToString())) ? references.ResMan("Common", Lingua, "titoloCollegati" + CodiceTipologia).ToString() : references.ResMan("Common", Lingua, "titoloCatalogoConsigliati").ToString() %>     </h2>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <asp:Literal Text="" ID="plhSuggeritiJs" runat="server" />
        </div>
    </div>

    <div id="richiedilinkpoint" style="padding-top: 176px; margin-top: -176px;"></div>

    <%--CONTATTI--%>
    <div class="ui-15 bg-secondary-color no-plygon-bottom" runat="server" id="divContactBelow" clientidmode="static" visible="false">
        <div class="container  bg-secondary-color formborder1 formshadow pb-3">
            <section class="mbr-section mbr-section__container article" id="header3-a" style="padding-top: 60px; padding-bottom: 10px;">
                <div class="container">
                    <div class="row justify-content-center">
                        <div class="col-12 col-sm-8 tx-dark-color">
                            <%--        <div style="text-align: center; width: 100%"><%= references.ResMan("Common", Lingua,"TestoDisponibilita") %></div>--%>
                            <div style="text-align: center; width: 100%"><%= references.ResMan("Common", Lingua,"testocontattadtl") %></div>
                        </div>
                    </div>
                </div>
            </section>
            <!-- Ui Form -->
            <div class="ui-form">
                <!-- Heading -->
                <div class="row justify-content-center" style="padding-right: inherit">
                    <div class="col-md-8 col-md-offset-2">
                        <div class="row">
                            <div class="col-sm-6">
                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactName" runat="server" />
                                <div class="ui-input">
                                    <!-- Input Box -->
                                    <input class="form-control" type="text" name="uname" validationgroup="contattilateral" placeholder="Nome" runat="server" id="txtContactName" />
                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto16lErr") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactCognome" runat="server" />
                                <div class="ui-input">
                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Cognome" runat="server" id="txtContactCognome" />
                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" ErrorMessage='<%# references.ResMan("Common", Lingua,"formtesto11Err") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactPhone" runat="server" />
                                <div class="ui-input">
                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Telefono" runat="server" id="txtContactPhone" />
                                    <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <asp:RequiredFieldValidator CssClass="errorvalidateclass" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactEmail" runat="server" />
                                <div class="ui-input">
                                    <input type="text" class="form-control" name="unname" validationgroup="contattilateral" placeholder="Email" runat="server" id="txtContactEmail" />
                                    <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                </div>
                            </div>
                        </div>
                        <div class="ui-input">
                            <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contattilateral" placeholder="Messaggio .." runat="server" id="txtContactMessage" />
                        </div>
                        <div class="row my-3" style="text-align: left">
                            <div class="col-sm-12 text-white">
                                <%= references.ResMan("basetext", Lingua,"privacytext") %>
                                <div class="checkbox" style="margin-bottom: 20px">
                                    <label>
                                        <input id="chkContactPrivacy" runat="server" type="checkbox" />
                                        <span class="cr"><i class="cr-icon fa fa-check" style="color: #000"></i></span><%= references.ResMan("basetext", Lingua,"privacyconsenso") %>
                                    </label>
                                </div>
                                <div class="checkbox" style="margin-bottom: 20px">
                                    <label>
                                        <input type="checkbox" id="chkNewsletter" runat="server" />
                                        <span class="cr"><i class="cr-icon fa fa-check" style="color: #000"></i></span><%= references.ResMan("basetext", Lingua,"privacyconsenso1") %>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <script>
                            function ConfirmValidationForm(elembtn) {
                                var chk1 = document.getElementById("<%= chkContactPrivacy.ClientID  %>");
                                var out1 = document.getElementById("outputContactdiv");
                                if (!chk1.checked) {
                                    out1.innerHTML = '<%= references.ResMan("Common", Lingua,"txtprivacyerror")%>';
                                    return false;
                                } else { out1.innerHTML = ''; }
                                if (Page_ClientValidate("contattilateral")) {
                                    /*do work and go for postback*/
                                    console.log('ok validated');
                    <%--                var buttpost = document.getElementById("<%= Button1srv.ClientID  %>");--%>
                                    $(elembtn).attr("disabled", "")

                                    //invio nopostback con handler////////////////////////////////////////////////////
                                    var contactdatas = {};
                                    contactdatas.chkprivacy = $('<%= "#" + chkContactPrivacy.ClientID %>')[0].checked;
                                    contactdatas.chknewsletter = $('<%= "#" + chkNewsletter.ClientID %>')[0].checked;
                                    getcontactdata2(contactdatas, function (contactdatas) {
                                        var tastotxt = $(elembtn).html();
                                        $(elembtn).html("Wait ..");
                                        inviamessaggiomail(lng, contactdatas, function (result) {
                                            if (result) {
                                                //in caso di errore visualizzo
                                                document.getElementById("outputContactdiv").innerHTML = (result);
                                                $(elembtn).removeAttr("disabled");
                                                $(elembtn).html(tastotxt);
                                            }
                                        }, tastotxt);
                                    }, $(elembtn));
                                    ///////////////////////////////////////////////////////////////////////

                                    ////////////////////////////////////////////////////////////////////////
                                    //Invio con postback
                                    <%--  var buttpost = document.getElementById("<%= btnInviaSrv.ClientID  %>");
                                    $(elembtn).html("Wait ..");
                                    buttpost.click();--%>
                                    ////////////////////////////////////////////////////////////////////////
                                } else {
                                    $('html,body').animate({
                                        scrollTop: $("#" + "<%= divContactBelow.ClientID  %>").offset().top - 160
                                    }, 5);
                                    console.log('not  validated');
                                    return false;
                                }
                            }
                            function getcontactdata2(contactdatas, callback) {
                                var contactdatas = contactdatas || {};
                                contactdatas.idofferta = '<%= idOfferta %>';
                                contactdatas.name = $("[id$='txtContactName']").val();
                                contactdatas.cognome = $("[id$='txtContactCognome']").val();
                                contactdatas.email = $("[id$='txtContactEmail']").val();
                                contactdatas.telefono = $("[id$='txtContactPhone']").val();
                                contactdatas.message = $("[id$='txtContactMessage']").val();
                                contactdatas.tipo = "informazioni";
                                callback(contactdatas);
                            }

                        </script>
                        <button id="Button1" type="button" class="btn btn-lg btn-block" style="width: 200px" runat="server" validationgroup="contattilateral" onclick="ConfirmValidationForm(this);"><%= references.ResMan("Common", Lingua,"TestoInvio") %> </button>
                        <%-- <asp:Button ID="Button1srv" Style="display: none" runat="server" OnClick="btnContatti_Click" />--%>

                        <div style="font-weight: 300; font-size: 1rem; color: red" id="outputContactdiv">
                            <asp:Literal Text="" ID="outputContact" runat="server" />
                        </div>
                        <asp:ValidationSummary ID="validsummary" ValidationGroup="contattilateral" runat="server" BackColor="#cccccc" DisplayMode="SingleParagraph" Font-Size="Medium" ShowValidationErrors="true" HeaderText='<%# references.ResMan("Common", Lingua,"ValidationError") +  "<br/><style> span.errorvalidateclass ~ * {  border: 2px solid red; }  </style>" %>' />

                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
