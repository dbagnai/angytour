<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="GestioneOfferte.aspx.cs" Inherits="AreaContenuti_Default3" MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <Ajax:ToolkitScriptManager ID="ScriptManagerMaster" runat="server" AllowCustomErrorsRedirect="True"
        AsyncPostBackErrorMessage="Errore generico. Contattare HelpDesk" AsyncPostBackTimeout="400"
        EnablePartialRendering="true" EnablePageMethods="true" EnableScriptLocalization="true"
        EnableScriptGlobalization="true">
    </Ajax:ToolkitScriptManager>--%>

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
                valid_children: "+a[div|i|span|h1|h2|h3|h4|h5|h6|p|#text],+body[style],+body[script]",
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

    <%--  <link href="../App_Themes/TemaPortale1/StylePortale1.css" rel="stylesheet" type="text/css" />--%>

    <div class="row" style="background-color: White; padding: 10px 10px 10px 10px">
        <div class="col-sm-5">
            <asp:HiddenField ID="cancelHidden" runat="server" Value="false" />
            <span style="font-size: 1.4rem; color: crimson">
                <asp:Literal ID="output" runat="server"></asp:Literal></span>
            <div style="margin-bottom: 30px">
                <h2>
                    <asp:Literal ID="litTitolo" runat="server"></asp:Literal>
                    - Selezione scheda struttura</h2>
                <hr />
                <div class="cerca">
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtinputCerca" Width="100%" />
                            <Ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtinputCerca"
                                runat="server" WatermarkText='Cerca'>
                            </Ajax:TextBoxWatermarkExtender>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 30px; margin-top: 10px;">
                        <div class="col-sm-8">
                            <asp:DropDownList Width="100%" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlSottoProdSearch"
                                runat="server" OnSelectedIndexChanged="ddlSottoProdSearch_SelectedIndexChange" />
                        </div>

                    </div>
                    <div class="row" style="margin-bottom: 30px; margin-top: 10px;">
                        <div class="col-sm-4">
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
                            <th>Vedi</th>
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
                                            <a onclick="JsSvuotaSession(this)" target="_blank" href="<%# CreaLinkRoutes(null,false,"I", ((WelcomeLibrary.DOM.Offerte)Container.DataItem).UrltextforlinkbyLingua("I"),Eval("Id").ToString(),Eval("CodiceTipologia").ToString())    %>">
                                                <asp:Literal ID="Literal4" runat="server" Text='<%# Eval("Id").ToString() %>'></asp:Literal></a>
                                        </div>
                                    </td>
                                    <td style="border: Solid 1px #ccc; width: 60px; overflow-x: auto">
                                        <div style="height: 50px; overflow-y: auto;">
                                            <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("CodiceProdotto").ToString() %>'></asp:Literal>
                                        </div>
                                    </td>
                                    <%-- <td style="width: 0px; height: 100px; border: Solid 1px Black;">
                            <asp:Image  ID="imgAnt" Width="95px" ImageUrl='<%# filemanage.ComponiUrlAnteprima(Eval("FotoCollection_M"),Eval("Id").ToString()) %>'
                                runat="server" />
                        </td>--%>
                                    <td style="border: Solid 1px #ccc;">
                                        <div style="height: 50px; overflow-y: auto">
                                            <asp:Literal ID="lit1" runat="server" Text='<%# Eval("DenominazioneI").ToString() %>'></asp:Literal>
                                        </div>
                                    </td>
                                    <td style="border: Solid 1px #ccc;">
                                        <div style="height: 50px; overflow-y: auto">
                                            <a target="_blank" href="<%# CreaLinkRoutes(null,false,"I",((WelcomeLibrary.DOM.Offerte)Container.DataItem).UrltextforlinkbyLingua("I"),Eval("Id").ToString(),Eval("CodiceTipologia").ToString())    %>">view
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
                            <%--<div class="dz-default dz-message"></div>--%>
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
                                Descrizione img alt (it)
                                <br />
                                <asp:TextBox CssClass="mceNoEditor form-control" runat="server" ID="txtDescrizioneFotoI" />
                                Descrizione img alt(en)
                                <br />
                                <asp:TextBox CssClass="mceNoEditor form-control" runat="server" ID="txtDescrizioneFotoGB" />
                                <span style="display: none">Descrizione img alt(ru)
                                <br />
                                    <asp:TextBox CssClass="mceNoEditor form-control" runat="server" ID="txtDescrizioneFotoRU" /></span>
                                <span style="display: block">Descrizione img alt(FR)
                                <br />
                                    <asp:TextBox CssClass="mceNoEditor form-control" runat="server" ID="txtDescrizioneFotoFR" /></span>
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
                                                    <asp:ImageButton Width="80px" Height="80px" ToolTip='<%# Eval("DescrizioneI").ToString() %>' ID="imgAntFoto" CommandArgument='<%# 
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
                            <asp:Label ID="Label18" runat="server" Text="Spengi Gallery" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:CheckBox ID="chkVetrina" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label39" runat="server" Text="Promozioni" /></strong>
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
                            OnSelectedIndexChanged="ddlProdotto_SelectedIndexChanged" runat="server" />
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
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label40" runat="server" Text="Autore" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox CssClass="mceNoEditor form-control" ReadOnly="true" ID="txtAutore" runat="server" value="" />
                    </div>
                </div>

                <div class="row" style="padding-top: 10px; padding-bottom: 10px">
                    <div class="col-sm-2 item-text">
                        <strong>
                            <asp:Label ID="Label41" runat="server" Text="META robots (opzionale)" /></strong>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtRobots" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>


                <div class="row">
                    <hr />
                    <ul class="nav nav-pills" style="margin-left: 0;">
                        <li class="active"><a data-toggle="pill" href="#promoita">Italiano</a></li>
                        <li><a data-toggle="pill" href="#promoeng">Inglese</a></li>
                        <li><a data-toggle="pill" href="#promoru">Russo</a></li>
                        <li><a data-toggle="pill" href="#promofr">Francese</a></li>
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
                                        <asp:Label ID="Label42" runat="server" Text="Link Canonical (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCanonicalI" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 20px;">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label50" runat="server" Text="link page" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:Literal ID="litlinkI" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label43" runat="server" Text="Url text (alternativo)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtUrlI" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="litDenominazioneI" runat="server" Text="Denominazione/h1 (Ita)" /></strong>
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
                                    <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtDescrizioneI"
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
                                        <asp:Label ID="Labelpre" runat="server" Text="Meta Title (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo1GB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Labelprse" runat="server" Text="Meta Description (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo2GB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label44" runat="server" Text="Link Canonical (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCanonicalGB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 20px;">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label48" runat="server" Text="link page" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:Literal ID="litlinkGB" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label45" runat="server" Text="Url text (alternativo)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtUrlGB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Literal2" runat="server" Text="Denominazione/h1 (Eng)" /></strong>
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
                                    <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtDescrizioneGB"
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
                                        <asp:Label ID="Label33" runat="server" Text="Meta Title(opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo1RU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label34" runat="server" Text="Meta Description (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo2RU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label46" runat="server" Text="Link Canonical (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCanonicalRU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 20px;">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label49" runat="server" Text="link page" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:Literal ID="litlinkRU" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label47" runat="server" Text="Url text (alternativo)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtUrlRU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label35" runat="server" Text="Denominazione/h1 (Ru)" /></strong>
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
                                    <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtDescrizioneRU"
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
                        <div id="promofr" class="tab-pane fade">
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label51" runat="server" Text="Meta Title(opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo1FR" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label52" runat="server" Text="Meta Description (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCampo2FR" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label53" runat="server" Text="Link Canonical (opzionale)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtCanonicalFR" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 20px;">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label54" runat="server" Text="link page" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:Literal ID="litlinkFR" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label55" runat="server" Text="Url text (alternativo)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" ID="txtUrlFR" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label56" runat="server" Text="Denominazione/h1 (FR)" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" TextMode="MultiLine" Height="50" ID="txtDenominazioneFR" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label57" runat="server" Text="Descrizione FR" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtDescrizioneFR"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 item-text">
                                    <strong>
                                        <asp:Label ID="Label58" runat="server" Text="Dettaglio FR" /></strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="mceNoEditor form-control" Height="150px" TextMode="MultiLine" ID="txtDatitecniciFR"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                    </div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Insertvalidate" />
                    <hr />
                </div>

                <asp:Panel runat="server" ID="pnlIndirizzo0">
                    <div class="row">
                        <div class="col-sm-2 item-text">
                            <strong>
                                <asp:Label ID="Label61" runat="server" Text="Nazione" /></strong>
                        </div>
                        <div class="col-sm-10">
                            <asp:DropDownList CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" ID="ddlNazione"
                                OnSelectedIndexChanged="ddlNazione_SelectedIndexChanged" runat="server" />
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
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlIndirizzo1" Visible="false">
                    <div class="row" style="border: 1px solid White; margin: 1%; padding: 20px">
                        <%-- <h3>Indirizzo 1</h3>
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Nome della posizione</strong>
                                <i>per es. un luogo di lavoro, una sede, un punto di ritrovo</i><br />
                            </div>
                            <div class="col-sm-8">
                                <asp:TextBox Width="60%" runat="server" ID="txtNomeposizione1_dts" /><br />
                            </div>
                        </div>--%>

                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Paese*</strong>
                            </div>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceNAZIONE1_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceNAZIONE1_dts_OnSelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ErrorMessage="Nazione Indirizzo 1 Obbligatorio"
                                    ControlToValidate="ddlCodiceNAZIONE1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Regione*</strong>
                            </div>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceREGIONE1_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceREGIONE1_dts_OnSelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ErrorMessage="Regione Indirizzo 1 Obbligatoria"
                                    ControlToValidate="txtCodiceREGIONE1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                <br />
                                <input id="txtCodiceREGIONE1_dts" style="width: 60%" runat="server" value="" />

                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Provincia*</strong>
                            </div>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" Width="60%" ID="ddlCodicePROVINCIA1_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodicePROVINCIA1_dts_OnSelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ErrorMessage="Provincia Indirizzo 1 Obbligatoria"
                                    ControlToValidate="txtCodicePROVINCIA1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                <br />
                                <input id="txtCodicePROVINCIA1_dts" style="width: 60%" runat="server" value="" />

                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Comune*</strong>
                            </div>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" Width="60%" ID="ddlCodiceCOMUNE1_dts" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodiceCOMUNE1_dts_OnSelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" ErrorMessage="Comune Indirizzo 1 Obbligatoria"
                                    ControlToValidate="txtCodiceCOMUNE1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                                <br />
                                <input id="txtCodiceCOMUNE1_dts" style="width: 60%" runat="server" value="" />

                            </div>
                        </div>

                        <%--  <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Via*</strong>
                            </div>
                            <div class="col-sm-8">
                                <input id="txtVia1_dts" style="width: 60%" runat="server" value="" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" ErrorMessage="Via Indirizzo 1 Obbligatoria"
                                    ControlToValidate="txtVia1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Cap*</strong>
                            </div>
                            <div class="col-sm-8">
                                <input id="txtCap1_dts" style="width: 60%" runat="server" value="" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" ErrorMessage="Cap Indirizzo 1 Obbligatorio"
                                    ControlToValidate="txtCap1_dts" runat="server" Text="*" ValidationGroup="Insertvalidate" />
                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Telefono</strong>
                            </div>
                            <div class="col-sm-8">
                                <input id="txtTelefono1_dts" style="width: 60%" runat="server" value="" />
                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 5px">
                            <div class="col-sm-4">
                                <strong>Latitudine</strong><br />
                                <asp:TextBox CssClass="mceNoEditor"  ID="TextBox1" runat="server" value="" />
                                <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtLatitudine1_dts"
                                    FilterType="Custom, Numbers" ValidChars="0123456789," />
                            </div>
                            <div class="col-sm-8">
                                <strong>Longitudine</strong><br />
                                <asp:TextBox CssClass="mceNoEditor"  ID="TextBox2" runat="server" value="" />
                                <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtLongitudine1_dts"
                                    FilterType="Custom, Numbers" ValidChars="0123456789," />
                            </div>
                        </div>--%>
                    </div>
                </asp:Panel>






                <asp:Panel runat="server" Visible="false">
                    <hr />
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
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label59" Text="Nome 1 Livello FR"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="mceNoEditor" ID="NomeNuovoProdFr" runat="server"></asp:TextBox>
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
                                            <br />
                                            <asp:Literal Text="" ID="linksezioneFR" runat="server" />
                                            <div style="height: 50px;">
                                            </div>
                                        </td>
                                        <td>
                                            <%-- Qui aggiungiamo le dll di selezione per la modifica del SottoProdotto--%>
                                            <div style="margin-left: 50px; border-left: 1px solid #000000; padding-left: 10px;">
                                                <div>
                                                    <h2><%= references.ResMan("Common",Lingua,"TitleSottProdottiGest") %></h2>
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
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="Label60" Text="Nome 2 Livello FR"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="mceNoEditor" ID="NomeNuovoSottFr" runat="server"></asp:TextBox>
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
                                                <br />
                                                <asp:Literal Text="" ID="linksottosezioneFR" runat="server" />
                                            </div>
                                            <div style="height: 50px;">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>

                <hr />

                <asp:Panel runat="server" ID="pnlGestioneCaratteristiche" Visible="false">
                    <h2>
                        <asp:Label runat="server" ID="Label25" Text="Gestione tabelle riferimento"></asp:Label>
                    </h2>
                    <hr />

                    <div style="background-color: #fafafa">
                        <h3>Tabella Caratteristica 1</h3>
                        <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica1_gest" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica1update">
                        </asp:DropDownList>
                        <br />

                        Descrizione italiano:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar1I" Text="" Width="300" /><br />
                        Descrizione inglese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar1GB" Text="" Width="300" /><br />
                        Descrizione Russo:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar1RU" Text="" Width="300" /><br />
                        Descrizione Francese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar1FR" Text="" Width="300" /><br />

                        <Ajax:TextBoxWatermarkExtender runat="server" ID="w1" WatermarkText="Inserire tipo (Italiano)" TargetControlID="txtCar1I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="w2" WatermarkText="Inserire tipo (Inglese)" TargetControlID="txtCar1GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="w3" WatermarkText="Inserire tipo (Russo)" TargetControlID="txtCar1RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="w4" WatermarkText="Inserire tipo (Francese)" TargetControlID="txtCar1FR">
                        </Ajax:TextBoxWatermarkExtender>
                        <asp:Button Text="Aggiorna/Inserisci" ID="btnAggiornaCaratteristica1" runat="server" OnClick="btnAggiornaCaratteristica1_Click" />
                        <br />
                    </div>
                    <div style="background-color: #fafafa">
                        <h3>Tabella Caratteristica 2</h3>
                        <asp:DropDownList runat="server" Width="310px" ID="ddlCaratteristica2_gest" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="caratteristica2update">
                        </asp:DropDownList><br />
                        Descrizione italiano:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar2I" Text="" Width="300" /><br />
                        Descrizione inglese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar2GB" Text="" Width="300" /><br />
                        Descrizione russo:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar2RU" Text="" Width="300" /><br />
                        Descrizione Francese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar2FR" Text="" Width="300" /><br />

                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender4" WatermarkText="Inserire indice (Italiano)" TargetControlID="txtCar2I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender5" WatermarkText="Inserire indice (Inglese)" TargetControlID="txtCar2GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender14" WatermarkText="Inserire indice (Russo)" TargetControlID="txtCar2RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender19" WatermarkText="Inserire indice (Francese)" TargetControlID="txtCar2FR">
                        </Ajax:TextBoxWatermarkExtender>
                        <asp:Button Text="Aggiorna/Inserisci" ID="btnAggiornaCaratteristica2" runat="server" OnClick="btnAggiornaCaratteristica2_Click" />
                        <br />
                    </div>

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
                        Descrizione Francese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar3FR" Text="" Width="300" /><br />

                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender6" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar3I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender7" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar3GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender15" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar3RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender20" WatermarkText="Inserire valore (Francese)" TargetControlID="txtCar3FR">
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
                        Descrizione Francese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar4FR" Text="" Width="300" /><br />
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender8" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar4I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender9" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar4GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender16" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar4RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender21" WatermarkText="Inserire valore (Francese)" TargetControlID="txtCar4FR">
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
                        Descrizione Francese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar5FR" Text="" Width="300" /><br />
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender10" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar5I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender11" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar5GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender17" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar5RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender22" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar5FR">
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
                        Descrizione Francese:
                       
                            <asp:TextBox CssClass="mceNoEditor" runat="server" ID="txtCar6FR" Text="" Width="300" /><br />
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender12" WatermarkText="Inserire valore (Italiano)" TargetControlID="txtCar6I">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender13" WatermarkText="Inserire valore (Inglese)" TargetControlID="txtCar6GB">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender18" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar6RU">
                        </Ajax:TextBoxWatermarkExtender>
                        <Ajax:TextBoxWatermarkExtender runat="server" ID="TextBoxWatermarkExtender23" WatermarkText="Inserire valore (Russo)" TargetControlID="txtCar6FR">
                        </Ajax:TextBoxWatermarkExtender>
                        <asp:Button Text="Aggiorna/Inserisci" ID="Button4" runat="server" OnClick="btnAggiornaCaratteristica6_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>

        </div>

    </div>
</asp:Content>

