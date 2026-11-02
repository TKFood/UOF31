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

public partial class CDS_WebPage_PUR_TB_PUR_WORKS : Ede.Uof.Utility.Page.BasePage
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
            // 頁面第一次載入時執行綁定
            BindProgressDropDownList1();
            Bind_FIND_DropDownList1();

        }
    }
    #region FUNCTION

    private void BindProgressDropDownList1()
    {
        // 1. 取得連線字串 (使用您現有的 ERPconnectionstring)
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢 (請根據您的資料表名稱與欄位進行修改)
        // 假設您的進度表是存放於某個設定檔或參數檔中
        string cmdTxt = @"SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKPUR].[dbo].[TBPARA] WHERE [KIND]='TB_PUR_WORKS' ORDER BY [PARANAME]";

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(cmdTxt));

        // 3. 執行資料繫結
        ADD_DropDownList_處理進度.DataSource = dt;
        ADD_DropDownList_處理進度.DataTextField = "PARAID";   // 在下拉選單顯示的文字
        ADD_DropDownList_處理進度.DataValueField = "PARAID"; // 實際上代表的值
        ADD_DropDownList_處理進度.DataBind();

        // 4. 插入預設選項 (選填)
        //ADD_DropDownList_處理進度.Items.Insert(0, new ListItem("請選擇", ""));
    }
    private void Bind_FIND_DropDownList1()
    {
        // 1. 取得連線字串 (使用您現有的 ERPconnectionstring)
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 定義 SQL 查詢 (請根據您的資料表名稱與欄位進行修改)
        // 假設您的進度表是存放於某個設定檔或參數檔中
        string cmdTxt = @"SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKPUR].[dbo].[TBPARA] WHERE [KIND]='TB_PUR_WORKS' UNION ALL SELECT 99,'','全部',99 ORDER BY [PARANAME]";

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(cmdTxt));

        // 3. 執行資料繫結
        FIND_DropDownList1.DataSource = dt;
        FIND_DropDownList1.DataTextField = "PARAID";   // 在下拉選單顯示的文字
        FIND_DropDownList1.DataValueField = "PARAID"; // 實際上代表的值
        FIND_DropDownList1.DataBind();
        
    }
    private void BindGrid()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder SQLQUERY1 = new StringBuilder();
        StringBuilder SQLQUERY2 = new StringBuilder();
        StringBuilder SQLQUERY3 = new StringBuilder();
        string OWNER = FIND_TextBox_負責承辦.Text;
        string WORKOBJECTS = FIND_TextBox_工作對象.Text;
        string STATUS = FIND_DropDownList1.Text;

        if (!string.IsNullOrEmpty(OWNER))
        {
            SQLQUERY1.AppendFormat(@" AND OWNER LIKE '%{0}%'", OWNER);
        }
        else { SQLQUERY1.AppendFormat(@" "); }
        if (!string.IsNullOrEmpty(WORKOBJECTS))
        {
            SQLQUERY2.AppendFormat(@" AND WORKOBJECTS LIKE '%{0}%'", WORKOBJECTS);
        }
        else { SQLQUERY2.AppendFormat(@" "); }

        if (!string.IsNullOrEmpty(STATUS))
        {
            if(STATUS.Equals("全部"))
            {
                SQLQUERY3.AppendFormat(@" ");
            }
            else
            {
                SQLQUERY3.AppendFormat(@" AND STATUS='{0}'", STATUS);
            }                
        }
        


        // 2. 定義 SQL 查詢字串 
        cmdTxt.AppendFormat(@"
                            SELECT 
                            [ID]
                            ,[OWNER] AS '負責承辦'
                            ,[WORKOBJECTS] AS '工作對象'
                            ,[PROCESS] AS '處理情況敘述'
                            ,[FINISHDATES] AS '預計完成日期'
                            ,[STATUS] AS '處理進度'
                            ,[COMMENTS] AS '備註說明'
                            ,[MANAGEREPLY] AS '主管交辦'
                            FROM [TKPUR].[dbo].[TB_PUR_WORKS]
                            WHERE 1=1
                            {0}
                            {1}
                            {2}
                            ORDER BY [ID]
                            ", SQLQUERY1.ToString(), SQLQUERY2.ToString(), SQLQUERY3.ToString());          
       

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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 1. 尋找控制項
            DropDownList ddlProgress = (DropDownList)e.Row.FindControl("DropDownList_處理進度");
            HiddenField hfValue = (HiddenField)e.Row.FindControl("Hidden_處理進度");

            if (ddlProgress != null)
            {
                // 2. 取得 SQL 資料 (建議將此 SQL 邏輯抽出成一個 DataTable 以免重複查詢多次)
                DataTable dtOptions = GetProgressOptions();

                ddlProgress.DataSource = dtOptions;
                ddlProgress.DataTextField = "PARAID";  // 顯示的文字 (請依 SQL 欄位修改)
                ddlProgress.DataValueField = "PARAID"; // 實際的值 (請依 SQL 欄位修改)
                ddlProgress.DataBind();

                // 3. 加入預設空白選項
                ddlProgress.Items.Insert(0, new ListItem("請選擇", ""));

                // 4. 設定選取值 (對應資料庫原本的值)
                if (hfValue != null && !string.IsNullOrEmpty(hfValue.Value))
                {
                    ddlProgress.SelectedValue = hfValue.Value;
                }
            }
        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        // 1. 檢查 CommandName 是否是您定義的更新命令
        if (e.CommandName == "MYUPDATE")
        {
            string ID = e.CommandArgument.ToString();
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

            TextBox txt_OWNER = (TextBox)row.FindControl("TextBox_負責承辦");
            string OWNER = txt_OWNER.Text;
            TextBox txt_WORKOBJECTS = (TextBox)row.FindControl("TextBox_工作對象");
            string WORKOBJECTS = txt_WORKOBJECTS.Text;
            TextBox txt_PROCESS = (TextBox)row.FindControl("TextBox_處理情況敘述");
            string PROCESS = txt_PROCESS.Text;
            TextBox txt_FINISHDATES = (TextBox)row.FindControl("TextBox_預計完成日期");
            string FINISHDATES = txt_FINISHDATES.Text;
            TextBox txt_COMMENTS = (TextBox)row.FindControl("TextBox_備註說明");
            string COMMENTS = txt_COMMENTS.Text;
            TextBox txt_MANAGEREPLY = (TextBox)row.FindControl("TextBox_主管交辦");
            string MANAGEREPLY = txt_MANAGEREPLY.Text;

            DropDownList drd_STATUS=(DropDownList)row.FindControl("DropDownList_處理進度");
            string STATUS = drd_STATUS.Text;

            UODATE_TB_PUR_WORKS(
             ID
            , OWNER
            , WORKOBJECTS
            , PROCESS
            , FINISHDATES
            , STATUS
            , COMMENTS
            , MANAGEREPLY
            );

            MsgBox("完成 \r\n " + ID, this.Page, this);
        }
        else  if (e.CommandName == "MUDELETE")
        {
            string ID = e.CommandArgument.ToString();
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

            DELETE_TB_PUR_WORKS(ID);

            MsgBox("完成 \r\n " + ID, this.Page, this);
        }

        BindGrid();
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    // 取得下拉選單資料的專用方法
    private DataTable GetProgressOptions()
    {
        // 使用您現有的 DatabaseHelper
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 這裡替換成您實際要查詢進度清單的 SQL
        string cmdTxt = @"SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKPUR].[dbo].[TBPARA] WHERE [KIND]='TB_PUR_WORKS' ORDER BY [PARANAME]";

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(cmdTxt));
        return dt;
    }

    public void UODATE_TB_PUR_WORKS(
          string ID
         , string OWNER
         , string WORKOBJECTS
         , string PROCESS
         , string FINISHDATES
         , string STATUS
         , string COMMENTS
        ,string MANAGEREPLY
     )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;

        // 1. 📌 使用參數化查詢，避免 SQL Injection
        string sqlQuery = @"
                            UPDATE [TKPUR].[dbo].[TB_PUR_WORKS]
                            SET [OWNER]=@OWNER
                            ,[WORKOBJECTS]=@WORKOBJECTS
                            ,[PROCESS]=@PROCESS
                            ,[FINISHDATES]=@FINISHDATES
                            ,[STATUS]=@STATUS
                            ,[COMMENTS]=@COMMENTS
                            ,[MANAGEREPLY]=@MANAGEREPLY
                            WHERE [ID]=@ID
                          
                            ";

        // 2. 📌 包裹在 Try-Catch 區塊中，處理例外狀況
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 加入參數，將值安全地傳遞給 SQL 查詢
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@OWNER", OWNER);
                    command.Parameters.AddWithValue("@WORKOBJECTS", WORKOBJECTS);
                    command.Parameters.AddWithValue("@PROCESS", PROCESS);
                    command.Parameters.AddWithValue("@FINISHDATES", FINISHDATES);
                    command.Parameters.AddWithValue("@STATUS", STATUS);
                    command.Parameters.AddWithValue("@COMMENTS", COMMENTS);
                    command.Parameters.AddWithValue("@MANAGEREPLY", MANAGEREPLY);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // 檢查是否有資料被更新
                    if (rowsAffected > 0)
                    {
                        MsgBox("完成 \r\n ", this.Page, this);
                    }
                    else
                    {
                        // 雖然執行成功，但沒有任何資料列被影響 (可能 ID 找不到)                       
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void DELETE_TB_PUR_WORKS(
      string ID   
     )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;

        // 1. 📌 使用參數化查詢，避免 SQL Injection
        string sqlQuery = @"
                            DELETE [TKPUR].[dbo].[TB_PUR_WORKS]                          
                            WHERE [ID]=@ID
                          
                            ";

        // 2. 📌 包裹在 Try-Catch 區塊中，處理例外狀況
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 加入參數，將值安全地傳遞給 SQL 查詢
                    command.Parameters.AddWithValue("@ID", ID);
                  
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // 檢查是否有資料被更新
                    if (rowsAffected > 0)
                    {
                        MsgBox("完成 \r\n ", this.Page, this);
                    }
                    else
                    {
                        // 雖然執行成功，但沒有任何資料列被影響 (可能 ID 找不到)                       
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void ADD_TB_PUR_WORKS(
     string ID
    , string OWNER
    , string WORKOBJECTS
    , string PROCESS
    , string FINISHDATES
    , string STATUS
    , string COMMENTS
    ,string MANAGEREPLY
    )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;

        // 1. 📌 使用參數化查詢，避免 SQL Injection
        string sqlQuery = @"
                            INSERT INTO  [TKPUR].[dbo].[TB_PUR_WORKS]
                            ([OWNER],[WORKOBJECTS],[PROCESS],[FINISHDATES],[STATUS],[COMMENTS],[MANAGEREPLY])
                            VALUES
                            (@OWNER,@WORKOBJECTS,@PROCESS,@FINISHDATES,@STATUS,@COMMENTS,@MANAGEREPLY)
                          
                            ";

        // 2. 📌 包裹在 Try-Catch 區塊中，處理例外狀況
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 加入參數，將值安全地傳遞給 SQL 查詢                   
                    command.Parameters.AddWithValue("@OWNER", OWNER);
                    command.Parameters.AddWithValue("@WORKOBJECTS", WORKOBJECTS);
                    command.Parameters.AddWithValue("@PROCESS", PROCESS);
                    command.Parameters.AddWithValue("@FINISHDATES", FINISHDATES);
                    command.Parameters.AddWithValue("@STATUS", STATUS);
                    command.Parameters.AddWithValue("@COMMENTS", COMMENTS);
                    command.Parameters.AddWithValue("@MANAGEREPLY", MANAGEREPLY);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // 檢查是否有資料被更新
                    if (rowsAffected > 0)
                    {
                        MsgBox("完成 \r\n ", this.Page, this);
                    }
                    else
                    {
                        // 雖然執行成功，但沒有任何資料列被影響 (可能 ID 找不到)                       
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
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
        string OWNER = ADD_TextBox_負責承辦.Text.Trim();
        string WORKOBJECTS = ADD_TextBox_工作對象.Text.Trim();
        string PROCESS = ADD_TextBox_處理情況敘述.Text.Trim();
        string FINISHDATES = ADD_TextBox_預計完成日期.Text.Trim();
        string STATUS = ADD_DropDownList_處理進度.Text.Trim();
        string COMMENTS = ADD_TextBox_備註說明.Text.Trim();
        string MANAGEREPLY = ADD_TextBox_主管交辦.Text.Trim();

        ADD_TB_PUR_WORKS(
              ID
            , OWNER
            , WORKOBJECTS
            , PROCESS
            , FINISHDATES
            , STATUS
            , COMMENTS
            , MANAGEREPLY
            );

        BindGrid();

    }

    #endregion
}