<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_FOPUNCH.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_FOPUNCH" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>

<link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
<link href="<%=Page.ResolveUrl("~/CDS/SCSHR/Assets/css/SCSHR.css")%>" rel="stylesheet" />

<%--引用bootstrap --%>
<link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>


<style>
    .divtitle {
        border-width: 2px;
        border-color: white;
        padding: 1px;
        border-style: solid;
    }
    .msgColor {
        color:red;
    }
</style>


<script>
    /**
    * 檢查是否檢核成功
    * @param sender
    * @param args
    */
    function CheckFopunch(sender, args) {
        var hidAPIResult = $("#<%=hidAPIResult.ClientID%>");
        if (hidAPIResult.length > 0) {
            args.IsValid = hidAPIResult.val() == "OK";
        }
    }
    /**
     * 檢查補卡原因
     * @param sender
     * @param args
     */
    function checkFOPUNCH_REASON(sender, args) {
        var kddlFOPUNCH_REASON = $("#<%=kddlFOPUNCH_REASON.ClientID%>_DropDownList1");
        if (kddlFOPUNCH_REASON.length > 0) {
            args.IsValid = kddlFOPUNCH_REASON.val() != null && kddlFOPUNCH_REASON.val() != "";
        }
    }
    /**
     * 檢查是否已勾選同意
     * @param sender
     * @param args
     */
    function checkAgree(sender, args) {
        var kchbox = $("#<%=kchbox.ClientID%>_CheckBox1");
        if (kchbox.length > 0) {
            args.IsValid = kchbox.prop("checked");
        }
    }
    /**
     * 檢查補卡狀態
     * @param sender
     * @param args
     */
    function checkFOPUNCHTYPE(sender, args) {
        var kddlFOPUNCHTYPE = $("#<%=kddlFOPUNCHTYPE.ClientID%>_DropDownList1"); // 補上班卡狀態
        var kddlFOPUNCHTYPE_OFF = $("#<%=kddlFOPUNCHTYPE_OFF.ClientID%>_DropDownList1"); // 補下班卡狀態

        if (kddlFOPUNCHTYPE.length > 0 &&
            kddlFOPUNCHTYPE_OFF.length > 0) {
            args.IsValid = kddlFOPUNCHTYPE.val() != "" || kddlFOPUNCHTYPE_OFF.val() != "";
        }
    }

    function checkVal(sender, args) { // 檢查
        var kdtpFOPUNCH_TIME = $("#<%=kdtpFOPUNCH_TIME.ClientID%>_textBox1");
        var kdtpFOPUNCH_TIME_OFF = $("#<%=kdtpFOPUNCH_TIME_OFF.ClientID%>_textBox1");
        var hidWorkResult = $("#<%=hidWorkResult.ClientID%>");
        var hidOffResult = $("#<%=hidOffResult.ClientID%>");
        var kddlFOPUNCHTYPE = $("#<%=kddlFOPUNCHTYPE.ClientID%>_DropDownList1"); // 補上班卡狀態
        var kddlFOPUNCHTYPE_OFF = $("#<%=kddlFOPUNCHTYPE_OFF.ClientID%>_DropDownList1"); // 補下班卡狀態


        if (kdtpFOPUNCH_TIME.length > 0 &&
            kdtpFOPUNCH_TIME_OFF.length > 0) {
            var rjson = $uof.pageMethod.syncUc("CDS/SCSHR/WKFFields/PUNCH/UC_KYTI_SCSHR_FOPUNCH.ascx",
                "CheckVal",
                [kdtpFOPUNCH_TIME.val(),
                kdtpFOPUNCH_TIME_OFF.val(),
                kddlFOPUNCHTYPE.val(),
                kddlFOPUNCHTYPE_OFF.val(),
                hidWorkResult.val(),
                hidOffResult.val()]);
            var jobj = JSON.parse(rjson);
            var msg = "";

            if (jobj["NOWORK"] != null && jobj["NOWORK"] != "") {
                msg += msg != '' ? jobj["NOWORK"] : jobj["NOWORK"];
            }
            if (jobj["NOWORK_START"] != null && jobj["NOWORK_START"] != "") {
                msg += msg != '' ? jobj["NOWORK_START"] : jobj["NOWORK_START"];
            }
            if (jobj["NOWORK_OFF"] != null && jobj["NOWORK_OFF"] != "") {
                msg += msg != '' ? jobj["NOWORK_OFF"] : jobj["NOWORK_OFF"];
            }

            if (jobj["START_TIME_Error"] != null && jobj["START_TIME_Error"] != "") {
                msg += msg != '' ? jobj["START_TIME_Error"] : jobj["START_TIME_Error"];
            }
            if (jobj["OFF_TIME_Error"] != null && jobj["OFF_TIME_Error"] != "") {
                msg += msg != '' ? jobj["OFF_TIME_Error"] : jobj["OFF_TIME_Error"];
            }

            if (msg != "") {
                args.IsValid = false; // 停送
                sender.innerHTML = msg;
                alert(msg);
                return;
            }
        }
        args.IsValid = true;

    }
</script>
<%--飛騰忘刷資訊--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">

    <ContentTemplate>
        <div class="container-fluid vertical-center">
            <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader">
                    補刷申請資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    帳務部門
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT_GROUPCODE" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    申請日期
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDATE" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    申請人
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTAccount" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTGuid" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    補卡原因
                </div>
                <div class="col-md-10">
                    <uc1:KYTDropDownList runat="server" ID="kddlFOPUNCH_REASON" />
                    <asp:CustomValidator ID="CustomValidator3" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="<br />必須選擇補卡原因" ClientValidationFunction="checkFOPUNCH_REASON"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-10">
                    <uc1:KYTCheckBox runat="server" ID="kchbox" Text="我已明確了解以下說明，並且同意遵守所有規定" CheckedValue="X" />
                    <asp:CustomValidator ID="CustomValidator4" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="<br />必須勾選同意事項" ClientValidationFunction="checkAgree"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-5">
                    <%--ktxtPUNCHMEMO Text文字之後需改為從飛騰取得，HUM0017000 的 BOFind 再依取回的 SYS_ID 去呼叫 BOMove
                    再去找回傳的 HUM0017000SUB Table 中過濾 SourceType = 0 的那一筆資料資料 TMP_TextFormNOTE 即為文字說明--%>
                    <uc1:KYTTextBox runat="server" ID="ktxtPUNCHMEMO" TextMode="MultiLine" Rows="3" Width="100%" readonly="true" Text="該考勤時間經由本人確認，如部分超過正常工時而未申請加班者，是由於本人自身因素提早進入公司或較晚離開公司，故無申請加班，以此證明。"/>
                </div>
            </div>  
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    補上班卡狀態<br />
                    補卡時間
                </div>
                <div class="col-md-10">
                    <uc1:KYTDropDownList runat="server" ID="kddlFOPUNCHTYPE" OnSelectedIndexChanged="kddlFOPUNCHTYPE_SelectedIndexChanged">
                        <asp:ListItem Value="">===請選擇===</asp:ListItem>
                    </uc1:KYTDropDownList>
                    <telerik:RadButton runat="server" ID="btnCheck" Text="檢查" AutoPostBack="true" CausesValidation="false" OnClick="btnCheck_Click" />
                    <br />
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpFOPUNCH_TIME" />
                    <asp:Label runat="server" ID="lblAPIResultError" ForeColor="Red" Font-Bold="true"></asp:Label>
                    <asp:HiddenField ID="hidConfirm" runat="server" />
                    <asp:HiddenField ID="hidAPIResult" runat="server" />
                    <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="<br />尚未檢查或者檢核失敗" ForeColor="Red" ClientValidationFunction="CheckFopunch" Display="Dynamic"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="必須選擇補卡狀態" ClientValidationFunction="checkFOPUNCHTYPE"></asp:CustomValidator>
                </div>
            </div>


            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    補下班卡狀態<br />
                    補卡時間
                </div>
                <div class="col-md-10">
                    <uc1:KYTDropDownList runat="server" ID="kddlFOPUNCHTYPE_OFF" OnSelectedIndexChanged="kddlFOPUNCHTYPE_OFF_SelectedIndexChanged">
                        <asp:ListItem Value="">===請選擇===</asp:ListItem>
                    </uc1:KYTDropDownList>
                    <br />
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpFOPUNCH_TIME_OFF" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    內文說明
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtREMARK" TextMode="MultiLine" Rows="4" Width="100%" />
                </div>
            </div>
            <div>
                <span class="color_red">
                    <asp:Label ID="lblWorkResult" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblOffResult" runat="server" Text=""></asp:Label>
                </span>
                <asp:HiddenField runat="server" ID="hidOffResult" />
                <asp:HiddenField runat="server" ID="hidWorkResult" />
                <asp:CustomValidator ID="CustomValidator1" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="<br />" ClientValidationFunction="checkVal"></asp:CustomValidator>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <uc1:KYTTextBox runat="server" ID="ktxtSignResult" ViewType="ReadOnly" ForeColor="Red" LabelCssClass="msgColor" />
                </div>
            </div>
        </div>
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
