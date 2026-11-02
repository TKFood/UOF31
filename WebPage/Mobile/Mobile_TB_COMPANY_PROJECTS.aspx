<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MobileMasterPage.master" AutoEventWireup="true" CodeFile="Mobile_TB_COMPANY_PROJECTS.aspx.cs" Inherits="CDS_WebPage_Mobile_TB_COMPANY_PROJECTS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .custom-button {
            background-color: #FF5733;
            color: #FFFFFF;
            width: 150px; /* 设置宽度 */
            height: 40px; /* 设置高度 */
            font-size: 16px; /* 设置字体大小 */
        }

        .fast-grid {
        }

            /* 表头样式 */
            .fast-grid th {
                background-color: dodgerblue; /* 背景颜色 */
                color: white; /* 字体颜色 */
                font-weight: bold; /* 字体粗体 */
                border: 2px solid white; /* 边框粗线 */
                padding: 5px;
                text-align: center;
            }

            /* 单元格框线 */
            .fast-grid td {
                border: 1px solid #000;
                padding: 5px;
                text-align: left; /* 根据需要更改文本对齐方式 */
            }
    </style>
    <head>
        <title>業務</title>
    </head>
    <body>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
                <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage">
                    <Tabs>
                        <telerik:RadTab Text="交辨回覆" PageViewID="RadPageView1">
                        </telerik:RadTab>
                        <telerik:RadTab Text="資料" PageViewID="RadPageView2">
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
                                            <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_OnRowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1" CssClass="fast-grid">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                                <Columns>
                                                    <%--<asp:TemplateField HeaderText="專案編號" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="專案編號" runat="server" Text='<%# Bind("NO") %>' Style="word-break: break-all; white-space: pre-line"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="建立日期" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="建立日期" runat="server" Text='<%# Bind("CREATEDATES") %>' Style="word-break: break-all; white-space: pre-line"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="專案名稱" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="專案名稱" runat="server" Text='<%# Bind("PROJECTNAMES") %>' Style="word-break: break-all; white-space: pre-line"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="專案屬性" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="專案屬性" runat="server" Text='<%# Bind("KINDS") %>' Style="word-break: break-all; white-space: pre-line"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="需求單位" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="需求單位" runat="server" Text='<%# Bind("DEPNAMES") %>' Style="word-break: break-all; white-space: pre-line"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="需求單位預計上市時間" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="需求單位預計上市時間" runat="server" Text='<%# Bind("SALEDATES") %>' Style="word-break: break-all; white-space: pre-line"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                                    <%--<asp:TemplateField HeaderText="專案進度" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="專案進度" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all; white-space: pre-line"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="最新進度" ItemStyle-Width="50%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="最新進度" runat="server" Text='<%# Bind("NEWCOMMENTS") %>' Style="word-break: break-all; white-space: pre-line"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--   <asp:TemplateField HeaderText="追蹤日" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="追蹤日" runat="server" Text='<%# Bind("TRACEDATES") %>' Style="word-break: break-all; white-space: pre-line" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="是否結案" runat="server" Text='<%# Bind("ISCLOSED") %>' Style="word-break: break-all; white-space: pre-line" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </telerik:RadPageView>

                    <telerik:RadPageView ID="RadPageView2" runat="server" Selected="false" PageViewID="5">
                        <div id="tabs-">
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </ContentTemplate>
        </asp:UpdatePanel>

    </body>
</asp:Content>

