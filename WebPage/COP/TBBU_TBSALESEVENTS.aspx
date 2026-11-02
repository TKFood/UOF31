<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_TBSALESEVENTS.aspx.cs" Inherits="CDS_WebPage_TBBU_TBSALESEVENTS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>    
        function btn4_Click(sender) {
            //從前端開始視窗
            //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
            //OpenDialogResult為關閉視後會執行的JS Function
            //參數使用JSON格式傳遞
            $uof.dialog.open2("~/CDS/WebPage/COP/TBBU_TBSALESEVENTSDialogADD.aspx", sender, "", 800, 600, OpenDialogResult, {});

            return false;

        }

        //如果有設定回傳值則執行sender Event
        function OpenDialogResult(returnValue) {
            if (typeof (returnValue) == "undefined")
                return false;
            else
                return true;
        }

        function words_deal() {
            var curLength = $("#TextBox1").val().length;
            if (curLength > 4) {
                var num = $("#TextBox1").val().substr(0, 4);
                $("#TextBox1").val(num);
                alert("超過字數限制，多出的字將被移除！");
            } else {
                $("#TextBox1").text(4 - $("#TextArea1").val().length);
            }
        }

        function ImageClick(img) {
            //alert(img);
            var clientId = '#' + img.id

            var widthString = $(clientId).css("width");
            var heightString = $(clientId).css("height");
            var widthUnit = widthString.replace("px", "");
            var heightUnit = heightString.replace("px", "");
            var width = parseInt(widthUnit, 10);
            var height = parseInt(heightUnit, 10);



            if ($(clientId).hasClass("BigImage")) {
                width = (width / 10);
                height = (height / 10);
                $(clientId).css("width", width + "px");
                $(clientId).css("height", height + "px");
                $(clientId).removeClass("BigImage");
            }
            else {
                width = (width * 10);
                height = (height * 10);
                $(clientId).css("width", width + "px");
                $(clientId).css("height", height + "px");
                $(clientId).addClass("BigImage");
            }

        }
    </script>

    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
        <Tabs>
            <telerik:RadTab Text="明細資料整理">
            </telerik:RadTab>
            <telerik:RadTab Text="新增資料">
            </telerik:RadTab>
            <telerik:RadTab Text="客情資料查詢">
            </telerik:RadTab>
            <telerik:RadTab Text="新增客情資料回寫">
            </telerik:RadTab>
            <telerik:RadTab Text="每日查詢">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server">
            <div id="tabs-1">
                <table class="PopTable">
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label1" runat="server" Text="是否結案" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>

                        </td>
                    </tr>

                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label4" runat="server" Text="業務員" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label5" runat="server" Text="類別" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownList3" runat="server"></asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label2" runat="server" Text="客戶" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

                            <%-- <asp:AutoCompleteExtender
                                ID="AutoCompleteExtender1"
                                runat="server"
                                MinimumPrefixLength="1"
                                TargetControlID="TextBox1"
                                ServiceMethod="GetCompletionList"
                                ServicePath="WebService/WebService.asmx"
                                CompletionSetCount="15" />--%>

                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label6" runat="server" Text="專案" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label9" runat="server" Text="新增/更新日期"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <telerik:RadDatePicker ID="RadDatePicker1" runat="server" Width="120px"></telerik:RadDatePicker>
                            <asp:Label ID="Label11" runat="server" Text="~"></asp:Label>
                            <telerik:RadDatePicker ID="RadDatePicker2" runat="server" Width="120px"></telerik:RadDatePicker>
                            <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
                        </td>
                        <td class="PopTableRightTD">

                            <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label18" runat="server" Text="起始日/結案日"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <telerik:RadDatePicker ID="RadDatePicker5" runat="server" Width="120px"></telerik:RadDatePicker>
                            <asp:Label ID="Label19" runat="server" Text="~"></asp:Label>
                            <telerik:RadDatePicker ID="RadDatePicker6" runat="server" Width="120px"></telerik:RadDatePicker>
                            <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
                        </td>
                        <td class="PopTableRightTD">

                            <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text=" 查詢 " OnClick="btn1_Click"
                                meta:resourcekey="btn5Resource1" />
                        </td>

                    </tr>

                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                            <asp:Button ID="Button7" runat="server" Text=" 7天內更新-查詢 " OnClick="Button7_Click"
                                meta:resourcekey="btn7Resource1" />
                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                            <asp:Button ID="Button6" runat="server" Text=" 7天以上未更新-查詢 " OnClick="Button6_Click"
                                meta:resourcekey="btn6Resource1" />
                        </td>

                    </tr>
                </table>
                <table class="PopTable">
                    <tr>
                        <td colspan="2" class="PopTableRightTD">
                            <div style="overflow-x: auto; width: 100%">
                                <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                    <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                    <Columns>
                                        <asp:BoundField HeaderText="編號" DataField="ID" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="業務員" DataField="SALES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="類別" DataField="KINDS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="客戶" DataField="CLIENTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="專案" DataField="PROJECTS" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="待辦事件" DataField="EVENTS" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="進度內容" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="CONTENT" runat="server" Text='<%# Bind("COMMENTS") %>' Style="text-align: left" HorizontalAlign="Left" Width="300px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                                <itemstyle horizontalalign="Left" width="300px"></itemstyle>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField HeaderText="進度內容" DataField="COMMENTS" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <itemstyle horizontalalign="Left" width="300px"></itemstyle>
                                        </asp:BoundField>--%>
                                        <asp:BoundField HeaderText="起始日" DataField="SDAYS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="結案日" DataField="EDAYS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="是否結案" DataField="ISCLOSE" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" runat="server" HorizontalAlign="Center" Length="100px" Width="100px" onclick="ImageClick(this)" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="更新進度" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Button2" runat="server" Text="更新" ForeColor="Red" CommandName="Button2" CommandArgument='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="修改" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Button1" runat="server" Text="修改" ForeColor="Red" CommandName="Button1" CommandArgument='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="結案按鈕" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Button3" runat="server" Text="結案" CommandName="Button3" ForeColor="Red" CommandArgument='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="新增時間" DataField="ADDDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="最新更新時間" DataField="UPDATEDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
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
                            <asp:Label ID="Label3" runat="server" Text="新增資料" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:Button ID="btn4" runat="server" Text="新增資料" ForeColor="red" OnClientClick="return btn4_Click(this)" meta:resourcekey="btn4Resource1" />

                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView3" runat="server">
            <div id="tabs-1">
                <table class="PopTable">
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label10" runat="server" Text="客戶" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                            <asp:Button ID="Button4" runat="server" Text=" 查詢 " OnClick="Button4_Click"
                                meta:resourcekey="btn5Resource1" />
                        </td>

                    </tr>
                </table>
                <table class="PopTable">
                    <tr>
                        <td colspan="2" class="PopTableRightTD">
                            <div style="overflow-x: auto; width: 100%">
                                <Fast:Grid ID="Grid2" OnRowDataBound="Grid2_RowDataBound" OnRowCommand="Grid2_RowCommand" runat="server" OnBeforeExport="OnBeforeExport2" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource2" OnPageIndexChanging="grid_PageIndexChanging2">
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                    <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                    <Columns>
                                        <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="日期" DataField="CREATE_DATETIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="客情記錄" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="NOTE_CONTENT" runat="server" Text='<%# Bind("NOTE_CONTENT") %>' Style="text-align: left" HorizontalAlign="Left" Width="600px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                                <itemstyle horizontalalign="Left" width="600px"></itemstyle>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--   <asp:BoundField HeaderText="日期" DataField="NOTE_CONTENT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField>--%>
                                    </Columns>
                                </Fast:Grid>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">
            <div id="tabs-2">
                <table class="PopTable">
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label7" runat="server" Text="新增客情資料回寫" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:Button ID="Button5" runat="server" Text="新增客情資料回寫" ForeColor="red" OnClick="Button5_OnClick" meta:resourcekey="btn4Resource1" />

                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadPageView>
         <telerik:RadPageView ID="RadPageView5" runat="server">
            <div id="tabs-1">
                <table class="PopTable">
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label8" runat="server" Text="是否結案" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownList4" runat="server"></asp:DropDownList>

                        </td>
                    </tr>

                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label12" runat="server" Text="業務員" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList5" runat="server"></asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label13" runat="server" Text="類別" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownList6" runat="server"></asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label14" runat="server" Text="客戶" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>

                            <%-- <asp:AutoCompleteExtender
                                ID="AutoCompleteExtender1"
                                runat="server"
                                MinimumPrefixLength="1"
                                TargetControlID="TextBox1"
                                ServiceMethod="GetCompletionList"
                                ServicePath="WebService/WebService.asmx"
                                CompletionSetCount="15" />--%>

                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label15" runat="server" Text="專案" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label16" runat="server" Text="查詢日期"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <telerik:RadDatePicker ID="RadDatePicker3" runat="server" Width="120px" OnSelectedDateChanged="RadDatePicker3_SelectedDateChanged" AutoPostBack="true"></telerik:RadDatePicker>
                            <asp:Label ID="Label17" runat="server" Text="~"></asp:Label>
                            <telerik:RadDatePicker ID="RadDatePicker4" runat="server" Width="120px"></telerik:RadDatePicker>
                            <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
                        </td>
                        <td class="PopTableRightTD">

                            <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD"></td>
                        <td>
                            <asp:Button ID="Button3" runat="server" Text=" 查詢 " OnClick="btn3_Click"
                                meta:resourcekey="btn5Resource1" />
                        </td>

                    </tr>
                </table>
                <table class="PopTable">
                    <tr>
                        <td colspan="2" class="PopTableRightTD">
                            <div style="overflow-x: auto; width: 100%">
                                <Fast:Grid ID="Grid3" OnRowDataBound="Grid3_RowDataBound" OnRowCommand="Grid3_RowCommand" runat="server" OnBeforeExport="OnBeforeExport3" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource3" OnPageIndexChanging="grid_PageIndexChanging3">
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                    <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                    <Columns>
                                        <asp:BoundField HeaderText="編號" DataField="ID" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="業務員" DataField="SALES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="類別" DataField="KINDS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="客戶" DataField="CLIENTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                     <%--   <asp:BoundField HeaderText="專案" DataField="PROJECTS" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="待辦事件" DataField="EVENTS" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="進度內容" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="CONTENT" runat="server" Text='<%# Bind("COMMENTS") %>' Style="text-align: left" HorizontalAlign="Left" Width="300px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                                <itemstyle horizontalalign="Left" width="300px"></itemstyle>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="起始日" DataField="SDAYS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="結案日" DataField="EDAYS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <%-- <asp:BoundField HeaderText="進度內容" DataField="COMMENTS" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <itemstyle horizontalalign="Left" width="300px"></itemstyle>
                                        </asp:BoundField>--%>
                                        <asp:BoundField HeaderText="是否結案" DataField="ISCLOSE" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" runat="server" HorizontalAlign="Center" Length="100px" Width="100px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="更新進度" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Button2" runat="server" Text="更新" ForeColor="Red" CommandName="Button2" CommandArgument='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="修改" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Button1" runat="server" Text="修改" ForeColor="Red" CommandName="Button1" CommandArgument='<%# Eval("ID") %>' />
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
</asp:Content>

