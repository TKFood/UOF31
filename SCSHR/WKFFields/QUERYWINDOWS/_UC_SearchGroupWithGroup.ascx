<%@ Control Language="C#" AutoEventWireup="true" CodeFile="_UC_SearchGroupWithGroup.ascx.cs" Inherits="CDS_SCSHR_WKFFields_QUERYWINDOW_UC_SearchGroupWithGroup" %>
<asp:Button ID="Button1" runat="server" Text="Button" CausesValidation="false" OnClick="Button1_Click" Visible="true" />
<div style="display: none">
    <asp:HiddenField runat="server" ID="hidSelectedUser" />
    <asp:HiddenField runat="server" ID="hidGroup" Value="Company" />
    <asp:HiddenField runat="server" ID="hidTitle" Value="選擇員工" />
    <asp:HiddenField runat="server" ID="hidWidth" Value="800" />
    <asp:HiddenField runat="server" ID="hidHeight" Value="530" />
    <asp:HiddenField runat="server" ID="hidButtonText" Value="選擇員工" />
    <asp:HiddenField runat="server" ID="hidIsGroup" Value="N" />
    <asp:HiddenField runat="server" ID="hidIsSingle" Value="N" />
    <asp:HiddenField runat="server" ID="hidGroupCS" />
    <asp:HiddenField runat="server" ID="hidGroupSQL" />
    <asp:HiddenField runat="server" ID="hidUserCS" />
    <asp:HiddenField runat="server" ID="hidUserSQL" />
</div>
