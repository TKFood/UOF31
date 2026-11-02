<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="PowerBIMaintain.aspx.cs" Inherits="CDS_PowerBI_Setting_PowerBIMaintain" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Ede" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function RadToolBar2_Clinking(sender, args) {

            switch (args.get_item().get_value()) {
                case "Insert":
                    args.set_cancel(true);
                    $uof.dialog.open2("~/CDS/PowerBI/Setting/ModifyPowerBI.aspx",
                        args.get_item(),
                        "新增維護視窗",
                        600,
                        400,
                        openDialogResult,
                        {
                            Mode: 'Insert',
                            ReportID: '',
                            ApplicationID: $('[id$=hfApplicationID]').val(),
                            WorkSpaceID: ''
                        });
                    break;
                case "Delete":
                    if (confirm('確認刪除？') == false) {
                        args.set_cancel(true);
                    }
                    break;
            }
        };

        function openDialogResult(returnValue) {
            if (typeof (returnValue) == "undefined") {
                return false;
            }
            return true;
        };
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1" OnTabClick="RadTabStrip1_TabClick">
                <Tabs>
                    <telerik:RadTab runat="server" Text="應用程式設定" Selected="True" />
                    <telerik:RadTab runat="server" Text="報表設定" />
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="RadPageView1" runat="server">
                    <telerik:RadToolBar ID="RadToolBar1" runat="server" Width="100%" OnButtonClick="RadToolBar1_ButtonClick" ValidationGroup="VG1">
                        <Items>
                            <telerik:RadToolBarButton runat="server" CheckedImageUrl="~/Common/Images/Icon/icon_m02.gif"
                                DisabledImageUrl="~/Common/Images/Icon/icon_m02.gif" HoveredImageUrl="~/Common/Images/Icon/icon_m02.gif"
                                ImageUrl="~/Common/Images/Icon/icon_m02.gif" Text="存檔" Value="Save">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true" />
                        </Items>
                    </telerik:RadToolBar>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="VG1" />
                    <table cellpadding="0" cellspacing="1" class="PopTable" style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblApplicationID" runat="server" Text="應用程式代碼GUID:"></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField ID="hfApplicationID" runat="server" />
                                <asp:TextBox ID="txtApplicationID" runat="server" Text="" Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ErrorMessage="請輸入應用程式代碼GUID！" Display="None"
                                    ControlToValidate="txtApplicationID"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblApplicationDesc" runat="server" Text="應用程式名稱:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApplicationDesc" runat="server" Text="" Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ErrorMessage="請輸入應用程式名稱！" Display="None"
                                    ControlToValidate="txtApplicationDesc"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblUserName" runat="server" Text="帳戶:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserName" runat="server" Text="" Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ErrorMessage="請輸入帳戶！" Display="None"
                                    ControlToValidate="txtUserName"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPassword" runat="server" Text="密碼:" Width="100%"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword" runat="server" Text="" TextMode="Password" Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ErrorMessage="請輸入密碼！" Display="None"
                                    ControlToValidate="txtPassword"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAuthorityUrl" runat="server" Text="AuthorityUrl:" Width="100%"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAuthorityUrl" runat="server" Text="" Width="100%" ></asp:TextBox>
                                <asp:Label ID="lblAuthorityUrl2" runat="server" Text="" Width="100%"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ErrorMessage="請輸入AuthorityUrl！" Display="None"
                                    ControlToValidate="txtAuthorityUrl"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblResourceUrl" runat="server" Text="ResourceUrl:" Width="100%"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtResourceUrl" runat="server" Text="" Width="100%" Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ErrorMessage="請輸入ResourceUrl！" Display="None"
                                    ControlToValidate="txtResourceUrl"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblApiUrl" runat="server" Text="ApiUrl:" Width="100%"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApiUrl" runat="server" Text="" Width="100%" Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                    ErrorMessage="請輸入ApiUrl！" Display="None"
                                    ControlToValidate="txtApiUrl"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <telerik:RadToolBar ID="RadToolBar2" runat="server" Width="100%"
                        OnClientButtonClicking="RadToolBar2_Clinking"
                        OnButtonClick="RadToolBar2_ButtonClick" Visible="false">
                        <Items>
                            <telerik:RadToolBarButton runat="server" CheckedImageUrl="~/Common/Images/Icon/icon_m02.gif"
                                DisabledImageUrl="~/Common/Images/Icon/icon_m02.gif" HoveredImageUrl="~/Common/Images/Icon/icon_m02.gif"
                                ImageUrl="~/Common/Images/Icon/icon_m02.gif" Text="新增" Value="Insert">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true" />
                            <telerik:RadToolBarButton runat="server" CheckedImageUrl="~/Common/Images/Icon/icon_m03.gif"
                                DisabledImageUrl="~/Common/Images/Icon/icon_m03.gif" HoveredImageUrl="~/Common/Images/Icon/icon_m03.gif"
                                ImageUrl="~/Common/Images/Icon/icon_m03.gif" Text="刪除" Value="Delete">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton runat="server" IsSeparator="true" />
                        </Items>
                    </telerik:RadToolBar>
                    <Ede:Grid ID="Grid1" runat="server"
                        AutoGenerateColumns="False"
                        AutoGenerateCheckBoxColumn="False"
                        AllowSorting="False" PageSize="15"
                        OnPageIndexChanging="Grid1_PageIndexChanging"
                        OnSorting="Grid1_Sorting"
                        OnRowDataBound="Grid1_RowDataBound"
                        CustomDropDownListPage="False"
                        DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending"
                        EmptyDataText="No data found" EnhancePager="False" KeepSelectedRows="False"
                        SelectedRowColor="" UnSelectedRowColor="" DataKeyNames="ApplicationID,WorkSpaceID,ReportID">
                        <EnhancePagerSettings FirstAltImageUrl="" FirstImageUrl="" LastAltImage="" LastImageUrl=""
                            NextIAltImageUrl="" NextImageUrl="" PageInfoCssClass="" PageNumberCssClass="" PageNumberCurrentCssClass=""
                            PageRedirectCssClass="" PreviousAltImageUrl="" PreviousImageUrl="" ShowHeaderPager="True" />
                        <ExportExcelSettings AllowExportToExcel="False" />
                        <Columns>
                            <asp:TemplateField HeaderText="報表代碼" SortExpression="ReportID">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnReportID" runat="server" Text='<%# Bind("ReportID") %>' OnClick="lbtnReportID_Click"></asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hfApplicationID" Value='<%# Bind("ApplicationID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="報表說明" SortExpression="ReportDesc">
                                <ItemTemplate>
                                    <asp:Label ID="lblReportDesc" runat="server" Text='<%# Bind("ReportDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="程式代碼" SortExpression="ReportNO">
                                <ItemTemplate>
                                    <asp:Label ID="lblReportNO" runat="server" Text='<%# Bind("ReportNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="工作區代碼" SortExpression="WorkSpaceID">
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkSpaceID" runat="server" Text='<%# Bind("WorkSpaceID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Ede:Grid>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <asp:Label ID="lblSaveSuccess" runat="server" Text="存檔成功。" Visible="false"></asp:Label>
            <asp:Label ID="lblSaveError" runat="server" Text="存檔失敗。" Visible="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

