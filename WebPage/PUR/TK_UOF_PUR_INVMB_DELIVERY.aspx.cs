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

public partial class CDS_WebPage_PUR_TK_UOF_PUR_INVMB_DELIVERY : Ede.Uof.Utility.Page.BasePage
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
            BindKindsDropDown();
            BindGrid2();
            BindGrid3();
            BindGrid4();
            BindGrid5();
            BindGrid6();
            BindGrid7();
            BindGrid8();
            BindGrid9();
        }
    }

    #region FUNCTION
    private void BindKindsDropDown()
    {
        // SQL 只需抓取資料庫現有的類別
        string sql = @"
                    SELECT  
                    '全部' AS KINDS
                    UNION ALL
                    SELECT  
                    [KINDS]
                    FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                    GROUP BY [KINDS]
                    ";

        // 這裡替換成您的連線字串
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            ddlSearchKinds.DataSource = dt;
            ddlSearchKinds.DataTextField = "KINDS"; // 顯示的文字
            ddlSearchKinds.DataValueField = "KINDS"; // 選取的值
            ddlSearchKinds.DataBind();
        }
    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        //KINDS
        string KINDS = ddlSearchKinds.Text.ToString();
        if (KINDS.Equals("全部"))
        {
            QUERYS1.AppendFormat(@" ");
        }
        else if (!KINDS.Equals("全部") && !string.IsNullOrEmpty(KINDS))
        {
            QUERYS1.AppendFormat(@" AND KINDS LIKE'%{0}%' ", KINDS);
        }      
        else
        {
            QUERYS1.AppendFormat(@" ");
        }
        //MB001
        string MB001 = FIND_TextBox1.Text.Trim();
        if (!string.IsNullOrEmpty(MB001))
        {
            QUERYS2.AppendFormat(@" AND (MB001 LIKE '%{0}%' OR MB002 LIKE '%{0}%')", MB001);
        }
        else
        {
            QUERYS2.AppendFormat(@" ");
        }

        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY [KINDS],[ID]

                            ", QUERYS1.ToString(), QUERYS2.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        //匯出專用
        EXCELDT1 = dt;

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlKinds");
            if (ddl != null)
            {
                // 1. 取得所有選項 (這部分建議改用全域變數，避免每一列都查一次資料庫)
                DataTable dtKinds = GetKindsList();
                ddl.DataSource = dtKinds;
                ddl.DataTextField = "KINDS";
                ddl.DataValueField = "KINDS";
                ddl.DataBind();

                // 2. 取得這一列資料庫真正的 KINDS 值
                // DataItem 是這一列繫結的原始資料
                string dbValue = DataBinder.Eval(e.Row.DataItem, "KINDS").ToString();

                // 3. 安全檢查：清單中是否有這個值？
                if (ddl.Items.FindByValue(dbValue) != null)
                {
                    ddl.SelectedValue = dbValue;
                }
                else
                {
                    // 如果找不到，手動加進去，避免崩潰，或者顯示「未知」
                    ddl.Items.Insert(0, new ListItem("未知(" + dbValue + ")", dbValue));
                    ddl.SelectedValue = dbValue;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn2 = (Button)e.Row.FindControl("Button2");
            if (btn2 != null)
            {
                string cellValue2 = btn2.CommandArgument;
                dynamic param2 = new { ID = cellValue2 }.ToExpando();
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn3 = (Button)e.Row.FindControl("Button3");
            if (btn3 != null)
            {
                string cellValue3 = btn3.CommandArgument;
                dynamic param3 = new { ID = cellValue3 }.ToExpando();
            }
        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

        // 初始化變數
        string ID = "";
        string KINDS = "";
        string MB001 = "";
        string MB002 = "";
        string MB003 = "";
        string MOQ = "";
        string UNITS = "";
        string DELIVERYDATS = "";

        if (rowIndex >= 0 && rowIndex < Grid1.Rows.Count)
        {
            GridViewRow row = Grid1.Rows[rowIndex];

            // 使用 FindControl 並加入防呆檢查 (避免找不控制項導致 NullReferenceException)
            Label lbl編號 = (Label)row.FindControl("Label_ID");
            DropDownList dd類別 = (DropDownList)row.FindControl("ddlKinds");
            TextBox txt品號 = (TextBox)row.FindControl("txtNewField_GV1_品號");
            TextBox txt品名 = (TextBox)row.FindControl("txtNewField_GV1_品名");
            TextBox txt規格 = (TextBox)row.FindControl("txtNewField_GV1_規格");
            TextBox txt最低採購量 = (TextBox)row.FindControl("txtNewField_GV1_最低採購量");
            TextBox txt單位 = (TextBox)row.FindControl("txtNewField_GV1_單位");
            TextBox txt交期 = (TextBox)row.FindControl("txtNewField_GV1_交期");


            // 賦值(C# 5.0 相容寫法)
            ID = (lbl編號 != null) ? lbl編號.Text : "";
            KINDS = dd類別.Text.Trim();
            MB001 = txt品號.Text.Trim();
            MB002 = txt品名.Text.Trim();
            MB003 = txt規格.Text.Trim();
            MOQ = txt最低採購量.Text.Trim();
            UNITS = txt單位.Text.Trim();
            DELIVERYDATS = txt交期.Text.Trim();

            // --- 邏輯判斷區 (修正括號層級) ---

            if (e.CommandName == "Button2")
            {
                // 保持原始 ISCLOSED
                ADD_UOF_PUR_INVMB_DELIVERY(
                ID
                , KINDS
                , MB001
                , MB002
                , MB003
                , MOQ
                , UNITS
                , DELIVERYDATS
                );
                MsgBox(ID + " 儲存完成", this.Page, this);
            }


            if (e.CommandName == "Button3")
            {
                // 保持原始 ISCLOSED
                DELETE_UOF_PUR_INVMB_DELIVERY(ID );

                MsgBox(ID + " 完成", this.Page, this);
            }

            // 最後重新繫結
            BindGrid();
            BindGrid2();
            BindGrid3();
            BindGrid4();
            BindGrid5();
            BindGrid6();
            BindGrid7();
            BindGrid8();
            BindGrid9();
        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

    }


    public void SETEXCEL()
    {

    }

    private void BindGrid2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            AND [KINDS]='彩盒'
                            ORDER BY [KINDS],[ID]

                            ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {      

    }
    private void BindGrid3()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            AND [KINDS]='袋'
                            ORDER BY [KINDS],[ID]

                            ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid3.DataSource = dt;
        Grid3.DataBind();
    }

    protected void grid_PageIndexChanging3(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

    }


    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }
    private void BindGrid4()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            AND [KINDS]='原料'
                            ORDER BY [KINDS],[ID]

                            ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid4.DataSource = dt;
        Grid4.DataBind();
    }

    protected void grid_PageIndexChanging4(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void Grid4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

    }


    public void OnBeforeExport4(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }
    private void BindGrid5()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            AND [KINDS]='外購品'
                            ORDER BY [KINDS],[ID]

                            ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid5.DataSource = dt;
        Grid5.DataBind();
    }

    protected void grid_PageIndexChanging5(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid5_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void Grid5_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

    }


    public void OnBeforeExport5(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }
    private void BindGrid6()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            AND [KINDS]='代工'
                            ORDER BY [KINDS],[ID]

                            ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid6.DataSource = dt;
        Grid6.DataBind();
    }

    protected void grid_PageIndexChanging6(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid6_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void Grid6_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

    }


    public void OnBeforeExport6(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }

    private void BindGrid7()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            AND [KINDS]='大陸製包材'
                            ORDER BY [KINDS],[ID]

                            ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid7.DataSource = dt;
        Grid7.DataBind();
    }

    protected void grid_PageIndexChanging7(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid7_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void Grid7_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

    }

    public void OnBeforeExport7(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }

    private void BindGrid8()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            AND [KINDS]='數位袋'
                            ORDER BY [KINDS],[ID]

                            ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid8.DataSource = dt;
        Grid8.DataBind();
    }

    protected void grid_PageIndexChanging8(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid8_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void Grid8_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

    }

    public void OnBeforeExport8(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }

    private void BindGrid9()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        cmdTxt.AppendFormat(@"                              
                            SELECT 
                            [ID]
                            ,[KINDS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[MOQ]
                            ,[UNITS]
                            ,[DELIVERYDATS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            WHERE 1=1
                            AND [KINDS]='打樣'
                            ORDER BY [KINDS],[ID]

                            ");




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid9.DataSource = dt;
        Grid9.DataBind();
    }

    protected void grid_PageIndexChanging9(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid9_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void Grid9_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 過濾無關的 Command
        if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

    }

    public void OnBeforeExport9(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }

    private DataTable GetKindsList()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();

            cmdTxt.AppendFormat(@"                              
                            SELECT  
                            [KINDS]
                            FROM [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY]
                            GROUP BY [KINDS]
                            ");




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));
            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch(Exception EX)
        {
            return null;

        }
        finally
        {

        }
      

    }

    public void ADD_UOF_PUR_INVMB_DELIVERY(
        string ID
        , string KINDS
        , string MB001
        , string MB002
        , string MB003
        , string MOQ
        , string UNITS
        , string DELIVERYDATS
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        string SQLCOMMAND = @" 
                            MERGE [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY] AS TARGET
                            USING (VALUES (@ID, @KINDS, @MB001, @MB002, @MB003, @MOQ, @UNITS, @DELIVERYDATS)) 
                            AS SOURCE (ID, KINDS, MB001, MB002, MB003, MOQ, UNITS, DELIVERYDATS)
                            ON TARGET.ID = SOURCE.ID 

                            WHEN MATCHED THEN 
                                UPDATE SET 
                                    KINDS = SOURCE.KINDS,
                                    MB001 = SOURCE.MB001,
                                    MB002 = SOURCE.MB002,
                                    MB003 = SOURCE.MB003,
                                    MOQ = SOURCE.MOQ,
                                    UNITS = SOURCE.UNITS,
                                    DELIVERYDATS = SOURCE.DELIVERYDATS
             

                            WHEN NOT MATCHED THEN
                                INSERT ( KINDS, MB001, MB002, MB003, MOQ, UNITS, DELIVERYDATS)
                                VALUES  (SOURCE.KINDS, SOURCE.MB001, SOURCE.MB002, SOURCE.MB003, SOURCE.MOQ, SOURCE.UNITS, SOURCE.DELIVERYDATS);
                            ";


        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    // 2. 修正參數綁定，確保每個參數對應正確的 SQL 變數名稱
                    cmd.Parameters.AddWithValue("@ID", (object)ID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@KINDS", (object)KINDS ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MB001", (object)MB001 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MB002", (object)MB002 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MB003", (object)MB003 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MOQ", (object)MOQ ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UNITS", (object)UNITS ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DELIVERYDATS", (object)DELIVERYDATS ?? DBNull.Value);

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(ID + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // 建議至少記錄錯誤，方便除錯
            // Log(ex.Message); 
            throw;
        }
    }

    public void DELETE_UOF_PUR_INVMB_DELIVERY(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        string SQLCOMMAND = @" 
                            DELETE  [TKPUR].[dbo].[UOF_PUR_INVMB_DELIVERY] 
                            WHERE ID=@ID
                            ";


        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    // 2. 修正參數綁定，確保每個參數對應正確的 SQL 變數名稱
                    cmd.Parameters.AddWithValue("@ID", (object)ID ?? DBNull.Value);
                  

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(ID + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // 建議至少記錄錯誤，方便除錯
            // Log(ex.Message); 
            throw;
        }

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
    protected void Button1_Click(object sender, EventArgs e)
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

    }
    protected void btnADD_Click(object sender, EventArgs e)
    {
        string ID = "-1";
        string KINDS = ADD_TextBox1.Text.Trim();
        string MB001 = ADD_TextBox2.Text.Trim();
        string MB002 = ADD_TextBox3.Text.Trim();
        string MB003 = ADD_TextBox4.Text.Trim();
        string MOQ = ADD_TextBox5.Text.Trim();
        string UNITS = ADD_TextBox6.Text.Trim();
        string DELIVERYDATS = ADD_TextBox7.Text.Trim();

        ADD_UOF_PUR_INVMB_DELIVERY(
              ID
              , KINDS
              , MB001
              , MB002
              , MB003
              , MOQ
              , UNITS
              , DELIVERYDATS
              );
        MsgBox(" 完成", this.Page, this);

        BindGrid();
        BindGrid2();
        BindGrid3();
        BindGrid4();
        BindGrid5();
        BindGrid6();
        BindGrid7();
        BindGrid8();
        BindGrid9();
    }

    #endregion
}