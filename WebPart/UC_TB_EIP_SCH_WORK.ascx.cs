using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CDS_WebPart_UC_TB_EIP_SCH_WORK : System.Web.UI.UserControl
{
    DataTable EXCELDT1 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDownList();
            BindDropDownList2();

            BindGrid2("");

            DateTime FirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime LastDay = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1);
            //DateTime.DaysInMonth()
            BindGrid(FirstDay.ToString("yyyy/MM/dd"), LastDay.ToString("yyyy/MM/dd"));

            //txtDate1.SelectedDate = FirstDay;
            //txtDate2.SelectedDate = LastDay;
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


        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @" SELECT '全部' AS SALESFOCUS UNION ALL SELECT SALESFOCUS  FROM  [TKBUSINESS].[dbo].[PRODUCTS]  GROUP BY SALESFOCUS ";

        //dt.Load(m_db.ExecuteReader(cmdTxt));

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

    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT
                            WORK_STATE_DESC
                        FROM
                        (
                            -- 1. 新增 '全部' (排序為 0)
                            SELECT
                                '全部' AS WORK_STATE_DESC,
                                0 AS SortOrder
    
                            UNION ALL
    
                            -- 2. 原始查詢的結果 (排序為 1)
                            SELECT
                                CASE
                                    WHEN W.WORK_STATE = 'Completed' THEN '已回覆'
                                    WHEN W.WORK_STATE = 'NotYetBegin' THEN '還沒回覆'
                                    WHEN W.WORK_STATE = 'Audit' THEN '回覆完成，交付人審查中'
                                    WHEN W.WORK_STATE = 'Proceeding' THEN '有回覆，但沒有完成'
                                    ELSE W.WORK_STATE
                                END AS WORK_STATE_DESC,
                                1 AS SortOrder
                            FROM
                                [UOF].dbo.TB_EIP_SCH_WORK W
                            WHERE
                                1 = 1
                            GROUP BY
                                W.WORK_STATE
                        ) AS ResultTable
                        ORDER BY
                            SortOrder, WORK_STATE_DESC;";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "WORK_STATE_DESC";
            DropDownList2.DataValueField = "WORK_STATE_DESC";
            DropDownList2.DataBind();
        }
        else
        {

        }

    }

    private void BindGrid(string SDATE, string EDATE)
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //string cmdTxt = @"
        //                SELECT 
        //                USER1.[NAME] AS 'NAME1',REPLACE([TB_EIP_SCH_WORK].[SUBJECT],char(10),'<br/>') AS   SUBJECT
        //                ,(SELECT TOP 1 ISNULL([DESCRIPTION],'') FROM [UOF].[dbo].[TB_EIP_SCH_WORK_RECORD] WHERE [TB_EIP_SCH_WORK_RECORD].[WORK_GUID]=[TB_EIP_SCH_WORK].[WORK_GUID] ORDER BY CREATE_TIME DESC) AS 'DESCRIPTION'
        //                ,CONVERT(NVARCHAR,[TB_EIP_SCH_WORK].[END_TIME],111)  AS END_TIME,DATEDIFF(day, [TB_EIP_SCH_WORK].[END_TIME],GETDATE()) AS DIFFDATES,USER2.[NAME]  AS 'NAME2'
        //                ,CASE WHEN [TB_EIP_SCH_WORK].[WORK_STATE]='NotYetBegin' THEN '當未開始' ELSE '進行中' END AS 'STATUS'
        //                ,[TB_EIP_SCH_WORK].[WORK_STATE],[TB_EIP_SCH_WORK].[EXECUTE_USER],[TB_EIP_SCH_WORK].[SOURCE_USER]
        //                ,[TB_EIP_SCH_WORK].WORK_GUID
        //                FROM [UOF].[dbo].[TB_EIP_SCH_WORK]
        //                LEFT JOIN [UOF].[dbo].[TB_EB_USER] USER1 ON USER1.USER_GUID=[TB_EIP_SCH_WORK].[EXECUTE_USER]
        //                LEFT JOIN [UOF].[dbo].[TB_EB_USER] USER2 ON USER2.USER_GUID=[TB_EIP_SCH_WORK].[SOURCE_USER]
        //                WHERE [WORK_STATE] IN ('NotYetBegin','Proceeding','Audit')
        //                AND USER1.[NAME] IN ('洪櫻芬','王琇平','葉枋俐','何姍怡','林琪琪','林杏育','張釋予','蔡顏鴻','陳帟靜','黃鈺涵')
        //                AND CONVERT(NVARCHAR,[TB_EIP_SCH_WORK].[END_TIME],111) >=@SDATE AND CONVERT(NVARCHAR,[TB_EIP_SCH_WORK].[END_TIME],111) <=@EDATE
        //                ORDER BY [EXECUTE_USER],[TB_EIP_SCH_WORK].[END_TIME],[SUBJECT]

        //                ";

        ////string cmdTxt = @"
        ////                SELECT TOP 1 
        ////                'NAME1'AS 'NAME1','SUBJECT' AS SUBJECT
        ////                ,'DESCRIPTION' AS 'DESCRIPTION'
        ////                ,''  AS END_TIME
        ////                ,''  AS DIFFDATES
        ////                ,'NAME2'  AS 'NAME2'
        ////                ,'STATUS' AS 'STATUS'
        ////                ,'1' 'WORK_STATE','' 'EXECUTE_USER','' 'SOURCE_USER'
        ////                ,'' 'WORK_GUID'
        ////                FROM [UOF].[dbo].TB_DMS_AGENCY_SUBSCRIBE

        ////                ";

        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //Grid1.DataSource = dt;
        //Grid1.DataBind();
    }



   
    protected void grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       
    }



  
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       

    }

   

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        
    }

    private void BindGrid2(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();


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

        //目前的狀況
        if (!string.IsNullOrEmpty(DropDownList2.Text))
        {
            if (DropDownList2.Text.Equals("全部"))
            {
                QUERYS2.AppendFormat(@"  ");
            }
            else if(DropDownList2.Text.Equals("還沒回覆"))
            {
                QUERYS2.AppendFormat(@" AND W.WORK_STATE  IN ('NotYetBegin') ");
            }
            else if (DropDownList2.Text.Equals("有回覆，但沒有完成"))
            {
                QUERYS2.AppendFormat(@"  AND W.WORK_STATE IN ('Proceeding') ");
            }
            else if (DropDownList2.Text.Equals("回覆完成，交付人審查中"))
            {
                QUERYS2.AppendFormat(@"  AND W.WORK_STATE IN ('Audit') ");
            }
            else if (DropDownList2.Text.Equals("已回覆"))
            {
                QUERYS2.AppendFormat(@"  AND W.WORK_STATE IN ('Completed') ");
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
                                {1}
                            ORDER BY SUBJECT,CREATE_TIME DESC
                             
                              
                                ", QUERYS.ToString(), QUERYS2.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        m_db.AddParameter("@SUBJECT", TextBox1.Text);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        //匯出專用
        EXCELDT1 = dt;

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {



    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL2();
    }

    public void SETEXCEL2()
    {
        BindGrid2("");
        //BindGrid中已帶入EXCELDT1
        if (EXCELDT1.Rows.Count >= 1)
        {
            //檔案名稱
            var fileName = "明細" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xlsx";
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
                ws.Cells[1, 1].Value = "校稿名稱";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 2].Value = "目前的狀況";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 3].Value = "交付者";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 4].Value = "執行者";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 5].Value = "開始時間";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 6].Value = "預計完成時間";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 7].Value = "最新回覆";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
              
                foreach (DataRow od in EXCELDT1.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["SUBJECT"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 2].Value = od["WORK_STATE"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 3].Value = od["交付者"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 4].Value = od["執行者"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 5].Value = od["CREATE_TIME"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 6].Value = od["END_TIME"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 7].Value = od["DESCRIPTION"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                  

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
        }
    }
    #endregion

    #region BUTTON
    protected void btn_Click(object sender, EventArgs e)
    {

    }
    protected void btn1_Click(object sender, EventArgs e)
    {
        //BindGrid(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
    }

    protected void btn5_Click(object sender, EventArgs e)
    {
        BindGrid2("");


        //if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        //{
        //    if (Dialog.GetReturnValue().Equals("NeedPostBack"))
        //    {

        //    }

        //}
    }

    #endregion
}