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

public partial class CDS_WebPage_RESEARCH_RESEARCH_UOF_FORMS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNTS = null;
    string NAMES = null;
    DataTable EXCELLDT = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDownList1();         

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
        SELECT '全部' AS FORM_NAME
        UNION ALL
        SELECT  [FORM_NAME]  FROM [TKRESEARCH].[dbo].[TB_UOF_RESEARCH_FORM_NAME]
        ";

        BindDropDownList(DropDownList1, sql, "FORM_NAME", "FORM_NAME");
    }

  
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder filterUser = new StringBuilder();
        StringBuilder filterForm = new StringBuilder();

        // 申請者
        string String_DropDownList1 = DropDownList1.SelectedValue;
        if (!string.IsNullOrEmpty(String_DropDownList1) && String_DropDownList1 != "全部")
        {
            filterForm.Append(" AND f.FORM_NAME=@FormName ");
            m_db.AddParameter("@FormName", String_DropDownList1);
        }

        string sql = string.Format(@"
                                    -- 20250923 查所有未結案的研發表單
                                    SELECT 
                                        f.FORM_NAME       AS '表單名稱',
                                        u.NAME            AS '申請者',
                                        CONVERT(nvarchar,t.BEGIN_TIME,111)      AS '申請時間',
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
	                                     (
                                            SELECT STUFF(
                                                (
                                                    SELECT ',' + u2.NAME
                                                    FROM [UOF].dbo.TB_WKF_TASK_NODE AS TN
                                                    LEFT JOIN [UOF].dbo.TB_EB_USER AS u2
                                                        ON u2.USER_GUID = TN.ORIGINAL_SIGNER
                                                    WHERE TN.SITE_ID <> t.CURRENT_SITE_ID
				                                    AND TN.TASK_ID = t.TASK_ID
				                                    AND ISNULL(FINISH_TIME,'')=''
				                                    ORDER BY NODE_SEQ
                                                    FOR XML PATH(''), TYPE
                                                ).value('.', 'nvarchar(max)')
                                            ,1,1,'')
                                        ) AS '下關的簽核者',
                                        CASE         
                                            WHEN f.FORM_NAME = '1001.品號申請單' 
			                                    THEN STUFF((
					                                    SELECT N',' + C.value('@fieldValue','nvarchar(200)')
					                                    FROM CURRENT_DOC.nodes('/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm001IT""]/DataGrid/Row/Cell[@fieldId=""RDFrm001IN""]')  AS T(C)
					                                    FOR XML PATH(''), TYPE
				                                    ).value('.', 'nvarchar(max)'), 1, 1, '')  
		                                    WHEN f.FORM_NAME = '1002.商品變更及設計' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm1002DS""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '1003.BOM表變更申請單' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm1003RS""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '1004.無品號試吃製作申請單' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""DV09""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '1005 研發業務工作申請單' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm1003RS""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '1006.樣品試吃回覆單' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""F02""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '1007.自主研發申請書' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""DV09""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '1008.無品號-烘培試吃製作申請單' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""DV09""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '2001.產品開發+包裝設計申請單' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""FIELD3""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '2001A.產品開發+包裝設計申請單(行企專用)' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""FIELD3""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '9001.新品號通知單' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""MB002""]/@fieldValue)[1]', 'nvarchar(max)')  
		                                    WHEN f.FORM_NAME = '9002.品號變更通知' 
                                                THEN STUFF((
					                                    SELECT N',' + C.value('@fieldValue','nvarchar(200)')
					                                    FROM CURRENT_DOC.nodes('/Form/FormFieldValue/FieldItem[@fieldId=""DETAILS""]/DataGrid/Row/Cell[@fieldId=""TL007""]')  AS T(C)
					                                    FOR XML PATH(''), TYPE
				                                    ).value('.', 'nvarchar(max)'), 1, 1, '')  
		                                    WHEN f.FORM_NAME = '9003.新品號明細的通知' 
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""MB002""]/@fieldValue)[1]', 'nvarchar(max)')  
                                            WHEN f.FORM_NAME = '1005.舊品變更申請單'                                                        
                                                THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm1002PD""]/@fieldValue)[1]', 'nvarchar(max)')     

                                            ELSE NULL
                                        END AS '表單標題',
                                         CASE         
                                                WHEN f.FORM_NAME = '1001.品號申請單' 
			                                        THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""RDFrm001FD_1""]/@fieldValue)[1]', 'nvarchar(max)')        
		                                        WHEN f.FORM_NAME = '1002.商品變更及設計' 
                                                    THEN '' 
		                                        WHEN f.FORM_NAME = '1003.BOM表變更申請單' 
                                                    THEN ''     
		                                        WHEN f.FORM_NAME = '1004.無品號試吃製作申請單' 
                                                    THEN STUFF((
					                                        SELECT N',' + C.value('@fieldValue','nvarchar(200)')
					                                        FROM CURRENT_DOC.nodes('/Form/FormFieldValue/FieldItem[@fieldId=""DETAILS""]/DataGrid/Row/Cell[@fieldId=""DVV07""]')  AS T(C)
					                                        FOR XML PATH(''), TYPE
				                                        ).value('.', 'nvarchar(max)'), 1, 1, '')  
		                                        WHEN f.FORM_NAME = '1005 研發業務工作申請單' 
                                                    THEN ''
		                                        WHEN f.FORM_NAME = '1006.樣品試吃回覆單' 
                                                    THEN ''
		                                        WHEN f.FORM_NAME = '1007.自主研發申請書' 
                                                    THEN ''  
		                                        WHEN f.FORM_NAME = '1008.無品號-烘培試吃製作申請單' 
                                                    THEN STUFF((
					                                        SELECT N',' + C.value('@fieldValue','nvarchar(200)')
					                                        FROM CURRENT_DOC.nodes('/Form/FormFieldValue/FieldItem[@fieldId=""DETAILS""]/DataGrid/Row/Cell[@fieldId=""DVV07""]')  AS T(C)
					                                        FOR XML PATH(''), TYPE
				                                        ).value('.', 'nvarchar(max)'), 1, 1, '')  
		                                        WHEN f.FORM_NAME = '2001.產品開發+包裝設計申請單' 
                                                    THEN t.CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""FIELD5""]/@fieldValue)[1]', 'nvarchar(max)')      
		                                        WHEN f.FORM_NAME = '1005.舊品變更申請單' 
                                                    THEN '' 
                                                ELSE NULL
                                            END AS '申請者要求完成日',	                                       
                                        t.CURRENT_SITE_ID,
	                                    t.TASK_ID,
                                        t.CURRENT_DOC
                                        ,CONVERT(nvarchar,[PLANDATES],111) AS 'PLANDATES'
										,[COMMENTS]
                                    FROM [UOF].dbo.TB_WKF_TASK AS t WITH (NOLOCK)
                                    LEFT JOIN [192.168.1.105].[TKRESEARCH].[dbo].[TB_RESEARCH_UOF_FORMS] WITH (NOLOCK) ON [TB_RESEARCH_UOF_FORMS].[DOC_NBR]=t.[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_CI_AS
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
                                            SELECT  [FORM_NAME]
		                                    FROM [192.168.1.105].[TKRESEARCH].[dbo].[TB_UOF_RESEARCH_FORM_NAME]
	                                       )
                                    {0}
                                    ORDER BY f.FORM_NAME,t.DOC_NBR;

                                ",  filterForm);

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
            Button btn2 = (Button)e.Row.FindControl("Button2");
            if (btn2 != null)
            {
                string cellValue2 = btn2.CommandArgument;
                dynamic param2 = new { ID = cellValue2 }.ToExpando();
            }
        }
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
            // 從資料來源讀原始值（比較可靠）
            object val = DataBinder.Eval(e.Row.DataItem, "PLANDATES");
            object FORM_KIND = DataBinder.Eval(e.Row.DataItem, "表單名稱");
            object FORM_START = DataBinder.Eval(e.Row.DataItem, "申請時間");
            DateTime dt_FORM_START = Convert.ToDateTime(FORM_START.ToString());
            TextBox txt = e.Row.FindControl("txtPLANDATES") as TextBox;

            if (val == null || val == DBNull.Value || string.IsNullOrWhiteSpace(val.ToString()))
            {
                if(FORM_KIND.ToString().Equals("1001.品號申請單"))
                {
                    txt.Text = dt_FORM_START.AddDays(3).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("1002.商品變更及設計"))
                {
                    txt.Text = dt_FORM_START.AddDays(3).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("1003.BOM表變更申請單"))
                {
                    txt.Text = dt_FORM_START.AddDays(7).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("1004.無品號試吃製作申請單"))
                {
                    txt.Text = dt_FORM_START.AddDays(7).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("1005 研發業務工作申請單"))
                {
                    txt.Text = dt_FORM_START.AddDays(7).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("1006.樣品試吃回覆單"))
                {
                    txt.Text = dt_FORM_START.AddDays(7).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("1007.自主研發申請"))
                {
                    txt.Text = dt_FORM_START.AddDays(3).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("008.無品號-烘培試吃製作申請單"))
                {
                    txt.Text = dt_FORM_START.AddDays(14).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("2001.產品開發+包裝設計申請單單"))
                {
                    txt.Text = dt_FORM_START.AddDays(30).ToString("yyyy/MM/dd");
                }
                else if (FORM_KIND.ToString().Equals("1005.舊品變更申請單"))
                {
                    txt.Text = dt_FORM_START.AddDays(14).ToString("yyyy/MM/dd");
                }
                //e.Row.Cells[4].Text = "NA"; // 第4欄 => index 3
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    object obj = DataBinder.Eval(e.Row.DataItem, "作業預估完成日BY申請日");
        //    if (obj != DBNull.Value)
        //    {
        //        DateTime dateValue = Convert.ToDateTime(obj);

        //        if (dateValue.Date < DateTime.Today)
        //        {
        //            e.Row.ForeColor = System.Drawing.Color.Red;
        //        }
        //    }
        //}
    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
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
                TextBox txtNewField_GV1_txtPLANDATES = (TextBox)row.FindControl("txtPLANDATES");
                string newTextValue_GV1_txtPLANDATES = txtNewField_GV1_txtPLANDATES.Text;
                TextBox txtNewField_GV1_txtCOMMENTS = (TextBox)row.FindControl("txtCOMMENTS");
                string newTextValue_GV1_ttxtCOMMENTS = txtNewField_GV1_txtCOMMENTS.Text;

                Label Label_表單編號 = (Label)row.FindControl("表單編號");
                string DOC_NBR = Label_表單編號.Text;
                string PLANDATES = newTextValue_GV1_txtPLANDATES;
                string COMMENTS = newTextValue_GV1_ttxtCOMMENTS;

                ADD_TB_RESEARCH_UOF_FORMS(
                                DOC_NBR,
                                PLANDATES,
                                COMMENTS                                
                               );
                MsgBox(DOC_NBR + " 完成", this.Page, this);
            }

            BindGrid();

        }
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        BindGrid();
        if (EXCELLDT!=null && EXCELLDT.Rows.Count>=1)
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
                ws.Cells[1, 4].Value = "申請者要求完成日";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 5].Value = "作業預估完成日BY申請日";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 6].Value = "表單編號";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 7].Value = "目前簽核者";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 8].Value = "表單標題";
                ws.Cells[1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線




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
                    ws.Cells[ROWS, 4].Value = od["申請者要求完成日"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 5].Value = od["PLANDATES"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 6].Value = od["表單編號"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 7].Value = od["目前簽核者"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 8].Value = od["表單標題"].ToString();
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

    public void ADD_TB_RESEARCH_UOF_FORMS(
      string DOC_NBR,
      string PLANDATES,
      string COMMENTS
      )
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                          MERGE [TKRESEARCH].[dbo].[TB_RESEARCH_UOF_FORMS] AS TARGET -- 建議為目標表取別名，增加清晰度

                            USING (VALUES (@DOC_NBR, @PLANDATES, @COMMENTS)) AS SOURCE (DOC_NBR, PLANDATES, COMMENTS)
                            ON TARGET.DOC_NBR = SOURCE.DOC_NBR

                            WHEN MATCHED THEN 
                                UPDATE SET 
                                    PLANDATES = SOURCE.PLANDATES,
                                    COMMENTS = SOURCE.COMMENTS

                            WHEN NOT MATCHED THEN
                                -- 修正此處，將 COMMENTSS 改為 COMMENTS
                                INSERT (DOC_NBR, PLANDATES, COMMENTS) 
                                VALUES (SOURCE.DOC_NBR, SOURCE.PLANDATES, SOURCE.COMMENTS);                                             
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    // 傳入參數
                    cmd.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
                    cmd.Parameters.AddWithValue("@PLANDATES", PLANDATES);
                    cmd.Parameters.AddWithValue("@COMMENTS", COMMENTS);
          

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(DOC_NBR + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception EX)
        {
        }
        finally
        {
        }
    }
    #endregion

    #region BUTTON
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    #endregion
}