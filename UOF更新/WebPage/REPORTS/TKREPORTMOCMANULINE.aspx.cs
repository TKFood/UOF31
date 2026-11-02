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

public partial class CDS_WebPage_TKREPORTMOCMANULINE : Ede.Uof.Utility.Page.BasePage
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
            BindGrid(DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).ToString("yyyyMMdd"));

            txtDate1.Text = DateTime.Now.ToString("yyyyMMdd");
            txtDate2.Text = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).ToString("yyyyMMdd");

        }
        else
        {
            BindGrid(txtDate1.Text, txtDate2.Text);
        }



    }

    #region FUNCTION
    
    private void BindGrid(string SDATE,string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [MANU],[CLINET],CONVERT(NVARCHAR,[MANUDATE],112) AS MANUDATE,[MOCMANULINE].[MB002],CONVERT(INT,(ISNULL([PACKAGE],0))) AS PACKAGE,ISNULL([NUM],0) AS NUM,MB004
                            FROM [TKMOC].[dbo].[MOCMANULINE]
                            LEFT JOIN [TK].dbo.INVMB ON INVMB.MB001=[MOCMANULINE].MB001
                            WHERE CONVERT(NVARCHAR,[MANUDATE],112)>=@SDATE AND  CONVERT(NVARCHAR,[MANUDATE],112)<=@EDATE
                            AND (NUM>0 OR [PACKAGE]>0)
                            ORDER BY [MANU],[MANUDATE],[CLINET],[MB002]

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
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [MANU],[CLINET],CONVERT(NVARCHAR,[MANUDATE],112) AS MANUDATE,[MOCMANULINE].[MB002],CONVERT(INT,(ISNULL([PACKAGE],0))) AS PACKAGE,ISNULL([NUM],0) AS NUM,MB004
                            FROM [TKMOC].[dbo].[MOCMANULINE]
                            LEFT JOIN [TK].dbo.INVMB ON INVMB.MB001=[MOCMANULINE].MB001
                            WHERE CONVERT(NVARCHAR,[MANUDATE],112)>=@SDATE AND  CONVERT(NVARCHAR,[MANUDATE],112)<=@EDATE
                            AND (NUM>0 OR [PACKAGE]>0)
                            ORDER BY [MANU],[MANUDATE],[CLINET],[MB002]

                        ";


        m_db.AddParameter("@SDATE", txtDate1.Text);
        m_db.AddParameter("@EDATE", txtDate2.Text);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count>0)
        {
            dt.Columns[0].Caption = "預排-線別";
            dt.Columns[1].Caption = "客戶";
            dt.Columns[2].Caption = "預計生產日期";
            dt.Columns[3].Caption = "品名";
            dt.Columns[4].Caption = "包裝數";
            dt.Columns[5].Caption = "重量";

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