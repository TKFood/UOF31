<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SCSHRLoginUrl.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_SCSHR_WebPages_SCSHRLoginUrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <iframe runat="server" id="iFramePage"  width="100%" height="100%" frameborder="0" scrolling="no"></iframe>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
