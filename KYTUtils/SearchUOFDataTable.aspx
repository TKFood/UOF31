<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchUOFDataTable.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_SearchUOFDataTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        $(function () {
            //$(".TableName").on("focus", function () {
            //   var rjson = $uof.pageMethod.syncUc("CDS/KYTUtils/SearchUOFDataTable.aspx", "getAllTableName", ["1"]);
            //    var jobj = JSON.parse(rjson);
            //    debugger;
            //});
        });
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TextBox ID="txtTableNames" runat="server" CssClass="TableName"></asp:TextBox><br />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
