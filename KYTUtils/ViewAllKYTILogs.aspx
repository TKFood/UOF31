<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewAllKYTILogs.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_ViewAllKYTILogs" %>

<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
            <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
            <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
            <div>
                <div>
                    <asp:DropDownList ID="ddlAllDDLConfig" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAllDDLConfig_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Size="XX-Large"></asp:Label>

                </div>
                <div>
                    <div>選擇LOG日期</div>
                    <div>
                        <%--<uc1:KYTDatePicker runat="server" ID="kytdpLocation" ViewType="Input" AutoPostBack="true" OnTextChanged="kytdpLocation_TextChanged" />--%>
                        <telerik:RadDatePicker runat="server" ID="rdpLocation" AutoPostBack="true" OnSelectedDateChanged="rdpLocation_SelectedDateChanged"></telerik:RadDatePicker>
                    </div>
                    <div>
                        <asp:DropDownList ID="ddlAllFiles" runat="server"></asp:DropDownList>
                        <asp:Button ID="btnRead" runat="server" Text="讀取檔案" OnClick="btnRead_Click" />
                    </div>
                </div>
                <div>
                    <asp:TextBox ID="txtLogMsg" runat="server" TextMode="MultiLine" Width="100%" Rows="20" Enabled="false"></asp:TextBox>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
