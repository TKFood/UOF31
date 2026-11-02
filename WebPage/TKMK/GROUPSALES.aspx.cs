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

public partial class CDS_WebPage_TKMK_GROUPSALES : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNT = null;
    string NAME = null;
    String ROLES = null;

    string MU002 = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        if (!IsPostBack)
        {
            Label_Todays.Text = "本日日期: "+DateTime.Now.ToString("yyyyMMdd");
            string CREATEDATES = DateTime.Now.ToString("yyyyMMdd");

            SEARCHGROUPSALES(CREATEDATES);

            BindDropDownList1();
            BindDropDownList2();

            //BindGrid();
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
        string CREATEDATES = DateTime.Now.ToString("yyyyMMdd");
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat(@"
                        SELECT 
                        LTRIM(RTRIM((MU001)))+' '+SUBSTRING(MU002,1,3) AS 'MU001' 
                        FROM [TK].dbo.POSMU WHERE (MU001 LIKE '69%')   
                        AND MU001 NOT IN (
	                        SELECT [EXCHANACOOUNT] FROM [TKMK].[dbo].[GROUPSALES] 
	                        WHERE CONVERT(nvarchar,[CREATEDATES],112)='{0}'  AND [STATUS]='預約接團' 
                        ) ORDER BY MU001
                    ", CREATEDATES);
        

        BindDropDownList(DropDownList1, sql.ToString(), "MU001", "MU001");
    }
    private void BindDropDownList2()
    {
        string CREATEDATES = DateTime.Now.ToString("yyyyMMDd");
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat(@"
                         SELECT [PARASNAMES],[DVALUES] FROM [TKMK].[dbo].[TBZPARAS] WHERE [KINDS]='ISEXCHANGE' ORDER BY [PARASNAMES]
                         ");


        BindDropDownList(DropDownList2, sql.ToString(), "PARASNAMES", "PARASNAMES");
    }

    /// <summary>
    /// 找出團務資料
    /// </summary>
    /// <param name="CREATEDATES"></param>
    public void SEARCHGROUPSALES(string CREATEDATES)
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder QUERY1 = new StringBuilder();
    
        // 2. 定義 SQL 查詢字串         

        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                             SELECT  
                            [SERNO] AS '序號'
                            ,[CARNAME] AS '車名'
                            ,[CARNO] AS '車號'
                            ,[CARKIND] AS '車種'
                            ,[GROUPKIND]  AS '團類'
                            ,[ISEXCHANGE] AS '兌換券'
                            ,[EXCHANGETOTALMONEYS] AS '券總額'
                            ,[EXCHANGESALESMMONEYS] AS '券消費'
                            ,[SALESMMONEYS] AS '消費總額'
                            ,[SPECIALMNUMS] AS '特賣數'
                            ,[SPECIALMONEYS] AS '特賣獎金'
                            ,[COMMISSIONBASEMONEYS] AS '茶水費'
                            ,[COMMISSIONPCTMONEYS] AS '消費獎金'
                            ,[TOTALCOMMISSIONMONEYS] AS '總獎金'
                            ,[CARNUM] AS '車數'
                            ,[GUSETNUM] AS '交易筆數'
                            ,[CARCOMPANY] AS '來車公司'
                            ,[TA008NO] AS '業務員名'
                            ,[TA008] AS '業務員帳號'
                            ,[EXCHANNO] AS '優惠券名'
                            ,[EXCHANACOOUNT] AS '優惠券帳號'
                            ,[PLAYDAYKINDS] AS '旅遊天數'
                            ,[PLAYDAYS] AS '第幾天'
                            ,CONVERT(varchar(100), [GROUPSTARTDATES],120) AS '實際到達時間'
                            ,CONVERT(varchar(100), [GROUPENDDATES],120) AS '實際離開時間'
                            ,[STATUS] AS '狀態'
                            ,CONVERT(varchar(100), [PURGROUPSTARTDATES],120) AS '預計到達時間'
                            ,CONVERT(varchar(100), [PURGROUPENDDATES],120) AS '預計離開時間'
                            ,[EXCHANGEMONEYS] AS '領券額'
                            ,[ID]
                            ,[CREATEDATES]

                            FROM [TKMK].[dbo].[GROUPSALES] WITH(NOLOCK)
                            WHERE CONVERT(nvarchar,[CREATEDATES],112)='{0}'
                            AND [STATUS]<>'取消預約'
                            ORDER BY CONVERT(nvarchar,[CREATEDATES],112),CONVERT(int,[SERNO]) DESC
                        ", CREATEDATES);


        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));


        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn3 = (Button)e.Row.FindControl("Button3");
            if (btn3 != null)
            {
                string cellValue3 = btn3.CommandArgument;
                dynamic param3 = new { ID = cellValue3 }.ToExpando();
            }
        }
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);

        if (rowIndex >= 0 && rowIndex < Grid1.Rows.Count)
        {
            GridViewRow row = Grid1.Rows[rowIndex];

            TextBox txt序號 = (TextBox)row.FindControl("TextBox_序號");
            TextBox txt車號 = (TextBox)row.FindControl("TextBox_車號");
            TextBox txt車名 = (TextBox)row.FindControl("TextBox_車名");
            TextBox txt優惠券帳號 = (TextBox)row.FindControl("TextBox_優惠券帳號");

            string CREATEDATES = DateTime.Now.ToString("yyyyMMdd");
            string SERNO = txt序號.Text.Trim();
            string CARNO = txt車號.Text.Trim();
            string CARNAME= txt車名.Text.Trim();
            string EXCHANACOOUNT = txt優惠券帳號.Text.Trim();

            UPDATE_GROUPSALES(CREATEDATES, SERNO, CARNO, CARNAME, EXCHANACOOUNT);

        }
    }
    // 雖然不應該被觸發，但定義它以避免 HttpCException
    protected void Grid1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // 什麼都不做，因為您不打算使用內建更新功能

        // 如果 GridView 處於編輯模式，這兩行可以讓它退出編輯模式
        Grid1.EditIndex = -1;
        // Grid1.DataBind(); 
    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        //SETEXCEL();

    }

    public void ADDGROUPSALES(
            string ID
            , string CREATEDATES
            , string SERNO
            , string CARCOMPANY
            , string TA008NO
            , string TA008
            , string CARNO
            , string CARNAME
            , string CARKIND
            , string GROUPKIND
            , string ISEXCHANGE
            , string EXCHANGEMONEYS
            , string EXCHANGETOTALMONEYS
            , string EXCHANGESALESMMONEYS
            , string SPECIALMNUMS
            , string SPECIALMONEYS
            , string SALESMMONEYS
            , string COMMISSIONBASEMONEYS
            , string COMMISSIONPCT
            , string COMMISSIONPCTMONEYS
            , string TOTALCOMMISSIONMONEYS
            , string CARNUM
            , string GUSETNUM
            , string EXCHANNO
            , string EXCHANACOOUNT
            , string PURGROUPSTARTDATES
            , string GROUPSTARTDATES
            , string PURGROUPENDDATES
            , string GROUPENDDATES
            , string STATUS
            , string PLAYDAYKINDS
            , string PLAYDAYS
           )
    {


        try
        {
            // 1.取得連線字串
            // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            // 1. 📌 使用參數化查詢，避免 SQL Injection
            string sqlQuery = @"
                            INSERT INTO [TKMK].[dbo].[GROUPSALES]
                                    (
                                    [CREATEDATES]
                                    ,[SERNO]
                                    ,[CARCOMPANY]
                                    ,[TA008NO]
                                    ,[TA008]
                                    ,[CARNO]
                                    ,[CARNAME]
                                    ,[CARKIND]
                                    ,[GROUPKIND]
                                    ,[ISEXCHANGE]
                                    ,[EXCHANGEMONEYS]
                                    ,[EXCHANGETOTALMONEYS]
                                    ,[EXCHANGESALESMMONEYS]
                                    ,[SPECIALMNUMS]
                                    ,[SPECIALMONEYS]
                                    ,[SALESMMONEYS]
                                    ,[COMMISSIONBASEMONEYS]
                                    ,[COMMISSIONPCT]
                                    ,[COMMISSIONPCTMONEYS]
                                    ,[TOTALCOMMISSIONMONEYS]
                                    ,[CARNUM]
                                    ,[GUSETNUM]
                                    ,[EXCHANNO]
                                    ,[EXCHANACOOUNT]
                                    ,[PURGROUPSTARTDATES]
                                    ,[GROUPSTARTDATES]
                                    ,[PURGROUPENDDATES]
                                    ,[GROUPENDDATES]
                                    ,[STATUS]
                                    ,[PLAYDAYKINDS]
                                    ,[PLAYDAYS]
                                    )
                                    VALUES
                                    (
                                    @CREATEDATES
                                    ,@SERNO
                                    ,@CARCOMPANY
                                    ,@TA008NO
                                    ,@TA008
                                    ,@CARNO
                                    ,@CARNAME
                                    ,@CARKIND
                                    ,@GROUPKIND
                                    ,@ISEXCHANGE
                                    ,@EXCHANGEMONEYS
                                    ,@EXCHANGETOTALMONEYS
                                    ,@EXCHANGESALESMMONEYS
                                    ,@SPECIALMNUMS
                                    ,@SPECIALMONEYS
                                    ,@SALESMMONEYS
                                    ,@COMMISSIONBASEMONEYS
                                    ,@COMMISSIONPCT
                                    ,@COMMISSIONPCTMONEYS
                                    ,@TOTALCOMMISSIONMONEYS
                                    ,@CARNUM
                                    ,@GUSETNUM
                                    ,@EXCHANNO
                                    ,@EXCHANACOOUNT
                                    ,@PURGROUPSTARTDATES
                                    ,@GROUPSTARTDATES
                                    ,@PURGROUPENDDATES
                                    ,@GROUPENDDATES
                                    ,@STATUS
                                    ,@PLAYDAYKINDS
                                    ,@PLAYDAYS
                                    )
                                    ";

            // 2. 📌 包裹在 Try-Catch 區塊中，處理例外狀況
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 加入參數，將值安全地傳遞給 SQL 查詢
                    command.Parameters.AddWithValue("@CREATEDATES", CREATEDATES);
                    command.Parameters.AddWithValue("@SERNO", SERNO);
                    command.Parameters.AddWithValue("@CARCOMPANY", CARCOMPANY);
                    command.Parameters.AddWithValue("@TA008NO", TA008NO);
                    command.Parameters.AddWithValue("@TA008", TA008);
                    command.Parameters.AddWithValue("@CARNO", CARNO);
                    command.Parameters.AddWithValue("@CARNAME", CARNAME);
                    command.Parameters.AddWithValue("@CARKIND", CARKIND);
                    command.Parameters.AddWithValue("@GROUPKIND", GROUPKIND);
                    command.Parameters.AddWithValue("@ISEXCHANGE", ISEXCHANGE);
                    command.Parameters.AddWithValue("@EXCHANGEMONEYS", EXCHANGEMONEYS);
                    command.Parameters.AddWithValue("@EXCHANGETOTALMONEYS", EXCHANGETOTALMONEYS);
                    command.Parameters.AddWithValue("@EXCHANGESALESMMONEYS", EXCHANGESALESMMONEYS);
                    command.Parameters.AddWithValue("@SPECIALMNUMS", SPECIALMNUMS);
                    command.Parameters.AddWithValue("@SPECIALMONEYS", SPECIALMONEYS);
                    command.Parameters.AddWithValue("@SALESMMONEYS", SALESMMONEYS);
                    command.Parameters.AddWithValue("@COMMISSIONBASEMONEYS", COMMISSIONBASEMONEYS);
                    command.Parameters.AddWithValue("@COMMISSIONPCT", COMMISSIONPCT);
                    command.Parameters.AddWithValue("@COMMISSIONPCTMONEYS", COMMISSIONPCTMONEYS);
                    command.Parameters.AddWithValue("@TOTALCOMMISSIONMONEYS", TOTALCOMMISSIONMONEYS);
                    command.Parameters.AddWithValue("@CARNUM", CARNUM);
                    command.Parameters.AddWithValue("@GUSETNUM", GUSETNUM);
                    command.Parameters.AddWithValue("@EXCHANNO", EXCHANNO);
                    command.Parameters.AddWithValue("@EXCHANACOOUNT", EXCHANACOOUNT);
                    command.Parameters.AddWithValue("@PURGROUPSTARTDATES", PURGROUPSTARTDATES);
                    command.Parameters.AddWithValue("@GROUPSTARTDATES", GROUPSTARTDATES);
                    command.Parameters.AddWithValue("@PURGROUPENDDATES", PURGROUPENDDATES);
                    command.Parameters.AddWithValue("@GROUPENDDATES", GROUPENDDATES);
                    command.Parameters.AddWithValue("@STATUS", STATUS);
                    command.Parameters.AddWithValue("@PLAYDAYKINDS", PLAYDAYKINDS);
                    command.Parameters.AddWithValue("@PLAYDAYS", PLAYDAYS);


                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // 檢查是否有資料被更新
                    if (rowsAffected > 0)
                    {
                        MsgBox("完成 \r\n ", this.Page, this);
                    }
                    else
                    {

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

    /// <summary>
    /// 自動編 流水號
    /// </summary>
    /// <param name="CREATEDATES"></param>
    /// <returns></returns>
    public string FINDSERNO(string CREATEDATES)
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);


        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                             SELECT ISNULL(MAX(SERNO),'0') SERNO FROM  [TKMK].[dbo].[GROUPSALES] WHERE CONVERT(NVARCHAR,[CREATEDATES],112)='{0}'
                            ", CREATEDATES);
                //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if(dt!=null &&dt.Rows.Count>=1)
        {
            string SERNO = SETSERNO(dt.Rows[0]["SERNO"].ToString());
            return SERNO;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 格式化 流水號
    /// </summary>
    /// <param name="TEMPSERNO"></param>
    /// <returns></returns>
    public string SETSERNO(string TEMPSERNO)
    {
        if (TEMPSERNO.Equals("0"))
        {
            return "1";
        }

        else
        {
            int serno = Convert.ToInt16(TEMPSERNO);
            serno = serno + 1;
            return serno.ToString();
        }
    }

    /// <summary>
    /// 自動編 流水號
    /// </summary>
    /// <param name="CREATEDATES"></param>
    /// <returns></returns>
    public string SEARCH_POSMU(string MU001)
    {
        // 1.取得連線字串
        // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                             SELECT MU001,SUBSTRING(MU002,1,3) AS MU002 FROM [TK].dbo.POSMU WHERE MU001='{0}' ORDER BY MU001 
                            ", MU001);
        //m_db.AddParameter("@QUERYMONEY", TextBox3.Text.Trim());

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt != null && dt.Rows.Count >= 1)
        {
            string MU002 = dt.Rows[0]["MU002"].ToString().Trim();
            return MU002;
        }
        else
        {
            return null;
        }
    }

    public void UPDATE_GROUPSALES(
        string CREATEDATES, 
        string SERNO,
        string CARNO,
        string CARNAME,
        string EXCHANACOOUNT
        )
    {
        string EXCHANNO = SEARCH_POSMU(EXCHANACOOUNT);

        try
        {
            // 1.取得連線字串
            // 請將 "YourConnectionStringName" 替換為 Web.config 中定義的連線名稱
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ConnectionString;
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            // 1. 📌 使用參數化查詢，避免 SQL Injection
            string sqlQuery = @"
                             UPDATE [TKMK].[dbo].[GROUPSALES]
                             SET CARNO=@CARNO,CARNAME=@CARNAME,EXCHANNO=@EXCHANNO,EXCHANACOOUNT=@EXCHANACOOUNT,TA008NO=@TA008NO,TA008=@TA008
                             WHERE CREATEDATES=@CREATEDATES AND SERNO=@SERNO
                                   
                                    ";

            // 2. 📌 包裹在 Try-Catch 區塊中，處理例外狀況
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 3. 📌 加入參數，將值安全地傳遞給 SQL 查詢
                    command.Parameters.AddWithValue("@CREATEDATES", CREATEDATES);
                    command.Parameters.AddWithValue("@SERNO", SERNO);
                    command.Parameters.AddWithValue("@CARNO", CARNO);
                    command.Parameters.AddWithValue("@CARNAME", CARNAME);
                    command.Parameters.AddWithValue("@EXCHANNO", EXCHANNO);
                    command.Parameters.AddWithValue("@EXCHANACOOUNT", EXCHANACOOUNT);
                    command.Parameters.AddWithValue("@TA008NO", EXCHANNO);
                    command.Parameters.AddWithValue("@TA008", EXCHANACOOUNT);

                

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // 檢查是否有資料被更新
                    if (rowsAffected > 0)
                    {
                        MsgBox("完成 \r\n ", this.Page, this);
                        SEARCHGROUPSALES(CREATEDATES);
                    }
                    else
                    {

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

        //MsgBox(CREATEDATES +" "+ SERNO + " "+CARNO + " "+ CARNAME+" "+ EXCHANNO+" "+ EXCHANACOOUNT, this.Page, this);
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
        // 查詢本日來車資料
        string CREATEDATES = DateTime.Now.ToString("yyyyMMdd");
        SEARCHGROUPSALES(CREATEDATES);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string ID = Guid.NewGuid().ToString();
        string CREATEDATES = DateTime.Now.ToString("yyyyMMdd");
        string SERNO = FINDSERNO(CREATEDATES);
        string CARNO = ADD_TextBox1.Text.Trim();
        string CARNAME = ADD_TextBox2.Text.Trim();
        string CARKIND = "20人以上(大巴)-成人團";
        string GROUPKIND = "社區團體協會";
        string ISEXCHANGE = DropDownList2.Text.Trim();
        string PLAYDAYKINDS = "1日旅遊";
        string PLAYDAYS = "第1天";



        string EXCHANGEMONEYS = "0";
        string EXCHANGETOTALMONEYS = "0";
        string EXCHANGESALESMMONEYS = "0";
        string SALESMMONEYS = "0";
        string SPECIALMNUMS = "0";
        string SPECIALMONEYS = "0";
        string COMMISSIONBASEMONEYS = "0";
        string COMMISSIONPCT = "0";
        string COMMISSIONPCTMONEYS = "0";
        string TOTALCOMMISSIONMONEYS = "0";
        string CARNUM = "1";
        string GUSETNUM = "0";
        string EXCHANNO = SEARCH_POSMU(DropDownList1.Text.Trim().Substring(0, 7).ToString());
        string EXCHANACOOUNT = DropDownList1.Text.Trim().Substring(0, 7).ToString();
        string PURGROUPSTARTDATES = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        string GROUPSTARTDATES = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        string PURGROUPENDDATES = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        string GROUPENDDATES = "1911/1/1";
        string STATUS = "預約接團";
        string TA008 = DropDownList1.Text.Trim().Substring(0, 7).ToString();
        string TA008NO = SEARCH_POSMU(DropDownList1.Text.Trim().Substring(0, 7).ToString());
        string CARCOMPANY = "老楊";

        try
        {
            if (!string.IsNullOrEmpty(SERNO) && !string.IsNullOrEmpty(CARNO) && !string.IsNullOrEmpty(EXCHANNO) && !string.IsNullOrEmpty(EXCHANACOOUNT) && Convert.ToInt32(CARNUM) >= 1)
            {
                ADDGROUPSALES(
                ID
                , CREATEDATES
                , SERNO
                , CARCOMPANY
                , TA008NO
                , TA008
                , CARNO
                , CARNAME
                , CARKIND
                , GROUPKIND
                , ISEXCHANGE
                , EXCHANGEMONEYS
                , EXCHANGETOTALMONEYS
                , EXCHANGESALESMMONEYS
                , SPECIALMNUMS
                , SPECIALMONEYS
                , SALESMMONEYS
                , COMMISSIONBASEMONEYS
                , COMMISSIONPCT
                , COMMISSIONPCTMONEYS
                , TOTALCOMMISSIONMONEYS
                , CARNUM
                , GUSETNUM
                , EXCHANNO
                , EXCHANACOOUNT
                , PURGROUPSTARTDATES
                , GROUPSTARTDATES
                , PURGROUPENDDATES
                , GROUPENDDATES
                , STATUS
                , PLAYDAYKINDS
                , PLAYDAYS
               );               
            }           
        }
        catch (Exception EX)
        {            
        }
        finally
        {
        }

        // 查詢本日來車資料
       
        SEARCHGROUPSALES(CREATEDATES);
    }

    #endregion
}