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

public partial class CDS_WebPage_RESEARCH_TK_UOF_FROMS_1002_RECORDS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    DataTable EXCELDT1 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            BindDropDownList_SORT();
            BindDropDownList_STATUS();
        }

    }

    #region FUNCTION

    private void BindDropDownList_SORT()
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
                                WHERE [KIND]='UOF_FROMS_1002_RECORDS_SORT'
                                ORDER BY [ID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList_SORT.DataSource = dt;
            DropDownList_SORT.DataTextField = "PARANAME";
            DropDownList_SORT.DataValueField = "PARANAME";
            DropDownList_SORT.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownList_STATUS()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARANAME", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT [PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='UOF_FROMS_1002_RECORDS_STATUS'  ORDER BY [PARAID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList_STATUS.DataSource = dt;
            DropDownList_STATUS.DataTextField = "PARANAME";
            DropDownList_STATUS.DataValueField = "PARANAME";
            DropDownList_STATUS.DataBind();

        }
        else
        {

        }
    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        StringBuilder QUERYS4 = new StringBuilder();


        StringBuilder ORDERBY = new StringBuilder();

        string ISCLOSED = ddlSearchIsClosed.Text.ToString();
        string ORDER= DropDownList_SORT.Text.ToString();
        string STATUS = DropDownList_STATUS.Text.ToString();

        //WHERE ISCLOSED
        if (ISCLOSED.Equals("N"))
        {
            QUERYS.AppendFormat(@" AND (ISNULL([TK_UOF_RECORDS_1002].[ISCLOSED],'')='' OR [TK_UOF_RECORDS_1002].[ISCLOSED]='N') ");
        }
        else if (ISCLOSED.Equals("Y"))
        {
            QUERYS.AppendFormat(@" AND [TK_UOF_RECORDS_1002].[ISCLOSED]='Y'");
        }
        else
        {
            QUERYS.AppendFormat(@" ");
        }
        //WHERE STATUS
        if (string.IsNullOrEmpty(STATUS) || STATUS.Equals("全部"))
        {
            // 如果是空值或「全部」，則不加任何篩選條件
        }
        else if (STATUS.Equals("未指定"))
        {
            QUERYS2.Append(" AND ISNULL(STATUS,'') = '' ");
        }
        else
        {
            // 注意：若 STATUS 是外部輸入，直接組字串會有 SQL 注入風險
            // 建議改用參數化查詢，或是確保這裡的 STATUS 是安全的內部變數
            QUERYS2.AppendFormat(" AND STATUS = '{0}' ", STATUS);
        }
        //WHERE 產品名稱
        string names = TextBox1.Text.Trim();
        if ( !string.IsNullOrEmpty(names))
        {
            // 如果 STATUS 是外部傳入的變數，建議防範 SQL 注入
            // 這裡維持您原本的串接方式，但精簡了邏輯
            QUERYS3.AppendFormat(@" AND TD.Row.value('(Cell[@fieldId=""DVV01""]/@fieldValue)[1]', 'NVARCHAR(MAX)') LIKE '%{0}%' ", names);
        }
        else
        {
            QUERYS3.AppendFormat(@" ");
        }


        // ORDER BY 
        if (ORDER.Equals("依預交日"))
        {
            ORDERBY.AppendFormat(@"  ORDER BY [TK_UOF_RECORDS_1002].[DVV07],t.DOC_NBR,TD.Row.value('@order', 'INT') + 1 ");
        }
        else
        {
            ORDERBY.AppendFormat(@"  ORDER BY f.FORM_NAME,u.NAME, t.DOC_NBR");
        }

        cmdTxt.AppendFormat(@"                              
                            SELECT 
                                f.FORM_NAME       AS '表單名稱',
                                u.NAME            AS '申請者',
                                CONVERT(nvarchar,t.BEGIN_TIME,111)      AS '申請時間',
                                t.DOC_NBR         AS '表單編號',   
	                            (CASE WHEN t.TASK_RESULT='0' THEN '已結案' ELSE '進行中' END ) AS '表單狀態',
                                t.CURRENT_SITE_ID,
	                            t.TASK_ID,
                                t.CURRENT_DOC,
	                            TD.Row.value('@order', 'INT') + 1 AS '項次',
                                TD.Row.value('(Cell[@fieldId=""DVV01""]/@fieldValue)[1]', 'NVARCHAR(MAX)') AS '產品名稱',
                                TD.Row.value('(Cell[@fieldId=""DVV02""]/@fieldValue)[1]', 'NVARCHAR(MAX)') AS '包裝方式',
                                TD.Row.value('(Cell[@fieldId=""DVV03""]/@fieldValue)[1]', 'NVARCHAR(MAX)') AS '規格',
                                TD.Row.value('(Cell[@fieldId=""DVV09""]/@fieldValue)[1]', 'NVARCHAR(MAX)') AS '尺寸',
                                TD.Row.value('(Cell[@fieldId=""DVV10""]/@fieldValue)[1]', 'NVARCHAR(MAX)') AS '包材',
                                TD.Row.value('(Cell[@fieldId=""DVV04""]/@fieldValue)[1]', 'NVARCHAR(MAX)') AS '需求量',
                                TD.Row.value('(Cell[@fieldId=""DVV07""]/@fieldValue)[1]', 'NVARCHAR(MAX)') AS '原試吃品需求日',                                

                               	[TK_UOF_RECORDS_1002].[COMMENTS] AS '備註',
                                [TK_UOF_RECORDS_1002].[ISCLOSED] AS '結案',
                                [TK_UOF_RECORDS_1002].[DVV07] AS '預交日',
                                CONVERT(NVARCHAR,[TK_UOF_RECORDS_1002].[CLOSEDDATES],111) AS '結案日',
                                [STATUS] AS '進度',
	                            [TBPARA].[PARAID]

                            FROM[UOF].dbo.TB_WKF_TASK AS t WITH(NOLOCK)
                            CROSS APPLY[CURRENT_DOC].nodes('/Form/FormFieldValue/FieldItem[@fieldId=""DETAILS""]/DataGrid/Row') AS TD(Row)
                            LEFT JOIN[192.168.1.105].[TKRESEARCH].[dbo].[TK_UOF_RECORDS_1002] WITH(NOLOCK) ON[TK_UOF_RECORDS_1002].[DOC_NBR] = t.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS AND[TK_UOF_RECORDS_1002].[SERNO] = TD.Row.value('@order', 'INT') + 1
                            LEFT JOIN [192.168.1.105].[TKRESEARCH].[dbo].[TBPARA] ON  [TBPARA].[KIND]='UOF_FROMS_1002_RECORDS_STATUS' AND [TBPARA].[PARANAME]=[TK_UOF_RECORDS_1002].STATUS
                            LEFT JOIN[UOF].dbo.TB_EB_USER AS u
                                ON u.USER_GUID = t.USER_GUID
                            LEFT JOIN[UOF].dbo.TB_EB_EMPL_DEP AS ed
                                ON ed.USER_GUID = u.USER_GUID AND ed.ORDERS = '0'
                            JOIN[UOF].dbo.TB_WKF_FORM_VERSION AS fv
                                ON t.FORM_VERSION_ID = fv.FORM_VERSION_ID
                            JOIN[UOF].dbo.TB_WKF_FORM AS f
                                ON f.FORM_ID = fv.FORM_ID
                            WHERE 1 = 1
                                AND t.BEGIN_TIME >= '2025-01-01'
                                AND f.FORM_NAME IN('1004.無品號試吃製作申請單')
                                {0}
                                {1}
                                {2}
                                {3}
                           ;

                            ", QUERYS.ToString()
                            , QUERYS2.ToString()
                            , QUERYS3.ToString()
                            , ORDERBY.ToString());




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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string taskId = DataBinder.Eval(e.Row.DataItem, "TASK_ID") as string;
            HyperLink hlTask = (HyperLink)e.Row.FindControl("hlTask");

            if (!string.IsNullOrEmpty(taskId))
            {
                hlTask.NavigateUrl = string.Format("https://eip.tkfood.com.tw/UOF/wkf/formuse/viewform.aspx?TASK_ID={0}", taskId);
            }
            else
            {
                hlTask.Visible = false; // 或改成顯示文字 Label
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn2 = (Button)e.Row.FindControl("Button2");
            if (btn2 != null)
            {
                string cellValue2 = btn2.CommandArgument;
                dynamic param2 = new { ID = cellValue2 }.ToExpando();
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn3 = (Button)e.Row.FindControl("Button3");
            if (btn3 != null)
            {
                string cellValue3 = btn3.CommandArgument;
                dynamic param3 = new { ID = cellValue3 }.ToExpando();
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn4 = (Button)e.Row.FindControl("Button4");
            if (btn4 != null)
            {
                string cellValue4 = btn4.CommandArgument;
                dynamic param4 = new { ID = cellValue4 }.ToExpando();
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 1. 尋找控制項
            DropDownList ddlProgress = (DropDownList)e.Row.FindControl("DropDownList_進度");
            HiddenField hfValue = (HiddenField)e.Row.FindControl("Hidden_進度");

            if (ddlProgress != null)
            {
                // 2. 取得 SQL 資料 (建議將此 SQL 邏輯抽出成一個 DataTable 以免重複查詢多次)
                DataTable dtOptions = GetProgressOptions();

                ddlProgress.DataSource = dtOptions;
                ddlProgress.DataTextField = "PARANAME";  // 顯示的文字 (請依 SQL 欄位修改)
                ddlProgress.DataValueField = "PARANAME"; // 實際的值 (請依 SQL 欄位修改)
                ddlProgress.DataBind();

                // 3. 加入預設空白選項
                ddlProgress.Items.Insert(0, new ListItem("請選擇", ""));

                // 4. 設定選取值 (對應資料庫原本的值)
                if (hfValue != null && !string.IsNullOrEmpty(hfValue.Value))
                {
                    ddlProgress.SelectedValue = hfValue.Value;
                }
            }
        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
       // 過濾無關的 Command
         if (e.CommandName == "Page" || e.CommandName == "Sort") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);

        // 初始化變數
        string DOC_NBR = "", SERNO = "", DVV01 = "", DVV02 = "", DVV03 = "";
        string DVV09 = "", DVV10 = "", DVV04 = "", DVV07 = "", ISCLOSED = "", COMMENTS = "";
        string CLOSEDDATES =null;
        string STATUS = null;

        if (rowIndex >= 0 && rowIndex < Grid1.Rows.Count)
        {
            GridViewRow row = Grid1.Rows[rowIndex];

            // 使用 FindControl 並加入防呆檢查 (避免找不控制項導致 NullReferenceException)
            TextBox txt備註 = (TextBox)row.FindControl("txtNewField_GV1_備註");
            TextBox txt試吃品需求日 = (TextBox)row.FindControl("txtNewField_GV1_預交日");
            DropDownList dr進度=(DropDownList)row.FindControl("DropDownList_進度");

            Label lbl編號 = (Label)row.FindControl("Label_表單編號");
            Label lbl項次 = (Label)row.FindControl("Label_項次");
            Label lbl產品 = (Label)row.FindControl("Label_產品名稱");
            Label lbl包裝 = (Label)row.FindControl("Label_包裝方式");
            Label lbl規格 = (Label)row.FindControl("Label_規格");
            Label lbl尺寸 = (Label)row.FindControl("Label_尺寸");
            Label lbl包材 = (Label)row.FindControl("Label_包材");
            Label lbl需求 = (Label)row.FindControl("Label_需求量");
            Label lbl結案 = (Label)row.FindControl("Label_結案");

            // 賦值(C# 5.0 相容寫法)
            DOC_NBR = (lbl編號 != null) ? lbl編號.Text : "";
            SERNO = (lbl項次 != null) ? lbl項次.Text : "";
            DVV01 = (lbl產品 != null) ? lbl產品.Text : "";
            DVV02 = (lbl包裝 != null) ? lbl包裝.Text : "";
            DVV03 = (lbl規格 != null) ? lbl規格.Text : "";
            DVV09 = (lbl尺寸 != null) ? lbl尺寸.Text : "";
            DVV10 = (lbl包材 != null) ? lbl包材.Text : "";
            DVV04 = (lbl需求 != null) ? lbl需求.Text : "";
            DVV07 = (txt試吃品需求日 != null) ? txt試吃品需求日.Text : "";
            ISCLOSED = (lbl結案 != null) ? lbl結案.Text : "";
                  
            COMMENTS = "";
            if (txt備註 != null)
            {
                COMMENTS = txt備註.Text.Trim();
            }
            DVV07 = "";
            if (txt試吃品需求日 != null)
            {
                string DATES = txt試吃品需求日.Text.Trim();

                DateTime dateValue = Convert.ToDateTime(DATES);
                DVV07 = dateValue.ToString("yyyy/MM/dd");
            }

            STATUS = dr進度.Text.Trim();

            // --- 邏輯判斷區 (修正括號層級) ---

            if (e.CommandName == "Button2")
            {
                // 保持原始 ISCLOSED
                ADD_TK_UOF_RECORDS_1002(DOC_NBR, SERNO, DVV01, DVV02, DVV03, DVV09, DVV10, DVV04, DVV07, COMMENTS, ISCLOSED, CLOSEDDATES, STATUS);
                MsgBox(DOC_NBR + " 儲存完成", this.Page, this);
            }
            else if (e.CommandName == "Button3")
            {
                ISCLOSED = "Y";
                CLOSEDDATES = DateTime.Now.ToString("yyyy/MM/dd");
                ADD_TK_UOF_RECORDS_1002(DOC_NBR, SERNO, DVV01, DVV02, DVV03, DVV09, DVV10, DVV04, DVV07, COMMENTS, ISCLOSED, CLOSEDDATES, STATUS);
                MsgBox(DOC_NBR + " 已結案", this.Page, this);
            }
            else if (e.CommandName == "Button4")
            {
                ISCLOSED = "N";
               
                ADD_TK_UOF_RECORDS_1002(DOC_NBR, SERNO, DVV01, DVV02, DVV03, DVV09, DVV10, DVV04, DVV07, COMMENTS, ISCLOSED, CLOSEDDATES, STATUS);
                MsgBox(DOC_NBR + " 已還原未結案", this.Page, this);
            }

            // 最後重新繫結
            BindGrid();
        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

    }

    private void BindGrid2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();      


        cmdTxt.AppendFormat(@"                              
                             SELECT   
	                            [TBPARA].[PARAID] AS '項目',
                                [STATUS] AS '進度',
	                            COUNT(t.DOC_NBR ) AS '件數'

                            FROM [UOF].dbo.TB_WKF_TASK AS t WITH(NOLOCK)
                            CROSS APPLY[CURRENT_DOC].nodes('/Form/FormFieldValue/FieldItem[@fieldId=""DETAILS""]/DataGrid/Row') AS TD(Row)
                            LEFT JOIN[192.168.1.105].[TKRESEARCH].[dbo].[TK_UOF_RECORDS_1002] WITH(NOLOCK) ON[TK_UOF_RECORDS_1002].[DOC_NBR] = t.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS AND[TK_UOF_RECORDS_1002].[SERNO] = TD.Row.value('@order', 'INT') + 1
                            LEFT JOIN[192.168.1.105].[TKRESEARCH].[dbo].[TBPARA] ON[TBPARA].[KIND] = 'UOF_FROMS_1002_RECORDS_STATUS' AND[TBPARA].[PARANAME] =[TK_UOF_RECORDS_1002].STATUS
                            LEFT JOIN[UOF].dbo.TB_EB_USER AS u
                                ON u.USER_GUID = t.USER_GUID
                            LEFT JOIN[UOF].dbo.TB_EB_EMPL_DEP AS ed
                                ON ed.USER_GUID = u.USER_GUID AND ed.ORDERS = '0'
                            JOIN[UOF].dbo.TB_WKF_FORM_VERSION AS fv
                                ON t.FORM_VERSION_ID = fv.FORM_VERSION_ID
                            JOIN[UOF].dbo.TB_WKF_FORM AS f
                                ON f.FORM_ID = fv.FORM_ID
                            WHERE 1 = 1
                                AND t.BEGIN_TIME >= '2025-01-01'
                                AND f.FORM_NAME IN('1004.無品號試吃製作申請單')
                            GROUP BY[STATUS],[TBPARA].[PARAID]
                            ORDER BY[TBPARA].[PARAID]

                            ");



        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

      

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {       

    }


    public void SETEXCEL()
    {
        BindGrid();
        if (EXCELDT1 == null || EXCELDT1.Rows.Count == 0) return;

        var fileName = "明細_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var excel = new ExcelPackage())
        {
            var ws = excel.Workbook.Worksheets.Add("明細列表");

            // 【16 個欄位對應表：Key 為資料庫欄位名，Value 為 Excel 標題】
            var columnMap = new Dictionary<string, string>
        {
            { "預交日", "預交日" },
            { "表單狀態", "表單狀態" },
            { "表單編號", "表單編號" },
            { "申請者", "申請者" },
            { "申請時間", "申請時間" },
            { "項次", "項次" },
            { "產品名稱", "產品名稱" },
            { "包裝方式", "包裝方式" },
            { "規格", "規格" },
            { "尺寸", "尺寸" },
            { "包材", "包材" },
            { "需求量", "需求量" },           
            { "結案", "結案" },
            { "結案日", "結案日" },
            { "備註", "備註" },
            { "進度", "進度" }
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

    // 取得下拉選單資料的專用方法
    private DataTable GetProgressOptions()
    {
        // 使用您現有的 DatabaseHelper
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        // 這裡替換成您實際要查詢進度清單的 SQL
        string cmdTxt = @"SELECT [PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='UOF_FROMS_1002_RECORDS_STATUS' AND [PARAID] NOT IN ('00') ORDER BY [PARAID]";

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(cmdTxt));
        return dt;
    }
    public void ADD_TK_UOF_RECORDS_1002(
        string DOC_NBR
        , string SERNO
        , string DVV01
        , string DVV02
        , string DVV03
        , string DVV09
        , string DVV10
        , string DVV04
        , string DVV07
        , string COMMENTS
        , string ISCLOSED
        , string CLOSEDDATES
        , string STATUS
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        string SQLCOMMAND = @" 
                            MERGE [TKRESEARCH].[dbo].[TK_UOF_RECORDS_1002] AS TARGET
                            USING (VALUES (@DOC_NBR, @SERNO, @DVV01, @DVV02, @DVV03, @DVV09, @DVV10, @DVV04, @DVV07, @COMMENTS, @ISCLOSED, @CLOSEDDATES,@STATUS)) 
                            AS SOURCE (DOC_NBR, SERNO, DVV01, DVV02, DVV03, DVV09, DVV10, DVV04, DVV07, COMMENTS, ISCLOSED, CLOSEDDATES,STATUS)
                            ON TARGET.DOC_NBR = SOURCE.DOC_NBR AND TARGET.SERNO = SOURCE.SERNO

                            WHEN MATCHED THEN 
                                UPDATE SET 
                                    DVV01 = SOURCE.DVV01,
                                    DVV02 = SOURCE.DVV02,
                                    DVV03 = SOURCE.DVV03,
                                    DVV09 = SOURCE.DVV09,
                                    DVV10 = SOURCE.DVV10,
                                    DVV04 = SOURCE.DVV04,
                                    DVV07 = SOURCE.DVV07,
                                    COMMENTS = SOURCE.COMMENTS,
                                    ISCLOSED = SOURCE.ISCLOSED,
                                    CLOSEDDATES = SOURCE.CLOSEDDATES,
                                    STATUS=SOURCE.STATUS

                            WHEN NOT MATCHED THEN                          
                                INSERT (DOC_NBR, SERNO, DVV01, DVV02, DVV03, DVV09, DVV10, DVV04, DVV07, COMMENTS, ISCLOSED, CLOSEDDATES,STATUS)
                                VALUES (SOURCE.DOC_NBR, SOURCE.SERNO, SOURCE.DVV01, SOURCE.DVV02, SOURCE.DVV03, SOURCE.DVV09, SOURCE.DVV10, SOURCE.DVV04, SOURCE.DVV07, SOURCE.COMMENTS, SOURCE.ISCLOSED, SOURCE.CLOSEDDATES,SOURCE.STATUS);
                            ";


        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    // 2. 修正參數綁定，確保每個參數對應正確的 SQL 變數名稱
                    cmd.Parameters.AddWithValue("@DOC_NBR", (object)DOC_NBR ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SERNO", (object)SERNO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DVV01", (object)DVV01 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DVV02", (object)DVV02 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DVV03", (object)DVV03 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DVV09", (object)DVV09 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DVV10", (object)DVV10 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DVV04", (object)DVV04 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DVV07", (object)DVV07 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@COMMENTS", (object)COMMENTS ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ISCLOSED", (object)ISCLOSED ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CLOSEDDATES", (object)CLOSEDDATES ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@STATUS", (object)STATUS ?? DBNull.Value);

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
        BindGrid2();
    }

    #endregion
}