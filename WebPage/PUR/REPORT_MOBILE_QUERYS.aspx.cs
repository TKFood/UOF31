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

public partial class CDS_WebPage_PUR_REPORT_MOBILE_QUERYS : Ede.Uof.Utility.Page.BasePage
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
            BindGrid();
            BindGrid2();
            BindGrid3();
            BindGrid4();
            BindGrid5();
            BindGrid6();
            BindGrid7();
            BindGrid8();
            BindGrid9();
            BindGrid10();
        }
    }
    #region FUNCTION
    private void BindGrid()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"
                        SELECT
                        LA001 AS '品號'
                        ,MB002  AS '品名'
                        ,SUM(LA005*LA011)  AS '庫存數量'
                        , MB004  AS '單位'
                        FROM [TK].dbo.INVLA WITH(NOLOCK )
                        LEFT JOIN [TK].dbo.INVMB ON MB001=LA001
                        WHERE LA001 IN
                        (
                        SELECT  [MB001]
                        FROM [TKPUR].[dbo].[UOF_QUERYS]
                        )
                        AND LA009 IN (SELECT  [LA009] FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS_INVLA])
                        GROUP BY LA001,MB002, MB004
                        ORDER BY LA001
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

    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();
        //string newFileGroupID = FileCenter.Clone("A表單的FILE_GROUP_ID", Module.WKF);
       

    }
    private void BindGrid2()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"                        
                        SELECT 
                        TD012 AS '預交日'
                        ,TC001 AS '採購單'
                        ,TC002 AS '採購單號'
                        ,TD004 AS '品號'
                        ,TD005 AS '品名'
                        ,TD008 AS '採購數量'
                        ,TD015 AS '已進貨數量'
                        ,TD009 AS '單位'
                        FROM [TK].dbo.PURTC,[TK].dbo.PURTD
                        WHERE TC001=TD001 AND TC002=TD002
                        AND TD007 IN (SELECT  [TD007]  FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS])
                        AND TD002>='20260408001'
                        AND TD004 IN
                        (
                        SELECT  [MB001]
                        FROM [TKPUR].[dbo].[UOF_QUERYS]
                        )
                        AND TD008>TD015
                        ORDER BY TD004,TC003

                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


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
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"
                        SELECT
                        LA001 AS '品號'
                        ,MB002  AS '品名'
                        ,SUM(LA005*LA011)  AS '庫存數量'
                        , MB004  AS '單位'
                        FROM [TK].dbo.INVLA WITH(NOLOCK )
                        LEFT JOIN [TK].dbo.INVMB ON MB001=LA001
                        WHERE LA001 IN
                        (
                        SELECT  [MB001]
                        FROM  [TKPUR].[dbo].[UOF_QUERYS_BUYS]
                        )
                        AND LA009 IN (SELECT  [LA009] FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS_INVLA])
                        GROUP BY LA001,MB002, MB004
                        ORDER BY LA001
                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


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

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"                        
                        SELECT 
                        TD012 AS '預交日'
                        ,TC001 AS '採購單'
                        ,TC002 AS '採購單號'
                        ,TD004 AS '品號'
                        ,TD005 AS '品名'
                        ,TD008 AS '採購數量'
                        ,TD015 AS '已進貨數量'
                        ,TD009 AS '單位'
                        FROM [TK].dbo.PURTC,[TK].dbo.PURTD
                        WHERE TC001=TD001 AND TC002=TD002
                        AND TD007 IN (SELECT  [TD007]  FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS])
                        AND TD002>='20260408001'
                        AND TD004 IN
                        (
                        SELECT  [MB001]
                        FROM  [TKPUR].[dbo].[UOF_QUERYS_BUYS]
                        )
                        AND TD008>TD015
                        ORDER BY TD004,TC003

                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


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
        //SETEXCEL();

    }

    private void BindGrid5()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"
                        SELECT
                        LA001 AS '品號'
                        ,MB002  AS '品名'
                        ,SUM(LA005*LA011)  AS '庫存數量'
                        , MB004  AS '單位'
                        FROM [TK].dbo.INVLA WITH(NOLOCK )
                        LEFT JOIN [TK].dbo.INVMB ON MB001=LA001
                        WHERE LA001 IN
                        (
                        SELECT  [MB001]
                        FROM   [TKPUR].[dbo].[UOF_QUERYS_DRINKS]
                        )
                        AND LA009 IN (SELECT  [LA009] FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS_INVLA])
                        GROUP BY LA001,MB002, MB004
                        ORDER BY LA001
                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


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
        //SETEXCEL();

    }
    private void BindGrid6()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"                        
                        SELECT 
                        TD012 AS '預交日'
                        ,TC001 AS '採購單'
                        ,TC002 AS '採購單號'
                        ,TD004 AS '品號'
                        ,TD005 AS '品名'
                        ,TD008 AS '採購數量'
                        ,TD015 AS '已進貨數量'
                        ,TD009 AS '單位'
                        FROM [TK].dbo.PURTC,[TK].dbo.PURTD
                        WHERE TC001=TD001 AND TC002=TD002
                        AND TD007 IN (SELECT  [TD007]  FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS])
                        AND TD002>='20260408001'
                        AND TD004 IN
                        (
                        SELECT  [MB001]
                        FROM  [TKPUR].[dbo].[UOF_QUERYS_DRINKS]
                        )
                        AND TD008>TD015
                        ORDER BY TD004,TC003

                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        Grid6.DataSource = dt;
        Grid6.DataBind();

    }

    protected void grid_PageIndexChanging6(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid6_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid6_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport6(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }
    private void BindGrid7()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"
                        SELECT
                        LA001 AS '品號'
                        ,MB002  AS '品名'
                        ,SUM(LA005*LA011)  AS '庫存數量'
                        , MB004  AS '單位'
                        FROM [TK].dbo.INVLA WITH(NOLOCK )
                        LEFT JOIN [TK].dbo.INVMB ON MB001=LA001
                        WHERE LA001 IN
                        (
                        SELECT  [MB001]
                        FROM   [TKPUR].[dbo].[UOF_QUERYS_PACKAGE]
                        )
                        AND LA009 IN (SELECT  [LA009] FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS_INVLA])
                        GROUP BY LA001,MB002, MB004
                        ORDER BY LA001
                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        Grid7.DataSource = dt;
        Grid7.DataBind();

    }

    protected void grid_PageIndexChanging7(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid7_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid7_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport7(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }
    private void BindGrid8()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"                        
                        SELECT 
                        TD012 AS '預交日'
                        ,TC001 AS '採購單'
                        ,TC002 AS '採購單號'
                        ,TD004 AS '品號'
                        ,TD005 AS '品名'
                        ,TD008 AS '採購數量'
                        ,TD015 AS '已進貨數量'
                        ,TD009 AS '單位'
                        FROM [TK].dbo.PURTC,[TK].dbo.PURTD
                        WHERE TC001=TD001 AND TC002=TD002
                        AND TD007 IN (SELECT  [TD007]  FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS])
                        AND TD002>='20260408001'
                        AND TD004 IN
                        (
                        SELECT  [MB001]
                        FROM   [TKPUR].[dbo].[UOF_QUERYS_PACKAGE]
                        )
                        AND TD008>TD015
                        ORDER BY TD004,TC003

                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        Grid8.DataSource = dt;
        Grid8.DataBind();

    }

    protected void grid_PageIndexChanging8(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid8_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid8_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport8(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();       

    }
    private void BindGrid9()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"
                        SELECT
                        LA001 AS '品號'
                        ,MB002  AS '品名'
                        ,SUM(LA005*LA011)  AS '庫存數量'
                        , MB004  AS '單位'
                        FROM [TK].dbo.INVLA WITH(NOLOCK )
                        LEFT JOIN [TK].dbo.INVMB ON MB001=LA001
                        WHERE LA001 IN
                        (
                        SELECT  [MB001]
                        FROM  [TKPUR].[dbo].[UOF_QUERYS_TASTE]
                        )
                        AND LA009 IN (SELECT  [LA009] FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS_INVLA])
                        GROUP BY LA001,MB002, MB004
                        ORDER BY LA001
                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        Grid9.DataSource = dt;
        Grid9.DataBind();

    }

    protected void grid_PageIndexChanging9(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid9_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid9_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport9(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }
    private void BindGrid10()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢字串           
        string cmdTxt = @"                        
                        SELECT 
                        TD012 AS '預交日'
                        ,TC001 AS '採購單'
                        ,TC002 AS '採購單號'
                        ,TD004 AS '品號'
                        ,TD005 AS '品名'
                        ,TD008 AS '採購數量'
                        ,TD015 AS '已進貨數量'
                        ,TD009 AS '單位'
                        FROM [TK].dbo.PURTC,[TK].dbo.PURTD
                        WHERE TC001=TD001 AND TC002=TD002
                        AND TD007 IN (SELECT  [TD007]  FROM [TKPUR].[dbo].[UOF_QUERYS_SOTCKS])
                        AND TD002>='20260408001'
                        AND TD004 IN
                        (
                        SELECT  [MB001]
                        FROM [TKPUR].[dbo].[UOF_QUERYS_TASTE]
                        )
                        AND TD008>TD015
                        ORDER BY TD004,TC003

                        ";

        //m_db.AddParameter("@DATESTART", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        Grid10.DataSource = dt;
        Grid10.DataBind();

    }

    protected void grid_PageIndexChanging10(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid10_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid10_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }


    public void OnBeforeExport10(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

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
        BindGrid2();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        BindGrid3();
        BindGrid4();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {

    }
    protected void Button4_Click(object sender, EventArgs e)
    {

    }
    protected void Button5_Click(object sender, EventArgs e)
    {

    }

    #endregion
}