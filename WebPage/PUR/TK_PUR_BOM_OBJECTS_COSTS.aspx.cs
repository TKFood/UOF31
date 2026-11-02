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
public partial class CDS_WebPage_PUR_TK_PUR_BOM_OBJECTS_COSTS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    DataTable EXCELDT1 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
           
        }

    }


    #region FUNCTION
    private void BindGrid1(string MB001)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();


        if (!string.IsNullOrEmpty(MB001))
        {

            cmdTxt.AppendFormat(@"  
                                WITH RecursiveBOM AS (
                                -- 1. 錨點查詢（第一層）：直接計算第 1 階用量 (MC004 * MD006 / MD007)
                                SELECT 
                                    1 AS [階層],
                                    MC1.MC001 AS [主要品號],
                                    MC1.MC001 AS [主件品號],
                                    MD1.MD003 AS [元件品號],
                                    CAST(1.0 AS FLOAT) AS [上層傳遞用量], -- 第一層基準
                                    ISNULL(MC1.MC004, 1) AS [當層標準批量],
                                    ISNULL(MD1.MD006, 0) AS [當層組成用量],
                                    ISNULL(MD1.MD007, 1) AS [當層底數],
                                    -- 🎯 第 1 階用量公式：MC004 * MD006 / MD007
                                    CAST(
                                        ISNULL(MC1.MC004, 1) * ISNULL(MD1.MD006, 0) * 1.0 / NULLIF(ISNULL(MD1.MD007, 1), 0) 
                                        AS FLOAT
                                    ) AS [遞迴累計總用量],
                                    CAST(MC1.MC001 + ' -> ' + ISNULL(MD1.MD003, '') AS NVARCHAR(MAX)) AS [階層路徑]
                                FROM [TK].dbo.BOMMC MC1 WITH(NOLOCK)
                                INNER JOIN [TK].dbo.INVMB MB1 WITH(NOLOCK) 
                                    ON MB1.MB001 = MC1.MC001
                                LEFT JOIN [TK].dbo.BOMMD MD1 WITH(NOLOCK) 
                                    ON MD1.MD001 = MC1.MC001
                                WHERE 1=1
                                  AND (MB1.MB001 LIKE '4%' OR MB1.MB001 LIKE '5%')     
                                  AND (MB1.MB001 LIKE '%{0}%' OR MB1.MB002 LIKE '%{0}%')

                                UNION ALL

                                -- 2. 遞迴查詢（第二層起）：套用 (上層傳遞用量 / 當層標準批量) * 當層組成用量 / 當層底數
                                SELECT 
                                    R.[階層] + 1 AS [階層],
                                    R.[主要品號] AS [主要品號],
                                    MD.MD001 AS [主件品號],     
                                    MD.MD003 AS [元件品號],
                                    R.[遞迴累計總用量] AS [上層傳遞用量], -- 繼承上一層算出的總用量
                                    ISNULL(MC.MC004, 1) AS [當層標準批量],
                                    ISNULL(MD.MD006, 0) AS [當層組成用量],
                                    ISNULL(MD.MD007, 1) AS [當層底數],
                                    -- 🎯 關鍵修正公式：(上層傳遞用量 / 當層標準批量) * 當層組成用量 / 當層底數
                                    CAST(
                                        (R.[遞迴累計總用量] / NULLIF(ISNULL(MC.MC004, 1), 0)) * ISNULL(MD.MD006, 0) * 1.0 / NULLIF(ISNULL(MD.MD007, 1), 0)
                                        AS FLOAT
                                    ) AS [遞迴累計總用量],
                                    CAST(R.[階層路徑] + ' -> ' + MD.MD003 AS NVARCHAR(MAX)) AS [階層路徑]
                                FROM RecursiveBOM R
                                INNER JOIN [TK].dbo.BOMMC MC WITH(NOLOCK) 
                                    ON MC.MC001 = R.[元件品號]
                                INNER JOIN [TK].dbo.BOMMD MD WITH(NOLOCK) 
                                    ON MD.MD001 = MC.MC001
                            )
                            -- 3. 最外層輸出結果
                            SELECT 
                                R.[主要品號],
                                MB0.MB002 AS [主要品名],         
                                R.[階層],
                                R.[主件品號],
                                MB1.MB002 AS [主件品名],
                                R.[元件品號],
                                MB2.MB002 AS [元件品名],
                                ROUND(R.[上層傳遞用量], 4) AS [上層傳遞用量],
                                R.[當層標準批量],
                                R.[當層組成用量],
                                R.[當層底數],
                                -- 🎯 修正後算出的精準總用量
                                ROUND(R.[遞迴累計總用量], 4) AS [遞迴計算總用量],
                                MB2.MB004 AS '單位',
                                MB2.MB050 AS '最近進貨價',
                                ROUND(R.[遞迴累計總用量] * ISNULL(MB2.MB050, 0), 2) AS [預估物料總金額],
                                R.[階層路徑]
                            FROM RecursiveBOM R
                            LEFT JOIN [TK].dbo.INVMB MB0 WITH(NOLOCK) 
                                ON MB0.MB001 = R.[主要品號]
                            LEFT JOIN [TK].dbo.INVMB MB1 WITH(NOLOCK) 
                                ON MB1.MB001 = R.[主件品號]
                            LEFT JOIN [TK].dbo.INVMB MB2 WITH(NOLOCK) 
                                ON MB2.MB001 = R.[元件品號]
                            WHERE R.[元件品號] LIKE '2%'
                            ORDER BY R.[主要品號], R.[階層路徑];
                                ", MB001);


        }




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

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

        MsgBox("MsgBox!!!!", this.Page, this);

    }
    public void MsgBox(String ex, Page pg, Object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);

        //string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        //Type cstype = obj.GetType();
        //ClientScriptManager cs = pg.ClientScript;
        //cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }
    #endregion

    #region BUTTON
    protected void btn1_Click(object sender, EventArgs e)
    {
        string MB001 = TextBox1.Text.Trim();
        BindGrid1(MB001);
    }

    #endregion
}