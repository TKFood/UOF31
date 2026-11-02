<%@ Control Language="C#" AutoEventWireup="true" CodeFile="optionField_FINDRANKUC.ascx.cs" Inherits="WKF_OptionalFields_optionField_FINDRANKUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>

<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>


<div  style="width:100%;height:100%;border:3px #cccccc dashed;">
    <table width="100%" class="" cellspacing="1">
        <tr class="">
            <td class="">
                 <asp:Label ID="Label1" runat="server" Text="請填寫工號: " ></asp:Label>
                 <asp:TextBox ID="TextBox1" runat="server" Text="" AutoPostBack="true" OnTextChanged="TextBox1_TextChanged" ></asp:TextBox>
            </td>
             <td>
                <asp:Label ID="Label3" runat="server" Text="姓名: " ></asp:Label>
                <asp:TextBox ID="TextBox3" runat="server" Text="" ReadOnly="true" ></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="職級: " ></asp:Label>
                <asp:TextBox ID="TextBox2" runat="server" Text="" ReadOnly="true" ></asp:TextBox>
            </td>
        </tr>
        </table>
    </div>