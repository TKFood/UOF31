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

public partial class CDS_WebPage_PUR_REPORT_ACTTB : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        
        if (!IsPostBack)
        {
            TextBox_起始日.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
            TextBox_結束日.Text = new DateTime(DateTime.Now.Year, 12, 31).ToString("yyyy-MM-dd");
        }
    }

    #region FUNCTION
    private void BindGrid1(string TB010, string yyyymmdd_start, string yyyymmdd_end)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();


        //TextBox1
        if (!string.IsNullOrEmpty(TB010))
        {
            QUERYS.AppendFormat(@" AND TB010 LIKE '%{0}%' ", TB010);
        }


        cmdTxt.AppendFormat(@"
                            SELECT 
                            CONVERT(NVARCHAR,(CONVERT(datetime,TA003)),111) TA003
                            ,TB010
                            ,TB007

                            FROM [TK].dbo.ACTTA WITH(NOLOCK),[TK].dbo.ACTTB WITH(NOLOCK)
                            WHERE TA001=TB001 AND TA002=TB002
                            AND TA001 IN ('A911')           
                            AND TB004 IN ('-1')
                            AND ISNULL(TB010,'')<>''
                            AND NOT EXISTS (
                                SELECT 1
                                FROM [TKKPI].[dbo].[UOFTB010NOTIN] N
                                WHERE TB010 LIKE '%' + N.TB010NOTIN + '%'
                                )
                            AND TA003>='{1}' AND TA003<='{2}'
                            {0}
                            ORDER BY TA003
                             ", QUERYS.ToString(), yyyymmdd_start, yyyymmdd_end);




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

    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);

    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

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
        string TB010 = TextBox1.Text.Trim();
        DateTime dt_start;
        DateTime dt_end;
        string yyyymmdd_start = "";
        string yyyymmdd_end = "";
        if (DateTime.TryParse(TextBox_起始日.Text, out dt_start))
        {
            yyyymmdd_start = dt_start.ToString("yyyyMMdd");
            // 使用 yyyymmdd
        }
        else
        {
            // 錯誤處理：輸入不是有效日期
            MsgBox("不是有效日期", this.Page, this);
        }
        if (DateTime.TryParse(TextBox_結束日.Text, out dt_end))
        {
            yyyymmdd_end = dt_end.ToString("yyyyMMdd");
            // 使用 yyyymmdd
        }
        else
        {
            // 錯誤處理：輸入不是有效日期
            MsgBox("不是有效日期", this.Page, this);
        }

        if (!string.IsNullOrEmpty(TB010))
        {
            BindGrid1(TB010, yyyymmdd_start, yyyymmdd_end);
        }
        else
        {
            MsgBox("關鍵字不得空白", this.Page, this);
        }
    }
    #endregion
}