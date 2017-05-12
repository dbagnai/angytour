<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="Iscriviti.aspx.cs" Inherits="_Iscriviti" Title="" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
      <script type="text/javascript">
          makeRevLower = true;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server" ID="updIscrivi">
        <ContentTemplate>
                    <div class="row">
                        <div class="alert-danger">
                            <asp:Label Font-Size="Medium" ID="output" runat="server" Text=""></asp:Label>
                            <asp:Label runat="server" ID="litDescrizioneIscrivi" Text="" Font-Size="Medium" />
                        </div>
                    </div>
                    <div class="row">

                        <script type="text/javascript" language="javascript">
                            function OpenWindow(Url, NomeFinestra, features) {
                                window.open(Url, NomeFinestra, features);
                            }
                        </script>

                        <asp:PlaceHolder runat="server" ID="plhForm">
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal2" runat="server" Text='<%$ Resources: Common, FormTesto2 %>' />
                                  
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtNome" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal3" runat="server" Text='<%$ Resources: Common, FormTesto16l %>' />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtCognome"
                                        ErrorMessage='<%$ Resources: Common, FormTesto16lErr %>' Text="*" ValidationGroup="MailInfo" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtCognome" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal5" runat="server" Text='<%$ Resources: Common, FormTesto4 %>' />*
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtEmail"
                                        ErrorMessage='<%$ Resources: Common, FormTesto4Err %>' Text="*" ValidationGroup="MailInfo" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtEmail" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>

                            <!-- Inserire qui ddlnaz/reg/prov/com -->
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal11" runat="server" Text='<%$ Resources: Common, FormTesto5 %>' />
                   
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlNazione"
                                        CssClass="form-control" Width="50%" AutoPostBack="true" runat="server" AppendDataBoundItems="true"
                                        OnSelectedIndexChanged="ddlNazione_SelectedIndexChanged" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal12" runat="server" Text='<%$ Resources: Common, FormTesto6 %>' />
                      
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlRegione"
                                        CssClass="form-control" Width="50%" AutoPostBack="true" runat="server" AppendDataBoundItems="true"
                                        OnSelectedIndexChanged="ddlRegione_SelectedIndexChanged" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal13" runat="server" Text='<%$ Resources: Common, FormTesto7 %>' />
                          
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlProvincia"
                                        AutoPostBack="true" CssClass="form-control" Width="50%" runat="server" AppendDataBoundItems="true"
                                        OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal14" runat="server" Text='<%$ Resources: Common, FormTesto8 %>' />
                
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlComune"
                                        CssClass="form-control" Width="50%" runat="server" AppendDataBoundItems="true" />
                                </div>
                            </div>
                            <!-- Fine selez. geografica-->
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal7" runat="server" Text='<%$ Resources: Common, FormTesto9 %>' />
             
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtCap" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal6" runat="server" Text='<%$ Resources: Common, FormTesto10 %>' />
                
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtIndirizzo" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal1" runat="server" Text='<%$ Resources: Common, FormTesto20 %>' />
         
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtPiva" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal8" runat="server" Text='<%$ Resources: Common, FormTesto11 %>' />
                                   
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtTelefono" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal9" runat="server" Text='<%$ Resources: Common, FormTesto12 %>' />
                        
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtProfessione" Width="100%" Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal10" runat="server" Text='<%$ Resources: Common, FormTesto13 %>' />
                                     
                                </div>
                                <div class="col-sm-9">

                                    <asp:TextBox ID="txtNascita" Width="100%"
                                        Style="height: 25px; margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />

                                    <Ajax:CalendarExtender ID="calext" runat="server" Format="dd/MM/yyyy"
                                        TargetControlID="txtNascita">
                                    </Ajax:CalendarExtender>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">
                                    <asp:Literal ID="Literal4" runat="server" Text='<%$ Resources: Common, FormTesto14 %>' />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtDescrizione" Width="100%" Font-Names="Raleway" TextMode="MultiLine"
                                        Height="250px" Style="margin-bottom: 5px; border: 1px Solid Black; background-color: #f0f0f0" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3" style="text-align: right">&nbsp;</div>
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="chkPrivacy" runat="server" Checked="true" Text='<%$ Resources: Common , testoPrivacy %>' />
                                    <a id="linkPrivacy" runat="server" onclick="OpenWindow('PrivacyI.htm','','scrollbars=yes,width=600,height=400')"
                                        href="javascript:;">(D.Lgs 196/2003)</a>
                                    <br />
                                    <asp:CheckBox ID="chkConsensoMail" runat="server" Checked="true" Text='<%$ Resources: Common , testoConsenso1 %>' />
                                    <br />
                                    <asp:Button ID="btnInvia" runat="server" Text="Invia" CausesValidation="true" UseSubmitBehavior="false"
                                        ValidationGroup="MailInfo" SkinID="btn1" OnClick="btnInvia_Click" />
                         

                                    <asp:ValidationSummary runat="server" ID="Summary" ValidationGroup="MailInfo" DisplayMode="BulletList"
                                        ShowSummary="true" HeaderText="Rilevati dati mancanti per l'invio della proposta:" />
                                </div>
                            </div>

                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="plhRisposta" runat="server" Visible="false">
                            <div class="alert alert-block">
                                <asp:Label Font-Size="Medium" ID="lblRisposta" runat="server" Text=""></asp:Label>
                                <br />
                            </div>
                        </asp:PlaceHolder>

                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 