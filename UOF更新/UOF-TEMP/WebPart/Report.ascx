<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report.ascx.cs" Inherits="CDS_WebPart_Report" %>

 
    <script type="text/javascript">
        function onSuccess(accessToken, embedUrl, embedReportId) {

            // Get models. models contains enums that can be used.
//            var models = window['powerbi-client'].models;

            // Embed configuration used to describe the what and how to embed.
            // This object is used when calling powerbi.embed.
            // This also includes settings and options such as filters.
            // You can find more information at https://github.com/Microsoft/PowerBI-JavaScript/wiki/Embed-Configuration-Details.


            var config = {
                type: 'report',
                tokenType: 1,
                accessToken: accessToken,
                embedUrl: embedUrl,
                id: embedReportId,
                permissions: 7,
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
    <div id="reportContainer" style="height: 768px; width: 1024px;"></div>
123456