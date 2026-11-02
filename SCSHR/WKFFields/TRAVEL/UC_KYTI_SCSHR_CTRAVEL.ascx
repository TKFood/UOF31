<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_CTRAVEL.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_CTRAVEL" %>
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
                ktxtTBY.val($(this).text().trim());
            }
        });
    }
</script>

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
    function checkTRAVEL_DOC_NBR(sender, args) {
        var kddlTRAVEL_DOC_NBR = $("#<%=kddlTRAVEL_DOC_NBR.ClientID%>_DropDownList1");
        if (kddlTRAVEL_DOC_NBR.length > 0) {
            args.IsValid = kddlTRAVEL_DOC_NBR.val() != "";
        }
    }
    function checkReadTRAVEL_DOC_NBR(sender, args) {
        var hidAPIResult = $("#<%=hidAPIResult.ClientID%>");
        if (hidAPIResult.length > 0) {
            args.IsValid = hidAPIResult.val() != "" && hidAPIResult.val() == "OK";
        }
    }
    function checkCANCEL_REASON(sender, args) {
        var ktxtCANCEL_REASON = $("#<%=ktxtCANCEL_REASON.ClientID%>_TextBox1");
        if (ktxtCANCEL_REASON.length > 0) {
            args.IsValid = ktxtCANCEL_REASON.val() != "";
        }
    }
</script>

<%--飛騰銷出差資訊--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container-fluid"> <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader" >銷出差資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    出差單單號
                </div>
                <div class="col-md-10" >
                     <asp:HiddenField runat="server" ID="hidTitleName" />
                    <asp:HiddenField runat="server" ID="hidApplicantGUID" />
                    <uc1:KYTDropDownList runat="server" ID="kddlTRAVEL_DOC_NBR" />
                    <telerik:RadButton runat="server" ID="btnRead" Text="讀取" AutoPostBack="true" CausesValidation="false" OnClick="btnRead_Click"></telerik:RadButton>
                    <asp:HiddenField runat="server" ID="HiddenField1" />
                    <uc1:KYTTextBox runat="server" ID="ktxtMessage" ViewType="ReadOnly" ForeColor="Red" />
                    <asp:CustomValidator ID="CustomValidator11" runat="server" ErrorMessage="<br />必須選擇請假單號" ForeColor="Red" ClientValidationFunction="checkTRAVEL_DOC_NBR" Display="Dynamic"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator12" runat="server" ErrorMessage="<br />必須讀取請假單號" ForeColor="Red" ClientValidationFunction="checkReadTRAVEL_DOC_NBR" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    銷出差原因
                </div>
                <div class="col-md-10" >
                    <uc1:KYTTextBox runat="server" ID="ktxtCANCEL_REASON" TextMode="MultiLine" Rows="3" Width="100%" />
                    <asp:CustomValidator ID="CustomValidator13" runat="server" ErrorMessage="<br />必須填寫銷出差原因" ForeColor="Red" ClientValidationFunction="checkCANCEL_REASON"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="container-fluid">
            <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader">
                    SCSHR出差資訊
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
                    <asp:CustomValidator ID="CustomValidator10" runat="server" ErrorMessage="<br />" ForeColor="Red" ClientValidationFunction="CheckSignLevel" Display="Dynamic"></asp:CustomValidator>
                    <uc1:KYTTextBox runat="server" ID="ktxtSignResult" ViewType="ReadOnly" />
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
