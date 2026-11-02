<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_PRODUCTS.aspx.cs" Inherits="CDS_WebPage_COP_TBBU_PRODUCTS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>    
        function btn4_Click(sender) {
            //從前端開始視窗
            //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
            //OpenDialogResult為關閉視後會執行的JS Function
            //參數使用JSON格式傳遞
            $uof.dialog.open2("~/CDS/WebPage/COP/TBBU_PRODUCTSDialogADD.aspx", sender, "", 800, 600, OpenDialogResult, {});

            return false;

        }

        function btn5_Click(sender) {
            //從前端開始視窗
            //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
            //OpenDialogResult為關閉視後會執行的JS Function
            //參數使用JSON格式傳遞
            $uof.dialog.open2("~/CDS/WebPage/COP/TBBU_PRODUCTSDialogADDPRODUCTS_OTHERS.aspx", sender, "", 800, 600, OpenDialogResult, {});

            return false;

        }

        //如果有設定回傳值則執行sender Event
        function OpenDialogResult(returnValue) {
            if (typeof (returnValue) == "REFRESH")
                return false;
            else
                return true;
        }



    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="商品資料整理">
                    </telerik:RadTab>
                    <telerik:RadTab Text="新增老楊的商品資料">
                    </telerik:RadTab>
                    <telerik:RadTab Text="新增非老楊的商品資料">
                    </telerik:RadTab>
                    <telerik:RadTab Text="老楊新品資料整理">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                    <div id="tabs-1">
                        <table class="PopTable">
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label14" runat="server" Text="公司:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label2" runat="server" Text="銷售通路:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label12" runat="server" Text="品號/品名:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label3" runat="server" Text="建議售價:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label4" runat="server" Text="~" meta:resourcekey="Label4Resource1"></asp:Label>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label5" runat="server" Text="IP價:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label6" runat="server" Text="~" meta:resourcekey="Label4Resource1"></asp:Label>
                                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label7" runat="server" Text="DM價:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label8" runat="server" Text="~" meta:resourcekey="Label4Resource1"></asp:Label>
                                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label9" runat="server" Text="口味:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label10" runat="server" Text="效期:(限數字)" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label11" runat="server" Text="銷售重點:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                                </td>
                                                         
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClick="btn1_Click"
                                        meta:resourcekey="btn5Resource1" />
                                </td>

                            </tr>
                        </table>
                        <table class="PopTable">
                            <tr>
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid1" Style="overflow-x: auto; width: 100%" OnRowDataBound="Grid1_RowDataBound" runat="server" OnRowCommand="Grid1_RowCommand" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource1" OnPageIndexChanging="grid1_PageIndexChanging">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="公司" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="公司" runat="server" Text='<%# Bind("COMPANYS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="品號" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="品號" runat="server" Text='<%# Bind("MB001") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品名" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="品名" runat="server" Text='<%# Bind("MB002") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="規格" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="規格" runat="server" Text='<%# Bind("MB003") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="單位" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="單位" runat="server" Text='<%# Bind("MB004") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="口味" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="口味" runat="server" Text='<%# Bind("MA003") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="箱入數" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="箱入數" runat="server" Text='<%# Bind("MD007") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="長*寬*高" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="長寬高" runat="server" Text='<%# Bind("MB093094095") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="有效期限" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="有效期限" runat="server" Text='<%# Bind("VALIDITYPERIOD") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="建議售價" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="建議售價" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRICES1", "{0:N0}") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IP價" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IP價" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRICES2", "{0:N0}") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DM價" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DM價" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRICES3", "{0:N0}") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="條碼" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="條碼" runat="server" Text='<%# Bind("MB013") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="銷售重點" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="銷售重點" runat="server" Text='<%# Bind("PRODUCTSFEATURES") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="銷售通路" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="銷售通路" runat="server" Text='<%# Bind("SALESFOCUS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="實際MOQ" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="實際MOQ" runat="server" Text='<%# Bind("MOQS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="照片" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image1" runat="server" HorizontalAlign="Center" Length="100px" Width="100px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="BTN" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="GVButton1" runat="server" CommandName="GVButton1" Text="修改" ForeColor="Red" CommandArgument='<%# Eval("MB001") %>' />
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
                                    <asp:Label ID="Label1" runat="server" Text="新增資料" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:Button ID="btn4" runat="server" Text="新增資料" ForeColor="red" OnClientClick="return btn4_Click(this)"  onclick="btn4_Click" meta:resourcekey="btn4Resource1" />

                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView3" runat="server">
                    <div id="tabs-3">
                        <table class="PopTable">
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label13" runat="server" Text="新增資料" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:Button ID="btn5" runat="server" Text="新增資料" ForeColor="red" OnClientClick="return btn5_Click(this)" onclick="btn5_Click" meta:resourcekey="btn5Resource1" />

                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPageView>
                  <telerik:RadPageView ID="RadPageView4" runat="server" Selected="false">
                    <div id="tabs-4">
                        <table class="PopTable">
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label17" runat="server" Text="公司:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:DropDownList ID="DropDownList3" runat="server"></asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label18" runat="server" Text="銷售通路:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td class="PopTableRightTD">
                                    <asp:DropDownList ID="DropDownList4" runat="server"></asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label19" runat="server" Text="品號/品名:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label20" runat="server" Text="建議售價:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label21" runat="server" Text="~" meta:resourcekey="Label4Resource1"></asp:Label>
                                    <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label22" runat="server" Text="IP價:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox15" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label23" runat="server" Text="~" meta:resourcekey="Label4Resource1"></asp:Label>
                                    <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label24" runat="server" Text="DM價:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label25" runat="server" Text="~" meta:resourcekey="Label4Resource1"></asp:Label>
                                    <asp:TextBox ID="TextBox18" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label26" runat="server" Text="口味:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox19" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label27" runat="server" Text="效期:(限數字)" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox20" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label28" runat="server" Text="銷售重點:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox21" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                                 <tr>
                                <td class="PopTableLeftTD">
                                    <asp:Label ID="Label29" runat="server" Text="新品建立日期:" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox22" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label30" runat="server" Text="(之後)" meta:resourcekey="Label4Resource1"></asp:Label>
                                     
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button3" runat="server" Text=" 只查老楊新品 " OnClick="btn3_Click"
                                        meta:resourcekey="btn3Resource1" />
                                </td>

                            </tr>
                        </table>
                        <table class="PopTable">
                            <tr>
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid2" Style="overflow-x: auto; width: 100%" OnRowDataBound="Grid2_RowDataBound" runat="server" OnRowCommand="Grid2_RowCommand" OnBeforeExport="OnBeforeExport2" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource2" OnPageIndexChanging="grid2_PageIndexChanging">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="公司" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="公司" runat="server" Text='<%# Bind("COMPANYS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="品號" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="品號" runat="server" Text='<%# Bind("MB001") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="品名" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="品名" runat="server" Text='<%# Bind("MB002") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="規格" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="規格" runat="server" Text='<%# Bind("MB003") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="單位" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="單位" runat="server" Text='<%# Bind("MB004") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="口味" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="口味" runat="server" Text='<%# Bind("MA003") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="箱入數" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="箱入數" runat="server" Text='<%# Bind("MD007") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="長*寬*高" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="長寬高" runat="server" Text='<%# Bind("MB093094095") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="有效期限" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="有效期限" runat="server" Text='<%# Bind("VALIDITYPERIOD") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="建議售價" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="建議售價" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRICES1", "{0:N0}") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IP價" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IP價" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRICES2", "{0:N0}") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DM價" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DM價" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRICES3", "{0:N0}") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="條碼" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="條碼" runat="server" Text='<%# Bind("MB013") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="銷售重點" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="銷售重點" runat="server" Text='<%# Bind("PRODUCTSFEATURES") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="銷售通路" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="銷售通路" runat="server" Text='<%# Bind("SALESFOCUS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="實際MOQ" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="實際MOQ" runat="server" Text='<%# Bind("MOQS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="照片" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image1" runat="server" HorizontalAlign="Center" Length="100px" Width="100px" />
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
            </telerik:RadMultiPage>​
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

