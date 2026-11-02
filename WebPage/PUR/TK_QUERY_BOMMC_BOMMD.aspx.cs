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

public partial class CDS_WebPage_PUR_TK_QUERY_BOMMC_BOMMD : Ede.Uof.Utility.Page.BasePage
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
           
        }
    }

    #region FUNCTION
    private void BindGrid()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        StringBuilder SQL_QUERY1 = new StringBuilder();

        string MB001 = TextBox1.Text.Trim();
        if(!string.IsNullOrEmpty(MB001))
        {
            SQL_QUERY1.AppendFormat(@" AND ( M1.MB001 LIKE '%{0}%'  OR  M1.MB002 LIKE '%{0}%') ", MB001);
        }
        else
        {
            SQL_QUERY1.AppendFormat(@" ");
        }

        // 2. 定義 SQL 查詢字串           
        cmdTxt.AppendFormat(@"      
                             WITH BOM_CTE AS (
                            -- 1. 錨點成員 (Anchor)：定義遞迴的起點 (第一階母件)
                            SELECT 
                                C.MC001 AS ParentID,               -- 母件品號
                                D.MD003 AS ChildID,                -- 子件品號
                                1 AS BOMLevel,                     -- 階層深度 (第1階)
                                -- 建立路徑，方便看清結構脈絡，也可預防無窮迴圈
                                CAST(RTRIM(C.MC001) + ' > ' + RTRIM(D.MD003) AS VARCHAR(MAX)) AS BOMPath
                            FROM [TK].dbo.BOMMC AS C WITH(NOLOCK)
                            INNER JOIN [TK].dbo.BOMMD AS D WITH(NOLOCK) ON D.MD001 = C.MC001
                            -- 這裡套用您原本的起點過濾條件
                            INNER JOIN [TK].dbo.INVMB AS M1 WITH(NOLOCK) ON C.MC001 = M1.MB001
                            WHERE  1=1
                            {0}

                            UNION ALL

                            -- 2. 遞迴成員 (Recursive)：將上一步的子件(ChildID)當作母件，繼續往下找子件
                            SELECT 
                                D2.MD001 AS ParentID,
                                D2.MD003 AS ChildID,
                                M.BOMLevel + 1 AS BOMLevel,        -- 階層自動加 1
                                CAST(M.BOMPath + ' > ' + RTRIM(D2.MD003) AS VARCHAR(MAX)) AS BOMPath
                            FROM [TK].dbo.BOMMD AS D2 WITH(NOLOCK)
                            -- 核心關鍵：拿目前的 BOMMD 的母件去對接上一個階層的子件
                            INNER JOIN BOM_CTE AS M ON D2.MD001 = M.ChildID
                        )
                        -- 3. 最終查詢：將遞迴完的完整樹狀結構，關聯出品名
                        SELECT 
                            B.BOMLevel AS [階層],
                            B.ParentID AS [母件品號],
                            MB1.MB002  AS [母件品名],
                            CONVERT(INT,MB1.MB047)  AS 母件標準售價,
	                        CONVERT(NVARCHAR,MB1.MB023)+ (CASE WHEN CONVERT(NVARCHAR,MB1.MB198)='1' THEN '天' WHEN CONVERT(NVARCHAR,MB1.MB198)='2' THEN '月' WHEN CONVERT(NVARCHAR,MB1.MB198)='3' THEN '年' END) AS 母件有效天數,
                            B.ChildID  AS [元件品號],
                            MB2.MB002  AS [元件品名],
	                        MB2.MB003  AS [元件規格],
	                        (SELECT TOP 1 '進貨單:'+TG001+'-'+TG002+' 進貨數量:'+CONVERT(NVARCHAR,TH007)
		                        FROM [TK].dbo.PURTG,[TK].dbo.PURTH
		                        WHERE TG001=TH001 AND TG002=TH002
		                        AND TG013='Y'
		                        AND TH004= B.ChildID
		                        ORDER BY TG003 DESC) AS '最近進貨',
                            B.BOMPath  AS [完整展開路徑]
                        FROM BOM_CTE AS B
                        LEFT JOIN [TK].dbo.INVMB AS MB1 WITH(NOLOCK) ON B.ParentID = MB1.MB001
                        LEFT JOIN [TK].dbo.INVMB AS MB2 WITH(NOLOCK) ON B.ChildID = MB2.MB001
                        -- 這裡保留您對子件的過濾，若想看全部子階可以視情況調整此條件
                        WHERE B.ChildID LIKE '2%'
                        -- 依路徑排序，可以讓樹狀結構在畫面上依序呈現（母 > 子 > 孫）
                        ORDER BY B.BOMPath;
                        ", SQL_QUERY1.ToString()
                        );

        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();

        if (!string.IsNullOrEmpty(MB001))
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