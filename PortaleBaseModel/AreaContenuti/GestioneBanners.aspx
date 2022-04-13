<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="GestioneBanners.aspx.cs" Inherits="AreaContenuti_GestioneBannersNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function ConfirmCancella() {
            //document.getElementByID("CheckBoxListExCtrl").value
            var conferma = confirm('Sei sicuro di voler cancellare il banner ?');
            if (conferma) {
                $get("<%=cancelHidden.ClientID%>").value = "true";
            }
            else {
                $get("<%=cancelHidden.ClientID%>").value = "false";
            }
        }
    </script>
    <div class="row">
        <div class="col-xs-12">
            <div class="row">
                <div class="col-sm-6">
                    <asp:HiddenField ID="cancelHidden" runat="server" Value="false" />
                    <h2>Banners</h2>
                    <%--          <asp:DropDownList runat="server" ID="ddlAreaAnnunci" AppendDataBoundItems="true"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlAreaAnnunci_SelectedIndexChanged"
                        Visible="false">
                    </asp:DropDownList>--%>
                    <br />
                    <asp:Label BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="2px" Visible="false"
                        ID="lblAnnunci" Text="SELEZIONE HOMEPAGE PER INSERIMENTO BANNER ANNUNCI <br/><br/> Per inserire banner home annunci obbligatorio usare nell'url il parametro Tipologia=ann000xxx. Es. <br/> AREA AUTO url?Lingua=I&Tipologia=ann000003 <br/> AREA LAVORO url?Lingua=I&Tipologia=ann000002 <br/> AREA CASA url?Lingua=I&Tipologia=ann000001<br/><br/>  E' OBBLIGATORIOO INSERIRE ENTRAMBI GLI URL PER ITALIANO INGLESE e RUSSO"
                        runat="server" />
                    <br />
                    <ul style="list-style-type: none;">
                        <asp:Repeater runat="server" ID="rptFoto">
                            <ItemTemplate>
                                <li style="list-style-type: none; display: inline;">
                                    <div style="float: left; margin-right: 3px">
                                        <asp:ImageButton ID="btnImmagine" OnClick="btnImmagine_Click" runat="server" Width="80px"
                                            Height="80px" ImageUrl='<%# Eval("ImageUrl") %>' CommandArgument='<%# Eval("ImageUrl") + "," + Eval("Id") %>'
                                            OnPreRender="btnImmagine_PreRender" />
                                        <br />
                                        <asp:Label ID="lblNavigate" Font-Size="8pt" runat="server" Text='<%# Eval("NavigateUrl") %>'></asp:Label>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <br />
                    <div style="clear: both">
                    </div>
                </div>
                <div class="col-sm-6">
                    <h2>Upload Banner</h2>
                    <br />
                    <asp:FileUpload ID="UploadFoto" CssClass="btn btn-default" runat="server" /><br />
                    <asp:Button ID="btnCarica" CssClass="btn btn-primary btn-sm" runat="server" Text="Carica Banner" OnClick="btnCarica_Click" />
                    <asp:Button ID="btnAggiorna" runat="server" CssClass="btn btn-primary btn-sm" Text="Aggiorna Banner" OnClick="btnAggiorna_Click" />
                    <asp:Button ID="btnElimina" runat="server" CssClass="btn btn-danger btn-sm" Text="Elimina Banner" OnClick="btnElimina_Click"  OnClientClick="javascript:ConfirmCancella()" UseSubmitBehavior="true"  />
                    <br />
                    <br />
                    <asp:Button ID="btnInserisciInglese" CssClass="btn btn-primary btn-sm" runat="server" Text="Carica Img Inglese"
                        OnClick="btnINserisciInglese_Click" />
                    <asp:Button ID="btnInserisciFR" CssClass="btn btn-primary btn-sm" runat="server" Text="Carica Img Francese"
                        OnClick="btnINserisciFR_Click" />
                    <asp:Button ID="btnInserisciDE" CssClass="btn btn-primary btn-sm" runat="server" Text="Carica Img Tedesco"
                        OnClick="btnINserisciDE_Click" />
                    <asp:Button ID="ButtonES" CssClass="btn btn-primary btn-sm" runat="server" Text="Carica Img Spagnolo"
                        OnClick="btnINserisciES_Click" />
                                <asp:Button ID="btnInserisciRusso" CssClass="btn btn-primary btn-sm" runat="server" Text="Carica Img Russo"
                        OnClick="btnINserisciRusso_Click" />

                </div>
            </div>

            <asp:Label ID="lbldimBanner" runat="server" ForeColor="Black" Font-Bold="true"></asp:Label><br />
            <asp:Label ID="output" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            <h3>Carica Banner</h3>

            <div style="background: #ccc; padding: 10px 30px 30px 30px;">
                <div class="row">
                    <div class="col-sm-2">
                        <strong>
                            <asp:Label ID="Label1" Text="Progressivo Banner" runat="server" />
                        </strong>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtProgressivo" />
                    </div>
                </div>

                <div class="row">
                    <ul class="nav nav-pills" style="margin-left: 0;">
                        <li class="active"><a data-toggle="pill" href="#tabita">Italiano</a></li>
                        <li><a data-toggle="pill" href="#tebeng">Inglese</a></li>
                        <li><a data-toggle="pill" href="#tabfr">Francese</a></li>
                        <li><a data-toggle="pill" href="#tabde">Tedesco</a></li>
                        <li><a data-toggle="pill" href="#tabes">Spagnolo</a></li>
                        <li><a data-toggle="pill" href="#tabru">Russo</a></li>
                    </ul>
                    <div class="tab-content">
                        <div id="tabita" class="tab-pane fade in active">
                            <h4>BANNER ITALIANO</h4>
                            <asp:Image ID="imgI" Width="250" runat="server" ImageUrl="" />
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="litNavigateUrl" runat="server" Text="Link Destinazione Ita" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtNavigateUrl" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1  text-right">
                                    <strong>
                                        <asp:Label Width="30%" ID="litDescrizioneI" runat="server" Text="Descrizione Ita" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass=" form-control" Height="250px" TextMode="MultiLine" ID="txtDescrizioneI"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1  text-right">
                                    <strong>
                                        <asp:Label ID="Label2" runat="server" Text="Img Alt Text Ita" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtImgalttextI" runat="server"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <div id="tebeng" class="tab-pane fade">
                            <h4>BANNER INGLESE</h4>
                            <asp:Image ID="imgGB" Width="250" runat="server" ImageUrl="" />
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="litNavigateUrlGB" runat="server" Text="Link Destinazione Inglese" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtNavigateUrlGB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label Width="30%" ID="litDescrizioneGB" runat="server" Text="Descrizione Eng" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass=" form-control" Height="250px" TextMode="MultiLine" ID="txtDescrizioneGB"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="Label3" runat="server" Text="Img Alt Text Eng" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtImgalttextGB" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="tabru" class="tab-pane fade">
                            <h4>BANNER RUSSO</h4>
                            <asp:Image ID="imgRU" Width="250" runat="server" ImageUrl="" />
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="litNavigateUrlRU" runat="server" Text="Link Destinazione Russo" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtNavigateUrlRU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label Width="30%" ID="litDescrizioneRU" runat="server" Text="Descrizione Russo" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass=" form-control" Height="250px" TextMode="MultiLine" ID="txtDescrizioneRU"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="Label4" runat="server" Text="Img Alt Text Ru" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtImgalttextRU" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="tabfr" class="tab-pane fade">
                            <h4>BANNER FRENCH</h4>
                            <asp:Image ID="imgFR" Width="250" runat="server" ImageUrl="" />
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="Label5" runat="server" Text="Link Destinazione Francese" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtNavigateUrlFR" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label Width="30%" ID="Label6" runat="server" Text="Descrizione Francese" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass=" form-control" Height="250px" TextMode="MultiLine" ID="txtDescrizioneFR"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="Label7" runat="server" Text="Img Alt Text Francese" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtImgalttextFR" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div id="tabde" class="tab-pane fade">
                            <h4>BANNER TEDESCO</h4>
                            <asp:Image ID="imgDE" Width="250" runat="server" ImageUrl="" />
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="Label8" runat="server" Text="Link Destinazione Tedesco" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtNavigateUrlDE" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label Width="30%" ID="Label9" runat="server" Text="Descrizione Tedesco" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass=" form-control" Height="250px" TextMode="MultiLine" ID="txtDescrizioneDE"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="Label10" runat="server" Text="Img Alt Text Tedesco" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtImgalttextDE" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div id="tabes" class="tab-pane fade">
                            <h4>BANNER SPAGNOLO</h4>
                            <asp:Image ID="imgES" Width="250" runat="server" ImageUrl="" />
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="Label11" runat="server" Text="Link Destinazione Spagnolo" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtNavigateUrlES" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label Width="30%" ID="Label12" runat="server" Text="Descrizione Spagnolo" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass=" form-control" Height="250px" TextMode="MultiLine" ID="txtDescrizioneES"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-bottom: 5px">
                                <div class="col-sm-1 text-right">
                                    <strong>
                                        <asp:Label ID="Label13" runat="server" Text="Img Alt Text Spagnolo" />
                                    </strong>
                                </div>
                                <div class="col-sm-10">
                                    <asp:TextBox CssClass="form-control" ID="txtImgalttextES" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>




                    </div>
                </div>


                <%--                <div class="row">
                    <div class="col-sm-6">
                    </div>
                    <div class="col-sm-6">
                    </div>
                </div>--%>
            </div>


        </div>
    </div>
</asp:Content>

