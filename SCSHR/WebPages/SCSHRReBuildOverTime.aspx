<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SCSHRReBuildOverTime.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_SCSHR_WebPages_SCSHRReBuildOverTime" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Button ID="btnAutoFix" runat="server" Text="全自動表單重送" OnClick="btnAutoFix_Click" />
            </div>
            <div>
                <asp:TextBox ID="txtDOC_NBR" runat="server"></asp:TextBox>
                <asp:Button ID="btnReCalc" runat="server" Text="重算表單時數" OnClick="btnReCalc_Click" />
                <asp:Button ID="btnReImport" runat="server" Text="重新更新飛騰簽核狀態" OnClick="btnReImport_Click" />
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
