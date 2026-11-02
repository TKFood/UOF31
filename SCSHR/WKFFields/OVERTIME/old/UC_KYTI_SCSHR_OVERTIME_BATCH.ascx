<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_OVERTIME_BATCH.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_OVERTIME_BATCH" %>
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
<%@ Register Src="~/Common/ChoiceCenter/UC_BtnChoiceOnce.ascx" TagPrefix="uc1" TagName="UC_BtnChoiceOnce" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceList.ascx" TagPrefix="uc1" TagName="UC_ChoiceList" %>
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
     * 加班時間起變更事件
     */
    function kdtpOT_START_Selected() {
        var ktxtOverTimeHours = $('#<%=ktxtOverTimeHours.TextBox1.ClientID%>'); // 取得加班時數
        ktxtOverTimeHours.val('0'); // 清空加班時數
    }
    /**
     * 加班時間迄變更事件
     */
    function kdtpOT_END_Selected() {
        var ktxtOverTimeHours = $('#<%=ktxtOverTimeHours.TextBox1.ClientID%>'); // 取得加班時數
        ktxtOverTimeHours.val('0'); // 清空加班時數
    }

    function checkREMARK(sender, args) {
        var kddlREMARK = $("#<%=kddlREMARK.ClientID%>_DropDownList1");
        if (kddlREMARK.length > 0) {
            args.IsValid = kddlREMARK.val() != null && kddlREMARK.val() != "";
        }
    }

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
        // 檢查給付方式
        var krblCHANGETYPE = $('#<%=krblCHANGETYPE.ClientID%>_RadioButtonList1');
        if (krblCHANGETYPE.length > 0) {
            if (krblCHANGETYPE.find("input[type=radio]:checked").val() == null ||
                krblCHANGETYPE.find("input[type=radio]:checked").val() == "") {
                msg += '必須選擇給付方式';
            }
        }
        if (msg != "") {
            alert(msg);
            args.IsValid = false;
        }
    }

    function checkVal(sender, args) {
        var canSend = true;
        var lblMessage = $('#<%=lblMessage.ClientID%>');
        var lblREMARKMSG = $('#<%=lblREMARKMSG.ClientID%>');
        var lblOT_TIMESMSG = $('#<%=lblOT_TIMESMSG.ClientID%>');
        var lblOT_ENDMSG = $('#<%=lblOT_ENDMSG.ClientID%>');
        var lblOT_STARTMSG = $('#<%=lblOT_STARTMSG.ClientID%>');
        var lblCHANGTYPE_MSG = $('#<%=lblCHANGTYPE_MSG.ClientID%>');
        lblMessage.text('');
        lblREMARKMSG.text('');
        lblOT_TIMESMSG.text('');
        lblOT_ENDMSG.text('');
        lblOT_STARTMSG.text('');
        lblCHANGTYPE_MSG.text('');

        var gvIndex = 0;
        var gvOVs = $('#<%=gvOVs.ClientID%>_GridView1'); // 找表格
        if (gvOVs.length > 0) {
            if (gvOVs.find("span:contains('目前沒有加班資料')").length > 0) {
                canSend = false;
                lblMessage.text('沒有加班資料不可送出');
            }
        }
        if (canSend) {
            gvOVs.find('TR').each(function (index, e) { // 巡覽TR
                debugger;
                var tr = $(e);
                if (index == 0) return; // 標題列不處理            
            });
        }
        if (!canSend)
            alert(sender.innerText);
        args.IsValid = canSend;
    }
    /**
     * 延時起動選人元件
     * */
    function DelayStartChoiceEmployee() {
        setTimeout(StartChoiceEmployee, 500);
    }
    /**
     * 觸發選人元件
     * */
    function StartChoiceEmployee() {
        $("input[name^='<%=btnclEmployee.ClientID%>']").click();
    }
</script>

<%--飛騰加班資訊--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">

    <ContentTemplate>
        <div class="container-fluid">
            <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader">
                    批次加班資訊
                </div>
                <div style="">
                </div>
            </div>
            <div runat="server" id="showHead">
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
                        <asp:HiddenField ID="hidGROUPCODE" runat="server" />
                        <uc1:UC_BtnChoiceOnce runat="server" ID="btncoGroup" ButtonText="選擇部門" ChoiceOnceKind="Department" ShowSubDept="true" OnEditButtonOnClick="btncoGroup_EditButtonOnClick" Visible="false" />


                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        申請人
                    </div>
                    <div class="col-md-6">
                        <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANT" Width="100%"/>
                        <asp:HiddenField ID="hidAPPLICANTGuid" runat="server" />
                        <asp:HiddenField runat="server" ID="hidAPPLICANT" />
                        <asp:Button runat="server" ID="btnEmployee" Text="選擇人員" CausesValidation="false" OnClick="btnEmployee_Click" Visible="false"></asp:Button>
                        <div style="display: none;">
                            <uc1:UC_ChoiceList runat="server" ID="btnclEmployee" ButtonText="選擇同部門人員" OnEditButtonOnClick="btnclEmployee_EditButtonOnClick" LimitChoice="WithOutUserDept" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <uc1:_UC_SelectUserFilterGroup runat="server" ID="btnAPPLICANT" ButtonText="選擇人員" SingleSelect="false" OnDialogReturn="btnAPPLICANT_DialogReturn" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        <span class="color_red">*</span>
                        給付方式
                    </div>
                    <div class="col-md-10">
                        <%--改為讀取來源為動態，Compnay, Workday之設定，TB_EIP_DUTY_SETTING_OVERTIME_HOURS--%>
                        <uc1:KYTRadioButtonList runat="server" ID="krblCHANGETYPE" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Selected="True">補休</asp:ListItem>
                            <asp:ListItem Value="0">加班費</asp:ListItem>
                            <asp:ListItem Value="2">加班費及補休</asp:ListItem>
                        </uc1:KYTRadioButtonList>
                        <asp:Label runat="server" ID="lblCHANGTYPE_MSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        <span class="color_red">*</span>
                        加班時間(起)
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDateTimePicker runat="server" ID="kdtpOT_START" OnClientDateSelected="kdtpOT_START_Selected" OnTextChanged="kdtpOT_START_TextChanged" AutoPostBack="true" />
                        <asp:Label runat="server" ID="lblOT_STARTMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-2 bg-light divtitle">
                        <span class="color_red">*</span>
                        加班時間(迄)
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDateTimePicker runat="server" ID="kdtpOT_END" OnClientDateSelected="kdtpOT_END_Selected" />
                        <asp:Label runat="server" ID="lblOT_ENDMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        加班班別
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtOT_CLASSTYPE_NAME" ViewType="ReadOnly" ForeColor="Red" />
                        <asp:HiddenField ID="hidOT_CLASSTYPE" runat="server" />
                    </div>
                    <div class="col-md-2 bg-light divtitle" style="display: none;">
                        <span class="color_red">*</span>
                        加班原因
                    </div>
                    <div class="col-md-4" style="display: none;">
                        <uc1:KYTDropDownList runat="server" ID="kddlREMARK" OnSelectedIndexChanged="kddlREMARK_SelectedIndexChanged" />
                        <%--<asp:CustomValidator ID="CustomValidator" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="<br />必須選擇加班原因" ClientValidationFunction="checkREMARK"></asp:CustomValidator>--%>

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
                        <asp:Label runat="server" ID="lblREMARKMSG" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        <%--<span class="color_red">*</span>--%>
                    加班內容說明
                    </div>
                    <div class="col-md-10">
                        <uc1:KYTTextBox runat="server" ID="ktxtNOTE" TextMode="MultiLine" Rows="3" Width="100%" />
                        <%--<asp:CustomValidator ID="CustomValidator2" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="必須填寫加班內容說明" ClientValidationFunction="checkNote"></asp:CustomValidator>--%>
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
                        <asp:Button runat="server" ID="btnCal" Text="計算" CausesValidation="false" AutoPostBack="true" OnClientClicked="checkTimes" OnClick="btnCal_Click"></asp:Button>
                        <asp:HiddenField ID="hidStyle" runat="server" />
                        <asp:HiddenField ID="hidConfirm" runat="server" />
                        <asp:HiddenField ID="hidAPIResult" runat="server" />
                        <asp:HiddenField ID="hidAllowCalcType" runat="server" />
                        <asp:Button runat="server" ID="rbtnFindOVERTIME" Text="加班狀況" CausesValidation="false" AutoPostBack="true" OnClick="rbtnFindOVERTIME_Click"></asp:Button>
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
                    <div class="col-md-12">
                        <uc1:KYTTextBox runat="server" ID="ktxtSignResult" ViewType="ReadOnly" ForeColor="Red" LabelCssClass="msgColor" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 divheader">
                    <%--<span class="color_red">*</span>--%>
                     加班明細
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <uc1:KYTGridView runat="server" ID="gvOVs"
                        CssClass="tsGridView2 horzFull"
                        AutoGenerateColumns="false"
                        ShowHeader="true"
                        ShowHeaderWhenEmpty="false"
                        OnRowDataBound="gvOVs_RowDataBound">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Label18" runat="server" ForeColor="Red" Font-Bold="true" Text="目前沒有加班資料"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Button ID="btnItemsDel" runat="server" Text="刪除" OnClick="btnItemsDel_Click" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="部門">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" Width="80px" FieldName="DEPT_NAME" Text='<%#Bind("DEPT_NAME")%>' />
                                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                                    <asp:HiddenField ID="hidAPPLICANTDATE" runat="server" />
                                    <asp:HiddenField ID="hidCompanyNo" runat="server" />
                                    <asp:HiddenField ID="hidGROUPCODE" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="申請人">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANT" Width="80px" FieldName="APPLICANT_NAME" Text='<%#Bind("APPLICANT_NAME")%>' />
                                    <asp:HiddenField ID="hidAPPLICANTGuid" runat="server" />
                                    <asp:HiddenField runat="server" ID="hidAPPLICANT" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="給付方式">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtCHANGETYPE" Width="80px" FieldName="CHANGETYPE_NAME" Text='<%#Bind("CHANGETYPE_NAME")%>' />
                                    <asp:HiddenField ID="hidCHANGETYPE" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="加班時間(起)">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_START" Width="150px" FieldName="OT_START" Text='<%#Bind("OT_START")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="加班時間(迄)">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_END" Width="150px" FieldName="OT_END" Text='<%#Bind("OT_END")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="加班班別">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_CLASSTYPE_NAME" Width="150px" FieldName="OT_CLASSTYPE_NAME" Text='<%#Bind("OT_CLASSTYPE_NAME")%>' />
                                    <asp:HiddenField ID="hidOT_CLASSTYPE" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="加班原因">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtREMARK" Width="150px" FieldName="REMARK_NAME" Text='<%#Bind("REMARK_NAME")%>' />
                                    <asp:HiddenField ID="hidREMARK" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="申請時數">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtApplyHours" Width="150px" FieldName="APPLYHOURS" Text='<%#Bind("APPLYHOURS")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="加班時數">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOverTimeHours" Width="150px" FieldName="OVERTIMEHOURS" Text='<%#Bind("OVERTIMEHOURS")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="加班內容說明">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtNOTE" Width="150px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="加班別">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_DOOVERTYPE" Width="150px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="加班別名稱">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_DOOVERTYPE_NAME" Width="150px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="首筆刷卡時間">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_WORKPUNCH" Width="150px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="尾筆刷卡時間">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_OFFWORKPUNCH" Width="150px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="計算後補休可休期限">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_MFREEDATE" Width="150px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="計算後的實際補休時數">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtOT_OVERCOMPHOURS" Width="150px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </uc1:KYTGridView>
                    <%--<asp:CustomValidator ID="CustomValidator2" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="必須填寫加班內容說明" ClientValidationFunction="checkNote"></asp:CustomValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label runat="server" ID="lblMessage" ForeColor="Red"></asp:Label>
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
