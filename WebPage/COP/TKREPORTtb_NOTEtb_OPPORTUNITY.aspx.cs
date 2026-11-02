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

public partial class CDS_WebPage_TKREPORTtb_NOTEtb_OPPORTUNITY : Ede.Uof.Utility.Page.BasePage
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
            BindGrid(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));

            txtDate1.Text = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
            txtDate2.Text = DateTime.Now.ToString("yyyy/MM/dd");

        }
        else
        {
            BindGrid(txtDate1.Text, txtDate2.Text);
        }



    }

    #region FUNCTION
    
    private void BindGrid(string SDATE,string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [USER_NAME],[COMPANY_NAME],[OPPORTUNITY_NAME],[PRODUCT],[NOTE_CONTENT],[tb_NOTE].[CREATE_DATETIME],Replace(Convert(Varchar(12),CONVERT(money,ISNULL([tb_OPPORTUNITY].[AMOUNT],0)),1),'.00','') AS AMOUNT,(CASE WHEN [USER_NAME]='公司' THEN '蔡顏鴻' ELSE [USER_NAME] END ) AS TEMP  
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
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [USER_NAME],[OPPORTUNITY_NAME],[PRODUCT],[NOTE_CONTENT],[tb_NOTE].[CREATE_DATETIME]
                           FROM [HJ_BM_DB].[dbo].[tb_NOTE],[HJ_BM_DB].[dbo].[tb_OPPORTUNITY] 
                           LEFT JOIN [HJ_BM_DB].[dbo].[tb_USER] ON [USER_ID]=[OWNER_ID]
                           WHERE [tb_NOTE].[OPPORTUNITY_ID]=[tb_OPPORTUNITY].[OPPORTUNITY_ID]
                           AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)>=@SDATE AND CONVERT(nvarchar,[tb_NOTE].[CREATE_DATETIME],111)<=@EDATE
                           ORDER BY [OPPORTUNITY_NAME],[tb_NOTE].[CREATE_DATETIME]

                        ";

        m_db.AddParameter("@SDATE", txtDate1.Text);
        m_db.AddParameter("@EDATE", txtDate2.Text);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count>0)
        {
            dt.Columns[0].Caption = "專案名稱";
            dt.Columns[1].Caption = "商品";
            dt.Columns[2].Caption = "記錄";
            dt.Columns[3].Caption = "記錄時間";      

            e.Datasource = dt;
        }
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