<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateSQLScript.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_CreateSQLScript" %>

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
            __doPostBack('rauXLSXUpload', args);
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rauXLSXUpload" />
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
                <div>

                    <asp:DropDownList ID="ddlMotion" runat="server">
                        <asp:ListItem Value="Create">新增</asp:ListItem>
                        <asp:ListItem Value="Drop">移除</asp:ListItem>
                        <asp:ListItem Value="Create_Drop">移除/新增</asp:ListItem>
                        <asp:ListItem Value="Modify_Update">變更-修改</asp:ListItem>
                        <asp:ListItem Value="Modify_Ins">變更-新增</asp:ListItem>
                        <asp:ListItem Value="Modify_Drop">變更-移除</asp:ListItem>
                    </asp:DropDownList>

                    <asp:LinkButton ID="lbtnExport" runat="server" OnClick="lbtnExport_Click">輸出</asp:LinkButton>
                </div>
                <div>
                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="rauXLSXUpload" MultipleFileSelection="Disabled" AllowedFileExtensions="xls,xlsx" Width="500px" OnFileUploaded="rauXLSXUpload_FileUploaded" OnClientFilesUploaded="OnClientFilesUploaded">
                        <Localization Select="上傳檔案" />
                    </telerik:RadAsyncUpload>
                    <telerik:RadProgressArea RenderMode="Lightweight" runat="server" ID="RadProgressArea1" />
                    <div>
                        <asp:LinkButton ID="lbtnDownloadSample" runat="server" OnClick="lbtnDownloadSample_Click">下載範例檔</asp:LinkButton>
                    </div>
                </div>
                <div>
                    <uc1:KYTTextBox runat="server" ID="ktxtShowScript" Width="100%" TextMode="MultiLine" Rows="5" ViewType="Input" />
                </div>
                <div>
                    <div>
                        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click">新增</asp:LinkButton>

                    </div>
                    表格名稱：<uc1:KYTTextBox runat="server" ID="ktxtTABLE_NAME" Width="200px" ViewType="Input" />
                    <div class="row" style="overflow: auto;">
                        <uc1:KYTGridView ID="gvColumns" runat="server"
                            CssClass="tsGridView2" Width="100%"
                            ShowHeader="true" ShowHeaderWhenEmpty="true"
                            AutoGenerateColumns="false"
                            ForeColor="#333333"
                            OnRowDataBound="gvColumns_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="鍵值">
                                    <ItemTemplate>
                                        <uc1:KYTCheckBox runat="server" ID="kcbPK" CheckedValue="Y" FieldName="PK" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="欄位名稱">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtCOLUMN_NAME" Width="120px" FieldName="COLUMN_NAME" Text='<%#Bind("COLUMN_NAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="欄位類型">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtDATA_TYPE" Width="120px" FieldName="DATA_TYPE" Text='<%#Bind("DATA_TYPE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="允許NULL">
                                    <ItemTemplate>
                                        <uc1:KYTCheckBox runat="server" ID="kcbALLOW_NULL" CheckedValue="Y" FieldName="ALLOW_NULL" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="預設值">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtDEFAULT_VALUE" Width="120px" FieldName="DEFAULT_VALUE" Text='<%#Bind("DEFAULT_VALUE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="說明">
                                    <ItemTemplate>
                                        <uc1:KYTTextBox runat="server" ID="ktxtCOLUMN_DESC" Width="200px" FieldName="COLUMN_DESC" Text='<%#Bind("COLUMN_DESC")%>' />
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
