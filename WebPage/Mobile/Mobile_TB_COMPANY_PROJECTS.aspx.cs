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

public partial class CDS_WebPage_Mobile_TB_COMPANY_PROJECTS : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //不顯示子視窗的按鈕
        ((Master_MobileMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_MobileMasterPage)this.Master).Button2Text = string.Empty;
        ((Master_MobileMasterPage)this.Master).Button3Text = string.Empty;

        if (!IsPostBack)
        {         
            BindDropDownListISCLOSE();
           

            BindGrid();
        }

    }


    #region FUNCTION  
  
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
            Query1.AppendFormat(@" AND ID IN (SELECT  [ID] FROM [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS] WHERE [PROJECTNAMES] LIKE '%{0}%') ", TextBox_PROJECTNAMES.Text);
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
                Query2.AppendFormat(@" AND ID IN ( SELECT  [ID] FROM [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS] WHERE [ISCLOSED] LIKE '%{0}%' )", DropDownListISCLOSE.SelectedValue.ToString());
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
                            ,[ISCLOSED]
                            ,[KINDS]
                            ,[PROJECTNAMES]
                            ,[DEPNAMES]
                            ,[PRODUCTAPPLYS]
                            ,[PACKAPPLYS]
                            ,[SALEDATES]
                            ,[STATUS]
                            ,[COMMENTS]
                            ,[COMMENTSDATES]
                            ,CONVERT(NVARCHAR,[TRACEDATES],111) AS 'TRACEDATES'
                            ,[NEWCOMMENTS]
                            ,CONVERT(NVARCHAR,[CREATEDATES],111) AS 'CREATEDATES'

                            FROM [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS]
                            WHERE 1=1
                            {0}
                            {1}
                        
                            ORDER BY CONVERT(NVARCHAR,[CREATEDATES],111)

                              
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


    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;


    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL(txtDate1.Text, txtDate2.Text);


    }


    #endregion

    #region BUTTON
    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    #endregion
}