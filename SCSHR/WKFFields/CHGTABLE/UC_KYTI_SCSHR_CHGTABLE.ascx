<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UC_KYTI_SCSHR_CHGTABLE.ascx.cs" Inherits="WKF_OptionalFields_UC_KYTI_SCSHR_CHGTABLE" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_BtnChoiceOnce.ascx" TagPrefix="uc1" TagName="UC_BtnChoiceOnce" %>

<%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
<%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
<%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
<%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
<%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
<%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>
<%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
<%@ Register Src="~/CDS/SCSHR/WKFFields/QUERYWINDOWS/_UC_SelectUserFilterGroup.ascx" TagPrefix="uc1" TagName="UC_SelectUserFilterGroup" %>

<link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
<link href="<%=Page.ResolveUrl("~/CDS/SCSHR/Assets/css/SCSHR.css")%>" rel="stylesheet" />


<!--引用bootstrap -->
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
</style>

<script>
    // 必填欄位不可為空白
    function checkMustInput(sender, args) {
        var kddlADJUSTTYPE = $("select[id*=kddlADJUSTTYPE]"); // 調班性質
        var itemsA = $("input.mustinputA,select.mustinputA");
        var itemsB = $("input.mustinputB,select.mustinputB");
        var IsValid;
        if (kddlADJUSTTYPE.length > 0) {
            if (kddlADJUSTTYPE.val() == "0") {
                // 個人調班
                for (var i = 0; i < itemsA.length; i++) {
                    if (!itemsA[i].value || itemsA[i].value == "0") {
                        itemsA[i].style.backgroundColor = "#E6AEAE";
                        itemsA[i].style.setProperty("background-color", "#e6aeae", "important");
                        IsValid = "1";
                    }
                }
            }
            else if (kddlADJUSTTYPE.val() == "1") {
                // 人員互調
                for (var i = 0; i < itemsB.length; i++) {
                    if (!itemsB[i].value || itemsB[i].value == "0") {
                        itemsB[i].style.backgroundColor = "#E6AEAE";
                        itemsB[i].style.setProperty("background-color", "#e6aeae", "important");
                        IsValid = "1";
                    }
                }
            }

        }
        if (IsValid == "1") {
            args.IsValid = false;
        }
    }

    // 檢查調班日期(起)必填
    function checkkdpDATE(sender, args) {
        var kdpDATE = $("#<%=kdpDATE.ClientID%>_textBox1");
        if (kdpDATE.length > 0) {
            $(kdpDATE).css("background-color", "white");
            if (kdpDATE.val() == "") {
                kdpDATE[0].style.setProperty("background-color", "#e6aeae", "important");
                args.IsValid = false;
            }
        }
    }

    // 檢查調班日期(迄)必填
    function checkkdpEND_DATE(sender, args) {
        var kdpEND_DATE = $("#<%=kdpEND_DATE.ClientID%>_textBox1");
        if (kdpEND_DATE.length > 0) {
            $(kdpEND_DATE).css("background-color", "white");
            if (kdpEND_DATE.val() == "") {
                kdpEND_DATE[0].style.setProperty("background-color", "#e6aeae", "important");
                args.IsValid = false;
            }
        }
    }

    debugger;
    // 當調班性質="0"(個人調班)，原班別與新班別不可相同
    function checkWorkID(sender, args) {
        debugger;
        var kddlADJUSTTYPE = $("select[id*=kddlADJUSTTYPE]"); // 調班性質
        var hidCURRWORKID_ID = $("#<%=hidCURRWORKID_ID.ClientID%>"); // 調班人員班別
        var hidTMP_CURRWORKID_ID = $("#<%=hidTMP_CURRWORKID_ID.ClientID%>"); // 新班別
        var msg_date = $("#<%=msg_date.ClientID%>_Label1");
        if (kddlADJUSTTYPE.length > 0) {
            if (kddlADJUSTTYPE.val() == "0") {
                if (hidCURRWORKID_ID.val() == hidTMP_CURRWORKID_ID.val()) {
                    msg_date.style = "display: none"
                    args.IsValid = false;
                }
            }
        }
    }

    // 當調班性質="1"(人員互調)，調班人員與調班對象不可相同
    function checkUser(sender, args) {
        var kddlADJUSTTYPE = $("select[id*=kddlADJUSTTYPE]"); // 調班性質
        var hidTMP_EMPLOYEEID_ACCOUNT = $("#<%=hidTMP_EMPLOYEEID_ACCOUNT.ClientID%>"); // 調班人員帳號
        var hidTMP_TARGET_ACCOUNT = $("#<%=hidTMP_TARGET_ACCOUNT.ClientID%>"); // 調班對象帳號
        if (kddlADJUSTTYPE.val() == "1") {
            if (hidTMP_EMPLOYEEID_ACCOUNT.length > 0) {
                if (hidTMP_EMPLOYEEID_ACCOUNT.val() == hidTMP_TARGET_ACCOUNT.val()) {
                    args.IsValid = false;
                }
            }
        }
    }

    
    <%--// 當調班性質="1"(人員互調)，調班人員班別與調班對象班別不可相同
    function checkObjectWorkID(sender, args) {
        debugger;
        var kddlADJUSTTYPE = $("select[id*=kddlADJUSTTYPE]"); // 調班性質
        var hidCURRWORKID_ID = $("#<%=hidCURRWORKID_ID.ClientID%>"); // 調班人員班別
        var hidTMP_TCURRWORKID_ID = $("#<%=hidTMP_TCURRWORKID_ID.ClientID%>"); // 調班對象班別
        if (kddlADJUSTTYPE.length > 0) {
            if (kddlADJUSTTYPE.val() == "1") {
                if (hidCURRWORKID_ID.val() != "" || hidTMP_TCURRWORKID_ID.val() != "") {
                    if (hidCURRWORKID_ID.val() == hidTMP_TCURRWORKID_ID.val()) {
                        args.IsValid = false;
                    }
                }
            }
        }
    }--%>


    //送單前表身至少要有一筆資料
    function checkGridViewEmpty(sender, args) {
        debugger;
        if ($("table[id*='gvItemsD1_GridView1'] > tbody > tr > td").length <= 1) {
            alert("目前沒有明細資料");
            args.IsValid = false;
        }
    }

</script>

<!--(飛騰)調班單-->
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container-fluid">
            <!--container-fluid-->
            <div class="row my-1">
                <div class="col-md-12 divheader">
                    調班申請資訊
                </div>
            </div>
            <div style="display: none">
                <asp:HiddenField runat="server" ID="hidGROUPCODE" />
                <asp:HiddenField runat="server" ID="hidGourp_Name" />
                <asp:HiddenField runat="server" ID="hidAPPLICANTGUID" />
                <asp:HiddenField runat="server" ID="hidAPPLICANTDEPT" />
                <asp:HiddenField runat="server" ID="hidAPPLICANTACCOUNT" />
            </div>
            <div runat="server" id="showHead">
                <div class="row my-1">
                    <div class="col-md-2  bg-light divtitle">
                        日期
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDatePicker runat="server" ID="kdpApplicantDate" />
                    </div>
                    <div class="col-md-2  bg-light divtitle">
                        調班性質
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDropDownList runat="server" ID="kddlADJUSTTYPE" OnSelectedIndexChanged="krblTYPE_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">個人調班</asp:ListItem>
                            <asp:ListItem Value="1">人員互調</asp:ListItem>
                        </uc1:KYTDropDownList>
                    </div>
                </div>
                <div class="row my-1">
                    <div class="col-md-2  bg-light divtitle">
                        調班人員
                    </div>
                    <div class="col-md-4">
                        <!--選人元件，預設申請者-->
                        <uc1:KYTTextBox runat="server" ID="ktxtTMP_EMPLOYEEID" TextBoxCssClass="mustinputA mustinputB" />
                        <asp:HiddenField runat="server" ID="hidTMP_EMPLOYEEID_GUID" />
                        <asp:HiddenField runat="server" ID="hidTMP_EMPLOYEEID_ACCOUNT" />
                        <uc1:UC_SelectUserFilterGroup runat="server" ID="SelectTMP_EMPLOYEEID" ButtonText="..." SingleSelect="true" OnDialogReturn="SelectTMP_EMPLOYEEID_DialogReturn" />
                    </div>
                    <div class="col-md-2  bg-light divtitle">
                        調班人員部門
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtTMP_EMPLOYEEID_DEP" TextBoxCssClass="mustinputA mustinputB" />
                        <asp:HiddenField runat="server" ID="hidTMP_EMPLOYEEID_DEP" />
                    </div>
                </div>
                <div class="row my-1">
                    <div class="col-md-2  bg-light divtitle">
                        調班對象
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtTMP_TARGETID" TextBoxCssClass="mustinputB" />
                        <asp:HiddenField runat="server" ID="hidTMP_TARGET_GUID" />
                        <asp:HiddenField runat="server" ID="hidTMP_TARGET_ACCOUNT" />
                        <uc1:UC_SelectUserFilterGroup runat="server" ID="SelectTMPTARGETID" ButtonText="..." SingleSelect="true" OnDialogReturn="SelectTMP_TARGETID_DialogReturn" />
                    </div>
                    <div class="col-md-2  bg-light divtitle">
                        調班對象部門
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtTMP_TARGETID_DEP" TextBoxCssClass="mustinputB" />
                        <asp:HiddenField runat="server" ID="hidTMP_TARGETID_DEP" />
                    </div>
                </div>
                <div class="row my-1">
                    <div class="col-md-2  bg-light divtitle">
                    <span class="color_red">*</span>
                        調班日期(起)
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDatePicker runat="server" ID="kdpDATE" AutoPostBack="true" OnTextChanged="kdpDATE_SelectedIndexChanged"/>
                    </div>
                    <div class="col-md-2  bg-light divtitle">
                    <span class="color_red">*</span>
                        調班日期(迄)
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDatePicker runat="server" ID="kdpEND_DATE" AutoPostBack="true" OnTextChanged="kdpDATE_SelectedIndexChanged"/>
                    </div>
                </div>
                <div class="row my-1">
                    <div class="col-md-2  bg-light divtitle">
                    <span class="color_red">*</span>   
                        調班人員班別
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtCURRWORKID"  />
                        <uc1:KYTTextBox runat="server" ID="ktxtNEW_CURRWORKID" Visible="false" />  <!--該最新日期的班別代號-->
                        <asp:HiddenField runat="server" ID="hidCURRWORKID_ID" />
                        <asp:HiddenField runat="server" ID="hidCURRWORKID_NAME" />
                        <asp:HiddenField runat="server" ID="hidCURRWORKID_Htype" />
                        <asp:HiddenField runat="server" ID="hidCURRWORKID_Start" />
                        <asp:HiddenField runat="server" ID="hidCURRWORKID_End" />
                    </div>
                    <div class="col-md-2  bg-light divtitle">
                        <span class="color_red">*</span>
                        <asp:Label runat="server" ID="lblTMP_CURRWORKID" Text="新班別"></asp:Label>
                        <asp:Label runat="server" ID="lblTMP_TCURRWORKID" Text="調班對象班別"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <!-- 新班別 -->
                        <uc1:KYTTextBox runat="server" ID="ktxtTMP_CURRWORKID" TextBoxCssClass="mustinputA" />
                        <asp:HiddenField runat="server" ID="hidTMP_CURRWORKID_ID" />
                        <asp:HiddenField runat="server" ID="hidTMP_CURRWORKID_NAME" />
                        <asp:HiddenField runat="server" ID="hidTMP_CURRWORKID_Htype" />
                        <asp:HiddenField runat="server" ID="hidTMP_CURRWORKID_Start" />
                        <asp:HiddenField runat="server" ID="hidTMP_CURRWORKID_End" />
                        <!-- 調班對象班別 /> -->
                        <uc1:KYTTextBox runat="server" ID="ktxtTMP_TCURRWORKID" />
                        <asp:HiddenField runat="server" ID="hidTMP_TCURRWORKID_ID" />
                        <asp:HiddenField runat="server" ID="hidTMP_TCURRWORKID_NAME" />
                        <asp:HiddenField runat="server" ID="hidTMP_TCURRWORKID_Htype" />
                        <asp:HiddenField runat="server" ID="hidTMP_TCURRWORKID_Start" />
                        <asp:HiddenField runat="server" ID="hidTMP_TCURRWORKID_End" />
                        <asp:ImageButton runat="server" ID="ibtnTMP_CURRWORKID" ImageUrl="~/Common/Images/SearchBtn.gif" OnClick="ibtnTMP_CURRWORKID_Click" />

                    </div>
                </div>
                <div class="row my-1">
                    <div class="col-md-2 bg-light divtitle">
                        備註
                    </div>
                    <div class="col-md-6">
                        <uc1:KYTTextBox runat="server" ID="ktxtBKTXT" Width="100%"  TextMode="MultiLine" Rows="4" Size="450"  />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <uc1:KYTTextBox runat="server" ID="ktxtMessage" ForeColor="Red" ViewType="ReadOnly" LabelCssClass="msgColor" />
                    <asp:CustomValidator ID="CustomValidator6" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[原班別不可與新班別相同]" ClientValidationFunction="checkWorkID" ValidationGroup="NewGvItemD1"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[必須填寫必填欄位]" ClientValidationFunction="checkMustInput" ValidationGroup="NewGvItemD1"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[請填寫調班日期(起)]" ClientValidationFunction="checkkdpDATE" ValidationGroup="NewGvItemD1"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator5" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[請填寫調班日期(迄)]" ClientValidationFunction="checkkdpEND_DATE" ValidationGroup="NewGvItemD1"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator3" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[調班人員與調班對象不可相同]" ClientValidationFunction="checkUser" ValidationGroup="NewGvItemD1"></asp:CustomValidator>
                    <%--<asp:CustomValidator ID="CustomValidator4" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[調班人員班別與調班對象班別不可相同]" ClientValidationFunction="checkObjectWorkID" ValidationGroup="NewGvItemD1"></asp:CustomValidator>--%>
                    <asp:CustomValidator ID="CustomValidator11" runat="server" Font-Bold="true" ForeColor="Red" Display="Dynamic" ErrorMessage="[明細項沒有資料]" ClientValidationFunction="checkGridViewEmpty" ></asp:CustomValidator>
                    <asp:Label ForeColor="Red" runat="server" ID="msg_date" Text="無班別"></asp:Label> <!-- 原班別不可與新班別相同 -->
                    </div>
                </div>
            </div>
            <div class="row">
                <!--該區由申請填寫 -->
                <div class="col-md-12 divheader3">
                    調班明細
                    <asp:Button ID="btnNewGvItemD1" runat="server" Text="新增" class="btn btn-success btn-sm" AutoPostBack="true" OnClick="btnNewGvItemD1_Click" ValidationGroup="NewGvItemD1" />
                </div>
            </div>
            <div class="row">
                <!--gvItems_D1-->
                <div class="col-md-12 GridViewNormal GridRWD">
                    <uc1:KYTGridView runat="server" ID="gvItemsD1"
                        CssClass="tsGridView2 horzFull"
                        AutoGenerateColumns="false"
                        ShowHeader="true"
                        ShowHeaderWhenEmpty="false"
                        OnRowDataBound="gvItemsD1_RowDataBound">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Label13" runat="server" ForeColor="Blue" Font-Bold="true" Text="目前沒有明細資料"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="操作" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnDeleteD1" Text="刪除" Width="20px" ImageUrl="~/Common/Images/Icon/icon_m03_g.gif" CausesValidation="false" OnClick="btnDeleteD1_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="項次" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtITEM_NO" FieldName="ITEM_NO" Text='<%#Bind("ITEM_NO")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="編號" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtSYS_VIEWID" FieldName="SYS_VIEWID" Text='<%#Bind("SYS_VIEWID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="日期" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTDatePicker runat="server" ID="ktxtSYS_DATE" FieldName="SYS_DATE" Text='<%#Bind("SYS_DATE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="調班性質" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtADJUSTTYPE" FieldName="ADJUSTTYPE" Text='<%#Bind("ADJUSTTYPE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="調班人員" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTMP_EMPLOYEEID" FieldName="TMP_EMPLOYEEID" Text='<%#Bind("TMP_EMPLOYEEID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="人員部門" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTMP_DEPARTID" FieldName="TMP_DEPARTID" Text='<%#Bind("TMP_DEPARTID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="調班對象" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTMP_TARGETID" FieldName="TMP_TARGETID" Text='<%#Bind("TMP_TARGETID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="對象部門" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTMP_TDEPARTID" FieldName="BNTMP_TDEPARTIDFPO" Text='<%#Bind("TMP_TDEPARTID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="調班日期" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTDatePicker runat="server" ID="ktxtDATE" FieldName="DATE" Text='<%#Bind("DATE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="調班人員班別" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTMP_EMPLOYEEWORK" FieldName="TMP_EMPLOYEEWORK" Text='<%#Bind("TMP_EMPLOYEEWORK")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="新班別" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTMP_CURRWORKID" FieldName="TMP_CURRWORKID" Text='<%#Bind("TMP_CURRWORKID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="新班Htype" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtHTYPE" FieldName="HTYPE" Text='<%#Bind("HTYPE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="調班對象班別" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTMP_TCURRWORKID" FieldName="TMP_TCURRWORKID" Text='<%#Bind("TMP_TCURRWORKID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="對象班Htype" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTHTYPE" FieldName="THTYPE" Text='<%#Bind("THTYPE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="調班對象新班別" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtTMP_NTCURRWORKID" FieldName="TMP_NTCURRWORKID" Text='<%#Bind("TMP_NTCURRWORKID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="對象新班Htype" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtNTHTYPE" FieldName="NTHTYPE" Text='<%#Bind("NTHTYPE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="備註" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtNOTE" FieldName="NOTE" Text='<%#Bind("NOTE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="訊息" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtMSG" FieldName="MSG" Text='<%#Bind("MSG")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="重拋" Visible="true">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtReSend" FieldName="ReSend" Text='<%#Bind("ReSend")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </uc1:KYTGridView>
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
