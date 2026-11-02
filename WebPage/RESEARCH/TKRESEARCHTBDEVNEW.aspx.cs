using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;

public partial class CDS_WebPage_TKRESEARCHTBDEVNEW : Ede.Uof.Utility.Page.BasePage
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

            BindDropDownList();
        }
        else
        {
           
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

        string cmdTxt = @" SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='MEMOSTATUS' UNION ALL SELECT  '0','0','全部','全部' ";

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
        string cmdTxt = null;

        if (STATUS.Equals("全部"))
        {
            cmdTxt = @" SELECT 
                            [SERNO]
                            ,[STATUS]
                            ,CONVERT(NVARCHAR(10),[SDATES],111) AS SDATES
                            ,[PRODUCTS]
                            ,[CLIENTS]
                            ,[SALES]
                            ,[NUMS]
                            ,CONVERT(NVARCHAR(10),[TESTDATES],111) AS TESTDATES
                            ,[TESTMEMO]
                            ,[TASTESMEMO]
                            ,[PACKAGES]
                            ,[FEASIBILITYS]
                            ,[DESINGS]
                            ,CONVERT(NVARCHAR(10),[DESINGSDATES] ,111) AS DESINGSDATES
                            ,[COSTS]
                            ,CONVERT(NVARCHAR(10),[COSTSDATES] ,111) AS COSTSDATES 
                            ,[PROOFREADINGS]
                            ,CONVERT(NVARCHAR(10),[PROOFREADINGSDATES] ,111) AS PROOFREADINGSDATES
                            ,[TESTPRODS]
                            ,[PRODS]
                            ,[ORICHECKS]
                            , CONVERT(NVARCHAR(10),[ORICHECKSDATES],111) AS ORICHECKSDATES
                            ,[NUTRICHECKS]
                            , CONVERT(NVARCHAR(10),[NUTRICHECKSDATES],111) AS NUTRICHECKSDATES
                            ,[REMARKS]
                            ,[CLOSEDDATES]
                            FROM [TKRESEARCH].[dbo].[TBDEVNEW]
                            WHERE 1=1
                            ORDER BY [SALES],[PRODUCTS]                            
                            ";
        }
        else
        {
             cmdTxt = @" SELECT 
                          [SERNO]
                            ,[STATUS]
                            ,CONVERT(NVARCHAR(10),[SDATES],111) AS SDATES
                            ,[PRODUCTS]
                            ,[CLIENTS]
                            ,[SALES]
                            ,[NUMS]
                            ,CONVERT(NVARCHAR(10),[TESTDATES],111) AS TESTDATES
                            ,[TESTMEMO]
                            ,[TASTESMEMO]
                            ,[PACKAGES]
                            ,[FEASIBILITYS]
                            ,[DESINGS]
                            ,CONVERT(NVARCHAR(10),[DESINGSDATES] ,111) AS DESINGSDATES
                            ,[COSTS]
                           ,CONVERT(NVARCHAR(10),[COSTSDATES] ,111) AS COSTSDATES 
                            ,[PROOFREADINGS]
                            ,CONVERT(NVARCHAR(10),[PROOFREADINGSDATES] ,111) AS PROOFREADINGSDATES
                            ,[TESTPRODS]
                            ,[PRODS]
                            ,[ORICHECKS]
                            , CONVERT(NVARCHAR(10),[ORICHECKSDATES],111) AS ORICHECKSDATES
                            ,[NUTRICHECKS]
                            , CONVERT(NVARCHAR(10),[NUTRICHECKSDATES],111) AS NUTRICHECKSDATES
                            ,[REMARKS]
                            ,[CLOSEDDATES]
                            FROM [TKRESEARCH].[dbo].[TBDEVNEW]
                            WHERE STATUS=@STATUS 
                            ORDER BY [SALES],[PRODUCTS]                                 
                            ";
        }
       

        m_db.AddParameter("@STATUS", STATUS);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();

        for (int i = 0; i < Grid1.Rows.Count; i++)
        {
            for (int j = 0; j < Grid1.Rows[i].Cells.Count; j++)
            {
                if (Grid1.Rows[i].Cells[j].Text.Equals("進行中"))
                {
                    Grid1.Rows[i].Cells[j].ForeColor = Color.Red;
                }
               
            }
        }

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
            Dialog.Open2(lbtnName, "~/CDS/WebPage/RESEARCH/TKRESEARCHTBDEVNEWDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

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
            Dialog.Open2(lbtnName2, "~/CDS/WebPage/RESEARCH/TKRESEARCHTBDEVNEWDialogMEMOADD.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param2);


        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //string STATUS = e.Row.Cells[1].Text.ToString();

            //if(STATUS.Contains("進行中"))
            //{
            //    e.Row.ForeColor = Color.Red;
            //}

            //if (e.Row.RowIndex % 2 == 0)
            //{
            //    e.Row.BackColor = Color.Blue;  //Color.Blue; 
            //}
            //else
            //{
            //    e.Row.BackColor = Color.Red;
            //}
        }


    }
    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Button1")
        {
            //MsgBox("Button1", this.Page, this);
            BindGrid(DropDownList1.Text);
        }
        if (e.CommandName == "Button2")
        {
            //MsgBox("Button1", this.Page, this);
            BindGrid(DropDownList1.Text);
        }


    }

    protected void Grid1_OnItemDataBound(object sender, GridViewCommandEventArgs e)
    {
        
    }


    public void OnBeforeExport(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string STATUS = DropDownList1.Text;
        string cmdTxt = null;      

        if (STATUS.Equals("全部"))
        {
            cmdTxt = @"    
                        SELECT 
                        [SERNO]
                        ,[STATUS]
                        ,[PRODUCTS]
                        ,[TESTMEMO]
                        ,[TASTESMEMO]
                        ,[PACKAGES]
                        ,[FEASIBILITYS]
                        ,[DESINGS]
                        ,CONVERT(NVARCHAR(10),[DESINGSDATES] ,111) AS DESINGSDATES
                        ,[COSTS]
                        ,CONVERT(NVARCHAR(10),[COSTSDATES] ,111) AS COSTSDATES 
                        ,[ORICHECKS]
                         , CONVERT(NVARCHAR(10),[ORICHECKSDATES],111) AS ORICHECKSDATES
                        ,[PROOFREADINGS]
                          ,CONVERT(NVARCHAR(10),[PROOFREADINGSDATES] ,111) AS PROOFREADINGSDATES
                        ,[TESTPRODS]
                        ,[PRODS]
                        ,[NUTRICHECKS]
                        , CONVERT(NVARCHAR(10),[NUTRICHECKSDATES],111) AS NUTRICHECKSDATES
                        ,[SALES]
                        ,[REMARKS]
                        ,[CLOSEDDATES]

                            FROM [TKRESEARCH].[dbo].[TBDEVNEW]
                            WHERE 1=1
                            ORDER BY [SALES],[PRODUCTS]                                 
                            ";
        }
        else
        {
            cmdTxt = @" 
                        SELECT 
                        [SERNO]
                        ,[STATUS]
                        ,[PRODUCTS]
                        ,[TESTMEMO]
                        ,[TASTESMEMO]
                        ,[PACKAGES]
                        ,[FEASIBILITYS]
                        ,[DESINGS]
                        ,CONVERT(NVARCHAR(10),[DESINGSDATES] ,111) AS DESINGSDATES
                        ,[COSTS]
                        ,CONVERT(NVARCHAR(10),[COSTSDATES] ,111) AS COSTSDATES 
                        ,[ORICHECKS]
                         , CONVERT(NVARCHAR(10),[ORICHECKSDATES],111) AS ORICHECKSDATES
                        ,[PROOFREADINGS]
                          ,CONVERT(NVARCHAR(10),[PROOFREADINGSDATES] ,111) AS PROOFREADINGSDATES
                        ,[TESTPRODS]
                        ,[PRODS]
                        ,[NUTRICHECKS]
                        , CONVERT(NVARCHAR(10),[NUTRICHECKSDATES],111) AS NUTRICHECKSDATES
                        ,[SALES]
                        ,[REMARKS]
                        ,[CLOSEDDATES]
                          
                        FROM [TKRESEARCH].[dbo].[TBDEVNEW]
                        WHERE STATUS=@STATUS 
                        ORDER BY [SALES],[PRODUCTS]           

                        ";
        }


        m_db.AddParameter("@STATUS", STATUS);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            dt.Columns[0].Caption = "編號";
            dt.Columns[1].Caption = "狀態";
            dt.Columns[2].Caption = "產品品項";
            dt.Columns[3].Caption = "樣品試作/試吃結果";
            dt.Columns[4].Caption = "口味確認";
            dt.Columns[5].Caption = "包裝型式重量";
            dt.Columns[6].Caption = "可行性";
            dt.Columns[7].Caption = "設計需求";
            dt.Columns[8].Caption = "設計需求預計完成日";
            dt.Columns[9].Caption = "成本試算";
            dt.Columns[10].Caption = "成本試算預計完成日";
            dt.Columns[11].Caption = "原料驗收作業";
            dt.Columns[12].Caption = "原料驗收作業預計完成日";
            dt.Columns[13].Caption = "校稿完成";
            dt.Columns[14].Caption = "校稿完成預計完成日";
            dt.Columns[15].Caption = "試量產日期";
            dt.Columns[16].Caption = "正式量產日期";
            dt.Columns[17].Caption = "營養標示作業";
            dt.Columns[18].Caption = "營養標示作業預計完成日";
            dt.Columns[19].Caption = "負責業務";
            dt.Columns[20].Caption = "備註";
            dt.Columns[21].Caption = "結案日期";
         




            e.Datasource = dt;
        }
    }

    public void MsgBox(string ex, Page pg, object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);

        //MsgBox("完成", this.Page, this);
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
        if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            if (Dialog.GetReturnValue().Equals("REFRESH"))
            {
                BindGrid(DropDownList1.Text);
            }

        }
    }

    protected void btn5_Click(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            if (Dialog.GetReturnValue().Equals("NeedPostBack"))
            {
                BindGrid(DropDownList1.Text);
            }

        }
    }

    protected void btn6_Click(object sender, EventArgs e)
    {
        BindGrid(DropDownList1.Text);
    }

    #endregion
}