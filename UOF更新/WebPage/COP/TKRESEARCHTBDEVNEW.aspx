<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCHTBDEVNEW.aspx.cs" Inherits="CDS_WebPage_TKRESEARCHTBDEVNEW" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>    
    function btn4_Click(sender) {
        //從前端開始視窗
        //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
        //OpenDialogResult為關閉視後會執行的JS Function
        //參數使用JSON格式傳遞
        $uof.dialog.open2("~/CDS/WebPage/COP/TKRESEARCHTBDEVNEWVDialogADD.aspx", sender, "", 800, 600, OpenDialogResult, {});

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

    <table class="PopTable" >         
         <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label1" runat="server" Text="新增資料" meta:resourcekey="Label4Resource1"></asp:Label>
            </td>
            <td class="PopTableRightTD">              
                <asp:Button ID="btn4" runat="server" Text="開新視窗" OnClientClick="return btn4_Click(this)" 
                    onclick="btn4_Click" meta:resourcekey="btn4Resource1" />
            </td>            
        </tr> 
                </tr> 
         <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label3" runat="server" Text="更新畫面" meta:resourcekey="Label4Resource1"></asp:Label>
            </td>
            <td class="PopTableRightTD">              
                <asp:Button ID="Button5" runat="server" Text="更新畫面" OnClientClick="return btn5_Click(this)" 
                    onclick="btn5_Click" meta:resourcekey="btn4Resource1" />
            </td>            
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label2" runat="server" Text="狀態:" meta:resourcekey="Label4Resource1"></asp:Label>             
            </td>
            <td class="PopTableRightTD"> 
                   <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClientClick="return btn5_Click(this)" 
                    onclick="btn6_Click" meta:resourcekey="btn4Resource1" />                
            </td>            
        </tr>
    </table>
    <table class="PopTable">
         <tr >
            <td colspan="2" class="PopTableRightTD" >
                <div style="overflow-x:auto;width:100%">
                     <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server" OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" >
                     <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                        <Columns>                            
                            <asp:BoundField HeaderText="編號" DataField="SERNO" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                            </asp:BoundField>       
                               <asp:BoundField HeaderText="狀態" DataField="STATUS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="接單日期" DataField="SDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="口味名稱" DataField="PRODUCTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                            </asp:BoundField> 
                               <asp:BoundField HeaderText="客戶" DataField="CLIENTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="負責業務" DataField="SALES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                               <asp:BoundField HeaderText="需求數量" DataField="NUMS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="最新試做日期" DataField="TESTDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                               <asp:BoundField HeaderText="最新試做結果" DataField="TESTMEMO" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                                   
                      
                            <asp:TemplateField HeaderText="修改明細" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="Button1" runat="server" Text="修改" ForeColor="Red"  CommandArgument='<%# Eval("SERNO") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="更新試做進度" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >                
                                  <ItemTemplate>
                                    <asp:Button ID="Button2" runat="server" Text="修改" ForeColor="Red"  CommandArgument='<%# Eval("SERNO") %>'/>
                                </ItemTemplate>                                
                            </asp:TemplateField> 
                            
                        </Columns>
                    </Fast:Grid>
                </div>               
            </td>
        </tr>
    </table>
</asp:Content>

