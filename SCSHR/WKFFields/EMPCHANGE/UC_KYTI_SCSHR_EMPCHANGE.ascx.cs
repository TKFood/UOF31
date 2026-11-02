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
    * 「前端網頁」中，〈SCSHR職務異動資訊〉改為〈職務異動資訊〉
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
* 修改時間：2019/07/04
* 修改人員：陳緯榕
* 修改項目：
    * 結束日期預設帶生效日期(選了生效日期時一併帶入結束日期)
    * 利潤中心 在草稿起單時沒有檢查到必填 
* 發生原因：
    * ************
    * 利潤中心取的是applicantDept，而下拉選單沒有這個value，所以才有這樣的誤差
* 修改位置：
    * 「前端網頁」中，〈KYTDatePicker-kdpSTARTDATE〉新增屬性〈AutoPostBack="true"〉、〈OnTextChanged="kdpSTARTDATE_TextChanged" 〉
    * 新增方法「kdpSTARTDATE_TextChanged(object sender, EventArgs e)」；將〈結束日期預設帶生效日期〉
    * ***********
    * 「SetField」中，〈hidOPROFITID〉填入〈TB_EB_EMPL_HR-BU〉，〈kddlPROFITNAME.SelectedValue〉給予〈hidOPROFITID〉，最後〈ktxtOPROFITNAME〉填入〈kddlPROFITNAME.SelectedItem.Text〉
* **/

/**
* 修改時間：2019/07/03
* 修改人員：陳緯榕
* 修改項目：
    * Trigger錯誤：指定的轉換無效
    * 觀看時按鈕不應該生效
* 發生原因：
    * 沒有填寫結束日期造成的
    * *******
* 修改位置：
    * 「前端網頁」中，〈checkENDDATE〉註解去除
    * 「前端網頁」中，〈CustomValidator3〉註解去除
    * **********
    * 「SetField」中，當開啟表單時，〈btnDEPARTID-帳部部門〉、〈btnPROFITID-利潤中心〉的屬性〈Visible〉要為〈false〉，起單時屬性為〈true〉
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
* 修改時間：2019/04/11
* 修改人員：陳緯榕
* 修改項目：
    * DateTimePicker的輸入框是否能輸入由dll.config控制
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack〉時，〈kdpSTARTDATE〉和〈kdpENDDATE〉的屬性〈TextBoxReadOnly〉由〈SCSHRConfiguration.IS_PICKER_READONLY〉控制
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
* 修改時間：2018/12/20
* 修改人員：陳緯榕
* 修改項目：
    * 結束日期非必填
* 修改位置：
    * 「前端網頁」〈checkENDDATE〉註解
    * 「前端網頁」〈CustomValidator3〉註解
* **/

/// <summary>
/// 飛騰職務異動資訊
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_EMPCHANGE : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_EMPCHANGE.RealValue.cv:{0}", cv));

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
            return string.Format(@"{0}, 異動原因:{1}",
                ktxtEMPLOYEE.Text,
                kddlREASONID.SelectedItem.Text);
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
            return string.Format(@"{0}, 異動原因:{1}",
                ktxtEMPLOYEE.Text,
                kddlREASONID.SelectedItem.Text);
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
            // DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_EMPCHANGE.RealValue.rv:{0}", rv));
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
                btnDEPARTID.Visible = false; // 帳部部門
                btnPROFITID.Visible = false; // 利潤中心

                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                        ktxtMessage.ViewType = KYTViewType.ReadOnly;
                        ktxtAPPLICANTDATE.ReadOnly = true; // 申請日期唯讀
                        ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                        ktxtEMPLOYEE.ReadOnly = true; // 異動人員唯讀
                        btnDEPARTID.Visible = true; // 帳部部門
                        btnPROFITID.Visible = true; // 利潤中心
                        if (this.FormFieldMode == FieldMode.Applicant)
                        {
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid); // 取得起單人資料
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            ktxtAPPLICANTDATE.Text = DateTime.Now.ToString("yyyy/MM/dd"); // 設定申請日期
                            ktxtEMPLOYEE.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account); // 設定異動人員名稱
                            hidEMPLOYEEGuid.Value = KUser.UserGUID; // 設定異動人員代號
                            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 設定異動人員部門名稱
                            hidAPPLICANTDEPT.Value = KUser.GroupID[0]; // 設定異動人員部門代號(UOF)
                            hidEMPLOYEETitleId.Value = KUser.Title_ID[0]; // 設定異動人員職稱代號
                            hidEMPLOYEETitleName.Value = KUser.Title_Name[0]; // 設定異動人員職稱
                            hidEMPLOYEEAccount.Value = KUser.Account; // 設定異動人員帳號
                            string[] sAccount = hidEMPLOYEEAccount.Value.Split('\\');
                            hidEMPLOYEEAccount.Value = sAccount[sAccount.Length - 1];
                            hidGROUPCODE.Value = KUser.GroupCode[0]; // 設定異動人員部門代號

                            #region 職務異動原因

                            DataTable dtEMPCHANGESource = getEMPCHANGEType(SCSHRConfiguration.SCSSReasonProdID, "SYS_ViewID,SYS_Name,FLAG");
                            // 新增請選擇選項
                            DataRow ndr = dtEMPCHANGESource.NewRow();
                            ndr["SYS_VIEWID"] = "";
                            ndr["SYS_NAME"] = "===請選擇===";
                            dtEMPCHANGESource.Rows.InsertAt(ndr, 0);

                            kddlREASONID.DataSource = dtEMPCHANGESource;
                            kddlREASONID.DataTextField = "SYS_NAME";
                            kddlREASONID.DataValueField = "SYS_VIEWID";
                            kddlREASONID.DataBind();

                            #endregion 職務異動原因

                            #region 異動前職務資訊

                            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                                -- 查詢申請者個人資訊
                                SELECT COMPANY_NO, OPTION2, OPTION3, OPTION4 
                                  FROM TB_EB_USER 
                                 WHERE USER_GUID = @USER_GUID

                                 -- 查詢申請者職稱
                                SELECT TITLE_NAME, TITLE_ID 
                                  FROM TB_EB_JOB_TITLE 
                                 WHERE TITLE_ID =(SELECT TOP 1 TITLE_ID 
					                                FROM TB_EB_EMPL_DEP 
				                                   WHERE USER_GUID = @USER_GUID 
				                                     AND GROUP_ID = @GROUP_ID)

                                -- 查詢申請者HR資訊
                                SELECT BU 
                                  FROM TB_EB_EMPL_HR
                                 WHERE USER_GUID = @USER_GUID
                                ", ConnectionString))
                            using (DataSet ds = new DataSet())
                            {
                                sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", hidEMPLOYEEGuid.Value);
                                sda.SelectCommand.Parameters.AddWithValue("@GROUP_ID", hidAPPLICANTDEPT.Value);
                                try
                                {
                                    sda.Fill(ds);
                                    if (ds.Tables[0].Rows.Count > 0 &&
                                        ds.Tables[1].Rows.Count > 0)
                                    {
                                        DataRow drUser = ds.Tables[0].Rows[0];
                                        DataRow drTitle = ds.Tables[1].Rows[0];
                                        DataRow drEmplHr = ds.Tables[2].Rows[0];
                                        hidOCOMPANYID.Value = drUser["COMPANY_NO"].ToString(); // 申報公司
                                        ktxtOCOMPANYNAME.Text = hidOCOMPANYID.Value; // 申報公司
                                        ktxtODEPARTNAME.Text = KUser.GroupName[0]; // 帳部部門
                                        hidODEPARTID.Value = hidAPPLICANTDEPT.Value; // 帳部部門
                                        //ktxtOPROFITNAME.Text = user.GroupName; // 利潤中心
                                        ktxtOPROFITNAME.Text = drEmplHr["BU"].ToString(); // 利潤中心
                                        hidOPROFITID.Value = drEmplHr["BU"].ToString(); // 利潤中心
                                        ktxtODUTYNAME.Text = drTitle["TITLE_NAME"].ToString(); // 職稱
                                        hidODUTYID.Value = drTitle["TITLE_ID"].ToString(); // 職稱
                                        ktxtOINSURANCESTATUS.Text = drUser["OPTION2"].ToString(); // 投保身份
                                        ktxtONotCutHIdentity.Text = drUser["OPTION3"].ToString(); // 免扣繳對象
                                        ktxtOIDYCLASSID.Text = drUser["OPTION4"].ToString(); // 參考代碼
                                    }
                                }
                                catch (Exception e)
                                {
                                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_EMPCHANGE.異動前職務資訊.SELECT.ERROR:{0}", e.Message));
                                }
                            }
                            #endregion 異動前職務資訊

                            #region 異動後職務資訊
                            #region 申報公司
                            DataTable dtCompanyIDSource = getModifyType(SCSHRConfiguration.SCSSCompanyIDProdID, "SYS_ViewID,SYS_Name");
                            // 新增請選擇選項
                            ndr = dtCompanyIDSource.NewRow();
                            ndr["SYS_VIEWID"] = "";
                            ndr["SYS_NAME"] = "===請選擇===";
                            dtCompanyIDSource.Rows.InsertAt(ndr, 0);

                            kddlCOMPANYID.DataSource = dtCompanyIDSource;
                            kddlCOMPANYID.DataTextField = "SYS_NAME";
                            kddlCOMPANYID.DataValueField = "SYS_VIEWID";
                            kddlCOMPANYID.DataBind();
                            kddlCOMPANYID.SelectedValue = hidOCOMPANYID.Value;
                            #endregion 申報公司
                            ktxtDEPARTNAME.Text = ktxtODEPARTNAME.Text; // 帳部部門
                            hidDEPARTID.Value = hidODEPARTID.Value; // 帳部部門
                            hidPROFITID.Value = hidOPROFITID.Value; // 利潤中心
                            kddlPROFITNAME.SelectedValue = hidOPROFITID.Value; // 利潤中心
                            if (!string.IsNullOrEmpty(kddlPROFITNAME.SelectedValue))
                                ktxtOPROFITNAME.Text = kddlPROFITNAME.SelectedItem.Text; // 如果找的到利潤中心的名稱就改用名稱
                            ktxtPROFITNAME.Text = ktxtOPROFITNAME.Text; // 利潤中心

                            #region 職稱

                            DataTable dtTitleSource = getModifyType(SCSHRConfiguration.SCSSTitleProdID, "SYS_ViewID,SYS_Name");
                            // 新增請選擇選項
                            ndr = dtTitleSource.NewRow();
                            ndr["SYS_VIEWID"] = "";
                            ndr["SYS_NAME"] = "===請選擇===";
                            dtTitleSource.Rows.InsertAt(ndr, 0);

                            kddlDUTYID.DataSource = dtTitleSource;
                            kddlDUTYID.DataTextField = "SYS_NAME";
                            kddlDUTYID.DataValueField = "SYS_VIEWID";
                            kddlDUTYID.DataBind();
                            kddlDUTYID.SelectedValue = hidODUTYID.Value;

                            #endregion 職稱

                            kddlINSURANCESTATUS.SelectedValue = ktxtOINSURANCESTATUS.Text; // 投保身份
                            kddlNotCutHIdentity.SelectedValue = ktxtONotCutHIdentity.Text; // 免扣繳對象

                            #region 參考代碼

                            DataTable dtIDYClassSource = getModifyType(SCSHRConfiguration.SCSSIDYClassIDProdID, "SYS_ViewID,SYS_Name");
                            // 新增請選擇選項
                            ndr = dtIDYClassSource.NewRow();
                            ndr["SYS_VIEWID"] = "";
                            ndr["SYS_NAME"] = "===請選擇===";
                            dtIDYClassSource.Rows.InsertAt(ndr, 0);

                            kddlIDYCLASSID.DataSource = dtIDYClassSource;
                            kddlIDYCLASSID.DataTextField = "SYS_NAME";
                            kddlIDYCLASSID.DataValueField = "SYS_VIEWID";
                            kddlIDYCLASSID.DataBind();
                            kddlIDYCLASSID.SelectedValue = ktxtOIDYCLASSID.Text;

                            #endregion 參考代碼

                            #endregion 異動後職務資訊

                            //取得利潤中心
                            kddlPROFITNAME.DataSource = getPROFITID(SCSHRConfiguration.SCSSPROFITID, "SYS_ViewID,SYS_Name");
                            kddlPROFITNAME.DataValueField = "SYS_VIEWID";
                            kddlPROFITNAME.DataTextField = "SYS_NAME";
                            kddlPROFITNAME.DataBind();
                        }
                        // 設定Picker是否能輸入
                        kdpSTARTDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdpENDDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
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
            }
            else // 如果網頁POSTBACK
            {
                JGlobalLibs.WebUtils.RequestHiddenFields(UpdatePanel1); // 取回HiddenField的值
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        // 設定Picker是否能輸入
                        kdpSTARTDATE.ViewType = KYTViewType.Input;
                        kdpENDDATE.ViewType = KYTViewType.Input;
                        kdpSTARTDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdpENDDATE.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
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
    /// 取得呼叫WS的BOFind後回傳的資料
    /// </summary>
    /// <param name="progId"></param>
    /// <param name="selectFields"></param>
    /// <returns></returns>
    private DataTable getModifyType(string progId, string selectFields)
    {
        DataTable dtSource = new DataTable();
        Exception ex = null;
        dtSource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_EMPCHANGE.getModifyType.service.Error:{0}", ex.Message));

        return dtSource;
    }

    /// <summary>
    /// 取得異動原因
    /// </summary>
    /// <param name="progId"></param>
    /// <param name="selectFields"></param>
    /// <returns></returns>
    private DataTable getEMPCHANGEType(string progId, string selectFields)
    {
        DataTable dtSource = new DataTable();
        dtSource = getModifyType(progId, selectFields);
        DataTable dtReturn = new DataTable();
        foreach (DataColumn dc in dtSource.Columns)
        {
            dtReturn.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
        }
        foreach (DataRow dr in dtSource.Rows)
        {
            if (dr["FLAG"].ToString() == "1")
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


    protected void btnDEPARTID_Click(object sender, EventArgs e)
    {
        ViewState["choiceType"] = "DEPART";
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), @"DelayStartChoiceGroup();", true);

    }

    protected void btnPROFITID_Click(object sender, EventArgs e)
    {
        ViewState["choiceType"] = "PROFIT";
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), @"DelayStartChoiceGroup();", true);

    }

    /// <summary>
    /// 按下UOF選人元件(單選)後事件
    /// </summary>
    /// <param name="choiceResult"></param>
    protected void choiceGroup_EditButtonOnClick(string[] choiceResult)
    {
        if (choiceResult != null && choiceResult.Length > 0)
        {
            switch ((string)ViewState["choiceType"])
            {
                case "DEPART": // 帳部部門
                    ktxtDEPARTNAME.Text = choiceResult[1];
                    hidDEPARTID.Value = choiceResult[0];
                    break;
                case "PROFIT": // 利潤中心
                    ktxtPROFITNAME.Text = choiceResult[1];
                    hidPROFITID.Value = choiceResult[0];
                    break;
            }
        }
    }
    /// <summary>
    /// 取得利潤中心
    /// </summary>
    private DataTable getPROFITID(string progId, string selectFields)
    {
        DataTable dtScource = new DataTable();
        Exception ex = null;
        dtScource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_EMPCHANGE.getPROFITID.service.Error:{0}", ex.Message));

        DataRow ndr = dtScource.NewRow();
        ndr["SYS_NAME"] = "===請選擇===";
        ndr["SYS_VIEWID"] = "";
        ndr["SYS_ID"] = "";
        dtScource.Rows.InsertAt(ndr, 0);
        return dtScource;
    }

    protected void kddlPROFITNAME_SelectedIndexChanged(object sender, EventArgs e)
    {
        hidPROFITID.Value = kddlPROFITNAME.SelectedValue;
    }

    protected void kdpSTARTDATE_TextChanged(object sender, EventArgs e)
    {
        KYTDatePicker _kdpSTARTDATE = (KYTDatePicker)sender;
        kdpENDDATE.Text = _kdpSTARTDATE.Text; // 結束日期預設帶生效日期
    }
}
