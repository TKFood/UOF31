using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Telerik.Web.UI;

public partial class CDS_WebPage_COP_TB_SALES_PROMOTIONS : Ede.Uof.Utility.Page.BasePage
{
    string ACCOUNTS = null;
    string NAMES = null;
    string FIRSTTIMES = "N";

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNTS = Current.Account;
        NAMES = Current.User.Name;

        if (!IsPostBack)
        {
            FIRSTTIMES = "Y";

            BindGrid();

            BindDropDownListISCLOSE();
            BindDropDownListKINDS();
            BindDropDownListADDISCLOSED();
            BindDropDownListADDKINIDS();
            BindDropDownList3ISCLOSE();
        }
        else
        {
            FIRSTTIMES = "N";

        }
    }
    #region FUNCTION
    private void BindDropDownListISCLOSE()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_SALES_PROMOTIONS_ISCLOSED'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListISCLOSE.DataSource = dt;
            DropDownListISCLOSE.DataTextField = "NAMES";
            DropDownListISCLOSE.DataValueField = "NAMES";
            DropDownListISCLOSE.DataBind();

        }
        else
        {

        }
    }
    private void BindDropDownListKINDS()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_SALES_PROMOTIONS_KINDS'
                        ORDER BY [VALUE]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListKINDS.DataSource = dt;
            DropDownListKINDS.DataTextField = "NAMES";
            DropDownListKINDS.DataValueField = "NAMES";
            DropDownListKINDS.DataBind();

        }
        else
        {

        }
    }
    public void BindDropDownListADDISCLOSED()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_SALES_PROMOTIONS_ISCLOSED'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListADDISCLOSED.DataSource = dt;
            DropDownListADDISCLOSED.DataTextField = "NAMES";
            DropDownListADDISCLOSED.DataValueField = "NAMES";
            DropDownListADDISCLOSED.DataBind();

        }
        else
        {

        }
    }
    public void BindDropDownListADDKINIDS()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_SALES_PROMOTIONS_KINDS'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownListADDKINIDS.DataSource = dt;
            DropDownListADDKINIDS.DataTextField = "NAMES";
            DropDownListADDKINIDS.DataValueField = "NAMES";
            DropDownListADDKINIDS.DataBind();

        }
        else
        {

        }
    }

    public void BindDropDownList3ISCLOSE()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(String));
        dt.Columns.Add("KIND", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_SALES_PROMOTIONS_ISCLOSED'
                        ORDER BY [ID]
                        ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList3ISCLOSE.DataSource = dt;
            DropDownList3ISCLOSE.DataTextField = "NAMES";
            DropDownList3ISCLOSE.DataValueField = "NAMES";
            DropDownList3ISCLOSE.DataBind();

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

        // 通路名稱
        if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox1.Text))
        {
            QUERYS1.AppendFormat(@" AND [SALESTO] LIKE '%{0}%'", TextBox1.Text.Trim());

        }
        else
        {
            QUERYS1.AppendFormat(@" ");
        }
        // 產品規格
        if (!string.IsNullOrEmpty(TextBox2.Text) && !string.IsNullOrEmpty(TextBox2.Text))
        {
            QUERYS2.AppendFormat(@" AND [PRODUCTS] LIKE '%{0}%'", TextBox2.Text.Trim());

        }
        else
        {
            QUERYS2.AppendFormat(@" ");
        }
        // 是否結案
        string String_DropDownListISCLOSE= DropDownListISCLOSE.SelectedValue.ToString();
        if (!string.IsNullOrEmpty(String_DropDownListISCLOSE))
        {
            if (String_DropDownListISCLOSE.ToString().Equals("全部"))
            {
                QUERYS3.AppendFormat(@"");
            }
            else
            {
                QUERYS3.AppendFormat(@"  AND ID IN ( SELECT [ID] FROM [TKBUSINESS].[dbo].[TB_SALES_PROMOTIONS] WHERE [ISCLOSEED] LIKE '%{0}%' ) ", String_DropDownListISCLOSE);
            }

        }
        else
        {
            QUERYS3.AppendFormat(@"");
        }
        // 活動類型
        string String_DropDownListKINDS = DropDownListKINDS.SelectedValue.ToString();
        if (!string.IsNullOrEmpty(String_DropDownListKINDS))
        {
            if (DropDownListKINDS.SelectedValue.ToString().Equals("全部"))
            {
                QUERYS4.AppendFormat(@"");
            }
            else
            {
                QUERYS4.AppendFormat(@"  AND ID IN ( SELECT [ID] FROM[TKBUSINESS].[dbo].[TB_SALES_PROMOTIONS] WHERE [KINDS] LIKE '%{0}%' ) ", String_DropDownListKINDS);
            }

        }
        else
        {
            QUERYS4.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                            SELECT 
                             [ID]
                            ,[ISCLOSEED]
                            ,[SALESTO]
                            ,[SDATES]
                            ,[PRODUCTS]
                            ,[SHIPDATES]
                            ,[KINDS]
                            ,[CONTEXTS]
                            FROM [TKBUSINESS].[dbo].[TB_SALES_PROMOTIONS]
                            WHERE 1=1 
                            {0}
                            {1}
                            {2}
                            {3}

                            ORDER BY [SDATES]
                        ", QUERYS1.ToString(), QUERYS2.ToString(), QUERYS3.ToString(), QUERYS4.ToString());
       

        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();

        if (FIRSTTIMES.Equals("N"))
        {
            MsgBox("完成 \r\n", this.Page, this);
        }
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
            DropDownList GW1DropDownListISCLOSED = (DropDownList)e.Row.FindControl("GW1DropDownListISCLOSED");

            if (GW1DropDownListISCLOSED != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NAMES", typeof(String));

                string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
                Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

                string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_SALES_PROMOTIONS_ISCLOSED' 
                        AND [NAMES] NOT IN ('全部')
                        ORDER BY [VALUE]
                        ";

                dt.Load(m_db.ExecuteReader(cmdTxt));

                // 在這裡設置DropDownListKINDS的資料來源和其他屬性
                if (dt.Rows.Count > 0)
                {
                    GW1DropDownListISCLOSED.DataSource = dt;
                    GW1DropDownListISCLOSED.DataTextField = "NAMES";
                    GW1DropDownListISCLOSED.DataValueField = "NAMES";
                    GW1DropDownListISCLOSED.DataBind();

                    // 獲取該列對應的資料行中的值
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    string defaultValue = rowView["ISCLOSEED"].ToString(); // 請替換YourDataField為您的資料行名稱

                    // 設定DropDownList的預設值
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        GW1DropDownListISCLOSED.SelectedValue = defaultValue;
                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList GW1DropDownListKINDS = (DropDownList)e.Row.FindControl("GW1DropDownListKINDS");

            if (GW1DropDownListKINDS != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NAMES", typeof(String));

                string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
                Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

                string cmdTxt = @" 
                       SELECT 
                        [ID]
                        ,[KINDS]
                        ,[NAMES]
                        ,[VALUE]
                        FROM [TKBUSINESS].[dbo].[TBPARA]
                        WHERE [KINDS]='TB_SALES_PROMOTIONS_KINDS' 
                        AND [NAMES] NOT IN ('全部')
                        ORDER BY [VALUE]
                        ";

                dt.Load(m_db.ExecuteReader(cmdTxt));

                // 在這裡設置DropDownListKINDS的資料來源和其他屬性
                if (dt.Rows.Count > 0)
                {
                    GW1DropDownListKINDS.DataSource = dt;
                    GW1DropDownListKINDS.DataTextField = "NAMES";
                    GW1DropDownListKINDS.DataValueField = "NAMES";
                    GW1DropDownListKINDS.DataBind();

                    // 獲取該列對應的資料行中的值
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    string defaultValue = rowView["KINDS"].ToString(); // 請替換YourDataField為您的資料行名稱

                    // 設定DropDownList的預設值
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        GW1DropDownListKINDS.SelectedValue = defaultValue;
                    }
                }
            }
        }


    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
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
                GridViewRow row = Grid1.Rows[rowIndex];
                // 獲取相應的ID
                Label LabelID = (Label)row.FindControl("ID");
                string ID = LabelID.Text;

                //是否結案
                DropDownList GW1DropDownListISCLOSED = (DropDownList)row.FindControl("GW1DropDownListISCLOSED");
                string stringGW1DropDownListISCLOSED = GW1DropDownListISCLOSED.Text.Trim();
                //通路
                TextBox txtSALESTO = (TextBox)row.FindControl("通路");
                string stringSALESTO = txtSALESTO.Text.Trim();
                //活動時間
                TextBox txtSDATES = (TextBox)row.FindControl("活動時間");
                string stringSDATES = txtSDATES.Text.Trim();
                //產品規格
                TextBox txtPRODUCTS = (TextBox)row.FindControl("產品規格");
                string stringPRODUCTS = txtPRODUCTS.Text.Trim();
                //出貨日
                TextBox txtSHIPDATES = (TextBox)row.FindControl("出貨日");
                string stringSHIPDATES = txtSHIPDATES.Text.Trim();
                //活動類型
                DropDownList GW1DropDownListKINDS = (DropDownList)row.FindControl("GW1DropDownListKINDS");
                string stringGW1DropDownListKINDS = GW1DropDownListKINDS.Text.Trim();
                //活動內容及價格
                TextBox txtCONTEXTS = (TextBox)row.FindControl("活動內容及價格");
                string stringCONTEXTS = txtCONTEXTS.Text.Trim();

                UPDATE_TB_SALES_PROMOTIONS(
                                          ID,
                                           stringGW1DropDownListISCLOSED,
                                           stringSALESTO,
                                           stringSDATES,
                                           stringPRODUCTS,
                                           stringSHIPDATES,
                                           stringGW1DropDownListKINDS,
                                           stringCONTEXTS
                                            );

            }
        }

    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        
    }

    private void BindGrid3()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS1 = new StringBuilder();
        StringBuilder QUERYS2 = new StringBuilder();
        StringBuilder QUERYS3 = new StringBuilder();
        StringBuilder QUERYS4 = new StringBuilder();

        // 通路名稱
        if (!string.IsNullOrEmpty(TextBox8.Text) && !string.IsNullOrEmpty(TextBox8.Text))
        {
            QUERYS1.AppendFormat(@" AND [SALESTO] LIKE '%{0}%'", TextBox8.Text.Trim());

        }
        else
        {
            QUERYS1.AppendFormat(@" ");
        }
        // 產品規格
        if (!string.IsNullOrEmpty(TextBox9.Text) && !string.IsNullOrEmpty(TextBox9.Text))
        {
            QUERYS2.AppendFormat(@" AND [PRODUCTS] LIKE '%{0}%'", TextBox9.Text.Trim());

        }
        else
        {
            QUERYS2.AppendFormat(@" ");
        }
        // 是否結案
        string String_DropDownListISCLOSE = DropDownList3ISCLOSE.SelectedValue.ToString();
        if (!string.IsNullOrEmpty(String_DropDownListISCLOSE))
        {
            if (String_DropDownListISCLOSE.ToString().Equals("全部"))
            {
                QUERYS3.AppendFormat(@"");
            }
            else
            {
                QUERYS3.AppendFormat(@"  AND ID IN ( SELECT [ID] FROM [TKBUSINESS].[dbo].[TB_SALES_PROMOTIONS] WHERE [ISCLOSEED] LIKE '%{0}%' ) ", String_DropDownListISCLOSE);
            }

        }
        else
        {
            QUERYS3.AppendFormat(@"");
        }
     

        cmdTxt.AppendFormat(@"
                            SELECT 
                             [ID]
                            ,[ISCLOSEED]
                            ,[SALESTO]
                            ,[SDATES]
                            ,[PRODUCTS]
                            ,[SHIPDATES]
                            ,[KINDS]
                            ,[CONTEXTS]
                            FROM [TKBUSINESS].[dbo].[TB_SALES_PROMOTIONS]
                            WHERE 1=1 
                            {0}
                            {1}
                            {2}
                            {3}

                            ORDER BY [SDATES]
                        ", QUERYS1.ToString(), QUERYS2.ToString(), QUERYS3.ToString(), QUERYS4.ToString());


        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid3.DataSource = dt;
        Grid3.DataBind();
               
    }

    protected void grid3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void Grid3_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Grid3Button1")
        {
            // 獲取所選行的索引
            rowIndex = Convert.ToInt32(e.CommandArgument);
            // 在GridView中找到所選行的索引


            // 確保找到了有效的行
            if (rowIndex >= 0)
            {
                GridViewRow row = Grid1.Rows[rowIndex];
                // 獲取相應的ID
                Label LabelID = (Label)row.FindControl("ID");
                string ID = LabelID.Text;

                DELETE_B_SALES_PROMOTIONS(ID);
                BindGrid3();
            }
        }

    }

    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }



    public void UPDATE_TB_SALES_PROMOTIONS(
        string ID,
        string ISCLOSEED,
        string SALESTO,
        string SDATES,
        string PRODUCTS,
        string SHIPDATES,
        string KINDS,
        string CONTEXTS
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";
        cmdTxt = @"
                    UPDATE [TKBUSINESS].[dbo].[TB_SALES_PROMOTIONS]
                    SET 
                    [ISCLOSEED]=@ISCLOSEED
                    ,[SALESTO]=@SALESTO
                    ,[SDATES]=@SDATES
                    ,[PRODUCTS]=@PRODUCTS
                    ,[SHIPDATES]=@SHIPDATES
                    ,[KINDS]=@KINDS
                    ,[CONTEXTS]=@CONTEXTS
                    WHERE [ID]=@ID
                        ";



        m_db.AddParameter("@ID", ID);
        m_db.AddParameter("@ISCLOSEED", ISCLOSEED);
        m_db.AddParameter("@SALESTO", SALESTO);
        m_db.AddParameter("@SDATES", SDATES);
        m_db.AddParameter("@PRODUCTS", PRODUCTS);
        m_db.AddParameter("@SHIPDATES", SHIPDATES);
        m_db.AddParameter("@KINDS", KINDS);
        m_db.AddParameter("@CONTEXTS", CONTEXTS);

        m_db.ExecuteNonQuery(cmdTxt);

        MsgBox("成功 \r\n", this.Page, this);
    }

    public void INSERT_TB_SALES_PROMOTIONS(
        string ISCLOSEED,
        string SALESTO,
        string SDATES,
        string PRODUCTS,
        string SHIPDATES,
        string KINDS,
        string CONTEXTS
        )
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";
        cmdTxt = @"                    
                INSERT INTO [TKBUSINESS].[dbo].[TB_SALES_PROMOTIONS]
                (
                [ISCLOSEED]
                ,[SALESTO]
                ,[SDATES]
                ,[PRODUCTS]
                ,[SHIPDATES]
                ,[KINDS]
                ,[CONTEXTS]
                )
                VALUES
                (
                @ISCLOSEED
                ,@SALESTO
                ,@SDATES
                ,@PRODUCTS
                ,@SHIPDATES
                ,@KINDS
                ,@CONTEXTS
                )
                        ";



      
        m_db.AddParameter("@ISCLOSEED", ISCLOSEED);
        m_db.AddParameter("@SALESTO", SALESTO);
        m_db.AddParameter("@SDATES", SDATES);
        m_db.AddParameter("@PRODUCTS", PRODUCTS);
        m_db.AddParameter("@SHIPDATES", SHIPDATES);
        m_db.AddParameter("@KINDS", KINDS);
        m_db.AddParameter("@CONTEXTS", CONTEXTS);

        m_db.ExecuteNonQuery(cmdTxt);

        MsgBox("成功 \r\n", this.Page, this);
    }

    public void DELETE_B_SALES_PROMOTIONS(string ID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";
        cmdTxt = @"                    
                DELETE [TKBUSINESS].[dbo].[TB_SALES_PROMOTIONS]
                WHERE ID=@ID
                        ";




        m_db.AddParameter("@ID", ID);
      

        m_db.ExecuteNonQuery(cmdTxt);

        MsgBox("成功 \r\n", this.Page, this);
    }

    public void MsgBox(string ex, Page pg, object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);
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
       
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindGrid();        

    }

    protected void btn2_Click(object sender, EventArgs e)
    {     
        string ISCLOSEED = DropDownListADDISCLOSED.SelectedValue.ToString();
        string SALESTO = TextBox3.Text.ToString();
        string SDATES = TextBox4.Text.ToString();
        string PRODUCTS = TextBox5.Text.ToString();
        string SHIPDATES = TextBox6.Text.ToString();
        string KINDS = DropDownListADDKINIDS.SelectedValue.ToString();
        string CONTEXTS = TextBox7.Text.ToString();

        INSERT_TB_SALES_PROMOTIONS(
                                    ISCLOSEED,
                                    SALESTO,
                                    SDATES,
                                    PRODUCTS,
                                    SHIPDATES,
                                    KINDS,
                                    CONTEXTS
                                    );

        RadTab tab1 = RadTabStrip2.Tabs.FindTabByText("通路");
        tab1.Selected = true;
        RadPageView1.Selected = true;
        BindGrid();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        BindGrid3();

    }
    #endregion
}