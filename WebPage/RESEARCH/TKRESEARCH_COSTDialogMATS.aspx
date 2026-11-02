<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCH_COSTDialogMATS.aspx.cs" Inherits="CDS_WebPage_TKRESEARCH_COSTDialogMATS" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>

    <div style="overflow-x: auto; width: 800px">

        <table class="PopTable">
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label1" runat="server" Text="TKRESEARCH_COSTDialogROWS"></asp:Label>
                </td>
                <td class="PopTableRightTD" colspan="2">
                    <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <table class="PopTable">
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label10" runat="server" Text="新增物料"></asp:Label>
                </td>
                <td class="PopTableRightTD" colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="物料品名"></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text="<br>"></asp:Label>

                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Label ID="Label5" runat="server" Text="<br>"></asp:Label>

                    <asp:Label ID="Label3" runat="server" Text="物料規格 "></asp:Label>
                    <asp:Label ID="Label6" runat="server" Text="<br>"></asp:Label>

                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    <asp:Label ID="Label8" runat="server" Text="<br>"></asp:Label>

                    <asp:Label ID="Label4" runat="server" Text="物料單價 "></asp:Label>
                    <asp:Label ID="Label9" runat="server" Text="<br>"></asp:Label>

                    <asp:TextBox ID="TextBox3" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    <asp:Label ID="Label11" runat="server" Text="<br>"></asp:Label>

                    <asp:Button ID="Button1" runat="server" Text="新增存檔" OnClick="btn1_Click" />
                </td>
            </tr>
        </table>
        <table class="PopTable">
            <tr>
                <td colspan="2" class="PopTableRightTD">
                    <Fast:Grid ID="Grid1"  OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="15" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1">
                        <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                        <ExportExcelSettings AllowExportToExcel="False"></ExportExcelSettings>
                        <Columns>
                            <asp:BoundField HeaderText="物料品名" DataField="MB002" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="MEMODATES">
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="物料規格" DataField="MB003" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="物料單價" DataField="COSTMAT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="刪除物料" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="GV1DELETE" runat="server" Text="刪除物料" ForeColor="Red" CommandName="GV1DELETE" CommandArgument='<%# Eval("MB002") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Fast:Grid>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

