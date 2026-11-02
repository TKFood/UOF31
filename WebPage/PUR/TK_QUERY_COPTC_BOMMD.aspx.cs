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

public partial class CDS_WebPage_PUR_TK_QUERY_COPTC_BOMMD : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            SETYEARSWEEKS();
        }
    }

    #region FUNCTION

    public void SETYEARSWEEKS()
    {
        txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
        txtDate2.Text = DateTime.Now.ToString("yyyy/MM/dd");
        

    }
    private void BindGrid()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        StringBuilder SQL_QUERY1 = new StringBuilder();
        StringBuilder SQL_QUERY2 = new StringBuilder();

        DateTime DT_SDATES = Convert.ToDateTime(txtDate1.Text);
        string SDATES = DT_SDATES.ToString("yyyyMMdd");
        DateTime DT_EDATES = Convert.ToDateTime(txtDate2.Text);
        string EDATES = DT_EDATES.ToString("yyyyMMdd");

        string MB001 = TextBox1.Text.Trim();
        string TC002 = TextBox2.Text.Trim();

        if(!string.IsNullOrEmpty(MB001))
        {
            SQL_QUERY1.AppendFormat(@" AND  (B.TD004 LIKE '%{0}%' OR B.TD005 LIKE '%{0}%') ", MB001);
        }
        else
        {
            SQL_QUERY1.AppendFormat(@" ");
        }
        if (!string.IsNullOrEmpty(TC002))
        {
            SQL_QUERY2.AppendFormat(@" AND B.TC002 LIKE '%{0}%' ", TC002);
        }
        else
        {
            SQL_QUERY2.AppendFormat(@" ");
        }

        // 2. 定義 SQL 查詢字串           
        cmdTxt.AppendFormat(@"      
                             --20260910 查訂單的bom
                            -- 20260910 查訂單的 BOM（多層展開遞迴版 - 修正型別問題）
                            -- 20260910 查訂單 BOM 與近 30 天所有分批採購明細（保留多筆採購 + 效能優化版）

                            WITH OrderBOM AS (
                                -------------------------------------------------------------------
                                -- Anchor Member（錨點）：找出符合條件的訂單第一階品號
                                -------------------------------------------------------------------
                                SELECT 
                                    1 AS [BOM_Level],                                      
                                    CAST(TD.TD004 AS NVARCHAR(1000)) AS [BOM_Path], 
                                    TC.TC001,
                                    TC.TC002,
                                    TC.TC003,
                                    TC.TC053,
                                    TD.TD003,
                                    TD.TD004,
                                    TD.TD005,
                                    TD.TD008,
                                    TD.TD009,
                                    TD.TD024,
                                    TD.TD025,
                                    TD.TD010,
                                    TD.TD013,
                                    CAST(TD.TD004 AS NVARCHAR(100)) AS [Parent_MD001],
                                    CAST(MD.MD003 AS NVARCHAR(100)) AS [Child_MD003],
                                    MD.MD002 AS [MD002_Seq]
                                FROM [TK].dbo.COPTC TC WITH (NOLOCK)
                                INNER JOIN [TK].dbo.COPTD TD WITH (NOLOCK) 
                                    ON TC.TC001 = TD.TD001 
                                   AND TC.TC002 = TD.TD002
                                LEFT JOIN [TK].dbo.BOMMD MD WITH (NOLOCK) 
                                    ON MD.MD001 = TD.TD004
                                WHERE TC.TC027 IN ('Y', 'N')
                                  AND (TD.TD009 + TD.TD024) < (TD.TD008 + TD.TD025)
                                  AND TC.TC003 >= '{0}' 
                                  AND TC.TC003 <= '{1}'

                                UNION ALL

                                -------------------------------------------------------------------
                                -- Recursive Member（遞迴）：展開 BOM 樹狀結構
                                -------------------------------------------------------------------
                                SELECT 
                                    P.[BOM_Level] + 1 AS [BOM_Level],     
                                    CAST(P.[BOM_Path] + N' > ' + ISNULL(C.MD003, N'') AS NVARCHAR(1000)) AS [BOM_Path],
                                    P.TC001,
                                    P.TC002,
                                    P.TC003,
                                    P.TC053,
                                    P.TD003,
                                    P.TD004,
                                    P.TD005,
                                    P.TD008,
                                    P.TD009,
                                    P.TD024,
                                    P.TD025,
                                    P.TD010,
                                    P.TD013,
                                    CAST(C.MD001 AS NVARCHAR(100)) AS [Parent_MD001],
                                    CAST(C.MD003 AS NVARCHAR(100)) AS [Child_MD003],
                                    C.MD002 AS [MD002_Seq]
                                FROM OrderBOM P
                                INNER JOIN [TK].dbo.BOMMD C WITH (NOLOCK) 
                                    ON P.[Child_MD003] = C.MD001          
                            )
                            -------------------------------------------------------------------
                            -- 主查詢結果輸出
                            -------------------------------------------------------------------
                            SELECT 
                                B.[BOM_Level]       AS 'BOM階層',
                                B.[BOM_Path]        AS '展階路徑',
                                B.TC001             AS '訂單單別',
                                B.TC002             AS '訂單單號',
                                B.TC003             AS '單據日期',
                                B.TC053             AS '客戶簡稱',
                                B.TD003             AS '序號',
                                B.TD004             AS '訂單品號',
                                B.TD005             AS '品名',
                                B.TD008             AS '訂單數量',
                                B.TD009             AS '已交數量',
                                B.TD024             AS '庫存數量',
                                B.TD025             AS '贈品數量',
                                B.TD010             AS '單位',
                                B.TD013             AS '預計交貨日',
                                B.Parent_MD001      AS '上層料號(MD001)',
                                B.Child_MD003       AS '下層元件料號MD003',
                                MB.MB002            AS '元件品名',
                                PUR.TB001           AS '請購單別',
                                PUR.TB002           AS '請購單號',
                                PUR.TB009           AS '請購數量',
                                PUR.TB007           AS '請購單位',
                                PUR.TB011           AS '預計交貨日'
                            FROM OrderBOM B
                            -- 串接品號主檔 (只顯示元件料號開頭為 2 的品項)
                            INNER JOIN [TK].dbo.INVMB MB WITH (NOLOCK) 
                                ON MB.MB001 = B.Child_MD003
                            -- 使用 OUTER APPLY：保留所有符合條件的分批採購單，不限制 TOP 1，且比 Derived Table 更省資源
                            OUTER APPLY (
                                SELECT 
                                    TB.TB001,
                                    TB.TB002,
                                    TB.TB009,
                                    TB.TB007,
                                    TB.TB011
                                FROM [TK].dbo.PURTB TB WITH (NOLOCK)
                                INNER JOIN [TK].dbo.PURTA TA WITH (NOLOCK) 
                                    ON TA.TA001 = TB.TB001 
                                   AND TA.TA002 = TB.TB002
                                WHERE TB.TB004 = B.Child_MD003
                                  AND TA.TA007 IN ('Y', 'N')
                                  AND TB.TB009 > 0
                                  -- 將比較運算子右移，確保索引 Seek 正常運作
                                  AND TB.TB011 > CONVERT(VARCHAR(8), DATEADD(DAY, -30, GETDATE()), 112)
                            ) PUR
                            WHERE 1=1
                            AND B.Child_MD003 LIKE '2%'
                            {2}
                            {3}
                            ORDER BY 
                                B.TC001, 
                                B.TC002, 
                                B.TD003, 
                                B.BOM_Path,
                                PUR.TB011 DESC, -- 採購單按預計交貨日由近到遠排序
                                PUR.TB001,
                                PUR.TB002
                            OPTION (MAXRECURSION 32);
                                                    ", SDATES, EDATES, SQL_QUERY1.ToString(), SQL_QUERY2.ToString()
                        );

        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();

        if (!string.IsNullOrEmpty(SDATES)&& !string.IsNullOrEmpty(EDATES))
        {
            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            Grid1.DataSource = dt;
            Grid1.DataBind();
        }

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
      
        MsgBox("MsgBox!!!!", this.Page, this);

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



    #endregion
}