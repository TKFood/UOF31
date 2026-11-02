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
using System.Web.UI;
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
    * 2.「btnRead_Click()」，傳入TMP_EMPLOYEEID，的值 -> 改傳入EmployeeNo
* **/

/**
* 修改時間：2021/05/26
* 修改人員：梁夢慈
* 修改項目：
    * 1. 加班單單號跳窗按鈕(ibtnDOC_NBR)除了起單/退回申請者以外，其餘隱藏
* 發生原因：
    * 1. BUG修正
* 修改位置：
    * 1.「SetField()」中，按鈕屬性"Visible"預設為false，起單/退回申請者設定為true
* **/

/**
* 修改時間：2021/05/21
* 修改人員：梁夢慈
* 修改項目：
    * 1. 請假單單號，改為使用跳窗模式帶回
* 發生原因：
    * 1. 新規格
* 修改位置：
    * 1.「前端網頁」中，新增文字格(ktxtDOC_NBR_Ori)、按鈕(ibtnDOC_NBR)，取代原本的下拉選單(kddlLVH_DOC_NBR)
    *   「前端網頁 -> checkLVH_DOC_NBR()」中，原是判斷kddlLVH_DOC_NBR，改為判斷ktxtDOC_NBR_Ori
    *   「SetField()」中，隱藏下拉選單(kddlLVH_DOC_NBR)設定Visible為false，新增按鈕(ibtnDOC_NBR)跳窗路徑設定
    *   「getCurrentValue()」中，原給予下拉選單的值(kddlLVH_DOC_NBR)，改為給予ktxtDOC_NBR_Ori的值
* **/

/**
* 修改時間：2020/04/16
* 修改人員：陳緯榕
* 修改項目：
    * 新規格：標題修改
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈SCSHR銷假資訊〉改為〈銷假資訊〉
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
* 修改時間：2020/01/02
* 修改人員：陳緯榕
* 修改項目：
    * ConditionValue和請假單的條件需要一致
* 發生原因：
    * 無印需要銷假跑原先的流程
* 修改位置：
    * 「ConditionValue」中，新增〈agentapply〉、〈has_agent〉兩個條件
* **/


/**
* 修改時間：2018/12/03
* 修改人員：陳緯榕
* 修改項目：銷假原因不能超過50個字元
* 修改位置： 
    * 「前端網頁」〈ktxtCANCEL_REASON〉限制長度〈50〉
* **/

/**
* 修改時間：2018/12/05
* 修改人員：陳緯榕
* 修改項目：銷假單的檢查流程改成先去取得中繼表資料，再呼叫WS BOImport
* 修改位置： 
    * 「btnRead_Click」更改流程
* **/

/**
* 修改時間：2018/12/18
* 修改人員：陳緯榕
* 修改項目：
    * 選完單號後按下讀取沒有反應
    * 前端網頁顯示錯誤必須要是紅字
* 修改位置： 
    * 「btnRead_Click」所有有DebugLog的地方都加上〈ktxtMessage〉來顯示錯誤
    * 「前端網頁」〈ktxtMessage〉的〈LabelCssClass〉加上〈color_red〉類別
* **/

/// <summary>
/// 飛騰銷假資訊
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_CLEAVE : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
                .Add("leacode", hidLEACODE.Value)
                .Add("leatimes", ktxtLEAHOURS.Text)
                .Add("leadays", ktxtLEADAYS.Text)
                .Add("title_name", hidTitleName.Value)
                .Add("agentapply", Current.UserGUID != this.ApplicantGuid ? "Y" : "N")
                .Add("has_agent", ktxtLEAAGENT.Text != "" ? "Y" : "N")
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CLEAVE.ConditionValue.cv:{0}", cv));
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
            return string.Format(@"{0}, 假別：{1}, 銷假時間:{2}~{3}, 天數{4}, 時數{5}"
                            , ktxtLEAEMP.Text
                            , ktxtLEACODE.Text
                            , kdtpSTARTTIME.Text
                            , kdtpENDTIME.Text
                            , ktxtLEADAYS.Text
                            , ktxtLEAHOURS.Text);
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
            return string.Format(@"{0}, 假別：{1}, 銷假時間:{2}~{3}, 天數{4}, 時數{5}"
                             , ktxtLEAEMP.Text
                             , ktxtLEACODE.Text
                             , kdtpSTARTTIME.Text
                             , kdtpENDTIME.Text
                             , ktxtLEADAYS.Text
                             , ktxtLEAHOURS.Text);
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
               .AddPerson("leaagent", hidLEAAGENT.Value)
               .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CLEAVE.RealValue.rv:{0}", rv));
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
                btnRead.Visible = false;
                ibtnDOC_NBR.Visible = false;

                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值

                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        hidAPIResult.Value = ""; // 清掉先前的旗標
                        btnRead.Visible = true;
                        ibtnDOC_NBR.Visible = true; 
                        kddlLVH_DOC_NBR.ViewType = KYTViewType.Input; // 請假單單號可輸入
                        ktxtCANCEL_REASON.ViewType = KYTViewType.Input; // 銷假原因可輸入
                        if (this.FormFieldMode == FieldMode.Applicant) // 如果是剛起單
                        {
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            hidApplicantGUID.Value = this.ApplicantGuid;
                            hidTitleName.Value = KUser.Title_Name[0];
                        }
                        //kddlLVH_DOC_NBR.DataSource = getLeaveData(hidApplicantGUID.Value);
                        //kddlLVH_DOC_NBR.DataTextField = "SHOW";
                        //kddlLVH_DOC_NBR.DataValueField = "DOC_NBR";
                        //kddlLVH_DOC_NBR.DataBind();
                        kddlLVH_DOC_NBR.Visible = false;
                        ktxtDOC_NBR_Ori.ViewType = KYTViewType.Input;
                        ktxtDOC_NBR_Ori.ReadOnly = true;
                        Dialog.Open2(ibtnDOC_NBR, string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/Search_OT_And_LEA_Form.aspx"), "查詢請假單號", 850, 600, Dialog.PostBackType.AfterReturn, new { FROM_TYPE = "LEAVE", USER_GUID = hidApplicantGUID.Value }.ToExpando());
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

    private DataTable getLeaveData(string APPLICANTGUID)
    {
        DataTable dtReturn = null;
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT '===請選擇===' AS 'SHOW', '' AS 'DOC_NBR'
            UNION ALL
            SELECT DOC_NBR AS 'SHOW', DOC_NBR
              FROM Z_SCSHR_LEAVE 
             WHERE (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)
               AND TASK_STATUS = 2 
               AND TASK_RESULT = 0
               AND APPLICANTGUID = @APPLICANTGUID
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@APPLICANTGUID", APPLICANTGUID);
            try
            {
                sda.Fill(ds);
                dtReturn = ds.Tables[0];
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_CLEAVE.getLeaveData.ERROR:{0}", e.Message));
            }
        }
        return dtReturn;
    }

    private DataTable getCurrentValue(string DOC_NBR)
    {
        DataTable dtReturn = null;

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT TOP 1 *,
                         ISNULL((SELECT TOP 1 NAME FROM TB_EB_USER WHERE USER_GUID = Z_SCSHR_LEAVE.LEAEMP), '') AS 'LEAEMP_NAME',
                         ISNULL((SELECT NAME FROM TB_EB_USER WHERE USER_GUID = Z_SCSHR_LEAVE.LEAAGENT), '') AS 'LEAAGENT_NAME',
                         ISNULL((SELECT TOP 1 GROUP_NAME FROM TB_EB_GROUP WHERE GROUP_CODE = Z_SCSHR_LEAVE.GROUP_CODE), '') AS 'GROUP_NAME'
              FROM Z_SCSHR_LEAVE WHERE DOC_NBR = @DOC_NBR
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
            try
            {
                sda.Fill(ds);
                dtReturn = ds.Tables[0];
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CLEAVE.getCurrentValue.dtReturn:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtReturn)));
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_CLEAVE.getCurrentValue.ERROR:{0}", e.Message));
            }
        }
        return dtReturn;
    }

    protected void btnRead_Click(object sender, EventArgs e)
    {
        ktxtMessage.Text = "";
        ktxtAPPLICANTDEPT.Text = "";
        ktxtAPPLICANTDATE.Text = "";
        hidCompanyNo.Value = "";
        hidGROUPCODE.Value = "";
        ktxtLEAEMP.Text = "";
        hidLEAEMP.Value = "";
        ktxtLEAAGENT.Text = "";
        hidLEAAGENT.Value = "";
        ktxtLEACODE.Text = "";
        hidLEACODE.Value = "";
        ktxtSP_DATE.Text = "";
        ktxtSP_NAME.Text = "";
        kdtpSTARTTIME.Text = "";
        kdtpENDTIME.Text = "";
        ktxtLEAHOURS.Text = "";
        ktxtLEADAYS.Text = "";
        ktxtREMARK.Text = "";
        if (!string.IsNullOrEmpty(ktxtDOC_NBR_Ori.Text))
        {
            Exception ex = null; // 初始化
            bool resultStatus = false;
            hidAPIResult.Value = ""; // 清空之前的API查詢結果
            DataTable dtOSource = getCurrentValue(ktxtDOC_NBR_Ori.Text);
            foreach (DataRow dr in dtOSource.Rows)
            {

                DateTime dtStart = DateTime.MinValue;
                DateTime.TryParse(dr["STARTTIME"].ToString(), out dtStart);
                DateTime dtEnd = DateTime.MinValue;
                DateTime.TryParse(dr["ENDTIME"].ToString(), out dtEnd);
                // 計為在途銷假單
                JArray jaTable = new JArray();
                JObject _joTable = new JObject();
                _joTable.Add(new JProperty("USERNO", "1"));
                _joTable.Add(new JProperty("SYS_VIEWID", ""));
                _joTable.Add(new JProperty("SYS_DATE", DateTime.Now.ToString("yyyyMMdd")));
                //_joTable.Add(new JProperty("TMP_EMPLOYEEID", JGlobalLibs.UOFUtils.getUserAccount(dr["LEAEMP"].ToString())));
                _joTable.Add(new JProperty("TMP_EMPLOYEEID",new KYT_UserPO().GetUserDetailByUserGuid(dr["LEAEMP"].ToString()).EmployeeNo));
                _joTable.Add(new JProperty("TMP_DEPARTID", dr["GROUP_CODE"].ToString()));
                _joTable.Add(new JProperty("TMP_LEAVEID", ktxtDOC_NBR_Ori.Text));
                _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd")));
                _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm")));
                _joTable.Add(new JProperty("ENDDATE", dtEnd.ToString("yyyyMMdd")));
                _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm")));
                _joTable.Add(new JProperty("REASON", ktxtCANCEL_REASON.Text));
                jaTable.Add(_joTable);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CLEAVE.btnRead_Click.jaTable:{0}", jaTable));
                DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                dtSource.TableName = SCSHRConfiguration.SCSSCLProgID;
                DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSCLProgID);
                dsSource.Tables.Add(dtSource);
                DataTable dtResult = service.BOImport(SCSHRConfiguration.SCSSCLProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CLEAVE.btnRead_Click.dtResult:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));

                if (ex != null)
                {
                    ktxtMessage.Text = ex.Message;
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_CLEAVE.btnRead_Click.BOImport.ERROR:{0}", ex.Message));
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_CLEAVE::btnRead_Click::BOImportWS::StackTrace::{0}::ERROR::{1}", ex.StackTrace, ex.Message));
                }
                if (dtResult != null &&
                    dtResult.Rows.Count > 0)
                {
                    resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
                    if (!resultStatus)
                    {
                        ktxtMessage.Text = dtResult.Rows[0]["_MESSAGE"].ToString();
                        hidAPIResult.Value = ktxtMessage.Text;

                    }
                }

                if (resultStatus)
                {
                    hidAPIResult.Value = "OK";

                    ktxtMessage.Text = "";
                    ktxtAPPLICANTDEPT.Text = dr["GROUP_NAME"].ToString();
                    ktxtAPPLICANTDATE.Text = dr["APPLICANTDATE"].ToString();
                    hidCompanyNo.Value = dr["APPLICANTCOMP"].ToString();
                    hidGROUPCODE.Value = dr["GROUP_CODE"].ToString();
                    ktxtLEAEMP.Text = dr["LEAEMP_NAME"].ToString();
                    hidLEAEMP.Value = dr["LEAEMP"].ToString();
                    ktxtLEAAGENT.Text = dr["LEAAGENT_NAME"].ToString();
                    hidLEAAGENT.Value = dr["LEAAGENT"].ToString();
                    ktxtLEACODE.Text = dr["LEACODENAME"].ToString();
                    hidLEACODE.Value = dr["LEACODE"].ToString();
                    ktxtSP_DATE.Text = dr["SP_DATE"].ToString();
                    ktxtSP_NAME.Text = dr["SP_NAME"].ToString();
                    kdtpSTARTTIME.Text = dr["STARTTIME"].ToString();
                    kdtpENDTIME.Text = dr["ENDTIME"].ToString();
                    ktxtLEAHOURS.Text = dr["LEAHOURS"].ToString();
                    ktxtLEADAYS.Text = dr["LEADAYS"].ToString();
                    ktxtREMARK.Text = dr["REMARK"].ToString();

                }
                ktxtMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }


    /// <summary>
    /// 請假單號_取回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnDOC_NBR_Click(object sender, ImageClickEventArgs e)
    {
        string value = Dialog.GetReturnValue();
        if (string.IsNullOrEmpty(value)) return;
        hidAPIResult.Value = ""; // 清空之前的API查詢結果
        string returnValue = "";
        returnValue = value.Replace("&#&#&&", "''").Replace("#&#&##", "'");
        DataTable result = JsonConvert.DeserializeObject<DataTable>(returnValue);
        DataRow dr = result.Rows[0];
        ktxtDOC_NBR_Ori.Text = dr["DOC_NBR"].ToString(); // 原表單單號
    }
}
