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
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Web.UI.HtmlControls;
using Ede.Uof.EIP.SystemInfo;
using Telerik.Web.UI;

public partial class CDS_WebPage_TKBUSINESS_TK_TB_COMPANY_PROJECTSE : Ede.Uof.Utility.Page.BasePage
{   
    string ACCOUNTS = null;
    string NAMES = null;
    string FIRSTTIMES = "N";

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNTS = Current.Account;
        NAMES = Current.User.Name;
        RadDatePicker1.SelectedDate = DateTime.Now;

        if (!IsPostBack)
        {
            FIRSTTIMES = "Y";
            BindDropDownListISCLOSE();

            BindDropDownListADDISCLOSED();
            BindDropDownListADDKINDS();
            BindDropDownListADDDEPNAMES();
            BindDropDownListADDPRODUCTAPPLYS();
            BindDropDownListADDPACKAPPLYS();
            BindDropDownListADDSTATUS();

            BindGrid(DropDownListISCLOSE.SelectedValue.ToString());
        }
        else
        {
            FIRSTTIMES = "N";
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
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_COMPANY_PROJECTS'
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

    public void BindDropDownListADDISCLOSED()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='ISCLOSED'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListADDISCLOSED.DataSource = dt;
            DropDownListADDISCLOSED.DataTextField = "NAMES";
            DropDownListADDISCLOSED.DataValueField = "NAMES";
            DropDownListADDISCLOSED.DataBind();

        }
        else
        {

        }
    }
    public void BindDropDownListADDKINDS()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='KINDS'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListADDKINDS.DataSource = dt;
            DropDownListADDKINDS.DataTextField = "NAMES";
            DropDownListADDKINDS.DataValueField = "NAMES";
            DropDownListADDKINDS.DataBind();

        }
        else
        {

        }

    }
    public void BindDropDownListADDDEPNAMES()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='DEPNAMES'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListADDDEPNAMES.DataSource = dt;
            DropDownListADDDEPNAMES.DataTextField = "NAMES";
            DropDownListADDDEPNAMES.DataValueField = "NAMES";
            DropDownListADDDEPNAMES.DataBind();

        }
        else
        {

        }
       

    }
    public void BindDropDownListADDPRODUCTAPPLYS()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='PRODUCTAPPLYS'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListADDPRODUCTAPPLYS.DataSource = dt;
            DropDownListADDPRODUCTAPPLYS.DataTextField = "NAMES";
            DropDownListADDPRODUCTAPPLYS.DataValueField = "NAMES";
            DropDownListADDPRODUCTAPPLYS.DataBind();

        }
        else
        {

        }
    }
    public void BindDropDownListADDPACKAPPLYS()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='PACKAPPLYS'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListADDPACKAPPLYS.DataSource = dt;
            DropDownListADDPACKAPPLYS.DataTextField = "NAMES";
            DropDownListADDPACKAPPLYS.DataValueField = "NAMES";
            DropDownListADDPACKAPPLYS.DataBind();

        }
        else
        {

        }
    }
    public void BindDropDownListADDSTATUS()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='STATUS'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListADDSTATUS.DataSource = dt;
            DropDownListADDSTATUS.DataTextField = "NAMES";
            DropDownListADDSTATUS.DataValueField = "NAMES";
            DropDownListADDSTATUS.DataBind();

        }
        else
        {

        }
    }

    private void BindGrid(string DropDownListISCLOSE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder Query1 = new StringBuilder();

        if (!string.IsNullOrEmpty(DropDownListISCLOSE))
        {
            if (DropDownListISCLOSE.Equals("全部"))
            {
                Query1.AppendFormat(@"");
            }
            else
            {
                Query1.AppendFormat(@"  AND ID IN ( SELECT [ID] FROM[TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS] WHERE [ISCLOSED] LIKE '%{0}%' ) ", DropDownListISCLOSE);
            }

        }
        else
        {
            Query1.AppendFormat(@"");
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
                            ,CONVERT(NVARCHAR,[COMMENTSDATES],111) COMMENTSDATES
                            ,[TRACEDATES]
                            ,[NEWCOMMENTS]
                            ,CONVERT(NVARCHAR,[CREATEDATES],111) AS 'CREATEDATES'

                            FROM [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS]
                            WHERE 1=1
                            {0}

                            ORDER BY [NO]

                              
                            ", Query1.ToString());


        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();

        //if(FIRSTTIMES.Equals("N"))
        //{
        //    MsgBox("完成 \r\n", this.Page, this);
        //}
       
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
            DropDownList DropDownListKINDS = (DropDownList)e.Row.FindControl("DropDownListKINDS");

            if (DropDownListKINDS != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NAMES", typeof(String));
               
                string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
                Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

                string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='KINDS' 
                        ORDER BY [VALUE]
                        ";

                dt.Load(m_db.ExecuteReader(cmdTxt));
              
                // 在這裡設置DropDownListKINDS的資料來源和其他屬性
                if (dt.Rows.Count > 0)
                {
                    DropDownListKINDS.DataSource = dt;
                    DropDownListKINDS.DataTextField = "NAMES";
                    DropDownListKINDS.DataValueField = "NAMES";
                    DropDownListKINDS.DataBind();

                    // 獲取該列對應的資料行中的值
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    string defaultValue = rowView["KINDS"].ToString(); // 請替換YourDataField為您的資料行名稱

                    // 設定DropDownList的預設值
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        DropDownListKINDS.SelectedValue = defaultValue;
                    }
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList DropDownListDEPNAMES = (DropDownList)e.Row.FindControl("DropDownListDEPNAMES");

            if (DropDownListDEPNAMES != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NAMES", typeof(String));

                string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
                Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

                string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='DEPNAMES' 
                        ORDER BY [VALUE]
                        ";

                dt.Load(m_db.ExecuteReader(cmdTxt));

                // 在這裡設置DropDownListKINDS的資料來源和其他屬性
                if (dt.Rows.Count > 0)
                {
                    DropDownListDEPNAMES.DataSource = dt;
                    DropDownListDEPNAMES.DataTextField = "NAMES";
                    DropDownListDEPNAMES.DataValueField = "NAMES";
                    DropDownListDEPNAMES.DataBind();

                    // 獲取該列對應的資料行中的值
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    string defaultValue = rowView["DEPNAMES"].ToString(); // 請替換YourDataField為您的資料行名稱

                    // 設定DropDownList的預設值
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        DropDownListDEPNAMES.SelectedValue = defaultValue;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList DropDownListPRODUCTAPPLYS = (DropDownList)e.Row.FindControl("DropDownListPRODUCTAPPLYS");

                if (DropDownListPRODUCTAPPLYS != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("NAMES", typeof(String));

                    string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
                    Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

                    string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='PRODUCTAPPLYS' 
                        ORDER BY [VALUE]
                        ";

                    dt.Load(m_db.ExecuteReader(cmdTxt));

                    // 在這裡設置DropDownListKINDS的資料來源和其他屬性
                    if (dt.Rows.Count > 0)
                    {
                        DropDownListPRODUCTAPPLYS.DataSource = dt;
                        DropDownListPRODUCTAPPLYS.DataTextField = "NAMES";
                        DropDownListPRODUCTAPPLYS.DataValueField = "NAMES";
                        DropDownListPRODUCTAPPLYS.DataBind();

                        // 獲取該列對應的資料行中的值
                        DataRowView rowView = (DataRowView)e.Row.DataItem;
                        string defaultValue = rowView["PRODUCTAPPLYS"].ToString(); // 請替換YourDataField為您的資料行名稱

                        // 設定DropDownList的預設值
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            DropDownListPRODUCTAPPLYS.SelectedValue = defaultValue;
                        }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList DropDownListPACKAPPLYS = (DropDownList)e.Row.FindControl("DropDownListPACKAPPLYS");

                if (DropDownListPACKAPPLYS != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("NAMES", typeof(String));

                    string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
                    Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

                    string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='PACKAPPLYS' 
                        ORDER BY [VALUE]
                        ";

                    dt.Load(m_db.ExecuteReader(cmdTxt));

                    // 在這裡設置DropDownListKINDS的資料來源和其他屬性
                    if (dt.Rows.Count > 0)
                    {
                        DropDownListPACKAPPLYS.DataSource = dt;
                        DropDownListPACKAPPLYS.DataTextField = "NAMES";
                        DropDownListPACKAPPLYS.DataValueField = "NAMES";
                        DropDownListPACKAPPLYS.DataBind();

                        // 獲取該列對應的資料行中的值
                        DataRowView rowView = (DataRowView)e.Row.DataItem;
                        string defaultValue = rowView["PACKAPPLYS"].ToString(); // 請替換YourDataField為您的資料行名稱

                        // 設定DropDownList的預設值
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            DropDownListPACKAPPLYS.SelectedValue = defaultValue;
                        }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList DropDownListSTATUS = (DropDownList)e.Row.FindControl("DropDownListSTATUS");

                if (DropDownListSTATUS != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("NAMES", typeof(String));

                    string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
                    Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

                    string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='STATUS' 
                        ORDER BY [VALUE]
                        ";

                    dt.Load(m_db.ExecuteReader(cmdTxt));

                    // 在這裡設置DropDownListKINDS的資料來源和其他屬性
                    if (dt.Rows.Count > 0)
                    {
                        DropDownListSTATUS.DataSource = dt;
                        DropDownListSTATUS.DataTextField = "NAMES";
                        DropDownListSTATUS.DataValueField = "NAMES";
                        DropDownListSTATUS.DataBind();

                        // 獲取該列對應的資料行中的值
                        DataRowView rowView = (DataRowView)e.Row.DataItem;
                        string defaultValue = rowView["STATUS"].ToString(); // 請替換YourDataField為您的資料行名稱

                        // 設定DropDownList的預設值
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            DropDownListSTATUS.SelectedValue = defaultValue;
                        }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList DropDownListISCLOSED = (DropDownList)e.Row.FindControl("DropDownListISCLOSED");

                if (DropDownListISCLOSED != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("NAMES", typeof(String));

                    string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
                    Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

                    string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='ISCLOSED' 
                        ORDER BY [VALUE]
                        ";

                    dt.Load(m_db.ExecuteReader(cmdTxt));

                    // 在這裡設置DropDownListKINDS的資料來源和其他屬性
                    if (dt.Rows.Count > 0)
                    {
                        DropDownListISCLOSED.DataSource = dt;
                        DropDownListISCLOSED.DataTextField = "NAMES";
                        DropDownListISCLOSED.DataValueField = "NAMES";
                        DropDownListISCLOSED.DataBind();

                        // 獲取該列對應的資料行中的值
                        DataRowView rowView = (DataRowView)e.Row.DataItem;
                        string defaultValue = rowView["ISCLOSED"].ToString(); // 請替換YourDataField為您的資料行名稱

                        // 設定DropDownList的預設值
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            DropDownListISCLOSED.SelectedValue = defaultValue;
                        }
                    }
                }
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
                GridViewRow row = Grid1.Rows[rowIndex];
                // 獲取相應的ID
                Label LabelID = (Label)row.FindControl("ID");
                string ID = LabelID.Text;
                // 獲取TextBox的值                 
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField");
                string newTextValue = txtNewField.Text;
                //取得 NEWCOMMENTS 的值
                TextBox txtNEWCOMMENTS = (TextBox)row.FindControl("txtNEWCOMMENTS");
                string newNEWCOMMENTS = txtNEWCOMMENTS.Text;

                string MID = ID;
                string COMMETNS = newTextValue;

                if(!string.IsNullOrEmpty(newTextValue))
                {
                    ADD_TB_COMPANY_PROJECTS_DETAILS(MID, NAMES, COMMETNS, newNEWCOMMENTS);
                    UPDATE_TB_COMPANY_PROJECTS(MID, NAMES, COMMETNS, newNEWCOMMENTS);

                    MsgBox("成功 \r\n" + ID + " > " + newTextValue, this.Page, this);

                    BindGrid(DropDownListISCLOSE.SelectedValue.ToString());
                }
                


            }
        }
        else if (e.CommandName == "Grid1Button2")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                GridViewRow row = Grid1.Rows[rowIndex];
                // 獲取相應的ID
                Label LabelID = (Label)row.FindControl("ID");
                string ID = LabelID.Text;
                //DropDownList 是否結案
                DropDownList DropDownListISCLOSED = (DropDownList)row.FindControl("DropDownListISCLOSED");
                string stringDropDownListISCLOSED= DropDownListISCLOSED.Text.Trim();
                //TextBox 專案名稱         
                TextBox txtPROJECTNAMES = (TextBox)row.FindControl("專案名稱");
                string stringPROJECTNAMES = txtPROJECTNAMES.Text.Trim();
                //DropDownList 需求單位
                DropDownList DropDownListDEPNAMES = (DropDownList)row.FindControl("DropDownListDEPNAMES");
                string stringDEPNAMES = DropDownListDEPNAMES.Text.Trim();
                //DropDownList 專案屬性
                DropDownList DropDownListKINDS = (DropDownList)row.FindControl("DropDownListKINDS");
                string stringDropDownListKINDS = DropDownListKINDS.Text.Trim();
                //DropDownList 產品開發申請書
                DropDownList DropDownListPRODUCTAPPLYS = (DropDownList)row.FindControl("DropDownListPRODUCTAPPLYS");
                string stringDropDownListPRODUCTAPPLYS = DropDownListPRODUCTAPPLYS.Text.Trim();
                //DropDownList 包材暨包裝設計及變更申請書
                DropDownList DropDownListPACKAPPLYS = (DropDownList)row.FindControl("DropDownListPACKAPPLYS");
                string stringDropDownListPACKAPPLYS = DropDownListPACKAPPLYS.Text.Trim();
                //TextBox 需求單位預計上市時間         
                TextBox txtSALEDATES = (TextBox)row.FindControl("需求單位預計上市時間");
                string stringSALEDATES = txtSALEDATES.Text.Trim();
                //DropDownList 專案進度
                DropDownList DropDownListSTATUS = (DropDownList)row.FindControl("DropDownListSTATUS");
                string stringDropDownListSTATUS = DropDownListSTATUS.Text.Trim();
                //TextBox 追蹤日         
                //TextBox txtTRACEDATES = (TextBox)row.FindControl("追蹤日");
                //string stringTRACEDATES = txtTRACEDATES.Text.Trim();
                RadDatePicker RadDatePickerTRACEDATES = (RadDatePicker)row.FindControl("RadDatePicker1");
                string stringTRACEDATES = RadDatePickerTRACEDATES.SelectedDate.Value.ToString("yyyy/MM/dd");

                UPDATE_TB_COMPANY_PROJECTS_FIELDS(
                        ID
                        , stringDropDownListISCLOSED
                        , stringDropDownListKINDS
                        , stringPROJECTNAMES
                        , stringDEPNAMES
                        , stringDropDownListPRODUCTAPPLYS
                        , stringDropDownListPACKAPPLYS
                        , stringSALEDATES
                        , stringDropDownListSTATUS
                        , stringTRACEDATES);

                MsgBox("成功 \r\n" + ID + " > " + stringPROJECTNAMES, this.Page, this);

                BindGrid(DropDownListISCLOSE.SelectedValue.ToString());
            }
        }

        else if (e.CommandName == "Grid1Button3")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                GridViewRow row = Grid1.Rows[rowIndex];
                // 獲取相應的ID
                Label LabelID = (Label)row.FindControl("ID");
                string ID = LabelID.Text;
                
                //TextBox 專案名稱         
                TextBox txtPROJECTNAMES = (TextBox)row.FindControl("專案名稱");
                string stringPROJECTNAMES = txtPROJECTNAMES.Text.Trim();

                UPDATE_TB_COMPANY_PROJECTS_ISCLOSED(ID,"Y");
                MsgBox("成功 \r\n" + ID + " > " + stringPROJECTNAMES, this.Page, this);

                BindGrid(DropDownListISCLOSE.SelectedValue.ToString());
            }
        }
        else if (e.CommandName == "Grid1Button4")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                GridViewRow row = Grid1.Rows[rowIndex];
                // 獲取相應的ID
                Label LabelID = (Label)row.FindControl("ID");
                string ID = LabelID.Text;

                //TextBox 專案名稱         
                TextBox txtPROJECTNAMES = (TextBox)row.FindControl("專案名稱");
                string stringPROJECTNAMES = txtPROJECTNAMES.Text.Trim();

                UPDATE_TB_COMPANY_PROJECTS_ISCLOSED(ID, "N");
                MsgBox("成功 \r\n" + ID + " > " + stringPROJECTNAMES, this.Page, this);

                BindGrid(DropDownListISCLOSE.SelectedValue.ToString());
            }
        }
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();


    }


    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    public void SETEXCEL()
    {
        

    }

   

    public void ADD_TB_COMPANY_PROJECTS_DETAILS(string MID, string NAMES, string COMMENTS,string NEWCOMMENTS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                    INSERT INTO [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS_DETAILS]
                    (
                    [MID]
                    ,[CREATEDATES]
                    ,[NAMES]
                    ,[COMMENTS]
                    ,[NEWCOMMENTS]
                    )
                    VALUES
                    (
                    @MID
                    ,GETDATE()
                    ,@NAMES
                    ,@COMMENTS
                    ,@NEWCOMMENTS
                    )
                        ";


        m_db.AddParameter("@MID", MID);
        m_db.AddParameter("@NAMES", NAMES);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@NEWCOMMENTS", NEWCOMMENTS);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void UPDATE_TB_COMPANY_PROJECTS(string ID, string NAMES, string COMMENTS, string NEWCOMMENTS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";
        cmdTxt = @"
                    UPDATE  [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS]
                    SET [COMMENTS]=@COMMENTS
                    ,[COMMENTSDATES]=GETDATE()
                    ,[NEWCOMMENTS]=@NEWCOMMENTS
                    WHERE ID=@ID
                        ";
       


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@NEWCOMMENTS", NEWCOMMENTS);


        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void UPDATE_TB_COMPANY_PROJECTS_FIELDS(
        string ID
        , string ISCLOSED
        , string KINDS
        , string PROJECTNAMES
        , string DEPNAMES
        , string PRODUCTAPPLYS
        , string PACKAPPLYS
        , string SALEDATES
        , string STATUS
        , string TRACEDATES)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";
        cmdTxt = @"
                    UPDATE [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS]
                    SET ISCLOSED=@ISCLOSED
                    ,KINDS=@KINDS
                    ,PROJECTNAMES=@PROJECTNAMES
                    ,DEPNAMES=@DEPNAMES
                    ,PRODUCTAPPLYS=@PRODUCTAPPLYS
                    ,PACKAPPLYS=@PACKAPPLYS
                    ,SALEDATES=@SALEDATES
                    ,STATUS=@STATUS
                    ,TRACEDATES=@TRACEDATES
                    WHERE [ID]=@ID
                        ";



        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@ISCLOSED", ISCLOSED);
        m_db.AddParameter("@KINDS", KINDS);
        m_db.AddParameter("@PROJECTNAMES", PROJECTNAMES);
        m_db.AddParameter("@DEPNAMES", DEPNAMES);
        m_db.AddParameter("@PRODUCTAPPLYS", PRODUCTAPPLYS);
        m_db.AddParameter("@PACKAPPLYS", PACKAPPLYS);
        m_db.AddParameter("@SALEDATES", SALEDATES);
        m_db.AddParameter("@STATUS", STATUS);
        m_db.AddParameter("@TRACEDATES", TRACEDATES);

        m_db.ExecuteNonQuery(cmdTxt);
    }
    public void INSERT_TB_COMPANY_PROJECTS_FIELDS(
        string NO
        ,string ISCLOSED
        , string KINDS
        , string PROJECTNAMES
        , string DEPNAMES
        , string PRODUCTAPPLYS
        , string PACKAPPLYS
        , string SALEDATES
        , string STATUS
        , string TRACEDATES
        , string COMMENTS
        , string COMMENTSDATES
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";

        cmdTxt = @"                  
                    INSERT INTO [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS]
                    (
                    [NO]
                    ,[ISCLOSED]
                    ,[KINDS]
                    ,[PROJECTNAMES]
                    ,[DEPNAMES]
                    ,[PRODUCTAPPLYS]
                    ,[PACKAPPLYS]
                    ,[SALEDATES]
                    ,[STATUS]                   
                    ,[TRACEDATES]
                    ,[COMMENTS]
                    ,[COMMENTSDATES]
                    )
                    VALUES
                    (
                    @NO
                    ,@ISCLOSED
                    ,@KINDS
                    ,@PROJECTNAMES
                    ,@DEPNAMES
                    ,@PRODUCTAPPLYS
                    ,@PACKAPPLYS
                    ,@SALEDATES
                    ,@STATUS                  
                    ,@TRACEDATES
                    ,@COMMENTS
                    ,@COMMENTSDATES
                    )
                        ";
        m_db.AddParameter("@NO", NO);
        m_db.AddParameter("@ISCLOSED", ISCLOSED);
        m_db.AddParameter("@KINDS", KINDS);
        m_db.AddParameter("@PROJECTNAMES", PROJECTNAMES);
        m_db.AddParameter("@DEPNAMES", DEPNAMES);
        m_db.AddParameter("@PRODUCTAPPLYS", PRODUCTAPPLYS);
        m_db.AddParameter("@PACKAPPLYS", PACKAPPLYS);
        m_db.AddParameter("@SALEDATES", SALEDATES);
        m_db.AddParameter("@STATUS", STATUS);
        m_db.AddParameter("@TRACEDATES", TRACEDATES);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@COMMENTSDATES", COMMENTSDATES);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public string FIND_MAX_ID(string NO)
    {
        DataTable dt = new DataTable();      

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        StringBuilder SQL = new StringBuilder();

        SQL.AppendFormat(@"
                        SELECT MAX([ID]) ID  FROM [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS]
                        WHERE NO='{0}'
                        ", NO);


        dt.Load(m_db.ExecuteReader(SQL.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["ID"].ToString();

        }
        else
        {
            return null;
        }
    }

    public void UPDATE_TB_COMPANY_PROJECTS_ISCLOSED(string ID, string ISCLOSED)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";

        cmdTxt = @"                  
                UPDATE [TKBUSINESS].[dbo].[TB_COMPANY_PROJECTS]
                SET [ISCLOSED]=@ISCLOSED
                WHERE [ID]=@ID
                        ";
        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@ISCLOSED", ISCLOSED);
       

        m_db.ExecuteNonQuery(cmdTxt);
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
    protected void btn_Click(object sender, EventArgs e)
    {


        //開窗後回傳參數
        if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            //txtReturnValue.Text = Dialog.GetReturnValue();
        }

    }


    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid(DropDownListISCLOSE.SelectedValue.ToString());
    }
    protected void btn2_Click(object sender, EventArgs e)
    {
        string NO = TextBox1.Text.ToString();
        string ISCLOSED = DropDownListADDISCLOSED.SelectedValue.ToString();
        string KINDS = DropDownListADDKINDS.SelectedValue.ToString();
        string PROJECTNAMES = TextBox2.Text.ToString();
        string DEPNAMES = DropDownListADDDEPNAMES.SelectedValue.ToString();
        string PRODUCTAPPLYS = DropDownListADDPRODUCTAPPLYS.SelectedValue.ToString();
        string PACKAPPLYS = DropDownListADDPACKAPPLYS.SelectedValue.ToString();
        string SALEDATES = TextBox3.Text.ToString();
        string STATUS = DropDownListADDSTATUS.SelectedValue.ToString();
        string TRACEDATES = RadDatePicker1.SelectedDate.Value.ToString("yyyy/MM/dd");
        string COMMENTS = TextBox4.Text.ToString();
        string COMMENTSDATES = DateTime.Now.ToString("yyyy/MM/dd");

    INSERT_TB_COMPANY_PROJECTS_FIELDS(
            NO
        ,ISCLOSED
        , KINDS
        , PROJECTNAMES
        , DEPNAMES
        , PRODUCTAPPLYS
        , PACKAPPLYS
        , SALEDATES
        , STATUS
        , TRACEDATES
        , COMMENTS
        , COMMENTSDATES
        );

        string MAXID = FIND_MAX_ID(NO);
        if(!string.IsNullOrEmpty(MAXID))
        {
            ADD_TB_COMPANY_PROJECTS_DETAILS(MAXID, NAMES, COMMENTS, COMMENTS);
            UPDATE_TB_COMPANY_PROJECTS(MAXID, NAMES, COMMENTS, COMMENTS);
        }

        BindGrid(DropDownListISCLOSE.SelectedValue.ToString());

        MsgBox("完成 \r\n" + NO, this.Page, this);
    }



    #endregion
}