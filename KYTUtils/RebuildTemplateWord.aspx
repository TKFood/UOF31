<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebuildTemplateWord.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_RebuildTemplateWord" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
    <%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
    <%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
    <%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
    <%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
    <%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
    <%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
    <%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
    <%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>
    <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
    <link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
    <script>
        function OnClientFilesUploaded(sender, args) {
            __doPostBack('rauDOCXUpload', args);
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rauDOCXUpload" />
        </Triggers>
        <ContentTemplate>
            <div style="padding: 10px 20px;">
                <div>
                    <asp:CheckBox runat="server" ID="chkForce" Text="檔案內除了參數外不包含任何[]" />
                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="rauDOCXUpload" MultipleFileSelection="Disabled" AllowedFileExtensions="doc,docx" Width="500px" OnFileUploaded="rauDOCXUpload_FileUploaded" OnClientFilesUploaded="OnClientFilesUploaded">
                        <Localization Select="上傳檔案" />
                    </telerik:RadAsyncUpload>
                    <telerik:RadProgressArea RenderMode="Lightweight" runat="server" ID="RadProgressArea1" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
