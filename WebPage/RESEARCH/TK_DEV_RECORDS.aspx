<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TK_DEV_RECORDS.aspx.cs" Inherits="CDS_WebPage_RESEARCH_TK_DEV_RECORDS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>  

</script>


    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage">
        <Tabs>
            <telerik:RadTab Text="專案" PageViewID="RadPageView1">
            </telerik:RadTab>
            <telerik:RadTab Text="專案回覆明細" PageViewID="RadPageView2">
            </telerik:RadTab>
            <telerik:RadTab Text="資料" PageViewID="RadPageView3">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" PageViewID="0">
            <div id="tabs-1">
                <table class="PopTable">
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="專案名稱: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_PROJECTNAMES" runat="server"></asp:TextBox>
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
                            <asp:Label ID="Label6" runat="server" Text="執行部門: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_EXEUNITS" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

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
                                        <asp:TemplateField HeaderText="立案單號" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="立案單號" runat="server" Text='<%# Bind("NO") %>' Style="word-break: break-all; white-space: pre-line" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="專案名稱" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="專案名稱" runat="server" Text='<%# Bind("PROJECTNAMES") %>' Style="word-break: break-all; white-space: pre-line" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="專案預計結案日期" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="專案預計結案日期" runat="server" Text='<%# Bind("PROJECTSDEADLINEDATES") %>' Style="word-break: break-all; white-space: pre-line" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="目前執行部門" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="目前執行部門" runat="server" Text='<%# Bind("EXEUNITS") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆期限" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="回覆期限" runat="server" Text='<%# Bind("EXEDEADLINEDATES") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="回覆" runat="server" Text='<%# Bind("COMMENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆日期(最近)" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("COMMENTSADDDATES") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="是否結案" runat="server" Text='<%# Bind("ISCLOSE") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
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
        <telerik:RadPageView ID="RadPageView2" runat="server" Selected="false" PageViewID="2">
            <div id="tabs-2">
                <table class="PopTable">
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="專案名稱: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_PROJECTNAMES_2" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="是否結案: "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListISCLOSE2" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="查詢 " OnClick="btn2_Click" meta:resourcekey="btn2_Resource1" />
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
                                        <asp:TemplateField HeaderText="立案單號" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="立案單號" runat="server" Text='<%# Bind("NO") %>' Style="word-break: break-all; white-space: pre-line" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="專案名稱" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="專案名稱" runat="server" Text='<%# Bind("PROJECTNAMES") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="專案預計結案日期" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="專案預計結案日期" runat="server" Text='<%# Bind("PROJECTSDEADLINEDATES") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="目前執行部門" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="目前執行部門" runat="server" Text='<%# Bind("EXEUNITS") %>' Style="word-break: break-all; white-space: pre-line" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆期限" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="回覆期限" runat="server" Text='<%# Bind("EXEDEADLINEDATES") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="回覆" runat="server" Text='<%# Bind("COMMENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆日期" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("COMMENTSADDDATES") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="回覆人" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="回覆人" runat="server" Text='<%# Bind("COMMENTSNAMES") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="是否結案" runat="server" Text='<%# Bind("ISCLOSE") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
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
        <telerik:RadPageView ID="RadPageView3" runat="server" Selected="false" PageViewID="3">
            <div id="tabs-3">
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>

