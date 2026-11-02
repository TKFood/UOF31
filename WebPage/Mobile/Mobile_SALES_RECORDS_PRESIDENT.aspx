<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MobileMasterPage.master" AutoEventWireup="true" CodeFile="Mobile_SALES_RECORDS_PRESIDENT.aspx.cs" Inherits="CDS_WebPage_Mobile_SALES_RECORDS_PRESIDENT" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- 在CSS文件中定义样式 -->
    <style>
        .custom-button {
            background-color: #FF5733;
            color: #FFFFFF;
            width: 150px; /* 设置宽度 */
            height: 40px; /* 设置高度 */
            font-size: 16px; /* 设置字体大小 */
        }

        .fast-grid {
        }

            /* 表头样式 */
            .fast-grid th {
                background-color: dodgerblue; /* 背景颜色 */
                color: white; /* 字体颜色 */
                font-weight: bold; /* 字体粗体 */
                border: 2px solid white; /* 边框粗线 */
                padding: 5px;
                text-align: center;
            }

            /* 单元格框线 */
            .fast-grid td {
                border: 1px solid #000;
                padding: 5px;
                text-align: left; /* 根据需要更改文本对齐方式 */
            }
    </style>
    <html>

    <head>
        <title>業務</title>
    </head>
    <body>
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
        <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
            <Tabs>
                <telerik:RadTab Text="報表">
                </telerik:RadTab>
                <telerik:RadTab Text="資料">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                <div id="tabs-1">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label8" runat="server" Text="日期:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="txtDate1" runat="server" Width="100px"></asp:TextBox>
                                    <asp:Label ID="Label11" runat="server" Text="~"></asp:Label>
                                    <asp:TextBox ID="txtDate2" runat="server" Width="100px"></asp:TextBox>
                                    <asp:Label ID="Label12" runat="server" Text=" "></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button2" runat="server" Text=" 查詢 " OnClick="btn2_Click" meta:resourcekey="btn2_Resource1" />
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button3" runat="server" Text=" 匯出 " OnClick="btn3_Click" meta:resourcekey="btn3_Resource1" />
                                </td>
                            </tr>

                        </table>
                        <table style="width: 100%">
                            <tr style="width: 100%">
                                <td colspan="2" style="width: 100%">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_OnRowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1" CssClass="fast-grid">

                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                            <Columns>
                                                <asp:BoundField HeaderText="業務員" DataField="業務員" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="舊客(有客代)" DataField="客戶" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="新客" DataField="新客" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="拜訪目的" DataField="拜訪目的" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="訪談內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="訪談內容" runat="server" Text='<%# Bind("訪談內容") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="訪談日期" DataField="訪談日期" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="圖片">
                                                    <ItemTemplate>
                                                        <div runat="server" id="ImageContainer" class="image-container">
                                                            <!-- Images will be added here dynamically -->
                                                        </div>
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



            <telerik:RadPageView ID="RadPageView2" runat="server" Selected="false">
                <div>
                </div>
            </telerik:RadPageView>
        </telerik:RadMultiPage>​
    </body>


    <script>       

</script>

    </html>



</asp:Content>

