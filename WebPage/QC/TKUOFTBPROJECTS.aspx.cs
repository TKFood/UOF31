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

public partial class CDS_WebPage_TKUOFTBPROJECTS : Ede.Uof.Utility.Page.BasePage
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
            BindGrid("已完成");
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

        string cmdTxt = @" SELECT [ID],[KIND],[PARAID],[PARANAME] FROM [TKUOF].[dbo].[TBPARA] WHERE [KIND]='PROJECTSTATUS' ORDER BY [PARAID] ";

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

        string cmdTxt = @" 
                        SELECT  [ID],[NO],[SUBJECT],[CREATE_NEME],[CREATE_DEP],CONVERT(NVARCHAR,[CREATE_TIME],111) [CREATE_TIME],[STATUS],[CONTENTS],CONVERT(NVARCHAR,[PUCLOSE_TIME],111) [PUCLOSE_TIME],[HADNUMS],[HADCONTENTS],[NOWNUMS],[TOTALNUMS],[CLOSE_CONTENTS],CONVERT(NVARCHAR,[CLOSE_TIME],111) [CLOSE_TIME]
                        FROM [TKQC].[dbo].[TBPROJECTS]
                        ORDER BY [NO]
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
            DataRowView row = (DataRowView)e.Row.DataItem;
            LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

            ExpandoObject param = new { ID = row["NO"].ToString() }.ToExpando();

            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName, "~/CDS/WebPage/QC/TKUOFTBPROJECTSDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            LinkButton MEMO = (LinkButton)e.Row.FindControl("MEMO");

            ExpandoObject param = new { ID = row["NO"].ToString() }.ToExpando();

            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(MEMO, "~/CDS/WebPage/QC/TKUOFTBPROJECTSMEMOADD.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        }
    }

    public void OnBeforeExport(object sender,Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string STATUS = DropDownList1.Text;

        string cmdTxt = @" 
                        SELECT  [ID],[NO],[SUBJECT],[CREATE_NEME],[CREATE_DEP],CONVERT(NVARCHAR,[CREATE_TIME],111) [CREATE_TIME],[STATUS],[CONTENTS],CONVERT(NVARCHAR,[PUCLOSE_TIME],111) [PUCLOSE_TIME],[HADNUMS],[HADCONTENTS],[NOWNUMS],[TOTALNUMS],[CLOSE_CONTENTS],CONVERT(NVARCHAR,[CLOSE_TIME],111) [CLOSE_TIME]
                        FROM [TKQC].[dbo].[TBPROJECTS]
                        ORDER BY [NO]
                        ";

        m_db.AddParameter("@STATUS", STATUS);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count>0)
        {
            dt.Columns[0].Caption = "ID";
            dt.Columns[1].Caption = "編號";
            dt.Columns[2].Caption = "專案主旨";
            dt.Columns[3].Caption = "創建人員";
            dt.Columns[4].Caption = "建立部門";
            dt.Columns[5].Caption = "創建日期";
            dt.Columns[6].Caption = "狀態";
            dt.Columns[7].Caption = "專案內容";
            dt.Columns[8].Caption = "預計結案期限";
            dt.Columns[9].Caption = "舊有案件數";
            dt.Columns[10].Caption = "舊有案件列表";
            dt.Columns[11].Caption = "專案案件列表";
            dt.Columns[12].Caption = "累計案件數";
            dt.Columns[13].Caption = "結案說明";
            dt.Columns[13].Caption = "結案日期";



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