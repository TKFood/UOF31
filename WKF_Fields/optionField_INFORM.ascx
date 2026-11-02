<%@ Control Language="C#" AutoEventWireup="true" CodeFile="optionField_INFORM.ascx.cs" Inherits="WKF_OptionalFields_optionField_INFORM" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>



<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>

<div style="width: 100%; height: 100%; border: 3px #cccccc dashed;">
    <table cellpadding="8" width="100%" class="">
        <tr>
            <td  style="font-size:20pt">請下拉選擇要通知的人員</td>
        </tr>
        <tr>
            <td style="font-size:20pt">
                <asp:DropDownList style="padding: 0;" ID="DropDownList1" runat="server" ></asp:DropDownList>
            </td>
            <tr>
                <td style="padding: 8px;font-size:20pt;">
                    <asp:Button ID="Button1" runat="server" Style="width: 200px; height: 50px; background-color: palevioletred;" Text="傳送通知" OnClick="Button1_Click" OnClientClick="this.disabled = true; this.value = '已通知';" UseSubmitBehavior="false" />
                </td>
            </tr>          
        <tr>
            <td style="padding: 8px;font-size:20pt;">

            </td>
        </tr>
    </table>
</div>
