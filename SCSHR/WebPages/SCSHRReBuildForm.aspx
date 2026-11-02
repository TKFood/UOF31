<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SCSHRReBuildForm.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_SCSHR_WebPages_SCSHRReBuildForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Button ID="btnAutoFix" runat="server" Text="全自動表單重送" OnClick="btnAutoFix_Click" />
            </div>
            <div>
                加班單單號
                <asp:TextBox ID="txtDOC_NBR_OV" runat="server"></asp:TextBox>
                <asp:Button ID="btnReCalcOverTime" runat="server" Text="重算加班單時數" OnClick="btnReCalcOverTime_Click" />
                <asp:Button ID="btnReImportOverTime" runat="server" Text="重新更新飛騰加班單簽核狀態" OnClick="btnReImportOverTime_Click" />
                <asp:Label ID="lblOVMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
             <div>
                請假單單號
                <asp:TextBox ID="txtDOC_NBR_LEV" runat="server"></asp:TextBox>
                <asp:Button ID="btnReImportLeave" runat="server" Text="重新更新飛騰請假單簽核狀態" OnClick="btnReImportLeave_Click" />
                <asp:Label ID="lblLEVMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
            <div>
                出差單單號
                <asp:TextBox ID="txtDOC_NBR_TRA" runat="server"></asp:TextBox>
                <asp:Button ID="btnReImportTravel" runat="server" Text="重新更新飛騰出差單簽核狀態" OnClick="btnReImportTravel_Click" />
                <asp:Label ID="lblTRAMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
