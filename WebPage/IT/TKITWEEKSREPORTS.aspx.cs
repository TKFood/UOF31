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

public partial class CDS_WebPage_IT_TKITWEEKSREPORTS : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SETYEARSWEEKS();
            BindDropDownList();
            BindDropDownList2();

            BindGrid1("");

        }
        else
        {


        }




    }
    #region FUNCTION
    public void SETYEARSWEEKS()
    {
        int YEARS = DateTime.Now.Year;
        // 假設現在是 2026/01/05
        DateTime now = DateTime.Now;

        System.Globalization.Calendar cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;

        // 使用 FirstFullWeek：第一週必須包含完整的 7 天（從週一到週日）都在該年份
        int WEEKS = cal.GetWeekOfYear(
            now,
            System.Globalization.CalendarWeekRule.FirstFullWeek,
            DayOfWeek.Monday
        );

        // 結果：
        // 2026/01/01 (四) -> 屬於 2025 年的最後一週 (回傳 52 或 53)
        // 2026/01/05 (一) -> 正式成為 2026 年的第 1 週
        

        TextBox1.Text = YEARS.ToString();
        //TextBox2.Text = WEEKS.ToString();
        TextBox3.Text = YEARS.ToString();
        TextBox4.Text = WEEKS.ToString();
        TextBox9.Text = WEEKS.ToString();
       
        // 1. 計算今天距離「本週週一」差幾天
        // DayOfWeek 在 C# 中週日=0, 週一=1...週六=6
        // 如果今天是週日 (0)，我們要減 6 天才會到週一
        // 如果今天是週一 (1) 到週六 (6)，我們要減去 (1-1) 到 (6-1) 天
        int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
        DateTime startDate = now.AddDays(-1 * diff).Date;

        // 2. 週日就是週一再加上 6 天
        DateTime lastDate = startDate.AddDays(6);

        // 輸出結果
        TextBox6.Text = startDate.ToString("yyyy/MM/dd");
        TextBox7.Text = lastDate.ToString("yyyy/MM/dd");
    }
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("NAMES", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT [ID] ,[NAMES]  FROM [TKIT].[dbo].[ITWEEKSREPORTSNAMES] WHERE ISUSED='Y' ORDER BY ID ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "NAMES";
            DropDownList1.DataValueField = "NAMES";
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
        dt.Columns.Add("STATUS", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT [ID],[FORM],[STATUS] FROM [TKIT].[dbo].[ITPARAS] WHERE [FORM]='週報' ORDER BY ID";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "STATUS";
            DropDownList2.DataValueField = "STATUS";
            DropDownList2.DataBind();

        }
        else
        {

        }
    }
    protected void TextBox3_TextChanged(object sender, EventArgs e)
    {
        int YEARS = Convert.ToInt32(TextBox3.Text.ToString());
        int WEEKS = Convert.ToInt32(TextBox4.Text.ToString());
        DateTime startDate, lastDate;

        GetDaysOfWeeks(YEARS, WEEKS, out startDate, out lastDate);
        TextBox6.Text = startDate.ToString("yyyy/MM/dd");
        TextBox7.Text = lastDate.ToString("yyyy/MM/dd");
    }
    protected void TextBox4_TextChanged(object sender, EventArgs e)
    {
        int YEARS = Convert.ToInt32(TextBox3.Text.ToString());
        int WEEKS = Convert.ToInt32(TextBox4.Text.ToString());
        DateTime startDate, lastDate;

        GetDaysOfWeeks(YEARS, WEEKS, out startDate, out lastDate);
        TextBox6.Text = startDate.ToString("yyyy/MM/dd");
        TextBox7.Text = lastDate.ToString("yyyy/MM/dd");
    }
    /// <summary>
    /// 查詢 年度 周次的起、迄日 
    /// </summary>
    /// <param name="year"></param>
    /// <param name="index"></param>
    /// <param name="first"></param>
    /// <param name="last"></param>
    /// <returns></returns>
    public static bool GetDaysOfWeeks(int year, int index, out DateTime first, out DateTime last)
    {
        first = DateTime.MinValue;
        last = DateTime.MinValue;
        if (year < 1700 || year > 9999)
        {
            //"年份超限"
            return false;
        }
        if (index < 1 || index > 53)
        {
            //"週數錯誤"
            return false;
        }
        DateTime startDay = new DateTime(year, 1, 1); //該年第一天
        DateTime endDay = new DateTime(year + 1, 1, 1).AddMilliseconds(-1);
        int dayOfWeek = 0;
        if (Convert.ToInt32(startDay.DayOfWeek.ToString("d")) > 0)
            dayOfWeek = Convert.ToInt32(startDay.DayOfWeek.ToString("d")); //該年第一天為星期幾
        if (dayOfWeek == 0) { dayOfWeek = 7; }
        if (index == 1)
        {
            first = startDay.AddDays(7 - dayOfWeek - 6);
            if (dayOfWeek == 6)
            {
                last = first;
            }
            else
            {
                last = startDay.AddDays((7 - dayOfWeek));
            }
        }
        else
        {
            first = startDay.AddDays((8 - dayOfWeek) + (index - 2) * 7); //index周的起始日期
            last = first.AddDays(6);
            //if (last > endDay)
            //{
            //  last = endDay;
            //}
        }
        if (first > endDay) //startDayOfWeeks不在該年範圍內
        {
            //"輸入週數大於本年最大週數";
            return false;
        }
        return true;
    }
   

    private void BindGrid1(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        



        //查詢條件
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND [WYEARS]='{0}'", TextBox1.Text);
        }

        //週別
        if (!string.IsNullOrEmpty(TextBox9.Text))
        {
            QUERYS.AppendFormat(@" AND [WEEKS]='{0}' ", TextBox9.Text);
        }
        else if (string.IsNullOrEmpty(TextBox9.Text))
        {
            QUERYS.AppendFormat(@" ");
        }

        if (!string.IsNullOrEmpty(DropDownList2.Text.ToString()))
        {
            if(DropDownList2.Text.ToString().Equals("未核准"))
            {
                QUERYS.AppendFormat(@" AND [ADMITCHECKS]='{0}'", DropDownList2.Text.ToString());
            }
            else if (DropDownList2.Text.ToString().Equals("已核准"))
            {
                QUERYS.AppendFormat(@" AND [ADMITCHECKS]='{0}'", DropDownList2.Text.ToString());
            }
            else
            {
                QUERYS.AppendFormat(@" ");
            }
            
        }

        cmdTxt.AppendFormat(@" 
                            SELECT 
                            [ID]
                            ,[NAMES]
                            ,[WYEARS]
                            ,[WEEKS]
                            ,CONVERT(NVARCHAR,[SDATES],111) AS SDATES
                            ,CONVERT(NVARCHAR,[EDATES],111) AS EDATES
                            
                            ,REPLACE([COMMENTS] ,char(10),'<br/>') AS [COMMENTS] 
                            ,REPLACE([NOTFINISHEDS] ,char(10),'<br/>') AS [NOTFINISHEDS]  
                            ,REPLACE([PLANWORKS] ,char(10),'<br/>')  AS [PLANWORKS]
                            ,[ADMITCHECKS] 

                            FROM [TKIT].[dbo].[ITWEEKSREPORTS]
                            WHERE  1=1
                            {0}

                            ORDER BY [NAMES],[WYEARS],[WEEKS]
                               
                                ", QUERYS.ToString());




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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ///Get the button that raised the event
            Button btn = (Button)e.Row.FindControl("GWButton1");
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue = btn.CommandArgument;
            DataRowView row = (DataRowView)e.Row.DataItem;
            Button lbtnName = (Button)e.Row.FindControl("GWButton1");
            ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            // Dialog.PostBackType.AfterReturn
            //Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            ///Get the button that raised the event
            Button btn2 = (Button)e.Row.FindControl("GWButton2");
            //Get the row that contains this button
            GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue2 = btn2.CommandArgument;
            DataRowView row2 = (DataRowView)e.Row.DataItem;
            Button lbtnName2 = (Button)e.Row.FindControl("GWButton2");
            ExpandoObject param2 = new { ID = Cellvalue }.ToExpando();
                    
            Dialog.Open2(lbtnName2, "~/CDS/WebPage/IT/TKITWEEKSREPORTSDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param2);

            ///Get the button that raised the event
            Button btn3 = (Button)e.Row.FindControl("GWButton3");
            //Get the row that contains this button
            GridViewRow gvr3 = (GridViewRow)btn3.NamingContainer;
          
            string Cellvalue3 = btn3.CommandArgument;
            DataRowView row3 = (DataRowView)e.Row.DataItem;
            Button lbtnName3 = (Button)e.Row.FindControl("GWButton3");
            ExpandoObject param3 = new { ID = Cellvalue3 }.ToExpando();

            string ADMITCHECKS = gvr.Cells[8].Text.Trim();
            if (ADMITCHECKS.Equals("已核准"))
            {
                btn.Enabled = false;
                btn2.Enabled = false;
            }
            else
            {
                btn.Enabled = true;
                btn2.Enabled = true;
            }
           



        }

    }
    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "GWButton1")
        {
            DELETEITWEEKSREPORTS(e.CommandArgument.ToString());

            BindGrid1("");
            MsgBox(e.CommandArgument.ToString()+" 已刪除", this.Page, this);

           
        }
      
        if (e.CommandName == "GWButton2")
        {
            BindGrid1("");
            MsgBox(e.CommandArgument.ToString() + " 已更新", this.Page, this);
        }

        if (e.CommandName == "GWButton3")
        {
            UPDATEITWEEKSREPORTSADMITCHECKS(e.CommandArgument.ToString());

            BindGrid1("");
            MsgBox(e.CommandArgument.ToString() + " 已核准", this.Page, this);


        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

        SETEXCEL();

        //string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //StringBuilder cmdTxt = new StringBuilder();
        //StringBuilder QUERYS = new StringBuilder();

        ////建議售價
        //if (!string.IsNullOrEmpty(TextBox1.Text))
        //{
        //    QUERYS.AppendFormat(@" AND DOC_NBR LIKE '%{0}%'", TextBox1.Text);
        //}


        //cmdTxt.AppendFormat(@" 
        //                    SELECT
        //                    usr2.NAME
        //                    ,(CASE WHEN  usr.IS_SUSPENDED = 1 THEN  usr.NAME + '(x)' WHEN  ISNULL(usr.ACCOUNT,'''') = '' THEN  'unknown user' ELSE usr.NAME END) AS APPLICANT_NAM
        //                    ,form.FORM_NAME
        //                    ,DOC_NBR
        //                    ,CONVERT(NVARCHAR,NODES.START_TIME,111) AS 'START_TIME'
        //                    ,DATEDIFF(HOUR,START_TIME,GETDATE()) AS 'HRS'
        //                    ,CONVERT(NVARCHAR,BEGIN_TIME,111) AS BEGIN_TIME

        //                    ,task.TASK_ID
        //                    ,END_TIME
        //                    ,TASK_RESULT
        //                    ,TASK_STATUS
        //                    ,task.USER_GUID
        //                    ,formVer.FORM_VERSION_ID
        //                    ,formVer.FORM_ID
        //                    ,CURRENT_SITE_ID
        //                    ,MESSAGE_CONTENT
        //                    ,LOCK_STATUS
        //                    ,ISNULL(formVer.DISPLAY_TITLE,'') AS VERSION_TITLE
        //                    ,ISNULL(task.JSON_DISPLAY,'') AS JSON_DISPLAY

        //                    FROM [UOF].dbo.TB_WKF_TASK task
        //                    INNER JOIN [UOF].dbo.TB_WKF_FORM_VERSION formVer ON task.FORM_VERSION_ID = formVer.FORM_VERSION_ID
        //                    INNER JOIN [UOF].dbo.TB_WKF_FORM form  ON  formVer.FORM_ID = form.FORM_ID 
        //                    LEFT JOIN [UOF].dbo.TB_EB_USER [usr]  ON task.USER_GUID = usr.USER_GUID
        //                    LEFT JOIN [UOF].dbo.TB_WKF_TASK_NODE [NODES] ON NODES.SITE_ID=task.CURRENT_SITE_ID
        //                    LEFT JOIN [UOF].dbo.TB_EB_USER [usr2]  ON NODES.ORIGINAL_SIGNER = [usr2].USER_GUID
        //                    WHERE
        //                    1=1  AND  TASK_STATUS NOT IN ('2')
        //                        {0}
        //                    ORDER BY HRS DESC,usr2.NAME,form.FORM_NAME,DOC_NBR

        //                        ", QUERYS.ToString());



        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        //if (dt.Rows.Count > 0)
        //{
        //    dt.Columns[0].Caption = "目前簽核者";
        //    dt.Columns[1].Caption = "申請者";
        //    dt.Columns[2].Caption = "表單";
        //    dt.Columns[3].Caption = "表單編號";
        //    dt.Columns[4].Caption = "送簽核者時間";
        //    dt.Columns[5].Caption = "停留時間(小時)";
        //    dt.Columns[6].Caption = "申請時間";



        //    e.Datasource = dt;
        //}
    }

    public void SETEXCEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@"", TextBox1.Text);
        }


        cmdTxt.AppendFormat(@" 
                            
                               
                                ", QUERYS.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "未結案表單" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
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

    public void ADDITWEEKSREPORTS(string NAMES, string WYEARS, string WEEKS, string SDATES, string EDATES, string COMMENTS,string NOTFINISHEDS, string PLANWORKS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        INSERT INTO [TKIT].[dbo].[ITWEEKSREPORTS]
                        (
                        NAMES
                        ,WYEARS
                        ,WEEKS
                        ,SDATES
                        ,EDATES
                        ,COMMENTS
                        ,NOTFINISHEDS
                        ,PLANWORKS
                        ,ADMITCHECKS
                        )
                        VALUES
                        (
                        @NAMES
                        ,@WYEARS
                        ,@WEEKS
                        ,@SDATES
                        ,@EDATES
                        ,@COMMENTS
                        ,@NOTFINISHEDS
                        ,@PLANWORKS
                        ,@ADMITCHECKS
                        )
                   
                            ";


        m_db.AddParameter("@NAMES", NAMES);
        m_db.AddParameter("@WYEARS", WYEARS);
        m_db.AddParameter("@WEEKS", WEEKS);
        m_db.AddParameter("@SDATES", SDATES);
        m_db.AddParameter("@EDATES", EDATES);
        m_db.AddParameter("@COMMENTS", COMMENTS);
        m_db.AddParameter("@NOTFINISHEDS", NOTFINISHEDS);
        m_db.AddParameter("@PLANWORKS", PLANWORKS);
        m_db.AddParameter("@ADMITCHECKS", "未核准");

        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void DELETEITWEEKSREPORTS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        DELETE [TKIT].[dbo].[ITWEEKSREPORTS]
                        WHERE ID=@ID
                            ";


        m_db.AddParameter("@ID", ID);


        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void UPDATEITWEEKSREPORTSADMITCHECKS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        UPDATE [TKIT].[dbo].[ITWEEKSREPORTS]
                        SET [ADMITCHECKS]='已核准'
                        WHERE ID=@ID
                            ";


        m_db.AddParameter("@ID", ID);


        m_db.ExecuteNonQuery(cmdTxt);
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
        RadTabStrip1.SelectedIndex = 0;

        RadPageView1.Selected = true;



        ADDITWEEKSREPORTS(DropDownList1.Text.ToString(), TextBox3.Text.ToString(), TextBox4.Text.ToString(), TextBox6.Text.ToString(), TextBox7.Text.ToString(), TextBox5.Text.ToString(),TextBox2.Text.ToString(), TextBox8.Text.ToString());
        BindGrid1("");
     

        MsgBox("完成", this.Page, this);

        



    }


    #endregion
}