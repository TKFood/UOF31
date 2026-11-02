<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Temp_FixTravelTBY.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_SCSHR_WebPages_Temp_FixTravelTBY" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TextBox ID="txtDOC_NBR" runat="server"></asp:TextBox>
            <asp:Button ID="btnFix" runat="server" Text="修復" OnClick="btnFix_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
