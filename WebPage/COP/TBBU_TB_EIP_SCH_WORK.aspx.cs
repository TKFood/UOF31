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

public partial class CDS_WebPage_COP_TBBU_PRODUCTS : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDownList();

            BindGrid("");
        }
        else
        {

            BindGrid("");

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
        DataTable dt = new DataTable();
        DataRow row;
        dt.Columns.Add("KINDS", typeof(String));

        row = dt.NewRow();
        row["KINDS"] = "N";
        dt.Rows.Add(row);

        row = dt.NewRow();
        row["KINDS"] = "Y";
        dt.Rows.Add(row);



        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @" SELECT '全部' AS SALESFOCUS UNION ALL SELECT SALESFOCUS  FROM  [TKBUSINESS].[dbo].[PRODUCTS]  GROUP BY SALESFOCUS ";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "KINDS";
            DropDownList1.DataValueField = "KINDS";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }
    private void BindGrid(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

      

        //狀態
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("Y"))
            {
                QUERYS.AppendFormat(@" AND WORK_STATE  IN ('Completed') ");
            }
            else if(DropDownList1.Text.Equals("N"))
            {
                QUERYS.AppendFormat(@"  AND WORK_STATE NOT IN ('Completed') ");
            }
        }

        //校稿名稱
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND SUBJECT LIKE  '%'+@SUBJECT+'%' ");
        
        }

        cmdTxt.AppendFormat(@" 
                            SELECT SUBJECT,CASE WHEN WORK_STATE='Completed' THEN '已完成'  WHEN WORK_STATE='NotYetBegin' THEN '尚未開始' WHEN WORK_STATE='Audit' THEN '交付人審查中' WHEN WORK_STATE='Proceeding' THEN '進行中' ELSE WORK_STATE END  WORK_STATE
                            ,USER1.NAME AS '交付者',USER2.NAME AS '執行者',CONVERT(nvarchar,CREATE_TIME,112 ) CREATE_TIME,CONVERT(nvarchar,END_TIME,112 ) END_TIME
                            ,(SELECT TOP 1 DESCRIPTION FROM  [UOF].dbo.TB_EIP_SCH_WORK_RECORD WHERE TB_EIP_SCH_WORK_RECORD.WORK_GUID=TB_EIP_SCH_WORK.WORK_GUID ORDER BY CREATE_TIME DESC) AS 'DESCRIPTION'
                            ,WORK_GUID,CREATE_USER,EXECUTE_USER
                            FROM [UOF].dbo.TB_EIP_SCH_WORK
                            LEFT JOIN [UOF].dbo.TB_EB_USER USER1 ON USER1.USER_GUID=CREATE_USER
                            LEFT JOIN [UOF].dbo.TB_EB_USER USER2 ON USER2.USER_GUID=EXECUTE_USER
                            WHERE 1=1
                            AND EXECUTE_USER IN 
                            (
                            SELECT
                            UserId.value('(.)', 'nvarchar(50)') AS UserId
                            FROM
                            [UOF].dbo.TB_EIP_SCH_DEVOLVE
                            CROSS APPLY
                            USER_SET.nodes('/UserSet/Element') AS UserSet(Element)
                            CROSS APPLY
                            Element.nodes('userId') AS UserId(UserId)
                            WHERE
                            TB_EIP_SCH_DEVOLVE.DEVOLVE_GUID=TB_EIP_SCH_WORK.DEVOLVE_GUID
                            )
                                {0}
                            ORDER BY CREATE_TIME DESC
                             
                              
                                ", QUERYS.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        m_db.AddParameter("@SUBJECT", TextBox1.Text);

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




        //StringBuilder PATH = new StringBuilder();

        //System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");



        //    if (!string.IsNullOrEmpty(row["PHOTO_GUID"].ToString()))
        //    {
        //        //img.ImageUrl = "https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id=8b2a033b-c301-419b-938d-e6cfedf28b82&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name=40100010650490.png";


        //        //PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id={0}&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name={1}
        //        //                ", row["RESIZE_FILE_ID"].ToString(), row["PHOTO_DESC"].ToString());

        //        PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/Common/FileCenter/V3/Handler/FileControlHandler.ashx?id={0}
        //                        ", row["RESIZE_FILE_ID"].ToString());
        //        img.ImageUrl = PATH.ToString();

        //        //img.ImageUrl  = Request.ApplicationPath + "/Common/FileCenter/ShowImage.aspx?id=" + row["THUMBNAIL_FILE_ID"].ToString();

        //        //img.ImageUrl = string.Format("~/Common/FileCenter/Downloadfile.ashx?id={0}", row["THUMBNAIL_FILE_ID"].ToString());

        //        //e.Row.Cells[0].Text = row["THUMBNAIL_FILE_ID"].ToString();
        //        ////獲取當前行的圖片路徑
        //        //string ImgUrl = img.ImageUrl;
        //        ////給帶圖片的單元格添加點擊事件
        //        //e.Row.Cells[3].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
        //        //    + ".checked=true;CellClick('" + ImgUrl + "')");

        //        //  img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
        //    }


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
            var fileName = "資料明細" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
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


        //if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        //{
        //    if (Dialog.GetReturnValue().Equals("NeedPostBack"))
        //    {

        //    }

        //}
    }
    #endregion
}