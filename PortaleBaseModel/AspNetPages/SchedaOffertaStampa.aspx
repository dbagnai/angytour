<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchedaOffertaStampa.aspx.cs"
    EnableTheming="true" Inherits="_SchedaOffertaStampa" %>

<%--<%@ Register Src="~/AspNetPages/UC/Pager.ascx" TagName="Pager" TagPrefix="UC" %>--%>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="Pager" TagPrefix="UC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta name="metaRobots" id="meta1" content="noindex,nofollow" runat="Server" />
    <style type="text/css">
        @import url(https://fonts.googleapis.com/css?family=Open+Sans:400,600);
        @import url(https://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600);
    </style>
</head>
<body style="background: none; width: 880px; font-family: Raleway; border: 1px solid #ccc;">
    <!--onload="javascript:ResizeWindow();javascript:PrintWindow();" -->
    <form id="formcontenuti" runat="server">
        <asp:ScriptManager ID="ScriptManagerMaster" runat="server" AllowCustomErrorsRedirect="True"
            AsyncPostBackErrorMessage="Errore generico. Contattare HelpDesk" AsyncPostBackTimeout="400"
            EnablePartialRendering="true" OnAsyncPostBackError="ScriptManagerMaster_AsyncPostBackError"
            EnablePageMethods="true" EnableScriptLocalization="true" EnableScriptGlobalization="true" />
        <script type="text/javascript">
            function ResizeWindow() {
                window.resizeTo(800, 750);
                window.moveTo(10, 10);
            }
            function CloseWindow()
            { window.close(); }
            function PrintWindow() {
                window.print();
                window.close();
            }
        </script>
        <asp:Label ID="output" runat="server"></asp:Label>
        <%--<img id="Img1" style="border: none" alt="" runat="server"
        src='<%= references.ResMan("Common", Lingua,"imgLogo") %>" width="380" />--%>
        <div style="width: 100%; height: 100px; padding-top: 30px; padding-bottom: 30px; margin-bottom: 10px; text-align: center; background: url(../images/SiteSpecific/sfondohead.jpg) repeat-x top left">
            <img src='<%=CommonPage.ReplaceAbsoluteLinks("~/images/main_logo_footer.png")%>'
                style="height: 100%; max-width: 300px;" alt="<%= WelcomeLibrary.UF.ConfigManagement.ReadKey("Nome") %>" />
        </div>
        <asp:UpdatePanel ID="UserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="margin: 0px; text-align: center">
                    <asp:Repeater ID="rptOfferta" runat="server">
                        <ItemTemplate>
                            <div style="padding: 5px">
                                <span style="line-height: 28px; color: Black; font-size: 24px;">
                                    <asp:Literal ID="litTitolo" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(  Eval("Denominazione" + Lingua).ToString() ) %>'
                                        runat="server"></asp:Literal></span><br />
                              <%--  <span style="font-size: 10px; font-style: italic">
                                    <asp:Literal ID="Literal7" Text="pubblicato il " runat="server" /><asp:Literal ID="Literal8"
                                        Text='<%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento")) %>' runat="server" />
                                </span>--%>
                            </div>

                            <div id="divCutHeight" runat="server" style="overflow: hidden; margin: 5px; padding: 10px">
                                <asp:Image ID="Anteprima" runat="server" OnPreRender="ImgAnt_PreRender"
                                    ImageUrl='<%#  ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString(),true) %>' />
                            </div>
                            <div style="text-align: justify; padding: 10px; margin-top: 10px">
                                <asp:Label ID="lbldescri" runat="server" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo( CommonPage.ReplaceLinks( Eval("Descrizione" + Lingua).ToString()) ) %>'></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text='<%#  WelcomeLibrary.UF.Utility.SostituisciTestoACapo( CommonPage.ReplaceLinks( Eval("Datitecnici" + Lingua).ToString()) ) %>'></asp:Label>
                            </div>
                            <table cellpadding="1" cellspacing="1" style="width: 100%; border-collapse: collapse; table-layout: fixed; margin-top: 15px">
                                <tr>
                                    <td style="width: 40%; vertical-align: bottom">
                                        <div style="float: left; width: 340px; overflow: hidden; padding: 10px">
                                            <div>
                                                <asp:Literal ID="Literal1" Text='<%# ControlloVuoto("",  Eval("Indirizzo").ToString()) %>'
                                                    runat="server"></asp:Literal>
                                            </div>
                                            <div>
                                                <asp:Literal ID="Literal2" Text='<%# ControlloVuotoPosizione( Eval("CodiceComune").ToString()  , Eval("CodiceProvincia").ToString() ) %>'
                                                    runat="server"></asp:Literal>
                                            </div>
                                            <div>
                                                <asp:Literal ID="Literal5" Text='<%#  ControlloVuoto("Tel:", Eval("Telefono").ToString() )%>'
                                                    runat="server"></asp:Literal>
                                            </div>
                                            <div>
                                                <asp:Literal ID="Literal6" Text='<%#   ControlloVuoto("Fax:",Eval("Fax").ToString())%>'
                                                    runat="server"></asp:Literal>
                                            </div>
                                            <div>
                                                <a href='<%# "mailto:" + Eval("Email").ToString() %>'>
                                                    <asp:Literal ID="Literal3" Text='<%#  ControlloVuoto("Email:", Eval("Email").ToString()) %>'
                                                        runat="server"></asp:Literal></a>
                                            </div>
                                            <div>
                                                <a href='<%# "http://" + Eval("Website").ToString() %>' target="_blank" style="text-decoration: underline;">
                                                    <asp:Literal ID="Literal4" Text='<%# ControlloVuoto("Website:", Eval("Website").ToString()) %>'
                                                        runat="server"></asp:Literal></a>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div style="width: 100%; text-align: center; border-top: 2px solid #ccc; font-size: 12px">
                    <br />
                    <asp:Literal ID="Literal5" runat="server" Text='<%# references.ResMan("Common", Lingua,"txtFooter") %>' />
                    <br />
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
