using ClosedXML.Excel;
using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
/// <summary>
/// 資料表SCHEMA輸出工具
/// </summary>
public partial class CDS_KYTUtils_TableSchema : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("資料表SCHEMA輸出工具", Request.Url.AbsoluteUri);
            txtConnectionString.Text = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
            btnRefresh_Click(null, null);
        }
    }
    /// <summary>
    /// 匯出EXCEL按下事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        List<string> lstTables = new List<string>();
        foreach (string key in Request.Form.AllKeys)
        {
            if (key.StartsWith("chk_TS_"))
            {
                string tableName = key.Substring(7).Trim();
                lstTables.Add(tableName);
            }
        }
        if (lstTables.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), @"
                alert('未選擇任何資料表');
            ", true);
            return;
        }
        using (SqlConnection Connection = new SqlConnection(txtConnectionString.Text))
        {
            Connection.Open();
            XLWorkbook book = new XLWorkbook();
            using (MemoryStream stream = new MemoryStream())
            {
                foreach (string tableName in lstTables)
                {
                    string sheetName = null;
                    int index = 0;
                    if (tableName.Length > 28)
                    {
                        sheetName = tableName.Substring(0, 28);
                        index++;
                        sheetName += "_" + index.ToString("0#");
                    }
                    else sheetName = tableName;
                    while (true)
                    {
                        try
                        {
                            book.Worksheets.Add(sheetName);
                            break;
                        }
                        catch
                        {
                            sheetName = tableName.Substring(0, 28);
                            index++;
                            sheetName += "_" + index.ToString("0#");
                        }
                    }
                    IXLWorksheet sheet = null;
                    book.TryGetWorksheet(sheetName, out sheet);
                    string tableDescription = ""; // 資料表說明
                    if (sheet != null)
                    {
                        // 查詢資料庫說明
                        using (SqlDataAdapter sda = new SqlDataAdapter(@"
                            SELECT ep.value AS 'Description'
                              FROM sys.objects
                       CROSS APPLY fn_listextendedproperty(default,
                                                           'SCHEMA', schema_name(schema_id),
                                                           'TABLE', name, null, null) ep
                            WHERE sys.objects.name = @TABLE_NAME
                         ORDER BY sys.objects.name
                        ", Connection))
                        using (DataSet ds = new DataSet())
                        {
                            sda.SelectCommand.Parameters.AddWithValue("@TABLE_NAME", tableName);
                            try
                            {
                                if (sda.Fill(ds) == 1)
                                {
                                    DataRow row = ds.Tables[0].Rows[0];
                                    tableDescription = row[0] != Convert.DBNull ? (string)row[0] : "";
                                }
                            }
                            catch { }
                        }

                        // A1-F1 資料表名稱
                        // A2-F2 資料表說明
                        // A3-F3 欄位名稱/型別(長度)/是否允許NULL/預設值/說明

                        sheet.Range("B1:F1").Merge();
                        sheet.Cell("A1").Value = "資料表名稱";
                        sheet.Cell("B1").Value = tableName;

                        sheet.Range("B2:F2").Merge();
                        sheet.Cell("A2").Value = "資料表說明";
                        sheet.Cell("B2").Value = tableDescription;

                        sheet.Cell("A3").Value = "主索引";
                        sheet.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        sheet.Cell("B3").Value = "欄位名稱";
                        sheet.Cell("C3").Value = "型別(長度)";
                        sheet.Cell("D3").Value = "允許NULL";
                        sheet.Cell("D3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        sheet.Cell("E3").Value = "預設值";
                        sheet.Cell("E3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        sheet.Cell("F3").Value = "欄位說明";

                        // 查詢資料表結構
                        using (SqlDataAdapter sda = new SqlDataAdapter(@"
                            SELECT *,
                                   ISNULL((SELECT TOP 1 [value]
                                             FROM sys.extended_properties AS B
                                            WHERE B.major_id = OBJECT_ID(A.TABLE_NAME)
                                              AND B.minor_id = A.ORDINAL_POSITION), '') AS 'DESCR',
                                   ISNULL((SELECT CONVERT(BIT, 1)
                                             FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
                                             JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K
                                               ON C.TABLE_NAME = K.TABLE_NAME
                                              AND C.CONSTRAINT_CATALOG = K.CONSTRAINT_CATALOG
                                              AND C.CONSTRAINT_SCHEMA = K.CONSTRAINT_SCHEMA
                                              AND C.CONSTRAINT_NAME = K.CONSTRAINT_NAME
                                            WHERE C.CONSTRAINT_TYPE = 'PRIMARY KEY'
                                              AND C.TABLE_NAME = @TABLE_NAME
                                              AND K.COLUMN_NAME = A.COLUMN_NAME), CONVERT(BIT, 0)) AS 'IS_PRIMARY_KEY'
                              FROM INFORMATION_SCHEMA.COLUMNS AS A
                             WHERE TABLE_NAME = @TABLE_NAME
                        ", Connection))
                        using (DataSet ds = new DataSet())
                        {
                            sda.SelectCommand.Parameters.AddWithValue("@TABLE_NAME", tableName);
                            try
                            {
                                sda.Fill(ds);
                                int rowIndex = 4;
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    sheet.Cell(rowIndex, 1).Value = (bool)row["IS_PRIMARY_KEY"] ? "YES" : "NO";
                                    sheet.Cell(rowIndex, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    sheet.Cell(rowIndex, 2).Value = row["COLUMN_NAME"].ToString();
                                    sheet.Cell(rowIndex, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    string dataType = ((string)row["DATA_TYPE"]).ToUpper();
                                    int CHARACTER_MAXIMUM_LENGTH = row["CHARACTER_MAXIMUM_LENGTH"] != Convert.DBNull ? (int)row["CHARACTER_MAXIMUM_LENGTH"] : -1;
                                    int NUMERIC_PRECISION = row["NUMERIC_PRECISION"] != Convert.DBNull ? int.Parse(row["NUMERIC_PRECISION"].ToString()) : -1;
                                    int NUMERIC_SCALE = row["NUMERIC_SCALE"] != Convert.DBNull ? int.Parse(row["NUMERIC_SCALE"].ToString()) : -1;
                                    switch (dataType)
                                    {
                                        case "NVARCHAR":
                                        case "VARCHAR":
                                        case "NCHAR":
                                        case "CHAR":
                                            if (CHARACTER_MAXIMUM_LENGTH > 0)
                                                dataType = string.Format("{0}({1})", dataType, CHARACTER_MAXIMUM_LENGTH);
                                            else
                                                dataType = string.Format("{0}(MAX)", dataType);
                                            break;
                                        case "DECIMAL":
                                            if (NUMERIC_PRECISION > 0)
                                            {
                                                dataType = string.Format("{0}({1}, {2})", dataType, NUMERIC_PRECISION, NUMERIC_SCALE);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    sheet.Cell(rowIndex, 3).Value = dataType;
                                    sheet.Cell(rowIndex, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    sheet.Cell(rowIndex, 4).Value = row["IS_NULLABLE"].ToString();
                                    sheet.Cell(rowIndex, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    sheet.Cell(rowIndex, 5).Value = row["COLUMN_DEFAULT"].ToString();
                                    sheet.Cell(rowIndex, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    sheet.Cell(rowIndex, 6).Value = row["DESCR"].ToString();
                                    sheet.Cell(rowIndex, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    rowIndex++;
                                }
                            }
                            catch { }
                        }

                        sheet.Columns("A:B:C:D:E:F").AdjustToContents();
                    }
                }
                book.SaveAs(stream);
                string name = Guid.NewGuid().ToString();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", name));
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                Response.End();
            }
        }
    }
    /// <summary>
    /// 重新整理按下事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        using (SqlConnection Connection = new SqlConnection(txtConnectionString.Text))
        {
            try { Connection.Open(); }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"
                    alert('{0}');
                ", ex.Message), true);
                return;
            }

            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT *
                  FROM INFORMATION_SCHEMA.TABLES
                 ORDER BY TABLE_NAME;
            ", Connection))
            using (DataSet ds = new DataSet())
            {
                try
                {
                    sda.Fill(ds);
                    decimal columns = 4m; // checkbox list橫向欄位數量
                    Table tblMain = new Table();
                    divTables.Controls.Add(tblMain);
                    int size = (int)Math.Ceiling((decimal)ds.Tables[0].Rows.Count / columns);
                    for (int i = 0; i < size; i++)
                    {
                        TableRow trMain = new TableRow();
                        tblMain.Rows.Add(trMain);
                        for (int j = 0; j < 6; j++)
                        {
                            TableCell tdMain = new TableCell();
                            trMain.Cells.Add(tdMain);
                        }
                    }
                    int rowIndex = 0;
                    int cellIndex = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        LiteralControl checkbox = new LiteralControl(string.Format("<input type='checkbox' name='chk_TS_{0}' /><span>{0}</span>", row["TABLE_NAME"]));
                        TableRow trMain = tblMain.Rows[rowIndex];
                        TableCell tdMain = trMain.Cells[cellIndex];
                        tdMain.Controls.Add(checkbox);
                        rowIndex++;
                        if (rowIndex == size)
                        {
                            rowIndex = 0;
                            cellIndex++;
                        }
                    }
                }
                catch { }
            }
        }
    }
}
