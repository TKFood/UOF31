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

public partial class CDS_WebPage_COWORK_TB_DEV_PRODUCT : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    string ROLES = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind_DropDownList_ISCLOSED();
            Bind_DropDownList_ISCLOSED2();
            BindGrid();
            BindGrid2();
        }
    }

    #region FUNCTION
    public void Bind_DropDownList_ISCLOSED()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT
                        [ID]
                        ,[KIND]
                        ,[PARAID]
                        ,[PARANAME]
                        FROM [TKRESEARCH].[dbo].[TBPARA]
                        WHERE [KIND]='TB_DEV_PRODUCT_ISCLOSEDYN'
                        ORDER BY [PARAID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList_ISCLOSED.DataSource = dt;
            DropDownList_ISCLOSED.DataTextField = "PARANAME";
            DropDownList_ISCLOSED.DataValueField = "PARANAME";
            DropDownList_ISCLOSED.DataBind();

        }
        else
        {

        }

    }
    public void Bind_DropDownList_ISCLOSED2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT
                        [ID]
                        ,[KIND]
                        ,[PARAID]
                        ,[PARANAME]
                        FROM [TKRESEARCH].[dbo].[TBPARA]
                        WHERE [KIND]='TB_DEV_PRODUCT_ISCLOSEDYN'
                        ORDER BY [PARAID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList_ISCLOSED2.DataSource = dt;
            DropDownList_ISCLOSED2.DataTextField = "PARANAME";
            DropDownList_ISCLOSED2.DataValueField = "PARANAME";
            DropDownList_ISCLOSED2.DataBind();

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
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        //DropDownList_ISCLOSED
        if (DropDownList_ISCLOSED.Text.Equals("N"))
        {
            QUERYS.AppendFormat(@" AND [ISCLOSED]='N' ");
        }
        else if (DropDownList_ISCLOSED.Text.Equals("Y"))
        {
            QUERYS.AppendFormat(@" AND [ISCLOSED]='Y' ");
        }      
        //TextBox1
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS2.AppendFormat(@" AND [NAMES] LIKE '%{0}%' ", TextBox1.Text);
        }
        else
        {
            QUERYS2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                            SELECT 
                            [ID]
                            ,[NAMES] AS '品名'
                            ,[PURPOSES] AS '開發目的'
                            ,[SPECIALS] AS '特色'
                            ,[REQUESTS] AS '訴求'
                            ,[RECORDS] AS '記錄'
                            ,[ISCLOSED] AS '是否結案'
                            ,CONVERT(NVARCHAR,[CREATEDATES],112) AS '建立日期'
                            FROM [TKRESEARCH].[dbo].[TB_DEV_PRODUCT]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY [ID]
                             ", QUERYS.ToString(), QUERYS2.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


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
            Button Grid1_Button1 = (Button)e.Row.FindControl("Grid1_Button1");
            if (Grid1_Button1!= null)
            {
                string cellValue_Grid1_Button1 = Grid1_Button1.CommandArgument;
                dynamic param_Grid1_Button1 = new { ID = cellValue_Grid1_Button1 }.ToExpando();
            }


        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Grid1_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "OK", this.Page, this);   

            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];

                Label Label_ID = (Label)row.FindControl("Label_ID");              
                TextBox txtNewField_Grid1_記錄 = (TextBox)row.FindControl("txtNewField_Grid1_記錄");

                string ID = Label_ID.Text;              
                string RECORDS = txtNewField_Grid1_記錄.Text;

                //更新 TB_DEV_PRODUCT
                UPDATE_TB_DEV_PRODUCT_RECORDS
                    (
                     ID,                     
                     RECORDS
                     );

            }



            BindGrid();
            BindGrid2();
        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL1();

    }
    public void SETEXCEL1()
    {

    }
    private void BindGrid2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        //DropDownList_ISCLOSED
        if (DropDownList_ISCLOSED2.Text.Equals("N"))
        {
            QUERYS.AppendFormat(@" AND [ISCLOSED]='N' ");
        }
        else if (DropDownList_ISCLOSED2.Text.Equals("Y"))
        {
            QUERYS.AppendFormat(@" AND [ISCLOSED]='Y' ");
        }
        //TextBox1
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS2.AppendFormat(@" AND [NAMES] LIKE '%{0}%' ", TextBox2.Text);
        }
        else
        {
            QUERYS2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                            SELECT 
                            [ID]
                            ,[NAMES] AS '品名'
                            ,[PURPOSES] AS '開發目的'
                            ,[SPECIALS] AS '特色'
                            ,[REQUESTS] AS '訴求'
                            ,[RECORDS] AS '記錄'
                            ,[ISCLOSED] AS '是否結案'
                            ,CONVERT(NVARCHAR,[CREATEDATES],112) AS '建立日期'
                            FROM [TKRESEARCH].[dbo].[TB_DEV_PRODUCT]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY [ID]
                             ", QUERYS.ToString(), QUERYS2.ToString());




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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button Grid2_Button1 = (Button)e.Row.FindControl("Grid2_Button1");
            if (Grid2_Button1 != null)
            {
                string cellValue_Grid2_Button1 = Grid2_Button1.CommandArgument;
                dynamic paramGrid2_Button1 = new { ID = cellValue_Grid2_Button1 }.ToExpando();
            }


        }
        //設選項
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlNewField_Grid2_是否結案");
            if (ddl != null)
            {
                // 取得資料來源，例如從資料表 "CaseStatus" 抓出 "Name"、"Code"
                string connStr = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendFormat(@" 
                                        SELECT
                                        [ID]
                                        ,[KIND]
                                        ,[PARAID]
                                        ,[PARANAME]
                                        FROM[TKRESEARCH].[dbo].[TBPARA]
                                        WHERE[KIND] = 'TB_DEV_PRODUCT_ISCLOSEDYN'
                                        ORDER BY[PARAID]
                                    ");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddl.DataSource = reader;
                    ddl.DataTextField = "PARANAME";   // 顯示文字
                    ddl.DataValueField = "PARANAME";  // 對應值
                    ddl.DataBind();

                    // 設定選取值
                    string currentValue = DataBinder.Eval(e.Row.DataItem, "是否結案").ToString();
                    if (ddl.Items.FindByValue(currentValue) != null)
                        ddl.SelectedValue = currentValue;

                    reader.Close();
                }
            }
        }
    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Grid2_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "OK", this.Page, this);   

            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid2.Rows[rowIndex];


                Label Grid2_Label_ID = (Label)row.FindControl("Grid2_Label_ID");

                TextBox txtNewField_Grid2_品名 = (TextBox)row.FindControl("txtNewField_Grid2_品名");
                TextBox txtNewField_Grid2_開發目的 = (TextBox)row.FindControl("txtNewField_Grid2_開發目的");
                TextBox txtNewField_Grid2_特色 = (TextBox)row.FindControl("txtNewField_Grid2_特色");
                TextBox txtNewField_Grid2_訴求 = (TextBox)row.FindControl("txtNewField_Grid2_訴求");
                TextBox txtNewField_Grid2_記錄 = (TextBox)row.FindControl("txtNewField_Grid2_記錄");

                DropDownList ddlNewField_Grid2_是否結案 = (DropDownList)row.FindControl("ddlNewField_Grid2_是否結案");             

                string ID = Grid2_Label_ID.Text;
                string NAMES = txtNewField_Grid2_品名.Text;
                string PURPOSES = txtNewField_Grid2_開發目的.Text;
                string SPECIALS = txtNewField_Grid2_特色.Text;
                string REQUESTS = txtNewField_Grid2_訴求.Text;
                string ISCLOSED = ddlNewField_Grid2_是否結案.SelectedItem.Text;
                string RECORDS= txtNewField_Grid2_記錄.Text;

                //更新 TB_DEV_PRODUCT
                UPDATE_TB_DEV_PRODUCT
                    (
                     ID,
                     NAMES, 
                     PURPOSES, 
                     SPECIALS, 
                     REQUESTS, 
                     ISCLOSED,
                     RECORDS
                     );

            }



            BindGrid();
            BindGrid2();
        }

    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        
    }
   
    public void UPDATE_TB_DEV_PRODUCT_RECORDS(
         string ID,
         string RECORDS
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                            UPDATE  [TKRESEARCH].[dbo].[TB_DEV_PRODUCT]
                            SET                           
                            [RECORDS]=@RECORDS
                            WHERE [ID]=@ID                                            
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                  
                    cmd.Parameters.AddWithValue("@RECORDS", RECORDS);

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(" 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    public void UPDATE_TB_DEV_PRODUCT(
        string ID,
        string NAMES,
        string PURPOSES,
        string SPECIALS,
        string REQUESTS,
        string ISCLOSED,
        string RECORDS
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                            UPDATE  [TKRESEARCH].[dbo].[TB_DEV_PRODUCT]
                            SET
                            [NAMES]=@NAMES,
                            [PURPOSES]=@PURPOSES,
                            [REQUESTS]=@REQUESTS,
                            [SPECIALS]=@SPECIALS,
                            [ISCLOSED]=@ISCLOSED,
                            [RECORDS]=@RECORDS
                            WHERE [ID]=@ID                                            
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@NAMES", NAMES);
                    cmd.Parameters.AddWithValue("@PURPOSES", PURPOSES);
                    cmd.Parameters.AddWithValue("@SPECIALS", SPECIALS);
                    cmd.Parameters.AddWithValue("@REQUESTS", REQUESTS);
                    cmd.Parameters.AddWithValue("@ISCLOSED", ISCLOSED);
                    cmd.Parameters.AddWithValue("@RECORDS", RECORDS);

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(NAMES + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    public void ADD_TB_DEV_PRODUCT(      
       string NAMES,
       string PURPOSES,
       string SPECIALS,
       string REQUESTS,
       string RECORDS
       )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                            INSERT INTO [TKRESEARCH].[dbo].[TB_DEV_PRODUCT]
                            (
                            [NAMES]
                            ,[PURPOSES]
                            ,[SPECIALS]
                            ,[REQUESTS]
                            ,[RECORDS]
                            )
                            VALUES
                            (
                            @NAMES
                            ,@PURPOSES
                            ,@SPECIALS
                            ,@REQUESTS
                            ,@RECORDS
                            )                                      
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {                  
                    cmd.Parameters.AddWithValue("@NAMES", NAMES);
                    cmd.Parameters.AddWithValue("@PURPOSES", PURPOSES);
                    cmd.Parameters.AddWithValue("@SPECIALS", SPECIALS);
                    cmd.Parameters.AddWithValue("@REQUESTS", REQUESTS);
                    cmd.Parameters.AddWithValue("@RECORDS", RECORDS);

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(NAMES + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
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
    protected void Button2_Click(object sender, EventArgs e)
    {
        BindGrid2();
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        string NAMES = NEW_品名.Text.Trim();
        string PURPOSES = NEW_開發目的.Text.Trim();
        string SPECIALS = NEW_特色.Text.Trim();
        string REQUESTS = NEW_訴求.Text.Trim();
        string RECORDS = NEW_記錄.Text.Trim();

        if (!string.IsNullOrEmpty(NAMES)&& !string.IsNullOrEmpty(PURPOSES) && !string.IsNullOrEmpty(SPECIALS) && !string.IsNullOrEmpty(REQUESTS))
        {
            ADD_TB_DEV_PRODUCT(
               NAMES,
               PURPOSES,
               SPECIALS,
               REQUESTS,
               RECORDS
              );
            BindGrid();
            BindGrid2();

            MsgBox(NAMES + " 完成", this.Page, this);
        }
        else
        {
            MsgBox("錯誤-所有欄位都必填", this.Page, this);
        }
       

     
    }

    #endregion
}