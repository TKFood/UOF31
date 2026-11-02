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
    * 「btnSearch_Click」中，查詢〈ATT0021700〉的結果註解
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
* 修改時間：2019/12/23
* 修改人員：高常昇
* 修改項目：
    * BUG修正
* 發生原因：
    * 1.「IE」無法使用字元符號「`」
* 修改位置：
    * 1.「前端網頁中」的「`」修改為「"」
* **/

/**
* 修改時間：2019/11/22
* 修改人員：高常昇
* 修改項目：
    * BUG修正
* 發生原因：
    * 1.「gvItems_RowDataBound」中的B2出勤狀態寫錯
* 修改位置：
    * 1.「lblOFFWORKTIMESTATUS」寫成「lblWORKTIMESTATUS」，修正便可;
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
/// 個人出缺勤狀況查詢
/// </summary>
public partial class WB_KYTI_SCSHR_DUTY_REPORT : BasePage
{

    private string ConnectionString;

    SCSServicesProxy service = null;
  

    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        service = ConstructorCommonSettings.setSCSSServiceProxDefault();

        if (!Page.IsPostBack) // 網頁首次載入
        {
            DateTime Current = DateTime.Today;
            this.kdtpSTARTTIME.SelectedDate = new DateTime(Current.Year, Current.Month, 1);
            this.kdtpENDTIME.SelectedDate = new DateTime(Current.Year, Current.Month, DateTime.DaysInMonth(Current.Year, Current.Month));
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
            //                 _UC_SearchGroupWithGroup.GroupID = user.GroupID;
            //                 _UC_SearchGroupWithGroup.Visible = true;
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
                        //_UC_SearchGroupWithGroup.GroupID = ds.Tables[0].Rows[0]["GROUP_ID"].ToString();
                        //_UC_SearchGroupWithGroup.Visible = true;
                    }

                }
                catch (Exception ex)
                {
                    JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_ADV_LEAVE_REPORT.Page_Load.SELECT.FN_EB_CheckUserIsInRole.ERROR: 「{0}」；ERROR LINE: 「{1}」", ex.Message, ex.StackTrace));
                }
            }
        }
    }

    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            switch (((HiddenField)gr.FindControl("hidBOVERTIMESTATUS")).Value) //出勤狀態(A1)
            {
                case "0": ((Label)gr.FindControl("lblBOVERTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblBOVERTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblBOVERTIMESTATUS")).Text = "早到"; break;
                case "3": ((Label)gr.FindControl("lblBOVERTIMESTATUS")).Text = "加班上班未刷卡"; break;
            }
            switch (((HiddenField)gr.FindControl("hidBOFFOVERTIMESTATUS")).Value) //出勤狀態(A2)
            {
                case "0": ((Label)gr.FindControl("lblBOFFOVERTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblBOFFOVERTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblBOFFOVERTIMESTATUS")).Text = "加班下班未刷卡"; break;
            }
            switch (((HiddenField)gr.FindControl("hidBSTATUS")).Value) //加班狀態(A)
            {
                case "0": ((Label)gr.FindControl("lblBSTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblBSTATUS")).Text = "無加班單"; break;
                case "2": ((Label)gr.FindControl("lblBSTATUS")).Text = "有加班單"; break;
            }
            switch (((HiddenField)gr.FindControl("hidWORKTIMESTATUS")).Value) //出勤狀態(B1)
            {
                case "0": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "未刷卡"; break;
                case "3": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "遲到"; break;
                case "4": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "上班未刷卡"; break;
                case "5": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "免刷卡"; break;
                case "6": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "休息日"; break;
                case "7": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "休息日加班"; break;
                case "8": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "晚到(要請假)"; break;
            }
            string keyB1 = ((HiddenField)gr.FindControl("hidWORKTIMESTATUS")).Value;
            ((Label)gr.FindControl("lblWORKTIMESTATUS")).ForeColor = keyB1 != "0" && keyB1 != "1" && keyB1 != "6" ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            switch (((HiddenField)gr.FindControl("hidSTATUS")).Value) //處理狀態(B1)
            {
                case "0": ((Label)gr.FindControl("lblSTATUS")).Text = "未處理"; break;
                case "1": ((Label)gr.FindControl("lblSTATUS")).Text = "已處理"; break;
                case "2": ((Label)gr.FindControl("lblSTATUS")).Text = "正常"; break;
            }
            switch (((HiddenField)gr.FindControl("hidOFFWORKTIMESTATUS")).Value) //出勤狀態(B2)
            {
                case "0": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "未刷卡"; break;
                case "3": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "早退"; break;
                case "4": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "下班未刷卡"; break;
                case "5": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "免刷卡"; break;
                case "6": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "休息日"; break;
                case "7": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "休息日加班"; break;
                case "8": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "晚到(要請假)"; break;
            }
            string keyB2 = ((HiddenField)gr.FindControl("hidOFFWORKTIMESTATUS")).Value;
            ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).ForeColor = keyB2 != "0" && keyB2 != "1" && keyB2 != "6" ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            switch (((HiddenField)gr.FindControl("hidSTATUS2")).Value) //處理狀態(B2)
            {
                case "0": ((Label)gr.FindControl("lblSTATUS2")).Text = "未處理"; break;
                case "1": ((Label)gr.FindControl("lblSTATUS2")).Text = "已處理"; break;
                case "2": ((Label)gr.FindControl("lblSTATUS2")).Text = "正常"; break;
            }
            switch (((HiddenField)gr.FindControl("hidAOVERTIMESTATUS")).Value) //出勤狀態(C1)
            {
                case "0": ((Label)gr.FindControl("lblAOVERTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblAOVERTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblAOVERTIMESTATUS")).Text = "加班上班未刷卡"; break;
            }
            switch (((HiddenField)gr.FindControl("hidAOFFOVERTIMESTATUS")).Value) //出勤狀態(C2)
            {
                case "0": ((Label)gr.FindControl("lblAOFFOVERTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblAOFFOVERTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblAOFFOVERTIMESTATUS")).Text = "晚退"; break;
                case "3": ((Label)gr.FindControl("lblAOFFOVERTIMESTATUS")).Text = "加班下班未刷卡"; break;
            }
            switch (((HiddenField)gr.FindControl("hidASTATUS")).Value) //加班狀態(C)
            {
                case "0": ((Label)gr.FindControl("lblASTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblASTATUS")).Text = "無加班單"; break;
                case "2": ((Label)gr.FindControl("lblASTATUS")).Text = "有加班單"; break;
            }
        }
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
        if (kdtpSTARTTIME.SelectedDate == null || kdtpENDTIME.SelectedDate == null || string.IsNullOrEmpty(lblAccount.Text))
        {
            gvItems.DataSource = CreateGvitem();
            gvItems.DataBind();
            return;
        }
        string accounts = lblAccount.Text;
        //UserSet US = UC_ChoiceListMobile.UserSet;
        //if (US.GetAllUsers().Rows.Count > 0)
        //{
        //    accounts = "";
        //    foreach (DataRow dr in US.GetAllUsers().Rows)
        //    {
        //        EBUser user = new UserUCO().GetEBUser(dr["USER_GUID"].ToString());
        //        if (!string.IsNullOrEmpty(accounts)) accounts += ";";
        //        accounts += user.Account;
        //    }
        //}
        Exception ex = null; // 初始化
        DataTable dtResult = null;
        SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(
        "ATT0021700",
        "GetAttendData_Web",
        SCSHR.Types.SCSParameter.getPatameters(new
        {
            StartDate = ((DateTime)kdtpSTARTTIME.SelectedDate).ToString("yyyyMMdd"),
            EndDate = ((DateTime)kdtpENDTIME.SelectedDate).ToString("yyyyMMdd"),
            CalcCHours = ddlHours.SelectedValue,
            ShowAbnormal = ddlAbnorma.SelectedValue,
            EmployeeID = accounts
        }),
        out ex);
        //JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_DUTY_REPORT.btnSearch_Click.BOExecFunc::{0}::Send::{1}", "ATT0021700", ((DateTime)kdtpSTARTTIME.SelectedDate).ToString("yyyyMMdd") + " , " + ((DateTime)kdtpENDTIME.SelectedDate).ToString("yyyyMMdd") + " , " + ddlHours.SelectedValue + " , " + ddlAbnorma.SelectedValue + " , " + accounts));
        //JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_DUTY_REPORT.btnSearch_Click.BOExecFunc::{0}::Result::{1}", "ATT0021700", Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));

        if (ex != null)
            JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_DUTY_REPORT.btnSearch_Click.BOExecFunc.ERROR:{0}", ex.Message));
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
            dv.Sort = "EMPLOYEEVIEWID";
            ViewState["gvitems"] = dv.ToTable();
            gvItems.DataSource = dv;
            gvItems.DataBind();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), "SetGvItems()", true);
            return;
        }
        ViewState["gvitems"] = dtResult;
        gvItems.DataSource = dtResult;
        gvItems.DataBind();
    }

    private DataTable CreateGvitem()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("EMPLOYEEVIEWID", typeof(String))); // 員工工號
        dt.Columns.Add(new DataColumn("EMPLOYEENAME", typeof(String))); // 員工姓名
        dt.Columns.Add(new DataColumn("DEPARTVIEWID", typeof(String))); // 部門編號
        dt.Columns.Add(new DataColumn("DEPARTNAME", typeof(String))); // 部門名稱
        dt.Columns.Add(new DataColumn("ATTENDDATE", typeof(String))); // 出勤日期
        dt.Columns.Add(new DataColumn("TMP_WORKID", typeof(String))); // 出勤班別代碼
        dt.Columns.Add(new DataColumn("TMP_WORKNAME", typeof(String))); // 出勤班別
        dt.Columns.Add(new DataColumn("CARDNO", typeof(String))); // 刷卡卡號
        dt.Columns.Add(new DataColumn("BOVERTIME", typeof(String))); // 出勤時間(A1)
        dt.Columns.Add(new DataColumn("BOVERTIMESTATUS", typeof(String))); // 出勤狀態(A1)
        dt.Columns.Add(new DataColumn("BOFFOVERTIME", typeof(String))); // 出勤時間(A2)
        dt.Columns.Add(new DataColumn("BOFFOVERTIMESTATUS", typeof(String))); // 出勤狀態(A2)
        dt.Columns.Add(new DataColumn("BSTATUS", typeof(String))); // 加班狀態(A)
        dt.Columns.Add(new DataColumn("WORKTIME", typeof(String))); // 出勤時間(B1) 
        dt.Columns.Add(new DataColumn("WORKTIMESTATUS", typeof(String))); // 出勤狀態(B1) 
        dt.Columns.Add(new DataColumn("STATUS", typeof(String))); // 處理狀態(B1) 
        dt.Columns.Add(new DataColumn("OFFWORKTIME", typeof(String))); // 出勤時間(B2) 
        dt.Columns.Add(new DataColumn("OFFWORKTIMESTATUS", typeof(String))); // 出勤狀態(B2) 
        dt.Columns.Add(new DataColumn("STATUS2", typeof(String))); // 處理狀態(B2) 
        dt.Columns.Add(new DataColumn("AOVERTIME", typeof(String))); // 出勤時間(C1) 
        dt.Columns.Add(new DataColumn("AOVERTIMESTATUS", typeof(String))); // 出勤狀態(C1) 
        dt.Columns.Add(new DataColumn("AOFFOVERTIME", typeof(String))); // 出勤時間(C2) 
        dt.Columns.Add(new DataColumn("AOFFOVERTIMESTATUS", typeof(String))); // 出勤狀態(C2) 
        dt.Columns.Add(new DataColumn("ASTATUS", typeof(String))); // 加班狀態(C) 
        dt.Columns.Add(new DataColumn("CHOURS", typeof(String))); // 缺勤時數 
        dt.Columns.Add(new DataColumn("LHOURS", typeof(String))); // 請假時數 
        dt.Columns.Add(new DataColumn("FHOURS", typeof(String))); // 簽核中的請假時數 
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
            WebUtils.GridViewExportToExcel(stream, "個人出缺勤狀況查詢", gvItems, "微軟正黑體", "A1");
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