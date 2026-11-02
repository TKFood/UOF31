<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_COVERTIME.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_COVERTIME" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>

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
        border-width:2px;
        border-color:white;
        padding:1px;
        border-style:solid;
    }
    .msgColor {
        color:red;
    }
</style>
<script>
    function checkOVH_DOC_NBR(sender, args) {
        ////var kddlOT_DOC_NBR = $("#<%=kddlOT_DOC_NBR.ClientID%>_DropDownList1");
        var ktxtDOC_NBR_Ori = $("#<%=ktxtDOC_NBR_Ori.ClientID%>_TextBox1");
        if (ktxtDOC_NBR_Ori.length > 0) {
            args.IsValid = ktxtDOC_NBR_Ori.val() != "";
        }
    }
    function checkReadOVH_DOC_NBR(sender, args) {
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

<%--飛騰銷班資訊--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container-fluid"> <%--container-fluid--%>
            <div class="row">
                <div class="col-md-22 divheader">銷班資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    加班單單號
                </div>
                <div class="col-md-10" >
                    <asp:HiddenField runat="server" ID="hidApplicantName" />
                    <asp:HiddenField runat="server" ID="hidApplicantGUID" />
                    <asp:HiddenField runat="server" ID="hidAPIResult" />
                    <asp:HiddenField ID="hidAPPLICANTDATE" runat="server" />
                    <uc1:KYTDropDownList runat="server" ID="kddlOT_DOC_NBR" />
                    <uc1:KYTTextBox runat="server" ID="ktxtDOC_NBR_Ori" placeholder="請選擇加班單號" />
                    <asp:ImageButton runat="server" ID="ibtnDOC_NBR" ImageUrl="~/Common/Images/SearchBtn.gif" OnClick="ibtnDOC_NBR_Click" />
                    <asp:Button ID="btnRead" runat="server" Text="讀取" CausesValidation="false" OnClick="btnRead_Click" />
                    <uc1:KYTTextBox runat="server" ID="ktxtMessage" ViewType="ReadOnly" ForeColor="Red"  LabelCssClass="msgColor" />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<br />必須選擇加班單號" ForeColor="Red" ClientValidationFunction="checkOVH_DOC_NBR" Display="Dynamic"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="<br />必須讀取加班單號" ForeColor="Red" ClientValidationFunction="checkReadOVH_DOC_NBR" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    銷班原因
                </div>
                <div class="col-md-10" >
                    <uc1:KYTTextBox runat="server" ID="ktxtCANCEL_REASON" TextMode="MultiLine" Rows="3" Width="100%"  Size="50"/>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<br />必須填寫銷班原因" ForeColor="Red" ClientValidationFunction="checkCANCEL_REASON"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="container-fluid"> <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader4">原加班單資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    部門
                </div>
                <div class="col-md-4" >
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                    <asp:HiddenField ID="hidGROUPCODE" runat="server" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    申請人
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANT" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    給付方式
                </div>
                <div class="col-md-10" >
                    <uc1:KYTRadioButtonList runat="server" ID="krblCHANGETYPE" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">補休</asp:ListItem>
                        <asp:ListItem Value="0">加班費</asp:ListItem>
                        <asp:ListItem Value="2">加班費及補休</asp:ListItem>
                    </uc1:KYTRadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    加班時間(起)
                </div>
                <div class="col-md-4" >
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpOT_START" OnClientDateSelected="kdtpOT_START_Selected" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    加班時間(迄)
                </div>
                <div class="col-md-4" >
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpOT_END" OnClientDateSelected="kdtpOT_END_Selected" />
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
                <div class="col-md-2 bg-light divtitle"  style="display:none;">
                    加班原因
                </div>
                <div class="col-md-4" style="display:none;"> <%--下拉選單WS:BOFind，ProgID 設定為 ATT0032200，SelectFields設定為 SYS_ViewID(放Value),SYS_Name(放Text),SYS_EngName   --%>
                    <uc1:KYTDropDownList runat="server" ID="kddlREMARK" />
                    <%--<uc1:KYTDropDownList runat="server" ID="kddlREMARK">
                        <asp:ListItem Value="1" Selected="True">1.人員突發狀況</asp:ListItem>
                        <asp:ListItem Value="2">2.代餐</asp:ListItem>
                        <asp:ListItem Value="3">3.常溫延遲到貨</asp:ListItem>
                        <asp:ListItem Value="4">4.冷藏延遲到貨</asp:ListItem>
                        <asp:ListItem Value="5">5.其他</asp:ListItem>
                    </uc1:KYTDropDownList>--%>
                    <%--選其他時ktxtREMARKOther才顯示並必填--%>
                    <uc1:KYTTextBox runat="server" ID="ktxtREMARKOther" TextBoxCssClass="textRight" Width="300px" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    申請時數
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtApplyHours" TextBoxCssClass="textRight" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    加班時數
                </div>
                <div class="col-md-2">
                    <uc1:KYTTextBox runat="server" ID="ktxtOverTimeHours" TextBoxCssClass="textRight" />
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
                    <uc1:KYTTextBox runat="server" ID="ktxtNOTE" TextMode="MultiLine" Rows="3" Width="100%"  />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label runat="server" ID="lblMessage"></asp:Label>
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
