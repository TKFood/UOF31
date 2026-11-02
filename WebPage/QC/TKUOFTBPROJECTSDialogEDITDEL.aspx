<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKUOFTBPROJECTSDialogEDITDEL.aspx.cs" Inherits="CDS_WebPage_TKUOFTBPROJECTSDialogEDITDEL" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>
<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

<script>
    $(function () {
        $("#<%= txtDate1.ClientID %>").datepicker({ dateFormat: "yy/mm/dd", });
         $("#<%= txtDate2.ClientID %>").datepicker({ dateFormat: "yy/mm/dd", });
         $("#<%= txtDate3.ClientID %>").datepicker({ dateFormat: "yy/mm/dd", });
     });
  
</script>
    <div style="overflow-x:auto;width:800px">

        </div>
    <table class="PopTable" >
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TKUOFTBPROJECTSDialogEDITDEL"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan=2>
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>            
        </tr>
       <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label8" runat="server" Text="狀態"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:DropDownList ID="DropDownList1" runat="server" Width = "100%"></asp:DropDownList>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label2" runat="server" Text="專案編號"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="專案主旨"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="創建人員"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="建立部門"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label7" runat="server" Text="創建日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtDate1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label9" runat="server" Text="專案內容"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label10" runat="server" Text="預計結案期限"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtDate2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label11" runat="server" Text="舊有案件數"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label12" runat="server" Text="舊有案件列表"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label13" runat="server" Text="專案案件列表"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label14" runat="server" Text="累計案件數"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox9" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label15" runat="server" Text="結案說明"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label16" runat="server" Text="結案日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtDate3" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
    
    </table>
     <table class="PopTable">
         <tr >
            <td colspan="2" class="PopTableRightTD" >
                <div style="overflow-x:auto;width:100%">
                    <Fast:Grid ID="Grid1" runat="server"  OnRowDataBound="Grid1_RowDataBound" OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" >
                        <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                            <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                            <Columns>
                               
                                <asp:BoundField HeaderText="專案編號" DataField="QCFrm002PN" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField> 
                                 <asp:BoundField HeaderText="表單編號" DataField="QCFrm002SN" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                </asp:BoundField> 
                                 <asp:BoundField HeaderText="表單主旨" DataField="QCFrm002QCC" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                    <ItemStyle HorizontalAlign="Center" Width="300px"></ItemStyle>
                                </asp:BoundField> 
                                <asp:TemplateField HeaderText="更新備註" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >                
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnName" runat="server" Text='<%# Bind("QCFrm002SN") %>' meta:resourcekey="lbtnNameResource1"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="300px"></ItemStyle>
                                </asp:TemplateField> 
                            </Columns>
                        </Fast:Grid>
                    </div>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label3" runat="server" Text="是否刪除"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                  <asp:Button ID="btn1" runat="server" Text="刪除" OnClientClick="return btn1_Click(this)" 
                    onclick="btn1_Click" meta:resourcekey="btn1Resource1" />
            </td> 
        </tr>
    </table>
 
</asp:Content>

