<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TB_YEARS_COST_CALS.aspx.cs" Inherits="CDS_WebPage_PUR_TB_YEARS_COST_CALS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .big-bold-button {
            font-size: 20px;
            font-weight: bold;
            background-color: #007bff;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 8px;
            box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.2);
            cursor: pointer;
        }

            .big-bold-button:hover {
                background-color: #0056b3;
            }


        /* 4. 不同功能的顏色調整（顯眼且好看） */
        .btn-add {
            background: #4a90e2;
            color: white !important;
        }
        /* 優雅藍 */
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="資料">
                    </telerik:RadTab>
                    <telerik:RadTab Text="其他">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                    <div id="tabs-1">
                        <table class="PopTable">
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="年度"></asp:Label>
                                    <asp:TextBox ID="TextBox_YEARS" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label_Todays" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="查詢" OnClick="Button1_Click"
                                        meta:resourcekey="btn1Resource1"
                                        Style="width: 150px; /* 調整寬度 */
                                        height: 30px; /* 調整高度 */
                                        background-color: #4CAF50; /* 調整背景顏色 (綠色) */
                                        color: white; /* 調整字體顏色 (白色) */
                                        font-size: 16px; /* 調整字體大小 */
                                        font-family: '微軟正黑體', Arial, sans-serif; /* 調整字體 */
                                        border: none; /* 移除邊框 (可選) */
                                        border-radius: 8px; /* 圓角邊框 (可選) */
                                        cursor: pointer; /* 鼠標樣式 (可選) */" />
                                </td>
                            </tr>
                        </table>
                        <table class="PopTable">
                            <tr>
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_ID" runat="server" Text='<%# Bind("編號說明") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="年度" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_年度" runat="server" Text='<%# Bind("年度") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="類別" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_類別" runat="server" Text='<%# Bind("類別") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="營業成本百分比a" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_營業成本百分比" runat="server"
                                                            Text='<%# Bind("營業成本百分比", "{0:N2}") %>'
                                                            Style="word-break: break-all; white-space: pre-line; width: 100%; text-align: right; display: block;">
                                                         </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="明細" ItemStyle-Width="400px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_明細" runat="server" Text='<%# Bind("明細") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="進貨金額佔類別平均%b" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_進貨金額佔類別平均" runat="server"
                                                            Text='<%# Bind("進貨金額佔類別平均", "{0:N2}") %>'
                                                            Style="word-break: break-all; white-space: pre-line; width: 100%; text-align: right; display: block;">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="調漲增加(減少%)c" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_調漲增加減少" runat="server"
                                                            Text='<%# Bind("調漲增加減少", "{0:F2}") %>'
                                                            CssClass="form-control"
                                                            Style="width: 100%; text-align: right;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="影響成本率增加% d=a*b*c" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1_影響成本率增加" runat="server"
                                                            Text='<%# Bind("影響成本率增加", "{0:N2}") %>'
                                                            Style="word-break: break-all; white-space: pre-line; width: 100%; text-align: right; display: block;">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="修改計算" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Grid1Button1" runat="server" Text="修改計算" CommandName="Grid1Button1"
                                                            CommandArgument='<%# Container.DataItemIndex %>'
                                                            OnClientClick="return confirm('確定？');"
                                                            CssClass="grid-btn btn-add" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </Fast:Grid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView99" runat="server">
                    <div id="tabs-99">
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

