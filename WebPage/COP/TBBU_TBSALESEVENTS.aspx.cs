using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using Telerik.Web.UI.Calendar;
using Image = System.Web.UI.WebControls.Image;

public partial class CDS_WebPage_TBBU_TBSALESEVENTS : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ACCOUNT = null;
        string NAME = null;
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;


        if (!IsPostBack)
        {

            RadDatePicker3.SelectedDate = DateTime.Now;
            RadDatePicker4.SelectedDate = DateTime.Now;

            BindDropDownList1();
            BindDropDownList2();
            BindDropDownList3();

            BindDropDownList4();
            BindDropDownList5();
            BindDropDownList6();

            BindGrid();
            BindGrid3();
        }
        else
        {

          
           

        }

       


    }
    #region FUNCTION
    protected void RadDatePicker3_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        RadDatePicker4.SelectedDate = RadDatePicker3.SelectedDate;
    }
    private void BindDropDownList1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ISCLOSES", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT [ID],[ISCLOSES] FROM [TKBUSINESS].[dbo].[TBSALESCLOSES] ORDER BY [ID]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "ISCLOSES";
            DropDownList1.DataValueField = "ISCLOSES";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("NAME", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[NAME] FROM [TKBUSINESS].[dbo].[TBSALESNAME] ORDER BY [ID]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "NAME";
            DropDownList2.DataValueField = "NAME";
            DropDownList2.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("KINDS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[KINDS] FROM [TKBUSINESS].[dbo].[TBSALESKINDS] ORDER BY [ID]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList3.DataSource = dt;
            DropDownList3.DataTextField = "KINDS";
            DropDownList3.DataValueField = "KINDS";
            DropDownList3.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList4()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ISCLOSES", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT [ID],[ISCLOSES] FROM [TKBUSINESS].[dbo].[TBSALESCLOSES] ORDER BY [ID]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList4.DataSource = dt;
            DropDownList4.DataTextField = "ISCLOSES";
            DropDownList4.DataValueField = "ISCLOSES";
            DropDownList4.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList5()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("NAME", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[NAME] FROM [TKBUSINESS].[dbo].[TBSALESNAME] ORDER BY [ID]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList5.DataSource = dt;
            DropDownList5.DataTextField = "NAME";
            DropDownList5.DataValueField = "NAME";
            DropDownList5.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList6()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("KINDS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[KINDS] FROM [TKBUSINESS].[dbo].[TBSALESKINDS] ORDER BY [ID]";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList6.DataSource = dt;
            DropDownList6.DataTextField = "KINDS";
            DropDownList6.DataValueField = "KINDS";
            DropDownList6.DataBind();

        }
        else
        {

        }



    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //是否結案
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [ISCLOSE] LIKE '%{0}%'", DropDownList1.Text);
            }
           
        }
        //業務員
        if (!string.IsNullOrEmpty(DropDownList2.Text))
        {
            if(DropDownList2.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else 
            {
                QUERYS.AppendFormat(@" AND [SALES] LIKE '%{0}%'", DropDownList2.Text);
            }
           
        }
        //是否結案
        if (!string.IsNullOrEmpty(DropDownList3.Text))
        {
            if (DropDownList3.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [KINDS] LIKE '%{0}%'", DropDownList3.Text);
            }

        }
        //客戶
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND [CLIENTS] LIKE '%{0}%'", TextBox1.Text);
        }
        //專案
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS.AppendFormat(@" AND [PROJECTS] LIKE '%{0}%'", TextBox2.Text);
        }
        //新增日期+更新日期
        if (RadDatePicker1.SelectedDate != null && RadDatePicker2.SelectedDate != null)
        {
            QUERYS.AppendFormat(@" AND (CONVERT(NVARCHAR,[ADDDATES],112)>='{0}' AND CONVERT(NVARCHAR,[ADDDATES],112)<='{1}' OR CONVERT(NVARCHAR,[UPDATEDATES],112)>='{2}' AND CONVERT(NVARCHAR,[UPDATEDATES],112)<='{3}' )", RadDatePicker1.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker2.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker1.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker2.SelectedDate.Value.ToString("yyyyMMdd"));

        }
        //起始日/結案日
        if (RadDatePicker5.SelectedDate != null && RadDatePicker6.SelectedDate != null)
        {
            QUERYS.AppendFormat(@" AND (REPLACE([SDAYS],'/','')>='{0}' AND REPLACE([SDAYS],'/','')<='{1}' OR REPLACE([EDAYS],'/','')>='{2}' AND REPLACE([EDAYS],'/','')<='{3}' )", RadDatePicker5.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker6.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker5.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker6.SelectedDate.Value.ToString("yyyyMMdd"));

        }






        cmdTxt.AppendFormat(@"                          
                            SELECT 
                            [ID]
                            ,[SALES]
                            ,[KINDS]
                            ,[CLIENTS]
                            ,[PROJECTS]
                            ,[EVENTS]
                            ,[SDAYS]
                            ,[EDAYS]
                            ,REPLACE([COMMENTS],char(10),'<br/>') AS [COMMENTS] 
                            ,[ISCLOSE]
                            ,[FILENAME]
                            ,CONVERT(NVARCHAR,[ADDDATES],111) AS [ADDDATES]
                            ,CONVERT(NVARCHAR,[UPDATEDATES],111) AS [UPDATEDATES] 

                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            WHERE 1=1
                            {0}
                            ORDER BY [SALES],[CLIENTS],[PROJECTS],[EDAYS]
                              
                            ", QUERYS.ToString());




        
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    private void BindGridOVER()
    {
        RadDatePicker1.SelectedDate = DateTime.Now.AddYears(-1).AddDays(-9);
        RadDatePicker2.SelectedDate = DateTime.Now.AddDays(-8);


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //是否結案
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [ISCLOSE] LIKE '%{0}%'", DropDownList1.Text);
            }

        }
        //業務員
        if (!string.IsNullOrEmpty(DropDownList2.Text))
        {
            if (DropDownList2.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [SALES] LIKE '%{0}%'", DropDownList2.Text);
            }

        }
        //是否結案
        if (!string.IsNullOrEmpty(DropDownList3.Text))
        {
            if (DropDownList3.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [KINDS] LIKE '%{0}%'", DropDownList3.Text);
            }

        }
        //客戶
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND [CLIENTS] LIKE '%{0}%'", TextBox1.Text);
        }
        //專案
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS.AppendFormat(@" AND [PROJECTS] LIKE '%{0}%'", TextBox2.Text);
        }
        //新增日期+更新日期
        if (RadDatePicker1.SelectedDate != null && RadDatePicker2.SelectedDate != null)
        {
            QUERYS.AppendFormat(@" AND (CONVERT(NVARCHAR,[ADDDATES],112)>='{0}' AND CONVERT(NVARCHAR,[ADDDATES],112)<='{1}' OR CONVERT(NVARCHAR,[UPDATEDATES],112)>='{2}' AND CONVERT(NVARCHAR,[UPDATEDATES],112)<='{3}' )", RadDatePicker1.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker2.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker1.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker2.SelectedDate.Value.ToString("yyyyMMdd"));

        }






        cmdTxt.AppendFormat(@"                          
                            SELECT 
                            [ID]
                            ,[SALES]
                            ,[KINDS]
                            ,[CLIENTS]
                            ,[PROJECTS]
                            ,[EVENTS]
                            ,[SDAYS]
                            ,[EDAYS]
                            ,REPLACE([COMMENTS],char(10),'<br/>') AS [COMMENTS] 
                            ,[ISCLOSE]
                            ,[FILENAME]
                            ,CONVERT(NVARCHAR,[ADDDATES],111) AS [ADDDATES]
                            ,CONVERT(NVARCHAR,[UPDATEDATES],111) AS [UPDATEDATES] 

                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            WHERE 1=1
                            {0}
                            AND CONVERT(NVARCHAR,[UPDATEDATES],112)<='{1}'

                            ORDER BY [SALES],[CLIENTS],[PROJECTS],[EDAYS]
                              
                            ", QUERYS.ToString(),DateTime.Now.AddDays(-8).ToString("yyyyMMdd"));





        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    private void BindGridIN()
    {
        RadDatePicker1.SelectedDate = DateTime.Now.AddDays(-7);
        RadDatePicker2.SelectedDate = DateTime.Now;


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //是否結案
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [ISCLOSE] LIKE '%{0}%'", DropDownList1.Text);
            }

        }
        //業務員
        if (!string.IsNullOrEmpty(DropDownList2.Text))
        {
            if (DropDownList2.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [SALES] LIKE '%{0}%'", DropDownList2.Text);
            }

        }
        //是否結案
        if (!string.IsNullOrEmpty(DropDownList3.Text))
        {
            if (DropDownList3.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [KINDS] LIKE '%{0}%'", DropDownList3.Text);
            }

        }
        //客戶
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND [CLIENTS] LIKE '%{0}%'", TextBox1.Text);
        }
        //專案
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS.AppendFormat(@" AND [PROJECTS] LIKE '%{0}%'", TextBox2.Text);
        }
        //新增日期+更新日期
        if (RadDatePicker1.SelectedDate != null && RadDatePicker2.SelectedDate != null)
        {
            QUERYS.AppendFormat(@" AND (CONVERT(NVARCHAR,[ADDDATES],112)>='{0}' AND CONVERT(NVARCHAR,[ADDDATES],112)<='{1}' OR CONVERT(NVARCHAR,[UPDATEDATES],112)>='{2}' AND CONVERT(NVARCHAR,[UPDATEDATES],112)<='{3}' )", RadDatePicker1.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker2.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker1.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker2.SelectedDate.Value.ToString("yyyyMMdd"));
            
        }
      





        cmdTxt.AppendFormat(@"                          
                            SELECT 
                            [ID]
                            ,[SALES]
                            ,[KINDS]
                            ,[CLIENTS]
                            ,[PROJECTS]
                            ,[EVENTS]
                            ,[SDAYS]
                            ,[EDAYS]
                            ,REPLACE([COMMENTS],char(10),'<br/>') AS [COMMENTS] 
                            ,[ISCLOSE]
                            ,[FILENAME]
                            ,CONVERT(NVARCHAR,[ADDDATES],111) AS [ADDDATES]
                            ,CONVERT(NVARCHAR,[UPDATEDATES],111) AS [UPDATEDATES] 

                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            WHERE 1=1
                            {0}

                            ORDER BY [SALES],[CLIENTS],[PROJECTS],[EDAYS]
                              
                            ", QUERYS.ToString());





        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }


    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid("");
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string PATH = "https://eip.tkfood.com.tw/BM/upload/note/";
        Image img = (Image)e.Row.FindControl("Image1");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            Image img1 = (Image)e.Row.FindControl("Image1");

            if (!string.IsNullOrEmpty(row["FILENAME"].ToString()))
            {
                img.ImageUrl = PATH + row["FILENAME"].ToString();

                //獲取當前行的圖片路徑
                string ImgUrl = img.ImageUrl;
                ////給帶圖片的單元格添加點擊事件
                //e.Row.Cells[10].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
                //    + ".checked=true;CellClick('" + ImgUrl + "')");

                //img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
            }


        }

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
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBSALESEVENTSDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);


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
            Dialog.Open2(lbtnName2, "~/CDS/WebPage/COP/TBBU_TBSALESEVENTSCOMMENTSDialogSALESADD.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param2);

        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {  
        if (e.CommandName == "Button1")
        {
            BindGrid();
        }
        if (e.CommandName == "Button2")
        {
            BindGrid();
        }
        if (e.CommandName == "Button3")
        {
            //取得 custid 的值
            var ID = e.CommandArgument;

            UPDATETBSALESEVENTS(ID.ToString());
            BindGrid();
            // ... 做後面要做的事情 .....
            //Response.Write("<script>alert('" + ID.ToString() + "')</script>");

            //顯示訊息
            //ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "alert('"+ID.ToString()+"')", true);
        }
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);


        //string cmdTxt = @" 
        //               SELECT [PRODUCTS].[MB001],[PRODUCTSFEATURES],[SALESFOCUS],[COPYWRITINGS],[PICPATHS]
        //                ,MB002,MB003,MB004,MA003,ISNULL(MD007,0) AS MD007,CONVERT(NVARCHAR,MB023)+(CASE WHEN MB198='1' THEN '天' ELSE (CASE WHEN MB198='2' THEN '月' ELSE '年' END ) END ) AS 'VALIDITYPERIOD',CONVERT(decimal(16,3),ISNULL(MB047,0)) AS MB047,MB013
        //                ,[ALBUM_GUID], [PHOTO_GUID],[PHOTO_DESC],[FILE_ID],[RESIZE_FILE_ID],[THUMBNAIL_FILE_ID]
        //                FROM [TKBUSINESS].[dbo].[PRODUCTS]
        //                LEFT JOIN [TK].dbo.[INVMB] ON [PRODUCTS].[MB001]=[INVMB].[MB001]
        //                LEFT JOIN [TK].dbo.INVMA ON MA001='9' AND MA002=MB115
        //                LEFT JOIN [TK].dbo.BOMMD ON MD001=[INVMB].[MB001] AND MD003 LIKE '201%'
        //                LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EIP_ALBUM_PHOTO] ON [PHOTO_TOPIC]=[PRODUCTS].[MB001] COLLATE Chinese_Taiwan_Stroke_BIN
        //                ORDER BY [PRODUCTS].[MB001]
        //                ";



        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    dt.Columns[0].Caption = "ID";


        //    e.Datasource = dt;
        //}
    }

    private void BindGrid2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();


        //客戶
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            QUERYS.AppendFormat(@" AND [tb_COMPANY].COMPANY_NAME LIKE '%{0}%'", TextBox3.Text);           
        }


        if (!string.IsNullOrEmpty(QUERYS.ToString()))
        {
            cmdTxt.AppendFormat(@"                          
                            SELECT
                            [tb_COMPANY].COMPANY_NAME
                            ,CONVERT(NVARCHAR,[tb_NOTE].[CREATE_DATETIME],111) AS [CREATE_DATETIME]
                            ,REPLACE([NOTE_CONTENT],char(10),'<br/>') AS [NOTE_CONTENT]

                            FROM [HJ_BM_DB].[dbo].[tb_NOTE]
                            LEFT JOIN [HJ_BM_DB].[dbo].[tb_COMPANY] ON [tb_NOTE].COMPANY_ID=[tb_COMPANY].COMPANY_ID
                            WHERE 1=1
                            {0}
                            ORDER BY [tb_NOTE].[CREATE_DATETIME]
                            ", QUERYS.ToString());

        }





        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid("");
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    
    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL2();
    }

    private void BindGrid3()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //是否結案
        if (!string.IsNullOrEmpty(DropDownList4.Text))
        {
            if (DropDownList4.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [ISCLOSE] LIKE '%{0}%'", DropDownList4.Text);
            }

        }
        //業務員
        if (!string.IsNullOrEmpty(DropDownList5.Text))
        {
            if (DropDownList5.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [SALES] LIKE '%{0}%'", DropDownList5.Text);
            }

        }
        else
        {
            //如果沒有指定的業務員，就不要查詢結果
            QUERYS.AppendFormat(@" AND 1=0");
        }
        //是否
        if (!string.IsNullOrEmpty(DropDownList6.Text))
        {
            if (DropDownList6.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [KINDS] LIKE '%{0}%'", DropDownList6.Text);
            }

        }
        //客戶
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            QUERYS.AppendFormat(@" AND [CLIENTS] LIKE '%{0}%'", TextBox4.Text);
        }
        //專案
        if (!string.IsNullOrEmpty(TextBox5.Text))
        {
            QUERYS.AppendFormat(@" AND [PROJECTS] LIKE '%{0}%'", TextBox5.Text);
        }






        cmdTxt.AppendFormat(@"                          
                            SELECT 
                            [ID]
                            ,[SALES]
                            ,[KINDS]
                            ,[CLIENTS]
                            ,[PROJECTS]
                            ,[EVENTS]
                            ,[SDAYS]
                            ,[EDAYS]
                            ,REPLACE([COMMENTS],char(10),'<br/>') AS [COMMENTS] 
                            ,[ISCLOSE]
                            ,[FILENAME]
                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            WHERE 1=1
                            {0}
                            AND REPLACE([SDAYS],'/','')>='{1}' AND REPLACE([SDAYS],'/','')<='{2}'
                            ORDER BY [SALES],[CLIENTS],[PROJECTS],[EDAYS]
                              
                            ", QUERYS.ToString(), RadDatePicker3.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker4.SelectedDate.Value.ToString("yyyyMMdd"));





        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid3.DataSource = dt;
        Grid3.DataBind();
    }

    protected void grid_PageIndexChanging3(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid("");
    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string PATH = "https://eip.tkfood.com.tw/BM/upload/note/";
        Image img = (Image)e.Row.FindControl("Image1");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            Image img1 = (Image)e.Row.FindControl("Image1");

            if (!string.IsNullOrEmpty(row["FILENAME"].ToString()))
            {
                img.ImageUrl = PATH + row["FILENAME"].ToString();

                //獲取當前行的圖片路徑
                string ImgUrl = img.ImageUrl;
                ////給帶圖片的單元格添加點擊事件
                //e.Row.Cells[10].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
                //    + ".checked=true;CellClick('" + ImgUrl + "')");

                //img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
            }


        }

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
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBSALESEVENTSFORSALESDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);


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
            Dialog.Open2(lbtnName2, "~/CDS/WebPage/COP/TBBU_TBSALESEVENTSCOMMENTSFORSALESDialogSALESADD.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param2);

        }
    }

    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName == "Button1")
        //{
        //    BindGrid();
        //}
        //if (e.CommandName == "Button2")
        //{
        //    BindGrid();
        //}

    }
    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL3();

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);


        //string cmdTxt = @" 
        //               SELECT [PRODUCTS].[MB001],[PRODUCTSFEATURES],[SALESFOCUS],[COPYWRITINGS],[PICPATHS]
        //                ,MB002,MB003,MB004,MA003,ISNULL(MD007,0) AS MD007,CONVERT(NVARCHAR,MB023)+(CASE WHEN MB198='1' THEN '天' ELSE (CASE WHEN MB198='2' THEN '月' ELSE '年' END ) END ) AS 'VALIDITYPERIOD',CONVERT(decimal(16,3),ISNULL(MB047,0)) AS MB047,MB013
        //                ,[ALBUM_GUID], [PHOTO_GUID],[PHOTO_DESC],[FILE_ID],[RESIZE_FILE_ID],[THUMBNAIL_FILE_ID]
        //                FROM [TKBUSINESS].[dbo].[PRODUCTS]
        //                LEFT JOIN [TK].dbo.[INVMB] ON [PRODUCTS].[MB001]=[INVMB].[MB001]
        //                LEFT JOIN [TK].dbo.INVMA ON MA001='9' AND MA002=MB115
        //                LEFT JOIN [TK].dbo.BOMMD ON MD001=[INVMB].[MB001] AND MD003 LIKE '201%'
        //                LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EIP_ALBUM_PHOTO] ON [PHOTO_TOPIC]=[PRODUCTS].[MB001] COLLATE Chinese_Taiwan_Stroke_BIN
        //                ORDER BY [PRODUCTS].[MB001]
        //                ";



        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    dt.Columns[0].Caption = "ID";


        //    e.Datasource = dt;
        //}
    }

    public override void VerifyRenderingInServerForm(Control control) 
    { 

    }

    public void SETEXCEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        QUERYS.AppendFormat(@" ");   

        //是否結案
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [ISCLOSE] LIKE '%{0}%'", DropDownList1.Text);
            }

        }
        //業務員
        if (!string.IsNullOrEmpty(DropDownList2.Text))
        {
            if (DropDownList2.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [SALES] LIKE '%{0}%'", DropDownList2.Text);
            }

        }
        //是否結案
        if (!string.IsNullOrEmpty(DropDownList3.Text))
        {
            if (DropDownList3.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [KINDS] LIKE '%{0}%'", DropDownList3.Text);
            }

        }
        //客戶
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND [CLIENTS] LIKE '%{0}%'", TextBox1.Text);
        }
        //專案
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS.AppendFormat(@" AND [PROJECTS] LIKE '%{0}%'", TextBox2.Text);
        }

        cmdTxt.AppendFormat(@"                          
                            SELECT 
                            [ID]
                            ,[SALES]
                            ,[KINDS]
                            ,[CLIENTS]
                            ,[PROJECTS]
                            ,[EVENTS]
                            ,[SDAYS]
                            ,[EDAYS]
                            ,[COMMENTS] 
                            ,[ISCLOSE]
                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            WHERE 1=1
                            {0}
                            ORDER BY [SALES],[CLIENTS],[PROJECTS],[EDAYS]
                              
                            ", QUERYS.ToString());





        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count>0)
        {
            //檔案名稱
            var fileName = "明細清單" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {              

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());


                //預設行高
                ws.DefaultRowHeight = 20;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "編號";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 2].Value = "業務員";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 3].Value = "類別";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 4].Value = "客戶";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 5].Value = "專案";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 6].Value = "待辦事件";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 7].Value = "起始日";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 8].Value = "結案日";
                ws.Cells[1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 9].Value = "進度內容";
                ws.Cells[1, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 10].Value = "是否結案";
                ws.Cells[1, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["ID"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 2].Value = od["SALES"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 3].Value = od["KINDS"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 4].Value = od["CLIENTS"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 5].Value = od["PROJECTS"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 6].Value = od["EVENTS"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 7].Value = od["SDAYS"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 8].Value = od["EDAYS"].ToString();
                    ws.Cells[ROWS, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 9].Value = od["COMMENTS"].ToString();
                    ws.Cells[ROWS, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線      
                

                    ws.Cells[ROWS, 10].Value = od["ISCLOSE"].ToString();
                    ws.Cells[ROWS, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線


                    ROWS++;

                }




                ////預設列寬、行高
                ws.DefaultColWidth = 20; //預設列寬
                ws.DefaultRowHeight = 20; //預設行高
                ws.Cells.Style.ShrinkToFit = true;//单元格自动适应大小
             

                //// 遇\n或(char)10自動斷行
                ws.Cells.Style.WrapText = true;

                //自適應寬度設定
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //自適應高度設定        
                //ws.Row(1).CustomHeight = true;



                //儲存Excel
                //Byte[] bin = excel.GetAsByteArray();
                //File.WriteAllBytes(@"C:\TEMP\" + fileName, bin);

                //儲存和歸來的Excel檔案作為一個ByteArray
                var data = excel.GetAsByteArray();
                HttpResponse response = HttpContext.Current.Response;
                Response.Clear();

                //輸出標頭檔案　　
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + "");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data);
                Response.Flush();
                Response.End();
                //package.Save();//這個方法是直接下載到本地
            }
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
            //                                                            // 沒設置的話會跳出 Please set the excelpackage.licensecontext property

            
            ////var file = new FileInfo(fileName);
            //using (var excel = new ExcelPackage(file))
            //{
                
            //}
        }

    }

    public void SETEXCEL2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();


        //客戶
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            QUERYS.AppendFormat(@" AND [tb_COMPANY].COMPANY_NAME LIKE '%{0}%'", TextBox3.Text);
        }


        if (!string.IsNullOrEmpty(QUERYS.ToString()))
        {
            cmdTxt.AppendFormat(@"                          
                            SELECT
                            [tb_COMPANY].COMPANY_NAME
                            ,CONVERT(NVARCHAR,[tb_NOTE].[CREATE_DATETIME],111) AS [CREATE_DATETIME]
                            ,[NOTE_CONTENT]

                            FROM [HJ_BM_DB].[dbo].[tb_NOTE]
                            LEFT JOIN [HJ_BM_DB].[dbo].[tb_COMPANY] ON [tb_NOTE].COMPANY_ID=[tb_COMPANY].COMPANY_ID
                            WHERE 1=1
                            {0}
                            ORDER BY [tb_NOTE].[CREATE_DATETIME]
                            ", QUERYS.ToString());

        }

        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "客情明細" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());


                //預設行高
                ws.DefaultRowHeight = 20;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "客戶";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 2].Value = "日期";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 3].Value = "客情記錄";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                

                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["COMPANY_NAME"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 2].Value = od["CREATE_DATETIME"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 3].Value = od["NOTE_CONTENT"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線



                    ROWS++;

                }




                ////預設列寬、行高
                ws.DefaultColWidth = 40; //預設列寬
                ws.DefaultRowHeight = 40; //預設行高
                ws.Cells.Style.ShrinkToFit = true;//单元格自动适应大小


                //// 遇\n或(char)10自動斷行
                ws.Cells.Style.WrapText = true;

                //自適應寬度設定
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //自適應高度設定        
                //ws.Row(1).CustomHeight = true;



                //儲存Excel
                //Byte[] bin = excel.GetAsByteArray();
                //File.WriteAllBytes(@"C:\TEMP\" + fileName, bin);

                //儲存和歸來的Excel檔案作為一個ByteArray
                var data = excel.GetAsByteArray();
                HttpResponse response = HttpContext.Current.Response;
                Response.Clear();

                //輸出標頭檔案　　
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + "");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data);
                Response.Flush();
                Response.End();
                //package.Save();//這個方法是直接下載到本地
            }
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
            //                                                            // 沒設置的話會跳出 Please set the excelpackage.licensecontext property


            ////var file = new FileInfo(fileName);
            //using (var excel = new ExcelPackage(file))
            //{

            //}
        }

    }

    public void SETEXCEL3()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        QUERYS.AppendFormat(@" ");

        //是否結案
        if (!string.IsNullOrEmpty(DropDownList4.Text))
        {
            if (DropDownList4.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [ISCLOSE] LIKE '%{0}%'", DropDownList4.Text);
            }

        }
        //業務員
        if (!string.IsNullOrEmpty(DropDownList5.Text))
        {
            if (DropDownList5.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [SALES] LIKE '%{0}%'", DropDownList5.Text);
            }

        }
        //是否結案
        if (!string.IsNullOrEmpty(DropDownList6.Text))
        {
            if (DropDownList6.Text.Equals("全部"))
            {
                QUERYS.AppendFormat(@" ");
            }
            else
            {
                QUERYS.AppendFormat(@" AND [KINDS] LIKE '%{0}%'", DropDownList6.Text);
            }

        }
        //客戶
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            QUERYS.AppendFormat(@" AND [CLIENTS] LIKE '%{0}%'", TextBox4.Text);
        }
        //專案
        if (!string.IsNullOrEmpty(TextBox5.Text))
        {
            QUERYS.AppendFormat(@" AND [PROJECTS] LIKE '%{0}%'", TextBox5.Text);
        }

        cmdTxt.AppendFormat(@"                          
                            SELECT 
                            [ID]
                            ,[SALES]
                            ,[KINDS]
                            ,[CLIENTS]
                            ,[COMMENTS] 
                            ,[SDAYS]
                            ,[EDAYS]                            
                            ,[ISCLOSE]
                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            WHERE 1=1
                            {0}
                            AND REPLACE([SDAYS],'/','')>='{1}' AND REPLACE([SDAYS],'/','')<='{2}'
                            ORDER BY [SALES],[CLIENTS],[PROJECTS],[EDAYS]
                              
                            ", QUERYS.ToString(), RadDatePicker3.SelectedDate.Value.ToString("yyyyMMdd"), RadDatePicker4.SelectedDate.Value.ToString("yyyyMMdd"));







        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "明細清單" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());


                //預設行高
                ws.DefaultRowHeight = 20;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "編號";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 2].Value = "業務員";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 3].Value = "類別";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 4].Value = "客戶";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 5].Value = "進度內容";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 6].Value = "起始日";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                ws.Cells[1, 7].Value = "結案日";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //               

                ws.Cells[1, 8].Value = "是否結案";
                ws.Cells[1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["ID"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 2].Value = od["SALES"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 3].Value = od["KINDS"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 4].Value = od["CLIENTS"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 5].Value = od["COMMENTS"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線      

                    ws.Cells[ROWS, 6].Value = od["SDAYS"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 7].Value = od["EDAYS"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 8].Value = od["ISCLOSE"].ToString();
                    ws.Cells[ROWS, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線


                    ROWS++;

                }




                ////預設列寬、行高
                ws.DefaultColWidth = 20; //預設列寬
                ws.DefaultRowHeight = 20; //預設行高
                ws.Cells.Style.ShrinkToFit = true;//单元格自动适应大小


                //// 遇\n或(char)10自動斷行
                ws.Cells.Style.WrapText = true;

                //自適應寬度設定
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //自適應高度設定        
                //ws.Row(1).CustomHeight = true;



                //儲存Excel
                //Byte[] bin = excel.GetAsByteArray();
                //File.WriteAllBytes(@"C:\TEMP\" + fileName, bin);

                //儲存和歸來的Excel檔案作為一個ByteArray
                var data = excel.GetAsByteArray();
                HttpResponse response = HttpContext.Current.Response;
                Response.Clear();

                //輸出標頭檔案　　
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + "");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data);
                Response.Flush();
                Response.End();
                //package.Save();//這個方法是直接下載到本地
            }
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
            //                                                            // 沒設置的話會跳出 Please set the excelpackage.licensecontext property


            ////var file = new FileInfo(fileName);
            //using (var excel = new ExcelPackage(file))
            //{

            //}
        }

    }
    public void UPDATETBSALESEVENTS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        try
        {
            string cmdTxt = @"  
                            UPDATE  [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            SET [ISCLOSE]='Y'
                            WHERE [ID]=@ID
                            ";



            m_db.AddParameter("@ID", ID);
   



            m_db.ExecuteNonQuery(cmdTxt);

            //顯示訊息
            ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "alert('結案完成')", true);
        }
        catch
        {
            //顯示訊息
            ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "alert('結案失敗')", true);
          
        }
        finally
        {

        }

        BindGrid();
    }

    public void ADDtb_NOTE()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        try
        {

            //先針對TBSALESEVENTSCOMMENTS補新增到tb_NOTE
            //再針對TBSALESEVENTS補新增到tb_NOTE，過濾已在TBSALESEVENTSCOMMENTS有資料的
            string cmdTxt = @"  
                          INSERT INTO [192.168.1.223]. [HJ_BM_DB].[dbo].[tb_NOTE]
                            (
                            [NOTE_CONTENT]
                            ,[NOTE_KIND]
                            ,[FILE_NAME]
                            ,[NOTE_DATE]
                            ,[NOTE_TIME]
                            ,[UPDATE_DATETIME]
                            ,[CONTACT_ID]
                            ,[COMPANY_ID]
                            ,[OPPORTUNITY_ID]
                            ,[SALES_STAGE]
                            ,[CREATE_DATETIME]
                            ,[CREATE_USER_ID]
                            ,[TBSALESEVENTSID]
                            ,[TBSALESEVENTSCOMMENTSID]
                            )
                            SELECT 
                            [PROJECTS]+'<br>'+[EVENTS]++'<br> 拜訪日:'+[EDAYS]+'<br>'+[TBSALESEVENTSCOMMENTS].[COMMENTS] AS [NOTE_CONTENT]
                            ,'1' AS [NOTE_KIND]
                            ,[TBSALESEVENTSCOMMENTS].[FILENAME] AS [FILE_NAME]
                            ,[EDAYS] AS [NOTE_DATE]
                            ,'00:00:00' AS [NOTE_TIME]
                            ,[TBSALESEVENTSCOMMENTS].[ADDDATES] AS [UPDATE_DATETIME]
                            ,'0' AS [CONTACT_ID]
                            ,[tb_COMPANY].[COMPANY_ID] AS [COMPANY_ID]
                            ,'0' AS [OPPORTUNITY_ID]
                            , NULL AS [SALES_STAGE]
                            ,[TBSALESEVENTSCOMMENTS].[ADDDATES] AS [CREATE_DATETIME]
                            ,[tb_COMPANY].[OWNER_ID] AS [CREATE_USER_ID]
                            ,[TBSALESEVENTSCOMMENTS].MID AS [TBSALESEVENTSID]
                            ,[TBSALESEVENTSCOMMENTS].ID AS [TBSALESEVENTSCOMMENTSID]

                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTSCOMMENTS],[TKBUSINESS].[dbo].[TBSALESEVENTS]
                            LEFT JOIN [192.168.1.223]. [HJ_BM_DB].[dbo].[tb_COMPANY] ON [COMPANY_NAME]=[CLIENTS]
                            WHERE [TBSALESEVENTS].ID=[TBSALESEVENTSCOMMENTS].MID
                            AND CONVERT(NVARCHAR,[tb_COMPANY].[COMPANY_ID])+CONVERT(NVARCHAR,[TBSALESEVENTSCOMMENTS].MID)+CONVERT(NVARCHAR,[TBSALESEVENTSCOMMENTS].ID) NOT IN (SELECT CONVERT(NVARCHAR,[COMPANY_ID])+[TBSALESEVENTSID]+[TBSALESEVENTSCOMMENTSID] FROM [192.168.1.223].[HJ_BM_DB].[dbo].[tb_NOTE] WHERE ISNULL([TBSALESEVENTSID]+[TBSALESEVENTSCOMMENTSID],'')<>'')

                            INSERT INTO [192.168.1.223]. [HJ_BM_DB].[dbo].[tb_NOTE]
                            (
                            [NOTE_CONTENT]
                            ,[NOTE_KIND]
                            ,[FILE_NAME]
                            ,[NOTE_DATE]
                            ,[NOTE_TIME]
                            ,[UPDATE_DATETIME]
                            ,[CONTACT_ID]
                            ,[COMPANY_ID]
                            ,[OPPORTUNITY_ID]
                            ,[SALES_STAGE]
                            ,[CREATE_DATETIME]
                            ,[CREATE_USER_ID]
                            ,[TBSALESEVENTSID]
                            ,[TBSALESEVENTSCOMMENTSID]
                            )
                            SELECT 
                            [PROJECTS]+'<br>'+[EVENTS]++'<br> 拜訪日:'+[EDAYS]+'<br>'+[COMMENTS] AS [NOTE_CONTENT]
                            ,'1' AS [NOTE_KIND]
                            ,[FILENAME] AS [FILE_NAME]
                            ,[EDAYS] AS [NOTE_DATE]
                            ,'00:00:00' AS [NOTE_TIME]
                            ,[ADDDATES] AS [UPDATE_DATETIME]
                            ,'0' AS [CONTACT_ID]
                            ,[tb_COMPANY].[COMPANY_ID] AS [COMPANY_ID]
                            ,'0' AS [OPPORTUNITY_ID]
                            , NULL AS [SALES_STAGE]
                            ,[ADDDATES] AS [CREATE_DATETIME]
                            ,[tb_COMPANY].[OWNER_ID] AS [CREATE_USER_ID]
                            ,ID AS [TBSALESEVENTSID]
                            ,'' AS [TBSALESEVENTSCOMMENTSID]

                            FROM [TKBUSINESS].[dbo].[TBSALESEVENTS]
                            LEFT JOIN [192.168.1.223]. [HJ_BM_DB].[dbo].[tb_COMPANY] ON [COMPANY_NAME]=[CLIENTS]
                            WHERE [tb_COMPANY].[COMPANY_ID]+CONVERT(NVARCHAR,[TBSALESEVENTS].[ID])  NOT IN (SELECT [COMPANY_ID]+[TBSALESEVENTSID] FROM [192.168.1.223].[HJ_BM_DB].[dbo].[tb_NOTE] WHERE ISNULL([TBSALESEVENTSID],'')<>'')
                            AND CONVERT(NVARCHAR,[TBSALESEVENTS].[ID])  NOT IN (SELECT MID FROM [TKBUSINESS].[dbo].[TBSALESEVENTSCOMMENTS])



                            ";



           //m_db.AddParameter("@ID", ID);
            m_db.ExecuteNonQuery(cmdTxt);

            //顯示訊息
            ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "alert('完成')", true);
        }
        catch
        {
            //顯示訊息
            ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "alert('失敗')", true);

        }
        finally
        {

        }
    }

    #endregion

    #region BUTTON

    protected void Button4_Click(object sender, EventArgs e)
    {
        BindGrid2();
    }

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
        BindGrid();
        //Response.Write("<script>alert('已儲存')</script>");
        //this.Session["SDATE"] = txtDate1.Text.Trim();
        //this.Session["EDATE"] = txtDate2.Text.Trim();
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        SETEXCEL();
    }
    protected void btn3_Click(object sender, EventArgs e)
    {
        BindGrid3();
    }
    protected void MyButtonClick(object sender, System.EventArgs e)
    {
      

    }

    protected void Button5_OnClick(object sender, EventArgs e)
    {

        ADDtb_NOTE();
    }
    protected void Button6_Click(object sender, EventArgs e)
    {

        BindGridOVER();
    }

    protected void Button7_Click(object sender, EventArgs e)
    {

        BindGridIN();
    }
    #endregion
}