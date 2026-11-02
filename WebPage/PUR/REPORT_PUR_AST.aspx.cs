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

public partial class CDS_WebPage_PUR_REPORT_PUR_AST : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
           
        }
        else
        {
          

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
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        StringBuilder QUERYS4 = new StringBuilder();


        //TextBox1
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS1.AppendFormat(@" AND (MA002 LIKE '%{0}%' OR TP005 LIKE '%{0}%')", TextBox1.Text);
        }
        else
        {
            QUERYS1.AppendFormat(@" ");
        }

        //TextBox4
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            QUERYS2.AppendFormat(@" AND EMPPLACES LIKE '%{0}%'", TextBox4.Text);
        }
        else
        {
            QUERYS2.AppendFormat(@" ");
        }
        //TextBox5
        if (!string.IsNullOrEmpty(TextBox5.Text))
        {
            QUERYS3.AppendFormat(@" AND COMMENTS LIKE '%{0}%'", TextBox5.Text);
        }
        else
        {
            QUERYS3.AppendFormat(@" ");
        }
        //TextBox7
        if (!string.IsNullOrEmpty(TextBox7.Text))
        {
            QUERYS4.AppendFormat(@" AND TM010 LIKE '%{0}%'", TextBox7.Text);
        }
        else
        {
            QUERYS4.AppendFormat(@" ");
        }


        cmdTxt.AppendFormat(@"
                                SELECT *                                
                                FROM 
                                (
                                SELECT 
                                TO001
                                ,TO002
                                ,TO003
                                ,TO005
                                ,MA002
                                ,TP005
                                ,TP006
                                ,TP007
                                ,TP008
                                ,TP009
                                ,TP010  
                                ,(TP037+TP038) AS TP037038
                                ,STUFF((SELECT DISTINCT  ',' +MB002+ '保管人:'+MV002+'放置:'+MC006 FROM [TK].dbo.ASTMB,[TK].dbo.ASTMC,[TK].dbo.CMSMV WHERE MC003=MV001 AND  MB001=MC001 AND MB002 LIKE '%'+TP005+'%' AND MB002 LIKE '%'+MA002+'%' FOR XML PATH('')) , 1, 1, '') AS 'EMPPLACES' 
                                ,REPLACE(TP001+TP002+TP003,' ','') AS TP001002003
                                ,(SELECT TOP 1 TM010 FROM [TK].dbo.ASTTM WHERE TM001+TM002=TP009+TP010) AS 'TM010'

                                FROM [TK].dbo.ASTTO,[TK].dbo.ASTTP,[TK].dbo.PURMA
                                WHERE 1=1
                                AND TO001=TP001 AND TO002=TP002
                                AND TO005=MA001
                                AND TO013 IN ('Y')
                                ) AS TEMP           
                                LEFT JOIN [TKPUR].[dbo].[UOF_PUR_REPORT_PUR_AST] ON [UOF_PUR_REPORT_PUR_AST].[MAINTP001002003]=TEMP.TP001002003
                                WHERE 1=1
                                {0}
                                {1}
                                {2} 
                                {3}
                                ORDER BY TO001,TO002


                             ", QUERYS1.ToString(), QUERYS2.ToString(), QUERYS3.ToString(), QUERYS4.ToString());



        //m_db.AddParameter("@MB001", TextBox1.Text.Trim());


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
            // 找到 Label 控制項
            Label lblEmpPlaces = (Label)e.Row.FindControl("保管人放置");

            // 檢查是否找到了 Label 控制項
            if (lblEmpPlaces != null)
            {
                // 從資料行中取得 EMPPLACES 字串
                string empPlaces = DataBinder.Eval(e.Row.DataItem, "EMPPLACES").ToString();

                // 將逗號替換為換行符號
                empPlaces = empPlaces.Replace(",", Environment.NewLine);

                // 將結果資料繫結到 Label 控制項
                lblEmpPlaces.Text = empPlaces;
            }
        }


    }
    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid1Button1")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                GridViewRow row = Grid1.Rows[rowIndex];
                // 獲取相應的ID
                Label LabelID = (Label)row.FindControl("ID");
                string ID = LabelID.Text;
                // 獲取txtNewField的值                 
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField");
                string newTextValue = txtNewField.Text;
                //取得 NEWCOMMENTS 的值
                Label LabelTP005 = (Label)row.FindControl("TP005");
                string TP005 = LabelTP005.Text;

                if (!string.IsNullOrEmpty(newTextValue))
                {

                    MODIFY_UOF_PUR_REPORT_PUR_AST(ID, TP005, newTextValue);

                    MsgBox("成功 \r\n" + TP005 + " > " + newTextValue, this.Page, this);

                    BindGrid("");
                }



            }
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

    private void BindGrid2(string NO)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();


        //TextBox3
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            QUERYS1.AppendFormat(@" AND (MB002 LIKE '%{0}%' OR MB008 LIKE '%{0}%') ", TextBox3.Text);
        }
        else
        {
            QUERYS1.AppendFormat(@" ");
        }


        //TextBox1
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS2.AppendFormat(@" AND EMPPLACES LIKE '%{0}%'  ", TextBox2.Text);
        }
        else
        {
            QUERYS2.AppendFormat(@" ");
        }
        //TextBox6
        if (!string.IsNullOrEmpty(TextBox6.Text))
        {
            QUERYS3.AppendFormat(@" AND MB016 LIKE '%{0}%'  ", TextBox6.Text);
        }
        else
        {
            QUERYS3.AppendFormat(@" ");
        }

        cmdTxt.AppendFormat(@"
                                 SELECT *
                                    FROM 
                                    (
                                    SELECT 
                                    MB001
                                    ,MB002
                                    ,MB003
                                    ,MB011
                                    ,MB012
                                    ,MB020
                                    ,MB008
                                    ,MB016
                                    ,STUFF((
                                            SELECT ', ' + MV002+' 放置地點:'+MC006+' 數量:'+CONVERT(NVARCHAR,MC004)
                                            FROM [TK].dbo.ASTMC,[TK].dbo.CMSMV
                                            WHERE MC003=MV001 AND MB001=MC001
                                            FOR XML PATH('')
                                        ), 1, 2, '') AS EMPPLACES
                                    FROM [TK].dbo.ASTMB
                                    ) AS TEMP
                                    WHERE 1=1
                                    {0}
                                    {1}
                                    {2}
                                    ORDER BY MB001
        


                             ", QUERYS1.ToString(), QUERYS2.ToString(), QUERYS3.ToString());



        //m_db.AddParameter("@MB001", TextBox1.Text.Trim());


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
            // 找到 Label 控制項
            Label lblEmpPlaces = (Label)e.Row.FindControl("保管人放置");

            // 檢查是否找到了 Label 控制項
            if (lblEmpPlaces != null)
            {
                // 從資料行中取得 EMPPLACES 字串
                string empPlaces = DataBinder.Eval(e.Row.DataItem, "EMPPLACES").ToString();

                // 將逗號替換為換行符號
                empPlaces = empPlaces.Replace(",", Environment.NewLine);

                // 將結果資料繫結到 Label 控制項
                lblEmpPlaces.Text = empPlaces;
            }
        }


    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
       

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

    public void MODIFY_UOF_PUR_REPORT_PUR_AST(
        string TP001002003
        ,string TP005
        ,string COMMENTS
       )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";

        cmdTxt = @" 
                    DELETE [TKPUR].[dbo].[UOF_PUR_REPORT_PUR_AST]
                    WHERE  [MAINTP001002003]=@TP001002003

                    INSERT INTO [TKPUR].[dbo].[UOF_PUR_REPORT_PUR_AST]
                    (
                    [MAINTP001002003]
                    ,[TP005NAMES]
                    ,[COMMENTS]
                    )
                    VALUES
                    (
                    @TP001002003
                    ,@TP005
                    ,@COMMENTS
                    )
                        ";
        m_db.AddParameter("@TP001002003", TP001002003);
        m_db.AddParameter("@TP005", TP005);
        m_db.AddParameter("@COMMENTS", COMMENTS);

        m_db.ExecuteNonQuery(cmdTxt);
    }
    public void MsgBox(String ex, Page pg, Object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);

        //string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        //Type cstype = obj.GetType();
        //ClientScriptManager cs = pg.ClientScript;
        //cs.RegisterClientScriptBlock(cstype, s, s.ToString());
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
        BindGrid("");
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        BindGrid2("");
    }
    
    
    #endregion
}