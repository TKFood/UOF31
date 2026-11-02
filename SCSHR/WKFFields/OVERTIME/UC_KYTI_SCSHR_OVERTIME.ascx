<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_OVERTIME.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_OVERTIME" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>
<%@ Register Src="~/CDS/SCSHR/WKFFields/QUERYWINDOWS/_UC_SelectUserFilterGroup.ascx" TagPrefix="uc1" TagName="_UC_SelectUserFilterGroup" %>

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
        color: red;
    }

    span[id*='btnCal'] > .rbText:before {
        content: '*';
        color: red;
    }
</style>



<script>

    function checkTimes(sender, args) {
        var msg = "";
        // 檢查加班時間(起)
        var kdtpOT_START = $('#<%=kdtpOT_START.ClientID%>_textBox1');
        if (kdtpOT_START.length > 0) {
            if (kdtpOT_START.val() == "")
                msg += '必須填寫加班時間(起)\n';
        }
        // 檢查加班時間(迄)
        var kdtpOT_END = $('#<%=kdtpOT_END.ClientID%>_textBox1');
        if (kdtpOT_END.length > 0) {
            if (kdtpOT_END.val() == "")
                msg += '必須填寫加班時間(迄)\n';
        }
        if (msg != "") {
            alert(msg);
            args.IsValid = false;
        }
    }
    /**
     * 檢查加班內容說明
     * @param sender
     * @param args
     */
   <%-- function checkNote(sender, args) {
        var ktxtNOTE = $('#<%=ktxtNOTE.ClientID%>_TextBox1');
        if (ktxtNOTE.length > 0) {
            args.IsValid = ktxtNOTE.val() != "";
        }
    }--%>

    function checkVal(sender, args) {
        var canSend = true;
        var isPre = false;
        var lblMessage = $('#<%=lblMessage.ClientID%>');
        var lblREMARKMSG = $('#<%=lblREMARKMSG.ClientID%>');
        var lblOT_TIMESMSG = $('#<%=lblOT_TIMESMSG.ClientID%>');
        var lblOT_ENDMSG = $('#<%=lblOT_ENDMSG.ClientID%>');
        var lblOT_STARTMSG = $('#<%=lblOT_STARTMSG.ClientID%>');
        var lblPreOT_ENDMSG = $('#<%=lblPreOT_ENDMSG.ClientID%>');
        var lblPreOT_STARTMSG = $('#<%=lblPreOT_STARTMSG.ClientID%>');
        var lblCHANGTYPE_MSG = $('#<%=lblCHANGTYPE_MSG.ClientID%>');
        var lblNoteMSG = $('#<%=lblNoteMSG.ClientID%>');
        var hidIS_OV_CHK_PUNCH = $('#<%=hidIS_OV_CHK_PUNCH.ClientID%>');
        var hidIS_CHECK_PRE_TIME = $('#<%=hidIS_CHECK_PRE_TIME.ClientID%>');
        var kcbIsOutPunch = $('#<%=kcbIsOutPunch.ClientID%>_CheckBox1');

        var FormNumber = '<%=FormNumber%>'; // 表單單號

        lblMessage.text('');
        lblREMARKMSG.text('');
        lblOT_TIMESMSG.text('');
        lblOT_ENDMSG.text('');
        lblOT_STARTMSG.text('');
        lblPreOT_ENDMSG.text('');
        lblPreOT_STARTMSG.text('');
        lblCHANGTYPE_MSG.text('');
        lblNoteMSG.text('');
        // 檢查給付方式
        var krblCHANGETYPE = $('#<%=krblCHANGETYPE.ClientID%>_RadioButtonList1');
        if (krblCHANGETYPE.length > 0) {
            if (krblCHANGETYPE.find("input[type=radio]:checked").val() == null ||
                krblCHANGETYPE.find("input[type=radio]:checked").val() == "") {
                lblCHANGTYPE_MSG.text('必須選擇給付方式');
            }
            if (canSend)
                canSend = krblCHANGETYPE.find("input[type=radio]:checked").val() != null && krblCHANGETYPE.find("input[type=radio]:checked").val() != "";
            // 檢查加班給付方式允許類型
            var hidAllowCalcType = $('#<%=hidAllowCalcType.ClientID%>');
            if (hidAllowCalcType.length > 0) {
                var allowType = "";
                var _msg = "";
                if (canSend) {
                    switch (hidAllowCalcType.val()) {
                        case "0": // 兩者均可(不控管)
                            allowType = "any";
                            break;
                        case "1": // 僅能加班費
                            allowType = "0";
                            _msg = "僅能加班費";
                            break;
                        case "2": // 僅能補休
                            allowType = "1";
                            _msg = "僅能補休";
                            break;
                        case "3": // 加班費及補休都給
                            allowType = "any";
                            break;
                    }
                    if (allowType != "any") {
                        canSend = allowType == "" || krblCHANGETYPE.find("input[type=radio]:checked").val() == allowType;
                        if (!canSend)
                            lblCHANGTYPE_MSG.text(_msg);
                    }
                }
            }
        }

        // 檢查預加班時間(起)
        var kdtpPreOT_START = $('#<%=kdtpPreOT_START.ClientID%>_textBox1');
        if (kdtpPreOT_START.length > 0) {
            isPre = true;
            if (canSend)
                canSend = kdtpPreOT_START.val() != "";
            if (kdtpPreOT_START.val() == "")
                lblPreOT_STARTMSG.text('必須填寫預加班時間(起)');
        }
        // 檢查預加班時間(迄)
        var kdtpPreOT_END = $('#<%=kdtpPreOT_END.ClientID%>_textBox1');
        if (kdtpPreOT_END.length > 0) {
            isPre = true;
            if (canSend)
                canSend = kdtpPreOT_END.val() != "";
            if (kdtpPreOT_END.val() == "")
                lblPreOT_ENDMSG.text('必須填寫預加班時間(迄)');
        }
        if (!isPre) {
            // 檢查加班時間(起)
            var kdtpOT_START = $('#<%=kdtpOT_START.ClientID%>_textBox1');
            if (kdtpOT_START.length > 0) {
                if (canSend)
                    canSend = kdtpOT_START.val() != "";
                if (kdtpOT_START.val() == "")
                    lblOT_STARTMSG.text('必須填寫加班時間(起)');
                if (canSend) {
                    if (hidIS_CHECK_PRE_TIME.val() == "Y") {
                        var kdtpPreOT_START_Label = $('#<%=kdtpPreOT_START.ClientID%>_Label1');
                        if (kdtpPreOT_START_Label.length > 0) { // 當現在是預加班簽核通過時的加班起單
                            canSend = kdtpPreOT_START_Label.text() != "";
                            if (canSend) {
                                var datePreSt = new Date(kdtpPreOT_START_Label.text());
                                var dateSt = new Date(kdtpOT_START.val());
                                canSend = dateSt >= datePreSt;
                                lblOT_STARTMSG.text(canSend ? '' : '加班時間(起)不可小於預加班時間(起)');
                            } else {
                                lblPreOT_STARTMSG.text('預加班時間(起)是空白的');
                            }
                        }
                    }

                    if (hidIS_OV_CHK_PUNCH.val() == "Y") {
                        if (!kcbIsOutPunch.prop("checked")) { // 目前不是場外加班才能檢查
                            // 檢查刷卡時間
                            var ktxtWorkPunch = $('#<%=ktxtWorkPunch.ClientID%>_TextBox1');
                            if (ktxtWorkPunch.length > 0) {
                                canSend = ktxtWorkPunch.val() != "";
                                if (canSend) {
                                    var datePunchSt = new Date(ktxtWorkPunch.val());
                                    var dateSt = new Date(kdtpOT_START.val());
                                    canSend = dateSt >= datePunchSt;
                                    lblOT_STARTMSG.text(canSend ? '' : '加班時間(起)不可小於首筆刷卡時間');
                                } else {
                                    lblOT_STARTMSG.text('首筆刷卡時間不可為空');
                                }
                            }
                        }
                    }
                }
            }

            // 檢查加班時間(迄)
            var kdtpOT_END = $('#<%=kdtpOT_END.ClientID%>_textBox1');
            if (kdtpOT_END.length > 0) {
                if (canSend)
                    canSend = kdtpOT_END.val() != "";
                if (kdtpOT_END.val() == "")
                    lblOT_ENDMSG.text('必須填寫加班時間(迄)');
                if (canSend) {
                    if (hidIS_CHECK_PRE_TIME.val() == "Y") {
                        var kdtpPreOT_END_Label = $('#<%=kdtpPreOT_END.ClientID%>_Label1');
                        if (kdtpPreOT_END_Label.length > 0) { // 當現在是預加班簽核通過時的加班起單
                            canSend = kdtpPreOT_END_Label.text() != "";
                            if (canSend) {
                                var datePreEd = new Date(kdtpPreOT_END_Label.text());
                                var dateEd = new Date(kdtpOT_END.val());
                                canSend = dateEd <= datePreEd;
                                lblOT_ENDMSG.text(canSend ? '' : '加班時間(迄)不可大於預加班時間(迄)');
                            } else {
                                lblPreOT_ENDMSG.text('預加班時間(迄)是空白的');
                            }
                        }
                    }

                    if (hidIS_OV_CHK_PUNCH.val() == "Y") {
                        if (!kcbIsOutPunch.prop("checked")) { // 目前不是場外加班才能檢查
                            // 檢查刷卡時間
                            var ktxtOffWorkPunch = $('#<%=ktxtOffWorkPunch.ClientID%>_TextBox1');
                            if (ktxtOffWorkPunch.length > 0) {
                                canSend = ktxtOffWorkPunch.val() != "";
                                if (canSend) {
                                    var datePunchSt = new Date(ktxtOffWorkPunch.val());
                                    var dateEd = new Date(kdtpOT_END.val());
                                    canSend = dateEd <= datePunchSt;
                                    lblOT_ENDMSG.text(canSend ? '' : '加班時間(迄)不可大於尾筆刷卡時間');
                                } else {
                                    lblOT_ENDMSG.text('尾筆刷卡時間不可為空');
                                }
                            }
                        }
                    }
                }
            }

           <%-- // 檢查加班原因
            var kddlREMARK = $("#<%=kddlREMARK.ClientID%>_DropDownList1");
            if (kddlREMARK.length > 0) {
                if (canSend) {
                    if (kddlREMARK.val() == null ||
                        kddlREMARK.val() == "") {
                        canSend = false;
                        lblREMARKMSG.text('必須選擇加班原因');
                    }
                }
            }
            // 檢查加班原因(其它)
            var ktxtREMARKOther = $('#<%=ktxtREMARKOther.ClientID%>_TextBox1');
            if (ktxtREMARKOther.length > 0) {
                if (canSend)
                    if (ktxtREMARKOther.val() == null ||
                        ktxtREMARKOther.val() == "") {
                        canSend = false;
                        lblREMARKMSG.text('選其它時，必須填寫');
                    }
            }--%>

            // 檢查加班內容說明
            var ktxtNOTE = $('#<%=ktxtNOTE.ClientID%>_TextBox1');
            if (ktxtNOTE.length > 0) {
                if (canSend) {
                    if (ktxtNOTE.val() == null ||
                        ktxtNOTE.val() == "") {
                        canSend = false;
                        lblNoteMSG.text('必須填寫加班內容說明');
                    }
                }
            }
            if (kdtpOT_START.length > 0) { // 有加班時間表示需要按下計算
                // 檢查查詢API結果
                var hidAPIResult = $('#<%=hidAPIResult.ClientID%>'); // 找CALLAPI結果
                if (hidAPIResult.length > 0) { // 有找到
                    if (canSend) {
                        canSend = hidAPIResult.val() != null && hidAPIResult.val() != "" ?
                            hidAPIResult.val() == "OK" : false;
                        if (!canSend) {
                            if (hidAPIResult.val() == "") {
                                lblMessage.text('必須按下計算檢查');
                            } else if (hidAPIResult.val() != "OK") {
                                lblMessage.text('檢查失敗，請檢查輸入內容');
                            }
                        }
                    }
                }
            }
            // 檢查加班時數
            var ktxtOverTimeHours = $('#<%=ktxtOverTimeHours.ClientID%>_TextBox1');
            if (ktxtOverTimeHours.length > 0) {
                if (canSend) {
                    if (ktxtOverTimeHours.val() == "" ||
                        ktxtOverTimeHours.val() == null) {
                        lblOT_TIMESMSG.text('加班時數不可為空');
                        canSend = false;
                    }
                    if (canSend) {
                        if ($.KYTConverter.toFloat(ktxtOverTimeHours) <= 0) {
                            lblOT_TIMESMSG.text('加班時數不可小於等於0');
                            canSend = false;
                        }
                    }
                }
            }



            if (canSend) { // 還能送單就檢查WS能不能送
                if (FormNumber != "") { // 表示起單關不檢查
                    var ktxtWorkPunch = $("#<%=ktxtWorkPunch.ClientID%>_Label1"); // 首筆刷卡時間
                    var ktxtOffWorkPunch = $("#<%=ktxtOffWorkPunch.ClientID%>_Label1"); // 尾筆刷卡時間
                    var hidDoOverType = $("#<%=hidDoOverType.ClientID%>"); // 加班別
                    var ktxtMFreeDate = $("#<%=ktxtMFreeDate.ClientID%>_Label1"); // 補休期限
                    var ktxtOverCompHours = $("#<%=ktxtOverCompHours.ClientID%>_Label1"); // 補休時數
                    if (ktxtWorkPunch.length > 0) {
                        var rjson = $uof.pageMethod.syncUc("CDS/SCSHR/WKFFields/OVERTIME/UC_KYTI_SCSHR_OVERTIME.ascx", "CheckSignLevel",
                            [
                                FormNumber,
                                ktxtWorkPunch.text(),
                                ktxtOffWorkPunch.text(),
                                hidDoOverType.val(),
                                ktxtMFreeDate.text(),
                                ktxtOverCompHours.text()
                            ]);
                        var jobj = JSON.parse(rjson);
                        if (jobj != null) {
                            if (jobj["CheckWSError"] != null && jobj["CheckWSError"] != "") { // 詢問飛騰是否能送單
                                lblMessage.text(jobj["CheckWSError"]);
                                canSend = false;
                            }
                            if (jobj["Error"] != null && jobj["Error"] != "") { // 錯誤
                                lblMessage.text(jobj["Error"]);
                                canSend = false;
                            }
                        }
                    }
                }
            }

        }

        // 檢查時間範圍日否正確("起"必須小於"迄")，不正確顯示「迄止時間小於起始時間」
        var hidIsOverTimeRange = $("#<%=hidIsOverTimeRange.ClientID%>"); 
        if (hidIsOverTimeRange.val() == "1") {
            canSend = false;
            lblMessage.text('迄止時間小於起始時間');
        }

        if (!canSend)
            alert(sender.innerText);
        args.IsValid = canSend;
    }

</script>

<%--飛騰加班資訊--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnCal" />
    </Triggers>
    <ContentTemplate>
        <div class="container-fluid">
            <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader">
                    加班資訊
                </div>
            </div>
            <div class="row" style="display: none">
                <asp:HiddenField runat="server" ID="hidIsOverTimeRange" />
            </div>
            <div class="row" runat="server" id="divH1" visible="false">
                <div class="col-md-12 divheader4">
                    預加班資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    部門
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPTName" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                    <asp:HiddenField ID="hidAPPLICANTDATE" runat="server" />
                    <asp:HiddenField ID="hidCompanyNo" runat="server" />
                    <asp:HiddenField ID="hidIS_OV_CHK_PUNCH" runat="server" />
                    <asp:HiddenField ID="hidGROUPCODE" runat="server" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    申請人
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANT" />
                    <asp:HiddenField ID="hidAPPLICANTGuid" runat="server" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANT" />
                    <uc1:_UC_SelectUserFilterGroup runat="server" ID="btnAPPLICANT" ButtonText="選擇人員" SingleSelect="true" OnDialogReturn="btnAPPLICANT_DialogReturn" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    給付方式
                </div>
                <div class="col-md-4">
                    <%--改為讀取來源為動態，Compnay, Workday之設定，TB_EIP_DUTY_SETTING_OVERTIME_HOURS--%>
                    <uc1:KYTRadioButtonList runat="server" ID="krblCHANGETYPE" RepeatDirection="Horizontal" OnSelectedIndexChanged="krblCHANGETYPE_SelectedIndexChanged">
                        <asp:ListItem Value="1" Selected="True">補休</asp:ListItem>
                        <asp:ListItem Value="0">加班費</asp:ListItem>
                        <asp:ListItem Value="2">加班費及補休</asp:ListItem>
                    </uc1:KYTRadioButtonList>
                    <asp:Label runat="server" ID="lblCHANGTYPE_MSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-2 bg-light divtitle" runat="server" id="divPreTitle">
                    是否預加班
                </div>
                <div class="col-md-4" runat="server" id="divPre">
                    <uc1:KYTCheckBox runat="server" ID="kcbIsPre" CheckedValue="X" OnCheckedChanged="kcbIsPre_CheckedChanged" />
                    <asp:HiddenField runat="server" ID="hidIS_CHECK_PRE_TIME" />
                </div>
            </div>
            <div class="row" runat="server" id="divPreOT" visible="false">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    預加班時間(起)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpPreOT_START" OnTextChanged="kdtpPreOT_START_TextChanged" AutoPostBack="true" />
                    <asp:Label runat="server" ID="lblPreOT_STARTMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    預加班時間(迄)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpPreOT_END" OnTextChanged="kdtpPreOT_END_TextChanged" AutoPostBack="true" />
                    <asp:Label runat="server" ID="lblPreOT_ENDMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
            </div>
            <div class="row" runat="server" id="divH2">
                <div class="col-md-12 divheader4">
                    加班資訊
                </div>
            </div>
            <div class="row" runat="server" id="div1">
                <div class="col-md-2 bg-light divtitle">
                    是否場外加班
                </div>
                <div class="col-md-4">
                    <uc1:KYTCheckBox runat="server" ID="kcbIsOutPunch" CheckedValue="X" />
                </div>
                <div class="col-md-2">
                </div>
                <div class="col-md-4">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    加班時間(起)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpOT_START" OnTextChanged="kdtpOT_START_TextChanged" AutoPostBack="true" />
                    <asp:Label runat="server" ID="lblOT_STARTMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    加班時間(迄)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpOT_END" AutoPostBack="true" OnTextChanged="kdtpOT_END_TextChanged" />
                    <asp:Label runat="server" ID="lblOT_ENDMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    加班班別
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtOT_CLASSTYPE_NAME" ViewType="ReadOnly" ForeColor="Red" />
                    <asp:Button runat="server" ID="btnOT_CLASSTYPE" Text="選擇班別" OnClick="btnOT_CLASSTYPE_Click"/>
                    <asp:Button runat="server" ID="btnClearOT_CLASSTYPE" Text="清除班別" CausesValidation="false" OnClick="btnClearOT_CLASSTYPE_Click" />
                    <asp:HiddenField ID="hidOT_CLASSTYPE" runat="server" />
                </div>
                <div runat="server" id="showRESTMINS_Title" class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    休息時數
                </div>
                <div runat="server" id="showRESTMINS" class="col-md-4">
                    <uc1:KYTDropDownList runat="server" ID="kddlRESTMINS" OnSelectedIndexChanged="kddlRESTMINS_SelectedIndexChanged">
                        <asp:ListItem Value="" Selected="True">無</asp:ListItem>
                        <asp:ListItem Value="0">0</asp:ListItem>
                        <asp:ListItem Value="1">0.5</asp:ListItem>
                        <asp:ListItem Value="2">1</asp:ListItem>
                        <asp:ListItem Value="3">1.5</asp:ListItem>
                        <asp:ListItem Value="4">2</asp:ListItem>
                    </uc1:KYTDropDownList>
                    <asp:HiddenField runat="server" ID="hidRESTMINS_Min" />
                    小時
                </div>
                <div class="col-md-2 bg-light divtitle" style="display: none;">
                    <span class="color_red">*</span>
                    加班原因
                </div>
                <div class="col-md-4" style="display: none;">
                    <uc1:KYTDropDownList runat="server" ID="kddlREMARK" OnSelectedIndexChanged="kddlREMARK_SelectedIndexChanged" />

                    <%--下拉選單WS:BOFind，ProgID 設定為 ATT0032200，SelectFields設定為 SYS_ViewID(放Value),SYS_Name(放Text),SYS_EngName   --%>
                    <%-- <uc1:KYTDropDownList runat="server" ID="kddlREMARK" OnSelectedIndexChanged="kddlREMARK_SelectedIndexChanged">
                        <asp:ListItem Value="1" Selected="True">1.人員突發狀況1</asp:ListItem>
                        <asp:ListItem Value="2">2.代餐</asp:ListItem>
                        <asp:ListItem Value="3">3.常溫延遲到貨</asp:ListItem>
                        <asp:ListItem Value="4">4.冷藏延遲到貨</asp:ListItem>
                        <asp:ListItem Value="5">5.其他</asp:ListItem>
                    </uc1:KYTDropDownList>--%>
                    <%--選其他時ktxtREMARKOther才顯示並必填--%>
                    <uc1:KYTTextBox runat="server" ID="ktxtREMARKOther" TextBoxCssClass="textRight" Width="300px" />
                    <%--<asp:CustomValidator ID="CustomValidator" runat="server" ErrorMessage="<br />加班原因不可為空" ForeColor="Red" ClientValidationFunction="CheckREMARKOther" Display="Dynamic"></asp:CustomValidator>--%>
                    <asp:Label runat="server" ID="lblREMARKMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    申請時數
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtApplyHours" TextBoxCssClass="textRight" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    加班時數
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtOverTimeHours" TextBoxCssClass="textRight" />
                    <telerik:RadButton runat="server" ID="btnCal" Text="計算" CausesValidation="false" AutoPostBack="true" OnClientClicked="checkTimes" OnClick="btnCal_Click"></telerik:RadButton>
                    <telerik:RadButton runat="server" ID="rbtnFindOVERTIME" Text="加班狀況" CausesValidation="false" AutoPostBack="true" OnClick="rbtnFindOVERTIME_Click"></telerik:RadButton>
                    <asp:HiddenField ID="hidStyle" runat="server" />
                    <asp:HiddenField ID="hidConfirm" runat="server" />
                    <asp:HiddenField ID="hidAPIResult" runat="server" />
                    <asp:HiddenField ID="hidAllowCalcType" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    補休時數
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtOverCompHours" TextBoxCssClass="textRight" ReadOnly="true" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    補休期限
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtMFreeDate" TextBoxCssClass="textRight" ReadOnly="true" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    首筆刷卡時間
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtWorkPunch" TextBoxCssClass="textRight" ReadOnly="true" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    尾筆刷卡時間
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtOffWorkPunch" TextBoxCssClass="textRight" ReadOnly="true" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    加班別
                </div>
                <div class="col-md-4">
                    <asp:HiddenField ID="hidDoOverType" runat="server" />
                    <uc1:KYTTextBox runat="server" ID="ktxtDoOverType" TextBoxCssClass="textRight" ReadOnly="true" />
                </div>
                <div class="col-md-2">
                </div>
                <div class="col-md-4">
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label runat="server" ID="lblOT_TIMESMSG" ForeColor="Red" Font-Bold="true" Display="Dynamic"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    本日累積時數
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtApplyHoursTODAY" TextBoxCssClass="textRight" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    本月累積時數
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtApplyHoursThemon" TextBoxCssClass="textRight" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 divheader">
                    <span class="color_red">*</span>
                    加班內容說明
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <uc1:KYTTextBox runat="server" ID="ktxtNOTE" TextMode="MultiLine" Rows="3" Width="100%" />
                    <%--<asp:CustomValidator ID="CustomValidator2" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="必須填寫加班內容說明" ClientValidationFunction="checkNote"></asp:CustomValidator>--%>
                    <asp:Label runat="server" ID="lblNoteMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <uc1:KYTTextBox runat="server" ID="ktxtSignResult" ViewType="ReadOnly" ForeColor="Red" LabelCssClass="msgColor" />
                    <asp:Label runat="server" ID="lblMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                    <br />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="不允許送單" ClientValidationFunction="checkVal"></asp:CustomValidator>
                </div>
            </div>

        </div>
        <%--container-fluid--%>
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
