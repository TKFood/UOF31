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
using Image = System.Web.UI.WebControls.Image;

public partial class CDS_WebPage_TBBU_TBSALESEVENTSFORSALES : Ede.Uof.Utility.Page.BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string ACCOUNT = null;
        string NAME = null;
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        //NAME = "洪櫻芬";

        if (!IsPostBack)
        {
            BindDropDownList1();
            BindDropDownList2(NAME);
            BindDropDownList3();

            BindGrid();           
        }
        else
        {

           BindGrid();
           

        }

       


    }
    #region FUNCTION
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

    private void BindDropDownList2(string NAME)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("NAME", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"SELECT  [ID],[NAME] FROM [TKBUSINESS].[dbo].[TBSALESNAME]  ORDER BY [ID]";
        string cmdTxt = @"SELECT  [ID],[NAME] FROM [TKBUSINESS].[dbo].[TBSALESNAME] WHERE [NAME]=@NAME ORDER BY [ID]";

        m_db.AddParameter("@NAME", NAME);

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
        else
        {
            //如果沒有指定的業務員，就不要查詢結果
            QUERYS.AppendFormat(@" AND 1=0");
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
                            ,REPLACE([COMMENTS],char(10),'<br/>') AS [COMMENTS] 
                            ,[ISCLOSE]
                            ,[FILENAME]
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
            var fileName = "清單" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
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
        //Response.ClearContent();
        //Response.AddHeader("content-disposition", "attachment; filename=test.xls");
        //Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
        //HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=big5>");
        //HttpContext.Current.Response.Write("<head><meta http-equiv=Content-Type content=text/html;charset=big5></head>");
        //Response.Charset = "big5";
        //Response.ContentType = "application/excel";


        //System.IO.StringWriter sw = new System.IO.StringWriter();
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //Grid1.RenderControl(htw);
        //Response.Write(sw.ToString());
        //Response.End();
    }
        protected void MyButtonClick(object sender, System.EventArgs e)
    {
      

    }

    protected void btn5_Click(object sender, EventArgs e)
    {

        ////this.Session["STATUS"] = DropDownList1.SelectedItem.Text ;
        //ViewState["TextBox1"] = TextBox1.Text.ToString();
        //ViewState["TextBox2"] = TextBox2.Text.ToString();
       

        //BindGrid("");

        //TextBox1.Text = ViewState["TextBox1"].ToString();
        //TextBox2.Text = ViewState["TextBox2"].ToString();
        
     

        //if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        //{
        //    if (Dialog.GetReturnValue().Equals("NeedPostBack"))
        //    {

        //    }

        //}
    }
    #endregion
}