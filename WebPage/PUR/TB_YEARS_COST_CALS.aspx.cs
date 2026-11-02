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

public partial class CDS_WebPage_PUR_TB_YEARS_COST_CALS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            SETDATE();
            BindGrid();
        }
    }

    #region FUNCTION
    public void SETDATE()
    {
        int last_years = DateTime.Now.Year - 1;
        TextBox_YEARS.Text= last_years.ToString();
    }
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        if(!string.IsNullOrEmpty(TextBox_YEARS.Text))
        {
            QUERYS1.AppendFormat(@" AND  [年度] = '{0}'", TextBox_YEARS.Text);
        }
        else
        {
            QUERYS1.AppendFormat(@"");
        }

       

        cmdTxt.AppendFormat(@"   
                            SELECT 
                                CASE 
                                    WHEN GROUPING([類別]) = 1 THEN '總合計'
                                    WHEN GROUPING([ID]) = 1 THEN [類別] + ' 小計'
                                    ELSE CAST([ID] AS NVARCHAR(50)) 
                                END AS [編號說明],    
                                -- 小計列不顯示年度
                                CASE WHEN GROUPING([ID]) = 0 THEN [年度] ELSE '' END AS [年度],
                                [類別],    
                                CASE WHEN GROUPING([ID]) = 0 THEN AVG([營業成本百分比a]) ELSE NULL END  AS [營業成本百分比],    
                                -- 修正點：如果是小計或合計列 (GROUPING = 1)，明細顯示空字串
                                CASE WHEN GROUPING([ID]) = 0 THEN MAX([明細]) ELSE '' END AS [明細],     
                                CASE WHEN GROUPING([類別]) <> 1 THEN SUM([進貨金額佔類別平均%b]) ELSE NULL END AS [進貨金額佔類別平均],
                                CASE WHEN GROUPING([ID]) = 0 THEN SUM([調漲增加(減少)c]) ELSE NULL END   AS [調漲增加減少],    
                                -- 計算 d 欄位的加總
                                SUM([影響成本率增加%  d=a*b*c]) AS [影響成本率增加]  

                            FROM [TKRESEARCH].[dbo].[TB_YEARS_COST_CALS]
                            WHERE 1=1
                            {0}
                            GROUP BY GROUPING SETS (
                                ([類別], [ID], [年度]), -- 明細層
                                ([類別]),               -- 類別小計層
                                ()                      -- 總合計層
                            )
                            ORDER BY 
                                GROUPING([類別]) ASC, 
                                [類別], 
                                GROUPING([ID]) ASC, 
                                [ID];


                            ", QUERYS1.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

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
        // 1. 確保目前處理的是資料列 (排除 Header 和 Footer)
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 2. 取得「年度」欄位的值 (假設資料庫欄位名稱為 "YEARS")
            // 使用 DataBinder.Eval 可以安全取得該列的原始資料
            object rowData = DataBinder.Eval(e.Row.DataItem, "年度");
            string yearValue = (rowData == null) ? "" : rowData.ToString().Trim();

            // 3. 尋找該列中的 Button1 控制項
            Button btn1 = (Button)e.Row.FindControl("Grid1Button1");
            TextBox txtNewField = (TextBox)e.Row.FindControl("txt_調漲增加減少");
            // 3. 判斷年度是否為空
            if (string.IsNullOrEmpty(yearValue) || yearValue == "0")
            {
                // 變更整列底色為橘色 (Orange)
                e.Row.BackColor = System.Drawing.Color.Orange;

                // 如果想要字體變粗或顏色變深也可以順便設定
                // e.Row.ForeColor = System.Drawing.Color.White; 
            }

            if (btn1 != null)
            {
                // 4. 如果年度是空的、NULL 或字串 "0"，就隱藏按鈕
                if (string.IsNullOrEmpty(yearValue) || yearValue == "0")
                {
                    btn1.Visible = false;
                    txtNewField.Visible = false;
                    // 或者用 btn1.Style["display"] = "none";
                }
                else
                {
                    btn1.Visible = true;
                    txtNewField.Visible = true;
                }
            }
        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid1Button1")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid1.Rows[rowIndex];
                TextBox txtNewField = (TextBox)row.FindControl("txt_調漲增加減少");
                string newTextValue = txtNewField.Text.Trim();

                // 獲取相應的ID
                Label txtid = (Label)row.FindControl("Label_ID");
                string id = txtid.Text;

                UPDATE_TB_YEARS_COST_CALS(id, newTextValue);

                MsgBox(id + " 完成", this.Page, this);
                // 在這裡執行保存的邏輯，例如將新的文本值與ID保存到資料庫中
                // ...

                // 重新繫結GridView，刷新顯示
                BindGrid();
            }
        }
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

    }


    public void SETEXCEL()
    {

    }

    public void UPDATE_TB_YEARS_COST_CALS(string ID,string newTextValue)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            string cmdTxt = @"   ";


            cmdTxt = @"
                UPDATE  [TKRESEARCH].[dbo].[TB_YEARS_COST_CALS]
                SET [調漲增加(減少)c]=@newTextValue
                ,[影響成本率增加%  d=a*b*c]=[營業成本百分比a]*[進貨金額佔類別平均%b]/1000*@newTextValue
                WHERE [ID]=@ID
                        ";


            m_db.AddParameter("@ID", ID);
            m_db.AddParameter("@newTextValue", newTextValue);

            m_db.ExecuteNonQuery(cmdTxt);
        }
        catch(Exception EX)
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
    #endregion
}