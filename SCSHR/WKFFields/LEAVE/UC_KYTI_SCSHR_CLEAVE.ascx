<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_CLEAVE.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_CLEAVE" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>

<link href="../../../../KYTControl/css/gemps.ui.css" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
<link href="../../../../KYTControl/css/font-awesome.min.css" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
<script src="../../../../KYTControl/js/gemps.ui.js"></script>
<script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>

<link href="../../Assets/css/SCSHR.css" rel="stylesheet" />
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
    function checkLVH_DOC_NBR(sender, args) {
        ////var kddlLVH_DOC_NBR = $("#<%=kddlLVH_DOC_NBR.ClientID%>_DropDownList1");
        var ktxtDOC_NBR_Ori = $("#<%=ktxtDOC_NBR_Ori.ClientID%>_TextBox1");
        if (ktxtDOC_NBR_Ori.length > 0) {
            args.IsValid = ktxtDOC_NBR_Ori.val() != "";
        }
    }
    function checkReadLVH_DOC_NBR(sender, args) {
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
<%--飛騰銷假資訊--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <div class="container-fluid"> <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12 divheader" >銷假資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    請假單單號
                </div>
                <div class="col-md-10" >
                     <asp:HiddenField runat="server" ID="hidTitleName" />
                    <asp:HiddenField runat="server" ID="hidApplicantGUID" />
                    <uc1:KYTDropDownList runat="server" ID="kddlLVH_DOC_NBR" />
                    <uc1:KYTTextBox runat="server" ID="ktxtDOC_NBR_Ori" placeholder="請選擇請假單號" />
                    <asp:ImageButton runat="server" ID="ibtnDOC_NBR" ImageUrl="~/Common/Images/SearchBtn.gif" OnClick="ibtnDOC_NBR_Click" />
                    <telerik:RadButton runat="server" ID="btnRead" Text="讀取" AutoPostBack="true" CausesValidation="false" OnClick="btnRead_Click"></telerik:RadButton>
                    <asp:HiddenField runat="server" ID="hidAPIResult" />
                    <uc1:KYTTextBox runat="server" ID="ktxtMessage" ViewType="ReadOnly" ForeColor="Red"  LabelCssClass="msgColor" />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<br />必須選擇請假單號" ForeColor="Red" ClientValidationFunction="checkLVH_DOC_NBR" Display="Dynamic"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="<br />必須讀取請假單號" ForeColor="Red" ClientValidationFunction="checkReadLVH_DOC_NBR" Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    <span class="color_red">*</span>
                    銷假原因
                </div>
                <div class="col-md-10" >
                    <uc1:KYTTextBox runat="server" ID="ktxtCANCEL_REASON" TextMode="MultiLine" Rows="3" Width="100%" />
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<br />必須填寫銷假原因" ForeColor="Red" ClientValidationFunction="checkCANCEL_REASON"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="container-fluid"> <%--container-fluid--%>
            <div class="row">
                <div class="col-md-12" style="background-color:#F2F2F2;padding: 5px">原請假資訊
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    部門
                </div>
                <div class="col-md-4" >
                    <uc1:KYTTextBox runat="server" ID="ktxtAPPLICANTDEPT" />
                    <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                    <asp:HiddenField ID="hidCompanyNo" runat="server" />
                    <asp:HiddenField ID="hidGROUPCODE" runat="server" />
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
                    請假人
                </div>
                <div class="col-md-4" >
                    <uc1:KYTTextBox runat="server" ID="ktxtLEAEMP" />
                    <asp:HiddenField runat="server" ID="hidLEAEMP" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    代理人
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtLEAAGENT" />
                    <asp:HiddenField runat="server" ID="hidLEAAGENT" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    子假別
                </div>
                <div class="col-md-10" >
                    <uc1:KYTTextBox runat="server" ID="ktxtLEACODE" />
                    <asp:HiddenField runat="server" ID="hidLEACODE" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    特殊日期
                </div>
                <div class="col-md-4" >
                    <uc1:KYTTextBox runat="server" ID="ktxtSP_DATE" />                    
                </div>
                <div class="col-md-2 bg-light divtitle">
                    特殊日對象
                </div>
                <div class="col-md-4">
                    <uc1:KYTTextBox runat="server" ID="ktxtSP_NAME" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    請假時間(起)
                </div>
                <div class="col-md-4" >
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpSTARTTIME" OnClientDateSelected="kdtpSTARTTIME_Selected" />
                </div>
                <div class="col-md-2 bg-light divtitle">
                    請假時間(迄)
                </div>
                <div class="col-md-4">
                    <uc1:KYTDateTimePicker runat="server" ID="kdtpENDTIME" OnClientDateSelected="kdtpENDTIME_Selected" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    請假時數
                </div>
                <div class="col-md-10" >
                    <uc1:KYTTextBox runat="server" ID="ktxtLEAHOURS" TextBoxCssClass="textRight" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    請假天數
                </div>
                <div class="col-md-10" >
                    <uc1:KYTTextBox runat="server" ID="ktxtLEADAYS" TextBoxCssClass="textRight" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 bg-light divtitle">
                    請假說明
                </div>
                <div class="col-md-10" >
                    <uc1:KYTTextBox runat="server" ID="ktxtREMARK" TextMode="MultiLine" Rows="4" Width="100%" />
                </div>
            </div>
        </div> <%--container-fluid--%>

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
