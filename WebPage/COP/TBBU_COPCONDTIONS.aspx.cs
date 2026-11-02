using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;

public partial class CDS_WebPage_COP_TBBU_COPCONDTIONS : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindGrid();
        }
        else
        {
           
        }
    }
    #region FUNCTION

    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        // 日期
        if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox1.Text))
        {            
            QUERYS.AppendFormat(@" AND (MA001 LIKE '%{0}%' OR MA002 LIKE '%{0}%')", TextBox1.Text.Trim() );

        }
        else
        {
            QUERYS.AppendFormat(@" ");
        }
        cmdTxt.AppendFormat(@"
                            SELECT 
                            [ID]
                            ,[SERNO]
                            ,[MA001]
                            ,[MA002]
                            ,[CONTACTPERSON]
                            ,[TEL1]
                            ,[TEL2]
                            ,[ISPURATTCH]
                            ,[ISCOPATTCH]
                            ,[ISSHOWMONEYS]
                            ,[ISINVOICES]
                            ,[ISSHIPMARK]
                            ,[LIMITDAYS]
                            ,[PAYMENT]
                            ,[SENDADDRESS]
                            ,[EMAILS]
                            ,REPLACE([COMMENT] ,char(10),'<br/>') AS [COMMENT] 
                            FROM [TKBUSINESS].[dbo].[COPCONDTIONS]
                            WHERE ISUSED='Y'
                            {0}

                            ORDER BY SERNO
                        ", QUERYS.ToString() );
       

        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {   
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
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_COPCONDTIONSDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        }


    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Button1")
        {
            //MsgBox("Button1", this.Page, this);
            BindGrid();

            MsgBox("完成", this.Page, this);
        }
       

    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);


        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[SERNO]
                        ,[MA001]
                        ,[MA002]
                        ,[CONTACTPERSON]
                        ,[TEL1]
                        ,[ISPURATTCH]
                        ,[ISCOPATTCH]
                        ,[ISSHOWMONEYS]
                        ,[ISINVOICES]
                        ,[ISSHIPMARK]
                        ,[LIMITDAYS]
                        ,[PAYMENT]
                        ,[SENDADDRESS]
                        ,[EMAILS]
                        ,REPLACE([COMMENT] ,char(10),'<br/>') AS [COMMENT] 
                        FROM [TKBUSINESS].[dbo].[COPCONDTIONS]
                        WHERE ISUSED='Y'
                        ORDER BY SERNO
                        ";



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            dt.Columns[0].Caption = "ID";
            dt.Columns[1].Caption = "序號";
            dt.Columns[2].Caption = "客戶代號";
            dt.Columns[3].Caption = "客戶名稱";
            dt.Columns[4].Caption = "連絡人";
            dt.Columns[5].Caption = "電話1";
            dt.Columns[6].Caption = "採購單(附單)";
            dt.Columns[7].Caption = "銷貨單(附單)";
            dt.Columns[8].Caption = "是否顯$(附單)";
            dt.Columns[9].Caption = "發票(附單)";
            dt.Columns[10].Caption = "麥頭(附單)";
            dt.Columns[11].Caption = "允收期限";
            dt.Columns[12].Caption = "收款條件";
            dt.Columns[13].Caption = "寄送地址";
            dt.Columns[14].Caption = "EMAILS";
            dt.Columns[15].Caption = "備註";

            e.Datasource = dt;
        }
    }
    public void MsgBox(string ex, Page pg, object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);
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
       
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();        

    }

    #endregion
}