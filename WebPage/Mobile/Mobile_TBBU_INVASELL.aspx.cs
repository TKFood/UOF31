using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

public partial class CDS_WebPage_Mobile_TBBU_INVASELL : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //不顯示子視窗的按鈕
        ((Master_MobileMasterPage)this.Master).Button1Text = string.Empty;
        ((Master_MobileMasterPage)this.Master).Button2Text = string.Empty;
        ((Master_MobileMasterPage)this.Master).Button3Text = string.Empty;

        if (!IsPostBack)
        { 
            //BindGrid("");
        }
        else
        {

        }
    }

    #region FUNCTION   
    private void BindGrid(string SALESFOCUS)
    {
        int Year = DateTime.Now.Year; // 設定要查詢的年份
        int Months = DateTime.Now.Month; // 設定要查詢的月份
        int Days = DateTime.Now.Day; // 設定要查詢的天數
        DateTime firstDayOfYear = new DateTime(Year, Months, 1);

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();


        cmdTxt.AppendFormat(@" 
                               DECLARE @SDAY nvarchar(10)
                                DECLARE @TOTALDAYS INT
                                SET @SDAY='{0}'
                                SET @TOTALDAYS={1}

                                SELECT LA001 AS '品號',INVMB.MB002 AS '品名',LA016 AS '批號',CONVERT(INT,NUMS) AS '庫存量',有效日期,製造日期,CONVERT(INT,總銷售數量) 總銷售數量,CONVERT(INT,平均天銷售數量) 平均天銷售數量,CONVERT(INT,預計銷售天) 預計銷售天,預計完銷日
                                ,DATEDIFF (MONTH,製造日期,預計完銷日) AS '生產到完銷的月數'
                                ,CONVERT(INT,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30001'))) AS '中山一店'
                                ,CONVERT(INT,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30002'))) AS '概念二店'
                                ,CONVERT(INT,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30003'))) AS '北港三店'
                                ,CONVERT(INT,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30004'))) AS '站前四店'
                                ,CONVERT(INT,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30012'))) AS '微風北車店'
                                ,CONVERT(INT,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30017'))) AS '大潤發中崙店'


                                ,CONVERT(INT,ISNULL(MB047,0)) AS '售價'
                                ,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30001'))*ISNULL(MB051,0) AS '中山一店可銷貨金額'
                                ,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30002'))*ISNULL(MB051,0) AS '概念二店可銷貨金額'
                                ,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30003'))*ISNULL(MB051,0) AS '北港三店可銷貨金額'
                                ,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30004'))*ISNULL(MB051,0) AS '站前四店可銷貨金額'
                                ,(SELECT ISNULL(SUM(LA005*LA011),0) FROM [TK].dbo.INVLA LA WITH (NOLOCK) WHERE LA.LA001=TEMP2.LA001 AND LA.LA016=TEMP2.LA016 AND LA009 IN ('30012'))*ISNULL(MB051,0) AS '微風北車店可銷貨金額'
                                ,@SDAY AS '銷售日起'
                                ,@TOTALDAYS  AS '銷售天數'
                                FROM (
                                SELECT LA001,MB002,LA016,NUMS,有效日期,製造日期,總銷售數量,平均天銷售數量,CASE WHEN 平均天銷售數量>0 THEN (NUMS/平均天銷售數量) ELSE -1 END '預計銷售天'
                                ,CASE WHEN 平均天銷售數量>0 THEN CONVERT(NVARCHAR,DATEADD(DAY,CEILING(NUMS/平均天銷售數量),GETDATE()),112) ELSE '' END AS '預計完銷日'

                                FROM (
                                SELECT LA001,MB002,LA016,SUM(LA005*LA011) AS 'NUMS'
                                ,CASE WHEN ISNULL((SELECT TOP 1 TG018 FROM [TK].dbo.MOCTF WITH (NOLOCK) ,[TK].dbo.MOCTG WITH (NOLOCK)  WHERE TF001=TG001 AND TF002=TG002 AND TG004=LA001 AND TG017=LA016 ORDER BY TG018 ),'')<>'' THEN (SELECT TOP 1 TG018 FROM [TK].dbo.MOCTF WITH (NOLOCK) ,[TK].dbo.MOCTG WITH (NOLOCK)  WHERE TF001=TG001 AND TF002=TG002 AND TG004=LA001 AND TG017=LA016 ORDER BY TG018 ) ELSE  LA016 END  AS '有效日期'
                                ,(SELECT TOP 1 TG040 FROM [TK].dbo.MOCTF WITH (NOLOCK) ,[TK].dbo.MOCTG WITH (NOLOCK) WHERE TF001=TG001 AND TF002=TG002 AND TG004=LA001 AND TG017=LA016 ORDER BY TG040 ) AS '製造日期'
                                ,(SELECT ISNULL(SUM(TB019),0) FROM [TK].dbo.POSTB WITH (NOLOCK) WHERE TB002 IN ('106501','106502','106503','106504','106513') AND TB010=LA001 AND TB001>=@SDAY) AS '總銷售數量'
                                ,(SELECT ISNULL(SUM(TB019),0) FROM [TK].dbo.POSTB WITH (NOLOCK) WHERE TB002 IN ('106501','106502','106503','106504','106513') AND TB010=LA001 AND TB001>=@SDAY)/@TOTALDAYS AS '平均天銷售數量'
                                FROM [TK].dbo.INVLA WITH (NOLOCK) ,[TK].dbo.INVMB WITH (NOLOCK) 
                                WHERE LA009 IN ('30001','30002','30003','30004','30012','30017')
                                AND LA001=MB001
                                AND (LA001 LIKE '4%' OR LA001 LIKE '5%')
                                AND LA016 LIKE '2%'
                                AND MB002 NOT LIKE '%試吃%'
                                GROUP BY LA001,MB002,LA016
                                HAVING SUM(LA005*LA011)>0

                                ) AS TEMP
                                ) AS TEMP2
                                LEFT JOIN [TK].dbo.INVMB ON MB001=LA001
                                WHERE INVMB.MB002 NOT LIKE '%暫停%'
                                ORDER BY LA001  
                                ", firstDayOfYear.ToString("yyyyMMdd"), Days);




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

    public void SETEXCEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        QUERYS.AppendFormat(@" ");

        cmdTxt.AppendFormat(@" 
                                
                                ");


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "清單" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());


                //預設行高
                ws.DefaultRowHeight = 60;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "品號";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
              
                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["MB001"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    

                }


                ////預設列寬、行高
                //sheet.DefaultColWidth = 10; //預設列寬
                //sheet.DefaultRowHeight = 30; //預設行高

                //// 遇\n或(char)10自動斷行
                //ws.Cells.Style.WrapText = true;

                //自適應寬度設定
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //自適應高度設定
                ws.Row(1).CustomHeight = true;



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
        SETEXCEL();
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
        BindGrid("");
    }
    #endregion
}