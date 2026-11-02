<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerFileVersionManagement.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_CustomerFileVersionManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script>

</script>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
        <ContentTemplate>
            <div>
                <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
