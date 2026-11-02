using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Design;
using JGlobalLibs.Types;
using KYTLog;
using Newtonsoft.Json;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Xml;
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
    * 1. 加班單單號，改為使用跳窗模式帶回
    * 2. 前端驗證顯示文字修正
* 發生原因：
    * 1. 新規格
    * 2. BUG修正，顯示錯誤，原為請假 -> 改為加班
* 修改位置：
    * 1.「前端網頁」中，新增文字格(ktxtDOC_NBR_Ori)、按鈕(ibtnDOC_NBR)，取代原本的下拉選單(kddlOT_DOC_NBR)
    *   「前端網頁 -> checkLVH_DOC_NBR()」中，原是判斷kddlOT_DOC_NBR，改為判斷ktxtDOC_NBR_Ori
    *   「SetField()」中，隱藏下拉選單(kddlOT_DOC_NBR)設定Visible為false，新增按鈕(ibtnDOC_NBR)跳窗路徑設定
    *   「ConditionValue、getCurrentValue()」中，原給予下拉選單的值(kddlOT_DOC_NBR)，改為給予ktxtDOC_NBR_Ori的值
    * 2.「前端網頁 -> CustomValidator方法 -> checkOVH_DOC_NBR、checkReadOVH_DOC_NBR、checkCANCEL_REASON)」中，文字修正
* **/

/**
* 修改時間：2020/04/16
* 修改人員：陳緯榕
* 修改項目：
    * 前端顯示項目錯字
    * 新規格：標題修改
* 發生原因：
    * 打錯字
    * *******
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈加假單單號〉改為〈加班單單號〉
    * ********
    * 「前端網頁」中，〈SCSHR銷班資訊〉改為〈銷班資訊〉
* **/

/**
* 修改時間：2020/03/13
* 修改人員：陳緯榕
* 修改項目：
    * kxtMessage顯示需為紅色
* 發生原因：
    * ，紅色是屬於警告意味強的顏色，故使用紅色顯示錯誤
* 修改位置：
    * 「前端網頁」中，新增〈CSS：msgColor〉；並新增〈KYTTextBox：ktxtMessage〉且給予屬性〈LabelCssClass〉設為〈msgColor〉
* **/

/**
* 修改時間：2020/03/03
* 修改人員：陳緯榕
* 修改項目：
    * 找不到加班單能夠銷班
* 發生原因：
    * 加班單找的是「當月」的所有表單，最好的方法是去找關帳日，但現在靠往前一個月來計算
* 修改位置：
    * 「getOverTimeData」中，〈SQL〉中，加班的的起迄時間改由〈@START〉、〈@END〉控制，開始時間是〈現在時間往前一個月〉
* **/

/**
* 修改時間：2020/01/02
* 修改人員：陳緯榕
* 修改項目：
    * ConditionValue和加班單的條件需要一致
* 發生原因：
    * 無印需要銷班跑原先的流程
* 修改位置：
    * 「ConditionValue」中，使用〈kddlOT_DOC_NBR〉查詢原加班單表單內容，取出表單〈KYTI_SCSHR_CLEAVE〉內容中的〈ktxtDoOverType〉、〈kcbIsPre〉
    * 「ConditionValue」中，新增〈IsPreOT〉、〈DoOverType〉兩個條件；〈othours〉改為〈OverTimeHours〉
* **/

/**
* 修改時間：2019/10/30
* 修改人員：陳緯榕
* 修改項目：
    * 加班原因不要顯示
    * 給付方式在尚未選擇時不要顯示東西
    * 新增加班內容說明欄
* 發生原因：
    * 客戶大多用不到，但飛騰預設要有
    * *********
    * krblCHANGETYPE的Selected = "True"拿掉
    * *********
    * 新規格：選擇後也要顯示加班內容說明
* 修改位置：
    * 「前端網頁」中，〈加班原因〉區塊的兩個div都加上〈 style="display:none;"〉
    * ********
    * 「前端網頁」中，〈krblCHANGETYPE〉內的項目的屬性〈Selected〉都不要
    * ********
    * 「前端網頁」中，在網頁底端加上〈KYTTextBox：ktxtNOTE〉
    * 「SetField」中，表單第一次載入時，〈ktxtNOTE〉固定〈ViewType〉設為〈Input〉，屬性〈ReadOnly〉設為〈true〉
    * 「btnRead_Click」中，先清空〈ktxtNOTE〉的〈Text〉屬性，然後再填入〈REMARK〉的資料
* **/

/**
* 修改時間：2018/12/03
* 修改人員：陳緯榕
* 修改項目：銷班原因不能超過50個字元
* 修改位置： 
    * 「前端網頁」〈ktxtCANCEL_REASON〉限制長度〈50〉；Rows改為3
* **/

/// <summary>
/// 飛騰銷班資訊
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_COVERTIME : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
            bool IsPreOT = false;
            string DoOverType = "";

            if (!string.IsNullOrEmpty(ktxtDOC_NBR_Ori.Text))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT *
                  FROM TB_WKF_TASK
                 WHERE TASK_ID = @TASK_ID
                ", new DatabaseHelper().Command.Connection.ConnectionString))
                using (DataSet ds = new DataSet())
                {
                    sda.SelectCommand.Parameters.AddWithValue("@TASK_ID", ktxtDOC_NBR_Ori.Text);
                    try
                    {
                        if (sda.Fill(ds) > 0)
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(ds.Tables[0].Rows[0]["CURRENT_DOC"].ToString());
                            XmlNode node = doc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='KYTI_SCSHR_CLEAVE']"); // 取出外掛欄位資料
                            KYTJsonDict dict = JsonConvert.DeserializeObject<KYTJsonDict>(HttpUtility.HtmlDecode(node.InnerText));
                            IsPreOT = dict.GetString("kcbIsPre").ToString() == "X";
                            DoOverType = dict.GetString("ktxtDoOverType").ToString();
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }


            string cv = new JGlobalLibs.UOFUtils.ConditionValue()
               .Add("dept", ktxtAPPLICANTDEPT.Text)
               .Add("changetype", krblCHANGETYPE.SelectedValue)
               .Add("OverTimeHours", ktxtOverTimeHours.Text)
               .Add("IsPreOT", IsPreOT ? "Y" : "N")
               .Add("DoOverType", DoOverType)
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
            return string.Format(@"{0}，加班折換方式：{1}，加班時間{2}~{3}，時數{4}", ktxtAPPLICANT.Text, krblCHANGETYPE.SelectedIndex > -1 ? krblCHANGETYPE.SelectedItem.Text : "未選擇", kdtpOT_START.Text, kdtpOT_END.Text, ktxtOverTimeHours.Text);
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
            return string.Format(@"{0}，加班折換方式：{1}，加班時間{2}~{3}，時數{4}", ktxtAPPLICANT.Text, krblCHANGETYPE.SelectedIndex > -1 ? krblCHANGETYPE.SelectedItem.Text : "未選擇", kdtpOT_START.Text, kdtpOT_END.Text, ktxtOverTimeHours.Text);

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

            // 取得資料庫連通字串
            ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
            service = ConstructorCommonSettings.setSCSSServiceProxDefault();

            if (!Page.IsPostBack) // 網頁首次載入
            {
                if (!string.IsNullOrEmpty(fieldOptional.FieldValue))
                    kytController.FieldValue = fieldOptional.FieldValue;

                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀
                ktxtNOTE.ViewType = KYTViewType.Input;
                ktxtNOTE.ReadOnly = true;
                btnRead.Visible = false; // 隱藏讀取
                ibtnDOC_NBR.Visible = false;
                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值

                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        hidAPIResult.Value = ""; // 清掉先前的旗標
                        btnRead.Visible = true; // 顯示讀取
                        ibtnDOC_NBR.Visible = true;
                        kddlOT_DOC_NBR.ViewType = KYTViewType.Input; // 加班單單號可輸入
                        ktxtCANCEL_REASON.ViewType = KYTViewType.Input; // 銷班原因可輸入
                        if (this.FormFieldMode == FieldMode.Applicant) // 如果是剛起單
                        {
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            hidApplicantGUID.Value = this.ApplicantGuid;
                            hidApplicantName.Value = KUser.Name;

                            DataTable dtSource = getOVERTIMEType(SCSHRConfiguration.SCSSOverTimeTypeProgID, "SYS_ViewID,SYS_Name,SYS_EngName");
                            kddlREMARK.DataSource = dtSource;
                            kddlREMARK.DataValueField = "SYS_VIEWID";
                            kddlREMARK.DataTextField = "SYS_NAME";
                            kddlREMARK.DataBind();
                        }
                        hidAPPLICANTDATE.Value = DateTime.Now.ToString("yyyy/MM/dd"); // 申請日期
                        //kddlOT_DOC_NBR.DataSource = getOverTimeData(hidApplicantGUID.Value);
                        //kddlOT_DOC_NBR.DataTextField = "SHOW";
                        //kddlOT_DOC_NBR.DataValueField = "DOC_NBR";
                        //kddlOT_DOC_NBR.DataBind();
                        kddlOT_DOC_NBR.Visible = false;
                        ktxtDOC_NBR_Ori.ViewType = KYTViewType.Input;
                        ktxtDOC_NBR_Ori.ReadOnly = true;
                        Dialog.Open2(ibtnDOC_NBR, string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/Search_OT_And_LEA_Form.aspx"), "查詢加班單號", 850, 600, Dialog.PostBackType.AfterReturn, new { FROM_TYPE = "OVER_TIME", USER_GUID = hidApplicantGUID.Value }.ToExpando());

                        btnRead_Click(btnRead, null);
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

    private DataTable getOVERTIMEType(string progId, string selectFields)
    {
        DataTable dtScource = new DataTable();
        Exception ex = null;
        dtScource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_COVERTIME.getLEAVEType.service.Error:{0}", ex.Message));

        return dtScource;
    }

    private DataTable getOverTimeData(string APPLICANTGUID)
    {
        DataTable dtReturn = null;
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT '===請選擇===' AS 'SHOW', '' AS 'DOC_NBR'
            UNION ALL
            SELECT DOC_NBR AS 'SHOW', DOC_NBR
              FROM Z_SCSHR_OVERTIME 
             WHERE (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)
               AND TASK_STATUS = 2 
               AND TASK_RESULT = 0
               AND APPLICANTGUID = @APPLICANTGUID
               AND ((OT_START BETWEEN @START AND @END)
                    OR (OT_END BETWEEN @START AND @END))
              -- AND ((OT_START BETWEEN DATEADD(mm, DATEDIFF(mm,0,GETDATE()), 0) -- 加班時間(起)在該月(月底23:59:59.997)
			--					  AND DATEADD(ms, -2, DATEADD(mm, DATEDIFF(m, 0, GETDATE()) + 1, 0)))
			  --  OR (OT_END BETWEEN DATEADD(mm, DATEDIFF(mm,0,GETDATE()), 0) -- 或加班時間(迄)在該月(月底23:59:59.997)
				--		 	   AND DATEADD(ms, -2, DATEADD(mm, DATEDIFF(m, 0, GETDATE()) + 1, 0)))               )
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@APPLICANTGUID", APPLICANTGUID);
            sda.SelectCommand.Parameters.AddWithValue("@START", DateTime.Now.AddMonths(-1));
            sda.SelectCommand.Parameters.AddWithValue("@END", DateTime.Now);
            try
            {
                sda.Fill(ds);
                dtReturn = ds.Tables[0];
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_COVERTIME.getOverTimeData.ERROR:{0}", e.Message));
            }
        }
        return dtReturn;
    }

    private DataTable getCurrentValue(string DOC_NBR)
    {
        DataTable dtReturn = null;

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT TOP 1 *,
                   ISNULL((SELECT TOP 1 NAME FROM TB_EB_USER WHERE USER_GUID = Z_SCSHR_OVERTIME.APPLICANTGUID), '') AS 'USER_NAME',
                   ISNULL((SELECT TOP 1 GROUP_ID FROM TB_EB_GROUP WHERE GROUP_CODE = Z_SCSHR_OVERTIME.GROUP_CODE), '') AS 'GROUP_ID',
                   ISNULL((SELECT TOP 1 GROUP_NAME FROM TB_EB_GROUP WHERE GROUP_CODE = Z_SCSHR_OVERTIME.GROUP_CODE), '') AS 'GROUP_NAME'
              FROM Z_SCSHR_OVERTIME WHERE DOC_NBR = @DOC_NBR
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
            try
            {
                sda.Fill(ds);
                dtReturn = ds.Tables[0];
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_COVERTIME.getCurrentValue.ERROR:{0}", e.Message));
            }
        }
        return dtReturn;
    }

    protected void btnRead_Click(object sender, EventArgs e)
    {
        ktxtAPPLICANTDEPT.Text = ""; // 部門
        hidGROUPCODE.Value = "";
        hidAPPLICANTDEPT.Value = "";
        ktxtAPPLICANT.Text = ""; // 申請人
        hidAPPLICANT.Value = "";
        //krblCHANGETYPE.SelectedItem.Text = ""; // 不顯示預設的給付方式
        kdtpOT_START.Text = ""; // 加班時間(起)
        kdtpOT_END.Text = ""; // 加班時間(迄)
        ktxtOT_CLASSTYPE_NAME.Text = ""; // 加班班別
        hidOT_CLASSTYPE.Value = "";
        //kddlREMARK.SelectedItem.Text = ""; // 不顯示預設的加班原因
        ktxtREMARKOther.Text = ""; // 其它
        ktxtApplyHours.Text = ""; // 申請時數
        ktxtOverTimeHours.Text = ""; // 加班時數
        ktxtNOTE.Text = ""; // 加班內容說明

        if (!string.IsNullOrEmpty(ktxtDOC_NBR_Ori.Text))
        {
            DataTable dtSource = getCurrentValue(ktxtDOC_NBR_Ori.Text);
            foreach (DataRow dr in dtSource.Rows)
            {
                ktxtAPPLICANTDEPT.Text = dr["GROUP_NAME"].ToString(); // 部門
                hidAPPLICANTDEPT.Value = dr["GROUP_ID"].ToString();
                hidGROUPCODE.Value = dr["GROUP_CODE"].ToString();
                ktxtAPPLICANT.Text = dr["USER_NAME"].ToString(); // 申請人
                hidAPPLICANT.Value = dr["APPLICANT"].ToString();
                krblCHANGETYPE.SelectedValue = dr["CHANGETYPE"].ToString(); // 給付方式
                kdtpOT_START.Text = dr["OT_START"].ToString(); // 加班時間(起)
                kdtpOT_END.Text = dr["OT_END"].ToString(); // 加班時間(迄)
                ktxtOT_CLASSTYPE_NAME.Text = dr["OT_WORKNAME"].ToString(); // 加班班別
                hidOT_CLASSTYPE.Value = dr["OT_WORKID"].ToString();
                kddlREMARK.SelectedValue = dr["OT_REASON"].ToString(); // 加班原因
                ktxtREMARKOther.Text = dr["OT_REASON_OTHER"].ToString(); // 其它
                ktxtApplyHours.Text = dr["OT_APTIMES"].ToString(); // 申請時數
                ktxtOverTimeHours.Text = dr["OT_TIMES"].ToString(); // 加班時數
                ktxtNOTE.Text = dr["REMARK"].ToString(); // 加班內容說明
            }
            hidAPIResult.Value = "OK";
        }
    }

    /// <summary>
    /// 加班單號_取回
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
