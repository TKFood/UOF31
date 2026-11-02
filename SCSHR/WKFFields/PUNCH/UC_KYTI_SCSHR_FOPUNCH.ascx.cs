using Ede.Uof.EIP.Organization;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.WKF.Design;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using UOFAssist.WKF;


/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
    * 2.呼叫飛騰，傳入參數-TMP_EMPLOYEEID，原給「ACCOUNT」 -> 改給「EMPLOYEEID」
* 修改原因：
    * 1.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
    * 2.規格修正，飛騰目前都改為固定傳入「EMPLOYEEID」
* 修改位置： 
    * 1.「SetField()」中，註解所有EBUser，改為KYT_EBUser
    * 2.「btnCal_Click()」，傳入TMP_EMPLOYEEID，的值 -> 改傳入EmployeeNo
* **/

/**
* 修改時間：2020/09/30
* 修改人員：陳緯榕
* 修改項目：
    * 補卡狀態由config控制
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈kddlFOPUNCHTYPE〉、〈kddlFOPUNCHTYPE_OFF〉的選項只留下〈====請選擇====〉
    * 「SetField」中，巡覽〈SCSHRConfiguration.FOPUNCH_TYPE〉，並且用〈,〉區分出上班補卡狀態，填入〈kddlFOPUNCHTYPE〉
    * 「SetField」中，巡覽〈SCSHRConfiguration.FOPUNCH_OFF_TYPE〉，並且用〈,〉區分出下班補卡狀態填入〈kddlFOPUNCHTYPE_OFF〉
* **/

/**
* 修改時間：2020/04/16
* 修改人員：陳緯榕
* 修改項目：
    * 新規格：標題修改
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈SCSHR補刷申請單〉改為〈補刷申請資訊〉
* **/

/**
* 修改時間：2020/03/13
* 修改人員：陳緯榕
* 修改項目：
    * ktxtSignResult顯示需為紅色
* 發生原因：
    * ，紅色是屬於警告意味強的顏色，故使用紅色顯示錯誤
* 修改位置：
    * 「前端網頁」中，新增〈CSS：msgColor〉；並將〈ktxtSignResult〉的屬性〈LabelCssClass〉設為〈msgColor〉
* **/

/**
* 修改時間：2019/12/30
* 修改人員：陳緯榕
* 修改項目：
    * 下班補卡時間沒填寫卻也能通過
* 發生原因：
    * 前端呼叫CheckVal取到的回傳值「NOWORK_OFF」後面多了空格，所以被認定為其他字串
* 修改位置：
    * 「前端網頁」「checkVal(sender, args)」中，〈NOWORK_OFF〉後面多了空格，要去除
* **/

/**
* 修改時間：2019/12/06
* 修改人員：陳緯榕
* 修改項目：
    * 當特定關卡時，開放原因可編輯
* 發生原因：
    * 新需求
* 修改位置：
    * 「SetField」中，當〈簽核〉時，判斷〈SiteCode〉為〈HR〉時，對〈kddlFOPUNCHTYPE〉補上班卡狀態、〈kddlFOPUNCHTYPE_OFF〉補下班卡狀態的屬性〈ViewType〉設為〈KYTViewType.Input〉
* **/

/**
* 修改時間：2019/11/27
* 修改人員：JAY
* 修改項目：
    * 飛騰那邊會維護一段文字，但目前api取不到
* 發生原因：
    * 新增需求
* 修改位置：
    * 「前端網頁」中，新增物件〈KYTTextBox：ktxtPUNCHMEMO〉，之後還需要從API取得資料
* **/

/**
* 修改時間：2019/07/17
* 修改人員：陳緯榕
* 修改項目：
    * KYTDateTimePicker在dll.Config設定的IS_PICKER_READONLY設定無效
* 發生原因：
    * KYTPicker要設定TextBoxReadOnly的特性是ViewType.Input，但原本的位置狀態會是ViewType.ReadOnly
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack(開啟表單)〉而且〈fieldOptional.FieldMode〉是〈Applicant〉或〈ReturnApplicant〉時，程式片段的最後設定〈TextBoxReadOnly〉屬性
    * 「SetField」中，當〈Page.IsPostBack(操作表單)〉而且〈fieldOptional.FieldMode〉是〈Applicant〉或〈ReturnApplicant〉時，設定〈ViewType = Input〉和〈TextBoxReadOnly〉屬性
* **/

/**
* 修改時間：2019/05/08
* 修改人員：陳緯榕
* 修改項目：
    * KYTPicker在Postback後無法再手動輸入
* 修改位置：
    * 「SetField」中，將KYTPicker的〈TextBoxReadOnly〉放到〈網頁首次載入〉之外，讓其每次都執行
* **/

/**
* 修改時間：2019/04/15
* 修改人員：陳緯榕
* 修改項目：
    * KYTDropDownList在ReadOnly狀態下，如果是請選擇就不顯示
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack〉時，原本在簽核、觀看時做的動作通通拿掉，改由UOFAssits中做
* **/

/**
* 修改時間：2019/04/11
* 修改人員：陳緯榕
* 修改項目：
    * DateTimePicker的輸入框是否能輸入由dll.config控制
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack〉時，〈kdtpFOPUNCH_TIME〉和〈kdtpFOPUNCH_TIME_OFF〉的屬性〈TextBoxReadOnly〉由〈SCSHRConfiguration.IS_PICKER_READONLY〉控制
* **/

/**
* 修改時間：2019/03/26
* 修改人員：陳緯榕
* 修改項目：
    * 退回申請者後，刷卡時間無法取消
* 修改位置：
    * 新增方法「kddlFOPUNCHTYPE_SelectedIndexChanged」，當下拉選單的〈SelectedValue〉為空字串時，清除時間
    * 新增方法「kddlFOPUNCHTYPE_OFF_SelectedIndexChanged」，當下拉選單的〈SelectedValue〉為空字串時，清除時間
    * 
* **/

/**
* 修改時間：2019/02/25
* 修改人員：陳緯榕
* 修改項目：
    * 當簽核呼叫WS失敗時，前端網頁必須顯示「SCSHR未起單成功」
* 修改位置：
    * 「前端網頁」中，新增〈ktxtSignResult〉物件
* **/

/**
* 修改時間：2019/02/11
* 修改人員：陳緯榕
* 修改項目：
    * 人員同步時，沒有管理部門代號(GROUP_CODE)，但是WS需要部門代號
* 修改位置：
    * 「SetField」中，如果〈GROUP_CODE〉為空字串，詢問WS〈HUM0020100〉，該帳號的〈TMP_DEPARTID〉
* **/

/**
* 修改時間：2019/01/29
* 修改人員：陳緯榕
* 修改項目：
    * 只填寫下班，無法檢核
* 修改位置：
    * 「btnCheck_Click」中，最初的判斷應該是〈補卡原因必填或上下班時間擇一〉
* **/

/**
* 修改時間：2019/01/29
* 修改人員：陳緯榕
* 修改項目：
    * 檢查完畢後，需出現檢查成功
* 修改位置：
    * 「btnCheck_Click」中，在最後加上〈lblAPIResultError〉為空時，給予〈檢查成功〉
* **/

/**
* 修改時間：2019/01/24
* 修改人員：陳緯榕
* 修改項目：
    * 帳號包含著網域，造成後續許多問題
* 修改位置：
    * 「SetField」中，將user.Account用〈//〉切割後取最右邊，意圖將網域名稱去除
* **/

/**
* 修改時間：2019/01/08
* 修改人員：陳緯榕
* 修改項目：補刷卡無法成功
* 修改位置：
    * 「btnCheck_Click」WS的表身 SUB中，〈USERNO〉固定為〈1〉
* **/

/**
* 修改時間：2018/12/22
* 修改人員：陳緯榕
* 修改項目：
    * 補卡狀態改為〈補上班卡狀態〉、〈補下班卡狀態〉
* 修改位置：
    * 「前端網頁」新增〈補下班卡狀態〉
    * 「前端網頁」〈checkFOPUNCHTYPE〉新增〈kddlFOPUNCHTYPE_OFF〉，用來判斷是否上下班狀態都沒選擇
    * 「前端網頁」〈checkVal〉新增〈kdtpFOPUNCH_TIME_OFF〉、〈kddlFOPUNCHTYPE〉、〈kddlFOPUNCHTYPE_OFF〉
    * 「checkVal」新增參數〈TIME_OFF〉、〈TYPE_START〉、〈TYPE_OFF〉，參數〈TIME〉變更為〈TIME_START〉
    * 「btnCheck_Click」呼叫WS時，上下班補卡需放在表身SUB中
* **/

/**
* 修改時間：2018/12/03
* 修改人員：陳緯榕
* 修改項目：補卡原因非必填，所以不用標記必填
* 修改位置：
    * 「前端網頁」補卡原因〈*〉註解掉
* **/

/// <summary>
/// 飛騰忘刷資訊
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_FOPUNCH : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
            .Add("dept", hidAPPLICANTDEPT.Value)
            .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_FORPUNCH.ConditionValue.cv:{0}", cv));
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
            return string.Format(@"{0}, 上班補卡時間：{1}, 下班補卡時間：{2}", ktxtAPPLICANT.Text, kdtpFOPUNCH_TIME.Text, kdtpFOPUNCH_TIME_OFF.Text);
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
            return string.Format(@"{0}, 上班補卡時間：{1}, 下班補卡時間：{2}", ktxtAPPLICANT.Text, kdtpFOPUNCH_TIME.Text, kdtpFOPUNCH_TIME_OFF.Text);
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
            return String.Empty;
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

            if (!Page.IsPostBack) // 網頁首次載入
            {
                if (!string.IsNullOrEmpty(fieldOptional.FieldValue))
                    kytController.FieldValue = fieldOptional.FieldValue;

                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀
                btnCheck.Visible = false; // 隱藏檢查

                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值

                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        hidAPIResult.Value = ""; // 清掉先前的旗標
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                        ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                        ktxtAPPLICANTDATE.ReadOnly = true; // 申請日期唯讀
                        ktxtAPPLICANT.ReadOnly = true; // 申請人唯讀
                        btnCheck.Visible = true; // 顯示檢查
                        if (this.FormFieldMode == FieldMode.Applicant) // 剛起單
                        {
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 部門名稱
                            hidAPPLICANTDEPT.Value = KUser.GroupID[0]; // 部門代碼
                            hidAPPLICANTDEPT_GROUPCODE.Value = KUser.GroupCode[0];
                            if (string.IsNullOrEmpty(hidAPPLICANTDEPT_GROUPCODE.Value)) // 如果UOF沒有維護GROUP_CODE，就呼叫WS取得
                            {
                                List<SCSHR.net.azurewebsites.scsservices_beta.FilterItem> lsItems = new List<SCSHR.net.azurewebsites.scsservices_beta.FilterItem>();
                                SCSHR.net.azurewebsites.scsservices_beta.FilterItem item = new SCSHR.net.azurewebsites.scsservices_beta.FilterItem();
                                item.FieldName = "SYS_VIEWID";
                                item.FilterValue = KUser.Account;
                                lsItems.Add(item);
                                //filters.SetValue(item, 0);
                                Exception ex = null;
                                DataTable dtEmployee = service.BOFind("HUM0020100", "*", lsItems.ToArray(), out ex);
                                if (dtEmployee.Rows.Count > 0)
                                {
                                    if (dtEmployee.Columns.Contains("TMP_DEPARTID"))
                                    {
                                        hidAPPLICANTDEPT_GROUPCODE.Value = dtEmployee.Rows[0]["TMP_DEPARTID"].ToString();
                                    }
                                }
                            }
                            ktxtAPPLICANTDATE.Text = DateTime.Now.ToString("yyyy/MM/dd"); // 申請日期
                            ktxtAPPLICANT.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account); // 申請人姓名
                            hidAPPLICANT.Value = KUser.UserGUID; // 申請人代碼
                            hidAPPLICANTAccount.Value = KUser.Account; // 申請人帳號
                            string[] sAccount = hidAPPLICANTAccount.Value.Split('\\');
                            hidAPPLICANTAccount.Value = sAccount[sAccount.Length - 1];
                            hidAPPLICANTGuid.Value = KUser.UserGUID; // 申請人Guid


                            kddlFOPUNCH_REASON.DataSource = getFOPUNCHType(SCSHRConfiguration.SCSSPunchTypeProgID, "SYS_ViewID,SYS_Name,SYS_EngName");
                            kddlFOPUNCH_REASON.DataValueField = "SYS_VIEWID";
                            kddlFOPUNCH_REASON.DataTextField = "SYS_NAME";
                            kddlFOPUNCH_REASON.DataBind();
                        }
                        // 設定Picker是否能輸入
                        kdtpFOPUNCH_TIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpFOPUNCH_TIME_OFF.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";

                        #region 綁定上班補卡狀態

                        if (!string.IsNullOrEmpty(SCSHRConfiguration.FOPUNCH_TYPE))
                        {
                            DataTable dtSource = new DataTable();
                            dtSource.Columns.Add(new DataColumn("VALUE", typeof(string)));
                            dtSource.Columns.Add(new DataColumn("TEXT", typeof(string)));
                            DataRow ndr = dtSource.NewRow();
                            ndr["TEXT"] = "====請選擇====";
                            ndr["VALUE"] = "";
                            dtSource.Rows.Add(ndr);
                            foreach (string _type in SCSHRConfiguration.FOPUNCH_TYPE.Split(","))
                            {
                                string typeName = "";
                                switch (_type)
                                {
                                    case "2":
                                        typeName = "刷卡上班";
                                        break;
                                    case "0":
                                        typeName = "加班上班(前)";
                                        break;
                                    case "4":
                                        typeName = "加班上班(後)";
                                        break;
                                }
                                //kddlFOPUNCHTYPE.Items.Add(new ListItem(typeName, _type));
                                DataRow _ndr = dtSource.NewRow();
                                _ndr["TEXT"] = typeName;
                                _ndr["VALUE"] = _type;
                                dtSource.Rows.Add(_ndr);
                            }
                            kddlFOPUNCHTYPE.DataSource = dtSource;
                            kddlFOPUNCHTYPE.DataTextField = "TEXT";
                            kddlFOPUNCHTYPE.DataValueField = "VALUE";
                            kddlFOPUNCHTYPE.DataBind();
                            kddlFOPUNCHTYPE.BindDataOnly = true;
                        }

                        #endregion 綁定上班補卡狀態

                        #region 綁定下班補卡狀態

                        if (!string.IsNullOrEmpty(SCSHRConfiguration.FOPUNCH_OFF_TYPE))
                        {
                            DataTable dtSource = new DataTable();
                            dtSource.Columns.Add(new DataColumn("VALUE", typeof(string)));
                            dtSource.Columns.Add(new DataColumn("TEXT", typeof(string)));
                            DataRow ndr = dtSource.NewRow();
                            ndr["TEXT"] = "====請選擇====";
                            ndr["VALUE"] = "";
                            dtSource.Rows.Add(ndr);
                            foreach (string _type in SCSHRConfiguration.FOPUNCH_OFF_TYPE.Split(","))
                            {
                                string typeName = "";
                                switch (_type)
                                {
                                    case "3":
                                        typeName = "刷卡下班";
                                        break;
                                    case "1":
                                        typeName = "加班下班(前)";
                                        break;
                                    case "5":
                                        typeName = "加班下班(後)";
                                        break;
                                }
                                //kddlFOPUNCHTYPE_OFF.Items.Add(new ListItem(typeName, _type));
                                DataRow _ndr = dtSource.NewRow();
                                _ndr["TEXT"] = typeName;
                                _ndr["VALUE"] = _type;
                                dtSource.Rows.Add(_ndr);
                            }
                            kddlFOPUNCHTYPE_OFF.DataSource = dtSource;
                            kddlFOPUNCHTYPE_OFF.DataTextField = "TEXT";
                            kddlFOPUNCHTYPE_OFF.DataValueField = "VALUE";
                            kddlFOPUNCHTYPE_OFF.DataBind();
                            kddlFOPUNCHTYPE_OFF.BindDataOnly = true;
                        }

                        #endregion 綁定下班補卡狀態
                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Signin: // 表單簽核
                        if (taskObj != null &&
                            taskObj.CurrentSite != null &&
                            taskObj.CurrentSite.SiteCode == "HR")
                        {
                            kddlFOPUNCHTYPE.ViewType = KYTViewType.Input; // 補上班卡狀態
                            kddlFOPUNCHTYPE_OFF.ViewType = KYTViewType.Input; // 補下班卡狀態
                        }
                        break;
                    case FieldMode.Print: // 表單列印
                    case FieldMode.Verify: // Verify
                    case FieldMode.View: // 表單觀看
                        //kddlFOPUNCHTYPE_SelectedIndexChanged(kddlFOPUNCHTYPE, null);
                        //kddlFOPUNCHTYPE_OFF_SelectedIndexChanged(kddlFOPUNCHTYPE_OFF, null);
                        break;
                }
                ktxtSignResult.ViewType = KYTViewType.ReadOnly;
            }
            else // 如果網頁POSTBACK
            {
                JGlobalLibs.WebUtils.RequestHiddenFields(UpdatePanel1); // 取回HiddenField的值
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        // 設定Picker是否能輸入
                        kdtpFOPUNCH_TIME.ViewType = KYTViewType.Input;
                        kdtpFOPUNCH_TIME_OFF.ViewType = KYTViewType.Input;
                        kdtpFOPUNCH_TIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpFOPUNCH_TIME_OFF.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
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

    private DataTable getFOPUNCHType(string progId, string selectFields)
    {
        DataTable dtScource = new DataTable();
        Exception ex = null;
        dtScource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_FOPUNCH.getLEAVEType.service.Error:{0}", ex.Message));

        DataRow ndr = dtScource.NewRow();
        ndr["SYS_NAME"] = "===請選擇===";
        ndr["SYS_VIEWID"] = "";
        ndr["SYS_ENGNAME"] = "";
        ndr["SYS_ID"] = "";
        dtScource.Rows.InsertAt(ndr, 0);
        return dtScource;
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        lblAPIResultError.Text = "";
        hidAPIResult.Value = "";
        if ((!string.IsNullOrEmpty(kddlFOPUNCHTYPE.SelectedValue) &&
           !string.IsNullOrEmpty(kdtpFOPUNCH_TIME.Text)) ||
           (!string.IsNullOrEmpty(kddlFOPUNCHTYPE_OFF.SelectedValue) &&
           !string.IsNullOrEmpty(kdtpFOPUNCH_TIME_OFF.Text)))
        {
            DataTable dtResult = null;
            bool resultStatus = false;
            hidAPIResult.Value = ""; // 清空之前的API查詢結果
            // 資料檢核
            DateTime dtAttend_START = DateTime.MinValue;
            DateTime.TryParse(kdtpFOPUNCH_TIME.Text, out dtAttend_START); // 上班補卡時間
            DateTime dtAttend_OFF = DateTime.MinValue;
            DateTime.TryParse(kdtpFOPUNCH_TIME_OFF.Text, out dtAttend_OFF); // 下班補卡時間
            // 表頭
            JArray jaTable = new JArray();
            JObject _joTable = new JObject();
            _joTable.Add(new JProperty("USERNO", "1")); // 自訂序號
            _joTable.Add(new JProperty("SYS_VIEWID", "")); // 編號
            _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(ktxtAPPLICANTDATE.Text).ToString("yyyyMMdd"))); // 日期
            //_joTable.Add(new JProperty("TMP_EMPLOYEEID", hidAPPLICANTAccount.Value)); // 人員編號
            _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByAccount(hidAPPLICANTAccount.Value).EmployeeNo)); // 人員編號
            _joTable.Add(new JProperty("TMP_DEPARTID", hidAPPLICANTDEPT_GROUPCODE.Value)); // 部門編號
            _joTable.Add(new JProperty("TMP_REASONID", kddlFOPUNCH_REASON.SelectedValue)); // 補卡原因
            _joTable.Add(new JProperty("ISAGREE", kchbox.Checked ? "1" : "0")); // 我已明確了解以下說明，並且同意遵守所有規定
            _joTable.Add(new JProperty("NOTE", ktxtREMARK.Text)); // 備註
            jaTable.Add(_joTable);
            DataTable dtHeadSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
            dtHeadSource.TableName = SCSHRConfiguration.SCSSPunchProgID;
            // 表身 SUB
            //int sub_userno = 1;
            jaTable = new JArray();
            if (!string.IsNullOrEmpty(kddlFOPUNCHTYPE.SelectedValue))
            {
                _joTable = new JObject();
                _joTable.Add(new JProperty("USERNO", "1")); // 自訂序號
                _joTable.Add(new JProperty("STATUS", kddlFOPUNCHTYPE.SelectedValue)); // 補刷卡狀態
                _joTable.Add(new JProperty("ATTENDDATE", dtAttend_START.ToString("yyyyMMdd"))); // 補刷卡日期
                _joTable.Add(new JProperty("ATTENDTIME", dtAttend_START.ToString("HHmm"))); // 補刷卡時間
                _joTable.Add(new JProperty("REASON", kddlFOPUNCH_REASON.SelectedValue)); // 補刷卡原因
                jaTable.Add(_joTable);
            }
            if (!string.IsNullOrEmpty(kddlFOPUNCHTYPE_OFF.SelectedValue))
            {
                _joTable = new JObject();
                _joTable.Add(new JProperty("USERNO", "1")); // 自訂序號
                _joTable.Add(new JProperty("STATUS", kddlFOPUNCHTYPE_OFF.SelectedValue)); // 補刷卡狀態
                _joTable.Add(new JProperty("ATTENDDATE", dtAttend_OFF.ToString("yyyyMMdd"))); // 補刷卡日期
                _joTable.Add(new JProperty("ATTENDTIME", dtAttend_OFF.ToString("HHmm"))); // 補刷卡時間
                _joTable.Add(new JProperty("REASON", kddlFOPUNCH_REASON.SelectedValue)); // 補刷卡原因
                jaTable.Add(_joTable);
            }
            DataTable dtSubSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
            dtSubSource.TableName = string.Format(@"{0}{1}", SCSHRConfiguration.SCSSPunchProgID, "SUB");
            // 建立DataSet
            DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSPunchProgID);
            dsSource.Tables.Add(dtHeadSource);
            dsSource.Tables.Add(dtSubSource);
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format("UC_KYTI_SCSHR_FOPUNCH::btnCheck_Click::BOImportWS::dsSource:{0}", JsonConvert.SerializeObject(dsSource)));
            Exception ex = null;
            dtResult = service.BOImport(SCSHRConfiguration.SCSSPunchProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
            if (ex != null)
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_FOPUNCH.btnCheck_Click.BOImport.ERROR:{0}", ex.Message));
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format("UC_KYTI_SCSHR_FOPUNCH::btnCheck_Click::BOImportWS::dtResult:{0}", JsonConvert.SerializeObject(dtResult)));
            if (dtResult != null &&
                dtResult.Rows.Count > 0)
            {
                resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
                if (!resultStatus)
                    lblAPIResultError.Text = dtResult.Rows[0]["_MESSAGE"].ToString();
            }
            hidAPIResult.Value = string.IsNullOrEmpty(lblAPIResultError.Text) ? "OK" : lblAPIResultError.Text;
            // 為了滿足檢查完畢後需出現檢查成功
            lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查成功" : lblAPIResultError.Text;
        }
        else
        {
            lblAPIResultError.Text = "無法進行檢核";
        }
    }

    [WebMethod]
    public static string CheckVal(string TIME_START, string TIME_OFF, string TYPE_START, string TYPE_OFF, string workResult, string offResult)
    {
        ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
        JObject joMessage = new JObject();
        string connectionstring = new DatabaseHelper().Command.Connection.ConnectionString;

        DateTime dtTIME_START = DateTime.MinValue;
        DateTime.TryParse(TIME_START, out dtTIME_START);
        DateTime dtTIME_OFF = DateTime.MinValue;
        DateTime.TryParse(TIME_OFF, out dtTIME_OFF);
        if (string.IsNullOrEmpty(TIME_START) &&
            string.IsNullOrEmpty(TIME_OFF))
            joMessage.Add(new JProperty("NOWORK", "\n補卡時間必填"));
        else if (!string.IsNullOrEmpty(TYPE_START) &&
            string.IsNullOrEmpty(TIME_START))
            joMessage.Add(new JProperty("NOWORK_START", "\n上班補卡時間必填"));
        else if (!string.IsNullOrEmpty(TYPE_OFF) &&
            string.IsNullOrEmpty(TIME_OFF))
            joMessage.Add(new JProperty("NOWORK_OFF", "\n下班補卡時間必填"));

        if (!string.IsNullOrEmpty(TYPE_START) &&
            dtTIME_START > DateTime.Now)
            joMessage.Add(new JProperty("START_TIME_Error", "\n上班補卡時間不能大於申請時間"));

        if (!string.IsNullOrEmpty(TYPE_OFF) &&
            dtTIME_OFF > DateTime.Now)
            joMessage.Add(new JProperty("OFF_TIME_Error", "\n下班補卡時間不能大於申請時間"));


        return joMessage.ToString();
    }



    protected void kddlFOPUNCHTYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTDropDownList _kddlFOPUNCHTYPE = (KYTDropDownList)sender;
        if (_kddlFOPUNCHTYPE.SelectedValue == "")
        {
            kdtpFOPUNCH_TIME.Text = "";
        }
    }

    protected void kddlFOPUNCHTYPE_OFF_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTDropDownList _FOPUNCHTYPE_OFF = (KYTDropDownList)sender;
        if (_FOPUNCHTYPE_OFF.SelectedValue == "")
        {
            kdtpFOPUNCH_TIME_OFF.Text = "";
        }
    }
}
