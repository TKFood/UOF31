<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateKYTJSONStructUsedInStartForm.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_CreateKYTJSONStructUsedInStartForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:TextBox ID="txtFormName" runat="server" placeHolder="表單名稱"></asp:TextBox>
                <asp:TextBox ID="txtFieldID" runat="server" placeHolder="外掛表單代碼FieldID"></asp:TextBox>
                <asp:TextBox ID="txtFromNumber" runat="server" placeHolder="必須先申請一張單，輸入單號"></asp:TextBox>

                <asp:Button ID="btnCreate" runat="server" Text="建立" OnClick="btnCreate_Click" />
            </div>
            <div>
                表單版本： <asp:Label ID="lblFormVersion" runat="server" Text=""></asp:Label>
                <br />
                <asp:TextBox ID="txtShowStruct" runat="server" TextMode="MultiLine" Width="100%" Rows="25"></asp:TextBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
