<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TK_GIFTSETS.aspx.cs" Inherits="CDS_WebPage_MARKETING_TK_GIFTSETS" %>

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
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="是否結案: "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="名稱: "></asp:Label>
                                    <asp:TextBox ID="QUERY_TextBox1" Text="" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="年度: "></asp:Label>
                                    <asp:TextBox ID="QUERY_TextBox2" Text="" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="查詢" OnClick="Button1_Click"
                                        meta:resourcekey="btn1Resource1" />
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
                                                <asp:TemplateField HeaderText="品名" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_品名"
                                                            runat="server"
                                                            Text='<%# Bind("MB002") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="200px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="箱入數" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_箱入數"
                                                            runat="server"
                                                            Text='<%# Bind("MB003") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品號" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_品號"
                                                            runat="server"
                                                            Text='<%# Bind("MB001") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="140px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="年度" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_年度"
                                                            runat="server"
                                                            Text='<%# Bind("YEARS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="售價" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_售價"
                                                            runat="server"
                                                            Text='<%# Bind("PRICES") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IP價" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_IP價"
                                                            runat="server"
                                                            Text='<%# Bind("IPPRICES") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DM價" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_DM價"
                                                            runat="server"
                                                            Text='<%# Bind("DMPRICES") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="門市" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_門市"
                                                            runat="server"
                                                            Text='<%# Bind("STORENUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="電商" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_電商"
                                                            runat="server"
                                                            Text='<%# Bind("ECOMMERCENUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="觀光" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_觀光"
                                                            runat="server"
                                                            Text='<%# Bind("TOURISHOPNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="國內-國軍" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_國內國軍"
                                                            runat="server"
                                                            Text='<%# Bind("INARMYNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="國內-中油" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_國內中油"
                                                            runat="server"
                                                            Text='<%# Bind("INOILNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="國內-經銷" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_國內經銷"
                                                            runat="server"
                                                            Text='<%# Bind("INSALENUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="業務公關" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_業務公關"
                                                            runat="server"
                                                            Text='<%# Bind("INPRNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="總經理公關" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_總經理公關"
                                                            runat="server"
                                                            Text='<%# Bind("BOSSPRNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="員購" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_員購"
                                                            runat="server"
                                                            Text='<%# Bind("STAFFNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="加總" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_加總"
                                                            runat="server"
                                                            Text='<%# Bind("TOTALNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="預估包材下單總量" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_預估包材下單總量"
                                                            runat="server"
                                                            Text='<%# Bind("PACKNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="預估包材到廠日" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_預估包材到廠日"
                                                            runat="server"
                                                            Text='<%# Bind("PACKINDATES") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="預估成品完成日" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_預估成品完成日"
                                                            runat="server"
                                                            Text='<%# Bind("PRODATES") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="銷售日期起" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_銷售日期起"
                                                            runat="server"
                                                            Text='<%# Bind("SDATES") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="銷售日期迄" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_銷售日期迄"
                                                            runat="server"
                                                            Text='<%# Bind("EDATES") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="日期區間內的銷售數量" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_日期區間內的銷售數量"
                                                            runat="server"
                                                            Text='<%# Bind("SELLEDNUMS") %>'
                                                            Style="word-break: break-all; white-space: pre-line;"
                                                            Width="100px">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DropDownList_是否結案"
                                                            runat="server"
                                                            SelectedValue='<%# Bind("ISCLOSED") %>'
                                                            Width="100px">

                                                            <%-- 設定下拉選單的選項 --%>
                                                            <asp:ListItem Text="N" Value="N" />
                                                            <asp:ListItem Text="Y" Value="Y" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnUpdate" runat="server" Text="更新" CommandName="UPDATE"
                                                            CommandArgument='<%# Eval("ID") %>'
                                                            OnClientClick="return confirm('是否更新？');"
                                                            CssClass="btn btn-danger btn-sm" />
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
                        <div>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"></asp:Label>
                            <hr />
                            <table class="PopTable">
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="品名：" runat="server" />
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
                                        <asp:TextBox ID="ADD_TextBox19" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="年度：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox20" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="箱入數：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox2" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="售價：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox3" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="IP價：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox4" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="DM價：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox5" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="門市：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox6" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="電商：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox7" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="觀光：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox8" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="國內-國軍：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox9" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="國內-中油：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox10" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="國內-經銷：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox11" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="業務公關：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox12" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="總經理公關：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox13" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="員購：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox14" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="加總：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox15" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="預估包材下單總量：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox16" runat="server" TextMode="Number" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="預估包材到廠日：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox17" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="預估成品完成日：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox18" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="銷售日期起：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox21" runat="server" TextMode="Date" Width="200px" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="銷售日期迄：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="ADD_TextBox22" runat="server" TextMode="Date" Width="200px" />
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
                                                Style="font-size: 24px; padding: 10px 20px;" />
                                            <%-- 📌 將字體放大到 24px --%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
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

