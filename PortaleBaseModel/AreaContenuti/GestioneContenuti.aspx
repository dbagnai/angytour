<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="GestioneContenuti.aspx.cs" Inherits="AreaContenuti_GestioneContenutiNew" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet"  href="/css/prettyPhoto.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmCancella() {
            //document.getElementByID("CheckBoxListExCtrl").value
            var conferma = confirm('Sei sicuro di voler cancellare questo contenuto?');
            if (conferma) {
                $get("<%=cancelHidden.ClientID%>").value = "true";
            }
            else {
                $get("<%=cancelHidden.ClientID%>").value = "false";
            }
        }
    </script>
    <div style="background-color: White; padding: 10px 10px 10px 10px">
        <asp:HiddenField ID="cancelHidden" runat="server" Value="false" />
        <asp:Literal ID="output" runat="server"></asp:Literal>
        <div class="row">
            <div class="col-sm-3">
                <h2>
                    <asp:Literal ID="litTitolo" runat="server"></asp:Literal>
                    - Selezione scheda contenuto</h2>
                <hr />
                <div style="width: 100%; overflow: auto">
                    <table class="table table-condensed" style="width: 100%">
                        <thead>
                            <tr>
                                <th></th>
                                <th>ID</th>
                                <th>Link</th>
                                <th>Titolo ITA</th>
                                <%--  <th>Titolo ENG</th>
                                <th>Titolo RU</th>--%>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptContenuti" OnItemDataBound="rptContenuti_ItemDataBound">
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
                                                <asp:Literal ID="Literal4" runat="server" Text='<%# Eval("Id").ToString() %>'></asp:Literal>
                                            </div>
                                        </td>
                                        <td><%# CreaLinkPaginastatica( (long)Eval("Id") ,true) %></td>
                                        <td style="border: Solid 1px #ccc;">
                                            <div style="height: 50px; overflow-y: auto">
                                                <asp:Literal ID="lit1" runat="server" Text='<%# Eval("TitoloI").ToString() %>'></asp:Literal>
                                            </div>
                                        </td>
                                        <%-- <td style="border: Solid 1px #ccc;">
                                            <div style="height: 50px; overflow-y: auto">
                                                <asp:Literal ID="lit2" runat="server" Text='<%# Eval("TitoloGB").ToString() %>'></asp:Literal>
                                            </div>
                                        </td>
                                        <td style="border: Solid 1px #ccc;">
                                            <div style="height: 50px; overflow-y: auto">
                                                <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("TitoloRU").ToString() %>'></asp:Literal>
                                            </div>
                                        </td>--%>
                                        <%--  <td style="border: Solid 1px #ccc;">
                                <div style="height: 50px; overflow-y: auto">
                                    <asp:Literal ID="lit3" runat="server" Text='<%# Eval("DescrizioneI").ToString() %>'></asp:Literal>
                                </div>
                            </td>
                            <td style="border: Solid 1px #ccc;">
                                <div style="height: 50px; overflow-y: auto">
                                    <asp:Literal ID="lit4" runat="server" Text='<%# Eval("DescrizioneGB").ToString() %>'></asp:Literal>
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
            </div>
            <div class="col-sm-9">
                <div class="row">
                    <div class="col-sm-12">
                        <h2>Dettaglio scheda</h2>
                        <hr />
                        <asp:Button ID="btnNuovo" CssClass="btn btn-primary btn-sm" runat="server" Text="Nuovo" OnClick="btnNuovo_Click" />
                        <asp:Button ID="btnAggiorna" CssClass="btn btn-primary btn-sm" runat="server" Text="Modifica" OnClick="btnAggiorna_Click" />
                        <asp:Button ID="btnCancella" CssClass="btn btn-danger btn-sm" runat="server" Text="Cancella" OnClick="btnCancella_Click"
                            OnClientClick="javascript:ConfirmCancella()" UseSubmitBehavior="true" />
                        <br />
                        <br />
                    </div>
                </div>
                <script type="text/javascript">
                    $(function () {
                        pretty();
                        tinymceinit();
                    });
                   function tinymceinit() {
                        tinymce.init({
                            mode: "textareas",
                            editor_deselector: "mceNoEditor", // class="mceNoEditor" will not have tinyMCE
                            extended_valid_elements: 'button[class|onclick|style|type|id|name],input[class|onclick|style|type|value|id|name|placeholder|multiple|onchange]',
                            theme: "modern",
                            convert_urls: false,
                            relative_urls: false,
                            forced_root_block: false,
                            verify_html: false,
                            allow_html_in_named_anchor: true,
                            valid_children: "+a[div|i|span|h1|h2|h3|h4|h5|h6|p|#text],+body[style],+div[ul]",
                             plugins: [
                                "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                                "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                                "save table directionality emoticons template paste textcolor"
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
                    function pretty() {
                        $("a[rel^='prettyPhoto']").prettyPhoto({
                            theme: 'pp_default',
                            social_tools: '',
                            default_width: 590,
                            default_height: 300,
                            horizontal_padding: 0,
                            callback: function () { ppclosed() }
                        });
                    }
                    function ppclosed() {
                        //Alla chiusura del pphoto  
                    }
                    function CaricaFile() {
                        $get('<%= btncaricafile.ClientID %>').click();
                        //__doPostBack('<%= btncaricafile.ClientID %>', '');
                    }
                    function SelezionaFile() {
                        // var percorsoFile = $get('<%= upFile.ClientID %>').value;
                        $get('<%= upFile.ClientID %>').click();
                    }
                    function InserisciImmagine() {
                        $('#linkpp1').trigger('click'); //Apro il popup con prettyphoto 
                        /*Avviamento modulo*/
                        //$get('<%= upFile.ClientID %>').click();
                        //var percorsoFile = $get('<%= upFile.ClientID %>').value;
                        //LoadFiles(percorsoFile); //Avvio il modulo
                    }

                    /*-------------------CARICAMENTO DATI CON MODULE ---------------------------*/
                    var sessionid = '<%= Session.SessionID %>';
                    var ContentPath = "<%= PercorsoComune %>";
                    var Percorsoassolutoapp = "<%= WelcomeLibrary.STATIC.Global.percorsobaseapplicazione %>";
                    function LoadFiles(percorsoFile) {
                        $.ajax({
                            url: "../Modules/HandlerDb.ashx",
                            type: "POST",
                            contentType: "application/x-www-form-urlencoded;charset=UTF8",
                            dataType: 'html',
                            data: "func=CaricaImmagine&Path=" + ContentPath + "&Percorsoassolutoapp=" + Percorsoassolutoapp + "&percorsoFile=" + percorsoFile + "&sessionid=" + sessionid,
                            headers: { 'key': 'value' },
                            success: callback
                        });
                    }
                    function callback(data, textStatus) {
                        //inserthtmlineditor(data);
                    }
                    /*-------------------FINE CARICAMENTO ASINCRONO-----------------------------*/
                </script>
                <div class="row">
                    <div class="col-sm-12">
                        <ul class="nav nav-pills" style="margin-left: 0;">
                            <li class="active"><a data-toggle="pill" href="#tabita">Italiano</a></li>
                            <li><a data-toggle="pill" href="#tabeng">Inglese</a></li>
                            <li><a data-toggle="pill" href="#tabru">Russo</a></li>
                        </ul>
                        <div class="tab-content">
                            <div id="tabita" class="tab-pane fade in active">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="litTitoloI" runat="server" Text="Titolo Ita" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox CssClass="form-control" ID="txtTitoloI" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="Label10" runat="server" Text="META title (custom)" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox CssClass="form-control" ID="txtCustomtitleI" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="Label5" runat="server" Text="META DESC (custom)" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox Width="60%" TextMode="MultiLine" CssClass="mceNoEditor" Height="20" ID="txtCustomdescI" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="outupload" Text="" runat="server" ForeColor="red" Font-Bold="true" />
                                        <div style="display: inline; width: 100%">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btncaricafile" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" ID="plhSimpleEditorI" Visible="false">
                                                        <span><strong>
                                                            <asp:Label Width="30%" ID="litDescrizioneI" runat="server" Text="Descrizione Ita" />
                                                        </strong>
                                                            <asp:TextBox Width="60%" Height="150px" TextMode="MultiLine" ID="txtDescrizioneI"
                                                                runat="server"></asp:TextBox>
                                                        </span>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="plhHtmlEditorI" Visible="false">
                                                        <strong>
                                                            <asp:Label ID="Label3" runat="server" Text="Descrizione Ita" /></strong><br />
                                                        <div>
                                                            <textarea class="tiny" id="tinyhtmlEditI" runat="server" style="width: 100%; height: 400px"></textarea>
                                                        </div>

                                                        Dimensione orizzontale(max) :
                                       
                                            <asp:TextBox runat="server" ID="resizeDimx" Text="300" Width="80px" />
                                                        Dimensione verticale(max) :
                                       
                                            <asp:TextBox runat="server" ID="resizeDimy" Text="300" Width="80px" />
                                                        <asp:FileUpload runat="server" ID="upFile" />


                                                        <asp:Button ID="btncaricafile" UseSubmitBehavior="false" runat="server" Text="Inserisci Foto" OnClick="btnCaricafile_Click" />
                                                        <br />
                                                    </asp:Panel>
                                                    <div style="background-color: #99973f; border: 1px solid black; border-radius: 4px; margin-right: 5px; padding: 2px; display: none">
                                                        <a id="linkpp1" href="#inline1" rel="prettyPhoto" style="padding: 3px; color: white; text-decoration: none;">INSERISCI FOTO</a>
                                                    </div>
                                                    <div id="inline1" style="display: none; padding: 10px" runat="server">
                                                        <div style="background-color: #f0f0f0; border: 1px solid black; border-radius: 4px; padding: 5px">
                                                            INSERIMENTO FOTO<br />
                                                        </div>
                                                        <br />
                                                        <div style="height: 100px; overflow: auto">
                                                            Seleziona la foto per il caricamento<br />

                                                            <br />
                                                            <div style="display: none">
                                                                <span style="background-color: #0094ff; border: 1px solid black; border-radius: 4px; margin-right: 5px; padding: 2px;">
                                                                    <a href="javascript:SelezionaFile()" style="padding: 3px; color: white; text-decoration: none;">Seleziona File</a>
                                                                </span>
                                                                <span style="background-color: #0094ff; border: 1px solid black; border-radius: 4px; margin-right: 5px; padding: 2px;">
                                                                    <a href="javascript:CaricaFile()" style="padding: 3px; color: white; text-decoration: none;">Carica File</a>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tabeng" class="tab-pane fade">
                                <div class="row" style="margin-top: 20px;">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="Label1" runat="server" Text="Titolo Eng" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox CssClass="form-control" ID="txtTitoloGB" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                  <div class="row">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="Label9" runat="server" Text="META title (custom)" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox CssClass="form-control" ID="txtCustomtitleGB" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="Label11" runat="server" Text="META DESC (custom)" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox Width="60%" TextMode="MultiLine" CssClass="mceNoEditor" Height="20" ID="txtCustomdescGB" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btncaricafileGB" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Panel runat="server" ID="plhSimpleEditorGB" Visible="false">
                                                    <span><strong>
                                                        <asp:Label Width="30%" ID="Label2" runat="server" Text="Descrizione Eng" />
                                                    </strong>
                                                        <asp:TextBox Width="60%" Height="150px" TextMode="MultiLine" ID="txtDescrizioneGB"
                                                            runat="server"></asp:TextBox>
                                                    </span>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="plhHtmlEditorGB" Visible="false">
                                                    <strong>
                                                        <asp:Label ID="Label4" runat="server" Text="Descrizione En" /></strong><br />
                                                    <div>
                                                        <textarea id="tinyhtmlEditGB" class="tiny" runat="server" style="width: 100%; height: 400px"></textarea>
                                                    </div>
                                                    Dimensione orizzontale :
                                       
                                        <asp:TextBox runat="server" ID="resizeDimxGB" Text="300" Width="80px" />
                                                    Dimensione verticale(max) :
                                       
                                        <asp:TextBox runat="server" ID="resizeDimyGB" Text="300" Width="80px" />
                                                    <asp:FileUpload runat="server" ID="upFileGB" />
                                                    <asp:Button ID="btncaricafileGB" UseSubmitBehavior="false" runat="server" Text="Inserisci Foto" OnClick="btnCaricafileGB_Click" />
                                                    <br />
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div id="tabru" class="tab-pane fade">
                                <div class="row" style="margin-top: 20px;">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="Label6" runat="server" Text="Titolo Russo" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox CssClass="form-control" ID="txtTitoloRU" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                  <div class="row">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="Label12" runat="server" Text="META title (custom)" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox CssClass="form-control" ID="txtCustomtitleRU" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <strong>
                                            <asp:Label ID="Label13" runat="server" Text="META DESC (custom)" /></strong>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox Width="60%" TextMode="MultiLine" CssClass="mceNoEditor" Height="20" ID="txtCustomdescRU" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btncaricafileRU" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Panel runat="server" ID="plhSimpleEditorRU" Visible="false">
                                                    <span><strong>
                                                        <asp:Label Width="30%" ID="Label7" runat="server" Text="Descrizione RU" />
                                                    </strong>
                                                        <asp:TextBox Width="60%" Height="150px" TextMode="MultiLine" ID="txtDescrizioneRU"
                                                            runat="server"></asp:TextBox>
                                                    </span>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="plhHtmlEditorRU" Visible="false">
                                                    <strong>
                                                        <asp:Label ID="Label8" runat="server" Text="Descrizione Ru" /></strong><br />
                                                    <div>
                                                        <textarea id="tinyhtmlEditRU" class="tiny" runat="server" style="width: 100%; height: 400px"></textarea>
                                                    </div>
                                                    Dimensione orizzontale :
                                       
                                        <asp:TextBox runat="server" ID="resizeDimxRU" Text="300" Width="80px" />
                                                    Dimensione verticale(max) :
                                       
                                        <asp:TextBox runat="server" ID="resizeDimyRU" Text="300" Width="80px" />
                                                    <asp:FileUpload runat="server" ID="upFileRU" />
                                                    <asp:Button ID="btncaricafileRU" UseSubmitBehavior="false" runat="server" Text="Inserisci Foto" OnClick="btnCaricafileRU_Click" />
                                                    <br />
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-sm-4"><strong>Imposta data (opzionale): </strong></div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtData" CssClass="form-control" runat="server"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="cal2" runat="server" Format="dd/MM/yyyy HH:mm:ss" TargetControlID="txtData">
                                        </Ajax:CalendarExtender>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row">
                            <div class="col-sm-4"><strong>Seleziona tipologia struttura (opzionale): </strong></div>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlTipologie" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTipologie_SelectedIndexChanged">
                                    <asp:ListItem Text="Seleziona tipologia" Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4"><strong>Seleziona struttura offerente (opzionale): </strong></div>
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlStruttura" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Seleziona struttura" Value="0" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div class="row">
            <div class="col-sm-12">

                <div style="margin-bottom: 30px">
                    <h2>Caricamento foto</h2>
                    <table width="100%" style="margin-top: 10px; font-size: 12px; table-layout: fixed; border-collapse: collapse;"
                        cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Foto" runat="server" style="width: 80%;">
                                <h3>
                                    <asp:Literal ID="litFoto" runat="server" Text=""></asp:Literal></h3>
                                <div>
                                    <h2>Caricamento foto scheda</h2>
                                    <hr />
                                    <asp:FileUpload ID="UploadFoto" runat="server" />
                                    <br />
                                    <asp:TextBox runat="server" ID="txtDescrizione" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnCarica" runat="server" CssClass="btn btn-primary btn-sm" Text="Carica Foto" OnClick="btnCarica_Click" />
                                    <asp:Button ID="btnModifica" runat="server" CssClass="btn btn-primary btn-sm" Text="Modifica Descrizione Foto" OnClick="btnModifica_Click" />
                                    <asp:Button ID="btnElimina" runat="server" CssClass="btn btn-danger btn-sm" Text="Elimina Foto" OnClick="btnElimina_Click" />
                                    <br />
                                    <br />

                                </div>
                                <div style="width: 100%; height: 500px;max-width:500px">
                                    <asp:HiddenField ID="txtFotoSchema" runat="server" />
                                    <asp:HiddenField ID="txtFotoValori" runat="server" />
                                    <div style="width: 100%; float: left; height: 400px; padding: 10px">
                                        <a id="linkFoto" runat="server" target="_blank">
                                            <asp:Image ID="imgFoto" Width="100%" runat="server" ImageUrl="" /></a>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div style="float: left; padding: 10px; height: 500px; width: 100%">
                                        <ul style="list-style: none; display: inline">
                                            <asp:Repeater runat="server" ID="rptImmagini">
                                                <ItemTemplate>
                                                    <li style="list-style: none; display: inline">
                                                        <asp:ImageButton Width="80px" Height="80px" ToolTip='<%# Eval("Descrizione").ToString() %>' ID="imgAntFoto" CommandArgument='<%# Eval("NomeFile").ToString() %>'
                                                            OnClick="linkgalleria_click" runat="server" ImageUrl='<%# ComponiUrlGalleriaAnteprima(Eval("NomeAnteprima").ToString()) %>' />
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


        </div>
    </div>
</asp:Content>

