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
using System.Web.Services;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Ede.Uof.EIP.SystemInfo;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CDS_WebPage_COP_TB_SALES_ASSINGED_CN : Ede.Uof.Utility.Page.BasePage
{
    DataTable EXCELDT1 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            BindDropDownList1();
            BindDropDownList2();
            BindDropDownList3();
            BindDropDownList4();

            BindDropDownListISCLOSE();
            BindDropDownListISCLOSE2();
            BindDropDownListISCLOSE3();

            BindGrid();
            BindGrid2();
            BindGrid3();

        }
    }

    #region FUNCTION
    private void BindDropDownList1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[NAME]
                        ,[LEADER]
                        FROM [TKBUSINESS].[dbo].[TB_SALESNAME_CN]
                        WHERE [NAME] NOT IN ('全部')
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "NAME";
            DropDownList1.DataValueField = "NAME";
            DropDownList1.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='是否結案'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "NAMES";
            DropDownList2.DataValueField = "NAMES";
            DropDownList2.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownList3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT  
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_SALES_ASSINGED'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList3.DataSource = dt;
            DropDownList3.DataTextField = "NAMES";
            DropDownList3.DataValueField = "NAMES";
            DropDownList3.DataBind();

        }
        else
        {

        }
    }

    private void BindDropDownList4()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT *
                        FROM 
                        (
                        SELECT 
                        [ID]
                        ,[NAME]
                        ,[LEADER]
                        FROM [TKBUSINESS].[dbo].[TB_SALESNAME_CN]
                        WHERE [NAME] NOT IN ('全部')
                        UNION ALL
                        SELECT 
                        '' [ID]
                        ,'' [NAME]
                        ,'' [LEADER]
                        ) AS TMEP
                        ORDER BY [ID]

                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList4.DataSource = dt;
            DropDownList4.DataTextField = "NAME";
            DropDownList4.DataValueField = "NAME";
            DropDownList4.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownListISCLOSE()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='是否結案'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE.DataSource = dt;
            DropDownListISCLOSE.DataTextField = "NAMES";
            DropDownListISCLOSE.DataValueField = "NAMES";
            DropDownListISCLOSE.DataBind();

        }
        else
        {

        }
    }

    private void BindDropDownListISCLOSE2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='是否結案'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE2.DataSource = dt;
            DropDownListISCLOSE2.DataTextField = "NAMES";
            DropDownListISCLOSE2.DataValueField = "NAMES";
            DropDownListISCLOSE2.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownListISCLOSE3()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KINDS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='是否結案'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE3.DataSource = dt;
            DropDownListISCLOSE3.DataTextField = "NAMES";
            DropDownListISCLOSE3.DataValueField = "NAMES";
            DropDownListISCLOSE3.DataBind();

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
        StringBuilder Query1 = new StringBuilder();
        StringBuilder Query2 = new StringBuilder();
        StringBuilder Query3 = new StringBuilder();
        StringBuilder Query99 = new StringBuilder();

        if (!string.IsNullOrEmpty(TextBox_CLIENTS.Text))
        {
            Query1.AppendFormat(@" AND ID IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN] WHERE [CLIENTS] LIKE '%{0}%') ", TextBox_CLIENTS.Text);
        }
        else
        {
            Query1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(DropDownListISCLOSE.SelectedValue.ToString()))
        {
            if (DropDownListISCLOSE.SelectedValue.ToString().Equals("全部"))
            {
                Query2.AppendFormat(@"");
            }
            else
            {
                Query2.AppendFormat(@"AND ID IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN] WHERE [ISCLOSE] LIKE '%{0}%')", DropDownListISCLOSE.SelectedValue.ToString());
            }

        }
        else
        {
            Query2.AppendFormat(@"");
        }

        if (!string.IsNullOrEmpty(DropDownList4.SelectedValue.ToString()))
        {
            if (!string.IsNullOrEmpty(DropDownList4.SelectedValue.ToString()))
            {
                Query3.AppendFormat(@"AND [SALES]='{0}'", DropDownList4.SelectedValue.ToString());
            }
            else
            {
                Query3.AppendFormat(@"");
            }

        }


        //ORDER BY
        if (!string.IsNullOrEmpty(DropDownList3.SelectedValue.ToString()))
        {
            if (DropDownList3.SelectedValue.ToString().Equals("依業務+客戶"))
            {
                Query99.AppendFormat(@"  ORDER BY [SALES],[CLIENTS],[EDAYS],[ID]");
            }
            else if (DropDownList3.SelectedValue.ToString().Equals("依業務+回覆期限"))
            {
                Query99.AppendFormat(@"  ORDER BY [SALES],[EDAYS],[CLIENTS],[ID]");
            }
            else
            {
                Query99.AppendFormat(@"  ORDER BY [SALES],[CLIENTS],[EDAYS],[ID]");

            }

        }
        else
        {
            Query99.AppendFormat(@"  ORDER BY [SALES],[CLIENTS],[EDAYS],[ID]");
        }


        cmdTxt.AppendFormat(@"
                            SELECT 
                            ROW_NUMBER() OVER ({3}) AS '序號'
                            ,[TB_SALES_ASSINGED_CN].[ID]
                            ,[SALES]
                            ,[CLIENTS]
                            ,[EVENTS]
                            ,CONVERT(NVARCHAR,[EDAYS],111) EDAYS
                            ,[ISCLOSE]
                            ,CONVERT(NVARCHAR,[ADDDATES],111) ADDDATES
                            ,CONVERT(NVARCHAR,[ISCLOSEDATES],111) ISCLOSEDATES
                            ,(SELECT TOP 1 [COMMENTS] FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS_CN] WHERE [TB_SALES_ASSINGED_COMMENTS_CN].MID=[TB_SALES_ASSINGED_CN].ID ORDER BY ID DESC) AS COMMENTS
                            ,(SELECT TOP 1 CONVERT(NVARCHAR,[TB_SALES_ASSINGED_COMMENTS_CN].[ADDDATES],111) FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS_CN] WHERE [TB_SALES_ASSINGED_COMMENTS_CN].MID=[TB_SALES_ASSINGED_CN].ID ORDER BY ID DESC) AS COMMENTSADDDATES

                            FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN]
                            WHERE 1=1
                            {0}
                            {1}
                            {2}

                            {3}
                           

                              
                            ", Query1.ToString(), Query2.ToString(), Query3.ToString(), Query99.ToString());


        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        //匯出專用
        EXCELDT1 = dt;

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
            // 假設 txtNewField 是一個 Label 控制項
            TextBox txtNewField = (TextBox)e.Row.FindControl("txtNewField");
            Button Grid1Button1 = (Button)e.Row.FindControl("Grid1Button1");
            Label LabelSALES = (Label)e.Row.FindControl("SALES");
            Button Grid1Button2 = (Button)e.Row.FindControl("Grid1Button2");
            Button Grid1Button3 = (Button)e.Row.FindControl("Grid1Button3");
            // 假設事件在資料繫結時，ISCLOSE 欄位的名稱是 "ISCLOSE"
            string eventValue = DataBinder.Eval(e.Row.DataItem, "ISCLOSE") as string;

            // 如果事件欄位的值為空，就隱藏 txtNewField
            if (string.IsNullOrWhiteSpace(eventValue))
            {
                txtNewField.Visible = false;
                Grid1Button1.Visible = false;
                LabelSALES.Visible = false;
                Grid1Button2.Visible = false;
                Grid1Button3.Visible = false;
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
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField");
                string newTextValue = txtNewField.Text.Trim();

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID");
                string id = txtid.Text;

                ADD_TB_SALES_ASSINGED_COMMENTS_CN(id, newTextValue);
                MsgBox(id + " 完成" , this.Page, this);

                BindGrid();
            }
        }
        if (e.CommandName == "Grid1Button2")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField");
                string newTextValue = txtNewField.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID");
                string id = txtid.Text;

                UPDATE_TB_SALES_ASSINGED_CN_YN(id, "Y");
                MsgBox(id + " 完成", this.Page, this);
             
                BindGrid();
            }
        }
        if (e.CommandName == "Grid1Button3")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField");
                string newTextValue = txtNewField.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID");
                string id = txtid.Text;

                UPDATE_TB_SALES_ASSINGED_CN_YN(id, "N");
                MsgBox(id + " 完成", this.Page, this);

                BindGrid();
            }
        }

    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();
    }
    public void SETEXCEL()
    {
        BindGrid();
        //BindGrid中已帶入EXCELDT1
        if (EXCELDT1.Rows.Count >= 1)
        {
            //檔案名稱
            var fileName = "明細" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xlsx";
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
                ws.Cells[1, 1].Value = "業務員";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 2].Value = "客戶";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 3].Value = "回覆期限";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 4].Value = "交辨內容";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 5].Value = "已回覆內容(最近)";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 6].Value = "回覆日期(最近)";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 7].Value = "是否結案";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 8].Value = "結案日";
                ws.Cells[1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                foreach (DataRow od in EXCELDT1.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["SALES"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 2].Value = od["CLIENTS"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 3].Value = od["EDAYS"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 4].Value = od["EVENTS"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 5].Value = od["COMMENTS"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 6].Value = od["COMMENTSADDDATES"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 7].Value = od["ISCLOSE"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 8].Value = od["ISCLOSEDATES"].ToString();
                    ws.Cells[ROWS, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線


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

    public void ADD_TB_SALES_ASSINGED_COMMENTS_CN(string MID, string COMMENTS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                   INSERT INTO [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS_CN]
                    ([MID],[COMMENTS])
                    VALUES (@MID,@COMMENTS)
                        ";


        m_db.AddParameter("@MID", MID);
        m_db.AddParameter("@COMMENTS", COMMENTS);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void UPDATE_TB_SALES_ASSINGED_CN_YN(string ID, string ISCLOSE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";
        string ISCLOSEDATES = DateTime.Now.ToString("yyyy/MM/dd");

        cmdTxt = @"
                UPDATE [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN]
                SET [ISCLOSE]=@ISCLOSE,[ISCLOSEDATES]=@ISCLOSEDATES
                WHERE [ID]=@ID
                        ";


        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@ISCLOSE", ISCLOSE);
        m_db.AddParameter("@ISCLOSEDATES", ISCLOSEDATES);

        m_db.ExecuteNonQuery(cmdTxt);
    }


    public void ADD_TB_SALES_ASSINGED_CN(
                                   string SALES
                                   , string CLIENTS
                                   , string EVENTS
                                   , string EDAYS
                                   , string ISCLOSE
                                   , string ADDDATES
                                   )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"               
                        INSERT INTO  [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN]
                        (
                        [SALES]
                        ,[CLIENTS]
                        ,[EVENTS]
                        ,[EDAYS]
                        ,[ISCLOSE]
                        ,[ADDDATES]
                        )
                        VALUES
                        (
                        @SALES
                        ,@CLIENTS
                        ,@EVENTS
                        ,@EDAYS
                        ,@ISCLOSE
                        ,@ADDDATES
                        )
                        ";


        m_db.AddParameter("@SALES", SALES);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@EVENTS", EVENTS);
        m_db.AddParameter("@EDAYS", EDAYS);
        m_db.AddParameter("@ISCLOSE", ISCLOSE);
        m_db.AddParameter("@ADDDATES", ADDDATES);

        m_db.ExecuteNonQuery(cmdTxt);
    }

    private void BindGrid2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder Query1 = new StringBuilder();
        StringBuilder Query2 = new StringBuilder();

        if (!string.IsNullOrEmpty(TextBox_CLIENTS2.Text))
        {
            Query1.AppendFormat(@" AND ID IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN] WHERE [CLIENTS] LIKE '%{0}%') ", TextBox_CLIENTS2.Text);
        }
        else
        {
            Query1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(DropDownListISCLOSE2.SelectedValue.ToString()))
        {
            if (DropDownListISCLOSE2.SelectedValue.ToString().Equals("全部"))
            {
                Query2.AppendFormat(@"");
            }
            else
            {
                Query2.AppendFormat(@"AND ID IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN] WHERE [ISCLOSE] LIKE '%{0}%')", DropDownListISCLOSE2.SelectedValue.ToString());
            }

        }
        else
        {
            Query2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                            SELECT 
                            [TB_SALES_ASSINGED_CN].[ID]
                            ,[SALES]
                            ,[CLIENTS]
                            ,[EVENTS]
                            ,CONVERT(NVARCHAR,[EDAYS],111) EDAYS
                            ,[ISCLOSE]
                            ,CONVERT(NVARCHAR,[ADDDATES],111) ADDDATES
                            ,(SELECT TOP 1 [COMMENTS] FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS_CN] WHERE [TB_SALES_ASSINGED_COMMENTS_CN].MID=[TB_SALES_ASSINGED_CN].ID ORDER BY ID DESC) AS COMMENTS
                            ,(SELECT TOP 1 CONVERT(NVARCHAR,[ADDDATES],111) FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS_CN] WHERE [TB_SALES_ASSINGED_COMMENTS_CN].MID=[TB_SALES_ASSINGED_CN].ID ORDER BY ID DESC) AS ADDDATES
                            FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN]
                            WHERE 1=1
                            {0}
                            {1}
                            ORDER BY [SALES],[CLIENTS],[EDAYS],[ID]

                              
                            ", Query1.ToString(), Query2.ToString()); ;


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

    protected void Grid2_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid2Button1")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引          

            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid2.Rows[rowIndex];
                TextBox TextBox_SALES = (TextBox)row.FindControl("業務員");
                string SALES = TextBox_SALES.Text;
                TextBox TextBox_CLIENTS = (TextBox)row.FindControl("客戶");
                string CLIENTS = TextBox_CLIENTS.Text;
                TextBox TextBox_EDAYS = (TextBox)row.FindControl("回覆期限");
                string EDAYS = TextBox_EDAYS.Text;
                TextBox TextBox_EVENTS = (TextBox)row.FindControl("專案內容");
                string EVENTS = TextBox_EVENTS.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID");
                string id = txtid.Text;

                UPDAT_TB_SALES_ASSINGED_CN(
                                       id
                                      , SALES
                                      , CLIENTS
                                      , EVENTS
                                      , EDAYS
                                      );
                MsgBox(id + " 完成", this.Page, this);

                BindGrid2();
            }
        }
    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL(txtDate1.Text, txtDate2.Text);
    }

    public void UPDAT_TB_SALES_ASSINGED_CN(
                                    string ID
                                   , string SALES
                                   , string CLIENTS
                                   , string EVENTS
                                   , string EDAYS
                                   )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"          
                UPDATE [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN]
                SET [SALES]=@SALES,[CLIENTS]=@CLIENTS,[EVENTS]=@EVENTS,[EDAYS]=@EDAYS
                WHERE [ID]=@ID
                      
                        ";

        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@SALES", SALES);
        m_db.AddParameter("@CLIENTS", CLIENTS);
        m_db.AddParameter("@EVENTS", EVENTS);
        m_db.AddParameter("@EDAYS", EDAYS);


        m_db.ExecuteNonQuery(cmdTxt);
    }

    private void BindGrid3()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder Query1 = new StringBuilder();
        StringBuilder Query2 = new StringBuilder();

        if (!string.IsNullOrEmpty(TextBox_CLIENTS3.Text))
        {
            Query1.AppendFormat(@" AND  [TB_SALES_ASSINGED_CN].[ID] IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN] WHERE [CLIENTS] LIKE '%{0}%') ", TextBox_CLIENTS3.Text);
        }
        else
        {
            Query1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(DropDownListISCLOSE2.SelectedValue.ToString()))
        {
            if (DropDownListISCLOSE3.SelectedValue.ToString().Equals("全部"))
            {
                Query2.AppendFormat(@"");
            }
            else
            {
                Query2.AppendFormat(@"AND [TB_SALES_ASSINGED_CN].[ID]  IN (SELECT ID FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN] WHERE [ISCLOSE] LIKE '%{0}%')", DropDownListISCLOSE3.SelectedValue.ToString());
            }

        }
        else
        {
            Query2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                           SELECT 
                            [TB_SALES_ASSINGED_CN].[ID]
                            ,[SALES]
                            ,[CLIENTS]
                            ,[EVENTS]
                            ,CONVERT(NVARCHAR,[EDAYS],111) EDAYS
                            ,[ISCLOSE]
                            ,[MID]
                            ,[COMMENTS]
                            ,CONVERT(NVARCHAR,[TB_SALES_ASSINGED_COMMENTS_CN].[ADDDATES] ,111)  ADDDATES
                            FROM [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_CN], [TKBUSINESS].[dbo].[TB_SALES_ASSINGED_COMMENTS_CN]
                            WHERE 1=1
                            AND  [TB_SALES_ASSINGED_COMMENTS_CN].MID=[TB_SALES_ASSINGED_CN].ID                           
                            {0}
                            {1}
                            ORDER BY [SALES],[CLIENTS],[EDAYS],[ID],[TB_SALES_ASSINGED_COMMENTS_CN].[ADDDATES]

                              
                            ", Query1.ToString(), Query2.ToString()); ;


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

    }

    protected void Grid3_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid3Button1")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引          

            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid3.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txtNewField3");
                string newTextValue = txtNewField.Text;

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("ID3");
                string id = txtid.Text;

                ADD_TB_SALES_ASSINGED_COMMENTS_CN(id, newTextValue);

                //MsgBox(id + " " + newTextValue, this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid3();
            }
        }


    }

    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL(txtDate1.Text, txtDate2.Text);

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
    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void btn2_Click(object sender, EventArgs e)
    {
        string SALES = DropDownList1.SelectedValue.ToString();
        string CLIENTS = TextBox1.Text.ToString();
        string EVENTS = TextBox2.Text.ToString();
        string EDAYS = txtDate1.Text.ToString();
        string ISCLOSE = DropDownList2.SelectedValue.ToString();
        string ADDDATES = DateTime.Now.ToString("yyyy/MM/dd");

        ADD_TB_SALES_ASSINGED_CN(
                                SALES
                                , CLIENTS
                                , EVENTS
                                , EDAYS
                                , ISCLOSE
                                , ADDDATES
                                );
        BindGrid();

        MsgBox(" 完成", this.Page, this);
    }

    protected void btn3_Click(object sender, EventArgs e)
    {
        BindGrid2();
    }
    protected void btn4_Click(object sender, EventArgs e)
    {
        BindGrid3();
    }

    #endregion
}