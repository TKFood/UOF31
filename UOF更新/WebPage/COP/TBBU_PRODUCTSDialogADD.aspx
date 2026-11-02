<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_PRODUCTSDialogADD.aspx.cs" Inherits="CDS_WebPage_TBBU_PRODUCTSDialogADD" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

<script>
    $(function () {
      
    }); 
  
</script>
    <div style="overflow-x:auto;width:100%">

        </div>
    <table class="PopTable" >
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TBBU_PRODUCTSDialogADD"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan=2>
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>            
        </tr>
       
    </table>
    <table>      
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label2" runat="server" Text="品號"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label3" runat="server" Text="銷售重點"  ></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server"  Text=" "  Width = "200%" TextMode="MultiLine"  Row="5" style="height:120px;"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="銷售通路"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server"  Text=" " Width = "200%" TextMode="MultiLine"  Row="5"  style="height:120px;"></asp:TextBox>
            </td> 
       </tr>
                        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="建議售價"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server"  Text="0" Width = "200%" TextMode="MultiLine"  Row="5"  style="height:120px;"></asp:TextBox>
            </td> 
       </tr>
                        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="IP價"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="0" Width = "200%" TextMode="MultiLine"  Row="5"  style="height:120px;"></asp:TextBox>
            </td> 
       </tr>
                        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label7" runat="server" Text="DM價"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server"  Text="0" Width = "200%" TextMode="MultiLine"  Row="5"  style="height:120px;"></asp:TextBox>
            </td> 
       </tr>

       

    </table>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Text="DM"></asp:Label>
            </td>
        </tr>
    </table>
    
 
</asp:Content>

