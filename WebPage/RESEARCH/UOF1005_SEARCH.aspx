<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="UOF1005_SEARCH.aspx.cs" Inherits="CDS_WebPage_RESEARCH_UOF1005_SEARCH" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .multiline-textbox {
            word-break: break-all;
            white-space: pre-line;
        }

        .msg {
            color: #c00;
            font-weight: bold;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="1005.雜項採購單查詢">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>

            <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                    <table class="PopTable">
                        <tr>
                            <td class="PopTableLeftTD">
                                <asp:Label ID="Label1" runat="server" Text="品名關鍵字: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKeyword" runat="server" Text="" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="PopTableLeftTD">
                                <asp:Label ID="Label2" runat="server" Text="單號: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocNbr" runat="server" Text="" Width="200px" placeholder="空白 = 不限"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="PopTableLeftTD">
                                <asp:Label ID="Label3" runat="server" Text="申請日期: "></asp:Label>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dpSDate" runat="server" Width="130px">
                                    <DateInput DateFormat="yyyy/MM/dd" DisplayDateFormat="yyyy/MM/dd"></DateInput>
                                </telerik:RadDatePicker>
                                ～
                                <telerik:RadDatePicker ID="dpEDate" runat="server" Width="130px">
                                    <DateInput DateFormat="yyyy/MM/dd" DisplayDateFormat="yyyy/MM/dd"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="PopTableLeftTD"></td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClick="Button1_Click" />
                                <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <table class="PopTable">
                        <tr>
                            <td colspan="2" class="PopTableRightTD">
                                <div style="overflow-x: auto; width: 100%">
                                    <Fast:Grid ID="Grid1" runat="server" OnRowDataBound="Grid1_RowDataBound" OnPageIndexChanging="Grid1_PageIndexChanging" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="False" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="查無資料" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="">
                                        <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                        <Columns>
                                            <asp:TemplateField HeaderText="請購品名" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl請購品名" runat="server" Text='<%# Eval("請購品名") %>' CssClass="multiline-textbox"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="單號" ItemStyle-Width="12%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlTask" runat="server" Text='<%# Eval("DOC_NBR") %>' Target="_blank"></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="請購人員" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl請購人員" runat="server" Text='<%# Eval("請購人員") %>' CssClass="multiline-textbox"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="主要用途說明" ItemStyle-Width="43%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl主要用途說明" runat="server" Text='<%# Eval("主要用途說明") %>' CssClass="multiline-textbox"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="請購部門" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl請購部門" runat="server" Text='<%# Eval("請購部門") %>' CssClass="multiline-textbox"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
