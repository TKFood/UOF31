<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TerminateTasks.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_TerminateTasks" %>

<%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceList.ascx" TagPrefix="uc1" TagName="UC_ChoiceList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .tblClass > tbody > tr > td {
            border: 1px solid #DEDEDE;
            padding: 5px;
            height: 25px;
        }

            .tblClass > tbody > tr > td:first-child {
                text-align: right;
            }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <table>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlTasks" runat="server"></asp:DropDownList><asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadButton runat="server" ID="btnTerminate" Text="作廢未結案表單" OnClick="btnTerminate_Click"></telerik:RadButton>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
