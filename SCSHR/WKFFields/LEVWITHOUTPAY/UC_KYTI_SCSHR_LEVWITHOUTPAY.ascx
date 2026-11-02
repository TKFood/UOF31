<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_LEVWITHOUTPAY.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_LEVWITHOUTPAY" %>
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
     * 檢查留停原因
     * @param sender
     * @param args
     */
     function CheckREASONID(sender, args) {
        var kddlREASONID = $("#<%=kddlREASONID.ClientID%>_DropDownList1");
        if (kddlREASONID.length > 0) {
            args.IsValid = kddlREASONID.val() != null && kddlREASONID.val() != "";
        }
    }
    /**
     * 檢查預計留停日
     * @param sender
     * @param args
     */
    function CheckPESTIMATERDATE(sender, args) {
        var kdpPESTIMATERDATE = $("#<%=kdpPESTIMATERDATE.ClientID%>_textBox1");
        if (kdpPESTIMATERDATE.length > 0) {
            args.IsValid = kdpPESTIMATERDATE.val() != null && kdpPESTIMATERDATE.val() != "";
        }
    }
    /**
     * 檢查預計復職日
     * @param sender
     * @param args
     */
    function CheckESTIMATEBDATE(sender, args) {
        var kdpESTIMATEBDATE = $("#<%=kdpESTIMATEBDATE.ClientID%>_textBox1");
        if (kdpESTIMATEBDATE.length > 0) {
            args.IsValid = kdpESTIMATEBDATE.val() != null && kdpESTIMATEBDATE.val() != "";
        }
    }




    // 處理表單按下Enter
    $(function () {
        ClickLimit();
        $(document).keypress(function (e) {
            if (e.which == 13) {
                return false;
            }
        });
    });
    function ClickLimit() {
        $(".NumberOnly").keypress(function (evt) {
            return isNumberKey(evt);
        });
    }
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
            return false;
        return true;
    }
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Get The Div Scroll Position
    function BeginRequestHandler(sender, args) {
    }
    //Set The Div Scroll Position
    function EndRequestHandler(sender, args) {
        ClickLimit();
    }


</script>
<style>
    .msgColor {
        color:red;
    }
</style>

<!--飛騰留停資訊-->
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        
    </Triggers>
    <ContentTemplate>

        <div class="container-fluid"> <!--container-fluid-->
            <div class="row">
                <div class="col-md-12 divheader" >
                    留停資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    部門
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" /> <!--自動帶入，唯讀-->
                    <asp:HiddenField ID="hidGROUPCODE" runat="server" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    申請日期 <!--SYS_DATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDATE" /> <!--自動帶入，唯讀，SYS_DATE-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                   留停人員 <!--TMP_EMPLOYEEID-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtRESIGNEMP" /> <!--自動帶入，申請人，顯示姓名與帳號-->
                    <asp:HiddenField runat="server" ID="hidRESIGNEMPTitleId" /> <!--自動帶入，職稱ID-->
                    <asp:HiddenField runat="server" ID="hidRESIGNEMPTitleName" /> <!--自動帶入，職稱名稱-->
                    <asp:HiddenField runat="server" ID="hidRESIGNEMPAccount" /> <!--自動帶入，帳號，TMP_EMPLOYEEID-->
                    <asp:HiddenField runat="server" ID="hidRESIGNEMPGuid" /> <!--自動帶入，申請人GUID-->
                </div>
                <div class="col-md-2 bg-light divtitle" >
                    <span class="color_red">*</span>
                    留停原因 <!--TMP_REASONID-->
                </div>
                <div class="col-md-4" > <!--下拉式選單，CALL API取得, ProgID:HUM0013900，只取FLAG=2，Value=SYS_VIEWID，顯示SYS_NAME-->
                    <uc1:KYTDropDownList runat="server" ID="kddlREASONID" RepeatDirection="Horizontal"> 
                        <asp:ListItem Value="" Selected="True">--請選擇--</asp:ListItem>
                        <asp:ListItem Value="11">另有高究</asp:ListItem>
                    </uc1:KYTDropDownList>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<br />必須選擇留停原因" ForeColor="Red" ClientValidationFunction="CheckREASONID" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    預計留停日 <!--PESTIMATERDATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpPESTIMATERDATE"  AutoPostBack="true" OnTextChanged="kdpPESTIMATERDATE_TextChanged" /> <!--PESTIMATERDATE-->
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<br />必須填寫預計留停日" ForeColor="Red" ClientValidationFunction="CheckPESTIMATERDATE" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    預計復職日 <!--ESTIMATEBDATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpESTIMATEBDATE"/> <!--ESTIMATEBDATE-->
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="<br />必須填寫預計復職日" ForeColor="Red" ClientValidationFunction="CheckESTIMATEBDATE" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    留停原因說明 <!--NOTE-->
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtNOTE" TextMode="MultiLine" Rows="3" Width="80%" /> <!--NOTE-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    最後計薪日 
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpLSPAYDATE" /> 
                </div>
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    留停起算日 <!--PSTARTDATE-->
                </div>
                <div class="col-md-4">
                    <uc1:KYTDatePicker runat="server" ID="kdpPSTARTDATE" /> <!--PSTARTDATE-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    檢核文件 <!--CHECKDOCUMENT-->
                </div>
                <div class="col-md-10">
                    <uc1:KYTTextBox runat="server" ID="ktxtCHECKDOCUMENT" Width="80%" /> <!--CHECKDOCUMENT-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    勞保費預繳狀態 <!--LPREPAYSTATUS-->
                </div>
                <div class="col-md-1">
                    <uc1:KYTDropDownList runat="server" ID="kddlLPREPAYSTATUS" RepeatDirection="Horizontal"> 
                        <asp:ListItem Value="0" Selected="True">未繳</asp:ListItem>
                        <asp:ListItem Value="1">已繳</asp:ListItem>
                    </uc1:KYTDropDownList>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    勞保費預繳月份數 <!--LPREPAYMONTH-->
                </div>
                <div class="col-md-1">
                    <uc1:KYTTextBox runat="server" ID="ktxtLPREPAYMONTH" Width="40px"  size="2" TextBoxCssClass="NumberOnly"/> <!--LPREPAYMONTH-->
                </div>
                <div class="col-md-2 bg-light divtitle">
                    勞保費預繳金額 <!--LPREPAYMONEY-->
                </div>
                <div class="col-md-3">
                    <uc1:KYTTextBox runat="server" ID="ktxtLPREPAYMONEY" Width="100px" size="8" TextBoxCssClass="NumberOnly"/> <!--LPREPAYMONEY-->
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    健保費預繳狀態 <!--HPREPAYSTATUS-->
                </div>
                <div class="col-md-1">
                    <uc1:KYTDropDownList runat="server" ID="kddlHPREPAYSTATUS" RepeatDirection="Horizontal"> 
                        <asp:ListItem Value="0" Selected="True">未繳</asp:ListItem>
                        <asp:ListItem Value="1">已繳</asp:ListItem>
                    </uc1:KYTDropDownList>
                </div>
                <div class="col-md-2 bg-light divtitle">
                    健保費預繳月份數 <!--HPREPAYMONTH-->
                </div>
                <div class="col-md-1">
                    <uc1:KYTTextBox runat="server" ID="ktxtHPREPAYMONTH" Width="40px" size="2" TextBoxCssClass="NumberOnly"/> <!--HPREPAYMONTH-->
                </div>
                <div class="col-md-2 bg-light divtitle">
                    健保費預繳金額 <!--HPREPAYMONEY-->
                </div>
                <div class="col-md-3">
                    <uc1:KYTTextBox runat="server" ID="ktxtHPREPAYMONEY" Width="100px" size="8" TextBoxCssClass="NumberOnly"/> <!--HPREPAYMONEY-->
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <uc1:KYTTextBox runat="server" ID="ktxtMessage" ForeColor="Red" ViewType="ReadOnly" LabelCssClass="msgColor"/>
                </div>
            </div>
        </div> <!--container-fluid-->
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
