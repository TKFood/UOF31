using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;

public partial class CDS_WebPage_TKREPORTtb_NOTE : Ede.Uof.Utility.Page.BasePage
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
            BindGrid(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid2(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid3(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid4(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid5(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid6(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid7(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid8(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid9(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid10(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));

            txtDate1.SelectedDate = DateTime.Now.AddDays(-6);
            txtDate2.SelectedDate = DateTime.Now;
            txtDate3.SelectedDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
            txtDate4.SelectedDate = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);

            BindGrid11(txtDate3.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate4.SelectedDate.Value.ToString("yyyy/MM/dd"), TextBox2.Text.ToString());
        }
        else
        {
            BindGrid(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid2(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid3(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid4(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid5(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid6(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid7(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid8(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid9(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
            BindGrid10(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));

            if(!string.IsNullOrEmpty(TextBox2.Text))
            {
                BindGrid11(txtDate3.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate4.SelectedDate.Value.ToString("yyyy/MM/dd"), TextBox2.Text.ToString());
            }
            


        }

        if (this.Session["COMPANY_NAME"] != null)
        {
            TextBox2.Text = this.Session["COMPANY_NAME"].ToString();

        }



    }

    #region FUNCTION
    
    private void BindGrid(string SDATE,string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // AND (NOTE_CONTENT LIKE '%主管決議:是%' OR NOTE_CONTENT LIKE '%主管決議: 是%' OR NOTE_CONTENT LIKE '%主管決議:  是%')

        string cmdTxt = @" 
                        SELECT [tb_USER].[USER_NAME],[COMPANY_NAME] ,REPLACE([NOTE_CONTENT],char(10),'<br/>') AS [NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME],CASE WHEN ([tb_NOTE].[FILE_NAME] LIKE '%.J%' OR [tb_NOTE].[FILE_NAME] LIKE '%.j%' OR [tb_NOTE].[FILE_NAME] LIKE '%.P%' OR [tb_NOTE].[FILE_NAME] LIKE '%.p%' ) THEN [tb_NOTE].[FILE_NAME] ELSE NULL END AS [FILE_NAME]
                        FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
                        LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                        LEFT JOIN [HJ_BM_DB].[dbo].[COPSALES] ON [COPSALES].[USER_ID]=[tb_USER].[USER_ID]
                        WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
                        AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                        AND [tb_NOTE].[COMPANY_ID]<>'0'
                          
                        ORDER BY [ORDERS],[USER_NAME],[COMPANY_NAME], [tb_NOTE].[CREATE_DATETIME]

                        ";

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid1.PageIndex = e.NewPageIndex;
        BindGrid(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string PATH = "https://eip.tkfood.com.tw/BM/upload/note/";
        Image img = (Image)e.Row.FindControl("Image1");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            Image img1 = (Image)e.Row.FindControl("Image1");

            if(!string.IsNullOrEmpty(row["FILE_NAME"].ToString()))
            {
                img.ImageUrl = PATH + row["FILE_NAME"].ToString();

                //獲取當前行的圖片路徑
                string ImgUrl = img.ImageUrl;
                //給帶圖片的單元格添加點擊事件
                e.Row.Cells[3].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
                    + ".checked=true;CellClick('" + ImgUrl + "')");

                //img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
            }

          
        }
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    private void BindGrid2(string SDATE, string EDATE)
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME] ,REPLACE([NOTE_CONTENT],char(10),'<br/>') AS [NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME],CASE WHEN ([tb_NOTE].[FILE_NAME] LIKE '%Jpg%' OR [tb_NOTE].[FILE_NAME] LIKE '%JPG%' OR [tb_NOTE].[FILE_NAME] LIKE '%jpg%' OR [tb_NOTE].[FILE_NAME] LIKE '%png%' OR [tb_NOTE].[FILE_NAME] LIKE '%PNG%' OR [tb_NOTE].[FILE_NAME] LIKE '%Pmg%') THEN [tb_NOTE].[FILE_NAME] ELSE NULL END AS [FILE_NAME]
        //                   FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
        //                   LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
        //                   WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
        //                   AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
        //                   AND [tb_NOTE].[COMPANY_ID]<>'0'
        //                   AND NOTE_ID NOT IN 
        //                       (
        //                       SELECT NOTE_ID
        //                       FROM [HJ_BM_DB].[dbo].[tb_NOTE]
        //                       WHERE  (NOTE_CONTENT LIKE '%主管決議:是%' OR NOTE_CONTENT LIKE '%主管決議: 是%' OR NOTE_CONTENT LIKE '%主管決議:  是%')
        //                       AND [tb_NOTE].[COMPANY_ID]<>'0'
        //                       AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
        //                       )
        //                   ORDER BY [USER_NAME],[COMPANY_NAME], [tb_NOTE].[CREATE_DATETIME]

        //                ";

        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //Grid2.DataSource = dt;
        //Grid2.DataBind();
    }

    protected void grid2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid2.PageIndex = e.NewPageIndex;
        BindGrid(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string PATH = "https://eip.tkfood.com.tw/BM/upload/note/";
        Image img = (Image)e.Row.FindControl("Image2");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            Image img1 = (Image)e.Row.FindControl("Image2");

            if (!string.IsNullOrEmpty(row["FILE_NAME"].ToString()))
            {
                img.ImageUrl = PATH + row["FILE_NAME"].ToString();

                //獲取當前行的圖片路徑
                string ImgUrl = img.ImageUrl;
                //給帶圖片的單元格添加點擊事件
                e.Row.Cells[4].Attributes.Add("onclick", e.Row.Cells[4].ClientID.ToString()
                    + ".checked=true;CellClick('" + ImgUrl + "')");

                //img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
            }


        }
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    private void BindGrid3(string SDATE, string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //AND (NOTE_CONTENT LIKE '%主管決議:是%' OR NOTE_CONTENT LIKE '%主管決議: 是%' OR NOTE_CONTENT LIKE '%主管決議:  是%')

        string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME],[OPPORTUNITY_NAME],[PRODUCT],REPLACE([NOTE_CONTENT],char(10),'<br/>') AS [NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME],Replace(Convert(Varchar(12),CONVERT(money,ISNULL([tb_OPPORTUNITY].[AMOUNT],0)),1),'.00','') AS AMOUNT,(CASE WHEN [USER_NAME]='公司' THEN '蔡顏鴻' ELSE [USER_NAME] END ) AS TEMP  
                           FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_OPPORTUNITY] 
                           LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                           LEFT JOIN [HJ_BM_DB].[dbo].[tb_COMPANY] ON [tb_OPPORTUNITY].[COMPANY_ID]=[tb_COMPANY].[COMPANY_ID]
                           WHERE [tb_NOTE].[OPPORTUNITY_ID]=[tb_OPPORTUNITY].[OPPORTUNITY_ID]
                           AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                           
                           ORDER BY TEMP,[USER_NAME],[COMPANY_NAME],[tb_NOTE].[CREATE_DATETIME]

                        ";

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid3.DataSource = dt;
        Grid3.DataBind();
    }

    protected void grid3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid1.PageIndex = e.NewPageIndex;
        BindGrid3(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    private void BindGrid4(string SDATE, string EDATE)
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME],[OPPORTUNITY_NAME],[PRODUCT],REPLACE([NOTE_CONTENT],char(10),'<br/>') AS [NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME],Replace(Convert(Varchar(12),CONVERT(money,ISNULL([tb_OPPORTUNITY].[AMOUNT],0)),1),'.00','') AS AMOUNT,(CASE WHEN [USER_NAME]='公司' THEN '蔡顏鴻' ELSE [USER_NAME] END ) AS TEMP  
        //                   FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_OPPORTUNITY] 
        //                   LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
        //                   LEFT JOIN [HJ_BM_DB].[dbo].[tb_COMPANY] ON [tb_OPPORTUNITY].[COMPANY_ID]=[tb_COMPANY].[COMPANY_ID]
        //                   WHERE [tb_NOTE].[OPPORTUNITY_ID]=[tb_OPPORTUNITY].[OPPORTUNITY_ID]
        //                   AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
        //                   AND NOTE_ID NOT IN 
        //                       (
        //                       SELECT NOTE_ID
        //                       FROM [HJ_BM_DB].[dbo].[tb_NOTE]
        //                       WHERE  (NOTE_CONTENT LIKE '%主管決議:是%' OR NOTE_CONTENT LIKE '%主管決議: 是%' OR NOTE_CONTENT LIKE '%主管決議:  是%')
        //                       AND OPPORTUNITY_ID<>'0'
        //                       AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
        //                       )
        //                   ORDER BY TEMP,[USER_NAME],[COMPANY_NAME],[tb_NOTE].[CREATE_DATETIME]

        //                ";

        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //Grid4.DataSource = dt;
        //Grid4.DataBind();
    }

    protected void grid4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid1.PageIndex = e.NewPageIndex;
        BindGrid3(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    private void BindGrid5(string SDATE, string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT COUNT(*) AS COUNTS
                            FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
                            LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                            WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
                            AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                            AND [tb_NOTE].[COMPANY_ID]<>'0'

                        ";

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid5.DataSource = dt;
        Grid5.DataBind();
    }

    protected void grid5_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid5.PageIndex = e.NewPageIndex;
        BindGrid3(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid5_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }



    private void BindGrid6(string SDATE, string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                            SELECT [USER_NAME],PRECOUNTS,COUNTS,CONVERT(decimal(16,2),(CASE WHEN PRECOUNTS>0 THEN (CONVERT(decimal(16,2),COUNTS)/CONVERT(decimal(16,2),PRECOUNTS)*100) ELSE COUNTS END)) AS 'PCTS'
                            FROM (
                            SELECT [USER_NAME]
                            ,(SELECT COUNT(*)  FROM [UOF].[dbo].[TB_EIP_SCH_WORK]  
                            WHERE [SUBJECT] LIKE '%拜訪%' 
                            AND CONVERT(nvarchar,[START_TIME],111)>=@SDATE AND CONVERT(nvarchar,[START_TIME],111)<=@EDATE
                            AND [EXECUTE_USER]=[USER_GUID]) AS PRECOUNTS
                            ,COUNT([COMPANY_NAME] ) AS COUNTS
                            FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
                            LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                            WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
                            AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                            AND [tb_NOTE].[COMPANY_ID]<>'0'
                            GROUP BY     [USER_NAME]   ,[USER_GUID]   
                            ) AS TEMP
                            WHERE [USER_NAME]  COLLATE Chinese_Taiwan_Stroke_CI_AS IN (SELECT [USER_NAME] FROM [HJ_BM_DB].[dbo].[COPSALES])
                            ORDER BY [USER_NAME] 
                                                   

                        ";

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid6.DataSource = dt;
        Grid6.DataBind();
    }

    protected void grid6_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid6.PageIndex = e.NewPageIndex;
        BindGrid6(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid6_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    private void BindGrid7(string SDATE, string EDATE)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                            SELECT [NAME],CONVERT(nvarchar,[START_TIME],111) AS 'DAYS',[SUBJECT]
                            FROM [UOF].[dbo].[TB_EIP_SCH_WORK], [UOF].[dbo].[TB_EB_USER]

                            WHERE [TB_EB_USER].[USER_GUID]=[TB_EIP_SCH_WORK].[EXECUTE_USER]
                            AND [SUBJECT] LIKE '%拜訪%' 
                            AND CONVERT(nvarchar,[START_TIME],111)>=@SDATE 
                            AND CONVERT(nvarchar,[START_TIME],111)<=@EDATE
                            ORDER BY [NAME],CONVERT(nvarchar,[START_TIME],111) ,[DESCRIPTION]
                        

                        ";

        DateTime SD = Convert.ToDateTime(SDATE);
        SD=SD.AddDays(7);
        DateTime ED = Convert.ToDateTime(EDATE);
        ED=ED.AddDays(7);

        string SSDATE = SD.ToString("yyyy/MM/dd");
        string EEDATE = ED.ToString("yyyy/MM/dd");

        m_db.AddParameter("@SDATE", SSDATE);
        m_db.AddParameter("@EDATE", EEDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid7.DataSource = dt;
        Grid7.DataBind();
    }

    protected void grid7_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid7.PageIndex = e.NewPageIndex;
        BindGrid7(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid7_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    private void BindGrid8(string SDATE, string EDATE)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"
                            SELECT ORDERS,USER_ID,USER_NAME,USER_ACCOUNT,SALES,COMS,NOTES,CONVERT(decimal(16,2),(CONVERT(decimal(16,2),COMS)/CONVERT(decimal(16,2),SALES))*100) AS 'PCTS'
                            FROM (
                            SELECT [ORDERS],[USER_ID],[USER_NAME],[USER_ACCOUNT]
                            ,(SELECT COUNT(DISTINCT  [COMPANY_ID]) FROM [HJ_BM_DB].[dbo].[tb_COMPANY] WHERE [STATUS]='1' AND [OWNER_ID]=[USER_ID]) AS 'SALES'
                            ,(SELECT COUNT(DISTINCT  [tb_NOTE].[COMPANY_ID]) FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] WHERE [STATUS]='1' AND [tb_NOTE].COMPANY_ID=[tb_COMPANY].COMPANY_ID AND [OWNER_ID]=[USER_ID]  AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE  AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE) AS 'COMS'
                            ,(SELECT COUNT([tb_NOTE].[COMPANY_ID]) FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] WHERE [STATUS]='1' AND [tb_NOTE].COMPANY_ID=[tb_COMPANY].COMPANY_ID AND [OWNER_ID]=[USER_ID]  AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE  AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE) AS 'NOTES'
                            FROM [HJ_BM_DB].[dbo].[COPSALES]
                            ) AS TEMP
                            ORDER BY [ORDERS]
                        

                        ";

        //DateTime SD = Convert.ToDateTime(SDATE);
        //SD = SD.AddDays(7);
        //DateTime ED = Convert.ToDateTime(EDATE);
        //ED = ED.AddDays(7);

        DateTime FirstDay = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
        DateTime LastDay = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);

        string SSDATE = FirstDay.ToString("yyyy/MM/dd");
        string EEDATE = LastDay.ToString("yyyy/MM/dd");

        m_db.AddParameter("@SDATE", SSDATE);
        m_db.AddParameter("@EDATE", EEDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid8.DataSource = dt;
        Grid8.DataBind();
    }

    protected void grid8_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid8.PageIndex = e.NewPageIndex;
        BindGrid8(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid8_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    private void BindGrid9(string SDATE, string EDATE)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"
                        SELECT [ORDERS],[USER_ID],[USER_NAME],[USER_ACCOUNT],[COMPANY_ID],[COMPANY_NAME],[ERPNO],NOTES
                        FROM (
                        SELECT  [ORDERS],[USER_ID],[USER_NAME],[USER_ACCOUNT],[COMPANY_ID],[COMPANY_NAME],[ERPNO]
                        ,(SELECT COUNT([tb_NOTE].[COMPANY_ID]) FROM [HJ_BM_DB].[dbo].[tb_NOTE] WHERE   [tb_NOTE].COMPANY_ID=[tb_COMPANY].COMPANY_ID  AND [OWNER_ID]=[USER_ID]  AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE) AS 'NOTES'
                        FROM [HJ_BM_DB].[dbo].[COPSALES]
                        LEFT JOIN [HJ_BM_DB].[dbo].[tb_COMPANY] ON [STATUS]='1' AND OWNER_ID=USER_ID 
                        ) AS TEMP
                        WHERE NOTES>0
                        ORDER BY [ORDERS],NOTES DESC,[COMPANY_NAME]   
                        

                        ";

        //DateTime SD = Convert.ToDateTime(SDATE);
        //SD = SD.AddDays(7);
        //DateTime ED = Convert.ToDateTime(EDATE);
        //ED = ED.AddDays(7);

        DateTime FirstDay = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
        DateTime LastDay = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);

        string SSDATE = FirstDay.ToString("yyyy/MM/dd");
        string EEDATE = LastDay.ToString("yyyy/MM/dd");

        m_db.AddParameter("@SDATE", SSDATE);
        m_db.AddParameter("@EDATE", EEDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid9.DataSource = dt;
        Grid9.DataBind();
    }

    protected void grid9_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid9.PageIndex = e.NewPageIndex;
        BindGrid9(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid9_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }


    private void BindGrid10(string SDATE, string EDATE)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"
                        SELECT [ORDERS],[USER_ID],[USER_NAME],[USER_ACCOUNT],[COMPANY_ID],[COMPANY_NAME],[ERPNO],NOTES
                        FROM (
                        SELECT  [ORDERS],[USER_ID],[USER_NAME],[USER_ACCOUNT],[COMPANY_ID],[COMPANY_NAME],[ERPNO]
                        ,(SELECT COUNT([tb_NOTE].[COMPANY_ID]) FROM [HJ_BM_DB].[dbo].[tb_NOTE] WHERE   [tb_NOTE].COMPANY_ID=[tb_COMPANY].COMPANY_ID  AND [OWNER_ID]=[USER_ID]  AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE) AS 'NOTES'
                        FROM [HJ_BM_DB].[dbo].[COPSALES]
                        LEFT JOIN [HJ_BM_DB].[dbo].[tb_COMPANY] ON [STATUS]='1' AND OWNER_ID=USER_ID 
                        ) AS TEMP
                        WHERE NOTES<=0
                        ORDER BY [ORDERS],NOTES DESC,[COMPANY_NAME]   
                        

                        ";

        //DateTime SD = Convert.ToDateTime(SDATE);
        //SD = SD.AddDays(7);
        //DateTime ED = Convert.ToDateTime(EDATE);
        //ED = ED.AddDays(7);

        DateTime FirstDay = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
        DateTime LastDay = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);

        string SSDATE = FirstDay.ToString("yyyy/MM/dd");
        string EEDATE = LastDay.ToString("yyyy/MM/dd");

        m_db.AddParameter("@SDATE", SSDATE);
        m_db.AddParameter("@EDATE", EEDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid10.DataSource = dt;
        Grid10.DataBind();
    }


    protected void grid10_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid10.PageIndex = e.NewPageIndex;
        BindGrid10(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid10_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }
    private void BindGrid11(string SDATE, string EDATE, string COMPANY_NAME)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // AND (NOTE_CONTENT LIKE '%主管決議:是%' OR NOTE_CONTENT LIKE '%主管決議: 是%' OR NOTE_CONTENT LIKE '%主管決議:  是%')
        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                        SELECT [tb_USER].[USER_NAME],[COMPANY_NAME] ,REPLACE([NOTE_CONTENT],char(10),'<br/>') AS [NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME],CASE WHEN ([tb_NOTE].[FILE_NAME] LIKE '%.J%' OR [tb_NOTE].[FILE_NAME] LIKE '%.j%' OR [tb_NOTE].[FILE_NAME] LIKE '%.P%' OR [tb_NOTE].[FILE_NAME] LIKE '%.p%' ) THEN [tb_NOTE].[FILE_NAME] ELSE NULL END AS [FILE_NAME]
                        FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
                        LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                        LEFT JOIN [HJ_BM_DB].[dbo].[COPSALES] ON [COPSALES].[USER_ID]=[tb_USER].[USER_ID]
                        WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
                        AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                        AND [tb_NOTE].[COMPANY_ID]<>'0'
                        AND [COMPANY_NAME] LIKE '%{0}%'
                        ORDER BY [ORDERS],[USER_NAME],[COMPANY_NAME], [tb_NOTE].[CREATE_DATETIME]

                        ", COMPANY_NAME);

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid11.DataSource = dt;
        Grid11.DataBind();
    }

    protected void grid11_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid11.PageIndex = e.NewPageIndex;
        BindGrid11(txtDate3.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate4.SelectedDate.Value.ToString("yyyy/MM/dd"), TextBox2.Text.Trim());
    }
    protected void Grid11_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    //OnBeforeExport

    public void OnBeforeExport(object sender,Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME] ,[NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME] 
                           FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
                           LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                           WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
                           AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                           ORDER BY [USER_NAME],[COMPANY_NAME],[tb_NOTE].[CREATE_DATETIME]

                        ";


        m_db.AddParameter("@SDATE", txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"));
        m_db.AddParameter("@EDATE", txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count>0)
        {
            dt.Columns[0].Caption = "客戶";
            dt.Columns[1].Caption = "記錄";
            dt.Columns[2].Caption = "記錄日期";
        

            e.Datasource = dt;
        }
    }

    /// <summary>
    /// 保留訊息換行內容
    /// </summary>
    /// <param name="htmlConent"></param>
    /// <returns></returns>
    internal string AppendContext(string htmlConent)
    {
        Regex regex_newline = new Regex("(\r\n|\r|\n)");
        var convertStr = regex_newline.Replace(htmlConent, "<br/>");
        var result = "";

        //處理字串需保留 < br > 符合設定格式
        string[] line = convertStr.Split(new string[] { "<br/>" }, StringSplitOptions.None);
        foreach (string ss in line)
        {
            result += AntiXssEncoder.HtmlEncode(ss, true) + "<br/>";
        }

        return result;
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
        //this.Session["SDATE"] = txtDate1.Text.Trim();
        //this.Session["EDATE"] = txtDate2.Text.Trim();
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            if (Dialog.GetReturnValue().Equals("NeedPostBack"))
            {
                this.Session["COMPANY_NAME"] = TextBox2.Text.Trim();
            }

        }

       
    }

    #endregion
}