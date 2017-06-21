<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="RisultatiRicerca.aspx.cs" Inherits="AspNetPages_RisultatiRicerca" Title="" Culture="it-IT"
    MaintainScrollPositionOnPostback="false" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>--%>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <%-- <!--=== Breadcrumbs ===-->
        <div class="breadcrumbs">
            <div class="container">
                <h1 class="pull-left">Login</h1>
                <p></p>
                <ul class="pull-right breadcrumb">
                    <li><a href='<%# references.ResMan("Common", Lingua,"LinkHome") %>' runat="server">
                        <asp:Literal Text='<%# references.ResMan("Common", Lingua,"testoHome" %>' runat="server" /></a></li>
                    <li class="active">da creare link pagina</li>
                </ul>
            </div>
        </div>
        <!--/breadcrumbs-->
        <!--=== End Breadcrumbs ===-->--%>
    <script type="text/javascript">
        makeRevLower = true;
    </script>
    <div class="container" style="text-align: center; margin-top: 10px">
        <div class="row" runat="server" id="divTitle">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12">
                  <h1 class="title-block"  style="line-height:normal;">
                    <asp:Literal Text="" runat="server" ID="litNomePagina" /></h1>
            </div>
        </div>
    </div>   
    <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 100%; padding: 0; margin: 0" runat="server" id="divContenutiPortfolioRival1" visible="false">
        <div class="container">
            <%--PORTFOLIO CONTENTS 1 ELEMENTS--%>
            <%--<div class="row" style="margin-bottom: 15px; margin-top: 15px;">
                <div class="col-xs-12" style="text-align: center; font-weight: 500; font-size: 1.6em; padding-bottom: 5px; padding-top: 5px; text-transform: uppercase">
                    <asp:Literal Text="" runat="server" ID="litNomeContenuti1" />
                </div>
            </div>--%>
            <div class="row" style="margin-bottom: 20px; margin-top: 20px">
                <div class=" col-sm-12">
                    <ul class="works-grid works-grid-gut works-grid-4 works-hover-lw">
                        <asp:Literal ID="litPortfolioRivals3" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>
            <div class="row" style="margin-bottom: 20px; margin-top: 20px">
                <div class="col-sm-12">
                    <ul class="works-grid works-grid-gut works-grid-3 works-hover-lw">
                        <asp:Literal ID="litPortfolioRivals3b" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div style="width: 100%; padding: 0px; margin: 0px" runat="server" id="divContenutiPortfolio4" visible="false">
        <div class="container">
            <%--PORTFOLIO CONTENTS 4 ELEMENTS--%>
            <%--<div class="row" style="margin-bottom: 15px; margin-top: 15px;">
                <div class="col-xs-12" style="text-align: center; font-weight: 500; font-size: 1.6em;padding-bottom: 5px; padding-top: 5px; text-transform: uppercase">
                    <asp:Literal Text="" runat="server" ID="litNomeContenuti4" />
                </div>
            </div>--%>
            <div class="row" style="margin-bottom: 20px; margin-top: 20px">
                <div class="col-md-12 col-sm-12">
                    <div class="portfolio-items portfolio-items-cols4">
                        <asp:Literal ID="litContenutiPortfolio4" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="width: 100%; padding: 0px; margin: 0px" runat="server" id="divContenutiPortfolio1" visible="false">
        <div class="container">
            <%--PORTFOLIO CONTENTS 1 ELEMENTS--%>
            <%--<div class="row" style="margin-bottom: 15px; margin-top: 15px;">
                <div class="col-xs-12" style="text-align: center; font-weight: 500; font-size: 1.6em; padding-bottom: 5px; padding-top: 5px; text-transform: uppercase">
                    <asp:Literal Text="" runat="server" ID="litNomeContenuti1" />
                </div>
            </div>--%>
            <div class="row" style="margin-bottom: 20px; margin-top: 50px">
                <div class="col-md-12 col-sm-12">
                    <div class="portfolio-items portfolio-items-cols1">
                        <asp:Literal ID="litContenutiPortfolio1" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="width: 100%; padding: 0px; margin: 0px" runat="server" id="divContenutiPortfolio2" visible="false">
        <div class="container">
            <%--PORTFOLIO CONTENTS 1 ELEMENTS--%>
            <%--<div class="row" style="margin-bottom: 15px; margin-top: 15px;">
                <div class="col-xs-12" style="text-align: center; font-weight: 500; font-size: 1.6em;   padding-bottom: 5px; padding-top: 5px; text-transform: uppercase">
                    <asp:Literal Text="" runat="server" ID="litNomeContenuti2" />
                </div>
            </div>--%>
            <div class="row" style="margin-bottom: 20px; margin-top: 50px">
                <div class="col-md-12 col-sm-12">
                    <div class="portfolio-items portfolio-items-cols2">
                        <asp:Literal ID="litContenutiPortfolio2" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div style="width: 100%; padding: 0px; margin: 0px" runat="server" id="divContenutiGallery" visible="false">
        <div class="row" style="margin-bottom: 20px; margin-top: 20px">
            <div class="col-sm-12">
                <ul class="works-grid works-grid-gut works-grid-3 works-hover-lw">
                    <asp:Literal Text="" ID="litGalleryDetails" runat="server" />
                </ul>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
    <div class="col-md-1 col-sm-1" runat="server" id="column1" visible="false">
        <%--<div id="divPortfolioList1Title" class="row" style="display: none">
            <div class="col-sm-12">
                <div class="subtitle-block clearfix">
                    <div class="row" style="text-align: left; padding-bottom: 0px; padding-top: 30px; margin-bottom: 0px; line-height: 40px; color: #33332e; border-bottom: 1px solid #33332e">
                        <%= Master.TestoSezione(Tipologia,false,true)  %>
                    </div>
                </div>
            </div>
        </div>--%>
        <div id="divPortfolioList1"></div>
        <div id="divPortfolioList1Pager"></div>
    </div>
    <div class="col-md-9 col-sm-9" runat="server" id="column2">

        <div class="row" style="display: none">
            <div class="col-md-6">
                <asp:Button ID="btnNexth"
                    Text='<%# references.ResMan("Common", Lingua,"txtTastoNext") %>' class="button btn-flat"
                    runat="server" OnClick="btnNext_click" />
            </div>
            <div class="col-md-6">
                <div class="pull-right">
                    <asp:Button ID="btnPrevh" class="button btn-flat" Text='<%# references.ResMan("Common", Lingua,"txtTastoPrev") %>'
                        runat="server" OnClick="btnPrev_click" />
                </div>
            </div>
        </div>
        <div style="width: 100%; padding: 0px; margin: 0px" runat="server" id="divRivalPortfolio2" visible="false">

            <div class="row" style="margin-bottom: 20px; margin-top: 20px">
                <div class="col-sm-12">
                    <ul class="works-grid works-grid-gut works-grid-2 works-hover-lw">
                        <asp:Literal ID="litPortfolioRivals2" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>
        </div>
        <asp:Repeater ID="rptOfferte" runat="server">
            <ItemTemplate>
                <div class="blog-post" style="text-align: justify;">
                    <div class="blog-span">

                        <h2 style="margin-bottom: 5px">
                            <a id="a4" runat="server"
                                href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                <asp:Literal ID="Literal9" Text='<%# estraititolo(  Eval("Denominazione" + Lingua) ) %>'
                                    runat="server"></asp:Literal><br />
                                <span style="font-size: 70%">
                                    <asp:Literal ID="Literal8" Text='<%# estraisottotitolo(  Eval("Denominazione" + Lingua) ) %>'
                                        runat="server"></asp:Literal></span></a>
                        </h2>

                        <div class="blog-post-details">
                            <div class="blog-post-details-item blog-post-details-item-left icon-calendar" runat="server" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString()) %>'>
                                <asp:Literal ID="Literal2" Text='<%# references.ResMan("Common", Lingua,"TestoPubblicatodata") %>' runat="server" /><asp:Literal ID="Literal3"
                                    Text='<%# string.Format("{0:dd/MM/yyyy}", Eval("DataInserimento")) + TestoSezione(Eval("CodiceTipologia").ToString()) %>'
                                    runat="server" />
                            </div>
                            <div class="blog-post-details-item blog-post-details-item-left">
                                <asp:Literal ID="litPosizione" runat="server"
                                    Text='<%# ControlloVuotoPosizione( Eval("CodiceComune").ToString()  , Eval("CodiceProvincia").ToString(), Eval("CodiceTipologia").ToString(),Lingua ) %>'></asp:Literal>
                            </div>
                            <div class="blog-post-details-item blog-post-details-item-left" runat="server" visible='<%# VerificaPresenzaPrezzo( Eval("Prezzo") ) %>'>

                                <asp:Literal ID="Literal5" runat="server"
                                    Text='<%# "Prezzo: "+ String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("Prezzo")) + " €<br/><br/>" %>'></asp:Literal>
                            </div>
                        </div>
                        <div class="blog-post-featured-img img-overlay" runat="server" style="max-height: 450px"
                            visible='<%# ControlloVisibilita(Eval("FotoCollection_M")) %>'>
                            <a id="a3" runat="server" class="portfolio-zoom"
                                href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                <asp:Image ID="Anteprima" runat="server" class="img-responsive" ImageUrl='<%#  ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString()) %>' />
                                <div class="item-img-overlay">
                                    <div class="item_img_overlay_content">
                                        <div class="blog-post-details-item blog-post-details-item-left">
                                            <a id="a6" runat="server"
                                                href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                                <span>
                                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"testoContinua") %>' runat="server" /></span></a>
                                            <%-- <a href="#" class="icon-facebook"></a>
                                    <a href="#" class="icon-twitter-alt"></a>
                                    <a href="#" class="icon-google"></a>
                                    <a href="#" class="icon-email-mail-streamline"></a>--%>
                                        </div>
                                        <%--div class="blog-post-details-item blog-post-details-item-right share-article">
                                    <a href="#" class="icon-heart">25</a>
                                </div>--%>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="responsive-video" id="divVideo" runat="server"
                            visible='<%#  ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima"), Eval("linkVideo")  ) %>'>
                            <iframe id="Iframe2" src='<%#  SorgenteVideo(  Eval("linkVideo") ) %>'
                                runat="server" frameborder="0" allowfullscreen></iframe>
                        </div>


                        <div class="blog-post-body">
                            <asp:Literal ID="lblBrDesc" Text='<%#  ReplaceLinks(WelcomeLibrary.UF.Utility.SostituisciTestoACapo( ConteggioCaratteri(  Eval("Descrizione" + Lingua).ToString(),900,true )) , false) %>'
                                runat="server"></asp:Literal>
                        </div>

                        <!-- Read More -->
                        <div class="pull-right">
                            <a id="a8" runat="server"
                                href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                target="_self" title='<%# CleanInput( Eval("Denominazione" + Lingua).ToString()) %>'>
                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"testoContinua") %>' runat="server" />
                            </a>
                        </div>
                        <!-- //Read More// -->
                        <div class="clearfix"></div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Panel runat="server" ID="pnlPager" Visible="false">
            <div class="row">
                <div class="pull-left">
                    <asp:Button ID="btnPrev" class="button divbuttonstyle" Text='<%# references.ResMan("Common", Lingua,"txtTastoPrev") %>'
                        runat="server" OnClick="btnPrev_click" />
                </div>
                <div class="pull-left">
                    <div id="pager" class="text-center">
                        <UC:PagerEx ID="PagerRisultati" runat="server" NavigateUrl="" PageSize="21" CurrentPage="1"
                            TotalRecords="0" dimensioneGruppo="8" nGruppoPagine="1" OnPageCommand="PagerRisultati_PageCommand"
                            OnPageGroupClickNext="PagerRisultati_PageGroupClickNext" OnPageGroupClickPrev="PagerRisultati_PageGroupClickPrev" />
                    </div>

                </div>
                <div class="pull-left">
                    <asp:Button ID="btnNext" class="button divbuttonstyle" Text='<%# references.ResMan("Common", Lingua,"txtTastoNext") %>'
                        runat="server" OnClick="btnNext_click" />
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="col-md-3 col-sm-3" runat="server" id="column3">
        <div class="sidebar" runat="server" id="RightSidebar">
            <!-- Sidebar Block -->
            <div class="sidebar-block" runat="server" id="divSearch" visible="false">
                <div class="sidebar-content tags blog-search">
                    <div class="input-group">
                        <input class="form-control blog-search-input text-input" name="q" type="text" placeholder='<%# references.ResMan("Common", Lingua,"TestoCercaBlog") %>' runat="server" id="inputCerca" />
                        <span class="input-group-addon">
                            <button onserverclick="Cerca_Click" id="BtnCerca" class="blog-search-button fa fa-search" runat="server" clientidmode="Static" />
                        </span>
                    </div>
                </div>
            </div>
            <!-- Sidebar Block -->

            <div class="sidebar-block" id="divCategorie" runat="server" visible="false">
                <h3 class="h3-sidebar-title sidebar-title">Categorie
                </h3>
                <div class="sidebar-content">
                    <ul class="posts-list">
                        <asp:Repeater ID="rptContenutiLink" runat="server"
                            ViewStateMode="Enabled">
                            <ItemTemplate>
                                <li><i class="fa fa-check color-green"></i>
                                    <a id="linkRubriche" style="font-size: 14px" onclick="javascript:JsSvuotaSession(this)"
                                        href='<%# CreaLinkRoutes(Session,false,Lingua,CommonPage.CleanUrl(Eval("Descrizione").ToString()),"",Eval("Codice").ToString()) %>'
                                        runat="server">
                                        <asp:Label ID="Titolo" runat="server" Text='<%# Eval("Descrizione").ToString() %>'></asp:Label></a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="rptProdottiContenutiLink" runat="server"
                            ViewStateMode="Enabled">
                            <ItemTemplate>
                                <li>
                                    <a id="linkRubriche" onclick="javascript:JsSvuotaSession(this)" href='<%# CommonPage.CreaLinkRoutes(Session, false, Lingua, CommonPage.CleanUrl(Eval("Descrizione").ToString()), "", Eval("CodiceTipologia").ToString(), Eval("CodiceProdotto").ToString())%>'
                                        runat="server">
                                        <asp:Label ID="Titolo" runat="server" Text='<%# Eval("Descrizione").ToString()%>'></asp:Label>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
            <!-- Sidebar Block -->
            <div class="sidebar-block" runat="server" id="divLatestPost" visible="false">
                <h3 class="h3-sidebar-title sidebar-title"><%= TestoSezione(Tipologia, false, true) %>
                </h3>
                <div class="sidebar-content">
                    <ul class="posts-list">
                        <asp:Repeater ID="rtpLatestPost" runat="server"
                            ViewStateMode="Enabled">
                            <ItemTemplate>
                                <li>
                                    <div class="posts-list-thumbnail">
                                        <a id="a5" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                            href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                            target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                            <asp:Image ID="Anteprima" runat="server" Style="max-width: 55px; max-height: 55px"
                                                ImageUrl='<%#  ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString()) %>'
                                                Visible='<%#  !ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima") ) %>' />
                                        </a>
                                    </div>
                                    <div class="posts-list-content">
                                        <a class="posts-list-title" id="a1" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                            href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                            target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                            <asp:Literal ID="litTitolo" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(  Eval("Denominazione" + Lingua).ToString() ) %>'
                                                runat="server"></asp:Literal></a>
                                        <span class="posts-list-meta" runat="server" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString()) %>'>
                                            <%-- <asp:Literal ID="lblBrDesc" Text='<%#  ReplaceLinks(WelcomeLibrary.UF.Utility.SostituisciTestoACapo( ConteggioCaratteri(   Eval("Descrizione" + Lingua).ToString() ,200,true  )  ) , false) %>'
                                            runat="server"></asp:Literal>--%>
                                            <i class="fa fa-calendar"></i>
                                            <asp:Literal ID="Literal2" Text='<%# references.ResMan("Common", Lingua,"TestoPubblicatodata")  %>' runat="server" /><asp:Literal ID="Literal3"
                                                Text='<%# string.Format("{0:dd/MM/yyyy}", Eval("DataInserimento")) + TestoSezione(Eval("CodiceTipologia").ToString()) %>'
                                                runat="server" />
                                        </span>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
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


            <!-- Sidebar Block -->
            <div class="sidebar-block" runat="server" id="divArchivio" visible="false">
                <h3 class="h3-sidebar-title sidebar-title"><%= Resources.Common.TestoArchivio %>
                </h3>
                <div class="sidebar-content"  style="overflow-y: auto" id="divArchivioList">
                    <asp:Repeater ID="rptArchivio" runat="server">
                        <ItemTemplate>
                            <ul class="posts-list" id="ulAnno" runat="server" title='<%# Eval("Key").ToString() %>'>
                                <asp:Repeater ID="rptArchivioMesi" runat="server" DataSource='<%# Eval("Value") %>'
                                    OnItemDataBound="rptArchivioMesi_ItemDataBound">
                                    <ItemTemplate>
                                        <li style="border-top: 1px dotted #e5e5e5; margin-top: 10px; padding-top: 10px">
                                            <i class="fa fa-check color-green"></i>
                                            <a id="alink" runat="server" href="#" target="_self">(<asp:Literal ID="NumeroElementi"
                                                runat="server" Text='<%# Eval("Value").ToString() %>'></asp:Literal>)</a>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </ItemTemplate>
                    </asp:Repeater>

                </div>
            </div>
            <!-- Sidebar Block -->
        </div>
    </div>
</asp:Content>
