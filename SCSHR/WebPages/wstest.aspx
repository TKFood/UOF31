<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wstest.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="cds_dreamful_wstest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .gv {
            background-color: white;
        }
        .gv > tbody > tr > th {
            background-color: #336699;
            color: white;
            border: 1px solid #DEDEDE;
            padding: 10px;
        }

        .gv > tbody > tr > td {
            padding: 10px;
            border: 1px solid #DEDEDE;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnClearAllGroupAndMember" runat="server" Text="刪除所有子部門及會員" OnClick="btnClearAllGroupAndMember_Click" />
            <asp:GridView ID="gvTest" runat="server" CssClass="gv">
            </asp:GridView>
            <div style="height: 25px;">&nbsp;</div>
            <asp:GridView ID="gvTest2" runat="server" CssClass="gv">
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
