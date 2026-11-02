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

public partial class CDS_WebPage_TKRESEARCHTBSALESDEV : Ede.Uof.Utility.Page.BasePage
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

        string cmdTxt = @" SELECT
                            [SERNO]
                            ,[STATUS]
                            ,[CLIENT]
                            ,[SALES]
                            ,[PROD]
                            ,[SPEC]
                            ,[VALID]
                            ,[PRICES]
                            ,[PROMOTIONS]
                            ,[PLACES]
                            ,[ONSALES]
                            ,CONVERT(NVARCHAR,[ONSALESDATES],111) AS ONSALESDATES
                            ,[DESIGNS]
                            ,[SALESHISTORYS]
                            ,[DEVHISTORYS]
                            FROM [TKRESEARCH].[dbo].[TBSALESDEV]
                            WHERE STATUS=@STATUS 
                            ORDER BY [SALES],[CLIENT],[PROD]                            
                            ";

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
            //Button1
            //Get the button that raised the event
            Button btn = (Button)e.Row.FindControl("Button1");
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue = btn.CommandArgument;
            DataRowView row = (DataRowView)e.Row.DataItem;
            Button lbtnName = (Button)e.Row.FindControl("Button1");
            ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TKRESEARCHTBSALESDEVDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            //Button2
            //Get the button that raised the event
            Button btn2 = (Button)e.Row.FindControl("Button2");
            //Get the row that contains this button
            GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue2 = btn2.CommandArgument;
            DataRowView row2 = (DataRowView)e.Row.DataItem;
            Button lbtnName2 = (Button)e.Row.FindControl("Button2");
            ExpandoObject param2 = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName2, "~/CDS/WebPage/COP/TKRESEARCHTBSALESDEVDialogSALESADD.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param2);

            //Button3
            //Get the button that raised the event
            Button btn3 = (Button)e.Row.FindControl("Button3");
            //Get the row that contains this button
            GridViewRow gvr3 = (GridViewRow)btn3.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue3 = btn3.CommandArgument;
            DataRowView row3 = (DataRowView)e.Row.DataItem;
            Button lbtnName3 = (Button)e.Row.FindControl("Button3");
            ExpandoObject param3 = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName3, "~/CDS/WebPage/COP/TKRESEARCHTBSALESDEVDialogDEVSADD.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param3);
        }


    }

    public void OnBeforeExport(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string STATUS = DropDownList1.Text;

        string cmdTxt = @" 
                            ";

        m_db.AddParameter("@STATUS", STATUS);


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            dt.Columns[0].Caption = "編號";
           

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