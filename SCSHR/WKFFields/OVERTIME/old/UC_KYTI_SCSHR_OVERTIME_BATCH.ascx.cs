using Ede.Uof.EIP.Organization;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Design;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;


/**
* 修改時間：2021/06/21
* 修改人員：梁夢慈
* 修改項目：
    * 1.隱藏加班狀況按鈕
* 修改原因：
    * 1.修改規格，原規則不適用於批次
* 修改位置：
    * 1.「SetField() -> // 起單或退回申請者」中，註解「rbtnFindOVERTIME.Visible = true; 」
* **/

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
    * 1.「SetField()、setDefaultUserInfo()、btnAPPLICANT_DialogReturn()」中，註解所有EBUser，改為KYT_EBUser
    * 2.「btnCal_Click()」，傳入TMP_EMPLOYEEID，的值 -> 改傳入EmployeeNo
* **/

/**
* 修改時間：2021/06/15
* 修改人員：梁夢慈
* 修改項目：
    * 1.加班人改為多選，按下計算時，一次新增至明細項
    * 2.草稿起單，清空明細項
* 修改原因：
    * 1.新增規格，因應客戶(老楊)新需求
    * 2.同批次請假單規則，因需按下"計算"按鈕(btnCal)才能新增至明細項
* 修改位置：
    * 1.「前端網頁」中，申請人選人按鈕(btnAPPLICANT)屬性-SingleSelect，原為true -> 改為false
    *   「btnAPPLICANT_DialogReturn()」中，將選取的人，新增至ViewState["APPLICANT"]暫存
    *   「btnCal_Click()」中，迴圈巡覽ViewState["APPLICANT"]，依次檢查是否符合卡控條件，再新增至明細項內，並列出每一人的成功/失敗訊息
    * 2.「SetField() -> 起單或退回申請者」中，重新綁訂空的明細
* **/

/**
* 修改時間：2021/06/03
* 修改人員：梁夢慈
* 修改項目：
    * 1. 跳窗-加班狀況，Dialog.Open傳入參數新增START_DATE，更改顯示標題-新增(顯示前後30天範圍內的所有加班單)
* 修改原因：
    * 1. 新增規格，依歸屬日(加班起)的月份，顯示前後1個月所有加班單
* 修改位置：
    * .「SetField()、btnAPPLICANT_DialogReturn()、kdtpOT_START_TextChanged()」中，設定加班狀況跳窗按鈕(rbtnFindOVERTIME)，傳入的參數、顯示標題
* **/

/**
* 修改時間：2021/05/27
* 修改人員：梁夢慈
* 修改項目：
    * 1. 使用選人元件選擇申請者時，重新綁訂"加班狀況"(rbtnFindOVERTIME)按鈕跳窗
* 發生原因：
    * 1. BUG修正，需再次綁訂申請者，不然只會是首次表單的申請者
* 修改位置：
    * 1.「btnAPPLICANT_DialogReturn()」中，按鈕(rbtnFindOVERTIME)再次導向Dialog.Open2
* **/

/**
* 修改時間：2021/05/24
* 修改人員：梁夢慈
* 修改項目：
    * 1. 飛騰加班單加參數，隱藏預加班「DEFAULT_OV_PE」；預設給付方式 X不預設 0加班費 1補休 2加班費及補休
* 發生原因：
    * 1. 新規格，要讓客戶好處理
* 修改位置：
    * 1.「reBindCHANGETYPE」中，〈krblCHANGETYPE〉的選取值，由〈SCSHRConfiguration.DEFAULT_OV_PE〉取得
* **/

/**
* 修改時間：2021/05/21
* 修改人員：莊仁豪
* 修改項目：
    * 1. 註解按下計算按鈕判斷是否有加班原因及加班清單資料的加班原因。
* 發生原因：
    * 1. 老楊測試機按下計算按鈕報錯。加班原因是call飛騰取得，但飛騰給的加班原因為空的，導致取不到加班原因而報錯。
* 修改位置：
    * 1.「insertToOTList」中，註解 ndr["REMARK_NAME"] = kddlREMARK.SelectedItem.Text;
    * 2.「btnCal_Click」中，註解 ndr["REMARK_NAME"] = kddlREMARK.SelectedItem.Text;、 if (string.IsNullOrEmpty(kddlREMARK.SelectedValue))
* **/


/**
* 修改時間：2021/03/31
* 修改人員：梁夢慈
* 修改項目：
    * 1. 將部門&申請人按鈕選擇合併，改為只用一個選人元件(btnAPPLICANT)，可選擇所有人
* 發生原因：
    * 1. 新增規格，操作上更便利，改用冠永騰版本的選人元件
* 修改位置：
    * 「前端網頁」中，於申請人欄位後面新增一個選人元件(btnAPPLICANT)
* **/

/**
* 修改時間：2021/03/15
* 修改人員：梁夢慈
* 修改項目：
    * 1. 除了請單關以外，其餘隱藏表頭
* 發生原因：
    * 1. 新增規格
* 修改位置：
    * 「前端網頁」中，新增一個標籤(id=showHead)，於後端「SetField() ->首次載入」時，將之設定為Visible = false，「SetField() -> 當FieldMode=起單或退回申請者」時，設定為Visible = true
* **/

/**
* 修改時間：2020/04/16
* 修改人員：陳緯榕
* 修改項目：
    * 新規格：標題修改
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈SCSHR批次加班資訊〉改為〈批次加班資訊〉
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
* 修改時間：2019/06/27
* 修改人員：陳緯榕
* 修改項目：
    * 增加：當時間(起)的日期更動時，同時變更時間(迄)的日期；更動時間時不處理
    * 改變畫面
* 修改位置：
    * 「kdtpOT_START_TextChanged」中，當〈SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE〉的值為〈Y〉時，判斷〈如果上次的日期不等於現在的日期，更動迄止時間〉是否成立，成立時修改時間(迄)的日期
    * ************
    * 「前端網頁」中，將〈SCSHR批次加班資訊〉區塊的〈CLASS〉改為〈col-md-12 divheader〉並且去除〈STYLE〉屬性
    * 「前端網頁」中，將〈加班明細〉區塊的〈CLASS〉改為〈col-md-12 divheader〉並且去除〈STYLE〉屬性
* **/

/**
* 修改時間：2019/05/08
* 修改人員：陳緯榕
* 修改項目：
    * KYTPicker在Postback後無法再手動輸入
* 修改位置：
    * 「前端網頁」中，將KYTPicker的屬性〈TextBoxReadOnly〉去除
    * 「SetField」中，將KYTPicker的〈TextBoxReadOnly〉放到〈網頁首次載入〉之外，讓其每次都執行
* **/

/**
* 修改時間：2019/04/12
* 修改人員：陳緯榕
* 修改項目：
    * DispalyTitle顯示錯誤
* 修改位置：
    * 「DispalyTitle」中，修改為〈申請人 申請人部門 申請批次筆數〉
    * 「Message」中，修改為〈申請人 申請人部門 申請批次筆數〉
* **/

/**
* 修改時間：2019/04/09
* 修改人員：陳緯榕
* 修改項目：
    * DateTimePicker的輸入框是否能輸入由dll.config控制
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack〉時，〈kdtpOT_START〉和〈kdtpOT_END〉的屬性〈TextBoxReadOnly〉由〈SCSHRConfiguration.IS_PICKER_READONLY〉控制
* **/

/**
* 修改時間：2019/04/03
* 修改人員：陳緯榕
* 修改項目：
    * 刷卡時間要不要擋必須是可控的
* 修改位置：
    * 「btnCal_Click」中，檢查刷卡時間前，需先判斷〈SCSHRConfiguration.IS_OV_CHK_PUNCH〉是否為〈Y〉
* **/

/**
* 修改時間：2019/03/29
* 修改人員：陳緯榕
* 修改項目：
    * 刷卡時間不合理卻沒被擋單
    * 申請人出現停用人員
* 修改位置：
    * 「btnCal_Click」中，檢查刷卡時間有錯誤後，應該是要判斷〈沒有錯誤訊息〉才繼續往下跑
    * **********
    * 「reBindChoiceEmployee」中，SQL要去〈TB_EB_USER〉過濾〈IS_SUSPENDED = 0〉
* **/

/**
* 修改時間：2019/03/28
* 修改人員：陳緯榕
* 修改項目：
    * 按下計算後，必須檢查刷卡時間
* 修改位置：
    * 「btnCal_Click」中，先檢查加班時間，當起日大於等於迄日時，不處理
    * 「btnCal_Click」中，取得刷卡時間後，檢查首筆刷卡時間是否小於等於起日；檢查尾筆刷卡時間是否大於等於迄日
* **/

/**
* 修改時間：2019/03/12
* 修改人員：陳緯榕
* 修改項目：
    * 簽核時，開起表單出現錯誤「找不到OT_DOOVERTYPE」
* 修改位置：
    * 「前端網頁」中，〈ktxtOT_DOOVERTYPE〉、〈ktxtOT_DOOVERTYPE_NAME〉、〈ktxtOT_WORKPUNCH〉、〈ktxtOT_OFFWORKPUNCH〉、〈ktxtOT_MFREEDATE〉、〈ktxtOT_OVERCOMPHOURS〉不使用自動塞入內容，改用RowDataBound時指定內容
    * 「gvOVs_RowDataBound」中，指定〈ktxtOT_DOOVERTYPE〉、〈ktxtOT_DOOVERTYPE_NAME〉、〈ktxtOT_WORKPUNCH〉、〈ktxtOT_OFFWORKPUNCH〉、〈ktxtOT_MFREEDATE〉、〈ktxtOT_OVERCOMPHOURS〉的內容
* **/

/**
* 修改時間：2019/03/06
* 修改人員：陳緯榕
* 修改項目：
    * 飛騰更新WS
* 修改位置：
    * 「前端網頁」中，新增〈ktxtOverCompHours〉補休時數、〈ktxtMFreeDate〉補休可休期限、〈ktxtWorkDate〉首筆刷卡日期、〈ktxtWorkTime〉首筆刷卡時間、〈ktxtOffWorkDate〉尾筆刷卡日期、〈ktxtOffWorkTime〉尾筆刷卡時間、〈ktxtDoOverType〉加班別名稱、〈hidDoOverType〉加班別代碼、〈hidAllowCalcType〉加班給付方式允許類型
    * 「前端網頁」「checkVal」中，新增檢查〈hidAllowCalcType〉，當加班給付方式允許類型和所選加班給付方式不同時不可送單
    * 「前端網頁」中，〈gvOVs〉明細增加欄位，〈ktxtOverCompHours〉補休時數、〈ktxtMFreeDate〉補休可休期限、〈ktxtWorkDate〉首筆刷卡日期、〈ktxtWorkTime〉首筆刷卡時間、〈ktxtOffWorkDate〉尾筆刷卡日期、〈ktxtOffWorkTime〉尾筆刷卡時間、〈ktxtDoOverType〉加班別名稱、〈hidDoOverType〉加班別代碼、〈hidAllowCalcType〉加班給付方式允許類型
    * 「getOverTimeListStruct」中，改變資料表結購
    * 「insertToOTList」中，將新的資料放進對應的資料表欄位中
    * 「btnCal_Click」中，呼叫WS的〈BOExecFunc〉〈GetOverTimeHours〉時，新增參數〈CalcType〉
    * 「btnCal_Click」中，收到WS〈BOExecFunc〉回傳值後，將回傳的內容填入新增的物件中
* **/

/**
* 修改時間：2019/02/27
* 修改人員：陳緯榕
* 修改項目：
    * 呼叫計算時數，加上參數〈CalcType〉
* 修改位置：
    * 「btnCal_Click」中，〈BOExecFunc〉加上〈CalcType〉參數
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
* 修改時間：2019/02/13
* 修改人員：陳緯榕
* 修改項目：
    * 客戶千附要求同一個人要能在同一天重複申請
* 修改位置：
    * 「canInsToOTList」中，查詢語法改為
        * 〈加班時間(起) 大於 加班申請時間(起) 而且 加班時間(起) 小於 加班申請時間(迄)〉或者
        * 〈加班時間(迄) 大於 加班申請時間(起) 而且 加班時間(迄) 小於等於 加班申請時間(迄)〉或者
        * 〈加班時間(起) 小於等於 加班申請時間(起) 而且 加班時間(迄) 大於等於 加班申請時間(迄)〉
* **/

/**
* 修改時間：2019/02/12
* 修改人員：陳緯榕
* 修改項目：
    * 隱藏明細項中的「加班原因」，新增欄位「加班內容說明」
* 修改位置：
    * 「前端網頁」中，〈gvOVs〉中的〈加班原因〉隱藏，新增區塊〈加班內容說明〉
    * 「getOverTimeListStruct」中，明細資料表新增欄位〈NOTE〉
    * 「insertToOTList」中，欄位〈NOTE〉給予值為〈ktxtNOTE〉
    * 「gvOVs_RowDataBound」中，物件〈hidREMARK〉全部註解
    * 「gvOVs_RowDataBound」中，物件〈ktxtNOTE〉從後端管理內容
* **/

/**
* 修改時間：2019/01/30
* 修改人員：陳緯榕
* 修改項目：
    * 加班原因要隱藏
    * 給付方式一律為加班費
* 修改位置：
    * 「前端網頁」中，隱藏〈加班原因〉所屬的兩個〈div〉區塊
    * ****
    * 「SetField」中，起單時，將〈krblCHANGETYPE〉設為唯讀，並且在綁定後，指定預設值為〈加班費(0)〉【這完全不考慮客戶沒有將〈TB_EIP_DUTY_SETTING_OVERTIME_HOURS〉，造成找不到加班費的錯誤】
* **/

/**
* 修改時間：2019/01/29
* 修改人員：陳緯榕
* 修改項目：
    * 檢查完畢後，需出現檢查成功
* 修改位置：
    * 「btnCal_Click」中，顯示訊息由〈lblMessage〉改為〈lblOT_TIMESMSG〉
* **/

/**
* 修改時間：2019/01/25
* 修改人員：陳緯榕
* 修改項目：
    * 按下計算顯示"加班內容說明"為必填欄位，但無加班內容說明
    * 加班原因為必填
* 修改位置：
    * 「前端網頁」中，新增加班內容說明區塊〈ktxtNOTE〉
    * 「btnCal_Click」中，呼叫WS前，欄位〈NOTE〉給予〈ktxtNOTE〉加班內容說明
    * 「btnCal_Click」中，處理之前先判斷〈kddlREMARK.SelectedValue〉是否為空，如果是空的就顯示錯誤訊息〈沒有選擇加班原因〉
* **/

/**
* 修改時間：2019/01/24
* 修改人員：陳緯榕
* 修改項目：
    * 帳號包含著網域，造成後續許多問題
* 修改位置：
    * 「SetField」中，將user.Account用〈//〉切割後取最右邊，意圖將網域名稱去除
* **/

/// <summary>
/// 飛騰加班資訊(批次)
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_OVERTIME_BATCH : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
             .Add("changetype", krblCHANGETYPE.SelectedValue)
             .Add("OverTimeHours", ktxtOverTimeHours.Text)
             .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.ConditionValue.cv:{0}", cv));
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
            return string.Format(@"{0}，申請人部門：{1}，申請批次筆數：{2}", ktxtAPPLICANT.Text, ktxtAPPLICANTDEPT.Text, gvOVs.DataTable.Rows.Count);
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
            return string.Format(@"{0}，申請人部門：{1}，申請批次筆數：{2}", ktxtAPPLICANT.Text, ktxtAPPLICANTDEPT.Text, gvOVs.DataTable.Rows.Count);
            //return string.Format(@"{0}，加班折換方式：{1}，加班時間{2}~{3}，時數{4}", ktxtAPPLICANT.Text, krblCHANGETYPE.SelectedIndex > -1 ? krblCHANGETYPE.SelectedItem.Text : "未選擇", kdtpOT_START.Text, kdtpOT_END.Text, ktxtOverTimeHours.Text);
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
                btnCal.Visible = false; // 隱藏計算
                btncoGroup.Visible = false; // 隱藏部門
                btnEmployee.Visible = false; // 隱藏人員
                rbtnFindOVERTIME.Visible = false; // 隱藏加班狀況    

                btnAPPLICANT.Visible = false; // 隱藏申請人

                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
                showHead.Visible = false;
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        hidAPIResult.Value = ""; // 清掉先前的旗標
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                        ktxtOT_CLASSTYPE_NAME.ViewType = KYTViewType.ReadOnly;
                        ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                        ktxtAPPLICANT.ReadOnly = true; // 申請人唯讀
                        ktxtApplyHours.ReadOnly = true; // 申請時數唯讀
                        ktxtOverTimeHours.ReadOnly = true; // 加班時數唯讀
                        btnCal.Visible = true; // 顯示計算
                        //btncoGroup.Visible = true; // 顯示部門
                        //btnEmployee.Visible = true; // 顯示人員
                        //rbtnFindOVERTIME.Visible = true; // 顯示加班狀況
                        btnAPPLICANT.Visible = true; // 顯示申請人
                        ktxtSignResult.ViewType = KYTViewType.ReadOnly; // 簽核狀態回饋
                        //krblCHANGETYPE.ViewType = KYTViewType.ReadOnly; // 給付方式
                        if (this.FormFieldMode == FieldMode.Applicant &&
                            string.IsNullOrEmpty(fieldOptional.FieldValue)) // 如果是剛起單
                        {
                            //setDefaultUserInfo(this.ApplicantGuid, this.ApplicantGroupId);
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 部門名稱
                            hidAPPLICANTDEPT.Value = ApplicantGroupId; // 部門代碼
                            ktxtAPPLICANT.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account); // 申請人姓名
                            hidAPPLICANTGuid.Value = KUser.UserGUID; // 申請人代碼
                            hidAPPLICANT.Value = KUser.Account; // 申請人帳號
                            string[] sAccount = hidAPPLICANT.Value.Split('\\');
                            hidAPPLICANT.Value = sAccount[sAccount.Length - 1];
                            hidGROUPCODE.Value = KUser.GroupCode[0];
                            hidCompanyNo.Value = KUser.CompanyNo;
                            hidAPPLICANTDATE.Value = DateTime.Now.ToString("yyyy/MM/dd"); // 申請日期
                            DataTable dtSource = getOVERTIMEType(SCSHRConfiguration.SCSSOverTimeTypeProgID, "SYS_ViewID,SYS_Name,SYS_EngName");
                            kddlREMARK.DataSource = dtSource;
                            kddlREMARK.DataValueField = "SYS_VIEWID";
                            kddlREMARK.DataTextField = "SYS_NAME";
                            kddlREMARK.DataBind();

                            reBindCHANGETYPE();
                            // 建立加班清單結購
                            gvOVs.DataSource = getOverTimeListStruct();
                            gvOVs.DataBind();
                        }
                        kddlREMARK_SelectedIndexChanged(kddlREMARK, null);
                        btnclEmployee.LimitXML = reBindChoiceEmployee(hidAPPLICANTDEPT.Value); // 綁定選擇同部門人員
                        //krblCHANGETYPE.SelectedValue = "0"; // 固定選加班費
                        ////未有API測試用
                        //hidAPIResult.Value = "OK";
                        // 設定Picker是否能輸入
                        kdtpOT_START.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpOT_END.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";

                        showHead.Visible = true;

                        if (!string.IsNullOrEmpty(ktxtAPPLICANT.Text))
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add(new DataColumn("APPLICANT_ACCOUNT", typeof(string))); // 加班人帳號
                            dt.Columns.Add(new DataColumn("APPLICANT_GUID", typeof(string))); // 加班人GUID
                            dt.Columns.Add(new DataColumn("APPLICANT_NAME", typeof(string))); // 加班人姓名
                            dt.Columns.Add(new DataColumn("GROUP_CODE", typeof(string))); // 加班人部門CODE
                            dt.Columns.Add(new DataColumn("DEPT_NAME", typeof(string))); // 加班人部門名稱
                            dt.Columns.Add(new DataColumn("GROUP_ID", typeof(string))); // 加班人GROUP_ID
                            dt.Columns.Add(new DataColumn("COMPANYNO", typeof(string))); // 加班人公司代碼

                            dt.Columns.Add(new DataColumn("OT_CLASSTYPE_NAME", typeof(string))); // 加班班別名稱
                            dt.Columns.Add(new DataColumn("OT_CLASSTYPE", typeof(string))); // 加班班別
                            dt.Columns.Add(new DataColumn("APPLYHOURS", typeof(string))); // 申請時數
                            dt.Columns.Add(new DataColumn("OVERTIMEHOURS", typeof(string))); // 加班時數
                            dt.Columns.Add(new DataColumn("OT_DOOVERTYPE", typeof(string))); // 加班別
                            dt.Columns.Add(new DataColumn("OT_DOOVERTYPE_NAME", typeof(string))); // 加班別名稱
                            dt.Columns.Add(new DataColumn("OT_WORKPUNCH", typeof(string))); // 首筆刷卡時間
                            dt.Columns.Add(new DataColumn("OT_OFFWORKPUNCH", typeof(string))); // 尾筆刷卡時間
                            dt.Columns.Add(new DataColumn("OT_MFREEDATE", typeof(string))); // 計算後補休可休期限
                            dt.Columns.Add(new DataColumn("OT_OVERCOMPHOURS", typeof(string))); // 計算後的實際補休時數

                            string[] arrLeaUser = ktxtAPPLICANT.Text.Split(',');
                            List<string> litLeaUser = new List<string>();
                            foreach (string name in arrLeaUser)
                            {
                                string acount = name.Split('(')[1].Substring(0, name.Split('(')[1].Length - 1);
                                //EBUser user = KYTUtilLibs.Utils.UOFUtils.UOFUser.getUserInfo(acount);
                                KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByAccount(acount); // 人員

                                string[] sAccount = KUser.Account.Split('\\');

                                DataRow ndrUser = dt.NewRow();
                                ndrUser["APPLICANT_ACCOUNT"] = sAccount[sAccount.Length - 1];
                                ndrUser["APPLICANT_GUID"] = KUser.UserGUID;
                                ndrUser["APPLICANT_NAME"] = KUser.Name;
                                ndrUser["GROUP_CODE"] = KUser.GroupCode[0];
                                ndrUser["DEPT_NAME"] = KUser.GroupName[0];
                                ndrUser["GROUP_ID"] = KUser.GroupID[0];
                                ndrUser["COMPANYNO"] = KUser.CompanyNo;
                                dt.Rows.Add(ndrUser);
                            }
                            ViewState["APPLICANT"] = dt;
                        }

                        // 清空明細
                        gvOVs.DataSource = getOverTimeListStruct();
                        gvOVs.DataBind();

                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Print: // 表單列印
                        break;
                    case FieldMode.Signin: // 表單簽核
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.View: // 表單觀看
                        break;
                }
                Dialog.Open2(rbtnFindOVERTIME,
                    string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/findOVERTIME.aspx"),
                    "已加班時數查詢 (顯示前後30天範圍內的所有加班單)",
                    800,
                    600,
                    Dialog.PostBackType.AfterReturn,
                    new { ACCOUNT = hidAPPLICANT.Value, START_DATE = kdtpOT_START.Text }.ToExpando());
            }
            else // 如果網頁POSTBACK
            {
                JGlobalLibs.WebUtils.RequestHiddenFields(UpdatePanel1); // 取回HiddenField的值
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        // 設定Picker是否能輸入
                        kdtpOT_START.ViewType = KYTViewType.Input;
                        kdtpOT_END.ViewType = KYTViewType.Input;
                        kdtpOT_START.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpOT_END.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
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

    /// <summary>
    /// 設定部門基本訊息
    /// </summary>
    /// <param name="ApplicantGuid"></param>
    /// <param name="ApplicantGroupId"></param>
    private void setDefaultUserInfo(string ApplicantGuid, string ApplicantGroupId)
    {
        //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
        KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
        ktxtAPPLICANTDEPT.Text = JGlobalLibs.UOFUtils.GetGroupNameByDepartmentID(ApplicantGroupId); // 部門名稱
        hidAPPLICANTDEPT.Value = ApplicantGroupId; // 部門代碼
        ktxtAPPLICANT.Text = KUser.Name; // 申請人姓名
        hidAPPLICANTGuid.Value = KUser.UserGUID; // 申請人代碼
        hidAPPLICANT.Value = KUser.Account; // 申請人帳號
        string[] sAccount = hidAPPLICANT.Value.Split('\\');
        hidAPPLICANT.Value = sAccount[sAccount.Length - 1];
        hidGROUPCODE.Value = KUser.GroupName[0];
        hidCompanyNo.Value = KUser.CompanyNo;
    }

    /// <summary>
    /// 設定給付方式
    /// </summary>
    private void reBindCHANGETYPE()
    {
        bool isTIMEOFF = false;
        bool isPAYMENT = false;
        bool isBOTH_TP = false;
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT * FROM TB_EIP_DUTY_SETTING_OVERTIME_HOURS WHERE GROUP_GUID = 'Company' AND TYPE = 'Workday'
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("", "");
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    isTIMEOFF = (bool)dr["ALLOW_TIMEOFF"];
                    isPAYMENT = (bool)dr["ALLOW_PAYMENT"];
                    isBOTH_TP = (bool)dr["ALLOW_BOTH_TP"];
                }
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.reBindCHANGETYPE.ERROR:{0}", e.Message));
            }
        }

        if (isTIMEOFF && isPAYMENT)
        {
            JGlobalLibs.WebUtils.DynamicListItemBind(krblCHANGETYPE, new object[] {
                new { Text= "加班費", Value= "0" },
                new { Text= "補休", Value= "1" }
            });
        }
        else if (isTIMEOFF)
        {
            JGlobalLibs.WebUtils.DynamicListItemBind(krblCHANGETYPE, new object[] {
                new { Text= "補休", Value= "1" }
            });
        }
        else if (isPAYMENT)
        {
            JGlobalLibs.WebUtils.DynamicListItemBind(krblCHANGETYPE, new object[] {
                new { Text= "加班費", Value= "0" }
            });
        }
        else if (isBOTH_TP)
        {
            JGlobalLibs.WebUtils.DynamicListItemBind(krblCHANGETYPE, new object[] {
                new { Text= "加班費及補休", Value= "2" }
            });
        }

        int defaultType = 1;
        int.TryParse(SCSHRConfiguration.DEFAULT_OV_PE, out defaultType);
        krblCHANGETYPE.SelectedValue = defaultType.ToString(); // 預設由設定檔取得
    }
    /// <summary>
    /// 綁定該部門下所有的使用者
    /// </summary>
    /// <param name="GROUP_ID"></param>
    /// <returns></returns>
    private string reBindChoiceEmployee(string GROUP_ID)
    {
        string result = "";
        // 綁定該部門下所有的使用者
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
                -- 查詢同部門人員
             SELECT * 
               FROM TB_EB_EMPL_DEP 
              WHERE GROUP_ID = @GROUP_ID
                AND USER_GUID = (SELECT USER_GUID 
                                   FROM TB_EB_USER 
                                  WHERE USER_GUID = TB_EB_EMPL_DEP.USER_GUID 
                                    AND IS_SUSPENDED = 0)
                ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@GROUP_ID", hidAPPLICANTDEPT.Value);
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    btnclEmployee.Clear();
                    UserSet us = new UserSet();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserSetUser usu = new UserSetUser();
                        usu.USER_GUID = dr["USER_GUID"].ToString();
                        us.Items.Add(usu);
                    }
                    result = us.GetXML();
                }
            }
            catch (Exception ex)
            {
            }
        }
        return result;
    }

    /// <summary>
    /// 取得加班時數
    /// </summary>
    /// <param name="account"></param>
    /// <param name="dtStart"></param>
    /// <param name="dtOff"></param>
    /// <returns></returns>
    private decimal getOverTimeHours(string account, DateTime dtStart, DateTime dtOff)
    {
        decimal totalHours = 0;

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            -- 找到時間範圍內的加班單
            SELECT ISNULL(SUM(OT_TIMES), 0)  AS 'TOTALHOURS'
              FROM Z_SCSHR_OVERTIME 
             WHERE APPLICANT = @ACCOUNT
               AND (OT_START BETWEEN @START 
                                 AND @OFF
                    OR OT_END BETWEEN @START 
                                  AND @OFF)
               AND ((TASK_STATUS = 1 AND TASK_RESULT IS NULL)
                    OR (TASK_STATUS = 2 AND TASK_RESULT = 0))
               AND CANCEL_DOC_NBR IS NULL

            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@ACCOUNT", account);
            sda.SelectCommand.Parameters.AddWithValue("@START", dtStart);
            sda.SelectCommand.Parameters.AddWithValue("@OFF", dtOff);
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    totalHours = (decimal)ds.Tables[0].Rows[0]["TOTALHOURS"];
                }
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.getOverTimeHours.ERROR:{0}", e.Message));
            }
        }
        return totalHours;
    }

    private DataTable getOVERTIMEType(string progId, string selectFields)
    {
        DataTable dtScource = new DataTable();
        Exception ex = null;
        dtScource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.getLEAVEType.service.Error:{0}", ex.Message));

        return dtScource;
    }
    /// <summary>
    /// 取得加班清單結購
    /// </summary>
    /// <returns></returns>
    private DataTable getOverTimeListStruct()
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("DEPT_NAME", typeof(string))); // 部門名稱
        dtSource.Columns.Add(new DataColumn("GROUP_ID", typeof(string))); // UOF部門代碼
        dtSource.Columns.Add(new DataColumn("APPLICANTDATE", typeof(DateTime))); // 申請日期
        dtSource.Columns.Add(new DataColumn("COMPANYNO", typeof(string))); // 公司代碼
        dtSource.Columns.Add(new DataColumn("GROUP_CODE", typeof(string))); // 部門代碼
        dtSource.Columns.Add(new DataColumn("APPLICANT_NAME", typeof(string))); // 申請人名稱
        dtSource.Columns.Add(new DataColumn("APPLICANT_GUID", typeof(string))); // 申請人代碼
        dtSource.Columns.Add(new DataColumn("APPLICANT_ACCOUNT", typeof(string))); // 申請人帳號
        dtSource.Columns.Add(new DataColumn("CHANGETYPE_NAME", typeof(string))); // 給付方式名稱
        dtSource.Columns.Add(new DataColumn("CHANGETYPE", typeof(string))); // 給付方式代碼
        dtSource.Columns.Add(new DataColumn("OT_START", typeof(DateTime))); // 加班時間(起)
        dtSource.Columns.Add(new DataColumn("OT_END", typeof(DateTime))); // 加班時間(迄)
        dtSource.Columns.Add(new DataColumn("OT_CLASSTYPE_NAME", typeof(string))); // 加班班別名稱
        dtSource.Columns.Add(new DataColumn("OT_CLASSTYPE", typeof(string))); // 加班班別
        dtSource.Columns.Add(new DataColumn("REMARK_NAME", typeof(string))); // 加班原因
        dtSource.Columns.Add(new DataColumn("REMARK", typeof(string))); // 加班原因代碼
        dtSource.Columns.Add(new DataColumn("APPLYHOURS", typeof(decimal))); // 申請時數
        dtSource.Columns.Add(new DataColumn("OVERTIMEHOURS", typeof(decimal))); // 加班時數
        dtSource.Columns.Add(new DataColumn("NOTE", typeof(string))); // 加班內容說明
        dtSource.Columns.Add(new DataColumn("OT_DOOVERTYPE", typeof(string))); // 加班別
        dtSource.Columns.Add(new DataColumn("OT_DOOVERTYPE_NAME", typeof(string))); // 加班別名稱
        dtSource.Columns.Add(new DataColumn("OT_WORKPUNCH", typeof(DateTime))); // 首筆刷卡時間
        dtSource.Columns.Add(new DataColumn("OT_OFFWORKPUNCH", typeof(DateTime))); // 尾筆刷卡時間
        dtSource.Columns.Add(new DataColumn("OT_MFREEDATE", typeof(string))); // 計算後補休可休期限
        dtSource.Columns.Add(new DataColumn("OT_OVERCOMPHOURS", typeof(decimal))); // 計算後的實際補休時數
        return dtSource;
    }

    /// <summary>
    /// 新增加班清單資料
    /// </summary>
    /// <returns></returns>
    private DataTable insertToOTList()
    {
        DataTable tblOVs = gvOVs.DataTable;
        DataRow ndr = tblOVs.NewRow();
        ndr["DEPT_NAME"] = ktxtAPPLICANTDEPT.Text; // 部門名稱
        ndr["GROUP_ID"] = hidAPPLICANTDEPT.Value; // UOF部門代碼
        ndr["APPLICANTDATE"] = DateTime.Now.Date; // 申請日期
        ndr["COMPANYNO"] = hidCompanyNo.Value; // 公司代碼
        ndr["GROUP_CODE"] = hidGROUPCODE.Value; // 部門代碼
        ndr["APPLICANT_NAME"] = ktxtAPPLICANT.Text; // 申請人名稱
        ndr["APPLICANT_GUID"] = hidAPPLICANTGuid.Value; // 申請人代碼
        ndr["APPLICANT_ACCOUNT"] = hidAPPLICANT.Value; // 申請人帳號
        ndr["CHANGETYPE_NAME"] = krblCHANGETYPE.SelectedItem.Text; // 給付方式名稱
        ndr["CHANGETYPE"] = krblCHANGETYPE.SelectedValue; // 給付方式代碼
        ndr["OT_START"] = kdtpOT_START.Text; // 加班時間(起)
        ndr["OT_END"] = kdtpOT_END.Text; // 加班時間(迄)
        ndr["OT_CLASSTYPE_NAME"] = ktxtOT_CLASSTYPE_NAME.Text; // 加班班別名稱
        ndr["OT_CLASSTYPE"] = hidOT_CLASSTYPE.Value; // 加班班別
        // ndr["REMARK_NAME"] = kddlREMARK.SelectedItem.Text; // 加班原因
        //ndr["REMARK"] = kddlREMARK.SelectedValue; // 加班原因
        ndr["APPLYHOURS"] = ktxtApplyHours.Text; // 申請時數
        ndr["OVERTIMEHOURS"] = ktxtOverTimeHours.Text; // 加班時數
        ndr["NOTE"] = ktxtNOTE.Text.Trim(); // 加班內容說明
        ndr["OT_DOOVERTYPE"] = hidDoOverType.Value.Trim(); // 加班別
        ndr["OT_DOOVERTYPE_NAME"] = ktxtDoOverType.Text.Trim(); // 加班別名稱
        ndr["OT_WORKPUNCH"] = !string.IsNullOrEmpty(ktxtWorkPunch.Text.Trim()) ? ktxtWorkPunch.Text.Trim() : Convert.DBNull; // 首筆刷卡時間
        ndr["OT_OFFWORKPUNCH"] = !string.IsNullOrEmpty(ktxtOffWorkPunch.Text.Trim()) ? ktxtOffWorkPunch.Text.Trim() : Convert.DBNull; // 尾筆刷卡時間
        ndr["OT_MFREEDATE"] = ktxtMFreeDate.Text.Trim(); // 計算後補休可休期限
        ndr["OT_OVERCOMPHOURS"] = !string.IsNullOrEmpty(ktxtOverCompHours.Text.Trim()) ? ktxtOverCompHours.Text.Trim() : "0"; // 計算後的實際補休時數
        tblOVs.Rows.Add(ndr);
        return tblOVs;
    }

    /// <summary>
    /// 檢查加班清單資料是否能新增
    /// </summary>
    /// <returns></returns>
    private bool canInsToOTList(string GROUP_ID, string APPLICANT_GUID)
    {
        DataTable dtSource = gvOVs.DataTable;
        //DataRow[] drs = dtSource.Select(string.Format("GROUP_ID='{0}' AND APPLICANT_GUID='{1}'", hidAPPLICANTDEPT.Value, hidAPPLICANTGuid.Value));
        DataRow[] drs = dtSource.Select(string.Format("GROUP_ID='{0}' AND APPLICANT_GUID='{1}' AND ((OT_START > '{2}' AND OT_START < '{3}') OR (OT_END > '{2}' AND OT_END <= '{3}') OR (OT_START <= '{2}' AND OT_END >= '{3}')) ",
            GROUP_ID,
            APPLICANT_GUID,
            kdtpOT_START.Text,
            kdtpOT_END.Text));
        return drs.Length == 0;
    }

    protected void kddlREMARK_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTDropDownList kddlREMARK = (KYTDropDownList)sender;
        ktxtREMARKOther.Visible = false;
        ktxtREMARKOther.Text = "";
        if (kddlREMARK.SelectedValue == "5")
            ktxtREMARKOther.Visible = true;
    }

    /// <summary>
    /// 按下計算按鈕觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCal_Click(object sender, EventArgs e)
    {
        ktxtApplyHours.Text = "0";
        ktxtOverTimeHours.Text = "0";
        hidAPIResult.Value = "";
        lblOT_TIMESMSG.Text = "";
        //hidC_DAY.Value = "";
        hidStyle.Value = "";
        hidConfirm.Value = "";
        lblMessage.Text = "";
        if (!string.IsNullOrEmpty(kdtpOT_END.Text) &&
            !string.IsNullOrEmpty(kdtpOT_START.Text) &&
            krblCHANGETYPE.SelectedIndex != -1 &&
            !string.IsNullOrEmpty(hidAPPLICANTDEPT.Value) &&
            !string.IsNullOrEmpty(hidAPPLICANTGuid.Value)
           //   !string.IsNullOrEmpty(kddlREMARK.SelectedValue)
           )
        {
            DataTable dtCalcResult = null;
            DateTime dtStart = DateTime.MinValue;
            DateTime dtEnd = DateTime.MinValue;
            DateTime.TryParse(kdtpOT_START.Text, out dtStart);
            DateTime.TryParse(kdtpOT_END.Text, out dtEnd);
            // 檢查加班起訖
            if (DateTime.Compare(dtStart, dtEnd) == -1) // 當加班起日小於迄日
            {

                // 迴圈檢查所有申請人
                string ErrorMSG_ALL = "";
                DataTable dt = (DataTable)ViewState["APPLICANT"];

                foreach (DataRow drAPPLICANT in dt.Rows)
                {
                    string ErrorMSG = "";
                    string ACCOUT = drAPPLICANT["APPLICANT_ACCOUNT"].ToString();
                    string GUID = drAPPLICANT["APPLICANT_GUID"].ToString();
                    string NAME = drAPPLICANT["APPLICANT_NAME"].ToString();
                    string GROUP_CODE = drAPPLICANT["GROUP_CODE"].ToString();
                    string GROUP_NAME = drAPPLICANT["DEPT_NAME"].ToString();
                    string GROUP_ID = drAPPLICANT["GROUP_ID"].ToString();
                    string COMPANYNO = drAPPLICANT["COMPANYNO"].ToString();
                    string OT_CLASSTYPE_NAME = ""; // 加班班別名稱
                    string OT_CLASSTYPE = ""; // 加班班別
                    string APPLYHOURS = ""; // 申請時數
                    string OVERTIMEHOURS = ""; // 加班時數
                    string OT_DOOVERTYPE = ""; // 加班別
                    string OT_DOOVERTYPE_NAME = ""; // 加班別名稱
                    string OT_WORKPUNCH = ""; // 首筆刷卡時間
                    string OT_OFFWORKPUNCH = ""; // 尾筆刷卡時間
                    string OT_MFREEDATE = ""; // 計算後補休可休期限
                    string OT_OVERCOMPHOURS = ""; // 計算後的實際補休時數

                    #region 呼叫飛騰-取得此人加班資料

                    Exception ex = null;
                    SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSGetOverTimeHoursProdID,
                    "GetOverTimeHours",
                    SCSHR.Types.SCSParameter.getPatameters(
                        new
                        {
                            TMP_EmployeeID = ACCOUT,
                            StartDate = dtStart.ToString("yyyyMMdd"),
                            StartTime = dtStart.ToString("HHmm"),
                            EndDate = dtEnd.ToString("yyyyMMdd"),
                            EndTime = dtEnd.ToString("HHmm"),
                            CalcType = krblCHANGETYPE.SelectedValue
                        }),
                    out ex);
                    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.BOExecFunc.Parameter:{0}", new
                    {
                        TMP_EmployeeID = ACCOUT,
                        StartDate = dtStart.ToString("yyyyMMdd"),
                        StartTime = dtStart.ToString("HHmm"),
                        EndDate = dtEnd.ToString("yyyyMMdd"),
                        EndTime = dtEnd.ToString("HHmm"),
                        CalcType = krblCHANGETYPE.SelectedValue
                    }.ToString()));

                    if (ex != null)
                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.GetOverTimeHours.ERROR:{0}", ex.Message));
                    if (parameters != null &&
                        parameters.Length > 0)
                    {
                        if (parameters[0].DataType.ToString() == "DataTable")
                        {
                            dtCalcResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                        }
                    }

                    #endregion 呼叫飛騰-取得此人加班資料

                    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.BOExecFunc:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtCalcResult)));
                    if (dtCalcResult != null &&
                        dtCalcResult.Rows.Count > 0)
                    {
                        // 填入申請時數、加班時數
                        APPLYHOURS = dtCalcResult.Rows[0]["ApplyHours"].ToString();
                        OVERTIMEHOURS = dtCalcResult.Rows[0]["OverTimeHours"].ToString();
                        OT_CLASSTYPE = dtCalcResult.Rows[0]["TMP_WorkID"].ToString();
                        OT_CLASSTYPE_NAME = dtCalcResult.Rows[0]["TMP_WorkName"].ToString();

                        OT_OVERCOMPHOURS = dtCalcResult.Columns.Contains("OverCompHours") ? dtCalcResult.Rows[0]["OverCompHours"].ToString() : ""; // 實際補休時數
                        OT_MFREEDATE = dtCalcResult.Columns.Contains("MFreeDate") ? dtCalcResult.Rows[0]["MFreeDate"].ToString() : ""; // 補休可休期限

                        string WorkDate = dtCalcResult.Columns.Contains("WorkDate") ? dtCalcResult.Rows[0]["WorkDate"].ToString() : ""; // 首筆刷卡日期
                        string WorkTime = dtCalcResult.Columns.Contains("WorkTime") ? dtCalcResult.Rows[0]["WorkTime"].ToString() : ""; //首筆刷卡時間
                        if (WorkDate.Length > 10)
                            OT_WORKPUNCH = string.Format(@"{0} {1}", WorkDate.Substring(0, 10), WorkTime.Insert(2, ":")); // 組合出時間

                        string OffWorkDate = dtCalcResult.Columns.Contains("OffWorkDate") ? dtCalcResult.Rows[0]["OffWorkDate"].ToString() : ""; // 尾筆刷卡日期
                        string OffWorkTime = dtCalcResult.Columns.Contains("OffWorkTime") ? dtCalcResult.Rows[0]["OffWorkTime"].ToString() : ""; // 尾筆刷卡時間
                        if (OffWorkDate.Length > 10)
                            OT_OFFWORKPUNCH = string.Format(@"{0} {1}", OffWorkDate.Substring(0, 10), OffWorkTime.Insert(2, ":")); // 組合出時間

                        OT_DOOVERTYPE = dtCalcResult.Columns.Contains("DoOverType") ? dtCalcResult.Rows[0]["DoOverType"].ToString() : ""; // 加班別
                        hidAllowCalcType.Value = dtCalcResult.Columns.Contains("AllowCalcType") ? dtCalcResult.Rows[0]["AllowCalcType"].ToString() : ""; // 加班給付方式允許類型  <-- 好像沒有用到此欄位了?
                        OT_DOOVERTYPE_NAME = OT_DOOVERTYPE == "0" ? "平日" : OT_DOOVERTYPE == "1" ? "休息日" : OT_DOOVERTYPE == "2" ? "特殊日" : OT_DOOVERTYPE == "3" ? "例假日" : OT_DOOVERTYPE == "4" ? "變形休息日" : OT_DOOVERTYPE == "5" ? "國定假日" : "";

                        // 回填表單，申請時數、加班時數
                        ktxtApplyHours.Text = APPLYHOURS;
                        ktxtOverTimeHours.Text = OVERTIMEHOURS;
                        hidOT_CLASSTYPE.Value = OT_CLASSTYPE;
                        ktxtOT_CLASSTYPE_NAME.Text = OT_CLASSTYPE_NAME;

                        ktxtOverCompHours.Text = OT_OVERCOMPHOURS; // 實際補休時數
                        ktxtMFreeDate.Text = OT_MFREEDATE; // 補休可休期限

                        ktxtWorkPunch.Text = OT_WORKPUNCH; // 組合出時間
                        ktxtOffWorkPunch.Text = OT_OFFWORKPUNCH; // 組合出時間

                        hidDoOverType.Value = OT_DOOVERTYPE; // 加班別
                        hidAllowCalcType.Value = dtCalcResult.Columns.Contains("AllowCalcType") ? dtCalcResult.Rows[0]["AllowCalcType"].ToString() : ""; // 加班給付方式允許類型
                        ktxtDoOverType.Text = OT_DOOVERTYPE_NAME;
                    }
                    else
                    {
                        ErrorMSG = "該會員無法申請加班，因為沒有班表資料";
                    }

                    if (!string.IsNullOrEmpty(OT_CLASSTYPE))
                    {
                        DateTime dtStartPunch = DateTime.MinValue;
                        DateTime.TryParse(OT_WORKPUNCH.Trim(), out dtStartPunch);
                        DateTime dtOffPunch = DateTime.MinValue;
                        DateTime.TryParse(OT_OFFWORKPUNCH.Trim(), out dtOffPunch);
                        if (SCSHRConfiguration.IS_OV_CHK_PUNCH.ToUpper() == "Y")
                        {
                            // 檢查刷卡時間合理性
                            if (DateTime.Compare(dtStartPunch, dtStart) == 1)
                            {
                                ErrorMSG =string.Format("加班時間(起)不可小於首筆刷卡時間，首筆刷卡時間:{0}", dtStartPunch.ToString("yyyy/MM/dd HH:mm"));
                            }
                            if (DateTime.Compare(dtEnd, dtOffPunch) == 1)
                            {
                                ErrorMSG = string.Format("加班時間(迄)不可小於尾筆刷卡時間，尾筆刷卡時間:{0}", dtOffPunch.ToString("yyyy/MM/dd HH:mm"));
                            }
                        }

                        if (string.IsNullOrEmpty(ErrorMSG))
                        {
                            bool resultStatus = false;
                            // 檢核資料
                            JArray jaTable = new JArray();
                            JObject _joTable = new JObject();
                            _joTable.Add(new JProperty("USERNO", "1"));
                            _joTable.Add(new JProperty("SYS_VIEWID", ""));
                            _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(hidAPPLICANTDATE.Value).ToString("yyyyMMdd"))); // 日期
                            //_joTable.Add(new JProperty("TMP_EMPLOYEEID", ACCOUT)); // 加班人員
                            _joTable.Add(new JProperty("TMP_EMPLOYEEID",new KYT_UserPO().GetUserDetailByAccount(ACCOUT).EmployeeNo)); // 加班人員
                            _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd"))); // 起始日期
                            _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm"))); // 起始時間
                            _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm"))); // 結束時間
                            _joTable.Add(new JProperty("DoOverType", OT_DOOVERTYPE)); // 加班別
                            _joTable.Add(new JProperty("WorkDate", dtStartPunch.ToString("yyyyMMdd"))); // 首筆刷卡日期
                            _joTable.Add(new JProperty("WorkTime", dtStartPunch.ToString("HHmm"))); // 首筆刷卡時間
                            _joTable.Add(new JProperty("OffWorkDate", dtOffPunch.ToString("yyyyMMdd"))); // 尾筆刷卡日期
                            _joTable.Add(new JProperty("OffWorkTime", dtOffPunch.ToString("HHmm"))); // 尾筆刷卡時間
                            _joTable.Add(new JProperty("MFreeDate", ktxtMFreeDate.Text)); // 計算後補休可休期限
                            _joTable.Add(new JProperty("OverCompHours", OT_OVERCOMPHOURS)); // 計算後的實際補休時數
                            _joTable.Add(new JProperty("TMP_ATTREASONID", "")); // 加班原因 <-- 因為加班原因欄位、卡控已拿掉
                            _joTable.Add(new JProperty("TMP_WORKID", OT_CLASSTYPE)); // 加班班別
                            //_joTable.Add(new JProperty("RESTMINS", "0")); // 休息時間(分鐘)
                            _joTable.Add(new JProperty("CALCTYPE", krblCHANGETYPE.SelectedValue)); // 給付方式
                            _joTable.Add(new JProperty("NOTE", ktxtNOTE.Text)); // 加班內容說明
                            jaTable.Add(_joTable);
                            DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                            dtSource.TableName = SCSHRConfiguration.SCSSOverTimeProgID;
                            DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSOverTimeProgID);
                            dsSource.Tables.Add(dtSource);
                            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.dtSource:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtSource)));
                            Exception oex = null;
                            DataTable dtResult = service.BOImport(SCSHRConfiguration.SCSSOverTimeProgID, false, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out oex);
                            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.Result:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
                            if (oex != null)
                            {
                                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH::btnCal_Click::BOImportWS::StackTrace::{0}::ERROR::{1}", oex.StackTrace, oex.Message));
                                
                                ErrorMSG = "檢查失敗";
                            }
                            else
                            {
                                if (dtResult != null &&
                                    dtResult.Rows.Count > 0)
                                {
                                    resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
                                    if (!resultStatus)
                                    {
                                        ErrorMSG = dtResult.Rows[0]["_MESSAGE"].ToString();
                                    }
                                }

                            }
                            if (resultStatus)
                            {
                                ErrorMSG = "檢查成功";
                                if (canInsToOTList(GROUP_ID, GUID))
                                {
                                    DataTable tblOVs = gvOVs.DataTable;
                                    DataRow ndr = tblOVs.NewRow();
                                    ndr["DEPT_NAME"] = GROUP_NAME; // 部門名稱
                                    ndr["GROUP_ID"] = GROUP_ID; // UOF部門代碼
                                    ndr["APPLICANTDATE"] = DateTime.Now.Date; // 申請日期
                                    ndr["COMPANYNO"] = COMPANYNO; // 公司代碼
                                    ndr["GROUP_CODE"] = GROUP_CODE; // 部門代碼
                                    ndr["APPLICANT_NAME"] = NAME; // 申請人名稱
                                    ndr["APPLICANT_GUID"] = GUID; // 申請人代碼
                                    ndr["APPLICANT_ACCOUNT"] = ACCOUT; // 申請人帳號
                                    ndr["CHANGETYPE_NAME"] = krblCHANGETYPE.SelectedItem.Text; // 給付方式名稱
                                    ndr["CHANGETYPE"] = krblCHANGETYPE.SelectedValue; // 給付方式代碼
                                    ndr["OT_START"] = kdtpOT_START.Text; // 加班時間(起)
                                    ndr["OT_END"] = kdtpOT_END.Text; // 加班時間(迄)
                                    ndr["OT_CLASSTYPE_NAME"] = OT_CLASSTYPE_NAME; // 加班班別名稱
                                    ndr["OT_CLASSTYPE"] = OT_CLASSTYPE; // 加班班別
                                    //ndr["REMARK_NAME"] = kddlREMARK.SelectedItem.Text; // 加班原因
                                    //ndr["REMARK"] = kddlREMARK.SelectedValue; // 加班原因
                                    ndr["APPLYHOURS"] = APPLYHOURS; // 申請時數
                                    ndr["OVERTIMEHOURS"] = OVERTIMEHOURS; // 加班時數
                                    ndr["NOTE"] = ktxtNOTE.Text.Trim(); // 加班內容說明
                                    ndr["OT_DOOVERTYPE"] = OT_DOOVERTYPE.Trim(); // 加班別
                                    ndr["OT_DOOVERTYPE_NAME"] = OT_DOOVERTYPE_NAME.Trim(); // 加班別名稱
                                    ndr["OT_WORKPUNCH"] = !string.IsNullOrEmpty(OT_WORKPUNCH.Trim()) ? OT_WORKPUNCH.Trim() : Convert.DBNull; // 首筆刷卡時間
                                    ndr["OT_OFFWORKPUNCH"] = !string.IsNullOrEmpty(OT_OFFWORKPUNCH.Trim()) ? OT_OFFWORKPUNCH.Trim() : Convert.DBNull; // 尾筆刷卡時間
                                    ndr["OT_MFREEDATE"] = OT_MFREEDATE.Trim(); // 計算後補休可休期限
                                    ndr["OT_OVERCOMPHOURS"] = !string.IsNullOrEmpty(OT_OVERCOMPHOURS.Trim()) ? OT_OVERCOMPHOURS.Trim() : "0"; // 計算後的實際補休時數
                                    tblOVs.Rows.Add(ndr);
                                    gvOVs.DataSource = tblOVs;
                                    gvOVs.DataBind();
                                }
                                else
                                {
                                    ErrorMSG = "該會員無法重複申請加班";
                                }
                            }
                        }
                        else
                        {
                            // 刷卡時間有問題
                            //ErrorMSG = "刷卡時間有問題";
                        }
                    }

                    ErrorMSG_ALL += string.Format("<br />申請人: {0}({1})，{2}", NAME, ACCOUT, ErrorMSG);
                }
                lblOT_TIMESMSG.Text = ErrorMSG_ALL;

                #region 原版-只檢查一人

                //Exception ex = null;
                //SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSGetOverTimeHoursProdID,
                //"GetOverTimeHours",
                //SCSHR.Types.SCSParameter.getPatameters(
                //    new
                //    {
                //        TMP_EmployeeID = hidAPPLICANT.Value,
                //        StartDate = dtStart.ToString("yyyyMMdd"),
                //        StartTime = dtStart.ToString("HHmm"),
                //        EndDate = dtEnd.ToString("yyyyMMdd"),
                //        EndTime = dtEnd.ToString("HHmm"),
                //        CalcType = krblCHANGETYPE.SelectedValue
                //    }),
                //out ex);
                //DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.BOExecFunc.Parameter:{0}", new
                //{
                //    TMP_EmployeeID = hidAPPLICANT.Value,
                //    StartDate = dtStart.ToString("yyyyMMdd"),
                //    StartTime = dtStart.ToString("HHmm"),
                //    EndDate = dtEnd.ToString("yyyyMMdd"),
                //    EndTime = dtEnd.ToString("HHmm"),
                //    CalcType = krblCHANGETYPE.SelectedValue
                //}.ToString()));

                //if (ex != null)
                //    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.GetOverTimeHours.ERROR:{0}", ex.Message));
                //if (parameters != null &&
                //    parameters.Length > 0)
                //{
                //    if (parameters[0].DataType.ToString() == "DataTable")
                //    {
                //        dtCalcResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                //    }
                //}

                //DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.BOExecFunc:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtCalcResult)));
                //if (dtCalcResult != null &&
                //    dtCalcResult.Rows.Count > 0)
                //{
                //    // 填入申請時數、加班時數
                //    ktxtApplyHours.Text = dtCalcResult.Rows[0]["ApplyHours"].ToString();
                //    ktxtOverTimeHours.Text = dtCalcResult.Rows[0]["OverTimeHours"].ToString();
                //    hidOT_CLASSTYPE.Value = dtCalcResult.Rows[0]["TMP_WorkID"].ToString();
                //    ktxtOT_CLASSTYPE_NAME.Text = dtCalcResult.Rows[0]["TMP_WorkName"].ToString();

                //    ktxtOverCompHours.Text = dtCalcResult.Columns.Contains("OverCompHours") ? dtCalcResult.Rows[0]["OverCompHours"].ToString() : ""; // 實際補休時數
                //    ktxtMFreeDate.Text = dtCalcResult.Columns.Contains("MFreeDate") ? dtCalcResult.Rows[0]["MFreeDate"].ToString() : ""; // 補休可休期限

                //    string WorkDate = dtCalcResult.Columns.Contains("WorkDate") ? dtCalcResult.Rows[0]["WorkDate"].ToString() : ""; // 首筆刷卡日期
                //    string WorkTime = dtCalcResult.Columns.Contains("WorkTime") ? dtCalcResult.Rows[0]["WorkTime"].ToString() : ""; //首筆刷卡時間
                //    if (WorkDate.Length > 10)
                //        ktxtWorkPunch.Text = string.Format(@"{0} {1}", WorkDate.Substring(0, 10), WorkTime.Insert(2, ":")); // 組合出時間

                //    string OffWorkDate = dtCalcResult.Columns.Contains("OffWorkDate") ? dtCalcResult.Rows[0]["OffWorkDate"].ToString() : ""; // 尾筆刷卡日期
                //    string OffWorkTime = dtCalcResult.Columns.Contains("OffWorkTime") ? dtCalcResult.Rows[0]["OffWorkTime"].ToString() : ""; // 尾筆刷卡時間
                //    if (OffWorkDate.Length > 10)
                //        ktxtOffWorkPunch.Text = string.Format(@"{0} {1}", OffWorkDate.Substring(0, 10), OffWorkTime.Insert(2, ":")); // 組合出時間

                //    hidDoOverType.Value = dtCalcResult.Columns.Contains("DoOverType") ? dtCalcResult.Rows[0]["DoOverType"].ToString() : ""; // 加班別
                //    hidAllowCalcType.Value = dtCalcResult.Columns.Contains("AllowCalcType") ? dtCalcResult.Rows[0]["AllowCalcType"].ToString() : ""; // 加班給付方式允許類型
                //    ktxtDoOverType.Text = hidDoOverType.Value == "0" ? "平日" : hidDoOverType.Value == "1" ? "休息日" : hidDoOverType.Value == "2" ? "特殊日" : hidDoOverType.Value == "3" ? "例假日" : hidDoOverType.Value == "4" ? "變形休息日" : hidDoOverType.Value == "5" ? "國定假日" : "";
                //}
                //else
                //{
                //    lblOT_TIMESMSG.Text = "該會員無法申請加班，因為沒有班表資料";
                //}

                //if (!string.IsNullOrEmpty(hidOT_CLASSTYPE.Value))
                //{
                //    DateTime dtStartPunch = DateTime.MinValue;
                //    DateTime.TryParse(ktxtWorkPunch.Text.Trim(), out dtStartPunch);
                //    DateTime dtOffPunch = DateTime.MinValue;
                //    DateTime.TryParse(ktxtOffWorkPunch.Text.Trim(), out dtOffPunch);
                //    if (SCSHRConfiguration.IS_OV_CHK_PUNCH.ToUpper() == "Y")
                //    {
                //        // 檢查刷卡時間合理性
                //        if (DateTime.Compare(dtStartPunch, dtStart) == 1)
                //        {
                //            lblOT_TIMESMSG.Text = "加班時間(起)不可小於首筆刷卡時間";
                //        }
                //        if (DateTime.Compare(dtEnd, dtOffPunch) == 1)
                //        {
                //            lblOT_TIMESMSG.Text = "加班時間(迄)不可大於尾筆刷卡時間";
                //        }
                //    }

                //    if (string.IsNullOrEmpty(lblOT_TIMESMSG.Text))
                //    {
                //        bool resultStatus = false;
                //        // 檢核資料
                //        JArray jaTable = new JArray();
                //        JObject _joTable = new JObject();
                //        _joTable.Add(new JProperty("USERNO", "1"));
                //        _joTable.Add(new JProperty("SYS_VIEWID", ""));
                //        _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(hidAPPLICANTDATE.Value).ToString("yyyyMMdd"))); // 日期
                //        _joTable.Add(new JProperty("TMP_EMPLOYEEID", hidAPPLICANT.Value)); // 加班人員
                //        _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByAccount(hidAPPLICANT.Value).EmployeeNo)); // 加班人員
                //        _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd"))); // 起始日期
                //        _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm"))); // 起始時間
                //        _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm"))); // 結束時間
                //        _joTable.Add(new JProperty("DoOverType", hidDoOverType.Value)); // 加班別
                //        _joTable.Add(new JProperty("WorkDate", dtStartPunch.ToString("yyyyMMdd"))); // 首筆刷卡日期
                //        _joTable.Add(new JProperty("WorkTime", dtStartPunch.ToString("HHmm"))); // 首筆刷卡時間
                //        _joTable.Add(new JProperty("OffWorkDate", dtOffPunch.ToString("yyyyMMdd"))); // 尾筆刷卡日期
                //        _joTable.Add(new JProperty("OffWorkTime", dtOffPunch.ToString("HHmm"))); // 尾筆刷卡時間
                //        _joTable.Add(new JProperty("MFreeDate", ktxtMFreeDate.Text)); // 計算後補休可休期限
                //        _joTable.Add(new JProperty("OverCompHours", ktxtOverCompHours.Text)); // 計算後的實際補休時數
                //        _joTable.Add(new JProperty("TMP_ATTREASONID", kddlREMARK.SelectedValue)); // 加班原因
                //        _joTable.Add(new JProperty("TMP_WORKID", hidOT_CLASSTYPE.Value)); // 加班班別
                //                                                                          //_joTable.Add(new JProperty("RESTMINS", "0")); // 休息時間(分鐘)
                //        _joTable.Add(new JProperty("CALCTYPE", krblCHANGETYPE.SelectedValue)); // 給付方式
                //        _joTable.Add(new JProperty("NOTE", ktxtNOTE.Text)); // 加班內容說明
                //        jaTable.Add(_joTable);
                //        DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                //        dtSource.TableName = SCSHRConfiguration.SCSSOverTimeProgID;
                //        DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSOverTimeProgID);
                //        dsSource.Tables.Add(dtSource);
                //        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.dtSource:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtSource)));
                //        Exception oex = null;
                //        DataTable dtResult = service.BOImport(SCSHRConfiguration.SCSSOverTimeProgID, false, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out oex);
                //        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH.btnCal_Click.Result:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
                //        if (oex != null)
                //        {
                //            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME_BATCH::btnCal_Click::BOImportWS::StackTrace::{0}::ERROR::{1}", oex.StackTrace, oex.Message));
                //            hidAPIResult.Value = "ERROR";
                //            lblOT_TIMESMSG.Text = "檢查失敗";
                //        }
                //        else
                //        {
                //            if (dtResult != null &&
                //                dtResult.Rows.Count > 0)
                //            {
                //                resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
                //                if (!resultStatus)
                //                    lblOT_TIMESMSG.Text = dtResult.Rows[0]["_MESSAGE"].ToString();
                //            }

                //        }
                //        if (resultStatus)
                //        {
                //            hidAPIResult.Value = "OK";
                //            lblOT_TIMESMSG.Text = "檢查成功";
                //            if (canInsToOTList())
                //            {
                //                gvOVs.DataSource = insertToOTList();
                //                gvOVs.DataBind();
                //            }
                //            else
                //            {
                //                lblOT_TIMESMSG.Text = "該會員無法重複申請加班";
                //            }
                //        }
                //    }
                //    else
                //    {
                //        // 刷卡時間有問題
                //    }
                //}

                #endregion 原版-只檢查一人
            }
            else
            {
                lblOT_TIMESMSG.Text = "加班時間(起)不可大於等於加班時間(迄)";
            }

            //if (canInsToOTList())
            //{
            //    gvOVs.DataSource = insertToOTList();
            //    gvOVs.DataBind();
            //}
        }
        else // 沒選加班時間、部門或人員
        {
            string msg = "";
            if (string.IsNullOrEmpty(hidAPPLICANTDEPT.Value))
                msg += "沒有選擇部門<br/>";
            if (string.IsNullOrEmpty(hidAPPLICANTGuid.Value))
                msg += "沒有選擇人員<br/>";
            //if (string.IsNullOrEmpty(kddlREMARK.SelectedValue))
            //    msg += "沒有選擇加班原因<br/>";
            if (string.IsNullOrEmpty(kdtpOT_START.Text))
                msg += "沒有選擇加班時間(起)<br/>";
            if (string.IsNullOrEmpty(kdtpOT_END.Text))
                msg += "沒有選擇加班時間(迄)<br/>";
            if (krblCHANGETYPE.SelectedIndex == -1)
                msg += "沒有選擇給付方式<br/>";


            lblOT_TIMESMSG.Text = msg.Substring(0, msg.Length - 5);
        }
    }


    protected void rbtnFindOVERTIME_Click(object sender, EventArgs e)
    {
        // TODO 這要做甚麼?
    }

    protected void kdtpOT_START_TextChanged(object sender, EventArgs e)
    {
        KYTDateTimePicker _kdtpOT_START = (KYTDateTimePicker)sender;
        //if (string.IsNullOrEmpty(kdtpOT_END.Text))
        //    kdtpOT_END.Text = _kdtpOT_START.Text;
        if (SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE.ToString().ToUpper() == "Y")
        {
            DateTime dtPreSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(ViewState["STARTTIME"] != null ? ViewState["STARTTIME"].ToString() : "", out dtPreSTARTTIME); // 如果沒有紀錄上次的時間，就記錄成時間最小值
            DateTime dtSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(_kdtpOT_START.Text, out dtSTARTTIME);
            if (dtPreSTARTTIME.Date != dtSTARTTIME.Date) // 如果上次的日期不等於現在的日期，更動迄止時間
            {
                DateTime dtENDTIME = DateTime.MinValue;
                DateTime.TryParse(kdtpOT_END.Text, out dtENDTIME);
                kdtpOT_END.Text = string.Format(@"{0} {1}", dtSTARTTIME.ToString("yyyy/MM/dd"), dtENDTIME.ToString("HH:mm")); // 只更動日期
                ViewState["STARTTIME"] = _kdtpOT_START.Text; // 紀錄這次的時間
            }
        }
        Dialog.Open2(rbtnFindOVERTIME,
            string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/findOVERTIME.aspx"),
            "已加班時數查詢 (顯示前後30天範圍內的所有加班單)",
            800,
            600,
            Dialog.PostBackType.AfterReturn,
            new { ACCOUNT = hidAPPLICANT.Value, START_DATE = kdtpOT_START.Text }.ToExpando());
    }

    /// <summary>
    /// 加班明細後端事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvOVs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        kytController.SetAllViewType(gvOVs, KYTViewType.ReadOnly);
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblOVs = gvOVs.DataTable; // 取出先前記住的 gvItems
            DataRow row = tblOVs.Rows[gr.DataItemIndex]; // 取出對映的資料列
            Button btnItemsDel = gr.FindControl("btnItemsDel") as Button; // 項次刪除按鈕
            HiddenField hidAPPLICANTDEPT = gr.FindControl("hidAPPLICANTDEPT") as HiddenField; // 部門Group_ID
            HiddenField hidAPPLICANTDATE = gr.FindControl("hidAPPLICANTDATE") as HiddenField; // 申請時間
            HiddenField hidCompanyNo = gr.FindControl("hidCompanyNo") as HiddenField; // 部門公司代碼
            HiddenField hidGROUPCODE = gr.FindControl("hidGROUPCODE") as HiddenField; // 部門代碼
            HiddenField hidAPPLICANTGuid = gr.FindControl("hidAPPLICANTGuid") as HiddenField; // 申請人GUID
            HiddenField hidAPPLICANT = gr.FindControl("hidAPPLICANT") as HiddenField; // 申請人ACCOUNT
            HiddenField hidCHANGETYPE = gr.FindControl("hidCHANGETYPE") as HiddenField; // 給付方式
            HiddenField hidOT_CLASSTYPE = gr.FindControl("hidOT_CLASSTYPE") as HiddenField; // 加班班別
            KYTTextBox ktxtNOTE = gr.FindControl("ktxtNOTE") as KYTTextBox; // 加班內容說明
            KYTTextBox ktxtOT_DOOVERTYPE = gr.FindControl("ktxtOT_DOOVERTYPE") as KYTTextBox; // 加班別
            KYTTextBox ktxtOT_DOOVERTYPE_NAME = gr.FindControl("ktxtOT_DOOVERTYPE_NAME") as KYTTextBox; // 加班別名稱
            KYTTextBox ktxtOT_WORKPUNCH = gr.FindControl("ktxtOT_WORKPUNCH") as KYTTextBox; // 首筆刷卡時間
            KYTTextBox ktxtOT_OFFWORKPUNCH = gr.FindControl("ktxtOT_OFFWORKPUNCH") as KYTTextBox; // 尾筆刷卡時間
            KYTTextBox ktxtOT_MFREEDATE = gr.FindControl("ktxtOT_MFREEDATE") as KYTTextBox; // 計算後補休可休期限
            KYTTextBox ktxtOT_OVERCOMPHOURS = gr.FindControl("ktxtOT_OVERCOMPHOURS") as KYTTextBox; // 計算後的實際補休時數
            //HiddenField hidREMARK = gr.FindControl("hidREMARK") as HiddenField; // 加班原因

            hidAPPLICANTDEPT.Value = row["GROUP_ID"].ToString(); // UOF部門代碼
            hidAPPLICANTDATE.Value = row["APPLICANTDATE"].ToString(); // 申請日期
            hidCompanyNo.Value = row["COMPANYNO"].ToString(); // 公司代碼
            hidGROUPCODE.Value = row["GROUP_CODE"].ToString(); // 部門代碼
            hidAPPLICANTGuid.Value = row["APPLICANT_GUID"].ToString(); // 申請人代碼
            hidAPPLICANT.Value = row["APPLICANT_ACCOUNT"].ToString(); // 申請人帳號
            hidCHANGETYPE.Value = row["CHANGETYPE"].ToString(); // 給付方式代碼
            hidOT_CLASSTYPE.Value = row["OT_CLASSTYPE"].ToString(); // 加班班別
            ktxtNOTE.Text = tblOVs.Columns.Contains("NOTE") ? row["NOTE"].ToString() : ""; // 加班內容說明
            ktxtOT_DOOVERTYPE.Text = tblOVs.Columns.Contains("OT_DOOVERTYPE") ? row["OT_DOOVERTYPE"].ToString() : ""; // 加班別
            ktxtOT_DOOVERTYPE_NAME.Text = tblOVs.Columns.Contains("OT_DOOVERTYPE_NAME") ? row["OT_DOOVERTYPE_NAME"].ToString() : ""; // 加班別名稱
            ktxtOT_WORKPUNCH.Text = tblOVs.Columns.Contains("OT_WORKPUNCH") ? row["OT_WORKPUNCH"].ToString() : ""; // 首筆刷卡時間
            ktxtOT_OFFWORKPUNCH.Text = tblOVs.Columns.Contains("OT_OFFWORKPUNCH") ? row["OT_OFFWORKPUNCH"].ToString() : ""; // 尾筆刷卡時間
            ktxtOT_MFREEDATE.Text = tblOVs.Columns.Contains("OT_MFREEDATE") ? row["OT_MFREEDATE"].ToString() : ""; // 計算後補休可休期限
            ktxtOT_OVERCOMPHOURS.Text = tblOVs.Columns.Contains("OT_OVERCOMPHOURS") ? row["OT_OVERCOMPHOURS"].ToString() : ""; // 計算後的實際補休時數
            //hidREMARK.Value = row["REMARK"].ToString(); // 加班原因
            if (this.FormFieldMode == FieldMode.Applicant ||
                this.FormFieldMode == FieldMode.ReturnApplicant) // 如果是剛起單
            {
                btnItemsDel.Visible = true;
            }
            else
            {
                btnItemsDel.Visible = false;
            }
        }
    }
    /// <summary>
    /// 加班明細刪除按鈕觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnItemsDel_Click(object sender, EventArgs e)
    {
        Button _btnItemsDel = (Button)sender;
        GridViewRow gr = _btnItemsDel.NamingContainer as GridViewRow;
        DataTable tblOVs = gvOVs.DataTable;
        tblOVs.Rows.RemoveAt(gr.RowIndex);

        gvOVs.DataSource = tblOVs;
        gvOVs.DataBind();
    }

    /// <summary>
    /// 點擊UOF選人元件(部門)返回事件
    /// </summary>
    /// <param name="choiceResult"></param>
    protected void btncoGroup_EditButtonOnClick(string[] choiceResult)
    {
        //        choiceResult
        //{ string[3]}
        //    [0]: "1698c3f7-647c-4758-81e8-75afdb5ce98d"
        //    [1]: "A100稽核管理室      "
        //    [2]: "False"
        if (choiceResult.Length > 0)
        {
            ktxtAPPLICANTDEPT.Text = choiceResult[1]; // 部門名稱
            hidAPPLICANTDEPTName.Value = choiceResult[1]; // 部門名稱
            hidAPPLICANTDEPT.Value = choiceResult[0]; // UOF部門代號
            hidGROUPCODE.Value = JGlobalLibs.UOFUtils.GetGroupCodeByDepartmentID(hidAPPLICANTDEPT.Value); // 部門代號
            // 清空申請人
            ktxtAPPLICANT.Text = ""; // 申請人名稱
            hidAPPLICANTGuid.Value = ""; // 申請人代號
            hidAPPLICANT.Value = ""; // 申請人帳號
            // 綁定選人
            btnclEmployee.LimitXML = reBindChoiceEmployee(hidAPPLICANTDEPT.Value); // 綁定選擇同部門人員
        }

    }

    /// <summary>
    /// 點擊UOF選人元件(人員)返回事件
    /// </summary>
    /// <param name="userSet"></param>
    protected void btnclEmployee_EditButtonOnClick(UserSet userSet)
    {
        /**
       userSet.Items.GetUserAaary()[0]
        {Ede.Uof.EIP.Organization.Util.UserSetUser}
            EBUsers: {Ede.Uof.EIP.Organization.Util.UserSetEBUsersCollection}
            Key: "admin"
            Name: "系統管理員(admin)"
            Type: User
            USER_GUID: "admin"
            WKFSignerInfoList: Count = 0
         */
        if (userSet.Items.Count > 0)
        {
            UserSetUser usu = userSet.Items.GetUserAaary()[0];
            ktxtAPPLICANT.Text = usu.Name; // 申請人名稱
            hidAPPLICANTGuid.Value = usu.USER_GUID; // 申請人代號
            hidAPPLICANT.Value = JGlobalLibs.UOFUtils.getUserAccount(usu.USER_GUID); // 申請人帳號
        }
    }

    /// <summary>
    /// 選人按鈕觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEmployee_Click(object sender, EventArgs e)
    {
        btnclEmployee.UserSet = new UserSet();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), @"DelayStartChoiceEmployee();", true);
    }

    /// <summary>
    /// 申請人_選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    protected void btnAPPLICANT_DialogReturn(object sender, string result)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("APPLICANT_ACCOUNT", typeof(string))); // 加班人帳號
        dt.Columns.Add(new DataColumn("APPLICANT_GUID", typeof(string))); // 加班人GUID
        dt.Columns.Add(new DataColumn("APPLICANT_NAME", typeof(string))); // 加班人姓名
        dt.Columns.Add(new DataColumn("GROUP_CODE", typeof(string))); // 加班人部門CODE
        dt.Columns.Add(new DataColumn("DEPT_NAME", typeof(string))); // 加班人部門名稱
        dt.Columns.Add(new DataColumn("GROUP_ID", typeof(string))); // 加班人GROUP_ID
        dt.Columns.Add(new DataColumn("COMPANYNO", typeof(string))); // 加班人公司代碼

        dt.Columns.Add(new DataColumn("OT_CLASSTYPE_NAME", typeof(string))); // 加班班別名稱
        dt.Columns.Add(new DataColumn("OT_CLASSTYPE", typeof(string))); // 加班班別
        dt.Columns.Add(new DataColumn("APPLYHOURS", typeof(string))); // 申請時數
        dt.Columns.Add(new DataColumn("OVERTIMEHOURS", typeof(string))); // 加班時數
        dt.Columns.Add(new DataColumn("OT_DOOVERTYPE", typeof(string))); // 加班別
        dt.Columns.Add(new DataColumn("OT_DOOVERTYPE_NAME", typeof(string))); // 加班別名稱
        dt.Columns.Add(new DataColumn("OT_WORKPUNCH", typeof(string))); // 首筆刷卡時間
        dt.Columns.Add(new DataColumn("OT_OFFWORKPUNCH", typeof(string))); // 尾筆刷卡時間
        dt.Columns.Add(new DataColumn("OT_MFREEDATE", typeof(string))); // 計算後補休可休期限
        dt.Columns.Add(new DataColumn("OT_OVERCOMPHOURS", typeof(string))); // 計算後的實際補休時數

        JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
        if (jArray.Count > 0)
        {
            //EBUser userFirst = new UserUCO().GetEBUser(jArray[0]["GUID"].ToString());
            //ktxtAPPLICANT.Text = userFirst.Name; // 申請人名稱
            //hidAPPLICANTGuid.Value = userFirst.UserGUID; // 申請人代號
            //hidAPPLICANT.Value = userFirst.Account; // 申請人帳號
            //string[] sAccount_First = hidAPPLICANT.Value.Split('\\');
            //hidAPPLICANT.Value = sAccount_First[sAccount_First.Length - 1]; // 申請人帳號

            //ktxtAPPLICANTDEPT.Text = userFirst.GroupName; // 部門名稱
            //hidAPPLICANTDEPTName.Value = userFirst.GroupName; // 部門名稱
            //hidAPPLICANTDEPT.Value = userFirst.GroupID; // UOF部門代號
            //hidGROUPCODE.Value = JGlobalLibs.UOFUtils.GetGroupCodeByDepartmentID(hidAPPLICANTDEPT.Value); // 部門代號
            //hidCompanyNo.Value = userFirst.CompanyNo;

            //// 重新綁訂"加班狀況"按鈕跳窗
            //Dialog.Open2(rbtnFindOVERTIME,
            //    string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/findOVERTIME.aspx"),
            //    "已加班時數查詢 (顯示前後30天範圍內的所有加班單)",
            //    800,
            //    600,
            //    Dialog.PostBackType.AfterReturn,
            //    new { ACCOUNT = hidAPPLICANT.Value, START_DATE = kdtpOT_START.Text }.ToExpando());

            string APPLICANT_ALL = "";
            // 選擇多人加班人
            foreach (JToken Jk in jArray)
            {
                //EBUser KUser = new UserUCO().GetEBUser(Jk["GUID"].ToString());
                KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(Jk["GUID"].ToString()); // 人員
                string[] sAccount = KUser.Account.Split('\\');

                DataRow ndr = dt.NewRow();
                ndr["APPLICANT_ACCOUNT"] = sAccount[sAccount.Length - 1];
                ndr["APPLICANT_GUID"] = KUser.UserGUID;
                ndr["APPLICANT_NAME"] = KUser.Name;
                ndr["GROUP_CODE"] = KUser.GroupCode[0];
                ndr["DEPT_NAME"] = KUser.GroupName[0];
                ndr["GROUP_ID"] = KUser.GroupID[0];
                ndr["COMPANYNO"] = KUser.CompanyNo;
                dt.Rows.Add(ndr);
                APPLICANT_ALL += string.Format(@"{0} ({1}){2}", KUser.Name, sAccount[sAccount.Length - 1], Jk == jArray.Last ? "" : ",");  // 設定加班人資訊

                ktxtAPPLICANT.Text = KUser.Name; // 申請人名稱
                hidAPPLICANTGuid.Value = KUser.UserGUID; // 申請人代號
                hidAPPLICANT.Value = KUser.Account; // 申請人帳號
                string[] sAccount_First = hidAPPLICANT.Value.Split('\\');
                hidAPPLICANT.Value = sAccount_First[sAccount_First.Length - 1]; // 申請人帳號

                ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 部門名稱
                hidAPPLICANTDEPTName.Value = KUser.GroupName[0]; // 部門名稱
                hidAPPLICANTDEPT.Value = KUser.GroupID[0]; // UOF部門代號
                hidGROUPCODE.Value = KUser.GroupCode[0]; // 部門代號
                hidCompanyNo.Value = KUser.CompanyNo;

                // 重新綁訂"加班狀況"按鈕跳窗
                Dialog.Open2(rbtnFindOVERTIME,
                    string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/findOVERTIME.aspx"),
                    "已加班時數查詢 (顯示前後30天範圍內的所有加班單)",
                    800,
                    600,
                    Dialog.PostBackType.AfterReturn,
                    new { ACCOUNT = hidAPPLICANT.Value, START_DATE = kdtpOT_START.Text }.ToExpando());
            }
            ktxtAPPLICANT.Text = APPLICANT_ALL;
            ViewState["APPLICANT"] = dt;
        }
        else
        {
            ktxtAPPLICANT.Text = ""; // 申請人名稱
            hidAPPLICANTGuid.Value = ""; // 申請人代號
            hidAPPLICANT.Value = ""; // 申請人帳號

            ktxtAPPLICANTDEPT.Text = ""; // 部門名稱
            hidAPPLICANTDEPTName.Value = ""; // 部門名稱
            hidAPPLICANTDEPT.Value = ""; // UOF部門代號
            hidGROUPCODE.Value = ""; // 部門代號
        }
    }
}
