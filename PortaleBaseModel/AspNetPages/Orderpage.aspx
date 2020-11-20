<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="Orderpage.aspx.cs"
    Inherits="AspNetPages_Orderpage" EnableSessionState="True"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <script type="text/javascript">
            makeRevLower = true;
        </script>
        <%--<asp:ValidationSummary runat="server" HeaderText='<%# references.ResMan("Common", Lingua,"ValidationError") %>' />--%>
        <asp:Panel runat="server" ID="pnlFormOrdine">
            <div class="position-relative my-4 py-0 my-sm-5 py-sm-3" style="z-index: 1;">
                <div id="divOrderformOverlay" style="position: absolute; top: 0; bottom: 0; z-index: 2"></div>
                <%-- TITOLO --%>
                <div class="row">
                    <div class="col-12">
                        <%= references.ResMan("basetext", Lingua,"testoordine1") %>
                        <h4>  <asp:Label Style="font-size: 2em" Text="" ID="output" runat="server" /></h4>
                        <asp:ValidationSummary runat="server" BackColor="#f0f0f0" DisplayMode="List" Font-Size="Medium"  
                            ShowValidationErrors="true"  HeaderText='<%# references.ResMan("Common", Lingua,"ValidationError") +  "<style> span.errorvalidateclass + * {  border: 2px solid red; }  </style>" %>' />
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-12 col-lg-7">
                        <div class="widget bill-address">
                            <div class="form-vertical">
                                <div class="form-row row">
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto2") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' ControlToValidate="inpNome" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpNome" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />
                                    </div>
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto3") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto3Err") %>' ControlToValidate="inpCognome" runat="server"  />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCognome" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto3") %>' />
                                    </div>
                                </div>
                                <div class="form-row row">
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto16s") %>
                                        </label>
                                        <%--    <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*" ErrorMessage="*" ControlToValidate="inpRagsoc" runat="server" />--%>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpRagsoc" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto16s") %>' />
                                    </div>
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto17s") %>
                                        </label>
                                        <%--   <asp:RequiredFieldValidator  CssClass="errorvalidateclass" Text="*"  ErrorMessage="*" ControlToValidate="inpPiva" runat="server" />--%>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpPiva" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto17s") %>' />
                                    </div>
                                </div>
                                <div class="form-row row"> 
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto10") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto10Err") %>' ControlToValidate="inpIndirizzo" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpIndirizzo" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' />
                                    </div>
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto8") %>
                                        </label>
                                          <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*"  ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto8Err") %>' ControlToValidate="inpComune" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpComune" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto8") %>' />
                                    </div>
                                </div>
                                <div class="form-row row">
                                    <div class="col-4 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto7") %>
                                        </label>
                                         <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*"  ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto7Err") %>' ControlToValidate="inpProvincia" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpProvincia" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' />
                                    </div>
                                    <div class="col-4 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto9") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*"  ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto9Err") %>' ControlToValidate="inpCap" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCap" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' />
                                    </div>
                                    <div class="col-4 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"selezionaNazione") %>
                                        </label>
                                        <asp:DropDownList ID="ddlNazione" Enabled="false" OnSelectedIndexChanged="ddlNazione_SelectedIndexChanged" AutoPostBack="true"
                                            CssClass="form-control" Width="100%" runat="server" AppendDataBoundItems="true" />
                                    </div>
                                </div>

                                <div class="form-row row">
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto4") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*"  ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>'  ControlToValidate="inpEmail" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpEmail" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' />
                                    </div>
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto11") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="*"  ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto11Err") %>'  ControlToValidate="inpTel" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpTel" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                                    </div>
                                </div>
                                <div class="form-row row">
                                    <div class="col-lg-12 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTestoPec") %>
                                        </label>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpPec" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPec") %>' />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- FORM INDIRIZZO SPEDIZIONE --%>
                        <div class="widget shop-shipping" runat="server" visible="true">
                            <h3>
                                <%= references.ResMan("Common", Lingua,"IndirizzoSpedizione") %> </h3>
                            <div class="form-row row">
                                <div class="col-12 col-sm-6 form-group">

                                    <label class="checkbox" style="margin: 5px 0 0 -2.5px;">
                                        <asp:CheckBox EnableViewState="true" Text='<%# references.ResMan("Common", Lingua,"testochkSpedizione") %>' runat="server"
                                            ID="chkSpedizione" Checked="true" AutoPostBack="true" OnCheckedChanged="checkbox_click" />
                                    </label>

                                </div>
                            </div>


                            <asp:PlaceHolder ID="plhShipping" runat="server" Visible="false">
                                <div class="form-row row">
                                    <div class="col-12 col-sm-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto10") %>
                                        </label>

                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpIndirizzoS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' />
                                    </div>
                                    <div class="col-12 col-sm-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto8") %>
                                        </label>

                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpComuneS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto8") %>' />
                                    </div>
                                </div>
                                <div class="form-row row form-group">
                                    <div class="col-12 col-sm-6">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto7") %>
                                        </label>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpProvinciaS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' />
                                    </div>
                                    <div class="col-12 col-sm-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto9") %>
                                        </label>

                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCaps" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' />
                                    </div>
                                </div>
                                <div class="form-row row">
                                    <div class="col-12 col-sm-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto11") %>
                                        </label>

                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpTelS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                                    </div>
                                    <div class="col-12 col-sm-6 form-group">
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                        </div>

                        <%-- FORM NOTE --%>
                        <hr />
                        <div class="form-row row">
                            <div class="col-12 form-group">
                                <label>
                                    <%= references.ResMan("Common", Lingua,"FormTesto14") %>
                                </label>
                                <textarea class="form-control" enableviewstate="true" placeholder='<%# references.ResMan("Common", Lingua,"testoNote") %>' runat="server" id="inpNote" rows="5"></textarea>
                            </div>
                        </div>



                        <%-- TABELLA RIEPILOGO ORDINE --%>
                        <div class="row">
                            <div class="col-12 mt-4">
                                <%= references.ResMan("basetext", Lingua,"testoriepilogoordine") %>
                            </div>
                        </div>

                        <div class="widget shop-selections">
                            <table class="table table-cart mb-1">
                                <thead>
                                    <tr>
                                        <td class="cart-product">
                                            <%= references.ResMan("Common", Lingua,"CarrelloArticolo") %>
                                        </td>
                                        <td class="cart-quantity" style="text-align: center !important;">
                                            <%= references.ResMan("Common", Lingua,"CarrelloQuantita") %></td>
                                        <td class="cart-total" style="font-size: 0.8rem; text-align: right !important;">
                                            <%= references.ResMan("Common", Lingua,"CarrelloTotale") %></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptProdotti" runat="server" ViewStateMode="Enabled">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="cart-product">
                                                    <%--       <a id="a3" runat="server"
                                                href='<%#  WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(,Lingua,CommonPage.CleanUrl( ((WelcomeLibrary.DOM.Offerte)(((WelcomeLibrary.DOM.Carrello)Container.DataItem).Offerta)).UrltextforlinkbyLingua(Lingua) ),   Eval("Offerta.Id").ToString(),Eval("Offerta.CodiceTipologia").ToString(), Eval("Offerta.CodiceCategoria").ToString()) %>'
                                                target="_self" title='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'
                                                class="product-thumb pull-left">
                                                <asp:Image ID="Anteprima" AlternateText='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'
                                                    runat="server" Style="width: auto; height: auto; max-width: 90px; max-height: 90px;"
                                                    ImageUrl='<%#  CommonPage.ComponiUrl(Eval("Offerta.FotoCollection_M.FotoAnteprima"),Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.Id").ToString()) %>'
                                                    Visible='<%#  !CommonPage.ControlloVideo ( Eval("Offerta.FotoCollection_M.FotoAnteprima") ) %>' />
                                            </a>--%>

                                                    <a id="a3" runat="server"
                                                        href='<%#  WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(Lingua,CleanUrl(((WelcomeLibrary.DOM.Offerte)(((WelcomeLibrary.DOM.Carrello)Container.DataItem).Offerta)).UrltextforlinkbyLingua(Lingua)),Eval("Offerta.Id").ToString(),Eval("Offerta.CodiceTipologia").ToString(), Eval("Offerta.CodiceCategoria").ToString()) %>'
                                                        target="_self" title='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'
                                                        class="product-thumb pull-left d-none d-sm-block m-0 ml-0 mr-sm-3">
                                                        <asp:Image ID="Anteprima" AlternateText='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'
                                                            runat="server" Style="width: auto; height: auto; max-width: 100px; max-height: 100px;"
                                                            ImageUrl='<%#  WelcomeLibrary.UF.filemanage.ComponiUrlAnteprima(Eval("Offerta.FotoCollection_M.FotoAnteprima"),Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.Id").ToString()) %>'
                                                            Visible='<%#  !CommonPage.ControlloVideo ( Eval("Offerta.FotoCollection_M.FotoAnteprima") ) %>' />
                                                    </a>
                                                    <div class="product-details  prod-tabella" style="height: auto;">
                                                        <h3 class="product-name">
                                                            <%--  <a id="a1" runat="server"
                                                        href='<%#  WelcomeLibrary.UF.SitemapManager.CreaLinkRoutes(,Lingua,CommonPage.CleanUrl( ((WelcomeLibrary.DOM.Offerte)(((WelcomeLibrary.DOM.Carrello)Container.DataItem).Offerta)).UrltextforlinkbyLingua(Lingua) ),   Eval("Offerta.Id").ToString(),Eval("Offerta.CodiceTipologia").ToString(), Eval("Offerta.CodiceCategoria").ToString()) %>'
                                                        target="_self" title='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'>--%>
                                                            <asp:Literal ID="litTitolo" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(  Eval("Offerta.Denominazione" + Lingua).ToString() ) %>'
                                                                runat="server"></asp:Literal>
                                                            <%--</a>--%>
                                                        </h3>
                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty( WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"datapartenza", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedata") + "</b>" +  WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"datapartenza", Lingua)  ):"" %>
                                                        </div>
                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty( WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"dataritorno", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedataritorno")  + "</b>" +  WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"dataritorno", Lingua)  ):"" %>
                                                        </div>
                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty( WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"idscaglione", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglioneid")  + "</b>" +  WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"idscaglione", Lingua)  ):"" %>
                                                        </div>
                                                        <div class="product-categories muted">
                                                            <%# Eval("Datastart") !=null? "<b>" + references.ResMan("Common", Lingua,"formtestoperiododa") + ": " + "</b>" +  string.Format("{0:dd/MM/yyyy}", Eval("Datastart")):"" %>

                                                            <%# Eval("Dataend") !=null? "<b>" + references.ResMan("Common", Lingua,"formtestoperiodoa") + ": " + "</b>" + string.Format("{0:dd/MM/yyyy}", Eval("Dataend")) : "" %>
                                                        </div>

                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty( WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica1", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica1") + ": " + "</b>" +  references.TestoCaratteristica(0, WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica1", Lingua), Lingua)):"" %>
                                                        </div>
                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty( WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica2", Lingua)) ?("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica2") + ": " + "</b>" +  references.TestoCaratteristica(1,WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica2", Lingua), Lingua)) : "" %>
                                                        </div>
                                                        <%--  
                                                            <div class="product-categories muted">
                                                                <%#  "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "adulti") + ": " + "</b>" +  WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"adulti", Lingua) %>
                                                            </div>
                                                            <div class="product-categories muted">
                                                                <%#   "<b>" + references.ResMan("basetext", Lingua, "formtesto" + "bambini") + ": " + "</b>" + WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"bambini", Lingua) %>
                                                            </div>--%>
                                                        <%--            <div class="product-categories muted">
                                                            <%# CommonPage.TestoCategoria(Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.CodiceCategoria").ToString(),Lingua) + "&nbsp;" %>
                                                            <%# CommonPage.TestoCategoria2liv(Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.CodiceCategoria").ToString(),Eval("Offerta.CodiceCategoria2Liv").ToString(),Lingua) %>
                                                        </div>--%>

                                                        <div class="product-categories muted">
                                                            <%# CommonPage.TestoCaratteristicaJson(Eval("Campo2").ToString(),Eval("Offerta.Xmlvalue").ToString(),Lingua) %>
                                                        </div>
                                                        <div class="product-categories muted">
                                                            <%# CommonPage.TestoCaratteristica(2,Eval("Offerta.Caratteristica1").ToString(),Lingua) %>
                                                            <%# CommonPage.TestoCaratteristica(2,Eval("Offerta.Caratteristica2").ToString(),Lingua) %>
                                                            <%# CommonPage.TestoCaratteristica(2,Eval("Offerta.Caratteristica3").ToString(),Lingua) %>
                                                            <%# CommonPage.TestoCaratteristica(3,Eval("Offerta.Caratteristica4").ToString(),Lingua) %>
                                                            <%# CommonPage.TestoCaratteristica(4,Eval("Offerta.Caratteristica5").ToString(),Lingua) %>
                                                        </div>
                                                        <%--<div class="product-categories muted">
                                                            <%# TestoSezione(Eval("Offerta.CodiceTipologia").ToString()) %>
                                                        </div>--%>
                                                    </div>
                                                    <b class="product-price ">
                                                        <%--      <asp:Literal ID="lblPrezzo" runat="server"
                                                            Text='<%# (Eval("Offerta.Prezzo")!=null && (Double)Eval("Offerta.Prezzo") !=0 )? String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { Eval("Offerta.Prezzo") }) + " €" :"" %>'></asp:Literal>--%>
                                                        <asp:Literal ID="Literal4" runat="server" Text='<%#  Eval("Numero").ToString() + "&times" +  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] {
                                                                (!string.IsNullOrEmpty( WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"prezzo", Lingua)) ? (   WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"prezzo", Lingua)  ): Eval("Offerta.Prezzo"))
                                                        }) + " €" %>'></asp:Literal>

                                                    </b>
                                                </td>
                                                <td class="cart-quantity">
                                                    <asp:Label runat="server"
                                                        ID="lblQuantita" class="quantity-order" Style="margin-left: calc(50% - 25px);" Text='<%# Eval("Numero") %>' />
                                                </td>
                                                <td class="cart-total" style="text-align: right !important;">
                                                    <%--TOTALE ARTICOLI DAL CARRELLO--%>
                                                    <span><%# TotaleArticolo( Eval("Numero") ,Eval("Prezzo") )  + " €" %></span>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Repeater runat="server" ID="rptTotali">
                                        <ItemTemplate>
                                            <tr>
                                                <th class="cart-heading" colspan="2">
                                                    <span>
                                                        <%= references.ResMan("Common", Lingua,"CarrelloTotaleRiepilogo") %></span>
                                                </th>
                                                <td class="cart-total" style="text-align: right !important;">
                                                    <span>
                                                        <asp:Literal ID="lblPrezzo" runat="server"
                                                            Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { Eval("TotaleOrdine") }) + " €" %>'></asp:Literal></span>
                                                </td>
                                            </tr>
                                            <%-- <tr>
                                                <th class="cart-heading" colspan="2">
                                                    <span>
                                                        <%= references.ResMan("Common", Lingua,"CarrelloTotaleSmaltimento") %><br />
                                                        <%= references.ResMan("Common", Lingua,"testoSmaltimento") %>
                                                    </span>
                                                </th>
                                                <td class="cart-total">
                                                    <span>
                                                        <asp:Literal ID="Literal3" runat="server"
                                                            Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] {Eval("TotaleSmaltimento")}) + " €" %>'></asp:Literal></span>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <th class="cart-heading" colspan="2">
                                                    <span>
                                                        <%= references.ResMan("Common", Lingua,"testoSconto") %></span>
                                                </th>
                                                <td class="cart-total" style="text-align: right !important;">

                                                    <span>
                                                        <asp:Literal ID="Literal3" runat="server"
                                                            Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { Eval("TotaleSconto") }) + " €" %>'></asp:Literal></span>
                                                </td>
                                            </tr>

                                            <tr>
                                                <th class="cart-heading" colspan="2">
                                                    <span>
                                                        <%= references.ResMan("Common", Lingua,"CarrelloTotaleSpedizione") %></span>
                                                </th>
                                                <td class="cart-total" style="text-align: right !important;">
                                                    <span>
                                                        <asp:Literal ID="Literal1" runat="server"
                                                            Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { Eval("TotaleSpedizione") }) + " €" %>'></asp:Literal></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th class="cart-heading" colspan="2">
                                                    <span>
                                                        <%= references.ResMan("Common", Lingua,"CarrelloTotaleOrdine") %></span>
                                                </th>
                                                <td class="cart-total" style="text-align: right !important;">
                                                    <span>
                                                        <asp:Literal ID="Literal2" runat="server"
                                                            Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",new object[] { (double)Eval("TotaleSpedizione") + (double)Eval("TotaleSmaltimento") + (double)Eval("TotaleOrdine") - (double)Eval("TotaleSconto") } ) + " €" %>'></asp:Literal></span>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>

                        <%-- CODICE SCONTO --%>
                        <div class="row">
                            <div class="col-12">
                                <div class="widget bill-payment text-center text-sm-left bg-light-color p-3" style="text-align: left; display: block">
                                    <div class="TitlePrezzo mb-2"><%= references.ResMan("Common", Lingua,"TitleCodiceSconto") %></div>
                                    <div class="d-inline float-none float-sm-left my-2" style="width: calc(100% - 219px); background: white;">
                                        <asp:TextBox runat="server" CssClass="w-100 px-2" ID="txtCodiceSconto" />
                                    </div>
                                    <div class="d-block d-sm-inline ml-0 ml-sm-3 my-2">
                                        <asp:Button Text='<%# references.ResMan("Common", Lingua,"testoBtnCodiceSconto") %>' runat="server" ID="btnCodiceSconto" OnClick="btnCodiceSconto_Click" />
                                    </div>
                                    <div style="color: red">
                                        <asp:Literal Text="" ID="lblCodiceSconto" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- NORME GENERALI DI ACQUISTO --%>
                        <div class="row">
                            <div class="col-lg-10 mt-4">
                                <%= references.ResMan("basetext", Lingua,"testoordine2") %>
                            </div>
                        </div>
                    </div>

                    <%-- COLONNA METODO PAGAMENTO E ACQUISTO --%>
                    <div class="col-12 col-lg-5">
                        <div class="position-sticky" style="top: 120px" id="divColumnsticky">
                            <div class="mb-0 mb-sm-3 px-3 py-4 bg-light-color">
                                <%--<div class="widget bill-payment" style="text-align: left; display: none">
                                    <span class="TitlePrezzo"><%= references.ResMan("Common", Lingua,"TitleCodiceSconto") %></span>
                                    <asp:TextBox runat="server" ID="txtCodiceSconto" />
                                    <asp:Button Text='<%# references.ResMan("Common", Lingua,"testoBtnCodiceSconto") %>' runat="server" ID="btnCodiceSconto" OnClick="btnCodiceSconto_Click" />
                                    <span style="color: red">
                                        <asp:Literal Text="" ID="lblCodiceSconto" runat="server" /></span>
                                </div>--%>
                                <div class="widget bill-payment p-2">
                                    <h3><%= references.ResMan("Common", Lingua,"testoMetodopagamento") %></h3>

                                    <script>
                                        function refreshcarrello(btn, args) {
                                            bloccaSblocca('divOrderformOverlay');
                                            __doPostBack('refreshcarrello', args);
                                        }
                                        function bloccaSblocca(idDiv) {

                                            $('#' + idDiv).attr('style', 'position: absolute; top: 0; bottom: 0; z-index: 2; height:100%; width:100%; background-color:rgba(0,0,0,0.2);');
                                        }
                                    </script>
                                    <ul class="unstyled m-0 p-0">
                                        <li style="display: block">
                                            <div class="clearfix" style="margin-bottom: 20px">
                                                <div class="float-left mt-1 mr-2" style="width: 25px">
                                                    <input type="radio" class="form-control" style="background-color: transparent; cursor: pointer" disabled="false" checked="false" name="payment_method" value="contanti" onclick="refreshcarrello(this, 'inpContanti')"
                                                        runat="server" id="inpContanti" />
                                                </div>
                                                <div class="float-left" style="width: calc(100% - 30px - .5rem);">
                                                    <b><%= references.ResMan("Common", Lingua,"txtcontanti") %></b>
                                                    <br />
                                                    <%= references.ResMan("Common", Lingua,"chkcontanti") %>
                                                </div>
                                            </div>
                                        </li>
                                        <li style="display: block">
                                            <div class="clearfix" style="margin-bottom: 20px">
                                                <div class="float-left mt-0 mr-2" style="width: 25px">
                                                    <input type="radio" class="form-control" style="background-color: transparent; cursor: pointer" name="payment_method" value="bacs" checked="false" onclick="refreshcarrello(this, 'inpBonifico')" autopostback="true" runat="server" id="inpBonifico" />
                                                </div>
                                                <div class="float-left" style="width: calc(100% - 30px - .5rem); margin-top: 6px;">
                                                    <b><%= references.ResMan("Common", Lingua,"txtbacs") %></b><br />
                                                    <%= references.ResMan("Common", Lingua,"chkbacs") %>
                                                </div>
                                            </div>
                                        </li>
                                        <%--<li>
                                    <input type="radio" id="payment_method_cheque" class="input-radio" name="payment_method" value="cheque">
                                    <label for="payment_method_cheque">
                                        <b>Cheque Payment</b>
                                        Please send your cheque to Store Name, Store Street, Store Town, Store State / County, Store Postcode.
                                    </label>
                                </li>--%>

                                        <li style="display: none">
                                            <div class="clearfix" style="margin-bottom: 20px">
                                                <div class="float-left mt-1 mr-2" style="width: 25px;">
                                                    <input type="radio" class="form-control" style="background-color: transparent" value="payway" runat="server" id="inpPayway" autopostback="true" />
                                                </div>
                                                <div class="float-left" style="width: calc(100% - 30px - .5rem);">
                                                    <b><%= references.ResMan("Common", Lingua,"txtpayway") %></b><br />
                                                    <%= references.ResMan("Common", Lingua,"chkpayway") %>
                                                </div>
                                            </div>
                                        </li>

                                        <li style="display: block" id="liPaypal" runat="server">
                                            <div class="clearfix" style="margin-bottom: 25px">
                                                <div class="float-left mt-0 mr-2" style="width: 25px;">
                                                    <input type="radio" class="form-control" style="background-color: transparent" disabled="false" checked="false" name="payment_method" value="paypal" runat="server" autopostback="true" id="inpPaypal" onclick="refreshcarrello(this, 'inpPaypal')" />
                                                </div>

                                                <div class="float-left" style="width: calc(100% - 30px - .5rem); margin-top: 6px;">
                                                    <b><%= references.ResMan("Common", Lingua,"txtpaypal") %></b>
                                                    <%--icone-carte di credito--%>
                                                    <div class="container" style="">
                                                        <div class="row justify-content-between d-flex justify-content-center my-2" style="height: 40px;">
                                                            <div class="logo-payment" style="width: 12.5%; background-size: 100% !important;"></div>
                                                            <div class="logo-payment" style="width: 12.5%; background-size: 100% !important;"></div>
                                                            <div class="logo-payment" style="width: 12.5%; background-size: 100% !important;"></div>
                                                            <div class="logo-payment" style="width: 12.5%; background-size: 100% !important;"></div>
                                                            <div class="logo-payment" style="width: 12.5%; background-size: 100% !important;"></div>
                                                            <div class="logo-payment" style="width: 12.5%; background-size: 100% !important;"></div>
                                                            <div class="logo-payment" style="width: 12.5%; background-size: 100% !important;"></div>
                                                        </div>
                                                    </div>
                                                    <%= references.ResMan("Common", Lingua,"chkpaypal") %>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                    <div class="col-12 col-sm-8 mx-auto px-0 d-none">
                                        <asp:CheckBox ID="chkSupplemento" runat="server" Font-Bold="true" ForeColor="Red" Checked="false" AutoPostBack="true" Text='<%# references.ResMan("Common", Lingua,"TestoSupplementoSpedizioni") %>'
                                            OnCheckedChanged="chkSupplemento_CheckedChanged" /><br />
                                        <br />
                                    </div>
                                    <div class="col-12 col-sm-8 mx-auto px-0">
                                        <asp:Button ID="btnConvalida" runat="server" Text='<%# references.ResMan("Common", Lingua,"OrdineEsegui") %>' class="btn w-100" OnClick="btnConvalidaOrdine" />
                                    </div>
                                    <div>
                                        <%= references.ResMan("Common", Lingua,"notespedizioni1") %>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <!--end:.row-->
            </div>
        </asp:Panel>
    </div>
</asp:Content>
