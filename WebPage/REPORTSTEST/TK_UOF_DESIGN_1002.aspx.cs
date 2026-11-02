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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

public partial class CDS_WebPage_RESEARCH_TK_UOF_DESIGN_1002 : Ede.Uof.Utility.Page.BasePage
{
    //1002.產品設計申請，表單成立就轉入

    string RowIndex = "";
    String connectionString;
    SqlConnection sqlConn = new SqlConnection();
    SqlTransaction tran;
    SqlCommand cmd = new SqlCommand();
    int result;
    StringBuilder sbSql = new StringBuilder();
    StringBuilder sbSqlQuery = new StringBuilder();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDownList();
            BindGrid1("");            
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


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"    SELECT 
                                 [ID]
                                ,[KIND]
                                ,[PARAID]
                                ,[PARANAME]
                                FROM [TKRESEARCH].[dbo].[TBPARA]
                                WHERE [KIND]='TK_UOF_RESEARCH_1002'
                                ORDER BY [ID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "PARAID";
            DropDownList1.DataValueField = "PARAID";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }


    private void BindGrid1(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();


        //查詢條件
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND [FIELDS10] LIKE '%{0}%' ", TextBox1.Text);
        }
        else
        {
            QUERYS.AppendFormat(@" ");
        }
        if (!string.IsNullOrEmpty(DropDownList1.SelectedValue.ToString()))
        {
            QUERYS2.AppendFormat(@" AND [ISCLOSED]='{0}' ", DropDownList1.SelectedValue.ToString());
        }
        else
        {
            QUERYS2.AppendFormat(@" ");
        }


        cmdTxt.AppendFormat(@" 
                            SELECT
                            [FIELDS1] AS '表單編號'
                            ,[FIELDS2] AS '類別'
                            ,[FIELDS3] AS '填表日期'
                            ,[FIELDS4] AS '生產部代簽部門'
                            ,[FIELDS5] AS '交付人'
                            ,[FIELDS6] AS '交付部門'
                            ,[FIELDS7] AS '交付者職級'
                            ,[FIELDS8] AS '接辦人員'
                            ,[FIELDS9] AS '期望交期'
                            ,[FIELDS10] AS '簡述交辦事項'
                            ,[FIELDS11] AS '交辦說明'
                            ,[FIELDS12] AS '接辦人處理項目描述'
                            ,[FIELDS13] AS '完成交辦文件'
                            ,[INPROCESSING] AS '處理進度'
                            ,[ISCLOSED] AS '是否結案'
                            FROM [TKRESEARCH].[dbo].[TK_UOF_DESIGN_1002]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY  [FIELDS1]
                               
                                ", QUERYS.ToString(), QUERYS2.ToString());


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
        


    }

    protected void Grid1_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    { 
        RowIndex = e.RowIndex.ToString();
    }
    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Button1")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = Grid1.Rows[rowIndex];
            // 接下來可以從行中取得 TextBox 控制項的值
            //TextBox TextBoxFIELDS1 = (TextBox)row.FindControl("FIELDS1");
            //string FIELDS1 = TextBoxFIELDS1.Text;


            TextBox TextBoxGRIDVIEWTextBox1 = (TextBox)row.FindControl("GRIDVIEWTextBox1");
            string INPROCESSING = TextBoxGRIDVIEWTextBox1.Text;
            DropDownList DropDownListGRIDVIEWDropDownList1=(DropDownList)row.FindControl("GRIDVIEWDropDownList1");
            string ISCLOSED = DropDownListGRIDVIEWDropDownList1.SelectedValue.ToString();

            Label LabelFIELDS1=(Label)row.FindControl("LabelFIELDS1");
            string FIELDS1 = LabelFIELDS1.Text;

            //更新UPDATE_TK_UOF_DESIGN_1002
            if(!string.IsNullOrEmpty(INPROCESSING))
            {
                UPDATE_TK_UOF_DESIGN_1002(FIELDS1, INPROCESSING, ISCLOSED);

                BindGrid1("");
            }
            else
            {
                MsgBox("表單: " + FIELDS1 + "\r\n" + "未填寫處理進度，不允許更新 ", this.Page, this);
            }
            
            //MsgBox(e.CommandArgument.ToString() + "\r\n  "+ " INPROCESSING: " + INPROCESSING + "\r\n  "  + " ISCLOSED: " + ISCLOSED + "\r\n  " + " FIELDS1: " + FIELDS1, this.Page, this);
        }
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

        SETEXCEL();

    }

    public void SETEXCEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();


        //查詢條件
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND [FIELDS10] LIKE '%{0}%' ", TextBox1.Text);
        }
        else
        {
            QUERYS.AppendFormat(@" ");
        }
        if (!string.IsNullOrEmpty(DropDownList1.SelectedValue.ToString()))
        {
            QUERYS2.AppendFormat(@" AND [ISCLOSED]='{0}' ", DropDownList1.SelectedValue.ToString());
        }
        else
        {
            QUERYS2.AppendFormat(@" ");
        }


        cmdTxt.AppendFormat(@" 
                            SELECT
                            [FIELDS1] AS '表單編號'
                            ,[FIELDS2] AS '類別'
                            ,[FIELDS3] AS '填表日期'
                            ,[FIELDS4] AS '生產部代簽部門'
                            ,[FIELDS5] AS '交付人'
                            ,[FIELDS6] AS '交付部門'
                            ,[FIELDS7] AS '交付者職級'
                            ,[FIELDS8] AS '接辦人員'
                            ,[FIELDS9] AS '期望交期'
                            ,[FIELDS10] AS '簡述交辦事項'
                            ,[FIELDS11] AS '交辦說明'
                            ,[FIELDS12] AS '接辦人處理項目描述'
                            ,[FIELDS13] AS '完成交辦文件'
                            ,[INPROCESSING] AS '處理進度'
                            ,[ISCLOSED] AS '是否結案'
                            FROM [TKRESEARCH].[dbo].[TK_UOF_DESIGN_1002]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY  [FIELDS1]
                               
                                ", QUERYS.ToString(), QUERYS2.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "明細" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());


                //預設行高
                //ws.DefaultRowHeight = 60;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "表單編號";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 2].Value = "類別";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 3].Value = "填表日期";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 4].Value = "生產部代簽部門";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 5].Value = "交付人";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 6].Value = "交付部門";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 7].Value = "交付者職級";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 8].Value = "接辦人員";
                ws.Cells[1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 9].Value = "期望交期";
                ws.Cells[1, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 10].Value = "簡述交辦事項";
                ws.Cells[1, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 11].Value = "交辦說明";
                ws.Cells[1, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 12].Value = "接辦人處理項目描述";
                ws.Cells[1, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 13].Value = "完成交辦文件";
                ws.Cells[1, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 13].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 14].Value = "處理進度";
                ws.Cells[1, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 14].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 15].Value = "是否結案";
                ws.Cells[1, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 15].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["表單編號"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 2].Value = od["類別"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 3].Value = od["填表日期"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 4].Value = od["生產部代簽部門"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 5].Value = od["交付人"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 6].Value = od["交付部門"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 7].Value = od["交付者職級"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 8].Value = od["接辦人員"].ToString();
                    ws.Cells[ROWS, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 9].Value = od["期望交期"].ToString();
                    ws.Cells[ROWS, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 10].Value = od["簡述交辦事項"].ToString();
                    ws.Cells[ROWS, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 11].Value = od["交辦說明"].ToString();
                    ws.Cells[ROWS, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 12].Value = od["接辦人處理項目描述"].ToString();
                    ws.Cells[ROWS, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 13].Value = od["完成交辦文件"].ToString();
                    ws.Cells[ROWS, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 13].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 14].Value = od["處理進度"].ToString();
                    ws.Cells[ROWS, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 14].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 15].Value = od["是否結案"].ToString();
                    ws.Cells[ROWS, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 15].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

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

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

        //SETEXCEL();

    }

    public void MsgBox(String ex, Page pg, Object obj)
    {
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }

    public void UPDATE_TK_UOF_DESIGN_1002(string FIELDS1,string INPROCESSING ,string ISCLOSED)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        StringBuilder queryString = new StringBuilder();
        queryString.AppendFormat(@"   
                                    UPDATE [TKRESEARCH].[dbo].[TK_UOF_DESIGN_1002]
                                    SET INPROCESSING=@INPROCESSING,ISCLOSED=@ISCLOSED
                                    WHERE FIELDS1=@FIELDS1
                                        ");

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString.ToString(), connection);
                command.Parameters.Add("@FIELDS1", SqlDbType.NVarChar).Value = FIELDS1;
                command.Parameters.Add("@INPROCESSING", SqlDbType.NVarChar).Value = INPROCESSING;
                command.Parameters.Add("@ISCLOSED", SqlDbType.NVarChar).Value = ISCLOSED;


                command.Connection.Open();

                int count = command.ExecuteNonQuery();

                connection.Close();
                connection.Dispose();

            }
        }
        catch
        {

        }
        finally
        {

        }
    }

    public void NEW_TKRESEARCH_TK_UOF_DESIGN_1002()
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();

        string THISYEARS = DateTime.Now.ToString("yyyy");
        //取西元年後2位
        THISYEARS = THISYEARS.Substring(2, 2);
        //THISYEARS = "21";
        string THISYEARSDAYS = DateTime.Now.ToString("yyyy") + "0101";

        try
        {         
            SqlConnectionStringBuilder sqlsb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
           
            sqlConn = new SqlConnection(sqlsb.ConnectionString);
        
            sbSql.Clear();
            sbSqlQuery.Clear();



            //核準過TASK_RESULT='0'
            //AND DOC_NBR  LIKE 'QC1002{0}%'
            // 1002.產品設計申請有申請就轉入，不要等核完表單
            //AND DOC_NBR COLLATE Chinese_Taiwan_Stroke_BIN NOT IN (SELECT  [FIELDS1] FROM [192.168.1.105].[TKRESEARCH].[dbo].[TK_UOF_DESIGN_1002])

            sbSql.AppendFormat(@"  
                                        SELECT TB_WKF_FORM.FORM_NAME,DOC_NBR,*
                                        FROM [UOF].dbo.TB_WKF_TASK,[UOF].dbo.TB_WKF_FORM,[UOF].dbo.TB_WKF_FORM_VERSION
                                        WHERE 1=1
                                        AND TB_WKF_TASK.FORM_VERSION_ID=TB_WKF_FORM_VERSION.FORM_VERSION_ID
                                        AND TB_WKF_FORM.FORM_ID=TB_WKF_FORM_VERSION.FORM_ID
                                        AND TB_WKF_FORM.FORM_NAME IN ('1002.產品設計申請')
                                        AND TB_WKF_TASK.TASK_STATUS='1'
                                       AND DOC_NBR COLLATE Chinese_Taiwan_Stroke_BIN NOT IN (SELECT  [FIELDS1] FROM [192.168.1.105].[TKRESEARCH].[dbo].[TK_UOF_DESIGN_1002])
                                        
                                       
                                    ");


            adapter1 = new SqlDataAdapter(@"" + sbSql, sqlConn);

            sqlCmdBuilder1 = new SqlCommandBuilder(adapter1);
            sqlConn.Open();
            ds1.Clear();
            adapter1.Fill(ds1, "ds1");
            sqlConn.Close();

            if (ds1.Tables["ds1"].Rows.Count >= 1)
            {
                foreach (DataRow dr in ds1.Tables["ds1"].Rows)
                {
                    SEARCH_UOF_TK_UOF_DESIGN_1002(dr["DOC_NBR"].ToString());
                }
            }
            else
            {

            }

        }
        catch
        {

        }
        finally
        {
            sqlConn.Close();
        }
    }

    //找出UOF表單的資料，將CURRENT_DOC的內容，轉成xmlDoc
    //從xmlDoc找出各節點的Attributes
    public void SEARCH_UOF_TK_UOF_DESIGN_1002(string DOC_NBR)
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();

        try
        {
            SqlConnectionStringBuilder sqlsb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

            String connectionString;
            sqlConn = new SqlConnection(sqlsb.ConnectionString);

            sbSql.Clear();
            sbSqlQuery.Clear();

            //庫存數量看LA009 IN ('20004','20006','20008','20019','20020'

            sbSql.AppendFormat(@"  
                                    SELECT * 
                                    FROM [UOF].DBO.TB_WKF_TASK 
                                    LEFT JOIN [UOF].[dbo].[TB_EB_USER] ON [TB_EB_USER].USER_GUID=TB_WKF_TASK.USER_GUID
                                    WHERE DOC_NBR LIKE '{0}%'
                              
                                    ", DOC_NBR);


            adapter1 = new SqlDataAdapter(@"" + sbSql, sqlConn);

            sqlCmdBuilder1 = new SqlCommandBuilder(adapter1);
            sqlConn.Open();
            ds1.Clear();
            adapter1.Fill(ds1, "ds1");
            sqlConn.Close();

            if (ds1.Tables["ds1"].Rows.Count >= 1)
            {
                string FIELDS1 = "";
                string FIELDS2 = "";
                string FIELDS3 = "";
                string FIELDS4 = "";
                string FIELDS5 = "";
                string FIELDS6 = "";
                string FIELDS7 = "";
                string FIELDS8 = "";
                string FIELDS9 = "";
                string FIELDS10 = "";
                string FIELDS11 = "";
                string FIELDS12 = "";
                string FIELDS13 = "";


                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(ds1.Tables["ds1"].Rows[0]["CURRENT_DOC"].ToString());



                //XmlNode node = xmlDoc.SelectSingleNode($"/Form/FormFieldValue/FieldItem[@fieldId='ID']");
                try
                {                   
                    FIELDS1 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00001']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS2 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00000']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS3 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00005']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS4 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00012']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS5 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00002']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS6 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00003']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS7 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00004']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS8 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00011']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS9 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00013']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS10 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00010']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS12 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00014']").Attributes["fieldValue"].Value;
                }
                catch { }
                try
                {
                    FIELDS13 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00015']").Attributes["fieldValue"].Value;
                }
                catch { }


                try
                {
                    //把html語法去除 
                    //QCFrm002Cmf = xmlDoc.SelectSingleNode($"/Form/FormFieldValue/FieldItem[@fieldId='QCFrm002Cmf']").Attributes["fieldValue"].Value;

                    string fieldValue1 = xmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='00009']").Attributes["fieldValue"].Value;

                    string fieldValue2 = Regex.Replace(fieldValue1, @"&#xD;", "");
                    string fieldValue3 = Regex.Replace(fieldValue2, @"&#xA;", "");
                    string fieldValue4 = Regex.Replace(fieldValue3, @"<p>", "");
                    string fieldValue5 = Regex.Replace(fieldValue4, @"</p>", "");

                    //FIELDS11 = fieldValue5;
                }
                catch { }
                //string OK = "";
                ADD_TK_UOF_DESIGN_1002(
                                          FIELDS1
                                        , FIELDS2
                                        , FIELDS3
                                        , FIELDS4
                                        , FIELDS5
                                        , FIELDS6
                                        , FIELDS7
                                        , FIELDS8
                                        , FIELDS9
                                        , FIELDS10
                                        , FIELDS11
                                        , FIELDS12
                                        , FIELDS13

                                       );


            }
            else
            {

            }

        }
        catch
        {

        }
        finally
        {
            sqlConn.Close();
        }
    }


    public void ADD_TK_UOF_DESIGN_1002(
                                        string FIELDS1
                                        , string FIELDS2
                                        , string FIELDS3
                                        , string FIELDS4
                                        , string FIELDS5
                                        , string FIELDS6
                                        , string FIELDS7
                                        , string FIELDS8
                                        , string FIELDS9
                                        , string FIELDS10
                                        , string FIELDS11
                                        , string FIELDS12
                                        , string FIELDS13

                                        )
    {
        try
        {
            SqlConnectionStringBuilder sqlsb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString);          


            String connectionString;
            sqlConn = new SqlConnection(sqlsb.ConnectionString);

            sqlConn.Close();
            sqlConn.Open();
            tran = sqlConn.BeginTransaction();

            sbSql.Clear();

            sbSql.AppendFormat(@"                                   
                                    
                                    INSERT INTO [TKRESEARCH].[dbo].[TK_UOF_DESIGN_1002]
                                    (
                                    [FIELDS1]
                                    ,[FIELDS2]
                                    ,[FIELDS3]
                                    ,[FIELDS4]
                                    ,[FIELDS5]
                                    ,[FIELDS6]
                                    ,[FIELDS7]
                                    ,[FIELDS8]
                                    ,[FIELDS9]
                                    ,[FIELDS10]
                                    ,[FIELDS11]
                                    ,[FIELDS12]
                                    ,[FIELDS13]
                                    )
                                    VALUES
                                    (
                                    '{0}'
                                    ,'{1}'
                                    ,'{2}'
                                    ,'{3}'
                                    ,'{4}'
                                    ,'{5}'
                                    ,'{6}'
                                    ,'{7}'
                                    ,'{8}'
                                    ,'{9}'
                                    ,'{10}'
                                    ,'{11}'
                                    ,'{12}'
                                    )
                                    ", FIELDS1
                                , FIELDS2
                                , FIELDS3
                                , FIELDS4
                                , FIELDS5
                                , FIELDS6
                                , FIELDS7
                                , FIELDS8
                                , FIELDS9
                                , FIELDS10
                                , FIELDS11
                                , FIELDS12
                                , FIELDS13);

            cmd.Connection = sqlConn;
            cmd.CommandTimeout = 60;
            cmd.CommandText = sbSql.ToString();
            cmd.Transaction = tran;
            result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                tran.Rollback();    //交易取消
            }
            else
            {
                tran.Commit();      //執行交易  
            }

        }
        catch
        {

        }

        finally
        {
            sqlConn.Close();
        }
    }


    #endregion

    #region BUTTON
    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid1("");

    }
    protected void btn2_Click(object sender, EventArgs e)
    {
        NEW_TKRESEARCH_TK_UOF_DESIGN_1002();
        BindGrid1("");

        MsgBox("完成", this.Page, this);

    }



    #endregion
}