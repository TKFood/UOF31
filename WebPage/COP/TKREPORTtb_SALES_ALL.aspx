<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TKREPORTtb_SALES_ALL.aspx.cs" Inherits="CDS_WebPage_TKREPORTtb_SALES_ALL" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

<script>    
    $(function () {
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
                <asp:Button ID="Button1" runat="server" Text=" 查詢 "
                    onclick="btn1_Click" meta:resourcekey="btn4Resource1" />                
            </td>            
        </tr>
    </table>

    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
	    <Tabs>
		    <telerik:RadTab Text="業務行事歷">
		    </telerik:RadTab>
		    <telerik:RadTab Text="業務客戶明細記錄">
		    </telerik:RadTab>
		    <telerik:RadTab Text="業務比對分析">
		    </telerik:RadTab>
	    </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
	    <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
		   <div id="tabs-1">
             <table class="PopTable">
                <td colspan="2" class="PopTableRightTD" >
                    <div style="overflow-x:auto;width:100%">
                        <Fast:Grid ID="Grid2" OnRowDataBound="Grid2_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid2Resource1" OnPageIndexChanging="grid2_PageIndexChanging" >
                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                <Columns>
                                     <asp:BoundField HeaderText="業務" DataField="NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    </asp:BoundField>   
                                     <asp:BoundField HeaderText="日期" DataField="START_TIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    </asp:BoundField> 
                                     <asp:BoundField HeaderText="拜訪主旨" DataField="SUBJECT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                    </asp:BoundField>   
                                       <asp:BoundField HeaderText="拜訪內容" DataField="DESCRIPTION" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Left" Width="500px"></ItemStyle>
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
                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging" >
                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                <Columns>
                                     <asp:BoundField HeaderText="業務" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    </asp:BoundField>   
                                     <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                    </asp:BoundField>   
                                  <%--  <asp:TemplateField HeaderText="記錄" >
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("NOTE_CONTENT") %>'></asp:Label>
                                        </ItemTemplate>                                        
                                     </asp:TemplateField>--%>
                                     <asp:BoundField HeaderText="記錄" DataField="NOTE_CONTENT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                    </asp:BoundField>   
                                     <asp:BoundField HeaderText="記錄日期" DataField="CREATE_DATETIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
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
                        <Fast:Grid ID="Grid3" OnRowDataBound="Grid3_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid3Resource1" OnPageIndexChanging="grid3_PageIndexChanging" >
                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                <Columns>
                                    <asp:BoundField HeaderText="日期" DataField="CREATE_DATETIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    </asp:BoundField> 
                                     <asp:BoundField HeaderText="業務" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                    </asp:BoundField>  
                                   <%--  <asp:BoundField HeaderText="拜訪主旨" DataField="SUBJECT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                    </asp:BoundField>  
                                          <asp:BoundField HeaderText="拜訪內容" DataField="DESCRIPTION" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Left" Width="500px"></ItemStyle>
                                    </asp:BoundField>  --%>
                                          <asp:BoundField HeaderText="公司" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                    </asp:BoundField>  
                                          <asp:BoundField HeaderText="記錄" DataField="NOTE_CONTENT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
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

