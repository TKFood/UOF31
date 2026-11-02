<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKWEBDialogEDITDEL.aspx.cs" Inherits="CDS_WebPage_Dialog" %>

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
                <asp:TextBox ID="txtTextBox1" runat="server" ></asp:TextBox>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="月份"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox2" runat="server"></asp:TextBox>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="週次"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox3" runat="server"></asp:TextBox>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox4" runat="server"></asp:TextBox>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label7" runat="server" Text="業務員"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox5" runat="server"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label8" runat="server" Text="金額"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="txtTextBox6" runat="server" ></asp:TextBox>
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

