using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

/**
* 修改時間：2021/11/23
* 修改人員：莊仁豪
* 修改項目：
    * 1.加班單搜尋條件由原本以上個月~當天加班時間的加班單改為上個月~當天申請時間的加班單。 
* 修改原因：
    * 1.老楊銷班單找不到加班時間在申請日以後的加班單。
* 修改位置： 
    * 1.「RefreshgvMain() -> SQL -> // 加班單」中，新增條件「APPLICANTDATE BETWEEN @START AND @END」，註解「OT_START與OT_END」。
* **/

/**
* 修改時間：2021/10/25
* 修改人員：梁夢慈
* 修改項目：
    * 1.新增排序日期時間起，由新到舊(DESC)
* 修改原因：
    * 1.新增規格，因應客戶新需求
* 修改位置： 
    * 1.「RefreshgvMain() -> SQL -> // 請假單」中，新增條件「ORDER BY STARTTIME DESC」
    *   「RefreshgvMain() -> SQL -> // 加班單」中，新增條件「ORDER BY OT_START DESC」
* **/

public partial class CDS_SCSHR_WKFFields_QUERYWINDOWS_Search_OT_And_LEA_Form : BasePage
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    private string ConnectionString;

    /// <summary>
    /// 由「加班單/請假單」開啟的跳窗
    /// </summary>
    private string FROM_TYPE;

    /// <summary>
    /// 申請者guid
    /// </summary>
    private string USER_GUID;

    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button2Text = ""; // Button2不顯示
        ((Master_DialogMasterPage)this.Master).Button1Text = ""; // Button1不顯示

        // 取得資料庫連通字串
        ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;

        if (!Page.IsPostBack) // 首次載入網頁
        {
            FROM_TYPE = Request["FROM_TYPE"] != null ? Request["FROM_TYPE"] : "";
            USER_GUID = Request["USER_GUID"] != null ? Request["USER_GUID"] : "";
            ViewState["FROM_TYPE"] = FROM_TYPE;
            ViewState["USER_GUID"] = USER_GUID;

            dateStart.ViewType = KYTViewType.Input;
            dateEnd.ViewType = KYTViewType.Input;

            gvMain.DataSource = RefreshgvMain();
            gvMain.DataBind();
        }
        else // 如果是POSTBACK
        {
            FROM_TYPE = ViewState["FROM_TYPE"].ToString();
            USER_GUID = ViewState["USER_GUID"].ToString();
        }
    }


    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMain.PageIndex = e.NewPageIndex;
        gvMain.DataSource = ViewState[gvMain.ID];
        gvMain.DataBind();
    }

    /// <summary>
    /// 取回事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnGet_Click(object sender, EventArgs e)
    {
        LinkButton lbSelect = sender as LinkButton; // 取得點擊按鈕
        GridViewRow gr = lbSelect.NamingContainer as GridViewRow; // 取得所在GridViewRow
        DataTable dtgvMain = ViewState[gvMain.ID] as DataTable; // 取得先前記住的table
        DataRow row = dtgvMain.Rows[gr.DataItemIndex]; // 取得對映DataRow
        DataTable dtReturn = new DataTable();
        DataRow ndr = dtReturn.NewRow();
        foreach (DataColumn dc in dtgvMain.Columns)
        {
            if (!dtReturn.Columns.Contains(dc.ColumnName))
                dtReturn.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
            if (dc.DataType == typeof(string))
            {
                ndr[dc.ColumnName] = row[dc.ColumnName].ToString().Replace("'", "#&#&##").Replace("\"", "&#&#&&");
            }
            else
            {
                ndr[dc.ColumnName] = row[dc.ColumnName];
            }
        }
        dtReturn.Columns.Add(new DataColumn("FORM_TYPE", typeof(string)));
        ndr["FORM_TYPE"] = FROM_TYPE;
        dtReturn.Rows.Add(ndr);
        Dialog.SetReturnValue2(Newtonsoft.Json.JsonConvert.SerializeObject(dtReturn));
        Dialog.Close(this);
    }

    /// <summary>
    /// 輸入查詢條件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        gvMain.DataSource = RefreshgvMain();
        gvMain.DataBind();
    }

    /// <summary>
    /// 尋找任務
    /// </summary>
    /// <returns></returns>
    private DataTable RefreshgvMain()
    {
        string search = "";
        if (!string.IsNullOrEmpty(txtDOC_NBR.Text)) search += " AND DOC_NBR = @DOC_NBR"; // 表單單號
        if (dateStart.SelectedDate != null) search += string.Format(" AND CONVERT(datetime,{0}) >= @START2", FROM_TYPE == "LEAVE" ? "STARTTIME" : "OT_START"); // 時間(起)
        if (dateEnd.SelectedDate != null) search += string.Format(" AND CONVERT(datetime,{0}) <= @END2", FROM_TYPE == "LEAVE" ? "ENDTIME" : "OT_END"); // 時間(迄)
        DataTable dt = new DataTable();
        if (FROM_TYPE == "LEAVE") // 請假單
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                -- SELECT '===請選擇===' AS 'SHOW', '' AS 'DOC_NBR'
                -- UNION ALL
                SELECT DOC_NBR AS 'SHOW', DOC_NBR, STARTTIME, ENDTIME
                  FROM Z_SCSHR_LEAVE 
                 WHERE (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)
                   AND TASK_STATUS = 2 
                   AND TASK_RESULT = 0
                   AND APPLICANTGUID = @APPLICANTGUID	  
	          ORDER BY STARTTIME DESC 		   			
            " + search, ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", txtDOC_NBR.Text);
                sda.SelectCommand.Parameters.AddWithValue("@APPLICANTGUID", USER_GUID);
                if (dateStart.SelectedDate != null)
                    sda.SelectCommand.Parameters.AddWithValue("@START2", ((DateTime)dateStart.SelectedDate).ToString("yyyy-MM-dd HH:mm"));
                if (dateEnd.SelectedDate != null)
                    sda.SelectCommand.Parameters.AddWithValue("@END2", ((DateTime)dateEnd.SelectedDate).ToString("yyyy-MM-dd HH:mm"));
                try
                {
                    sda.Fill(ds);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Add(new DataColumn("START_TIME", typeof(string)));
                        dt.Columns.Add(new DataColumn("END_TIME", typeof(string)));
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["START_TIME"] = dr["STARTTIME"];
                            dr["END_TIME"] = dr["ENDTIME"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, "SearchForm::RefreshgvMain()::L請假單::錯誤:{0}", ex.Message);
                }
            }
        }
        else if (FROM_TYPE == "OVER_TIME") // 加班單
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                -- SELECT '===請選擇===' AS 'SHOW', '' AS 'DOC_NBR'
                -- UNION ALL
                SELECT DOC_NBR AS 'SHOW', DOC_NBR, OT_START, OT_END
                  FROM Z_SCSHR_OVERTIME 
                 WHERE (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)
                   AND TASK_STATUS = 2 
                   AND TASK_RESULT = 0
                   AND APPLICANTGUID = @APPLICANTGUID
                   AND ((APPLICANTDATE BETWEEN @START AND @END))
               --    AND ((OT_START BETWEEN @START AND @END)
               --         OR (OT_END BETWEEN @START AND @END))
                  -- AND ((OT_START BETWEEN DATEADD(mm, DATEDIFF(mm,0,GETDATE()), 0) -- 加班時間(起)在該月(月底23:59:59.997)
			    --					  AND DATEADD(ms, -2, DATEADD(mm, DATEDIFF(m, 0, GETDATE()) + 1, 0)))
			      --  OR (OT_END BETWEEN DATEADD(mm, DATEDIFF(mm,0,GETDATE()), 0) -- 或加班時間(迄)在該月(月底23:59:59.997)
				    --		 	   AND DATEADD(ms, -2, DATEADD(mm, DATEDIFF(m, 0, GETDATE()) + 1, 0)))               )	
	          ORDER BY OT_START DESC 	
            " + search, ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", txtDOC_NBR.Text);
                sda.SelectCommand.Parameters.AddWithValue("@APPLICANTGUID", USER_GUID);
                sda.SelectCommand.Parameters.AddWithValue("@START", DateTime.Now.AddMonths(-1));
                sda.SelectCommand.Parameters.AddWithValue("@END", DateTime.Now);

                if (dateStart.SelectedDate != null)
                    sda.SelectCommand.Parameters.AddWithValue("@START2", ((DateTime)dateStart.SelectedDate).ToString("yyyy-MM-dd HH:mm"));
                if (dateEnd.SelectedDate != null)
                    sda.SelectCommand.Parameters.AddWithValue("@END2", ((DateTime)dateEnd.SelectedDate).ToString("yyyy-MM-dd HH:mm"));
                try
                {
                    sda.Fill(ds);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Add(new DataColumn("START_TIME", typeof(string)));
                        dt.Columns.Add(new DataColumn("END_TIME", typeof(string)));
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["START_TIME"] = dr["OT_START"];
                            dr["END_TIME"] = dr["OT_END"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, "SearchForm::RefreshgvMain()::O加班單::錯誤:{0}", ex.Message);
                }
            }
        }
        ViewState[gvMain.ID] = dt;
        return dt;
    }

    /// <summary>
    /// 查詢
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSEARCH_Click(object sender, EventArgs e)
    {
        gvMain.DataSource = RefreshgvMain();
        gvMain.DataBind();
    }


}