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

public partial class CDS_WebPage_PUR_REPORT_BOM : Ede.Uof.Utility.Page.BasePage
{
    string CHECK_column1Value = "";
    string column1Value = "";
    // 全域變數，用於跨頁處理（可選）
    private string previousValue = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            BindGrid("");
        }
        else
        {

          

            //if (ViewState["TextBox1"] != null)
            //{
            //    TextBox1.Text = ViewState["TextBox1"].ToString();

            //}


            //if (this.Session["STATUS"] != null)
            //{
            //    DropDownList1.SelectedItem.Text = this.Session["STATUS"].ToString();

            //}

        }

       


    }
    #region FUNCTION
    private void BindDropDownList()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("SALESFOCUS", typeof(String));


        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @" SELECT '全部' AS SALESFOCUS UNION ALL SELECT SALESFOCUS  FROM  [TKBUSINESS].[dbo].[PRODUCTS]  GROUP BY SALESFOCUS ";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    DropDownList1.DataSource = dt;
        //    DropDownList1.DataTextField = "SALESFOCUS";
        //    DropDownList1.DataValueField = "SALESFOCUS";
        //    DropDownList1.DataBind();

        //}
        //else
        //{

        //}



    }
    private void BindGrid(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();



        //TextBox1
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND (MB001 LIKE '%'+@MB001+'%' OR MB002 LIKE '%'+@MB001+'%')");
        }


        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            cmdTxt.AppendFormat(@"
                                SELECT 
                                MB001 '品號'
                                ,MB002 '品名'
                                ,MB003 '規格'                               
                                FROM [TK].dbo.INVMB
                                WHERE 1=1
                                AND (MB001 LIKE '3%' OR MB001 LIKE '4%' OR  MB001 LIKE '5%' )
                                {0}
                                ORDER BY MB001
        


                             ", QUERYS.ToString());
        }
        else
        {
            cmdTxt.AppendFormat(@"
                              
                             ");
        }



        m_db.AddParameter("@MB001", TextBox1.Text.Trim());


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
            ////Get the button that raised the event
            //Button btn = (Button)e.Row.FindControl("GVButton1");

            ////Get the row that contains this button
            //GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            ////string cellvalue = gvr.Cells[2].Text.Trim();
            //string Cellvalue = btn.CommandArgument;

            //DataRowView row = (DataRowView)e.Row.DataItem;
            //Button lbtnName = (Button)e.Row.FindControl("GVButton1");

            //ExpandoObject param = new { ID = Cellvalue }.ToExpando();

            ////Grid開窗是用RowDataBound事件再開窗
            ////Dialog.Open2(lbtnName, "~/CDS/WebPage/STOCK/COPTGTHDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
           
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

    private void BindGrid2(string MB001)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        cmdTxt.AppendFormat(@"
                            WITH RecursiveCTE AS (
                                -- 基本查詢，選擇起始點
                                SELECT  1 AS Level, CAST('主品號' AS NVARCHAR) AS 品號,MD001,MD003,MD006,MD007,MD008,MC004,CAST((((MD006*(1+MD008))/MD007)/MC004) AS FLOAT) AS USED,CAST( 1 AS FLOAT) AS LASTUSED
                                FROM [TK].dbo.BOMMD -- 請替換YourDatabaseName為實際的數據庫名稱
	                            INNER JOIN [TK].dbo.BOMMC ON MC001=MD001
	                            INNER JOIN [TK].dbo.INVMB ON MB001=MC001
                                WHERE (INVMB.MB001 LIKE  '%{0}%' OR INVMB.MB002 LIKE '%{0}%'  )
	
                                UNION ALL

                                -- 遞迴查詢，選擇下一級
                                SELECT  R.Level + 1,CAST('明細' AS NVARCHAR) AS 品號,D.MD001, D.MD003,D.MD006,D.MD007,D.MD008,C.MC004,CAST((((D.MD006*(1+D.MD008))/D.MD007)/C.MC004) AS FLOAT)*CAST( 1 AS FLOAT)*R.USED AS USED,R.USED AS LASTUSED
                                FROM [TK].dbo.BOMMD D
	                            INNER JOIN [TK].dbo.BOMMC C ON C.MC001=D.MD001
                                INNER JOIN RecursiveCTE R ON R.MD003 = D.MD001
                            )
                            -- 最終查詢，選擇遞迴結果
                            SELECT RecursiveCTE.*
                            ,MB1.MB002 MAINMB002,MB1.MB003 MAINMB003,MB1.MB004 MAINMB004
                            ,MB2.MB002 DMB002,MB2.MB003 DMB003,MB2.MB004 DMB004
                            ,CONVERT(DECIMAL(16,4),RecursiveCTE.USED) AS NEWUSED                           
                            ,(CASE WHEN Level=1 THEN CONVERT(DECIMAL(16,0),RecursiveCTE.LASTUSED) ELSE  CONVERT(DECIMAL(16,4),RecursiveCTE.LASTUSED) END )AS NEWLASTUSED

                            FROM RecursiveCTE
                            LEFT JOIN [TK].dbo.INVMB MB1 ON MB1.MB001=RecursiveCTE.MD001
                            LEFT JOIN [TK].dbo.INVMB MB2 ON MB2.MB001=RecursiveCTE.MD003
                            ORDER BY RecursiveCTE.Level;  -- 按遞迴的層級排序


                            ", MB001);



       // m_db.AddParameter("@MB001", MB001);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 取得資料綁定到 GridView 的資料物件
            DataRowView dataItem = (DataRowView)e.Row.DataItem;

            // 把整數1.000改成1
            if(e.Row.Cells[4].Text.Equals("1.0000"))
            {
                e.Row.Cells[4].Text = string.Format("{0:#}", DataBinder.Eval(e.Row.DataItem, "NEWLASTUSED"));
            }
            else
            {
                e.Row.Cells[4].Text = string.Format("{0:F4}", DataBinder.Eval(e.Row.DataItem, "NEWLASTUSED"));
            }


            // 假設您要根據某個欄位的值來判斷是否顯示 Column2 的值
            column1Value = dataItem["MD001"].ToString();

            // 在這裡設定 Column2 的顯示值，您可以根據特定條件決定是否留空白
            if (CHECK_column1Value.Equals(column1Value))
            {
                // 留空白
                e.Row.Cells[1].Text = string.Empty;
                e.Row.Cells[2].Text = string.Empty;
                e.Row.Cells[3].Text = string.Empty;
                e.Row.Cells[4].Text = string.Empty;
                e.Row.Cells[5].Text = string.Empty;
            }
            else
            {
                CHECK_column1Value = column1Value;
            }
        }
    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
       
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

       
       cmdTxt.AppendFormat(@" 
                             
                                ", QUERYS.ToString());

        //string cmdTxt = @" 
        //                SELECT [PRODUCTS].[MB001],[PRODUCTSFEATURES],[SALESFOCUS],[COPYWRITINGS],[PICPATHS]
        //                ,MB002,MB003,MB004,MA003,ISNULL(MD007,0) AS MD007,CONVERT(NVARCHAR,MB023)+(CASE WHEN MB198='1' THEN '天' ELSE (CASE WHEN MB198='2' THEN '月' ELSE '年' END ) END ) AS 'VALIDITYPERIOD',CONVERT(decimal(16,3),ISNULL(MB047,0)) AS MB047,MB013
        //                ,[ALBUM_GUID], [PHOTO_GUID],[PHOTO_DESC],[FILE_ID],[RESIZE_FILE_ID],[THUMBNAIL_FILE_ID]
        //                FROM [TKBUSINESS].[dbo].[PRODUCTS]
        //                LEFT JOIN [TK].dbo.[INVMB] ON [PRODUCTS].[MB001]=[INVMB].[MB001]
        //                LEFT JOIN [TK].dbo.INVMA ON MA001='9' AND MA002=MB115
        //                LEFT JOIN [TK].dbo.BOMMD ON MD001=[INVMB].[MB001] AND MD003 LIKE '201%'
        //                LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EIP_ALBUM_PHOTO] ON [PHOTO_DESC] LIKE '%'+[PRODUCTS].[MB001]+'%' COLLATE Chinese_Taiwan_Stroke_BIN
        //                ORDER BY [PRODUCTS].[MB001]
        //                ";

        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if(dt.Rows.Count>0)
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
                   
                   

                    ROWS++;
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

    public void SEARCH_BOMMCBOMMD(string  MB001)
    {
       
        Label3.Text = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
    }

    private void BindGrid3(string MB001,string DMB001,string PERCENTS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();



        //TextBox1
        if (!string.IsNullOrEmpty(MB001))
        {
            QUERYS.AppendFormat(@" AND MD003 LIKE '%{0}%' OR DINVMB.MB002 LIKE '%{0}%'", MB001);
        }


        if (!string.IsNullOrEmpty(MB001))
        {
            cmdTxt.AppendFormat(@"
                                --20240110 成本
                                --不管月結成本
                                --採購件=直接用單價進價*用量佔比
                                --半成品=最近的月結成本*用量佔比
                                WITH CTE_FILTERED_MC AS (
                                SELECT DISTINCT 
                                MC001
                                FROM [TK].dbo.BOMMC
                                JOIN [TK].dbo.BOMMD ON MC001 = MD001
                                JOIN [TK].dbo.INVMB AS MINVMB ON MC001 = MINVMB.MB001
                                JOIN [TK].dbo.INVMB AS DINVMB ON MD003 = DINVMB.MB001
                                WHERE 1=1
                                {0}
                                ),
                                CTE_CSTME AS (
                                SELECT 
                                ME001,
                                ME003,
                                ME004,
                                ME007,
                                ME008,
                                ME009,
                                ME010,
                                ROW_NUMBER() OVER (PARTITION BY ME001 ORDER BY ME002 DESC) AS RN
                                FROM [TK].dbo.CSTME
                                )

                                SELECT *
                                ,(CASE WHEN 明細品號='{1}' THEN 明細材料成本*{2} ELSE 明細材料成本 END) AS '調整後的明細材料成本'
                                ,((CASE WHEN 明細品號='{1}' THEN 明細材料成本*{2} ELSE 明細材料成本 END)-明細材料成本) AS '影響成本'
                                FROM 
                                (
	                                SELECT *
	                                ,CONVERT(DECIMAL(16,3),(CASE WHEN 進價用量比成本>0 THEN 進價用量比成本 ELSE 最近成本用量比成本 END)) AS '明細材料成本'
	                                ,CONVERT(DECIMAL(16,3),SUM((CASE WHEN 進價用量比成本 > 0 THEN 進價用量比成本 ELSE 最近成本用量比成本 END)) OVER (PARTITION BY 主品號)) AS '合計明細材料成本'
	                                FROM
	                                (
		                                SELECT 
		                                MC.MC001 AS '主品號',
		                                MINVMB.MB002 AS '主品名',
		                                MINVMB.MB004 AS '主單位',
		                                MD.MD003  AS '明細品號',
		                                DINVMB.MB002  AS '明細品名',
		                                DINVMB.MB004  AS '明細單位',
		                                MC.MC004 AS '標準批量',
		                                MD.MD006 AS '使用量',
		                                MD.MD007 AS '底數',
		                                MD.MD008 AS '損秏率',
		                                CONVERT(DECIMAL(16,3),((MD.MD006/MD.MD007*(1+MD.MD008))/MC.MC004)) AS '明細用量比',
		                                --單個的材料、人工、製造、加工的成本
		                                CONVERT(DECIMAL(16,3),(CASE WHEN  TEMP.ME007>0 AND (ME003+ME004)>0 THEN  TEMP.ME007/(ME003+ME004) ELSE 0 END)) AS '單個材料',
		                                CONVERT(DECIMAL(16,3),(CASE WHEN  TEMP.ME008>0 AND (ME003+ME004)>0 THEN  TEMP.ME008/(ME003+ME004) ELSE 0 END)) AS '單個人工',
		                                CONVERT(DECIMAL(16,3),(CASE WHEN  TEMP.ME009>0 AND (ME003+ME004)>0 THEN  TEMP.ME009/(ME003+ME004) ELSE 0 END)) AS '單個製造',
		                                CONVERT(DECIMAL(16,3),(CASE WHEN  TEMP.ME010>0 AND (ME003+ME004)>0 THEN  TEMP.ME010/(ME003+ME004) ELSE 0 END)) AS '單個加工'
		                                ,DINVMB.MB050 AS '最近單位進價'	 
		                                ,(DINVMB.MB050 * CONVERT(DECIMAL(16,3),((MD.MD006/MD.MD007*(1+MD.MD008))/MC.MC004))) AS '進價用量比成本'	
		                                ,ISNULL((SELECT TOP 1 CONVERT(DECIMAL(16,3),( (CSTME.ME007+CSTME.ME008+CSTME.ME009+CSTME.ME010)/(CSTME.ME003+CSTME.ME004)) )
		                                FROM [TK].dbo.CSTME
		                                WHERE CSTME.ME001=MD.MD003 
		                                ORDER BY ME002 DESC
		                                )* CONVERT(DECIMAL(16,3),((MD.MD006/MD.MD007*(1+MD.MD008))/MC.MC004)),0) AS '最近成本用量比成本'
		                                FROM [TK].dbo.BOMMC AS MC
		                                JOIN [TK].dbo.BOMMD AS MD ON MC.MC001 = MD.MD001
		                                JOIN [TK].dbo.INVMB AS MINVMB ON MC.MC001 = MINVMB.MB001
		                                JOIN [TK].dbo.INVMB AS DINVMB ON MD.MD003 = DINVMB.MB001
		                                LEFT JOIN CTE_CSTME AS TEMP ON MC.MC001 = TEMP.ME001 AND TEMP.RN = 1
		                                WHERE MC.MC001 IN (SELECT MC001 FROM CTE_FILTERED_MC)
	                                ) AS TEMP1
	                                WHERE 1=1
                                ) AS TEMP2
                                WHERE 1=1
                                ORDER BY 主品號,明細品號
                                --AND MC.MC001='3010100215'
                                    ;


                             ", QUERYS.ToString(), DMB001,  PERCENTS);
        }
        else
        {
            cmdTxt.AppendFormat(@"
                              
                             ");
        }



        //m_db.AddParameter("@MB001", TextBox1.Text.Trim());


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid3.DataSource = dt;
        Grid3.DataBind();
    }

    protected void grid3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Grid3
        if (e.Row.RowType == DataControlRowType.DataRow)        {
            // 獲取當前行的目標欄位值（假設是第一欄）
            string currentValue = DataBinder.Eval(e.Row.DataItem, "主品號").ToString();
            // 檢查是否與上一行相同
            if (!string.IsNullOrEmpty(previousValue) && currentValue == previousValue)
            {
                // 設置當前行的該欄位為空白
                e.Row.Cells[0].Text = string.Empty;
                e.Row.Cells[1].Text = string.Empty;
                e.Row.Cells[2].Text = string.Empty;
                e.Row.Cells[3].Text = string.Empty;               
            }
            // 更新上一行的值
            previousValue = currentValue;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 獲取目標欄位的值
            string numberText = DataBinder.Eval(e.Row.DataItem, "影響成本").ToString();

            // 在 C# 5.0 中，需先宣告變數
            decimal numberValue;

            // 嘗試將目標值解析為數字
            if (decimal.TryParse(numberText, out numberValue) && numberValue != 0)
            {
                // 如果數值不為 0，將文字顏色設為紅色
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

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
        string DMB001 = "";
        string PERCENTS = "0";
        BindGrid3(TextBox2.Text.Trim(), DMB001, PERCENTS);
    }
        

    protected void btn5_Click(object sender, EventArgs e)
    {
        BindGrid("");

    }
    protected void GVButton1_Click(object sender, EventArgs e)
    {
        // 獲取按鈕控制項
        Button btn = (Button)sender;

        // 獲取 GridView 的行 (Row)
        GridViewRow row = (GridViewRow)btn.NamingContainer;

        // 獲取行中某個單元格 (Cell) 的值，例如第一個單元格的值
        string cellValue = row.Cells[0].Text; // 假設您想獲取第一個單元格的值

        // 在這裡您可以處理獲取的值，例如顯示在標籤或執行其他操作
        // Label1.Text = cellValue;
        // 其他操作...

        Label3.Text = cellValue;
        BindGrid2(cellValue);
    }
    protected void GV3Button1_Click(object sender, EventArgs e)
    {
        // 獲取按鈕控制項
        Button btn = (Button)sender;

        // 獲取 GridView 的行 (Row)
        GridViewRow row = (GridViewRow)btn.NamingContainer;

        // 獲取行中某個單元格 (Cell) 的值，例如第一個單元格的值
        string DMB001 = row.Cells[4].Text; // 假設您想獲取第一個單元格的值
        string DMB002 = row.Cells[5].Text; // 假設您想獲取第一個單元格的值
                                           // 找到 TextBox 控制項並獲取它的值
        TextBox txt單個成本 = (TextBox)row.FindControl("txt單個成本");
        string PERCENTS = "0";

        if (txt單個成本 != null)
        {
            string SETPERCENTS = txt單個成本.Text;

            // 計算百分比
            decimal percentage = 1 + Convert.ToDecimal(SETPERCENTS) / 100;
            PERCENTS = percentage.ToString(); // 格式化為百分比形式

            // 更新 Label 的文字
            Label5.Text = "品號: "+DMB001 +" 品名: "+ DMB002 + " 調整後百分比" + PERCENTS;
        }
        else
        {
            Label5.Text = "無法找到 txt單個成本 控制項";
        }
        // 在這裡您可以處理獲取的值，例如顯示在標籤或執行其他操作
        // Label1.Text = cellValue;
        // 其他操作...      
      
        BindGrid3(TextBox2.Text.Trim(), DMB001, PERCENTS);    
    }

    #endregion
}