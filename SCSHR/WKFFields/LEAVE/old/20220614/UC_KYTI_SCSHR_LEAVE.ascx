<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_LEAVE.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_LEAVE" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/CDS/SCSHR/WKFFields/QUERYWINDOWS/_UC_SelectUserFilterGroup.ascx" TagPrefix="uc1" TagName="_UC_SelectUserFilterGroup" %>
<%@ Register Src="~/CDS/SCSHR/WKFFields/QUERYWINDOWS/_UC_SearchGroupWithGroup.ascx" TagPrefix="uc1" TagName="_UC_SearchGroupWithGroup" %>




<link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
<link href="<%=Page.ResolveUrl("~/CDS/SCSHR/Assets/css/SCSHR.css")%>" rel="stylesheet" />

<%--引用bootstrap --%>
<link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
<style>
    .GridPager a, .GridPager span {
        display: block;
        height: 25px;
        width: 25px;
        font-weight: bold;
        text-align: center;
        text-decoration: none;
    }

    .GridPager a {
        background-color: #f5f5f5;
        color: #969696;
        border: 1px solid #969696;
    }

    .GridPager span {
        background-color: #A1DCF2;
        color: #ff0000;
        border: 1px solid #3AC0F2;
    }

    table .grid th {
        border: 1px solid white;
        height: 25px;
        background-color: #2DC1FC;
        color: white;
        padding: 5px;
    }

    table .grid tr td {
        border: 1px solid white;
        height: 25px;
        padding: 5px;
    }

    table .grid tr:last-child td {
        border: 1px solid white;
        height: 25px;
        padding: 5px;
    }

    table .grid tr:last-child table {
        border: 0px;
    }
</style>
<script>
    /**
     * 請假時間起變更事件
     */
    function kdtpSTARTTIME_Selected() {
        var hidAPIResult = $('#<%=hidAPIResult.ClientID%>'); // 找CALLAPI結果
        var ktxtLEAHOURS = $('#<%=ktxtLEAHOURS.TextBox1.ClientID%>'); // 取得請假時數
        var ktxtLEADAYS = $('#<%=ktxtLEADAYS.TextBox1.ClientID%>'); // 取得請假天數
        ktxtLEAHOURS.val('0'); // 請空時數天數
        ktxtLEADAYS.val('0');
        hidAPIResult.val('');
    }
    /**
     * 請假時間迄變更事件
     */
    function kdtpENDTIME_Selected() {
        var hidAPIResult = $('#<%=hidAPIResult.ClientID%>'); // 找CALLAPI結果
        var ktxtLEAHOURS = $('#<%=ktxtLEAHOURS.TextBox1.ClientID%>'); // 取得請假時數
        var ktxtLEADAYS = $('#<%=ktxtLEADAYS.TextBox1.ClientID%>'); // 取得請假天數
        ktxtLEAHOURS.val('0'); // 請空時數天數
        ktxtLEADAYS.val('0');
        hidAPIResult.val('');
    }

    function CheckLEAANDAPP(sender, args) {
        var hidLEAAGENT = $("#<%=hidLEAAGENT.ClientID%>"); // 代理人
        var hidLEAEMP = $("#<%=hidLEAEMP.ClientID%>"); // 申請人
        if (hidLEAAGENT.length > 0) { // 有代理人才檢查
            if (hidLEAAGENT.val() != "" &&
                hidLEAEMP.val() != "") {
                args.IsValid = hidLEAAGENT.val() != hidLEAEMP.val();
            }
        }        
    }

    function CheckLEACODE(sender, args) {
        var kddlLEACODE = $("#<%=kddlLEACODE.ClientID%>_DropDownList1");
        if (kddlLEACODE.length > 0) {
            args.IsValid = kddlLEACODE.val() != null && kddlLEACODE.val() != "";
        }
    }

    function CheckSP_NAME(sender, args) {
        var ktxtSP_NAME = $("#<%=ktxtSP_NAME.ClientID%>_TextBox1");
        if (ktxtSP_NAME.length > 0) {
            args.IsValid = ktxtSP_NAME.val() != "";
        }
    }

    function CheckSTARTTIME(sender, args) {
        var kdtpSTARTTIME = $("#<%=kdtpSTARTTIME.ClientID%>_textBox1");
        if (kdtpSTARTTIME.length > 0) {
            args.IsValid = kdtpSTARTTIME.val() != "";
        }
    }
    function CheckENDTIME(sender, args) {
        var kdtpENDTIME = $("#<%=kdtpENDTIME.ClientID%>_textBox1");
        if (kdtpENDTIME.length > 0) {
            args.IsValid = kdtpENDTIME.val() != "";
        }
    }

    function CheckLEAHOURS(sender, args) {
        var ktxtLEAHOURS = $("#<%=ktxtLEAHOURS.ClientID%>_TextBox1");
        if (ktxtLEAHOURS.length > 0) {
            args.IsValid = ktxtLEAHOURS.val() != "";
        }
    }
    function CheckLEADAYS(sender, args) {
        var ktxtLEADAYS = $("#<%=ktxtLEADAYS.ClientID%>_TextBox1");
        if (ktxtLEADAYS.length > 0) {
            args.IsValid = ktxtLEADAYS.val() != "";
        }
    }
    function CheckREMARK(sender, args) {
        var ktxtREMARK = $("#<%=ktxtREMARK.ClientID%>_TextBox1");
        if (ktxtREMARK.length > 0) {
            args.IsValid = ktxtREMARK.val() != "";
        }
    }

    function CheckWork(sender, args) {
        var canSend = true;
        var msg = "";
        var hidAPIResult = $('#<%=hidAPIResult.ClientID%>'); // 找CALLAPI結果
        var ktxtLEAHOURS = $("#<%=ktxtLEAHOURS.ClientID%>_TextBox1"); // 請假時數
        var ktxtLEADAYS = $("#<%=ktxtLEADAYS.ClientID%>_TextBox1"); // 請假天數
        var kdtpSTARTTIME = $("#<%=kdtpSTARTTIME.ClientID%>_textBox1"); // 請假時間(起)
        var kdtpENDTIME = $("#<%=kdtpENDTIME.ClientID%>_textBox1"); // 請假時間(迄)
        var hidLEAEMP = $("#<%=hidLEAEMP.ClientID%>"); // 請假人GUID
        var hidLEAAGENT = $("#<%=hidLEAAGENT.ClientID%>"); // 代理人GUID
        var kddlLEACODE = $("#<%=kddlLEACODE.ClientID%>_DropDownList1"); // 假別
        var ktxtSP_NAME = $("#<%=ktxtSP_NAME.ClientID%>_TextBox1"); // 特殊日對象
        var kdpSP_DATE = $("#<%=kdpSP_DATE.ClientID%>_textBox1"); // 特殊日期
        var ktxtREMARK = $("#<%=ktxtREMARK.ClientID%>_TextBox1"); // 請假說明
        var ktxtAPPLICANTDATE = $("#<%=ktxtAPPLICANTDATE.ClientID%>_TextBox1"); // 申請日期
        var ktxtMessage = $("#<%=ktxtMessage.ClientID%>_Label1"); // 說明
        var hidNeedAttach = $('#<%=hidNeedAttach.ClientID%>'); // 確認需不需要檢查附件
        var hidSEXAPPLY = $('#<%=hidSEXAPPLY.ClientID%>'); // 確認需不需要檢查性別
        var hidDOC_NBR = $('#<%=hidDOC_NBR.ClientID%>'); // 表單單號
        var hidFormScriptID = $('#<%=hidFormScriptID.ClientID%>'); // 表單草稿單號

        var Agent_ID = "";
        if (hidLEAAGENT.length > 0) { // 有開放代理人才處理
            if (hidLEAAGENT.val() != "" &&
                hidLEAEMP.val() != "") {
                Agent_ID = hidLEAAGENT.val();
            }
        }
        

        if (ktxtLEAHOURS.length > 0 &&
            ktxtLEADAYS.length > 0 &&
            kdtpSTARTTIME.length > 0 &&
            kdtpENDTIME.length > 0 &&
            hidAPIResult.length > 0 &&
            hidLEAEMP.length > 0) {
            canSend = !$.KYTValidators.isEmpty(hidAPIResult) ? hidAPIResult.val() == "OK" : false;
            if (!canSend)
                msg = "計算請假時數時有錯誤或者尚未計算";
            if (canSend) {
                if (hidLEAEMP.val() != "" &&
                    kdtpSTARTTIME.length > 0 &&
                    kdtpSTARTTIME.val() != "" &&
                    kdtpENDTIME.length > 0 &&
                    kdtpENDTIME.val() != "") {
                    var rjson = $uof.pageMethod.syncUc("CDS/SCSHR/WKFFields/LEAVE/UC_KYTI_SCSHR_LEAVE.ascx", "checkVal",
                        [hidLEAEMP.val(),
                        kdtpSTARTTIME.val(),
                        kdtpENDTIME.val(),
                        kddlLEACODE.val(),
                        ktxtAPPLICANTDATE.val(),
                        ktxtREMARK.val(),
                        kdpSP_DATE.val(),
                        ktxtSP_NAME.val(),
                        hidFormScriptID.val(),
                        hidDOC_NBR.val(),
                        hidNeedAttach.val(),
                            Agent_ID,
                            hidSEXAPPLY.val()
                        ]);
                    var jobj = JSON.parse(rjson);
                    if (jobj["docrepeat"] != null && jobj["docrepeat"] != "") {
                        msg += "<br />" + jobj["docrepeat"];
                    }
                    if (jobj["wsrepeat"] != null && jobj["wsrepeat"] != "") {
                        msg += "<br />" + jobj["wsrepeat"];
                    }
                    if (jobj["isError"] != null && jobj["isError"] != "") {
                        msg += "<br />" + jobj["isError"];
                    }
                    if (jobj["nofile"] != null && jobj["nofile"] != "") { // 沒有上傳附件
                        msg += "<br />" + jobj["nofile"];
                    }
                    //if (jobj["LEA_SEX"] == "M" && // 男性
                    //    (kddlLEACODE.val() == "06" || // 生理假
                    //        kddlLEACODE.val() == "08")) { // 產假
                    //    msg += "<br />男性不能申請";
                    //}
                    if (jobj["STARTTIME_BIGGER"] != null && jobj["STARTTIME_BIGGER"] != "") { // 迄止時間小於開始時間
                        msg += "<br />" + jobj["STARTTIME_BIGGER"];
                    }
                    if (jobj["SEXAPPLY"] != null && jobj["SEXAPPLY"] != "") { // 該性別無法申請這個假別
                        msg += "<br />" + jobj["SEXAPPLY"];
                    }
                    if (jobj["START_LIMIT"] != null && jobj["START_LIMIT"] != "") { // 請假起始時間不可小於限定的請假起始時間
                        msg += "<br />" + jobj["START_LIMIT"];
                    }
                    if (jobj["OFF_LIMIT"] != null && jobj["OFF_LIMIT"] != "") { // 請假迄止時間不可大於限定的請假迄止時間
                        msg += "<br />" + jobj["OFF_LIMIT"];
                    }
                }
            }
            if (msg != "") {
                args.IsValid = false; // 停送
                sender.innerHTML = msg;
                return;
            }
        } else { // 起單之外的狀態檢查事項
            if (ktxtMessage.length > 0) {
                if (!$.KYTValidators.isEmpty(ktxtMessage)) { // 訊息不為空
                    alert("該單據回寫HR失敗，請退回申請者或作廢/否決表單");
                } else { // 檢查飛騰是否能送單
                    var rjson = $uof.pageMethod.syncUc("CDS/SCSHR/WKFFields/LEAVE/UC_KYTI_SCSHR_LEAVE.ascx", "CheckSignLevel",
                        [
                            hidDOC_NBR.val()
                        ]);
                    var jobj = JSON.parse(rjson);

                    if (jobj["CheckWSError"] != null && jobj["CheckWSError"] != "") { // 詢問飛騰是否能送單
                        msg += "<br />" + jobj["CheckWSError"];
                    }

                    if (jobj["Error"] != null && jobj["Error"] != "") { // 是否有錯誤
                        msg += "<br />" + jobj["Error"];
                    }
                    if (msg != "") {
                        args.IsValid = false; // 停送
                        sender.innerHTML = msg;
                        return;
                    }
                }
            }
        }
    }
</script>

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

<%--飛騰請假資訊--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnLEAAGENT" />
    </Triggers>
    <ContentTemplate>

        <div class="container-fluid">
            <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader">
                    請假資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    部門
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" />
                    <asp:HiddenField ID="hidGROUPCODE" runat="server" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                    <asp:HiddenField runat="server" ID="hidFormScriptID" />
                    <asp:HiddenField runat="server" ID="hidSEXAPPLY" />
                    <asp:HiddenField runat="server" ID="hidNeedAttach" />
                    <asp:HiddenField ID="hidDOC_NBR" runat="server" />
                    <%--<asp:HiddenField runat="server" ID="hidIsMobileUI" />--%>
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
                    <span class="color_red">*</span>
                    請假人
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtLEAEMP" />
                    <asp:HiddenField runat="server" ID="hidLEAEMPTitleId" />
                    <asp:HiddenField runat="server" ID="hidLEAEMPTitleName" />
                    <asp:HiddenField runat="server" ID="hidLEAEMPAccount" />
                    <asp:HiddenField runat="server" ID="hidLEAEMP" />
                    <uc1:_UC_SelectUserFilterGroup runat="server" ID="btnLEAEMP" ButtonText="選擇" SingleSelect="true" isSearchSelfGroupUsers="false" OnDialogReturn="btnLEAEMP_DialogReturn" />
                </div>
                <div class="col-md-2 bg-light divtitle" runat="server" id="divAgentTitle">
                    代理人
                </div>
                <div class="col-md-4" runat="server" id="divAgent">
                    <uc1:KYTTextBox runat="server" ID="ktxtLEAAGENT" />
                    <asp:HiddenField runat="server" ID="hidLEAAGENT" />
                    <uc1:_UC_SelectUserFilterGroup runat="server" ID="btnLEAAGENT" ButtonText="選擇" SingleSelect="true" isSearchSelfGroupUsers="true" OnDialogReturn="btnLEAAGENT_DialogReturn" />
                    <uc1:_UC_SearchGroupWithGroup runat="server" ID="btnLEAAGENT_Group"  ButtonText="選擇" SingleSelect="true" OnDialogReturn="btnLEAAGENT_DialogReturn"/>
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="<br />代理人不可和申請人同一人" ForeColor="Red" ClientValidationFunction="CheckLEAANDAPP" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    假別
                </div>
                <div class="col-md-4">
                    <uc1:KYTDropDownList runat="server" ID="kddlLEACODE" OnSelectedIndexChanged="kddlLEACODE_SelectedIndexChanged" />

                    <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="<br />必須選擇假別" ForeColor="Red" ClientValidationFunction="CheckLEACODE" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div class="col-md-2 bg-light divtitle" runat="server" id="divSetAgentTitle">
                    啟動代理
                </div>
                <div class="col-md-4" runat="server" id="divSetAgent">
                    <asp:Label ID="lblAGENT" runat="server" Text=""></asp:Label>
                    <uc1:KYTCheckBox runat="server" ID="kchkAGENT" CheckedValue="X" />
                </div>
            </div>
            <div class="row" runat="server" id="divSP">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    特殊日期
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpSP_DATE" TextBoxReadOnly="false" />
                    <asp:Label ID="lblSP_DATE" runat="server" Text="※該假別特殊日需填寫事發當日日期" ForeColor="Blue"></asp:Label>
                    <br />
                    <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="<br />必須選擇特殊日期" ForeColor="Red" ClientValidationFunction="CheckSP_NAME" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    特殊日對象
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtSP_NAME" />
                    <br />
                    <asp:Label ID="lblSP_NAME" runat="server" Text="※同一事由之請假單，關係別的對象欄位需填寫一致" ForeColor="Blue"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    請假時間(起)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpSTARTTIME" OnClientDateSelected="kdtpSTARTTIME_Selected" OnTextChanged="kdtpSTARTTIME_TextChanged" AutoPostBack="true" />
                    <asp:CustomValidator ID="CustomValidator7" runat="server" ErrorMessage="<br />必須選擇請假時間(起)" ForeColor="Red" ClientValidationFunction="CheckSTARTTIME" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    請假時間(迄)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpENDTIME" OnClientDateSelected="kdtpENDTIME_Selected" OnTextChanged="kdtpENDTIME_TextChanged" AutoPostBack="true" />
                    <asp:CustomValidator ID="CustomValidator8" runat="server" ErrorMessage="<br />必須選擇請假時間(迄)" ForeColor="Red" ClientValidationFunction="CheckENDTIME" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    請假時數
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtLEAHOURS" TextBoxCssClass="textRight" />
                    <asp:Button ID="btnCal" runat="server" Text="計算" CausesValidation="false" OnClick="btnCal_Click" />
                    <%--<telerik:RadButton runat="server" ID="btnCANLEAS" CausesValidation="false" Text="可休假餘額"></telerik:RadButton>--%>
                    <asp:Label runat="server" ID="lblAPIResultError" ForeColor="Red" Font-Bold="true"></asp:Label>
                    <asp:HiddenField ID="hidConfirm" runat="server" />
                    <asp:HiddenField ID="hidAPIResult" runat="server" />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<br />尚未計算時數" ForeColor="Red" ClientValidationFunction="CheckLEAHOURS" Display="Dynamic"></asp:CustomValidator>

                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    請假天數
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtLEADAYS" TextBoxCssClass="textRight" />
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<br />尚未計算天數" ForeColor="Red" ClientValidationFunction="CheckLEADAYS" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    請假說明
                </div>
                <div class="col-md-6">
                    <uc1:KYTTextBox runat="server" ID="ktxtREMARK" TextMode="MultiLine" Rows="4" Width="100%" Size="450" />
                    <asp:CustomValidator ID="CustomValidator9" runat="server" ErrorMessage="<br />必須填寫請假說明" ForeColor="Red" ClientValidationFunction="CheckREMARK" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <uc1:KYTTextBox runat="server" ID="ktxtMessage" ForeColor="Red" ViewType="ReadOnly" LabelCssClass="msgColor" />
                    <asp:CustomValidator ID="CustomValidator10" runat="server" ErrorMessage="<br />" ForeColor="Red" ClientValidationFunction="CheckWork" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    假別資訊
                </div>
                <div class="col-md-6">
                    <asp:GridView ID="gvMain" runat="server"
                        CssClass="grid" Width="100%"
                        ShowHeader="true" ShowHeaderWhenEmpty="true"
                        AutoGenerateColumns="false"
                        ForeColor="#333333">
                        <PagerStyle CssClass="GridPager" />
                        <AlternatingRowStyle BackColor="#ffffcc" />
                        <Columns>
                            <asp:BoundField HeaderText="假別" DataField="LEVNAME" />
                            <asp:BoundField HeaderText="可休時數" DataField="HOURS" />
                            <asp:BoundField HeaderText="已休/已結時數" DataField="LEAVEHOURS" />
                            <asp:BoundField HeaderText="剩餘時數" DataField="RESIDUEHOURS" />
                            <asp:BoundField HeaderText="生效起始日期" DataField="STARTDATE" />
                            <asp:BoundField HeaderText="生效結束日期" DataField="ENDDATE" />
                        </Columns>
                    </asp:GridView>
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
