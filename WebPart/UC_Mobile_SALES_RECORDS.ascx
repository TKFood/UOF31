<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_Mobile_SALES_RECORDS.ascx.cs" Inherits="CDS_WebPart_UC_Mobile_SALES_RECORDS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>


<!-- 在CSS文件中定义样式 -->
<style>
    .multiline-textbox {
        width: 200px;
    }

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

    /* 3. 按鈕字體優化 */
    .grid-btn {
        /* 使用較圓潤的字體並加粗 */
        font-family: "Microsoft JhengHei", sans-serif;
        font-weight: 600;
        font-size: 14px;
        letter-spacing: 0.5px;
        padding: 6px 12px;
        border-radius: 6px; /* 更圓潤的邊角 */
        border: none;
        cursor: pointer;
        transition: all 0.3s ease; /* 增加平滑過渡感 */
        /* 允許折行 */
        white-space: normal !important;
        line-height: 1.4;
        width: 100%;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1); /* 輕微陰影更有立體感 */
    }

    /* 4. 不同功能的顏色調整（顯眼且好看） */
    .btn-add {
        background: #4a90e2;
        color: white !important;
    }
    /* 優雅藍 */
    .btn-close {
        background: #e74c3c;
        color: white !important;
    }
    /* 警示紅 */
    .btn-reopen {
        background: #f39c12;
        color: white !important;
    }
    /* 活潑橘 */

    .grid-btn:hover {
        transform: translateY(-1px); /* 懸停時輕微上浮 */
        box-shadow: 0 4px 8px rgba(0,0,0,0.15);
        filter: brightness(1.1);
    }

    /* 5. 內容文字優化（Label） */
    .fast-grid span {
        line-height: 1.6;
        font-size: 14px;
    }

    /* 獨立按鈕的基本樣式 */
    .btn-main {
        font-family: "Microsoft JhengHei", "微軟正黑體", sans-serif;
        font-weight: 600;
        font-size: 16px; /* 比 Grid 內的按鈕大一點 */
        letter-spacing: 2px; /* 字間距拉開更有質感 */
        color: white !important;
        background: linear-gradient(135deg, #6a11cb 0%, #2575fc 100%); /* 漸層色更現代 */
        padding: 8px 24px; /* 左右留白多一點，呈現長橢圓感 */
        border: none;
        border-radius: 50px; /* 圓角設大一點，變膠囊狀 */
        cursor: pointer;
        box-shadow: 0 4px 15px rgba(37, 117, 252, 0.3); /* 藍色系的發光陰影 */
        transition: all 0.3s ease;
        display: inline-block;
    }

        .btn-main:hover {
            transform: translateY(-2px); /* 懸停浮起 */
            box-shadow: 0 6px 20px rgba(37, 117, 252, 0.4);
            filter: brightness(1.1);
        }

        .btn-main:active {
            transform: translateY(0px); /* 點擊按下感 */
        }

    /* 修改按鈕：深灰色或石板色，看起來穩重且專業 */
    .btn-edit {
        background-color: #6c757d;
        color: white !important;
    }

        .btn-edit:hover {
            background-color: #5a6268;
            filter: brightness(1.2);
        }

    .btn-search {
        font-family: "Microsoft JhengHei", sans-serif;
        font-weight: 600;
        font-size: 15px;
        letter-spacing: 2px;
        color: #ffffff !important;
        /* 改用更透亮的商務藍 */
        background-color: #446e9b;
        padding: 5px 25px;
        border: 1px solid #345577;
        border-radius: 4px;
        cursor: pointer;
        /* 減輕陰影，讓它不那麼沉重 */
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        transition: all 0.2s ease;
    }

        .btn-search:hover {
            /* 懸停時稍微亮一點 */
            background-color: #5180b3;
            box-shadow: 0 3px 6px rgba(0, 0, 0, 0.15);
            filter: brightness(1.1);
        }
</style>
<html>

<head>
    <title>業務</title>
</head>
<body>
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage">
        <Tabs>
            <telerik:RadTab Text="業務-客戶-專案-回覆" PageViewID="RadPageView1">
            </telerik:RadTab>
            <telerik:RadTab Text="業務-客戶-專案-新增" PageViewID="RadPageView2">
            </telerik:RadTab>
            <telerik:RadTab Text="業務-客戶-專案-修改" PageViewID="RadPageView3">
            </telerik:RadTab>
            <telerik:RadTab Text="業務-客戶-專案-回覆明細" PageViewID="RadPageView4">
            </telerik:RadTab>
            <telerik:RadTab Text="資料" PageViewID="RadPageView5">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" PageViewID="0">
            <div id="tabs-1">
                <table class="PopTable">
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="業務員: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>
                        </td>
                    </tr>
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
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="排序: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                            <asp:Button ID="Button1" runat="server"
                                Text="查詢"
                                OnClick="btn1_Click"
                                meta:resourcekey="btn1_Resource1"
                                CssClass="btn-search" />
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
                                                <asp:Label ID="業務員" runat="server" Text='<%# Bind("SALES") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="客戶" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="客戶" runat="server" Text='<%# Bind("CLIENTS") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="回覆期限" DataField="EDAYS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="專案內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="專案內容" runat="server" Text='<%# Bind("EVENTS") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="已回覆內容(最近)" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("COMMENTS") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆日期(最近)" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("COMMENTSADDDATES") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="是否結案" runat="server" Text='<%# Bind("ISCLOSE") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="結案日" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="結案日" runat="server" Text='<%# Bind("ISCLOSEDATES") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="輸入回覆" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%--<asp:TextBox ID="txtNewField" runat="server" Text='<%# Bind("COMMENTS") %>' TextMode="MultiLine" Rows="3" Width="200px"></asp:TextBox>--%>
                                                <asp:TextBox ID="txtNewField" runat="server" Text='<%# Bind("COMMENTS") %>' TextMode="MultiLine" CssClass="multiline-textbox" Rows="3" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="新增回覆" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Grid1Button1" runat="server" Text="新增回覆" CommandName="Grid1Button1"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    OnClientClick="return confirm('確定？');"
                                                    CssClass="grid-btn btn-add" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="確定結案" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Grid1Button2" runat="server" Text="確定結案" CommandName="Grid1Button2"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    OnClientClick="return confirm('確定？');"
                                                    CssClass="grid-btn btn-close" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="改未結案" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Grid1Button3" runat="server" Text="改未結案" CommandName="Grid1Button3"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    OnClientClick="return confirm('確定？');"
                                                    CssClass="grid-btn btn-reopen" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
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
                            <asp:Label ID="Label4" runat="server" Text="專案內容: "></asp:Label>
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
                            <asp:Button ID="Button2" runat="server"
                                Text="新增專案"
                                OnClick="btn2_Click"
                                meta:resourcekey="btn2_Resource1"
                                CssClass="btn-main" />
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
                            <asp:Button ID="Button3" runat="server"
                                Text="查詢"
                                OnClick="btn3_Click"
                                meta:resourcekey="btn3_Resource1"
                                CssClass="btn-search" />
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
                                                <asp:TextBox ID="業務員" runat="server" Text='<%# Bind("SALES") %>' TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="客戶" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="客戶" runat="server" Text='<%# Bind("CLIENTS") %>' TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆期限" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="回覆期限" runat="server" Text='<%# Bind("EDAYS") %>' TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="專案內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="專案內容" runat="server" Text='<%# Bind("EVENTS") %>' TextMode="MultiLine" Rows="3"></asp:TextBox>
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
                                        <asp:TemplateField HeaderText="修改" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Grid2Button1" runat="server"
                                                    Text="修改"
                                                    CommandName="Grid2Button1"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    OnClientClick="return confirm('確定？');"
                                                    CssClass="grid-btn btn-edit" />
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
                <table class="PopTable">
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="客戶: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_CLIENTS3" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="是否結案: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListISCLOSE3" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>

                            <asp:Button ID="Button4" runat="server"
                                Text="查詢"
                                OnClick="btn4_Click"
                                meta:resourcekey="btn4_Resource1"
                                CssClass="btn-search" />
                        </td>
                    </tr>


                </table>
                <table style="width: 100%">
                    <tr style="width: 100%">
                        <td colspan="2" style="width: 100%">
                            <div style="overflow-x: auto; width: 100%">
                                <Fast:Grid ID="Grid3" OnRowDataBound="Grid3_RowDataBound" OnRowCommand="Grid3_OnRowCommand" runat="server" OnBeforeExport="OnBeforeExport3" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid3Resource1" OnPageIndexChanging="grid_PageIndexChanging3" CssClass="fast-grid">
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
                                        <asp:TemplateField HeaderText="專案內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="專案內容" runat="server" Text='<%# Bind("EVENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="已回覆內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("COMMENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆日期" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("ADDDATES") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="是否結案" runat="server" Text='<%# Bind("ISCLOSE") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="輸入回覆" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNewField3" runat="server" Text='' TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="新增回覆" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Grid3Button1" runat="server"
                                                    Text="新增回覆"
                                                    CommandName="Grid3Button1"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    OnClientClick="return confirm('確定？');"
                                                    CssClass="grid-btn btn-add" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="ID3" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line" Width="30px"></asp:Label>
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
        <telerik:RadPageView ID="RadPageView5" runat="server" Selected="false" PageViewID="5">
            <div id="tabs-">
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
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

    function autoResizeTextBox(textbox) {
        textbox.style.height = 'auto'; // Reset the height
        textbox.style.height = (textbox.scrollHeight) + 'px'; // Adjust the height to fit the content
    }

    function initializeTextBoxes() {
        var textboxes = document.getElementsByClassName('multiline-textbox');
        for (var i = 0; i < textboxes.length; i++) {
            autoResizeTextBox(textboxes[i]);
        }
    }

    // Call this function on window load and after partial postback
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initializeTextBoxes);
    window.onload = initializeTextBoxes;
</script>

