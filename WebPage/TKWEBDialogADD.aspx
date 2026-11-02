<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKWEBDialogADD.aspx.cs" Inherits="CDS_WebPage_Dialog" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script>
        
</script>

    <table class="PopTable" >
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label1" runat="server" Text="TKWEBDialogADD"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan=2>
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>            
        </tr>
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label2" runat="server" Text="年度"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox1" runat="server" Text="2020"></asp:TextBox>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="月份"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox2" runat="server" Text="5"></asp:TextBox>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="週次"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox3" runat="server"  Text="18"></asp:TextBox>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox4" runat="server" Text="20200508"></asp:TextBox>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label7" runat="server" Text="業務員"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox5" runat="server"  Text="JJ"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label8" runat="server" Text="金額"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox6" runat="server"  Text="10000"></asp:TextBox>
            </td> 
       </tr>

    </table>

 
</asp:Content>

