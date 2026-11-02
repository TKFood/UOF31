<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TK_UOF_RESEARCH_1002.aspx.cs" Inherits="CDS_WebPage_RESEARCH_TK_UOF_RESEARCH_1002" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="1002.設計需求內容清單 明細">
                    </telerik:RadTab>
                    <telerik:RadTab Text="資料">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label1" runat="server" Text="關鍵字:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label2" runat="server" Text="是否結案:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                                </td>

                            </tr>

                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClick="btn1_Click"
                                        meta:resourcekey="btn1Resource1" />
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button2" runat="server" Text="轉入新資料" OnClick="btn2_Click"
                                        meta:resourcekey="btn2Resource1" />
                                </td>

                            </tr>
                        </table>
                        <table class="PopTable">
                            <tr>
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid1" Style="overflow-x: auto; width: 100%" OnRowDataBound="Grid1_RowDataBound" OnRowUpdating="Grid1_OnRowUpdating" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource1" OnPageIndexChanging="grid1_PageIndexChanging">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="表單編號" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="FIELDS1" runat="server" Text='<%# Eval("表單編號") %>' Width="120px"  ></asp:TextBox>--%>
                                                        <asp:Label ID="LabelRDF1002SN" runat="server" Text='<%# Eval("表單編號") %>' Width="120px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="申請人" DataField="申請人" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="預計設計須完成日(需求單位填寫)" DataField="預計設計須完成日(需求單位填寫)" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="預計設計上校稿日(行銷單位填寫)" DataField="預計設計上校稿日(行銷單位填寫)" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="設計別" DataField="設計別" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="需求部門" DataField="需求部門" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="產品名稱" DataField="產品名稱" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Left" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="產品規格" DataField="產品規格" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="預計出貨日期" DataField="預計出貨日期" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="預計上市日期" DataField="預計上市日期" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="預計銷售通路/國家別" DataField="預計銷售通路/國家別" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="預估量（最小單位）" DataField="預估量（最小單位）" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="商品屬性" DataField="商品屬性" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="產品包裝形式" DataField="產品包裝形式" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="設計需求具體內容" DataField="設計需求具體內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                                                </asp:BoundField>

                                                <asp:BoundField HeaderText="處理進度" DataField="處理進度" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="是否結案" DataField="是否結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="填寫處理進度" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="GRIDVIEWTextBox1" runat="server" Text="" Width="200px" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="填寫是否結案" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="GRIDVIEWDropDownList1" runat="server" Width="60px">
                                                            <asp:ListItem>N</asp:ListItem>
                                                            <asp:ListItem>Y</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="更新" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button1" runat="server" Text="更新" ForeColor="Red" CommandName="Button1" CommandArgument='<%# Container.DataItemIndex %>' />

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
                    <div id="tabs-3">
                    </div>
                </telerik:RadPageView>

            </telerik:RadMultiPage>​  
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

