<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TK_AD_MANAGE.aspx.cs" Inherits="CDS_WebPage_MARKETING_TK_AD_MANAGE" %>

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
                height: auto;      /* 保持圖片的寬高比 */
                width: auto;       /* 保持圖片的寬高比 */
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
        <ContentTemplate>
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="資料">
                    </telerik:RadTab>
                    <telerik:RadTab Text="上傳">
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
                                    <asp:Label ID="Label1" runat="server" Text="年度: "></asp:Label>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="主題: "></asp:Label>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="描述: "></asp:Label>
                                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClick="Button1_Click"
                                        meta:resourcekey="btn1Resource1" />
                                </td>

                            </tr>
                        </table>
                        <table class="PopTable">
                            <tr>
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="年度" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_YEARS" runat="server" Text='<%# Bind("YEARS") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="主題" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_SUBJECTS" runat="server" Text='<%# Bind("SUBJECTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="描述" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_DESCRIPTIONS" runat="server" Text='<%# Bind("DESCRIPTIONS") %>' Style="word-break: break-all; white-space: pre-line;" Width="300px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="附件預覽/下載" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFileDisplay" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnDelete" runat="server" Text="刪除" CommandName="DeleteRecord"
                                                            CommandArgument='<%# Eval("ID") %>'
                                                            OnClientClick="return confirm('是否確認刪除此記錄和附件？');"
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
                        <h3>檔案上傳與記錄 (TK_MARKETING_AD_MANAGE)</h3>
                        <div>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"></asp:Label>
                            <hr />

                            <table style="width: 100%; border-collapse: collapse;">
                                <tr>
                                    <td style="width: 15%; padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="年度 (YEARS)：" runat="server" />
                                    </td>
                                    <td style="width: 85%; padding: 5px;">
                                        <asp:TextBox ID="txtYear" runat="server" TextMode="Number" Width="400px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="主題 (SUBJECTS)：" runat="server" />
                                    </td>
                                    <td style="padding: 5px;">
                                        <asp:TextBox ID="txtSubject" runat="server" TextMode="MultiLine"
                                            Rows="3"
                                            Width="500px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="描述 (DESCRIPTIONS)：" runat="server" />
                                    </td>
                                    <td style="padding: 5px;">
                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                                            Rows="5"
                                            Width="500px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 5px; font-weight: bold; vertical-align: top;">
                                        <asp:Label Text="選擇檔案：" runat="server" />
                                    </td>
                                    <td style="padding: 5px;">
                                        <asp:FileUpload ID="fileUploader" runat="server" />
                                    </td>
                                </tr>
                            </table>

                            <div style="padding: 10px 5px;">
                                <asp:Button ID="btnUpload" runat="server" Text="上傳並記錄" OnClick="btnUpload_Click" />
                            </div>
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
