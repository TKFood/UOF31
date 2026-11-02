<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MobileMasterPage.master" AutoEventWireup="true"
    Async="true"
    CodeFile="MobileReport.aspx.cs" Inherits="CDS_WebPage_MobileReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
    <div id="reportContainer" style="height: 768px; width:1024px ;"></div>


</asp:Content>

