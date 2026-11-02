<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TK_TBPURGOODS.aspx.cs" Inherits="CDS_WebPage_PUR_TK_TBPURGOODS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- 在CSS文件中定义样式 -->
    <style>
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

        .btn-email {
            font-family: "Microsoft JhengHei", sans-serif;
            font-weight: 600;
            font-size: 15px;
            letter-spacing: 2px;
            color: #ffffff !important;
            /* 改用更透亮的商務藍 */
            background-color: #1a508b;
            padding: 5px 25px;
            border: 1px solid #345577;
            border-radius: 4px;
            cursor: pointer;
            /* 減輕陰影，讓它不那麼沉重 */
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            transition: all 0.2s ease;
        }

            .btn-email:hover {
                /* 懸停時稍微亮一點 */
                background-color: #5180b3;
                box-shadow: 0 3px 6px rgba(0, 0, 0, 0.15);
                filter: brightness(1.1);
            }

        .btn-ADD {
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

            .btn-ADD:hover {
                /* 懸停時稍微亮一點 */
                background-color: #5180b3;
                box-shadow: 0 3px 6px rgba(0, 0, 0, 0.15);
                filter: brightness(1.1);
            }

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
    </style>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="資料">
                    </telerik:RadTab>
                    <telerik:RadTab Text="新增">
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
                                    <asp:Label ID="Label7" runat="server" Text="廠商: "></asp:Label>
                                    <asp:TextBox ID="QUERY_TextBox1" Text="" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="品項: "></asp:Label>
                                    <asp:TextBox ID="QUERY_TextBox2" Text="" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="查詢"
                                        OnClick="Button1_Click"
                                        meta:resourcekey="btn1Resource1"
                                        CssClass="btn-search" />
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button2" runat="server" Text="寄送MAIL"
                                        OnClick="Button2_Click"
                                        meta:resourcekey="btn1Resource2"
                                        CssClass="btn-email" />
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
                                                <asp:TemplateField HeaderText="廠商" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_廠商"
                                                            runat="server"
                                                            Text='<%# Bind("廠商") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="200px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品號" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_品號"
                                                            runat="server"
                                                            Text='<%# Bind("品號") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品項" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_品項"
                                                            runat="server"
                                                            Text='<%# Bind("品項") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="300px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="數量" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_數量"
                                                            runat="server"
                                                            Text='<%# Bind("數量") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="單價" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_單價"
                                                            runat="server"
                                                            Text='<%# Bind("單價") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="總計" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_總計"
                                                            runat="server"
                                                            Text='<%# Bind("總計") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="提供日期" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_提供日期"
                                                            runat="server"
                                                            Text='<%# Bind("提供日期") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="備註" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_備註"
                                                            runat="server"
                                                            Text='<%# Bind("備註") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="200px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="月叫貨量" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_月叫貨量"
                                                            runat="server"
                                                            Text='<%# Bind("月叫貨量") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="200px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnUpdate" runat="server" Text="更新" CommandName="MYUPDATE"
                                                            CommandArgument='<%# Eval("ID") %>'
                                                            OnClientClick="return confirm('是否更新？');"
                                                            CssClass="grid-btn btn-add" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnDelete" runat="server" Text="刪除" CommandName="MYDELETE"
                                                            CommandArgument='<%# Eval("ID") %>'
                                                            OnClientClick="return confirm('是否刪除？');"
                                                            CssClass="grid-btn btn-close" />
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
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <div id="tabs-2">
                        <h3>新增</h3>
                        <table class="PopTable">
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="廠商：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox1" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="品號：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox9" runat="server" Width="300px" />
                                 </td>
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="品項：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox2" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="數量：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox3" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="單價：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox4" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="總計：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox5" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="提供日期：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox6" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="備註：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox7" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                    <asp:Label Text="月叫貨量：" runat="server" />
                                </td>
                                <td style="width: 85%; padding: 5px;">
                                    <asp:TextBox ID="ADD_TextBox8" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <div style="padding: 10px 5px;">
                                        <asp:Button ID="btnADD"
                                            runat="server"
                                            Text="新增"
                                            OnClick="btnADD_Click"
                                            CssClass="btn-ADD" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPageView>

                <telerik:RadPageView ID="RadPageView99" runat="server">
                    <div id="tabs-99">
                        <asp:Label ID="Label2" runat="server" Text="Labe2"></asp:Label>
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>​
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

