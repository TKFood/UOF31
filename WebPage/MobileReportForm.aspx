<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileReportForm.aspx.cs" 
Async="true"
Inherits="CDS_WebPage_MobileReportForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <script src="../PowerBI/Scripts/powerbi.js"></script>
    <script type="text/javascript">
        function onSuccess(accessToken, embedUrl, embedReportId) {

            // Get models. models contains enums that can be used.
            var models = window['powerbi-client'].models;

            // Embed configuration used to describe the what and how to embed.
            // This object is used when calling powerbi.embed.
            // This also includes settings and options such as filters.
            // You can find more information at https://github.com/Microsoft/PowerBI-JavaScript/wiki/Embed-Configuration-Details.
            var config = {
                type: 'report',
                tokenType: models.TokenType.Embed,
                accessToken: accessToken,
                embedUrl: embedUrl,
                id: embedReportId,
                permissions: models.Permissions.All,
                settings: {
                    filterPaneEnabled: true,
                    navContentPaneEnabled: true
                }
            };
            // Get a reference to the embedded report HTML element
            var reportContainer = $('#reportContainer')[0];

            // Embed the report and display it within the div container.
            var report = powerbi.embed(reportContainer, config);
        };
    </script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">


  <telerik:RadStyleSheetManager ID="RadStyleSheetManager2" runat="server" >
            <StyleSheets>
                <telerik:StyleSheetReference Path="~/Common/Style/cssReset.css" />
                <telerik:StyleSheetReference Path="~/Common/Style/uof-fixed.css" />
                <telerik:StyleSheetReference Path="~/Common/Style/telerik.css" />
            </StyleSheets>
        </telerik:RadStyleSheetManager>
        <telerik:RadScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True" AsyncPostBackTimeout="36000">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                <asp:ScriptReference Path="~/Common/javascript/json2.min.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/Common/javascript/jQuery/plugin/jquery-ui.min.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/Common/javascript/jQuery/plugin/jquery.blockUI.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/Common/javascript/jQuery/plugin/ticker.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/Common/javascript/uofScript.js?v=18.5"  ScriptMode="Release"/>
                <asp:ScriptReference Path="~/Common/javascript/jQuery/plugin/jquery.cookie.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/Common/javascript/signature_pad.min.js" ScriptMode="Release"/>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Modal="True"
            ReloadOnShow="True" DestroyOnClose="True" ShowOnTopWhenMaximized="False"
            EnableShadow="True" ShowContentDuringLoad="False"
            OnClientCommand="$uof.telerikFix.windowManager.onClientCommand"
            Style="z-index: 9001" Behaviors="Resize, Close, Maximize, Move" VisibleStatusbar="False" Behavior="Resize, Close, Maximize, Move" meta:resourcekey="RadWindowManager1Resource1">
        </telerik:RadWindowManager>


    <div id="reportContainer" style="height: 99.5%; width: 100%;"></div>


    </form>
</body>
</html>
