<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false" ValidateRequest="false" CodeFile="GestioneNewsletter.aspx.cs" Inherits="AreaContenuti_GestioneNewsletter" %>


<%@ MasterType VirtualPath="~/AreaContenuti/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <title></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="background-color: White; padding: 20px">

        <%--  <asp:UpdateProgress ID="UpdateProgress8" runat="server" DisplayAfter="0" DynamicLayout="false"
                        AssociatedUpdatePanelID="updNewsletter">
                        <ProgressTemplate>
                            <div style="float: left; background-color: Transparent; color: Black; padding: 2px">
                                <img runat="server" alt="" src="~/Images/indicator.gif" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>--%>
        <h3>Archivio newsletter</h3>
        <hr />
        <asp:Literal ID="output" runat="server"></asp:Literal>

        <asp:Literal Text="Seleziona una newsletter: " runat="server" /><br />
        <asp:ListBox runat="server" Width="600" Height="150" ID="listNewsLetter" AutoPostBack="true"
            OnSelectedIndexChanged="listitemchange"></asp:ListBox>
        <br />
        <asp:Literal ID="Literal1" Text="Id Newsletter selezionata : " runat="server" />
        <asp:Literal Text="" ID="litId" runat="server" />
        <br />
        <asp:Literal ID="Literal3" Text="Link pagina sottoscrizione : " runat="server" />
        <br />
        <asp:Literal Text="" ID="litLink" runat="server" />
        <br />
        <br />

        <div style="background-color: #e6e6e6; padding: 10px; position: relative">
            <h3>Composizione newsletter</h3>
            <hr />
            <div style="float: left; width: 300px">
                <span>Specificare Lingua della newsletter: </span>
            </div>
            <div style="float: left; width: 300px">
                <asp:DropDownList runat="server" ID="ddlLingua" Width="100px" AutoPostBack="true"
                    OnSelectedIndexChanged="linguachange">
                    <asp:ListItem Text="Italiano" Value="I" Selected="True" />
                    <asp:ListItem Text="Inglese" Value="GB" />
                    <asp:ListItem Text="Russo" Value="RU" />
                    <asp:ListItem Text="Francese" Value="FR" />
                    <asp:ListItem Text="Tedesco" Value="DE" />
                    <asp:ListItem Text="Spagnolo" Value="ES" />
                </asp:DropDownList>
            </div>
            <div style="clear: both">
                <br />
            </div>
            <div style="float: left; width: 300px">
                <span>Oggetto messaggio newsletter: </span>
            </div>
            <div style="float: left; width: 600px">
                <asp:TextBox runat="server" ID="txtSoggetto" Width="600px" />
            </div>
            <div style="clear: both">
                <br />
            </div>
            <div style="float: left; width: 300px">
                <span>Testo per il click adesione posto in fondo mail, inserire il testo al momento della salvataggio/modifica delle mail<br />
                    (se vuoto non inserisce il link per richiesta adesione): </span>
            </div>
            <div style="float: left; width: 600px">
                <asp:TextBox runat="server" ID="txtInvito" Text=""
                    Width="600px" />
                <%--      <Ajax:TextBoxWatermarkExtender   id="invitoadesione" TargetControlID="txtInvito" runat="server" WatermarkText="Inserisci il testo per l'invito all'adesione ( se vuoto non è inserito il link di adesione )"></Ajax:TextBoxWatermarkExtender>
                --%>
            </div>
            <div style="clear: both">
                <br />
            </div>
            <div style="float: left; width: 300px">
                <span>Testo indicazioni nell'intestazione del form adesione ( solo per richieste adesione ): </span>
            </div>
            <div style="float: left; width: 600px">
                <asp:TextBox runat="server" ID="txtAdesione" TextMode="MultiLine" Height="50" Text="Compila i dati richiesti per l'adesione all'offerta."
                    Width="600px" />
            </div>
            <div style="clear: both">
                <br />
            </div>
            <br />
            <br />
            <%--<asp:UpdatePanel runat="server" ID="updNewsletter">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btncaricafile" />
                    </Triggers>
                    <ContentTemplate>--%>
            <script type="text/javascript">
                $(function () {
                    tinymceinit();
                });
                function tinymceinit() {
                    tinymce.init({
                        selector: "textarea.tiny",
                        extended_valid_elements: 'button[class|onclick|style|type|id|name],input[class|onclick|style|type|value|id|name|placeholder]',
                        theme: "modern",
                        convert_urls: false,
                        relative_urls: false,
                        allow_html_in_named_anchor: true,
                        allow_conditional_comments: true,
                        schema: 'html5',
                        valid_children: "+a[div|i|span|h1|h2|h3|h4|h5|h6|p|#text],+body[style]",
                        plugins: [
                            "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                            "save table directionality emoticons template paste textcolor fullpage"
                        ],
                        menubar: false,
                        toolbar_items_size: 'small',
                        toolbar: "insertfile undo redo cut copy paste pastetext | table | bold italic underline superscript superscript | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link unlink anchor | code preview fullpage | charmap styleselect fontselect fontsizeselect forecolor backcolor | mybutton",
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
            <div style="display: none; width: 100%; height: 450px" runat="server" visible="false">
                <asp:TextBox runat="server" ID="txtContent" TextMode="MultiLine" Width="940" Height="400" />
                <%-- <Ajax:HtmlEditorExtender ID="htmlExtender" DisplaySourceTab="true" OnImageUploadComplete="ajaxFileUpload_OnUploadComplete"
                            runat="server" TargetControlID="txtContent" EnableSanitization="false">
                            <Toolbar>
                                <ajaxToolkit:Undo />
                                <ajaxToolkit:Redo />
                                <ajaxToolkit:Bold />
                                <ajaxToolkit:Italic />
                                <ajaxToolkit:Underline />
                                <ajaxToolkit:StrikeThrough />
                                <ajaxToolkit:Subscript />
                                <ajaxToolkit:Superscript />
                                <ajaxToolkit:JustifyLeft />
                                <ajaxToolkit:JustifyCenter />
                                <ajaxToolkit:JustifyRight />
                                <ajaxToolkit:JustifyFull />
                                <ajaxToolkit:InsertOrderedList />
                                <ajaxToolkit:InsertUnorderedList />
                                <ajaxToolkit:CreateLink />
                                <ajaxToolkit:UnLink />
                                <ajaxToolkit:RemoveFormat />
                                <ajaxToolkit:SelectAll />
                                <ajaxToolkit:UnSelect />
                                <ajaxToolkit:Delete />
                                <ajaxToolkit:Cut />
                                <ajaxToolkit:Copy />
                                <ajaxToolkit:Paste />
                                <ajaxToolkit:BackgroundColorSelector />
                                <ajaxToolkit:ForeColorSelector />
                                <ajaxToolkit:FontNameSelector />
                                <ajaxToolkit:FontSizeSelector />
                                <ajaxToolkit:Indent />
                                <ajaxToolkit:Outdent />
                                <ajaxToolkit:InsertHorizontalRule />
                                <ajaxToolkit:HorizontalSeparator />
                                <ajaxToolkit:InsertImage />
                            </Toolbar>
                        </Ajax:HtmlEditorExtender>--%>
            </div>
            <asp:Label Text="" runat="server" ID="outupload" ForeColor="Red" Font-Size="Medium" />
            <HTMLEditor:Editor runat="server" ID="htmlEdit" Height="500px" Width="100%" Visible="false"></HTMLEditor:Editor>
            <div>
                <textarea class="tiny" id="tinyhtmlEdit" runat="server" style="width: 100%; height: 400px"></textarea>
            </div>
            Dimensione orizzontale :
                <asp:TextBox runat="server" ID="resizeDimx" Text="800" Width="80px" />
            Dimensione verticale :
                <asp:TextBox runat="server" ID="resizeDimy" Text="800" Width="80px" />
            <asp:FileUpload runat="server" ID="upFile" />
            <asp:Button ID="btncaricafile" UseSubmitBehavior="false" runat="server" Text="Inserisci Foto" OnClick="btnCaricafile_Click" />
            <br />
            <div style="clear: both">
                <br />
            </div>
            <%--         </ContentTemplate>
                </asp:UpdatePanel>--%>
            <asp:Button ID="Button4" runat="server" Text="Nuova Newsletter" OnClick="Nuova" />
            <asp:Button ID="Button3" runat="server" Text="Memorizza/Modifica newsletter in archivio"
                OnClick="Save" />
            <asp:Button ID="Button5" runat="server" Text="Cancella Newsletter da archivio" OnClick="Cancella" />
            <br />
            <br />
            <%--  <asp:UpdateProgress ID="UpdateProgress4" runat="server" DisplayAfter="0" DynamicLayout="false"
                            AssociatedUpdatePanelID="updNewsletter">
                            <ProgressTemplate>
                                <div style="float: left; background-color: Transparent; color: Black; padding: 2px">
                                    <img runat="server" alt="" src="~/Images/indicator.gif" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
        </div>
        <br />
        <br />

        <%--   <asp:UpdatePanel runat="server" ID="updGestioneClienti">
                <ContentTemplate>--%>
        <div style="background-color: #f0f0f0; padding: 10px">
            <h3>Selezione clienti per mailing e invio newsletter</h3>
            <hr />
            <h4>Filtri selezione anagrafica clienti per creazione gruppi</h4>
            <div style="float: right; text-align: right">
                <asp:Button Text="Aggiorna Filtro clienti" runat="server" ID="btnFiltroClienti" OnClick="btnFiltroClienti_Click" />
                <br />
                <asp:Literal ID="Literal2" Text="Seleziona Tipologia Clienti: " runat="server" />
                <asp:DropDownList runat="server" Width="200" ID="ddlTipiClienti" AutoPostBack="true"
                    AppendDataBoundItems="true" OnSelectedIndexChanged="TipoClienteChange">
                </asp:DropDownList>
                <br />
                Filtro clienti età min:
            <asp:TextBox runat="server" ID="txtetamin" Width="50" />

                Filtro clienti età max:
            <asp:TextBox runat="server" ID="txtetamax" Width="50" />

                <br />
                <br />
                Filtro Sesso clienti:
            <asp:RadioButtonList runat="server" ID="radSessoRicerca" RepeatLayout="Flow">
                <asp:ListItem Text="Qualsiasi" Value="" Selected="True" />
                <asp:ListItem Text="Uomo" Value="uomo" />
                <asp:ListItem Text="Donna" Value="donna" />
            </asp:RadioButtonList>
                <br />
                <br />
                <asp:Literal ID="Literal8" Text="Seleziona Lingua Clienti anagrafica: " runat="server" />
                <asp:DropDownList runat="server" ID="ddlLinguaFiltroClienti" Width="200" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlLinguaFiltroClientiChange">
                    <asp:ListItem Text="Italiano" Value="I" Selected="True" />
                    <asp:ListItem Text="Inglese" Value="GB" />
                    <asp:ListItem Text="Russo" Value="RU" />
                    <asp:ListItem Text="Francese" Value="FR" />
                    <asp:ListItem Text="Tedesco" Value="DE" />
                    <asp:ListItem Text="Spagnolo" Value="ES" />
                </asp:DropDownList>
                <br />
                <asp:Literal ID="Literal10" Text="Seleziona Nazione Clienti anagrafica: " runat="server" />
                <asp:DropDownList Width="310px" runat="server" ID="ddlNazioniFiltro" AppendDataBoundItems="true"
                    AutoPostBack="true" SkinID="Insert_ddl" OnSelectedIndexChanged="ddlFiltraPerNazioneChange" />
            </div>
            <div style="float: left; text-align: right">
                <asp:Literal ID="Literal5" Text="Crea nuovo gruppo clienti: " runat="server" /><asp:TextBox
                    ID="txtNomeGruppo" runat="server" /><br />
                <asp:Button ID="btnNuovoGruppo" Width="300" runat="server" Text="Crea nuovo gruppo mailing"
                    OnClick="CreaNuovoGruppo" />
                <br />
                <asp:Button ID="btnModificaGruppo" Width="300" runat="server" Text="Modifica descrizione gruppo mailing"
                    OnClick="ModificaGruppo" />
                <br />
            </div>
            <div style="clear: both">
            </div>
            <div style="float: left">
                <asp:Literal ID="Literal4" Text="Seleziona gruppo clienti per invio: " runat="server" /><br />
                <asp:ListBox runat="server" Width="200" Height="150" ID="listGruppi" AutoPostBack="true"
                    OnSelectedIndexChanged="listgruppiitemchange"></asp:ListBox>
            </div>
            <div style="float: left">
                <asp:Literal ID="Literal6" Text="Clienti presenti nel gruppo: " runat="server" /><br />
                <asp:ListBox runat="server" Width="280" Height="150" ID="listClientiNelgruppo" AutoPostBack="true"
                    OnSelectedIndexChanged="listClientiNelgruppoitemchange"></asp:ListBox>
            </div>
            <div style="float: left">
                <br />
                <asp:Button ID="Button7" runat="server" Font-Size="10px" Width="100" Text="<-Aggiungi Tutti"
                    OnClick="AggiungiTuttiAGruppo" /><br />
                <br />
                <asp:Button ID="btnAggiungiClienteAGruppo" Font-Size="10px" runat="server" Width="100"
                    Text="<-Aggiungi" OnClick="AggiungiAGruppo" /><br />
                <br />
                <asp:Button ID="btnTogliClienteDAGruppo" Font-Size="10px" runat="server" Width="100"
                    Text="Togli->" OnClick="EliminaDaGruppo" />
                <br />
                <br />
                <asp:Button ID="Button8" runat="server" Font-Size="10px" Width="100" Text="Togli Tutti->"
                    OnClick="EliminaTuttiDaGruppo" />
            </div>
            <div style="float: left">
                <asp:Literal ID="Literal7" Text="Clienti In archivio: " runat="server" /><br />
                <asp:CheckBox ID="chkCaricamentolista" ToolTip="Se selezionata questa voce viene caricata nel box sottostante tutta la lista clienti per la gestione dei gruppi, altrimenti viene solo selezionata la lista per la preparazione delle mail senza visualizzarla in pagina" Text="Visualizza lista" runat="server" Enabled="true" Checked="false" />
                <br />
                <asp:ListBox runat="server" Width="325" Height="150" ID="listAnagraficaClienti" AutoPostBack="true"
                    OnSelectedIndexChanged="listAnagraficaClientiitemchange"></asp:ListBox>
            </div>
            <div style="clear: both">
            </div>
            <asp:Literal Text="" ID="litClientiIngruppo" runat="server" /><br />
        </div>
        <br />
        <hr />
        <div style="background-color: #f9f9f9; padding: 10px">
            <h3>SPEDIZIONE DELLE EMAIL</h3>
            <br />
            <asp:CheckBox Text="Forza invio anche a clienti già contattati" Checked="false" ID="chkForzaInvio" runat="server" />
            <br />
            <br />
            <div style="float: left; width: 400px">
                <span>Variazione Denominazione del mittente della mail(opzionale): </span>
            </div>
            <div style="float: left; width: 300px">
                <asp:TextBox runat="server" ID="txtNomeMittente" Width="300px" />
            </div>
            <div style="clear: both">
                <br />
                <div style="float: left; width: 400px">
                    <span>Variazione Email mittente(opzionale): </span>
                </div>
                <div style="float: left; width: 300px">
                    <asp:TextBox runat="server" ID="txtEmailMittente" Width="300px" />
                </div>
                <div style="clear: both">
                    <br />
                </div>
                <br />

                <asp:Button ID="btnPreparaMailPerGruppo" Width="700" runat="server" Text="Prepara mail per i clienti del GRUPPO selezionato ( la lingua è quella selezionata per la newsletter )"
                    OnClick="IncrociaGruppo" />
                <br />
                <br />
                <asp:Button ID="btnPreparaMailPerTipologia" Width="700" runat="server" Text="Prepara mail per i clienti della TIPOLOGIA selezionata ( la lingua è quella selezionata per la newsletter )"
                    OnClick="Incrocia" />
                <br />
                <h4>
                    <asp:Literal Text="Totale mail preparate: " ID="litMails" runat="server" /></h4>
                <br />
                <asp:Literal Text="" ID="Message" runat="server" />

                <br />
                <br />
                <asp:Literal Text="Invia adesso tutte le email presenti !" ID="Literal9" runat="server" />
                <asp:Button ID="Button2" runat="server" Text="Esegui Invio Mailing" OnClick="Esegui" Width="200"
                    Enabled="false" /><br />
                <asp:Button ID="Button9" runat="server" Text="Cancella mail in attesa" OnClick="CancellaMail" Width="200"
                    Enabled="true" /><br />
                <asp:Button ID="Button10" runat="server" Text="Svuota tabella mail" OnClick="SvuotaMail" Width="200"
                    Enabled="true" /><br />
                <asp:Button ID="btnComprimi" Visible="false" runat="server" Text="Comprimi db tabella mail" OnClick="ComprimiDatabase" Width="200"
                    Enabled="true" /><br />
                <asp:Literal ID="outputmailing" runat="server"></asp:Literal>
            </div>
            <%-- <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" DynamicLayout="false"
                            AssociatedUpdatePanelID="updGestioneClienti">
                            <ProgressTemplate>
                                <div style="float: left; background-color: Transparent; color: Black; padding: 2px">
                                    <img id="Img1" runat="server" alt="" src="~/Images/indicator.gif" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
            <%--   </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </div>

</asp:Content>
