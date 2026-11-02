<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KYTGridViewTable.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_KYTGridViewTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TextBox ID="txtKYTGridViewStruct" runat="server" TextMode="MultiLine" Rows="20" Width="45%"></asp:TextBox>
            <asp:Button ID="btnConvert" runat="server" Text="轉換" OnClick="btnConvert_Click" />
            <asp:TextBox ID="txtOutPutTable" runat="server" TextMode="MultiLine" Enabled="false" Rows="20" Width="45%"></asp:TextBox>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
