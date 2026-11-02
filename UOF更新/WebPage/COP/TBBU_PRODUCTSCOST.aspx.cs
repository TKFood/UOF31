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
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

public partial class CDS_WebPage_COP_TBBU_PRODUCTSCOST : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindGrid("");

            txtDate1.SelectedDate = DateTime.Now.AddMonths(-1);
            txtDate2.SelectedDate = DateTime.Now.AddMonths(-1);

        }
        else
        {
            BindGrid("");
        }

       

        if (this.Session["STATUS"] != null)
        {
           

        }


    }
    #region FUNCTION
   
    private void BindGrid(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();


        QUERYS.AppendFormat(@" ");

        

        if (!string.IsNullOrEmpty(TextBox1.Text) )
        {
            cmdTxt.AppendFormat(@" 
                                SELECT ME002,ME001,MB002,材料成本之單位成本,人工成本之單位成本,製造費用之單位成本,加工費用之單位成本,單位成本
                                FROM (
                                SELECT 
                                ME002,ME001,MB002
                                ,CASE WHEN (ME007>0 AND (ME003+ME004+ME005)>0 ) THEN CONVERT(DECIMAL(16,4),ME007/(ME003+ME004+ME005)) ELSE 0 END AS '材料成本之單位成本'
                                ,CASE WHEN (ME008>0 AND (ME003+ME004+ME006)>0 ) THEN CONVERT(DECIMAL(16,4),ME008/(ME003+ME004+ME006)) ELSE 0 END AS '人工成本之單位成本'
                                ,CASE WHEN (ME009>0 AND (ME003+ME004+ME006)>0 ) THEN CONVERT(DECIMAL(16,4),ME009/(ME003+ME004+ME006)) ELSE 0 END AS '製造費用之單位成本'
                                ,CASE WHEN (ME010>0 AND (ME003+ME004+ME014)>0 ) THEN CONVERT(DECIMAL(16,4),ME010/(ME003+ME004+ME014)) ELSE 0 END AS '加工費用之單位成本'
                                ,((CASE WHEN (ME007>0 AND (ME003+ME004+ME005)>0 ) THEN CONVERT(DECIMAL(16,4),ME007/(ME003+ME004+ME005)) ELSE 0 END)+(CASE WHEN (ME008>0 AND (ME003+ME004+ME006)>0 ) THEN CONVERT(DECIMAL(16,4),ME008/(ME003+ME004+ME006)) ELSE 0 END)+(CASE WHEN (ME009>0 AND (ME003+ME004+ME006)>0 ) THEN CONVERT(DECIMAL(16,4),ME009/(ME003+ME004+ME006)) ELSE 0 END)+(CASE WHEN (ME010>0 AND (ME003+ME004+ME014)>0 ) THEN CONVERT(DECIMAL(16,4),ME010/(ME003+ME004+ME014)) ELSE 0 END)) AS '單位成本'
                                FROM [TK].dbo.CSTME,[TK].dbo.INVMB
                                WHERE ME001=MB001
                                AND ( ME001 LIKE '4%' OR ME001 LIKE '5%')
                                AND ME002>='{1}'  AND ME002<='{2}'  
                                AND (MB002 LIKE '%{0}%')
                                UNION ALL
                                SELECT '平均',ME001,MB002
                                ,AVG(CASE WHEN (ME007>0 AND (ME003+ME004+ME005)>0 ) THEN CONVERT(DECIMAL(16,4),ME007/(ME003+ME004+ME005)) ELSE 0 END) AS '材料成本之單位成本'
                                ,AVG(CASE WHEN (ME008>0 AND (ME003+ME004+ME006)>0 ) THEN CONVERT(DECIMAL(16,4),ME008/(ME003+ME004+ME006)) ELSE 0 END) AS '人工成本之單位成本'
                                ,AVG(CASE WHEN (ME009>0 AND (ME003+ME004+ME006)>0 ) THEN CONVERT(DECIMAL(16,4),ME009/(ME003+ME004+ME006)) ELSE 0 END) AS '製造費用之單位成本'
                                ,AVG(CASE WHEN (ME010>0 AND (ME003+ME004+ME014)>0 ) THEN CONVERT(DECIMAL(16,4),ME010/(ME003+ME004+ME014)) ELSE 0 END) AS '加工費用之單位成本'
                                ,AVG((CASE WHEN (ME007>0 AND (ME003+ME004+ME005)>0 ) THEN CONVERT(DECIMAL(16,4),ME007/(ME003+ME004+ME005)) ELSE 0 END)+(CASE WHEN (ME008>0 AND (ME003+ME004+ME006)>0 ) THEN CONVERT(DECIMAL(16,4),ME008/(ME003+ME004+ME006)) ELSE 0 END)+(CASE WHEN (ME009>0 AND (ME003+ME004+ME006)>0 ) THEN CONVERT(DECIMAL(16,4),ME009/(ME003+ME004+ME006)) ELSE 0 END)+(CASE WHEN (ME010>0 AND (ME003+ME004+ME014)>0 ) THEN CONVERT(DECIMAL(16,4),ME010/(ME003+ME004+ME014)) ELSE 0 END)) AS '單位成本'
                                FROM [TK].dbo.CSTME,[TK].dbo.INVMB
                                WHERE ME001=MB001
                                AND ( ME001 LIKE '4%' OR ME001 LIKE '5%')
                                AND ME002>='{1}'  AND ME002<='{2}'  
                                AND (MB002 LIKE '%{0}%')
                                GROUP BY ME001,MB002
                                ) AS TEMP
                                ORDER BY ME001,ME002
                                ", TextBox1.Text.Trim(),txtDate1.SelectedDate.Value.ToString("yyyyMM"), txtDate2.SelectedDate.Value.ToString("yyyyMM"));
        }
        else
        {
            cmdTxt.AppendFormat(@" ");
        }




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
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    //Get the button that raised the event
        //    Button btn = (Button)e.Row.FindControl("Button1");

        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //    //string cellvalue = gvr.Cells[2].Text.Trim();
        //    string Cellvalue = btn.CommandArgument;

        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    Button lbtnName = (Button)e.Row.FindControl("Button1");

        //    ExpandoObject param = new { ID = Cellvalue }.ToExpando();

        //    //Grid開窗是用RowDataBound事件再開窗
        //    Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_PRODUCTSDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //}



    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
       

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

    //private void AddImage(ExcelWorksheet oSheet, int rowIndex, int colIndex, string imagePath)
    //{
    //    Bitmap image = new Bitmap(imagePath);
    //    ExcelPicture excelImage = null;
    //    if (image != null)
    //    {
    //        excelImage = oSheet.Drawings.AddPicture("Debopam Pal", image);
    //        excelImage.From.Column = colIndex;
    //        excelImage.From.Row = rowIndex;
    //        excelImage.SetSize(100, 100);
    //        //2x2 px space for better alignment
    //        excelImage.From.ColumnOff = Pixel2MTU(2);
    //        excelImage.From.RowOff = Pixel2MTU(2);
    //    }
    //}

    //public int Pixel2MTU(int pixels)
    //{
    //    int mtus = pixels * 9525;
    //    return mtus;
    //}

    public override void VerifyRenderingInServerForm(Control control) 
    { 

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

    protected void btn2_Click(object sender, EventArgs e)
    {
        
    }
    protected void btn3_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=test.xls");
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
        HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=big5>");
        HttpContext.Current.Response.Write("<head><meta http-equiv=Content-Type content=text/html;charset=big5></head>");
        Response.Charset = "big5";
        Response.ContentType = "application/excel";


        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        Grid1.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
        protected void MyButtonClick(object sender, System.EventArgs e)
    {
      

    }

    protected void btn5_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            if (Dialog.GetReturnValue().Equals("NeedPostBack"))
            {
                
            }

        }
    }
    #endregion
}