using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Log;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Net.Mail;
using System.Threading.Tasks;
using Ede.Uof.Utility.FileCenter.V3;

public partial class CDS_WebPage_QC_TK_TEMP_HUMI_LOG : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 第一次載入時先查詢一次
            BindData();
            BindDropDownList();
            BindDropDownList2();
            BindDropDownList3();
            BindDropDownList4();
            BindDropDownList5();
            BindDropDownList6();
        }
    }

    #region FUNCTION
    // 每次 Timer 到期時觸發的事件
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        BindData(); // 重新向資料庫查詢並繫結資料
        BindGrid();
    }

    private void BindData()
    {
        lblStatus.Text = "最後更新時間：" + DateTime.Now.ToString("HH:mm:ss");
        // GridView1.DataSource = ... 取得資料庫資料
        // GridView1.DataBind();
    }
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[KIND],[PARAID],[PARANAME] FROM [TKQC].[dbo].[TBPARA] WHERE [KIND]='CDS_WebPage_QC_TK_TEMP_HUMI_LOG' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "PARANAME";
            DropDownList1.DataValueField = "PARANAME";
            DropDownList1.DataBind();

        }
        else
        {
        }
    }
    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[KIND],[PARAID],[PARANAME] FROM [TKQC].[dbo].[TBPARA] WHERE [KIND]='CDS_WebPage_QC_TK_TEMP_HUMI_LOG' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "PARANAME";
            DropDownList2.DataValueField = "PARANAME";
            DropDownList2.DataBind();

        }
        else
        {
        }
    }
    private void BindDropDownList3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[KIND],[PARAID],[PARANAME] FROM [TKQC].[dbo].[TBPARA] WHERE [KIND]='CDS_WebPage_QC_TK_TEMP_HUMI_LOG_HRS' ORDER BY [ID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList3.DataSource = dt;
            DropDownList3.DataTextField = "PARANAME";
            DropDownList3.DataValueField = "PARANAME";
            DropDownList3.DataBind();

        }
        else
        {
        }
    }
    protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 取得使用者選取的值 (例如："ALL" 或 "超標")
        string selectedValue = DropDownList3.SelectedValue;
        if(selectedValue.Equals("作業時間"))
        {
            Label6.Text = "07 點~21 點";
        }
        else
        {
            Label6.Text = "";
        }
       
    }
    protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 取得使用者選取的值 (例如："ALL" 或 "超標")
        string selectedValue = DropDownList4.SelectedValue;
        if (selectedValue.Equals("作業時間"))
        {
            Label7.Text = "07 點~21 點";
        }
        else
        {
            Label7.Text = "";
        }

    }
    private void BindDropDownList4()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[KIND],[PARAID],[PARANAME] FROM [TKQC].[dbo].[TBPARA] WHERE [KIND]='CDS_WebPage_QC_TK_TEMP_HUMI_LOG_HRS' ORDER BY [ID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList4.DataSource = dt;
            DropDownList4.DataTextField = "PARANAME";
            DropDownList4.DataValueField = "PARANAME";
            DropDownList4.DataBind();

        }
        else
        {
        }
    }

    private void BindDropDownList5()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("MACHINE", typeof(String));
        dt.Columns.Add("AREAS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [MACHINE],[AREAS] FROM [TKQC].[dbo].[LOG_MACHINE_AREAS]  ORDER BY SERNO ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList5.DataSource = dt;
            DropDownList5.DataTextField = "AREAS";
            DropDownList5.DataValueField = "AREAS";
            DropDownList5.DataBind();

        }
        else
        {
        }
    }
    private void BindDropDownList6()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("MACHINE", typeof(String));
        dt.Columns.Add("AREAS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [MACHINE],[AREAS] FROM [TKQC].[dbo].[LOG_MACHINE_AREAS]  ORDER BY SERNO ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList6.DataSource = dt;
            DropDownList6.DataTextField = "AREAS";
            DropDownList6.DataValueField = "AREAS";
            DropDownList6.DataBind();

        }
        else
        {
        }
    }

    private void BindGrid()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringRecipe"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"
                         SELECT 
                             M.[ID]
                            ,M.[機台名稱]
                            ,M.[區域]
                            ,M.[TMP]
                            ,M.[HUM]
                            ,L.[實際溫度]
                            ,L.[溫度上限]
                            ,L.[溫度下限]
                            ,L.[實際溼度]
                            ,L.[溼度上限]
                            ,L.[溼度下限]
                        FROM [TK_FOOD].[dbo].[Machine] AS M WITH(NOLOCK)
                        OUTER APPLY (
                            SELECT TOP 1 
                                 TRY_CONVERT(DECIMAL(16,2), [控項_1]) AS '實際溫度'
                                ,TRY_CONVERT(DECIMAL(16,2), [控項_2]) AS '溫度上限'
                                ,TRY_CONVERT(DECIMAL(16,2), [控項_3]) AS '溫度下限'
                                ,TRY_CONVERT(DECIMAL(16,2), [控項_4]) AS '實際溼度'
                                ,TRY_CONVERT(DECIMAL(16,2), [控項_5]) AS '溼度上限'
                                ,TRY_CONVERT(DECIMAL(16,2), [控項_6]) AS '溼度下限'
                            FROM [TK_FOOD].[dbo].[log_table] WITH(NOLOCK)
                            WHERE [機台名稱] = M.[機台名稱]
                              -- 新增條件：限制日期時間必須在「今天 00:00:00」至「明天 00:00:00」之間（支援索引）
                              AND [日期時間] >= CAST(CAST(GETDATE() AS DATE) AS DATETIME)
                              AND [日期時間] < CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME)
                            ORDER BY [日期時間] DESC
                        ) AS L
                        WHERE M.[機台名稱] LIKE '%溫濕度%'
                        ORDER BY TRY_CONVERT(INT, M.[ID]);
                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());     

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        Grid1.DataSource = dt;
        Grid1.DataBind();

    }

    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // 必須先判斷是否為「資料列」（避開 Header 和 Footer）
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal num = 0;
            decimal upLimit = 0;
            decimal lowLimit = 0;

            // 1. 抓取 TemplateField 內部的 Label 控制項（請確認 ID 是否正確）
            Label lblNum = (Label)e.Row.FindControl("Label_實際溫度"); // 假設 Cell[2] 裡面的 Label ID 是這個
            Label lblUp = (Label)e.Row.FindControl("Label_溫度上限");      // 若 Cell[3] 也是 TemplateField 就這樣抓
            Label lblLow = (Label)e.Row.FindControl("Label_溫度下限");     // 若 Cell[4] 也是 TemplateField 就這樣抓

            // 2. 取出 Label 的 .Text 並轉型 (如果不是 TemplateField，才用 e.Row.Cells[x].Text)
            string strNum = lblNum != null ? lblNum.Text : e.Row.Cells[2].Text;
            string strUp = lblUp != null ? lblUp.Text : e.Row.Cells[3].Text;
            string strLow = lblLow != null ? lblLow.Text : e.Row.Cells[4].Text;

            // 3. 安全轉型為 decimal
            decimal.TryParse(strNum, out num);
            decimal.TryParse(strUp, out upLimit);
            decimal.TryParse(strLow, out lowLimit);

            // 💡 條件判斷：超過上限 或 低於下限
            if (num > upLimit || num < lowLimit)
            {
                // 整行變粉紅色背景
                e.Row.BackColor = System.Drawing.Color.Red;

                // 或者：若只要「實際溫度」儲存格變紅底白字
                // if (lblNum != null) {
                //     e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                //     lblNum.ForeColor = System.Drawing.Color.White;
                // }
            }
        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    private void BindGrid2()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringRecipe"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        
        StringBuilder SQL_QUERY = new StringBuilder();
        StringBuilder QUERY = new StringBuilder();
        StringBuilder QUERY2 = new StringBuilder();
        StringBuilder QUERY3 = new StringBuilder();

        string KINDS = DropDownList1.Text;
        if (KINDS.Equals("超標"))
        {
            QUERY.AppendFormat(@"
                                AND (
                                 TRY_CONVERT(DECIMAL(16,2), [控項_1]) > TRY_CONVERT(DECIMAL(16,2), [控項_2])
                                 OR     TRY_CONVERT(DECIMAL(16,2), [控項_1]) < TRY_CONVERT(DECIMAL(16,2), [控項_3])
                                 )
                                ");
        }
        else
        {
            QUERY.AppendFormat(@"");
        }

        string HRS= DropDownList3.Text;
        if(HRS.Equals("作業時間"))
        {
            QUERY2.AppendFormat(@"
                                AND DATEPART(HOUR, [日期時間]) >= 7 
                                AND DATEPART(HOUR, [日期時間]) < 21
                                ");
        }
        else
        {
            QUERY2.AppendFormat(@"");
        }

        string AREAS = DropDownList5.Text;
        if (!AREAS.Equals("全部"))
        {
            QUERY3.AppendFormat(@"
                                AND [區域] ='{0}'
                               
                                ", AREAS);
        }
        else
        {
            QUERY3.AppendFormat(@"");
        }

        // 2. 進行解析與預設值處理
        DateTime selectedDate;
        if (!DateTime.TryParse(Date1.Text, out selectedDate))
        {
            selectedDate = DateTime.Today;
        }

        // 3. 後續轉換成您需要的格式
        string dateStart = selectedDate.ToString("yyyy-MM-dd 00:00:00");
        string dateEnd = selectedDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

        // 2. 定義 SQL 查詢字串 
        SQL_QUERY.AppendFormat(@"
                                WITH CTE AS (
                                SELECT 
                                    CONVERT(NVARCHAR(19), [日期時間], 120) AS 日期時間,
                                    [ID],
                                    [Machine].[機台名稱],
                                    [區域],
                                    [控項_1] AS '實際溫度',
                                    [控項_2] AS '溫度上限',
                                    [控項_3] AS '溫度下限',
                                    -- 核心修改：利用總分鐘數整除 5，將時間歸類到每 5 分鐘一個區塊
                                   ROW_NUMBER() OVER(
                                        PARTITION BY [Machine].[機台名稱], [區域], DATEDIFF(MINUTE, 0, [日期時間]) / 5 
                                        ORDER BY [日期時間] ASC
                                    ) AS RN
                                FROM [TK_FOOD].[dbo].[log_table] WITH(NOLOCK)
                                INNER JOIN [TK_FOOD].[dbo].[Machine] WITH(NOLOCK) ON [Machine].機台名稱 = [log_table].機台名稱
                                WHERE [Machine].[機台名稱]  LIKE '%溫濕度%'                                 
                                  AND [日期時間] >= '{0}' AND [日期時間] < '{1}'
                                {2}
                                {3}
                                {4}
                            )
                            SELECT 
                                [日期時間],
                                [ID],
                                [機台名稱],
                                [區域],
                                CONVERT(DECIMAL(16,2),[實際溫度]) 實際溫度,
                                CONVERT(DECIMAL(16,2),[溫度上限]) 溫度上限,
                                CONVERT(DECIMAL(16,2),[溫度下限]) 溫度下限
                            FROM CTE
                            WHERE RN = 1 -- 只取每 5 分鐘區間內的第一筆紀錄
                            ORDER BY [ID],[日期時間] 
                            ", dateStart, dateEnd, QUERY.ToString(), QUERY2.ToString(), QUERY3.ToString());
        
        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());     

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(SQL_QUERY.ToString()));


        Grid2.DataSource = dt;
        Grid2.DataBind();

    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    private void BindGrid3()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringRecipe"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder SQL_QUERY = new StringBuilder();
        StringBuilder QUERY = new StringBuilder();
        StringBuilder QUERY2 = new StringBuilder();
        StringBuilder QUERY3 = new StringBuilder();

        string KINDS = DropDownList2.Text;
        if (KINDS.Equals("超標"))
        {
            QUERY.AppendFormat(@" 
                                AND (
                                 TRY_CONVERT(DECIMAL(16,2), [控項_4]) > TRY_CONVERT(DECIMAL(16,2), [控項_5])
                                 OR     TRY_CONVERT(DECIMAL(16,2), [控項_4]) < TRY_CONVERT(DECIMAL(16,2), [控項_6])
                                )"
                               );
        }
        else
        {
            QUERY.AppendFormat(@"");
        }
        string HRS = DropDownList4.Text;
        if (HRS.Equals("作業時間"))
        {
            QUERY2.AppendFormat(@"
                                AND DATEPART(HOUR, [日期時間]) >= 7 
                                AND DATEPART(HOUR, [日期時間]) < 21
                                ");
        }
        else
        {
            QUERY2.AppendFormat(@"");
        }

        string AREAS = DropDownList6.Text;
        if (!AREAS.Equals("全部"))
        {
            QUERY3.AppendFormat(@"
                                AND [區域] ='{0}'
                               
                                ", AREAS);
        }
        else
        {
            QUERY3.AppendFormat(@"");
        }

        // 2. 進行解析與預設值處理
        DateTime selectedDate;
        if (!DateTime.TryParse(Date2.Text, out selectedDate))
        {
            selectedDate = DateTime.Today;
        }

        // 3. 後續轉換成您需要的格式
        string dateStart = selectedDate.ToString("yyyy-MM-dd 00:00:00");
        string dateEnd = selectedDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

        // 2. 定義 SQL 查詢字串 
        SQL_QUERY.AppendFormat(@"
                                WITH CTE AS (
                                SELECT 
                                    CONVERT(NVARCHAR(19), [日期時間], 120) AS 日期時間,
                                    [ID],
                                    [Machine].[機台名稱],
                                    [區域],
                                    [控項_4] AS '實際溼度',
                                    [控項_5] AS '溼度上限',
                                    [控項_6] AS '溼度下限',
                                    -- 核心修改：利用總分鐘數整除 5，將時間歸類到每 5 分鐘一個區塊
                                   ROW_NUMBER() OVER(
                                        PARTITION BY [Machine].[機台名稱], [區域], DATEDIFF(MINUTE, 0, [日期時間]) / 5 
                                        ORDER BY [日期時間] ASC
                                    ) AS RN
                                FROM [TK_FOOD].[dbo].[log_table] WITH(NOLOCK)
                                INNER JOIN [TK_FOOD].[dbo].[Machine] WITH(NOLOCK) ON [Machine].機台名稱 = [log_table].機台名稱
                                WHERE [Machine].[機台名稱]  LIKE '%溫濕度%'                               
                                  AND [日期時間] >= '{0}' AND [日期時間] < '{1}'
                                   {2}
                                   {3}
                                    {4}
                            )
                            SELECT 
                                [日期時間],
                                [ID],
                                [機台名稱],
                                [區域],
                                CONVERT(DECIMAL(16,2),[實際溼度]) 實際溼度,
                                CONVERT(DECIMAL(16,2),[溼度上限]) 溼度上限,
                                CONVERT(DECIMAL(16,2),[溼度下限]) 溼度下限
                            FROM CTE
                            WHERE RN = 1 -- 只取每 5 分鐘區間內的第一筆紀錄
                            ORDER BY [ID],[日期時間] 
                            ", dateStart, dateEnd, QUERY.ToString(), QUERY2.ToString(), QUERY3.ToString());

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());     

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(SQL_QUERY.ToString()));


        Grid3.DataSource = dt;
        Grid3.DataBind();

    }

    protected void grid_PageIndexChanging3(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }
    #endregion


    #region BUTTON
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        BindGrid2();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        BindGrid3();
    }
    #endregion

}