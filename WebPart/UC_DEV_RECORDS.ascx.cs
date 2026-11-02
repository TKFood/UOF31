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
using Ede.Uof.EIP.SystemInfo;

public partial class CDS_WebPart_UC_DEV_RECORDS : System.Web.UI.UserControl
{
    string ACCOUNT = null;
    string NAME = null;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            BindDropDownList1();
            BindDropDownList2();
            BindDropDownList3();

            BindDropDownListISCLOSE();
            BindDropDownListISCLOSE2();
            BindDropDownListISCLOSE3();

            Bind_DropDownList_EXEUNITS();         

            BindGrid();
            BindGrid2();
            BindGrid3();

        }
    }


    #region FUNCTION
    private void BindDropDownList1()
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
                        WHERE [KIND]='TBDEV_RECORDS_EXEUNITS'
                        AND [PARANAME]>='10'
                        ORDER BY [PARANAME]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "PARAID";
            DropDownList1.DataValueField = "PARAID";
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

    private void BindDropDownList3()
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
            //DropDownList3.DataSource = dt;
            //DropDownList3.DataTextField = "NAMES";
            //DropDownList3.DataValueField = "NAMES";
            //DropDownList3.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownListISCLOSE()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                      SELECT
                        [ID]
                        ,[KIND]
                        ,[PARAID]
                        ,[PARANAME]
                        FROM [TKRESEARCH].[dbo].[TBPARA]
                        WHERE [KIND]='TBDEV_RECORDS'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE.DataSource = dt;
            DropDownListISCLOSE.DataTextField = "PARANAME";
            DropDownListISCLOSE.DataValueField = "PARANAME";
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
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                         SELECT
                        [ID]
                        ,[KIND]
                        ,[PARAID]
                        ,[PARANAME]
                        FROM [TKRESEARCH].[dbo].[TBPARA]
                        WHERE [KIND]='TBDEV_RECORDS'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE2.DataSource = dt;
            DropDownListISCLOSE2.DataTextField = "PARANAME";
            DropDownListISCLOSE2.DataValueField = "PARANAME";
            DropDownListISCLOSE2.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownListISCLOSE3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                          SELECT
                        [ID]
                        ,[KIND]
                        ,[PARAID]
                        ,[PARANAME]
                        FROM [TKRESEARCH].[dbo].[TBPARA]
                        WHERE [KIND]='TBDEV_RECORDS'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE3.DataSource = dt;
            DropDownListISCLOSE3.DataTextField = "PARANAME";
            DropDownListISCLOSE3.DataValueField = "PARANAME";
            DropDownListISCLOSE3.DataBind();

        }
        else
        {

        }
    }


    private void Bind_DropDownList_EXEUNITS()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                         SELECT
                        [ID]
                        ,[KIND]
                        ,[PARAID]
                        ,[PARANAME]
                        FROM [TKRESEARCH].[dbo].[TBPARA]
                        WHERE [KIND]='TBDEV_RECORDS_EXEUNITS'
                        ORDER BY [PARANAME]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList_EXEUNITS.DataSource = dt;
            DropDownList_EXEUNITS.DataTextField = "PARAID";
            DropDownList_EXEUNITS.DataValueField = "PARAID";
            DropDownList_EXEUNITS.DataBind();

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
        StringBuilder Query3 = new StringBuilder();

        if (!string.IsNullOrEmpty(TextBox_PROJECTNAMES.Text))
        {
            Query1.AppendFormat(@" AND ID IN (SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [PROJECTNAMES] LIKE '%{0}%') ", TextBox_PROJECTNAMES.Text);
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
                Query2.AppendFormat(@" AND ID IN ( SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [ISCLOSE] LIKE '%{0}%' )", DropDownListISCLOSE.SelectedValue.ToString());
            }

        }
        else
        {
            Query2.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(DropDownList_EXEUNITS.SelectedValue.ToString()))
        {
            if (DropDownList_EXEUNITS.SelectedValue.ToString().Equals("全部"))
            {
                Query3.AppendFormat(@"");
            }
            else
            {
                Query3.AppendFormat(@" AND ID IN ( SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [EXEUNITS] LIKE '%{0}%' )", DropDownList_EXEUNITS.SelectedValue.ToString());
            }

        }
        else
        {
            Query3.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                            SELECT
                            [ID]
                            ,[NO]
                            ,[PROJECTNAMES]
                            ,CONVERT(NVARCHAR,[PROJECTSDEADLINEDATES],111) AS 'PROJECTSDEADLINEDATES'
                            ,[COMMENTS]
                            ,CONVERT(NVARCHAR,[COMMENTSADDDATES],111) AS 'COMMENTSADDDATES' 
                            ,[EXEUNITS]
                            ,CONVERT(NVARCHAR,[EXEDEADLINEDATES],111) AS 'EXEDEADLINEDATES'  
                            ,[ISCLOSE]
                            FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                            WHERE 1=1
                            {0}
                            {1}
                            {2}
                            ORDER BY [NO],[PROJECTNAMES]

                              
                            ", Query1.ToString(), Query2.ToString(), Query3.ToString()); ;


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
            // 找到 DropDownList 控件
            DropDownList Grid1_DropDownList_EXEUNITS = (DropDownList)e.Row.FindControl("Grid1_DropDownList_EXEUNITS");

            // 獲取當前行的資料
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            DataRow row = rowView.Row;

            // 獲取相應的ID
            string EXEUNITS = row["EXEUNITS"].ToString();


            DataTable dt = SERACH_EXEUNITS(EXEUNITS);

            // 使用 DataTable 的 foreach 迴圈來遍歷資料並添加到 DropDownList 中
            if(dt!=null && dt.Rows.Count>1)
            {
                foreach (DataRow DTROW in dt.Rows)
                {
                    // 從每一行中取得相應的值，例如部門名稱和部門編號
                    string PARAID = DTROW["PARAID"].ToString();
                    string PARANAME = DTROW["PARANAME"].ToString();

                    // 添加選項到 DropDownList 中
                    Grid1_DropDownList_EXEUNITS.Items.Add(new ListItem(PARAID, PARANAME));
                }
            }
            else
            {
                Grid1_DropDownList_EXEUNITS.Items.Add(new ListItem(EXEUNITS, EXEUNITS));
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
                Label txtid = (Label)row.FindControl("立案單號");
                string NO = txtid.Text;

                ADD_TBDEV_RECORDS_DETAILS(NO, newTextValue, NAME);
                UPDATE_TBDEV_RECORDS(NO, newTextValue, NAME);

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
                BindGrid2();
                BindGrid3();
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
                Label txtid = (Label)row.FindControl("立案單號");
                string NO = txtid.Text;

                UPDATE_TBDEV_RECORDS_ISCLOSE_YN(NO, "Y");

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
                BindGrid2();
                BindGrid3();
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
                Label txtid = (Label)row.FindControl("立案單號");
                string NO = txtid.Text;

                UPDATE_TBDEV_RECORDS_ISCLOSE_YN(NO, "N");

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
                BindGrid2();
                BindGrid3();
            }
        }
        if (e.CommandName == "Grid1Button4")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("立案單號");
                string NO = txtid.Text;
                // 找到 DropDownList 控件
                DropDownList ddlGrid1_DropDownList_EXEUNITS = (DropDownList)row.FindControl("Grid1_DropDownList_EXEUNITS");
                string selectedText = "";
                // 確保找到了 DropDownList 控件
                if (ddlGrid1_DropDownList_EXEUNITS != null)
                {
                    // 獲取選取的文字
                    selectedText = ddlGrid1_DropDownList_EXEUNITS.SelectedItem.Text;
                }

                UPDATE_TBDEV_RECORDS_EXEUNITS(NO, selectedText);

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
                BindGrid2();
                BindGrid3();
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

        if (!string.IsNullOrEmpty(TextBox_PROJECTNAMES2.Text))
        {
            Query1.AppendFormat(@" AND ID IN (SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [PROJECTNAMES] LIKE '%{0}%') ", TextBox_PROJECTNAMES.Text);
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
                Query2.AppendFormat(@" AND ID IN ( SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [ISCLOSE] LIKE '%{0}%' )", DropDownListISCLOSE2.SelectedValue.ToString());
            }

        }
        else
        {
            Query2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                            SELECT
                            [ID]
                            ,[NO]
                            ,[PROJECTNAMES]
                            ,CONVERT(NVARCHAR,[PROJECTSDEADLINEDATES],111) AS 'PROJECTSDEADLINEDATES'
                            ,[COMMENTS]
                            ,CONVERT(NVARCHAR,[COMMENTSADDDATES],111) AS 'COMMENTSADDDATES' 
                            ,[EXEUNITS]
                            ,CONVERT(NVARCHAR,[EXEDEADLINEDATES],111) AS 'EXEDEADLINEDATES'  
                            ,[ISCLOSE]
                            FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY [NO],[PROJECTNAMES]

                              
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

                TextBox TextBox_PROJECTNAMES = (TextBox)row.FindControl("專案名稱");
                string PROJECTNAMES = TextBox_PROJECTNAMES.Text;
                TextBox TextBox_PROJECTSDEADLINEDATES = (TextBox)row.FindControl("txtDate2");
                string PROJECTSDEADLINEDATES = TextBox_PROJECTSDEADLINEDATES.Text;
                TextBox TextBox_EXEDEADLINEDATES = (TextBox)row.FindControl("txtDate3");
                string EXEDEADLINEDATES = TextBox_EXEDEADLINEDATES.Text;


                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("立案單號");
                string NO = txtid.Text;

                UPDAT_TBDEV_RECORDS(
                                       NO
                                      , PROJECTNAMES
                                      , PROJECTSDEADLINEDATES
                                      , EXEDEADLINEDATES
                                      );

                ////MsgBox(id + " " + newTextValue, this.Page, this);
                //// 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                //// ...

                //// 重新繫結GridView，刷新顯示
                BindGrid();
                BindGrid2();
                BindGrid3();
            }
        }


    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL(txtDate1.Text, txtDate2.Text);


    }

    private void BindGrid3()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder Query1 = new StringBuilder();
        StringBuilder Query2 = new StringBuilder();

        if (!string.IsNullOrEmpty(TextBox_PROJECTNAMES_3.Text))
        {
            Query1.AppendFormat(@" AND  [TBDEV_RECORDS].ID IN (SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [PROJECTNAMES] LIKE '%{0}%') ", TextBox_PROJECTNAMES_3.Text);
        }
        else
        {
            Query1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(DropDownListISCLOSE3.SelectedValue.ToString()))
        {
            if (DropDownListISCLOSE3.SelectedValue.ToString().Equals("全部"))
            {
                Query2.AppendFormat(@"");
            }
            else
            {
                Query2.AppendFormat(@" AND  [TBDEV_RECORDS].ID IN ( SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [ISCLOSE] LIKE '%{0}%' )", DropDownListISCLOSE3.SelectedValue.ToString());
            }

        }
        else
        {
            Query2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                          SELECT
                            [TBDEV_RECORDS].[ID]
                            ,[TBDEV_RECORDS].[NO]
                            ,[TBDEV_RECORDS].[PROJECTNAMES]
                            ,CONVERT(NVARCHAR,[TBDEV_RECORDS].[PROJECTSDEADLINEDATES],111) AS 'PROJECTSDEADLINEDATES'
                            ,[TBDEV_RECORDS].[COMMENTS]
                            ,CONVERT(NVARCHAR,[TBDEV_RECORDS].[COMMENTSADDDATES],111) AS 'COMMENTSADDDATES' 
                            ,[TBDEV_RECORDS].[EXEUNITS]
                            ,CONVERT(NVARCHAR,[TBDEV_RECORDS].[EXEDEADLINEDATES],111) AS 'EXEDEADLINEDATES'  
                            ,[TBDEV_RECORDS].[ISCLOSE]
                            ,[TBDEV_RECORDS_DETAILS].[COMMENTS]
                            ,[TBDEV_RECORDS_DETAILS].[COMMENTSNAMES]
                            ,CONVERT(NVARCHAR,[TBDEV_RECORDS_DETAILS].[COMMENTSADDDATES],111) AS 'COMMENTSADDDATES'
                            FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                            LEFT JOIN [TKRESEARCH].[dbo].[TBDEV_RECORDS_DETAILS] ON [TBDEV_RECORDS].NO=[TBDEV_RECORDS_DETAILS].NO
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY [NO],[TBDEV_RECORDS].[PROJECTNAMES]

                              
                            ", Query1.ToString(), Query2.ToString()); ;


        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));
       

        Grid3.DataSource = dt;
        Grid3.DataBind();
    }

    protected void grid_PageIndexChanging3(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid("");
    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid3_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid3Button1")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引          

            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid3.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField3");
                string newTextValue = txtNewField.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("立案單號");
                string NO = txtid.Text;

                ADD_TBDEV_RECORDS_DETAILS(NO, newTextValue, NAME);
                UPDATE_TBDEV_RECORDS(NO, newTextValue, NAME);

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
                BindGrid2();
                BindGrid3();
            }
        }


    }

    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL(txtDate1.Text, txtDate2.Text);


    }

    public void ADD_TBDEV_RECORDS_DETAILS(string NO, string COMMENTS,string NAME)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                   INSERT INTO  [TKRESEARCH].[dbo].[TBDEV_RECORDS_DETAILS]
                    ([NO],[COMMENTS],[COMMENTSNAMES])
                    VALUES (@NO,@COMMENTS,@COMMENTSNAMES)
                        ";


        m_db.AddParameter("@NO", NO);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@COMMENTSNAMES", NAME);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void UPDATE_TBDEV_RECORDS(string NO, string COMMENTS, string NAME)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                   UPDATE  [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                    SET [COMMENTS]=@COMMENTS,[COMMENTSADDDATES]=GETDATE()
                    WHERE [NO]=@NO
                        ";


        m_db.AddParameter("@NO", NO);
        m_db.AddParameter("@COMMENTS", NAME+':'+Environment.NewLine+COMMENTS);
  

        m_db.ExecuteNonQuery(cmdTxt);
    }
    public void UPDATE_TBDEV_RECORDS_ISCLOSE_YN(string NO, string ISCLOSE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                UPDATE [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                SET [ISCLOSE]=@ISCLOSE
                WHERE [NO]=@NO
                        ";


        m_db.AddParameter("@NO", NO);
        m_db.AddParameter("@ISCLOSE", ISCLOSE);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void ADD_TBDEV_RECORDS(
                            string NO,
                            string PROJECTNAMES,
                            string PROJECTSDEADLINEDATES,
                            string COMMENTS,
                            string EXEUNITS,
                            string ISCLOSE,
                            string ADDDATES
                                    )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"               
                   INSERT INTO [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                    (
                    NO,
                    PROJECTNAMES,
                    PROJECTSDEADLINEDATES,
                    COMMENTS,
                    EXEUNITS,
                    ISCLOSE,
                    ADDDATES
                    )
                    VALUES
                    (
                    @NO,
                    @PROJECTNAMES,
                    @PROJECTSDEADLINEDATES,
                    @COMMENTS,
                    @EXEUNITS,
                    @ISCLOSE,
                    @ADDDATES
                    ) 
                        ";


    
        m_db.AddParameter("@NO", NO);
        m_db.AddParameter("@PROJECTNAMES", PROJECTNAMES);
        m_db.AddParameter("@PROJECTSDEADLINEDATES", PROJECTSDEADLINEDATES);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@EXEUNITS", EXEUNITS);
        m_db.AddParameter("@ISCLOSE", ISCLOSE);
        m_db.AddParameter("@ADDDATES", ADDDATES);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void UPDAT_TBDEV_RECORDS(
                                     string NO
                                    , string PROJECTNAMES
                                    , string PROJECTSDEADLINEDATES  
                                    , string EXEDEADLINEDATES
                                    )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"          
                    UPDATE [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                    SET [PROJECTNAMES]=@PROJECTNAMES,[PROJECTSDEADLINEDATES]=@PROJECTSDEADLINEDATES,[EXEDEADLINEDATES]=@EXEDEADLINEDATES
                    WHERE [NO]=@NO
                      
                        ";

        m_db.AddParameter("@NO", NO);
        m_db.AddParameter("@PROJECTNAMES", PROJECTNAMES);
        m_db.AddParameter("@PROJECTSDEADLINEDATES", PROJECTSDEADLINEDATES);
        m_db.AddParameter("@EXEDEADLINEDATES", EXEDEADLINEDATES);

        m_db.ExecuteNonQuery(cmdTxt);
    }
    public string GETMAXNO(DateTime DATES)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
           

            cmdTxt.AppendFormat(@"
                            SELECT ISNULL(MAX(NO),'0000000000') AS NO
                            FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                            WHERE [NO] LIKE '%{0}%'

                              
                            ", DATES.ToString("yyyyMM")); ;


            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if(dt!=null && dt.Rows.Count>=1)
            {
                string MAXNO = SETMAXNO(DateTime.Now,dt.Rows[0]["NO"].ToString());
                return MAXNO;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
        finally
        {
            
        }
    }

    public string SETMAXNO(DateTime dt, string NO)
    {
        if (NO.Equals("0000000000"))
        {
            return dt.ToString("yyyyMM") + "001";
        }

        else
        {
            int serno = Convert.ToInt16(NO.Substring(7, 3));
            serno = serno + 1;
            string temp = serno.ToString();
            temp = temp.PadLeft(3, '0');
            return dt.ToString("yyyyMM") + temp.ToString();
        }


    }

    public DataTable SERACH_EXEUNITS(string NOW_EXEUNITS)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();


            cmdTxt.AppendFormat(@"
                               SELECT 
                                [ID]
                                ,[KIND]
                                ,[PARAID]
                                ,[PARANAME]
                                FROM [TKRESEARCH].[dbo].[TBPARA]
                                WHERE [KIND]='TBDEV_RECORDS_EXEUNITS'
                                AND [PARAID] NOT IN ('全部')
                                AND [PARAID]>'{0}'
                              
                            ", NOW_EXEUNITS); ;


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
        catch
        {
            return null;
        }
        finally
        {

        }
    }
    public void UPDATE_TBDEV_RECORDS_EXEUNITS(string NO, string EXEUNITS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                UPDATE [TKRESEARCH].[dbo].[TBDEV_RECORDS]
                SET [EXEUNITS]=@EXEUNITS
                WHERE [NO]=@NO
                        ";


        m_db.AddParameter("@NO", NO);
        m_db.AddParameter("@EXEUNITS", EXEUNITS);

        m_db.ExecuteNonQuery(cmdTxt);
    }



    #endregion


    #region BUTTON
    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid();
        BindGrid2();
        BindGrid3();
    }
    protected void btn2_Click(object sender, EventArgs e)
    {
        string NO = TextBox1.Text.ToString().Trim();
        string PROJECTNAMES= TextBox2.Text.ToString().Trim();
        string PROJECTSDEADLINEDATES= txtDate1.Text.ToString();
        string COMMENTS= TextBox3.Text.ToString();
        string EXEUNITS= DropDownList1.SelectedValue.ToString();
        string ISCLOSE = DropDownList2.SelectedValue.ToString();
        string ADDDATES = DateTime.Now.ToString("yyyy/MM/dd");


        if(!string.IsNullOrEmpty(NO))
        {
            ADD_TBDEV_RECORDS(
                          NO
                           , PROJECTNAMES
                           , PROJECTSDEADLINEDATES
                           , COMMENTS
                           , EXEUNITS
                           , ISCLOSE
                           , ADDDATES
                               );
            BindGrid();
            BindGrid2();
            BindGrid3();

            // 在伺服器端註冊 JavaScript
            string script = "alert('完成');";

            // 使用ScriptManager
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessage", script, true);
        }
        else
        {
            // 在伺服器端註冊 JavaScript
            string script = "alert('沒有單號');";

            // 使用ScriptManager
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessage", script, true);
        }

       

      
    }

    protected void btn3_Click(object sender, EventArgs e)
    {
        BindGrid();
        BindGrid2();
        BindGrid3();
    }
    protected void btn4_Click(object sender, EventArgs e)
    {
        BindGrid();
        BindGrid2();
        BindGrid3();
    }

    protected void btn5_Click(object sender, EventArgs e)
    {
        TextBox1.Text = 'R'+GETMAXNO(DateTime.Now);
    }
    #endregion
}