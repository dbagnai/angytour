<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="GestioneBookingPrenotazioni.aspx.cs" Inherits="AreaContenuti_GestioneBookingPrenotazioni" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="<%= CommonPage.ReplaceCdnLinks( "~/js/jqwidgets/jqwidgets/styles/jqx.base.css") %>" />
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxcore.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxbuttons.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxscrollbar.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxdata.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxdate.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxscheduler.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxscheduler.api.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxdatetimeinput.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxmenu.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxcalendar.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxtooltip.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxwindow.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxcheckbox.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxlistbox.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxdropdownlist.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxnumberinput.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxradiobutton.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/jqxinput.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/globalization/globalize.js"></script>
    <script type="text/javascript" src="/js/jqwidgets/jqwidgets/globalization/globalize.culture.it-IT.js"></script>
    <%--<script type="text/javascript" src="/js/jqwidgets/scripts/demos.js"></script>--%>
    <script type="text/javascript" src="<%= CommonPage.ReplaceCdnLinks("~/lib/js/bookingeventi.js" ) + CommonPage.AppendModTime(Server,"~/lib/js/bookingeventi.js") %>"></script>

    <div class="container">
        <div class="row">
            <div class="col-xs-12">
                <div class="pull-left">
                    <label>Selezione Location</label>
                    <select id="ddl1reslist" class="form-control searchlist" style="width: auto" onchange="reloadbyresourceid(this)">
                    </select>
                </div>
                <div class="pull-right">
                    <div id="divMessages"></div>
                </div>
                <br /> 
           
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12" style="height: 90vh">

                <div id="schedulerprenotazioni"></div>
            </div>
        </div>
    </div>

</asp:Content>

