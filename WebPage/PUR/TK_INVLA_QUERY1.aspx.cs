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

public partial class CDS_WebPage_PUR_TK_INVLA_QUERY1 : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SETYEARSWEEKS();
           

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
        txtDate1.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy/MM/dd");
        txtDate2.Text = DateTime.Now.ToString("yyyy/MM/dd");
        txtDate3.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy/MM/dd");
        txtDate4.Text = DateTime.Now.ToString("yyyy/MM/dd");

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
            string NEWMB001 = MB001.ToUpper();

            cmdTxt.AppendFormat(@"                               
                               SELECT 
                                t2.品號, 
                                t2.品名, 
                                t2.規格, 
                                t2.單位, 
                                invla_sum.目前庫存量,
                                t2.年月, 
                                t2.最近進貨價, 
                                t2.最低補量,
                                NULLIF(SUM(t2.FILEDS1),0) AS '1進貨/入庫',
                                NULLIF(SUM(t2.FILEDS2),0) AS '2銷貨',
                                NULLIF(SUM(t2.FILEDS3),0) AS '3領用',
                                NULLIF(SUM(t2.FILEDS4),0) AS '4組合領用',
                                NULLIF(SUM(t2.FILEDS5),0) AS '5組合生產',
                                CONVERT(decimal(16,2),MB047)  AS '標準售價',
                                CONVERT(decimal(16,2),MB051)  AS '零售價',
                                CASE WHEN SUM(t2.FILEDS2)<0 THEN ISNULL(avg_sale.平均售價,0) ELSE 0 END AS 平均售價,
                                半成品進價.進價 AS 半成品進價,
                                最近成本.AVGCOSTS AS 最近成本,
                                t2.員購數量
                            FROM
                            (
                                SELECT 
                                    NEWMQ008 AS 分類,
                                    LA001 AS 品號,
                                    SUBSTRING(LA004,1,6)  AS 年月,
                                    INVMB.MB002 AS 品名,
                                    INVMB.MB003 AS 規格,
                                    INVMB.MB004 AS 單位,
                                    CONVERT(DECIMAL(16,2),INVMB.MB050) AS 最近進貨價,
                                    CONVERT(DECIMAL(16,0),INVMB.MB039) AS 最低補量,
                                    CASE WHEN NEWMQ008='1進貨/入庫' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS1,
                                    CASE WHEN NEWMQ008='2銷貨' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS2,
                                    CASE WHEN NEWMQ008='3領用' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS3,
                                    CASE WHEN NEWMQ008='4組合領用' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS4,
                                    CASE WHEN NEWMQ008='5組合生產' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS5,
                                    (
                                        SELECT SUM(LA016)
                                        FROM [TK].dbo.SASLA s
                                        WHERE s.LA043 IN (SELECT SASLA_LA043 FROM [TKPUR].[dbo].[TK_SASLA_LA043])
                                          AND s.LA005=INVLA.LA001
                                          AND LEFT(CONVERT(NVARCHAR(8),s.LA015,112),6)=LEFT(CONVERT(NVARCHAR(8),INVLA.LA004,112),6)
                                    ) AS 員購數量
                                FROM [TK].dbo.INVLA INVLA WITH(NOLOCK)
                                LEFT JOIN [TK].dbo.CMSMQ MQ WITH(NOLOCK) ON LA006=MQ001
                                INNER JOIN [TK].dbo.INVMB INVMB WITH(NOLOCK) ON LA001=MB001
                                CROSS APPLY (
                                    SELECT CASE  
                                        WHEN MQ001 IN ('A421','A422','A431') AND LA005=-1 THEN '4組合領用'
                                        WHEN MQ001 IN ('A421','A422','A431') AND LA005=1 THEN '5組合生產'
                                        WHEN MQ008='1' THEN '1進貨/入庫'
                                        WHEN MQ008='2' OR (ISNULL(MQ008,'')='' AND LA005=-1) THEN '2銷貨'
                                        WHEN MQ008='3' THEN '3領用'
                                    END AS NEWMQ008
                                ) x
                                WHERE LA004 BETWEEN '{1}' AND '{2}'
                                  AND (LA001 LIKE  '%{0}%' OR UPPER(MB002) LIKE '%{0}%')
                                GROUP BY LA001,MB002,MB003,MB004,MB050,MB039,NEWMQ008,SUBSTRING(LA004,1,6)
                            ) t2
                            LEFT JOIN [TK].dbo.INVMB b ON b.MB001=t2.品號
                            OUTER APPLY (
                                SELECT SUM(LA005*LA011) AS 目前庫存量
                                FROM TK.dbo.INVLA la
                                WHERE la.LA001=t2.品號
                            ) invla_sum
                            OUTER APPLY (
                                SELECT ISNULL(CONVERT(DECIMAL(16,2),SUM(LA017-LA020-LA021-LA022-LA023)/NULLIF(SUM(LA016+LA025-LA019),0)),0) AS 平均售價
                                FROM [TK].dbo.SASLA s
                                WHERE s.LA005=t2.品號
                                  AND LEFT(CONVERT(NVARCHAR(8),s.LA015,112),6)=t2.年月
                                  AND (LA017-LA020-LA021-LA022-LA023)>0
                                  AND (LA016+LA025-LA019)>0
                            ) avg_sale
                            OUTER APPLY (
                                SELECT TOP 1 CONVERT(DECIMAL(16,2),CASE WHEN NUMS>0 AND TOTALCOSTS>0 THEN TOTALCOSTS/NUMS ELSE 0 END) AS AVGCOSTS
                                FROM (
                                    SELECT (ME003+ME004+ME005) AS NUMS,(ME007+ME008+ME009+ME010) AS TOTALCOSTS
                                    FROM [TK].dbo.CSTME
                                    WHERE ME001=t2.品號
                                ) c
                                ORDER BY c.NUMS DESC
                            ) 最近成本
                            OUTER APPLY (
                                SELECT TOP 1 CONVERT(NVARCHAR,ISNULL(CONVERT(decimal(16,2),MB050),0))+' /'+MB004 AS 進價
                                FROM [TK].dbo.INVMB mb
                                WHERE mb.MB050>0
                                  AND mb.MB001 IN (
                                      SELECT MD003 FROM [TK].dbo.BOMMD WHERE MD003 LIKE '3%' AND MD001=t2.品號
                                  )
                            ) 半成品進價
                            GROUP BY t2.品號,t2.品名,t2.規格,t2.單位,invla_sum.目前庫存量,
                                     t2.年月,t2.最近進貨價,MB047,MB051,t2.員購數量,t2.最低補量,
                                     半成品進價.進價,最近成本.AVGCOSTS,avg_sale.平均售價
                            ORDER BY t2.品號,t2.品名,t2.規格,t2.單位,t2.年月,t2.最近進貨價;
                               
                                ", NEWMB001, SDATES.ToString("yyyyMMdd"), EDATES.ToString("yyyyMMdd"));


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

    private void BindGrid2(string LA001,string SDATES,string EDATES)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

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


        cmdTxt.AppendFormat(@"                               
                            SELECT 
                            t2.品號, 
                            t2.品名, 
                            t2.規格, 
                            t2.單位, 
                            invla_sum.目前庫存量,
                            t2.最近進貨價, 
                            t2.最低補量,
                            NULLIF(SUM(t2.FILEDS1),0) / NULLIF(月份數.月份數,0) AS '1進貨/入庫',
                            NULLIF(SUM(t2.FILEDS2),0) / NULLIF(月份數.月份數,0) AS '2銷貨',
                            NULLIF(SUM(t2.FILEDS3),0) / NULLIF(月份數.月份數,0) AS '3領用',
                            NULLIF(SUM(t2.FILEDS4),0) / NULLIF(月份數.月份數,0) AS '4組合領用',
                            NULLIF(SUM(t2.FILEDS5),0) / NULLIF(月份數.月份數,0) AS '5組合生產',
                            CONVERT(decimal(16,2),MB047)  AS '標準售價',
                            CONVERT(decimal(16,2),MB051)  AS '零售價',
                            CASE WHEN SUM(t2.FILEDS2)<0 THEN ISNULL(avg_sale.平均售價,0) ELSE 0 END AS 平均售價,
                            半成品進價.進價 AS 半成品進價,
                            最近成本.AVGCOSTS AS 最近成本,
                            t2.員購數量 / NULLIF(月份數.月份數,0) AS 員購數量
                        FROM
                        (
                            SELECT 
                                NEWMQ008 AS 分類,
                                LA001 AS 品號,
                                INVMB.MB002 AS 品名,
                                INVMB.MB003 AS 規格,
                                INVMB.MB004 AS 單位,
                                CONVERT(DECIMAL(16,2),INVMB.MB050) AS 最近進貨價,
                                CONVERT(DECIMAL(16,0),INVMB.MB039) AS 最低補量,
                                CASE WHEN NEWMQ008='1進貨/入庫' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS1,
                                CASE WHEN NEWMQ008='2銷貨' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS2,
                                CASE WHEN NEWMQ008='3領用' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS3,
                                CASE WHEN NEWMQ008='4組合領用' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS4,
                                CASE WHEN NEWMQ008='5組合生產' THEN SUM(LA005*LA011) ELSE 0 END AS FILEDS5,
                                (
                                    SELECT SUM(LA016)
                                    FROM [TK].dbo.SASLA s
                                    WHERE s.LA043 IN (SELECT SASLA_LA043 FROM [TKPUR].[dbo].[TK_SASLA_LA043])
                                      AND s.LA005=INVLA.LA001
                                ) AS 員購數量,
                                SUBSTRING(LA004,1,6) AS 年月   -- 保留年月供月份數計算
                            FROM [TK].dbo.INVLA INVLA WITH(NOLOCK)
                            LEFT JOIN [TK].dbo.CMSMQ MQ WITH(NOLOCK) ON LA006=MQ001
                            INNER JOIN [TK].dbo.INVMB INVMB WITH(NOLOCK) ON LA001=MB001
                            CROSS APPLY (
                                SELECT CASE  
                                    WHEN MQ001 IN ('A421','A422','A431') AND LA005=-1 THEN '4組合領用'
                                    WHEN MQ001 IN ('A421','A422','A431') AND LA005=1 THEN '5組合生產'
                                    WHEN MQ008='1' THEN '1進貨/入庫'
                                    WHEN MQ008='2' OR (ISNULL(MQ008,'')='' AND LA005=-1) THEN '2銷貨'
                                    WHEN MQ008='3' THEN '3領用'
                                END AS NEWMQ008
                            ) x
                            WHERE LA004 BETWEEN '{1}' AND '{2}'
                              AND (LA001 LIKE  '%{0}%' OR UPPER(MB002) LIKE '%{0}%')
                            GROUP BY LA001,MB002,MB003,MB004,MB050,MB039,NEWMQ008,SUBSTRING(LA004,1,6)
                        ) t2
                        LEFT JOIN [TK].dbo.INVMB b ON b.MB001=t2.品號
                        OUTER APPLY (
                            SELECT SUM(LA005*LA011) AS 目前庫存量
                            FROM TK.dbo.INVLA la
                            WHERE la.LA001=t2.品號
                        ) invla_sum
                        OUTER APPLY (
                            SELECT ISNULL(
                                       CONVERT(DECIMAL(16,2),
                                               SUM(LA017-LA020-LA021-LA022-LA023) / NULLIF(SUM(LA016+LA025-LA019),0)
                                       ),0) AS 平均售價
                            FROM [TK].dbo.SASLA s
                            WHERE s.LA005=t2.品號
                              AND (LA017-LA020-LA021-LA022-LA023)>0
                              AND (LA016+LA025-LA019)>0
                        ) avg_sale
                        OUTER APPLY (
                            SELECT TOP 1 CONVERT(DECIMAL(16,2),CASE WHEN NUMS>0 AND TOTALCOSTS>0 THEN TOTALCOSTS/NUMS ELSE 0 END) AS AVGCOSTS
                            FROM (
                                SELECT (ME003+ME004+ME005) AS NUMS,(ME007+ME008+ME009+ME010) AS TOTALCOSTS
                                FROM [TK].dbo.CSTME
                                WHERE ME001=t2.品號
                            ) c
                            ORDER BY c.NUMS DESC
                        ) 最近成本
                        OUTER APPLY (
                            SELECT TOP 1 CONVERT(NVARCHAR,ISNULL(CONVERT(decimal(16,2),MB050),0))+' /'+MB004 AS 進價
                            FROM [TK].dbo.INVMB mb
                            WHERE mb.MB050>0
                              AND mb.MB001 IN (
                                  SELECT MD003 FROM [TK].dbo.BOMMD WHERE MD003 LIKE '3%' AND MD001=t2.品號
                              )
                        ) 半成品進價
                        OUTER APPLY (
                            SELECT COUNT(DISTINCT SUBSTRING(LA004,1,6)) AS 月份數
                            FROM [TK].dbo.INVLA v
                            WHERE v.LA001 = t2.品號
                              AND v.LA004 BETWEEN '{1}' AND '{2}'
                        ) 月份數
                        GROUP BY t2.品號,t2.品名,t2.規格,t2.單位,invla_sum.目前庫存量,
                                 t2.最近進貨價,MB047,MB051,t2.員購數量,t2.最低補量,
                                 半成品進價.進價,最近成本.AVGCOSTS,avg_sale.平均售價,月份數.月份數
                        ORDER BY t2.品號,t2.品名,t2.規格,t2.單位,t2.最近進貨價;

                               
                                ", LA001, SDATES, EDATES);


        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

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
       

    }
    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        //if (e.CommandName == "GWButton1")
        //{

        //    BindGrid1("");

        //}

    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

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


    protected void btn2_Click(object sender, EventArgs e)
    {
        DateTime DTSDATES = Convert.ToDateTime(txtDate3.Text);
        string SDATES = DTSDATES.ToString("yyyyMMdd");
        DateTime DTEDATES = Convert.ToDateTime(txtDate4.Text);
        string EDATES = DTEDATES.ToString("yyyyMMdd");

        string MB001 = TextBox2.Text;

        BindGrid2(MB001, SDATES, EDATES);
    }


    #endregion
}