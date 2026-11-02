<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TKUOFtb_COMPANYCOMPANY_ID.ascx.cs" Inherits="WKF_OptionalFields_TKUOFtb_COMPANYCOMPANY_ID" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>

<table style="border-collapse:separate; border-spacing:0 10px; margin-top:-15px;">
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="請輸入客戶代號: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="查詢" OnClientClick="return btn_Click(this)"  onclick="btn_Click"/>
        </td>
        <td>
            
        </td>
    </tr>
   <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text=" 客戶名稱: "></asp:Label>
        </td>
        <td>
            <asp:Label ID="LabelNAME" runat="server" Text=""></asp:Label>
        </td>
   </tr>
   <tr>
        <td>
            <asp:Label ID="Label18" runat="server" Text="業務人員(工號): "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox15" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="資本額: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox2" runat="server" Text="0"></asp:TextBox>
        </td>    
    </tr>
      <tr>
        <td>
            <asp:Label ID="Label16" runat="server" Text="負責人: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox13" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
       <tr>
        <td>
            <asp:Label ID="Label17" runat="server" Text="連絡人: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox14" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
    <tr>
        <td>
            交易幣別
        </td>
        <td>
            <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            部門別
        </td>
        <td>
            <asp:DropDownList ID="DropDownList2" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            通路別
        </td>
        <td>
            <asp:DropDownList ID="DropDownList3" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            型態別
        </td>
        <td>
            <asp:DropDownList ID="DropDownList12" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
     <tr>
        <td>
            地區別
        </td>
        <td>
            <asp:DropDownList ID="DropDownList4" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            國家別
        </td>
        <td>
            <asp:DropDownList ID="DropDownList5" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="銷售評等: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox3" runat="server" Text="A"></asp:TextBox>
        </td>    
    </tr>
    <tr>
        <td>
            付款條件
        </td>
        <td>
            <asp:DropDownList ID="DropDownList6" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
            <asp:Label ID="Label12" runat="server" Text=""></asp:Label>
        </td>      
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Text="信用額度: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox4" runat="server" Text="0"></asp:TextBox>
        </td>    
    </tr>
    <tr>
        <td>
            發票聯數
        </td>
        <td>
            <asp:DropDownList ID="DropDownList7" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
     <tr>
        <td>
            課稅別
        </td>
        <td>
            <asp:DropDownList ID="DropDownList8" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label6" runat="server" Text="郵遞區號: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox5" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
     <tr>
        <td>
            收款方式
        </td>
        <td>
            <asp:DropDownList ID="DropDownList9" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
     <tr>
        <td>
            票據寄領
        </td>
        <td>
            <asp:DropDownList ID="DropDownList10" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
   <tr>
        <td>
            <asp:Label ID="Label7" runat="server" Text="結帳日期: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox6" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label8" runat="server" Text="付款銀行(一): "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox7" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label10" runat="server" Text="銀行帳號(一): "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox9" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
    <tr>
        <td>
            運輸方式
        </td>
        <td>
            <asp:DropDownList ID="DropDownList11" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>  
   
    <tr>
        <td>
            <asp:Label ID="Label11" runat="server" Text="取價順序 "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox10" runat="server" Text="142"></asp:TextBox>
            <asp:Label ID="Label13" runat="server" Text="(1客戶計價,2標準售價,3零售價,4售價定價一,5售價定價二): "></asp:Label>
        </td>    
    </tr>
    <tr>
        <td>
            隨貨附發票
        </td>
        <td>
            <asp:DropDownList ID="DropDownList13" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr> 
    <tr>
        <td>
            稅額
        </td>
        <td>
            <asp:DropDownList ID="DropDownList14" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr> 
     <tr>
        <td>
            <asp:Label ID="Label19" runat="server" Text="收貨部門: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox16" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
     <tr>
        <td>
            <asp:Label ID="Label14" runat="server" Text="收貨人: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox11" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
    <tr>
        <td>
            客戶分類
        </td>
        <td>
            <asp:DropDownList ID="DropDownList15" runat="server" AppendDataBoundItems="false"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label15" runat="server" Text="食品登錄字號: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox12" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
     <tr>
        <td>
            <asp:Label ID="Label9" runat="server" Text="備註: "></asp:Label>
        </td>      
        <td>
            <asp:TextBox ID="TextBox8" runat="server" Text=""></asp:TextBox>
        </td>    
    </tr>
</table>

<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>