<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TKSALESCLIENT.aspx.cs" Inherits="CDS_WebPage_TKSALESCLIENT" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script>    
 
    //如果有設定回傳值則執行sender Event
    function OpenDialogResult(returnValue) {
        if (typeof (returnValue) == "undefined")
            return false;
        else
            return true;
    }

    $(function () {
        $("#tabs").tabs();

        $("#<%= txtDate1.ClientID %>").datepicker({ dateFormat: "yy/mm/dd", });
        $("#<%= txtDate2.ClientID %>").datepicker({ dateFormat: "yy/mm/dd", });
    });


</script>

    <table class="PopTable" >         
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label2" runat="server" Text="日期:" meta:resourcekey="Label4Resource1"></asp:Label>             
            </td>
            <td class="PopTableRightTD"> 
                <asp:TextBox ID="txtDate1"  runat="server" Width = "100px"></asp:TextBox>
                <asp:Label ID="Label11" runat="server" Text="~"></asp:Label>
                <asp:TextBox ID="txtDate2"  runat="server" Width = "100px"></asp:TextBox>
                <asp:Label ID="Label12" runat="server" Text=" "></asp:Label>
                <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClientClick="return btn5_Click(this)" 
                    onclick="btn6_Click" meta:resourcekey="btn4Resource1" />                
            </td>            
        </tr>
    </table> 

         <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
	    <Tabs>
		    <telerik:RadTab Text="客戶記錄">
		    </telerik:RadTab>
		    <telerik:RadTab Text="客戶明細記錄">
		    </telerik:RadTab>
		    <telerik:RadTab Text="未聯絡的客戶">
		    </telerik:RadTab>
	    </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
	    <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
		   <div id="tabs-1">
             <table class="PopTable">
                <td colspan="2" class="PopTableRightTD" >
                    <div style="overflow-x:auto;width:100%">
                      <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server" OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging">
                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                    <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                    <Columns>
                               
                                        <asp:BoundField HeaderText="業務員" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="部門" DataField="GROUP_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="客戶記錄筆數" DataField="NOTENUMS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="客戶筆數" DataField="COPNOTENUMS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="負責總客戶數" DataField="TOTALNUMS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="聯絡記錄率%" DataField="PCTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
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
                <td colspan="2" class="PopTableRightTD" >
                    <div style="overflow-x:auto;width:100%">
                       <Fast:Grid ID="Grid2" OnRowDataBound="Grid2_RowDataBound" runat="server" OnBeforeExport="OnBeforeExport2" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid2Resource1" OnPageIndexChanging="grid2_PageIndexChanging">
                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                    <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                    <Columns>
                               
                                        <asp:BoundField HeaderText="業務員" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="部門" DataField="GROUP_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="300px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="客戶代號" DataField="ERPNO" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                         <asp:BoundField HeaderText="客戶記錄筆數" DataField="NOTENUMS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                        </asp:BoundField> 
                               
                             
                                    </Columns>
                                </Fast:Grid>
                        </div>
                </td>
            </tr>
        </table>
          </div>
	    </telerik:RadPageView>
	    <telerik:RadPageView ID="RadPageView3" runat="server">
		    <div id="tabs-3">
                <table class="PopTable">
                <td colspan="2" class="PopTableRightTD" >
                    <div style="overflow-x:auto;width:100%">
                          <Fast:Grid ID="Grid3" OnRowDataBound="Grid3_RowDataBound" runat="server" OnBeforeExport="OnBeforeExport3" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid3Resource1" OnPageIndexChanging="grid3_PageIndexChanging" >
                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                    <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                    <Columns>
                               
                                        <asp:BoundField HeaderText="業務員" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                        <asp:BoundField HeaderText="部門" DataField="GROUP_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField> 
                                        <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="300px"></ItemStyle>
                                        </asp:BoundField> 
                               
                             
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

