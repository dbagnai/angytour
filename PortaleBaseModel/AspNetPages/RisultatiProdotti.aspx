<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="RisultatiProdotti.aspx.cs" Inherits="AspNetPages_RisultatiProdotti" Title="" Culture="it-IT"
    MaintainScrollPositionOnPostback="false" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>--%>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <script type="text/javascript">
        makeRevLower = true;
    </script>
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
    <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
    <script type="text/javascript">
        var makeRevLower = true;
    </script>
    <div style="max-width:1800px;margin:0px auto">
    <div id="divPortfolioList1"></div>
    <div id="divPortfolioList1Pager"></div>
    </div>
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField runat="server" ID="hddTagCombined" ClientIDMode="Static" />


    <div style="width: 100%; padding: 0px; margin: 0px" runat="server" id="divContenutiPortfolioRival1" visible="false">
        <div class="container">
            <%--PORTFOLIO CONTENTS 1 ELEMENTS--%>
            <%--<div class="row" style="margin-bottom: 15px; margin-top: 15px;">
                <div class="col-xs-12" style="text-align: center; font-weight: 500; font-size: 1.6em; padding-bottom: 5px; padding-top: 5px; text-transform: uppercase">
                    <asp:Literal Text="" runat="server" ID="litNomeContenuti1" />
                </div>
            </div>--%>
            <%-- <div class="row" style="margin-bottom: 20px; margin-top: 50px">
                <div class="col-sm-offset-3 col-sm-6">
                    <ul  class="works-grid works-grid-gut works-grid-3 works-hover-w">
                        <asp:Literal ID="litPortfolioRivals3" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>--%>
            <div class="row" style="margin-bottom: 20px; margin-top: 20px">
                <div class="col-sm-12">
                    <ul class="works-grid works-grid-gut works-grid-3 works-hover-lw">
                        <asp:Literal ID="litPortfolioRivals3b" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div style="width: 100%; padding: 0px; margin: 0px" runat="server" id="divContenutiPortfolioRival2" visible="false">
        <div class="container">
            <div class="row" style="margin-bottom: 20px; margin-top: 20px">
                <div class="col-sm-12">
                    <ul class="works-grid works-grid-gut works-grid-2 works-hover-lw">
                        <asp:Literal ID="litPortfolioRivals3" runat="server"></asp:Literal>
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
            <div class="row" style="margin-bottom: 20px; margin-top: 20px">
                <div class="col-md-12 col-sm-12">
                    <div class="portfolio-items portfolio-items-cols2">
                        <asp:Literal ID="litContenutiPortfolio2" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="col-md-9 col-sm-9">
        <div class="sorting-block">
            <ul class="row sorting-grid">
                <asp:Repeater ID="rptProdotti" runat="server" ViewStateMode="Enabled">
                    <ItemTemplate>

                        <li class="col-md-4 col-sm-4 col-xs-12">

                            <a id="a3" runat="server"
                                href='<%# CreaLinkRoutes(Session,false,Lingua,CommonPage.CleanUrl(Eval("Denominazione" + Lingua).ToString()),   Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                <div style="max-height: 130px; overflow: hidden">
                                    <%# TestoCategoria(Eval("CodiceTipologia").ToString(),Eval("CodiceCategoria").ToString(),Lingua) %>
                                    <asp:Image ID="Anteprima" class="img-responsive"
                                        AlternateText='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'
                                        runat="server"
                                        ImageUrl='<%#  ComponiUrl(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString()) %>'
                                        Visible='<%# ControlloVisibilita(Eval("FotoCollection_M")) %>' />
                                    <div class="responsive-video" id="divVideo" runat="server" visible='<%#  ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima"), Eval("linkVideo")  ) %>'>
                                        <iframe id="Iframe2" src='<%#  SorgenteVideo(  Eval("linkVideo") ) %>'
                                            runat="server" frameborder="0" allowfullscreen></iframe>
                                    </div>
                                </div>

                                <h2 style="margin-bottom: 5px">
                                    <a id="a1" runat="server"
                                        href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                        target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                        <asp:Literal ID="Literal1" Text='<%# estraititolo(  Eval("Denominazione" + Lingua) ) %>'
                                            runat="server"></asp:Literal><br />
                                        <span style="font-size: 70%">
                                            <asp:Literal ID="Literal2" Text='<%# estraisottotitolo(  Eval("Denominazione" + Lingua) ) %>'
                                                runat="server"></asp:Literal></span></a>
                                </h2>

                                <%--        <p>
                                <asp:LinkButton runat="server"
                                    class="btn btn-success pull-right" OnClick="btnInsertcart" CommandArgument='<%# Eval("Id") %>'>
                                   <asp:Literal text='<%# references.ResMan("Common", Lingua,"testoInseriscicarrello") %>' runat="server" />  <i class="icon-shopping-cart"></i>
                                </asp:LinkButton>
                                <asp:Literal ID="lblPrezzo" runat="server" Visible='<%# VerificaPresenzaPrezzo( Eval("Prezzo") ) %>'
                                    Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("Prezzo")) + " €" %>'></asp:Literal>
                                <asp:Literal ID="lblPrezzoListino" runat="server" Visible='<%# VerificaPresenzaPrezzo( Eval("Prezzolistino") ) %>'
                                    Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("Prezzolistino")) + " €" %>'></asp:Literal>
                            </p>--%>
                                <p>
                                    <%# TestoCaratteristica(2,Eval("Caratteristica3").ToString(),Lingua) %>
                                    <%# TestoCaratteristica(3,Eval("Caratteristica4").ToString(),Lingua) %>
                                    <%# TestoCaratteristica(4,Eval("Caratteristica5").ToString(),Lingua) %>
                                </p>
                            </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>

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
    </div>
    <div class="col-md-3 col-sm-3">
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
</asp:Content>
