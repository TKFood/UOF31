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

public partial class CDS_WebPage_MARKETING_TK_QUERYS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    DataTable EXPORT_DT4 = new DataTable();

    // 當前組別的小計變數
    private decimal SUB_TOTALNUMS = 0.00m;
    private decimal SUB_TOTALMONEYS = 0.00m;
    private decimal SUB_TOTALCOSTS = 0.00m;

    // 追蹤前一筆資料的 TH004 值
    private string previousTH004 = string.Empty;
    string currentTH004 = string.Empty;

    // 新增：追蹤手動插入行的總數量
    private int insertedRowCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        // 強制讓 Grid4 的匯出按鈕走全頁回傳
        ScriptManager.GetCurrent(this).RegisterPostBackControl(Button5);
        // 強制讓 Grid4 的匯出按鈕走全頁回傳
        ScriptManager.GetCurrent(this).RegisterPostBackControl(Grid4);

        if (!IsPostBack)
        {
            SET_DATES();
            BindDropDownList1();
        }
    }

    #region FUNCTION
    public void SET_DATES()
    {
        TextBox1.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox2.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox4.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox5.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox7.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox8.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox9.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox10.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox17.Text = DateTime.Now.ToString("yyyyMMdd");
        TextBox18.Text = DateTime.Now.ToString("yyyyMMdd");
    }

    private void BindDropDownList1()
    {
        // 1. 從 Web.config 取得連線字串
        string connString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;

        // 2. 撰寫 SQL 查詢語句
        string sql = "SELECT MT002,MT003 FROM[TK].dbo.POSMT WHERE MT003 LIKE '%折價券%'";

        DataTable dt = new DataTable();

        // 3. 使用 using 自動管理資料庫連線與資源釋放
        using (SqlConnection conn = new SqlConnection(connString))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            adapter.Fill(dt);
        }

        // 4. 設定 DropDownList 屬性並繫結資料
        DropDownList1.DataSource = dt;
        DropDownList1.DataTextField = "MT003"; // 顯示在下拉選單上的文字
        DropDownList1.DataValueField = "MT002";   // 選項對應的實際 Value 值
        DropDownList1.DataBind();

        // 5. (可選) 加入預設提示選項
        DropDownList1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- 請選擇類別 --", "0"));
    }
    private void BindGrid()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"
                            SELECT *
                            FROM 
	                            (
	                            SELECT
		                            MIN(POSTA.TA001) AS '日期起',
		                            MAX(POSTA.TA001) AS '日期迄',
		                            POSTA.TA002 AS '門市代',
		                            CMSME.ME002 AS '門市',    
		                            -- 銷售總筆數 (全量)
		                            COUNT(1) AS '銷售總筆數含退貨',    
		                            -- 銷售總金額 (全量)
		                            SUM(POSTA.TA026) AS '銷售總金額含退貨',    
		                            -- 滿額筆數 (>= 1000)
		                            SUM(CASE WHEN POSTA.TA026 >= @QUERYMONEY THEN 1 ELSE 0 END) AS '滿額總筆數不含退貨',    
		                            -- 滿額總金額 (>= 1000)
		                            SUM(CASE WHEN POSTA.TA026 >= @QUERYMONEY THEN POSTA.TA026 ELSE 0 END) AS '滿額金額含稅不含退貨'
	                            FROM [TK].dbo.POSTA POSTA WITH (NOLOCK)
	                            INNER JOIN [TK].dbo.CMSME CMSME WITH (NOLOCK) 
		                            ON POSTA.TA002 = CMSME.ME001
	                            WHERE POSTA.TA038 NOT IN ('4')
	                              AND POSTA.TA001 >= @DATESTART
	                              AND POSTA.TA001 <= @DATESEND
                                  AND POSTA.TA002 LIKE '106%'
	                            GROUP BY 
		                            POSTA.TA002, 
		                            CMSME.ME002

	                            UNION ALL

	                            SELECT 
		                            MIN(COPTG.TG003) AS '日期起',
		                            MAX(COPTG.TG003) AS '日期迄',
		                            COPTG.TG005 AS '門市代',
		                            CMSME.ME002 AS '門市',    
		                            -- 銷售總筆數 (全量)
		                            COUNT(1) AS '銷售總筆數含退貨',    
		                            -- 銷售總金額 (全量)
		                            SUM(COPTG.TG045 + COPTG.TG046) AS '銷售總金額含退貨',    
		                            -- 滿額筆數 (>= 1000)
		                            SUM(CASE WHEN (COPTG.TG045 + COPTG.TG046) >= @QUERYMONEY THEN 1 ELSE 0 END) AS '滿額總筆數不含退貨',    
		                            -- 滿額總金額 (>= 1000)
		                            SUM(CASE WHEN (COPTG.TG045 + COPTG.TG046) >= @QUERYMONEY THEN (COPTG.TG045 + COPTG.TG046) ELSE 0 END) AS '滿額金額含稅不含退貨'
	                            FROM [TK].dbo.COPTG COPTG WITH (NOLOCK)
	                            INNER JOIN [TK].dbo.CMSME CMSME WITH (NOLOCK) 
		                            ON COPTG.TG005 = CMSME.ME001
	                            WHERE COPTG.TG023 = 'Y'
	                              AND COPTG.TG003 >=@DATESTART 
	                              AND COPTG.TG003 <=@DATESEND
                                  -- 若門市代號格式固定，建議移除開頭 % 符號以利用 Index (例如改為 LIKE '117%' OR LIKE '120%')
	                              AND (COPTG.TG005 LIKE '%117%' OR COPTG.TG005 LIKE '%120%')
	                              -- 使用 EXISTS 代替 IN 效率更高
	                              AND EXISTS (
		                              SELECT 1 
		                              FROM [TKBUSINESS].[dbo].[TK_TG001] TK 
		                              WHERE TK.TG001 = COPTG.TG001
	                              )
	                            GROUP BY 
		                            COPTG.TG005, 
		                            CMSME.ME002
                            ) AS TEMP
                            ORDER BY 門市代
                        ";

        m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());
        m_db.AddParameter("@DATESEND", TextBox2.Text.Trim());
        m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

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
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder SQL_QUERY1 = new StringBuilder();
        StringBuilder cmdTxt = new StringBuilder();

        string DATESTART= TextBox4.Text.Trim();
        string DATESEND= TextBox5.Text.Trim();
        string MB001 = TextBox6.Text.Trim();

      
        //定義 SQL 查詢字串           
        cmdTxt.AppendFormat(@"
                           SELECT 
                                TG005 AS '客戶通路代碼',
                                ME002 AS '名稱',
                                TH004 AS '品號',
                                MB002 AS '品名',
                                SUM(NUMS) AS '總數量',
                                SUM(MONEYS) AS '總金額',
                                SUM(COSTS) AS '總成本'
                            FROM 
                            (
                                -- 1. 銷貨單部分 (加上 WITH(NOLOCK) 避免鎖定)
                                SELECT 
                                    TG005, ME.ME002, TH004, MB.MB002,
                                    (TH008 + TH024) AS NUMS,
                                    TH013 AS MONEYS,
                                    LA.LA013 AS COSTS
                                FROM [TK].dbo.COPTG TG WITH(NOLOCK)
                                INNER JOIN [TK].dbo.COPTH TH WITH(NOLOCK) ON TG001 = TH001 AND TG002 = TH002
                                INNER JOIN [TK].dbo.INVMB MB WITH(NOLOCK) ON MB.MB001 = TH004
                                INNER JOIN [TK].dbo.INVLA LA WITH(NOLOCK) ON LA006 = TH001 AND LA007 = TH002 AND LA008 = TH003
                                LEFT JOIN [TK].dbo.CMSME ME WITH(NOLOCK) ON ME.ME001 = TG.TG005
                                WHERE TG023 = 'Y'
                                  AND TH017 <> '********************'
                                  AND TG003 BETWEEN @DATESTART AND @DATESEND
                                  AND (TH004 LIKE '%'+@MB001+'%' OR MB.MB002 LIKE '%'+@MB001+'%')

                                UNION ALL

                                -- 2. 銷退單部分
                                SELECT 
                                    TI005, ME.ME002, TJ004, MB.MB002,
                                    (TJ007 * -1) AS NUMS,
                                    (TJ012 * -1) AS MONEYS,
                                    (LA.LA013 * -1) AS COSTS
                                FROM [TK].dbo.COPTI TI WITH(NOLOCK)
                                INNER JOIN [TK].dbo.COPTJ TJ WITH(NOLOCK) ON TI001 = TJ001 AND TI002 = TJ002
                                INNER JOIN [TK].dbo.INVMB MB WITH(NOLOCK) ON MB.MB001 = TJ004
                                INNER JOIN [TK].dbo.INVLA LA WITH(NOLOCK) ON LA.LA006 = TJ001 AND LA.LA007 = TJ002 AND LA.LA008 = TJ003
                                LEFT JOIN [TK].dbo.CMSME ME WITH(NOLOCK) ON ME.ME001 = TI.TI005
                                WHERE TI019 = 'Y'
                                  AND TJ014 <> '********************'
                                  AND TI003 BETWEEN @DATESTART AND @DATESEND
                                  AND (TJ004 LIKE '%'+@MB001+'%' OR MB.MB002 LIKE '%'+@MB001+'%')

                                UNION ALL

                                -- 3. POS 銷售部分 (優化子查詢)
                                SELECT 
                                    MA005, ME.ME002, TB010, MB.MB002,
                                    TB019 AS NUMS,
                                    (TB031 + TB032) AS MONEYS,
                                    ISNULL(LA.LA012, 0) * TB019 AS COSTS
                                FROM [TK].dbo.POSTB TB WITH(NOLOCK)
                                INNER JOIN [TK].dbo.INVMB MB WITH(NOLOCK) ON MB.MB001 = TB010
                                LEFT JOIN [TK].dbo.WSCMA MA WITH(NOLOCK) ON MA.MA001 = TB.TB002
                                LEFT JOIN [TK].dbo.CMSME ME WITH(NOLOCK) ON MA.MA005 = ME.ME001
                                -- 改用 OUTER APPLY 提升成本抓取的效能，僅查一次
                                OUTER APPLY (
                                    SELECT TOP 1 LA012 
                                    FROM [TK].dbo.INVLA WITH(NOLOCK) 
                                    WHERE LA001 = TB.TB010 AND LA006 = TB.TB002 AND LA007 = TB.TB001
                                    ORDER BY LA004 DESC -- 確保抓到正確日期的成本
                                ) AS LA
                                WHERE TB042 IN ('1','2','3','6','7','8','9')
                                  AND TB001 BETWEEN @DATESTART AND @DATESEND
                                  AND (TB010 LIKE '%'+@MB001+'%' OR MB.MB002 LIKE '%'+@MB001+'%')

                            ) AS TEMP
                            GROUP BY TG005, ME002, TH004, MB002
                            ORDER BY TG005,TH004
                        ");

        m_db.AddParameter("@DATESTART", DATESTART);
        m_db.AddParameter("@DATESEND", DATESEND);
        m_db.AddParameter("@MB001", MB001);
        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        // ***關鍵步驟 1：重設所有追蹤變數***

         // 1. 重設小計和總計累積變數
        SUB_TOTALNUMS = 0.00m;
        SUB_TOTALMONEYS = 0.00m;
        SUB_TOTALCOSTS = 0.00m;
        // 如果您有總計變數 (如 GRAND_TOTALNUMS)，也要重設
        // GRAND_TOTALNUMS = 0.00m;

        // 2. 重設分組追蹤變數
        previousTH004 = string.Empty;

        // 3. 重設手動插入行追蹤變數
        insertedRowCount = 0;

        // *** 關鍵步驟 2：清除 GridView 的 Controls 集合 ***
        // 手動清除之前插入的小計行，避免它們殘留。
        // 這是一個額外的保險，特別是當您手動使用 AddAt 插入 Controls 時。
        if (Grid2.Controls.Count > 0 && Grid2.Controls[0].Controls.Count > 0)
        {
            // Controls[0] 通常是 Table 物件
            Grid2.Controls[0].Controls.Clear();
        }

        Grid2.DataSource = dt;
        Grid2.DataBind();

    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        // 判斷當前處理的是否為資料列 (DataRow)
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object th004Value = DataBinder.Eval(e.Row.DataItem, "品號");
            // 檢查 DataBinder.Eval 的結果是否為 null，如果不是，則調用 ToString()；
            // 如果是 null，則直接返回 string.Empty
            currentTH004 = (th004Value != null) ? th004Value.ToString() : string.Empty;

            decimal currentNUMS = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "總數量"));
            decimal currentMONEYS = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "總金額"));
            decimal currentCOSTS = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "總成本"));

            // 只有在第一次載入之後 (previousTH004 不為空) 且 TH004 改變時才插入小計行
            if (!string.IsNullOrEmpty(previousTH004) && currentTH004 != previousTH004)
            {
                // 計算真實的插入位置：e.Row.RowIndex (來自 GridView) + 已經插入的行數 + 1 (插入在下方)
                int actualInsertIndex = e.Row.RowIndex + insertedRowCount + 1;

                InsertSubtotalRow(actualInsertIndex, previousTH004);
                // 重要：插入成功後，更新偏移量
                insertedRowCount++;
            }
            SUB_TOTALNUMS += currentNUMS;
            SUB_TOTALMONEYS += currentMONEYS;
            SUB_TOTALCOSTS += currentCOSTS;

            // ----------------------------------------------------------------------
            // 4. 更新追蹤變數
            // ----------------------------------------------------------------------
            previousTH004 = currentTH004;

            // 可選：對 DataRow 中的金額進行格式化顯示
            //e.Row.Cells[4].Text = amount.ToString("N0"); // 三位一逗點，無小數點
        }

        // 判斷當前處理的是否為合計列 (Footer)
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            // B. 顯示合計文字和結果

            // 設置第一個儲存格顯示 "合計" 字樣 (索引 0)
            e.Row.Cells[3].Text = "小計 (" + currentTH004 + ")：";
            // 可選：設置對齊和粗體
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Font.Bold = true;

            // 設置金額合計結果 (索引 1)
            e.Row.Cells[4].Text = SUB_TOTALNUMS.ToString("N0"); // 格式化為三位一逗點
            e.Row.Cells[5].Text = SUB_TOTALMONEYS.ToString("N0"); // 格式化為三位一逗點
            e.Row.Cells[6].Text = SUB_TOTALCOSTS.ToString("N0"); // 格式化為三位一逗點

            // 設定樣式
            e.Row.BackColor = System.Drawing.Color.LightYellow; // 淺黃色背景
            e.Row.Font.Bold = true;
            e.Row.Font.Size = new FontUnit("14pt");

        }
    }

    /// <summary>
    /// 手動插入一個小計行到 GridView
    /// </summary>
    private void InsertSubtotalRow(int currentRowIndex, string th004Value)
    {
        // 建立新的 GridView Row
        GridViewRow subTotalRow = new GridViewRow(currentRowIndex, -1, DataControlRowType.Separator, DataControlRowState.Normal);

        // 依照 GridView 的欄位數量建立儲存格
        for (int i = 0; i < Grid2.Columns.Count; i++)
        {
            TableCell cell = new TableCell();
            cell.Text = "&nbsp;"; // 預設空白
            subTotalRow.Cells.Add(cell);
        }

        // 合併 TH004 之前的欄位，顯示小計資訊 (假設是第 0, 1, 2 欄位)
        subTotalRow.Cells[0].Text = "小計 (" + th004Value + ")：";
        subTotalRow.Cells[0].ColumnSpan =4; // 合併欄位，假設前面有 3 個不顯示數字的欄位
        subTotalRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;

        // 移除合併後多餘的儲存格，以確保 GridView 結構正確
        // 注意：您需要根據 GridView 的實際欄位數量來調整這個移除邏輯
        for (int i = 1; i < 3; i++)
        {
            subTotalRow.Cells.RemoveAt(1);
        }

        // 填充小計結果 (注意儲存格索引會因為 ColumnSpan 而改變)
        // 假設小計結果分別放在索引 1, 2, 3 的位置
        // 如果您合併了 3 欄 (0, 1, 2)，新的 Cell[1] 就是原來的 Cell[4]

        // 設定樣式
        subTotalRow.BackColor = System.Drawing.Color.LightYellow; // 淺黃色背景
        subTotalRow.Font.Bold = true;
        subTotalRow.Font.Size = new FontUnit("14pt");

        // 填充小計結果 (索引需要調整，如果合併 3 欄，TOTALNUMS 會在 Cell[1])
        subTotalRow.Cells[1].Text = SUB_TOTALNUMS.ToString("N0");
        subTotalRow.Cells[2].Text = SUB_TOTALMONEYS.ToString("N0");
        subTotalRow.Cells[3].Text = SUB_TOTALCOSTS.ToString("N0");

        // 將小計行插入
        Grid2.Controls[0].Controls.AddAt(currentRowIndex, subTotalRow);

        // 重置小計變數，準備下一組的計算
        SUB_TOTALNUMS = 0.00m;
        SUB_TOTALMONEYS = 0.00m;
        SUB_TOTALCOSTS = 0.00m;
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
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder SQL_QUERY1 = new StringBuilder();
        StringBuilder cmdTxt = new StringBuilder();

        string DATESTART = TextBox7.Text.Trim();
        string DATESEND = TextBox8.Text.Trim();
      
        // 2. 定義 SQL 查詢字串           
        cmdTxt.AppendFormat(@"
                            --20260609 來客數
                            SELECT TT002 AS '門代'
                            ,STORESNAME AS '門店'
                            ,YEARS AS '年'
                            ,MONTHS AS '月'
                            ,WEEKS AS '週'
                            ,Fdate1 AS '日'
                            ,DAYOFWEEK AS '星期'
                            ,SUMNUMS AS '來客數'
                            ,ISNULL(CONVERT(INT,SUMTT018),0) AS '銷售未稅總金額'
                            ,ISNULL(COUNTSTA001,0) AS '交易筆數'
                            ,CONVERT(INT,SUMSTB019) AS '交易商品數'
                            ,(CASE WHEN SUMNUMS>0 AND COUNTSTA001>0 THEN CONVERT(DECIMAL(16,2),((CONVERT(DECIMAL(16,4),COUNTSTA001)/CONVERT(DECIMAL(16,4),SUMNUMS)))) ELSE 0 END ) AS '提袋率'
                            ,(CASE WHEN SUMTT018>0 AND COUNTSTA001>0 THEN CONVERT(DECIMAL(16,0),SUMTT018/COUNTSTA001) ELSE 0 END ) AS '客單價'
                            ,(CASE WHEN SUMSTB019>0 AND COUNTSTA001>0 THEN CONVERT(DECIMAL(16,2),SUMSTB019/COUNTSTA001) ELSE 0 END ) AS '每單平均商品數'
                            ,(CASE WHEN TT002 IN ('106701') THEN (
	                            SELECT COUNT(ID)
	                            FROM 
	                            [TKMK].[dbo].[GROUPSALES] WITH (NOLOCK)
	                            WHERE  STATUS='完成接團'  AND CONVERT(NVARCHAR, [CREATEDATES], 112)=Fdate1
                            ) ELSE 0 END) AS '團車數'
                            ,(
	                            SELECT COUNT(TA001)
	                            FROM [TK].dbo.POSTA
	                            WHERE (TA008 LIKE '68%' OR	TA008 LIKE '69%') AND TA002=TT002 AND TA001=Fdate1
                            ) AS '團客交易筆數'
                            FROM 
                            (
                                SELECT View_t_visitors.TT002,STORESNAME,YEARS,MONTHS,WEEKS,Fdate1,DAYOFWEEK,SUM(Fin_data+Fout_data)/2 AS SUMNUMS
                                ,(SELECT SUM(TT018) FROM [TK].dbo.POSTT WITH(NOLOCK) WHERE View_t_visitors.TT002=POSTT.TT002 AND View_t_visitors.Fdate1=POSTT.TT001) AS 'SUMTT018'
                                ,(SELECT SUM(TT008) FROM [TK].dbo.POSTT  WITH(NOLOCK) WHERE View_t_visitors.TT002=POSTT.TT002 AND View_t_visitors.Fdate1=POSTT.TT001) AS 'SUMTT008'
                                ,(SELECT COUNT(TA001) FROM [TK].dbo.POSTA WITH(NOLOCK)  WHERE  TA002=View_t_visitors.TT002 AND TA004=View_t_visitors.Fdate1) AS 'COUNTSTA001'
                                ,(SELECT SUM(TB019) FROM [TK].dbo.POSTB  WITH(NOLOCK) WHERE  TB002=View_t_visitors.TT002 AND TB004=View_t_visitors.Fdate1 AND TB010 NOT LIKE '1%'  AND TB010 NOT LIKE '2%'  AND TB010 NOT LIKE '3%') AS 'SUMSTB019'
                                FROM [TKMK].[dbo].[View_t_visitors]
                                WHERE  TT002 IN ('106501','106502','106503','106504','106513','106702','106703','106704','106705') 
                                AND CONVERT(NVARCHAR,Fdate1,112)>='{0}'
                                AND CONVERT(NVARCHAR,Fdate1,112)<='{1}'
                                GROUP BY View_t_visitors.TT002,STORESNAME,YEARS,MONTHS,WEEKS,Fdate1,DAYOFWEEK

                                UNION ALL
                                SELECT View_t_visitors.TT002,STORESNAME,YEARS,MONTHS,WEEKS,Fdate1,DAYOFWEEK,SUM(Fout_data) AS SUMNUMS
                                ,(SELECT SUM(TT018) FROM [TK].dbo.POSTT WITH(NOLOCK) WHERE View_t_visitors.TT002=POSTT.TT002 AND View_t_visitors.Fdate1=POSTT.TT001) AS 'SUMTT011'
                                ,(SELECT SUM(TT008) FROM [TK].dbo.POSTT WITH(NOLOCK)  WHERE View_t_visitors.TT002=POSTT.TT002 AND View_t_visitors.Fdate1=POSTT.TT001) AS 'SUMTT008'
                                ,(SELECT COUNT(TA001) FROM [TK].dbo.POSTA WITH(NOLOCK)  WHERE  TA002=View_t_visitors.TT002 AND TA004=View_t_visitors.Fdate1) AS 'COUNTSTA001'
                                ,(SELECT SUM(TB019) FROM [TK].dbo.POSTB  WITH(NOLOCK) WHERE  TB002=View_t_visitors.TT002 AND TB004=View_t_visitors.Fdate1 AND TB010 NOT LIKE '1%'  AND TB010 NOT LIKE '2%'  AND TB010 NOT LIKE '3%') AS 'SUMSTB019'
                                FROM [TKMK].[dbo].[View_t_visitors]
                                WHERE  TT002 IN ('106701') 
                                AND CONVERT(NVARCHAR,Fdate1,112)>='{0}'
                                AND CONVERT(NVARCHAR,Fdate1,112)<='{1}'
              
                                GROUP BY View_t_visitors.TT002,STORESNAME,YEARS,MONTHS,WEEKS,Fdate1,DAYOFWEEK
                            ) AS TEMP
                            ORDER BY TT002,Fdate1
                        ", DATESTART, DATESEND);
      
        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

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

    private void BindGrid4()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
          
        StringBuilder cmdTxt = new StringBuilder();

        StringBuilder SQL_QUERY1 = new StringBuilder();
        StringBuilder SQL_QUERY2 = new StringBuilder();
        StringBuilder SQL_QUERY3 = new StringBuilder();
        StringBuilder SQL_QUERY4 = new StringBuilder();
        StringBuilder SQL_QUERY5 = new StringBuilder();
        StringBuilder SQL_QUERY6 = new StringBuilder();
        StringBuilder SQL_QUERY7 = new StringBuilder();

        string DATESTART = TextBox9.Text.Trim();
        string DATESEND = TextBox10.Text.Trim();
        string TA002 = TextBox11.Text.Trim();
        string TA009 = TextBox12.Text.Trim();
        string TA014 = TextBox13.Text.Trim();
        string TB010 = TextBox14.Text.Trim();
        string TC008= TextBox15.Text.Trim();
        string TB036 = TextBox16.Text.Trim();

        if (!string.IsNullOrEmpty(DATESTART) && !string.IsNullOrEmpty(DATESEND))
        {
            SQL_QUERY1.AppendFormat(@"AND TA001>='{0}' AND TA001<='{1}'", DATESTART, DATESEND);
        }
        else
        {
            SQL_QUERY1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TA002) )
        {
            SQL_QUERY2.AppendFormat(@" AND TA002='{0}'", TA002);
        }
        else
        {
            SQL_QUERY2.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TA009))
        {
            SQL_QUERY3.AppendFormat(@" AND TA009='{0}'", TA009);
        }
        else
        {
            SQL_QUERY3.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TA014))
        {
            SQL_QUERY4.AppendFormat(@" AND TA014='{0}'", TA014);
        }
        else
        {
            SQL_QUERY4.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TB010))
        {
            SQL_QUERY5.AppendFormat(@" AND (TB010 LIKE '%{0}%' OR INVMB.MB002  LIKE '%{0}%')", TB010);
        }
        else
        {
            SQL_QUERY5.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TC008))
        {
            SQL_QUERY6.AppendFormat(@" AND TC008='{0}'", TC008);
        }
        else
        {
            SQL_QUERY6.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TB036))
        {
            SQL_QUERY7.AppendFormat(@" AND TB036='{0}'", TB036);
        }
        else
        {
            SQL_QUERY7.AppendFormat(@"");
        }

        // 2. 定義 SQL 查詢字串           
        cmdTxt.AppendFormat(@"      
                            SELECT 
                            TA002 AS '門市'
                            ,TA009 AS '會員'
                            ,TA014 AS '發票'
                            ,TB010 AS '品號'
                            ,INVMB.MB002 AS '品名'
                            ,TB019 AS '銷售數量'
                            ,TB031 AS '未稅金額'
                            ,TC008 AS '付款ID'
                            ,MT003 AS '付款別'
                            ,TB036 AS '特價代號'
                            ,POSMB.MB004 AS '特價'
                            ,MI004 AS '組合品搭贈'
                            ,MM004 AS '滿額折價'

                            FROM [TK].dbo.POSTA,[TK].dbo.POSTB
                            LEFT JOIN [TK].dbo.INVMB ON INVMB.MB001=TB010
                            LEFT JOIN [TK].dbo.POSMB ON POSMB.MB003=TB036
                            LEFT JOIN [TK].dbo.POSMI ON MI003=TB036
                            LEFT JOIN [TK].dbo.POSMM ON MM003=TB036
                            ,[TK].dbo.POSTC,[TK].dbo.POSMT
                            WHERE TA001=TB001 AND TA002=TB002 AND TA003=TB003 AND TA006=TB006
                            AND TA001=TC001 AND TA002=TC002 AND TA003=TC003 AND TA006=TC006
                            AND TC008=MT002 
                            AND TB042 IN ('1','2','3','6','7','8','9')
                            AND (TB010 LIKE '4%' OR  TB010 LIKE '5%')
                            {0}
                            {1}
                            {2}
                            {3}
                            {4}
                            {5}
                            {6}

                            ORDER BY TA002,TB010
                        ", SQL_QUERY1.ToString()
                        , SQL_QUERY2.ToString()
                        , SQL_QUERY3.ToString()
                        , SQL_QUERY4.ToString()
                        , SQL_QUERY5.ToString()
                        , SQL_QUERY6.ToString()
                        , SQL_QUERY7.ToString());

        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));
        EXPORT_DT4 = dt;

        Grid4.DataSource = dt;
        Grid4.DataBind();

       
    }

    protected void grid_PageIndexChanging4(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid4_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport4(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL4();
        MsgBox("MsgBox!!!!" , this.Page, this);

    }
    public void SETEXCEL4()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        StringBuilder SQL_QUERY1 = new StringBuilder();
        StringBuilder SQL_QUERY2 = new StringBuilder();
        StringBuilder SQL_QUERY3 = new StringBuilder();
        StringBuilder SQL_QUERY4 = new StringBuilder();
        StringBuilder SQL_QUERY5 = new StringBuilder();
        StringBuilder SQL_QUERY6 = new StringBuilder();
        StringBuilder SQL_QUERY7 = new StringBuilder();

        string DATESTART = TextBox9.Text.Trim();
        string DATESEND = TextBox10.Text.Trim();
        string TA002 = TextBox11.Text.Trim();
        string TA009 = TextBox12.Text.Trim();
        string TA014 = TextBox13.Text.Trim();
        string TB010 = TextBox14.Text.Trim();
        string TC008 = TextBox15.Text.Trim();
        string TB036 = TextBox16.Text.Trim();

        if (!string.IsNullOrEmpty(DATESTART) && !string.IsNullOrEmpty(DATESEND))
        {
            SQL_QUERY1.AppendFormat(@"AND TA001>='{0}' AND TA001<='{1}'", DATESTART, DATESEND);
        }
        else
        {
            SQL_QUERY1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TA002))
        {
            SQL_QUERY2.AppendFormat(@" AND TA002='{0}'", TA002);
        }
        else
        {
            SQL_QUERY2.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TA009))
        {
            SQL_QUERY3.AppendFormat(@" AND TA009='{0}'", TA009);
        }
        else
        {
            SQL_QUERY3.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TA014))
        {
            SQL_QUERY4.AppendFormat(@" AND TA014='{0}'", TA014);
        }
        else
        {
            SQL_QUERY4.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TB010))
        {
            SQL_QUERY5.AppendFormat(@" AND (TB010 LIKE '%{0}%' OR INVMB.MB002  LIKE '%{0}%')", TB010);
        }
        else
        {
            SQL_QUERY5.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TC008))
        {
            SQL_QUERY6.AppendFormat(@" AND TC008='{0}'", TC008);
        }
        else
        {
            SQL_QUERY6.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TB036))
        {
            SQL_QUERY7.AppendFormat(@" AND TB036='{0}'", TB036);
        }
        else
        {
            SQL_QUERY7.AppendFormat(@"");
        }

        // 2. 定義 SQL 查詢字串           
        cmdTxt.AppendFormat(@"      
                            SELECT 
                            TA002 AS '門市'
                            ,TA009 AS '會員'
                            ,TA014 AS '發票'
                            ,TB010 AS '品號'
                            ,INVMB.MB002 AS '品名'
                            ,TB019 AS '銷售數量'
                            ,TB031 AS '未稅金額'
                            ,TC008 AS '付款ID'
                            ,MT003 AS '付款別'
                            ,TB036 AS '特價代號'
                            ,POSMB.MB004 AS '特價'
                            ,MI004 AS '組合品搭贈'
                            ,MM004 AS '滿額折價'

                            FROM [TK].dbo.POSTA,[TK].dbo.POSTB
                            LEFT JOIN [TK].dbo.INVMB ON INVMB.MB001=TB010
                            LEFT JOIN [TK].dbo.POSMB ON POSMB.MB003=TB036
                            LEFT JOIN [TK].dbo.POSMI ON MI003=TB036
                            LEFT JOIN [TK].dbo.POSMM ON MM003=TB036
                            ,[TK].dbo.POSTC,[TK].dbo.POSMT
                            WHERE TA001=TB001 AND TA002=TB002 AND TA003=TB003 AND TA006=TB006
                            AND TA001=TC001 AND TA002=TC002 AND TA003=TC003 AND TA006=TC006
                            AND TC008=MT002 
                            AND TB042 IN ('1','2','3','6','7','8','9')
                            AND (TB010 LIKE '4%' OR  TB010 LIKE '5%')
                            {0}
                            {1}
                            {2}
                            {3}
                            {4}
                            {5}
                            {6}

                            ORDER BY TA002,TB010
                        ", SQL_QUERY1.ToString()
                        , SQL_QUERY2.ToString()
                        , SQL_QUERY3.ToString()
                        , SQL_QUERY4.ToString()
                        , SQL_QUERY5.ToString()
                        , SQL_QUERY6.ToString()
                        , SQL_QUERY7.ToString());

        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        if (dt != null && dt.Rows.Count > 0)
        {
            // 定義匯出對應 [資料庫欄位, Excel標題]
            var exportMap = new Dictionary<string, string> {
            { "門市", "門市" },
            { "會員", "會員" },
            { "發票", "發票" } ,
            { "品號", "品號" },
            { "品名", "品名" },
            { "銷售數量", "銷售數量" } ,
            { "未稅金額", "未稅金額" },
            { "付款ID", "付款ID" },
            { "付款別", "付款別" } ,
            { "特價代號", "特價代號" },
            { "特價", "特價" },
            { "組合品搭贈", "組合品搭贈" } ,
            { "滿額折價", "滿額折價" }
        };

            // 指定哪些欄位要以「整數」匯出 (對應 Excel 標題名)
            List<string> intFields = new List<string> { "數量", "未稅金額" };

            var fileName = "清單_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excel = new ExcelPackage())
            {
                var ws = excel.Workbook.Worksheets.Add("Sheet1");

                // --- 核心優化：整塊載入資料 ---
                // 先過濾出需要的欄位
                DataTable dtExport = dt.DefaultView.ToTable(false, exportMap.Keys.ToArray());
                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    dtExport.Columns[i].ColumnName = exportMap.Values.ElementAt(i);
                }

                // 寫入 Excel (從 A1 開始，包含標題)
                ws.Cells["A1"].LoadFromDataTable(dtExport, true);

                int rowCount = dtExport.Rows.Count;
                int colCount = dtExport.Columns.Count;

                // --- 處理整數型態與格式 ---
                for (int col = 1; col <= colCount; col++)
                {
                    string headerName = ws.Cells[1, col].Text;

                    if (intFields.Contains(headerName))
                    {
                        // 取得該欄位的資料範圍 (從第 2 列到最後一列)
                        var dataRange = ws.Cells[2, col, rowCount + 1, col];

                        // 1. 強制將儲存格內容轉為數值 (避免 Excel 顯示「以文字儲存的數字」警告)
                        // LoadFromDataTable 通常會處理型別，但如果原始 DataTable 是字串，這步很保險
                        dataRange.Style.Numberformat.Format = "#,##0"; // 千分位整數格式

                        // 2. 靠右對齊 (數值習慣靠右)
                        dataRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }
                }

                // --- 整體樣式優化 ---
                using (var allCells = ws.Cells[1, 1, rowCount + 1, colCount])
                {
                    allCells.Style.Font.Name = "微軟正黑體";
                    allCells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    allCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    allCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    allCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    allCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // 標題列美化
                using (var header = ws.Cells[1, 1, 1, colCount])
                {
                    header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    header.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                    header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    header.Style.Font.Bold = true;
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //自適應高度設定
                ws.Row(1).CustomHeight = true;

                //儲存Excel
                //Byte[] bin = excel.GetAsByteArray();
                //File.WriteAllBytes(@"C:\TEMP\" + fileName, bin);

                //儲存和歸來的Excel檔案作為一個ByteArray
                var data = excel.GetAsByteArray();
                HttpResponse response = HttpContext.Current.Response;
                Response.Clear();

                //輸出標頭檔案　　
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + "");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data);
                Response.Flush();
                Response.End();
            }
        }
    }



    private void BindGrid5()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        StringBuilder SQL_QUERY1 = new StringBuilder();
        StringBuilder SQL_QUERY2 = new StringBuilder();
        StringBuilder SQL_QUERY3 = new StringBuilder();
        StringBuilder SQL_QUERY4 = new StringBuilder();
        StringBuilder SQL_QUERY5 = new StringBuilder();
        StringBuilder SQL_QUERY6 = new StringBuilder();
        StringBuilder SQL_QUERY7 = new StringBuilder();

        string DATESTART = TextBox17.Text.Trim();
        string DATESEND = TextBox18.Text.Trim();
        string TC008 = DropDownList1.SelectedValue.ToString();

        if (!string.IsNullOrEmpty(DATESTART) && !string.IsNullOrEmpty(DATESEND))
        {
            SQL_QUERY1.AppendFormat(@"AND  TC004>='{0}' AND TC004<='{1}'", DATESTART, DATESEND);
        }
        else
        {
            SQL_QUERY1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TC008))
        {
            SQL_QUERY2.AppendFormat(@"AND TC008 = '{0}' ", TC008);
        }
        else
        {
            SQL_QUERY2.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(TC008))
        {
            SQL_QUERY3.AppendFormat(@"AND  TC2.TC008 = '{0}' ", TC008);
        }
        else
        {
            SQL_QUERY3.AppendFormat(@"");
        }

        // 2. 定義 SQL 查詢字串           
        cmdTxt.AppendFormat(@"      
                            --20261006 查付款別的折扣.sql

                            WITH OrderTotals AS (
                                -- 第一階段：依單據主鍵 (TC001, TC002, TC003, TC006) 算每筆交易的總金額
                                SELECT 
                                    TC001,
                                    TC002,
                                    TC003,
                                    TC006,
                                    SUM(TC009) AS SingleOrderAmount
                                FROM [TK].dbo.POSTC WITH (NOLOCK)
                                WHERE 1=1
                                    {1}
                                GROUP BY TC001, TC002, TC003, TC006
                            )

                            -- 正確的 EXISTS 寫法：將 POSTC 與 CTE (OrderTotals) 進行關聯
                            SELECT 
                            TC002 '門代'
                            ,ME002 '門市'
                            ,(SELECT COUNT(TC008) FROM [TK].dbo.POSTC TC2 WHERE 1=1 AND TC2.TC002=P.TC002 {2} {0}) '交易筆數'
                            ,SUM(P.TC009) AS 銷售總金額
                            FROM [TK].dbo.POSTC P WITH (NOLOCK)
                            LEFT JOIN [TK].dbo.CMSME ON ME001=TC002
                            WHERE EXISTS (
                                SELECT 1 
                                FROM OrderTotals O
                                WHERE P.TC001 = O.TC001
                                  AND P.TC002 = O.TC002
                                  AND P.TC003 = O.TC003
                                  AND P.TC006 = O.TC006
                            )
                            {0}
                            GROUP BY TC002,ME002
                            ORDER BY TC002,ME002

                        ", SQL_QUERY1.ToString()
                        , SQL_QUERY2.ToString()
                        , SQL_QUERY3.ToString()
                        , SQL_QUERY4.ToString()
                        , SQL_QUERY5.ToString()
                        , SQL_QUERY6.ToString()
                        , SQL_QUERY7.ToString());

        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));
        //EXPORT_DT5 = dt;

        Grid5.DataSource = dt;
        Grid5.DataBind();


    }

    protected void grid_PageIndexChanging5(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid5_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid5_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport5(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL5();
        MsgBox("MsgBox!!!!", this.Page, this);

    }

    public void SETEXCEL5()
    {

    }

    public void MsgBox(String ex, Page pg, Object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);

        // MsgBox("MsgBox!!!!    " + error + "\r\n" + Form.OuterXml, this.Page, this);
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

        Label_query_dates.Text = "查詢日期區間: "+TextBox4.Text.Trim() + " ~ " + TextBox5.Text.Trim();
        Label_query_dates.Font.Size = new FontUnit("16pt"); 
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        BindGrid3();

        Label_query_dates3.Text = "查詢日期區間: " + TextBox7.Text.Trim() + " ~ " + TextBox8.Text.Trim();
        Label_query_dates3.Font.Size = new FontUnit("16pt");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        BindGrid4();
      

    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        SETEXCEL4();
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        BindGrid5();
    }
    #endregion
}