<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_RESIGN.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_RESIGN" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceList.ascx" TagPrefix="uc1" TagName="UC_ChoiceList" %>


<link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
<link href="<%=Page.ResolveUrl("~/CDS/SCSHR/Assets/css/SCSHR.css")%>" rel="stylesheet" />

<!--引用bootstrap -->
<link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>

<script>
    /**
     * 檢查離職原因
     * @param sender
     * @param args
     */
    function CheckREASONID(sender, args) {
        debugger;
        var kddlREASONID = $("#<%=kddlREASONID.ClientID%>_DropDownList1");
        if (kddlREASONID.length > 0) {
            args.IsValid = kddlREASONID.val() != null && kddlREASONID.val() != "";
        }
    }

    /**
     * 檢查預計離職日
     * @param sender
     * @param args
     */
    function CheckRESIGNDATE(sender, args) {
        var kdpRESIGNDATE = $("#<%=kdpRESIGNDATE.ClientID%>_textBox1");
        if (kdpRESIGNDATE.length > 0) {
            args.IsValid = kdpRESIGNDATE.val() != null && kdpRESIGNDATE.val() != "";
        }
    }
    /**
     * 檢查實際離職日
     * @param sender
     * @param args
     */
    function CheckRESIGNATIONDATE(sender, args) {
        var kdpRESIGNATIONDATE = $("#<%=kdpRESIGNATIONDATE.ClientID%>_textBox1");
        if (kdpRESIGNATIONDATE.length > 0) {
            args.IsValid = kdpRESIGNATIONDATE.val() != null && kdpRESIGNATIONDATE.val() != "";
        }
    }
    /**
     * 檢查離職原因說明
     * @param sender
     * @param args
     */
    function CheckNOTE(sender, args) {
        var ktxtNOTE = $("#<%=ktxtNOTE.ClientID%>_TextBox1");
        if (ktxtNOTE.length > 0) {
            args.IsValid = ktxtNOTE.val() != null && ktxtNOTE.val() != "";
        }
    }
</script>

<style>
    .msgColor {
        color: red;
    }
</style>

<!--飛騰離職資訊-->
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
    </Triggers>
    <ContentTemplate>

        <div class="container-fluid">
            <!--container-fluid-->
            <div class="row">
                <div class="col-md-12" style="background-color: #00CCFF; padding: 5px">
                    離職資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    部門
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" />
                    <!--自動帶入，唯讀-->
                    <asp:HiddenField ID="hidGROUPCODE" runat="server" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    申請日期
                    <!--SYS_DATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDATE" />
                    <!--自動帶入，唯讀，SYS_DATE-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    離職人員
                    <!--TMP_EMPLOYEEID-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtRESIGNEMP" />
                    <!--自動帶入，申請人，顯示姓名與帳號-->
                    <asp:HiddenField runat="server" ID="hidRESIGNEMPTitleId" />
                    <!--自動帶入，職稱ID-->
                    <asp:HiddenField runat="server" ID="hidRESIGNEMPTitleName" />
                    <!--自動帶入，職稱名稱-->
                    <asp:HiddenField runat="server" ID="hidRESIGNEMPAccount" />
                    <!--自動帶入，帳號，TMP_EMPLOYEEID-->
                    <asp:HiddenField runat="server" ID="hidRESIGNEMPGuid" />
                    <!--自動帶入，申請人GUID-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    離職原因
                    <!--TMP_REASONID-->
                </div>
                <div class="col-md-4">
                    <!--下拉式選單，CALL API取得, ProgID:HUM0013900，只取FLAG=3，Value=SYS_VIEWID，顯示SYS_NAME-->
                    <uc1:KYTDropDownList runat="server" ID="kddlREASONID" RepeatDirection="Horizontal">
                        <asp:ListItem Value="" Selected="True">--請選擇--</asp:ListItem>
                        <asp:ListItem Value="11">另有高究</asp:ListItem>
                    </uc1:KYTDropDownList>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<br />必須選擇離職原因" ForeColor="Red" ClientValidationFunction="CheckREASONID" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    證明份數
                    <!--RESIGNPROOF-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtRESIGNPROOF" TextMode="Number" />
                    <!--限輸入數字，RESIGNPROOF-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    預計離職日
                    <!--RESIGNDATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpRESIGNDATE" OnTextChanged="kdpRESIGNDATE_TextChanged" AutoPostBack="true" />
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<br />必須填寫預計離職日" ForeColor="Red" ClientValidationFunction="CheckRESIGNDATE" Display="Dynamic"></asp:CustomValidator>
                    <!--RESIGNDATE-->
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    實際離職日
                    <!--RESIGNATIONDATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpRESIGNATIONDATE" />
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="<br />必須填寫實際離職日" ForeColor="Red" ClientValidationFunction="CheckRESIGNATIONDATE" Display="Dynamic"></asp:CustomValidator>
                    <!--RESIGNATIONDATE-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    離職原因說明
                    <!--NOTE-->
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtNOTE" TextMode="MultiLine" Rows="3" Width="80%" />
                    <!--NOTE-->
                    <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="<br />必須填寫離職原因說明" ForeColor="Red" ClientValidationFunction="CheckNOTE" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    主管面談
                    <!--MANAGERTALK-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtMANAGERTALK" Width="100%" />
                    <!--MANAGERTALK-->
                </div>
                <div class="col-md-2 bg-light divtitle">
                    HR面談
                    <!--HRTALK-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtHRTALK" Width="100%" />
                    <!--HRTALK-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 divtitle">
                </div>
                <div class="col-md-3">
                    <uc1:KYTCheckBox runat="server" ID="chkINRESIGNRATE" Text="納入離職率計算" CheckedValue="1" />
                    <!--INRESIGNRATE-->
                </div>
                <div class="col-md-3">
                    <uc1:KYTCheckBox runat="server" ID="chkNEVERHIRED" Text="永不錄用" CheckedValue="1" />
                    <!--NEVERHIRED-->
                </div>
                <div class="col-md-3">
                    <uc1:KYTCheckBox runat="server" ID="chkALLOWREHIRED" Text="優先再聘" CheckedValue="1" />
                    <!--ALLOWREHIRED-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    永不錄用原因
                    <!--NEVERHIREDREASON-->
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtNEVERHIREDREASON" TextMode="MultiLine" Rows="2" Width="80%" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <uc1:KYTTextBox runat="server" ID="ktxtMessage" ForeColor="Red" ViewType="ReadOnly" LabelCssClass="msgColor" />
                </div>
            </div>
        </div>
        <!--container-fluid-->
    </ContentTemplate>
</asp:UpdatePanel>

<asp:LinkButton ID="lnk_Edit" runat="server" OnClick="lnk_Edit_Click" Visible="False" CausesValidation="False" meta:resourcekey="lnk_EditResource1">[修改]</asp:LinkButton>
<asp:LinkButton ID="lnk_Cannel" runat="server" OnClick="lnk_Cannel_Click" Visible="False" CausesValidation="False" meta:resourcekey="lnk_CannelResource1">[取消]</asp:LinkButton>
<asp:LinkButton ID="lnk_Submit" runat="server" OnClick="lnk_Submit_Click" Visible="False" CausesValidation="False" meta:resourcekey="lnk_SubmitResource1">[確定]</asp:LinkButton>
<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>
