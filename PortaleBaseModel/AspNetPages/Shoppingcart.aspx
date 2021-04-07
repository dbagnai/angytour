<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="Shoppingcart.aspx.cs" Inherits="AspNetPages_Shoppingcart" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <script type="text/javascript">
        var makeRevLower = true;
    </script>
    <div class="container d-none" style="text-align: center;" runat="server" id="divTitle">
        <div class="row">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-10 col-sm-10 col-xs-12">

                <h1 class="title-block" style="line-height: normal;">
                    <asp:Literal Text="" runat="server" ID="litNomeContenuti" /></h1>
            </div>
            <div class="col-md-1 col-sm-1">
            </div>
        </div>
    </div>
    <asp:Literal ID="litMainContent" runat="server"></asp:Literal>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container my-3 py-0 my-sm-5 py-sm-3">

        <%-- TITOLO --%>
        <div class="row d-none d-sm-block">
            <div class="col-12">
                <%= references.ResMan("basetext", Lingua,"testodettaglioordine") %>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <h4>
                    <asp:Label Style="font-size: 1.4em; color: red; padding-top: 10px;" ID="output" runat="server" Text=""></asp:Label></h4>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-9">
                <div class="widget shop-selections">
                    <h3><%= references.ResMan("Common", Lingua,",CarrelloSelezioneArticoli") %></h3>
                    <table class="table table-cart mb-3 mb-0">
                        <thead>
                            <tr>
                                <td></td>
                                <td>
                                    <%= references.ResMan("Common", Lingua,"CarrelloArticolo") %>
                                </td>
                                <td style="text-align: center;">
                                    <%= references.ResMan("Common", Lingua,"CarrelloQuantita") %>

                                </td>
                                <td style="text-align: right;">
                                    <%= references.ResMan("Common", Lingua,"CarrelloTotale") %></td>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="rptProdotti" runat="server" ViewStateMode="Enabled">
                                <ItemTemplate>
                                    <tr>
                                        <td class="cart-remove">
                                            <asp:LinkButton runat="server" ID="btnDelete"
                                                OnClick="btnDelete" CommandArgument='<%# Eval("id") %>'>
                                                  <i class="fa fa-2x fa-times"></i></asp:LinkButton>
                                        </td>
                                        <td>
                                            <a id="a3" runat="server"
                                                href='<%# CreaLinkRoutes(Session,true,Lingua,CleanUrl( ((WelcomeLibrary.DOM.Offerte)(((WelcomeLibrary.DOM.Carrello)Container.DataItem).Offerta)).UrltextforlinkbyLingua(Lingua) ),Eval("Offerta.Id").ToString(),Eval("Offerta.CodiceTipologia").ToString(), Eval("Offerta.CodiceCategoria").ToString()) %>'
                                                target="_self" title='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'
                                                class="pull-left m-0 ml-0 mr-sm-3">
                                                <%--  <div class="work-image">--%>
                                                <asp:Image ID="Anteprima" AlternateText='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'
                                                    runat="server" Style="width: auto; height: auto; max-width: 90px; max-height: 90px;"
                                                    ImageUrl='<%#  WelcomeLibrary.UF.filemanage.ComponiUrlAnteprima(Eval("Offerta.FotoCollection_M.FotoAnteprima"),Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.Id").ToString()) %>'
                                                    Visible='<%#  !CommonPage.ControlloVideo ( Eval("Offerta.FotoCollection_M.FotoAnteprima") ) %>' />
                                                <%--</div>--%>
                                            </a>
                                            <div class="product-details prod-tabella" style="padding-top: 0;">

                                                <h3 class="product-name tx-dark-color">
                                                    <a id="a1" runat="server"
                                                        href='<%# CreaLinkRoutes(Session,true,Lingua,CleanUrl( ((WelcomeLibrary.DOM.Offerte)(((WelcomeLibrary.DOM.Carrello)Container.DataItem).Offerta)).UrltextforlinkbyLingua(Lingua) ),Eval("Offerta.Id").ToString(),Eval("Offerta.CodiceTipologia").ToString(), Eval("Offerta.CodiceCategoria").ToString()) %>'
                                                        target="_self" title='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'>
                                                        <asp:Literal ID="litTitolo" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(  Eval("Offerta.Denominazione" + Lingua).ToString() ) %>'
                                                            runat="server"></asp:Literal>
                                                    </a>
                                                </h3>

                                                <div class="product-categories muted">
                                                    <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"datapartenza", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedata")  + "</b>" +  
                                                        WelcomeLibrary.UF.Utility.reformatdatetimestring((string)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1"), "datapartenza", Lingua) ) ) : "" %>
                                                </div>
                                                <div class="product-categories muted">
                                                    <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"dataritorno", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedataritorno")  + "</b>" +  
                                                            WelcomeLibrary.UF.Utility.reformatdatetimestring((string)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1"), "dataritorno", Lingua) ) ) : "" %>
                                                </div>

                                                <div class="product-categories muted">
                                                    <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"idscaglione", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglioneid") + "</b>" +  WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"idscaglione", Lingua)  ):"" %>
                                                </div>

                                                <div class="product-categories muted">
                                                    <%# Eval("Datastart") !=null? "<b>" + references.ResMan("Common", Lingua,"formtestoperiododa") + ": " + "</b>" +  string.Format("{0:dd/MM/yyyy}", Eval("Datastart")):"" %>
                                                    <%# Eval("Dataend") !=null? "<b>" + references.ResMan("Common", Lingua,"formtestoperiodoa") + ": " + "</b>" + string.Format("{0:dd/MM/yyyy}", Eval("Dataend")) : "" %>
                                                </div>
                                                <div class="product-categories muted">
                                                    <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica1", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica1") + ": " + "</b>" +  references.TestoCaratteristica(0, (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica1", Lingua), Lingua)):"" %>
                                                </div>
                                                <div class="product-categories muted">
                                                    <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica2", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica2") + ": " + "</b>" +  references.TestoCaratteristica(1,(String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica2", Lingua), Lingua)) : "" %>
                                                </div>
                                                <%--  
                                                <div class="product-categories muted">
                                                    <%#  "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "adulti") + ": " + "</b>" +  WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"adulti", Lingua) %>
                                                </div>
                                                <div class="product-categories muted">
                                                    <%#   "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "bambini") + ": " + "</b>" + WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"bambini", Lingua) %>
                                                </div>--%>
                                                <%--  CATEGORIA E SOTTOCATEGORIA--%>
                                                <%-- <div class="product-categories muted d-none d-sm-block">
                                                    <%# CommonPage.TestoCategoria(Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.CodiceCategoria").ToString(),Lingua) %>
                                                &nbsp;<%# CommonPage.TestoCategoria2liv(Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.CodiceCategoria").ToString(),Eval("Offerta.CodiceCategoria2Liv").ToString(),Lingua) %>
                                                </div> --%>

                                                <div class="product-categories muted">
                                                    <%# CommonPage.TestoCaratteristicaJson(Eval("Campo2").ToString(),Eval("Offerta.Xmlvalue").ToString(),Lingua) %>
                                                </div>
                                                <%--   <div class="product-categories muted">
                                                <%# CommonPage.TestoCaratteristica(1,Eval("Offerta.Caratteristica2").ToString(),Lingua) %>
                                                </div>--%>
                                                <%-- <div class="product-categories muted">
                                                        <%# TestoSezione(Eval("Offerta.CodiceTipologia").ToString()) %>
                                                </div>--%>

                                                <b class="product-price d-none d-sm-block">
                                                    <asp:Literal ID="lblPrezzo" runat="server" Visible='<%# VerificaPresenzaPrezzo( Eval("Offerta.Prezzo") ) %>'
                                                        Text='<%#  Eval("Numero").ToString() + "&times" +  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] {
                                                                (!string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"prezzo", Lingua)) ? (   WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"prezzo", Lingua)  ): Eval("Offerta.Prezzo"))
                                                        }) + " €" %>'></asp:Literal>

                                                    <%--  <em>
                                                        <asp:Literal ID="lblPrezzoListino" runat="server" Visible='<%# VerificaPresenzaPrezzo( Eval("Offerta.Prezzolistino") ) %>'
                                                            Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { Eval("Offerta.Prezzolistino") }) + " €" %>'></asp:Literal>
                                                    </em>--%>
                                                </b>

                                            </div>
                                        </td>
                                        <td>
                                            <%--                                            <asp:Label runat="server" CssClass="quantity-order"
                                                Text='<%# Eval("Numero") %>'></asp:Label>--%>
                                            <div class="w-100 d-flex pr-2 py-3 bd-primary-color justify-content-center">


                                                <asp:LinkButton runat="server" ID="btnSottrai" Style="padding-left: 2px !important; font-size: 0.8rem;"
                                                    OnClick="btnDecrement" class="button-carrello" CommandArgument='<%# Eval("id") %>'><i class="fa fa-minus"></i> </asp:LinkButton>


                                                <input runat="server" class="form-control mx-1" style="width: 35px; text-align: center"
                                                    id="txtQuantita" type="text" value='<%# Eval("Numero") %>' />


                                                <asp:LinkButton runat="server" ID="btnAggiungi" Style="padding-left: 2px !important; font-size: 0.8rem;"
                                                    OnClick="btnIncrement" class="button-carrello" CommandArgument='<%# Eval("id") %>'>
                                                        <i class="fa fa-plus"></i>
                                                </asp:LinkButton>

                                            </div>
                                            <div class="clearfix"></div>
                                        </td>
                                        <td style="text-align: right;">
                                            <span><%# TotaleArticolo( Eval("Numero") ,Eval("Prezzo") )  + " €" %></span>
                                        </td>
                                    </tr>


                                </ItemTemplate>
                            </asp:Repeater>

                        </tbody>
                    </table>
                </div>

            </div>
            <div class="col-lg-3">

                <%--<div class="widget">
                        <h3>Coupon Code</h3>
                        <div class="input-group">
                            <input type="text" class="form-control" placeholder="Place promotion code here..">
                            <b class="input-group-btn">
                                <button type="submit" class="btn btn-default">
                                    Apply
                                </button>
                            </b>
                        </div>
                    </div>--%>
                <div class="widget">
                    <h3 class="mbr-section-title display-5 bg-dark-color noafter" style="padding: 0.8rem 0; font-size: 0.8rem; margin-top: 2px; color: #fff; letter-spacing: 2px;">
                        <%= references.ResMan("Common", Lingua,"CarrelloRiepilogo") %></h3>
                    <asp:Repeater runat="server" ID="rptTotali">
                        <ItemTemplate>
                            <table class="table table-summary">
                                <tbody>
                                    <tr class="cart-subtotal">
                                        <th>
                                            <%# references.ResMan("Common", Lingua,"CarrelloTotaleRiepilogo") %></th>
                                        <td style="text-align: right;"><span class="amount">
                                            <asp:Literal ID="lblPrezzo" runat="server"
                                                Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}", new object[] { Eval("TotaleOrdine") }) + " €" %>'></asp:Literal>
                                        </span></td>
                                    </tr>
                                    <tr class="shipping">
                                        <th>
                                            <%# references.ResMan("Common", Lingua,"CarrelloTotaleSpedizione") %></th>
                                        <td style="text-align: right;">
                                            <span class="amount">
                                                <%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { Eval("TotaleSpedizione") }) + " €" %></span></td>
                                    </tr>
                                    <tr class="total">
                                        <th>
                                            <%# references.ResMan("Common", Lingua,"CarrelloTotaleOrdine") %></th>
                                        <td style="text-align: right;">
                                            <span class="amount">
                                                <%--     <%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { (double)Eval("TotaleSmaltimento") + (double)Eval("TotaleSpedizione") + (double)Eval("TotaleOrdine") - (double)Eval("TotaleSconto") }  ) + " €" %> --%>

                                                <%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { (double)Eval("TotaleAcconto") + (double)Eval("TotaleSaldo")  }  ) + " €" %> 
                                            </span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                    <%-- <button class="btn btn-default btn-block">
                            Update totals
                        </button>--%>
                    <%--    <a class="btn btn-large btn-success btn-block"
                    id="A7"   href='<%= ReplaceAbsoluteLinks( references.ResMan("Common", Lingua,"LinkOrder") ) %>'>
                    <%= references.ResMan("Common", Lingua,"TestoProcediOrdine") %> 
                </a><br /><br />--%>
                    <a class="btn btn-block"
                        id="A2" href='<%= ReplaceAbsoluteLinks( references.ResMan("Common", Lingua,"LinkOrderNoregistrazione")) %>'>
                        <%= references.ResMan("Common", Lingua,"TestoProcediOrdineNoregistrazione") %>
                    </a>

                </div>
                <div class="widget shop-shipping pt-4 d-none">
                    <h3><%= references.ResMan("Common", Lingua,"CarrelloCalcolaTotaleSpedizione") %></h3>
                    <div class="form-group">
                        <asp:DropDownList ID="ddlNazione" CssClass="form-control" Width="100%" runat="server">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-row row">
                      <%--  <span class="col-lg-4">
                               <input type="text" class="form-control" placeholder="State / county"> 
                        </span>
                        <span class="col-lg-4">
                            <input type="text" class="form-control" placeholder="Postcode / Zip"> 
                        </span>--%>
                        <span class="col-auto">
                            <asp:LinkButton class="btn btn-default btn-block" OnClick="lnkUpdateCart_Click" ID="lnkUpdateCart" runat="server"><%= references.ResMan("Common", Lingua,"CarrelloRicalcolaTotaleSpedizione") %></asp:LinkButton>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

