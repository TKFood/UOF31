<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TB_PRODUCT_CLASS.aspx.cs" Inherits="CDS_WebPage_RESEARCH_TB_PRODUCT_CLASS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>    


</script>
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
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="PopTable">
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="類別/名稱: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="PopTableLeftTD"></td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="查詢 " OnClick="Button1_Click"
                            meta:resourcekey="btn1Resource1" />
                    </td>
                </tr>
            </table>
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="資料">
                    </telerik:RadTab>
                    <telerik:RadTab Text="修改">
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
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="類別" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_類別" runat="server" Text='<%# Bind("類別") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="產品" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_產品" runat="server" Text='<%# Bind("產品") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="成本結構(半成品單位-片/個)" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_成本結構" runat="server" Text='<%# Bind("成本結構") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="效期評估-市場" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_效期評估市場" runat="server" Text='<%# Bind("效期評估市場") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="效期評估-生產" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_效期評估生產" runat="server" Text='<%# Bind("效期評估生產") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="最小批量" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_最小批量" runat="server" Text='<%# Bind("最小批量") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="日產量" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_日產量" runat="server" Text='<%# Bind("日產量") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="關鍵原料" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_關鍵原料" runat="server" Text='<%# Bind("關鍵原料") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="關鍵製程" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_關鍵製程" runat="server" Text='<%# Bind("關鍵製程") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="素別" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_素別" runat="server" Text='<%# Bind("素別") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="建立日" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1_建立日" runat="server" Text='<%# Bind("CREATEDATES") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="更新日" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1_更新日" runat="server" Text='<%# Bind("UPDATEDATES") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
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
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid2" OnRowDataBound="Grid2_RowDataBound" OnRowCommand="Grid2_RowCommand" runat="server" OnBeforeExport="OnBeforeExport2" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource2" OnPageIndexChanging="grid_PageIndexChanging2">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="類別" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_類別" runat="server" Text='<%# Bind("類別") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="產品" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_產品" runat="server" Text='<%# Bind("產品") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="成本結構" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_成本結構" runat="server" Text='<%# Bind("成本結構") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="效期評估市場" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_效期評估市場" runat="server" Text='<%# Bind("效期評估市場") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="效期評估生產" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_效期評估生產" runat="server" Text='<%# Bind("效期評估生產") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="最小批量" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_最小批量" runat="server" Text='<%# Bind("最小批量") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="日產量" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_日產量" runat="server" Text='<%# Bind("日產量") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="5" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="關鍵原料" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_關鍵原料" runat="server" Text='<%# Bind("關鍵原料") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="5" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="關鍵製程" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_關鍵製程" runat="server" Text='<%# Bind("關鍵製程") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="5" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="素別" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_素別" runat="server" Text='<%# Bind("素別") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="5" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="建立日" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1_建立日" runat="server" Text='<%# Bind("CREATEDATES") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="更新日" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1_更新日" runat="server" Text='<%# Bind("UPDATEDATES") %>' Style="word-break: break-all; white-space: pre-line; width: 100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_ID" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="更新" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button2" runat="server" Text="更新" CommandName="Button2" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('確定？');" />
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
                <telerik:RadPageView ID="RadPageView3" runat="server">
                    <div id="tabs-3">
                        <table class="PopTable">
                            <td colspan="2" class="PopTableRightTD">
                                <div style="overflow-x: auto; width: 100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="類別"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_類別" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="產品"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_產品" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="成本結構"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_成本結構" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="效期評估市場"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_效期評估市場" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="效期評估生產"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_效期評估生產" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="最小批量"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_最小批量" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="日產量"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_日產量" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="1" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Text="關鍵原料"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_關鍵原料" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="5" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="關鍵製程"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_關鍵製程" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="5" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="素別"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="NEW_素別" runat="server" Text="" Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="5" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button3" runat="server" Text="新增專案" CssClass="big-bold-button" OnClick="Button3_Click" />
                                        </td>
                                    </tr>
                                </div>
                            </td>

                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView99" runat="server">
                    <div id="tabs-99">
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

