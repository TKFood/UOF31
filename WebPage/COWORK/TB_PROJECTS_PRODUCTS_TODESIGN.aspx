<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TB_PROJECTS_PRODUCTS_TODESIGN.aspx.cs" Inherits="CDS_WebPage_COWORK_TB_PROJECTS_PRODUCTS_TODESIGN" %>

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
                        <asp:Label ID="Label14" runat="server" Text="是否結案: "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_ISCLOSED" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

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
                                             
                                                <asp:TemplateField HeaderText="表單連結" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:HyperLink
                                                            ID="hlTask"
                                                            runat="server"
                                                            Text='點我開表單'
                                                            Target="_blank">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              <asp:TemplateField HeaderText="專案編號" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_專案編號" runat="server" Text='<%# Bind("專案編號") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="分類" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_分類" runat="server" Text='<%# Bind("分類") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="項目名稱" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_項目名稱" runat="server" Text='<%# Bind("項目名稱") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="專案負責人" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_專案負責人s" runat="server" Text='<%# Bind("專案負責人") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="是否需要設計" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_是否需要設計" runat="server" Text='<%# Bind("是否需要設計") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="設計其他要求" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_設計其他要求" runat="server" Text='<%# Bind("設計其他要求") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="提供需求方圖檔(電子檔)" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_提供需求方圖檔電子檔" runat="server" Text='<%# Bind("提供需求方圖檔電子檔") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="期望設計風格(風格圖2-3張)" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_期望設計風格風格圖23張" runat="server" Text='<%# Bind("期望設計風格風格圖23張") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="特別說明" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <itemtemplate>
                                                        <asp:Label ID="Label_特別說明" runat="server" Text='<%# Bind("特別說明") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </itemtemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="設計回覆" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNewField_GV1_設計回覆" runat="server" Text='<%# Bind("設計回覆") %>' Width="100%" TextMode="MultiLine" CssClass="multiline-textbox" Rows="5" onkeyup="autoResizeTextBox(this)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>   
                                                 <asp:TemplateField HeaderText="功能" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button2" runat="server" Text="更新設計回覆" CommandName="Button2" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('確定？');" />                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_是否結案" runat="server" Text='<%# Bind("是否結案") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                      
                                                   <asp:TemplateField HeaderText="ID" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_ID" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line; width: 100%;"></asp:Label>
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

                <telerik:RadPageView ID="RadPageView99" runat="server">
                    <div id="tabs-99">
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>

            ​
          
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

