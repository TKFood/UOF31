<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="ModifyPowerBI.aspx.cs" Inherits="CDS_PowerBI_Setting_ModifyPowerBI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <table cellpadding="0" cellspacing="1" class="PopTable" style="width: 100%">
                <tr>
                    <td style="width: 30%;">
                        <asp:HiddenField ID="hfApplicationID" runat="server" Value="" />
                        <asp:Label ID="Label10" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblReportID" runat="server" Text="報表代碼"></asp:Label>
                    </td>
                    <td>
                        <asp:HiddenField ID="hfReportID" runat="server" Value="" />
                        <asp:TextBox ID="txtReportID" runat="server" Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ErrorMessage="報表代碼必填!" ControlToValidate="txtReportID" Display="None">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblReportDesc" runat="server" Text="報表說明"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReportDesc" runat="server" Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                            ErrorMessage="報表說明必填!" ControlToValidate="txtReportDesc"
                            Display="None">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblReportNO" runat="server" Text="程式代碼"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReportNO" runat="server" Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                            runat="server" ErrorMessage="程式代碼必填!" ControlToValidate="txtReportNO"
                            Display="None">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblWorkSpaceID" runat="server" Text="工作區代碼"></asp:Label>
                    </td>
                    <td>
                        <asp:HiddenField ID="hfWorkSpaceID" runat="server" Value="" />
                        <asp:TextBox ID="txtWorkSpaceID" runat="server" Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ErrorMessage="工作區代碼必填!" ControlToValidate="txtWorkSpaceID"
                            Display="None">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="cvData" runat="server" ErrorMessage="資料不可重複"></asp:CustomValidator>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

