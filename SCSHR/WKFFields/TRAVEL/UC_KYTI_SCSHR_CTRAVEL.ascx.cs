using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Data;
using Ede.Uof.WKF.Design;
using KYTLog;
using Newtonsoft.Json.Linq;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using UOFAssist.WKF;


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
    * 1.「SetField()」中，註解所有EBUser，改為KYT_EBUser
    * 2.「btnChk_Click()、CheckSignLevel()」，傳入TMP_EMPLOYEEI、TMP_AGENTIDD，的值 -> 改傳入EmployeeNo
* **/

/**
* 修改時間：2020/04/16
* 修改人員：陳緯榕
* 修改項目：
    * 新規格：標題修改
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈SCSHR銷出差資訊〉改為〈銷出差資訊〉
* **/

/// <summary>
/// 飛騰銷出差單
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_CTRAVEL : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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
    /// 表單單號
    /// </summary>
    public string FormNumber;

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
                .Add("DOMESTIC", krblDOMESTIC.SelectedValue)
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.ConditionValue.cv:{0}", cv));
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
            return string.Format(@"{0}, 出差時間:{1}~{2}, 事由:{3}", ktxtTRAVEL_MEN.Text, kdtpSTARTTIME.Text, kdtpENDTIME.Text, ktxtTRAVEL_REASON.Text);
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
            return string.Format(@"{0}, 出差時間:{1}~{2}, 事由:{3}", ktxtTRAVEL_MEN.Text, kdtpSTARTTIME.Text, kdtpENDTIME.Text, ktxtTRAVEL_REASON.Text);
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
                .AddPerson("TravelerXML", hidTRAVEL_MEN_GUID.Value)
                .AddPerson("TRAVELAGNET", hidTRAVEL_AGENT_GUID.Value)
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.RealValue.rv:{0}", rv));
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
            if (hidIsMobileUI.Value == "Y" &&
                btnMobAgent.Visible)
            {
                //if (btnMobMen.UserSet.Items.Count > 0) // 選擇出差人員
                //{
                //    ktxtTRAVEL_MEN.Text = btnMobMen.UserSet.Items[0].Name;
                //    hidTRAVEL_MEN_GUID.Value = btnMobMen.UserSet.Items[0].Key;
                //    hidTRAVEL_MEN_ACCOUNT.Value = JGlobalLibs.UOFUtils.getUserAccount(hidTRAVEL_MEN_GUID.Value);
                //}
                //else
                //{
                //    ktxtTRAVEL_MEN.Text = "";
                //    hidTRAVEL_MEN_GUID.Value = "";
                //    hidTRAVEL_MEN_ACCOUNT.Value = "";
                //}
                if (btnMobAgent.UserSet.Items.Count > 0)
                {
                    ktxtTRAVEL_AGENT.Text = btnMobAgent.UserSet.Items[0].Name;
                    hidTRAVEL_AGENT_GUID.Value = btnMobAgent.UserSet.Items[0].Key;
                }
                else
                {
                    ktxtTRAVEL_AGENT.Text = "";
                    hidTRAVEL_AGENT_GUID.Value = "";
                }

            }
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
            // 設定Picker是否能輸入
            kdtpSTARTTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
            kdtpENDTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";

            if (!Page.IsPostBack) // 網頁首次載入
            {
                if (!string.IsNullOrEmpty(fieldOptional.FieldValue))
                    kytController.FieldValue = fieldOptional.FieldValue;

                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀
                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值


                //btnCheck.Visible = false; // 隱藏檢查
                btnPLAdd.Visible = false;
                btnItmAdd.Visible = false;
                btnChk.Visible = false;
                btnMen.Visible = false; // 選擇出差人員按鈕
                //btnMobMen.Visible = false; // MOBILEUI版選擇出差人員
                btnAGENT.Visible = false; // 選擇代理人按鈕
                btnMobAgent.Visible = false; // MOBILEUI版選擇代理人按鈕
                hidIsMobileUI.Value = base.MobileUI ? "Y" : "N"; // 判斷現在是否是MobileUI

                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        //hidIsMobileUI.Value = "Y";
                        hidAPIResult.Value = ""; // 清掉先前的旗標
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                        ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                        ktxtAPPLICANTDATE.ReadOnly = true; // 申請日期唯讀
                        ktxtAPPLICANT.ReadOnly = true; // 申請人唯讀
                        btnAGENT.Visible = true; // 顯示選取代理人
                        //btnCheck.Visible = true; // 顯示檢查
                        btnPLAdd.Visible = true;
                        btnItmAdd.Visible = true;
                        ktxtTRAVEL_AGENT.Visible = false; // 代理人
                        ktxtTRAVEL_MEN.Visible = false; // 出差人員
                        if (hidIsMobileUI.Value == "N") // 現在是MOBILEUI之外的畫面
                        {
                            ktxtTRAVEL_MEN.Visible = true; // 出差人員
                            btnMen.Visible = true; // 選擇出差人員按鈕
                            //btnMobMen.Visible = false; // MOBILEUI版選擇出差人員
                            ktxtTRAVEL_AGENT.Visible = true; // 代理人
                            btnAGENT.Visible = true; // 選擇代理人按鈕
                            btnMobAgent.Visible = false; // MOBILEUI版選擇代理人按鈕
                        }
                        else
                        {
                            ktxtTRAVEL_MEN.Visible = false; // 出差人員
                            btnMen.Visible = false; // 選擇出差人員按鈕
                            //btnMobMen.Visible = true; // MOBILEUI版選擇出差人員
                            ktxtTRAVEL_AGENT.Visible = false; // 代理人
                            btnAGENT.Visible = false; // 選擇代理人按鈕
                            btnMobAgent.Visible = true; // MOBILEUI版選擇代理人按鈕
                        }

                        btnChk.Visible = true;
                        if (this.FormFieldMode == FieldMode.Applicant &&
                            string.IsNullOrEmpty(fieldOptional.FieldValue)) // 剛起單
                        {
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            ktxtAPPLICANTDATE.Text = DateTime.Now.ToString("yyyy/MM/dd"); // 設定申請日期
                            ktxtAPPLICANT.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account); // 設定申請人資訊
                            hidAPPLICANT.Value = KUser.UserGUID;
                            hidAPPLICANTAccount.Value = KUser.Account;
                            string[] sAccount = hidAPPLICANTAccount.Value.Split('\\');
                            hidAPPLICANTAccount.Value = sAccount[sAccount.Length - 1];
                            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 設定部門資訊
                            hidAPPLICANTDEPT.Value = KUser.GroupID[0];
                            hidAPPLICANTDEPT_GROUPCODE.Value = KUser.GroupCode[0];
                            ktxtTRAVEL_MEN.Text = ktxtAPPLICANT.Text;
                            hidTRAVEL_MEN_GUID.Value = KUser.UserGUID;
                            hidTRAVEL_MEN_ACCOUNT.Value = KUser.Account;
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
                                if (ex == null &&
                                    dtEmployee.Rows.Count > 0)
                                {
                                    if (dtEmployee.Columns.Contains("TMP_DEPARTID"))
                                    {
                                        hidAPPLICANTDEPT_GROUPCODE.Value = dtEmployee.Rows[0]["TMP_DEPARTID"].ToString();
                                    }
                                }
                            }
                            // 使用UOF代理人設定
                            if (SCSHR.SCSHRConfiguration.AGENT_FUNC.ToUpper() == "AGENT")
                            {
                                UserSet _us = new UserSet();

                                using (SqlDataAdapter sda = new SqlDataAdapter(@"
                                -- 查詢UOF中所設定的代理人
                                SELECT * FROM TB_EB_USER_AGENT WHERE USER_GUID = @USER_GUID
                                ", ConnectionString))
                                using (DataSet ds = new DataSet())
                                {
                                    sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", hidAPPLICANT.Value);
                                    try
                                    {
                                        sda.Fill(ds);
                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {
                                            UserSetUser usu = new UserSetUser();
                                            usu.USER_GUID = (string)dr["AGENT_USER"];
                                            _us.Items.Add(usu);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_TRAVEL SetField SELECT TB_EB_EMPL_DEP ERROR: {0}", e.Message));
                                    }
                                }
                                ubtnAGENT.LimitChoice = Ede.Uof.Common.ChoiceCenter.LimitChoiceList.WithOutUserDept;
                                ubtnAGENT.LimitXML = _us.GetXML();
                            }

                            UserSet us = new UserSet();
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
                                    sda.Fill(ds);
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        UserSetUser usu = new UserSetUser();
                                        usu.USER_GUID = (string)dr["USER_GUID"];
                                        us.Items.Add(usu);
                                    }
                                }
                                catch (Exception e)
                                {
                                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_TRAVEL SetField SELECT TB_EB_EMPL_DEP ERROR: {0}", e.Message));
                                }
                            }

                            utnMen.LimitXML = us.GetXML();
                            // 出差地點
                            kddlTRAVEL_POINT.DataSource = getBOFind("HUM0014100", "*");
                            kddlTRAVEL_POINT.DataValueField = "SYS_VIEWID";
                            kddlTRAVEL_POINT.DataTextField = "SYS_NAME";
                            kddlTRAVEL_POINT.DataBind();
                            // 幣別
                            kddlTRAVELCURR.DataSource = getBOFind("HUM0015000", "*");
                            kddlTRAVELCURR.DataValueField = "SYS_VIEWID";
                            kddlTRAVELCURR.DataTextField = "SYS_NAME";
                            kddlTRAVELCURR.DataBind();

                            //gridview data
                            if (gvPLs.DataTable == null)
                            {
                                gvPLs.DataSource = CreateItems();
                                gvPLs.DataBind();
                            }
                            if (gvItems.DataTable == null)
                            {
                                gvItems.DataSource = CreateItems1();
                                gvItems.DataBind();
                            }

                            //CreateTestData();
                            //CreateTestData1();

                            // 出差單預設出差時間
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

                            // 出差單假別綁定
                            krblDOMESTIC.Items.Clear();
                            JObject joDOMESTIC = JObject.Parse(SCSHRConfiguration.TRAVEL_TYPE);
                            foreach (KeyValuePair<string, JToken> item in joDOMESTIC)
                            {
                                krblDOMESTIC.Items.Add(new ListItem(item.Key, item.Value.ToString()));
                            }
                            if (krblDOMESTIC.Items.Count > 0)
                                krblDOMESTIC.Items[0].Selected = true;
                        }
                        kytController.SetAllViewType(gvPLs, KYTViewType.ReadOnly);
                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Print: // 表單列印
                        break;
                    case FieldMode.Signin: // 表單簽核
                        FormNumber = taskObj.FormNumber;
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.View: // 表單觀看
                        break;
                }
                //hidAPIResult.Value = "OK";
                ktxtSignResult.ViewType = KYTViewType.ReadOnly;
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

    private DataTable getBOFind(string progId, string selectFields)
    {
        DataTable dtScource = new DataTable();
        Exception ex = null;
        dtScource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_TRAVEL.getBOFind.service.Error:{0}", ex.Message));
        DataRow ndr = dtScource.NewRow();
        ndr["SYS_VIEWID"] = "";
        ndr["SYS_NAME"] = "===請選擇===";
        dtScource.Rows.InsertAt(ndr, 0);
        return dtScource;
    }

    private void CreateTestData()
    {
        DataTable tblgvPLs = gvPLs.DataTable;
        DataRow ndr = tblgvPLs.NewRow();
        ndr["PL"] = "A01";
        ndr["TSTARTTIME"] = "2018/08/08";
        ndr["TENDTIME"] = "2018/08/08";
        ndr["TDAYS"] = "1";
        ndr["TCITY"] = "高雄";
        ndr["TBY"] = "高鐵";
        ndr["SP"] = "台北";
        ndr["DP"] = "高雄";
        ndr["REMARK"] = "SCSHR REMARK";
        tblgvPLs.Rows.Add(ndr);
        gvPLs.DataSource = tblgvPLs;
        gvPLs.DataBind();
    }

    private void CreateTestData1()
    {
        DataTable tblgvItems = gvItems.DataTable;
        DataRow ndr = tblgvItems.NewRow();
        ndr["CT"] = "TEL";
        ndr["MEN"] = "A君";
        ndr["PL1"] = "A01";
        ndr["TEAM"] = "";
        ndr["PR"] = "訪談工作";
        ndr["THOPE"] = "進度";
        ndr["PRSS"] = "無";
        ndr["REMARK1"] = "SCSHR REMARK2";
        tblgvItems.Rows.Add(ndr);
        gvItems.DataSource = tblgvItems;
        gvItems.DataBind();
    }

    private DataTable CreateItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("PL", typeof(String))); // 行程規劃
        dt.Columns.Add(new DataColumn("TSTARTTIME", typeof(String))); // 起始日期
        dt.Columns.Add(new DataColumn("TENDTIME", typeof(String))); // 結束日期
        dt.Columns.Add(new DataColumn("TDAYS", typeof(String))); // 出差天數
        dt.Columns.Add(new DataColumn("TCITY", typeof(String))); // 出差城市
        dt.Columns.Add(new DataColumn("TBY", typeof(String))); // 交通工具
        dt.Columns.Add(new DataColumn("SP", typeof(String))); // 出發地點
        dt.Columns.Add(new DataColumn("DP", typeof(String))); // 目的地
        dt.Columns.Add(new DataColumn("REMARK", typeof(String))); // 備註
        return dt;
    }

    private DataTable CreateItems1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("CT", typeof(String))); // 聯絡方式
        dt.Columns.Add(new DataColumn("MEN", typeof(String))); // 拜訪對象
        dt.Columns.Add(new DataColumn("PL1", typeof(String))); // 行程規劃
        dt.Columns.Add(new DataColumn("TEAM", typeof(String))); // 出差人員及分工分式
        dt.Columns.Add(new DataColumn("PR", typeof(String))); // 訪談重點與及預期效果
        dt.Columns.Add(new DataColumn("THOPE", typeof(String))); // 希望授權事項及進度
        dt.Columns.Add(new DataColumn("PRSS", typeof(String))); // 協助他部門辦理事項
        dt.Columns.Add(new DataColumn("REMARK1", typeof(String))); // 備註
        return dt;
    }

    protected void gvPLs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblItems = gvPLs.DataTable; // 取出先前記住的 gvItems
            DataRow row = tblItems.Rows[gr.DataItemIndex]; // 取出對映的資料列
            KYTTextBox ktxtPL = gr.FindControl("ktxtPL") as KYTTextBox; // 行程規劃
            KYTDatePicker kdtpTSTARTTIME = gr.FindControl("kdtpTSTARTTIME") as KYTDatePicker; // 起始日期
            KYTDatePicker kdtpTENDTIME = gr.FindControl("kdtpTENDTIME") as KYTDatePicker; // 結束日期
            KYTTextBox ktxtTDAYS = gr.FindControl("ktxtTDAYS") as KYTTextBox; // 出差天數
            KYTTextBox ktxtTCITY = gr.FindControl("ktxtTCITY") as KYTTextBox; // 出差城市
            DropDownList ddlTBY = gr.FindControl("ddlTBY") as DropDownList; // 交通工具
            KYTTextBox ktxtTBY = gr.FindControl("ktxtTBY") as KYTTextBox; // 工廠
            HtmlGenericControl divTBY = gr.FindControl("divTBY") as HtmlGenericControl;
            KYTTextBox ktxtSP = gr.FindControl("ktxtSP") as KYTTextBox; // 出發地點
            KYTTextBox ktxtDP = gr.FindControl("ktxtDP") as KYTTextBox; // 目的地
            KYTTextBox ktxtREMARK = gr.FindControl("ktxtREMARK") as KYTTextBox; // 備註  
            Button btnPLsDel = gr.FindControl("btnPLsDel") as Button;
            if (this.FormFieldMode == FieldMode.Applicant ||
               this.FormFieldMode == FieldMode.ReturnApplicant)
            {
                btnPLsDel.Visible = true;
                kytController.SetAllViewType(gvPLs, KYTViewType.Input);
                // 交通工具
                ddlTBY.DataSource = getBOFind("HUM0014200", "SYS_ViewID,SYS_Name,SYS_EngName");
                ddlTBY.DataValueField = "SYS_VIEWID";
                ddlTBY.DataTextField = "SYS_NAME";
                ddlTBY.DataBind();
                if (ddlTBY.Items.FindByText(ktxtTBY.Text) != null)
                    ddlTBY.Items.FindByText(ktxtTBY.Text).Selected = true;
                else
                    ddlTBY.SelectedIndex = 0;

                ddlTBY.Visible = true;
                divTBY.Attributes.Add("style", "display:none");
                kdtpTSTARTTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                kdtpTENDTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
            }
            else
            {
                kytController.SetAllViewType(gvPLs, KYTViewType.ReadOnly);
                btnPLsDel.Visible = false;
                ddlTBY.Visible = false;
                divTBY.Attributes.Remove("style");
            }
        }
    }

    protected void gvItems_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblItems = gvItems.DataTable; // 取出先前記住的 gvItems
            DataRow row = tblItems.Rows[gr.DataItemIndex]; // 取出對映的資料列
            KYTTextBox ktxtCT = gr.FindControl("ktxtCT") as KYTTextBox; // 聯絡方式
            KYTTextBox ktxMEN = gr.FindControl("ktxMEN") as KYTTextBox; // 拜訪對象
            KYTTextBox ktxtPL1 = gr.FindControl("ktxtPL1") as KYTTextBox; // 行程規劃
            KYTTextBox ktxtTEAM = gr.FindControl("ktxtTEAM") as KYTTextBox; // 出差人員及分工分式
            KYTTextBox ktxtPR = gr.FindControl("ktxtPR") as KYTTextBox; // 訪談重點與及預期效果
            KYTTextBox ktxtTHOPE = gr.FindControl("ktxtTHOPE") as KYTTextBox; // 希望授權事項及進度
            KYTTextBox ktxtPRSS = gr.FindControl("ktxtPRSS") as KYTTextBox; // 協助他部門辦理事項
            KYTTextBox ktxtREMARK1 = gr.FindControl("ktxtREMARK1") as KYTTextBox; // 備註 
            Button btnItemsDel = gr.FindControl("btnItemsDel") as Button;
            if (this.FormFieldMode == FieldMode.Applicant ||
                this.FormFieldMode == FieldMode.ReturnApplicant)
            {
                kytController.SetAllViewType(gvItems, KYTViewType.Input);
                btnItemsDel.Visible = true;
            }
            else
            {
                kytController.SetAllViewType(gvItems, KYTViewType.ReadOnly);
                btnItemsDel.Visible = false;
            }
        }
    }


    protected void utnMen_EditButtonOnClick(UserSet userSet)
    {
        if (userSet.Items.Count > 0)
        {
            // 只取第一筆
            ktxtTRAVEL_MEN.Text = userSet.Items[0].Name;
            hidTRAVEL_MEN_GUID.Value = userSet.Items[0].Key;
            hidTRAVEL_MEN_ACCOUNT.Value = JGlobalLibs.UOFUtils.getUserAccount(hidTRAVEL_MEN_GUID.Value);
        }
    }


    /// <summary>
    /// UOF選人元件-代理人回傳事件
    /// </summary>
    /// <param name="userSet"></param>
    protected void ubtnAGENT_EditButtonOnClick(UserSet userSet)
    {
        if (userSet.Items.Count > 0)
        {
            ktxtTRAVEL_AGENT.Text = userSet.Items[0].Name;
            hidTRAVEL_AGENT_GUID.Value = userSet.Items[0].Key;
        }
        else
        {
            ktxtTRAVEL_AGENT.Text = "";
            hidTRAVEL_AGENT_GUID.Value = "";
        }
    }

    //protected void btnChk_Click(object sender, EventArgs e)
    //{
    //    lblAPIResultError.Text = "";
    //    hidAPIResult.Value = "";
    //    if (!string.IsNullOrEmpty(kdtpSTARTTIME.Text) &&
    //       !string.IsNullOrEmpty(kdtpENDTIME.Text))
    //    {
    //        DataTable dtResult = null;
    //        bool resultStatus = false;
    //        hidAPIResult.Value = ""; // 清空之前的API查詢結果
    //        // 資料檢核
    //        DateTime dtStart = DateTime.MinValue;
    //        DateTime.TryParse(kdtpSTARTTIME.Text, out dtStart);
    //        DateTime dtEnd = DateTime.MinValue;
    //        DateTime.TryParse(kdtpENDTIME.Text, out dtEnd);
    //        // 表頭
    //        JArray jaTable = new JArray();
    //        JObject _joTable = new JObject();
    //        _joTable.Add(new JProperty("UserNo", "1")); // 自訂序號
    //        _joTable.Add(new JProperty("SYS_VIEWID", "")); // 編號
    //        _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(ktxtAPPLICANTDATE.Text).ToString("yyyyMMdd"))); // 日期
    //        _joTable.Add(new JProperty("TMP_EMPLOYEEID", hidTRAVEL_MEN_ACCOUNT.Value)); // 人員編號
    //        _joTable.Add(new JProperty("TMP_EMPLOYEEID",new KYT_UserPO().GetUserDetailByAccount(hidTRAVEL_MEN_ACCOUNT.Value).EmployeeNo)); // 人員編號
    //        _joTable.Add(new JProperty("TMP_DEPARTID", hidAPPLICANTDEPT_GROUPCODE.Value)); // 部門編號
    //        _joTable.Add(new JProperty("TMP_BTRIPPLACEID", kddlTRAVEL_POINT.SelectedValue)); // 出差地點
    //        _joTable.Add(new JProperty("TMP_CURRENCYID", kddlTRAVELCURR.SelectedValue)); // 幣別
    //        _joTable.Add(new JProperty("PREPAY", ktxtTRAVELMONEY.Text)); // 預支旅費
    //        _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd"))); // 出差起始日期
    //        _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm"))); // 出差起始時間
    //        _joTable.Add(new JProperty("ENDDATE", dtEnd.ToString("yyyyMMdd"))); // 出差結束日期
    //        _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm"))); // 出差結束時間
    //        _joTable.Add(new JProperty("NOTE", ktxtTRAVEL_REASON.Text)); // 備註
    //        jaTable.Add(_joTable);
    //        DataTable dtHeadSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
    //        dtHeadSource.TableName = SCSHRConfiguration.SCSSTravelProgID;
    //        // 表身 SUB
    //        jaTable = new JArray();
    //        for (int i = 0; i < (gvItems.DataSource as DataTable).Rows.Count; i++)
    //        {
    //            DataRow dr = (gvItems.DataSource as DataTable).Rows[i];
    //            _joTable = new JObject();
    //            _joTable.Add(new JProperty("UserNo", "1")); // 自訂序號
    //            _joTable.Add(new JProperty("CONTACTTYPE", dr["CT"].ToString())); // 聯絡方式
    //            _joTable.Add(new JProperty("VISITTARGET", dr["MEN"].ToString())); // 拜訪對象
    //            _joTable.Add(new JProperty("TRIPPLANNING", dr["PL1"].ToString())); // 行程規劃
    //            _joTable.Add(new JProperty("TODO1", dr["TEAM"].ToString())); // 出差人員及分工方式
    //            _joTable.Add(new JProperty("TODO2", dr["PR"].ToString())); // 訪談重點及預期效果
    //            _joTable.Add(new JProperty("TODO3", dr["THOPE"].ToString())); // 希望授權事項及進度
    //            _joTable.Add(new JProperty("TODO4", dr["PRSS"].ToString())); // 協助他部門辦理事項
    //            _joTable.Add(new JProperty("NOTE", dr["REMARK1"].ToString())); // 備註
    //            jaTable.Add(_joTable);
    //        }
    //        DataTable dtSubSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
    //        dtSubSource.TableName = string.Format(@"{0}{1}", SCSHRConfiguration.SCSSTravelProgID, "SUB");
    //        // 表身SUB2
    //        jaTable = new JArray();
    //        for (int i = 0; i < (gvPLs.DataSource as DataTable).Rows.Count; i++)
    //        {
    //            DataRow dr = (gvPLs.DataSource as DataTable).Rows[i];
    //            DateTime dtTStart = DateTime.MinValue;
    //            DateTime dtTEnd = DateTime.MinValue;
    //            DateTime.TryParse(dr["TSTARTTIME"].ToString(), out dtTStart);
    //            DateTime.TryParse(dr["TENDTIME"].ToString(), out dtTEnd);
    //            _joTable = new JObject();
    //            _joTable.Add(new JProperty("UserNo", "1")); // 自訂序號
    //            _joTable.Add(new JProperty("ID", dr["PL"].ToString())); // 行程規劃
    //            _joTable.Add(new JProperty("STARTDATE", dtTStart.ToString("yyyyMMdd"))); // 起始日期
    //            _joTable.Add(new JProperty("ENDDATE", dtTEnd.ToString("yyyyMMdd"))); // 結束日期
    //            _joTable.Add(new JProperty("DAYS", dr["TDAYS"].ToString())); // 出差天數
    //            _joTable.Add(new JProperty("CITY", dr["TCITY"].ToString())); // 出差城市
    //            _joTable.Add(new JProperty("TMP_TRAFFICID", dr["TBY"].ToString())); // 交通工具
    //            _joTable.Add(new JProperty("DEPARTURE", dr["SP"].ToString())); // 出發地點
    //            _joTable.Add(new JProperty("DESTINATION", dr["DP"].ToString())); // 目的地
    //            _joTable.Add(new JProperty("NOTE", dr["REMARK"].ToString())); // 備註
    //            jaTable.Add(_joTable);
    //        }
    //        DataTable dtSub2Source = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
    //        dtSub2Source.TableName = string.Format(@"{0}{1}", SCSHRConfiguration.SCSSTravelProgID, "SUB2");
    //        // 建立DataSet
    //        DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSTravelProgID);
    //        dsSource.Tables.Add(dtHeadSource);
    //        if (dtSubSource.Rows.Count > 0)
    //            dsSource.Tables.Add(dtSubSource);
    //        if (dtSub2Source.Rows.Count > 0)
    //            dsSource.Tables.Add(dtSub2Source);
    //        Exception ex = null;
    //        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.btnChk_Click.dsSource:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dsSource)));

    //        dtResult = service.BOImport(SCSHRConfiguration.SCSSTravelProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
    //        if (ex != null)
    //            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_TRAVEL.btnChk_Click.BOImport.ERROR:{0}", ex.Message));
    //        if (dtResult != null &&
    //            dtResult.Rows.Count > 0)
    //        {
    //            resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
    //            if (!resultStatus)
    //                lblAPIResultError.Text = dtResult.Rows[0]["_MESSAGE"].ToString();
    //        }
    //        hidAPIResult.Value = string.IsNullOrEmpty(lblAPIResultError.Text) ? "OK" : lblAPIResultError.Text;
    //        // 為了滿足檢查完畢後需出現檢查成功
    //        lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查成功" : lblAPIResultError.Text;
    //        //hidAPIResult.Value = "OK";
    //        //lblAPIResultError.Text = "檢查成功";
    //    }
    //    else
    //    {
    //        lblAPIResultError.Text = "無法進行檢核";
    //    }
    //}

    protected void btnChk_Click(object sender, EventArgs e)
    {
        lblAPIResultError.Text = "";
        hidAPIResult.Value = "";
        string LEACODE = "";
        JObject joDOMESTIC = JObject.Parse(SCSHRConfiguration.CREATE_LEV_CODE);
        foreach (KeyValuePair<string, JToken> item in joDOMESTIC)
        {
            if (krblDOMESTIC.SelectedValue.Equals(item.Key)) // 出差類型
            {
                LEACODE = item.Value.ToString();
                break;
            }
        }
        if (!string.IsNullOrEmpty(LEACODE))
        {
            if (!string.IsNullOrEmpty(kdtpSTARTTIME.Text) &&
               !string.IsNullOrEmpty(kdtpENDTIME.Text))
            {
                DataTable dtResult = null;
                #region 檢核飛騰請假單請假時間
                // 資料檢核

                DateTime dtStart = DateTime.MinValue;
                DateTime.TryParse(kdtpSTARTTIME.Text, out dtStart);
                DateTime dtEnd = DateTime.MinValue;
                DateTime.TryParse(kdtpENDTIME.Text, out dtEnd);

                string[] sAccount = hidTRAVEL_MEN_ACCOUNT.Value.Split('\\'); // 出差人
                string leaAgentGUID = hidTRAVEL_AGENT_GUID.Value;

                JArray jaTable = new JArray();
                JObject _joTable = new JObject();
                _joTable.Add(new JProperty("USERNO", "1"));
                _joTable.Add(new JProperty("SYS_VIEWID", ""));
                _joTable.Add(new JProperty("SYS_DATE", DateTime.Now.ToString("yyyyMMdd")));
                //_joTable.Add(new JProperty("TMP_EMPLOYEEID", sAccount[sAccount.Length - 1])); // 取最右邊
                _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByAccount(sAccount[sAccount.Length - 1]).EmployeeNo)); // 取最右邊
                _joTable.Add(new JProperty("TMP_SVACATIONID", LEACODE));
                //_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(leaAgentGUID) ? JGlobalLibs.UOFUtils.getUserAccount(leaAgentGUID) : ""));
                _joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(leaAgentGUID) ? new KYT_UserPO().GetUserDetailByUserGuid(leaAgentGUID).EmployeeNo : ""));
                _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd")));
                _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm")));
                _joTable.Add(new JProperty("ENDDATE", dtEnd.ToString("yyyyMMdd")));
                _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm")));
                _joTable.Add(new JProperty("NOTE", ktxtTRAVEL_REASON.Text.Trim()));

                jaTable.Add(_joTable);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CreateSCSHRLeaveWithTravel.JATABLE:{0}", jaTable));
                DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
                DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
                dsSource.Tables.Add(dtSource);
                Exception ex = null;
                dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CreateSCSHRLeaveWithTravel.BOImport::{0}::Result::{1}", SCSHRConfiguration.SCSSLeaveProgID, Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
                if (ex != null)
                {
                    lblAPIResultError.Text = ex.Message;
                }

                if (dtResult != null &&
                    dtResult.Rows.Count > 0)
                {
                    if (dtResult.Rows[0]["_STATUS"].ToString() != "0")
                    {
                        string msg = dtResult.Rows[0]["_MESSAGE"].ToString();
                        if (msg.Contains("請假")) // 如果回傳訊息包含這個，訊息要加上一行字
                        {
                            lblAPIResultError.Text = string.Format(@"{0}, 該時段與已申請的出差或公出重疊，請確認時間", msg);
                            return;
                        }
                        else
                        {
                            lblAPIResultError.Text = msg;
                            return;
                        }
                    }
                }
                #endregion 檢核飛騰請假單請假時間

                #region 詢問飛騰請假單請假時間
                // 計算時間
                decimal LEAHOURS = 0;
                decimal LEADAYS = 0;

                ex = null; // 初始化
                DataTable dtCalcResult = null;
                SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSGetLeaveHourProgID,
                "GetLeaveHours",
                SCSHR.Types.SCSParameter.getPatameters(new
                {
                    TMP_EmployeeID = sAccount[sAccount.Length - 1],
                    TMP_SVacationID = LEACODE,
                    StartDate = dtStart.ToString("yyyyMMdd"),
                    StartTime = dtStart.ToString("HHmm"),
                    EndDate = dtEnd.ToString("yyyyMMdd"),
                    EndTime = dtEnd.ToString("HHmm")
                }),
                out ex);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CreateSCSHRLeaveWithTravel.BOExecFunc::{0}::Result::{1}", SCSHRConfiguration.SCSSGetLeaveHourProgID, Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));

                if (ex != null)
                {
                    lblAPIResultError.Text = ex.Message;
                }
                if (parameters != null &&
                    parameters.Length > 0)
                {
                    if (parameters[0].DataType.ToString() == "DataTable")
                    {
                        dtCalcResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                    }
                }
                if (dtCalcResult != null &&
                    dtCalcResult.Rows.Count > 0)
                {
                    decimal.TryParse(dtCalcResult.Rows[0]["LeaveHours"].ToString(), out LEAHOURS);
                    decimal.TryParse(dtCalcResult.Rows[0]["LeaveDays"].ToString(), out LEADAYS);
                }
                #endregion 詢問飛騰請假單請假時間
                if (LEAHOURS > 0 || // 有請假時數才可申請請假單
                    LEADAYS > 0)
                {
                    lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查成功" : lblAPIResultError.Text;
                    hidAPIResult.Value = "OK";
                }
                else
                {
                    lblAPIResultError.Text = "沒有請假時數，無法申請";
                }
            }
        }
        else // 不需要請假
        {
            lblAPIResultError.Text = "檢查成功";
            hidAPIResult.Value = "OK";
        }
    }

    protected void btnPLAdd_Click(object sender, EventArgs e)
    {
        DataTable tblgvPLs = gvPLs.DataTable;
        DataRow ndr = tblgvPLs.NewRow();
        ndr["PL"] = "";
        ndr["TSTARTTIME"] = "";
        ndr["TENDTIME"] = "";
        ndr["TDAYS"] = "";
        ndr["TCITY"] = "";
        ndr["TBY"] = "";
        ndr["SP"] = "";
        ndr["DP"] = "";
        ndr["REMARK"] = "";
        tblgvPLs.Rows.Add(ndr);
        gvPLs.DataSource = tblgvPLs;
        gvPLs.DataBind();
    }

    protected void btnItmAdd_Click(object sender, EventArgs e)
    {
        DataTable tblgvItems = gvItems.DataTable;
        DataRow ndr = tblgvItems.NewRow();
        ndr["CT"] = "";
        ndr["MEN"] = "";
        ndr["PL1"] = "";
        ndr["TEAM"] = "";
        ndr["PR"] = "";
        ndr["THOPE"] = "";
        ndr["PRSS"] = "";
        ndr["REMARK1"] = "";
        tblgvItems.Rows.Add(ndr);
        gvItems.DataSource = tblgvItems;
        gvItems.DataBind();
    }


    protected void btnPLsDel_Click(object sender, EventArgs e)
    {
        Button _btnPLsDel = (Button)sender;
        GridViewRow gr = _btnPLsDel.NamingContainer as GridViewRow;
        DataTable tblPLs = gvPLs.DataTable;
        tblPLs.Rows.RemoveAt(gr.DataItemIndex);

        gvPLs.DataSource = tblPLs;
        gvPLs.DataBind();

    }

    protected void btnItemsDel_Click(object sender, EventArgs e)
    {
        Button _btnItemsDel = (Button)sender;
        GridViewRow gr = _btnItemsDel.NamingContainer as GridViewRow;
        DataTable tblItems = gvItems.DataTable;
        tblItems.Rows.RemoveAt(gr.DataItemIndex);

        gvItems.DataSource = tblItems;
        gvItems.DataBind();
    }

    /// <summary>
    /// 變更出差時間(起)事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kdtpSTARTTIME_TextChanged(object sender, EventArgs e)
    {
        hidAPIResult.Value = ""; // 清空API檢查結果
        lblAPIResultError.Text = "";
    }
    /// <summary>
    /// 變更出差時間(迄)事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kdtpENDTIME_TextChanged(object sender, EventArgs e)
    {
        hidAPIResult.Value = ""; // 清空API檢查結果
        lblAPIResultError.Text = "";
    }


    /// <summary>
    /// 出差預定行程規劃-出差天數變更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ktxtTDAYS_TextChanged(object sender, EventArgs e)
    {
        KYTTextBox _ktxtTDAYS = (KYTTextBox)sender;
        string finalStr = "";
        foreach (char _str in _ktxtTDAYS.Text)
        {
            try
            {
                int.Parse(_str.ToString());
                finalStr += _str.ToString();
            }
            catch
            {
                if (_str == '.')
                    finalStr += '.';
            }
        }
        _ktxtTDAYS.Text = finalStr;
    }

    protected void krblDOMESTIC_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTRadioButtonList _krblDOMESTIC = (KYTRadioButtonList)sender;
        hidAPIResult.Value = ""; // 清空API檢查結果
        lblAPIResultError.Text = "";
    }

    [WebMethod]
    public static string CheckSignLevel(string DOC_NBR)
    {
        ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
        string cstring = new DatabaseHelper().Command.Connection.ConnectionString;
        JObject joMessage = new JObject();

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT * FROM Z_SCSHR_TRAVEL WHERE DOC_NBR = @DOC_NBR
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

                    string LEACODE = "";
                    JObject joDOMESTIC = JObject.Parse(SCSHRConfiguration.CREATE_LEV_CODE);
                    foreach (KeyValuePair<string, JToken> item in joDOMESTIC)
                    {
                        if (dr["TRAVELFD"].ToString().Equals(item.Key)) // 出差類型
                        {
                            LEACODE = item.Value.ToString();
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(LEACODE))
                    {
                        DataTable dtResult = null;
                        #region 檢核飛騰請假單請假時間
                        // 資料檢核

                        DateTime dtStart = (DateTime)dr["STARTTIME"];
                        DateTime dtEnd = (DateTime)dr["ENDTIME"];
                        string leaAccount = JGlobalLibs.UOFUtils.getUserAccount(dr["TRAVEL_MEN"].ToString()); // 這樣取就能避免取到domain
                        string leaAgentGUID = dr["AGENT_GUID"].ToString();

                        JArray jaTable = new JArray();
                        JObject _joTable = new JObject();
                        _joTable.Add(new JProperty("USERNO", "1"));
                        _joTable.Add(new JProperty("SYS_VIEWID", ""));
                        _joTable.Add(new JProperty("SYS_DATE", DateTime.Now.ToString("yyyyMMdd")));
                        //_joTable.Add(new JProperty("TMP_EMPLOYEEID", leaAccount));
                        _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByAccount(leaAccount).EmployeeNo));
                        _joTable.Add(new JProperty("TMP_SVACATIONID", LEACODE));
                        //_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(leaAgentGUID) ? JGlobalLibs.UOFUtils.getUserAccount(leaAgentGUID) : ""));
                        _joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(leaAgentGUID) ? new KYT_UserPO().GetUserDetailByUserGuid(leaAgentGUID).EmployeeNo : ""));
                        _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd")));
                        _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm")));
                        _joTable.Add(new JProperty("ENDDATE", dtEnd.ToString("yyyyMMdd")));
                        _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm")));
                        _joTable.Add(new JProperty("NOTE", dr["TRAVEL_REASON"].ToString()));

                        jaTable.Add(_joTable);
                        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CheckSignLevel.JATABLE:{0}", jaTable));
                        DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                        dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
                        DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
                        dsSource.Tables.Add(dtSource);
                        Exception ex = null;
                        dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
                        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CheckSignLevel.BOImport::{0}::Result::{1}", SCSHRConfiguration.SCSSLeaveProgID, Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
                        if (ex != null)
                        {
                            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CheckSignLevel.BOImport.ERROR: {0}", ex.Message));
                            joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                            goto toEnd;
                        }

                        if (dtResult != null &&
                            dtResult.Rows.Count > 0)
                        {
                            if (dtResult.Rows[0]["_STATUS"].ToString() != "0")
                            {
                                string msg = dtResult.Rows[0]["_MESSAGE"].ToString();
                                if (msg.Contains("請假")) // 如果回傳訊息包含這個，訊息要加上一行字
                                {
                                    joMessage.Add(new JProperty("LVHError", string.Format(@"{0}, 該時段與已申請的出差或公出重疊，請確認時間", msg)));
                                    goto toEnd;
                                }
                                else
                                {
                                    joMessage.Add(new JProperty("LVHError", msg));
                                    goto toEnd;
                                }
                            }
                        }
                        #endregion 檢核飛騰請假單請假時間

                        #region 詢問飛騰請假單請假時間
                        // 計算時間
                        decimal LEAHOURS = 0;
                        decimal LEADAYS = 0;

                        ex = null; // 初始化
                        DataTable dtCalcResult = null;
                        SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSGetLeaveHourProgID,
                        "GetLeaveHours",
                        SCSHR.Types.SCSParameter.getPatameters(new
                        {
                            TMP_EmployeeID = leaAccount,
                            TMP_SVacationID = LEACODE,
                            StartDate = dtStart.ToString("yyyyMMdd"),
                            StartTime = dtStart.ToString("HHmm"),
                            EndDate = dtEnd.ToString("yyyyMMdd"),
                            EndTime = dtEnd.ToString("HHmm")
                        }),
                        out ex);
                        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CheckSignLevel.BOExecFunc::{0}::Result::{1}", SCSHRConfiguration.SCSSGetLeaveHourProgID, Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));

                        if (ex != null)
                        {
                            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CheckSignLevel.BOExecFunc.ERROR: {0}", ex.Message));
                            joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                            goto toEnd;
                        }

                        if (parameters != null &&
                            parameters.Length > 0)
                        {
                            if (parameters[0].DataType.ToString() == "DataTable")
                            {
                                dtCalcResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                            }
                        }
                        if (dtCalcResult != null &&
                            dtCalcResult.Rows.Count > 0)
                        {
                            decimal.TryParse(dtCalcResult.Rows[0]["LeaveHours"].ToString(), out LEAHOURS);
                            decimal.TryParse(dtCalcResult.Rows[0]["LeaveDays"].ToString(), out LEADAYS);
                        }
                        #endregion 詢問飛騰請假單請假時間
                        if (LEAHOURS > 0 || // 有請假時數才可申請請假單
                            LEADAYS > 0)
                        {
                        }
                        else
                        {
                            joMessage.Add(new JProperty("LVHError", "沒有請假時數，無法申請"));
                            goto toEnd;
                        }
                    }
                    else // 不需要請假
                    {
                        goto toEnd;
                    }
                }
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CheckSignLevel.SELECT.Z_SCSHR_LEAVE.ERROR: {0}", e.Message));
                joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                goto toEnd;
            }
        }
    toEnd:

        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_TRAVEL.CheckSignLevel.Result:{0}", joMessage.ToString()));
        return joMessage.ToString();
    }
}
