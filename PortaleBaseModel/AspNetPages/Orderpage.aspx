<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="Orderpage.aspx.cs"
    Inherits="AspNetPages_Orderpage" EnableSessionState="True"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        makeRevLower = true;
    </script>
    <h4 style="line-height: 2em">
        <asp:Label Style="font-size: 2em; color: darkgreen" Text="" ID="output" runat="server" /></h4>
    <asp:ValidationSummary runat="server" HeaderText='<%# references.ResMan("Common", Lingua,"ValidationError") %>' />
    <br />
    <asp:Panel runat="server" ID="pnlFormOrdine">
        <div class="row" style="display: none">
            <div class="col-sm-12">
                <div>
                    <asp:CheckBox ID="chkSupplemento" runat="server" Font-Bold="true" ForeColor="Red" Checked="false" AutoPostBack="true" Text='<%# references.ResMan("Common", Lingua,"TestoSupplementoSpedizioni") %>'
                        OnCheckedChanged="chkSupplemento_CheckedChanged" /><br />
                    <br />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-8">
                <div class="widget bill-address">
                    <%--  <h3>
                        <asp:Literal Text='<%# references.ResMan("Common", Lingua,"IndirizzoFatturazione") %>' runat="server" /></h3>--%>
                    <div class="form-vertical">
                        <div class="form-row row" style="display: block">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"selezionaNazione") %>' runat="server" /></label>
                                <asp:DropDownList ID="ddlNazione" Enabled="false"
                                    CssClass="form-control" Width="100%" runat="server" AppendDataBoundItems="true" />
                            </div>
                        </div>
                        <div class="form-row row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpNome" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />
                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpNome" runat="server" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto3") %>' runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCognome" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto3") %>' />
                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpCognome" runat="server" />
                            </div>
                        </div>
                        <div class="form-row row" style="display: block">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto16s") %>' runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpRagsoc" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto16s") %>' />
                                <%--        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpRagsoc" runat="server" />--%>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto17s") %>' runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpPiva" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto17s") %>' />
                                <%--   <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpPiva" runat="server" />--%>
                            </div>
                        </div>
                        <div class="form-row row" style="display: block">

                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpIndirizzo" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' />
                                <%-- <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpIndirizzo" runat="server" />--%>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto8") %>' runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpComune" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto8") %>' />
                                <%--  <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpComune" runat="server" />--%>
                            </div>

                        </div>
                        <div class="form-row row form-group" style="display: block">
                            <div class="col-lg-6">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpProvincia" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' />
                                <%-- <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpProvincia" runat="server" />--%>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCap" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' />
                                <%--                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpCap" runat="server" />--%>
                            </div>
                        </div>

                        <div class="form-row row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpEmail" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' />
                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpEmail" runat="server" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpTel" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="widget shop-shipping" runat="server" visible="true">
                    <h3>
                        <asp:Literal Text='<%# references.ResMan("Common", Lingua,"IndirizzoSpedizione") %>' runat="server" /></h3>
                    <div class="form-row row">
                        <div class="col-lg-12 form-group">
                            <label class="checkbox">
                                <asp:CheckBox EnableViewState="true" Text='<%# references.ResMan("Common", Lingua,"testochkSpedizione") %>' runat="server"
                                    ID="chkSpedizione" Checked="true" AutoPostBack="true" OnCheckedChanged="checkbox_click" />
                            </label>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="plhShipping" runat="server" Visible="false">
                        <div class="form-row row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpIndirizzoS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto8") %>' runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpComuneS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto8") %>' />
                            </div>
                        </div>
                        <div class="form-row row form-group">
                            <div class="col-lg-6">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpProvinciaS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCaps" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' />
                            </div>
                        </div>
                        <div class="form-row row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpTelS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                            </div>
                            <div class="col-lg-6 form-group">
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <hr />
                    <div class="form-row row">
                        <div class="col-lg-12 form-group">
                            <label>
                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"FormTesto14") %>' runat="server" />
                            </label>
                            <textarea class="form-control" enableviewstate="true" placeholder='<%# references.ResMan("Common", Lingua,"testoNote") %>' runat="server" id="inpNote" rows="5"></textarea>
                        </div>
                    </div>
                </div>
                <div class="widget shop-selections">
                    <h3>
                        <asp:Literal runat="server" Text='<%# references.ResMan("Common", Lingua,"CarrelloSelezioneArticoli") %>'></asp:Literal></h3>
                    <table class="table table-cart">
                        <thead>
                            <tr>
                                <td class="cart-product">
                                    <asp:Literal runat="server" Text='<%# references.ResMan("Common", Lingua,"CarrelloArticolo") %>'></asp:Literal>
                                </td>
                                <td class="cart-quantity">
                                    <asp:Literal runat="server" Text='<%# references.ResMan("Common", Lingua,"CarrelloQuantita") %>'></asp:Literal></td>
                                <td class="cart-total">
                                    <asp:Literal runat="server" Text='<%# references.ResMan("Common", Lingua,"CarrelloTotale") %>'></asp:Literal></td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptProdotti" runat="server" ViewStateMode="Enabled">
                                <ItemTemplate>
                                    <tr>
                                        <td class="cart-product">
                                            <%--       <a id="a3" runat="server"
                                                href='<%# CommonPage.CreaLinkRoutes(Session,true,Lingua,CommonPage.CleanUrl(Eval("Offerta.Denominazione" + Lingua).ToString()),   Eval("Offerta.Id").ToString(),Eval("Offerta.CodiceTipologia").ToString(), Eval("Offerta.CodiceCategoria").ToString()) %>'
                                                target="_self" title='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'
                                                class="product-thumb pull-left">
                                                <asp:Image ID="Anteprima" AlternateText='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'
                                                    runat="server" Style="width: auto; height: auto; max-width: 90px; max-height: 90px;"
                                                    ImageUrl='<%#  CommonPage.ComponiUrl(Eval("Offerta.FotoCollection_M.FotoAnteprima"),Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.Id").ToString()) %>'
                                                    Visible='<%#  !CommonPage.ControlloVideo ( Eval("Offerta.FotoCollection_M.FotoAnteprima") ) %>' />
                                            </a>--%>
                                            <div class="product-details" style="height:auto">
                                                <h3 class="product-name">
                                                    <%--  <a id="a1" runat="server"
                                                        href='<%# CommonPage.CreaLinkRoutes(Session,true,Lingua,CommonPage.CleanUrl(Eval("Offerta.Denominazione" + Lingua).ToString()),   Eval("Offerta.Id").ToString(),Eval("Offerta.CodiceTipologia").ToString(), Eval("Offerta.CodiceCategoria").ToString()) %>'
                                                        target="_self" title='<%# CommonPage.CleanInput(CommonPage.ConteggioCaratteri(  Eval("Offerta.Denominazione" + Lingua).ToString(),300,true )) %>'>--%>
                                                    <asp:Literal ID="litTitolo" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(  Eval("Offerta.Denominazione" + Lingua).ToString() ) %>'
                                                        runat="server"></asp:Literal>
                                                    <%--</a>--%>
                                                </h3>
                                                <div class="product-categories muted">
                                                    <%# CommonPage.TestoCategoria(Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.CodiceCategoria").ToString(),Lingua) %>
                                                       &nbsp;<%# CommonPage.TestoCategoria2liv(Eval("Offerta.CodiceTipologia").ToString(),Eval("Offerta.CodiceCategoria").ToString(),Eval("Offerta.CodiceCategoria2Liv").ToString(),Lingua) %>
                                                </div>
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
                                                <asp:Literal ID="lblPrezzo" runat="server"
                                                    Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("Offerta.Prezzo")) + " €" %>'></asp:Literal>
                                            </b>
                                            </div>
                                        </td>
                                        <td class="cart-quantity">
                                            <asp:Label runat="server"
                                                ID="lblQuantita" Text='<%# Eval("Numero") %>' />
                                        </td>
                                        <td class="cart-total">
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
                                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"CarrelloTotaleRiepilogo") %>' runat="server" /></span>
                                        </th>
                                        <td class="cart-total">
                                            <span>
                                                <asp:Literal ID="lblPrezzo" runat="server"
                                                    Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("TotaleOrdine")) + " €" %>'></asp:Literal></span>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                                <th class="cart-heading" colspan="2">
                                                    <span>
                                                        <asp:Literal Text='<%# references.ResMan("Common", Lingua,"CarrelloTotaleSmaltimento") %>' runat="server" /><br />
                                                        <asp:Literal Text='<%# references.ResMan("Common", Lingua,"testoSmaltimento") %>' runat="server" />

                                                    </span>
                                                </th>
                                                <td class="cart-total">
                                                    <span>
                                                        <asp:Literal ID="Literal3" runat="server"
                                                            Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("TotaleSmaltimento")) + " €" %>'></asp:Literal></span>
                                                </td>
                                            </tr>--%>
                                    <tr>
                                        <th class="cart-heading" colspan="2">
                                            <span>
                                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"testoSconto") %>' runat="server" /></span>
                                        </th>
                                        <td class="cart-total">

                                            <span>
                                                <asp:Literal ID="Literal3" runat="server"
                                                    Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("TotaleSconto")) + " €" %>'></asp:Literal></span>

                                        </td>
                                    </tr>

                                    <tr>
                                        <th class="cart-heading" colspan="2">
                                            <span>
                                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"CarrelloTotaleSpedizione") %>' runat="server" /></span>
                                        </th>
                                        <td class="cart-total">
                                            <span>
                                                <asp:Literal ID="Literal1" runat="server"
                                                    Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("TotaleSpedizione")) + " €" %>'></asp:Literal></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="cart-heading" colspan="2">
                                            <span>
                                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"CarrelloTotaleOrdine") %>' runat="server" /></span>
                                        </th>
                                        <td class="cart-total">
                                            <span>
                                                <asp:Literal ID="Literal2" runat="server"
                                                    Text='<%#  String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",(double)Eval("TotaleSpedizione") +(double)Eval("TotaleSmaltimento") + (double)Eval("TotaleOrdine") - (double)Eval("TotaleSconto")    ) + " €" %>'></asp:Literal></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div class="widget bill-payment" style="text-align: left">

                    <asp:Label runat="server" ID="Label1" CssClass="TitlePrezzo" Text='<%# references.ResMan("Common", Lingua,"TitleCodiceSconto") %>' />
                    <asp:TextBox runat="server" ID="txtCodiceSconto" />
                    <asp:Button Text='<%# references.ResMan("Common", Lingua,"testoBtnCodiceSconto") %>' runat="server" ID="btnCodiceSconto" OnClick="btnCodiceSconto_Click" />
                    <span style="color: red">
                        <asp:Literal Text="" ID="lblCodiceSconto" runat="server" /></span>

                </div>

                <div class="widget bill-payment">
                    <h3>
                        <asp:Literal Text='<%# references.ResMan("Common", Lingua,"testoMetodopagamento") %>' runat="server" /></h3>

                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"txtEstero") %>' runat="server" />
                    <ul class="unstyled">
                        <li>
                            <input type="radio" class="input-radio" name="payment_method" value="bacs" checked="false"
                                runat="server" id="inpBonifico" />
                            <label for="payment_method_bacs">
                                <b>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"txtbacs") %>' runat="server" /></b><br />

                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"chkbacs") %>' runat="server" />

                            </label>

                        </li>
                        <li style="display: none">
                            <input type="radio" class="input-radio" disabled="false" name="payment_method" value="contanti"
                                runat="server" id="inpContanti" /><br />
                            <label for="payment_method_contanti">
                                <b>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"txtcontanti") %>' runat="server" /></b><br />

                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"chkcontanti") %>' runat="server" />

                            </label>

                        </li>
                        <%--<li>
                                    <input type="radio" id="payment_method_cheque" class="input-radio" name="payment_method" value="cheque">
                                    <label for="payment_method_cheque">
                                        <b>Cheque Payment</b>
                                        Please send your cheque to Store Name, Store Street, Store Town, Store State / County, Store Postcode.
                                    </label>
                                </li>--%>

                        <li style="display: none">
                            <input type="radio" class="input-radio" disabled="false" name="payment_method" value="payway" runat="server" id="inpPayway" />
                            <label for="payment_method_payway">
                                <b>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"txtpayway") %>' runat="server" /></b><br />

                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"chkpayway") %>' runat="server" />
                            </label>
                        </li>

                        <li>
                            <input type="radio" class="input-radio" disabled="false" checked="true" name="payment_method" value="paypal" runat="server" id="inpPaypal" />
                            <label for="payment_method_paypal">
                                <b>
                                    <asp:Literal Text='<%# references.ResMan("Common", Lingua,"txtpaypal") %>' runat="server" /></b><br />

                                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"chkpaypal") %>' runat="server" />
                            </label>
                        </li>
                    </ul>
                </div>


                <hr />

                <asp:Button ID="btnConvalida" runat="server" Text='<%# references.ResMan("Common", Lingua,"OrdineEsegui") %>' class="btn btn-purple" OnClick="btnConvalidaOrdine" />

            </div>
        </div>
        <!--end:.row-->
    </asp:Panel>

</asp:Content>

