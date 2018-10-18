<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchedaResourceStampa.aspx.cs" EnableViewState="false"
    EnableTheming="true" Inherits="_SchedaResourceStampa" %>

<%--<%@ Register Src="~/AspNetPages/UC/Pager.ascx" TagName="Pager" TagPrefix="UC" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=10" />
     <meta name="metaRobots" id="meta1" content="noindex,nofollow" runat="Server" />
    <style type="text/css">
        @import url(https://fonts.googleapis.com/css?family=Open+Sans:400,600);
        @import url(https://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600);

        body {
            font-weight: 300;
        }

        .blog-price {
            font-size: 1.5em;
            font-weight: 400;
            margin-bottom: 0px;
            color: #0d5bab;
        }

        .blog-sezione {
            font-size: 1.4em;
            width: 100%;
            border-bottom: 1px solid #666;
            color: _#666;
            margin: 20px 0px 10px 0px;
        }

        /* H1&H3  for page titles */
        h1, h2, h3, h4, h5, h6 {
            font-family: "Raleway", sans-serif;
            margin: 0 0 15px 0;
            font-weight: 300;
            color: #4b4b4b;
        }

        h1 {
            font-size: 1.7em;
            line-height: 35px;
        }

        h2 {
            font-size: 1.4em;
            line-height: 30px;
        }

        h3 {
            font-size: 1.2em;
            line-height: 26px;
        }

        h4 {
            font-size: 1em;
            line-height: 22px;
        }

        h5 {
            font-size: 1em;
            line-height: 22px;
        }

        h6 {
            font-size: 1em;
            line-height: 20px;
        }

        table a:link {
            color: #666;
            font-weight: bold;
            text-decoration: none;
        }

        table a:visited {
            color: #999999;
            font-weight: bold;
            text-decoration: none;
        }

        table a:active,
        table a:hover {
            color: #0d5bab;
            text-decoration: underline;
        }

        table {
            font-family: Arial, Helvetica, sans-serif;
            color: #666;
            font-size: 12px;
            text-shadow: 1px 1px 0px #fff;
            background: #eaebec;
            margin: 0px;
            border: #ccc 1px solid;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            -moz-box-shadow: 0 1px 2px #d1d1d1;
            -webkit-box-shadow: 0 1px 2px #d1d1d1;
            box-shadow: 0 1px 2px #d1d1d1;
        }

            table th {
                padding: 8px;
                border-top: 1px solid #fafafa;
                border-bottom: 1px solid #e0e0e0;
                background: #ededed;
                background: -webkit-gradient(linear, left top, left bottom, from(#ededed), to(#ebebeb));
                background: -moz-linear-gradient(top, #ededed, #ebebeb);
            }

                table th:first-child {
                    text-align: left;
                    padding-left: 8px;
                }

            table tr:first-child th:first-child {
                -moz-border-radius-topleft: 3px;
                -webkit-border-top-left-radius: 3px;
                border-top-left-radius: 3px;
            }

            table tr:first-child th:last-child {
                -moz-border-radius-topright: 3px;
                -webkit-border-top-right-radius: 3px;
                border-top-right-radius: 3px;
            }

            table tr {
                text-align: center;
                padding-left: 8px;
            }

            table td:first-child {
                text-align: left;
                padding-left: 8px;
                border-left: 0;
            }

            table td {
                padding: 8px;
                border-top: 1px solid #ffffff;
                border-bottom: 1px solid #e0e0e0;
                border-left: 1px solid #e0e0e0;
                background: #fafafa;
                background: -webkit-gradient(linear, left top, left bottom, from(#fbfbfb), to(#fafafa));
                background: -moz-linear-gradient(top, #fbfbfb, #fafafa);
            }

            table tr.even td {
                background: #f6f6f6;
                background: -webkit-gradient(linear, left top, left bottom, from(#f8f8f8), to(#f6f6f6));
                background: -moz-linear-gradient(top, #f8f8f8, #f6f6f6);
            }

            table tr:last-child td {
                border-bottom: 0;
            }

                table tr:last-child td:first-child {
                    -moz-border-radius-bottomleft: 3px;
                    -webkit-border-bottom-left-radius: 3px;
                    border-bottom-left-radius: 3px;
                }

                table tr:last-child td:last-child {
                    -moz-border-radius-bottomright: 3px;
                    -webkit-border-bottom-right-radius: 3px;
                    border-bottom-right-radius: 3px;
                }

            table tr:hover td {
                background: #f2f2f2;
                background: -webkit-gradient(linear, left top, left bottom, from(#f2f2f2), to(#f0f0f0));
                background: -moz-linear-gradient(top, #f2f2f2, #f0f0f0);
            }

        .item {
            width: 100%;
            text-align: center;
            margin: 0px auto;
        }

            .item img {
                width: 80%;
            }
    </style>
</head>
<body style="background: none; width: 880px; font-family: Raleway; border: none">
    <!--onload="javascript:ResizeWindow();javascript:PrintWindow();" -->
    <form id="formcontenuti" runat="server">
        <asp:ScriptManager ID="ScriptManagerMaster" runat="server" AllowCustomErrorsRedirect="True"
            AsyncPostBackErrorMessage="Errore generico. Contattare HelpDesk" AsyncPostBackTimeout="400"
            EnablePartialRendering="true" OnAsyncPostBackError="ScriptManagerMaster_AsyncPostBackError"
            EnablePageMethods="true" EnableScriptLocalization="true" EnableScriptGlobalization="true">
            <Scripts>
                <asp:ScriptReference Path="~/js/jquery-2.1.3.min.js"/>
                <asp:ScriptReference Path="~/js/jquery-migrate-1.4.1.min.js" />
            </Scripts>
        </asp:ScriptManager>
        <!--DEFIANTJS to query json obkects-->
        <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/js/defiant/defiant.min.js")%>"></script>
        <script type="text/javascript" src='<%= CommonPage.ReplaceCdnLinks("~/js/localforage.min.js") %>'></script>
        <script type="text/javascript" src='<%= CommonPage.ReplaceCdnLinks("~/js/pako/pako.min.js") %>'></script>
        <script type="text/javascript" src='<%= CommonPage.ReplaceCdnLinks("~/js/moment-with-locales.min.js") %>'></script>

        <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/lib/js/common.js")%>"></script>
        <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/lib/js/searchcontrol.js")%>"></script>
        <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/lib/js/immobili2.js")%>"></script>

        <script type="text/javascript">
            initLingua('<%= Lingua %>');

            (function wait() {
                if (typeof injectandloadimmobile === "function") {
                    injectandloadimmobile("schedadetailsimmobileStampa.html", "divItemContainter1", "divitem", false, true, "<%= idOfferta %>");
                    }
                    else {
                        setTimeout(wait, 50);
                    }
                })();
            $(document).ready(function () {
               
               <%-- setTimeout(function () { loadref(InitImmobiliJS,  '<%= idOfferta %>','<%= Lingua %>'); }, 100);--%>

                <%--    var promise = initreferencesdata('<%= Lingua %>');
                promise.then(function (result) {
                    InitSearchControls()
                    CaricaVariabiliRiferimento(function (result) {
                        message = result;
                        InitImmobiliJS(<%= idOfferta %>);
                       });
                });--%>

            });
        </script>
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
        <div style="width: 100%; height: 60px; padding-top: 30px">
            <img src='<%=CommonPage.ReplaceAbsoluteLinks("~/images/main_logo.png")%>'
                style="width: 100%; max-width: 300px;" alt="<%= ConfigurationManager.AppSettings["Nome"].ToString()%>" />
        </div>

        <div id="divItemContainter1"></div>





        <div style="width: 100%; text-align: center; border-top: 2px solid #ccc; font-size: 12px">
            <br />
            <%= references.ResMan("Common", Lingua, "txtFooter") %>
            <br />
            <br />
        </div>

    </form>
</body>
</html>
