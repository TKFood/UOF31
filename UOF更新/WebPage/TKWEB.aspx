<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TKWEB.aspx.cs" Inherits="CDS_WebPage_TKWEB" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>
    function btn3_Click(sender) {
        //從前端開始視窗
        //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
        //OpenDialogResult為關閉視後會執行的JS Function
        //參數使用JSON格式傳遞
        $uof.dialog.open2("~/CDS/WebPage/Dialog.aspx", sender, "", 800, 600, OpenDialogResult, { "ID": $('#<%=txtParam3.ClientID%>').val() });

        return false;

    }

    function btn4_Click(sender) {
        //從前端開始視窗
        //sender為註冊是由哪個視窗開啟，作為事後要觸發哪個元件的依據
        //OpenDialogResult為關閉視後會執行的JS Function
        //參數使用JSON格式傳遞
        $uof.dialog.open2("~/CDS/WebPage/TKWEBDialogADD.aspx", sender, "", 800, 600, OpenDialogResult, {});

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
                <asp:Label ID="Label2" runat="server" Text="使用Dialog.Open2開新視窗，並且傳參數" meta:resourcekey="Label2Resource1"></asp:Label>
            </td>
            <td class="PopTableRightTD">               
                <asp:TextBox ID="txtParam" runat="server" meta:resourcekey="txtParamResource1"></asp:TextBox>
                 <asp:Button ID="btn" runat="server" Text="開新視窗" onclick="btn_Click" meta:resourcekey="btnResource1" />
            </td>            
        </tr> 
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label4" runat="server" Text="使用$uof.dialog.open2開新視窗，並且傳參數" meta:resourcekey="Label4Resource1"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtParam3" runat="server" meta:resourcekey="txtParam3Resource1"></asp:TextBox>
                <asp:Button ID="btn3" runat="server" Text="開新視窗" OnClientClick="return btn3_Click(this)" 
                    onclick="btn3_Click" meta:resourcekey="btn3Resource1" />
            </td>            
        </tr> 
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
                <asp:Button ID="Button1" runat="server" Text="更新畫面" OnClientClick="return btn5_Click(this)" 
                    onclick="btn5_Click" meta:resourcekey="btn4Resource1" />
            </td>            
        </tr>
    </table>
    <table class="PopTable">
         <tr >
            <td colspan="2" class="PopTableRightTD" >
                <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="15" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" >
                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                    <ExportExcelSettings AllowExportToExcel="False"></ExportExcelSettings>
                    <Columns>
                        <asp:TemplateField HeaderText="單號" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" >                
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnName" runat="server" Text='<%# Bind("ID") %>' meta:resourcekey="lbtnNameResource1"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:TemplateField>
                            <asp:BoundField HeaderText="年度" DataField="YEARS" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" >
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:BoundField>                  
                            <asp:BoundField HeaderText="月份" DataField="MONTHS" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" >
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:BoundField>             
                            <asp:BoundField HeaderText="週次" DataField="WEEKS" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" >
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:BoundField>                      
                            <asp:BoundField HeaderText="日期" DataField="DATES" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" >
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:BoundField>                    
                            <asp:BoundField HeaderText="業務" DataField="SELLER" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" >
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:BoundField>                                   
                            <asp:BoundField HeaderText="金額" DataField="MONEYS" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" >
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:BoundField>                       
                    </Columns>
                </Fast:Grid>
            </td>
        </tr>
    </table>
</asp:Content>

