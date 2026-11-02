using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using UOFAssist.WKF;

/**
* 修改時間：2021/01/23
* 修改人員：陳緯榕
* 修改項目：
    * 基礎完成版
* 修改原因：
    * 原本的在資料量大的時候太慢了
* 修改位置：
    * 新增「訊息區間」，會讓「時間區間無效」，將以兩個訊息各自最接近時間當作起訖
        * ※ 可以只給一個
        * 使用範例：排程的啟動訊息和結束訊息
    * 查詢條件關鍵字用「+」區隔的功能，連接越多關鍵字，找得越精準
        * 關鍵字會「標記」內容(類似於Ctrl+F)的功能
        * 每個關鍵字都會「標記」
        * ※ 算BUG： 無法換行.......
        * 查詢時等待時間減少
    * 新增「每頁顯示幾筆」的下拉選單
        * 選完按查詢才能生效
    * 新增「頁碼」
        * 第一頁、最後一頁(顯示頁碼)用淡紫色顯示
        * 上一頁、下一頁是在還有的時候才能按
        * 現在頁碼用淡棕色顯示
        * 明細的上下皆有頁碼
* **/

/// <summary>
/// 冠永騰專用LOG查詢(可過濾資料)
/// </summary>
public partial class CDS_KYTUtils_SearchAllKYTILogsV2 : BasePage
{
    public string Page_NAME = "冠永騰專用LOG查詢V2";

    private string ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;

    private static int PageSize = 30; // 一頁顯示幾筆資料
    private int PageDisplayRange = 10; // 顯示的頁碼個數
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode(Page_NAME, Request.Url.AbsoluteUri);
            startDateTime.Value = DateTime.Now.AddHours(-1).ToString("yyyy-MM-ddTHH:00:00");
            endDateTime.Value = DateTime.Now.AddMinutes(2).ToString("yyyy-MM-ddTHH:mm:ss"); // 加兩分鐘的操作時間

            ddlLogLevel.DataSource = getLogLevel();
            ddlLogLevel.DataTextField = "TEXT";
            ddlLogLevel.DataValueField = "VALUE";
            ddlLogLevel.DataBind();

            ddlFileName.DataSource = getDistinctColumnValue("FILE_NAME");
            ddlFileName.DataValueField = "TEXT";
            ddlFileName.DataTextField = "TEXT";
            ddlFileName.DataBind();

            ddlNamespace.DataSource = getDistinctColumnValue("NAMESPACE");
            ddlNamespace.DataValueField = "TEXT";
            ddlNamespace.DataTextField = "TEXT";
            ddlNamespace.DataBind();

            ddlMethodName.DataSource = getDistinctColumnValue("METHOD_NAME");
            ddlMethodName.DataValueField = "TEXT";
            ddlMethodName.DataTextField = "TEXT";
            ddlMethodName.DataBind();

            ddlPageSize_SelectedIndexChanged(ddlPageSize, null); // 觸發每頁顯示幾筆
        }
    }

    #region 非控制項事件

    /// <summary>
    /// 取得LOG等級
    /// </summary>
    /// <returns></returns>
    private DataTable getLogLevel()
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("TEXT", typeof(string)));
        dtSource.Columns.Add(new DataColumn("VALUE", typeof(int)));
        DataRow ndr = dtSource.NewRow();
        ndr["TEXT"] = "===請選擇===";
        ndr["VALUE"] = -1;
        dtSource.Rows.Add(ndr);
        foreach (int _level in Enum.GetValues(typeof(KYTLog.DebugLog.LogLevel)))
        {
            DataRow _ndr = dtSource.NewRow();
            _ndr["TEXT"] = ((KYTLog.DebugLog.LogLevel)_level).ToString().ToUpper();
            _ndr["VALUE"] = _level;
            dtSource.Rows.Add(_ndr);
        }
        return dtSource;
    }

    private DataTable getDistinctColumnValue(string columnName)
    {
        DataTable dtSource = new DataTable();

        using (SqlDataAdapter sda = new SqlDataAdapter(string.Format(@"
                SELECT '===請選擇===' AS 'TEXT'
             UNION ALL
                SELECT DISTINCT [{0}] AS 'TEXT' FROM ZUOF_KYTI_LOG
            ", columnName)
            , new DatabaseHelper().Command.Connection.ConnectionString))
        using (DataSet ds = new DataSet())
        {
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    dtSource = ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"SearchAllKYTILogsV2.getDistinctColumnValue.SELECT.ZUOF_KYTI_LOG.{0}.ERROR:{1}", columnName, e.Message));
            }
        }
        return dtSource;
    }

    private void Search(int _PageNo, int _PageSize, int _PageDisplayRange)
    {
        string filter = "";
        string[] _filters = txtFilter.Text.Trim().Split('+');

        if (ddlLogLevel.SelectedIndex != 0) // 如果有選擇LOG等級
        {
            filter += " AND LOG_LEVEL = @LOG_LEVEL";
        }
        if (ddlFileName.SelectedIndex != 0)
        {
            filter += " AND [FILE_NAME] = @FILE_NAME";
        }
        if (ddlNamespace.SelectedIndex != 0)
        {
            filter += " AND NAMESPACE = @NAMESPACE";
        }
        if (ddlMethodName.SelectedIndex != 0)
        {
            filter += " AND METHOD_NAME = @METHOD_NAME";
        }
        if (!string.IsNullOrEmpty(txtFilter.Text.Trim()))
        {
            for (int i = 0; i < _filters.Length; i++)
            {
                filter += string.Format(" AND MSG LIKE '%' + @MSGFILTER{0} + '%'", i);
            }
        }
        DateTime dtSTARTTIME = DateTime.Parse(startDateTime.Value);
        DateTime dtENDTIME = DateTime.Parse(endDateTime.Value);
        if (!string.IsNullOrEmpty(txtKYTTaskStartMsg.Text.Trim()))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                    SELECT TOP 1 LOG_TIME -- 找出開始時間 
                      FROM ZUOF_KYTI_LOG
                     WHERE MSG LIKE '%' + @STARTMSG + '%'
                  ORDER BY LOG_TIME DESC
                ", ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@STARTMSG", txtKYTTaskStartMsg.Text.Trim());
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        dtSTARTTIME = (DateTime)ds.Tables[0].Rows[0]["LOG_TIME"];
                    }
                }
                catch (Exception ex)
                {
                    KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"SearchAllKYTILogsV2.btnFilter_Click.SELECT.{1}.ERROR:{0}", ex.Message, txtKYTTaskStartMsg.Text));

                }
            }
        }
        if (!string.IsNullOrEmpty(txtKYTTaskEndMsg.Text.Trim()))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                    SELECT TOP 1 LOG_TIME -- 找出結束時間 
                      FROM ZUOF_KYTI_LOG
                     WHERE MSG LIKE '%' + @ENDMSG + '%'
                  ORDER BY LOG_TIME DESC
                ", ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@ENDMSG", txtKYTTaskEndMsg.Text.Trim());
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        dtENDTIME = (DateTime)ds.Tables[0].Rows[0]["LOG_TIME"];
                    }
                }
                catch (Exception ex)
                {
                    KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"SearchAllKYTILogsV2.btnFilter_Click.SELECT.{1}.ERROR:{0}", ex.Message, txtKYTTaskStartMsg.Text));
                }
            }
        }
        if (dtENDTIME.CompareTo(dtSTARTTIME) < 0) // 如果結束時間小於開始時間，兩個時間對調
        {
            DateTime dtTmp = dtENDTIME;
            dtENDTIME = dtSTARTTIME;
            dtSTARTTIME = dtTmp; 
        }
       
        using (SqlDataAdapter sda = new SqlDataAdapter(string.Format(@"
                 SET NOCOUNT ON;
                 DECLARE @MAX_COUNT INT = 0
                 DECLARE @MAX_PAGE INT = 0

                 DECLARE @TABLE TABLE (
	                ROWNO BIGINT,
	                LOG_ID NVARCHAR(50) PRIMARY KEY,
	                LOG_TIME DATETIME2(7),
	                [FILE_NAME] NVARCHAR(50),
	                [NAMESPACE] NVARCHAR(200),
	                [METHOD_NAME] NVARCHAR(50),
	                LINE_NUMBER INT,
	                LOG_LEVEL INT,
	                LOG_LEVEL_TYPE NVARCHAR(15),
	                MSG NVARCHAR(MAX)
                 )
 
                 INSERT INTO @TABLE (ROWNO, LOG_ID, LOG_TIME, [FILE_NAME], [NAMESPACE], [METHOD_NAME], LINE_NUMBER, LOG_LEVEL, LOG_LEVEL_TYPE, MSG) 
	                  SELECT ROW_NUMBER() OVER(ORDER BY LOG_TIME DESC) AS 'ROWNO',
			                 LOG_ID,
			                 LOG_TIME,
			                 [FILE_NAME], 
			                 [NAMESPACE], 
			                 [METHOD_NAME],
			                 LINE_NUMBER, 
			                 LOG_LEVEL, 
			                 LOG_LEVEL_TYPE, 
			                 MSG
                        FROM ZUOF_KYTI_LOG
                       WHERE LOG_TIME BETWEEN @STARTTIME AND @ENDTIME
	                   {0}
                          ORDER BY LOG_TIME DESC


	                SELECT @MAX_COUNT = COUNT(*) FROM @TABLE
	                SET @MAX_PAGE = CAST(CEILING(CAST(@MAX_COUNT AS DECIMAL) / CAST(@PAGE_SIZE AS DECIMAL)) AS INT)
	                --SELECT @MAX_PAGE AS 'MAX_PAGE', @MAX_COUNT AS 'MAX_COUNT'
	                 IF (@PAGE_NO < 1) -- 指定頁碼小於 1
		                 BEGIN
			                SET @PAGE_NO = 1
		                 END
                ELSE IF (@PAGE_NO > @MAX_PAGE) -- 指定頁碼大於最大頁碼
		                BEGIN
			                SET @PAGE_NO = @MAX_PAGE
		                END

	                DECLARE @FIRSTPAGE INT = @PAGE_NO
	                DECLARE @LASTPAGE INT = @MAX_PAGE
	                 IF (@LASTPAGE < @PAGE_DRANGE) -- 最後一頁小於頁碼顯示範圍
		                BEGIN
			                SET @FIRSTPAGE = 1
		                END
                ELSE IF (@PAGE_NO < (@PAGE_DRANGE / 2) + 1) -- 指定頁碼小於顯示範圍的一半
		                BEGIN
			                SET @FIRSTPAGE = 1
			                SET @LASTPAGE = @PAGE_DRANGE
		                END
                ELSE IF (@PAGE_NO > @MAX_PAGE - ((@PAGE_DRANGE / 2) + 1)) -- 指定頁碼大於最大頁碼減去顯示範圍的一半的頁碼
		                BEGIN
			                SET @FIRSTPAGE = @MAX_PAGE - (@PAGE_DRANGE - 1)
		                END
                   ELSE -- 指定頁碼大於或等於顯示範圍的一半
		                BEGIN
			                SET @FIRSTPAGE = @PAGE_NO - (CEILING(CAST(@PAGE_DRANGE AS decimal) / 2) - 1)
			                SET @LASTPAGE = @FIRSTPAGE + @PAGE_DRANGE - 1
		                END


                --SELECT @FIRSTPAGE AS 'FIRSTPAGE',@LASTPAGE AS 'LASTPAGE'

                ;WITH PAGES AS
                (
	                SELECT @FIRSTPAGE AS 'NOW_PAGE'
	                UNION ALL
	                SELECT NOW_PAGE + 1
	                FROM PAGES
	                WHERE NOW_PAGE < @LASTPAGE
                )

                SELECT @MAX_COUNT AS 'MAX_COUNT',
	                   @MAX_PAGE AS 'MAX_PAGE',
	                   NOW_PAGE AS 'PAGE_NO',
	                   @PAGE_SIZE AS 'PAGE_SIZE'
                  FROM PAGES
	
	                --------
	                SELECT * 
	                  FROM @TABLE
	                 WHERE ROWNO BETWEEN (@PAGE_SIZE * (@PAGE_NO - 1)) + 1 
					                AND CASE WHEN @PAGE_SIZE * @PAGE_NO > @MAX_COUNT 
							                 THEN @MAX_COUNT 
							                 ELSE @PAGE_SIZE * @PAGE_NO 
							                  END
                  ORDER BY ROWNO ASC
            ", filter)
            , ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@STARTTIME", dtSTARTTIME);
            sda.SelectCommand.Parameters.AddWithValue("@ENDTIME", dtENDTIME);
            sda.SelectCommand.Parameters.AddWithValue("@LOG_LEVEL", ddlLogLevel.SelectedValue);
            sda.SelectCommand.Parameters.AddWithValue("@FILE_NAME", ddlFileName.SelectedValue);
            sda.SelectCommand.Parameters.AddWithValue("@NAMESPACE", ddlNamespace.SelectedValue);
            sda.SelectCommand.Parameters.AddWithValue("@METHOD_NAME", ddlMethodName.SelectedValue);
            sda.SelectCommand.Parameters.AddWithValue("@PAGE_NO", _PageNo);
            sda.SelectCommand.Parameters.AddWithValue("@PAGE_SIZE", _PageSize);
            sda.SelectCommand.Parameters.AddWithValue("@PAGE_DRANGE", _PageDisplayRange);
            for (int i = 0; i < _filters.Length; i++)
            {
                string __f = _filters[i];
                sda.SelectCommand.Parameters.AddWithValue("@MSGFILTER" + i.ToString(), __f.Trim());
            }
            try
            {
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtPages = ds.Tables[0];
                    dtPages.Columns.Add(new DataColumn("CURRENT_PAGE_NO", typeof(int)));
                    foreach (DataRow dr in dtPages.Rows)
                        dr["CURRENT_PAGE_NO"] = _PageNo;
                    ViewState["rptPages"] = dtPages;
                    rptPages.DataSource = (DataTable)ViewState["rptPages"];
                    rptPages.DataBind();
                    rptPagesBottom.DataSource = (DataTable)ViewState["rptPages"];
                    rptPagesBottom.DataBind();
                    lblSearchCount.Text = ds.Tables[0].Rows[0]["MAX_COUNT"].ToString();
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewState["gbLogs"] = ds.Tables[1];
                    gvLogs.DataSource = (DataTable)ViewState["gbLogs"];
                    gvLogs.DataBind();
                }
                else
                {
                    ViewState["gbLogs"] = new DataTable();
                    gvLogs.DataSource = (DataTable)ViewState["gbLogs"];
                    gvLogs.DataBind();
                }
            }
            catch (Exception ex)
            {
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"SearchAllKYTILogsV2.btnFilter_Click.SELECT.ERROR:{0}", ex.Message));
            }
        }
    }

    #endregion 非控制項事件

    #region 控制項事件


    /// <summary>
    /// 按下過濾搜尋按鈕後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        ViewState["CurrentPage"] = 1;
        Search(1, PageSize, PageDisplayRange);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), "MarkEachFilter();", true);
    }

    /// <summary>
    /// 明細項的每筆資料的反應
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLogs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        gr.Attributes.Add("onmouseover", "this.bkc=this.style.backgroundColor;this.style.backgroundColor='#ccffff'"); // 設定每筆資料在滑鼠移動進去時的背景顏色
        gr.Attributes.Add("onmouseout", "this.style.backgroundColor=this.bkc"); // 設定每筆資料在滑鼠移出時的背景色
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;

            Label lblRowNo = gr.FindControl("lblRowNo") as Label; // 序列號
            Label lblDateTime = gr.FindControl("lblDateTime") as Label; // 時間
            Label lblLevel = gr.FindControl("lblLevel") as Label; // LOG等級
            Label lblFileName = gr.FindControl("lblFileName") as Label; // 檔案名稱
            Label lblNameSpace = gr.FindControl("lblNameSpace") as Label; // NameSpace
            Label lblMethodName = gr.FindControl("lblMethodName") as Label; // 方法名稱
            Label lblLineNumber = gr.FindControl("lblLineNumber") as Label; // 行號
            //TextBox txtValues = gr.FindControl("txtValues") as TextBox; // 內容
            HtmlGenericControl divValues = gr.FindControl("divValues") as HtmlGenericControl;

            Color _logColor = Color.Black;
            switch (row["LOG_LEVEL_TYPE"].ToString().ToUpper()) // 不同的LOG等級顯示不同的文字顏色
            {
                case "IMPORT":
                    _logColor = Color.FromArgb(0x83d831);
                    break;
                case "ERROR":
                    _logColor = Color.Red;
                    break;
                case "DEBUG":
                    _logColor = Color.Yellow;
                    break;
                case "INFO":
                    _logColor = Color.Blue;
                    break;
                case "DETAILINFO":
                    _logColor = Color.Green;
                    break;
                case "MAXINFO":
                    _logColor = Color.Gray;
                    break;
                case "無等級":
                    _logColor = Color.Brown;
                    break;
            }
            lblRowNo.Text = row["ROWNO"].ToString();
            lblDateTime.Text = ((DateTime)row["LOG_TIME"]).ToString("yyyy/MM/dd HH:mm:ss");
            lblLevel.Text = row["LOG_LEVEL_TYPE"].ToString();
            lblLevel.ForeColor = _logColor;
            lblFileName.Text = row["FILE_NAME"].ToString();
            lblNameSpace.Text = row["NAMESPACE"].ToString();
            lblMethodName.Text = row["METHOD_NAME"].ToString();
            lblLineNumber.Text = row["LINE_NUMBER"].ToString();
            //txtValues.Text = row["MSG"].ToString();
            divValues.InnerText = row["MSG"].ToString();
        }
    }

    #endregion 控制項事件

    /// <summary>
    /// 頁碼
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem ri = e.Item;
        DataTable dtSource = (DataTable)ViewState["rptPages"];
        DataRow firstRow = dtSource.Rows[0]; // 第一筆
        if (ri.ItemType == ListItemType.Item ||
            ri.ItemType == ListItemType.AlternatingItem)
        {
            DataRow dr = dtSource.Rows[ri.ItemIndex];
            LinkButton lbtnCurrent = ri.FindControl("lbtnCurrent") as LinkButton;
            lbtnCurrent.CssClass = "";

            lbtnCurrent.Text = dr["PAGE_NO"].ToString();
            if ((int)dr["CURRENT_PAGE_NO"] == (int)dr["PAGE_NO"])
            {
                lbtnCurrent.CssClass = "pageno-current";
                lbtnCurrent.Enabled = false; // 現在的頁碼不用按
            }

        }
        else if (ri.ItemType == ListItemType.Header)
        {
            LinkButton lbtnPrev = ri.FindControl("lbtnPrev") as LinkButton;
            LinkButton lbtnTop = ri.FindControl("lbtnTop") as LinkButton;
            lbtnTop.Visible = true;
            lbtnPrev.Enabled = true;
            lbtnPrev.CssClass = "";
            DataRow[] drHSearch = dtSource.Select(string.Format("PAGE_NO = {0}", 1));
            if (drHSearch.Length > 0)
            {
                lbtnTop.Visible = false; // 不需要操作第一頁
            }
            if ((int)firstRow["CURRENT_PAGE_NO"] == 1)
            {
                lbtnPrev.Enabled = false; // 第一頁的時候不用顯示第一頁
                lbtnPrev.CssClass = "pageno-disabled";
            }
        }
        else if (ri.ItemType == ListItemType.Footer)
        {
            LinkButton lbtnBottom = ri.FindControl("lbtnBottom") as LinkButton;
            LinkButton lbtnNext = ri.FindControl("lbtnNext") as LinkButton;
            lbtnBottom.Text = firstRow["MAX_PAGE"].ToString();
            lbtnBottom.Visible = true;
            lbtnNext.Enabled = true;
            lbtnNext.CssClass = "";
            if ((int)firstRow["CURRENT_PAGE_NO"] == (int)firstRow["MAX_PAGE"])
            {
                lbtnBottom.Visible = false;
                lbtnNext.Enabled = false;
                lbtnNext.CssClass = "pageno-disabled";
            }
            DataRow[] drFSearch = dtSource.Select(string.Format("PAGE_NO = {0}", (int)firstRow["MAX_PAGE"]));
            if (drFSearch.Length > 0)
            {
                lbtnBottom.Visible = false; // 不需要操作最後頁
            }
        }
    }

    protected void lbtnBottom_Click(object sender, EventArgs e)
    {
        LinkButton lbtnBottom = (LinkButton)sender;
        int _page = int.Parse(lbtnBottom.Text);
        ViewState["CurrentPage"] = _page;
        Search(_page, PageSize, PageDisplayRange);
    }

    protected void lbtnNext_Click(object sender, EventArgs e)
    {
        LinkButton lbtnNext = (LinkButton)sender;
        int _page = (int)ViewState["CurrentPage"] + 1;
        ViewState["CurrentPage"] = _page;
        Search(_page, PageSize, PageDisplayRange);
    }

    protected void lbtnCurrent_Click(object sender, EventArgs e)
    {
        LinkButton lbtnCurrent = (LinkButton)sender;
        int _page = int.Parse(lbtnCurrent.Text);
        ViewState["CurrentPage"] = _page;
        Search(_page, PageSize, PageDisplayRange);
    }

    protected void lbtnPrev_Click(object sender, EventArgs e)
    {
        LinkButton lbtnPrev = (LinkButton)sender;
        int _page = (int)ViewState["CurrentPage"] - 1;
        ViewState["CurrentPage"] = _page;
        Search(_page, PageSize, PageDisplayRange);

    }

    protected void lbtnTop_Click(object sender, EventArgs e)
    {
        LinkButton lbtnTop = (LinkButton)sender;
        int _page = int.Parse(lbtnTop.Text);
        ViewState["CurrentPage"] = _page;
        Search(_page, PageSize, PageDisplayRange);
    }

    /// <summary>
    /// 選擇每頁顯示幾筆功能變更後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList _ddlPageSize = (DropDownList)sender;
        PageSize = int.Parse(_ddlPageSize.SelectedValue);
    }
}
