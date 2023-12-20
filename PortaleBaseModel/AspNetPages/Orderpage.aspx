<%@ Page Title="" Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="Orderpage.aspx.cs"
    Inherits="AspNetPages_Orderpage" EnableSessionState="True"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Stripe.js injects the Card Element DA ABILITARE PER USARE STRIPE -->
    <%--<script src="https://js.stripe.com/v3/"></script>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script>
         //rileva la navigazione con cronologia se effettuata ricarica la pagina completamente
         window.addEventListener("pageshow", function (event) {
             const entries = window.performance.getEntriesByType("navigation");
             var historyTraversal = event.persisted
                 //  ||  (typeof window.performance != "undefined" && window.performance.navigation.type === 2);
                 || (entries[0].type === "back_forward");
             if (historyTraversal) {
                 // Handle page restore.
                 window.location.reload();
                 console.log('page reload by hystory nav');
             }
         });
     </script>
    <div class="container">
        <div class="row">
            <div class="col-12">
                <%= references.ResMan("basetext", Lingua,"testoordine1") %>
                <h4>
                    <asp:Label Style="font-size: 1.2em" Text="" ID="output" runat="server" /></h4>
                <asp:ValidationSummary ID="validsummary" ValidationGroup="" runat="server" BackColor="#f0f0f0" DisplayMode="SingleParagraph" Font-Size="Medium" ShowValidationErrors="true" HeaderText='<%# references.ResMan("Common", Lingua,"ValidationError") +  "<br/><style> span.errorvalidateclass[style*=\"visibility: visible\"] + * {  border: 2px solid red; }  </style>" %>' />
                <%--      <asp:ValidationSummary runat="server" BackColor="#f0f0f0" DisplayMode="SingleParagraph" Font-Size="Medium"  
                            ShowValidationErrors="true"  HeaderText='<%# references.ResMan("Common", Lingua,"ValidationError")  %>' />--%>
            </div>
        </div>
        <%--<asp:ValidationSummary runat="server" HeaderText='<%# references.ResMan("Common", Lingua,"ValidationError") %>' />--%>
        <asp:Panel runat="server" ID="pnlFormOrdine">
            <div class="position-relative mt-1 mb-3 py-0" style="z-index: 1;">
                <div id="divOrderformOverlay" style="position: absolute; top: 0; bottom: 0; z-index: 2"></div>
                <%--ACCESSO UTENTE E LOGIN--%>
                <div class="row py-2 my-2 d-none">
                    <div class="col-12">
                        <div class=" border-tipo1 p-2 bg-light-color w-100">
                            <%= references.ResMan("Common",Lingua,"testologinacquisto") %><br />
                            <br />
                            <script>
                                function verificalogin(callcontrol) {
                                    Loginuser($("[id*='inputName']").val(), $("[id*='inputPassword']").val(), function (ret) {
                                        if (ret == '') {
                                            refreshcarrello('', 'loginuser');
                                        }
                                        $("[id*='outputlogin']").html(ret)
                                    })
                                }
                                //function logoffuser(btn, args) { __doPostBack('logoffuser', args); }
                                function logoffuser(callcontrol) {
                                    Logoff(function (ret) {
                                        if (ret == '') {
                                            refreshcarrello('', 'logoffuser');
                                        }
                                        $("[id*='outputlogin']").html(ret)
                                    })
                                }
                                //function recuperapass(btn, args) { __doPostBack('recuperapass', args); }
                                function recuperapass(callcontrol) {
                                    var onclicktxt = $(callcontrol).attr("onclick");
                                    $(callcontrol).removeAttr("onclick");
                                    var tastotxt = $(callcontrol).html();
                                    $(callcontrol).html("Wait ..");
                                    RecuperoPassword($("[id*='inputName']").val(), function (ret) {
                                        $(callcontrol).attr("onclick", onclicktxt);
                                        $(callcontrol).html(tastotxt);
                                        $("[id*='outputlogin']").html(ret);
                                    })
                                }
                                function refreshcarrello(btn, args) {
                                    bloccaSblocca('divOrderformOverlay');
                                    resetValidationState();
                                    __doPostBack('refreshcarrello', args);
                                }
                                function bloccaSblocca(idDiv) {
                                    $('#' + idDiv).attr('style', 'position: absolute; top: 0; bottom: 0; z-index: 2; height:100%; width:100%; background-color:rgba(0,0,0,0.2);');
                                }

                            </script>
                            <div runat="server" visible='<%# ControlloLogin() %>'>
                                <input type="text" class="form-control" placeholder="Username" style="display: inline-block; width: 180px; background-color: #fff" runat="server" id="inputName" value="" />
                                <input type="password" class="form-control" placeholder="Password" style="display: inline-block; width: 180px; background-color: #fff" runat="server" id="inputPassword" value="" />
                                <input type="button" id="btnlogin" class="btn btn-small" onclick="verificalogin(this)" value="<%= references.ResMan("Common",Lingua,"testoLoginAccedi") %>" />
                                <br />
                                <a id="linkpassrecover" href="javascript:void(0)" onclick="recuperapass(this)" class="secondary-color pt-2" style="display: block"><%= references.ResMan("Common",Lingua,"forgetrequest0") %> </a>
                            </div>
                            <div runat="server" visible='<%# !ControlloLogin() %>'>
                                <input type="button" id="btnlogoff" class="btn btn-small" onclick="logoffuser(this)" value="<%= references.ResMan("Common",Lingua,"testologindisconnetti") + " " + Page.User.Identity.Name %>" />
                            </div>
                            <div class="pt-2" style="font-weight: bold; color: red">
                                <asp:Label ID="outputlogin" ClientIDMode="Static" Text="" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <%--fine ACCESSO UTENTE E LOGIN--%>
                <div class="row">
                    <div class="col-12 col-lg-7">
                        <div class="widget bill-address">
                            <div class="form-vertical">
                                <%-- CODICE SCONTO --%>
                                <div class=" bg-light-color p-3">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="TitlePrezzo mb-2"><%= references.ResMan("Common", Lingua,"TitleCodiceSconto") %></div>

                                        </div>
                                    </div>
                                    <div class="row d-flex align-items-center">
                                        <div class="col-12 col-sm-8">
                                            <asp:TextBox runat="server" autocomplete="none" CssClass="w-100 px-2 form-control bg-white" ID="txtCodiceSconto" /><br />
                                        </div>
                                        <div class="col-12 col-sm-4">
                                            <asp:Button Style="width: 180px" CausesValidation="false" Text='<%# references.ResMan("Common", Lingua,"testoBtnCodiceSconto") %>' runat="server" ID="Button1" OnClick="btnCodiceSconto_Click" />
                                        </div>
                                    </div>
                                    <div class="row d-flex align-items-center my-2">
                                        <div class="col-12 col-sm-8">
                                            <asp:Literal runat="server" ID="litCodiceSconto"></asp:Literal><br />
                                            <div style="color: red; width: 100%">
                                                <asp:Literal Text="" ID="outputCodiceSconto" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-12 col-sm-4">
                                            <asp:Button Style="width: 180px" CausesValidation="false" Text='<%# references.ResMan("Common", Lingua,"testoBtnResetCodiceSconto") %>' runat="server" ID="btnCodiceSconto" OnClick="btnResetCodiceSconto_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-row row">
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto2") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="* Error" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' ControlToValidate="inpNome" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpNome" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />
                                    </div>
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto3") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="* Error" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto3Err") %>' ControlToValidate="inpCognome" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCognome" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto3") %>' />
                                    </div>
                                </div>
                                <div class="form-row row  ">
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto4") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="* Error" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto4Err") %>' ControlToValidate="inpEmail" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpEmail" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto4") %>' />
                                    </div>
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto11") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="* Error" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto11Err") %>' ControlToValidate="inpTel" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpTel" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto11") %>' />
                                    </div>
                                </div>

                                <div class="form-row row">
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto10") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="* Error" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto10Err") %>' ControlToValidate="inpIndirizzo" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpIndirizzo" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto10") %>' />
                                    </div>
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto8") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="* Error" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto8Err") %>' ControlToValidate="inpComune" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpComune" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto8") %>' />
                                    </div>
                                </div>
                                <div class="form-row row">
                                    <div class="col-4 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto7") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="* Error" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto7Err") %>' ControlToValidate="inpProvincia" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpProvincia" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' />
                                    </div>
                                    <div class="col-4 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto9") %>
                                        </label>
                                        <asp:RequiredFieldValidator CssClass="errorvalidateclass" Text="* Error" ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto9Err") %>' ControlToValidate="inpCap" runat="server" />
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCap" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' />
                                    </div>
                                    <div class="col-4 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"selezionaNazione") %>
                                        </label>
                                        <%--  <asp:DropDownList ID="ddlNazione" Enabled="true" OnSelectedIndexChanged="ddlNazione_SelectedIndexChanged"  AutoPostBack="true"
                                            CssClass="form-control" Width="100%" runat="server" AppendDataBoundItems="true"  />  --%>
                                        <asp:DropDownList ID="ddlNazione" Enabled="true" onchange="refreshcarrello(this, '')"
                                            CssClass="form-control" Width="100%" runat="server" AppendDataBoundItems="true" />
                                    </div>
                                </div>
                                <div class="form-row row d-block">
                                    <div class="col-12 form-group">
                                        <label class="checkbox" style="margin: 5px 0 0 -2.5px;">
                                            <asp:CheckBox EnableViewState="true" Text='<%# references.ResMan("Common", Lingua,"TestoSupplementoSpedizioni") %>' runat="server"
                                                ID="chkSupplemento" ForeColor="Red" Font-Bold="true" Checked="false" AutoPostBack="true" OnCheckedChanged="chkSupplemento_CheckedChanged" />
                                        </label>
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
                                    <div class="col-lg-12 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTestoPec") %>
                                        </label>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpPec" placeholder='<%# references.ResMan("Common", Lingua,"FormTestoPec") %>' />
                                    </div>
                                </div>


                                <div class="form-row row">
                                    <div class="col-lg-12 form-group">
                                        <div style="display: none; margin-top: 15px; text-align: left">
                                            <label class="checkbox" style="margin: 5px 0 0 -2.5px;">
                                                <asp:CheckBox EnableViewState="true" runat="server" CssClass="pull-left" ForeColor="Red" Font-Bold="true"
                                                    ID="chkRichiedifattura" Checked="false" Text='<%# references.ResMan("Common", Lingua,"txtRichiestafattura") %>' />
                                            </label>
                                            <%--    <div style="color: red; font-size: 1rem" class="pull-left">
                                                <%= references.ResMan("Common", Lingua,"txtRichiestafattura") %>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <%-- FORM INDIRIZZO SPEDIZIONE --%>
                        <div class="widget shop-shipping d-block">
                            <h3><%= references.ResMan("Common", Lingua,"IndirizzoSpedizione") %> </h3>
                            <div class="form-row row">
                                <div class="col-12">
                                    <label class="checkbox" style="margin: 5px 0 0 -2.5px;">
                                        <input type="checkbox" enableviewstate="true" class="form-control" runat="server" clientidmode="Static" id="chkSpedizione" checked="true" />
                                        <%=  references.ResMan("Common", Lingua,"testochkSpedizione") %><br />
                                    </label>
                                    <script>
                                        jQuery(document).ready(function () {
                                            $("[id$='chkSpedizione']").on('change', function () {
                                                if ($("[id$='chkSpedizione']")[0].checked)
                                                    $("[id$='plhShipping']").hide();
                                                else $("[id$='plhShipping']").show();
                                            });
                                            if ($("[id$='chkSpedizione']")[0].checked)
                                                $("[id$='plhShipping']").hide();
                                            else $("[id$='plhShipping']").show();
                                        });


                                    </script>
                                </div>
                            </div>
                            <div id="plhShipping" runat="server" clientidmode="Static">
                                <div class="form-row row">
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto2") %>
                                        </label>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpNomeS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto2") %>' />
                                    </div>
                                    <div class="col-6 form-group">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto3") %>
                                        </label>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCognomeS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto3") %>' />
                                    </div>
                                </div>

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
                                    <div class="col-12 col-sm-4">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto7") %>
                                        </label>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpProvinciaS" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto7") %>' />
                                    </div>
                                    <div class="col-12 col-sm-4">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"FormTesto9") %>
                                        </label>
                                        <input type="text" enableviewstate="true" class="form-control" runat="server" id="inpCaps" placeholder='<%# references.ResMan("Common", Lingua,"FormTesto9") %>' />
                                    </div>
                                    <div class="col-12 col-sm-4">
                                        <label>
                                            <%= references.ResMan("Common", Lingua,"selezionaNazione") %>
                                        </label>
                                        <%--   <asp:DropDownList ID="ddlNazioneS" Enabled="true" OnSelectedIndexChanged="ddlNazioneS_SelectedIndexChanged" AutoPostBack="true"
                                            CssClass="form-control" Width="100%" runat="server" AppendDataBoundItems="true" />--%>
                                        <asp:DropDownList ID="ddlNazioneS" Enabled="true" onchange="refreshcarrello(this, '')"
                                            CssClass="form-control" Width="100%" runat="server" AppendDataBoundItems="true" />
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
                            </div>
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



                        <%-- NORME GENERALI DI ACQUISTO --%>
                        <div class="row">
                            <div class="col-lg-10 mt-4">
                                <%= references.ResMan("basetext", Lingua,"testoordine2") %>
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
                                                            <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"datapartenza", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedata")  + "</b>" +  
                                                             WelcomeLibrary.UF.Utility.reformatdatetimestring((string)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1"), "datapartenza", Lingua) ) ) : "" %>
                                                        </div>
                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"dataritorno", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglionedataritorno")  + "</b>" +  
                                                             WelcomeLibrary.UF.Utility.reformatdatetimestring((string)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1"), "dataritorno", Lingua) ) ) : "" %>
                                                        </div>


                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty((String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"idscaglione", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "scaglioneid")  + "</b>" +  WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"idscaglione", Lingua)  ):"" %>
                                                        </div>
                                                        <div class="product-categories muted">
                                                            <%# Eval("Datastart") !=null? "<b>" + references.ResMan("Common", Lingua,"formtestoperiododa") + ": " + "</b>" +  string.Format("{0:dd/MM/yyyy}", Eval("Datastart")):"" %>

                                                            <%# Eval("Dataend") !=null? "<b>" + references.ResMan("Common", Lingua,"formtestoperiodoa") + ": " + "</b>" + string.Format("{0:dd/MM/yyyy}", Eval("Dataend")) : "" %>
                                                        </div>

                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica1", Lingua)) ? ("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica1") + ": " + "</b>" +  references.TestoCaratteristica(0, (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica1", Lingua), Lingua)):"" %>
                                                        </div>
                                                        <div class="product-categories muted">
                                                            <%# !string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica2", Lingua)) ?("<b>" + references.ResMan("basetext", Lingua, "formtesto" + "Caratteristica2") + ": " + "</b>" +  references.TestoCaratteristica(1, (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"Caratteristica2", Lingua), Lingua)) : "" %>
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
                                                                (!string.IsNullOrEmpty( (String)WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"prezzo", Lingua)) ? (   WelcomeLibrary.DAL.eCommerceDM.Selezionadajson(Eval("jsonfield1") ,"prezzo", Lingua)  ): Eval("Offerta.Prezzo"))
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
                                                        <asp:Literal ID="Literal5" runat="server"
                                                            Text='<%#   String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",
                                                                  new object[] { (Double)Eval("TotaleAcconto") + (Double)Eval("TotaleSaldo") }  ) + " €" %>'></asp:Literal>
                                                    </span>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>


                    </div>

                    <%-- COLONNA METODO PAGAMENTO E ACQUISTO --%>
                    <div class="col-12 col-lg-5">
                        <div class="position-sticky" style="top: 120px" id="divColumnsticky">
                            <div class="mb-0 mb-sm-3 px-3 pt-1 pb-3 bg-light-color">
                                <asp:Literal Text="" ID="litMessage" runat="server" />
                                <%--<div class="widget bill-payment" style="text-align: left; display: none">
                                    <span class="TitlePrezzo"><%= references.ResMan("Common", Lingua,"TitleCodiceSconto") %></span>
                                    <asp:TextBox runat="server" ID="txtCodiceSconto" />
                                    <asp:Button Text='<%# references.ResMan("Common", Lingua,"testoBtnCodiceSconto") %>' runat="server" ID="btnCodiceSconto" OnClick="btnCodiceSconto_Click" />
                                    <span style="color: red">
                                        <asp:Literal Text="" ID="lblCodiceSconto" runat="server" /></span>
                                </div>--%>
                                <div class="widget bill-payment p-2" id="divPayment" runat="server">
                                    <h3><%= references.ResMan("Common", Lingua,"testoMetodopagamento") %></h3>

                                    <div class="col-12 mx-auto px-0 py-2">
                                        <ul class="unstyled m-0 p-0">
                                            <li style="display: block">
                                                <div class="clearfix" style="margin-bottom: 20px">
                                                    <div class="float-left mt-1 mr-2" style="width: 25px">
                                                        <input type="radio" class="form-control" style="background-color: transparent; cursor: pointer" disabled="false" checked="false" name="payment_method" value="contanti" validationgroup="none" autopostback="true" onclick="refreshcarrello(this, 'inpContanti')"
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
                                                        <input type="radio" class="form-control" style="background-color: transparent; cursor: pointer" name="payment_method" value="bacs" checked="false" onclick="refreshcarrello(this, 'inpBonifico')" validationgroup="none" autopostback="true" runat="server" id="inpBonifico" />
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
                                                        <input type="radio" class="form-control" style="background-color: transparent" value="payway" runat="server" id="inpPayway" autopostback="true" validationgroup="none" />
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
                                                        <input type="radio" class="form-control" style="background-color: transparent" disabled="false" checked="false" name="payment_method" value="paypal" runat="server" validationgroup="none" autopostback="true" id="inpPaypal" onclick="refreshcarrello(this, 'inpPaypal')" />
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
                                            <li style="display: none" id="listripe" runat="server">
                                                <div class="clearfix" style="margin-bottom: 20px">
                                                    <div class="float-left mt-0 mr-2" style="width: 25px;">
                                                        <input type="radio" class="form-control" style="background-color: transparent" disabled="false" checked="false" name="payment_method" value="stripe" runat="server" validationgroup="none" autopostback="true" id="inpstripe" onclick="refreshcarrello(this, 'inpstripe')" />
                                                    </div>
                                                    <div class="float-left" style="width: calc(100% - 30px - .5rem);">
                                                        <%= references.ResMan("Common", Lingua,"chkstripe") %>
                                                        <%--icone-carte di credito--%>
                                                        <div>
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
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>

                                <%--  BLOCCO ACCETTAZIONE CONDIZIONI DI VENDITA--%>
                                <div class="form-row row d-none">
                                    <div class="col-12">
                                        <label class="checkbox" style="margin: 5px 0 0 -2.5px;">
                                            <input type="checkbox" enableviewstate="true" class="form-control" runat="server" clientidmode="static" id="chkcondizioni" />
                                            <%= references.ResMan("Common", Lingua,"chkcondizioni") %><br />
                                        </label>
                                        <div class="pt-2" style="font-weight: bold; color: red">
                                            <asp:Label ID="outputcondizioni" ClientIDMode="Static" Text="" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="widget bill-payment p-2 col-12" id="divOrderrequest" runat="server" visible="false">
                                    <ul class="unstyled m-0 p-0">
                                        <li>
                                            <div class="clearfix" style="margin-bottom: 20px">
                                                <div class="float-left mt-1 mr-2" style="width: 25px">
                                                    <input type="radio" class="form-control" style="background-color: transparent; cursor: pointer" disabled="false" checked="false" name="payment_method" value="richiesta" validationgroup="none" onclick="refreshcarrello(this, 'inpRichiesta')"
                                                        runat="server" id="inpRichiesta" />
                                                </div>
                                                <div class="float-left" style="width: calc(100% - 30px - .5rem);">
                                                    <b><%= references.ResMan("Common", Lingua,"txtrichiesta") %></b>
                                                    <br />
                                                    <%= references.ResMan("Common", Lingua,"chkrichiesta") %>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <div class="col-12 col-sm-8 mx-auto px-0 py-1" id="stdbutton">
                                    <!- Normal button -->
                                        <button id="btnConvalida" type="button" class="btn w-100" runat="server" onclick="ConfirmValidationOrder(this);"><%= references.ResMan("Common", Lingua,"OrdineEsegui") %> </button>
                                    <asp:Button ID="btnConvalidaSrv" ClientIDMode="Static" Style="display: none" runat="server" OnClick="btnConvalidaOrdine" />
                                </div>

                                <div class="col-12 col-sm-8 mx-auto px-0 py-1" id="stripebutton">
                                    <!- Stripe button -->
                                        <button type="button" class="btn w-100" id="stripeproceedbtn" onclick="javascript:executeorder(this)">
                                            <%= references.ResMan("Common", Lingua,"OrdineEsegui") %> Stripe
                                        </button>
                                    <p id="card-error-pre" class="py-1" role="alert" style="color: red"></p>
                                </div>
                                <script>
                                    var stripechk = false;
                                    if ($("[id$='inpstripe']").length != 0)
                                        stripechk = $("[id$='inpstripe']")[0].checked;
                                    if (stripechk == true) { $("[id$='stripebutton']").show(); $("[id$='stdbutton']").hide(); }
                                    else { $("[id$='stripebutton']").hide(); $("[id$='stdbutton']").show(); }
                                </script>
                                <script>
                                    function ConfirmValidationOrder(elembtn) {
                                          <%--  var chkorder = document.getElementById("<%= chkcondizioni.ClientID  %>");
                                            var outorder = document.getElementById("<%= outputcondizioni.ClientID  %>");

                                            if (!chkorder.checked) {
                                                outorder.innerHTML = '<%= references.ResMan("Common", Lingua,"errchkcondizioni")%>';
                                                $('html,body').animate({
                                                    scrollTop: $("#" + "<%= outputcondizioni.ClientID  %>").offset().top - 190
                                                }, 'fast');
                                                return false;
                                            } else { outorder.innerHTML = ''; }--%>

                                        if (Page_ClientValidate("")) {
                                            /*do work and go for postback*/
                                            console.log('ok validated');
                                            $(elembtn).attr("disabled", "")
                                            var tastotxt = $(elembtn).html();
                                            $(elembtn).html("Wait ..");

                                            ///////////////////////////////////////////////////////////////////////
                                            //Invio con post back (metodo originale)
                                            var buttpost = document.getElementById("<%= btnConvalidaSrv.ClientID  %>");
                                            //$(elembtn).html("Wait ..");
                                            document.getElementById("<%= btnConvalidaSrv.ClientID  %>").click();
                                            ////////////////////////////////////////////////////////////////////////
                                              //$(elembtn).removeAttr("disabled")
                                            //$(elembtn).html(tastotxt);
                                        } else {
                                            $('html,body').animate({
                                                scrollTop: $("#" + "<%= validsummary.ClientID  %>").offset().top - 160
                                            }, 'fast');
                                            resetValidationState();
                                            console.log('not  validated');
                                            //return false;
                                        }
                                    }
                                    function resetValidationState() {
                                        // clear any postback blocks from client validation of asp.net validators, which occur after validation fails:
                                        window.Page_BlockSubmit = false;
                                    }
                                </script>

                                <div>
                                    <%= references.ResMan("Common", Lingua,"notespedizioni1") %>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--end:.row-->
            </div>
        </asp:Panel>
    </div>
    <%--SEZIONE CONTROLLO STRIPE MODAL--%>
    <!-- Modal stripe payments-->
    <section id="sectionfilters">
        <!--<div class="container searchbar-container py-3 text-center"><button class="btn btn-primary" type="button" data-toggle="modal" data-target="#stripeModal"> Apri stripe </button></div>-->
        <div class="modal fade bd-example-modal-lg" id="stripeModal" tabindex="-1" role="dialog" aria-labelledby="stripeModalLabel" aria-hidden="true">
            <div class="modal-dialog  modal-lg  " role="document">
                <div class="modal-content position-relative">
                    <div class="modal-header" style="background-color: #ccc">
                        <%--     <div class="float:left" style="background: url('/images/logo-dark.png') center bottom no-repeat !important; height: 50px; width: 50px; background-size: 110px !important;"></div>--%>
                        <h3 class="mbr-section-subtitle modal-title" id="stripeModalLabel">Pagamento ordine</h3>
                        <br />

                        <button class="close" type="button" data-dismiss="modal"><span aria-hidden="true">&times;</span> </button>
                    </div>
                    <div class="modal-body " style="background-color: #ddd; min-height: 220px; text-align: center">
                        <!-- Modal stripe card and buttot for payment -->
                        <p class="lead d-block">Inserisci i dati della carta per procedere al pagamento</p>
                        <!-- Display a payment form -->
                        <div id="payment-form-div">
                            <%--  <input type="text" id="email" placeholder="Email address" />--%>
                            <div id="card-element">
                            </div>
                            <button type="button" id="submit-payment-button">
                                <div class="spinnerstripe hidden" id="spinnerstripe"></div>
                                <span id="button-text">Paga adesso</span>
                            </button>
                            <p id="card-error" role="alert"></p>
                            <p id="card-completecode"></p>
                            <p class="result-message hidden">
                                <br />
                                <a href="" target="_blank">Link per verifica sul Stripe dashboard (solo per sviluppo).</a>
                            </p>
                        </div>
                    </div>
                    <div class="modal-footer position-relative" style="background-color: #ddd">
                        <!--<div class="w-100">
                        <div class="row justify-content-center">
                            <div class="col-auto py-1">
                            </div>
                        </div>
                    </div>-->
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal -->
    </section>
    <script>
        //PREPARO I MESSAGGI DI VALIDAZIONE PER LA FUNZIONE paymentmng.js
        var contactdatasgeneral = {};
        contactdatasgeneral.msgnome = '<%= references.ResMan("Common", Lingua, "FormTesto2Err") %>';
        contactdatasgeneral.msgcognome =  '<%= references.ResMan("Common", Lingua, "FormTesto3Err") %>';
        contactdatasgeneral.msgindirizzo =  '<%= references.ResMan("Common", Lingua, "FormTesto10Err") %>';
        contactdatasgeneral.msgcomune =  '<%= references.ResMan("Common", Lingua, "FormTesto8Err") %>';
        contactdatasgeneral.msgprovincia =  '<%= references.ResMan("Common", Lingua, "FormTesto7Err") %>';
        contactdatasgeneral.masgcap =  '<%= references.ResMan("Common", Lingua, "FormTesto9Err") %>';
        contactdatasgeneral.msgemail =  '<%= references.ResMan("Common", Lingua, "FormTesto4Err") %>';
        contactdatasgeneral.msgtel =  '<%= references.ResMan("Common", Lingua, "FormTesto11Err") %>';
        contactdatasgeneral.msgcondizioni =  '<%= references.ResMan("Common", Lingua, "chkcondizioni") %>';
        function showModalStripe() {
            $('#stripeModal').modal().show();  //modal per il pagamento stripe
        }
    </script>
    <%-- FINE SEZIONE CONTROLLO STRIPE MODAL--%>
</asp:Content>
