<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_TBSALESSCHEDULESDialogEDITDEL.aspx.cs" Inherits="CDS_WebPage_TBBU_TBSALESSCHEDULESDialogEDITDEL" %>

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
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TBBU_TBSALESSCHEDULESDialogEDITDEL"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan=2>
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>            
        </tr>
       
    </table>
    <table>      
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label2" runat="server" Text="通路"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label3" runat="server" Text="經銷商"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="事件"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="1月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label7" runat="server" Text="2月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label8" runat="server" Text="3月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label9" runat="server" Text="4月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label10" runat="server" Text="5月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label11" runat="server" Text="6月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox9" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label12" runat="server" Text="7月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label13" runat="server" Text="8月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox11" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label14" runat="server" Text="9月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox12" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label15" runat="server" Text="10月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox13" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label16" runat="server" Text="11月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox14" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label17" runat="server" Text="12月"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox15" runat="server"  Text="" Width = "100%" TextMode="MultiLine"  Row="5" ></asp:TextBox>
            </td> 
       </tr>

    </table>
            
    <table>
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="是否刪除"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                  <asp:Button ID="Button1" runat="server" Text="刪除" forecolor="red"
                    onclick="btn1_Click" meta:resourcekey="btn1Resource1" />
            </td> 
        </tr>     
    </table>
 
</asp:Content>

