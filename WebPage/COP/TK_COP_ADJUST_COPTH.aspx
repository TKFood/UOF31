<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TK_COP_ADJUST_COPTH.aspx.cs" Inherits="CDS_WebPage_COP_TK_COP_ADJUST_COPTH" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
        /* 1. 活力亮藍 (原本的款式) */
        .btn-add_1 {
            background: #4a90e2;
            color: white !important;
            border: none;
            padding: 6px 15px;
            border-radius: 4px;
            transition: all 0.3s;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            cursor: pointer;
        }

            .btn-add_1:hover {
                background: #357abd;
                box-shadow: 0 4px 8px rgba(0,0,0,0.15);
            }

        /* 2. 深邃優雅藍 (原本的款式) */
        .btn-add_2 {
            background: #1e3a8a;
            color: white !important;
            border: none;
            padding: 6px 15px;
            border-radius: 4px;
            transition: all 0.3s;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            cursor: pointer;
        }

            .btn-add_2:hover {
                background: #172554;
            }

        /* 3. 森林清新綠 (成功、完成) */
        .btn-add_3 {
            background: #2ecc71;
            color: white !important;
            border: none;
            padding: 6px 15px;
            border-radius: 4px;
            transition: all 0.3s;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            cursor: pointer;
        }

            .btn-add_3:hover {
                background: #27ae60;
            }

        /* 4. 暖陽明亮橘 (注意、修改) */
        .btn-add_4 {
            background: #f39c12;
            color: white !important;
            border: none;
            padding: 6px 15px;
            border-radius: 4px;
            transition: all 0.3s;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            cursor: pointer;
        }

            .btn-add_4:hover {
                background: #e67e22;
            }

        /* 5. 簡約質感知性灰 (次要、返回) */
        .btn-add_5 {
            background: #7f8c8d;
            color: white !important;
            border: none;
            padding: 6px 15px;
            border-radius: 4px;
            transition: all 0.3s;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            cursor: pointer;
        }

            .btn-add_5:hover {
                background: #636e72;
            }

        /* 6. 現代時尚紫 (特殊功能、亮點) */
        .btn-add_6 {
            background: #9b59b6;
            color: white !important;
            border: none;
            padding: 6px 15px;
            border-radius: 4px;
            transition: all 0.3s;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            cursor: pointer;
        }

            .btn-add_6:hover {
                background: #8e44ad;
            }
        /* 優雅 */
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
            background-color: #4a90e2;
            padding: 5px 25px;
            border: 1px solid #345577;
            border-radius: 4px;
            cursor: pointer;
            /* 減輕陰影，讓它不那麼沉重 */
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            transition: all 0.2s ease;
        }

        Button1_Click
        .btn-search:hover {
            /* 懸停時稍微亮一點 */
            background-color: #5180b3;
            box-shadow: 0 3px 6px rgba(0, 0, 0, 0.15);
            filter: brightness(1.1);
        }
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
                                    <asp:Label ID="Label7" runat="server" Text="日期/單號: "></asp:Label>
                                    <asp:TextBox ID="TextBox_TH002" Text="" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="查詢" OnClick="Button1_Click"
                                        CssClass="btn-search" />
                                </td>
                            </tr>
                        </table>
                        <table class="PopTable">
                            <tr>
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid1" OnRowUpdating="Grid1_RowUpdating" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="單別" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="label_單別"
                                                            runat="server"
                                                            Text='<%# Bind("單別") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="單號" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="label_單號"
                                                            runat="server"
                                                            Text='<%# Bind("單號") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="客戶" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="label_客戶"
                                                            runat="server"
                                                            Text='<%# Bind("客戶") %>'
                                                            Style="word-break: break-all; white-space: pre-line; text-align: left;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="序號" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="label_序號"
                                                            runat="server"
                                                            Text='<%# Bind("序號") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品號" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="label_品號"
                                                            runat="server"
                                                            Text='<%# Bind("品號") %>'
                                                            Style="word-break: break-all; white-space: pre-line; text-align: left;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品名" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="label_品名"
                                                            runat="server"
                                                            Text='<%# Bind("品名") %>'
                                                            Style="word-break: break-all; white-space: pre-line; text-align: left;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="本幣未稅金額" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="label_本幣未稅金額"
                                                            runat="server"
                                                            Text='<%# Eval("本幣未稅金額", "{0:N0}") %>'
                                                            Style="word-break: break-all; white-space: pre-line; text-align: right;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="本幣稅額" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="label_本幣稅額"
                                                            runat="server"
                                                            Text='<%# Eval("本幣稅額", "{0:N0}") %>'
                                                            Style="word-break: break-all; white-space: pre-line; text-align: right;"
                                                            Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                    總金額不變<br />
                                                    -未稅金額加1<br />
                                                    且稅額減1
                                                </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button ID="Grid1Button1" runat="server"
                                                            Text="未稅加1且稅額減1"
                                                            CommandName="Grid1Button1"
                                                            CommandArgument='<%# Container.DataItemIndex %>'
                                                            OnClientClick="return confirm('調整 未稅加1且稅額減1 確定？');"
                                                            CssClass="grid-btn btn-add_1" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                     <HeaderTemplate>
                                                    總金額不變<br />
                                                    -未稅金額減1 <br />
                                                    且稅額加1
                                                </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button ID="Grid1Button2" runat="server"
                                                            Text="未稅減1且稅額加1"
                                                            CommandName="Grid1Button2"
                                                            CommandArgument='<%# Container.DataItemIndex %>'
                                                            OnClientClick="return confirm('調整 未稅減1且稅額加1 確定？');"
                                                            CssClass="grid-btn btn-add_2" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                     <HeaderTemplate>
                                                    總金額加1<br />
                                                    -未稅金額加1
                                                </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button ID="Grid1Button3" runat="server"
                                                            Text="總金額加1-未稅加1"
                                                            CommandName="Grid1Button3"
                                                            CommandArgument='<%# Container.DataItemIndex %>'
                                                            OnClientClick="return confirm('調整 總金額加1-未稅金額加1 確定？');"
                                                            CssClass="grid-btn btn-add_3" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                     <HeaderTemplate>
                                                    總金額加1<br />
                                                    -稅額加1
                                                </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button ID="Grid1Button4" runat="server"
                                                            Text="總金額加1-稅額加1"
                                                            CommandName="Grid1Button4"
                                                            CommandArgument='<%# Container.DataItemIndex %>'
                                                            OnClientClick="return confirm('調整 總金額加1-稅額加1確定？');"
                                                            CssClass="grid-btn btn-add_4" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                      <HeaderTemplate>
                                                    總金額減1<br />
                                                    -未稅金額減1
                                                </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button ID="Grid1Button5" runat="server"
                                                            Text="總金額減1-未稅減1"
                                                            CommandName="Grid1Button5"
                                                            CommandArgument='<%# Container.DataItemIndex %>'
                                                            OnClientClick="return confirm('調整 總金額減1-未稅金額減1 確定？');"
                                                            CssClass="grid-btn btn-add_5" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                     <HeaderTemplate>
                                                    總金額減1<br />
                                                    - 稅額減1
                                                </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button ID="Grid1Button6" runat="server"
                                                            Text="總金額減1-稅額減1"
                                                            CommandName="Grid1Button6"
                                                            CommandArgument='<%# Container.DataItemIndex %>'
                                                            OnClientClick="return confirm('調整 總金額減1-稅額減1確定？');"
                                                            CssClass="grid-btn btn-add_6" />
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

