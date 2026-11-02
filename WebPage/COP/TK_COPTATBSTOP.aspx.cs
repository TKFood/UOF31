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

public partial class CDS_WebPage_COP_TK_COPTATBSTOP : Ede.Uof.Utility.Page.BasePage
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
    private void BindGrid()
    {
        // 1. 取得連線字串
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 2. 處理查詢條件 (全面改用參數化，消滅 AppendFormat 注入風險)
        StringBuilder queryConditions = new StringBuilder();

        string ta002 = QUERY_TextBox1.Text.Trim();
        if (!string.IsNullOrEmpty(ta002))
        {
            queryConditions.AppendLine(" AND A.TA002 LIKE @TA002 + '%' ");
            m_db.AddParameter("@TA002", ta002);
        }

        // 💡 預留：如果您未來要加入日期欄位查詢 (範例)
        // DateTime searchDate;
        // if (DateTime.TryParse(txtSearchDate.Text, out searchDate))
        // {
        //     queryConditions.AppendLine(" AND B.TB016 = @SearchDate ");
        //     // 轉成 yyyy/MM/dd 萬國通用字串格式傳給 SQL
        //     m_db.AddParameter("@SearchDate", searchDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture));
        // }

        // 3. 組裝 SQL (改用標準 INNER JOIN 提高大表關聯效能)
        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                            SELECT 
                                A.TA001 AS '報價單別',
                                A.TA002 AS '報價單號',
                                A.TA006 AS '客戶',
                                M.MV002 AS '業務員',
                                B.TB004 AS '品號',
                                B.TB005 AS '品名',
                                B.TB007 AS '報價數量',
                                B.TB008 AS '報價單位',
                                CONVERT(INT,B.TB009) AS '報價單價',
                                (CASE A.TA022 
                                    WHEN '1' THEN '內含'  
                                    WHEN '2' THEN '外加'  
                                    WHEN '3' THEN '零稅率' 
                                    WHEN '4' THEN '免稅' 
                                    WHEN '5' THEN '不計稅' 
                                    ELSE '' 
                                 END) AS '稅別',
                                CONVERT(VARCHAR(10), B.TB016, 111) AS '生效日期', -- 確保格式為 yyyy/MM/dd
                                CONVERT(VARCHAR(10), B.TB017, 111) AS '失效日期', -- 確保格式為 yyyy/MM/dd
                                B.TB006 AS '規格',
                                A.TA004 AS '客代',
                                A.TA005 AS '業務',
                                A.TA007 AS '幣別'
                            FROM [TK].dbo.COPTA A
                            INNER JOIN [TK].dbo.COPTB B ON A.TA001 = B.TB001 AND A.TA002 = B.TB002
                            LEFT JOIN [TK].dbo.CMSMV M ON M.MV001 = A.TA005
                            WHERE 1=1
                            {0}
                            ORDER BY A.TA001, A.TA002
                        ", queryConditions.ToString());

        // 4. 執行與綁定
        DataTable dt = new DataTable();
        using (var reader = m_db.ExecuteReader(cmdTxt.ToString()))
        {
            dt.Load(reader);
        }

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
        if (e.CommandName == "MYUPDATE")
        {
            // 1. 取得由前端傳過來的 3 個複合主鍵字串
            string argument = (e.CommandArgument != null) ? e.CommandArgument.ToString() : "";
            if (string.IsNullOrEmpty(argument)) return;

            // 2. 用豎線拆開
            string[] keys = argument.Split('|');
            if (keys.Length != 5) return;
            string ta001 = keys[0].Trim(); // 報價單別 (Key 1)
            string ta002 = keys[1].Trim(); // 報價單號 (Key 2)
            string tb004 = keys[2].Trim(); // 品號     (Key 3)
            string mb017 = keys[3].Trim(); // 生效日     (Key 4)
            string mb001 = keys[4].Trim(); // 客戶代號     (Key 5)

            //MsgBox(ta001+ ta002+ tb004+ mb017 + "完成 \r\n ", this.Page, this);

            //設定失效日
            if (string.IsNullOrEmpty(txtDate.Text))
            {
                // 使用者沒有選日期的處理邏輯 
                MsgBox("日期格式請選擇日期不正確 \r\n ", this.Page, this);
                return;
            }
            else if (!string.IsNullOrEmpty(txtDate.Text))
            {
                //設定失效日               
                DateTime selectedDate;
              
                if (DateTime.TryParse(txtDate.Text, out selectedDate))
                {
                    // 3. 成功取得值，轉為您要的 yyyy/MM/dd 格式
                    string finalDateFormat = selectedDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                   
                    // 3. 執行參數化更新
                    UPDATE_COPTB_COPMB(ta001, ta002, tb004, mb017, finalDateFormat, mb001);

                    // 4. 更新完畢重新綁定畫面
                    BindGrid();
                }
                else
                {
                    MsgBox("日期格式不正確 \r\n ", this.Page, this);
                }
            }

            
           

          
        }

        if (e.CommandName == "MYUPDATENULL")
        {
            // 1. 取得由前端傳過來的 3 個複合主鍵字串
            string argument = (e.CommandArgument != null) ? e.CommandArgument.ToString() : "";
            if (string.IsNullOrEmpty(argument)) return;

            // 2. 用豎線拆開
            string[] keys = argument.Split('|');
            if (keys.Length != 5) return;
            string ta001 = keys[0].Trim(); // 報價單別 (Key 1)
            string ta002 = keys[1].Trim(); // 報價單號 (Key 2)
            string tb004 = keys[2].Trim(); // 品號     (Key 3)
            string mb017 = keys[3].Trim(); // 生效日     (Key 4)
            string mb001 = keys[4].Trim(); // 客戶代號     (Key 5)

            //MsgBox(ta001+ ta002+ tb004+ mb017 + "完成 \r\n ", this.Page, this);

            UPDATE_COPTB_COPMB(ta001, ta002, tb004, mb017, null, mb001); ;
            // 4. 更新完畢重新綁定畫面
            BindGrid();

        }

    }
    // 雖然不應該被觸發，但定義它以避免 HttpCException
    protected void Grid1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // 這裡可以不寫程式碼
    }

    protected void Grid1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // 這裡可以不寫程式碼，因為你的邏輯寫在 RowCommand 了
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    public void UPDATE_COPTB_COPMB(string TA001, string TA002, string TB004, string TB016, string TB017,string MB001)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;

        // 1. 📌 真正的參數化查詢，移除欄位字串相加，改用標準 AND 對齊（完美發揮索引效能）
        string sqlQuery = @"
                          UPDATE [TK].dbo.COPTB
                          SET [TB017] = @TB017
                          WHERE [TB001] = @TA001
                          AND [TB002] = @TA002
                          AND [TB004] = @TB004
                          AND [TB016] = @TB016;

                          UPDATE [TK].dbo.COPMB
                          SET MB018= @TB017
                          WHERE [MB001] = @MB001
                          AND [MB002] = @TB004
                          AND [MB017] = @TB016
                          
                    ";

        // 2. 📌 包裹在 Try-Catch 區塊中
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 精準綁定 C# 5.0 參數，避免 Null 造成的異常
                    command.Parameters.AddWithValue("@TA001", string.IsNullOrEmpty(TA001) ? (object)DBNull.Value : TA001.Trim());
                    command.Parameters.AddWithValue("@TA002", string.IsNullOrEmpty(TA002) ? (object)DBNull.Value : TA002.Trim());
                    command.Parameters.AddWithValue("@TB004", string.IsNullOrEmpty(TB004) ? (object)DBNull.Value : TB004.Trim());
                    command.Parameters.AddWithValue("@TB016", string.IsNullOrEmpty(TB016) ? (object)DBNull.Value : TB016.Trim());
                    command.Parameters.AddWithValue("@TB017", string.IsNullOrEmpty(TB017) ? (object)DBNull.Value : TB017.Trim());
                    command.Parameters.AddWithValue("@MB001", string.IsNullOrEmpty(MB001) ? (object)DBNull.Value : MB001.Trim());
                    command.CommandTimeout = 60; // 設定超時防護

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // 檢查是否有資料被更新
                    if (rowsAffected > 0)
                    {
                        MsgBox("完成 \r\n ", this.Page, this);
                    }
                    else
                    {
                        // 雖然執行成功，但沒有任何資料列被影響 (可能找不到對應的主鍵資料)
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // 💡 實際開發強烈建議加上 Log 或至少在 Debug 時能看到錯誤
            // LogError(ex.Message); 
            Response.Write("<script>alert('資料庫更新失敗：" + ex.Message.Replace("'", "\\'") + "');</script>");
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
    #endregion
}