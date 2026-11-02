<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QickCheckCompareVersion_Simple.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_QickCheckCompareVersion_Simple" %>

<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
<%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function OnClientFilesUploaded(sender, args) {
            __doPostBack('rauVersionListUpload', args);
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rauVersionListUpload" />
        </Triggers>
        <ContentTemplate>
            <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
            <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
            <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
            <link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
            <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
            <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
            <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
            <div style="padding: 10px 20px;">
                <div runat="server" id="divChoiceList">
                    <div>
                        <asp:DropDownList runat="server" ID="ddlKYTLists" AutoPostBack="true" OnSelectedIndexChanged="ddlKYTLists_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                    <div>
                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="rauVersionListUpload" MultipleFileSelection="Disabled"  Width="500px" OnFileUploaded="rauVersionListUpload_FileUploaded" OnClientFilesUploaded="OnClientFilesUploaded">
                            <Localization Select="上傳檔案" />
                        </telerik:RadAsyncUpload>
                        <telerik:RadProgressArea RenderMode="Lightweight" runat="server" ID="RadProgressArea1" />
                    </div>
                <div>
                    <%--<asp:LinkButton ID="lbtnRefresh" runat="server" OnClick="lbtnRefresh_Click">更新自己的版本</asp:LinkButton>--%>
                    <asp:LinkButton ID="lbtnReVerify" runat="server" OnClick="lbtnReVerify_Click" class="btn btn-outline-primary btn-sm">重新驗證版本</asp:LinkButton>
                    <asp:LinkButton ID="lbtnExport" runat="server" OnClick="lbtnExport_Click" class="btn btn-outline-success  btn-sm">輸出KYTLIST檔案</asp:LinkButton>
                </div>
                <div>
                    <div class="row" style="overflow: auto;">
                        <uc1:KYTGridView ID="gvFILE_VERSION" runat="server"
                            CssClass="tsGridView2" Width="100%"
                            ShowHeader="true" ShowHeaderWhenEmpty="true"
                            AutoGenerateColumns="false"
                            ForeColor="#333333"
                            OnRowDataBound="gvFILE_VERSION_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="檔案名稱">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtFileName" Width="120px" FieldName="FILE_NAME" Text='<%#Bind("FILE_NAME")%>' ViewType="ReadOnly" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="最後驗證時間">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtMODIFY_TIME" Width="120px" FieldName="MODIFY_TIME" Text='<%#Bind("MODIFY_TIME")%>' ViewType="ReadOnly" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="版本號碼">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtFILE_VERSION" Width="120px" FieldName="FILE_VERSION" Text='<%#Bind("FILE_VERSION")%>' ViewType="ReadOnly" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="版本雜湊值">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtFILE_SHA" Width="120px" FieldName="FILE_SHA" Text='<%#Bind("FILE_SHA")%>' ViewType="ReadOnly" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="比對雜湊值">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtFILE_COMPARE_SHA" Width="120px" FieldName="FILE_COMPARE_SHA" Text='<%#Bind("FILE_COMPARE_SHA")%>' ViewType="ReadOnly" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </uc1:KYTGridView>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
