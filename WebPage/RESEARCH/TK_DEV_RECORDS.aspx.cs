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

public partial class CDS_WebPage_RESEARCH_TK_DEV_RECORDS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {

            BindDropDownListISCLOSE();
            BindDropDownListISCLOSE2();

            Bind_DropDownList_EXEUNITS();

            BindGrid();
            BindGrid2();

        }
    }

    #region FUNCTION
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
                            ORDER BY [NO]

                              
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

    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
       
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

        if (!string.IsNullOrEmpty(TextBox_PROJECTNAMES_2.Text))
        {
            Query1.AppendFormat(@" AND  [TBDEV_RECORDS].ID IN (SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [PROJECTNAMES] LIKE '%{0}%') ", TextBox_PROJECTNAMES_2.Text);
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
                Query2.AppendFormat(@" AND  [TBDEV_RECORDS].ID IN ( SELECT [ID] FROM [TKRESEARCH].[dbo].[TBDEV_RECORDS] WHERE [ISCLOSE] LIKE '%{0}%' )", DropDownListISCLOSE2.SelectedValue.ToString());
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
                            ORDER BY [NO]

                              
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
    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL(txtDate1.Text, txtDate2.Text);


    }
    #endregion


    #region BUTTON
    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid();
      
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        BindGrid2();
    }

    #endregion
}