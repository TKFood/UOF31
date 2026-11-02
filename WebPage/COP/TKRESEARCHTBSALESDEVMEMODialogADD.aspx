<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCHTBSALESDEVMEMODialogADD.aspx.cs" Inherits="CDS_WebPage_TKRESEARCHTBSALESDEVMEMODialogADD" %>

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
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TKRESEARCHTBSALESDEVMEMODialogADD"></asp:Label>
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
                <asp:Label ID="Label2" runat="server" Text="客戶"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label3" runat="server" Text="產品品項"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label4" runat="server" Text="末售"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label5" runat="server" Text="促銷設定"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label6" runat="server" Text="規格及屬性"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label7" runat="server" Text="產品效期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label9" runat="server" Text="通路"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label10" runat="server" Text="預估上市說明"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server"  Text="" Width = "100%"></asp:TextBox>
            </td> 
       </tr>
                  <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label20" runat="server" Text="預估上市日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <telerik:RadDatePicker ID="RadDatePicker4"  runat="server" Width = "120px"></telerik:RadDatePicker>
            </td> 
       </tr>
             <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label11" runat="server" Text="產品圖/樣袋 完稿日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox9" runat="server"  Text=""  Width = "100%"></asp:TextBox>
            </td> 
       </tr>
         <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label12" runat="server" Text="可行性評估 申請日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                  <telerik:RadDatePicker ID="RadDatePicker1"  runat="server" Width = "120px"></telerik:RadDatePicker>
            </td> 
       </tr>
               <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label13" runat="server" Text="成本試算 申請日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                  <telerik:RadDatePicker ID="RadDatePicker2"  runat="server" Width = "120px"></telerik:RadDatePicker>
            </td> 
       </tr>        
             <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label14" runat="server" Text="報價日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server"  Text=""  Width = "100%"></asp:TextBox>
            </td> 
       </tr>
            <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label15" runat="server" Text="營標送驗"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox11" runat="server"  Text=""  Width = "100%"></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label16" runat="server" Text="營標送驗 申請日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                 <telerik:RadDatePicker ID="RadDatePicker3"  runat="server" Width = "120px"></telerik:RadDatePicker>
            </td> 
       </tr>    
        <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label21" runat="server" Text="業務樣本確認日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox12" runat="server"  Text=""  Width = "100%"></asp:TextBox>
            </td> 
       </tr>  
             <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label17" runat="server" Text="負責業務"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox13" runat="server"  Text=""  Width = "100%"></asp:TextBox>
            </td> 
       </tr>  
             <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label18" runat="server" Text="業務進度"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox14" runat="server"  Text="" TextMode="MultiLine" Width = "100%" ></asp:TextBox>
            </td> 
       </tr>
          <tr>
            <td class="PopTableLeftTD" >
                <asp:Label ID="Label19" runat="server" Text="研發進度"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox15" runat="server"  Text="" TextMode="MultiLine" Width = "100%" ></asp:TextBox>
            </td> 
       </tr>

    </table>

 
</asp:Content>

