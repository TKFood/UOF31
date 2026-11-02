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
using System.Globalization;

public partial class CDS_WebPage_CUSTOMERIZE_TK_INVLA_QUERY1 : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SETYEARSWEEKS();
            BindGrid1("");

        }
        else
        {


        }




    }
    #region FUNCTION
  
    //private void BindDropDownList()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("ID", typeof(String));
    //    dt.Columns.Add("NAMES", typeof(String));

    //    string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
    //    Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

    //    string cmdTxt = @"SELECT '未完成' AS KINDS UNION ALL SELECT '全部' ";

    //    dt.Load(m_db.ExecuteReader(cmdTxt));

    //    if (dt.Rows.Count > 0)
    //    {
    //        DropDownList1.DataSource = dt;
    //        DropDownList1.DataTextField = "KINDS";
    //        DropDownList1.DataValueField = "KINDS";
    //        DropDownList1.DataBind();

    //    }
    //    else
    //    {

    //    }
    //}


    public void SETYEARSWEEKS()
    {
        txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
        txtDate2.Text = DateTime.Now.ToString("yyyy/MM/dd");
    }

    private void BindGrid1(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        DateTime SDATES = Convert.ToDateTime(txtDate1.Text);
        DateTime EDATES = Convert.ToDateTime(txtDate2.Text);
        string MB001 = TextBox1.Text;


        //查詢條件
        //if (!string.IsNullOrEmpty(TextBox1.Text))
        //{
        //    QUERYS.AppendFormat(@"  AND TB_EIP_SCH_WORK.SUBJECT LIKE '%{0}%'", TextBox1.Text);
        //    QUERYS.AppendFormat(@"  AND TB_EIP_SCH_DEVOLVE.SUBJECT LIKE  '%{0}%'", TextBox1.Text);
        //}    
        //else if (DropDownList1.Text.Equals("全部"))
        //{
        //    QUERYS.AppendFormat(@"  ");
        //}

        if(!string.IsNullOrEmpty(MB001))
        {
            cmdTxt.AppendFormat(@"
                            SELECT 品號,品名,規格,單位,年月,最近進貨價,SUM(FILEDS1) AS '1進貨/入庫',SUM(FILEDS2) AS '2銷貨',SUM(FILEDS3) AS '3領用',SUM(FILEDS4) AS '4組合領用',SUM(FILEDS5) AS '5組合生產'
                            FROM
                            (
                            SELECT NEWMQ008 AS '分類',LA001 AS '品號',SUBSTRING(LA004,1,6)  AS '年月',SUM(LA005*LA011)  AS '數量',INVMB.MB002 AS '品名',INVMB.MB003 AS '規格',INVMB.MB004 AS '單位'
                            ,CONVERT(DECIMAL(16,2),INVMB.MB050) AS '最近進貨價'
                            ,(CASE WHEN NEWMQ008 IN ('1進貨/入庫') THEN SUM(LA005*LA011) ELSE 0 END ) AS 'FILEDS1'
                            ,(CASE WHEN NEWMQ008 IN ('2銷貨') THEN SUM(LA005*LA011) ELSE 0 END ) AS 'FILEDS2'
                            ,(CASE WHEN NEWMQ008 IN ('3領用') THEN SUM(LA005*LA011) ELSE 0 END ) AS 'FILEDS3'
                            ,(CASE WHEN NEWMQ008 IN ('4組合領用') THEN SUM(LA005*LA011) ELSE 0 END ) AS 'FILEDS4'
                            ,(CASE WHEN NEWMQ008 IN ('5組合生產') THEN SUM(LA005*LA011) ELSE 0 END ) AS 'FILEDS5'

                            FROM 
                            (
                            SELECT MQ001,MQ002,MQ008,LA001,LA004,LA005,LA006,LA007,LA008,LA011,MB002,MB003,MB004
                            ,CASE  WHEN MQ001 IN ('A421','A422','A431') AND LA005=-1 THEN '4組合領用' WHEN MQ001 IN ('A421','A422','A431') AND LA005=1 THEN '5組合生產'  WHEN MQ008='1' THEN '1進貨/入庫'  WHEN (MQ008='2' OR (ISNULL(MQ008,'')='' AND LA005=-1))THEN '2銷貨'  WHEN MQ008='3' THEN '3領用' END NEWMQ008 
                            FROM [TK].dbo.INVLA WITH(NOLOCK)
                            LEFT JOIN [TK].dbo.CMSMQ WITH(NOLOCK) ON LA006=MQ001
                            ,[TK].dbo.INVMB WITH(NOLOCK)
                            WHERE LA001=MB001
                            AND LA004>='{1}' AND LA004<='{2}'

                            AND (LA001 LIKE  '%{0}%' OR MB002 LIKE '%{0}%')
                            ) AS TEMP,[TK].dbo.INVMB
                            WHERE LA001=MB001

                            GROUP BY LA001,NEWMQ008,SUBSTRING(LA004,1,6),INVMB.MB002,INVMB.MB003,INVMB.MB004,INVMB.MB050
                            ) AS TMPE2
                            GROUP BY 品號,品名,規格,單位,年月,最近進貨價
                            ORDER BY 品號,品名,規格,單位,年月,最近進貨價


                               
                                ", MB001, SDATES.ToString("yyyyMMdd"), EDATES.ToString("yyyyMMdd"));


        }
        else
        {
            cmdTxt.AppendFormat(@"

                                ");
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
        //    ///Get the button that raised the event
        //    Button btn = (Button)e.Row.FindControl("GWButton1");
        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        //    //string cellvalue = gvr.Cells[2].Text.Trim();
        //    string Cellvalue = btn.CommandArgument;
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    Button lbtnName = (Button)e.Row.FindControl("GWButton1");
        //    ExpandoObject param = new { ID = Cellvalue }.ToExpando();
        //    //Grid開窗是用RowDataBound事件再開窗          

        //    Dialog.Open2(lbtnName, "~/CDS/WebPage/CUSTOMERIZE/TK_SCH_DEVOLVEDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);


        //}

    }
    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        //if (e.CommandName == "GWButton1")
        //{

        //    BindGrid1("");

        //}
      
       
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

        SETEXCEL();

    }

    public void SETEXCEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //
        //if (!string.IsNullOrEmpty(TextBox1.Text))
        //{
        //    QUERYS.AppendFormat(@"", TextBox1.Text);
        //}


        cmdTxt.AppendFormat(@" 
                            
                               
                                ", QUERYS.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "資料" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
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
                ws.Cells[1, 1].Value = "目前簽核者";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線


                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["NAME"].ToString();
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
    #endregion

    #region BUTTON

    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid1("");
    }

   


    #endregion
}