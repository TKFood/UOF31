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
using System.Threading.Tasks;

public partial class CDS_WebPage_COWORK_TB_PROJECTS_PRODUCTS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    DataTable EXCELDT1 = new DataTable();
    DataTable EXCELDT4 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        DataTable DT_ROLES = FIND_TB_PROJECTS_ROLES(NAME);
        if(DT_ROLES!=null && DT_ROLES.Rows.Count>=1)
        {
            ROLES = DT_ROLES.Rows[0]["ROLES"].ToString();
        }
        


        if (!IsPostBack)
        {
            Bind_DropDownList_ISCLOSED();
            Bind_DropDownList_OWNER();
            Bind_DropDownList_DESIGNER();


            //先算統計
            BindGrid4(DropDownList_ISCLOSED.SelectedValue.ToString());
        }
    }


    #region FUNCTION
    public void Bind_DropDownList_ISCLOSED()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT
                        [ID]
                        ,[KIND]
                        ,[PARAID]
                        ,[PARANAME]
                        FROM [TKRESEARCH].[dbo].[TBPARA]
                        WHERE [KIND]='TB_PROJECTS_PRODUCTS_ISCLOSED'
                        ORDER BY [PARAID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList_ISCLOSED.DataSource = dt;
            DropDownList_ISCLOSED.DataTextField = "PARANAME";
            DropDownList_ISCLOSED.DataValueField = "PARANAME";
            DropDownList_ISCLOSED.DataBind();

        }
        else
        {

        }

    }

   

    public void Bind_DropDownList_OWNER()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"                         
                           SELECT
                                TEMP.OWNER,
                                TEMP.RowNum
                            FROM
                                (
                                    SELECT
                                        '全部' AS OWNER,
                                        -- 為 '全部' 賦予序號 0 或 1 (取決於您希望它排在最前面還是最後面)
                                        -- 這裡使用 0 以確保它排在第一個
                                        0 AS RowNum
                                    UNION ALL
                                    SELECT
                                        [OWNER],
                                        -- 為實際的 OWNER 列表生成序號
                                        -- 透過 ROW_NUMBER() 函數，根據 OWNER 欄位排序來賦予序號
                                        -- 這裡假設您希望它們按字母順序排列，並從 1 開始編號
                                        ROW_NUMBER() OVER (ORDER BY [OWNER] ASC) AS RowNum
                                    FROM
                                        [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                                    GROUP BY
                                        [OWNER]
                                ) AS TEMP
                            -- 最終排序是依據新生成的 RowNum 欄位
                            ORDER BY
                                TEMP.RowNum ASC;
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList_OWNER.DataSource = dt;
            DropDownList_OWNER.DataTextField = "OWNER";
            DropDownList_OWNER.DataValueField = "OWNER";
            DropDownList_OWNER.DataBind();

        }
        else
        {

        }
    }

    public void Bind_DropDownList_DESIGNER()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"                         
                         SELECT
                            TEMP.DESIGNER,
                            TEMP.RowNum
                        FROM
                            (
                                -- 1. 處理 '全部' 選項
                                SELECT
                                    '全部' AS DESIGNER,
                                    -- 為 '全部' 賦予序號 0，確保它排在第一個
                                    0 AS RowNum
                                UNION ALL
                                -- 2. 處理實際的 DESIGNER 列表
                                SELECT
                                    T.[DESIGNER],
                                    -- 為有效的 DESIGNER 列表生成序號
                                    -- 使用 ROW_NUMBER() 函數，根據 DESIGNER 欄位排序來賦予序號
                                    -- 這裡假設您希望 DESIGNER 按字母順序排列，並從 1 開始編號
                                    ROW_NUMBER() OVER (ORDER BY T.[DESIGNER] ASC) AS RowNum
                                FROM
                                    [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS] AS T
                                WHERE
                                    -- 排除 NULL 或空字串的 DESIGNER
                                    ISNULL(T.[DESIGNER] ,'') <> ''
                                GROUP BY
                                    T.[DESIGNER]
                            ) AS TEMP
                        -- 最終排序是依據新生成的 RowNum 欄位
                        ORDER BY
                            TEMP.RowNum ASC;
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList_DESIGNER.DataSource = dt;
            DropDownList_DESIGNER.DataTextField = "DESIGNER";
            DropDownList_DESIGNER.DataValueField = "DESIGNER";
            DropDownList_DESIGNER.DataBind();

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
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        StringBuilder QUERYS4 = new StringBuilder();

        //DropDownList_ISCLOSED
        if (DropDownList_ISCLOSED.Text.Equals("全部"))
        {
            QUERYS.AppendFormat(@"");
        }
        else if (DropDownList_ISCLOSED.Text.Equals("進行中"))
        {
            QUERYS.AppendFormat(@" AND [ISCLOSED]='N' ");
        }
        else if (DropDownList_ISCLOSED.Text.Equals("已完成"))
        {
            QUERYS.AppendFormat(@" AND [ISCLOSED]='Y' ");
        }
        //DropDownList_OWNER
        if (DropDownList_OWNER.Text.Equals("全部"))
        {
            QUERYS2.AppendFormat(@"");
        }
        else
        {
            QUERYS2.AppendFormat(@" AND OWNER=N'{0}' ", DropDownList_OWNER.Text);
        }
        //TextBox1
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS3.AppendFormat(@" AND PROJECTNAMES LIKE '%{0}%' ", TextBox1.Text);
        }
        else
        {
            QUERYS3.AppendFormat(@"");
        }
        //DropDownList_DESIGNER
        if( DropDownList_DESIGNER.Text.Equals("全部"))
        {
            QUERYS4.AppendFormat(@"");
        }
        else
        {
            QUERYS4.AppendFormat(@" AND DESIGNER=N'{0}' ", DropDownList_DESIGNER.Text);
        }

        cmdTxt.AppendFormat(@"
                            SELECT 
                            [ID]
                            ,[NO] AS '專案編號'
                            ,[KINDS] AS '分類'
                            ,[PROJECTNAMES] AS '項目名稱'
                            ,[TRYSDATES] AS '產品打樣日'
                            ,[TASTESDATES] AS '產品試吃日'
                            ,[DESIGNSDATES] AS '包裝設計日'
                            ,[SALESDATES] AS '上市日'
                            ,[OWNER] AS '專案負責人'
                            ,[STATUS] AS '研發進度回覆'
                            ,[TASTESREPLYS] AS '業務進度回覆'
                            ,[DESIGNER] AS '設計負責人'
                            ,[DESIGNREPLYS] AS '設計回覆'
                            ,[PURREPLYS] AS '採購回覆'
                            ,[QCREPLYS] AS '品保回覆'
                            ,[STAGES] AS '專案階段'
                            ,[ISCLOSED] AS '是否結案'
                            ,[DOC_NBR] AS '表單編號'
                            ,CONVERT(NVARCHAR,[UPDATEDATES],112) AS '更新日'
                            ,(SELECT TOP 1 TASK_ID FROM [192.168.1.223].[UOF].[dbo].[View_TB_WKF_TASK] WHERE ISNULL([TB_PROJECTS_PRODUCTS].[DOC_NBR],'')<>'' AND  DOC_NBR LIKE 'NEWDESIGN%' AND [View_TB_WKF_TASK].[DOC_NBR]=[TB_PROJECTS_PRODUCTS].[DOC_NBR] COLLATE Chinese_Taiwan_Stroke_BIN) AS 'TASK_ID'

                            FROM [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                            WHERE 1=1
                            {0}
                            {1}
                            {2}
                            {3}
                            ORDER BY [OWNER],[NO]
                             ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString(), QUERYS4.ToString());




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
        if (e.Row.RowType == DataControlRowType.DataRow) // 只處理資料列
        {
            TextBox txtNewField_GV1_研發進度回覆 = (TextBox)e.Row.FindControl("txtNewField_GV1_研發進度回覆");
            if (txtNewField_GV1_研發進度回覆 != null)
            {
                if (string.IsNullOrWhiteSpace(txtNewField_GV1_研發進度回覆.Text))
                {
                    txtNewField_GV1_研發進度回覆.Text =
                       "1.樣本提供:" + "\r\n" +
                       "2.成本提供(初算/正式):" + "\r\n" +
                       "3.特殊原料、製程批量說明:";
                }
            }
        }


        if (e.Row.RowType == DataControlRowType.DataRow) // 只處理資料列
        {
            TextBox txtNewField_GV1_業務進度回覆 = (TextBox)e.Row.FindControl("txtNewField_GV1_業務進度回覆");
            if (txtNewField_GV1_業務進度回覆 != null)
            {
                if (string.IsNullOrWhiteSpace(txtNewField_GV1_業務進度回覆.Text))
                {
                    // 設定你的預設文字
                    txtNewField_GV1_業務進度回覆.Text =
                       "1.試吃確認:" + "\r\n" +
                       "2.報價確認:" + "\r\n" +
                       "3.進度更新:";

                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow) // 只處理資料列
        {
            TextBox txtNewField_GV1_設計回覆 = (TextBox)e.Row.FindControl("txtNewField_GV1_設計回覆");
            if (txtNewField_GV1_設計回覆 != null)
            {
                if (string.IsNullOrWhiteSpace(txtNewField_GV1_設計回覆.Text))
                {
                    // 設定你的預設文字
                    txtNewField_GV1_設計回覆.Text =
                       "1.圖面設計:" + "\r\n" +
                       "2.上校稿:" + "\r\n" +
                       "3.廠商確稿發包:";

                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow) // 只處理資料列
        {
            TextBox txtNewField_GV1_採購回覆 = (TextBox)e.Row.FindControl("txtNewField_GV1_採購回覆");
            if (txtNewField_GV1_採購回覆 != null)
            {
                if (string.IsNullOrWhiteSpace(txtNewField_GV1_採購回覆.Text))
                {
                    // 設定你的預設文字
                    txtNewField_GV1_採購回覆.Text =
                       "1.物料:" + "\r\n" +
                       "2.物料-估價完成日:" + "\r\n" +
                       "3.物料-發包完成日:" + "\r\n" +
                       "4.包材到貨日:" + "\r\n" +
                       "5.原料:" + "\r\n" +
                       "6.原料-估價+發包完成日:" + "\r\n" +
                       "7.原料到貨日:";

                }
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
            Button btn2 = (Button)e.Row.FindControl("Button2");
            if (btn2 != null)
            {
                string cellValue2 = btn2.CommandArgument;
                dynamic param2 = new { ID = cellValue2 }.ToExpando();
            }
            Button btn6 = (Button)e.Row.FindControl("Button6");
            if (btn6 != null)
            {
                string cellValue6 = btn6.CommandArgument;
                dynamic param6 = new { ID = cellValue6 }.ToExpando();
            }
            Button btn7 = (Button)e.Row.FindControl("Button7");
            if (btn7 != null)
            {
                string cellValue7 = btn7.CommandArgument;
                dynamic param7 = new { ID = cellValue7 }.ToExpando();
            }
        }

        //設權限
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SET_ALLOWED_MODIFY_GV1(e.Row);
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
                TextBox txtNewField_GV1_研發進度回覆 = (TextBox)row.FindControl("txtNewField_GV1_研發進度回覆");
                string newTextValue_GV1_研發進度回覆 = txtNewField_GV1_研發進度回覆.Text;
                TextBox txtNewField_GV1_業務進度回覆 = (TextBox)row.FindControl("txtNewField_GV1_業務進度回覆");
                string newTextValue_GV1_業務進度回覆 = txtNewField_GV1_業務進度回覆.Text;
                TextBox txtNewField_GV1_設計回覆 = (TextBox)row.FindControl("txtNewField_GV1_設計回覆");
                string newTextValue_GV1_設計回覆 = txtNewField_GV1_設計回覆.Text;
                TextBox txtNewField_GV1_設計負責人 = (TextBox)row.FindControl("txtNewField_GV1_設計負責人");
                string newTextValue_GV1_設計負責人 = txtNewField_GV1_設計負責人.Text;
                TextBox txtNewField_GV1_品保回覆 = (TextBox)row.FindControl("txtNewField_GV1_品保回覆");
                string newTextValue_GV1_品保回覆 = txtNewField_GV1_品保回覆.Text;
                TextBox txtNewField_GV1_採購回覆 = (TextBox)row.FindControl("txtNewField_GV1_採購回覆");
                string newTextValue_GV1_採購回覆 = txtNewField_GV1_採購回覆.Text;
                HyperLink hlTask = (HyperLink)row.FindControl("hlTask");

                Label Label_ID = (Label)row.FindControl("Label_ID");
                Label Label_NO = (Label)row.FindControl("Label_專案編號");
                Label Label_項目名稱 = (Label)row.FindControl("Label_項目名稱");
                Label Label_分類 = (Label)row.FindControl("Label_分類");               
                Label Label_專案負責人 = (Label)row.FindControl("Label_專案負責人");
                Label Label_表單編號 = (Label)row.FindControl("Label_表單編號");
                Label Label_STAGES = (Label)row.FindControl("Label_專案階段");
                Label Label_是否結案 = (Label)row.FindControl("Label_是否結案");

                string ID = Label_ID.Text;
                string NO = Label_NO.Text;
                string PROJECTNAMES = Label_項目名稱.Text;
                string KINDS = Label_分類.Text;                
                string OWNER = Label_專案負責人.Text;
                string STATUS = newTextValue_GV1_研發進度回覆;
                string DOC_NBR = Label_表單編號.Text;
                string STAGES= Label_STAGES.Text;
                string ISCLOSED = Label_是否結案.Text;
                string TASTESREPLYS = newTextValue_GV1_業務進度回覆;
                string DESIGNREPLYS = newTextValue_GV1_設計回覆;
                string DESIGNER = newTextValue_GV1_設計負責人;
                string QCREPLYS = newTextValue_GV1_品保回覆;
                string PURREPLYS = newTextValue_GV1_採購回覆;

                //新增記錄檔
                ADD_TB_PROJECTS_PRODUCTS_HISTORYS(
                    ID,
                    NO,
                    PROJECTNAMES,
                    KINDS,
                    OWNER,
                    STATUS,
                    TASTESREPLYS,
                    DESIGNER,
                    DESIGNREPLYS,
                    PURREPLYS,
                    QCREPLYS,
                    DOC_NBR,
                    STAGES,
                    ISCLOSED
                );
                //更新研發進度回覆
                UPDATE_TB_PROJECTS_PRODUCTS_STATUS(
                    ID,
                    NO,
                    STATUS,
                    TASTESREPLYS,
                    DESIGNREPLYS,                   
                    DESIGNER,
                    PURREPLYS,
                    QCREPLYS
                    );

                //寄通知mail
                string url = hlTask.NavigateUrl;
                string subject = "系統通知-商品專案-有修改內容" + "， 專案編號: " + NO + " 項目名稱: " + PROJECTNAMES;
                string body = string.Format(
                                          "專案編號: {0}<br>" +
                                          "項目名稱: {1}<br>" +
                                          "目前研發進度回覆: {2}<br>" +
                                          "目前業務進度回覆: {3}<br>" +
                                          "目前設計回覆: {4}<br>" +
                                          "您好，請點選以下連結：<br><a href='{5}'>點我前往表單</a><br>",
                                          NO, PROJECTNAMES, STATUS, TASTESREPLYS, DESIGNREPLYS, url
                                      );

                ////建立收件人
                ////要寄給負責人+研發群               
                ////DataTable DT_MAILS = SET_MAILTO(OWNER);
                ////SendEmail(subject, body, DT_MAILS);

                //Task.Run(() =>
                //{
                //    DataTable DT_MAILS = SET_MAILTO(OWNER);  
                //    if (DT_MAILS != null)
                //    {
                //       SendEmail(subject, body, DT_MAILS);
                //    }
                //});

            }

          

            BindGrid();
            BindGrid2();
        }

        if (e.CommandName == "Button6")
        {
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField_GV1_研發進度回覆 = (TextBox)row.FindControl("txtNewField_GV1_研發進度回覆");
                string newTextValue_GV1_研發進度回覆 = txtNewField_GV1_研發進度回覆.Text;
                TextBox txtNewField_GV1_業務進度回覆 = (TextBox)row.FindControl("txtNewField_GV1_業務進度回覆");
                string newTextValue_GV1_業務進度回覆 = txtNewField_GV1_業務進度回覆.Text;
                TextBox txtNewField_GV1_設計回覆 = (TextBox)row.FindControl("txtNewField_GV1_設計回覆");
                string newTextValue_GV1_設計回覆 = txtNewField_GV1_設計回覆.Text;
                TextBox txtNewField_GV1_採購回覆 = (TextBox)row.FindControl("txtNewField_GV1_採購回覆");
                string newTextValue_GV1_採購回覆 = txtNewField_GV1_採購回覆.Text;
                HyperLink hlTask = (HyperLink)row.FindControl("hlTask");

                Label Label_ID = (Label)row.FindControl("Label_ID");
                Label Label_NO = (Label)row.FindControl("Label_專案編號");
                Label Label_項目名稱 = (Label)row.FindControl("Label_項目名稱");               
                Label Label_專案負責人 = (Label)row.FindControl("Label_專案負責人");
                Label Label_是否結案 = (Label)row.FindControl("Label_是否結案");

                string ID = Label_ID.Text;
                string NO = Label_NO.Text;
                string PROJECTNAMES = Label_項目名稱.Text;                
                string OWNER = Label_專案負責人.Text;
                string STATUS = newTextValue_GV1_研發進度回覆;
                string ISCLOSED = Label_是否結案.Text;
                string TASTESREPLYS = newTextValue_GV1_業務進度回覆;
                string DESIGNREPLYS = newTextValue_GV1_設計回覆;
                string PURREPLYS = newTextValue_GV1_採購回覆;


                //寄通知mail
                string url = hlTask.NavigateUrl;
                string subject = "系統通知-商品專案-試吃完成" + "， 專案編號: " + NO + " 項目名稱: " + PROJECTNAMES;
                string body = string.Format(
                                         "試吃完成 <br> " +
                                         "專案編號: {0}<br>" +
                                         "項目名稱: {1}<br>" +
                                         "目前研發進度回覆: {2}<br>" +
                                         "目前業務進度回覆: {3}<br>" +
                                         "目前設計回覆: {4}<br>" +
                                         "目前採購回覆: {6}<br>" +
                                         "您好，請點選以下連結：<br><a href='{5}'>點我前往表單</a><br>",
                                         NO, PROJECTNAMES, STATUS, TASTESREPLYS, DESIGNREPLYS, url, PURREPLYS
                                     );



                ////建立收件人
                ////要寄給負責人+研發群               
                //DataTable DT_MAILS = SET_MAILTO(OWNER);
                //SendEmail(subject, body, DT_MAILS);
                //MsgBox(" MAIL已寄送", this.Page, this);
            }            
        }
        if (e.CommandName == "Button7")
        {
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField_GV1_研發進度回覆 = (TextBox)row.FindControl("txtNewField_GV1_研發進度回覆");
                string newTextValue_GV1_研發進度回覆 = txtNewField_GV1_研發進度回覆.Text;
                TextBox txtNewField_GV1_業務進度回覆 = (TextBox)row.FindControl("txtNewField_GV1_業務進度回覆");
                string newTextValue_GV1_業務進度回覆 = txtNewField_GV1_業務進度回覆.Text;
                TextBox txtNewField_GV1_設計回覆 = (TextBox)row.FindControl("txtNewField_GV1_設計回覆");
                string newTextValue_GV1_設計回覆 = txtNewField_GV1_設計回覆.Text;
                TextBox txtNewField_GV1_採購回覆 = (TextBox)row.FindControl("txtNewField_GV1_採購回覆");
                string newTextValue_GV1_採購回覆 = txtNewField_GV1_採購回覆.Text;
                HyperLink hlTask = (HyperLink)row.FindControl("hlTask");

                Label Label_ID = (Label)row.FindControl("Label_ID");
                Label Label_NO = (Label)row.FindControl("Label_專案編號");
                Label Label_項目名稱 = (Label)row.FindControl("Label_項目名稱");                
                Label Label_專案負責人 = (Label)row.FindControl("Label_專案負責人");
                Label Label_是否結案 = (Label)row.FindControl("Label_是否結案");

                string ID = Label_ID.Text;
                string NO = Label_NO.Text;
                string PROJECTNAMES = Label_項目名稱.Text;                
                string OWNER = Label_專案負責人.Text;
                string STATUS = newTextValue_GV1_研發進度回覆;
                string ISCLOSED = Label_是否結案.Text;
                string TASTESREPLYS = newTextValue_GV1_業務進度回覆;
                string DESIGNREPLYS = newTextValue_GV1_設計回覆;
                string PURREPLYS = newTextValue_GV1_採購回覆;

                //寄通知mail
                string url = hlTask.NavigateUrl;
                string subject = "系統通知-商品專案-可開始設計" + "， 專案編號: " + NO + " 項目名稱: " + PROJECTNAMES;
                string body = string.Format(
                                       "可開始設計 < br > " +
                                       "專案編號: {0}<br>" +
                                       "項目名稱: {1}<br>" +
                                       "目前研發進度回覆: {2}<br>" +
                                       "目前業務進度回覆: {3}<br>" +
                                       "目前設計回覆: {4}<br>" +
                                        "目前採購回覆: {6}<br>" +
                                       "您好，請點選以下連結：<br><a href='{5}'>點我前往表單</a><br>",
                                       NO, PROJECTNAMES, STATUS, TASTESREPLYS, DESIGNREPLYS, url, PURREPLYS
                                   );
                

                ////建立收件人
                ////要寄給負責人+研發群+設計群               
                //DataTable DT_MAILS = SET_MAILTO_DESIGNER(OWNER);
                //SendEmail(subject, body, DT_MAILS);
                //MsgBox(" MAIL已寄送", this.Page, this);
            }

        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL1();

    }

    private void BindGrid2()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        StringBuilder QUERYS4 = new StringBuilder();

        //DropDownList_ISCLOSED
        if (DropDownList_ISCLOSED.Text.Equals("全部"))
        {
            QUERYS.AppendFormat(@"");
        }
        else if (DropDownList_ISCLOSED.Text.Equals("進行中"))
        {
            QUERYS.AppendFormat(@" AND [ISCLOSED]='N' ");
        }
        else if (DropDownList_ISCLOSED.Text.Equals("已完成"))
        {
            QUERYS.AppendFormat(@" AND [ISCLOSED]='Y' ");
        }
        //DropDownList_OWNER
        if (DropDownList_OWNER.Text.Equals("全部"))
        {
            QUERYS2.AppendFormat(@"");
        }
        else
        {
            QUERYS2.AppendFormat(@" AND OWNER=N'{0}' ", DropDownList_OWNER.Text);
        }
        //TextBox1
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS3.AppendFormat(@" AND PROJECTNAMES LIKE '%{0}%' ", TextBox1.Text);
        }
        else
        {
            QUERYS3.AppendFormat(@"");
        }
        //DropDownList_DESIGNER
        if (DropDownList_DESIGNER.Text.Equals("全部"))
        {
            QUERYS4.AppendFormat(@"");
        }
        else
        {
            QUERYS4.AppendFormat(@" AND DESIGNER=N'{0}' ", DropDownList_DESIGNER.Text);
        }

        cmdTxt.AppendFormat(@"


                            SELECT 
                            [ID]
                            ,[NO] AS '專案編號'
                            ,[KINDS] AS '分類'
                            ,[PROJECTNAMES] AS '項目名稱'
                            ,[TRYSDATES] AS '產品打樣日'
                            ,[TASTESDATES] AS '產品試吃日'
                            ,[DESIGNSDATES] AS '包裝設計日'
                            ,[SALESDATES] AS '上市日'
                            ,[OWNER] AS '專案負責人'
                            ,[STATUS] AS '研發進度回覆'
                            ,[TASTESREPLYS] AS '業務進度回覆'
                            ,[STAGES] AS '專案階段'
                            ,[DESIGNER] AS '設計負責人'
                            ,[DESIGNREPLYS] AS '設計回覆'
                            ,[PURREPLYS] AS '採購回覆'
                            ,[QCREPLYS] AS '品保回覆'
                            ,[ISCLOSED] AS '是否結案'
                            ,[DOC_NBR] AS '表單編號'
                            ,CONVERT(NVARCHAR,[UPDATEDATES],112) AS '更新日'
                            FROM [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                            WHERE 1=1
                            {0}
                            {1}
                            {2}
                            {3}
                            ORDER BY [OWNER],[NO]
                             ", QUERYS.ToString(), QUERYS2.ToString(), QUERYS3.ToString(), QUERYS4.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow) // 只處理資料列
        {
            TextBox txtNewField_GV2_研發進度回覆 = (TextBox)e.Row.FindControl("txtNewField_GV2_研發進度回覆");
            if (txtNewField_GV2_研發進度回覆 != null)
            {
                if (string.IsNullOrWhiteSpace(txtNewField_GV2_研發進度回覆.Text))
                {
                    txtNewField_GV2_研發進度回覆.Text =
                       "1.樣本提供:" + "\r\n" +
                       "2.成本提供(初算/正式):" + "\r\n" +
                       "3.特殊原料、製程批量說明:";
                }
            }
        }


        if (e.Row.RowType == DataControlRowType.DataRow) // 只處理資料列
        {
            TextBox txtNewField_GV2_業務進度回覆 = (TextBox)e.Row.FindControl("txtNewField_GV2_業務進度回覆");
            if (txtNewField_GV2_業務進度回覆 != null)
            {
                if (string.IsNullOrWhiteSpace(txtNewField_GV2_業務進度回覆.Text))
                {
                    // 設定你的預設文字
                    txtNewField_GV2_業務進度回覆.Text =
                       "1.試吃確認:" + "\r\n" +
                       "2.報價確認:" + "\r\n" +
                       "3.進度更新:";

                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow) // 只處理資料列
        {
            TextBox txtNewField_GV2_設計回覆 = (TextBox)e.Row.FindControl("txtNewField_GV2_設計回覆");
            if (txtNewField_GV2_設計回覆 != null)
            {
                if (string.IsNullOrWhiteSpace(txtNewField_GV2_設計回覆.Text))
                {
                    // 設定你的預設文字
                    txtNewField_GV2_設計回覆.Text =
                       "1.圖面設計:" + "\r\n" +
                       "2.上校稿:" + "\r\n" +
                       "3.廠商確稿發包:";

                }
            }
        }
        //設選項
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlNewField_GV2_是否結案");
            if (ddl != null)
            {
                // 取得資料來源，例如從資料表 "CaseStatus" 抓出 "Name"、"Code"
                string connStr = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendFormat(@" 
                                        SELECT
                                        [ID]
                                        ,[KIND]
                                        ,[PARAID]
                                        ,[PARANAME]
                                        FROM[TKRESEARCH].[dbo].[TBPARA]
                                        WHERE[KIND] = 'TB_PROJECTS_PRODUCTS_ISCLOSEDYN'
                                        ORDER BY[PARAID]
                                    ");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddl.DataSource = reader;
                    ddl.DataTextField = "PARANAME";   // 顯示文字
                    ddl.DataValueField = "PARANAME";  // 對應值
                    ddl.DataBind();

                    // 設定選取值
                    string currentValue = DataBinder.Eval(e.Row.DataItem, "是否結案").ToString();
                    if (ddl.Items.FindByValue(currentValue) != null)
                        ddl.SelectedValue = currentValue;

                    reader.Close();
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlNewField_GV2_專案負責人");
            if (ddl != null)
            {
                // 取得資料來源，例如從資料表 "CaseStatus" 抓出 "Name"、"Code"
                string connStr = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendFormat(@" 
                                       SELECT
	                                   [OWNER]      
	                                   FROM [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
	                                   GROUP BY [OWNER]
                                       ORDER BY OWNER
                                    ");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddl.DataSource = reader;
                    ddl.DataTextField = "OWNER";   // 顯示文字
                    ddl.DataValueField = "OWNER";  // 對應值
                    ddl.DataBind();

                    // 設定選取值
                    string currentValue = DataBinder.Eval(e.Row.DataItem, "專案負責人").ToString();
                    if (ddl.Items.FindByValue(currentValue) != null)
                        ddl.SelectedValue = currentValue;

                    reader.Close();
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlNewField_GV2_分類");
            if (ddl != null)
            {
                // 取得資料來源，例如從資料表 "CaseStatus" 抓出 "Name"、"Code"
                string connStr = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendFormat(@" 
                                      SELECT 
                                        [KINDS]
                                        FROM [TKRESEARCH].[dbo].[TB_PROJECTS_KINDS]
                                        ORDER BY [ID]
                                    ");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddl.DataSource = reader;
                    ddl.DataTextField = "KINDS";   // 顯示文字
                    ddl.DataValueField = "KINDS";  // 對應值
                    ddl.DataBind();

                    // 設定選取值
                    string currentValue = DataBinder.Eval(e.Row.DataItem, "分類").ToString();
                    if (ddl.Items.FindByValue(currentValue) != null)
                        ddl.SelectedValue = currentValue;

                    reader.Close();
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlNewField_GV2_專案階段");
            if (ddl != null)
            {
                // 取得資料來源，例如從資料表 "CaseStatus" 抓出 "Name"、"Code"
                string connStr = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendFormat(@" 
                                     SELECT 
                                        [ID]
                                        ,[STAGES]
                                        FROM [TKRESEARCH].[dbo].[TB_PROJECTS_STAGES]
                                        ORDER BY [ID]
                                    ");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddl.DataSource = reader;
                    ddl.DataTextField = "STAGES";   // 顯示文字
                    ddl.DataValueField = "STAGES";  // 對應值
                    ddl.DataBind();

                    // 設定選取值
                    string currentValue = DataBinder.Eval(e.Row.DataItem, "專案階段").ToString();
                    if (ddl.Items.FindByValue(currentValue) != null)
                        ddl.SelectedValue = currentValue;

                    reader.Close();
                }
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

        //設權限
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SET_ALLOWED_MODIFY_GV2(e.Row);
        }
    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Button3")
        {
            //MsgBox(e.CommandArgument.ToString() + "OK", this.Page, this);   

            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid2.Rows[rowIndex];


                Label Label_ID = (Label)row.FindControl("Label_ID");

                TextBox txtNewField_專案編號 = (TextBox)row.FindControl("txtNewField_GV2_專案編號");
                TextBox txtNewField_項目名稱 = (TextBox)row.FindControl("txtNewField_GV2_項目名稱");               
                TextBox txtNewField_研發進度回覆 = (TextBox)row.FindControl("txtNewField_GV2_研發進度回覆");
                TextBox txtNewField_業務進度回覆 = (TextBox)row.FindControl("txtNewField_GV2_業務進度回覆");
                TextBox txtNewField_設計回覆 = (TextBox)row.FindControl("txtNewField_GV2_設計回覆");
                TextBox txtNewField_設計負責人 = (TextBox)row.FindControl("txtNewField_GV2_設計負責人");
                TextBox txtNewField_表單編號 = (TextBox)row.FindControl("txtNewField_GV2_表單編號");
                DropDownList ddlNewField_專案負責人 = (DropDownList)row.FindControl("ddlNewField_GV2_專案負責人");
                DropDownList ddlNewField_是否結案 = (DropDownList)row.FindControl("ddlNewField_GV2_是否結案");
                DropDownList ddlNewField_分類 = (DropDownList)row.FindControl("ddlNewField_GV2_分類");
                DropDownList ddlNewField_專案階段 = (DropDownList)row.FindControl("ddlNewField_GV2_專案階段");
                TextBox txtNewField_GV2_品保回覆 = (TextBox)row.FindControl("txtNewField_GV2_品保回覆");
                string newTextValue_GV2_品保回覆 = txtNewField_GV2_品保回覆.Text;
                TextBox txtNewField_GV2_採購回覆 = (TextBox)row.FindControl("txtNewField_GV2_採購回覆");
                string newTextValue_GV2_採購回覆 = txtNewField_GV2_採購回覆.Text;


                string ID = Label_ID.Text;
                string NO = txtNewField_專案編號.Text;
                string PROJECTNAMES = txtNewField_項目名稱.Text;
                string KINDS= ddlNewField_分類.SelectedItem.Text;              
                string OWNER = ddlNewField_專案負責人.SelectedItem.Text;
                string STATUS = txtNewField_研發進度回覆.Text;
                string TASTESREPLYS = txtNewField_業務進度回覆.Text;
                string DESIGNER = txtNewField_設計負責人.Text;
                string DESIGNREPLYS = txtNewField_設計回覆.Text;
                string DOC_NBR = txtNewField_表單編號.Text;
                string STAGES = ddlNewField_專案階段.SelectedItem.Text;
                string ISCLOSED = ddlNewField_是否結案.SelectedItem.Text;
                string QCREPLYS = newTextValue_GV2_品保回覆;
                string PURREPLYS = newTextValue_GV2_採購回覆;

                if (ISCLOSED == "Y" && STAGES != "作廢" && STAGES != "結案" && STAGES != "上市")
                {
                    MsgBox("專案要結案，但是「專案階段」不是「作廢」、「結案」、「上市」，請修改「專案階段」", this.Page, this);
                }

                //新增記錄檔
                ADD_TB_PROJECTS_PRODUCTS_HISTORYS(
                    ID,
                    NO,
                    PROJECTNAMES,
                    KINDS,                    
                    OWNER,
                    STATUS,
                    TASTESREPLYS,
                    DESIGNER,
                    DESIGNREPLYS,
                    PURREPLYS,
                    QCREPLYS,
                    DOC_NBR,
                    STAGES,
                    ISCLOSED
                );

                //更新TB_PROJECTS_PRODUCTS
                UPDATE_TB_PROJECTS_PRODUCTS(
                    ID,
                    NO,
                    PROJECTNAMES,
                    KINDS,                  
                    OWNER,
                    STATUS,
                    TASTESREPLYS,
                    DESIGNER,
                    DESIGNREPLYS,
                    PURREPLYS,
                    QCREPLYS,
                    DOC_NBR,
                    STAGES,
                    ISCLOSED
                    );

                //寄通知mail
                string subject = "系統通知-商品專案-有修改內容" + "， 專案編號: " + NO + " 項目名稱: " + PROJECTNAMES;
                string body = string.Format(                         
                            "專案編號: {0}<br>" +
                            "項目名稱: {1}<br>" +
                            "目前研發進度回覆: {2}<br>" +
                            "目前業務進度回覆: {3}<br>" +
                            "目前設計回覆: {4}<br>" ,                          
                            NO, PROJECTNAMES, STATUS, TASTESREPLYS, DESIGNREPLYS
                        );

                ////建立收件人
                ////要寄給負責人+研發群               
                //DataTable DT_MAILS = SET_MAILTO(OWNER);
                //SendEmail(subject, body, DT_MAILS);
            }

           

            BindGrid();
            BindGrid2();
        }

    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }

    private void BindGrid3(string PROJECTNAMES)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
      
       
        //TextBox1
        if (!string.IsNullOrEmpty(PROJECTNAMES))
        {
            QUERYS.AppendFormat(@" AND [TB_PROJECTS_PRODUCTS].PROJECTNAMES LIKE '%{0}%' ", TextBox2.Text);
        }
      

        cmdTxt.AppendFormat(@"
                            SELECT 
                            [TB_PROJECTS_PRODUCTS_HISTORYS].[ID]
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[NO] AS '專案編號'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[KINDS] AS '分類'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[PROJECTNAMES] AS '項目名稱'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[TRYSDATES] AS '產品打樣日'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[TASTESDATES] AS '產品試吃日'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[DESIGNSDATES] AS '包裝設計日'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[SALESDATES] AS '上市日'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[OWNER] AS '專案負責人'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[STATUS] AS '研發進度回覆'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[TASTESREPLYS] AS '業務進度回覆'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[STAGES] AS '專案階段'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[DESIGNER] AS '設計負責人'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[DESIGNREPLYS] AS '設計回覆'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[PURREPLYS] AS '採購回覆'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[QCREPLYS] AS '品保回覆'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[ISCLOSED] AS '是否結案'
                            ,[TB_PROJECTS_PRODUCTS_HISTORYS].[DOC_NBR] AS '表單編號'
                            ,CONVERT(NVARCHAR,[TB_PROJECTS_PRODUCTS_HISTORYS].[CREATEDATES],112) AS '更新日'
                            FROM [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS], [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS_HISTORYS]
                            WHERE 1=1
                            AND [TB_PROJECTS_PRODUCTS].ID=[TB_PROJECTS_PRODUCTS_HISTORYS].SID
                            {0}
                            ORDER BY [TB_PROJECTS_PRODUCTS_HISTORYS].ID DESC
                             ", QUERYS.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

      

        Grid3.DataSource = dt;
        Grid3.DataBind();
    }

    protected void grid_PageIndexChanging3(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }

    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);

    }


    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {       

    }

    private void BindGrid4(string ISCLOSED)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        if(ISCLOSED.Equals("進行中"))
        {

            cmdTxt.AppendFormat(@"                           
                            WITH BASE AS (
                                SELECT * 
                                FROM [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                                WHERE ISCLOSED = 'N'
                            )

                            SELECT '依分類' AS '分類', [KINDS] AS 'KINDS' , COUNT(*) AS 'NUMS'
                            FROM BASE
                            GROUP BY [KINDS]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依負責人' AS KINDS, [OWNER], COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY [OWNER]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依狀態' AS KINDS, [STAGES], COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY [STAGES]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依年度' AS KINDS,  CONVERT(NVARCHAR,YEAR(CREATEDATES)), COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY YEAR(CREATEDATES)


                             ");
        }
        else if (ISCLOSED.Equals("已完成"))
        {

            cmdTxt.AppendFormat(@"                           
                            WITH BASE AS (
                                SELECT * 
                                FROM [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                                WHERE ISCLOSED = 'Y'
                            )

                            SELECT '依分類' AS '分類', [KINDS] AS 'KINDS' , COUNT(*) AS 'NUMS'
                            FROM BASE
                            GROUP BY [KINDS]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依負責人' AS KINDS, [OWNER], COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY [OWNER]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依狀態' AS KINDS, [STAGES], COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY [STAGES]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依年度' AS KINDS,  CONVERT(NVARCHAR,YEAR(CREATEDATES)), COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY YEAR(CREATEDATES)



                             ");
        }
        else if (ISCLOSED.Equals("全部"))
        {

            cmdTxt.AppendFormat(@"                           
                            WITH BASE AS (
                                SELECT * 
                                FROM [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                              
                            )

                            SELECT '依分類' AS '分類', [KINDS] AS 'KINDS' , COUNT(*) AS 'NUMS'
                            FROM BASE
                            GROUP BY [KINDS]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依負責人' AS KINDS, [OWNER], COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY [OWNER]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依狀態' AS KINDS, [STAGES], COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY [STAGES]
                            UNION ALL
                            SELECT '--', '--', NULL
                            UNION ALL
                            SELECT '依年度' AS KINDS,  CONVERT(NVARCHAR,YEAR(CREATEDATES)), COUNT(*) AS NUMS
                            FROM BASE
                            GROUP BY YEAR(CREATEDATES)



                             ");
        }
        
        cmdTxt.AppendFormat(@" ");

        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        //匯出專用
        EXCELDT4 = dt;

        Grid4.DataSource = dt;
        Grid4.DataBind();
    }

    protected void grid_PageIndexChanging4(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid4_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);

    }


    public void OnBeforeExport4(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL4();
    }

    public void ADD_TB_PROJECTS_PRODUCTS_HISTORYS(
        string SID,
        string NO,
        string PROJECTNAMES,
        string KINDS,       
        string OWNER,
        string STATUS,
        string TASTESREPLYS,
        string DESIGNER,
        string DESIGNREPLYS,
        string PURREPLYS,
        string QCREPLYS,
        string DOC_NBR,
        string STAGES,
        string ISCLOSED
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @"                           
                            INSERT INTO [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS_HISTORYS]
                            (
                            [SID]
                            ,[NO]
                            ,[PROJECTNAMES]
                            ,[KINDS]                           
                            ,[OWNER]
                            ,[STATUS]
                            ,[TASTESREPLYS]
                            ,[DESIGNER]
                            ,[DESIGNREPLYS]
                            ,[PURREPLYS]
                            ,[QCREPLYS]
                            ,[DOC_NBR]
                            ,[STAGES]
                            ,[ISCLOSED]
                        
                            )
                            VALUES
                            (
                            @SID
                            ,@NO
                            ,@PROJECTNAMES
                            ,@KINDS                         
                            ,@OWNER
                            ,@STATUS
                            ,@TASTESREPLYS
                            ,@DESIGNER
                            ,@DESIGNREPLYS
                            ,@PURREPLYS
                            ,@QCREPLYS
                            ,@DOC_NBR
                            ,@STAGES
                            ,@ISCLOSED
                        
                            )
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    cmd.Parameters.AddWithValue("@SID", SID);
                    cmd.Parameters.AddWithValue("@NO", NO);
                    cmd.Parameters.AddWithValue("@PROJECTNAMES", PROJECTNAMES);
                    cmd.Parameters.AddWithValue("@KINDS", KINDS);                   
                    cmd.Parameters.AddWithValue("@OWNER", OWNER);
                    cmd.Parameters.AddWithValue("@STATUS", STATUS);
                    cmd.Parameters.AddWithValue("@TASTESREPLYS", TASTESREPLYS);
                    cmd.Parameters.AddWithValue("@DESIGNER", DESIGNER);
                    cmd.Parameters.AddWithValue("@DESIGNREPLYS", DESIGNREPLYS);
                    cmd.Parameters.AddWithValue("@QCREPLYS", QCREPLYS);
                    cmd.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
                    cmd.Parameters.AddWithValue("@STAGES", STAGES);
                    cmd.Parameters.AddWithValue("@ISCLOSED", ISCLOSED);
                    cmd.Parameters.AddWithValue("@PURREPLYS", PURREPLYS);



                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected >= 1)
                    {
                        //MsgBox(NO + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    public void UPDATE_TB_PROJECTS_PRODUCTS_STATUS(
        string ID,
        string NO,
        string STATUS,
        string TASTESREPLYS,
        string DESIGNREPLYS,
        string DESIGNER,
        string PURREPLYS,
        string QCREPLYS
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                            UPDATE [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                            SET [STATUS]=@STATUS,[UPDATEDATES]=@UPDATEDATES,[TASTESREPLYS]=@TASTESREPLYS,[DESIGNREPLYS]=@DESIGNREPLYS,[DESIGNER]=@DESIGNER,[QCREPLYS]=@QCREPLYS,[PURREPLYS]=@PURREPLYS
                            WHERE [ID]=@ID
                        
                            
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@STATUS", STATUS);
                    cmd.Parameters.AddWithValue("@TASTESREPLYS", TASTESREPLYS);
                    cmd.Parameters.AddWithValue("@DESIGNREPLYS", DESIGNREPLYS);
                    cmd.Parameters.AddWithValue("@DESIGNER", DESIGNER);
                    cmd.Parameters.AddWithValue("@QCREPLYS", QCREPLYS);
                    cmd.Parameters.AddWithValue("@UPDATEDATES", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@PURREPLYS", PURREPLYS);

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(NO + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    public void UPDATE_TB_PROJECTS_PRODUCTS(
                    string ID,
                    string NO,
                    string PROJECTNAMES,
                    string KINDS,                    
                    string OWNER,
                    string STATUS,
                    string TASTESREPLYS,
                    string DESIGNER,
                    string DESIGNREPLYS,
                    string PURREPLYS,
                    string QCREPLYS,
                    string DOC_NBR,
                    string STAGES,
                    string ISCLOSED
                    )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                         UPDATE [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                         SET [NO]=@NO
                            ,[PROJECTNAMES]=@PROJECTNAMES                           
                            ,[OWNER]=@OWNER
                            ,[STATUS]=@STATUS
                            ,[ISCLOSED]=@ISCLOSED
                            ,[UPDATEDATES]=@UPDATEDATES
                            ,[TASTESREPLYS]=@TASTESREPLYS
                            ,[DOC_NBR]=@DOC_NBR
                            ,[KINDS]=@KINDS
                            ,[STAGES]=@STAGES
                            ,[DESIGNREPLYS]=@DESIGNREPLYS
                            ,[DESIGNER]=@DESIGNER
                            ,[PURREPLYS]=@PURREPLYS
                            ,[QCREPLYS]=@QCREPLYS
                            WHERE [ID]=@ID
                        
                            
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@NO", NO);
                    cmd.Parameters.AddWithValue("@PROJECTNAMES", PROJECTNAMES);
                    cmd.Parameters.AddWithValue("@KINDS", KINDS);                    
                    cmd.Parameters.AddWithValue("@OWNER", OWNER);
                    cmd.Parameters.AddWithValue("@STATUS", STATUS);
                    cmd.Parameters.AddWithValue("@TASTESREPLYS", TASTESREPLYS);
                    cmd.Parameters.AddWithValue("@ISCLOSED", ISCLOSED);
                    cmd.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
                    cmd.Parameters.AddWithValue("@STAGES", STAGES);                 
                    cmd.Parameters.AddWithValue("@UPDATEDATES", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@DESIGNREPLYS", DESIGNREPLYS);
                    cmd.Parameters.AddWithValue("@DESIGNER", DESIGNER);
                    cmd.Parameters.AddWithValue("@QCREPLYS", QCREPLYS);
                    cmd.Parameters.AddWithValue("@PURREPLYS", PURREPLYS);

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(NO + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    public void ADD_TB_PROJECTS_PRODUCTS(                   
                    string NO,
                    string PROJECTNAMES,                    
                    string OWNER,
                    string STATUS,
                    string ISCLOSED                   
                    )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                        INSERT INTO [TKRESEARCH].[dbo].[TB_PROJECTS_PRODUCTS]
                        (
                        [NO]
                        ,[PROJECTNAMES]                     
                        ,[OWNER]
                        ,[STATUS]
                        ,[ISCLOSED]                    
                        )
                        VALUES
                        (
                        @NO
                        ,@PROJECTNAMES                      
                        ,@OWNER
                        ,@STATUS
                        ,@ISCLOSED                      
                        )                                                    
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {                  
                    cmd.Parameters.AddWithValue("@NO", NO);
                    cmd.Parameters.AddWithValue("@PROJECTNAMES", PROJECTNAMES);                    
                    cmd.Parameters.AddWithValue("@OWNER", OWNER);
                    cmd.Parameters.AddWithValue("@STATUS", STATUS);
                    cmd.Parameters.AddWithValue("@ISCLOSED", ISCLOSED);
                    cmd.Parameters.AddWithValue("@UPDATEDATES", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(NO + " 完成", this.Page, this);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    public void SETEXCEL1()
    {
        BindGrid();
        //BindGrid 中已帶入 EXCELDT1
        DataTable EXDT = EXCELDT1;

        if (EXDT.Rows.Count>=1)
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
                ws.Cells[1, 1].Value = "專案編號";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 2].Value = "項目名稱";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線                
                ws.Cells[1, 3].Value = "專案負責人";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 4].Value = "研發進度回覆";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 5].Value = "業務進度回覆";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 6].Value = "設計負責人";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 7].Value = "設計回覆";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線 品保回覆
                ws.Cells[1, 8].Value = "採購回覆";
                ws.Cells[1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線 
                ws.Cells[1, 9].Value = "品保回覆";
                ws.Cells[1, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線  
                ws.Cells[1, 10].Value = "專案階段";
                ws.Cells[1, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線  
                ws.Cells[1, 11].Value = "是否結案";
                ws.Cells[1, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線  
                ws.Cells[1, 12].Value = "表單編號";
                ws.Cells[1, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線 
                ws.Cells[1, 13].Value = "ID";
                ws.Cells[1, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位置中
                ws.Cells[1, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 13].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線 

                foreach (DataRow od in EXDT.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["專案編號"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 2].Value = od["項目名稱"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 3].Value = od["專案負責人"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 4].Value = od["研發進度回覆"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 5].Value = od["業務進度回覆"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 6].Value = od["設計負責人"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 7].Value = od["設計回覆"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 8].Value = od["採購回覆"].ToString();
                    ws.Cells[ROWS, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 9].Value = od["品保回覆"].ToString();
                    ws.Cells[ROWS, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 10].Value = od["專案階段"].ToString();
                    ws.Cells[ROWS, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 11].Value = od["是否結案"].ToString();
                    ws.Cells[ROWS, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 12].Value = od["表單編號"].ToString();
                    ws.Cells[ROWS, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 13].Value = od["ID"].ToString();
                    ws.Cells[ROWS, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 13].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

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

    public void SETEXCEL4()
    {
        BindGrid4(DropDownList_ISCLOSED.SelectedValue.ToString());
        //BindGrid4 中已帶入 EXCELDT4
        DataTable EXDT = EXCELDT4;

        if (EXDT.Rows.Count >= 1)
        {
            //檔案名稱
            var fileName = "統計" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xlsx";
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
                ws.Cells[1, 1].Value = "分類";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 2].Value = "明細";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線                
                ws.Cells[1, 3].Value = "件數";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                
                foreach (DataRow od in EXDT.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["分類"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 2].Value = od["KINDS"].ToString();
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 3].Value = od["NUMS"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                   

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

    public DataTable FIND_TB_PROJECTS_ROLES(string NAMES)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ROLES", typeof(String));
        dt.Columns.Add("NAMES", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder SQL = new StringBuilder();
        SQL.AppendFormat(@"
                       SELECT TOP 1
                        [ROLES]
                        ,[NAMES]
                        FROM [TKRESEARCH].[dbo].[TB_PROJECTS_ROLES]
                        WHERE [NAMES]='{0}'
                        ", NAMES);


        dt.Load(m_db.ExecuteReader(SQL.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return null;
        }

    }
    public void SET_ALLOWED_MODIFY_GV1(GridViewRow row)
    {
        Button Button6 = (Button)row.FindControl("Button6");
        Button Button7 = (Button)row.FindControl("Button7");

        if (Button6 != null) Button6.Visible = false;
        if (Button7 != null) Button7.Visible = false;

        if (ROLES != null)
        {
            if (ROLES.Equals("ADMIN"))
            {
                // 管理員全部可編輯（範例）
                if (Button6 != null) Button6.Visible = true;
                if (Button7 != null) Button7.Visible = true;

            }
            else if (ROLES.Equals("MANAGER"))
            {
                //MANAGER
                if (Button6 != null) Button6.Visible = true;
                if (Button7 != null) Button7.Visible = true;
            }           
        }
        else
        {
            if (Button6 != null) Button6.Visible = false;
            if (Button7 != null) Button7.Visible = false;
        }
    }
    public void SET_ALLOWED_MODIFY_GV2(GridViewRow row)
    {
        Button Button3 = (Button)row.FindControl("Button3");     
       
        if (Button3 != null) Button3.Visible = false;
      

        if (ROLES!=null)
        {
            if (ROLES.Equals("ADMIN"))
            {
                // 管理員全部可編輯（範例）
                if (Button3 != null) Button3.Visible = true;            

            }
            else if (ROLES.Equals("MANAGER"))
            {
                //MANAGER
                if (Button3 != null) Button3.Visible = true;            

            }
        }        
        else
        {
            if (Button3 != null) Button3.Visible = false;
           
        }
    }

    public DataTable FIND_TB_EMAILS_BY_NAMES(string NAMES)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("EMAILS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder SQL = new StringBuilder();
        SQL.AppendFormat(@"
                        SELECT TOP 1
                        [ID]
                        ,[NAMES]
                        ,[EMAILS]
                        ,[KINDS]
                        FROM [TKRESEARCH].[dbo].[TB_EMAILS]
                        WHERE [NAMES]='{0}'
                        ", NAMES);


        dt.Load(m_db.ExecuteReader(SQL.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return null;
        }

    }

    public DataTable FIND_TB_EMAILS_BY_KINDS(string KINDS)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("EMAILS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder SQL = new StringBuilder();
        SQL.AppendFormat(@"
                        SELECT 
                        [ID]
                        ,[NAMES]
                        ,[EMAILS]
                        ,[KINDS]
                        FROM [TKRESEARCH].[dbo].[TB_EMAILS]
                        WHERE [KINDS]='{0}'
                        ", KINDS);


        dt.Load(m_db.ExecuteReader(SQL.ToString()));

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return null;
        }

    }

    public DataTable SET_MAILTO(string OWNER)
    {
        DataTable DT_MAILS = new DataTable();
        // 建立欄位
        DT_MAILS.Columns.Add("EMAILS", typeof(string));

        //負責人
        //FIND_TB_EMAILS_BY_NAMES
        DataTable DT_NAMES = FIND_TB_EMAILS_BY_NAMES(OWNER);
        if (DT_NAMES != null && DT_NAMES.Rows.Count >= 1)
        {
            // 新增一筆資料
            DataRow newRowNames = DT_MAILS.NewRow();
            newRowNames["EMAILS"] = DT_NAMES.Rows[0]["EMAILS"].ToString();
            DT_MAILS.Rows.Add(newRowNames);
        }

        //研發群
        //FIND_TB_EMAILS_BY_KINDS
        DataTable DT_MANAGER = FIND_TB_EMAILS_BY_KINDS("MANAGER");
        if (DT_MANAGER != null && DT_MANAGER.Rows.Count >= 1)
        {
            foreach (DataRow DRrows in DT_MANAGER.Rows)
            {
                DT_MAILS.ImportRow(DRrows);
            }
        }
        // 新增一筆資料
        DataRow newRow = DT_MAILS.NewRow();
        newRow["EMAILS"] = "tk290@tkfood.com.tw";

        DT_MAILS.Rows.Add(newRow);

        if(DT_MAILS!=null && DT_MAILS.Rows.Count>=1)
        {
            return DT_MAILS;
        }
        else
        {
            return null;
        }
    }

    public DataTable SET_MAILTO_DESIGNER(string OWNER)
    {
        DataTable DT_MAILS = new DataTable();
        // 建立欄位
        DT_MAILS.Columns.Add("EMAILS", typeof(string));

        //負責人
        //FIND_TB_EMAILS_BY_NAMES
        DataTable DT_NAMES = FIND_TB_EMAILS_BY_NAMES(OWNER);
        if (DT_NAMES != null && DT_NAMES.Rows.Count >= 1)
        {
            // 新增一筆資料
            DataRow newRowNames = DT_MAILS.NewRow();
            newRowNames["EMAILS"] = DT_NAMES.Rows[0]["EMAILS"].ToString();
            DT_MAILS.Rows.Add(newRowNames);
        }

        //研發群
        //FIND_TB_EMAILS_BY_KINDS
        DataTable DT_MANAGER = FIND_TB_EMAILS_BY_KINDS("MANAGER");
        if (DT_MANAGER != null && DT_MANAGER.Rows.Count >= 1)
        {
            foreach (DataRow DRrows in DT_MANAGER.Rows)
            {
                DT_MAILS.ImportRow(DRrows);
            }
        }

        //設計群
        //FIND_TB_EMAILS_BY_KINDS
        DataTable DT_DESIGNER = FIND_TB_EMAILS_BY_KINDS("DESIGNER");
        if (DT_DESIGNER != null && DT_DESIGNER.Rows.Count >= 1)
        {
            foreach (DataRow DRrows in DT_DESIGNER.Rows)
            {
                DT_MAILS.ImportRow(DRrows);
            }
        }
        // 新增一筆資料
        DataRow newRow = DT_MAILS.NewRow();
        newRow["EMAILS"] = "tk290@tkfood.com.tw";

        DT_MAILS.Rows.Add(newRow);

        if (DT_MAILS != null && DT_MAILS.Rows.Count >= 1)
        {
            return DT_MAILS;
        }
        else
        {
            return null;
        }
    }
    public static void SendEmail(string subject, string body,DataTable mailTo)
    {
        MailAddress MAIL_FORM = new MailAddress("tk290@tkfood.com.tw");
        try
        {
            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            string emailAccount = ConfigurationManager.AppSettings["mailAccount"];
            string emailPassword = ConfigurationManager.AppSettings["mailPwd"];
            bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(emailAccount, emailPassword),
                EnableSsl = enableSsl
            };

            MailMessage mail = new MailMessage
            {
                From = MAIL_FORM,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (DataRow DR in mailTo.Rows)
            {
                if (!string.IsNullOrWhiteSpace(DR["EMAILS"].ToString()))
                {
                    mail.To.Add(DR["EMAILS"].ToString());
                }
            }

            smtpClient.Send(mail);
        }
        catch (Exception ex)
        {
            //Console.WriteLine("寄送失敗: " + ex.Message);
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
    protected void Button4_Click(object sender, EventArgs e)
    {
        //MsgBox("OK", this.Page, this);

        string NO = NEW_專案編號.Text.Trim();
        string PROJECTNAMES = NEW_項目名稱.Text.Trim();       
        string OWNER = NEW_專案負責人.Text.Trim();
        string STATUS = NEW_研發進度回覆.Text.Trim();
        string ISCLOSED = "N";
        string UPDATEDATES = DateTime.Now.ToString("yyyyMMdd");

        if(!string.IsNullOrEmpty(NO) && !string.IsNullOrEmpty(NO))
        {
            ADD_TB_PROJECTS_PRODUCTS(
                               NO,
                               PROJECTNAMES,                              
                               OWNER,
                               STATUS,
                               ISCLOSED

                               );
            BindGrid();
            BindGrid2();
        }
        else
        {
            MsgBox("專案編號、項目名稱不可空白", this.Page, this);
        }
       

    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        string PROJECTNAMES = TextBox2.Text.Trim();
        if (!string.IsNullOrEmpty(PROJECTNAMES))
        {
            BindGrid3(PROJECTNAMES);
        }
        else
        {
            MsgBox("項目名稱不可空白", this.Page, this);
        }
      
    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        BindGrid4(DropDownList_ISCLOSED.SelectedValue.ToString());
    }

    #endregion
}