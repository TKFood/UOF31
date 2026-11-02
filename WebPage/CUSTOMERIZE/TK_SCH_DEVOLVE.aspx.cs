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

public partial class CDS_WebPage_CUSTOMERIZE_TK_SCH_DEVOLVE : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {    
            BindDropDownList();   
            BindGrid1("");
        }
        else
        {
        }
    }
    #region FUNCTION
  
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        DataRow row;
        dt.Columns.Add("KINDS", typeof(String));

        row = dt.NewRow();
        row["KINDS"] = "N";
        dt.Rows.Add(row);

        row = dt.NewRow();
        row["KINDS"] = "Y";
        dt.Rows.Add(row);

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "KINDS";
            DropDownList1.DataValueField = "KINDS";
            DropDownList1.DataBind();

        }
        else
        {

        }
    }

  




    private void BindGrid1(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();



        //狀態
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("Y"))
            {
                QUERYS.AppendFormat(@" AND W.WORK_STATE  IN ('Completed') ");
            }
            else if (DropDownList1.Text.Equals("N"))
            {
                QUERYS.AppendFormat(@"  AND W.WORK_STATE NOT IN ('Completed') ");
            }
        }

        //校稿名稱
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND W.SUBJECT LIKE  '%{0}%' ", TextBox1.Text);

        }

        //執行者
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS.AppendFormat(@" AND USER2.NAME LIKE  '%{0}%' ", TextBox2.Text);

        }

        //交付者
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            QUERYS.AppendFormat(@" AND USER1.NAME LIKE  '%{0}%' ", TextBox3.Text);

        }


        cmdTxt.AppendFormat(@"
                            -- 1. 先把 DEVOLE_GUID 對應的 userId 拉出來
                            WITH DEVOLVE_USERS AS (
                                SELECT 
                                    D.DEVOLVE_GUID,
                                    UserId.value('(.)', 'nvarchar(50)') AS UserId
                                FROM [UOF].dbo.TB_EIP_SCH_DEVOLVE D
                                CROSS APPLY D.USER_SET.nodes('/UserSet/Element/userId') AS X(UserId)
                            )

                            -- 2. 主查詢
                            SELECT 
                                W.WORK_GUID,
                                W.DEVOLVE_GUID,
                                W.SUBJECT,
                                W.WORK_STATE,
                                CASE 
                                    WHEN W.WORK_STATE = 'Completed' THEN '已回覆'
                                    WHEN W.WORK_STATE = 'NotYetBegin' THEN '還沒回覆'
                                    WHEN W.WORK_STATE = 'Audit' THEN '回覆完成，交付人審查中'
                                    WHEN W.WORK_STATE = 'Proceeding' THEN '有回覆，但沒有完成'
                                    ELSE W.WORK_STATE
                                END AS WORK_STATE_DESC,
                                USER1.NAME AS 交付者,
                                USER2.NAME AS 執行者,
                                CONVERT(nvarchar, W.CREATE_TIME, 112) AS CREATE_TIME,
                                CONVERT(nvarchar, W.END_TIME, 112) AS END_TIME,
                                (
                                    SELECT TOP 1 DESCRIPTION 
                                    FROM [UOF].dbo.TB_EIP_SCH_WORK_RECORD R
                                    WHERE R.WORK_GUID = W.WORK_GUID
                                    ORDER BY R.CREATE_TIME DESC
                                ) AS DESCRIPTION,
                                W.WORK_GUID,
                                W.CREATE_USER,
                                W.EXECUTE_USER
                            FROM [UOF].dbo.TB_EIP_SCH_WORK W
                            LEFT JOIN [UOF].dbo.TB_EB_USER USER1 ON USER1.USER_GUID = W.CREATE_USER
                            LEFT JOIN [UOF].dbo.TB_EB_USER USER2 ON USER2.USER_GUID = W.EXECUTE_USER
                            INNER JOIN DEVOLVE_USERS DU ON DU.DEVOLVE_GUID = W.DEVOLVE_GUID AND DU.UserId = W.EXECUTE_USER
                            WHERE 1=1
                                {0}
                            ORDER BY SUBJECT,CREATE_TIME DESC
                               
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

            Dialog.Open2(lbtnName, "~/CDS/WebPage/CUSTOMERIZE/TK_SCH_DEVOLVEDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);


        }

    }
    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "GWButton1")
        {

            BindGrid1("");

        }
      
       
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

    public void MsgBox(string ex, Page pg, object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);

        //MsgBox("完成", this.Page, this);
    }

    #endregion

    #region BUTTON

    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid1("");
    }

   


    #endregion
}