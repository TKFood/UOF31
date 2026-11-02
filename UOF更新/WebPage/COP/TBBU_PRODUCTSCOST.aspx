<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_PRODUCTSCOST.aspx.cs" Inherits="CDS_WebPage_COP_TBBU_PRODUCTSCOST" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>    
    function btn4_Click(sender) {
        //從前端開始視窗
        //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
        //OpenDialogResult為關閉視後會執行的JS Function
        //參數使用JSON格式傳遞
       // $uof.dialog.open2("~/CDS/WebPage/COP/TBBU_PRODUCTSDialogADD.aspx", sender, "", 800, 600, OpenDialogResult, {});

          return false;

      }

    //如果有設定回傳值則執行sender Event
    function OpenDialogResult(returnValue) {
        if (typeof (returnValue) == "undefined")
            return false;
        else
            return true;
    }



</script>

      <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
	    <Tabs>
		    <telerik:RadTab Text="資料整理">
		    </telerik:RadTab>
		    <telerik:RadTab Text="新增資料">
		    </telerik:RadTab>
	    </Tabs>
    </telerik:RadTabStrip>
	<telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
	    <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
		   <div id="tabs-1">
              <table class="PopTable">
                   <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label2" runat="server" Text="品名:" meta:resourcekey="Label4Resource1"></asp:Label>             
                        </td>
                        <td class="PopTableRightTD"> 
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>                                         
                        </td>  
                  </tr>                   
                  <tr>
                      <td class="PopTableLeftTD">
                            <asp:Label ID="Label1" runat="server" Text="年月:" meta:resourcekey="Label4Resource1"></asp:Label>             
                        </td>
                        <td class="PopTableRightTD">                    
                            <telerik:RadDatePicker ID="txtDate1"  runat="server" Width = "120px"></telerik:RadDatePicker>
                            <asp:Label ID="Label11" runat="server" Text="~"></asp:Label>
                            <telerik:RadDatePicker ID="txtDate2"  runat="server" Width = "120px"></telerik:RadDatePicker>
                            <asp:Label ID="Label12" runat="server" Text=" "></asp:Label>
         
                        </td>
                    </tr>                 
                     <tr>
                        <td class="PopTableLeftTD">
                                       
                        </td>
                       <td>
                            <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClientClick="return btn5_Click(this)" 
                                 meta:resourcekey="btn4Resource1" />   
                        </td>
                                 
                    </tr> 
                  </table>
             <table class="PopTable">
                 <tr>
                    <td colspan="2" class="PopTableRightTD" >
                        <div style="overflow-x:auto;width:100%">
                            <Fast:Grid ID="Grid1" style="overflow-x:auto;width:100%" OnRowDataBound="Grid1_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport1" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="GridResource1" OnPageIndexChanging="grid1_PageIndexChanging" >
                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>                 
                                    <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"  ></ExportExcelSettings>
                                        <Columns>
                                            <asp:BoundField HeaderText="年月" DataField="ME002" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="品號" DataField="ME001" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                              <asp:BoundField HeaderText="品名" DataField="MB002" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                              <asp:BoundField HeaderText="材料成本之單位成本" DataField="材料成本之單位成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                               <asp:BoundField HeaderText="人工成本之單位成本" DataField="人工成本之單位成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                               <asp:BoundField HeaderText="製造費用之單位成本" DataField="製造費用之單位成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                               <asp:BoundField HeaderText="加工費用之單位成本" DataField="加工費用之單位成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                               <asp:BoundField HeaderText="單位成本" DataField="單位成本" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
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
                                 
                    </tr> 
                  </table>
          </div>
	    </telerik:RadPageView>
    </telerik:RadMultiPage>​
</asp:Content>

