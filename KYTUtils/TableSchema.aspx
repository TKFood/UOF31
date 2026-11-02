<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TableSchema.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_TableSchema" %>

<%--資料表SCHEMA輸出工具--%>
<%--產生TABLE SCHEMA EXCEL文件--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .headerTable > tbody > tr > td {
            border: 0px;
            padding: 5px 5px 5px 5px;
            white-space: nowrap;
            vertical-align: middle;
        }

            .headerTable > tbody > tr > td:nth-child(1) {
                text-align: right;
            }
    </style>
    <script>
        function SelectAll(selected) {
            $('#trCheckboxList input[type=checkbox]').prop('checked', selected);
            return false;
        }
        function btnSelectAll_Click(sender, args) {
            SelectAll(true);
            args.set_cancel(true);
            return false;
        }
        function btnUnSelectAll_Click(sender, args) {
            SelectAll(false);
            args.set_cancel(true);
            return false;
        }
        function btnSelectByCondition_Click(sender, args) {
            var txtFilter = $('#<%=txtFilter.ClientID%>');
            if (txtFilter.length > 0) {
                $('#trCheckboxList input[type=checkbox][name*=' + txtFilter.val() + ']').prop('checked', true);
            }
            args.set_cancel(true);
            return false;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
        <ContentTemplate>
            <table class="headerTable" border="0">
                <tr>
                    <td width="1%" nowrap>資料庫連通字串
                    </td>
                    <td nowrap>
                        <asp:TextBox ID="txtConnectionString" runat="server" Width="800px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="1%" nowrap></td>
                    <td nowrap>
                        <telerik:RadButton runat="server" ID="btnRefresh" OnClick="btnRefresh_Click" Text="重新整理"></telerik:RadButton>
                        <telerik:RadButton runat="server" ID="btnExport" OnClick="btnExport_Click" Text="匯出EXCEL"></telerik:RadButton>
                    </td>
                </tr>
                <tr id="trCheckboxList">
                    <td width="1%" nowrap style="vertical-align: top;">選擇資料表</td>
                    <td nowrap>
                        <table width="100%" border="0">
                            <tr>
                                <td>
                                    <telerik:RadButton runat="server" ID="btnSelectAll" Text="全選" OnClientClicking="btnSelectAll_Click"></telerik:RadButton>
                                    <telerik:RadButton runat="server" ID="btnUnSelectAll" Text="全取消" OnClientClicking="btnUnSelectAll_Click"></telerik:RadButton>
                                    依條件選取：<asp:TextBox ID="txtFilter" runat="server"></asp:TextBox>
                                    <telerik:RadButton runat="server" ID="btnSelectByCondition" Text="依條件選取" OnClientClicking="btnSelectByCondition_Click"></telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                        <div runat="server" id="divTables">
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
