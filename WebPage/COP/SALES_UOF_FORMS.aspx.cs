using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using Telerik.Web.UI;

public partial class CDS_WebPage_COP_SALES_UOF_FORMS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNTS = null;
    string NAMES = null;
    DataTable EXCELLDT = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDownList1();
            BindDropDownList2();

            BindGrid();
        }
    }


    #region FUNCTION
    private void BindDropDownList(DropDownList ddl, string sql, string textField, string valueField)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(sql));

        ddl.DataSource = dt;
        ddl.DataTextField = textField;
        ddl.DataValueField = valueField;
        ddl.DataBind();
    }

    private void BindDropDownList1()
    {
        string sql = @"
        SELECT '全部' AS NAMES
        UNION ALL
        SELECT [NAMES] FROM [TKMQ].[dbo].[TB_UOF_SALES_NAMES]";

        BindDropDownList(DropDownList1, sql, "NAMES", "NAMES");
    }

    private void BindDropDownList2()
    {
        string sql = @"
        SELECT '全部' AS FORM_NAME
        UNION ALL
        SELECT [FORM_NAME] FROM [TKMQ].[dbo].[TB_UOF_SALES_FORM_NAME]";

        BindDropDownList(DropDownList2, sql, "FORM_NAME", "FORM_NAME");
    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder filterUser = new StringBuilder();
        StringBuilder filterForm = new StringBuilder();
        StringBuilder filter_FLOW_NAME = new StringBuilder();
        StringBuilder filter_FLOW_BEGIN_TIME = new StringBuilder();

        // 申請者
        string String_DropDownList1 = DropDownList1.SelectedValue;
        if (!string.IsNullOrEmpty(String_DropDownList1) && String_DropDownList1 != "全部")
        {
            filterUser.Append(" AND 申請者=@UserName ");
            m_db.AddParameter("@UserName", String_DropDownList1);
        }

        // 表單名稱
        string String_DropDownList2 = DropDownList2.SelectedValue;
        if (!string.IsNullOrEmpty(String_DropDownList2) && String_DropDownList2 != "全部")
        {
            filterForm.Append(" AND 表單名稱=@FormName ");
            m_db.AddParameter("@FormName", String_DropDownList2);
        }


        // 目前簽核者
        string FLOW_NAME = QUERY_TextBox1.Text.Trim();
        if (!string.IsNullOrEmpty(FLOW_NAME) )
        {
            filter_FLOW_NAME.Append("  AND 目前簽核者 LIKE '%'+@FLOW_NAME+'%'  ");
            m_db.AddParameter("@FLOW_NAME", FLOW_NAME);
        }
        else
        {
            filter_FLOW_NAME.Append("  ");
        }

        // 申請日期
        string FLOW_BEGIN_TIME = txtSearchDate.Text.Trim();
        if (!string.IsNullOrEmpty(FLOW_BEGIN_TIME))
        {
            if (!string.IsNullOrEmpty(txtSearchDate.Text))
            {
                // 1. 先解析成 DateTime 物件
                DateTime parsedDate = DateTime.Parse(txtSearchDate.Text);
                // 2. 轉換為斜線格式的字串 (結果會是 "2026/06/16")
                string formattedDate = parsedDate.ToString("yyyy/MM/dd");
                filter_FLOW_BEGIN_TIME.Append(" AND 申請日期>=@FLOW_BEGIN_TIME ");
                m_db.AddParameter("@FLOW_BEGIN_TIME", formattedDate);
            }

           
        }
        else
        {
            filter_FLOW_BEGIN_TIME.Append("  ");
        }


        string sql = string.Format(@"
                                    SELECT *
                                    FROM 
                                    (
	                                    SELECT 
		                                    f.FORM_NAME       AS '表單名稱',
		                                    u.NAME            AS '申請者',
		                                    CONVERT(nvarchar,t.BEGIN_TIME,111) AS '申請日期',
		                                    t.DOC_NBR         AS '表單編號',   
		                                    (
			                                    SELECT STUFF(
				                                    (
					                                    SELECT ',' + u2.NAME
					                                    FROM [UOF].dbo.TB_WKF_TASK_NODE AS TN
					                                    LEFT JOIN [UOF].dbo.TB_EB_USER AS u2
						                                    ON u2.USER_GUID = TN.ORIGINAL_SIGNER
					                                    WHERE TN.SITE_ID = t.CURRENT_SITE_ID
					                                    FOR XML PATH(''), TYPE
				                                    ).value('.', 'nvarchar(max)')
			                                    ,1,1,'')
		                                    ) AS '目前簽核者',
		                                    CASE 
			                                    WHEN f.FORM_NAME = '1001.文宣樣品申請' 
				                                    THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""00010""]/@fieldValue)[1]', 'nvarchar(max)')  
			                                    WHEN f.FORM_NAME = '1001.品號申請單' 
				                                    THEN STUFF((
					                                    SELECT N',' + C.value('@fieldValue','nvarchar(200)')
					                                    FROM CURRENT_DOC.nodes('/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm001IT""]/DataGrid/Row/Cell[@fieldId=""RDFrm001IN""]') AS T(C)
					                                    FOR XML PATH(''), TYPE
				                                    ).value('.', 'nvarchar(max)'), 1, 1, '')           
			                                    WHEN f.FORM_NAME = '1001.工作交付單' 
				                                    THEN [UOF].dbo.StripHTML(t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""00010""]/@fieldValue)[1]', 'nvarchar(max)'))
			                                    WHEN f.FORM_NAME = '1002.客訴異常處理單' 
				                                    THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm002PRD""]/@fieldValue)[1]', 'nvarchar(max)') 
			                                    WHEN f.FORM_NAME = '1004.無品號試吃製作申請單' 
				                                    THEN STUFF((
					                                    SELECT N',' + C.value('@fieldValue','nvarchar(200)')
					                                    FROM CURRENT_DOC.nodes('/Form/FormFieldValue/FieldItem[@fieldId=""DETAILS""]/DataGrid/Row/Cell[@fieldId=""DVV01""]') AS T(C)
					                                    FOR XML PATH(''), TYPE
				                                    ).value('.', 'nvarchar(max)'), 1, 1, '')
			                                    WHEN f.FORM_NAME = '1004.品保業務工作申請單' 
				                                    THEN [UOF].dbo.StripHTML(t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""QCFrm004Process""]/@fieldValue)[1]', 'nvarchar(max)'))
			                                    WHEN f.FORM_NAME = '1005 研發業務工作申請單' 
				                                    THEN [UOF].dbo.StripHTML(t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm1003RS""]/@fieldValue)[1]', 'nvarchar(max)'))
			                                    WHEN f.FORM_NAME = '1005.舊品變更申請單' 
				                                    THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm1002PD""]/@fieldValue)[1]', 'nvarchar(max)')  
			                                    WHEN f.FORM_NAME = '1006.委外送驗申請單' 
				                                    THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""QC6007""]/DataGrid/Row[@order=""0""]/Cell[@fieldId=""QC60071""]/@fieldValue)[1]', 'nvarchar(200)')
			                                    WHEN f.FORM_NAME = '1008.無品號-烘培試吃製作申請單' 
				                                    THEN [UOF].dbo.StripHTML(t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""DV09""]/@fieldValue)[1]', 'nvarchar(max)'))
			                                    WHEN f.FORM_NAME = '2001.產品開發+包裝設計申請單' 
				                                    THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""FIELD3""]/@fieldValue)[1]', 'nvarchar(max)')  
                                                WHEN f.FORM_NAME = '2001A.產品開發+包裝設計申請單(行企專用)' 
				                                    THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""FIELD3""]/@fieldValue)[1]', 'nvarchar(max)')  

                                                ELSE NULL
		                                    END AS '表單標題',
		                                    t.CURRENT_SITE_ID,
		                                    t.TASK_ID,
		                                    t.CURRENT_DOC
	                                    FROM [UOF].dbo.TB_WKF_TASK AS t WITH (NOLOCK)
	                                    LEFT JOIN [UOF].dbo.TB_EB_USER AS u
		                                    ON u.USER_GUID = t.USER_GUID
	                                    LEFT JOIN [UOF].dbo.TB_EB_EMPL_DEP AS ed
		                                    ON ed.USER_GUID = u.USER_GUID AND ed.ORDERS = '0'
	                                    JOIN [UOF].dbo.TB_WKF_FORM_VERSION AS fv
		                                    ON t.FORM_VERSION_ID = fv.FORM_VERSION_ID
	                                    JOIN [UOF].dbo.TB_WKF_FORM AS f
		                                    ON f.FORM_ID = fv.FORM_ID
	                                    WHERE 1=1
		                                    AND ISNULL(t.TASK_STATUS,'') NOT IN ('2','3')
		                                    AND t.BEGIN_TIME >= '2025-01-01'
		                                    AND f.FORM_NAME COLLATE Chinese_Taiwan_Stroke_BIN IN (
			                                    SELECT [FORM_NAME]
			                                    FROM [192.168.1.105].[TKMQ].[dbo].[TB_UOF_SALES_FORM_NAME]
		                                    )
		                                    AND u.ACCOUNT COLLATE Chinese_Taiwan_Stroke_BIN IN (
			                                    SELECT [ID]
			                                    FROM [192.168.1.105].[TKMQ].[dbo].[TB_UOF_SALES_NAMES]
		                                    )
                                    ) AS TEMP
                                    WHERE 1=1
{0}
{1}
{2}
{3}                                  
                                    
                                    ORDER BY TEMP.表單名稱, TEMP.申請者, TEMP.表單編號;
                                ", filterUser, filterForm, filter_FLOW_NAME, filter_FLOW_BEGIN_TIME);

        DataTable dt = new DataTable();
        using (IDataReader reader = m_db.ExecuteReader(sql))
        {
            dt.Load(reader);
        }
        EXCELLDT = dt;

        Grid1.DataSource = dt.Rows.Count > 0 ? dt : null;
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
    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        BindGrid();
        if (EXCELLDT != null && EXCELLDT.Rows.Count >= 1)
        {
            SETEXCEL(EXCELLDT);
        }
    }

    public void SETEXCEL(DataTable dt)
    {

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "清單" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());

                //預設行高
                ws.DefaultRowHeight = 15;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "表單名稱";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 2].Value = "申請者";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 3].Value = "申請時間";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 4].Value = "表單編號";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 5].Value = "目前簽核者";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 6].Value = "表單標題";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線              




                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["表單名稱"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 2].Value = od["申請者"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 3].Value = od["申請時間"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 4].Value = od["表單編號"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 5].Value = od["目前簽核者"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 6].Value = od["表單標題"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線               



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
    public void MsgBox(string ex, Page pg, object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);
    }

    #endregion

    #region BUTTON
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    #endregion
}