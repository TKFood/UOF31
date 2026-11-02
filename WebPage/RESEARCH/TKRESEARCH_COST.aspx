<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCH_COST.aspx.cs" Inherits="CDS_WebPage_RESEARCH_TKRESEARCH_COST" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>    
        function btn4_Click(sender) {
            //從前端開始視窗
            //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
            //OpenDialogResult為關閉視後會執行的JS Function
            //參數使用JSON格式傳遞
            //$uof.dialog.open2("~/CDS/WebPage/COP/TBBU_PRODUCTSDialogADD.aspx", sender, "", 800, 600, OpenDialogResult, {});

            //return false;

        }

        //如果有設定回傳值則執行sender Event
        function OpenDialogResult(returnValue) {
            if (typeof (returnValue) == "undefined")
                return false;
            else
                return true;
        }

        function myFunction() {
            //document.getElementById("demo").innerHTML = "Hello World";

        }

        $(document).ready(function () {
            $('.header').click(function () {
                $('.content').slideToggle();
            });

            // 預設隱藏content區塊
            $('.content').hide();
        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    </script>



    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
        <Tabs>
            <telerik:RadTab Text="每年平均成本-有製令的">
            </telerik:RadTab>
            <telerik:RadTab Text="分攤的每年平均成本-有製令的">
            </telerik:RadTab>
            <telerik:RadTab Text="依BOM查詢平均成本-無製令的">
            </telerik:RadTab>
            <telerik:RadTab Text="人工記錄的成本">
            </telerik:RadTab>
            <telerik:RadTab Text="空白">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
            <div id="tabs-1">
                <table class="PopTable">
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label2" runat="server" Text="年度:" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:TextBox ID="txtDate1" runat="server" Width="100px"></asp:TextBox>

                        </td>
                    </tr>

                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label3" runat="server" Text="品號:" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label1" runat="server" Text="品名:" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                            <p id="demo"></p>
                            <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClick="btn1_Click"
                                meta:resourcekey="btn1Resource1" />
                        </td>

                    </tr>


                    <%--<tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                             <button onclick="myFunction()">Click me</button>
                        </td>

                    </tr>--%>
                </table>
                <table class="PopTable">
                    <tr>
                        <td colspan="2" class="PopTableRightTD">
                            <div style="overflow-x: auto; width: 100%">
                                <Fast:Grid ID="Grid1" Style="overflow-x: auto; width: 100%" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource1" OnPageIndexChanging="grid1_PageIndexChanging">
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                    <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                    <Columns>
                                        <asp:TemplateField HeaderText="年月" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <asp:Label ID="年月" runat="server" Text='<%# Eval("年月") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="品號" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                            <ItemTemplate>
                                                <asp:Label ID="品號" runat="server" Text='<%# Eval("品號") %>' Style="word-break: break-all; white-space: pre-line;" Width="140px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="品名" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                            <ItemTemplate>
                                                <asp:Label ID="品名" runat="server" Text='<%# Eval("品名") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="規格" ItemStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                            <ItemTemplate>
                                                <asp:Label ID="規格" runat="server" Text='<%# Eval("規格") %>' Style="word-break: break-all; white-space: pre-line;" Width="160px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="單位" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="單位" runat="server" Text='<%# Eval("單位") %>' Style="word-break: break-all; white-space: pre-line;" Width="40px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="標準售價" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="標準售價" runat="server" Text='<%# Eval("MB047", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="單位材料成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="單位材料成本" runat="server" Text='<%# Eval("單位材料成本", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="單位人工成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="單位人工成本" runat="server" Text='<%# Eval("單位人工成本", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="單位製造成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="單位製造成本" runat="server" Text='<%# Eval("單位製造成本", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="單位加工成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="單位加工成本" runat="server" Text='<%# Eval("單位加工成本", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="單位成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="單位成本" runat="server" Text='<%# Eval("單位成本", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="生產入庫數" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="生產入庫數" runat="server" Text='<%# Eval("生產入庫數", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="託外入庫數" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="託外入庫數" runat="server" Text='<%# Eval("託外入庫數", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="人工工時" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="人工工時" runat="server" Text='<%# Eval("SUMPROTIMES", "{0:N2}") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
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
            <table class="PopTable">
                <tr>
                    <td colspan="2" class="PopTableRightTD">
                        <div style="overflow-x: auto; width: 100%">
                            <Fast:Grid ID="Grid2" Style="overflow-x: auto; width: 100%" OnRowDataBound="Grid2_RowDataBound" OnRowCommand="Grid2_RowCommand" runat="server" OnBeforeExport="OnBeforeExport2" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource2" OnPageIndexChanging="grid2_PageIndexChanging">
                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                <Columns>
                                    <asp:BoundField HeaderText="年度" DataField="年度" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="成品品號" DataField="成品品號" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="成品品名" DataField="成品品名" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="使用品號" DataField="使用品號" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="使用品名" DataField="使用品名" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="成品規格" DataField="成品規格" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="成品單位" DataField="成品單位" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="分類" DataField="分類" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="分攤成本(原料/物料是取最近進價，跟平均不同)" DataField="分攤成本" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="各百分比" DataField="各百分比" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>



                                </Columns>
                            </Fast:Grid>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>

        <telerik:RadPageView ID="RadPageView3" runat="server">
            <table class="PopTable">
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label15" runat="server" Text="年度:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td class="PopTableRightTD">
                        <asp:TextBox ID="txtDate2" runat="server" Width="100px"></asp:TextBox>

                    </td>
                </tr>

                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label5" runat="server" Text="品號/品名:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                    </td>

                </tr>

                <tr>
                    <td class="PopTableLeftTD"></td>
                    <td>
                        <p id="demo"></p>
                        <asp:Button ID="Button2" runat="server" Text=" 查詢 " OnClick="btn2_Click"
                            meta:resourcekey="btn2Resource1" />
                    </td>

                </tr>


                <%--<tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                             <button onclick="myFunction()">Click me</button>
                        </td>

                    </tr>--%>
            </table>
            <table class="PopTable">
                <tr>
                    <td colspan="2" class="PopTableRightTD">
                        <div style="overflow-x: auto; width: 100%">
                            <Fast:Grid ID="Grid3" Style="overflow-x: auto; width: 100%" OnRowDataBound="Grid3_RowDataBound" OnRowCommand="Grid3_RowCommand" runat="server" OnBeforeExport="OnBeforeExport3" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource3" OnPageIndexChanging="grid3_PageIndexChanging">
                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                <Columns>
                                    <asp:BoundField HeaderText="成品品號" DataField="成品品號" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="成品品名" DataField="成品品名" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="200px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="單位成本-材料" DataField="單位成本-材料" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="組件品號" DataField="組件品號" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="組件品名" DataField="組件品名" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="200px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="分攤單位進貨成本" DataField="分攤單位進貨成本" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="非採購單位成本" DataField="非採購單位成本" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="組件單位" DataField="組件單位" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="最近進價" DataField="最近進價" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="進價是否含稅" DataField="進價是否含稅" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="進貨日期+廠商" DataField="MA002" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="200px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="標準批量" DataField="標準批量" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="組成用量" DataField="組成用量" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="底數" DataField="底數" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="損秏率" DataField="損秏率" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>






                                </Columns>
                            </Fast:Grid>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>

        <telerik:RadPageView ID="RadPageView4" runat="server">
            <table class="PopTable">
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label4" runat="server" Text="品名:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label6" runat="server" Text="是否結案:" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td class="PopTableRightTD">
                        <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="PopTableLeftTD"></td>
                    <td>
                        <p id="demo"></p>
                        <asp:Button ID="Button3" runat="server" Text=" 查詢 " OnClick="btn3_Click"
                            meta:resourcekey="btn3Resource1" />
                    </td>

                </tr>
                <tr>
                    <td class="PopTableLeftTD"></td>
                    <td>
                        <div class="container">
                            <div class="header">
                                <span>點擊這裡新增資料</span>
                            </div>
                            <div class="content" style="display: none;">
                                <table>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="品名"></asp:Label>
                                            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Label ID="Label13" runat="server" Text="規格"></asp:Label>
                                            <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="單位原料成本"></asp:Label>
                                            <asp:TextBox ID="TextBox7" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="單位物料成本"></asp:Label>
                                            <asp:TextBox ID="TextBox8" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="單位人工成本"></asp:Label>
                                            <asp:TextBox ID="TextBox9" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="單位製造成本"></asp:Label>
                                            <asp:TextBox ID="TextBox10" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Text="單位加工成本"></asp:Label>
                                            <asp:TextBox ID="TextBox11" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text="備註"></asp:Label>
                                            <asp:TextBox ID="TextBox12" runat="server" TextMode="MultiLine" Rows="5" Columns="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 10px;">
                                        <td>
                                            <asp:Button ID="Button4" runat="server" Text="儲存" OnClick="btn4_Click" Style="width: 100px; height: 50px; font-size: 24px !important; background-color: darkseagreen; color: white;" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                    </td>
                </tr>

                <%--<tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                             <button onclick="myFunction()">Click me</button>
                        </td>

                    </tr>--%>
            </table>
            <table class="PopTable">
                <tr>
                    <td colspan="2" class="PopTableRightTD">
                        <div style="overflow-x: auto; width: 100%">
                            <Fast:Grid ID="Grid4" Style="overflow-x: auto; width: 100%" OnRowDataBound="Grid4_RowDataBound" OnRowCommand="Grid4_RowCommand" runat="server" OnBeforeExport="OnBeforeExport4" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource4" OnPageIndexChanging="grid4_PageIndexChanging">
                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                <Columns>
                                    <asp:BoundField HeaderText="品號" DataField="品號" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="品名" DataField="品名" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="200px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="規格" DataField="規格" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="40px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="單位原料成本" DataField="單位原料成本" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="原料" ItemStyle-Width="25%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("TBCOSTRECORDSROWS").ToString().Replace("\n", "<br>") %>'></asp:Label>
                                            <itemstyle horizontalalign="Left" width="25%" wrap="true"></itemstyle>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="單位物料成本" DataField="單位物料成本" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="物料" ItemStyle-Width="25%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("TBCOSTRECORDSMAT").ToString().Replace("\n", "<br>") %>'></asp:Label>
                                            <itemstyle horizontalalign="Left" width="25%" wrap="true"></itemstyle>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="單位人工成本" DataField="單位人工成本" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="單位製造成本" DataField="單位製造成本" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="單位加工成本" DataField="單位加工成本" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="單位成本" DataField="單位成本" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="備註" DataField="備註" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="200px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="是否結案" DataField="是否結案" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="left" Width="10px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="修改明細" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="GV4Button1" runat="server" Text="修改" ForeColor="Red" CommandName="GV4Button1" CommandArgument='<%# Eval("品號") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="修改原料" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="GV4Button2" runat="server" Text="修改原料" ForeColor="Red" CommandName="GV4Button2" CommandArgument='<%# Eval("品號") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="修改物料" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="GV4Button3" runat="server" Text="修改物料" ForeColor="Red" CommandName="GV4Button3" CommandArgument='<%# Eval("品號") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </Fast:Grid>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView5" runat="server">
        </telerik:RadPageView>
    </telerik:RadMultiPage>​
</asp:Content>

