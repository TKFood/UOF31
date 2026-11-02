<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TB_PUR_WORKS.aspx.cs" Inherits="CDS_WebPage_PUR_TB_PUR_WORKS" %>

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
                    <telerik:RadTab Text="查詢">
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
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label9" runat="server" Text="負責承辦"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="FIND_TextBox_負責承辦" runat="server"
                                        Text=""
                                        Width="50%"
                                        TextMode="MultiLine"
                                        Rows="1"
                                        Style="resize: none; word-break: break-all;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label10" runat="server" Text="工作對象"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="FIND_TextBox_工作對象" runat="server"
                                        Text=""
                                        Width="50%"
                                        TextMode="MultiLine"
                                        Rows="1"
                                        Style="resize: none; word-break: break-all;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label8" runat="server" Text="處理進度"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <%-- 改為 DropDownList --%>
                                    <asp:DropDownList ID="FIND_DropDownList1" runat="server"
                                        Width="20%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
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
                                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_ID" runat="server"
                                                            Text='<%# Bind("ID") %>'
                                                            Style="word-break: break-all; white-space: pre-line;" Width="100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="負責承辦" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_負責承辦" runat="server"
                                                            Text='<%# Bind("負責承辦") %>'
                                                            TextMode="MultiLine" Rows="3"
                                                            Width="100%" Style="resize: none;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="工作對象" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_工作對象" runat="server"
                                                            Text='<%# Bind("工作對象") %>'
                                                            TextMode="MultiLine" Rows="3"
                                                            Width="100%" Style="resize: none;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="處理情況敘述" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_處理情況敘述" runat="server"
                                                            Text='<%# Bind("處理情況敘述") %>'
                                                            TextMode="MultiLine" Rows="3"
                                                            Width="100%" Style="resize: none;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="預計完成日期" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_預計完成日期" runat="server"
                                                            Text='<%# Bind("預計完成日期") %>'
                                                            TextMode="MultiLine" Rows="3"
                                                            Width="100%" Style="resize: none;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="處理進度" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DropDownList_處理進度" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                        <%-- 隱藏欄位用來存放資料庫原本的值，方便在 RowDataBound 比對 --%>
                                                        <asp:HiddenField ID="Hidden_處理進度" runat="server" Value='<%# Bind("處理進度") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="備註說明" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_備註說明" runat="server"
                                                            Text='<%# Bind("備註說明") %>'
                                                            TextMode="MultiLine" Rows="3"
                                                            Width="100%" Style="resize: none;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="主管交辦" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_主管交辦" runat="server"
                                                            Text='<%# Bind("主管交辦") %>'
                                                            TextMode="MultiLine" Rows="3"
                                                            Width="100%" Style="resize: none;"></asp:TextBox>
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
                                                        <asp:Button ID="btnDelete" runat="server" Text="刪除" CommandName="MUDELETE"
                                                            CommandArgument='<%# Eval("ID") %>'
                                                            OnClientClick="return confirm('是否刪除？');"
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
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label1" runat="server" Text="負責承辦"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="ADD_TextBox_負責承辦" runat="server"
                                        Text=""
                                        Width="50%"
                                        TextMode="MultiLine"
                                        Rows="3"
                                        Style="resize: none; word-break: break-all;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label3" runat="server" Text="工作對象"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="ADD_TextBox_工作對象" runat="server"
                                        Text=""
                                        Width="50%"
                                        TextMode="MultiLine"
                                        Rows="3"
                                        Style="resize: none; word-break: break-all;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label4" runat="server" Text="處理情況敘述"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="ADD_TextBox_處理情況敘述" runat="server"
                                        Text=""
                                        Width="50%"
                                        TextMode="MultiLine"
                                        Rows="3"
                                        Style="resize: none; word-break: break-all;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label5" runat="server" Text="預計完成日期"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="ADD_TextBox_預計完成日期" runat="server"
                                        Text=""
                                        Width="50%"
                                        TextMode="MultiLine"
                                        Rows="3"
                                        Style="resize: none; word-break: break-all;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label7" runat="server" Text="處理進度"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <%-- 改為 DropDownList --%>
                                    <asp:DropDownList ID="ADD_DropDownList_處理進度" runat="server"
                                        Width="50%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label6" runat="server" Text="備註說明"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="ADD_TextBox_備註說明" runat="server"
                                        Text=""
                                        Width="50%"
                                        TextMode="MultiLine"
                                        Rows="3"
                                        Style="resize: none; word-break: break-all;">
                                    </asp:TextBox>
                                </td>
                                <tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label11" runat="server" Text="主管交辦"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:TextBox ID="ADD_TextBox_主管交辦" runat="server"
                                        Text=""
                                        Width="50%"
                                        TextMode="MultiLine"
                                        Rows="3"
                                        Style="resize: none; word-break: break-all;">
                                    </asp:TextBox>
                                </td>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" Text="新增" OnClick="Button2_Click"
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
                            </tr>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView99" runat="server">
                    <div id="tabs-99">
                        <asp:Label ID="Label2" runat="server" Text="Labe2"></asp:Label>
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            ​
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

