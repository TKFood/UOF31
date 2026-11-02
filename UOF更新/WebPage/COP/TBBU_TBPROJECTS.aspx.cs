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

public partial class CDS_WebPage_COP_TBPROJECTS : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBox1.Text = DateTime.Now.Year.ToString();
            TextBox2.Text = DateTime.Now.Year.ToString();

            BindGrid();
            BindGrid2();
        }
        else
        {

            BindGrid();
            BindGrid2();


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
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();


        //END +ISNULL([NAMES],'')+'<br>'+ISNULL([DAYS],'')+'<br>'+ISNULL([TARGETS],'')+'<br> '+ISNULL([FEES],'')+'<br> '+ISNULL([ITEMS],'') AS 'data()'

        cmdTxt.AppendFormat(@" 
                            --20210617 查年度的週計劃
                            SELECT [YEARS],[WEEKS],[FDAY],[EDAY],[好市多],[全聯],[CVS-7-11],[CVS-全家],[KA-家樂福],[門市],[官網],[新東陽],[經銷統記],[經銷統創],[ㄧ般經銷]
                            FROM (
                            SELECT 
                            [YEARS]
                            ,[WEEKS]
                            ,CONVERT(NVARCHAR,[FDAY],111) FDAY
                            ,CONVERT(NVARCHAR,[EDAY],111) EDAY
                            ,ISNULL( (     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%好市多%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '好市多' 
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%全聯%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '全聯'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%CVS-7-11%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As 'CVS-7-11'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                 END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%CVS-全家%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As 'CVS-全家'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%KA-家樂福%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As 'KA-家樂福'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%門市%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '門市'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                 END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%官網%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '官網'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%新東陽%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '新東陽'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                   END +ISNULL([NAMES],'') AS 'data()'
                                FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%統記%'
                             
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '經銷統記'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                   END +ISNULL([NAMES],'') AS 'data()'
                                FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%統創%'
                             
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '經銷統創'
                            , ISNULL((     
                            SELECT CASE
                                    WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                    ELSE '<br />'
                                  END +ISNULL([NAMES],'') AS 'data()'
                             FROM [TKBUSINESS].[dbo].[TBPROJECTS] WHERE [TBPROJECTS].[YEARS]=[TBYEARWEEKS].YEARS AND [TBPROJECTS].[WEEKS]=[TBYEARWEEKS].WEEKS AND [STORES] LIKE '%ㄧ般經銷%'
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As 'ㄧ般經銷'

                            FROM [TKBUSINESS].[dbo].[TBYEARWEEKS]
                            ) AS TEMP
                            WHERE 1=1
                            AND [YEARS]=@YEARS
                            ORDER BY [YEARS],CONVERT(INT,[WEEKS])
                              
                            ");




        m_db.AddParameter("@YEARS", TextBox1.Text.ToString().Trim());
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
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //通路
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            QUERYS.AppendFormat(@" AND [STORES] LIKE '%{0}%'", TextBox3.Text);
        }
        //活動名稱
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            QUERYS.AppendFormat(@" AND [NAMES] LIKE '%{0}%'", TextBox4.Text);
        }


        cmdTxt.AppendFormat(@" 
                            SELECT
                            [ID]
                            ,[YEARS]
                            ,[WEEKS]
                            ,[STORES]
                            ,[NAMES]
                            ,[ITEMS]
                            ,[CONTENTS]
                            ,[DAYS]
                            FROM [TKBUSINESS].[dbo].[TBPROJECTS]
                            WHERE 1=1
                            AND [YEARS]=@YEARS
                            {0}
                            ORDER BY [YEARS],CONVERT(INT,[WEEKS]),[STORES] ,[NAMES]
                              
                            ", QUERYS.ToString());




        m_db.AddParameter("@YEARS", TextBox2.Text.ToString().Trim());
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
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBPROJECTSDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        }
    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

      
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


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if(dt.Rows.Count>0)
        {
            //檔案名稱
            var fileName = "計劃清單" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
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