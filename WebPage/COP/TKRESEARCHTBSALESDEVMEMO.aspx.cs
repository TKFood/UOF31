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

public partial class CDS_WebPage_TKRESEARCHTBSALESDEVMEMO : Ede.Uof.Utility.Page.BasePage
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
            BindGrid("進行中");
        }
        else
        {
            BindGrid(DropDownList1.Text);
        }
        

        BindDropDownList();

        if (this.Session["STATUS"] != null)
        {
            DropDownList1.Text = this.Session["STATUS"].ToString();

        }

    }

    #region FUNCTION
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='MEMOSTATUS' ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "PARANAME";
            DropDownList1.DataValueField = "PARANAME";
            DropDownList1.DataBind();

        }
        else
        {

        }

       

    }
    private void BindGrid(string STATUS)
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        this.Session["STATUS"] = STATUS;

        string cmdTxt = @" SELECT [ID],[SERNO],[STATUS],[CLIENT],[PROD],[PRICES],[PROMOTIONS],[SPEC],[VALID],[PLACES],[ONSALES],CONVERT(NVARCHAR,[ONSALESDATES],111) ONSALESDATES,[PRODESGIN],CONVERT(NVARCHAR,[ASSESSMENTDATES],111) ASSESSMENTDATES ,CONVERT(NVARCHAR,[COSTSDATES],111) COSTSDATES,[SALESPRICES],[TEST],CONVERT(NVARCHAR,[TESTDATES],111) TESTDATES,[OWNER],[MEMO],[DEVMEMO],CONVERT(NVARCHAR,[MEMODATES],111) [MEMODATES],CONVERT(NVARCHAR,[DEVMEMODATES],111) [DEVMEMODATES],[SALESCHECKOK] FROM [TKRESEARCH].[dbo].[TBSALESDEVMEMO] WHERE STATUS=@STATUS ORDER BY [OWNER],[CLIENT],[PROD]                            ";

        m_db.AddParameter("@STATUS", STATUS);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();

       
    }

    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

            ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TKRESEARCHTBSALESDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            LinkButton lbtnName = (LinkButton)e.Row.FindControl("MEMO");

            ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TKRESEARCHTBSALESDEVMEMODialogMEMOADD.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            LinkButton lbtnName = (LinkButton)e.Row.FindControl("DEVMEMO");

            ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TKRESEARCHTBSALESDEVMEMODialogMEMODEVADD.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[13].Text.Equals("1911/01/01"))
            {
                e.Row.Cells[13].ForeColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[15].Text.Equals("1911/01/01"))
            {
                e.Row.Cells[15].ForeColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[16].Text.Equals("1911/01/01"))
            {
                e.Row.Cells[16].ForeColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[19].Text.Equals("1911/01/01"))
            {
                e.Row.Cells[19].ForeColor = System.Drawing.Color.Red;
            }
        }
          
    }

    public void OnBeforeExport(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string STATUS = DropDownList1.Text;

        string cmdTxt = @" SELECT [SERNO],[CLIENT],[OWNER],[PROD],[MEMO],[DEVMEMO],[STATUS],[PRICES],[PROMOTIONS],[SPEC],[VALID],[PLACES],[ONSALES],CONVERT(NVARCHAR,[ONSALESDATES],111) ONSALESDATES,[PRODESGIN],CONVERT(NVARCHAR,[ASSESSMENTDATES],111) ASSESSMENTDATES ,CONVERT(NVARCHAR,[COSTSDATES],111) COSTSDATES,[SALESPRICES],[TEST],CONVERT(NVARCHAR,[TESTDATES],111) TESTDATES,CONVERT(NVARCHAR,[MEMODATES],111) [MEMODATES],CONVERT(NVARCHAR,[DEVMEMODATES],111) [DEVMEMODATES] FROM [TKRESEARCH].[dbo].[TBSALESDEVMEMO] WHERE STATUS=@STATUS ORDER BY [OWNER],[CLIENT],[PROD]                            ";

        m_db.AddParameter("@STATUS", STATUS);


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            dt.Columns[0].Caption = "編號";
            dt.Columns[1].Caption = "客戶";
            dt.Columns[2].Caption = "負責業務";
            dt.Columns[3].Caption = "產品品項";
            dt.Columns[4].Caption = "業務進度";
            dt.Columns[5].Caption = "研發進度";
            dt.Columns[6].Caption = "狀態";
            dt.Columns[7].Caption = "末售";
            dt.Columns[8].Caption = "促銷設定";
            dt.Columns[9].Caption = "規格及屬性";
            dt.Columns[10].Caption = "產品效期";
            dt.Columns[11].Caption = "通路";
            dt.Columns[12].Caption = "預估上市說明";
            dt.Columns[13].Caption = "預估上市日期";
            dt.Columns[14].Caption = "產品圖/樣袋完稿備註";
            dt.Columns[15].Caption = "可行性評估申請日期";
            dt.Columns[16].Caption = "成本試算申請日期";
            dt.Columns[17].Caption = "報價備註";
            dt.Columns[18].Caption = "營標送驗";
            dt.Columns[19].Caption = "營標送驗申請日期";
            dt.Columns[20].Caption = "業務更新日期";
            dt.Columns[21].Caption = "研發更新日期";

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

    protected void btn3_Click(object sender, EventArgs e)
    {

    }

    protected void btn4_Click(object sender, EventArgs e)
    {

    }

    protected void btn5_Click(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            if (Dialog.GetReturnValue().Equals("NeedPostBack"))
            {                
                this.Session["STATUS"] = DropDownList1.Text.Trim();
            }

        }
    }

    protected void btn6_Click(object sender, EventArgs e)
    {
        this.Session["STATUS"] = DropDownList1.Text.Trim();
    }

    #endregion
}