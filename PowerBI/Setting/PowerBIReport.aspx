<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="PowerBIReport.aspx.cs" Inherits="CDS_PowerBI_Setting_PowerBIReport" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../Scripts/powerbi.js"></script>
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
    <div id="reportContainer" style="height: 99.5%; width: 100%;"></div>
</asp:Content>

