<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_COPCONDTIONSDialogADD.aspx.cs" Inherits="CDS_WebPage_TBBU_COPCONDTIONSDialogADD" %>

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
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TBBU_COPCONDTIONSDialogEDITDEL"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan=2>
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>            
        </tr>
       
    </table>
    <table>      
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label2" runat="server" Text="客戶代號"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label3" runat="server" Text="客戶名稱"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="連絡人"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="電話1"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
              <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="EMAIL"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
               <%-- <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label7" runat="server" Text="電話2"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>--%>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label8" runat="server" Text="採購單(附單)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label9" runat="server" Text="銷貨單(附單)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label10" runat="server" Text="是否顯$(附單)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label11" runat="server" Text="發票(附單)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox9" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label12" runat="server" Text="麥頭(附單)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label13" runat="server" Text="允收期限"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox11" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label14" runat="server" Text="收款條件"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox12" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label15" runat="server" Text="寄送地址"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox13" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label16" runat="server" Text="備註" Height="300%" ></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox14" runat="server"  Text="" Width = "100%"  Height="120px" TextMode="MultiLine" ></asp:TextBox>
            </td> 
       </tr>

    </table>
            
    
 
</asp:Content>

