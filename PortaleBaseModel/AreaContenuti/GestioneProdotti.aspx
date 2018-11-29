<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="GestioneProdotti.aspx.cs" Inherits="AreaContenuti_Gestioneprodotti" MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript">
        function ConfirmCancella() {
            //document.getElementByID("CheckBoxListExCtrl").value
            var conferma = confirm('Sei sicuro di voler cancellare questa voce?');
            if (conferma) {
                $get("<%=cancelHidden.ClientID%>").value = "true";
            }
            else {
                $get("<%=cancelHidden.ClientID%>").value = "false";
            }
        }
        function aggiornaview() {
            //var ctrmail = $('#' + item);
            __doPostBack('aggiornatettaglio', '');
        }

        $(function () {
            tinymceinit();
        });
        function tinymceinit() {
            tinymce.init({
                mode: "textareas",
                editor_deselector: "mceNoEditor", // class="mceNoEditor" will not have tinyMCE
                extended_valid_elements: 'button[class|onclick|style|type|id|name],input[class|onclick|style|type|value|id|name|placeholder]',
                theme: "modern",
                convert_urls: false,
                relative_urls: false,
                forced_root_block: false,
                verify_html: false,
                allow_html_in_named_anchor: true,
                valid_children: "+a[div|i|span|h1|h2|h3|h4|h5|h6|p|#text]",
                plugins: [
                    "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                    "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                    "save table directionality emoticons template paste textcolor"
                ],
                menubar: false,
                toolbar_items_size: 'small',
                toolbar: "insertfile undo redo cut copy paste pastetext | table | bold italic underline superscript superscript | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link unlink anchor | code fullpage | charmap fontsizeselect forecolor backcolor | mybutton",
                setup: function (editor) {
                    editor.addButton('mybutton', {
                        text: 'Test',
                        icon: false,
                        onclick: function () {
                            editor.insertContent('Click Test Button');
                        }
                    });
                },
                style_formats: [
                    { title: 'Bold text', inline: 'b' },
                    { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
                    { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
                    { title: 'Example 1', inline: 'span', classes: 'example1' },
                    { title: 'Example 2', inline: 'span', classes: 'example2' },
                    { title: 'Table styles' },
                    { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
                ]
            });
        }

    </script>
    <input type="hidden" id="hidIdselected" runat="server" clientidmode="static" />
    <input type="hidden" id="hidTipologia" runat="server" clientidmode="static" />

    <div class="row" style="background-color: White; padding: 10px 10px 10px 10px">
        <div class="col-sm-5">
            <asp:HiddenField ID="cancelHidden" runat="server" Value="false" />
            <span style="font-size: 1.8rem; color: crimson">
                <asp:Literal ID="output" runat="server"></asp:Literal></span>
            <div style="margin-bottom: 30px">
                <h2>
                    <asp:Literal ID="litTitolo" runat="server"></asp:Literal>
                    - Selezione scheda struttura</h2>
                <hr />
                <div class="cerca">
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtinputCerca" Width="100%" placeholder="cerca .." />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 30px; margin-top: 10px;">
                        <%-- <div class="col-sm-4">
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtinputmese" />
                            <Ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtinputmese"
                                runat="server" WatermarkText="mese">
                            </Ajax:TextBoxWatermarkExtender>
                            <Ajax:FilteredTextBoxExtender ID="ftbe2" runat="server" TargetControlID="txtinputmese"
                                FilterType="Custom, Numbers" ValidChars="0123456789" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtinputanno" />
                            <Ajax:FilteredTextBoxExtender ID="ftbe1" runat="server" TargetControlID="txtinputanno"
                                FilterType="Custom, Numbers" ValidChars="0123456789" />
                            <Ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtinputanno"
                                runat="server" WatermarkText="anno">
                            </Ajax:TextBoxWatermarkExtender>
                        </div>--%>
                        <div class="col-sm-8">
                            <asp:DropDownList Width="100%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlSottoProdSearch"
                                runat="server" OnSelectedIndexChanged="ddlSottoProdSearch_SelectedIndexChange" />
                        </div>
                        <div class="col-sm-4">
                            <asp:Button runat="server" ID="ImgBtnCerca" OnClick="ImgBtnCerca_Click" CssClass="btn btn-primary btn-sm" Width="100%" Text="CERCA" />
                        </div>
                    </div>
                </div>


                <table class="table table-condensed">
                    <thead>
                        <tr>
                            <th></th>
                            <th>ID</th>
                            <th>Codice</th>
                            <th>Denominazione ITA</th>
                            <th>Link ITA</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptOfferte" OnItemDataBound="rptOfferte_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="border: Solid 1px #ccc;">
                                        <asp:LinkButton runat="server" ID="imgSelect" CommandArgument='<%# Eval("Id").ToString() %>' OnPreRender="ImageButton1_PreRender"
                                            OnClick="link_click" ToolTip='<%# Eval("Id").ToString() %>'>
                                            <i id="imgButton" runat="server" class="fa fa-search fa-2x"></i>
                                        </asp:LinkButton>
                                    </td>

                                    <td style="border: Solid 1px #ccc;">
                                        <div style="height: 50px; overflow-y: auto">
                                            <a onclick="JsSvuotaSession(this)" target="_blank" href="<%# CreaLinkRoutes(null,false,"I",Eval("DenominazioneI").ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString())    %>">
                                                <asp:Literal ID="Literal4" runat="server" Text='<%# Eval("Id").ToString() %>'></asp:Literal></a>

                                        </div>
                                    </td>
                                    <td style="border: Solid 1px #ccc;">
                                        <div style="height: 50px; overflow-y: auto">
                                            <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("CodiceProdotto").ToString() %>'></asp:Literal>
                                        </div>
                                    </td>

                                    <td style="border: Solid 1px #ccc;">
                                        <div style="height: 50px; overflow-y: auto">
                                            <asp:Literal ID="lit1" runat="server" Text='<%# Eval("DenominazioneI").ToString() %>'></asp:Literal>
                                        </div>
                                    </td>
                                    <td style="border: Solid 1px #ccc;">
                                        <div style="height: 50px; overflow-y: auto">
                                            <a target="_blank" href="<%# CreaLinkRoutes(null,false,"I",Eval("DenominazioneI").ToString(),Eval("Id").ToString(),Eval("CodiceTipologia").ToString())    %>">view
                                        </div>
                                    </td>
                                    <%--<td style="border: Solid 1px Black;">
                                <div style="height: 50px; overflow-y: auto">
                                    <asp:Literal ID="lit2" runat="server" Text='<%# Eval("DescrizioneI").ToString() %>'></asp:Literal>
                                </div>
                            </td>--%>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                <div id="pager" class="divPager">
                    <UC:PagerEx ID="PagerRisultati" runat="server" NavigateUrl="" PageSize="20" CurrentPage="1"
                        TotalRecords="0" dimensioneGruppo="20" nGruppoPagine="1" OnPageCommand="PagerRisultati_PageCommand"
                        OnPageGroupClickNext="PagerRisultati_PageGroupClickNext" OnPageGroupClickPrev="PagerRisultati_PageGroupClickPrev" />
                </div>
            </div>
            <div style="margin-bottom: 30px">
                <h2>Caricamento foto</h2>
                <script type="text/javascript">
                    $(document).ready(function () {
                        startDropZone();
                    });

                </script>
                <div style="overflow-y: auto; height: 370px; width: 600px">
                    <div id="dzContainer">
                        <div id="dZUpload" class="dropzone" style="background-color: lightyellow; min-height: 370px">
                            <div class="dz-default"></div>
                        </div>
                    </div>
                </div>


                <div id="preview-template" style="display: none;">
                    <div class="dz-preview dz-file-preview">
                        <div class="dz-details">
                            <div class="dz-filename"><span data-dz-name></span></div>
                            <div class="dz-size" data-dz-size></div>
                            <img data-dz-thumbnail />
                        </div>
                        <div class="dz-progress"><span class="dz-upload" data-dz-uploadprogress></span></div>
                        <div class="dz-success-mark"><span>✔</span></div>
                        <div class="dz-error-mark"><span>✘</span></div>
                        <div class="dz-error-message"><span data-dz-errormessage></span></div>
                    </div>
                </div>


                <table width="100%" style="margin-top: 10px; font-size: 12px; table-layout: fixed; border-collapse: collapse;"
                    cellpadding="0" cellspacing="0">
                    <tr>
                        <td id="Foto" runat="server" style="width: 60%; vertical-align: top">
                            <h3>
                                <asp:Literal ID="litFoto" runat="server" Text=""></asp:Literal></h3>
                            <div>
                                <asp:FileUpload ID="UploadFoto" CssClass="btn btn-default btn-sm" runat="server" />
                                <br />
                                Descrizione
                                    <br />
                                <asp:TextBox CssClass="mceNoEditor form-control" runat="server" ID="txtDescrizione" />
                                <br />
                                Progressivo
                                <br />
                                <asp:TextBox CssClass="mceNoEditor form-control" runat="server" ID="txtProgressivo" />
                                <br />
                                <asp:Button ID="btnCarica" runat="server" CssClass="btn btn-primary btn-sm" Text="Carica Foto" OnClick="btnCarica_Click" />
                                <asp:Button ID="btnModifica" runat="server" CssClass="btn btn-primary btn-sm" Text="Modifica Descrizione Foto" OnClick="btnModifica_Click" />
                                <asp:Button ID="btnElimina" runat="server" CssClass="btn btn-danger btn-sm" Text="Elimina Foto" OnClick="btnElimina_Click" />
                                <br />
                                <br />
                            </div>
                            <div style="width: 100%;">
                                <asp:HiddenField ID="txtFotoSchema" runat="server" />
                                <asp:HiddenField ID="txtFotoValori" runat="server" />
                                <div style="width: 100%; float: left; height: 400px; padding: 10px; overflow: hidden">
                                    <a id="linkFoto" runat="server" target="_blank">
                                        <asp:Image ID="imgFoto" Width="100%" runat="server" ImageUrl="" /></a>
                                </div>
                                <div style="clear: both"></div>
                                <div style="float: left; padding: 10px; width: 100%">
                                    <ul style="list-style: none; display: inline">
                                        <asp:Repeater runat="server" ID="rptImmagini">
                                            <ItemTemplate>
                                                <li style="list-style: none; display: inline">
                                                    <%--   <asp:ImageButton Width="80px" Height="80px" ToolTip='<%# Eval("Descrizione").ToString() %>' ID="imgAntFoto" CommandArgument='<%# Eval("NomeFile").ToString() %>' OnClick="linkgalleria_click" runat="server" ImageUrl='<%# ComponiUrlGalleriaAnteprima(Eval("NomeAnteprima").ToString()) %>' />--%>
                                                    <asp:ImageButton Width="80px" Height="80px" ToolTip='<%# Eval("Descrizione").ToString() %>' ID="imgAntFoto" CommandArgument='<%# 
                                                         Newtonsoft.Json.JsonConvert.SerializeObject((WelcomeLibrary.DOM.Allegato)Container.DataItem, Newtonsoft.Json.Formatting.Indented) %>'
                                                        OnClick="linkgalleria_click" runat="server" ImageUrl='<%# WelcomeLibrary.UF.filemanage.ComponiUrlAnteprima(Eval("NomeAnteprima").ToString(),TipologiaOfferte,OffertaIDSelected) %>' />
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                                <div style="clear: both"></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

        </div>
        <div class="col-sm-7">
            <h2>Dettaglio scheda</h2>
            <hr />
            <asp:Button ID="btnNuovo" runat="server" CssClass="btn btn-primary btn-sm" Text="Nuovo" OnClick="btnNuovo_Click" />
            <asp:Button ID="btnAggiorna" runat="server" CssClass="btn btn-primary btn-sm" Text="Modifica" OnClick="btnAggiorna_Click" />
            <asp:Button ID="btnCancella" runat="server" CssClass="btn btn-danger btn-sm" Text="Cancella" OnClick="btnCancella_Click"
                OnClientClick="javascript:ConfirmCancella()" UseSubmitBehavior="true" />
            <asp:Button ID="btnAnnulla" runat="server" CssClass="btn btn-default btn-sm" Text="Annulla" OnClick="btnAnnulla_Click"
                Visible="false" />
            <br />
            <br />
            <div style="background: #ccc; padding: 10px 30px 30px 30px;">

                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label18" runat="server" Text="Vetrina" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:CheckBox ID="chkVetrina" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label39" runat="server" Text="Promozione" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:CheckBox ID="chkPromozione" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label26" runat="server" Text="Archiviato" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:CheckBox ID="chkArchiviato" runat="server" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label16" runat="server" Text="Categoria 1 Livello" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlProdotto"
                            OnSelectedIndexChanged="ddlProdotto_SelectedIndexChanged" runat="server" Enabled="true" />
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Categoria Prodotto Obbligatoria"
                            ControlToValidate="ddlProdotto" runat="server" Text="*" ValidationGroup="Insertvalidate" />--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label17" runat="server" Text="Categoria 2 Livello" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" ID="ddlSottoProdotto" runat="server" />
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="SottoCategoria Prodotto Obbligatoria"
                                ControlToValidate="ddlSottoProdotto" runat="server" Text="*" ValidationGroup="Insertvalidate" />--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label12" runat="server" Text="CODICE PRODOTTO" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCodiceProd" runat="server"></asp:TextBox>
                        <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="CodiceProdotto Obbligatorio"
                                ControlToValidate="txtCodiceProd" runat="server" Text="*" ValidationGroup="Insertvalidate" />--%>
                    </div>
                </div>
                <div class="row">
                    <hr />
                    <ul class="nav nav-pills" style="margin-left: 0;">
                        <li class="active"><a data-toggle="pill" href="#promoita">Italiano</a></li>
                        <li><a data-toggle="pill" href="#promoeng">Inglese</a></li>
                        <li><a data-toggle="pill" href="#promoru">Russo</a></li>
                    </ul>
                    <div class="tab-content">
                        <div id="promoita" class="tab-pane fade in active">
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label13" runat="server" Text="Meta Title(opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo1I" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Labelps1" runat="server" Text="Meta Description (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo2I" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="litDenominazioneI" runat="server" Text="Denominazione Ita" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" TextMode="MultiLine" Height="50" ID="txtDenominazioneI" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="litDescrizioneI" runat="server" Text="Descrizione Ita" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="  form-control" Height="150px" TextMode="MultiLine" ID="txtDescrizioneI"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label1" runat="server" Text="Dettagli Ita" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtDatitecniciI"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="promoeng" class="tab-pane fade">
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Labelpre" runat="server" Text="Meta Title Eng (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo1GB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Labelprse" runat="server" Text="Meta Description Eng (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo2GB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Literal2" runat="server" Text="Denominazione Eng" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" TextMode="MultiLine" Height="50" ID="txtDenominazioneGB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Literal3" runat="server" Text="Descrizione Eng" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="  form-control" Height="150px" TextMode="MultiLine" ID="txtDescrizioneGB"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label2" runat="server" Text="Dettaglio Eng" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtDatitecniciGB"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>

                        </div>



                        <div id="promoru" class="tab-pane fade">
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label33" runat="server" Text="Testo promo Ru" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo1RU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label34" runat="server" Text="Testo promo sconto Ru" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo2RU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label35" runat="server" Text="Denominazione Ru" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" TextMode="MultiLine" Height="50" ID="txtDenominazioneRU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label36" runat="server" Text="Descrizione Ru" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="  form-control" Height="150px" TextMode="MultiLine" ID="txtDescrizioneRU"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label37" runat="server" Text="Dettaglio Ru" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtDatitecniciRU"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>

                        </div>


                    </div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Insertvalidate" />
                    <hr />
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label40" runat="server" Text="Autore" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtAutore" runat="server" value="" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label9" runat="server" Text="Regione" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlRegione"
                            OnSelectedIndexChanged="ddlRegione_SelectedIndexChanged" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label10" runat="server" Text="Provincia" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"
                            AutoPostBack="true" ID="ddlProvincia" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label8" runat="server" Text="Comune" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" ID="ddlComune" runat="server" />
                    </div>
                </div>
                <hr />

                <asp:Panel runat="server" Visible="false">

                    <div class="row">
                        <div class="col-sm-2 item-text">
                            <strong>
                                <asp:Label ID="Label23" runat="server" Text="Caratteristica 1" /></strong>
                        </div>
                        <div class="col-sm-10">
                            <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" ID="ddlCaratteristica1" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 item-text">
                            <strong>
                                <asp:Label ID="Label24" runat="server" Text="Caratteristica 2" /></strong>
                        </div>
                        <div class="col-sm-10">
                            <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" ID="ddlCaratteristica2" runat="server" />
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-sm-2 item-text">
                            <strong>
                                <asp:Label ID="Label27" runat="server" Text="Caratteristica 3" /></strong>
                        </div>
                        <div class="col-sm-10">
                            <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" ID="ddlCaratteristica3" runat="server" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-2 item-text">
                            <strong>
                                <asp:Label ID="Label28" runat="server" Text="Caratteristica 4" /></strong>
                        </div>
                        <div class="col-sm-10">
                            <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" ID="ddlCaratteristica4" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 item-text">
                            <strong>
                                <asp:Label ID="Label29" runat="server" Text="Caratteristica 5" /></strong>
                        </div>
                        <div class="col-sm-10">
                            <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" ID="ddlCaratteristica5" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 item-text">
                            <strong>
                                <asp:Label ID="Label30" runat="server" Text="Caratteristica 6" /></strong>
                        </div>
                        <div class="col-sm-10">
                            <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" ID="ddlCaratteristica6" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 item-text">
                            <strong>
                                <asp:Label ID="Label22" runat="server" Text="Anno ( 4 cifre )" /></strong>
                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox CssClass="mceNoEditor form-control" ID="txtAnno" runat="server"></asp:TextBox>
                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtAnno"
                                FilterMode="ValidChars" ValidChars="0123456789" />
                        </div>
                    </div>
                    <hr />
                </asp:Panel>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label19" runat="server" Text="Prezzo Listino &euro;" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtPrezzoListino" runat="server"></asp:TextBox>
                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPrezzoListino"
                            FilterMode="ValidChars" ValidChars="0123456789," />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Labelprezzo" runat="server" Text="Prezzo Web &euro;" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtPrezzo" runat="server"></asp:TextBox>
                        <Ajax:FilteredTextBoxExtender ID="ftbe" runat="server" TargetControlID="txtPrezzo"
                            FilterMode="ValidChars" ValidChars="0123456789," />
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label Width="30%" ID="Label38" runat="server" Text="Quantità disponibile" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor" Width="60%" ID="txtQta_vendita" runat="server"></asp:TextBox>
                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtQta_vendita"
                            FilterMode="ValidChars" ValidChars="0123456789," />
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label3" runat="server" Text="Indirizzo" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtIndirizzo" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label4" runat="server" Text="Email" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtEmail" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label5" runat="server" Text="Website" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtWebsite" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label6" runat="server" Text="Telefono" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtTelefono" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label7" runat="server" Text="Fax" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtFax" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label20" runat="server" Text="Videolink" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtVideo" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label11" runat="server" Text="Imposta Data (opzionale)" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtData" runat="server"></asp:TextBox>
                        <Ajax:CalendarExtender ID="cal2" runat="server" Format="dd/MM/yyyy HH.mm.ss" TargetControlID="txtData">
                        </Ajax:CalendarExtender>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label15" runat="server" Text="Abilita contatto" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:CheckBox ID="chkContatto" runat="server" Checked="false"></asp:CheckBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label21" runat="server" Text="Id collegato" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtIdcollegato" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label31" runat="server" Text="Latitudite" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtLatitudine1_dts" runat="server" value="" />
                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtLatitudine1_dts"
                            FilterType="Custom, Numbers" ValidChars="0123456789," />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label32" runat="server" Text="Longitudine" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ID="txtLongitudine1_dts" runat="server" value="" />
                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtLongitudine1_dts"
                            FilterType="Custom, Numbers" ValidChars="0123456789," />
                    </div>
                </div>
                <asp:Panel runat="server" ID="pnlProdottiAssociati" Visible="false">
                    <script>
                        var propcollegati = [];
                        var proprietatmp = {};
                        var caratteristica2 = {};
                        var caratteristica1 = {};
                        $(document).ready(function () {
                            initCaratteristicheCollegate();
                        });
                        function initCaratteristicheCollegate() {
                            $("#txtCombCar1").autocomplete({
                                source: pathAbs + '/lib/hnd/HandlerDataCommon.ashx' + '?q=autocompletecaratteristiche&progressivo=0',
                                minLength: 0,
                                change: function (event, ui) {
                                    onchangeScelta(this);
                                },
                                select: function (event, ui) {
                                    if (ui.item != null) {
                                        $("#txtCombCar1").text(ui.item.Campo1);
                                        caratteristica1 = {};
                                        caratteristica1.id = ui.item.id;
                                        caratteristica1.value = ui.item.value;
                                        caratteristica1.codice = ui.item.codice;
                                    }
                                },
                                open: function () {
                                    //console.log("OPEN");
                                    setTimeout(function () {
                                        $('.ui-autocomplete').css('z-index', 99999999999999);
                                    }, 0);
                                }
                            }).on("focus", function () {
                                $(this).autocomplete("search", "");
                            });
                            $("#txtCombCar2").autocomplete({
                                source: pathAbs + '/lib/hnd/HandlerDataCommon.ashx' + '?q=autocompletecaratteristiche&progressivo=1',
                                minLength: 0,
                                change: function (event, ui) {
                                    onchangeScelta(this);
                                },
                                select: function (event, ui) {
                                    if (ui.item != null) {
                                        $("#txtCombCar2").text(ui.item.Campo1);
                                        caratteristica2 = {};
                                        caratteristica2.id = ui.item.id;
                                        caratteristica2.value = ui.item.value;
                                        caratteristica2.codice = ui.item.codice;
                                    }
                                },
                                open: function () {
                                    //console.log("OPEN");
                                    setTimeout(function () {
                                        $('.ui-autocomplete').css('z-index', 99999999999999);
                                    }, 0);
                                }
                            }).on("focus", function () {
                                $(this).autocomplete("search", "");
                            });

                            propcollegati = [];
                            proprietatmp = {};
                            caratteristica2 = {};
                            caratteristica1 = {};
                            var jsonvalues = $("#hProdotticollegati").val();
                            if (jsonvalues !== null && jsonvalues !== '')
                                propcollegati = JSON.parse(jsonvalues);
                            ShowList('', 'divcontainerlist', 'listapcollegati', propcollegati);
                        }
                        function eliminafiltri() {
                            $("#txtCombCar1").val('');
                            $("#txtCombCar2").val('');
                            $("#txtQtacollegati").val('');
                            proprietatmp = {};
                            caratteristica1 = {};
                            caratteristica2 = {};
                        }
                        function emptycollegato(el, idcollegato) {
                            if ($(el)[0].value === '')
                                $("#" + idcollegato).val('');
                        }
                        function addupdateprodottocollegato(el) {
                            var found = false;
                            if (caratteristica1.codice != null && caratteristica1.codice != "" && caratteristica2.codice != null && caratteristica2.codice != "") {
                                var tmpId = caratteristica1.codice + "-" + caratteristica2.codice;//Separo con carattere per evitare conflitti di id caratteristiche diverse
                                for (var j = 0; j < propcollegati.length; j++) {
                                    if (propcollegati[j].hasOwnProperty('id')) {
                                        if (propcollegati[j].id == tmpId) {
                                            found = true;
                                            propcollegati[j].qta = $('#txtQtacollegati').val();
                                        }
                                    }
                                }
                                if (!found) {
                                    proprietatmp.id = tmpId;
                                    proprietatmp.qta = $('#txtQtacollegati').val();
                                    proprietatmp.caratteristica1 = {};
                                    proprietatmp.caratteristica1 = caratteristica1;
                                    proprietatmp.caratteristica2 = {};
                                    proprietatmp.caratteristica2 = caratteristica2;
                                    //console.log(proprietatmp);
                                    var prodottoclone = (JSON.parse(JSON.stringify(proprietatmp)));
                                    propcollegati.push(prodottoclone);
                                }
                                $("#hProdotticollegati").val(JSON.stringify(propcollegati));
                                ShowList('', 'divcontainerlist', 'listapcollegati', propcollegati);
                            }
                            else {
                                if (caratteristica2.codice == null || caratteristica2.codice == "") {
                                    $('#txtCombCar2').addClass('has-error');
                                    $('#errorcombCar2').removeClass('spento');
                                }
                                if (caratteristica1.codice == null || caratteristica1.codice == "") {
                                    $('#txtCombCar1').addClass('has-error');
                                    $('#errorcombCar1').removeClass('spento');
                                }
                            }
                        }
                        function selectProdotto(idp) {
                            var found = false;
                            var jfound = 0;
                            for (var j = 0; j < propcollegati.length; j++) {
                                //console.log(propcollegati[j]);
                                if (propcollegati[j].hasOwnProperty('id')) {
                                    if (propcollegati[j].id == idp) {
                                        found = true;
                                        jfound = j;
                                        proprietatmp = propcollegati[j];
                                        caratteristica2 = {};
                                        caratteristica1 = {};
                                        caratteristica1 = proprietatmp.caratteristica1;
                                        caratteristica2 = proprietatmp.caratteristica2;
                                    }
                                }
                            }
                            if (found) {
                                $("#txtCombCar1").attr("value", proprietatmp.caratteristica1.value);
                                $("#txtCombCar2").attr("value", proprietatmp.caratteristica2.value);
                                $("#txtQtacollegati").attr("value", proprietatmp.qta);
                            }
                        }


                        function cancelProdotto(idp) {
                            if (confirm('Vuoi cancellare il prodotto corrente?')) {
                                //cancello dalla lista il selezionato
                                var pulito = $.grep(propcollegati, function (e) {
                                    return e.id != idp;
                                });
                                propcollegati = pulito;
                                if (propcollegati.length != 0)
                                    $("#hProdotticollegati").val(JSON.stringify(propcollegati));
                                else
                                    $("#hProdotticollegati").val('');

                                //SE selezionato l'elemento che cancello lo elimino dalla memoria temporanea
                                if (proprietatmp != null && proprietatmp.hasOwnProperty('id') && idp == proprietatmp.id) {
                                    proprietatmp = {};
                                    eliminafiltri();
                                }
                                ShowList('', 'divcontainerlist', 'listapcollegati', propcollegati);
                            }
                        }

                        function onchangeScelta(elem) {
                            proprietatmp = {};
                            if ($(elem)[0].value != "" && $(elem)[0].value != null) {
                                $(elem).removeClass('has-error');
                                if ($(elem)[0].id.indexOf('txtCombCar1') != -1)
                                    $('#errorcombCar1').addClass('spento');
                                else $('#errorcombCar2').addClass('spento');
                            }
                            else {
                                $(elem).addClass('has-error');
                                if ($(elem)[0].id.indexOf('txtCombCar1') != -1)
                                    $('#errorcombCar1').removeClass('spento');
                                else $('#errorcombCar2').removeClass('spento');
                            }
                        }
                    </script>
                    <input type="hidden" id="hProdotticollegati" runat="server" clientidmode="static" />
                    <hr />
                    <div class="row">
                        <style>
                            #listapcollegati span {
                                font-size: 16px;
                            }

                            .ui-menu .ui-menu-item {
                                font-size: 16px !important;
                            }
                        </style>
                        <div class="col-sm-12">
                            <h2>Aggiunta Caratteristiche e Disponibilità</h2>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4">
                            <strong><%= references.ResMan("BaseText", Lingua, "selectcat1") %></strong>
                            <br />
                            <input id="txtCombCar1" type="text" class="form-control" onchange="onchangeScelta(this)" onkeyup="onchangeScelta(this)" />
                            <p class="error spento" id="errorcombCar1">* Taglia obbligatoria</p>

                        </div>
                        <div class="col-sm-4">
                            <strong><%= references.ResMan("BaseText", Lingua, "selectcat2") %></strong>
                            <br />
                            <input id="txtCombCar2" type="text" class="form-control" onchange="onchangeScelta(this)" onkeyup="onchangeScelta(this)" />
                            <p class="error spento" id="errorcombCar2">* Valore obbligatorio</p>
                        </div>
                        <div class="col-sm-2">
                            <strong>Qta</strong>
                            <br />
                            <input id="txtQtacollegati" type="text" class="form-control" value="1" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <button id="btnAddcollegato" type="button" class="btn btn-primary btn-sm" onclick="javascript:addupdateprodottocollegato()">
                                Aggiungi
                                    <br />
                                Aggiorna</button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="divColTagl" id="divcontainerlist">
                            </div>
                        </div>
                    </div>

                </asp:Panel>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <div style="margin-bottom: 30px">
                <hr />
                <h2>
                    <asp:Label runat="server" ID="TitleGestione" Text="Area Modifica / Inserimento CATEGORIE "></asp:Label>
                </h2>
                <hr />
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div style="margin-left: auto; padding: 0px 10px 0px 10px; border: 1px solid #000000;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div>
                                                <h2>
                                                    <%= references.ResMan("Common",Lingua,"TitleProdottiGest") %></h2>
                                            </div>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 150px;">
                                                        <asp:Label runat="server" ID="lblTipologia" Text="Tipologia 1 Livello"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList Width="90%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlTipologiaNewProd"
                                                            OnSelectedIndexChanged="TipologiaProd_SelectedIndexChanged" runat="server" Enabled="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblsceltaprod" Text="Scelta  1 Livello"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList Width="100%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlProdottoNewProd1"
                                                            runat="server" OnSelectedIndexChanged="ddlProdottoNewProd1_SelectedIndexChanged" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="height: 35px;">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="NomeIta" Text="Nome 1 Livello Ita"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="mceNoEditor" ID="NomeNuovoProdIt" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="NomeEng" Text="Nome 1 Livello Eng"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="mceNoEditor" ID="NomeNuovoProdEng" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="NomeRu" Text="Nome 1 Livello Ru"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="mceNoEditor" ID="NomeNuovoProdRu" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <br />
                                            <asp:Button ID="OkButton" runat="server" CssClass="btn btn-primary btn-sm" Text="Nuovo" OnClick="btnInsertNewProd_Click" />
                                            <asp:Button ID="btnModificaProd" runat="server" CssClass="btn btn-primary btn-sm" Text="Modifica" OnClick="btnModifiProd_Click" />
                                            <asp:Button ID="btnEliminaProdotto" runat="server" CssClass="btn btn-danger btn-sm" Text="Elimina" OnClick="btnEliminaProd_Click" /><br />
                                            <br />
                                            <asp:Label runat="server" ID="ErrorMsgNuovoProdotto" ForeColor="Red"></asp:Label>


                                            <br />
                                            <asp:Literal Text="" ID="linksezioneI" runat="server" />
                                            <br />
                                            <asp:Literal Text="" ID="linksezioneGB" runat="server" />
                                            <br />
                                            <asp:Literal Text="" ID="linksezioneRU" runat="server" />

                                            <div style="height: 50px;">
                                            </div>
                                        </td>
                                        <td>
                                            <%-- Qui aggiungiamo le dll di selezione per la modifica del SottoProdotto--%>
                                            <div style="margin-left: 50px; border-left: 1px solid #000000; padding-left: 10px;">
                                                <div>
                                                    <h2>
                                                        <%= references.ResMan("Common",Lingua,"TitleSottProdottiGest") %></h2>
                                                </div>
                                                <table>
                                                    <tr>
                                                        <td style="width: 150px;">
                                                            <asp:Label runat="server" ID="lblTipologiaSott" Text="Tipologia 1 Livello"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList Width="100%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlTipologiaNewSottProd"
                                                                runat="server" Enabled="false" />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblProdotto" Text="Scelta 1 Livello"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList Width="100%" AppendDataBoundItems="true" ID="ddlProdottoNewProd"
                                                                AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProdottoNewProd_SelectedIndexChange" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="Label14" Text="Scelta 2 Livello"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList Width="100%" AppendDataBoundItems="true" ID="ddlProdottoNewSProd"
                                                                runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProdottoNewSProd_SelectedIndexChange" />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="NomeSottIta" Text="Nome 2 Livello Ita"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="mceNoEditor" ID="NomeNuovoSottIt" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="NomeSottEng" Text="Nome 2 Livello Eng"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="mceNoEditor" ID="NomeNuovoSottEng" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="NomeSottRu" Text="Nome 2 Livello Ru"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="mceNoEditor" ID="NomeNuovoSottRu" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                                <asp:Button ID="OkButton2" runat="server" CssClass="btn btn-primary btn-sm" Text="Nuovo" OnClick="btnInsertNewSottProd_Click" />
                                                <asp:Button ID="btnModificaSottoProd" runat="server" CssClass="btn btn-primary btn-sm" Text="Modifica" OnClick="btnModificaSottProd_Click" />
                                                <asp:Button ID="btnEliminaSottoProd" runat="server" CssClass="btn btn-danger btn-sm" Text="Elimina" OnClick="btnEliminaSottProd_Click" /><br />
                                                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>

                                                <br />
                                                <asp:Literal Text="" ID="linksottosezioneI" runat="server" />
                                                <br />
                                                <asp:Literal Text="" ID="linksottosezioneGB" runat="server" />
                                                <br />
                                                <asp:Literal Text="" ID="linksottosezioneRU" runat="server" />

                                            </div>
                                            <div style="height: 50px;">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <script>
                                    function bloccaSblocca(idDiv) {
                                        $('#' + idDiv).attr('style', 'position: absolute; top: 0; bottom: 0; z-index: 2; heigth:100%; width:100%; background-color:rgba(0,0,0,0.2);');
                                    }
                                </script>
                            </div>
                        </td>
                    </tr>
                </table>

                <hr />


                <h2>
                    <asp:Label runat="server" ID="Label25" Text="Gestione tabelle riferimento"></asp:Label>
                </h2>
                <hr />
                <div style="background-color: #fafafa; position: relative; z-index: 1;">
                    <div id="divBlockOverlay1" style="position: absolute; top: 0; bottom: 0; z-index: 2"></div>
                    <h3>Tabella Caratteristica 1 / Formato</h3>
                    <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica1_gest" AutoPostBack="true"
                        AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica1update">
                    </asp:DropDownList><br />
                    <asp:Literal Text="" ID="litCodp" runat="server" />
                    <br />
                    Descrizione italiano:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar1I" Text="" Width="300" /><br />
                    Descrizione inglese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar1GB" Text="" Width="300" /><br />
                    Descrizione Russo:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar1RU" Text="" Width="300" /><br />

                    <Ajax:TextBoxWatermarkExtender runat="server" ID="w1" WatermarkText="Inserire  (Italiano)" TargetControlID="txtCar1I">
                    </Ajax:TextBoxWatermarkExtender>
                    <Ajax:TextBoxWatermarkExtender runat="server" ID="w2" WatermarkText="Inserire  (Inglese)" TargetControlID="txtCar1GB">
                    </Ajax:TextBoxWatermarkExtender>
                    <Ajax:TextBoxWatermarkExtender runat="server" ID="w3" WatermarkText="Inserire  (Russo)" TargetControlID="txtCar1RU">
                    </Ajax:TextBoxWatermarkExtender>
                    <asp:Button Text="Aggiorna/Inserisci" ID="btnAggiornaCaratteristica1" runat="server" OnClick="btnAggiornaCaratteristica1_Click" OnClientClick="bloccaSblocca('divBlockOverlay1')" />
                    <br />
                </div>

                <div style="background-color: #fafafa; position: relative; z-index: 1;">
                    <div id="divBlockOverlay2" style="position: absolute; top: 0; bottom: 0; z-index: 2"></div>
                    <h3>Tabella Caratteristica 2 / Colore</h3>
                    <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica2_gest" AutoPostBack="true"
                        AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica2update">
                    </asp:DropDownList><br />
                    Descrizione italiano:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar2I" Text="" Width="300" /><br />
                    Descrizione inglese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar2GB" Text="" Width="300" /><br />
                    Descrizione russo:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar2RU" Text="" Width="300" /><br />

                    <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender4" WatermarkText="Inserire  (Italiano)" TargetControlID="txtCar2I">
                    </Ajax:TextBoxWatermarkExtender>
                    <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender5" WatermarkText="Inserire  (Inglese)" TargetControlID="txtCar2GB">
                    </Ajax:TextBoxWatermarkExtender>
                    <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender14" WatermarkText="Inserire  (Russo)" TargetControlID="txtCar2RU">
                    </Ajax:TextBoxWatermarkExtender>
                    <asp:Button Text="Aggiorna/Inserisci" ID="btnAggiornaCaratteristica2" runat="server" OnClick="btnAggiornaCaratteristica2_Click" OnClientClick="bloccaSblocca('divBlockOverlay2')" />
                    <br />
                </div>

                <asp:Panel runat="server" ID="pnlGestioneCaratteristiche" Visible="false">



                    <div style="background-color: #fafafa">
                        <h3>Tabella Caratteristica 3</h3>
                        <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica3_gest" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica3update">
                        </asp:DropDownList><br />
                        Descrizione italiano:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar3I" Text="" Width="300" /><br />
                        Descrizione inglese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar3GB" Text="" Width="300" /><br />
                        Descrizione Russo:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar3RU" Text="" Width="300" /><br />
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender6" WatermarkText="Inserire Provenienza (Italiano)" TargetControlID="txtCar3I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender7" WatermarkText="Inserire Provenienza (Inglese)" TargetControlID="txtCar3GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender15" WatermarkText="Inserire Provenienza (Russo)" TargetControlID="txtCar3RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <asp:Button Text="Aggiorna/Inserisci" ID="Button1" runat="server" OnClick="btnAggiornaCaratteristica3_Click" />
                        <br />
                    </div>


                    <div style="background-color: #fafafa">
                        <h3>Tabella Caratteristica 4</h3>
                        <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica4_gest" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica4update">
                        </asp:DropDownList><br />
                        Descrizione italiano:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar4I" Text="" Width="300" /><br />
                        Descrizione inglese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar4GB" Text="" Width="300" /><br />
                        Descrizione russo:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar4RU" Text="" Width="300" /><br />
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender8" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar4I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender9" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar4GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender16" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar4RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <asp:Button Text="Aggiorna/Inserisci" ID="Button2" runat="server" OnClick="btnAggiornaCaratteristica4_Click" />
                        <br />
                    </div>

                    <div style="background-color: #fafafa">
                        <h3>Tabella Caratteristica 5</h3>
                        <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica5_gest" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica5update">
                        </asp:DropDownList><br />
                        Descrizione italiano:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar5I" Text="" Width="300" /><br />
                        Descrizione inglese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar5GB" Text="" Width="300" /><br />
                        Descrizione Russo:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar5RU" Text="" Width="300" /><br />
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender10" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar5I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender11" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar5GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender17" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar5RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <asp:Button Text="Aggiorna/Inserisci" ID="Button3" runat="server" OnClick="btnAggiornaCaratteristica5_Click" />
                        <br />
                    </div>

                    <div style="background-color: #fafafa">
                        <h3>Tabella Caratteristica 6</h3>
                        <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica6_gest" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica6update">
                        </asp:DropDownList><br />
                        Descrizione italiano:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar6I" Text="" Width="300" /><br />
                        Descrizione inglese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar6GB" Text="" Width="300" /><br />
                        Descrizione russo:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar6RU" Text="" Width="300" /><br />
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender12" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar6I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender13" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar6GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender18" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar6RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <asp:Button Text="Aggiorna/Inserisci" ID="Button4" runat="server" OnClick="btnAggiornaCaratteristica6_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>

        </div>

    </div>
</asp:Content>

