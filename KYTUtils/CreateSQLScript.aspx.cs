using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

// TODO: 2020/06/02 資料表沒有nvarcahr(MAX)出錯的問題

/**
* 修改時間：2020/07/16
* 修改人員：陳緯榕
* 修改項目：
    * 選擇變更系列都會出錯，並且沒有註解
* 發生原因：
    * 組合出的語法是錯的：「ALTER TABLE [dbo].[BUDGET] ADD [decimal](15,3) NULL」
* 修改位置：
    * 「lbtnExport_Click」中，
        * 新增變數〈sqlCommand_Modify_DESC〉，用來存放變更系列的說明
        * 在〈處理欄位及新增/移除時的預設值〉區塊時，
            * 當〈ddlMotion〉包含〈Modify〉時，〈sqlCommand_Drop_DESC〉都要加上〈移除描述〉的語法
            * 〈sqlCommand_Modify_ADD〉的字串組合中，第〈0〉個應該要放〈ktxtTABLE_NAME〉
            * 任何〈處理PK〉的區塊再新增語法前，都先需判斷〈lsPK_Columns.Count > 0〉
            * 〈sqlCommand_Common〉最後再組合一組〈sqlCommand_Modify_DESC〉的內容進去
* **/

/// <summary>
/// 使用冠永騰規格建立SQL語法
/// </summary>
public partial class CDS_KYTUtils_CreateSQLScript : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("使用冠永騰規格建立SQL語法", Request.Url.AbsoluteUri);
            gvColumns.DataSource = getDefalutTable();
            gvColumns.DataBind();
        }
    }

    #region 非控制事項事件
    private DataTable getDefalutTable()
    {
        DataTable dtReturn = new DataTable();
        dtReturn.Columns.Add("PK", typeof(string)); // 鍵值
        dtReturn.Columns.Add("COLUMN_NAME", typeof(string)); // 欄位名稱
        dtReturn.Columns.Add("DATA_TYPE", typeof(string)); // 欄位類型
        dtReturn.Columns.Add("ALLOW_NULL", typeof(string)); // 允許NULL
        dtReturn.Columns.Add("DEFAULT_VALUE", typeof(string)); // 預設值
        dtReturn.Columns.Add("COLUMN_DESC", typeof(string)); // 說明
        return dtReturn;
    }

    #endregion 非控制事項事件

    #region 控制事項事件
    /// <summary>
    /// 選擇檔案後觸發的上傳檔案事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rauXLSXUpload_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
    {
        if (rauXLSXUpload.UploadedFiles.Count > 0) // 判斷是否有上傳檔案
        {
            List<string> lsTempPath = new List<string>();
            foreach (Telerik.Web.UI.UploadedFile postedFile in rauXLSXUpload.UploadedFiles) // 巡覽上傳的所有檔案
            {
                if (postedFile != null)
                {
                    string extensionName = System.IO.Path.GetExtension(postedFile.FileName);
                    if (extensionName.Equals(".xls") || extensionName.Equals(".xlsx")) // 沒有
                    {
                        using (FileStream filestream = postedFile.InputStream as FileStream)
                        using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook(filestream))
                        {
                            lsTempPath.Add(filestream.Name);
                            ClosedXML.Excel.IXLWorksheet sheet = null;
                            try
                            {
                                sheet = wb.Worksheets.Worksheet("Sheet1");
                            }
                            catch
                            {
                                sheet = wb.Worksheets.Worksheet("工作表1");
                            }
                            if (sheet == null) // 找不到工作表就離開
                                break;
                            ktxtTABLE_NAME.Text = sheet.Cell(1, 1).GetString(); // 表格名稱
                            int rowIndex = 3;
                            DataTable dtSource = getDefalutTable(); // 每次都當最新的
                            while (!sheet.Cell(rowIndex, 2).IsEmpty()) // 欄位名稱是空的
                            {
                                DataRow ndr = dtSource.NewRow();
                                ndr["PK"] = sheet.Cell(rowIndex, 1).GetString().ToUpper().Trim(); // 鍵值
                                ndr["COLUMN_NAME"] = sheet.Cell(rowIndex, 2).GetString().ToUpper().Trim(); // 欄位名稱
                                ndr["DATA_TYPE"] = sheet.Cell(rowIndex, 3).GetString().ToUpper().Trim(); // 欄位類型
                                ndr["ALLOW_NULL"] = sheet.Cell(rowIndex, 4).GetString().ToUpper().Trim(); // 允許NULL
                                ndr["DEFAULT_VALUE"] = ""; // 預設值
                                ndr["COLUMN_DESC"] = sheet.Cell(rowIndex, 5).GetString().ToUpper().Trim(); // 說明
                                dtSource.Rows.Add(ndr);
                                rowIndex++;
                            }
                            ViewState["gvColumns"] = dtSource;
                            gvColumns.DataSource = ViewState["gvColumns"];
                            gvColumns.DataBind();
                        }
                    }

                }
            }
            // 刪除暫存檔
            foreach (string _tmpPath in lsTempPath)
            {
                File.Delete(_tmpPath);
            }
        }
    }

    protected void gvColumns_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblColumns = gvColumns.DataTable;
            DataRow row = tblColumns.Rows[gr.DataItemIndex];
            KYTCheckBox kcbPK = (KYTCheckBox)gr.FindControl("kcbPK"); // 鍵值
            KYTCheckBox kcbALLOW_NULL = (KYTCheckBox)gr.FindControl("kcbALLOW_NULL"); // 允許NULL
            KYTTextBox ktxtCOLUMN_NAME = (KYTTextBox)gr.FindControl("ktxtCOLUMN_NAME"); // 欄位名稱
            KYTTextBox ktxtDATA_TYPE = (KYTTextBox)gr.FindControl("ktxtDATA_TYPE"); // 欄位類型
            KYTTextBox ktxtDEFAULT_VALUE = (KYTTextBox)gr.FindControl("ktxtDEFAULT_VALUE"); // 預設值
            KYTTextBox ktxtCOLUMN_DESC = (KYTTextBox)gr.FindControl("ktxtCOLUMN_DESC"); // 說明
            kcbPK.ViewType = KYTViewType.Input;
            kcbALLOW_NULL.ViewType = KYTViewType.Input;
            ktxtCOLUMN_NAME.ViewType = KYTViewType.Input;
            ktxtDATA_TYPE.ViewType = KYTViewType.Input;
            ktxtDEFAULT_VALUE.ViewType = KYTViewType.Input;
            ktxtCOLUMN_DESC.ViewType = KYTViewType.Input;
            kcbPK.Checked = row["PK"].ToString().ToUpper() == "Y";
            kcbALLOW_NULL.Checked = row["ALLOW_NULL"].ToString().ToUpper() == "Y";

        }
    }
    #endregion 控制事項事件

    protected void lbtnExport_Click(object sender, EventArgs e)
    {
        string sqlCommand = "";
        string sqlCommand_Create_DESC = "";
        string sqlCommand_Create_PK = "";
        string sqlCommand_Create_Default = "";
        string sqlCommand_Create = "";
        string sqlCommand_Drop_DESC = "";
        string sqlCommand_Drop = "";
        string sqlCommand_Drop_Default = "";
        string sqlCommand_Common = "";
        string sqlCommand_Modify_ADD = "";
        string sqlCommand_Modify_UPDATE = "";
        string sqlCommand_Modify_DESC = "";
        string sqlCommand_Modify_DEL_PK = "";
        string sqlCommand_Modify_DEL_DEFAULT = "";
        string sqlCommand_Modify_DEL_COLUMNS = "";
        List<string> lsDefaultValue_Columns = new List<string>(); // 收納有預設值的欄位名稱
        List<string> lsDefaultValue = new List<string>(); // 收納有預設值的預設值
        List<string> lsPK_Columns = new List<string>(); // 收納有PK的欄位名稱
        DataTable dtSource = new DataTable();
        foreach (DataRow dr in gvColumns.DataTable.Rows)
        {
            if (!string.IsNullOrEmpty(dr["COLUMN_NAME"].ToString()) && // 欄位名稱
                !string.IsNullOrEmpty(dr["COLUMN_NAME"].ToString())) // 欄位類型
            {
                DataRow ndr = dtSource.NewRow();
                foreach (DataColumn dc in gvColumns.DataTable.Columns)
                {
                    if (!dtSource.Columns.Contains(dc.ColumnName))
                        dtSource.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                    ndr[dc.ColumnName] = dr[dc.ColumnName];
                }
                dtSource.Rows.Add(ndr);
            }
        }

        #region 處理欄位及新增/移除時的預設值
        foreach (DataRow dr in dtSource.Rows)
        {
            if (ddlMotion.SelectedValue.Contains("Modify"))
            {
                sqlCommand_Drop_DESC += string.Format(@"
                    --- 移除說明 ---
                    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{1}'
                    GO {2}
                    --- 移除說明 ---
                        ",
                ktxtTABLE_NAME.Text.Trim(),
                dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                Environment.NewLine);
            }
            switch (ddlMotion.SelectedValue)
            {
                case "Drop": // 移除說明

                    break;
                case "Create":
                case "Create_Drop":
                    if (ddlMotion.SelectedValue.Contains("Drop"))
                    {
                        // 移除說明
                        sqlCommand_Drop_DESC += string.Format(@"
                            --- 移除說明 ---
                            EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{1}'
                            GO {2}
                            --- 移除說明 ---
                                ",
                          ktxtTABLE_NAME.Text.Trim(),
                          dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                          Environment.NewLine);
                    }
                    sqlCommand_Create_DESC += string.Format(@"
                                --- 新增說明 ---
                                EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{2}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{1}'
                                GO{3}
                                --- 新增說明 ---
                                    ",
                                  ktxtTABLE_NAME.Text.Trim(),
                                  dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                                  dr["COLUMN_DESC"].ToString().ToUpper().Trim(),
                                  Environment.NewLine);
                    break;
                case "Modify_Ins":
                    sqlCommand_Modify_ADD += string.Format(@"
                                --- 新增欄位 ---
                                ALTER TABLE [dbo].[{0}] ADD [{1}]{2}{3} {4}
                                GO{5}
                                --- 新增欄位 ---
                                ",
                              ktxtTABLE_NAME.Text.Trim(),
                              dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                              dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(')[0],
                              dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(').Length > 1 ? "(" + dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(')[1] : "",
                              dr["ALLOW_NULL"].ToString().ToUpper().Trim() == "Y" ? "NULL" : "NOT NULL",
                              Environment.NewLine);

                    sqlCommand_Modify_DESC += string.Format(@"
                                --- 新增說明 ---
                                EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{2}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{1}'
                                GO{3}
                                --- 新增說明 ---
                                    ",
                              ktxtTABLE_NAME.Text.Trim(),
                              dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                              dr["COLUMN_DESC"].ToString().ToUpper().Trim(),
                              Environment.NewLine);
                    break;
                case "Modify_Update":
                    sqlCommand_Modify_UPDATE += string.Format(@"
                                --- 新增欄位 ---
                                ALTER TABLE [dbo].[{0}] ALTER COLUMN [{1}]{2}{3} {4}
                                GO{5}
                                --- 新增欄位 ---
                                    ",
                              ktxtTABLE_NAME.Text.Trim(),
                              dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                              dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(')[0],
                              dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(').Length > 1 ? "(" + dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(')[1] : "",
                              dr["ALLOW_NULL"].ToString().ToUpper().Trim() == "Y" ? "NULL" : "NOT NULL",
                              Environment.NewLine);

                    sqlCommand_Modify_DESC += string.Format(@"
                                --- 新增說明 ---
                                EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{2}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{1}'
                                GO{3}
                                --- 新增說明 ---
                                    ",
                              ktxtTABLE_NAME.Text.Trim(),
                              dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                              dr["COLUMN_DESC"].ToString().ToUpper().Trim(),
                              Environment.NewLine);
                    break;
                case "Modify_Drop":
                    sqlCommand_Modify_DEL_COLUMNS += string.Format(@"
                                --- 移除欄位 ---
                                ALTER TABLE [dbo].[{0}] DROP COLUMN [{1}]
                                GO {2}
                                --- 移除欄位 ---
                                    ",
                               ktxtTABLE_NAME.Text.Trim(),
                               dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                               Environment.NewLine);
                    break;
            }
            if (!string.IsNullOrEmpty(dr["DEFAULT_VALUE"].ToString()))
            {
                lsDefaultValue_Columns.Add(dr["COLUMN_NAME"].ToString().ToUpper().Trim());
                lsDefaultValue.Add(dr["DEFAULT_VALUE"].ToString().ToUpper().Trim());
            }
            if (!string.IsNullOrEmpty(dr["PK"].ToString()) &&
                dr["PK"].ToString().ToUpper() == "Y")
            {
                lsPK_Columns.Add(dr["COLUMN_NAME"].ToString().ToUpper().Trim());
            }
        }
        #endregion 處理欄位及新增/移除時的預設值
        #region 處理預設值

        for (int i = 0; i < lsDefaultValue_Columns.Count; i++)
        {
            string str_DefaultValues = lsDefaultValue_Columns[i];
            switch (ddlMotion.SelectedValue)
            {
                case "Drop":
                    // do nothing
                    break;
                case "Create":
                case "Create_Drop":
                    if (ddlMotion.SelectedValue.Contains("Drop"))
                        sqlCommand_Drop_Default += string.Format(@"
                                    --- 移除原本設定的預設值 ---                
                                    ALTER TABLE [dbo].[{0}] DROP CONSTRAINT [DF_{0}_{1}]
                                    GO {2}
                                    --- 移除原本設定的預設值 ---
                                    ",
                                       ktxtTABLE_NAME.Text.Trim(),
                                       str_DefaultValues,
                                       Environment.NewLine);
                    sqlCommand_Create_Default += string.Format(@"
                                    --- 新增預設值 ---
                                    ALTER TABLE [dbo].[{0}] ADD CONSTRAINT [DF_{0}_{1}]  DEFAULT ('{2}') FOR [{1}]
                                    GO {3}
                                    --- 新增預設值 ---
                                    ",
                                        ktxtTABLE_NAME.Text.Trim(),
                                        str_DefaultValues,
                                        lsDefaultValue[i],
                                        Environment.NewLine);
                    break;
                case "Modify_Ins":
                    sqlCommand_Modify_ADD += string.Format(@"
                                ------- 變更資料表內欄位預設值 --------
                                --- 移除原本設定的預設值 ---
                                IF EXISTS (SELECT T.NAME AS 'TABLENAME',
				                                  C.NAME AS 'COLUMNNAME',
				                                  DC.NAME,
				                                  DC.DEFINITION
			                                 FROM SYS.TABLES T
	                                   INNER JOIN SYS.DEFAULT_CONSTRAINTS DC 
			                                   ON T.OBJECT_ID = DC.PARENT_OBJECT_ID
	                                   INNER JOIN SYS.COLUMNS C 
		                                       ON DC.PARENT_OBJECT_ID = C.OBJECT_ID 
			                                  AND C.COLUMN_ID = DC.PARENT_COLUMN_ID
			                                WHERE T.NAME = '{0}'
			                                  AND DC.NAME = 'DF_{0}_{1}')
	                                BEGIN
		                                    ALTER TABLE [dbo].[{0}] 
	                                    DROP CONSTRAINT [DF_{0}_{1}]
	                                END
                                    --- 移除原本設定的預設值 ---
                                    --- 新增預設值 ---
                                    ALTER TABLE [dbo].[{0}] ADD CONSTRAINT [DF_{0}_{1}] DEFAULT ('{2}') FOR [{1}]
                                    --- 新增預設值 ---
                                    GO {3}
                                ------- 變更資料表內欄位預設值 --------
                                ",
                      ktxtTABLE_NAME.Text.Trim(),
                      str_DefaultValues,
                      lsDefaultValue[i],
                      Environment.NewLine);
                    break;
                case "Modify_Update":
                    sqlCommand_Modify_UPDATE += string.Format(@"
                                ------- 變更資料表內欄位預設值 --------
                                --- 移除原本設定的預設值 ---
                                IF EXISTS (SELECT T.NAME AS 'TABLENAME',
				                                  C.NAME AS 'COLUMNNAME',
				                                  DC.NAME,
				                                  DC.DEFINITION
			                                 FROM SYS.TABLES T
	                                   INNER JOIN SYS.DEFAULT_CONSTRAINTS DC 
			                                   ON T.OBJECT_ID = DC.PARENT_OBJECT_ID
	                                   INNER JOIN SYS.COLUMNS C 
		                                       ON DC.PARENT_OBJECT_ID = C.OBJECT_ID 
			                                  AND C.COLUMN_ID = DC.PARENT_COLUMN_ID
			                                WHERE T.NAME = '{0}'
			                                  AND DC.NAME = 'DF_{0}_{1}')
	                                BEGIN
		                                    ALTER TABLE [dbo].[{0}] 
	                                    DROP CONSTRAINT [DF_{0}_{1}]
	                                END
                                    --- 移除原本設定的預設值 ---
                                    --- 新增預設值 ---
                                    ALTER TABLE [dbo].[{0}] ADD CONSTRAINT [DF_{0}_{1}] DEFAULT ('{2}') FOR [{1}]
                                    --- 新增預設值 ---
                                    GO {3}
                                ------- 變更資料表內欄位預設值 --------
                                ",
                            ktxtTABLE_NAME.Text.Trim(),
                            str_DefaultValues,
                            lsDefaultValue[i],
                            Environment.NewLine);
                    break;
                case "Modify_Drop":
                    sqlCommand_Modify_DEL_DEFAULT += string.Format(@"
                                    --- 移除原本設定的預設值 ---
                                    ALTER TABLE [dbo].[{0}] DROP CONSTRAINT [DF_{0}_{1}]
                                    GO {2}
                                    --- 移除原本設定的預設值 ---
                                        ",
                           ktxtTABLE_NAME.Text.Trim(),
                           str_DefaultValues,
                           Environment.NewLine);
                    break;
            }
        }


        #endregion 處理預設值

        string str_Normal_Columns = "";
        bool hasTEXTIMAGE_ON = false;
        switch (ddlMotion.SelectedValue)
        {
            case "Create":
                foreach (DataRow dr in dtSource.Rows)
                {
                    str_Normal_Columns += string.Format(@"[{0}] [{1}]{2} {3},{4}",
                                        dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                                        dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(')[0],
                                        dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(').Length > 1 ? "(" + dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(')[1] : "",
                                        dr["ALLOW_NULL"].ToString().ToUpper().Trim() == "Y" ? "NULL" : "NOT NULL",
                                        Environment.NewLine);
                    if (dr["DATA_TYPE"].ToString().ToLower().Trim() == "text" ||
                        dr["DATA_TYPE"].ToString().ToLower().Trim() == "ntext" ||
                        dr["DATA_TYPE"].ToString().ToLower().Trim() == "varchar(max)" ||
                        dr["DATA_TYPE"].ToString().ToLower().Trim() == "nvarchar(max)" ||
                        dr["DATA_TYPE"].ToString().ToLower().Trim() == "image" ||
                        dr["DATA_TYPE"].ToString().ToLower().Trim() == "xml" ||
                        dr["DATA_TYPE"].ToString().ToLower().Trim() == "varbinary(max)")
                    {
                        hasTEXTIMAGE_ON = true;
                    }
                }
                if (lsPK_Columns.Count == 0)
                    str_Normal_Columns = str_Normal_Columns.Substring(0, str_Normal_Columns.Length - 1); // 去掉最後的逗號
                else // 有PK
                {
                    string strPK_COL = "";
                    foreach (string str_PKs in lsPK_Columns)
                    {
                        strPK_COL += string.Format(@"[{0}] ASC,", str_PKs);
                    }
                    strPK_COL = strPK_COL.Length > 0 ? strPK_COL.Substring(0, strPK_COL.Length - 1) : strPK_COL;
                    sqlCommand_Create_PK += string.Format(@"
                        CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
                            (
	                            {1}
                            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]{2}",
                         ktxtTABLE_NAME.Text.Trim(),
                         strPK_COL,
                         Environment.NewLine);
                    str_Normal_Columns += sqlCommand_Create_PK;
                }

                sqlCommand_Create += string.Format(@"
                                    --- 新增資料表 ---
                                        CREATE TABLE [dbo].[{0}](
                                            {1}
                                        ) ON [PRIMARY] {2}
                                        GO{3}
                                    --- 新增資料表 ---
                                    ",
                         ktxtTABLE_NAME.Text.Trim(),
                         str_Normal_Columns,
                         hasTEXTIMAGE_ON ? "TEXTIMAGE_ON [PRIMARY]" : "",
                         Environment.NewLine);
                break;
            case "Drop":
                sqlCommand_Drop += string.Format(@"
                                    --- 移除資料表 ---
                                    DROP TABLE [dbo].[{0}]
                                    GO {1}
                                    --- 移除資料表 ---
                                    ",
                         ktxtTABLE_NAME.Text.Trim(),
                         Environment.NewLine);
                break;

            case "Create_Drop":
                sqlCommand_Drop += string.Format(@"
                                    --- 移除資料表 ---
                                    DROP TABLE [dbo].[{0}]
                                    GO {1}
                                    --- 移除資料表 ---
                                        ",
                         ktxtTABLE_NAME.Text.Trim(),
                         Environment.NewLine);
                foreach (DataRow dr in dtSource.Rows)
                {
                    str_Normal_Columns += string.Format(@"[{0}] [{1}]{2} {3},{4}",
                                        dr["COLUMN_NAME"].ToString().ToUpper().Trim(),
                                        dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(')[0],
                                        dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(').Length > 1 ? "(" + dr["DATA_TYPE"].ToString().ToLower().Trim().Split('(')[1] : "",
                                        dr["ALLOW_NULL"].ToString().ToUpper().Trim() == "Y" ? "NULL" : "NOT NULL",
                                        Environment.NewLine);
                    if (dr["DATA_TYPE"].ToString().ToLower().Trim() == "text" ||
                       dr["DATA_TYPE"].ToString().ToLower().Trim() == "ntext" ||
                       dr["DATA_TYPE"].ToString().ToLower().Trim() == "varchar(max)" ||
                       dr["DATA_TYPE"].ToString().ToLower().Trim() == "nvarchar(max)" ||
                       dr["DATA_TYPE"].ToString().ToLower().Trim() == "image" ||
                       dr["DATA_TYPE"].ToString().ToLower().Trim() == "xml" ||
                       dr["DATA_TYPE"].ToString().ToLower().Trim() == "varbinary(max)")
                    {
                        hasTEXTIMAGE_ON = true;
                    }
                }
                if (lsPK_Columns.Count == 0)
                    str_Normal_Columns = str_Normal_Columns.Substring(0, str_Normal_Columns.Length - 1); // 去掉最後的逗號
                else // 有PK
                {
                    string strPK_COL = "";
                    foreach (string str_PKs in lsPK_Columns)
                    {
                        strPK_COL += string.Format(@"[{0}] ASC,", str_PKs);
                    }
                    strPK_COL = strPK_COL.Length > 0 ? strPK_COL.Substring(0, strPK_COL.Length - 1) : strPK_COL;
                    sqlCommand_Create_PK += string.Format(@"
                                    CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
                                    (
	                                    {1}
                                    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]{2}",
                                            ktxtTABLE_NAME.Text.Trim(),
                                            strPK_COL,
                                            Environment.NewLine);
                    str_Normal_Columns += sqlCommand_Create_PK;
                }

                sqlCommand_Create += string.Format(@"
                                    --- 設定語系 ---
                                    SET ANSI_NULLS ON
                                    GO {3}
                                    --- 設定語系 ---
                                    --- 設定識別碼可由雙引號分隔，且常值必須由單引號分隔  ---
                                    SET QUOTED_IDENTIFIER ON
                                    GO {3}
                                    --- 設定識別碼可由雙引號分隔，且常值必須由單引號分隔  ---
                                    --- 新增資料表 ---
                                    CREATE TABLE [dbo].[{0}]({3}
                                        {1}
                                    ) ON [PRIMARY] {2}
                                    GO{3}
                                    --- 新增資料表 ---
                                    ",
                                    ktxtTABLE_NAME.Text.Trim(),
                                    str_Normal_Columns,
                                    hasTEXTIMAGE_ON ? "TEXTIMAGE_ON [PRIMARY]" : "",
                                    Environment.NewLine);

                break;
            case "Modify_Ins":
                if (lsPK_Columns.Count > 0) // 有PK才新增
                {
                    sqlCommand_Modify_ADD += string.Format(@"
                    ----------- 變更資料表PK --------------
                    --- 移除資料表PK ---

                    IF ((SELECT PK.[NAME] AS PK_NAME
			                     FROM SYS.TABLES TAB
                      LEFT OUTER JOIN SYS.INDEXES PK
		                           ON TAB.OBJECT_ID = PK.OBJECT_ID 
			                      AND PK.IS_PRIMARY_KEY = 1
   			                    WHERE TAB.NAME = '{0}') IS NOT NULL) -- PK只有一個
	                    BEGIN 
	                      ALTER TABLE [dbo].[{0}] DROP CONSTRAINT PK_{0} -- 命名方式一致
	                    END
	                    

                    --- 移除資料表PK ---

                    --- 新增資料表PK ---
                        ALTER TABLE [dbo].[{0}]
                                ADD CONSTRAINT PK_{0} PRIMARY KEY ({1});

                    --- 新增資料表PK ---
                    ----------- 變更資料表PK --------------
                    GO{2}
                    ",
                 ktxtTABLE_NAME.Text.Trim(),
                 string.Join(", ", lsPK_Columns.ToArray()),
                 Environment.NewLine);
                }
                break;
            case "Modify_Update":
                if (lsPK_Columns.Count > 0) // 有PK才新增
                {
                    sqlCommand_Modify_UPDATE += string.Format(@"
                    ----------- 變更資料表PK --------------
                    --- 移除資料表PK ---

                    IF ((SELECT PK.[NAME] AS PK_NAME
			                     FROM SYS.TABLES TAB
                      LEFT OUTER JOIN SYS.INDEXES PK
		                           ON TAB.OBJECT_ID = PK.OBJECT_ID 
			                      AND PK.IS_PRIMARY_KEY = 1
   			                    WHERE TAB.NAME = '{0}') IS NOT NULL) -- PK只有一個
	                    BEGIN 
	                      ALTER TABLE [{0}] DROP CONSTRAINT PK_{0} -- 命名方式一致
	                    END
	                    

                    --- 移除資料表PK ---

                    --- 新增資料表PK ---
                        ALTER TABLE [{0}]
                                ADD CONSTRAINT PK_{0} PRIMARY KEY ({1});

                    --- 新增資料表PK ---
                    ----------- 變更資料表PK --------------
                    GO{2}
                    ",
                    ktxtTABLE_NAME.Text.Trim(),
                    string.Join(", ", lsPK_Columns.ToArray()),
                    Environment.NewLine);
                }
                break;
            case "Modify_Drop":
                if (lsPK_Columns.Count > 0) // 有PK才新增
                {
                    sqlCommand_Modify_DEL_PK += string.Format(@"
                    ----------- 變更資料表PK --------------
                    --- 移除資料表PK ---

                    IF ((SELECT PK.[NAME] AS PK_NAME
			                     FROM SYS.TABLES TAB
                      LEFT OUTER JOIN SYS.INDEXES PK
		                           ON TAB.OBJECT_ID = PK.OBJECT_ID 
			                      AND PK.IS_PRIMARY_KEY = 1
   			                    WHERE TAB.NAME = '{0}') IS NOT NULL) -- PK只有一個
	                    BEGIN 
	                      ALTER TABLE [{0}] DROP CONSTRAINT PK_{0} -- 命名方式一致
	                    END
	                    

                    --- 移除資料表PK ---
                    ----------- 變更資料表PK --------------
                    GO{1}
                    ",
                    ktxtTABLE_NAME.Text.Trim(),
                    Environment.NewLine);
                }
                break;
        }
        sqlCommand_Common += string.Format(@"{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                            sqlCommand_Drop_DESC,
                            sqlCommand_Drop_Default,
                            sqlCommand_Drop,
                            sqlCommand_Create,
                            sqlCommand_Create_Default,
                            sqlCommand_Create_DESC,
                            sqlCommand_Modify_ADD,
                            sqlCommand_Modify_UPDATE,
                            string.Format("{0}{1}{2}", sqlCommand_Modify_DEL_PK, sqlCommand_Modify_DEL_DEFAULT, sqlCommand_Modify_DEL_COLUMNS),
                            sqlCommand_Modify_DESC,
                            Environment.NewLine);
        ktxtShowScript.Text = sqlCommand_Common;
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        DataTable dtSource = gvColumns.DataTable;
        DataRow ndr = dtSource.NewRow();
        foreach (DataColumn dc in gvColumns.DataTable.Columns)
        {
            if (!dtSource.Columns.Contains(dc.ColumnName))
                dtSource.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
            ndr[dc.ColumnName] = "";
        }
        dtSource.Rows.Add(ndr);
        gvColumns.DataSource = dtSource;
        gvColumns.DataBind();
    }

    /// <summary>
    /// 按下下載範例檔按鈕後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnDownloadSample_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/CDS/KYTUtils/Assets/Examples/SQL表格上傳範例.xlsx");
        if (File.Exists(filePath))
        {
            ScriptManager.RegisterClientScriptBlock(
           UpdatePanel1,
           UpdatePanel1.GetType(),
           Guid.NewGuid().ToString(),
           string.Format(@"
                window.location = '{0}?filepath={1}';
            "
           , Page.ResolveUrl("~/CDS/KYTUtils/WebService/FORMPRINT/DownFileWithPath.ashx")
           , HttpUtility.UrlEncode(filePath)),
           true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), @"
                document.addEventListener('DOMContentLoaded', function() {
                    alert('範例檔不存在, 請聯繫系統管理員');
                });
            ", true);
    }




}
