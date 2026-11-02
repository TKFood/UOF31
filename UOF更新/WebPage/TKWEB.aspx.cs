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

public partial class CDS_WebPage_TKWEB : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //用ExpandoObject物件傳遞參數
        //ExpandoObject param = new { Name = "asd" }.ToExpando();
        ExpandoObject param = new { Name = txtParam.Text }.ToExpando();

        //因為執行此行後，才會把JS的Event註冊到頁面上，所以過此行後下一次按btn元件的Event才會開窗並傳參數
        //故Dialog.Open2適合於參數為固定式的
        Dialog.Open2(btn, "~/CDS/WebPage/Dialog.aspx", "", 800, 600, Dialog.PostBackType.Allows, param);

        BindGrid();

        //if (!IsPostBack)
        //{
        //    BindGrid();
        //}
       

    }

    private void BindGrid()
    {
        //建立Grid資料
        DataSet ds = new DataSet();
        DatabaseHelper DbQuery = new DatabaseHelper();
        DataTable dt = new DataTable();

        //資源來源-直接指定dt
        //DataRow row;

        //dt.Columns.Add("RANK");
        //dt.Columns.Add("TITLE_NAME");

        //row = dt.NewRow();
        //row["RANK"] = "Tony";
        //row["TITLE_NAME"] = "001";
        //dt.Rows.Add(row);

        //row = dt.NewRow();
        //row["RANK"] = "Mary";
        //row["TITLE_NAME"] = "002";
        //dt.Rows.Add(row);

        //dt.Rows.Add(new object[] { $"{DateTime.Today.ToString("yyyyMMdd")}001", "Tony" });
        //dt.Rows.Add(new object[] { $"{DateTime.Today.ToString("yyyyMMdd")}002", "Mary" });
        //dt.Rows.Add(new object[] { $"{DateTime.Today.ToString("yyyyMMdd")}003", "Wang" });

        //資源來源-用DatabaseHelper的DbQuery來查詢
        //ds = DbQuery.ExecuteDataSet("SELECT [RANK],[TITLE_NAME] FROM [UOFTEST].[dbo].[TB_EB_JOB_TITLE] ORDER BY [RANK]");

        //資源來源-用SqlCommand +SqlDataAdapter +DataTable 來查詢
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("SELECT  [ID],[YEARS] ,[MONTHS],[WEEKS],CONVERT(NVARCHAR,[DATES],112) AS DATES,[SELLER],[MONEYS] FROM [UOFTEST].[dbo].[TKSELLERMONEYS] ORDER BY [YEARS] ,[MONTHS],[WEEKS],[SELLER]", conn);
            ds.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            conn.Open();

            adapter.Fill(ds, command.ToString());
        }

        Grid1.DataSource = ds.Tables[0];
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
            Dialog.Open2(lbtnName, "~/CDS/WebPage/TKWEBDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        }
    }

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
                BindGrid();
            }

        }
    }
}