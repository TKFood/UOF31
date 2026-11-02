<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_TBSALESSCHEDULES.aspx.cs" Inherits="CDS_WebPage_COP_TBBU_TBSALESSCHEDULES" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>    
    function btn4_Click(sender) {
        //從前端開始視窗
        //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
        //OpenDialogResult為關閉視後會執行的JS Function
        //參數使用JSON格式傳遞
        $uof.dialog.open2("~/CDS/WebPage/COP/TBBU_TBSALESSCHEDULESDialogADD.aspx", sender, "", 800, 600, OpenDialogResult, {});

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
		    <telerik:RadTab Text="業務-通路檔期資料">
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
                            <%--<asp:Button ID="Button2" runat="server" Text="匯出" onclick="btn2_Click"  meta:resourcekey="btn2Resource1" />--%>
                            <%--<asp:Button ID="Button3" runat="server" Text="匯出2" onclick="btn3_Click"  meta:resourcekey="btn2Resource1" />--%>
                             
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
                                             <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="通路" DataField="DEALER" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="經銷商" DataField="OWNERS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="事件" DataField="EVENTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="1月" DataField="MONTHS1" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="2月" DataField="MONTHS2" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="3月" DataField="MONTHS3" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="4月" DataField="MONTHS4" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="5月" DataField="MONTHS5" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="6月" DataField="MONTHS6" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="7月" DataField="MONTHS7" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="8月" DataField="MONTHS8" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="9月" DataField="MONTHS9" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="10月" DataField="MONTHS10" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="11月" DataField="MONTHS11" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField>
                                               <asp:BoundField HeaderText="12月" DataField="MONTHS12" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="10px"></ItemStyle>
                                            </asp:BoundField> 
                                            
                                           <asp:TemplateField HeaderText="BTN" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Button1" runat="server" Text="修改" ForeColor="Red"  CommandArgument='<%# Eval("ID") %>'/>
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
	    <telerik:RadPageView ID="RadPageView2" runat="server">
		    <div id="tabs-2">
              <table class="PopTable">
                 <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label1" runat="server" Text="新增資料" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                        <td class="PopTableRightTD">      
                            <asp:Button ID="btn4" runat="server" Text="新增資料" ForeColor="red" OnClientClick="return btn4_Click(this)"  meta:resourcekey="btn4Resource1" />

                        </td>            
                    </tr> 
                  </table>
          </div>
	    </telerik:RadPageView>
    </telerik:RadMultiPage>​
</asp:Content>

