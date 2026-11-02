<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true"
    Async="true"
    CodeFile="TestReport.aspx.cs" Inherits="CDS_WebPage_TestReport" %>

<%@ Register Src="~/CDS/WebPart/Report.ascx" TagPrefix="uc1" TagName="Report" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <uc1:Report runat="server" id="Report" />
</asp:Content>

