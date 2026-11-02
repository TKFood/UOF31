using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using KYTLog;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


/**
* 修改時間：2021/06/04
* 修改人員：梁夢慈
* 修改項目：
    * 1. 顯示排序依加班日期小到大排序
    * 2. 簽核結果，新增判斷顯示，如果已銷單顯示"銷班"
* 修改原因：
    * 1~2. 新增規格
* 修改位置：
    * 1.「RefreshMainData()」中，SQL語法中，新增ORDER BY OT_DATE ASC
    * 2.「RefreshMainData()」中，SQL語法中，新增CASE條件判斷「WHEN (CANCEL_DOC_NBR != '' AND CANCEL_DOC_NBR IS NOT NULL)  THEN '銷班'」
* **/

/**
* 修改時間：2021/06/02
* 修改人員：梁夢慈
* 修改項目：
    * 1. 改為顯示加班(起)，前後1月的加班單
* 修改原因：
    * 1. 修改規格
* 修改位置：
    * 1.「Page_Load()」中，新增接收參數START_DATE
    *   「RefreshMainData()」中，SQL條件中，OT_START範圍改為「START_DATE」日期，並註解掉OT_END的條件
* **/

public partial class CDS_SCSHR_WKFFields_QUERYWINDOWS_findOVERTIME : BasePage
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    private string ConnectionString;

    private string START_DATE;

    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button2Text = ""; // Button2不顯示
        ((Master_DialogMasterPage)this.Master).Button1Text = ""; // Button1不顯示
        // 取出資料庫連通字串
        ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值

        if (!Page.IsPostBack) // 首次載入網頁
        {
            string ACCOUNT = Request["ACCOUNT"] != null ? Request["ACCOUNT"] : "";
            string START_DATE = Request["START_DATE"] != null ? Request["START_DATE"] : "";
            ViewState["ACCOUNT"] = ACCOUNT;
            ViewState["START_DATE"] = START_DATE;
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"findOVERTIME.ACCOUNT:{0}", ACCOUNT));
            RefreshMainData(ACCOUNT);
        }
        else // 如果是POSTBACK
        {
        }
    }

    private void RefreshMainData(string ACCOUNT)
    {
        if (!string.IsNullOrEmpty(ACCOUNT))
        {
            DateTime start = DateTime.Now;
            DateTime.TryParse(ViewState["START_DATE"].ToString(), out start);
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                -- 找前後一個月範圍內的單；並且找到所有尚未結案的單
                SELECT APPLICANT AS ACCOUNT,
	                   ISNULL((SELECT [NAME] 
				                 FROM TB_EB_USER 
				                WHERE ACCOUNT = Z_SCSHR_OVERTIME.APPLICANT), '') AS 'USER_NAME',
		                CONVERT(nvarchar, OT_START, 111) AS 'OT_DATE',
		                OT_START,
		                OT_END,
		                OT_TIMES,
		                CASE CHANGETYPE 
		                WHEN '0' THEN '加班費'
		                WHEN '1' THEN '補休'
		                WHEN '2' THEN '加班費及補休'
		                ELSE '錯誤' END AS 'CHANGETYPE_DESC',
		                CASE 
		                WHEN (CANCEL_DOC_NBR != '' AND CANCEL_DOC_NBR IS NOT NULL)  THEN '銷班'
		                WHEN TASK_STATUS = 1 AND TASK_RESULT IS NULL THEN '簽核中'
		                WHEN TASK_STATUS = 2 AND TASK_RESULT = 0 THEN '同意'
		                WHEN TASK_STATUS = 2 AND TASK_RESULT = 1 THEN '否決'
		                ELSE '作廢' END AS 'SIGN_RESULT'
                  FROM Z_SCSHR_OVERTIME 
                 WHERE APPLICANT = @APPLICANT
                   AND (
		                (OT_START BETWEEN DATEADD(MONTH, -1, @START)  -- 加班(起)時間在前後一個月內
					                  AND DATEADD(MONTH, 1, @START)) 
		                --OR (OT_END BETWEEN DATEADD(MONTH, -1, GETDATE()) -- 或者 加班(迄)時間在前後一個月內
					    --               AND DATEADD(MONTH, 1, GETDATE()))
	                  OR TASK_RESULT IS NULL) -- 或者尚未結案的
              ORDER BY OT_DATE ASC
                ", ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@APPLICANT", ACCOUNT);
                sda.SelectCommand.Parameters.AddWithValue("@START", start);
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        gvMain.DataSource = ds.Tables[0];
                        gvMain.DataBind();
                    }
                }
                catch (Exception e)
                {
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"findOVERTIME.RefreshMainData.ACCOUNT:{0}.ERROR:{1}", ACCOUNT, e.Message));
                }
            }


        }
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        DataTable ALLLeave = gvMain.DataSource as DataTable;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataRow dr = ALLLeave.Rows[gr.DataItemIndex];
            // do nothing
        }
    }
}
