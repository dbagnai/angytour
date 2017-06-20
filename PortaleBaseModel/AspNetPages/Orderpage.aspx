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
    <asp:ValidationSummary runat="server" HeaderText="<%$ Resources:Common,ValidationError %>" />
    <br />

    <asp:Panel runat="server" ID="pnlFormOrdine">
        <div class="row" style="display:none">
            <div class="col-sm-12">
                <div>
                    <asp:CheckBox ID="chkSupplemento" runat="server" Font-Bold="true" ForeColor="Red" Checked="false" AutoPostBack="true" Text='<%$ Resources:Common,TestoSupplementoSpedizioni %>'
                        OnCheckedChanged="chkSupplemento_CheckedChanged" /><br />
                    <br />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-8">

                <div class="widget bill-address">
                  <%--  <h3>
                        <asp:Literal Text="<%$ Resources:Common,IndirizzoFatturazione %>" runat="server" /></h3>--%>
                    <div class="form-vertical">
                        <div class="form-row row" style="display:none">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,selezionaNazione %>" runat="server" /></label>
                                <asp:DropDownList ID="ddlNazione" Enabled="false"
                                    CssClass="form-control" Width="100%" runat="server" AppendDataBoundItems="true" />
                            </div>
                        </div>
                        <div class="form-row row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto2 %>" runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpNome" placeholder="<%$ Resources:Common,FormTesto2 %>" />
                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpNome" runat="server" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto3 %>" runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCognome" placeholder="<%$ Resources:Common,FormTesto3 %>" />
                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpCognome" runat="server" />
                            </div>
                        </div>
                        <div class="form-row row" style="display:none">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto16s %>" runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpRagsoc" placeholder="<%$ Resources:Common,FormTesto16s %>" />
                                <%--        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpRagsoc" runat="server" />--%>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto17s %>" runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpPiva" placeholder="<%$ Resources:Common,FormTesto17s %>" />
                                <%--   <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpPiva" runat="server" />--%>
                            </div>
                        </div>
                        <div class="form-row row" style="display: none">
                            <div class="col-lg-6">
                                <div class="col-lg-6 form-group">
                                    <label>
                                        <asp:Literal Text="<%$ Resources:Common,FormTesto10 %>" runat="server" />
                                    </label>
                                    <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpIndirizzo" placeholder="<%$ Resources:Common,FormTesto10 %>" />
                                    <%-- <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpIndirizzo" runat="server" />--%>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label>
                                        <asp:Literal Text="<%$ Resources:Common,FormTesto8 %>" runat="server" />
                                    </label>

                                    <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpComune" placeholder="<%$ Resources:Common,FormTesto8 %>" />
                                    <%--  <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpComune" runat="server" />--%>
                                </div>
                            </div>
                        </div>
                        <div class="form-row row form-group" style="display: none">
                            <div class="col-lg-6">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto7 %>" runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpProvincia" placeholder="<%$ Resources:Common,FormTesto7 %>" />
                               <%-- <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpProvincia" runat="server" />--%>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto9 %>" runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCap" placeholder="<%$ Resources:Common,FormTesto9 %>" />
                                <%--                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpCap" runat="server" />--%>
                            </div>
                        </div>
                     
                        <div class="form-row row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto4 %>" runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpEmail" placeholder="<%$ Resources:Common,FormTesto4 %>" />
                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="inpEmail" runat="server" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto11 %>" runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpTel" placeholder="<%$ Resources:Common,FormTesto11 %>" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="widget shop-shipping" runat="server" visible="false">
                    <h3>
                        <asp:Literal Text="<%$ Resources:Common,IndirizzoSpedizione %>" runat="server" /></h3>
                    <div class="form-row row">
                        <div class="col-lg-12 form-group">
                            <label class="checkbox">
                                <asp:CheckBox EnableViewState="true" Text="<%$ Resources:Common,testochkSpedizione %>" runat="server"
                                    ID="chkSpedizione" Checked="true" AutoPostBack="true" OnCheckedChanged="checkbox_click" />
                            </label>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="plhShipping" runat="server" Visible="false">
                        <div class="form-row row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto10 %>" runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpIndirizzoS" placeholder="<%$ Resources:Common,FormTesto10 %>" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto8 %>" runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpComuneS" placeholder="<%$ Resources:Common,FormTesto8 %>" />
                            </div>
                        </div>
                        <div class="form-row row form-group">
                            <div class="col-lg-6">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto7 %>" runat="server" />
                                </label>
                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpProvinciaS" placeholder="<%$ Resources:Common,FormTesto7 %>" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto9 %>" runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCaps" placeholder="<%$ Resources:Common,FormTesto9 %>" />
                            </div>
                        </div>
                        <div class="form-row row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    <asp:Literal Text="<%$ Resources:Common,FormTesto11 %>" runat="server" />
                                </label>

                                <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpTelS" placeholder="<%$ Resources:Common,FormTesto11 %>" />
                            </div>
                            <div class="col-lg-6 form-group">
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <hr />
                    <div class="form-row row">
                        <div class="col-lg-12 form-group">
                            <label>
                                <asp:Literal Text="<%$ Resources:Common,FormTesto14 %>" runat="server" />
                            </label>
                            <textarea class="form-control" enableviewstate="true" placeholder="<%$ Resources:Common,testoNote %>" runat="server" id="inpNote" rows="5"></textarea>
                        </div>
                    </div>
                </div>
                <div class="widget shop-selections">
                    <h3>
                        <asp:Literal runat="server" Text='<%$ Resources:Common,CarrelloSelezioneArticoli %>'></asp:Literal></h3>
                    <table class="table table-cart">
                        <thead>
                            <tr>
                                <td class="cart-product">
                                    <asp:Literal runat="server" Text='<%$ Resources:Common,CarrelloArticolo %>'></asp:Literal>
                                </td>
                                <td class="cart-quantity">
                                    <asp:Literal runat="server" Text='<%$ Resources:Common,CarrelloQuantita %>'></asp:Literal></td>
                                <td class="cart-total">
                                    <asp:Literal runat="server" Text='<%$ Resources:Common,CarrelloTotale %>'></asp:Literal></td>
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
                                            <div class="product-details">
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
                                                </div>
                                                <div class="product-categories muted">
                                                    <%# CommonPage.TestoCaratteristica(2,Eval("Offerta.Caratteristica3").ToString(),Lingua) %>&nbsp;
                                                                <%# CommonPage.TestoCaratteristica(3,Eval("Offerta.Caratteristica4").ToString(),Lingua) %>&nbsp;
                                                                <%# CommonPage.TestoCaratteristica(4,Eval("Offerta.Caratteristica5").ToString(),Lingua) %>
                                                </div>
                                                <%--<div class="product-categories muted">
                                                            <%# TestoSezione(Eval("Offerta.CodiceTipologia").ToString()) %>
                                                        </div>--%>
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
                                                <asp:Literal Text="<%$ Resources:Common,CarrelloTotaleRiepilogo %>" runat="server" /></span>
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
                                                        <asp:Literal Text="<%$ Resources:Common,CarrelloTotaleSmaltimento %>" runat="server" /><br />
                                                        <asp:Literal Text="<%$ Resources:Common,testoSmaltimento %>" runat="server" />

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
                                                <asp:Literal Text="<%$ Resources:Common,testoSconto %>" runat="server" /></span>
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
                                                <asp:Literal Text="<%$ Resources:Common,CarrelloTotaleSpedizione %>" runat="server" /></span>
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
                                                <asp:Literal Text="<%$ Resources:Common,CarrelloTotaleOrdine %>" runat="server" /></span>
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

                    <asp:Label runat="server" ID="Label1" CssClass="TitlePrezzo" Text="<%$ Resources:Common,TitleCodiceSconto %>" />
                    <asp:TextBox runat="server" ID="txtCodiceSconto" />
                    <asp:Button Text="<%$ Resources:Common,testoBtnCodiceSconto %>" runat="server" ID="btnCodiceSconto" OnClick="btnCodiceSconto_Click" />
                    <span style="color: red">
                        <asp:Literal Text="" ID="lblCodiceSconto" runat="server" /></span>

                </div>

                <div class="widget bill-payment">
                    <h3>
                        <asp:Literal Text="<%$ Resources:Common,testoMetodopagamento %>" runat="server" /></h3>

                    <asp:Literal Text="<%$ Resources:Common,txtEstero %>" runat="server" />
                    <ul class="unstyled">
                        <li style="display: none">
                            <input type="radio" class="input-radio" name="payment_method" value="bacs" checked="false"
                                runat="server" id="inpBonifico" />
                            <label for="payment_method_bacs">
                                <b>
                                    <asp:Literal Text="<%$ Resources:Common,txtbacs %>" runat="server" /></b><br />

                                <asp:Literal Text="<%$ Resources:Common,chkbacs %>" runat="server" />

                            </label>

                        </li>
                        <li style="display: none">
                            <input type="radio" class="input-radio" disabled="false" name="payment_method" value="contanti"
                                runat="server" id="inpContanti" /><br />
                            <label for="payment_method_contanti">
                                <b>
                                    <asp:Literal Text="<%$ Resources:Common,txtcontanti %>" runat="server" /></b><br />

                                <asp:Literal Text="<%$ Resources:Common,chkcontanti %>" runat="server" />

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
                                    <asp:Literal Text="<%$ Resources:Common,txtpayway %>" runat="server" /></b><br />

                                <asp:Literal Text="<%$ Resources:Common,chkpayway %>" runat="server" />
                            </label>
                        </li>

                        <li>
                            <input type="radio" class="input-radio" disabled="false" checked="true" name="payment_method" value="paypal" runat="server" id="inpPaypal" />
                            <label for="payment_method_paypal">
                                <b>
                                    <asp:Literal Text="<%$ Resources:Common,txtpaypal %>" runat="server" /></b><br />

                                <asp:Literal Text="<%$ Resources:Common,chkpaypal %>" runat="server" />
                            </label>
                        </li>
                    </ul>
                </div>


                <hr />

                <asp:Button ID="btnConvalida" runat="server" Text="<%$ Resources:Common,OrdineEsegui %>" class="btn btn-purple" OnClick="btnConvalidaOrdine" />

            </div>
        </div>
        <!--end:.row-->
    </asp:Panel>

</asp:Content>

