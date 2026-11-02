<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCHTBSALESDEVMEMO.aspx.cs" Inherits="CDS_WebPage_TKRESEARCHTBSALESDEVMEMO" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>    
    function btn4_Click(sender) {
        //從前端開始視窗
        //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
        //OpenDialogResult為關閉視後會執行的JS Function
        //參數使用JSON格式傳遞
        $uof.dialog.open2("~/CDS/WebPage/COP/TKRESEARCHTBSALESDEVMEMODialogADD.aspx", sender, "", 800, 600, OpenDialogResult, {});

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
                <div style="overflow-x:auto;width:2900px">
                     <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server" OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" >
                     <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                        <Columns>                            
                            <asp:BoundField HeaderText="編號" DataField="SERNO" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                            </asp:BoundField>                           
                             <asp:BoundField HeaderText="客戶" DataField="CLIENT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                              <asp:BoundField HeaderText="負責業務" DataField="OWNER" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField>   
                             <asp:BoundField HeaderText="產品品項" DataField="PROD" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Left" Width="360px"></ItemStyle>
                            </asp:BoundField> 
                                  <asp:BoundField HeaderText="業務進度" DataField="MEMO" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Left" Width="360px"></ItemStyle>
                            </asp:BoundField>
                                  <asp:BoundField HeaderText="研發進度" DataField="DEVMEMO" ItemStyle-Width="300px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Left" Width="360px"></ItemStyle>
                            </asp:BoundField>
                               <asp:BoundField HeaderText="狀態" DataField="STATUS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="末售" DataField="PRICES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="促銷設定" DataField="PROMOTIONS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="規格及屬性" DataField="SPEC" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="產品效期" DataField="VALID" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="通路" DataField="PLACES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="預估上市說明" DataField="ONSALES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                               <asp:BoundField HeaderText="預估上市日期" DataField="ONSALESDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="產品圖/樣袋完稿備註" DataField="PRODESGIN" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="可行性評估申請日期" DataField="ASSESSMENTDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="成本試算申請日期" DataField="COSTSDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="報價備註" DataField="SALESPRICES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="營標送驗備註" DataField="TEST" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="營標送驗申請日期" DataField="TESTDATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                             <asp:BoundField HeaderText="業務樣本確認日期" DataField="SALESCHECKOK" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                            <asp:BoundField HeaderText="業務更新日期" DataField="MEMODATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                            <asp:BoundField HeaderText="研發更新日期" DataField="DEVMEMODATES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            </asp:BoundField> 
                         
                            <asp:TemplateField HeaderText="維護專案" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >                
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnName" runat="server" Text='<%# Bind("ID") %>' meta:resourcekey="lbtnNameResource1"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                            </asp:TemplateField> 
                              <asp:TemplateField HeaderText="更新業務進度" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >                
                                <ItemTemplate>
                                    <asp:LinkButton ID="MEMO" runat="server" Text='<%# Bind("ID") %>' meta:resourcekey="lbtnNameResource1"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="更新研發進度" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >                
                                <ItemTemplate>
                                    <asp:LinkButton ID="DEVMEMO" runat="server" Text='<%# Bind("ID") %>' meta:resourcekey="lbtnNameResource1"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                            </asp:TemplateField> 
                        </Columns>
                    </Fast:Grid>
                </div>               
            </td>
        </tr>
    </table>
</asp:Content>

