<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCHTBPROJECTDialogADD.aspx.cs" Inherits="CDS_WebPage_TKRESEARCHTBPROJECTDialogADDD" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

<script>
    $(function () {
       
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
                <asp:TextBox ID="TextBox1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label3" runat="server" Text="專案名稱"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="線上校稿完成"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="紙本簽核完成"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="備註"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>

      

    </table>

 
</asp:Content>

