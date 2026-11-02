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
using System.Web.UI.HtmlControls;

public partial class CDS_WebPage_COP_TK_REPORTS_Mobile_SALES_RECORDSE : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate1.Text = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
            txtDate2.Text = DateTime.Now.ToString("yyyy/MM/dd");

            BindGrid(txtDate1.Text, txtDate2.Text);
        }
        else
        {

        }




    }
    #region FUNCTION

    private void BindGrid(string SDAYS, string EDAYS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();

        cmdTxt.AppendFormat(@" 
                            SELECT 
                            [ID]
                            ,CONVERT(nvarchar,[CREATDATES],111) AS '建立日期'
                            ,[SALESNAMES] AS '業務員'
                            ,[CLIENTSID] AS '客戶代號'
                            ,[CLIENTSNAMES] AS '客戶'
                            ,[NEWCLIENTSNAMES] AS '新客'
                            ,[KINDS] AS '拜訪目的'
                            ,[RECORDS] AS '訪談內容'
                            ,CONVERT(nvarchar,[RECORDSDATES],111) AS '訪談日期'
                            ,[PHOTOS]
                            ,[PHOTOSID]
                            FROM [TKBUSINESS].[dbo].[TB_SALES_RECORDS]
                            WHERE CONVERT(nvarchar,[RECORDSDATES],111)>='{0}' AND CONVERT(nvarchar,[RECORDSDATES],111)<='{1}'
                            ORDER BY [SALESNAMES],[CLIENTSNAMES],[ID]

                              
                            ", SDAYS, EDAYS);


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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 找到 Image 容器
            var imageContainer = (HtmlGenericControl)e.Row.FindControl("ImageContainer");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            DataTable dt = new DataTable();
            if (dr["PHOTOSID"] != DBNull.Value && dr["PHOTOSID"] != null)
            {
                dt = SEARCH_TB_SALES_RECORDS_PHOTOS(dr["PHOTOSID"].ToString());
                if (dt != null && dt.Rows.Count >= 1)
                {
                    // 迴圈處理每張圖片
                    foreach (DataRow imageRow in dt.Rows)
                    {
                        // 創建一個 Image 控制項
                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                        string imageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])imageRow["PHOTOS"]);
                        img.ImageUrl = imageUrl; // 假設你的資料表中有一個 "ImageUrl" 的欄位

                        // 設置寬度和高度
                        img.Width = Unit.Pixel(100);
                        img.Height = Unit.Pixel(100);
                        // 設置靠左對齊
                        img.Style.Add("float", "left");


                        // 將 Image 控制項加入到容器中
                        imageContainer.Controls.Add(img);
                    }
                }
            }

            //DataRowView dr = (DataRowView)e.Row.DataItem;
            //System.Web.UI.WebControls.Image imgPhoto = e.Row.FindControl("Image1") as System.Web.UI.WebControls.Image;


            //if (dr["PHOTOS"] != DBNull.Value && dr["PHOTOS"] != null)
            //{
            //    string imageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dr["PHOTOS"]);
            //    imgPhoto.ImageUrl = imageUrl;
            //}
            //else
            //{
            //    // 如果PHOTOS字段为空或NULL，清空图像
            //    imgPhoto.ImageUrl = string.Empty;
            //}

        }


    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL(txtDate1.Text, txtDate2.Text);


    }


    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    public void SETEXCEL(string SDAYS, string EDAYS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        int imageCounter = 1; // 用于生成唯一图像名称的计数器

        QUERYS.AppendFormat(@" ");

        cmdTxt.AppendFormat(@" 
                            SELECT 
                            [SALESNAMES] AS '業務員'
                            ,[CLIENTSNAMES] AS '客戶'
                            ,[NEWCLIENTSNAMES] AS '新客'
                            ,[KINDS] AS '拜訪目的'
                            ,[RECORDS] AS '訪談內容'
                            ,CONVERT(nvarchar,[RECORDSDATES],111) AS '訪談日期'
                            ,[PHOTOS]
                            ,[ID]
                            ,CONVERT(nvarchar,[CREATDATES],111) AS '建立日期'                            
                            ,[CLIENTSID] AS '客戶代號'
                            ,[PHOTOSID]
                            FROM [TKBUSINESS].[dbo].[TB_SALES_RECORDS]
                            WHERE CONVERT(nvarchar,[RECORDSDATES],111)>='{0}' AND CONVERT(nvarchar,[RECORDSDATES],111)<='{1}'
                            ORDER BY [SALESNAMES],[CLIENTSNAMES],[ID]

                              
                            ", SDAYS, EDAYS);


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "清單" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xlsx";
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
                int COUNT_IMAGES =0;


                //excel標題
                ws.Cells[1, 1].Value = "業務員";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 2].Value = "客戶";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 3].Value = "新客";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 4].Value = "拜訪目的";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 5].Value = "訪談內容";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 6].Value = "訪談日期";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線               
                ws.Cells[1, 7].Value = "ID";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 8].Value = "建立日期";
                ws.Cells[1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 9].Value = "客戶代號";
                ws.Cells[1, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 10].Value = "照片";
                ws.Cells[1, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["業務員"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 2].Value = od["客戶"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 3].Value = od["新客"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 4].Value = od["拜訪目的"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 5].Value = od["訪談內容"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 6].Value = od["訪談日期"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 7].Value = od["ID"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 8].Value = od["建立日期"].ToString();
                    ws.Cells[ROWS, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 9].Value = od["客戶代號"].ToString();
                    ws.Cells[ROWS, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    //ws.Cells[ROWS, 7].Value = od["照片"].ToString();
                    //ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    try
                    {
                        if (od["PHOTOSID"] != DBNull.Value && od["PHOTOSID"] != null)
                        {
                            DataTable dtPHOTOS = SEARCH_TB_SALES_RECORDS_PHOTOS(od["PHOTOSID"].ToString());
                            if(dtPHOTOS != null&& dtPHOTOS.Rows.Count>=1)
                            {
                                COUNT_IMAGES = 0;

                                foreach (DataRow row in dtPHOTOS.Rows)
                                {
                                    if (row["PHOTOS"] != DBNull.Value && row["PHOTOS"] != null)
                                    {
                                        byte[] imageBytes = (byte[])row["PHOTOS"];

                                        using (MemoryStream ms = new MemoryStream(imageBytes))
                                        {
                                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                                            string imageName = row["ID"].ToString() + "_Image" + imageCounter; // 生成唯一的圖像名稱
                                            ExcelPicture picture = excel.Workbook.Worksheets[0].Drawings.AddPicture(imageName, img);

                                            // 設置圖片位置，這裡將 COLUMNS 作為 X 軸偏移
                                            picture.SetPosition(1 * ROWS - 1, 5, 9 + COUNT_IMAGES, 5);

                                            // 設置圖片大小
                                            picture.SetSize(50, 50);

                                            COUNT_IMAGES = COUNT_IMAGES + 1;
                                            imageCounter = imageCounter + 1; // 递增計數器以確保下一個圖像具有唯一的名稱
                                        }
                                    }
                                }
                            }
                        }
                        //if (od["PHOTOS"] != DBNull.Value && od["PHOTOS"] != null)
                        //{
                        //    byte[] imageBytes = (byte[])od["PHOTOS"];

                        //    using (MemoryStream ms = new MemoryStream(imageBytes))
                        //    {
                        //        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                        //        string imageName = od["ID"].ToString() + "_Image" + imageCounter; // 生成唯一的图像名称
                        //        ExcelPicture picture = excel.Workbook.Worksheets[0].Drawings.AddPicture(imageName, img);

                        //        picture.From.Row = ROWS;
                        //        picture.From.Column = COLUMNS;

                        //        picture.SetPosition(1 * ROWS - 1, 5, 6, 5);
                        //        picture.SetSize(50, 50);

                        //        imageCounter++; // 递增计数器以确保下一个图像具有唯一的名称
                        //    }
                        //}
                    }
                    catch
                    {

                    }
                    finally
                    {

                    }

                 


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
        }

    }

    public DataTable SEARCH_TB_SALES_RECORDS_PHOTOS(string PHOTOSID)
    {
        DataTable dt = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                            SELECT 
                            [ID]
                            ,[PHOTOSID]
                            ,[PHOTOS]
                            FROM [TKBUSINESS].[dbo].[TB_SALES_RECORDS_PHOTOS]
                            WHERE PHOTOSID='{0}'
                            ", PHOTOSID);


        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return null;
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
        BindGrid(txtDate1.Text, txtDate2.Text);
    }



    #endregion
}