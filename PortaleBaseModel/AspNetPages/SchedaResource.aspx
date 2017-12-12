<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="SchedaResource.aspx.cs" Inherits="AspNetPages_SchedaResource" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderSubsSlider" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <!-- Go to www.addthis.com/dashboard to customize your tools -->
    <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-57dd1b48e4253a53"></script>

    <div class="container" style="text-align: center; margin-top: 10px">
        <div class="row" runat="server" id="divTitle">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12">
                <h1 class="title-block" style="line-height: normal;">
                    <asp:Literal Text="" runat="server" ID="litNomePagina" /></h1>
            </div>
        </div>
    </div>
    <div class="loaderrelative" style="display: none">
        <div class="spinner"></div>
    </div>
    <asp:Literal Text="" runat="server" ID="litTextHeadPage" />

    <div class="container">
        <asp:Panel runat="server" ID="pnlButtonsnav" Visible="false">
            <div class="row">
                <div class="col-sm-3 col-sm-offset-1">
                    <a href="#frmContact">
                        <div class="divbuttonstyle" style="font-size: 0.8em; padding: 4px; margin-bottom: 3px">
                            <%--  <a href='<%= ReplaceAbsoluteLinks( "~/Aspnetpages/Content_Tipo3.aspx?idOfferta=" + idOfferta + "&Tipologia=" + CodiceTipologia + "&TipoContenuto=Richiesta"  + "&Lingua=" + Lingua) %>' title="">
                        </a>--%>   <%= ImpostaTestoRichiesta() %>
                        </div>
                    </a>
                </div>
                <div class="col-sm-3 hidden-xs">

                    <%= "<a  target=\"_blank\" href=\"" + PercorsoAssolutoApplicazione +"/aspnetpages/SchedaResourceStampa.aspx?idOfferta=" + idOfferta + "&CodiceTipologia=" + CodiceTipologia + "&Lingua=" + Lingua
            + "\"><div class=\"divbuttonstyle\"   style=\"font-size: 0.8em; padding: 4px; margin-bottom: 3px\"><i class=\"fa fa-print\"></i>" +  references.ResMan("Basetext", Lingua, "Stampa") + "</div> </a>"  %>
                </div>

                <div class="col-sm-offset-2 col-sm-2">

                    <%= "<a  href=\"" +  GeneraBackLink()  + "\"> <div  class=\"divbuttonstyle\"  style=\"font-size: 0.8em; padding: 4px; margin-bottom: 3px\"><i class=\"fa fa-reply-all\"></i>&nbsp;" +  references.ResMan("Common", Lingua, "testoIndietro") + "</div></a>" %>
                </div>
            </div>
            <br />
        </asp:Panel>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="col-md-1 col-sm-1" runat="server" id="column1">
    </div>
    <div class="col-md-9 col-sm-9" runat="server" id="column2">
        <div class="row">
            <asp:Label ID="output" runat="server"></asp:Label>
        </div>
        <div class="loaderrelative" style="display: none">
            <div class="spinner"></div>
        </div>
        <div id="divItemContainter1" style="position: relative; display: none"></div>

        <div class="blog-post">

            <%-- <div class="row" runat="server" id="divContact" visible="true">
                <div class="ui-15">
                    <div class="ui-content">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12 col-sm-12 ui-padd">
                                    <!-- Ui Form -->
                                    <div class="ui-form" style="padding-top: 100px; margin-top: -100px;" id="frmContact">
                                        <!-- Heading -->
                                        <h3 class="blog-sezione">
                                            <%=  references.ResMan("Basetext", Lingua, "TestoDisponibilita")  %>
                                        </h3>
                                        <!-- Form -->
                                        <!-- UI Input -->
                                        <div class="ui-input">
                                            <!-- Input Box -->
                                            <input class="form-control" type="text" name="uname" validationgroup="contatti" placeholder='<%# references.ResMan("Common", Lingua, "FormTesto2") %>' runat="server" id="txtContactName" />
                                            <label class="ui-icon"><i class="fa fa-user"></i></label>
                                        </div>
                                        <div class="ui-input">
                                            <input class="form-control" type="text" name="unname" validationgroup="contatti"
                                                placeholder='<%# references.ResMan("Common", Lingua, "FormTesto16l") %>' runat="server" id="txtContactCognome" />
                                            <label class="ui-icon"><i class="fa fa-user"></i></label>
                                        </div>
                                        <div class="ui-input">
                                            <input class="form-control" type="text" name="unname" validationgroup="contatti"
                                                placeholder='<%# references.ResMan("Common", Lingua, "FormTesto11") %>'
                                                runat="server" id="txtContactPhone" />
                                            <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                        </div>
                                        <div class="ui-input">
                                            <input type="text" class="form-control" name="unname" validationgroup="contatti" placeholder="Email" runat="server" id="txtContactEmail" />
                                            <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                        </div>
                                        <div class="ui-input">
                                            <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contatti" placeholder='<%# references.ResMan("Common", Lingua, "FormTesto17") %>' runat="server" id="txtContactMessage" />
                                        </div>
                                        <button id="btnFormContatto" class="divbuttonstyle" runat="server" validationgroup="contatti" onserverclick="btnContatti1_Click"><%= references.ResMan("Basetext", Lingua, "TestoInvio")  %></button>
                                        <div style="clear: both"></div>
                                        <asp:CheckBox ID="chkContactPrivacy" runat="server" Style="font-weight: 300; font-size: 10px" Checked="true"
                                            Text='<%# references.ResMan("Common", Lingua, "testoPrivacy") %>' />
                                        <asp:RequiredFieldValidator
                                            ErrorMessage='<%# references.ResMan("Common", Lingua, "FormTesto2Err") %>'
                                            ValidationGroup="contatti" ControlToValidate="txtContactName" runat="server" />
                                        <asp:RequiredFieldValidator
                                            ErrorMessage='<%# references.ResMan("Common", Lingua, "FormTesto16lErr") %>'
                                            ValidationGroup="contatti" ControlToValidate="txtContactCognome" runat="server" />
                                        <asp:RequiredFieldValidator
                                            ErrorMessage='<%# references.ResMan("Common", Lingua, "FormTesto4Err") %>'
                                            ValidationGroup="contatti" ControlToValidate="txtContactEmail" runat="server" />
                                        <div style="font-weight: 300; font-size: 10px; color: red">
                                            <asp:Literal Text="" ID="outputContact" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
        </div>
    </div>
    <div class="col-md-3 col-sm-3" runat="server" id="column3">
        <div class="sidebar">
            <!-- Sidebar Block -->
            <%--  <div class="sidebar-block" runat="server" id="divContact" visible="true">
                <div class="sidebar-content">
                    <div class="ui-15">
                        <div class="ui-content">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 ui-padd">
                                        <!-- Ui Form -->
                                        <div class="ui-form" style="padding-top: 100px; margin-top: -100px;" id="frmContact">
                                            <!-- Heading -->
                                            <h3 class="h3-sidebar-title sidebar-title">
                                                 <%=  references.ResMan("Basetext", Lingua, "TestoDisponibilita")  %>
                                            </h3>
                                            <!-- Form -->
                                            <!-- UI Input -->
                                            <div class="ui-input">
                                                <!-- Input Box -->
                                                <input class="form-control" type="text" name="uname" validationgroup="contatti" placeholder='Nome" runat="server" id="txtContactName" />
                                                <label class="ui-icon"><i class="fa fa-user"></i></label>
                                            </div>
                                            <div class="ui-input">
                                                <input class="form-control" type="text" name="unname" validationgroup="contatti" placeholder='Cognome" runat="server" id="txtContactCognome" />
                                                <label class="ui-icon"><i class="fa fa-user"></i></label>
                                            </div>
                                            <div class="ui-input">
                                                <input class="form-control" type="text" name="unname" validationgroup="contatti" placeholder='Telefono" runat="server" id="txtContactPhone" />
                                                <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                            </div>
                                            <div class="ui-input">
                                                <input type="text" class="form-control" name="unname" validationgroup="contatti" placeholder='Email" runat="server" id="txtContactEmail" />
                                                <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                            </div>
                                            <div class="ui-input">
                                                <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contatti" placeholder='Messaggio .." runat="server" id="txtContactMessage" />
                                            </div>


                                            <button id="btnFormContatto" class="divbuttonstyle" runat="server" validationgroup="contatti" onserverclick="btnContatti1_Click"><%=  references.ResMan("Basetext", Lingua, "TestoInvio") %></button>
                                            <div style="clear: both"></div>
                                            <asp:CheckBox ID="chkContactPrivacy" runat="server" Style="font-weight: 300; font-size: 10px" Checked="true" Text="Acconsento al trattamento dei miei dati personali (D.Lgs 196/2003) " />
                                              <asp:RequiredFieldValidator
                                            ErrorMessage='<%# references.ResMan("Common", Lingua, "FormTesto2Err") %>'
                                            ValidationGroup="contatti" ControlToValidate="txtContactName" runat="server" />
                                        <asp:RequiredFieldValidator
                                            ErrorMessage='<%# references.ResMan("Common", Lingua, "FormTesto16lErr") %>'
                                            ValidationGroup="contatti" ControlToValidate="txtContactCognome" runat="server" />
                                        <asp:RequiredFieldValidator
                                            ErrorMessage='<%# references.ResMan("Common", Lingua, "FormTesto4Err") %>'
                                            ValidationGroup="contatti" ControlToValidate="txtContactEmail" runat="server" />
                                            <div style="font-weight: 300; font-size: 10px; color: red">
                                                <asp:Literal Text="" ID="outputContact" runat="server" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
        </div>
    </div>
    <!-- Scroller per similari -->
    <div id="plhSimilari"></div>
    <!-- Fine scroller similari  -->
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderIndextext" runat="Server">
    <div id="richiedilinkpoint" style="padding-top: 80px; margin-top: -80px;"></div>
    <div class="ui-15" runat="server" id="divContactBelow" clientidmode="static" visible="false" style="background-color: #efefef">
        <div class="container">
            <section class="mbr-section mbr-section__container article" id="header3-a" style="padding-top: 20px; padding-bottom: 10px;">
                <div class="container">
                    <div class="row">
                        <div class="col-xs-12">
                            <div style="text-align: center; width: 100%"><%= references.ResMan("Common", Lingua,"TestoDisponibilita") %></div>
                        </div>
                    </div>
                </div>
            </section>
            <!-- Ui Form -->
            <div class="ui-form">
                <!-- Heading -->
                <div class="row" style="padding-right: inherit">
                    <div class="col-md-8 col-md-offset-2">
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="ui-input">
                                    <!-- Input Box -->
                                    <input class="form-control" type="text" name="uname" validationgroup="contattilateral" placeholder='<%# references.ResMan("Common", Lingua, "FormTesto2") %>' runat="server" id="txtContactName" />
                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="ui-input">
                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder='<%# references.ResMan("Common", Lingua, "FormTesto16l") %>' runat="server" id="txtContactCognome" />
                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="ui-input">
                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder='<%# references.ResMan("Common", Lingua, "FormTesto11") %>' runat="server" id="txtContactPhone" />
                                    <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="ui-input">
                                    <input type="text" class="form-control" name="unname" validationgroup="contattilateral" placeholder="Email" runat="server" id="txtContactEmail" />
                                    <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                </div>
                            </div>
                        </div>
                        <div class="ui-input">
                            <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contattilateral" placeholder='<%# references.ResMan("Common", Lingua, "FormTesto17") %>' runat="server" id="txtContactMessage" />
                        </div>


                        <button id="Button1" class="btn btn-blue btn-lg btn-block" runat="server" validationgroup="contattilateral" onserverclick="btnContatti1_Click"><%= references.ResMan("Common", Lingua,"TestoInvio") %></button>
                        <asp:CheckBox ID="chkContactPrivacy" runat="server" Style="font-weight: 300; font-size: 10px" Checked="true" Text="Acconsento al trattamento dei miei dati personali (D.Lgs 196/2003) " />
                        <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactName" runat="server" />
                        <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto16lErr") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactCognome" runat="server" />
                        <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' ValidationGroup="contattilateral" ControlToValidate="txtContactEmail" runat="server" />
                        <div style="font-weight: 300; font-size: 10px; color: red">
                            <asp:Literal Text="" ID="outputContact" runat="server" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div style="background-color: #f2ece6;">
        <div style="max-width: 1600px; margin: 0px auto; position: relative; padding-left: 10px; padding-right: 10px;">
            <div class="row">
                <div id="divScrollerSuggeritiJsTitle" class="row" style="display: none; margin-left: 30px; margin-right: 30px">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="subtitle-block clearfix">
                                <div class="row" style="text-align: left; padding-bottom: 0px; padding-top: 30px; margin-bottom: 0px; line-height: 40px; color: #33332e; border-bottom: 1px solid #33332e">
                                    <div class="pull-left lead">
                                        <h2 style="margin-bottom: 3px">
                                            <%--<%= (CodiceTipologia=="rif000100" || CodiceTipologia=="rif000101" || CodiceTipologia=="rif000003") ?  references.ResMan("Common",Lingua,"titoloCollegati").ToString(): references.ResMan("Common",Lingua,"titoloCatalogoConsigliati").ToString() %>--%>
                                            <%= (CodiceTipologia=="rif000100" || CodiceTipologia=="rif000101" || CodiceTipologia=="rif000003") ?  references.ResMan("Common", Lingua,"titoloCollegati") : references.ResMan("Common", Lingua, "titoloCatalogoConsigliati") %>
                                        </h2>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div id="divScrollerSuggeritiJs"></div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentPlaceHolderJs" runat="Server">
</asp:Content>

