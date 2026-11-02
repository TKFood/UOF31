<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TKREPORTTB_EIP_SCH_WORK.aspx.cs" Inherits="CDS_WebPage_TKREPORTTB_EIP_SCH_WORK" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

    <script>    
        $(function () {

        });

    </script>

    <table class="PopTable">
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label9" runat="server" Text="日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <telerik:RadDatePicker ID="RadDatePicker1" runat="server" Width="120px"></telerik:RadDatePicker>
                <asp:Label ID="Label11" runat="server" Text="~"></asp:Label>
                <telerik:RadDatePicker ID="RadDatePicker2" runat="server" Width="120px"></telerik:RadDatePicker>
                <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
            </td>
            <td class="PopTableRightTD">

                <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label2" runat="server" Text="查詢:" meta:resourcekey="Label4Resource1" ></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:Label ID="Label1" runat="server" Text="是否結案"></asp:Label>
                <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td></td>
            <td class="PopTableRightTD">

                <asp:Button ID="Button1" runat="server" Text=" 查詢 "
                    OnClick="btn1_Click" meta:resourcekey="btn4Resource1" />
            </td>
        </tr>
    </table>
    <table class="PopTable">
        <td colspan="2" class="PopTableRightTD">
            <div style="overflow-x: auto; width: 100%">
                <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server" OnBeforeExport="OnBeforeExport" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging">
                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>

                    <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                    <Columns>
                        <asp:BoundField HeaderText="被交辨人" DataField="NAME1" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="交辨事項" DataField="SUBJECT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="回覆內容" DataField="DESCRIPTION" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="預計完成日" DataField="END_TIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="延遲天數" DataField="DIFFDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="交辨人" DataField="NAME2" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="狀態" DataField="STATUS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                </Fast:Grid>
            </div>
        </td>
        </tr>
    </table>
</asp:Content>

