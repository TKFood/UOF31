using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using JGlobalLibs;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
* 修改原因：
    * 1.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
* 修改位置： 
    * 1.「Page_Load()、UC_ChoiceList1_EditButtonOnClick()」中，註解所有EBUser，改為KYT_EBUser
* **/

/**
* 修改時間：2020/04/01
* 修改人員：陳緯榕
* 修改項目：
    * JGlobalLibs.DebugLog寫ERROR以外的訊息都註解掉
* 發生原因：
    * 無印這便訊息寫太多，可能是造成LOG寫入錯誤的理由之一，沒時間完整的把飛騰內所有的JGlobalLibs淘汰掉，所以先處理LOG部分
* 修改位置：
    * 「btnSearch_Click」中，查詢〈ATT0013400〉的結果註解
* **/

/**
* 修改時間：2020/03/10
* 修改人員：陳緯榕
* 修改項目：
    * 有掛 "HR管理者" 的人 可以選全部人
* 發生原因：
    * 新規格
* 修改位置：
    * 「Page_Load」中，當〈網頁首次載入〉時，判斷是否要顯示選人元件，並設定選人元件最上層部門的指定
* **/

/**
* 修改時間：2019/09/24
* 修改人員：高常昇
* 修改項目：
    * BUG修正
* 發生原因：
    * 1.只會正確帶出Date，不會帶出TIME，因為飛騰是將DATE跟TIME分開傳，另外飛騰提供的XML轉Datable沒有考慮到此狀況
* 修改位置：
    * 1.「btnSearch_Click」於取得資料後，做後續修正DataTable dt;
* **/

/// <summary>
/// 個人預支假查詢
/// </summary>
public partial class WB_KYTI_SCSHR_ADV_LEAVE_REPORT : BasePage
{
    private string ConnectionString;

    SCSServicesProxy service = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        service = ConstructorCommonSettings.setSCSSServiceProxDefault();

        if (!Page.IsPostBack) // 網頁首次載入
        {
            //EBUser user = new UserUCO().GetEBUser(Page.User.Identity.Name);
            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(Page.User.Identity.Name); // 人員
            string[] Account = KUser.Account.Split('\\');
            if (Account.Length > 1) lblAccount.Text = Account[1];
            else lblAccount.Text = Account[0];
            //     //檢查是否為部門主管
            //     using (SqlDataAdapter sda = new SqlDataAdapter(@"
            //         SELECT *
            //           FROM TB_EB_EMPL_FUNC
            //          WHERE FUNC_ID = 'Superior'  
            //AND USER_GUID = @GUID
            //     ", ConnectionString))
            //     using (DataSet ds = new DataSet())
            //     {
            //         sda.SelectCommand.Parameters.AddWithValue("@GUID", user.UserGUID); //大類名稱
            //         try
            //         {
            //             sda.Fill(ds);
            //             if (ds.Tables[0].Rows.Count > 0) //是部門主管
            //             {
            //                 //UC_ChoiceList1.Visible = true;
            //                 //UC_ChoiceListMobile.Visible = true;
            //                 //UserSet us = new UserSet();
            //                 //UserSetGroup usg = new UserSetGroup();
            //                 //usg.GROUP_ID = user.GroupID;
            //                 //usg.IS_DEPTH = true;
            //                 //us.Items.Add(usg);
            //                 //UC_ChoiceList1.LimitXML = us.GetXML();
            //                 //UC_ChoiceList1.ChioceType = Ede.Uof.Common.ChoiceCenter.ChoiceTypeList.Group;
            //                 //UC_ChoiceList1.LimitChoice = Ede.Uof.Common.ChoiceCenter.LimitChoiceList.WithUserDept;
            //                 //UC_ChoiceList1.ExpandToUser = true;
            //                 //UC_ChoiceList1.LimitChoice = Ede.Uof.Common.ChoiceCenter.LimitChoiceList.DeptManager;
            //                 //UC_ChoiceListMobile.LimitXml = us.GetXML();
            //                 //UC_ChoiceListMobile.LimitChoice = Ede.Uof.Common.ChoiceCenter.LimitChoiceList.WithUserDept;
            //                 //Dialog.Open2(btnWeekSelect, string.Format(@"~/CDS/MUJI/WKFFields/Dialog/DialogWeek.aspx"), "選擇週別", 800, 530, Dialog.PostBackType.AfterReturn, new { }.ToExpando());
            //                 _UC_SearchGroupWithGroup.GroupID = user.GroupID;
            //                 _UC_SearchGroupWithGroup.Visible = true;
            //                 _UC_SearchGroupWithGroup.isMobile = true;
            //             }
            //         }
            //         catch { }
            //     }

            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                IF ((SELECT dbo.FN_EB_CheckUserIsInRole ('DutyManager',@USER_GUID)) = '1') -- 判斷該帳號是否是HR管理者
	                BEGIN
		                SELECT TOP 1 GROUP_ID -- 取得最上層的部門
		                  FROM TB_EB_GROUP 
		                 WHERE PARENT_GROUP_ID IS NULL
	                END
                ELSE
	                BEGIN
		                SELECT * -- 取得擔任部門主管的部門
		                  FROM TB_EB_EMPL_DEP 
		                 WHERE GROUP_ID IN (SELECT GROUP_ID
						                      FROM TB_EB_EMPL_FUNC
						                     WHERE FUNC_ID = 'Superior'  
						                       AND USER_GUID = @USER_GUID)
		                   AND USER_GUID = @USER_GUID
		                   AND ORDERS = 0
	                END
                ", ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", KUser.UserGUID);
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        _UC_SearchGroupWithGroup.GroupID = ds.Tables[0].Rows[0]["GROUP_ID"].ToString();
                        _UC_SearchGroupWithGroup.Visible = true;
                        _UC_SearchGroupWithGroup.isMobile = true;
                    }
                    
                }
                catch (Exception ex)
                {
                    JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_ADV_LEAVE_REPORT.Page_Load.SELECT.FN_EB_CheckUserIsInRole.ERROR: 「{0}」；ERROR LINE: 「{1}」",ex.Message, ex.StackTrace));
                }
            }
        }
    }

    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    /// <summary>
    /// 按下選擇帳號
    /// </summary>
    protected void UC_ChoiceList1_EditButtonOnClick(UserSet userSet)
    {
        if (userSet.Items.Count <= 0) return;
        //EBUser user = new UserUCO().GetEBUser(Page.User.Identity.Name);
        KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(Page.User.Identity.Name); // 人員
        //檢查是否為部門主管
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT *
                  FROM TB_EB_EMPL_FUNC
                 WHERE FUNC_ID = 'Superior'
				   AND USER_GUID = @GUID
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@GUID", KUser.UserGUID); //大類名稱
            try
            {
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0) //是部門主管
                {
                    lblAccount.Text = "";
                    string[] Account = userSet.Items[0].EBUsers[0].Account.Split('\\');
                    if (Account.Length > 1) lblAccount.Text = Account[1];
                    else lblAccount.Text = Account[0];
                    for (int i = 1; i < userSet.Items.Count; i++)
                    {
                        lblAccount.Text += ";";
                        Account = userSet.Items[i].EBUsers[0].Account.Split('\\');
                        if (Account.Length > 1) lblAccount.Text += Account[1];
                        else lblAccount.Text += Account[0];
                    }
                }
            }
            catch { }
        }
    }


    /// <summary>
    /// 按下查詢按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtYear.Text) || string.IsNullOrEmpty(lblAccount.Text))
        {
            gvItems.DataSource = CreateGvitem();
            gvItems.DataBind();
            return;
        }
        string accounts = lblAccount.Text;

        Exception ex = null; // 初始化
        DataTable dtResult = null;
        SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(
        "ATT0013400",
        "GetAdvLeaveData_Web",
        SCSHR.Types.SCSParameter.getPatameters(new
        {
            AdvYear = txtYear.Text,
            LeaveEmp = ddlLeaveEmp.SelectedValue,
            EmployeeID = accounts
        }),
        out ex);
        //JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_ADV_LEAVE_REPORT.btnSearch_Click.BOExecFunc::{0}::Send::{1}", "ATT0013400", txtYear.Text + " , " + ddlLeaveEmp.SelectedValue + " , " + accounts));
        //JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_ADV_LEAVE_REPORT.btnSearch_Click.BOExecFunc::{0}::Result::{1}", "ATT0013400", Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));

        if (ex != null)
            JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_ADV_LEAVE_REPORT.btnSearch_Click.BOExecFunc.ERROR:{0}", ex.Message));
        if (parameters != null &&
            parameters.Length > 0)
        {
            if (parameters[0].DataType.ToString() == "DataTable")
            {
                //DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOExecFunc.Result.XML:{0}", parameters[0].Xml));

                dtResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
            }
        }

        DataTable dt = new DataTable();
        foreach (DataColumn dc in dtResult.Columns)
        {
            dt.Columns.Add(new DataColumn(dc.ColumnName, typeof(String)));
        }
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            DataRow dr = dtResult.Rows[i];
            DataRow row = dt.NewRow();
            foreach (DataColumn dc in dtResult.Columns)
            {
                row[dc.ColumnName] = dr[dc.ColumnName];
                if (dc.DataType == typeof(DateTime) && dc.ColumnName.Length >= 4)
                {
                    string title = string.Format("{0}TIME", dc.ColumnName.Substring(0, dc.ColumnName.Length - 4));
                    string date = row[dc.ColumnName].ToString();
                    if (dtResult.Columns.Contains(title))
                    {
                        string time = dr[title].ToString();
                        row[dc.ColumnName] = string.Format("{0} {1}", date.Length > 10 ? date.Substring(0, 10) : date, time.Length >= 3 ? time.Insert(2, ":") : time);
                    }
                    else
                    {
                        row[dc.ColumnName] = date.Length > 10 ? date.Substring(0, 10) : date;
                    }
                }
            }
            dt.Rows.Add(row);
        }
        if (dtResult != null)
        {
            DataView dv = new DataView(dt);
            dv.Sort = "TMP_EMPLOYEEID";
            ViewState["gvitems"] = dv.ToTable();
            gvItems.DataSource = dv;
            gvItems.DataBind();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), "LimitHeight()", true);
            return;
        }
        ViewState["gvitems"] = dtResult;
        gvItems.DataSource = dtResult;
        gvItems.DataBind();
    }

    private DataTable CreateGvitem()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("TMP_EMPLOYEEID", typeof(String))); // 員工工號
        dt.Columns.Add(new DataColumn("TMP_EMPLOYEENAME", typeof(String))); // 員工姓名
        dt.Columns.Add(new DataColumn("TMP_DEPARTID", typeof(String))); // 部門編號
        dt.Columns.Add(new DataColumn("TMP_DEPARTNAME", typeof(String))); // 部門名稱
        dt.Columns.Add(new DataColumn("ADVYEAR", typeof(String))); // 預支假年度
        dt.Columns.Add(new DataColumn("SENIORITY", typeof(String))); // 年資
        dt.Columns.Add(new DataColumn("ADVRANGE", typeof(String))); // 期數
        dt.Columns.Add(new DataColumn("ADVHOURS", typeof(String))); // 可預支時數
        dt.Columns.Add(new DataColumn("CHANGEHOURS", typeof(String))); // 增減可預支時數
        dt.Columns.Add(new DataColumn("LEAVEHOURS", typeof(String))); // 已預支時數
        dt.Columns.Add(new DataColumn("RHOURS", typeof(String))); // 剩餘時數
        dt.Columns.Add(new DataColumn("CLOSEHOURS", typeof(String))); // 結清時數
        dt.Columns.Add(new DataColumn("ALLOWHOURS", typeof(String))); // 轉扣款時數
        dt.Columns.Add(new DataColumn("HLEAVEHOURS", typeof(String))); // 抵(輪休)時數 
        dt.Columns.Add(new DataColumn("SLEAVEHOURS", typeof(String))); // 抵(特休)時數
        dt.Columns.Add(new DataColumn("OLEAVEHOURS", typeof(String))); // 抵(加班補休)時數
        dt.Columns.Add(new DataColumn("RHOURS2", typeof(String))); // 尚未折抵時數
        dt.Columns.Add(new DataColumn("STARTDATE", typeof(String))); // 可預支起始日期
        dt.Columns.Add(new DataColumn("ENDDATE", typeof(String))); // 可預支結束日期
        ViewState["gvitems"] = dt;
        return dt;
    }

    /// <summary>
    /// 按下載按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDLExcel_Click(object sender, EventArgs e)
    {
        if (ViewState["gvitems"] == null)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), @"
                alert('無資料可供匯出');
            ", true);
            return;
        }
        DataTable objtable = ViewState["gvitems"] as DataTable;
        if (objtable.Rows.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), @"
                alert('無資料可供匯出');
            ", true);
            return;
        }
        string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString() + ".xlsx";
        string folder = Server.MapPath("~/CDS/SCSHR/Temp/");
        if (!Directory.Exists(folder))
        {
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch { }
        }
        string filePath = folder + filename;
        using (MemoryStream stream = new MemoryStream())
        {
            WebUtils.GridViewExportToExcel(stream, "個人預支假查詢", gvItems, "微軟正黑體", "A1");
            stream.Seek(0, SeekOrigin.Begin);
            File.WriteAllBytes(filePath, stream.ToArray());
        }

        ScriptManager.RegisterClientScriptBlock(
           UpdatePanel1,
           UpdatePanel1.GetType(),
           Guid.NewGuid().ToString(),
           string.Format(@"
                    document.addEventListener('DOMContentLoaded', function() {{
                        window.location = '{0}?filepath={1}';
                    }});
                "
           , Page.ResolveUrl("~/CDS/SCSHR/WKFFields/FORMPRINT/DownFileWithPath.ashx")
           , HttpUtility.UrlEncode(filePath)),
           true);
    }

    protected void _UC_SearchGroupWithGroup_DialogReturn(object sender, string result)
    {
        DataTable dtResult = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(result);
        GridViewRow gr = ((Button)sender).Parent.Parent.Parent as GridViewRow;
        if (dtResult == null) return;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            sb.Append(dtResult.Rows[i]["ACCOUNT"]);
            if (i < dtResult.Rows.Count - 1) sb.Append(';');
        }
        this.lblAccount.Text = sb.ToString();
    }
}