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

public partial class CDS_WebPage_TKREPORTtb_SALES_ALL : Ede.Uof.Utility.Page.BasePage
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
            BindGrid(DateTime.Now.ToString("yyyy/MM/dd"),DateTime.Now.AddDays(7).ToString("yyyy/MM/dd"));
            BindGrid2(DateTime.Now.ToString("yyyy/MM/dd"),DateTime.Now.AddDays(7).ToString("yyyy/MM/dd"));
            BindGrid3( DateTime.Now.ToString("yyyy/MM/dd"),DateTime.Now.AddDays(7).ToString("yyyy/MM/dd"));

            txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtDate2.Text = DateTime.Now.AddDays(7).ToString("yyyy/MM/dd");

        }
        else
        {
            BindGrid(txtDate1.Text, txtDate2.Text);
            BindGrid2(txtDate1.Text, txtDate2.Text);
            BindGrid3(txtDate1.Text, txtDate2.Text);
        }



    }

    #region FUNCTION
    
    private void BindGrid(string SDATE,string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME] ,[NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME]
                           FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
                           LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                           WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
                           AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                           ORDER BY [USER_NAME],[COMPANY_NAME], [tb_NOTE].[CREATE_DATETIME]

                        ";

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    private void BindGrid2(string SDATE, string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"
                        SELECT [TB_EB_USER].[USER_GUID],[tb_USER].[USER_ID],[ACCOUNT],[NAME],CONVERT(NVARCHAR,[START_TIME],111) AS START_TIME,SUBJECT,DESCRIPTION
                        FROM [UOF].[dbo].[TB_EB_USER],[UOF].dbo.[TB_EIP_SCH_WORK],[HJ_BM_DB].[dbo].[tb_USER]
                        WHERE [TB_EB_USER].[USER_GUID]=[TB_EIP_SCH_WORK].EXECUTE_USER
                        AND [tb_USER].[USER_GUID]=[TB_EB_USER].[USER_GUID]
                        AND [SOURCE_TYPE]='Self'
                        AND [NAME] IN ('洪櫻芬','王琇平','葉枋俐','何姍怡','林琪琪','林杏育','張釋予','蔡顏鴻','陳帟靜','黃鈺涵')
                        AND CONVERT(NVARCHAR,[START_TIME],111)>=@SDATE AND CONVERT(NVARCHAR,[START_TIME],111)<=@EDATE
                        ORDER BY [NAME],CONVERT(NVARCHAR,[START_TIME],112)

                        ";

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    private void BindGrid3(string SDATE, string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME] ,[NOTE_CONTENT] ,CONVERT(NVARCHAR,[tb_NOTE].[CREATE_DATETIME],111) AS [CREATE_DATETIME],CASE WHEN ([tb_NOTE].[FILE_NAME] LIKE '%Jpg%' OR [tb_NOTE].[FILE_NAME] LIKE '%JPG%' OR [tb_NOTE].[FILE_NAME] LIKE '%jpg%' OR [tb_NOTE].[FILE_NAME] LIKE '%png%' OR [tb_NOTE].[FILE_NAME] LIKE '%PNG%' OR [tb_NOTE].[FILE_NAME] LIKE '%Pmg%') THEN [tb_NOTE].[FILE_NAME] ELSE NULL END AS [FILE_NAME]
                           FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
                           LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                           WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
                           AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                           ORDER BY [USER_NAME],[COMPANY_NAME], [tb_NOTE].[CREATE_DATETIME]

                        ";

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        //string cmdTxt = @"
        //                SELECT CONVERT(NVARCHAR,DATES,111) DATES,[TB_EB_USER].[USER_GUID],[TB_EB_USER].[ACCOUNT],[TB_EB_USER].[NAME],[tb_USER].USER_ID,START_TIME,[SUBJECT],[DESCRIPTION],TEMP4.[CREATE_DATETIME],[COMPANY_NAME] ,[NOTE_CONTENT]
        //                FROM (
        //                SELECT  DATEADD(d,rows-1,@SDATE) DATES from
        //                (
        //                SELECT 
        //                ID,row_number()over(ORDER BY id) rows  
        //                FROM  
        //                sysobjects
        //                )TEMP 
        //                WHERE  
        //                TEMP.rows <= DATEDIFF(d,@SDATE, @EDATE) + 1
        //                ) AS TEMP2
        //                LEFT JOIN [UOF].[dbo].[TB_EB_USER] ON  [TB_EB_USER].[NAME] IN ('洪櫻芬','王琇平','葉枋俐','何姍怡','林琪琪','林杏育','張釋予','蔡顏鴻','陳帟靜','黃鈺涵')
        //                LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [tb_USER].USER_GUID=[TB_EB_USER].USER_GUID
        //                LEFT JOIN
        //                (
        //                SELECT [EXECUTE_USER],CONVERT(NVARCHAR,[START_TIME],111) AS START_TIME,[SUBJECT],[DESCRIPTION]
        //                FROM [UOF].dbo.[TB_EIP_SCH_WORK]
        //                WHERE [SOURCE_TYPE]='Self'
        //                ) AS TEMP3 ON TEMP3.[EXECUTE_USER]=[TB_EB_USER].USER_GUID AND START_TIME=CONVERT(NVARCHAR,DATES,111)
        //                LEFT JOIN
        //                (
        //                SELECT [OWNER_ID],[COMPANY_NAME] ,[NOTE_CONTENT],CONVERT(NVARCHAR,[tb_NOTE].[CREATE_DATETIME],111) CREATE_DATETIME
        //                FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY]
        //                WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
        //                ) AS TEMP4 ON TEMP4.OWNER_ID=USER_ID AND TEMP4.CREATE_DATETIME=CONVERT(NVARCHAR,DATES,111)
        //                WHERE (ISNULL([SUBJECT],'')<>'' OR ISNULL([NOTE_CONTENT],'')<>'')
        //                ORDER BY [NAME],CONVERT(NVARCHAR,DATES,111)

        //                ";



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid3.DataSource = dt;
        Grid3.DataBind();
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
        Grid2.PageIndex = e.NewPageIndex;
        BindGrid2(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
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

    public void OnBeforeExport(object sender,Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME] ,[NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME] 
        //                   FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
        //                   LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
        //                   WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
        //                   AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
        //                   ORDER BY [USER_NAME],[COMPANY_NAME],[tb_NOTE].[CREATE_DATETIME]

        //                ";


        //m_db.AddParameter("@SDATE", txtDate1.Text);
        //m_db.AddParameter("@EDATE", txtDate2.Text);

        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count>0)
        //{
        //    dt.Columns[0].Caption = "客戶";
        //    dt.Columns[1].Caption = "記錄";
        //    dt.Columns[2].Caption = "記錄日期";
        

        //    e.Datasource = dt;
        //}
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

    #endregion
}