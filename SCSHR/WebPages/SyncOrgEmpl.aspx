<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SyncOrgEmpl.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="cds_SCSHR_WebPages_SyncOrgEmpl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .btnStart {
            margin-top: 20px;
            margin-left: 20px;
        }
    </style>
    <script>
        function doStart() {
            showMsg('同步中請稍候');
        }
        function showMsg(msg) {
            $('#<%=lblMsg.ClientID%>').html(msg);
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="btnStart">
                <div>
                    <telerik:RadButton runat="server" Text="開始同步" ID="btnStart" OnClick="btnStart_Click" OnClientClicked="doStart"></telerik:RadButton>
                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
