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

public partial class CDS_WebPage_TKREPORTMOCTA : Ede.Uof.Utility.Page.BasePage
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

        string cmdTxt = @" SELECT MD002,ISNULL(MA002,'') AS MA002,TA003,TA034,CASE WHEN TA007<>'kg' THEN CONVERT(INT,TA015) ELSE 0  END  AS TA015,CASE WHEN TA007='kg' THEN TA015 ELSE 0  END  AS TA015KG,TA007
                            FROM [TK].dbo.CMSMD,[TK].dbo.MOCTA
                            LEFT JOIN [TK].dbo.COPTC ON TC001=TA026 AND TC002=TA027
                            LEFT JOIN [TK].dbo.COPMA ON MA001=TC004
                            WHERE TA021=MD001
                            AND MD001 IN ('02','03','04','09')
                            AND TA003>=@SDATE AND TA003<=@EDATE
                            ORDER BY  MD002,TA003,MA002

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

        string cmdTxt = @" SELECT MD002,ISNULL(MA002,'') AS MA002,TA003,TA034,CASE WHEN TA007<>'kg' THEN CONVERT(INT,TA015) ELSE 0  END  AS TA015,CASE WHEN TA007='kg' THEN TA015 ELSE 0  END  AS TA015KG,TA007
                            FROM [TK].dbo.CMSMD,[TK].dbo.MOCTA
                            LEFT JOIN [TK].dbo.COPTC ON TC001=TA026 AND TC002=TA027
                            LEFT JOIN [TK].dbo.COPMA ON MA001=TC004
                            WHERE TA021=MD001
                            AND MD001 IN ('02','03','04','09')
                            AND TA003>=@SDATE AND TA003<=@EDATE
                            ORDER BY  MD002,TA003,MA002

                        ";


        m_db.AddParameter("@SDATE", txtDate1.Text);
        m_db.AddParameter("@EDATE", txtDate2.Text);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count>0)
        {
            dt.Columns[0].Caption = "實際-線別";
            dt.Columns[1].Caption = "客戶";
            dt.Columns[2].Caption = "預計生產日期";
            dt.Columns[3].Caption = "品名";
            dt.Columns[4].Caption = "包裝數";
            dt.Columns[5].Caption = "重量";
            dt.Columns[6].Caption = "單位";

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