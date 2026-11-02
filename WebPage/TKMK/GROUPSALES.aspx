<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MobileMasterPage.master" AutoEventWireup="true" CodeFile="GROUPSALES.aspx.cs" Inherits="CDS_WebPage_TKMK_GROUPSALES" %>

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
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="車號: "></asp:Label>
                                    <asp:TextBox ID="ADD_TextBox1" Text="" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="車名: "></asp:Label>
                                    <asp:TextBox ID="ADD_TextBox2" Text="" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="優惠券帳號: "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="兌換券: "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="Button2" runat="server" Text="新增團車" OnClick="Button2_Click"
                                        meta:resourcekey="btn1Resource2"
                                        Style="width: 150px; /* 寬度略增 */
                                            height: 30px; /* 高度略增 */
        
                                            /* === 顏色調整 (更明顯) === */
                                            background-color: #FF5722; /* 鮮豔的亮橘色/赤紅色 */
                                            color: white; /* 純白色字體，與背景形成高對比 */
        
                                            /* === 字體調整 === */
                                            font-size: 18px; /* 字體加大 */
                                            font-weight: bold; /* 字體加粗 */
                                            font-family: '微軟正黑體', Arial, sans-serif; /* === 視覺優化 === */
                                            border: none; /* 移除邊框 */
                                            border-radius: 10px; /* 增加圓角 */
                                            box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.3); /* 添加陰影，增加立體感 */
                                            cursor: pointer;" />
                                </td>
                            </tr>
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
                                        <Fast:Grid ID="Grid1" OnRowUpdating="Grid1_RowUpdating" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="序號" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_序號"
                                                            runat="server"
                                                            Text='<%# Bind("序號") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="車號" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_車號"
                                                            runat="server"
                                                            Text='<%# Bind("車號") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="車名" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_車名"
                                                            runat="server"
                                                            Text='<%# Bind("車名") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="優惠券帳號" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_優惠券帳號"
                                                            runat="server"
                                                            Text='<%# Bind("優惠券帳號") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="業務員名" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_業務員名"
                                                            runat="server"
                                                            Text='<%# Bind("業務員名") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="兌換券" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_兌換券"
                                                            runat="server"
                                                            Text='<%# Bind("兌換券") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="狀態" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_狀態"
                                                            runat="server"
                                                            Text='<%# Bind("狀態") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100%">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                          
                                                <asp:TemplateField HeaderText="功能" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="VerticalCenter">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button3" runat="server" Text="填寫" CommandName="Button3" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('確定？');" />
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

