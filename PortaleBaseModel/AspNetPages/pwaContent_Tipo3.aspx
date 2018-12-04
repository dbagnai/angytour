<%@ Page Language="C#" MasterPageFile="~/AspNetPages/pwaMasterPage.master" AutoEventWireup="true"
    CodeFile="pwaContent_Tipo3.aspx.cs" Inherits="AspNetPages_Content_Tipo3" Title=""
    MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AspNetPages/pwaMasterPage.master" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderSubsSlider" runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function OpenWindow(Url, NomeFinestra, features) {
            window.open(Url, NomeFinestra, features);
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/js/googleMaps.js")%>"></script>
    <script type="text/javascript">
        makeRevLower = true;
    </script>
    <div class="container d-none" style="text-align: center">
        <div class="row">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-10 col-sm-10 col-xs-12">
                <h2 class="h1-body-title" style="color: #5c5c5c">
                    <asp:Literal Text="" runat="server" ID="litNomePagina" /></h2>
            </div>
            <div class="col-md-1 col-sm-1">
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div style="text-align: center">
                    <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row" style="text-align: center">
        <div class="col">

            <%--TITOLO PAGINA--%>
            <div class="d-none">
            <p>
                <asp:Label runat="server" ID="output" Font-Size="Medium"></asp:Label>
            </p>
            <asp:Label runat="server" ID="lblContenutiContatti"></asp:Label>
                </div>


            <!--  MENU TOP MAIl e MAPPA -->
<ul class="nav nav-tabs px-3" id="myTab" role="tablist">
    <li class="nav-item">
        <a class="nav-link active" id="contatti-tab" data-toggle="tab" href="#contatti" role="tab" aria-controls="contatti" aria-selected="true"><%= references.ResMan("Common", Lingua,"testocontatti") %></a>
    </li>
    <li class="nav-item">
        <a class="nav-link" id="mappa-tab" data-toggle="tab" href="#mappa" role="tab" aria-controls="mappa" aria-selected="false"><%= references.ResMan("Common", Lingua,"testoid5") %></a>
    </li>
    <!--<li class="nav-item">
        <a class="nav-link" id="contact-tab" data-toggle="tab" href="#contact" role="tab" aria-controls="contact" aria-selected="false">Contact</a>
    </li>-->
</ul>
            <div class="tab-content" id="myTabContent">

            <%--STRAT FORM CONTATTI--%>
<div class="tab-pane fade show active" id="contatti" role="tabpanel" aria-labelledby="contatti-tab">
            <section class="mbr-section mbr-parallax-background p-0 bg-white">
                <div class="w-100 py-3 px-3 ">
                    <div class="row justify-content-end">

                        <%--STRAT FORM CONTATTI--%>

                        <%--AVVISO ERRORE DATI NON INSERITI--%>
                            <asp:ValidationSummary runat="server" ID="Summary" class="errormessageform" ValidationGroup="MailInfo" DisplayMode="BulletList"
                                ShowSummary="true" HeaderText='<%# references.ResMan("Common", Lingua,"testoDatiMancanti")  %>' />
                            <%--AVVISO ERRORE TRATTAMENTO DATI NON SPUNTATO--%>
                            <div id="recaptcharesponse" class="errormessageformprivacy errore-privacy-pwa-contact" style="display:none;"></div>

                        <asp:PlaceHolder runat="server" ID="plhForm" Visible="true">
                            <div class="col-12 text-center <%--bg-manual-secondary-color-fade--%> bg-white">
                                <div class="ui-15">
                                    <div class="ui-content my-0">
                                        <div class="container-fluid px-3 mx-auto">
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12 ui-padd">
                                                    <div class="ui-form p-0">

                                                        <%--TITOLO Vuoi informazioni?--%>
                                                        <%--<div class="row pb-3" runat="server" visible="true" id="divTitlemail">
                                                            <div class="col-sm-12">
                                                                <div class="lead text-white">
                                                                    <%= references.ResMan("Common", Lingua,"TestoContattaciFooter") %>
                                                                </div>
                                                            </div>
                                                        </div>--%>

                                                        <asp:Panel runat="server" ID="pnlPrenotazione" Visible="false">
                                                           

                                                            <%--COLLOCAZIONE--%>
                                                            <div class="row" runat="server" visible="true" id="divLocations">
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input mt-0">
                                                                        <asp:DropDownList runat="server" Width="100%" ID="ddlLocations" class="form-control">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                                <div class="col-sm-3">
                                                                    <div class="ui-input mt-0">
                                                                        <script>
                                                                            jQuery(function ($) {
                                                                                $("#ctl00_ContentPlaceHolder1_txtAdulti").mask("99", { placeholder: " " });
                                                                            });
                                                                        </script>
                                                                        <asp:TextBox ID="txtAdulti" Width="100%" runat="server"
                                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoAdulti") %>' />
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <div class="ui-input mt-0">

                                                                        <script>
                                                                            jQuery(function ($) {
                                                                                $("#ctl00_ContentPlaceHolder1_txtBambini").mask("99", { placeholder: " " });
                                                                            });
                                                                        </script>
                                                                        <asp:TextBox ID="txtBambini" Width="100%" runat="server"
                                                                            class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoBambini") %>' />
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <%--DATA ARRIVO E PARTENZA--%>
                                                            <div class="row">
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input mt-2 mb-4">
                                                                        <asp:TextBox ID="txtArrivo" Width="100%" runat="server"
                                                                            class="form-control datepicker" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPeriododa") %>' />
                                                                        <label class="ui-icon"><i class="fa fa-calendar"></i></label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <div class="ui-input mt-2 mb-4">
                                                                        <asp:TextBox ID="txtPartenza" Width="100%" runat="server" class="form-control datepicker" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPeriodoa") %>' />
                                                                        <label class="ui-icon"><i class="fa fa-calendar"></i></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                        <%--nome--%>
                                                        <div class="row">
                                                            <div class="col-sm-6">
                                                                <div class="ui-input mt-0 mb-3">
                                                                    <asp:TextBox ID="txtNome" Width="100%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                                    <div class="position-absolute w-100" style="top:0;">
                                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtNome"
                                                                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTestoGenericErr") %>' Text="" ValidationGroup="MailInfo" />
                                                                        </div>
                                                                </div>
                                                            </div>
                                                            <%--cognome--%>
                                                            <div class="col-sm-6">
                                                                <div class="ui-input mt-0 mb-3">
                                                                    <asp:TextBox ID="txtSoc" Width="100%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto16l") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                                    <div class="position-absolute w-100" style="top:0;">
                                                                    <asp:RequiredFieldValidator runat="server" ID="reqValidatorNome" ControlToValidate="txtSoc"
                                                                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTestoGenericErr") %>' Text="" ValidationGroup="MailInfo" />
                                                                        </div>
                                                                </div>
                                                            </div>                                                      
                                                        <%--mail--%>
                                                            <div class="col-sm-6">
                                                                <div class="ui-input mt-0 mb-3">
                                                                    <asp:TextBox ID="txtEmail" Width="100%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                                                    <div class="position-absolute w-100" style="top:0;">
                                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtEmail"
                                                                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTestoGenericErr") %>' Text="" ValidationGroup="MailInfo" />
                                                                        </div>
                                                                </div>
                                                            </div>
                                                        <%--telefono--%>
                                                            <div class="col-sm-6">
                                                                <div class="ui-input mt-0 mb-3">
                                                                    <asp:TextBox ID="txtTel" Width="100%" runat="server"
                                                                        class="form-control" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                                                                    <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    <%--regione--%>
                                                        <div class="row" runat="server" id="divRegione" visible="false">
                                                            <div class="col-sm-12">
                                                                <div class="ui-input mt-0 mb-3">
                                                                    <asp:Literal ID="Literal8" runat="server" Text='<%# references.ResMan("Common", Lingua,"FormTesto6") %>' />*<br />
                                                                    <asp:DropDownList class="form-control" runat="server" ID="ddlRegione" AppendDataBoundItems="true"
                                                                        OnInit="ddlRegione_OnInit" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto6") %>' />
                                                                    <div class="position-absolute w-100" style="top:0;">
                                                                    <asp:RequiredFieldValidator runat="server" ID="reqValidatorRegione" ControlToValidate="ddlRegione" Enabled="false"
                                                                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTestoGenericErr") %>' Text="" ValidationGroup="MailInfo" />
                                                                        </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        </div>

                                                    <%--messaggio--%>
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="ui-input mt-0 mb-3">
                                                                    <asp:TextBox ID="txtDescrizione" Width="100%"
                                                                        TextMode="MultiLine" Font-Names="Raleway"
                                                                        Height="250px" runat="server" class="form-control tx-primary-color" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto17") %>' />
                                                                    <div class="position-absolute w-100" style="top:0;">
                                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtDescrizione"
                                                                        ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTestoGenericErr") %>' Text="" ValidationGroup="MailInfo" />
                                                                        </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-sm-12" style="text-align: center">

                                                                <button id="btnInvia" type="button" class="btn btn-lg btn-block text-white" style="width: 200px" runat="server" validationgroup="MailInfo" onclick="ConfirmContactValue(this);"><%=  references.ResMan("Common", Lingua,"TestoInvio")  %> </button>
                                                                <asp:Button ID="btnInviaSrv" Style="display: none" runat="server" OnClick="btnInvia_Click" />
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
                                                                        if ($('<%= "#" + chkPrivacy.ClientID %>')[0].checked == false) {
                                                                            $("#recaptcharesponse").html('<%= references.ResMan("Common", Lingua,"txtPrivacyError") %>');
                                                                            $('#recaptcharesponse').css({ 'display': 'block' });
                                                                            return false;
                                                                        }
                                                                        else {
                                                                            $("#recaptcharesponse").html('');
                                                                            $('#recaptcharesponse').css({ 'display': 'none' });
                                                                        }
                                                            <%--  var response = grecaptcha.getResponse();
                                                            if (response.length == 0)  //reCaptcha not verified
                                                            {
                                                                $("#recaptcharesponse").html('<%= references.ResMan("Common", Lingua,"testoCaptcha") %>');
                                                                return false;
                                                            }
                                                            else {--%>
                                                                        if (Page_ClientValidate("MailInfo")) {
                                                                            /*do work and go for postback*/
                                                                            $(elembtn).attr("disabled", "")

                                                                            //invio nopostback con handler////////////////////////////////////////////////////
                                                                            var contactdatas = {};
                                                                            contactdatas.chkprivacy = $('<%= "#" + chkPrivacy.ClientID %>')[0].checked;
                                                                            contactdatas.chknewsletter = $('<%= "#" + chkNewsletter.ClientID %>')[0].checked;
                                                                            getcontactdata1(contactdatas, function (contactdatas) {
                                                                                var tastotxt = $(elembtn).html();
                                                                                $(elembtn).html("Wait ..");
                                                                                inviamessaggiomail(lng, contactdatas, function (result) {
                                                                                    if (result) {
                                                                                        //in caso di errore visualizzo
                                                                                        $("#recaptcharesponse").html(result);
                                                                                        $('#recaptcharesponse').css({ 'display': 'block' });
                                                                                        $(elembtn).removeAttr("disabled")
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
                                                                            // alert('not validated');
                                                                            return false;
                                                                        }
                                                                        //}
                                                                    }
                                                                    function getcontactdata1(contactdatas, callback) {
                                                                        var contactdatas = contactdatas || {};
                                                                        contactdatas.idofferta = '<%= idOfferta %>';
                                                                        contactdatas.tipocontenuto = '<%= TipoContenuto %>';
                                                                        contactdatas.location = $("#ddlLocations option:selected").text();
                                                                        contactdatas.regione = $("#ddlRegione option:selected").val();
                                                                        contactdatas.adulti = $("[id$='txtAdulti']").val();
                                                                        contactdatas.bambini = $("[id$='txtBambini']").val();
                                                                        contactdatas.arrivo = $("[id$='txtArrivo']").val();
                                                                        contactdatas.partenza = $("[id$='txtPartenza']").val();
                                                                        contactdatas.name = $("[id$='txtNome']").val();
                                                                        contactdatas.cognome = $("[id$='txtSoc']").val();
                                                                        contactdatas.email = $("[id$='txtEmail']").val();
                                                                        contactdatas.telefono = $("[id$='txtTel']").val();
                                                                        contactdatas.message = $("[id$='txtDescrizione']").val();
                                                                        contactdatas.tipo = "informazioni";
                                                                        callback(contactdatas);
                                                                    }
                                                                </script>

                                                                <br />
                                                                <div class="checkbox mt-2">
                                                                    <label class="w-100 text-left" style="margin-left: 30px;">
                                                                        <asp:CheckBox ID="chkPrivacy" runat="server" Checked="false" />
                                                                        <span class="cr" style="margin-left: -30px;"><i class="cr-icon fa fa-check"></i></span>
                                                                        <%= references.ResMan("Common", Lingua,"chkprivacy") %><a style="color: #ffd800" target="_blank" href="<%=CommonPage.ReplaceAbsoluteLinks(references.ResMan("Common", Lingua,"linkPrivacypolicy")) %>"> (<%= references.ResMan("Common", Lingua,"testoprivacyperlink") %>) </a>
                                                                    </label>
                                                                </div>

                                                                <div class="checkbox">
                                                                    <label class="w-100 text-left mb-0" style="margin-left: 30px;">
                                                                        <asp:CheckBox ID="chkNewsletter" runat="server" Checked="false" />
                                                                        <span class="cr" style="margin-left: -30px;"><i class="cr-icon fa fa-check"></i></span>
                                                                        <%= references.ResMan("Common", Lingua,"titoloNewsletter1") %>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                            </div>
                       
                </div>
            </section>
    </div>


            <%--MAP--%>
                <div class="tab-pane fade" id="mappa" role="tabpanel" aria-labelledby="mappa-tab">
            <asp:PlaceHolder ID="plhRisposta" runat="server" Visible="false">
                <h2>
                    <asp:Literal ID="lblRisposta" runat="server" Text=""></asp:Literal></h2>
                <br />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plhDove" runat="server" Visible="false">
                <div id="map" style="height: 410px; width: calc(100% - 2rem); margin: 1rem auto">class
                </div>
            </asp:PlaceHolder>
            <div class="container" runat="server" id="divRoutes" visible="false">
                <div class="row">
                    <div class="col-sm-12">
                        <%= references.ResMan("Common", Lingua,"TestoRaggiungerci") %>
                    </div>
                </div>
                <div class="row" style="text-align: center">
                    <div class="col-sm-12">
                        <asp:Label runat="server" ID="lblContenutiDove"></asp:Label>
                        <div class="divContentTitle mt-0 mb-3 mx-auto d-block" style="height: auto;">
                            <div style="font-size: 14px; padding-top: 0px;">
                                <span id="lblPartenza">
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"GooglePartenza") %>' runat="server" />
                                </span>
                            </div>
                            <div class="py-2">
                                <input id="startvalue" type="text" style="height: 30px; border: 1px solid #999999; width: 300px; padding: 5px 10px; font-size: 1rem;" />
                            </div>
                            <div style="font-size: 14px;">
                                <input id="Button1" style="box-shadow: none; border: none; color: white; cursor: pointer; width: 300px;"
                                    type="button" value="<%= references.ResMan("Common",Lingua,"GoogleCalcolapercorso").ToString() %>" onclick="return Button1_onclick()" />
                            </div>
                        </div>
                        <div id="directionpanel" style="height: auto; width: 96%; padding-bottom: 10px">
                        </div>
                    </div>
                    <div class="col-sm-2" style="text-align: left">
                        <asp:Literal ID="litPosizione" Text="" runat="server" />
                    </div>
                </div>
            </div>
                    </div>
                </div>





        </div>
    </div>

    <%--ELIMINO IL FORM CONTATTI DELLA MASTER--%>
    <script>
        jQuery(function ($) {
            $('#ctl00_divContattiMaster').css({ 'display': 'none' });
        });
    </script>
</asp:Content>
