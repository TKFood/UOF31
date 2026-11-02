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

public partial class CDS_WebPage_PUR_TBOUTPRODSPERCENTS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    DataTable EXCELDT1 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            SETQUERY();
        }
    }

    #region FUNCTION
    public void SETQUERY()
    {
        // 取得今年這個月的第一天
        DateTime firstDayOfThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        Date1.Text = firstDayOfThisMonth.ToString("yyyy-MM-dd");

        // 取得本月的最後一天
        DateTime firstDayOfNextMonth = firstDayOfThisMonth.AddMonths(1);
        DateTime lastDayOfThisMonth = firstDayOfNextMonth.AddDays(-1);
        // 修正：這裡原本寫錯變數了
        Date2.Text = lastDayOfThisMonth.ToString("yyyy-MM-dd");
    }
    private void BindGrid(string SDATES,string EDATES)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();

        // 假設 SDATES 和 EDATES 是從 Date1.Text 和 Date2.Text 取得的字串
        SDATES = Date1.Text.Replace("-", "");
        EDATES = Date2.Text.Replace("-", "");

        if(!string.IsNullOrEmpty(QUERY_TextBox1.Text))
        {
            QUERYS.AppendFormat(@" AND (TA032 LIKE '%{0}%' OR MA002 LIKE '%{0}%')", QUERY_TextBox1.Text.Trim());
        }
        else
        {
            QUERYS.AppendFormat(@" ");
        }
        if (!string.IsNullOrEmpty(QUERY_TextBox2.Text))
        {
            QUERYS2.AppendFormat(@" AND (TA006 LIKE '%{0}%' OR MB1.MB002 LIKE '%{0}%')", QUERY_TextBox2.Text.Trim());
        }
        else
        {
            QUERYS2.AppendFormat(@" ");
        }

        cmdTxt.AppendFormat(@"  
                            SELECT 
                            TA032 AS '加工廠商'
                            ,MA002 AS '廠商'
                            ,TA001 AS '製令單別'
                            ,TA002 AS '製令單號'
                            ,TA003 AS '製令日期'
                            ,TA009 AS '預計開工'
                            ,TA006 AS '產品品號'
                            ,MB1.MB002 AS '產品品名'
                            ,TA015 AS '預計產量'
                            ,TA017 AS '已生產量'
                            ,TA007 AS '生產單位'
                            ,TB003 AS '材料品號'
                            ,TB012 AS '材料品名'
                            ,TB004 AS '需領用量'
                            ,TB005 AS '已領用量'
                            ,TB007 AS '材料單位'
                            ,(CASE WHEN TB005>0 AND TB004>0 THEN CONVERT(decimal(16,2),(TB005/TB004)*100) ELSE 0 END ) AS '領用率'
                            ,(CASE WHEN TB005>0 AND TB004>0 AND CONVERT(decimal(16,2),(TB005/TB004)*100)>100 THEN CONVERT(decimal(16,2),(TB005/TB004)*100)-100 ELSE 0 END ) AS '損耗率'
                            ,ISNULL([PERCENTS],'') AS '合約耗損率'
                            , MB1.UDF07
                            --分母=MB1.UDF07*TA017/1000(單位是KG)，分子=TB005(單位是KG)
                            ,(CASE WHEN (TB003 LIKE '3%' OR TB003 LIKE '4%' OR TB003 LIKE '5%' ) AND MB1.UDF07*TA017>0 AND TB005>0  THEN CONVERT(decimal(16,2),((MB1.UDF07*TA017/1000)/TB005)*100) ELSE 0 END ) AS '得料率'

                            FROM [TK].dbo.MOCTA
                            LEFT JOIN [TK].dbo.PURMA ON MA001=TA032
                            ,[TK].dbo.MOCTB
                            LEFT JOIN [TKPUR].[dbo].[TBOUTPRODSPERCENTS] ON  [TBOUTPRODSPERCENTS].[MB001]=TB003
                            ,[TK].dbo.INVMB MB1
                            ,[TK].dbo.INVMB MB2
                            WHERE TA001=TB001 AND TA002=TB002
                            AND TA006=MB1.MB001
                            AND TB003=MB2.MB001
                            AND TA001 IN ('A512')
                            AND TA003>='{0}' AND TA003<='{1}'
                            {2}
                            {3}
                            ORDER BY TA001,TA002,TA006,TB003

                             ", SDATES, EDATES, QUERYS.ToString(), QUERYS2.ToString());

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
       
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);
    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL1();

    }


    public void SETEXCEL1()
    {
        BindGrid(Date1.Text, Date2.Text);
        //BindGrid中已帶入EXCELDT1
        if (EXCELDT1.Rows.Count >= 1)
        {
           
        }
    }

    private void BindGrid_TBOUTPRODSPERCENTS()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();


        cmdTxt.AppendFormat(@"  
                            SELECT 
                            [MB001]
                            ,[MB002]
                            ,[PERCENTS]
                            FROM [TKPUR].[dbo].[TBOUTPRODSPERCENTS]
                            ORDER BY [MB001]

                             ");

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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn1 = (Button)e.Row.FindControl("GW2_Button1");
            if (btn1 != null)
            {
                string cellValue1 = btn1.CommandArgument;
                dynamic param1 = new { ID = cellValue1 }.ToExpando();
            }
        }
    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;
        // 獲取所選行的索引
        rowIndex = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "GW2_Button1")
        {
            //MsgBox(e.CommandArgument.ToString() + "OK", this.Page, this);
            if (rowIndex >= 0)
            {
                // 獲取TextBox的值
                GridViewRow row = Grid2.Rows[rowIndex];               

                Label Label_MB001 = (Label)row.FindControl("Label_材料品號");
                string MB001 = Label_MB001.Text;



                DELETE_TBOUTPRODSPERCENTS(
                               MB001
                               );
              
            }

            BindGrid_TBOUTPRODSPERCENTS();

        }
    }


    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL2();
    }


    public void SETEXCEL2()
    {   
    }

    public void ADD_TBOUTPRODSPERCENTS(
        string MB001,
        string MB002,
        string PERCENTS
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                            INSERT INTO [TKPUR].[dbo].[TBOUTPRODSPERCENTS]
                            (
                            [MB001]
                            ,[MB002]
                            ,[PERCENTS]
                            )
                            VALUES
                            (
                            @MB001
                            ,@MB002
                            ,@PERCENTS
                            )                                      
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    // 傳入參數
                    cmd.Parameters.AddWithValue("@MB001", MB001);
                    cmd.Parameters.AddWithValue("@MB002", MB002);
                    cmd.Parameters.AddWithValue("@PERCENTS", PERCENTS);
                  

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(MB001 + " 完成", this.Page, this);
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

    public void DELETE_TBOUTPRODSPERCENTS(string MB001)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = @" 
                           DELETE [TKPUR].[dbo].[TBOUTPRODSPERCENTS]
                           WHERE MB001=@MB001                         
                            ";

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {
                    // 傳入參數
                    cmd.Parameters.AddWithValue("@MB001", MB001);                   


                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        MsgBox(MB001 + " 完成", this.Page, this);
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
        BindGrid(Date1.Text ,Date2.Text);
        BindGrid_TBOUTPRODSPERCENTS();

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string MB001 = TextBox1.Text;
        string MB002 = TextBox2.Text;
        string PERCENTS = TextBox3.Text;

        ADD_TBOUTPRODSPERCENTS(
        MB001,
        MB002,
        PERCENTS
        );

        MsgBox(MB001 + " 完成", this.Page, this);

    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        BindGrid_TBOUTPRODSPERCENTS();
    }

    #endregion
}