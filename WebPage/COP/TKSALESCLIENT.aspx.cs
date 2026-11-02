using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;

public partial class CDS_WebPage_TKSALESCLIENT : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //用ExpandoObject物件傳遞參數
        //ExpandoObject param = new { Name = "asd" }.ToExpando();
        //ExpandoObject param = new { Name = txtParam.Text }.ToExpando();

        //因為執行此行後，才會把JS的Event註冊到頁面上，所以過此行後下一次按btn元件的Event才會開窗並傳參數
        //故Dialog.Open2適合於參數為固定式的
        //Dialog.Open2(btn, "~/CDS/WebPage/Dialog.aspx", "", 800, 600, Dialog.PostBackType.Allows, param);


        if (!IsPostBack)
        {
            BindGrid(DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy/MM/dd"), DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).ToString("yyyy/MM/dd"));
            BindGrid2(DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy/MM/dd"), DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).ToString("yyyy/MM/dd"));
            BindGrid3(DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy/MM/dd"), DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).ToString("yyyy/MM/dd"));

            txtDate1.Text = DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy/MM/dd");
            txtDate2.Text = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).ToString("yyyy/MM/dd");

            
        }
        else
        {
            BindGrid(txtDate1.Text, txtDate2.Text);
            BindGrid2(txtDate1.Text, txtDate2.Text);
            BindGrid3(txtDate1.Text, txtDate2.Text);

        }


        //BindDropDownList();

        if (this.Session["SDAY"] != null&& this.Session["EDAY"] != null)
        {
            txtDate1.Text = this.Session["SDAY"].ToString();
            txtDate2.Text = this.Session["EDAY"].ToString();

        }

    }

    #region FUNCTION
    private void BindDropDownList()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("PARAID", typeof(String));
        //dt.Columns.Add("PARANAME", typeof(String));

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @" SELECT [ID],[KIND],[PARAID],[PARANAME] FROM [TKUOF].[dbo].[TBPARA] WHERE [KIND]='PROJECTSTATUS' ORDER BY [PARAID] ";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    DropDownList1.DataSource = dt;
        //    DropDownList1.DataTextField = "PARANAME";
        //    DropDownList1.DataValueField = "PARANAME";
        //    DropDownList1.DataBind();

        //}
        //else
        //{

        //}

       

    }
    private void BindGrid(string SDAY, string EDAY)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        this.Session["SDAY"] = SDAY;
        this.Session["EDAY"] = EDAY;

        string cmdTxt = @" 
                        SELECT USER_ACCOUNT,USER_NAME,GROUP_NAME,NOTENUMS,COPNOTENUMS,TOTALNUMS,CONVERT(decimal(16,2),CONVERT(DECIMAL,COPNOTENUMS)/CONVERT(DECIMAL,TOTALNUMS)*100) AS PCTS
                        FROM(
                        SELECT [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME
                        ,(SELECT COUNT([NOTE_CONTENT]) FROM [HJ_BM_DB].[dbo].[tb_NOTE] WHERE [tb_NOTE].CREATE_USER_ID=[tb_USER].USER_ID AND  CONVERT(NVARCHAR,[UPDATE_DATETIME],111)>=@SDAY AND  CONVERT(NVARCHAR,[UPDATE_DATETIME],111)<=@EDAY ) NOTENUMS
                        ,(SELECT COUNT( DISTINCT COMPANY_ID) FROM [HJ_BM_DB].[dbo].[tb_NOTE] WHERE [tb_NOTE].CREATE_USER_ID=[tb_USER].USER_ID AND  CONVERT(NVARCHAR,[UPDATE_DATETIME],111)>=@SDAY AND  CONVERT(NVARCHAR,[UPDATE_DATETIME],111)<=@EDAY ) COPNOTENUMS
                        ,(SELECT DISTINCT  COUNT(COMP.COMPANY_NAME) FROM [HJ_BM_DB].[dbo].[tb_COMPANY] COMP WHERE COMP.OWNER_ID=[tb_USER].[USER_ID]) TOTALNUMS
                        FROM [HJ_BM_DB].[dbo].[tb_USER]
                        ) AS TEMP
                        WHERE NOTENUMS>0
                        ORDER BY TEMP.GROUP_NAME,TEMP.USER_ACCOUNT
                        ";

        m_db.AddParameter("@SDAY", SDAY);
        m_db.AddParameter("@EDAY", EDAY);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    private void BindGrid2(string SDAY, string EDAY)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        this.Session["SDAY"] = SDAY;
        this.Session["EDAY"] = EDAY;

        string cmdTxt = @" 
                        SELECT [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME,[tb_COMPANY].ERPNO,COUNT([NOTE_CONTENT]) AS NOTENUMS
                        FROM [HJ_BM_DB].[dbo].[tb_USER],[HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY]
                        WHERE [tb_USER].USER_ID=[tb_NOTE].CREATE_USER_ID
                        AND [tb_NOTE].COMPANY_ID=[tb_COMPANY].COMPANY_ID
                        AND CONVERT(NVARCHAR,[tb_NOTE].[UPDATE_DATETIME],111)>=@SDAY AND CONVERT(NVARCHAR,[tb_NOTE].[UPDATE_DATETIME],111)<=@EDAY
                        GROUP BY  [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME,[tb_COMPANY].ERPNO
                        ORDER BY  [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME,[tb_COMPANY].ERPNO

                        ";

        m_db.AddParameter("@SDAY", SDAY);
        m_db.AddParameter("@EDAY", EDAY);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    private void BindGrid3(string SDAY, string EDAY)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        this.Session["SDAY"] = SDAY;
        this.Session["EDAY"] = EDAY;

        string cmdTxt = @" 
                        SELECT [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME
                        FROM [HJ_BM_DB].[dbo].[tb_USER],[HJ_BM_DB].[dbo].[tb_COMPANY]
                        WHERE [tb_USER].USER_ID=[tb_COMPANY].OWNER_ID
                        AND COMPANY_ID NOT IN 
                        (
                        SELECT COMPANY_ID
                        FROM [HJ_BM_DB].[dbo].[tb_NOTE]
                        WHERE CONVERT(NVARCHAR,[tb_NOTE].[UPDATE_DATETIME],111)>=@SDAY AND CONVERT(NVARCHAR,[tb_NOTE].[UPDATE_DATETIME],111)<=@EDAY
                        )
                        GROUP BY  [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME
                        ORDER BY  [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME
                        ";

        m_db.AddParameter("@SDAY", SDAY);
        m_db.AddParameter("@EDAY", EDAY);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid3.DataSource = dt;
        Grid3.DataBind();
    }

    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //    ExpandoObject param = new { ID = row["NO"].ToString() }.ToExpando();

        //    //Grid開窗是用RowDataBound事件再開窗
        //    Dialog.Open2(lbtnName, "~/CDS/WebPage/TKUOFTBPROJECTSDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //}

    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //    ExpandoObject param = new { ID = row["NO"].ToString() }.ToExpando();

        //    //Grid開窗是用RowDataBound事件再開窗
        //    Dialog.Open2(lbtnName, "~/CDS/WebPage/TKUOFTBPROJECTSDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //}

    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //    ExpandoObject param = new { ID = row["NO"].ToString() }.ToExpando();

        //    //Grid開窗是用RowDataBound事件再開窗
        //    Dialog.Open2(lbtnName, "~/CDS/WebPage/TKUOFTBPROJECTSDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //}

    }

    public void OnBeforeExport(object sender,Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        this.Session["SDAY"] = txtDate1.Text;
        this.Session["EDAY"] = txtDate2.Text;

        string cmdTxt = @" 
                        SELECT USER_ACCOUNT,USER_NAME,GROUP_NAME,NOTENUMS,COPNOTENUMS,TOTALNUMS,CONVERT(decimal(16,2),CONVERT(DECIMAL,COPNOTENUMS)/CONVERT(DECIMAL,TOTALNUMS)*100) AS PCTS
                        FROM(
                        SELECT [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME
                        ,(SELECT COUNT([NOTE_CONTENT]) FROM [HJ_BM_DB].[dbo].[tb_NOTE] WHERE [tb_NOTE].CREATE_USER_ID=[tb_USER].USER_ID AND  CONVERT(NVARCHAR,[UPDATE_DATETIME],111)>=@SDAY AND  CONVERT(NVARCHAR,[UPDATE_DATETIME],111)<=@EDAY ) NOTENUMS
                        ,(SELECT COUNT( DISTINCT COMPANY_ID) FROM [HJ_BM_DB].[dbo].[tb_NOTE] WHERE [tb_NOTE].CREATE_USER_ID=[tb_USER].USER_ID AND  CONVERT(NVARCHAR,[UPDATE_DATETIME],111)>=@SDAY AND  CONVERT(NVARCHAR,[UPDATE_DATETIME],111)<=@EDAY ) COPNOTENUMS
                        ,(SELECT DISTINCT  COUNT(COMP.COMPANY_NAME) FROM [HJ_BM_DB].[dbo].[tb_COMPANY] COMP WHERE COMP.OWNER_ID=[tb_USER].[USER_ID]) TOTALNUMS
                        FROM [HJ_BM_DB].[dbo].[tb_USER]
                        ) AS TEMP
                        WHERE NOTENUMS>0
                        ORDER BY TEMP.GROUP_NAME,TEMP.USER_ACCOUNT
                        ";

        m_db.AddParameter("@SDAY", txtDate1.Text);
        m_db.AddParameter("@EDAY", txtDate2.Text);


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            dt.Columns[0].Caption = "業務員工號";
            dt.Columns[1].Caption = "業務員";
            dt.Columns[2].Caption = "部門";
            dt.Columns[3].Caption = "客戶記錄筆數";
            dt.Columns[4].Caption = "客戶筆數";
            dt.Columns[5].Caption = "負責總客戶數";
            dt.Columns[5].Caption = "聯絡記錄率%";



            e.Datasource = dt;
        }
    }
    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        this.Session["SDAY"] = txtDate1.Text;
        this.Session["EDAY"] = txtDate2.Text;

        string cmdTxt = @" 
                        SELECT [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME,[tb_COMPANY].ERPNO,COUNT([NOTE_CONTENT]) AS NOTENUMS
                        FROM [HJ_BM_DB].[dbo].[tb_USER],[HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY]
                        WHERE [tb_USER].USER_ID=[tb_NOTE].CREATE_USER_ID
                        AND [tb_NOTE].COMPANY_ID=[tb_COMPANY].COMPANY_ID
                        AND CONVERT(NVARCHAR,[tb_NOTE].[UPDATE_DATETIME],111)>=@SDAY AND CONVERT(NVARCHAR,[tb_NOTE].[UPDATE_DATETIME],111)<=@EDAY
                        GROUP BY  [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME,[tb_COMPANY].ERPNO
                        ORDER BY  [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME,[tb_COMPANY].ERPNO

                        ";

        m_db.AddParameter("@SDAY", txtDate1.Text);
        m_db.AddParameter("@EDAY", txtDate2.Text);


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            dt.Columns[0].Caption = "業務員";
            dt.Columns[1].Caption = "部門";
            dt.Columns[2].Caption = "客戶";
            dt.Columns[3].Caption = "客戶代號";
            dt.Columns[4].Caption = "客戶記錄筆數";

            e.Datasource = dt;
        }
    }

    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        this.Session["SDAY"] = txtDate1.Text;
        this.Session["EDAY"] = txtDate2.Text;

        string cmdTxt = @" 
                        SELECT [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME
                        FROM [HJ_BM_DB].[dbo].[tb_USER],[HJ_BM_DB].[dbo].[tb_COMPANY]
                        WHERE [tb_USER].USER_ID=[tb_COMPANY].OWNER_ID
                        AND COMPANY_ID NOT IN 
                        (
                        SELECT COMPANY_ID
                        FROM [HJ_BM_DB].[dbo].[tb_NOTE]
                        WHERE CONVERT(NVARCHAR,[tb_NOTE].[UPDATE_DATETIME],111)>=@SDAY AND CONVERT(NVARCHAR,[tb_NOTE].[UPDATE_DATETIME],111)<=@EDAY
                        )
                        GROUP BY  [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME
                        ORDER BY  [tb_USER].USER_ACCOUNT,[tb_USER].USER_NAME,[tb_USER].GROUP_NAME,[tb_COMPANY].COMPANY_NAME
                        ";


        m_db.AddParameter("@SDAY", txtDate1.Text);
        m_db.AddParameter("@EDAY", txtDate2.Text);


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            dt.Columns[0].Caption = "業務員";
            dt.Columns[1].Caption = "部門";
            dt.Columns[2].Caption = "客戶";

            e.Datasource = dt;
        }
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid1.PageIndex = e.NewPageIndex;
        BindGrid(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }

    protected void grid2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid2.PageIndex = e.NewPageIndex;
        BindGrid2(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }

    protected void grid3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid3.PageIndex = e.NewPageIndex;
        BindGrid3(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
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

    protected void btn3_Click(object sender, EventArgs e)
    {

    }

    protected void btn4_Click(object sender, EventArgs e)
    {

    }

    protected void btn5_Click(object sender, EventArgs e)
    {
        
    }

    protected void btn6_Click(object sender, EventArgs e)
    {
        //this.Session["STATUS"] = DropDownList1.Text.Trim();
    }

    #endregion
}