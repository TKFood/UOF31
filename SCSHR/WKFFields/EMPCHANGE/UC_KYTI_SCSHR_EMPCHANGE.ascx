<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_EMPCHANGE.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_EMPCHANGE" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceList.ascx" TagPrefix="uc1" TagName="UC_ChoiceList" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_BtnChoiceOnce.ascx" TagPrefix="uc1" TagName="UC_BtnChoiceOnce" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceListMobile.ascx" TagPrefix="uc1" TagName="UC_ChoiceListMobile" %>


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
     * 檢查職務異動原因
     * @param sender
     * @param args
     */
    function checkREASONID(sender, args) {
        var kddlREASONID = $("#<%=kddlREASONID.ClientID%>_DropDownList1");
        if (kddlREASONID.length > 0) {
            args.IsValid = kddlREASONID.val() != null && kddlREASONID.val() != "";
        }
    }
    /**
     * 檢查生效日期
     * @param sender
     * @param args
     */
    function checkSTARTDATE(sender, args) {
        var kdpSTARTDATE = $("#<%=kdpSTARTDATE.ClientID%>_textBox1");
        if (kdpSTARTDATE.length > 0) {
            args.IsValid = kdpSTARTDATE.val() != "";
        }
    }
    /**
     * 檢查結束日期
     * @param sender
     * @param args
     */
   function checkENDDATE(sender, args) {
        var kdpENDDATE = $("#<%=kdpENDDATE.ClientID%>_textBox1");
        if (kdpENDDATE.length > 0) {
            args.IsValid = kdpENDDATE.val() != "";
        }
    }

    /**
     * 檢查參考代碼
     * @param sender
     * @param args
     */
    function checkIDYCLASSID(sender, args) {
        var kddlIDYCLASSID = $("#<%=kddlIDYCLASSID.ClientID%>_DropDownList1");
        if (kddlIDYCLASSID.length > 0) {
            args.IsValid = kddlIDYCLASSID.val() != null && kddlIDYCLASSID.val() != "";
        }
    }
    /**
     * 檢查帳部部門
     * @param sender
     * @param args
     */
    function checkDEPARTID(sender, args) {
        var hidDEPARTID = $("#<%=hidDEPARTID.ClientID%>");
        if (hidDEPARTID.length > 0) {
            args.IsValid = hidDEPARTID.val() != null && hidDEPARTID.val() != "";
        }
    }
     /**
     * 檢查利潤中心
     * @param sender
     * @param args
     */
    function checkPROFITID(sender, args) {
        var hidPROFITID = $("#<%=hidPROFITID.ClientID%>");
        if (hidPROFITID.length > 0) {
            args.IsValid = hidPROFITID.val() != null && hidPROFITID.val() != "";
        }
    }
     /**
     * 檢查申報公司
     * @param sender
     * @param args
     */
    function checkCOMPANYID(sender, args) {
        var kddlCOMPANYID = $("#<%=kddlCOMPANYID.ClientID%>_DropDownList1");
        if (kddlCOMPANYID.length > 0) {
            args.IsValid = kddlCOMPANYID.val() != null && kddlCOMPANYID.val() != "";
        }
    }

   /***
     * 延遲觸發事件(以避免沒反應)
     * */
    function DelayStartChoiceGroup() {
        setTimeout(StartChoiceGroup, 500);
    }

    /***
     * 模擬點擊選人元件事件
     * */
    function StartChoiceGroup() {
        $("input[name^='<%=choiceGroup.ClientID%>']").click();
    }

</script>

<style>
    .msgColor {
        color:red;
    }
    
</style>

<!--飛騰職務異動資訊-->
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
    </Triggers>
    <ContentTemplate>

        <div class="container-fluid">
            <!--container-fluid-->
            <div class="row">
                <div class="col-md-12 divheader">
                    職務異動資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    部門
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT"  ReadOnly="true"/>
                    <!--自動帶入，唯讀-->
                    <asp:HiddenField ID="hidGROUPCODE" runat="server" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                    <div style="display:none;">
                        <uc1:UC_BtnChoiceOnce runat="server" ID="choiceGroup" ButtonText="選擇部門" ShowMember="false" ShowSubDept="true" ChoiceOnceKind="Department" OnEditButtonOnClick="choiceGroup_EditButtonOnClick" />
                    </div>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    申請日期
                    <!--SYS_DATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDATE"  ReadOnly="true"/>
                    <!--自動帶入，唯讀，SYS_DATE-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    異動人員
                    <!--TMP_EMPLOYEEID-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtEMPLOYEE"  ReadOnly="true"/>
                    <!--自動帶入，申請人，顯示姓名與帳號-->
                    <asp:HiddenField runat="server" ID="hidEMPLOYEETitleId" />
                    <!--自動帶入，職稱ID-->
                    <asp:HiddenField runat="server" ID="hidEMPLOYEETitleName" />
                    <!--自動帶入，職稱名稱-->
                    <asp:HiddenField runat="server" ID="hidEMPLOYEEAccount" />
                    <!--自動帶入，帳號，TMP_EMPLOYEEID-->
                    <asp:HiddenField runat="server" ID="hidEMPLOYEEGuid" />
                    <!--自動帶入，申請人GUID-->
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    異動原因
                    <!--TMP_REASONID-->
                </div>
                <div class="col-md-4">
                    <!--下拉式選單，CALL API取得, ProgID:HUM0013900，只取FLAG=1，Value=SYS_VIEWID，顯示SYS_NAME-->
                    <uc1:KYTDropDownList runat="server" ID="kddlREASONID" RepeatDirection="Horizontal">
                        <asp:ListItem Value="" Selected="True">--請選擇--</asp:ListItem>
                        <asp:ListItem Value="11">另有高究</asp:ListItem>
                    </uc1:KYTDropDownList>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="必須填寫異動原因" ForeColor="Red" ClientValidationFunction="checkREASONID" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    生效日期
                    <!--STARTDATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpSTARTDATE" AutoPostBack="true" OnTextChanged="kdpSTARTDATE_TextChanged"  />
                    <!--STARTDATE-->
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="必須填寫生效日期" ForeColor="Red" ClientValidationFunction="checkSTARTDATE" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    結束日期
                    <!--ENDDATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpENDDATE"  />
                    <!--ENDDATE-->
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="必須填寫結束日期" ForeColor="Red" ClientValidationFunction="checkENDDATE" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5 divheader2">
                    異動前資料
                </div>
                <div class="col-md-1">
                </div>
                <div class="col-md-6 divheader3">
                    異動後資料
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    申報公司 
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtOCOMPANYNAME" ReadOnly="true"/>
                    <!--自動帶入TB_EB_USER-COMPANY_NO-->
                    <asp:HiddenField ID="hidOCOMPANYID" runat="server" />
                    <!--放置所選部門之GROUP_CODE-->
                </div>
                <div class="col-md-6">
                    <!--TMP_DECCOMPANYID-->
                    <!--下拉式選單，CALL API取得, ProgID:HUM0010200，Value=SYS_VIEWID，顯示SYS_NAME-->
                    <uc1:KYTDropDownList runat="server" ID="kddlCOMPANYID" RepeatDirection="Horizontal">
                        <asp:ListItem Value="T011" Selected="True">T011</asp:ListItem>
                        <asp:ListItem Value="H011">H011</asp:ListItem>
                    </uc1:KYTDropDownList>
                    <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="必須填寫申報公司" ForeColor="Red" ClientValidationFunction="checkCOMPANYID" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    帳部部門 
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtODEPARTNAME" ReadOnly="true" />
                    <!--自動帶入使用部門名稱-->
                    <asp:HiddenField ID="hidODEPARTID" runat="server" />
                    <!--放置所選部門之GROUP_CODE-->
                </div>
                <div class="col-md-6">
                    <uc1:KYTTextBox runat="server" ID="ktxtDEPARTNAME" ReadOnly="true" />
                    <!--TMP_DEPARTID-->
                    <asp:HiddenField ID="hidDEPARTID" runat="server" />
                    <!--放置所選部門之GROUP_CODE-->
                    <asp:Button ID="btnDEPARTID" runat="server" CausesValidation="false" Text="..." class="btn btn-secondary btn-sm" OnClick="btnDEPARTID_Click" />
                    <!--選擇部門-->
                    <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="必須填寫帳部部門" ForeColor="Red" ClientValidationFunction="checkDEPARTID" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    利潤中心 
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtOPROFITNAME" ReadOnly="true" />
                    <!--自動帶入使用部門名稱-->
                    <asp:HiddenField ID="hidOPROFITID" runat="server" />
                </div>
                <div class="col-md-6">
                    <uc1:KYTDropDownList runat="server" ID="kddlPROFITNAME" OnSelectedIndexChanged="kddlPROFITNAME_SelectedIndexChanged"/>
                    <div style="display:none;">
                        <uc1:KYTTextBox runat="server" ID="ktxtPROFITNAME" ReadOnly="true" />
                        <asp:Button ID="btnPROFITID" runat="server" CausesValidation="false" Text="..." class="btn btn-secondary btn-sm" OnClick="btnPROFITID_Click" />
                    </div>

                    <!--顯示選擇的部門名稱-->
                    <asp:HiddenField ID="hidPROFITID" runat="server" />
                    <!--放置所選部門之GROUP_CODE   TMP_PROFITID-->
                    <asp:HiddenField ID="hidPROFITXml" runat="server" />
                    
                    <!--選擇部門-->
                    <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="必須填寫利潤中心" ForeColor="Red" ClientValidationFunction="checkPROFITID" Display="Dynamic"></asp:CustomValidator>
                </div>
                


            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    職稱 
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtODUTYNAME"  ReadOnly="true"/>
                    <!--自動帶入使用者職稱名稱-->
                    <asp:HiddenField ID="hidODUTYID" runat="server" />
                </div>
                <div class="col-md-6">
                    <!--TMP_DUTYID-->
                    <!--下拉式選單，CALL API取得, ProgID:HUM0010700，Value=SYS_VIEWID，顯示SYS_NAME-->
                    <uc1:KYTDropDownList runat="server" ID="kddlDUTYID" RepeatDirection="Horizontal">
                        <asp:ListItem Value="A01" Selected="True">董事長</asp:ListItem>
                        <asp:ListItem Value="A02">總經理</asp:ListItem>
                    </uc1:KYTDropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    投保身份 
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtOINSURANCESTATUS"  ReadOnly="true"/>
                    <!--帶入TB_EB_USER-OPTION2-->
                </div>
                <div class="col-md-6">
                    <!--INSURANCESTATUS-->
                    <uc1:KYTDropDownList runat="server" ID="kddlINSURANCESTATUS" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Selected="True">0-在公司投保健保</asp:ListItem>
                        <asp:ListItem Value="1">1-非在公司投保健保</asp:ListItem>
                    </uc1:KYTDropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    免扣繳對象
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtONotCutHIdentity"  ReadOnly="true"/>
                    <!--帶入TB_EB_USER-OPTION3-->
                </div>
                <div class="col-md-6">
                    <!--NotCutHIdentity-->
                    <uc1:KYTDropDownList runat="server" ID="kddlNotCutHIdentity" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Selected="True">0-正常</asp:ListItem>
                        <asp:ListItem Value="1">1-無投保資格者</asp:ListItem>
                        <asp:ListItem Value="2">2-第5類被保險人(低收入戶)</asp:ListItem>
                        <asp:ListItem Value="3">3-第2類被保險人</asp:ListItem>
                        <asp:ListItem Value="4">4-專門職業及技術人員自行執業者</asp:ListItem>
                        <asp:ListItem Value="5">5-自營作業而參加職業工會者</asp:ListItem>
                        <asp:ListItem Value="6">6-兒童及少年者</asp:ListItem>
                        <asp:ListItem Value="7">7-中低收入戶者</asp:ListItem>
                        <asp:ListItem Value="8">8-中低收入老人者</asp:ListItem>
                        <asp:ListItem Value="9">9-領取身心障礙者生活補助費者</asp:ListItem>
                        <asp:ListItem Value="10">10-勞工保險投保薪資未達基本工資之身心障礙者</asp:ListItem>
                        <asp:ListItem Value="11">11-國內就學之大專生且無專職工作者</asp:ListItem>
                        <asp:ListItem Value="12">12-符合健保法第100條所 定之經濟困難者</asp:ListItem>
                    </uc1:KYTDropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    參考代碼
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtOIDYCLASSID" ReadOnly="true" />
                    <!--帶入TB_EB_USER-OPTION4-->
                </div>
                <div class="col-md-6">
                    <!--TMP_IDYCLASSID-->
                    <!--下拉式選單，CALL API取得, ProgID:HUM0010600，Value=SYS_VIEWID，顯示SYS_NAME-->
                    <uc1:KYTDropDownList runat="server" ID="kddlIDYCLASSID" RepeatDirection="Horizontal">
                        <asp:ListItem Value="A01" Selected="True">總公司人員</asp:ListItem>
                        <asp:ListItem Value="A02">館店人員</asp:ListItem>
                        <asp:ListItem Value="A03">館店人員(7.5hr)</asp:ListItem>
                        <asp:ListItem Value="A04">館店人員(7hr)</asp:ListItem>
                        <asp:ListItem Value="A05">館店人員(6.5hr)</asp:ListItem>
                        <asp:ListItem Value="B01">時薪人員</asp:ListItem>
                    </uc1:KYTDropDownList>
                    <asp:CustomValidator ID="CustomValidator7" runat="server" ErrorMessage="必須填寫參考代碼" ForeColor="Red" ClientValidationFunction="checkIDYCLASSID" Display="Dynamic"></asp:CustomValidator>
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
