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

public partial class CDS_WebPage_Mobile_MobileTKREPORTtb_NOTE : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //不顯示子視窗的按鈕
        ((Master_MobileMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_MobileMasterPage)this.Master).Button2Text = string.Empty;
        ((Master_MobileMasterPage)this.Master).Button3Text = string.Empty;

        //用ExpandoObject物件傳遞參數
        //ExpandoObject param = new { Name = "asd" }.ToExpando();
        //ExpandoObject param = new { Name = txtParam.Text }.ToExpando();

        //因為執行此行後，才會把JS的Event註冊到頁面上，所以過此行後下一次按btn元件的Event才會開窗並傳參數
        //故Dialog.Open2適合於參數為固定式的
        //Dialog.Open2(btn, "~/CDS/WebPage/Dialog.aspx", "", 800, 600, Dialog.PostBackType.Allows, param);




        if (!IsPostBack)
        {
            BindGrid(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid2(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid3(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
            BindGrid4(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));

            txtDate1.Text = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
            txtDate2.Text = DateTime.Now.ToString("yyyy/MM/dd");

        }
        else
        {
            BindGrid(txtDate1.Text, txtDate2.Text);
            BindGrid2(txtDate1.Text, txtDate2.Text);
            BindGrid3(txtDate1.Text, txtDate2.Text);
            BindGrid4(txtDate1.Text, txtDate2.Text);
        }

    }

    #region FUNCTION

    private void BindGrid(string SDATE, string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME] ,REPLACE([NOTE_CONTENT],char(10),'<br/>') AS [NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME],CASE WHEN ([tb_NOTE].[FILE_NAME] LIKE '%.J%' OR [tb_NOTE].[FILE_NAME] LIKE '%.j%' OR [tb_NOTE].[FILE_NAME] LIKE '%.P%' OR [tb_NOTE].[FILE_NAME] LIKE '%.p%' ) THEN [tb_NOTE].[FILE_NAME] ELSE NULL END AS [FILE_NAME]
                           FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
                           LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                           WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
                           AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                           AND [tb_NOTE].[COMPANY_ID]<>'0'

                           ORDER BY [USER_NAME],[COMPANY_NAME], [tb_NOTE].[CREATE_DATETIME]

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

            if (!string.IsNullOrEmpty(row["FILE_NAME"].ToString()))
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
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

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

        //string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME] ,REPLACE([NOTE_CONTENT],char(10),'<br/>') AS [NOTE_CONTENT] ,[tb_NOTE].[CREATE_DATETIME],CASE WHEN ([tb_NOTE].[FILE_NAME] LIKE '%Jpg%' OR [tb_NOTE].[FILE_NAME] LIKE '%JPG%' OR [tb_NOTE].[FILE_NAME] LIKE '%jpg%' OR [tb_NOTE].[FILE_NAME] LIKE '%png%' OR [tb_NOTE].[FILE_NAME] LIKE '%PNG%' OR [tb_NOTE].[FILE_NAME] LIKE '%Pmg%') THEN [tb_NOTE].[FILE_NAME] ELSE NULL END AS [FILE_NAME]
        //                   FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_COMPANY] 
        //                   LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
        //                   WHERE [tb_COMPANY].COMPANY_ID=[tb_NOTE].COMPANY_ID 
        //                   AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
        //                   AND [tb_NOTE].[COMPANY_ID]<>'0'
                          
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
        BindGrid2(this.Session["SDATE"].ToString(), this.Session["EDATE"].ToString());
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

    private void BindGrid3(string SDATE, string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

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

    public void OnBeforeExport(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
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


        m_db.AddParameter("@SDATE", txtDate1.Text);
        m_db.AddParameter("@EDATE", txtDate2.Text);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
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

    #endregion
}