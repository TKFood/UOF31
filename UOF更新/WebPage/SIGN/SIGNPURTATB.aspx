<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="SIGNPURTATB.aspx.cs" Inherits="CDS_WebPage_SIGN_SIGNPURTATB" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>





<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>    
<%--    $(function () {
        $("#<%= txtDate1.ClientID %>").datepicker({ dateFormat: "yymmdd", });       
    });--%>

   
</script>

     <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
     <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
	    <Tabs>
		    <telerik:RadTab Text="請購">
		    </telerik:RadTab>
		    <telerik:RadTab Text="變更">
		    </telerik:RadTab>		    
	    </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
	<telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
         <table class="PopTable" >         
             <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label2" runat="server" Text="日期:" meta:resourcekey="Label1Resource1"></asp:Label>             
                </td>
                <td class="PopTableRightTD">                    
                    <asp:TextBox ID="txtDate1"  runat="server" Width = "100px"></asp:TextBox>           
                    <asp:Button ID="Button1" runat="server" Text=" 查詢 "
                        onclick="btn1_Click" meta:resourcekey="btn1Resource1" />                
                </td>            
            </tr>
        </table>
        <label>訂單</label>

        <table class="PopTable">
              <tr>
                <td colspan="2" class="PopTableRightTD" >
                    <div style="overflow-x:auto;width:100%">
                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport1" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging" >
                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                <Columns>
                                      <asp:TemplateField HeaderText="CheckBox" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox" AutoCallBack="true" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                      </asp:TemplateField> 
                                      <asp:BoundField HeaderText="單別" DataField="MQ002" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    </asp:BoundField>  
                                     <asp:BoundField HeaderText="單別" DataField="TA001" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    </asp:BoundField>  
                                     <asp:BoundField HeaderText="單號" DataField="TA002" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                    </asp:BoundField>                                       
                                    <asp:TemplateField HeaderText="明細" ItemStyle-Width="600px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                            <asp:Label ID="CONTENT" runat="server" Text='<%# Bind("DETAILS") %>' style="text-align:left" HorizontalAlign="Left" Width="600px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                            <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                        </ItemTemplate>                                        
                                    </asp:TemplateField>   
                                
                                </Columns>
                            </Fast:Grid>
                        </div>
                </td>
            </tr>
        </table>
        <table class="PopTable" >         
             <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label1" runat="server" Text="簽核" meta:resourcekey="Label2Resource1"></asp:Label>             
                </td>
                <td class="PopTableRightTD">                                    
                    <asp:Button ID="Button2" runat="server" Text="簽核 " onclick="btn2_Click" meta:resourcekey="btn2Resource1" Width="200px" BackColor="red" />                
                </td>            
            </tr>
        </table>
            <table class="PopTable" >         
             <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label3" runat="server" Text="" meta:resourcekey="Label2Resource1"></asp:Label>             
                </td>          
            </tr>
        </table>

	</telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">
             <table class="PopTable" >         
                 <tr>
                    <td class="PopTableLeftTD">
                        <asp:Label ID="Label6" runat="server" Text="年度:" meta:resourcekey="Label1Resource1"></asp:Label>             
                    </td>
                    <td class="PopTableRightTD">                    
                        <asp:TextBox ID="txtDate2"  runat="server" Width = "100px"></asp:TextBox>           
                        <asp:Button ID="Button4" runat="server" Text=" 查詢 "
                            onclick="btn4_Click" meta:resourcekey="btn1Resource1" />                
                    </td>            
                </tr>
            </table>
            <label>變更</label>
                <table class="PopTable">
                     
                </table>
                <table class="PopTable" >         
                     <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label4" runat="server" Text="簽核" meta:resourcekey="Label2Resource1"></asp:Label>             
                        </td>
                        <td class="PopTableRightTD">                                    
                            <asp:Button ID="Button3" runat="server" Text="變更簽核 " onclick="btn3_Click" meta:resourcekey="btn2Resource1" Width="200px" BackColor="red" />                
                        </td>            
                    </tr>
                </table>
                    <table class="PopTable" >         
                     <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label5" runat="server" Text="" meta:resourcekey="Label5Resource1"></asp:Label>   
                            <asp:Label ID="Label7" runat="server" Text="" meta:resourcekey="Label7Resource1"></asp:Label>  
                        </td>          
                    </tr>
                </table>                                
        </telerik:RadPageView>
    </telerik:RadMultiPage>​
</asp:Content>

