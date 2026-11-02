using Ede.Uof.EIP.Organization;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Design;
using JGlobalLibs.UOF.Org;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

/**
* 修改時間：2022/11/14
* 修改人員：黃仕聿
* 修改項目：
    * 1.呼叫飛騰檢查/申請[請假單]給予的參數，更改參數順序，將TMP_SVACATIONID移到STARTTIME的後面
* 修改原因：
    * 1.因飛騰端程式處理順序，使用「任意給假單」請假時，飛騰要求需要先傳起始日期時間再傳假別代碼
* 修改位置： 
    * 1.「btnCal_Click()、checkVal()、CheckSignLevel()」，將TMP_SVACATIONID移到STARTTIME的後面
* **/

/**
* 修改時間：2021/11/01
* 修改人員：梁夢慈
* 修改項目：
    * 1.新增config參數(IS_SHOWAGENT)判斷是否顯示啟動代理(divSetAgentTitle、divSetAgent)
* 修改原因：
    * 1.新增規格
* 修改位置： 
    * 1.「SetField() -> 代理人顯示開關」中，取得參數(IS_SHOWAGENT)，並重新設定啟動代理的屬性Visible
* **/

/**
* 修改時間：2021/09/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.檢查按鈕(btnCal)，文字左方新增紅色*
* 修改原因：
    * 1.新增規格，因應客戶新需求
* 修改位置： 
    * 1.「前端網頁 -> CSS語法」中，新增語法「a.star:before、.star」，創造偽元素； 
    *   「前端網頁 -> btnCal按鈕」中，新增CSS「star」，並將按鈕標籤原為:"asp:Button" -> 改為"asp:LinkButton" (因偽元素只能使用在非Input標籤)
* **/

/**
* 修改時間：2021/09/03
* 修改人員：梁夢慈
* 修改項目：
    * 1.呼叫飛騰檢查/申請[請假單]給予的參數，更改參數順序，將TMP_AGENTID移到時間欄位的後面
* 修改原因：
    * 1.BUG修正，因飛騰端程式處理順序，讀取時間欄位的值，會飛騰系統端會重新抓取代理人(取預設值:第一代理人)
* 修改位置： 
    * 1.「btnCal_Click()、checkVal()、CheckSignLevel()」，將TMP_AGENTID移到時間欄位的後面
* **/

/**
* 修改時間：2021/08/16
* 修改人員：梁夢慈
* 修改項目：
    * 1.請假時間(起)(迄)改變時，移除「呼叫飛騰取得請假資訊」
    * 2.選假別呼叫WS取得假別資訊傳入參數更改，改為:預設帶入，起:當天、迄:當天+一個月
* 修改原因：
    * 1.修改規格，因呼叫飛騰WS非常耗時
    * 2.新增規格
* 修改位置： 
    * 1.「kdtpSTARTTIME_TextChanged()、kdtpENDTIME_TextChanged()」，註解「kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);」
    * 2.「kddlLEACODE_SelectedIndexChanged() -> if (SCSHRConfiguration.IS_LEAVE_DETAIL == "Y") -> RefreshMainData()」，傳入LEVDATE_START、LEVDATE_END，改為:預設帶入，起:當天、迄:當天+一個月
* **/

/**
* 修改時間：2021/06/21
* 修改人員：梁夢慈
* 修改項目：
    * 1.隱藏「代理人選擇」、「假別資訊」
    * 2.傳給飛騰固定傳第一順位代理人，如果沒有就給申請者自己
* 修改原因：
    * 1~2.修改規格，原規則不適用於批次
* 修改位置：
    * 1.「前端網頁 -> 代理人、假別資訊，最上層div標籤」中，新增屬性「style="display:none"」
    * 2.「前端網頁 -> CheckWork()」、「checkVal()」中，移除傳入代理人參數
    *   「checkVal()、CheckSignLevel()、btnCal_Click」中，傳入飛騰參數-TMP_AGENTID，改取請假人的UOF第一順位代理人，，如果沒有就給請假者自己
* **/

/**
* 修改時間：2021/06/18
* 修改人員：梁夢慈
* 修改項目：
    * 1.計算後，新增至明細項請假天數與請假時數放相反位置
* 修改原因：
    * 1.BUG修正，轉型為數值型別的欄位名稱給相反了
* 修改位置：
    * 1.「btnCal_Click() -> 呼叫飛騰取得請假天數/時數(GetLeaveHours)」中，修正轉型的欄位名稱「LeaveDays <--> LeaveHours」
* **/

/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
    * 2.呼叫飛騰，傳入參數-TMP_EMPLOYEEID、TMP_AGENTID，原給「ACCOUNT」 -> 改給「EMPLOYEEID」
* 修改原因：
    * 1.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
    * 2.規格修正，飛騰目前都改為固定傳入「EMPLOYEEID」
* 修改位置： 
    * 1.「SetField()、btnLEAAGENT_DialogReturn()、btnLEAEMP_DialogReturn()、btnLEAEMP_DialogReturn()、checkVal()」中，註解所有EBUser，改為KYT_EBUser
    * 2.「btnCal_Click()、checkVal()、CheckSignLevel()」，傳入TMP_EMPLOYEEID、TMP_AGENTID，的值 -> 改傳入EmployeeNo
* **/

/**
* 修改時間：2021/06/11
* 修改人員：梁夢慈
* 修改項目：
    * 1.請假人改為多選，按下計算時，一次新增至明細項
* 修改原因：
    * 1.新增規格，因應客戶(老楊)新需求
* 修改位置：
    * 1.「前端網頁中」中，請假人選人按鈕(btnLEAEMP)屬性-SingleSelect，原為true -> 改為false
    *   「btnLEAEMP_DialogReturn()」中，將選取的人，新增至ViewState["LEAEMP"]暫存
    *   「btnCal_Click()」中，迴圈巡覽ViewState["LEAEMP"]，依次檢查是否符合卡控條件，再新增至明細項內，並列出每一人的成功/失敗訊息
* **/

/**
* 修改時間：2021/04/27
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取得假別資訊前，新增判斷式，檢查設定值如果="Y"，才進入方法去呼叫飛騰
* 修改原因：
    * 1. 新增規格
* 修改位置：
    * 「kddlLEACODE_SelectedIndexChanged()」中，於呼叫RefreshMainData前，新增判斷式「SCSHRConfiguration.IS_LEAVE_DETAIL == "Y"」
* **/

/**
* 修改時間：2021/04/20
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取得假別資訊，當為「一般假」時，明細-假別，內容值為回傳DataTable欄位"SVacationName"，「特殊假」保持原規則不變
* 修改原因：
    * 1. 新增規格
* 修改位置：
    * 「RefreshMainData()」中，取得假別資訊回傳果後，新增判斷式當為「一般假」時，"LEVNAME"內容值，為回傳的"SVacationName"內容值
* **/

/**
* 修改時間：2021/03/31
* 修改人員：梁夢慈
* 修改項目：
    * 1. 請假人(ktxtLEAEMP)改為選人元件，可選所有人
    * 2. 隱藏前端驗證方法「必須填寫請假說明(CheckREMARK)、必須選擇假別(CheckLEACODE)」
* 修改原因：
    * 1. 新增規格
    * 2. BUG修正，已於計算按紐(btnCal)驗證，送出表單不需要再卡控這些驗證
* 修改位置：
    * 1.「前端網頁」中，於申請人欄位後面新增一個選人元件(btnLEAEMP)
    * 2.「前端網頁」中，註解此兩種驗證方法CustomValidator-CheckREMARK、CustomValidator-CheckLEACODE
* **/

/**
* 修改時間：2021/03/29
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取假別(非特別假)餘額的FundID改為GetEmpAllLRData(原為CheckLeaveLRData)
* 修改原因：
    * 1. 飛騰端修改傳入ID，不知道他們為什麼要改，反正就是改了
* 修改位置：
    * 「RefreshMainData」中，當不是特別假，funcId給予"GetEmpAllLRData"
* **/

/**
* 修改時間：2021/03/18
* 修改人員：梁夢慈
* 修改項目：
    * 1. 綁訂下拉選單-假別(kddlLEACODE)前，針對屬性BindDataOnly指定為true
* 修改原因：
    * 1. BUG修正，當申請多筆明細時，第一筆明細寫入飛騰的假別，都和實際表單所申請的不一樣
* 修改位置： 
    * 1.「SetField()」中，綁訂下拉選單(DataSource)前，設定 -> 「kddlLEACODE.BindDataOnly = true;」
* **/

/// <summary>
/// 飛騰批次請假資訊
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_BATLEAVE : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    string ConnectionString;


    SCSServicesProxy service = null;

    /// <summary>
    /// 當前FieldMode
    /// </summary>
    FieldMode FormFieldMode;

    /// <summary>
    /// KYT控制元件
    /// </summary>
    KYTController kytController;

    #region ==============公開方法及屬性==============
    //表單設計時
    //如果為False時,表示是在表單設計時
    private bool m_ShowGetValueButton = true;
    public bool ShowGetValueButton
    {
        get { return this.m_ShowGetValueButton; }
        set { this.m_ShowGetValueButton = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //這裡不用修改
        //欄位的初始化資料都到SetField Method去做
        SetField(m_versionField);
    }

    /// <summary>
    /// 外掛欄位的條件值
    /// </summary>
    public override string ConditionValue
    {
        get
        {
            //回傳字串
            //此字串的內容將會被表單拿來當做條件判斷的值
            string cv = new JGlobalLibs.UOFUtils.ConditionValue()
                .Add("dept", ktxtAPPLICANTDEPT.Text)
                .Add("leacode", kddlLEACODE.SelectedValue)
                .Add("leatimes", ktxtLEAHOURS.Text)
                .Add("leadays", ktxtLEADAYS.Text)
                .Add("title_name", hidLEAEMPTitleName.Value)
                .Add("agentapply", Current.UserGUID != this.ApplicantGuid ? "Y" : "N")
                .Add("has_agent", ktxtLEAAGENT.Text != "" ? "Y" : "N")
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.ConditionValue.cv:{0}", cv));

            return cv;
        }
    }

    /// <summary>
    /// 是否被修改
    /// </summary>
    public override bool IsModified
    {
        get
        {
            //請自行判斷欄位內容是否有被修改
            //有修改回傳True
            //沒有修改回傳False
            return false;
        }
    }

    /// <summary>
    /// 查詢顯示的標題
    /// </summary>
    public override string DisplayTitle
    {
        get
        {
            //表單查詢或WebPart顯示的標題
            //回傳字串
            return string.Format(@"{0}, 假別:{1}, 請假時間:{2}~{3}, 天數:{4}, 時數:{5}",
                ktxtLEAEMP.Text,
                kddlLEACODE.SelectedItem.Text,
                kdtpSTARTTIME.Text,
                kdtpENDTIME.Text,
                ktxtLEADAYS.Text,
                ktxtLEAHOURS.Text);
        }
    }

    /// <summary>
    /// 訊息通知的內容
    /// </summary>
    public override string Message
    {
        get
        {
            //表單訊息通知顯示的內容
            //回傳字串
            return string.Format(@"{0}, 假別:{1}, 請假時間:{2}~{3}, 天數:{4}, 時數:{5}",
                ktxtLEAEMP.Text,
                kddlLEACODE.SelectedItem.Text,
                kdtpSTARTTIME.Text,
                kdtpENDTIME.Text,
                ktxtLEADAYS.Text,
                ktxtLEAHOURS.Text);
        }
    }


    /// <summary>
    /// 真實的值
    /// </summary>
    public override string RealValue
    {
        get
        {
            //回傳字串
            //取得表單欄位簽核者的UsetSet字串
            //內容必須符合EB UserSet的格式
            string rv = new JGlobalLibs.UOFUtils.RealValue()
                .AddPerson("LeaAgent", hidTMP_AGENTID_GUID.Value)
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.RealValue.rv:{0}", rv));
            return rv;
        }
        set
        {
            //這個屬性不用修改
            base.m_fieldValue = value;
        }
    }


    /// <summary>
    /// 欄位的內容
    /// </summary>
    public override string FieldValue
    {
        get
        {
            //回傳字串
            //取得表單欄位填寫的內容
            // 2019/01/31 因為〈UC_ChoiceListMobile〉沒有任何能夠觸發的事件，所以寫在第一個被觸發的點
            //if (hidIsMobileUI.Value == "Y" &&
            //    btnMobLEAAGENT.Visible)
            //{
            //    if (btnMobLEAAGENT.UserSet.Items.Count > 0) // 選擇代理人員
            //    {
            //        ktxtLEAAGENT.Text = btnMobLEAAGENT.UserSet.Items[0].Name;
            //        hidTMP_AGENTID_GUID.Value = btnMobLEAAGENT.UserSet.Items[0].Key;
            //    }
            //    else
            //    {
            //        ktxtLEAAGENT.Text = "";
            //        hidTMP_AGENTID_GUID.Value = "";
            //    }
            //}
            return kytController.FieldValue;
        }
        set
        {
            //這個屬性不用修改
            base.m_fieldValue = value;
        }
    }

    /// <summary>
    /// 是否為第一次填寫
    /// </summary>
    public override bool IsFirstTimeWrite
    {
        get
        {
            //這裡請自行判斷是否為第一次填寫
            return false;
        }
        set
        {
            //這個屬性不用修改
            base.IsFirstTimeWrite = value;
        }
    }

    /// <summary>
    /// 顯示時欄位初始值
    /// </summary>
    /// <param name="versionField">欄位集合</param>
    public override void SetField(Ede.Uof.WKF.Design.VersionField versionField)
    {
        FieldOptional fieldOptional = versionField as FieldOptional;

        if (fieldOptional != null)
        {
            #region ==============屬性說明==============『』
            //fieldOptional.IsRequiredField『是否為必填欄位,如果是必填(True),如果不是必填(False)』
            //fieldOptional.DisplayOnly『是否為純顯示,如果是(True),如果不是(False),一般在觀看表單及列印表單時,屬性為True』
            //fieldOptional.HasAuthority『是否有填寫權限,如果有填寫權限(True),如果沒有填寫權限(False)』
            //fieldOptional.FieldValue『如果已有人填寫過欄位,則此屬性為記錄其內容』
            //fieldOptional.FieldDefault『如果欄位有預設值,則此屬性為記錄其內容』
            //fieldOptional.FieldModify『是否允許修改,如果允許(fieldOptional.FieldModify=FieldModifyType.yes),如果不允許(fieldOptional.FieldModify=FieldModifyType.no)』
            //fieldOptional.Modifier『如果欄位有被修改過,則Modifier的內容為EBUser,如果沒有被修改過,則會等於Null』
            #endregion

            //#region ==============如果沒有填寫權限時,就要顯示有填寫權限人員的清單,只要把以下註解拿掉即可==============
            //if (!fieldOptional.HasAuthority『是否有填寫權限)
            //{
            //    string strItemName = String.Empty;
            //    Ede.Uof.EIP.Organization.Util.UserSet userSet = ((FieldOptional)versionField).FieldControlData;

            //    for (int i = 0; i < userSet.Items.Count; i++)
            //    {
            //        if (i == userSet.Items.Count - 1)
            //        {
            //            strItemName += userSet.Items[i].Name;
            //        }
            //        else
            //        {
            //            strItemName += userSet.Items[i].Name + "、";
            //        }
            //    }

            //    lblHasNoAuthority.ToolTip = lblAuthorityMsg.Text + "：" + strItemName;
            //}
            //#endregion

            #region ==============如果有修改，要顯示修改者資訊==============
            if (fieldOptional.Modifier != null)
            {
                lblModifier.Visible = true;
                lblModifier.ForeColor = System.Drawing.Color.Red;
                lblModifier.Text = String.Format("( {0}：{1} )", this.lblMsgSigner.Text, fieldOptional.Modifier.Name);
            }
            #endregion

            this.FormFieldMode = fieldOptional.FieldMode; // 記住本次 FieldMode

            // 初始化kytcontroller
            kytController = new KYTController(UpdatePanel1);

            // 取得資料庫連通字串
            ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
            service = ConstructorCommonSettings.setSCSSServiceProxDefault();

            //hidIsMobileUI.Value = base.MobileUI ? "Y" : "N"; // 判斷現在是否是MobileUI
            //hidAPIResult.Value = "OK";

            if (!Page.IsPostBack) // 網頁首次載入
            {
                //表單初始化狀態
                gvItemsD1.DataSource = CreatePT_gvItemsD1();
                gvItemsD1.DataBind();

                if (!string.IsNullOrEmpty(fieldOptional.FieldValue))
                    kytController.FieldValue = fieldOptional.FieldValue;

                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀
                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
                btnCal.Visible = false; // 隱藏計算
                //btnCANLEAS.Visible = false; // 隱藏可休假餘額
                btnLEAAGENT.Visible = false; // 隱藏代理人選人按鈕
                btnLEAAGENT_Group.Visible = false; // 隱藏代理人選人按鈕
                //btnMobLEAAGENT.Visible = false; // MOBILEUI版選擇代理人員
                btnLEAEMP.isSearchSelfGroupUsers = false;

                btnNewGvItemD1.Visible = false; // 明細新增按鈕
                ShowHead.Visible = false;

                string leaValue = kddlLEACODE.SelectedValue;
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        hidAPIResult.Value = ""; // 清掉先前的旗標
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                        ktxtMessage.ViewType = KYTViewType.ReadOnly;
                        ktxtAPPLICANTDATE.ReadOnly = true; // 申請日期唯讀
                        ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                        ktxtLEAEMP.ReadOnly = true; // 請假人唯讀
                        //ktxtLEAAGENT.Visible = false; // 隱藏代理人
                        ktxtLEAAGENT.ReadOnly = true; // 代理人唯讀
                        ktxtLEAHOURS.ReadOnly = true; // 請假時數唯讀
                        ktxtLEADAYS.ReadOnly = true; // 請假天數唯讀
                        btnCal.Visible = true; // 顯示計算
                        //btnCANLEAS.Visible = true; // 顯示可休假餘額
                        //if (hidIsMobileUI.Value == "N") // 現在是MOBILEUI之外的畫面
                        //{
                        //    btnLEAAGENT.Visible = true; // 隱藏代理人選人按鈕
                        //    btnMobLEAAGENT.Visible = false; // MOBILEUI版選擇代理人員
                        //    ktxtLEAAGENT.Visible = true; // 隱藏代理人
                        //}
                        //else
                        //{
                        //    btnLEAAGENT.Visible = false; // 隱藏代理人選人按鈕
                        //    btnMobLEAAGENT.Visible = true; // MOBILEUI版選擇代理人員
                        //    ktxtLEAAGENT.Visible = false; // 隱藏代理人
                        //}
                        if (fieldOptional.FieldMode == FieldMode.Applicant &&
                            string.IsNullOrEmpty(fieldOptional.FieldValue)) // 起單時專用操作狀態或初始值
                        {
                            // 草稿ID，查詢有無上傳附件
                            hidFormScriptID.Value = Request["scriptId"] != null ? (string)Request["scriptId"] : "";
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            ktxtAPPLICANTDATE.Text = DateTime.Now.ToString("yyyy/MM/dd"); // 設定申請日期
                            ktxtLEAEMP.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account); ; // 設定請假人資訊
                            hidLEAEMP.Value = KUser.UserGUID;
                            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 設定部門資訊
                            hidAPPLICANTDEPT.Value = KUser.GroupID[0];
                            hidLEAEMPTitleId.Value = KUser.Title_ID[0];
                            hidLEAEMPTitleName.Value = KUser.Title_Name[0];
                            hidLEAEMPAccount.Value = KUser.Account;
                            string[] sAccount = hidLEAEMPAccount.Value.Split('\\');
                            hidLEAEMPAccount.Value = sAccount[sAccount.Length - 1];
                            hidGROUPCODE.Value = KUser.GroupCode[0];


                            // 請假單預設請假時間
                            DateTime dtDefaultStart = DateTime.MinValue;
                            DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_START_TIME), out dtDefaultStart);
                            if (dtDefaultStart > DateTime.MinValue)
                            {
                                kdtpSTARTTIME.Text = dtDefaultStart.ToString("yyyy/MM/dd HH:mm");
                            }
                            DateTime dtDefaultEnd = DateTime.MinValue;
                            DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_END_TIME), out dtDefaultEnd);
                            if (dtDefaultEnd > DateTime.MinValue)
                            {
                                kdtpENDTIME.Text = dtDefaultEnd.ToString("yyyy/MM/dd HH:mm");
                            }
                        }
                        else // 退回申請者
                        {
                            hidDOC_NBR.Value = taskObj != null ? taskObj.FormNumber : "";
                        }
                        // 使用UOF代理人設定
                        if (SCSHR.SCSHRConfiguration.AGENT_FUNC.ToUpper() == "AGENT")
                        {
                            btnLEAAGENT.Visible = true; // 顯示代理人選人按鈕
                            using (SqlDataAdapter sda = new SqlDataAdapter(@"SELECT COUNT(*) 
                                                                               FROM TB_EB_USER_AGENT
                                                                              WHERE USER_GUID = @USER_GUID", ConnectionString))
                            using (DataSet ds = new DataSet())
                            {
                                sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", hidLEAEMP.Value);
                                try
                                {
                                    if (sda.Fill(ds) > 0)
                                    {
                                        if (((int)ds.Tables[0].Rows[0][0]) > 0)
                                        {
                                            btnLEAAGENT.UserSql = string.Format(@"SELECT ACCOUNT,NAME,USER_GUID,'' AS 'GROUP_ID' FROM TB_EB_USER WHERE USER_GUID IN (SELECT AGENT_USER FROM TB_EB_USER_AGENT WHERE USER_GUID = '{0}') AND IS_SUSPENDED = 0 AND (EXPIRE_DATE IS NULL OR EXPIRE_DATE > GETDATE())",
                                                     hidLEAEMP.Value);
                                        }
                                        else
                                        {
                                            btnLEAAGENT.GroupID = this.ApplicantGroupId;
                                            btnLEAAGENT.isSearchSelfGroupUsers = true;
                                        }
                                    }

                                }
                                catch (Exception e)
                                {
                                    KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.SetField.SELECT.TB_EB_USER_AGENT.ERROR:{0}", e.Message));
                                    KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.SetField.SELECT.TB_EB_USER_AGENT.ERROR.STACKTRACE:{0}", e.StackTrace));
                                }
                            }
                        }
                        else if (SCSHR.SCSHRConfiguration.AGENT_FUNC.ToUpper() == "ALL")
                        {
                            btnLEAAGENT_Group.Visible = true; // 顯示代理人選人按鈕
                            // 選人語法用預設
                            //btnLEAAGENT_Group.UserSql = string.Format(@"
                            //SELECT A.ACCOUNT,A.[NAME],A.USER_GUID, B.GROUP_ID
                            //  FROM TB_EB_USER AS A, TB_EB_EMPL_DEP AS B
                            // WHERE B.GROUP_ID = @GROUP_ID
                            //   AND A.IS_SUSPENDED = 0 
                            //   AND (A.EXPIRE_DATE IS NULL 
                            //       OR A.EXPIRE_DATE > GETDATE())
                            //");
                        }
                        // 2018/12/20 因為KYT的缺陷(暫不修改)的問題，每次都重新綁定DataSource
                        //string leaValue = kddlLEACODE.SelectedValue;
                        // 接收TMP_NEEDATTACH，用來判斷是否要檢查附件
                        DataTable dtLEASource = getLEAVEType(SCSHRConfiguration.SCSSLeaveTypeProgID, "*");
                        DataTable dtSpecSettingSource = getLEAVEType("ATT0011300", "*");
                        // 將SFSTARTDATE對應到dtLEASource中
                        foreach (DataColumn dc in dtSpecSettingSource.Columns)
                        {
                            if (!dtLEASource.Columns.Contains(dc.ColumnName))
                                dtLEASource.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                        }
                        foreach (DataRow dr in dtLEASource.Rows)
                        {
                            foreach (DataRow _dr in dtSpecSettingSource.Rows)
                            {
                                if (dr["VACATIONID"].ToString().Equals(_dr["SYS_ID"].ToString()))
                                {
                                    dr["SFSTARTDATE"] = _dr["SFSTARTDATE"];
                                    break;
                                }
                            }
                        }
                        // 將過期或尚未到達的可用假別去掉
                        for (int i = dtLEASource.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtLEASource.Rows[i];
                            DateTime dtBegin = DateTime.MinValue;
                            DateTime dtEnd = DateTime.MaxValue;
                            DateTime.TryParse(dr["SYS_BEGINDATE"].ToString(), out dtBegin);
                            bool isNotEndMin = DateTime.TryParse(dr["SYS_ENDDATE"].ToString(), out dtEnd);
                            if (!isNotEndMin) dtEnd = DateTime.MaxValue;
                            if (DateTime.Now.Date < dtBegin.Date || // 如果現在日期大於等於生效日
                                    DateTime.Now.Date > dtEnd.Date) // 或者現在日期小於等於到期日
                            {
                                dtLEASource.Rows.RemoveAt(i); // 移除這筆資料
                            }
                        }
                        // 新增請選擇選項
                        DataRow ndr = dtLEASource.NewRow();
                        ndr["SYS_VIEWID"] = "";
                        ndr["SYS_NAME"] = "===請選擇===";
                        dtLEASource.Rows.InsertAt(ndr, 0);
                        // 綁定子假別
                        //kddlLEACODE.BindDataOnly = true;
                        kddlLEACODE.DataSource = dtLEASource;
                        kddlLEACODE.DataValueField = "SYS_VIEWID";
                        kddlLEACODE.DataTextField = "SYS_NAME";
                        kddlLEACODE.DataBind();
                        kddlLEACODE.SelectedValue = leaValue;
                        kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);
                        KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.SetField.Applicant.dtLEASource:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtLEASource)));
                        // 設定Picker是否能輸入
                        kdtpSTARTTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpENDTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        // 草稿起單，一律清空明細
                        gvItemsD1.DataSource = CreatePT_gvItemsD1();
                        gvItemsD1.DataBind();
                        ShowHead.Visible = true;

                        // 將草稿暫存的請假人復原
                        if (!string.IsNullOrEmpty(ktxtLEAEMP.Text))
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add(new DataColumn("ACCOUT", typeof(string))); // 請假人帳號
                            dt.Columns.Add(new DataColumn("GUID", typeof(string))); // 請假人GUID
                            dt.Columns.Add(new DataColumn("NAME", typeof(string))); // 請假人姓名
                            dt.Columns.Add(new DataColumn("GROUP_CODE", typeof(string))); // 請假人部門CODE
                            dt.Columns.Add(new DataColumn("GROUP_NAME", typeof(string))); // 請假人部門名稱
                            dt.Columns.Add(new DataColumn("GROUP_ID", typeof(string))); // 請假人GROUP_ID
                            dt.Columns.Add(new DataColumn("LEADAYS", typeof(decimal))); // 請假天數
                            dt.Columns.Add(new DataColumn("LEAHOURS", typeof(decimal))); // 請假時設

                            string[] arrLeaUser = ktxtLEAEMP.Text.Split(',');
                            List<string> litLeaUser = new List<string>();
                            foreach (string name in arrLeaUser)
                            {
                                string acount = name.Split('(')[1].Substring(0, name.Split('(')[1].Length - 1);
                                //EBUser user = KYTUtilLibs.Utils.UOFUtils.UOFUser.getUserInfo(acount);
                                KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByAccount(acount); // 人員

                                string[] sAccount = KUser.Account.Split('\\');

                                DataRow ndrUser = dt.NewRow();
                                ndrUser["ACCOUT"] = sAccount[sAccount.Length - 1];
                                ndrUser["GUID"] = KUser.UserGUID;
                                ndrUser["NAME"] = KUser.Name;
                                ndrUser["GROUP_CODE"] = KUser.GroupCode[0];
                                ndrUser["GROUP_NAME"] = KUser.GroupName[0];
                                ndrUser["GROUP_ID"] = KUser.GroupID[0];
                                dt.Rows.Add(ndrUser);
                            }
                            ViewState["LEAEMP"] = dt;
                        }


                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.Print: // 表單列印
                    case FieldMode.View: // 表單觀看
                    case FieldMode.Signin: // 表單簽核
                        //2019 / 03 / 18 因為KYT的缺陷(暫不修改)的問題，每次都重新綁定DataSource
                        //string leaValue = kddlLEACODE.SelectedValue;
                        // 接收TMP_NEEDATTACH，用來判斷是否要檢查附件
                        DataTable _dtLEASource = getLEAVEType(SCSHRConfiguration.SCSSLeaveTypeProgID, "*");
                        DataTable _dtSpecSettingSource = getLEAVEType("ATT0011300", "*");
                        // 將SFSTARTDATE對應到dtLEASource中
                        foreach (DataColumn dc in _dtSpecSettingSource.Columns)
                        {
                            if (!_dtLEASource.Columns.Contains(dc.ColumnName))
                                _dtLEASource.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                        }
                        foreach (DataRow dr in _dtLEASource.Rows)
                        {
                            foreach (DataRow _dr in _dtSpecSettingSource.Rows)
                            {
                                if (dr["VACATIONID"].ToString().Equals(_dr["SYS_ID"].ToString()))
                                {
                                    dr["SFSTARTDATE"] = _dr["SFSTARTDATE"];
                                    break;
                                }
                            }
                        }
                        // 將過期或尚未到達的可用假別去掉
                        for (int i = _dtLEASource.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = _dtLEASource.Rows[i];
                            DateTime dtBegin = DateTime.MinValue;
                            DateTime dtEnd = DateTime.MaxValue;
                            DateTime.TryParse(dr["SYS_BEGINDATE"].ToString(), out dtBegin);
                            bool isNotEndMin = DateTime.TryParse(dr["SYS_ENDDATE"].ToString(), out dtEnd);
                            if (!isNotEndMin) dtEnd = DateTime.MaxValue;
                            if (DateTime.Now.Date < dtBegin.Date || // 如果現在日期大於等於生效日
                                    DateTime.Now.Date > dtEnd.Date) // 或者現在日期小於等於到期日
                            {
                                _dtLEASource.Rows.RemoveAt(i); // 移除這筆資料
                            }
                        }
                        // 新增請選擇選項
                        DataRow _ndr = _dtLEASource.NewRow();
                        _ndr["SYS_VIEWID"] = "";
                        _ndr["SYS_NAME"] = "===請選擇===";
                        _dtLEASource.Rows.InsertAt(_ndr, 0);
                        // 綁定子假別
                        //kddlLEACODE.BindDataOnly = true;
                        kddlLEACODE.DataSource = _dtLEASource;
                        kddlLEACODE.DataValueField = "SYS_VIEWID";
                        kddlLEACODE.DataTextField = "SYS_NAME";
                        kddlLEACODE.DataBind();
                        kddlLEACODE.SelectedValue = leaValue;
                        kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);
                        lblAGENT.Text = kchkAGENT.Checked ? "是" : "否";
                        hidDOC_NBR.Value = taskObj.FormNumber;
                        break;
                }
                #region 代理人顯示開關

                bool is_show_Agent = SCSHRConfiguration.IS_LEV_AGENT_SHOW.ToUpper() == "Y";
                divAgentTitle.Visible = is_show_Agent;
                divAgent.Visible = is_show_Agent;
                divSetAgentTitle.Visible = is_show_Agent;
                divSetAgent.Visible = is_show_Agent;
                bool is_showAgent = SCSHRConfiguration.IS_SHOWAGENT.ToUpper() == "Y";
                divSetAgentTitle.Visible = is_showAgent;
                divSetAgent.Visible = is_showAgent;

                #endregion 代理人顯示開關
            }
            else // 如果網頁POSTBACK
            {
                JGlobalLibs.WebUtils.RequestHiddenFields(UpdatePanel1); // 取回HiddenField的值
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        // 設定Picker是否能輸入
                        kdtpSTARTTIME.ViewType = KYTViewType.Input;
                        kdtpENDTIME.ViewType = KYTViewType.Input;
                        kdtpSTARTTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpENDTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        break;
                }
            }

        }
    }

    #region ==============修改權限LinkButton的事件==============
    protected void lnk_Edit_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        lnk_Cannel.Visible = true;
        lnk_Edit.Visible = false;
        lnk_Submit.Visible = true;
    }
    protected void lnk_Cannel_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        SetField(m_versionField);

        lnk_Cannel.Visible = false;
        lnk_Edit.Visible = true;
        lnk_Submit.Visible = false;
    }
    protected void lnk_Submit_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        lnk_Cannel.Visible = false;
        lnk_Edit.Visible = true;
        lnk_Submit.Visible = false;

        //儲存表單資料
        if (base._IFieldOOServer.Count == 0) return;
        ((IFieldCompetenceServer)base._IFieldOOServer[0]).SaveForm();
    }
    #endregion                

    private DataTable getLEAVEType(string progId, string selectFields)
    {
        DataTable dtScource = new DataTable();
        Exception ex = null;
        dtScource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.getLEAVEType.service.Error:{0}", ex.Message));
        for (int i = dtScource.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = dtScource.Rows[i];
            string lev_type = SCSHR.SCSHRConfiguration.LEV_TYPE_FILTER;
            string[] lev_filters = lev_type.Split(',');
            if (!string.IsNullOrEmpty(lev_type) &&
                lev_filters.Any(levtype => ((string)dr["SYS_VIEWID"]).Equals(levtype)))
            {
                dtScource.Rows.RemoveAt(i);
            }

            //switch ((string)dr["SYS_VIEWID"])
            //{
            //    case "2101": // 曠職
            //    case "2201": // 早退
            //    case "2202": // 遲到
            //    //case "2301": // 忘帶卡
            //        dtScource.Rows.RemoveAt(i);
            //        break;
            //}
        }
        return dtScource;
    }

    private void RefreshMainData(string EMPID, string LEVCODE, string LEVNAME, string LEVDATE_START, string LEVDATE_END, string IS_SPEC)
    {
        DateTime dtStart = DateTime.MinValue;
        DateTime.TryParse(LEVDATE_START, out dtStart);
        DateTime dtEnd = DateTime.MinValue;
        DateTime.TryParse(LEVDATE_END, out dtEnd);
        Exception ex = null;
        SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSLeaveProgID,
           string.IsNullOrEmpty(IS_SPEC) ? "GetEmpAllLRData" : "GetLeaveSpecInfo",
            SCSHR.Types.SCSParameter.getPatameters(new { TMP_EmployeeID = EMPID, Tmp_svacationID = LEVCODE, StartDate = dtStart.ToString("yyyyMMdd"), EndDate = dtEnd.ToString("yyyyMMdd") }),
            out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"listLEAVE_TIMES.RefreshMainData.GetEmpAllLRData.ERROR:{0}", ex.Message));
        if (parameters != null &&
                    parameters.Length > 0)
        {
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"listLEAVE_TIMES.RefreshMainData.GetEmpAllLRData.result:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));
            if (parameters[0].DataType.ToString() == "DataTable")
            {
                DataTable dtSource = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                dtSource.Columns.Add("LEVNAME", typeof(string));
                if (string.IsNullOrEmpty(IS_SPEC))
                {
                    foreach (DataRow dr in dtSource.Rows)
                    {
                        dr["LEVNAME"] = dr["SVacationName"];
                    }
                }
                else
                {
                    foreach (DataRow dr in dtSource.Rows)
                    {
                        dr["LEVNAME"] = LEVNAME;
                    }
                }
                gvMain.DataSource = dtSource;
                gvMain.DataBind();
                gvMain.Visible = dtSource.Rows.Count > 0;
            }
        }

    }

    /// <summary>
    /// 檢查是否是特別假
    /// </summary>
    /// <param name="dtSource"></param>
    /// <param name="selectedValue"></param>
    /// <param name="needAttach"></param>
    /// <returns></returns>
    private bool checkIsSpecDate(DataTable dtSource, string selectedValue, out string needAttach, out string SEXAPPLY)
    {
        bool isSFDate = false;
        needAttach = "";
        SEXAPPLY = "0";
        if (dtSource != null)
        {
            // 用Select都會有奇妙的錯，所以自己跑回圈找，非常浪費效能
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr["SYS_VIEWID"].ToString().Equals(selectedValue))
                {
                    isSFDate = dr["SFSTARTDATE"] != Convert.DBNull ? (bool)dr["SFSTARTDATE"] : false;
                    needAttach = dr["TMP_NEEDATTACH"].ToString();
                    SEXAPPLY = dr["TMP_LIMITSEXAPPLY"].ToString();
                    break;
                }
            }
        }
        return isSFDate;
    }

    protected void kddlLEACODE_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTDropDownList _kddlLEACODE = (KYTDropDownList)sender;
        DataTable dtLEASource = _kddlLEACODE.DataSource as DataTable;

        bool isSFDate = false;
        string needAttach = "";
        string SEXAPPLY = "";
        isSFDate = checkIsSpecDate(dtLEASource, _kddlLEACODE.SelectedValue, out needAttach, out SEXAPPLY);
        hidNeedAttach.Value = needAttach;
        hidSEXAPPLY.Value = SEXAPPLY;
        string IS_SPEC = "";
        if (isSFDate) // 現在是特別假
        {
            IS_SPEC = "1";
            //if (this.FormFieldMode == FieldMode.Applicant ||
            //    this.FormFieldMode == FieldMode.ReturnApplicant) // 起單關才清除
            //    kdpSP_DATE.Text = "";
            divSP.Visible = true;
        }
        else // 一般假別
        {
            if (this.FormFieldMode == FieldMode.Applicant ||
                   this.FormFieldMode == FieldMode.ReturnApplicant)  // 起單關才清除
                kdpSP_DATE.Text = "";
            ktxtSP_NAME.Text = "";
            divSP.Visible = false;
        }
        hidAPIResult.Value = ""; // 有更動就清空
        lblAPIResultError.Text = "";

        if (SCSHRConfiguration.IS_LEAVE_DETAIL == "Y")
        {
            DateTime today = DateTime.Now;
            RefreshMainData(hidLEAEMPAccount.Value, _kddlLEACODE.SelectedValue, kddlLEACODE.SelectedItem.Text, today.ToString("yyyy/MM/dd"), today.AddMonths(1).ToString("yyyy/MM/dd"), IS_SPEC);

            //RefreshMainData(hidLEAEMPAccount.Value, _kddlLEACODE.SelectedValue, kddlLEACODE.SelectedItem.Text, kdtpSTARTTIME.Text, kdtpENDTIME.Text, IS_SPEC);
        }

    }

    //protected void ubtnLEAAGENT_EditButtonOnClick(UserSet userSet)
    //{
    //    if (userSet.Items.Count > 0)
    //    {
    //        // 只取第一筆
    //        ktxtLEAAGENT.Text = userSet.Items[0].Name;
    //        hidTMP_AGENTID_GUID.Value = userSet.Items[0].Key;
    //    }
    //    else
    //    {
    //        ktxtLEAAGENT.Text = "";
    //        hidTMP_AGENTID_GUID.Value = "";
    //    }
    //}

    /// <summary>
    /// 計算
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCal_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ktxtREMARK.Text))
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"
                    alert('必須填寫請假說明');
                    "), true);
            return;
        }
        ktxtMessage.Text = "";
        ktxtLEAHOURS.Text = "0";
        ktxtLEADAYS.Text = "0";
        hidAPIResult.Value = "";
        lblAPIResultError.Text = "";

        hidConfirm.Value = "";
        if (!string.IsNullOrEmpty(kdtpSTARTTIME.Text) &&
            !string.IsNullOrEmpty(kdtpENDTIME.Text))
        {
            DataTable dtLEASource = kddlLEACODE.DataSource as DataTable;

            bool isSFDate = false;
            string needAttach = "";
            string SEXAPPLY = "";
            isSFDate = checkIsSpecDate(dtLEASource, kddlLEACODE.SelectedValue, out needAttach, out SEXAPPLY);

            if (isSFDate && // 當現在是特別假
                string.IsNullOrEmpty(kdpSP_DATE.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"
                    alert('必須選擇特殊日期');
                    "), true);
                return;
            }

            if (isSFDate && // 當現在是特別假
                string.IsNullOrEmpty(ktxtSP_NAME.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"
                    alert('必須選擇特殊日對象');
                    "), true);
                return;
            }

            DataTable dtResult = null;
            bool resultStatus = false;
            hidAPIResult.Value = ""; // 清空之前的API查詢結果
            // 資料檢核
            DateTime dtStart = DateTime.MinValue;
            DateTime.TryParse(kdtpSTARTTIME.Text, out dtStart);
            DateTime dtEnd = DateTime.MinValue;
            DateTime.TryParse(kdtpENDTIME.Text, out dtEnd);


            // 迴圈檢查所有請假人
            string ErrorMSG_ALL = "";
            DataTable dt = (DataTable)ViewState["LEAEMP"];
            foreach (DataRow drLEAEMP in dt.Rows)
            {
                string ErrorMSG = "";
                string ACCOUT = drLEAEMP["ACCOUT"].ToString();
                string GUID = drLEAEMP["GUID"].ToString();
                string NAME = drLEAEMP["NAME"].ToString();
                string GROUP_CODE = drLEAEMP["GROUP_CODE"].ToString();
                string GROUP_NAME = drLEAEMP["GROUP_NAME"].ToString();
                string GROUP_ID = drLEAEMP["GROUP_ID"].ToString();
                decimal LEADAYS = 0;
                decimal LEAHOURS = 0;

                #region 呼叫飛騰-檢查此人是否可申請
                string[] sAccount = hidLEAEMPAccount.Value.Split('\\');
                JArray jaTable = new JArray();
                JObject _joTable = new JObject();
                _joTable.Add(new JProperty("USERNO", "1"));
                _joTable.Add(new JProperty("SYS_VIEWID", ""));
                _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(ktxtAPPLICANTDATE.Text).ToString("yyyyMMdd")));
                //_joTable.Add(new JProperty("TMP_EMPLOYEEID", ACCOUT)); // 取最右邊
                KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByAccount(ACCOUT);
                string AGENTID = GetFirstAgent(KUser.UserGUID);
                _joTable.Add(new JProperty("TMP_EMPLOYEEID", KUser.EmployeeNo));
                _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd")));
                _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm")));
                _joTable.Add(new JProperty("TMP_SVACATIONID", kddlLEACODE.SelectedValue));
                _joTable.Add(new JProperty("ENDDATE", dtEnd.ToString("yyyyMMdd")));
                _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm")));
                _joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(AGENTID) ? new KYT_UserPO().GetUserDetailByUserGuid(AGENTID.Split(',')[0]).EmployeeNo : KUser.EmployeeNo));
                _joTable.Add(new JProperty("NOTE", ktxtREMARK.Text.Trim()));
                if (!string.IsNullOrEmpty(ktxtSP_NAME.Text))
                {
                    //_joTable.Add(new JProperty("SPECIALDATE", kddlSP_DATE.SelectedValue));
                    _joTable.Add(new JProperty("SPECIALDATE", kdpSP_DATE.Text));
                    _joTable.Add(new JProperty("STARGETNAME", ktxtSP_NAME.Text));
                }
                jaTable.Add(_joTable);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.JATABLE:{0}", jaTable));
                DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
                DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
                dsSource.Tables.Add(dtSource);
                Exception ex = null;
                dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOImport::{0}::Result::{1}", SCSHRConfiguration.SCSSLeaveProgID, Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
                if (ex != null)
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOImport.ERROR:{0}", ex.Message));
                if (dtResult != null &&
                    dtResult.Rows.Count > 0)
                {
                    resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
                    if (!resultStatus)
                    {
                        ErrorMSG = dtResult.Rows[0]["_MESSAGE"].ToString();
                        lblAPIResultError.Text = dtResult.Rows[0]["_MESSAGE"].ToString();
                    }
                }

                #endregion 呼叫飛騰-檢查此人是否可申請

                if (resultStatus)
                {
                    hidAPIResult.Value = "OK";

                    // 計算時間
                    ex = null; // 初始化
                    DataTable dtCalcResult = null;
                    SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSGetLeaveHourProgID,
                    "GetLeaveHours",
                    SCSHR.Types.SCSParameter.getPatameters(new
                    {
                        TMP_EmployeeID = ACCOUT,
                        TMP_SVacationID = kddlLEACODE.SelectedValue,
                        StartDate = dtStart.ToString("yyyyMMdd"),
                        StartTime = dtStart.ToString("HHmm"),
                        EndDate = dtEnd.ToString("yyyyMMdd"),
                        EndTime = dtEnd.ToString("HHmm")
                    }),
                    out ex);
                    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOExecFunc::{0}::Result::{1}", SCSHRConfiguration.SCSSGetLeaveHourProgID, Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));

                    if (ex != null)
                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOExecFunc.ERROR:{0}", ex.Message));
                    if (parameters != null &&
                        parameters.Length > 0)
                    {
                        if (parameters[0].DataType.ToString() == "DataTable")
                        {
                            //DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOExecFunc.Result.XML:{0}", parameters[0].Xml));

                            dtCalcResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                        }
                    }
                    if (dtCalcResult != null &&
                        dtCalcResult.Rows.Count > 0)
                    {
                        ktxtLEAHOURS.Text = dtCalcResult.Rows[0]["LeaveHours"].ToString();
                        ktxtLEADAYS.Text = dtCalcResult.Rows[0]["LeaveDays"].ToString();
                        LEADAYS = decimal.Parse(dtCalcResult.Rows[0]["LeaveDays"].ToString());
                        LEAHOURS = decimal.Parse(dtCalcResult.Rows[0]["LeaveHours"].ToString());
                        drLEAEMP["LEADAYS"] = LEADAYS;
                        drLEAEMP["LEAHOURS"] = LEAHOURS;
                    }

                    //// 為了滿足檢查完畢後需出現檢查成功
                    //lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查成功" : lblAPIResultError.Text;

                    // 檢查成功後 -> 檢查明細項內請假日期區間不可重覆， 沒重覆->新增，重覆->不新增
                    DataTable dtD1 = gvItemsD1.DataTable;
                    string filter = "TMP_EMPLOYEEID_GUID ='" + GUID +
                                                "' AND ((START >= '" + kdtpSTARTTIME.Text + "' AND START < '" + kdtpENDTIME.Text + "') OR" +
                                                       "(END > '" + kdtpSTARTTIME.Text + "' AND END <= '" + kdtpENDTIME.Text + "') OR" +
                                                       "(START <= '" + kdtpSTARTTIME.Text + "' AND END >= '" + kdtpENDTIME.Text + "'))";
                    DataRow[] row = dtD1.Select(filter);

                    if (row.Length > 0)
                    {
                        // 重覆
                        ErrorMSG = "明細項已存在相同請假區間，不予新增";
                        //lblAPIResultError.Text = "檢查成功，明細項已存在相同請假區間，不予新增";
                    }
                    else
                    {
                        // 不重覆
                        //btnNewGvItemD1_Click(null, null);
                        DataTable tblgvItems_D1 = gvItemsD1.DataTable;
                        DataRow ndr = tblgvItems_D1.NewRow();
                        //string agent = GetFirstAgent(GUID);
                        ndr["ITEM_NO"] = "1";
                        ndr["SYS_VIEWID"] = ""; // 編號
                        ndr["APPLICANTDEPT"] = GROUP_NAME; // 部門*
                        ndr["SYS_DATE"] = ktxtAPPLICANTDATE.Text; // 申請日期
                        ndr["TMP_EMPLOYEEID"] = NAME; // 請假人*
                        ndr["TMP_EMPLOYEEID_GUID"] = GUID; // 請假人GUID*
                        ndr["TMP_AGENTID"] = !string.IsNullOrEmpty(AGENTID) ? AGENTID.Split(',')[1] : NAME; // 代理人
                        ndr["TMP_AGENTID_GUID"] = !string.IsNullOrEmpty(AGENTID) ? AGENTID.Split(',')[0] : GUID; // 代理人GUID
                        ndr["TMP_SVACATIONID"] = kddlLEACODE.SelectedValue; // 假別
                        ndr["NEED_ATTACH"] = hidNeedAttach.Value; // 特別假別
                        ndr["SEXAPPLY"] = hidSEXAPPLY.Value; // 假別性別
                        ndr["TMP_SVACATIONID_TXT"] = kddlLEACODE.SelectedItem.Text; // 假別
                        ndr["IS_AGENTID"] = kchkAGENT.Checked; // 啟動代理
                        ndr["SPECIALDATE"] = kdpSP_DATE.Text; // 特殊日期
                        ndr["STARGETNAME"] = ktxtSP_NAME.Text; // 特殊日對象
                        ndr["START"] = dtStart; // 請假時間(起)
                        ndr["STARTDATE"] = dtStart.ToString("yyyyMMdd"); // 請假時間(起)
                        ndr["STARTTIME"] = dtStart.ToString("HHmm"); // 請假時間(起)
                        ndr["END"] = dtEnd; // 請假時間(迄)
                        ndr["ENDDATE"] = dtEnd.ToString("yyyyMMdd"); // 請假時間(迄)
                        ndr["ENDTIME"] = dtEnd.ToString("HHmm"); // 請假時間(迄)
                        ndr["LEADAYS"] = LEADAYS; // 請假天數
                        ndr["LEAHOURS"] = LEAHOURS; // 請假時數
                        ndr["NOTE"] = ktxtREMARK.Text; // 請假說明
                        tblgvItems_D1.Rows.Add(ndr);
                        tblgvItems_D1.AcceptChanges();
                        this.ResetGridViewITEMNO(tblgvItems_D1, "ITEM_NO"); //重新排序項目編號
                        gvItemsD1.DataSource = tblgvItems_D1;
                        gvItemsD1.DataBind();

                        ErrorMSG = "檢查成功，並新增至請假明細";
                        //lblAPIResultError.Text = "檢查成功，並新增至請假明細";
                    }
                }
                else
                {
                    ErrorMSG = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查失敗" : lblAPIResultError.Text;
                    //lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查失敗" : lblAPIResultError.Text;
                }

                ErrorMSG_ALL += string.Format("<br />請假人: {0}({1})，{2}", NAME, ACCOUT, ErrorMSG);
            }
            lblAPIResultError.Text = ErrorMSG_ALL;
            #region 原版-只檢查單一人
            //string[] sAccount = hidLEAEMPAccount.Value.Split('\\');
            //JArray jaTable = new JArray();
            //JObject _joTable = new JObject();
            //_joTable.Add(new JProperty("USERNO", "1"));
            //_joTable.Add(new JProperty("SYS_VIEWID", ""));
            //_joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(ktxtAPPLICANTDATE.Text).ToString("yyyyMMdd")));
            //_joTable.Add(new JProperty("TMP_EMPLOYEEID", sAccount[sAccount.Length - 1])); // 取最右邊
            //_joTable.Add(new JProperty("TMP_SVACATIONID", kddlLEACODE.SelectedValue));
            ////_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(hidTMP_AGENTID_GUID.Value) ?
            ////    JGlobalLibs.UOFUtils.getUserAccount(hidTMP_AGENTID_GUID.Value) :
            ////    btnMobLEAAGENT.UserSet.Items.Count > 0 ?
            ////    !string.IsNullOrEmpty(btnMobLEAAGENT.UserSet.Items[0].Key) ?
            ////    btnMobLEAAGENT.UserSet.Items[0].Key :
            ////    "" :
            ////    ""));
            //string AGENTID = JGlobalLibs.UOFUtils.getUserAccount(hidTMP_AGENTID_GUID.Value);
            //_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(AGENTID) ? AGENTID : "admin"));
            //_joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd")));
            //_joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm")));
            //_joTable.Add(new JProperty("ENDDATE", dtEnd.ToString("yyyyMMdd")));
            //_joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm")));
            //_joTable.Add(new JProperty("NOTE", ktxtREMARK.Text.Trim()));
            //if (!string.IsNullOrEmpty(ktxtSP_NAME.Text))
            //{
            //    //_joTable.Add(new JProperty("SPECIALDATE", kddlSP_DATE.SelectedValue));
            //    _joTable.Add(new JProperty("SPECIALDATE", kdpSP_DATE.Text));
            //    _joTable.Add(new JProperty("STARGETNAME", ktxtSP_NAME.Text));
            //}
            //jaTable.Add(_joTable);
            //DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.JATABLE:{0}", jaTable));
            //DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
            //dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
            //DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
            //dsSource.Tables.Add(dtSource);
            //Exception ex = null;
            //dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
            //DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOImport::{0}::Result::{1}", SCSHRConfiguration.SCSSLeaveProgID, Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
            //if (ex != null)
            //    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOImport.ERROR:{0}", ex.Message));
            //if (dtResult != null &&
            //    dtResult.Rows.Count > 0)
            //{
            //    resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
            //    if (!resultStatus)
            //        lblAPIResultError.Text = dtResult.Rows[0]["_MESSAGE"].ToString();
            //}

            //if (resultStatus)
            //{
            //    hidAPIResult.Value = "OK";
            //    // 計算時間
            //    ex = null; // 初始化
            //    DataTable dtCalcResult = null;
            //    SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSGetLeaveHourProgID,
            //    "GetLeaveHours",
            //    SCSHR.Types.SCSParameter.getPatameters(new
            //    {
            //        TMP_EmployeeID = sAccount[sAccount.Length - 1],
            //        TMP_SVacationID = kddlLEACODE.SelectedValue,
            //        StartDate = dtStart.ToString("yyyyMMdd"),
            //        StartTime = dtStart.ToString("HHmm"),
            //        EndDate = dtEnd.ToString("yyyyMMdd"),
            //        EndTime = dtEnd.ToString("HHmm")
            //    }),
            //    out ex);
            //    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOExecFunc::{0}::Result::{1}", SCSHRConfiguration.SCSSGetLeaveHourProgID, Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));

            //    if (ex != null)
            //        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOExecFunc.ERROR:{0}", ex.Message));
            //    if (parameters != null &&
            //        parameters.Length > 0)
            //    {
            //        if (parameters[0].DataType.ToString() == "DataTable")
            //        {
            //            //DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.btnCal_Click.BOExecFunc.Result.XML:{0}", parameters[0].Xml));

            //            dtCalcResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
            //        }
            //    }
            //    if (dtCalcResult != null &&
            //        dtCalcResult.Rows.Count > 0)
            //    {
            //        ktxtLEAHOURS.Text = dtCalcResult.Rows[0]["LeaveHours"].ToString();
            //        ktxtLEADAYS.Text = dtCalcResult.Rows[0]["LeaveDays"].ToString();
            //    }

            //    // 為了滿足檢查完畢後需出現檢查成功
            //    lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查成功" : lblAPIResultError.Text;

            //    // 檢查成功後 -> 檢查明細項內請假日期區間不可重覆， 沒重覆->新增，重覆->不新增
            //    DataTable dtD1 = gvItemsD1.DataTable;
            //    string filter = "TMP_EMPLOYEEID_GUID ='" + hidLEAEMP.Value +
            //                                "' AND ((START >= '" + kdtpSTARTTIME.Text + "' AND START < '" + kdtpENDTIME.Text + "') OR" +
            //                                       "(END > '" + kdtpSTARTTIME.Text + "' AND END <= '" + kdtpENDTIME.Text + "') OR" +
            //                                       "(START <= '" + kdtpSTARTTIME.Text + "' AND END >= '" + kdtpENDTIME.Text + "'))";
            //    DataRow[] row = dtD1.Select(filter);

            //    if (row.Length > 0)
            //    {
            //        // 重覆
            //        lblAPIResultError.Text = "檢查成功，明細項已存在相同請假區間，不予新增";
            //    }
            //    else
            //    {
            //        // 不重覆
            //        btnNewGvItemD1_Click(null, null);
            //        lblAPIResultError.Text = "檢查成功，並新增至請假明細";
            //    }
            //}
            //else
            //    lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查失敗" : lblAPIResultError.Text;

            #endregion  原版-只檢查單一人
        }
        else // 沒選請假時間
        {
            // do nothing
        }
    }

    [WebMethod]
    public static string checkVal(string USER_GUID, string START_TIME, string END_TIME, string LEACODE, string APPLICANTDATE, string REMARK, string SP_DATE, string SP_NAME, string SCRIPTID, string DOC_NBR, string NEEDATTACH, string SEXAPPLY)
    {
        ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
        JObject joMessage = new JObject();
        string connectionstring = new DatabaseHelper().Command.Connection.ConnectionString;
        bool isNoError = true;
        string SEX = "M";
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.checkVal.USER_GUID: {0}, START_TIME: {1}, END_TIME: {2}", USER_GUID, START_TIME, END_TIME));
        DateTime dtSTART_TIME = DateTime.MinValue;
        DateTime.TryParse(START_TIME, out dtSTART_TIME);
        DateTime dtEND_TIME = DateTime.MinValue;
        DateTime.TryParse(END_TIME, out dtEND_TIME);
        if (dtEND_TIME > dtSTART_TIME) // 迄止時間大於開始時間時
        {
            if (dtSTART_TIME > DateTime.MinValue &&
            dtEND_TIME > DateTime.MinValue)
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(@"
                -- 檢查請假時間是否有重複
                SELECT DOC_NBR
                  FROM Z_SCSHR_LEAVE
                  WHERE APPLICANTGUID = @USER_GUID
	                AND ((STARTTIME >= @STARTTIME AND STARTTIME < @ENDTIME)
                         OR (ENDTIME > @STARTTIME AND ENDTIME <= @ENDTIME)
                         OR (STARTTIME <= @STARTTIME AND ENDTIME >= @ENDTIME))
                    AND (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)
                    AND ((TASK_STATUS = 1 AND TASK_RESULT = 0)
                        OR (TASK_STATUS = 2 AND TASK_RESULT = 0)
                        OR (TASK_STATUS = 1 AND TASK_RESULT IS NULL))
                
                -- 查性別
                SELECT SEX FROM TB_EB_EMPL WHERE USER_GUID = @USER_GUID

                -- 查附件
                IF (@DOC_NBR = '') -- 草稿起單
	                BEGIN
		                IF EXISTS (SELECT * 
					                 FROM TB_WKF_SCRIPT 
				                    WHERE SCRIPT_ID = @SCRIPT_ID 
					                  AND (PERSONAL_ATTACH_ID IS NOT NULL 
					                      OR PERSONAL_ATTACH_ID != ''))
			                BEGIN
				                IF EXISTS (SELECT * -- 檔案有沒有被刪掉
							                 FROM TB_EB_FILE_STORE 
						                    WHERE FILE_GROUP_ID = (SELECT PERSONAL_ATTACH_ID
													                 FROM TB_WKF_SCRIPT 
													                WHERE SCRIPT_ID = @SCRIPT_ID)
							                  AND DELETED <> 1)
					                BEGIN
						                SELECT 0 -- 草稿起單時有上傳個人附件，通過
					                  END
				                ELSE
				                    BEGIN
						                SELECT 1 -- 草稿起單時沒有上傳個人附件，沒通過
					                  END
			                  END
		                 ELSE -- 草稿起單時沒有上傳個人附件
			                BEGIN
				                SELECT 2 -- 草稿起單時沒有上傳個人附件，沒通過
			                  END
	                  END
                ELSE -- 不是草稿起單
	                BEGIN
		                IF EXISTS(SELECT ATTACH_ID -- 是退回申請者或取回
	                                FROM TB_WKF_TASK_NODE 
	                               WHERE TASK_ID = (SELECT TASK_ID 
									                  FROM TB_WKF_TASK 
									                 WHERE DOC_NBR = @DOC_NBR)
					                 AND NODE_STATUS = 4)
	                          BEGIN
					                IF EXISTS(SELECT ATTACH_ID -- 是退回申請者或取回有沒有上傳過附件
							                    FROM TB_WKF_TASK_NODE 
						                       WHERE TASK_ID = (SELECT TASK_ID 
												                  FROM TB_WKF_TASK 
												                 WHERE DOC_NBR = @DOC_NBR)
								                 AND NODE_STATUS = 4
								                 AND (ATTACH_ID != '' 
									                 OR ATTACH_ID IS NOT NULL))
						                BEGIN
							                IF EXISTS(SELECT * -- 檔案有沒有被刪掉
										                     FROM TB_EB_FILE_STORE 
										                    WHERE FILE_GROUP_ID = (SELECT ATTACH_ID 
										  	  					                     FROM TB_WKF_TASK_NODE 
										  						                    WHERE TASK_ID = (SELECT TASK_ID 
										  											                   FROM TB_WKF_TASK 
										  											                  WHERE DOC_NBR = @DOC_NBR)
										  							                  AND NODE_STATUS = 4
										  							                  AND (ATTACH_ID != '' 
										  								                  OR ATTACH_ID IS NOT NULL))
									                          AND DELETED <> 1)
									                BEGIN
										                SELECT 0 -- 取回或退回申請者起單時有上傳個人附件，通過
									                END
								                ELSE
									                BEGIN
										                SELECT 4 -- 取回或退回申請者起單時沒有上傳個人附件，不通過
									                END	 
						                END
					                ELSE
						                BEGIN
							                SELECT 5 -- 是退回申請者或取回有未曾上傳過附件
						                END
				                END
		                ELSE
		                    BEGIN
		   		                SELECT 0 -- 不是取回或退回申請者起單，不檢查
		                     END
	                END
                ", connectionstring))
                using (DataSet ds = new DataSet())
                {
                    sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", USER_GUID);
                    sda.SelectCommand.Parameters.AddWithValue("@STARTTIME", dtSTART_TIME);
                    sda.SelectCommand.Parameters.AddWithValue("@ENDTIME", dtEND_TIME);
                    sda.SelectCommand.Parameters.AddWithValue("@SCRIPT_ID", SCRIPTID);
                    sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
                    try
                    {
                        sda.Fill(ds);

                        #region 直接送給飛騰檢查是否有重複

                        SCSServicesProxy service = ConstructorCommonSettings.setSCSSServiceProxDefault();
                        DataTable dtResult = null;
                        JArray jaTable = new JArray();
                        JObject _joTable = new JObject();
                        _joTable.Add(new JProperty("USERNO", "1"));
                        _joTable.Add(new JProperty("SYS_VIEWID", ""));
                        _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(APPLICANTDATE).ToString("yyyyMMdd")));
                        //_joTable.Add(new JProperty("TMP_EMPLOYEEID", new UserUCO().GetEBUser(USER_GUID).Account));
                        //_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(AGENT_GUID) ? JGlobalLibs.UOFUtils.getUserAccount(AGENT_GUID) : "admin"));
                        KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(USER_GUID);
                        string AGENTID = GetFirstAgent(KUser.UserGUID);
                        _joTable.Add(new JProperty("TMP_EMPLOYEEID", KUser.Account));
                        _joTable.Add(new JProperty("STARTDATE", dtSTART_TIME.ToString("yyyyMMdd")));
                        _joTable.Add(new JProperty("STARTTIME", dtSTART_TIME.ToString("HHmm")));
                        _joTable.Add(new JProperty("TMP_SVACATIONID", LEACODE));
                        _joTable.Add(new JProperty("ENDDATE", dtEND_TIME.ToString("yyyyMMdd")));
                        _joTable.Add(new JProperty("ENDTIME", dtEND_TIME.ToString("HHmm")));
                        _joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(AGENTID) ? new KYT_UserPO().GetUserDetailByUserGuid(AGENTID.Split(',')[0]).EmployeeNo : KUser.EmployeeNo));
                        _joTable.Add(new JProperty("NOTE", REMARK.Trim()));
                        if (!string.IsNullOrEmpty(SP_NAME))
                        {
                            //_joTable.Add(new JProperty("SPECIALDATE", kddlSP_DATE.SelectedValue));
                            _joTable.Add(new JProperty("SPECIALDATE", SP_DATE));
                            _joTable.Add(new JProperty("STARGETNAME", SP_NAME));
                        }
                        jaTable.Add(_joTable);
                        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.checkVal.JATABLE:{0}", jaTable));
                        DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                        dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
                        DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
                        dsSource.Tables.Add(dtSource);
                        Exception ex = null;
                        dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
                        if (ex != null)
                            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.checkVal.BOImport.ERROR:{0}", ex.Message));
                        if (dtResult != null &&
                            dtResult.Rows.Count > 0)
                        {
                            if (dtResult.Rows[0]["_STATUS"].ToString() == "0")
                                joMessage.Add(new JProperty("wsrepeat", dtResult.Rows[0]["_MESSAGE"].ToString()));
                        }

                        #endregion 直接送給飛騰檢查是否有重複
                        //if (ds.Tables[0].Rows.Count > 0) // 有重複申請的請假時間
                        //{
                        //    //joMessage.Add(new JProperty("docrepeat", string.Format(@"<br />[請假時間]重複 (單號：{0} 日期：{1} ~ {2})",
                        //    //(string)ds.Tables[0].Rows[0]["DOC_NBR"], dtSTART_TIME.ToString(), dtEND_TIME.ToString())));
                        //}
                        //else if (ds.Tables[0].Rows.Count == 0) // 中繼表找不到
                        //{

                        //}
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            SEX = ds.Tables[1].Rows[0]["SEX"].ToString();
                        }
                        if (NEEDATTACH.ToUpper() == "TRUE")
                        {
                            if ((int)ds.Tables[2].Rows[0][0] != 0) // 沒有上傳附件
                            {
                                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.checkVal.檢查上傳附件結果: {0}", (int)ds.Tables[2].Rows[0][0]));
                                joMessage.Add(new JProperty("nofile", "需在個人附件上傳請假證明\n"));
                            }
                        }
                        // 需要阻擋請假時間起訖區間
                        if (SCSHRConfiguration.NEED_BLOCK_LEV_LIMIT_TIME.ToUpper() == "Y")
                        {
                            int preDays = 0;
                            int nextDays = 0;
                            int.TryParse(SCSHRConfiguration.LEV_PREVIOUS_DAYS, out preDays);
                            int.TryParse(SCSHRConfiguration.LEV_NEXT_DAYS, out nextDays);
                            DateTime dtStartLimit = DateTime.Parse(string.Format("{0} 00:00:00", DateTime.Now.AddDays(-preDays).ToString("yyyy/MM/dd")));
                            DateTime dtOffLimit = DateTime.Parse(string.Format("{0} 23:59:59", DateTime.Now.AddDays(nextDays).ToString("yyyy/MM/dd")));
                            if (preDays > 0)
                            {
                                if (dtEND_TIME < dtStartLimit)
                                {
                                    joMessage.Add(new JProperty("START_LIMIT", string.Format(@"已超過請假需 {0} 日內送單申請", preDays)));
                                }
                            }
                            if (nextDays > 0)
                            {
                                if (dtEND_TIME > dtOffLimit)
                                {
                                    joMessage.Add(new JProperty("OFF_LIMIT", string.Format(@"已超過請假需 {0} 日前送單申請", nextDays)));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE checkVal Error: {0}", e.Message));
                        isNoError = false;
                    }
                }
            }
            else
            {
                isNoError = false;
            }
        }
        else // 迄止時間小於開始時間
        {
            joMessage.Add(new JProperty("STARTTIME_BIGGER", "迄止時間小於開始時間"));
        }

        if (!isNoError)
        {
            joMessage.Add(new JProperty("isError", "<br />發生錯誤"));
        }
        if (SEX == "M" && SEXAPPLY == "2") // 如果是男性，假別卻要求女性
        {
            joMessage.Add(new JProperty("SEXAPPLY", "男性不能申請"));
        }
        else if (SEX == "F" && SEXAPPLY == "1") // 如果是女性，假別卻要求男性
        {
            joMessage.Add(new JProperty("SEXAPPLY", "女性不能申請"));
        }
        //joMessage.Add(new JProperty("LEA_SEX", SEX));
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.checkVal.Result:{0}", joMessage.ToString()));

        return joMessage.ToString();
    }


    [WebMethod]
    public static string CheckSignLevel(string DOC_NBR)
    {
        ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
        string cstring = new DatabaseHelper().Command.Connection.ConnectionString;
        JObject joMessage = new JObject();

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT * FROM Z_SCSHR_LEAVE WHERE DOC_NBR = @DOC_NBR
            ", cstring))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    DateTime dtSTART_TIME = (DateTime)dr["STARTTIME"];
                    DateTime dtEND_TIME = (DateTime)dr["ENDTIME"];
                    SCSServicesProxy service = ConstructorCommonSettings.setSCSSServiceProxDefault();
                    DataTable dtResult = null;
                    JArray jaTable = new JArray();
                    JObject _joTable = new JObject();
                    _joTable.Add(new JProperty("USERNO", "1"));
                    _joTable.Add(new JProperty("SYS_VIEWID", DOC_NBR));
                    _joTable.Add(new JProperty("SYS_DATE", ((DateTime)dr["APPLICANTDATE"]).ToString("yyyyMMdd")));
                    //_joTable.Add(new JProperty("TMP_EMPLOYEEID", JGlobalLibs.UOFUtils.getUserAccount(dr["LEAEMP"].ToString())));
                    //_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(dr["LEAAGENT"].ToString()) ? JGlobalLibs.UOFUtils.getUserAccount(dr["LEAAGENT"].ToString()) : "admin"));
                    //_joTable.Add(new JProperty("TMP_EMPLOYEEID", ACCOUT)); // 取最右邊
                    KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(dr["LEAEMP"].ToString());
                    string AGENTID = GetFirstAgent(KUser.UserGUID);
                    _joTable.Add(new JProperty("TMP_EMPLOYEEID", KUser.EmployeeNo));
                    _joTable.Add(new JProperty("STARTDATE", dtSTART_TIME.ToString("yyyyMMdd")));
                    _joTable.Add(new JProperty("STARTTIME", dtSTART_TIME.ToString("HHmm")));
                    _joTable.Add(new JProperty("TMP_SVACATIONID", dr["LEACODE"].ToString()));
                    _joTable.Add(new JProperty("ENDDATE", dtEND_TIME.ToString("yyyyMMdd")));
                    _joTable.Add(new JProperty("ENDTIME", dtEND_TIME.ToString("HHmm")));
                    _joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(AGENTID) ? new KYT_UserPO().GetUserDetailByUserGuid(AGENTID.Split(',')[0]).EmployeeNo : KUser.EmployeeNo));
                    _joTable.Add(new JProperty("NOTE", dr["REMARK"].ToString().Trim()));
                    if (!string.IsNullOrEmpty(dr["SP_NAME"].ToString()))
                    {
                        //_joTable.Add(new JProperty("SPECIALDATE", kddlSP_DATE.SelectedValue));
                        _joTable.Add(new JProperty("SPECIALDATE", ((DateTime)dr["SP_DATE"]).ToString("yyyy/MM/dd")));
                        _joTable.Add(new JProperty("STARGETNAME", dr["SP_NAME"].ToString()));
                    }
                    jaTable.Add(_joTable);
                    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.CheckSignLevel.JATABLE:{0}", jaTable));
                    DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                    dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
                    DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
                    dsSource.Tables.Add(dtSource);
                    Exception ex = null;
                    //dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
                    if (ex != null)
                    {
                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.CheckSignLevel.BOImport.ERROR:{0}", ex.Message));
                        joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                        goto toEnd;
                    }
                    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.CheckSignLevel.dtResult:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
                    if (dtResult != null &&
                        dtResult.Rows.Count > 0)
                    {
                        if (dtResult.Rows[0]["_STATUS"].ToString() != "0")
                            joMessage.Add(new JProperty("CheckWSError", dtResult.Rows[0]["_MESSAGE"].ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.CheckSignLevel.SELECT.Z_SCSHR_LEAVE.ERROR: {0}", e.Message));
                joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                goto toEnd;
            }
        }
    toEnd:
        if (joMessage.Count == 0)
            joMessage.Add(new JProperty("checkMsg", "success"));
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_BATLEAVE.CheckSignLevel.Result:{0}", joMessage.ToString()));
        return joMessage.ToString();
    }

    protected void kdtpSTARTTIME_TextChanged(object sender, EventArgs e)
    {
        KYTDateTimePicker _kdtpSTARTTIME = (KYTDateTimePicker)sender;
        //kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);
        if (SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE.ToString().ToUpper() == "Y")
        {
            DateTime dtPreSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(ViewState["STARTTIME"] != null ? ViewState["STARTTIME"].ToString() : "", out dtPreSTARTTIME); // 如果沒有紀錄上次的時間，就記錄成時間最小值
            DateTime dtSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(_kdtpSTARTTIME.Text, out dtSTARTTIME);
            if (dtPreSTARTTIME.Date != dtSTARTTIME.Date) // 如果上次的日期不等於現在的日期，更動迄止時間
            {
                DateTime dtENDTIME = DateTime.MinValue;
                DateTime.TryParse(kdtpENDTIME.Text, out dtENDTIME);
                kdtpENDTIME.Text = string.Format(@"{0} {1}", dtSTARTTIME.ToString("yyyy/MM/dd"), dtENDTIME.ToString("HH:mm")); // 只更動日期
                ViewState["STARTTIME"] = _kdtpSTARTTIME.Text; // 紀錄這次的時間
            }
        }
    }

    protected void kdtpENDTIME_TextChanged(object sender, EventArgs e)
    {
        KYTDateTimePicker _kdtpENDTIME = (KYTDateTimePicker)sender;
        //kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);

    }

    protected void kddlSP_DATE_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTDropDownList _kddlSP_DATE = (KYTDropDownList)sender;
        DataTable dtSource = _kddlSP_DATE.DataSource as DataTable;
        foreach (DataRow dr in dtSource.Rows)
        {
            if (dr["SPECIALDATE"].ToString() == _kddlSP_DATE.SelectedValue)
            {
                ktxtSP_NAME.Text = dr["STARGETNAME"].ToString();
            }
        }
    }

    protected void btnLEAAGENT_DialogReturn(object sender, string result)
    {
        JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
        if (jArray.Count > 0)
        {
            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(jArray[0]["GUID"].ToString());
            //EBUser user = new UserUCO().GetEBUser(jArray[0]["GUID"].ToString());
            ktxtLEAAGENT.Text = KUser.Name;
            hidTMP_AGENTID_GUID.Value = KUser.UserGUID;
        }
        else
        {
            ktxtLEAAGENT.Text = "";
            hidTMP_AGENTID_GUID.Value = "";
        }
    }

    /// <summary>
    /// 是否為起單或者退回申請者
    /// </summary>
    /// <returns></returns>
    private bool IsModify()
    {
        return this.FormFieldMode == FieldMode.Applicant || this.FormFieldMode == FieldMode.ReturnApplicant;
    }

    /// <summary>
    /// 重新排序Gridview的項目編號
    /// </summary>
    private void ResetGridViewITEMNO(DataTable dt, string ITEM_NO)
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i][ITEM_NO] = (i * 1 + 1).ToString().PadLeft(2, '0');
        }
    }

    /// <summary>
    /// 建立申請明細
    /// </summary>
    /// <returns></returns>
    private DataTable CreatePT_gvItemsD1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ITEM_NO", typeof(String))); // 項次
        dt.Columns.Add(new DataColumn("SYS_VIEWID", typeof(String))); // 編號
        dt.Columns.Add(new DataColumn("APPLICANTDEPT", typeof(String))); // 部門
        dt.Columns.Add(new DataColumn("SYS_DATE", typeof(String))); // 申請日期
        dt.Columns.Add(new DataColumn("TMP_EMPLOYEEID", typeof(String))); // 請假人
        dt.Columns.Add(new DataColumn("TMP_EMPLOYEEID_GUID", typeof(String))); // 請假人GUID
        dt.Columns.Add(new DataColumn("TMP_AGENTID", typeof(String))); // 代理人
        dt.Columns.Add(new DataColumn("TMP_AGENTID_GUID", typeof(String))); // 代理人GUID
        dt.Columns.Add(new DataColumn("TMP_SVACATIONID", typeof(String))); // 假別
        dt.Columns.Add(new DataColumn("SEXAPPLY", typeof(String))); // 假別性別
        dt.Columns.Add(new DataColumn("NEED_ATTACH", typeof(String))); // 特別假別
        dt.Columns.Add(new DataColumn("TMP_SVACATIONID_TXT", typeof(String))); // 假別名稱
        dt.Columns.Add(new DataColumn("IS_AGENTID", typeof(bool))); // 啟動代理
        dt.Columns.Add(new DataColumn("SPECIALDATE", typeof(String))); // 特殊日期
        dt.Columns.Add(new DataColumn("STARGETNAME", typeof(String))); // 特殊日對象
        dt.Columns.Add(new DataColumn("START", typeof(String))); // 請假時間(起)
        dt.Columns.Add(new DataColumn("STARTDATE", typeof(String))); // 請假時間(起)
        dt.Columns.Add(new DataColumn("STARTTIME", typeof(String))); // 請假時間(起)
        dt.Columns.Add(new DataColumn("END", typeof(String))); // 請假時間(迄)
        dt.Columns.Add(new DataColumn("ENDDATE", typeof(String))); // 請假時間(迄)
        dt.Columns.Add(new DataColumn("ENDTIME", typeof(String))); // 請假時間(迄)
        dt.Columns.Add(new DataColumn("LEADAYS", typeof(decimal))); // 請假天數
        dt.Columns.Add(new DataColumn("LEAHOURS", typeof(decimal))); // 請假時數
        dt.Columns.Add(new DataColumn("NOTE", typeof(String))); // 請假說明
        return dt;
    }

    /// <summary>
    /// 建立新的申請明細-D1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNewGvItemD1_Click(object sender, EventArgs e)
    {
        if (hidAPIResult.Value == "OK")
        {
            DataTable tblgvItems_D1 = gvItemsD1.DataTable;
            DateTime start = (DateTime)kdtpSTARTTIME.SelectedDate;
            DateTime end = (DateTime)kdtpENDTIME.SelectedDate;
            DataRow ndr = tblgvItems_D1.NewRow();
            string agent = GetFirstAgent(hidLEAEMP.Value);
            ndr["ITEM_NO"] = "1";
            ndr["SYS_VIEWID"] = ""; // 編號
            ndr["APPLICANTDEPT"] = ktxtAPPLICANTDEPT.Text; // 部門*
            ndr["SYS_DATE"] = ktxtAPPLICANTDATE.Text; // 申請日期
            ndr["TMP_EMPLOYEEID"] = ktxtLEAEMP.Text; // 請假人*
            ndr["TMP_EMPLOYEEID_GUID"] = hidLEAEMP.Value; // 請假人GUID*
            ndr["TMP_AGENTID"] = !string.IsNullOrEmpty(agent) ? agent.Split(',')[1] : ktxtLEAEMP.Text; // 代理人
            ndr["TMP_AGENTID_GUID"] = !string.IsNullOrEmpty(agent) ? agent.Split(',')[0] : hidLEAEMP.Value; // 代理人GUID
            ndr["TMP_SVACATIONID"] = kddlLEACODE.SelectedValue; // 假別
            ndr["NEED_ATTACH"] = hidNeedAttach.Value; // 特別假別
            ndr["SEXAPPLY"] = hidSEXAPPLY.Value; // 假別性別
            ndr["TMP_SVACATIONID_TXT"] = kddlLEACODE.SelectedItem.Text; // 假別
            ndr["IS_AGENTID"] = kchkAGENT.Checked; // 啟動代理
            ndr["SPECIALDATE"] = kdpSP_DATE.Text; // 特殊日期
            ndr["START"] = start; // 請假時間(起)
            ndr["STARTDATE"] = start.ToString("yyyyMMdd"); // 請假時間(起)
            ndr["STARTTIME"] = start.ToString("HHmm"); // 請假時間(起)
            ndr["END"] = end; // 請假時間(迄)
            ndr["ENDDATE"] = end.ToString("yyyyMMdd"); // 請假時間(迄)
            ndr["ENDTIME"] = end.ToString("HHmm"); // 請假時間(迄)
            ndr["LEADAYS"] = ktxtLEADAYS.Text; // 請假天數
            ndr["LEAHOURS"] = ktxtLEAHOURS.Text; // 請假時數
            ndr["NOTE"] = ktxtREMARK.Text; // 請假說明
            tblgvItems_D1.Rows.Add(ndr);
            tblgvItems_D1.AcceptChanges();
            this.ResetGridViewITEMNO(tblgvItems_D1, "ITEM_NO"); //重新排序項目編號
            gvItemsD1.DataSource = tblgvItems_D1;
            gvItemsD1.DataBind();
        }
    }

    /// <summary>
    /// gvItemD1_刪除按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteD1_Click(object sender, EventArgs e)
    {
        ImageButton imgDelPMCostOne = (ImageButton)sender;
        GridViewRow gr = imgDelPMCostOne.NamingContainer as GridViewRow;
        DataTable tblDataTable = (DataTable)gvItemsD1.DataSource;
        tblDataTable.Rows[gr.RowIndex].Delete();
        tblDataTable.AcceptChanges();
        ResetGridViewITEMNO(tblDataTable, "ITEM_NO");
        gvItemsD1.DataSource = tblDataTable;
        gvItemsD1.DataBind();
    }

    /// <summary>
    /// 申請明細
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvItemsD1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < gvItemsD1.Columns.Count; i++)
            {
                TemplateField _column = gvItemsD1.Columns[i] as TemplateField;
                switch (_column.HeaderText) // 取得標題
                {
                    case "項次":
                    case "編號":
                        gr.Cells[i].Attributes.Add("style", "display:none;");
                        break;
                }
            }
            DataTable tblgvItemsD1 = gvItemsD1.DataTable; //取出先前記住的 gvItems
            DataRow row = tblgvItemsD1.Rows[gr.DataItemIndex]; //取出對應的資料列

            ImageButton btnDeleteD1 = gr.FindControl("btnDeleteD1") as ImageButton; // 刪除
            KYTTextBox ktxtITEM_NO = gr.FindControl("ktxtITEM_NO") as KYTTextBox; // 項次
            KYTTextBox ktxtSYS_VIEWID = gr.FindControl("ktxtSYS_VIEWID") as KYTTextBox; // 編號
            KYTTextBox ktxtAPPLICANTDEPT = gr.FindControl("ktxtAPPLICANTDEPT") as KYTTextBox; // 部門
            KYTTextBox ktxtSYS_DATE = gr.FindControl("ktxtSYS_DATE") as KYTTextBox; // 申請日期
            KYTTextBox ktxtTMP_EMPLOYEEID = gr.FindControl("ktxtTMP_EMPLOYEEID") as KYTTextBox; // 請假人
            KYTTextBox ktxtTMP_AGENTID = gr.FindControl("ktxtTMP_AGENTID") as KYTTextBox; // 代理人
            KYTTextBox ktxtTMP_SVACATIONID_TXT = gr.FindControl("ktxtTMP_SVACATIONID_TXT") as KYTTextBox; // 假別
            KYTCheckBox kchkAGENT = gr.FindControl("kchkAGENT") as KYTCheckBox; // 啟動代理
            KYTTextBox ktxtSPECIALDATE = gr.FindControl("ktxtSPECIALDATE") as KYTTextBox; // 特殊日期
            KYTTextBox ktxtSTARGETNAME = gr.FindControl("ktxtSTARGETNAME") as KYTTextBox; // 特殊日對象
            KYTTextBox ktxtSTART = gr.FindControl("ktxtSTART") as KYTTextBox; // 請假時間(起)
            KYTTextBox ktxtEND = gr.FindControl("ktxtEND") as KYTTextBox; // 請假時間(迄)
            KYTTextBox ktxtLEADAYS = gr.FindControl("ktxtLEADAYS") as KYTTextBox; // 請假天數
            KYTTextBox ktxtLEAHOURS = gr.FindControl("ktxtLEAHOURS") as KYTTextBox; // 請假時數
            KYTTextBox ktxtNOTE = gr.FindControl("ktxtNOTE") as KYTTextBox; // 請假說明

            btnDeleteD1.Visible = false; // 刪除按鈕隱藏
            ktxtITEM_NO.ViewType = KYTViewType.ReadOnly; // 項次
            ktxtSYS_VIEWID.ViewType = KYTViewType.ReadOnly; // 編號
            ktxtAPPLICANTDEPT.ViewType = KYTViewType.ReadOnly; // 部門
            ktxtSYS_DATE.ViewType = KYTViewType.ReadOnly; // 申請日期
            ktxtTMP_EMPLOYEEID.ViewType = KYTViewType.ReadOnly; // 請假人
            ktxtTMP_AGENTID.ViewType = KYTViewType.ReadOnly; // 代理人
            ktxtTMP_SVACATIONID_TXT.ViewType = KYTViewType.ReadOnly; // 假別
            kchkAGENT.ViewType = KYTViewType.Input; // 啟動代理
            kchkAGENT.CheckBox1.Enabled = false;
            ktxtSPECIALDATE.ViewType = KYTViewType.ReadOnly; // 特殊日期
            ktxtSTARGETNAME.ViewType = KYTViewType.ReadOnly; // 特殊日對象
            ktxtSTART.ViewType = KYTViewType.ReadOnly; // 請假時間(起)
            ktxtEND.ViewType = KYTViewType.ReadOnly; // 請假時間(迄)
            ktxtLEADAYS.ViewType = KYTViewType.ReadOnly; // 請假天數
            ktxtLEAHOURS.ViewType = KYTViewType.ReadOnly; // 請假時數
            ktxtNOTE.ViewType = KYTViewType.ReadOnly; // 請假說明
            kchkAGENT.Checked = (bool)row["IS_AGENTID"];
            if (this.IsModify())
            {
                btnDeleteD1.Visible = true; // 刪除按鈕顯示

            }
            else
            {
                btnDeleteD1.Visible = false; // 刪除按鈕隱藏

            }
        }
        else if (gr.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < gvItemsD1.Columns.Count; i++)
            {
                TemplateField _column = gvItemsD1.Columns[i] as TemplateField;
                switch (_column.HeaderText) // 取得標題
                {
                    case "項次":
                    case "編號":
                        gr.Cells[i].Attributes.Add("style", "display:none;");
                        break;
                }
            }
        }


    }

    /// <summary>
    /// 申請人_選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    protected void btnLEAEMP_DialogReturn(object sender, string result)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ACCOUT", typeof(string))); // 請假人帳號
        dt.Columns.Add(new DataColumn("GUID", typeof(string))); // 請假人GUID
        dt.Columns.Add(new DataColumn("NAME", typeof(string))); // 請假人姓名
        dt.Columns.Add(new DataColumn("GROUP_CODE", typeof(string))); // 請假人部門CODE
        dt.Columns.Add(new DataColumn("GROUP_NAME", typeof(string))); // 請假人部門名稱
        dt.Columns.Add(new DataColumn("GROUP_ID", typeof(string))); // 請假人GROUP_ID
        dt.Columns.Add(new DataColumn("LEADAYS", typeof(decimal))); // 請假天數
        dt.Columns.Add(new DataColumn("LEAHOURS", typeof(decimal))); // 請假時設

        JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
        if (jArray.Count > 0)
        {
            //EBUser userFirst = new UserUCO().GetEBUser(jArray[0]["GUID"].ToString());
            //ktxtLEAEMP.Text = string.Format(@"{0} ({1})", userFirst.Name, userFirst.Account);  // 設定請假人資訊
            //hidLEAEMP.Value = userFirst.UserGUID;
            //ktxtAPPLICANTDEPT.Text = userFirst.GroupName; // 設定部門資訊
            //hidAPPLICANTDEPT.Value = userFirst.GroupID;
            //hidLEAEMPTitleId.Value = userFirst.JobTitleID;
            //hidLEAEMPTitleName.Value = userFirst.JobTitle;
            //hidLEAEMPAccount.Value = userFirst.Account;
            //string[] sAccount_First = hidLEAEMPAccount.Value.Split('\\');
            //hidLEAEMPAccount.Value = sAccount_First[sAccount_First.Length - 1];
            //hidGROUPCODE.Value = JGlobalLibs.UOFUtils.GetGroupCodeByDepartmentID(userFirst.GroupID);
            //btnLEAAGENT.GroupID = userFirst.GroupID; // 代理人跳窗，設定第一位請假人
            //btnLEAAGENT.isSearchSelfGroupUsers = true;

            string LEAEMP_ALL = "";
            // 選擇多人請假人
            foreach (JToken Jk in jArray)
            {
                KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(Jk["GUID"].ToString());
                //EBUser user = new UserUCO().GetEBUser(Jk["GUID"].ToString());
                string[] sAccount = KUser.Account.Split('\\');

                DataRow ndr = dt.NewRow();
                ndr["ACCOUT"] = sAccount[sAccount.Length - 1];
                ndr["GUID"] = KUser.UserGUID;
                ndr["NAME"] = KUser.Name;
                ndr["GROUP_CODE"] = KUser.GroupCode[0];
                ndr["GROUP_NAME"] = KUser.GroupName[0];
                ndr["GROUP_ID"] = KUser.GroupID[0];
                dt.Rows.Add(ndr);
                LEAEMP_ALL += string.Format(@"{0} ({1}){2}", KUser.Name, sAccount[sAccount.Length - 1], Jk == jArray.Last ? "" : ",");  // 設定請假人資訊


                ktxtLEAEMP.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account);  // 設定請假人資訊
                hidLEAEMP.Value = KUser.UserGUID;
                ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 設定部門資訊
                hidAPPLICANTDEPT.Value = KUser.GroupID[0];
                hidLEAEMPTitleId.Value = KUser.Title_ID[0];
                hidLEAEMPTitleName.Value = KUser.Title_Name[0];
                hidLEAEMPAccount.Value = KUser.Account;
                string[] sAccount_First = hidLEAEMPAccount.Value.Split('\\');
                hidLEAEMPAccount.Value = sAccount_First[sAccount_First.Length - 1];
                hidGROUPCODE.Value = KUser.GroupCode[0];
                btnLEAAGENT.GroupID = KUser.GroupID[0]; // 代理人跳窗，設定第一位請假人
                btnLEAAGENT.isSearchSelfGroupUsers = true;
            }
            ktxtLEAEMP.Text = LEAEMP_ALL;
            ViewState["LEAEMP"] = dt;
        }
        else
        {
            ktxtLEAEMP.Text = ""; // 設定請假人資訊
            hidLEAEMP.Value = "";
            ktxtAPPLICANTDEPT.Text = ""; // 設定部門資訊
            hidAPPLICANTDEPT.Value = "";
            hidLEAEMPTitleId.Value = "";
            hidLEAEMPTitleName.Value = "";
            hidLEAEMPAccount.Value = "";
            hidLEAEMPAccount.Value = "";
            hidGROUPCODE.Value = "";
        }
    }

    /// <summary>
    /// 取得UOF第一順位代理人
    /// </summary>
    /// <param name="UserGuid"></param>
    /// <returns></returns>
    public static string GetFirstAgent(string UserGuid)
    {
        string ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        string Agent = "";
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
          SELECT [USER_GUID]
                ,[AGENT_USER]
                ,[ORDERS]
	            ,ISNULL((SELECT NAME FROM TB_EB_USER WHERE TB_EB_USER_AGENT.AGENT_USER = TB_EB_USER.USER_GUID),'') AS NAME
            FROM [TB_EB_USER_AGENT]
           WHERE USER_GUID = @USER_GUID
        ORDER BY ORDERS ASC
        ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", UserGuid); // 請假人GUID
            try
            {
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Agent = string.Format("{0},{1}", dr["AGENT_USER"].ToString(), dr["NAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_BATLEAVE::GetFirstAgent()::取得UOF第一順位代理人::失敗:{0}", ex.Message));
            }
        }
        return Agent;

    }

}
