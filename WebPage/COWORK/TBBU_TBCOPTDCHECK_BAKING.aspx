<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_TBCOPTDCHECK_BAKING.aspx.cs" Inherits="CDS_WebPage_COP_TBBU_TBCOPTDCHECK_BAKING" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../CSS/coptcheck.css?v=3" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0" AutoPostBack="True" OnTabClick="RadTabStrip2_TabClick">
                <Tabs>
                    <telerik:RadTab Text="訂單協調" PageViewID="RadPageView1"></telerik:RadTab>
                    <telerik:RadTab Text="生管用" PageViewID="RadPageView2"></telerik:RadTab>
                    <telerik:RadTab Text="採購用" PageViewID="RadPageView3" Visible="false"></telerik:RadTab>
                    <telerik:RadTab Text="業務用" PageViewID="RadPageView4"></telerik:RadTab>
                    <telerik:RadTab Text="不需生產庫存出貨" PageViewID="RadPageView5"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <div class="check-legend">
                <span><i class="legend-ready"></i>需生產，生管已核准</span>
                <span><i class="legend-wait"></i>需生產，等待生管核准</span>
                <span><i class="legend-free"></i>不需生產</span>
                <span class="legend-note">整張單需要生產的明細都經生管核准後，才會出現「送簽」按鈕</span>
            </div>

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
                                    <asp:Label ID="Label3" runat="server" Text="月份: "></asp:Label>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="是否確認: "></asp:Label>
                                    <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="是否生產: "></asp:Label>
                                    <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="訂單單號: "></asp:Label>
                                    <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label24" runat="server" Text="客戶名稱: "></asp:Label>
                                    <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label26" runat="server" Text="品名: "></asp:Label>
                                    <asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>
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

                                    <div class="grid-wrap">
                                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:BoundField HeaderText="客戶" DataField="MA002" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                <asp:BoundField HeaderText="單別" DataField="TD001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:BoundField HeaderText="單號" DataField="TD002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:BoundField HeaderText="序號" DataField="TD003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:TemplateField HeaderText="是否生產" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_COPTDUDF01" runat="server" Text='<%# Bind("COPTDUDF01") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="品號" DataField="TD004" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                <asp:BoundField HeaderText="品名" DataField="TD005" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                <asp:BoundField HeaderText="訂單數量" DataField="TD008" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                <asp:BoundField HeaderText="贈品量" DataField="TD024" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                <asp:BoundField HeaderText="已交數量" DataField="TD009" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                <asp:BoundField HeaderText="贈品已交" DataField="TD025" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                <asp:BoundField HeaderText="單位" DataField="TD010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:BoundField HeaderText="預交日" DataField="TD013" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                <asp:BoundField HeaderText="單頭備註" DataField="TC015" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:BoundField HeaderText="單身備註" DataField="TD020" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:TemplateField HeaderText="生管核準" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_MOCCHECKS" runat="server" Text='<%# Bind("MOCCHECKS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:TemplateField HeaderText="更新進度" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-edit" ID="Button1" runat="server" Text="更新" CommandName="Button1" CommandArgument='<%# Eval("TD123") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="是否送簽" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-send" ID="Button2" runat="server" Text="送簽" CommandName="Button2" CommandArgument='<%# Eval("TD12") %>' />
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
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="月份: "></asp:Label>
                                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList3" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="是否生產: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList4" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox18" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label28" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox19" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button3" runat="server" Text=" 查詢 " OnClick="Button3_Click"
                                            meta:resourcekey="btn5Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button4" runat="server" Text=" 生管批次更新 " OnClick="Button4_Click"
                                            meta:resourcekey="btn4Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid2" OnRowDataBound="Grid2_RowDataBound" OnRowCommand="Grid2_RowCommand" runat="server" OnBeforeExport="OnBeforeExport2" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource2" OnPageIndexChanging="grid_PageIndexChanging2">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="MA002" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TD001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TD002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="序號" DataField="TD003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="品號" DataField="TD004" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TD005" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="訂單數量" DataField="TD008" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="贈品量" DataField="TD024" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="已交數量" DataField="TD009" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="贈品已交" DataField="TD025" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TD010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TD013" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TC015" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身備註" DataField="TD020" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" Visible="false" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" Visible="false" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" Visible="false" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="生管備註填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GRIDVIEWTextBox1" runat="server" Text="" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="生管核準填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="GRIDVIEWDropDownList1" runat="server">
                                                                <asp:ListItem>N</asp:ListItem>
                                                                <asp:ListItem>Y</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView3" runat="server">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="月份: "></asp:Label>
                                        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList5" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="是否生產: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList6" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label19" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label29" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox20" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label30" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox21" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button5" runat="server" Text=" 查詢 " OnClick="Button5_Click"
                                            meta:resourcekey="btn5Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button6" runat="server" Text=" 採購批次更新 " OnClick="Button6_Click"
                                            meta:resourcekey="btn6Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid3" OnRowDataBound="Grid3_RowDataBound" OnRowCommand="Grid3_RowCommand" runat="server" OnBeforeExport="OnBeforeExport3" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource3" OnPageIndexChanging="grid_PageIndexChanging3">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="MA002" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TD001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TD002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="序號" DataField="TD003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="品號" DataField="TD004" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TD005" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="訂單數量" DataField="TD008" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="贈品量" DataField="TD024" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="已交數量" DataField="TD009" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="贈品已交" DataField="TD025" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TD010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TD013" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TC015" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身備註" DataField="TD020" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="採購備註填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GRIDVIEW2TextBox1" runat="server" Text="" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="採購核準填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="GRIDVIEW2DropDownList1" runat="server">
                                                                <asp:ListItem>N</asp:ListItem>
                                                                <asp:ListItem>Y</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView4" runat="server">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="月份: "></asp:Label>
                                        <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label15" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList7" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="是否生產: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList8" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label20" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label31" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox22" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label32" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox23" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button7" runat="server" Text=" 查詢 " OnClick="Button7_Click"
                                            meta:resourcekey="btn7Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button8" runat="server" Text=" 業務批次更新 " OnClick="Button8_Click"
                                            meta:resourcekey="btn8Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid4" OnRowDataBound="Grid4_RowDataBound" OnRowCommand="Grid4_RowCommand" runat="server" OnBeforeExport="OnBeforeExport4" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource4" OnPageIndexChanging="grid_PageIndexChanging4">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="MA002" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TD001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TD002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="序號" DataField="TD003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:TemplateField HeaderText="是否生產" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_COPTDUDF01" runat="server" Text='<%# Bind("COPTDUDF01") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="品號" DataField="TD004" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TD005" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="訂單數量" DataField="TD008" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="贈品量" DataField="TD024" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="已交數量" DataField="TD009" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="贈品已交" DataField="TD025" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TD010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TD013" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TC015" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身備註" DataField="TD020" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" Visible="false" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" Visible="false" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" Visible="false" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="業務備註填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GRIDVIEW4TextBox1" runat="server" Text="" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="是否送簽" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                        <ItemTemplate>
                                                            <asp:Button CssClass="btn-send" ID="GRIDVIEW4Button1" runat="server" Text="送簽" CommandName="GRIDVIEW4Button1" CommandArgument='<%# Eval("TD12") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView5" runat="server">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label21" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label22" runat="server" Text="月份: "></asp:Label>
                                        <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList9" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>

                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label25" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox15" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label33" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox24" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label34" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox25" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button9" runat="server" Text=" 查詢 " OnClick="Button9_Click"
                                            meta:resourcekey="btn9Resource1" />
                                    </td>

                                </tr>
                            </table>

                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid5" OnRowDataBound="Grid5_RowDataBound" OnRowCommand="Grid5_RowCommand" runat="server" OnBeforeExport="OnBeforeExport5" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource5" OnPageIndexChanging="grid_PageIndexChanging5">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="MA002" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TD001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TD002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="序號" DataField="TD003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="品號" DataField="TD004" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TD005" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="訂單數量" DataField="TD008" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="贈品量" DataField="TD024" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="已交數量" DataField="TD009" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="贈品已交" DataField="TD025" DataFormatString="{0:N2}" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TD010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TD013" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TC015" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身備註" DataField="TD020" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" Visible="false" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" Visible="false" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" Visible="false" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="是否送簽" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                        <ItemTemplate>
                                                            <asp:Button CssClass="btn-send" ID="GRIDVIEW5Button1" runat="server" Text="送簽" CommandName="GRIDVIEW5Button1" CommandArgument='<%# Eval("TD12") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

