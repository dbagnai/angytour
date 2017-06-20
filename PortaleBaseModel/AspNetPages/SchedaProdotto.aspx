<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="SchedaProdotto.aspx.cs"
    EnableTheming="true" Culture="it-IT" Inherits="_SchedaProdotto" Title="" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <script type="text/javascript">
        var makeRevLower = true;
    </script>
    <div class="container" style="padding-left: 35px; padding-right: 50px; text-align: center; margin-top: 0px">
        <div class="row">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-10 col-sm-10 col-xs-12">
                <h2 class="h1-body-title" style="color: #5c5c5c; margin-bottom: 10px">
                    <asp:Literal Text="" runat="server" ID="litSezione" /></h2>
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
        <div class="row" runat="server" visible="false">
            <div class="col-sm-4">
                <div class="divbuttonstyle" style="font-size: 0.8em; padding: 4px; margin-bottom: 3px">
                    <a href='<%= ReplaceAbsoluteLinks("~/Aspnetpages/Content_Tipo3.aspx?idOfferta=" + idOfferta + "&TipoContenuto=Richiesta" + "&Lingua=" + Lingua)%>' title="">
                        <%= ImpostaTestoRichiesta()%>
                       
                    </a>
                </div>
            </div>
            <div class="col-sm-offset-4 col-sm-2 hidden-xs">
                <div class="divbuttonstyle" style="font-size: 0.8em; padding: 4px; margin-bottom: 3px">
                    <%= "<a  target=\"_blank\" href=\"" + PercorsoAssolutoApplicazione + "/aspnetpages/SchedaOffertaStampa.aspx?idOfferta=" + idOfferta + "&CodiceTipologia=" + CodiceTipologia + "&Lingua=" + Lingua
            + "\"><i class=\"fa fa-print\"></i>&nbsp;Stampa</a>"%>
                </div>
            </div>
            <div class="col-sm-2">
                <div class="divbuttonstyle" style="font-size: 0.8em; padding: 4px; margin-bottom: 3px">
                    <%= "<a  href=\"" + GeneraBackLink() + "\"><i class=\"fa fa-reply-all\"></i>&nbsp;" + Resources.Common.testoIndietro + "</a>"%>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">

    <div class="row">
        <asp:Label ID="output" runat="server"></asp:Label>
    </div>


    <div class="row">
        <div class="col-md-9 col-sm-9">




            <asp:Repeater ID="rptOfferta" runat="server" OnItemDataBound="rptOfferta_ItemDataBound">
                <ItemTemplate>
                    <div class="blog-post" style="text-align: justify" itemscope="" itemtype="http://schema.org/Product">
                        <div class="blog-span">

                            <h1 itemprop="name">
                                <a id="a4" runat="server"
                                    href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                    target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                    <asp:Literal ID="Literal7" Text='<%# estraititolo(  Eval("Denominazione" + Lingua) ) %>'
                                        runat="server"></asp:Literal><br />
                                    <span style="font-size: 70%">
                                        <asp:Literal ID="Literal12" Text='<%# estraisottotitolo(  Eval("Denominazione" + Lingua) ) %>'
                                            runat="server"></asp:Literal></span>
                                </a>
                            </h1>
                            <em class="pull-left">
                                <span itemprop="author" style="font-size: 1.3rem; color: #1f809f"><%# TestoCaratteristica(0, Eval("Caratteristica1").ToString(), Lingua)%></span><br />
                                <span itemscope itemtype="http://schema.org/Organization">
                                    <span itemprop="name" style="font-size: 1rem; color: #1f809f"><%# TestoCaratteristica(1, Eval("Caratteristica2").ToString(), Lingua)%></span></span><br />
                                <span itemprop="category" style="font-size: 0.8em"><%# TestoCategoria(Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString(), Lingua) %></span>
                            </em>
                            <div class="clearfix"></div>

                            <b class="product-price pull-right" runat="server" visible='<%# VerificaPresenzaPrezzo(Eval("Prezzo"))%>'>
                                <div style="font-weight: 500; font-size: 1.4em; padding-right: 0px;text-align:right" itemprop="offers" itemscope="" itemtype="http://schema.org/Offer">
                                    <meta itemprop="price" content='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:##,###.00}", Eval("Prezzo")) + " €"%>'>
                                    <meta itemprop="priceCurrency" content="EUR">
                                    <asp:Literal ID="Literal4" runat="server"  Text='<%#  ImpostaIntroPrezzo(Eval("CodiceTipologia").ToString()) + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:##,###.00}", Eval("Prezzo")) + " €"%>'></asp:Literal><br />
                                    <span style="text-decoration: line-through; font-size: 0.9em; color: #aaa; padding-left: 0px; padding-right: 0px">
                                        <asp:Literal ID="lblPrezzoListino" runat="server" Visible='<%# VerificaPresenzaPrezzo(Eval("Prezzolistino"))%>'
                                            Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:##,###.00}", Eval("Prezzolistino")) + " €"%>'></asp:Literal>
                                    </span>
                                    <%#  Resources.Common.TitoloPrezzounita %>
                                </div>
                            </b>
                            <div class="clearfix"></div>
                            <div class="pull-right"  style="font-size: 0.9rem; color: #1f809f; padding-right: 10px">
                              <em>  <%# Resources.Basetext.Proprietario %>
                                 <span><%# TestoCaratteristica(2, Eval("Caratteristica3").ToString(), Lingua)%></span></em>
                            </div>
                            <meta itemprop="availability" content="in_stock">
                            <div style="width: 100%; margin-top: 10px; overflow: hidden;" runat="server"
                                visible='<%# ControlloVisibilita(Eval("FotoCollection_M"))%>'>


                                <div class="flexslider" data-transition="slide" data-slidernav="auto" id="scheda-slider" style="width: 100%; overflow: hidden; margin-bottom: 10px; margin-top: 0px;">
                                    <div class="slides" runat="server" id="divFlexScheda">
                                        <%# CreaSlide(Container.DataItem,0,600) %>
                                    </div>
                                </div>

                            </div>
                            <asp:Panel runat="server" Visible='<%# ControlloVisibilitaMiniature(Eval("FotoCollection_M"))  %>'>
                                <div class="flexslider" id="carousel-scheda-slider" style="height: 80px; padding-top: 0px; padding-bottom: 0px; overflow: hidden">
                                    <ul class="slides">
                                        <%# CreaSlideNavigation(Container.DataItem,0,0) %>
                                    </ul>
                                    <div style="clear: both"></div>
                                </div>
                            </asp:Panel>

                            <div class="clearfix"></div>
                                       <!-- Go to www.addthis.com/dashboard to customize your tools -->
              <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-58fdc705ad8f59e1"></script>
                            <!-- Go to www.addthis.com/dashboard to customize your tools -->
                            <div class="addthis_inline_share_toolbox"></div>




                            <%--  <div class="blog-post-details" runat="server" id="divPostDetails" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString()) || !string.IsNullOrEmpty(ControlloVuotoPosizione(Eval("CodiceComune").ToString(), Eval("CodiceProvincia").ToString(), Eval("CodiceTipologia").ToString(), Lingua))
                                    || VerificaPresenzaPrezzo(Eval("Prezzo"))%>'>
                                <div style="padding-left: 0px" class="blog-post-details-item blog-post-details-item-left" runat="server" visible='<%# VerificaPresenzaPrezzo(Eval("Prezzo"))%>'>
                                    <span style="font-weight: 800; font-size: 1.9em;">
                                        <asp:Literal ID="Literal15" runat="server"
                                            Text='<%#  ImpostaIntroPrezzo(Eval("CodiceTipologia").ToString()) + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:##,###.00}", Eval("Prezzo")) + " €<br/><br/>"%>'></asp:Literal></span>
                                </div>
                            </div>--%>

                            <div class="blog-post-tags" style="display: block">

                                <%--   <asp:LinkButton runat="server" ID="btnSottrai"
                                    OnClick="btnDecrement" class="btn" CommandArgument='<%# Eval("Id")%>'>-</asp:LinkButton>
                                <input runat="server" class="form-control text-center" id="txtQuantita" type="text" value='<%# CaricaQuantitaNelCarrello(Request, Session, Eval("Id").ToString())%>'>
                                <asp:LinkButton runat="server" ID="btnAggiungi" runat="server"
                                    OnClick="btnIncrement" class="btn" CommandArgument='<%# Eval("Id")%>'>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" Text="<%$ Resources:Common,testoAggiornacarrello %>" class="btn-u btn-u-dark" OnClick="btnUpdateCart" CommandArgument='<%# Eval("Id")%>'>
                                </asp:LinkButton>--%>
                            </div>
                            <%--   <asp:UpdatePanel runat="server">
                                <ContentTemplate>--%>
                            <!--end:.product-header-->
                            <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>

                            <%--  <%# CreaRigheDettaglio(Container.DataItem) %>--%>
                            <div class="clearfix"></div>
                            <div class="blog-post-body">
                                <%--INSERIAMO I PARAMETRI DEL PRODOTTO--%>
                                <p>
                                    <asp:Label ID="lbldescri" runat="server" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(CommonPage.ReplaceLinks(Eval("Descrizione" + Lingua).ToString()))%>'></asp:Label>
                                </p>
                                <p>
                                    <asp:Label ID="lbldatite" runat="server" Text='<%#  WelcomeLibrary.UF.Utility.SostituisciTestoACapo(CommonPage.ReplaceLinks(Eval("Datitecnici" + Lingua).ToString()))%>'></asp:Label>
                                </p>
                            </div>
                            <%--  <%# CreaRigheOpzioni(Container.DataItem)%>--%>

                            <div class="row">
                                <div class="col-sm-4">
                                    <div runat="server" id="divContact" visible='<%# AttivaContatto(Eval("Abilitacontatto"))%>'>
                                        <a id="A1" runat="server" href='<%# "~/Aspnetpages/Content_Tipo3.aspx?idOfferta=" + Eval("Id").ToString() + "&TipoContenuto=Richiesta" + "&Lingua=" + Lingua%>'
                                            target="_blank" title="" class="buttonstyle">
                                            <%= ImpostaTestoRichiesta()%>
                                        </a>
                                    </div>

                                </div>

                                <div class="col-sm-4">
                                    <%= "<a class=\"buttonstyle\"  target=\"_blank\" href=\"" + PercorsoAssolutoApplicazione + "/aspnetpages/SchedaOffertaStampa.aspx?idOfferta=" + idOfferta + "&CodiceTipologia=" + CodiceTipologia + "&Lingua=" + Lingua
            + "\"><i class=\"fa fa-print\"></i>&nbsp;Stampa</a>"%>
                                </div>
                                <div class="col-sm-4">
                                    <%= "<a class=\"buttonstyle\"    href=\"" + GeneraBackLink() + "\"><i class=\"fa fa-reply-all\"></i>&nbsp;" + Resources.Common.testoIndietro + "</a>"%>
                                </div>

                            </div>


                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div class="ui-15" runat="server" id="div1" visible="true">
                <div class="ui-content">
                    <div class="container-fluid">
                        <div class="row" style="padding-right: inherit">
                            <div class="col-md-12 col-sm-12 ui-padd">
                                <!-- Ui Form -->
                                <div class="ui-form">
                                    <!-- Heading -->
                                    <h3 class="h3-sidebar-title sidebar-title">
                                        <%= Resources.Common.TestoDisponibilita %>
                                    </h3>
                                    <!-- Form -->


                                    <!-- UI Input -->
                                    <div class="ui-input">
                                        <!-- Input Box -->
                                        <input class="form-control" type="text" name="uname" validationgroup="contattilateral" placeholder="Nome" runat="server" id="txtContactName" />
                                        <label class="ui-icon"><i class="fa fa-user"></i></label>
                                    </div>
                                    <div class="ui-input">
                                        <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Cognome" runat="server" id="txtContactCognome" />
                                        <label class="ui-icon"><i class="fa fa-user"></i></label>
                                    </div>
                                    <div class="ui-input">
                                        <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Telefono" runat="server" id="txtContactPhone" />
                                        <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                    </div>
                                    <div class="ui-input">
                                        <input type="text" class="form-control" name="unname" validationgroup="contattilateral" placeholder="Email" runat="server" id="txtContactEmail" />
                                        <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                    </div>
                                    <div class="ui-input">
                                        <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contattilateral" placeholder="Messaggio .." runat="server" id="txtContactMessage" />
                                    </div>


                                    <button id="Button1" class="btn btn-blue btn-lg btn-block" runat="server" validationgroup="contattilateral" onserverclick="btnContatti_Click"><%= Resources.Common.TestoInvio %></button>
                                    <asp:CheckBox ID="chkContactPrivacy" runat="server" Style="font-weight: 300; font-size: 10px" Checked="true" Text="Acconsento al trattamento dei miei dati personali (D.Lgs 196/2003) " />
                                    <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto2Err %>" ValidationGroup="contattilateral" ControlToValidate="txtContactName" runat="server" />
                                    <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto16lErr %>" ValidationGroup="contattilateral" ControlToValidate="txtContactCognome" runat="server" />
                                    <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto4Err %>" ValidationGroup="contattilateral" ControlToValidate="txtContactEmail" runat="server" />
                                    <div style="font-weight: 300; font-size: 10px; color: red">
                                        <asp:Literal Text="" ID="outputContact" runat="server" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="clearfix"></div>
       

            <%--SUGGERITI--%>
            <hr />
            <div class="row" runat="server" id="divSuggeriti" style="margin-bottom: 15px" visible="false">

                <div class="headline">
                    <h2>
                        <%= (CodiceTipologia == "rif000100" || CodiceTipologia == "rif000101") ? GetGlobalResourceObject("Common", "titoloCollegati").ToString() : GetGlobalResourceObject("Common", "titoloCatalogoConsigliati").ToString()%>
                    </h2>
                </div>

                <asp:Repeater ID="rptArticoliSuggeriti" runat="server"
                    ViewStateMode="Enabled">
                    <ItemTemplate>
                        <div class="col-sm-4" style="padding-left: 0px;">
                            <div class="blog-post">

                                <h4 style="margin-bottom: 5px">
                                    <a id="a4" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                        href='<%# CreaLinkRoutes(Session, false, Lingua, CleanUrl(Eval("Denominazione" + Lingua).ToString()), Eval("Id").ToString(), Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString())%>'
                                        target="_self" title='<%# CleanInput(ConteggioCaratteri(Eval("Denominazione" + Lingua).ToString(), 300, true))%>'>
                                        <asp:Literal ID="Literal1" Text='<%# estraititolo(Eval("Denominazione" + Lingua))%>'
                                            runat="server"></asp:Literal><br />
                                        <i style="font-size: 70%">
                                            <asp:Literal ID="Literal8" Text='<%# estraisottotitolo(Eval("Denominazione" + Lingua))%>'
                                                runat="server"></asp:Literal>></i></a>
                                </h4>

                                <%-- <i><a id="a2" runat="server" onclick="javascript:JsSvuotaSession(this)" style="font-size:0.9em"
                                    href='<%# CreaLinkRoutes(Session, false, Lingua, CleanUrl(Eval("Denominazione" + Lingua).ToString()), Eval("Id").ToString(), Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString())%>'
                                    target="_self" title='<%# CleanInput(ConteggioCaratteri(Eval("Denominazione" + Lingua).ToString(), 300, true))%>'>
                                    <asp:Literal ID="Literal8" Text='<%# estraisottotitolo(Eval("Denominazione" + Lingua))%>'
                                        runat="server"></asp:Literal></a></i>--%>

                                <div class="blog-post-tags">
                                    <ul class="list-unstyled list-inline blog-info" style="margin-bottom: 0px">
                                        <li runat="server" visible='<%# VerificaPresenzaPrezzo(Eval("Prezzo"))%>'>
                                            <span style="font-weight: 800; font-size: 1.5em; color: #f56f28">
                                                <asp:Literal ID="Literal5" runat="server"
                                                    Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"), "{0:##,###.00}", Eval("Prezzo")) + " €"%>'></asp:Literal>
                                            </span>
                                        </li>
                                        <li runat="server" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString())%>'>

                                            <asp:Literal ID="Literal2" Text="pubblicato il " runat="server" /><asp:Literal ID="Literal3"
                                                Text='<%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento")) + TestoSezione(Eval("CodiceTipologia").ToString())%>'
                                                runat="server" /></li>
                                        <li>
                                            <asp:Literal ID="litPosizione" runat="server"
                                                Text='<%# ControlloVuotoPosizione(Eval("CodiceComune").ToString(), Eval("CodiceProvincia").ToString(), Eval("CodiceTipologia").ToString(), Lingua)%>'></asp:Literal>
                                        </li>

                                    </ul>
                                </div>
                                <br />
                                <div class="blog-post-featured-img img-overlay" runat="server"
                                    style="max-height: 200px; overflow: hidden; margin-bottom: 0px" runat="server" visible='<%#  !ControlloVideo(Eval("FotoCollection_M.FotoAnteprima"))%>'>
                                    <a id="a3" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                        href='<%# CreaLinkRoutes(Session, false, Lingua, CleanUrl(Eval("Denominazione" + Lingua).ToString()), Eval("Id").ToString(), Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString())%>'
                                        target="_self" title='<%# CleanInput(ConteggioCaratteri(Eval("Denominazione" + Lingua).ToString(), 300, true))%>'>
                                        <asp:Image ID="Anteprima" runat="server" class="img-responsive"
                                            ImageUrl='<%#  ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"), Eval("CodiceTipologia").ToString(), Eval("Id").ToString())%>'
                                            Visible='<%#  !ControlloVideo(Eval("FotoCollection_M.FotoAnteprima"))%>' /></a>
                                    <div class="item-img-overlay">
                                        <a id="a8" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                            href='<%# CreaLinkRoutes(Session, false, Lingua, CleanUrl(Eval("Denominazione" + Lingua).ToString()), Eval("Id").ToString(), Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString())%>'
                                            target="_self" class="portfolio-zoom" title='<%# CleanInput(ConteggioCaratteri(Eval("Denominazione" + Lingua).ToString(), 300, true))%>'></a>
                                    </div>
                                </div>
                                <div class="blog-post-featured-img img-overlay" runat="server" style="max-height: 100px; overflow: hidden; margin-bottom: 0px">
                                    <div class="responsive-video" id="divVideo" runat="server" visible='<%#  ControlloVideo(Eval("FotoCollection_M.FotoAnteprima"), Eval("linkVideo"))%>'>
                                        <iframe id="Iframe2" src='<%#  SorgenteVideo(Eval("linkVideo"))%>'
                                            visible='<%#  ControlloVideo(Eval("FotoCollection_M.FotoAnteprima"))%>'
                                            runat="server" frameborder="0" allowfullscreen></iframe>
                                    </div>
                                </div>
                                <br />


                            </div>
                        </div>
                        <%= SeparaRows()%>
                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
        <div class="col-md-3 col-sm-3" runat="server" id="RightColumn">
            <div class="sidebar sticker">
                <!-- Sidebar Block -->
                <div class="sidebar-block" runat="server" visible="false">
                    <div class="sidebar-content tags blog-search">
                        <div class="input-group">
                            <input class="form-control blog-search-input text-input" name="q" type="text" placeholder="<%$ Resources:Common,TestoCercaBlog %>" runat="server" id="inputCerca" />
                            <span class="input-group-addon">
                                <button onserverclick="Cerca_Click" id="BtnCerca" class="blog-search-button icon-search" runat="server" clientidmode="Static" />
                            </span>
                        </div>
                    </div>
                </div>
                <!-- Sidebar Block -->

                <div class="sidebar-block" runat="server" id="divContact" visible="true">
                    <div class="sidebar-content">
                        <div class="ui-15">
                            <div class="ui-content">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 ui-padd">
                                            <!-- Ui Form -->
                                            <div class="ui-form">
                                                <!-- Heading -->
                                                <h3 class="h3-sidebar-title sidebar-title">
                                                    <%= Resources.Common.TestoDisponibilita %>
                                                </h3>
                                                <!-- Form -->
                                                <!-- UI Input -->
                                                <div class="ui-input">
                                                    <!-- Input Box -->
                                                    <input class="form-control" type="text" name="uname" validationgroup="contattilateral1" placeholder="Nome" runat="server" id="txtContactName1" />
                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral1" placeholder="Cognome" runat="server" id="txtContactCognome1" />
                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral1" placeholder="Telefono" runat="server" id="txtContactPhone1" />
                                                    <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input type="text" class="form-control" name="unname" validationgroup="contattilateral1" placeholder="Email" runat="server" id="txtContactEmail1" />
                                                    <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contattilateral1" placeholder="Messaggio .." runat="server" id="txtContactMessage1" />
                                                </div>
                                                <button id="btnFormContatto" class="btn btn-brown btn-lg btn-block" runat="server" validationgroup="contattilateral1" onserverclick="btnContatti1_Click"><%= Resources.Common.TestoInvio %></button>
                                                <div style="clear: both"></div>
                                                <asp:CheckBox ID="chkContactPrivacy1" runat="server" Style="font-weight: 300; font-size: 10px" Checked="true" Text="Acconsento al trattamento dei miei dati personali (D.Lgs 196/2003) " />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto2Err %>" ValidationGroup="contattilateral1" ControlToValidate="txtContactName1" runat="server" />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto16lErr %>" ValidationGroup="contattilateral1" ControlToValidate="txtContactCognome1" runat="server" />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto4Err %>" ValidationGroup="contattilateral1" ControlToValidate="txtContactEmail1" runat="server" />
                                                <div style="font-weight: 300; font-size: 10px; color: red">
                                                    <asp:Literal Text="" ID="outputContact1" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Sidebar Block -->
                <div class="sidebar-block" runat="server" id="divLatestPost" visible="false">
                    <h3 class="h3-sidebar-title sidebar-title">
                        <%--<%= TestoSezione(CodiceTipologia, false, true)%>--%>
                        <%= Resources.Common.titoloNewsultime %>
                    </h3>
                    <div class="sidebar-content">
                        <ul class="posts-list">
                            <asp:Repeater ID="rtpLatestPost" runat="server"
                                ViewStateMode="Enabled">
                                <ItemTemplate>
                                    <li>
                                        <div class="posts-list-thumbnail">
                                            <a id="a5" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                                href='<%# CreaLinkRoutes(Session, false, Lingua, CleanUrl(Eval("Denominazione" + Lingua).ToString()), Eval("Id").ToString(), Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString())%>'
                                                target="_self" title='<%# CleanInput(ConteggioCaratteri(Eval("Denominazione" + Lingua).ToString(), 300, true))%>'>
                                                <asp:Image ID="Anteprima" runat="server" Style="max-width: 55px; max-height: 55px"
                                                    ImageUrl='<%#  ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"), Eval("CodiceTipologia").ToString(), Eval("Id").ToString())%>'
                                                    Visible='<%#  !ControlloVideo(Eval("FotoCollection_M.FotoAnteprima"))%>' />
                                            </a>
                                        </div>
                                        <div class="posts-list-content">
                                            <a class="posts-list-title" id="a1" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                                href='<%# CreaLinkRoutes(Session, false, Lingua, CleanUrl(Eval("Denominazione" + Lingua).ToString()), Eval("Id").ToString(), Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString())%>'
                                                target="_self" title='<%# CleanInput(ConteggioCaratteri(Eval("Denominazione" + Lingua).ToString(), 300, true))%>'>
                                                <asp:Literal ID="litTitolo" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(Eval("Denominazione" + Lingua).ToString())%>'
                                                    runat="server"></asp:Literal></a>
                                            <div class="posts-list-meta" runat="server" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString())%>'>
                                                <%-- <asp:Literal ID="lblBrDesc" Text='<%#  ReplaceLinks(WelcomeLibrary.UF.Utility.SostituisciTestoACapo( ConteggioCaratteri(   Eval("Descrizione" + Lingua).ToString() ,200,true  )  ) , false) %>'
                                            runat="server"></asp:Literal>--%>
                                                <i class="fa fa-calendar"></i>
                                                <asp:Literal ID="Literal2" Text="<%$ Resources:Common,TestoPubblicatodata  %>" runat="server" /><asp:Literal ID="Literal3"
                                                    Text='<%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento")) + TestoSezione(Eval("CodiceTipologia").ToString())%>'
                                                    runat="server" />
                                            </div>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div id="divScrollerSuggeritiJsTitle" class="row" style="display: none; margin: 10px;">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="subtitle-block clearfix">
                        <div class="row" style="text-align: left; padding-bottom: 0px; padding-top: 30px; margin-bottom: 0px; line-height: 40px; color: #33332e; border-bottom: 1px solid #33332e">
                            <div class="headline pull-left">
                                <h2>
                                    <%= (CodiceTipologia=="rif000001" ) ?  GetGlobalResourceObject("Common", "titoloCollegati").ToString(): GetGlobalResourceObject("Common", "titoloCatalogoConsigliati").ToString() %>
                                </h2>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div id="divScrollerSuggeritiJs"></div>
    </div>

    <%--   <div class="row" style="margin-top: 20px">
            <div class="pull-right">
                <div id="fb-root"></div>
                <script type="text/javascript">(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/it_IT/all.js#xfbml=1&appId=1452481705030868";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
                </script>
                <div class="fb-comments" data-width="600" data-num-posts="5" runat="server" id="divComments"></div>
            </div>
        </div>--%>


    <script type="text/javascript">
        function goBack() {
            window.history.back()
        }
        $(document).ready(function () {
            inizializzaFlexsliderScheda();
        });
        function inizializzaFlexsliderScheda() {
            //Plugin: flexslider con funzione di animazione dei messaggi o oggetti sopra
            // ------------------------------------------------------------------
            if ($('#scheda-slider') != null)
                $('#scheda-slider').each(function () {
                    var sliderSettings = {
                        animation: $(this).attr('data-transition'),
                        easing: "swing",
                        selector: ".slides > .slide",
                        smoothHeight: false,
                        // controlsContainer: ".flex-container",
                        animationLoop: false,             //Boolean: Should the animation loop? If false, directionNav will received "disable" classes at either end
                        slideshow: false,                //Boolean: Animate slider automatically
                        //   slideshowSpeed: 4000,           //Integer: Set the speed of the slideshow cycling, in milliseconds
                        //  animationSpeed: 600,            //Integer: Set the speed of animations, in milliseconds

                        // Usability features
                        pauseOnAction: true,            //Boolean: Pause the slideshow when interacting with control elements, highly recommended.
                        pauseOnHover: true,            //Boolean: Pause the slideshow when hovering over slider, then resume when no longer hovering
                        useCSS: true,                   //{NEW} Boolean: Slider will use CSS3 transitions if available
                        touch: true,                    //{NEW} Boolean: Allow touch swipe navigation of the slider on touch-enabled devices
                        video: false,                   //{NEW} Boolean: If using video in the slider, will prevent CSS3 3D Transforms to avoid graphical glitches
                        sync: "#carousel-scheda-slider",

                        // Primary Controls
                        // controlNav: "thumbnails",       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                        controlNav: false,       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                        directionNav: true,             //Boolean: Create navigation for previous/next navigation? (true/false)
                        prevText: "",           //String: Set the text for the "previous" directionNav item
                        nextText: "",               //String: Set the text for the "next" directionNav item

                        start: function (slider) {
                            //hide all animated elements
                            slider.find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });

                            //animate in first slide
                            slider.find('.slide').eq(1).find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                                if ($(this).data('animate-delay')) {
                                    $(this).addClass($(this).data('animate-delay'));
                                }
                                if ($(this).data('animate-duration')) {
                                    $(this).addClass($(this).data('animate-duration'));
                                }
                                $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                                $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                                  function () {
                                      $(this).removeClass($(this).data('animate-in'));
                                  }
                                );
                            });
                        },
                        before: function (slider) {
                            //hide next animate element so it can animate in
                            slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });
                        },
                        after: function (slider) {
                            //hide animtaed elements so they can animate in again
                            slider.find('.slide').find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });

                            //animate in next slide
                            slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                                if ($(this).data('animate-delay')) {
                                    $(this).addClass($(this).data('animate-delay'));
                                }
                                if ($(this).data('animate-duration')) {
                                    $(this).addClass($(this).data('animate-duration'));
                                }
                                $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                                $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                                  function () {
                                      $(this).removeClass($(this).data('animate-in'));
                                  }
                                );
                            });
                            /* auto-restart player if paused after action */
                            if (!slider.playing) {
                                slider.play();
                            }

                        }
                    };

                    var sliderNav = $(this).attr('data-slidernav');
                    if (sliderNav !== 'auto') {
                        sliderSettings = $.extend({}, sliderSettings, {
                            //  controlsContainer: '.flex-container',
                            manualControls: sliderNav + ' li a'

                        });
                    }

                    $('html').addClass('has-flexslider');
                    $(this).flexslider(sliderSettings);
                });
            //    $('#scheda-slider').resize(); //make sure height is right load assets loaded
            if ($('#carousel-scheda-slider') != null)
                $('#carousel-scheda-slider').flexslider({
                    animation: "slide",
                    controlNav: false,
                    animationLoop: false,
                    slideshow: false,
                    itemWidth: 120,
                    itemHeight: 80,
                    itemMargin: 5,
                    asNavFor: '#scheda-slider'
                });

        }

    </script>
</asp:Content>
