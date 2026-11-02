using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Web.Services;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Ede.Uof.EIP.SystemInfo;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
public partial class CDS_WebPage_Mobile_Mobile_SALES_RECORDS_ADMIN : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            BindDropDownList1();
            BindDropDownList2();
           
            BindDropDownListISCLOSE();
            BindDropDownListISCLOSE2();

            BindGrid();
            BindGrid2();

        }
           
    }

    #region FUNCTION
    private void BindDropDownList1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[NAME]
                        ,[LEADER]
                        FROM [TKBUSINESS].[dbo].[TBSALESNAME]
                        WHERE [NAME] NOT IN ('全部')
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "NAME";
            DropDownList1.DataValueField = "NAME";
            DropDownList1.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='是否結案'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "NAMES";
            DropDownList2.DataValueField = "NAMES";
            DropDownList2.DataBind();

        }
        else
        {

        }
    }
  
    private void BindDropDownListISCLOSE()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='是否結案'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE.DataSource = dt;
            DropDownListISCLOSE.DataTextField = "NAMES";
            DropDownListISCLOSE.DataValueField = "NAMES";
            DropDownListISCLOSE.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownListISCLOSE2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='是否結案'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE2.DataSource = dt;
            DropDownListISCLOSE2.DataTextField = "NAMES";
            DropDownListISCLOSE2.DataValueField = "NAMES";
            DropDownListISCLOSE2.DataBind();

        }
        else
        {

        }
    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder Query1 = new StringBuilder();
        StringBuilder Query2 = new StringBuilder();

        if (!string.IsNullOrEmpty(TextBox_CLIENTS.Text))
        {
            Query1.AppendFormat(@" AND ID IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED] WHERE [CLIENTS] LIKE '%{0}%') ", TextBox_CLIENTS.Text);
        }
        else
        {
            Query1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(DropDownListISCLOSE.SelectedValue.ToString()))
        {
            if (DropDownListISCLOSE.SelectedValue.ToString().Equals("全部"))
            {
                Query2.AppendFormat(@"");
            }
            else
            {
                Query2.AppendFormat(@"AND ID IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED] WHERE [ISCLOSE] LIKE '%{0}%')", DropDownListISCLOSE.SelectedValue.ToString());
            }

        }
        else
        {
            Query2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"

                            SELECT 
                            [TB_SALES_ASSINGED].[ID]
                            ,[SALES]
                            ,[CLIENTS]
                            ,[EVENTS]
                            ,CONVERT(NVARCHAR,[EDAYS],111) EDAYS
                            ,[ISCLOSE]
                            ,CONVERT(NVARCHAR,[ADDDATES],111) ADDDATES
                            ,(SELECT TOP 1 [COMMENTS] FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS] WHERE [TB_SALES_ASSINGED_COMMENTS].MID=[TB_SALES_ASSINGED].ID ORDER BY ID DESC) AS COMMENTS
                            FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY [SALES],[EDAYS],[ID]

                              
                            ", Query1.ToString(), Query2.ToString()); ;


        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid("");
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 假設 txtNewField 是一個 Label 控制項
            TextBox txtNewField = (TextBox)e.Row.FindControl("txtNewField");
            Button Grid1Button1 = (Button)e.Row.FindControl("Grid1Button1");
            Label LabelSALES = (Label)e.Row.FindControl("SALES");
            Button Grid1Button2 = (Button)e.Row.FindControl("Grid1Button2");
            Button Grid1Button3 = (Button)e.Row.FindControl("Grid1Button3");
            // 假設事件在資料繫結時，ISCLOSE 欄位的名稱是 "ISCLOSE"
            string eventValue = DataBinder.Eval(e.Row.DataItem, "ISCLOSE") as string;

            // 如果事件欄位的值為空，就隱藏 txtNewField
            if (string.IsNullOrWhiteSpace(eventValue))
            {
                txtNewField.Visible = false;
                Grid1Button1.Visible = false;
                LabelSALES.Visible = false;
                Grid1Button2.Visible = false;
                Grid1Button3.Visible = false;
            }
        }

    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid1Button1")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField");
                string newTextValue = txtNewField.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID");
                string id = txtid.Text;

                ADD_TB_SALES_ASSINGED_COMMENTS(id, newTextValue);

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
            }
        }
        if (e.CommandName == "Grid1Button2")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField");
                string newTextValue = txtNewField.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID");
                string id = txtid.Text;

                UPDATE_TB_SALES_ASSINGED_YN(id,"Y");

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
            }
        }
        if (e.CommandName == "Grid1Button3")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField");
                string newTextValue = txtNewField.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID");
                string id = txtid.Text;

                UPDATE_TB_SALES_ASSINGED_YN(id, "N");

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
            }
        }

    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL(txtDate1.Text, txtDate2.Text);


    }

    private void BindGrid2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder Query1 = new StringBuilder();
        StringBuilder Query2 = new StringBuilder();

        if (!string.IsNullOrEmpty(TextBox_CLIENTS2.Text))
        {
            Query1.AppendFormat(@" AND ID IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED] WHERE [CLIENTS] LIKE '%{0}%') ", TextBox_CLIENTS2.Text);
        }
        else
        {
            Query1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(DropDownListISCLOSE2.SelectedValue.ToString()))
        {
            if (DropDownListISCLOSE2.SelectedValue.ToString().Equals("全部"))
            {
                Query2.AppendFormat(@"");
            }
            else
            {
                Query2.AppendFormat(@"AND ID IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED] WHERE [ISCLOSE] LIKE '%{0}%')", DropDownListISCLOSE2.SelectedValue.ToString());
            }

        }
        else
        {
            Query2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"

                            SELECT 
                            [TB_SALES_ASSINGED].[ID]
                            ,[SALES]
                            ,[CLIENTS]
                            ,[EVENTS]
                            ,CONVERT(NVARCHAR,[EDAYS],111) EDAYS
                            ,[ISCLOSE]
                            ,CONVERT(NVARCHAR,[ADDDATES],111) ADDDATES
                            ,(SELECT TOP 1 [COMMENTS] FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS] WHERE [TB_SALES_ASSINGED_COMMENTS].MID=[TB_SALES_ASSINGED].ID ORDER BY ID DESC) AS COMMENTS
                            FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY [SALES],[EDAYS],[ID]

                              
                            ", Query1.ToString(), Query2.ToString()); ;


        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid("");
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid2_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid2Button1")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引          

            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid2.Rows[rowIndex];
                TextBox TextBox_SALES = (TextBox)row.FindControl("業務員");
                string SALES = TextBox_SALES.Text;
                TextBox TextBox_CLIENTS = (TextBox)row.FindControl("客戶");
                string CLIENTS = TextBox_CLIENTS.Text;
                TextBox TextBox_EDAYS = (TextBox)row.FindControl("回覆期限");
                string EDAYS = TextBox_EDAYS.Text;
                TextBox TextBox_EVENTS = (TextBox)row.FindControl("交辨內容");
                string EVENTS = TextBox_EVENTS.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID");
                string id = txtid.Text;

                UPDAT_TB_SALES_ASSINGED(
                                       id
                                      , SALES
                                      , CLIENTS
                                      , EVENTS
                                      , EDAYS
                                      );

                ////MsgBox(id + " " + newTextValue, this.Page, this);
                //// 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                //// ...

                //// 重新繫結GridView，刷新顯示
                BindGrid2();
            }
        }
       

    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL(txtDate1.Text, txtDate2.Text);


    }

    public void ADD_TB_SALES_ASSINGED_COMMENTS(string MID, string COMMENTS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                   INSERT INTO [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS]
                    ([MID],[COMMENTS])
                    VALUES (@MID,@COMMENTS)
                        ";


        m_db.AddParameter("@MID", MID);
        m_db.AddParameter("@COMMENTS", COMMENTS);

        m_db.ExecuteNonQuery(cmdTxt);
    }
    public void UPDATE_TB_SALES_ASSINGED_YN(string ID,string ISCLOSE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                UPDATE [TKBUSINESS].[dbo].[TB_SALES_ASSINGED]
                SET [ISCLOSE]=@ISCLOSE
                WHERE [ID]=@ID
                        ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@ISCLOSE", ISCLOSE);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void ADD_TB_SALES_ASSINGED(
                                    string SALES
                                    , string CLIENTS
                                    , string EVENTS
                                    , string EDAYS
                                    , string ISCLOSE
                                    , string ADDDATES
                                    )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"               
                        INSERT INTO  [TKBUSINESS].[dbo].[TB_SALES_ASSINGED]
                        (
                        [SALES]
                        ,[CLIENTS]
                        ,[EVENTS]
                        ,[EDAYS]
                        ,[ISCLOSE]
                        ,[ADDDATES]
                        )
                        VALUES
                        (
                        @SALES
                        ,@CLIENTS
                        ,@EVENTS
                        ,@EDAYS
                        ,@ISCLOSE
                        ,@ADDDATES
                        )
                        ";


        m_db.AddParameter("@SALES", SALES);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@EVENTS", EVENTS);
        m_db.AddParameter("@EDAYS", EDAYS);
        m_db.AddParameter("@ISCLOSE", ISCLOSE);
        m_db.AddParameter("@ADDDATES", ADDDATES);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void UPDAT_TB_SALES_ASSINGED(
                                     string ID
                                    , string SALES
                                    , string CLIENTS
                                    , string EVENTS
                                    , string EDAYS
                                    )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"          
                UPDATE [TKBUSINESS].[dbo].[TB_SALES_ASSINGED]
                SET [SALES]=@SALES,[CLIENTS]=@CLIENTS,[EVENTS]=@EVENTS,[EDAYS]=@EDAYS
                WHERE [ID]=@ID
                      
                        ";

        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@SALES", SALES);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@EVENTS", EVENTS);
        m_db.AddParameter("@EDAYS", EDAYS);


        m_db.ExecuteNonQuery(cmdTxt);
    }
    public void MsgBox(String ex, Page pg, Object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);
    }

    #endregion


    #region BUTTON
    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void btn2_Click(object sender, EventArgs e)
    {
        string SALES = DropDownList1.SelectedValue.ToString();
        string CLIENTS = TextBox1.Text.ToString();
        string EVENTS = TextBox2.Text.ToString();
        string EDAYS = txtDate1.Text.ToString();
        string ISCLOSE = DropDownList2.SelectedValue.ToString();
        string ADDDATES = DateTime.Now.ToString("yyyy/MM/dd");

        ADD_TB_SALES_ASSINGED(
                                SALES
                                , CLIENTS
                                , EVENTS
                                , EDAYS
                                , ISCLOSE
                                , ADDDATES
                                );
        BindGrid();

        // 在伺服器端註冊 JavaScript
        string script = "alert('完成');";

        // 使用ScriptManager
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessage", script, true);
    }
    protected void btn3_Click(object sender, EventArgs e)
    {
        BindGrid2();
    }
    #endregion
}