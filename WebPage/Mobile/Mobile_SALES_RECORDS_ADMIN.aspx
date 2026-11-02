<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="Mobile_SALES_RECORDS_ADMIN.aspx.cs" Inherits="CDS_WebPage_Mobile_Mobile_SALES_RECORDS_ADMIN" %>

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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
                <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage">
                    <Tabs>
                        <telerik:RadTab Text="交辨回覆" PageViewID="RadPageView1">
                        </telerik:RadTab>
                        <telerik:RadTab Text="交辨新增" PageViewID="RadPageView2">
                        </telerik:RadTab>
                        <telerik:RadTab Text="交辨修改" PageViewID="RadPageView3">
                        </telerik:RadTab>
                        <telerik:RadTab Text="資料" PageViewID="RadPageView4">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
                    <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" PageViewID="0">
                        <div id="tabs-1">
                            <table class="PopTable">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="客戶: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox_CLIENTS" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="是否結案: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownListISCLOSE" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button1" runat="server" Text="查詢 " OnClick="btn1_Click" meta:resourcekey="btn1_Resource1" />
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
                                                    <asp:TemplateField HeaderText="業務員" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="業務員" runat="server" Text='<%# Bind("SALES") %>' Style="word-break: break-all; white-space: pre-line" Width="60px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="客戶" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="客戶" runat="server" Text='<%# Bind("CLIENTS") %>' Style="word-break: break-all; white-space: pre-line" Width="60px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="回覆期限" DataField="EDAYS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="交辨內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="交辨內容" runat="server" Text='<%# Bind("EVENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="已回覆內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("COMMENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="是否結案" runat="server" Text='<%# Bind("ISCLOSE") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="輸入回覆" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtNewField" runat="server" Text='<%# Bind("COMMENTS") %>' TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="新增回覆" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Grid1Button1" runat="server" Text="新增回覆" CommandName="Grid1Button1" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('確定？');" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="確定結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Grid1Button2" runat="server" Text="確定結案" CommandName="Grid1Button2" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('確定？');" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="改未結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Grid1Button3" runat="server" Text="改未結案" CommandName="Grid1Button3" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('確定？');" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line" Width="30px"></asp:Label>
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
                    <telerik:RadPageView ID="RadPageView2" runat="server" Selected="false" PageViewID="1">
                        <div id="tabs-2">
                            <table class="PopTable">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="業務員: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="客戶: " Columns="100"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox1" runat="server" Columns="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="交辨內容: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Rows="4" Columns="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="回覆期限: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDate1" runat="server" Width="100px" onblur="validateDate()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="是否結案: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" Text="新增 " OnClick="btn2_Click" meta:resourcekey="btn2_Resource1" />
                                    </td>
                                </tr>


                            </table>
                        </div>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView3" runat="server" Selected="false" PageViewID="3">
                        <div id="tabs-3">
                            <table class="PopTable">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="客戶: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox_CLIENTS2" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="是否結案: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownListISCLOSE2" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button3" runat="server" Text="查詢 " OnClick="btn3_Click" meta:resourcekey="btn3_Resource1" />
                                    </td>
                                </tr>


                            </table>
                            <table style="width: 100%">
                                <tr style="width: 100%">
                                    <td colspan="2" style="width: 100%">
                                        <div style="overflow-x: auto; width: 100%">
                                            <Fast:Grid ID="Grid2" OnRowDataBound="Grid2_RowDataBound" OnRowCommand="Grid2_OnRowCommand" runat="server" OnBeforeExport="OnBeforeExport2" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid2Resource1" OnPageIndexChanging="grid_PageIndexChanging2" CssClass="fast-grid">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="業務員" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="業務員" runat="server" Text='<%# Bind("SALES") %>' TextMode="MultiLine" Rows="1"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="客戶" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="客戶" runat="server" Text='<%# Bind("CLIENTS") %>' TextMode="MultiLine" Rows="1"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="回覆期限" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="回覆期限" runat="server" Text='<%# Bind("EDAYS") %>' TextMode="MultiLine" Rows="1"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="交辨內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="交辨內容" runat="server" Text='<%# Bind("EVENTS") %>' TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="已回覆內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("COMMENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="是否結案" runat="server" Text='<%# Bind("ISCLOSE") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="修改" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Grid2Button1" runat="server" Text="修改" CommandName="Grid2Button1" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('確定？');" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line" Width="30px"></asp:Label>
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
                    <telerik:RadPageView ID="RadPageView4" runat="server" Selected="false" PageViewID="4">
                        <div id="tabs-4">
                        </div>
                    </telerik:RadPageView>


                </telerik:RadMultiPage>
            </ContentTemplate>
        </asp:UpdatePanel>
    </body>

    ​
    </html>
    <script type="text/javascript">
        function validateDate() {
            var dateTextbox = document.getElementById('<%= txtDate1.ClientID %>');
            var dateValue = dateTextbox.value.trim();

            // 使用正则表达式检查日期格式（yyyy/mm/dd）
            var dateRegex = /^\d{4}\/(?:0?[1-9]|1[0-2])\/(?:0?[1-9]|[12][0-9]|3[01])$/;

            if (!dateRegex.test(dateValue)) {
                alert('請輸入有效的日期，格式为YYYY/MM/DD。');
                var today = new Date();
                var formattedDate = today.getFullYear() + '/' + (today.getMonth() + 1) + '/' + today.getDate();
                dateTextbox.value = formattedDate;
                dateTextbox.focus();
            }
        }
    </script>
</asp:Content>

