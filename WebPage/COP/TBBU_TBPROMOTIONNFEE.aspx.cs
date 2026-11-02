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

public partial class CDS_WebPage_COP_TBBU_TBPROMOTIONNFEE : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBox1.Text = DateTime.Now.Year.ToString();
            TextBox2.Text = DateTime.Now.Year.ToString();
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";

            BindDropDownList();
        }
        else
        {

           
           
        }

       


    }
    #region FUNCTION
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("VALUE", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT [ID],[KINDS],[NAMES],[VALUE] FROM [TKBUSINESS].[dbo].[TBPARA] WHERE [KINDS]='是否結案' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "VALUE";
            DropDownList1.DataValueField = "VALUE";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        StringBuilder QUERYS4 = new StringBuilder();
        StringBuilder QUERYS5 = new StringBuilder();
        StringBuilder QUERYS6 = new StringBuilder();
        StringBuilder QUERYS7 = new StringBuilder();

        //年度
        if (!string.IsNullOrEmpty(TextBox1.Text)&& !string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS1.AppendFormat(@" AND [YEARS]>='{0}'  AND [YEARS]<='{1}'  ", TextBox1.Text, TextBox2.Text);
        }

        //活動/目的
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            QUERYS2.AppendFormat(@" AND [NAMES] LIKE '%{0}%'  ", TextBox3.Text);
        }
        //對象(經銷商)
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            QUERYS3.AppendFormat(@" AND [CLIENTS] LIKE '%{0}%'  ", TextBox4.Text);
        }
        //通路
        if (!string.IsNullOrEmpty(TextBox5.Text))
        {
            QUERYS4.AppendFormat(@" AND [STORES] LIKE '%{0}%'  ", TextBox5.Text);
        }
        //活動內容
        if (!string.IsNullOrEmpty(TextBox6.Text))
        {
            QUERYS5.AppendFormat(@" AND [ACTIONS] LIKE '%{0}%'  ", TextBox6.Text);
        }
        //商品
        if (!string.IsNullOrEmpty(TextBox7.Text))
        {
            QUERYS6.AppendFormat(@" AND [PRODUCTS] LIKE '%{0}%'  ", TextBox7.Text);
        }
        //是否結案
        if (!string.IsNullOrEmpty(DropDownList1.SelectedValue.ToString())&& DropDownList1.SelectedValue.ToString().Equals("N"))
        {
            QUERYS7.AppendFormat(@" AND [ISCLOSED] LIKE '%{0}%'  ", DropDownList1.SelectedValue.ToString());
        }
        else if (!string.IsNullOrEmpty(DropDownList1.SelectedValue.ToString()) && DropDownList1.SelectedValue.ToString().Equals("Y"))
        {
            QUERYS7.AppendFormat(@" AND [ISCLOSED] LIKE '%{0}%'  ", DropDownList1.SelectedValue.ToString());
        }
        else if (!string.IsNullOrEmpty(DropDownList1.SelectedValue.ToString()) && DropDownList1.SelectedValue.ToString().Equals("全部"))
        {
            QUERYS7.AppendFormat(@" ");
        }
       

        cmdTxt.AppendFormat(@" 
                           SELECT 
                            [ID]
                            ,[YEARS]
                            ,[DEPNAME]
                            ,[TITLES]
                            ,[SALES]
                            ,[NAMES]
                            ,[KINDS]
                            ,[PROMOTIONS]
                            ,[PROMOTIONSSETS]
                            ,[SDATES]
                            ,[CLIENTS]
                            ,[STORES]
                            ,[SALESNUMS]
                            ,ROUND([SALESMONEYS],0) AS SALESMONEYS
                            ,ROUND([COSTMONEYS],0) AS COSTMONEYS
                            ,ROUND([FEEMONEYS],0) AS FEEMONEYS
                            ,ROUND([PROFITS],0) AS PROFITS
                            ,[COMMENTS]
                            ,ROUND([ACTSALESMONEYS],0) AS ACTSALESMONEYS
                            ,ROUND([ACTCOSTMONEYS],0) AS ACTCOSTMONEYS
                            ,ROUND([ACTFEEMONEYS],0) AS ACTFEEMONEYS
                            ,ROUND([ACTPROFITS],0) AS ACTPROFITS
                            ,ACTIONS
                            ,PRODUCTS
                            ,[DOC_NBR]
                            ,ISCLOSED
                            ,ISNULL( (     
                            SELECT CASE
                            WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                            ELSE '<br />'
                            END +ISNULL([FEENAME],'')+':'+ISNULL(CONVERT(NVARCHAR,(CONVERT(INT,[FEEMONEYS]))),'')+'元' AS 'data()'
                            FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEDETAILS] WHERE [TBPROMOTIONNFEEDETAILS].[MID]=[TBPROMOTIONNFEE].[ID]
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '各項費用預估' 
                            ,ISNULL( (     
                            SELECT CASE
                            WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                            ELSE '<br />'
                            END +ISNULL([MB002],'')+'<br>'+'預估總業績 '+ISNULL(CONVERT(NVARCHAR,(CONVERT(INT,[MONEYS]))),'')+'元'+'<br>'+'預估總費用 '+ISNULL(CONVERT(NVARCHAR,(CONVERT(INT,[FEES]))),'')+'元'+'<br>'+'總成本 '+ISNULL(CONVERT(NVARCHAR,(CONVERT(INT,[COSTS]))),'')+'元' AS 'data()'
                            FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEPRODUCTS] WHERE [TBPROMOTIONNFEEPRODUCTS].[MID]=[TBPROMOTIONNFEE].[ID]
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '各項商品' 

                            FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEE]
                            WHERE 1=1                           
                            {0}
                            {1}
                            {2}
                            {3}
                            {4}
                            {5}
                            {6}
                          
                              
                            ", QUERYS1.ToString(), QUERYS2.ToString(), QUERYS3.ToString(), QUERYS4.ToString(), QUERYS5.ToString(), QUERYS6.ToString(), QUERYS7.ToString());




        //m_db.AddParameter("@YEARS", TextBox1.Text.ToString().Trim());
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid("");
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Button1
            //Get the button that raised the event
            Button btn = (Button)e.Row.FindControl("Button1");

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue = btn.CommandArgument;

            DataRowView row = (DataRowView)e.Row.DataItem;
            Button lbtnName = (Button)e.Row.FindControl("Button1");

            ExpandoObject param = new { ID = Cellvalue }.ToExpando();

            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBPROMOTIONNFEEDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
           

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        { 
            //Button2
            //Get the button that raised the event
            Button btn2 = (Button)e.Row.FindControl("Button2");

            //Get the row that contains this button
            GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;

            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue2 = btn2.CommandArgument;

            DataRowView row2 = (DataRowView)e.Row.DataItem;
            Button lbtnName2 = (Button)e.Row.FindControl("Button2");

            ExpandoObject param2 = new { ID = Cellvalue2 }.ToExpando();

            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName2, "~/CDS/WebPage/COP/TBBU_TBPROMOTIONNFEEDETAILSDialog.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param2);
        }



        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    //Button2
        //    //Get the button that raised the event
        //    Button btn3 = (Button)e.Row.FindControl("Button3");

        //    //Get the row that contains this button
        //    GridViewRow gvr3 = (GridViewRow)btn3.NamingContainer;

        //    //string cellvalue = gvr.Cells[2].Text.Trim();
        //    string Cellvalue3 = btn3.CommandArgument;

        //    DataRowView row3 = (DataRowView)e.Row.DataItem;
        //    Button lbtnName3 = (Button)e.Row.FindControl("Button3");

        //    ExpandoObject param3 = new { ID = Cellvalue3 }.ToExpando();

        //    //Grid開窗是用RowDataBound事件再開窗
        //    Dialog.Open2(lbtnName3, "~/CDS/WebPage/COP/TBBU_TBPROMOTIONNFEEPRODUCTSDialog.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param3);
        //}

    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ADDPROJECTS")
        {
            BindGrid();
        }
        if (e.CommandName == "ADDFEES")
        {
            BindGrid();
        }
        if (e.CommandName == "ADDPRODUCTS")
        {
            BindGrid();
        }
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);


        //string cmdTxt = @" 
        //               SELECT [PRODUCTS].[MB001],[PRODUCTSFEATURES],[SALESFOCUS],[COPYWRITINGS],[PICPATHS]
        //                ,MB002,MB003,MB004,MA003,ISNULL(MD007,0) AS MD007,CONVERT(NVARCHAR,MB023)+(CASE WHEN MB198='1' THEN '天' ELSE (CASE WHEN MB198='2' THEN '月' ELSE '年' END ) END ) AS 'VALIDITYPERIOD',CONVERT(decimal(16,3),ISNULL(MB047,0)) AS MB047,MB013
        //                ,[ALBUM_GUID], [PHOTO_GUID],[PHOTO_DESC],[FILE_ID],[RESIZE_FILE_ID],[THUMBNAIL_FILE_ID]
        //                FROM [TKBUSINESS].[dbo].[PRODUCTS]
        //                LEFT JOIN [TK].dbo.[INVMB] ON [PRODUCTS].[MB001]=[INVMB].[MB001]
        //                LEFT JOIN [TK].dbo.INVMA ON MA001='9' AND MA002=MB115
        //                LEFT JOIN [TK].dbo.BOMMD ON MD001=[INVMB].[MB001] AND MD003 LIKE '201%'
        //                LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EIP_ALBUM_PHOTO] ON [PHOTO_TOPIC]=[PRODUCTS].[MB001] COLLATE Chinese_Taiwan_Stroke_BIN
        //                ORDER BY [PRODUCTS].[MB001]
        //                ";



        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    dt.Columns[0].Caption = "ID";


        //    e.Datasource = dt;
        //}
    }

   



    public override void VerifyRenderingInServerForm(Control control) 
    { 

    }

    public void SETEXCEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        StringBuilder QUERYS4 = new StringBuilder();
        StringBuilder QUERYS5 = new StringBuilder();
        StringBuilder QUERYS6 = new StringBuilder();
        StringBuilder QUERYS7 = new StringBuilder();

        //年度
        if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS1.AppendFormat(@" AND [YEARS]>='{0}'  AND [YEARS]<='{1}'  ", TextBox1.Text, TextBox2.Text);
        }

        //活動/目的
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            QUERYS2.AppendFormat(@" AND [NAMES] LIKE '%{0}%'  ", TextBox3.Text);
        }
        //對象(經銷商)
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            QUERYS3.AppendFormat(@" AND [CLIENTS] LIKE '%{0}%'  ", TextBox4.Text);
        }
        //通路
        if (!string.IsNullOrEmpty(TextBox5.Text))
        {
            QUERYS4.AppendFormat(@" AND [STORES] LIKE '%{0}%'  ", TextBox5.Text);
        }
        //活動內容
        if (!string.IsNullOrEmpty(TextBox6.Text))
        {
            QUERYS5.AppendFormat(@" AND [ACTIONS] LIKE '%{0}%'  ", TextBox6.Text);
        }
        //商品
        if (!string.IsNullOrEmpty(TextBox7.Text))
        {
            QUERYS6.AppendFormat(@" AND [PRODUCTS] LIKE '%{0}%'  ", TextBox7.Text);
        }
        //是否結案
        if (!string.IsNullOrEmpty(DropDownList1.SelectedValue.ToString()) && DropDownList1.SelectedValue.ToString().Equals("N"))
        {
            QUERYS7.AppendFormat(@" AND [ISCLOSED] LIKE '%{0}%'  ", DropDownList1.SelectedValue.ToString());
        }
        else if (!string.IsNullOrEmpty(DropDownList1.SelectedValue.ToString()) && DropDownList1.SelectedValue.ToString().Equals("Y"))
        {
            QUERYS7.AppendFormat(@" AND [ISCLOSED] LIKE '%{0}%'  ", DropDownList1.SelectedValue.ToString());
        }
        else if (!string.IsNullOrEmpty(DropDownList1.SelectedValue.ToString()) && DropDownList1.SelectedValue.ToString().Equals("全部"))
        {
            QUERYS7.AppendFormat(@" ");
        }


        cmdTxt.AppendFormat(@" 
                           SELECT 
                            [ID] AS '編號'
                            ,[YEARS] AS '年度'
                            ,[DEPNAME] AS '申請部門'
                            ,[TITLES] AS '職務'
                            ,[SALES] AS '申請人'
                            ,[NAMES] AS '活動/目的'
                            ,[SDATES] AS '日期:起~迄'
                            ,[CLIENTS] AS '對象(經銷商)'
                            ,[STORES] AS '通路'
                            ,ACTIONS AS '活動內容(詳述)'
                            ,PRODUCTS AS '商品'
                            ,[DOC_NBR] AS '會辨單'
                            ,ROUND([SALESMONEYS],0) AS '實際-總收入'
                            ,ROUND([COSTMONEYS],0) AS '實際-總成本'
                            ,ROUND([FEEMONEYS],0) AS '實際-總費用'
                            ,ROUND([PROFITS],0) AS '實際-利潤'
                            ,ROUND([ACTSALESMONEYS],0) AS '預估-總收入'
                            ,ROUND([ACTCOSTMONEYS],0) AS '預估-總成本'
                            ,ROUND([ACTFEEMONEYS],0) AS '預估-總費用'
                            ,ROUND([ACTPROFITS],0) AS '預估-利潤' 
                            ,ISCLOSED AS '是否結案'
                            ,ISNULL( (     
                            SELECT CASE
                            WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                            ELSE '<br />'
                            END +ISNULL([FEENAME],'')+':'+ISNULL(CONVERT(NVARCHAR,(CONVERT(INT,[FEEMONEYS]))),'')+'元' AS 'data()'
                            FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEEDETAILS] WHERE [TBPROMOTIONNFEEDETAILS].[MID]=[TBPROMOTIONNFEE].[ID]
                            FOR XML PATH(''), TYPE  
                            ).value('.','nvarchar(max)'),'')  As '各項費用預估'                            
                            FROM [TKBUSINESS].[dbo].[TBPROMOTIONNFEE]
                            WHERE 1=1                 
                            {0}
                            {1}
                            {2}
                            {3}
                            {4}
                            {5}
                            {6}
                          
                              
                            ", QUERYS1.ToString(), QUERYS2.ToString(), QUERYS3.ToString(), QUERYS4.ToString(), QUERYS5.ToString(), QUERYS6.ToString(), QUERYS7.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if(dt.Rows.Count>0)
        {
            //檔案名稱
            var fileName = "匯出" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {              

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());


                //預設行高
                ws.DefaultRowHeight = 60;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "編號";
                ws.Cells[1, 2].Value = "年度";
                ws.Cells[1, 3].Value = "申請部門";
                ws.Cells[1, 4].Value = "職務";
                ws.Cells[1, 5].Value = "申請人";
                ws.Cells[1, 6].Value = "活動/目的";
                ws.Cells[1, 7].Value = "日期:起~迄";
                ws.Cells[1, 8].Value = "對象(經銷商)";
                ws.Cells[1, 9].Value = "通路";
                ws.Cells[1, 10].Value = "活動內容(詳述)";
                ws.Cells[1, 11].Value = "商品";
                ws.Cells[1, 12].Value = "會辨單";
                ws.Cells[1, 13].Value = "實際-總收入";
                ws.Cells[1, 14].Value = "實際-總成本";
                ws.Cells[1, 15].Value = "實際-總費用";
                ws.Cells[1, 16].Value = "實際-利潤";
                ws.Cells[1, 17].Value = "預估-總收入";
                ws.Cells[1, 18].Value = "預估-總成本";
                ws.Cells[1, 19].Value = "預估-總費用";
                ws.Cells[1, 20].Value = "預估-利潤";
                ws.Cells[1, 21].Value = "是否結案";

                //標題-儲存格框
                for (int i=1; i<=21; i++)
                {
                    ws.Cells[1, i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                    ws.Cells[1, i].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[1, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                }

                //明細-儲存格框
                ROWS = 2;
                foreach (DataRow od in dt.Rows)
                {
                    
                    for (int i = 1; i <= 21; i++)
                    {
                        ws.Cells[ROWS, i].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                        ws.Cells[ROWS, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    }

                    ROWS = ROWS + 1;
                }

                //明細-儲存格
                ROWS = 2;
                foreach (DataRow od in dt.Rows)
                {
                    
                    ws.Cells[ROWS, 1].Value = od["編號"].ToString();
                    ws.Cells[ROWS, 2].Value = od["年度"].ToString();
                    ws.Cells[ROWS, 3].Value = od["申請部門"].ToString();
                    ws.Cells[ROWS, 4].Value = od["職務"].ToString();
                    ws.Cells[ROWS, 5].Value = od["申請人"].ToString();
                    ws.Cells[ROWS, 6].Value = od["活動/目的"].ToString();
                    ws.Cells[ROWS, 7].Value = od["日期:起~迄"].ToString();
                    ws.Cells[ROWS, 8].Value = od["對象(經銷商)"].ToString();
                    ws.Cells[ROWS, 9].Value = od["通路"].ToString();
                    ws.Cells[ROWS, 10].Value = od["活動內容(詳述)"].ToString();
                    ws.Cells[ROWS, 11].Value = od["商品"].ToString();
                    ws.Cells[ROWS, 12].Value = od["會辨單"].ToString();
                    ws.Cells[ROWS, 13].Value = od["實際-總收入"].ToString();
                    ws.Cells[ROWS, 14].Value = od["實際-總成本"].ToString();
                    ws.Cells[ROWS, 15].Value = od["實際-總費用"].ToString();
                    ws.Cells[ROWS, 16].Value = od["實際-利潤"].ToString();
                    ws.Cells[ROWS, 17].Value = od["預估-總收入"].ToString();
                    ws.Cells[ROWS, 18].Value = od["預估-總成本"].ToString();
                    ws.Cells[ROWS, 19].Value = od["預估-總費用"].ToString();
                    ws.Cells[ROWS, 20].Value = od["預估-利潤"].ToString();
                    ws.Cells[ROWS, 21].Value = od["是否結案"].ToString();

                    ROWS = ROWS + 1;
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

    #endregion

    #region BUTTON
    protected void btn_Click(object sender, EventArgs e)
    {


        //開窗後回傳參數
        if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            //txtReturnValue.Text = Dialog.GetReturnValue();
        }


    }


    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid();
        
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        SETEXCEL();
    }
    protected void btn3_Click(object sender, EventArgs e)
    {
        //Response.ClearContent();
        //Response.AddHeader("content-disposition", "attachment; filename=test.xls");
        //Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
        //HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=big5>");
        //HttpContext.Current.Response.Write("<head><meta http-equiv=Content-Type content=text/html;charset=big5></head>");
        //Response.Charset = "big5";
        //Response.ContentType = "application/excel";


        //System.IO.StringWriter sw = new System.IO.StringWriter();
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //Grid1.RenderControl(htw);
        //Response.Write(sw.ToString());
        //Response.End();
    }
    protected void MyButtonClick(object sender, System.EventArgs e)
    {
      

    }
    protected void btn4_Click(object sender, EventArgs e)
    {
        //開窗後回傳參數
        if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            //Label3.Text = Dialog.GetReturnValue();
            if (Dialog.GetReturnValue().Equals("ADD"))
            {
                BindGrid();
            }
            
        }
    }
  
    #endregion
}