using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Page.Common;

public partial class CDS_WebPage_RESEARCH_UOF1005_SEARCH : Ede.Uof.Utility.Page.BasePage
{
    // 查詢結果暫存，換頁時不必重跑 SQL
    private const string SESSION_KEY = "UOF1005_SEARCH_DT";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 預設查今年 1/1 ~ 12/31
            dpSDate.SelectedDate = new DateTime(DateTime.Today.Year, 1, 1);
            dpEDate.SelectedDate = new DateTime(DateTime.Today.Year, 12, 31);
            Session.Remove(SESSION_KEY);
        }
    }

    #region EVENT
    protected void Button1_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        string keyword = txtKeyword.Text.Trim();
        string docNbr = txtDocNbr.Text.Trim();

        if (keyword == "")
        {
            lblMsg.Text = "請輸入品名關鍵字";
            return;
        }
        if (!dpSDate.SelectedDate.HasValue || !dpEDate.SelectedDate.HasValue)
        {
            lblMsg.Text = "請輸入申請日期起訖";
            return;
        }

        DateTime sDate = dpSDate.SelectedDate.Value.Date;
        DateTime eDate = dpEDate.SelectedDate.Value.Date;
        if (sDate > eDate)
        {
            lblMsg.Text = "申請日期起不可大於訖";
            return;
        }

        try
        {
            DataTable dt = Search(keyword, docNbr, sDate, eDate);
            Session[SESSION_KEY] = dt;

            Grid1.PageIndex = 0;
            Grid1.DataSource = dt;
            Grid1.DataBind();

            lblMsg.Text = string.Format("共 {0} 筆", dt.Rows.Count);
        }
        catch (Exception ex)
        {
            lblMsg.Text = "查詢失敗：" + Server.HtmlEncode(ex.Message);
        }
    }

    protected void Grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt = Session[SESSION_KEY] as DataTable;
        if (dt == null)
        {
            return;
        }
        Grid1.PageIndex = e.NewPageIndex;
        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object taskId = DataBinder.Eval(e.Row.DataItem, "TASK_ID");
            HyperLink hlTask = (HyperLink)e.Row.FindControl("hlTask");

            if (hlTask != null && taskId != null && taskId != DBNull.Value)
            {
                hlTask.NavigateUrl = string.Format("https://eip.tkfood.com.tw/UOF/wkf/formuse/viewform.aspx?TASK_ID={0}", taskId);
            }
        }
    }
    #endregion

    #region FUNCTION
    private DataTable Search(string keyword, string docNbr, DateTime sDate, DateTime eDate)
    {
        // 20261102 查1005.雜項採購單
        const string sql = @"
IF OBJECT_ID('tempdb..#FV') IS NOT NULL DROP TABLE #FV;
SELECT FV.FORM_VERSION_ID, F.FORM_NAME
INTO #FV
FROM [UOF].dbo.TB_WKF_FORM_VERSION FV
JOIN [UOF].dbo.TB_WKF_FORM F ON F.FORM_ID = FV.FORM_ID
WHERE F.FORM_NAME = N'1005.雜項採購單';

IF OBJECT_ID('tempdb..#DOC') IS NOT NULL DROP TABLE #DOC;
SELECT T.TASK_ID, T.DOC_NBR, V.FORM_NAME,
       CAST(D.DOCTXT AS XML) AS DOCXML
INTO #DOC
FROM [UOF].dbo.TB_WKF_TASK T
JOIN #FV V ON V.FORM_VERSION_ID = T.FORM_VERSION_ID
CROSS APPLY (SELECT CAST(T.CURRENT_DOC AS NVARCHAR(MAX)) AS DOCTXT) D
WHERE T.BEGIN_TIME >= @SDATE AND T.BEGIN_TIME < DATEADD(DAY,1,@EDATE)
  AND (@DOC_NBR = N'' OR T.DOC_NBR = @DOC_NBR)
  AND D.DOCTXT COLLATE Latin1_General_BIN2
      LIKE N'%' + @KEYWORD + N'%' COLLATE Latin1_General_BIN2;

SELECT
     R.C.value('(Cell[@fieldId=""GG002""]/@fieldValue)[1]',  'NVARCHAR(200)')     AS [請購品名]
    ,B.DOC_NBR
    ,H.F.value('(FieldItem[@fieldId=""GA003""]/@fieldValue)[1]', 'NVARCHAR(100)') AS [請購人員]
    ,H.F.value('(FieldItem[@fieldId=""GA019""]/@fieldValue)[1]', 'NVARCHAR(MAX)') AS [主要用途說明]
    ,R.C.value('(Cell[@fieldId=""GG011""]/@fieldValue)[1]',  'NVARCHAR(100)')     AS [請購部門]
    ,CONVERT(NVARCHAR(50), B.TASK_ID) AS TASK_ID
FROM #DOC B
CROSS APPLY B.DOCXML.nodes('/Form/FormFieldValue') H(F)
CROSS APPLY H.F.nodes('FieldItem[@fieldId=""DETAILS""]/DataGrid/Row
        [Cell[(@fieldId=""GG002"" or @fieldId=""GG002B"")
              and contains(@fieldValue, sql:variable(""@KEYWORD""))]]') R(C)
ORDER BY B.DOC_NBR;

DROP TABLE #DOC;
DROP TABLE #FV;
";

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandTimeout = 300;
            cmd.Parameters.Add("@KEYWORD", SqlDbType.NVarChar, 200).Value = keyword;
            cmd.Parameters.Add("@DOC_NBR", SqlDbType.NVarChar, 50).Value = docNbr;
            cmd.Parameters.Add("@SDATE", SqlDbType.DateTime).Value = sDate;
            cmd.Parameters.Add("@EDATE", SqlDbType.DateTime).Value = eDate;

            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }
    }
    #endregion
}
