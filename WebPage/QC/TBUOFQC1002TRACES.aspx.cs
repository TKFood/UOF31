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
using System.Xml;
using System.Xml.Linq;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Log;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Net.Mail;

public partial class CDS_WebPage_QC_TBUOFQC1002TRACES : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    DataTable EXCELDT1 = new DataTable();
    // 宣告一個全域變數存清單
    DataTable dtKinds1 = null;
    DataTable dtKinds2 = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            SETQUERY();
        }
    }

    #region FUNCTION
    public void SETQUERY()
    {
        // 取得今年這個月的第一天
        DateTime firstDayOfThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        Date1.Text = firstDayOfThisMonth.ToString("yyyy-MM-dd"); // TextMode="Date" 需要 yyyy-MM-dd 格式

        // 取得今年
        string firstDayOfThisYEARS = DateTime.Now.Year.ToString();
        Date2.Text = firstDayOfThisYEARS;
    }
    private void BindGrid()
    {
        // 1. 先抓好「分類」的清單
        dtKinds1 = GetKinds1List();
        dtKinds2 = GetKinds2List();

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        if (!string.IsNullOrEmpty(Date1.Text))
        {
            DateTime parsedDate;
            if (DateTime.TryParse(Date1.Text, out parsedDate))
            {
                string formatted = parsedDate.ToString("yyyy/MM/dd");
                QUERYS.AppendFormat(@"
                                    AND QCFrm002Date >= '{0}'
                                    ", formatted);
                                    }
        }
        else
        {

            QUERYS.AppendFormat(@"");
        }


        cmdTxt.AppendFormat(@"  
                            WITH TEMP AS (
                            SELECT 
                            [TB_WKF_FORM].[FORM_NAME]
                            ,[CURRENT_DOC]
                            ,TB_WKF_TASK.[DOC_NBR]
                            ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Date""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002Date
                            ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PRD""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002PRD
                            ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abns""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002Abns
                            ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abns""]/@customValue)[1]', 'NVARCHAR(100)') AS QCFrm002AbnscustomValue
                            ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002CUST""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002CUST
					        ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abn""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002Abn
                            ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PNO""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002PNO
                            ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Cmf""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002Cmf
                            , TB_WKF_TASK.TASK_ID
                            , (CASE WHEN TASK_STATUS = '1' THEN '簽核中' ELSE '已簽核' END) TASK_STATUS
                            , (CASE WHEN TASK_RESULT='0' THEN '已結案' ELSE '進行中' END ) TASK_RESULT
                            ,(SELECT TOP (1)
                            [TB_EB_USER].NAME
                            FROM [UOF].[dbo].[TB_WKF_TASK_NODE],[UOF].[dbo].[TB_EB_USER]
                            WHERE [TB_WKF_TASK_NODE].ORIGINAL_SIGNER=[TB_EB_USER].USER_GUID
                            AND ISNULL([FINISH_TIME],'')=''
                            AND TASK_ID= TB_WKF_TASK.TASK_ID
                            ORDER BY [NODE_SEQ]) AS 'ORIGINAL_SIGNER'
                            ,[TBUOFQC1002TRACES].[KINDS1]
                            ,[TBUOFQC1002TRACES].[KINDS2]
                            ,[TBUOFQC1002TRACES].[REASONS]
                            ,[TBUOFQC1002TRACES].[IMPROVES]
                            ,[TBUOFQC1002TRACES].[IMPROVESOWNER]
                            ,REPLACE(CONVERT(VARCHAR,[TBUOFQC1002TRACES].[IMPROVESDATES], 111), '/', '-') IMPROVESDATES

                            FROM [UOF].[dbo].TB_WKF_TASK
                            LEFT JOIN[UOF].[dbo].[TB_WKF_FORM_VERSION] ON[TB_WKF_FORM_VERSION].FORM_VERSION_ID = TB_WKF_TASK.FORM_VERSION_ID
                            LEFT JOIN[UOF].[dbo].[TB_WKF_FORM] ON[TB_WKF_FORM].FORM_ID = [TB_WKF_FORM_VERSION].FORM_ID
                            LEFT JOIN[192.168.1.105].[TKQC].[dbo].[TBUOFQC1002TRACES] ON[TBUOFQC1002TRACES].[DOC_NBR]=TB_WKF_TASK.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS
                            WHERE[FORM_NAME] = '1002.客訴異常處理單'
                            AND ISNULL(TASK_RESULT,'') NOT IN('1','2')

                            )
                            SELECT TEMP.*
                            FROM TEMP
                            WHERE 1=1
                            AND[DOC_NBR]>='QC1002250100001'
                            {0}
                            ORDER BY QCFrm002Date

                             ", QUERYS.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
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
        //BindGrid();
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // 所有的處理都集中在 DataRow 判定內
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var dataItem = e.Row.DataItem;

            // 1. 處理 HyperLink (TASK_ID)
            string taskId = DataBinder.Eval(dataItem, "TASK_ID") as string;
            HyperLink hlTask = (HyperLink)e.Row.FindControl("hlTask");
            if (hlTask != null)
            {
                if (!string.IsNullOrEmpty(taskId))
                {
                    hlTask.NavigateUrl = string.Format("https://eip.tkfood.com.tw/UOF/wkf/formuse/viewform.aspx?TASK_ID={0}", taskId);
                }
                else
                {
                    hlTask.Visible = false;
                }
            }

            // 2. 處理 Button2 (CommandArgument)
            Button btn2 = (Button)e.Row.FindControl("Button2");
            if (btn2 != null)
            {
                // 如果後續沒有用到 param2 做其他事，這段動態物件宣告可考慮移除或整合
                string cellValue2 = btn2.CommandArgument;
                // dynamic param2 = new { ID = cellValue2 }.ToExpando(); 
            }

            // 3. 處理下拉選單：大分類
            BindGridDropDown(e.Row, "ddl_大分類", dtKinds1, "KINDS1");

            // 4. 處理下拉選單：中分類
            BindGridDropDown(e.Row, "ddl_中分類", dtKinds2, "KINDS2");
        }
    }

    /// <summary>
    /// 通用的 GridView 下拉選單繫結方法
    /// </summary>
    private void BindGridDropDown(GridViewRow row, string controlId, DataTable dataSource, string dataFieldName)
    {
        DropDownList ddl = (DropDownList)row.FindControl(controlId);
        if (ddl != null && dataSource != null)
        {
            // 繫結選項
            ddl.DataSource = dataSource;
            ddl.DataTextField = "NAMES";
            ddl.DataValueField = "NAMES";
            ddl.DataBind();

            // 插入預設項
            ddl.Items.Insert(0, new ListItem("--- 請選擇 ---", ""));

            // 讀取並設定選取值
            object val = DataBinder.Eval(row.DataItem, dataFieldName);
            string currentVal = (val == null || val == DBNull.Value) ? "" : val.ToString().Trim();

            if (!string.IsNullOrEmpty(currentVal))
            {
                if (ddl.Items.FindByValue(currentVal) != null)
                {
                    ddl.SelectedValue = currentVal;
                }
                else
                {
                    // 如果值不在清單內，動態加入以避免報錯
                    ddl.Items.Add(new ListItem(currentVal, currentVal));
                    ddl.SelectedValue = currentVal;
                }
            }
        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Button2")
        {
            //MsgBox(e.CommandArgument.ToString() + "OK", this.Page, this);
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField_GV1_改善方案 = (TextBox)row.FindControl("txtNewField_GV1_改善方案");
                string newTextValue_GV1_改善方案 = txtNewField_GV1_改善方案.Text;                
                TextBox txtNewField_GV1_原因分析 = (TextBox)row.FindControl("txtNewField_GV1_原因分析");
                string newTextValue_GV1_原因分析 = txtNewField_GV1_原因分析.Text;
                TextBox txtNewField_GV1_改善負責單位 = (TextBox)row.FindControl("txtNewField_GV1_改善負責單位");
                string newTextValue_GV1_改善負責單位 = txtNewField_GV1_改善負責單位.Text;
                TextBox txtNewField_GV1_預計改善完成日 = (TextBox)row.FindControl("txtNewField_GV1_預計改善完成日");
                string newTextValue_GV1預計改善完成日 = txtNewField_GV1_預計改善完成日.Text;

                DropDownList ddl_GV1_大分類 = (DropDownList)row.FindControl("ddl_大分類");
                string GV1_大分類 = ddl_GV1_大分類.Text;
                DropDownList ddl_GV1_中分類 = (DropDownList)row.FindControl("ddl_中分類");
                string GV1_中分類 = ddl_GV1_中分類.Text;

                Label Label_表單編號 = (Label)row.FindControl("Label_表單編號");
                Label Label_客訴日期 = (Label)row.FindControl("Label_客訴日期");
                Label Label_客訴商品 = (Label)row.FindControl("Label_客訴商品");
                Label Label_客訴原因 = (Label)row.FindControl("Label_客訴原因");
                Label Label_原因明細 = (Label)row.FindControl("Label_原因明細");
                Label Label_TASK_ID = (Label)row.FindControl("Label_TASK_ID");
                Label Label_DOC_NBR = (Label)row.FindControl("Label_表單編號");
                Label Label_批號 = (Label)row.FindControl("Label_批號");

                string DOC_NBR = Label_表單編號.Text;
                string QCFrm002Date = Label_客訴日期.Text;
                string QCFrm002PRD = Label_客訴商品.Text;
                string QCFrm002Abns = "";
                string QCFrm002AbnscustomValue = "";
                string TASK_ID = Label_TASK_ID.Text;
                string QCFrm002PNO= Label_批號.Text;
                string IMPROVES = newTextValue_GV1_改善方案.Trim();
                string QCFrm002CUST = null;
                string QCFrm002CU = null;
                string QCFrm002BP = null;
                string KINDS1= GV1_大分類.Trim();
                string KINDS2 = GV1_中分類.Trim();
                string REASONS= newTextValue_GV1_原因分析.Trim();
                string IMPROVESOWNER= newTextValue_GV1_改善負責單位.Trim();
                //string IMPROVESDATES=newTextValue_GV1預計改善完成日.Trim();
                string IMPROVESDATES = txtNewField_GV1_預計改善完成日.Text; // 抓到的是 "2023-10-25"
                string IMPROVESDATES_saveToDbFormat = null;

                DateTime dt;

                if (DateTime.TryParse(IMPROVESDATES, out dt))
                {
                    // 轉成您要的 yyyy/MM/dd 格式字串
                    IMPROVESDATES_saveToDbFormat = dt.ToString("yyyy/MM/dd");
                }

                //表單明細FIND_FORM_DETAILS
                DataTable dt_FORM_DETAILS = FIND_FORM_DETAILS(DOC_NBR);
                if(dt_FORM_DETAILS!=null && dt_FORM_DETAILS.Rows.Count>=1)
                {
                    QCFrm002CUST = dt_FORM_DETAILS.Rows[0]["QCFrm002CUST"].ToString();
                    QCFrm002CU = dt_FORM_DETAILS.Rows[0]["QCFrm002CU"].ToString();
                    QCFrm002BP = dt_FORM_DETAILS.Rows[0]["QCFrm002BP"].ToString();
                }

                ADD_TBUOFQC1002TRACES(
                               DOC_NBR,
                               QCFrm002Date,
                               QCFrm002PRD,
                               QCFrm002Abns,
                               QCFrm002AbnscustomValue,
                               TASK_ID,
                               IMPROVES,
                               QCFrm002PNO,
                               KINDS1,
                               KINDS2,
                               REASONS,
                               IMPROVESOWNER,
                               IMPROVESDATES_saveToDbFormat,
                               QCFrm002CUST,
                               QCFrm002CU,
                               QCFrm002BP
                              );
                MsgBox(DOC_NBR+" 完成", this.Page, this);
            }

            BindGrid();
        
        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

    }


    public void SETEXCEL()
    {
        BindGrid();
        if (EXCELDT1 == null || EXCELDT1.Rows.Count == 0) return;

        var fileName = "客訴明細_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var excel = new ExcelPackage())
        {
            var ws = excel.Workbook.Worksheets.Add("明細列表");

            // 【16 個欄位對應表：Key 為資料庫欄位名，Value 為 Excel 標題】
            var columnMap = new Dictionary<string, string>
        {
            { "DOC_NBR", "表單編號" },
            { "QCFrm002Date", "客訴日期" },
            { "QCFrm002PRD", "客訴商品" },
            { "QCFrm002PNO", "批號" },
            { "QCFrm002Abns", "客訴原因" },
            { "QCFrm002AbnscustomValue", "原因明細" },
            { "QCFrm002CUST", "客人" },
            { "QCFrm002Abn", "客訴內容" },
            { "QCFrm002Cmf", "品保初判" },
            { "TASK_RESULT", "簽核狀態" },
            { "ORIGINAL_SIGNER", "簽核人" },
            { "KINDS", "客訴類型" },
            { "REASONS", "原因分析" },
            { "IMPROVES", "改善方案" },
            { "IMPROVESOWNER", "改善負責單位" },
            { "IMPROVESDATES", "預計改善完成日" }
        };

            // 1. 寫入標題列並設定樣式
            int colIndex = 1;
            foreach (var item in columnMap)
            {
                var cell = ws.Cells[1, colIndex];
                cell.Value = item.Value;

                // 標題美化
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSlateGray);
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                colIndex++;
            }

            // 2. 寫入資料內容
            int rowIndex = 2;
            foreach (DataRow dr in EXCELDT1.Rows)
            {
                colIndex = 1;
                foreach (var item in columnMap)
                {
                    ws.Cells[rowIndex, colIndex].Value = dr[item.Key];

                    // 設定格線
                    ws.Cells[rowIndex, colIndex].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    colIndex++;
                }
                rowIndex++;
            }

            // 3. 後端優化設定
            // 開啟自動篩選
            ws.Cells[1, 1, 1, columnMap.Count].AutoFilter = true;

            // 凍結首列
            ws.View.FreezePanes(2, 1);

            // 自適應欄寬 (內容多時會跑比較久，若效能考量可固定特定欄寬)
            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            // 針對「客訴內容」、「原因分析」等長文字欄位設定自動斷行
            // 假設它們是第 8, 13, 14 欄
            int[] longTextCols = { 8, 13, 14 };
            foreach (int colNum in longTextCols)
            {
                ws.Column(colNum).Width = 40; // 固定寬度避免太長
                ws.Column(colNum).Style.WrapText = true;
            }

            // 4. 輸出下載
            byte[] data = excel.GetAsByteArray();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName));
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(data);
            Response.Flush();
            Response.End();
        }
    }
    private DataTable GetKinds1List()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        // 這裡放入您的 SQL 查詢
        StringBuilder sql = new StringBuilder();

        sql.AppendFormat(@"SELECT 
                        [SERNO]
                        ,[KINDS]
                        ,[NAMES]
                        ,[UPKINDS]
                        FROM[TKQC].[dbo].[TBUOFQC1002TRACES_KINDS]
                        WHERE[KINDS] = 'KINDS1'
                        ORDER BY[SERNO]");

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(sql.ToString()));

        if (dt != null && dt.Rows.Count >= 1)
        {
            return dt;
        }
        else
        {
            return null;
        }
    }
    private DataTable GetKinds2List()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        // 這裡放入您的 SQL 查詢
        StringBuilder sql = new StringBuilder();

         sql.AppendFormat(@"SELECT 
                        [SERNO]
                        ,[KINDS]
                        ,[NAMES]
                        ,[UPKINDS]
                        FROM[TKQC].[dbo].[TBUOFQC1002TRACES_KINDS]
                        WHERE[KINDS] = 'KINDS2'
                        ORDER BY[SERNO]");

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(sql.ToString()));
        
        if(dt!=null && dt.Rows.Count>=1)
        {
            return dt;
        }
        else
        {
            return null;
        }
    }
    public void ADD_TBUOFQC1002TRACES(
        string DOC_NBR,
        string QCFrm002Date,
        string QCFrm002PRD,
        string QCFrm002Abns,
        string QCFrm002AbnscustomValue,
        string TASK_ID,
        string IMPROVES,
        string QCFrm002PNO,
        string KINDS1,
        string KINDS2,
        string REASONS,
        string IMPROVESOWNER,
        string IMPROVESDATES,
        string QCFrm002CUST,
        string QCFrm002CU,
        string QCFrm002BP
        )
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        string SQLCOMMAND = @" 
        MERGE [TKQC].[dbo].[TBUOFQC1002TRACES] AS TARGET
        USING (VALUES (@DOC_NBR, @QCFrm002Date, @QCFrm002PRD, @QCFrm002Abns, @QCFrm002AbnscustomValue, @TASK_ID, @IMPROVES, @QCFrm002PNO, @KINDS1, @KINDS2, @REASONS, @IMPROVESOWNER, @IMPROVESDATES,@QCFrm002CUST,@QCFrm002CU,@QCFrm002BP)) 
        AS SOURCE (DOC_NBR, QCFrm002Date, QCFrm002PRD, QCFrm002Abns, QCFrm002AbnscustomValue, TASK_ID, IMPROVES, QCFrm002PNO, KINDS1, KINDS2, REASONS, IMPROVESOWNER, IMPROVESDATES,QCFrm002CUST,QCFrm002CU,QCFrm002BP)
        ON TARGET.DOC_NBR = SOURCE.DOC_NBR

        WHEN MATCHED THEN 
            UPDATE SET 
                QCFrm002Date = SOURCE.QCFrm002Date,
                QCFrm002PRD = SOURCE.QCFrm002PRD,
                QCFrm002Abns = SOURCE.QCFrm002Abns,
                QCFrm002AbnscustomValue = SOURCE.QCFrm002AbnscustomValue,
                TASK_ID = SOURCE.TASK_ID,
                IMPROVES = SOURCE.IMPROVES,
                QCFrm002PNO = SOURCE.QCFrm002PNO,
                KINDS1 = SOURCE.KINDS1,
                KINDS2 = SOURCE.KINDS2,
                REASONS = SOURCE.REASONS,
                IMPROVESOWNER = SOURCE.IMPROVESOWNER,
                IMPROVESDATES = SOURCE.IMPROVESDATES,
                QCFrm002CUST = SOURCE.QCFrm002CUST,
                QCFrm002CU = SOURCE.QCFrm002CU,
                QCFrm002BP = SOURCE.QCFrm002BP

        WHEN NOT MATCHED THEN
            INSERT (DOC_NBR, QCFrm002Date, QCFrm002PRD, QCFrm002Abns, QCFrm002AbnscustomValue, TASK_ID, IMPROVES, QCFrm002PNO, KINDS1,KINDS2, REASONS, IMPROVESOWNER, IMPROVESDATES,QCFrm002CUST,QCFrm002CU,QCFrm002BP)
            VALUES (SOURCE.DOC_NBR, SOURCE.QCFrm002Date, SOURCE.QCFrm002PRD, SOURCE.QCFrm002Abns, SOURCE.QCFrm002AbnscustomValue, SOURCE.TASK_ID, SOURCE.IMPROVES, SOURCE.QCFrm002PNO, SOURCE.KINDS1, SOURCE.KINDS2, SOURCE.REASONS, SOURCE.IMPROVESOWNER, SOURCE.IMPROVESDATES,SOURCE.QCFrm002CUST,SOURCE.QCFrm002CU,SOURCE.QCFrm002BP);";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    // 2. 修正參數綁定，確保每個參數對應正確的 SQL 變數名稱
                    cmd.Parameters.AddWithValue("@DOC_NBR", (object)DOC_NBR ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QCFrm002Date", (object)QCFrm002Date ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QCFrm002PRD", (object)QCFrm002PRD ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QCFrm002Abns", (object)QCFrm002Abns ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QCFrm002AbnscustomValue", (object)QCFrm002AbnscustomValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TASK_ID", (object)TASK_ID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IMPROVES", (object)IMPROVES ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QCFrm002PNO", (object)QCFrm002PNO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@KINDS1", (object)KINDS1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@KINDS2", (object)KINDS2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@REASONS", (object)REASONS ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IMPROVESOWNER", (object)IMPROVESOWNER ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IMPROVESDATES", (object)IMPROVESDATES ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QCFrm002CUST", (object)QCFrm002CUST ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QCFrm002CU", (object)QCFrm002CU ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QCFrm002BP", (object)QCFrm002BP ?? DBNull.Value);
                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(DOC_NBR + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // 建議至少記錄錯誤，方便除錯
            // Log(ex.Message); 
            throw;
        }
    }
    public DataTable FIND_FORM_DETAILS(string DOC_NBR)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();

            cmdTxt.AppendFormat(@"                                
                                WITH TEMP AS (
                                SELECT 
                                [TB_WKF_FORM].[FORM_NAME]
                                ,[CURRENT_DOC]
                                ,TB_WKF_TASK.[DOC_NBR]
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Date""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002Date
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PRD""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002PRD
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abns""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002Abns
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abns""]/@customValue)[1]', 'NVARCHAR(100)') AS QCFrm002AbnscustomValue
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abn""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002Abn
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PNO""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002PNO
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Cmf""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002Cmf
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002CUST""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002CUST
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002CU""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002CU
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002BP""]/@fieldValue)[1]', 'NVARCHAR(100)') AS QCFrm002BP
                                , TB_WKF_TASK.TASK_ID
                                , (CASE WHEN TASK_STATUS = '1' THEN '簽核中' ELSE '已簽核' END) TASK_STATUS
                                , (CASE WHEN TASK_RESULT = '0' THEN '已結案' ELSE '進行中' END ) TASK_RESULT
                                ,(SELECT TOP(1)
                                [TB_EB_USER].NAME
                                FROM[UOF].[dbo].[TB_WKF_TASK_NODE],[UOF].[dbo].[TB_EB_USER]
                                    WHERE[TB_WKF_TASK_NODE].ORIGINAL_SIGNER=[TB_EB_USER].USER_GUID
                                AND ISNULL([FINISH_TIME],'')=''
                                AND TASK_ID = TB_WKF_TASK.TASK_ID
                                ORDER BY[NODE_SEQ]) AS 'ORIGINAL_SIGNER'
                                ,[TBUOFQC1002TRACES].[KINDS1]
                                ,[TBUOFQC1002TRACES].[KINDS2]
                                ,[TBUOFQC1002TRACES].[REASONS]
                                ,[TBUOFQC1002TRACES].[IMPROVES]
                                ,[TBUOFQC1002TRACES].[IMPROVESOWNER]
                                ,REPLACE(CONVERT(VARCHAR,[TBUOFQC1002TRACES].[IMPROVESDATES], 111), '/', '-') IMPROVESDATES

                                FROM[UOF].[dbo].TB_WKF_TASK
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM_VERSION] ON[TB_WKF_FORM_VERSION].FORM_VERSION_ID = TB_WKF_TASK.FORM_VERSION_ID
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM] ON[TB_WKF_FORM].FORM_ID = [TB_WKF_FORM_VERSION].FORM_ID
                                LEFT JOIN[192.168.1.105].[TKQC].[dbo].[TBUOFQC1002TRACES] ON[TBUOFQC1002TRACES].[DOC_NBR]=TB_WKF_TASK.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS
                                WHERE[FORM_NAME] = '1002.客訴異常處理單'
                                AND ISNULL(TASK_RESULT,'') NOT IN('1','2')

                                )
                                SELECT TEMP.*
                                FROM TEMP
                                WHERE 1=1
                                AND [DOC_NBR]=@DOC_NBR



            ");




            m_db.AddParameter("@DOC_NBR", DOC_NBR);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch (Exception EX)
        {
            return null;
        }
        finally
        {

        }

    }

    public void ExportMultiTablesWithMapping(string YEARS)
    {
        try
        {
            // 1. 取得資料來源 (假設來源)
            DataTable dt_MONTHS = Get_MONTHS(YEARS);
            DataTable dt_KINDS = Get_KINDS(YEARS);
            DataTable dt_IMPROVESOWNER = Get_IMPROVESOWNER(YEARS);
            DataTable dt_PRODUCTS = Get_PRODUCTS(YEARS);
            DataTable dt_DETAILS = Get_DETAILS(YEARS);

            var fileName = "報表_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excel = new ExcelPackage())
            {
                var ws = excel.Workbook.Worksheets.Add("資料");
                int currentRow = 1; // 追蹤目前寫到哪一行

                //第1個表格
                var map1 = new Dictionary<string, string> {
            { "年度累積件數", "年度累積件數" },
            { "已結案總數", "已結案總數" },
            { "未結案總數", "未結案總數" }

        };
                ws.Cells[currentRow, 1].Value = "【1、今年件數統計】";
                ws.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                // 呼叫自定義寫入方法
                currentRow = WriteTableToExcel(ws, dt_MONTHS, map1, currentRow);

                // --- 加入間距 (空兩行) ---
                currentRow += 2;

                //第2個表格
                var map2 = new Dictionary<string, string> {
            { "1月件數", "1月件數" },
            { "2月件數", "2月件數" },
            { "3月件數", "3月件數" },
            { "4月件數", "4月件數" },
            { "5月件數", "5月件數" },
            { "6月件數", "6月件數" },
            { "7月件數", "7月件數" },
            { "8月件數", "8月件數" },
            { "9月件數", "9月件數" },
            { "10月件數", "10月件數" },
            { "11月件數", "11月件數" },
            { "12月件數", "12月件數" }

             };
                ws.Cells[currentRow, 1].Value = "【2、月份統計】";
                ws.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                // 呼叫自定義寫入方法
                currentRow = WriteTableToExcel(ws, dt_MONTHS, map2, currentRow);

                // --- 加入間距 (空兩行) ---
                currentRow += 2;

                //第3個表格
                var map3 = new Dictionary<string, string> {
            { "本週件數", "本週件數" },
            { "上週件數", "上週件數" }

             };
                ws.Cells[currentRow, 1].Value = "【3、本週統計】";
                ws.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                // 呼叫自定義寫入方法
                currentRow = WriteTableToExcel(ws, dt_MONTHS, map3, currentRow);

                // --- 加入間距 (空兩行) ---
                currentRow += 2;

                //第4個表格
                var map4 = new Dictionary<string, string> {
            { "客訴分類", "客訴分類" },
            { "年度累積件數", "年度累積件數" },
            { "年度百分比%", "年度百分比%" },
            { "本月件數", "改善本月件數對策" },
            { "本月百分比%", "本月百分比%" }
        };
                ws.Cells[currentRow, 1].Value = "【4、客訴分類】";
                ws.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                currentRow = WriteTableToExcel(ws, dt_KINDS, map4, currentRow);

                // --- 加入間距 (空兩行) ---
                currentRow += 2;

                //第5個表格
                var map5 = new Dictionary<string, string> {
            { "負責單位", "負責單位" },
            { "總件數", "總件數" },
            { "已結案總數", "已結案總數" },
            { "未結案總數", "未結案總數" }
        };
                ws.Cells[currentRow, 1].Value = "【5、負責單位】";
                ws.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                currentRow = WriteTableToExcel(ws, dt_IMPROVESOWNER, map5, currentRow);

                // --- 加入間距 (空兩行) ---
                currentRow += 2;

                //第6個表格
                var map6 = new Dictionary<string, string> {
            { "序號", "序號" },
            { "品名", "品名" },
            { "客訴件數", "已結客訴件數案總數" },
            { "去年年度銷售金額", "去年年度銷售金額" },
            { "每百萬銷售客訴件數", "每百萬銷售客訴件數" }
        };
                ws.Cells[currentRow, 1].Value = "【6、客訴商品排名】";
                ws.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                currentRow = WriteTableToExcel(ws, dt_PRODUCTS, map6, currentRow);

                // --- 加入間距 (空兩行) ---
                currentRow += 2;

                //第7個表格
                var map7 = new Dictionary<string, string> {
            { "是否結案", "是否結案" },
            { "大分類", "大分類" },
            { "中分類", "中分類" },
            { "客訴編號", "客訴編號" },
            { "客訴日期", "客訴日期" },
            { "客訴商品", "客訴商品" },
            { "客訴批號", "客訴批號" },
            { "客訴分類明細", "客訴分類明細" },
            { "改善方案", "改善方案" },
            { "負責單位", "負責單位" },
            { "改善完成日", "改善完成日" },
            { "客人", "客人" },
            { "客訴內容", "客訴內容" },
            { "客訴分析", "客訴分析" }
        };
                ws.Cells[currentRow, 1].Value = "【7、客訴明細】";
                ws.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                currentRow = WriteTableToExcel(ws, dt_DETAILS, map7, currentRow);

                // --- 加入間距 (空兩行) ---
                currentRow += 2;

                // --- 全域樣式設定 ---
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.View.FreezePanes(2, 1); // 凍結首列

                // --- 下載輸出 ---
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName));
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(excel.GetAsByteArray());
                Response.End();
            }
        }
        catch(Exception EX)
        { }
        finally 
        { }
        
    }

    /// <summary>
    /// 通用表格寫入方法：指定 Sheet, DataTable, 映射表, 與起始列
    /// 回傳結束後的下一個 RowIndex
    /// </summary>
    private int WriteTableToExcel(ExcelWorksheet ws, DataTable dt, Dictionary<string, string> map, int startRow)
    {
        int colIndex = 1;
        int rowIndex = startRow;

        // 1. 寫入自定義標題列
        foreach (var item in map)
        {
            var cell = ws.Cells[rowIndex, colIndex];
            cell.Value = item.Value;
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            colIndex++;
        }

        rowIndex++;

        // 2. 寫入資料列
        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                colIndex = 1;
                foreach (var item in map)
                {
                    var targetCell = ws.Cells[rowIndex, colIndex];
                    var originalValue = dr[item.Key];

                    // 判斷是否為欄位
                    if (item.Key.Contains("百分比%"))
                    {
                        double numericValue;
                        if (originalValue != DBNull.Value && double.TryParse(originalValue.ToString(), out numericValue))
                        {
                            targetCell.Value = numericValue;
                        }
                        else
                        {
                            targetCell.Value = originalValue;
                        }
                        // 強制顯示小數點後兩位
                        targetCell.Style.Numberformat.Format = "0.00";
                    }
                    else if (item.Key.Contains("去年年度銷售金額"))
                    {
                        double numericValue;
                        if (originalValue != DBNull.Value && double.TryParse(originalValue.ToString(), out numericValue))
                        {
                            targetCell.Value = numericValue;
                        }
                        else
                        {
                            targetCell.Value = originalValue;
                        }
                        // 強制顯示小數點後兩位
                        targetCell.Style.Numberformat.Format = "#,##";
                    }
                    else
                    {
                        targetCell.Value = originalValue;
                    }

                    // 設定框線
                    targetCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // 【重點】所有處理完後才增加索引
                    colIndex++;
                }
                rowIndex++;
            }
        }

        return rowIndex; // 回傳最後寫到的位置
    }

    public DataTable Get_MONTHS(string YEARS)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();           
            YEARS = YEARS.Substring(2, 2);

            cmdTxt.AppendFormat(@"                              
                                WITH TEMP AS (
                                    SELECT 
                                        [TB_WKF_FORM].[FORM_NAME]
                                        ,TB_WKF_TASK.[DOC_NBR]
                                        -- 處理天數：僅在已結案時計算，否則給 NULL
                                        ,CASE WHEN TASK_RESULT = '0' THEN DATEDIFF(DAY, TB_WKF_TASK.BEGIN_TIME, TB_WKF_TASK.END_TIME) ELSE NULL END AS 結案處理天數
                                        ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Date""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴日期
                                        , (CASE WHEN TASK_RESULT = '0' THEN '已結案' ELSE '進行中' END) AS 是否結案                                   
                                        -- 逾期天數：僅在進行中且已逾期時計算，否則給 NULL（若給 0 會拉低平均）
                                        ,(CASE
                                            WHEN ISNULL(TASK_RESULT,'1') <> '0' AND DATEDIFF(DAY, [TBUOFQC1002TRACES].[IMPROVESDATES], GETDATE()) > 0 
                                            THEN DATEDIFF(DAY, [TBUOFQC1002TRACES].[IMPROVESDATES], GETDATE())
                                            ELSE NULL
                                        END) AS 實際逾期天數
                                    FROM[UOF].[dbo].TB_WKF_TASK WITH(NOLOCK)
                                    LEFT JOIN[UOF].[dbo].[TB_WKF_FORM_VERSION] ON[TB_WKF_FORM_VERSION].FORM_VERSION_ID = TB_WKF_TASK.FORM_VERSION_ID
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM] ON[TB_WKF_FORM].FORM_ID = [TB_WKF_FORM_VERSION].FORM_ID
                                LEFT JOIN[192.168.1.105].[TKQC].[dbo].[TBUOFQC1002TRACES] ON[TBUOFQC1002TRACES].[DOC_NBR] = TB_WKF_TASK.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS
                                WHERE[FORM_NAME] = '1002.客訴異常處理單'
                                      AND ISNULL(TASK_RESULT,'') NOT IN('1','2')
                                      AND TB_WKF_TASK.[DOC_NBR] >= 'QC1002260100001'
                                      AND TB_WKF_TASK.[DOC_NBR] LIKE '%'+@YEARS+'%'
                                )
                                SELECT
                                    COUNT(*) AS 年度累積件數
                                    , SUM(CASE WHEN 是否結案 = '已結案' THEN 1 ELSE 0 END) AS 已結案總數
                                     , SUM(CASE WHEN 是否結案 <> '已結案' THEN 1 ELSE 0 END) AS 未結案總數
                                    -- 核心修正：AVG 會自動忽略 NULL，所以分母只會計算「已結案」或「已逾期」的件數
                                    ,ISNULL(AVG(結案處理天數), 0) AS 平均結案天數
                                    , ISNULL(AVG(實際逾期天數), 0) AS 平均逾期天數
                                     , SUM(CASE WHEN MONTH(客訴日期) = 1 THEN 1 ELSE 0 END) AS '1月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 2 THEN 1 ELSE 0 END) AS '2月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 3 THEN 1 ELSE 0 END) AS '3月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 4 THEN 1 ELSE 0 END) AS '4月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 5 THEN 1 ELSE 0 END) AS '5月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 6 THEN 1 ELSE 0 END) AS '6月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 7 THEN 1 ELSE 0 END) AS '7月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 8 THEN 1 ELSE 0 END) AS '8月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 9 THEN 1 ELSE 0 END) AS '9月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 10 THEN 1 ELSE 0 END) AS '10月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 11 THEN 1 ELSE 0 END) AS '11月件數'
                                    ,SUM(CASE WHEN MONTH(客訴日期) = 12 THEN 1 ELSE 0 END) AS '12月件數'
	                                -- 【本週】: 從本週一 00:00 到 本週日 23:59
                                        , SUM(CASE WHEN 客訴日期 >= DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0)
                                                  AND 客訴日期 <= DATEADD(day, 6, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0)) THEN 1 ELSE 0 END) AS 本週件數
        
                                        -- 【上週】: 從上週一 00:00 到 上週日 23:59
                                        , SUM(CASE WHEN 客訴日期 >= DATEADD(wk, DATEDIFF(wk, 0, GETDATE()) - 1, 0)
                                                  AND 客訴日期 <= DATEADD(day, 6, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()) - 1, 0)) THEN 1 ELSE 0 END) AS 上週件數


                                FROM TEMP;


                             ");




           m_db.AddParameter("@YEARS", YEARS);        

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt!=null && dt.Rows.Count>=1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch(Exception EX)
        {
            return null;
        }
        finally
        {

        }
    
    }

    public DataTable Get_KINDS(string YEARS)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            
            YEARS = YEARS.Substring(2, 2);

            cmdTxt.AppendFormat(@" 
                               WITH TEMP AS (
                                SELECT 
                                    [TB_WKF_FORM].[FORM_NAME]
                                    ,TB_WKF_TASK.[DOC_NBR]
                                    ,CAST([CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Date""]/@fieldValue)[1]', 'NVARCHAR(100)') AS DATE) AS 客訴日期                                  
                                    ,[TBUOFQC1002TRACES].[KINDS1]  AS 大分類
                                    ,[TBUOFQC1002TRACES].[KINDS2] AS 中分類
                                FROM[UOF].[dbo].TB_WKF_TASK WITH(NOLOCK)
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM_VERSION] ON[TB_WKF_FORM_VERSION].FORM_VERSION_ID = TB_WKF_TASK.FORM_VERSION_ID
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM] ON[TB_WKF_FORM].FORM_ID = [TB_WKF_FORM_VERSION].FORM_ID
                                LEFT JOIN[192.168.1.105].[TKQC].[dbo].[TBUOFQC1002TRACES] ON[TBUOFQC1002TRACES].[DOC_NBR] = TB_WKF_TASK.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS
                                WHERE[FORM_NAME] = '1002.客訴異常處理單'
                                    AND ISNULL(TASK_RESULT, '') NOT IN('1', '2')
                                    AND TB_WKF_TASK.[DOC_NBR] >= 'QC1002260100001'
                                    AND TB_WKF_TASK.[DOC_NBR] LIKE '%'+@YEARS+'%'
                            ),
                            AGG_DATA AS(
                                SELECT
                                    ISNULL(大分類, '未分類') AS 大分類
                                    , COUNT(*) AS 年度累積件數
                                        , SUM(CASE WHEN MONTH(客訴日期) = MONTH(GETDATE()) THEN 1 ELSE 0 END) AS 本月件數
                                        , SUM(COUNT(*)) OVER() AS 年度總分母
                                        , SUM(SUM(CASE WHEN MONTH(客訴日期) = MONTH(GETDATE()) THEN 1 ELSE 0 END)) OVER() AS 本月總分母
                                FROM TEMP
                                GROUP BY 大分類
                            )
                            SELECT
                                CASE WHEN(GROUPING(大分類) = 1) THEN '合計' ELSE 大分類 END AS 客訴分類
                                ,SUM(年度累積件數) AS 年度累積件數
                                , CASE
                                    WHEN GROUPING(大分類) = 1 THEN 100.00
                                    WHEN MAX(年度總分母) = 0 THEN 0.00
                                    ELSE CAST(SUM(年度累積件數) * 100.0 / MAX(年度總分母) AS DECIMAL(5,2)) 
                                    END AS '年度百分比%'
                                ,SUM(本月件數) AS 本月件數
                                , CASE
                                    --修正點：合計列若總件數為0則顯示0，否則100
                                    WHEN GROUPING(大分類) = 1 THEN(CASE WHEN MAX(本月總分母) = 0 THEN 0.00 ELSE 100.00 END)
                                    -- 修正點：若本月件數為0，百分比直接給0
                                    WHEN SUM(本月件數) = 0 OR MAX(本月總分母) = 0 THEN 0.00
                                    ELSE CAST(SUM(本月件數) * 100.0 / MAX(本月總分母) AS DECIMAL(5,2)) 
                                    END AS '本月百分比%'
                            FROM AGG_DATA
                            GROUP BY ROLLUP(大分類)
                            ORDER BY GROUPING(大分類), 年度累積件數 DESC;


            ");




            m_db.AddParameter("@YEARS", YEARS);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch (Exception EX)
        {
            return null;
        }
        finally
        {

        }

    }

    public DataTable Get_IMPROVESOWNER(string YEARS)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            
            YEARS = YEARS.Substring(2, 2);

            cmdTxt.AppendFormat(@" 
                                WITH TEMP AS (
                                SELECT 
                                [TB_WKF_FORM].[FORM_NAME]
                                ,[CURRENT_DOC]
                                ,TB_WKF_TASK.[DOC_NBR]
                                ,TB_WKF_TASK.BEGIN_TIME
                                ,TB_WKF_TASK.END_TIME
                                ,DATEDIFF(DAY, TB_WKF_TASK.BEGIN_TIME, TB_WKF_TASK.END_TIME) AS 處理天數
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Date""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴日期
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PRD""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴商品
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abns""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴原因
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abns""]/@customValue)[1]', 'NVARCHAR(100)') AS 客訴原因值
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002CUST""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客人
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abn""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴內容
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PNO""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴批號
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Cmf""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴分析
                                , TB_WKF_TASK.TASK_ID
                                , (CASE WHEN TASK_STATUS = '1' THEN '簽核中' ELSE '已簽核' END) 目前簽核
                                , (CASE WHEN TASK_RESULT = '0' THEN '已結案' ELSE '進行中' END ) 是否結案
                                ,(SELECT TOP(1)
                                [TB_EB_USER].NAME
                                FROM[UOF].[dbo].[TB_WKF_TASK_NODE],[UOF].[dbo].[TB_EB_USER]
                                    WHERE[TB_WKF_TASK_NODE].ORIGINAL_SIGNER=[TB_EB_USER].USER_GUID
                                AND ISNULL([FINISH_TIME],'')=''
                                AND TASK_ID = TB_WKF_TASK.TASK_ID
                                ORDER BY[NODE_SEQ]) AS 目前簽核人                               
                                ,[TBUOFQC1002TRACES].[REASONS]
                                    AS 客訴分類明細
                                ,[TBUOFQC1002TRACES].[IMPROVES]
                                    AS 改善方案
                                ,[TBUOFQC1002TRACES].[IMPROVESOWNER]
                                    AS 負責單位
                                ,[TBUOFQC1002TRACES].[IMPROVESDATES]
                                    AS 改善完成日
                                , DATEDIFF(DAY,  [TBUOFQC1002TRACES].[IMPROVESDATES], GETDATE()) AS 改善天數
                                 ,(CASE 
                                     WHEN ISNULL(TASK_RESULT, '1') <> '0' AND DATEDIFF(DAY, [TBUOFQC1002TRACES].[IMPROVESDATES], GETDATE()) > 0 
                                     THEN DATEDIFF(DAY, [TBUOFQC1002TRACES].[IMPROVESDATES], GETDATE()) 
                                     ELSE 0
                                 END) AS 逾期天數
                                FROM[UOF].[dbo].TB_WKF_TASK
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM_VERSION] ON[TB_WKF_FORM_VERSION].FORM_VERSION_ID = TB_WKF_TASK.FORM_VERSION_ID
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM] ON[TB_WKF_FORM].FORM_ID = [TB_WKF_FORM_VERSION].FORM_ID
                                LEFT JOIN[192.168.1.105].[TKQC].[dbo].[TBUOFQC1002TRACES] ON[TBUOFQC1002TRACES].[DOC_NBR]=TB_WKF_TASK.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS
                                WHERE[FORM_NAME] = '1002.客訴異常處理單'
                                AND ISNULL(TASK_RESULT,'') NOT IN('1','2')
                                AND TB_WKF_TASK.[DOC_NBR]>='QC1002260100001'
                                AND TB_WKF_TASK.[DOC_NBR] LIKE '%'+@YEARS+'%'
                                )

                                SELECT TEMP.負責單位
                                , COUNT(負責單位) AS '總件數'
                                , SUM(CASE WHEN 是否結案 = '已結案' THEN 1 ELSE 0 END) AS 已結案總數
                                 , SUM(CASE WHEN 是否結案 <> '已結案' THEN 1 ELSE 0 END) AS 未結案總數
                                FROM TEMP
                                WHERE 1=1
                                GROUP BY TEMP.負責單位
                                ORDER BY TEMP.負責單位

            ");




            m_db.AddParameter("@YEARS", YEARS);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch (Exception EX)
        {
            return null;
        }
        finally
        {

        }

    }

    public DataTable Get_PRODUCTS(string YEARS)
    {
        int lastyears = Convert.ToInt32(YEARS) - 1;

        // 直接指定 年, 月, 日
        DateTime SDATES = new DateTime(lastyears, 1, 1);
        DateTime EDATES = new DateTime(lastyears, 12, 31);

        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();

            YEARS = YEARS.Substring(2, 2);

            cmdTxt.AppendFormat(@"             
                                SELECT TOP 10
                                    -- 核心修正：在外層建立流水號排名
                                    ROW_NUMBER() OVER (ORDER BY ISNULL(MAX(TEMP.MONEYS), 0) DESC) AS '序號'
                                    ,[QCFrm002PRD] AS '品名'
                                    ,COUNT([QCFrm002PRD]) AS '客訴件數'
                                    ,ISNULL(MAX(TEMP.MONEYS), 0) AS '去年年度銷售金額'
                                    ,CAST(COUNT([QCFrm002PRD]) * 1000000.0 / NULLIF(MAX(TEMP.MONEYS), 0) AS DECIMAL(10,2)) AS '每百萬銷售客訴件數'
                                FROM [TKQC].[dbo].[TBUOFQC1002TRACES]
                                LEFT JOIN (
                                    SELECT 
                                        MB002, 
                                        SUM(LA017 - LA020) AS MONEYS
                                    FROM [TK].dbo.SASLA WITH(NOLOCK)
                                    INNER JOIN [TK].dbo.INVMB WITH(NOLOCK) ON LA005 = MB001
                                    WHERE LA015 >= @SDATES AND LA015 <= @EDATES
                                    GROUP BY MB002
                                ) AS TEMP ON TEMP.MB002 = [TBUOFQC1002TRACES].[QCFrm002PRD]
                                WHERE [DOC_NBR] >= 'QC1002260100001'
                                  AND [DOC_NBR] LIKE '%'+@YEARS+'%'
                                GROUP BY [QCFrm002PRD]
                                -- 這裡的 ORDER BY 必須與 ROW_NUMBER() 的排序邏輯一致
                                ORDER BY '序號' ASC;                   

            ");




            m_db.AddParameter("@YEARS", YEARS);
            m_db.AddParameter("@SDATES", SDATES.ToString("yyyyMMdd"));
            m_db.AddParameter("@EDATES", EDATES.ToString("yyyyMMdd"));

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch (Exception EX)
        {
            return null;
        }
        finally
        {

        }

    }

    public DataTable Get_DETAILS(string YEARS)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
           
            YEARS = YEARS.Substring(2, 2);

            cmdTxt.AppendFormat(@" 
                               WITH TEMP AS (
                                SELECT 
                                [TB_WKF_FORM].[FORM_NAME]
                                ,[CURRENT_DOC]
                                ,TB_WKF_TASK.[DOC_NBR] AS '客訴編號'
                                ,TB_WKF_TASK.BEGIN_TIME
                                ,TB_WKF_TASK.END_TIME
                                ,DATEDIFF(DAY, TB_WKF_TASK.BEGIN_TIME, TB_WKF_TASK.END_TIME) AS 處理天數
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Date""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴日期
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PRD""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴商品
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abns""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴原因
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abns""]/@customValue)[1]', 'NVARCHAR(100)') AS 客訴原因值
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002CUST""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客人
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Abn""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴內容
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PNO""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴批號
                                ,[CURRENT_DOC].value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002Cmf""]/@fieldValue)[1]', 'NVARCHAR(100)') AS 客訴分析
                                , TB_WKF_TASK.TASK_ID
                                , (CASE WHEN TASK_STATUS = '1' THEN '簽核中' ELSE '已簽核' END) 目前簽核
                                , (CASE WHEN TASK_RESULT = '0' THEN '已結案' ELSE '進行中' END ) 是否結案
                                ,(SELECT TOP(1)
                                [TB_EB_USER].NAME
                                FROM[UOF].[dbo].[TB_WKF_TASK_NODE],[UOF].[dbo].[TB_EB_USER]
                                    WHERE[TB_WKF_TASK_NODE].ORIGINAL_SIGNER=[TB_EB_USER].USER_GUID
                                AND ISNULL([FINISH_TIME],'')=''
                                AND TASK_ID = TB_WKF_TASK.TASK_ID
                                ORDER BY[NODE_SEQ]) AS 目前簽核人
                                ,[TBUOFQC1002TRACES].[KINDS1]
                                    AS 大分類
                                ,[TBUOFQC1002TRACES].[KINDS2]
                                    AS 中分類
                                ,[TBUOFQC1002TRACES].[REASONS]
                                    AS 客訴分類明細
                                ,[TBUOFQC1002TRACES].[IMPROVES]
                                    AS 改善方案
                                ,[TBUOFQC1002TRACES].[IMPROVESOWNER]
                                    AS 負責單位
                                ,[TBUOFQC1002TRACES].[IMPROVESDATES]
                                    AS 改善完成日
                                , DATEDIFF(DAY,  [TBUOFQC1002TRACES].[IMPROVESDATES], GETDATE()) AS 改善天數
                                    ,(CASE
                                        WHEN ISNULL(TASK_RESULT, '1') <> '0' AND DATEDIFF(DAY, [TBUOFQC1002TRACES].[IMPROVESDATES], GETDATE()) > 0
                                        THEN DATEDIFF(DAY, [TBUOFQC1002TRACES].[IMPROVESDATES], GETDATE())
                                        ELSE 0
                                    END) AS 逾期天數
                                FROM[UOF].[dbo].TB_WKF_TASK
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM_VERSION] ON[TB_WKF_FORM_VERSION].FORM_VERSION_ID = TB_WKF_TASK.FORM_VERSION_ID
                                LEFT JOIN[UOF].[dbo].[TB_WKF_FORM] ON[TB_WKF_FORM].FORM_ID = [TB_WKF_FORM_VERSION].FORM_ID
                                LEFT JOIN[192.168.1.105].[TKQC].[dbo].[TBUOFQC1002TRACES] ON[TBUOFQC1002TRACES].[DOC_NBR]=TB_WKF_TASK.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS
                                WHERE[FORM_NAME] = '1002.客訴異常處理單'
                                AND ISNULL(TASK_RESULT,'') NOT IN('1','2')
                                AND TB_WKF_TASK.[DOC_NBR]>='QC1002260100001'
                                     AND TB_WKF_TASK.[DOC_NBR] LIKE '%'+@YEARS+'%'
                                )


                                SELECT
                                是否結案
                                , 大分類
                                , 中分類
                                , 客訴編號
                                , 客訴日期
                                , 客訴商品
                                , 客訴批號
                                , 客訴分類明細
                                , 改善方案
                                , 負責單位
                                , 改善完成日
                                , 客人
                                , 客訴內容
                                , 客訴分析

                                FROM TEMP
                                WHERE 1=1
                                ORDER BY 是否結案 DESC, 大分類, 客訴編號

            ");




            m_db.AddParameter("@YEARS", YEARS);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch (Exception EX)
        {
            return null;
        }
        finally
        {

        }

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
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();
      
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        string YEARS = Date2.Text.Trim().ToString();
        ExportMultiTablesWithMapping(YEARS);
        MsgBox("完成", this.Page, this);
    }
    #endregion
}