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

public partial class CDS_WebPage_MARKETING_TK_GIFTSETS : Ede.Uof.Utility.Page.BasePage
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
            BindDropDownList1();

            BindGrid();
        }
    }

    #region FUNCTION
    private void BindDropDownList(DropDownList ddl, string sql, string textField, string valueField)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(sql));

        ddl.DataSource = dt;
        ddl.DataTextField = textField;
        ddl.DataValueField = valueField;
        ddl.DataBind();
    }

    private void BindDropDownList1()
    {
        string sql = @"
       SELECT 
        [KINDS]
        ,[PARASNAMES]
        ,[DVALUES]
        FROM [TKMARKETING].[dbo].[TBZPARAS]
        WHERE [KINDS]='TKGIFTSETS'
        ORDER BY [DVALUES]
                ";

        BindDropDownList(DropDownList1, sql, "PARASNAMES", "PARASNAMES");
    }
    private void BindGrid()
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder QUERY1 = new StringBuilder();
        StringBuilder QUERY2 = new StringBuilder();
        StringBuilder QUERY3 = new StringBuilder();
        // 2. 定義 SQL 查詢字串  
        string ISCLOSED = DropDownList1.SelectedValue.ToString();
        if (!string.IsNullOrEmpty(ISCLOSED) && ISCLOSED.Equals("N"))
        {
            QUERY1.AppendFormat(@" AND  ISCLOSED='N' ");
        }
        else if(!string.IsNullOrEmpty(ISCLOSED) && ISCLOSED.Equals("Y"))
        {
            QUERY1.AppendFormat(@" AND  ISCLOSED='Y' ");
        }
        else
        {
            QUERY1.AppendFormat(@"");
        }
        //MB002
        string MB002 = QUERY_TextBox1.Text.Trim();
        if (!string.IsNullOrEmpty(MB002) )
        {
            QUERY2.AppendFormat(@" AND MB002 LIKE '%{0}%' ", MB002);
        }       
        else
        {
            QUERY2.AppendFormat(@"");
        }
        //YEARS
        string YEARS = QUERY_TextBox2.Text.Trim();
        if (!string.IsNullOrEmpty(YEARS))
        {
            QUERY3.AppendFormat(@" AND YEARS LIKE '%{0}%' ", YEARS);
        }
        else
        {
            QUERY3.AppendFormat(@"");
        }

        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                        SELECT  
                        [ID]
                        ,[MB001]
                        ,[MB002]
                        ,[MB003]
                        ,[YEARS]
                        ,[PRICES]
                        ,[IPPRICES]
                        ,[DMPRICES]
                        ,[STORENUMS]
                        ,[ECOMMERCENUMS]
                        ,[TOURISHOPNUMS]
                        ,[INARMYNUMS]
                        ,[INOILNUMS]
                        ,[INSALENUMS]
                        ,[INPRNUMS]
                        ,[BOSSPRNUMS]
                        ,[STAFFNUMS]
                        ,[TOTALNUMS]
                        ,[PACKNUMS]
                        ,[PACKINDATES]
                        ,[PRODATES]
                        ,CONVERT(NVARCHAR,SDATES,111) AS 'SDATES'
	                    ,CONVERT(NVARCHAR,EDATES,111) AS 'EDATES'
	                    ,[SELLEDNUMS]
                        ,[ISCLOSED]
                        FROM [TKMARKETING].[dbo].[TKGIFTSETS]
                        WHERE 1=1
                        {0}
                        {1}
                        {2}
                        ORDER BY [YEARS],[MB001],[MB002]
                        ", QUERY1.ToString(), QUERY2.ToString(), QUERY3.ToString());
                
      
        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

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

        // 1. 檢查 CommandName 是否是您定義的更新命令
        if (e.CommandName == "UPDATE")
        {
            // 取得 ID (主鍵) - 這個是正確的
            string recordId = e.CommandArgument.ToString();

            // 2. 📌 正確地取得按鈕所在的 GridViewRow 物件
            // e.CommandSource 是按鈕物件，它的 NamingContainer 就是 GridViewRow
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

            // 3. 📌 使用 FindControl 取得 TextBox 和 DropDownList 控制項

            // 取得品名 (TextBox)
            TextBox txt_YEARS = (TextBox)row.FindControl("TextBox_年度");
            string YEARS = txt_YEARS.Text;
            TextBox txt_MB001 = (TextBox)row.FindControl("TextBox_品號");
            string MB001 = txt_MB001.Text;
            TextBox txt_MB002 = (TextBox)row.FindControl("TextBox_品名");
            string MB002 = txt_MB002.Text;
            TextBox txt_MB003 = (TextBox)row.FindControl("TextBox_箱入數");
            string MB003 = txt_MB003.Text;
            TextBox txt_PRICES = (TextBox)row.FindControl("TextBox_售價");
            string PRICES = txt_PRICES.Text;
            TextBox txt_IPPRICES = (TextBox)row.FindControl("TextBox_IP價");
            string IPPRICES = txt_IPPRICES.Text;
            TextBox txt_DMPRICES = (TextBox)row.FindControl("TextBox_DM價");
            string DMPRICES = txt_DMPRICES.Text;
            TextBox txt_STORENUMS = (TextBox)row.FindControl("TextBox_門市");
            string STORENUMS = txt_STORENUMS.Text;
            TextBox txt_ECOMMERCENUMS = (TextBox)row.FindControl("TextBox_電商");
            string ECOMMERCENUMS = txt_ECOMMERCENUMS.Text;
            TextBox txt_TOURISHOPNUMS = (TextBox)row.FindControl("TextBox_觀光");
            string TOURISHOPNUMS = txt_TOURISHOPNUMS.Text;
            TextBox txt_INARMYNUMS = (TextBox)row.FindControl("TextBox_國內國軍");
            string INARMYNUMS = txt_INARMYNUMS.Text;
            TextBox txt_INOILNUMS = (TextBox)row.FindControl("TextBox_國內中油");
            string INOILNUMS = txt_INOILNUMS.Text;
            TextBox txtINSALENUMS = (TextBox)row.FindControl("TextBox_國內經銷");
            string INSALENUMS = txtINSALENUMS.Text;
            TextBox txt_INPRNUMS = (TextBox)row.FindControl("TextBox_業務公關");
            string INPRNUMS = txt_INPRNUMS.Text;
            TextBox txt_BOSSPRNUMS = (TextBox)row.FindControl("TextBox_總經理公關");
            string BOSSPRNUMS = txt_BOSSPRNUMS.Text;
            TextBox txt_STAFFNUMS = (TextBox)row.FindControl("TextBox_員購");
            string STAFFNUMS = txt_STAFFNUMS.Text;
            TextBox txt_TOTALNUMS = (TextBox)row.FindControl("TextBox_加總");
            string TOTALNUMS = txt_TOTALNUMS.Text;
            TextBox txt_PACKNUMS = (TextBox)row.FindControl("TextBox_預估包材下單總量");
            string PACKNUMS = txt_PACKNUMS.Text;
            TextBox txt_PACKINDATES = (TextBox)row.FindControl("TextBox_預估包材到廠日");
            string PACKINDATES = txt_PACKINDATES.Text;
            TextBox txt_PRODATES = (TextBox)row.FindControl("TextBox_預估成品完成日");
            string PRODATES = txt_PRODATES.Text;
            TextBox txt_SDATES = (TextBox)row.FindControl("TextBox_銷售日期起");
            string SDATES = txt_SDATES.Text;
            TextBox txt_EDATES = (TextBox)row.FindControl("TextBox_銷售日期迄");
            string EDATES = txt_EDATES.Text;
            // 取得是否結案 (DropDownList)
            DropDownList ddlIsClosed = (DropDownList)row.FindControl("DropDownList_是否結案");
            string ISCLOSED = ddlIsClosed.SelectedValue;
            // 4. 取得其值
            if (MB002 != null )
            {
                // 5. 進行資料庫更新操作
                // 例如：
                // UpdateData(newPinMing, newIsClosed, primaryKey);

                UPDATE_TKGIFTSETS(
                    recordId     
                    , YEARS
                    , MB001
                    , MB002
                    , MB003
                    , PRICES
                    , IPPRICES
                    , DMPRICES
                    , STORENUMS
                    , ECOMMERCENUMS
                    , TOURISHOPNUMS
                    , INARMYNUMS
                    , INOILNUMS
                    , INSALENUMS
                    , INPRNUMS
                    , BOSSPRNUMS
                    , STAFFNUMS
                    , TOTALNUMS
                    , PACKNUMS
                    , PACKINDATES
                    , PRODATES
                    , SDATES
                    , EDATES
                    , ISCLOSED
                    );

                BindGrid();
            }
        }
    }
    // 雖然不應該被觸發，但定義它以避免 HttpCException
    protected void Grid1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // 什麼都不做，因為您不打算使用內建更新功能

        // 如果 GridView 處於編輯模式，這兩行可以讓它退出編輯模式
        Grid1.EditIndex = -1;
        // Grid1.DataBind(); 
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    private void UPDATE_TKGIFTSETS(
        string recordId
        , string YEARS
        , string MB001
        ,string MB002
        ,string MB003
        ,string PRICES
        ,string IPPRICES
        ,string DMPRICES
        ,string STORENUMS
        ,string ECOMMERCENUMS
        ,string TOURISHOPNUMS
        ,string INARMYNUMS
        ,string INOILNUMS
        ,string INSALENUMS
        ,string INPRNUMS
        ,string BOSSPRNUMS
        ,string STAFFNUMS
        ,string TOTALNUMS
        ,string PACKNUMS
        ,string PACKINDATES
        ,string PRODATES
        , string SDATES
        , string EDATES
        , string ISCLOSED
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;

        // 1. 📌 使用參數化查詢，避免 SQL Injection
        string sqlQuery = @"
                            UPDATE [TKMARKETING].[dbo].[TKGIFTSETS]
                            SET 
                            YEARS=@YEARS
                            ,MB001 = @MB001
                            ,MB002 = @MB002
                            ,MB003 =@MB003
                            ,PRICES=@PRICES
                            ,IPPRICES=@IPPRICES
                            ,DMPRICES=@DMPRICES
                            ,STORENUMS=@STORENUMS
                            ,ECOMMERCENUMS=@ECOMMERCENUMS
                            ,TOURISHOPNUMS=@TOURISHOPNUMS
                            ,INARMYNUMS=@INARMYNUMS
                            ,INOILNUMS=@INOILNUMS
                            ,INSALENUMS=@INSALENUMS
                            ,INPRNUMS=@INPRNUMS
                            ,BOSSPRNUMS=@BOSSPRNUMS
                            ,STAFFNUMS=@STAFFNUMS
                            ,TOTALNUMS=@TOTALNUMS
                            ,PACKNUMS=@PACKNUMS
                            ,PACKINDATES=@PACKINDATES
                            ,PRODATES=@PRODATES
                            ,SDATES=@SDATES
                            ,EDATES=@EDATES
                            ,ISCLOSED=@ISCLOSED
                            WHERE [ID] = @recordId

                            UPDATE  [TKMARKETING].[dbo].[TKGIFTSETS]
                            SET TOTALNUMS=STORENUMS+ECOMMERCENUMS+TOURISHOPNUMS+INARMYNUMS+INOILNUMS+INSALENUMS+INPRNUMS+BOSSPRNUMS+STAFFNUMS
                            ";

        // 2. 📌 包裹在 Try-Catch 區塊中，處理例外狀況
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 加入參數，將值安全地傳遞給 SQL 查詢
                    command.Parameters.AddWithValue("@YEARS", YEARS);
                    command.Parameters.AddWithValue("@MB001", MB001);
                    command.Parameters.AddWithValue("@MB002", MB002);
                    command.Parameters.AddWithValue("@MB003", MB003);
                    command.Parameters.AddWithValue("@PRICES", PRICES);
                    command.Parameters.AddWithValue("@IPPRICES", IPPRICES);
                    command.Parameters.AddWithValue("@DMPRICES", DMPRICES);
                    command.Parameters.AddWithValue("@STORENUMS", STORENUMS);
                    command.Parameters.AddWithValue("@ECOMMERCENUMS", ECOMMERCENUMS);
                    command.Parameters.AddWithValue("@TOURISHOPNUMS", TOURISHOPNUMS);
                    command.Parameters.AddWithValue("@INARMYNUMS", INARMYNUMS);
                    command.Parameters.AddWithValue("@INOILNUMS", INOILNUMS);
                    command.Parameters.AddWithValue("@INSALENUMS", INSALENUMS);
                    command.Parameters.AddWithValue("@INPRNUMS", INPRNUMS);
                    command.Parameters.AddWithValue("@BOSSPRNUMS", BOSSPRNUMS);
                    command.Parameters.AddWithValue("@STAFFNUMS", STAFFNUMS);
                    command.Parameters.AddWithValue("@TOTALNUMS", TOTALNUMS);
                    command.Parameters.AddWithValue("@PACKNUMS", PACKNUMS);
                    command.Parameters.AddWithValue("@PACKINDATES", PACKINDATES);
                    command.Parameters.AddWithValue("@PRODATES", PRODATES);
                    command.Parameters.AddWithValue("@ISCLOSED", ISCLOSED);
                    command.Parameters.AddWithValue("@SDATES", SDATES);
                    command.Parameters.AddWithValue("@EDATES", EDATES);
                    command.Parameters.AddWithValue("@recordId", recordId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // 檢查是否有資料被更新
                    if (rowsAffected > 0)
                    {
                        UPDATE_TKGIFTSETS_SELLEDNUMS();
                        MsgBox("更新完成 \r\n ", this.Page, this);
                    }
                    else
                    {
                       
                    }
                }
            }
        }      
        catch (Exception ex)
        {
        }
    }

    public void ADD_TKGIFTSETS
        (
        string YEARS,
        string MB001,
        string MB002,
        string MB003,
        string PRICES,
        string IPPRICES,
        string DMPRICES,
        string STORENUMS,
        string ECOMMERCENUMS,
        string TOURISHOPNUMS,
        string INARMYNUMS,
        string INOILNUMS,
        string INSALENUMS,
        string INPRNUMS,
        string BOSSPRNUMS,
        string STAFFNUMS,
        string TOTALNUMS,
        string PACKNUMS,
        string PACKINDATES,
        string PRODATES,
        object SDATES,
        object EDATES
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;


        // 1. 📌 使用參數化查詢，避免 SQL Injection
        string sqlQuery = @"
                            INSERT INTO [TKMARKETING].[dbo].[TKGIFTSETS]
                            (
                            [YEARS]
                            ,[MB001]
                            ,[MB002]
                            ,[MB003]
                            ,[PRICES]
                            ,[IPPRICES]
                            ,[DMPRICES]
                            ,[STORENUMS]
                            ,[ECOMMERCENUMS]
                            ,[TOURISHOPNUMS]
                            ,[INARMYNUMS]
                            ,[INOILNUMS]
                            ,[INSALENUMS]
                            ,[INPRNUMS]
                            ,[BOSSPRNUMS]
                            ,[STAFFNUMS]
                            ,[TOTALNUMS]
                            ,[PACKNUMS]
                            ,[PACKINDATES]
                            ,[PRODATES]
                            ,[SDATES]
                            ,[EDATES]
                            ,[ISCLOSED]
                            ) 
                            VALUES
                            (
                            @YEARS,
                            @MB001,
                            @MB002,
                            @MB003,
                            @PRICES,
                            @IPPRICES,
                            @DMPRICES,
                            @STORENUMS,
                            @ECOMMERCENUMS,
                            @TOURISHOPNUMS,
                            @INARMYNUMS,
                            @INOILNUMS,
                            @INSALENUMS,
                            @INPRNUMS,
                            @BOSSPRNUMS,
                            @STAFFNUMS,
                            @TOTALNUMS,
                            @PACKNUMS,
                            @PACKINDATES,
                            @PRODATES,
                            @SDATES,
                            @EDATES,
                            @ISCLOSED
                            )

                            UPDATE  [TKMARKETING].[dbo].[TKGIFTSETS]
                            SET TOTALNUMS=STORENUMS+ECOMMERCENUMS+TOURISHOPNUMS+INARMYNUMS+INOILNUMS+INSALENUMS+INPRNUMS+BOSSPRNUMS+STAFFNUMS
                            ";

        // 2. 📌 包裹在 Try-Catch 區塊中，處理例外狀況
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 加入參數，將值安全地傳遞給 SQL 查詢
                    command.Parameters.AddWithValue("@YEARS", YEARS);
                    command.Parameters.AddWithValue("@MB001", MB001);
                    command.Parameters.AddWithValue("@MB002", MB002);
                    command.Parameters.AddWithValue("@MB003", MB003);
                    command.Parameters.AddWithValue("@PRICES", PRICES);
                    command.Parameters.AddWithValue("@IPPRICES", IPPRICES);
                    command.Parameters.AddWithValue("@DMPRICES", DMPRICES);
                    command.Parameters.AddWithValue("@STORENUMS", STORENUMS);
                    command.Parameters.AddWithValue("@ECOMMERCENUMS", ECOMMERCENUMS);
                    command.Parameters.AddWithValue("@TOURISHOPNUMS", TOURISHOPNUMS);
                    command.Parameters.AddWithValue("@INARMYNUMS", INARMYNUMS);
                    command.Parameters.AddWithValue("@INOILNUMS", INOILNUMS);
                    command.Parameters.AddWithValue("@INSALENUMS", INSALENUMS);
                    command.Parameters.AddWithValue("@INPRNUMS", INPRNUMS);
                    command.Parameters.AddWithValue("@BOSSPRNUMS", BOSSPRNUMS);
                    command.Parameters.AddWithValue("@STAFFNUMS", STAFFNUMS);
                    command.Parameters.AddWithValue("@TOTALNUMS", TOTALNUMS);
                    command.Parameters.AddWithValue("@PACKNUMS", PACKNUMS);
                    command.Parameters.AddWithValue("@PACKINDATES", PACKINDATES);
                    command.Parameters.AddWithValue("@PRODATES", PRODATES);
                    command.Parameters.AddWithValue("@SDATES", SDATES);
                    command.Parameters.AddWithValue("@EDATES", EDATES);
                    command.Parameters.AddWithValue("@ISCLOSED", "N");              

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

    public void UPDATE_TKGIFTSETS_SELLEDNUMS()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;


        // 1. 📌 使用參數化查詢，避免 SQL Injection
        string sqlQuery = @"
                            UPDATE [TKMARKETING].[dbo].[TKGIFTSETS]
                            SET [SELLEDNUMS]=TEMP.TOTALSELLEDNUMS
                            FROM 
                            (
	                            SELECT
		                            T1.MB001,
		                            T2.TOTALSELLEDNUMS
	                            FROM
		                            [TKMARKETING].[dbo].[TKGIFTSETS] AS T1 -- 左側主表，每一行都會觸發 T2 執行
	                            CROSS APPLY
	                            (
		                            SELECT
			                            SUM(SalesData.NUMS) AS TOTALSELLEDNUMS
		                            FROM
		                            (
			                            -- 1. COPTG (銷貨)
			                            SELECT
				                            TH004 AS ItemCode, -- 統一命名為 ItemCode
				                            (TH008 + TH024) AS NUMS,
				                            TG003 AS DocDate
		                               FROM [192.168.1.105].[TK].dbo.COPTG
			                            LEFT JOIN [192.168.1.105].[TK].dbo.CMSME ON ME001=TG005
			                            ,[192.168.1.105].[TK].dbo.COPTH,[192.168.1.105].[TK].dbo.INVMB
			                            ,[192.168.1.105].[TK].dbo.INVLA
			                            WHERE TG001=TH001 AND TG002=TH002
			                            AND MB001=TH004
			                            AND TG023 IN ('Y')
			                            AND TH017<>'********************'
			                            AND LA006=TH001 AND LA007=TH002 AND LA008=TH003
			                            AND TG003>=CONVERT(NVARCHAR,T1.SDATES,112)AND TG003<=CONVERT(NVARCHAR,T1.EDATES,112)

			                            UNION ALL
			                            -- 2. COPTI (銷退)
			                            SELECT
				                            TJ004 AS ItemCode,
				                            (TJ007) * -1 AS NUMS,
				                            TI003 AS DocDate
			                            FROM [192.168.1.105].[TK].dbo.COPTI
			                            LEFT JOIN [192.168.1.105].[TK].dbo.CMSME ON ME001=TI005
			                            ,[192.168.1.105].[TK].dbo.COPTJ,[192.168.1.105].[TK].dbo.INVMB
			                            ,[192.168.1.105].[TK].dbo.INVLA
			                            WHERE TI001=TJ001 AND TI002=TJ002
			                            AND MB001=TJ004
			                            AND TI019 IN ('Y')
			                            AND TJ014<>'********************'
			                            AND LA006=TJ001 AND LA007=TJ002 AND LA008=TJ003
			                            AND TI003>=CONVERT(NVARCHAR,T1.SDATES,112) AND TI003<=CONVERT(NVARCHAR,T1.EDATES,112)
       

			                            UNION ALL
			                            -- 3. POSTB (POS 銷貨)
			                            SELECT
				                            TB010 AS ItemCode,
				                            (TB019) AS NUMS,
				                            TB001 AS DocDate
		                               FROM [192.168.1.105].[TK].dbo.POSTB
			                            LEFT JOIN [192.168.1.105].[TK].dbo.CMSME ON ME001=TB002
			                            ,[192.168.1.105].[TK].dbo.INVMB
			                            , [192.168.1.105].[TK].dbo.INVLA 
			                            WHERE MB001=TB010	
			                            AND LA001=TB010 AND LA006=TB002 AND LA007=TB001
			                            AND TB001>= CONVERT(NVARCHAR,T1.SDATES,112) AND TB001<=CONVERT(NVARCHAR,T1.EDATES,112)
        
		                            ) AS SalesData
		                            WHERE
			                            SalesData.ItemCode = T1.MB001 -- 關鍵：只計算與 T1 當前行匹配的貨品
		                            GROUP BY
			                            SalesData.ItemCode -- 這裡的 GROUP BY 僅用於聚合 SUM(NUMS)
	                            ) AS T2
	                            WHERE ISNULL(T1.MB001,'')<>''
	                            AND ISNULL(T1.SDATES,'')<>''
	                            AND ISNULL(T1.EDATES,'')<>''
                            ) AS TEMP
                            WHERE TEMP.MB001=[TKGIFTSETS].MB001
                            ";

        // 2. 📌 包裹在 Try-Catch 區塊中，處理例外狀況
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 加入參數，將值安全地傳遞給 SQL 查詢 
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // 檢查是否有資料被更新
                    if (rowsAffected > 0)
                    {
                        //MsgBox("完成 \r\n ", this.Page, this);
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
    }
    protected void btnADD_Click(object sender, EventArgs e)
    {
        string YEARS = ADD_TextBox20.Text.Trim().ToString();
        string MB001 = ADD_TextBox19.Text.Trim().ToString();
        string MB002 = ADD_TextBox1.Text.Trim().ToString();
        string MB003 = ADD_TextBox2.Text.Trim().ToString();
        string PRICES = ADD_TextBox3.Text.Trim().ToString();
        string IPPRICES = ADD_TextBox4.Text.Trim().ToString();
        string DMPRICES = ADD_TextBox5.Text.Trim().ToString();
        string STORENUMS = ADD_TextBox6.Text.Trim().ToString();
        string ECOMMERCENUMS = ADD_TextBox7.Text.Trim().ToString();
        string TOURISHOPNUMS = ADD_TextBox8.Text.Trim().ToString();
        string INARMYNUMS = ADD_TextBox9.Text.Trim().ToString();
        string INOILNUMS = ADD_TextBox10.Text.Trim().ToString();
        string INSALENUMS = ADD_TextBox11.Text.Trim().ToString();
        string INPRNUMS = ADD_TextBox12.Text.Trim().ToString();
        string BOSSPRNUMS = ADD_TextBox13.Text.Trim().ToString();
        string STAFFNUMS = ADD_TextBox14.Text.Trim().ToString();
        string TOTALNUMS = ADD_TextBox15.Text.Trim().ToString();
        string PACKNUMS = ADD_TextBox16.Text.Trim().ToString();
        string PACKINDATES = ADD_TextBox17.Text.Trim().ToString();
        string PRODATES = ADD_TextBox18.Text.Trim().ToString();
        string SDATES = ADD_TextBox21.Text.Trim().ToString();
        //處理日期空值
        object sDatesParam = DBNull.Value;
        object eDatesParam = DBNull.Value;
        if (!string.IsNullOrWhiteSpace(SDATES))
        {
            sDatesParam = SDATES;
        }        
        string EDATES = ADD_TextBox22.Text.Trim().ToString();
        if (!string.IsNullOrWhiteSpace(EDATES))
        {
            eDatesParam = EDATES;
        }

        ADD_TKGIFTSETS
        (
            YEARS,
            MB001,
            MB002,
            MB003,
            PRICES,
            IPPRICES,
            DMPRICES,
            STORENUMS,
            ECOMMERCENUMS,
            TOURISHOPNUMS,
            INARMYNUMS,
            INOILNUMS,
            INSALENUMS,
            INPRNUMS,
            BOSSPRNUMS,
            STAFFNUMS,
            TOTALNUMS,
            PACKNUMS,
            PACKINDATES,
            PRODATES,
            sDatesParam,
            eDatesParam
         );

        UPDATE_TKGIFTSETS_SELLEDNUMS();

        BindGrid();

    }

    #endregion
}