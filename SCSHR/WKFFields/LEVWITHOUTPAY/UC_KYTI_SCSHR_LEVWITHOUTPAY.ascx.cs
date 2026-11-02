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
using System.Web;
using System.Web.Services;
using System.Web.UI;
using UOFAssist.WKF;


/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
* 修改原因：
    * 1.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
* 修改位置： 
    * 1.「SetField()」中，註解所有EBUser，改為KYT_EBUser
* **/

/**
* 修改時間：2020/04/16
* 修改人員：陳緯榕
* 修改項目：
    * 新規格：標題修改
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈SCSHR留停資訊〉改為〈留停資訊〉
* **/

/**
* 修改時間：2020/03/13
* 修改人員：陳緯榕
* 修改項目：
    * kxtMessage顯示需為紅色
* 發生原因：
    * ，紅色是屬於警告意味強的顏色，故使用紅色顯示錯誤
* 修改位置：
    * 「前端網頁」中，新增〈CSS：msgColor〉；並將〈ktxtMessage〉的屬性〈LabelCssClass〉設為〈msgColor〉
* **/

/**
* 修改時間：2019/07/17
* 修改人員：陳緯榕
* 修改項目：
    * KYTDatePicker在dll.Config設定的IS_PICKER_READONLY設定無效
* 發生原因：
    * KYTPicker要設定TextBoxReadOnly的特性是ViewType.Input，但原本的位置狀態會是ViewType.ReadOnly
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack(開啟表單)〉而且〈fieldOptional.FieldMode〉是〈Applicant〉或〈ReturnApplicant〉時，程式片段的最後設定〈TextBoxReadOnly〉屬性
    * 「SetField」中，當〈Page.IsPostBack(操作表單)〉而且〈fieldOptional.FieldMode〉是〈Applicant〉或〈ReturnApplicant〉時，設定〈ViewType = Input〉和〈TextBoxReadOnly〉屬性
* **/

/**
* 修改時間：2019/07/03
* 修改人員：陳緯榕
* 修改項目：
    * 最後計薪日要減一天
* 修改位置：
    * 「kdpPESTIMATERDATE_TextChanged」中，〈kdpLSPAYDATE〉的日期要少一天
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
* 修改時間：2019/04/09
* 修改人員：陳緯榕
* 修改項目：
    * DatePicker的輸入框是否能輸入由dll.config控制
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack〉時，〈kdpESTIMATEBDATE〉、〈kdpLSPAYDATE〉、〈kdpPESTIMATERDATE〉和〈kdpPSTARTDATE〉的屬性〈TextBoxReadOnly〉由〈SCSHRConfiguration.IS_PICKER_READONLY〉控制
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
* 修改時間：2018/11/15
* 修改人員：陳緯榕
* 修改項目：
    *  起單關沒有填寫的欄位，WS卻是必填
* 修正位置：
    * 「kdpPESTIMATERDATE_TextChanged」將〈預計留停日〉的值填入〈預計復職日〉、〈最後計薪日〉、〈留停起算日〉
* **/

/// <summary>
/// 飛騰留停資訊
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_LEVWITHOUTPAY : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEVWITHOUTPAY.RealValue.cv:{0}", cv));

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
            return string.Format(@"{0}, 預計留停日:{1}",
                ktxtRESIGNEMP.Text,
                kdpPESTIMATERDATE.Text);
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
            return string.Format(@"{0}, 預計留停日:{1}",
                ktxtRESIGNEMP.Text,
                kdpPESTIMATERDATE.Text);
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
            // string rv = new JGlobalLibs.UOFUtils.RealValue()
            //     .AddPerson("RESIGNAgent", hidRESIGNAGENT.Value)
            //     .ToString();
            // DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEVWITHOUTPAY.RealValue.rv:{0}", rv));
            // return rv;
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

                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                        ktxtMessage.ViewType = KYTViewType.ReadOnly;
                        ktxtAPPLICANTDATE.ReadOnly = true; // 申請日期唯讀
                        ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                        ktxtRESIGNEMP.ReadOnly = true; // 離職人員唯讀
                        kdpESTIMATEBDATE.ViewType = KYTViewType.Input; // 預計復職日
                        kdpLSPAYDATE.ViewType = KYTViewType.ReadOnly; // 最後計薪日
                        kdpPSTARTDATE.ViewType = KYTViewType.ReadOnly; // 留停起算日
                        ktxtCHECKDOCUMENT.ViewType = KYTViewType.ReadOnly; // 檢核文件
                        kddlLPREPAYSTATUS.ViewType = KYTViewType.ReadOnly; // 勞保費預繳狀態
                        ktxtLPREPAYMONTH.ViewType = KYTViewType.ReadOnly; // 勞保費預繳月份數
                        ktxtLPREPAYMONEY.ViewType = KYTViewType.ReadOnly; // 勞保費預繳金額
                        kddlHPREPAYSTATUS.ViewType = KYTViewType.ReadOnly; // 健保費預繳狀態
                        ktxtHPREPAYMONTH.ViewType = KYTViewType.ReadOnly; // 健保費預繳月份數
                        ktxtHPREPAYMONEY.ViewType = KYTViewType.ReadOnly; // 健保費預繳金額

                        if (this.FormFieldMode == FieldMode.Applicant)
                        {
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            ktxtAPPLICANTDATE.Text = DateTime.Now.ToString("yyyy/MM/dd"); // 設定申請日期
                            ktxtRESIGNEMP.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account); // 設定請 離職人
                            hidRESIGNEMPGuid.Value = KUser.UserGUID;
                            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 設定部門資訊
                            hidAPPLICANTDEPT.Value = KUser.GroupID[0];
                            hidRESIGNEMPTitleId.Value = KUser.Title_Name[0];
                            hidRESIGNEMPTitleName.Value = KUser.Title_ID[0];
                            hidRESIGNEMPAccount.Value = KUser.Account;
                            string[] sAccount = hidRESIGNEMPAccount.Value.Split('\\');
                            hidRESIGNEMPAccount.Value = sAccount[sAccount.Length - 1];
                            hidGROUPCODE.Value = KUser.GroupCode[0];

                            DataTable dtRESIGNSource = getRESIGNVEType(SCSHRConfiguration.SCSSReasonProdID, "SYS_ViewID,SYS_Name,FLAG");
                            // 新增請選擇選項
                            DataRow ndr = dtRESIGNSource.NewRow();
                            ndr["SYS_VIEWID"] = "";
                            ndr["SYS_NAME"] = "===請選擇===";
                            dtRESIGNSource.Rows.InsertAt(ndr, 0);
                            // 綁定離職原因
                            kddlREASONID.DataSource = dtRESIGNSource;
                            kddlREASONID.DataValueField = "SYS_VIEWID";
                            kddlREASONID.DataTextField = "SYS_NAME";
                            kddlREASONID.DataBind();
                        }
                        // 設定Picker是否能輸入
                        kdpESTIMATEBDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdpLSPAYDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdpPESTIMATERDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdpPSTARTDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Print: // 表單列印
                        break;
                    case FieldMode.Signin: // 表單簽核
                        kdpESTIMATEBDATE.ViewType = KYTViewType.Input; // 預計復職日
                        kdpLSPAYDATE.ViewType = KYTViewType.Input; // 最後計薪日
                        kdpPSTARTDATE.ViewType = KYTViewType.Input; // 留停起算日
                        ktxtCHECKDOCUMENT.ViewType = KYTViewType.Input; // 檢核文件
                        kddlLPREPAYSTATUS.ViewType = KYTViewType.Input; // 勞保費預繳狀態
                        ktxtLPREPAYMONTH.ViewType = KYTViewType.Input; // 勞保費預繳月份數
                        ktxtLPREPAYMONEY.ViewType = KYTViewType.Input; // 勞保費預繳金額
                        kddlHPREPAYSTATUS.ViewType = KYTViewType.Input; // 健保費預繳狀態
                        ktxtHPREPAYMONTH.ViewType = KYTViewType.Input; // 健保費預繳月份數
                        ktxtHPREPAYMONEY.ViewType = KYTViewType.Input; // 健保費預繳金額
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.View: // 表單觀看
                        break;
                }
            }
            else // 如果網頁POSTBACK
            {
                JGlobalLibs.WebUtils.RequestHiddenFields(UpdatePanel1); // 取回HiddenField的值
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                    case FieldMode.Signin: // 表單簽核
                        // 設定Picker是否能輸入
                        kdpESTIMATEBDATE.ViewType = KYTViewType.Input; // 預計復職日
                        kdpLSPAYDATE.ViewType = KYTViewType.Input; // 最後計薪日
                        kdpPSTARTDATE.ViewType = KYTViewType.Input; // 留停起算日
                        if (fieldOptional.FieldMode != FieldMode.Signin)
                            kdpPESTIMATERDATE.ViewType = KYTViewType.Input; // 
                        kdpESTIMATEBDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdpLSPAYDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdpPESTIMATERDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdpPSTARTDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
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
    /// 取得離職原因
    /// </summary>
    /// <param name="progId"></param>
    /// <param name="selectFields"></param>
    /// <returns></returns>
    private DataTable getRESIGNVEType(string progId, string selectFields)
    {
        DataTable dtSource = new DataTable();
        Exception ex = null;
        dtSource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEVWITHOUTPAY.getRESIGNVEType.service.Error:{0}", ex.Message));
        DataTable dtReturn = new DataTable();
        foreach (DataColumn dc in dtSource.Columns)
        {
            dtReturn.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
        }
        foreach (DataRow dr in dtSource.Rows)
        {
            if (dr["FLAG"].ToString() == "2")
            {
                DataRow ndr = dtReturn.NewRow();
                foreach (DataColumn dc in dtSource.Columns)
                {
                    ndr[dc.ColumnName] = dr[dc.ColumnName];
                }
                dtReturn.Rows.Add(ndr);
            }
        }
        return dtReturn;
    }


    /// <summary>
    /// 預計留停日變更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kdpPESTIMATERDATE_TextChanged(object sender, EventArgs e)
    {
        KYTDatePicker _kdpPESTIMATERDATE = (KYTDatePicker)sender;
        kdpESTIMATEBDATE.Text = _kdpPESTIMATERDATE.Text; // 預計復職日
        kdpLSPAYDATE.Text = DateTime.Parse(_kdpPESTIMATERDATE.Text).AddDays(-1).ToString("yyyy/MM/dd"); // 最後計薪日
        kdpPSTARTDATE.Text = _kdpPESTIMATERDATE.Text; // 留停起算日
    }
}
