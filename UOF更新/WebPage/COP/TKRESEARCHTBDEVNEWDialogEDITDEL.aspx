<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCHTBDEVNEWDialogEDITDEL.aspx.cs" Inherits="CDS_WebPage_TKRESEARCHTBDEVNEWDialogEDITDEL" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

<script>
    $(function () {
       

    }); 
  
</script>
    <div style="overflow-x:auto;width:800px">

        </div>
    <table class="PopTable" >
            <tr>
                <td class="PopTableLeftTD" >
                    <asp:Label ID="Label1" runat="server" Text="TKRESEARCHTBDEVNEWDialogEDITDEL"></asp:Label>
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
                <asp:Label ID="Label20" runat="server" Text="接單日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                  <telerik:RadDatePicker ID="RadDatePicker1"  runat="server" Width = "120px"></telerik:RadDatePicker>
                 <%--<asp:TextBox ID="TDATES1" runat="server"  Text="" Width = "100%"></asp:TextBox>--%>
            </td> 
       </tr>
                  <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label3" runat="server" Text="口味名稱"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label2" runat="server" Text="客戶"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label17" runat="server" Text="負責業務"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server"  Text=""  Width = "100%"></asp:TextBox>
            </td> 
       </tr>  

        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="需求數量"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
             <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="最新試做日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                  <telerik:RadDatePicker ID="RadDatePicker2"  runat="server" Width = "120px"></telerik:RadDatePicker>
                 <%--<asp:TextBox ID="TDATES2" runat="server"  Text="" Width = "100%"></asp:TextBox>--%>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="最新試做結果"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
    </table>
     <table>
            <tr>
               <%-- <td class="PopTableLeftTD" >
                    <asp:Label ID="Label7" runat="server" Text="是否刪除"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                      <asp:Button ID="btn1" runat="server" Text="刪除" ForeColor="Red" OnClientClick="return btn1_Click(this)" 
                        onclick="btn1_Click" meta:resourcekey="btn1Resource1" />
                </td> --%>
            </tr>
    </table>
</asp:Content>

