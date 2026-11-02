<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_TB_EIP_SCH_WORK.ascx.cs" Inherits="CDS_WebPart_UC_TB_EIP_SCH_WORK" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<script>    


</script>


<table class="PopTable">
</table>

<telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
<telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
    <Tabs>
        <telerik:RadTab Text="校稿交付">
        </telerik:RadTab>
       <%-- <telerik:RadTab Text="業務交辨">
        </telerik:RadTab>--%>
        <telerik:RadTab Text="記錄">
        </telerik:RadTab>
    </Tabs>
</telerik:RadTabStrip>

<telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
    <telerik:RadPageView ID="RadPageView3" runat="server" Selected="true">
        <div id="tabs-3">
            <table class="PopTable">
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label1" runat="server" Text="是否結案:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td class="PopTableRightTD">
                        <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label2" runat="server" Text="校稿狀況:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td class="PopTableRightTD">
                        <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label3" runat="server" Text="校稿名稱:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td class="PopTableRightTD">
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label4" runat="server" Text="執行者:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td class="PopTableRightTD">
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label5" runat="server" Text="交付者:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td class="PopTableRightTD">
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="PopTableLeftTD"></td>
                    <td>
                        <asp:Button ID="Button2" runat="server" Text=" 查詢 " OnClick="btn5_Click"
                            meta:resourcekey="btn5Resource1" />
                    </td>

                </tr>
            </table>
            <table class="PopTable">
                <tr>
                    <td colspan="2" class="PopTableRightTD">
                        <div style="overflow-x: auto; width: 100%">
                            <Fast:Grid ID="Grid2" Style="overflow-x: auto; width: 100%" OnRowDataBound="Grid2_RowDataBound" runat="server" OnBeforeExport="OnBeforeExport2" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource2" OnPageIndexChanging="grid2_PageIndexChanging">
                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                <Columns>
                                    <asp:BoundField HeaderText="校稿名稱" DataField="SUBJECT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="目前的狀況" DataField="WORK_STATE_DESC" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="交付者" DataField="交付者" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="執行者" DataField="執行者" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="開始時間" DataField="CREATE_TIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="預計完成時間" DataField="END_TIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="最新回覆" DataField="DESCRIPTION" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                            </Fast:Grid>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </telerik:RadPageView>
  <%--  <telerik:RadPageView ID="RadPageView1" runat="server">
        <div id="tabs-1">
            <table class="PopTable">
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label2" runat="server" Text="日期:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td class="PopTableRightTD">
                        <telerik:RadDatePicker ID="txtDate1" runat="server" Width="160px"></telerik:RadDatePicker>

                        <asp:Label ID="Label11" runat="server" Text="~"></asp:Label>
                        <telerik:RadDatePicker ID="txtDate2" runat="server" Width="160px"></telerik:RadDatePicker>

                        <asp:Label ID="Label12" runat="server" Text=" "></asp:Label>
                        <asp:Button ID="Button1" runat="server" Text=" 查詢 "
                            OnClick="btn1_Click" meta:resourcekey="btn4Resource1" />
                    </td>
                </tr>
            </table>
            <table class="PopTable">
                <td colspan="2" class="PopTableRightTD">
                    <div style="overflow-x: auto; width: 100%">
                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid1_PageIndexChanging">
                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>

                            <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                            <Columns>
                                <asp:BoundField HeaderText="執行人" DataField="NAME1" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="交辨截止" DataField="END_TIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="交辨內容" DataField="SUBJECT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="交辨主" DataField="NAME2" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Fast:Grid>
                    </div>
                </td>
            </table>
        </div>
    </telerik:RadPageView>--%>
    <telerik:RadPageView ID="RadPageView2" runat="server">
        <div id="tabs-2">
        </div>
    </telerik:RadPageView>
</telerik:RadMultiPage>​




