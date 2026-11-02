<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKUOFTBPROJECTSDialogADD.aspx.cs" Inherits="CDS_WebPage_TKUOFTBPROJECTSDialogADD" %>

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

    <table class="PopTable" >
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TKRESEARCHTBPROJECTDialogADDD"></asp:Label>
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
                <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true"  Text="自動編號" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label3" runat="server" Text="專案主旨"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="創建人員"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="建立部門"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="創建日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtDate1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label7" runat="server" Text="專案內容"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label9" runat="server" Text="預計結案期限"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtDate2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label10" runat="server" Text="舊有案件數"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label11" runat="server" Text="舊有案件列表"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label12" runat="server" Text="專案案件列表"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label13" runat="server" Text="累計案件數"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox9" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr> 
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label14" runat="server" Text="結案說明"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label15" runat="server" Text="結案日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtDate3" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
      

    </table>

 
</asp:Content>

