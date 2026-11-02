<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_TRAVEL.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_TRAVEL" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>
<%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceList.ascx" TagPrefix="uc1" TagName="UC_ChoiceList" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_BtnChoiceOnce.ascx" TagPrefix="uc1" TagName="UC_BtnChoiceOnce" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceListMobile.ascx" TagPrefix="uc1" TagName="UC_ChoiceListMobile" %>



<link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
<link href="<%=Page.ResolveUrl("~/CDS/SCSHR/Assets/css/SCSHR.css")%>" rel="stylesheet" />

<%--引用bootstrap --%>
<link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>


<script>
    function ddlTBY_OnClientChange() {
        var ddlTBY = $(event.target);
        var TR = ddlTBY.parent(); // 找上層TR
        while (TR[0].tagName != 'TR') TR = TR.parent();
        debugger;
        var ktxtTBY = TR.find('input[id*=ktxtTBY]'); // 找交通工具
        ddlTBY.children().each(function () {
            if ($(this).val() == ddlTBY.val()) {
                ktxtTBY.val($(this).val().trim());
            }
        });
    }
</script>

<style>
    .msgColor {
        color:red;
    }
</style>

<style>
    div.tablediv {
        width: 100%;
        display: flex;
        flex-direction: column;
    }

        div.tablediv > div {
            display: flex;
            text-align: left;
        }

            div.tablediv > div.td_head {
                text-align: center;
                background-color: antiquewhite;
            }

            div.tablediv > div > div {
                border-top: 1px solid #000;
                border-bottom: 1px solid #000;
                border-left: 1px solid #000;
                min-height: 35px;
            }

                div.tablediv > div > div:last-child {
                    border-right: 1px solid #000;
                }

            div.tablediv > div:not(.td_head) > div {
                border-top-width: 0px;
            }

        div.tablediv.sp1_18 > div > div:nth-child(2),
        div.tablediv.sp1_18 > div > div:nth-child(3),
        div.tablediv.sp1_18 > div > div:nth-child(4),
        div.tablediv.sp1_18 > div > div:nth-child(5),
        div.tablediv.sp1_18 > div > div:nth-child(11) {
            flex-basis: 130px;
        }

        div.tablediv.sp1_18 > div > div:nth-child(1),
        div.tablediv.sp1_18 > div > div:nth-child(6),
        div.tablediv.sp1_18 > div > div:nth-child(7),
        div.tablediv.sp1_18 > div > div:nth-child(10) {
            flex-basis: 65px;
        }

        div.tablediv.sp1_18 > div > div:nth-child(8),
        div.tablediv.sp1_18 > div > div:nth-child(9),
        div.tablediv.sp1_18 > div > div:nth-child(12),
        div.tablediv.sp1_18 > div > div:nth-child(13),
        div.tablediv.sp1_18 > div > div:nth-child(14) {
            flex-basis: 195px;
        }

        div.tablediv.sp1_18 > div > div:nth-child(15) {
            flex-basis: 325px;
        }

        div.tablediv.sp1_18 > div > div:nth-child(16) {
            flex-basis: 400px;
        }

        div.tablediv.sp1_18 > div > div:nth-child(17) {
            flex-basis: 270px;
        }

        div.tablediv.sp1_18 > div > div:nth-child(18) {
            flex-basis: 280px;
        }

    .tsGridView2 {
        width: 100%;
    }

        .tsGridView2 > tbody > tr > th {
            background-color: #336699;
            color: white;
            vertical-align: middle;
            border: 1px solid #DEDEDE;
            height: 25px;
            padding: 5px;
            white-space: nowrap;
            text-align: left;
        }

        .tsGridView2 > tbody > tr > td {
            vertical-align: middle;
            border: 1px solid #DEDEDE;
            height: 25px;
            padding: 5px;
            white-space: nowrap;
            text-align: left;
        }

    .tsGridView2Cell {
        background-color: white;
    }

    .tsGridView2AltCell {
        background-color: #FFFDCE;
    }
</style>
<script>
    /**
     * 選擇出差人員按鈕事件
     * @param sender
     * @param args
     */
    function btnMen_OnClientClicked(sender, args) {
        $("#<%=utnMen.ClientID%>_btnEdit").click();

    }
    /**
     * 選擇代理人員按鈕事件
     * @param sender
     * @param args
     */
    function btnAGENT_OnClientClicked(sender, args) {
        $("#<%=ubtnAGENT.ClientID%>_btnEdit").click();
    }

    /**
     * 檢查出差人員
     * @param sender
     * @param args
     */
    function CheckMen(sender, args) {
        var hidTRAVEL_MEN_ACCOUNT = $("#<%=hidTRAVEL_MEN_ACCOUNT.ClientID%>");
        var hidIsMobileUI = $("#<%=hidIsMobileUI.ClientID%>"); // 是否是MOBILEUI
        if (hidIsMobileUI.val() == "N") { // 現在不是MOBILEUI
            args.IsValid = hidTRAVEL_MEN_ACCOUNT.val() != "";
        } else { // 現在是MOBILEUI
            //if (btnMobMen.length > 0) { // 有找到出差人員選人物件
            //    args.IsValid = btnMobMen.val() != "";
            //}
        }
    }
    /**
     * 檢查出差時間(起)
     * @param sender
     * @param args
     */
    function CheckSTARTTIME(sender, args) {
        var kdtpSTARTTIME = $("#<%=kdtpSTARTTIME.ClientID%>_textBox1");
        if (kdtpSTARTTIME.length > 0) {
            args.IsValid = kdtpSTARTTIME.val() != "";
        }
    }
    /**
     * 檢查出差時間(訖)
     * @param sender
     * @param args
     */
    function CheckENDTIME(sender, args) {
        var kdtpENDTIME = $("#<%=kdtpENDTIME.ClientID%>_textBox1");
        if (kdtpENDTIME.length > 0) {
            args.IsValid = kdtpENDTIME.val() != "";
        }
    }
    /**
     * 檢查出差時間是否合理
     * @param sender
     * @param args
     */
    function CheckTravelTime(sender, args) {
        var kdtpENDTIME = $("#<%=kdtpENDTIME.ClientID%>_textBox1");
        var kdtpSTARTTIME = $("#<%=kdtpSTARTTIME.ClientID%>_textBox1");
        if (kdtpSTARTTIME.length > 0 &&
            kdtpENDTIME.length > 0) {
            var dateSt = new Date(kdtpSTARTTIME.val());
            var dateEd = new Date(kdtpENDTIME.val());
            args.IsValid = dateEd > dateSt;
        }
    }
    /**
     * 檢查是否檢核成功
     * @param sender
     * @param args
     */
    function CheckTravel(sender, args) {
        var hidAPIResult = $("#<%=hidAPIResult.ClientID%>");
        if (hidAPIResult.length > 0) {
            args.IsValid = hidAPIResult.val() == "OK";
        }
    }
    /**
     * 檢查出差地點
     * @param sender
     * @param args
     */
    function CheckTRAVEL_POINT(sender, args) {
        var kddlTRAVEL_POINT = $("#<%=kddlTRAVEL_POINT.ClientID%>_DropDownList1");
        if (kddlTRAVEL_POINT.length > 0) {
            args.IsValid = kddlTRAVEL_POINT.val() != "";
        }
    }
    /**
     * 檢查預支旅費
     * @param sender
     * @param args
     */
    function CheckTRAVELMONEY(sender, args) {
        var ktxtTRAVELMONEY = $("#<%=ktxtTRAVELMONEY.ClientID%>_TextBox1");
        if (ktxtTRAVELMONEY.length > 0) {
            args.IsValid = ktxtTRAVELMONEY.val() != "";
        }
    }

    /**
     * 檢查代理人
     * @param sender
     * @param args
     */
    function checkTRAVEL_AGENT(sender, args) {
        var hidTRAVEL_AGENT_GUID = $("#<%=hidTRAVEL_AGENT_GUID.ClientID%>"); // 代理人
        var hidIsMobileUI = $("#<%=hidIsMobileUI.ClientID%>"); // 是否是MOBILEUI
        var btnMobAgent = $("#<%=btnMobAgent.ClientID%>_hiddenSelected");
        if (hidIsMobileUI.val() == "N") { // 現在不是MOBILEUI
            args.IsValid = hidTRAVEL_AGENT_GUID.val() != "";
        } else { // 現在是MOBILEUI
            if (btnMobAgent.length > 0) { // 有找到代理人員選人物件
                args.IsValid = btnMobAgent.val() != "";
            }
        }
    }

    /**
     * 檢查出差人及代理人
     * @param sender
     * @param args
     */
    function checkMenAndAGENT(sender, args) {
        var hidTRAVEL_AGENT_GUID = $("#<%=hidTRAVEL_AGENT_GUID.ClientID%>"); // 代理人
        var hidIsMobileUI = $("#<%=hidIsMobileUI.ClientID%>"); // 是否是MOBILEUI
        var btnMobAgent = $("#<%=btnMobAgent.ClientID%>_hiddenSelected");
        var hidTRAVEL_MEN_GUID = $("#<%=hidTRAVEL_MEN_GUID.ClientID%>"); // 出差人

        if (hidIsMobileUI.val() == "N") { // 現在不是MOBILEUI
            args.IsValid = hidTRAVEL_MEN_GUID.val() != hidTRAVEL_AGENT_GUID.val();
        } else { // 現在是MOBILEUI
            //if (btnMobMen.length > 0) { // 有找到出差人員選人物件
            //    args.IsValid = btnMobMen.val() != btnMobAgent.val();
            //}
        }
    }

    /**
     * 檢查幣別
     * @param sender
     * @param args
     */
    function checkTRAVELCURR(sender, args) {
        var kddlTRAVELCURR = $("#<%=kddlTRAVELCURR.ClientID%>_DropDownList1");
        if (kddlTRAVELCURR.length > 0) {
            args.IsValid = kddlTRAVELCURR.val() != null && kddlTRAVELCURR.val() != "";
        }
    }

    /**
     * 簽核關檢查飛騰狀態
     * @param sender
     * @param args
     */
    function CheckSignLevel(sender, args) {
        var msg = "";
        var FormNumber = '<%=FormNumber%>'; // 表單單號
        if (FormNumber != "") { // 表示起單關不檢查
            var rjson = $uof.pageMethod.syncUc("CDS/SCSHR/WKFFields/TRAVEL/UC_KYTI_SCSHR_TRAVEL.ascx", "CheckSignLevel",
                [
                    FormNumber
                ]);
            var jobj = JSON.parse(rjson);
            if (jobj != null) {
                if (jobj["LVHError"] != null && jobj["LVHError"] != "") { // 詢問飛騰是否能送單
                    msg += jobj["LVHError"];
                }
                if (jobj["Error"] != null && jobj["Error"] != "") { // 錯誤
                    msg += jobj["Error"];
                }

                if (msg != "") {
                    alert(msg);
                    args.IsValid = false; // 停送
                    sender.innerHTML = msg;
                    return;
                }
            }
        }
    }

    <%--/**
     * 檢查出差預定行程規劃
     * @param sender
     * @param args
     */
    function CheckPLs(sender, args) {
        var message = ''; // 錯誤訊息
        var gvIndex = 0;
        var gvPLs = $('#<%=gvPLs.ClientID%>_GridView1'); // 找表格
        gvPLs.find('TR').each(function (index, e) { // 巡覽TR
            var tr = $(e);
            if (tr.prop("innerText") == "目前沒有明細資料") {
                message = "沒有資料";
                return;
            }
            if (index == 0) return; // 標題列不處理
            gvIndex++;
            var ktxtPL = tr.find('input[id*=ktxtPL_TextBox1]'); // 找行程規劃
            if (ktxtPL.length == 0) return; // 不是輸入狀態不驗證
            var kdtpTSTARTTIME = tr.find('input[id*=kdtpTSTARTTIME_textBox1]'); // 找起始日期
            var kdtpTENDTIME = tr.find('input[id*=kdtpTENDTIME_textBox1]'); // 找結束日期
            var ktxtTDAYS = tr.find('input[id*=ktxtTDAYS_TextBox1]'); // 找出差天數
            var ktxtTCITY = tr.find('input[id*=ktxtTCITY_TextBox1]'); // 找出差城市
            var ktxtTBY = tr.find('input[id*=ktxtTBY_TextBox1]'); // 找交通工具
            var ktxtSP = tr.find('input[id*=ktxtSP_TextBox1]'); // 找出發地點
            var ktxtDP = tr.find('input[id*=ktxtDP_TextBox1]'); // 找目的地
            var ktxtREMARK = tr.find('input[id*=ktxtREMARK_TextBox1]'); // 找備註
            var single_message = $.KYTValidators.format('第 {0} 筆資料', gvIndex);
            var valid_length = single_message.length; // 無錯誤時的 single_message 長度
            if ($.KYTValidators.isEmpty(ktxtPL))
                single_message += ', 行程規劃空白';
            if ($.KYTValidators.isEmpty(kdtpTSTARTTIME))
                single_message += ', 起始日期空白';
            if ($.KYTValidators.isEmpty(kdtpTENDTIME))
                single_message += ', 結束日期空白';
            if ($.KYTValidators.isEmpty(ktxtTDAYS))
                single_message += ', 出差天數空白';
            if ($.KYTValidators.isEmpty(ktxtTCITY))
                single_message += ', 出差城市空白';
            if ($.KYTValidators.isEmpty(ktxtTBY))
                single_message += ', 交通工具空白';
            if ($.KYTValidators.isEmpty(ktxtSP))
                single_message += ', 出發地點空白';
            if ($.KYTValidators.isEmpty(ktxtDP))
                single_message += ', 目的地空白';
            if ($.KYTValidators.isEmpty(ktxtREMARK))
                single_message += ', 備註空白';
            if (single_message.length > valid_length)
                message += single_message + '\n';
        });
        if (message.trim() != '') { // 如果有錯誤訊息
            args.IsValid = false;
            message = '出差預定行程規劃\n' + message;
            $(sender).html(message);
            alert(message);
        }
    }

    /**
     * 檢查出差辦理事項
     * @param sender
     * @param args
     */
    function CheckItems(sender, args) {
        var message = ''; // 錯誤訊息
        var gvIndex = 0;
        var gvItems = $('#<%=gvItems.ClientID%>_GridView1'); // 找表格
        gvItems.find('TR').each(function (index, e) { // 巡覽TR
            var tr = $(e);
            if (tr.prop("innerText") == "目前沒有明細資料") {
                message = "目前沒有明細資料";
                return;
            }
            if (index == 0) return; // 標題列不處理
            gvIndex++;
            var ktxtPL1 = tr.find('input[id*=ktxtPL1_TextBox1]'); // 找行程規劃
            if (ktxtPL1.length == 0) return; // 不是輸入狀態不驗證
            var ktxtCT = tr.find('input[id*=ktxtCT_TextBox1]'); // 找聯絡方式
            var ktxMEN = tr.find('input[id*=ktxMEN_TextBox1]'); // 找拜訪對象
            var ktxtTEAM = tr.find('input[id*=ktxtTEAM_TextBox1]'); // 找出差人員及分工分式
            var ktxtPR = tr.find('input[id*=ktxtPR_TextBox1]'); // 找訪談重點與及預期效果
            var ktxtTHOPE = tr.find('input[id*=ktxtTHOPE_TextBox1]'); // 找希望授權事項及進度
            var ktxtPRSS = tr.find('input[id*=ktxtPRSS_TextBox1]'); // 找協助他部門辦理事項
            var ktxtREMARK1 = tr.find('input[id*=ktxtREMARK1_TextBox1]'); // 找備註
            var single_message = $.KYTValidators.format('第 {0} 筆資料', gvIndex);
            var valid_length = single_message.length; // 無錯誤時的 single_message 長度
            if ($.KYTValidators.isEmpty(ktxtPL1))
                single_message += ', 行程規劃空白';
            if ($.KYTValidators.isEmpty(ktxtCT))
                single_message += ', 聯絡方式空白';
            if ($.KYTValidators.isEmpty(ktxMEN))
                single_message += ', 拜訪對象空白';
            if ($.KYTValidators.isEmpty(ktxtTEAM))
                single_message += ', 出差人員及分工分式空白';
            if ($.KYTValidators.isEmpty(ktxtPR))
                single_message += ', 訪談重點與及預期效果空白';
            if ($.KYTValidators.isEmpty(ktxtTHOPE))
                single_message += ', 希望授權事項及進度空白';
            if ($.KYTValidators.isEmpty(ktxtPRSS))
                single_message += ', 協助他部門辦理事項空白';
            if ($.KYTValidators.isEmpty(ktxtREMARK1))
                single_message += ', 備註空白';
            if (single_message.length > valid_length)
                message += single_message + '\n';
        });
        if (message.trim() != '') { // 如果有錯誤訊息
            args.IsValid = false;
            message = '出差辦理事項\n' + message;
            $(sender).html(message);
            alert(message);
        }
    }--%>

    function my_key_down(e) {
        var key;
        //console.warn(e.keyCode);
        //console.warn(e.which);
        if (window.event) {
            key = e.keyCode;
        } else if (e.which) {
            key = e.which;
        } else {
            return true;
        }
        //console.log(key);
        if ((key >= 48 && key <= 57)
            || (key >= 96 && key <= 105) //數字鍵盤
            || 8 == key || 46 == key || 37 == key || 39 == key //8:backspace 46:delete 37:左 39:右 (倒退鍵、刪除鍵、左、右鍵也允許作用)
        ) {
            return true;
        } else {
            return false;
        }
    }
</script>

<%--飛騰出差資訊--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container-fluid">
            <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader">
                    出差資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    部門
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT_GROUPCODE" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                    <asp:HiddenField runat="server" ID="hidIsMobileUI" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    申請日期
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDATE" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    申請人
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTAccount" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANT" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    出差人員
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtTRAVEL_MEN" ReadOnly="true" />
                    <asp:HiddenField runat="server" ID="hidTRAVEL_MEN_GUID" />
                    <asp:HiddenField runat="server" ID="hidTRAVEL_MEN_ACCOUNT" />
                    <%--<uc1:UC_ChoiceListMobile runat="server" ID="btnMobMen" ChoiceType="User" />--%>
                    <telerik:RadButton runat="server" ID="btnMen" Text="選擇" CausesValidation="false" OnClientClicked="btnMen_OnClientClicked"></telerik:RadButton>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<br />必須選擇出差人員" ForeColor="Red" ClientValidationFunction="CheckMen" Display="Dynamic"></asp:CustomValidator>
                    <div style="display: none;">
                        <uc1:UC_ChoiceList runat="server" ID="utnMen" CausesValidation="false" ButtonText="出差人員" ChioceType="User" OnEditButtonOnClick="utnMen_EditButtonOnClick" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-4">
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    代理人
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtTRAVEL_AGENT" ReadOnly="true" />
                    <asp:HiddenField runat="server" ID="hidTRAVEL_AGENT_GUID" />
                    <uc1:UC_ChoiceListMobile runat="server" ID="btnMobAgent" ChoiceType="User" />
                    <telerik:RadButton runat="server" ID="btnAGENT" Text="代理人員" CausesValidation="false" OnClientClicked="btnAGENT_OnClientClicked"></telerik:RadButton>
                    <div style="display: none;">
                        <uc1:UC_ChoiceList runat="server" ID="ubtnAGENT" CausesValidation="false" ButtonText="代理人" ChioceType="User" OnEditButtonOnClick="ubtnAGENT_EditButtonOnClick" />
                    </div>
                    <asp:CustomValidator ID="CustomValidator8" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="<br />代理人不可和出差人相同" ClientValidationFunction="checkMenAndAGENT"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="<br />必須選擇代理人" ClientValidationFunction="checkTRAVEL_AGENT"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    出差地點
                </div>
                <div class="col-md-4">
                    <%--<uc1:KYTTextBox runat="server" ID="ktxtTRAVEL_POINT" />--%>
                    <uc1:KYTDropDownList runat="server" ID="kddlTRAVEL_POINT" />
                    <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="<br />出差地點必須填寫" ForeColor="Red" ClientValidationFunction="CheckTRAVEL_POINT" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    幣別
                </div>
                <div class="col-md-4">
                    <uc1:KYTDropDownList runat="server" ID="kddlTRAVELCURR" />
                    <asp:CustomValidator ID="CustomValidator7" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="<br />必須選擇幣別" ClientValidationFunction="checkTRAVELCURR"></asp:CustomValidator>
                    <%--<uc1:KYTRadioButtonList runat="server" ID="krblTRAVELCURR" RepeatDirection="Horizontal">
                        <asp:ListItem Value="TWD" Selected="True">台幣</asp:ListItem>
                        <asp:ListItem Value="USD">美金</asp:ListItem>
                    </uc1:KYTRadioButtonList>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    國內或國外
                </div>
                <div class="col-md-4">
                    <uc1:KYTRadioButtonList runat="server" ID="krblDOMESTIC" RepeatDirection="Horizontal" OnSelectedIndexChanged="krblDOMESTIC_SelectedIndexChanged" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    預支旅費
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtTRAVELMONEY" TextMode="Number" />
                    <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="<br />預支旅費必須填寫" ForeColor="Red" ClientValidationFunction="CheckTRAVELMONEY" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    出差時間(起)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpSTARTTIME" AutoPostBack="true" OnTextChanged="kdtpSTARTTIME_TextChanged" />
                    <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="<br />尚未輸入出差時間(起)" ForeColor="Red" ClientValidationFunction="CheckSTARTTIME" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    出差時間(訖)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpENDTIME"  AutoPostBack="true" OnTextChanged="kdtpENDTIME_TextChanged" />
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="<br />尚未輸入出差時間(訖)" ForeColor="Red" ClientValidationFunction="CheckENDTIME" Display="Dynamic"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator9" runat="server" ErrorMessage="<br />出差時間(訖)不得小於等於出差時間(起)" ForeColor="Red" ClientValidationFunction="CheckTravelTime" Display="Dynamic"></asp:CustomValidator>
                    <telerik:RadButton runat="server" ID="btnChk" Text="檢查" AutoPostBack="true" CausesValidation="false" OnClick="btnChk_Click"></telerik:RadButton>
                    <asp:Label runat="server" ID="lblAPIResultError" ForeColor="Red" Font-Bold="true"></asp:Label>
                    <asp:HiddenField ID="hidConfirm" runat="server" />
                    <asp:HiddenField ID="hidAPIResult" runat="server" />
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<br />尚未檢查或者檢核失敗" ForeColor="Red" ClientValidationFunction="CheckTravel" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    預支說明
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtTRAVELMONEY_REASON" TextMode="MultiLine" Rows="3" Width="100%" />

                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    備註
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtTRAVEL_REASON" TextMode="MultiLine" Rows="3" Width="100%" Size="400" />

                </div>
            </div>
            <div class="row">
                <div class="col-md-12 divheader2">
                    出差預定行程規劃
                    <telerik:RadButton runat="server" ID="btnPLAdd" Text="新增" CausesValidation="false" AutoPostBack="true" OnClick="btnPLAdd_Click"></telerik:RadButton>
                </div>
            </div>
            <div class="row">
                <%--gvItems1--%>
                <div class="col-md-12">
                    <uc1:KYTGridView runat="server" ID="gvPLs"
                        CssClass="tsGridView2 horzFull"
                        AutoGenerateColumns="false"
                        ShowHeader="true"
                        ShowHeaderWhenEmpty="false"
                        OnRowDataBound="gvPLs_RowDataBound">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Label17" runat="server" ForeColor="Red" Font-Bold="true" Text="目前沒有明細資料"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Button ID="btnPLsDel" runat="server" Text="刪除" OnClick="btnPLsDel_Click" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="行程規劃">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtPL" Width="80px" FieldName="PL" Text='<%#Bind("PL")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="起始日期">
                                <ItemTemplate>
                                    <uc1:KYTDatePicker runat="server" ID="kdtpTSTARTTIME" Width="80px" FieldName="TSTARTTIME" Text='<%#Bind("TSTARTTIME")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="結束日期">
                                <ItemTemplate>
                                    <uc1:KYTDatePicker runat="server" ID="kdtpTENDTIME" Width="80px" FieldName="TENDTIME" Text='<%#Bind("TENDTIME")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="出差天數">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTDAYS" Width="80px" FieldName="TDAYS" Text='<%#Bind("TDAYS")%>' TextMode="Number" OnTextChanged="ktxtTDAYS_TextChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="出差城市">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTCITY" Width="150px" FieldName="TCITY" Text='<%#Bind("TCITY")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="交通工具">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTBY" runat="server" onchange="ddlTBY_OnClientChange();">
                                    </asp:DropDownList>
                                    <div runat="server" id="divTBY" style="display: none;">
                                        <uc1:KYTTextBox runat="server" ID="ktxtTBY" FieldName="TBY" Text='<%# Bind("TBY") %>' ViewType="ReadOnly" />
                                    </div>
                                    <%--<uc1:KYTTextBox runat="server" ID="ktxtTBY" Width="80px" FieldName="TBY" Text='<%#Bind("TBY")%>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="出發地點">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtSP" Width="150px" FieldName="SP" Text='<%#Bind("SP")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="目的地">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtDP" Width="150px" FieldName="DP" Text='<%#Bind("DP")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="備註">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtREMARK" Width="150px" FieldName="REMARK" Text='<%#Bind("REMARK")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </uc1:KYTGridView>
                    <%--<asp:CustomValidator ID="CustomValidator9" runat="server" ErrorMessage="<br />" ForeColor="Red" ClientValidationFunction="CheckPLs" Display="Dynamic"></asp:CustomValidator>--%>
                </div>
                <%--gvItems1--%>
            </div>
            <div class="row">
                <div class="col-md-12 divheader3">
                    出差辦理事項
                    <telerik:RadButton runat="server" ID="btnItmAdd" Text="新增" CausesValidation="false" AutoPostBack="true" OnClick="btnItmAdd_Click"></telerik:RadButton>
                </div>
            </div>
            <div class="row">
                <%--gvItems2--%>
                <div class="col-md-12">
                    <uc1:KYTGridView runat="server" ID="gvItems"
                        CssClass="tsGridView2 horzFull"
                        AutoGenerateColumns="false"
                        ShowHeader="true"
                        ShowHeaderWhenEmpty="false"
                        OnRowDataBound="gvItems_RowDataBound">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Label18" runat="server" ForeColor="Red" Font-Bold="true" Text="目前沒有明細資料"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Button ID="btnItemsDel" runat="server" Text="刪除" OnClick="btnItemsDel_Click" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="聯絡方式">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtCT" Width="80px" FieldName="CT" Text='<%#Bind("CT")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="拜訪對象">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxMEN" Width="80px" FieldName="MEN" Text='<%#Bind("MEN")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="行程規劃">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtPL1" Width="80px" FieldName="PL1" Text='<%#Bind("PL1")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="出差人員及分工分式">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTEAM" Width="150px" FieldName="TEAM" Text='<%#Bind("TEAM")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="訪談重點與及預期效果">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtPR" Width="150px" FieldName="PR" Text='<%#Bind("PR")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="希望授權事項及進度">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTHOPE" Width="150px" FieldName="THOPE" Text='<%#Bind("THOPE")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="協助他部門辦理事項">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtPRSS" Width="150px" FieldName="PRSS" Text='<%#Bind("PRSS")%>' Size="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="備註">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtREMARK1" Width="150px" FieldName="REMARK1" Text='<%#Bind("REMARK1")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </uc1:KYTGridView>
                    <%--<asp:CustomValidator ID="CustomValidator10" runat="server" ErrorMessage="<br />" ForeColor="Red" ClientValidationFunction="CheckItems" Display="Dynamic"></asp:CustomValidator>--%>
                </div>
            </div>
            <%--gvItems2--%>
            <div class="row">
                <div class="col-md-12">
                    <%--<asp:CustomValidator ID="CustomValidator10" runat="server" ErrorMessage="<br />" ForeColor="Red" ClientValidationFunction="CheckSignLevel" Display="Dynamic"></asp:CustomValidator>--%>
                    <uc1:KYTTextBox runat="server" ID="ktxtSignResult" ViewType="ReadOnly" ForeColor="Red" LabelCssClass="msgColor"/>
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
