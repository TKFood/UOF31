<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TK_TB_COMPANY_PROJECTS.aspx.cs" Inherits="CDS_WebPage_TKBUSINESS_TK_TB_COMPANY_PROJECTSE" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .multiline-textbox {
            width: 200px;
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
    </style>
    <script>
        $(function () {

        });

        function updateNewComments(textBox) {
            // 获取当前 txtNewField 的值
            var newValue = textBox.value;

            // 查找同一行中的 回饋進度與內容 标签
            var gridViewRow = textBox.closest('tr');
            var oldContentLabel = gridViewRow.querySelector('span[id$="COMMENTSLabel"]');
            var txtNewComments = gridViewRow.querySelector('#' + textBox.id.replace('txtNewField', 'txtNEWCOMMENTS'));

            // 获取旧内容
            if (oldContentLabel) {
                var oldValue = oldContentLabel.innerText;

                // 找出新增的文字
                var newText = newValue.replace(oldValue, '').trim();

                // 去除多余的空行
                newText = newText.replace(/^\s*\n/gm, '');

                // 更新 txtNEWCOMMENTS 的值
                if (txtNewComments) {
                    txtNewComments.value = newText;
                }
            }
        }

        //如果有設定回傳值則執行sender Event
        function OpenDialogResult(returnValue) {
            if (typeof (returnValue) == "undefined")
                return false;
            else
                return true;
        }
        function autoResizeTextBox(textbox) {
            textbox.style.height = 'auto'; // Reset the height
            textbox.style.height = (textbox.scrollHeight) + 'px'; // Adjust the height to fit the content
        }

        function initializeTextBoxes() {
            var textboxes = document.getElementsByClassName('multiline-textbox');
            for (var i = 0; i < textboxes.length; i++) {
                autoResizeTextBox(textboxes[i]);
            }
        }

        // Call this function on window load and after partial postback
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initializeTextBoxes);
        window.onload = initializeTextBoxes;

    </script>

    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
        <Tabs>
            <telerik:RadTab Text="專案進度">
            </telerik:RadTab>
            <telerik:RadTab Text="專案新增">
            </telerik:RadTab>
            <telerik:RadTab Text="資料">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server">
            <div id="tabs-1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table class="PopTable">
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text="是否結案: "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownListISCLOSE" runat="server" AutoPostBack="true" ></asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClick="btn1_Click" meta:resourcekey="btn1_Resource1" />
                                </td>

                            </tr>

                        </table>
                        <table class="PopTable">
                            <tr>
                                <td colspan="2" class="PopTableRightTD">
                                    <div style="overflow-x: auto; width: 100%">
                                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_OnRowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                            <Columns>
                                               <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>                                                        
                                                        <asp:DropDownList ID="DropDownListISCLOSED" runat="server" Style="word-wrap: break-word; min-width: 100%;"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <%--<asp:TemplateField HeaderText="專案編號" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="專案編號" runat="server" Text='<%# Bind("NO") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="專案屬性" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>                                                        
                                                        <asp:DropDownList ID="DropDownListKINDS" runat="server" Style="width: 100%; word-break: break-all;"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="專案名稱" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="專案名稱" runat="server" Text='<%# Bind("PROJECTNAMES") %>' Style="word-break: break-all; white-space: pre-line;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="需求單位" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="需求單位" runat="server" Text='<%# Bind("DEPNAMES") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>--%>
                                                        <asp:DropDownList ID="DropDownListDEPNAMES" runat="server" Style="word-wrap: break-word; min-width: 100%;"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="產品開發申請書" ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="產品開發申請書" runat="server" Text='<%# Bind("PRODUCTAPPLYS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>--%>
                                                        <asp:DropDownList ID="DropDownListPRODUCTAPPLYS" runat="server" Style="width: 100%; word-break: break-all;"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="包材暨包裝設計及變更申請書" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="包材暨包裝設計及變更申請書" runat="server" Text='<%# Bind("PACKAPPLYS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>--%>
                                                        <asp:DropDownList ID="DropDownListPACKAPPLYS" runat="server" Style="width: 100%; word-break: break-all;"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="需求單位預計上市時間" ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="需求單位預計上市時間" runat="server" Text='<%# Bind("SALEDATES") %>' Style="word-break: break-all; white-space: pre-line; max-width: 100px;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="專案進度" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="專案進度" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>--%>
                                                        <asp:DropDownList ID="DropDownListSTATUS" runat="server" Style="word-wrap: break-word; min-width: 100%;"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="回饋進度與內容" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="COMMENTSLabel" runat="server" Text='<%# Bind("COMMENTS") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="追蹤日" ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="追蹤日" runat="server" Text='<%# Bind("TRACEDATES") %>'  Style="word-break: break-all; white-space: pre-line; max-width: 100px;"></asp:TextBox>--%>
                                                        <telerik:RadDatePicker ID="RadDatePicker1" SelectedDate='<%# Bind("TRACEDATES") %>' runat="server" Width="120px"></telerik:RadDatePicker>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="輸入回覆" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="txtNewField" runat="server" Text='' TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtNewField" runat="server" Text='<%# Bind("COMMENTS") %>' TextMode="MultiLine" CssClass="multiline-textbox" Rows="3" onkeyup="autoResizeTextBox(this);updateNewComments(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="主管查閱的回覆" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="txtNewField" runat="server" Text='' TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtNEWCOMMENTS" runat="server" Text='<%# Bind("NEWCOMMENTS") %>' TextMode="MultiLine" CssClass="multiline-textbox" Rows="3" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>



                                                <asp:TemplateField HeaderText="新增回覆" ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Grid1Button1" runat="server" Text="新增回覆" CommandName="Grid1Button1" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('新增回覆 確定？');" />
                                                        <!-- 上空行 -->
                                                        <div style="margin-top: 20px;"></div>
                                                        <asp:Button ID="Grid1Button2" runat="server" Text="修改專案" CommandName="Grid1Button2" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('修改專案 確定？');" />
                                                        <!-- 上空行 -->
                                                        <div style="margin-top: 20px;"></div>
                                                        <asp:Button ID="Grid1Button3" runat="server" Text="改結案" CommandName="Grid1Button3" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('改結案 確定？');" />
                                                        <!-- 上空行 -->
                                                        <div style="margin-top: 20px;"></div>
                                                        <asp:Button ID="Grid1Button4" runat="server" Text="改未結案" CommandName="Grid1Button4" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('改未結案 確定？');" />
                                                        <!-- 下空行 -->
                                                        <div style="margin-bottom: 20px;"></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                        </Fast:Grid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>
        </telerik:RadPageView>

        <telerik:RadPageView ID="RadPageView2" runat="server">
            <div id="tabs-2">
                <table class="PopTable">
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label1" runat="server" Text="專案新增"></asp:Label>
                        </td>
                        <td class="PopTableRightTD"></td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label12" runat="server" Text="是否結案"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownListADDISCLOSED" runat="server" Width="50%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label2" runat="server" Text="專案編號"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:TextBox ID="TextBox1" runat="server" Text="" Width="50%" Row="1" Style="height: 100%;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label3" runat="server" Text="專案屬性"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownListADDKINDS" runat="server" Width="50%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label4" runat="server" Text="專案名稱"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:TextBox ID="TextBox2" runat="server" Text="" Width="50%" Row="1" Style="height: 100%;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label5" runat="server" Text="需求單位"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownListADDDEPNAMES" runat="server" Width="50%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label6" runat="server" Text="產品開發申請書"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownListADDPRODUCTAPPLYS" runat="server" Width="50%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label7" runat="server" Text="包材暨包裝設計及變更申請書"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownListADDPACKAPPLYS" runat="server" Width="50%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label8" runat="server" Text="需求單位預計上市時間"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:TextBox ID="TextBox3" runat="server" Text="" Width="50%" Row="1" Style="height: 100%;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label9" runat="server" Text="專案進度"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:DropDownList ID="DropDownListADDSTATUS" runat="server" Width="50%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label10" runat="server" Text="回饋進度與內容"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <%--<asp:TextBox ID="TextBox4" runat="server" TextMode="MultiLine" Rows="10" Text="" Width="50%" Style="height: 100%;"></asp:TextBox>--%>
                            <asp:TextBox ID="TextBox4" runat="server" Text="" TextMode="MultiLine" Rows="5" Width="50%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label11" runat="server" Text="追蹤日"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <telerik:RadDatePicker ID="RadDatePicker1" runat="server" Width="50%"></telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label13" runat="server" Text=""></asp:Label>
                        </td>
                        <td class="PopTableRightTD">
                            <asp:Button ID="Button2" runat="server" Text="新增專案" OnClick="btn2_Click" meta:resourcekey="btn2_Resource1" CssClass="custom-button" />
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView3" runat="server">
            <div id="tabs-3">
            </div>
        </telerik:RadPageView>

    </telerik:RadMultiPage>​
     

</asp:Content>


