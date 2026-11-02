<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="REPORT_PURTGPURTH.aspx.cs" Inherits="CDS_WebPage_PUR_REPORT_PURTGPURTH" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .multiline-textbox {
            word-break: break-all;
            white-space: pre-line;
            height: 60px; /* 根据需要设置高度 */
        }

        .custom-button {
            width: 150px; /* 设置按钮宽度 */
            height: 40px; /* 设置按钮高度 */
            font-size: 20px; /* 设置按钮字体大小 */
            color: #ffffff; /* 设置按钮文本颜色 */
            background-color: #007bff; /* 设置按钮背景颜色 */
            border: none; /* 去除按钮边框 */
            border-radius: 5px; /* 设置按钮圆角 */
        }

            .custom-button:hover {
                background-color: #0056b3; /* 设置按钮鼠标悬停时的背景颜色 */
            }

        .file-preview {
            max-width: 400px; /* 限制圖片的最大寬度 */
            max-height: 400px; /* 限制圖片的最大高度 */
            height: auto; /* 保持圖片的寬高比 */
            width: auto; /* 保持圖片的寬高比 */
            border: 1px solid #ddd;
            border-radius: 4px;
            padding: 2px;
        }

        /* 確保圖片連結不會導致佈局錯亂 */
        .file-preview-container {
            display: inline-block;
        }

        .VerticalCenter {
            /* 這是關鍵：將內容垂直對齊到儲存格的中間 */
            vertical-align: middle;
        }
    </style>
    <script>    


</script>

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
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="日期起~迄"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TXT_SDATES" runat="server" Width="130px" TextMode="Date"></asp:TextBox>
                                    <asp:Label ID="Label1" runat="server" Text=" ~ "></asp:Label>
                                    <asp:TextBox ID="TXT_EDATES" runat="server" Width="130px" TextMode="Date"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="品號/品名"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TXT_MB001" runat="server" Width="130px"></asp:TextBox>
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
                                        <Fast:Grid ID="Grid1" Style="overflow-x: auto; width: 100%" OnRowUpdating="Grid1_RowUpdating" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="進貨日" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl進貨日"
                                                            runat="server"
                                                            Text='<%# Bind("進貨日") %>'
                                                            Style="word-break: break-all; white-space: pre-line; display: block;"
                                                            Width="100%">
                                                    </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="進貨單別" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="進貨單別"
                                                            runat="server"
                                                            Text='<%# Bind("進貨單別") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="進貨單號" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="進貨單號"
                                                            runat="server"
                                                            Text='<%# Bind("進貨單號") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品號" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="品號"
                                                            runat="server"
                                                            Text='<%# Bind("品號") %>'
                                                            Style="word-break: break-all; white-space: pre-line; text-align: left;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品名" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="品名"
                                                            runat="server"
                                                            Text='<%# Bind("品名") %>'
                                                            Style="word-break: break-all; white-space: pre-line; text-align: left;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="規格" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="規格"
                                                            runat="server"
                                                            Text='<%# Bind("規格") %>'
                                                            Style="word-break: break-all; white-space: pre-line; text-align: left;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="數量" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="數量"
                                                            runat="server"
                                                            Text='<%# Bind("數量") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="單位" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="單位"
                                                            runat="server"
                                                            Text='<%# Bind("單位") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="批號" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="批號"
                                                            runat="server"
                                                            Text='<%# Bind("批號") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="單價" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="單價"
                                                            runat="server"
                                                            Text='<%# Bind("單價") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="庫別" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="庫別"
                                                            runat="server"
                                                            Text='<%# Bind("庫別") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="是否確認" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="是否確認"
                                                            runat="server"
                                                            Text='<%# Bind("是否確認") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
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
                        <asp:Label ID="Label99" runat="server" Text="Label99"></asp:Label>
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>​
        </ContentTemplate>

    </asp:UpdatePanel>


</asp:Content>

